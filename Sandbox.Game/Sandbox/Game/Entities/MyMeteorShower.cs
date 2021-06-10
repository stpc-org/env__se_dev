// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyMeteorShower
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Audio;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
  [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
  [StaticEventOwner]
  internal class MyMeteorShower : MySessionComponentBase
  {
    private static readonly int WAVES_IN_SHOWER = 1;
    private static readonly double HORIZON_ANGLE_FROM_ZENITH_RATIO = Math.Sin(0.35);
    private static readonly double METEOR_BLUR_KOEF = 2.5;
    private static Vector3D m_tgtPos;
    private static Vector3D m_normalSun;
    private static Vector3D m_pltTgtDir;
    private static Vector3D m_mirrorDir;
    private static int m_waveCounter;
    private static List<MyEntity> m_meteorList = new List<MyEntity>();
    private static List<MyEntity> m_tmpEntityList = new List<MyEntity>();
    private static BoundingSphereD? m_currentTarget;
    private static List<BoundingSphereD> m_targetList = new List<BoundingSphereD>();
    private static int m_lastTargetCount;
    private static Vector3 m_downVector;
    private static Vector3 m_rightVector = Vector3.Zero;
    private static int m_meteorcount;
    private static List<MyCubeGrid> m_tmpHitGroup = new List<MyCubeGrid>();
    private static string[] m_enviromentHostilityName = new string[4]
    {
      "Safe",
      "MeteorWave",
      "MeteorWaveCataclysm",
      "MeteorWaveCataclysmUnreal"
    };
    private static Vector3D m_meteorHitPos;

    public static BoundingSphereD? CurrentTarget
    {
      get => MyMeteorShower.m_currentTarget;
      set => MyMeteorShower.m_currentTarget = value;
    }

    public override bool IsRequiredByGame => MyPerGameSettings.Game == GameEnum.SE_GAME;

    public override void LoadData()
    {
      MyMeteorShower.m_waveCounter = -1;
      MyMeteorShower.m_lastTargetCount = 0;
      base.LoadData();
    }

    protected override void UnloadData()
    {
      foreach (MyEntity meteor in MyMeteorShower.m_meteorList)
      {
        if (!meteor.MarkedForClose)
          meteor.Close();
      }
      MyMeteorShower.m_meteorList.Clear();
      MyMeteorShower.m_currentTarget = new BoundingSphereD?();
      MyMeteorShower.m_targetList.Clear();
      base.UnloadData();
    }

    public override void BeforeStart()
    {
      base.BeforeStart();
      if (!Sync.IsServer)
        return;
      MyGlobalEventBase myGlobalEventBase1 = (MyGlobalEvents.GetEventById(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GlobalEventBase), "MeteorWave")) ?? MyGlobalEvents.GetEventById(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GlobalEventBase), "MeteorWaveCataclysm"))) ?? MyGlobalEvents.GetEventById(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GlobalEventBase), "MeteorWaveCataclysmUnreal"));
      if (myGlobalEventBase1 == null && MySession.Static.EnvironmentHostility != MyEnvironmentHostilityEnum.SAFE && MyFakes.ENABLE_METEOR_SHOWERS)
      {
        MyGlobalEventBase globalEvent = MyGlobalEventFactory.CreateEvent(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GlobalEventBase), "MeteorWave"));
        globalEvent.SetActivationTime(MyMeteorShower.CalculateShowerTime(MySession.Static.EnvironmentHostility));
        MyGlobalEvents.AddGlobalEvent(globalEvent);
      }
      else
      {
        if (myGlobalEventBase1 == null)
          return;
        if (MySession.Static.EnvironmentHostility == MyEnvironmentHostilityEnum.SAFE || !MyFakes.ENABLE_METEOR_SHOWERS)
        {
          myGlobalEventBase1.Enabled = false;
        }
        else
        {
          myGlobalEventBase1.Enabled = true;
          MyEnvironmentHostilityEnum? nullable1 = MySession.Static.PreviousEnvironmentHostility;
          if (!nullable1.HasValue)
            return;
          int environmentHostility1 = (int) MySession.Static.EnvironmentHostility;
          nullable1 = MySession.Static.PreviousEnvironmentHostility;
          int num1 = (int) nullable1.Value;
          if (environmentHostility1 == num1)
            return;
          MyGlobalEventBase myGlobalEventBase2 = myGlobalEventBase1;
          int environmentHostility2 = (int) MySession.Static.EnvironmentHostility;
          nullable1 = MySession.Static.PreviousEnvironmentHostility;
          int num2 = (int) nullable1.Value;
          TimeSpan activationTime = myGlobalEventBase1.ActivationTime;
          TimeSpan showerTime = MyMeteorShower.CalculateShowerTime((MyEnvironmentHostilityEnum) environmentHostility2, (MyEnvironmentHostilityEnum) num2, activationTime);
          myGlobalEventBase2.SetActivationTime(showerTime);
          MySession mySession = MySession.Static;
          nullable1 = new MyEnvironmentHostilityEnum?();
          MyEnvironmentHostilityEnum? nullable2 = nullable1;
          mySession.PreviousEnvironmentHostility = nullable2;
        }
      }
    }

    private static void MeteorWaveInternal(object senderEvent)
    {
      if (MySession.Static.EnvironmentHostility == MyEnvironmentHostilityEnum.SAFE)
      {
        ((MyGlobalEventBase) senderEvent).Enabled = false;
      }
      else
      {
        if (!Sync.IsServer)
          return;
        ++MyMeteorShower.m_waveCounter;
        if (MyMeteorShower.m_waveCounter == 0)
        {
          MyMeteorShower.ClearMeteorList();
          if (MyMeteorShower.m_targetList.Count == 0)
          {
            MyMeteorShower.GetTargets();
            if (MyMeteorShower.m_targetList.Count == 0)
            {
              MyMeteorShower.m_waveCounter = MyMeteorShower.WAVES_IN_SHOWER + 1;
              MyMeteorShower.RescheduleEvent(senderEvent);
              return;
            }
          }
          MyMeteorShower.m_currentTarget = new BoundingSphereD?(MyMeteorShower.m_targetList.ElementAt<BoundingSphereD>(MyUtils.GetRandomInt(MyMeteorShower.m_targetList.Count - 1)));
          MyMultiplayer.RaiseStaticEvent<BoundingSphereD?>((Func<IMyEventOwner, Action<BoundingSphereD?>>) (x => new Action<BoundingSphereD?>(MyMeteorShower.UpdateShowerTarget)), MyMeteorShower.m_currentTarget);
          MyMeteorShower.m_targetList.Remove(MyMeteorShower.m_currentTarget.Value);
          MyMeteorShower.m_meteorcount = (int) (Math.Pow(MyMeteorShower.m_currentTarget.Value.Radius, 2.0) * Math.PI / 6000.0);
          MyMeteorShower.m_meteorcount /= MySession.Static.EnvironmentHostility == MyEnvironmentHostilityEnum.CATACLYSM || MySession.Static.EnvironmentHostility == MyEnvironmentHostilityEnum.CATACLYSM_UNREAL ? 1 : 8;
          MyMeteorShower.m_meteorcount = MathHelper.Clamp(MyMeteorShower.m_meteorcount, 1, 30);
        }
        MyMeteorShower.RescheduleEvent(senderEvent);
        MyMeteorShower.CheckTargetValid();
        if (MyMeteorShower.m_waveCounter < 0)
          return;
        MyMeteorShower.StartWave();
      }
    }

    private static void StartWave()
    {
      if (!MyMeteorShower.m_currentTarget.HasValue)
        return;
      Vector3 correctedDirection = MyMeteorShower.GetCorrectedDirection(MySector.DirectionToSunNormalized);
      MyMeteorShower.SetupDirVectors(correctedDirection);
      float randomFloat1 = MyUtils.GetRandomFloat((float) Math.Min(2, MyMeteorShower.m_meteorcount - 3), (float) (MyMeteorShower.m_meteorcount + 3));
      Vector3 circleNormalized1 = MyUtils.GetRandomVector3CircleNormalized();
      float randomFloat2 = MyUtils.GetRandomFloat(0.0f, 1f);
      Vector3D vector3D1 = (Vector3D) (circleNormalized1.X * MyMeteorShower.m_rightVector + circleNormalized1.Z * MyMeteorShower.m_downVector);
      Vector3D vector3D2 = MyMeteorShower.m_currentTarget.Value.Center + Math.Pow((double) randomFloat2, 0.699999988079071) * MyMeteorShower.m_currentTarget.Value.Radius * vector3D1 * MyMeteorShower.METEOR_BLUR_KOEF;
      Vector3D vector3D3 = -Vector3D.Normalize((Vector3D) MyGravityProviderSystem.CalculateNaturalGravityInPoint(vector3D2));
      if (vector3D3 != Vector3D.Zero)
      {
        MyPhysics.HitInfo? nullable = MyPhysics.CastRay(vector3D2 + vector3D3 * 3000.0, vector3D2, 15);
        if (nullable.HasValue)
          vector3D2 = nullable.Value.Position;
      }
      MyMeteorShower.m_meteorHitPos = vector3D2;
      for (int index = 0; (double) index < (double) randomFloat1; ++index)
      {
        Vector3 circleNormalized2 = MyUtils.GetRandomVector3CircleNormalized();
        float randomFloat3 = MyUtils.GetRandomFloat(0.0f, 1f);
        Vector3D vector3D4 = (Vector3D) (circleNormalized2.X * MyMeteorShower.m_rightVector + circleNormalized2.Z * MyMeteorShower.m_downVector);
        vector3D2 += Math.Pow((double) randomFloat3, 0.699999988079071) * MyMeteorShower.m_currentTarget.Value.Radius * vector3D4;
        Vector3 vector3 = correctedDirection * (float) (2000 + 100 * index);
        Vector3 circleNormalized3 = MyUtils.GetRandomVector3CircleNormalized();
        Vector3D vector3D5 = (Vector3D) (circleNormalized3.X * MyMeteorShower.m_rightVector + circleNormalized3.Z * MyMeteorShower.m_downVector);
        Vector3D position = vector3D2 + vector3 + Math.Tan((double) MyUtils.GetRandomFloat(0.0f, 0.1745329f)) * vector3D5;
        MyMeteorShower.m_meteorList.Add(MyMeteor.SpawnRandom(position, Vector3.Normalize(vector3D2 - position)));
      }
      MyMeteorShower.m_rightVector = Vector3.Zero;
    }

    private static Vector3 GetCorrectedDirection(Vector3 direction)
    {
      Vector3 v = direction;
      if (!MyMeteorShower.m_currentTarget.HasValue)
        return v;
      Vector3D center = MyMeteorShower.m_currentTarget.Value.Center;
      MyMeteorShower.m_tgtPos = center;
      if (!MyGravityProviderSystem.IsPositionInNaturalGravity(center))
        return v;
      Vector3D vector3D1 = -Vector3D.Normalize((Vector3D) MyGravityProviderSystem.CalculateNaturalGravityInPoint(center));
      Vector3D vector3D2 = Vector3D.Normalize(Vector3D.Cross(vector3D1, (Vector3D) v));
      Vector3D vector3D3 = Vector3D.Normalize(Vector3D.Cross(vector3D2, vector3D1));
      MyMeteorShower.m_mirrorDir = vector3D3;
      MyMeteorShower.m_pltTgtDir = vector3D1;
      MyMeteorShower.m_normalSun = vector3D2;
      double num = vector3D1.Dot(v);
      if (num < -MyMeteorShower.HORIZON_ANGLE_FROM_ZENITH_RATIO)
        return (Vector3) Vector3D.Reflect((Vector3D) -v, vector3D3);
      if (num >= MyMeteorShower.HORIZON_ANGLE_FROM_ZENITH_RATIO)
        return v;
      MatrixD fromAxisAngle = MatrixD.CreateFromAxisAngle(vector3D2, -Math.Asin(MyMeteorShower.HORIZON_ANGLE_FROM_ZENITH_RATIO));
      return (Vector3) Vector3D.Transform(vector3D3, fromAxisAngle);
    }

    public static void StartDebugWave(Vector3 pos)
    {
      MyMeteorShower.m_currentTarget = new BoundingSphereD?(new BoundingSphereD((Vector3D) pos, 100.0));
      MyMeteorShower.m_meteorcount = (int) (Math.Pow(MyMeteorShower.m_currentTarget.Value.Radius, 2.0) * Math.PI / 3000.0);
      MyMeteorShower.m_meteorcount /= MySession.Static.EnvironmentHostility == MyEnvironmentHostilityEnum.CATACLYSM || MySession.Static.EnvironmentHostility == MyEnvironmentHostilityEnum.CATACLYSM_UNREAL ? 1 : 8;
      MyMeteorShower.m_meteorcount = MathHelper.Clamp(MyMeteorShower.m_meteorcount, 1, 40);
      MyMeteorShower.StartWave();
    }

    public override void Draw()
    {
      base.Draw();
      if (!MyDebugDrawSettings.DEBUG_DRAW_METEORITS_DIRECTIONS)
        return;
      Vector3D correctedDirection = (Vector3D) MyMeteorShower.GetCorrectedDirection(MySector.DirectionToSunNormalized);
      MyRenderProxy.DebugDrawPoint(MyMeteorShower.m_meteorHitPos, Color.White, false);
      MyRenderProxy.DebugDrawText3D(MyMeteorShower.m_meteorHitPos, "Hit position", Color.White, 0.5f, false);
      MyRenderProxy.DebugDrawLine3D(MyMeteorShower.m_tgtPos, MyMeteorShower.m_tgtPos + 10f * MySector.DirectionToSunNormalized, Color.Yellow, Color.Yellow, false);
      MyRenderProxy.DebugDrawText3D(MyMeteorShower.m_tgtPos + 10f * MySector.DirectionToSunNormalized, "Sun direction (sd)", Color.Yellow, 0.5f, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM);
      MyRenderProxy.DebugDrawLine3D(MyMeteorShower.m_tgtPos, MyMeteorShower.m_tgtPos + 10.0 * correctedDirection, Color.Red, Color.Red, false);
      MyRenderProxy.DebugDrawText3D(MyMeteorShower.m_tgtPos + 10.0 * correctedDirection, "Current meteorits direction (cd)", Color.Red, 0.5f, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
      if (!MyGravityProviderSystem.IsPositionInNaturalGravity(MyMeteorShower.m_tgtPos))
        return;
      MyRenderProxy.DebugDrawLine3D(MyMeteorShower.m_tgtPos, MyMeteorShower.m_tgtPos + 10.0 * MyMeteorShower.m_normalSun, Color.Blue, Color.Blue, false);
      MyRenderProxy.DebugDrawText3D(MyMeteorShower.m_tgtPos + 10.0 * MyMeteorShower.m_normalSun, "Perpendicular to sd and n0 ", Color.Blue, 0.5f, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
      MyRenderProxy.DebugDrawLine3D(MyMeteorShower.m_tgtPos, MyMeteorShower.m_tgtPos + 10.0 * MyMeteorShower.m_pltTgtDir, Color.Green, Color.Green, false);
      MyRenderProxy.DebugDrawText3D(MyMeteorShower.m_tgtPos + 10.0 * MyMeteorShower.m_pltTgtDir, "Dir from center of planet to target (n0)", Color.Green, 0.5f, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
      MyRenderProxy.DebugDrawLine3D(MyMeteorShower.m_tgtPos, MyMeteorShower.m_tgtPos + 10.0 * MyMeteorShower.m_mirrorDir, Color.Purple, Color.Purple, false);
      MyRenderProxy.DebugDrawText3D(MyMeteorShower.m_tgtPos + 10.0 * MyMeteorShower.m_mirrorDir, "Horizon in plane n0 and sd (ho)", Color.Purple, 0.5f, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
    }

    private static void CheckTargetValid()
    {
      if (!MyMeteorShower.m_currentTarget.HasValue)
        return;
      MyMeteorShower.m_tmpEntityList.Clear();
      BoundingSphereD boundingSphere = MyMeteorShower.m_currentTarget.Value;
      MyMeteorShower.m_tmpEntityList = MyEntities.GetEntitiesInSphere(ref boundingSphere);
      if (MyMeteorShower.m_tmpEntityList.OfType<MyCubeGrid>().ToList<MyCubeGrid>().Count == 0)
        MyMeteorShower.m_waveCounter = -1;
      if (MyMeteorShower.m_waveCounter >= 0 && MyMusicController.Static != null)
      {
        foreach (MyEntity tmpEntity in MyMeteorShower.m_tmpEntityList)
        {
          if (tmpEntity is MyCharacter && MySession.Static != null && tmpEntity as MyCharacter == MySession.Static.LocalCharacter)
          {
            MyMusicController.Static.MeteorShowerIncoming();
            break;
          }
        }
      }
      MyMeteorShower.m_tmpEntityList.Clear();
    }

    private static void RescheduleEvent(object senderEvent)
    {
      if (MyMeteorShower.m_waveCounter > MyMeteorShower.WAVES_IN_SHOWER)
      {
        TimeSpan showerTime = MyMeteorShower.CalculateShowerTime(MySession.Static.EnvironmentHostility);
        MyGlobalEvents.RescheduleEvent((MyGlobalEventBase) senderEvent, showerTime);
        MyMeteorShower.m_waveCounter = -1;
        MyMeteorShower.m_currentTarget = new BoundingSphereD?();
        MyMultiplayer.RaiseStaticEvent<BoundingSphereD?>((Func<IMyEventOwner, Action<BoundingSphereD?>>) (x => new Action<BoundingSphereD?>(MyMeteorShower.UpdateShowerTarget)), MyMeteorShower.m_currentTarget);
      }
      else
      {
        TimeSpan time = TimeSpan.FromSeconds((double) MyMeteorShower.m_meteorcount / 5.0 + (double) MyUtils.GetRandomFloat(2f, 5f));
        MyGlobalEvents.RescheduleEvent((MyGlobalEventBase) senderEvent, time);
      }
    }

    public static double GetActivationTime(
      MyEnvironmentHostilityEnum hostility,
      double defaultMinMinutes,
      double defaultMaxMinutes)
    {
      MyGlobalEventDefinition eventDefinition = MyDefinitionManager.Static.GetEventDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GlobalEventBase), MyMeteorShower.m_enviromentHostilityName[(int) hostility]));
      if (eventDefinition != null)
      {
        TimeSpan timeSpan;
        if (eventDefinition.MinActivationTime.HasValue)
        {
          timeSpan = eventDefinition.MinActivationTime.Value;
          defaultMinMinutes = timeSpan.TotalMinutes;
        }
        if (eventDefinition.MaxActivationTime.HasValue)
        {
          timeSpan = eventDefinition.MaxActivationTime.Value;
          defaultMaxMinutes = timeSpan.TotalMinutes;
        }
      }
      return MyUtils.GetRandomDouble(defaultMinMinutes, defaultMaxMinutes);
    }

    public static TimeSpan CalculateShowerTime(MyEnvironmentHostilityEnum hostility)
    {
      double num = 5.0;
      switch (hostility)
      {
        case MyEnvironmentHostilityEnum.NORMAL:
          num = MathHelper.Max(0.4, MyMeteorShower.GetActivationTime(hostility, 16.0, 24.0) / (double) MathHelper.Max(1f, (float) MyMeteorShower.m_lastTargetCount));
          break;
        case MyEnvironmentHostilityEnum.CATACLYSM:
          num = MathHelper.Max(0.4, MyMeteorShower.GetActivationTime(hostility, 1.0, 1.5) / (double) MathHelper.Max(1f, (float) MyMeteorShower.m_lastTargetCount));
          break;
        case MyEnvironmentHostilityEnum.CATACLYSM_UNREAL:
          num = MyMeteorShower.GetActivationTime(hostility, 0.100000001490116, 0.300000011920929);
          break;
      }
      return TimeSpan.FromMinutes(num);
    }

    private static double GetMaxActivationTime(MyEnvironmentHostilityEnum environment)
    {
      double num = 0.0;
      switch (environment)
      {
        case MyEnvironmentHostilityEnum.NORMAL:
          num = 24.0;
          break;
        case MyEnvironmentHostilityEnum.CATACLYSM:
          num = 1.5;
          break;
        case MyEnvironmentHostilityEnum.CATACLYSM_UNREAL:
          num = 0.300000011920929;
          break;
      }
      MyGlobalEventDefinition eventDefinition = MyDefinitionManager.Static.GetEventDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GlobalEventBase), MyMeteorShower.m_enviromentHostilityName[(int) environment]));
      if (eventDefinition != null && eventDefinition.MaxActivationTime.HasValue)
        num = eventDefinition.MaxActivationTime.Value.TotalMinutes;
      return num;
    }

    public static TimeSpan CalculateShowerTime(
      MyEnvironmentHostilityEnum newHostility,
      MyEnvironmentHostilityEnum oldHostility,
      TimeSpan oldTime)
    {
      double totalMinutes = oldTime.TotalMinutes;
      double num = 1.0;
      if (oldHostility != MyEnvironmentHostilityEnum.SAFE)
        num = totalMinutes / MyMeteorShower.GetMaxActivationTime(oldHostility);
      return TimeSpan.FromMinutes(num * MyMeteorShower.GetMaxActivationTime(newHostility));
    }

    private static void GetTargets()
    {
      List<MyCubeGrid> list = MyEntities.GetEntities().OfType<MyCubeGrid>().ToList<MyCubeGrid>();
      for (int index = 0; index < list.Count; ++index)
      {
        if ((list[index].Max - list[index].Min + Vector3I.One).Size < 16 || !MySessionComponentTriggerSystem.Static.IsAnyTriggerActive((MyEntity) list[index]))
        {
          list.RemoveAt(index);
          --index;
        }
      }
      while (list.Count > 0)
      {
        MyCubeGrid myCubeGrid1 = list[MyUtils.GetRandomInt(list.Count - 1)];
        MyMeteorShower.m_tmpHitGroup.Add(myCubeGrid1);
        list.Remove(myCubeGrid1);
        BoundingSphereD worldVolume = myCubeGrid1.PositionComp.WorldVolume;
        bool flag = true;
        while (flag)
        {
          flag = false;
          foreach (MyCubeGrid myCubeGrid2 in MyMeteorShower.m_tmpHitGroup)
            worldVolume.Include(myCubeGrid2.PositionComp.WorldVolume);
          MyMeteorShower.m_tmpHitGroup.Clear();
          worldVolume.Radius += 10.0;
          for (int index = 0; index < list.Count; ++index)
          {
            if (list[index].PositionComp.WorldVolume.Intersects(worldVolume))
            {
              flag = true;
              MyMeteorShower.m_tmpHitGroup.Add(list[index]);
              list.RemoveAt(index);
              --index;
            }
          }
        }
        worldVolume.Radius += 150.0;
        MyMeteorShower.m_targetList.Add(worldVolume);
      }
      MyMeteorShower.m_lastTargetCount = MyMeteorShower.m_targetList.Count;
    }

    private static void ClearMeteorList() => MyMeteorShower.m_meteorList.Clear();

    private static void SetupDirVectors(Vector3 direction)
    {
      if (!(MyMeteorShower.m_rightVector == Vector3.Zero))
        return;
      direction.CalculatePerpendicularVector(out MyMeteorShower.m_rightVector);
      MyMeteorShower.m_downVector = MyUtils.Normalize(Vector3.Cross(direction, MyMeteorShower.m_rightVector));
    }

    [MyGlobalEventHandler(typeof (MyObjectBuilder_GlobalEventBase), "MeteorWave")]
    public static void MeteorWave(object senderEvent) => MyMeteorShower.MeteorWaveInternal(senderEvent);

    [Event(null, 506)]
    [Reliable]
    [Broadcast]
    private static void UpdateShowerTarget([Serialize(MyObjectFlags.DefaultZero)] BoundingSphereD? target)
    {
      if (target.HasValue)
        MyMeteorShower.CurrentTarget = new BoundingSphereD?(new BoundingSphereD(target.Value.Center, target.Value.Radius));
      else
        MyMeteorShower.CurrentTarget = new BoundingSphereD?();
    }

    protected sealed class UpdateShowerTarget\u003C\u003ESystem_Nullable`1\u003CVRageMath_BoundingSphereD\u003E : ICallSite<IMyEventOwner, BoundingSphereD?, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in BoundingSphereD? target,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyMeteorShower.UpdateShowerTarget(target);
      }
    }
  }
}
