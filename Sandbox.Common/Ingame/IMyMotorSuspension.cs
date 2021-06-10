// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.IMyMotorSuspension
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
  public interface IMyMotorSuspension : IMyMotorBase, IMyMechanicalConnectionBlock, IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
  {
    bool Steering { get; set; }

    bool Propulsion { get; set; }

    bool InvertSteer { get; set; }

    bool InvertPropulsion { get; set; }

    [Obsolete]
    float Damping { get; }

    float Strength { get; set; }

    float Friction { get; set; }

    float Power { get; set; }

    float Height { get; set; }

    float SteerAngle { get; }

    float MaxSteerAngle { get; set; }

    [Obsolete]
    float SteerSpeed { get; }

    [Obsolete]
    float SteerReturnSpeed { get; }

    [Obsolete]
    float SuspensionTravel { get; }

    bool Brake { get; set; }

    bool AirShockEnabled { get; set; }
  }
}
