// Decompiled with JetBrains decompiler
// Type: VRageMath.Vector2B
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

namespace VRageMath
{
  public struct Vector2B
  {
    private byte X;
    private byte Y;

    public Vector2B(byte x, byte y)
    {
      this.X = x;
      this.Y = y;
    }

    public static Vector2I operator *(Vector2B op1, Vector2I op2) => new Vector2I((int) op1.X * op2.X, (int) op1.Y * op2.Y);

    public static Vector2I operator *(Vector2B op1, int op2) => new Vector2I((int) op1.X * op2, (int) op1.Y * op2);
  }
}
