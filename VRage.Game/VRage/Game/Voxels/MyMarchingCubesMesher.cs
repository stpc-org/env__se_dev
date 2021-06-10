// Decompiled with JetBrains decompiler
// Type: VRage.Game.Voxels.MyMarchingCubesMesher
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;

namespace VRage.Game.Voxels
{
  public class MyMarchingCubesMesher : IMyIsoMesher
  {
    private const int POLYCUBE_EDGES = 12;
    private readonly MyMarchingCubesMesher.MyEdge[] m_edges = new MyMarchingCubesMesher.MyEdge[12];
    private const int CELL_EDGES_SIZE = 90;
    private MyMarchingCubesMesher.MyEdgeVertex[][][][] m_edgeVertex;
    private int m_edgeVertexCalcCounter;
    private readonly MyVoxelVertex[] m_resultVertices = new MyVoxelVertex[7680];
    private int m_resultVerticesCounter;
    private readonly MyVoxelTriangle[] m_resultTriangles = new MyVoxelTriangle[25600];
    private int m_resultTrianglesCounter;
    private Vector3I m_polygCubes;
    private Vector3I m_voxelStart;
    private const int COPY_TABLE_SIZE = 11;
    private int m_temporaryVoxelsCounter;
    private readonly MyMarchingCubesMesher.MyTemporaryVoxel[] m_temporaryVoxels = new MyMarchingCubesMesher.MyTemporaryVoxel[13310];
    private const int m_sX = 1;
    private const int m_sY = 11;
    private const int m_sZ = 121;
    private readonly MyStorageData m_cache = new MyStorageData();
    private Vector3I m_sizeMinusOne;
    private float m_voxelSizeInMeters;
    private Vector3 m_originPosition;

    public MyMarchingCubesMesher()
    {
      for (int index = 0; index < this.m_edges.Length; ++index)
        this.m_edges[index] = new MyMarchingCubesMesher.MyEdge();
      for (int index = 0; index < this.m_temporaryVoxels.Length; ++index)
        this.m_temporaryVoxels[index] = new MyMarchingCubesMesher.MyTemporaryVoxel();
      this.m_edgeVertexCalcCounter = 0;
      this.m_edgeVertex = new MyMarchingCubesMesher.MyEdgeVertex[90][][][];
      for (int index1 = 0; index1 < 90; ++index1)
      {
        this.m_edgeVertex[index1] = new MyMarchingCubesMesher.MyEdgeVertex[90][][];
        for (int index2 = 0; index2 < 90; ++index2)
        {
          this.m_edgeVertex[index1][index2] = new MyMarchingCubesMesher.MyEdgeVertex[90][];
          for (int index3 = 0; index3 < 90; ++index3)
          {
            this.m_edgeVertex[index1][index2][index3] = new MyMarchingCubesMesher.MyEdgeVertex[3];
            for (int index4 = 0; index4 < 3; ++index4)
            {
              this.m_edgeVertex[index1][index2][index3][index4] = new MyMarchingCubesMesher.MyEdgeVertex();
              this.m_edgeVertex[index1][index2][index3][index4].CalcCounter = 0;
            }
          }
        }
      }
    }

    private void CalcPolygCubeSize(int lodIdx, Vector3I storageSize)
    {
      Vector3I vector3I = storageSize >> lodIdx;
      this.m_polygCubes.X = this.m_voxelStart.X + 8 >= vector3I.X ? 8 : 9;
      this.m_polygCubes.Y = this.m_voxelStart.Y + 8 >= vector3I.Y ? 8 : 9;
      this.m_polygCubes.Z = this.m_voxelStart.Z + 8 >= vector3I.Z ? 8 : 9;
    }

    private byte GetVoxelContent(int x, int y, int z) => this.m_cache.Content(x, y, z);

