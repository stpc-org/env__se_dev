// Decompiled with JetBrains decompiler
// Type: VRage.Network.MyStateDataEntry
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRage.Library;

namespace VRage.Network
{
  public class MyStateDataEntry : FastPriorityQueue<MyStateDataEntry>.Node
  {
    public readonly NetworkId GroupId;
    public readonly IMyStateGroup Group;
    public readonly IMyReplicable Owner;

    public MyStateDataEntry(IMyReplicable owner, NetworkId groupId, IMyStateGroup group)
    {
      this.Owner = owner;
      this.Priority = 0L;
      this.GroupId = groupId;
      this.Group = group;
    }

    public override string ToString() => string.Format("{0:0.00}, {1}, {2}", (object) this.Priority, (object) this.GroupId, (object) this.Group);
  }
}
