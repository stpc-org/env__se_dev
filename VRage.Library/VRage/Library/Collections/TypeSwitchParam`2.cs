// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.TypeSwitchParam`2
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace VRage.Library.Collections
{
  public sealed class TypeSwitchParam<TKeyBase, TParam> : TypeSwitchBase<TKeyBase, Func<TParam, TKeyBase>>
  {
    public TRet Switch<TRet>(TParam par) where TRet : class, TKeyBase
    {
      Func<TParam, TKeyBase> func = this.SwitchInternal<TRet>();
      return func != null ? (TRet) (object) func(par) : default (TRet);
    }
  }
}
