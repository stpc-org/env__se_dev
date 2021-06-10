// Decompiled with JetBrains decompiler
// Type: VRage.Import.VF_Packer
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using VRageMath;
using VRageMath.PackedVector;

namespace VRage.Import
{
  public class VF_Packer
  {
    public static short PackAmbientAndAlpha(float ambient, byte alpha) => (short) ((int) (short) ((double) ambient * 8191.0) + (int) (short) ((int) alpha << 13));

    public static float UnpackAmbient(float packed) => (float) ((double) packed % 8192.0 / 8191.0);

    public static float UnpackAmbient(short packed) => (float) ((double) packed % 8192.0 / 8191.0);

    public static byte UnpackAlpha(short packed) => (byte) Math.Abs((int) packed / 8192);

    public static byte UnpackAlpha(float packed) => (byte) Math.Abs(packed / 8192f);

    public static uint PackNormal(Vector3 normal) => VF_Packer.PackNormal(ref normal);

    public static uint PackNormal(ref Vector3 normal)
    {
      Vector3 vector3 = normal;
      vector3.X = (float) (0.5 * ((double) vector3.X + 1.0));
      vector3.Y = (float) (0.5 * ((double) vector3.Y + 1.0));
      int num1 = (int) (ushort) ((double) vector3.X * (double) short.MaxValue);
      uint num2 = (uint) (ushort) ((double) vector3.Y * (double) short.MaxValue);
      int num3 = (int) (ushort) (((double) vector3.Z > 0.0 ? 1U : 0U) << 15);
      return (uint) (num1 | num3 | (int) num2 << 16);
    }

    public static uint PackTangentSign(ref Vector4 tangent)
    {
      Vector4 vector4 = tangent;
      vector4.X = (float) (0.5 * ((double) vector4.X + 1.0));
      vector4.Y = (float) (0.5 * ((double) vector4.Y + 1.0));
      int num1 = (int) (ushort) ((double) vector4.X * (double) short.MaxValue);
      uint num2 = (uint) (ushort) ((double) vector4.Y * (double) short.MaxValue);
      int num3 = (int) (ushort) (((double) vector4.Z > 0.0 ? 1U : 0U) << 15);
      return (uint) (num1 | num3 | ((int) num2 | (int) (ushort) (((double) vector4.W > 0.0 ? 1 : 0) << 15)) << 16);
    }

    public static Vector4 UnpackTangentSign(ref Byte4 packedTangent)
    {
      Vector4 vector4 = packedTangent.ToVector4();
      double num1 = (double) vector4.Y > 127.5 ? 1.0 : -1.0;
      float w = (double) vector4.W > 127.5 ? 1f : -1f;
      if (num1 > 0.0)
        vector4.Y -= 128f;
      if ((double) w > 0.0)
        vector4.W -= 128f;
      float num2 = vector4.X + 256f * vector4.Y;
      float num3 = vector4.Z + 256f * vector4.W;
      float num4 = num2 / (float) short.MaxValue;
      float num5 = num3 / (float) short.MaxValue;
      float x = (float) (2.0 * (double) num4 - 1.0);
      float y = (float) (2.0 * (double) num5 - 1.0);
      float z = (float) num1 * (float) Math.Sqrt((double) Math.Max(0.0f, (float) (1.0 - (double) x * (double) x - (double) y * (double) y)));
      return new Vector4(x, y, z, w);
    }

    public static Byte4 PackNormalB4(ref Vector3 normal)
    {
      uint num = VF_Packer.PackNormal(ref normal);
      return new Byte4() { PackedValue = num };
    }

    public static Byte4 PackTangentSignB4(ref Vector4 tangentW)
    {
      uint num = VF_Packer.PackTangentSign(ref tangentW);
      return new Byte4() { PackedValue = num };
    }

    public static Vector3 UnpackNormal(ref uint packedNormal) => VF_Packer.UnpackNormal(ref new Byte4()
    {
      PackedValue = packedNormal
    });

    public static Vector3 UnpackNormal(uint packedNormal) => VF_Packer.UnpackNormal(ref new Byte4()
    {
      PackedValue = packedNormal
    });

    public static Vector3 UnpackNormal(Byte4 packedNormal) => VF_Packer.UnpackNormal(ref packedNormal);

    public static Vector3 UnpackNormal(ref Byte4 packedNormal)
    {
      Vector4 vector4 = packedNormal.ToVector4();
      double num1 = (double) vector4.Y > 127.5 ? 1.0 : -1.0;
      if (num1 > 0.0)
        vector4.Y -= 128f;
      float num2 = vector4.X + 256f * vector4.Y;
      float num3 = vector4.Z + 256f * vector4.W;
      float num4 = num2 / (float) short.MaxValue;
      float num5 = num3 / (float) short.MaxValue;
      float x = (float) (2.0 * (double) num4 - 1.0);
      float y = (float) (2.0 * (double) num5 - 1.0);
      float z = (float) num1 * (float) Math.Sqrt((double) Math.Max(0.0f, (float) (1.0 - (double) x * (double) x - (double) y * (double) y)));
      return new Vector3(x, y, z);
    }

    public static HalfVector4 PackPosition(Vector3 position)
    {
      Vector3 position1 = position;
      return PositionPacker.PackPosition(ref position1);
    }

    public static HalfVector4 PackPosition(ref Vector3 position) => PositionPacker.PackPosition(ref position);

    public static Vector3 UnpackPosition(ref HalfVector4 position) => PositionPacker.UnpackPosition(ref position);

    public static Vector3 UnpackPosition(HalfVector4 position) => PositionPacker.UnpackPosition(ref position);

    public static Vector3 RepackModelPosition(ref Vector3 position)
    {
      HalfVector4 position1 = VF_Packer.PackPosition(ref position);
      return VF_Packer.UnpackPosition(ref position1);
    }
  }
}
