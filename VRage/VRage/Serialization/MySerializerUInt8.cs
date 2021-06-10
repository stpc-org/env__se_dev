// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MySerializerUInt8
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRage.Library.Collections;

namespace VRage.Serialization
{
  public class MySerializerUInt8 : MySerializer<byte>
  {
    public override void Clone(ref byte value)
    {
    }

    public override bool Equals(ref byte a, ref byte b) => (int) a == (int) b;

    public override void Read(BitStream stream, out byte value, MySerializeInfo info) => value = stream.ReadByte();

    public override void Write(BitStream stream, ref byte value, MySerializeInfo info) => stream.WriteByte(value);
  }
}
