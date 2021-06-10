// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.ComponentSystem.MyObjectBuilder_TimerComponent
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.ComponentSystem
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_TimerComponent : MyObjectBuilder_ComponentBase
  {
    [ProtoMember(1)]
    public bool Repeat;
    [ProtoMember(4)]
    public float TimeToEvent;
    [ProtoMember(7)]
    public float SetTimeMinutes;
    [ProtoMember(10)]
    public bool TimerEnabled;
    [ProtoMember(13)]
    public bool RemoveEntityOnTimer;
    [ProtoMember(15)]
    public MyTimerTypes TimerType = MyTimerTypes.Frame100;

    [ProtoMember(17)]
    public uint FramesFromLastTrigger { get; set; }

    [ProtoMember(19)]
    public uint TimerTickInFrames { get; set; }

    [ProtoMember(21)]
    public bool IsSessionUpdateEnabled { get; set; } = true;

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TimerComponent\u003C\u003ERepeat\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TimerComponent, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TimerComponent owner, in bool value) => owner.Repeat = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TimerComponent owner, out bool value) => value = owner.Repeat;
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TimerComponent\u003C\u003ETimeToEvent\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TimerComponent, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TimerComponent owner, in float value) => owner.TimeToEvent = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TimerComponent owner, out float value) => value = owner.TimeToEvent;
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TimerComponent\u003C\u003ESetTimeMinutes\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TimerComponent, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TimerComponent owner, in float value) => owner.SetTimeMinutes = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TimerComponent owner, out float value) => value = owner.SetTimeMinutes;
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TimerComponent\u003C\u003ETimerEnabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TimerComponent, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TimerComponent owner, in bool value) => owner.TimerEnabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TimerComponent owner, out bool value) => value = owner.TimerEnabled;
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TimerComponent\u003C\u003ERemoveEntityOnTimer\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TimerComponent, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TimerComponent owner, in bool value) => owner.RemoveEntityOnTimer = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TimerComponent owner, out bool value) => value = owner.RemoveEntityOnTimer;
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TimerComponent\u003C\u003ETimerType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TimerComponent, MyTimerTypes>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TimerComponent owner, in MyTimerTypes value) => owner.TimerType = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TimerComponent owner, out MyTimerTypes value) => value = owner.TimerType;
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TimerComponent\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TimerComponent, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TimerComponent owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TimerComponent owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TimerComponent\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TimerComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TimerComponent owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TimerComponent owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TimerComponent\u003C\u003EFramesFromLastTrigger\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TimerComponent, uint>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TimerComponent owner, in uint value) => owner.FramesFromLastTrigger = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TimerComponent owner, out uint value) => value = owner.FramesFromLastTrigger;
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TimerComponent\u003C\u003ETimerTickInFrames\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TimerComponent, uint>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TimerComponent owner, in uint value) => owner.TimerTickInFrames = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TimerComponent owner, out uint value) => value = owner.TimerTickInFrames;
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TimerComponent\u003C\u003EIsSessionUpdateEnabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TimerComponent, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TimerComponent owner, in bool value) => owner.IsSessionUpdateEnabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TimerComponent owner, out bool value) => value = owner.IsSessionUpdateEnabled;
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TimerComponent\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TimerComponent, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TimerComponent owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TimerComponent owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TimerComponent\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TimerComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TimerComponent owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TimerComponent owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TimerComponent\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_TimerComponent>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_TimerComponent();

      MyObjectBuilder_TimerComponent IActivator<MyObjectBuilder_TimerComponent>.CreateInstance() => new MyObjectBuilder_TimerComponent();
    }
  }
}
