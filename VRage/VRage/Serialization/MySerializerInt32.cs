// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MySerializerInt32
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRage.Library.Collections;

namespace VRage.Serialization
{
  public class MySerializerInt32 : MySerializer<int>
  {
    public override void Clone(ref int value)
    {
    }

    public override bool Equals(ref int a, ref int b) => a == b;

    public override void Read(BitStream stream, out int value, MySerializeInfo info)
    {
      if (info.IsVariant)
        value = (int) stream.ReadUInt32Variant();
      else if (info.IsVariantSigned)
        value = stream.ReadInt32Variant();
      else
        value = stream.ReadInt32();
    }

    public override void Write(BitStream stream, ref int value, MySerializeInfo info)
    {
      if (info.IsVariant)
        stream.WriteVariant((uint) value);
      else if (info.IsVariantSigned)
        stream.WriteVariantSigned(value);
      else
        stream.WriteInt32(value);
    }
  }
}
