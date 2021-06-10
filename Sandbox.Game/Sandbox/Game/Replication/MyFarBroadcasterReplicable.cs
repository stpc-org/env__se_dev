// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.MyFarBroadcasterReplicable
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Replication.StateGroups;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Library.Collections;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRageMath;

namespace Sandbox.Game.Replication
{
  internal class MyFarBroadcasterReplicable : MyExternalReplicableEvent<MyDataBroadcaster>
  {
    private MyEntityPositionStateGroup m_positionStateGroup;
    private MyProxyAntenna m_proxyAntenna;

    public override bool IsValid => this.Instance != null && this.Instance.Entity != null && !this.Instance.Entity.MarkedForClose;

    public override bool PriorityUpdate => false;

    protected override void OnHook()
    {
      base.OnHook();
      this.m_positionStateGroup = new MyEntityPositionStateGroup((IMyReplicable) this, this.Instance.Entity);
      this.Instance.BeforeRemovedFromContainer += (Action<MyEntityComponentBase>) (component => this.OnRemovedFromContainer());
    }

    private void OnRemovedFromContainer() => this.RaiseDestroyed();

    public override IMyReplicable GetParent() => (IMyReplicable) null;

    protected override void OnLoad(BitStream stream, Action<MyDataBroadcaster> loadingDoneHandler)
    {
      MyObjectBuilder_ProxyAntenna builderProxyAntenna;
      MySerializer.CreateAndRead<MyObjectBuilder_ProxyAntenna>(stream, out builderProxyAntenna, MyObjectBuilderSerializer.Dynamic);
      this.m_proxyAntenna = MyEntities.CreateFromObjectBuilderAndAdd((MyObjectBuilder_EntityBase) builderProxyAntenna, false) as MyProxyAntenna;
      loadingDoneHandler(this.m_proxyAntenna.Broadcaster);
    }

    public override bool HasToBeChild => true;

    public override bool OnSave(BitStream stream, Endpoint clientEndpoint)
    {
      MyObjectBuilder_ProxyAntenna newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ProxyAntenna>();
      this.Instance.InitProxyObjectBuilder(newObject);
      MySerializer.Write<MyObjectBuilder_ProxyAntenna>(stream, ref newObject, MyObjectBuilderSerializer.Dynamic);
      return true;
    }

    public override void OnDestroyClient()
    {
      if (this.m_proxyAntenna != null)
        this.m_proxyAntenna.Close();
      this.m_proxyAntenna = (MyProxyAntenna) null;
    }

    public override void GetStateGroups(List<IMyStateGroup> resultList) => resultList.Add((IMyStateGroup) this.m_positionStateGroup);

    public override BoundingBoxD GetAABB()
    {
      MyDataBroadcaster instance = this.Instance;
      return this.Instance != null && this.Instance.Entity != null ? this.Instance.Entity.WorldAABB : BoundingBoxD.CreateInvalid();
    }
  }
}
