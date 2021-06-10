// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Physics.MyPhysics
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using ParallelTasks;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Library.Memory;
using VRage.Library.Threading;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.Profiler;
using VRage.Utils;
using VRageMath;
using VRageMath.Spatial;
using VRageRender;

namespace Sandbox.Engine.Physics
{
  [MySessionComponentDescriptor(MyUpdateOrder.Simulation, 1000)]
  [StaticEventOwner]
  public class MyPhysics : MySessionComponentBase
  {
    private static int? m_currentUserThreadId;
    public static int ThreadId;
    public static MyClusterTree Clusters;
    private static bool ClustersNeedSync = false;
    private static HkJobThreadPool m_threadPool;
    private static HkJobQueue m_jobQueue;
    private static MyProfiler[] m_havokThreadProfilers;
    private bool m_updateKinematicBodies = MyFakes.ENABLE_ANIMATED_KINEMATIC_UPDATE && !Sync.IsServer;
    private List<MyPhysicsBody> m_iterationBodies = new List<MyPhysicsBody>();
    private List<HkCharacterRigidBody> m_characterIterationBodies = new List<HkCharacterRigidBody>();
    [ThreadStatic]
    private static List<MyEntity> m_tmpEntityResults = new List<MyEntity>();
    private int m_nextMemoryUpdate;
    private MyMemorySystem.AllocationRecord? m_currentAllocations;
    private static MyMemorySystem m_physicsMemorySystem = Singleton<MyMemoryTracker>.Instance.ProcessMemorySystem.RegisterSubsystem("Physics");
    private static Queue<long> m_timestamps = new Queue<long>(120);
    private MyWorldObserver m_worldObserver;
    private static SpinLockRef m_raycastLock = new SpinLockRef();
    private const string HkProfilerSymbol = "HkProfiler";
    private List<HkSimulationIslandInfo> m_simulationIslandInfos;
    private static Queue<MyPhysics.FractureImpactDetails> m_destructionQueue = new Queue<MyPhysics.FractureImpactDetails>();
    public static bool DebugDrawClustersEnable = false;
    public static MatrixD DebugDrawClustersMatrix = MatrixD.Identity;
    private static List<BoundingBoxD> m_clusterStaticObjects = new List<BoundingBoxD>();
    private static List<MyLineSegmentOverlapResult<MyVoxelBase>> m_foundEntities = new List<MyLineSegmentOverlapResult<MyVoxelBase>>();
    private static List<HkHitInfo?> m_resultShapeCasts;
    private static readonly HashSet<MyPhysicsBody> m_pendingCollisionFilterRefreshes = new HashSet<MyPhysicsBody>();
    public static bool SyncVDBCamera = false;
    private static MatrixD? ClientCameraWM;
    private static bool IsVDBRecording = false;
    private static string VDBRecordFile = (string) null;
    [ThreadStatic]
    private static List<HkHitInfo> m_resultHits;
    [ThreadStatic]
    private static List<MyClusterTree.MyClusterQueryResult> m_resultWorlds;
    private static List<HkShapeCollision> m_shapeCollisionResultsCache;
    private static MyPhysics.ParallelRayCastQuery m_pendingRayCasts;
    private static MyConcurrentPool<MyPhysics.DeliverData> m_pendingRayCastsParallelPool = new MyConcurrentPool<MyPhysics.DeliverData>(expectedAllocations: 100);
    private const int SWITCH_HISTERESIS_FRAMES = 10;
    private const float SLOW_FRAME_DEVIATION_FACTOR = 3f;
    private const float FAST_FRAME_DEVIATION_FACTOR = 1.5f;
    private const float SLOW_FRAME_ABSOLUTE_THRESHOLD = 30f;
    private int m_slowFrames;
    private int m_fastFrames;
    private bool m_optimizeNextFrame;
    private bool m_optimizationsEnabled;
    private float m_averageFrameTime;
    private const int NUM_FRAMES_TO_CONSIDER = 100;
    private const int NUM_SLOW_FRAMES_TO_CONSIDER = 1000;
    private List<IPhysicsStepOptimizer> m_optimizers;
    private List<MyTuple<HkWorld, MyTimeSpan>> m_timings = new List<MyTuple<HkWorld, MyTimeSpan>>();
    private bool ParallelSteppingInitialized;

    public static int StepsLastSecond => MyPhysics.m_timestamps.Count;

    public static float SimulationRatio => MyFakes.ENABLE_SIMSPEED_LOCKING || MyFakes.PRECISE_SIM_SPEED ? Sandbox.Engine.Platform.Game.SimulationRatio : (float) Math.Round((double) Math.Max(0.5f, (float) MyPhysics.StepsLastSecond) / 60.0, 2);

    public static float RestingVelocity => !MyPerGameSettings.BallFriendlyPhysics ? float.MaxValue : 3f;

    public static Queue<MyPhysics.ForceInfo> QueuedForces { get; private set; }

    public static SpinLockRef RaycastLock => MyPhysics.m_raycastLock;

    private static void InitCollisionFilters(HkWorld world)
    {
      world.DisableCollisionsBetween(16, 17);
      world.DisableCollisionsBetween(17, 18);
      world.DisableCollisionsBetween(16, 22);
      world.DisableCollisionsBetween(12, 13);
      world.DisableCollisionsBetween(12, 28);
      world.DisableCollisionsBetween(17, 15);
      world.DisableCollisionsBetween(17, 13);
      world.DisableCollisionsBetween(17, 28);
      world.DisableCollisionsBetween(17, 8);
      world.DisableCollisionsBetween(21, 13);
      world.DisableCollisionsBetween(21, 28);
      world.DisableCollisionsBetween(21, 15);
      world.DisableCollisionsBetween(21, 16);
      world.DisableCollisionsBetween(21, 17);
      world.DisableCollisionsBetween(21, 18);
      world.DisableCollisionsBetween(21, 22);
      world.DisableCollisionsBetween(21, 24);
      world.DisableCollisionsBetween(21, 8);
      world.DisableCollisionsBetween(25, 13);
      world.DisableCollisionsBetween(25, 28);
      world.DisableCollisionsBetween(25, 15);
      world.DisableCollisionsBetween(25, 18);
      world.DisableCollisionsBetween(25, 22);
      world.DisableCollisionsBetween(25, 16);
      world.DisableCollisionsBetween(25, 17);
      world.DisableCollisionsBetween(25, 20);
      world.DisableCollisionsBetween(25, 23);
      world.DisableCollisionsBetween(25, 10);
      world.DisableCollisionsBetween(25, 24);
      world.DisableCollisionsBetween(25, 25);
      world.DisableCollisionsBetween(25, 8);
      world.DisableCollisionsBetween(19, 13);
      world.DisableCollisionsBetween(19, 28);
      world.DisableCollisionsBetween(19, 15);
      world.DisableCollisionsBetween(19, 18);
      world.DisableCollisionsBetween(19, 22);
      world.DisableCollisionsBetween(19, 16);
      world.DisableCollisionsBetween(19, 17);
      world.DisableCollisionsBetween(19, 20);
      world.DisableCollisionsBetween(19, 23);
      world.DisableCollisionsBetween(19, 10);
      world.DisableCollisionsBetween(19, 21);
      world.DisableCollisionsBetween(19, 24);
      world.DisableCollisionsBetween(19, 25);
      world.DisableCollisionsBetween(19, 19);
      world.DisableCollisionsBetween(19, 8);
      world.DisableCollisionsBetween(6, 13);
      world.DisableCollisionsBetween(6, 28);
      world.DisableCollisionsBetween(6, 22);
      world.DisableCollisionsBetween(6, 16);
      world.DisableCollisionsBetween(6, 17);
      world.DisableCollisionsBetween(6, 20);
      world.DisableCollisionsBetween(6, 23);
      world.DisableCollisionsBetween(6, 10);
      world.DisableCollisionsBetween(6, 21);
      world.DisableCollisionsBetween(6, 24);
      world.DisableCollisionsBetween(6, 25);
      world.DisableCollisionsBetween(6, 6);
      if (MyPerGameSettings.PhysicsNoCollisionLayerWithDefault)
      {
        world.DisableCollisionsBetween(19, 0);
        world.DisableCollisionsBetween(6, 0);
      }
      world.DisableCollisionsBetween(24, 24);
      world.DisableCollisionsBetween(26, 13);
      world.DisableCollisionsBetween(26, 28);
      world.DisableCollisionsBetween(26, 15);
      world.DisableCollisionsBetween(26, 18);
      world.DisableCollisionsBetween(26, 22);
      world.DisableCollisionsBetween(26, 16);
      world.DisableCollisionsBetween(26, 17);
      world.DisableCollisionsBetween(26, 20);
      world.DisableCollisionsBetween(26, 21);
      world.DisableCollisionsBetween(26, 24);
      world.DisableCollisionsBetween(26, 25);
      world.DisableCollisionsBetween(26, 8);
      int num = Sync.IsServer ? 1 : 0;
      if (!MyFakes.ENABLE_CHARACTER_AND_DEBRIS_COLLISIONS)
      {
        world.DisableCollisionsBetween(20, 18);
        world.DisableCollisionsBetween(20, 22);
        world.DisableCollisionsBetween(20, 8);
        world.DisableCollisionsBetween(10, 18);
        world.DisableCollisionsBetween(10, 22);
        world.DisableCollisionsBetween(10, 8);
      }
      world.DisableCollisionsBetween(29, 28);
      world.DisableCollisionsBetween(29, 18);
      world.DisableCollisionsBetween(29, 19);
      world.DisableCollisionsBetween(29, 6);
      world.DisableCollisionsBetween(29, 20);
      world.DisableCollisionsBetween(29, 21);
      world.DisableCollisionsBetween(29, 22);
      world.DisableCollisionsBetween(29, 23);
      world.DisableCollisionsBetween(29, 10);
      world.DisableCollisionsBetween(29, 24);
      world.DisableCollisionsBetween(29, 25);
      world.DisableCollisionsBetween(29, 26);
      world.DisableCollisionsBetween(29, 27);
      world.DisableCollisionsBetween(30, 18);
      world.DisableCollisionsBetween(30, 19);
      world.DisableCollisionsBetween(30, 6);
      world.DisableCollisionsBetween(14, 14);
      world.DisableCollisionsBetween(14, 16);
      world.DisableCollisionsBetween(14, 15);
      world.DisableCollisionsBetween(14, 18);
      world.DisableCollisionsBetween(14, 19);
      world.DisableCollisionsBetween(14, 6);
      world.DisableCollisionsBetween(14, 20);
      world.DisableCollisionsBetween(14, 21);
      world.DisableCollisionsBetween(14, 22);
      world.DisableCollisionsBetween(14, 23);
      world.DisableCollisionsBetween(14, 10);
      world.DisableCollisionsBetween(14, 24);
      world.DisableCollisionsBetween(14, 25);
      world.DisableCollisionsBetween(14, 26);
      world.DisableCollisionsBetween(14, 27);
      world.DisableCollisionsBetween(14, 8);
      world.DisableCollisionsBetween(31, 13);
      world.DisableCollisionsBetween(31, 28);
      world.DisableCollisionsBetween(31, 15);
      world.DisableCollisionsBetween(31, 18);
      world.DisableCollisionsBetween(31, 22);
      world.DisableCollisionsBetween(31, 16);
      world.DisableCollisionsBetween(31, 17);
      world.DisableCollisionsBetween(31, 20);
      world.DisableCollisionsBetween(31, 23);
      world.DisableCollisionsBetween(31, 10);
      world.DisableCollisionsBetween(31, 21);
      world.DisableCollisionsBetween(31, 24);
      world.DisableCollisionsBetween(31, 25);
      world.DisableCollisionsBetween(31, 19);
      world.DisableCollisionsBetween(31, 6);
      world.DisableCollisionsBetween(31, 29);
      world.DisableCollisionsBetween(31, 30);
      world.DisableCollisionsBetween(31, 14);
      world.DisableCollisionsBetween(31, 26);
      world.DisableCollisionsBetween(31, 27);
      world.DisableCollisionsBetween(31, 8);
      if (!MyFakes.ENABLE_JETPACK_RAGDOLL_COLLISIONS)
        world.DisableCollisionsBetween(31, 31);
      if (MyVoxelPhysicsBody.UseLod1VoxelPhysics)
      {
        world.DisableCollisionsBetween(16, 28);
        world.DisableCollisionsBetween(17, 28);
        world.DisableCollisionsBetween(15, 28);
        world.DisableCollisionsBetween(14, 28);
        world.DisableCollisionsBetween(20, 28);
        world.DisableCollisionsBetween(23, 28);
        world.DisableCollisionsBetween(8, 28);
        world.DisableCollisionsBetween(24, 11);
        world.DisableCollisionsBetween(18, 11);
        world.DisableCollisionsBetween(22, 11);
        world.DisableCollisionsBetween(10, 11);
        world.DisableCollisionsBetween(31, 11);
        world.DisableCollisionsBetween(29, 11);
        world.DisableCollisionsBetween(26, 11);
        world.DisableCollisionsBetween(21, 11);
        world.DisableCollisionsBetween(19, 11);
        world.DisableCollisionsBetween(6, 11);
        world.DisableCollisionsBetween(25, 11);
        world.DisableCollisionsBetween(17, 11);
        world.DisableCollisionsBetween(12, 11);
      }
      world.DisableCollisionsBetween(9, 17);
      world.DisableCollisionsBetween(9, 21);
      world.DisableCollisionsBetween(9, 25);
      world.DisableCollisionsBetween(9, 19);
      world.DisableCollisionsBetween(9, 6);
      world.DisableCollisionsBetween(9, 26);
      world.DisableCollisionsBetween(9, 14);
      world.DisableCollisionsBetween(9, 31);
      world.DisableCollisionsBetween(9, 28);
      world.DisableCollisionsBetween(9, 11);
      if (!Sync.IsServer)
        world.DisableCollisionsBetween(9, 22);
      world.DisableCollisionsBetween(7, 17);
      world.DisableCollisionsBetween(7, 21);
      world.DisableCollisionsBetween(7, 25);
      world.DisableCollisionsBetween(7, 19);
      world.DisableCollisionsBetween(7, 6);
      world.DisableCollisionsBetween(7, 26);
      world.DisableCollisionsBetween(7, 14);
      world.DisableCollisionsBetween(7, 31);
      world.DisableCollisionsBetween(7, 28);
      world.DisableCollisionsBetween(7, 11);
    }

