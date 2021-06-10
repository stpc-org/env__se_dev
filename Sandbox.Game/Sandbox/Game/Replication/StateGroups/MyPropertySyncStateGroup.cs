// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.StateGroups.MyPropertySyncStateGroup
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Sync;

namespace Sandbox.Game.Replication.StateGroups
{
  public sealed class MyPropertySyncStateGroup : IMyStateGroup, IMyNetObject, IMyEventOwner
  {
    public Func<MyEventContext, ValidationResult> GlobalValidate = (Func<MyEventContext, ValidationResult>) (context => ValidationResult.Passed);
    public MyPropertySyncStateGroup.PriorityAdjustDelegate PriorityAdjust = (MyPropertySyncStateGroup.PriorityAdjustDelegate) ((frames, state, priority) => priority);
    private readonly MyPropertySyncStateGroup.ClientData m_clientData = Sandbox.Game.Multiplayer.Sync.IsServer ? (MyPropertySyncStateGroup.ClientData) null : new MyPropertySyncStateGroup.ClientData();
    private readonly MyPropertySyncStateGroup.ServerData m_serverData = Sandbox.Game.Multiplayer.Sync.IsServer ? new MyPropertySyncStateGroup.ServerData() : (MyPropertySyncStateGroup.ServerData) null;
    private ListReader<SyncBase> m_properties;
    private readonly List<MyTimeSpan> m_propertyTimestamps;
    private readonly MyTimeSpan m_invalidTimestamp = MyTimeSpan.FromTicks(long.MinValue);

    public bool IsHighPriority => false;

    public IMyReplicable Owner { get; private set; }

    public bool IsStreaming => false;

    public bool IsValid => this.Owner != null && this.Owner.IsValid;

    public int PropertyCount => this.m_properties.Count;

    public bool NeedsUpdate => this.m_clientData.DirtyProperties.Bits > 0UL;

    public MyPropertySyncStateGroup(IMyReplicable ownerReplicable, SyncType syncType)
    {
      this.Owner = ownerReplicable;
      syncType.PropertyChangedNotify += new Action<SyncBase>(this.Notify);
      syncType.PropertyCountChanged += new Action(this.OnPropertyCountChanged);
      this.m_properties = syncType.Properties;
      this.m_propertyTimestamps = new List<MyTimeSpan>(this.m_properties.Count);
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        for (int index = 0; index < this.m_properties.Count; ++index)
          this.m_propertyTimestamps.Add(MyMultiplayer.Static.ReplicationLayer.GetSimulationUpdateTime());
      }
      else
      {
        for (int index = 0; index < this.m_properties.Count; ++index)
          this.m_propertyTimestamps.Add(this.m_invalidTimestamp);
      }
    }

