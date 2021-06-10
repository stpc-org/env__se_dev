// Decompiled with JetBrains decompiler
// Type: System.BitCompressionExtensions
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRage.BitCompression;
using VRage.Library.Collections;
using VRageMath;

namespace System
{
  public static class BitCompressionExtensions
  {
    public static Quaternion ReadQuaternionNormCompressed(this BitStream stream) => CompressedQuaternion.Read(stream);

    public static void WriteQuaternionNormCompressed(this BitStream stream, Quaternion value) => CompressedQuaternion.Write(stream, value);

    public static void SerializeNormCompressed(this BitStream stream, ref Quaternion quat)
    {
      if (stream.Reading)
        quat = stream.ReadQuaternionNormCompressed();
      else
        stream.WriteQuaternionNormCompressed(quat);
    }

    public static Quaternion ReadQuaternionNormCompressedIdentity(this BitStream stream) => stream.ReadBool() ? Quaternion.Identity : CompressedQuaternion.Read(stream);

    public static void WriteQuaternionNormCompressedIdentity(
      this BitStream stream,
      Quaternion value)
    {
      bool flag = value == Quaternion.Identity;
      stream.WriteBool(flag);
      if (flag)
        return;
      CompressedQuaternion.Write(stream, value);
    }

    public static void SerializeNormCompressedIdentity(this BitStream stream, ref Quaternion quat)
    {
      if (stream.Reading)
        quat = stream.ReadQuaternionNormCompressedIdentity();
      else
        stream.WriteQuaternionNormCompressedIdentity(quat);
    }
  }
}
