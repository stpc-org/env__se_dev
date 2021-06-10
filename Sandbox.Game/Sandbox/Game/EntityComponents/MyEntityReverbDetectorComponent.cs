// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.MyEntityReverbDetectorComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using VRage.Audio;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
  [MyComponentBuilder(typeof (MyObjectBuilder_EntityReverbDetectorComponent), true)]
  public class MyEntityReverbDetectorComponent : MyEntityComponentBase
  {
    private const float RAYCAST_LENGTH = 25f;
    private const float INFINITY_PENALTY = 50f;
    private const float REVERB_THRESHOLD_SMALL = 3f;
    private const float REVERB_THRESHOLD_MEDIUM = 7f;
    private const float REVERB_THRESHOLD_LARGE = 12f;
    private const int REVERB_NO_OBSTACLE_LIMIT = 3;
    private static Vector3[] m_directions = new Vector3[26];
    private static bool m_systemInitialized = false;
    private static int m_currentReverbPreset = -1;
    private float[] m_detectedLengths;
    private MyEntityReverbDetectorComponent.ReverbDetectedType[] m_detectedObjects;
    private MyEntity m_entity;
    private int m_currentDirectionIndex;
    private bool m_componentInitialized;
    private bool m_sendInformationToAudio;
    private float m_averageHitLength;
    private int m_voxels;
    private int m_grids;

    public bool Initialized => this.m_componentInitialized && MyEntityReverbDetectorComponent.m_systemInitialized;

    public static string CurrentReverbPreset
    {
      get
      {
        if (MyEntityReverbDetectorComponent.m_currentReverbPreset == 1)
          return "Cave";
        return MyEntityReverbDetectorComponent.m_currentReverbPreset == 0 ? "Ship or station" : "None (reverb is off)";
      }
    }

    public void InitComponent(MyEntity entity, bool sendInformationToAudio)
    {
      int index1 = 0;
      if (!MyEntityReverbDetectorComponent.m_systemInitialized)
      {
        for (int index2 = -1; index2 <= 1; ++index2)
        {
          for (int index3 = -1; index3 <= 1; ++index3)
          {
            for (int index4 = -1; index4 <= 1; ++index4)
            {
              if (index2 != 0 || index4 != 0 || index3 != 0)
              {
                MyEntityReverbDetectorComponent.m_directions[index1] = Vector3.Normalize(new Vector3((float) index2, (float) index4, (float) index3));
                ++index1;
              }
            }
          }
        }
        MyEntityReverbDetectorComponent.m_systemInitialized = true;
      }
      this.m_entity = entity;
      this.m_detectedLengths = new float[MyEntityReverbDetectorComponent.m_directions.Length];
      this.m_detectedObjects = new MyEntityReverbDetectorComponent.ReverbDetectedType[MyEntityReverbDetectorComponent.m_directions.Length];
      for (int index2 = 0; index2 < MyEntityReverbDetectorComponent.m_directions.Length; ++index2)
      {
        this.m_detectedLengths[index2] = -1f;
        this.m_detectedObjects[index2] = MyEntityReverbDetectorComponent.ReverbDetectedType.None;
      }
      this.m_sendInformationToAudio = sendInformationToAudio && MyPerGameSettings.UseReverbEffect;
      this.m_componentInitialized = true;
    }

    public void UpdateParallel()
    {
      if (!this.Initialized || this.m_entity == null)
        return;
      Vector3 center = (Vector3) this.m_entity.PositionComp.WorldAABB.Center;
      Vector3 vector3 = center + MyEntityReverbDetectorComponent.m_directions[this.m_currentDirectionIndex] * 25f;
      LineD lineD = new LineD((Vector3D) center, (Vector3D) vector3);
      MyPhysics.HitInfo? nullable = MyPhysics.CastRay(lineD.From, lineD.To, 30);
      IMyEntity myEntity = (IMyEntity) null;
      Vector3D vector3D = Vector3D.Zero;
      Vector3 zero = Vector3.Zero;
      if (nullable.HasValue)
      {
        myEntity = (IMyEntity) (nullable.Value.HkHitInfo.GetHitEntity() as MyEntity);
        vector3D = nullable.Value.Position;
        MyPhysics.HitInfo hitInfo = nullable.Value;
      }
      if (myEntity != null)
      {
        this.m_detectedLengths[this.m_currentDirectionIndex] = Vector3.Distance(center, (Vector3) vector3D);
        this.m_detectedObjects[this.m_currentDirectionIndex] = myEntity is MyCubeGrid || myEntity is MyCubeBlock ? MyEntityReverbDetectorComponent.ReverbDetectedType.Grid : MyEntityReverbDetectorComponent.ReverbDetectedType.Voxel;
      }
      else
      {
        this.m_detectedLengths[this.m_currentDirectionIndex] = -1f;
        this.m_detectedObjects[this.m_currentDirectionIndex] = MyEntityReverbDetectorComponent.ReverbDetectedType.None;
      }
      ++this.m_currentDirectionIndex;
      if (this.m_currentDirectionIndex < MyEntityReverbDetectorComponent.m_directions.Length)
        return;
      this.m_currentDirectionIndex = 0;
      if (!this.m_sendInformationToAudio)
        return;
      this.m_averageHitLength = this.GetDetectedAverage();
      this.GetDetectedNumberOfObjects(out this.m_grids, out this.m_voxels);
    }

    public void Update()
    {
      if (this.m_currentDirectionIndex != 0 || !this.m_sendInformationToAudio)
        return;
      MyEntityReverbDetectorComponent.SetReverb(this.m_averageHitLength, this.Grids, this.Voxels);
    }

    public int Voxels => this.m_voxels;

    public int Grids => this.m_grids;

    private static void SetReverb(float distance, int grids, int voxels)
    {
      if (MyAudio.Static == null)
        return;
      MySession mySession = MySession.Static;
      bool flag = true;
      if (mySession.Settings.RealisticSound)
      {
        MyAtmosphereDetectorComponent atmosphereDetectorComp = mySession.LocalCharacter?.AtmosphereDetectorComp;
        flag = atmosphereDetectorComp != null && (atmosphereDetectorComp.InShipOrStation || atmosphereDetectorComp.InAtmosphere);
      }
      int num1 = MyEntityReverbDetectorComponent.m_directions.Length - grids - voxels;
      int num2 = -1;
      if (flag && (double) distance <= 12.0 && num1 <= 3)
        num2 = voxels <= grids ? 0 : 1;
      if (num2 == MyEntityReverbDetectorComponent.m_currentReverbPreset)
        return;
      MyEntityReverbDetectorComponent.m_currentReverbPreset = num2;
      if (MyEntityReverbDetectorComponent.m_currentReverbPreset <= -1)
      {
        MyAudio.Static.ApplyReverb = false;
        MySessionComponentPlanetAmbientSounds.SetAmbientOn();
      }
      else if (MyEntityReverbDetectorComponent.m_currentReverbPreset == 0)
      {
        MyAudio.Static.ApplyReverb = false;
        MySessionComponentPlanetAmbientSounds.SetAmbientOff();
      }
      else
      {
        MyAudio.Static.ApplyReverb = true;
        MySessionComponentPlanetAmbientSounds.SetAmbientOff();
      }
    }

    public float GetDetectedAverage(bool onlyDetected = false)
    {
      float num1 = 0.0f;
      int num2 = 0;
      for (int index = 0; index < this.m_detectedLengths.Length; ++index)
      {
        if ((double) this.m_detectedLengths[index] >= 0.0)
        {
          num1 += this.m_detectedLengths[index];
          ++num2;
        }
        else if (!onlyDetected)
          num1 += 50f;
      }
      return !onlyDetected ? num1 / (float) this.m_detectedLengths.Length : (num2 > 0 ? num1 / (float) num2 : 50f);
    }

    public int GetDetectedNumberOfObjects(
      MyEntityReverbDetectorComponent.ReverbDetectedType type = MyEntityReverbDetectorComponent.ReverbDetectedType.Grid)
    {
      int num = 0;
      for (int index = 0; index < this.m_detectedObjects.Length; ++index)
      {
        if (this.m_detectedObjects[index] == type)
          ++num;
      }
      return num;
    }

    public void GetDetectedNumberOfObjects(out int gridCount, out int voxelCount)
    {
      gridCount = voxelCount = 0;
      for (int index = 0; index < this.m_detectedObjects.Length; ++index)
      {
        switch (this.m_detectedObjects[index])
        {
          case MyEntityReverbDetectorComponent.ReverbDetectedType.None:
            continue;
          case MyEntityReverbDetectorComponent.ReverbDetectedType.Voxel:
            ++voxelCount;
            continue;
          case MyEntityReverbDetectorComponent.ReverbDetectedType.Grid:
            ++gridCount;
            continue;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }

    public override string ComponentTypeDebugString => "EntityReverbDetector";

    public enum ReverbDetectedType
    {
      None,
      Voxel,
      Grid,
    }

    private class Sandbox_Game_EntityComponents_MyEntityReverbDetectorComponent\u003C\u003EActor : IActivator, IActivator<MyEntityReverbDetectorComponent>
    {
      object IActivator.CreateInstance() => (object) new MyEntityReverbDetectorComponent();

      MyEntityReverbDetectorComponent IActivator<MyEntityReverbDetectorComponent>.CreateInstance() => new MyEntityReverbDetectorComponent();
    }
  }
}
