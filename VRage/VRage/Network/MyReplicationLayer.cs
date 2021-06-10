// Decompiled with JetBrains decompiler
// Type: VRage.Network.MyReplicationLayer
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using VRage.Library;
using VRage.Library.Algorithms;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Profiler;
using VRage.Replication;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace VRage.Network
{
  public abstract class MyReplicationLayer : MyReplicationLayerBase, IDisposable, INetObjectResolver
  {
    private readonly SequenceIdGenerator m_networkIdGenerator = SequenceIdGenerator.CreateWithStopwatch(TimeSpan.FromSeconds(1800.0), 100000);
    protected readonly bool m_isNetworkAuthority;
    private readonly Dictionary<NetworkId, IMyNetObject> m_networkIDToObject = new Dictionary<NetworkId, IMyNetObject>();
    private readonly Dictionary<IMyNetObject, NetworkId> m_objectToNetworkID = new Dictionary<IMyNetObject, NetworkId>();
    private readonly Dictionary<IMyEventProxy, IMyProxyTarget> m_proxyToTarget = new Dictionary<IMyEventProxy, IMyProxyTarget>();
    private readonly Dictionary<Type, Boxed<MyReplicationLayer.NetworkObjectStat>> m_networkObjectStats = new Dictionary<Type, Boxed<MyReplicationLayer.NetworkObjectStat>>();
    protected MyReplicablesBase m_replicables;
    private const int TIMESTAMP_CORRECTION_MINIMUM = 10;
    private const float SMOOTH_TIMESTAMP_CORRECTION_AMPLITUDE = 1f;
    public float PingSmoothFactor = 3f;
    private readonly FastResourceLock m_networkObjectLock = new FastResourceLock();
    protected long SyncFrameCounter;
    private readonly Thread m_mainThread;
    [ThreadStatic]
    private static MyReplicationLayer.ActiveSerializationData m_currentSerializationData;
    private readonly Queue<CallSite> m_lastFiveSites = new Queue<CallSite>(5);
    private readonly Dictionary<(Type Type, uint Event), (int Count, int Size)> m_sentEvents = new Dictionary<(Type, uint), (int, int)>();
    private readonly Dictionary<(Type Type, uint Event), (int Count, int Size)> m_receivedEvents = new Dictionary<(Type, uint), (int, int)>();
    private object m_eventCountLock = new object();
    public const bool CollectingEventStats = false;

    public bool UseSmoothPing { get; set; }

    public bool UseSmoothCorrection { get; set; }

    public float SmoothCorrectionAmplitude { get; set; }

    public int TimestampCorrectionMinimum { get; set; }

    protected MyReplicationLayer(
      bool isNetworkAuthority,
      EndpointId localEndpoint,
      Thread mainThread)
    {
      this.m_mainThread = mainThread;
      this.TimestampCorrectionMinimum = 10;
      this.SmoothCorrectionAmplitude = 1f;
      this.m_isNetworkAuthority = isNetworkAuthority;
      this.SetLocalEndpoint(localEndpoint);
    }

    public virtual void Dispose()
    {
      using (this.m_networkObjectLock.AcquireExclusiveUsing())
      {
        this.m_networkObjectStats.Clear();
        this.m_networkIDToObject.Clear();
        this.m_objectToNetworkID.Clear();
        this.m_proxyToTarget.Clear();
      }
    }

    public override void AdvanceSyncTime() => ++this.SyncFrameCounter;

    public virtual void SetPriorityMultiplier(EndpointId id, float priority)
    {
    }

    protected Type GetTypeByTypeId(TypeId typeId) => this.m_typeTable.Get(typeId).Type;

    protected TypeId GetTypeIdByType(Type type) => this.m_typeTable.Get(type).TypeId;

    public bool IsTypeReplicated(Type type)
    {
      MySynchronizedTypeInfo typeInfo;
      return this.m_typeTable.TryGet(type, out typeInfo) && typeInfo.IsReplicated;
    }

    protected void AddNetworkObjectServer(IMyNetObject obj) => this.AddNetworkObject(new NetworkId(this.m_networkIdGenerator.NextId()), obj);

    protected virtual void AddNetworkObject(NetworkId networkID, IMyNetObject obj)
    {
      using (this.m_networkObjectLock.AcquireExclusiveUsing())
      {
        IMyNetObject myNetObject;
        if (!this.m_networkIDToObject.TryGetValue(networkID, out myNetObject))
        {
          this.m_networkIDToObject.Add(networkID, obj);
          this.m_objectToNetworkID.Add(obj, networkID);
          Type type = obj.GetType();
          Boxed<MyReplicationLayer.NetworkObjectStat> boxed;
          if (!this.m_networkObjectStats.TryGetValue(type, out boxed))
          {
            boxed = new Boxed<MyReplicationLayer.NetworkObjectStat>(new MyReplicationLayer.NetworkObjectStat()
            {
              Group = MyReplicationLayer.GetNetworkObjectGroup(obj)
            });
            this.m_networkObjectStats[type] = boxed;
          }
          ++boxed.BoxedValue.Count;
          if (!(obj is IMyProxyTarget myProxyTarget) || myProxyTarget.Target == null || this.m_proxyToTarget.ContainsKey(myProxyTarget.Target))
            return;
          this.m_proxyToTarget.Add(myProxyTarget.Target, myProxyTarget);
        }
        else
        {
          if (obj == null || myNetObject == null)
            return;
          MyLog.Default.WriteLine("Replicated object already exists adding : " + obj.ToString() + " existing : " + myNetObject.ToString() + " id : " + networkID.ToString());
        }
      }
    }

    protected NetworkId RemoveNetworkedObject(IMyNetObject obj)
    {
      using (this.m_networkObjectLock.AcquireExclusiveUsing())
      {
        NetworkId networkID;
        if (this.m_objectToNetworkID.TryGetValue(obj, out networkID))
          this.RemoveNetworkedObjectInternal(networkID, obj);
        return networkID;
      }
    }

    protected virtual void RemoveNetworkedObjectInternal(NetworkId networkID, IMyNetObject obj)
    {
      this.m_objectToNetworkID.Remove(obj);
      this.m_networkIDToObject.Remove(networkID);
      if (obj is IMyProxyTarget myProxyTarget && myProxyTarget.Target != null)
        this.m_proxyToTarget.Remove(myProxyTarget.Target);
      --this.m_networkObjectStats[obj.GetType()].BoxedValue.Count;
      this.m_networkIdGenerator.Return(networkID.Value);
    }

    public bool TryGetNetworkIdByObject(IMyNetObject obj, out NetworkId networkId)
    {
      if (obj == null)
      {
        networkId = NetworkId.Invalid;
        return false;
      }
      using (this.m_networkObjectLock.AcquireSharedUsing())
        return this.m_objectToNetworkID.TryGetValue(obj, out networkId);
    }

    public NetworkId GetNetworkIdByObject(IMyNetObject obj)
    {
      if (obj == null)
        return NetworkId.Invalid;
      using (this.m_networkObjectLock.AcquireSharedUsing())
        return this.m_objectToNetworkID.GetValueOrDefault<IMyNetObject, NetworkId>(obj, NetworkId.Invalid);
    }

    protected IMyNetObject GetObjectByNetworkId(NetworkId id)
    {
      using (this.m_networkObjectLock.AcquireSharedUsing())
        return this.m_networkIDToObject.GetValueOrDefault<NetworkId, IMyNetObject>(id);
    }

    public IMyProxyTarget GetProxyTarget(IMyEventProxy proxy)
    {
      using (this.m_networkObjectLock.AcquireSharedUsing())
        return this.m_proxyToTarget.GetValueOrDefault<IMyEventProxy, IMyProxyTarget>(proxy);
    }

    public abstract void UpdateBefore();

    public abstract void UpdateAfter();

    public abstract void UpdateClientStateGroups();

    public abstract void Simulate();

    public abstract void SendUpdate();

    public abstract MyTimeSpan GetSimulationUpdateTime();

    protected abstract MyPacketDataBitStreamBase GetBitStreamPacketData();

    public abstract void Disconnect();

    public void ReportReplicatedObjects()
    {
      MyStatsGraph.ProfileAdvanced(true);
      MyStatsGraph.Begin("ReportObjects", member: nameof (ReportReplicatedObjects), line: 253, file: "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationLayer.cs");
      this.ReportObjects("Replicable objects", MyReplicationLayer.NetworkObjectGroup.Replicable);
      this.ReportObjects("State groups", MyReplicationLayer.NetworkObjectGroup.StatGroup);
      this.ReportObjects("Unknown net objects", MyReplicationLayer.NetworkObjectGroup.Unknown);
      MyStatsGraph.End(member: nameof (ReportReplicatedObjects), line: 257, file: "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationLayer.cs");
      MyStatsGraph.ProfileAdvanced(false);
    }

    private static MyReplicationLayer.NetworkObjectGroup GetNetworkObjectGroup(
      IMyNetObject obj)
    {
      MyReplicationLayer.NetworkObjectGroup networkObjectGroup = MyReplicationLayer.NetworkObjectGroup.None;
      if (obj is IMyReplicable)
        networkObjectGroup |= MyReplicationLayer.NetworkObjectGroup.Replicable;
      if (obj is IMyStateGroup)
        networkObjectGroup |= MyReplicationLayer.NetworkObjectGroup.StatGroup;
      if (networkObjectGroup == MyReplicationLayer.NetworkObjectGroup.None)
        networkObjectGroup |= MyReplicationLayer.NetworkObjectGroup.Unknown;
      return networkObjectGroup;
    }

    private void ReportObjects(string name, MyReplicationLayer.NetworkObjectGroup group)
    {
      using (this.m_networkObjectLock.AcquireSharedUsing())
      {
        int num = 0;
        MyStatsGraph.Begin(name, member: nameof (ReportObjects), line: 278, file: "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationLayer.cs");
        foreach (KeyValuePair<Type, Boxed<MyReplicationLayer.NetworkObjectStat>> networkObjectStat in this.m_networkObjectStats)
        {
          Boxed<MyReplicationLayer.NetworkObjectStat> boxed = networkObjectStat.Value;
          if (boxed.BoxedValue.Count > 0 && boxed.BoxedValue.Group.HasFlag((Enum) group))
          {
            num += boxed.BoxedValue.Count;
            MyStatsGraph.Begin(networkObjectStat.Key.Name, member: nameof (ReportObjects), line: 285, file: "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationLayer.cs");
            MyStatsGraph.End(new float?((float) boxed.BoxedValue.Count), byteFormat: "{0:.} x", callFormat: "", member: nameof (ReportObjects), line: 286, file: "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationLayer.cs");
          }
        }
        MyStatsGraph.End(new float?((float) num), byteFormat: "{0:.} x", callFormat: "", member: nameof (ReportObjects), line: 289, file: "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationLayer.cs");
      }
    }

    public virtual MyClientStateBase GetClientData(Endpoint endpointId) => (MyClientStateBase) null;

    internal void SerializeTypeTable(BitStream stream) => this.m_typeTable.Serialize(stream);

    public void RefreshReplicableHierarchy(IMyReplicable replicable) => this.m_replicables.Refresh(replicable);

    public virtual string GetMultiplayerStat() => "Multiplayer Statistics:" + MyEnvironment.NewLine;

    public abstract void UpdateStatisticsData(
      int outgoing,
      int incoming,
      int tamperred,
      float gcMemory,
      float processMemory);

    public abstract MyPacketStatistics ClearClientStatistics();

    public abstract MyPacketStatistics ClearServerStatistics();

    [Conditional("DEBUG")]
    protected void CheckThread()
    {
    }

    public static IMyReplicable CurrentSerializingReplicable => MyReplicationLayer.m_currentSerializationData.Replicable;

    public static Endpoint CurrentSerializationDestinationEndpoint => MyReplicationLayer.m_currentSerializationData.TargetEndpoint;

    public static MyReplicationLayer.CurrentSerializingReplicableToken StartSerializingReplicable(
      IMyReplicable replicable,
      Endpoint targetEndpoint)
    {
      MyReplicationLayer.m_currentSerializationData.Replicable = replicable;
      MyReplicationLayer.m_currentSerializationData.TargetEndpoint = targetEndpoint;
      return new MyReplicationLayer.CurrentSerializingReplicableToken();
    }

    private static void StopSerializingReplicable() => MyReplicationLayer.m_currentSerializationData = new MyReplicationLayer.ActiveSerializationData();

    protected abstract bool DispatchEvent(
      IPacketData stream,
      CallSite site,
      EndpointId recipient,
      IMyNetObject eventInstance,
      Vector3D? position);

    protected abstract bool DispatchBlockingEvent(
      IPacketData stream,
      CallSite site,
      EndpointId recipient,
      IMyNetObject eventInstance,
      Vector3D? position,
      IMyNetObject blockedNetObject);

    protected abstract void OnEvent(
      MyPacketDataBitStreamBase data,
      CallSite site,
      object obj,
      IMyNetObject sendAs,
      Vector3D? position,
      EndpointId source);

    protected override sealed void DispatchEvent<T1, T2, T3, T4, T5, T6, T7, T8>(
      CallSite callSite,
      EndpointId recipient,
      Vector3D? position,
      ref T1 arg1,
      ref T2 arg2,
      ref T3 arg3,
      ref T4 arg4,
      ref T5 arg5,
      ref T6 arg6,
      ref T7 arg7,
      ref T8 arg8)
    {
      uint id = callSite.Id;
      IMyNetObject eventInstance;
      NetworkId networkId1;
      if (callSite.MethodInfo.IsStatic)
      {
        eventInstance = (IMyNetObject) null;
        networkId1 = NetworkId.Invalid;
      }
      else
      {
        if ((object) arg1 == null)
          throw new InvalidOperationException("First argument (the instance on which is event invoked) cannot be null for non-static events");
        if ((object) arg1 is IMyEventProxy)
        {
          eventInstance = (IMyNetObject) this.GetProxyTarget((IMyEventProxy) (object) arg1);
          if (eventInstance == null)
          {
            MyLog.Default.WriteLine("Raising event on object which is not recognized by replication: " + (object) arg1);
            return;
          }
          id += (uint) this.m_typeTable.Get(eventInstance.GetType()).EventTable.Count;
          networkId1 = this.GetNetworkIdByObject(eventInstance);
        }
        else
        {
          if (!((object) arg1 is IMyNetObject))
            throw new InvalidOperationException("Instance events may be called only on IMyNetObject or IMyEventProxy");
          eventInstance = (IMyNetObject) (object) arg1;
          networkId1 = this.GetNetworkIdByObject(eventInstance);
        }
      }
      NetworkId networkId2 = NetworkId.Invalid;
      IMyNetObject blockedNetObject = (IMyNetObject) null;
      if ((object) arg8 is IMyEventProxy && callSite.IsBlocking)
      {
        blockedNetObject = (IMyNetObject) this.GetProxyTarget((IMyEventProxy) (object) arg8);
        networkId2 = this.GetNetworkIdByObject(blockedNetObject);
      }
      else
      {
        if ((object) arg8 is IMyEventProxy && !callSite.IsBlocking)
          throw new InvalidOperationException("Rising blocking event but event itself does not have Blocking attribute");
        if (!((object) arg8 is IMyEventProxy) && callSite.IsBlocking)
          throw new InvalidOperationException("Event contain Blocking attribute but blocked event proxy is not set or raised event is not blocking one");
      }
      CallSite<T1, T2, T3, T4, T5, T6, T7> site = (CallSite<T1, T2, T3, T4, T5, T6, T7>) callSite;
      MyPacketDataBitStreamBase streamPacketData = this.GetBitStreamPacketData();
      streamPacketData.Stream.WriteNetworkId(networkId1);
      streamPacketData.Stream.WriteNetworkId(networkId2);
      streamPacketData.Stream.WriteUInt16((ushort) id);
      streamPacketData.Stream.WriteBool(position.HasValue);
      if (position.HasValue)
        streamPacketData.Stream.Write(position.Value);
      long bitPosition = streamPacketData.Stream.BitPosition;
      using (MySerializerNetObject.Using((INetObjectResolver) this))
        site.Serializer(arg1, streamPacketData.Stream, ref arg2, ref arg3, ref arg4, ref arg5, ref arg6, ref arg7);
      streamPacketData.Stream.Terminate();
      if (!(networkId2.IsInvalid ? this.DispatchEvent((IPacketData) streamPacketData, callSite, recipient, eventInstance, position) : this.DispatchBlockingEvent((IPacketData) streamPacketData, callSite, recipient, eventInstance, position, blockedNetObject)))
        return;
      this.InvokeLocally<T1, T2, T3, T4, T5, T6, T7>(site, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
    }

    public bool Invoke(
      CallSite callSite,
      BitStream stream,
      object obj,
      EndpointId source,
      MyClientStateBase clientState,
      bool validate)
    {
      using (MySerializerNetObject.Using((INetObjectResolver) this))
      {
        using (MyEventContext.Set(source, clientState, false))
          return callSite.Invoke(stream, obj, validate) && (!validate || !MyEventContext.Current.HasValidationFailed);
      }
    }

    public void OnEvent(MyPacket packet)
    {
      MyPacketDataBitStreamBase streamPacketData = this.GetBitStreamPacketData();
      streamPacketData.Stream.ResetRead(packet.BitStream);
      this.ProcessEvent(streamPacketData, packet.Sender.Id);
      packet.Return();
    }

    private void ProcessEvent(MyPacketDataBitStreamBase data, EndpointId sender)
    {
      NetworkId networkId = data.Stream.ReadNetworkId();
      NetworkId blockedNetId = data.Stream.ReadNetworkId();
      uint eventId = (uint) data.Stream.ReadUInt16();
      int num = data.Stream.ReadBool() ? 1 : 0;
      Vector3D? position = new Vector3D?();
      if (num != 0)
        position = new Vector3D?(data.Stream.ReadVector3D());
      this.OnEvent(data, networkId, blockedNetId, eventId, sender, position);
    }

    protected virtual void OnEvent(
      MyPacketDataBitStreamBase data,
      NetworkId networkId,
      NetworkId blockedNetId,
      uint eventId,
      EndpointId sender,
      Vector3D? position)
    {
      CallSite site;
      IMyNetObject sendAs;
      object obj;
      try
      {
        if (networkId.IsInvalid)
        {
          site = this.m_typeTable.StaticEventTable.Get(eventId);
          sendAs = (IMyNetObject) null;
          obj = (object) null;
        }
        else
        {
          sendAs = this.GetObjectByNetworkId(networkId);
          if (sendAs == null || !sendAs.IsValid)
            return;
          MySynchronizedTypeInfo synchronizedTypeInfo = this.m_typeTable.Get(sendAs.GetType());
          int count = synchronizedTypeInfo.EventTable.Count;
          if ((long) eventId < (long) count)
          {
            obj = (object) sendAs;
            site = synchronizedTypeInfo.EventTable.Get(eventId);
          }
          else
          {
            obj = (object) ((IMyProxyTarget) sendAs).Target;
            site = this.m_typeTable.Get(obj.GetType()).EventTable.Get(eventId - (uint) count);
          }
        }
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine("Static: " + (!networkId.IsInvalid).ToString());
        MyLog.Default.WriteLine("EventId: " + (object) eventId);
        MyLog.Default.WriteLine("Last five sites:");
        foreach (object lastFiveSite in this.m_lastFiveSites)
          MyLog.Default.WriteLine(lastFiveSite.ToString());
        throw ex;
      }
      if (this.m_lastFiveSites.Count >= 5)
        this.m_lastFiveSites.Dequeue();
      this.m_lastFiveSites.Enqueue(site);
      this.OnEvent(data, site, obj, sendAs, position, sender);
    }

    void INetObjectResolver.Resolve<T>(BitStream stream, ref T obj)
    {
      if (stream.Reading)
      {
        obj = (T) this.GetObjectByNetworkId(stream.ReadNetworkId());
      }
      else
      {
        NetworkId networkId;
        stream.WriteNetworkId(this.TryGetNetworkIdByObject((IMyNetObject) obj, out networkId) ? networkId : NetworkId.Invalid);
      }
    }

    [Conditional("COLLECT_EVENT_STATS")]
    private void RecordSentEvent(Type type, uint @event, int size) => this.RecordEvent(this.m_sentEvents, type, @event, size);

    [Conditional("COLLECT_EVENT_STATS")]
    private void RecordReceivedEvent(Type type, uint @event, int size) => this.RecordEvent(this.m_receivedEvents, type, @event, size);

    private void RecordEvent(
      Dictionary<(Type Type, uint Event), (int Count, int Size)> dict,
      Type type,
      uint @event,
      int size)
    {
      lock (this.m_eventCountLock)
      {
        (Type, uint) key = (type, @event);
        (int Count, int Size) tuple;
        dict.TryGetValue(key, out tuple);
        dict[key] = (tuple.Count + 1, tuple.Size + size);
      }
    }

    private IEnumerable<(Type Type, MethodInfo Method, int Count, int Size)> GetEventCounts(
      bool received)
    {
      MyReplicationLayer replicationLayer = this;
      foreach (KeyValuePair<(Type, uint), (int, int)> pair in received ? replicationLayer.m_receivedEvents : replicationLayer.m_sentEvents)
      {
        (Type, uint) k;
        (int, int) v;
        pair.Deconstruct<(Type, uint), (int, int)>(out k, out v);
        (Type, uint) tuple1 = k;
        (int, int) tuple2 = v;
        Type type = tuple1.Item1;
        uint id = tuple1.Item2;
        int num1 = tuple2.Item1;
        int num2 = tuple2.Item2;
        MethodInfo methodInfo = !(type == (Type) null) ? replicationLayer.m_typeTable.Get(type).EventTable.Get(id).MethodInfo : replicationLayer.m_typeTable.StaticEventTable.Get(id).MethodInfo;
        yield return (type, methodInfo, num1, num2);
      }
    }

    private void ClearEventCounts()
    {
      this.m_sentEvents.Clear();
      this.m_receivedEvents.Clear();
    }

    public void ReportEvents()
    {
    }

    [Conditional("COLLECT_EVENT_STATS")]
    private void ReportEventsInternal()
    {
      lock (this.m_eventCountLock)
      {
        MyStatsGraph.ProfilePacketStatistics(true);
        MyStatsGraph.Begin("Events Sent", 0, nameof (ReportEventsInternal), 316, "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationLayer.Events.cs");
        this.ReportEvents(false);
        MyStatsGraph.End(member: nameof (ReportEventsInternal), line: 318, file: "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationLayer.Events.cs");
        MyStatsGraph.Begin("Events Received", 0, nameof (ReportEventsInternal), 319, "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationLayer.Events.cs");
        this.ReportEvents(true);
        MyStatsGraph.End(member: nameof (ReportEventsInternal), line: 321, file: "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationLayer.Events.cs");
        MyStatsGraph.ProfilePacketStatistics(false);
        this.ClearEventCounts();
      }
    }

    private void ReportEvents(bool received)
    {
      foreach ((Type Type, MethodInfo Method, int Count, int Size) eventCount in this.GetEventCounts(received))
      {
        MyStatsGraph.Begin(eventCount.Method.Name, member: nameof (ReportEvents), line: 331, file: "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationLayer.Events.cs");
        MyStatsGraph.End(new float?((float) eventCount.Size), numCalls: eventCount.Count, member: nameof (ReportEvents), line: 332, file: "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationLayer.Events.cs");
      }
    }

    [Flags]
    private enum NetworkObjectGroup
    {
      None = 0,
      Replicable = 1,
      StatGroup = 2,
      Unknown = 4,
    }

    private struct NetworkObjectStat
    {
      public int Count;
      public MyReplicationLayer.NetworkObjectGroup Group;
    }

    private struct ActiveSerializationData
    {
      public IMyReplicable Replicable;
      public Endpoint TargetEndpoint;
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct CurrentSerializingReplicableToken : IDisposable
    {
      public void Dispose() => MyReplicationLayer.StopSerializingReplicable();
    }
  }
}
