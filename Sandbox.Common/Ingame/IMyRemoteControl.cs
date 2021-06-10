// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.IMyRemoteControl
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System.Collections.Generic;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace Sandbox.ModAPI.Ingame
{
  public interface IMyRemoteControl : IMyShipController, IMyTerminalBlock, IMyCubeBlock, IMyEntity
  {
    bool GetNearestPlayer(out Vector3D playerPosition);

    void ClearWaypoints();

    void GetWaypointInfo(List<MyWaypointInfo> waypoints);

    void AddWaypoint(Vector3D coords, string name);

    void AddWaypoint(MyWaypointInfo coords);

    void SetAutoPilotEnabled(bool enabled);

    bool IsAutoPilotEnabled { get; }

    void SetCollisionAvoidance(bool enabled);

    void SetDockingMode(bool enabled);

    float SpeedLimit { get; set; }

    FlightMode FlightMode { get; set; }

    Base6Directions.Direction Direction { get; set; }

    MyWaypointInfo CurrentWaypoint { get; }

    bool WaitForFreeWay { get; set; }
  }
}
