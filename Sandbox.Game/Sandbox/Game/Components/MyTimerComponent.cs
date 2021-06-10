// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyTimerComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.EntityComponents;
using Sandbox.Game.EntityComponents.Systems;
using Sandbox.Game.Multiplayer;
using System;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Game.Components
{
  [MyComponentType(typeof (MyTimerComponent))]
  [MyComponentBuilder(typeof (MyObjectBuilder_TimerComponent), true)]
  public class MyTimerComponent : MyEntityComponentBase
  {
    private bool m_forceTrigger;

    public bool TimerEnabled { get; private set; } = true;

    public bool RemoveEntityOnTimer { get; set; }

    public bool Repeat { get; set; }

    [Obsolete("Use FramesFromLastTrigger")]
    public float TimeToEvent { get; set; }

    public MyTimerTypes TimerType { get; private set; }

    public Action<MyEntityComponentContainer> EventToTrigger { get; set; }

    [Obsolete("Use TimerTickInFrames")]
    public float TimerTick { get; private set; }

    public uint FramesFromLastTrigger { get; set; }

    public uint TimerTickInFrames { get; private set; }

    public bool IsSessionUpdateEnabled { get; set; } = true;

    public override string ComponentTypeDebugString => "Timer";

    public void SetRemoveEntityTimer(float timeMin)
    {
      this.RemoveEntityOnTimer = true;
      this.SetTimer(timeMin, MyTimerComponent.GetRemoveEntityOnTimerEvent());
    }

    public void SetTimer(
      float timeMin,
      Action<MyEntityComponentContainer> triggerEvent,
      bool start = true,
      bool repeat = false)
    {
      this.SetTimer((uint) ((double) timeMin * 60.0 * 60.0), triggerEvent, start, repeat);
    }

    public void SetTimer(
      uint timeTickInFrames,
      Action<MyEntityComponentContainer> triggerEvent,
      bool start = true,
      bool repeat = false)
    {
      this.TimerTickInFrames = timeTickInFrames;
      this.Repeat = repeat;
      this.EventToTrigger = triggerEvent;
      this.TimerEnabled = start;
      this.m_forceTrigger = start;
    }

    public void ChangeTimerTick(uint timeTickInFrames) => this.TimerTickInFrames = timeTickInFrames;

    public void SetType(MyTimerTypes type)
    {
      int num = MyTimerComponentSystem.Static.IsRegisteredAny(this) ? 1 : 0;
      if (num != 0)
        MyTimerComponentSystem.Static.Unregister(this);
      this.TimerType = type;
      if (num == 0)
        return;
      MyTimerComponentSystem.Static.Register(this);
    }

    public void ClearEvent() => this.EventToTrigger = (Action<MyEntityComponentContainer>) null;

    public void Pause() => this.TimerEnabled = false;

    public void Resume(bool forceTrigger = false)
    {
      this.TimerEnabled = true;
      this.m_forceTrigger = forceTrigger;
    }

    public void Update(bool forceUpdate = false)
    {
      if (!this.TimerEnabled && !forceUpdate)
        return;
      switch (this.TimerType)
      {
        case MyTimerTypes.Frame10:
          this.FramesFromLastTrigger += 10U;
          break;
        case MyTimerTypes.Frame100:
          this.FramesFromLastTrigger += 100U;
          break;
      }
      if (this.m_forceTrigger | forceUpdate)
      {
        this.m_forceTrigger = false;
        this.FramesFromLastTrigger = this.TimerTickInFrames;
      }
      if (this.FramesFromLastTrigger < this.TimerTickInFrames)
        return;
      this.EventToTrigger.InvokeIfNotNull<MyEntityComponentContainer>(this.Container);
      if (this.Repeat)
        this.FramesFromLastTrigger = 0U;
      else
        this.TimerEnabled = false;
    }

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      if ((double) this.TimerTick != 0.0)
        this.TimerTickInFrames = (uint) ((double) this.TimerTick * 3600.0);
      if ((double) this.TimeToEvent != 0.0)
        this.FramesFromLastTrigger = (uint) (((double) this.TimerTick - (double) this.TimeToEvent) * 3600.0);
      MyTimerComponentSystem.Static.Register(this);
    }

    public override void OnBeforeRemovedFromContainer()
    {
      base.OnBeforeRemovedFromContainer();
      if (MyTimerComponentSystem.Static == null)
        return;
      MyTimerComponentSystem.Static.Unregister(this);
    }

    public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
    {
      MyObjectBuilder_TimerComponent objectBuilder = MyComponentFactory.CreateObjectBuilder((MyComponentBase) this) as MyObjectBuilder_TimerComponent;
      objectBuilder.Repeat = this.Repeat;
      objectBuilder.TimeToEvent = 0.0f;
      objectBuilder.SetTimeMinutes = 0.0f;
      objectBuilder.TimerEnabled = this.TimerEnabled;
      objectBuilder.RemoveEntityOnTimer = this.RemoveEntityOnTimer;
      objectBuilder.TimerType = this.TimerType;
      objectBuilder.FramesFromLastTrigger = this.FramesFromLastTrigger;
      objectBuilder.TimerTickInFrames = this.TimerTickInFrames;
      objectBuilder.IsSessionUpdateEnabled = this.IsSessionUpdateEnabled;
      return (MyObjectBuilder_ComponentBase) objectBuilder;
    }

    public override void Deserialize(MyObjectBuilder_ComponentBase baseBuilder)
    {
      MyObjectBuilder_TimerComponent builderTimerComponent = baseBuilder as MyObjectBuilder_TimerComponent;
      this.Repeat = builderTimerComponent.Repeat;
      this.TimeToEvent = builderTimerComponent.TimeToEvent;
      this.TimerTick = builderTimerComponent.SetTimeMinutes;
      this.TimerEnabled = builderTimerComponent.TimerEnabled;
      this.RemoveEntityOnTimer = builderTimerComponent.RemoveEntityOnTimer;
      this.TimerType = builderTimerComponent.TimerType;
      this.FramesFromLastTrigger = builderTimerComponent.FramesFromLastTrigger;
      this.TimerTickInFrames = builderTimerComponent.TimerTickInFrames;
      this.IsSessionUpdateEnabled = builderTimerComponent.IsSessionUpdateEnabled;
      if (!this.RemoveEntityOnTimer || !Sync.IsServer)
        return;
      this.EventToTrigger = MyTimerComponent.GetRemoveEntityOnTimerEvent();
    }

    public override bool IsSerialized() => true;

    public override void Init(MyComponentDefinitionBase definition)
    {
      base.Init(definition);
      if (!(definition is MyTimerComponentDefinition componentDefinition))
        return;
      this.TimerEnabled = (double) componentDefinition.TimeToRemoveMin > 0.0;
      this.TimerTickInFrames = (uint) ((double) componentDefinition.TimeToRemoveMin * 3600.0);
      this.FramesFromLastTrigger = 0U;
      this.RemoveEntityOnTimer = (double) componentDefinition.TimeToRemoveMin > 0.0;
      if (!this.RemoveEntityOnTimer || !Sync.IsServer)
        return;
      this.EventToTrigger = MyTimerComponent.GetRemoveEntityOnTimerEvent();
    }

    private static Action<MyEntityComponentContainer> GetRemoveEntityOnTimerEvent() => (Action<MyEntityComponentContainer>) (container =>
    {
      MyLog.Default.Info(string.Format("MyTimerComponent removed entity '{0}:{1}' with entity id '{2}'", (object) container.Entity.Name, (object) container.Entity.DisplayName, (object) container.Entity.EntityId));
      if (container.Entity.MarkedForClose)
        return;
      container.Entity.Close();
    });

    private class Sandbox_Game_Components_MyTimerComponent\u003C\u003EActor : IActivator, IActivator<MyTimerComponent>
    {
      object IActivator.CreateInstance() => (object) new MyTimerComponent();

      MyTimerComponent IActivator<MyTimerComponent>.CreateInstance() => new MyTimerComponent();
    }
  }
}
