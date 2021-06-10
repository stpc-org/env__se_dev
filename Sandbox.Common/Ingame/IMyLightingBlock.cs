// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.IMyLightingBlock
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace Sandbox.ModAPI.Ingame
{
  public interface IMyLightingBlock : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
  {
    float Radius { get; set; }

    [Obsolete("Use Radius")]
    float ReflectorRadius { get; }

    float Intensity { get; set; }

    float Falloff { get; set; }

    float BlinkIntervalSeconds { get; set; }

    [Obsolete("Use BlinkLength instead.")]
    float BlinkLenght { get; }

    float BlinkLength { get; set; }

    float BlinkOffset { get; set; }

    Color Color { get; set; }
  }
}
