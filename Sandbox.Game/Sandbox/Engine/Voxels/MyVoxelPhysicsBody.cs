// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyVoxelPhysicsBody
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using ParallelTasks;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GUI.DebugInputComponents;
using System;
using System.Collections.Generic;
using System.Threading;
using VRage.Definitions.Components;
using VRage.Entities.Components;
using VRage.Factory;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Voxels;
using VRage.Library.Collections;
using VRage.ModAPI;
using VRage.ObjectBuilders.Definitions.Components;
using VRage.Profiler;
using VRage.Utils;
using VRage.Voxels;
using VRage.Voxels.DualContouring;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Voxels
{
  [MyDependency(typeof (MyVoxelMesherComponent), Critical = true)]
  public class MyVoxelPhysicsBody : MyPhysicsBody
  {
    public static int ActiveVoxelPhysicsBodies = 0;
    public static int ActiveVoxelPhysicsBodiesWithExtendedCache = 0;
    public static bool EnableShapeDiscard = true;
    private const int ShapeDiscardThreshold = 0;
    private const int ShapeDiscardCheckInterval = 18;
    private static Vector3I[] m_cellsToGenerateBuffer = new Vector3I[128];
    internal readonly HashSet<Vector3I>[] InvalidCells;
    internal readonly MyPrecalcJobPhysicsBatch[] RunningBatchTask = new MyPrecalcJobPhysicsBatch[2];
    public readonly MyVoxelBase m_voxelMap;
    private bool m_needsShapeUpdate;
    private bool m_bodiesInitialized;
    private readonly List<MyEntity> m_nearbyEntities = new List<MyEntity>();
    private int m_nearbyEntitiesUpdateCounter;
    private MyVoxelMesherComponent m_mesher;
    private readonly MyWorkTracker<MyCellCoord, MyPrecalcJobPhysicsPrefetch> m_workTracker = new MyWorkTracker<MyCellCoord, MyPrecalcJobPhysicsPrefetch>((IEqualityComparer<MyCellCoord>) MyCellCoord.Comparer);
    private readonly Vector3I m_cellsOffset = new Vector3I(0, 0, 0);
    private bool m_staticForCluster = true;
    private float m_phantomExtend;
    private float m_predictionSize = 3f;
    private int m_lastDiscardCheck;
    private BoundingBoxI m_queuedRange = new BoundingBoxI(-1, -1);
    private bool m_queueInvalidation;
    private int m_hasExtendedCache;
    private int m_voxelQueriesInLastFrames;
    private const int EXTENDED_CACHE_QUERY_THRESHOLD = 20000;
    private const int EXTENDED_CACHE_QUERIES_THRESHOLD = 10000;
    private static List<MyCellCoord> m_toBeCancelledCache;
    [ThreadStatic]
    private static MyList<int> m_indexListCached;
    [ThreadStatic]
    private static MyList<byte> m_materialListCached;
    [ThreadStatic]
    private static MyList<Vector3> m_vertexListCached;
    public static bool UseLod1VoxelPhysics = false;

    internal MyVoxelPhysicsBody(
      MyVoxelBase voxelMap,
      float phantomExtend,
      float predictionSize = 3f,
      bool lazyPhysics = false)
      : base((IMyEntity) voxelMap, RigidBodyFlag.RBF_STATIC)
    {
      this.InvalidCells = new HashSet<Vector3I>[2];
      this.InvalidCells[0] = new HashSet<Vector3I>();
      this.InvalidCells[1] = new HashSet<Vector3I>();
      this.m_predictionSize = predictionSize;
      this.m_phantomExtend = phantomExtend;
      this.m_voxelMap = voxelMap;
      Vector3I vector3I = this.m_voxelMap.Size >> 3;
      this.m_cellsOffset = this.m_voxelMap.StorageMin >> 3;
      if (!MyFakes.ENABLE_LAZY_VOXEL_PHYSICS || !lazyPhysics)
        this.CreateRigidBodies();
      this.MaterialType = VRage.Game.MyMaterialType.ROCK;
      this.m_nearbyEntitiesUpdateCounter = MyUtils.GetRandomInt(10);
    }

    public override void OnAddedToScene()
    {
      base.OnAddedToScene();
      if (this.m_mesher == null)
      {
        this.m_mesher = new MyVoxelMesherComponent();
        if (this.m_voxelMap.RootVoxel is MyPlanet rootVoxel)
        {
          MyObjectBuilder_VoxelMesherComponentDefinition mesherPostprocessing = rootVoxel.Generator.MesherPostprocessing;
          if (mesherPostprocessing != null)
          {
            MyVoxelMesherComponentDefinition def = new MyVoxelMesherComponentDefinition();
            def.Init((MyObjectBuilder_DefinitionBase) mesherPostprocessing, MyModContext.BaseGame);
            this.m_mesher.Init(def);
          }
        }
        this.m_mesher.SetContainer((MyComponentContainer) this.Entity.Components);
        this.m_mesher.OnAddedToScene();
      }
      if (!this.m_bodiesInitialized)
        this.CreateRigidBodies();
      ++MyVoxelPhysicsBody.ActiveVoxelPhysicsBodies;
    }

    public override void OnRemovedFromScene()
    {
      base.OnRemovedFromScene();
      --MyVoxelPhysicsBody.ActiveVoxelPhysicsBodies;
      if (this.m_hasExtendedCache == 0)
        return;
      Interlocked.Decrement(ref MyVoxelPhysicsBody.ActiveVoxelPhysicsBodiesWithExtendedCache);
    }

    public override bool IsStatic => true;

    public bool QueueInvalidate
    {
      get => this.m_queueInvalidation;
      set
      {
        this.m_queueInvalidation = value;
        if (value || this.m_queuedRange.Max.X < 0)
          return;
        this.InvalidateRange(this.m_queuedRange.Min, this.m_queuedRange.Max);
        this.m_queuedRange = new BoundingBoxI(-1, -1);
      }
    }

    public HkRigidBody GetRigidBody(int lod) => MyVoxelPhysicsBody.UseLod1VoxelPhysics && lod == 1 ? this.RigidBody2 : this.RigidBody;

    public bool GetShape(int lod, out HkUniformGridShape gridShape)
    {
      HkRigidBody rigidBody = this.GetRigidBody(lod);
      if ((HkReferenceObject) rigidBody == (HkReferenceObject) null)
      {
        gridShape = new HkUniformGridShape();
        return false;
      }
      gridShape = (HkUniformGridShape) rigidBody.GetShape();
      return true;
    }

    public bool GetShape(int lod, Vector3D localPos, out HkBvCompressedMeshShape mesh)
    {
      HkUniformGridShape shape = (HkUniformGridShape) this.GetRigidBody(lod).GetShape();
      localPos += this.m_voxelMap.SizeInMetresHalf;
      Vector3I vector3I = new Vector3I(localPos / (8.0 * (double) (1 << lod)));
      return shape.GetChild(vector3I.X, vector3I.Y, vector3I.Z, out mesh);
    }

    private void CreateRigidBodies()
    {
      if (this.Entity.MarkedForClose || this.m_mesher == null)
        return;
      if (this.m_world == null)
        return;
      try
      {
        if (this.m_bodiesInitialized)
          return;
        Vector3I vector3I = this.m_voxelMap.Size >> 3;
        HkRigidBody hkRigidBody = (HkRigidBody) null;
        HkUniformGridShape uniformGridShape;
        HkUniformGridShapeArgs uniformGridShapeArgs;
        if (MyVoxelPhysicsBody.UseLod1VoxelPhysics)
        {
          ref HkUniformGridShape local = ref uniformGridShape;
          uniformGridShapeArgs = new HkUniformGridShapeArgs();
          uniformGridShapeArgs.CellsCount = vector3I >> 1;
          uniformGridShapeArgs.CellSize = 16f;
          uniformGridShapeArgs.CellOffset = 0.5f;
          uniformGridShapeArgs.CellExpand = 1f;
          HkUniformGridShapeArgs args = uniformGridShapeArgs;
          local = new HkUniformGridShape(args);
          uniformGridShape.SetShapeRequestHandler(new RequestShapeBlockingDelegate(this.RequestShapeBlockingLod1));
          this.CreateFromCollisionObject((HkShape) uniformGridShape, -this.m_voxelMap.SizeInMetresHalf, this.m_voxelMap.WorldMatrix, collisionFilter: 11);
          uniformGridShape.Base.RemoveReference();
          hkRigidBody = this.RigidBody;
          this.RigidBody = (HkRigidBody) null;
        }
        ref HkUniformGridShape local1 = ref uniformGridShape;
        uniformGridShapeArgs = new HkUniformGridShapeArgs();
        uniformGridShapeArgs.CellsCount = vector3I;
        uniformGridShapeArgs.CellSize = 8f;
        uniformGridShapeArgs.CellOffset = 0.5f;
        uniformGridShapeArgs.CellExpand = 1f;
        HkUniformGridShapeArgs args1 = uniformGridShapeArgs;
        local1 = new HkUniformGridShape(args1);
        uniformGridShape.SetShapeRequestHandler(new RequestShapeBlockingDelegate(this.RequestShapeBlocking));
        this.CreateFromCollisionObject((HkShape) uniformGridShape, -this.m_voxelMap.SizeInMetresHalf, this.m_voxelMap.WorldMatrix, collisionFilter: 28);
        uniformGridShape.Base.RemoveReference();
        this.RigidBody.IsEnvironment = true;
        if (MyVoxelPhysicsBody.UseLod1VoxelPhysics)
          this.RigidBody2 = hkRigidBody;
        if (MyFakes.ENABLE_PHYSICS_HIGH_FRICTION)
          this.Friction = 0.65f;
        this.m_bodiesInitialized = true;
        if (!this.Enabled)
          return;
        Matrix rigidBodyMatrix = this.GetRigidBodyMatrix();
        this.RigidBody.SetWorldMatrix(rigidBodyMatrix);
        this.m_world.AddRigidBody(this.RigidBody);
        if (!MyVoxelPhysicsBody.UseLod1VoxelPhysics)
          return;
        this.RigidBody2.SetWorldMatrix(rigidBodyMatrix);
        this.m_world.AddRigidBody(this.RigidBody2);
      }
      finally
      {
      }
    }

    private void MarkForShapeUpdate()
    {
      if (this.m_needsShapeUpdate)
        return;
      MyEntities.InvokeLater(new Action(this.UpdateRigidBodyShape));
      this.m_needsShapeUpdate = true;
    }

    private void UpdateRigidBodyShape()
    {
      if (!this.m_needsShapeUpdate || this.Entity == null)
        return;
      if (!this.m_bodiesInitialized)
        this.CreateRigidBodies();
      this.m_needsShapeUpdate = false;
      if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null)
      {
        int num1 = (int) this.RigidBody.UpdateShape();
      }
      if (!((HkReferenceObject) this.RigidBody2 != (HkReferenceObject) null))
        return;
      int num2 = (int) this.RigidBody2.UpdateShape();
    }

    private bool QueryEmptyOrFull(int minX, int minY, int minZ, int maxX, int maxY, int maxZ)
    {
      BoundingBoxI box = new BoundingBoxI(new Vector3I(minX, minY, minZ), new Vector3I(maxX, maxY, maxZ));
      if ((double) box.Volume() < 100.0)
        return false;
      bool flag = this.m_voxelMap.Storage.Intersect(ref box, 0) != ContainmentType.Intersects;
      BoundingBoxD boundingBoxD = new BoundingBoxD((Vector3D) (new Vector3((float) minX, (float) minY, (float) minZ) * 8f), (Vector3D) (new Vector3((float) maxX, (float) maxY, (float) maxZ) * 8f));
      boundingBoxD.TransformFast(this.Entity.WorldMatrix);
      MyOrientedBoundingBoxD orientedBoundingBoxD = new MyOrientedBoundingBoxD(boundingBoxD, this.Entity.WorldMatrix);
      MyRenderProxy.DebugDrawAABB(boundingBoxD, flag ? Color.Green : Color.Red, depthRead: false);
      return flag;
    }

    private void RequestShapeBlockingLod1(HkShapeBatch batch) => this.RequestShapeBatchBlockingInternal(batch, true);

    private void RequestShapeBlocking(HkShapeBatch batch) => this.RequestShapeBatchBlockingInternal(batch, false);

    private void RequestShapeBatchBlockingInternal(HkShapeBatch info, bool lod1physics)
    {
      if (this.m_voxelMap.MarkedForClose)
        return;
      if (!this.m_bodiesInitialized)
        this.CreateRigidBodies();
      this.MarkForShapeUpdate();
      int count = info.Count;
      if ((count > 20000 || Interlocked.Add(ref this.m_voxelQueriesInLastFrames, count) >= 10000) && Interlocked.Exchange(ref this.m_hasExtendedCache, 1) == 0)
      {
        Interlocked.Increment(ref MyVoxelPhysicsBody.ActiveVoxelPhysicsBodiesWithExtendedCache);
        HkUniformGridShape gridShape;
        if (this.GetShape(lod1physics ? 1 : 0, out gridShape))
          gridShape.EnableExtendedCache();
      }
      int lod = lod1physics ? 1 : 0;
      Parallel.For(0, count, (Action<int>) (i =>
      {
        Vector3I cell;
        info.GetInfo(i, out cell);
        MyCellCoord geometryCellCoord = new MyCellCoord(lod, cell);
        if (MyDebugDrawSettings.DEBUG_DRAW_REQUEST_SHAPE_BLOCKING)
        {
          BoundingBoxD worldAABB;
          MyVoxelCoordSystems.GeometryCellCoordToWorldAABB(this.m_voxelMap.PositionLeftBottomCorner, ref geometryCellCoord, out worldAABB);
          MyRenderProxy.DebugDrawAABB(worldAABB, lod1physics ? Color.Yellow : Color.Red, depthRead: false);
        }
        HkBvCompressedMeshShape shapeBlocking = this.CreateShapeBlocking(geometryCellCoord);
        info.SetResult(i, shapeBlocking);
      }), 1, WorkPriority.VeryHigh, new WorkOptions?(Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Voxels, "Physics.RequestVoxels")), true);
    }

    private HkBvCompressedMeshShape CreateShapeBlocking(MyCellCoord cellCoord)
    {
      bool flag = false;
      HkBvCompressedMeshShape compressedMeshShape = (HkBvCompressedMeshShape) HkShape.Empty;
      MyPrecalcJobPhysicsPrefetch jobPhysicsPrefetch = this.m_workTracker.Cancel(cellCoord);
      if (jobPhysicsPrefetch != null && jobPhysicsPrefetch.ResultComplete && Interlocked.Exchange(ref jobPhysicsPrefetch.Taken, 1) == 0)
      {
        flag = true;
        compressedMeshShape = jobPhysicsPrefetch.Result;
      }
      if (!flag)
      {
        VrVoxelMesh mesh = this.CreateMesh(cellCoord);
        compressedMeshShape = this.CreateShape(mesh, cellCoord.Lod);
        mesh?.Dispose();
      }
      return compressedMeshShape;
    }

    internal void InvalidateRange(Vector3I minVoxelChanged, Vector3I maxVoxelChanged)
    {
      this.InvalidateRange(minVoxelChanged, maxVoxelChanged, 0);
      if (!MyVoxelPhysicsBody.UseLod1VoxelPhysics)
        return;
      this.InvalidateRange(minVoxelChanged, maxVoxelChanged, 1);
    }

    private void GetPrediction(IMyEntity entity, out BoundingBoxD box)
    {
      Vector3 predictionOffset = this.ComputePredictionOffset(entity);
      box = entity.WorldAABB;
      if ((double) entity.Physics.AngularVelocity.Sum > 0.0299999993294477)
      {
        float num = entity.LocalAABB.HalfExtents.Length();
        box = new BoundingBoxD(box.Center - (double) num, box.Center + num);
      }
      if (box.Extents.Max() > 8.0)
        box.Inflate(8.0);
      else
        box.InflateToMinimum((Vector3D) new Vector3(8f));
      box.Translate((Vector3D) predictionOffset);
    }

    internal void InvalidateRange(Vector3I minVoxelChanged, Vector3I maxVoxelChanged, int lod)
    {
      if (!this.m_bodiesInitialized)
        return;
      if (this.m_queueInvalidation)
      {
        if (this.m_queuedRange.Max.X < 0)
        {
          this.m_queuedRange = new BoundingBoxI(minVoxelChanged, maxVoxelChanged);
        }
        else
        {
          BoundingBoxI box = new BoundingBoxI(minVoxelChanged, maxVoxelChanged);
          this.m_queuedRange.Include(ref box);
        }
      }
      else
      {
        minVoxelChanged -= 2;
        maxVoxelChanged += 1;
        this.m_voxelMap.Storage.ClampVoxelCoord(ref minVoxelChanged);
        this.m_voxelMap.Storage.ClampVoxelCoord(ref maxVoxelChanged);
        Vector3I geometryCellCoord1;
        MyVoxelCoordSystems.VoxelCoordToGeometryCellCoord(ref minVoxelChanged, out geometryCellCoord1);
        Vector3I geometryCellCoord2;
        MyVoxelCoordSystems.VoxelCoordToGeometryCellCoord(ref maxVoxelChanged, out geometryCellCoord2);
        Vector3I minChanged = geometryCellCoord1 - this.m_cellsOffset >> lod;
        Vector3I result = geometryCellCoord2 - this.m_cellsOffset >> lod;
        Vector3I geometryCellCoord3 = this.m_voxelMap.Size - 1;
        MyVoxelCoordSystems.VoxelCoordToGeometryCellCoord(ref geometryCellCoord3, out geometryCellCoord3);
        Vector3I vector3I = geometryCellCoord3 >> lod;
        Vector3I.Min(ref result, ref vector3I, out result);
        HkRigidBody rigidBody = this.GetRigidBody(lod);
        if (minChanged == Vector3I.Zero && result == vector3I)
        {
          this.m_workTracker.CancelAll();
        }
        else
        {
          using (MyUtils.ReuseCollection<MyCellCoord>(ref MyVoxelPhysicsBody.m_toBeCancelledCache))
          {
            BoundingBoxI boundingBoxI = new BoundingBoxI(geometryCellCoord1, geometryCellCoord2);
            foreach (KeyValuePair<MyCellCoord, MyPrecalcJobPhysicsPrefetch> keyValuePair in this.m_workTracker)
            {
              if (boundingBoxI.Contains(keyValuePair.Key.CoordInLod) != ContainmentType.Disjoint)
                MyVoxelPhysicsBody.m_toBeCancelledCache.Add(keyValuePair.Key);
            }
            foreach (MyCellCoord id in MyVoxelPhysicsBody.m_toBeCancelledCache)
              this.m_workTracker.CancelIfStarted(id);
          }
        }
        if ((HkReferenceObject) rigidBody != (HkReferenceObject) null)
        {
          HkUniformGridShape shape = (HkUniformGridShape) rigidBody.GetShape();
          int size = (result - minChanged + 1).Size;
          if (size >= MyVoxelPhysicsBody.m_cellsToGenerateBuffer.Length)
            MyVoxelPhysicsBody.m_cellsToGenerateBuffer = new Vector3I[MathHelper.GetNearestBiggerPowerOfTwo(size)];
          Vector3I[] toGenerateBuffer = MyVoxelPhysicsBody.m_cellsToGenerateBuffer;
          int num = shape.InvalidateRange(ref minChanged, ref result, toGenerateBuffer);
          for (int index = 0; index < num; ++index)
            this.StartPrecalcJobPhysicsIfNeeded(new MyCellCoord(lod, MyVoxelPhysicsBody.m_cellsToGenerateBuffer[index]));
        }
        this.m_voxelMap.RaisePhysicsChanged();
      }
    }

    internal void UpdateBeforeSimulation10()
    {
      if (!this.m_bodiesInitialized)
        this.CreateRigidBodies();
      this.m_voxelQueriesInLastFrames = 0;
    }

    internal void UpdateAfterSimulation10()
    {
      if (this.m_voxelMap.Storage == null)
        return;
      if (--this.m_nearbyEntitiesUpdateCounter < 0)
      {
        this.m_nearbyEntitiesUpdateCounter = 10;
        this.UpdateNearbyEntities();
      }
      foreach (MyEntity nearbyEntity in this.m_nearbyEntities)
      {
        if (nearbyEntity != null)
        {
          bool flag = false;
          if (nearbyEntity.Physics is MyPhysicsBody physics && (HkReferenceObject) physics.RigidBody != (HkReferenceObject) null && (physics.RigidBody.Layer == 23 || physics.RigidBody.Layer == 10))
            flag = true;
          if ((nearbyEntity is MyCubeGrid || flag) && (!nearbyEntity.MarkedForClose && nearbyEntity.Physics != null))
          {
            BoundingBoxD box1;
            this.GetPrediction((IMyEntity) nearbyEntity, out box1);
            if (box1.Intersects(this.m_voxelMap.PositionComp.WorldAABB))
            {
              int lod = flag || !MyVoxelPhysicsBody.UseLod1VoxelPhysics ? 0 : 1;
              int num = 1 << lod;
              BoundingBoxD boundingBoxD = box1.TransformFast(this.m_voxelMap.PositionComp.WorldMatrixInvScaled);
              boundingBoxD.Translate((Vector3D) this.m_voxelMap.SizeInMetresHalf);
              Vector3 max1 = (Vector3) boundingBoxD.Max;
              Vector3 min1 = (Vector3) boundingBoxD.Min;
              Vector3I min2;
              Vector3I max2;
              this.ClampVoxelCoords(ref min1, ref max1, out min2, out max2);
              min2 >>= lod;
              max2 >>= lod;
              int size = (max2 - min2 + 1).Size;
              if (size >= MyVoxelPhysicsBody.m_cellsToGenerateBuffer.Length)
                MyVoxelPhysicsBody.m_cellsToGenerateBuffer = new Vector3I[MathHelper.GetNearestBiggerPowerOfTwo(size)];
              HkUniformGridShape gridShape;
              if (this.GetShape(lod, out gridShape) && !gridShape.Base.IsZero)
              {
                int missingCellsInRange = gridShape.GetMissingCellsInRange(ref min2, ref max2, MyVoxelPhysicsBody.m_cellsToGenerateBuffer);
                if (missingCellsInRange != 0)
                {
                  BoundingBoxI box2 = new BoundingBoxI(min2 * 8 * num, (max2 + 1) * 8 * num);
                  box2.Translate(this.m_voxelMap.StorageMin);
                  if (missingCellsInRange > 0 && this.m_voxelMap.Storage.Intersect(ref box2, lod) != ContainmentType.Intersects)
                  {
                    this.SetEmptyShapes(lod, ref gridShape, missingCellsInRange);
                  }
                  else
                  {
                    for (int index = 0; index < missingCellsInRange; ++index)
                      this.StartPrecalcJobPhysicsIfNeeded(new MyCellCoord(lod, MyVoxelPhysicsBody.m_cellsToGenerateBuffer[index]));
                  }
                }
              }
            }
          }
        }
      }
      this.ScheduleBatchJobs();
      if (!this.m_bodiesInitialized)
        return;
      this.CheckAndDiscardShapes();
    }

    private void ClampVoxelCoords(
      ref Vector3 localPositionMin,
      ref Vector3 localPositionMax,
      out Vector3I min,
      out Vector3I max)
    {
      MyVoxelCoordSystems.LocalPositionToVoxelCoord(ref localPositionMin, out min);
      MyVoxelCoordSystems.LocalPositionToVoxelCoord(ref localPositionMax, out max);
      this.m_voxelMap.Storage.ClampVoxelCoord(ref min);
      this.m_voxelMap.Storage.ClampVoxelCoord(ref max);
      MyVoxelCoordSystems.VoxelCoordToGeometryCellCoord(ref min, out min);
      MyVoxelCoordSystems.VoxelCoordToGeometryCellCoord(ref max, out max);
    }

    private void ScheduleBatchJobs()
    {
      for (int lod = 0; lod < 2; ++lod)
      {
        if (this.InvalidCells[lod].Count > 0 && this.RunningBatchTask[lod] == null)
          MyPrecalcJobPhysicsBatch.Start(this, ref this.InvalidCells[lod], lod);
      }
    }

    private bool StartPrecalcJobPhysicsIfNeeded(MyCellCoord coord)
    {
      if (this.m_workTracker.Exists(coord))
        return false;
      try
      {
        return MyPrecalcJobPhysicsPrefetch.Start(new MyPrecalcJobPhysicsPrefetch.Args()
        {
          TargetPhysics = this,
          Tracker = this.m_workTracker,
          GeometryCell = coord,
          Storage = this.m_voxelMap.Storage
        });
      }
      finally
      {
      }
    }

    private void SetEmptyShapes(int lod, ref HkUniformGridShape shape, int requiredCellsCount)
    {
      for (int index = 0; index < requiredCellsCount; ++index)
      {
        Vector3I coordInLod = MyVoxelPhysicsBody.m_cellsToGenerateBuffer[index];
        this.m_workTracker.Cancel(new MyCellCoord(lod, coordInLod));
        shape.SetChild(coordInLod.X, coordInLod.Y, coordInLod.Z, (HkBvCompressedMeshShape) HkShape.Empty, HkReferencePolicy.TakeOwnership);
      }
    }

    private void CheckAndDiscardShapes()
    {
      ++this.m_lastDiscardCheck;
      if (this.m_lastDiscardCheck <= 18 || this.m_nearbyEntities.Count != 0 || (!((HkReferenceObject) this.RigidBody != (HkReferenceObject) null) || !MyVoxelPhysicsBody.EnableShapeDiscard))
        return;
      this.m_lastDiscardCheck = 0;
      HkUniformGridShape shape1 = (HkUniformGridShape) this.GetShape();
      int hitsAndClear = shape1.GetHitsAndClear();
      if (shape1.ShapeCount <= 0 || hitsAndClear > 0)
        return;
      shape1.DiscardLargeData();
      if (!((HkReferenceObject) this.RigidBody2 != (HkReferenceObject) null))
        return;
      HkUniformGridShape shape2 = (HkUniformGridShape) this.RigidBody2.GetShape();
      if (shape2.GetHitsAndClear() > 0)
        return;
      shape2.DiscardLargeData();
    }

    private Vector3 ComputePredictionOffset(IMyEntity entity) => entity.Physics.LinearVelocity;

    public override void DebugDraw()
    {
      base.DebugDraw();
      if (!MyDebugDrawSettings.DEBUG_DRAW_VOXEL_PHYSICS_PREDICTION)
        return;
      foreach (MyEntity nearbyEntity in this.m_nearbyEntities)
      {
        if (!nearbyEntity.MarkedForClose)
        {
          BoundingBoxD worldAabb = nearbyEntity.PositionComp.WorldAABB;
          MyRenderProxy.DebugDrawAABB(worldAabb, Color.Bisque);
          MyRenderProxy.DebugDrawLine3D(this.GetWorldMatrix().Translation, worldAabb.Center, Color.Bisque, Color.BlanchedAlmond, true);
          BoundingBoxD box;
          this.GetPrediction((IMyEntity) nearbyEntity, out box);
          MyRenderProxy.DebugDrawAABB(box, Color.Crimson);
        }
      }
      using (IMyDebugDrawBatchAabb debugDrawBatchAabb = MyRenderProxy.DebugDrawBatchAABB(this.GetWorldMatrix(), new Color(Color.Cyan, 0.2f), shaded: false))
      {
        int num = 0;
        foreach (KeyValuePair<MyCellCoord, MyPrecalcJobPhysicsPrefetch> keyValuePair in this.m_workTracker)
        {
          ++num;
          MyCellCoord key = keyValuePair.Key;
          BoundingBoxD aabb;
          aabb.Min = (Vector3D) (key.CoordInLod << key.Lod);
          aabb.Min *= 8.0;
          aabb.Min -= this.m_voxelMap.SizeInMetresHalf;
          aabb.Max = aabb.Min + 8f;
          debugDrawBatchAabb.Add(ref aabb);
          if (num > 250)
            break;
        }
      }
    }

    internal void OnTaskComplete(MyCellCoord coord, HkBvCompressedMeshShape childShape)
    {
      if (!((HkReferenceObject) this.RigidBody != (HkReferenceObject) null))
        return;
      HkUniformGridShape gridShape;
      this.GetShape(coord.Lod, out gridShape);
      gridShape.SetChild(coord.CoordInLod.X, coord.CoordInLod.Y, coord.CoordInLod.Z, childShape, HkReferencePolicy.None);
      if (childShape.IsZero)
        return;
      this.MarkForShapeUpdate();
    }

    internal void OnBatchTaskComplete(
      Dictionary<Vector3I, HkBvCompressedMeshShape> newShapes,
      int lod)
    {
      if (!((HkReferenceObject) this.RigidBody != (HkReferenceObject) null))
        return;
      HkUniformGridShape gridShape;
      this.GetShape(lod, out gridShape);
      bool flag = false;
      foreach (KeyValuePair<Vector3I, HkBvCompressedMeshShape> newShape in newShapes)
      {
        Vector3I key = newShape.Key;
        HkBvCompressedMeshShape shape = newShape.Value;
        gridShape.SetChild(key.X, key.Y, key.Z, shape, HkReferencePolicy.None);
        flag |= !shape.IsZero;
      }
      if (!flag)
        return;
      this.MarkForShapeUpdate();
    }

    internal VrVoxelMesh CreateMesh(MyCellCoord coord)
    {
      coord.CoordInLod += this.m_cellsOffset >> coord.Lod;
      Vector3I vector3I1 = coord.CoordInLod << 3;
      Vector3I vector3I2 = vector3I1 + 8;
      Vector3I lodVoxelMin = vector3I1 - 1;
      Vector3I lodVoxelMax = vector3I2 + 2;
      MyMesherResult mesh = this.m_mesher.CalculateMesh(coord.Lod, lodVoxelMin, lodVoxelMax, flags: (MyVoxelRequestFlags.SurfaceMaterial | MyVoxelRequestFlags.ForPhysics));
      if (!mesh.MeshProduced)
      {
        MyVoxelDebugInputComponent.PhysicsComponent physicsComponent = MyVoxelDebugInputComponent.PhysicsComponent.Static;
      }
      return mesh.Mesh;
    }

    internal unsafe HkBvCompressedMeshShape CreateShape(
      VrVoxelMesh mesh,
      int lod)
    {
      if (mesh == null || mesh.TriangleCount == 0 || mesh.VertexCount == 0)
        return (HkBvCompressedMeshShape) HkShape.Empty;
      using (MyUtils.ReuseCollection<int>(ref MyVoxelPhysicsBody.m_indexListCached))
      {
        using (MyUtils.ReuseCollection<Vector3>(ref MyVoxelPhysicsBody.m_vertexListCached))
        {
          using (MyUtils.ReuseCollection<byte>(ref MyVoxelPhysicsBody.m_materialListCached))
          {
            MyList<int> indexListCached = MyVoxelPhysicsBody.m_indexListCached;
            MyList<Vector3> vertexListCached = MyVoxelPhysicsBody.m_vertexListCached;
            MyList<byte> materialListCached = MyVoxelPhysicsBody.m_materialListCached;
            vertexListCached.EnsureCapacity(mesh.VertexCount);
            indexListCached.EnsureCapacity(mesh.TriangleCount * 3);
            materialListCached.EnsureCapacity(mesh.TriangleCount);
            for (int index = 0; index < mesh.TriangleCount; ++index)
            {
              indexListCached.Add((int) mesh.Triangles[index].V0);
              indexListCached.Add((int) mesh.Triangles[index].V2);
              indexListCached.Add((int) mesh.Triangles[index].V1);
            }
            float scale = mesh.Scale;
            VrVoxelVertex* vertices1 = mesh.Vertices;
            Vector3 vector3 = mesh.Start * scale - this.m_voxelMap.StorageMin * 1f;
            for (int index = 0; index < mesh.VertexCount; ++index)
              vertexListCached.Add(vertices1[index].Position * scale + vector3);
            uint num = 4294967294;
            for (int index = 0; index < mesh.TriangleCount; ++index)
            {
              VrVoxelTriangle vrVoxelTriangle = mesh.Triangles[index];
              byte material = vertices1[vrVoxelTriangle.V0].Material;
              if (num == 4294967294U)
                num = (uint) material;
              else if ((int) num != (int) material)
                num = uint.MaxValue;
              materialListCached.Add(material);
            }
            fixed (int* indices = indexListCached.GetInternalArray())
              fixed (byte* materials = materialListCached.GetInternalArray())
                fixed (Vector3* vertices2 = vertexListCached.GetInternalArray())
                {
                  float physicsConvexRadius = MyPerGameSettings.PhysicsConvexRadius;
                  HkBvCompressedMeshShape compressedMeshShape = new HkBvCompressedMeshShape(vertices2, vertexListCached.Count, indices, indexListCached.Count, materials, materialListCached.Count, HkWeldingType.None, physicsConvexRadius);
                  if (num == 4294967294U)
                    num = uint.MaxValue;
                  HkShape.SetUserData((HkShape) compressedMeshShape, (ulong) num);
                  return compressedMeshShape;
                }
          }
        }
      }
    }

    public override bool IsStaticForCluster
    {
      get => this.m_staticForCluster;
      set => this.m_staticForCluster = value;
    }

    public override void Close()
    {
      base.Close();
      this.m_workTracker.CancelAll();
      for (int index = 0; index < this.RunningBatchTask.Length; ++index)
      {
        if (this.RunningBatchTask[index] != null)
        {
          this.RunningBatchTask[index].Cancel();
          this.RunningBatchTask[index] = (MyPrecalcJobPhysicsBatch) null;
        }
      }
    }

    private void UpdateNearbyEntities()
    {
      this.m_nearbyEntities.Clear();
      Vector3 vector3 = this.m_voxelMap.SizeInMetres * this.m_phantomExtend;
      BoundingBoxD boundingBox;
      ref BoundingBoxD local = ref boundingBox;
      BoundingBoxD worldAabb = this.m_voxelMap.PositionComp.WorldAABB;
      Vector3D min = worldAabb.Center - 0.5f * vector3;
      worldAabb = this.m_voxelMap.PositionComp.WorldAABB;
      Vector3D max = worldAabb.Center + 0.5f * vector3;
      local = new BoundingBoxD(min, max);
      MyEntities.GetTopMostEntitiesInBox(ref boundingBox, this.m_nearbyEntities, MyEntityQueryType.Dynamic);
      for (int index = this.m_nearbyEntities.Count - 1; index >= 0; --index)
      {
        MyEntity nearbyEntity = this.m_nearbyEntities[index];
        if (!(nearbyEntity is MyCharacter))
        {
          HkRigidBody rigidBody = nearbyEntity.Physics?.RigidBody;
          if ((HkReferenceObject) rigidBody == (HkReferenceObject) null || rigidBody.IsFixedOrKeyframed)
            this.m_nearbyEntities.RemoveAt(index);
        }
      }
    }

    internal void GenerateAllShapes()
    {
      if (this.m_mesher == null)
        return;
      if (!this.m_bodiesInitialized)
        this.CreateRigidBodies();
      Vector3I zero = Vector3I.Zero;
      Vector3I size = this.m_voxelMap.Size;
      Vector3I end = new Vector3I(0, 0, 0);
      end.X = size.X >> 3;
      end.Y = size.Y >> 3;
      end.Z = size.Z >> 3;
      end += zero;
      MyPrecalcJobPhysicsPrefetch.Args args = new MyPrecalcJobPhysicsPrefetch.Args()
      {
        GeometryCell = new MyCellCoord(1, zero),
        Storage = this.m_voxelMap.Storage,
        TargetPhysics = this,
        Tracker = this.m_workTracker
      };
      Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref zero, ref end);
      while (vector3IRangeIterator.IsValid())
      {
        MyPrecalcJobPhysicsPrefetch.Start(args);
        vector3IRangeIterator.GetNext(out args.GeometryCell.CoordInLod);
      }
    }

    public override MyStringHash GetMaterialAt(Vector3D worldPos)
    {
      MyVoxelMaterialDefinition materialDefinition = this.m_voxelMap != null ? this.m_voxelMap.GetMaterialAt(ref worldPos) : (MyVoxelMaterialDefinition) null;
      return materialDefinition == null ? MyStringHash.NullOrEmpty : materialDefinition.MaterialTypeNameHash;
    }

    public void PrefetchShapeOnRay(ref LineD ray)
    {
      if (this.m_mesher == null)
        return;
      int lod = MyVoxelPhysicsBody.UseLod1VoxelPhysics ? 1 : 0;
      Vector3 localPosition1;
      MyVoxelCoordSystems.WorldPositionToLocalPosition(this.m_voxelMap.PositionLeftBottomCorner, ref ray.From, out localPosition1);
      Vector3 localPosition2;
      MyVoxelCoordSystems.WorldPositionToLocalPosition(this.m_voxelMap.PositionLeftBottomCorner, ref ray.To, out localPosition2);
      HkUniformGridShape gridShape;
      if (!this.GetShape(lod, out gridShape))
        return;
      if (MyVoxelPhysicsBody.m_cellsToGenerateBuffer.Length < 64)
        MyVoxelPhysicsBody.m_cellsToGenerateBuffer = new Vector3I[64];
      int hitCellsInRange = gridShape.GetHitCellsInRange(localPosition1, localPosition2, MyVoxelPhysicsBody.m_cellsToGenerateBuffer);
      if (hitCellsInRange == 0)
        return;
      for (int index = 0; index < hitCellsInRange; ++index)
      {
        MyCellCoord id = new MyCellCoord(lod, MyVoxelPhysicsBody.m_cellsToGenerateBuffer[index]);
        if (!this.m_workTracker.Exists(id))
          MyPrecalcJobPhysicsPrefetch.Start(new MyPrecalcJobPhysicsPrefetch.Args()
          {
            TargetPhysics = this,
            Tracker = this.m_workTracker,
            GeometryCell = id,
            Storage = this.m_voxelMap.Storage
          });
      }
    }

    public Vector3 ComputeCellCenterOffset(Vector3 bodyLocal) => (Vector3I) (bodyLocal / 8f) * 8f;

    private class Sandbox_Engine_Voxels_MyVoxelPhysicsBody\u003C\u003EActor
    {
    }
  }
}
