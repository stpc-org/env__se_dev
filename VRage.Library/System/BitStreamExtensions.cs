// Decompiled with JetBrains decompiler
// Type: System.BitStreamExtensions
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections.Generic;
using VRage.Library.Collections;

namespace System
{
  public static class BitStreamExtensions
  {
    private static void Serialize<T>(
      this BitStream bs,
      T[] data,
      int len,
      BitStreamExtensions.SerializeCallback<T> serializer)
    {
      for (int index = 0; index < len; ++index)
        serializer(bs, ref data[index]);
    }

    public static void SerializeList<T>(
      this BitStream bs,
      ref List<T> list,
      BitStreamExtensions.SerializeCallback<T> serializer)
    {
      if (bs.Writing)
      {
        bs.WriteVariant((uint) list.Count);
        for (int index = 0; index < list.Count; ++index)
        {
          T obj = list[index];
          serializer(bs, ref obj);
        }
      }
      else
      {
        T obj = default (T);
        int capacity = (int) bs.ReadUInt32Variant();
        list = list ?? new List<T>(capacity);
        list.Clear();
        for (int index = 0; index < capacity; ++index)
        {
          serializer(bs, ref obj);
          list.Add(obj);
        }
      }
    }

    public static void SerializeList<T>(
      this BitStream bs,
      ref List<T> list,
      BitStreamExtensions.Reader<T> reader,
      BitStreamExtensions.Writer<T> writer)
    {
      if (bs.Writing)
      {
        bs.WriteVariant((uint) list.Count);
        for (int index = 0; index < list.Count; ++index)
          writer(bs, list[index]);
      }
      else
      {
        int capacity = (int) bs.ReadUInt32Variant();
        list = list ?? new List<T>(capacity);
        list.Clear();
        for (int index = 0; index < capacity; ++index)
          list.Add(reader(bs));
      }
    }

    public static void SerializeList(this BitStream bs, ref List<int> list) => bs.SerializeList<int>(ref list, (BitStreamExtensions.Reader<int>) (b => b.ReadInt32()), (BitStreamExtensions.Writer<int>) ((b, v) => b.WriteInt32(v)));

    public static void SerializeList(this BitStream bs, ref List<uint> list) => bs.SerializeList<uint>(ref list, (BitStreamExtensions.Reader<uint>) (b => b.ReadUInt32()), (BitStreamExtensions.Writer<uint>) ((b, v) => b.WriteUInt32(v)));

    public static void SerializeList(this BitStream bs, ref List<long> list) => bs.SerializeList<long>(ref list, (BitStreamExtensions.Reader<long>) (b => b.ReadInt64()), (BitStreamExtensions.Writer<long>) ((b, v) => b.WriteInt64(v)));

    public static void SerializeList(this BitStream bs, ref List<ulong> list) => bs.SerializeList<ulong>(ref list, (BitStreamExtensions.Reader<ulong>) (b => b.ReadUInt64()), (BitStreamExtensions.Writer<ulong>) ((b, v) => b.WriteUInt64(v)));

    public static void SerializeListVariant(this BitStream bs, ref List<int> list) => bs.SerializeList<int>(ref list, (BitStreamExtensions.Reader<int>) (b => b.ReadInt32Variant()), (BitStreamExtensions.Writer<int>) ((b, v) => b.WriteVariantSigned(v)));

    public static void SerializeListVariant(this BitStream bs, ref List<uint> list) => bs.SerializeList<uint>(ref list, (BitStreamExtensions.Reader<uint>) (b => b.ReadUInt32Variant()), (BitStreamExtensions.Writer<uint>) ((b, v) => b.WriteVariant(v)));

    public static void SerializeListVariant(this BitStream bs, ref List<long> list) => bs.SerializeList<long>(ref list, (BitStreamExtensions.Reader<long>) (b => b.ReadInt64Variant()), (BitStreamExtensions.Writer<long>) ((b, v) => b.WriteVariantSigned(v)));

    public static void SerializeListVariant(this BitStream bs, ref List<ulong> list) => bs.SerializeList<ulong>(ref list, (BitStreamExtensions.Reader<ulong>) (b => b.ReadUInt64Variant()), (BitStreamExtensions.Writer<ulong>) ((b, v) => b.WriteVariant(v)));

    public delegate void SerializeCallback<T>(BitStream stream, ref T item);

    public delegate T Reader<T>(BitStream bs);

    public delegate void Writer<T>(BitStream bs, T value);
  }
}
