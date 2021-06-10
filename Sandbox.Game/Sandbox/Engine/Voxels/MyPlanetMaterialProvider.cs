// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyPlanetMaterialProvider
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Noise;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
  public class MyPlanetMaterialProvider
  {
    private int m_mapResolutionMinusOne;
    private MyPlanetShapeProvider m_planetShape;
    private Dictionary<byte, MyPlanetMaterialProvider.PlanetMaterial> m_materials;
    private Dictionary<byte, MyPlanetMaterialProvider.PlanetBiome> m_biomes;
    private Dictionary<byte, List<MyPlanetMaterialProvider.PlanetOre>> m_ores;
    private MyPlanetMaterialProvider.PlanetMaterial m_defaultMaterial;
    private MyPlanetMaterialProvider.PlanetMaterial m_subsurfaceMaterial;
    private MyDynamicAABBTree m_stationTree = new MyDynamicAABBTree(Vector3.Zero, 0.0f);
    private MyCubemap m_materialMap;
    private MyCubemap m_biomeMap;
    private MyCubemap m_oreMap;
    private MyTileTexture<byte> m_blendingTileset;
    private MyPlanetGeneratorDefinition m_generator;
    private float m_invHeightRange;
    private float m_heightmapScale;
    private float m_biomePixelSize;
    private bool m_hasRules;
    private int m_hashCode;
    [ThreadStatic]
    private static List<MyPlanetMaterialProvider.PlanetMaterialRule>[] m_rangeBiomes;
    [ThreadStatic]
    private static bool m_rangeClean;
    [ThreadStatic]
    private static WeakReference<MyPlanetMaterialProvider> m_chachedProviderRef;
    private MyPerlin m_perlin = new MyPerlin(seed: 123456, frequency: 5.0);
    private Vector3 m_targetShift = new Vector3(0.0f, -0.9f, -0.08f);
    [ThreadStatic]
    private static MyPlanetMaterialProvider.MapBlendCache m_materialBC;

    public MyCubemap[] Maps { get; private set; }

    private static MyPlanetMaterialProvider CachedProvider
    {
      get
      {
        MyPlanetMaterialProvider target;
        return MyPlanetMaterialProvider.m_chachedProviderRef != null && MyPlanetMaterialProvider.m_chachedProviderRef.TryGetTarget(out target) ? target : (MyPlanetMaterialProvider) null;
      }
      set
      {
        if (value == null && MyPlanetMaterialProvider.m_chachedProviderRef == null)
          return;
        if (MyPlanetMaterialProvider.m_chachedProviderRef != null)
          MyPlanetMaterialProvider.m_chachedProviderRef.SetTarget(value);
        else
          MyPlanetMaterialProvider.m_chachedProviderRef = new WeakReference<MyPlanetMaterialProvider>(value);
      }
    }

    public bool Closed { get; private set; }

    public MyPlanetMaterialProvider(
      MyPlanetGeneratorDefinition generatorDef,
      MyPlanetShapeProvider planetShape,
      MyCubemap[] maps)
    {
      this.m_materials = new Dictionary<byte, MyPlanetMaterialProvider.PlanetMaterial>(generatorDef.SurfaceMaterialTable.Length);
      for (int index = 0; index < generatorDef.SurfaceMaterialTable.Length; ++index)
        this.m_materials[generatorDef.SurfaceMaterialTable[index].Value] = new MyPlanetMaterialProvider.PlanetMaterial(generatorDef.SurfaceMaterialTable[index], generatorDef.MinimumSurfaceLayerDepth);
      this.m_defaultMaterial = new MyPlanetMaterialProvider.PlanetMaterial(generatorDef.DefaultSurfaceMaterial, generatorDef.MinimumSurfaceLayerDepth);
      this.m_subsurfaceMaterial = generatorDef.DefaultSubSurfaceMaterial == null ? this.m_defaultMaterial : new MyPlanetMaterialProvider.PlanetMaterial(generatorDef.DefaultSubSurfaceMaterial, generatorDef.MinimumSurfaceLayerDepth);
      this.m_planetShape = planetShape;
      this.Maps = maps;
      this.m_materialMap = maps[0];
      this.m_biomeMap = maps[1];
      this.m_oreMap = maps[2];
      if (this.m_materialMap != null)
        this.m_mapResolutionMinusOne = this.m_materialMap.Resolution - 1;
      this.m_generator = generatorDef;
      this.m_invHeightRange = (float) (1.0 / ((double) this.m_planetShape.MaxHillHeight - (double) this.m_planetShape.MinHillHeight));
      this.m_biomePixelSize = (float) (((double) planetShape.MaxHillHeight + (double) planetShape.Radius) * Math.PI) / ((float) (this.m_mapResolutionMinusOne + 1) * 2f);
      this.m_hashCode = generatorDef.FolderName.GetHashCode();
      if (this.m_generator.MaterialGroups != null && this.m_generator.MaterialGroups.Length != 0)
      {
        this.m_biomes = new Dictionary<byte, MyPlanetMaterialProvider.PlanetBiome>();
        foreach (MyPlanetMaterialGroup materialGroup in this.m_generator.MaterialGroups)
          this.m_biomes[materialGroup.Value] = new MyPlanetMaterialProvider.PlanetBiome(materialGroup, this.m_generator.MinimumSurfaceLayerDepth);
      }
      this.m_blendingTileset = MySession.Static.GetComponent<MyHeightMapLoadingSystem>().GetTerrainBlendTexture(this.m_generator.MaterialBlending);
      this.m_ores = new Dictionary<byte, List<MyPlanetMaterialProvider.PlanetOre>>();
      foreach (MyPlanetOreMapping oreMapping in this.m_generator.OreMappings)
      {
        MyVoxelMaterialDefinition material = MyPlanetMaterialProvider.GetMaterial(oreMapping.Type);
        if (material != null)
        {
          MyPlanetMaterialProvider.PlanetOre planetOre = new MyPlanetMaterialProvider.PlanetOre()
          {
            Depth = oreMapping.Depth,
            Start = oreMapping.Start,
            Value = oreMapping.Value,
            Material = material,
            ColorInfluence = oreMapping.ColorInfluence
          };
          if (oreMapping.ColorShift.HasValue)
            planetOre.TargetColor = new Vector3?(((Color) oreMapping.ColorShift.Value).ColorToHSV());
          if (!this.m_ores.ContainsKey(oreMapping.Value))
          {
            List<MyPlanetMaterialProvider.PlanetOre> planetOreList = new List<MyPlanetMaterialProvider.PlanetOre>()
            {
              planetOre
            };
            this.m_ores.Add(oreMapping.Value, planetOreList);
          }
          this.m_ores[oreMapping.Value].Add(planetOre);
        }
      }
      this.Closed = false;
    }

    public bool IsMaterialBlacklistedForStation(MyDefinitionId materialId) => this.m_generator.StationBlockingMaterials != null && this.m_generator.StationBlockingMaterials.Contains(materialId);

    public void Close()
    {
      this.m_blendingTileset = (MyTileTexture<byte>) null;
      this.m_subsurfaceMaterial = (MyPlanetMaterialProvider.PlanetMaterial) null;
      this.m_generator = (MyPlanetGeneratorDefinition) null;
      this.m_biomeMap = (MyCubemap) null;
      this.m_biomes = (Dictionary<byte, MyPlanetMaterialProvider.PlanetBiome>) null;
      this.m_materials = (Dictionary<byte, MyPlanetMaterialProvider.PlanetMaterial>) null;
      this.m_planetShape = (MyPlanetShapeProvider) null;
      this.m_ores = (Dictionary<byte, List<MyPlanetMaterialProvider.PlanetOre>>) null;
      this.m_materialMap = (MyCubemap) null;
      this.m_oreMap = (MyCubemap) null;
      this.m_biomeMap = (MyCubemap) null;
      this.Maps = (MyCubemap[]) null;
      this.Closed = true;
    }

    private unsafe void GetRuleBounds(ref BoundingBox request, out BoundingBox ruleBounds)
    {
      Vector3* corners = stackalloc Vector3[8];
      ruleBounds.Min = new Vector3(float.PositiveInfinity);
      ruleBounds.Max = new Vector3(float.NegativeInfinity);
      request.GetCornersUnsafe(corners);
      if (Vector3.Zero.IsInsideInclusive(ref request.Min, ref request.Max))
      {
        ruleBounds.Min.X = 0.0f;
      }
      else
      {
        Vector3 vector3 = Vector3.Clamp(Vector3.Zero, request.Min, request.Max);
        ruleBounds.Min.X = this.m_planetShape.DistanceToRatio(vector3.Length());
      }
      Vector3 center = request.Center;
      Vector3 vector3_1;
      vector3_1.X = (double) center.X >= 0.0 ? request.Max.X : request.Min.X;
      vector3_1.Y = (double) center.Y >= 0.0 ? request.Max.Y : request.Min.Y;
      vector3_1.Z = (double) center.Z >= 0.0 ? request.Max.Z : request.Min.Z;
      ruleBounds.Max.X = this.m_planetShape.DistanceToRatio(vector3_1.Length());
      if ((double) request.Min.X < 0.0 && (double) request.Min.Z < 0.0 && ((double) request.Max.X > 0.0 && (double) request.Max.Z > 0.0))
      {
        ruleBounds.Min.Z = -1f;
        ruleBounds.Max.Z = 3f;
        for (int index = 0; index < 8; ++index)
        {
          float num1 = corners[index].Length();
          float num2 = corners[index].Y / num1;
          if ((double) ruleBounds.Min.Y > (double) num2)
            ruleBounds.Min.Y = num2;
          if ((double) ruleBounds.Max.Y < (double) num2)
            ruleBounds.Max.Y = num2;
        }
      }
      else
      {
        for (int index = 0; index < 8; ++index)
        {
          float num1 = corners[index].Length();
          Vector3* vector3Ptr = corners + index;
          *vector3Ptr = *vector3Ptr / num1;
          float y = corners[index].Y;
          Vector2 vector2 = new Vector2(-corners[index].X, -corners[index].Z);
          vector2.Normalize();
          float num2 = vector2.Y;
          if ((double) vector2.X > 0.0)
            num2 = 2f - num2;
          if ((double) ruleBounds.Min.Y > (double) y)
            ruleBounds.Min.Y = y;
          if ((double) ruleBounds.Max.Y < (double) y)
            ruleBounds.Max.Y = y;
          if ((double) ruleBounds.Min.Z > (double) num2)
            ruleBounds.Min.Z = num2;
          if ((double) ruleBounds.Max.Z < (double) num2)
            ruleBounds.Max.Z = num2;
        }
      }
    }

    public void PrepareRulesForBox(ref BoundingBox request)
    {
      if (this.m_biomes == null)
        return;
      if ((double) request.Extents.Sum > 50.0)
        this.PrepareRulesForBoxInternal(ref request);
      else
        this.CleanRules();
    }

    private void PrepareRulesForBoxInternal(ref BoundingBox request)
    {
      MyPlanetMaterialProvider.EnsureCleanRangeBiomes();
      request.Translate(-this.m_planetShape.Center());
      request.Inflate(request.Extents.Length() * 0.1f);
      BoundingBox ruleBounds;
      this.GetRuleBounds(ref request, out ruleBounds);
      foreach (MyPlanetMaterialProvider.PlanetBiome planetBiome in this.m_biomes.Values)
      {
        if (MyPlanetMaterialProvider.m_rangeBiomes[(int) planetBiome.Value] == null)
          MyPlanetMaterialProvider.m_rangeBiomes[(int) planetBiome.Value] = new List<MyPlanetMaterialProvider.PlanetMaterialRule>();
        planetBiome.MateriaTree.OverlapAllBoundingBox<MyPlanetMaterialProvider.PlanetMaterialRule>(ref ruleBounds, MyPlanetMaterialProvider.m_rangeBiomes[(int) planetBiome.Value]);
        MyPlanetMaterialProvider.m_rangeBiomes[(int) planetBiome.Value].Sort();
      }
      MyPlanetMaterialProvider.m_rangeClean = false;
      MyPlanetMaterialProvider.CachedProvider = this;
    }

    private static void EnsureCleanRangeBiomes()
    {
      if (MyPlanetMaterialProvider.m_rangeBiomes == null)
      {
        MyPlanetMaterialProvider.m_rangeBiomes = new List<MyPlanetMaterialProvider.PlanetMaterialRule>[256];
      }
      else
      {
        for (int index = 0; index < MyPlanetMaterialProvider.m_rangeBiomes.Length; ++index)
        {
          if (MyPlanetMaterialProvider.m_rangeBiomes[index] != null)
            MyPlanetMaterialProvider.m_rangeBiomes[index].Clear();
        }
      }
    }

    private void CleanRules()
    {
      MyPlanetMaterialProvider.EnsureCleanRangeBiomes();
      foreach (MyPlanetMaterialProvider.PlanetBiome planetBiome in this.m_biomes.Values)
      {
        if (MyPlanetMaterialProvider.m_rangeBiomes[(int) planetBiome.Value] != null)
          MyPlanetMaterialProvider.m_rangeBiomes[(int) planetBiome.Value].AddRange((IEnumerable<MyPlanetMaterialProvider.PlanetMaterialRule>) planetBiome.Rules);
        else
          MyPlanetMaterialProvider.m_rangeBiomes[(int) planetBiome.Value] = new List<MyPlanetMaterialProvider.PlanetMaterialRule>((IEnumerable<MyPlanetMaterialProvider.PlanetMaterialRule>) planetBiome.Rules);
      }
      MyPlanetMaterialProvider.m_rangeClean = true;
      MyPlanetMaterialProvider.CachedProvider = this;
    }

    public void ReadMaterialRange(ref MyVoxelDataRequest req, bool detectOnly = false)
    {
      req.Flags = req.RequestFlags & MyVoxelRequestFlags.RequestFlags;
      Vector3I minInLod = req.MinInLod;
      Vector3I maxInLod = req.MaxInLod;
      float lodSize = (float) (1 << req.Lod);
      bool flag1 = req.RequestFlags.HasFlags(MyVoxelRequestFlags.SurfaceMaterial);
      bool flag2 = req.RequestFlags.HasFlags(MyVoxelRequestFlags.ConsiderContent);
      bool preciseOrePositions = req.RequestFlags.HasFlags(MyVoxelRequestFlags.PreciseOrePositions);
      this.m_planetShape.PrepareCache();
      if (this.m_biomes != null)
      {
        if (req.SizeLinear > 125)
        {
          BoundingBox request = new BoundingBox((Vector3) minInLod * lodSize, (Vector3) maxInLod * lodSize);
          this.PrepareRulesForBoxInternal(ref request);
        }
        else if (!MyPlanetMaterialProvider.m_rangeClean || MyPlanetMaterialProvider.CachedProvider != this)
          this.CleanRules();
      }
      Vector3 vector3 = (minInLod + 0.5f) * lodSize;
      Vector3 pos = vector3;
      Vector3I vector3I1 = -minInLod + req.Offset;
      if (detectOnly)
      {
        Vector3I vector3I2;
        for (vector3I2.Z = minInLod.Z; vector3I2.Z <= maxInLod.Z; ++vector3I2.Z)
        {
          for (vector3I2.Y = minInLod.Y; vector3I2.Y <= maxInLod.Y; ++vector3I2.Y)
          {
            for (vector3I2.X = minInLod.X; vector3I2.X <= maxInLod.X; ++vector3I2.X)
            {
              MyVoxelMaterialDefinition materialForPosition = this.GetMaterialForPosition(ref pos, lodSize, out byte _, preciseOrePositions);
              if (materialForPosition != null && materialForPosition.Index != byte.MaxValue)
                return;
              pos.X += lodSize;
            }
            pos.Y += lodSize;
            pos.X = vector3.X;
          }
          pos.Z += lodSize;
          pos.Y = vector3.Y;
        }
        req.Flags |= MyVoxelRequestFlags.EmptyData;
      }
      else
      {
        bool flag3 = true;
        MyStorageData target = req.Target;
        Vector3I vector3I2;
        for (vector3I2.Z = minInLod.Z; vector3I2.Z <= maxInLod.Z; ++vector3I2.Z)
        {
          for (vector3I2.Y = minInLod.Y; vector3I2.Y <= maxInLod.Y; ++vector3I2.Y)
          {
            vector3I2.X = minInLod.X;
            Vector3I p = vector3I2 + vector3I1;
            int linear = target.ComputeLinear(ref p);
            for (; vector3I2.X <= maxInLod.X; ++vector3I2.X)
            {
              byte materialIdx;
              if (flag1 && target.Material(linear) != (byte) 0 || flag2 && target.Content(linear) == (byte) 0)
              {
                materialIdx = byte.MaxValue;
              }
              else
              {
                MyVoxelMaterialDefinition materialForPosition = this.GetMaterialForPosition(ref pos, lodSize, out byte _, preciseOrePositions);
                materialIdx = materialForPosition != null ? materialForPosition.Index : byte.MaxValue;
              }
              target.Material(linear, materialIdx);
              flag3 &= materialIdx == byte.MaxValue;
              linear += target.StepLinear;
              pos.X += lodSize;
            }
            pos.Y += lodSize;
            pos.X = vector3.X;
          }
          pos.Z += lodSize;
          pos.Y = vector3.Y;
        }
        if (!flag3)
          return;
        req.Flags |= MyVoxelRequestFlags.EmptyData;
      }
    }

    private static MyVoxelMaterialDefinition GetMaterial(string name)
    {
      MyVoxelMaterialDefinition materialDefinition = MyDefinitionManager.Static.GetVoxelMaterialDefinition(name);
      if (materialDefinition != null)
        return materialDefinition;
      MyLog.Default.WriteLine("Could not load voxel material " + name);
      return materialDefinition;
    }

    public MyVoxelMaterialDefinition GetMaterialForPosition(
      ref Vector3 pos,
      float lodSize)
    {
      return this.GetMaterialForPosition(ref pos, lodSize, out byte _, false);
    }

    public MyVoxelMaterialDefinition GetMaterialForPosition(
      ref Vector3 pos,
      float lodSize,
      out byte biomeValue,
      bool preciseOrePositions)
    {
      biomeValue = (byte) 0;
      MyPlanetMaterialProvider.MaterialSampleParams ps;
      this.GetPositionParams(ref pos, lodSize, out ps);
      MyVoxelMaterialDefinition materialDefinition = (MyVoxelMaterialDefinition) null;
      float num1 = !preciseOrePositions ? (float) ((double) ps.SurfaceDepth / (double) Math.Max(lodSize * 0.5f, 1f) + 0.5) : ps.SurfaceDepth + 0.5f;
      List<MyPlanetMaterialProvider.PlanetOre> planetOreList;
      if (this.m_oreMap != null && this.m_ores.TryGetValue(this.m_oreMap.Faces[ps.Face].GetValue(ps.Texcoord.X, ps.Texcoord.Y), out planetOreList))
      {
        foreach (MyPlanetMaterialProvider.PlanetOre planetOre in planetOreList)
        {
          if ((double) planetOre.Start <= -(double) num1 && (double) planetOre.Start + (double) planetOre.Depth >= -(double) num1)
          {
            if (planetOre.Material.IsRare)
            {
              BoundingBox bbox = new BoundingBox(pos - Vector3.One, pos + Vector3.One);
              if (this.m_stationTree.OverlapsAnyLeafBoundingBox(ref bbox))
                continue;
            }
            return planetOre.Material;
          }
        }
      }
      MyPlanetMaterialProvider.PlanetMaterial materialForPosition = this.GetLayeredMaterialForPosition(ref ps, out biomeValue);
      float num2 = ps.SurfaceDepth / lodSize;
      if (materialForPosition.HasLayers)
      {
        MyPlanetMaterialProvider.VoxelMaterial[] layers = materialForPosition.Layers;
        for (int index = 0; index < layers.Length; ++index)
        {
          if ((double) num2 >= -(double) layers[index].Depth)
          {
            materialDefinition = materialForPosition.Layers[index].Material;
            break;
          }
        }
      }
      else if ((double) num2 >= -(double) materialForPosition.Depth)
        materialDefinition = materialForPosition.Material;
      if (materialDefinition == null)
        materialDefinition = this.m_subsurfaceMaterial.FirstOrDefault;
      return materialDefinition;
    }

    public Vector3 GetColorShift(Vector3 position, byte material, float maxDepth = 1f)
    {
      if ((double) maxDepth < 1.0)
        return Vector3.Zero;
      MyVoxelMaterialDefinition materialDefinition = MyDefinitionManager.Static.GetVoxelMaterialDefinition(material);
      if (materialDefinition == null || !materialDefinition.ColorKey.HasValue)
        return Vector3.Zero;
      Vector3 localPos = position - this.m_planetShape.Center();
      int face;
      Vector2 texCoord;
      MyCubemapHelpers.CalculateSampleTexcoord(ref localPos, out face, out texCoord);
      List<MyPlanetMaterialProvider.PlanetOre> planetOreList;
      if (this.m_oreMap == null || !this.m_ores.TryGetValue(this.m_oreMap.Faces[face].GetValue(texCoord.X, texCoord.Y), out planetOreList))
        return Vector3.Zero;
      float num1 = (float) MathHelper.Saturate(this.m_perlin.GetValue((double) texCoord.X * 1000.0, (double) texCoord.Y * 1000.0, 0.0));
      foreach (MyPlanetMaterialProvider.PlanetOre planetOre in planetOreList)
      {
        if (planetOre.Material.IsRare)
        {
          BoundingBox bbox = new BoundingBox(position - Vector3.One, position + Vector3.One);
          if (this.m_stationTree.OverlapsAnyLeafBoundingBox(ref bbox))
            continue;
        }
        float colorInfluence = planetOre.ColorInfluence;
        float num2 = 256f;
        if ((double) num2 >= 1.0 && (double) num2 >= (double) planetOre.Start && ((double) planetOre.Start <= (double) maxDepth && planetOre.TargetColor.HasValue))
        {
          Vector3 vector3 = planetOre.TargetColor.Value;
          Color violet = Color.Violet;
          vector3 = !(vector3 == Vector3.Backward) ? this.m_targetShift : new Vector3(0.0f, 1f, -0.08f);
          return num1 * vector3 * (float) (1.0 - (double) planetOre.Start / (double) num2);
        }
      }
      return Vector3.Zero;
    }

    private unsafe byte ComputeMapBlend(
      Vector2 coords,
      int face,
      ref MyPlanetMaterialProvider.MapBlendCache cache,
      MyCubemapData<byte> map)
    {
      coords = coords * (float) map.Resolution - 0.5f;
      Vector2I vector2I = new Vector2I(coords);
      if (cache.HashCode != this.m_hashCode || (int) cache.Face != face || cache.Cell != vector2I)
      {
        cache.HashCode = this.m_hashCode;
        cache.Cell = vector2I;
        cache.Face = (byte) face;
        if (this.m_materialMap != null)
        {
          byte tl;
          map.GetValue(vector2I.X, vector2I.Y, out tl);
          byte tr;
          map.GetValue(vector2I.X + 1, vector2I.Y, out tr);
          byte bl;
          map.GetValue(vector2I.X, vector2I.Y + 1, out bl);
          byte br;
          map.GetValue(vector2I.X + 1, vector2I.Y + 1, out br);
          byte* numPtr1 = stackalloc byte[4];
          numPtr1[0] = tl;
          numPtr1[1] = tr;
          numPtr1[2] = bl;
          numPtr1[3] = br;
          if ((int) tl == (int) tr && (int) bl == (int) br && (int) bl == (int) tl)
          {
            fixed (ushort* numPtr2 = cache.Data)
            {
              numPtr2[0] = (ushort) ((int) tl << 8 | 15);
              numPtr2[1] = (ushort) 0;
              numPtr2[2] = (ushort) 0;
              numPtr2[3] = (ushort) 0;
            }
          }
          else
          {
            fixed (ushort* values = cache.Data)
            {
              MyPlanetMaterialProvider.Sort4(numPtr1);
              MyPlanetMaterialProvider.ComputeTilePattern(tl, tr, bl, br, numPtr1, values);
            }
          }
        }
      }
      byte computed;
      fixed (ushort* values = cache.Data)
      {
        coords -= Vector2.Floor(coords);
        if ((double) coords.X == 1.0)
          coords.X = 0.99999f;
        if ((double) coords.Y == 1.0)
          coords.Y = 0.99999f;
        this.SampleTile(values, ref coords, out computed);
      }
      return computed;
    }

    private static unsafe void Sort4(byte* v)
    {
      if ((int) *v > (int) v[1])
      {
        byte num = v[1];
        v[1] = *v;
        *v = num;
      }
      if ((int) v[2] > (int) v[3])
      {
        byte num = v[2];
        v[2] = v[3];
        v[3] = num;
      }
      if ((int) *v > (int) v[3])
      {
        byte num = v[3];
        v[3] = *v;
        v[3] = num;
      }
      if ((int) v[1] > (int) v[2])
      {
        byte num = v[1];
        v[1] = v[2];
        v[2] = num;
      }
      if ((int) *v > (int) v[1])
      {
        byte num = v[1];
        v[1] = *v;
        *v = num;
      }
      if ((int) v[2] <= (int) v[3])
        return;
      byte num1 = v[2];
      v[2] = v[3];
      v[3] = num1;
    }

    private static unsafe void ComputeTilePattern(
      byte tl,
      byte tr,
      byte bl,
      byte br,
      byte* ss,
      ushort* values)
    {
      int index1 = 0;
      for (int index2 = 0; index2 < 4; ++index2)
      {
        if (index2 <= 0 || (int) ss[index2] != (int) ss[index2 - 1])
          values[index1++] = (ushort) ((int) ss[index2] << 8 | ((int) ss[index2] == (int) tl ? 8 : 0) | ((int) ss[index2] == (int) tr ? 4 : 0) | ((int) ss[index2] == (int) bl ? 2 : 0) | ((int) ss[index2] == (int) br ? 1 : 0));
      }
      for (; index1 < 4; ++index1)
        values[index1] = (ushort) 0;
    }

    private unsafe void SampleTile(ushort* values, ref Vector2 coords, out byte computed)
    {
      byte num1 = 0;
      for (int index = 0; index < 4; ++index)
      {
        byte num2 = (byte) ((uint) values[index] >> 8);
        if (values[index] != (ushort) 0)
        {
          byte num3;
          this.m_blendingTileset.GetValue((int) values[index] & 15, coords, out num3);
          num1 = num2;
          if (num3 == (byte) 0)
            break;
        }
        else
          break;
      }
      computed = num1;
    }

    public void GetPositionParams(
      ref Vector3 pos,
      float lodSize,
      out MyPlanetMaterialProvider.MaterialSampleParams ps,
      bool skipCache = false)
    {
      Vector3 localPos = pos - this.m_planetShape.Center();
      ps.DistanceToCenter = localPos.Length();
      ps.LodSize = lodSize;
      if ((double) ps.DistanceToCenter < 0.00999999977648258)
      {
        ps.SurfaceDepth = 0.0f;
        ps.Gravity = Vector3.Down;
        ps.Latitude = 0.0f;
        ps.Longitude = 0.0f;
        ps.Texcoord = Vector2.One / 2f;
        ps.Face = 0;
        ps.Normal = Vector3.Backward;
        ps.SampledHeight = 0.0f;
      }
      else
      {
        ps.Gravity = localPos / ps.DistanceToCenter;
        MyCubemapHelpers.CalculateSampleTexcoord(ref localPos, out ps.Face, out ps.Texcoord);
        ps.SampledHeight = !skipCache ? this.m_planetShape.GetValueForPositionWithCache(ps.Face, ref ps.Texcoord, out ps.Normal) : this.m_planetShape.GetValueForPositionCacheless(ps.Face, ref ps.Texcoord, out ps.Normal);
        ps.SurfaceDepth = this.m_planetShape.SignedDistanceWithSample(lodSize, ps.DistanceToCenter, ps.SampledHeight) * ps.Normal.Z;
        ps.Latitude = ps.Gravity.Y;
        Vector2 vector2 = new Vector2(-ps.Gravity.X, -ps.Gravity.Z);
        vector2.Normalize();
        ps.Longitude = vector2.Y;
        if (-(double) ps.Gravity.X <= 0.0)
          return;
        ps.Longitude = 2f - ps.Longitude;
      }
    }

    public MyPlanetMaterialProvider.PlanetMaterial GetLayeredMaterialForPosition(
      ref MyPlanetMaterialProvider.MaterialSampleParams ps,
      out byte biomeValue)
    {
      if ((double) ps.DistanceToCenter < 0.01)
      {
        biomeValue = byte.MaxValue;
        return this.m_defaultMaterial;
      }
      byte key = 0;
      MyPlanetMaterialProvider.PlanetMaterial planetMaterial = (MyPlanetMaterialProvider.PlanetMaterial) null;
      byte num = 0;
      if (this.m_biomeMap != null)
        num = this.m_biomeMap.Faces[ps.Face].GetValue(ps.Texcoord.X, ps.Texcoord.Y);
      if ((double) this.m_biomePixelSize < (double) ps.LodSize)
      {
        if (this.m_materialMap != null)
          key = this.m_materialMap.Faces[ps.Face].GetValue(ps.Texcoord.X, ps.Texcoord.Y);
      }
      else if (this.m_materialMap != null)
        key = this.ComputeMapBlend(ps.Texcoord, ps.Face, ref MyPlanetMaterialProvider.m_materialBC, this.m_materialMap.Faces[ps.Face]);
      this.m_materials.TryGetValue(key, out planetMaterial);
      if (planetMaterial == null && this.m_biomes != null)
      {
        List<MyPlanetMaterialProvider.PlanetMaterialRule> rangeBiome = MyPlanetMaterialProvider.m_rangeBiomes[(int) key];
        if (rangeBiome != null && rangeBiome.Count != 0)
        {
          float height = (ps.SampledHeight - this.m_planetShape.MinHillHeight) * this.m_invHeightRange;
          foreach (MyPlanetMaterialProvider.PlanetMaterialRule planetMaterialRule in rangeBiome)
          {
            if (planetMaterialRule.Check(height, ps.Latitude, ps.Longitude, ps.Normal.Z))
            {
              planetMaterial = (MyPlanetMaterialProvider.PlanetMaterial) planetMaterialRule;
              break;
            }
          }
        }
      }
      if (planetMaterial == null)
        planetMaterial = this.m_defaultMaterial;
      biomeValue = num;
      return planetMaterial;
    }

    public void SetStationOreBlockTree(MyDynamicAABBTree tree) => this.m_stationTree = tree;

    public void GetMaterialForPositionDebug(
      ref Vector3 pos,
      out MyPlanetStorageProvider.SurfacePropertiesExtended props)
    {
      MyPlanetMaterialProvider.MaterialSampleParams ps;
      this.GetPositionParams(ref pos, 1f, out ps, true);
      props.Position = pos;
      props.Gravity = -ps.Gravity;
      props.Material = this.m_defaultMaterial.FirstOrDefault;
      props.Slope = ps.Normal.Z;
      props.HeightRatio = this.m_planetShape.AltitudeToRatio(ps.SampledHeight);
      props.Depth = ps.SurfaceDepth;
      props.Latitude = ps.Latitude;
      props.Longitude = ps.Longitude;
      props.Altitude = ps.DistanceToCenter - this.m_planetShape.Radius;
      props.GroundHeight = ps.SampledHeight + this.m_planetShape.Radius;
      props.Face = ps.Face;
      props.Texcoord = ps.Texcoord;
      props.BiomeValue = (byte) 0;
      props.MaterialValue = (byte) 0;
      props.OreValue = (byte) 0;
      props.EffectiveRule = (MyPlanetMaterialProvider.PlanetMaterial) null;
      props.Biome = (MyPlanetMaterialProvider.PlanetBiome) null;
      props.Ore = new MyPlanetMaterialProvider.PlanetOre();
      props.Origin = MyPlanetStorageProvider.SurfacePropertiesExtended.MaterialOrigin.Default;
      MyPlanetMaterialProvider.PlanetMaterial planetMaterial = (MyPlanetMaterialProvider.PlanetMaterial) null;
      if (this.m_oreMap != null)
      {
        props.OreValue = this.m_oreMap.Faces[ps.Face].GetValue(ps.Texcoord.X, ps.Texcoord.Y);
        List<MyPlanetMaterialProvider.PlanetOre> planetOreList;
        if (this.m_ores.TryGetValue(props.OreValue, out planetOreList))
        {
          foreach (MyPlanetMaterialProvider.PlanetOre planetOre in planetOreList)
          {
            props.Ore = planetOre;
            if ((double) planetOre.Start <= -(double) ps.SurfaceDepth && (double) planetOre.Start + (double) planetOre.Depth >= -(double) ps.SurfaceDepth)
            {
              props.Material = planetOre.Material;
              props.Origin = MyPlanetStorageProvider.SurfacePropertiesExtended.MaterialOrigin.Ore;
              break;
            }
          }
        }
      }
      if ((double) ps.DistanceToCenter < 0.01)
        return;
      byte key = 0;
      if ((double) this.m_biomePixelSize < (double) ps.LodSize)
      {
        if (this.m_materialMap != null)
          key = this.m_materialMap.Faces[ps.Face].GetValue(ps.Texcoord.X, ps.Texcoord.Y);
      }
      else if (this.m_materialMap != null)
        key = this.ComputeMapBlend(ps.Texcoord, ps.Face, ref MyPlanetMaterialProvider.m_materialBC, this.m_materialMap.Faces[ps.Face]);
      this.m_materials.TryGetValue(key, out planetMaterial);
      props.Origin = MyPlanetStorageProvider.SurfacePropertiesExtended.MaterialOrigin.Map;
      props.MaterialValue = key;
      if (planetMaterial == null && this.m_biomes != null)
      {
        MyPlanetMaterialProvider.PlanetBiome planetBiome;
        this.m_biomes.TryGetValue(key, out planetBiome);
        props.Biome = planetBiome;
        if (planetBiome != null && planetBiome.IsValid)
        {
          foreach (MyPlanetMaterialProvider.PlanetMaterialRule rule in planetBiome.Rules)
          {
            if (rule.Check(props.HeightRatio, ps.Latitude, ps.Longitude, ps.Normal.Z))
            {
              planetMaterial = (MyPlanetMaterialProvider.PlanetMaterial) rule;
              props.Origin = MyPlanetStorageProvider.SurfacePropertiesExtended.MaterialOrigin.Rule;
              break;
            }
          }
        }
      }
      if (planetMaterial == null)
      {
        planetMaterial = this.m_defaultMaterial;
        props.Origin = MyPlanetStorageProvider.SurfacePropertiesExtended.MaterialOrigin.Default;
      }
      byte num1 = 0;
      if (this.m_biomeMap != null)
        num1 = this.m_biomeMap.Faces[ps.Face].GetValue(ps.Texcoord.X, ps.Texcoord.Y);
      props.BiomeValue = num1;
      float num2 = ps.SurfaceDepth + 0.5f;
      if (planetMaterial.HasLayers)
      {
        MyPlanetMaterialProvider.VoxelMaterial[] layers = planetMaterial.Layers;
        for (int index = 0; index < layers.Length; ++index)
        {
          if ((double) num2 >= -(double) layers[index].Depth)
          {
            props.Material = planetMaterial.Layers[index].Material;
            break;
          }
        }
      }
      else if ((double) num2 >= -(double) planetMaterial.Depth)
        props.Material = planetMaterial.Material;
      props.EffectiveRule = planetMaterial;
    }

    private struct MapBlendCache
    {
      public Vector2I Cell;
      public unsafe fixed ushort Data[4];
      public byte Face;
      public int HashCode;
    }

    public struct PlanetOre
    {
      public byte Value;
      public float Depth;
      public float Start;
      public float ColorInfluence;
      public MyVoxelMaterialDefinition Material;
      public Vector3? TargetColor;

      public override string ToString()
      {
        if (this.Material == null)
          return "";
        return string.Format("{0}({1}:{2}; {3})", (object) this.Material.Id.SubtypeName, (object) this.Start, (object) this.Depth, (object) this.Value);
      }
    }

    public struct MaterialSampleParams
    {
      public Vector3 Gravity;
      public Vector3 Normal;
      public float DistanceToCenter;
      public float SampledHeight;
      public int Face;
      public Vector2 Texcoord;
      public float SurfaceDepth;
      public float LodSize;
      public float Latitude;
      public float Longitude;
    }

    public class VoxelMaterial
    {
      public MyVoxelMaterialDefinition Material;
      public float Depth;
      public byte Value;

      public virtual bool IsRule => false;

      public override string ToString() => this.Material != null ? string.Format("({0}:{1})", (object) this.Material.Id.SubtypeName, (object) this.Depth) : "null";
    }

    public class PlanetMaterial : MyPlanetMaterialProvider.VoxelMaterial
    {
      public MyPlanetMaterialProvider.VoxelMaterial[] Layers;

      public bool HasLayers => this.Layers != null && (uint) this.Layers.Length > 0U;

      public MyVoxelMaterialDefinition FirstOrDefault => !this.HasLayers ? this.Material : this.Layers[0].Material;

      public PlanetMaterial(MyPlanetMaterialDefinition def, float minimumSurfaceLayerDepth)
      {
        this.Depth = def.MaxDepth;
        if (def.Material != null)
          this.Material = MyPlanetMaterialProvider.GetMaterial(def.Material);
        this.Value = def.Value;
        if (!def.HasLayers)
          return;
        int length = def.Layers.Length;
        if ((double) def.Layers[0].Depth < (double) minimumSurfaceLayerDepth && MyPlanetMaterialProvider.GetMaterial(def.Layers[0].Material).RenderParams.Foliage.HasValue)
          ++length;
        this.Layers = new MyPlanetMaterialProvider.VoxelMaterial[length];
        int index1 = 0;
        int index2 = 0;
        while (index1 < def.Layers.Length)
        {
          this.Layers[index2] = new MyPlanetMaterialProvider.VoxelMaterial()
          {
            Material = MyPlanetMaterialProvider.GetMaterial(def.Layers[index1].Material),
            Depth = def.Layers[index1].Depth
          };
          if (index2 == 0 && (double) def.Layers[index1].Depth < (double) minimumSurfaceLayerDepth)
          {
            if ((double) minimumSurfaceLayerDepth > 1.0 && this.Layers[index2].Material.RenderParams.Foliage.HasValue)
            {
              MyVoxelMaterialDefinition materialDefinition = string.IsNullOrEmpty(this.Layers[index2].Material.BareVariant) ? this.Layers[index2].Material : MyDefinitionManager.Static.GetVoxelMaterialDefinition(this.Layers[index2].Material.BareVariant);
              this.Layers[index2].Depth = 1f;
              ++index2;
              this.Layers[index2] = new MyPlanetMaterialProvider.VoxelMaterial()
              {
                Material = materialDefinition,
                Depth = minimumSurfaceLayerDepth - 1f
              };
            }
            else
              this.Layers[index2].Depth = minimumSurfaceLayerDepth;
          }
          ++index1;
          ++index2;
        }
      }

      private string FormatLayers(int padding)
      {
        StringBuilder stringBuilder = new StringBuilder();
        string str = new string(' ', padding);
        stringBuilder.Append('[');
        if (this.Layers.Length != 0)
        {
          stringBuilder.Append('\n');
          for (int index = 0; index < this.Layers.Length; ++index)
          {
            stringBuilder.Append(str);
            stringBuilder.Append("\t\t");
            stringBuilder.Append((object) this.Layers[index]);
            stringBuilder.Append('\n');
          }
        }
        stringBuilder.Append(str);
        stringBuilder.Append(']');
        return stringBuilder.ToString();
      }

      public override string ToString() => this.ToString(0);

      public string ToString(int padding) => this.HasLayers ? string.Format("LayeredMaterial({0})", (object) this.FormatLayers(padding)) : "SimpleMaterial" + base.ToString();
    }

    public class PlanetMaterialRule : MyPlanetMaterialProvider.PlanetMaterial, IComparable<MyPlanetMaterialProvider.PlanetMaterialRule>
    {
      public SerializableRange Height;
      public SymmetricSerializableRange Latitude;
      public SerializableRange Longitude;
      public SerializableRange Slope;
      public int Index;

      public override bool IsRule => true;

      public bool Check(float height, float latitude, float longitude, float slope) => this.Height.ValueBetween(height) && this.Latitude.ValueBetween(latitude) && this.Longitude.ValueBetween(longitude) && this.Slope.ValueBetween(slope);

      public PlanetMaterialRule(
        MyPlanetMaterialPlacementRule def,
        int index,
        float minimumSurfaceLayerDepth)
        : base((MyPlanetMaterialDefinition) def, minimumSurfaceLayerDepth)
      {
        this.Height = def.Height;
        this.Latitude = def.Latitude;
        this.Longitude = def.Longitude;
        this.Slope = def.Slope;
        this.Index = index;
      }

      public override string ToString() => string.Format("MaterialRule(\n\tHeight: {0};\n\tSlope: {1};\n\tLatitude: {2};\n\tLongitude: {3};\n\tMaterials: {4})", (object) this.Height.ToString(), (object) this.Slope.ToStringAcos(), (object) this.Latitude.ToStringAsin(), (object) this.Longitude.ToStringLongitude(), (object) this.ToString(4));

      public int CompareTo(MyPlanetMaterialProvider.PlanetMaterialRule other)
      {
        if (this == other)
          return 0;
        return other == null ? 1 : this.Index.CompareTo(other.Index);
      }
    }

    public class PlanetBiome
    {
      public MyDynamicAABBTree MateriaTree;
      public byte Value;
      public string Name;
      public List<MyPlanetMaterialProvider.PlanetMaterialRule> Rules;

      public bool IsValid => this.Rules.Count > 0;

      public PlanetBiome(MyPlanetMaterialGroup group, float minimumSurfaceLayerDepth)
      {
        this.Value = group.Value;
        this.Name = group.Name;
        this.Rules = new List<MyPlanetMaterialProvider.PlanetMaterialRule>(group.MaterialRules.Length);
        for (int index = 0; index < group.MaterialRules.Length; ++index)
          this.Rules.Add(new MyPlanetMaterialProvider.PlanetMaterialRule(group.MaterialRules[index], index, minimumSurfaceLayerDepth));
        this.MateriaTree = new MyDynamicAABBTree(Vector3.Zero);
        foreach (MyPlanetMaterialProvider.PlanetMaterialRule rule in this.Rules)
        {
          BoundingBox aabb = new BoundingBox(new Vector3(rule.Height.Min, rule.Latitude.Min, rule.Longitude.Min), new Vector3(rule.Height.Max, rule.Latitude.Max, rule.Longitude.Max));
          this.MateriaTree.AddProxy(ref aabb, (object) rule, 0U);
          if (rule.Latitude.Mirror)
          {
            float num = -aabb.Max.Y;
            aabb.Max.Y = -aabb.Min.Y;
            aabb.Min.Y = num;
            this.MateriaTree.AddProxy(ref aabb, (object) rule, 0U);
          }
        }
      }
    }
  }
}
