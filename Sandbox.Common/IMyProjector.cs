// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.IMyProjector
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using VRage.Game;

namespace Sandbox.ModAPI
{
  public interface IMyProjector : IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyProjector, Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider, IMyTextSurfaceProvider
  {
    VRage.Game.ModAPI.IMyCubeGrid ProjectedGrid { get; }

    void SetProjectedGrid(MyObjectBuilder_CubeGrid grid);

    BuildCheckResult CanBuild(
      VRage.Game.ModAPI.IMySlimBlock projectedBlock,
      bool checkHavokIntersections);

    void Build(
      VRage.Game.ModAPI.IMySlimBlock cubeBlock,
      long owner,
      long builder,
      bool requestInstant,
      long builtBy = 0);

    bool LoadBlueprint(string name);

    bool LoadRandomBlueprint(string searchPattern);
  }
}
