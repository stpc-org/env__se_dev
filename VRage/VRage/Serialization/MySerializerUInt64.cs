// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MySerializerUInt64
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRage.Library.Collections;

namespace VRage.Serialization
{
  public class MySerializerUInt64 : MySerializer<ulong>
  {
    public override void Clone(ref ulong value)
    {
    }

    public override bool Equals(ref ulong a, ref ulong b) => (long) a == (long) b;

    public override void Read(BitStream stream, out ulong value, MySerializeInfo info)
    {
      if (info.IsVariant || info.IsVariantSigned)
        value = stream.ReadUInt64Variant();
      else
        value = stream.ReadUInt64();
    }

    public override void Write(BitStream stream, ref ulong value, MySerializeInfo info)
    {
      if (info.IsVariant || info.IsVariantSigned)
        stream.WriteVariant(value);
      else
        stream.WriteUInt64(value);
    }
  }
}
