// Decompiled with JetBrains decompiler
// Type: System.SerializationExtensionsMath
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System.Collections.Generic;
using VRage.Library.Collections;
using VRageMath;
using VRageMath.PackedVector;

namespace System
{
  public static class SerializationExtensionsMath
  {
    public static void Serialize(this BitStream stream, ref Vector2 vec)
    {
      stream.Serialize(ref vec.X);
      stream.Serialize(ref vec.Y);
    }

    public static void Serialize(this BitStream stream, ref Vector3 vec)
    {
      stream.Serialize(ref vec.X);
      stream.Serialize(ref vec.Y);
      stream.Serialize(ref vec.Z);
    }

    public static void Serialize(this BitStream stream, ref Vector4 vec)
    {
      stream.Serialize(ref vec.X);
      stream.Serialize(ref vec.Y);
      stream.Serialize(ref vec.Z);
      stream.Serialize(ref vec.W);
    }

    public static Quaternion ReadQuaternion(this BitStream stream)
    {
      Quaternion quaternion;
      quaternion.X = stream.ReadFloat();
      quaternion.Y = stream.ReadFloat();
      quaternion.Z = stream.ReadFloat();
      quaternion.W = stream.ReadFloat();
      return quaternion;
    }

    public static void WriteQuaternion(this BitStream stream, Quaternion q)
    {
      stream.WriteFloat(q.X);
      stream.WriteFloat(q.Y);
      stream.WriteFloat(q.Z);
      stream.WriteFloat(q.W);
    }

    public static void Serialize(this BitStream stream, ref Quaternion quat)
    {
      stream.Serialize(ref quat.X);
      stream.Serialize(ref quat.Y);
      stream.Serialize(ref quat.Z);
      stream.Serialize(ref quat.W);
    }

    public static Quaternion ReadQuaternionNorm(this BitStream stream)
    {
      int num1 = stream.ReadBool() ? 1 : 0;
      int num2 = stream.ReadBool() ? 1 : 0;
      bool flag1 = stream.ReadBool();
      bool flag2 = stream.ReadBool();
      ushort num3 = stream.ReadUInt16();
      ushort num4 = stream.ReadUInt16();
      ushort num5 = stream.ReadUInt16();
      Quaternion quaternion;
      quaternion.X = (float) num3 / (float) ushort.MaxValue;
      quaternion.Y = (float) num4 / (float) ushort.MaxValue;
      quaternion.Z = (float) num5 / (float) ushort.MaxValue;
      if (num2 != 0)
        quaternion.X = -quaternion.X;
      if (flag1)
        quaternion.Y = -quaternion.Y;
      if (flag2)
        quaternion.Z = -quaternion.Z;
      float num6 = (float) (1.0 - (double) quaternion.X * (double) quaternion.X - (double) quaternion.Y * (double) quaternion.Y - (double) quaternion.Z * (double) quaternion.Z);
      if ((double) num6 < 0.0)
        num6 = 0.0f;
      quaternion.W = (float) Math.Sqrt((double) num6);
      if (num1 != 0)
        quaternion.W = -quaternion.W;
      return quaternion;
    }

    public static void WriteQuaternionNorm(this BitStream stream, Quaternion value)
    {
      stream.WriteBool((double) value.W < 0.0);
      stream.WriteBool((double) value.X < 0.0);
      stream.WriteBool((double) value.Y < 0.0);
      stream.WriteBool((double) value.Z < 0.0);
      stream.WriteUInt16((ushort) ((double) Math.Abs(value.X) * (double) ushort.MaxValue));
      stream.WriteUInt16((ushort) ((double) Math.Abs(value.Y) * (double) ushort.MaxValue));
      stream.WriteUInt16((ushort) ((double) Math.Abs(value.Z) * (double) ushort.MaxValue));
    }

    public static void SerializeNorm(this BitStream stream, ref Quaternion quat)
    {
      if (stream.Reading)
        quat = stream.ReadQuaternionNorm();
      else
        stream.WriteQuaternionNorm(quat);
    }

    public static void Serialize(this BitStream stream, ref HalfVector4 vec) => stream.Serialize(ref vec.PackedValue);

