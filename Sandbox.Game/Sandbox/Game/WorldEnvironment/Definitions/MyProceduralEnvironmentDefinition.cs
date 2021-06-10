// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.Definitions.MyProceduralEnvironmentDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.WorldEnvironment.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ProceduralWorldEnvironment), typeof (MyProceduralEnvironmentDefinitionPostprocessor))]
  public class MyProceduralEnvironmentDefinition : MyWorldEnvironmentDefinition
  {
    private static readonly int[] ArrayOfZero = new int[1];
    private MyObjectBuilder_ProceduralWorldEnvironment m_ob;
    public Dictionary<string, MyItemTypeDefinition> ItemTypes = new Dictionary<string, MyItemTypeDefinition>();
    public Dictionary<MyBiomeMaterial, List<MyEnvironmentItemMapping>> MaterialEnvironmentMappings;
    public MyProceduralScanningMethod ScanningMethod;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ProceduralWorldEnvironment worldEnvironment = (MyObjectBuilder_ProceduralWorldEnvironment) builder;
      this.m_ob = worldEnvironment;
      this.ScanningMethod = worldEnvironment.ScanningMethod;
    }

    public void Prepare()
    {
      if (this.m_ob.ItemTypes != null)
      {
        foreach (MyEnvironmentItemTypeDefinition itemType in this.m_ob.ItemTypes)
        {
          try
          {
            MyItemTypeDefinition itemTypeDefinition = new MyItemTypeDefinition(itemType);
            this.ItemTypes.Add(itemType.Name, itemTypeDefinition);
          }
          catch (ArgumentException ex)
          {
            MyLog.Default.Error("Duplicate environment item definition for item {0}.", (object) itemType.Name);
          }
          catch (Exception ex)
          {
            MyLog.Default.Error("Error preparing environment item definition for item {0}:\n {1}", (object) itemType.Name, (object) ex.Message);
          }
        }
      }
      this.MaterialEnvironmentMappings = new Dictionary<MyBiomeMaterial, List<MyEnvironmentItemMapping>>(MyBiomeMaterial.Comparer);
      List<MyRuntimeEnvironmentItemInfo> environmentItemInfoList = new List<MyRuntimeEnvironmentItemInfo>();
      MyProceduralEnvironmentMapping[] environmentMappings = this.m_ob.EnvironmentMappings;
      if (environmentMappings != null && environmentMappings.Length != 0)
      {
        this.MaterialEnvironmentMappings = new Dictionary<MyBiomeMaterial, List<MyEnvironmentItemMapping>>(MyBiomeMaterial.Comparer);
        for (int index1 = 0; index1 < environmentMappings.Length; ++index1)
        {
          MyProceduralEnvironmentMapping environmentMapping = environmentMappings[index1];
          MyEnvironmentRule rule = new MyEnvironmentRule()
          {
            Height = environmentMapping.Height,
            Slope = environmentMapping.Slope,
            Latitude = environmentMapping.Latitude,
            Longitude = environmentMapping.Longitude
          };
          if (environmentMapping.Materials == null)
          {
            MyLog.Default.Warning("Mapping in definition {0} does not define any materials, it will not be applied.", (object) this.Id);
          }
          else
          {
            if (environmentMapping.Biomes == null)
              environmentMapping.Biomes = MyProceduralEnvironmentDefinition.ArrayOfZero;
            bool flag = false;
            MyRuntimeEnvironmentItemInfo[] map = new MyRuntimeEnvironmentItemInfo[environmentMapping.Items.Length];
            for (int index2 = 0; index2 < environmentMapping.Items.Length; ++index2)
            {
              if (!this.ItemTypes.ContainsKey(environmentMapping.Items[index2].Type))
              {
                MyLog.Default.Error("No definition for item type {0}", (object) environmentMapping.Items[index2].Type);
              }
              else
              {
                map[index2] = new MyRuntimeEnvironmentItemInfo(this, environmentMapping.Items[index2], environmentItemInfoList.Count);
                environmentItemInfoList.Add(map[index2]);
                flag = true;
              }
            }
            if (flag)
            {
              MyEnvironmentItemMapping environmentItemMapping = new MyEnvironmentItemMapping(map, rule, this);
              foreach (int biome in environmentMapping.Biomes)
              {
                foreach (string material in environmentMapping.Materials)
                {
                  if (MyDefinitionManager.Static.GetVoxelMaterialDefinition(material) != null)
                  {
                    MyBiomeMaterial key = new MyBiomeMaterial((byte) biome, MyDefinitionManager.Static.GetVoxelMaterialDefinition(material).Index);
                    List<MyEnvironmentItemMapping> environmentItemMappingList;
                    if (!this.MaterialEnvironmentMappings.TryGetValue(key, out environmentItemMappingList))
                    {
                      environmentItemMappingList = new List<MyEnvironmentItemMapping>();
                      this.MaterialEnvironmentMappings[key] = environmentItemMappingList;
                    }
                    environmentItemMappingList.Add(environmentItemMapping);
                  }
                }
              }
            }
          }
        }
      }
      this.Items = environmentItemInfoList.ToArray();
      this.m_ob = (MyObjectBuilder_ProceduralWorldEnvironment) null;
    }

    public override Type SectorType => typeof (MyEnvironmentSector);

    public static MyWorldEnvironmentDefinition FromLegacyPlanet(
      MyObjectBuilder_PlanetGeneratorDefinition pgdef,
      MyModContext context)
    {
      MyObjectBuilder_ProceduralWorldEnvironment newObject1 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ProceduralWorldEnvironment>(pgdef.Id.SubtypeId);
      newObject1.Id = new SerializableDefinitionId(newObject1.TypeId, newObject1.SubtypeName);
      SerializableDefinitionId serializableDefinitionId1 = new SerializableDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_ProceduralEnvironmentModuleDefinition), "Static");
      SerializableDefinitionId serializableDefinitionId2 = new SerializableDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_ProceduralEnvironmentModuleDefinition), "Memory");
      SerializableDefinitionId serializableDefinitionId3 = new SerializableDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_EnvironmentModuleProxyDefinition), "Breakable");
      SerializableDefinitionId serializableDefinitionId4 = new SerializableDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_EnvironmentModuleProxyDefinition), "VoxelMap");
      SerializableDefinitionId serializableDefinitionId5 = new SerializableDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_EnvironmentModuleProxyDefinition), "BotSpawner");
      SerializableDefinitionId serializableDefinitionId6 = new SerializableDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_EnvironmentModuleProxyDefinition), "EnvironmentalParticles");
      newObject1.ItemTypes = new MyEnvironmentItemTypeDefinition[4]
      {
        new MyEnvironmentItemTypeDefinition()
        {
          LodFrom = -1,
          LodTo = 1,
          Name = "Tree",
          Provider = new SerializableDefinitionId?(serializableDefinitionId1),
          Proxies = new SerializableDefinitionId[1]
          {
            serializableDefinitionId3
          }
        },
        new MyEnvironmentItemTypeDefinition()
        {
          LodFrom = 0,
          LodTo = -1,
          Name = "Bush",
          Provider = new SerializableDefinitionId?(serializableDefinitionId1),
          Proxies = new SerializableDefinitionId[1]
          {
            serializableDefinitionId3
          }
        },
        new MyEnvironmentItemTypeDefinition()
        {
          LodFrom = 0,
          LodTo = -1,
          Name = "VoxelMap",
          Provider = new SerializableDefinitionId?(serializableDefinitionId2),
          Proxies = new SerializableDefinitionId[1]
          {
            serializableDefinitionId4
          }
        },
        new MyEnvironmentItemTypeDefinition()
        {
          LodFrom = 0,
          LodTo = -1,
          Name = "Bot",
          Provider = new SerializableDefinitionId?(),
          Proxies = new SerializableDefinitionId[1]
          {
            serializableDefinitionId5
          }
        }
      };
      newObject1.ScanningMethod = MyProceduralScanningMethod.Random;
      newObject1.ItemsPerSqMeter = 0.0034;
      newObject1.MaxSyncLod = 0;
      newObject1.SectorSize = 200.0;
      List<MyProceduralEnvironmentMapping> environmentMappingList = new List<MyProceduralEnvironmentMapping>();
      List<MyEnvironmentItemInfo> environmentItemInfoList = new List<MyEnvironmentItemInfo>();
      MyPlanetSurfaceRule planetSurfaceRule1 = new MyPlanetSurfaceRule();
      if (pgdef.EnvironmentItems != null)
      {
        foreach (PlanetEnvironmentItemMapping environmentItem in pgdef.EnvironmentItems)
        {
          MyProceduralEnvironmentMapping environmentMapping = new MyProceduralEnvironmentMapping();
          environmentMapping.Biomes = environmentItem.Biomes;
          environmentMapping.Materials = environmentItem.Materials;
          MyPlanetSurfaceRule planetSurfaceRule2 = environmentItem.Rule ?? planetSurfaceRule1;
          environmentMapping.Height = planetSurfaceRule2.Height;
          environmentMapping.Latitude = planetSurfaceRule2.Latitude;
          environmentMapping.Longitude = planetSurfaceRule2.Longitude;
          environmentMapping.Slope = planetSurfaceRule2.Slope;
          environmentItemInfoList.Clear();
          foreach (MyPlanetEnvironmentItemDef environmentItemDef in environmentItem.Items)
          {
            MyEnvironmentItemInfo environmentItemInfo = new MyEnvironmentItemInfo()
            {
              Density = environmentItemDef.Density,
              Subtype = MyStringHash.GetOrCompute(environmentItemDef.SubtypeId)
            };
            string typeId = environmentItemDef.TypeId;
            if (!(typeId == "MyObjectBuilder_DestroyableItems"))
            {
              if (!(typeId == "MyObjectBuilder_Trees"))
              {
                if (typeId == "MyObjectBuilder_VoxelMapStorageDefinition")
                {
                  environmentItemInfo.Type = "VoxelMap";
                  environmentItemInfo.Density *= 0.5f;
                  if (environmentItemDef.SubtypeId == null)
                  {
                    MyStringHash orCompute = MyStringHash.GetOrCompute(string.Format("G({0})M({1})", (object) environmentItemDef.GroupId, (object) environmentItemDef.ModifierId));
                    if (MyDefinitionManager.Static.GetDefinition<MyVoxelMapCollectionDefinition>(orCompute) == null)
                    {
                      MyObjectBuilder_VoxelMapCollectionDefinition newObject2 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_VoxelMapCollectionDefinition>(orCompute.ToString());
                      newObject2.Id = new SerializableDefinitionId(newObject2.TypeId, newObject2.SubtypeName);
                      newObject2.StorageDefs = new MyObjectBuilder_VoxelMapCollectionDefinition.VoxelMapStorage[1]
                      {
                        new MyObjectBuilder_VoxelMapCollectionDefinition.VoxelMapStorage()
                        {
                          Storage = environmentItemDef.GroupId
                        }
                      };
                      newObject2.Modifier = environmentItemDef.ModifierId;
                      MyVoxelMapCollectionDefinition collectionDefinition = new MyVoxelMapCollectionDefinition();
                      collectionDefinition.Init((MyObjectBuilder_DefinitionBase) newObject2, context);
                      MyDefinitionManager.Static.Definitions.AddDefinition((MyDefinitionBase) collectionDefinition);
                    }
                    environmentItemInfo.Subtype = orCompute;
                  }
                }
                else
                {
                  MyLog.Default.Error("Planet Generator {0}: Invalid Item Type: {1}", (object) pgdef.SubtypeName, (object) environmentItemDef.SubtypeId);
                  continue;
                }
              }
              else
                environmentItemInfo.Type = "Tree";
            }
            else
            {
              environmentItemInfo.Type = "Bush";
              environmentItemInfo.Density *= 0.5f;
            }
            MyStringHash subtype = environmentItemInfo.Subtype;
            environmentItemInfoList.Add(environmentItemInfo);
          }
          environmentMapping.Items = environmentItemInfoList.ToArray();
          environmentMappingList.Add(environmentMapping);
        }
      }
      environmentMappingList.Capacity = environmentMappingList.Count;
      newObject1.EnvironmentMappings = environmentMappingList.ToArray();
      MyProceduralEnvironmentDefinition environmentDefinition = new MyProceduralEnvironmentDefinition();
      environmentDefinition.Context = context;
      environmentDefinition.Init((MyObjectBuilder_DefinitionBase) newObject1);
      return (MyWorldEnvironmentDefinition) environmentDefinition;
    }

    public void GetItemDefinition(ushort definitionIndex, out MyRuntimeEnvironmentItemInfo def)
    {
      if ((int) definitionIndex >= this.Items.Length)
        def = (MyRuntimeEnvironmentItemInfo) null;
      else
        def = this.Items[(int) definitionIndex];
    }

    private class Sandbox_Game_WorldEnvironment_Definitions_MyProceduralEnvironmentDefinition\u003C\u003EActor : IActivator, IActivator<MyProceduralEnvironmentDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyProceduralEnvironmentDefinition();

      MyProceduralEnvironmentDefinition IActivator<MyProceduralEnvironmentDefinition>.CreateInstance() => new MyProceduralEnvironmentDefinition();
    }
  }
}
