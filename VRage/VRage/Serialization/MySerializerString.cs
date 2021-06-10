// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MySerializerString
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRage.Library.Collections;

namespace VRage.Serialization
{
  public class MySerializerString : MySerializer<string>
  {
    public override void Clone(ref string value)
    {
    }

    public override bool Equals(ref string a, ref string b) => a == b;

    public override void Read(BitStream stream, out string value, MySerializeInfo info) => value = stream.ReadPrefixLengthString(info.Encoding);

    public override void Write(BitStream stream, ref string value, MySerializeInfo info) => stream.WritePrefixLengthString(value, 0, value.Length, info.Encoding);
  }
}
