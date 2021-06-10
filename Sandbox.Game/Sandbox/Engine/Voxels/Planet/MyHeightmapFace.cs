// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.Planet.MyHeightmapFace
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using System;
using System.Runtime.CompilerServices;
using VRage;
using VRage.Library;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Voxels.Planet
{
  public class MyHeightmapFace : IMyWrappedCubemapFace, IDisposable
  {
    private static NativeArrayAllocator BufferAllocator = new NativeArrayAllocator(MyPlanet.MemoryTracker.RegisterSubsystem("HeightmapFaces"));
    public static readonly MyHeightmapFace Default = new MyHeightmapFace(MyHeightmapFace.HeightmapNode.HEIGHTMAP_LEAF_SIZE);
    private int m_realResolution;
    private float m_pixelSizeFour;
    private MyHeightmapFace.Box2I m_bounds;
    private NativeArray m_dataBuffer;
    public unsafe ushort* Data;
    public MyHeightmapFace.HeightmapNode Root;
    public MyHeightmapFace.HeightmapLevel[] PruningTree;
    [ThreadStatic]
    private static MyHeightmapFace.SEntry[] m_queryStack;

    public bool IsPersistent { get; private set; }

    public int Resolution { get; set; }

    public int ResolutionMinusOne { get; set; }

    public int RowStride => this.m_realResolution;

    static unsafe MyHeightmapFace()
    {
      MyHeightmapFace.Default.IsPersistent = true;
      Unsafe.InitBlockUnaligned((void*) MyHeightmapFace.Default.Data, (byte) 0, (uint) MyHeightmapFace.Default.m_dataBuffer.Size);
      MyVRage.RegisterExitCallback((Action) (() =>
      {
        MyHeightmapFace.Default.IsPersistent = false;
        MyHeightmapFace.Default.Dispose();
      }));
    }

    public unsafe MyHeightmapFace(int resolution)
    {
      this.m_realResolution = resolution + 2;
      this.Resolution = resolution;
      this.ResolutionMinusOne = this.Resolution - 1;
      this.m_dataBuffer = MyHeightmapFace.BufferAllocator.Allocate(this.m_realResolution * this.m_realResolution * 2);
      this.Data = (ushort*) (void*) this.m_dataBuffer.Ptr;
      this.m_pixelSizeFour = 4f / (float) this.Resolution;
      this.m_bounds = new MyHeightmapFace.Box2I(Vector2I.Zero, new Vector2I(this.Resolution - 1));
    }

    public unsafe void GetValue(int x, int y, out ushort value) => value = this.Data[(y + 1) * this.m_realResolution + (x + 1)];

    public unsafe void Get4Row(int linearOfft, float* values, ushort* map)
    {
      *values = (float) map[linearOfft] * 1.525902E-05f;
      values[1] = (float) map[linearOfft + 1] * 1.525902E-05f;
      values[2] = (float) map[linearOfft + 2] * 1.525902E-05f;
      values[3] = (float) map[linearOfft + 3] * 1.525902E-05f;
    }

    public unsafe void Get4Row(int linearOfft, float* values)
    {
      *values = (float) this.Data[linearOfft] * 1.525902E-05f;
      values[1] = (float) this.Data[linearOfft + 1] * 1.525902E-05f;
      values[2] = (float) this.Data[linearOfft + 2] * 1.525902E-05f;
      values[3] = (float) this.Data[linearOfft + 3] * 1.525902E-05f;
    }

    public unsafe void GetHermiteSliceRow(int linearOfft, float* values)
    {
      *values = (float) this.Data[linearOfft + 1] * 1.525902E-05f;
      values[1] = (float) ((int) this.Data[linearOfft + 2] - (int) this.Data[linearOfft]) * 3.051804E-05f;
      values[2] = (float) this.Data[linearOfft + 2] * 1.525902E-05f;
      values[3] = (float) ((int) this.Data[linearOfft + 3] - (int) this.Data[linearOfft + 1]) * 3.051804E-05f;
    }

    public unsafe ushort GetValue(int x, int y)
    {
      if (x < 0)
        x = 0;
      else if (x >= this.Resolution)
        x = this.Resolution - 1;
      if (y < 0)
        y = 0;
      else if (y >= this.Resolution)
        y = this.Resolution - 1;
      return this.Data[(y + 1) * this.m_realResolution + (x + 1)];
    }

    public unsafe ushort this[int x, int y]
    {
      get
      {
        if (x < 0)
          x = 0;
        else if (x >= this.Resolution)
          x = this.Resolution - 1;
        if (y < 0)
          y = 0;
        else if (y >= this.Resolution)
          y = this.Resolution - 1;
        return this.Data[(y + 1) * this.m_realResolution + (x + 1)];
      }
    }

    public unsafe float GetValuef(int x, int y) => (float) this.Data[(y + 1) * this.m_realResolution + (x + 1)] * 1.525902E-05f;

    public unsafe void SetValue(int x, int y, ushort value) => this.Data[(y + 1) * this.m_realResolution + (x + 1)] = value;

    public int GetRowStart(int y) => (y + 1) * this.m_realResolution + 1;

    public void CopyRange(
      Vector2I start,
      Vector2I end,
      MyHeightmapFace other,
      Vector2I oStart,
      Vector2I oEnd)
    {
      Vector2I step1 = MyCubemapHelpers.GetStep(ref start, ref end);
      Vector2I step2 = MyCubemapHelpers.GetStep(ref oStart, ref oEnd);
      ushort num;
      while (start != end)
      {
        other.GetValue(oStart.X, oStart.Y, out num);
        this.SetValue(start.X, start.Y, num);
        start += step1;
        oStart += step2;
      }
      other.GetValue(oStart.X, oStart.Y, out num);
      this.SetValue(start.X, start.Y, num);
    }

    public void CopyRange(
      Vector2I start,
      Vector2I end,
      IMyWrappedCubemapFace other,
      Vector2I oStart,
      Vector2I oEnd)
    {
      this.CopyRange(start, end, other as MyHeightmapFace, oStart, oEnd);
    }

    public void FinishFace(string faceName)
    {
      int num = this.Resolution - 1;
      this.SetValue(-1, -1, (ushort) (((int) this.GetValue(0, 0) + (int) this.GetValue(-1, 0) + (int) this.GetValue(0, -1)) / 3));
      this.SetValue(this.Resolution, -1, (ushort) (((int) this.GetValue(num, 0) + (int) this.GetValue(this.Resolution, 0) + (int) this.GetValue(num, -1)) / 3));
      this.SetValue(-1, this.Resolution, (ushort) (((int) this.GetValue(0, num) + (int) this.GetValue(-1, num) + (int) this.GetValue(0, this.Resolution)) / 3));
      this.SetValue(this.Resolution, this.Resolution, (ushort) (((int) this.GetValue(num, num) + (int) this.GetValue(this.Resolution, num) + (int) this.GetValue(num, this.Resolution)) / 3));
      this.CreatePruningTree(faceName);
    }

    public unsafe void CreatePruningTree(string mapName)
    {
      int length = 0;
      for (int index = this.Resolution / MyHeightmapFace.HeightmapNode.HEIGHTMAP_LEAF_SIZE; index != 1; index /= MyHeightmapFace.HeightmapNode.HEIGHTMAP_BRANCH_FACTOR)
      {
        if (index % MyHeightmapFace.HeightmapNode.HEIGHTMAP_BRANCH_FACTOR != 0)
        {
          MyLog.Default.Error("Cannot build prunning tree for heightmap face {0}!", (object) mapName);
          MyLog.Default.Error("Heightmap resolution must be divisible by {1}, and after that a power of {0}. Failing to achieve so will disable several important optimizations!!", (object) MyHeightmapFace.HeightmapNode.HEIGHTMAP_BRANCH_FACTOR, (object) MyHeightmapFace.HeightmapNode.HEIGHTMAP_LEAF_SIZE);
          return;
        }
        ++length;
      }
      this.PruningTree = new MyHeightmapFace.HeightmapLevel[length];
      int rowStart = this.GetRowStart(0);
      if (length == 0)
      {
        float num1 = float.PositiveInfinity;
        float num2 = float.NegativeInfinity;
        int num3 = rowStart;
        for (int index1 = 0; index1 < MyHeightmapFace.HeightmapNode.HEIGHTMAP_LEAF_SIZE; ++index1)
        {
          for (int index2 = 0; index2 < MyHeightmapFace.HeightmapNode.HEIGHTMAP_LEAF_SIZE; ++index2)
          {
            float num4 = (float) this.Data[num3 + index2] * 1.525902E-05f;
            if ((double) num1 > (double) num4)
              num1 = num4;
            if ((double) num2 < (double) num4)
              num2 = num4;
          }
          num3 += this.m_realResolution;
        }
        this.Root.Max = num2;
        this.Root.Min = num1;
      }
      else
      {
        int heightmapBranchFactor = MyHeightmapFace.HeightmapNode.HEIGHTMAP_BRANCH_FACTOR;
        int num1 = this.Resolution / MyHeightmapFace.HeightmapNode.HEIGHTMAP_LEAF_SIZE;
        this.PruningTree[0].Nodes = new MyHeightmapFace.HeightmapNode[num1 * num1];
        this.PruningTree[0].Res = (uint) num1;
        int num2 = 0;
        MyHeightmapFace.HeightmapNode heightmapNode1;
        for (int index1 = 0; index1 < num1; ++index1)
        {
          int num3 = rowStart;
          for (int index2 = 0; index2 < num1; ++index2)
          {
            float num4 = float.PositiveInfinity;
            float num5 = float.NegativeInfinity;
            int num6 = num3 - this.m_realResolution;
            for (int index3 = -1; index3 <= MyHeightmapFace.HeightmapNode.HEIGHTMAP_LEAF_SIZE; ++index3)
            {
              for (int index4 = -1; index4 <= MyHeightmapFace.HeightmapNode.HEIGHTMAP_LEAF_SIZE; ++index4)
              {
                float num7 = (float) this.Data[num6 + index4] * 1.525902E-05f;
                if ((double) num4 > (double) num7)
                  num4 = num7;
                if ((double) num5 < (double) num7)
                  num5 = num7;
              }
              num6 += this.m_realResolution;
            }
            MyHeightmapFace.HeightmapNode[] nodes = this.PruningTree[0].Nodes;
            int index5 = num2;
            heightmapNode1 = new MyHeightmapFace.HeightmapNode();
            heightmapNode1.Max = num5;
            heightmapNode1.Min = num4;
            MyHeightmapFace.HeightmapNode heightmapNode2 = heightmapNode1;
            nodes[index5] = heightmapNode2;
            ++num2;
            num3 += MyHeightmapFace.HeightmapNode.HEIGHTMAP_LEAF_SIZE;
          }
          rowStart += MyHeightmapFace.HeightmapNode.HEIGHTMAP_LEAF_SIZE * this.m_realResolution;
        }
        int index6 = 0;
        for (int index1 = 1; index1 < length; ++index1)
        {
          int num3 = 0;
          int num4 = num1 / MyHeightmapFace.HeightmapNode.HEIGHTMAP_BRANCH_FACTOR;
          this.PruningTree[index1].Nodes = new MyHeightmapFace.HeightmapNode[num4 * num4];
          this.PruningTree[index1].Res = (uint) num4;
          int num5 = 0;
          for (int index2 = 0; index2 < num4; ++index2)
          {
            int num6 = num3;
            for (int index3 = 0; index3 < num4; ++index3)
            {
              float num7 = float.PositiveInfinity;
              float num8 = float.NegativeInfinity;
              int num9 = num6;
              for (int index4 = 0; index4 < MyHeightmapFace.HeightmapNode.HEIGHTMAP_BRANCH_FACTOR; ++index4)
              {
                for (int index5 = 0; index5 < MyHeightmapFace.HeightmapNode.HEIGHTMAP_BRANCH_FACTOR; ++index5)
                {
                  MyHeightmapFace.HeightmapNode node = this.PruningTree[index6].Nodes[num9 + index5];
                  if ((double) num7 > (double) node.Min)
                    num7 = node.Min;
                  if ((double) num8 < (double) node.Max)
                    num8 = node.Max;
                }
                num9 += num1;
              }
              MyHeightmapFace.HeightmapNode[] nodes = this.PruningTree[index1].Nodes;
              int index7 = num5;
              heightmapNode1 = new MyHeightmapFace.HeightmapNode();
              heightmapNode1.Max = num8;
              heightmapNode1.Min = num7;
              MyHeightmapFace.HeightmapNode heightmapNode2 = heightmapNode1;
              nodes[index7] = heightmapNode2;
              ++num5;
              num6 += MyHeightmapFace.HeightmapNode.HEIGHTMAP_BRANCH_FACTOR;
            }
            num3 += MyHeightmapFace.HeightmapNode.HEIGHTMAP_BRANCH_FACTOR * num1;
          }
          ++index6;
          num1 = num4;
        }
        float num10 = float.PositiveInfinity;
        float num11 = float.NegativeInfinity;
        int num12 = 0;
        for (int index1 = 0; index1 < MyHeightmapFace.HeightmapNode.HEIGHTMAP_BRANCH_FACTOR; ++index1)
        {
          for (int index2 = 0; index2 < MyHeightmapFace.HeightmapNode.HEIGHTMAP_BRANCH_FACTOR; ++index2)
          {
            MyHeightmapFace.HeightmapNode node = this.PruningTree[length - 1].Nodes[num12++];
            if ((double) num10 > (double) node.Min)
              num10 = node.Min;
            if ((double) num11 < (double) node.Max)
              num11 = node.Max;
          }
        }
        this.Root.Max = num11;
        this.Root.Min = num10;
      }
    }

    public ContainmentType QueryHeight(ref BoundingBox query)
    {
      if (this.PruningTree == null)
        return ContainmentType.Intersects;
      if (MyHeightmapFace.m_queryStack == null || MyHeightmapFace.m_queryStack.Length < this.PruningTree.Length)
        MyHeightmapFace.m_queryStack = new MyHeightmapFace.SEntry[this.PruningTree.Length];
      if ((double) query.Min.Z > (double) this.Root.Max)
        return ContainmentType.Disjoint;
      if ((double) query.Max.Z < (double) this.Root.Min)
        return ContainmentType.Contains;
      if ((double) query.Max.X < 0.0 || (double) query.Max.Y < 0.0 || ((double) query.Min.X > 1.0 || (double) query.Min.Y > 1.0))
        return ContainmentType.Disjoint;
      if (this.PruningTree.Length == 0)
        return ContainmentType.Intersects;
      if ((double) query.Max.X == 1.0)
        query.Max.X = 1f;
      if ((double) query.Max.Y == 1.0)
        query.Max.Y = 1f;
      ContainmentType containmentType1 = ContainmentType.Intersects;
      float num1 = Math.Max(query.Width, query.Height);
      if ((double) num1 < (double) this.m_pixelSizeFour)
      {
        MyHeightmapFace.Box2I box2I = new MyHeightmapFace.Box2I(ref query, (uint) this.Resolution);
        box2I.Min -= 1;
        box2I.Max += 1;
        box2I.Intersect(ref this.m_bounds);
        int num2 = (int) ((double) query.Min.Z * (double) ushort.MaxValue);
        int num3 = (int) ((double) query.Max.Z * (double) ushort.MaxValue);
        ushort num4;
        this.GetValue(box2I.Min.X, box2I.Min.Y, out num4);
        ContainmentType containmentType2;
        if ((int) num4 > num3)
        {
          containmentType2 = ContainmentType.Contains;
        }
        else
        {
          if ((int) num4 >= num2)
            return ContainmentType.Intersects;
          containmentType2 = ContainmentType.Disjoint;
        }
        int num5 = (int) ushort.MaxValue;
        int num6 = 0;
        for (int y = box2I.Min.Y; y <= box2I.Max.Y; ++y)
        {
          for (int x = box2I.Min.X; x <= box2I.Max.X; ++x)
          {
            this.GetValue(x, y, out num4);
            if ((int) num4 > num6)
              num6 = (int) num4;
            if ((int) num4 < num5)
              num5 = (int) num4;
          }
        }
        int num7 = num6 - num5;
        int num8 = num7 + (num7 >> 1);
        int num9 = num6 + num8;
        int num10 = num5 - num8;
        if (num2 > num9)
          return ContainmentType.Disjoint;
        return num3 < num10 ? ContainmentType.Contains : ContainmentType.Intersects;
      }
      uint num11 = (uint) MathHelper.Clamp(Math.Log((double) num1 * (double) (this.Resolution / MyHeightmapFace.HeightmapNode.HEIGHTMAP_LEAF_SIZE)) / Math.Log((double) MyHeightmapFace.HeightmapNode.HEIGHTMAP_BRANCH_FACTOR), 0.0, (double) (this.PruningTree.Length - 1));
      int index = 0;
      MyHeightmapFace.SEntry[] queryStack = MyHeightmapFace.m_queryStack;
      MyHeightmapFace.Box2I other = new MyHeightmapFace.Box2I(Vector2I.Zero, new Vector2I((int) this.PruningTree[(int) num11].Res - 1));
      queryStack[0].Bounds = new MyHeightmapFace.Box2I(ref query, this.PruningTree[(int) num11].Res);
      queryStack[0].Bounds.Intersect(ref other);
      queryStack[0].Next = queryStack[0].Bounds.Min;
      queryStack[0].Level = num11;
      queryStack[0].Result = containmentType1;
      queryStack[0].Continue = false;
label_38:
      while (index != -1)
      {
        MyHeightmapFace.SEntry sentry = queryStack[index];
        for (int y = sentry.Next.Y; y <= sentry.Bounds.Max.Y; ++y)
        {
          for (int x = sentry.Bounds.Min.X; x <= sentry.Bounds.Max.X; ++x)
          {
            if (!sentry.Continue)
            {
              sentry.Intersection = this.PruningTree[(int) sentry.Level].Intersect(x, y, ref query);
              if (sentry.Intersection == ContainmentType.Intersects && this.PruningTree[(int) sentry.Level].IsCellNotContained(x, y, ref query) && sentry.Level != 0U)
              {
                sentry.Next = new Vector2I(x, y);
                sentry.Continue = true;
                queryStack[index] = sentry;
                ++index;
                queryStack[index] = new MyHeightmapFace.SEntry(ref query, this.PruningTree[(int) sentry.Level - 1].Res, new Vector2I(x, y), sentry.Result, sentry.Level - 1U);
                goto label_38;
              }
            }
            else
            {
              sentry.Continue = false;
              x = sentry.Next.X;
            }
            switch (sentry.Intersection)
            {
              case ContainmentType.Disjoint:
                if (sentry.Result == ContainmentType.Contains)
                {
                  sentry.Result = ContainmentType.Intersects;
                  goto label_57;
                }
                else
                {
                  sentry.Result = ContainmentType.Disjoint;
                  break;
                }
              case ContainmentType.Contains:
                if (sentry.Result == ContainmentType.Disjoint)
                {
                  sentry.Result = ContainmentType.Intersects;
                  goto label_57;
                }
                else
                {
                  sentry.Result = ContainmentType.Contains;
                  break;
                }
              case ContainmentType.Intersects:
                sentry.Result = ContainmentType.Intersects;
                goto label_57;
            }
          }
        }
label_57:
        containmentType1 = sentry.Result;
        --index;
        if (index >= 0)
          queryStack[index].Intersection = containmentType1;
      }
      return containmentType1;
    }

    public void GetBounds(ref BoundingBox query)
    {
      float num1 = Math.Max(query.Width, query.Height);
      if ((double) num1 < (double) this.m_pixelSizeFour || this.PruningTree == null || this.PruningTree.Length == 0)
      {
        MyHeightmapFace.Box2I box2I = new MyHeightmapFace.Box2I(ref query, (uint) this.Resolution);
        box2I.Min -= 1;
        box2I.Max += 1;
        box2I.Intersect(ref this.m_bounds);
        ushort num2;
        this.GetValue(box2I.Min.X, box2I.Min.Y, out num2);
        int num3 = (int) ushort.MaxValue;
        int num4 = 0;
        for (int y = box2I.Min.Y; y <= box2I.Max.Y; ++y)
        {
          for (int x = box2I.Min.X; x <= box2I.Max.X; ++x)
          {
            this.GetValue(x, y, out num2);
            if ((int) num2 > num4)
              num4 = (int) num2;
            if ((int) num2 < num3)
              num3 = (int) num2;
          }
        }
        int num5 = (num4 - num3) * 2 / 3;
        int num6 = num4 + num5;
        int num7 = num3 - num5;
        query.Min.Z = (float) num7 * 1.525902E-05f;
        query.Max.Z = (float) num6 * 1.525902E-05f;
      }
      else
      {
        uint num2 = (uint) (this.PruningTree.Length - 1) - (uint) MathHelper.Clamp(Math.Log((double) this.Resolution / ((double) num1 * (double) MyHeightmapFace.HeightmapNode.HEIGHTMAP_LEAF_SIZE)) / Math.Log((double) MyHeightmapFace.HeightmapNode.HEIGHTMAP_BRANCH_FACTOR), 0.0, (double) (this.PruningTree.Length - 1));
        MyHeightmapFace.Box2I other = new MyHeightmapFace.Box2I(Vector2I.Zero, new Vector2I((int) this.PruningTree[(int) num2].Res - 1));
        MyHeightmapFace.Box2I box2I = new MyHeightmapFace.Box2I(ref query, this.PruningTree[(int) num2].Res);
        box2I.Intersect(ref other);
        query.Min.Z = float.PositiveInfinity;
        query.Max.Z = float.NegativeInfinity;
        int res = (int) this.PruningTree[(int) num2].Res;
        for (int y = box2I.Min.Y; y <= box2I.Max.Y; ++y)
        {
          for (int x = box2I.Min.X; x <= box2I.Max.X; ++x)
          {
            MyHeightmapFace.HeightmapNode node = this.PruningTree[(int) num2].Nodes[y * res + x];
            if ((double) query.Min.Z > (double) node.Min)
              query.Min.Z = node.Min;
            if ((double) query.Max.Z < (double) node.Max)
              query.Max.Z = node.Max;
          }
        }
      }
    }

    public bool QueryLine(
      ref Vector3 from,
      ref Vector3 to,
      out float startOffset,
      out float endOffset)
    {
      if (this.PruningTree == null)
      {
        startOffset = 0.0f;
        endOffset = 1f;
        return true;
      }
      Vector2 vector2_1 = new Vector2(from.X, from.Y);
      Vector2 vector2_2 = new Vector2(to.X, to.Y);
      Vector2 vector2_3 = vector2_1 * (float) this.ResolutionMinusOne;
      Vector2 vector2_4 = vector2_2 * (float) this.ResolutionMinusOne;
      int num1 = (int) Math.Ceiling((double) (vector2_4 - vector2_3).Length());
      Vector3 vector3_1 = new Vector3(vector2_3 - vector2_4, (float) (((double) to.Z - (double) from.Z) * (double) ushort.MaxValue));
      float z1 = vector3_1.Z;
      float num2 = 1f / (float) num1;
      vector3_1 *= num2;
      Vector3 vector3_2 = new Vector3(vector2_3, from.Z * (float) ushort.MaxValue);
      float z2 = vector3_2.Z;
      for (int index1 = 0; index1 < num1; ++index1)
      {
        int num3 = (int) Math.Round((double) vector3_2.X);
        int num4 = (int) Math.Round((double) vector3_2.Y);
        int num5 = (int) vector3_2.Z;
        int num6 = (int) ((double) vector3_2.Z + (double) vector3_1.Z + 0.5);
        if (num5 > num6)
        {
          int num7 = num6;
          num6 = num5;
          num5 = num7;
        }
        int val2_1 = int.MaxValue;
        int val2_2 = int.MinValue;
        for (int index2 = -1; index2 < 2; ++index2)
        {
          for (int index3 = -1; index3 < 2; ++index3)
          {
            int val1 = (int) this.GetValue(num3 + index2, num4 + index3);
            val2_1 = Math.Min(val1, val2_1);
            val2_2 = Math.Max(val1, val2_2);
          }
        }
        if (num6 >= val2_1 && num5 <= val2_2)
        {
          startOffset = (double) vector3_1.Z >= 0.0 ? Math.Max(((float) val2_1 - z2) / z1, 0.0f) : Math.Max((float) -((double) z2 - (double) val2_2) / z1, 0.0f);
          endOffset = 1f;
          return (double) startOffset < (double) endOffset;
        }
        vector3_2 += vector3_1;
      }
      startOffset = 0.0f;
      endOffset = 1f;
      return false;
    }

    public unsafe void Dispose()
    {
      if (this.IsPersistent)
        return;
      if (this.m_dataBuffer != null)
        MyHeightmapFace.BufferAllocator.Dispose(this.m_dataBuffer);
      this.m_dataBuffer = (NativeArray) null;
      this.Data = (ushort*) null;
    }

    public struct HeightmapNode
    {
      public static readonly int HEIGHTMAP_BRANCH_FACTOR = 4;
      public static readonly int HEIGHTMAP_LEAF_SIZE = 8;
      public float Min;
      public float Max;

      internal ContainmentType Intersect(ref BoundingBox query)
      {
        if ((double) query.Min.Z > (double) this.Max)
          return ContainmentType.Disjoint;
        return (double) query.Max.Z < (double) this.Min ? ContainmentType.Contains : ContainmentType.Intersects;
      }
    }

    public struct HeightmapLevel
    {
      public MyHeightmapFace.HeightmapNode[] Nodes;
      private uint m_res;
      public float Recip;

      public uint Res
      {
        get => this.m_res;
        set
        {
          this.m_res = value;
          this.Recip = 1f / (float) this.m_res;
        }
      }

      public ContainmentType Intersect(int x, int y, ref BoundingBox query) => this.Nodes[(long) y * (long) this.Res + (long) x].Intersect(ref query);

      public bool IsCellContained(int x, int y, ref BoundingBox box)
      {
        Vector2 vector2_1 = new Vector2((float) x, (float) y) * this.Recip;
        Vector2 vector2_2 = vector2_1 + this.Recip;
        return (double) box.Min.X <= (double) vector2_1.X && (double) box.Min.Y <= (double) vector2_1.Y && ((double) box.Max.X >= (double) vector2_2.X && (double) box.Max.Y >= (double) vector2_2.Y);
      }

      public bool IsCellNotContained(int x, int y, ref BoundingBox box)
      {
        Vector2 vector2_1 = new Vector2((float) x, (float) y) * this.Recip;
        Vector2 vector2_2 = vector2_1 + this.Recip;
        return (double) box.Min.X > (double) vector2_1.X || (double) box.Min.Y > (double) vector2_1.Y || ((double) box.Max.X < (double) vector2_2.X || (double) box.Max.Y > (double) vector2_2.Y);
      }
    }

    private struct Box2I
    {
      public Vector2I Min;
      public Vector2I Max;

      public Box2I(ref BoundingBox bb, uint scale)
      {
        this.Min = new Vector2I((int) ((double) bb.Min.X * (double) scale), (int) ((double) bb.Min.Y * (double) scale));
        this.Max = new Vector2I((int) ((double) bb.Max.X * (double) scale), (int) ((double) bb.Max.Y * (double) scale));
      }

      public Box2I(Vector2I min, Vector2I max)
      {
        this.Min = min;
        this.Max = max;
      }

      public void Intersect(ref MyHeightmapFace.Box2I other)
      {
        this.Min.X = Math.Max(this.Min.X, other.Min.X);
        this.Min.Y = Math.Max(this.Min.Y, other.Min.Y);
        this.Max.X = Math.Min(this.Max.X, other.Max.X);
        this.Max.Y = Math.Min(this.Max.Y, other.Max.Y);
      }

      public override string ToString() => string.Format("[({0}), ({1})]", (object) this.Min, (object) this.Max);
    }

    private struct SEntry
    {
      public MyHeightmapFace.Box2I Bounds;
      public Vector2I Next;
      public ContainmentType Result;
      public ContainmentType Intersection;
      public uint Level;
      public bool Continue;

      public SEntry(
        ref BoundingBox query,
        uint res,
        Vector2I cell,
        ContainmentType result,
        uint level)
      {
        MyHeightmapFace.Box2I box2I = new MyHeightmapFace.Box2I(ref query, res);
        cell *= MyHeightmapFace.HeightmapNode.HEIGHTMAP_BRANCH_FACTOR;
        MyHeightmapFace.Box2I other = new MyHeightmapFace.Box2I(cell, cell + MyHeightmapFace.HeightmapNode.HEIGHTMAP_BRANCH_FACTOR - 1);
        box2I.Intersect(ref other);
        this.Bounds = box2I;
        this.Next = box2I.Min;
        this.Level = level;
        this.Result = result;
        this.Intersection = result;
        this.Continue = false;
      }
    }
  }
}
