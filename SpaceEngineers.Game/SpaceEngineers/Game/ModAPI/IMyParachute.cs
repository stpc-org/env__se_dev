﻿// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.ModAPI.IMyParachute
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.ModAPI.Ingame;
using System;
using VRage.Game.ModAPI.Ingame;

namespace SpaceEngineers.Game.ModAPI
{
  public interface IMyParachute : SpaceEngineers.Game.ModAPI.Ingame.IMyParachute, IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
  {
    event Action<bool> ParachuteStateChanged;

    event Action<bool> DoorStateChanged;
  }
}
