// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.IMyMotorStator
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
  public interface IMyMotorStator : IMyMotorBase, IMyMechanicalConnectionBlock, IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
  {
    float Angle { get; }

    float Torque { get; set; }

    float BrakingTorque { get; set; }

    float TargetVelocityRad { get; set; }

    float TargetVelocityRPM { get; set; }

    float LowerLimitRad { get; set; }

    float LowerLimitDeg { get; set; }

    float UpperLimitRad { get; set; }

    float UpperLimitDeg { get; set; }

    float Displacement { get; set; }

    bool RotorLock { get; set; }
  }
}
