// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MySphericalNaturalGravityComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRageMath;

namespace Sandbox.Game.Entities
{
  public class MySphericalNaturalGravityComponent : MyGravityProviderComponent
  {
    private const double GRAVITY_LIMIT_STRENGTH = 0.05;
    private readonly double m_minRadius;
    private readonly double m_maxRadius;
    private readonly double m_falloff;
    private readonly double m_intensity;
    private float m_gravityLimit;
    private float m_gravityLimitSq;

    public Vector3D Position { get; private set; }

    public MySphericalNaturalGravityComponent(
      double minRadius,
      double maxRadius,
      double falloff,
      double intensity)
    {
      this.m_minRadius = minRadius;
      this.m_maxRadius = maxRadius;
      this.m_falloff = falloff;
      this.m_intensity = intensity;
      double num1 = intensity;
      double num2 = maxRadius;
      double y = 1.0 / falloff;
      this.GravityLimit = (float) (num2 * Math.Pow(num1 / 0.05, y));
    }

    public override bool IsWorking => true;

    public float GravityLimit
    {
      get => this.m_gravityLimit;
      private set
      {
        this.m_gravityLimitSq = value * value;
        this.m_gravityLimit = value;
      }
    }

    public float GravityLimitSq
    {
      get => this.m_gravityLimitSq;
      private set
      {
        this.m_gravityLimitSq = value;
        this.m_gravityLimit = (float) Math.Sqrt((double) value);
      }
    }

    public override bool IsPositionInRange(Vector3D worldPoint) => (this.Position - worldPoint).LengthSquared() <= (double) this.m_gravityLimitSq;

    public override void GetProxyAABB(out BoundingBoxD aabb)
    {
      BoundingSphereD sphere = new BoundingSphereD(this.Position, (double) this.GravityLimit);
      BoundingBoxD.CreateFromSphere(ref sphere, out aabb);
    }

    public override Vector3 GetWorldGravity(Vector3D worldPoint) => this.GetWorldGravityNormalized(in worldPoint) * 9.81f * this.GetGravityMultiplier(worldPoint);

    public override float GetGravityMultiplier(Vector3D worldPoint)
    {
      double num1 = (this.Position - worldPoint).Length();
      if (num1 > (double) this.m_gravityLimit)
        return 0.0f;
      float num2 = 1f;
      if (num1 > this.m_maxRadius)
        num2 = (float) Math.Pow(num1 / this.m_maxRadius, -this.m_falloff);
      else if (num1 < this.m_minRadius)
      {
        num2 = (float) (num1 / this.m_minRadius);
        if ((double) num2 < 0.00999999977648258)
          num2 = 0.01f;
      }
      return num2 * (float) this.m_intensity;
    }

    public Vector3 GetWorldGravityNormalized(in Vector3D worldPoint)
    {
      Vector3 vector3 = (Vector3) (this.Position - worldPoint);
      double num = (double) vector3.Normalize();
      return vector3;
    }

    public override string ComponentTypeDebugString => this.GetType().Name;

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      this.Position = this.Entity.PositionComp.GetPosition();
    }

    private class Sandbox_Game_Entities_MySphericalNaturalGravityComponent\u003C\u003EActor
    {
    }
  }
}
