// Decompiled with JetBrains decompiler
// Type: VRage.Voxels.Mesh.MyIsoMeshTaylor
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Game.Voxels;
using VRage.Library.Collections;
using VRage.Voxels.DualContouring;
using VRage.Voxels.Sewing;
using VRageMath;

namespace VRage.Voxels.Mesh
{
  public class MyIsoMeshTaylor
  {
    [ThreadStatic]
    private static MyIsoMeshTaylor m_instance;
    [ThreadStatic]
    private static VrTailor m_nativeInstance;
    private static readonly Vector3I[] Axes = new Vector3I[3]
    {
      new Vector3I(0, 1, 2),
      new Vector3I(2, 0, 1),
      new Vector3I(1, 2, 0)
    };
    private static readonly Vector3I[] InverseAxes = new Vector3I[3]
    {
      new Vector3I(0, 1, 2),
      new Vector3I(1, 2, 0),
      new Vector3I(2, 0, 1)
    };
    private static readonly int[] FaceOffsets = new int[3]
    {
      1,
      2,
      4
    };
    private Vector3I m_startOffset;
    private MyIsoMeshTaylor.VertexGenerator m_generator;
    private int m_minRelativeLod;
    private Dictionary<MyIsoMeshTaylor.Vx, ushort> m_addedVertices = new Dictionary<MyIsoMeshTaylor.Vx, ushort>(MyIsoMeshTaylor.Vx.Comparer);
    private Vector3I m_min;
    private Vector3I m_max;
    private List<MyVoxelQuad> m_tmpQuads = new List<MyVoxelQuad>(3);
    private int[] m_cornerIndices = new int[8];
    private ushort[] m_cornerOffsets = new ushort[8]
    {
      (ushort) 0,
      (ushort) 1,
      (ushort) 2,
      (ushort) 3,
      (ushort) 4,
      (ushort) 5,
      (ushort) 6,
      (ushort) 7
    };
    private MyIsoMeshTaylor.Vx[] m_buffer;
    private Vector3I m_maxes;
    private Vector3I m_bufferMin;
    private int m_coordinateIndex;

    public static MyIsoMeshTaylor Instance => MyIsoMeshTaylor.m_instance ?? (MyIsoMeshTaylor.m_instance = new MyIsoMeshTaylor());

    public static VrTailor NativeInstance => MyIsoMeshTaylor.m_nativeInstance ?? (MyIsoMeshTaylor.m_nativeInstance = new VrTailor());

    internal MyIsoMeshStitch[] Meshes { get; private set; }

    internal int Target { get; private set; }

    internal int Lod { get; private set; }

    public MyIsoMeshTaylor() => this.m_generator = new MyIsoMeshTaylor.VertexGenerator(this);

    public void Stitch(
      MyIsoMeshStitch[] meshes,
      int primary = 0,
      VrSewOperation operation = VrSewOperation.All,
      BoundingBoxI? range = null)
    {
      if (meshes == null)
        throw new ArgumentNullException(nameof (meshes));
      if (meshes.Length != 8)
        throw new ArgumentException("Expecting exactly 8 neighboring mesh references.", nameof (meshes));
      if (meshes[primary] == null)
        throw new ArgumentException("Primary mesh cannot be null");
      if (!MyIsoMeshTaylor.CheckVicinity(meshes))
        throw new ArgumentException("The meshes to be stitched do not line up!", nameof (meshes));
      this.Lod = int.MaxValue;
      foreach (MyIsoMeshStitch mesh in meshes)
      {
        if (mesh != null)
        {
          mesh.IndexEdges();
          this.Lod = Math.Min(mesh.Mesh.Lod, this.Lod);
        }
      }
      this.m_minRelativeLod = meshes[0].Mesh.Lod - this.Lod;
      this.m_startOffset = meshes[0].Mesh.CellStart << this.m_minRelativeLod;
      this.Meshes = meshes;
      this.Target = primary;
      this.CalculateRange(ref range);
      Vector3I startOffset = this.m_startOffset;
      int num = this.Meshes[primary].Mesh.Lod - this.Lod;
      if (num > 0)
        startOffset >>= num;
      Vector3I targetOffset = startOffset - this.Meshes[primary].Mesh.CellStart;
      this.m_generator.Prepare(this.Meshes[primary], (sbyte) primary, targetOffset);
      for (int coordIndex = 0; coordIndex < 3; ++coordIndex)
      {
        if (operation.Contains((VrSewOperation) (1 << 3 - coordIndex)) && this.CollectFace(coordIndex))
          this.GenerateQuads();
      }
      for (int coordIndex = 0; coordIndex < 3; ++coordIndex)
      {
        if (operation.Contains((VrSewOperation) (1 << coordIndex + 4)) && this.CollectEdge(coordIndex))
          this.GenerateQuads();
      }
      if (operation.Contains(VrSewOperation.XYZ) && this.CollectCorner())
        this.GenerateQuads();
      this.m_generator.FinalizeGeneratedVertices();
      this.m_generator.Clear();
      this.m_addedVertices.Clear();
    }

