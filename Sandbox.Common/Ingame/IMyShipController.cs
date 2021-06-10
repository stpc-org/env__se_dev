// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.IMyShipController
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace Sandbox.ModAPI.Ingame
{
  public interface IMyShipController : IMyTerminalBlock, IMyCubeBlock, IMyEntity
  {
    bool CanControlShip { get; }

    bool IsUnderControl { get; }

    bool HasWheels { get; }

    bool ControlWheels { get; set; }

    bool ControlThrusters { get; set; }

    bool HandBrake { get; set; }

    bool DampenersOverride { get; set; }

    bool ShowHorizonIndicator { get; set; }

    Vector3D GetNaturalGravity();

    Vector3D GetArtificialGravity();

    Vector3D GetTotalGravity();

    double GetShipSpeed();

    MyShipVelocities GetShipVelocities();

    MyShipMass CalculateShipMass();

    bool TryGetPlanetPosition(out Vector3D position);

    bool TryGetPlanetElevation(MyPlanetElevation detail, out double elevation);

    Vector3 MoveIndicator { get; }

    Vector2 RotationIndicator { get; }

    float RollIndicator { get; }

    Vector3D CenterOfMass { get; }

    bool IsMainCockpit { get; set; }
  }
}
