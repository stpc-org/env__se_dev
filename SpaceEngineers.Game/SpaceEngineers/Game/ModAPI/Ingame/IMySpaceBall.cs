// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.ModAPI.Ingame.IMySpaceBall
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.ModAPI.Ingame;
using System;
using VRage.Game.ModAPI.Ingame;

namespace SpaceEngineers.Game.ModAPI.Ingame
{
  public interface IMySpaceBall : IMyVirtualMass, IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
  {
    float Friction { get; set; }

    float Restitution { get; set; }

    [Obsolete("Use IMySpaceBall.Broadcasting")]
    bool IsBroadcasting { get; }

    bool Broadcasting { get; set; }

    new float VirtualMass { get; set; }
  }
}
