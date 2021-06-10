// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.MyVoxelReplicable
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication.StateGroups;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Voxels;
using VRage.Library.Collections;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Replication
{
  internal class MyVoxelReplicable : MyEntityReplicableBaseEvent<MyVoxelBase>, IMyStreamableReplicable
  {
    private List<MyEntity> m_entities;
    private Action<MyVoxelBase> m_loadingDoneHandler;
    private MyStreamingEntityStateGroup<MyVoxelReplicable> m_streamingGroup;

    private MyVoxelBase Voxel => this.Instance;

    public override bool IncludeInIslands => false;

    protected override void OnHook()
    {
      base.OnHook();
      if (!Sync.IsServer)
        return;
      MyReplicationServer server = MyMultiplayer.GetReplicationServer();
      if (server == null)
        return;
      this.Voxel.RangeChanged += (MyVoxelBase.StorageChanged) ((_param1, _param2, _param3, _param4) => server.InvalidateClientCache((IMyReplicable) this, this.Voxel.StorageName));
    }

    public override bool ShouldReplicate(MyClientInfo client) => this.Voxel != null && this.Voxel.Storage != null && !this.Voxel.Closed && (this.Voxel is MyPlanet || this.Voxel.Save || (this.Voxel.ContentChanged || this.Voxel.BeforeContentChanged));

    public override bool OnSave(BitStream stream, Endpoint clientEndpoint) => false;

    protected override void OnLoad(BitStream stream, Action<MyVoxelBase> loadingDoneHandler)
    {
      bool andRead1 = MySerializer.CreateAndRead<bool>(stream);
      bool andRead2 = MySerializer.CreateAndRead<bool>(stream);
      int num = MySerializer.CreateAndRead<bool>(stream) ? 1 : 0;
      bool andRead3 = MySerializer.CreateAndRead<bool>(stream);
      byte[] memoryBuffer = (byte[]) null;
      string asteroid = (string) null;
      if (num != 0)
        memoryBuffer = MySerializer.CreateAndRead<byte[]>(stream);
      else if (andRead1)
        asteroid = MySerializer.CreateAndRead<string>(stream);
      MyLog.Default.WriteLine("MyVoxelReplicable.OnLoad - isUserCreated:" + andRead1.ToString() + " isFromPrefab:" + andRead2.ToString() + " contentChanged:" + andRead3.ToString() + " data?: " + (memoryBuffer != null).ToString());
      MyVoxelBase objectBuilderNoinit;
      if (andRead2)
      {
        MyObjectBuilder_EntityBase andRead4 = MySerializer.CreateAndRead<MyObjectBuilder_EntityBase>(stream, MyObjectBuilderSerializer.Dynamic);
        if (memoryBuffer != null)
        {
          bool isOldFormat = false;
          IMyStorage storage = (IMyStorage) MyStorageBase.Load(memoryBuffer, out isOldFormat);
          if (MyEntities.TryGetEntityById<MyVoxelBase>(andRead4.EntityId, out objectBuilderNoinit))
          {
            switch (objectBuilderNoinit)
            {
              case MyVoxelMap _:
                objectBuilderNoinit.Storage = storage;
                break;
              case MyPlanet _:
                MyPlanet myPlanet1 = objectBuilderNoinit as MyPlanet;
                myPlanet1.Storage = storage;
                myPlanet1.VoxelStorageUpdated();
                break;
            }
          }
          else
          {
            objectBuilderNoinit = (MyVoxelBase) MyEntities.CreateFromObjectBuilderNoinit(andRead4);
            if (objectBuilderNoinit is MyVoxelMap)
              objectBuilderNoinit.Init(andRead4, storage);
            else if (objectBuilderNoinit is MyPlanet)
            {
              MyPlanet myPlanet2 = objectBuilderNoinit as MyPlanet;
              myPlanet2.Init(andRead4, storage);
              myPlanet2.VoxelStorageUpdated();
            }
            if (objectBuilderNoinit != null)
              MyEntities.Add((MyEntity) objectBuilderNoinit);
          }
          objectBuilderNoinit.Save = true;
        }
        else if (!andRead3)
        {
          if (andRead4 is MyObjectBuilder_Planet)
          {
            if (MyEntities.TryGetEntityById<MyVoxelBase>(andRead4.EntityId, out objectBuilderNoinit))
              ;
          }
          else if (andRead1)
          {
            this.TryRemoveExistingEntity(andRead4.EntityId);
            IMyStorage asteroidStorage = (IMyStorage) MyStorageBase.CreateAsteroidStorage(asteroid);
            objectBuilderNoinit = (MyVoxelBase) MyEntities.CreateFromObjectBuilderNoinit(andRead4);
            if (objectBuilderNoinit is MyVoxelMap myVoxelMap)
              myVoxelMap.Init(andRead4, asteroidStorage);
            if (objectBuilderNoinit != null)
              MyEntities.Add((MyEntity) objectBuilderNoinit);
          }
          else
          {
            this.TryRemoveExistingEntity(andRead4.EntityId);
            this.GenerateFromObjectBuilder(andRead4, out objectBuilderNoinit);
          }
        }
        else
          this.GenerateFromObjectBuilder(andRead4, out objectBuilderNoinit);
      }
      else
        MyEntities.TryGetEntityById<MyVoxelBase>(MySerializer.CreateAndRead<long>(stream), out objectBuilderNoinit);
      loadingDoneHandler(objectBuilderNoinit);
    }

    private void GenerateFromObjectBuilder(
      MyObjectBuilder_EntityBase builder,
      out MyVoxelBase voxelMap)
    {
      voxelMap = (MyVoxelBase) null;
      try
      {
        if (!(builder is MyObjectBuilder_VoxelMap objectBuilderVoxelMap))
          return;
        if (MyEntities.TryGetEntityById<MyVoxelBase>(builder.EntityId, out voxelMap))
        {
          MyStorageBase myStorageBase = MyStorageBase.Load(objectBuilderVoxelMap.StorageName);
          if (voxelMap is MyVoxelMap)
          {
            voxelMap.Storage = (IMyStorage) myStorageBase;
          }
          else
          {
            if (!(voxelMap is MyPlanet))
              return;
            ((MyPlanet) voxelMap).Storage = (IMyStorage) myStorageBase;
          }
        }
        else
        {
          this.TryRemoveExistingEntity(builder.EntityId);
          voxelMap = (MyVoxelBase) MyEntities.CreateFromObjectBuilderNoinit(builder);
          if (voxelMap == null)
            return;
          voxelMap.Init(builder);
          MyEntities.Add((MyEntity) voxelMap);
        }
      }
      catch
      {
        voxelMap = (MyVoxelBase) null;
        MyObjectBuilder_VoxelMap objectBuilderVoxelMap = (MyObjectBuilder_VoxelMap) builder;
        if (objectBuilderVoxelMap != null)
          MyMultiplayer.RaiseStaticEvent<string>((Func<IMyEventOwner, Action<string>>) (s => new Action<string>(MyMultiplayerBase.InvalidateVoxelCache)), objectBuilderVoxelMap.StorageName);
        MyLog.Default.WriteLine("Failed to load voxel from cache.");
      }
    }

    protected override void OnDestroyClientInternal()
    {
      if (this.Voxel == null)
        return;
      if (this.Voxel.Storage != null)
      {
        byte[] outCompressedData;
        this.Voxel.Storage.Save(out outCompressedData);
        MyMultiplayer.Static.VoxelMapData.Write(this.Voxel.StorageName, outCompressedData);
      }
      MyPlanet voxel = this.Voxel as MyPlanet;
      if (!this.Voxel.Save || voxel != null)
        return;
      this.Voxel.Close();
    }

    public override void GetStateGroups(List<IMyStateGroup> resultList)
    {
      if (this.m_streamingGroup != null)
        resultList.Add((IMyStateGroup) this.m_streamingGroup);
      base.GetStateGroups(resultList);
    }

    public void OnLoadBegin(Action<bool> loadingDoneHandler) => this.m_loadingDoneHandler = (Action<MyVoxelBase>) (instance => this.OnLoadDone(instance, loadingDoneHandler));

    public void CreateStreamingStateGroup() => this.m_streamingGroup = new MyStreamingEntityStateGroup<MyVoxelReplicable>(this, (IMyReplicable) this);

    public IMyStreamingStateGroup GetStreamingStateGroup() => (IMyStreamingStateGroup) this.m_streamingGroup;

    public void Serialize(
      BitStream stream,
      HashSet<string> cachedData,
      Endpoint forClient,
      Action writeData)
    {
      if (this.Voxel.Closed)
        return;
      bool isUserCreated = this.Voxel.CreatedByUser && this.Voxel.AsteroidName != null;
      bool isFromPrefab = this.Voxel.Save;
      bool contentChanged = this.Voxel.ContentChanged || this.Voxel.BeforeContentChanged;
      bool sendContent = (cachedData == null || !cachedData.Contains(this.Voxel.StorageName)) && (contentChanged || isFromPrefab && !isUserCreated);
      sendContent |= this.Voxel.AsteroidName == null;
      string asteroidName = this.Voxel.AsteroidName;
      long entityId = this.Voxel.EntityId;
      byte[] data = (byte[]) null;
      MyObjectBuilder_EntityBase builder = (MyObjectBuilder_EntityBase) null;
      if (sendContent)
        this.Voxel.Storage.Save(out data);
      if (isFromPrefab)
      {
        using (MyReplicationLayer.StartSerializingReplicable((IMyReplicable) this, forClient))
          builder = this.Voxel.GetObjectBuilder(false);
      }
      Parallel.Start((Action) (() =>
      {
        MySerializer.Write<bool>(stream, ref isUserCreated);
        MySerializer.Write<bool>(stream, ref isFromPrefab);
        MySerializer.Write<bool>(stream, ref sendContent);
        MySerializer.Write<bool>(stream, ref contentChanged);
        if (sendContent)
          MySerializer.Write<byte[]>(stream, ref data);
        else if (isUserCreated)
          MySerializer.Write<string>(stream, ref asteroidName);
        if (isFromPrefab)
          MySerializer.Write<MyObjectBuilder_EntityBase>(stream, ref builder, MyObjectBuilderSerializer.Dynamic);
        else
          MySerializer.Write<long>(stream, ref entityId);
        writeData();
      }));
      cachedData?.Add(this.Voxel.StorageName);
    }

    public void LoadDone(BitStream stream) => this.OnLoad(stream, this.m_loadingDoneHandler);

    public bool NeedsToBeStreamed => true;

    public void LoadCancel() => this.m_loadingDoneHandler((MyVoxelBase) null);

    public override BoundingBoxD GetAABB()
    {
      BoundingBoxD worldAabb = this.Instance.PositionComp.WorldAABB;
      if (this.Voxel is MyPlanet)
        return worldAabb;
      worldAabb.Inflate((double) this.Voxel.SizeInMetres.Length() * 50.0);
      return worldAabb;
    }

    protected override void RaiseDestroyed()
    {
      MyPlanet instance = this.Instance as MyPlanet;
      base.RaiseDestroyed();
      if (!Sync.IsServer || instance == null)
        return;
      MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (s => new Action<long>(MyEntities.ForceCloseEntityOnClients)), instance.EntityId);
    }
  }
}
