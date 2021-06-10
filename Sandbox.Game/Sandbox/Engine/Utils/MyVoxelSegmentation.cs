// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Utils.MyVoxelSegmentation
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace Sandbox.Engine.Utils
{
  public class MyVoxelSegmentation
  {
    private HashSet<Vector3I> m_filledVoxels = new HashSet<Vector3I>((IEqualityComparer<Vector3I>) new MyVoxelSegmentation.Vector3IEqualityComparer());
    private HashSet<Vector3I> m_selectionList = new HashSet<Vector3I>((IEqualityComparer<Vector3I>) new MyVoxelSegmentation.Vector3IEqualityComparer());
    private List<MyVoxelSegmentation.Segment> m_segments = new List<MyVoxelSegmentation.Segment>();
    private List<MyVoxelSegmentation.Segment> m_tmpSegments = new List<MyVoxelSegmentation.Segment>();
    private HashSet<Vector3I> m_usedVoxels = new HashSet<Vector3I>();

    public void ClearInput() => this.m_filledVoxels.Clear();

    public void AddInput(Vector3I input) => this.m_filledVoxels.Add(input);

    public int InputCount => this.m_filledVoxels.Count;

    public List<MyVoxelSegmentation.Segment> FindSegments(
      MyVoxelSegmentationType segmentationType = MyVoxelSegmentationType.Optimized,
      int mergeIterations = 1)
    {
      this.m_segments.Clear();
      switch (segmentationType)
      {
        case MyVoxelSegmentationType.ExtraSimple:
          this.CreateSegmentsExtraSimple();
          break;
        case MyVoxelSegmentationType.Simple:
          this.CreateSegmentsSimple();
          break;
        case MyVoxelSegmentationType.Simple2:
          this.CreateSegmentsSimple2();
          break;
        default:
          this.CreateSegments(segmentationType == MyVoxelSegmentationType.Fast);
          this.m_segments.Sort((IComparer<MyVoxelSegmentation.Segment>) new MyVoxelSegmentation.SegmentSizeComparer());
          this.RemoveFullyContainedOptimized();
          this.ClipSegments();
          for (int index = 0; index < mergeIterations; ++index)
            this.MergeSegments();
          break;
      }
      return this.m_segments;
    }

    private void CreateSegmentsExtraSimple()
    {
      while (this.m_filledVoxels.Count > 0)
      {
        HashSet<Vector3I>.Enumerator enumerator = this.m_filledVoxels.GetEnumerator();
        enumerator.MoveNext();
        Vector3I current = enumerator.Current;
        Vector3I pos = current;
        this.ExpandX(ref current, ref pos);
        this.ExpandY(ref current, ref pos);
        this.ExpandZ(ref current, ref pos);
        this.m_segments.Add(new MyVoxelSegmentation.Segment(current, pos));
        for (int x = current.X; x <= pos.X; ++x)
        {
          for (int y = current.Y; y <= pos.Y; ++y)
          {
            for (int z = current.Z; z <= pos.Z; ++z)
              this.m_filledVoxels.Remove(new Vector3I(x, y, z));
          }
        }
      }
    }

    private void MergeSegments()
    {
      for (int index1 = 0; index1 < this.m_segments.Count; ++index1)
      {
        int index2 = index1 + 1;
        while (index2 < this.m_segments.Count)
        {
          MyVoxelSegmentation.Segment segment1 = this.m_segments[index1];
          MyVoxelSegmentation.Segment segment2 = this.m_segments[index2];
          int num = 0;
          if (segment1.Min.X == segment2.Min.X && segment1.Max.X == segment2.Max.X)
            ++num;
          if (segment1.Min.Y == segment2.Min.Y && segment1.Max.Y == segment2.Max.Y)
            ++num;
          if (segment1.Min.Z == segment2.Min.Z && segment1.Max.Z == segment2.Max.Z)
            ++num;
          if (num == 2 && (segment1.Min.X == segment2.Max.X + 1 || segment1.Max.X + 1 == segment2.Min.X || (segment1.Min.Y == segment2.Max.Y + 1 || segment1.Max.Y + 1 == segment2.Min.Y) || (segment1.Min.Z == segment2.Max.Z + 1 || segment1.Max.Z + 1 == segment2.Min.Z)))
          {
            segment1.Min = Vector3I.Min(segment1.Min, segment2.Min);
            segment1.Max = Vector3I.Max(segment1.Max, segment2.Max);
            this.m_segments[index1] = segment1;
            this.m_segments.RemoveAt(index2);
          }
          else
            ++index2;
        }
      }
    }

    private void ClipSegments()
    {
      for (int index1 = this.m_segments.Count - 1; index1 >= 0; --index1)
      {
        this.m_filledVoxels.Clear();
        this.AddAllVoxels(this.m_segments[index1].Min, this.m_segments[index1].Max);
        for (int index2 = this.m_segments.Count - 1; index2 >= 0; --index2)
        {
          if (index1 != index2)
          {
            this.RemoveVoxels(this.m_segments[index2].Min, this.m_segments[index2].Max);
            if (this.m_filledVoxels.Count == 0)
              break;
          }
        }
        if (this.m_filledVoxels.Count == 0)
        {
          this.m_segments.RemoveAt(index1);
        }
        else
        {
          MyVoxelSegmentation.Segment segment = this.m_segments[index1];
          segment.Replace((IEnumerable<Vector3I>) this.m_filledVoxels);
          this.m_segments[index1] = segment;
        }
      }
    }

    private void AddAllVoxels(Vector3I from, Vector3I to)
    {
      for (int x = from.X; x <= to.X; ++x)
      {
        for (int y = from.Y; y <= to.Y; ++y)
        {
          for (int z = from.Z; z <= to.Z; ++z)
            this.m_filledVoxels.Add(new Vector3I(x, y, z));
        }
      }
    }

    private void RemoveVoxels(Vector3I from, Vector3I to)
    {
      for (int x = from.X; x <= to.X; ++x)
      {
        for (int y = from.Y; y <= to.Y; ++y)
        {
          for (int z = from.Z; z <= to.Z; ++z)
            this.m_filledVoxels.Remove(new Vector3I(x, y, z));
        }
      }
    }

    private void RemoveFullyContained()
    {
      for (int index1 = 0; index1 < this.m_segments.Count; ++index1)
      {
        int index2 = index1 + 1;
        while (index2 < this.m_segments.Count)
        {
          if (this.m_segments[index1].Contains(this.m_segments[index2]))
            this.m_segments.RemoveAt(index2);
          else
            ++index2;
        }
      }
    }

    private void RemoveFullyContainedOptimized()
    {
      this.m_filledVoxels.Clear();
      this.m_tmpSegments.Clear();
      for (int index = 0; index < this.m_segments.Count; ++index)
      {
        bool flag = false;
        Vector3I min = this.m_segments[index].Min;
        Vector3I max = this.m_segments[index].Max;
        Vector3I vector3I;
        for (vector3I.X = min.X; vector3I.X <= max.X; ++vector3I.X)
        {
          for (vector3I.Y = min.Y; vector3I.Y <= max.Y; ++vector3I.Y)
          {
            for (vector3I.Z = min.Z; vector3I.Z <= max.Z; ++vector3I.Z)
              flag = this.m_filledVoxels.Add(vector3I) | flag;
          }
        }
        if (flag)
          this.m_tmpSegments.Add(this.m_segments[index]);
      }
      List<MyVoxelSegmentation.Segment> segments = this.m_segments;
      this.m_segments = this.m_tmpSegments;
      this.m_tmpSegments = segments;
    }

    private void CreateSegments(bool fastMethod)
    {
      this.m_usedVoxels.Clear();
      foreach (Vector3I filledVoxel in this.m_filledVoxels)
      {
        if (!this.m_usedVoxels.Contains(filledVoxel))
        {
          Vector3I vector3I1 = filledVoxel;
          Vector3I vector3I2 = filledVoxel;
          this.ExpandX(ref vector3I1, ref vector3I2);
          this.ExpandY(ref vector3I1, ref vector3I2);
          this.ExpandZ(ref vector3I1, ref vector3I2);
          this.AddSegment(ref vector3I1, ref vector3I2);
          if (!fastMethod)
          {
            while (vector3I2.X > vector3I1.X)
            {
              while (vector3I2.Y > vector3I1.Y)
              {
                --vector3I2.Y;
                vector3I2.Z = vector3I1.Z;
                this.ExpandZ(ref vector3I1, ref vector3I2);
                this.AddSegment(ref vector3I1, ref vector3I2);
              }
              --vector3I2.X;
              vector3I2.Y = vector3I1.Y;
              vector3I2.Z = vector3I1.Z;
              this.ExpandY(ref vector3I1, ref vector3I2);
              this.ExpandZ(ref vector3I1, ref vector3I2);
              this.AddSegment(ref vector3I1, ref vector3I2);
            }
          }
        }
      }
    }

    private void AddSegment(ref Vector3I from, ref Vector3I to)
    {
      bool flag = false;
      Vector3I vector3I;
      for (vector3I.X = from.X; vector3I.X <= to.X; ++vector3I.X)
      {
        for (vector3I.Y = from.Y; vector3I.Y <= to.Y; ++vector3I.Y)
        {
          for (vector3I.Z = from.Z; vector3I.Z <= to.Z; ++vector3I.Z)
            flag = this.m_usedVoxels.Add(vector3I) | flag;
        }
      }
      if (!flag)
        return;
      this.m_segments.Add(new MyVoxelSegmentation.Segment(from, to));
    }

    private Vector3I ShiftVector(Vector3I vec) => new Vector3I(vec.Z, vec.X, vec.Y);

    private bool AllFilled(Vector3I from, Vector3I to)
    {
      Vector3I vector3I;
      for (vector3I.X = to.X; vector3I.X >= from.X; --vector3I.X)
      {
        for (vector3I.Y = to.Y; vector3I.Y >= from.Y; --vector3I.Y)
        {
          for (vector3I.Z = to.Z; vector3I.Z >= from.Z; --vector3I.Z)
          {
            if (!this.m_filledVoxels.Contains(vector3I))
              return false;
          }
        }
      }
      return true;
    }

    private int Expand(Vector3I start, ref Vector3I pos, ref Vector3I expand)
    {
      int num = 0;
      while (this.AllFilled(start + expand, pos + expand))
      {
        start += expand;
        pos += expand;
        ++num;
      }
      return num;
    }

    private int ExpandX(ref Vector3I start, ref Vector3I pos) => this.Expand(start, ref pos, ref Vector3I.UnitX);

    private int ExpandY(ref Vector3I start, ref Vector3I pos) => this.Expand(start, ref pos, ref Vector3I.UnitY);

    private int ExpandZ(ref Vector3I start, ref Vector3I pos) => this.Expand(start, ref pos, ref Vector3I.UnitZ);

    private void CreateSegmentsSimple2()
    {
      this.m_selectionList.Clear();
      foreach (Vector3I filledVoxel in this.m_filledVoxels)
        this.m_selectionList.Add(filledVoxel);
      this.CreateSegmentsSimpleCore();
    }

    private void CreateSegmentsSimple()
    {
      HashSet<Vector3I> selectionList = this.m_selectionList;
      this.m_selectionList = this.m_filledVoxels;
      this.CreateSegmentsSimpleCore();
      this.m_selectionList = selectionList;
    }

    private void CreateSegmentsSimpleCore()
    {
      while (this.m_selectionList.Count > 0)
      {
        HashSet<Vector3I>.Enumerator enumerator = this.m_selectionList.GetEnumerator();
        enumerator.MoveNext();
        bool flag1 = true;
        bool flag2 = true;
        bool flag3 = true;
        bool flag4 = true;
        bool flag5 = true;
        bool flag6 = true;
        Vector3I current = enumerator.Current;
        Vector3I max = current;
        this.m_filledVoxels.Remove(current);
        this.m_selectionList.Remove(current);
        while (flag1 | flag2 | flag3 | flag4 | flag5 | flag6)
        {
          if (flag1)
            flag1 = this.ExpandByOnePlusX(ref current, ref max);
          if (flag4)
            flag4 = this.ExpandByOneMinusX(ref current, ref max);
          if (flag2)
            flag2 = this.ExpandByOnePlusY(ref current, ref max);
          if (flag5)
            flag5 = this.ExpandByOneMinusY(ref current, ref max);
          if (flag3)
            flag3 = this.ExpandByOnePlusZ(ref current, ref max);
          if (flag6)
            flag6 = this.ExpandByOneMinusZ(ref current, ref max);
        }
        this.m_segments.Add(new MyVoxelSegmentation.Segment(current, max));
      }
    }

    private bool ExpandByOnePlusX(ref Vector3I min, ref Vector3I max)
    {
      int x = max.X + 1;
      for (int y = min.Y; y <= max.Y; ++y)
      {
        for (int z = min.Z; z <= max.Z; ++z)
        {
          if (!this.m_filledVoxels.Contains(new Vector3I(x, y, z)))
            return false;
        }
      }
      max.X = x;
      for (int y = min.Y; y <= max.Y; ++y)
      {
        for (int z = min.Z; z <= max.Z; ++z)
          this.m_selectionList.Remove(new Vector3I(x, y, z));
      }
      return true;
    }

    private bool ExpandByOnePlusY(ref Vector3I min, ref Vector3I max)
    {
      int y = max.Y + 1;
      for (int x = min.X; x <= max.X; ++x)
      {
        for (int z = min.Z; z <= max.Z; ++z)
        {
          if (!this.m_filledVoxels.Contains(new Vector3I(x, y, z)))
            return false;
        }
      }
      max.Y = y;
      for (int x = min.X; x <= max.X; ++x)
      {
        for (int z = min.Z; z <= max.Z; ++z)
          this.m_selectionList.Remove(new Vector3I(x, y, z));
      }
      return true;
    }

    private bool ExpandByOnePlusZ(ref Vector3I min, ref Vector3I max)
    {
      int z = max.Z + 1;
      for (int x = min.X; x <= max.X; ++x)
      {
        for (int y = min.Y; y <= max.Y; ++y)
        {
          if (!this.m_filledVoxels.Contains(new Vector3I(x, y, z)))
            return false;
        }
      }
      max.Z = z;
      for (int x = min.X; x <= max.X; ++x)
      {
        for (int y = min.Y; y <= max.Y; ++y)
          this.m_selectionList.Remove(new Vector3I(x, y, z));
      }
      return true;
    }

    private bool ExpandByOneMinusX(ref Vector3I min, ref Vector3I max)
    {
      int x = min.X - 1;
      for (int y = min.Y; y <= max.Y; ++y)
      {
        for (int z = min.Z; z <= max.Z; ++z)
        {
          if (!this.m_filledVoxels.Contains(new Vector3I(x, y, z)))
            return false;
        }
      }
      min.X = x;
      for (int y = min.Y; y <= max.Y; ++y)
      {
        for (int z = min.Z; z <= max.Z; ++z)
          this.m_selectionList.Remove(new Vector3I(x, y, z));
      }
      return true;
    }

    private bool ExpandByOneMinusY(ref Vector3I min, ref Vector3I max)
    {
      int y = min.Y - 1;
      for (int x = min.X; x <= max.X; ++x)
      {
        for (int z = min.Z; z <= max.Z; ++z)
        {
          if (!this.m_filledVoxels.Contains(new Vector3I(x, y, z)))
            return false;
        }
      }
      min.Y = y;
      for (int x = min.X; x <= max.X; ++x)
      {
        for (int z = min.Z; z <= max.Z; ++z)
          this.m_selectionList.Remove(new Vector3I(x, y, z));
      }
      return true;
    }

    private bool ExpandByOneMinusZ(ref Vector3I min, ref Vector3I max)
    {
      int z = min.Z - 1;
      for (int x = min.X; x <= max.X; ++x)
      {
        for (int y = min.Y; y <= max.Y; ++y)
        {
          if (!this.m_filledVoxels.Contains(new Vector3I(x, y, z)))
            return false;
        }
      }
      min.Z = z;
      for (int x = min.X; x <= max.X; ++x)
      {
        for (int y = min.Y; y <= max.Y; ++y)
          this.m_selectionList.Remove(new Vector3I(x, y, z));
      }
      return true;
    }

    [ProtoContract]
    public struct Segment
    {
      [ProtoMember(1)]
      public Vector3I Min;
      [ProtoMember(4)]
      public Vector3I Max;

      public Vector3I Size => this.Max - this.Min + Vector3I.One;

      public int VoxelCount => this.Size.X * this.Size.Y * this.Size.Z;

      public Segment(Vector3I min, Vector3I max)
      {
        this.Min = min;
        this.Max = max;
      }

      public bool Contains(MyVoxelSegmentation.Segment b) => Vector3I.Min(b.Min, this.Min) == this.Min && Vector3I.Max(b.Max, this.Max) == this.Max;

      public void Replace(IEnumerable<Vector3I> voxels)
      {
        this.Min = Vector3I.MaxValue;
        this.Max = Vector3I.MinValue;
        foreach (Vector3I voxel in voxels)
        {
          this.Min = Vector3I.Min(this.Min, voxel);
          this.Max = Vector3I.Max(this.Max, voxel);
        }
      }

      protected class Sandbox_Engine_Utils_MyVoxelSegmentation\u003C\u003ESegment\u003C\u003EMin\u003C\u003EAccessor : IMemberAccessor<MyVoxelSegmentation.Segment, Vector3I>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyVoxelSegmentation.Segment owner, in Vector3I value) => owner.Min = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyVoxelSegmentation.Segment owner, out Vector3I value) => value = owner.Min;
      }

      protected class Sandbox_Engine_Utils_MyVoxelSegmentation\u003C\u003ESegment\u003C\u003EMax\u003C\u003EAccessor : IMemberAccessor<MyVoxelSegmentation.Segment, Vector3I>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyVoxelSegmentation.Segment owner, in Vector3I value) => owner.Max = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyVoxelSegmentation.Segment owner, out Vector3I value) => value = owner.Max;
      }

      private class Sandbox_Engine_Utils_MyVoxelSegmentation\u003C\u003ESegment\u003C\u003EActor : IActivator, IActivator<MyVoxelSegmentation.Segment>
      {
        object IActivator.CreateInstance() => (object) new MyVoxelSegmentation.Segment();

        MyVoxelSegmentation.Segment IActivator<MyVoxelSegmentation.Segment>.CreateInstance() => new MyVoxelSegmentation.Segment();
      }
    }

    private class SegmentSizeComparer : IComparer<MyVoxelSegmentation.Segment>
    {
      public int Compare(MyVoxelSegmentation.Segment x, MyVoxelSegmentation.Segment y) => y.VoxelCount - x.VoxelCount;
    }

    private class Vector3IComparer : IComparer<Vector3I>
    {
      public int Compare(Vector3I x, Vector3I y) => x.CompareTo(y);
    }

    private class Vector3IEqualityComparer : IEqualityComparer<Vector3I>
    {
      public bool Equals(Vector3I v1, Vector3I v2) => v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;

      public int GetHashCode(Vector3I obj) => (obj.X * 9767 ^ obj.Y) * 9767 ^ obj.Z;
    }

    private class DescIntComparer : IComparer<int>
    {
      public int Compare(int x, int y) => y - x;
    }
  }
}