    private void GetVoxelNormal(
      MyMarchingCubesMesher.MyTemporaryVoxel temporaryVoxel,
      ref Vector3I coord,
      ref Vector3I voxelCoord,
      MyMarchingCubesMesher.MyTemporaryVoxel centerVoxel)
    {
      if (temporaryVoxel.Normal_CalcCounter == this.m_temporaryVoxelsCounter)
        return;
      Vector3I result1 = coord - 1;
      Vector3I result2 = coord + 1;
      MyStorageData cache = this.m_cache;
      Vector3I vector3I = cache.Size3D - 1;
      Vector3I.Max(ref result1, ref Vector3I.Zero, out result1);
      Vector3I.Min(ref result2, ref vector3I, out result2);
      Vector3 vec = new Vector3((float) ((int) cache.Content(result1.X, coord.Y, coord.Z) - (int) cache.Content(result2.X, coord.Y, coord.Z)) / (float) byte.MaxValue, (float) ((int) cache.Content(coord.X, result1.Y, coord.Z) - (int) cache.Content(coord.X, result2.Y, coord.Z)) / (float) byte.MaxValue, (float) ((int) cache.Content(coord.X, coord.Y, result1.Z) - (int) cache.Content(coord.X, coord.Y, result2.Z)) / (float) byte.MaxValue);
      if ((double) vec.LengthSquared() <= 9.99999997475243E-07)
        temporaryVoxel.Normal = centerVoxel.Normal;
      else
        MyUtils.Normalize(ref vec, out temporaryVoxel.Normal);
      temporaryVoxel.Normal_CalcCounter = this.m_temporaryVoxelsCounter;
    }

    private Vector3 ComputeVertexNormal(ref Vector3 position)
    {
      Vector3 vector3 = (position - this.m_originPosition) / this.m_voxelSizeInMeters + 1f;
      Vector3 result;
      result.X = this.SampleContent(vector3.X - 0.01f, vector3.Y, vector3.Z) - this.SampleContent(vector3.X + 0.01f, vector3.Y, vector3.Z);
      result.Y = this.SampleContent(vector3.X, vector3.Y - 0.01f, vector3.Z) - this.SampleContent(vector3.X, vector3.Y + 0.01f, vector3.Z);
      result.Z = this.SampleContent(vector3.X, vector3.Y, vector3.Z - 0.01f) - this.SampleContent(vector3.X, vector3.Y, vector3.Z + 0.01f);
      Vector3.Normalize(ref result, out result);
      return result;
    }

    private float SampleContent(float x, float y, float z)
    {
      Vector3 vector3_1 = new Vector3(x, y, z);
      Vector3I vector3I = Vector3I.Floor(vector3_1);
      Vector3 vector3_2 = vector3_1 - (Vector3) vector3I;
      float num1 = (float) this.m_cache.Content(vector3I.X, vector3I.Y, vector3I.Z);
      float num2 = (float) this.m_cache.Content(vector3I.X + 1, vector3I.Y, vector3I.Z);
      float num3 = (float) this.m_cache.Content(vector3I.X, vector3I.Y + 1, vector3I.Z);
      float num4 = (float) this.m_cache.Content(vector3I.X + 1, vector3I.Y + 1, vector3I.Z);
      float num5 = (float) this.m_cache.Content(vector3I.X, vector3I.Y, vector3I.Z + 1);
      float num6 = (float) this.m_cache.Content(vector3I.X + 1, vector3I.Y, vector3I.Z + 1);
      float num7 = (float) this.m_cache.Content(vector3I.X, vector3I.Y + 1, vector3I.Z + 1);
      float num8 = (float) this.m_cache.Content(vector3I.X + 1, vector3I.Y + 1, vector3I.Z + 1);
      float num9 = num1 + vector3_2.X * (num2 - num1);
      float num10 = num3 + vector3_2.X * (num4 - num3);
      float num11 = num5 + vector3_2.X * (num6 - num5);
      float num12 = num7 + vector3_2.X * (num8 - num7);
      float num13 = num9 + vector3_2.Y * (num10 - num9);
      float num14 = num11 + vector3_2.Y * (num12 - num11);
      return num13 + vector3_2.Z * (num14 - num13);
    }

