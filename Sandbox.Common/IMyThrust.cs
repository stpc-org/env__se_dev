// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.IMyThrust
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;

namespace Sandbox.ModAPI
{
  public interface IMyThrust : Sandbox.ModAPI.Ingame.IMyThrust, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyFunctionalBlock, IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity
  {
    event Action<IMyThrust, float> ThrustOverrideChanged;

    float ThrustMultiplier { get; set; }

    float PowerConsumptionMultiplier { get; set; }
  }
}