    private void CalculateRange(ref BoundingBoxI? range)
    {
      if (range.HasValue)
      {
        this.m_min = range.Value.Min;
        this.m_max = range.Value.Max;
      }
      else
      {
        int lod = this.Meshes[0].Mesh.Lod;
        if (lod <= this.Lod)
        {
          this.m_min = Vector3I.Zero;
          this.m_max = this.Meshes[0].Mesh.Size - 1;
        }
        else
        {
          BoundingBoxI boundingBoxI1 = this.BoundsInLod(0);
          Vector3I min = boundingBoxI1.Min;
          if (this.Meshes[1] != null && this.Meshes[1].Mesh.Lod < lod)
          {
            BoundingBoxI boundingBoxI2 = this.BoundsInLod(1);
            boundingBoxI1.Min.Y = Math.Max(boundingBoxI1.Min.Y, boundingBoxI2.Min.Y);
            boundingBoxI1.Min.Z = Math.Max(boundingBoxI1.Min.Z, boundingBoxI2.Min.Z);
            boundingBoxI1.Max.Y = Math.Min(boundingBoxI1.Max.Y, boundingBoxI2.Max.Y);
            boundingBoxI1.Max.Z = Math.Min(boundingBoxI1.Max.Z, boundingBoxI2.Max.Z);
          }
          if (this.Meshes[2] != null && this.Meshes[2].Mesh.Lod < lod)
          {
            BoundingBoxI boundingBoxI2 = this.BoundsInLod(2);
            boundingBoxI1.Min.X = Math.Max(boundingBoxI1.Min.X, boundingBoxI2.Min.X);
            boundingBoxI1.Min.Z = Math.Max(boundingBoxI1.Min.Z, boundingBoxI2.Min.Z);
            boundingBoxI1.Max.X = Math.Min(boundingBoxI1.Max.X, boundingBoxI2.Max.X);
            boundingBoxI1.Max.Z = Math.Min(boundingBoxI1.Max.Z, boundingBoxI2.Max.Z);
          }
          if (this.Meshes[4] != null && this.Meshes[4].Mesh.Lod < lod)
          {
            BoundingBoxI boundingBoxI2 = this.BoundsInLod(4);
            boundingBoxI1.Min.X = Math.Max(boundingBoxI1.Min.X, boundingBoxI2.Min.X);
            boundingBoxI1.Min.Y = Math.Max(boundingBoxI1.Min.Y, boundingBoxI2.Min.Y);
            boundingBoxI1.Max.X = Math.Min(boundingBoxI1.Max.X, boundingBoxI2.Max.X);
            boundingBoxI1.Max.Y = Math.Min(boundingBoxI1.Max.Y, boundingBoxI2.Max.Y);
          }
          if (this.Meshes[3] != null && this.Meshes[3].Mesh.Lod < lod)
          {
            BoundingBoxI boundingBoxI2 = this.BoundsInLod(3);
            boundingBoxI1.Max.X = Math.Min(boundingBoxI1.Max.X, boundingBoxI2.Min.X);
            boundingBoxI1.Max.Y = Math.Min(boundingBoxI1.Max.Y, boundingBoxI2.Min.Y);
            boundingBoxI1.Min.Z = Math.Max(boundingBoxI1.Min.Z, boundingBoxI2.Min.Z);
            boundingBoxI1.Max.Z = Math.Min(boundingBoxI1.Max.Z, boundingBoxI2.Max.Z);
          }
          if (this.Meshes[5] != null && this.Meshes[5].Mesh.Lod < lod)
          {
            BoundingBoxI boundingBoxI2 = this.BoundsInLod(5);
            boundingBoxI1.Max.X = Math.Min(boundingBoxI1.Max.X, boundingBoxI2.Min.X);
            boundingBoxI1.Max.Z = Math.Min(boundingBoxI1.Max.Z, boundingBoxI2.Min.Z);
            boundingBoxI1.Min.Y = Math.Max(boundingBoxI1.Min.Y, boundingBoxI2.Min.Y);
            boundingBoxI1.Max.Y = Math.Min(boundingBoxI1.Max.Y, boundingBoxI2.Max.Y);
          }
          if (this.Meshes[6] != null && this.Meshes[6].Mesh.Lod < lod)
          {
            BoundingBoxI boundingBoxI2 = this.BoundsInLod(6);
            boundingBoxI1.Max.Y = Math.Min(boundingBoxI1.Max.Y, boundingBoxI2.Min.Y);
            boundingBoxI1.Max.Z = Math.Min(boundingBoxI1.Max.Z, boundingBoxI2.Min.Z);
            boundingBoxI1.Min.X = Math.Max(boundingBoxI1.Min.X, boundingBoxI2.Min.X);
            boundingBoxI1.Max.X = Math.Min(boundingBoxI1.Max.X, boundingBoxI2.Max.X);
          }
          if (this.Meshes[7] != null && this.Meshes[7].Mesh.Lod < lod)
          {
            BoundingBoxI boundingBoxI2 = this.BoundsInLod(7);
            boundingBoxI1.Max.X = Math.Min(boundingBoxI1.Max.X, boundingBoxI2.Min.X);
            boundingBoxI1.Max.Y = Math.Min(boundingBoxI1.Max.Y, boundingBoxI2.Min.Y);
            boundingBoxI1.Max.Z = Math.Min(boundingBoxI1.Max.Z, boundingBoxI2.Min.Z);
          }
          Vector3I size = boundingBoxI1.Size;
          if (size.Size == 0)
            Debugger.Break();
          if (size.X != size.Y && size.Y != size.Z && size.X != size.Z)
            Debugger.Break();
          int num = this.Meshes[0].Mesh.Lod - this.Lod;
          this.m_min = boundingBoxI1.Min - min >> num;
          this.m_max = boundingBoxI1.Max - min >> num;
        }
      }
    }

    private BoundingBoxI BoundsInLod(int index)
    {
      MyIsoMesh mesh = this.Meshes[index].Mesh;
      int num = mesh.Lod - this.Lod;
      return new BoundingBoxI(mesh.CellStart << num, mesh.CellEnd << num);
    }

