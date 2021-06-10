// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.World.Environment.MyEnvironmentalParticleLogicSpace
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders;
using VRage.Library.Utils;
using VRageMath;
using VRageRender;

namespace SpaceEngineers.Game.World.Environment
{
  [MyEnvironmentalParticleLogicType(typeof (MyObjectBuilder_EnvironmentalParticleLogicSpace), true)]
  internal class MyEnvironmentalParticleLogicSpace : MyEnvironmentalParticleLogic
  {
    private int m_lastParticleSpawn;
    private float m_particlesLeftToSpawn;
    private bool m_isPlanetary;

    public MyEntity ControlledEntity { get; private set; }

    public Vector3 ControlledVelocity { get; private set; }

    public bool ShouldDrawParticles { get; private set; }

    public override void Init(MyObjectBuilder_EnvironmentalParticleLogic builder)
    {
      base.Init(builder);
      MyObjectBuilder_EnvironmentalParticleLogicSpace particleLogicSpace = builder as MyObjectBuilder_EnvironmentalParticleLogicSpace;
    }

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      this.ShouldDrawParticles = false;
      try
      {
        this.ControlledEntity = this.GetControlledEntity();
        if (this.ControlledEntity == null)
          return;
        MyPhysicsComponentBase physics = this.ControlledEntity.Physics;
        this.ControlledVelocity = physics != null ? physics.LinearVelocity : Vector3.Zero;
        if ((double) this.ControlledVelocity.LengthSquared() < 100.0 || this.IsInGridAABB())
          return;
        this.ShouldDrawParticles = true;
        float particleSpawnDistance = this.ParticleSpawnDistance;
        double num1 = Math.PI / 2.0;
        float num2 = 1f;
        Vector3 vector3_1 = this.ControlledVelocity - 8.5f * Vector3.Normalize(this.ControlledVelocity);
        float num3 = 4f * particleSpawnDistance * particleSpawnDistance * num2;
        this.m_isPlanetary = this.IsNearPlanet();
        if (this.m_isPlanetary && !MyFakes.ENABLE_STARDUST_ON_PLANET)
          return;
        this.m_particlesLeftToSpawn += (float) ((0.25 + (double) MyRandom.Instance.NextFloat() * 1.25) * (double) vector3_1.Length() * (double) num3 * (double) this.ParticleDensity * 16.0);
        if ((double) this.m_particlesLeftToSpawn < 1.0)
          return;
        double num4 = num1 / 2.0;
        double num5 = num4 + num1;
        double num6 = num4 + (double) MyRandom.Instance.NextFloat() * (num5 - num4);
        double num7 = num4 + (double) MyRandom.Instance.NextFloat() * (num5 - num4);
        float num8 = 6f;
        while ((double) this.m_particlesLeftToSpawn-- >= 1.0)
        {
          float num9 = (float) Math.PI / 180f;
          if (Math.Abs(num6 - Math.PI / 2.0) < (double) num8 * (double) num9 && Math.Abs(num7 - Math.PI / 2.0) < (double) num8 * (double) num9)
          {
            num6 += (double) Math.Sign(MyRandom.Instance.NextFloat()) * (double) num8 * (double) num9;
            num7 += (double) Math.Sign(MyRandom.Instance.NextFloat()) * (double) num8 * (double) num9;
          }
          float num10 = (float) Math.Sin(num7);
          float num11 = (float) Math.Cos(num7);
          float num12 = (float) Math.Sin(num6);
          float num13 = (float) Math.Cos(num6);
          Vector3 upVector = MySector.MainCamera.UpVector;
          Vector3 vector3_2 = Vector3.Normalize(vector3_1);
          Vector3 vector2 = Vector3.Cross(vector3_2, -upVector);
          if (Vector3.IsZero(vector2))
          {
            vector2 = Vector3.CalculatePerpendicularVector(vector3_2);
          }
          else
          {
            double num14 = (double) vector2.Normalize();
          }
          Vector3 vector3_3 = Vector3.Cross(vector3_2, vector2);
          this.Spawn((Vector3) (MySector.MainCamera.Position + particleSpawnDistance * (vector3_3 * num11 + vector2 * num10 * num13 + vector3_2 * num10 * num12)));
          this.m_lastParticleSpawn = MySandboxGame.TotalGamePlayTimeInMilliseconds;
        }
      }
      finally
      {
      }
    }

    public override void UpdateAfterSimulation()
    {
      if (!this.ShouldDrawParticles)
      {
        this.DeactivateAll();
        this.m_particlesLeftToSpawn = 0.0f;
      }
      base.UpdateAfterSimulation();
    }

    public override void Draw()
    {
      base.Draw();
      if (!this.ShouldDrawParticles)
        return;
      Vector3 directionNormalized = -Vector3.Normalize(this.ControlledVelocity);
      float num1 = 0.025f;
      float num2 = (float) MathHelper.Clamp((double) this.ControlledVelocity.Length() / 50.0, 0.0, 1.0);
      float num3 = 1f;
      float num4 = 1f;
      if (this.m_isPlanetary)
      {
        num3 = 1.5f;
        num4 = 3f;
      }
      foreach (MyEnvironmentalParticleLogic.MyEnvironmentalParticle activeParticle in this.m_activeParticles)
      {
        if (activeParticle.Active)
        {
          if (this.m_isPlanetary)
            MyTransparentGeometry.AddLineBillboard(activeParticle.MaterialPlanet, activeParticle.ColorPlanet, (Vector3D) activeParticle.Position, directionNormalized, num2 * num4, num1 * num3, MyBillboard.BlendTypeEnum.LDR);
          else
            MyTransparentGeometry.AddLineBillboard(activeParticle.Material, activeParticle.Color, (Vector3D) activeParticle.Position, directionNormalized, num2 * num4, num1 * num3, MyBillboard.BlendTypeEnum.LDR);
        }
      }
    }

    private bool IsInGridAABB()
    {
      bool flag = false;
      BoundingSphereD boundingSphere = new BoundingSphereD(MySector.MainCamera.Position, 0.100000001490116);
      List<MyEntity> myEntityList = (List<MyEntity>) null;
      try
      {
        myEntityList = MyEntities.GetEntitiesInSphere(ref boundingSphere);
        foreach (MyEntity myEntity in myEntityList)
        {
          if (myEntity is MyCubeGrid myCubeGrid && myCubeGrid.GridSizeEnum != MyCubeSize.Small)
          {
            flag = true;
            break;
          }
        }
      }
      finally
      {
        myEntityList?.Clear();
      }
      return flag;
    }

    private MyEntity GetControlledEntity()
    {
      if (!(MySession.Static.ControlledEntity is MyEntity myEntity) || MySession.Static.IsCameraUserControlledSpectator())
        return (MyEntity) null;
      switch (myEntity)
      {
        case MyRemoteControl myRemoteControl:
          myEntity = myRemoteControl.GetTopMostParent((Type) null);
          break;
        case MyCockpit myCockpit:
          myEntity = myCockpit.GetTopMostParent((Type) null);
          break;
      }
      return myEntity;
    }

    private bool IsNearPlanet() => this.ControlledEntity != null && !Vector3.IsZero(MyGravityProviderSystem.CalculateNaturalGravityInPoint(this.ControlledEntity.PositionComp.GetPosition()));
  }
}
