// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MySerializationHelpers
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using VRage.Library.Collections;

namespace VRage.Serialization
{
  public static class MySerializationHelpers
  {
    public static bool CreateAndRead<TMember>(
      BitStream stream,
      out TMember result,
      MySerializer<TMember> serializer,
      MySerializeInfo info)
    {
      if (MySerializationHelpers.ReadNullable(stream, info.IsNullable))
      {
        if (MySerializer<TMember>.IsClass && info.IsDynamic)
        {
          Type t = typeof (TMember);
          bool flag = true;
          if (info.IsDynamicDefault)
            flag = stream.ReadBool();
          if (flag)
            t = stream.ReadDynamicType(typeof (TMember), info.DynamicSerializer);
          object obj;
          MyFactory.GetSerializer(t).Read(stream, out obj, info);
          result = (TMember) obj;
        }
        else
          serializer.Read(stream, out result, info);
        return true;
      }
      result = default (TMember);
      return false;
    }

    public static void Write<TMember>(
      BitStream stream,
      ref TMember value,
      MySerializer<TMember> serializer,
      MySerializeInfo info)
    {
      if (!MySerializationHelpers.WriteNullable<TMember>(stream, ref value, info.IsNullable, serializer))
        return;
      if (MySerializer<TMember>.IsClass && info.IsDynamic)
      {
        Type baseType = typeof (TMember);
        Type type = value.GetType();
        bool flag = true;
        if (info.IsDynamicDefault)
        {
          flag = baseType != type;
          stream.WriteBool(flag);
        }
        if (flag)
          stream.WriteDynamicType(baseType, type, info.DynamicSerializer);
        MyFactory.GetSerializer(value.GetType()).Write(stream, (object) value, info);
      }
      else
      {
        if (!MySerializer<TMember>.IsValueType && !(value.GetType() == typeof (TMember)))
          throw new MySerializeException(MySerializeErrorEnum.DynamicNotAllowed);
        serializer.Write(stream, ref value, info);
      }
    }

    private static bool ReadNullable(BitStream stream, bool isNullable) => !isNullable || stream.ReadBool();

    private static bool WriteNullable<T>(
      BitStream stream,
      ref T value,
      bool isNullable,
      MySerializer<T> serializer)
    {
      if (isNullable)
      {
        T b = default (T);
        bool flag = !serializer.Equals(ref value, ref b);
        stream.WriteBool(flag);
        return flag;
      }
      if (!typeof (T).IsValueType && (object) value == null)
        throw new MySerializeException(MySerializeErrorEnum.NullNotAllowed);
      return true;
    }
  }
}