    public static void Serialize(this BitStream stream, ref HalfVector3 vec)
    {
      stream.Serialize(ref vec.X);
      stream.Serialize(ref vec.Y);
      stream.Serialize(ref vec.Z);
    }

    public static void Serialize(this BitStream stream, ref Vector3D vec)
    {
      stream.Serialize(ref vec.X);
      stream.Serialize(ref vec.Y);
      stream.Serialize(ref vec.Z);
    }

    public static void Serialize(this BitStream stream, ref Vector4D vec)
    {
      stream.Serialize(ref vec.X);
      stream.Serialize(ref vec.Y);
      stream.Serialize(ref vec.Z);
      stream.Serialize(ref vec.W);
    }

    public static void Serialize(this BitStream stream, ref Vector3I vec)
    {
      stream.Serialize(ref vec.X);
      stream.Serialize(ref vec.Y);
      stream.Serialize(ref vec.Z);
    }

    public static void SerializeVariant(this BitStream stream, ref Vector3I vec)
    {
      stream.SerializeVariant(ref vec.X);
      stream.SerializeVariant(ref vec.Y);
      stream.SerializeVariant(ref vec.Z);
    }

    public static void Write(this BitStream stream, HalfVector4 vec) => stream.WriteUInt64(vec.PackedValue);

    public static void Write(this BitStream stream, HalfVector3 vec)
    {
      stream.WriteUInt16(vec.X);
      stream.WriteUInt16(vec.Y);
      stream.WriteUInt16(vec.Z);
    }

    public static void Write(this BitStream stream, Vector3 vec)
    {
      stream.WriteFloat(vec.X);
      stream.WriteFloat(vec.Y);
      stream.WriteFloat(vec.Z);
    }

    public static void Write(this BitStream stream, Vector3D vec)
    {
      stream.WriteDouble(vec.X);
      stream.WriteDouble(vec.Y);
      stream.WriteDouble(vec.Z);
    }

    public static void Write(this BitStream stream, Vector4 vec)
    {
      stream.WriteFloat(vec.X);
      stream.WriteFloat(vec.Y);
      stream.WriteFloat(vec.Z);
      stream.WriteFloat(vec.W);
    }

    public static void Write(this BitStream stream, Vector4D vec)
    {
      stream.WriteDouble(vec.X);
      stream.WriteDouble(vec.Y);
      stream.WriteDouble(vec.Z);
      stream.WriteDouble(vec.W);
    }

    public static void Write(this BitStream stream, Vector3I vec)
    {
      stream.WriteInt32(vec.X);
      stream.WriteInt32(vec.Y);
      stream.WriteInt32(vec.Z);
    }

    public static void WriteNormalizedSignedVector3(
      this BitStream stream,
      Vector3 vec,
      int bitCount)
    {
      vec = Vector3.Clamp(vec, Vector3.MinusOne, Vector3.One);
      stream.WriteNormalizedSignedFloat(vec.X, bitCount);
      stream.WriteNormalizedSignedFloat(vec.Y, bitCount);
      stream.WriteNormalizedSignedFloat(vec.Z, bitCount);
    }

    public static void WriteVariant(this BitStream stream, Vector3I vec)
    {
      stream.WriteVariantSigned(vec.X);
      stream.WriteVariantSigned(vec.Y);
      stream.WriteVariantSigned(vec.Z);
    }

    public static HalfVector4 ReadHalfVector4(this BitStream stream)
    {
      HalfVector4 halfVector4;
      halfVector4.PackedValue = stream.ReadUInt64();
      return halfVector4;
    }

    public static HalfVector3 ReadHalfVector3(this BitStream stream)
    {
      HalfVector3 halfVector3;
      halfVector3.X = stream.ReadUInt16();
      halfVector3.Y = stream.ReadUInt16();
      halfVector3.Z = stream.ReadUInt16();
      return halfVector3;
    }

    public static Vector3 ReadNormalizedSignedVector3(this BitStream stream, int bitCount)
    {
      Vector3 vector3;
      vector3.X = stream.ReadNormalizedSignedFloat(bitCount);
      vector3.Y = stream.ReadNormalizedSignedFloat(bitCount);
      vector3.Z = stream.ReadNormalizedSignedFloat(bitCount);
      return vector3;
    }

