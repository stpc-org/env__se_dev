// Decompiled with JetBrains decompiler
// Type: VRage.Game.CompactSerializedArray`1
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace VRage.Game
{
  public struct CompactSerializedArray<T>
  {
    private T[] m_data;
    private static int Size = Marshal.SizeOf<T>();

    private CompactSerializedArray(in T[] array) => this.m_data = array;

    [XmlText]
    public byte[] SerializableData
    {
      get
      {
        if (this.m_data == null)
          return (byte[]) null;
        byte[] numArray = new byte[CompactSerializedArray<T>.Size * this.m_data.Length];
        Buffer.BlockCopy((Array) this.m_data, 0, (Array) numArray, 0, numArray.Length);
        return numArray;
      }
      set
      {
        if (value == null)
        {
          this.m_data = (T[]) null;
        }
        else
        {
          this.m_data = new T[value.Length / CompactSerializedArray<T>.Size];
          Buffer.BlockCopy((Array) value, 0, (Array) this.m_data, 0, value.Length);
        }
      }
    }

    public static implicit operator T[](in CompactSerializedArray<T> array) => array.m_data;

    public static implicit operator CompactSerializedArray<T>(in T[] array) => new CompactSerializedArray<T>(in array);
  }
}
