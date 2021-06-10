// Decompiled with JetBrains decompiler
// Type: VRage.Stats.MyPriorityComparer
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections.Generic;

namespace VRage.Stats
{
  internal class MyPriorityComparer : Comparer<KeyValuePair<string, MyStat>>
  {
    public override int Compare(KeyValuePair<string, MyStat> x, KeyValuePair<string, MyStat> y) => -x.Value.Priority.CompareTo(y.Value.Priority);
  }
}
