// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.Ingame.Utilities.StringSegmentComparer
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using VRage.Utils;

namespace VRage.Game.ModAPI.Ingame.Utilities
{
  public class StringSegmentComparer : IEqualityComparer<StringSegment>
  {
    public static readonly StringSegmentComparer DEFAULT = new StringSegmentComparer();

    public bool Equals(StringSegment x, StringSegment y)
    {
      if (x.Length != y.Length)
        return false;
      string text1 = x.Text;
      int start1 = x.Start;
      string text2 = y.Text;
      int start2 = y.Start;
      for (int index = 0; index < x.Length; ++index)
      {
        if ((int) text1[start1] != (int) text2[start2])
          return false;
        ++start1;
        ++start2;
      }
      return true;
    }

    public int GetHashCode(StringSegment obj) => MyUtils.GetHash(obj.Text, obj.Start, obj.Length);
  }
}
