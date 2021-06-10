// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Navigation.MyTargetSteering
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage.Game.Entity;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Navigation
{
  public class MyTargetSteering : MySteeringBase
  {
    protected Vector3D? m_target;
    protected MyEntity m_entity;
    private const float m_slowdownRadius = 0.0f;
    private const float m_maxSpeed = 1f;
    private float m_capsuleRadiusSq = 1f;
    private const float m_capsuleHeight = 0.5f;
    private const float m_capsuleOffset = -0.8f;

    public bool TargetSet => this.m_target.HasValue;

    public bool Flying { get; private set; }

    public Vector3D? TargetWorld
    {
      get
      {
        if (this.m_entity == null || this.m_entity.MarkedForClose)
          return this.m_target;
        return this.m_target.HasValue ? new Vector3D?(Vector3D.Transform(this.m_target.Value, this.m_entity.WorldMatrix)) : new Vector3D?();
      }
    }

    public MyTargetSteering(MyBotNavigation navigation)
      : base(navigation, 1f)
      => this.m_target = new Vector3D?();

    public override string GetName() => "Target steering";

    public void SetTarget(
      Vector3D target,
      float radius = 1f,
      MyEntity relativeEntity = null,
      float weight = 1f,
      bool fly = false)
    {
      if (relativeEntity == null || relativeEntity.MarkedForClose)
      {
        this.m_entity = (MyEntity) null;
        this.m_target = new Vector3D?(target);
      }
      else
      {
        this.m_entity = relativeEntity;
        this.m_target = new Vector3D?(Vector3D.Transform(target, this.m_entity.PositionComp.WorldMatrixNormalizedInv));
      }
      this.m_capsuleRadiusSq = radius * radius;
      this.Weight = weight;
      this.Flying = fly;
    }

    public void UnsetTarget() => this.m_target = new Vector3D?();

    public bool TargetReached()
    {
      if (!this.TargetWorld.HasValue)
        return false;
      Vector3D target = this.TargetWorld.Value;
      return this.TargetReached(ref target, this.m_capsuleRadiusSq);
    }

    protected Vector3D CapsuleCenter() => this.Parent.PositionAndOrientation.Translation + this.Parent.PositionAndOrientation.Up * -0.300000011920929 * 0.5;

    public double TargetDistanceSq(ref Vector3D target)
    {
      Vector3D up = this.Parent.PositionAndOrientation.Up;
      Vector3D vector1 = this.Parent.PositionAndOrientation.Translation + up * -0.800000011920929;
      double result1;
      Vector3D.Dot(ref vector1, ref up, out result1);
      double result2;
      Vector3D.Dot(ref target, ref up, out result2);
      double num = result2 - result1;
      if (num >= 0.5)
        vector1 += up;
      else if (num >= 0.0)
        vector1 += up * num;
      double result3;
      Vector3D.DistanceSquared(ref target, ref vector1, out result3);
      return result3;
    }

    public bool TargetReached(ref Vector3D target, float radiusSq) => this.TargetDistanceSq(ref target) < (double) radiusSq;

    public override void AccumulateCorrection(ref Vector3 correctionHint, ref float weight)
    {
      if (this.m_entity != null && this.m_entity.MarkedForClose)
        this.m_entity = (MyEntity) null;
      Vector3 currentMovement;
      Vector3 wantedMovement;
      this.GetMovements(out currentMovement, out wantedMovement);
      correctionHint += (wantedMovement - currentMovement) * this.Weight;
      weight += this.Weight;
    }

    public override void Update()
    {
      base.Update();
      if (!this.TargetReached())
        return;
      this.UnsetTarget();
    }

    private void GetMovements(out Vector3 currentMovement, out Vector3 wantedMovement)
    {
      Vector3D? targetWorld = this.TargetWorld;
      Vector3? nullable = targetWorld.HasValue ? new Vector3?((Vector3) targetWorld.GetValueOrDefault()) : new Vector3?();
      currentMovement = this.Parent.ForwardVector * this.Parent.Speed;
      if (nullable.HasValue)
      {
        wantedMovement = (Vector3) (nullable.Value - this.Parent.PositionAndOrientation.Translation);
        float num = wantedMovement.Length();
        if ((double) num > 0.0)
          wantedMovement = wantedMovement * 1f / num;
        else
          wantedMovement = wantedMovement * 1f / 0.0f;
      }
      else
        wantedMovement = Vector3.Zero;
    }

    public override void DebugDraw()
    {
      MatrixD positionAndOrientation = this.Parent.PositionAndOrientation;
      Vector3D translation = positionAndOrientation.Translation;
      positionAndOrientation = this.Parent.PositionAndOrientation;
      Vector3D vector3D1 = positionAndOrientation.Up * -0.800000011920929;
      Vector3D p0 = translation + vector3D1;
      Vector3D vector3D2 = p0 + this.Parent.PositionAndOrientation.Up * 0.5;
      Vector3D pointFrom = (p0 + vector3D2) * 0.5;
      Vector3 currentMovement;
      Vector3 wantedMovement;
      this.GetMovements(out currentMovement, out wantedMovement);
      Vector3D? targetWorld = this.TargetWorld;
      if (targetWorld.HasValue)
      {
        MyRenderProxy.DebugDrawLine3D(pointFrom, targetWorld.Value, Color.White, Color.White, true);
        MyRenderProxy.DebugDrawSphere(targetWorld.Value, 0.05f, (Color) Color.White.ToVector3(), depthRead: false);
        MyRenderProxy.DebugDrawCapsule(p0, vector3D2, (float) Math.Sqrt((double) this.m_capsuleRadiusSq), Color.Yellow, false);
      }
      MyRenderProxy.DebugDrawLine3D(vector3D2, vector3D2 + wantedMovement, Color.Red, Color.Red, false);
      MyRenderProxy.DebugDrawLine3D(vector3D2, vector3D2 + currentMovement, Color.Green, Color.Green, false);
    }
  }
}
