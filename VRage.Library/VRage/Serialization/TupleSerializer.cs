// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.TupleSerializer
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

namespace VRage.Serialization
{
  public class TupleSerializer : ISerializer<MyTuple>
  {
    void ISerializer<MyTuple>.Serialize(ByteStream destination, ref MyTuple data)
    {
    }

    void ISerializer<MyTuple>.Deserialize(ByteStream source, out MyTuple data)
    {
    }
  }
}