    private void GetVoxelAmbient(
      MyMarchingCubesMesher.MyTemporaryVoxel temporaryVoxel,
      ref Vector3I coord,
      ref Vector3I tempVoxelCoord)
    {
      if (temporaryVoxel.Ambient_CalcCounter == this.m_temporaryVoxelsCounter)
        return;
      MyStorageData cache = this.m_cache;
      float num1 = 0.0f;
      for (int index1 = -1; index1 <= 1; ++index1)
      {
        for (int index2 = -1; index2 <= 1; ++index2)
        {
          for (int index3 = -1; index3 <= 1; ++index3)
          {
            Vector3I vector3I = new Vector3I(coord.X + index1 - 1, coord.Y + index2 - 1, coord.Z + index3 - 1);
            if (vector3I.X >= 0 && vector3I.X <= this.m_sizeMinusOne.X && (vector3I.Y >= 0 && vector3I.Y <= this.m_sizeMinusOne.Y) && (vector3I.Z >= 0 && vector3I.Z <= this.m_sizeMinusOne.Z))
              num1 += (float) cache.Content(coord.X + index1, coord.Y + index2, coord.Z + index3);
          }
        }
      }
      float num2 = MathHelper.Clamp(1f - num1 / 6885f, 0.4f, 0.9f);
      temporaryVoxel.Ambient = num2;
      temporaryVoxel.Ambient_CalcCounter = this.m_temporaryVoxelsCounter;
    }

    private void GetVertexInterpolation(
      MyStorageData cache,
      MyMarchingCubesMesher.MyTemporaryVoxel inputVoxelA,
      MyMarchingCubesMesher.MyTemporaryVoxel inputVoxelB,
      int edgeIndex)
    {
      MyMarchingCubesMesher.MyEdge edge = this.m_edges[edgeIndex];
      byte num1 = cache.Content(inputVoxelA.IdxInCache);
      byte num2 = cache.Content(inputVoxelB.IdxInCache);
      byte num3 = cache.Material(inputVoxelA.IdxInCache);
      byte num4 = cache.Material(inputVoxelB.IdxInCache);
      if ((double) Math.Abs((int) sbyte.MaxValue - (int) num1) < 9.99999974737875E-06)
      {
        edge.Position = inputVoxelA.Position;
        edge.Normal = inputVoxelA.Normal;
        edge.Material = num3;
        edge.Ambient = inputVoxelA.Ambient;
      }
      else if ((double) Math.Abs((int) sbyte.MaxValue - (int) num2) < 9.99999974737875E-06)
      {
        edge.Position = inputVoxelB.Position;
        edge.Normal = inputVoxelB.Normal;
        edge.Material = num4;
        edge.Ambient = inputVoxelB.Ambient;
      }
      else
      {
        float num5 = (float) ((int) sbyte.MaxValue - (int) num1) / (float) ((int) num2 - (int) num1);
        edge.Position.X = inputVoxelA.Position.X + num5 * (inputVoxelB.Position.X - inputVoxelA.Position.X);
        edge.Position.Y = inputVoxelA.Position.Y + num5 * (inputVoxelB.Position.Y - inputVoxelA.Position.Y);
        edge.Position.Z = inputVoxelA.Position.Z + num5 * (inputVoxelB.Position.Z - inputVoxelA.Position.Z);
        edge.Normal.X = inputVoxelA.Normal.X + num5 * (inputVoxelB.Normal.X - inputVoxelA.Normal.X);
        edge.Normal.Y = inputVoxelA.Normal.Y + num5 * (inputVoxelB.Normal.Y - inputVoxelA.Normal.Y);
        edge.Normal.Z = inputVoxelA.Normal.Z + num5 * (inputVoxelB.Normal.Z - inputVoxelA.Normal.Z);
        edge.Normal = !MathHelper.IsZero(edge.Normal) ? MyUtils.Normalize(edge.Normal) : inputVoxelA.Normal;
        if (MathHelper.IsZero(edge.Normal))
          edge.Normal = inputVoxelA.Normal;
        float num6 = (float) num2 / ((float) num1 + (float) num2);
        edge.Material = (double) num6 <= 0.5 ? num3 : num4;
        edge.Ambient = inputVoxelA.Ambient + num6 * (inputVoxelB.Ambient - inputVoxelA.Ambient);
      }
    }

