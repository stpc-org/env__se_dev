// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyFloatingObjects
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 800)]
  [StaticEventOwner]
  public class MyFloatingObjects : MySessionComponentBase
  {
    private static MyFloatingObjects.MyFloatingObjectComparer m_entityComparer = new MyFloatingObjects.MyFloatingObjectComparer();
    private static MyFloatingObjects.MyFloatingObjectTimestampComparer m_comparer = new MyFloatingObjects.MyFloatingObjectTimestampComparer();
    private static SortedSet<MyFloatingObject> m_floatingOres = new SortedSet<MyFloatingObject>((IComparer<MyFloatingObject>) MyFloatingObjects.m_comparer);
    private static SortedSet<MyFloatingObject> m_floatingItems = new SortedSet<MyFloatingObject>((IComparer<MyFloatingObject>) MyFloatingObjects.m_comparer);
    private static List<MyVoxelBase> m_tmpResultList = new List<MyVoxelBase>();
    private static List<MyFloatingObject> m_synchronizedFloatingObjects = new List<MyFloatingObject>();
    private static List<MyFloatingObject> m_floatingObjectsToSyncCreate = new List<MyFloatingObject>();
    private static MyFloatingObjects.MyFloatingObjectsSynchronizationComparer m_synchronizationComparer = new MyFloatingObjects.MyFloatingObjectsSynchronizationComparer();
    private static int m_updateCounter = 0;
    private static int m_checkObjectInsideVoxel = 0;
    private static List<Tuple<MyPhysicalInventoryItem, BoundingBoxD, Vector3D>> m_itemsToSpawnNextUpdate = new List<Tuple<MyPhysicalInventoryItem, BoundingBoxD, Vector3D>>();
    private static readonly MyConcurrentPool<List<Tuple<MyPhysicalInventoryItem, BoundingBoxD, Vector3D>>> m_itemsToSpawnPool = new MyConcurrentPool<List<Tuple<MyPhysicalInventoryItem, BoundingBoxD, Vector3D>>>();

    internal static bool CanSpawn(MyPhysicalInventoryItem ob) => !MySession.Static.SimplifiedSimulation || ob.Content is MyObjectBuilder_Component || ob.Content is MyObjectBuilder_GasContainerObject || ob.Content is MyObjectBuilder_PhysicalGunObject;

    public override void LoadData()
    {
      base.LoadData();
      MyVoxelMaterialDefinition materialDefinition = MyDefinitionManager.Static.GetVoxelMaterialDefinition((byte) 0);
      MyFloatingObjects.Spawn(new MyPhysicalInventoryItem((MyFixedPoint) 1, (MyObjectBuilder_PhysicalObject) MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Ore>(materialDefinition.MinedOre)), new BoundingSphereD(), (MyPhysicsComponentBase) null, materialDefinition, (Action<MyEntity>) (item => item.Close()));
    }

    public override Type[] Dependencies => new Type[1]
    {
      typeof (MyDebris)
    };

    public override void UpdateAfterSimulation()
    {
      if (!Sync.IsServer)
        return;
      this.CheckObjectInVoxel();
      ++MyFloatingObjects.m_updateCounter;
      if (MyFloatingObjects.m_updateCounter > 100)
        MyFloatingObjects.ReduceFloatingObjects();
      if (MyFloatingObjects.m_itemsToSpawnNextUpdate.Count > 0)
      {
        List<Tuple<MyPhysicalInventoryItem, BoundingBoxD, Vector3D>> tmp = MyFloatingObjects.m_itemsToSpawnNextUpdate;
        MyFloatingObjects.m_itemsToSpawnNextUpdate = MyFloatingObjects.m_itemsToSpawnPool.Get();
        if (MySandboxGame.Config.SyncRendering)
        {
          MyEntityIdentifier.PrepareSwapData();
          MyEntityIdentifier.SwapPerThreadData();
        }
        Parallel.Start((Action) (() =>
        {
          this.SpawnInventoryItems(tmp);
          tmp.Clear();
          MyFloatingObjects.m_itemsToSpawnPool.Return(tmp);
        }));
        if (MySandboxGame.Config.SyncRendering)
          MyEntityIdentifier.ClearSwapDataAndRestore();
      }
      base.UpdateAfterSimulation();
      if (MyFloatingObjects.m_updateCounter > 100)
        this.OptimizeFloatingObjects();
      if (MyFloatingObjects.m_updateCounter <= 100)
        return;
      MyFloatingObjects.m_updateCounter = 0;
    }

    private void OptimizeFloatingObjects()
    {
      MyFloatingObjects.ReduceFloatingObjects();
      this.OptimizeQualityType();
    }

    private void CheckObjectInVoxel()
    {
      if (!Sync.IsServer)
        return;
      if (MyFloatingObjects.m_checkObjectInsideVoxel >= MyFloatingObjects.m_synchronizedFloatingObjects.Count)
        MyFloatingObjects.m_checkObjectInsideVoxel = 0;
      if (MyFloatingObjects.m_synchronizedFloatingObjects.Count > 0)
      {
        MyFloatingObject synchronizedFloatingObject = MyFloatingObjects.m_synchronizedFloatingObjects[MyFloatingObjects.m_checkObjectInsideVoxel];
        BoundingBoxD localAabb = (BoundingBoxD) synchronizedFloatingObject.PositionComp.LocalAABB;
        MatrixD aabbWorldTransform = synchronizedFloatingObject.PositionComp.WorldMatrixRef;
        BoundingBoxD worldAabb = synchronizedFloatingObject.PositionComp.WorldAABB;
        using (MyFloatingObjects.m_tmpResultList.GetClearToken<MyVoxelBase>())
        {
          MyGamePruningStructure.GetAllVoxelMapsInBox(ref worldAabb, MyFloatingObjects.m_tmpResultList);
          bool flag = false;
          foreach (MyVoxelBase tmpResult in MyFloatingObjects.m_tmpResultList)
          {
            if (tmpResult != null && !tmpResult.MarkedForClose && (!(tmpResult is MyVoxelPhysics) && tmpResult.AreAllAabbCornersInside(ref aabbWorldTransform, localAabb)))
            {
              flag = true;
              break;
            }
          }
          if (flag)
          {
            ++synchronizedFloatingObject.NumberOfFramesInsideVoxel;
            if (synchronizedFloatingObject.NumberOfFramesInsideVoxel > 5)
              MyFloatingObjects.RemoveFloatingObject(synchronizedFloatingObject);
          }
          else
            synchronizedFloatingObject.NumberOfFramesInsideVoxel = 0;
        }
      }
      ++MyFloatingObjects.m_checkObjectInsideVoxel;
    }

    private void SpawnInventoryItems(
      List<Tuple<MyPhysicalInventoryItem, BoundingBoxD, Vector3D>> itemsList)
    {
      for (int index = 0; index < itemsList.Count; ++index)
      {
        Tuple<MyPhysicalInventoryItem, BoundingBoxD, Vector3D> item = itemsList[index];
        item.Item1.Spawn(item.Item1.Amount, item.Item2, completionCallback: ((Action<MyEntity>) (entity =>
        {
          entity.Physics.LinearVelocity = (Vector3) item.Item3;
          entity.Physics.ApplyImpulse(MyUtils.GetRandomVector3Normalized() * entity.Physics.Mass / 5f, entity.PositionComp.GetPosition());
        })));
      }
      itemsList.Clear();
    }

    public static void Spawn(
      MyPhysicalInventoryItem item,
      Vector3D position,
      Vector3D forward,
      Vector3D up,
      MyPhysicsComponentBase motionInheritedFrom = null,
      Action<MyEntity> completionCallback = null)
    {
      if (!MyEntities.IsInsideWorld(position))
        return;
      Vector3D forward1 = forward;
      Vector3D up1 = up;
      Vector3D vector3D = Vector3D.Cross(up, forward);
      MyPhysicalItemDefinition definition = (MyPhysicalItemDefinition) null;
      if (MyDefinitionManager.Static.TryGetDefinition<MyPhysicalItemDefinition>(item.Content.GetObjectId(), out definition))
      {
        if (definition.RotateOnSpawnX)
        {
          forward1 = up;
          up1 = -forward;
        }
        if (definition.RotateOnSpawnY)
          forward1 = vector3D;
        if (definition.RotateOnSpawnZ)
          up1 = -vector3D;
      }
      MyFloatingObjects.Spawn(item, MatrixD.CreateWorld(position, forward1, up1), motionInheritedFrom, completionCallback);
    }

    public static void Spawn(
      MyPhysicalInventoryItem item,
      MatrixD worldMatrix,
      MyPhysicsComponentBase motionInheritedFrom,
      Action<MyEntity> completionCallback)
    {
      if (!MyFloatingObjects.CanSpawn(item) || !MyEntities.IsInsideWorld(worldMatrix.Translation))
        return;
      MyObjectBuilder_FloatingObject builderFloatingObject = MyFloatingObjects.PrepareBuilder(ref item);
      builderFloatingObject.PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation(worldMatrix));
      MyEntities.CreateFromObjectBuilderParallel((MyObjectBuilder_EntityBase) builderFloatingObject, true, (Action<MyEntity>) (entity =>
      {
        if (entity == null || entity.Physics == null)
          return;
        entity.Physics.ForceActivate();
        MyFloatingObjects.ApplyPhysics(entity, motionInheritedFrom);
        if (MyVisualScriptLogicProvider.ItemSpawned != null)
          MyVisualScriptLogicProvider.ItemSpawned(item.Content.TypeId.ToString(), item.Content.SubtypeName, entity.EntityId, item.Amount.ToIntSafe(), worldMatrix.Translation);
        if (completionCallback == null)
          return;
        completionCallback(entity);
      }));
    }

    internal static MyEntity Spawn(
      MyPhysicalInventoryItem item,
      BoundingBoxD box,
      MyPhysicsComponentBase motionInheritedFrom = null)
    {
      if (!MyFloatingObjects.CanSpawn(item))
        return (MyEntity) null;
      MyEntity fromObjectBuilder = MyEntities.CreateFromObjectBuilder((MyObjectBuilder_EntityBase) MyFloatingObjects.PrepareBuilder(ref item), false);
      if (fromObjectBuilder != null)
      {
        float radius = fromObjectBuilder.PositionComp.LocalVolume.Radius;
        Vector3D vector3D = (Vector3D) Vector3.Max((Vector3) (box.Size / 2.0 - new Vector3(radius)), Vector3.Zero);
        box = new BoundingBoxD(box.Center - vector3D, box.Center + vector3D);
        Vector3D randomPosition = MyUtils.GetRandomPosition(ref box);
        MyFloatingObjects.AddToPos(fromObjectBuilder, randomPosition, motionInheritedFrom);
        fromObjectBuilder.Physics.ForceActivate();
        if (MyVisualScriptLogicProvider.ItemSpawned != null)
          MyVisualScriptLogicProvider.ItemSpawned(item.Content.TypeId.ToString(), item.Content.SubtypeName, fromObjectBuilder.EntityId, item.Amount.ToIntSafe(), randomPosition);
      }
      return fromObjectBuilder;
    }

    public static void Spawn(
      MyPhysicalInventoryItem item,
      BoundingSphereD sphere,
      MyPhysicsComponentBase motionInheritedFrom,
      MyVoxelMaterialDefinition voxelMaterial,
      Action<MyEntity> OnDone)
    {
      if (!MyFloatingObjects.CanSpawn(item))
        return;
      MyEntities.CreateFromObjectBuilderParallel((MyObjectBuilder_EntityBase) MyFloatingObjects.PrepareBuilder(ref item), completionCallback: ((Action<MyEntity>) (entity =>
      {
        if (voxelMaterial.DamagedMaterial != MyStringHash.NullOrEmpty)
          voxelMaterial = MyDefinitionManager.Static.GetVoxelMaterialDefinition(voxelMaterial.DamagedMaterial.ToString());
        ((MyFloatingObject) entity).VoxelMaterial = voxelMaterial;
        sphere = new BoundingSphereD(sphere.Center, Math.Max(sphere.Radius - (double) entity.PositionComp.LocalVolume.Radius, 0.0));
        Vector3D randomBorderPosition = MyUtils.GetRandomBorderPosition(ref sphere);
        MyFloatingObjects.AddToPos(entity, randomBorderPosition, motionInheritedFrom);
        if (MyVisualScriptLogicProvider.ItemSpawned != null)
          MyVisualScriptLogicProvider.ItemSpawned(item.Content.TypeId.ToString(), item.Content.SubtypeName, entity.EntityId, item.Amount.ToIntSafe(), randomBorderPosition);
        OnDone(entity);
      })));
    }

    public static void Spawn(
      MyPhysicalItemDefinition itemDefinition,
      Vector3D translation,
      Vector3D forward,
      Vector3D up,
      int amount = 1,
      float scale = 1f)
    {
      MyObjectBuilder_PhysicalObject newObject = MyObjectBuilderSerializer.CreateNewObject(itemDefinition.Id.TypeId, itemDefinition.Id.SubtypeName) as MyObjectBuilder_PhysicalObject;
      MyFloatingObjects.Spawn(new MyPhysicalInventoryItem((MyFixedPoint) amount, newObject, scale), translation, forward, up);
    }

    public static void EnqueueInventoryItemSpawn(
      MyPhysicalInventoryItem inventoryItem,
      BoundingBoxD boundingBox,
      Vector3D inheritedVelocity)
    {
      MyFloatingObjects.m_itemsToSpawnNextUpdate.Add(Tuple.Create<MyPhysicalInventoryItem, BoundingBoxD, Vector3D>(inventoryItem, boundingBox, inheritedVelocity));
    }

    private static MyObjectBuilder_FloatingObject PrepareBuilder(
      ref MyPhysicalInventoryItem item)
    {
      MyObjectBuilder_FloatingObject newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_FloatingObject>();
      newObject.Item = item.GetObjectBuilder();
      MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition((MyObjectBuilder_Base) item.Content);
      newObject.ModelVariant = physicalItemDefinition.HasModelVariants ? MyUtils.GetRandomInt(physicalItemDefinition.Models.Length) : 0;
      newObject.PersistentFlags |= MyPersistentEntityFlags2.Enabled | MyPersistentEntityFlags2.InScene;
      return newObject;
    }

    private static void AddToPos(
      MyEntity thrownEntity,
      Vector3D pos,
      MyPhysicsComponentBase motionInheritedFrom)
    {
      Vector3 vector3Normalized1 = MyUtils.GetRandomVector3Normalized();
      Vector3 vector3Normalized2 = MyUtils.GetRandomVector3Normalized();
      while (vector3Normalized1 == vector3Normalized2)
        vector3Normalized2 = MyUtils.GetRandomVector3Normalized();
      Vector3 up = Vector3.Cross(Vector3.Cross(vector3Normalized1, vector3Normalized2), vector3Normalized1);
      thrownEntity.WorldMatrix = MatrixD.CreateWorld(pos, vector3Normalized1, up);
      MyEntities.Add(thrownEntity);
      MyFloatingObjects.ApplyPhysics(thrownEntity, motionInheritedFrom);
    }

    private static void ApplyPhysics(
      MyEntity thrownEntity,
      MyPhysicsComponentBase motionInheritedFrom)
    {
      if (thrownEntity.Physics == null || motionInheritedFrom == null)
        return;
      thrownEntity.Physics.LinearVelocity = motionInheritedFrom.LinearVelocity;
      thrownEntity.Physics.AngularVelocity = motionInheritedFrom.AngularVelocity;
    }

    private void OptimizeQualityType()
    {
      for (int index = 0; index < MyFloatingObjects.m_synchronizedFloatingObjects.Count; ++index)
      {
        MyFloatingObject synchronizedFloatingObject = MyFloatingObjects.m_synchronizedFloatingObjects[index];
      }
    }

    public static int FloatingOreCount => MyFloatingObjects.m_floatingOres.Count;

    public static int FloatingItemCount => MyFloatingObjects.m_floatingItems.Count;

    internal static void RegisterFloatingObject(MyFloatingObject obj)
    {
      if (obj.WasRemovedFromWorld)
        return;
      obj.CreationTime = Stopwatch.GetTimestamp();
      if (obj.VoxelMaterial != null)
        MyFloatingObjects.m_floatingOres.Add(obj);
      else
        MyFloatingObjects.m_floatingItems.Add(obj);
      if (!Sync.IsServer)
        return;
      MyFloatingObjects.AddToSynchronization(obj);
    }

    internal static void UnregisterFloatingObject(MyFloatingObject obj)
    {
      if (obj.VoxelMaterial != null)
        MyFloatingObjects.m_floatingOres.Remove(obj);
      else
        MyFloatingObjects.m_floatingItems.Remove(obj);
      if (Sync.IsServer)
        MyFloatingObjects.RemoveFromSynchronization(obj);
      obj.WasRemovedFromWorld = true;
    }

    public static void AddFloatingObjectAmount(MyFloatingObject obj, MyFixedPoint amount)
    {
      MyPhysicalInventoryItem physicalInventoryItem = obj.Item;
      physicalInventoryItem.Amount += amount;
      obj.Item = physicalInventoryItem;
      obj.Amount.Value = physicalInventoryItem.Amount;
      obj.UpdateInternalState();
    }

    public static void RemoveFloatingObject(MyFloatingObject obj, bool sync)
    {
      if (sync)
      {
        if (Sync.IsServer)
          MyFloatingObjects.RemoveFloatingObject(obj);
        else
          obj.SendCloseRequest();
      }
      else
        MyFloatingObjects.RemoveFloatingObject(obj);
    }

    public static void RemoveFloatingObject(MyFloatingObject obj) => MyFloatingObjects.RemoveFloatingObject(obj, MyFixedPoint.MaxValue);

    internal static void RemoveFloatingObject(MyFloatingObject obj, MyFixedPoint amount)
    {
      if (amount <= (MyFixedPoint) 0)
        return;
      if (amount < obj.Item.Amount)
      {
        obj.Amount.Value -= amount;
        obj.RefreshDisplayName();
      }
      else
      {
        obj.Render.FadeOut = false;
        obj.Close();
        obj.WasRemovedFromWorld = true;
      }
    }

    public static void ReduceFloatingObjects()
    {
      int num1 = MyFloatingObjects.m_floatingOres.Count + MyFloatingObjects.m_floatingItems.Count;
      int num2 = Math.Max((int) MySession.Static.MaxFloatingObjects / 5, 4);
      for (; num1 > (int) MySession.Static.MaxFloatingObjects; --num1)
      {
        SortedSet<MyFloatingObject> source = MyFloatingObjects.m_floatingOres.Count > num2 || MyFloatingObjects.m_floatingItems.Count == 0 ? MyFloatingObjects.m_floatingOres : MyFloatingObjects.m_floatingItems;
        if (source.Count > 0)
        {
          MyFloatingObject myFloatingObject = source.Last<MyFloatingObject>();
          source.Remove(myFloatingObject);
          if (Sync.IsServer)
            MyFloatingObjects.RemoveFloatingObject(myFloatingObject);
        }
      }
    }

    private static void AddToSynchronization(MyFloatingObject floatingObject)
    {
      MyFloatingObjects.m_floatingObjectsToSyncCreate.Add(floatingObject);
      MyFloatingObjects.m_synchronizedFloatingObjects.Add(floatingObject);
      floatingObject.OnClose += new Action<MyEntity>(MyFloatingObjects.floatingObject_OnClose);
    }

    private static void floatingObject_OnClose(MyEntity obj)
    {
    }

    private static void RemoveFromSynchronization(MyFloatingObject floatingObject)
    {
      floatingObject.OnClose -= new Action<MyEntity>(MyFloatingObjects.floatingObject_OnClose);
      MyFloatingObjects.m_synchronizedFloatingObjects.Remove(floatingObject);
      MyFloatingObjects.m_floatingObjectsToSyncCreate.Remove(floatingObject);
    }

    public static MyObjectBuilder_FloatingObject ChangeObjectBuilder(
      MyComponentDefinition componentDef,
      MyObjectBuilder_EntityBase entityOb)
    {
      MyObjectBuilder_PhysicalObject newObject = MyObjectBuilderSerializer.CreateNewObject(componentDef.Id.TypeId, componentDef.Id.SubtypeName) as MyObjectBuilder_PhysicalObject;
      Vector3 up = (Vector3) entityOb.PositionAndOrientation.Value.Up;
      Vector3 forward = (Vector3) entityOb.PositionAndOrientation.Value.Forward;
      Vector3D position = (Vector3D) entityOb.PositionAndOrientation.Value.Position;
      MyPhysicalInventoryItem physicalInventoryItem = new MyPhysicalInventoryItem((MyFixedPoint) 1, newObject);
      MyObjectBuilder_FloatingObject builderFloatingObject = MyFloatingObjects.PrepareBuilder(ref physicalInventoryItem);
      builderFloatingObject.PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation(position, forward, up));
      builderFloatingObject.EntityId = entityOb.EntityId;
      return builderFloatingObject;
    }

    public static void RequestSpawnCreative(MyObjectBuilder_FloatingObject obj)
    {
      if (!MySession.Static.HasCreativeRights && !MySession.Static.CreativeMode)
        return;
      MyMultiplayer.RaiseStaticEvent<MyObjectBuilder_FloatingObject>((Func<IMyEventOwner, Action<MyObjectBuilder_FloatingObject>>) (x => new Action<MyObjectBuilder_FloatingObject>(MyFloatingObjects.RequestSpawnCreative_Implementation)), obj);
    }

    [Event(null, 654)]
    [Reliable]
    [Server]
    private static void RequestSpawnCreative_Implementation(MyObjectBuilder_FloatingObject obj)
    {
      if (MySession.Static.CreativeMode || (MyEventContext.Current.IsLocallyInvoked || MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value)))
        MyEntities.CreateFromObjectBuilderAndAdd((MyObjectBuilder_EntityBase) obj, false);
      else
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
    }

    private class MyFloatingObjectComparer : IEqualityComparer<MyFloatingObject>
    {
      public bool Equals(MyFloatingObject x, MyFloatingObject y) => x.EntityId == y.EntityId;

      public int GetHashCode(MyFloatingObject obj) => (int) obj.EntityId;
    }

    private class MyFloatingObjectTimestampComparer : IComparer<MyFloatingObject>
    {
      public int Compare(MyFloatingObject x, MyFloatingObject y) => x.CreationTime != y.CreationTime ? y.CreationTime.CompareTo(x.CreationTime) : y.EntityId.CompareTo(x.EntityId);
    }

    private class MyFloatingObjectsSynchronizationComparer : IComparer<MyFloatingObject>
    {
      public int Compare(MyFloatingObject x, MyFloatingObject y) => x.ClosestDistanceToAnyPlayerSquared.CompareTo(y.ClosestDistanceToAnyPlayerSquared);
    }

    private struct StabilityInfo
    {
      public MyPositionAndOrientation PositionAndOr;

      public StabilityInfo(MyPositionAndOrientation posAndOr) => this.PositionAndOr = posAndOr;
    }

    protected sealed class RequestSpawnCreative_Implementation\u003C\u003EVRage_Game_MyObjectBuilder_FloatingObject : ICallSite<IMyEventOwner, MyObjectBuilder_FloatingObject, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyObjectBuilder_FloatingObject obj,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyFloatingObjects.RequestSpawnCreative_Implementation(obj);
      }
    }
  }
}
