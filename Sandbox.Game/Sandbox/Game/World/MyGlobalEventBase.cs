// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MyGlobalEventBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.World
{
  [MyEventType(typeof (MyObjectBuilder_GlobalEventBase), true)]
  [MyEventType(typeof (MyObjectBuilder_GlobalEventDefinition), false)]
  public class MyGlobalEventBase : IComparable
  {
    public bool IsOneTime => !this.Definition.MinActivationTime.HasValue;

    public bool IsPeriodic => !this.IsOneTime;

    public bool IsInPast => this.ActivationTime.Ticks <= 0L;

    public bool IsInFuture => this.ActivationTime.Ticks > 0L;

    public bool IsHandlerValid => this.Action != (MethodInfo) null;

    public MyGlobalEventDefinition Definition { private set; get; }

    public MethodInfo Action { private set; get; }

    public TimeSpan ActivationTime { private set; get; }

    public bool Enabled { get; set; }

    public bool RemoveAfterHandlerExit { get; set; }

    public virtual void InitFromDefinition(MyGlobalEventDefinition definition)
    {
      this.Definition = definition;
      this.Action = MyGlobalEventFactory.GetEventHandler(this.Definition.Id);
      if (this.Definition.FirstActivationTime.HasValue)
        this.ActivationTime = this.Definition.FirstActivationTime.Value;
      else
        this.RecalculateActivationTime();
      this.Enabled = true;
      this.RemoveAfterHandlerExit = false;
    }

    public virtual void Init(MyObjectBuilder_GlobalEventBase ob)
    {
      this.Definition = MyDefinitionManager.Static.GetEventDefinition(ob.GetId());
      this.Action = MyGlobalEventFactory.GetEventHandler(ob.GetId());
      this.ActivationTime = TimeSpan.FromMilliseconds((double) ob.ActivationTimeMs);
      this.Enabled = ob.Enabled;
      this.RemoveAfterHandlerExit = false;
    }

    public virtual MyObjectBuilder_GlobalEventBase GetObjectBuilder()
    {
      MyObjectBuilder_GlobalEventBase newObject = MyObjectBuilderSerializer.CreateNewObject(this.Definition.Id.TypeId, this.Definition.Id.SubtypeName) as MyObjectBuilder_GlobalEventBase;
      newObject.ActivationTimeMs = this.ActivationTime.Ticks / 10000L;
      newObject.Enabled = this.Enabled;
      return newObject;
    }

    public void RecalculateActivationTime()
    {
      TimeSpan? minActivationTime = this.Definition.MinActivationTime;
      TimeSpan? maxActivationTime = this.Definition.MaxActivationTime;
      this.ActivationTime = (minActivationTime.HasValue == maxActivationTime.HasValue ? (minActivationTime.HasValue ? (minActivationTime.GetValueOrDefault() == maxActivationTime.GetValueOrDefault() ? 1 : 0) : 1) : 0) == 0 ? MyUtils.GetRandomTimeSpan(this.Definition.MinActivationTime.Value, this.Definition.MaxActivationTime.Value) : this.Definition.MinActivationTime.Value;
      MySandboxGame.Log.WriteLine("MyGlobalEvent.RecalculateActivationTime:");
      MySandboxGame.Log.WriteLine("Next activation in " + this.ActivationTime.ToString());
    }

    public void SetActivationTime(TimeSpan time) => this.ActivationTime = time;

    public int CompareTo(object obj)
    {
      if (!(obj is MyGlobalEventBase))
        return 0;
      TimeSpan timeSpan = this.ActivationTime - (obj as MyGlobalEventBase).ActivationTime;
      if (timeSpan.Ticks == 0L)
        return RuntimeHelpers.GetHashCode((object) this) - RuntimeHelpers.GetHashCode(obj);
      return timeSpan.Ticks >= 0L ? 1 : -1;
    }
  }
}