    public int AffectedRangeOffset => -1;

    public int AffectedRangeSizeChange => 3;

    public int InvalidatedRangeInflate => 2;

    public int VertexPositionRangeSizeChange => 0;

    public float VertexPositionOffsetChange => 0.5f;

    public MyIsoMesh Precalc(
      IMyStorage storage,
      int lod,
      Vector3I voxelStart,
      Vector3I voxelEnd,
      MyStorageDataTypeFlags properties = MyStorageDataTypeFlags.ContentAndMaterial,
      MyVoxelRequestFlags flags = (MyVoxelRequestFlags) 0)
    {
      this.m_resultVerticesCounter = 0;
      this.m_resultTrianglesCounter = 0;
      ++this.m_edgeVertexCalcCounter;
      ++this.m_temporaryVoxelsCounter;
      this.CalcPolygCubeSize(lod, storage.Size);
      this.m_voxelStart = voxelStart;
      Vector3I size = storage.Size;
      this.m_cache.Resize(voxelStart, voxelEnd);
      storage.ReadRange(this.m_cache, MyStorageDataTypeFlags.Content, lod, voxelStart, voxelEnd);
      if (!this.m_cache.ContainsIsoSurface())
        return (MyIsoMesh) null;
      storage.ReadRange(this.m_cache, MyStorageDataTypeFlags.Material, lod, voxelStart, voxelEnd);
      this.ComputeSizeAndOrigin(lod, storage.Size);
      Vector3I zero = Vector3I.Zero;
      Vector3I end = voxelEnd - voxelStart - 3;
      Vector3I next = zero;
      Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref zero, ref end);
      while (vector3IRangeIterator.IsValid())
      {
        int cubeIndex = 0;
        if (this.m_cache.Content(next.X, next.Y, next.Z) < (byte) 127)
          cubeIndex |= 1;
        if (this.m_cache.Content(next.X + 1, next.Y, next.Z) < (byte) 127)
          cubeIndex |= 2;
        if (this.m_cache.Content(next.X + 1, next.Y, next.Z + 1) < (byte) 127)
          cubeIndex |= 4;
        if (this.m_cache.Content(next.X, next.Y, next.Z + 1) < (byte) 127)
          cubeIndex |= 8;
        if (this.m_cache.Content(next.X, next.Y + 1, next.Z) < (byte) 127)
          cubeIndex |= 16;
        if (this.m_cache.Content(next.X + 1, next.Y + 1, next.Z) < (byte) 127)
          cubeIndex |= 32;
        if (this.m_cache.Content(next.X + 1, next.Y + 1, next.Z + 1) < (byte) 127)
          cubeIndex |= 64;
        if (this.m_cache.Content(next.X, next.Y + 1, next.Z + 1) < (byte) 127)
          cubeIndex |= 128;
        if (MyMarchingCubesConstants.EdgeTable[cubeIndex] != 0)
        {
          Vector3I temporaryVoxelData = this.ComputeTemporaryVoxelData(this.m_cache, ref next, cubeIndex, lod);
          this.CreateTriangles(ref next, cubeIndex, ref temporaryVoxelData);
        }
        vector3IRangeIterator.GetNext(out next);
      }
      Vector3I size3D = this.m_cache.Size3D;
      Vector3I vector3I = voxelStart - this.AffectedRangeOffset;
      MyIsoMesh myIsoMesh1 = new MyIsoMesh();
      for (int index = 0; index < this.m_resultVerticesCounter; ++index)
      {
        Vector3 vector3 = (this.m_resultVertices[index].Position - (Vector3) storage.Size / 2f) / (Vector3) storage.Size;
        this.m_resultVertices[index].Position = vector3;
      }
      for (int index = 0; index < this.m_resultVerticesCounter; ++index)
        myIsoMesh1.WriteVertex(ref this.m_resultVertices[index].Cell, ref this.m_resultVertices[index].Position, ref this.m_resultVertices[index].Normal, (byte) this.m_resultVertices[index].Material, 0U);
      for (int index = 0; index < this.m_resultTrianglesCounter; ++index)
        myIsoMesh1.WriteTriangle((int) this.m_resultTriangles[index].V0, (int) this.m_resultTriangles[index].V1, (int) this.m_resultTriangles[index].V2);
      MyIsoMesh myIsoMesh2 = myIsoMesh1;
      myIsoMesh2.PositionOffset = (Vector3D) (storage.Size / 2);
      myIsoMesh2.PositionScale = (Vector3) storage.Size;
      myIsoMesh2.CellStart = voxelStart;
      myIsoMesh2.CellEnd = voxelEnd;
      Vector3I[] internalArray = myIsoMesh2.Cells.GetInternalArray();
      for (int index = 0; index < myIsoMesh2.VerticesCount; ++index)
        internalArray[index] += vector3I;
      return myIsoMesh1;
    }

