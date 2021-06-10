// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.MyLogicalEnvironmentSectorReplicable
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication;
using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System;
using System.Collections.Generic;
using VRage.Library.Collections;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRageMath;

namespace Sandbox.Game.WorldEnvironment
{
  internal class MyLogicalEnvironmentSectorReplicable : MyExternalReplicableEvent<MyLogicalEnvironmentSectorBase>
  {
    private static readonly MySerializeInfo serialInfo = new MySerializeInfo(MyObjectFlags.DefaultZero | MyObjectFlags.Dynamic, MyPrimitiveFlags.None, (ushort) 0, new DynamicSerializerDelegate(MyObjectBuilderSerializer.SerializeDynamic), (MySerializeInfo) null, (MySerializeInfo) null);
    private long m_planetEntityId;
    private long m_packedSectorId;
    private MyObjectBuilder_EnvironmentSector m_ob;

    public override bool IncludeInIslands => false;

    public override bool IsValid => this.m_parent != null && this.m_parent.IsValid;

    public override IMyReplicable GetParent() => this.m_parent;

    public override bool HasToBeChild => false;

    public override bool IsSpatial => true;

    public override bool ShouldReplicate(MyClientInfo client)
    {
      MyClientState state = client.State as MyClientState;
      if (this.Instance.Owner.Entity == null)
        return false;
      long entityId = this.Instance.Owner.Entity.EntityId;
      HashSet<long> longSet;
      return state.KnownSectors.TryGetValue(entityId, out longSet) && longSet.Contains(this.Instance.Id);
    }

    public override bool OnSave(BitStream stream, Endpoint clientEndpoint)
    {
      stream.WriteInt64(this.Instance.Owner.Entity.EntityId);
      stream.WriteInt64(this.Instance.Id);
      MyObjectBuilder_EnvironmentSector objectBuilder = this.Instance.GetObjectBuilder();
      MySerializer.Write<MyObjectBuilder_EnvironmentSector>(stream, ref objectBuilder, MyLogicalEnvironmentSectorReplicable.serialInfo);
      return true;
    }

    public override void OnDestroyClient()
    {
      if (this.Instance == null)
        return;
      this.Instance.ServerOwned = false;
    }

    public override void GetStateGroups(List<IMyStateGroup> resultList)
    {
    }

    protected override void OnLoad(
      BitStream stream,
      Action<MyLogicalEnvironmentSectorBase> loadingDoneHandler)
    {
      if (stream != null)
      {
        this.m_planetEntityId = stream.ReadInt64();
        this.m_packedSectorId = stream.ReadInt64();
        this.m_ob = MySerializer.CreateAndRead<MyObjectBuilder_EnvironmentSector>(stream, MyLogicalEnvironmentSectorReplicable.serialInfo);
      }
      if (!(MyEntities.GetEntityById(this.m_planetEntityId) is MyPlanet entityById))
      {
        loadingDoneHandler((MyLogicalEnvironmentSectorBase) null);
      }
      else
      {
        MyLogicalEnvironmentSectorBase logicalSector = entityById.Components.Get<MyPlanetEnvironmentComponent>().GetLogicalSector(this.m_packedSectorId);
        bool flag = MyExternalReplicable.FindByObject((object) entityById) != null;
        if (logicalSector != null & flag)
          logicalSector.Init(this.m_ob);
        loadingDoneHandler(logicalSector != null && logicalSector.ServerOwned || !flag ? (MyLogicalEnvironmentSectorBase) null : logicalSector);
      }
    }

    protected override void OnHook()
    {
      base.OnHook();
      if (Sync.IsServer)
        this.Instance.OnClose += new Action(this.Sector_OnClose);
      else
        this.Instance.ServerOwned = true;
      this.m_parent = (IMyReplicable) MyExternalReplicable.FindByObject((object) this.Instance.Owner.Entity);
    }

    private void Sector_OnClose()
    {
      this.Instance.OnClose -= new Action(this.Sector_OnClose);
      this.Instance.ServerOwned = false;
      this.RaiseDestroyed();
    }

    public override BoundingBoxD GetAABB()
    {
      BoundingBoxD boundingBoxD = BoundingBoxD.CreateInvalid();
      foreach (Vector3D bound in this.Instance.Bounds)
        boundingBoxD = boundingBoxD.Include(bound);
      return boundingBoxD;
    }
  }
}
