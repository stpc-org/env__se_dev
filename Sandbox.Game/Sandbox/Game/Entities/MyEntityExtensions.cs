// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyEntityExtensions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World.Generator;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.Entities
{
  public static class MyEntityExtensions
  {
    internal static void SetCallbacks()
    {
      MyEntity.AddToGamePruningStructureExtCallBack = new Action<MyEntity>(MyEntityExtensions.AddToGamePruningStructure);
      MyEntity.RemoveFromGamePruningStructureExtCallBack = new Action<MyEntity>(MyEntityExtensions.RemoveFromGamePruningStructure);
      MyEntity.UpdateGamePruningStructureExtCallBack = new Action<MyEntity>(MyEntityExtensions.UpdateGamePruningStructure);
      MyEntity.MyEntityFactoryCreateObjectBuilderExtCallback = new MyEntity.MyEntityFactoryCreateObjectBuilderDelegate(MyEntityExtensions.EntityFactoryCreateObjectBuilder);
      MyEntity.CreateDefaultSyncEntityExtCallback = new MyEntity.CreateDefaultSyncEntityDelegate(MyEntityExtensions.CreateDefaultSyncEntity);
      MyEntity.MyWeldingGroupsGetGroupNodesExtCallback = new Action<MyEntity, List<MyEntity>>(MyEntityExtensions.GetWeldingGroupNodes);
      MyEntity.MyProceduralWorldGeneratorTrackEntityExtCallback = new Action<MyEntity>(MyEntityExtensions.ProceduralWorldGeneratorTrackEntity);
      MyEntity.CreateStandardRenderComponentsExtCallback = new Action<MyEntity>(MyEntityExtensions.CreateStandardRenderComponents);
      MyEntity.InitComponentsExtCallback = new Action<MyComponentContainer, MyObjectBuilderType, MyStringHash, MyObjectBuilder_ComponentContainer>(MyComponentContainerExtension.InitComponents);
      MyEntity.MyEntitiesCreateFromObjectBuilderExtCallback = new Func<MyObjectBuilder_EntityBase, bool, MyEntity>(MyEntities.CreateFromObjectBuilder);
    }

    public static MyPhysicsBody GetPhysicsBody(this MyEntity thisEntity) => thisEntity.Physics as MyPhysicsBody;

    public static void UpdateGamePruningStructure(this MyEntity thisEntity) => MyGamePruningStructure.Move(thisEntity);

    public static void AddToGamePruningStructure(this MyEntity thisEntity) => MyGamePruningStructure.Add(thisEntity);

    public static void RemoveFromGamePruningStructure(this MyEntity thisEntity) => MyGamePruningStructure.Remove(thisEntity);

    public static MyObjectBuilder_EntityBase EntityFactoryCreateObjectBuilder(
      this MyEntity thisEntity)
    {
      return MyEntityFactory.CreateObjectBuilder(thisEntity);
    }

    public static MySyncComponentBase CreateDefaultSyncEntity(
      this MyEntity thisEntity)
    {
      return (MySyncComponentBase) new MySyncEntity(thisEntity);
    }

    public static void AddNodeToWeldingGroups(this MyEntity thisEntity) => MyWeldingGroups.Static.AddNode(thisEntity);

    public static void RemoveNodeFromWeldingGroups(this MyEntity thisEntity) => MyWeldingGroups.Static.RemoveNode(thisEntity);

    public static void GetWeldingGroupNodes(this MyEntity thisEntity, List<MyEntity> result) => MyWeldingGroups.Static.GetGroupNodes(thisEntity, result);

    public static bool WeldingGroupExists(this MyEntity thisEntity) => MyWeldingGroups.Static.GetGroup(thisEntity) != null;

    public static void ProceduralWorldGeneratorTrackEntity(this MyEntity thisEntity)
    {
      if (!MyFakes.ENABLE_ASTEROID_FIELDS || MyProceduralWorldGenerator.Static == null)
        return;
      MyProceduralWorldGenerator.Static.TrackEntity(thisEntity);
    }

    public static bool TryGetInventory(this MyEntity thisEntity, out MyInventoryBase inventoryBase)
    {
      inventoryBase = (MyInventoryBase) null;
      return thisEntity.Components.TryGet<MyInventoryBase>(out inventoryBase);
    }

    public static bool TryGetInventory(this MyEntity thisEntity, out MyInventory inventory)
    {
      inventory = (MyInventory) null;
      if (thisEntity.Components.Has<MyInventoryBase>())
        inventory = MyEntityExtensions.GetInventory(thisEntity);
      return inventory != null;
    }

    public static MyInventory GetInventory(this MyEntity thisEntity, int index = 0) => thisEntity.GetInventoryBase(index) as MyInventory;

    internal static void CreateStandardRenderComponents(this MyEntity thisEntity)
    {
      thisEntity.Render = (MyRenderComponentBase) new MyRenderComponent();
      thisEntity.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponent((IMyEntity) thisEntity));
    }
  }
}
