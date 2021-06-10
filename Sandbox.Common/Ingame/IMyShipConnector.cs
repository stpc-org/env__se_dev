// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.IMyShipConnector
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
  public interface IMyShipConnector : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
  {
    bool ThrowOut { get; set; }

    bool CollectAll { get; set; }

    float PullStrength { get; set; }

    [Obsolete("Use the Status property")]
    bool IsLocked { get; }

    [Obsolete("Use the Status property")]
    bool IsConnected { get; }

    MyShipConnectorStatus Status { get; }

    IMyShipConnector OtherConnector { get; }

    void Connect();

    void Disconnect();

    void ToggleConnect();
  }
}
