// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.TypeSwitchBase`2
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;

namespace VRage.Library.Collections
{
  public abstract class TypeSwitchBase<TKeyBase, TValBase> where TValBase : class
  {
    public Dictionary<Type, TValBase> Matches { get; private set; }

    protected TypeSwitchBase() => this.Matches = new Dictionary<Type, TValBase>();

    public TypeSwitchBase<TKeyBase, TValBase> Case<TKey>(TValBase action) where TKey : class, TKeyBase
    {
      this.Matches.Add(typeof (TKey), action);
      return this;
    }

    protected TValBase SwitchInternal<TKey>() where TKey : class, TKeyBase
    {
      TValBase valBase;
      return !this.Matches.TryGetValue(typeof (TKey), out valBase) ? default (TValBase) : valBase;
    }
  }
}
