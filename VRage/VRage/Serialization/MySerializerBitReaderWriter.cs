// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MySerializerBitReaderWriter
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using VRage.Library.Collections;

namespace VRage.Serialization
{
  public class MySerializerBitReaderWriter : MySerializer<BitReaderWriter>
  {
    public override void Clone(ref BitReaderWriter value) => throw new NotSupportedException();

    public override bool Equals(ref BitReaderWriter a, ref BitReaderWriter b) => throw new NotSupportedException();

    public override void Read(BitStream stream, out BitReaderWriter value, MySerializeInfo info) => value = BitReaderWriter.ReadFrom(stream);

    public override void Write(BitStream stream, ref BitReaderWriter value, MySerializeInfo info) => value.Write(stream);
  }
}
