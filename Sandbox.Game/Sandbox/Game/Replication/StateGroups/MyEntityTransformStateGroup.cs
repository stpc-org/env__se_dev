// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.StateGroups.MyEntityTransformStateGroup
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Replication.StateGroups
{
  internal class MyEntityTransformStateGroup : IMyStateGroup, IMyNetObject, IMyEventOwner
  {
    private MatrixD m_transform = MatrixD.Identity;
    private readonly IMyEntity m_entity;

    public bool IsStreaming => false;

    public bool NeedsUpdate => true;

    public bool IsHighPriority => false;

    public IMyReplicable Owner { get; private set; }

    public bool IsValid => !this.m_entity.MarkedForClose;

    public MyEntityTransformStateGroup(IMyReplicable ownerReplicable, IMyEntity entity)
    {
      this.Owner = ownerReplicable;
      this.m_entity = entity;
      this.m_transform = this.m_entity.WorldMatrix;
    }

    public void CreateClientData(MyClientStateBase forClient)
    {
    }

    public void DestroyClientData(MyClientStateBase forClient)
    {
    }

    public void ClientUpdate(MyTimeSpan clientTimestamp)
    {
      if (this.m_entity.PositionComp.WorldMatrixRef.Equals(this.m_transform))
        return;
      this.m_entity.SetWorldMatrix(this.m_transform);
    }

    public void Destroy() => this.Owner = (IMyReplicable) null;

    public float GetGroupPriority(int frameCountWithoutSync, MyClientInfo forClient) => 1f;

    public void Serialize(
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
        stream.Write(this.m_entity.PositionComp.GetPosition());
        Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(in this.m_entity.PositionComp.WorldMatrixRef);
        stream.WriteQuaternion(fromRotationMatrix);
      }
      else
      {
        Vector3D vector3D = stream.ReadVector3D();
        this.m_transform = MatrixD.CreateFromQuaternion(stream.ReadQuaternion());
        this.m_transform.Translation = vector3D;
      }
    }

    public void OnAck(MyClientStateBase forClient, byte packetId, bool delivered)
    {
    }

    public void ForceSend(MyClientStateBase clientData)
    {
    }

    public void Reset(bool reinit, MyTimeSpan clientTimestamp)
    {
    }

    public bool IsStillDirty(Endpoint forClient)
    {
      if (this.m_entity is MyWaypoint entity)
        return !entity.Freeze;
      return !(this.m_entity is MySafeZone entity) || !entity.IsStatic;
    }

    public MyStreamProcessingState IsProcessingForClient(Endpoint forClient) => MyStreamProcessingState.None;
  }
}
