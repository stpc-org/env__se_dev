// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MySerializerEnum`1
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using VRage.Library.Collections;
using VRage.Library.Utils;

namespace VRage.Serialization
{
  public class MySerializerEnum<TEnum> : MySerializer<TEnum> where TEnum : struct, IComparable, IFormattable, IConvertible
  {
    private static readonly int m_valueCount = MyEnum<TEnum>.Values.Length;
    private static readonly TEnum m_firstValue = ((IEnumerable<TEnum>) MyEnum<TEnum>.Values).FirstOrDefault<TEnum>();
    private static readonly TEnum m_secondValue = ((IEnumerable<TEnum>) MyEnum<TEnum>.Values).Skip<TEnum>(1).FirstOrDefault<TEnum>();
    private static readonly ulong m_firstUlong = MyEnum<TEnum>.GetValue(MySerializerEnum<TEnum>.m_firstValue);
    private static readonly int m_bitCount = (int) Math.Log((double) MyEnum<TEnum>.GetValue(MyEnum<TEnum>.Range.Max), 2.0) + 1;
    public static readonly bool HasNegativeValues = Comparer<TEnum>.Default.Compare(MyEnum<TEnum>.Range.Min, default (TEnum)) < 0;

    public override void Clone(ref TEnum value)
    {
    }

    public override bool Equals(ref TEnum a, ref TEnum b) => (long) MyEnum<TEnum>.GetValue(a) == (long) MyEnum<TEnum>.GetValue(b);

    public override void Read(BitStream stream, out TEnum value, MySerializeInfo info)
    {
      switch (MySerializerEnum<TEnum>.m_valueCount)
      {
        case 1:
          value = MySerializerEnum<TEnum>.m_firstValue;
          break;
        case 2:
          value = stream.ReadBool() ? MySerializerEnum<TEnum>.m_firstValue : MySerializerEnum<TEnum>.m_secondValue;
          break;
        default:
          if (MySerializerEnum<TEnum>.m_valueCount > 2)
          {
            if (MySerializerEnum<TEnum>.HasNegativeValues)
            {
              value = MyEnum<TEnum>.SetValue((ulong) stream.ReadInt64Variant());
              break;
            }
            value = MyEnum<TEnum>.SetValue(stream.ReadUInt64(MySerializerEnum<TEnum>.m_bitCount));
            break;
          }
          value = default (TEnum);
          break;
      }
    }

    public override void Write(BitStream stream, ref TEnum value, MySerializeInfo info)
    {
      ulong num = MyEnum<TEnum>.GetValue(value);
      if (MySerializerEnum<TEnum>.m_valueCount == 2)
      {
        stream.WriteBool((long) num == (long) MySerializerEnum<TEnum>.m_firstUlong);
      }
      else
      {
        if (MySerializerEnum<TEnum>.m_valueCount <= 2)
          return;
        if (MySerializerEnum<TEnum>.HasNegativeValues)
          stream.WriteVariantSigned((long) num);
        else
          stream.WriteUInt64(num, MySerializerEnum<TEnum>.m_bitCount);
      }
    }
  }
}
