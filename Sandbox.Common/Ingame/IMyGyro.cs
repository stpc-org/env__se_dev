// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.IMyGyro
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
  public interface IMyGyro : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
  {
    float GyroPower { get; set; }

    bool GyroOverride { get; set; }

    float Yaw { get; set; }

    float Pitch { get; set; }

    float Roll { get; set; }
  }
}
