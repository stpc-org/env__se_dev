// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.ModAPI.IMyAirVent
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Game.EntityComponents;
using VRage.Game.Components;

namespace SpaceEngineers.Game.ModAPI
{
  public interface IMyAirVent : Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMyAirVent
  {
    float GasOutputPerSecond { get; }

    float GasInputPerSecond { get; }

    MyResourceSinkInfo OxygenSinkInfo { get; set; }

    MyResourceSourceComponent SourceComp { get; set; }
  }
}
