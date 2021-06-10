// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.IMyProgrammableBlock
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using Sandbox.ModAPI.Ingame;

namespace Sandbox.ModAPI
{
  public interface IMyProgrammableBlock : IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyProgrammableBlock, Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider
  {
    void Recompile();

    void Run();

    void Run(string argument);

    void Run(string argument, UpdateType updateSource);

    new bool TryRun(string argument);

    string ProgramData { get; set; }

    string StorageData { get; set; }

    bool HasCompileErrors { get; }
  }
}
