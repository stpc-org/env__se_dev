// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MyEnvironmentalParticleLogicFireFly
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.EnvironmentItems;
using Sandbox.Game.GameSystems;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders;
using VRage.Library.Utils;
using VRageMath;

namespace Sandbox.Game.World
{
  [MyEnvironmentalParticleLogicType(typeof (MyObjectBuilder_EnvironmentalParticleLogicFireFly), true)]
  public class MyEnvironmentalParticleLogicFireFly : MyEnvironmentalParticleLogic
  {
    private int m_particleSpawnInterval = 60;
    private float m_particleSpawnIntervalRandomness = 0.5f;
    private int m_particleSpawnCounter;
    private static int m_updateCounter;
    private const int m_killDeadParticlesInterval = 60;
    private List<HkBodyCollision> m_bodyCollisions = new List<HkBodyCollision>();
    private List<MyEnvironmentItems.ItemInfo> m_tmpItemInfos = new List<MyEnvironmentItems.ItemInfo>();

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      if (!MyFakes.ENABLE_PLANET_FIREFLIES || this.m_particleSpawnCounter-- > 0)
        return;
      this.m_particleSpawnCounter = 0;
      if (!(MySession.Static.ControlledEntity is MyEntity controlledEntity))
        return;
      MyEntity topMostParent = controlledEntity.GetTopMostParent((System.Type) null);
      if (topMostParent == null)
        return;
      try
      {
        this.m_particleSpawnCounter = (int) Math.Round((double) this.m_particleSpawnCounter + (double) this.m_particleSpawnCounter * (double) this.m_particleSpawnIntervalRandomness * ((double) MyRandom.Instance.NextFloat() * 2.0 - 1.0));
        if (((double) MyRandom.Instance.FloatNormal() + 3.0) / 6.0 > (double) this.m_particleDensity || (double) MyGravityProviderSystem.CalculateNaturalGravityInPoint(MySector.MainCamera.Position).Dot(MySector.DirectionToSunNormalized) <= 0.0)
          return;
        Vector3 vector3_1 = Vector3.Zero;
        if (topMostParent.Physics != null && MySession.Static.GetCameraControllerEnum() != MyCameraControllerEnum.Entity && MySession.Static.GetCameraControllerEnum() != MyCameraControllerEnum.ThirdPersonSpectator)
          vector3_1 = topMostParent.Physics.LinearVelocity;
        double num1 = (double) vector3_1.Length();
        MyCharacter myCharacter = topMostParent as MyCharacter;
        float num2 = MyGridPhysics.ShipMaxLinearVelocity();
        if (myCharacter != null && myCharacter.Physics != null && myCharacter.Physics.CharacterProxy != null)
          num2 = myCharacter.Physics.CharacterProxy.CharacterFlyingMaxLinearVelocity();
        Vector3 vector3_2 = Vector3.One * this.m_particleSpawnDistance;
        double num3 = (double) num2;
        if (num1 / num3 > 1.0)
        {
          Vector3 vector3_3 = vector3_2 + 10f * vector3_1 / num2;
        }
        Vector3D vector3D1 = MySector.MainCamera.Position + vector3_1;
        if (MyGamePruningStructure.GetClosestPlanet(MySector.MainCamera.Position) == null)
          return;
        Vector3D position = MySector.MainCamera.Position;
        Vector3D vector3D2 = new Vector3D();
        if ((uint) this.m_tmpItemInfos.Count <= 0U)
          return;
        MyEnvironmentalParticleLogic.MyEnvironmentalParticle particle = this.Spawn((Vector3) this.m_tmpItemInfos[MyRandom.Instance.Next(0, this.m_tmpItemInfos.Count - 1)].Transform.Position);
        if (particle == null)
          return;
        this.InitializePath(particle);
      }
      finally
      {
        this.m_bodyCollisions.Clear();
        this.m_tmpItemInfos.Clear();
      }
    }

    public override void Simulate()
    {
      base.Simulate();
      foreach (MyEnvironmentalParticleLogic.MyEnvironmentalParticle activeParticle in this.m_activeParticles)
      {
        Vector3 position = activeParticle.Position;
        Vector3D interpolatedPosition = this.GetInterpolatedPosition(activeParticle);
        activeParticle.Position = (Vector3) interpolatedPosition;
      }
    }

    public override void UpdateAfterSimulation()
    {
      if (MyEnvironmentalParticleLogicFireFly.m_updateCounter++ >= 60)
      {
        foreach (MyEnvironmentalParticleLogic.MyEnvironmentalParticle activeParticle in this.m_activeParticles)
        {
          if (this.IsInGridAABB((Vector3D) activeParticle.Position))
            activeParticle.Deactivate();
        }
        MyEnvironmentalParticleLogicFireFly.m_updateCounter = 0;
      }
      base.UpdateAfterSimulation();
    }

