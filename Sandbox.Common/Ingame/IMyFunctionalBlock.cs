// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.IMyFunctionalBlock
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
  public interface IMyFunctionalBlock : IMyTerminalBlock, IMyCubeBlock, IMyEntity
  {
    bool Enabled { get; set; }

    [Obsolete("Use the setter of Enabled")]
    void RequestEnable(bool enable);
  }
}
