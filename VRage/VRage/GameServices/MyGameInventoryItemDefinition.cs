// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MyGameInventoryItemDefinition
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.GameServices
{
  [Serializable]
  public class MyGameInventoryItemDefinition
  {
    public int ID { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string DisplayType { get; set; }

    public string IconTexture { get; set; }

    public string AssetModifierId { get; set; }

    public MyGameInventoryItemSlot ItemSlot { get; set; }

    public string ToolName { get; set; }

    public string NameColor { get; set; }

    public string BackgroundColor { get; set; }

    public MyGameInventoryItemDefinitionType DefinitionType { get; set; }

    public bool Hidden { get; set; }

    public bool IsStoreHidden { get; set; }

    public bool CanBePurchased { get; set; }

    public MyGameInventoryItemQuality ItemQuality { get; set; }

    public string Exchange { get; set; }

    public override string ToString() => string.Format("({0}) {1}", (object) this.ID, (object) this.Name);

    protected class VRage_GameServices_MyGameInventoryItemDefinition\u003C\u003EID\u003C\u003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyGameInventoryItemDefinition owner, in int value) => owner.ID = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyGameInventoryItemDefinition owner, out int value) => value = owner.ID;
    }

    protected class VRage_GameServices_MyGameInventoryItemDefinition\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyGameInventoryItemDefinition owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyGameInventoryItemDefinition owner, out string value) => value = owner.Name;
    }

    protected class VRage_GameServices_MyGameInventoryItemDefinition\u003C\u003EDescription\u003C\u003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyGameInventoryItemDefinition owner, in string value) => owner.Description = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyGameInventoryItemDefinition owner, out string value) => value = owner.Description;
    }

    protected class VRage_GameServices_MyGameInventoryItemDefinition\u003C\u003EDisplayType\u003C\u003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyGameInventoryItemDefinition owner, in string value) => owner.DisplayType = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyGameInventoryItemDefinition owner, out string value) => value = owner.DisplayType;
    }

    protected class VRage_GameServices_MyGameInventoryItemDefinition\u003C\u003EIconTexture\u003C\u003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyGameInventoryItemDefinition owner, in string value) => owner.IconTexture = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyGameInventoryItemDefinition owner, out string value) => value = owner.IconTexture;
    }

    protected class VRage_GameServices_MyGameInventoryItemDefinition\u003C\u003EAssetModifierId\u003C\u003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyGameInventoryItemDefinition owner, in string value) => owner.AssetModifierId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyGameInventoryItemDefinition owner, out string value) => value = owner.AssetModifierId;
    }

    protected class VRage_GameServices_MyGameInventoryItemDefinition\u003C\u003EItemSlot\u003C\u003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, MyGameInventoryItemSlot>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyGameInventoryItemDefinition owner,
        in MyGameInventoryItemSlot value)
      {
        owner.ItemSlot = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyGameInventoryItemDefinition owner,
        out MyGameInventoryItemSlot value)
      {
        value = owner.ItemSlot;
      }
    }

    protected class VRage_GameServices_MyGameInventoryItemDefinition\u003C\u003EToolName\u003C\u003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyGameInventoryItemDefinition owner, in string value) => owner.ToolName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyGameInventoryItemDefinition owner, out string value) => value = owner.ToolName;
    }

    protected class VRage_GameServices_MyGameInventoryItemDefinition\u003C\u003ENameColor\u003C\u003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyGameInventoryItemDefinition owner, in string value) => owner.NameColor = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyGameInventoryItemDefinition owner, out string value) => value = owner.NameColor;
    }

    protected class VRage_GameServices_MyGameInventoryItemDefinition\u003C\u003EBackgroundColor\u003C\u003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyGameInventoryItemDefinition owner, in string value) => owner.BackgroundColor = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyGameInventoryItemDefinition owner, out string value) => value = owner.BackgroundColor;
    }

    protected class VRage_GameServices_MyGameInventoryItemDefinition\u003C\u003EDefinitionType\u003C\u003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, MyGameInventoryItemDefinitionType>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyGameInventoryItemDefinition owner,
        in MyGameInventoryItemDefinitionType value)
      {
        owner.DefinitionType = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyGameInventoryItemDefinition owner,
        out MyGameInventoryItemDefinitionType value)
      {
        value = owner.DefinitionType;
      }
    }

    protected class VRage_GameServices_MyGameInventoryItemDefinition\u003C\u003EHidden\u003C\u003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyGameInventoryItemDefinition owner, in bool value) => owner.Hidden = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyGameInventoryItemDefinition owner, out bool value) => value = owner.Hidden;
    }

    protected class VRage_GameServices_MyGameInventoryItemDefinition\u003C\u003EIsStoreHidden\u003C\u003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyGameInventoryItemDefinition owner, in bool value) => owner.IsStoreHidden = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyGameInventoryItemDefinition owner, out bool value) => value = owner.IsStoreHidden;
    }

    protected class VRage_GameServices_MyGameInventoryItemDefinition\u003C\u003ECanBePurchased\u003C\u003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyGameInventoryItemDefinition owner, in bool value) => owner.CanBePurchased = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyGameInventoryItemDefinition owner, out bool value) => value = owner.CanBePurchased;
    }

    protected class VRage_GameServices_MyGameInventoryItemDefinition\u003C\u003EItemQuality\u003C\u003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, MyGameInventoryItemQuality>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyGameInventoryItemDefinition owner,
        in MyGameInventoryItemQuality value)
      {
        owner.ItemQuality = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyGameInventoryItemDefinition owner,
        out MyGameInventoryItemQuality value)
      {
        value = owner.ItemQuality;
      }
    }

    protected class VRage_GameServices_MyGameInventoryItemDefinition\u003C\u003EExchange\u003C\u003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyGameInventoryItemDefinition owner, in string value) => owner.Exchange = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyGameInventoryItemDefinition owner, out string value) => value = owner.Exchange;
    }
  }
}
