// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.IMyThrust
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace Sandbox.ModAPI.Ingame
{
  public interface IMyThrust : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
  {
    float ThrustOverride { get; set; }

    float ThrustOverridePercentage { get; set; }

    float MaxThrust { get; }

    float MaxEffectiveThrust { get; }

    float CurrentThrust { get; }

    Vector3I GridThrustDirection { get; }
  }
}
