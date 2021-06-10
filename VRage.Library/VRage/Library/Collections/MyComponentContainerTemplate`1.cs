// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.MyComponentContainerTemplate`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;

namespace VRage.Library.Collections
{
  public class MyComponentContainerTemplate<T> where T : class
  {
    internal MyIndexedComponentContainer<Func<Type, T>> Components = new MyIndexedComponentContainer<Func<Type, T>>();

    public MyComponentContainerTemplate(List<Type> types, List<Func<Type, T>> compoentFactories)
    {
      for (int index = 0; index < types.Count; ++index)
        this.Components.Add(types[index], compoentFactories[index]);
    }

    public MyComponentContainerTemplate(Type[] types, Func<Type, T>[] compoentFactories)
    {
      for (int index = 0; index < types.Length; ++index)
        this.Components.Add(types[index], compoentFactories[index]);
    }
  }
}
