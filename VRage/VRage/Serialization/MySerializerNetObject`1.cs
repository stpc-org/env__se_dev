// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MySerializerNetObject`1
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using VRage.Library.Collections;
using VRage.Network;

namespace VRage.Serialization
{
  public class MySerializerNetObject<T> : MySerializer<T> where T : class, IMyNetObject
  {
    public override void Clone(ref T value) => throw new NotSupportedException();

    public override bool Equals(ref T a, ref T b) => (object) a == (object) b;

    public override void Read(BitStream stream, out T value, MySerializeInfo info)
    {
      value = default (T);
      MySerializerNetObject.NetObjectResolver.Resolve<T>(stream, ref value);
    }

    public override void Write(BitStream stream, ref T value, MySerializeInfo info) => MySerializerNetObject.NetObjectResolver.Resolve<T>(stream, ref value);
  }
}