    public static bool CheckVicinity(MyIsoMeshStitch[] meshes)
    {
      int num = ((IEnumerable<MyIsoMeshStitch>) meshes).Where<MyIsoMeshStitch>((Func<MyIsoMeshStitch, bool>) (x => x != null)).Min<MyIsoMeshStitch>((Func<MyIsoMeshStitch, int>) (x => x.Mesh.Lod));
      Vector3I vector3I1 = meshes[0].Mesh.CellEnd << meshes[0].Mesh.Lod - num;
      for (int index = 1; index < 8; ++index)
      {
        if (meshes[index] != null && meshes[index] != meshes[0])
        {
          Vector3I vector3I2 = meshes[index].Mesh.CellStart << meshes[index].Mesh.Lod - num;
          if (vector3I1.X != vector3I2.X && vector3I1.Y != vector3I2.Y && vector3I1.Z != vector3I2.Z)
            return false;
        }
      }
      return true;
    }

    public static bool CheckVicinity(VrSewGuide[] meshes)
    {
      int num = ((IEnumerable<VrSewGuide>) meshes).Where<VrSewGuide>((Func<VrSewGuide, bool>) (x => x != null)).Min<VrSewGuide>((Func<VrSewGuide, int>) (x => x.Lod));
      Vector3I vector3I1 = meshes[0].End << meshes[0].Lod - num;
      for (int index = 1; index < 8; ++index)
      {
        if (meshes[index] != null && meshes[index] != meshes[0])
        {
          Vector3I vector3I2 = meshes[index].Start << meshes[index].Lod - num;
          if (vector3I1.X != vector3I2.X && vector3I1.Y != vector3I2.Y && vector3I1.Z != vector3I2.Z)
            return false;
        }
      }
      return true;
    }

    private bool CollectFace(int coordIndex)
    {
      int x = MyIsoMeshTaylor.Axes[coordIndex].X;
      int y = MyIsoMeshTaylor.Axes[coordIndex].Y;
      int z = MyIsoMeshTaylor.Axes[coordIndex].Z;
      sbyte faceOffset = (sbyte) MyIsoMeshTaylor.FaceOffsets[z];
      MyIsoMeshStitch mesh1 = this.Meshes[0];
      MyIsoMeshStitch mesh2 = this.Meshes[(int) faceOffset];
      if (mesh1 == mesh2)
        return false;
      Vector3I max = (this.m_max << this.m_minRelativeLod) - 1;
      Vector3I min = this.m_min << this.m_minRelativeLod;
      min = new Vector3I(min[x], min[y], max[z]);
      max = new Vector3I(max[x], max[y], max[z] + 1);
      this.ResizeBuffer(min, max, coordIndex);
      Vector3I vector3I1 = new Vector3I();
      vector3I1[z] = min.Z;
      int num1 = 0;
      vector3I1[y] = min.Y;
      while (vector3I1[y] <= max.Y)
      {
        int num2 = 0;
        vector3I1[x] = min.X;
        while (vector3I1[x] <= max.X)
        {
          int index1 = num2 + num1;
          ushort index2;
          if (this.TryGetVertex(mesh1, vector3I1, out index2))
            this.m_buffer[index1] = new MyIsoMeshTaylor.Vx((sbyte) 0, (int) index2);
          this.m_buffer[index1].OverIso = this.IsContentOverIso(mesh1, vector3I1);
          Vector3I vector3I2 = vector3I1;
          vector3I2[z]++;
          int index3 = index1 + this.m_maxes.Y;
          if (this.TryGetVertex(mesh2, vector3I2, out index2))
            this.m_buffer[index3] = new MyIsoMeshTaylor.Vx(faceOffset, (int) index2);
          this.m_buffer[index3].OverIso = this.IsContentOverIso(mesh2, vector3I2);
          vector3I1[x]++;
          ++num2;
        }
        vector3I1[y]++;
        num1 += this.m_maxes.X;
      }
      return true;
    }

