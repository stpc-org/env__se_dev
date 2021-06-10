// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.__helper_namespace.TypeComparer
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;

namespace VRage.Library.Collections.__helper_namespace
{
  internal class TypeComparer : IComparer<Type>
  {
    public static readonly TypeComparer Instance = new TypeComparer();

    public int Compare(Type x, Type y) => string.CompareOrdinal(x.AssemblyQualifiedName, y.AssemblyQualifiedName);
  }
}
