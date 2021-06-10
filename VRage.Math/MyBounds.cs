// Decompiled with JetBrains decompiler
// Type: VRageMath.MyBounds
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

namespace VRageMath
{
  public struct MyBounds
  {
    public float Min;
    public float Max;
    public float Default;

    public MyBounds(float min, float max, float def)
    {
      this.Min = min;
      this.Max = max;
      this.Default = def;
    }

    public float Normalize(float value) => (float) (((double) value - (double) this.Min) / ((double) this.Max - (double) this.Min));

    public float Clamp(float value) => MathHelper.Clamp(value, this.Min, this.Max);

    public override string ToString() => string.Format("Min={0}, Max={1}, Default={2}", (object) this.Min, (object) this.Max, (object) this.Default);
  }
}
