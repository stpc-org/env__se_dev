// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.MyWaypointReplicable
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Replication.StateGroups;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Replication
{
  internal class MyWaypointReplicable : MyEntityReplicableBaseEvent<MyWaypoint>
  {
    protected override void OnDestroyClientInternal()
    {
      if (this.Instance == null || !this.Instance.Save)
        return;
      this.Instance.Close();
    }

    protected override IMyStateGroup CreatePhysicsGroup() => (IMyStateGroup) new MyEntityTransformStateGroup((IMyReplicable) this, (IMyEntity) this.Instance);

    public override BoundingBoxD GetAABB() => new BoundingBoxD(new Vector3D(-999999.0), new Vector3D(999999.0));
  }
}