    private bool CollectEdge(int coordIndex)
    {
      int x = MyIsoMeshTaylor.Axes[coordIndex].X;
      int y = MyIsoMeshTaylor.Axes[coordIndex].Y;
      int z = MyIsoMeshTaylor.Axes[coordIndex].Z;
      sbyte faceOffset1 = (sbyte) MyIsoMeshTaylor.FaceOffsets[x];
      sbyte faceOffset2 = (sbyte) MyIsoMeshTaylor.FaceOffsets[y];
      sbyte mesh1 = (sbyte) ((int) faceOffset1 + (int) faceOffset2);
      MyIsoMeshStitch mesh2 = this.Meshes[0];
      MyIsoMeshStitch mesh3 = this.Meshes[(int) faceOffset1];
      MyIsoMeshStitch mesh4 = this.Meshes[(int) faceOffset2];
      MyIsoMeshStitch mesh5 = this.Meshes[(int) mesh1];
      Vector3I vector3I1 = (this.m_max << this.m_minRelativeLod) - 1;
      Vector3I vector3I2 = this.m_min << this.m_minRelativeLod;
      if (mesh2 == mesh3 && mesh3 == mesh4 && mesh4 == mesh5)
        return false;
      this.ResizeBuffer(new Vector3I(vector3I2[z], vector3I1[x], vector3I1[y]), new Vector3I(vector3I1[z], vector3I1[x] + 1, vector3I1[y] + 1), (coordIndex + 1) % 3);
      Vector3I vector3I3 = vector3I1;
      vector3I3[z] = vector3I2[z];
      int num = 0;
      while (vector3I3[z] <= vector3I1[z])
      {
        int index1 = num;
        int index2 = num + this.m_maxes.X;
        int index3 = num + this.m_maxes.Y;
        int index4 = num + this.m_maxes.X + this.m_maxes.Y;
        ushort index5;
        if (this.TryGetVertex(mesh2, vector3I3, out index5))
          this.m_buffer[index1] = new MyIsoMeshTaylor.Vx((sbyte) 0, (int) index5);
        this.m_buffer[index1].OverIso = this.IsContentOverIso(mesh2, vector3I3);
        vector3I3[x]++;
        if (this.TryGetVertex(mesh3, vector3I3, out index5))
          this.m_buffer[index2] = new MyIsoMeshTaylor.Vx(faceOffset1, (int) index5);
        this.m_buffer[index2].OverIso = this.IsContentOverIso(mesh3, vector3I3);
        vector3I3[y]++;
        if (this.TryGetVertex(mesh5, vector3I3, out index5))
          this.m_buffer[index4] = new MyIsoMeshTaylor.Vx(mesh1, (int) index5);
        this.m_buffer[index4].OverIso = this.IsContentOverIso(mesh5, vector3I3);
        vector3I3[x]--;
        if (this.TryGetVertex(mesh4, vector3I3, out index5))
          this.m_buffer[index3] = new MyIsoMeshTaylor.Vx(faceOffset2, (int) index5);
        this.m_buffer[index3].OverIso = this.IsContentOverIso(mesh4, vector3I3);
        vector3I3[y]--;
        vector3I3[z]++;
        ++num;
      }
      return true;
    }

    private bool CollectCorner()
    {
      MyIsoMeshStitch mesh1 = this.Meshes[0];
      MyIsoMeshStitch mesh2 = this.Meshes[1];
      MyIsoMeshStitch mesh3 = this.Meshes[2];
      MyIsoMeshStitch mesh4 = this.Meshes[3];
      MyIsoMeshStitch mesh5 = this.Meshes[4];
      MyIsoMeshStitch mesh6 = this.Meshes[5];
      MyIsoMeshStitch mesh7 = this.Meshes[6];
      MyIsoMeshStitch mesh8 = this.Meshes[7];
      Vector3I min = (this.m_max << this.m_minRelativeLod) - 1;
      this.ResizeBuffer(min, min + 1, 0);
      Vector3I vector3I = min;
      ushort index;
      if (this.TryGetVertex(mesh1, vector3I, out index))
        this.m_buffer[0] = new MyIsoMeshTaylor.Vx((sbyte) 0, (int) index);
      this.m_buffer[0].OverIso = this.IsContentOverIso(mesh1, vector3I);
      ++vector3I.X;
      if (this.TryGetVertex(mesh2, vector3I, out index))
        this.m_buffer[1] = new MyIsoMeshTaylor.Vx((sbyte) 1, (int) index);
      this.m_buffer[1].OverIso = this.IsContentOverIso(mesh2, vector3I);
      ++vector3I.Y;
      if (this.TryGetVertex(mesh4, vector3I, out index))
        this.m_buffer[3] = new MyIsoMeshTaylor.Vx((sbyte) 3, (int) index);
      this.m_buffer[3].OverIso = this.IsContentOverIso(mesh4, vector3I);
      --vector3I.X;
      if (this.TryGetVertex(mesh3, vector3I, out index))
        this.m_buffer[2] = new MyIsoMeshTaylor.Vx((sbyte) 2, (int) index);
      this.m_buffer[2].OverIso = this.IsContentOverIso(mesh3, vector3I);
      ++vector3I.Z;
      if (this.TryGetVertex(mesh7, vector3I, out index))
        this.m_buffer[6] = new MyIsoMeshTaylor.Vx((sbyte) 6, (int) index);
      this.m_buffer[6].OverIso = this.IsContentOverIso(mesh7, vector3I);
      ++vector3I.X;
      if (this.TryGetVertex(mesh8, vector3I, out index))
        this.m_buffer[7] = new MyIsoMeshTaylor.Vx((sbyte) 7, (int) index);
      this.m_buffer[7].OverIso = this.IsContentOverIso(mesh8, vector3I);
      --vector3I.Y;
      if (this.TryGetVertex(mesh6, vector3I, out index))
        this.m_buffer[5] = new MyIsoMeshTaylor.Vx((sbyte) 5, (int) index);
      this.m_buffer[5].OverIso = this.IsContentOverIso(mesh6, vector3I);
      --vector3I.X;
      if (this.TryGetVertex(mesh5, vector3I, out index))
        this.m_buffer[4] = new MyIsoMeshTaylor.Vx((sbyte) 4, (int) index);
      this.m_buffer[4].OverIso = this.IsContentOverIso(mesh5, vector3I);
      return true;
    }

    private bool TryGetVertex(MyIsoMeshStitch mesh, Vector3I coord, out ushort index)
    {
      if (mesh == null)
      {
        index = (ushort) 0;
        return false;
      }
      coord += this.m_startOffset;
      int num = mesh.Mesh.Lod - this.Lod;
      if (num > 0)
        coord >>= num;
      coord -= mesh.Mesh.CellStart;
      return mesh.TryGetVertex(coord, out index);
    }

    private bool IsContentOverIso(MyIsoMeshStitch mesh, Vector3I pos)
    {
      if (mesh == null)
        mesh = this.Meshes[0];
      pos += this.m_startOffset;
      int num = mesh.Mesh.Lod - this.Lod;
      if (num > 0)
        pos >>= num;
      pos -= mesh.Mesh.CellStart;
      byte content;
      mesh.SampleEdge(pos, out byte _, out content);
      return (int) content - 128 >= 0;
    }

