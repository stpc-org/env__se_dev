// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Contracts.MyContractObtainAndDeliver
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Text;
using VRage.Game;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Contracts
{
  [MyContractDescriptor(typeof (MyObjectBuilder_ContractObtainAndDeliver))]
  public class MyContractObtainAndDeliver : MyContract
  {
    public override MyObjectBuilder_Contract GetObjectBuilder() => base.GetObjectBuilder();

    public override void Init(MyObjectBuilder_Contract ob)
    {
      base.Init(ob);
      MyObjectBuilder_ContractObtainAndDeliver obtainAndDeliver = ob as MyObjectBuilder_ContractObtainAndDeliver;
    }

    public override MyDefinitionId? GetDefinitionId() => new MyDefinitionId?(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_ContractTypeDefinition), "ObtainAndDeliver"));

    protected override void Activate_Internal(MyTimeSpan timeOfActivation) => base.Activate_Internal(timeOfActivation);

    protected override void FailFor_Internal(long player, bool abandon = false) => base.FailFor_Internal(player, abandon);

    protected override void FinishFor_Internal(long player, int rewardeeCount) => base.FinishFor_Internal(player, rewardeeCount);

    public override string ToDebugString()
    {
      StringBuilder stringBuilder = new StringBuilder(base.ToDebugString());
      stringBuilder.Append(this.ContractCondition.ToDebugString());
      return stringBuilder.ToString();
    }

    public MyDefinitionId? GetItemId() => !(this.ContractCondition is MyContractConditionDeliverItems contractCondition) ? new MyDefinitionId?() : new MyDefinitionId?(contractCondition.ItemType);

    protected override bool CanBeShared_Internal() => true;

    protected override bool CanPlayerReceiveReward(long identityId) => true;
  }
}
