// Decompiled with JetBrains decompiler
// Type: VRage.Game.Entity.MyPhysicalInventoryItem
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.ObjectBuilders;

namespace VRage.Game.Entity
{
  [Serializable]
  public struct MyPhysicalInventoryItem : VRage.Game.ModAPI.IMyInventoryItem, VRage.Game.ModAPI.Ingame.IMyInventoryItem
  {
    public MyFixedPoint Amount;
    public float Scale;
    [DynamicObjectBuilder(false)]
    public MyObjectBuilder_PhysicalObject Content;
    public uint ItemId;

    public MyPhysicalInventoryItem(
      MyFixedPoint amount,
      MyObjectBuilder_PhysicalObject content,
      float scale = 1f)
    {
      this.ItemId = 0U;
      this.Amount = amount;
      this.Scale = scale;
      this.Content = content;
    }

    public MyPhysicalInventoryItem(MyObjectBuilder_InventoryItem item)
    {
      this.ItemId = 0U;
      this.Amount = item.Amount;
      this.Scale = item.Scale;
      this.Content = item.PhysicalContent.Clone() as MyObjectBuilder_PhysicalObject;
    }

    public MyObjectBuilder_InventoryItem GetObjectBuilder()
    {
      MyObjectBuilder_InventoryItem newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_InventoryItem>();
      newObject.Amount = this.Amount;
      newObject.Scale = this.Scale;
      newObject.PhysicalContent = this.Content;
      newObject.ItemId = this.ItemId;
      return newObject;
    }

    public override string ToString() => string.Format("{0}x {1}", (object) this.Amount, (object) this.Content.GetId());

    MyFixedPoint VRage.Game.ModAPI.Ingame.IMyInventoryItem.Amount
    {
      get => this.Amount;
      set => this.Amount = value;
    }

    float VRage.Game.ModAPI.Ingame.IMyInventoryItem.Scale
    {
      get => this.Scale;
      set => this.Scale = value;
    }

    MyObjectBuilder_Base VRage.Game.ModAPI.Ingame.IMyInventoryItem.Content
    {
      get => (MyObjectBuilder_Base) this.Content;
      set => this.Content = value as MyObjectBuilder_PhysicalObject;
    }

    uint VRage.Game.ModAPI.Ingame.IMyInventoryItem.ItemId
    {
      get => this.ItemId;
      set => this.ItemId = value;
    }

    protected class VRage_Game_Entity_MyPhysicalInventoryItem\u003C\u003EAmount\u003C\u003EAccessor : IMemberAccessor<MyPhysicalInventoryItem, MyFixedPoint>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPhysicalInventoryItem owner, in MyFixedPoint value) => owner.Amount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPhysicalInventoryItem owner, out MyFixedPoint value) => value = owner.Amount;
    }

    protected class VRage_Game_Entity_MyPhysicalInventoryItem\u003C\u003EScale\u003C\u003EAccessor : IMemberAccessor<MyPhysicalInventoryItem, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPhysicalInventoryItem owner, in float value) => owner.Scale = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPhysicalInventoryItem owner, out float value) => value = owner.Scale;
    }

    protected class VRage_Game_Entity_MyPhysicalInventoryItem\u003C\u003EContent\u003C\u003EAccessor : IMemberAccessor<MyPhysicalInventoryItem, MyObjectBuilder_PhysicalObject>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyPhysicalInventoryItem owner,
        in MyObjectBuilder_PhysicalObject value)
      {
        owner.Content = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyPhysicalInventoryItem owner,
        out MyObjectBuilder_PhysicalObject value)
      {
        value = owner.Content;
      }
    }

    protected class VRage_Game_Entity_MyPhysicalInventoryItem\u003C\u003EItemId\u003C\u003EAccessor : IMemberAccessor<MyPhysicalInventoryItem, uint>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPhysicalInventoryItem owner, in uint value) => owner.ItemId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPhysicalInventoryItem owner, out uint value) => value = owner.ItemId;
    }

    protected class VRage_Game_Entity_MyPhysicalInventoryItem\u003C\u003EVRage\u002EGame\u002EModAPI\u002EIngame\u002EIMyInventoryItem\u002EAmount\u003C\u003EAccessor : IMemberAccessor<MyPhysicalInventoryItem, MyFixedPoint>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPhysicalInventoryItem owner, in MyFixedPoint value) => owner.VRage\u002EGame\u002EModAPI\u002EIngame\u002EIMyInventoryItem\u002EAmount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPhysicalInventoryItem owner, out MyFixedPoint value) => value = owner.VRage\u002EGame\u002EModAPI\u002EIngame\u002EIMyInventoryItem\u002EAmount;
    }

    protected class VRage_Game_Entity_MyPhysicalInventoryItem\u003C\u003EVRage\u002EGame\u002EModAPI\u002EIngame\u002EIMyInventoryItem\u002EScale\u003C\u003EAccessor : IMemberAccessor<MyPhysicalInventoryItem, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPhysicalInventoryItem owner, in float value) => owner.VRage\u002EGame\u002EModAPI\u002EIngame\u002EIMyInventoryItem\u002EScale = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPhysicalInventoryItem owner, out float value) => value = owner.VRage\u002EGame\u002EModAPI\u002EIngame\u002EIMyInventoryItem\u002EScale;
    }

    protected class VRage_Game_Entity_MyPhysicalInventoryItem\u003C\u003EVRage\u002EGame\u002EModAPI\u002EIngame\u002EIMyInventoryItem\u002EContent\u003C\u003EAccessor : IMemberAccessor<MyPhysicalInventoryItem, MyObjectBuilder_Base>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPhysicalInventoryItem owner, in MyObjectBuilder_Base value) => owner.VRage\u002EGame\u002EModAPI\u002EIngame\u002EIMyInventoryItem\u002EContent = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPhysicalInventoryItem owner, out MyObjectBuilder_Base value) => value = owner.VRage\u002EGame\u002EModAPI\u002EIngame\u002EIMyInventoryItem\u002EContent;
    }

    protected class VRage_Game_Entity_MyPhysicalInventoryItem\u003C\u003EVRage\u002EGame\u002EModAPI\u002EIngame\u002EIMyInventoryItem\u002EItemId\u003C\u003EAccessor : IMemberAccessor<MyPhysicalInventoryItem, uint>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPhysicalInventoryItem owner, in uint value) => owner.VRage\u002EGame\u002EModAPI\u002EIngame\u002EIMyInventoryItem\u002EItemId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPhysicalInventoryItem owner, out uint value) => value = owner.VRage\u002EGame\u002EModAPI\u002EIngame\u002EIMyInventoryItem\u002EItemId;
    }
  }
}
