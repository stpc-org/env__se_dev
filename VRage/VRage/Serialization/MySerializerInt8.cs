// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MySerializerInt8
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRage.Library.Collections;

namespace VRage.Serialization
{
  public class MySerializerInt8 : MySerializer<sbyte>
  {
    public override void Clone(ref sbyte value)
    {
    }

    public override bool Equals(ref sbyte a, ref sbyte b) => (int) a == (int) b;

    public override void Read(BitStream stream, out sbyte value, MySerializeInfo info) => value = stream.ReadSByte();

    public override void Write(BitStream stream, ref sbyte value, MySerializeInfo info) => stream.WriteSByte(value);
  }
}
