// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.History.MySnapshotSyncSetup
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game.Networking;

namespace Sandbox.Game.Replication.History
{
  public class MySnapshotSyncSetup : MySnapshotFlags
  {
    public string ProfileName;
    public bool ExtrapolationSmoothing;
    public bool IgnoreParentId;
    public bool UserTrend;
  }
}
