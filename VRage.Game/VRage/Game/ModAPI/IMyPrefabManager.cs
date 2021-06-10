// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.IMyPrefabManager
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRageMath;

namespace VRage.Game.ModAPI
{
  public interface IMyPrefabManager
  {
    void SpawnPrefab(
      List<IMyCubeGrid> resultList,
      string prefabName,
      Vector3D position,
      Vector3 forward,
      Vector3 up,
      Vector3 initialLinearVelocity = default (Vector3),
      Vector3 initialAngularVelocity = default (Vector3),
      string beaconName = null,
      SpawningOptions spawningOptions = SpawningOptions.None,
      bool updateSync = false,
      Action callback = null);

    void SpawnPrefab(
      List<IMyCubeGrid> resultList,
      string prefabName,
      Vector3D position,
      Vector3 forward,
      Vector3 up,
      Vector3 initialLinearVelocity = default (Vector3),
      Vector3 initialAngularVelocity = default (Vector3),
      string beaconName = null,
      SpawningOptions spawningOptions = SpawningOptions.None,
      long ownerId = 0,
      bool updateSync = false,
      Action callback = null);

    bool IsPathClear(Vector3D from, Vector3D to);

    bool IsPathClear(Vector3D from, Vector3D to, double halfSize);
  }
}