    public static bool CheckThread() => Thread.CurrentThread.ManagedThreadId == (MyPhysics.m_currentUserThreadId ?? MyPhysics.ThreadId);

    [Conditional("DEBUG")]
    [DebuggerStepThrough]
    public static void AssertThread()
    {
    }

    public override void LoadData()
    {
      HkVDB.Port = Sync.IsServer ? 25001 : 25002;
      HkBaseSystem.EnableAssert(-668493307, false);
      HkBaseSystem.EnableAssert(952495168, false);
      HkBaseSystem.EnableAssert(1501626980, false);
      HkBaseSystem.EnableAssert(-258736554, false);
      HkBaseSystem.EnableAssert(524771844, false);
      HkBaseSystem.EnableAssert(1081361407, false);
      HkBaseSystem.EnableAssert(-1383504214, false);
      HkBaseSystem.EnableAssert(-265005969, false);
      HkBaseSystem.EnableAssert(1976984315, false);
      HkBaseSystem.EnableAssert(-252450131, false);
      HkBaseSystem.EnableAssert(-1400416854, false);
      MyPhysics.ThreadId = Thread.CurrentThread.ManagedThreadId;
      MyPhysics.Clusters = new MyClusterTree(MySession.Static.WorldBoundaries, MyFakes.MP_SYNC_CLUSTERTREE && !Sync.IsServer);
      MyPhysics.Clusters.OnClusterCreated += new Func<int, BoundingBoxD, object>(this.OnClusterCreated);
      MyPhysics.Clusters.OnClusterRemoved += new Action<object, int>(this.OnClusterRemoved);
      MyPhysics.Clusters.OnFinishBatch += new Action<object>(this.OnFinishBatch);
      MyPhysics.Clusters.OnClustersReordered += new Action(this.Tree_OnClustersReordered);
      MyPhysics.Clusters.GetEntityReplicableExistsById += new Func<long, bool>(this.GetEntityReplicableExistsById);
      if (Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.Settings.EnableSelectivePhysicsUpdates)
        this.m_worldObserver = new MyWorldObserver(MyPhysics.Clusters);
      MyPhysics.QueuedForces = new Queue<MyPhysics.ForceInfo>();
      if (MyFakes.ENABLE_HAVOK_MULTITHREADING)
      {
        this.ParallelSteppingInitialized = false;
        MyPhysics.m_threadPool = new HkJobThreadPool();
        MyPhysics.m_jobQueue = new HkJobQueue(MyPhysics.m_threadPool.ThreadCount + 1);
      }
      HkCylinderShape.SetNumberOfVirtualSideSegments(32);
      this.InitStepOptimizer();
    }

    private HkWorld OnClusterCreated(int clusterId, BoundingBoxD bbox) => MyPhysics.CreateHkWorld((float) bbox.Size.Max());

    private void OnClusterRemoved(object world, int clusterId)
    {
      HkWorld hkWorld = (HkWorld) world;
      if ((HkReferenceObject) hkWorld.DestructionWorld != (HkReferenceObject) null)
      {
        hkWorld.DestructionWorld.Dispose();
        hkWorld.DestructionWorld = (HkdWorld) null;
      }
      hkWorld.Dispose();
      if (this.m_worldObserver == null)
        return;
      this.m_worldObserver.RemoveCluster(clusterId);
    }

    private void OnFinishBatch(object world) => ((HkWorld) world).FinishBatch();

    public static HkWorld CreateHkWorld(float broadphaseSize = 100000f)
    {
      HkWorld.CInfo worldCinfo = MyPhysics.CreateWorldCInfo(MyPerGameSettings.EnableGlobalGravity, broadphaseSize, MyFakes.WHEEL_SOFTNESS ? float.MaxValue : MyPhysics.RestingVelocity, MyFakes.ENABLE_HAVOK_MULTITHREADING, MySession.Static.Settings.PhysicsIterations);
      HkWorld world = new HkWorld(ref worldCinfo);
      world.MarkForWrite();
      if (MySession.Static.Settings.WorldSizeKm > 0)
        world.EntityLeftWorld += new HkEntityHandler(MyPhysics.HavokWorld_EntityLeftWorld);
      if (MyPerGameSettings.Destruction && Sync.IsServer)
        world.DestructionWorld = new HkdWorld(world);
      if (MyFakes.ENABLE_HAVOK_MULTITHREADING)
        world.InitMultithreading(MyPhysics.m_threadPool, MyPhysics.m_jobQueue);
      world.DeactivationRotationSqrdA /= 3f;
      world.DeactivationRotationSqrdB /= 3f;
      MyPhysics.InitCollisionFilters(world);
      return world;
    }

    private static HkWorld.CInfo CreateWorldCInfo(
      bool enableGlobalGravity,
      float broadphaseCubeSideLength,
      float contactRestingVelocity,
      bool enableMultithreading,
      int solverIterations)
    {
      HkWorld.CInfo cinfo = HkWorld.CInfo.Create();
      cinfo.Gravity = enableGlobalGravity ? new Vector3(0.0, -9.8, 0.0) : Vector3.Zero;
      cinfo.BroadPhaseWorldAabb = new BoundingBox(new Vector3(broadphaseCubeSideLength * -0.5f), new Vector3(broadphaseCubeSideLength * 0.5f));
      cinfo.ContactPointGeneration = HkWorld.ContactPointGeneration.CONTACT_POINT_REJECT_DUBIOUS;
      cinfo.SolverTau = 0.6f;
      cinfo.SolverDamp = 1f;
      cinfo.SolverIterations = solverIterations < 8 ? 8 : solverIterations;
      cinfo.SimulationType = enableMultithreading ? HkWorld.SimulationType.SIMULATION_TYPE_MULTITHREADED : HkWorld.SimulationType.SIMULATION_TYPE_CONTINUOUS;
      cinfo.BroadPhaseNumMarkers = 0;
      cinfo.BroadPhaseBorderBehaviour = HkWorld.BroadPhaseBorderBehaviour.BROADPHASE_BORDER_REMOVE_ENTITY;
      cinfo.CollisionTolerance = 0.1f;
      cinfo.FireCollisionCallbacks = true;
      cinfo.ContactRestingVelocity = (double) contactRestingVelocity >= 3.40282001837566E+38 ? 3.40282E+38f : contactRestingVelocity;
      cinfo.ExpectedMinPsiDeltaTime = 0.01666667f;
      cinfo.SolverMicrosteps = 2;
      cinfo.MinDesiredIslandSize = 2U;
      return cinfo;
    }

    private static void HavokWorld_EntityLeftWorld(HkEntity hkEntity)
    {
      List<IMyEntity> allEntities = hkEntity.GetAllEntities();
      foreach (IMyEntity myEntity in allEntities)
      {
        if (Sync.IsServer)
        {
          switch (myEntity)
          {
            case null:
            case MyVoxelMap _:
            case MyCubeBlock _:
              continue;
            case MyCharacter _:
              ((MyCharacter) myEntity).DoDamage(1000f, MyDamageType.Suicide, true, 0L);
              continue;
            case MyCubeGrid _:
              MyCubeGrid.KillAllCharacters(myEntity as MyCubeGrid);
              MyLog.Default.Info(string.Format("HavokWorld_EntityLeftWorld removed entity '{0}:{1}' with entity id '{2}'", (object) myEntity.Name, (object) myEntity.DisplayName, (object) myEntity.EntityId));
              myEntity.Close();
              continue;
            case MyFloatingObject _:
              MyFloatingObjects.RemoveFloatingObject((MyFloatingObject) myEntity);
              continue;
            case MyFracturedPiece _:
              MyFracturedPiecesManager.Static.RemoveFracturePiece((MyFracturedPiece) myEntity, 0.0f);
              continue;
            default:
              myEntity.Close();
              continue;
          }
        }
      }
      allEntities.Clear();
    }

    protected override void UnloadData()
    {
      if (this.m_worldObserver != null)
        this.m_worldObserver.CleanUp(MyPhysics.Clusters);
      MyPhysics.Clusters.Dispose();
      MyPhysics.Clusters.OnClusterCreated -= new Func<int, BoundingBoxD, object>(this.OnClusterCreated);
      MyPhysics.Clusters.OnClusterRemoved -= new Action<object, int>(this.OnClusterRemoved);
      MyPhysics.Clusters = (MyClusterTree) null;
      MyPhysics.QueuedForces = (Queue<MyPhysics.ForceInfo>) null;
      if (MyFakes.ENABLE_HAVOK_MULTITHREADING)
      {
        MyPhysics.m_threadPool.Dispose();
        MyPhysics.m_threadPool = (HkJobThreadPool) null;
        MyPhysics.m_jobQueue.Dispose();
        MyPhysics.m_jobQueue = (HkJobQueue) null;
      }
      MyPhysics.m_destructionQueue.Clear();
      if (MyPerGameSettings.Destruction)
        HkdBreakableShape.DisposeSharedMaterial();
      this.UnloadStepOptimizer();
      this.CancelParallelRayCasts();
      ref MyMemorySystem.AllocationRecord? local = ref this.m_currentAllocations;
      if (local.HasValue)
        local.GetValueOrDefault().Dispose();
      this.m_currentAllocations = new MyMemorySystem.AllocationRecord?();
    }

    private void AddTimestamp()
    {
      long timestamp = Stopwatch.GetTimestamp();
      MyPhysics.m_timestamps.Enqueue(timestamp);
      long num = timestamp - Stopwatch.Frequency;
      while (MyPhysics.m_timestamps.Peek() < num)
        MyPhysics.m_timestamps.Dequeue();
    }

