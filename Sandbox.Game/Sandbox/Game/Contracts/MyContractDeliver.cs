// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Contracts.MyContractDeliver
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using System.Text;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Contracts
{
  [MyContractDescriptor(typeof (MyObjectBuilder_ContractDeliver))]
  public class MyContractDeliver : MyContract
  {
    public double DeliverDistance { get; set; }

    public override MyObjectBuilder_Contract GetObjectBuilder()
    {
      MyObjectBuilder_Contract objectBuilder = base.GetObjectBuilder();
      (objectBuilder as MyObjectBuilder_ContractDeliver).DeliverDistance = this.DeliverDistance;
      return objectBuilder;
    }

    public override void Init(MyObjectBuilder_Contract ob)
    {
      base.Init(ob);
      if (!(ob is MyObjectBuilder_ContractDeliver builderContractDeliver))
        return;
      this.DeliverDistance = builderContractDeliver.DeliverDistance;
    }

    public override string ToDebugString() => new StringBuilder(base.ToDebugString()).ToString();

    protected override MyActivationResults CanActivate_Internal(long playerId)
    {
      MyActivationResults activationResults = base.CanActivate_Internal(playerId);
      if (activationResults != MyActivationResults.Success)
        return activationResults;
      return !this.CheckPlayerInventory(playerId) ? MyActivationResults.Fail_InsufficientInventorySpace : MyActivationResults.Success;
    }

    protected override void Activate_Internal(MyTimeSpan timeOfActivation) => base.Activate_Internal(timeOfActivation);

    protected override void FailFor_Internal(long player, bool abandon = false) => base.FailFor_Internal(player, abandon);

    protected override void FinishFor_Internal(long player, int rewardeeCount) => base.FinishFor_Internal(player, rewardeeCount);

    public override MyDefinitionId? GetDefinitionId() => new MyDefinitionId?(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_ContractTypeDefinition), "Deliver"));

    private bool CheckPlayerInventory(long playerId)
    {
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(playerId);
      if (identity == null)
        return false;
      MyCharacter character = identity.Character;
      if (character == null || character.InventoryCount <= 0)
        return false;
      MyInventoryBase inventoryBase = character.GetInventoryBase();
      MyPackageDefinition packageItem = this.GetPackageItem();
      if (packageItem == null)
        return false;
      float mass = packageItem.Mass;
      float volume = packageItem.Volume;
      return (double) (float) (inventoryBase.MaxMass - inventoryBase.CurrentMass) >= (double) mass && (double) (float) (inventoryBase.MaxVolume - inventoryBase.CurrentVolume) >= (double) volume;
    }

    private MyPackageDefinition GetPackageItem() => MyDefinitionManager.Static.GetPhysicalItemDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_Package), "Package")) as MyPackageDefinition;
  }
}
