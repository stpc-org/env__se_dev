// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MySerializer`1
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRage.Library.Collections;

namespace VRage.Serialization
{
  public abstract class MySerializer<T> : MySerializer
  {
    public static bool IsValueType => typeof (T).IsValueType;

    public static bool IsClass => !MySerializer<T>.IsValueType;

    public abstract void Clone(ref T value);

    public abstract bool Equals(ref T a, ref T b);

    public abstract void Read(BitStream stream, out T value, MySerializeInfo info);

    public abstract void Write(BitStream stream, ref T value, MySerializeInfo info);

    protected internal override sealed void Clone(ref object value, MySerializeInfo info)
    {
      T obj = (T) value;
      this.Clone(ref obj);
      value = (object) obj;
    }

    protected internal override sealed bool Equals(
      ref object a,
      ref object b,
      MySerializeInfo info)
    {
      T a1 = (T) a;
      T b1 = (T) b;
      return this.Equals(ref a1, ref b1);
    }

    protected internal override sealed void Read(
      BitStream stream,
      out object value,
      MySerializeInfo info)
    {
      T obj;
      this.Read(stream, out obj, info);
      value = (object) obj;
    }

    protected internal override sealed void Write(
      BitStream stream,
      object value,
      MySerializeInfo info)
    {
      T obj = (T) value;
      this.Write(stream, ref obj, info);
    }
  }
}
