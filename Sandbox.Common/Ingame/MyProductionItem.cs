// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.MyProductionItem
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using VRage;
using VRage.Game;

namespace Sandbox.ModAPI.Ingame
{
  public struct MyProductionItem
  {
    public readonly MyFixedPoint Amount;
    public readonly MyDefinitionId BlueprintId;
    public readonly uint ItemId;

    public MyProductionItem(uint itemId, MyDefinitionId blueprintId, MyFixedPoint amount)
      : this()
    {
      this.ItemId = itemId;
      this.BlueprintId = blueprintId;
      this.Amount = amount;
    }
  }
}
