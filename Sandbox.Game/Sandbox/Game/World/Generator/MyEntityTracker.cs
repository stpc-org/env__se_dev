// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyEntityTracker
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Character;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
  public class MyEntityTracker
  {
    public BoundingSphereD BoundingVolume = new BoundingSphereD(Vector3D.PositiveInfinity, 0.0);

    public MyEntity Entity { get; private set; }

    public Vector3D CurrentPosition => this.Entity.PositionComp.WorldAABB.Center;

    public Vector3D LastPosition
    {
      get => this.BoundingVolume.Center;
      private set => this.BoundingVolume.Center = value;
    }

    public double Radius
    {
      get => this.BoundingVolume.Radius;
      set
      {
        this.Tolerance = MathHelper.Clamp(value / 2.0, 128.0, 512.0);
        this.BoundingVolume.Radius = value + this.Tolerance;
      }
    }

    public double Tolerance { get; private set; }

    public MyEntityTracker(MyEntity entity, double radius)
    {
      this.Entity = entity;
      this.Radius = radius;
    }

    public bool ShouldGenerate(bool trackStatic)
    {
      if (this.Entity.Closed || !this.Entity.Save || (this.CurrentPosition - this.LastPosition).Length() < this.Tolerance)
        return false;
      return this.Entity is MyCharacter || this.Entity.Physics == null || !this.Entity.Physics.IsStatic | trackStatic;
    }

    public void UpdateLastPosition() => this.LastPosition = this.CurrentPosition;

    public override string ToString() => this.Entity.ToString() + ", " + (object) this.BoundingVolume + ", " + (object) this.Tolerance;
  }
}