    private void GenerateQuads()
    {
      Vector3I vector3I1 = new Vector3I(1, this.m_maxes.X, this.m_maxes.Y);
      Vector3I vector3I2 = this.m_maxes - vector3I1;
      int[] cornerIndices = this.m_cornerIndices;
      List<MyVoxelQuad> tmpQuads = this.m_tmpQuads;
      for (int index1 = 0; index1 < vector3I2.Z; index1 += vector3I1.Z)
      {
        int num1 = index1 + this.m_maxes.Y;
        for (int index2 = 0; index2 < vector3I2.Y; index2 += this.m_maxes.X)
        {
          int num2 = index2 + this.m_maxes.X;
          byte cubeMask = 0;
          if (this.m_buffer[index2 + index1].OverIso)
            cubeMask |= (byte) 2;
          if (this.m_buffer[num2 + index1].OverIso)
            cubeMask |= (byte) 8;
          if (this.m_buffer[index2 + num1].OverIso)
            cubeMask |= (byte) 32;
          if (this.m_buffer[num2 + num1].OverIso)
            cubeMask |= (byte) 128;
          cornerIndices[1] = (int) (ushort) (index2 + index1);
          cornerIndices[3] = (int) (ushort) (num2 + index1);
          cornerIndices[5] = (int) (ushort) (index2 + num1);
          cornerIndices[7] = (int) (ushort) (num2 + num1);
          uint num3;
          for (uint index3 = 0; (long) index3 < (long) vector3I2.X; index3 = num3)
          {
            num3 = index3 + 1U;
            cubeMask = (byte) ((uint) (byte) ((uint) cubeMask >> 1) & 85U);
            if (this.m_buffer[(long) num3 + (long) index2 + (long) index1].OverIso)
              cubeMask |= (byte) 2;
            if (this.m_buffer[(long) num3 + (long) num2 + (long) index1].OverIso)
              cubeMask |= (byte) 8;
            if (this.m_buffer[(long) num3 + (long) index2 + (long) num1].OverIso)
              cubeMask |= (byte) 32;
            if (this.m_buffer[(long) num3 + (long) num2 + (long) num1].OverIso)
              cubeMask |= (byte) 128;
            this.LeftShift(cornerIndices);
            cornerIndices[1] = (int) (ushort) ((ulong) num3 + (ulong) index2 + (ulong) index1);
            cornerIndices[3] = (int) (ushort) ((ulong) num3 + (ulong) num2 + (ulong) index1);
            cornerIndices[5] = (int) (ushort) ((ulong) num3 + (ulong) index2 + (ulong) num1);
            cornerIndices[7] = (int) (ushort) ((ulong) num3 + (ulong) num2 + (ulong) num1);
            if (MyDualContouringMesher.EdgeTable[(int) cubeMask] != 0)
            {
              MyDualContouringMesher.GenerateQuads(cubeMask, this.m_cornerOffsets, tmpQuads);
              for (int index4 = 0; index4 < tmpQuads.Count; ++index4)
              {
                MyVoxelQuad quad = tmpQuads[index4];
                MyIsoMeshTaylor.Vx vertex1 = this.m_buffer[cornerIndices[(int) quad.V0]];
                MyIsoMeshTaylor.Vx vertex2 = this.m_buffer[cornerIndices[(int) quad.V1]];
                MyIsoMeshTaylor.Vx vertex3 = this.m_buffer[cornerIndices[(int) quad.V2]];
                MyIsoMeshTaylor.Vx vertex4 = this.m_buffer[cornerIndices[(int) quad.V3]];
                bool flag = false;
                if (!vertex1.Valid || !vertex2.Valid || (!vertex3.Valid || !vertex4.Valid))
                {
                  Vector3I baseIndex = this.m_bufferMin + new Vector3I((float) index3, (float) index2, (float) index1) / vector3I1;
                  this.m_generator.GenerateVertex(ref vertex1, baseIndex, ref quad, 0);
                  this.m_generator.GenerateVertex(ref vertex2, baseIndex, ref quad, 1);
                  this.m_generator.GenerateVertex(ref vertex3, baseIndex, ref quad, 2);
                  this.m_generator.GenerateVertex(ref vertex4, baseIndex, ref quad, 3);
                  flag = true;
                }
                if (vertex1.Mesh != (sbyte) -1 && vertex2.Mesh != (sbyte) -1 && (vertex3.Mesh != (sbyte) -1 && vertex4.Mesh != (sbyte) -1))
                {
                  this.TranslateVertex(ref vertex1);
                  this.TranslateVertex(ref vertex2);
                  this.TranslateVertex(ref vertex3);
                  this.TranslateVertex(ref vertex4);
                  if (vertex1 != vertex2 && vertex2 != vertex3 && vertex3 != vertex1)
                    this.Meshes[this.Target].WriteTriangle(vertex2.Index, vertex3.Index, vertex1.Index);
                  if (vertex1 != vertex4 && vertex4 != vertex3 && vertex3 != vertex1)
                    this.Meshes[this.Target].WriteTriangle(vertex3.Index, vertex4.Index, vertex1.Index);
                  if (flag)
                  {
                    this.m_generator.RegisterConnections(vertex1.Index, vertex2.Index, vertex3.Index);
                    this.m_generator.RegisterConnections(vertex4.Index, vertex3.Index, vertex1.Index);
                  }
                }
              }
              tmpQuads.Clear();
            }
          }
        }
      }
    }