    private void Notify(SyncBase sync)
    {
      if (this.Owner == null || sync == null || !this.Owner.IsValid)
        return;
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        MyTimeSpan simulationUpdateTime = MyMultiplayer.Static.ReplicationLayer.GetSimulationUpdateTime();
        this.m_propertyTimestamps[sync.Id] = simulationUpdateTime >= this.m_propertyTimestamps[sync.Id] ? simulationUpdateTime : this.m_propertyTimestamps[sync.Id];
        foreach (KeyValuePair<Endpoint, MyPropertySyncStateGroup.ServerData.DataPerClient> keyValuePair in this.m_serverData.ServerClientData)
        {
          if (keyValuePair.Value != null)
            keyValuePair.Value.DirtyProperties[sync.Id] = true;
        }
        MyMultiplayer.GetReplicationServer()?.AddToDirtyGroups((IMyStateGroup) this);
      }
      else
      {
        if (!(this.m_propertyTimestamps[sync.Id] != this.m_invalidTimestamp))
          return;
        MyTimeSpan simulationUpdateTime = MyMultiplayer.Static.ReplicationLayer.GetSimulationUpdateTime();
        this.m_propertyTimestamps[sync.Id] = simulationUpdateTime;
        this.m_clientData.DirtyProperties[sync.Id] = true;
        if (!(MyMultiplayer.ReplicationLayer is MyReplicationClient replicationLayer))
          return;
        replicationLayer.AddToUpdates((IMyStateGroup) this);
      }
    }

    public void MarkDirty()
    {
      if (this.m_properties.Count == 0)
        return;
      foreach (KeyValuePair<Endpoint, MyPropertySyncStateGroup.ServerData.DataPerClient> keyValuePair in this.m_serverData.ServerClientData)
        keyValuePair.Value.DirtyProperties.Reset(true);
      MyMultiplayer.GetReplicationServer().AddToDirtyGroups((IMyStateGroup) this);
    }

    public void CreateClientData(MyClientStateBase forClient)
    {
      MyPropertySyncStateGroup.ServerData.DataPerClient dataPerClient = new MyPropertySyncStateGroup.ServerData.DataPerClient();
      this.m_serverData.ServerClientData.Add(forClient.EndpointId, dataPerClient);
      if (this.m_properties.Count <= 0)
        return;
      dataPerClient.DirtyProperties.Reset(true);
    }

    public void DestroyClientData(MyClientStateBase forClient) => this.m_serverData.ServerClientData.Remove(forClient.EndpointId);

    public void ClientUpdate(MyTimeSpan clientTimestamp)
    {
      if (this.m_clientData.DirtyProperties.Bits == 0UL || MyMultiplayer.Static.FrameCounter - this.m_clientData.LastUpdateFrame < 6U)
        return;
      foreach (SyncBase property in this.m_properties)
      {
        if (this.m_clientData.DirtyProperties[property.Id])
          MyMultiplayer.RaiseEvent<MyPropertySyncStateGroup, byte, double, BitReaderWriter>(this, (Func<MyPropertySyncStateGroup, Action<byte, double, BitReaderWriter>>) (x => new Action<byte, double, BitReaderWriter>(x.SyncPropertyChanged_Implementation)), (byte) property.Id, this.m_propertyTimestamps[property.Id].Milliseconds, (BitReaderWriter) property);
      }
      this.m_clientData.DirtyProperties.Reset(false);
      this.m_clientData.LastUpdateFrame = MyMultiplayer.Static.FrameCounter;
    }

    public void Destroy() => this.Owner = (IMyReplicable) null;

    [Event(null, 248)]
    [Reliable]
    [Server]
    private void SyncPropertyChanged_Implementation(
      byte propertyIndex,
      double propertyTimestampMs,
      BitReaderWriter reader)
    {
      ValidationResult validationResult = this.GlobalValidate(MyEventContext.Current);
      if (!MyEventContext.Current.IsLocallyInvoked && validationResult != ValidationResult.Passed)
      {
        SyncBase syncBase = (SyncBase) null;
        if ((int) propertyIndex < this.m_properties.Count)
          syncBase = this.m_properties[(int) propertyIndex];
        if (syncBase.ShouldValidate)
        {
          (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, validationResult.HasFlag((Enum) ValidationResult.Kick), validationResult.ToString() + " " + (syncBase != null ? this.m_properties[(int) propertyIndex].DebugName : "Incorrect property index"), false);
          return;
        }
      }
      if ((int) propertyIndex >= this.m_properties.Count)
        return;
      MyMultiplayer.GetReplicationServer();
      Endpoint key = new Endpoint(MyEventContext.Current.Sender, (byte) 0);
      MyTimeSpan myTimeSpan = MyTimeSpan.FromMilliseconds(propertyTimestampMs);
      bool acceptAndSetValue = myTimeSpan >= this.m_propertyTimestamps[(int) propertyIndex];
      if (reader.ReadData((IBitSerializable) this.m_properties[(int) propertyIndex], true, acceptAndSetValue))
      {
        this.m_propertyTimestamps[(int) propertyIndex] = myTimeSpan;
      }
      else
      {
        if (!acceptAndSetValue)
          return;
        MyPropertySyncStateGroup.ServerData.DataPerClient dataPerClient;
        this.m_serverData.ServerClientData.TryGetValue(key, out dataPerClient);
        if (dataPerClient == null)
          return;
        dataPerClient.DirtyProperties[(int) propertyIndex] = true;
      }
    }

    public void Serialize(
      BitStream stream,
      MyClientInfo forClient,
      MyTimeSpan serverTimestamp,
      MyTimeSpan lastClientTimestamp,
      byte packetId,
      int maxBitPosition,
      HashSet<string> cachedData)
    {
      int num1 = MyCompilationSymbols.EnableNetworkServerOutgoingPacketTracking ? 1 : 0;
      SmallBitField dirtyProperties;
      if (stream.Writing)
      {
        dirtyProperties = this.m_serverData.ServerClientData[forClient.EndpointId].DirtyProperties;
        stream.WriteUInt64(dirtyProperties.Bits, this.m_properties.Count);
      }
      else
        dirtyProperties.Bits = stream.ReadUInt64(this.m_properties.Count);
      int num2 = MyCompilationSymbols.EnableNetworkServerOutgoingPacketTracking ? 1 : 0;
      for (int index = 0; index < this.m_properties.Count; ++index)
      {
        if (dirtyProperties[index])
        {
          if (stream.Reading)
          {
            MyTimeSpan myTimeSpan = MyTimeSpan.FromMilliseconds(stream.ReadDouble());
            if (this.m_properties[index].Serialize(stream, false, myTimeSpan >= this.m_propertyTimestamps[index]))
            {
              this.m_propertyTimestamps[index] = myTimeSpan;
              this.m_clientData.DirtyProperties[index] = false;
            }
          }
          else
          {
            MyMultiplayer.GetReplicationServer();
            double milliseconds = this.m_propertyTimestamps[index].Milliseconds;
            int num3 = MyCompilationSymbols.EnableNetworkServerOutgoingPacketTracking ? 1 : 0;
            stream.WriteDouble(milliseconds);
            this.m_properties[index].Serialize(stream, false, true);
          }
        }
      }
      if (stream.Writing && stream.BitPosition <= (long) maxBitPosition)
      {
        MyPropertySyncStateGroup.ServerData.DataPerClient dataPerClient = this.m_serverData.ServerClientData[forClient.EndpointId];
        dataPerClient.SentProperties[(int) packetId].Bits = dataPerClient.DirtyProperties.Bits;
        dataPerClient.DirtyProperties.Bits = 0UL;
      }
      int num4 = MyCompilationSymbols.EnableNetworkServerOutgoingPacketTracking ? 1 : 0;
    }

    public void OnAck(MyClientStateBase forClient, byte packetId, bool delivered)
    {
      MyPropertySyncStateGroup.ServerData.DataPerClient dataPerClient = this.m_serverData.ServerClientData[forClient.EndpointId];
      if (delivered)
        return;
      dataPerClient.DirtyProperties.Bits |= dataPerClient.SentProperties[(int) packetId].Bits;
      MyMultiplayer.GetReplicationServer().AddToDirtyGroups((IMyStateGroup) this);
    }

    public void ForceSend(MyClientStateBase clientData)
    {
    }

    public void Reset(bool reinit, MyTimeSpan clientTimestamp)
    {
    }

    public bool IsStillDirty(Endpoint forClient) => this.m_serverData.ServerClientData[forClient].DirtyProperties.Bits > 0UL;

    public MyStreamProcessingState IsProcessingForClient(Endpoint forClient) => MyStreamProcessingState.None;

    private void OnPropertyCountChanged()
    {
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        for (int count = this.m_propertyTimestamps.Count; count < this.m_properties.Count; ++count)
          this.m_propertyTimestamps.Add(MyMultiplayer.Static.ReplicationLayer.GetSimulationUpdateTime());
      }
      else
      {
        for (int count = this.m_propertyTimestamps.Count; count < this.m_properties.Count; ++count)
          this.m_propertyTimestamps.Add(this.m_invalidTimestamp);
      }
    }

    private class ServerData
    {
      public readonly Dictionary<Endpoint, MyPropertySyncStateGroup.ServerData.DataPerClient> ServerClientData = new Dictionary<Endpoint, MyPropertySyncStateGroup.ServerData.DataPerClient>();

      public class DataPerClient
      {
        public SmallBitField DirtyProperties = new SmallBitField(false);
        public readonly SmallBitField[] SentProperties = new SmallBitField[256];
      }
    }

    private class ClientData
    {
      public SmallBitField DirtyProperties;
      public uint LastUpdateFrame;
    }

    public delegate float PriorityAdjustDelegate(
      int frameCountWithoutSync,
      MyClientStateBase clientState,
      float basePriority);

    protected sealed class SyncPropertyChanged_Implementation\u003C\u003ESystem_Byte\u0023System_Double\u0023VRage_Library_Collections_BitReaderWriter : ICallSite<MyPropertySyncStateGroup, byte, double, BitReaderWriter, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyPropertySyncStateGroup @this,
        in byte propertyIndex,
        in double propertyTimestampMs,
        in BitReaderWriter reader,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.SyncPropertyChanged_Implementation(propertyIndex, propertyTimestampMs, reader);
      }
    }
  }
}
