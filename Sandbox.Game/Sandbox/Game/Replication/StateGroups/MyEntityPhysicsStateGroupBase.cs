// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.StateGroups.MyEntityPhysicsStateGroupBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication.History;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Replication.StateGroups
{
  public abstract class MyEntityPhysicsStateGroupBase : IMyStateGroup, IMyNetObject, IMyEventOwner
  {
    protected IMySnapshotSync m_snapshotSync;
    protected bool m_forcedWorldSnapshots;
    private bool m_physicsActive;
    private const float MIN_SIZE = 10f;
    private const float MIN_ACCELERATION = 5f;
    private readonly List<MyEntity> m_tmpEntityResults = new List<MyEntity>();
    protected bool m_supportInited;
    protected long m_lastSupportId;

    public IMyReplicable Owner { get; private set; }

    public MyEntity Entity { get; private set; }

    public bool IsHighPriority
    {
      get
      {
        if (this.m_forcedWorldSnapshots)
          return true;
        return (double) this.Entity.PositionComp.LocalAABB.Size.LengthSquared() > 100.0 && this.Entity.Physics != null && (double) this.Entity.Physics.LinearAcceleration.LengthSquared() > 25.0;
      }
    }

    public bool IsStreaming => false;

    public bool NeedsUpdate => true;

    protected bool IsControlled => Sync.Players.GetControllingPlayer(this.Entity) != null;

    protected bool IsControlledLocally => MySession.Static.TopMostControlledEntity == this.Entity;

    public bool IsValid => !this.Entity.MarkedForClose;

    public MyEntityPhysicsStateGroupBase(
      MyEntity entity,
      IMyReplicable ownerReplicable,
      bool createSync = true)
    {
      this.Entity = entity;
      this.Owner = ownerReplicable;
      if (!Sync.IsServer & createSync)
        this.m_snapshotSync = (IMySnapshotSync) new MyAnimatedSnapshotSync(this.Entity);
      if (!Sync.IsServer)
        return;
      this.Entity.AddedToScene += new Action<MyEntity>(this.RegisterPhysics);
    }

    private void OnPhysicsComponentChanged(
      MyPhysicsComponentBase oldComponent,
      MyPhysicsComponentBase newComponent)
    {
      if (oldComponent != null)
        oldComponent.OnBodyActiveStateChanged -= new Action<MyPhysicsComponentBase, bool>(this.ActiveStateChanged);
      if (newComponent == null)
        return;
      this.m_physicsActive = newComponent.IsActive;
      newComponent.OnBodyActiveStateChanged += new Action<MyPhysicsComponentBase, bool>(this.ActiveStateChanged);
    }

    private void RegisterPhysics(MyEntity obj)
    {
      this.Entity.AddedToScene -= new Action<MyEntity>(this.RegisterPhysics);
      if (this.Entity.Physics == null)
        return;
      this.m_physicsActive = this.Entity.Physics.IsActive;
      this.Entity.Physics.OnBodyActiveStateChanged += new Action<MyPhysicsComponentBase, bool>(this.ActiveStateChanged);
      this.Entity.OnPhysicsComponentChanged += new Action<MyPhysicsComponentBase, MyPhysicsComponentBase>(this.OnPhysicsComponentChanged);
    }

    private void ActiveStateChanged(MyPhysicsComponentBase physics, bool active)
    {
      this.m_physicsActive = active;
      if (!active)
        return;
      MyMultiplayer.GetReplicationServer().AddToDirtyGroups((IMyStateGroup) this);
    }

    public void CreateClientData(MyClientStateBase forClient)
    {
    }

    public void DestroyClientData(MyClientStateBase forClient)
    {
    }

    public abstract void ClientUpdate(MyTimeSpan clientTimestamp);

    public virtual void Destroy()
    {
      if (this.Entity.Physics != null)
        this.Entity.Physics.OnBodyActiveStateChanged -= new Action<MyPhysicsComponentBase, bool>(this.ActiveStateChanged);
      if (Sync.IsServer)
        return;
      this.m_snapshotSync.Destroy();
    }

    public void OnAck(MyClientStateBase forClient, byte packetId, bool delivered)
    {
    }

    public void ForceSend(MyClientStateBase clientData)
    {
    }

    public void Reset(bool reinit, MyTimeSpan clientTimestamp)
    {
      this.m_snapshotSync.Reset(reinit);
      if (!reinit)
        return;
      this.ClientUpdate(clientTimestamp);
      this.m_snapshotSync.Reset(reinit);
    }

    public virtual void Serialize(
      BitStream stream,
      MyClientInfo forClient,
      MyTimeSpan serverTimestamp,
      MyTimeSpan lastClientTimestamp,
      byte packetId,
      int maxBitPosition,
      HashSet<string> cachedData)
    {
      if (stream.Writing)
      {
        new MySnapshot(this.Entity, forClient).Write(stream);
        bool isControlled = this.IsControlled;
        stream.WriteBool(isControlled);
        if (!isControlled)
          return;
        this.Entity.SerializeControls(stream);
      }
      else
      {
        MySnapshot mySnapshot = new MySnapshot(stream);
        this.m_snapshotSync.Read(ref mySnapshot, serverTimestamp);
        if (!stream.ReadBool())
          return;
        this.Entity.DeserializeControls(stream, false);
      }
    }

    public virtual bool IsStillDirty(Endpoint forClient) => this.m_physicsActive;

    public MyStreamProcessingState IsProcessingForClient(Endpoint forClient) => MyStreamProcessingState.None;

    protected long UpdateParenting(
      MyEntityPhysicsStateGroupBase.ParentingSetup parentingSetup,
      long currentParentId)
    {
      if (this.Entity.Closed)
      {
        MyLog.Default.Error("Entity {0} in physics state group is closed.", (object) this.Entity);
        return 0;
      }
      List<MyEntity> tmpEntityResults = this.m_tmpEntityResults;
      MyCubeGrid myCubeGrid1 = (MyCubeGrid) null;
      BoundingBoxD boundingBox = this.Entity.PositionComp.WorldAABB.Inflate((double) parentingSetup.MaxParentDisconnectDistance);
      MyEntities.GetTopMostEntitiesInBox(ref boundingBox, tmpEntityResults, MyEntityQueryType.Dynamic);
      bool flag1 = false;
      float num1 = float.MaxValue;
      float num2 = float.MaxValue;
      float num3 = this.Entity.PositionComp.LocalAABB.Size.LengthSquared();
      Vector3 vector3 = this.Entity.Physics.LinearVelocity;
      float num4 = vector3.LengthSquared();
      foreach (MyEntity myEntity in tmpEntityResults)
      {
        if (myEntity.EntityId != this.Entity.EntityId && myEntity is MyCubeGrid myCubeGrid2 && (myCubeGrid2.Physics != null && myCubeGrid2.BlocksCount > 1))
        {
          vector3 = myCubeGrid2.PositionComp.LocalAABB.Size;
          float num5 = vector3.LengthSquared();
          if ((double) num5 > (double) num3)
          {
            bool flag2 = currentParentId == myCubeGrid2.EntityId;
            if (myCubeGrid2.PositionComp.WorldAABB.Contains(this.Entity.PositionComp.WorldAABB) == ContainmentType.Contains)
            {
              if (!flag1)
              {
                myCubeGrid1 = (MyCubeGrid) null;
                flag1 = true;
              }
              float num6 = flag2 ? parentingSetup.MinDisconnectInsideParentSpeed : parentingSetup.MinInsideParentSpeed;
              if ((double) myCubeGrid2.Physics.GetVelocityAtPoint(this.Entity.PositionComp.GetPosition()).LengthSquared() >= (double) num6 * (double) num6 && (double) num5 < (double) num1)
              {
                num1 = num5;
                myCubeGrid1 = myCubeGrid2;
              }
            }
            else if (!flag1)
            {
              float num6 = flag2 ? parentingSetup.MinDisconnectParentSpeed : parentingSetup.MinParentSpeed;
              float num7 = num6 / 2f;
              if ((double) num4 >= (double) num7 * (double) num7)
              {
                float num8 = flag2 ? parentingSetup.MaxParentDisconnectDistance : parentingSetup.MaxParentDistance;
                float num9 = flag2 ? parentingSetup.MaxDisconnectParentAcceleration : parentingSetup.MaxParentAcceleration;
                float num10 = (float) myCubeGrid2.PositionComp.WorldAABB.DistanceSquared(this.Entity.PositionComp.GetPosition());
                if ((double) num10 <= (double) num8 * (double) num8 && (double) myCubeGrid2.Physics.GetVelocityAtPoint(this.Entity.PositionComp.GetPosition()).LengthSquared() >= (double) num6 * (double) num6)
                {
                  vector3 = myCubeGrid2.Physics.LinearAcceleration;
                  if ((double) vector3.LengthSquared() <= (double) num9 * (double) num9 && (double) num10 < (double) num2)
                  {
                    num2 = num10;
                    myCubeGrid1 = myCubeGrid2;
                  }
                }
              }
            }
          }
        }
      }
      tmpEntityResults.Clear();
      long num11 = myCubeGrid1 != null ? myCubeGrid1.EntityId : 0L;
      if (!this.m_supportInited || this.m_lastSupportId == num11 || num11 == 0L)
        currentParentId = num11;
      this.m_lastSupportId = num11;
      this.m_supportInited = true;
      return currentParentId;
    }

    public class ParentingSetup
    {
      public float MaxParentDistance;
      public float MinParentSpeed;
      public float MaxParentAcceleration;
      public float MinInsideParentSpeed;
      public float MaxParentDisconnectDistance;
      public float MinDisconnectParentSpeed;
      public float MaxDisconnectParentAcceleration;
      public float MinDisconnectInsideParentSpeed;
    }
  }
}
