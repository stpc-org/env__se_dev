// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.Ingame.MyInventoryItem
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.Game.ModAPI.Ingame
{
  public struct MyInventoryItem : IComparable<MyInventoryItem>, IEquatable<MyInventoryItem>
  {
    public readonly uint ItemId;
    public readonly MyFixedPoint Amount;
    public readonly MyItemType Type;

    public MyInventoryItem(MyItemType type, uint itemId, MyFixedPoint amount)
    {
      this.Type = type;
      this.ItemId = itemId;
      this.Amount = amount;
    }

    public static bool operator ==(MyInventoryItem a, MyInventoryItem b) => (int) a.ItemId == (int) b.ItemId && a.Amount == b.Amount && a.Type == b.Type;

    public static bool operator !=(MyInventoryItem a, MyInventoryItem b) => !(a == b);

    public bool Equals(MyInventoryItem other) => this == other;

    public override bool Equals(object obj) => obj is MyInventoryItem other && this.Equals(other);

    public override int GetHashCode() => MyTuple.CombineHashCodes(this.ItemId.GetHashCode(), this.Amount.GetHashCode(), this.Type.GetHashCode());

    public int CompareTo(MyInventoryItem other)
    {
      int num1 = this.ItemId.CompareTo(other.ItemId);
      if (num1 != 0)
        return num1;
      int num2 = ((double) this.Amount).CompareTo((double) other.Amount);
      return num2 != 0 ? num2 : this.Type.CompareTo(other.Type);
    }

    public override string ToString() => string.Format("{0}x {1}", (object) this.Amount.ToString(), (object) ((MyDefinitionId) this.Type).ToString());
  }
}
