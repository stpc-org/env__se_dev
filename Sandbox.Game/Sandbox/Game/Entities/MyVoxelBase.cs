// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyVoxelBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
  public abstract class MyVoxelBase : MyEntity, IMyVoxelDrawable, IMyVoxelBase, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, IMyDecalProxy, IMyEventProxy, IMyEventOwner
  {
    public int VoxelMapPruningProxyId = -1;
    protected Vector3I m_storageMin = new Vector3I(0, 0, 0);
    protected Vector3I m_storageMax;
    public bool? IsSeedOpen;
    private VRage.Game.Voxels.IMyStorage m_storageInternal;
    public bool CreateStorageCopyOnWrite;
    private bool m_contentChanged;
    private bool m_beforeContentChanged;
    [ThreadStatic]
    protected static MyStorageData m_tempStorage;
    private static readonly MyShapeSphere m_sphereShape = new MyShapeSphere();
    private static readonly MyShapeBox m_boxShape = new MyShapeBox();
    private static readonly MyShapeRamp m_rampShape = new MyShapeRamp();
    private static readonly MyShapeCapsule m_capsuleShape = new MyShapeCapsule();
    private static readonly MyShapeEllipsoid m_ellipsoidShape = new MyShapeEllipsoid();
    private static readonly List<MyEntity> m_foundElements = new List<MyEntity>();
    public MyBoulderInformation? BoulderInfo;
    private bool m_createdByUser;
    private bool m_voxelShapeInProgress;

    public Vector3I StorageMin => this.m_storageMin;

    public Vector3I StorageMax => this.m_storageMax;

    public string StorageName { get; protected set; }

    public float VoxelSize { get; private set; }

    protected VRage.Game.Voxels.IMyStorage m_storage
    {
      get => this.m_storageInternal;
      set
      {
        if (value != null && !value.Shared && (value is MyStorageBase myStorageBase && !myStorageBase.CachedWrites))
          myStorageBase.InitWriteCache();
        this.m_storageInternal = value;
      }
    }

    public virtual VRage.Game.Voxels.IMyStorage Storage
    {
      get => this.m_storage;
      set => this.m_storage = value;
    }

    public bool DelayRigidBodyCreation { get; set; }

    public Vector3I Size => this.m_storageMax - this.m_storageMin;

    public Vector3I SizeMinusOne => this.Size - 1;

    public Vector3 SizeInMetres { get; protected set; }

    public Vector3 SizeInMetresHalf { get; protected set; }

    public virtual Vector3D PositionLeftBottomCorner { get; set; }

    public Matrix Orientation => (Matrix) ref this.PositionComp.WorldMatrixRef;

    public bool ContentChanged
    {
      get => this.m_contentChanged;
      protected set
      {
        this.m_contentChanged = value;
        this.BeforeContentChanged = false;
      }
    }

    public abstract MyVoxelBase RootVoxel { get; }

    public bool BeforeContentChanged
    {
      get => this.m_beforeContentChanged;
      protected set
      {
        if (this.m_beforeContentChanged == value)
          return;
        this.m_beforeContentChanged = value;
        if (!this.m_beforeContentChanged || this.Storage == null || (!this.Storage.Shared || this.m_storage == null))
          return;
        this.Storage = this.m_storage.Copy();
        this.StorageName = MyVoxelMap.GetNewStorageName(this.StorageName, this.EntityId);
      }
    }

    public event MyVoxelBase.StorageChanged RangeChanged;

    public bool CreatedByUser
    {
      get => this.m_createdByUser;
      set => this.m_createdByUser = value;
    }

    public string AsteroidName { get; set; }

    protected internal void OnRangeChanged(
      Vector3I voxelRangeMin,
      Vector3I voxelRangeMax,
      MyStorageDataTypeFlags changedData)
    {
      if (this.RangeChanged == null)
        return;
      this.RangeChanged(this, voxelRangeMin, voxelRangeMax, changedData);
    }

    public bool IsBoxIntersectingBoundingBoxOfThisVoxelMap(ref BoundingBoxD boundingBox)
    {
      bool result;
      this.PositionComp.WorldAABB.Intersects(ref boundingBox, out result);
      return result;
    }

    public MyVoxelBase() => this.VoxelSize = 1f;

    public abstract void Init(MyObjectBuilder_EntityBase builder, VRage.Game.Voxels.IMyStorage storage);

    public void Init(string storageName, VRage.Game.Voxels.IMyStorage storage, Vector3D positionMinCorner)
    {
      MatrixD translation = MatrixD.CreateTranslation(positionMinCorner + storage.Size / 2);
      this.Init(storageName, storage, translation);
    }

    public virtual void Init(
      string storageName,
      VRage.Game.Voxels.IMyStorage storage,
      MatrixD worldMatrix,
      bool useVoxelOffset = true)
    {
      if (this.Name == null)
        this.Init((MyObjectBuilder_EntityBase) null);
      this.StorageName = storageName;
      this.m_storage = storage;
      this.InitVoxelMap(worldMatrix, storage.Size, useVoxelOffset);
    }

    protected virtual void InitVoxelMap(MatrixD worldMatrix, Vector3I size, bool useOffset = true)
    {
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
      this.SizeInMetres = size * 1f;
      this.SizeInMetresHalf = this.SizeInMetres / 2f;
      this.PositionComp.LocalAABB = new BoundingBox(-this.SizeInMetresHalf, this.SizeInMetresHalf);
      if (MyPerGameSettings.OffsetVoxelMapByHalfVoxel & useOffset)
      {
        worldMatrix.Translation += 0.5f;
        this.PositionLeftBottomCorner += 0.5f;
      }
      this.PositionComp.SetWorldMatrix(ref worldMatrix);
      this.ContentChanged = false;
    }

    protected override void BeforeDelete()
    {
      base.BeforeDelete();
      this.RangeChanged = (MyVoxelBase.StorageChanged) null;
      if (this.Storage == null || this.Storage.Shared || this is MyVoxelPhysics)
        return;
      this.Storage.Close();
    }

    VRage.ModAPI.IMyStorage IMyVoxelBase.Storage => (VRage.ModAPI.IMyStorage) this.Storage;

    string IMyVoxelBase.StorageName => this.StorageName;

    public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
    {
      MyObjectBuilder_VoxelMap objectBuilder = (MyObjectBuilder_VoxelMap) base.GetObjectBuilder(copy);
      Vector3D leftBottomCorner = this.PositionLeftBottomCorner;
      this.PositionLeftBottomCorner = this.WorldMatrix.Translation - Vector3D.TransformNormal(this.SizeInMetresHalf, this.WorldMatrix);
      if (MyPerGameSettings.OffsetVoxelMapByHalfVoxel)
        leftBottomCorner -= 0.5;
      objectBuilder.PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation(leftBottomCorner, (Vector3) this.WorldMatrix.Forward, (Vector3) this.WorldMatrix.Up));
      objectBuilder.StorageName = this.StorageName;
      objectBuilder.MutableStorage = true;
      objectBuilder.ContentChanged = new bool?(this.ContentChanged);
      objectBuilder.CreatedByUser = this.CreatedByUser;
      if (this.BoulderInfo.HasValue)
      {
        objectBuilder.BoulderPlanetId = new long?(this.BoulderInfo.Value.PlanetId);
        objectBuilder.BoulderSectorId = new long?(this.BoulderInfo.Value.SectorId);
        objectBuilder.BoulderItemId = new int?(this.BoulderInfo.Value.ItemId);
      }
      else
      {
        MyObjectBuilder_VoxelMap objectBuilderVoxelMap1 = objectBuilder;
        MyObjectBuilder_VoxelMap objectBuilderVoxelMap2 = objectBuilder;
        MyObjectBuilder_VoxelMap objectBuilderVoxelMap3 = objectBuilder;
        int? nullable1 = new int?();
        int? nullable2;
        int? nullable3 = nullable2 = nullable1;
        objectBuilderVoxelMap3.BoulderItemId = nullable2;
        int? nullable4 = nullable3;
        long? nullable5;
        long? nullable6 = nullable5 = nullable4.HasValue ? new long?((long) nullable4.GetValueOrDefault()) : new long?();
        objectBuilderVoxelMap2.BoulderSectorId = nullable5;
        long? nullable7 = nullable6;
        objectBuilderVoxelMap1.BoulderPlanetId = nullable7;
      }
      return (MyObjectBuilder_EntityBase) objectBuilder;
    }

    protected void WorldPositionChanged(object source) => this.PositionLeftBottomCorner = this.WorldMatrix.Translation - Vector3D.TransformNormal(this.SizeInMetresHalf, this.WorldMatrix);

    public MyTuple<float, float> GetVoxelContentInBoundingBox_Fast(
      BoundingBoxD localAabb,
      MatrixD worldMatrix)
    {
      MatrixD matrix = worldMatrix * this.PositionComp.WorldMatrixNormalizedInv;
      MatrixD.Invert(ref matrix, out MatrixD _);
      MyOrientedBoundingBoxD orientedBoundingBoxD = new MyOrientedBoundingBoxD(localAabb, worldMatrix);
      BoundingBoxD boundingBoxD = orientedBoundingBoxD.GetAABB();
      boundingBoxD = boundingBoxD.TransformFast(this.PositionComp.WorldMatrixNormalizedInv);
      boundingBoxD.Translate((Vector3D) (this.SizeInMetresHalf + (Vector3) this.StorageMin));
      Vector3I vector3I1 = Vector3I.Floor(boundingBoxD.Min);
      Vector3I vector3I2 = Vector3I.Ceiling((Vector3) boundingBoxD.Max);
      int lodIndex = Math.Max((MathHelper.Log2Ceiling((int) (localAabb.Volume / 1.0)) - MathHelper.Log2Ceiling(100)) / 3, 0);
      float num1 = 1f * (float) (1 << lodIndex);
      float num2 = num1 * num1 * num1;
      Vector3I lodVoxelRangeMin = vector3I1 >> lodIndex;
      Vector3I lodVoxelRangeMax = vector3I2 >> lodIndex;
      Vector3I vector3I3 = (this.Size >> 1) + this.StorageMin >> lodIndex;
      if (MyVoxelBase.m_tempStorage == null)
        MyVoxelBase.m_tempStorage = new MyStorageData();
      MyVoxelBase.m_tempStorage.Resize(lodVoxelRangeMax - lodVoxelRangeMin + 1);
      this.Storage.ReadRange(MyVoxelBase.m_tempStorage, MyStorageDataTypeFlags.Content, lodIndex, lodVoxelRangeMin, lodVoxelRangeMax);
      float num3 = 0.0f;
      float num4 = 0.0f;
      int num5 = 0;
      Vector3I vector3I4;
      vector3I4.Z = lodVoxelRangeMin.Z;
      Vector3I p;
      for (p.Z = 0; vector3I4.Z <= lodVoxelRangeMax.Z; ++p.Z)
      {
        vector3I4.Y = lodVoxelRangeMin.Y;
        for (p.Y = 0; vector3I4.Y <= lodVoxelRangeMax.Y; ++p.Y)
        {
          vector3I4.X = lodVoxelRangeMin.X;
          for (p.X = 0; vector3I4.X <= lodVoxelRangeMax.X; ++p.X)
          {
            float num6 = (float) MyVoxelBase.m_tempStorage.Content(ref p) / (float) byte.MaxValue;
            if ((double) num6 > 0.0)
            {
              BoundingBoxD box = new BoundingBoxD();
              box.Min = ((Vector3D) vector3I4 - 0.5) * (double) num1 - this.SizeInMetresHalf;
              box.Max = ((Vector3D) vector3I4 + 0.5) * (double) num1 - this.SizeInMetresHalf;
              MatrixD worldMatrix1 = this.WorldMatrix;
              worldMatrix1.Translation = Vector3D.Zero;
              MyOrientedBoundingBoxD other = new MyOrientedBoundingBoxD(box, worldMatrix1);
              other.Center += this.WorldMatrix.Translation;
              if (orientedBoundingBoxD.Contains(ref other) != ContainmentType.Disjoint)
              {
                num3 += num6 * num2;
                num4 += num6;
                ++num5;
              }
            }
            ++vector3I4.X;
          }
          ++vector3I4.Y;
        }
        ++vector3I4.Z;
      }
      float num7 = num4 / (float) num5;
      return new MyTuple<float, float>(num3, num7);
    }

    public override bool GetIntersectionWithLine(
      ref LineD worldLine,
      out MyIntersectionResultLineTriangleEx? t,
      IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
    {
      t = new MyIntersectionResultLineTriangleEx?();
      if (!this.PositionComp.WorldAABB.Intersects(ref worldLine, out double _))
        return false;
      try
      {
        Line line = new Line((Vector3) (worldLine.From - this.PositionLeftBottomCorner), (Vector3) (worldLine.To - this.PositionLeftBottomCorner));
        MyIntersectionResultLineTriangle result;
        if (this.Storage.GetGeometry().Intersect(ref line, out result, flags))
        {
          t = new MyIntersectionResultLineTriangleEx?(new MyIntersectionResultLineTriangleEx(result, (VRage.ModAPI.IMyEntity) this, ref line));
          MyIntersectionResultLineTriangleEx resultLineTriangleEx = t.Value;
          return true;
        }
        t = new MyIntersectionResultLineTriangleEx?();
        return false;
      }
      finally
      {
      }
    }

    public override bool GetIntersectionWithLine(
      ref LineD worldLine,
      out Vector3D? v,
      bool useCollisionModel = true,
      IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
    {
      MyIntersectionResultLineTriangleEx? t;
      this.GetIntersectionWithLine(ref worldLine, out t, IntersectionFlags.ALL_TRIANGLES);
      v = new Vector3D?();
      if (!t.HasValue)
        return false;
      v = new Vector3D?(t.Value.IntersectionPointInWorldSpace);
      return true;
    }

    public bool AreAllAabbCornersInside(ref MatrixD aabbWorldTransform, BoundingBoxD aabb) => this.CountCornersInside(ref aabbWorldTransform, ref aabb) == 8;

    public bool IsAnyAabbCornerInside(ref MatrixD aabbWorldTransform, BoundingBoxD aabb) => this.CountCornersInside(ref aabbWorldTransform, ref aabb) > 0;

    private unsafe int CountCornersInside(ref MatrixD aabbWorldTransform, ref BoundingBoxD aabb)
    {
      Vector3D* vector3DPtr = stackalloc Vector3D[8];
      aabb.GetCornersUnsafe(vector3DPtr);
      for (int index = 0; index < 8; ++index)
        Vector3D.Transform(ref vector3DPtr[index], ref aabbWorldTransform, out vector3DPtr[index]);
      return this.CountPointsInside(vector3DPtr, 8);
    }

    public unsafe int CountPointsInside(Vector3D* worldPoints, int pointCount)
    {
      if (MyVoxelBase.m_tempStorage == null)
        MyVoxelBase.m_tempStorage = new MyStorageData();
      MatrixD matrix = this.PositionComp.WorldMatrixInvScaled;
      int num1 = 0;
      Vector3I vector3I1 = new Vector3I(int.MaxValue);
      Vector3I vector3I2 = new Vector3I(int.MinValue);
      for (int index = 0; index < pointCount; ++index)
      {
        Vector3D result;
        Vector3D.Transform(ref worldPoints[index], ref matrix, out result);
        Vector3D r = result + (Vector3D) (this.Size / 2);
        Vector3I vector3I3 = Vector3D.Floor(r);
        Vector3D.Fract(ref r, out r);
        Vector3I vector3I4 = vector3I3 + this.StorageMin;
        Vector3I vector3I5 = vector3I4 + 1;
        if (vector3I4 != vector3I1 && vector3I5 != vector3I2)
        {
          MyVoxelBase.m_tempStorage.Resize(vector3I4, vector3I5);
          this.Storage.ReadRange(MyVoxelBase.m_tempStorage, MyStorageDataTypeFlags.Content, 0, vector3I4, vector3I5);
          vector3I1 = vector3I4;
          vector3I2 = vector3I5;
        }
        double num2 = (double) MyVoxelBase.m_tempStorage.Content(0, 0, 0);
        double num3 = (double) MyVoxelBase.m_tempStorage.Content(1, 0, 0);
        double num4 = (double) MyVoxelBase.m_tempStorage.Content(0, 1, 0);
        double num5 = (double) MyVoxelBase.m_tempStorage.Content(1, 1, 0);
        double num6 = (double) MyVoxelBase.m_tempStorage.Content(0, 0, 1);
        double num7 = (double) MyVoxelBase.m_tempStorage.Content(1, 0, 1);
        double num8 = (double) MyVoxelBase.m_tempStorage.Content(0, 1, 1);
        double num9 = (double) MyVoxelBase.m_tempStorage.Content(1, 1, 1);
        double num10 = num2 + (num3 - num2) * r.X;
        double num11 = num4 + (num5 - num4) * r.X;
        double num12 = num6 + (num7 - num6) * r.X;
        double num13 = num8 + (num9 - num8) * r.X;
        double num14 = num10 + (num11 - num10) * r.Y;
        double num15 = num12 + (num13 - num12) * r.Y;
        if (num14 + (num15 - num14) * r.Z >= (double) sbyte.MaxValue)
          ++num1;
      }
      return num1;
    }

    public virtual bool IsOverlapOverThreshold(BoundingBoxD worldAabb, float thresholdPercentage = 0.9f) => false;

    public virtual MyClipmapScaleEnum ScaleGroup => MyClipmapScaleEnum.Normal;

    public override bool DoOverlapSphereTest(float sphereRadius, Vector3D spherePos)
    {
      if (this.Storage.Closed)
        return false;
      spherePos = Vector3D.Transform(spherePos, this.PositionComp.WorldMatrixInvScaled);
      spherePos /= (double) this.VoxelSize;
      sphereRadius /= this.VoxelSize;
      spherePos += this.SizeInMetresHalf;
      return this.OverlapsSphereLocal(sphereRadius, spherePos);
    }

    internal void RevertProceduralAsteroidVoxelSettings()
    {
      if (!this.CreatedByUser)
        this.Save = false;
      this.ContentChanged = false;
      this.BeforeContentChanged = false;
    }

    protected bool OverlapsSphereLocal(float sphereRadius, Vector3D spherePos)
    {
      double num = (double) sphereRadius * (double) sphereRadius;
      Vector3I voxelCoord1 = new Vector3I(spherePos - (double) sphereRadius);
      Vector3I voxelCoord2 = new Vector3I(spherePos + sphereRadius);
      this.Storage.ClampVoxelCoord(ref voxelCoord1);
      this.Storage.ClampVoxelCoord(ref voxelCoord2);
      BoundingBoxI box = new BoundingBoxI(voxelCoord1, voxelCoord2);
      if (this.Storage.Intersect(ref box, 0) == ContainmentType.Disjoint)
        return false;
      if (MyVoxelBase.m_tempStorage == null)
        MyVoxelBase.m_tempStorage = new MyStorageData();
      MyVoxelBase.m_tempStorage.Resize(voxelCoord1, voxelCoord2);
      this.Storage.ReadRange(MyVoxelBase.m_tempStorage, MyStorageDataTypeFlags.Content, 0, voxelCoord1, voxelCoord2);
      Vector3I vector3I;
      vector3I.Z = voxelCoord1.Z;
      Vector3I p;
      for (p.Z = 0; vector3I.Z <= voxelCoord2.Z; ++p.Z)
      {
        vector3I.Y = voxelCoord1.Y;
        for (p.Y = 0; vector3I.Y <= voxelCoord2.Y; ++p.Y)
        {
          vector3I.X = voxelCoord1.X;
          for (p.X = 0; vector3I.X <= voxelCoord2.X; ++p.X)
          {
            if (MyVoxelBase.m_tempStorage.Content(ref p) >= (byte) 127)
            {
              Vector3 max = vector3I + this.VoxelSize;
              if (new BoundingBox((Vector3) vector3I, max).Contains(spherePos) == ContainmentType.Contains || Vector3D.DistanceSquared((Vector3D) vector3I, spherePos) < num)
                return true;
            }
            ++vector3I.X;
          }
          ++vector3I.Y;
        }
        ++vector3I.Z;
      }
      return false;
    }

    public bool GetContainedVoxelCoords(
      ref BoundingBoxD worldAabb,
      out Vector3I min,
      out Vector3I max)
    {
      min = new Vector3I();
      max = new Vector3I();
      if (!this.IsBoxIntersectingBoundingBoxOfThisVoxelMap(ref worldAabb))
        return false;
      MyVoxelCoordSystems.WorldPositionToVoxelCoord(this.PositionLeftBottomCorner, ref worldAabb.Min, out min);
      MyVoxelCoordSystems.WorldPositionToVoxelCoord(this.PositionLeftBottomCorner, ref worldAabb.Max, out max);
      min += this.StorageMin;
      max += this.StorageMin;
      this.Storage.ClampVoxelCoord(ref min);
      this.Storage.ClampVoxelCoord(ref max);
      return true;
    }

    public virtual int GetOrePriority() => 1;

    void IMyDecalProxy.AddDecals(
      ref MyHitInfo hitInfo,
      MyStringHash source,
      Vector3 forwardDirection,
      object customdata,
      IMyDecalHandler decalHandler,
      MyStringHash physicalMaterial,
      MyStringHash voxelMaterial,
      bool isTrail)
    {
      MyDecalRenderInfo renderInfo = new MyDecalRenderInfo()
      {
        Flags = MyDecalFlags.World,
        Position = hitInfo.Position,
        Normal = hitInfo.Normal,
        Source = source,
        RenderObjectIds = this.RootVoxel.Render.RenderObjectIDs,
        PhysicalMaterial = physicalMaterial,
        VoxelMaterial = voxelMaterial,
        Forward = forwardDirection,
        IsTrail = isTrail
      };
      decalHandler.AddDecal(ref renderInfo);
    }

    public void RequestVoxelCutoutSphere(
      Vector3D center,
      float radius,
      bool createDebris,
      bool damage)
    {
      MyMultiplayer.RaiseEvent<MyVoxelBase, Vector3D, float, bool, bool>(this.RootVoxel, (Func<MyVoxelBase, Action<Vector3D, float, bool, bool>>) (x => new Action<Vector3D, float, bool, bool>(x.VoxelCutoutSphere_Implementation)), center, radius, createDebris, damage);
    }

    [Event(null, 755)]
    [Reliable]
    [Broadcast]
    [RefreshReplicable]
    private void VoxelCutoutSphere_Implementation(
      Vector3D center,
      float radius,
      bool createDebris,
      bool damage = false)
    {
      this.BeforeContentChanged = true;
      MyExplosion.CutOutVoxelMap(radius, center, this, createDebris && MySession.Static.Ready, damage);
    }

    public void RequestVoxelOperationCapsule(
      Vector3D A,
      Vector3D B,
      float radius,
      MatrixD Transformation,
      byte material,
      MyVoxelBase.OperationType Type)
    {
      MyMultiplayer.RaiseStaticEvent<long, MyVoxelBase.MyCapsuleShapeParams, MyVoxelBase.OperationType>((Func<IMyEventOwner, Action<long, MyVoxelBase.MyCapsuleShapeParams, MyVoxelBase.OperationType>>) (s => new Action<long, MyVoxelBase.MyCapsuleShapeParams, MyVoxelBase.OperationType>(MyVoxelBase.VoxelOperationCapsule_Implementation)), this.EntityId, new MyVoxelBase.MyCapsuleShapeParams()
      {
        A = A,
        B = B,
        Radius = radius,
        Transformation = Transformation,
        Material = material
      }, Type);
    }

    [Event(null, 774)]
    [Reliable]
    [Server]
    [RefreshReplicable]
    private static void VoxelOperationCapsule_Implementation(
      long entityId,
      MyVoxelBase.MyCapsuleShapeParams capsuleParams,
      MyVoxelBase.OperationType Type)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.GetVoxelHandAvailable(MyEventContext.Current.Sender.Value))
      {
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        MyVoxelBase.m_capsuleShape.Transformation = capsuleParams.Transformation;
        MyVoxelBase.m_capsuleShape.A = capsuleParams.A;
        MyVoxelBase.m_capsuleShape.B = capsuleParams.B;
        MyVoxelBase.m_capsuleShape.Radius = capsuleParams.Radius;
        if (!MyVoxelBase.CanPlaceInArea(Type, (MyShape) MyVoxelBase.m_capsuleShape))
          return;
        MyEntity entity;
        MyEntities.TryGetEntityById(entityId, out entity);
        if (!(entity is MyVoxelBase myVoxelBase) || myVoxelBase.m_voxelShapeInProgress)
          return;
        myVoxelBase.BeforeContentChanged = true;
        MyMultiplayer.RaiseEvent<MyVoxelBase, MyVoxelBase.MyCapsuleShapeParams, MyVoxelBase.OperationType>(myVoxelBase.RootVoxel, (Func<MyVoxelBase, Action<MyVoxelBase.MyCapsuleShapeParams, MyVoxelBase.OperationType>>) (x => new Action<MyVoxelBase.MyCapsuleShapeParams, MyVoxelBase.OperationType>(x.PerformVoxelOperationCapsule_Implementation)), capsuleParams, Type);
        myVoxelBase.UpdateVoxelShape(Type, (MyShape) MyVoxelBase.m_capsuleShape, capsuleParams.Material);
      }
    }

    [Event(null, 805)]
    [Reliable]
    [Broadcast]
    [RefreshReplicable]
    private void PerformVoxelOperationCapsule_Implementation(
      MyVoxelBase.MyCapsuleShapeParams capsuleParams,
      MyVoxelBase.OperationType Type)
    {
      this.BeforeContentChanged = true;
      MyVoxelBase.m_capsuleShape.Transformation = capsuleParams.Transformation;
      MyVoxelBase.m_capsuleShape.A = capsuleParams.A;
      MyVoxelBase.m_capsuleShape.B = capsuleParams.B;
      MyVoxelBase.m_capsuleShape.Radius = capsuleParams.Radius;
      this.UpdateVoxelShape(Type, (MyShape) MyVoxelBase.m_capsuleShape, capsuleParams.Material);
    }

    public void RequestVoxelOperationSphere(
      Vector3D center,
      float radius,
      byte material,
      MyVoxelBase.OperationType Type)
    {
      MyMultiplayer.RaiseStaticEvent<long, Vector3D, float, byte, MyVoxelBase.OperationType>((Func<IMyEventOwner, Action<long, Vector3D, float, byte, MyVoxelBase.OperationType>>) (s => new Action<long, Vector3D, float, byte, MyVoxelBase.OperationType>(MyVoxelBase.VoxelOperationSphere_Implementation)), this.EntityId, center, radius, material, Type);
    }

    [Event(null, 823)]
    [Reliable]
    [Server]
    private static void VoxelOperationSphere_Implementation(
      long entityId,
      Vector3D center,
      float radius,
      byte material,
      MyVoxelBase.OperationType Type)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.GetVoxelHandAvailable(MyEventContext.Current.Sender.Value))
      {
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        MyVoxelBase.m_sphereShape.Center = center;
        MyVoxelBase.m_sphereShape.Radius = radius;
        if (!MyVoxelBase.CanPlaceInArea(Type, (MyShape) MyVoxelBase.m_sphereShape))
          return;
        MyEntity entity;
        MyEntities.TryGetEntityById(entityId, out entity);
        if (!(entity is MyVoxelBase myVoxelBase) || myVoxelBase.m_voxelShapeInProgress)
          return;
        myVoxelBase.BeforeContentChanged = true;
        MyMultiplayer.RaiseEvent<MyVoxelBase, Vector3D, float, byte, MyVoxelBase.OperationType>(myVoxelBase.RootVoxel, (Func<MyVoxelBase, Action<Vector3D, float, byte, MyVoxelBase.OperationType>>) (x => new Action<Vector3D, float, byte, MyVoxelBase.OperationType>(x.PerformVoxelOperationSphere_Implementation)), center, radius, material, Type);
        myVoxelBase.UpdateVoxelShape(Type, (MyShape) MyVoxelBase.m_sphereShape, material);
      }
    }

    [Event(null, 852)]
    [Reliable]
    [Broadcast]
    [RefreshReplicable]
    private void PerformVoxelOperationSphere_Implementation(
      Vector3D center,
      float radius,
      byte material,
      MyVoxelBase.OperationType Type)
    {
      MyVoxelBase.m_sphereShape.Center = center;
      MyVoxelBase.m_sphereShape.Radius = radius;
      this.BeforeContentChanged = true;
      this.UpdateVoxelShape(Type, (MyShape) MyVoxelBase.m_sphereShape, material);
    }

    public void RequestVoxelOperationBox(
      BoundingBoxD box,
      MatrixD Transformation,
      byte material,
      MyVoxelBase.OperationType Type)
    {
      MyMultiplayer.RaiseStaticEvent<long, BoundingBoxD, MatrixD, byte, MyVoxelBase.OperationType>((Func<IMyEventOwner, Action<long, BoundingBoxD, MatrixD, byte, MyVoxelBase.OperationType>>) (s => new Action<long, BoundingBoxD, MatrixD, byte, MyVoxelBase.OperationType>(MyVoxelBase.VoxelOperationBox_Implementation)), this.EntityId, box, Transformation, material, Type);
    }

    [Event(null, 869)]
    [Reliable]
    [Server]
    [RefreshReplicable]
    private static void VoxelOperationBox_Implementation(
      long entityId,
      BoundingBoxD box,
      MatrixD Transformation,
      byte material,
      MyVoxelBase.OperationType Type)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.GetVoxelHandAvailable(MyEventContext.Current.Sender.Value))
      {
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        MyVoxelBase.m_boxShape.Transformation = Transformation;
        MyVoxelBase.m_boxShape.Boundaries.Max = box.Max;
        MyVoxelBase.m_boxShape.Boundaries.Min = box.Min;
        if (!MyVoxelBase.CanPlaceInArea(Type, (MyShape) MyVoxelBase.m_boxShape))
          return;
        MyEntity entity;
        MyEntities.TryGetEntityById(entityId, out entity);
        if (!(entity is MyVoxelBase myVoxelBase) || myVoxelBase.m_voxelShapeInProgress)
          return;
        myVoxelBase.BeforeContentChanged = true;
        MyMultiplayer.RaiseEvent<MyVoxelBase, BoundingBoxD, MatrixD, byte, MyVoxelBase.OperationType>(myVoxelBase.RootVoxel, (Func<MyVoxelBase, Action<BoundingBoxD, MatrixD, byte, MyVoxelBase.OperationType>>) (x => new Action<BoundingBoxD, MatrixD, byte, MyVoxelBase.OperationType>(x.PerformVoxelOperationBox_Implementation)), box, Transformation, material, Type);
        myVoxelBase.UpdateVoxelShape(Type, (MyShape) MyVoxelBase.m_boxShape, material);
      }
    }

    [Event(null, 899)]
    [Reliable]
    [Broadcast]
    private void PerformVoxelOperationBox_Implementation(
      BoundingBoxD box,
      MatrixD Transformation,
      byte material,
      MyVoxelBase.OperationType Type)
    {
      this.BeforeContentChanged = true;
      MyVoxelBase.m_boxShape.Transformation = Transformation;
      MyVoxelBase.m_boxShape.Boundaries.Max = box.Max;
      MyVoxelBase.m_boxShape.Boundaries.Min = box.Min;
      this.UpdateVoxelShape(Type, (MyShape) MyVoxelBase.m_boxShape, material);
    }

    public void RequestVoxelOperationRamp(
      BoundingBoxD box,
      Vector3D rampNormal,
      double rampNormalW,
      MatrixD Transformation,
      byte material,
      MyVoxelBase.OperationType Type)
    {
      MyMultiplayer.RaiseStaticEvent<long, MyVoxelBase.MyRampShapeParams, MyVoxelBase.OperationType>((Func<IMyEventOwner, Action<long, MyVoxelBase.MyRampShapeParams, MyVoxelBase.OperationType>>) (s => new Action<long, MyVoxelBase.MyRampShapeParams, MyVoxelBase.OperationType>(MyVoxelBase.VoxelOperationRamp_Implementation)), this.EntityId, new MyVoxelBase.MyRampShapeParams()
      {
        Box = box,
        RampNormal = rampNormal,
        RampNormalW = rampNormalW,
        Transformation = Transformation,
        Material = material
      }, Type);
    }

    [Event(null, 923)]
    [Reliable]
    [Server]
    [RefreshReplicable]
    private static void VoxelOperationRamp_Implementation(
      long entityId,
      MyVoxelBase.MyRampShapeParams shapeParams,
      MyVoxelBase.OperationType Type)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.GetVoxelHandAvailable(MyEventContext.Current.Sender.Value))
      {
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        MyVoxelBase.m_rampShape.Transformation = shapeParams.Transformation;
        MyVoxelBase.m_rampShape.Boundaries.Max = shapeParams.Box.Max;
        MyVoxelBase.m_rampShape.Boundaries.Min = shapeParams.Box.Min;
        MyVoxelBase.m_rampShape.RampNormal = shapeParams.RampNormal;
        MyVoxelBase.m_rampShape.RampNormalW = shapeParams.RampNormalW;
        if (!MyVoxelBase.CanPlaceInArea(Type, (MyShape) MyVoxelBase.m_rampShape))
          return;
        MyEntity entity;
        MyEntities.TryGetEntityById(entityId, out entity);
        if (!(entity is MyVoxelBase myVoxelBase) || myVoxelBase.m_voxelShapeInProgress)
          return;
        myVoxelBase.BeforeContentChanged = true;
        MyMultiplayer.RaiseEvent<MyVoxelBase, MyVoxelBase.MyRampShapeParams, MyVoxelBase.OperationType>(myVoxelBase.RootVoxel, (Func<MyVoxelBase, Action<MyVoxelBase.MyRampShapeParams, MyVoxelBase.OperationType>>) (x => new Action<MyVoxelBase.MyRampShapeParams, MyVoxelBase.OperationType>(x.PerformVoxelOperationRamp_Implementation)), shapeParams, Type);
        myVoxelBase.UpdateVoxelShape(Type, (MyShape) MyVoxelBase.m_rampShape, shapeParams.Material);
      }
    }

    [Event(null, 955)]
    [Reliable]
    [Broadcast]
    private void PerformVoxelOperationRamp_Implementation(
      MyVoxelBase.MyRampShapeParams shapeParams,
      MyVoxelBase.OperationType Type)
    {
      this.BeforeContentChanged = true;
      MyVoxelBase.m_rampShape.Transformation = shapeParams.Transformation;
      MyVoxelBase.m_rampShape.Boundaries.Max = shapeParams.Box.Max;
      MyVoxelBase.m_rampShape.Boundaries.Min = shapeParams.Box.Min;
      MyVoxelBase.m_rampShape.RampNormal = shapeParams.RampNormal;
      MyVoxelBase.m_rampShape.RampNormalW = shapeParams.RampNormalW;
      this.UpdateVoxelShape(Type, (MyShape) MyVoxelBase.m_rampShape, shapeParams.Material);
    }

    public void RequestVoxelOperationElipsoid(
      Vector3 radius,
      MatrixD Transformation,
      byte material,
      MyVoxelBase.OperationType Type)
    {
      MyMultiplayer.RaiseStaticEvent<long, Vector3, MatrixD, byte, MyVoxelBase.OperationType>((Func<IMyEventOwner, Action<long, Vector3, MatrixD, byte, MyVoxelBase.OperationType>>) (s => new Action<long, Vector3, MatrixD, byte, MyVoxelBase.OperationType>(MyVoxelBase.VoxelOperationElipsoid_Implementation)), this.EntityId, radius, Transformation, material, Type);
    }

    [Event(null, 974)]
    [Reliable]
    [Server]
    [RefreshReplicable]
    private static void VoxelOperationElipsoid_Implementation(
      long entityId,
      Vector3 radius,
      MatrixD Transformation,
      byte material,
      MyVoxelBase.OperationType Type)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.GetVoxelHandAvailable(MyEventContext.Current.Sender.Value))
      {
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        MyVoxelBase.m_ellipsoidShape.Transformation = Transformation;
        MyVoxelBase.m_ellipsoidShape.Radius = radius;
        if (!MyVoxelBase.CanPlaceInArea(Type, (MyShape) MyVoxelBase.m_ellipsoidShape))
          return;
        MyEntity entity;
        MyEntities.TryGetEntityById(entityId, out entity);
        if (!(entity is MyVoxelBase myVoxelBase) || myVoxelBase.m_voxelShapeInProgress)
          return;
        myVoxelBase.BeforeContentChanged = true;
        MyMultiplayer.RaiseEvent<MyVoxelBase, Vector3, MatrixD, byte, MyVoxelBase.OperationType>(myVoxelBase.RootVoxel, (Func<MyVoxelBase, Action<Vector3, MatrixD, byte, MyVoxelBase.OperationType>>) (x => new Action<Vector3, MatrixD, byte, MyVoxelBase.OperationType>(x.PerformVoxelOperationElipsoid_Implementation)), radius, Transformation, material, Type);
        myVoxelBase.UpdateVoxelShape(Type, (MyShape) MyVoxelBase.m_ellipsoidShape, material);
      }
    }

    [Event(null, 1003)]
    [Reliable]
    [Broadcast]
    private void PerformVoxelOperationElipsoid_Implementation(
      Vector3 radius,
      MatrixD Transformation,
      byte material,
      MyVoxelBase.OperationType Type)
    {
      this.BeforeContentChanged = true;
      MyVoxelBase.m_ellipsoidShape.Transformation = Transformation;
      MyVoxelBase.m_ellipsoidShape.Radius = radius;
      this.UpdateVoxelShape(Type, (MyShape) MyVoxelBase.m_ellipsoidShape, material);
    }

    [Event(null, 1013)]
    [Reliable]
    [ServerInvoked]
    [BroadcastExcept]
    public void RevertVoxelAccess(Vector3I key, MyStorageDataTypeFlags flags)
    {
      if (this.Storage == null || !(this.Storage is MyStorageBase storage))
        return;
      storage.AccessDelete(ref key, flags);
    }

    [Event(null, 1027)]
    [Reliable]
    [Broadcast]
    public void PerformCutOutSphereFast(Vector3D center, float radius, bool notify) => MyVoxelGenerator.CutOutSphereFast(this, ref center, radius, out Vector3I _, out Vector3I _, notify);

    public void BroadcastShipCutout(in MyShipMiningSystem.NetworkCutoutData data)
    {
      if (!Sync.IsServer)
        return;
      MyMultiplayer.RaiseEvent<MyVoxelBase, MyShipMiningSystem.NetworkCutoutData>(this, (Func<MyVoxelBase, Action<MyShipMiningSystem.NetworkCutoutData>>) (x => new Action<MyShipMiningSystem.NetworkCutoutData>(x.PerformShipCutout)), data);
    }

    [Event(null, 1045)]
    [Reliable]
    [Broadcast]
    private void PerformShipCutout(MyShipMiningSystem.NetworkCutoutData data) => MyShipMiningSystem.PerformCutoutClient(this, in data);

    public void CutOutShapeWithProperties(
      MyShape shape,
      out float voxelsCountInPercent,
      out MyVoxelMaterialDefinition voxelMaterial,
      Dictionary<MyVoxelMaterialDefinition, int> exactCutOutMaterials = null,
      bool updateSync = false,
      bool onlyCheck = false,
      bool applyDamageMaterial = false,
      bool onlyApplyMaterial = false)
    {
      this.BeforeContentChanged = true;
      MyVoxelGenerator.CutOutShapeWithProperties(this, shape, out voxelsCountInPercent, out voxelMaterial, exactCutOutMaterials, updateSync, onlyCheck, applyDamageMaterial, onlyApplyMaterial);
    }

    public void CutOutShapeWithPropertiesAsync(
      MyVoxelBase.OnCutOutResults results,
      MyShape shape,
      bool updateSync = false,
      bool onlyCheck = false,
      bool applyDamageMaterial = false,
      bool onlyApplyMaterial = false,
      bool skipCache = true)
    {
      this.BeforeContentChanged = true;
      float voxelsCountInPercent = 0.0f;
      MyVoxelMaterialDefinition voxelMaterial = (MyVoxelMaterialDefinition) null;
      Dictionary<MyVoxelMaterialDefinition, int> exactCutOutMaterials = new Dictionary<MyVoxelMaterialDefinition, int>();
      Parallel.Start((Action) (() =>
      {
        using (this.Pin())
        {
          if (this.MarkedForClose)
            return;
          MyVoxelGenerator.CutOutShapeWithProperties(this, shape, out voxelsCountInPercent, out voxelMaterial, exactCutOutMaterials, updateSync, onlyCheck, applyDamageMaterial, onlyApplyMaterial, skipCache);
        }
      }), (Action) (() =>
      {
        if (results == null)
          return;
        results(voxelsCountInPercent, voxelMaterial, exactCutOutMaterials);
      }));
    }

    private static bool CanPlaceInArea(MyVoxelBase.OperationType type, MyShape Shape)
    {
      if (type == MyVoxelBase.OperationType.Fill || type == MyVoxelBase.OperationType.Revert)
      {
        MyVoxelBase.m_foundElements.Clear();
        BoundingBoxD worldBoundaries = Shape.GetWorldBoundaries();
        MyEntities.GetElementsInBox(ref worldBoundaries, MyVoxelBase.m_foundElements);
        foreach (MyEntity foundElement in MyVoxelBase.m_foundElements)
        {
          if (MyVoxelBase.IsForbiddenEntity(foundElement) && foundElement.PositionComp.WorldAABB.Intersects(worldBoundaries))
            return false;
        }
      }
      return true;
    }

    public static bool IsForbiddenEntity(MyEntity entity)
    {
      MyCubeGrid myCubeGrid = entity as MyCubeGrid;
      if (entity is MyCharacter || myCubeGrid != null && !myCubeGrid.IsStatic && !myCubeGrid.IsPreview)
        return true;
      return entity is MyCockpit && (entity as MyCockpit).Pilot != null;
    }

    private void UpdateVoxelShape(MyVoxelBase.OperationType type, MyShape shape, byte Material)
    {
      MyShape localShape = shape.Clone();
      this.m_voxelShapeInProgress = true;
      switch (type)
      {
        case MyVoxelBase.OperationType.Fill:
          if (MyFakes.VOXELHAND_PARALLEL)
          {
            Parallel.Start((Action) (() => MyVoxelGenerator.FillInShape(this, localShape, Material)), (Action) (() => this.m_voxelShapeInProgress = false));
            break;
          }
          MyVoxelGenerator.FillInShape(this, localShape, Material);
          break;
        case MyVoxelBase.OperationType.Paint:
          if (MyFakes.VOXELHAND_PARALLEL)
          {
            Parallel.Start((Action) (() => MyVoxelGenerator.PaintInShape(this, localShape, Material)), (Action) (() => this.m_voxelShapeInProgress = false));
            break;
          }
          MyVoxelGenerator.PaintInShape(this, localShape, Material);
          break;
        case MyVoxelBase.OperationType.Cut:
          if (MyFakes.VOXELHAND_PARALLEL)
          {
            Parallel.Start((Action) (() => MyVoxelGenerator.CutOutShape(this, localShape, true)), (Action) (() => this.m_voxelShapeInProgress = false));
            break;
          }
          MyVoxelGenerator.CutOutShape(this, localShape, true);
          break;
        case MyVoxelBase.OperationType.Revert:
          if (MyFakes.VOXELHAND_PARALLEL)
          {
            Parallel.Start((Action) (() => MyVoxelGenerator.RevertShape(this, localShape)), (Action) (() => this.m_voxelShapeInProgress = false));
            break;
          }
          MyVoxelGenerator.RevertShape(this, localShape);
          break;
        default:
          this.m_voxelShapeInProgress = false;
          break;
      }
    }

    public void CreateVoxelMeteorCrater(
      Vector3D center,
      float radius,
      Vector3 direction,
      MyVoxelMaterialDefinition material)
    {
      this.BeforeContentChanged = true;
      MyMultiplayer.RaiseEvent<MyVoxelBase, Vector3D, float, Vector3, byte>(this.RootVoxel, (Func<MyVoxelBase, Action<Vector3D, float, Vector3, byte>>) (x => new Action<Vector3D, float, Vector3, byte>(x.CreateVoxelMeteorCrater_Implementation)), center, radius, direction, material.Index);
    }

    [Event(null, 1189)]
    [Reliable]
    [Broadcast]
    private void CreateVoxelMeteorCrater_Implementation(
      Vector3D center,
      float radius,
      Vector3 direction,
      byte material)
    {
      this.BeforeContentChanged = true;
      MyVoxelGenerator.MakeCrater(this, (BoundingSphereD) new BoundingSphere((Vector3) center, radius), direction, MyDefinitionManager.Static.GetVoxelMaterialDefinition(material));
    }

    public void GetFilledStorageBounds(out Vector3I min, out Vector3I max)
    {
      min = Vector3I.MaxValue;
      max = Vector3I.MinValue;
      Vector3I size = this.Size;
      Vector3I lodVoxelRangeMax = this.Size - 1;
      if (MyVoxelBase.m_tempStorage == null)
        MyVoxelBase.m_tempStorage = new MyStorageData();
      MyVoxelBase.m_tempStorage.Resize(this.Size);
      this.Storage.ReadRange(MyVoxelBase.m_tempStorage, MyStorageDataTypeFlags.Content, 0, Vector3I.Zero, lodVoxelRangeMax);
      for (int z = 0; z < size.Z; ++z)
      {
        for (int y = 0; y < size.Y; ++y)
        {
          for (int x = 0; x < size.X; ++x)
          {
            if (MyVoxelBase.m_tempStorage.Content(x, y, z) > (byte) 127)
            {
              Vector3I vector3I1 = Vector3I.Max(new Vector3I(x - 1, y - 1, z - 1), Vector3I.Zero);
              min = Vector3I.Min(vector3I1, min);
              Vector3I vector3I2 = Vector3I.Min(new Vector3I(x + 1, y + 1, z + 1), lodVoxelRangeMax);
              max = Vector3I.Max(vector3I2, max);
            }
          }
        }
      }
    }

    public MyVoxelContentConstitution GetVoxelRangeTypeInBoundingBox(
      BoundingBoxD worldAabb)
    {
      Vector3I voxelCoord1;
      MyVoxelCoordSystems.WorldPositionToVoxelCoord(this.PositionLeftBottomCorner, ref worldAabb.Min, out voxelCoord1);
      Vector3I voxelCoord2;
      MyVoxelCoordSystems.WorldPositionToVoxelCoord(this.PositionLeftBottomCorner, ref worldAabb.Max, out voxelCoord2);
      Vector3I voxelCoord3 = voxelCoord1 + this.StorageMin;
      Vector3I voxelCoord4 = voxelCoord2 + this.StorageMin;
      this.Storage.ClampVoxelCoord(ref voxelCoord3);
      this.Storage.ClampVoxelCoord(ref voxelCoord4);
      return MyVoxelContentConstitution.Mixed;
    }

    public HashSet<byte> GetMaterialsInShape(MyShape shape, int lod = 0)
    {
      BoundingBoxD worldBoundaries = shape.GetWorldBoundaries();
      Vector3I voxelCoord1;
      MyVoxelCoordSystems.WorldPositionToVoxelCoord(this.PositionLeftBottomCorner, ref worldBoundaries.Min, out voxelCoord1);
      Vector3I voxelCoord2;
      MyVoxelCoordSystems.WorldPositionToVoxelCoord(this.PositionLeftBottomCorner, ref worldBoundaries.Max, out voxelCoord2);
      Vector3I voxelCoord3 = voxelCoord1 - 1;
      Vector3I voxelCoord4 = voxelCoord2 + 1;
      this.Storage.ClampVoxelCoord(ref voxelCoord3);
      this.Storage.ClampVoxelCoord(ref voxelCoord4);
      Vector3I vector3I1 = (voxelCoord3 >> lod) - 1;
      Vector3I vector3I2 = (voxelCoord4 >> lod) + 1;
      if (MyVoxelBase.m_tempStorage == null)
        MyVoxelBase.m_tempStorage = new MyStorageData();
      MyVoxelBase.m_tempStorage.Resize(vector3I1, vector3I2);
      using (this.Storage.Pin())
        this.Storage.ReadRange(MyVoxelBase.m_tempStorage, MyStorageDataTypeFlags.Material, lod, vector3I1, vector3I2);
      HashSet<byte> byteSet = new HashSet<byte>();
      Vector3I vector3I3;
      for (vector3I3.X = vector3I1.X; vector3I3.X <= vector3I2.X; ++vector3I3.X)
      {
        for (vector3I3.Y = vector3I1.Y; vector3I3.Y <= vector3I2.Y; ++vector3I3.Y)
        {
          for (vector3I3.Z = vector3I1.Z; vector3I3.Z <= vector3I2.Z; ++vector3I3.Z)
          {
            Vector3I p = vector3I3 - vector3I1;
            int linear = MyVoxelBase.m_tempStorage.ComputeLinear(ref p);
            byte num = MyVoxelBase.m_tempStorage.Material(linear);
            if (num != byte.MaxValue)
              byteSet.Add(num);
          }
        }
      }
      return byteSet;
    }

    public ContainmentType IntersectStorage(ref BoundingBox box, bool lazy = true)
    {
      box.Transform(this.PositionComp.WorldMatrixInvScaled);
      box.Translate(this.SizeInMetresHalf + (Vector3) this.StorageMin);
      return this.Storage.Intersect(ref box, lazy);
    }

    public virtual Vector3D FindOutsidePosition(Vector3D localPosition, float radius)
    {
      Vector3D totalGravityInPoint = (Vector3D) MyGravityProviderSystem.CalculateTotalGravityInPoint(Vector3D.Transform(localPosition - this.SizeInMetresHalf, this.WorldMatrix));
      Vector3D vector3D1;
      if (totalGravityInPoint.LengthSquared() > 0.01)
      {
        Vector3D axis = Vector3D.TransformNormal(totalGravityInPoint, this.PositionComp.WorldMatrixNormalizedInv);
        axis.Normalize();
        vector3D1 = MyUtils.GetRandomPerpendicularVector(ref axis);
      }
      else
      {
        vector3D1 = localPosition - this.SizeInMetresHalf;
        vector3D1.Normalize();
      }
      double num = (double) radius;
      Vector3D vector3D2;
      while (this.OverlapsSphereLocal(radius, vector3D2 = localPosition + vector3D1 * num))
        num *= 2.0;
      return vector3D2;
    }

    public override void OnReplicationStarted()
    {
      base.OnReplicationStarted();
      MySession.Static.GetComponent<MyPhysics>()?.InformReplicationStarted((MyEntity) this);
    }

    public override void OnReplicationEnded()
    {
      base.OnReplicationEnded();
      MySession.Static.GetComponent<MyPhysics>()?.InformReplicationEnded((MyEntity) this);
    }

    [Serializable]
    private struct MyRampShapeParams
    {
      public BoundingBoxD Box;
      public Vector3D RampNormal;
      public double RampNormalW;
      public MatrixD Transformation;
      public byte Material;

      protected class Sandbox_Game_Entities_MyVoxelBase\u003C\u003EMyRampShapeParams\u003C\u003EBox\u003C\u003EAccessor : IMemberAccessor<MyVoxelBase.MyRampShapeParams, BoundingBoxD>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyVoxelBase.MyRampShapeParams owner, in BoundingBoxD value) => owner.Box = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyVoxelBase.MyRampShapeParams owner, out BoundingBoxD value) => value = owner.Box;
      }

      protected class Sandbox_Game_Entities_MyVoxelBase\u003C\u003EMyRampShapeParams\u003C\u003ERampNormal\u003C\u003EAccessor : IMemberAccessor<MyVoxelBase.MyRampShapeParams, Vector3D>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyVoxelBase.MyRampShapeParams owner, in Vector3D value) => owner.RampNormal = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyVoxelBase.MyRampShapeParams owner, out Vector3D value) => value = owner.RampNormal;
      }

      protected class Sandbox_Game_Entities_MyVoxelBase\u003C\u003EMyRampShapeParams\u003C\u003ERampNormalW\u003C\u003EAccessor : IMemberAccessor<MyVoxelBase.MyRampShapeParams, double>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyVoxelBase.MyRampShapeParams owner, in double value) => owner.RampNormalW = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyVoxelBase.MyRampShapeParams owner, out double value) => value = owner.RampNormalW;
      }

      protected class Sandbox_Game_Entities_MyVoxelBase\u003C\u003EMyRampShapeParams\u003C\u003ETransformation\u003C\u003EAccessor : IMemberAccessor<MyVoxelBase.MyRampShapeParams, MatrixD>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyVoxelBase.MyRampShapeParams owner, in MatrixD value) => owner.Transformation = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyVoxelBase.MyRampShapeParams owner, out MatrixD value) => value = owner.Transformation;
      }

      protected class Sandbox_Game_Entities_MyVoxelBase\u003C\u003EMyRampShapeParams\u003C\u003EMaterial\u003C\u003EAccessor : IMemberAccessor<MyVoxelBase.MyRampShapeParams, byte>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyVoxelBase.MyRampShapeParams owner, in byte value) => owner.Material = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyVoxelBase.MyRampShapeParams owner, out byte value) => value = owner.Material;
      }
    }

    [Serializable]
    private struct MyCapsuleShapeParams
    {
      public Vector3D A;
      public Vector3D B;
      public float Radius;
      public MatrixD Transformation;
      public byte Material;

      protected class Sandbox_Game_Entities_MyVoxelBase\u003C\u003EMyCapsuleShapeParams\u003C\u003EA\u003C\u003EAccessor : IMemberAccessor<MyVoxelBase.MyCapsuleShapeParams, Vector3D>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyVoxelBase.MyCapsuleShapeParams owner, in Vector3D value) => owner.A = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyVoxelBase.MyCapsuleShapeParams owner, out Vector3D value) => value = owner.A;
      }

      protected class Sandbox_Game_Entities_MyVoxelBase\u003C\u003EMyCapsuleShapeParams\u003C\u003EB\u003C\u003EAccessor : IMemberAccessor<MyVoxelBase.MyCapsuleShapeParams, Vector3D>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyVoxelBase.MyCapsuleShapeParams owner, in Vector3D value) => owner.B = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyVoxelBase.MyCapsuleShapeParams owner, out Vector3D value) => value = owner.B;
      }

      protected class Sandbox_Game_Entities_MyVoxelBase\u003C\u003EMyCapsuleShapeParams\u003C\u003ERadius\u003C\u003EAccessor : IMemberAccessor<MyVoxelBase.MyCapsuleShapeParams, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyVoxelBase.MyCapsuleShapeParams owner, in float value) => owner.Radius = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyVoxelBase.MyCapsuleShapeParams owner, out float value) => value = owner.Radius;
      }

      protected class Sandbox_Game_Entities_MyVoxelBase\u003C\u003EMyCapsuleShapeParams\u003C\u003ETransformation\u003C\u003EAccessor : IMemberAccessor<MyVoxelBase.MyCapsuleShapeParams, MatrixD>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyVoxelBase.MyCapsuleShapeParams owner, in MatrixD value) => owner.Transformation = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyVoxelBase.MyCapsuleShapeParams owner, out MatrixD value) => value = owner.Transformation;
      }

      protected class Sandbox_Game_Entities_MyVoxelBase\u003C\u003EMyCapsuleShapeParams\u003C\u003EMaterial\u003C\u003EAccessor : IMemberAccessor<MyVoxelBase.MyCapsuleShapeParams, byte>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyVoxelBase.MyCapsuleShapeParams owner, in byte value) => owner.Material = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyVoxelBase.MyCapsuleShapeParams owner, out byte value) => value = owner.Material;
      }
    }

    public enum OperationType : byte
    {
      Fill,
      Paint,
      Cut,
      Revert,
    }

    public delegate void StorageChanged(
      MyVoxelBase storage,
      Vector3I minVoxelChanged,
      Vector3I maxVoxelChanged,
      MyStorageDataTypeFlags changedData);

    public delegate void OnCutOutResults(
      float voxelsCountInPercent,
      MyVoxelMaterialDefinition voxelMaterial,
      Dictionary<MyVoxelMaterialDefinition, int> exactCutOutMaterials);

    protected sealed class VoxelCutoutSphere_Implementation\u003C\u003EVRageMath_Vector3D\u0023System_Single\u0023System_Boolean\u0023System_Boolean : ICallSite<MyVoxelBase, Vector3D, float, bool, bool, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyVoxelBase @this,
        in Vector3D center,
        in float radius,
        in bool createDebris,
        in bool damage,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.VoxelCutoutSphere_Implementation(center, radius, createDebris, damage);
      }
    }

    protected sealed class VoxelOperationCapsule_Implementation\u003C\u003ESystem_Int64\u0023Sandbox_Game_Entities_MyVoxelBase\u003C\u003EMyCapsuleShapeParams\u0023Sandbox_Game_Entities_MyVoxelBase\u003C\u003EOperationType : ICallSite<IMyEventOwner, long, MyVoxelBase.MyCapsuleShapeParams, MyVoxelBase.OperationType, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in MyVoxelBase.MyCapsuleShapeParams capsuleParams,
        in MyVoxelBase.OperationType Type,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVoxelBase.VoxelOperationCapsule_Implementation(entityId, capsuleParams, Type);
      }
    }

    protected sealed class PerformVoxelOperationCapsule_Implementation\u003C\u003ESandbox_Game_Entities_MyVoxelBase\u003C\u003EMyCapsuleShapeParams\u0023Sandbox_Game_Entities_MyVoxelBase\u003C\u003EOperationType : ICallSite<MyVoxelBase, MyVoxelBase.MyCapsuleShapeParams, MyVoxelBase.OperationType, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyVoxelBase @this,
        in MyVoxelBase.MyCapsuleShapeParams capsuleParams,
        in MyVoxelBase.OperationType Type,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.PerformVoxelOperationCapsule_Implementation(capsuleParams, Type);
      }
    }

    protected sealed class VoxelOperationSphere_Implementation\u003C\u003ESystem_Int64\u0023VRageMath_Vector3D\u0023System_Single\u0023System_Byte\u0023Sandbox_Game_Entities_MyVoxelBase\u003C\u003EOperationType : ICallSite<IMyEventOwner, long, Vector3D, float, byte, MyVoxelBase.OperationType, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in Vector3D center,
        in float radius,
        in byte material,
        in MyVoxelBase.OperationType Type,
        in DBNull arg6)
      {
        MyVoxelBase.VoxelOperationSphere_Implementation(entityId, center, radius, material, Type);
      }
    }

    protected sealed class PerformVoxelOperationSphere_Implementation\u003C\u003EVRageMath_Vector3D\u0023System_Single\u0023System_Byte\u0023Sandbox_Game_Entities_MyVoxelBase\u003C\u003EOperationType : ICallSite<MyVoxelBase, Vector3D, float, byte, MyVoxelBase.OperationType, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyVoxelBase @this,
        in Vector3D center,
        in float radius,
        in byte material,
        in MyVoxelBase.OperationType Type,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.PerformVoxelOperationSphere_Implementation(center, radius, material, Type);
      }
    }

    protected sealed class VoxelOperationBox_Implementation\u003C\u003ESystem_Int64\u0023VRageMath_BoundingBoxD\u0023VRageMath_MatrixD\u0023System_Byte\u0023Sandbox_Game_Entities_MyVoxelBase\u003C\u003EOperationType : ICallSite<IMyEventOwner, long, BoundingBoxD, MatrixD, byte, MyVoxelBase.OperationType, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in BoundingBoxD box,
        in MatrixD Transformation,
        in byte material,
        in MyVoxelBase.OperationType Type,
        in DBNull arg6)
      {
        MyVoxelBase.VoxelOperationBox_Implementation(entityId, box, Transformation, material, Type);
      }
    }

    protected sealed class PerformVoxelOperationBox_Implementation\u003C\u003EVRageMath_BoundingBoxD\u0023VRageMath_MatrixD\u0023System_Byte\u0023Sandbox_Game_Entities_MyVoxelBase\u003C\u003EOperationType : ICallSite<MyVoxelBase, BoundingBoxD, MatrixD, byte, MyVoxelBase.OperationType, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyVoxelBase @this,
        in BoundingBoxD box,
        in MatrixD Transformation,
        in byte material,
        in MyVoxelBase.OperationType Type,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.PerformVoxelOperationBox_Implementation(box, Transformation, material, Type);
      }
    }

    protected sealed class VoxelOperationRamp_Implementation\u003C\u003ESystem_Int64\u0023Sandbox_Game_Entities_MyVoxelBase\u003C\u003EMyRampShapeParams\u0023Sandbox_Game_Entities_MyVoxelBase\u003C\u003EOperationType : ICallSite<IMyEventOwner, long, MyVoxelBase.MyRampShapeParams, MyVoxelBase.OperationType, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in MyVoxelBase.MyRampShapeParams shapeParams,
        in MyVoxelBase.OperationType Type,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVoxelBase.VoxelOperationRamp_Implementation(entityId, shapeParams, Type);
      }
    }

    protected sealed class PerformVoxelOperationRamp_Implementation\u003C\u003ESandbox_Game_Entities_MyVoxelBase\u003C\u003EMyRampShapeParams\u0023Sandbox_Game_Entities_MyVoxelBase\u003C\u003EOperationType : ICallSite<MyVoxelBase, MyVoxelBase.MyRampShapeParams, MyVoxelBase.OperationType, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyVoxelBase @this,
        in MyVoxelBase.MyRampShapeParams shapeParams,
        in MyVoxelBase.OperationType Type,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.PerformVoxelOperationRamp_Implementation(shapeParams, Type);
      }
    }

    protected sealed class VoxelOperationElipsoid_Implementation\u003C\u003ESystem_Int64\u0023VRageMath_Vector3\u0023VRageMath_MatrixD\u0023System_Byte\u0023Sandbox_Game_Entities_MyVoxelBase\u003C\u003EOperationType : ICallSite<IMyEventOwner, long, Vector3, MatrixD, byte, MyVoxelBase.OperationType, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in Vector3 radius,
        in MatrixD Transformation,
        in byte material,
        in MyVoxelBase.OperationType Type,
        in DBNull arg6)
      {
        MyVoxelBase.VoxelOperationElipsoid_Implementation(entityId, radius, Transformation, material, Type);
      }
    }

    protected sealed class PerformVoxelOperationElipsoid_Implementation\u003C\u003EVRageMath_Vector3\u0023VRageMath_MatrixD\u0023System_Byte\u0023Sandbox_Game_Entities_MyVoxelBase\u003C\u003EOperationType : ICallSite<MyVoxelBase, Vector3, MatrixD, byte, MyVoxelBase.OperationType, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyVoxelBase @this,
        in Vector3 radius,
        in MatrixD Transformation,
        in byte material,
        in MyVoxelBase.OperationType Type,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.PerformVoxelOperationElipsoid_Implementation(radius, Transformation, material, Type);
      }
    }

    protected sealed class RevertVoxelAccess\u003C\u003EVRageMath_Vector3I\u0023VRage_Voxels_MyStorageDataTypeFlags : ICallSite<MyVoxelBase, Vector3I, MyStorageDataTypeFlags, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyVoxelBase @this,
        in Vector3I key,
        in MyStorageDataTypeFlags flags,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.RevertVoxelAccess(key, flags);
      }
    }

    protected sealed class PerformCutOutSphereFast\u003C\u003EVRageMath_Vector3D\u0023System_Single\u0023System_Boolean : ICallSite<MyVoxelBase, Vector3D, float, bool, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyVoxelBase @this,
        in Vector3D center,
        in float radius,
        in bool notify,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.PerformCutOutSphereFast(center, radius, notify);
      }
    }

    protected sealed class PerformShipCutout\u003C\u003ESandbox_Game_GameSystems_MyShipMiningSystem\u003C\u003ENetworkCutoutData : ICallSite<MyVoxelBase, MyShipMiningSystem.NetworkCutoutData, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyVoxelBase @this,
        in MyShipMiningSystem.NetworkCutoutData data,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.PerformShipCutout(data);
      }
    }

    protected sealed class CreateVoxelMeteorCrater_Implementation\u003C\u003EVRageMath_Vector3D\u0023System_Single\u0023VRageMath_Vector3\u0023System_Byte : ICallSite<MyVoxelBase, Vector3D, float, Vector3, byte, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyVoxelBase @this,
        in Vector3D center,
        in float radius,
        in Vector3 direction,
        in byte material,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.CreateVoxelMeteorCrater_Implementation(center, radius, direction, material);
      }
    }
  }
}
