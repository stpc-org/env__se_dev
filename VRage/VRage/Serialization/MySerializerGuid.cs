// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MySerializerGuid
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using VRage.Library.Collections;

namespace VRage.Serialization
{
  public class MySerializerGuid : MySerializer<Guid>
  {
    public override void Clone(ref Guid value)
    {
    }

    public override bool Equals(ref Guid a, ref Guid b) => a == b;

    public override void Read(BitStream stream, out Guid value, MySerializeInfo info)
    {
      string g = stream.ReadPrefixLengthString(info.Encoding);
      value = new Guid(g);
    }

    public override void Write(BitStream stream, ref Guid value, MySerializeInfo info)
    {
      string str = value.ToString();
      stream.WritePrefixLengthString(str, 0, str.Length, info.Encoding);
    }
  }
}
