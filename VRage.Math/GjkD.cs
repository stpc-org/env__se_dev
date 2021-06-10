// Decompiled with JetBrains decompiler
// Type: VRageMath.GjkD
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
  [Serializable]
  internal class GjkD
  {
    private static int[] BitsToIndices = new int[16]
    {
      0,
      1,
      2,
      17,
      3,
      25,
      26,
      209,
      4,
      33,
      34,
      273,
      35,
      281,
      282,
      2257
    };
    private Vector3D closestPoint;
    private Vector3D[] y;
    private double[] yLengthSq;
    private Vector3D[][] edges;
    private double[][] edgeLengthSq;
    private double[][] det;
    private int simplexBits;
    private double maxLengthSq;

    public bool FullSimplex => this.simplexBits == 15;

    public double MaxLengthSquared => this.maxLengthSq;

    public Vector3D ClosestPoint => this.closestPoint;

    public GjkD()
    {
      this.y = new Vector3D[4];
      this.yLengthSq = new double[4];
      this.edges = new Vector3D[4][]
      {
        new Vector3D[4],
        new Vector3D[4],
        new Vector3D[4],
        new Vector3D[4]
      };
      this.edgeLengthSq = new double[4][]
      {
        new double[4],
        new double[4],
        new double[4],
        new double[4]
      };
      this.det = new double[16][];
      for (int index = 0; index < 16; ++index)
        this.det[index] = new double[4];
    }

    public void Reset()
    {
      this.simplexBits = 0;
      this.maxLengthSq = 0.0;
    }

    public bool AddSupportPoint(ref Vector3D newPoint)
    {
      int index1 = (GjkD.BitsToIndices[this.simplexBits ^ 15] & 7) - 1;
      this.y[index1] = newPoint;
      this.yLengthSq[index1] = newPoint.LengthSquared();
      for (int bitsToIndex = GjkD.BitsToIndices[this.simplexBits]; bitsToIndex != 0; bitsToIndex >>= 3)
      {
        int index2 = (bitsToIndex & 7) - 1;
        Vector3D vector3D = this.y[index2] - newPoint;
        this.edges[index2][index1] = vector3D;
        this.edges[index1][index2] = -vector3D;
        this.edgeLengthSq[index1][index2] = this.edgeLengthSq[index2][index1] = vector3D.LengthSquared();
      }
      this.UpdateDeterminant(index1);
      return this.UpdateSimplex(index1);
    }

    private static double Dot(ref Vector3D a, ref Vector3D b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;

    private void UpdateDeterminant(int xmIdx)
    {
      int index1 = 1 << xmIdx;
      this.det[index1][xmIdx] = 1.0;
      int bitsToIndex = GjkD.BitsToIndices[this.simplexBits];
      int num1 = bitsToIndex;
      int num2 = 0;
      while (num1 != 0)
      {
        int index2 = (num1 & 7) - 1;
        int num3 = 1 << index2;
        int index3 = num3 | index1;
        this.det[index3][index2] = GjkD.Dot(ref this.edges[xmIdx][index2], ref this.y[xmIdx]);
        this.det[index3][xmIdx] = GjkD.Dot(ref this.edges[index2][xmIdx], ref this.y[index2]);
        int num4 = bitsToIndex;
        for (int index4 = 0; index4 < num2; ++index4)
        {
          int index5 = (num4 & 7) - 1;
          int num5 = 1 << index5;
          int index6 = index3 | num5;
          int index7 = this.edgeLengthSq[index2][index5] < this.edgeLengthSq[xmIdx][index5] ? index2 : xmIdx;
          this.det[index6][index5] = this.det[index3][index2] * GjkD.Dot(ref this.edges[index7][index5], ref this.y[index2]) + this.det[index3][xmIdx] * GjkD.Dot(ref this.edges[index7][index5], ref this.y[xmIdx]);
          int index8 = this.edgeLengthSq[index5][index2] < this.edgeLengthSq[xmIdx][index2] ? index5 : xmIdx;
          this.det[index6][index2] = this.det[num5 | index1][index5] * GjkD.Dot(ref this.edges[index8][index2], ref this.y[index5]) + this.det[num5 | index1][xmIdx] * GjkD.Dot(ref this.edges[index8][index2], ref this.y[xmIdx]);
          int index9 = this.edgeLengthSq[index2][xmIdx] < this.edgeLengthSq[index5][xmIdx] ? index2 : index5;
          this.det[index6][xmIdx] = this.det[num3 | num5][index5] * GjkD.Dot(ref this.edges[index9][xmIdx], ref this.y[index5]) + this.det[num3 | num5][index2] * GjkD.Dot(ref this.edges[index9][xmIdx], ref this.y[index2]);
          num4 >>= 3;
        }
        num1 >>= 3;
        ++num2;
      }
      if ((this.simplexBits | index1) != 15)
        return;
      int index10 = this.edgeLengthSq[1][0] < this.edgeLengthSq[2][0] ? (this.edgeLengthSq[1][0] < this.edgeLengthSq[3][0] ? 1 : 3) : (this.edgeLengthSq[2][0] < this.edgeLengthSq[3][0] ? 2 : 3);
      this.det[15][0] = this.det[14][1] * GjkD.Dot(ref this.edges[index10][0], ref this.y[1]) + this.det[14][2] * GjkD.Dot(ref this.edges[index10][0], ref this.y[2]) + this.det[14][3] * GjkD.Dot(ref this.edges[index10][0], ref this.y[3]);
      int index11 = this.edgeLengthSq[0][1] < this.edgeLengthSq[2][1] ? (this.edgeLengthSq[0][1] < this.edgeLengthSq[3][1] ? 0 : 3) : (this.edgeLengthSq[2][1] < this.edgeLengthSq[3][1] ? 2 : 3);
      this.det[15][1] = this.det[13][0] * GjkD.Dot(ref this.edges[index11][1], ref this.y[0]) + this.det[13][2] * GjkD.Dot(ref this.edges[index11][1], ref this.y[2]) + this.det[13][3] * GjkD.Dot(ref this.edges[index11][1], ref this.y[3]);
      int index12 = this.edgeLengthSq[0][2] < this.edgeLengthSq[1][2] ? (this.edgeLengthSq[0][2] < this.edgeLengthSq[3][2] ? 0 : 3) : (this.edgeLengthSq[1][2] < this.edgeLengthSq[3][2] ? 1 : 3);
      this.det[15][2] = this.det[11][0] * GjkD.Dot(ref this.edges[index12][2], ref this.y[0]) + this.det[11][1] * GjkD.Dot(ref this.edges[index12][2], ref this.y[1]) + this.det[11][3] * GjkD.Dot(ref this.edges[index12][2], ref this.y[3]);
      int index13 = this.edgeLengthSq[0][3] < this.edgeLengthSq[1][3] ? (this.edgeLengthSq[0][3] < this.edgeLengthSq[2][3] ? 0 : 2) : (this.edgeLengthSq[1][3] < this.edgeLengthSq[2][3] ? 1 : 2);
      this.det[15][3] = this.det[7][0] * GjkD.Dot(ref this.edges[index13][3], ref this.y[0]) + this.det[7][1] * GjkD.Dot(ref this.edges[index13][3], ref this.y[1]) + this.det[7][2] * GjkD.Dot(ref this.edges[index13][3], ref this.y[2]);
    }

    private bool UpdateSimplex(int newIndex)
    {
      int yBits = this.simplexBits | 1 << newIndex;
      int xBits = 1 << newIndex;
      for (int simplexBits = this.simplexBits; simplexBits != 0; --simplexBits)
      {
        if ((simplexBits & yBits) == simplexBits && this.IsSatisfiesRule(simplexBits | xBits, yBits))
        {
          this.simplexBits = simplexBits | xBits;
          this.closestPoint = this.ComputeClosestPoint();
          return true;
        }
      }
      bool flag = false;
      if (this.IsSatisfiesRule(xBits, yBits))
      {
        this.simplexBits = xBits;
        this.closestPoint = this.y[newIndex];
        this.maxLengthSq = this.yLengthSq[newIndex];
        flag = true;
      }
      return flag;
    }

    private Vector3D ComputeClosestPoint()
    {
      double num1 = 0.0;
      Vector3D zero = Vector3D.Zero;
      this.maxLengthSq = 0.0;
      for (int bitsToIndex = GjkD.BitsToIndices[this.simplexBits]; bitsToIndex != 0; bitsToIndex >>= 3)
      {
        int index = (bitsToIndex & 7) - 1;
        double num2 = this.det[this.simplexBits][index];
        num1 += num2;
        zero += this.y[index] * num2;
        this.maxLengthSq = MathHelper.Max(this.maxLengthSq, this.yLengthSq[index]);
      }
      return zero / num1;
    }

    private bool IsSatisfiesRule(int xBits, int yBits)
    {
      bool flag = true;
      for (int bitsToIndex = GjkD.BitsToIndices[yBits]; bitsToIndex != 0; bitsToIndex >>= 3)
      {
        int index = (bitsToIndex & 7) - 1;
        int num = 1 << index;
        if ((num & xBits) != 0)
        {
          if (this.det[xBits][index] <= 0.0)
          {
            flag = false;
            break;
          }
        }
        else if (this.det[xBits | num][index] > 0.0)
        {
          flag = false;
          break;
        }
      }
      return flag;
    }

    protected class VRageMath_GjkD\u003C\u003EclosestPoint\u003C\u003EAccessor : IMemberAccessor<GjkD, Vector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref GjkD owner, in Vector3D value) => owner.closestPoint = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref GjkD owner, out Vector3D value) => value = owner.closestPoint;
    }

    protected class VRageMath_GjkD\u003C\u003Ey\u003C\u003EAccessor : IMemberAccessor<GjkD, Vector3D[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref GjkD owner, in Vector3D[] value) => owner.y = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref GjkD owner, out Vector3D[] value) => value = owner.y;
    }

    protected class VRageMath_GjkD\u003C\u003EyLengthSq\u003C\u003EAccessor : IMemberAccessor<GjkD, double[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref GjkD owner, in double[] value) => owner.yLengthSq = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref GjkD owner, out double[] value) => value = owner.yLengthSq;
    }

    protected class VRageMath_GjkD\u003C\u003Eedges\u003C\u003EAccessor : IMemberAccessor<GjkD, Vector3D[][]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref GjkD owner, in Vector3D[][] value) => owner.edges = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref GjkD owner, out Vector3D[][] value) => value = owner.edges;
    }

    protected class VRageMath_GjkD\u003C\u003EedgeLengthSq\u003C\u003EAccessor : IMemberAccessor<GjkD, double[][]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref GjkD owner, in double[][] value) => owner.edgeLengthSq = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref GjkD owner, out double[][] value) => value = owner.edgeLengthSq;
    }

    protected class VRageMath_GjkD\u003C\u003Edet\u003C\u003EAccessor : IMemberAccessor<GjkD, double[][]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref GjkD owner, in double[][] value) => owner.det = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref GjkD owner, out double[][] value) => value = owner.det;
    }

    protected class VRageMath_GjkD\u003C\u003EsimplexBits\u003C\u003EAccessor : IMemberAccessor<GjkD, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref GjkD owner, in int value) => owner.simplexBits = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref GjkD owner, out int value) => value = owner.simplexBits;
    }

    protected class VRageMath_GjkD\u003C\u003EmaxLengthSq\u003C\u003EAccessor : IMemberAccessor<GjkD, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref GjkD owner, in double value) => owner.maxLengthSq = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref GjkD owner, out double value) => value = owner.maxLengthSq;
    }
  }
}
