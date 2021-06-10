// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MySerializerDouble
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRage.Library.Collections;

namespace VRage.Serialization
{
  public class MySerializerDouble : MySerializer<double>
  {
    public override void Clone(ref double value)
    {
    }

    public override bool Equals(ref double a, ref double b) => a == b;

    public override void Read(BitStream stream, out double value, MySerializeInfo info) => value = stream.ReadDouble();

    public override void Write(BitStream stream, ref double value, MySerializeInfo info) => stream.WriteDouble(value);
  }
}
