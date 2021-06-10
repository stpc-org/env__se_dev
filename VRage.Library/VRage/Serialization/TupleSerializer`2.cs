// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.TupleSerializer`2
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

namespace VRage.Serialization
{
  public class TupleSerializer<T1, T2> : ISerializer<MyTuple<T1, T2>>
  {
    public readonly ISerializer<T1> m_serializer1;
    public readonly ISerializer<T2> m_serializer2;

    public TupleSerializer(ISerializer<T1> serializer1, ISerializer<T2> serializer2)
    {
      this.m_serializer1 = serializer1;
      this.m_serializer2 = serializer2;
    }

    void ISerializer<MyTuple<T1, T2>>.Serialize(
      ByteStream destination,
      ref MyTuple<T1, T2> data)
    {
      this.m_serializer1.Serialize(destination, ref data.Item1);
      this.m_serializer2.Serialize(destination, ref data.Item2);
    }

    void ISerializer<MyTuple<T1, T2>>.Deserialize(
      ByteStream source,
      out MyTuple<T1, T2> data)
    {
      this.m_serializer1.Deserialize(source, out data.Item1);
      this.m_serializer2.Deserialize(source, out data.Item2);
    }
  }
}
