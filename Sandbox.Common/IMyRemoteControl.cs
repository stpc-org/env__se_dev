// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.IMyRemoteControl
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using VRage.Game.ModAPI.Interfaces;
using VRageMath;

namespace Sandbox.ModAPI
{
  public interface IMyRemoteControl : IMyShipController, IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyTerminalBlock, Sandbox.ModAPI.Ingame.IMyShipController, IMyControllableEntity, Sandbox.ModAPI.Ingame.IMyRemoteControl
  {
    new bool GetNearestPlayer(out Vector3D playerPosition);

    Vector3D GetFreeDestination(
      Vector3D originalDestination,
      float checkRadius,
      float shipRadius = 0.0f);
  }
}
