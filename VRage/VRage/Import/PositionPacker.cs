// Decompiled with JetBrains decompiler
// Type: VRage.Import.PositionPacker
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using VRageMath;
using VRageMath.PackedVector;

namespace VRage.Import
{
  public static class PositionPacker
  {
    public static HalfVector4 PackPosition(ref Vector3 position)
    {
      float w = Math.Min((float) Math.Floor((double) Math.Max(Math.Max(Math.Abs(position.X), Math.Abs(position.Y)), Math.Abs(position.Z))), 2048f);
      float num;
      if ((double) w > 0.0)
        num = 1f / w;
      else
        w = num = 1f;
      return new HalfVector4(num * position.X, num * position.Y, num * position.Z, w);
    }

    public static Vector3 UnpackPosition(ref HalfVector4 position)
    {
      Vector4 vector4 = position.ToVector4();
      return vector4.W * new Vector3(vector4.X, vector4.Y, vector4.Z);
    }
  }
}