    public override void Draw()
    {
      base.Draw();
      double num1 = 0.0750000029802322;
      float thickness = (float) (num1 / 1.6599999666214);
      float length = (float) num1;
      foreach (MyEnvironmentalParticleLogic.MyEnvironmentalParticle activeParticle in this.m_activeParticles)
      {
        if (activeParticle.Active)
        {
          Vector4 color = activeParticle.Color;
          float num2 = (float) (MySandboxGame.TotalGamePlayTimeInMilliseconds - activeParticle.BirthTime) / (float) activeParticle.LifeTime;
          if ((double) num2 < 0.100000001490116)
            color = activeParticle.Color * num2;
          else if ((double) num2 > 0.899999976158142)
            color = activeParticle.Color * (1f - num2);
          Vector3D perpendicularVector = Vector3D.CalculatePerpendicularVector(-Vector3D.Normalize(activeParticle.Position - MySector.MainCamera.Position));
          MyTransparentGeometry.AddLineBillboard(activeParticle.Material, color, (Vector3D) activeParticle.Position, (Vector3) perpendicularVector, length, thickness);
        }
      }
    }

    private void InitializePath(
      MyEnvironmentalParticleLogic.MyEnvironmentalParticle particle)
    {
      MyEnvironmentalParticleLogicFireFly.PathData pathData = new MyEnvironmentalParticleLogicFireFly.PathData();
      if (pathData.PathPoints == null)
        pathData.PathPoints = new Vector3D[18];
      Vector3D vector3D1 = Vector3D.Normalize((Vector3D) MyGravityProviderSystem.CalculateNaturalGravityInPoint((Vector3D) particle.Position));
      pathData.PathPoints[1] = particle.Position - Vector3D.Normalize((Vector3D) MyGravityProviderSystem.CalculateNaturalGravityInPoint((Vector3D) particle.Position)) * (double) MyRandom.Instance.NextFloat() * 2.5;
      for (int index = 2; index < 17; ++index)
      {
        float num = 5f;
        Vector3D vector3D2 = Vector3D.Normalize(new Vector3D((double) MyRandom.Instance.NextFloat(), (double) MyRandom.Instance.NextFloat(), (double) MyRandom.Instance.NextFloat()) * 2.0 - Vector3D.One);
        pathData.PathPoints[index] = pathData.PathPoints[index - 1] + vector3D2 * ((double) MyRandom.Instance.NextFloat() + 1.0) * (double) num - vector3D1 / (double) index * (double) num;
      }
      pathData.PathPoints[0] = pathData.PathPoints[1] - vector3D1;
      pathData.PathPoints[17] = pathData.PathPoints[16] + Vector3D.Normalize(pathData.PathPoints[16] - pathData.PathPoints[15]);
      particle.UserData = (object) pathData;
    }

    private Vector3D GetInterpolatedPosition(
      MyEnvironmentalParticleLogic.MyEnvironmentalParticle particle)
    {
      Vector3D position = (Vector3D) particle.Position;
      if (particle.UserData == null)
        return position;
      double num1 = MathHelper.Clamp((double) (MySandboxGame.TotalGamePlayTimeInMilliseconds - particle.BirthTime) / (double) particle.LifeTime, 0.0, 1.0);
      int num2 = 14;
      int index = 1 + (int) (num1 * (double) num2);
      float num3 = (float) (num1 * (double) num2 - Math.Truncate(num1 * (double) num2));
      MyEnvironmentalParticleLogicFireFly.PathData pathData = (particle.UserData as MyEnvironmentalParticleLogicFireFly.PathData?).Value;
      Vector3D vector3D = Vector3D.CatmullRom(pathData.PathPoints[index - 1], pathData.PathPoints[index], pathData.PathPoints[index + 1], pathData.PathPoints[index + 2], (double) num3);
      if (!vector3D.IsValid())
        vector3D = (Vector3D) particle.Position;
      return vector3D;
    }

    private bool IsInGridAABB(Vector3D worldPosition)
    {
      BoundingSphereD boundingSphere = new BoundingSphereD(worldPosition, 0.100000001490116);
      List<MyEntity> myEntityList = (List<MyEntity>) null;
      try
      {
        myEntityList = Sandbox.Game.Entities.MyEntities.GetEntitiesInSphere(ref boundingSphere);
        foreach (MyEntity myEntity in myEntityList)
        {
          if (myEntity is MyCubeGrid)
            return true;
        }
      }
      finally
      {
        myEntityList?.Clear();
      }
      return false;
    }

    private struct PathData
    {
      public const int PathPointCount = 16;
      public Vector3D[] PathPoints;
    }
  }
}
