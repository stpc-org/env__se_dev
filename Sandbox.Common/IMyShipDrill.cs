// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.IMyShipDrill
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

namespace Sandbox.ModAPI
{
  public interface IMyShipDrill : Sandbox.ModAPI.Ingame.IMyShipDrill, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyFunctionalBlock, IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity
  {
    float DrillHarvestMultiplier { get; set; }

    float PowerConsumptionMultiplier { get; set; }
  }
}