    private void UpdateMemoryStats()
    {
      if (this.m_nextMemoryUpdate-- > 0)
        return;
      this.m_nextMemoryUpdate = 100;
      long size = HkBaseSystem.GetCurrentMemoryConsumption();
      if (size > (long) int.MaxValue)
        size = (long) int.MaxValue;
      ref MyMemorySystem.AllocationRecord? local = ref this.m_currentAllocations;
      if (local.HasValue)
        local.GetValueOrDefault().Dispose();
      this.m_currentAllocations = new MyMemorySystem.AllocationRecord?(MyPhysics.m_physicsMemorySystem.RegisterAllocation("Pooled memory", size));
    }

    private static void ProfilerBegin(string block)
    {
    }

    private static void ProfilerEnd(long elapsedTicks) => MyTimeSpan.FromTicks(elapsedTicks);

    private void SimulateInternal()
    {
      this.ExecuteParallelRayCasts();
      MyPhysics.InsideSimulation = true;
      HkBaseSystem.OnSimulationFrameStarted((long) MySandboxGame.Static.SimulationFrameCounter);
      MyPhysics.StepVDB();
      MyPhysics.ProcessDestructions();
      this.StepWorlds();
      HkBaseSystem.OnSimulationFrameFinished();
      MyPhysics.InsideSimulation = false;
      MyPhysics.ReplayHavokTimers();
      this.DrawIslands();
      this.UpdateActiveRigidBodies();
      this.UpdateCharacters();
      this.EnsureClusterSpace();
    }

    private void UpdateCharacters()
    {
      foreach (HkWorld world in MyPhysics.Clusters.GetList())
        this.IterateCharacters(world);
      MyPhysics.Clusters.SuppressClusterReorder = true;
      foreach (HkCharacterRigidBody characterIterationBody in this.m_characterIterationBodies)
      {
        MyPhysicsBody userObject = (MyPhysicsBody) characterIterationBody.GetHitRigidBody().UserObject;
        int num = userObject.Entity.Parent != null ? 0 : (Vector3D.DistanceSquared(userObject.Entity.WorldMatrix.Translation, userObject.GetWorldMatrix().Translation) > 9.99999974737875E-05 ? 1 : 0);
        if (userObject.Entity is MyCharacter entity)
          entity.UpdatePhysicalMovement();
        if (num != 0)
          userObject.UpdateCluster();
      }
      MyPhysics.Clusters.SuppressClusterReorder = false;
    }

    private void UpdateActiveRigidBodies()
    {
      long num = 0;
      foreach (HkWorld world in MyPhysics.Clusters.GetList())
      {
        this.IterateBodies(world);
        num += (long) world.ActiveRigidBodies.Count;
      }
      MyPerformanceCounter.PerCameraDrawWrite["Active rigid bodies"] = (float) num;
      MyPhysics.Clusters.SuppressClusterReorder = true;
      foreach (MyPhysicsBody iterationBody in this.m_iterationBodies)
      {
        if (this.m_updateKinematicBodies && iterationBody.IsKinematic)
          iterationBody.OnMotionKinematic();
        else
          iterationBody.OnMotionDynamic();
      }
      MyPhysics.Clusters.SuppressClusterReorder = false;
    }

    private void DrawIslands()
    {
      if (!MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_SIMULATION_ISLANDS)
        return;
      foreach (MyClusterTree.MyCluster cluster in MyPhysics.Clusters.GetClusters())
      {
        Vector3D center = cluster.AABB.Center;
        HkWorld userData = (HkWorld) cluster.UserData;
        using (MyUtils.ReuseCollection<HkSimulationIslandInfo>(ref this.m_simulationIslandInfos))
        {
          userData.ReadSimulationIslandInfos(this.m_simulationIslandInfos);
          foreach (HkSimulationIslandInfo simulationIslandInfo in this.m_simulationIslandInfos)
          {
            BoundingBoxD aabb = new BoundingBoxD(simulationIslandInfo.AABB.Min + center, simulationIslandInfo.AABB.Max + center);
            if (aabb.Distance(MySector.MainCamera.Position) < 500.0 && simulationIslandInfo.IsActive)
              MyRenderProxy.DebugDrawAABB(aabb, simulationIslandInfo.IsActive ? Color.Red : Color.RoyalBlue);
          }
        }
      }
    }

    private static void ReplayHavokTimers()
    {
    }

    public override void Simulate()
    {
      if (!MySandboxGame.IsGameReady)
        return;
      this.AddTimestamp();
      this.UpdateMemoryStats();
      if (MyFakes.PAUSE_PHYSICS && !MyFakes.STEP_PHYSICS)
        return;
      MyFakes.STEP_PHYSICS = false;
      MySimpleProfiler.Begin("Physics", callingMember: nameof (Simulate));
      while (MyPhysics.QueuedForces.Count > 0)
      {
        MyPhysics.ForceInfo forceInfo = MyPhysics.QueuedForces.Dequeue();
        forceInfo.Body.AddForce(forceInfo.Type, forceInfo.Force, forceInfo.Position, forceInfo.Torque, forceInfo.MaxSpeed, true, forceInfo.ActiveOnly);
      }
      MyPhysics.ProcessCollisionFilterRefreshes();
      using (MyPhysics.m_raycastLock.Acquire())
        this.SimulateInternal();
      this.m_iterationBodies.Clear();
      this.m_characterIterationBodies.Clear();
      if (Sync.IsServer && MyFakes.MP_SYNC_CLUSTERTREE && MyPhysics.ClustersNeedSync)
      {
        List<BoundingBoxD> list = new List<BoundingBoxD>();
        MyPhysics.SerializeClusters(list);
        MyMultiplayer.RaiseStaticEvent<List<BoundingBoxD>>((Func<IMyEventOwner, Action<List<BoundingBoxD>>>) (s => new Action<List<BoundingBoxD>>(MyPhysics.OnClustersReordered)), list);
        MyPhysics.ClustersNeedSync = false;
      }
      MySimpleProfiler.End(nameof (Simulate));
    }

    private static void StepVDB()
    {
      if (!MyPhysics.ClientCameraWM.HasValue && !MyPhysics.SyncVDBCamera)
        return;
      MatrixD matrixD = MatrixD.Identity;
      if (MySector.MainCamera != null)
        matrixD = MySector.MainCamera.WorldMatrix;
      HkWorld world = (HkWorld) null;
      Vector3D vector3D = Vector3D.Zero;
      if (Sync.IsDedicated && MyPhysics.ClientCameraWM.HasValue)
      {
        MyClusterTree.MyCluster clusterForPosition = MyPhysics.Clusters.GetClusterForPosition(MyPhysics.ClientCameraWM.Value.Translation);
        if (clusterForPosition != null)
        {
          vector3D = -clusterForPosition.AABB.Center;
          world = (HkWorld) clusterForPosition.UserData;
        }
      }
      if (world == null)
      {
        if (MyFakes.VDB_ENTITY != null && MyFakes.VDB_ENTITY.GetTopMostParent((System.Type) null).GetPhysicsBody() != null)
        {
          MyPhysicsBody physicsBody = MyFakes.VDB_ENTITY.GetTopMostParent((System.Type) null).GetPhysicsBody();
          vector3D = physicsBody.WorldToCluster(Vector3D.Zero);
          world = physicsBody.HavokWorld;
        }
        else if (MySession.Static.ControlledEntity != null && MySession.Static.ControlledEntity.Entity.GetTopMostParent((System.Type) null).GetPhysicsBody() != null)
        {
          MyPhysicsBody physicsBody = MySession.Static.ControlledEntity.Entity.GetTopMostParent((System.Type) null).GetPhysicsBody();
          vector3D = physicsBody.WorldToCluster(Vector3D.Zero);
          world = physicsBody.HavokWorld;
        }
        else if (MyPhysics.Clusters.GetList().Count > 0)
        {
          MyClusterTree.MyCluster cluster = MyPhysics.Clusters.GetClusters()[0];
          vector3D = -cluster.AABB.Center;
          world = (HkWorld) cluster.UserData;
        }
      }
      if (world == null)
        return;
      HkVDB.SyncTimers(MyPhysics.m_threadPool);
      HkVDB.StepVDB(world, 0.01666667f);
      if (Sync.IsDedicated)
      {
        if (MyPhysics.ClientCameraWM.HasValue)
          matrixD = MyPhysics.ClientCameraWM.Value;
      }
      else if (!Sync.IsServer && MyPhysics.SyncVDBCamera)
        MyMultiplayer.RaiseStaticEvent<MatrixD>((Func<IMyEventOwner, Action<MatrixD>>) (x => new Action<MatrixD>(MyPhysics.UpdateServerDebugCamera)), matrixD);
      Vector3 up = (Vector3) matrixD.Up;
      Vector3 from = (Vector3) (matrixD.Translation + vector3D);
      Vector3 to = (Vector3) (from + matrixD.Forward);
      HkVDB.UpdateCamera(ref from, ref to, ref up);
      bool flag = MyPhysics.VDBRecordFile != null;
      if (MyPhysics.IsVDBRecording == flag)
        return;
      MyPhysics.IsVDBRecording = flag;
      if (flag)
        HkVDB.Capture(MyPhysics.VDBRecordFile);
      else
        HkVDB.EndCapture();
    }

    private static void ProcessDestructions()
    {
      int num = 0;
      while (MyPhysics.m_destructionQueue.Count > 0)
      {
        ++num;
        MyPhysics.FractureImpactDetails fractureImpactDetails = MyPhysics.m_destructionQueue.Dequeue();
        HkdFractureImpactDetails details = fractureImpactDetails.Details;
        if (details.IsValid())
        {
          details.Flag |= HkdFractureImpactDetails.Flags.FLAG_DONT_DELAY_OPERATION;
          for (int i1 = 0; i1 < details.GetBreakingBody().BreakableBody.BreakableShape.GetChildrenCount(); ++i1)
          {
            HkdShapeInstanceInfo child = details.GetBreakingBody().BreakableBody.BreakableShape.GetChild(i1);
            double strenght1 = (double) child.Shape.GetStrenght();
            for (int i2 = 0; i2 < child.Shape.GetChildrenCount(); ++i2)
            {
              double strenght2 = (double) child.Shape.GetChild(i2).Shape.GetStrenght();
            }
          }
          fractureImpactDetails.World.DestructionWorld.TriggerDestruction(ref details);
          MySyncDestructions.AddDestructionEffect(MyPerGameSettings.CollisionParticle.LargeGridClose, fractureImpactDetails.ContactInWorld, (Vector3) Vector3D.Forward, 0.2f);
          MySyncDestructions.AddDestructionEffect(MyPerGameSettings.DestructionParticle.DestructionHit, fractureImpactDetails.ContactInWorld, (Vector3) Vector3D.Forward, 0.1f);
        }
        fractureImpactDetails.Details.RemoveReference();
      }
    }

    private void IterateBodies(HkWorld world)
    {
      int worldVersion;
      bool cacheValid;
      List<HkRigidBody> rigidBodiesCache = world.GetActiveRigidBodiesCache(out worldVersion, out cacheValid);
      if (!cacheValid)
      {
        rigidBodiesCache.Clear();
        foreach (HkRigidBody activeRigidBody in world.ActiveRigidBodies)
        {
          MyPhysicsBody userObject = (MyPhysicsBody) activeRigidBody.UserObject;
          if (userObject != null && (!userObject.IsKinematic || this.m_updateKinematicBodies) && (userObject.Entity.Parent == null && activeRigidBody.Layer != 17))
            rigidBodiesCache.Add(activeRigidBody);
        }
        world.UpdateActiveRigidBodiesCache(rigidBodiesCache, worldVersion);
      }
      foreach (HkEntity hkEntity in rigidBodiesCache)
        this.m_iterationBodies.Add((MyPhysicsBody) hkEntity.UserObject);
    }

    private void IterateCharacters(HkWorld world)
    {
      foreach (HkCharacterRigidBody characterRigidBody in world.CharacterRigidBodies)
        this.m_characterIterationBodies.Add(characterRigidBody);
    }

