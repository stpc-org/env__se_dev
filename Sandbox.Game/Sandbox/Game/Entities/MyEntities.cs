// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyEntities
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRage.Groups;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Plugins;
using VRage.Profiler;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
  [StaticEventOwner]
  public static class MyEntities
  {
    private static readonly long EntityNativeMemoryLimit = 1363148800;
    private static readonly long EntityManagedMemoryLimit = 681574400;
    private static MyConcurrentHashSet<MyEntity> m_entities = new MyConcurrentHashSet<MyEntity>();
    private static ConcurrentCachingList<IMyEntity> m_entitiesForDraw = new ConcurrentCachingList<IMyEntity>();
    private static readonly List<IMySceneComponent> m_sceneComponents = new List<IMySceneComponent>();
    public static IMyUpdateOrchestrator Orchestrator;
    [ThreadStatic]
    private static MyEntityIdRemapHelper m_remapHelper;
    private static readonly int MAX_ENTITIES_CLOSE_PER_UPDATE = 10;
    public static bool IsClosingAll = false;
    public static bool IgnoreMemoryLimits = false;
    private static MyEntityCreationThread m_creationThread;
    private static Dictionary<uint, IMyEntity> m_renderObjectToEntityMap = new Dictionary<uint, IMyEntity>();
    private static readonly FastResourceLock m_renderObjectToEntityMapLock = new FastResourceLock();
    [ThreadStatic]
    private static List<MyEntity> m_overlapRBElementList;
    private static readonly List<List<MyEntity>> m_overlapRBElementListCollection = new List<List<MyEntity>>();
    private static List<HkBodyCollision> m_rigidBodyList = new List<HkBodyCollision>();
    private static readonly List<MyLineSegmentOverlapResult<MyEntity>> LineOverlapEntityList = new List<MyLineSegmentOverlapResult<MyEntity>>();
    private static readonly List<MyPhysics.HitInfo> m_hits = new List<MyPhysics.HitInfo>();
    [ThreadStatic]
    private static HashSet<IMyEntity> m_entityResultSet;
    private static readonly List<HashSet<IMyEntity>> m_entityResultSetCollection = new List<HashSet<IMyEntity>>();
    [ThreadStatic]
    private static List<MyEntity> m_entityInputList;
    private static readonly List<List<MyEntity>> m_entityInputListCollection = new List<List<MyEntity>>();
    private static HashSet<MyEntity> m_entitiesToDelete = new HashSet<MyEntity>();
    private static HashSet<MyEntity> m_entitiesToDeleteNextFrame = new HashSet<MyEntity>();
    public static ConcurrentDictionary<string, MyEntity> m_entityNameDictionary = new ConcurrentDictionary<string, MyEntity>();
    private static bool m_isLoaded = false;
    private static HkShape m_cameraSphere;
    public static FastResourceLock EntityCloseLock = new FastResourceLock();
    public static FastResourceLock EntityMarkForCloseLock = new FastResourceLock();
    public static FastResourceLock UnloadDataLock = new FastResourceLock();
    public static bool UpdateInProgress = false;
    public static bool CloseAllowed = false;
    private static int m_update10Index = 0;
    private static int m_update100Index = 0;
    private static float m_update10Count = 0.0f;
    private static float m_update100Count = 0.0f;
    [ThreadStatic]
    private static List<MyEntity> m_allIgnoredEntities;
    private static readonly List<List<MyEntity>> m_allIgnoredEntitiesCollection = new List<List<MyEntity>>();
    private static readonly HashSet<System.Type> m_hiddenTypes = new HashSet<System.Type>();
    public static bool SafeAreasHidden;
    public static bool SafeAreasSelectable;
    public static bool DetectorsHidden;
    public static bool DetectorsSelectable;
    public static bool ParticleEffectsHidden;
    public static bool ParticleEffectsSelectable;
    private static readonly Dictionary<string, int> m_typesStats = new Dictionary<string, int>();
    private static List<MyCubeGrid> m_cubeGridList = new List<MyCubeGrid>();
    private static readonly HashSet<MyCubeGrid> m_cubeGridHash = new HashSet<MyCubeGrid>();
    private static readonly HashSet<IMyEntity> m_entitiesForDebugDraw = new HashSet<IMyEntity>();
    private static readonly HashSet<object> m_groupDebugHelper = new HashSet<object>();
    private static readonly MyStringId GIZMO_LINE_MATERIAL = MyStringId.GetOrCompute("GizmoDrawLine");
    private static readonly MyStringId GIZMO_LINE_MATERIAL_WHITE = MyStringId.GetOrCompute("GizmoDrawLineWhite");
    private static readonly ConcurrentDictionary<MyEntity, MyEntities.BoundingBoxDrawArgs> m_entitiesForBBoxDraw = new ConcurrentDictionary<MyEntity, MyEntities.BoundingBoxDrawArgs>();

    public static event Action<MyEntity> OnEntityRemove;

    public static event Action<MyEntity> OnEntityAdd;

    public static event Action<MyEntity> OnEntityCreate;

    public static event Action<MyEntity> OnEntityDelete;

    public static event Action OnCloseAll;

    public static event Action<MyEntity, string, string> OnEntityNameSet;

    public static bool IsAsyncUpdateInProgress { get; private set; }

    static MyEntities()
    {
      System.Type type = typeof (MyEntity);
      MyEntityFactory.RegisterDescriptor(type.GetCustomAttribute<MyEntityTypeAttribute>(false), type);
      MyEntityFactory.RegisterDescriptorsFromAssembly(typeof (MyEntities).Assembly);
      MyEntityFactory.RegisterDescriptorsFromAssembly(MyPlugins.GameAssembly);
      MyEntityFactory.RegisterDescriptorsFromAssembly(MyPlugins.SandboxAssembly);
      MyEntityFactory.RegisterDescriptorsFromAssembly(MyPlugins.UserAssemblies);
      MyEntityExtensions.SetCallbacks();
      MyEntitiesInterface.RegisterUpdate = new Action<MyEntity>(MyEntities.RegisterForUpdate);
      MyEntitiesInterface.UnregisterUpdate = new Action<MyEntity, bool>(MyEntities.UnregisterForUpdate);
      MyEntitiesInterface.RegisterDraw = new Action<MyEntity>(MyEntities.RegisterForDraw);
      MyEntitiesInterface.UnregisterDraw = new Action<MyEntity>(MyEntities.UnregisterForDraw);
      MyEntitiesInterface.SetEntityName = new Action<MyEntity, bool>(MyEntities.SetEntityName);
      MyEntitiesInterface.IsUpdateInProgress = new Func<bool>(MyEntities.IsUpdateInProgress);
      MyEntitiesInterface.IsCloseAllowed = new Func<bool>(MyEntities.IsCloseAllowed);
      MyEntitiesInterface.RemoveName = new Action<MyEntity>(MyEntities.RemoveName);
      MyEntitiesInterface.RemoveFromClosedEntities = new Action<MyEntity>(MyEntities.RemoveFromClosedEntities);
      MyEntitiesInterface.Remove = new Func<MyEntity, bool>(MyEntities.Remove);
      MyEntitiesInterface.RaiseEntityRemove = new Action<MyEntity>(MyEntities.RaiseEntityRemove);
      MyEntitiesInterface.Close = new Action<MyEntity>(MyEntities.Close);
      MyEntities.Orchestrator = (IMyUpdateOrchestrator) Activator.CreateInstance(MyPerGameSettings.UpdateOrchestratorType);
    }

    public static void AddRenderObjectToMap(uint id, IMyEntity entity)
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      using (MyEntities.m_renderObjectToEntityMapLock.AcquireExclusiveUsing())
        MyEntities.m_renderObjectToEntityMap.Add(id, entity);
    }

    public static void RemoveRenderObjectFromMap(uint id)
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      using (MyEntities.m_renderObjectToEntityMapLock.AcquireExclusiveUsing())
        MyEntities.m_renderObjectToEntityMap.Remove(id);
    }

    private static List<MyEntity> OverlapRBElementList
    {
      get
      {
        if (MyEntities.m_overlapRBElementList == null)
        {
          MyEntities.m_overlapRBElementList = new List<MyEntity>(256);
          lock (MyEntities.m_overlapRBElementListCollection)
            MyEntities.m_overlapRBElementListCollection.Add(MyEntities.m_overlapRBElementList);
        }
        return MyEntities.m_overlapRBElementList;
      }
    }

    public static bool IsShapePenetrating(
      HkShape shape,
      ref Vector3D position,
      ref Quaternion rotation,
      int filter = 15,
      MyEntity ignoreEnt = null)
    {
      using (MyUtils.ReuseCollection<HkBodyCollision>(ref MyEntities.m_rigidBodyList))
      {
        MyPhysics.GetPenetrationsShape(shape, ref position, ref rotation, MyEntities.m_rigidBodyList, filter);
        if (ignoreEnt != null)
        {
          for (int index = MyEntities.m_rigidBodyList.Count - 1; index >= 0; --index)
          {
            if (MyEntities.m_rigidBodyList[index].GetCollisionEntity() == ignoreEnt)
            {
              MyEntities.m_rigidBodyList.RemoveAtFast<HkBodyCollision>(index);
              break;
            }
          }
        }
        return MyEntities.m_rigidBodyList.Count > 0;
      }
    }

    public static bool IsSpherePenetrating(ref BoundingSphereD bs) => MyEntities.IsShapePenetrating(MyEntities.m_cameraSphere, ref bs.Center, ref Quaternion.Identity);

    public static Vector3D? FindFreePlace(
      ref MatrixD matrix,
      Vector3 axis,
      float radius,
      int maxTestCount = 20,
      int testsPerDistance = 5,
      float stepSize = 1f)
    {
      Vector3 forward = (Vector3) matrix.Forward;
      double num1 = (double) forward.Normalize();
      Vector3D vector3D1 = matrix.Translation;
      Quaternion identity = Quaternion.Identity;
      HkShape shape = (HkShape) new HkSphereShape(radius);
      try
      {
        if (MyEntities.IsInsideWorld(vector3D1) && !MyEntities.IsShapePenetrating(shape, ref vector3D1, ref identity) && MyEntities.FindFreePlaceVoxelMap(vector3D1, radius, ref shape, ref vector3D1))
          return new Vector3D?(vector3D1);
        int num2 = (int) Math.Ceiling((double) maxTestCount / (double) testsPerDistance);
        float num3 = 6.283185f / (float) testsPerDistance;
        float num4 = 0.0f;
        for (int index1 = 0; index1 < num2; ++index1)
        {
          num4 += radius * stepSize;
          Vector3D vector3D2 = (Vector3D) forward;
          float angle = 0.0f;
          for (int index2 = 0; index2 < testsPerDistance; ++index2)
          {
            if (index2 != 0)
            {
              angle += num3;
              Quaternion fromAxisAngle = Quaternion.CreateFromAxisAngle(axis, angle);
              vector3D2 = Vector3D.Transform((Vector3D) forward, fromAxisAngle);
            }
            vector3D1 = matrix.Translation + vector3D2 * (double) num4;
            if (MyEntities.IsInsideWorld(vector3D1) && !MyEntities.IsShapePenetrating(shape, ref vector3D1, ref identity) && MyEntities.FindFreePlaceVoxelMap(vector3D1, radius, ref shape, ref vector3D1))
              return new Vector3D?(vector3D1);
          }
        }
        return new Vector3D?();
      }
      finally
      {
        shape.RemoveReference();
      }
    }

    public static Vector3D? FindFreePlace(
      Vector3D basePos,
      float radius,
      int maxTestCount = 20,
      int testsPerDistance = 5,
      float stepSize = 1f,
      MyEntity ignoreEnt = null)
    {
      return MyEntities.FindFreePlaceCustom(basePos, radius, maxTestCount, testsPerDistance, stepSize, ignoreEnt: ignoreEnt);
    }

    public static Vector3D? FindFreePlaceCustom(
      Vector3D basePos,
      float radius,
      int maxTestCount = 20,
      int testsPerDistance = 5,
      float stepSize = 1f,
      float radiusIncrement = 0.0f,
      MyEntity ignoreEnt = null)
    {
      Vector3D position = basePos;
      Quaternion identity = Quaternion.Identity;
      HkShape shape = (HkShape) new HkSphereShape(radius);
      try
      {
        if (MyEntities.IsInsideWorld(position) && !MyEntities.IsShapePenetrating(shape, ref position, ref identity, ignoreEnt: ignoreEnt))
        {
          BoundingSphereD sphere = new BoundingSphereD(position, (double) radius);
          MyVoxelBase overlappingWithSphere = MySession.Static.VoxelMaps.GetOverlappingWithSphere(ref sphere);
          if (overlappingWithSphere == null)
            return new Vector3D?(position);
          if (overlappingWithSphere is MyPlanet)
            (overlappingWithSphere as MyPlanet).CorrectSpawnLocation(ref basePos, (double) radius);
          return new Vector3D?(basePos);
        }
        int num1 = (int) Math.Ceiling((double) maxTestCount / (double) testsPerDistance);
        float num2 = 0.0f;
        for (int index1 = 0; index1 < num1; ++index1)
        {
          num2 += radius * stepSize + radiusIncrement;
          for (int index2 = 0; index2 < testsPerDistance; ++index2)
          {
            position = basePos + MyUtils.GetRandomVector3Normalized() * num2;
            if (MyEntities.IsInsideWorld(position) && !MyEntities.IsShapePenetrating(shape, ref position, ref identity, ignoreEnt: ignoreEnt))
            {
              BoundingSphereD sphere = new BoundingSphereD(position, (double) radius);
              MyVoxelBase overlappingWithSphere = MySession.Static.VoxelMaps.GetOverlappingWithSphere(ref sphere);
              if (overlappingWithSphere == null)
                return new Vector3D?(position);
              if (overlappingWithSphere is MyPlanet)
                (overlappingWithSphere as MyPlanet).CorrectSpawnLocation(ref basePos, (double) radius);
            }
          }
        }
        return new Vector3D?();
      }
      finally
      {
        shape.RemoveReference();
      }
    }

    public static Vector3D? TestPlaceInSpace(Vector3D basePos, float radius)
    {
      List<MyVoxelBase> voxels = new List<MyVoxelBase>();
      Vector3D position = basePos;
      Quaternion identity = Quaternion.Identity;
      HkShape shape = (HkShape) new HkSphereShape(radius);
      try
      {
        if (MyEntities.IsInsideWorld(position) && !MyEntities.IsShapePenetrating(shape, ref position, ref identity))
        {
          BoundingSphereD sphere = new BoundingSphereD(position, (double) radius);
          MySession.Static.VoxelMaps.GetAllOverlappingWithSphere(ref sphere, voxels);
          if (voxels.Count == 0)
            return new Vector3D?(position);
          bool flag = true;
          foreach (MyVoxelBase myVoxelBase in voxels)
          {
            if (!(myVoxelBase is MyPlanet myPlanet))
            {
              flag = false;
              break;
            }
            if ((position - myPlanet.PositionComp.GetPosition()).Length() < (double) myPlanet.MaximumRadius)
            {
              flag = false;
              break;
            }
          }
          if (flag)
            return new Vector3D?(position);
        }
        return new Vector3D?();
      }
      finally
      {
        shape.RemoveReference();
      }
    }

    private static bool FindFreePlaceVoxelMap(
      Vector3D currentPos,
      float radius,
      ref HkShape shape,
      ref Vector3D ret)
    {
      BoundingSphereD sphere = new BoundingSphereD(currentPos, (double) radius);
      MyVoxelBase overlappingWithSphere = MySession.Static.VoxelMaps.GetOverlappingWithSphere(ref sphere);
      MyVoxelBase myVoxelBase = overlappingWithSphere == null ? (MyVoxelBase) null : overlappingWithSphere.RootVoxel;
      if (myVoxelBase == null)
      {
        ret = currentPos;
        return true;
      }
      if (myVoxelBase is MyPlanet myPlanet)
      {
        int num = myPlanet.CorrectSpawnLocation2(ref currentPos, (double) radius) ? 1 : 0;
        Quaternion identity = Quaternion.Identity;
        if (num != 0)
        {
          if (!MyEntities.IsShapePenetrating(shape, ref currentPos, ref identity))
          {
            ret = currentPos;
            return true;
          }
          if (myPlanet.CorrectSpawnLocation2(ref currentPos, (double) radius, true) && !MyEntities.IsShapePenetrating(shape, ref currentPos, ref identity))
          {
            ret = currentPos;
            return true;
          }
        }
      }
      return false;
    }

    public static void GetInflatedPlayerBoundingBox(ref BoundingBoxD playerBox, float inflation)
    {
      foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) Sync.Players.GetOnlinePlayers())
        playerBox.Include(onlinePlayer.GetPosition());
      playerBox.Inflate((double) inflation);
    }

    public static bool IsInsideVoxel(
      Vector3D pos,
      Vector3D hintPosition,
      out Vector3D lastOutsidePos)
    {
      MyEntities.m_hits.Clear();
      lastOutsidePos = pos;
      MyPhysics.CastRay(hintPosition, pos, MyEntities.m_hits, 15);
      int num = 0;
      foreach (MyPhysics.HitInfo hit in MyEntities.m_hits)
      {
        if (hit.HkHitInfo.GetHitEntity() is MyVoxelMap)
        {
          ++num;
          lastOutsidePos = hit.Position;
        }
      }
      MyEntities.m_hits.Clear();
      return (uint) (num % 2) > 0U;
    }

    public static bool IsWorldLimited() => MySession.Static != null && (uint) MySession.Static.Settings.WorldSizeKm > 0U;

    public static float WorldHalfExtent() => MySession.Static == null ? 0.0f : (float) (MySession.Static.Settings.WorldSizeKm * 500);

    public static float WorldSafeHalfExtent()
    {
      float num = MyEntities.WorldHalfExtent();
      return (double) num != 0.0 ? num - 600f : 0.0f;
    }

    public static bool IsInsideWorld(Vector3D pos)
    {
      float num = MyEntities.WorldHalfExtent();
      return (double) num == 0.0 || pos.AbsMax() <= (double) num;
    }

    public static bool IsRaycastBlocked(Vector3D pos, Vector3D target)
    {
      MyEntities.m_hits.Clear();
      MyPhysics.CastRay(pos, target, MyEntities.m_hits);
      return MyEntities.m_hits.Count > 0;
    }

    public static List<MyEntity> GetEntitiesInAABB(ref BoundingBox boundingBox)
    {
      BoundingBoxD box = (BoundingBoxD) boundingBox;
      MyGamePruningStructure.GetAllEntitiesInBox(ref box, MyEntities.OverlapRBElementList);
      return MyEntities.OverlapRBElementList;
    }

    public static List<MyEntity> GetEntitiesInAABB(
      ref BoundingBoxD boundingBox,
      bool exact = false)
    {
      MyGamePruningStructure.GetAllEntitiesInBox(ref boundingBox, MyEntities.OverlapRBElementList);
      if (exact)
      {
        int index = 0;
        while (index < MyEntities.OverlapRBElementList.Count)
        {
          MyEntity overlapRbElement = MyEntities.OverlapRBElementList[index];
          if (!boundingBox.Intersects(overlapRbElement.PositionComp.WorldAABB))
            MyEntities.OverlapRBElementList.RemoveAt(index);
          else
            ++index;
        }
      }
      return MyEntities.OverlapRBElementList;
    }

    public static List<MyEntity> GetEntitiesInSphere(ref BoundingSphereD boundingSphere)
    {
      MyGamePruningStructure.GetAllEntitiesInSphere(ref boundingSphere, MyEntities.OverlapRBElementList);
      return MyEntities.OverlapRBElementList;
    }

    public static List<MyEntity> GetEntitiesInOBB(ref MyOrientedBoundingBoxD obb)
    {
      MyGamePruningStructure.GetAllEntitiesInOBB(ref obb, MyEntities.OverlapRBElementList);
      return MyEntities.OverlapRBElementList;
    }

    public static List<MyEntity> GetTopMostEntitiesInSphere(
      ref BoundingSphereD boundingSphere)
    {
      MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref boundingSphere, MyEntities.OverlapRBElementList);
      return MyEntities.OverlapRBElementList;
    }

    public static void GetElementsInBox(ref BoundingBoxD boundingBox, List<MyEntity> foundElements) => MyGamePruningStructure.GetAllEntitiesInBox(ref boundingBox, foundElements);

    public static void GetTopMostEntitiesInBox(
      ref BoundingBoxD boundingBox,
      List<MyEntity> foundElements,
      MyEntityQueryType qtype = MyEntityQueryType.Both)
    {
      MyGamePruningStructure.GetAllTopMostStaticEntitiesInBox(ref boundingBox, foundElements, qtype);
    }

    private static HashSet<IMyEntity> EntityResultSet
    {
      get
      {
        if (MyEntities.m_entityResultSet == null)
        {
          MyEntities.m_entityResultSet = new HashSet<IMyEntity>();
          lock (MyEntities.m_entityResultSetCollection)
            MyEntities.m_entityResultSetCollection.Add(MyEntities.m_entityResultSet);
        }
        return MyEntities.m_entityResultSet;
      }
    }

    private static List<MyEntity> EntityInputList
    {
      get
      {
        if (MyEntities.m_entityInputList == null)
        {
          MyEntities.m_entityInputList = new List<MyEntity>(32);
          lock (MyEntities.m_entityInputListCollection)
            MyEntities.m_entityInputListCollection.Add(MyEntities.m_entityInputList);
        }
        return MyEntities.m_entityInputList;
      }
    }

    public static bool IsLoaded => MyEntities.m_isLoaded;

    private static void AddComponents()
    {
      MyEntities.m_sceneComponents.Add((IMySceneComponent) new MyCubeGridGroups());
      MyEntities.m_sceneComponents.Add((IMySceneComponent) new MyWeldingGroups());
      MyEntities.m_sceneComponents.Add((IMySceneComponent) new MyGridPhysicalHierarchy());
      MyEntities.m_sceneComponents.Add((IMySceneComponent) new MySharedTensorsGroups());
      MyEntities.m_sceneComponents.Add((IMySceneComponent) new MyFixedGrids());
    }

    public static void LoadData()
    {
      MyEntities.m_entities.Clear();
      MyEntities.m_entitiesToDelete.Clear();
      MyEntities.m_entitiesToDeleteNextFrame.Clear();
      MyEntities.m_cameraSphere = (HkShape) new HkSphereShape(0.125f);
      MyEntities.AddComponents();
      foreach (IMySceneComponent sceneComponent in MyEntities.m_sceneComponents)
        sceneComponent.Load();
      MyEntities.m_creationThread = new MyEntityCreationThread();
      MyEntities.m_isLoaded = true;
    }

    public static void UnloadData()
    {
      if (MyEntities.m_isLoaded)
        MyEntities.m_cameraSphere.RemoveReference();
      using (MyEntities.UnloadDataLock.AcquireExclusiveUsing())
      {
        MyEntities.m_creationThread.Dispose();
        MyEntities.m_creationThread = (MyEntityCreationThread) null;
        MyEntities.CloseAll();
        MyEntities.m_overlapRBElementList = (List<MyEntity>) null;
        MyEntities.m_entityResultSet = (HashSet<IMyEntity>) null;
        MyEntities.m_isLoaded = false;
        lock (MyEntities.m_entityInputListCollection)
        {
          foreach (List<MyEntity> entityInputList in MyEntities.m_entityInputListCollection)
            entityInputList.Clear();
        }
        lock (MyEntities.m_overlapRBElementListCollection)
        {
          foreach (List<MyEntity> overlapRbElementList in MyEntities.m_overlapRBElementListCollection)
            overlapRbElementList.Clear();
        }
        lock (MyEntities.m_entityResultSetCollection)
        {
          foreach (HashSet<IMyEntity> entityResultSet in MyEntities.m_entityResultSetCollection)
            entityResultSet.Clear();
        }
        lock (MyEntities.m_allIgnoredEntitiesCollection)
        {
          foreach (List<MyEntity> allIgnoredEntities in MyEntities.m_allIgnoredEntitiesCollection)
            allIgnoredEntities.Clear();
        }
      }
      for (int index = MyEntities.m_sceneComponents.Count - 1; index >= 0; --index)
        MyEntities.m_sceneComponents[index].Unload();
      MyEntities.m_sceneComponents.Clear();
      MyEntities.OnEntityRemove = (Action<MyEntity>) null;
      MyEntities.OnEntityAdd = (Action<MyEntity>) null;
      MyEntities.OnEntityCreate = (Action<MyEntity>) null;
      MyEntities.OnEntityDelete = (Action<MyEntity>) null;
      MyEntities.Orchestrator.Unload();
      MyEntities.m_entitiesToDelete.Clear();
      MyEntities.m_entitiesToDeleteNextFrame.Clear();
      MyEntities.m_entities = new MyConcurrentHashSet<MyEntity>();
      MyEntities.m_entitiesForDraw = new ConcurrentCachingList<IMyEntity>();
      MyEntities.m_remapHelper = new MyEntityIdRemapHelper();
      MyEntities.m_renderObjectToEntityMap = new Dictionary<uint, IMyEntity>();
      MyEntities.m_entityNameDictionary.Clear();
      MyEntities.m_entitiesForBBoxDraw.Clear();
    }

    public static void Add(MyEntity entity, bool insertIntoScene = true)
    {
      if (insertIntoScene)
        entity.OnAddedToScene((object) entity);
      if (MyEntities.Exist(entity))
        return;
      if (entity is MyVoxelBase)
        MySession.Static.VoxelMaps.Add((MyVoxelBase) entity);
      MyEntities.m_entities.Add(entity);
      if (MyEntities.GetEntityById(entity.EntityId) == null)
        MyEntityIdentifier.AddEntityWithId((IMyEntity) entity);
      MyEntities.RaiseEntityAdd(entity);
    }

    public static void SetEntityName(MyEntity myEntity, bool possibleRename = true)
    {
      string str = (string) null;
      string name = myEntity.Name;
      if (possibleRename)
      {
        foreach (KeyValuePair<string, MyEntity> entityName in MyEntities.m_entityNameDictionary)
        {
          if (entityName.Value == myEntity)
          {
            MyEntities.m_entityNameDictionary.Remove<string, MyEntity>(entityName.Key);
            str = entityName.Key;
            break;
          }
        }
      }
      if (!string.IsNullOrEmpty(myEntity.Name))
      {
        MyEntity myEntity1;
        if (MyEntities.m_entityNameDictionary.TryGetValue(myEntity.Name, out myEntity1))
        {
          if (myEntity1 == myEntity)
            return;
        }
        else
          MyEntities.m_entityNameDictionary.TryAdd(myEntity.Name, myEntity);
      }
      if (MyEntities.OnEntityNameSet == null)
        return;
      MyEntities.OnEntityNameSet(myEntity, str, name);
    }

    public static bool IsNameExists(MyEntity entity, string name)
    {
      foreach (KeyValuePair<string, MyEntity> entityName in MyEntities.m_entityNameDictionary)
      {
        if (entityName.Key == name && entityName.Value != entity)
          return true;
      }
      return false;
    }

    public static bool EntityNameExists(string name) => MyEntities.m_entityNameDictionary.ContainsKey(name);

    public static bool Remove(MyEntity entity)
    {
      if (entity is MyVoxelBase)
        MySession.Static.VoxelMaps.RemoveVoxelMap((MyVoxelBase) entity);
      if (!MyEntities.m_entities.Remove(entity))
        return false;
      entity.OnRemovedFromScene((object) entity);
      MyEntities.RaiseEntityRemove(entity);
      return true;
    }

    public static void DeleteRememberedEntities()
    {
      MyEntities.CloseAllowed = true;
      while (MyEntities.m_entitiesToDelete.Count > 0)
      {
        using (MyEntities.EntityCloseLock.AcquireExclusiveUsing())
        {
          MyEntity entity = MyEntities.m_entitiesToDelete.FirstElement<MyEntity>();
          if (!entity.Pinned)
          {
            Action<MyEntity> onEntityDelete = MyEntities.OnEntityDelete;
            if (onEntityDelete != null)
              onEntityDelete(entity);
            entity.Delete();
          }
          else
          {
            MyEntities.Remove(entity);
            MyEntities.m_entitiesToDelete.Remove(entity);
            MyEntities.m_entitiesToDeleteNextFrame.Add(entity);
          }
        }
      }
      MyEntities.CloseAllowed = false;
      HashSet<MyEntity> entitiesToDelete = MyEntities.m_entitiesToDelete;
      MyEntities.m_entitiesToDelete = MyEntities.m_entitiesToDeleteNextFrame;
      MyEntities.m_entitiesToDeleteNextFrame = entitiesToDelete;
    }

    public static bool HasEntitiesToDelete() => MyEntities.m_entitiesToDelete.Count > 0;

    public static void RemoveFromClosedEntities(MyEntity entity)
    {
      if (MyEntities.m_entitiesToDelete.Count > 0)
        MyEntities.m_entitiesToDelete.Remove(entity);
      if (MyEntities.m_entitiesToDeleteNextFrame.Count <= 0)
        return;
      MyEntities.m_entitiesToDeleteNextFrame.Remove(entity);
    }

    public static void RemoveName(MyEntity entity)
    {
      if (string.IsNullOrEmpty(entity.Name))
        return;
      MyEntities.m_entityNameDictionary.Remove<string, MyEntity>(entity.Name);
    }

    public static bool Exist(MyEntity entity) => MyEntities.m_entities != null && MyEntities.m_entities.Contains(entity);

    public static void Close(MyEntity entity)
    {
      if (MyEntities.CloseAllowed)
      {
        MyEntities.m_entitiesToDeleteNextFrame.Add(entity);
      }
      else
      {
        if (MyEntities.m_entitiesToDelete.Contains(entity))
          return;
        using (MyEntities.EntityMarkForCloseLock.AcquireExclusiveUsing())
          MyEntities.m_entitiesToDelete.Add(entity);
      }
    }

    public static void CloseAll()
    {
      MyEntities.IsClosingAll = true;
      if (MyEntities.OnCloseAll != null)
        MyEntities.OnCloseAll();
      MyEntities.CloseAllowed = true;
      List<MyEntity> source = new List<MyEntity>();
      foreach (MyEntity entity in MyEntities.m_entities)
      {
        entity.Close();
        MyEntities.m_entitiesToDelete.Add(entity);
      }
      foreach (MyEntity myEntity in MyEntities.m_entitiesToDelete.ToArray<MyEntity>())
      {
        if (!myEntity.Pinned)
        {
          myEntity.Render.FadeOut = false;
          myEntity.Delete();
        }
        else
          source.Add(myEntity);
      }
      while (source.Count > 0)
      {
        MyEntity myEntity = source.First<MyEntity>();
        if (!myEntity.Pinned)
        {
          myEntity.Render.FadeOut = false;
          myEntity.Delete();
          source.Remove(myEntity);
        }
        else
          Thread.Sleep(10);
      }
      MyEntities.CloseAllowed = false;
      MyEntities.m_entitiesToDelete.Clear();
      MyEntityIdentifier.Clear();
      MyGamePruningStructure.Clear();
      MyRadioBroadcasters.Clear();
      MyEntities.m_entitiesForDraw.ApplyChanges();
      MyEntities.IsClosingAll = false;
    }

    public static void InvokeLater(Action action, string callerDebugName = null) => MyEntities.Orchestrator.InvokeLater(action, callerDebugName);

    public static void RegisterForUpdate(MyEntity entity)
    {
      if (entity.NeedsUpdate == MyEntityUpdateEnum.NONE && (!(entity is IMyParallelUpdateable parallelUpdateable) || parallelUpdateable.UpdateFlags == MyParallelUpdateFlags.NONE))
        return;
      MyEntities.Orchestrator.AddEntity(entity);
    }

    public static void RegisterForDraw(IMyEntity entity)
    {
      if (!entity.Render.NeedsDraw)
        return;
      if (!Sandbox.Engine.Platform.Game.IsDedicated)
        MyEntities.m_entitiesForDraw.Add(entity);
      entity.Render.SetVisibilityUpdates(true);
    }

    public static void UnregisterForUpdate(MyEntity entity, bool immediate = false) => MyEntities.Orchestrator.RemoveEntity(entity, immediate);

    public static void UnregisterForDraw(IMyEntity entity)
    {
      if (!Sandbox.Engine.Platform.Game.IsDedicated)
        MyEntities.m_entitiesForDraw.Remove(entity);
      entity.Render.SetVisibilityUpdates(false);
    }

    public static bool IsUpdateInProgress() => MyEntities.UpdateInProgress;

    public static bool IsCloseAllowed() => MyEntities.CloseAllowed;

    public static void UpdateBeforeSimulation()
    {
      if (!MySandboxGame.IsGameReady)
        return;
      MyEntities.UpdateInProgress = true;
      MyEntities.UpdateOnceBeforeFrame();
      MyEntities.Orchestrator.DispatchBeforeSimulation();
      MyEntities.UpdateInProgress = false;
    }

    public static void UpdateOnceBeforeFrame() => MyEntities.Orchestrator.DispatchOnceBeforeFrame();

    public static void Simulate()
    {
      if (!MySandboxGame.IsGameReady)
        return;
      MyEntities.UpdateInProgress = true;
      MyEntities.Orchestrator.DispatchSimulate();
      MyEntities.UpdateInProgress = false;
    }

    public static void UpdateAfterSimulation()
    {
      if (!MySandboxGame.IsGameReady)
        return;
      MyEntities.UpdateInProgress = true;
      MyEntities.Orchestrator.DispatchAfterSimulation();
      MyEntities.UpdateInProgress = false;
      MyEntities.DeleteRememberedEntities();
      if (MyMultiplayer.Static == null || !MyEntities.m_creationThread.AnyResult)
        return;
      do
        ;
      while (MyEntities.m_creationThread.ConsumeResult(MyMultiplayer.Static.ReplicationLayer.GetSimulationUpdateTime()));
    }

    public static void UpdatingStopped() => MyEntities.Orchestrator.DispatchUpdatingStopped();

    public static MyEntities.AsyncUpdateToken StartAsyncUpdateBlock()
    {
      MyEntities.IsAsyncUpdateInProgress = true;
      return new MyEntities.AsyncUpdateToken();
    }

    private static bool IsAnyRenderObjectVisible(MyEntity entity)
    {
      if (entity.MarkedForClose)
        MyLog.Default.WriteLine(string.Format("Entity {0} is closed.", (object) entity));
      if (entity.Render == null)
        MyLog.Default.WriteLine(string.Format("Entity {0} nas no render.", (object) entity));
      if (entity.Render.RenderObjectIDs == null)
        MyLog.Default.WriteLine(string.Format("Entity {0} nas no render object ids.", (object) entity));
      if (MyRenderProxy.VisibleObjectsRead == null)
        MyLog.Default.WriteLine("Set of visible objects is null.");
      foreach (uint renderObjectId in entity.Render.RenderObjectIDs)
      {
        if (MyRenderProxy.VisibleObjectsRead.Contains(renderObjectId))
          return true;
      }
      return false;
    }

    public static void Draw()
    {
      MyEntities.m_entitiesForDraw.ApplyChanges();
      foreach (MyEntity entity in MyEntities.m_entitiesForDraw)
      {
        if (MyEntities.IsAnyRenderObjectVisible(entity))
        {
          entity.PrepareForDraw();
          entity.Render.Draw();
        }
      }
      foreach (KeyValuePair<MyEntity, MyEntities.BoundingBoxDrawArgs> keyValuePair in MyEntities.m_entitiesForBBoxDraw)
      {
        MatrixD worldMatrix = keyValuePair.Key.WorldMatrix;
        BoundingBoxD localAabb = (BoundingBoxD) keyValuePair.Key.PositionComp.LocalAABB;
        MyEntities.BoundingBoxDrawArgs boundingBoxDrawArgs = keyValuePair.Value;
        localAabb.Min -= boundingBoxDrawArgs.InflateAmount;
        localAabb.Max += boundingBoxDrawArgs.InflateAmount;
        MatrixD worldToLocal = MatrixD.Invert(worldMatrix);
        MySimpleObjectDraw.DrawAttachedTransparentBox(ref worldMatrix, ref localAabb, ref boundingBoxDrawArgs.Color, keyValuePair.Key.Render.GetRenderObjectID(), ref worldToLocal, MySimpleObjectRasterizer.Wireframe, Vector3I.One, boundingBoxDrawArgs.LineWidth, lineMaterial: new MyStringId?(boundingBoxDrawArgs.LineMaterial), blendType: MyBillboard.BlendTypeEnum.LDR);
        if (keyValuePair.Value.WithAxis)
        {
          Color color = Color.Green;
          Vector4 vector4_1 = color.ToVector4();
          color = Color.Red;
          Vector4 vector4_2 = color.ToVector4();
          color = Color.Blue;
          Vector4 vector4_3 = color.ToVector4();
          MatrixD identity = MatrixD.Identity;
          identity.Forward = worldMatrix.Forward;
          identity.Up = worldMatrix.Up;
          identity.Right = worldMatrix.Right;
          Vector3D start = worldMatrix.Translation + Vector3D.Transform(localAabb.Center, identity);
          MySimpleObjectDraw.DrawLine(start, start + worldMatrix.Right * localAabb.Size.X / 2.0, new MyStringId?(MyEntities.GIZMO_LINE_MATERIAL_WHITE), ref vector4_2, 0.25f, MyBillboard.BlendTypeEnum.LDR);
          MySimpleObjectDraw.DrawLine(start, start + worldMatrix.Up * localAabb.Size.Y / 2.0, new MyStringId?(MyEntities.GIZMO_LINE_MATERIAL_WHITE), ref vector4_1, 0.25f, MyBillboard.BlendTypeEnum.LDR);
          MySimpleObjectDraw.DrawLine(start, start + worldMatrix.Forward * localAabb.Size.Z / 2.0, new MyStringId?(MyEntities.GIZMO_LINE_MATERIAL_WHITE), ref vector4_3, 0.25f, MyBillboard.BlendTypeEnum.LDR);
        }
      }
    }

    private static List<MyEntity> AllIgnoredEntities
    {
      get
      {
        if (MyEntities.m_allIgnoredEntities == null)
        {
          MyEntities.m_allIgnoredEntities = new List<MyEntity>();
          MyEntities.m_allIgnoredEntitiesCollection.Add(MyEntities.m_allIgnoredEntities);
        }
        return MyEntities.m_allIgnoredEntities;
      }
    }

    public static MyEntity GetIntersectionWithSphere(ref BoundingSphereD sphere) => MyEntities.GetIntersectionWithSphere(ref sphere, (MyEntity) null, (MyEntity) null, false, false);

    public static MyEntity GetIntersectionWithSphere(
      ref BoundingSphereD sphere,
      MyEntity ignoreEntity0,
      MyEntity ignoreEntity1)
    {
      return MyEntities.GetIntersectionWithSphere(ref sphere, ignoreEntity0, ignoreEntity1, false, true);
    }

    public static void GetIntersectionWithSphere(
      ref BoundingSphereD sphere,
      MyEntity ignoreEntity0,
      MyEntity ignoreEntity1,
      bool ignoreVoxelMaps,
      bool volumetricTest,
      ref List<MyEntity> result)
    {
      BoundingBoxD boundingBox = BoundingBoxD.CreateInvalid();
      boundingBox = boundingBox.Include(sphere);
      List<MyEntity> entitiesInAabb = MyEntities.GetEntitiesInAABB(ref boundingBox);
      foreach (MyEntity myEntity in entitiesInAabb)
      {
        if ((!ignoreVoxelMaps || !(myEntity is MyVoxelMap)) && (myEntity != ignoreEntity0 && myEntity != ignoreEntity1))
        {
          if (myEntity.GetIntersectionWithSphere(ref sphere))
            result.Add(myEntity);
          if (volumetricTest && myEntity is MyVoxelMap && (myEntity as MyVoxelMap).DoOverlapSphereTest((float) sphere.Radius, sphere.Center))
            result.Add(myEntity);
        }
      }
      entitiesInAabb.Clear();
    }

    public static MyEntity GetIntersectionWithSphere(
      ref BoundingSphereD sphere,
      MyEntity ignoreEntity0,
      MyEntity ignoreEntity1,
      bool ignoreVoxelMaps,
      bool volumetricTest,
      bool excludeEntitiesWithDisabledPhysics = false,
      bool ignoreFloatingObjects = true,
      bool ignoreHandWeapons = true)
    {
      BoundingBoxD boundingBox = BoundingBoxD.CreateInvalid();
      boundingBox = boundingBox.Include(sphere);
      MyEntity myEntity1 = (MyEntity) null;
      List<MyEntity> entitiesInAabb = MyEntities.GetEntitiesInAABB(ref boundingBox);
      foreach (MyEntity myEntity2 in entitiesInAabb)
      {
        if ((!ignoreVoxelMaps || !(myEntity2 is MyVoxelMap)) && (myEntity2 != ignoreEntity0 && myEntity2 != ignoreEntity1) && (!excludeEntitiesWithDisabledPhysics || myEntity2.Physics == null || myEntity2.Physics.Enabled) && ((!ignoreFloatingObjects || !(myEntity2 is MyFloatingObject) && !(myEntity2 is MyDebrisBase)) && (!ignoreHandWeapons || !(myEntity2 is IMyHandheldGunObject<MyDeviceBase>) && !(myEntity2.Parent is IMyHandheldGunObject<MyDeviceBase>))))
        {
          if (volumetricTest && myEntity2.IsVolumetric && myEntity2.DoOverlapSphereTest((float) sphere.Radius, sphere.Center))
          {
            myEntity1 = myEntity2;
            break;
          }
          if (myEntity2.GetIntersectionWithSphere(ref sphere))
          {
            myEntity1 = myEntity2;
            break;
          }
        }
      }
      entitiesInAabb.Clear();
      return myEntity1;
    }

    public static void OverlapAllLineSegment(
      ref LineD line,
      List<MyLineSegmentOverlapResult<MyEntity>> resultList)
    {
      MyGamePruningStructure.GetAllEntitiesInRay(ref line, resultList);
    }

    public static MyIntersectionResultLineTriangleEx? GetIntersectionWithLine(
      ref LineD line,
      MyEntity ignoreEntity0,
      MyEntity ignoreEntity1,
      bool ignoreChildren = false,
      bool ignoreFloatingObjects = true,
      bool ignoreHandWeapons = true,
      IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES,
      float timeFrame = 0.0f,
      bool ignoreObjectsWithoutPhysics = true,
      bool ignoreCharacters = false)
    {
      MyEntities.EntityResultSet.Clear();
      if (ignoreChildren)
      {
        if (ignoreEntity0 != null)
        {
          ignoreEntity0 = ignoreEntity0.GetBaseEntity();
          ignoreEntity0.Hierarchy.GetChildrenRecursive(MyEntities.EntityResultSet);
        }
        if (ignoreEntity1 != null)
        {
          ignoreEntity1 = ignoreEntity1.GetBaseEntity();
          ignoreEntity1.Hierarchy.GetChildrenRecursive(MyEntities.EntityResultSet);
        }
      }
      MyEntities.LineOverlapEntityList.Clear();
      MyGamePruningStructure.GetAllEntitiesInRay(ref line, MyEntities.LineOverlapEntityList);
      MyEntities.LineOverlapEntityList.Sort((IComparer<MyLineSegmentOverlapResult<MyEntity>>) MyLineSegmentOverlapResult<MyEntity>.DistanceComparer);
      MyIntersectionResultLineTriangleEx? a = new MyIntersectionResultLineTriangleEx?();
      RayD ray = new RayD(line.From, line.Direction);
      foreach (MyLineSegmentOverlapResult<MyEntity> lineOverlapEntity in MyEntities.LineOverlapEntityList)
      {
        if (a.HasValue)
        {
          double? nullable = lineOverlapEntity.Element.PositionComp.WorldAABB.Intersects(ray);
          if (nullable.HasValue && Vector3D.DistanceSquared(line.From, a.Value.IntersectionPointInWorldSpace) < nullable.Value * nullable.Value)
          {
            if (lineOverlapEntity.Distance <= 0.0)
              continue;
            break;
          }
        }
        MyEntity element = lineOverlapEntity.Element;
        if (element != ignoreEntity0 && element != ignoreEntity1 && (!ignoreChildren || !MyEntities.EntityResultSet.Contains((IMyEntity) element)) && (!ignoreObjectsWithoutPhysics || element.Physics != null && element.Physics.Enabled) && (!element.MarkedForClose && (!ignoreFloatingObjects || !(element is MyFloatingObject) && !(element is MyDebrisBase))) && ((!ignoreHandWeapons || !(element is IMyHandheldGunObject<MyDeviceBase>) && !(element.Parent is IMyHandheldGunObject<MyDeviceBase>)) && (!ignoreCharacters || !(element is MyCharacter))))
        {
          MyIntersectionResultLineTriangleEx? t = new MyIntersectionResultLineTriangleEx?();
          if ((double) timeFrame == 0.0 || element.Physics == null || ((double) element.Physics.LinearVelocity.LengthSquared() < 0.100000001490116 || !element.IsCCDForProjectiles))
          {
            element.GetIntersectionWithLine(ref line, out t, flags);
          }
          else
          {
            float num1 = element.Physics.LinearVelocity.Length() * timeFrame;
            float radius = element.PositionComp.LocalVolume.Radius;
            float num2 = 0.0f;
            Vector3D position = element.PositionComp.GetPosition();
            Vector3 vector3 = Vector3.Normalize(element.Physics.LinearVelocity);
            for (; !t.HasValue && (double) num2 < (double) num1; num2 += radius)
            {
              element.PositionComp.SetPosition(position + (Vector3D) (num2 * vector3));
              element.GetIntersectionWithLine(ref line, out t, flags);
            }
            element.PositionComp.SetPosition(position);
          }
          if (t.HasValue && t.Value.Entity != ignoreEntity0 && t.Value.Entity != ignoreEntity1 && (!ignoreChildren || !MyEntities.EntityResultSet.Contains(t.Value.Entity)))
            a = MyIntersectionResultLineTriangleEx.GetCloserIntersection(ref a, ref t);
        }
      }
      MyEntities.LineOverlapEntityList.Clear();
      return a;
    }

    public static MyConcurrentHashSet<MyEntity> GetEntities() => MyEntities.m_entities;

    public static MyEntity GetEntityById(long entityId, bool allowClosed = false) => MyEntityIdentifier.GetEntityById(entityId, allowClosed) as MyEntity;

    public static bool IsEntityIdValid(long entityId) => MyEntityIdentifier.GetEntityById(entityId, true) is MyEntity entityById && !entityById.GetTopMostParent((System.Type) null).MarkedForClose;

    public static MyEntity GetEntityByIdOrDefault(
      long entityId,
      MyEntity defaultValue = null,
      bool allowClosed = false)
    {
      IMyEntity entity;
      MyEntityIdentifier.TryGetEntity(entityId, out entity, allowClosed);
      return entity is MyEntity myEntity ? myEntity : defaultValue;
    }

    public static T GetEntityByIdOrDefault<T>(long entityId, T defaultValue = null, bool allowClosed = false) where T : MyEntity
    {
      IMyEntity entity;
      MyEntityIdentifier.TryGetEntity(entityId, out entity, allowClosed);
      return entity is T obj ? obj : defaultValue;
    }

    public static bool EntityExists(long entityId) => MyEntityIdentifier.ExistsById(entityId);

    public static bool TryGetEntityById(long entityId, out MyEntity entity, bool allowClosed = false) => MyEntityIdentifier.TryGetEntity<MyEntity>(entityId, out entity, allowClosed);

    public static bool TryGetEntityById<T>(long entityId, out T entity, bool allowClosed = false) where T : MyEntity
    {
      MyEntity entity1;
      int num = !MyEntityIdentifier.TryGetEntity<MyEntity>(entityId, out entity1, allowClosed) ? 0 : (entity1 is T ? 1 : 0);
      entity = entity1 as T;
      return num != 0;
    }

    public static MyEntity GetEntityByName(string name) => MyEntities.m_entityNameDictionary[name];

    public static bool TryGetEntityByName(string name, out MyEntity entity) => MyEntities.m_entityNameDictionary.TryGetValue(name, out entity);

    public static bool TryGetEntityByName<T>(string name, out T entity) where T : MyEntity
    {
      MyEntity myEntity;
      if (MyEntities.m_entityNameDictionary.TryGetValue(name, out myEntity) && myEntity is T obj)
      {
        entity = obj;
        return true;
      }
      entity = default (T);
      return false;
    }

    public static bool EntityExists(string name) => MyEntities.m_entityNameDictionary.ContainsKey(name);

    public static void RaiseEntityRemove(MyEntity entity)
    {
      if (MyEntities.OnEntityRemove == null)
        return;
      MyEntities.OnEntityRemove(entity);
    }

    public static void RaiseEntityAdd(MyEntity entity)
    {
      if (MyEntities.OnEntityAdd == null)
        return;
      MyEntities.OnEntityAdd(entity);
    }

    public static void SetTypeHidden(System.Type type, bool hidden)
    {
      if (hidden == MyEntities.m_hiddenTypes.Contains(type))
        return;
      if (hidden)
        MyEntities.m_hiddenTypes.Add(type);
      else
        MyEntities.m_hiddenTypes.Remove(type);
    }

    public static bool IsTypeHidden(System.Type type)
    {
      foreach (System.Type hiddenType in MyEntities.m_hiddenTypes)
      {
        if (hiddenType.IsAssignableFrom(type))
          return true;
      }
      return false;
    }

    public static bool IsVisible(IMyEntity entity) => !MyEntities.IsTypeHidden(entity.GetType());

    public static void UnhideAllTypes()
    {
      foreach (System.Type type in MyEntities.m_hiddenTypes.ToList<System.Type>())
        MyEntities.SetTypeHidden(type, false);
    }

    public static void DebugDrawGridStatistics()
    {
      MyEntities.m_cubeGridList.Clear();
      MyEntities.m_cubeGridHash.Clear();
      int num1 = 0;
      int num2 = 0;
      Vector2 screenCoord = new Vector2(100f, 0.0f);
      MyRenderProxy.DebugDrawText2D(screenCoord, "Detailed grid statistics", Color.Yellow, 1f);
      foreach (MyEntity entity in MyEntities.GetEntities())
      {
        if (entity is MyCubeGrid myCubeGrid)
        {
          MyEntities.m_cubeGridList.Add(entity as MyCubeGrid);
          MyEntities.m_cubeGridHash.Add(MyGridPhysicalHierarchy.Static.GetRoot(entity as MyCubeGrid));
          if ((myCubeGrid.NeedsUpdate & MyEntityUpdateEnum.EACH_FRAME) != MyEntityUpdateEnum.NONE)
          {
            MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD((BoundingBoxD) myCubeGrid.PositionComp.LocalAABB, myCubeGrid.PositionComp.WorldMatrixRef), Color.Red, 0.1f, true, true);
            ++num1;
          }
          if (myCubeGrid.NeedsPerFrameDraw)
            ++num2;
        }
      }
      MyEntities.m_cubeGridList = MyEntities.m_cubeGridList.OrderByDescending<MyCubeGrid, int>((Func<MyCubeGrid, int>) (x => x.BlocksCount)).ToList<MyCubeGrid>();
      float scale = 0.7f;
      screenCoord.Y += 50f;
      MyRenderProxy.DebugDrawText2D(screenCoord, "Grids by blocks (" + (object) MyEntities.m_cubeGridList.Count + "):", Color.Yellow, scale);
      screenCoord.Y += 30f;
      MyRenderProxy.DebugDrawText2D(screenCoord, "Grids needing update: " + (object) num1, Color.Yellow, scale);
      screenCoord.Y += 30f;
      MyRenderProxy.DebugDrawText2D(screenCoord, "Grids needing draw: " + (object) num2, Color.Yellow, scale);
      screenCoord.Y += 30f;
      foreach (MyCubeGrid cubeGrid in MyEntities.m_cubeGridList)
      {
        MyRenderProxy.DebugDrawText2D(screenCoord, cubeGrid.DisplayName + ": " + cubeGrid.BlocksCount.ToString() + "x", Color.Yellow, scale);
        screenCoord.Y += 20f;
      }
      screenCoord.Y = 0.0f;
      screenCoord.X += 800f;
      screenCoord.Y += 50f;
      MyEntities.m_cubeGridList = MyEntities.m_cubeGridHash.OrderByDescending<MyCubeGrid, int>((Func<MyCubeGrid, int>) (x => MyGridPhysicalHierarchy.Static.GetNode(x) == null ? 0 : MyGridPhysicalHierarchy.Static.GetNode(x).Children.Count)).ToList<MyCubeGrid>();
      MyEntities.m_cubeGridList.RemoveAll((Predicate<MyCubeGrid>) (x => MyGridPhysicalHierarchy.Static.GetNode(x) == null || MyGridPhysicalHierarchy.Static.GetNode(x).Children.Count == 0));
      MyRenderProxy.DebugDrawText2D(screenCoord, "Root grids (" + (object) MyEntities.m_cubeGridList.Count + "):", Color.Yellow, scale);
      screenCoord.Y += 30f;
      foreach (MyCubeGrid cubeGrid in MyEntities.m_cubeGridList)
      {
        int num3 = MyGridPhysicalHierarchy.Static.GetNode(cubeGrid) != null ? MyGridPhysicalHierarchy.Static.GetNode(cubeGrid).Children.Count : 0;
        MyRenderProxy.DebugDrawText2D(screenCoord, cubeGrid.DisplayName + ": " + num3.ToString() + "x", Color.Yellow, scale);
        screenCoord.Y += 20f;
      }
    }

    public static void DebugDrawStatistics()
    {
      MyEntities.Orchestrator.DebugDraw();
      MyEntities.m_typesStats.Clear();
      Vector2 zero = Vector2.Zero;
      foreach (object entity in MyEntities.m_entities)
      {
        string key = entity.GetType().Name.ToString();
        if (!MyEntities.m_typesStats.ContainsKey(key))
          MyEntities.m_typesStats.Add(key, 0);
        MyEntities.m_typesStats[key]++;
      }
      zero.X += 300f;
      zero.Y += 50f;
      float scale = 0.7f;
      zero.Y += 50f;
      MyRenderProxy.DebugDrawText2D(zero, "All entities:", Color.Yellow, scale);
      zero.Y += 30f;
      foreach (KeyValuePair<string, int> keyValuePair in (IEnumerable<KeyValuePair<string, int>>) MyEntities.m_typesStats.OrderByDescending<KeyValuePair<string, int>, int>((Func<KeyValuePair<string, int>, int>) (x => x.Value)))
      {
        MyRenderProxy.DebugDrawText2D(zero, keyValuePair.Key + ": " + keyValuePair.Value.ToString() + "x", Color.Yellow, scale);
        zero.Y += 20f;
      }
    }

    public static IMyEntity GetEntityFromRenderObjectID(uint renderObjectID)
    {
      using (MyEntities.m_renderObjectToEntityMapLock.AcquireSharedUsing())
      {
        IMyEntity myEntity = (IMyEntity) null;
        MyEntities.m_renderObjectToEntityMap.TryGetValue(renderObjectID, out myEntity);
        return myEntity;
      }
    }

    private static void DebugDrawGroups<TNode, TGroupData>(MyGroups<TNode, TGroupData> groups)
      where TNode : MyCubeGrid
      where TGroupData : IGroupData<TNode>, new()
    {
      int num1 = 0;
      foreach (MyGroups<TNode, TGroupData>.Group group in groups.Groups)
      {
        Color color1 = new Vector3((float) (num1++ % 15) / 15f, 1f, 1f).HSVtoColor();
        foreach (MyGroups<TNode, TGroupData>.Node node1 in group.Nodes)
        {
          try
          {
            foreach (MyGroups<TNode, TGroupData>.Node child in node1.Children)
              MyEntities.m_groupDebugHelper.Add((object) child);
            foreach (object obj in MyEntities.m_groupDebugHelper)
            {
              MyGroups<TNode, TGroupData>.Node node2 = (MyGroups<TNode, TGroupData>.Node) null;
              int num2 = 0;
              foreach (MyGroups<TNode, TGroupData>.Node child in node1.Children)
              {
                if (obj == child)
                {
                  node2 = child;
                  ++num2;
                }
              }
              MyRenderProxy.DebugDrawLine3D(node1.NodeData.PositionComp.WorldAABB.Center, node2.NodeData.PositionComp.WorldAABB.Center, color1, color1, false);
              BoundingBoxD worldAabb = node1.NodeData.PositionComp.WorldAABB;
              Vector3D center1 = worldAabb.Center;
              worldAabb = node2.NodeData.PositionComp.WorldAABB;
              Vector3D center2 = worldAabb.Center;
              MyRenderProxy.DebugDrawText3D((center1 + center2) * 0.5, num2.ToString(), color1, 1f, false);
            }
            Color color2 = new Color(color1.ToVector3() + 0.25f);
            MyRenderProxy.DebugDrawSphere(node1.NodeData.PositionComp.WorldAABB.Center, 0.2f, (Color) color2.ToVector3(), 0.5f, false, true);
            MyRenderProxy.DebugDrawText3D(node1.NodeData.PositionComp.WorldAABB.Center, node1.LinkCount.ToString(), color2, 1f, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
          }
          finally
          {
            MyEntities.m_groupDebugHelper.Clear();
          }
        }
      }
    }

    public static void DebugDraw()
    {
      MyEntityComponentsDebugDraw.DebugDraw();
      if (MyCubeGridGroups.Static != null)
      {
        if (MyDebugDrawSettings.DEBUG_DRAW_GRID_GROUPS_PHYSICAL)
          MyEntities.DebugDrawGroups<MyCubeGrid, MyGridPhysicalGroupData>(MyCubeGridGroups.Static.Physical);
        if (MyDebugDrawSettings.DEBUG_DRAW_GRID_GROUPS_LOGICAL)
          MyEntities.DebugDrawGroups<MyCubeGrid, MyGridLogicalGroupData>(MyCubeGridGroups.Static.Logical);
        if (MyDebugDrawSettings.DEBUG_DRAW_SMALL_TO_LARGE_BLOCK_GROUPS)
          MyCubeGridGroups.DebugDrawBlockGroups<MySlimBlock, MyBlockGroupData>(MyCubeGridGroups.Static.SmallToLargeBlockConnections);
        if (MyDebugDrawSettings.DEBUG_DRAW_DYNAMIC_PHYSICAL_GROUPS)
          MyEntities.DebugDrawGroups<MyCubeGrid, MyGridPhysicalDynamicGroupData>(MyCubeGridGroups.Static.PhysicalDynamic);
      }
      if (MyDebugDrawSettings.DEBUG_DRAW_PHYSICS || MyDebugDrawSettings.ENABLE_DEBUG_DRAW || MyFakes.SHOW_INVALID_TRIANGLES)
      {
        using (MyEntities.m_renderObjectToEntityMapLock.AcquireSharedUsing())
        {
          MyEntities.m_entitiesForDebugDraw.Clear();
          foreach (uint key in MyRenderProxy.VisibleObjectsRead)
          {
            IMyEntity myEntity;
            MyEntities.m_renderObjectToEntityMap.TryGetValue(key, out myEntity);
            if (myEntity != null)
            {
              IMyEntity topMostParent = myEntity.GetTopMostParent();
              if (!MyEntities.m_entitiesForDebugDraw.Contains(topMostParent))
                MyEntities.m_entitiesForDebugDraw.Add(topMostParent);
            }
          }
          if (MyDebugDrawSettings.DEBUG_DRAW_GRID_COUNTER)
            MyRenderProxy.DebugDrawText2D(new Vector2(700f, 0.0f), "Grid number: " + (object) MyCubeGrid.GridCounter, Color.Red, 1f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
          foreach (MyEntity entity in MyEntities.m_entities)
            MyEntities.m_entitiesForDebugDraw.Add((IMyEntity) entity);
          foreach (IMyEntity myEntity in MyEntities.m_entitiesForDebugDraw)
          {
            if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
              myEntity.DebugDraw();
            if (MyFakes.SHOW_INVALID_TRIANGLES)
              myEntity.DebugDrawInvalidTriangles();
          }
          if (MyDebugDrawSettings.DEBUG_DRAW_VELOCITIES | MyDebugDrawSettings.DEBUG_DRAW_INTERPOLATED_VELOCITIES | MyDebugDrawSettings.DEBUG_DRAW_RIGID_BODY_ACTIONS)
          {
            foreach (IMyEntity myEntity in MyEntities.m_entitiesForDebugDraw)
            {
              if (myEntity.Physics != null && Vector3D.Distance(MySector.MainCamera.Position, myEntity.WorldAABB.Center) < 500.0)
              {
                MyOrientedBoundingBoxD obb = new MyOrientedBoundingBoxD((BoundingBoxD) myEntity.LocalAABB, myEntity.WorldMatrix);
                if (MyDebugDrawSettings.DEBUG_DRAW_VELOCITIES)
                {
                  Color color = Color.Yellow;
                  if (myEntity.Physics.IsStatic)
                    color = Color.RoyalBlue;
                  else if (!myEntity.Physics.IsActive)
                    color = Color.Red;
                  MyRenderProxy.DebugDrawOBB(obb, color, 1f, false, false);
                  MyRenderProxy.DebugDrawLine3D(myEntity.WorldAABB.Center, myEntity.WorldAABB.Center + myEntity.Physics.LinearVelocity * 100f, Color.Green, Color.White, false);
                }
                if (MyDebugDrawSettings.DEBUG_DRAW_INTERPOLATED_VELOCITIES)
                {
                  HkRigidBody rigidBody = myEntity.Physics.RigidBody;
                  Vector3 velocity;
                  if ((HkReferenceObject) rigidBody != (HkReferenceObject) null && rigidBody.GetCustomVelocity(out velocity))
                  {
                    MyRenderProxy.DebugDrawOBB(obb, Color.RoyalBlue, 1f, false, false);
                    MyRenderProxy.DebugDrawLine3D(myEntity.WorldAABB.Center, myEntity.WorldAABB.Center + velocity * 100f, Color.Green, Color.White, false);
                  }
                }
              }
            }
          }
          MyEntities.m_entitiesForDebugDraw.Clear();
          if (MyDebugDrawSettings.DEBUG_DRAW_GAME_PRUNNING)
            MyGamePruningStructure.DebugDraw();
          if (MyDebugDrawSettings.DEBUG_DRAW_RADIO_BROADCASTERS)
            MyRadioBroadcasters.DebugDraw();
        }
        MyEntities.m_entitiesForDebugDraw.Clear();
      }
      if (MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_CLUSTERS)
      {
        if (MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_CLUSTERS != MyPhysics.DebugDrawClustersEnable && MySector.MainCamera != null)
          MyPhysics.DebugDrawClustersMatrix = MySector.MainCamera.WorldMatrix;
        MyPhysics.DebugDrawClusters();
      }
      MyPhysics.DebugDrawClustersEnable = MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_CLUSTERS;
      if (MyDebugDrawSettings.DEBUG_DRAW_ENTITY_STATISTICS)
        MyEntities.DebugDrawStatistics();
      if (!MyDebugDrawSettings.DEBUG_DRAW_GRID_STATISTICS)
        return;
      MyEntities.DebugDrawGridStatistics();
    }

    public static MyEntity CreateFromObjectBuilderAndAdd(
      MyObjectBuilder_EntityBase objectBuilder,
      bool fadeIn)
    {
      bool insertIntoScene = (objectBuilder.PersistentFlags & MyPersistentEntityFlags2.InScene) > MyPersistentEntityFlags2.None;
      if (MyFakes.ENABLE_LARGE_OFFSET && objectBuilder.PositionAndOrientation.Value.Position.X < 10000.0)
        objectBuilder.PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation()
        {
          Forward = objectBuilder.PositionAndOrientation.Value.Forward,
          Up = objectBuilder.PositionAndOrientation.Value.Up,
          Position = new SerializableVector3D((Vector3D) objectBuilder.PositionAndOrientation.Value.Position + new Vector3D(1000000000.0))
        });
      MyEntity entity = MyEntities.CreateFromObjectBuilder(objectBuilder, fadeIn);
      if (entity != null)
      {
        if (entity.EntityId == 0L)
          entity = (MyEntity) null;
        else
          MyEntities.Add(entity, insertIntoScene);
      }
      return entity;
    }

    public static void CreateAsync(
      MyObjectBuilder_EntityBase objectBuilder,
      bool addToScene,
      Action<MyEntity> doneHandler = null)
    {
      if (MyEntities.m_creationThread == null)
        return;
      MyEntities.m_creationThread.SubmitWork(objectBuilder, addToScene, doneHandler);
    }

    public static void InitAsync(
      MyEntity entity,
      MyObjectBuilder_EntityBase objectBuilder,
      bool addToScene,
      Action<MyEntity> doneHandler = null,
      double serializationTimestamp = 0.0,
      bool fadeIn = false)
    {
      if (MyEntities.m_creationThread == null)
        return;
      MyEntities.m_creationThread.SubmitWork(objectBuilder, addToScene, doneHandler, entity, serializationTimestamp: serializationTimestamp, fadeIn: fadeIn);
    }

    public static void ReleaseWaitingAsync(byte index, Dictionary<long, MatrixD> matrices) => MyEntities.m_creationThread.ReleaseWaiting(index, matrices);

    public static void CallAsync(MyEntity entity, Action<MyEntity> doneHandler) => MyEntities.InitAsync(entity, (MyObjectBuilder_EntityBase) null, false, doneHandler);

    public static void CallAsync(Action doneHandler) => MyEntities.InitAsync((MyEntity) null, (MyObjectBuilder_EntityBase) null, false, (Action<MyEntity>) (e => doneHandler()));

    public static bool MemoryLimitAddFailure { get; private set; }

    public static void MemoryLimitAddFailureReset() => MyEntities.MemoryLimitAddFailure = false;

    public static void RemapObjectBuilderCollection(
      IEnumerable<MyObjectBuilder_EntityBase> objectBuilders)
    {
      string[] array = objectBuilders.Select<MyObjectBuilder_EntityBase, string>((Func<MyObjectBuilder_EntityBase, string>) (x => x.Name)).ToArray<string>();
      if (MyEntities.m_remapHelper == null)
        MyEntities.m_remapHelper = new MyEntityIdRemapHelper();
      foreach (MyObjectBuilder_EntityBase objectBuilder in objectBuilders)
        objectBuilder.Remap((IMyRemapHelper) MyEntities.m_remapHelper);
      MyEntities.m_remapHelper.Clear();
      int index = 0;
      foreach (MyObjectBuilder_EntityBase objectBuilder in objectBuilders)
      {
        if (!string.IsNullOrEmpty(array[index]) && !MyEntities.EntityNameExists(array[index]))
          objectBuilder.Name = array[index];
        ++index;
      }
    }

    public static void RemapObjectBuilder(MyObjectBuilder_EntityBase objectBuilder)
    {
      if (MyEntities.m_remapHelper == null)
        MyEntities.m_remapHelper = new MyEntityIdRemapHelper();
      objectBuilder.Remap((IMyRemapHelper) MyEntities.m_remapHelper);
      MyEntities.m_remapHelper.Clear();
    }

    public static MyEntity CreateFromObjectBuilderNoinit(
      MyObjectBuilder_EntityBase objectBuilder)
    {
      return MyEntityFactory.CreateEntity((MyObjectBuilder_Base) objectBuilder);
    }

    public static MyEntity CreateFromObjectBuilderParallel(
      MyObjectBuilder_EntityBase objectBuilder,
      bool addToScene = false,
      Action<MyEntity> completionCallback = null,
      MyEntity entity = null,
      MyEntity relativeSpawner = null,
      Vector3D? relativeOffset = null,
      bool checkPosition = false,
      bool fadeIn = false)
    {
      if (entity == null)
      {
        entity = MyEntities.CreateFromObjectBuilderNoinit(objectBuilder);
        if (entity == null)
          return (MyEntity) null;
      }
      MyEntities.InitEntityData initData = new MyEntities.InitEntityData(objectBuilder, addToScene, completionCallback, entity, fadeIn, relativeSpawner, relativeOffset, checkPosition);
      Parallel.Start((Action) (() =>
      {
        if (!MyEntities.CallInitEntity((WorkData) initData))
          return;
        MySandboxGame.Static.Invoke((Action) (() => MyEntities.OnEntityInitialized((WorkData) initData)), "CreateFromObjectBuilderParallel(alreadyParallel: true)");
      }));
      return entity;
    }

    private static bool CallInitEntity(WorkData workData)
    {
      if (workData is MyEntities.InitEntityData initEntityData)
        return initEntityData.CallInitEntity().Success;
      workData.FlagAsFailed();
      return false;
    }

    private static void OnEntityInitialized(WorkData workData)
    {
      if (!(workData is MyEntities.InitEntityData initEntityData))
        workData.FlagAsFailed();
      else
        initEntityData.OnEntityInitialized();
    }

    public static MyEntity CreateFromObjectBuilder(
      MyObjectBuilder_EntityBase objectBuilder,
      bool fadeIn)
    {
      MyEntity objectBuilderNoinit = MyEntities.CreateFromObjectBuilderNoinit(objectBuilder);
      objectBuilderNoinit.Render.FadeIn = fadeIn;
      MyEntities.InitEntity(objectBuilder, ref objectBuilderNoinit);
      return objectBuilderNoinit;
    }

    public static bool InitEntity(
      MyObjectBuilder_EntityBase objectBuilder,
      ref MyEntity entity,
      bool tolerateBlacklistedPlanets = false)
    {
      if (entity != null)
      {
        try
        {
          entity.Init(objectBuilder);
        }
        catch (MyPlanetWhitelistException ex) when (tolerateBlacklistedPlanets)
        {
          MySandboxGame.Log.WriteLine("Planet skipped " + (object) ex);
          entity.EntityId = 0L;
          entity = (MyEntity) null;
          throw;
        }
        catch (Exception ex) when (!(ex is OutOfMemoryException))
        {
          MySandboxGame.Log.WriteLine("ERROR Entity init!: " + (object) ex);
          entity.EntityId = 0L;
          entity = (MyEntity) null;
          return false;
        }
      }
      return true;
    }

    public static bool Load(
      List<MyObjectBuilder_EntityBase> objectBuilders,
      out MyStringId? errorMessage)
    {
      MyGuiScreenLoading loading = MyScreenManager.GetFirstScreenOfType<MyGuiScreenLoading>();
      MyEntityIdentifier.AllocationSuspended = true;
      bool flag = true;
      try
      {
        if (objectBuilders != null)
        {
          ConcurrentQueue<MyEntities.InitEntityData> results = new ConcurrentQueue<MyEntities.InitEntityData>();
          if (MySandboxGame.Config.SyncRendering)
          {
            MyEntityIdentifier.PrepareSwapData();
            MyEntityIdentifier.SwapPerThreadData();
          }
          MyEntityContainerEventExtensions.SkipProcessingEvents(true);
          int entitiesLoaded = 0;
          Task task = new Task();
          if (MyPlatformGameSettings.SYNCHRONIZED_PLANET_LOADING)
            task = Parallel.Start((Action) (() =>
            {
              foreach (MyObjectBuilder_EntityBase objectBuilder in objectBuilders)
              {
                if (objectBuilder is MyObjectBuilder_Planet)
                {
                  bool success;
                  MyEntities.InitEntityData initEntityData = MyEntities.LoadEntity(objectBuilder, out success);
                  if (success)
                  {
                    Interlocked.Increment(ref entitiesLoaded);
                    if (initEntityData != null)
                      results.Enqueue(initEntityData);
                  }
                }
              }
            }), Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Loading, "LoadPlanets"), WorkPriority.VeryHigh);
          Parallel.For(0, objectBuilders.Count, (Action<int>) (i =>
          {
            MyObjectBuilder_EntityBase objectBuilder = objectBuilders[i];
            if (!MyPlatformGameSettings.SYNCHRONIZED_PLANET_LOADING || !(objectBuilder is MyObjectBuilder_Planet))
            {
              bool success;
              MyEntities.InitEntityData initEntityData = MyEntities.LoadEntity(objectBuilder, out success);
              if (success)
              {
                Interlocked.Increment(ref entitiesLoaded);
                if (initEntityData != null)
                  results.Enqueue(initEntityData);
              }
            }
            if (MyUtils.MainThread != Thread.CurrentThread)
              return;
            MyEntities.InitEntityData result;
            while (results.TryDequeue(out result))
              result?.OnEntityInitialized();
            loading?.DrawLoading();
          }), options: new WorkOptions?(Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Loading, "LoadEntities")));
          if (task.valid)
            task.WaitOrExecute();
          MyEntityContainerEventExtensions.SkipProcessingEvents(false);
          MyEntities.InitEntityData result1;
          while (results.TryDequeue(out result1))
            result1?.OnEntityInitialized();
          flag = entitiesLoaded == objectBuilders.Count;
          if (MySandboxGame.Config.SyncRendering)
            MyEntityIdentifier.ClearSwapDataAndRestore();
        }
        loading?.DrawLoading();
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine("Exceptions during entities load:");
        MyLog.Default.WriteLine(ex);
        if (ex is TaskException taskException && ((IEnumerable<Exception>) taskException.InnerExceptions).All<Exception>((Func<Exception, bool>) (x => x is MyPlanetWhitelistException)))
          errorMessage = new MyStringId?(MySpaceTexts.Notification_TooManyPlanets);
        else
          errorMessage = new MyStringId?();
        return false;
      }
      finally
      {
        MyEntityIdentifier.InEntityCreationBlock = false;
        MyEntityIdentifier.AllocationSuspended = false;
      }
      MyLog.Default.WriteLine("Entities loaded & initialized");
      errorMessage = new MyStringId?();
      return flag;
    }

    private static MyEntities.InitEntityData LoadEntity(
      MyObjectBuilder_EntityBase objectBuilder,
      out bool success)
    {
      success = true;
      if (objectBuilder is MyObjectBuilder_Character builderCharacter && MyMultiplayer.Static != null && (Sync.IsServer && !builderCharacter.IsPersistenceCharacter) && (!builderCharacter.IsStartingCharacterForLobby || Sandbox.Engine.Platform.Game.IsDedicated))
        return (MyEntities.InitEntityData) null;
      if (MyFakes.SKIP_VOXELS_DURING_LOAD && objectBuilder.TypeId == typeof (MyObjectBuilder_VoxelMap) && (objectBuilder as MyObjectBuilder_VoxelMap).StorageName != "BaseAsteroid")
        return (MyEntities.InitEntityData) null;
      try
      {
        MyEntity objectBuilderNoinit = MyEntities.CreateFromObjectBuilderNoinit(objectBuilder);
        if (objectBuilderNoinit != null)
        {
          MyEntities.InitEntityData initEntityData = new MyEntities.InitEntityData(objectBuilder, true, (Action<MyEntity>) null, objectBuilderNoinit, false);
          (bool Success, MyEntity Entity) tuple = initEntityData.CallInitEntity(true);
          success = tuple.Success;
          if (tuple.Entity != null)
            return initEntityData;
        }
        success = false;
        return (MyEntities.InitEntityData) null;
      }
      finally
      {
      }
    }

    internal static List<MyObjectBuilder_EntityBase> Save()
    {
      List<MyObjectBuilder_EntityBase> builderEntityBaseList = new List<MyObjectBuilder_EntityBase>();
      foreach (MyEntity entity in MyEntities.m_entities)
      {
        if (entity.Save && !MyEntities.m_entitiesToDelete.Contains(entity) && !entity.MarkedForClose)
        {
          entity.BeforeSave();
          MyObjectBuilder_EntityBase objectBuilder = entity.GetObjectBuilder(false);
          builderEntityBaseList.Add(objectBuilder);
        }
      }
      return builderEntityBaseList;
    }

    public static void EnableEntityBoundingBoxDraw(
      MyEntity entity,
      bool enable,
      Vector4? color = null,
      float lineWidth = 0.01f,
      Vector3? inflateAmount = null,
      MyStringId? lineMaterial = null,
      bool withAxis = false)
    {
      if (enable)
      {
        if (!MyEntities.m_entitiesForBBoxDraw.ContainsKey(entity))
          entity.OnClose += new Action<MyEntity>(MyEntities.entityForBBoxDraw_OnClose);
        MyEntities.m_entitiesForBBoxDraw[entity] = new MyEntities.BoundingBoxDrawArgs()
        {
          Color = (Color) (color ?? Vector4.One),
          LineWidth = lineWidth,
          InflateAmount = inflateAmount ?? Vector3.Zero,
          LineMaterial = lineMaterial ?? MyEntities.GIZMO_LINE_MATERIAL,
          WithAxis = withAxis
        };
      }
      else
      {
        MyEntities.m_entitiesForBBoxDraw.Remove<MyEntity, MyEntities.BoundingBoxDrawArgs>(entity);
        entity.OnClose -= new Action<MyEntity>(MyEntities.entityForBBoxDraw_OnClose);
      }
    }

    private static void entityForBBoxDraw_OnClose(MyEntity entity) => MyEntities.m_entitiesForBBoxDraw.Remove<MyEntity, MyEntities.BoundingBoxDrawArgs>(entity);

    public static MyEntity CreateFromComponentContainerDefinitionAndAdd(
      MyDefinitionId entityContainerDefinitionId,
      bool fadeIn,
      bool insertIntoScene = true)
    {
      if (!typeof (MyObjectBuilder_EntityBase).IsAssignableFrom((System.Type) entityContainerDefinitionId.TypeId))
        return (MyEntity) null;
      if (!MyComponentContainerExtension.TryGetContainerDefinition(entityContainerDefinitionId.TypeId, entityContainerDefinitionId.SubtypeId, out MyContainerDefinition _))
      {
        MySandboxGame.Log.WriteLine("Entity container definition not found: " + (object) entityContainerDefinitionId);
        return (MyEntity) null;
      }
      if (!(MyObjectBuilderSerializer.CreateNewObject(entityContainerDefinitionId.TypeId, entityContainerDefinitionId.SubtypeName) is MyObjectBuilder_EntityBase newObject))
      {
        MySandboxGame.Log.WriteLine("Entity builder was not created: " + (object) entityContainerDefinitionId);
        return (MyEntity) null;
      }
      if (insertIntoScene)
        newObject.PersistentFlags |= MyPersistentEntityFlags2.InScene;
      return MyEntities.CreateFromObjectBuilderAndAdd(newObject, fadeIn);
    }

    public static void RaiseEntityCreated(MyEntity entity)
    {
      Action<MyEntity> onEntityCreate = MyEntities.OnEntityCreate;
      if (onEntityCreate == null)
        return;
      onEntityCreate(entity);
    }

    public static MyEntity CreateEntityAndAdd(
      MyDefinitionId entityContainerId,
      bool fadeIn,
      bool setPosAndRot = false,
      Vector3? position = null,
      Vector3? up = null,
      Vector3? forward = null)
    {
      if (!MyDefinitionManager.Static.TryGetContainerDefinition(entityContainerId, out MyContainerDefinition _))
        return (MyEntity) null;
      if (!(MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) entityContainerId) is MyObjectBuilder_EntityBase newObject))
        return (MyEntity) null;
      if (setPosAndRot)
        newObject.PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation((Vector3D) (position.HasValue ? position.Value : Vector3.Zero), forward.HasValue ? forward.Value : Vector3.Forward, up.HasValue ? up.Value : Vector3.Up));
      return MyEntities.CreateFromObjectBuilderAndAdd(newObject, fadeIn);
    }

    public static MyEntity CreateEntity(
      MyDefinitionId entityContainerId,
      bool fadeIn,
      bool setPosAndRot = false,
      Vector3? position = null,
      Vector3? up = null,
      Vector3? forward = null)
    {
      if (!MyDefinitionManager.Static.TryGetContainerDefinition(entityContainerId, out MyContainerDefinition _))
        return (MyEntity) null;
      if (!(MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) entityContainerId) is MyObjectBuilder_EntityBase newObject))
        return (MyEntity) null;
      if (setPosAndRot)
        newObject.PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation((Vector3D) (position.HasValue ? position.Value : Vector3.Zero), forward.HasValue ? forward.Value : Vector3.Forward, up.HasValue ? up.Value : Vector3.Up));
      return MyEntities.CreateFromObjectBuilder(newObject, fadeIn);
    }

    public static void SendCloseRequest(IMyEntity entity) => MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (s => new Action<long>(MyEntities.OnEntityCloseRequest)), entity.EntityId);

    [Event(null, 2764)]
    [Reliable]
    [Server]
    private static void OnEntityCloseRequest(long entityId)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsCopyPastingEnabledForUser(MyEventContext.Current.Sender.Value) && (!MySession.Static.CreativeMode && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value)))
      {
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        MyEntity entity;
        MyEntities.TryGetEntityById(entityId, out entity);
        if (entity == null)
          return;
        MyLog.Default.Info(string.Format("OnEntityCloseRequest removed entity '{0}:{1}' with entity id '{2}'", (object) entity.Name, (object) entity.DisplayName, (object) entity.EntityId));
        MyVoxelBase myVoxelBase = entity as MyVoxelBase;
        if (MyMultiplayer.Static != null && myVoxelBase != null && (!myVoxelBase.Save && !myVoxelBase.ContentChanged) && !myVoxelBase.BeforeContentChanged)
          MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (s => new Action<long>(MyEntities.ForceCloseEntityOnClients)), entityId);
        entity.OnEntityCloseRequest.InvokeIfNotNull<MyEntity>(entity);
        if (entity.MarkedForClose)
          return;
        entity.Close();
      }
    }

    [Event(null, 2791)]
    [Reliable]
    [Broadcast]
    public static void ForceCloseEntityOnClients(long entityId)
    {
      MyEntity entity;
      MyEntities.TryGetEntityById(entityId, out entity);
      if (entity == null)
        return;
      entity.OnEntityCloseRequest.InvokeIfNotNull<MyEntity>(entity);
      if (entity.MarkedForClose)
        return;
      entity.Close();
    }

    [Conditional("DEBUG")]
    private static void AssertNoLeakingEntitiesInByNameDictionary()
    {
      MySession mySession = MySession.Static;
      if ((mySession != null ? ((uint) (mySession.GameplayFrameCounter % 100) > 0U ? 1 : 0) : 1) != 0)
        return;
      foreach (KeyValuePair<string, MyEntity> entityName in MyEntities.m_entityNameDictionary)
        ;
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct AsyncUpdateToken : IDisposable
    {
      public void Dispose() => MyEntities.IsAsyncUpdateInProgress = false;
    }

    public class InitEntityData : WorkData
    {
      private readonly MyObjectBuilder_EntityBase m_objectBuilder;
      private readonly bool m_addToScene;
      private readonly Action<MyEntity> m_completionCallback;
      private MyEntity m_entity;
      private List<IMyEntity> m_resultIDs;
      private readonly MyEntity m_relativeSpawner;
      private Vector3D? m_relativeOffset;
      private readonly bool m_checkPosition;
      private readonly bool m_fadeIn;

      public InitEntityData(
        MyObjectBuilder_EntityBase objectBuilder,
        bool addToScene,
        Action<MyEntity> completionCallback,
        MyEntity entity,
        bool fadeIn,
        MyEntity relativeSpawner = null,
        Vector3D? relativeOffset = null,
        bool checkPosition = false)
      {
        this.m_objectBuilder = objectBuilder;
        this.m_addToScene = addToScene;
        this.m_completionCallback = completionCallback;
        this.m_entity = entity;
        this.m_fadeIn = fadeIn;
        this.m_relativeSpawner = relativeSpawner;
        this.m_relativeOffset = relativeOffset;
        this.m_checkPosition = checkPosition;
      }

      public (bool Success, MyEntity Entity) CallInitEntity(bool tolerateBlacklistedPlanets = false)
      {
        try
        {
          MyEntityIdentifier.InEntityCreationBlock = true;
          MyEntityIdentifier.LazyInitPerThreadStorage(2048);
          this.m_entity.Render.FadeIn = this.m_fadeIn;
          return (MyEntities.InitEntity(this.m_objectBuilder, ref this.m_entity, tolerateBlacklistedPlanets), this.m_entity);
        }
        finally
        {
          this.m_resultIDs = new List<IMyEntity>();
          MyEntityIdentifier.GetPerThreadEntities(this.m_resultIDs);
          MyEntityIdentifier.ClearPerThreadEntities();
          MyEntityIdentifier.InEntityCreationBlock = false;
        }
      }

      public void OnEntityInitialized()
      {
        if (this.m_relativeSpawner != null && this.m_relativeOffset.HasValue)
        {
          MatrixD worldMatrix = this.m_entity.WorldMatrix;
          worldMatrix.Translation = this.m_relativeSpawner.WorldMatrix.Translation + this.m_relativeOffset.Value;
          this.m_entity.WorldMatrix = worldMatrix;
        }
        MyCubeGrid entity1 = this.m_entity as MyCubeGrid;
        if (MyFakes.ENABLE_GRID_PLACEMENT_TEST && this.m_checkPosition && (entity1 != null && entity1.CubeBlocks.Count == 1))
        {
          MyGridPlacementSettings placementSettings = MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.GetGridPlacementSettings(entity1.GridSizeEnum, entity1.IsStatic);
          if (!MyCubeGrid.TestPlacementArea(entity1, entity1.IsStatic, ref placementSettings, (BoundingBoxD) entity1.PositionComp.LocalAABB, false))
          {
            MyLog.Default.Info(string.Format("OnEntityInitialized removed entity '{0}:{1}' with entity id '{2}'", (object) entity1.Name, (object) entity1.DisplayName, (object) entity1.EntityId));
            this.m_entity.Close();
            return;
          }
        }
        foreach (IMyEntity resultId in this.m_resultIDs)
        {
          IMyEntity entity2;
          MyEntityIdentifier.TryGetEntity(resultId.EntityId, out entity2);
          if (entity2 != null)
          {
            MyLog.Default.WriteLineAndConsole("Dropping entity with duplicated id: " + (object) resultId.EntityId);
            resultId.Close();
          }
          else
            MyEntityIdentifier.AddEntityWithId(resultId);
        }
        if (this.m_entity == null || this.m_entity.EntityId == 0L)
          return;
        if (this.m_addToScene)
          MyEntities.Add(this.m_entity, (this.m_objectBuilder.PersistentFlags & MyPersistentEntityFlags2.InScene) > MyPersistentEntityFlags2.None);
        if (this.m_completionCallback == null)
          return;
        this.m_completionCallback(this.m_entity);
      }
    }

    private struct BoundingBoxDrawArgs
    {
      public Color Color;
      public float LineWidth;
      public Vector3 InflateAmount;
      public MyStringId LineMaterial;
      public bool WithAxis;
    }

    protected sealed class OnEntityCloseRequest\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyEntities.OnEntityCloseRequest(entityId);
      }
    }

    protected sealed class ForceCloseEntityOnClients\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyEntities.ForceCloseEntityOnClients(entityId);
      }
    }
  }
}
