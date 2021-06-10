// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.StringExtensions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Text;
using VRageMath;

namespace Sandbox.Game
{
  public static class StringExtensions
  {
    public static int Get7bitEncodedSize(this string self)
    {
      int byteCount = Encoding.UTF8.GetByteCount(self);
      return byteCount + (MathHelper.Log2Floor(byteCount) + 6) / 7;
    }
  }
}
