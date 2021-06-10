// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MySerializerMyStringHash
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRage.Library.Collections;
using VRage.Utils;

namespace VRage.Serialization
{
  public class MySerializerMyStringHash : MySerializer<MyStringHash>
  {
    public override void Clone(ref MyStringHash value)
    {
    }

    public override bool Equals(ref MyStringHash a, ref MyStringHash b) => a.Equals(b);

    public override void Read(BitStream stream, out MyStringHash value, MySerializeInfo info) => value = MyStringHash.TryGet(stream.ReadInt32());

    public override void Write(BitStream stream, ref MyStringHash value, MySerializeInfo info) => stream.WriteInt32((int) value);
  }
}
