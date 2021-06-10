// Decompiled with JetBrains decompiler
// Type: Sandbox.AppCode.Game.TransparentGeometry.MySunWind
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.AppCode.Game.TransparentGeometry
{
  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, Priority = 1000)]
  internal class MySunWind : MySessionComponentBase
  {
    public static bool IsActive = false;
    public static bool IsVisible = true;
    public static Vector3D Position;
    private static Vector3D m_initialSunWindPosition;
    private static Vector3D m_directionFromSunNormalized;
    private static PlaneD m_planeMiddle;
    private static PlaneD m_planeFront;
    private static PlaneD m_planeBack;
    private static double m_distanceToSunWind;
    private static Vector3D m_positionOnCameraLine;
    private static int m_timeLastUpdate;
    private static float m_speed;
    private static Vector3D m_rightVector;
    private static Vector3D m_downVector;
    private static float m_strength;
    public static System.Type[] DoNotIgnoreTheseTypes = new System.Type[1]
    {
      typeof (MyVoxelMap)
    };
    private static MySunWind.MySunWindBillboard[][] m_largeBillboards;
    private static MySunWind.MySunWindBillboardSmall[][] m_smallBillboards;
    private static bool m_smallBillboardsStarted;
    private static List<IMyEntity> m_sunwindEntities = new List<IMyEntity>();
    private static List<HkBodyCollision> m_intersectionLst;
    private static List<MySunWind.MyEntityRayCastPair> m_rayCastQueue = new List<MySunWind.MyEntityRayCastPair>();
    private static int m_computedMaxDistances;
    private static float m_deltaTime;
    private int m_rayCastCounter;
    private List<MyPhysics.HitInfo> m_hitLst = new List<MyPhysics.HitInfo>();

    public override void LoadData()
    {
      MyLog.Default.WriteLine("MySunWind.LoadData() - START");
      MyLog.Default.IncreaseIndent();
      MySunWind.m_intersectionLst = new List<HkBodyCollision>();
      MySunWind.m_largeBillboards = new MySunWind.MySunWindBillboard[MySunWindConstants.LARGE_BILLBOARDS_SIZE.X][];
      for (int index1 = 0; index1 < MySunWindConstants.LARGE_BILLBOARDS_SIZE.X; ++index1)
      {
        MySunWind.m_largeBillboards[index1] = new MySunWind.MySunWindBillboard[MySunWindConstants.LARGE_BILLBOARDS_SIZE.Y];
        for (int index2 = 0; index2 < MySunWindConstants.LARGE_BILLBOARDS_SIZE.Y; ++index2)
        {
          MySunWind.m_largeBillboards[index1][index2] = new MySunWind.MySunWindBillboard();
          MySunWind.MySunWindBillboard sunWindBillboard = MySunWind.m_largeBillboards[index1][index2];
          sunWindBillboard.Radius = MyUtils.GetRandomFloat(20000f, 35000f);
          sunWindBillboard.InitialAngle = MyUtils.GetRandomRadian();
          sunWindBillboard.RotationSpeed = MyUtils.GetRandomSign() * MyUtils.GetRandomFloat(0.5f, 1.2f);
          sunWindBillboard.Color.X = MyUtils.GetRandomFloat(0.5f, 3f);
          sunWindBillboard.Color.Y = MyUtils.GetRandomFloat(0.5f, 1f);
          sunWindBillboard.Color.Z = MyUtils.GetRandomFloat(0.5f, 1f);
          sunWindBillboard.Color.W = MyUtils.GetRandomFloat(0.5f, 1f);
        }
      }
      MySunWind.m_smallBillboards = new MySunWind.MySunWindBillboardSmall[MySunWindConstants.SMALL_BILLBOARDS_SIZE.X][];
      for (int index1 = 0; index1 < MySunWindConstants.SMALL_BILLBOARDS_SIZE.X; ++index1)
      {
        MySunWind.m_smallBillboards[index1] = new MySunWind.MySunWindBillboardSmall[MySunWindConstants.SMALL_BILLBOARDS_SIZE.Y];
        for (int index2 = 0; index2 < MySunWindConstants.SMALL_BILLBOARDS_SIZE.Y; ++index2)
        {
          MySunWind.m_smallBillboards[index1][index2] = new MySunWind.MySunWindBillboardSmall();
          MySunWind.MySunWindBillboardSmall windBillboardSmall = MySunWind.m_smallBillboards[index1][index2];
          windBillboardSmall.Radius = MyUtils.GetRandomFloat(250f, 500f);
          windBillboardSmall.InitialAngle = MyUtils.GetRandomRadian();
          windBillboardSmall.RotationSpeed = MyUtils.GetRandomSign() * MyUtils.GetRandomFloat(1.4f, 3.5f);
          windBillboardSmall.Color.X = MyUtils.GetRandomFloat(0.5f, 1f);
          windBillboardSmall.Color.Y = MyUtils.GetRandomFloat(0.2f, 0.5f);
          windBillboardSmall.Color.Z = MyUtils.GetRandomFloat(0.2f, 0.5f);
          windBillboardSmall.Color.W = MyUtils.GetRandomFloat(0.1f, 0.5f);
          windBillboardSmall.TailBillboardsCount = MyUtils.GetRandomInt(8, 14);
          windBillboardSmall.TailBillboardsDistance = MyUtils.GetRandomFloat(300f, 450f);
          windBillboardSmall.RadiusScales = new float[windBillboardSmall.TailBillboardsCount];
          for (int index3 = 0; index3 < windBillboardSmall.TailBillboardsCount; ++index3)
            windBillboardSmall.RadiusScales[index3] = MyUtils.GetRandomFloat(0.7f, 1f);
        }
      }
      MyLog.Default.DecreaseIndent();
      MyLog.Default.WriteLine("MySunWind.LoadData() - END");
    }

    protected override void UnloadData()
    {
      MyLog.Default.WriteLine("MySunWind.UnloadData - START");
      MyLog.Default.IncreaseIndent();
      MySunWind.IsActive = false;
      MyLog.Default.DecreaseIndent();
      MyLog.Default.WriteLine("MySunWind.UnloadData - END");
    }

    public static void Start()
    {
      MySunWind.IsActive = true;
      MySunWind.m_smallBillboardsStarted = false;
      MySunWind.m_timeLastUpdate = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      Vector3D directionToSunNormalized = (Vector3D) MySector.DirectionToSunNormalized;
      MySunWind.m_initialSunWindPosition = directionToSunNormalized * 30000.0 / 2.0;
      MySunWind.m_directionFromSunNormalized = -directionToSunNormalized;
      MySunWind.StopCue();
      MySunWind.m_speed = MyUtils.GetRandomFloat(1300f, 1500f);
      MySunWind.m_strength = MyUtils.GetRandomFloat(0.0f, 1f);
      MySunWind.m_directionFromSunNormalized.CalculatePerpendicularVector(out MySunWind.m_rightVector);
      MySunWind.m_downVector = MyUtils.Normalize(Vector3D.Cross(MySunWind.m_directionFromSunNormalized, MySunWind.m_rightVector));
      MySunWind.StartBillboards();
      MySunWind.m_computedMaxDistances = 0;
      MySunWind.m_deltaTime = 0.0f;
      MySunWind.m_sunwindEntities.Clear();
    }

    public override void UpdateBeforeSimulation()
    {
      int num1 = 0;
      if (MySunWind.m_rayCastQueue.Count > 0 && this.m_rayCastCounter % 20 == 0)
      {
        while (num1 < 50 && MySunWind.m_rayCastQueue.Count > 0)
        {
          int randomInt = MyUtils.GetRandomInt(MySunWind.m_rayCastQueue.Count - 1);
          MyEntity entity = MySunWind.m_rayCastQueue[randomInt].Entity;
          MySunWind.MyEntityRayCastPair rayCast = MySunWind.m_rayCastQueue[randomInt];
          Vector3D position = MySunWind.m_rayCastQueue[randomInt].Position;
          MyParticleEffect particle = MySunWind.m_rayCastQueue[randomInt].Particle;
          if (entity is MyCubeGrid)
          {
            particle.Stop();
            MyCubeGrid myCubeGrid = entity as MyCubeGrid;
            MatrixD matrix = myCubeGrid.PositionComp.WorldMatrixNormalizedInv;
            if (myCubeGrid.BlocksDestructionEnabled)
              myCubeGrid.Physics.ApplyDeformation(6f, 3f, 3f, (Vector3) Vector3.Transform((Vector3) position, matrix), Vector3.Normalize(Vector3.Transform((Vector3) MySunWind.m_directionFromSunNormalized, matrix)), MyDamageType.Environment);
            MySunWind.m_rayCastQueue.RemoveAt(randomInt);
            this.m_hitLst.Clear();
            break;
          }
        }
      }
      ++this.m_rayCastCounter;
      if (!MySunWind.IsActive)
        return;
      float num2 = (float) (((double) MySandboxGame.TotalGamePlayTimeInMilliseconds - (double) MySunWind.m_timeLastUpdate) / 1000.0);
      MySunWind.m_timeLastUpdate = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      if (MySandboxGame.IsPaused)
        return;
      MySunWind.m_deltaTime += num2;
      float num3 = MySunWind.m_speed * MySunWind.m_deltaTime;
      if ((double) num3 >= 60000.0)
      {
        MySunWind.IsActive = false;
        MySunWind.StopCue();
      }
      else
      {
        Vector3D point = MySession.Static.LocalCharacter == null ? Vector3D.Zero : MySession.Static.LocalCharacter.Entity.WorldMatrix.Translation;
        MySunWind.m_planeMiddle = new PlaneD(MySunWind.m_initialSunWindPosition + MySunWind.m_directionFromSunNormalized * (double) num3, MySunWind.m_directionFromSunNormalized);
        MySunWind.m_distanceToSunWind = MySunWind.m_planeMiddle.DistanceToPoint(ref point);
        MySunWind.m_positionOnCameraLine = -MySunWind.m_directionFromSunNormalized * MySunWind.m_distanceToSunWind;
        Vector3D position1 = MySunWind.m_positionOnCameraLine + MySunWind.m_directionFromSunNormalized * 2000.0;
        Vector3D position2 = MySunWind.m_positionOnCameraLine + MySunWind.m_directionFromSunNormalized * -2000.0;
        MySunWind.m_planeFront = new PlaneD(position1, MySunWind.m_directionFromSunNormalized);
        MySunWind.m_planeBack = new PlaneD(position2, MySunWind.m_directionFromSunNormalized);
        MySunWind.m_planeFront.DistanceToPoint(ref point);
        MySunWind.m_planeBack.DistanceToPoint(ref point);
        int index1 = 0;
        while (index1 < MySunWind.m_sunwindEntities.Count)
        {
          if (MySunWind.m_sunwindEntities[index1].MarkedForClose)
            MySunWind.m_sunwindEntities.RemoveAtFast<IMyEntity>(index1);
          else
            ++index1;
        }
        Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(Matrix.CreateFromDir((Vector3) MySunWind.m_directionFromSunNormalized, (Vector3) MySunWind.m_downVector));
        Vector3 halfExtents = new Vector3(10000f, 10000f, 2000f);
        MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(position1 + MySunWind.m_directionFromSunNormalized * 2500.0, (Vector3D) halfExtents, fromRotationMatrix), (Color) Color.Red.ToVector3(), 1f, false, false);
        if (this.m_rayCastCounter == 120)
        {
          Vector3D translation = position1 + MySunWind.m_directionFromSunNormalized * 2500.0;
          MyPhysics.GetPenetrationsBox(ref halfExtents, ref translation, ref fromRotationMatrix, MySunWind.m_intersectionLst, 15);
          foreach (HkBodyCollision collision in MySunWind.m_intersectionLst)
          {
            IMyEntity collisionEntity = collision.GetCollisionEntity();
            if (!(collisionEntity is MyVoxelMap) && !MySunWind.m_sunwindEntities.Contains(collisionEntity))
              MySunWind.m_sunwindEntities.Add(collisionEntity);
          }
          MySunWind.m_intersectionLst.Clear();
          int num4;
          for (int index2 = 0; index2 < MySunWind.m_sunwindEntities.Count; index2 = num4 + 1)
          {
            IMyEntity sunwindEntity = MySunWind.m_sunwindEntities[index2];
            if (sunwindEntity is MyCubeGrid)
            {
              MyCubeGrid myCubeGrid = sunwindEntity as MyCubeGrid;
              BoundingBoxD worldAabb = myCubeGrid.PositionComp.WorldAABB;
              double num5 = (worldAabb.Center - worldAabb.Min).Length();
              double num6 = ((worldAabb.Center - worldAabb.Min) / MySunWind.m_rightVector).AbsMin();
              double num7 = ((worldAabb.Center - worldAabb.Min) / MySunWind.m_downVector).AbsMin();
              Vector3I vector3I = myCubeGrid.Max - myCubeGrid.Min;
              Math.Max(vector3I.X, Math.Max(vector3I.Y, vector3I.Z));
              ref readonly MatrixD local = ref myCubeGrid.PositionComp.WorldMatrixNormalizedInv;
              Vector3D vector3D1 = worldAabb.Center - num6 * MySunWind.m_rightVector - num7 * MySunWind.m_downVector;
              for (int index3 = 0; (double) index3 < num6 * 2.0; index3 += myCubeGrid.GridSizeEnum == MyCubeSize.Large ? 25 : 10)
              {
                for (int index4 = 0; (double) index4 < num7 * 2.0; index4 += myCubeGrid.GridSizeEnum == MyCubeSize.Large ? 25 : 10)
                {
                  Vector3D vector3D2 = vector3D1 + (double) index3 * MySunWind.m_rightVector + (double) index4 * MySunWind.m_downVector + num5 * MySunWind.m_directionFromSunNormalized;
                  Vector3 circleNormalized = MyUtils.GetRandomVector3CircleNormalized();
                  float randomFloat = MyUtils.GetRandomFloat(0.0f, myCubeGrid.GridSizeEnum == MyCubeSize.Large ? 10f : 5f);
                  Vector3D to = vector3D2 + (MySunWind.m_rightVector * (double) circleNormalized.X * (double) randomFloat + MySunWind.m_downVector * (double) circleNormalized.Z * (double) randomFloat);
                  LineD lineD = new LineD(to - MySunWind.m_directionFromSunNormalized * num5, to);
                  if (myCubeGrid.RayCastBlocks(lineD.From, lineD.To).HasValue)
                  {
                    lineD.From = to - MySunWind.m_directionFromSunNormalized * 1000.0;
                    MyPhysics.CastRay(lineD.From, lineD.To, this.m_hitLst);
                    ++this.m_rayCastCounter;
                    if (this.m_hitLst.Count == 0 || this.m_hitLst[0].HkHitInfo.GetHitEntity() != myCubeGrid.Components)
                    {
                      this.m_hitLst.Clear();
                    }
                    else
                    {
                      MyParticleEffect effect;
                      MyParticlesManager.TryCreateParticleEffect("Dummy", MatrixD.CreateWorld(this.m_hitLst[0].Position, Vector3D.Forward, Vector3D.Up), out effect);
                      MySunWind.m_rayCastQueue.Add(new MySunWind.MyEntityRayCastPair()
                      {
                        Entity = (MyEntity) myCubeGrid,
                        _Ray = lineD,
                        Position = this.m_hitLst[0].Position,
                        Particle = effect
                      });
                    }
                  }
                }
              }
              MySunWind.m_sunwindEntities.Remove((IMyEntity) myCubeGrid);
              num4 = index2 - 1;
            }
            else
            {
              MySunWind.m_sunwindEntities.Remove(sunwindEntity);
              num4 = index2 - 1;
            }
          }
          this.m_rayCastCounter = 0;
        }
        if (MySunWind.m_distanceToSunWind <= 10000.0)
          MySunWind.m_smallBillboardsStarted = true;
        MySunWind.ComputeMaxDistances();
        base.UpdateBeforeSimulation();
      }
    }

    public static bool IsActiveForHudWarning() => true;

    private static void StopCue()
    {
    }

    public static Vector4 GetSunColor()
    {
      float num = (float) (1.0 - MathHelper.Clamp(Math.Abs(MySunWind.m_distanceToSunWind) / 10000.0, 0.0, 1.0)) * MathHelper.Lerp(3f, 4f, MySunWind.m_strength);
      return new Vector4(MySector.SunProperties.EnvironmentLight.SunColorRaw, 1f) * (1f + num);
    }

    public static float GetParticleDustFieldAlpha() => (float) Math.Pow(MathHelper.Clamp(Math.Abs(MySunWind.m_distanceToSunWind) / 27000.0, 0.0, 1.0), 4.0);

    public override void Draw()
    {
      if (!MySunWind.IsActive || !MySunWind.IsVisible)
        return;
      float num = MySunWind.m_speed * MySunWind.m_deltaTime;
      Vector3 vector3 = (Vector3) (MySunWind.m_directionFromSunNormalized * (double) num);
      base.Draw();
    }

    private static void StartBillboards()
    {
      for (int index1 = 0; index1 < MySunWindConstants.LARGE_BILLBOARDS_SIZE.X; ++index1)
      {
        for (int index2 = 0; index2 < MySunWindConstants.LARGE_BILLBOARDS_SIZE.Y; ++index2)
        {
          MySunWind.MySunWindBillboard sunWindBillboard = MySunWind.m_largeBillboards[index1][index2];
          Vector3 vector3_1 = new Vector3(MyUtils.GetRandomFloat(-50f, 50f), MyUtils.GetRandomFloat(-50f, 50f), MyUtils.GetRandomFloat(-50f, 50f));
          Vector3 vector3_2 = new Vector3((float) (index1 - MySunWindConstants.LARGE_BILLBOARDS_SIZE_HALF.X) * 7500f, (float) (index2 - MySunWindConstants.LARGE_BILLBOARDS_SIZE_HALF.Y) * 7500f, (float) ((double) (index1 - MySunWindConstants.LARGE_BILLBOARDS_SIZE_HALF.X) * 7500.0 * 0.200000002980232));
          Vector3 vector3_3 = (Vector3) (MySunWind.m_initialSunWindPosition + MySunWind.m_rightVector * ((double) vector3_1.X + (double) vector3_2.X) + MySunWind.m_downVector * ((double) vector3_1.Y + (double) vector3_2.Y) + -1.0 * MySunWind.m_directionFromSunNormalized * ((double) vector3_1.Z + (double) vector3_2.Z));
          sunWindBillboard.InitialAbsolutePosition = vector3_3;
        }
      }
      Vector3D vector3D = MySession.Static.LocalCharacter == null ? Vector3D.Zero : MySession.Static.LocalCharacter.Entity.WorldMatrix.Translation - MySunWind.m_directionFromSunNormalized * 30000.0 / 3.0;
      for (int index1 = 0; index1 < MySunWindConstants.SMALL_BILLBOARDS_SIZE.X; ++index1)
      {
        for (int index2 = 0; index2 < MySunWindConstants.SMALL_BILLBOARDS_SIZE.Y; ++index2)
        {
          MySunWind.MySunWindBillboardSmall windBillboardSmall = MySunWind.m_smallBillboards[index1][index2];
          Vector2 vector2_1 = new Vector2(MyUtils.GetRandomFloat(-300f, 300f), MyUtils.GetRandomFloat(-300f, 300f));
          Vector2 vector2_2 = new Vector2((float) (index1 - MySunWindConstants.SMALL_BILLBOARDS_SIZE_HALF.X) * 350f, (float) (index2 - MySunWindConstants.SMALL_BILLBOARDS_SIZE_HALF.Y) * 350f);
          Vector3 vector3 = (Vector3) (vector3D + MySunWind.m_rightVector * ((double) vector2_1.X + (double) vector2_2.X) + MySunWind.m_downVector * ((double) vector2_1.Y + (double) vector2_2.Y));
          windBillboardSmall.InitialAbsolutePosition = vector3;
        }
      }
    }

    private static void ComputeMaxDistances()
    {
      int num = MySunWindConstants.SMALL_BILLBOARDS_SIZE.X * MySunWindConstants.SMALL_BILLBOARDS_SIZE.Y;
      if (MySunWind.m_computedMaxDistances >= num)
        return;
      for (int index1 = (int) ((double) num / 1.0 / 0.0166666675359011); MySunWind.m_computedMaxDistances < num && index1 > 0; --index1)
      {
        int index2 = MySunWind.m_computedMaxDistances % MySunWindConstants.SMALL_BILLBOARDS_SIZE.Y;
        int index3 = MySunWind.m_computedMaxDistances / MySunWindConstants.SMALL_BILLBOARDS_SIZE.X;
        MySunWind.ComputeMaxDistance(MySunWind.m_smallBillboards[index2][index3]);
        ++MySunWind.m_computedMaxDistances;
      }
    }

    private static void ComputeMaxDistance(MySunWind.MySunWindBillboardSmall billboard)
    {
      Vector3 vector3 = (Vector3) (MySunWind.m_directionFromSunNormalized * 30000.0);
      Vector3D vector3D = -MySunWind.m_directionFromSunNormalized * 30000.0;
      LineD line = new LineD(vector3 + billboard.InitialAbsolutePosition + vector3D, billboard.InitialAbsolutePosition + MySunWind.m_directionFromSunNormalized * 60000.0);
      MyIntersectionResultLineTriangleEx? intersectionWithLine = MyEntities.GetIntersectionWithLine(ref line, (MyEntity) null, (MyEntity) null);
      if (intersectionWithLine.HasValue)
        billboard.MaxDistance = intersectionWithLine.Value.Triangle.Distance - billboard.Radius;
      else
        billboard.MaxDistance = 60000f;
    }

    private class MySunWindBillboard
    {
      public Vector4 Color;
      public float Radius;
      public float InitialAngle;
      public float RotationSpeed;
      public Vector3 InitialAbsolutePosition;
    }

    private class MySunWindBillboardSmall : MySunWind.MySunWindBillboard
    {
      public float MaxDistance;
      public int TailBillboardsCount;
      public float TailBillboardsDistance;
      public float[] RadiusScales;
    }

    private struct MyEntityRayCastPair
    {
      public MyEntity Entity;
      public LineD _Ray;
      public Vector3D Position;
      public MyParticleEffect Particle;
    }
  }
}
