// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyPlanetGeneratorDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;
using Sandbox.Game.WorldEnvironment.Definitions;
using System;
using System.Collections.Generic;
using System.Reflection;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.ObjectBuilders.Definitions.Components;
using VRage.Utils;
using VRageMath;
using VRageRender.Messages;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_PlanetGeneratorDefinition), typeof (MyPlanetGeneratorDefinition.Postprocessor))]
  public class MyPlanetGeneratorDefinition : MyDefinitionBase
  {
    public MyDefinitionId? EnvironmentId;
    public MyWorldEnvironmentDefinition EnvironmentDefinition;
    private MyObjectBuilder_PlanetGeneratorDefinition m_pgob;
    public bool HasAtmosphere;
    public List<MyCloudLayerSettings> CloudLayers;
    public MyPlanetMaps PlanetMaps;
    public SerializableRange HillParams;
    public SerializableRange MaterialsMaxDepth;
    public SerializableRange MaterialsMinDepth;
    public MyPlanetOreMapping[] OreMappings = new MyPlanetOreMapping[0];
    public float GravityFalloffPower = 7f;
    public MyObjectBuilder_PlanetMapProvider MapProvider;
    public MyAtmosphereColorShift HostileAtmosphereColorShift = new MyAtmosphereColorShift();
    public MyPlanetMaterialDefinition[] SurfaceMaterialTable = new MyPlanetMaterialDefinition[0];
    public MyPlanetDistortionDefinition[] DistortionTable = new MyPlanetDistortionDefinition[0];
    public MyPlanetMaterialDefinition DefaultSurfaceMaterial;
    public MyPlanetMaterialDefinition DefaultSubSurfaceMaterial;
    public MyPlanetEnvironmentalSoundRule[] SoundRules;
    public List<MyMusicCategory> MusicCategories;
    public MyPlanetMaterialGroup[] MaterialGroups = new MyPlanetMaterialGroup[0];
    public Dictionary<int, Dictionary<string, List<MyPlanetEnvironmentMapping>>> MaterialEnvironmentMappings = new Dictionary<int, Dictionary<string, List<MyPlanetEnvironmentMapping>>>();
    public float SurfaceGravity = 1f;
    public float AtmosphereHeight;
    public float SectorDensity = 0.0017f;
    public MyTemperatureLevel DefaultSurfaceTemperature = MyTemperatureLevel.Cozy;
    public MyPlanetAtmosphere Atmosphere = new MyPlanetAtmosphere();
    public MyAtmosphereSettings? AtmosphereSettings;
    public MyPlanetMaterialBlendSettings MaterialBlending = new MyPlanetMaterialBlendSettings()
    {
      Texture = "Data/PlanetDataFiles/Extra/material_blend_grass",
      CellSize = 64
    };
    public string FolderName;
    public MyPlanetSurfaceDetail Detail;
    public MyPlanetAnimalSpawnInfo AnimalSpawnInfo;
    public MyPlanetAnimalSpawnInfo NightAnimalSpawnInfo;
    public Type EnvironmentSectorType;
    public MyObjectBuilder_VoxelMesherComponentDefinition MesherPostprocessing;
    public float MinimumSurfaceLayerDepth;
    public List<MyDefinitionId> StationBlockingMaterials;
    public List<MyWeatherGeneratorSettings> WeatherGenerators;
    public int WeatherFrequencyMin;
    public int WeatherFrequencyMax;
    public bool GlobalWeather;
    public MyStringId Difficulty = MySpaceTexts.DifficultyNormal;

    private void InheritFrom(string generator)
    {
      MyPlanetGeneratorDefinition definition = MyDefinitionManager.Static.GetDefinition<MyPlanetGeneratorDefinition>(MyStringHash.GetOrCompute(generator));
      if (definition == null)
        MyDefinitionManager.Static.LoadingSet.m_planetGeneratorDefinitions.TryGetValue(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_PlanetGeneratorDefinition), generator), out definition);
      if (definition == null)
      {
        MyLog.Default.WriteLine(string.Format("Could not find planet generator definition for '{0}'.", (object) generator));
      }
      else
      {
        this.PlanetMaps = definition.PlanetMaps;
        this.HasAtmosphere = definition.HasAtmosphere;
        this.Atmosphere = definition.Atmosphere;
        this.CloudLayers = definition.CloudLayers;
        this.SoundRules = definition.SoundRules;
        this.MusicCategories = definition.MusicCategories;
        this.HillParams = definition.HillParams;
        this.MaterialsMaxDepth = definition.MaterialsMaxDepth;
        this.MaterialsMinDepth = definition.MaterialsMinDepth;
        this.GravityFalloffPower = definition.GravityFalloffPower;
        this.HostileAtmosphereColorShift = definition.HostileAtmosphereColorShift;
        this.SurfaceMaterialTable = definition.SurfaceMaterialTable;
        this.DistortionTable = definition.DistortionTable;
        this.DefaultSurfaceMaterial = definition.DefaultSurfaceMaterial;
        this.DefaultSubSurfaceMaterial = definition.DefaultSubSurfaceMaterial;
        this.MaterialGroups = definition.MaterialGroups;
        this.MaterialEnvironmentMappings = definition.MaterialEnvironmentMappings;
        this.SurfaceGravity = definition.SurfaceGravity;
        this.AtmosphereSettings = definition.AtmosphereSettings;
        this.FolderName = definition.FolderName;
        this.MaterialBlending = definition.MaterialBlending;
        this.OreMappings = definition.OreMappings;
        this.AnimalSpawnInfo = definition.AnimalSpawnInfo;
        this.NightAnimalSpawnInfo = definition.NightAnimalSpawnInfo;
        this.Detail = definition.Detail;
        this.SectorDensity = definition.SectorDensity;
        this.DefaultSurfaceTemperature = definition.DefaultSurfaceTemperature;
        this.EnvironmentSectorType = definition.EnvironmentSectorType;
        this.MesherPostprocessing = definition.MesherPostprocessing;
        this.MinimumSurfaceLayerDepth = definition.MinimumSurfaceLayerDepth;
      }
    }

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_PlanetGeneratorDefinition generatorDefinition = builder as MyObjectBuilder_PlanetGeneratorDefinition;
      if (generatorDefinition.InheritFrom != null && generatorDefinition.InheritFrom.Length > 0)
        this.InheritFrom(generatorDefinition.InheritFrom);
      if (generatorDefinition.Environment.HasValue)
        this.EnvironmentId = new MyDefinitionId?((MyDefinitionId) generatorDefinition.Environment.Value);
      else
        this.m_pgob = generatorDefinition;
      if (generatorDefinition.PlanetMaps.HasValue)
        this.PlanetMaps = generatorDefinition.PlanetMaps.Value;
      if (generatorDefinition.HasAtmosphere.HasValue)
        this.HasAtmosphere = generatorDefinition.HasAtmosphere.Value;
      if (generatorDefinition.CloudLayers != null)
        this.CloudLayers = generatorDefinition.CloudLayers;
      if (generatorDefinition.SoundRules != null)
      {
        this.SoundRules = new MyPlanetEnvironmentalSoundRule[generatorDefinition.SoundRules.Length];
        for (int index = 0; index < generatorDefinition.SoundRules.Length; ++index)
        {
          MyPlanetEnvironmentalSoundRule environmentalSoundRule = new MyPlanetEnvironmentalSoundRule()
          {
            Latitude = generatorDefinition.SoundRules[index].Latitude,
            Height = generatorDefinition.SoundRules[index].Height,
            SunAngleFromZenith = generatorDefinition.SoundRules[index].SunAngleFromZenith,
            EnvironmentSound = MyStringHash.GetOrCompute(generatorDefinition.SoundRules[index].EnvironmentSound)
          };
          environmentalSoundRule.Latitude.ConvertToSine();
          environmentalSoundRule.SunAngleFromZenith.ConvertToCosine();
          this.SoundRules[index] = environmentalSoundRule;
        }
      }
      if (generatorDefinition.MusicCategories != null)
        this.MusicCategories = generatorDefinition.MusicCategories;
      if (generatorDefinition.HillParams.HasValue)
        this.HillParams = generatorDefinition.HillParams.Value;
      if (generatorDefinition.Atmosphere != null)
        this.Atmosphere = generatorDefinition.Atmosphere;
      if (generatorDefinition.GravityFalloffPower.HasValue)
        this.GravityFalloffPower = generatorDefinition.GravityFalloffPower.Value;
      if (generatorDefinition.HostileAtmosphereColorShift != null)
        this.HostileAtmosphereColorShift = generatorDefinition.HostileAtmosphereColorShift;
      if (generatorDefinition.MaterialsMaxDepth.HasValue)
        this.MaterialsMaxDepth = generatorDefinition.MaterialsMaxDepth.Value;
      if (generatorDefinition.MaterialsMinDepth.HasValue)
        this.MaterialsMinDepth = generatorDefinition.MaterialsMinDepth.Value;
      if (generatorDefinition.CustomMaterialTable != null && generatorDefinition.CustomMaterialTable.Length != 0)
      {
        this.SurfaceMaterialTable = new MyPlanetMaterialDefinition[generatorDefinition.CustomMaterialTable.Length];
        for (int index1 = 0; index1 < this.SurfaceMaterialTable.Length; ++index1)
        {
          this.SurfaceMaterialTable[index1] = generatorDefinition.CustomMaterialTable[index1].Clone() as MyPlanetMaterialDefinition;
          if (this.SurfaceMaterialTable[index1].Material == null && !this.SurfaceMaterialTable[index1].HasLayers)
            MyLog.Default.WriteLine("Custom material does not contain any material ids.");
          else if (this.SurfaceMaterialTable[index1].HasLayers)
          {
            float depth = this.SurfaceMaterialTable[index1].Layers[0].Depth;
            for (int index2 = 1; index2 < this.SurfaceMaterialTable[index1].Layers.Length; ++index2)
            {
              this.SurfaceMaterialTable[index1].Layers[index2].Depth += depth;
              depth = this.SurfaceMaterialTable[index1].Layers[index2].Depth;
            }
          }
        }
      }
      if (generatorDefinition.DistortionTable != null && generatorDefinition.DistortionTable.Length != 0)
        this.DistortionTable = generatorDefinition.DistortionTable;
      if (generatorDefinition.DefaultSurfaceMaterial != null)
        this.DefaultSurfaceMaterial = generatorDefinition.DefaultSurfaceMaterial;
      if (generatorDefinition.DefaultSubSurfaceMaterial != null)
        this.DefaultSubSurfaceMaterial = generatorDefinition.DefaultSubSurfaceMaterial;
      if (generatorDefinition.SurfaceGravity.HasValue)
        this.SurfaceGravity = generatorDefinition.SurfaceGravity.Value;
      if (generatorDefinition.AtmosphereSettings.HasValue)
        this.AtmosphereSettings = generatorDefinition.AtmosphereSettings;
      this.FolderName = generatorDefinition.FolderName != null ? generatorDefinition.FolderName : generatorDefinition.Id.SubtypeName;
      if (generatorDefinition.ComplexMaterials != null && generatorDefinition.ComplexMaterials.Length != 0)
      {
        this.MaterialGroups = new MyPlanetMaterialGroup[generatorDefinition.ComplexMaterials.Length];
        for (int index1 = 0; index1 < generatorDefinition.ComplexMaterials.Length; ++index1)
        {
          this.MaterialGroups[index1] = generatorDefinition.ComplexMaterials[index1].Clone() as MyPlanetMaterialGroup;
          MyPlanetMaterialGroup materialGroup = this.MaterialGroups[index1];
          MyPlanetMaterialPlacementRule[] self = materialGroup.MaterialRules;
          List<int> indices = new List<int>();
          for (int index2 = 0; index2 < self.Length; ++index2)
          {
            if (self[index2].Material == null && (self[index2].Layers == null || self[index2].Layers.Length == 0))
            {
              MyLog.Default.WriteLine("Material rule does not contain any material ids.");
              indices.Add(index2);
            }
            else
            {
              if (self[index2].Layers != null && self[index2].Layers.Length != 0)
              {
                float depth = self[index2].Layers[0].Depth;
                for (int index3 = 1; index3 < self[index2].Layers.Length; ++index3)
                {
                  self[index2].Layers[index3].Depth += depth;
                  depth = self[index2].Layers[index3].Depth;
                }
              }
              self[index2].Slope.ConvertToCosine();
              self[index2].Latitude.ConvertToSine();
              self[index2].Longitude.ConvertToCosineLongitude();
            }
          }
          if (indices.Count > 0)
            self = self.RemoveIndices<MyPlanetMaterialPlacementRule>(indices);
          materialGroup.MaterialRules = self;
        }
      }
      if (generatorDefinition.OreMappings != null)
        this.OreMappings = generatorDefinition.OreMappings;
      if (generatorDefinition.MaterialBlending.HasValue)
        this.MaterialBlending = generatorDefinition.MaterialBlending.Value;
      if (generatorDefinition.SurfaceDetail != null)
        this.Detail = generatorDefinition.SurfaceDetail;
      if (generatorDefinition.AnimalSpawnInfo != null)
        this.AnimalSpawnInfo = generatorDefinition.AnimalSpawnInfo;
      if (generatorDefinition.NightAnimalSpawnInfo != null)
        this.NightAnimalSpawnInfo = generatorDefinition.NightAnimalSpawnInfo;
      if (generatorDefinition.SectorDensity.HasValue)
        this.SectorDensity = generatorDefinition.SectorDensity.Value;
      this.WeatherFrequencyMin = generatorDefinition.WeatherFrequencyMin;
      this.WeatherFrequencyMax = generatorDefinition.WeatherFrequencyMax;
      this.GlobalWeather = generatorDefinition.GlobalWeather;
      if (generatorDefinition.WeatherGenerators != null)
        this.WeatherGenerators = generatorDefinition.WeatherGenerators;
      this.DefaultSurfaceTemperature = generatorDefinition.DefaultSurfaceTemperature;
      this.StationBlockingMaterials = new List<MyDefinitionId>();
      if (generatorDefinition.StationBlockingMaterials != null)
      {
        foreach (SerializableDefinitionId blockingMaterial in generatorDefinition.StationBlockingMaterials)
          this.StationBlockingMaterials.Add((MyDefinitionId) blockingMaterial);
      }
      MyObjectBuilder_PlanetMapProvider planetMapProvider = generatorDefinition.MapProvider;
      if (planetMapProvider == null)
        planetMapProvider = (MyObjectBuilder_PlanetMapProvider) new MyObjectBuilder_PlanetTextureMapProvider()
        {
          Path = this.FolderName
        };
      this.MapProvider = planetMapProvider;
      this.MesherPostprocessing = generatorDefinition.MesherPostprocessing;
      if (this.MesherPostprocessing == null)
        MyLog.Default.Warning("PERFORMANCE WARNING: Postprocessing voxel triangle decimation steps not defined for " + (object) this);
      if (generatorDefinition.Difficulty != MyStringId.NullOrEmpty)
        this.Difficulty = generatorDefinition.Difficulty;
      this.MinimumSurfaceLayerDepth = generatorDefinition.MinimumSurfaceLayerDepth;
    }

    public override string ToString()
    {
      string str = base.ToString();
      foreach (FieldInfo field in typeof (MyPlanetGeneratorDefinition).GetFields())
      {
        if (field.IsPublic)
        {
          object obj = field.GetValue((object) this);
          str = str + "\n   " + field.Name + " = " + (obj ?? (object) "<null>");
        }
      }
      foreach (PropertyInfo property in typeof (MyPlanetGeneratorDefinition).GetProperties())
      {
        object obj = property.GetValue((object) this, (object[]) null);
        str = str + "\n   " + property.Name + " = " + (obj ?? (object) "<null>");
      }
      return str;
    }

    internal class Postprocessor : MyDefinitionPostprocessor
    {
      public override int Priority => 1000;

      public override void AfterLoaded(ref MyDefinitionPostprocessor.Bundle definitions)
      {
      }

      public override void AfterPostprocess(
        MyDefinitionSet set,
        Dictionary<MyStringHash, MyDefinitionBase> definitions)
      {
        List<int> indices = new List<int>();
        foreach (MyDefinitionBase myDefinitionBase in definitions.Values)
        {
          MyPlanetGeneratorDefinition generatorDefinition = (MyPlanetGeneratorDefinition) myDefinitionBase;
          if (!generatorDefinition.EnvironmentId.HasValue)
          {
            generatorDefinition.EnvironmentDefinition = MyProceduralEnvironmentDefinition.FromLegacyPlanet(generatorDefinition.m_pgob, myDefinitionBase.Context);
            set.AddOrRelaceDefinition((MyDefinitionBase) generatorDefinition.EnvironmentDefinition);
            generatorDefinition.m_pgob = (MyObjectBuilder_PlanetGeneratorDefinition) null;
          }
          else
            generatorDefinition.EnvironmentDefinition = MyDefinitionManager.Static.GetDefinition<MyWorldEnvironmentDefinition>(generatorDefinition.EnvironmentId.Value);
          if (generatorDefinition.EnvironmentDefinition != null)
          {
            generatorDefinition.EnvironmentSectorType = generatorDefinition.EnvironmentDefinition.SectorType;
            foreach (Dictionary<string, List<MyPlanetEnvironmentMapping>> dictionary in generatorDefinition.MaterialEnvironmentMappings.Values)
            {
              foreach (List<MyPlanetEnvironmentMapping> environmentMappingList in dictionary.Values)
              {
                foreach (MyPlanetEnvironmentMapping environmentMapping in environmentMappingList)
                {
                  for (int index = 0; index < environmentMapping.Items.Length; ++index)
                  {
                    if (environmentMapping.Items[index].IsEnvironemntItem && !MyDefinitionManager.Static.TryGetDefinition<MyEnvironmentItemsDefinition>(environmentMapping.Items[index].Definition, out MyEnvironmentItemsDefinition _))
                    {
                      MyLog.Default.WriteLine(string.Format("Could not find environment item definition for {0}.", (object) environmentMapping.Items[index].Definition));
                      indices.Add(index);
                    }
                  }
                  if (indices.Count > 0)
                  {
                    environmentMapping.Items = environmentMapping.Items.RemoveIndices<MyMaterialEnvironmentItem>(indices);
                    environmentMapping.ComputeDistribution();
                    indices.Clear();
                  }
                }
              }
            }
          }
        }
      }
    }

    private class Sandbox_Definitions_MyPlanetGeneratorDefinition\u003C\u003EActor : IActivator, IActivator<MyPlanetGeneratorDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyPlanetGeneratorDefinition();

      MyPlanetGeneratorDefinition IActivator<MyPlanetGeneratorDefinition>.CreateInstance() => new MyPlanetGeneratorDefinition();
    }
  }
}
