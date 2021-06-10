// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyContractCreationResults
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

namespace Sandbox.Game.World.Generator
{
  public enum MyContractCreationResults
  {
    Success,
    Fail_Common,
    Fail_Impossible,
    Fail_NoAccess,
    Fail_GridNotFound,
    Fail_BlockNotFound,
    Error,
    Error_MissingKeyStructure,
    Fail_NotAnOwnerOfBlock,
    Fail_NotAnOwnerOfGrid,
    Fail_NotEnoughFunds,
    Fail_CreationLimitHard,
  }
}
