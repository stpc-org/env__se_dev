﻿// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.TupleSerializer`5
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

namespace VRage.Serialization
{
  public class TupleSerializer<T1, T2, T3, T4, T5> : ISerializer<MyTuple<T1, T2, T3, T4, T5>>
  {
    public readonly ISerializer<T1> m_serializer1;
    public readonly ISerializer<T2> m_serializer2;
    public readonly ISerializer<T3> m_serializer3;
    public readonly ISerializer<T4> m_serializer4;
    public readonly ISerializer<T5> m_serializer5;

    public TupleSerializer(
      ISerializer<T1> serializer1,
      ISerializer<T2> serializer2,
      ISerializer<T3> serializer3,
      ISerializer<T4> serializer4,
      ISerializer<T5> serializer5)
    {
      this.m_serializer1 = serializer1;
      this.m_serializer2 = serializer2;
      this.m_serializer3 = serializer3;
      this.m_serializer4 = serializer4;
      this.m_serializer5 = serializer5;
    }

    void ISerializer<MyTuple<T1, T2, T3, T4, T5>>.Serialize(
      ByteStream destination,
      ref MyTuple<T1, T2, T3, T4, T5> data)
    {
      this.m_serializer1.Serialize(destination, ref data.Item1);
      this.m_serializer2.Serialize(destination, ref data.Item2);
      this.m_serializer3.Serialize(destination, ref data.Item3);
      this.m_serializer4.Serialize(destination, ref data.Item4);
      this.m_serializer5.Serialize(destination, ref data.Item5);
    }

    void ISerializer<MyTuple<T1, T2, T3, T4, T5>>.Deserialize(
      ByteStream source,
      out MyTuple<T1, T2, T3, T4, T5> data)
    {
      this.m_serializer1.Deserialize(source, out data.Item1);
      this.m_serializer2.Deserialize(source, out data.Item2);
      this.m_serializer3.Deserialize(source, out data.Item3);
      this.m_serializer4.Deserialize(source, out data.Item4);
      this.m_serializer5.Deserialize(source, out data.Item5);
    }
  }
}
