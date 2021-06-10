// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyGamePruningStructure
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Library.Threading;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
  [MySessionComponentDescriptor(MyUpdateOrder.Simulation)]
  public class MyGamePruningStructure : MySessionComponentBase
  {
    private static MyDynamicAABBTreeD m_dynamicObjectsTree;
    private static MyDynamicAABBTreeD m_staticObjectsTree;
    private static MyDynamicAABBTreeD m_voxelMapsTree;
    [ThreadStatic]
    private static List<MyVoxelBase> m_cachedVoxelList;
    private static readonly SpinLockRef m_movedLock = new SpinLockRef();
    private static HashSet<MyEntity> m_moved = new HashSet<MyEntity>();
    private static HashSet<MyEntity> m_movedUpdate = new HashSet<MyEntity>();

    static MyGamePruningStructure() => MyGamePruningStructure.Init();

    private static void Init()
    {
      MyGamePruningStructure.m_dynamicObjectsTree = new MyDynamicAABBTreeD(MyConstants.GAME_PRUNING_STRUCTURE_AABB_EXTENSION);
      MyGamePruningStructure.m_voxelMapsTree = new MyDynamicAABBTreeD(MyConstants.GAME_PRUNING_STRUCTURE_AABB_EXTENSION);
      MyGamePruningStructure.m_staticObjectsTree = new MyDynamicAABBTreeD(Vector3D.Zero);
    }

    public static BoundingBoxD GetEntityAABB(MyEntity entity)
    {
      BoundingBoxD boundingBoxD = entity.PositionComp.WorldAABB;
      if (entity.Physics != null)
        boundingBoxD = boundingBoxD.Include(entity.WorldMatrix.Translation + entity.Physics.LinearVelocity * 0.01666667f * 5f);
      return boundingBoxD;
    }

    private static bool IsEntityStatic(MyEntity entity) => entity.Physics == null || entity.Physics.IsStatic;

    public static void Add(MyEntity entity)
    {
      if (entity.Parent != null && (entity.Flags & EntityFlags.IsGamePrunningStructureObject) == (EntityFlags) 0 || entity.TopMostPruningProxyId != -1)
        return;
      BoundingBoxD entityAabb = MyGamePruningStructure.GetEntityAABB(entity);
      if (entityAabb.Size == Vector3D.Zero)
        return;
      if (MyGamePruningStructure.IsEntityStatic(entity))
      {
        entity.TopMostPruningProxyId = MyGamePruningStructure.m_staticObjectsTree.AddProxy(ref entityAabb, (object) entity, 0U);
        entity.StaticForPruningStructure = true;
      }
      else
      {
        entity.TopMostPruningProxyId = MyGamePruningStructure.m_dynamicObjectsTree.AddProxy(ref entityAabb, (object) entity, 0U);
        entity.StaticForPruningStructure = false;
      }
      if (!(entity is MyVoxelBase myVoxelBase))
        return;
      myVoxelBase.VoxelMapPruningProxyId = MyGamePruningStructure.m_voxelMapsTree.AddProxy(ref entityAabb, (object) entity, 0U);
    }

    public static void Remove(MyEntity entity)
    {
      if (entity is MyVoxelBase myVoxelBase && myVoxelBase.VoxelMapPruningProxyId != -1)
      {
        MyGamePruningStructure.m_voxelMapsTree.RemoveProxy(myVoxelBase.VoxelMapPruningProxyId);
        myVoxelBase.VoxelMapPruningProxyId = -1;
      }
      if (entity.TopMostPruningProxyId == -1)
        return;
      if (entity.StaticForPruningStructure)
        MyGamePruningStructure.m_staticObjectsTree.RemoveProxy(entity.TopMostPruningProxyId);
      else
        MyGamePruningStructure.m_dynamicObjectsTree.RemoveProxy(entity.TopMostPruningProxyId);
      entity.TopMostPruningProxyId = -1;
    }

    public static void Clear()
    {
      MyGamePruningStructure.m_voxelMapsTree.Clear();
      MyGamePruningStructure.m_dynamicObjectsTree.Clear();
      MyGamePruningStructure.m_staticObjectsTree.Clear();
      using (MyGamePruningStructure.m_movedLock.Acquire())
        MyGamePruningStructure.m_moved.Clear();
    }

    public static void Move(MyEntity entity)
    {
      using (MyGamePruningStructure.m_movedLock.Acquire())
        MyGamePruningStructure.m_moved.Add(entity);
    }

    private static void MoveInternal(MyEntity entity)
    {
      if (entity.TopMostPruningProxyId == -1)
        return;
      BoundingBoxD entityAabb = MyGamePruningStructure.GetEntityAABB(entity);
      if (entityAabb.Size == Vector3D.Zero)
      {
        MyGamePruningStructure.Remove(entity);
      }
      else
      {
        if (entity is MyVoxelBase myVoxelBase)
          MyGamePruningStructure.m_voxelMapsTree.MoveProxy(myVoxelBase.VoxelMapPruningProxyId, ref entityAabb, Vector3D.Zero);
        if (entity.TopMostPruningProxyId == -1)
          return;
        bool flag = MyGamePruningStructure.IsEntityStatic(entity);
        if (flag != entity.StaticForPruningStructure)
        {
          if (entity.StaticForPruningStructure)
          {
            MyGamePruningStructure.m_staticObjectsTree.RemoveProxy(entity.TopMostPruningProxyId);
            entity.TopMostPruningProxyId = MyGamePruningStructure.m_dynamicObjectsTree.AddProxy(ref entityAabb, (object) entity, 0U);
          }
          else
          {
            MyGamePruningStructure.m_dynamicObjectsTree.RemoveProxy(entity.TopMostPruningProxyId);
            entity.TopMostPruningProxyId = MyGamePruningStructure.m_staticObjectsTree.AddProxy(ref entityAabb, (object) entity, 0U);
          }
          entity.StaticForPruningStructure = flag;
        }
        else if (entity.StaticForPruningStructure)
          MyGamePruningStructure.m_staticObjectsTree.MoveProxy(entity.TopMostPruningProxyId, ref entityAabb, Vector3D.Zero);
        else
          MyGamePruningStructure.m_dynamicObjectsTree.MoveProxy(entity.TopMostPruningProxyId, ref entityAabb, Vector3D.Zero);
      }
    }

    private static void Update()
    {
      using (MyGamePruningStructure.m_movedLock.Acquire())
        MyUtils.Swap<HashSet<MyEntity>>(ref MyGamePruningStructure.m_moved, ref MyGamePruningStructure.m_movedUpdate);
      foreach (MyEntity entity in MyGamePruningStructure.m_movedUpdate)
        MyGamePruningStructure.MoveInternal(entity);
      MyGamePruningStructure.m_movedUpdate.Clear();
    }

    public static void GetAllEntitiesInBox(
      ref BoundingBoxD box,
      List<MyEntity> result,
      MyEntityQueryType qtype = MyEntityQueryType.Both)
    {
      if (qtype.HasDynamic())
        MyGamePruningStructure.m_dynamicObjectsTree.OverlapAllBoundingBox<MyEntity>(ref box, result, clear: false);
      if (qtype.HasStatic())
        MyGamePruningStructure.m_staticObjectsTree.OverlapAllBoundingBox<MyEntity>(ref box, result, clear: false);
      int count = result.Count;
      for (int index = 0; index < count; ++index)
      {
        if (result[index].Hierarchy != null)
          result[index].Hierarchy.QueryAABB(ref box, result);
      }
    }

    public static void GetAllEntitiesInOBB(
      ref MyOrientedBoundingBoxD obb,
      List<MyEntity> result,
      MyEntityQueryType qtype = MyEntityQueryType.Both)
    {
      if (qtype.HasDynamic())
        MyGamePruningStructure.m_dynamicObjectsTree.OverlapAllBoundingBox<MyEntity>(ref obb, result, clear: false);
      if (!qtype.HasStatic())
        return;
      MyGamePruningStructure.m_staticObjectsTree.OverlapAllBoundingBox<MyEntity>(ref obb, result, clear: false);
    }

    public static void GetTopMostEntitiesInBox(
      ref BoundingBoxD box,
      List<MyEntity> result,
      MyEntityQueryType qtype = MyEntityQueryType.Both)
    {
      if (qtype.HasDynamic())
        MyGamePruningStructure.m_dynamicObjectsTree.OverlapAllBoundingBox<MyEntity>(ref box, result, clear: false);
      if (!qtype.HasStatic())
        return;
      MyGamePruningStructure.m_staticObjectsTree.OverlapAllBoundingBox<MyEntity>(ref box, result, clear: false);
    }

    public static void GetAllTopMostStaticEntitiesInBox(
      ref BoundingBoxD box,
      List<MyEntity> result,
      MyEntityQueryType qtype = MyEntityQueryType.Both)
    {
      if (qtype.HasDynamic())
        MyGamePruningStructure.m_dynamicObjectsTree.OverlapAllBoundingBox<MyEntity>(ref box, result, clear: false);
      if (!qtype.HasStatic())
        return;
      MyGamePruningStructure.m_staticObjectsTree.OverlapAllBoundingBox<MyEntity>(ref box, result, clear: false);
    }

    public static void GetAllEntitiesInSphere(
      ref BoundingSphereD sphere,
      List<MyEntity> result,
      MyEntityQueryType qtype = MyEntityQueryType.Both)
    {
      if (qtype.HasDynamic())
        MyGamePruningStructure.m_dynamicObjectsTree.OverlapAllBoundingSphere<MyEntity>(ref sphere, result, false);
      if (qtype.HasStatic())
        MyGamePruningStructure.m_staticObjectsTree.OverlapAllBoundingSphere<MyEntity>(ref sphere, result, false);
      int count = result.Count;
      for (int index = 0; index < count; ++index)
      {
        if (result[index].Hierarchy != null)
          result[index].Hierarchy.QuerySphere(ref sphere, result);
      }
    }

    public static void GetAllTopMostEntitiesInSphere(
      ref BoundingSphereD sphere,
      List<MyEntity> result,
      MyEntityQueryType qtype = MyEntityQueryType.Both)
    {
      if (qtype.HasDynamic())
        MyGamePruningStructure.m_dynamicObjectsTree.OverlapAllBoundingSphere<MyEntity>(ref sphere, result, false);
      if (!qtype.HasStatic())
        return;
      MyGamePruningStructure.m_staticObjectsTree.OverlapAllBoundingSphere<MyEntity>(ref sphere, result, false);
    }

    public static void GetAllTargetsInSphere(
      ref BoundingSphereD sphere,
      List<MyEntity> result,
      MyEntityQueryType qtype = MyEntityQueryType.Both)
    {
      if (qtype.HasDynamic())
        MyGamePruningStructure.m_dynamicObjectsTree.OverlapAllBoundingSphere<MyEntity>(ref sphere, result, false);
      if (qtype.HasStatic())
        MyGamePruningStructure.m_staticObjectsTree.OverlapAllBoundingSphere<MyEntity>(ref sphere, result, false);
      int count = result.Count;
      for (int index = 0; index < count; ++index)
      {
        if (result[index].Hierarchy != null)
          result[index].Hierarchy.QuerySphere(ref sphere, result);
      }
    }

    public static void GetAllEntitiesInRay(
      ref LineD ray,
      List<MyLineSegmentOverlapResult<MyEntity>> result,
      MyEntityQueryType qtype = MyEntityQueryType.Both)
    {
      if (qtype.HasDynamic())
        MyGamePruningStructure.m_dynamicObjectsTree.OverlapAllLineSegment<MyEntity>(ref ray, result);
      if (qtype.HasStatic())
        MyGamePruningStructure.m_staticObjectsTree.OverlapAllLineSegment<MyEntity>(ref ray, result, false);
      int count = result.Count;
      for (int index = 0; index < count; ++index)
      {
        if (result[index].Element.Hierarchy != null)
          result[index].Element.Hierarchy.QueryLine(ref ray, result);
      }
    }

    public static void GetTopmostEntitiesOverlappingRay(
      ref LineD ray,
      List<MyLineSegmentOverlapResult<MyEntity>> result,
      MyEntityQueryType qtype = MyEntityQueryType.Both)
    {
      if (qtype.HasDynamic())
        MyGamePruningStructure.m_dynamicObjectsTree.OverlapAllLineSegment<MyEntity>(ref ray, result);
      if (!qtype.HasStatic())
        return;
      MyGamePruningStructure.m_staticObjectsTree.OverlapAllLineSegment<MyEntity>(ref ray, result, false);
    }

    public static void GetVoxelMapsOverlappingRay(
      ref LineD ray,
      List<MyLineSegmentOverlapResult<MyVoxelBase>> result)
    {
      MyGamePruningStructure.m_voxelMapsTree.OverlapAllLineSegment<MyVoxelBase>(ref ray, result);
    }

    public static void GetAproximateDynamicClustersForSize(
      ref BoundingBoxD container,
      double clusterSize,
      List<BoundingBoxD> clusters)
    {
      MyGamePruningStructure.m_dynamicObjectsTree.GetAproximateClustersForAabb(ref container, clusterSize, clusters);
    }

    public static void GetAllVoxelMapsInBox(ref BoundingBoxD box, List<MyVoxelBase> result) => MyGamePruningStructure.m_voxelMapsTree.OverlapAllBoundingBox<MyVoxelBase>(ref box, result, clear: false);

    public static bool AnyVoxelMapInBox(ref BoundingBoxD box) => MyGamePruningStructure.m_voxelMapsTree.OverlapsAnyLeafBoundingBox(ref box);

    public static MyPlanet GetClosestPlanet(Vector3D position)
    {
      BoundingBoxD box = new BoundingBoxD(position, position);
      return MyGamePruningStructure.GetClosestPlanet(ref box);
    }

    public static MyPlanet GetClosestPlanet(ref BoundingBoxD box)
    {
      using (MyUtils.ReuseCollection<MyVoxelBase>(ref MyGamePruningStructure.m_cachedVoxelList))
      {
        MyGamePruningStructure.m_voxelMapsTree.OverlapAllBoundingBox<MyVoxelBase>(ref box, MyGamePruningStructure.m_cachedVoxelList, clear: false);
        MyPlanet myPlanet = (MyPlanet) null;
        Vector3D center = box.Center;
        double num1 = double.PositiveInfinity;
        foreach (MyVoxelBase cachedVoxel in MyGamePruningStructure.m_cachedVoxelList)
        {
          if (cachedVoxel is MyPlanet)
          {
            double num2 = (center - cachedVoxel.WorldMatrix.Translation).LengthSquared();
            if (num2 < num1)
            {
              num1 = num2;
              myPlanet = (MyPlanet) cachedVoxel;
            }
          }
        }
        return myPlanet;
      }
    }

    public static void GetAllVoxelMapsInSphere(ref BoundingSphereD sphere, List<MyVoxelBase> result) => MyGamePruningStructure.m_voxelMapsTree.OverlapAllBoundingSphere<MyVoxelBase>(ref sphere, result, false);

    public static void DebugDraw()
    {
      List<BoundingBoxD> boxsList = new List<BoundingBoxD>();
      MyGamePruningStructure.m_dynamicObjectsTree.GetAllNodeBounds(boxsList);
      using (IMyDebugDrawBatchAabb debugDrawBatchAabb = MyRenderProxy.DebugDrawBatchAABB(MatrixD.Identity, new Color(Color.SkyBlue, 0.05f), false, false))
      {
        foreach (BoundingBoxD aabb in boxsList)
          debugDrawBatchAabb.Add(ref aabb);
      }
      boxsList.Clear();
      MyGamePruningStructure.m_staticObjectsTree.GetAllNodeBounds(boxsList);
      using (IMyDebugDrawBatchAabb debugDrawBatchAabb = MyRenderProxy.DebugDrawBatchAABB(MatrixD.Identity, new Color(Color.Aquamarine, 0.05f), false, false))
      {
        foreach (BoundingBoxD aabb in boxsList)
          debugDrawBatchAabb.Add(ref aabb);
      }
    }

    public override void Simulate()
    {
      base.Simulate();
      MyGamePruningStructure.Update();
    }

    public static void GetTopmostEntitiesInBox(
      ref BoundingBoxD box,
      List<MyEntity> result,
      MyEntityQueryType qtype = MyEntityQueryType.Both)
    {
      if (qtype.HasDynamic())
        MyGamePruningStructure.m_dynamicObjectsTree.OverlapAllBoundingBox<MyEntity>(ref box, result, clear: false);
      if (!qtype.HasStatic())
        return;
      MyGamePruningStructure.m_staticObjectsTree.OverlapAllBoundingBox<MyEntity>(ref box, result, clear: false);
    }
  }
}
