// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyPlanetShapeProvider
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Voxels.Planet;
using System;
using VRage.Game;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
  public class MyPlanetShapeProvider
  {
    private const int MAX_UNCULLED_HISTORY = 10;
    private static Matrix CR;
    private static Matrix CRT;
    private static Matrix BInv;
    private static Matrix BInvT;
    private static float Tau;
    public static MyDebugHitCounter PruningStats = new MyDebugHitCounter();
    public static MyDebugHitCounter CacheStats = new MyDebugHitCounter();
    public static MyDebugHitCounter CullStats = new MyDebugHitCounter();
    private readonly int m_mapResolutionMinusOne;
    private readonly float m_radius;
    private string m_dataFileName;
    private MyHeightCubemap m_heightmap;
    private VrPlanetShape m_nativeShape;
    private MyPlanetShapeProvider.SurfaceDetailSampler m_detail;
    private float m_detailSlopeRecip;
    private float m_detailFade;
    private readonly Vector3 m_translation;
    private readonly float m_maxHillHeight;
    private readonly float m_minHillHeight;
    private readonly float m_heightRatio;
    private readonly float m_heightRatioRecip;
    private float m_detailScale;
    private readonly float m_pixelSize;
    private readonly float m_pixelSizeRecip2;
    private readonly float m_mapStepScale;
    private readonly float m_mapStepScaleSquare;
    private float m_mapHeightScale;
    [ThreadStatic]
    private static MyPlanetShapeProvider.Cache m_cache;
    private const bool EnableBezierCull = true;
    [ThreadStatic]
    private static Matrix s_Cz;
    private const bool ForceBilinear = false;

    static MyPlanetShapeProvider()
    {
      MyPlanetShapeProvider.SetTau(0.5f);
      MyPlanetShapeProvider.BInvT = new Matrix(1f, 1f, 1f, 1f, 0.0f, 0.3333333f, 0.6666667f, 1f, 0.0f, 0.0f, 0.3333333f, 1f, 0.0f, 0.0f, 0.0f, 1f);
      MyPlanetShapeProvider.BInv = Matrix.Transpose(MyPlanetShapeProvider.BInvT);
    }

    public static void SetTau(float tau)
    {
      MyPlanetShapeProvider.Tau = tau;
      MyPlanetShapeProvider.CRT = new Matrix(0.0f, -MyPlanetShapeProvider.Tau, 2f * MyPlanetShapeProvider.Tau, -MyPlanetShapeProvider.Tau, 1f, 0.0f, MyPlanetShapeProvider.Tau - 3f, 2f - MyPlanetShapeProvider.Tau, 0.0f, MyPlanetShapeProvider.Tau, (float) (3.0 - 2.0 * (double) MyPlanetShapeProvider.Tau), MyPlanetShapeProvider.Tau - 2f, 0.0f, 0.0f, -MyPlanetShapeProvider.Tau, MyPlanetShapeProvider.Tau);
      MyPlanetShapeProvider.CR = Matrix.Transpose(MyPlanetShapeProvider.CRT);
    }

    public static float GetTau() => MyPlanetShapeProvider.Tau;

    public float OuterRadius { get; private set; }

    public float InnerRadius { get; private set; }

    internal Vector3 Center() => this.m_translation;

    public bool Closed { get; private set; }

    public float Radius => this.m_radius;

    public float HeightRatio => this.m_heightRatio;

    public float MinHillHeight => this.m_minHillHeight;

    public float MaxHillHeight => this.m_maxHillHeight;

    public MyHeightCubemap Heightmap => this.m_heightmap;

    public MyPlanetShapeProvider(
      Vector3 translation,
      float radius,
      MyPlanetGeneratorDefinition definition,
      MyHeightCubemap cubemap,
      MyPlanetTextureMapProvider texProvider)
    {
      this.m_radius = radius;
      this.m_translation = translation;
      this.m_maxHillHeight = definition.HillParams.Max * this.m_radius;
      this.m_minHillHeight = definition.HillParams.Min * this.m_radius;
      this.InnerRadius = radius + this.m_minHillHeight;
      this.OuterRadius = radius + this.m_maxHillHeight;
      this.m_heightmap = cubemap;
      this.m_mapResolutionMinusOne = this.m_heightmap.Resolution - 1;
      this.m_heightRatio = this.m_maxHillHeight - this.m_minHillHeight;
      this.m_heightRatioRecip = 1f / this.m_heightRatio;
      float faceSize = (float) ((double) radius * Math.PI * 0.5);
      this.m_pixelSize = faceSize / (float) this.m_heightmap.Resolution;
      this.m_pixelSizeRecip2 = 0.5f / this.m_pixelSize;
      this.m_mapStepScale = this.m_pixelSize / this.m_heightRatio;
      this.m_mapStepScaleSquare = this.m_mapStepScale * this.m_mapStepScale;
      if (definition.Detail != null)
        this.m_detail.Init(texProvider, definition.Detail, faceSize);
      VrPlanetShape.DetailMapData detailMapData = new VrPlanetShape.DetailMapData();
      if (definition.Detail != null)
        detailMapData = this.m_detail.GetDetailMapData();
      VrPlanetShape.Mapset mapset = this.m_heightmap.GetMapset();
      this.m_nativeShape = new VrPlanetShape(translation, radius, definition.HillParams.Min, definition.HillParams.Max, mapset, detailMapData, true);
      this.Closed = false;
    }

    public MyPlanetShapeProvider(
      Vector3 translation,
      float radius,
      MyPlanetGeneratorDefinition definition)
    {
      this.m_radius = radius;
      this.m_translation = translation;
      this.m_maxHillHeight = definition.HillParams.Max * this.m_radius;
      this.m_minHillHeight = definition.HillParams.Min * this.m_radius;
      this.InnerRadius = radius + this.m_minHillHeight;
      this.OuterRadius = radius + this.m_maxHillHeight;
    }

    public void Close()
    {
      this.m_nativeShape?.Dispose();
      this.m_detail.Dispose();
      this.m_nativeShape = (VrPlanetShape) null;
      this.m_heightmap = (MyHeightCubemap) null;
      this.Closed = true;
    }

    public void PrepareCache()
    {
      if (!(this.m_heightmap.Name != MyPlanetShapeProvider.m_cache.Name))
        return;
      MyPlanetShapeProvider.m_cache.Name = this.m_heightmap.Name;
      MyPlanetShapeProvider.m_cache.Clean();
    }

    private unsafe void CalculateCacheCell(
      MyHeightmapFace map,
      MyPlanetShapeProvider.Cache.Cell* cell,
      bool compouteBounds = false)
    {
      int x = cell->Coord.X;
      int y = cell->Coord.Y;
      fixed (float* values = &MyPlanetShapeProvider.s_Cz.M11)
      {
        int linearOfft1 = map.GetRowStart(y - 1) + x - 1;
        ushort* data = map.Data;
        map.Get4Row(linearOfft1, values, data);
        int linearOfft2 = linearOfft1 + map.RowStride;
        map.Get4Row(linearOfft2, values + 4, data);
        int linearOfft3 = linearOfft2 + map.RowStride;
        map.Get4Row(linearOfft3, values + 8, data);
        int linearOfft4 = linearOfft3 + map.RowStride;
        map.Get4Row(linearOfft4, values + 12, data);
        Matrix.Multiply(ref MyPlanetShapeProvider.CR, ref MyPlanetShapeProvider.s_Cz, out cell->Gz);
        Matrix.Multiply(ref cell->Gz, ref MyPlanetShapeProvider.CRT, out cell->Gz);
        if (compouteBounds)
        {
          float num1 = float.PositiveInfinity;
          float num2 = float.NegativeInfinity;
          Matrix result;
          Matrix.Multiply(ref MyPlanetShapeProvider.BInv, ref cell->Gz, out result);
          Matrix.Multiply(ref result, ref MyPlanetShapeProvider.BInvT, out result);
          float* numPtr = &result.M11;
          for (int index = 0; index < 16; ++index)
          {
            if ((double) num2 < (double) numPtr[index])
              num2 = numPtr[index];
            if ((double) num1 > (double) numPtr[index])
              num1 = numPtr[index];
          }
          cell->Max = num2;
          cell->Min = num1;
        }
        else
        {
          cell->Max = 1f;
          cell->Min = 0.0f;
        }
      }
    }

    private float SampleHeightBicubic(float s, float t, ref Matrix Gz, out Vector3 Normal)
    {
      float num1 = s * s;
      float num2 = num1 * s;
      float num3 = t * t;
      float num4 = num3 * t;
      float num5 = (float) ((double) Gz.M12 + (double) Gz.M22 * (double) t + (double) Gz.M32 * (double) num3 + (double) Gz.M42 * (double) num4);
      float num6 = (float) ((double) Gz.M13 + (double) Gz.M23 * (double) t + (double) Gz.M33 * (double) num3 + (double) Gz.M43 * (double) num4);
      float num7 = (float) ((double) Gz.M14 + (double) Gz.M24 * (double) t + (double) Gz.M34 * (double) num3 + (double) Gz.M44 * (double) num4);
      double num8 = (double) Gz.M11 + (double) Gz.M21 * (double) t + (double) Gz.M31 * (double) num3 + (double) Gz.M41 * (double) num4 + (double) s * (double) num5 + (double) num1 * (double) num6 + (double) num2 * (double) num7;
      float x = (float) ((double) num5 + 2.0 * (double) s * (double) num6 + 3.0 * (double) num1 * (double) num7);
      float y = (float) ((double) Gz.M21 + (double) Gz.M22 * (double) s + (double) Gz.M23 * (double) num1 + (double) Gz.M24 * (double) num2 + 2.0 * (double) t * ((double) Gz.M31 + (double) Gz.M32 * (double) s + (double) Gz.M33 * (double) num1 + (double) Gz.M34 * (double) num2) + 3.0 * (double) num3 * ((double) Gz.M41 + (double) Gz.M42 * (double) s + (double) Gz.M43 * (double) num1 + (double) Gz.M44 * (double) num2));
      Normal = new Vector3(x, y, this.m_mapStepScale);
      double num9 = (double) Normal.Normalize();
      return (float) num8;
    }

    private float SampleHeightBilinear(
      MyHeightmapFace map,
      float lodSize,
      float s,
      float t,
      int sx,
      int sy,
      out Vector3 Normal)
    {
      float num1 = lodSize * this.m_pixelSizeRecip2;
      float num2 = 1f - s;
      float num3 = 1f - t;
      int x = Math.Min(sx + (int) Math.Ceiling((double) num1), this.m_heightmap.Resolution);
      int y = Math.Min(sy + (int) Math.Ceiling((double) num1), this.m_heightmap.Resolution);
      float valuef1 = map.GetValuef(sx, sy);
      float valuef2 = map.GetValuef(x, sy);
      float valuef3 = map.GetValuef(sx, y);
      float valuef4 = map.GetValuef(x, y);
      double num4 = (double) valuef1 * (double) num2 * (double) num3 + (double) valuef2 * (double) s * (double) num3 + (double) valuef3 * (double) num2 * (double) t + (double) valuef4 * (double) s * (double) t;
      float num5 = (float) (((double) valuef2 - (double) valuef1) * (double) num3 + ((double) valuef4 - (double) valuef3) * (double) t);
      float num6 = (float) (((double) valuef3 - (double) valuef1) * (double) num2 + ((double) valuef4 - (double) valuef2) * (double) s);
      Normal = new Vector3(this.m_mapStepScale * num5, this.m_mapStepScale * num6, this.m_mapStepScaleSquare);
      double num7 = (double) Normal.Normalize();
      return (float) num4;
    }

    internal float SignedDistanceLocalCacheless(Vector3 position)
    {
      float num1 = position.Length();
      if ((double) num1 <= 0.1 || (double) num1 < (double) this.InnerRadius - 1.0)
        return -1f;
      if ((double) num1 > (double) this.OuterRadius + 1.0)
        return float.PositiveInfinity;
      double num2 = (double) num1 - (double) this.m_radius;
      int face;
      Vector2 texCoord;
      MyCubemapHelpers.CalculateSampleTexcoord(ref position, out face, out texCoord);
      Vector3 localNormal;
      float positionCacheless = this.GetValueForPositionCacheless(face, ref texCoord, out localNormal);
      if (this.m_detail.Matches(localNormal.Z))
      {
        float num3 = texCoord.X * this.m_detail.Factor;
        float num4 = texCoord.Y * this.m_detail.Factor;
        float dtx = num3 - (float) Math.Floor((double) num3);
        float dty = num4 - (float) Math.Floor((double) num4);
        positionCacheless += this.m_detail.GetValue(dtx, dty, localNormal.Z);
      }
      double num5 = (double) positionCacheless;
      return (float) (num2 - num5) * localNormal.Z;
    }

    public unsafe float GetValueForPositionCacheless(
      int face,
      ref Vector2 texcoord,
      out Vector3 localNormal)
    {
      if (this.m_heightmap == null)
      {
        localNormal = Vector3.Zero;
        return 0.0f;
      }
      Vector2 vector2 = texcoord * (float) this.m_mapResolutionMinusOne;
      if ((double) vector2.X >= (double) this.m_heightmap.Resolution || (double) vector2.Y >= (double) this.m_heightmap.Resolution || ((double) vector2.X < 0.0 || (double) vector2.Y < 0.0))
      {
        localNormal = Vector3.Zero;
        return 0.0f;
      }
      MyPlanetShapeProvider.Cache.Cell cell1;
      MyPlanetShapeProvider.Cache.Cell* cell2 = &cell1;
      cell2->Coord = new Vector3I((int) vector2.X, (int) vector2.Y, face);
      this.CalculateCacheCell(this.m_heightmap.Faces[face], cell2);
      return this.SampleHeightBicubic(vector2.X - (float) Math.Floor((double) vector2.X), vector2.Y - (float) Math.Floor((double) vector2.Y), ref cell2->Gz, out localNormal) * this.m_heightRatio + this.m_minHillHeight;
    }

    public float SignedDistanceWithSample(float lodVoxelSize, float distance, float value) => distance - this.m_radius - value;

    public bool ProjectToSurface(Vector3 localPos, out Vector3 surface)
    {
      float f = localPos.Length();
      if (f.IsZero())
      {
        surface = localPos;
        return false;
      }
      Vector3 vector3 = localPos / f;
      int face;
      Vector2 texCoord;
      MyCubemapHelpers.CalculateSampleTexcoord(ref localPos, out face, out texCoord);
      float num = this.GetValueForPositionCacheless(face, ref texCoord, out Vector3 _) + this.Radius;
      surface = num * vector3;
      return true;
    }

    public double GetDistanceToSurfaceWithCache(Vector3 localPos)
    {
      float f = localPos.Length();
      if (f.IsZero())
        return 0.0;
      int face;
      Vector2 texCoord;
      MyCubemapHelpers.CalculateSampleTexcoord(ref localPos, out face, out texCoord);
      float num = this.GetValueForPositionWithCache(face, ref texCoord, out Vector3 _) + this.Radius;
      return (double) f - (double) num;
    }

    public double GetDistanceToSurfaceCacheless(Vector3 localPos)
    {
      float f = localPos.Length();
      if (f.IsZero() || !f.IsValid())
        return 0.0;
      int face;
      Vector2 texCoord;
      MyCubemapHelpers.CalculateSampleTexcoord(ref localPos, out face, out texCoord);
      float num = this.GetValueForPositionCacheless(face, ref texCoord, out Vector3 _) + this.Radius;
      return (double) f - (double) num;
    }

    public unsafe float GetValueForPositionWithCache(
      int face,
      ref Vector2 texcoord,
      out Vector3 localNormal)
    {
      Vector2 vector2 = texcoord * (float) this.m_mapResolutionMinusOne;
      Vector3I coord = new Vector3I((int) vector2.X, (int) vector2.Y, face);
      fixed (MyPlanetShapeProvider.Cache.Cell* cell = &MyPlanetShapeProvider.m_cache.Cells[MyPlanetShapeProvider.m_cache.CellCoord(ref coord)])
      {
        if (cell->Coord != coord)
        {
          cell->Coord = coord;
          this.CalculateCacheCell(this.m_heightmap.Faces[face], cell);
        }
        return this.SampleHeightBicubic(vector2.X - (float) Math.Floor((double) vector2.X), vector2.Y - (float) Math.Floor((double) vector2.Y), ref cell->Gz, out localNormal) * this.m_heightRatio + this.m_minHillHeight;
      }
    }

    public float GetValue(int face, ref Vector2 texcoord, out Vector3 localNormal) => this.m_nativeShape.GetValue(ref texcoord, face, out localNormal);

    public float GetHeight(int face, ref Vector2 texcoord, out Vector3 localNormal) => this.m_nativeShape.GetValue(ref texcoord, face, out localNormal) * this.m_heightRatio + this.InnerRadius;

    internal unsafe float GetValueForPositionInternal(
      int face,
      ref Vector2 texcoord,
      float lodSize,
      float distance,
      out Vector3 Normal)
    {
      Vector2 vector2 = texcoord * (float) this.m_mapResolutionMinusOne;
      float s = vector2.X - (float) Math.Floor((double) vector2.X);
      float t = vector2.Y - (float) Math.Floor((double) vector2.Y);
      int x = (int) vector2.X;
      int y = (int) vector2.Y;
      float num1;
      if ((double) lodSize < (double) this.m_pixelSize)
      {
        Vector3I coord = new Vector3I(x, y, face);
        fixed (MyPlanetShapeProvider.Cache.Cell* cell = &MyPlanetShapeProvider.m_cache.Cells[MyPlanetShapeProvider.m_cache.CellCoord(ref coord)])
        {
          if (cell->Coord != coord)
          {
            cell->Coord = coord;
            this.CalculateCacheCell(this.m_heightmap.Faces[face], cell, true);
          }
          float num2 = (distance - this.InnerRadius) * this.m_heightRatioRecip;
          float num3 = lodSize * this.m_heightRatioRecip;
          if ((double) num2 > (double) cell->Max + (double) num3)
          {
            Normal = Vector3.Backward;
            return float.NegativeInfinity;
          }
          if ((double) num2 < (double) cell->Min - (double) num3)
          {
            Normal = Vector3.Backward;
            return float.PositiveInfinity;
          }
          num1 = this.SampleHeightBicubic(vector2.X - (float) Math.Floor((double) vector2.X), vector2.Y - (float) Math.Floor((double) vector2.Y), ref cell->Gz, out Normal);
          // ISSUE: __unpin statement
          __unpin(cell);
        }
      }
      else
        num1 = this.SampleHeightBilinear(this.m_heightmap.Faces[face], lodSize, s, t, x, y, out Normal);
      return num1 * this.m_heightRatio + this.m_minHillHeight;
    }

    internal float SignedDistanceLocal(Vector3 position, float lodVoxelSize)
    {
      float distance = position.Length();
      if ((double) distance <= 0.1 || (double) distance < (double) this.InnerRadius - (double) lodVoxelSize)
        return -lodVoxelSize;
      if ((double) distance > (double) this.OuterRadius + (double) lodVoxelSize)
        return float.PositiveInfinity;
      double num1 = (double) distance - (double) this.m_radius;
      int face;
      Vector2 texCoord;
      MyCubemapHelpers.CalculateSampleTexcoord(ref position, out face, out texCoord);
      Vector3 Normal;
      float positionInternal = this.GetValueForPositionInternal(face, ref texCoord, lodVoxelSize, distance, out Normal);
      if (this.m_detail.Matches(Normal.Z))
      {
        float num2 = texCoord.X * this.m_detail.Factor;
        float num3 = texCoord.Y * this.m_detail.Factor;
        float dtx = num2 - (float) Math.Floor((double) num2);
        float dty = num3 - (float) Math.Floor((double) num3);
        positionInternal += this.m_detail.GetValue(dtx, dty, Normal.Z);
      }
      double num4 = (double) positionInternal;
      return (float) (num1 - num4) * Normal.Z;
    }

    internal float SignedDistanceLocal(Vector3 position, float lodVoxelSize, int face)
    {
      float distance = position.Length();
      if ((double) distance <= 0.1 || (double) distance < (double) this.InnerRadius - (double) lodVoxelSize)
        return -lodVoxelSize;
      if ((double) distance > (double) this.OuterRadius + (double) lodVoxelSize)
        return float.PositiveInfinity;
      double num1 = (double) distance - (double) this.m_radius;
      Vector2 texCoord;
      MyCubemapHelpers.CalculateTexcoordForFace(ref position, face, out texCoord);
      Vector3 Normal;
      float positionInternal = this.GetValueForPositionInternal(face, ref texCoord, lodVoxelSize, distance, out Normal);
      if (this.m_detail.Matches(Normal.Z))
      {
        float num2 = texCoord.X * this.m_detail.Factor;
        float num3 = texCoord.Y * this.m_detail.Factor;
        float dtx = num2 - (float) Math.Floor((double) num2);
        float dty = num3 - (float) Math.Floor((double) num3);
        positionInternal += this.m_detail.GetValue(dtx, dty, Normal.Z);
      }
      double num4 = (double) positionInternal;
      return (float) (num1 - num4) * Normal.Z;
    }

    public float AltitudeToRatio(float altitude) => (altitude - this.m_minHillHeight) * this.m_heightRatioRecip;

    internal float DistanceToRatio(float distance) => (distance - this.InnerRadius) * this.m_heightRatioRecip;

    public ContainmentType IntersectBoundingBox(ref BoundingBox box, float lodLevel)
    {
      box.Inflate(1f);
      BoundingSphere boundingSphere = new BoundingSphere(Vector3.Zero, this.OuterRadius + lodLevel);
      bool result1;
      boundingSphere.Intersects(ref box, out result1);
      if (!result1)
        return ContainmentType.Disjoint;
      boundingSphere.Radius = this.InnerRadius - lodLevel;
      ContainmentType result2;
      boundingSphere.Contains(ref box, out result2);
      return result2 == ContainmentType.Contains ? ContainmentType.Contains : this.IntersectBoundingBoxInternal(ref box, lodLevel, out int _);
    }

    private unsafe ContainmentType IntersectBoundingBoxCornerCase(
      uint faces,
      Vector3* vertices,
      float minHeight,
      float maxHeight)
    {
      BoundingBox query = new BoundingBox(new Vector3(float.PositiveInfinity, float.PositiveInfinity, minHeight), new Vector3(float.NegativeInfinity, float.NegativeInfinity, maxHeight));
      query.Min.Z = (query.Min.Z - this.m_radius - this.m_detailScale - this.m_minHillHeight) * this.m_heightRatioRecip;
      query.Max.Z = (query.Max.Z - this.m_radius - this.m_minHillHeight) * this.m_heightRatioRecip;
      ContainmentType containmentType1 = ~ContainmentType.Disjoint;
      ContainmentType containmentType2 = containmentType1;
      for (int index1 = 0; index1 <= 5; ++index1)
      {
        if (((long) faces & (long) (1 << index1)) != 0L)
        {
          query.Min.X = float.PositiveInfinity;
          query.Min.Y = float.PositiveInfinity;
          query.Max.X = float.NegativeInfinity;
          query.Max.Y = float.NegativeInfinity;
          for (int index2 = 0; index2 < 8; ++index2)
          {
            Vector2 texcoord;
            MyCubemapHelpers.TexcoordCalculators[index1](ref vertices[index2], out texcoord);
            if ((double) texcoord.X < (double) query.Min.X)
              query.Min.X = texcoord.X;
            if ((double) texcoord.X > (double) query.Max.X)
              query.Max.X = texcoord.X;
            if ((double) texcoord.Y < (double) query.Min.Y)
              query.Min.Y = texcoord.Y;
            if ((double) texcoord.Y > (double) query.Max.Y)
              query.Max.Y = texcoord.Y;
          }
          ContainmentType containmentType3 = this.m_heightmap.Faces[index1].QueryHeight(ref query);
          if (containmentType3 != containmentType2)
          {
            if (containmentType2 != containmentType1)
              return ContainmentType.Intersects;
            containmentType2 = containmentType3;
          }
        }
      }
      return containmentType2;
    }

    protected unsafe ContainmentType IntersectBoundingBoxInternal(
      ref BoundingBox box,
      float lodLevel,
      out int boxFace)
    {
      int face1 = -1;
      uint faces = 0;
      bool flag = false;
      Vector3* vector3Ptr = stackalloc Vector3[8];
      box.GetCornersUnsafe(vector3Ptr);
      for (int index = 0; index < 8; ++index)
      {
        int face2;
        MyCubemapHelpers.GetCubeFace(ref vector3Ptr[index], out face2);
        if (face1 == -1)
          face1 = face2;
        else if (face1 != face2)
          flag = true;
        faces |= (uint) (1 << face2);
      }
      float num1 = !Vector3.Zero.IsInsideInclusive(ref box.Min, ref box.Max) ? Vector3.Clamp(Vector3.Zero, box.Min, box.Max).Length() : 0.0f;
      Vector3 center = box.Center;
      Vector3 vector3;
      vector3.X = (double) center.X >= 0.0 ? box.Max.X : box.Min.X;
      vector3.Y = (double) center.Y >= 0.0 ? box.Max.Y : box.Min.Y;
      vector3.Z = (double) center.Z >= 0.0 ? box.Max.Z : box.Min.Z;
      float num2 = vector3.Length();
      if (flag)
      {
        boxFace = -1;
        return this.IntersectBoundingBoxCornerCase(faces, vector3Ptr, num1, num2);
      }
      BoundingBox query = new BoundingBox(new Vector3(float.PositiveInfinity, float.PositiveInfinity, num1), new Vector3(float.NegativeInfinity, float.NegativeInfinity, num2));
      for (int index = 0; index < 8; ++index)
      {
        Vector2 texCoord;
        MyCubemapHelpers.CalculateTexcoordForFace(ref vector3Ptr[index], face1, out texCoord);
        if ((double) texCoord.X < (double) query.Min.X)
          query.Min.X = texCoord.X;
        if ((double) texCoord.X > (double) query.Max.X)
          query.Max.X = texCoord.X;
        if ((double) texCoord.Y < (double) query.Min.Y)
          query.Min.Y = texCoord.Y;
        if ((double) texCoord.Y > (double) query.Max.Y)
          query.Max.Y = texCoord.Y;
      }
      query.Min.Z = (query.Min.Z - this.m_radius - this.m_detailScale - this.m_minHillHeight) * this.m_heightRatioRecip;
      query.Max.Z = (query.Max.Z - this.m_radius - this.m_minHillHeight) * this.m_heightRatioRecip;
      boxFace = face1;
      return this.m_heightmap.Faces[face1].QueryHeight(ref query);
    }

    private bool IntersectLineCornerCase(
      ref LineD line,
      uint faces,
      out double startOffset,
      out double endOffset)
    {
      startOffset = 1.0;
      endOffset = 0.0;
      return true;
    }

    public bool IntersectLineFace(
      ref LineD ll,
      int face,
      out double startOffset,
      out double endOffset)
    {
      Vector3 from = (Vector3) ll.From;
      Vector3 to = (Vector3) ll.To;
      Vector2 texCoord1;
      MyCubemapHelpers.CalculateTexcoordForFace(ref from, face, out texCoord1);
      Vector2 texCoord2;
      MyCubemapHelpers.CalculateTexcoordForFace(ref to, face, out texCoord2);
      int num1 = (int) Math.Ceiling((double) (texCoord2 - texCoord1).Length() * (double) this.m_heightmap.Resolution);
      double num2 = 1.0 / (double) num1;
      for (int index = 0; index < num1; ++index)
      {
        Vector3 vector3_1 = (Vector3) (ll.From + ll.Direction * ll.Length * (double) index * num2);
        Vector3 vector3_2 = (Vector3) (ll.From + ll.Direction * ll.Length * (double) (index + 1) * num2);
        float num3 = vector3_1.Length();
        float num4 = vector3_2.Length();
        MyCubemapHelpers.CalculateTexcoordForFace(ref vector3_1, face, out texCoord1);
        MyCubemapHelpers.CalculateTexcoordForFace(ref vector3_2, face, out texCoord2);
        vector3_1.X = texCoord1.X;
        vector3_1.Y = texCoord1.Y;
        vector3_1.Z = (num3 - this.m_radius - this.m_minHillHeight) * this.m_heightRatioRecip;
        vector3_2.X = texCoord2.X;
        vector3_2.Y = texCoord2.Y;
        vector3_2.Z = (num4 - this.m_radius - this.m_minHillHeight) * this.m_heightRatioRecip;
        float startOffset1;
        if (this.m_heightmap[face].QueryLine(ref vector3_1, ref vector3_2, out startOffset1, out float _))
        {
          startOffset = Math.Max(((double) index + (double) startOffset1) * num2, 0.0);
          endOffset = 1.0;
          return true;
        }
      }
      startOffset = 0.0;
      endOffset = 1.0;
      return false;
    }

    public unsafe bool IntersectLine(ref LineD ll, out double startOffset, out double endOffset)
    {
      BoundingBox boundingBox = (BoundingBox) ll.GetBoundingBox();
      int face1 = -1;
      uint faces = 0;
      bool flag = false;
      Vector3* corners = stackalloc Vector3[8];
      boundingBox.GetCornersUnsafe(corners);
      for (int index = 0; index < 8; ++index)
      {
        int face2;
        MyCubemapHelpers.GetCubeFace(ref corners[index], out face2);
        if (face1 == -1)
          face1 = face2;
        else if (face1 != face2)
          flag = true;
        faces |= (uint) (1 << face2);
      }
      return flag ? this.IntersectLineCornerCase(ref ll, faces, out startOffset, out endOffset) : this.IntersectLineFace(ref ll, face1, out startOffset, out endOffset);
    }

    internal void ReadContentRange(ref MyVoxelDataRequest req, bool detectOnly = false)
    {
      if (this.Closed)
        return;
      float num = (float) (1 << req.Lod) * 1f;
      Vector3I minInLod = req.MinInLod;
      Vector3I maxInLod = req.MaxInLod;
      Vector3 vector3 = minInLod * num - this.m_translation;
      int boxFace = -1;
      MyVoxelRequestFlags voxelRequestFlags = req.RequestFlags & ~(MyVoxelRequestFlags.EmptyData | MyVoxelRequestFlags.FullContent | MyVoxelRequestFlags.ContentChecked | MyVoxelRequestFlags.ContentCheckedDeep);
      if ((req.MinInLod - req.MaxInLod).Size > 8)
      {
        BoundingBox box = new BoundingBox(vector3, vector3 + (maxInLod - minInLod) * num);
        box.Inflate(num);
        BoundingSphere boundingSphere = new BoundingSphere(Vector3.Zero, this.OuterRadius + num);
        bool result1;
        boundingSphere.Intersects(ref box, out result1);
        ContainmentType containmentType;
        if (!result1)
        {
          containmentType = ContainmentType.Disjoint;
        }
        else
        {
          boundingSphere.Radius = this.InnerRadius - num;
          ContainmentType result2;
          boundingSphere.Contains(ref box, out result2);
          if (result2 == ContainmentType.Contains)
          {
            containmentType = result2;
          }
          else
          {
            containmentType = this.IntersectBoundingBoxInternal(ref box, num, out boxFace);
            if (containmentType == ContainmentType.Intersects)
              goto label_7;
          }
        }
        if (containmentType == ContainmentType.Disjoint)
        {
          if (req.RequestFlags.HasFlags(MyVoxelRequestFlags.ContentChecked))
            voxelRequestFlags |= MyVoxelRequestFlags.EmptyData | MyVoxelRequestFlags.ContentChecked | MyVoxelRequestFlags.ContentCheckedDeep;
          else
            req.Target.BlockFillContent(req.Offset, req.Offset + maxInLod - minInLod, (byte) 0);
        }
        else if (containmentType == ContainmentType.Contains)
        {
          if (req.RequestFlags.HasFlags(MyVoxelRequestFlags.ContentChecked))
            voxelRequestFlags |= MyVoxelRequestFlags.FullContent | MyVoxelRequestFlags.ContentChecked | MyVoxelRequestFlags.ContentCheckedDeep;
          else
            req.Target.BlockFillContent(req.Offset, req.Offset + maxInLod - minInLod, byte.MaxValue);
        }
        req.Flags = voxelRequestFlags;
        return;
      }
label_7:
      if (detectOnly)
      {
        if (this.m_nativeShape != null)
        {
          switch (this.m_nativeShape.CheckContentRange(ref req.MinInLod, ref req.MaxInLod, num, boxFace))
          {
            case 0:
              voxelRequestFlags |= MyVoxelRequestFlags.EmptyData;
              break;
            case (int) byte.MaxValue:
              voxelRequestFlags |= MyVoxelRequestFlags.FullContent;
              break;
          }
        }
      }
      else if (this.m_nativeShape != null)
      {
        int linear = req.Target.ComputeLinear(ref req.Offset);
        Vector3I step = req.Target.Step;
        this.m_nativeShape.ReadContentRange(req.Target[MyStorageDataTypeEnum.Content], linear, ref step, ref req.MinInLod, ref req.MaxInLod, num, boxFace);
      }
      else
      {
        MyStorageData target = req.Target;
        Vector3I writeOffsetLoc = req.Offset - minInLod;
        this.CalculateDistanceFieldInternal(vector3, boxFace, minInLod, maxInLod, writeOffsetLoc, target, num);
      }
      req.Flags = voxelRequestFlags;
    }

    private void CalculateDistanceFieldInternal(
      Vector3 localPos,
      int faceHint,
      Vector3I min,
      Vector3I max,
      Vector3I writeOffsetLoc,
      MyStorageData target,
      float lodVoxelSize)
    {
      this.PrepareCache();
      Vector3 vector3 = localPos;
      if (faceHint == -1)
      {
        Vector3I vector3I;
        for (vector3I.Z = min.Z; vector3I.Z <= max.Z; ++vector3I.Z)
        {
          for (vector3I.Y = min.Y; vector3I.Y <= max.Y; ++vector3I.Y)
          {
            vector3I.X = min.X;
            Vector3I p = vector3I + writeOffsetLoc;
            int linear = target.ComputeLinear(ref p);
            for (; vector3I.X <= max.X; ++vector3I.X)
            {
              byte content = (byte) (((double) MathHelper.Clamp((float) -((double) this.SignedDistanceLocal(localPos, lodVoxelSize) / (double) lodVoxelSize), -1f, 1f) * 0.5 + 0.5) * (double) byte.MaxValue);
              target.Content(linear, content);
              linear += target.StepLinear;
              localPos.X += lodVoxelSize;
            }
            localPos.Y += lodVoxelSize;
            localPos.X = vector3.X;
          }
          localPos.Z += lodVoxelSize;
          localPos.Y = vector3.Y;
        }
      }
      else
      {
        Vector3I vector3I;
        for (vector3I.Z = min.Z; vector3I.Z <= max.Z; ++vector3I.Z)
        {
          for (vector3I.Y = min.Y; vector3I.Y <= max.Y; ++vector3I.Y)
          {
            vector3I.X = min.X;
            Vector3I p = vector3I + writeOffsetLoc;
            int linear = target.ComputeLinear(ref p);
            for (; vector3I.X <= max.X; ++vector3I.X)
            {
              byte content = (byte) (((double) MathHelper.Clamp((float) -((double) this.SignedDistanceLocal(localPos, lodVoxelSize, faceHint) / (double) lodVoxelSize), -1f, 1f) * 0.5 + 0.5) * (double) byte.MaxValue);
              target.Content(linear, content);
              linear += target.StepLinear;
              localPos.X += lodVoxelSize;
            }
            localPos.Y += lodVoxelSize;
            localPos.X = vector3.X;
          }
          localPos.Z += lodVoxelSize;
          localPos.Y = vector3.Y;
        }
      }
    }

    public unsafe void GetBounds(
      Vector3D* localPoints,
      int pointCount,
      out float minHeight,
      out float maxHeight)
    {
      int face1 = -1;
      for (int index = 0; index < pointCount; ++index)
      {
        Vector3 position = (Vector3) localPoints[index];
        int face2;
        MyCubemapHelpers.GetCubeFace(ref position, out face2);
        if (face1 == -1)
          face1 = face2;
      }
      BoundingBox query = new BoundingBox(new Vector3(float.PositiveInfinity, float.PositiveInfinity, 0.0f), new Vector3(float.NegativeInfinity, float.NegativeInfinity, 0.0f));
      for (int index = 0; index < pointCount; ++index)
      {
        Vector3 localPos = (Vector3) localPoints[index];
        Vector2 texCoord;
        MyCubemapHelpers.CalculateTexcoordForFace(ref localPos, face1, out texCoord);
        if ((double) texCoord.X < (double) query.Min.X)
          query.Min.X = texCoord.X;
        if ((double) texCoord.X > (double) query.Max.X)
          query.Max.X = texCoord.X;
        if ((double) texCoord.Y < (double) query.Min.Y)
          query.Min.Y = texCoord.Y;
        if ((double) texCoord.Y > (double) query.Max.Y)
          query.Max.Y = texCoord.Y;
      }
      this.m_heightmap.Faces[face1].GetBounds(ref query);
      minHeight = query.Min.Z * this.m_heightRatio + this.InnerRadius;
      maxHeight = query.Max.Z * this.m_heightRatio + this.InnerRadius;
    }

    public unsafe void GetBounds(
      Vector3* localPoints,
      int pointCount,
      out float minHeight,
      out float maxHeight)
    {
      int face1 = -1;
      for (int index = 0; index < pointCount; ++index)
      {
        int face2;
        MyCubemapHelpers.GetCubeFace(ref localPoints[index], out face2);
        if (face1 == -1)
          face1 = face2;
      }
      BoundingBox query = new BoundingBox(new Vector3(float.PositiveInfinity, float.PositiveInfinity, 0.0f), new Vector3(float.NegativeInfinity, float.NegativeInfinity, 0.0f));
      for (int index = 0; index < pointCount; ++index)
      {
        Vector2 texCoord;
        MyCubemapHelpers.CalculateTexcoordForFace(ref localPoints[index], face1, out texCoord);
        float f = texCoord.X * texCoord.Y;
        if (!float.IsNaN(f) && !float.IsInfinity(f))
        {
          if ((double) texCoord.X < (double) query.Min.X)
            query.Min.X = texCoord.X;
          if ((double) texCoord.X > (double) query.Max.X)
            query.Max.X = texCoord.X;
          if ((double) texCoord.Y < (double) query.Min.Y)
            query.Min.Y = texCoord.Y;
          if ((double) texCoord.Y > (double) query.Max.Y)
            query.Max.Y = texCoord.Y;
        }
      }
      this.m_heightmap.Faces[face1].GetBounds(ref query);
      minHeight = query.Min.Z * this.m_heightRatio + this.InnerRadius;
      maxHeight = query.Max.Z * this.m_heightRatio + this.InnerRadius;
    }

    public unsafe void GetBounds(ref BoundingBox box)
    {
      Vector3* vector3Ptr = stackalloc Vector3[8];
      box.GetCornersUnsafe(vector3Ptr);
      this.GetBounds(vector3Ptr, 8, out box.Min.Z, out box.Max.Z);
    }

    public void GetBounds(ref BoundingBox2 texcoordRange, int face, out float min, out float max)
    {
      BoundingBox query = new BoundingBox(new Vector3(texcoordRange.Min, 0.0f), new Vector3(texcoordRange.Max, 0.0f));
      this.m_heightmap.Faces[face].GetBounds(ref query);
      min = query.Min.Z;
      max = query.Max.Z;
    }

    private struct SurfaceDetailSampler : IDisposable
    {
      private MyHeightDetailTexture m_detail;
      public float Factor;
      public float Size;
      public float Scale;
      private float m_min;
      private float m_max;
      private float m_in;
      private float m_out;
      private float m_inRecip;
      private float m_outRecip;
      private float m_mid;

      public void Init(
        MyPlanetTextureMapProvider texProvider,
        MyPlanetSurfaceDetail def,
        float faceSize)
      {
        this.m_detail = texProvider.GetDetailMap(def.Texture);
        this.Size = def.Size;
        this.Factor = faceSize / this.Size;
        this.m_min = (float) Math.Cos((double) MathHelper.ToRadians(def.Slope.Max));
        this.m_max = (float) Math.Cos((double) MathHelper.ToRadians(def.Slope.Min));
        this.m_in = (float) Math.Cos((double) MathHelper.ToRadians(def.Slope.Max - def.Transition));
        this.m_out = (float) Math.Cos((double) MathHelper.ToRadians(def.Slope.Min + def.Transition));
        this.m_inRecip = (float) (1.0 / ((double) this.m_in - (double) this.m_min));
        this.m_outRecip = (float) (1.0 / ((double) this.m_max - (double) this.m_out));
        this.m_mid = (float) Math.Cos((double) MathHelper.ToRadians((float) (((double) def.Slope.Max + (double) def.Slope.Min) / 2.0)));
        this.Scale = def.Scale;
      }

      public bool Matches(float angle) => (double) angle <= (double) this.m_max && (double) angle >= (double) this.m_min;

      public float GetValue(float dtx, float dty, float angle)
      {
        if (this.m_detail == null)
          return 0.0f;
        float num = (double) angle <= (double) this.m_mid ? Math.Max(Math.Min((angle - this.m_in) * this.m_inRecip, 1f), 0.0f) : Math.Min(Math.Max((float) (1.0 - ((double) angle - (double) this.m_out) * (double) this.m_outRecip), 0.0f), 1f);
        return this.m_detail.GetValue(dtx, dty) * num * this.Scale;
      }

      public unsafe VrPlanetShape.DetailMapData GetDetailMapData() => new VrPlanetShape.DetailMapData()
      {
        Data = this.m_detail.Data,
        Factor = this.Factor,
        Size = this.Size,
        Resolution = (int) this.m_detail.Resolution,
        Scale = this.Scale,
        m_min = this.m_min,
        m_max = this.m_max,
        m_in = this.m_in,
        m_out = this.m_out,
        m_inRecip = this.m_inRecip,
        m_outRecip = this.m_outRecip,
        m_mid = this.m_mid
      };

      public void Dispose()
      {
        if (this.m_detail != null)
          this.m_detail.Dispose();
        this.m_detail = (MyHeightDetailTexture) null;
      }
    }

    private struct Cache
    {
      private const int CacheBits = 4;
      private const int CacheMask = 15;
      private const int CacheSize = 256;
      public MyPlanetShapeProvider.Cache.Cell[] Cells;
      public string Name;

      public int CellCoord(ref Vector3I coord) => (coord.Y & 15) << 4 | coord.X & 15;

      internal void Clean()
      {
        if (this.Cells == null)
          this.Cells = new MyPlanetShapeProvider.Cache.Cell[256];
        for (int index = 0; index < 256; ++index)
          this.Cells[index].Coord = new Vector3I(-1);
      }

      public struct Cell
      {
        public Matrix Gz;
        public float Min;
        public float Max;
        public Vector3I Coord;
      }
    }
  }
}
