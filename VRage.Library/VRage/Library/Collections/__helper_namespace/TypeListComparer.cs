// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.__helper_namespace.TypeListComparer
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections.Generic;

namespace VRage.Library.Collections.__helper_namespace
{
  internal class TypeListComparer : IEqualityComparer<ITypeList>
  {
    public bool Equals(ITypeList x, ITypeList y)
    {
      if (x.Count != y.Count)
        return false;
      for (int index = 0; index < x.Count; ++index)
      {
        if (x[index] != y[index])
          return false;
      }
      return true;
    }

    public int GetHashCode(ITypeList obj) => obj.HashCode;
  }
}
