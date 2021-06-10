// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.__helper_namespace.ITypeListHelper
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace VRage.Library.Collections.__helper_namespace
{
  internal static class ITypeListHelper
  {
    [ThreadStatic]
    private static TypeListWith m_withInstance;
    [ThreadStatic]
    private static TypeListWithout m_withoutInstance;

    public static int ComputeHashCode(this ITypeList self)
    {
      int num = 757602046;
      for (int index = 0; index < self.Count; ++index)
        num = num * 377 + self[index].GetHashCode();
      return num;
    }

    public static ITypeList With(this TypeList self, int index, Type type)
    {
      TypeListWith typeListWith = ITypeListHelper.m_withInstance ?? (ITypeListHelper.m_withInstance = new TypeListWith());
      typeListWith.Set(self, type, index);
      return (ITypeList) typeListWith;
    }

    public static ITypeList Without(this TypeList self, int index)
    {
      TypeListWithout typeListWithout = ITypeListHelper.m_withoutInstance ?? (ITypeListHelper.m_withoutInstance = new TypeListWithout());
      typeListWithout.Set(self, index);
      return (ITypeList) typeListWithout;
    }
  }
}
