// Decompiled with JetBrains decompiler
// Type: VRage.Replication.MyEventsBuffer
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace VRage.Replication
{
  public class MyEventsBuffer : IDisposable
  {
    private readonly Stack<MyEventsBuffer.MyBufferedEvent> m_eventPool;
    private readonly Stack<Queue<MyEventsBuffer.MyBufferedEvent>> m_listPool;
    private readonly Dictionary<NetworkId, MyEventsBuffer.MyObjectEventsBuffer> m_buffer = new Dictionary<NetworkId, MyEventsBuffer.MyObjectEventsBuffer>(16);
    private readonly Thread m_mainThread;

    public MyEventsBuffer(Thread mainThread, int eventCapacity = 32)
    {
      this.m_mainThread = mainThread;
      this.m_listPool = new Stack<Queue<MyEventsBuffer.MyBufferedEvent>>(16);
      for (int index = 0; index < 16; ++index)
        this.m_listPool.Push(new Queue<MyEventsBuffer.MyBufferedEvent>(16));
      this.m_eventPool = new Stack<MyEventsBuffer.MyBufferedEvent>(eventCapacity);
      for (int index = 0; index < eventCapacity; ++index)
        this.m_eventPool.Push(new MyEventsBuffer.MyBufferedEvent());
    }

    public void Dispose()
    {
      this.m_eventPool.Clear();
      foreach (KeyValuePair<NetworkId, MyEventsBuffer.MyObjectEventsBuffer> keyValuePair in this.m_buffer)
      {
        foreach (MyEventsBuffer.MyBufferedEvent myBufferedEvent in keyValuePair.Value.Events)
        {
          if (myBufferedEvent.Data != null)
            myBufferedEvent.Data.Return();
        }
      }
      this.m_buffer.Clear();
    }

    private MyEventsBuffer.MyBufferedEvent ObtainEvent() => this.m_eventPool.Count > 0 ? this.m_eventPool.Pop() : new MyEventsBuffer.MyBufferedEvent();

    private void ReturnEvent(MyEventsBuffer.MyBufferedEvent evnt)
    {
      if (evnt.Data != null)
        evnt.Data.Return();
      evnt.Data = (MyPacketDataBitStreamBase) null;
      this.m_eventPool.Push(evnt);
    }

    private Queue<MyEventsBuffer.MyBufferedEvent> ObtainList() => this.m_listPool.Count > 0 ? this.m_listPool.Pop() : new Queue<MyEventsBuffer.MyBufferedEvent>(16);

    private void ReturnList(Queue<MyEventsBuffer.MyBufferedEvent> list) => this.m_listPool.Push(list);

    public void EnqueueEvent(
      MyPacketDataBitStreamBase data,
      NetworkId targetObjectId,
      NetworkId blockingObjectId,
      uint eventId,
      EndpointId sender,
      Vector3D? position)
    {
      MyEventsBuffer.MyBufferedEvent myBufferedEvent = this.ObtainEvent();
      myBufferedEvent.Data = data;
      myBufferedEvent.TargetObjectId = targetObjectId;
      myBufferedEvent.BlockingObjectId = blockingObjectId;
      myBufferedEvent.EventId = eventId;
      myBufferedEvent.Sender = sender;
      myBufferedEvent.IsBarrier = false;
      myBufferedEvent.Position = position;
      MyEventsBuffer.MyObjectEventsBuffer objectEventsBuffer;
      if (!this.m_buffer.TryGetValue(targetObjectId, out objectEventsBuffer))
      {
        objectEventsBuffer = new MyEventsBuffer.MyObjectEventsBuffer()
        {
          Events = this.ObtainList()
        };
        this.m_buffer.Add(targetObjectId, objectEventsBuffer);
      }
      objectEventsBuffer.IsProcessing = false;
      objectEventsBuffer.Events.Enqueue(myBufferedEvent);
    }

    public void EnqueueBarrier(NetworkId targetObjectId, NetworkId blockingObjectId)
    {
      MyEventsBuffer.MyBufferedEvent myBufferedEvent = this.ObtainEvent();
      myBufferedEvent.TargetObjectId = targetObjectId;
      myBufferedEvent.BlockingObjectId = blockingObjectId;
      myBufferedEvent.IsBarrier = true;
      MyEventsBuffer.MyObjectEventsBuffer objectEventsBuffer;
      if (!this.m_buffer.TryGetValue(targetObjectId, out objectEventsBuffer))
      {
        objectEventsBuffer = new MyEventsBuffer.MyObjectEventsBuffer();
        objectEventsBuffer.Events = this.ObtainList();
        this.m_buffer.Add(targetObjectId, objectEventsBuffer);
      }
      objectEventsBuffer.IsProcessing = false;
      objectEventsBuffer.Events.Enqueue(myBufferedEvent);
    }

    public void RemoveEvents(NetworkId objectInstance)
    {
      MyEventsBuffer.MyObjectEventsBuffer objectEventsBuffer;
      if (this.m_buffer.TryGetValue(objectInstance, out objectEventsBuffer))
      {
        foreach (MyEventsBuffer.MyBufferedEvent evnt in objectEventsBuffer.Events)
          this.ReturnEvent(evnt);
        objectEventsBuffer.Events.Clear();
        this.ReturnList(objectEventsBuffer.Events);
        objectEventsBuffer.Events = (Queue<MyEventsBuffer.MyBufferedEvent>) null;
      }
      this.m_buffer.Remove(objectInstance);
    }

    private bool TryLiftBarrier(NetworkId targetObjectId)
    {
      MyEventsBuffer.MyObjectEventsBuffer objectEventsBuffer;
      if (this.m_buffer.TryGetValue(targetObjectId, out objectEventsBuffer))
      {
        MyEventsBuffer.MyBufferedEvent evnt = objectEventsBuffer.Events.Peek();
        if (evnt.IsBarrier && evnt.TargetObjectId.Equals(targetObjectId))
        {
          objectEventsBuffer.Events.Dequeue();
          this.ReturnEvent(evnt);
          return true;
        }
      }
      return false;
    }

    public bool ContainsEvents(NetworkId netId)
    {
      MyEventsBuffer.MyObjectEventsBuffer objectEventsBuffer;
      return this.m_buffer.TryGetValue(netId, out objectEventsBuffer) && objectEventsBuffer.Events.Count > 0;
    }

    public bool ProcessEvents(
      NetworkId targetObjectId,
      MyEventsBuffer.Handler eventHandler,
      MyEventsBuffer.IsBlockedHandler isBlockedHandler,
      NetworkId caller)
    {
      bool flag = false;
      Queue<NetworkId> postProcessQueue = new Queue<NetworkId>();
      MyEventsBuffer.MyObjectEventsBuffer eventsBuffer;
      if (!this.m_buffer.TryGetValue(targetObjectId, out eventsBuffer) || eventsBuffer.IsProcessing)
        return false;
      eventsBuffer.IsProcessing = true;
      int num = this.ProcessEventsBuffer(eventsBuffer, targetObjectId, eventHandler, isBlockedHandler, caller, ref postProcessQueue) ? 1 : 0;
      eventsBuffer.IsProcessing = false;
      if (num == 0)
        return false;
      if (eventsBuffer.Events.Count == 0)
      {
        this.ReturnList(eventsBuffer.Events);
        eventsBuffer.Events = (Queue<MyEventsBuffer.MyBufferedEvent>) null;
        flag = true;
      }
      if (flag)
        this.m_buffer.Remove(targetObjectId);
      while (postProcessQueue.Count > 0)
        this.ProcessEvents(postProcessQueue.Dequeue(), eventHandler, isBlockedHandler, targetObjectId);
      return true;
    }

    private bool ProcessEventsBuffer(
      MyEventsBuffer.MyObjectEventsBuffer eventsBuffer,
      NetworkId targetObjectId,
      MyEventsBuffer.Handler eventHandler,
      MyEventsBuffer.IsBlockedHandler isBlockedHandler,
      NetworkId caller,
      ref Queue<NetworkId> postProcessQueue)
    {
      while (eventsBuffer.Events.Count > 0)
      {
        bool flag = true;
        MyEventsBuffer.MyBufferedEvent myBufferedEvent = eventsBuffer.Events.Peek();
        if (myBufferedEvent.Data != null)
        {
          long bitPosition = myBufferedEvent.Data.Stream.BitPosition;
        }
        if (myBufferedEvent.IsBarrier)
        {
          flag = this.ProcessBarrierEvent(targetObjectId, myBufferedEvent, eventHandler, isBlockedHandler);
        }
        else
        {
          if (myBufferedEvent.BlockingObjectId.IsValid)
          {
            flag = this.ProcessBlockingEvent(targetObjectId, myBufferedEvent, caller, eventHandler, isBlockedHandler, ref postProcessQueue);
          }
          else
          {
            eventHandler(myBufferedEvent.Data, myBufferedEvent.TargetObjectId, myBufferedEvent.BlockingObjectId, myBufferedEvent.EventId, myBufferedEvent.Sender, myBufferedEvent.Position);
            myBufferedEvent.Data = (MyPacketDataBitStreamBase) null;
          }
          if (flag)
          {
            eventsBuffer.Events.Dequeue();
            if (myBufferedEvent.Data != null && !myBufferedEvent.Data.Stream.CheckTerminator())
              MyLog.Default.WriteLine("RPC: Invalid stream terminator");
            this.ReturnEvent(myBufferedEvent);
          }
        }
        if (!flag)
        {
          eventsBuffer.IsProcessing = false;
          return false;
        }
      }
      return true;
    }

    private bool ProcessBarrierEvent(
      NetworkId targetObjectId,
      MyEventsBuffer.MyBufferedEvent eventToProcess,
      MyEventsBuffer.Handler eventHandler,
      MyEventsBuffer.IsBlockedHandler isBlockedHandler)
    {
      return !isBlockedHandler(eventToProcess.TargetObjectId, eventToProcess.BlockingObjectId) && this.ProcessEvents(eventToProcess.BlockingObjectId, eventHandler, isBlockedHandler, targetObjectId);
    }

    private bool ProcessBlockingEvent(
      NetworkId targetObjectId,
      MyEventsBuffer.MyBufferedEvent eventToProcess,
      NetworkId caller,
      MyEventsBuffer.Handler eventHandler,
      MyEventsBuffer.IsBlockedHandler isBlockedHandler,
      ref Queue<NetworkId> postProcessQueue)
    {
      if (isBlockedHandler(eventToProcess.TargetObjectId, eventToProcess.BlockingObjectId))
        return false;
      if (!this.TryLiftBarrier(eventToProcess.BlockingObjectId))
        return this.ProcessEvents(eventToProcess.BlockingObjectId, eventHandler, isBlockedHandler, targetObjectId);
      eventHandler(eventToProcess.Data, eventToProcess.TargetObjectId, eventToProcess.BlockingObjectId, eventToProcess.EventId, eventToProcess.Sender, eventToProcess.Position);
      eventToProcess.Data = (MyPacketDataBitStreamBase) null;
      if (eventToProcess.BlockingObjectId.IsValid && !eventToProcess.BlockingObjectId.Equals(caller))
        postProcessQueue.Enqueue(eventToProcess.BlockingObjectId);
      return true;
    }

    public string GetEventsBufferStat()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Pending Events Buffer:");
      foreach (KeyValuePair<NetworkId, MyEventsBuffer.MyObjectEventsBuffer> keyValuePair in this.m_buffer)
      {
        string str = "    NetworkId: " + (object) keyValuePair.Key + ", EventsCount: " + (object) keyValuePair.Value.Events.Count;
        stringBuilder.AppendLine(str);
      }
      return stringBuilder.ToString();
    }

    [Conditional("DEBUG")]
    private void CheckThread()
    {
    }

    public delegate void Handler(
      MyPacketDataBitStreamBase data,
      NetworkId objectInstance,
      NetworkId blockedNetId,
      uint eventId,
      EndpointId sender,
      Vector3D? position);

    public delegate bool IsBlockedHandler(NetworkId objectInstance, NetworkId blockedNetId);

    private class MyBufferedEvent
    {
      public MyPacketDataBitStreamBase Data;
      public NetworkId TargetObjectId;
      public NetworkId BlockingObjectId;
      public uint EventId;
      public EndpointId Sender;
      public bool IsBarrier;
      public Vector3D? Position;
    }

    private struct MyObjectEventsBuffer
    {
      public Queue<MyEventsBuffer.MyBufferedEvent> Events;
      public bool IsProcessing;
    }
  }
}
