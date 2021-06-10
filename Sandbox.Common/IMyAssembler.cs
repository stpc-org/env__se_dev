﻿// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.IMyAssembler
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;

namespace Sandbox.ModAPI
{
  public interface IMyAssembler : IMyProductionBlock, IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyProductionBlock, Sandbox.ModAPI.Ingame.IMyAssembler
  {
    event Action<IMyAssembler> CurrentProgressChanged;

    event Action<IMyAssembler> CurrentStateChanged;

    event Action<IMyAssembler> CurrentModeChanged;
  }
}
