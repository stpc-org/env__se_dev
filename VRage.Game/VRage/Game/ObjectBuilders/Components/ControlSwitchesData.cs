// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.ControlSwitchesData
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game.ObjectBuilders.Components
{
  [ProtoContract]
  public struct ControlSwitchesData
  {
    [ProtoMember(37)]
    public bool SwitchThrusts;
    [ProtoMember(40)]
    public bool SwitchDamping;
    [ProtoMember(43)]
    public bool SwitchLights;
    [ProtoMember(46)]
    public bool SwitchLandingGears;
    [ProtoMember(49)]
    public bool SwitchReactors;
    [ProtoMember(52)]
    public bool SwitchHelmet;

    protected class VRage_Game_ObjectBuilders_Components_ControlSwitchesData\u003C\u003ESwitchThrusts\u003C\u003EAccessor : IMemberAccessor<ControlSwitchesData, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ControlSwitchesData owner, in bool value) => owner.SwitchThrusts = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ControlSwitchesData owner, out bool value) => value = owner.SwitchThrusts;
    }

    protected class VRage_Game_ObjectBuilders_Components_ControlSwitchesData\u003C\u003ESwitchDamping\u003C\u003EAccessor : IMemberAccessor<ControlSwitchesData, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ControlSwitchesData owner, in bool value) => owner.SwitchDamping = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ControlSwitchesData owner, out bool value) => value = owner.SwitchDamping;
    }

    protected class VRage_Game_ObjectBuilders_Components_ControlSwitchesData\u003C\u003ESwitchLights\u003C\u003EAccessor : IMemberAccessor<ControlSwitchesData, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ControlSwitchesData owner, in bool value) => owner.SwitchLights = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ControlSwitchesData owner, out bool value) => value = owner.SwitchLights;
    }

    protected class VRage_Game_ObjectBuilders_Components_ControlSwitchesData\u003C\u003ESwitchLandingGears\u003C\u003EAccessor : IMemberAccessor<ControlSwitchesData, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ControlSwitchesData owner, in bool value) => owner.SwitchLandingGears = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ControlSwitchesData owner, out bool value) => value = owner.SwitchLandingGears;
    }

    protected class VRage_Game_ObjectBuilders_Components_ControlSwitchesData\u003C\u003ESwitchReactors\u003C\u003EAccessor : IMemberAccessor<ControlSwitchesData, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ControlSwitchesData owner, in bool value) => owner.SwitchReactors = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ControlSwitchesData owner, out bool value) => value = owner.SwitchReactors;
    }

    protected class VRage_Game_ObjectBuilders_Components_ControlSwitchesData\u003C\u003ESwitchHelmet\u003C\u003EAccessor : IMemberAccessor<ControlSwitchesData, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ControlSwitchesData owner, in bool value) => owner.SwitchHelmet = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ControlSwitchesData owner, out bool value) => value = owner.SwitchHelmet;
    }

    private class VRage_Game_ObjectBuilders_Components_ControlSwitchesData\u003C\u003EActor : IActivator, IActivator<ControlSwitchesData>
    {
      object IActivator.CreateInstance() => (object) new ControlSwitchesData();

      ControlSwitchesData IActivator<ControlSwitchesData>.CreateInstance() => new ControlSwitchesData();
    }
  }
}