    private void LeftShift(int[] corners)
    {
      corners[0] = corners[1];
      corners[2] = corners[3];
      corners[4] = corners[5];
      corners[6] = corners[7];
    }

    private void TranslateVertex(ref MyIsoMeshTaylor.Vx vertex)
    {
      if ((int) vertex.Mesh == this.Target)
        return;
      ushort num;
      if (!this.m_addedVertices.TryGetValue(vertex, out num))
      {
        MyIsoMesh mesh1 = this.Meshes[(int) vertex.Mesh].Mesh;
        MyIsoMesh mesh2 = this.Meshes[this.Target].Mesh;
        ushort index = vertex.Index;
        Vector3I cell = new Vector3I(-1);
        Vector3 pos = MyIsoMeshTaylor.RemapVertex(mesh1, mesh2, index);
        Vector3 normal = mesh1.Normals[(int) index];
        uint colorShift = mesh1.ColorShiftHSV[(int) index];
        byte material = mesh1.Materials[(int) index];
        num = this.Meshes[this.Target].WriteVertex(ref cell, ref pos, ref normal, material, colorShift);
        this.m_addedVertices[vertex] = num;
      }
      vertex = new MyIsoMeshTaylor.Vx((sbyte) this.Target, (int) num);
    }

    private static Vector3 RemapVertex(MyIsoMesh src, MyIsoMesh target, ushort index) => (Vector3) (src.Positions[(int) index] * src.PositionScale + src.PositionOffset - target.PositionOffset) / target.PositionScale;

    private void ResizeBuffer(Vector3I min, Vector3I max, int coordinateIndex)
    {
      this.m_bufferMin = min;
      this.m_coordinateIndex = coordinateIndex;
      Vector3I vector3I = max - min + 1;
      this.m_maxes.X = vector3I.X;
      this.m_maxes.Y = vector3I.Y * this.m_maxes.X;
      this.m_maxes.Z = vector3I.Z * this.m_maxes.Y;
      if (this.m_buffer == null || this.m_maxes.Z > this.m_buffer.Length)
        this.m_buffer = new MyIsoMeshTaylor.Vx[this.m_maxes.Z];
      this.ClearBuffer();
    }

    private void ClearBuffer(int start = 0)
    {
      for (int index = start; index < this.m_buffer.Length; ++index)
        this.m_buffer[index] = MyIsoMeshTaylor.Vx.Invalid;
    }

    private class VertexGenerator
    {
      private readonly MyIsoMeshTaylor m_taylor;
      private MyIsoMeshStitch m_target;
      private sbyte m_targetIndex;
      private Vector3I m_targetOffset;
      private readonly Dictionary<Vector3I, ushort> m_createdVertices = new Dictionary<Vector3I, ushort>((IEqualityComparer<Vector3I>) Vector3I.Comparer);
      private readonly Dictionary<ushort, MyIsoMeshTaylor.Vx> m_generatedPairs = new Dictionary<ushort, MyIsoMeshTaylor.Vx>();
      private readonly MyHashSetDictionary<ushort, ushort> m_adjacentVertices = new MyHashSetDictionary<ushort, ushort>();
      private Vector3I[] m_cornerPositions = new Vector3I[8]
      {
        new Vector3I(0, 0, 0),
        new Vector3I(1, 0, 0),
        new Vector3I(0, 1, 0),
        new Vector3I(1, 1, 0),
        new Vector3I(0, 0, 1),
        new Vector3I(1, 0, 1),
        new Vector3I(0, 1, 1),
        new Vector3I(1, 1, 1)
      };
      private HashSet<uint> m_queued = new HashSet<uint>();

      public VertexGenerator(MyIsoMeshTaylor taylor) => this.m_taylor = taylor;

      public void Prepare(MyIsoMeshStitch target, sbyte targetIndex, Vector3I targetOffset)
      {
        this.m_target = target;
        this.m_targetIndex = targetIndex;
        this.m_targetOffset = targetOffset;
      }

      public void Clear()
      {
        this.m_createdVertices.Clear();
        this.m_adjacentVertices.Clear();
        this.m_generatedPairs.Clear();
        this.m_queued.Clear();
      }

      public void GenerateVertex(
        ref MyIsoMeshTaylor.Vx vertex,
        Vector3I baseIndex,
        ref MyVoxelQuad quad,
        int index)
      {
        if (vertex.Valid)
          return;
        int coordinateIndex = this.m_taylor.m_coordinateIndex;
        Vector3I inverseAx = MyIsoMeshTaylor.InverseAxes[coordinateIndex];
        Vector3I vector3I1 = this.m_cornerPositions[(int) quad[index]];
        vector3I1 = new Vector3I(vector3I1[inverseAx.X], vector3I1[inverseAx.Y], vector3I1[inverseAx.Z]);
        baseIndex = new Vector3I(baseIndex[inverseAx.X], baseIndex[inverseAx.Y], baseIndex[inverseAx.Z]);
        Vector3I key = baseIndex + vector3I1;
        ushort num1;
        if (!this.m_createdVertices.TryGetValue(key, out num1))
        {
          int num2 = this.m_target.Mesh.Lod - this.m_taylor.Lod;
          Vector3I vector3I2 = key;
          if (num2 > 0)
            vector3I2 >>= num2;
          Vector3I cell = vector3I2 + this.m_targetOffset;
          MyIsoMeshTaylor.Vx goodNeighbour = this.FindGoodNeighbour(index, ref quad);
          if (goodNeighbour.Valid)
          {
            this.m_taylor.TranslateVertex(ref goodNeighbour);
            this.m_createdVertices[key] = goodNeighbour.Index;
            vertex = goodNeighbour;
            return;
          }
          Vector3 pos = cell + 0.5f;
          Vector3 normal = Vector3.Normalize(pos);
          if (!key.IsInsideInclusiveEnd(Vector3I.Zero, this.m_target.Mesh.Size - 2))
          {
            MyIsoMeshStitch mesh = this.m_taylor.Meshes[(int) (sbyte) Vector3I.Dot(Vector3I.Sign(Vector3I.Max(key - this.m_target.Mesh.Size + 2, Vector3I.Zero)), new Vector3I(1, 2, 4))];
          }
          num1 = this.m_target.WriteVertex(ref cell, ref pos, ref normal, byte.MaxValue, 0U);
          this.m_createdVertices[key] = num1;
        }
        vertex = new MyIsoMeshTaylor.Vx(this.m_targetIndex, (int) num1);
      }

