// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MyGameInventoryItem
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.GameServices
{
  [Serializable]
  public class MyGameInventoryItem
  {
    public ulong ID { get; set; }

    public MyGameInventoryItemDefinition ItemDefinition { get; set; }

    public ushort Quantity { get; set; }

    public HashSet<long> UsingCharacters { get; set; } = new HashSet<long>();

    public bool IsStoreFakeItem { get; set; }

    public bool IsNew { get; set; }

    protected class VRage_GameServices_MyGameInventoryItem\u003C\u003EID\u003C\u003EAccessor : IMemberAccessor<MyGameInventoryItem, ulong>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyGameInventoryItem owner, in ulong value) => owner.ID = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyGameInventoryItem owner, out ulong value) => value = owner.ID;
    }

    protected class VRage_GameServices_MyGameInventoryItem\u003C\u003EItemDefinition\u003C\u003EAccessor : IMemberAccessor<MyGameInventoryItem, MyGameInventoryItemDefinition>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyGameInventoryItem owner, in MyGameInventoryItemDefinition value) => owner.ItemDefinition = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyGameInventoryItem owner,
        out MyGameInventoryItemDefinition value)
      {
        value = owner.ItemDefinition;
      }
    }

    protected class VRage_GameServices_MyGameInventoryItem\u003C\u003EQuantity\u003C\u003EAccessor : IMemberAccessor<MyGameInventoryItem, ushort>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyGameInventoryItem owner, in ushort value) => owner.Quantity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyGameInventoryItem owner, out ushort value) => value = owner.Quantity;
    }

    protected class VRage_GameServices_MyGameInventoryItem\u003C\u003EUsingCharacters\u003C\u003EAccessor : IMemberAccessor<MyGameInventoryItem, HashSet<long>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyGameInventoryItem owner, in HashSet<long> value) => owner.UsingCharacters = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyGameInventoryItem owner, out HashSet<long> value) => value = owner.UsingCharacters;
    }

    protected class VRage_GameServices_MyGameInventoryItem\u003C\u003EIsStoreFakeItem\u003C\u003EAccessor : IMemberAccessor<MyGameInventoryItem, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyGameInventoryItem owner, in bool value) => owner.IsStoreFakeItem = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyGameInventoryItem owner, out bool value) => value = owner.IsStoreFakeItem;
    }

    protected class VRage_GameServices_MyGameInventoryItem\u003C\u003EIsNew\u003C\u003EAccessor : IMemberAccessor<MyGameInventoryItem, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyGameInventoryItem owner, in bool value) => owner.IsNew = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyGameInventoryItem owner, out bool value) => value = owner.IsNew;
    }
  }
}
