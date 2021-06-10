// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.MyProceduralEnvironmentProvider
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment.Definitions;
using Sandbox.Game.WorldEnvironment.Modules;
using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.WorldEnvironment
{
  public class MyProceduralEnvironmentProvider : IMyEnvironmentDataProvider
  {
    private readonly FastResourceLock m_sectorsLock = new FastResourceLock();
    private readonly Dictionary<long, MyProceduralLogicalSector> m_sectors = new Dictionary<long, MyProceduralLogicalSector>();
    private readonly Dictionary<long, MyObjectBuilder_ProceduralEnvironmentSector> m_savedSectors = new Dictionary<long, MyObjectBuilder_ProceduralEnvironmentSector>();
    private volatile bool m_sectorsQueued;
    private readonly MyConcurrentQueue<MyProceduralLogicalSector> m_sectorsToRaise = new MyConcurrentQueue<MyProceduralLogicalSector>();
    private readonly MyConcurrentQueue<MyProceduralLogicalSector> m_sectorsToDestroy = new MyConcurrentQueue<MyProceduralLogicalSector>();
    private readonly MyConcurrentHashSet<MyProceduralLogicalSector> m_sectorsForReplication = new MyConcurrentHashSet<MyProceduralLogicalSector>();
    public int LodFactor = 3;
    private Vector3D m_origin;
    private Vector3D m_basisX;
    private Vector3D m_basisY;
    private double m_sectorSize;
    private readonly Action m_raiseCallback;

    public IMyEnvironmentOwner Owner { get; private set; }

    public int ProviderId { get; set; }

    internal int SyncLod => this.Owner.EnvironmentDefinition.SyncLod;

    public MyProceduralEnvironmentProvider() => this.m_raiseCallback = new Action(this.RaiseLogicalSectors);

    public void Init(
      IMyEnvironmentOwner owner,
      ref Vector3D origin,
      ref Vector3D basisA,
      ref Vector3D basisB,
      double sectorSize,
      MyObjectBuilder_Base ob)
    {
      this.Owner = owner;
      this.m_sectorSize = sectorSize;
      this.m_origin = origin;
      this.m_basisX = basisA;
      this.m_basisY = basisB;
      if (!(ob is MyObjectBuilder_ProceduralEnvironmentProvider environmentProvider))
        return;
      for (int index = 0; index < environmentProvider.Sectors.Count; ++index)
      {
        MyObjectBuilder_ProceduralEnvironmentSector sector = environmentProvider.Sectors[index];
        this.m_savedSectors.Add(sector.SectorId, sector);
      }
    }

    private void RaiseLogicalSectors()
    {
      MyMultiplayerServerBase multiplayerServerBase = (MyMultiplayerServerBase) MyMultiplayer.Static;
      this.m_sectorsQueued = false;
      MyProceduralLogicalSector instance;
      while (this.m_sectorsToDestroy.TryDequeue(out instance))
        instance.Close();
      while (this.m_sectorsToRaise.TryDequeue(out instance))
        multiplayerServerBase?.RaiseReplicableCreated((object) instance);
    }

    private void QueueRaiseLogicalSector(MyProceduralLogicalSector sector)
    {
      if (!Sync.IsServer || !Sync.MultiplayerActive)
        return;
      this.m_sectorsToRaise.Enqueue(sector);
    }

    private void QueueDestroyLogicalSector(MyProceduralLogicalSector sector)
    {
      if (Sync.IsServer && Sync.MultiplayerActive)
        this.m_sectorsToDestroy.Enqueue(sector);
      else
        sector.Close();
    }

    public MyEnvironmentDataView GetItemView(
      int lod,
      ref Vector2I start,
      ref Vector2I end,
      ref Vector3D localOrigin)
    {
      int localLod = lod / this.LodFactor;
      int logicalLod = lod % this.LodFactor;
      start >>= localLod * this.LodFactor;
      end >>= localLod * this.LodFactor;
      MyProceduralDataView view = new MyProceduralDataView(this, lod, ref start, ref end);
      for (int y = start.Y; y <= end.Y; ++y)
      {
        for (int x = start.X; x <= end.X; ++x)
          this.GetLogicalSector(x, y, localLod).AddView(view, localOrigin, logicalLod);
      }
      if ((this.m_sectorsToRaise.Count > 0 || this.m_sectorsToRaise.Count > 0) && !this.m_sectorsQueued)
      {
        this.m_sectorsQueued = true;
        MySandboxGame.Static.Invoke(this.m_raiseCallback, "RaiseLogicalSectors");
      }
      return (MyEnvironmentDataView) view;
    }

    private MyProceduralLogicalSector GetLogicalSector(
      int x,
      int y,
      int localLod)
    {
      long key = MyPlanetSectorId.MakeSectorId(x, y, this.ProviderId, localLod);
      MyProceduralLogicalSector proceduralLogicalSector1;
      bool flag;
      using (this.m_sectorsLock.AcquireSharedUsing())
        flag = this.m_sectors.TryGetValue(key, out proceduralLogicalSector1);
      if (!flag)
      {
        using (this.m_sectorsLock.AcquireExclusiveUsing())
        {
          if (!this.m_sectors.TryGetValue(key, out proceduralLogicalSector1))
          {
            MyObjectBuilder_ProceduralEnvironmentSector moduleData;
            this.m_savedSectors.TryGetValue(key, out moduleData);
            MyProceduralLogicalSector proceduralLogicalSector2 = new MyProceduralLogicalSector(this, x, y, localLod, moduleData);
            proceduralLogicalSector2.Id = key;
            proceduralLogicalSector1 = proceduralLogicalSector2;
            proceduralLogicalSector1.OnViewerEmpty += new Action<MyProceduralLogicalSector>(this.CloseSector);
            this.m_sectors[key] = proceduralLogicalSector1;
          }
        }
      }
      return proceduralLogicalSector1;
    }

    public MyObjectBuilder_EnvironmentDataProvider GetObjectBuilder()
    {
      MyObjectBuilder_ProceduralEnvironmentProvider newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ProceduralEnvironmentProvider>();
      using (this.m_sectorsLock.AcquireSharedUsing())
      {
        foreach (KeyValuePair<long, MyProceduralLogicalSector> sector in this.m_sectors)
        {
          MyObjectBuilder_EnvironmentSector objectBuilder = sector.Value.GetObjectBuilder();
          if (objectBuilder != null)
            newObject.Sectors.Add((MyObjectBuilder_ProceduralEnvironmentSector) objectBuilder);
        }
        foreach (KeyValuePair<long, MyObjectBuilder_ProceduralEnvironmentSector> savedSector in this.m_savedSectors)
        {
          if (!this.m_sectors.ContainsKey(savedSector.Key))
            newObject.Sectors.Add(savedSector.Value);
        }
      }
      return (MyObjectBuilder_EnvironmentDataProvider) newObject;
    }

    public void DebugDraw()
    {
      float num = MyPlanetEnvironmentSessionComponent.DebugDrawDistance * MyPlanetEnvironmentSessionComponent.DebugDrawDistance;
      using (this.m_sectorsLock.AcquireSharedUsing())
      {
        foreach (MyProceduralLogicalSector proceduralLogicalSector in this.m_sectors.Values)
        {
          MyRenderProxy.DebugDraw6FaceConvex(proceduralLogicalSector.Bounds, Color.Violet, 0.5f, true, false);
          Vector3D vector3D = (proceduralLogicalSector.Bounds[4] + proceduralLogicalSector.Bounds[7]) / 2.0;
          if (Vector3D.DistanceSquared(vector3D, MySector.MainCamera.Position) < (double) num)
          {
            Vector3 vector3 = -MySector.MainCamera.UpVector * 3f;
            MyRenderProxy.DebugDrawText3D(vector3D + vector3, proceduralLogicalSector.ToString(), Color.Violet, 1f, true);
          }
        }
      }
    }

    public IEnumerable<MyLogicalEnvironmentSectorBase> LogicalSectors => (IEnumerable<MyLogicalEnvironmentSectorBase>) this.m_sectorsForReplication;

    public MyLogicalEnvironmentSectorBase GetLogicalSector(
      long sectorId)
    {
      using (this.m_sectorsLock.AcquireSharedUsing())
      {
        MyProceduralLogicalSector proceduralLogicalSector;
        this.m_sectors.TryGetValue(sectorId, out proceduralLogicalSector);
        return (MyLogicalEnvironmentSectorBase) proceduralLogicalSector;
      }
    }

    public void CloseView(MyProceduralDataView view)
    {
      int lod = view.Lod / this.LodFactor;
      for (int y = view.Start.Y; y <= view.End.Y; ++y)
      {
        for (int x = view.Start.X; x <= view.End.X; ++x)
        {
          long key = MyPlanetSectorId.MakeSectorId(x, y, this.ProviderId, lod);
          MyProceduralLogicalSector sector;
          using (this.m_sectorsLock.AcquireSharedUsing())
            sector = this.m_sectors[key];
          sector.RemoveView(view);
        }
      }
    }

    private void CloseSector(MyProceduralLogicalSector sector)
    {
      if (sector.ServerOwned)
        return;
      sector.OnViewerEmpty -= new Action<MyProceduralLogicalSector>(this.CloseSector);
      this.SaveLogicalSector(sector);
      using (this.m_sectorsLock.AcquireExclusiveUsing())
        this.m_sectors.Remove(sector.Id);
      if (sector.Replicable)
        this.UnmarkReplicable(sector);
      this.QueueDestroyLogicalSector(sector);
    }

    private void SaveLogicalSector(MyProceduralLogicalSector sector)
    {
      MyObjectBuilder_EnvironmentSector objectBuilder = sector.GetObjectBuilder();
      if (objectBuilder == null)
        this.m_savedSectors.Remove(sector.Id);
      else
        this.m_savedSectors[sector.Id] = (MyObjectBuilder_ProceduralEnvironmentSector) objectBuilder;
    }

    public MyProceduralLogicalSector TryGetLogicalSector(
      int lod,
      int logicalx,
      int logicaly)
    {
      using (this.m_sectorsLock.AcquireSharedUsing())
      {
        MyProceduralLogicalSector proceduralLogicalSector;
        this.m_sectors.TryGetValue(MyPlanetSectorId.MakeSectorId(logicalx, logicaly, this.ProviderId, lod), out proceduralLogicalSector);
        return proceduralLogicalSector;
      }
    }

    public void GeSectorWorldParameters(
      int x,
      int y,
      int localLod,
      out Vector3D worldPos,
      out Vector3 scanBasisA,
      out Vector3 scanBasisB)
    {
      double num = (double) (1 << localLod) * this.m_sectorSize;
      worldPos = this.m_origin + this.m_basisX * (((double) x + 0.5) * num) + this.m_basisY * (((double) y + 0.5) * num);
      scanBasisA = (Vector3) (this.m_basisX * (num * 0.5));
      scanBasisB = (Vector3) (this.m_basisY * (num * 0.5));
    }

    public int GetSeed() => this.Owner.GetSeed();

    internal void MarkReplicable(MyProceduralLogicalSector sector)
    {
      this.m_sectorsForReplication.Add(sector);
      this.QueueRaiseLogicalSector(sector);
      sector.Replicable = true;
      if (Sync.IsServer)
        return;
      MyMultiplayer.RaiseStaticEvent<long, long>((Func<IMyEventOwner, Action<long, long>>) (s => new Action<long, long>(MyClientState.AddKnownSector)), this.Owner.Entity.EntityId, sector.Id);
    }

    internal void UnmarkReplicable(MyProceduralLogicalSector sector)
    {
      this.m_sectorsForReplication.Remove(sector);
      sector.Replicable = false;
      if (Sync.IsServer)
        return;
      MyMultiplayer.RaiseStaticEvent<long, long>((Func<IMyEventOwner, Action<long, long>>) (s => new Action<long, long>(MyClientState.RemoveKnownSector)), this.Owner.Entity.EntityId, sector.Id);
    }

    public void RevalidateItem(long sectorId, int itemId)
    {
      MyObjectBuilder_ProceduralEnvironmentSector environmentSector;
      if (!this.m_savedSectors.TryGetValue(sectorId, out environmentSector))
        return;
      for (int index = 0; index < environmentSector.SavedModules.Length && !(MyDefinitionManager.Static.GetDefinition<MyProceduralEnvironmentModuleDefinition>((MyDefinitionId) environmentSector.SavedModules[index].ModuleId).ModuleType != typeof (MyMemoryEnvironmentModule)); ++index)
      {
        if (environmentSector.SavedModules[index].Builder is MyObjectBuilder_DummyEnvironmentModule builder && builder.DisabledItems.Contains(itemId))
          builder.DisabledItems.Remove(itemId);
      }
    }
  }
}
