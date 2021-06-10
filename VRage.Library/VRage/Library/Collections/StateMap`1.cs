// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.StateMap`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace VRage.Library.Collections
{
  public class StateMap<T> : IReadOnlyList<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>
    where T : unmanaged, Enum
  {
    private ulong[] m_data;
    private int m_size;
    private readonly T m_default;
    public static readonly int ElementBitSize;
    public static readonly int ElementByteSize;
    public static readonly ulong ElementMask;
    public static readonly bool IsSigned;
    public static bool IsFlags;
    private static HashSet<Type> SignedTypes = new HashSet<Type>()
    {
      typeof (sbyte),
      typeof (short),
      typeof (int),
      typeof (long)
    };
    private static Dictionary<Type, int> BitCount = new Dictionary<Type, int>()
    {
      {
        typeof (sbyte),
        8
      },
      {
        typeof (byte),
        8
      },
      {
        typeof (short),
        16
      },
      {
        typeof (ushort),
        16
      },
      {
        typeof (int),
        32
      },
      {
        typeof (uint),
        32
      },
      {
        typeof (long),
        64
      },
      {
        typeof (ulong),
        64
      }
    };

    public StateMap(int count, T defaultState = default (T))
    {
      this.m_default = defaultState;
      this.Resize(count);
    }

    public void Resize(int count)
    {
      int size = this.m_size;
      this.m_size = count;
      Array.Resize<ulong>(ref this.m_data, count * StateMap<T>.ElementBitSize + 63 >> 6);
      if (StateMap<T>.Cast(this.m_default) == 0UL)
        return;
      for (int index = size; index < this.m_size; ++index)
        this[index] = this.m_default;
    }

    public int Count => this.m_size;

    public T this[int index]
    {
      get
      {
        if (index < 0 || index >= this.m_size)
          throw new ArgumentOutOfRangeException(nameof (index));
        int cell;
        int offset;
        this.ComputeCell(index, out cell, out offset);
        ulong num = this.m_data[cell] >> offset & StateMap<T>.ElementMask;
        if (StateMap<T>.IsSigned && ((long) num & ~(long) (StateMap<T>.ElementMask >> 1)) != 0L)
          num |= ~StateMap<T>.ElementMask;
        return StateMap<T>.Cast(num);
      }
      set
      {
        if (index < 0 || index >= this.m_size)
          throw new ArgumentOutOfRangeException(nameof (index));
        int cell;
        int offset;
        this.ComputeCell(index, out cell, out offset);
        ulong num = (ulong) (((long) StateMap<T>.Cast(value) & (long) StateMap<T>.ElementMask) << offset);
        this.m_data[cell] &= (ulong) ~((long) StateMap<T>.ElementMask << offset);
        this.m_data[cell] |= num;
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ComputeCell(int index, out int cell, out int offset)
    {
      int num = index * StateMap<T>.ElementBitSize;
      offset = num & 63;
      cell = num >> 6;
    }

    public IEnumerator<T> GetEnumerator() => throw new NotImplementedException();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    static StateMap()
    {
      Type type = typeof (T);
      StateMap<T>.IsSigned = StateMap<T>.SignedTypes.Contains(Enum.GetUnderlyingType(type));
      StateMap<T>.IsFlags = type.HasAttribute<FlagsAttribute>();
      StateMap<T>.ElementByteSize = sizeof (T);
      Array values = Enum.GetValues(typeof (T));
      ulong val1 = 0;
      bool flag = false;
      foreach (T obj in values)
      {
        ulong val2 = StateMap<T>.Cast(obj);
        if (StateMap<T>.IsSigned && (long) val2 < 0L)
        {
          flag = true;
          if (!StateMap<T>.IsFlags)
            val2 = (ulong) -(long) val2;
        }
        val1 = Math.Max(val1, val2);
      }
      if (((!StateMap<T>.IsSigned ? 0 : (StateMap<T>.IsFlags ? 1 : 0)) & (flag ? 1 : 0)) != 0)
      {
        StateMap<T>.ElementBitSize = StateMap<T>.ElementByteSize * 8;
      }
      else
      {
        int num = 65;
        for (ulong maxValue = ulong.MaxValue; maxValue > 0UL && maxValue >= val1; maxValue >>= 1)
          --num;
        StateMap<T>.ElementBitSize = num;
        if (flag)
          ++StateMap<T>.ElementBitSize;
      }
      StateMap<T>.ElementMask = (ulong) (1L << StateMap<T>.ElementBitSize) - 1UL;
    }

    private static unsafe T Cast(ulong value) => *(T*) &value;

    private static unsafe ulong Cast(T value)
    {
      switch (StateMap<T>.ElementByteSize)
      {
        case 1:
          return StateMap<T>.IsSigned ? (ulong) *(sbyte*) &value : (ulong) *(byte*) &value;
        case 2:
          return StateMap<T>.IsSigned ? (ulong) *(short*) &value : (ulong) *(ushort*) &value;
        case 4:
          return StateMap<T>.IsSigned ? (ulong) *(int*) &value : (ulong) *(uint*) &value;
        case 8:
          int num = StateMap<T>.IsSigned ? 1 : 0;
          return (ulong) *(long*) &value;
        default:
          throw new InvalidBranchException();
      }
    }
  }
}
