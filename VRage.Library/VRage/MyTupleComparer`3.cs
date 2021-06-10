// Decompiled with JetBrains decompiler
// Type: VRage.MyTupleComparer`3
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;

namespace VRage
{
  public class MyTupleComparer<T1, T2, T3> : IEqualityComparer<MyTuple<T1, T2, T3>>
    where T1 : IEquatable<T1>
    where T2 : IEquatable<T2>
    where T3 : IEquatable<T3>
  {
    public bool Equals(MyTuple<T1, T2, T3> x, MyTuple<T1, T2, T3> y) => x.Item1.Equals(y.Item1) && x.Item2.Equals(y.Item2) && x.Item3.Equals(y.Item3);

    public int GetHashCode(MyTuple<T1, T2, T3> obj) => obj.Item1.GetHashCode() * 1610612741 + obj.Item2.GetHashCode() * 1610612741 + obj.Item3.GetHashCode();
  }
}
