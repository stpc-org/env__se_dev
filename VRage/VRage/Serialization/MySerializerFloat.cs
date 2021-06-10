// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MySerializerFloat
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRage.Library.Collections;

namespace VRage.Serialization
{
  public class MySerializerFloat : MySerializer<float>
  {
    public override void Clone(ref float value)
    {
    }

    public override bool Equals(ref float a, ref float b) => (double) a == (double) b;

    public override void Read(BitStream stream, out float value, MySerializeInfo info)
    {
      if (info.IsNormalized && info.IsFixed8)
        value = (float) stream.ReadByte() / (float) byte.MaxValue;
      else if (info.IsNormalized && info.IsFixed16)
        value = (float) stream.ReadUInt16() / (float) ushort.MaxValue;
      else
        value = stream.ReadFloat();
    }

    public override void Write(BitStream stream, ref float value, MySerializeInfo info)
    {
      if (info.IsNormalized && info.IsFixed8)
        stream.WriteByte((byte) ((double) value * (double) byte.MaxValue));
      else if (info.IsNormalized && info.IsFixed16)
        stream.WriteUInt16((ushort) ((double) value * (double) ushort.MaxValue));
      else
        stream.WriteFloat(value);
    }
  }
}
