// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MySerializerUInt16
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRage.Library.Collections;

namespace VRage.Serialization
{
  public class MySerializerUInt16 : MySerializer<ushort>
  {
    public override void Clone(ref ushort value)
    {
    }

    public override bool Equals(ref ushort a, ref ushort b) => (int) a == (int) b;

    public override void Read(BitStream stream, out ushort value, MySerializeInfo info)
    {
      if (info.IsVariant || info.IsVariantSigned)
        value = (ushort) stream.ReadUInt32Variant();
      else
        value = stream.ReadUInt16();
    }

    public override void Write(BitStream stream, ref ushort value, MySerializeInfo info)
    {
      if (info.IsVariant || info.IsVariantSigned)
        stream.WriteVariant((uint) value);
      else
        stream.WriteUInt16(value);
    }
  }
}
