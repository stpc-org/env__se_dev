// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.IMyGasGenerator
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

namespace Sandbox.ModAPI
{
  public interface IMyGasGenerator : Sandbox.ModAPI.Ingame.IMyGasGenerator, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyFunctionalBlock, IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity
  {
    float ProductionCapacityMultiplier { get; set; }

    float PowerConsumptionMultiplier { get; set; }
  }
}
