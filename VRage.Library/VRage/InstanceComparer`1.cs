// Decompiled with JetBrains decompiler
// Type: VRage.InstanceComparer`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VRage
{
  public class InstanceComparer<T> : IEqualityComparer<T> where T : class
  {
    public static readonly InstanceComparer<T> Default = new InstanceComparer<T>();

    public bool Equals(T x, T y) => (object) x == (object) y;

    public int GetHashCode(T obj) => RuntimeHelpers.GetHashCode((object) obj);
  }
}
