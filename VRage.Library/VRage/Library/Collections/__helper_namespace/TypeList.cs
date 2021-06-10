// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.__helper_namespace.TypeList
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;

namespace VRage.Library.Collections.__helper_namespace
{
  internal class TypeList : List<Type>, ITypeList
  {
    private int m_cachedHashCode;

    public void UpdateHashCode() => this.m_cachedHashCode = this.ComputeHashCode();

    public int HashCode => this.m_cachedHashCode;

    public TypeList GetSolidified() => this;

    public override string ToString() => string.Join(", ", this.Select<Type, string>((Func<Type, string>) (x => x.Name)));
  }
}
