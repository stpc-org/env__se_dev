// Decompiled with JetBrains decompiler
// Type: VRage.ModAPI.MyAPIGatewayShortcuts
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRageMath;

namespace VRage.ModAPI
{
  public static class MyAPIGatewayShortcuts
  {
    public static Action<IMyEntity> RegisterEntityUpdate;
    public static Action<IMyEntity, bool> UnregisterEntityUpdate;
    public static MyAPIGatewayShortcuts.GetMainCameraCallback GetMainCamera;
    public static MyAPIGatewayShortcuts.GetWorldBoundariesCallback GetWorldBoundaries;
    public static MyAPIGatewayShortcuts.GetLocalPlayerPositionCallback GetLocalPlayerPosition;

    public delegate IMyCamera GetMainCameraCallback();

    public delegate BoundingBoxD GetWorldBoundariesCallback();

    public delegate Vector3D GetLocalPlayerPositionCallback();
  }
}
