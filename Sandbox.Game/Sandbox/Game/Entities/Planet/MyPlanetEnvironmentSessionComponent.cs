// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Planet.MyPlanetEnvironmentSessionComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment;
using System;
using System.Collections.Generic;
using System.Threading;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Planet
{
  [StaticEventOwner]
  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, 500)]
  public class MyPlanetEnvironmentSessionComponent : MySessionComponentBase
  {
    private const int TIME_TO_UPDATE = 10;
    private const int UPDATES_TO_LAZY_UPDATE = 10;
    private int m_updateInterval;
    private int m_lazyUpdateInterval;
    public static bool EnableUpdate = true;
    public static bool DebugDrawSectors = false;
    public static bool DebugDrawDynamicObjectClusters = false;
    public static bool DebugDrawEnvironmentProviders = false;
    public static bool DebugDrawActiveSectorItems = false;
    public static bool DebugDrawActiveSectorProvider = false;
    public static bool DebugDrawProxies = false;
    public static bool DebugDrawCollisionCheckers = false;
    public static float DebugDrawDistance = 150f;
    private readonly HashSet<IMyEnvironmentDataProvider> m_environmentProviders = new HashSet<IMyEnvironmentDataProvider>();
    private readonly HashSet<MyPlanetEnvironmentComponent> m_planetEnvironments = new HashSet<MyPlanetEnvironmentComponent>();
    public static MyEnvironmentSector ActiveSector;
    private const int NewEnvReleaseVersion = 1133002;
    private MyListDictionary<MyEntity, BoundingBoxD> m_cubeBlocksToWork = new MyListDictionary<MyEntity, BoundingBoxD>();
    private volatile MyListDictionary<MyEntity, BoundingBoxD> m_cubeBlocksPending = new MyListDictionary<MyEntity, BoundingBoxD>();
    private volatile bool m_itemDisableJobRunning;
    private List<MyVoxelBase> m_tmpVoxelList = new List<MyVoxelBase>();
    private List<MyEntity> m_tmpEntityList = new List<MyEntity>();
    private MyListDictionary<MyEnvironmentSector, int> m_itemsToDisable = new MyListDictionary<MyEnvironmentSector, int>();

    public override Type[] Dependencies => new Type[1]
    {
      typeof (MyCubeGrids)
    };

    public override bool UpdatedBeforeInit() => true;

    public override void UpdateBeforeSimulation()
    {
      if (!MyPlanetEnvironmentSessionComponent.EnableUpdate)
        return;
      ++this.m_updateInterval;
      if (this.m_updateInterval <= 10)
        return;
      this.m_updateInterval = 0;
      ++this.m_lazyUpdateInterval;
      bool doLazy = false;
      if (this.m_lazyUpdateInterval > 10)
      {
        doLazy = true;
        this.m_lazyUpdateInterval = 0;
      }
      this.UpdatePlanetEnvironments(doLazy);
      if (this.m_itemDisableJobRunning || this.m_cubeBlocksPending.Count<KeyValuePair<MyEntity, List<BoundingBoxD>>>() <= 0)
        return;
      Parallel.Start(new Action(this.GatherEnvItemsInBoxes), new Action(this.DisableGatheredItems));
      this.m_itemDisableJobRunning = true;
    }

    public override void Draw()
    {
      if (MyPlanetEnvironmentSessionComponent.DebugDrawEnvironmentProviders)
      {
        foreach (IMyEnvironmentDataProvider environmentProvider in this.m_environmentProviders)
          environmentProvider.DebugDraw();
      }
      MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(MySector.MainCamera.Position);
      if (!MyPlanetEnvironmentSessionComponent.DebugDrawSectors || closestPlanet == null)
        return;
      MyPlanetEnvironmentSessionComponent.ActiveSector = closestPlanet.Components.Get<MyPlanetEnvironmentComponent>().GetSectorForPosition(MySector.MainCamera.Position);
    }

    public override void LoadData()
    {
      base.LoadData();
      int num = Sync.IsServer ? 1 : 0;
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      int num = Sync.IsServer ? 1 : 0;
    }

    public void RegisterPlanetEnvironment(MyPlanetEnvironmentComponent env)
    {
      this.m_planetEnvironments.Add(env);
      foreach (IMyEnvironmentDataProvider provider in env.Providers)
        this.m_environmentProviders.Add(provider);
    }

    public void UnregisterPlanetEnvironment(MyPlanetEnvironmentComponent env)
    {
      this.m_planetEnvironments.Remove(env);
      foreach (IMyEnvironmentDataProvider provider in env.Providers)
        this.m_environmentProviders.Remove(provider);
    }

    private void UpdatePlanetEnvironments(bool doLazy)
    {
      foreach (MyPlanetEnvironmentComponent planetEnvironment in this.m_planetEnvironments)
        planetEnvironment.Update(doLazy);
    }

    public override void BeforeStart()
    {
      if (MySession.Static.AppVersionFromSave >= 1133002)
        return;
      foreach (MyPlanetEnvironmentComponent planetEnvironment in this.m_planetEnvironments)
        planetEnvironment.InitClearAreasManagement();
    }

    private void OnEntityAdded(MyEntity myEntity)
    {
      int num = MySession.Static.Ready ? 1 : 0;
    }

    private void MyCubeGridsOnBlockBuilt(MyCubeGrid myCubeGrid, MySlimBlock mySlimBlock)
    {
      if (mySlimBlock == null || !myCubeGrid.IsStatic)
        return;
      MySlimBlock cubeBlock = myCubeGrid.GetCubeBlock(mySlimBlock.Min);
      if (cubeBlock != null && cubeBlock.FatBlock is MyCompoundCubeBlock fatBlock && mySlimBlock.FatBlock != fatBlock)
        return;
      BoundingBoxD aabb;
      mySlimBlock.GetWorldBoundingBox(out aabb, true);
      this.m_cubeBlocksPending.Add((MyEntity) myCubeGrid, aabb);
    }

    public void ClearEnvironmentItems(MyEntity entity, BoundingBoxD worldBBox)
    {
      if (!Sync.IsServer)
        MyLog.Default.Error("This method can be used only on server.");
      else
        this.m_cubeBlocksPending.Add(entity, worldBBox);
    }

    private void GatherEnvItemsInBoxes()
    {
      MyListDictionary<MyEntity, BoundingBoxD> myListDictionary = Interlocked.Exchange<MyListDictionary<MyEntity, BoundingBoxD>>(ref this.m_cubeBlocksPending, this.m_cubeBlocksToWork);
      this.m_cubeBlocksToWork = myListDictionary;
      int num1 = 0;
      int num2 = 0;
      foreach (List<BoundingBoxD> boundingBoxDList in myListDictionary.Values)
      {
        for (int index1 = 0; index1 < boundingBoxDList.Count; ++index1)
        {
          BoundingBoxD boundingBoxD = boundingBoxDList[index1];
          MyGamePruningStructure.GetAllVoxelMapsInBox(ref boundingBoxD, this.m_tmpVoxelList);
          ++num2;
          for (int index2 = 0; index2 < this.m_tmpVoxelList.Count; ++index2)
          {
            if (this.m_tmpVoxelList[index2] is MyPlanet tmpVoxel)
            {
              tmpVoxel.Hierarchy.QueryAABB(ref boundingBoxD, this.m_tmpEntityList);
              for (int index3 = 0; index3 < this.m_tmpEntityList.Count; ++index3)
              {
                if (!(this.m_tmpEntityList[index3] is MyEnvironmentSector tmpEntity))
                  return;
                BoundingBoxD aabb = boundingBoxD;
                tmpEntity.GetItemsInAabb(ref aabb, this.m_itemsToDisable.GetOrAdd(tmpEntity));
                if (tmpEntity.DataView != null && tmpEntity.DataView.Items != null)
                  num1 += tmpEntity.DataView.Items.Count;
              }
              this.m_tmpEntityList.Clear();
            }
          }
          this.m_tmpVoxelList.Clear();
        }
      }
      myListDictionary.Clear();
    }

    public void DisableGatheredItems()
    {
      foreach (KeyValuePair<MyEnvironmentSector, List<int>> keyValuePair in (MyCollectionDictionary<MyEnvironmentSector, List<int>, int>) this.m_itemsToDisable)
      {
        for (int index = 0; index < keyValuePair.Value.Count; ++index)
          keyValuePair.Key.EnableItem(keyValuePair.Value[index], false);
        if (keyValuePair.Value.Count > 0)
          MyMultiplayer.RaiseStaticEvent<long, long, List<int>>((Func<IMyEventOwner, Action<long, long, List<int>>>) (x => new Action<long, long, List<int>>(MyPlanetEnvironmentSessionComponent.DisableItemsInSector)), keyValuePair.Key.Owner.Entity.EntityId, keyValuePair.Key.SectorId, keyValuePair.Value);
      }
      this.m_itemsToDisable.Clear();
      this.m_itemDisableJobRunning = false;
    }

    [Broadcast]
    [Event(null, 314)]
    [Reliable]
    public static void DisableItemsInSector(long planetId, long sectorId, List<int> items)
    {
      MyPlanet entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyPlanet>(planetId, out entity))
        return;
      MyPlanetEnvironmentComponent environmentComponent = entity.Components.Get<MyPlanetEnvironmentComponent>();
      MyEnvironmentSector environmentSector;
      if (environmentComponent == null || !environmentComponent.TryGetSector(sectorId, out environmentSector))
        return;
      for (int index = 0; index < items.Count; ++index)
        environmentSector.EnableItem(items[index], false);
    }

    protected sealed class DisableItemsInSector\u003C\u003ESystem_Int64\u0023System_Int64\u0023System_Collections_Generic_List`1\u003CSystem_Int32\u003E : ICallSite<IMyEventOwner, long, long, List<int>, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long planetId,
        in long sectorId,
        in List<int> items,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyPlanetEnvironmentSessionComponent.DisableItemsInSector(planetId, sectorId, items);
      }
    }
  }
}
