// Decompiled with JetBrains decompiler
// Type: VRage.MyTuple`6
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
  public struct MyTuple<T1, T2, T3, T4, T5, T6>
  {
    public T1 Item1;
    public T2 Item2;
    public T3 Item3;
    public T4 Item4;
    public T5 Item5;
    public T6 Item6;

    public MyTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
    {
      this.Item1 = item1;
      this.Item2 = item2;
      this.Item3 = item3;
      this.Item4 = item4;
      this.Item5 = item5;
      this.Item6 = item6;
    }

    protected class VRage_MyTuple`6\u003C\u003EItem1\u003C\u003EAccessor : IMemberAccessor<MyTuple<T1, T2, T3, T4, T5, T6>, T1>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyTuple<T1, T2, T3, T4, T5, T6> owner, in T1 value) => owner.Item1 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyTuple<T1, T2, T3, T4, T5, T6> owner, out T1 value) => value = owner.Item1;
    }

    protected class VRage_MyTuple`6\u003C\u003EItem2\u003C\u003EAccessor : IMemberAccessor<MyTuple<T1, T2, T3, T4, T5, T6>, T2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyTuple<T1, T2, T3, T4, T5, T6> owner, in T2 value) => owner.Item2 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyTuple<T1, T2, T3, T4, T5, T6> owner, out T2 value) => value = owner.Item2;
    }

    protected class VRage_MyTuple`6\u003C\u003EItem3\u003C\u003EAccessor : IMemberAccessor<MyTuple<T1, T2, T3, T4, T5, T6>, T3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyTuple<T1, T2, T3, T4, T5, T6> owner, in T3 value) => owner.Item3 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyTuple<T1, T2, T3, T4, T5, T6> owner, out T3 value) => value = owner.Item3;
    }

    protected class VRage_MyTuple`6\u003C\u003EItem4\u003C\u003EAccessor : IMemberAccessor<MyTuple<T1, T2, T3, T4, T5, T6>, T4>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyTuple<T1, T2, T3, T4, T5, T6> owner, in T4 value) => owner.Item4 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyTuple<T1, T2, T3, T4, T5, T6> owner, out T4 value) => value = owner.Item4;
    }

    protected class VRage_MyTuple`6\u003C\u003EItem5\u003C\u003EAccessor : IMemberAccessor<MyTuple<T1, T2, T3, T4, T5, T6>, T5>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyTuple<T1, T2, T3, T4, T5, T6> owner, in T5 value) => owner.Item5 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyTuple<T1, T2, T3, T4, T5, T6> owner, out T5 value) => value = owner.Item5;
    }

    protected class VRage_MyTuple`6\u003C\u003EItem6\u003C\u003EAccessor : IMemberAccessor<MyTuple<T1, T2, T3, T4, T5, T6>, T6>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyTuple<T1, T2, T3, T4, T5, T6> owner, in T6 value) => owner.Item6 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyTuple<T1, T2, T3, T4, T5, T6> owner, out T6 value) => value = owner.Item6;
    }
  }
}
