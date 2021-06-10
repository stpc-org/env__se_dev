// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MyContractResults
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

namespace Sandbox.Game.Entities.Blocks
{
  public enum MyContractResults
  {
    Success,
    Error_Unknown,
    Error_MissingKeyStructure,
    Error_InvalidData,
    Fail_CannotAccess,
    Fail_NotPossible,
    Fail_ActivationConditionsNotMet,
    Fail_ActivationConditionsNotMet_InsufficientFunds,
    Fail_ActivationConditionsNotMet_InsufficientSpace,
    Fail_FinishConditionsNotMet,
    Fail_FinishConditionsNotMet_MissingPackage,
    Fail_FinishConditionsNotMet_IncorrectTargetEntity,
    Fail_ContractNotFound_Activation,
    Fail_ContractNotFound_Abandon,
    Fail_ContractNotFound_Finish,
    Fail_FinishConditionsNotMet_NotEnoughItems,
    Fail_ActivationConditionsNotMet_ContractLimitReachedHard,
    Fail_ActivationConditionsNotMet_TargetOffline,
    Fail_FinishConditionsNotMet_NotEnoughSpace,
    Fail_ActivationConditionsNotMet_YouAreTargetOfThisHunt,
  }
}
