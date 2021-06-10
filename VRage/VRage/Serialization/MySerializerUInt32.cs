// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MySerializerUInt32
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRage.Library.Collections;

namespace VRage.Serialization
{
  public class MySerializerUInt32 : MySerializer<uint>
  {
    public override void Clone(ref uint value)
    {
    }

    public override bool Equals(ref uint a, ref uint b) => (int) a == (int) b;

    public override void Read(BitStream stream, out uint value, MySerializeInfo info)
    {
      if (info.IsVariant || info.IsVariantSigned)
        value = stream.ReadUInt32Variant();
      else
        value = stream.ReadUInt32();
    }

    public override void Write(BitStream stream, ref uint value, MySerializeInfo info)
    {
      if (info.IsVariant || info.IsVariantSigned)
        stream.WriteVariant(value);
      else
        stream.WriteUInt32(value);
    }
  }
}