      private MyIsoMeshTaylor.Vx FindGoodNeighbour(int index, ref MyVoxelQuad quad)
      {
        int num1 = (int) quad[index];
        int corner1 = num1 ^ 1;
        int corner2 = num1 ^ 2;
        int corner3 = num1 ^ 4;
        int num2 = this.CountTriangles(corner1, index, ref quad);
        int num3 = this.CountTriangles(corner2, index, ref quad);
        int num4 = this.CountTriangles(corner3, index, ref quad);
        return num2 == num3 && num3 == num4 && num2 == 0 ? MyIsoMeshTaylor.Vx.Invalid : this.GetBufferVertex(num2 <= num3 ? (num3 <= num4 ? corner3 : corner2) : (num2 <= num4 ? corner3 : corner1));
      }

      private int CountTriangles(int corner, int index, ref MyVoxelQuad quad)
      {
        MyVoxelQuad myVoxelQuad = quad;
        myVoxelQuad[index] = (ushort) corner;
        if (!this.GetBufferVertex(corner).Valid)
          return 0;
        MyTuple<MyIsoMeshTaylor.Vx, MyIsoMeshTaylor.Vx, MyIsoMeshTaylor.Vx, MyIsoMeshTaylor.Vx> myTuple = new MyTuple<MyIsoMeshTaylor.Vx, MyIsoMeshTaylor.Vx, MyIsoMeshTaylor.Vx, MyIsoMeshTaylor.Vx>(this.GetBufferVertex((int) myVoxelQuad.V0), this.GetBufferVertex((int) myVoxelQuad.V1), this.GetBufferVertex((int) myVoxelQuad.V2), this.GetBufferVertex((int) myVoxelQuad.V3));
        if (myTuple.Item1 == myTuple.Item3)
          return 0;
        return myTuple.Item2 == myTuple.Item4 || myTuple.Item2 == myTuple.Item1 || (myTuple.Item2 == myTuple.Item3 || myTuple.Item4 == myTuple.Item1) || myTuple.Item4 == myTuple.Item3 ? 1 : 2;
      }

      private MyIsoMeshTaylor.Vx GetBufferVertex(int cornerIndex) => this.m_taylor.m_buffer[this.m_taylor.m_cornerIndices[cornerIndex]];

      public void RegisterConnections(ushort v0, ushort v1, ushort v2)
      {
        if (this.IsGenerated(v0))
          this.m_adjacentVertices.Add(v0, v1, v2);
        if (this.IsGenerated(v1))
          this.m_adjacentVertices.Add(v1, v2, v0);
        if (!this.IsGenerated(v2))
          return;
        this.m_adjacentVertices.Add(v2, v0, v1);
      }

      public void FinalizeGeneratedVertices()
      {
        foreach (ushort num in this.m_createdVertices.Values)
        {
          this.m_target.AddVertexToIndex(num);
          if (this.IsGenerated(num))
            this.FinalizeVertex(num);
        }
        this.m_queued.Clear();
      }

      private void FinalizeVertex(ushort vx)
      {
        HashSet<ushort> list;
        if (!this.m_queued.Add((uint) vx) || !this.m_adjacentVertices.TryGet(vx, out list))
          return;
        List<Vector3> poolObject1;
        PoolManager.Get<List<Vector3>>(out poolObject1);
        Dictionary<byte, int> poolObject2;
        PoolManager.Get<Dictionary<byte, int>>(out poolObject2);
        foreach (ushort index in list)
        {
          if (!this.IsGenerated(index))
          {
            poolObject1.Add(this.m_target.Mesh.Positions[(int) index]);
            int num;
            poolObject2.TryGetValue(this.m_target.Mesh.Materials[(int) index], out num);
            poolObject2[this.m_target.Mesh.Materials[(int) index]] = num + 1;
          }
        }
        Vector3 pos;
        Vector3 normal1;
        if (this.FitPosition(poolObject1, out pos, out normal1))
        {
          this.m_target.Mesh.Positions[(int) vx] = pos;
          this.m_target.Mesh.Normals[(int) vx] = normal1;
        }
        int num1 = 0;
        byte num2 = 0;
        foreach (KeyValuePair<byte, int> keyValuePair in poolObject2)
        {
          if (keyValuePair.Value > num1)
          {
            num2 = keyValuePair.Key;
            num1 = keyValuePair.Value;
          }
        }
        this.m_target.Mesh.Materials[(int) vx] = num2;
        MyIsoMeshTaylor.Vx vx1;
        if (this.m_generatedPairs.TryGetValue(vx, out vx1))
        {
          MyIsoMesh mesh = this.m_taylor.Meshes[(int) vx1.Mesh].Mesh;
          Vector3 vector3 = MyIsoMeshTaylor.RemapVertex(this.m_target.Mesh, mesh, vx);
          Vector3 normal2 = this.m_target.Mesh.Normals[(int) vx];
          mesh.Positions[(int) vx1.Index] = vector3;
          mesh.Normals[(int) vx1.Index] = normal2;
          mesh.Materials[(int) vx1.Index] = num2;
        }
        PoolManager.Return<List<Vector3>>(ref poolObject1);
        PoolManager.Return<Dictionary<byte, int>>(ref poolObject2);
      }

