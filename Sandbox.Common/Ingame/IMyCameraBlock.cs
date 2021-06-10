// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.IMyCameraBlock
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace Sandbox.ModAPI.Ingame
{
  public interface IMyCameraBlock : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
  {
    bool IsActive { get; }

    MyDetectedEntityInfo Raycast(double distance, float pitch = 0.0f, float yaw = 0.0f);

    MyDetectedEntityInfo Raycast(Vector3D targetPos);

    MyDetectedEntityInfo Raycast(double distance, Vector3D targetDirection);

    double AvailableScanRange { get; }

    bool EnableRaycast { get; set; }

    bool CanScan(double distance);

    bool CanScan(double distance, Vector3D direction);

    bool CanScan(Vector3D target);

    int TimeUntilScan(double distance);

    float RaycastConeLimit { get; }

    double RaycastDistanceLimit { get; }
  }
}
