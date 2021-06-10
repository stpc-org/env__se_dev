// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MyPrefabManager
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.World
{
  public class MyPrefabManager : IMyPrefabManager
  {
    private static FastResourceLock m_builderLock = new FastResourceLock();
    private readonly Vector3 MASK_COLOR = new Vector3(1f, 0.2f, 0.55f);
    public static EventWaitHandle FinishedProcessingGrids = (EventWaitHandle) new AutoResetEvent(false);
    public static int PendingGrids;
    public static readonly MyPrefabManager Static;
    private static List<MyPhysics.HitInfo> m_raycastHits = new List<MyPhysics.HitInfo>();

    static MyPrefabManager() => MyPrefabManager.Static = new MyPrefabManager();

    public static void SavePrefab(string prefabName, MyObjectBuilder_EntityBase entity)
    {
      string path = Path.Combine(MyFileSystem.ContentPath, Path.Combine("Data", "Prefabs", prefabName + ".sbc"));
      MyObjectBuilder_PrefabDefinition newObject1 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_PrefabDefinition>();
      newObject1.Id = (SerializableDefinitionId) new MyDefinitionId(new MyObjectBuilderType(typeof (MyObjectBuilder_PrefabDefinition)), prefabName);
      newObject1.CubeGrid = (MyObjectBuilder_CubeGrid) entity;
      MyObjectBuilder_Definitions newObject2 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Definitions>();
      newObject2.Prefabs = new MyObjectBuilder_PrefabDefinition[1];
      newObject2.Prefabs[0] = newObject1;
      MyObjectBuilder_Definitions builderDefinitions = newObject2;
      MyObjectBuilderSerializer.SerializeXML(path, false, (MyObjectBuilder_Base) builderDefinitions);
    }

    public static MyObjectBuilder_PrefabDefinition SavePrefab(
      string prefabName,
      List<MyObjectBuilder_CubeGrid> copiedPrefab)
    {
      string path = Path.Combine(MyFileSystem.ContentPath, Path.Combine("Data", "Prefabs", prefabName + ".sbc"));
      return MyPrefabManager.SavePrefabToPath(prefabName, path, copiedPrefab);
    }

    public static MyObjectBuilder_PrefabDefinition SavePrefabToPath(
      string prefabName,
      string path,
      List<MyObjectBuilder_CubeGrid> copiedPrefab)
    {
      MyObjectBuilder_PrefabDefinition newObject1 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_PrefabDefinition>();
      newObject1.Id = (SerializableDefinitionId) new MyDefinitionId(new MyObjectBuilderType(typeof (MyObjectBuilder_PrefabDefinition)), prefabName);
      newObject1.CubeGrids = copiedPrefab.Select<MyObjectBuilder_CubeGrid, MyObjectBuilder_CubeGrid>((Func<MyObjectBuilder_CubeGrid, MyObjectBuilder_CubeGrid>) (x => (MyObjectBuilder_CubeGrid) x.Clone())).ToArray<MyObjectBuilder_CubeGrid>();
      foreach (MyObjectBuilder_CubeGrid cubeGrid in newObject1.CubeGrids)
      {
        foreach (MyObjectBuilder_CubeBlock cubeBlock in cubeGrid.CubeBlocks)
        {
          cubeBlock.Owner = 0L;
          cubeBlock.BuiltBy = 0L;
        }
      }
      MyObjectBuilder_Definitions newObject2 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Definitions>();
      newObject2.Prefabs = new MyObjectBuilder_PrefabDefinition[1];
      newObject2.Prefabs[0] = newObject1;
      MyObjectBuilderSerializer.SerializeXML(path, false, (MyObjectBuilder_Base) newObject2);
      return newObject1;
    }

    public MyObjectBuilder_CubeGrid[] GetGridPrefab(string prefabName)
    {
      MyPrefabDefinition prefabDefinition = MyDefinitionManager.Static.GetPrefabDefinition(prefabName);
      if (prefabDefinition == null)
        return (MyObjectBuilder_CubeGrid[]) null;
      MyObjectBuilder_CubeGrid[] cubeGrids = prefabDefinition.CubeGrids;
      Sandbox.Game.Entities.MyEntities.RemapObjectBuilderCollection((IEnumerable<MyObjectBuilder_EntityBase>) cubeGrids);
      return cubeGrids;
    }

    public void AddShipPrefab(
      string prefabName,
      Matrix? worldMatrix = null,
      long ownerId = 0,
      bool spawnAtOrigin = false)
    {
      List<MyCubeGrid> results = new List<MyCubeGrid>();
      string prefabName1 = prefabName;
      Matrix matrix = worldMatrix ?? Matrix.Identity;
      MatrixD worldMatrix1 = (MatrixD) ref matrix;
      long num1 = ownerId;
      int num2 = spawnAtOrigin ? 128 : 0;
      long ownerId1 = num1;
      MyPrefabManager.CreateGridsData createGridsData = new MyPrefabManager.CreateGridsData(results, prefabName1, worldMatrix1, (SpawningOptions) num2, ownerId: ownerId1);
      Interlocked.Increment(ref MyPrefabManager.PendingGrids);
      Parallel.Start(new Action<WorkData>(createGridsData.CallCreateGridsFromPrefab), new Action<WorkData>(createGridsData.OnGridsCreated), (WorkData) createGridsData);
    }

    public void AddShipPrefabRandomPosition(
      string prefabName,
      Vector3D position,
      float distance,
      long ownerId = 0,
      bool spawnAtOrigin = false)
    {
      MyPrefabDefinition prefabDefinition = MyDefinitionManager.Static.GetPrefabDefinition(prefabName);
      if (prefabDefinition == null)
        return;
      BoundingSphereD sphere = new BoundingSphereD(Vector3D.Zero, (double) prefabDefinition.BoundingSphere.Radius);
      int num1 = 0;
      Vector3 position1;
      MyEntity intersectionWithSphere;
      do
      {
        position1 = (Vector3) (position + MyUtils.GetRandomVector3Normalized() * MyUtils.GetRandomFloat(0.5f, 1f) * distance);
        sphere.Center = (Vector3D) position1;
        intersectionWithSphere = Sandbox.Game.Entities.MyEntities.GetIntersectionWithSphere(ref sphere);
        ++num1;
        if (num1 % 8 == 0)
          distance += (float) sphere.Radius / 2f;
      }
      while (intersectionWithSphere != null);
      List<MyCubeGrid> results = new List<MyCubeGrid>();
      string prefabName1 = prefabName;
      Matrix world = Matrix.CreateWorld(position1, Vector3.Forward, Vector3.Up);
      MatrixD worldMatrix = (MatrixD) ref world;
      long num2 = ownerId;
      int num3 = spawnAtOrigin ? 128 : 0;
      long ownerId1 = num2;
      MyPrefabManager.CreateGridsData createGridsData = new MyPrefabManager.CreateGridsData(results, prefabName1, worldMatrix, (SpawningOptions) num3, ownerId: ownerId1);
      Interlocked.Increment(ref MyPrefabManager.PendingGrids);
      Parallel.Start(new Action<WorkData>(createGridsData.CallCreateGridsFromPrefab), new Action<WorkData>(createGridsData.OnGridsCreated), (WorkData) createGridsData);
    }

    private void CreateGridsFromPrefab(
      List<MyCubeGrid> results,
      string prefabName,
      MatrixD worldMatrix,
      SpawningOptions spawningOptions,
      bool ignoreMemoryLimits,
      long ownerId,
      Stack<Action> callbacks)
    {
      MyPrefabDefinition prefabDefinition = MyDefinitionManager.Static.GetPrefabDefinition(prefabName);
      if (prefabDefinition == null)
        return;
      MyObjectBuilder_CubeGrid[] objectBuilderCubeGridArray = new MyObjectBuilder_CubeGrid[prefabDefinition.CubeGrids.Length];
      if (objectBuilderCubeGridArray.Length == 0)
        return;
      int num = 0;
      MatrixD matrix = MatrixD.Identity;
      MyPositionAndOrientation positionAndOrientation;
      for (int index = 0; index < objectBuilderCubeGridArray.Length; ++index)
      {
        objectBuilderCubeGridArray[index] = (MyObjectBuilder_CubeGrid) prefabDefinition.CubeGrids[index].Clone();
        if (objectBuilderCubeGridArray[index].CubeBlocks.Count > num)
        {
          num = objectBuilderCubeGridArray[index].CubeBlocks.Count;
          MatrixD matrixD;
          if (!objectBuilderCubeGridArray[index].PositionAndOrientation.HasValue)
          {
            matrixD = MatrixD.Identity;
          }
          else
          {
            positionAndOrientation = objectBuilderCubeGridArray[index].PositionAndOrientation.Value;
            matrixD = positionAndOrientation.GetMatrix();
          }
          matrix = matrixD;
        }
      }
      Sandbox.Game.Entities.MyEntities.RemapObjectBuilderCollection((IEnumerable<MyObjectBuilder_EntityBase>) objectBuilderCubeGridArray);
      MatrixD world;
      if (spawningOptions.HasFlag((Enum) SpawningOptions.UseGridOrigin))
      {
        Vector3D vector3D = Vector3D.Zero;
        if (prefabDefinition.CubeGrids[0].PositionAndOrientation.HasValue)
          vector3D = (Vector3D) prefabDefinition.CubeGrids[0].PositionAndOrientation.Value.Position;
        world = MatrixD.CreateWorld(-vector3D, Vector3D.Forward, Vector3D.Up);
      }
      else
        world = MatrixD.CreateWorld((Vector3D) -prefabDefinition.BoundingSphere.Center, Vector3D.Forward, Vector3D.Up);
      bool ignoreMemoryLimits1 = Sandbox.Game.Entities.MyEntities.IgnoreMemoryLimits;
      Sandbox.Game.Entities.MyEntities.IgnoreMemoryLimits = ignoreMemoryLimits;
      bool flag = spawningOptions.HasFlag((Enum) SpawningOptions.SetAuthorship);
      for (int index = 0; index < objectBuilderCubeGridArray.Length; ++index)
      {
        if (ownerId != 0L)
        {
          foreach (MyObjectBuilder_CubeBlock cubeBlock in objectBuilderCubeGridArray[index].CubeBlocks)
          {
            cubeBlock.Owner = ownerId;
            cubeBlock.ShareMode = MyOwnershipShareModeEnum.Faction;
            if (flag)
              cubeBlock.BuiltBy = ownerId;
          }
        }
        MatrixD newWorldMatrix;
        if (spawningOptions.HasFlag((Enum) SpawningOptions.UseOnlyWorldMatrix))
        {
          if (objectBuilderCubeGridArray[index].PositionAndOrientation.HasValue)
          {
            positionAndOrientation = objectBuilderCubeGridArray[index].PositionAndOrientation.Value;
            newWorldMatrix = positionAndOrientation.GetMatrix() * MatrixD.Invert(matrix) * worldMatrix;
            objectBuilderCubeGridArray[index].PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation(newWorldMatrix));
          }
          else
          {
            newWorldMatrix = worldMatrix;
            objectBuilderCubeGridArray[index].PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation(worldMatrix));
          }
        }
        else
        {
          MatrixD matrix1;
          if (!objectBuilderCubeGridArray[index].PositionAndOrientation.HasValue)
          {
            matrix1 = MatrixD.Identity;
          }
          else
          {
            positionAndOrientation = objectBuilderCubeGridArray[index].PositionAndOrientation.Value;
            matrix1 = positionAndOrientation.GetMatrix();
          }
          newWorldMatrix = MatrixD.Multiply(matrix1, MatrixD.Multiply(world, worldMatrix));
          objectBuilderCubeGridArray[index].PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation(newWorldMatrix));
        }
        MyEntity entity = Sandbox.Game.Entities.MyEntities.CreateFromObjectBuilder((MyObjectBuilder_EntityBase) objectBuilderCubeGridArray[index], false);
        if (entity is MyCubeGrid myCubeGrid && myCubeGrid.CubeBlocks.Count > 0)
        {
          results.Add(myCubeGrid);
          callbacks.Push((Action) (() => this.SetPrefabPosition(entity, newWorldMatrix)));
        }
      }
      Sandbox.Game.Entities.MyEntities.IgnoreMemoryLimits = ignoreMemoryLimits1;
    }

    private void SetPrefabPosition(MyEntity entity, MatrixD newWorldMatrix)
    {
      if (!(entity is MyCubeGrid myCubeGrid))
        return;
      myCubeGrid.PositionComp.SetWorldMatrix(ref newWorldMatrix, forceUpdate: true);
      if (!MyPerGameSettings.Destruction || !myCubeGrid.IsStatic || (myCubeGrid.Physics == null || myCubeGrid.Physics.Shape == null))
        return;
      myCubeGrid.Physics.Shape.RecalculateConnectionsToWorld(myCubeGrid.GetBlocks());
    }

    public void SpawnPrefab(
      string prefabName,
      Vector3D position,
      Vector3 forward,
      Vector3 up,
      Vector3 initialLinearVelocity = default (Vector3),
      Vector3 initialAngularVelocity = default (Vector3),
      string beaconName = null,
      string entityName = null,
      SpawningOptions spawningOptions = SpawningOptions.None,
      long ownerId = 0,
      bool updateSync = false,
      Stack<Action> callbacks = null)
    {
      if (callbacks == null)
        callbacks = new Stack<Action>();
      this.SpawnPrefabInternal(new List<MyCubeGrid>(), prefabName, position, forward, up, initialLinearVelocity, initialAngularVelocity, beaconName, entityName, spawningOptions, ownerId, updateSync, callbacks);
    }

    public void SpawnPrefab(
      List<MyCubeGrid> resultList,
      string prefabName,
      Vector3D position,
      Vector3 forward,
      Vector3 up,
      Vector3 initialLinearVelocity = default (Vector3),
      Vector3 initialAngularVelocity = default (Vector3),
      string beaconName = null,
      string entityName = null,
      SpawningOptions spawningOptions = SpawningOptions.None,
      long ownerId = 0,
      bool updateSync = false,
      Stack<Action> callbacks = null)
    {
      if (callbacks == null)
        callbacks = new Stack<Action>();
      this.SpawnPrefabInternal(resultList, prefabName, position, forward, up, initialLinearVelocity, initialAngularVelocity, beaconName, entityName, spawningOptions, ownerId, updateSync, callbacks);
    }

    void IMyPrefabManager.SpawnPrefab(
      List<IMyCubeGrid> resultList,
      string prefabName,
      Vector3D position,
      Vector3 forward,
      Vector3 up,
      Vector3 initialLinearVelocity,
      Vector3 initialAngularVelocity,
      string beaconName,
      SpawningOptions spawningOptions,
      bool updateSync,
      Action callback)
    {
      ((IMyPrefabManager) this).SpawnPrefab(resultList, prefabName, position, forward, up, initialAngularVelocity, initialAngularVelocity, beaconName, spawningOptions, 0L, updateSync, callback);
    }

    void IMyPrefabManager.SpawnPrefab(
      List<IMyCubeGrid> resultList,
      string prefabName,
      Vector3D position,
      Vector3 forward,
      Vector3 up,
      Vector3 initialLinearVelocity,
      Vector3 initialAngularVelocity,
      string beaconName,
      SpawningOptions spawningOptions,
      long ownerId,
      bool updateSync,
      Action callback)
    {
      Stack<Action> callbacks = new Stack<Action>();
      if (callback != null)
        callbacks.Push(callback);
      List<MyCubeGrid> results = new List<MyCubeGrid>();
      this.SpawnPrefab(results, prefabName, position, forward, up, initialLinearVelocity, initialAngularVelocity, beaconName, spawningOptions: spawningOptions, ownerId: ownerId, updateSync: updateSync, callbacks: callbacks);
      callbacks.Push((Action) (() =>
      {
        foreach (IMyCubeGrid myCubeGrid in results)
          resultList.Add(myCubeGrid);
      }));
    }

    internal void SpawnPrefabInternal(
      List<MyCubeGrid> resultList,
      string prefabName,
      Vector3D position,
      Vector3 forward,
      Vector3 up,
      Vector3 initialLinearVelocity,
      Vector3 initialAngularVelocity,
      string beaconName,
      string entityName,
      SpawningOptions spawningOptions,
      long ownerId,
      bool updateSync,
      Stack<Action> callbacks)
    {
      this.SpawnPrefabInternal(new MySpawnPrefabProperties()
      {
        ResultList = resultList,
        PrefabName = prefabName,
        Position = position,
        Forward = forward,
        Up = up,
        InitialAngularVelocity = initialAngularVelocity,
        InitialLinearVelocity = initialLinearVelocity,
        BeaconName = beaconName,
        EntityName = entityName,
        SpawningOptions = spawningOptions,
        OwnerId = ownerId,
        UpdateSync = updateSync
      }, callbacks);
    }

    internal void SpawnPrefabInternal(
      MySpawnPrefabProperties spawnPrefabProperties,
      Action callback,
      Action spawnFailedCallback = null)
    {
      Stack<Action> callbacks = new Stack<Action>();
      if (callback != null)
        callbacks.Push(callback);
      this.SpawnPrefabInternal(spawnPrefabProperties, callbacks, spawnFailedCallback);
    }

    internal void SpawnPrefabInternal(
      MySpawnPrefabProperties spawnPrefabProperties,
      Stack<Action> callbacks,
      Action spawnFailedCallback = null)
    {
      if (spawnPrefabProperties == null)
        throw new ArgumentNullException(nameof (spawnPrefabProperties));
      if (callbacks == null)
        throw new ArgumentNullException(nameof (callbacks));
      if (spawnPrefabProperties.ResultList == null)
        spawnPrefabProperties.ResultList = new List<MyCubeGrid>();
      Vector3 forward = spawnPrefabProperties.Forward;
      Vector3 up = spawnPrefabProperties.Up;
      MatrixD world = MatrixD.CreateWorld(spawnPrefabProperties.Position, forward, up);
      MyPrefabManager.CreateGridsData createGridsData = new MyPrefabManager.CreateGridsData(spawnPrefabProperties.ResultList, spawnPrefabProperties.PrefabName, world, spawnPrefabProperties.SpawningOptions, ownerId: spawnPrefabProperties.OwnerId, callbacks: callbacks, spawnFailedCallback: spawnFailedCallback);
      Interlocked.Increment(ref MyPrefabManager.PendingGrids);
      callbacks.Push((Action) (() => this.SpawnPrefabInternalSetProperties(spawnPrefabProperties)));
      callbacks.Push((Action) (() =>
      {
        if (spawnPrefabProperties.ResultList.Count <= 0)
          return;
        PrefabSpawnedEvent prefabSpawnedDetailed = MyVisualScriptLogicProvider.PrefabSpawnedDetailed;
        if (prefabSpawnedDetailed == null)
          return;
        prefabSpawnedDetailed(spawnPrefabProperties.ResultList[0].EntityId, spawnPrefabProperties.PrefabName);
      }));
      if (MySandboxGame.Config.SyncRendering)
      {
        MyEntityIdentifier.PrepareSwapData();
        MyEntityIdentifier.SwapPerThreadData();
      }
      Parallel.Start(new Action<WorkData>(createGridsData.CallCreateGridsFromPrefab), new Action<WorkData>(createGridsData.OnGridsCreated), (WorkData) createGridsData);
      if (!MySandboxGame.Config.SyncRendering)
        return;
      MyEntityIdentifier.ClearSwapDataAndRestore();
    }

    private void SpawnPrefabInternalSetProperties(MySpawnPrefabProperties spawnPrefabProperties)
    {
      int num1 = 0;
      using (spawnPrefabProperties.UpdateSync ? MyRandom.Instance.PushSeed(num1 = MyRandom.Instance.CreateRandomSeed()) : new MyRandom.StateToken())
      {
        SpawningOptions spawningOptions = spawnPrefabProperties.SpawningOptions;
        bool flag1 = spawningOptions.HasFlag((Enum) SpawningOptions.RotateFirstCockpitTowardsDirection);
        bool flag2 = spawningOptions.HasFlag((Enum) SpawningOptions.SpawnRandomCargo);
        bool flag3 = spawningOptions.HasFlag((Enum) SpawningOptions.SetNeutralOwner);
        bool flag4 = spawningOptions.HasFlag((Enum) SpawningOptions.ReplaceColor);
        string beaconName = spawnPrefabProperties.BeaconName;
        string entityName = spawnPrefabProperties.EntityName;
        bool flag5 = ((flag2 | flag1 | flag3 ? 1 : (beaconName != null ? 1 : 0)) | (flag4 ? 1 : 0)) != 0;
        List<MyCockpit> myCockpitList = new List<MyCockpit>();
        List<MyRemoteControl> myRemoteControlList = new List<MyRemoteControl>();
        bool flag6 = spawningOptions.HasFlag((Enum) SpawningOptions.TurnOffReactors);
        foreach (MyCubeGrid result in spawnPrefabProperties.ResultList)
        {
          result.ClearSymmetries();
          if (spawningOptions.HasFlag((Enum) SpawningOptions.DisableDampeners))
          {
            MyEntityThrustComponent entityThrustComponent = result.Components.Get<MyEntityThrustComponent>();
            if (entityThrustComponent != null)
              entityThrustComponent.DampenersEnabled = false;
          }
          if (spawningOptions.HasFlag((Enum) SpawningOptions.DisableSave))
            result.Save = false;
          if (flag5 | flag6)
          {
            foreach (MySlimBlock block in result.GetBlocks())
            {
              if (block.ColorMaskHSV.Equals(this.MASK_COLOR, 0.01f))
              {
                block.ColorMaskHSV = spawnPrefabProperties.Color;
                block.UpdateVisual(false);
              }
              if (block.FatBlock is MyCockpit && block.FatBlock.IsFunctional)
                myCockpitList.Add((MyCockpit) block.FatBlock);
              else if (block.FatBlock is MyCargoContainer & flag2)
                (block.FatBlock as MyCargoContainer).SpawnRandomCargo();
              else if (block.FatBlock is MyBeacon && beaconName != null)
                (block.FatBlock as MyBeacon).SetCustomName(beaconName);
              else if (flag6 && block.FatBlock != null && block.FatBlock.Components.Contains(typeof (MyResourceSourceComponent)))
              {
                MyResourceSourceComponent resourceSourceComponent = block.FatBlock.Components.Get<MyResourceSourceComponent>();
                if (resourceSourceComponent != null && resourceSourceComponent.ResourceTypes.Contains<MyDefinitionId>(MyResourceDistributorComponent.ElectricityId))
                  resourceSourceComponent.Enabled = false;
              }
              else if (block.FatBlock is MyRemoteControl)
                myRemoteControlList.Add(block.FatBlock as MyRemoteControl);
            }
          }
          if (!string.IsNullOrEmpty(entityName))
          {
            int num2 = spawnPrefabProperties.ResultList.IndexOf(result);
            string str = num2 == 0 ? entityName : entityName + (object) num2;
            result.Name = str;
            Sandbox.Game.Entities.MyEntities.SetEntityName((MyEntity) result);
          }
        }
        if (myCockpitList.Count > 1)
          myCockpitList.Sort((Comparison<MyCockpit>) ((cockpitA, cockpitB) =>
          {
            int num2 = cockpitB.IsMainCockpit.CompareTo(cockpitA.IsMainCockpit);
            return num2 != 0 ? num2 : cockpitB.EnableShipControl.CompareTo(cockpitA.EnableShipControl);
          }));
        MyCubeBlock myCubeBlock = (MyCubeBlock) null;
        if (myCockpitList.Count > 0 && (myCockpitList[0].EnableShipControl || myRemoteControlList.Count <= 0))
          myCubeBlock = (MyCubeBlock) myCockpitList[0];
        if (myCubeBlock == null && myRemoteControlList.Count > 0)
        {
          if (myRemoteControlList.Count > 1)
            myRemoteControlList.Sort((Comparison<MyRemoteControl>) ((remoteA, remoteB) => remoteB.IsMainRemoteControl.CompareTo(remoteA.IsMainCockpit)));
          myCubeBlock = (MyCubeBlock) myRemoteControlList[0];
        }
        Matrix matrix = Matrix.Identity;
        if (flag1 && myCubeBlock != null)
        {
          MatrixD worldMatrix = myCubeBlock.WorldMatrix;
          matrix = Matrix.Multiply(Matrix.Invert((Matrix) ref worldMatrix), Matrix.CreateWorld((Vector3) myCubeBlock.WorldMatrix.Translation, spawnPrefabProperties.Forward, spawnPrefabProperties.Up));
        }
        foreach (MyCubeGrid result in spawnPrefabProperties.ResultList)
        {
          if (myCubeBlock != null & flag1)
            result.WorldMatrix = result.WorldMatrix * matrix;
          if (result.Physics != null)
          {
            result.Physics.LinearVelocity = spawnPrefabProperties.InitialLinearVelocity;
            result.Physics.AngularVelocity = spawnPrefabProperties.InitialAngularVelocity;
          }
          SingleKeyEntityNameEvent prefabSpawned = MyVisualScriptLogicProvider.PrefabSpawned;
          if (prefabSpawned != null)
          {
            string name = result.Name;
            if (string.IsNullOrWhiteSpace(name))
              name = result.EntityId.ToString();
            prefabSpawned(name);
          }
        }
      }
    }

    bool IMyPrefabManager.IsPathClear(Vector3D from, Vector3D to)
    {
      MyPhysics.CastRay(from, to, MyPrefabManager.m_raycastHits, 24);
      MyPrefabManager.m_raycastHits.Clear();
      return MyPrefabManager.m_raycastHits.Count == 0;
    }

    bool IMyPrefabManager.IsPathClear(Vector3D from, Vector3D to, double halfSize)
    {
      Vector3D vector2_1 = new Vector3D();
      vector2_1.X = 1.0;
      Vector3D vector1 = to - from;
      vector1.Normalize();
      if (Vector3D.Dot(vector1, vector2_1) > 0.899999976158142 || Vector3D.Dot(vector1, vector2_1) < -0.899999976158142)
      {
        vector2_1.X = 0.0;
        vector2_1.Y = 1.0;
      }
      Vector3D vector2_2 = Vector3D.Cross(vector1, vector2_1);
      vector2_2.Normalize();
      vector2_2 *= halfSize;
      MyPhysics.CastRay(from + vector2_2, to + vector2_2, MyPrefabManager.m_raycastHits, 24);
      if (MyPrefabManager.m_raycastHits.Count > 0)
      {
        MyPrefabManager.m_raycastHits.Clear();
        return false;
      }
      vector2_2 *= -1.0;
      MyPhysics.CastRay(from + vector2_2, to + vector2_2, MyPrefabManager.m_raycastHits, 24);
      if (MyPrefabManager.m_raycastHits.Count > 0)
      {
        MyPrefabManager.m_raycastHits.Clear();
        return false;
      }
      vector2_2 = Vector3D.Cross(vector1, vector2_2);
      MyPhysics.CastRay(from + vector2_2, to + vector2_2, MyPrefabManager.m_raycastHits, 24);
      if (MyPrefabManager.m_raycastHits.Count > 0)
      {
        MyPrefabManager.m_raycastHits.Clear();
        return false;
      }
      vector2_2 *= -1.0;
      MyPhysics.CastRay(from + vector2_2, to + vector2_2, MyPrefabManager.m_raycastHits, 24);
      if (MyPrefabManager.m_raycastHits.Count <= 0)
        return true;
      MyPrefabManager.m_raycastHits.Clear();
      return false;
    }

    public class CreateGridsData : WorkData
    {
      private List<MyCubeGrid> m_results;
      private string m_prefabName;
      private MatrixD m_worldMatrix;
      private SpawningOptions m_spawnOptions;
      private bool m_ignoreMemoryLimits;
      private long m_ownerId;
      private Stack<Action> m_callbacks;
      private List<IMyEntity> m_resultIDs;
      private Action m_spawnFailedCallback;

      public CreateGridsData(
        List<MyCubeGrid> results,
        string prefabName,
        MatrixD worldMatrix,
        SpawningOptions spawnOptions,
        bool ignoreMemoryLimits = true,
        long ownerId = 0,
        Stack<Action> callbacks = null,
        Action spawnFailedCallback = null)
      {
        this.m_results = results;
        this.m_ownerId = ownerId;
        this.m_prefabName = prefabName;
        this.m_worldMatrix = worldMatrix;
        this.m_spawnOptions = spawnOptions;
        this.m_ignoreMemoryLimits = ignoreMemoryLimits;
        this.m_callbacks = callbacks ?? new Stack<Action>();
        this.m_spawnFailedCallback = spawnFailedCallback;
        if (!spawnOptions.HasFlag((Enum) SpawningOptions.SetNeutralOwner))
          return;
        List<MyFaction> npcFactions = MySession.Static.Factions.GetNpcFactions();
        if (npcFactions.Count > 0)
        {
          int index = MyRandom.Instance.Next(0, npcFactions.Count - 1);
          this.m_ownerId = npcFactions[index].FounderId;
        }
        else
        {
          HashSet<long> npcIdentities = MySession.Static.Players.GetNPCIdentities();
          if (npcIdentities.Count == 0)
            MySession.Static.Players.CreateNewNpcIdentity("NPC " + (object) MyRandom.Instance.Next(1000, 9999));
          int count = MyRandom.Instance.Next(0, npcIdentities.Count - 1);
          this.m_ownerId = npcIdentities.Skip<long>(count).First<long>();
        }
      }

      public void CallCreateGridsFromPrefab(WorkData workData)
      {
        try
        {
          MyEntityIdentifier.InEntityCreationBlock = true;
          MyEntityIdentifier.LazyInitPerThreadStorage(2048);
          MyPrefabManager.Static.CreateGridsFromPrefab(this.m_results, this.m_prefabName, this.m_worldMatrix, this.m_spawnOptions, this.m_ignoreMemoryLimits, this.m_ownerId, this.m_callbacks);
        }
        catch (Exception ex)
        {
          MyLog.Default.WriteLine("Could not spawn prefab " + this.m_prefabName + ".");
          MyLog.Default.WriteLine(ex);
          throw;
        }
        finally
        {
          this.m_resultIDs = new List<IMyEntity>();
          MyEntityIdentifier.GetPerThreadEntities(this.m_resultIDs);
          MyEntityIdentifier.ClearPerThreadEntities();
          MyEntityIdentifier.InEntityCreationBlock = false;
          Interlocked.Decrement(ref MyPrefabManager.PendingGrids);
          if (MyPrefabManager.PendingGrids <= 0)
            MyPrefabManager.FinishedProcessingGrids.Set();
        }
      }

      public void OnGridsCreated(WorkData workData)
      {
        if (this.m_resultIDs == null || this.m_results == null)
        {
          if (this.m_spawnFailedCallback == null)
            return;
          this.m_spawnFailedCallback.InvokeIfNotNull();
        }
        else
        {
          foreach (IMyEntity resultId in this.m_resultIDs)
          {
            IMyEntity entity;
            MyEntityIdentifier.TryGetEntity(resultId.EntityId, out entity);
            if (entity == null)
              MyEntityIdentifier.AddEntityWithId(resultId);
          }
          foreach (MyEntity result in this.m_results)
            Sandbox.Game.Entities.MyEntities.Add(result);
          if (this.m_spawnFailedCallback != null && this.m_results.Count == 0)
          {
            this.m_spawnFailedCallback.InvokeIfNotNull();
          }
          else
          {
            if (this.m_callbacks == null)
              return;
            while (this.m_callbacks.Count > 0)
              this.m_callbacks.Pop().InvokeIfNotNull();
          }
        }
      }
    }
  }
}
