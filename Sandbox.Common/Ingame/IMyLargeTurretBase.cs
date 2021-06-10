// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.IMyLargeTurretBase
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace Sandbox.ModAPI.Ingame
{
  public interface IMyLargeTurretBase : IMyUserControllableGun, IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
  {
    bool IsUnderControl { get; }

    bool CanControl { get; }

    float Range { get; }

    bool IsAimed { get; }

    void TrackTarget(Vector3D pos, Vector3 velocity);

    void SetTarget(Vector3D pos);

    bool HasTarget { get; }

    float Elevation { get; set; }

    void SyncElevation();

    float Azimuth { get; set; }

    void SyncAzimuth();

    bool EnableIdleRotation { get; set; }

    void SyncEnableIdleRotation();

    bool AIEnabled { get; }

    void ResetTargetingToDefault();

    MyDetectedEntityInfo GetTargetedEntity();
  }
}
