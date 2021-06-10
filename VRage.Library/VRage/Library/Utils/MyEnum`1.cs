// Decompiled with JetBrains decompiler
// Type: VRage.Library.Utils.MyEnum`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using VRage.Compiler;

namespace VRage.Library.Utils
{
  public static class MyEnum<T> where T : struct, IComparable, IFormattable, IConvertible
  {
    public static readonly T[] Values = (T[]) Enum.GetValues(typeof (T));
    public static readonly Type UnderlyingType = typeof (T).UnderlyingSystemType;
    private static readonly Dictionary<int, string> m_names = new Dictionary<int, string>();

    public static string Name => TypeNameHelper<T>.Name;

    public static string GetName(T value)
    {
      int key = Array.IndexOf<T>(MyEnum<T>.Values, value);
      string str;
      if (!MyEnum<T>.m_names.TryGetValue(key, out str))
      {
        str = value.ToString();
        MyEnum<T>.m_names[key] = str;
      }
      return str;
    }

    public static unsafe ulong GetValue(T value)
    {
      void* source = Unsafe.AsPointer<T>(ref value);
      switch (TypeExtensions.SizeOf<T>())
      {
        case 1:
          return (ulong) Unsafe.ReadUnaligned<byte>(source);
        case 2:
          return (ulong) Unsafe.ReadUnaligned<ushort>(source);
        case 4:
          return (ulong) Unsafe.ReadUnaligned<uint>(source);
        default:
          return Unsafe.ReadUnaligned<ulong>(source);
      }
    }

    public static unsafe T SetValue(ulong value)
    {
      T obj = default (T);
      void* destination = Unsafe.AsPointer<T>(ref obj);
      switch (TypeExtensions.SizeOf<T>())
      {
        case 1:
          Unsafe.WriteUnaligned<byte>(destination, (byte) value);
          break;
        case 2:
          Unsafe.WriteUnaligned<ushort>(destination, (ushort) value);
          break;
        case 4:
          Unsafe.WriteUnaligned<uint>(destination, (uint) value);
          break;
        default:
          Unsafe.WriteUnaligned<ulong>(destination, value);
          break;
      }
      return obj;
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct Range
    {
      public static readonly T Min;
      public static readonly T Max;

      static Range()
      {
        T[] values = MyEnum<T>.Values;
        Comparer<T> comparer = Comparer<T>.Default;
        if (values.Length == 0)
          return;
        MyEnum<T>.Range.Max = values[0];
        MyEnum<T>.Range.Min = values[0];
        for (int index = 1; index < values.Length; ++index)
        {
          T y = values[index];
          if (comparer.Compare(MyEnum<T>.Range.Max, y) < 0)
            MyEnum<T>.Range.Max = y;
          if (comparer.Compare(MyEnum<T>.Range.Min, y) > 0)
            MyEnum<T>.Range.Min = y;
        }
      }
    }
  }
}
