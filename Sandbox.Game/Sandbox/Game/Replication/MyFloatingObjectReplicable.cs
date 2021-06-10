// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.MyFloatingObjectReplicable
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Replication.History;
using Sandbox.Game.Replication.StateGroups;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Network;

namespace Sandbox.Game.Replication
{
  public class MyFloatingObjectReplicable : MyEntityReplicableBaseEvent<MyFloatingObject>
  {
    private static readonly MyPredictedSnapshotSyncSetup m_settings;
    private MyPropertySyncStateGroup m_propertySync;

    protected override IMyStateGroup CreatePhysicsGroup() => (IMyStateGroup) new MySimplePhysicsStateGroup((MyEntity) this.Instance, (IMyReplicable) this, MyFloatingObjectReplicable.m_settings);

    protected override void OnHook()
    {
      base.OnHook();
      this.m_propertySync = new MyPropertySyncStateGroup((IMyReplicable) this, this.Instance.SyncType);
    }

    public override void GetStateGroups(List<IMyStateGroup> resultList)
    {
      base.GetStateGroups(resultList);
      if (this.m_propertySync == null || this.m_propertySync.PropertyCount <= 0)
        return;
      resultList.Add((IMyStateGroup) this.m_propertySync);
    }

    static MyFloatingObjectReplicable()
    {
      MyPredictedSnapshotSyncSetup snapshotSyncSetup = new MyPredictedSnapshotSyncSetup();
      snapshotSyncSetup.ProfileName = "FloatingObject";
      snapshotSyncSetup.ApplyPosition = true;
      snapshotSyncSetup.ApplyRotation = true;
      snapshotSyncSetup.ApplyPhysicsAngular = false;
      snapshotSyncSetup.ApplyPhysicsLinear = true;
      snapshotSyncSetup.ExtrapolationSmoothing = true;
      snapshotSyncSetup.MaxPositionFactor = 100f;
      snapshotSyncSetup.MaxLinearFactor = 100f;
      snapshotSyncSetup.MaxRotationFactor = 100f;
      snapshotSyncSetup.MaxAngularFactor = 1f;
      snapshotSyncSetup.IterationsFactor = 0.3f;
      MyFloatingObjectReplicable.m_settings = snapshotSyncSetup;
    }
  }
}
