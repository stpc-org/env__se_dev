// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.IMyContractSystem
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.Game.ModAPI
{
  public interface IMyContractSystem
  {
    Func<long, long, bool> CustomFinishCondition { get; set; }

    Func<long, long, MyActivationCustomResults> CustomCanActivateContract { get; set; }

    Func<long, bool> CustomNeedsUpdate { get; set; }

    event MyContractConditionDelegate CustomConditionFinished;

    event MyContractActivateDelegate CustomActivateContract;

    event MyContractFailedDelegate CustomFailFor;

    event MyContractFinishedDelegate CustomFinishFor;

    event MyContractChangeDelegate CustomFinish;

    event MyContractChangeDelegate CustomFail;

    event MyContractChangeDelegate CustomCleanUp;

    event MyContractChangeDelegate CustomTimeRanOut;

    event MyContractUpdateDelegate CustomUpdate;

    MyAddContractResultWrapper AddContract(IMyContract contract);

    bool RemoveContract(long contractId);

    bool IsContractActive(long contractId);

    MyCustomContractStateEnum GetContractState(long contractId);

    bool TryFinishCustomContract(long contractId);

    bool TryFailCustomContract(long contractId);

    bool TryAbandonCustomContract(long contractId, long playerId);

    MyDefinitionId? GetContractDefinitionId(long contractId);
  }
}
