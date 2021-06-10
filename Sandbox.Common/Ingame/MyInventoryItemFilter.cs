// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.MyInventoryItemFilter
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using System.Runtime.CompilerServices;
using VRage.Game;
using VRage.Game.ModAPI.Ingame;
using VRage.Network;

namespace Sandbox.ModAPI.Ingame
{
  [Serializable]
  public struct MyInventoryItemFilter
  {
    public readonly bool AllSubTypes;
    public readonly MyDefinitionId ItemId;

    public static implicit operator MyInventoryItemFilter(MyItemType itemType) => new MyInventoryItemFilter((MyDefinitionId) itemType);

    public static implicit operator MyInventoryItemFilter(
      MyDefinitionId definitionId)
    {
      return new MyInventoryItemFilter(definitionId);
    }

    public MyItemType ItemType => (MyItemType) this.ItemId;

    public MyInventoryItemFilter(string itemId, bool allSubTypes = false)
      : this()
    {
      this.ItemId = MyDefinitionId.Parse(itemId);
      this.AllSubTypes = allSubTypes;
    }

    public MyInventoryItemFilter(MyDefinitionId itemId, bool allSubTypes = false)
      : this()
    {
      this.ItemId = itemId;
      this.AllSubTypes = allSubTypes;
    }

    protected class Sandbox_ModAPI_Ingame_MyInventoryItemFilter\u003C\u003EAllSubTypes\u003C\u003EAccessor : IMemberAccessor<MyInventoryItemFilter, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyInventoryItemFilter owner, in bool value) => owner.AllSubTypes = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyInventoryItemFilter owner, out bool value) => value = owner.AllSubTypes;
    }

    protected class Sandbox_ModAPI_Ingame_MyInventoryItemFilter\u003C\u003EItemId\u003C\u003EAccessor : IMemberAccessor<MyInventoryItemFilter, MyDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyInventoryItemFilter owner, in MyDefinitionId value) => owner.ItemId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyInventoryItemFilter owner, out MyDefinitionId value) => value = owner.ItemId;
    }
  }
}
