// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyPlanetStorageProvider
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Voxels.Planet;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment;
using System;
using System.Collections.Generic;
using System.IO;
using VRage.Game;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
  [MyStorageDataProvider(10042)]
  public class MyPlanetStorageProvider : IMyStorageDataProvider
  {
    private static readonly int STORAGE_VERSION = 1;
    private MyPlanetStorageProvider.PlanetData m_data;
    private string m_path;

    public MyPlanetGeneratorDefinition Generator { get; private set; }

    public int SerializedSize => sizeof (MyPlanetStorageProvider.PlanetData) + this.Generator.Id.SubtypeName.Get7bitEncodedSize();

    public void WriteTo(Stream stream)
    {
      stream.WriteNoAlloc(this.m_data.Version);
      stream.WriteNoAlloc(this.m_data.Seed);
      stream.WriteNoAlloc(this.m_data.Radius);
      stream.WriteNoAlloc(this.Generator.Id.SubtypeName);
    }

    public void ReadFrom(int storageVersion, Stream stream, int size, ref bool isOldFormat)
    {
      this.m_data.Version = stream.ReadInt64();
      this.m_data.Seed = stream.ReadInt64();
      this.m_data.Radius = stream.ReadDouble();
      string str = stream.ReadString();
      if (this.m_data.Version != (long) MyPlanetStorageProvider.STORAGE_VERSION)
        isOldFormat = true;
      this.Generator = MyDefinitionManager.Static.GetDefinition<MyPlanetGeneratorDefinition>(MyStringHash.GetOrCompute(str)) ?? throw new Exception(string.Format("Cannot load planet generator definition for subtype '{0}'.", (object) str));
      this.Init(this.m_data.Seed, true);
    }

    public bool Closed { get; private set; }

    public MyPlanetShapeProvider Shape { get; private set; }

    public MyPlanetMaterialProvider Material { get; private set; }

    public Vector3I StorageSize { get; private set; }

    public float Radius => (float) this.m_data.Radius;

    public void Init(
      long seed,
      MyPlanetGeneratorDefinition generator,
      double radius,
      bool loadTextures)
    {
      radius = Math.Max(radius, 1.0);
      this.Generator = generator;
      this.m_data = new MyPlanetStorageProvider.PlanetData()
      {
        Radius = radius,
        Seed = seed,
        Version = (long) MyPlanetStorageProvider.STORAGE_VERSION
      };
      this.Init(seed, loadTextures);
      this.Closed = false;
    }

    private void Init(long seed, bool loadTextures)
    {
      float radius = (float) this.m_data.Radius;
      float num1 = radius * this.Generator.HillParams.Max;
      this.StorageSize = MyVoxelCoordSystems.FindBestOctreeSize(2f * (radius + num1));
      float num2 = (float) this.StorageSize.X * 0.5f;
      MyPlanetTextureMapProvider texProvider = new MyPlanetTextureMapProvider();
      texProvider.Init(seed, this.Generator, this.Generator.MapProvider);
      this.m_path = texProvider.TexturePath;
      if (loadTextures)
      {
        string errorMessage;
        if (!MyPlanets.Static.CanSpawnPlanet(this.Generator, true, out errorMessage))
          throw new MyPlanetWhitelistException(errorMessage);
        this.Shape = new MyPlanetShapeProvider(new Vector3(num2), radius, this.Generator, texProvider.GetHeightmap(), texProvider);
        this.Material = new MyPlanetMaterialProvider(this.Generator, this.Shape, texProvider.GetMaps(this.Generator.PlanetMaps.ToSet()));
        MyHeightMapLoadingSystem component = MySession.Static.GetComponent<MyHeightMapLoadingSystem>();
        component.RetainTilesetMap(this.Generator.MaterialBlending.Texture);
        component.RetainHeightMap(this.m_path);
        component.RetainPlanetMap(this.m_path);
      }
      else
        this.Shape = new MyPlanetShapeProvider(new Vector3(num2), radius, this.Generator);
    }

    public void Close()
    {
      if (this.Material != null)
      {
        MyHeightMapLoadingSystem component = MySession.Static.GetComponent<MyHeightMapLoadingSystem>();
        component.ReleaseTilesetMap(this.Generator.MaterialBlending.Texture);
        component.ReleaseHeightMap(this.m_path);
        component.ReleasePlanetMap(this.m_path);
        this.Material.Close();
      }
      this.Shape.Close();
      this.Closed = true;
    }

    public unsafe void PostProcess(VrVoxelMesh mesh, MyStorageDataTypeFlags dataTypes)
    {
      if (!dataTypes.Requests(MyStorageDataTypeEnum.Material))
        return;
      VrVoxelVertex* vertices = mesh.Vertices;
      int vertexCount = mesh.VertexCount;
      Vector3 start = (Vector3) mesh.Start;
      float scale = mesh.Scale;
      for (int index = 0; index < vertexCount; ++index)
      {
        Vector3 colorShift = this.Material.GetColorShift((start + vertices[index].Position) * scale, vertices[index].Material, 1024f);
        if (colorShift != Vector3.Zero)
          vertices[index].Color.PackedValue = MyPlanetStorageProvider.PackColorShift(colorShift * new Vector3(360f, 100f, 100f));
      }
    }

    private static uint PackColorShift(Vector3 hsv)
    {
      int x = (int) hsv.X;
      int y = (int) hsv.Y;
      int z = (int) hsv.Z;
      return (uint) (((int) ushort.MaxValue & x % 360) << 16 | ((int) byte.MaxValue & MathHelper.Clamp(y, -100, 100)) << 8 | (int) byte.MaxValue & MathHelper.Clamp(z, -100, 100));
    }

    public void ReadRange(
      MyStorageData target,
      MyStorageDataTypeFlags dataType,
      ref Vector3I writeOffset,
      int lodIndex,
      ref Vector3I minInLod,
      ref Vector3I maxInLod)
    {
      if (this.Closed)
        return;
      MyVoxelDataRequest req = new MyVoxelDataRequest()
      {
        Target = target,
        Offset = writeOffset,
        RequestedData = dataType,
        Lod = lodIndex,
        MinInLod = minInLod,
        MaxInLod = maxInLod
      };
      this.ReadRange(ref req, false);
    }

    public MyVoxelMaterialDefinition GetMaterialAtPosition(
      ref Vector3D localPosition)
    {
      if (this.Closed)
        return (MyVoxelMaterialDefinition) null;
      Vector3 pos = (Vector3) localPosition;
      return this.Material.GetMaterialForPosition(ref pos, 1f);
    }

    public void ReadRange(ref MyVoxelDataRequest req, bool detectOnly = false)
    {
      if (this.Closed)
        return;
      if (req.RequestedData.Requests(MyStorageDataTypeEnum.Content))
      {
        this.Shape.ReadContentRange(ref req, detectOnly);
        req.RequestFlags |= MyVoxelRequestFlags.ConsiderContent;
      }
      if (req.Flags.HasFlags(MyVoxelRequestFlags.EmptyData))
      {
        if (detectOnly || !req.RequestedData.Requests(MyStorageDataTypeEnum.Material))
          return;
        req.Target.BlockFill(MyStorageDataTypeEnum.Material, req.MinInLod, req.MaxInLod, byte.MaxValue);
      }
      else
      {
        if (!req.RequestedData.Requests(MyStorageDataTypeEnum.Material))
          return;
        this.Material.ReadMaterialRange(ref req, detectOnly);
      }
    }

    public ContainmentType Intersect(BoundingBoxI box, int lod)
    {
      if (this.Closed)
        return ContainmentType.Disjoint;
      BoundingBox box1 = new BoundingBox(box);
      box1.Translate(-this.Shape.Center());
      return this.Shape.IntersectBoundingBox(ref box1, 1f);
    }

    public bool Intersect(ref LineD line, out double startOffset, out double endOffset)
    {
      LineD ll = line;
      Vector3 vector3 = this.Shape.Center();
      ll.To -= vector3;
      ll.From -= vector3;
      if (!this.Shape.IntersectLine(ref ll, out startOffset, out endOffset))
        return false;
      ll.From += vector3;
      ll.To += vector3;
      line = ll;
      return true;
    }

    public void DebugDraw(ref MatrixD worldMatrix)
    {
    }

    public void ReindexMaterials(Dictionary<byte, byte> oldToNewIndexMap)
    {
    }

    public void ComputeCombinedMaterialAndSurface(
      Vector3 position,
      bool useCache,
      out MySurfaceParams props)
    {
      if (this.Closed)
      {
        props = new MySurfaceParams();
      }
      else
      {
        position -= this.Shape.Center();
        float num = position.Length();
        MyPlanetMaterialProvider.MaterialSampleParams ps;
        ps.Gravity = position / num;
        props.Latitude = ps.Gravity.Y;
        Vector2 vector2 = new Vector2(-ps.Gravity.X, -ps.Gravity.Z);
        vector2.Normalize();
        props.Longitude = vector2.Y;
        if (-(double) ps.Gravity.X > 0.0)
          props.Longitude = 2f - props.Longitude;
        int face;
        Vector2 texCoord;
        MyCubemapHelpers.CalculateSampleTexcoord(ref position, out face, out texCoord);
        float altitude = useCache ? this.Shape.GetValueForPositionWithCache(face, ref texCoord, out props.Normal) : this.Shape.GetValueForPositionCacheless(face, ref texCoord, out props.Normal);
        ps.SampledHeight = altitude;
        ps.SurfaceDepth = 0.0f;
        ps.Texcoord = texCoord;
        ps.LodSize = 1f;
        ps.Latitude = props.Latitude;
        ps.Longitude = props.Longitude;
        ps.Face = face;
        ps.Normal = props.Normal;
        props.Position = ps.Gravity * (this.Radius + altitude) + this.Shape.Center();
        props.Gravity = ps.Gravity = -ps.Gravity;
        ps.DistanceToCenter = props.Position.Length();
        MyPlanetMaterialProvider.PlanetMaterial materialForPosition = this.Material.GetLayeredMaterialForPosition(ref ps, out props.Biome);
        props.Material = materialForPosition.FirstOrDefault != null ? materialForPosition.FirstOrDefault.Index : (byte) 0;
        props.Normal = ps.Normal;
        props.HeightRatio = this.Shape.AltitudeToRatio(altitude);
      }
    }

    public void ComputeCombinedMaterialAndSurfaceExtended(
      Vector3 position,
      out MyPlanetStorageProvider.SurfacePropertiesExtended props)
    {
      if (this.Closed)
        props = new MyPlanetStorageProvider.SurfacePropertiesExtended();
      else
        this.Material.GetMaterialForPositionDebug(ref position, out props);
    }

    private struct PlanetData
    {
      public long Version;
      public long Seed;
      public double Radius;
    }

    public struct SurfacePropertiesExtended
    {
      public Vector3 Position;
      public Vector3 Gravity;
      public MyVoxelMaterialDefinition Material;
      public float Slope;
      public float HeightRatio;
      public float Depth;
      public float GroundHeight;
      public float Latitude;
      public float Longitude;
      public float Altitude;
      public int Face;
      public Vector2 Texcoord;
      public byte BiomeValue;
      public byte MaterialValue;
      public byte OreValue;
      public MyPlanetMaterialProvider.PlanetMaterial EffectiveRule;
      public MyPlanetMaterialProvider.PlanetBiome Biome;
      public MyPlanetMaterialProvider.PlanetOre Ore;
      public MyPlanetStorageProvider.SurfacePropertiesExtended.MaterialOrigin Origin;

      public enum MaterialOrigin
      {
        Rule,
        Ore,
        Map,
        Default,
      }
    }
  }
}
