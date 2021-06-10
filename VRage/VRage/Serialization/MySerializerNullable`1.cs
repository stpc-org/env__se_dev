// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MySerializerNullable`1
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRage.Library.Collections;

namespace VRage.Serialization
{
  public class MySerializerNullable<T> : MySerializer<T?> where T : struct
  {
    private MySerializer<T> m_serializer = MyFactory.GetSerializer<T>();

    public override void Clone(ref T? value)
    {
      if (!value.HasValue)
        return;
      T obj = value.Value;
      this.m_serializer.Clone(ref obj);
      value = new T?(obj);
    }

    public override bool Equals(ref T? a, ref T? b)
    {
      if (a.HasValue != b.HasValue)
        return false;
      if (!a.HasValue)
        return true;
      T a1 = a.Value;
      T b1 = b.Value;
      return this.m_serializer.Equals(ref a1, ref b1);
    }

    public override void Read(BitStream stream, out T? value, MySerializeInfo info)
    {
      if (stream.ReadBool())
      {
        T obj;
        this.m_serializer.Read(stream, out obj, info);
        value = new T?(obj);
      }
      else
        value = new T?();
    }

    public override void Write(BitStream stream, ref T? value, MySerializeInfo info)
    {
      if (value.HasValue)
      {
        T obj = value.Value;
        stream.WriteBool(true);
        this.m_serializer.Write(stream, ref obj, info);
      }
      else
        stream.WriteBool(false);
    }
  }
}
