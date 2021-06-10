// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MySpawnGroupDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_SpawnGroupDefinition), null)]
  public class MySpawnGroupDefinition : MyDefinitionBase
  {
    public float Frequency;
    private float m_spawnRadius;
    private bool m_initialized;
    public bool IsPirate;
    public bool IsEncounter;
    public bool IsCargoShip;
    public bool ReactorsOn;
    public List<MySpawnGroupDefinition.SpawnGroupPrefab> Prefabs = new List<MySpawnGroupDefinition.SpawnGroupPrefab>();
    public List<MySpawnGroupDefinition.SpawnGroupVoxel> Voxels = new List<MySpawnGroupDefinition.SpawnGroupVoxel>();

    public float SpawnRadius
    {
      get
      {
        if (!this.m_initialized)
          this.ReloadPrefabs();
        return this.m_spawnRadius;
      }
      private set => this.m_spawnRadius = value;
    }

    public bool IsValid => (double) this.Frequency != 0.0 && (double) this.m_spawnRadius != 0.0 && (uint) this.Prefabs.Count > 0U;

    protected override void Init(MyObjectBuilder_DefinitionBase baseBuilder)
    {
      base.Init(baseBuilder);
      MyObjectBuilder_SpawnGroupDefinition spawnGroupDefinition = baseBuilder as MyObjectBuilder_SpawnGroupDefinition;
      this.Frequency = spawnGroupDefinition.Frequency;
      if ((double) this.Frequency == 0.0)
      {
        MySandboxGame.Log.WriteLine("Spawn group initialization: spawn group has zero frequency");
      }
      else
      {
        this.SpawnRadius = 0.0f;
        BoundingSphere boundingSphere = new BoundingSphere(Vector3.Zero, float.MinValue);
        this.Prefabs.Clear();
        foreach (MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab prefab in spawnGroupDefinition.Prefabs)
        {
          MySpawnGroupDefinition.SpawnGroupPrefab spawnGroupPrefab = new MySpawnGroupDefinition.SpawnGroupPrefab();
          spawnGroupPrefab.Position = prefab.Position;
          spawnGroupPrefab.SubtypeId = prefab.SubtypeId;
          spawnGroupPrefab.BeaconText = prefab.BeaconText;
          spawnGroupPrefab.Speed = prefab.Speed;
          spawnGroupPrefab.ResetOwnership = prefab.ResetOwnership;
          spawnGroupPrefab.PlaceToGridOrigin = prefab.PlaceToGridOrigin;
          spawnGroupPrefab.Behaviour = prefab.Behaviour;
          spawnGroupPrefab.BehaviourActivationDistance = prefab.BehaviourActivationDistance;
          if (MyDefinitionManager.Static.GetPrefabDefinition(spawnGroupPrefab.SubtypeId) == null)
          {
            MySandboxGame.Log.WriteLine("Spawn group initialization: Could not get prefab " + spawnGroupPrefab.SubtypeId);
            return;
          }
          this.Prefabs.Add(spawnGroupPrefab);
        }
        this.Voxels.Clear();
        if (spawnGroupDefinition.Voxels != null)
        {
          foreach (MyObjectBuilder_SpawnGroupDefinition.SpawnGroupVoxel voxel in spawnGroupDefinition.Voxels)
            this.Voxels.Add(new MySpawnGroupDefinition.SpawnGroupVoxel()
            {
              Offset = voxel.Offset,
              StorageName = voxel.StorageName,
              CenterOffset = voxel.CenterOffset
            });
        }
        this.SpawnRadius = boundingSphere.Radius + 5f;
        this.IsEncounter = spawnGroupDefinition.IsEncounter;
        this.IsCargoShip = spawnGroupDefinition.IsCargoShip;
        this.IsPirate = spawnGroupDefinition.IsPirate;
        this.ReactorsOn = spawnGroupDefinition.ReactorsOn;
      }
    }

    public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
    {
      MyObjectBuilder_SpawnGroupDefinition objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_SpawnGroupDefinition;
      objectBuilder.Frequency = this.Frequency;
      objectBuilder.Prefabs = new MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab[this.Prefabs.Count];
      int index1 = 0;
      foreach (MySpawnGroupDefinition.SpawnGroupPrefab prefab in this.Prefabs)
      {
        objectBuilder.Prefabs[index1] = new MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab();
        objectBuilder.Prefabs[index1].BeaconText = prefab.BeaconText;
        objectBuilder.Prefabs[index1].SubtypeId = prefab.SubtypeId;
        objectBuilder.Prefabs[index1].Position = prefab.Position;
        objectBuilder.Prefabs[index1].Speed = prefab.Speed;
        objectBuilder.Prefabs[index1].ResetOwnership = prefab.ResetOwnership;
        objectBuilder.Prefabs[index1].PlaceToGridOrigin = prefab.PlaceToGridOrigin;
        objectBuilder.Prefabs[index1].Behaviour = prefab.Behaviour;
        objectBuilder.Prefabs[index1].BehaviourActivationDistance = prefab.BehaviourActivationDistance;
        ++index1;
      }
      objectBuilder.Voxels = new MyObjectBuilder_SpawnGroupDefinition.SpawnGroupVoxel[this.Voxels.Count];
      int index2 = 0;
      foreach (MySpawnGroupDefinition.SpawnGroupVoxel voxel in this.Voxels)
      {
        objectBuilder.Voxels[index2] = new MyObjectBuilder_SpawnGroupDefinition.SpawnGroupVoxel();
        objectBuilder.Voxels[index2].Offset = voxel.Offset;
        objectBuilder.Voxels[index2].CenterOffset = voxel.CenterOffset;
        objectBuilder.Voxels[index2].StorageName = voxel.StorageName;
        ++index2;
      }
      objectBuilder.IsCargoShip = this.IsCargoShip;
      objectBuilder.IsEncounter = this.IsEncounter;
      objectBuilder.IsPirate = this.IsPirate;
      objectBuilder.ReactorsOn = this.ReactorsOn;
      return (MyObjectBuilder_DefinitionBase) objectBuilder;
    }

    public void ReloadPrefabs()
    {
      BoundingSphere boundingSphere1 = new BoundingSphere(Vector3.Zero, float.MinValue);
      float val1 = 0.0f;
      foreach (MySpawnGroupDefinition.SpawnGroupPrefab prefab in this.Prefabs)
      {
        MyPrefabDefinition prefabDefinition = MyDefinitionManager.Static.GetPrefabDefinition(prefab.SubtypeId);
        if (prefabDefinition == null)
        {
          MySandboxGame.Log.WriteLine("Spawn group initialization: Could not get prefab " + prefab.SubtypeId);
          return;
        }
        BoundingSphere boundingSphere2 = prefabDefinition.BoundingSphere;
        boundingSphere2.Center += prefab.Position;
        boundingSphere1.Include(boundingSphere2);
        if (prefabDefinition.CubeGrids != null)
        {
          foreach (MyObjectBuilder_CubeGrid cubeGrid in prefabDefinition.CubeGrids)
          {
            float cubeSize = MyDefinitionManager.Static.GetCubeSize(cubeGrid.GridSizeEnum);
            val1 = Math.Max(val1, 2f * cubeSize);
          }
        }
      }
      this.SpawnRadius = boundingSphere1.Radius + val1;
      this.m_initialized = true;
    }

    public struct SpawnGroupPrefab
    {
      public Vector3 Position;
      public string SubtypeId;
      public string BeaconText;
      public float Speed;
      public bool ResetOwnership;
      public bool PlaceToGridOrigin;
      public string Behaviour;
      public float BehaviourActivationDistance;
    }

    public struct SpawnGroupVoxel
    {
      public Vector3 Offset;
      public bool CenterOffset;
      public string StorageName;
    }

    private class Sandbox_Definitions_MySpawnGroupDefinition\u003C\u003EActor : IActivator, IActivator<MySpawnGroupDefinition>
    {
      object IActivator.CreateInstance() => (object) new MySpawnGroupDefinition();

      MySpawnGroupDefinition IActivator<MySpawnGroupDefinition>.CreateInstance() => new MySpawnGroupDefinition();
    }
  }
}
