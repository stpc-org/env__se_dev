// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyVoxelMaterialDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using Medieval.ObjectBuilders.Definitions;
using System;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace VRage.Game
{
  [MyDefinitionType(typeof (MyObjectBuilder_VoxelMaterialDefinition), null)]
  public class MyVoxelMaterialDefinition : MyDefinitionBase
  {
    private static byte m_indexCounter;
    public string MaterialTypeName;
    public string MinedOre;
    public float MinedOreRatio;
    public bool CanBeHarvested;
    public bool IsRare;
    public int MinVersion;
    public int MaxVersion;
    public bool SpawnsInAsteroids;
    public bool SpawnsFromMeteorites;
    public string VoxelHandPreview;
    public float Friction;
    public float Restitution;
    public string LandingEffect;
    public int AsteroidGeneratorSpawnProbabilityMultiplier;
    public string BareVariant;
    private MyStringId m_materialTypeNameIdCache;
    private MyStringHash m_materialTypeNameHashCache;
    public MyStringHash DamagedMaterial;
    public MyRenderVoxelMaterialData RenderParams;
    public Vector3? ColorKey;

    public MyStringId MaterialTypeNameId
    {
      get
      {
        if (this.m_materialTypeNameIdCache == new MyStringId())
          this.m_materialTypeNameIdCache = MyStringId.GetOrCompute(this.MaterialTypeName);
        return this.m_materialTypeNameIdCache;
      }
    }

    public MyStringHash MaterialTypeNameHash
    {
      get
      {
        if (this.m_materialTypeNameHashCache == new MyStringHash())
          this.m_materialTypeNameHashCache = MyStringHash.GetOrCompute(this.MaterialTypeName);
        return this.m_materialTypeNameHashCache;
      }
    }

    public byte Index { get; set; }

    public bool HasDamageMaterial => this.DamagedMaterial != MyStringHash.NullOrEmpty;

    public void AssignIndex()
    {
      this.Index = MyVoxelMaterialDefinition.m_indexCounter++;
      this.RenderParams.Index = this.Index;
    }

    public static void ResetIndexing() => MyVoxelMaterialDefinition.m_indexCounter = (byte) 0;

    protected override void Init(MyObjectBuilder_DefinitionBase ob)
    {
      base.Init(ob);
      MyObjectBuilder_Dx11VoxelMaterialDefinition materialDefinition = ob as MyObjectBuilder_Dx11VoxelMaterialDefinition;
      this.MaterialTypeName = materialDefinition.MaterialTypeName;
      this.MinedOre = materialDefinition.MinedOre;
      this.MinedOreRatio = materialDefinition.MinedOreRatio;
      this.CanBeHarvested = materialDefinition.CanBeHarvested;
      this.IsRare = materialDefinition.IsRare;
      this.SpawnsInAsteroids = materialDefinition.SpawnsInAsteroids;
      this.SpawnsFromMeteorites = materialDefinition.SpawnsFromMeteorites;
      this.VoxelHandPreview = materialDefinition.VoxelHandPreview;
      this.MinVersion = materialDefinition.MinVersion;
      this.MaxVersion = materialDefinition.MaxVersion;
      this.DamagedMaterial = MyStringHash.GetOrCompute(materialDefinition.DamagedMaterial);
      this.Friction = materialDefinition.Friction;
      this.Restitution = materialDefinition.Restitution;
      this.LandingEffect = materialDefinition.LandingEffect;
      this.BareVariant = materialDefinition.BareVariant;
      this.AsteroidGeneratorSpawnProbabilityMultiplier = materialDefinition.AsteroidGeneratorSpawnProbabilityMultiplier;
      if (materialDefinition.ColorKey.HasValue)
        this.ColorKey = new Vector3?(((Color) materialDefinition.ColorKey.Value).ColorToHSV());
      this.RenderParams.Index = this.Index;
      this.RenderParams.TextureSets = new MyRenderVoxelMaterialData.TextureSet[3];
      this.RenderParams.TextureSets[0].ColorMetalXZnY = materialDefinition.ColorMetalXZnY;
      this.RenderParams.TextureSets[0].ColorMetalY = materialDefinition.ColorMetalY;
      this.RenderParams.TextureSets[0].NormalGlossXZnY = materialDefinition.NormalGlossXZnY;
      this.RenderParams.TextureSets[0].NormalGlossY = materialDefinition.NormalGlossY;
      this.RenderParams.TextureSets[0].ExtXZnY = materialDefinition.ExtXZnY;
      this.RenderParams.TextureSets[0].ExtY = materialDefinition.ExtY;
      this.RenderParams.TextureSets[0].Check();
      this.RenderParams.TextureSets[1].ColorMetalXZnY = materialDefinition.ColorMetalXZnYFar1 ?? this.RenderParams.TextureSets[0].ColorMetalXZnY;
      this.RenderParams.TextureSets[1].ColorMetalY = materialDefinition.ColorMetalYFar1 ?? this.RenderParams.TextureSets[1].ColorMetalXZnY;
      this.RenderParams.TextureSets[1].NormalGlossXZnY = materialDefinition.NormalGlossXZnYFar1 ?? this.RenderParams.TextureSets[0].NormalGlossXZnY;
      this.RenderParams.TextureSets[1].NormalGlossY = materialDefinition.NormalGlossYFar1 ?? this.RenderParams.TextureSets[1].NormalGlossXZnY;
      this.RenderParams.TextureSets[1].ExtXZnY = materialDefinition.ExtXZnYFar1 ?? this.RenderParams.TextureSets[0].ExtXZnY;
      this.RenderParams.TextureSets[1].ExtY = materialDefinition.ExtYFar1 ?? this.RenderParams.TextureSets[1].ExtXZnY;
      this.RenderParams.TextureSets[2].ColorMetalXZnY = materialDefinition.ColorMetalXZnYFar2 ?? this.RenderParams.TextureSets[1].ColorMetalXZnY;
      this.RenderParams.TextureSets[2].ColorMetalY = materialDefinition.ColorMetalYFar2 ?? this.RenderParams.TextureSets[2].ColorMetalXZnY;
      this.RenderParams.TextureSets[2].NormalGlossXZnY = materialDefinition.NormalGlossXZnYFar2 ?? this.RenderParams.TextureSets[1].NormalGlossXZnY;
      this.RenderParams.TextureSets[2].NormalGlossY = materialDefinition.NormalGlossYFar2 ?? this.RenderParams.TextureSets[2].NormalGlossXZnY;
      this.RenderParams.TextureSets[2].ExtXZnY = materialDefinition.ExtXZnYFar2 ?? this.RenderParams.TextureSets[1].ExtXZnY;
      this.RenderParams.TextureSets[2].ExtY = materialDefinition.ExtYFar2 ?? this.RenderParams.TextureSets[2].ExtXZnY;
      this.RenderParams.StandardTilingSetup.InitialScale = materialDefinition.InitialScale;
      this.RenderParams.StandardTilingSetup.ScaleMultiplier = materialDefinition.ScaleMultiplier;
      this.RenderParams.StandardTilingSetup.InitialDistance = materialDefinition.InitialDistance;
      this.RenderParams.StandardTilingSetup.DistanceMultiplier = materialDefinition.DistanceMultiplier;
      this.RenderParams.StandardTilingSetup.TilingScale = materialDefinition.TilingScale;
      this.RenderParams.StandardTilingSetup.Far1Distance = materialDefinition.Far1Distance;
      this.RenderParams.StandardTilingSetup.Far2Distance = materialDefinition.Far2Distance;
      this.RenderParams.StandardTilingSetup.Far3Distance = materialDefinition.Far3Distance;
      this.RenderParams.StandardTilingSetup.Far1Scale = materialDefinition.Far1Scale;
      this.RenderParams.StandardTilingSetup.Far2Scale = materialDefinition.Far2Scale;
      this.RenderParams.StandardTilingSetup.Far3Scale = materialDefinition.Far3Scale;
      this.RenderParams.StandardTilingSetup.ExtensionDetailScale = materialDefinition.ExtDetailScale;
      if (materialDefinition.SimpleTilingSetup == null)
      {
        this.RenderParams.SimpleTilingSetup = this.RenderParams.StandardTilingSetup;
      }
      else
      {
        Medieval.ObjectBuilders.Definitions.TilingSetup simpleTilingSetup = materialDefinition.SimpleTilingSetup;
        this.RenderParams.SimpleTilingSetup.InitialScale = simpleTilingSetup.InitialScale;
        this.RenderParams.SimpleTilingSetup.ScaleMultiplier = simpleTilingSetup.ScaleMultiplier;
        this.RenderParams.SimpleTilingSetup.InitialDistance = simpleTilingSetup.InitialDistance;
        this.RenderParams.SimpleTilingSetup.DistanceMultiplier = simpleTilingSetup.DistanceMultiplier;
        this.RenderParams.SimpleTilingSetup.TilingScale = simpleTilingSetup.TilingScale;
        this.RenderParams.SimpleTilingSetup.Far1Distance = simpleTilingSetup.Far1Distance;
        this.RenderParams.SimpleTilingSetup.Far2Distance = simpleTilingSetup.Far2Distance;
        this.RenderParams.SimpleTilingSetup.Far3Distance = simpleTilingSetup.Far3Distance;
        this.RenderParams.SimpleTilingSetup.Far1Scale = simpleTilingSetup.Far1Scale;
        this.RenderParams.SimpleTilingSetup.Far2Scale = simpleTilingSetup.Far2Scale;
        this.RenderParams.SimpleTilingSetup.Far3Scale = simpleTilingSetup.Far3Scale;
        this.RenderParams.SimpleTilingSetup.ExtensionDetailScale = simpleTilingSetup.ExtDetailScale;
      }
      this.RenderParams.Far3Color = materialDefinition.Far3Color;
      MyRenderFoliageData renderFoliageData = new MyRenderFoliageData();
      if (materialDefinition.FoliageColorTextureArray != null)
      {
        renderFoliageData.Type = (MyFoliageType) materialDefinition.FoliageType;
        renderFoliageData.Density = materialDefinition.FoliageDensity;
        string[] colorTextureArray = materialDefinition.FoliageColorTextureArray;
        string[] normalTextureArray = materialDefinition.FoliageNormalTextureArray;
        int val1;
        if (normalTextureArray != null)
        {
          if (colorTextureArray.Length != normalTextureArray.Length)
            MyLog.Default.Warning("Legacy foliage format has different size normal and color arrays, only the minimum length will be used.");
          val1 = Math.Min(colorTextureArray.Length, normalTextureArray.Length);
        }
        else
          val1 = colorTextureArray.Length;
        int length = Math.Min(val1, 16);
        renderFoliageData.Entries = new MyRenderFoliageData.FoliageEntry[length];
        for (int index = 0; index < length; ++index)
          renderFoliageData.Entries[index] = new MyRenderFoliageData.FoliageEntry()
          {
            ColorAlphaTexture = colorTextureArray[index],
            NormalGlossTexture = normalTextureArray?[index],
            Probability = 1f,
            Size = materialDefinition.FoliageScale,
            SizeVariation = materialDefinition.FoliageRandomRescaleMult
          };
      }
      if ((double) renderFoliageData.Density <= 0.0)
        return;
      this.RenderParams.Foliage = new MyRenderFoliageData?(renderFoliageData);
    }

    public override MyObjectBuilder_DefinitionBase GetObjectBuilder() => (MyObjectBuilder_DefinitionBase) null;

    public void UpdateVoxelMaterial() => MyRenderProxy.UpdateRenderVoxelMaterials(new MyRenderVoxelMaterialData[1]
    {
      this.RenderParams
    });

    public string Icon => this.Icons != null && this.Icons.Length != 0 ? this.Icons[0] : this.RenderParams.TextureSets[0].ColorMetalXZnY;

    private class VRage_Game_MyVoxelMaterialDefinition\u003C\u003EActor : IActivator, IActivator<MyVoxelMaterialDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyVoxelMaterialDefinition();

      MyVoxelMaterialDefinition IActivator<MyVoxelMaterialDefinition>.CreateInstance() => new MyVoxelMaterialDefinition();
    }
  }
}
