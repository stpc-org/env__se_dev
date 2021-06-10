// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyScenarioDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ScenarioDefinition), null)]
  public class MyScenarioDefinition : MyDefinitionBase
  {
    public MyDefinitionId GameDefinition;
    public MyDefinitionId Environment;
    public BoundingBoxD? WorldBoundaries;
    public MyWorldGeneratorStartingStateBase[] PossiblePlayerStarts;
    public MyWorldGeneratorOperationBase[] WorldGeneratorOperations;
    public bool AsteroidClustersEnabled;
    public float AsteroidClustersOffset;
    public bool CentralClusterEnabled;
    public MyEnvironmentHostilityEnum DefaultEnvironment;
    public MyStringId[] CreativeModeWeapons;
    public MyStringId[] SurvivalModeWeapons;
    public MyScenarioDefinition.StartingItem[] CreativeModeComponents;
    public MyScenarioDefinition.StartingItem[] SurvivalModeComponents;
    public MyScenarioDefinition.StartingPhysicalItem[] CreativeModePhysicalItems;
    public MyScenarioDefinition.StartingPhysicalItem[] SurvivalModePhysicalItems;
    public MyScenarioDefinition.StartingItem[] CreativeModeAmmoItems;
    public MyScenarioDefinition.StartingItem[] SurvivalModeAmmoItems;
    public MyObjectBuilder_InventoryItem[] CreativeInventoryItems;
    public MyObjectBuilder_InventoryItem[] SurvivalInventoryItems;
    public MyObjectBuilder_Toolbar CreativeDefaultToolbar;
    public MyObjectBuilder_Toolbar SurvivalDefaultToolbar;
    public MyStringId MainCharacterModel;
    public DateTime GameDate;
    public Vector3 SunDirection;

    public bool HasPlanets => this.WorldGeneratorOperations != null && ((IEnumerable<MyWorldGeneratorOperationBase>) this.WorldGeneratorOperations).Any<MyWorldGeneratorOperationBase>((Func<MyWorldGeneratorOperationBase, bool>) (s => s is MyWorldGenerator.OperationAddPlanetPrefab || s is MyWorldGenerator.OperationCreatePlanet));

    public MyObjectBuilder_Toolbar DefaultToolbar => !MySession.Static.CreativeMode ? this.SurvivalDefaultToolbar : this.CreativeDefaultToolbar;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ScenarioDefinition scenarioDefinition = (MyObjectBuilder_ScenarioDefinition) builder;
      this.GameDefinition = (MyDefinitionId) scenarioDefinition.GameDefinition;
      this.Environment = (MyDefinitionId) scenarioDefinition.EnvironmentDefinition;
      this.AsteroidClustersEnabled = scenarioDefinition.AsteroidClusters.Enabled;
      this.AsteroidClustersOffset = scenarioDefinition.AsteroidClusters.Offset;
      this.CentralClusterEnabled = scenarioDefinition.AsteroidClusters.CentralCluster;
      this.DefaultEnvironment = scenarioDefinition.DefaultEnvironment;
      this.CreativeDefaultToolbar = scenarioDefinition.CreativeDefaultToolbar;
      this.SurvivalDefaultToolbar = scenarioDefinition.SurvivalDefaultToolbar;
      this.MainCharacterModel = MyStringId.GetOrCompute(scenarioDefinition.MainCharacterModel);
      this.GameDate = new DateTime(scenarioDefinition.GameDate);
      this.SunDirection = (Vector3) scenarioDefinition.SunDirection;
      if (scenarioDefinition.PossibleStartingStates != null && scenarioDefinition.PossibleStartingStates.Length != 0)
      {
        this.PossiblePlayerStarts = new MyWorldGeneratorStartingStateBase[scenarioDefinition.PossibleStartingStates.Length];
        for (int index = 0; index < scenarioDefinition.PossibleStartingStates.Length; ++index)
          this.PossiblePlayerStarts[index] = MyWorldGenerator.StartingStateFactory.CreateInstance(scenarioDefinition.PossibleStartingStates[index]);
      }
      if (scenarioDefinition.WorldGeneratorOperations != null && scenarioDefinition.WorldGeneratorOperations.Length != 0)
      {
        this.WorldGeneratorOperations = new MyWorldGeneratorOperationBase[scenarioDefinition.WorldGeneratorOperations.Length];
        for (int index = 0; index < scenarioDefinition.WorldGeneratorOperations.Length; ++index)
          this.WorldGeneratorOperations[index] = MyWorldGenerator.OperationFactory.CreateInstance(scenarioDefinition.WorldGeneratorOperations[index]);
      }
      if (scenarioDefinition.CreativeModeWeapons != null && scenarioDefinition.CreativeModeWeapons.Length != 0)
      {
        this.CreativeModeWeapons = new MyStringId[scenarioDefinition.CreativeModeWeapons.Length];
        for (int index = 0; index < scenarioDefinition.CreativeModeWeapons.Length; ++index)
          this.CreativeModeWeapons[index] = MyStringId.GetOrCompute(scenarioDefinition.CreativeModeWeapons[index]);
      }
      if (scenarioDefinition.SurvivalModeWeapons != null && scenarioDefinition.SurvivalModeWeapons.Length != 0)
      {
        this.SurvivalModeWeapons = new MyStringId[scenarioDefinition.SurvivalModeWeapons.Length];
        for (int index = 0; index < scenarioDefinition.SurvivalModeWeapons.Length; ++index)
          this.SurvivalModeWeapons[index] = MyStringId.GetOrCompute(scenarioDefinition.SurvivalModeWeapons[index]);
      }
      if (scenarioDefinition.CreativeModeComponents != null && scenarioDefinition.CreativeModeComponents.Length != 0)
      {
        this.CreativeModeComponents = new MyScenarioDefinition.StartingItem[scenarioDefinition.CreativeModeComponents.Length];
        for (int index = 0; index < scenarioDefinition.CreativeModeComponents.Length; ++index)
        {
          this.CreativeModeComponents[index].amount = (MyFixedPoint) scenarioDefinition.CreativeModeComponents[index].amount;
          this.CreativeModeComponents[index].itemName = MyStringId.GetOrCompute(scenarioDefinition.CreativeModeComponents[index].itemName);
        }
      }
      if (scenarioDefinition.SurvivalModeComponents != null && scenarioDefinition.SurvivalModeComponents.Length != 0)
      {
        this.SurvivalModeComponents = new MyScenarioDefinition.StartingItem[scenarioDefinition.SurvivalModeComponents.Length];
        for (int index = 0; index < scenarioDefinition.SurvivalModeComponents.Length; ++index)
        {
          this.SurvivalModeComponents[index].amount = (MyFixedPoint) scenarioDefinition.SurvivalModeComponents[index].amount;
          this.SurvivalModeComponents[index].itemName = MyStringId.GetOrCompute(scenarioDefinition.SurvivalModeComponents[index].itemName);
        }
      }
      if (scenarioDefinition.CreativeModePhysicalItems != null && scenarioDefinition.CreativeModePhysicalItems.Length != 0)
      {
        this.CreativeModePhysicalItems = new MyScenarioDefinition.StartingPhysicalItem[scenarioDefinition.CreativeModePhysicalItems.Length];
        for (int index = 0; index < scenarioDefinition.CreativeModePhysicalItems.Length; ++index)
        {
          this.CreativeModePhysicalItems[index].amount = (MyFixedPoint) scenarioDefinition.CreativeModePhysicalItems[index].amount;
          this.CreativeModePhysicalItems[index].itemName = MyStringId.GetOrCompute(scenarioDefinition.CreativeModePhysicalItems[index].itemName);
          this.CreativeModePhysicalItems[index].itemType = MyStringId.GetOrCompute(scenarioDefinition.CreativeModePhysicalItems[index].itemType);
        }
      }
      if (scenarioDefinition.SurvivalModePhysicalItems != null && scenarioDefinition.SurvivalModePhysicalItems.Length != 0)
      {
        this.SurvivalModePhysicalItems = new MyScenarioDefinition.StartingPhysicalItem[scenarioDefinition.SurvivalModePhysicalItems.Length];
        for (int index = 0; index < scenarioDefinition.SurvivalModePhysicalItems.Length; ++index)
        {
          this.SurvivalModePhysicalItems[index].amount = (MyFixedPoint) scenarioDefinition.SurvivalModePhysicalItems[index].amount;
          this.SurvivalModePhysicalItems[index].itemName = MyStringId.GetOrCompute(scenarioDefinition.SurvivalModePhysicalItems[index].itemName);
          this.SurvivalModePhysicalItems[index].itemType = MyStringId.GetOrCompute(scenarioDefinition.SurvivalModePhysicalItems[index].itemType);
        }
      }
      if (scenarioDefinition.CreativeModeAmmoItems != null && scenarioDefinition.CreativeModeAmmoItems.Length != 0)
      {
        this.CreativeModeAmmoItems = new MyScenarioDefinition.StartingItem[scenarioDefinition.CreativeModeAmmoItems.Length];
        for (int index = 0; index < scenarioDefinition.CreativeModeAmmoItems.Length; ++index)
        {
          this.CreativeModeAmmoItems[index].amount = (MyFixedPoint) scenarioDefinition.CreativeModeAmmoItems[index].amount;
          this.CreativeModeAmmoItems[index].itemName = MyStringId.GetOrCompute(scenarioDefinition.CreativeModeAmmoItems[index].itemName);
        }
      }
      if (scenarioDefinition.SurvivalModeAmmoItems != null && scenarioDefinition.SurvivalModeAmmoItems.Length != 0)
      {
        this.SurvivalModeAmmoItems = new MyScenarioDefinition.StartingItem[scenarioDefinition.SurvivalModeAmmoItems.Length];
        for (int index = 0; index < scenarioDefinition.SurvivalModeAmmoItems.Length; ++index)
        {
          this.SurvivalModeAmmoItems[index].amount = (MyFixedPoint) scenarioDefinition.SurvivalModeAmmoItems[index].amount;
          this.SurvivalModeAmmoItems[index].itemName = MyStringId.GetOrCompute(scenarioDefinition.SurvivalModeAmmoItems[index].itemName);
        }
      }
      this.CreativeInventoryItems = scenarioDefinition.CreativeInventoryItems;
      this.SurvivalInventoryItems = scenarioDefinition.SurvivalInventoryItems;
      SerializableBoundingBoxD? worldBoundaries = scenarioDefinition.WorldBoundaries;
      this.WorldBoundaries = worldBoundaries.HasValue ? new BoundingBoxD?((BoundingBoxD) worldBoundaries.GetValueOrDefault()) : new BoundingBoxD?();
    }

    public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
    {
      MyObjectBuilder_ScenarioDefinition objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_ScenarioDefinition;
      objectBuilder.AsteroidClusters.Enabled = this.AsteroidClustersEnabled;
      objectBuilder.AsteroidClusters.Offset = this.AsteroidClustersOffset;
      objectBuilder.AsteroidClusters.CentralCluster = this.CentralClusterEnabled;
      objectBuilder.DefaultEnvironment = this.DefaultEnvironment;
      objectBuilder.CreativeDefaultToolbar = this.CreativeDefaultToolbar;
      objectBuilder.SurvivalDefaultToolbar = this.SurvivalDefaultToolbar;
      objectBuilder.MainCharacterModel = this.MainCharacterModel.ToString();
      objectBuilder.GameDate = this.GameDate.Ticks;
      if (this.PossiblePlayerStarts != null && this.PossiblePlayerStarts.Length != 0)
      {
        objectBuilder.PossibleStartingStates = new MyObjectBuilder_WorldGeneratorPlayerStartingState[this.PossiblePlayerStarts.Length];
        for (int index = 0; index < this.PossiblePlayerStarts.Length; ++index)
          objectBuilder.PossibleStartingStates[index] = this.PossiblePlayerStarts[index].GetObjectBuilder();
      }
      if (this.WorldGeneratorOperations != null && this.WorldGeneratorOperations.Length != 0)
      {
        objectBuilder.WorldGeneratorOperations = new MyObjectBuilder_WorldGeneratorOperation[this.WorldGeneratorOperations.Length];
        for (int index = 0; index < this.WorldGeneratorOperations.Length; ++index)
          objectBuilder.WorldGeneratorOperations[index] = this.WorldGeneratorOperations[index].GetObjectBuilder();
      }
      if (this.CreativeModeWeapons != null && this.CreativeModeWeapons.Length != 0)
      {
        objectBuilder.CreativeModeWeapons = new string[this.CreativeModeWeapons.Length];
        for (int index = 0; index < this.CreativeModeWeapons.Length; ++index)
          objectBuilder.CreativeModeWeapons[index] = this.CreativeModeWeapons[index].ToString();
      }
      if (this.SurvivalModeWeapons != null && this.SurvivalModeWeapons.Length != 0)
      {
        objectBuilder.SurvivalModeWeapons = new string[this.SurvivalModeWeapons.Length];
        for (int index = 0; index < this.SurvivalModeWeapons.Length; ++index)
          objectBuilder.SurvivalModeWeapons[index] = this.SurvivalModeWeapons[index].ToString();
      }
      return (MyObjectBuilder_DefinitionBase) objectBuilder;
    }

    public struct StartingItem
    {
      public MyFixedPoint amount;
      public MyStringId itemName;
    }

    public struct StartingPhysicalItem
    {
      public MyFixedPoint amount;
      public MyStringId itemName;
      public MyStringId itemType;
    }

    private class Sandbox_Definitions_MyScenarioDefinition\u003C\u003EActor : IActivator, IActivator<MyScenarioDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyScenarioDefinition();

      MyScenarioDefinition IActivator<MyScenarioDefinition>.CreateInstance() => new MyScenarioDefinition();
    }
  }
}
