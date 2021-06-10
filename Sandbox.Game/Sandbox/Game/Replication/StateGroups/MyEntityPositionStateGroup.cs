// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.StateGroups.MyEntityPositionStateGroup
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Replication.StateGroups
{
  internal class MyEntityPositionStateGroup : IMyStateGroup, IMyNetObject, IMyEventOwner
  {
    private Vector3D m_position;
    private readonly IMyEntity m_entity;

    public bool IsStreaming => false;

    public bool NeedsUpdate => true;

    public bool IsHighPriority => false;

    public IMyReplicable Owner { get; private set; }

    public bool IsValid => !this.m_entity.MarkedForClose;

    public MyEntityPositionStateGroup(IMyReplicable ownerReplicable, IMyEntity entity)
    {
      this.Owner = ownerReplicable;
      this.m_entity = entity;
    }

    public void CreateClientData(MyClientStateBase forClient)
    {
    }

    public void DestroyClientData(MyClientStateBase forClient)
    {
    }

    public void ClientUpdate(MyTimeSpan clientTimestamp)
    {
      if (this.m_entity.PositionComp.GetPosition().Equals(this.m_position, 1.0))
        return;
      this.m_entity.SetWorldMatrix(MatrixD.CreateTranslation(this.m_position));
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
        stream.Write(this.m_entity.PositionComp.GetPosition());
      else
        this.m_position = stream.ReadVector3D();
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

    public bool IsStillDirty(Endpoint forClient) => true;

    public MyStreamProcessingState IsProcessingForClient(Endpoint forClient) => MyStreamProcessingState.None;
  }
}
