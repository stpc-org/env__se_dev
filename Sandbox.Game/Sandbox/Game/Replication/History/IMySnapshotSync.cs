// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.History.IMySnapshotSync
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Library.Utils;

namespace Sandbox.Game.Replication.History
{
  public interface IMySnapshotSync
  {
    void Destroy();

    long Update(MyTimeSpan clientTimestamp, MySnapshotSyncSetup setup);

    void Read(ref MySnapshot item, MyTimeSpan timeStamp);

    void Reset(bool reinit = false);
  }
}
