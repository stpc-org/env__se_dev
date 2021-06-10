// Decompiled with JetBrains decompiler
// Type: BulletXNA.BulletCollision.BoxCollision
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace BulletXNA.BulletCollision
{
  internal static class BoxCollision
  {
    public static bool BT_GREATER(float x, float y) => (double) Math.Abs(x) > (double) y;

    public static float BT_MAX(float a, float b) => Math.Max(a, b);

    public static float BT_MIN(float a, float b) => Math.Min(a, b);
  }
}
