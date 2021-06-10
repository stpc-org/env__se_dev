// Decompiled with JetBrains decompiler
// Type: VRage.Profiler.MyProfilerBlockKeyComparer
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections.Generic;

namespace VRage.Profiler
{
  public class MyProfilerBlockKeyComparer : IEqualityComparer<MyProfilerBlockKey>
  {
    public bool Equals(MyProfilerBlockKey x, MyProfilerBlockKey y) => x.ParentId == y.ParentId && x.Name == y.Name && (x.Member == y.Member && x.File == y.File) && x.Line == y.Line;

    public int GetHashCode(MyProfilerBlockKey obj) => obj.HashCode;
  }
}
