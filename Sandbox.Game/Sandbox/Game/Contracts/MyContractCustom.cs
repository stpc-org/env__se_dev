// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Contracts.MyContractCustom
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Library.Utils;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Contracts
{
  [MyContractDescriptor(typeof (MyObjectBuilder_ContractCustom))]
  public class MyContractCustom : MyContract
  {
    private MyDefinitionId m_definitionId;
    private MySessionComponentContractSystem m_cachedContractSystem;

    public string ContractName { get; private set; }

    public string ContractDescription { get; private set; }

    protected MySessionComponentContractSystem ContractSystem
    {
      get
      {
        if (this.m_cachedContractSystem == null)
          this.m_cachedContractSystem = MySession.Static.GetComponent<MySessionComponentContractSystem>();
        return this.m_cachedContractSystem;
      }
    }

    public override void Init(MyObjectBuilder_Contract ob)
    {
      base.Init(ob);
      if (!(ob is MyObjectBuilder_ContractCustom builderContractCustom))
        return;
      this.m_definitionId = (MyDefinitionId) builderContractCustom.DefinitionId;
      this.ContractName = builderContractCustom.ContractName;
      this.ContractDescription = builderContractCustom.ContractDescription;
    }

    public override MyObjectBuilder_Contract GetObjectBuilder()
    {
      MyObjectBuilder_Contract objectBuilder = base.GetObjectBuilder();
      if (!(objectBuilder is MyObjectBuilder_ContractCustom builderContractCustom))
        return objectBuilder;
      builderContractCustom.DefinitionId = (SerializableDefinitionId) this.m_definitionId;
      builderContractCustom.ContractName = this.ContractName;
      builderContractCustom.ContractDescription = this.ContractDescription;
      return objectBuilder;
    }

    private MyActivationResults ConvertActivationResult(
      MyActivationCustomResults result)
    {
      switch (result)
      {
        case MyActivationCustomResults.Success:
          return MyActivationResults.Success;
        case MyActivationCustomResults.Fail_General:
          return MyActivationResults.Fail;
        case MyActivationCustomResults.Fail_InsufficientFunds:
          return MyActivationResults.Fail_InsufficientFunds;
        case MyActivationCustomResults.Fail_InsufficientInventorySpace:
          return MyActivationResults.Fail_InsufficientInventorySpace;
        case MyActivationCustomResults.Error_General:
          return MyActivationResults.Error;
        default:
          return MyActivationResults.Error;
      }
    }

    protected override MyActivationResults CanActivate_Internal(long playerId)
    {
      MyActivationResults activationResults1 = base.CanActivate_Internal(playerId);
      if (activationResults1 != MyActivationResults.Success)
        return activationResults1;
      MySessionComponentContractSystem contractSystem = this.ContractSystem;
      if (contractSystem.CustomCanActivateContract != null)
      {
        MyActivationResults activationResults2 = this.ConvertActivationResult(contractSystem.CustomCanActivateContract(this.Id, playerId));
        if (activationResults2 != MyActivationResults.Success)
          return activationResults2;
      }
      return MyActivationResults.Success;
    }

    protected override void Activate_Internal(MyTimeSpan timeOfActivation)
    {
      base.Activate_Internal(timeOfActivation);
      MySessionComponentContractSystem contractSystem = this.ContractSystem;
      long num = 0;
      if (this.Owners.Count >= 1)
        num = this.Owners[0];
      long id = this.Id;
      long identityId = num;
      contractSystem.OnCustomActivateContract(id, identityId);
    }

    protected override void FailFor_Internal(long player, bool abandon = false)
    {
      base.FailFor_Internal(player, abandon);
      this.ContractSystem.OnCustomFailFor(this.Id, player, abandon);
    }

    protected override void FinishFor_Internal(long player, int rewardeeCount)
    {
      base.FinishFor_Internal(player, rewardeeCount);
      this.ContractSystem.OnCustomFinishFor(this.Id, player, rewardeeCount);
    }

    protected override bool NeedsUpdate_Internal()
    {
      MySessionComponentContractSystem contractSystem = this.ContractSystem;
      if (base.NeedsUpdate_Internal())
        return true;
      return contractSystem.CustomNeedsUpdate != null && contractSystem.CustomNeedsUpdate(this.Id);
    }

    protected override void Finish_Internal()
    {
      base.Finish_Internal();
      this.ContractSystem.OnCustomFinish(this.Id);
    }

    protected override void Fail_Internal()
    {
      base.Fail_Internal();
      this.ContractSystem.OnCustomFail(this.Id);
    }

    protected override void CleanUp_Internal()
    {
      base.CleanUp_Internal();
      this.ContractSystem.OnCustomCleanUp(this.Id);
    }

    public override void TimeRanOut_Internal()
    {
      this.ContractSystem.OnCustomTimeRanOut(this.Id);
      base.TimeRanOut_Internal();
    }

    public override void Update(MyTimeSpan currentTime)
    {
      base.Update(currentTime);
      this.ContractSystem.OnCustomUpdate(this.Id, MyContractCustom.ConvertContractState(this.State), currentTime);
    }

    internal static MyCustomContractStateEnum ConvertContractState(
      MyContractStateEnum state)
    {
      switch (state)
      {
        case MyContractStateEnum.Inactive:
          return MyCustomContractStateEnum.Inactive;
        case MyContractStateEnum.Active:
          return MyCustomContractStateEnum.Active;
        case MyContractStateEnum.Finished:
          return MyCustomContractStateEnum.Finished;
        case MyContractStateEnum.Failed:
          return MyCustomContractStateEnum.Failed;
        case MyContractStateEnum.ToBeDisposed:
          return MyCustomContractStateEnum.ToBeDisposed;
        case MyContractStateEnum.Disposed:
          return MyCustomContractStateEnum.Disposed;
        default:
          return MyCustomContractStateEnum.Invalid;
      }
    }

    public override MyDefinitionId? GetDefinitionId() => new MyDefinitionId?(this.m_definitionId);
  }
}
