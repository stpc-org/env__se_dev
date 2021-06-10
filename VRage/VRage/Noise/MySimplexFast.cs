// Decompiled with JetBrains decompiler
// Type: VRage.Noise.MySimplexFast
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRageMath;

namespace VRage.Noise
{
  public class MySimplexFast : MyModuleFast
  {
    private static MySimplexFast.Grad[] grad3 = new MySimplexFast.Grad[12]
    {
      new MySimplexFast.Grad(1.0, 1.0, 0.0),
      new MySimplexFast.Grad(-1.0, 1.0, 0.0),
      new MySimplexFast.Grad(1.0, -1.0, 0.0),
      new MySimplexFast.Grad(-1.0, -1.0, 0.0),
      new MySimplexFast.Grad(1.0, 0.0, 1.0),
      new MySimplexFast.Grad(-1.0, 0.0, 1.0),
      new MySimplexFast.Grad(1.0, 0.0, -1.0),
      new MySimplexFast.Grad(-1.0, 0.0, -1.0),
      new MySimplexFast.Grad(0.0, 1.0, 1.0),
      new MySimplexFast.Grad(0.0, -1.0, 1.0),
      new MySimplexFast.Grad(0.0, 1.0, -1.0),
      new MySimplexFast.Grad(0.0, -1.0, -1.0)
    };
    private int m_seedSimplex;
    private byte[] m_permSimplex = new byte[512];
    private byte[] m_gradSimplex = new byte[512];

    public override int Seed
    {
      get => this.m_seedSimplex;
      set
      {
        this.m_seedSimplex = value;
        MyRNG myRng = new MyRNG(this.m_seedSimplex);
        for (int index = 0; index < 256; ++index)
        {
          this.m_permSimplex[index] = (byte) myRng.NextIntRange(0.0f, (float) byte.MaxValue);
          this.m_permSimplex[256 + index] = this.m_permSimplex[index];
          this.m_gradSimplex[index] = (byte) ((uint) this.m_permSimplex[index] % 12U);
          this.m_gradSimplex[256 + index] = this.m_gradSimplex[index];
        }
      }
    }

    public double Frequency { get; set; }

    public MySimplexFast(int seed = 1, double frequency = 1.0)
    {
      this.Seed = seed;
      this.Frequency = frequency;
    }

    public override double GetValue(double x)
    {
      x *= this.Frequency;
      int num1 = MathHelper.Floor(x);
      double x1 = x - (double) num1;
      double x2 = x1 - 1.0;
      double num2 = 1.0 - x1 * x1;
      double num3 = 1.0 - x2 * x2;
      double num4 = num2 * num2;
      double num5 = num3 * num3;
      return 0.395 * (num4 * num4 * this.Dot(MySimplexFast.grad3[(int) this.m_gradSimplex[num1 & (int) byte.MaxValue]], x1) + num5 * num5 * this.Dot(MySimplexFast.grad3[(int) this.m_gradSimplex[num1 + 1 & (int) byte.MaxValue]], x2));
    }

    public override double GetValue(double x, double y)
    {
      x *= this.Frequency;
      y *= this.Frequency;
      double num1 = (x + y) * 0.366025403784439;
      int num2 = MathHelper.Floor(x + num1);
      int num3 = MathHelper.Floor(y + num1);
      double num4 = (double) (num2 + num3) * 0.211324865405187;
      double x1 = x - (double) num2 + num4;
      double y1 = y - (double) num3 + num4;
      int num5;
      int num6;
      if (x1 > y1)
      {
        num5 = 1;
        num6 = 0;
      }
      else
      {
        num5 = 0;
        num6 = 1;
      }
      double x2 = x1 - (double) num5 + 0.211324865405187;
      double y2 = y1 - (double) num6 + 0.211324865405187;
      double x3 = x1 - 1.0 + 0.211324865405187 + 0.211324865405187;
      double y3 = y1 - 1.0 + 0.211324865405187 + 0.211324865405187;
      int num7 = num2 & (int) byte.MaxValue;
      int index = num3 & (int) byte.MaxValue;
      double num8 = 0.5 - x1 * x1 - y1 * y1;
      double num9 = 0.5 - x2 * x2 - y2 * y2;
      double num10 = 0.5 - x3 * x3 - y3 * y3;
      double num11;
      if (num8 < 0.0)
      {
        num11 = 0.0;
      }
      else
      {
        double num12 = num8 * num8;
        num11 = num12 * num12 * this.Dot(MySimplexFast.grad3[(int) this.m_gradSimplex[num7 + (int) this.m_permSimplex[index] & (int) byte.MaxValue]], x1, y1);
      }
      double num13;
      if (num9 < 0.0)
      {
        num13 = 0.0;
      }
      else
      {
        double num12 = num9 * num9;
        num13 = num12 * num12 * this.Dot(MySimplexFast.grad3[(int) this.m_gradSimplex[num7 + num5 + (int) this.m_permSimplex[index + num6 & (int) byte.MaxValue] & (int) byte.MaxValue]], x2, y2);
      }
      double num14;
      if (num10 < 0.0)
      {
        num14 = 0.0;
      }
      else
      {
        double num12 = num10 * num10;
        num14 = num12 * num12 * this.Dot(MySimplexFast.grad3[(int) this.m_gradSimplex[num7 + 1 + (int) this.m_permSimplex[index + 1 & (int) byte.MaxValue] & (int) byte.MaxValue]], x3, y3);
      }
      return 70.0 * (num11 + num13 + num14);
    }

