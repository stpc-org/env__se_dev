// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MySerializerInt16
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRage.Library.Collections;

namespace VRage.Serialization
{
  public class MySerializerInt16 : MySerializer<short>
  {
    public override void Clone(ref short value)
    {
    }

    public override bool Equals(ref short a, ref short b) => (int) a == (int) b;

    public override void Read(BitStream stream, out short value, MySerializeInfo info)
    {
      if (info.IsVariant)
        value = (short) stream.ReadUInt32Variant();
      else if (info.IsVariantSigned)
        value = (short) stream.ReadInt32Variant();
      else
        value = stream.ReadInt16();
    }

    public override void Write(BitStream stream, ref short value, MySerializeInfo info)
    {
      if (info.IsVariant)
        stream.WriteVariant((uint) (ushort) value);
      else if (info.IsVariantSigned)
        stream.WriteVariantSigned((int) value);
      else
        stream.WriteInt16(value);
    }
  }
}
