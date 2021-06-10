// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Contracts.MyContractConditionDeliverItems
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Contracts
{
  [MyContractConditionDescriptor(typeof (MyObjectBuilder_ContractConditionDeliverItems))]
  public class MyContractConditionDeliverItems : MyContractCondition
  {
    public MyDefinitionId ItemType;
    public int ItemAmount;
    public float ItemVolume;
    public bool TransferItems;

    public override void Init(MyObjectBuilder_ContractCondition builder)
    {
      base.Init(builder);
      if (!(builder is MyObjectBuilder_ContractConditionDeliverItems conditionDeliverItems))
        return;
      this.ItemType = (MyDefinitionId) conditionDeliverItems.ItemType;
      this.ItemAmount = conditionDeliverItems.ItemAmount;
      this.ItemVolume = conditionDeliverItems.ItemVolume;
      this.TransferItems = conditionDeliverItems.TransferItems;
    }

    public override MyObjectBuilder_ContractCondition GetObjectBuilder()
    {
      MyObjectBuilder_ContractCondition objectBuilder = base.GetObjectBuilder();
      MyObjectBuilder_ContractConditionDeliverItems conditionDeliverItems = objectBuilder as MyObjectBuilder_ContractConditionDeliverItems;
      conditionDeliverItems.ItemType = (SerializableDefinitionId) this.ItemType;
      conditionDeliverItems.ItemAmount = this.ItemAmount;
      conditionDeliverItems.ItemVolume = this.ItemVolume;
      conditionDeliverItems.TransferItems = this.TransferItems;
      return objectBuilder;
    }

    public override string ToDebugString() => string.Format("  Deliver Item\n   {0}\n   {1}", (object) this.ItemType, (object) this.ItemAmount);
  }
}
