// Decompiled with JetBrains decompiler
// Type: VRage.MyTuple`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using VRage.Network;

namespace VRage
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct MyTuple<T1>
  {
    public T1 Item1;

    public MyTuple(T1 item1) => this.Item1 = item1;

    protected class VRage_MyTuple`1\u003C\u003EItem1\u003C\u003EAccessor : IMemberAccessor<MyTuple<T1>, T1>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyTuple<T1> owner, in T1 value) => owner.Item1 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyTuple<T1> owner, out T1 value) => value = owner.Item1;
    }
  }
}
