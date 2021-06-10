// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.ProtoSerializer`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using ProtoBuf.Meta;
using System.IO;

namespace VRage.Serialization
{
  public class ProtoSerializer<T> : ISerializer<T>
  {
    public readonly TypeModel Model;
    public static readonly ProtoSerializer<T> Default = new ProtoSerializer<T>();

    public ProtoSerializer(TypeModel model = null) => this.Model = model ?? TypeModel.Default;

    public void Serialize(ByteStream destination, ref T data) => this.Model.Serialize((Stream) destination, (object) data);

    public void Deserialize(ByteStream source, out T data) => data = (T) this.Model.Deserialize((Stream) source, (object) null, typeof (T));
  }
}
