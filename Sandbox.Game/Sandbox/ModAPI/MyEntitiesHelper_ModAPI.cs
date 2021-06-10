// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.MyEntitiesHelper_ModAPI
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using VRage;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.ModAPI
{
  public class MyEntitiesHelper_ModAPI : IMyEntities
  {
    private List<MyEntity> m_entityList = new List<MyEntity>();

    void IMyEntities.GetEntities(
      HashSet<IMyEntity> entities,
      Func<IMyEntity, bool> collect)
    {
      foreach (MyEntity entity in MyEntities.GetEntities())
      {
        if (collect == null || collect((IMyEntity) entity))
          entities.Add((IMyEntity) entity);
      }
    }

    bool IMyEntities.TryGetEntityById(long id, out IMyEntity entity)
    {
      MyEntity entity1;
      int num = MyEntities.TryGetEntityById(id, out entity1) ? 1 : 0;
      entity = (IMyEntity) entity1;
      return num != 0;
    }

    bool IMyEntities.TryGetEntityById(long? id, out IMyEntity entity)
    {
      entity = (IMyEntity) null;
      bool flag = false;
      if (id.HasValue)
      {
        MyEntity entity1;
        flag = MyEntities.TryGetEntityById(id.Value, out entity1);
        entity = (IMyEntity) entity1;
      }
      return flag;
    }

    bool IMyEntities.TryGetEntityByName(string name, out IMyEntity entity)
    {
      MyEntity entity1;
      int num = MyEntities.TryGetEntityByName(name, out entity1) ? 1 : 0;
      entity = (IMyEntity) entity1;
      return num != 0;
    }

    bool IMyEntities.EntityExists(string name) => MyEntities.EntityExists(name);

    void IMyEntities.AddEntity(IMyEntity entity, bool insertIntoScene)
    {
      if (!(entity is MyEntity))
        return;
      MyEntities.Add(entity as MyEntity, insertIntoScene);
    }

    IMyEntity IMyEntities.CreateFromObjectBuilder(
      MyObjectBuilder_EntityBase objectBuilder)
    {
      return (IMyEntity) MyEntities.CreateFromObjectBuilder(objectBuilder, false);
    }

    IMyEntity IMyEntities.CreateFromObjectBuilderAndAdd(
      MyObjectBuilder_EntityBase objectBuilder)
    {
      return (IMyEntity) MyEntities.CreateFromObjectBuilderAndAdd(objectBuilder, false);
    }

    void IMyEntities.RemoveEntity(IMyEntity entity) => MyEntities.Remove(entity as MyEntity);

    private Action<MyEntity> GetDelegate(Action<IMyEntity> value) => (Action<MyEntity>) Delegate.CreateDelegate(typeof (Action<MyEntity>), value.Target, value.Method);

    private Action<MyEntity, string, string> GetDelegate(
      Action<IMyEntity, string, string> value)
    {
      return (Action<MyEntity, string, string>) Delegate.CreateDelegate(typeof (Action<MyEntity, string, string>), value.Target, value.Method);
    }

    event Action<IMyEntity> IMyEntities.OnEntityRemove
    {
      add => MyEntities.OnEntityRemove += this.GetDelegate(value);
      remove => MyEntities.OnEntityRemove -= this.GetDelegate(value);
    }

    event Action<IMyEntity> IMyEntities.OnEntityAdd
    {
      add => MyEntities.OnEntityAdd += this.GetDelegate(value);
      remove => MyEntities.OnEntityAdd -= this.GetDelegate(value);
    }

    event Action IMyEntities.OnCloseAll
    {
      add => MyEntities.OnCloseAll += value;
      remove => MyEntities.OnCloseAll -= value;
    }

    event Action<IMyEntity, string, string> IMyEntities.OnEntityNameSet
    {
      add => MyEntities.OnEntityNameSet += this.GetDelegate(value);
      remove => MyEntities.OnEntityNameSet -= this.GetDelegate(value);
    }

    bool IMyEntities.IsSpherePenetrating(ref BoundingSphereD bs) => MyEntities.IsSpherePenetrating(ref bs);

    Vector3D? IMyEntities.FindFreePlace(
      Vector3D basePos,
      float radius,
      int maxTestCount,
      int testsPerDistance,
      float stepSize)
    {
      return MyEntities.FindFreePlace(basePos, radius, maxTestCount, testsPerDistance, stepSize);
    }

    [Obsolete]
    void IMyEntities.GetInflatedPlayerBoundingBox(
      ref BoundingBox playerBox,
      float inflation)
    {
      BoundingBoxD invalid = BoundingBoxD.CreateInvalid();
      MyEntities.GetInflatedPlayerBoundingBox(ref invalid, inflation);
      playerBox = (BoundingBox) invalid;
    }

    void IMyEntities.GetInflatedPlayerBoundingBox(
      ref BoundingBoxD playerBox,
      float inflation)
    {
      MyEntities.GetInflatedPlayerBoundingBox(ref playerBox, inflation);
    }

    [Obsolete]
    bool IMyEntities.IsInsideVoxel(
      Vector3 pos,
      Vector3 hintPosition,
      out Vector3 lastOutsidePos)
    {
      Vector3D lastOutsidePos1;
      int num = MyEntities.IsInsideVoxel((Vector3D) pos, (Vector3D) hintPosition, out lastOutsidePos1) ? 1 : 0;
      lastOutsidePos = (Vector3) lastOutsidePos1;
      return num != 0;
    }

    bool IMyEntities.IsInsideVoxel(
      Vector3D pos,
      Vector3D hintPosition,
      out Vector3D lastOutsidePos)
    {
      return MyEntities.IsInsideVoxel(pos, hintPosition, out lastOutsidePos);
    }

    bool IMyEntities.IsWorldLimited() => MyEntities.IsWorldLimited();

    float IMyEntities.WorldHalfExtent() => MyEntities.WorldHalfExtent();

    float IMyEntities.WorldSafeHalfExtent() => MyEntities.WorldSafeHalfExtent();

    bool IMyEntities.IsInsideWorld(Vector3D pos) => MyEntities.IsInsideWorld(pos);

    bool IMyEntities.IsRaycastBlocked(Vector3D pos, Vector3D target) => MyEntities.IsRaycastBlocked(pos, target);

    List<IMyEntity> IMyEntities.GetEntitiesInAABB(
      ref BoundingBoxD boundingBox)
    {
      List<MyEntity> entitiesInAabb = MyEntities.GetEntitiesInAABB(ref boundingBox);
      List<IMyEntity> myEntityList = new List<IMyEntity>(entitiesInAabb.Count);
      foreach (MyEntity myEntity in entitiesInAabb)
        myEntityList.Add((IMyEntity) myEntity);
      entitiesInAabb.Clear();
      return myEntityList;
    }

    List<IMyEntity> IMyEntities.GetEntitiesInSphere(
      ref BoundingSphereD boundingSphere)
    {
      List<MyEntity> entitiesInSphere = MyEntities.GetEntitiesInSphere(ref boundingSphere);
      List<IMyEntity> myEntityList = new List<IMyEntity>(entitiesInSphere.Count);
      foreach (MyEntity myEntity in entitiesInSphere)
        myEntityList.Add((IMyEntity) myEntity);
      entitiesInSphere.Clear();
      return myEntityList;
    }

    List<IMyEntity> IMyEntities.GetTopMostEntitiesInSphere(
      ref BoundingSphereD boundingSphere)
    {
      List<MyEntity> entitiesInSphere = MyEntities.GetTopMostEntitiesInSphere(ref boundingSphere);
      List<IMyEntity> myEntityList = new List<IMyEntity>(entitiesInSphere.Count);
      foreach (MyEntity myEntity in entitiesInSphere)
        myEntityList.Add((IMyEntity) myEntity);
      entitiesInSphere.Clear();
      return myEntityList;
    }

    List<IMyEntity> IMyEntities.GetElementsInBox(
      ref BoundingBoxD boundingBox)
    {
      this.m_entityList.Clear();
      MyEntities.GetElementsInBox(ref boundingBox, this.m_entityList);
      List<IMyEntity> myEntityList = new List<IMyEntity>(this.m_entityList.Count);
      foreach (MyEntity entity in this.m_entityList)
        myEntityList.Add((IMyEntity) entity);
      return myEntityList;
    }

    List<IMyEntity> IMyEntities.GetTopMostEntitiesInBox(
      ref BoundingBoxD boundingBox)
    {
      this.m_entityList.Clear();
      MyEntities.GetTopMostEntitiesInBox(ref boundingBox, this.m_entityList);
      List<IMyEntity> myEntityList = new List<IMyEntity>(this.m_entityList.Count);
      foreach (MyEntity entity in this.m_entityList)
        myEntityList.Add((IMyEntity) entity);
      return myEntityList;
    }

    IMyEntity IMyEntities.CreateFromObjectBuilderParallel(
      MyObjectBuilder_EntityBase objectBuilder,
      bool addToScene,
      Action<IMyEntity> completionCallback)
    {
      return (IMyEntity) MyEntities.CreateFromObjectBuilderParallel(objectBuilder, addToScene, (Action<MyEntity>) completionCallback);
    }

    void IMyEntities.SetEntityName(IMyEntity entity, bool possibleRename)
    {
      if (!(entity is MyEntity))
        return;
      MyEntities.SetEntityName(entity as MyEntity, possibleRename);
    }

    bool IMyEntities.IsNameExists(IMyEntity entity, string name) => entity is MyEntity && MyEntities.IsNameExists(entity as MyEntity, name);

    void IMyEntities.RemoveFromClosedEntities(IMyEntity entity)
    {
      if (!(entity is MyEntity))
        return;
      MyEntities.RemoveFromClosedEntities(entity as MyEntity);
    }

    void IMyEntities.RemoveName(IMyEntity entity)
    {
      if (string.IsNullOrEmpty(entity.Name))
        return;
      MyEntities.m_entityNameDictionary.Remove<string, MyEntity>(entity.Name);
    }

    bool IMyEntities.Exist(IMyEntity entity) => entity is MyEntity && MyEntities.Exist(entity as MyEntity);

    void IMyEntities.MarkForClose(IMyEntity entity)
    {
      if (!(entity is MyEntity))
        return;
      MyEntities.Close(entity as MyEntity);
    }

    void IMyEntities.RegisterForUpdate(IMyEntity entity)
    {
      MyEntity e = entity as MyEntity;
      if (e == null)
        return;
      if (Thread.CurrentThread == MyUtils.MainThread)
      {
        MyEntities.RegisterForUpdate(e);
      }
      else
      {
        MyVRage.Platform.Scripting.ReportIncorrectBehaviour(MyCommonTexts.ModRuleViolation_EngineParallelAccess);
        MySandboxGame.Static.Invoke((Action) (() => MyEntities.RegisterForUpdate(e)), "RegisterForUpdate");
      }
    }

    void IMyEntities.RegisterForDraw(IMyEntity entity)
    {
      if (!(entity is MyEntity myEntity))
        return;
      MyEntities.RegisterForDraw((IMyEntity) myEntity);
    }

    void IMyEntities.UnregisterForUpdate(IMyEntity entity, bool immediate)
    {
      MyEntity e = entity as MyEntity;
      if (e == null)
        return;
      if (Thread.CurrentThread == MyUtils.MainThread)
      {
        MyEntities.UnregisterForUpdate(e, immediate);
      }
      else
      {
        MyVRage.Platform.Scripting.ReportIncorrectBehaviour(MyCommonTexts.ModRuleViolation_EngineParallelAccess);
        MySandboxGame.Static.Invoke((Action) (() => MyEntities.UnregisterForUpdate(e, immediate)), "UnregisterForUpdate");
      }
    }

    void IMyEntities.UnregisterForDraw(IMyEntity entity)
    {
      if (!(entity is MyEntity myEntity))
        return;
      MyEntities.UnregisterForDraw((IMyEntity) myEntity);
    }

    IMyEntity IMyEntities.GetIntersectionWithSphere(ref BoundingSphereD sphere) => (IMyEntity) MyEntities.GetIntersectionWithSphere(ref sphere);

    IMyEntity IMyEntities.GetIntersectionWithSphere(
      ref BoundingSphereD sphere,
      IMyEntity ignoreEntity0,
      IMyEntity ignoreEntity1)
    {
      return (IMyEntity) MyEntities.GetIntersectionWithSphere(ref sphere, ignoreEntity0 as MyEntity, ignoreEntity1 as MyEntity);
    }

    List<IMyEntity> IMyEntities.GetIntersectionWithSphere(
      ref BoundingSphereD sphere,
      IMyEntity ignoreEntity0,
      IMyEntity ignoreEntity1,
      bool ignoreVoxelMaps,
      bool volumetricTest)
    {
      this.m_entityList.Clear();
      MyEntities.GetIntersectionWithSphere(ref sphere, ignoreEntity0 as MyEntity, ignoreEntity1 as MyEntity, ignoreVoxelMaps, volumetricTest, ref this.m_entityList);
      List<IMyEntity> myEntityList = new List<IMyEntity>(this.m_entityList.Count);
      foreach (MyEntity entity in this.m_entityList)
        myEntityList.Add((IMyEntity) entity);
      return myEntityList;
    }

    IMyEntity IMyEntities.GetIntersectionWithSphere(
      ref BoundingSphereD sphere,
      IMyEntity ignoreEntity0,
      IMyEntity ignoreEntity1,
      bool ignoreVoxelMaps,
      bool volumetricTest,
      bool excludeEntitiesWithDisabledPhysics,
      bool ignoreFloatingObjects,
      bool ignoreHandWeapons)
    {
      return (IMyEntity) MyEntities.GetIntersectionWithSphere(ref sphere, ignoreEntity0 as MyEntity, ignoreEntity1 as MyEntity, ignoreVoxelMaps, volumetricTest, excludeEntitiesWithDisabledPhysics, ignoreFloatingObjects, ignoreHandWeapons);
    }

    IMyEntity IMyEntities.GetEntityById(long entityId) => !MyEntities.EntityExists(entityId) ? (IMyEntity) null : (IMyEntity) MyEntities.GetEntityById(entityId);

    IMyEntity IMyEntities.GetEntityById(long? entityId) => !entityId.HasValue ? (IMyEntity) null : (IMyEntity) MyEntities.GetEntityById(entityId.Value);

    bool IMyEntities.EntityExists(long entityId) => MyEntities.EntityExists(entityId);

    bool IMyEntities.EntityExists(long? entityId) => entityId.HasValue && MyEntities.EntityExists(entityId.Value);

    IMyEntity IMyEntities.GetEntityByName(string name) => (IMyEntity) MyEntities.GetEntityByName(name);

    void IMyEntities.SetTypeHidden(Type type, bool hidden) => MyEntities.SetTypeHidden(type, hidden);

    bool IMyEntities.IsTypeHidden(Type type) => MyEntities.IsTypeHidden(type);

    bool IMyEntities.IsVisible(IMyEntity entity) => ((IMyEntities) this).IsTypeHidden(entity.GetType());

    void IMyEntities.UnhideAllTypes() => MyEntities.UnhideAllTypes();

    void IMyEntities.RemapObjectBuilderCollection(
      IEnumerable<MyObjectBuilder_EntityBase> objectBuilders)
    {
      MyEntities.RemapObjectBuilderCollection(objectBuilders);
    }

    void IMyEntities.RemapObjectBuilder(MyObjectBuilder_EntityBase objectBuilder) => MyEntities.RemapObjectBuilder(objectBuilder);

    IMyEntity IMyEntities.CreateFromObjectBuilderNoinit(
      MyObjectBuilder_EntityBase objectBuilder)
    {
      return (IMyEntity) MyEntities.CreateFromObjectBuilderNoinit(objectBuilder);
    }

    void IMyEntities.EnableEntityBoundingBoxDraw(
      IMyEntity entity,
      bool enable,
      Vector4? color,
      float lineWidth,
      Vector3? inflateAmount)
    {
      if (!(entity is MyEntity))
        return;
      if (Thread.CurrentThread == MyUtils.MainThread)
      {
        MyEntities.EnableEntityBoundingBoxDraw(entity as MyEntity, enable, color, lineWidth, inflateAmount);
      }
      else
      {
        MyVRage.Platform.Scripting.ReportIncorrectBehaviour(MyCommonTexts.ModRuleViolation_EngineParallelAccess);
        MySandboxGame.Static.Invoke((Action) (() => MyEntities.EnableEntityBoundingBoxDraw(entity as MyEntity, enable, color, lineWidth, inflateAmount)), "EnableEntityBoundingBoxDraw");
      }
    }

    IMyEntity IMyEntities.GetEntity(Func<IMyEntity, bool> match)
    {
      foreach (MyEntity entity in MyEntities.GetEntities())
      {
        if (match((IMyEntity) entity))
          return (IMyEntity) entity;
      }
      return (IMyEntity) null;
    }
  }
}
