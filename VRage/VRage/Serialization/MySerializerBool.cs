// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MySerializerBool
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRage.Library.Collections;

namespace VRage.Serialization
{
  public class MySerializerBool : MySerializer<bool>
  {
    public override void Clone(ref bool value)
    {
    }

    public override bool Equals(ref bool a, ref bool b) => a == b;

    public override void Read(BitStream stream, out bool value, MySerializeInfo info) => value = stream.ReadBool();

    public override void Write(BitStream stream, ref bool value, MySerializeInfo info) => stream.WriteBool(value);
  }
}
