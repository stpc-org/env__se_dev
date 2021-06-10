// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.IMyDoor
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
  public interface IMyDoor : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
  {
    [Obsolete("Use the Status property instead")]
    bool Open { get; }

    DoorStatus Status { get; }

    float OpenRatio { get; }

    void OpenDoor();

    void CloseDoor();

    void ToggleDoor();
  }
}
