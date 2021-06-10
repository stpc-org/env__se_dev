// Decompiled with JetBrains decompiler
// Type: VRage.Reflection.FullyQualifiedNameComparer
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;

namespace VRage.Reflection
{
  public class FullyQualifiedNameComparer : IComparer<Type>
  {
    public static readonly FullyQualifiedNameComparer Default = new FullyQualifiedNameComparer();

    public int Compare(Type x, Type y) => x.FullName.CompareTo(y.FullName);
  }
}