    public override double GetValue(double x, double y, double z)
    {
      x *= this.Frequency;
      y *= this.Frequency;
      z *= this.Frequency;
      double num1 = (x + y + z) * (1.0 / 3.0);
      int num2 = MathHelper.Floor(x + num1);
      int num3 = MathHelper.Floor(y + num1);
      int num4 = MathHelper.Floor(z + num1);
      double num5 = (double) (num2 + num3 + num4) * (1.0 / 6.0);
      double x1 = x - (double) num2 + num5;
      double y1 = y - (double) num3 + num5;
      double z1 = z - (double) num4 + num5;
      int num6;
      int num7;
      int num8;
      int num9;
      int num10;
      int num11;
      if (x1 >= y1)
      {
        if (y1 >= z1)
        {
          num6 = 1;
          num7 = 0;
          num8 = 0;
          num9 = 1;
          num10 = 1;
          num11 = 0;
        }
        else if (x1 >= z1)
        {
          num6 = 1;
          num7 = 0;
          num8 = 0;
          num9 = 1;
          num10 = 0;
          num11 = 1;
        }
        else
        {
          num6 = 0;
          num7 = 0;
          num8 = 1;
          num9 = 1;
          num10 = 0;
          num11 = 1;
        }
      }
      else if (y1 < z1)
      {
        num6 = 0;
        num7 = 0;
        num8 = 1;
        num9 = 0;
        num10 = 1;
        num11 = 1;
      }
      else if (x1 < z1)
      {
        num6 = 0;
        num7 = 1;
        num8 = 0;
        num9 = 0;
        num10 = 1;
        num11 = 1;
      }
      else
      {
        num6 = 0;
        num7 = 1;
        num8 = 0;
        num9 = 1;
        num10 = 1;
        num11 = 0;
      }
      double x2 = x1 - (double) num6 + 1.0 / 6.0;
      double y2 = y1 - (double) num7 + 1.0 / 6.0;
      double z2 = z1 - (double) num8 + 1.0 / 6.0;
      double x3 = x1 - (double) num9 + 1.0 / 3.0;
      double y3 = y1 - (double) num10 + 1.0 / 3.0;
      double z3 = z1 - (double) num11 + 1.0 / 3.0;
      double x4 = x1 - 1.0 + 0.5;
      double y4 = y1 - 1.0 + 0.5;
      double z4 = z1 - 1.0 + 0.5;
      int num12 = num2 & (int) byte.MaxValue;
      int num13 = num3 & (int) byte.MaxValue;
      int index = num4 & (int) byte.MaxValue;
      double num14 = 0.6 - x1 * x1 - y1 * y1 - z1 * z1;
      double num15 = 0.6 - x2 * x2 - y2 * y2 - z2 * z2;
      double num16 = 0.6 - x3 * x3 - y3 * y3 - z3 * z3;
      double num17 = 0.6 - x4 * x4 - y4 * y4 - z4 * z4;
      double num18;
      if (num14 < 0.0)
      {
        num18 = 0.0;
      }
      else
      {
        double num19 = num14 * num14;
        num18 = num19 * num19 * this.Dot(MySimplexFast.grad3[(int) this.m_gradSimplex[num12 + (int) this.m_permSimplex[num13 + (int) this.m_permSimplex[index]]]], x1, y1, z1);
      }
      double num20;
      if (num15 < 0.0)
      {
        num20 = 0.0;
      }
      else
      {
        double num19 = num15 * num15;
        num20 = num19 * num19 * this.Dot(MySimplexFast.grad3[(int) this.m_gradSimplex[num12 + num6 + (int) this.m_permSimplex[num13 + num7 + (int) this.m_permSimplex[index + num8]]]], x2, y2, z2);
      }
      double num21;
      if (num16 < 0.0)
      {
        num21 = 0.0;
      }
      else
      {
        double num19 = num16 * num16;
        num21 = num19 * num19 * this.Dot(MySimplexFast.grad3[(int) this.m_gradSimplex[num12 + num9 + (int) this.m_permSimplex[num13 + num10 + (int) this.m_permSimplex[index + num11]]]], x3, y3, z3);
      }
      double num22;
      if (num17 < 0.0)
      {
        num22 = 0.0;
      }
      else
      {
        double num19 = num17 * num17;
        num22 = num19 * num19 * this.Dot(MySimplexFast.grad3[(int) this.m_gradSimplex[num12 + 1 + (int) this.m_permSimplex[num13 + 1 + (int) this.m_permSimplex[index + 1]]]], x4, y4, z4);
      }
      return 32.0 * (num18 + num20 + num21 + num22);
    }

    private double Dot(MySimplexFast.Grad g, double x) => g.x * x;

    private double Dot(MySimplexFast.Grad g, double x, double y) => g.x * x + g.y * y;

    private double Dot(MySimplexFast.Grad g, double x, double y, double z) => g.x * x + g.y * y + g.z * z;

    private class Grad
    {
      public double x;
      public double y;
      public double z;

      public Grad(double x, double y, double z)
      {
        this.x = x;
        this.y = y;
        this.z = z;
      }
    }
  }
}
