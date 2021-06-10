// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.MyStoreItemData
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRage.ObjectBuilders;

namespace VRage.Game.ModAPI
{
  public struct MyStoreItemData
  {
    public SerializableDefinitionId ItemId { get; private set; }

    public int Amount { get; private set; }

    public int PricePerUnit { get; private set; }

    public Action<int, int, long, long, long> OnTransaction { get; private set; }

    public Action OnCancel { get; private set; }

    public MyStoreItemData(
      SerializableDefinitionId itemId,
      int amount,
      int pricePerUnit,
      Action<int, int, long, long, long> onTransactionCallback,
      Action onCancelCallback)
    {
      this.ItemId = itemId;
      this.Amount = amount;
      this.PricePerUnit = pricePerUnit;
      this.OnTransaction = onTransactionCallback;
      this.OnCancel = onCancelCallback;
    }
  }
}
