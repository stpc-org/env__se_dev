// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.IMyMotorBase
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using VRageMath;

namespace Sandbox.ModAPI
{
  public interface IMyMotorBase : IMyMechanicalConnectionBlock, IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock, Sandbox.ModAPI.Ingame.IMyMotorBase
  {
    float MaxRotorAngularVelocity { get; }

    Vector3 RotorAngularVelocity { get; }

    [Obsolete("Use IMyMechanicalConnectionBlock.TopGrid")]
    VRage.Game.ModAPI.IMyCubeGrid RotorGrid { get; }

    [Obsolete("Use IMyMechanicalConnectionBlock.Top")]
    VRage.Game.ModAPI.IMyCubeBlock Rotor { get; }

    event Action<IMyMotorBase> AttachedEntityChanged;

    Vector3 DummyPosition { get; }

    void Attach(IMyMotorRotor rotor, bool updateGroup = true);
  }
}
