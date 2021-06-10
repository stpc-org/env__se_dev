// Decompiled with JetBrains decompiler
// Type: VRageMath.MyLineSegmentOverlapResult`1
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System.Collections.Generic;

namespace VRageMath
{
  public struct MyLineSegmentOverlapResult<T>
  {
    public static MyLineSegmentOverlapResult<T>.MyLineSegmentOverlapResultComparer DistanceComparer = new MyLineSegmentOverlapResult<T>.MyLineSegmentOverlapResultComparer();
    public double Distance;
    public T Element;

    public class MyLineSegmentOverlapResultComparer : IComparer<MyLineSegmentOverlapResult<T>>
    {
      public int Compare(MyLineSegmentOverlapResult<T> x, MyLineSegmentOverlapResult<T> y) => x.Distance.CompareTo(y.Distance);
    }
  }
}
