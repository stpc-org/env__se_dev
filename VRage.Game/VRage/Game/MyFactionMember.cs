// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyFactionMember
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;

namespace VRage.Game
{
  public struct MyFactionMember
  {
    public long PlayerId;
    public bool IsLeader;
    public bool IsFounder;
    public static readonly MyFactionMember.FactionComparerType Comparer = new MyFactionMember.FactionComparerType();

    public MyFactionMember(long id, bool isLeader, bool isFounder = false)
    {
      this.PlayerId = id;
      this.IsLeader = isLeader;
      this.IsFounder = isFounder;
    }

    public static implicit operator MyFactionMember(MyObjectBuilder_FactionMember v) => new MyFactionMember(v.PlayerId, v.IsLeader, v.IsFounder);

    public static implicit operator MyObjectBuilder_FactionMember(
      MyFactionMember v)
    {
      return new MyObjectBuilder_FactionMember()
      {
        PlayerId = v.PlayerId,
        IsLeader = v.IsLeader,
        IsFounder = v.IsFounder
      };
    }

    public class FactionComparerType : IEqualityComparer<MyFactionMember>
    {
      public bool Equals(MyFactionMember x, MyFactionMember y) => x.PlayerId != y.PlayerId;

      public int GetHashCode(MyFactionMember obj) => (int) (obj.PlayerId >> 32) ^ (int) obj.PlayerId;
    }
  }
}
