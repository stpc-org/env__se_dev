// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.Ingame.MyStoreItemDataSimple
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.ObjectBuilders;

namespace VRage.Game.ModAPI.Ingame
{
  public struct MyStoreItemDataSimple
  {
    public MyItemType ItemId { get; private set; }

    public int Amount { get; private set; }

    public int PricePerUnit { get; private set; }

    public MyStoreItemDataSimple(SerializableDefinitionId itemId, int amount, int pricePerUnit)
    {
      this.ItemId = (MyItemType) itemId;
      this.Amount = amount;
      this.PricePerUnit = pricePerUnit;
    }
  }
}
