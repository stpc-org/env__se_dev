// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.__helper_namespace.TypeIndexTupleComparer
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;

namespace VRage.Library.Collections.__helper_namespace
{
  internal class TypeIndexTupleComparer : IComparer<MyTuple<Type, int>>
  {
    public static readonly TypeComparer Instance = new TypeComparer();

    public int Compare(MyTuple<Type, int> x, MyTuple<Type, int> y) => string.CompareOrdinal(x.Item1.AssemblyQualifiedName, y.Item1.AssemblyQualifiedName);
  }
}
