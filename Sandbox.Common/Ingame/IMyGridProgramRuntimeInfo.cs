// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.IMyGridProgramRuntimeInfo
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;

namespace Sandbox.ModAPI.Ingame
{
  public interface IMyGridProgramRuntimeInfo
  {
    TimeSpan TimeSinceLastRun { get; }

    double LastRunTimeMs { get; }

    int MaxInstructionCount { get; }

    int CurrentInstructionCount { get; }

    int MaxCallChainDepth { get; }

    int CurrentCallChainDepth { get; }

    UpdateFrequency UpdateFrequency { get; set; }
  }
}