    public static void ActivateInBox(ref BoundingBoxD box)
    {
      using (MyUtils.ReuseCollection<MyEntity>(ref MyPhysics.m_tmpEntityResults))
      {
        MyGamePruningStructure.GetTopMostEntitiesInBox(ref box, MyPhysics.m_tmpEntityResults, MyEntityQueryType.Dynamic);
        foreach (MyEntity tmpEntityResult in MyPhysics.m_tmpEntityResults)
        {
          if (tmpEntityResult.Physics != null && tmpEntityResult.Physics.Enabled && (HkReferenceObject) tmpEntityResult.Physics.RigidBody != (HkReferenceObject) null)
            tmpEntityResult.Physics.RigidBody.Activate();
        }
      }
    }

    public static void EnqueueDestruction(MyPhysics.FractureImpactDetails details) => MyPhysics.m_destructionQueue.Enqueue(details);

    public static void RemoveDestructions(MyEntity entity)
    {
      List<MyPhysics.FractureImpactDetails> list = MyPhysics.m_destructionQueue.ToList<MyPhysics.FractureImpactDetails>();
      for (int index = 0; index < list.Count; ++index)
      {
        if (list[index].Entity == entity)
        {
          list[index].Details.RemoveReference();
          list.RemoveAt(index);
          --index;
        }
      }
      MyPhysics.m_destructionQueue.Clear();
      foreach (MyPhysics.FractureImpactDetails fractureImpactDetails in list)
        MyPhysics.m_destructionQueue.Enqueue(fractureImpactDetails);
    }

    public static void RemoveDestructions(HkRigidBody body)
    {
      List<MyPhysics.FractureImpactDetails> list = MyPhysics.m_destructionQueue.ToList<MyPhysics.FractureImpactDetails>();
      for (int index = 0; index < list.Count; ++index)
      {
        if (!list[index].Details.IsValid() || (HkReferenceObject) list[index].Details.GetBreakingBody() == (HkReferenceObject) body)
        {
          list[index].Details.RemoveReference();
          list.RemoveAt(index);
          --index;
        }
      }
      MyPhysics.m_destructionQueue.Clear();
      foreach (MyPhysics.FractureImpactDetails fractureImpactDetails in list)
        MyPhysics.m_destructionQueue.Enqueue(fractureImpactDetails);
    }