    public static Vector3 ReadVector3(this BitStream stream)
    {
      Vector3 vector3;
      vector3.X = stream.ReadFloat();
      vector3.Y = stream.ReadFloat();
      vector3.Z = stream.ReadFloat();
      return vector3;
    }

    public static Vector3D ReadVector3D(this BitStream stream)
    {
      Vector3D vector3D;
      vector3D.X = stream.ReadDouble();
      vector3D.Y = stream.ReadDouble();
      vector3D.Z = stream.ReadDouble();
      return vector3D;
    }

    public static Vector4 ReadVector4(this BitStream stream)
    {
      Vector4 vector4;
      vector4.X = stream.ReadFloat();
      vector4.Y = stream.ReadFloat();
      vector4.Z = stream.ReadFloat();
      vector4.W = stream.ReadFloat();
      return vector4;
    }

    public static Vector4D ReadVector4D(this BitStream stream)
    {
      Vector4D vector4D;
      vector4D.X = stream.ReadDouble();
      vector4D.Y = stream.ReadDouble();
      vector4D.Z = stream.ReadDouble();
      vector4D.W = stream.ReadDouble();
      return vector4D;
    }

    public static Vector3I ReadVector3I(this BitStream stream)
    {
      Vector3I vector3I;
      vector3I.X = stream.ReadInt32();
      vector3I.Y = stream.ReadInt32();
      vector3I.Z = stream.ReadInt32();
      return vector3I;
    }

    public static Vector3I ReadVector3IVariant(this BitStream stream)
    {
      Vector3I vector3I;
      vector3I.X = stream.ReadInt32Variant();
      vector3I.Y = stream.ReadInt32Variant();
      vector3I.Z = stream.ReadInt32Variant();
      return vector3I;
    }

    public static void SerializePositionOrientation(this BitStream stream, ref Matrix m)
    {
      if (stream.Writing)
      {
        Vector3 translation = m.Translation;
        Quaternion result;
        Quaternion.CreateFromRotationMatrix(ref m, out result);
        stream.Serialize(ref translation);
        stream.SerializeNorm(ref result);
      }
      else
      {
        Vector3 vec = new Vector3();
        Quaternion quaternion = new Quaternion();
        stream.Serialize(ref vec);
        stream.SerializeNorm(ref quaternion);
        Matrix.CreateFromQuaternion(ref quaternion, out m);
        m.Translation = vec;
      }
    }

    public static void SerializePositionOrientation(this BitStream stream, ref MatrixD m)
    {
      if (stream.Writing)
      {
        Vector3D translation = m.Translation;
        Quaternion result;
        Quaternion.CreateFromRotationMatrix(ref m, out result);
        stream.Serialize(ref translation);
        stream.SerializeNorm(ref result);
      }
      else
      {
        Vector3D vec = new Vector3D();
        Quaternion quaternion = new Quaternion();
        stream.Serialize(ref vec);
        stream.SerializeNorm(ref quaternion);
        MatrixD.CreateFromQuaternion(ref quaternion, out m);
        m.Translation = vec;
      }
    }

    public static unsafe void Serialize(this BitStream stream, ref MyBlockOrientation orientation)
    {
      MyBlockOrientation blockOrientation = orientation;
      stream.SerializeMemory((void*) &blockOrientation, (long) (sizeof (MyBlockOrientation) * 8));
      orientation = blockOrientation;
    }

    public static void SerializeList(this BitStream stream, ref List<Vector3D> list) => stream.SerializeList<Vector3D>(ref list, (BitStreamExtensions.SerializeCallback<Vector3D>) ((BitStream bs, ref Vector3D vec) => bs.Serialize(ref vec)));

    public static void Serialize(this BitStream stream, ref BoundingBox bb)
    {
      stream.Serialize(ref bb.Min);
      stream.Serialize(ref bb.Max);
    }

    public static void Serialize(this BitStream stream, ref BoundingBoxD bb)
    {
      stream.Serialize(ref bb.Min);
      stream.Serialize(ref bb.Max);
    }
  }
}
