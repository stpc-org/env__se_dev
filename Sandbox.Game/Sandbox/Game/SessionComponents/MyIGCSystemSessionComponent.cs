// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIGCSystemSessionComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.IntergridCommunication;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRageMath;
using VRageMath.PackedVector;
using VRageRender;

namespace Sandbox.Game.SessionComponents
{
  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, 666, typeof (MyObjectBuilder_MyIGCSystemSessionComponent), null, false)]
  internal class MyIGCSystemSessionComponent : MySessionComponentBase
  {
    private static MyIGCSystemSessionComponent m_static;
    private MySwapList<MyIGCSystemSessionComponent.Message> m_messagesForNextTick = new MySwapList<MyIGCSystemSessionComponent.Message>();
    private Queue<MyTuple<int, Action>> m_debugDrawQueue;
    private Dictionary<long, MyIntergridCommunicationContext> m_perPBCommContexts = new Dictionary<long, MyIntergridCommunicationContext>();
    private CachingHashSet<MyIntergridCommunicationContext> m_contextsWithPendingCallbacks = new CachingHashSet<MyIntergridCommunicationContext>();
    private Dictionary<string, CachingHashSet<BroadcastListener>> m_activeBroadcastListeners = new Dictionary<string, CachingHashSet<BroadcastListener>>();
    private List<long> m_idsToInitialize;

    public static MyIGCSystemSessionComponent Static => MyIGCSystemSessionComponent.m_static;

    public Action<MyCubeGrid, HashSet<MyDataBroadcaster>, long> BroadcasterProvider { get; private set; }

    public Func<MyProgrammableBlock, MyDataBroadcaster, long, bool> ConnectionProvider { get; private set; }

    public MyIntergridCommunicationContext GetContextForPB(
      long programmableBlockId)
    {
      MyIntergridCommunicationContext communicationContext;
      this.m_perPBCommContexts.TryGetValue(programmableBlockId, out communicationContext);
      return communicationContext;
    }

    public MyIntergridCommunicationContext GetOrMakeContextFor(
      MyProgrammableBlock block)
    {
      long entityId = block.EntityId;
      MyIntergridCommunicationContext communicationContext;
      if (!this.m_perPBCommContexts.TryGetValue(entityId, out communicationContext))
      {
        communicationContext = new MyIntergridCommunicationContext(block);
        this.m_perPBCommContexts.Add(entityId, communicationContext);
      }
      return communicationContext;
    }

    public void EvictContextFor(MyProgrammableBlock block)
    {
      long entityId = block.EntityId;
      MyIntergridCommunicationContext contextForPb = this.GetContextForPB(entityId);
      if (contextForPb == null)
        return;
      contextForPb.DisposeContext();
      this.m_perPBCommContexts.Remove(entityId);
    }

    public void RegisterBroadcastListener(BroadcastListener listener)
    {
      CachingHashSet<BroadcastListener> cachingHashSet;
      if (!this.m_activeBroadcastListeners.TryGetValue(listener.Tag, out cachingHashSet))
      {
        cachingHashSet = new CachingHashSet<BroadcastListener>();
        this.m_activeBroadcastListeners.Add(listener.Tag, cachingHashSet);
      }
      cachingHashSet.Add(listener);
    }

    public void UnregisterBroadcastListener(BroadcastListener listener) => this.m_activeBroadcastListeners[listener.Tag].Remove(listener);

    public void RegisterContextWithPendingCallbacks(MyIntergridCommunicationContext context) => this.m_contextsWithPendingCallbacks.Add(context);

    public void UnregisterContextWithPendingCallbacks(MyIntergridCommunicationContext context) => this.m_contextsWithPendingCallbacks.Remove(context);

    public void EnqueueMessage(MyIGCSystemSessionComponent.Message message) => this.m_messagesForNextTick.Add(message);

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      this.m_messagesForNextTick.Swap();
      foreach (MyIGCSystemSessionComponent.Message background in this.m_messagesForNextTick.BackgroundList)
      {
        if (background.Source.IsActive)
        {
          MyIGCMessage message = new MyIGCMessage(background.Data, background.Tag, background.Source.GetAddressOfThisContext());
          if (background.IsUnicast)
          {
            MyIntergridCommunicationContext unicastDestination = background.UnicastDestination;
            if (unicastDestination.IsActive)
              unicastDestination.UnicastListener.EnqueueMessage(message);
          }
          else
          {
            CachingHashSet<BroadcastListener> cachingHashSet;
            if (this.m_activeBroadcastListeners.TryGetValue(background.Tag, out cachingHashSet))
            {
              cachingHashSet.ApplyChanges();
              if (cachingHashSet.Count > 0)
              {
                foreach (BroadcastListener broadcastListener in cachingHashSet)
                {
                  if (broadcastListener.Context != background.Source && background.Source.IsConnectedTo(broadcastListener.Context, background.TransmissionDistance))
                    broadcastListener.EnqueueMessage(message);
                }
              }
            }
          }
        }
      }
      this.m_messagesForNextTick.BackgroundList.Clear();
      this.m_contextsWithPendingCallbacks.ApplyChanges();
      foreach (MyIntergridCommunicationContext withPendingCallback in this.m_contextsWithPendingCallbacks)
        withPendingCallback.InvokeSinglePendingCallback();
      if (!MyDebugDrawSettings.DEBUG_DRAW_IGC)
        return;
      foreach (MyIntergridCommunicationContext withPendingCallback in this.m_contextsWithPendingCallbacks)
        MyRenderProxy.DebugDrawSphere(withPendingCallback.ProgrammableBlock.WorldMatrix.Translation, 2f, Color.Orange);
      foreach (CachingHashSet<BroadcastListener> cachingHashSet in this.m_activeBroadcastListeners.Values)
      {
        cachingHashSet.ApplyChanges();
        foreach (BroadcastListener broadcastListener in cachingHashSet)
          MyRenderProxy.DebugDrawText3D(broadcastListener.Context.ProgrammableBlock.WorldMatrix.Translation, broadcastListener.Tag, Color.Blue, 0.7f, false);
      }
      if (this.m_debugDrawQueue == null)
        return;
      foreach (MyTuple<int, Action> debugDraw in this.m_debugDrawQueue)
        debugDraw.Item2();
      while (this.m_debugDrawQueue.Count > 0 && this.m_debugDrawQueue.Peek().Item1 <= MySession.Static.GameplayFrameCounter)
        this.m_debugDrawQueue.Dequeue();
    }

    public void AddDebugDraw(Action action)
    {
      if (this.m_debugDrawQueue == null)
        this.m_debugDrawQueue = new Queue<MyTuple<int, Action>>();
      this.m_debugDrawQueue.Enqueue(MyTuple.Create<int, Action>(MySession.Static.GameplayFrameCounter + 30, action));
    }

    public override MyObjectBuilder_SessionComponent GetObjectBuilder()
    {
      MyObjectBuilder_MyIGCSystemSessionComponent objectBuilder = (MyObjectBuilder_MyIGCSystemSessionComponent) base.GetObjectBuilder();
      objectBuilder.ActiveProgrammableBlocks = new List<long>(this.m_perPBCommContexts.Count);
      foreach (long key in this.m_perPBCommContexts.Keys)
        objectBuilder.ActiveProgrammableBlocks.Add(key);
      return (MyObjectBuilder_SessionComponent) objectBuilder;
    }

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      base.Init(sessionComponent);
      this.m_idsToInitialize = ((MyObjectBuilder_MyIGCSystemSessionComponent) sessionComponent).ActiveProgrammableBlocks;
    }

    public override void BeforeStart()
    {
      base.BeforeStart();
      if (!Sync.IsServer || this.m_idsToInitialize == null)
        return;
      foreach (long entityId in this.m_idsToInitialize)
      {
        MyProgrammableBlock entityById = (MyProgrammableBlock) MyEntities.GetEntityById(entityId);
        if (entityById != null)
          this.GetOrMakeContextFor(entityById);
      }
    }

    public override void LoadData()
    {
      base.LoadData();
      MyIGCSystemSessionComponent.m_static = this;
      this.BroadcasterProvider = new Action<MyCubeGrid, HashSet<MyDataBroadcaster>, long>(MyAntennaSystem.GetCubeGridGroupBroadcasters);
      this.ConnectionProvider = (Func<MyProgrammableBlock, MyDataBroadcaster, long, bool>) ((target, source, rightsCheckedIdentity) => MyAntennaSystem.Static.CheckConnection((MyEntity) target, source, rightsCheckedIdentity, false));
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      foreach (MyIntergridCommunicationContext communicationContext in this.m_perPBCommContexts.Values)
        communicationContext.DisposeContext();
      this.m_debugDrawQueue = (Queue<MyTuple<int, Action>>) null;
      this.m_perPBCommContexts = (Dictionary<long, MyIntergridCommunicationContext>) null;
      this.ConnectionProvider = (Func<MyProgrammableBlock, MyDataBroadcaster, long, bool>) null;
      this.BroadcasterProvider = (Action<MyCubeGrid, HashSet<MyDataBroadcaster>, long>) null;
      this.m_contextsWithPendingCallbacks = (CachingHashSet<MyIntergridCommunicationContext>) null;
      MyIGCSystemSessionComponent.m_static = (MyIGCSystemSessionComponent) null;
    }

    public override bool IsRequiredByGame => true;

    public override Type[] Dependencies => new Type[1]
    {
      typeof (MyAntennaSystem)
    };

    public static object BoxMessage<TMessage>(TMessage message)
    {
      if (!MyIGCSystemSessionComponent.MessageTypeChecker<TMessage>.IsAllowed)
        throw new Exception("Message type " + (object) typeof (TMessage) + " is not allowed!");
      return (object) message;
    }

    public struct Message
    {
      public readonly string Tag;
      public readonly object Data;
      public readonly TransmissionDistance TransmissionDistance;
      public readonly MyIntergridCommunicationContext Source;
      public readonly MyIntergridCommunicationContext UnicastDestination;

      public bool IsUnicast => this.UnicastDestination != null;

      private Message(
        object data,
        string tag,
        MyIntergridCommunicationContext source,
        MyIntergridCommunicationContext unicastDestination,
        TransmissionDistance transmissionDistance)
      {
        this.Tag = tag;
        this.Data = data;
        this.Source = source;
        this.UnicastDestination = unicastDestination;
        this.TransmissionDistance = transmissionDistance;
      }

      public static MyIGCSystemSessionComponent.Message FromBroadcast(
        object data,
        string broadcastTag,
        TransmissionDistance transmissionDistance,
        MyIntergridCommunicationContext source)
      {
        if (broadcastTag == null)
          throw new ArgumentNullException(nameof (broadcastTag), "Broadcast tag can't be null");
        return new MyIGCSystemSessionComponent.Message(data, broadcastTag, source, (MyIntergridCommunicationContext) null, transmissionDistance);
      }

      public static MyIGCSystemSessionComponent.Message FromUnicast(
        object data,
        string unicastTag,
        MyIntergridCommunicationContext source,
        MyIntergridCommunicationContext unicastDestination)
      {
        return new MyIGCSystemSessionComponent.Message(data, unicastTag, source, unicastDestination, TransmissionDistance.AntennaRelay);
      }
    }

    private static class MessageTypeChecker<TMessageType>
    {
      public static readonly bool IsAllowed = MyIGCSystemSessionComponent.MessageTypeChecker<TMessageType>.IsTypeAllowed(typeof (TMessageType), 25);

      private static bool IsTypeAllowed(Type type, int recursion)
      {
        if (recursion <= 0)
          return false;
        if (MyIGCSystemSessionComponent.MessageTypeChecker<TMessageType>.IsPrimitiveOfSafeStruct(type))
          return true;
        if (!type.IsGenericType)
          return false;
        Type[] genericArguments = type.GetGenericArguments();
        Type genericTypeDefinition = type.GetGenericTypeDefinition();
        if (!MyIGCSystemSessionComponent.MessageTypeChecker<TMessageType>.IsMyTuple(genericTypeDefinition, genericArguments.Length) && !MyIGCSystemSessionComponent.MessageTypeChecker<TMessageType>.IsImmutableCollection(genericTypeDefinition))
          return false;
        foreach (Type type1 in genericArguments)
        {
          if (!MyIGCSystemSessionComponent.MessageTypeChecker<TMessageType>.IsTypeAllowed(type1, recursion - 1))
            return false;
        }
        return true;
      }

      private static bool IsMyTuple(Type type, int genericArgs)
      {
        switch (genericArgs)
        {
          case 1:
            return type == typeof (MyTuple<>);
          case 2:
            return type == typeof (MyTuple<,>);
          case 3:
            return type == typeof (MyTuple<,,>);
          case 4:
            return type == typeof (MyTuple<,,,>);
          case 5:
            return type == typeof (MyTuple<,,,,>);
          case 6:
            return type == typeof (MyTuple<,,,,,>);
          default:
            return false;
        }
      }

      private static bool IsImmutableCollection(Type type) => type == typeof (ImmutableArray<>) || type == typeof (ImmutableList<>) || (type == typeof (ImmutableQueue<>) || type == typeof (ImmutableStack<>)) || (type == typeof (ImmutableHashSet<>) || type == typeof (ImmutableSortedSet<>) || type == typeof (ImmutableDictionary<,>)) || type == typeof (ImmutableSortedDictionary<,>);

      private static bool IsPrimitiveOfSafeStruct(Type type) => type.IsPrimitive || type == typeof (string) || (type == typeof (Ray) || type == typeof (RayD)) || (type == typeof (Line) || type == typeof (LineD) || (type == typeof (Color) || type == typeof (Plane))) || (type == typeof (VRageMath.Point) || type == typeof (PlaneD) || (type == typeof (MyQuad) || type == typeof (Matrix)) || (type == typeof (MatrixD) || type == typeof (MatrixI) || (type == typeof (MyQuadD) || type == typeof (Capsule)))) || (type == typeof (Vector2) || type == typeof (Vector3) || (type == typeof (Vector4) || type == typeof (CapsuleD)) || (type == typeof (Vector2D) || type == typeof (Vector2B) || (type == typeof (Vector3L) || type == typeof (Vector4D))) || (type == typeof (Vector3D) || type == typeof (MyShort4) || (type == typeof (MyBounds) || type == typeof (Vector3B)) || (type == typeof (Vector3S) || type == typeof (Vector2I) || (type == typeof (Vector4I) || type == typeof (CubeFace))))) || (type == typeof (Vector3I) || type == typeof (Matrix3x3) || (type == typeof (MyUShort4) || type == typeof (Rectangle)) || (type == typeof (Quaternion) || type == typeof (RectangleF) || (type == typeof (BoundingBox) || type == typeof (QuaternionD))) || (type == typeof (MyTransform) || type == typeof (BoundingBox2) || (type == typeof (BoundingBoxI) || type == typeof (BoundingBoxD)) || (type == typeof (MyTransformD) || type == typeof (Vector3UByte) || (type == typeof (CurveTangent) || type == typeof (Vector4UByte)))) || (type == typeof (BoundingBox2I) || type == typeof (BoundingBox2D) || (type == typeof (Vector3Ushort) || type == typeof (CurveLoopType)) || (type == typeof (BoundingSphere) || type == typeof (BoundingSphereD) || (type == typeof (ContainmentType) || type == typeof (CurveContinuity))) || (type == typeof (MyBlockOrientation) || type == typeof (Base6Directions.Axis) || (type == typeof (MyOrientedBoundingBox) || type == typeof (PlaneIntersectionType)) || (type == typeof (MyOrientedBoundingBoxD) || type == typeof (Vector3I_RangeIterator) || (type == typeof (Base6Directions.Direction) || type == typeof (Base27Directions.Direction)))))) || (type == typeof (CompressedPositionOrientation) || type == typeof (Base6Directions.DirectionFlags) || (type == typeof (HalfVector3) || type == typeof (HalfVector2))) || type == typeof (HalfVector4);
    }
  }
}
