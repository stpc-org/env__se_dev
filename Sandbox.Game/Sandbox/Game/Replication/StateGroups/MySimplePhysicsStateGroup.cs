// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.StateGroups.MySimplePhysicsStateGroup
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Replication.History;
using VRage.Game.Entity;
using VRage.Library.Utils;
using VRage.Network;

namespace Sandbox.Game.Replication.StateGroups
{
  public class MySimplePhysicsStateGroup : MyEntityPhysicsStateGroupBase
  {
    private readonly MyPredictedSnapshotSyncSetup m_settings;

    public MySimplePhysicsStateGroup(
      MyEntity entity,
      IMyReplicable owner,
      MyPredictedSnapshotSyncSetup settings)
      : base(entity, owner)
    {
      this.m_settings = settings;
    }

    public override void ClientUpdate(MyTimeSpan clientTimestamp) => this.m_snapshotSync.Update(clientTimestamp, (MySnapshotSyncSetup) this.m_settings);
  }
}
