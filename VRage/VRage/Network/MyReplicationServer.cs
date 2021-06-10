// Decompiled with JetBrains decompiler
// Type: VRage.Network.MyReplicationServer
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Text;
using System.Threading;
using VRage.Collections;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Replication;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace VRage.Network
{
  public class MyReplicationServer : MyReplicationLayer
  {
    private const int MAX_NUM_STATE_SYNC_PACKETS_PER_CLIENT = 7;
    private readonly IReplicationServerCallback m_callback;
    private readonly CacheList<IMyStateGroup> m_tmpGroups = new CacheList<IMyStateGroup>(4);
    private HashSet<IMyReplicable> m_toRecalculateHash = new HashSet<IMyReplicable>();
    private readonly List<IMyReplicable> m_tmpReplicableList = new List<IMyReplicable>();
    private readonly HashSet<IMyReplicable> m_tmpReplicableDepsList = new HashSet<IMyReplicable>();
    private readonly CacheList<IMyReplicable> m_tmp = new CacheList<IMyReplicable>();
    private readonly CacheList<IMyReplicable> m_tmpAdd = new CacheList<IMyReplicable>();
    private readonly HashSet<IMyReplicable> m_lastLayerAdditions = new HashSet<IMyReplicable>();
    private readonly CachingHashSet<IMyReplicable> m_postponedDestructionReplicables = new CachingHashSet<IMyReplicable>();
    private readonly ConcurrentCachingHashSet<IMyReplicable> m_priorityUpdates = new ConcurrentCachingHashSet<IMyReplicable>();
    private MyTimeSpan m_serverTimeStamp = MyTimeSpan.Zero;
    private long m_serverFrame;
    private readonly ConcurrentQueue<IMyStateGroup> m_dirtyGroups = new ConcurrentQueue<IMyStateGroup>();
    public static SerializableVector3I StressSleep = new SerializableVector3I(0, 0, 0);
    private readonly Dictionary<IMyReplicable, List<IMyStateGroup>> m_replicableGroups = new Dictionary<IMyReplicable, List<IMyStateGroup>>();
    private readonly ConcurrentDictionary<Endpoint, MyClient> m_clientStates = new ConcurrentDictionary<Endpoint, MyClient>();
    private readonly ConcurrentDictionary<Endpoint, MyTimeSpan> m_recentClientsStates = new ConcurrentDictionary<Endpoint, MyTimeSpan>();
    private readonly HashSet<Endpoint> m_recentClientStatesToRemove = new HashSet<Endpoint>();
    private readonly MyTimeSpan SAVED_CLIENT_DURATION = MyTimeSpan.FromSeconds(60.0);
    [ThreadStatic]
    private static List<EndpointId> m_recipients;
    private readonly List<EndpointId> m_endpoints = new List<EndpointId>();

    public MyReplicationServer(
      IReplicationServerCallback callback,
      EndpointId localEndpoint,
      Thread mainThread)
      : base(true, localEndpoint, mainThread)
    {
      this.m_callback = callback;
      this.m_replicables = (MyReplicablesBase) new MyReplicablesAABB(mainThread);
      this.m_replicables.OnChildAdded += new Action<IMyReplicable>(this.OnReplicableChildAdded);
    }

    private void OnReplicableChildAdded(IMyReplicable child)
    {
      foreach (MyClient myClient in (IEnumerable<MyClient>) this.m_clientStates.Values)
      {
        MyClient.UpdateLayer updateLayer;
        if (myClient.ReplicableToLayer.TryGetValue(child, out updateLayer))
          updateLayer.UpdateTimer = 0;
      }
    }

    protected override MyPacketDataBitStreamBase GetBitStreamPacketData() => this.m_callback.GetBitStreamPacketData();

    public void Replicate(IMyReplicable obj)
    {
      if (!this.IsTypeReplicated(obj.GetType()))
        return;
      if (!obj.IsReadyForReplication)
      {
        obj.ReadyForReplicationAction.Add(obj, (Action) (() => this.Replicate(obj)));
      }
      else
      {
        this.AddNetworkObjectServer((IMyNetObject) obj);
        this.m_replicables.Add(obj, out IMyReplicable _);
        this.AddStateGroups(obj);
        if (!obj.PriorityUpdate)
          return;
        this.m_priorityUpdates.Add(obj);
      }
    }

    public void ForceReplicable(IMyReplicable obj, IMyReplicable parent = null)
    {
      if (obj == null)
        return;
      foreach (KeyValuePair<Endpoint, MyClient> clientState in this.m_clientStates)
      {
        if ((parent == null || clientState.Value.Replicables.ContainsKey(parent)) && !clientState.Value.Replicables.ContainsKey(obj))
          this.RefreshReplicable(obj, clientState.Key, clientState.Value, true);
      }
    }

    private void ForceReplicable(IMyReplicable obj, Endpoint clientEndpoint)
    {
      if (this.m_localEndpoint == clientEndpoint.Id || (clientEndpoint.Id.IsNull || obj == null || !this.m_clientStates.ContainsKey(clientEndpoint)))
        return;
      MyClient clientState = this.m_clientStates[clientEndpoint];
      if (clientState.Replicables.ContainsKey(obj))
        return;
      this.AddForClient(obj, clientEndpoint, clientState, true);
    }

    public void RemoveForClientIfIncomplete(IMyEventProxy objA)
    {
      if (objA == null)
        return;
      IMyReplicable replicableA = this.GetProxyTarget(objA) as IMyReplicable;
      this.RemoveForClients(replicableA, (Func<MyClient, bool>) (x => x.IsReplicablePending(replicableA)), true);
    }

    public void ForceEverything(Endpoint clientEndpoint) => this.m_replicables.IterateRoots((Action<IMyReplicable>) (replicable => this.ForceReplicable(replicable, clientEndpoint)));

    public void Destroy(IMyReplicable obj)
    {
      if (!this.IsTypeReplicated(obj.GetType()) || !obj.IsReadyForReplication || obj.ReadyForReplicationAction != null && obj.ReadyForReplicationAction.Count > 0 || this.GetNetworkIdByObject((IMyNetObject) obj).IsInvalid)
        return;
      this.m_priorityUpdates.Remove(obj);
      this.m_priorityUpdates.ApplyChanges();
      bool isAnyClientPending = false;
      bool flag = obj.GetParent() != null && !obj.GetParent().IsValid;
      this.RemoveForClients(obj, (Func<MyClient, bool>) (client =>
      {
        if (client.BlockedReplicables.ContainsKey(obj))
        {
          client.BlockedReplicables[obj].Remove = true;
          if (!obj.HasToBeChild && !this.m_priorityUpdates.Contains(obj))
            this.m_priorityUpdates.Add(obj);
          isAnyClientPending = true;
          return false;
        }
        client.PermanentReplicables.Remove(obj);
        client.CrucialReplicables.Remove(obj);
        return true;
      }), !flag);
      this.m_replicables.RemoveHierarchy(obj);
      if (!isAnyClientPending)
      {
        this.RemoveStateGroups(obj);
        this.RemoveNetworkedObject((IMyNetObject) obj);
        this.m_postponedDestructionReplicables.Remove(obj);
        obj.OnRemovedFromReplication();
      }
      else
        this.m_postponedDestructionReplicables.Add(obj);
    }

    public void ResetForClients(IMyReplicable obj) => this.RemoveForClients(obj, (Func<MyClient, bool>) (client => client.Replicables.ContainsKey(obj)), true);

    public void AddClient(Endpoint endpoint, MyClientStateBase clientState)
    {
      if (this.m_clientStates.ContainsKey(endpoint))
        return;
      clientState.EndpointId = endpoint;
      this.m_clientStates.TryAdd(endpoint, new MyClient(clientState, this.m_callback));
    }

    private void OnClientConnected(EndpointId endpointId, MyClientStateBase clientState) => this.AddClient(new Endpoint(endpointId, (byte) 0), clientState);

    public void OnClientReady(Endpoint endpointId, ref ClientReadyDataMsg msg)
    {
      MyClient myClient;
      if (!this.m_clientStates.TryGetValue(endpointId, out myClient))
        return;
      myClient.IsReady = true;
      myClient.ForcePlayoutDelayBuffer = msg.ForcePlayoutDelayBuffer;
      myClient.UsePlayoutDelayBufferForCharacter = msg.UsePlayoutDelayBufferForCharacter;
      myClient.UsePlayoutDelayBufferForJetpack = msg.UsePlayoutDelayBufferForJetpack;
      myClient.UsePlayoutDelayBufferForGrids = msg.UsePlayoutDelayBufferForGrids;
    }

    public void OnClientReady(Endpoint endpointId, MyPacket packet)
    {
      ClientReadyDataMsg andRead = MySerializer.CreateAndRead<ClientReadyDataMsg>(packet.BitStream);
      this.OnClientReady(endpointId, ref andRead);
      this.SendServerData(endpointId);
    }

    private void SendServerData(Endpoint endpointId)
    {
      MyPacketDataBitStreamBase streamPacketData = this.m_callback.GetBitStreamPacketData();
      this.SerializeTypeTable(streamPacketData.Stream);
      this.m_callback.SendServerData((IPacketData) streamPacketData, endpointId);
    }

    public void OnClientLeft(EndpointId endpointId)
    {
      bool flag;
      do
      {
        flag = false;
        foreach (KeyValuePair<Endpoint, MyClient> clientState in this.m_clientStates)
        {
          if (clientState.Key.Id == endpointId)
          {
            flag = true;
            this.RemoveClient(clientState.Key);
            break;
          }
        }
      }
      while (flag);
    }

    private void RemoveClient(Endpoint endpoint)
    {
      MyClient client;
      if (!this.m_clientStates.TryGetValue(endpoint, out client))
        return;
      while (client.Replicables.Count > 0)
        this.RemoveForClient(client.Replicables.FirstPair().Key, client, false);
      this.m_clientStates.Remove<Endpoint, MyClient>(endpoint);
      this.m_recentClientsStates[endpoint] = this.m_callback.GetUpdateTime() + this.SAVED_CLIENT_DURATION;
    }

    public override void Disconnect()
    {
      foreach (KeyValuePair<Endpoint, MyClient> clientState in this.m_clientStates)
        this.RemoveClient(clientState.Key);
    }

    public override void SetPriorityMultiplier(EndpointId id, float priority)
    {
      MyClient myClient;
      if (!this.m_clientStates.TryGetValue(new Endpoint(id, (byte) 0), out myClient))
        return;
      myClient.PriorityMultiplier = priority;
    }

    public void OnClientJoined(EndpointId endpointId, MyClientStateBase clientState) => this.OnClientConnected(endpointId, clientState);

    public void OnClientAcks(MyPacket packet)
    {
      MyClient myClient;
      if (!this.m_clientStates.TryGetValue(packet.Sender, out myClient))
      {
        packet.Return();
      }
      else
      {
        myClient.OnClientAcks(packet);
        packet.Return();
      }
    }

    public void OnClientUpdate(MyPacket packet)
    {
      MyClient myClient;
      if (!this.m_clientStates.TryGetValue(packet.Sender, out myClient))
        packet.Return();
      else
        myClient.OnClientUpdate(packet, this.m_serverTimeStamp);
    }

    public override MyTimeSpan GetSimulationUpdateTime() => this.m_serverTimeStamp;

    [HandleProcessCorruptedStateExceptions]
    [SecurityCritical]
    public override void UpdateBefore()
    {
      Endpoint endpoint = new Endpoint();
      foreach (KeyValuePair<Endpoint, MyClient> clientState in this.m_clientStates)
      {
        try
        {
          clientState.Value.Update(this.m_serverTimeStamp);
        }
        catch (Exception ex)
        {
          MyLog.Default.WriteLine(ex);
          endpoint = clientState.Key;
        }
      }
      if (endpoint.Id.IsValid)
        this.m_callback.DisconnectClient(endpoint.Id.Value);
      MyTimeSpan updateTime = this.m_callback.GetUpdateTime();
      foreach (KeyValuePair<Endpoint, MyTimeSpan> recentClientsState in this.m_recentClientsStates)
      {
        if (recentClientsState.Value < updateTime)
          this.m_recentClientStatesToRemove.Add(recentClientsState.Key);
      }
      foreach (Endpoint key in this.m_recentClientStatesToRemove)
        this.m_recentClientsStates.Remove<Endpoint, MyTimeSpan>(key);
      this.m_recentClientStatesToRemove.Clear();
      this.m_postponedDestructionReplicables.ApplyAdditions();
      foreach (IMyReplicable destructionReplicable in this.m_postponedDestructionReplicables)
        this.Destroy(destructionReplicable);
      this.m_postponedDestructionReplicables.ApplyRemovals();
    }

    public override void UpdateAfter()
    {
    }

    public override void UpdateClientStateGroups()
    {
    }

    public override void Simulate()
    {
    }

    public override void SendUpdate()
    {
      this.m_serverTimeStamp = this.m_callback.GetUpdateTime();
      ++this.m_serverFrame;
      this.ApplyDirtyGroups();
      if (this.m_clientStates.Count == 0)
        return;
      this.m_priorityUpdates.ApplyChanges();
      foreach (KeyValuePair<Endpoint, MyClient> clientState in this.m_clientStates)
      {
        MyClient client = clientState.Value;
        if (client.IsReady)
        {
          IMyReplicable controlledReplicable = client.State.ControlledReplicable;
          IMyReplicable characterReplicable = client.State.CharacterReplicable;
          MyClient.UpdateLayer updateLayer1 = client.UpdateLayers[client.UpdateLayers.Length - 1];
          for (int index1 = 0; index1 < client.UpdateLayers.Length; ++index1)
          {
            MyClient.UpdateLayer updateLayer2 = client.UpdateLayers[index1];
            --updateLayer2.UpdateTimer;
            updateLayer2.PreviousLayersPCU = index1 > 0 ? client.UpdateLayers[index1 - 1].TotalCumulativePCU : 0;
            if (updateLayer2.UpdateTimer <= 0)
            {
              updateLayer2.UpdateTimer = updateLayer2.Descriptor.UpdateInterval;
              if (client.State.Position.HasValue)
                this.m_replicables.GetReplicablesInBox(new BoundingBoxD(client.State.Position.Value - new Vector3D((double) updateLayer2.Descriptor.Radius), client.State.Position.Value + new Vector3D((double) updateLayer2.Descriptor.Radius)), this.m_tmpReplicableList);
              HashSet<IMyReplicable> replicables = updateLayer2.Replicables;
              updateLayer2.Replicables = this.m_toRecalculateHash;
              updateLayer2.LayerPCU = 0;
              this.m_toRecalculateHash = replicables;
              updateLayer2.Sender.List.Clear();
              foreach (IMyReplicable key in this.m_toRecalculateHash)
              {
                MyClient.UpdateLayer updateLayer3;
                if (client.ReplicableToLayer.TryGetValue(key, out updateLayer3) && updateLayer3 == updateLayer2)
                  client.ReplicableToLayer.Remove(key);
              }
              bool flag = updateLayer2 == updateLayer1;
              foreach (IMyReplicable tmpReplicable in this.m_tmpReplicableList)
              {
                if (this.AddReplicableToLayer(tmpReplicable, updateLayer2, client) & flag)
                  this.m_tmpReplicableDepsList.Add(tmpReplicable);
              }
              this.m_tmpReplicableList.Clear();
              foreach (KeyValuePair<IMyReplicable, byte> permanentReplicable in client.PermanentReplicables)
              {
                if ((int) permanentReplicable.Value == index1 && this.AddReplicableToLayer(permanentReplicable.Key, updateLayer2, client))
                  this.m_tmpReplicableDepsList.Add(permanentReplicable.Key);
              }
              if (updateLayer2 == updateLayer1)
              {
                client.CrucialReplicables.Clear();
                if (controlledReplicable != null)
                {
                  if (this.AddReplicableToLayer(controlledReplicable, updateLayer2, client))
                    client.PlayerControllableUsesPredictedPhysics = this.AddReplicableDependenciesToLayer(controlledReplicable, updateLayer2, client);
                  if (this.AddReplicableToLayer(characterReplicable, updateLayer2, client))
                    this.AddReplicableDependenciesToLayer(characterReplicable, updateLayer2, client);
                  this.AddCrucialReplicable(client, controlledReplicable);
                  this.AddReplicableDependenciesToLayer(controlledReplicable, updateLayer2, client);
                  this.AddCrucialReplicable(client, characterReplicable);
                  this.AddReplicableDependenciesToLayer(characterReplicable, updateLayer2, client);
                  HashSet<IMyReplicable> dependencies = characterReplicable.GetDependencies(true);
                  if (dependencies != null)
                  {
                    foreach (IMyReplicable myReplicable in dependencies)
                    {
                      this.AddCrucialReplicable(client, myReplicable);
                      if (this.AddReplicableToLayer(myReplicable, updateLayer2, client))
                        this.m_tmpReplicableDepsList.Add(myReplicable);
                    }
                  }
                }
                foreach (IMyReplicable lastLayerAddition in this.m_lastLayerAdditions)
                {
                  this.AddCrucialReplicable(client, lastLayerAddition);
                  this.m_tmpReplicableDepsList.Add(lastLayerAddition);
                }
                this.m_lastLayerAdditions.Clear();
              }
              foreach (IMyReplicable tmpReplicableDeps in this.m_tmpReplicableDepsList)
                this.AddReplicableDependenciesToLayer(tmpReplicableDeps, updateLayer2, client);
              this.m_tmpReplicableDepsList.Clear();
              MyClient.UpdateLayer updateLayer4 = updateLayer2;
              int num;
              if (index1 != 0 && client.PCULimit.HasValue)
              {
                int totalCumulativePcu = updateLayer2.TotalCumulativePCU;
                int? pcuLimit = client.PCULimit;
                int valueOrDefault = pcuLimit.GetValueOrDefault();
                num = totalCumulativePcu <= valueOrDefault & pcuLimit.HasValue ? 1 : 0;
              }
              else
                num = 1;
              updateLayer4.Enabled = num != 0;
              int index2 = -1;
              if (!updateLayer2.Enabled && index1 > 0 && client.UpdateLayers[index1 - 1].Enabled)
                index2 = index1 - 1;
              else if (updateLayer2.Enabled && updateLayer2 == updateLayer1)
                index2 = index1;
              int? nullable1;
              if (index2 >= 0 && client.LastEnabledLayer != index2)
              {
                client.LastEnabledLayer = index2;
                MyClientStateBase state = client.State;
                nullable1 = index2 == updateLayer1.Index ? new int?() : new int?(client.UpdateLayers[index2].Descriptor.Radius);
                float? nullable2 = nullable1.HasValue ? new float?((float) nullable1.GetValueOrDefault()) : new float?();
                state.ReplicationRange = nullable2;
              }
              Endpoint key1 = clientState.Key;
              foreach (IMyReplicable replicable in updateLayer2.Replicables)
              {
                if (!updateLayer2.Enabled)
                {
                  nullable1 = replicable.PCU;
                  if (nullable1.HasValue)
                  {
                    this.m_toRecalculateHash.Add(replicable);
                    continue;
                  }
                }
                IMyReplicable parent = replicable.GetParent();
                if (!client.HasReplicable(replicable) && (parent == null || client.HasReplicable(parent)))
                  this.AddForClient(replicable, key1, client, false);
              }
              foreach (IMyReplicable replicable in this.m_toRecalculateHash)
                this.RefreshReplicable(replicable, key1, client);
              this.m_toRecalculateHash.Clear();
              if (client.WantsBatchCompleteConfirmation && updateLayer2 == updateLayer1 && client.PendingReplicables == 0)
              {
                this.m_callback.SendPendingReplicablesDone(key1);
                client.WantsBatchCompleteConfirmation = false;
              }
            }
          }
          foreach (IMyReplicable priorityUpdate in this.m_priorityUpdates)
            this.RefreshReplicable(priorityUpdate, clientState.Key, client);
        }
      }
      foreach (IMyReplicable priorityUpdate in this.m_priorityUpdates)
        this.m_priorityUpdates.Remove(priorityUpdate);
      this.m_priorityUpdates.ApplyRemovals();
      foreach (KeyValuePair<Endpoint, MyClient> clientState in this.m_clientStates)
        this.FilterStateSync(clientState.Value);
      foreach (KeyValuePair<Endpoint, MyClient> clientState in this.m_clientStates)
        clientState.Value.SendUpdate(this.m_serverTimeStamp);
      if (MyReplicationServer.StressSleep.X <= 0)
        return;
      Thread.Sleep(MyReplicationServer.StressSleep.Z != 0 ? (int) (Math.Sin(this.m_serverTimeStamp.Milliseconds * Math.PI / (double) MyReplicationServer.StressSleep.Z) * (double) MyReplicationServer.StressSleep.Y + (double) MyReplicationServer.StressSleep.X) : MyRandom.Instance.Next(MyReplicationServer.StressSleep.X, MyReplicationServer.StressSleep.Y));
    }

    private bool AddReplicableToLayer(
      IMyReplicable rep,
      MyClient.UpdateLayer layer,
      MyClient client)
    {
      if (this.IsReplicableInPreviousLayer(rep, layer, client))
        return false;
      this.AddReplicableToLayerSingle(rep, layer, client);
      HashSet<IMyReplicable> criticalDependencies = rep.GetCriticalDependencies();
      if (criticalDependencies != null)
      {
        foreach (IMyReplicable rep1 in criticalDependencies)
        {
          if (!this.IsReplicableInPreviousLayer(rep1, layer, client))
            this.AddReplicableToLayerSingle(rep1, layer, client);
        }
      }
      return true;
    }

    private bool AddReplicableDependenciesToLayer(
      IMyReplicable rep,
      MyClient.UpdateLayer layer,
      MyClient client)
    {
      bool flag = true;
      int? nullable;
      if (client.PCULimit.HasValue)
      {
        int totalCumulativePcu = layer.TotalCumulativePCU;
        nullable = client.PCULimit;
        int valueOrDefault = nullable.GetValueOrDefault();
        if (!(totalCumulativePcu < valueOrDefault & nullable.HasValue))
          goto label_19;
      }
      HashSet<IMyReplicable> physicalDependencies = rep.GetPhysicalDependencies(this.m_serverTimeStamp, this.m_replicables);
      if (physicalDependencies != null)
      {
        if (client.PCULimit.HasValue)
        {
          int num1 = 0;
          foreach (IMyReplicable rep1 in physicalDependencies)
          {
            if (!this.IsReplicableInPreviousLayer(rep1, layer, client) && !layer.Replicables.Contains(rep))
            {
              int num2 = num1;
              nullable = rep.PCU;
              int num3 = nullable ?? 0;
              num1 = num2 + num3;
            }
          }
          int num4 = num1 + layer.TotalCumulativePCU;
          int? pcuLimit = client.PCULimit;
          int valueOrDefault = pcuLimit.GetValueOrDefault();
          if (num4 > valueOrDefault & pcuLimit.HasValue)
            flag = false;
        }
        if (flag)
        {
          foreach (IMyReplicable rep1 in physicalDependencies)
          {
            if (!this.IsReplicableInPreviousLayer(rep1, layer, client))
              this.AddReplicableToLayerSingle(rep1, layer, client);
          }
        }
      }
label_19:
      return flag;
    }

    private void AddReplicableToLayerSingle(
      IMyReplicable rep,
      MyClient.UpdateLayer layer,
      MyClient client,
      bool removeFromDelete = true)
    {
      if (layer.Replicables.Add(rep))
      {
        layer.Sender.List.Add(rep);
        layer.LayerPCU += rep.PCU ?? 0;
      }
      client.ReplicableToLayer[rep] = layer;
      if (removeFromDelete)
        this.m_toRecalculateHash.Remove(rep);
      HashSet<IMyReplicable> dependencies = rep.GetDependencies(false);
      if (dependencies == null)
        return;
      foreach (IMyReplicable myReplicable in dependencies)
        this.m_lastLayerAdditions.Add(myReplicable);
    }

    private bool IsReplicableInPreviousLayer(
      IMyReplicable rep,
      MyClient.UpdateLayer layer,
      MyClient client)
    {
      MyClient.UpdateLayer updateLayer;
      return client.ReplicableToLayer.TryGetValue(rep, out updateLayer) && updateLayer != layer && updateLayer.Index < layer.Index;
    }

    public void SetClientPCULimit(EndpointId clientEndpoint, int pcuLimit)
    {
      MyClient myClient;
      if (!this.m_clientStates.TryGetValue(new Endpoint(clientEndpoint, (byte) 0), out myClient))
        return;
      myClient.PCULimit = new int?(pcuLimit);
    }

    public IEnumerable<(BoundingBoxD Bounds, IEnumerable<IMyReplicable> Replicables, int PCU, bool Enabled)> GetLayerData(
      EndpointId clientEndpoint)
    {
      MyClient client;
      if (this.m_clientStates.TryGetValue(new Endpoint(clientEndpoint, (byte) 0), out client))
      {
        Vector3D? position = client.State.Position;
        if (position.HasValue)
        {
          MyClient.UpdateLayer[] updateLayerArray = client.UpdateLayers;
          for (int index = 0; index < updateLayerArray.Length; ++index)
          {
            MyClient.UpdateLayer updateLayer = updateLayerArray[index];
            position = client.State.Position;
            Vector3D vector3D = position.Value;
            yield return (new BoundingBoxD(vector3D - new Vector3D((double) updateLayer.Descriptor.Radius), vector3D + new Vector3D((double) updateLayer.Descriptor.Radius)), (IEnumerable<IMyReplicable>) updateLayer.Replicables, updateLayer.LayerPCU, updateLayer.Enabled);
          }
          updateLayerArray = (MyClient.UpdateLayer[]) null;
        }
      }
    }

    private void RefreshReplicable(
      IMyReplicable replicable,
      Endpoint endPoint,
      MyClient client,
      bool force = false)
    {
      if (!replicable.IsSpatial)
      {
        IMyReplicable parent = replicable.GetParent();
        if (parent == null && !client.CrucialReplicables.Contains(replicable))
          RemoveReplicable();
        else if (parent == null || client.HasReplicable(parent))
        {
          this.AddForClient(replicable, endPoint, client, force);
        }
        else
        {
          if (!replicable.HasToBeChild || client.HasReplicable(parent))
            return;
          RemoveReplicable();
        }
      }
      else
      {
        bool flag = true;
        if (replicable.HasToBeChild)
        {
          IMyReplicable parent = replicable.GetParent();
          if (parent != null)
            flag = client.HasReplicable(parent);
        }
        MyClient.UpdateLayer layerOfReplicable = client.CalculateLayerOfReplicable(replicable);
        if (layerOfReplicable != null & flag && (layerOfReplicable.Enabled || !replicable.PCU.HasValue))
        {
          this.AddForClient(replicable, endPoint, client, force);
          this.AddReplicableToLayerSingle(replicable, layerOfReplicable, client, false);
        }
        else if (((replicable == client.State.ControlledReplicable || replicable == client.State.CharacterReplicable || client.CrucialReplicables.Contains(replicable) ? 1 : (client.PermanentReplicables.ContainsKey(replicable) ? 1 : 0)) & (flag ? 1 : 0)) != 0)
        {
          this.AddReplicableToLayerSingle(replicable, client.UpdateLayers[0], client, false);
          this.AddForClient(replicable, endPoint, client, force);
        }
        else
          RemoveReplicable();
      }

      void RemoveReplicable()
      {
        if (!client.Replicables.ContainsKey(replicable) || client.BlockedReplicables.ContainsKey(replicable))
          return;
        this.RemoveForClient(replicable, client, true);
      }
    }

    private void AddCrucialReplicable(MyClient client, IMyReplicable replicable)
    {
      client.CrucialReplicables.Add(replicable);
      HashSet<IMyReplicable> criticalDependencies = replicable.GetCriticalDependencies();
      if (criticalDependencies == null)
        return;
      foreach (IMyReplicable myReplicable in criticalDependencies)
        client.CrucialReplicables.Add(myReplicable);
    }

    public void AddToDirtyGroups(IMyStateGroup group)
    {
      if (!group.Owner.IsReadyForReplication)
        return;
      this.m_dirtyGroups.Enqueue(group);
    }

    private void ApplyDirtyGroups()
    {
      IMyStateGroup result;
      while (this.m_dirtyGroups.TryDequeue(out result))
      {
        foreach (KeyValuePair<Endpoint, MyClient> clientState in this.m_clientStates)
        {
          MyStateDataEntry groupEntry;
          if (clientState.Value.StateGroups.TryGetValue(result, out groupEntry))
            this.ScheduleStateGroupSync(clientState.Value, groupEntry, this.SyncFrameCounter);
        }
      }
    }

    private void SendStreamingEntry(MyClient client, MyStateDataEntry entry)
    {
      Endpoint endpointId = client.State.EndpointId;
      if (entry.Group.IsProcessingForClient(endpointId) == MyStreamProcessingState.Finished)
      {
        MyPacketDataBitStreamBase streamPacketData = this.m_callback.GetBitStreamPacketData();
        BitStream stream = streamPacketData.Stream;
        MyTimeSpan clientTimestamp;
        if (!client.WritePacketHeader(stream, true, this.m_serverTimeStamp, out clientTimestamp))
        {
          streamPacketData.Return();
          return;
        }
        stream.Terminate();
        stream.WriteNetworkId(entry.GroupId);
        long bitPosition1 = stream.BitPosition;
        stream.WriteInt32(0);
        long bitPosition2 = stream.BitPosition;
        client.Serialize(entry.Group, stream, clientTimestamp, streaming: true);
        client.AddPendingAck(entry.Group, true);
        long bitPosition3 = stream.BitPosition;
        stream.SetBitPositionWrite(bitPosition1);
        stream.WriteInt32((int) (bitPosition3 - bitPosition2));
        stream.SetBitPositionWrite(bitPosition3);
        stream.Terminate();
        this.m_callback.SendStateSync((IPacketData) streamPacketData, endpointId, true);
      }
      else
      {
        client.Serialize(entry.Group, (BitStream) null, MyTimeSpan.Zero);
        this.ScheduleStateGroupSync(client, entry, this.SyncFrameCounter);
      }
      IMyReplicable owner = entry.Group.Owner;
      if (owner == null)
        return;
      using (this.m_tmpAdd)
      {
        this.m_replicables.GetAllChildren(owner, (List<IMyReplicable>) this.m_tmpAdd);
        foreach (IMyReplicable replicable in (List<IMyReplicable>) this.m_tmpAdd)
        {
          if (!client.HasReplicable(replicable))
            this.AddForClient(replicable, endpointId, client, false);
        }
      }
    }

    private void FilterStateSync(MyClient client)
    {
      if (!client.IsAckAvailable())
        return;
      this.ApplyDirtyGroups();
      int num1 = 0;
      MyPacketDataBitStreamBase data = (MyPacketDataBitStreamBase) null;
      List<MyStateDataEntry> myStateDataEntryList = PoolManager.Get<List<MyStateDataEntry>>();
      int mtuSize = this.m_callback.GetMTUSize();
      int count = client.DirtyQueue.Count;
      int num2 = 7;
      MyStateDataEntry entry = (MyStateDataEntry) null;
      while (count-- > 0 && num2 > 0 && client.DirtyQueue.First.Priority < this.SyncFrameCounter)
      {
        MyStateDataEntry stateGroupEntry = client.DirtyQueue.Dequeue();
        myStateDataEntryList.Add(stateGroupEntry);
        MyReplicableClientData replicableClientData;
        if (stateGroupEntry.Owner == null || stateGroupEntry.Group.IsStreaming || client.Replicables.TryGetValue(stateGroupEntry.Owner, out replicableClientData) && replicableClientData.HasActiveStateSync)
        {
          if (stateGroupEntry.Group.IsStreaming)
          {
            if (entry == null && stateGroupEntry.Group.IsProcessingForClient(client.State.EndpointId) != MyStreamProcessingState.Processing)
              entry = stateGroupEntry;
          }
          else if (client.SendStateSync(stateGroupEntry, mtuSize, ref data, this.m_serverTimeStamp))
          {
            ++num1;
            if (data == null)
              --num2;
          }
          else
            break;
        }
      }
      if (data != null)
      {
        data.Stream.Terminate();
        this.m_callback.SendStateSync((IPacketData) data, client.State.EndpointId, false);
      }
      if (entry != null)
        this.SendStreamingEntry(client, entry);
      long syncFrameCounter = this.SyncFrameCounter;
      foreach (MyStateDataEntry groupEntry in myStateDataEntryList)
      {
        if (client.StateGroups.ContainsKey(groupEntry.Group) && groupEntry.Group.IsStillDirty(client.State.EndpointId))
          this.ScheduleStateGroupSync(client, groupEntry, syncFrameCounter);
      }
    }

    private void ScheduleStateGroupSync(
      MyClient client,
      MyStateDataEntry groupEntry,
      long currentTime,
      bool allowReplicableRemoval = true)
    {
      IMyReplicable myReplicable = groupEntry.Owner.GetParent() ?? groupEntry.Owner;
      MyClient.UpdateLayer updateLayer = (MyClient.UpdateLayer) null;
      if (!client.ReplicableToLayer.TryGetValue(myReplicable, out updateLayer))
      {
        updateLayer = client.CalculateLayerOfReplicable(myReplicable);
        if (updateLayer == null)
        {
          if (client.HasReplicable(myReplicable) & allowReplicableRemoval && client.State.Position.HasValue)
          {
            if (client.IsReplicableReady(myReplicable))
            {
              this.RemoveForClient(myReplicable, client, true);
              return;
            }
            MyLog.Default.Warning("Trying to remove entity with name " + myReplicable.InstanceName + " that is not yet replicated on client");
            return;
          }
          updateLayer = client.UpdateLayers[client.UpdateLayers.Length - 1];
        }
      }
      long num = (long) MyRandom.Instance.Next(1, (myReplicable == client.State.ControlledReplicable ? 1 : (groupEntry.Group.IsHighPriority ? Math.Max(updateLayer.Descriptor.SendInterval >> 4, 1) : updateLayer.Descriptor.SendInterval)) * 2) + currentTime;
      if (!client.DirtyQueue.Contains(groupEntry))
      {
        if (!groupEntry.Owner.IsValid && !this.m_postponedDestructionReplicables.Contains(groupEntry.Owner))
          return;
        client.DirtyQueue.Enqueue(groupEntry, num);
      }
      else if (groupEntry.Owner.IsValid || this.m_postponedDestructionReplicables.Contains(groupEntry.Owner))
      {
        long priority1 = groupEntry.Priority;
        long priority2 = Math.Min(priority1, num);
        if (priority2 == priority1)
          return;
        client.DirtyQueue.UpdatePriority(groupEntry, priority2);
      }
      else
        client.DirtyQueue.Remove(groupEntry);
    }

    public override void UpdateStatisticsData(
      int outgoing,
      int incoming,
      int tamperred,
      float gcMemory,
      float processMemory)
    {
      foreach (KeyValuePair<Endpoint, MyClient> clientState in this.m_clientStates)
        clientState.Value.Statistics.UpdateData(outgoing, incoming, tamperred, gcMemory, processMemory);
    }

    public override MyPacketStatistics ClearServerStatistics() => new MyPacketStatistics();

    public override MyPacketStatistics ClearClientStatistics()
    {
      MyPacketStatistics packetStatistics = new MyPacketStatistics();
      foreach (KeyValuePair<Endpoint, MyClient> clientState in this.m_clientStates)
        packetStatistics.Add(clientState.Value.Statistics);
      return packetStatistics;
    }

    private void AddForClient(
      IMyReplicable replicable,
      Endpoint clientEndpoint,
      MyClient client,
      bool force,
      bool addDependencies = false)
    {
      if (!replicable.IsReadyForReplication || client.HasReplicable(replicable) || (!replicable.ShouldReplicate(new MyClientInfo(client)) || !replicable.IsValid))
        return;
      this.AddClientReplicable(replicable, client, force);
      replicable.OnReplication();
      this.SendReplicationCreate(replicable, client, clientEndpoint);
      if (replicable is IMyStreamableReplicable)
        return;
      foreach (IMyReplicable child in this.m_replicables.GetChildren(replicable))
        this.AddForClient(child, clientEndpoint, client, force);
    }

    private void RemoveForClients(
      IMyReplicable replicable,
      Func<MyClient, bool> validate,
      bool sendDestroyToClient)
    {
      using (this.m_tmp)
      {
        if (MyReplicationServer.m_recipients == null)
          MyReplicationServer.m_recipients = new List<EndpointId>();
        bool flag = true;
        foreach (MyClient client in (IEnumerable<MyClient>) this.m_clientStates.Values)
        {
          if (validate(client))
          {
            if (flag)
            {
              this.m_replicables.GetAllChildren(replicable, (List<IMyReplicable>) this.m_tmp);
              this.m_tmp.Add(replicable);
              flag = false;
            }
            this.RemoveForClientInternal(replicable, client);
            if (sendDestroyToClient)
              MyReplicationServer.m_recipients.Add(client.State.EndpointId.Id);
          }
        }
        if (MyReplicationServer.m_recipients.Count <= 0)
          return;
        this.SendReplicationDestroy(replicable, MyReplicationServer.m_recipients);
        MyReplicationServer.m_recipients.Clear();
      }
    }

    private void RemoveForClient(
      IMyReplicable replicable,
      MyClient client,
      bool sendDestroyToClient)
    {
      if (MyReplicationServer.m_recipients == null)
        MyReplicationServer.m_recipients = new List<EndpointId>();
      using (this.m_tmp)
      {
        this.m_replicables.GetAllChildren(replicable, (List<IMyReplicable>) this.m_tmp);
        this.m_tmp.Add(replicable);
        this.RemoveForClientInternal(replicable, client);
        if (!sendDestroyToClient)
          return;
        MyReplicationServer.m_recipients.Add(client.State.EndpointId.Id);
        this.SendReplicationDestroy(replicable, MyReplicationServer.m_recipients);
        MyReplicationServer.m_recipients.Clear();
      }
    }

    private void RemoveForClientInternal(IMyReplicable replicable, MyClient client)
    {
      foreach (IMyReplicable myReplicable in (List<IMyReplicable>) this.m_tmp)
      {
        client.BlockedReplicables.Remove(myReplicable);
        this.RemoveClientReplicable(myReplicable, client);
      }
      foreach (MyClient.UpdateLayer updateLayer in client.UpdateLayers)
      {
        if (updateLayer.Replicables.Remove(replicable))
          updateLayer.LayerPCU -= replicable.PCU ?? 0;
      }
    }

    private void SendReplicationCreate(IMyReplicable obj, MyClient client, Endpoint clientEndpoint)
    {
      TypeId typeIdByType = this.GetTypeIdByType(obj.GetType());
      NetworkId networkIdByObject = this.GetNetworkIdByObject((IMyNetObject) obj);
      NetworkId networkId = NetworkId.Invalid;
      IMyReplicable parent = obj.GetParent();
      if (parent != null)
        networkId = this.GetNetworkIdByObject((IMyNetObject) parent);
      List<IMyStateGroup> replicableGroup = this.m_replicableGroups[obj];
      MyPacketDataBitStreamBase streamPacketData = this.m_callback.GetBitStreamPacketData();
      BitStream stream = streamPacketData.Stream;
      stream.WriteTypeId(typeIdByType);
      stream.WriteNetworkId(networkIdByObject);
      stream.WriteNetworkId(networkId);
      bool flag = obj is IMyStreamableReplicable streamableReplicable && streamableReplicable.NeedsToBeStreamed;
      if (streamableReplicable != null && !streamableReplicable.NeedsToBeStreamed)
        stream.WriteByte((byte) (replicableGroup.Count - 1));
      else
        stream.WriteByte((byte) replicableGroup.Count);
      for (int index = 0; index < replicableGroup.Count; ++index)
      {
        if (flag || !replicableGroup[index].IsStreaming)
          stream.WriteNetworkId(this.GetNetworkIdByObject((IMyNetObject) replicableGroup[index]));
      }
      if (flag)
      {
        client.Replicables[obj].IsStreaming = true;
        this.m_callback.SendReplicationCreateStreamed((IPacketData) streamPacketData, clientEndpoint);
      }
      else
      {
        obj.OnSave(stream, clientEndpoint);
        this.m_callback.SendReplicationCreate((IPacketData) streamPacketData, clientEndpoint);
      }
    }

    private void SendReplicationDestroy(IMyReplicable obj, List<EndpointId> recipients)
    {
      MyPacketDataBitStreamBase streamPacketData = this.m_callback.GetBitStreamPacketData();
      streamPacketData.Stream.WriteNetworkId(this.GetNetworkIdByObject((IMyNetObject) obj));
      this.m_callback.SendReplicationDestroy((IPacketData) streamPacketData, recipients);
    }

    public void ReplicableReady(MyPacket packet)
    {
      NetworkId id = packet.BitStream.ReadNetworkId();
      bool flag = packet.BitStream.ReadBool();
      if (!packet.BitStream.CheckTerminator())
        throw new EndOfStreamException("Invalid BitStream terminator");
      MyClient client;
      if (this.m_clientStates.TryGetValue(packet.Sender, out client))
      {
        if (this.GetObjectByNetworkId(id) is IMyReplicable objectByNetworkId)
        {
          MyReplicableClientData replicableClientData;
          if (client.Replicables.TryGetValue(objectByNetworkId, out replicableClientData))
          {
            if (flag)
            {
              replicableClientData.IsPending = false;
              replicableClientData.IsStreaming = false;
              --client.PendingReplicables;
              if (client.WantsBatchCompleteConfirmation && client.PendingReplicables == 0)
              {
                this.m_callback.SendPendingReplicablesDone(packet.Sender);
                client.WantsBatchCompleteConfirmation = false;
              }
            }
          }
          else if (!flag)
            this.RemoveForClient(objectByNetworkId, client, false);
        }
        if (objectByNetworkId != null)
          this.ProcessBlocker(objectByNetworkId, packet.Sender, client, (IMyReplicable) null);
      }
      packet.Return();
    }

    public void ReplicableRequest(MyPacket packet)
    {
      long entityId = packet.BitStream.ReadInt64();
      bool flag = packet.BitStream.ReadBool();
      byte num = 0;
      if (flag)
        num = packet.BitStream.ReadByte();
      IMyReplicable replicableByEntityId = this.m_callback.GetReplicableByEntityId(entityId);
      MyClient myClient;
      if (this.m_clientStates.TryGetValue(packet.Sender, out myClient))
      {
        if (flag)
        {
          if (replicableByEntityId != null)
            myClient.PermanentReplicables[replicableByEntityId] = num;
        }
        else if (replicableByEntityId != null)
          myClient.PermanentReplicables.Remove(replicableByEntityId);
      }
      packet.Return();
    }

    private bool ProcessBlocker(
      IMyReplicable replicable,
      Endpoint endpoint,
      MyClient client,
      IMyReplicable parent)
    {
      if (client.BlockedReplicables.ContainsKey(replicable))
      {
        MyReplicationServer.MyDestroyBlocker blockedReplicable = client.BlockedReplicables[replicable];
        if (blockedReplicable.IsProcessing)
          return true;
        blockedReplicable.IsProcessing = true;
        foreach (IMyReplicable blocker in blockedReplicable.Blockers)
        {
          if (!client.IsReplicableReady(replicable) || !client.IsReplicableReady(blocker))
          {
            blockedReplicable.IsProcessing = false;
            return false;
          }
          bool flag = true;
          if (blocker != parent)
            flag = this.ProcessBlocker(blocker, endpoint, client, replicable);
          if (!flag)
          {
            blockedReplicable.IsProcessing = false;
            return false;
          }
        }
        client.BlockedReplicables.Remove(replicable);
        if (blockedReplicable.Remove)
          this.RemoveForClient(replicable, client, true);
        blockedReplicable.IsProcessing = false;
      }
      return true;
    }

    private void AddStateGroups(IMyReplicable replicable)
    {
      using (this.m_tmpGroups)
      {
        if (replicable is IMyStreamableReplicable streamableReplicable)
          streamableReplicable.CreateStreamingStateGroup();
        replicable.GetStateGroups((List<IMyStateGroup>) this.m_tmpGroups);
        foreach (IMyNetObject tmpGroup in (List<IMyStateGroup>) this.m_tmpGroups)
          this.AddNetworkObjectServer(tmpGroup);
        this.m_replicableGroups.Add(replicable, new List<IMyStateGroup>((IEnumerable<IMyStateGroup>) this.m_tmpGroups));
      }
    }

    private void RemoveStateGroups(IMyReplicable replicable)
    {
      foreach (MyClient client in (IEnumerable<MyClient>) this.m_clientStates.Values)
        this.RemoveClientReplicable(replicable, client);
      foreach (IMyStateGroup myStateGroup in this.m_replicableGroups[replicable])
      {
        this.RemoveNetworkedObject((IMyNetObject) myStateGroup);
        myStateGroup.Destroy();
      }
      this.m_replicableGroups.Remove(replicable);
    }

    private void AddClientReplicable(IMyReplicable replicable, MyClient client, bool force)
    {
      client.Replicables.Add(replicable, new MyReplicableClientData());
      ++client.PendingReplicables;
      if (!this.m_replicableGroups.ContainsKey(replicable))
        return;
      foreach (IMyStateGroup myStateGroup in this.m_replicableGroups[replicable])
      {
        NetworkId networkIdByObject = this.GetNetworkIdByObject((IMyNetObject) myStateGroup);
        if (!myStateGroup.IsStreaming || (replicable as IMyStreamableReplicable).NeedsToBeStreamed)
        {
          client.StateGroups.Add(myStateGroup, new MyStateDataEntry(replicable, networkIdByObject, myStateGroup));
          this.ScheduleStateGroupSync(client, client.StateGroups[myStateGroup], this.SyncFrameCounter, false);
          myStateGroup.CreateClientData(client.State);
          if (force)
            myStateGroup.ForceSend(client.State);
        }
      }
    }

    private void RemoveClientReplicable(IMyReplicable replicable, MyClient client)
    {
      if (!this.m_replicableGroups.ContainsKey(replicable))
        return;
      using (this.m_tmpGroups)
      {
        replicable.GetStateGroups((List<IMyStateGroup>) this.m_tmpGroups);
        foreach (IMyStateGroup key in this.m_replicableGroups[replicable])
        {
          key.DestroyClientData(client.State);
          if (client.StateGroups.ContainsKey(key))
          {
            if (client.DirtyQueue.Contains(client.StateGroups[key]))
              client.DirtyQueue.Remove(client.StateGroups[key]);
            client.StateGroups.Remove(key);
          }
        }
        MyReplicableClientData replicableClientData;
        if (client.Replicables.TryGetValue(replicable, out replicableClientData) && replicableClientData.IsPending)
          --client.PendingReplicables;
        client.Replicables.Remove(replicable);
        replicable.OnUnreplication();
        this.m_tmpGroups.Clear();
      }
    }

    private bool ShouldSendEvent(
      IMyNetObject eventInstance,
      Vector3D? position,
      MyClient client,
      CallSite site)
    {
      if (position.HasValue || site.HasDistanceRadius)
      {
        if (!client.State.Position.HasValue)
          return false;
        float num1;
        if (site.HasDistanceRadius)
        {
          num1 = site.DistanceRadiusSquared;
        }
        else
        {
          float? replicationRange = client.State.ReplicationRange;
          float num2 = replicationRange.HasValue ? replicationRange.GetValueOrDefault() : (float) MyLayers.GetSyncDistance();
          num1 = num2 * num2;
        }
        if (Vector3D.DistanceSquared(!position.HasValue ? (eventInstance as IMyReplicable).GetAABB().Center : position.Value, client.State.Position.Value) > (double) num1)
          return false;
      }
      if (eventInstance == null)
        return true;
      return eventInstance is IMyReplicable && client.Replicables.ContainsKey((IMyReplicable) eventInstance);
    }

    public override MyClientStateBase GetClientData(Endpoint endpointId)
    {
      MyClient myClient;
      return !this.m_clientStates.TryGetValue(endpointId, out myClient) ? (MyClientStateBase) null : myClient.State;
    }

    protected override bool DispatchBlockingEvent(
      IPacketData data,
      CallSite site,
      EndpointId target,
      IMyNetObject targetReplicable,
      Vector3D? position,
      IMyNetObject blockingReplicable)
    {
      Endpoint key = new Endpoint(target, (byte) 0);
      IMyReplicable blockingReplicable1 = blockingReplicable as IMyReplicable;
      IMyReplicable targetReplicable1 = targetReplicable as IMyReplicable;
      if (site.HasBroadcastFlag || site.HasBroadcastExceptFlag)
      {
        foreach (KeyValuePair<Endpoint, MyClient> clientState in this.m_clientStates)
        {
          if ((!site.HasBroadcastExceptFlag || !(clientState.Key.Id == target)) && (clientState.Key.Index == (byte) 0 && this.ShouldSendEvent(targetReplicable, position, clientState.Value, site)))
            MyReplicationServer.TryAddBlockerForClient(clientState.Value, targetReplicable1, blockingReplicable1);
        }
      }
      else
      {
        MyClient client;
        if (site.HasClientFlag && this.m_localEndpoint != target && (this.m_clientStates.TryGetValue(key, out client) && this.ShouldSendEvent(targetReplicable, position, client, site)))
          MyReplicationServer.TryAddBlockerForClient(client, targetReplicable1, blockingReplicable1);
      }
      return this.DispatchEvent(data, site, target, targetReplicable, position);
    }

    private static void TryAddBlockerForClient(
      MyClient client,
      IMyReplicable targetReplicable,
      IMyReplicable blockingReplicable)
    {
      if (client.IsReplicableReady(targetReplicable) && client.IsReplicableReady(blockingReplicable) && (!client.BlockedReplicables.ContainsKey(targetReplicable) && !client.BlockedReplicables.ContainsKey(blockingReplicable)))
        return;
      MyReplicationServer.MyDestroyBlocker myDestroyBlocker1;
      if (!client.BlockedReplicables.TryGetValue(targetReplicable, out myDestroyBlocker1))
      {
        myDestroyBlocker1 = new MyReplicationServer.MyDestroyBlocker();
        client.BlockedReplicables.Add(targetReplicable, myDestroyBlocker1);
      }
      myDestroyBlocker1.Blockers.Add(blockingReplicable);
      MyReplicationServer.MyDestroyBlocker myDestroyBlocker2;
      if (!client.BlockedReplicables.TryGetValue(blockingReplicable, out myDestroyBlocker2))
      {
        myDestroyBlocker2 = new MyReplicationServer.MyDestroyBlocker();
        client.BlockedReplicables.Add(blockingReplicable, myDestroyBlocker2);
      }
      myDestroyBlocker2.Blockers.Add(targetReplicable);
    }

    protected override bool DispatchEvent(
      IPacketData data,
      CallSite site,
      EndpointId target,
      IMyNetObject eventInstance,
      Vector3D? position)
    {
      if (MyReplicationServer.m_recipients == null)
        MyReplicationServer.m_recipients = new List<EndpointId>();
      Endpoint key = new Endpoint(target, (byte) 0);
      bool flag = false;
      if (site.HasBroadcastFlag || site.HasBroadcastExceptFlag)
      {
        foreach (KeyValuePair<Endpoint, MyClient> clientState in this.m_clientStates)
        {
          if ((!site.HasBroadcastExceptFlag || !(clientState.Key.Id == target)) && (clientState.Key.Index == (byte) 0 && this.ShouldSendEvent(eventInstance, position, clientState.Value, site)))
            MyReplicationServer.m_recipients.Add(clientState.Key.Id);
        }
        if (MyReplicationServer.m_recipients.Count > 0)
        {
          this.DispatchEvent(data, MyReplicationServer.m_recipients, site.IsReliable);
          flag = true;
          MyReplicationServer.m_recipients.Clear();
        }
      }
      else
      {
        MyClient client;
        if (site.HasClientFlag && this.m_localEndpoint != target && (this.m_clientStates.TryGetValue(key, out client) && this.ShouldSendEvent(eventInstance, position, client, site)))
        {
          this.DispatchEvent(data, client, site.IsReliable);
          flag = true;
        }
      }
      if (!flag)
        data.Return();
      return MyReplicationLayerBase.ShouldServerInvokeLocally(site, this.m_localEndpoint, target);
    }

    private void DispatchEvent(IPacketData data, MyClient client, bool reliable)
    {
      MyReplicationServer.m_recipients.Add(client.State.EndpointId.Id);
      this.DispatchEvent(data, MyReplicationServer.m_recipients, reliable);
      MyReplicationServer.m_recipients.Clear();
    }

    private void DispatchEvent(IPacketData data, List<EndpointId> recipients, bool reliable) => this.m_callback.SendEvent(data, reliable, recipients);

    protected override void OnEvent(
      MyPacketDataBitStreamBase data,
      CallSite site,
      object obj,
      IMyNetObject sendAs,
      Vector3D? position,
      EndpointId source)
    {
      MyClientStateBase clientData = this.GetClientData(new Endpoint(source, (byte) 0));
      if (clientData == null)
        data.Return();
      else if (site.HasServerInvokedFlag)
      {
        data.Return();
        this.m_callback.ValidationFailed(source.Value, additionalInfo: ("ServerInvoked " + site.ToString()), stackTrace: false);
      }
      else
      {
        if (sendAs is IMyReplicable myReplicable)
        {
          ValidationResult validationResult = myReplicable.HasRights(source, site.ValidationFlags);
          if (validationResult != ValidationResult.Passed)
          {
            data.Return();
            this.m_callback.ValidationFailed(source.Value, validationResult.HasFlag((Enum) ValidationResult.Kick), validationResult.ToString() + " " + site.ToString(), false);
            return;
          }
        }
        if (!this.Invoke(site, data.Stream, obj, source, clientData, true))
        {
          data.Return();
        }
        else
        {
          if (!data.Stream.CheckTerminator())
            throw new EndOfStreamException("Invalid BitStream terminator");
          if (site.HasClientFlag || site.HasBroadcastFlag || site.HasBroadcastExceptFlag)
            this.DispatchEvent((IPacketData) data, site, source, sendAs, position);
          else
            data.Return();
        }
      }
    }

    public void SendJoinResult(ref JoinResultMsg msg, ulong sendTo)
    {
      MyPacketDataBitStreamBase streamPacketData = this.m_callback.GetBitStreamPacketData();
      MySerializer.Write<JoinResultMsg>(streamPacketData.Stream, ref msg);
      this.m_callback.SendJoinResult((IPacketData) streamPacketData, new EndpointId(sendTo));
    }

    public void SendWorldData(ref ServerDataMsg msg)
    {
      MyPacketDataBitStreamBase streamPacketData = this.m_callback.GetBitStreamPacketData();
      MySerializer.Write<ServerDataMsg>(streamPacketData.Stream, ref msg);
      this.m_endpoints.Clear();
      foreach (KeyValuePair<Endpoint, MyClient> clientState in this.m_clientStates)
        this.m_endpoints.Add(clientState.Key.Id);
      this.m_callback.SendWorldData((IPacketData) streamPacketData, this.m_endpoints);
    }

    public void SendWorld(byte[] worldData, EndpointId sendTo)
    {
      MyPacketDataBitStreamBase streamPacketData = this.m_callback.GetBitStreamPacketData();
      MySerializer.Write<byte[]>(streamPacketData.Stream, ref worldData);
      this.m_callback.SendWorld((IPacketData) streamPacketData, sendTo);
    }

    public void SendPlayerData(Action<MyPacketDataBitStreamBase> serializer)
    {
      MyPacketDataBitStreamBase streamPacketData = this.m_callback.GetBitStreamPacketData();
      serializer(streamPacketData);
      this.m_endpoints.Clear();
      foreach (KeyValuePair<Endpoint, MyClient> clientState in this.m_clientStates)
        this.m_endpoints.Add(clientState.Key.Id);
      this.m_callback.SendPlayerData((IPacketData) streamPacketData, this.m_endpoints);
    }

    public ConnectedClientDataMsg OnClientConnected(MyPacket packet) => MySerializer.CreateAndRead<ConnectedClientDataMsg>(packet.BitStream);

    public void SendClientConnected(ref ConnectedClientDataMsg msg, ulong sendTo)
    {
      MyPacketDataBitStreamBase streamPacketData = this.m_callback.GetBitStreamPacketData();
      MySerializer.Write<ConnectedClientDataMsg>(streamPacketData.Stream, ref msg);
      this.m_callback.SentClientJoined((IPacketData) streamPacketData, new EndpointId(sendTo));
    }

    public void InvalidateClientCache(IMyReplicable replicable, string storageName)
    {
      foreach (KeyValuePair<Endpoint, MyClient> clientState in this.m_clientStates)
      {
        if (clientState.Value.RemoveCache(replicable, storageName))
          this.m_callback.SendVoxelCacheInvalidated(storageName, clientState.Key.Id);
      }
    }

    public void InvalidateSingleClientCache(string storageName, EndpointId clientId)
    {
      foreach (KeyValuePair<Endpoint, MyClient> clientState in this.m_clientStates)
      {
        if (clientState.Key.Id == clientId)
          clientState.Value.RemoveCache((IMyReplicable) null, storageName);
      }
    }

    public MyTimeSpan GetClientRelevantServerTimestamp(Endpoint clientEndpoint) => this.m_serverTimeStamp;

    public void GetClientPings(out SerializableDictionary<ulong, short> pings)
    {
      pings = new SerializableDictionary<ulong, short>();
      foreach (KeyValuePair<Endpoint, MyClient> clientState in this.m_clientStates)
        pings[clientState.Key.Id.Value] = clientState.Value.State.Ping;
    }

    public void ResendMissingReplicableChildren(IMyEventProxy target) => this.ResendMissingReplicableChildren(this.GetProxyTarget(target) as IMyReplicable);

    private void ResendMissingReplicableChildren(IMyReplicable replicable)
    {
      this.m_replicables.GetAllChildren(replicable, this.m_tmpReplicableList);
      foreach (KeyValuePair<Endpoint, MyClient> clientState in this.m_clientStates)
      {
        if (clientState.Value.HasReplicable(replicable))
        {
          foreach (IMyReplicable tmpReplicable in this.m_tmpReplicableList)
          {
            if (!clientState.Value.HasReplicable(tmpReplicable))
              this.AddForClient(tmpReplicable, clientState.Key, clientState.Value, false);
          }
        }
      }
      this.m_tmpReplicableList.Clear();
    }

    public void SetClientBatchConfrmation(Endpoint clientEndpoint, bool value)
    {
      MyClient myClient;
      if (!this.m_clientStates.TryGetValue(clientEndpoint, out myClient))
        return;
      myClient.WantsBatchCompleteConfirmation = value;
      if (!value)
        return;
      myClient.ResetLayerTimers();
    }

    public bool IsReplicated(IMyReplicable replicable)
    {
      foreach (MyClient myClient in (IEnumerable<MyClient>) this.m_clientStates.Values)
      {
        if (myClient.HasReplicable(replicable) && myClient.IsReplicableReady(replicable))
          return true;
      }
      return false;
    }

    public override string GetMultiplayerStat()
    {
      StringBuilder stringBuilder = new StringBuilder();
      string multiplayerStat = base.GetMultiplayerStat();
      stringBuilder.Append(multiplayerStat);
      stringBuilder.AppendLine("Client state info:");
      foreach (KeyValuePair<Endpoint, MyClient> clientState in this.m_clientStates)
      {
        string str = "    Endpoint: " + (object) clientState.Key + ", Blocked Close Msgs Count: " + (object) clientState.Value.BlockedReplicables.Count;
        stringBuilder.AppendLine(str);
      }
      return stringBuilder.ToString();
    }

    internal class MyDestroyBlocker
    {
      public bool Remove;
      public bool IsProcessing;
      public readonly List<IMyReplicable> Blockers = new List<IMyReplicable>();
    }
  }
}
