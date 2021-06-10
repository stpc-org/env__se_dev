// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyVoxelMap
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Components;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.Game.World;
using System;
using System.Text;
using System.Text.RegularExpressions;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.Entities
{
  [MyEntityType(typeof (MyObjectBuilder_VoxelMap), true)]
  public class MyVoxelMap : MyVoxelBase, IMyVoxelMap, IMyVoxelBase, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity
  {
    public override VRage.Game.Voxels.IMyStorage Storage
    {
      get => this.m_storage;
      set
      {
        bool flag = false;
        if (this.m_storage != null)
        {
          flag = true;
          this.m_storage.RangeChanged -= new Action<Vector3I, Vector3I, MyStorageDataTypeFlags>(this.storage_RangeChanged);
        }
        this.m_storage = value;
        this.m_storage.RangeChanged += new Action<Vector3I, Vector3I, MyStorageDataTypeFlags>(this.storage_RangeChanged);
        this.m_storageMax = this.m_storage.Size;
        if (!flag)
          return;
        this.m_storage.NotifyChanged(this.m_storageMin, this.m_storageMax, MyStorageDataTypeFlags.ContentAndMaterial);
      }
    }

    internal MyVoxelPhysicsBody Physics
    {
      get => base.Physics as MyVoxelPhysicsBody;
      set => this.Physics = (MyPhysicsComponentBase) value;
    }

    public override MyVoxelBase RootVoxel => (MyVoxelBase) this;

    public MyVoxelMap()
    {
      ((MyPositionComponent) this.PositionComp).WorldPositionChanged = new Action<object>(((MyVoxelBase) this).WorldPositionChanged);
      this.Render = (MyRenderComponentBase) new MyRenderComponentVoxelMap();
      this.Render.DrawOutsideViewDistance = true;
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentVoxelMap((MyVoxelBase) this));
    }

    public override void Init(MyObjectBuilder_EntityBase builder)
    {
      MyObjectBuilder_VoxelMap objectBuilderVoxelMap = (MyObjectBuilder_VoxelMap) builder;
      if (objectBuilderVoxelMap == null)
        return;
      this.m_storage = (VRage.Game.Voxels.IMyStorage) MyStorageBase.Load(objectBuilderVoxelMap.StorageName, false);
      if (this.m_storage == null)
        return;
      this.Init(builder, this.m_storage);
      if (objectBuilderVoxelMap.ContentChanged.HasValue)
        this.ContentChanged = objectBuilderVoxelMap.ContentChanged.Value;
      else
        this.ContentChanged = true;
    }

    public override void Init(MyObjectBuilder_EntityBase builder, VRage.Game.Voxels.IMyStorage storage)
    {
      this.SyncFlag = true;
      base.Init(builder);
      this.Init((StringBuilder) null, (string) null, (MyEntity) null, new float?());
      MyObjectBuilder_VoxelMap objectBuilderVoxelMap = (MyObjectBuilder_VoxelMap) builder;
      if (objectBuilderVoxelMap == null)
        return;
      if (objectBuilderVoxelMap.MutableStorage)
        this.StorageName = objectBuilderVoxelMap.StorageName;
      else
        this.StorageName = MyVoxelMap.GetNewStorageName(objectBuilderVoxelMap.StorageName, this.EntityId);
      this.m_storage = storage;
      this.m_storage.RangeChanged += new Action<Vector3I, Vector3I, MyStorageDataTypeFlags>(this.storage_RangeChanged);
      this.m_storageMax = this.m_storage.Size;
      this.CreatedByUser = objectBuilderVoxelMap.CreatedByUser;
      if (objectBuilderVoxelMap != null && objectBuilderVoxelMap.BoulderItemId.HasValue && (objectBuilderVoxelMap.BoulderSectorId.HasValue && objectBuilderVoxelMap.BoulderPlanetId.HasValue))
        this.BoulderInfo = new MyBoulderInformation?(new MyBoulderInformation()
        {
          PlanetId = objectBuilderVoxelMap.BoulderPlanetId.Value,
          SectorId = objectBuilderVoxelMap.BoulderSectorId.Value,
          ItemId = objectBuilderVoxelMap.BoulderItemId.Value
        });
      else if (MyFakes.ENABLE_BOULDER_NAME_PARSING && objectBuilderVoxelMap != null && !string.IsNullOrEmpty(objectBuilderVoxelMap.StorageName))
      {
        Match match = new Regex("P\\((.*)\\)S\\((\\d+)\\)A\\((.*)__(\\d+)\\)").Match(objectBuilderVoxelMap.StorageName);
        if (match.Success)
        {
          GroupCollection groups = match.Groups;
          if (groups.Count >= 4)
          {
            bool flag = true;
            MyBoulderInformation boulderInformation = new MyBoulderInformation();
            if (MyEntities.GetEntityByName(groups[1].Value) is MyPlanet entityByName)
              boulderInformation.PlanetId = entityByName.EntityId;
            else
              flag = false;
            boulderInformation.SectorId = long.Parse(groups[2].Value);
            boulderInformation.ItemId = int.Parse(groups[4].Value);
            if (flag)
              this.BoulderInfo = new MyBoulderInformation?(boulderInformation);
          }
        }
      }
      else
        this.BoulderInfo = new MyBoulderInformation?();
      Vector3D position = (Vector3D) objectBuilderVoxelMap.PositionAndOrientation.Value.Position + Vector3D.TransformNormal((Vector3D) this.m_storage.Size / 2.0, this.WorldMatrix);
      MatrixD worldMatrix = this.WorldMatrix;
      Vector3D forward = worldMatrix.Forward;
      worldMatrix = this.WorldMatrix;
      Vector3D up = worldMatrix.Up;
      this.InitVoxelMap(MatrixD.CreateWorld(position, forward, up), this.m_storage.Size, true);
    }

    public static string GetNewStorageName(string storageName, long entityId) => string.Format("{0}-{1}", (object) storageName, (object) entityId);

    public override void UpdateOnceBeforeFrame() => base.UpdateOnceBeforeFrame();

    public override bool IsOverlapOverThreshold(BoundingBoxD worldAabb, float thresholdPercentage)
    {
      if (this.m_storage == null)
      {
        if (MyEntities.GetEntityByIdOrDefault(this.EntityId) != this)
          MyDebug.FailRelease("Voxel map was deleted!", "E:\\Repo3\\Sources\\Sandbox.Game\\Game\\Entities\\MyVoxelMap.cs", 180);
        else
          MyDebug.FailRelease("Voxel map is still in world but has null storage!", "E:\\Repo3\\Sources\\Sandbox.Game\\Game\\Entities\\MyVoxelMap.cs", 182);
        return false;
      }
      Vector3I voxelCoord1;
      MyVoxelCoordSystems.WorldPositionToVoxelCoord(this.PositionLeftBottomCorner, ref worldAabb.Min, out voxelCoord1);
      Vector3I voxelCoord2;
      MyVoxelCoordSystems.WorldPositionToVoxelCoord(this.PositionLeftBottomCorner, ref worldAabb.Max, out voxelCoord2);
      Vector3I voxelCoord3 = voxelCoord1 + this.StorageMin;
      Vector3I voxelCoord4 = voxelCoord2 + this.StorageMin;
      this.Storage.ClampVoxelCoord(ref voxelCoord3);
      this.Storage.ClampVoxelCoord(ref voxelCoord4);
      if (MyVoxelBase.m_tempStorage == null)
        MyVoxelBase.m_tempStorage = new MyStorageData();
      MyVoxelBase.m_tempStorage.Resize(voxelCoord3, voxelCoord4);
      this.Storage.ReadRange(MyVoxelBase.m_tempStorage, MyStorageDataTypeFlags.Content, 0, voxelCoord3, voxelCoord4);
      double num1 = 1.0 / (double) byte.MaxValue;
      double num2 = 1.0;
      double num3 = 0.0;
      double volume1 = worldAabb.Volume;
      Vector3I voxelCoord5;
      voxelCoord5.Z = voxelCoord3.Z;
      Vector3I p;
      for (p.Z = 0; voxelCoord5.Z <= voxelCoord4.Z; ++p.Z)
      {
        voxelCoord5.Y = voxelCoord3.Y;
        for (p.Y = 0; voxelCoord5.Y <= voxelCoord4.Y; ++p.Y)
        {
          voxelCoord5.X = voxelCoord3.X;
          for (p.X = 0; voxelCoord5.X <= voxelCoord4.X; ++p.X)
          {
            BoundingBoxD worldAABB;
            MyVoxelCoordSystems.VoxelCoordToWorldAABB(this.PositionLeftBottomCorner, ref voxelCoord5, out worldAABB);
            if (worldAabb.Intersects(worldAABB))
            {
              double num4 = (double) MyVoxelBase.m_tempStorage.Content(ref p) * num1 * num2;
              double volume2 = worldAabb.Intersect(worldAABB).Volume;
              num3 += num4 * volume2;
            }
            ++voxelCoord5.X;
          }
          ++voxelCoord5.Y;
        }
        ++voxelCoord5.Z;
      }
      return num3 / volume1 >= (double) thresholdPercentage;
    }

    public override bool GetIntersectionWithSphere(ref BoundingSphereD sphere)
    {
      try
      {
        if (!this.PositionComp.WorldAABB.Intersects(ref sphere))
          return false;
        BoundingSphere localSphere = new BoundingSphere((Vector3) (sphere.Center - this.PositionLeftBottomCorner), (float) sphere.Radius);
        return this.Storage.GetGeometry().Intersects(ref localSphere);
      }
      finally
      {
      }
    }

    public override bool GetIntersectionWithAABB(ref BoundingBoxD aabb)
    {
      try
      {
        if (!this.PositionComp.WorldAABB.Intersects(ref aabb))
          return false;
        BoundingSphere localSphere = new BoundingSphere((Vector3) (aabb.Center - this.PositionLeftBottomCorner), (float) aabb.HalfExtents.Length());
        return this.Storage.GetGeometry().Intersects(ref localSphere);
      }
      finally
      {
      }
    }

    public override void UpdateBeforeSimulation10()
    {
      base.UpdateBeforeSimulation10();
      if (this.Physics == null)
        return;
      this.Physics.UpdateBeforeSimulation10();
    }

    public override void UpdateAfterSimulation10()
    {
      base.UpdateAfterSimulation10();
      if (this.Physics == null)
        return;
      this.Physics.UpdateAfterSimulation10();
    }

    protected override void BeforeDelete()
    {
      base.BeforeDelete();
      this.m_storage = (VRage.Game.Voxels.IMyStorage) null;
      MySession.Static.VoxelMaps.RemoveVoxelMap((MyVoxelBase) this);
    }

    private void storage_RangeChanged(
      Vector3I minChanged,
      Vector3I maxChanged,
      MyStorageDataTypeFlags dataChanged)
    {
      if ((dataChanged & MyStorageDataTypeFlags.Content) != MyStorageDataTypeFlags.None && this.Physics != null)
        this.Physics.InvalidateRange(minChanged, maxChanged);
      if (this.Render is MyRenderComponentVoxelMap)
        (this.Render as MyRenderComponentVoxelMap).InvalidateRange(minChanged, maxChanged);
      this.OnRangeChanged(minChanged, maxChanged, dataChanged);
      this.ContentChanged = true;
    }

    public override string GetFriendlyName() => nameof (MyVoxelMap);

    public override void Init(
      string storageName,
      VRage.Game.Voxels.IMyStorage storage,
      MatrixD worldMatrix,
      bool useVoxelOffset = true)
    {
      this.m_storageMax = storage.Size;
      base.Init(storageName, storage, worldMatrix, useVoxelOffset);
      this.m_storage.RangeChanged += new Action<Vector3I, Vector3I, MyStorageDataTypeFlags>(this.storage_RangeChanged);
    }

    protected override void InitVoxelMap(MatrixD worldMatrix, Vector3I size, bool useOffset = true)
    {
      base.InitVoxelMap(worldMatrix, size, useOffset);
      ((MyStorageBase) this.Storage).InitWriteCache(8);
      this.Physics = new MyVoxelPhysicsBody((MyVoxelBase) this, 3f, lazyPhysics: this.DelayRigidBodyCreation);
      this.Physics.Enabled = true;
    }

    public bool IsStaticForCluster
    {
      get => this.Physics.IsStaticForCluster;
      set => this.Physics.IsStaticForCluster = value;
    }

    public override int GetOrePriority() => 1;

    void IMyVoxelMap.Close() => this.Close();

    bool IMyVoxelMap.DoOverlapSphereTest(float sphereRadius, Vector3D spherePos) => this.DoOverlapSphereTest(sphereRadius, spherePos);

    void IMyVoxelMap.ClampVoxelCoord(ref Vector3I voxelCoord) => this.Storage.ClampVoxelCoord(ref voxelCoord);

    bool IMyVoxelMap.GetIntersectionWithSphere(ref BoundingSphereD sphere) => this.GetIntersectionWithSphere(ref sphere);

    MyObjectBuilder_EntityBase IMyVoxelMap.GetObjectBuilder(
      bool copy)
    {
      return this.GetObjectBuilder(copy);
    }

    float IMyVoxelMap.GetVoxelContentInBoundingBox(
      BoundingBoxD worldAabb,
      out float cellCount)
    {
      cellCount = 0.0f;
      return 0.0f;
    }

    Vector3I IMyVoxelMap.GetVoxelCoordinateFromMeters(Vector3D pos)
    {
      Vector3I voxelCoord;
      MyVoxelCoordSystems.WorldPositionToVoxelCoord(this.PositionLeftBottomCorner, ref pos, out voxelCoord);
      return voxelCoord;
    }

    void IMyVoxelMap.Init(MyObjectBuilder_EntityBase builder) => this.Init(builder);

    private class Sandbox_Game_Entities_MyVoxelMap\u003C\u003EActor : IActivator, IActivator<MyVoxelMap>
    {
      object IActivator.CreateInstance() => (object) new MyVoxelMap();

      MyVoxelMap IActivator<MyVoxelMap>.CreateInstance() => new MyVoxelMap();
    }
  }
}
