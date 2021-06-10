// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.TupleSerializer`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

namespace VRage.Serialization
{
  public class TupleSerializer<T1> : ISerializer<MyTuple<T1>>
  {
    public readonly ISerializer<T1> m_serializer1;

    public TupleSerializer(ISerializer<T1> serializer1) => this.m_serializer1 = serializer1;

    void ISerializer<MyTuple<T1>>.Serialize(
      ByteStream destination,
      ref MyTuple<T1> data)
    {
      this.m_serializer1.Serialize(destination, ref data.Item1);
    }

    void ISerializer<MyTuple<T1>>.Deserialize(
      ByteStream source,
      out MyTuple<T1> data)
    {
      this.m_serializer1.Deserialize(source, out data.Item1);
    }
  }
}
