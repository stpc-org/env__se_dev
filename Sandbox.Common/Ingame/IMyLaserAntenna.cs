// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.IMyLaserAntenna
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace Sandbox.ModAPI.Ingame
{
  public interface IMyLaserAntenna : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
  {
    bool RequireLoS { get; }

    Vector3D TargetCoords { get; }

    void SetTargetCoords(string coords);

    void Connect();

    bool IsPermanent { get; set; }

    [Obsolete("Check the Status property instead.")]
    bool IsOutsideLimits { get; }

    MyLaserAntennaStatus Status { get; }

    float Range { get; set; }
  }
}
