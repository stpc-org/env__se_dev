// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.SwitchWeaponData
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
  public struct SwitchWeaponData
  {
    [ProtoMember(19)]
    public SerializableDefinitionId? WeaponDefinition;
    [ProtoMember(22)]
    public uint? InventoryItemId;
    [ProtoMember(25)]
    public long WeaponEntityId;

    protected class VRage_Game_ObjectBuilders_Components_SwitchWeaponData\u003C\u003EWeaponDefinition\u003C\u003EAccessor : IMemberAccessor<SwitchWeaponData, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SwitchWeaponData owner, in SerializableDefinitionId? value) => owner.WeaponDefinition = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SwitchWeaponData owner, out SerializableDefinitionId? value) => value = owner.WeaponDefinition;
    }

    protected class VRage_Game_ObjectBuilders_Components_SwitchWeaponData\u003C\u003EInventoryItemId\u003C\u003EAccessor : IMemberAccessor<SwitchWeaponData, uint?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SwitchWeaponData owner, in uint? value) => owner.InventoryItemId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SwitchWeaponData owner, out uint? value) => value = owner.InventoryItemId;
    }

    protected class VRage_Game_ObjectBuilders_Components_SwitchWeaponData\u003C\u003EWeaponEntityId\u003C\u003EAccessor : IMemberAccessor<SwitchWeaponData, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SwitchWeaponData owner, in long value) => owner.WeaponEntityId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SwitchWeaponData owner, out long value) => value = owner.WeaponEntityId;
    }

    private class VRage_Game_ObjectBuilders_Components_SwitchWeaponData\u003C\u003EActor : IActivator, IActivator<SwitchWeaponData>
    {
      object IActivator.CreateInstance() => (object) new SwitchWeaponData();

      SwitchWeaponData IActivator<SwitchWeaponData>.CreateInstance() => new SwitchWeaponData();
    }
  }
}
