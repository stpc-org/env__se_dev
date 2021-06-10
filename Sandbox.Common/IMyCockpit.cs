// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.IMyCockpit
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;

namespace Sandbox.ModAPI
{
  public interface IMyCockpit : IMyShipController, IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyTerminalBlock, Sandbox.ModAPI.Ingame.IMyShipController, IMyControllableEntity, Sandbox.ModAPI.Ingame.IMyCockpit, Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider, IMyCameraController, IMyTextSurfaceProvider
  {
    new float OxygenFilledRatio { get; set; }

    void AttachPilot(IMyCharacter pilot);

    void RemovePilot();
  }
}
