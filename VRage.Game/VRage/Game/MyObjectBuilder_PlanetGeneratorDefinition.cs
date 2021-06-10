// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_PlanetGeneratorDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.ObjectBuilders.Definitions.Components;
using VRage.Utils;
using VRageMath;
using VRageRender.Messages;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlType("PlanetGeneratorDefinition")]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_PlanetGeneratorDefinition : MyObjectBuilder_DefinitionBase
  {
    [ProtoMember(79)]
    public MyPlanetMaps? PlanetMaps;
    [ProtoMember(80)]
    public bool? HasAtmosphere;
    [ProtoMember(81)]
    [XmlArrayItem("CloudLayer")]
    public List<MyCloudLayerSettings> CloudLayers;
    [ProtoMember(82)]
    public SerializableRange? HillParams;
    [ProtoMember(83)]
    public float? GravityFalloffPower;
    [ProtoMember(84)]
    public SerializableRange? MaterialsMaxDepth;
    [ProtoMember(85)]
    public SerializableRange? MaterialsMinDepth;
    [ProtoMember(86)]
    public MyAtmosphereColorShift HostileAtmosphereColorShift;
    [ProtoMember(87)]
    [XmlArrayItem("Material")]
    public MyPlanetMaterialDefinition[] CustomMaterialTable;
    [ProtoMember(88)]
    [XmlArrayItem("Distortion")]
    public MyPlanetDistortionDefinition[] DistortionTable;
    [ProtoMember(89)]
    public MyPlanetMaterialDefinition DefaultSurfaceMaterial;
    [ProtoMember(90)]
    public MyPlanetMaterialDefinition DefaultSubSurfaceMaterial;
    [ProtoMember(91)]
    public float? SurfaceGravity;
    [ProtoMember(92)]
    public MyPlanetAtmosphere Atmosphere;
    [ProtoMember(93)]
    public MyAtmosphereSettings? AtmosphereSettings;
    [ProtoMember(94)]
    public string FolderName;
    [ProtoMember(95)]
    public MyPlanetMaterialGroup[] ComplexMaterials;
    [ProtoMember(96)]
    [XmlArrayItem("SoundRule")]
    public MySerializablePlanetEnvironmentalSoundRule[] SoundRules;
    [ProtoMember(97)]
    [XmlArrayItem("MusicCategory")]
    public List<MyMusicCategory> MusicCategories;
    [ProtoMember(98)]
    [XmlArrayItem("Ore")]
    public MyPlanetOreMapping[] OreMappings;
    [ProtoMember(99)]
    [XmlArrayItem("Item")]
    public PlanetEnvironmentItemMapping[] EnvironmentItems;
    [ProtoMember(100)]
    public MyPlanetMaterialBlendSettings? MaterialBlending;
    [ProtoMember(101)]
    public MyPlanetSurfaceDetail SurfaceDetail;
    [ProtoMember(102)]
    public MyPlanetAnimalSpawnInfo AnimalSpawnInfo;
    [ProtoMember(103)]
    public MyPlanetAnimalSpawnInfo NightAnimalSpawnInfo;
    public float? SectorDensity;
    [ProtoMember(104)]
    public string InheritFrom;
    public SerializableDefinitionId? Environment;
    [XmlElement(typeof (MyAbstractXmlSerializer<MyObjectBuilder_PlanetMapProvider>))]
    public MyObjectBuilder_PlanetMapProvider MapProvider;
    [ProtoMember(105)]
    public MyObjectBuilder_VoxelMesherComponentDefinition MesherPostprocessing;
    [ProtoMember(106)]
    public float MinimumSurfaceLayerDepth = 4f;
    [ProtoMember(108)]
    public List<SerializableDefinitionId> StationBlockingMaterials;
    [ProtoMember(110)]
    [DefaultValue(MyTemperatureLevel.Cozy)]
    public MyTemperatureLevel DefaultSurfaceTemperature = MyTemperatureLevel.Cozy;
    [ProtoMember(115)]
    [XmlArrayItem("WeatherGenerator")]
    public List<MyWeatherGeneratorSettings> WeatherGenerators;
    [ProtoMember(120)]
    public int WeatherFrequencyMin;
    [ProtoMember(121)]
    public int WeatherFrequencyMax;
    [ProtoMember(125)]
    public bool GlobalWeather;
    [ProtoMember(126)]
    public MyStringId Difficulty;

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EPlanetMaps\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyPlanetMaps?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in MyPlanetMaps? value)
      {
        owner.PlanetMaps = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out MyPlanetMaps? value)
      {
        value = owner.PlanetMaps;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EHasAtmosphere\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, bool?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in bool? value)
      {
        owner.HasAtmosphere = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out bool? value)
      {
        value = owner.HasAtmosphere;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003ECloudLayers\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, List<MyCloudLayerSettings>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in List<MyCloudLayerSettings> value)
      {
        owner.CloudLayers = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out List<MyCloudLayerSettings> value)
      {
        value = owner.CloudLayers;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EHillParams\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, SerializableRange?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in SerializableRange? value)
      {
        owner.HillParams = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out SerializableRange? value)
      {
        value = owner.HillParams;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EGravityFalloffPower\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, float?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in float? value)
      {
        owner.GravityFalloffPower = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out float? value)
      {
        value = owner.GravityFalloffPower;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EMaterialsMaxDepth\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, SerializableRange?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in SerializableRange? value)
      {
        owner.MaterialsMaxDepth = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out SerializableRange? value)
      {
        value = owner.MaterialsMaxDepth;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EMaterialsMinDepth\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, SerializableRange?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in SerializableRange? value)
      {
        owner.MaterialsMinDepth = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out SerializableRange? value)
      {
        value = owner.MaterialsMinDepth;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EHostileAtmosphereColorShift\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyAtmosphereColorShift>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in MyAtmosphereColorShift value)
      {
        owner.HostileAtmosphereColorShift = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out MyAtmosphereColorShift value)
      {
        value = owner.HostileAtmosphereColorShift;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003ECustomMaterialTable\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyPlanetMaterialDefinition[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in MyPlanetMaterialDefinition[] value)
      {
        owner.CustomMaterialTable = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out MyPlanetMaterialDefinition[] value)
      {
        value = owner.CustomMaterialTable;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EDistortionTable\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyPlanetDistortionDefinition[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in MyPlanetDistortionDefinition[] value)
      {
        owner.DistortionTable = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out MyPlanetDistortionDefinition[] value)
      {
        value = owner.DistortionTable;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EDefaultSurfaceMaterial\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyPlanetMaterialDefinition>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in MyPlanetMaterialDefinition value)
      {
        owner.DefaultSurfaceMaterial = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out MyPlanetMaterialDefinition value)
      {
        value = owner.DefaultSurfaceMaterial;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EDefaultSubSurfaceMaterial\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyPlanetMaterialDefinition>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in MyPlanetMaterialDefinition value)
      {
        owner.DefaultSubSurfaceMaterial = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out MyPlanetMaterialDefinition value)
      {
        value = owner.DefaultSubSurfaceMaterial;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003ESurfaceGravity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, float?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in float? value)
      {
        owner.SurfaceGravity = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out float? value)
      {
        value = owner.SurfaceGravity;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EAtmosphere\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyPlanetAtmosphere>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in MyPlanetAtmosphere value)
      {
        owner.Atmosphere = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out MyPlanetAtmosphere value)
      {
        value = owner.Atmosphere;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EAtmosphereSettings\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyAtmosphereSettings?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in MyAtmosphereSettings? value)
      {
        owner.AtmosphereSettings = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out MyAtmosphereSettings? value)
      {
        value = owner.AtmosphereSettings;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EFolderName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in string value)
      {
        owner.FolderName = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out string value)
      {
        value = owner.FolderName;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EComplexMaterials\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyPlanetMaterialGroup[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in MyPlanetMaterialGroup[] value)
      {
        owner.ComplexMaterials = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out MyPlanetMaterialGroup[] value)
      {
        value = owner.ComplexMaterials;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003ESoundRules\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MySerializablePlanetEnvironmentalSoundRule[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in MySerializablePlanetEnvironmentalSoundRule[] value)
      {
        owner.SoundRules = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out MySerializablePlanetEnvironmentalSoundRule[] value)
      {
        value = owner.SoundRules;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EMusicCategories\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, List<MyMusicCategory>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in List<MyMusicCategory> value)
      {
        owner.MusicCategories = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out List<MyMusicCategory> value)
      {
        value = owner.MusicCategories;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EOreMappings\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyPlanetOreMapping[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in MyPlanetOreMapping[] value)
      {
        owner.OreMappings = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out MyPlanetOreMapping[] value)
      {
        value = owner.OreMappings;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EEnvironmentItems\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, PlanetEnvironmentItemMapping[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in PlanetEnvironmentItemMapping[] value)
      {
        owner.EnvironmentItems = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out PlanetEnvironmentItemMapping[] value)
      {
        value = owner.EnvironmentItems;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EMaterialBlending\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyPlanetMaterialBlendSettings?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in MyPlanetMaterialBlendSettings? value)
      {
        owner.MaterialBlending = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out MyPlanetMaterialBlendSettings? value)
      {
        value = owner.MaterialBlending;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003ESurfaceDetail\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyPlanetSurfaceDetail>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in MyPlanetSurfaceDetail value)
      {
        owner.SurfaceDetail = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out MyPlanetSurfaceDetail value)
      {
        value = owner.SurfaceDetail;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EAnimalSpawnInfo\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyPlanetAnimalSpawnInfo>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in MyPlanetAnimalSpawnInfo value)
      {
        owner.AnimalSpawnInfo = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out MyPlanetAnimalSpawnInfo value)
      {
        value = owner.AnimalSpawnInfo;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003ENightAnimalSpawnInfo\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyPlanetAnimalSpawnInfo>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in MyPlanetAnimalSpawnInfo value)
      {
        owner.NightAnimalSpawnInfo = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out MyPlanetAnimalSpawnInfo value)
      {
        value = owner.NightAnimalSpawnInfo;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003ESectorDensity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, float?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in float? value)
      {
        owner.SectorDensity = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out float? value)
      {
        value = owner.SectorDensity;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EInheritFrom\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in string value)
      {
        owner.InheritFrom = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out string value)
      {
        value = owner.InheritFrom;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EEnvironment\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in SerializableDefinitionId? value)
      {
        owner.Environment = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out SerializableDefinitionId? value)
      {
        value = owner.Environment;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EMapProvider\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyObjectBuilder_PlanetMapProvider>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in MyObjectBuilder_PlanetMapProvider value)
      {
        owner.MapProvider = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out MyObjectBuilder_PlanetMapProvider value)
      {
        value = owner.MapProvider;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EMesherPostprocessing\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyObjectBuilder_VoxelMesherComponentDefinition>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in MyObjectBuilder_VoxelMesherComponentDefinition value)
      {
        owner.MesherPostprocessing = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out MyObjectBuilder_VoxelMesherComponentDefinition value)
      {
        value = owner.MesherPostprocessing;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EMinimumSurfaceLayerDepth\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in float value)
      {
        owner.MinimumSurfaceLayerDepth = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out float value)
      {
        value = owner.MinimumSurfaceLayerDepth;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EStationBlockingMaterials\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, List<SerializableDefinitionId>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in List<SerializableDefinitionId> value)
      {
        owner.StationBlockingMaterials = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out List<SerializableDefinitionId> value)
      {
        value = owner.StationBlockingMaterials;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EDefaultSurfaceTemperature\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyTemperatureLevel>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in MyTemperatureLevel value)
      {
        owner.DefaultSurfaceTemperature = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out MyTemperatureLevel value)
      {
        value = owner.DefaultSurfaceTemperature;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EWeatherGenerators\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, List<MyWeatherGeneratorSettings>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in List<MyWeatherGeneratorSettings> value)
      {
        owner.WeatherGenerators = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out List<MyWeatherGeneratorSettings> value)
      {
        value = owner.WeatherGenerators;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EWeatherFrequencyMin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in int value)
      {
        owner.WeatherFrequencyMin = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out int value)
      {
        value = owner.WeatherFrequencyMin;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EWeatherFrequencyMax\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in int value)
      {
        owner.WeatherFrequencyMax = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out int value)
      {
        value = owner.WeatherFrequencyMax;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EGlobalWeather\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in bool value)
      {
        owner.GlobalWeather = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out bool value)
      {
        value = owner.GlobalWeather;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EDifficulty\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyStringId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in MyStringId value)
      {
        owner.Difficulty = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out MyStringId value)
      {
        value = owner.Difficulty;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetGeneratorDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_PlanetGeneratorDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_PlanetGeneratorDefinition();

      MyObjectBuilder_PlanetGeneratorDefinition IActivator<MyObjectBuilder_PlanetGeneratorDefinition>.CreateInstance() => new MyObjectBuilder_PlanetGeneratorDefinition();
    }
  }
}
