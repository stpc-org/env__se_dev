// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MySerializer
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRage.Library.Collections;

namespace VRage.Serialization
{
  public abstract class MySerializer
  {
    protected internal abstract void Clone(ref object value, MySerializeInfo info);

    protected internal abstract bool Equals(ref object a, ref object b, MySerializeInfo info);

    protected internal abstract void Read(BitStream stream, out object value, MySerializeInfo info);

    protected internal abstract void Write(BitStream stream, object value, MySerializeInfo info);

    public static T CreateAndRead<T>(BitStream stream, MySerializeInfo serializeInfo = null)
    {
      T obj;
      MySerializer.CreateAndRead<T>(stream, out obj, serializeInfo);
      return obj;
    }

    public static void CreateAndRead<T>(
      BitStream stream,
      out T value,
      MySerializeInfo serializeInfo = null)
    {
      MySerializationHelpers.CreateAndRead<T>(stream, out value, MyFactory.GetSerializer<T>(), serializeInfo ?? MySerializeInfo.Default);
    }

    public static void Write<T>(BitStream stream, ref T value, MySerializeInfo serializeInfo = null) => MySerializationHelpers.Write<T>(stream, ref value, MyFactory.GetSerializer<T>(), serializeInfo ?? MySerializeInfo.Default);

    public static bool AnyNull(object a, object b) => a == null || b == null;
  }
}
