// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.PerFrameData
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.ObjectBuilders;

namespace VRage.Game.ObjectBuilders.Components
{
  [ProtoContract]
  public struct PerFrameData
  {
    [ProtoMember(64)]
    public VRage.Game.ObjectBuilders.Components.MovementData? MovementData;
    [ProtoMember(67)]
    public VRage.Game.ObjectBuilders.Components.SwitchWeaponData? SwitchWeaponData;
    [ProtoMember(70)]
    public VRage.Game.ObjectBuilders.Components.ShootData? ShootData;
    [ProtoMember(73)]
    public VRage.Game.ObjectBuilders.Components.AnimationData? AnimationData;
    [ProtoMember(76)]
    public VRage.Game.ObjectBuilders.Components.ControlSwitchesData? ControlSwitchesData;
    [ProtoMember(79)]
    public VRage.Game.ObjectBuilders.Components.UseData? UseData;

    public bool ShouldSerializeSwitchWeaponData()
    {
      ref VRage.Game.ObjectBuilders.Components.SwitchWeaponData? local1 = ref this.SwitchWeaponData;
      string str;
      if (!local1.HasValue)
      {
        str = (string) null;
      }
      else
      {
        ref SerializableDefinitionId? local2 = ref local1.GetValueOrDefault().WeaponDefinition;
        str = local2.HasValue ? local2.GetValueOrDefault().TypeIdString : (string) null;
      }
      if (!MyObjectBuilderType.IsValidTypeName(str))
      {
        if (!this.SwitchWeaponData.HasValue)
          return false;
        VRage.Game.ObjectBuilders.Components.SwitchWeaponData switchWeaponData = this.SwitchWeaponData.Value;
        switchWeaponData.WeaponDefinition = new SerializableDefinitionId?();
        this.SwitchWeaponData = new VRage.Game.ObjectBuilders.Components.SwitchWeaponData?(switchWeaponData);
      }
      return true;
    }

    public override string ToString()
    {
      if (!this.MovementData.HasValue)
        return base.ToString();
      return this.MovementData.Value.MoveVector.ToString() + "\n" + this.MovementData.Value.RotateVector.ToString() + "\n" + this.MovementData.Value.MovementFlags.ToString();
    }

    protected class VRage_Game_ObjectBuilders_Components_PerFrameData\u003C\u003EMovementData\u003C\u003EAccessor : IMemberAccessor<PerFrameData, VRage.Game.ObjectBuilders.Components.MovementData?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerFrameData owner, in VRage.Game.ObjectBuilders.Components.MovementData? value) => owner.MovementData = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerFrameData owner, out VRage.Game.ObjectBuilders.Components.MovementData? value) => value = owner.MovementData;
    }

    protected class VRage_Game_ObjectBuilders_Components_PerFrameData\u003C\u003ESwitchWeaponData\u003C\u003EAccessor : IMemberAccessor<PerFrameData, VRage.Game.ObjectBuilders.Components.SwitchWeaponData?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerFrameData owner, in VRage.Game.ObjectBuilders.Components.SwitchWeaponData? value) => owner.SwitchWeaponData = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerFrameData owner, out VRage.Game.ObjectBuilders.Components.SwitchWeaponData? value) => value = owner.SwitchWeaponData;
    }

    protected class VRage_Game_ObjectBuilders_Components_PerFrameData\u003C\u003EShootData\u003C\u003EAccessor : IMemberAccessor<PerFrameData, VRage.Game.ObjectBuilders.Components.ShootData?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerFrameData owner, in VRage.Game.ObjectBuilders.Components.ShootData? value) => owner.ShootData = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerFrameData owner, out VRage.Game.ObjectBuilders.Components.ShootData? value) => value = owner.ShootData;
    }

    protected class VRage_Game_ObjectBuilders_Components_PerFrameData\u003C\u003EAnimationData\u003C\u003EAccessor : IMemberAccessor<PerFrameData, VRage.Game.ObjectBuilders.Components.AnimationData?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerFrameData owner, in VRage.Game.ObjectBuilders.Components.AnimationData? value) => owner.AnimationData = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerFrameData owner, out VRage.Game.ObjectBuilders.Components.AnimationData? value) => value = owner.AnimationData;
    }

    protected class VRage_Game_ObjectBuilders_Components_PerFrameData\u003C\u003EControlSwitchesData\u003C\u003EAccessor : IMemberAccessor<PerFrameData, VRage.Game.ObjectBuilders.Components.ControlSwitchesData?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerFrameData owner, in VRage.Game.ObjectBuilders.Components.ControlSwitchesData? value) => owner.ControlSwitchesData = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerFrameData owner, out VRage.Game.ObjectBuilders.Components.ControlSwitchesData? value) => value = owner.ControlSwitchesData;
    }

    protected class VRage_Game_ObjectBuilders_Components_PerFrameData\u003C\u003EUseData\u003C\u003EAccessor : IMemberAccessor<PerFrameData, VRage.Game.ObjectBuilders.Components.UseData?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerFrameData owner, in VRage.Game.ObjectBuilders.Components.UseData? value) => owner.UseData = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerFrameData owner, out VRage.Game.ObjectBuilders.Components.UseData? value) => value = owner.UseData;
    }

    private class VRage_Game_ObjectBuilders_Components_PerFrameData\u003C\u003EActor : IActivator, IActivator<PerFrameData>
    {
      object IActivator.CreateInstance() => (object) new PerFrameData();

      PerFrameData IActivator<PerFrameData>.CreateInstance() => new PerFrameData();
    }
  }
}