      private bool FitPosition(List<Vector3> positions, out Vector3 pos, out Vector3 normal)
      {
        Vector3 zero = Vector3.Zero;
        if (positions.Count < 3)
        {
          pos = new Vector3();
          normal = new Vector3();
          return false;
        }
        foreach (Vector3 position in positions)
          zero += position;
        Vector3 vector3_1 = zero / (float) positions.Count;
        float num1 = 0.0f;
        float num2 = 0.0f;
        float num3 = 0.0f;
        float num4 = 0.0f;
        float num5 = 0.0f;
        float num6 = 0.0f;
        foreach (Vector3 position in positions)
        {
          Vector3 vector3_2 = position - vector3_1;
          num1 += vector3_2.X * vector3_2.X;
          num2 += vector3_2.X * vector3_2.Y;
          num3 += vector3_2.X * vector3_2.Z;
          num4 += vector3_2.Y * vector3_2.Y;
          num5 += vector3_2.Y * vector3_2.Z;
          num6 += vector3_2.Z * vector3_2.Z;
        }
        float num7 = (float) ((double) num4 * (double) num6 - (double) num5 * (double) num5);
        float num8 = (float) ((double) num1 * (double) num6 - (double) num3 * (double) num3);
        float num9 = (float) ((double) num1 * (double) num4 - (double) num2 * (double) num2);
        float num10 = num7;
        int num11 = 0;
        if ((double) num8 > (double) num10)
        {
          num11 = 1;
          num10 = num8;
        }
        if ((double) num9 > (double) num10)
        {
          num11 = 2;
          num10 = num9;
        }
        if ((double) num10 < 9.99999974737875E-05)
        {
          pos = new Vector3();
          normal = new Vector3();
          return false;
        }
        switch (num11)
        {
          case 1:
            float x1 = (float) ((double) num5 * (double) num3 - (double) num2 * (double) num6) / num8;
            float z1 = (float) ((double) num2 * (double) num3 - (double) num5 * (double) num1) / num8;
            normal = new Vector3(x1, 1f, z1);
            break;
          case 2:
            float x2 = (float) ((double) num5 * (double) num2 - (double) num3 * (double) num4) / num9;
            float y1 = (float) ((double) num3 * (double) num2 - (double) num5 * (double) num1) / num9;
            normal = new Vector3(x2, y1, 1f);
            break;
          default:
            float y2 = (float) ((double) num3 * (double) num5 - (double) num2 * (double) num6) / num7;
            float z2 = (float) ((double) num2 * (double) num5 - (double) num3 * (double) num4) / num7;
            normal = new Vector3(1f, y2, z2);
            break;
        }
        double num12 = (double) normal.Normalize();
        pos = vector3_1;
        return true;
      }

      public bool IsGenerated(ushort index) => this.m_target.Mesh.Materials[(int) index] == byte.MaxValue;
    }

    private struct Vx
    {
      public sbyte Mesh;
      public bool OverIso;
      public ushort Index;
      public static MyIsoMeshTaylor.Vx Invalid = new MyIsoMeshTaylor.Vx((sbyte) -1, 0);
      public static readonly IEqualityComparer<MyIsoMeshTaylor.Vx> Comparer = (IEqualityComparer<MyIsoMeshTaylor.Vx>) new MyIsoMeshTaylor.Vx.MeshIndexEqualityComparer();

      public bool Valid => this.Mesh != (sbyte) -1;

      public Vx(sbyte mesh, int index)
      {
        this.Mesh = mesh;
        this.Index = (ushort) index;
        this.OverIso = false;
      }

      public static bool operator ==(MyIsoMeshTaylor.Vx left, MyIsoMeshTaylor.Vx right) => left.Equals(right);

      public static bool operator !=(MyIsoMeshTaylor.Vx left, MyIsoMeshTaylor.Vx right) => !(left == right);

      public bool Equals(MyIsoMeshTaylor.Vx other) => (int) this.Mesh == (int) other.Mesh && (int) this.Index == (int) other.Index;

      public override bool Equals(object obj) => obj != null && obj is MyIsoMeshTaylor.Vx other && this.Equals(other);

      public override int GetHashCode() => this.Mesh.GetHashCode() * 397 ^ this.Index.GetHashCode();

      public override string ToString() => !this.Valid ? "[Invalid]" : string.Format("[M{0}: {1}", (object) this.Mesh, (object) this.Index);

      private sealed class MeshIndexEqualityComparer : IEqualityComparer<MyIsoMeshTaylor.Vx>
      {
        public bool Equals(MyIsoMeshTaylor.Vx x, MyIsoMeshTaylor.Vx y) => (int) x.Mesh == (int) y.Mesh && (int) x.Index == (int) y.Index;

        public int GetHashCode(MyIsoMeshTaylor.Vx obj) => obj.Mesh.GetHashCode() * 397 ^ obj.Index.GetHashCode();
      }
    }
  }
}
