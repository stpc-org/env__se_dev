// Decompiled with JetBrains decompiler
// Type: VRage.BitCompression.CompressedQuaternion
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using VRage.Library.Collections;
using VRageMath;

namespace VRage.BitCompression
{
  public static class CompressedQuaternion
  {
    private const float MIN_QF_LENGTH = -0.7071065f;
    private const float MAX_QF_LENGTH = 0.7071065f;
    private const int QF_BITS = 9;
    private const int QF_VALUE = 511;
    private const float QF_SCALE = 511f;
    private const float QF_SCALE_INV = 0.001956947f;

    public static void Write(BitStream stream, Quaternion value)
    {
      value.Normalize();
      int largestIndex = value.FindLargestIndex();
      if ((double) value.GetComponent(largestIndex) < 0.0)
        value = -value;
      stream.WriteInt32(largestIndex, 2);
      for (int index = 0; index < 4; ++index)
      {
        if (index != largestIndex)
        {
          uint num = (uint) Math.Floor(((double) value.GetComponent(index) - -0.707106530666351) / 1.4142130613327 * 511.0 + 0.5);
          stream.WriteUInt32(num, 9);
        }
      }
    }

    public static Quaternion Read(BitStream stream)
    {
      Quaternion identity = Quaternion.Identity;
      int index1 = stream.ReadInt32(2);
      float num1 = 0.0f;
      for (int index2 = 0; index2 < 4; ++index2)
      {
        if (index2 != index1)
        {
          float num2 = (float) ((double) stream.ReadInt32(9) * (1.0 / 511.0) * 1.4142130613327 - 0.707106530666351);
          identity.SetComponent(index2, num2);
          num1 += num2 * num2;
        }
      }
      identity.SetComponent(index1, (float) Math.Sqrt(1.0 - (double) num1));
      identity.Normalize();
      return identity;
    }

    public static bool CompressedQuaternionUnitTest()
    {
      BitStream stream = new BitStream();
      stream.ResetWrite();
      Quaternion identity = Quaternion.Identity;
      stream.WriteQuaternionNormCompressed(identity);
      stream.ResetRead();
      Quaternion quaternion1 = stream.ReadQuaternionNormCompressed();
      bool flag1 = !identity.Equals(quaternion1, 1f / 511f);
      stream.ResetWrite();
      Quaternion fromAxisAngle1 = Quaternion.CreateFromAxisAngle(Vector3.Forward, 1.047198f);
      stream.WriteQuaternionNormCompressed(fromAxisAngle1);
      stream.ResetRead();
      Quaternion quaternion2 = stream.ReadQuaternionNormCompressed();
      bool flag2 = flag1 | !fromAxisAngle1.Equals(quaternion2, 1f / 511f);
      stream.ResetWrite();
      Vector3 axis = new Vector3(1f, -1f, 3f);
      double num = (double) axis.Normalize();
      Quaternion fromAxisAngle2 = Quaternion.CreateFromAxisAngle(axis, 1.047198f);
      stream.WriteQuaternionNormCompressed(fromAxisAngle2);
      stream.ResetRead();
      Quaternion quaternion3 = stream.ReadQuaternionNormCompressed();
      return flag2 | !fromAxisAngle2.Equals(quaternion3, 1f / 511f);
    }
  }
}
