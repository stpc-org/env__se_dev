// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MySpawnPrefabProperties
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using System.Collections.Generic;
using VRage.Game.ModAPI;
using VRageMath;

namespace Sandbox.Game.World
{
  internal class MySpawnPrefabProperties
  {
    internal string PrefabName { get; set; }

    internal List<MyCubeGrid> ResultList { get; set; }

    internal Vector3D Position { get; set; }

    internal Vector3 Forward { get; set; }

    internal Vector3 Up { get; set; }

    internal Vector3 InitialLinearVelocity { get; set; }

    internal Vector3 InitialAngularVelocity { get; set; }

    internal string BeaconName { get; set; }

    internal string EntityName { get; set; }

    internal SpawningOptions SpawningOptions { get; set; }

    internal bool UpdateSync { get; set; }

    internal long OwnerId { get; set; }

    internal Vector3 Color { get; set; }
  }
}
