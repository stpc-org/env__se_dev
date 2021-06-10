// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.Comparers.IntPtrComparer
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;

namespace VRage.Library.Collections.Comparers
{
  public sealed class IntPtrComparer : EqualityComparer<IntPtr>
  {
    public static readonly IntPtrComparer Instance = new IntPtrComparer();

    public override bool Equals(IntPtr x, IntPtr y) => x == y;

    public override int GetHashCode(IntPtr obj) => obj.GetHashCode();
  }
}