    private Vector3I ComputeTemporaryVoxelData(
      MyStorageData cache,
      ref Vector3I coord0,
      int cubeIndex,
      int lod)
    {
      int index = coord0.X + coord0.Y * 11 + coord0.Z * 121;
      MyMarchingCubesMesher.MyTemporaryVoxel temporaryVoxel1 = this.m_temporaryVoxels[index];
      MyMarchingCubesMesher.MyTemporaryVoxel temporaryVoxel2 = this.m_temporaryVoxels[index + 1];
      MyMarchingCubesMesher.MyTemporaryVoxel temporaryVoxel3 = this.m_temporaryVoxels[index + 1 + 121];
      MyMarchingCubesMesher.MyTemporaryVoxel temporaryVoxel4 = this.m_temporaryVoxels[index + 121];
      MyMarchingCubesMesher.MyTemporaryVoxel temporaryVoxel5 = this.m_temporaryVoxels[index + 11];
      MyMarchingCubesMesher.MyTemporaryVoxel temporaryVoxel6 = this.m_temporaryVoxels[index + 1 + 11];
      MyMarchingCubesMesher.MyTemporaryVoxel temporaryVoxel7 = this.m_temporaryVoxels[index + 1 + 11 + 121];
      MyMarchingCubesMesher.MyTemporaryVoxel temporaryVoxel8 = this.m_temporaryVoxels[index + 11 + 121];
      Vector3I coord1 = new Vector3I(coord0.X + 1, coord0.Y, coord0.Z);
      Vector3I coord2 = new Vector3I(coord0.X + 1, coord0.Y, coord0.Z + 1);
      Vector3I coord3 = new Vector3I(coord0.X, coord0.Y, coord0.Z + 1);
      Vector3I coord4 = new Vector3I(coord0.X, coord0.Y + 1, coord0.Z);
      Vector3I coord5 = new Vector3I(coord0.X + 1, coord0.Y + 1, coord0.Z);
      Vector3I coord6 = new Vector3I(coord0.X + 1, coord0.Y + 1, coord0.Z + 1);
      Vector3I coord7 = new Vector3I(coord0.X, coord0.Y + 1, coord0.Z + 1);
      Vector3I vector3I1 = coord0;
      Vector3I vector3I2 = coord1;
      Vector3I vector3I3 = coord2;
      Vector3I vector3I4 = coord3;
      Vector3I vector3I5 = coord4;
      Vector3I vector3I6 = coord5;
      Vector3I vector3I7 = coord6;
      Vector3I vector3I8 = coord7;
      temporaryVoxel1.IdxInCache = cache.ComputeLinear(ref vector3I1);
      temporaryVoxel2.IdxInCache = cache.ComputeLinear(ref vector3I2);
      temporaryVoxel3.IdxInCache = cache.ComputeLinear(ref vector3I3);
      temporaryVoxel4.IdxInCache = cache.ComputeLinear(ref vector3I4);
      temporaryVoxel5.IdxInCache = cache.ComputeLinear(ref vector3I5);
      temporaryVoxel6.IdxInCache = cache.ComputeLinear(ref vector3I6);
      temporaryVoxel7.IdxInCache = cache.ComputeLinear(ref vector3I7);
      temporaryVoxel8.IdxInCache = cache.ComputeLinear(ref vector3I8);
      temporaryVoxel1.Position.X = (float) (this.m_voxelStart.X + coord0.X) * this.m_voxelSizeInMeters;
      temporaryVoxel1.Position.Y = (float) (this.m_voxelStart.Y + coord0.Y) * this.m_voxelSizeInMeters;
      temporaryVoxel1.Position.Z = (float) (this.m_voxelStart.Z + coord0.Z) * this.m_voxelSizeInMeters;
      temporaryVoxel2.Position.X = temporaryVoxel1.Position.X + this.m_voxelSizeInMeters;
      temporaryVoxel2.Position.Y = temporaryVoxel1.Position.Y;
      temporaryVoxel2.Position.Z = temporaryVoxel1.Position.Z;
      temporaryVoxel3.Position.X = temporaryVoxel1.Position.X + this.m_voxelSizeInMeters;
      temporaryVoxel3.Position.Y = temporaryVoxel1.Position.Y;
      temporaryVoxel3.Position.Z = temporaryVoxel1.Position.Z + this.m_voxelSizeInMeters;
      temporaryVoxel4.Position.X = temporaryVoxel1.Position.X;
      temporaryVoxel4.Position.Y = temporaryVoxel1.Position.Y;
      temporaryVoxel4.Position.Z = temporaryVoxel1.Position.Z + this.m_voxelSizeInMeters;
      temporaryVoxel5.Position.X = temporaryVoxel1.Position.X;
      temporaryVoxel5.Position.Y = temporaryVoxel1.Position.Y + this.m_voxelSizeInMeters;
      temporaryVoxel5.Position.Z = temporaryVoxel1.Position.Z;
      temporaryVoxel6.Position.X = temporaryVoxel1.Position.X + this.m_voxelSizeInMeters;
      temporaryVoxel6.Position.Y = temporaryVoxel1.Position.Y + this.m_voxelSizeInMeters;
      temporaryVoxel6.Position.Z = temporaryVoxel1.Position.Z;
      temporaryVoxel7.Position.X = temporaryVoxel1.Position.X + this.m_voxelSizeInMeters;
      temporaryVoxel7.Position.Y = temporaryVoxel1.Position.Y + this.m_voxelSizeInMeters;
      temporaryVoxel7.Position.Z = temporaryVoxel1.Position.Z + this.m_voxelSizeInMeters;
      temporaryVoxel8.Position.X = temporaryVoxel1.Position.X;
      temporaryVoxel8.Position.Y = temporaryVoxel1.Position.Y + this.m_voxelSizeInMeters;
      temporaryVoxel8.Position.Z = temporaryVoxel1.Position.Z + this.m_voxelSizeInMeters;
      this.GetVoxelNormal(temporaryVoxel1, ref coord0, ref vector3I1, temporaryVoxel1);
      this.GetVoxelNormal(temporaryVoxel2, ref coord1, ref vector3I2, temporaryVoxel1);
      this.GetVoxelNormal(temporaryVoxel3, ref coord2, ref vector3I3, temporaryVoxel1);
      this.GetVoxelNormal(temporaryVoxel4, ref coord3, ref vector3I4, temporaryVoxel1);
      this.GetVoxelNormal(temporaryVoxel5, ref coord4, ref vector3I5, temporaryVoxel1);
      this.GetVoxelNormal(temporaryVoxel6, ref coord5, ref vector3I6, temporaryVoxel1);
      this.GetVoxelNormal(temporaryVoxel7, ref coord6, ref vector3I7, temporaryVoxel1);
      this.GetVoxelNormal(temporaryVoxel8, ref coord7, ref vector3I8, temporaryVoxel1);
      this.GetVoxelAmbient(temporaryVoxel1, ref coord0, ref vector3I1);
      this.GetVoxelAmbient(temporaryVoxel2, ref coord1, ref vector3I2);
      this.GetVoxelAmbient(temporaryVoxel3, ref coord2, ref vector3I3);
      this.GetVoxelAmbient(temporaryVoxel4, ref coord3, ref vector3I4);
      this.GetVoxelAmbient(temporaryVoxel5, ref coord4, ref vector3I5);
      this.GetVoxelAmbient(temporaryVoxel6, ref coord5, ref vector3I6);
      this.GetVoxelAmbient(temporaryVoxel7, ref coord6, ref vector3I7);
      this.GetVoxelAmbient(temporaryVoxel8, ref coord7, ref vector3I8);
      int num = MyMarchingCubesConstants.EdgeTable[cubeIndex];
      if ((num & 1) == 1)
        this.GetVertexInterpolation(cache, temporaryVoxel1, temporaryVoxel2, 0);
      if ((num & 2) == 2)
        this.GetVertexInterpolation(cache, temporaryVoxel2, temporaryVoxel3, 1);
      if ((num & 4) == 4)
        this.GetVertexInterpolation(cache, temporaryVoxel3, temporaryVoxel4, 2);
      if ((num & 8) == 8)
        this.GetVertexInterpolation(cache, temporaryVoxel4, temporaryVoxel1, 3);
      if ((num & 16) == 16)
        this.GetVertexInterpolation(cache, temporaryVoxel5, temporaryVoxel6, 4);
      if ((num & 32) == 32)
        this.GetVertexInterpolation(cache, temporaryVoxel6, temporaryVoxel7, 5);
      if ((num & 64) == 64)
        this.GetVertexInterpolation(cache, temporaryVoxel7, temporaryVoxel8, 6);
      if ((num & 128) == 128)
        this.GetVertexInterpolation(cache, temporaryVoxel8, temporaryVoxel5, 7);
      if ((num & 256) == 256)
        this.GetVertexInterpolation(cache, temporaryVoxel1, temporaryVoxel5, 8);
      if ((num & 512) == 512)
        this.GetVertexInterpolation(cache, temporaryVoxel2, temporaryVoxel6, 9);
      if ((num & 1024) == 1024)
        this.GetVertexInterpolation(cache, temporaryVoxel3, temporaryVoxel7, 10);
      if ((num & 2048) == 2048)
        this.GetVertexInterpolation(cache, temporaryVoxel4, temporaryVoxel8, 11);
      return vector3I1;
    }

