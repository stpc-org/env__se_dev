// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Character.Components.MyCharacterOxygenComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Entities.Character.Components
{
  public class MyCharacterOxygenComponent : MyCharacterComponent
  {
    public static readonly float LOW_OXYGEN_RATIO = 0.2f;
    public static readonly float GAS_REFILL_RATION = 0.3f;
    private Dictionary<MyDefinitionId, int> m_gasIdToIndex;
    private MyCharacterOxygenComponent.GasData[] m_storedGases;
    private float m_oldSuitOxygenLevel;
    private const int m_gasRefillInterval = 5;
    private int m_lastOxygenUpdateTime;
    private const int m_updateInterval = 100;
    private MyResourceSinkComponent m_characterGasSink;
    private MyResourceSourceComponent m_characterGasSource;
    public static readonly MyDefinitionId OxygenId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GasProperties), "Oxygen");
    public static readonly MyDefinitionId HydrogenId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GasProperties), "Hydrogen");
    private MyEntity3DSoundEmitter m_soundEmitter;
    private MySoundPair m_helmetOpenSound = new MySoundPair("PlayHelmetOn");
    private MySoundPair m_helmetCloseSound = new MySoundPair("PlayHelmetOff");
    private MySoundPair m_helmetAirEscapeSound = new MySoundPair("PlayChokeInitiate");

    public MyResourceSinkComponent CharacterGasSink
    {
      get => this.m_characterGasSink;
      set => this.SetGasSink(value);
    }

    public MyResourceSourceComponent CharacterGasSource
    {
      get => this.m_characterGasSource;
      set => this.SetGasSource(value);
    }

    private MyCharacterDefinition Definition => this.Character.Definition;

    public float OxygenCapacity
    {
      get
      {
        int typeIndex = -1;
        MyDefinitionId oxygenId = MyCharacterOxygenComponent.OxygenId;
        return !this.TryGetTypeIndex(ref oxygenId, out typeIndex) ? 0.0f : this.m_storedGases[typeIndex].MaxCapacity;
      }
    }

    public float SuitOxygenAmount
    {
      get => this.GetGasFillLevel(MyCharacterOxygenComponent.OxygenId) * this.OxygenCapacity;
      set
      {
        MyDefinitionId oxygenId = MyCharacterOxygenComponent.OxygenId;
        this.UpdateStoredGasLevel(ref oxygenId, MyMath.Clamp(value / this.OxygenCapacity, 0.0f, 1f));
      }
    }

    public float SuitOxygenAmountMissing => this.OxygenCapacity - this.GetGasFillLevel(MyCharacterOxygenComponent.OxygenId) * this.OxygenCapacity;

    public float SuitOxygenLevel
    {
      get => (double) this.OxygenCapacity == 0.0 ? 0.0f : this.GetGasFillLevel(MyCharacterOxygenComponent.OxygenId);
      set
      {
        MyDefinitionId oxygenId = MyCharacterOxygenComponent.OxygenId;
        this.UpdateStoredGasLevel(ref oxygenId, value);
      }
    }

    public bool HelmetEnabled => !this.NeedsOxygenFromSuit;

    public bool NeedsOxygenFromSuit { get; set; }

    public override string ComponentTypeDebugString => "Oxygen Component";

    public float EnvironmentOxygenLevel => this.Character.EnvironmentOxygenLevel;

    public float OxygenLevelAtCharacterLocation => this.Character.OxygenLevel;

    public virtual void Init(MyObjectBuilder_Character characterOb)
    {
      this.m_lastOxygenUpdateTime = MySession.Static.GameplayFrameCounter;
      this.m_gasIdToIndex = new Dictionary<MyDefinitionId, int>();
      if (MyFakes.ENABLE_HYDROGEN_FUEL && this.Definition.SuitResourceStorage != null)
      {
        this.m_storedGases = new MyCharacterOxygenComponent.GasData[this.Definition.SuitResourceStorage.Count];
        for (int index = 0; index < this.m_storedGases.Length; ++index)
        {
          SuitResourceDefinition resourceDefinition = this.Definition.SuitResourceStorage[index];
          this.m_storedGases[index] = new MyCharacterOxygenComponent.GasData()
          {
            Id = (MyDefinitionId) resourceDefinition.Id,
            FillLevel = 1f,
            MaxCapacity = resourceDefinition.MaxCapacity,
            Throughput = resourceDefinition.Throughput,
            LastOutputTime = MySession.Static.GameplayFrameCounter,
            LastInputTime = MySession.Static.GameplayFrameCounter
          };
          this.m_gasIdToIndex.Add((MyDefinitionId) resourceDefinition.Id, index);
        }
        if (characterOb.StoredGases != null && !MySession.Static.CreativeMode)
        {
          foreach (MyObjectBuilder_Character.StoredGas storedGase in characterOb.StoredGases)
          {
            int index;
            if (this.m_gasIdToIndex.TryGetValue((MyDefinitionId) storedGase.Id, out index))
              this.m_storedGases[index].FillLevel = storedGase.FillLevel;
          }
        }
      }
      if (this.m_storedGases == null)
        this.m_storedGases = new MyCharacterOxygenComponent.GasData[0];
      if (MySession.Static.Settings.EnableOxygen)
      {
        float gasFillLevel = this.GetGasFillLevel(MyCharacterOxygenComponent.OxygenId);
        this.m_oldSuitOxygenLevel = (double) gasFillLevel == 0.0 ? this.OxygenCapacity : gasFillLevel;
      }
      if (Sync.IsServer)
      {
        this.Character.EnvironmentOxygenLevelSync.Value = characterOb.EnvironmentOxygenLevel;
        this.Character.OxygenLevelAtCharacterLocation.Value = 0.0f;
      }
      string str1;
      this.Character.Definition.AnimationNameToSubtypeName.TryGetValue("HelmetOpen", out str1);
      string str2;
      this.Character.Definition.AnimationNameToSubtypeName.TryGetValue("HelmetClose", out str2);
      this.NeedsOxygenFromSuit = str1 != null && str2 != null || this.Character.UseNewAnimationSystem && this.Character.AnimationController.Controller.GetLayerByName("Helmet") != null ? characterOb.NeedsOxygenFromSuit : this.Definition.NeedsOxygen;
      this.NeedsUpdateBeforeSimulation100 = true;
      if (this.m_soundEmitter == null)
        this.m_soundEmitter = new MyEntity3DSoundEmitter((MyEntity) this.Character);
      if (this.HelmetEnabled)
        return;
      this.AnimateHelmet();
    }

    public virtual void GetObjectBuilder(MyObjectBuilder_Character objectBuilder)
    {
      objectBuilder.OxygenLevel = this.SuitOxygenLevel;
      objectBuilder.EnvironmentOxygenLevel = this.Character.EnvironmentOxygenLevel;
      objectBuilder.NeedsOxygenFromSuit = this.NeedsOxygenFromSuit;
      if (this.m_storedGases == null || this.m_storedGases.Length == 0)
        return;
      if (objectBuilder.StoredGases == null)
        objectBuilder.StoredGases = new List<MyObjectBuilder_Character.StoredGas>();
      foreach (MyCharacterOxygenComponent.GasData storedGase in this.m_storedGases)
      {
        MyCharacterOxygenComponent.GasData storedGas = storedGase;
        if (objectBuilder.StoredGases.TrueForAll((Predicate<MyObjectBuilder_Character.StoredGas>) (obGas => (MyDefinitionId) obGas.Id != storedGas.Id)))
          objectBuilder.StoredGases.Add(new MyObjectBuilder_Character.StoredGas()
          {
            Id = (SerializableDefinitionId) storedGas.Id,
            FillLevel = storedGas.FillLevel
          });
      }
    }

    public override void UpdateBeforeSimulation100()
    {
      base.UpdateBeforeSimulation100();
      this.UpdateOxygen();
    }

    private void UpdateOxygen()
    {
      List<MyEntity> result = new List<MyEntity>();
      BoundingBoxD worldAabb = this.Character.PositionComp.WorldAABB;
      bool lowOxygenDamage = MySession.Static.Settings.EnableOxygen;
      bool noOxygenDamage = MySession.Static.Settings.EnableOxygen;
      bool isInEnvironment = true;
      bool flag1 = false;
      if (Sync.IsServer)
      {
        this.Character.EnvironmentOxygenLevelSync.Value = MyOxygenProviderSystem.GetOxygenInPoint(this.Character.PositionComp.GetPosition());
        this.Character.OxygenLevelAtCharacterLocation.Value = this.Character.EnvironmentOxygenLevel;
        MyOxygenRoom room = (MyOxygenRoom) null;
        if (MySession.Static.Settings.EnableOxygen)
        {
          MyCharacterOxygenComponent.GasData data;
          if (this.TryGetGasData(MyCharacterOxygenComponent.OxygenId, out data))
          {
            float num = (float) (MySession.Static.GameplayFrameCounter - data.LastOutputTime) * 0.01666667f;
            flag1 = (double) this.CharacterGasSink.CurrentInputByType(MyCharacterOxygenComponent.OxygenId) * (double) num > (double) this.Definition.OxygenConsumption;
            if (flag1)
            {
              noOxygenDamage = false;
              lowOxygenDamage = false;
            }
          }
          MyCockpit parent = this.Character.Parent as MyCockpit;
          bool flag2 = false;
          if (parent != null && parent.BlockDefinition.IsPressurized)
          {
            if (MySession.Static.SurvivalMode && !flag1)
            {
              if ((double) parent.OxygenAmount >= (double) this.Definition.OxygenConsumption * (double) this.Definition.OxygenConsumptionMultiplier)
              {
                parent.OxygenAmount -= this.Definition.OxygenConsumption * this.Definition.OxygenConsumptionMultiplier;
                noOxygenDamage = false;
                lowOxygenDamage = false;
              }
              else
                parent.OxygenAmount = 0.0f;
            }
            this.Character.EnvironmentOxygenLevelSync.Value = parent.OxygenFillLevel;
            isInEnvironment = false;
            flag2 = true;
          }
          if (!flag2 || MyFakes.ENABLE_NEW_SOUNDS && MySession.Static.Settings.RealisticSound)
          {
            this.Character.OxygenSourceGridEntityId.Value = 0L;
            Vector3D center = this.Character.PositionComp.WorldAABB.Center;
            MyGamePruningStructure.GetTopMostEntitiesInBox(ref worldAabb, result);
            foreach (MyEntity myEntity in result)
            {
              if (myEntity is MyCubeGrid myCubeGrid && myCubeGrid.GridSystems.GasSystem != null)
              {
                MyOxygenBlock myOxygenBlock = parent != null ? myCubeGrid.GridSystems.GasSystem.GetSafeOxygenBlock(parent.PositionComp.GetPosition()) : myCubeGrid.GridSystems.GasSystem.GetSafeOxygenBlock(center);
                if (myOxygenBlock != null && myOxygenBlock.Room != null)
                {
                  room = myOxygenBlock.Room;
                  if ((double) room.OxygenLevel(myCubeGrid.GridSize) > (double) this.Definition.PressureLevelForLowDamage && !this.HelmetEnabled)
                    lowOxygenDamage = false;
                  if (room.IsAirtight)
                  {
                    float num = room.OxygenLevel(myCubeGrid.GridSize);
                    if (!flag2)
                      this.Character.EnvironmentOxygenLevelSync.Value = num;
                    this.Character.OxygenLevelAtCharacterLocation.Value = num;
                    this.Character.OxygenSourceGridEntityId.Value = myCubeGrid.EntityId;
                    if ((double) room.OxygenAmount > (double) this.Definition.OxygenConsumption * (double) this.Definition.OxygenConsumptionMultiplier)
                    {
                      if (!this.HelmetEnabled)
                      {
                        noOxygenDamage = false;
                        myOxygenBlock.PreviousOxygenAmount = myOxygenBlock.OxygenAmount() - this.Definition.OxygenConsumption * this.Definition.OxygenConsumptionMultiplier;
                        myOxygenBlock.OxygenChangeTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
                        if (!flag1)
                        {
                          room.OxygenAmount -= this.Definition.OxygenConsumption * this.Definition.OxygenConsumptionMultiplier;
                          break;
                        }
                        break;
                      }
                      break;
                    }
                  }
                  else
                  {
                    float environmentOxygen = room.EnvironmentOxygen;
                    this.Character.OxygenLevelAtCharacterLocation.Value = environmentOxygen;
                    if (!flag2)
                    {
                      this.Character.EnvironmentOxygenLevelSync.Value = environmentOxygen;
                      if (!this.HelmetEnabled && (double) this.Character.EnvironmentOxygenLevelSync.Value > (double) this.Definition.OxygenConsumption * (double) this.Definition.OxygenConsumptionMultiplier)
                      {
                        noOxygenDamage = false;
                        break;
                      }
                    }
                  }
                  isInEnvironment = false;
                }
              }
            }
          }
          this.m_oldSuitOxygenLevel = this.SuitOxygenLevel;
        }
        this.UpdateGassesFillLevelsAndAmounts(room);
      }
      this.CharacterGasSink.Update();
      if (!Sync.IsServer || MySession.Static.CreativeMode)
        return;
      this.RefillSuitGassesFromBottles();
      if (MySession.Static.Settings.EnableOxygen)
        this.UpdateSuitOxygen(lowOxygenDamage, noOxygenDamage, isInEnvironment);
      foreach (MyCharacterOxygenComponent.GasData storedGase in this.m_storedGases)
        this.Character.UpdateStoredGas(storedGase.Id, storedGase.FillLevel);
    }

    private void UpdateSuitOxygen(bool lowOxygenDamage, bool noOxygenDamage, bool isInEnvironment)
    {
      if (noOxygenDamage | lowOxygenDamage)
      {
        if (this.HelmetEnabled && (double) this.SuitOxygenAmount > (double) this.Definition.OxygenConsumption * (double) this.Definition.OxygenConsumptionMultiplier)
        {
          noOxygenDamage = false;
          lowOxygenDamage = false;
        }
        if (isInEnvironment && !this.HelmetEnabled)
        {
          if ((double) this.Character.EnvironmentOxygenLevelSync.Value > (double) this.Definition.PressureLevelForLowDamage)
            lowOxygenDamage = false;
          if ((double) this.Character.EnvironmentOxygenLevelSync.Value > 0.0)
            noOxygenDamage = false;
        }
      }
      this.m_oldSuitOxygenLevel = this.SuitOxygenLevel;
      if (noOxygenDamage)
        this.Character.DoDamage(this.Definition.DamageAmountAtZeroPressure, MyDamageType.LowPressure, true, 0L);
      else if (lowOxygenDamage)
        this.Character.DoDamage(1f, MyDamageType.Asphyxia, true, 0L);
      this.Character.UpdateOxygen(this.SuitOxygenAmount);
    }

    private void RefillSuitGassesFromBottles()
    {
      foreach (MyCharacterOxygenComponent.GasData storedGase in this.m_storedGases)
      {
        if ((double) storedGase.FillLevel < (double) MyCharacterOxygenComponent.GAS_REFILL_RATION)
        {
          if (storedGase.NextGasRefill == -1)
            storedGase.NextGasRefill = MySandboxGame.TotalGamePlayTimeInMilliseconds + 5000;
          if (MySandboxGame.TotalGamePlayTimeInMilliseconds >= storedGase.NextGasRefill)
          {
            storedGase.NextGasRefill = -1;
            MyInventory inventory = MyEntityExtensions.GetInventory(this.Character);
            List<MyPhysicalInventoryItem> items = inventory.GetItems();
            bool flag = false;
            foreach (MyPhysicalInventoryItem physicalInventoryItem in items)
            {
              if (physicalInventoryItem.Content is MyObjectBuilder_GasContainerObject content && (double) content.GasLevel != 0.0)
              {
                MyOxygenContainerDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition((MyObjectBuilder_Base) content) as MyOxygenContainerDefinition;
                if (!(physicalItemDefinition.StoredGasId != storedGase.Id))
                {
                  float val1 = content.GasLevel * physicalItemDefinition.Capacity;
                  float gasInput = Math.Min(val1, (1f - storedGase.FillLevel) * storedGase.MaxCapacity);
                  content.GasLevel = Math.Max((val1 - gasInput) / physicalItemDefinition.Capacity, 0.0f);
                  double gasLevel = (double) content.GasLevel;
                  if ((double) gasInput != 0.0)
                    inventory.RaiseContentsChanged();
                  flag = true;
                  this.TransferSuitGas(ref storedGase.Id, gasInput, 0.0f);
                  if ((double) storedGase.FillLevel == 1.0)
                    break;
                }
              }
            }
            if (flag && MySession.Static.LocalCharacter != this.Character)
              this.Character.SendRefillFromBottle(storedGase.Id);
            MyCharacterJetpackComponent jetpackComp = this.Character.JetpackComp;
            if (jetpackComp != null && jetpackComp.TurnedOn && (jetpackComp.FuelDefinition != null && jetpackComp.FuelDefinition.Id == storedGase.Id) && (double) storedGase.FillLevel <= 0.0 && (this.Character.ControllerInfo.Controller != null && !MySession.Static.CreativeToolsEnabled(this.Character.ControllerInfo.Controller.Player.Id.SteamId) || MySession.Static.LocalCharacter != this.Character && !Sync.IsServer))
            {
              if (Sync.IsServer && MySession.Static.LocalCharacter != this.Character)
                MyMultiplayer.RaiseEvent<MyCharacter>(this.Character, (Func<MyCharacter, Action>) (x => new Action(x.SwitchJetpack)), new EndpointId(this.Character.ControllerInfo.Controller.Player.Id.SteamId));
              jetpackComp.SwitchThrusts();
            }
          }
        }
        else
          storedGase.NextGasRefill = -1;
      }
    }

    private void UpdateGassesFillLevelsAndAmounts(MyOxygenRoom room)
    {
      foreach (MyCharacterOxygenComponent.GasData storedGase in this.m_storedGases)
      {
        float num1 = (float) (MySession.Static.GameplayFrameCounter - storedGase.LastOutputTime) * 0.01666667f;
        float num2 = (float) (MySession.Static.GameplayFrameCounter - storedGase.LastInputTime) * 0.01666667f;
        storedGase.LastOutputTime = MySession.Static.GameplayFrameCounter;
        storedGase.LastInputTime = MySession.Static.GameplayFrameCounter;
        float num3 = this.CharacterGasSource.CurrentOutputByType(storedGase.Id) * num1;
        float num4 = this.CharacterGasSink.CurrentInputByType(storedGase.Id) * num2;
        if (storedGase.Id == MyCharacterOxygenComponent.OxygenId && MySession.Static.Settings.EnableOxygen && ((double) this.Definition.OxygenSuitRefillTime > 0.0 && (double) storedGase.FillLevel < 1.0))
        {
          float num5 = MySession.Static.Settings.EnableOxygenPressurization ? Math.Max(this.Character.EnvironmentOxygenLevel, this.Character.OxygenLevel) : this.Character.EnvironmentOxygenLevel;
          if ((double) num5 >= (double) this.Definition.MinOxygenLevelForSuitRefill)
          {
            float num6 = (float) ((double) storedGase.MaxCapacity / (double) this.Definition.OxygenSuitRefillTime * ((double) num2 * 1000.0));
            float num7 = storedGase.MaxCapacity - storedGase.FillLevel * storedGase.MaxCapacity;
            float num8 = MathHelper.Min(num5 * num6, num7);
            num4 += num8;
            if (MySession.Static.Settings.EnableOxygenPressurization && room != null && room.IsAirtight)
            {
              if ((double) room.OxygenAmount >= (double) num4)
              {
                room.OxygenAmount -= num4;
              }
              else
              {
                num4 = room.OxygenAmount;
                room.OxygenAmount = 0.0f;
              }
            }
          }
        }
        float num9 = -MathHelper.Clamp(storedGase.NextGasTransfer, float.NegativeInfinity, 0.0f);
        float num10 = MathHelper.Clamp(storedGase.NextGasTransfer, 0.0f, float.PositiveInfinity);
        storedGase.NextGasTransfer = 0.0f;
        this.TransferSuitGas(ref storedGase.Id, num4 + num10, num3 + num9);
      }
    }

    public void SwitchHelmet()
    {
      if (MySession.Static == null || this.Character == null || (this.Character.IsDead || this.Character.AnimationController == null) || this.Character.AtmosphereDetectorComp == null)
        return;
      string str1;
      this.Character.Definition.AnimationNameToSubtypeName.TryGetValue("HelmetOpen", out str1);
      string str2;
      this.Character.Definition.AnimationNameToSubtypeName.TryGetValue("HelmetClose", out str2);
      if (str1 != null && str2 != null || this.Character.UseNewAnimationSystem && this.Character.AnimationController.Controller.GetLayerByName("Helmet") != null)
      {
        this.NeedsOxygenFromSuit = !this.NeedsOxygenFromSuit;
        this.AnimateHelmet();
      }
      this.Character.SinkComp.Update();
      if (this.m_soundEmitter != null)
      {
        bool force2D = false;
        if (this.NeedsOxygenFromSuit)
          this.m_soundEmitter.PlaySound(this.m_helmetOpenSound, true, force2D: force2D, force3D: new bool?(!MyFakes.FORCE_CHARACTER_2D_SOUND));
        else
          this.m_soundEmitter.PlaySound(this.m_helmetCloseSound, true, force2D: force2D, force3D: new bool?(!MyFakes.FORCE_CHARACTER_2D_SOUND));
        if (!MySession.Static.CreativeMode && this.NeedsOxygenFromSuit && (this.Character.AtmosphereDetectorComp != null && !this.Character.AtmosphereDetectorComp.InAtmosphere) && (!this.Character.AtmosphereDetectorComp.InShipOrStation && (double) this.SuitOxygenAmount >= 0.5))
          this.m_soundEmitter.PlaySound(this.m_helmetAirEscapeSound, force2D: force2D);
      }
      if (!MyFakes.ENABLE_NEW_SOUNDS || !MyFakes.ENABLE_NEW_SOUNDS_QUICK_UPDATE || !MySession.Static.Settings.RealisticSound)
        return;
      MyEntity3DSoundEmitter.UpdateEntityEmitters(true, true, false);
    }

    private void AnimateHelmet()
    {
      string animationName1;
      this.Character.Definition.AnimationNameToSubtypeName.TryGetValue("HelmetOpen", out animationName1);
      string animationName2;
      this.Character.Definition.AnimationNameToSubtypeName.TryGetValue("HelmetClose", out animationName2);
      if (this.Character.Definition == null)
        return;
      if (this.NeedsOxygenFromSuit && animationName1 != null)
      {
        this.Character.PlayCharacterAnimation(animationName1, MyBlendOption.Immediate, MyFrameOption.StayOnLastFrame, 0.2f, sync: true);
      }
      else
      {
        if (this.NeedsOxygenFromSuit || animationName2 == null)
          return;
        this.Character.PlayCharacterAnimation(animationName2, MyBlendOption.Immediate, MyFrameOption.StayOnLastFrame, 0.2f, sync: true);
      }
    }

    public bool ContainsGasStorage(MyDefinitionId gasId) => this.m_gasIdToIndex.ContainsKey(gasId);

    private bool TryGetGasData(MyDefinitionId gasId, out MyCharacterOxygenComponent.GasData data)
    {
      int typeIndex = -1;
      data = (MyCharacterOxygenComponent.GasData) null;
      if (!this.TryGetTypeIndex(ref gasId, out typeIndex))
        return false;
      data = this.m_storedGases[typeIndex];
      return true;
    }

    public float GetGasFillLevel(MyDefinitionId gasId)
    {
      int typeIndex = -1;
      return !this.TryGetTypeIndex(ref gasId, out typeIndex) ? 0.0f : this.m_storedGases[typeIndex].FillLevel;
    }

    public void UpdateStoredGasLevel(ref MyDefinitionId gasId, float fillLevel)
    {
      int typeIndex = -1;
      if (!this.TryGetTypeIndex(ref gasId, out typeIndex))
        return;
      this.m_storedGases[typeIndex].FillLevel = fillLevel;
      this.CharacterGasSource.SetRemainingCapacityByType(gasId, fillLevel * this.m_storedGases[typeIndex].MaxCapacity);
      this.CharacterGasSource.SetProductionEnabledByType(gasId, (double) fillLevel > 0.0);
    }

    internal void TransferSuitGas(ref MyDefinitionId gasId, float gasInput, float gasOutput)
    {
      int typeIndex = this.GetTypeIndex(ref gasId);
      if ((double) gasInput > 0.0)
        ;
      float val1 = gasInput - gasOutput;
      if (MySession.Static.CreativeMode)
        val1 = Math.Max(val1, 0.0f);
      if ((double) val1 == 0.0)
        return;
      MyCharacterOxygenComponent.GasData storedGase = this.m_storedGases[typeIndex];
      storedGase.FillLevel = MathHelper.Clamp(storedGase.FillLevel + val1 / storedGase.MaxCapacity, 0.0f, 1f);
      this.CharacterGasSource.SetRemainingCapacityByType(storedGase.Id, storedGase.FillLevel * storedGase.MaxCapacity);
      this.CharacterGasSource.SetProductionEnabledByType(storedGase.Id, (double) storedGase.FillLevel > 0.0);
    }

    private void Source_CurrentOutputChanged(
      MyDefinitionId changedResourceId,
      float oldOutput,
      MyResourceSourceComponent source)
    {
      int typeIndex;
      if (!this.TryGetTypeIndex(ref changedResourceId, out typeIndex))
        return;
      float num1 = (float) (MySession.Static.GameplayFrameCounter - this.m_storedGases[typeIndex].LastOutputTime) * 0.01666667f;
      this.m_storedGases[typeIndex].LastOutputTime = MySession.Static.GameplayFrameCounter;
      float num2 = oldOutput * num1;
      this.m_storedGases[typeIndex].NextGasTransfer -= num2;
    }

    private void Sink_CurrentInputChanged(
      MyDefinitionId resourceTypeId,
      float oldInput,
      MyResourceSinkComponent sink)
    {
      int typeIndex;
      if (!this.TryGetTypeIndex(ref resourceTypeId, out typeIndex))
        return;
      float num1 = (float) (MySession.Static.GameplayFrameCounter - this.m_storedGases[typeIndex].LastInputTime) * 0.01666667f;
      float num2 = oldInput * num1;
      this.m_storedGases[typeIndex].NextGasTransfer += num2;
    }

    private void SetGasSink(MyResourceSinkComponent characterSinkComponent)
    {
      foreach (MyCharacterOxygenComponent.GasData storedGase in this.m_storedGases)
      {
        storedGase.LastInputTime = MySession.Static.GameplayFrameCounter;
        if (Sync.IsServer)
        {
          if (this.m_characterGasSink != null)
            this.m_characterGasSink.CurrentInputChanged -= new MyCurrentResourceInputChangedDelegate(this.Sink_CurrentInputChanged);
          if (characterSinkComponent != null)
            characterSinkComponent.CurrentInputChanged += new MyCurrentResourceInputChangedDelegate(this.Sink_CurrentInputChanged);
        }
      }
      this.m_characterGasSink = characterSinkComponent;
    }

    private void SetGasSource(MyResourceSourceComponent characterSourceComponent)
    {
      foreach (MyCharacterOxygenComponent.GasData storedGase in this.m_storedGases)
      {
        storedGase.LastOutputTime = MySession.Static.GameplayFrameCounter;
        if (this.m_characterGasSource != null)
        {
          this.m_characterGasSource.SetRemainingCapacityByType(storedGase.Id, 0.0f);
          if (Sync.IsServer)
            this.m_characterGasSource.OutputChanged -= new MyResourceOutputChangedDelegate(this.Source_CurrentOutputChanged);
        }
        if (characterSourceComponent != null)
        {
          characterSourceComponent.SetRemainingCapacityByType(storedGase.Id, storedGase.FillLevel * storedGase.MaxCapacity);
          characterSourceComponent.SetProductionEnabledByType(storedGase.Id, (double) storedGase.FillLevel > 0.0);
          if (Sync.IsServer)
            characterSourceComponent.OutputChanged += new MyResourceOutputChangedDelegate(this.Source_CurrentOutputChanged);
        }
      }
      this.m_characterGasSource = characterSourceComponent;
    }

    public void AppendSinkData(List<MyResourceSinkInfo> sinkData)
    {
      for (int index = 0; index < this.m_storedGases.Length; ++index)
      {
        int captureIndex = index;
        sinkData.Add(new MyResourceSinkInfo()
        {
          ResourceTypeId = this.m_storedGases[index].Id,
          MaxRequiredInput = this.m_storedGases[index].Throughput,
          RequiredInputFunc = (Func<float>) (() => this.Sink_ComputeRequiredGas(this.m_storedGases[captureIndex]))
        });
      }
    }

    public void AppendSourceData(List<MyResourceSourceInfo> sourceData)
    {
      for (int index = 0; index < this.m_storedGases.Length; ++index)
        sourceData.Add(new MyResourceSourceInfo()
        {
          ResourceTypeId = this.m_storedGases[index].Id,
          DefinedOutput = this.m_storedGases[index].Throughput,
          ProductionToCapacityMultiplier = 1f,
          IsInfiniteCapacity = false
        });
    }

    private float Sink_ComputeRequiredGas(MyCharacterOxygenComponent.GasData gas) => Math.Min((float) (((1.0 - (double) gas.FillLevel) * (double) gas.MaxCapacity + (gas.Id == MyCharacterOxygenComponent.OxygenId ? (double) this.Definition.OxygenConsumption * (double) this.Definition.OxygenConsumptionMultiplier : 0.0)) / 60.0 * 100.0), gas.Throughput);

    private int GetTypeIndex(ref MyDefinitionId gasId)
    {
      int num = 0;
      if (this.m_gasIdToIndex.Count > 1)
        num = this.m_gasIdToIndex[gasId];
      return num;
    }

    private bool TryGetTypeIndex(ref MyDefinitionId gasId, out int typeIndex) => this.m_gasIdToIndex.TryGetValue(gasId, out typeIndex);

    private class GasData
    {
      public MyDefinitionId Id;
      public float FillLevel;
      public float MaxCapacity;
      public float Throughput;
      public float NextGasTransfer;
      public int LastOutputTime;
      public int LastInputTime;
      public int NextGasRefill = -1;

      public override string ToString() => string.Format("Subtype: {0}, FillLevel: {1}, CurrentCapacity: {2}, MaxCapacity: {3}", (object) this.Id.SubtypeName, (object) this.FillLevel, (object) (float) ((double) this.FillLevel * (double) this.MaxCapacity), (object) this.MaxCapacity);
    }

    private class Sandbox_Game_Entities_Character_Components_MyCharacterOxygenComponent\u003C\u003EActor : IActivator, IActivator<MyCharacterOxygenComponent>
    {
      object IActivator.CreateInstance() => (object) new MyCharacterOxygenComponent();

      MyCharacterOxygenComponent IActivator<MyCharacterOxygenComponent>.CreateInstance() => new MyCharacterOxygenComponent();
    }
  }
}
