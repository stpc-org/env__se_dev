// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.ComponentIndex
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;
using VRage.Library.Collections.__helper_namespace;

namespace VRage.Library.Collections
{
  public class ComponentIndex
  {
    internal readonly TypeList Types;
    public readonly Dictionary<Type, int> Index = new Dictionary<Type, int>();

    internal ComponentIndex(TypeList typeList)
    {
      for (int index = 0; index < typeList.Count; ++index)
        this.Index[typeList[index]] = index;
      this.Types = typeList;
    }
  }
}
