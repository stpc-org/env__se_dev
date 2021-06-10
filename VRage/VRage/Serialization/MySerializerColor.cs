// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MySerializerColor
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRage.Library.Collections;
using VRageMath;

namespace VRage.Serialization
{
  public class MySerializerColor : MySerializer<Color>
  {
    public override void Clone(ref Color value)
    {
    }

    public override bool Equals(ref Color a, ref Color b) => (int) a.PackedValue == (int) b.PackedValue;

    public override void Read(BitStream stream, out Color value, MySerializeInfo info) => value.PackedValue = stream.ReadUInt32();

    public override void Write(BitStream stream, ref Color value, MySerializeInfo info) => stream.WriteUInt32(value.PackedValue);
  }
}
