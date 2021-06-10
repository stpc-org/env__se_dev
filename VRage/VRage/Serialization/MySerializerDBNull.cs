// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MySerializerDBNull
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using VRage.Library.Collections;

namespace VRage.Serialization
{
  public class MySerializerDBNull : MySerializer<DBNull>
  {
    public override void Clone(ref DBNull value)
    {
    }

    public override bool Equals(ref DBNull a, ref DBNull b) => true;

    public override void Read(BitStream stream, out DBNull value, MySerializeInfo info) => value = DBNull.Value;

    public override void Write(BitStream stream, ref DBNull value, MySerializeInfo info)
    {
    }
  }
}
