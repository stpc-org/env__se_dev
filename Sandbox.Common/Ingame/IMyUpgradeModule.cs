// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.IMyUpgradeModule
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System.Collections.Generic;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;

namespace Sandbox.ModAPI.Ingame
{
  public interface IMyUpgradeModule : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
  {
    void GetUpgradeList(out List<MyUpgradeModuleInfo> upgrades);

    uint UpgradeCount { get; }

    uint Connections { get; }
  }
}