    private void ComputeSizeAndOrigin(int lodIdx, Vector3I storageSize)
    {
      this.m_voxelSizeInMeters = 1f * (float) (1 << lodIdx);
      this.m_sizeMinusOne = (storageSize >> lodIdx) - 1;
      this.m_originPosition = this.m_voxelStart * this.m_voxelSizeInMeters + 0.5f * this.m_voxelSizeInMeters;
    }

    private void CreateTriangles(ref Vector3I coord0, int cubeIndex, ref Vector3I tempVoxelCoord0)
    {
      MyVoxelVertex myVoxelVertex = new MyVoxelVertex();
      Vector3I vector3I = new Vector3I(coord0.X, coord0.Y, coord0.Z);
      for (int index1 = 0; MyMarchingCubesConstants.TriangleTable[cubeIndex, index1] != -1; index1 += 3)
      {
        int index2 = MyMarchingCubesConstants.TriangleTable[cubeIndex, index1];
        int index3 = MyMarchingCubesConstants.TriangleTable[cubeIndex, index1 + 1];
        int index4 = MyMarchingCubesConstants.TriangleTable[cubeIndex, index1 + 2];
        MyMarchingCubesMesher.MyEdge edge1 = this.m_edges[index2];
        MyMarchingCubesMesher.MyEdge edge2 = this.m_edges[index3];
        MyMarchingCubesMesher.MyEdge edge3 = this.m_edges[index4];
        Vector4I vector4I1 = MyMarchingCubesConstants.EdgeConversion[index2];
        Vector4I vector4I2 = MyMarchingCubesConstants.EdgeConversion[index3];
        Vector4I vector4I3 = MyMarchingCubesConstants.EdgeConversion[index4];
        MyMarchingCubesMesher.MyEdgeVertex myEdgeVertex1 = this.m_edgeVertex[vector3I.X + vector4I1.X][vector3I.Y + vector4I1.Y][vector3I.Z + vector4I1.Z][vector4I1.W];
        MyMarchingCubesMesher.MyEdgeVertex myEdgeVertex2 = this.m_edgeVertex[vector3I.X + vector4I2.X][vector3I.Y + vector4I2.Y][vector3I.Z + vector4I2.Z][vector4I2.W];
        MyMarchingCubesMesher.MyEdgeVertex myEdgeVertex3 = this.m_edgeVertex[vector3I.X + vector4I3.X][vector3I.Y + vector4I3.Y][vector3I.Z + vector4I3.Z][vector4I3.W];
        if (!this.IsWrongTriangle(ref new MyVoxelVertex()
        {
          Position = edge1.Position
        }, ref new MyVoxelVertex()
        {
          Position = edge2.Position
        }, ref new MyVoxelVertex()
        {
          Position = edge3.Position
        }))
        {
          if (myEdgeVertex1.CalcCounter != this.m_edgeVertexCalcCounter)
          {
            myEdgeVertex1.CalcCounter = this.m_edgeVertexCalcCounter;
            myEdgeVertex1.VertexIndex = (ushort) this.m_resultVerticesCounter;
            myVoxelVertex.Position = edge1.Position;
            myVoxelVertex.Normal = edge1.Normal;
            myVoxelVertex.Material = (int) edge1.Material;
            this.m_resultVertices[this.m_resultVerticesCounter] = myVoxelVertex;
            ++this.m_resultVerticesCounter;
          }
          if (myEdgeVertex2.CalcCounter != this.m_edgeVertexCalcCounter)
          {
            myEdgeVertex2.CalcCounter = this.m_edgeVertexCalcCounter;
            myEdgeVertex2.VertexIndex = (ushort) this.m_resultVerticesCounter;
            myVoxelVertex.Position = edge2.Position;
            myVoxelVertex.Normal = edge2.Normal;
            myVoxelVertex.Material = (int) edge2.Material;
            this.m_resultVertices[this.m_resultVerticesCounter] = myVoxelVertex;
            ++this.m_resultVerticesCounter;
          }
          if (myEdgeVertex3.CalcCounter != this.m_edgeVertexCalcCounter)
          {
            myEdgeVertex3.CalcCounter = this.m_edgeVertexCalcCounter;
            myEdgeVertex3.VertexIndex = (ushort) this.m_resultVerticesCounter;
            myVoxelVertex.Position = edge3.Position;
            myVoxelVertex.Normal = edge3.Normal;
            myVoxelVertex.Material = (int) edge3.Material;
            myVoxelVertex.Cell = coord0;
            this.m_resultVertices[this.m_resultVerticesCounter] = myVoxelVertex;
            ++this.m_resultVerticesCounter;
          }
          this.m_resultTriangles[this.m_resultTrianglesCounter].V0 = myEdgeVertex1.VertexIndex;
          this.m_resultTriangles[this.m_resultTrianglesCounter].V1 = myEdgeVertex2.VertexIndex;
          this.m_resultTriangles[this.m_resultTrianglesCounter].V2 = myEdgeVertex3.VertexIndex;
          ++this.m_resultTrianglesCounter;
        }
      }
    }

    private bool IsWrongTriangle(
      ref MyVoxelVertex edge0,
      ref MyVoxelVertex edge1,
      ref MyVoxelVertex edge2)
    {
      return MyUtils.IsWrongTriangle(edge0.Position, edge1.Position, edge2.Position);
    }

    private class MyEdgeVertex
    {
      public ushort VertexIndex;
      public int CalcCounter;
    }

    private class MyEdge
    {
      public Vector3 Position;
      public Vector3 Normal;
      public float Ambient;
      public byte Material;
    }

    private class MyTemporaryVoxel
    {
      public int IdxInCache;
      public Vector3 Position;
      public Vector3 Normal;
      public float Ambient;
      public int Normal_CalcCounter;
      public int Ambient_CalcCounter;
    }
  }
}
