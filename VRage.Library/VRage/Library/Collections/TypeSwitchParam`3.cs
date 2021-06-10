// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.TypeSwitchParam`3
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace VRage.Library.Collections
{
  public sealed class TypeSwitchParam<TKeyBase, TParam1, TParam2> : TypeSwitchBase<TKeyBase, Func<TParam1, TParam2, TKeyBase>>
  {
    public TRet Switch<TRet>(TParam1 par1, TParam2 par2) where TRet : class, TKeyBase
    {
      Func<TParam1, TParam2, TKeyBase> func = this.SwitchInternal<TRet>();
      return func != null ? (TRet) (object) func(par1, par2) : default (TRet);
    }
  }
}