    public static void DebugDrawClusters()
    {
      if (MyPhysics.Clusters == null)
        return;
      double num1 = 2000.0;
      MatrixD world = MatrixD.CreateWorld(MyPhysics.DebugDrawClustersMatrix.Translation + num1 * MyPhysics.DebugDrawClustersMatrix.Forward, Vector3D.Forward, Vector3D.Up);
      using (MyUtils.ReuseCollection<MyClusterTree.MyClusterQueryResult>(ref MyPhysics.m_resultWorlds))
      {
        MyPhysics.Clusters.GetAll(MyPhysics.m_resultWorlds);
        BoundingBoxD boundingBoxD = BoundingBoxD.CreateInvalid();
        foreach (MyClusterTree.MyClusterQueryResult resultWorld in MyPhysics.m_resultWorlds)
          boundingBoxD = boundingBoxD.Include(resultWorld.AABB);
        double num2 = boundingBoxD.Size.AbsMax();
        double num3 = num1 / num2;
        Vector3D center = boundingBoxD.Center;
        boundingBoxD.Min -= center;
        boundingBoxD.Max -= center;
        MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(new BoundingBoxD(boundingBoxD.Min * num3 * 1.01999998092651, boundingBoxD.Max * num3 * 1.01999998092651), world), Color.Green, 0.2f, false, false);
        MyRenderProxy.DebugDrawAxis(world, 50f, false);
        if (MySession.Static != null)
        {
          foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) Sync.Players.GetOnlinePlayers())
          {
            if (onlinePlayer.Character != null)
              MyRenderProxy.DebugDrawSphere(Vector3D.Transform((onlinePlayer.Character.PositionComp.GetPosition() - center) * num3, world), 1f, (Color) Vector3.One, depthRead: false);
          }
        }
        MyPhysics.Clusters.GetAllStaticObjects(MyPhysics.m_clusterStaticObjects);
        foreach (BoundingBoxD clusterStaticObject in MyPhysics.m_clusterStaticObjects)
          MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(new BoundingBoxD((clusterStaticObject.Min - center) * num3, (clusterStaticObject.Max - center) * num3), world), Color.Blue, 0.2f, false, false);
        foreach (MyClusterTree.MyClusterQueryResult resultWorld in MyPhysics.m_resultWorlds)
        {
          MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(new BoundingBoxD((resultWorld.AABB.Min - center) * num3, (resultWorld.AABB.Max - center) * num3), world), Color.White, 0.2f, false, false);
          foreach (HkCharacterRigidBody characterRigidBody in ((HkWorld) resultWorld.UserData).CharacterRigidBodies)
          {
            Vector3D pointFrom = Vector3D.Transform((resultWorld.AABB.Center + characterRigidBody.Position - center) * num3, world);
            Vector3D vector3D = Vector3D.TransformNormal((Vector3D) characterRigidBody.LinearVelocity, world) * 10.0;
            if (vector3D.Length() > 0.00999999977648258)
              MyRenderProxy.DebugDrawLine3D(pointFrom, pointFrom + vector3D, Color.Blue, Color.White, false);
          }
          foreach (HkRigidBody rigidBody in ((HkWorld) resultWorld.UserData).RigidBodies)
          {
            if (rigidBody.GetEntity(0U) != null)
            {
              MyOrientedBoundingBoxD obb = new MyOrientedBoundingBoxD((BoundingBoxD) rigidBody.GetEntity(0U).LocalAABB, rigidBody.GetEntity(0U).WorldMatrix);
              obb.Center = (obb.Center - center) * num3;
              obb.HalfExtent *= num3;
              obb.Transform(world);
              Color color = Color.Yellow;
              if ((double) rigidBody.GetEntity(0U).LocalAABB.Size.Max() > 1000.0)
                color = Color.Red;
              MyRenderProxy.DebugDrawOBB(obb, color, 1f, false, false);
              Vector3D vector3D = Vector3D.TransformNormal((Vector3D) rigidBody.LinearVelocity, world) * 10.0;
              if (vector3D.Length() > 0.00999999977648258)
                MyRenderProxy.DebugDrawLine3D(obb.Center, obb.Center + vector3D, Color.Red, Color.White, false);
              if (Vector3D.Distance(obb.Center, MySector.MainCamera.Position) < 10.0)
                MyRenderProxy.DebugDrawText3D(obb.Center, rigidBody.GetEntity(0U).ToString(), Color.White, 0.5f, false);
            }
          }
        }
      }
    }

    public static MyPhysics.HitInfo? CastLongRay(Vector3D from, Vector3D to, bool any = false)
    {
      using (MyPhysics.m_raycastLock.Acquire())
      {
        using (MyUtils.ReuseCollection<MyClusterTree.MyClusterQueryResult>(ref MyPhysics.m_resultWorlds))
        {
          MyPhysics.Clusters.CastRay(from, to, MyPhysics.m_resultWorlds);
          MyPhysics.HitInfo? nullable1 = new MyPhysics.HitInfo?();
          MyPhysics.HitInfo? nullable2 = MyPhysics.CastRayInternal(from, to, MyPhysics.m_resultWorlds, 9);
          if (nullable2.HasValue)
          {
            if (any)
              return nullable2;
            to = nullable2.Value.Position + nullable2.Value.Position;
          }
          LineD ray = new LineD(from, to);
          MyGamePruningStructure.GetVoxelMapsOverlappingRay(ref ray, MyPhysics.m_foundEntities);
          double num1 = 1.0;
          double num2 = 0.0;
          bool flag = false;
          foreach (MyLineSegmentOverlapResult<MyVoxelBase> foundEntity in MyPhysics.m_foundEntities)
          {
            if (foundEntity.Element.GetOrePriority() == -1)
            {
              MyVoxelBase rootVoxel = foundEntity.Element.RootVoxel;
              if (rootVoxel.Storage.DataProvider != null)
              {
                LineD line = new LineD(Vector3D.Transform(ray.From, rootVoxel.PositionComp.WorldMatrixInvScaled) + rootVoxel.SizeInMetresHalf, Vector3D.Transform(ray.To, rootVoxel.PositionComp.WorldMatrixInvScaled) + rootVoxel.SizeInMetresHalf);
                double startOffset;
                double endOffset;
                flag = rootVoxel.Storage.DataProvider.Intersect(ref line, out startOffset, out endOffset);
                if (flag)
                {
                  if (startOffset < num1)
                    num1 = startOffset;
                  if (endOffset > num2)
                    num2 = endOffset;
                }
              }
            }
          }
          if (!flag)
            return nullable2;
          to = from + ray.Direction * ray.Length * num2;
          from += ray.Direction * ray.Length * num1;
          MyPhysics.m_foundEntities.Clear();
          MyPhysics.HitInfo? nullable3 = MyPhysics.CastRayInternal(from, to, MyPhysics.m_resultWorlds, 28);
          return !nullable2.HasValue || nullable3.HasValue && nullable2.HasValue && (double) nullable3.Value.HkHitInfo.HitFraction < (double) nullable2.Value.HkHitInfo.HitFraction ? nullable3 : nullable2;
        }
      }
    }

    public static void GetPenetrationsShape(
      HkShape shape,
      ref Vector3D translation,
      ref Quaternion rotation,
      List<HkBodyCollision> results,
      int filter)
    {
      using (MyPhysics.m_raycastLock.Acquire())
      {
        using (MyUtils.ReuseCollection<MyClusterTree.MyClusterQueryResult>(ref MyPhysics.m_resultWorlds))
        {
          MyPhysics.Clusters.Intersects(translation, MyPhysics.m_resultWorlds);
          foreach (MyClusterTree.MyClusterQueryResult resultWorld in MyPhysics.m_resultWorlds)
          {
            Vector3 translation1 = (Vector3) (translation - resultWorld.AABB.Center);
            ((HkWorld) resultWorld.UserData).GetPenetrationsShape(shape, ref translation1, ref rotation, results, filter);
          }
        }
      }
    }

    public static float? CastShape(
      Vector3D to,
      HkShape shape,
      ref MatrixD transform,
      int filterLayer,
      float extraPenetration = 0.0f)
    {
      using (MyPhysics.m_raycastLock.Acquire())
      {
        using (MyUtils.ReuseCollection<MyClusterTree.MyClusterQueryResult>(ref MyPhysics.m_resultWorlds))
        {
          MyPhysics.Clusters.Intersects(to, MyPhysics.m_resultWorlds);
          if (MyPhysics.m_resultWorlds.Count == 0)
            return new float?();
          MyClusterTree.MyClusterQueryResult resultWorld = MyPhysics.m_resultWorlds[0];
          Matrix transform1 = (Matrix) ref transform;
          transform1.Translation = (Vector3) (transform.Translation - resultWorld.AABB.Center);
          Vector3 to1 = (Vector3) (to - resultWorld.AABB.Center);
          return ((HkWorld) resultWorld.UserData).CastShape(to1, shape, ref transform1, filterLayer, extraPenetration);
        }
      }
    }

    public static float? CastShapeInAllWorlds(
      Vector3D to,
      HkShape shape,
      ref MatrixD transform,
      int filterLayer,
      float extraPenetration = 0.0f)
    {
      using (MyPhysics.m_raycastLock.Acquire())
      {
        using (MyUtils.ReuseCollection<MyClusterTree.MyClusterQueryResult>(ref MyPhysics.m_resultWorlds))
        {
          MyPhysics.Clusters.CastRay(transform.Translation, to, MyPhysics.m_resultWorlds);
          foreach (MyClusterTree.MyClusterQueryResult resultWorld in MyPhysics.m_resultWorlds)
          {
            Matrix transform1 = (Matrix) ref transform;
            ref Matrix local = ref transform1;
            Vector3D translation = transform.Translation;
            BoundingBoxD aabb = resultWorld.AABB;
            Vector3D center1 = aabb.Center;
            Vector3 vector3 = (Vector3) (translation - center1);
            local.Translation = vector3;
            Vector3D vector3D = to;
            aabb = resultWorld.AABB;
            Vector3D center2 = aabb.Center;
            Vector3 to1 = (Vector3) (vector3D - center2);
            float? nullable = ((HkWorld) resultWorld.UserData).CastShape(to1, shape, ref transform1, filterLayer, extraPenetration);
            if (nullable.HasValue)
              return nullable;
          }
        }
      }
      return new float?();
    }

    public static Vector3D? CastShapeReturnPoint(
      Vector3D to,
      HkShape shape,
      ref MatrixD transform,
      int filterLayer,
      float extraPenetration)
    {
      using (MyPhysics.m_raycastLock.Acquire())
      {
        using (MyUtils.ReuseCollection<MyClusterTree.MyClusterQueryResult>(ref MyPhysics.m_resultWorlds))
        {
          MyPhysics.m_resultWorlds.Clear();
          MyPhysics.Clusters.Intersects(to, MyPhysics.m_resultWorlds);
          if (MyPhysics.m_resultWorlds.Count == 0)
            return new Vector3D?();
          MyClusterTree.MyClusterQueryResult resultWorld = MyPhysics.m_resultWorlds[0];
          Matrix transform1 = (Matrix) ref transform;
          transform1.Translation = (Vector3) (transform.Translation - resultWorld.AABB.Center);
          Vector3 to1 = (Vector3) (to - resultWorld.AABB.Center);
          Vector3? nullable = ((HkWorld) resultWorld.UserData).CastShapeReturnPoint(to1, shape, ref transform1, filterLayer, extraPenetration);
          return !nullable.HasValue ? new Vector3D?() : new Vector3D?((Vector3D) nullable.Value + resultWorld.AABB.Center);
        }
      }
    }

    public static HkContactPoint? CastShapeReturnContact(
      Vector3D to,
      HkShape shape,
      ref MatrixD transform,
      int filterLayer,
      float extraPenetration,
      out Vector3 worldTranslation)
    {
      using (MyPhysics.m_raycastLock.Acquire())
      {
        using (MyUtils.ReuseCollection<MyClusterTree.MyClusterQueryResult>(ref MyPhysics.m_resultWorlds))
        {
          MyPhysics.Clusters.Intersects(to, MyPhysics.m_resultWorlds);
          worldTranslation = Vector3.Zero;
          if (MyPhysics.m_resultWorlds.Count == 0)
            return new HkContactPoint?();
          MyClusterTree.MyClusterQueryResult resultWorld = MyPhysics.m_resultWorlds[0];
          worldTranslation = (Vector3) resultWorld.AABB.Center;
          Matrix transform1 = (Matrix) ref transform;
          transform1.Translation = (Vector3) (transform.Translation - resultWorld.AABB.Center);
          Vector3 to1 = (Vector3) (to - resultWorld.AABB.Center);
          return ((HkWorld) resultWorld.UserData).CastShapeReturnContact(to1, shape, ref transform1, filterLayer, extraPenetration);
        }
      }
    }

    public static HkContactPointData? CastShapeReturnContactData(
      Vector3D to,
      HkShape shape,
      ref MatrixD transform,
      uint collisionFilter,
      float extraPenetration,
      bool ignoreConvexShape = true)
    {
      using (MyPhysics.m_raycastLock.Acquire())
      {
        using (MyUtils.ReuseCollection<MyClusterTree.MyClusterQueryResult>(ref MyPhysics.m_resultWorlds))
        {
          MyPhysics.Clusters.Intersects(to, MyPhysics.m_resultWorlds);
          if (MyPhysics.m_resultWorlds.Count == 0)
            return new HkContactPointData?();
          MyClusterTree.MyClusterQueryResult resultWorld = MyPhysics.m_resultWorlds[0];
          Matrix transform1 = (Matrix) ref transform;
          transform1.Translation = (Vector3) (transform.Translation - resultWorld.AABB.Center);
          Vector3 to1 = (Vector3) (to - resultWorld.AABB.Center);
          HkContactPointData? nullable = ((HkWorld) resultWorld.UserData).CastShapeReturnContactData(to1, shape, ref transform1, collisionFilter, extraPenetration);
          if (!nullable.HasValue)
            return new HkContactPointData?();
          HkContactPointData contactPointData = nullable.Value;
          contactPointData.HitPosition += resultWorld.AABB.Center;
          return new HkContactPointData?(contactPointData);
        }
      }
    }

    public static MyPhysics.HitInfo? CastShapeReturnContactBodyData(
      Vector3D to,
      HkShape shape,
      ref MatrixD transform,
      uint collisionFilter,
      float extraPenetration,
      bool ignoreConvexShape = true)
    {
      using (MyPhysics.m_raycastLock.Acquire())
      {
        using (MyUtils.ReuseCollection<MyClusterTree.MyClusterQueryResult>(ref MyPhysics.m_resultWorlds))
        {
          MyPhysics.Clusters.Intersects(to, MyPhysics.m_resultWorlds);
          if (MyPhysics.m_resultWorlds.Count == 0)
            return new MyPhysics.HitInfo?();
          MyClusterTree.MyClusterQueryResult resultWorld = MyPhysics.m_resultWorlds[0];
          Matrix transform1 = (Matrix) ref transform;
          transform1.Translation = (Vector3) (transform.Translation - resultWorld.AABB.Center);
          Vector3 to1 = (Vector3) (to - resultWorld.AABB.Center);
          HkHitInfo? nullable = ((HkWorld) resultWorld.UserData).CastShapeReturnContactBodyData(to1, shape, ref transform1, collisionFilter, extraPenetration);
          if (!nullable.HasValue)
            return new MyPhysics.HitInfo?();
          HkHitInfo hi = nullable.Value;
          return new MyPhysics.HitInfo?(new MyPhysics.HitInfo(hi, hi.Position + resultWorld.AABB.Center));
        }
      }
    }

    public static bool CastShapeReturnContactBodyDatas(
      Vector3D to,
      HkShape shape,
      ref MatrixD transform,
      uint collisionFilter,
      float extraPenetration,
      List<MyPhysics.HitInfo> result,
      bool ignoreConvexShape = true)
    {
      using (MyPhysics.m_raycastLock.Acquire())
      {
        using (MyUtils.ReuseCollection<MyClusterTree.MyClusterQueryResult>(ref MyPhysics.m_resultWorlds))
        {
          MyPhysics.Clusters.Intersects(to, MyPhysics.m_resultWorlds);
          if (MyPhysics.m_resultWorlds.Count == 0)
            return false;
          MyClusterTree.MyClusterQueryResult resultWorld = MyPhysics.m_resultWorlds[0];
          Matrix transform1 = (Matrix) ref transform;
          transform1.Translation = (Vector3) (transform.Translation - resultWorld.AABB.Center);
          Vector3 to1 = (Vector3) (to - resultWorld.AABB.Center);
          using (MyUtils.ReuseCollection<HkHitInfo?>(ref MyPhysics.m_resultShapeCasts))
          {
            if (((HkWorld) resultWorld.UserData).CastShapeReturnContactBodyDatas(to1, shape, ref transform1, collisionFilter, extraPenetration, MyPhysics.m_resultShapeCasts))
            {
              foreach (HkHitInfo? resultShapeCast in MyPhysics.m_resultShapeCasts)
              {
                HkHitInfo hkHitInfo = resultShapeCast.Value;
                MyPhysics.HitInfo hitInfo = new MyPhysics.HitInfo()
                {
                  HkHitInfo = hkHitInfo,
                  Position = hkHitInfo.Position + resultWorld.AABB.Center
                };
                result.Add(hitInfo);
              }
              return true;
            }
          }
          return false;
        }
      }
    }

    public static bool IsPenetratingShapeShape(
      HkShape shape1,
      ref Vector3D translation1,
      ref Quaternion rotation1,
      HkShape shape2,
      ref Vector3D translation2,
      ref Quaternion rotation2)
    {
      using (MyPhysics.m_raycastLock.Acquire())
      {
        using (MyUtils.ReuseCollection<MyClusterTree.MyClusterQueryResult>(ref MyPhysics.m_resultWorlds))
        {
          rotation1.Normalize();
          rotation2.Normalize();
          MyPhysics.Clusters.Intersects(translation1, MyPhysics.m_resultWorlds);
          foreach (MyClusterTree.MyClusterQueryResult resultWorld in MyPhysics.m_resultWorlds)
          {
            BoundingBoxD aabb = resultWorld.AABB;
            if (aabb.Contains(translation2) != ContainmentType.Contains)
              return false;
            Vector3D vector3D1 = translation1;
            aabb = resultWorld.AABB;
            Vector3D center1 = aabb.Center;
            Vector3 translation1_1 = (Vector3) (vector3D1 - center1);
            Vector3D vector3D2 = translation2;
            aabb = resultWorld.AABB;
            Vector3D center2 = aabb.Center;
            Vector3 translation2_1 = (Vector3) (vector3D2 - center2);
            if (((HkWorld) resultWorld.UserData).IsPenetratingShapeShape(shape1, ref translation1_1, ref rotation1, shape2, ref translation2_1, ref rotation2))
              return true;
          }
          return false;
        }
      }
    }

    public static bool IsPenetratingShapeShape(
      HkShape shape1,
      ref Matrix transform1,
      HkShape shape2,
      ref Matrix transform2)
    {
      using (MyPhysics.m_raycastLock.Acquire())
        return (MyPhysics.Clusters.GetList()[0] as HkWorld).IsPenetratingShapeShape(shape1, ref transform1, shape2, ref transform2);
    }

    public static HkWorld SingleWorld => MyPhysics.Clusters.GetList()[0] as HkWorld;

    public static bool InsideSimulation { get; private set; }

    public static int GetCollisionLayer(string strLayer)
    {
      if (strLayer == "LightFloatingObjectCollisionLayer")
        return 10;
      if (strLayer == "VoxelLod1CollisionLayer")
        return 11;
      if (strLayer == "NotCollideWithStaticLayer")
        return 12;
      if (strLayer == "StaticCollisionLayer")
        return 13;
      if (strLayer == "CollideWithStaticLayer")
        return 14;
      if (strLayer == "DefaultCollisionLayer")
        return 15;
      if (strLayer == "DynamicDoubledCollisionLayer")
        return 16;
      if (strLayer == "KinematicDoubledCollisionLayer")
        return 17;
      if (strLayer == "CharacterCollisionLayer")
        return 18;
      if (strLayer == "NoCollisionLayer")
        return 19;
      if (strLayer == "TargetDummyLayer")
        return 6;
      if (strLayer == "DebrisCollisionLayer")
        return 20;
      if (strLayer == "GravityPhantomLayer")
        return 21;
      if (strLayer == "CharacterNetworkCollisionLayer")
        return 22;
      if (strLayer == "FloatingObjectCollisionLayer")
        return 23;
      if (strLayer == "ObjectDetectionCollisionLayer")
        return 24;
      if (strLayer == "VirtualMassLayer")
        return 25;
      if (strLayer == "CollectorCollisionLayer")
        return 26;
      if (strLayer == "AmmoLayer")
        return 27;
      if (strLayer == "VoxelCollisionLayer")
        return 28;
      if (strLayer == "ExplosionRaycastLayer")
        return 29;
      if (strLayer == "CollisionLayerWithoutCharacter")
        return 30;
      if (strLayer == "RagdollCollisionLayer")
        return 31;
      if (strLayer == "NoVoxelCollisionLayer")
        return 9;
      return strLayer == "MissileLayer" ? 8 : 15;
    }

    public static void EnsurePhysicsSpace(BoundingBoxD aabb)
    {
      using (MyPhysics.m_raycastLock.Acquire())
        MyPhysics.Clusters.EnsureClusterSpace(aabb);
    }

    public static void MoveObject(ulong id, BoundingBoxD aabb, Vector3 velocity) => MyPhysics.Clusters.MoveObject(id, aabb, velocity);

    public static void RemoveObject(ulong id) => MyPhysics.Clusters.RemoveObject(id);

    public static Vector3D GetObjectOffset(ulong id) => MyPhysics.Clusters.GetObjectOffset(id);

    public static ulong TryAddEntity(
      IMyEntity entity,
      MyPhysicsBody activationHandler,
      bool batchRequest)
    {
      ulong num;
      if (MyPhysics.Clusters != null)
      {
        string tag = entity is MyEntity myEntity ? myEntity.DebugName : string.Empty;
        BoundingBoxD worldAabb = entity.WorldAABB;
        long entityId = entity.EntityId;
        num = MyPhysics.Clusters.AddObject(worldAabb, (MyClusterTree.IMyActivationHandler) activationHandler, new ulong?(), tag, entityId, batchRequest);
      }
      else
        num = ulong.MaxValue;
      if (num == ulong.MaxValue)
        MyPhysics.HavokWorld_EntityLeftWorld((HkEntity) activationHandler.RigidBody);
      return num;
    }

    public static void RefreshCollisionFilter(MyPhysicsBody physicsBody) => MyPhysics.m_pendingCollisionFilterRefreshes.Add(physicsBody);

    private static void ProcessCollisionFilterRefreshes()
    {
      foreach (MyPhysicsBody collisionFilterRefresh in MyPhysics.m_pendingCollisionFilterRefreshes)
      {
        if (collisionFilterRefresh.IsInWorld)
        {
          if ((HkReferenceObject) collisionFilterRefresh.RigidBody != (HkReferenceObject) null)
            collisionFilterRefresh.HavokWorld.RefreshCollisionFilterOnEntity((HkEntity) collisionFilterRefresh.RigidBody);
          if ((HkReferenceObject) collisionFilterRefresh.RigidBody2 != (HkReferenceObject) null)
            collisionFilterRefresh.HavokWorld.RefreshCollisionFilterOnEntity((HkEntity) collisionFilterRefresh.RigidBody2);
        }
      }
      MyPhysics.m_pendingCollisionFilterRefreshes.Clear();
    }

    public static ListReader<object>? GetClusterList() => MyPhysics.Clusters == null ? new ListReader<object>?() : new ListReader<object>?(MyPhysics.Clusters.GetList());

    public static void GetAll(List<MyClusterTree.MyClusterQueryResult> results) => MyPhysics.Clusters.GetAll(results);

    private void EnsureClusterSpace()
    {
      if (MyFakes.FORCE_CLUSTER_REORDER)
      {
        MyPhysics.ForceClustersReorder();
        MyFakes.FORCE_CLUSTER_REORDER = false;
      }
      foreach (MyPhysicsBody iterationBody in this.m_iterationBodies)
      {
        Vector3 linearVelocity = iterationBody.LinearVelocity;
        if ((double) linearVelocity.LengthSquared() > 0.100000001490116)
        {
          BoundingBoxD aabb = MyClusterTree.AdjustAABBByVelocity(iterationBody.Entity.WorldAABB, linearVelocity);
          MyPhysics.Clusters.EnsureClusterSpace(aabb);
        }
      }
      foreach (HkCharacterRigidBody characterIterationBody in this.m_characterIterationBodies)
      {
        if ((double) characterIterationBody.LinearVelocity.LengthSquared() > 0.100000001490116)
        {
          BoundingBoxD aabb = MyClusterTree.AdjustAABBByVelocity(((MyPhysicsComponentBase) characterIterationBody.GetHitRigidBody().UserObject).Entity.PositionComp.WorldAABB, characterIterationBody.LinearVelocity);
          MyPhysics.Clusters.EnsureClusterSpace(aabb);
        }
      }
    }

    public static void SerializeClusters(List<BoundingBoxD> list) => MyPhysics.Clusters.Serialize(list);

    public static void DeserializeClusters(List<BoundingBoxD> list) => MyPhysics.Clusters.Deserialize(list);

    private void Tree_OnClustersReordered()
    {
      MySandboxGame.Log.WriteLine("Clusters reordered");
      MyPhysics.ClustersNeedSync = true;
    }

    [Event(null, 1979)]
    [Reliable]
    [Broadcast]
    private static void OnClustersReordered(List<BoundingBoxD> list) => MyPhysics.DeserializeClusters(list);

    internal static void ForceClustersReorder() => MyPhysics.Clusters.ReorderClusters(BoundingBoxD.CreateInvalid());

    public bool GetEntityReplicableExistsById(long entityId)
    {
      MyEntity entityByIdOrDefault = MyEntities.GetEntityByIdOrDefault(entityId);
      return entityByIdOrDefault != null && MyExternalReplicable.FindByObject((object) entityByIdOrDefault) != null;
    }

    public static void ProfileHkCall(Action action)
    {
      HkVDB.SyncTimers(MyPhysics.m_threadPool);
      action();
      HkTaskProfiler.ReplayTimers((HkTaskProfiler.BlockBeginFunc) (x => {}), (HkTaskProfiler.BlockEndFunc) (x => {}));
    }

    [Event(null, 2040)]
    [Reliable]
    [Server]
    private static void UpdateServerDebugCamera(MatrixD wm)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value))
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      else
        MyPhysics.ClientCameraWM = new MatrixD?(wm);
    }

    [Event(null, 2052)]
    [Reliable]
    [Server]
    public static void ControlVDBRecording(string fileName)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value))
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      else
        MyPhysics.VDBRecordFile = fileName == null ? (string) null : Path.Combine(MyFileSystem.UserDataPath, fileName.Replace('\\', '_').Replace('/', '_').Replace(':', '_'));
    }

    public void InformReplicationStarted(MyEntity entity)
    {
      if (this.m_worldObserver == null || entity == null)
        return;
      this.m_worldObserver.AddReplicatedEntity(entity);
    }

    public void InformReplicationEnded(MyEntity entity)
    {
      if (this.m_worldObserver == null || entity == null)
        return;
      this.m_worldObserver.RemoveReplicatedEntity(entity);
    }

    public static void CastRay(
      Vector3D from,
      Vector3D to,
      List<MyPhysics.HitInfo> toList,
      int raycastFilterLayer = 0)
    {
      MyPhysics.CastRayInternal(ref from, ref to, toList, raycastFilterLayer);
    }

    public static MyPhysics.HitInfo? CastRay(
      Vector3D from,
      Vector3D to,
      int raycastFilterLayer = 0)
    {
      return MyPhysics.CastRayInternal(ref from, ref to, raycastFilterLayer);
    }

    public static List<MyPhysics.HitInfo> CastRay_AllHits(Vector3D from, Vector3D to)
    {
      List<MyPhysics.HitInfo> toList = new List<MyPhysics.HitInfo>();
      MyPhysics.CastRayInternal(ref from, ref to, toList);
      return toList;
    }

    public static void GetPenetrationsBox(
      ref Vector3 halfExtents,
      ref Vector3D translation,
      ref Quaternion rotation,
      List<HkBodyCollision> results,
      int filter)
    {
      MyPhysics.GetPenetrationsBoxInternal(ref halfExtents, ref translation, ref rotation, results, filter);
    }

    private static void CastRayInternal(
      ref Vector3D from,
      ref Vector3D to,
      List<MyPhysics.HitInfo> toList,
      int raycastFilterLayer = 0)
    {
      toList.Clear();
      using (MyUtils.ReuseCollection<HkHitInfo>(ref MyPhysics.m_resultHits))
      {
        using (MyUtils.ReuseCollection<MyClusterTree.MyClusterQueryResult>(ref MyPhysics.m_resultWorlds))
        {
          MyPhysics.Clusters.CastRay(from, to, MyPhysics.m_resultWorlds);
          foreach (MyClusterTree.MyClusterQueryResult resultWorld in MyPhysics.m_resultWorlds)
          {
            Vector3D vector3D1 = from - resultWorld.AABB.Center;
            Vector3D vector3D2 = to;
            BoundingBoxD aabb = resultWorld.AABB;
            Vector3D center1 = aabb.Center;
            Vector3D vector3D3 = vector3D2 - center1;
            ((HkWorld) resultWorld.UserData)?.CastRay((Vector3) vector3D1, (Vector3) vector3D3, MyPhysics.m_resultHits, raycastFilterLayer);
            foreach (HkHitInfo resultHit in MyPhysics.m_resultHits)
            {
              List<MyPhysics.HitInfo> hitInfoList = toList;
              MyPhysics.HitInfo hitInfo1 = new MyPhysics.HitInfo();
              hitInfo1.HkHitInfo = resultHit;
              ref MyPhysics.HitInfo local = ref hitInfo1;
              Vector3 position = resultHit.Position;
              aabb = resultWorld.AABB;
              Vector3D center2 = aabb.Center;
              Vector3D vector3D4 = position + center2;
              local.Position = vector3D4;
              MyPhysics.HitInfo hitInfo2 = hitInfo1;
              hitInfoList.Add(hitInfo2);
            }
          }
        }
      }
    }

    private static MyPhysics.HitInfo? CastRayInternal(
      ref Vector3D from,
      ref Vector3D to,
      int raycastFilterLayer)
    {
      using (MyUtils.ReuseCollection<MyClusterTree.MyClusterQueryResult>(ref MyPhysics.m_resultWorlds))
      {
        MyPhysics.Clusters.CastRay(from, to, MyPhysics.m_resultWorlds);
        return MyPhysics.CastRayInternal(from, to, MyPhysics.m_resultWorlds, raycastFilterLayer);
      }
    }

    public static bool CastRay(
      Vector3D from,
      Vector3D to,
      out MyPhysics.HitInfo hitInfo,
      uint raycastCollisionFilter,
      bool ignoreConvexShape)
    {
      using (MyUtils.ReuseCollection<MyClusterTree.MyClusterQueryResult>(ref MyPhysics.m_resultWorlds))
      {
        MyPhysics.Clusters.CastRay(from, to, MyPhysics.m_resultWorlds);
        hitInfo = new MyPhysics.HitInfo();
        foreach (MyClusterTree.MyClusterQueryResult resultWorld in MyPhysics.m_resultWorlds)
        {
          Vector3D vector3D1 = from;
          BoundingBoxD aabb = resultWorld.AABB;
          Vector3D center1 = aabb.Center;
          Vector3 from1 = (Vector3) (vector3D1 - center1);
          Vector3D vector3D2 = to;
          aabb = resultWorld.AABB;
          Vector3D center2 = aabb.Center;
          Vector3 to1 = (Vector3) (vector3D2 - center2);
          HkHitInfo info = new HkHitInfo();
          bool flag = ((HkWorld) resultWorld.UserData).CastRay(from1, to1, out info, raycastCollisionFilter, ignoreConvexShape);
          if (flag)
          {
            ref MyPhysics.HitInfo local = ref hitInfo;
            Vector3D position = (Vector3D) info.Position;
            aabb = resultWorld.AABB;
            Vector3D center3 = aabb.Center;
            Vector3D vector3D3 = position + center3;
            local.Position = vector3D3;
            hitInfo.HkHitInfo = info;
            return flag;
          }
        }
      }
      return false;
    }

    private static MyPhysics.HitInfo? CastRayInternal(
      Vector3D from,
      Vector3D to,
      List<MyClusterTree.MyClusterQueryResult> worlds,
      int raycastFilterLayer = 0)
    {
      float maxValue = float.MaxValue;
      foreach (MyClusterTree.MyClusterQueryResult world in worlds)
      {
        Vector3 from1 = (Vector3) (from - world.AABB.Center);
        Vector3 to1 = (Vector3) (to - world.AABB.Center);
        HkHitInfo? nullable1 = ((HkWorld) world.UserData).CastRay(from1, to1, raycastFilterLayer);
        if (nullable1.HasValue && (double) nullable1.Value.HitFraction < (double) maxValue)
        {
          Vector3D worldPosition = (Vector3D) nullable1.Value.Position + world.AABB.Center;
          MyPhysics.HitInfo? nullable2 = new MyPhysics.HitInfo?(new MyPhysics.HitInfo(nullable1.Value, worldPosition));
          float hitFraction = nullable1.Value.HitFraction;
          return nullable2;
        }
      }
      return new MyPhysics.HitInfo?();
    }

    private static void GetPenetrationsBoxInternal(
      ref Vector3 halfExtents,
      ref Vector3D translation,
      ref Quaternion rotation,
      List<HkBodyCollision> results,
      int filter)
    {
      using (MyUtils.ReuseCollection<MyClusterTree.MyClusterQueryResult>(ref MyPhysics.m_resultWorlds))
      {
        MyPhysics.Clusters.Intersects(translation, MyPhysics.m_resultWorlds);
        foreach (MyClusterTree.MyClusterQueryResult resultWorld in MyPhysics.m_resultWorlds)
        {
          Vector3 translation1 = (Vector3) (translation - resultWorld.AABB.Center);
          ((HkWorld) resultWorld.UserData).GetPenetrationsBox(ref halfExtents, ref translation1, ref rotation, results, filter);
        }
      }
    }

    public static ClearToken<HkShapeCollision> GetPenetrationsShapeShape(
      HkShape shape1,
      ref Vector3 translation1,
      ref Quaternion rotation1,
      HkShape shape2,
      ref Vector3 translation2,
      ref Quaternion rotation2)
    {
      MyUtils.Init<List<HkShapeCollision>>(ref MyPhysics.m_shapeCollisionResultsCache).AssertEmpty<HkShapeCollision>();
      ((HkWorld) MyPhysics.Clusters.GetList()[0]).GetPenetrationsShapeShape(shape1, ref translation1, ref rotation1, shape2, ref translation2, ref rotation2, MyPhysics.m_shapeCollisionResultsCache);
      return MyPhysics.m_shapeCollisionResultsCache.GetClearToken<HkShapeCollision>();
    }

    private void ExecuteParallelRayCasts()
    {
      using (MyEntities.StartAsyncUpdateBlock())
      {
        using (HkAccessControl.PushState(HkAccessControl.AccessState.SharedRead))
        {
          MyPhysics.ParallelRayCastQuery parallelRayCastQuery = Interlocked.Exchange<MyPhysics.ParallelRayCastQuery>(ref MyPhysics.m_pendingRayCasts, (MyPhysics.ParallelRayCastQuery) null);
          if (parallelRayCastQuery == null)
            return;
          MyPhysics.DeliverData deliverData = MyPhysics.m_pendingRayCastsParallelPool.Get();
          for (; parallelRayCastQuery != null; parallelRayCastQuery = parallelRayCastQuery.Next)
            deliverData.Jobs.Add(parallelRayCastQuery);
          Parallel.For(0, deliverData.Jobs.Count, deliverData.ExecuteJob, 1, WorkPriority.VeryHigh, new WorkOptions?(Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Physics, "Raycast")), true);
          Parallel.Start((IWork) deliverData);
        }
      }
    }

    private void CancelParallelRayCasts()
    {
      MyPhysics.ParallelRayCastQuery next;
      for (MyPhysics.ParallelRayCastQuery parallelRayCastQuery = Interlocked.Exchange<MyPhysics.ParallelRayCastQuery>(ref MyPhysics.m_pendingRayCasts, (MyPhysics.ParallelRayCastQuery) null); parallelRayCastQuery != null; parallelRayCastQuery = next)
      {
        next = parallelRayCastQuery.Next;
        parallelRayCastQuery.Cancel();
      }
    }

    public static void CastRayParallel(
      ref Vector3D from,
      ref Vector3D to,
      int raycastFilterLayer,
      Action<MyPhysics.HitInfo?> callback)
    {
      MyPhysics.ParallelRayCastQuery query = MyPhysics.ParallelRayCastQuery.Allocate();
      query.Kind = MyPhysics.ParallelRayCastQuery.TKind.RaycastSingle;
      query.RayCastData = new MyPhysics.RayCastData()
      {
        To = to,
        From = from,
        Callback = (object) callback,
        RayCastFilterLayer = raycastFilterLayer
      };
      MyPhysics.EnqueueParallelQuery(query);
    }

    public static void CastRayParallel(
      ref Vector3D from,
      ref Vector3D to,
      List<MyPhysics.HitInfo> collector,
      int raycastFilterLayer,
      Action<List<MyPhysics.HitInfo>> callback)
    {
      MyPhysics.ParallelRayCastQuery query = MyPhysics.ParallelRayCastQuery.Allocate();
      query.Kind = MyPhysics.ParallelRayCastQuery.TKind.RaycastAll;
      query.RayCastData = new MyPhysics.RayCastData()
      {
        To = to,
        From = from,
        Callback = (object) callback,
        Collector = collector,
        RayCastFilterLayer = raycastFilterLayer
      };
      MyPhysics.EnqueueParallelQuery(query);
    }

    public static void GetPenetrationsBoxParallel(
      ref Vector3 halfExtents,
      ref Vector3D translation,
      ref Quaternion rotation,
      List<HkBodyCollision> results,
      int filter,
      Action<List<HkBodyCollision>> callback)
    {
      MyPhysics.ParallelRayCastQuery query = MyPhysics.ParallelRayCastQuery.Allocate();
      query.Kind = MyPhysics.ParallelRayCastQuery.TKind.GetPenetrationsBox;
      query.QueryBoxData = new MyPhysics.QueryBoxData()
      {
        Filter = filter,
        Results = results,
        Rotation = rotation,
        Callback = callback,
        Translation = translation,
        HalfExtents = halfExtents
      };
      MyPhysics.EnqueueParallelQuery(query);
    }

    private static void EnqueueParallelQuery(MyPhysics.ParallelRayCastQuery query)
    {
      MyPhysics.ParallelRayCastQuery comparand = MyPhysics.m_pendingRayCasts;
      while (true)
      {
        query.Next = comparand;
        MyPhysics.ParallelRayCastQuery parallelRayCastQuery = Interlocked.CompareExchange<MyPhysics.ParallelRayCastQuery>(ref MyPhysics.m_pendingRayCasts, query, comparand);
        if (parallelRayCastQuery != comparand)
          comparand = parallelRayCastQuery;
        else
          break;
      }
    }

    private void StepWorlds()
    {
      if (!MyFakes.ENABLE_HAVOK_STEP_OPTIMIZERS || Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.Settings.EnableSelectivePhysicsUpdates)
      {
        this.StepWorldsInternal((List<MyTuple<HkWorld, MyTimeSpan>>) null);
      }
      else
      {
        if (this.m_optimizeNextFrame)
        {
          this.m_optimizeNextFrame = false;
          this.StepWorldsInternal(this.m_timings);
          this.EnableOptimizations(this.m_timings);
          this.m_timings.Clear();
        }
        else
        {
          Stopwatch stopwatch = Stopwatch.StartNew();
          this.StepWorldsInternal((List<MyTuple<HkWorld, MyTimeSpan>>) null);
          double totalMilliseconds = stopwatch.Elapsed.TotalMilliseconds;
          bool flag1 = totalMilliseconds > (double) this.m_averageFrameTime * 3.0 && totalMilliseconds > 10.0 || totalMilliseconds >= 30.0;
          bool flag2 = totalMilliseconds < (double) this.m_averageFrameTime * 1.5 && !flag1;
          int num1 = flag1 ? 1000 : 100;
          this.m_averageFrameTime = (float) ((double) this.m_averageFrameTime * ((double) (num1 - 1) / (double) num1) + totalMilliseconds * (1.0 / (double) num1));
          int num2 = MyFakes.ENABLE_HAVOK_STEP_OPTIMIZERS_TIMINGS ? 1 : 0;
          if (this.m_optimizationsEnabled)
          {
            if (flag2)
            {
              ++this.m_fastFrames;
              if (this.m_fastFrames > 10)
                this.DisableOptimizations();
            }
            else
              this.m_fastFrames = 0;
          }
          else if (flag1)
          {
            ++this.m_slowFrames;
            if (this.m_slowFrames > 10)
            {
              this.m_slowFrames = 0;
              this.m_optimizeNextFrame = true;
            }
          }
          else if (flag2 && this.m_slowFrames > 0)
            --this.m_slowFrames;
        }
        if (!MyDebugDrawSettings.DEBUG_DRAW_TOI_OPTIMIZED_GRIDS)
          return;
        DisableGridTOIsOptimizer.Static.DebugDraw();
      }
    }

    private void EnableOptimizations(List<MyTuple<HkWorld, MyTimeSpan>> timings)
    {
      this.m_optimizationsEnabled = true;
      MyLog.Default.WriteLine("Optimizing physics step of " + (object) timings.Count + " worlds");
      foreach (IPhysicsStepOptimizer optimizer in this.m_optimizers)
        optimizer.EnableOptimizations(timings);
    }

    private void DisableOptimizations()
    {
      this.m_optimizationsEnabled = false;
      foreach (IPhysicsStepOptimizer optimizer in this.m_optimizers)
        optimizer.DisableOptimizations();
    }

    private void InitStepOptimizer()
    {
      this.m_optimizeNextFrame = false;
      this.m_optimizationsEnabled = false;
      this.m_averageFrameTime = 16.66667f;
      this.m_optimizers = new List<IPhysicsStepOptimizer>()
      {
        (IPhysicsStepOptimizer) new DisableGridTOIsOptimizer()
      };
    }

    private void UnloadStepOptimizer()
    {
      foreach (IPhysicsStepOptimizer optimizer in this.m_optimizers)
        optimizer.Unload();
      this.m_optimizers.Clear();
    }

    private void StepWorldsInternal(List<MyTuple<HkWorld, MyTimeSpan>> timings)
    {
      if (timings != null)
        this.StepWorldsMeasured(timings);
      else if (MyFakes.ENABLE_HAVOK_PARALLEL_SCHEDULING)
        this.StepWorldsParallel();
      else
        this.StepWorldsSequential();
      if (HkBaseSystem.IsOutOfMemory)
        throw new OutOfMemoryException("Havok run out of memory");
    }

    private void StepWorldsSequential()
    {
      foreach (HkWorld world in MyPhysics.Clusters.GetList())
        this.StepSingleWorld(world);
    }

    private void StepWorldsMeasured(List<MyTuple<HkWorld, MyTimeSpan>> timings)
    {
      foreach (HkWorld world in MyPhysics.Clusters.GetList())
      {
        Stopwatch stopwatch = Stopwatch.StartNew();
        this.StepSingleWorld(world);
        MyTimeSpan myTimeSpan = MyTimeSpan.FromTicks(stopwatch.ElapsedTicks);
        timings.Add(MyTuple.Create<HkWorld, MyTimeSpan>(world, myTimeSpan));
      }
    }

    private void StepWorldsParallel()
    {
      ListReader<MyClusterTree.MyCluster> clusters = MyPhysics.Clusters.GetClusters();
      using (MyEntities.StartAsyncUpdateBlock())
      {
        using (HkAccessControl.PushState(HkAccessControl.AccessState.SharedRead))
        {
          MyPhysics.m_jobQueue.WaitPolicy = HkJobQueue.WaitPolicyT.WAIT_INDEFINITELY;
          MyPhysics.m_threadPool.ExecuteJobQueue(MyPhysics.m_jobQueue);
          foreach (MyClusterTree.MyCluster myCluster in clusters)
          {
            if (myCluster.UserData is HkWorld userData && this.IsClusterActive(myCluster.ClusterId, userData.CharacterRigidBodies.Count))
            {
              userData.ExecutePendingCriticalOperations();
              userData.UnmarkForWrite();
              userData.InitMtStep(MyPhysics.m_jobQueue, 0.01666667f * MyFakes.SIMULATION_SPEED);
            }
          }
          MyPhysics.m_jobQueue.WaitPolicy = HkJobQueue.WaitPolicyT.WAIT_UNTIL_ALL_WORK_COMPLETE;
          MyPhysics.m_jobQueue.ProcessAllJobs();
          MyPhysics.m_threadPool.WaitForCompletion();
          foreach (MyClusterTree.MyCluster myCluster in clusters)
          {
            if (myCluster.UserData is HkWorld userData && this.IsClusterActive(myCluster.ClusterId, userData.CharacterRigidBodies.Count))
            {
              userData.FinishMtStep(MyPhysics.m_jobQueue, MyPhysics.m_threadPool);
              userData.MarkForWrite();
            }
          }
        }
      }
    }

    private bool IsClusterActive(int clusterId, int rigidBodiesCount) => this.m_worldObserver == null || rigidBodiesCount != 0 || this.m_worldObserver.IsClusterActive(clusterId);

    private void StepSingleWorld(HkWorld world)
    {
      int num1 = MyFakes.DEFORMATION_LOGGING ? 1 : 0;
      world.ExecutePendingCriticalOperations();
      world.UnmarkForWrite();
      MyEntities.AsyncUpdateToken? nullable1;
      HkAccessControl.AccessStateToken? nullable2;
      if (MyFakes.ENABLE_HAVOK_MULTITHREADING)
      {
        nullable1 = new MyEntities.AsyncUpdateToken?(MyEntities.StartAsyncUpdateBlock());
        nullable2 = new HkAccessControl.AccessStateToken?(HkAccessControl.PushState(HkAccessControl.AccessState.SharedRead));
      }
      else
      {
        nullable1 = new MyEntities.AsyncUpdateToken?();
        nullable2 = new HkAccessControl.AccessStateToken?();
      }
      if (MyFakes.TWO_STEP_SIMULATIONS)
      {
        world.StepSimulation(0.008333334f * MyFakes.SIMULATION_SPEED, MyFakes.ENABLE_HAVOK_MULTITHREADING);
        world.StepSimulation(0.008333334f * MyFakes.SIMULATION_SPEED, MyFakes.ENABLE_HAVOK_MULTITHREADING);
      }
      else
        world.StepSimulation(0.01666667f * MyFakes.SIMULATION_SPEED, MyFakes.ENABLE_HAVOK_MULTITHREADING);
      world.MarkForWrite();
      nullable1?.Dispose();
      nullable2?.Dispose();
      int num2 = MyFakes.DEFORMATION_LOGGING ? 1 : 0;
    }

    public static void CommitSchedulingSettingToServer() => MyMultiplayer.RaiseStaticEvent<bool, bool>((Func<IMyEventOwner, Action<bool, bool>>) (x => new Action<bool, bool>(MyPhysics.SetScheduling)), MyFakes.ENABLE_HAVOK_MULTITHREADING, MyFakes.ENABLE_HAVOK_PARALLEL_SCHEDULING);

    [Event(null, 207)]
    [Reliable]
    [Server]
    public static void SetScheduling(bool multithread, bool parallel)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value))
      {
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        MyFakes.ENABLE_HAVOK_MULTITHREADING = multithread;
        MyFakes.ENABLE_HAVOK_PARALLEL_SCHEDULING = parallel;
      }
    }

    public struct HitInfo : IHitInfo
    {
      public HkHitInfo HkHitInfo;
      public Vector3D Position;

      public HitInfo(HkHitInfo hi, Vector3D worldPosition)
      {
        this.HkHitInfo = hi;
        this.Position = worldPosition;
      }

      Vector3D IHitInfo.Position => this.Position;

      IMyEntity IHitInfo.HitEntity => this.HkHitInfo.GetHitEntity();

      Vector3 IHitInfo.Normal => this.HkHitInfo.Normal;

      float IHitInfo.Fraction => this.HkHitInfo.HitFraction;

      public override string ToString()
      {
        IMyEntity hitEntity = this.HkHitInfo.GetHitEntity();
        return hitEntity != null ? hitEntity.ToString() : base.ToString();
      }
    }

    public struct MyContactPointEvent
    {
      public HkContactPointEvent ContactPointEvent;
      public Vector3D Position;

      public Vector3 Normal => this.ContactPointEvent.ContactPoint.Normal;
    }

    public struct FractureImpactDetails
    {
      public HkdFractureImpactDetails Details;
      public HkWorld World;
      public Vector3D ContactInWorld;
      public MyEntity Entity;
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct CollisionLayers
    {
      public const int BlockPlacementTestCollisionLayer = 7;
      public const int MissileLayer = 8;
      public const int NoVoxelCollisionLayer = 9;
      public const int LightFloatingObjectCollisionLayer = 10;
      public const int VoxelLod1CollisionLayer = 11;
      public const int NotCollideWithStaticLayer = 12;
      public const int StaticCollisionLayer = 13;
      public const int CollideWithStaticLayer = 14;
      public const int DefaultCollisionLayer = 15;
      public const int DynamicDoubledCollisionLayer = 16;
      public const int KinematicDoubledCollisionLayer = 17;
      public const int CharacterCollisionLayer = 18;
      public const int NoCollisionLayer = 19;
      public const int DebrisCollisionLayer = 20;
      public const int GravityPhantomLayer = 21;
      public const int CharacterNetworkCollisionLayer = 22;
      public const int FloatingObjectCollisionLayer = 23;
      public const int ObjectDetectionCollisionLayer = 24;
      public const int VirtualMassLayer = 25;
      public const int CollectorCollisionLayer = 26;
      public const int AmmoLayer = 27;
      public const int VoxelCollisionLayer = 28;
      public const int ExplosionRaycastLayer = 29;
      public const int CollisionLayerWithoutCharacter = 30;
      public const int RagdollCollisionLayer = 31;
      public const int TargetDummyLayer = 6;
    }

    public struct ForceInfo
    {
      public readonly bool ActiveOnly;
      public readonly float? MaxSpeed;
      public readonly Vector3? Force;
      public readonly Vector3? Torque;
      public readonly Vector3D? Position;
      public readonly MyPhysicsBody Body;
      public readonly MyPhysicsForceType Type;

      public ForceInfo(
        MyPhysicsBody body,
        bool activeOnly,
        float? maxSpeed,
        Vector3? force,
        Vector3? torque,
        Vector3D? position,
        MyPhysicsForceType type)
      {
        this.Body = body;
        this.Type = type;
        this.Force = force;
        this.Torque = torque;
        this.Position = position;
        this.MaxSpeed = maxSpeed;
        this.ActiveOnly = activeOnly;
      }
    }

    private struct RayCastData
    {
      public object Callback;
      public Vector3D To;
      public Vector3D From;
      public MyPhysics.HitInfo? HitInfo;
      public int RayCastFilterLayer;
      public List<MyPhysics.HitInfo> Collector;
    }

    private struct QueryBoxData
    {
      public int Filter;
      public Quaternion Rotation;
      public Vector3 HalfExtents;
      public Vector3D Translation;
      public List<HkBodyCollision> Results;
      public Action<List<HkBodyCollision>> Callback;
    }

    private class ParallelRayCastQuery
    {
      private static MyConcurrentPool<MyPhysics.ParallelRayCastQuery> m_pool = new MyConcurrentPool<MyPhysics.ParallelRayCastQuery>(expectedAllocations: 100000);
      public MyPhysics.ParallelRayCastQuery Next;
      public MyPhysics.ParallelRayCastQuery.TKind Kind;
      public MyPhysics.RayCastData RayCastData;
      public MyPhysics.QueryBoxData QueryBoxData;

      public void ExecuteRayCast()
      {
        switch (this.Kind)
        {
          case MyPhysics.ParallelRayCastQuery.TKind.RaycastSingle:
            this.RayCastData.HitInfo = MyPhysics.CastRayInternal(ref this.RayCastData.From, ref this.RayCastData.To, this.RayCastData.RayCastFilterLayer);
            break;
          case MyPhysics.ParallelRayCastQuery.TKind.RaycastAll:
            MyPhysics.CastRayInternal(ref this.RayCastData.From, ref this.RayCastData.To, this.RayCastData.Collector, this.RayCastData.RayCastFilterLayer);
            break;
          case MyPhysics.ParallelRayCastQuery.TKind.GetPenetrationsBox:
            MyPhysics.GetPenetrationsBox(ref this.QueryBoxData.HalfExtents, ref this.QueryBoxData.Translation, ref this.QueryBoxData.Rotation, this.QueryBoxData.Results, this.QueryBoxData.Filter);
            break;
        }
      }

      public void DeliverResults()
      {
        try
        {
          switch (this.Kind)
          {
            case MyPhysics.ParallelRayCastQuery.TKind.RaycastSingle:
              ((Action<MyPhysics.HitInfo?>) this.RayCastData.Callback).InvokeIfNotNull<MyPhysics.HitInfo?>(this.RayCastData.HitInfo);
              break;
            case MyPhysics.ParallelRayCastQuery.TKind.RaycastAll:
              ((Action<List<MyPhysics.HitInfo>>) this.RayCastData.Callback).InvokeIfNotNull<List<MyPhysics.HitInfo>>(this.RayCastData.Collector);
              break;
            case MyPhysics.ParallelRayCastQuery.TKind.GetPenetrationsBox:
              this.QueryBoxData.Callback.InvokeIfNotNull<List<HkBodyCollision>>(this.QueryBoxData.Results);
              break;
          }
        }
        finally
        {
          this.Return();
        }
      }

      public static MyPhysics.ParallelRayCastQuery Allocate() => MyPhysics.ParallelRayCastQuery.m_pool.Get();

      public void Return()
      {
        this.Next = (MyPhysics.ParallelRayCastQuery) null;
        this.RayCastData = new MyPhysics.RayCastData();
        this.QueryBoxData = new MyPhysics.QueryBoxData();
        MyPhysics.ParallelRayCastQuery.m_pool.Return(this);
      }

      public void Cancel() => this.Return();

      public enum TKind
      {
        RaycastSingle,
        RaycastAll,
        GetPenetrationsBox,
      }
    }

    private class DeliverData : AbstractWork
    {
      public readonly Action<int> ExecuteJob;
      public readonly List<MyPhysics.ParallelRayCastQuery> Jobs = new List<MyPhysics.ParallelRayCastQuery>();

      public DeliverData()
      {
        this.ExecuteJob = new Action<int>(this.ExecuteJobImpl);
        this.Options = Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.HK_JOB_TYPE_RAYCAST_QUERY, "RayCastResults");
      }

      private void ExecuteJobImpl(int i) => this.Jobs[i].ExecuteRayCast();

      public override void DoWork(WorkData unused) => this.DeliverResults();

      private void DeliverResults()
      {
        foreach (MyPhysics.ParallelRayCastQuery job in this.Jobs)
          job.DeliverResults();
        this.Jobs.Clear();
        MyPhysics.m_pendingRayCastsParallelPool.Return(this);
      }

      private class Sandbox_Engine_Physics_MyPhysics\u003C\u003EDeliverData\u003C\u003EActor : IActivator, IActivator<MyPhysics.DeliverData>
      {
        object IActivator.CreateInstance() => (object) new MyPhysics.DeliverData();

        MyPhysics.DeliverData IActivator<MyPhysics.DeliverData>.CreateInstance() => new MyPhysics.DeliverData();
      }
    }

    protected sealed class OnClustersReordered\u003C\u003ESystem_Collections_Generic_List`1\u003CVRageMath_BoundingBoxD\u003E : ICallSite<IMyEventOwner, List<BoundingBoxD>, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in List<BoundingBoxD> list,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyPhysics.OnClustersReordered(list);
      }
    }

    protected sealed class UpdateServerDebugCamera\u003C\u003EVRageMath_MatrixD : ICallSite<IMyEventOwner, MatrixD, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MatrixD wm,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyPhysics.UpdateServerDebugCamera(wm);
      }
    }

    protected sealed class ControlVDBRecording\u003C\u003ESystem_String : ICallSite<IMyEventOwner, string, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string fileName,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyPhysics.ControlVDBRecording(fileName);
      }
    }

    protected sealed class SetScheduling\u003C\u003ESystem_Boolean\u0023System_Boolean : ICallSite<IMyEventOwner, bool, bool, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in bool multithread,
        in bool parallel,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyPhysics.SetScheduling(multithread, parallel);
      }
    }
  }
}
