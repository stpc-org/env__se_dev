// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyDirtyRegion
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Concurrent;
using VRage.Collections;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
  public class MyDirtyRegion
  {
    public ConcurrentQueue<MyCube> PartsToRemove = new ConcurrentQueue<MyCube>();
    public ConcurrentCachingHashSet<Vector3I> Cubes = new ConcurrentCachingHashSet<Vector3I>();

    public void AddCube(Vector3I pos) => this.Cubes.Add(pos);

    public void AddCubeRegion(Vector3I min, Vector3I max)
    {
      Vector3I vector3I;
      for (vector3I.X = min.X; vector3I.X <= max.X; ++vector3I.X)
      {
        for (vector3I.Y = min.Y; vector3I.Y <= max.Y; ++vector3I.Y)
        {
          for (vector3I.Z = min.Z; vector3I.Z <= max.Z; ++vector3I.Z)
            this.Cubes.Add(vector3I);
        }
      }
    }

    public bool IsDirty
    {
      get
      {
        this.Cubes.ApplyChanges();
        return this.Cubes.Count > 0 || !this.PartsToRemove.IsEmpty;
      }
    }

    public int Count => this.Cubes.Count + this.PartsToRemove.Count;

    public void Clear() => this.Cubes.Clear();
  }
}
