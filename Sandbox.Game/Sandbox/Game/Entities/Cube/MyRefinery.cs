// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyRefinery
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.Entities.Cube
{
  [MyCubeBlockType(typeof (MyObjectBuilder_Refinery))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyRefinery), typeof (Sandbox.ModAPI.Ingame.IMyRefinery)})]
  public class MyRefinery : MyProductionBlock, Sandbox.ModAPI.IMyRefinery, Sandbox.ModAPI.IMyProductionBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyProductionBlock, Sandbox.ModAPI.Ingame.IMyRefinery
  {
    private MyEntity m_currentUser;
    private MyRefineryDefinition m_refineryDef;
    private bool m_queueNeedsRebuild;
    private bool m_processingLock;
    private float m_actualCheckFillValue;
    private readonly List<KeyValuePair<int, MyBlueprintDefinitionBase>> m_tmpSortedBlueprints = new List<KeyValuePair<int, MyBlueprintDefinitionBase>>();

    public override bool IsTieredUpdateSupported => true;

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.UpgradeValues.Add("Productivity", 0.0f);
      this.UpgradeValues.Add("Effectiveness", 1f);
      this.UpgradeValues.Add("PowerEfficiency", 1f);
      base.Init(objectBuilder, cubeGrid);
      this.m_refineryDef = this.BlockDefinition as MyRefineryDefinition;
      if (this.InventoryAggregate.InventoryCount > 2)
        this.FixInputOutputInventories(this.m_refineryDef.InputInventoryConstraint, this.m_refineryDef.OutputInventoryConstraint);
      this.InputInventory.Constraint = this.m_refineryDef.InputInventoryConstraint;
      this.InputInventory.FilterItemsUsingConstraint();
      this.ResetActualCheckFillValue(this.InputInventory);
      this.OutputInventory.Constraint = this.m_refineryDef.OutputInventoryConstraint;
      this.OutputInventory.FilterItemsUsingConstraint();
      this.m_queueNeedsRebuild = true;
      this.m_baseIdleSound = this.BlockDefinition.PrimarySound;
      this.m_processSound = this.BlockDefinition.ActionSound;
      this.ResourceSink.RequiredInputChanged += new MyRequiredResourceChangeDelegate(this.PowerReceiver_RequiredInputChanged);
      this.OnUpgradeValuesChanged += (Action) (() =>
      {
        this.SetDetailedInfoDirty();
        this.RaisePropertiesChanged();
      });
      this.SetDetailedInfoDirty();
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
    }

    private void ResetActualCheckFillValue(MyInventory inventory)
    {
      if ((double) this.m_refineryDef.InventoryFillFactorMin > (double) inventory.VolumeFillFactor)
        this.m_actualCheckFillValue = this.m_refineryDef.InventoryFillFactorMax;
      else
        this.m_actualCheckFillValue = this.m_refineryDef.InventoryFillFactorMin;
    }

    protected override void OnBeforeInventoryRemovedFromAggregate(
      MyInventoryAggregate aggregate,
      MyInventoryBase inventory)
    {
      if (inventory == this.InputInventory)
        this.InputInventory.ContentsChanged += new Action<MyInventoryBase>(this.inventory_OnContentsChanged);
      else if (inventory == this.OutputInventory)
        this.OutputInventory.ContentsChanged += new Action<MyInventoryBase>(this.inventory_OnContentsChanged);
      base.OnBeforeInventoryRemovedFromAggregate(aggregate, inventory);
    }

    protected override void OnInventoryAddedToAggregate(
      MyInventoryAggregate aggregate,
      MyInventoryBase inventory)
    {
      base.OnInventoryAddedToAggregate(aggregate, inventory);
      if (inventory == this.InputInventory)
      {
        this.InputInventory.ContentsChanged += new Action<MyInventoryBase>(this.inventory_OnContentsChanged);
      }
      else
      {
        if (inventory != this.OutputInventory)
          return;
        this.OutputInventory.ContentsChanged += new Action<MyInventoryBase>(this.inventory_OnContentsChanged);
      }
    }

    public override void DoUpdateTimerTick()
    {
      base.DoUpdateTimerTick();
      this.GetOreFromConveyorSystem();
      this.UpdateProduction(this.GetFramesFromLastTrigger());
      if (!(bool) this.m_useConveyorSystem || (double) this.OutputInventory.VolumeFillFactor <= 0.75)
        return;
      MyGridConveyorSystem.PushAnyRequest((IMyConveyorEndpointBlock) this, this.OutputInventory);
    }

    private void GetOreFromConveyorSystem()
    {
      if (!(bool) this.m_useConveyorSystem || !this.IsWorking || !this.Enabled)
        return;
      MyInventory inputInventory = this.InputInventory;
      if (inputInventory == null)
        return;
      if ((double) this.m_actualCheckFillValue > (double) inputInventory.VolumeFillFactor)
      {
        MyFixedPoint? nullable = this.m_refineryDef.OreAmountPerPullRequest;
        if (!nullable.HasValue)
          nullable = new MyFixedPoint?(MyFixedPoint.MaxValue);
        this.CubeGrid.GridSystems.ConveyorSystem.PullItems(inputInventory.Constraint, nullable.Value, (IMyConveyorEndpointBlock) this, inputInventory);
        if ((double) this.m_actualCheckFillValue != (double) this.m_refineryDef.InventoryFillFactorMin)
          return;
        this.m_actualCheckFillValue = this.m_refineryDef.InventoryFillFactorMax;
      }
      else
      {
        if ((double) this.m_actualCheckFillValue != (double) this.m_refineryDef.InventoryFillFactorMax)
          return;
        this.m_actualCheckFillValue = this.m_refineryDef.InventoryFillFactorMin;
      }
    }

    private void PowerReceiver_RequiredInputChanged(
      MyDefinitionId resourceTypeId,
      MyResourceSinkComponent receiver,
      float oldRequirement,
      float newRequirement)
    {
      this.SetDetailedInfoDirty();
    }

    private void inventory_OnContentsChanged(MyInventoryBase inv)
    {
      if (this.m_processingLock || !Sync.IsServer)
        return;
      this.m_queueNeedsRebuild = true;
    }

    private void RebuildQueue()
    {
      this.m_queueNeedsRebuild = false;
      this.ClearQueue(false);
      this.m_tmpSortedBlueprints.Clear();
      MyPhysicalInventoryItem[] array = this.InputInventory.GetItems().ToArray();
      for (int key = 0; key < array.Length; ++key)
      {
        for (int index1 = 0; index1 < this.m_refineryDef.BlueprintClasses.Count; ++index1)
        {
          foreach (MyBlueprintDefinitionBase blueprintDefinitionBase in this.m_refineryDef.BlueprintClasses[index1])
          {
            bool flag = false;
            MyDefinitionId other = new MyDefinitionId(array[key].Content.TypeId, array[key].Content.SubtypeId);
            for (int index2 = 0; index2 < blueprintDefinitionBase.Prerequisites.Length; ++index2)
            {
              if (blueprintDefinitionBase.Prerequisites[index2].Id.Equals(other))
              {
                flag = true;
                break;
              }
            }
            if (flag)
            {
              this.m_tmpSortedBlueprints.Add(new KeyValuePair<int, MyBlueprintDefinitionBase>(key, blueprintDefinitionBase));
              break;
            }
          }
        }
      }
      for (int index = 0; index < this.m_tmpSortedBlueprints.Count; ++index)
      {
        MyBlueprintDefinitionBase blueprint = this.m_tmpSortedBlueprints[index].Value;
        MyFixedPoint myFixedPoint = MyFixedPoint.MaxValue;
        foreach (MyBlueprintDefinitionBase.Item prerequisite in blueprint.Prerequisites)
        {
          MyFixedPoint amount = array[index].Amount;
          if (amount == (MyFixedPoint) 0)
          {
            myFixedPoint = (MyFixedPoint) 0;
            break;
          }
          myFixedPoint = MyFixedPoint.Min(amount * (1f / (float) prerequisite.Amount), myFixedPoint);
        }
        if (blueprint.Atomic)
          myFixedPoint = MyFixedPoint.Floor(myFixedPoint);
        if (myFixedPoint > (MyFixedPoint) 0 && myFixedPoint != MyFixedPoint.MaxValue)
          this.InsertQueueItemRequest(-1, blueprint, myFixedPoint);
      }
      this.m_tmpSortedBlueprints.Clear();
    }

    protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
    {
      base.UpdateDetailedInfo(detailedInfo);
      detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
      detailedInfo.Append(this.BlockDefinition.DisplayNameText);
      detailedInfo.AppendFormat("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
      MyValueFormatter.AppendWorkInBestUnit(this.GetOperationalPowerConsumption(), detailedInfo);
      detailedInfo.AppendFormat("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_RequiredInput));
      MyValueFormatter.AppendWorkInBestUnit(this.ResourceSink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId), detailedInfo);
      detailedInfo.AppendFormat("\n\n");
      detailedInfo.Append(MyTexts.Get(MySpaceTexts.BlockPropertiesText_Productivity).ToString() + " ");
      detailedInfo.Append(((float) (((double) this.UpgradeValues["Productivity"] + 1.0) * 100.0)).ToString("F0"));
      detailedInfo.Append("%\n");
      detailedInfo.Append(MyTexts.Get(MySpaceTexts.BlockPropertiesText_Effectiveness).ToString() + " ");
      detailedInfo.Append((this.UpgradeValues["Effectiveness"] * 100f).ToString("F0"));
      detailedInfo.Append("%\n");
      detailedInfo.Append(MyTexts.Get(MySpaceTexts.BlockPropertiesText_Efficiency).ToString() + " ");
      detailedInfo.Append((this.UpgradeValues["PowerEfficiency"] * 100f).ToString("F0"));
      detailedInfo.Append("%\n\n");
      this.PrintUpgradeModuleInfo(detailedInfo);
    }

    private void UpdateProduction(uint framesFromLastTrigger)
    {
      int timeDelta = (int) framesFromLastTrigger * 16;
      if (this.m_queueNeedsRebuild && Sync.IsServer)
        this.RebuildQueue();
      bool flag = this.IsWorking && !this.IsQueueEmpty && !this.OutputInventory.IsFull;
      float powerConsumption = this.GetOperationalPowerConsumption();
      if ((!this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) || (double) this.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId) < (double) powerConsumption) && !this.ResourceSink.IsPowerAvailable(MyResourceDistributorComponent.ElectricityId, powerConsumption))
        flag = false;
      this.IsProducing = flag;
      if (!this.IsProducing)
        return;
      this.ProcessQueueItems(timeDelta);
    }

    private void ProcessQueueItems(int timeDelta)
    {
      this.m_processingLock = true;
      if (Sync.IsServer)
      {
        while (!this.IsQueueEmpty && timeDelta > 0)
        {
          MyProductionBlock.QueueItem queueItem = this.TryGetFirstQueueItem().Value;
          MyFixedPoint blueprintAmount = (MyFixedPoint) (float) ((double) timeDelta * ((double) this.m_refineryDef.RefineSpeed + (double) this.UpgradeValues["Productivity"]) * (double) MySession.Static.RefinerySpeedMultiplier / ((double) queueItem.Blueprint.BaseProductionTimeInSeconds * 1000.0));
          foreach (MyBlueprintDefinitionBase.Item prerequisite in queueItem.Blueprint.Prerequisites)
          {
            MyFixedPoint itemAmount = this.InputInventory.GetItemAmount(prerequisite.Id, MyItemFlags.None, false);
            MyFixedPoint myFixedPoint = blueprintAmount * prerequisite.Amount;
            if (itemAmount < myFixedPoint)
              blueprintAmount = itemAmount * (1f / (float) prerequisite.Amount);
          }
          if (blueprintAmount == (MyFixedPoint) 0)
          {
            this.m_queueNeedsRebuild = true;
            break;
          }
          timeDelta -= Math.Max(1, (int) ((double) (float) blueprintAmount * (double) queueItem.Blueprint.BaseProductionTimeInSeconds / (double) this.m_refineryDef.RefineSpeed * 1000.0));
          if (timeDelta < 0)
            timeDelta = 0;
          this.ChangeRequirementsToResults(queueItem.Blueprint, blueprintAmount);
        }
      }
      this.IsProducing = !this.IsQueueEmpty;
      this.m_processingLock = false;
    }

    private void ChangeRequirementsToResults(
      MyBlueprintDefinitionBase queueItem,
      MyFixedPoint blueprintAmount)
    {
      if (this.m_refineryDef == null)
      {
        MyLog.Default.WriteLine("m_refineryDef shouldn't be null!!!" + (object) this);
      }
      else
      {
        if (!Sync.IsServer || MySession.Static == null || (queueItem == null || queueItem.Prerequisites == null) || (this.OutputInventory == null || this.InputInventory == null || (queueItem.Results == null || this.m_refineryDef == null)))
          return;
        if (!MySession.Static.CreativeMode)
          blueprintAmount = MyFixedPoint.Min(this.OutputInventory.ComputeAmountThatFits(queueItem), blueprintAmount);
        if (blueprintAmount == (MyFixedPoint) 0)
          return;
        foreach (MyBlueprintDefinitionBase.Item prerequisite in queueItem.Prerequisites)
        {
          if (!(MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) prerequisite.Id) is MyObjectBuilder_PhysicalObject newObject))
          {
            MyLog.Default.WriteLine("obPrerequisite shouldn't be null!!! " + (object) this);
          }
          else
          {
            this.InputInventory.RemoveItemsOfType((MyFixedPoint) ((float) blueprintAmount * (float) prerequisite.Amount), newObject, false, false);
            MyFixedPoint itemAmount = this.InputInventory.GetItemAmount(prerequisite.Id, MyItemFlags.None, false);
            if (itemAmount < (MyFixedPoint) 0.01f)
              this.InputInventory.RemoveItemsOfType(itemAmount, prerequisite.Id, MyItemFlags.None, false);
          }
        }
        foreach (MyBlueprintDefinitionBase.Item result in queueItem.Results)
        {
          if (!(MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) result.Id) is MyObjectBuilder_PhysicalObject newObject))
          {
            MyLog.Default.WriteLine("obResult shouldn't be null!!! " + (object) this);
          }
          else
          {
            float num = (float) result.Amount * this.m_refineryDef.MaterialEfficiency * this.UpgradeValues["Effectiveness"];
            this.OutputInventory.AddItems((MyFixedPoint) ((float) blueprintAmount * num), (MyObjectBuilder_Base) newObject);
          }
        }
        this.RemoveFirstQueueItemAnnounce(blueprintAmount);
      }
    }

    protected override float GetOperationalPowerConsumption() => (float) ((double) base.GetOperationalPowerConsumption() * (1.0 + (double) this.UpgradeValues["Productivity"]) * (1.0 / (double) this.UpgradeValues["PowerEfficiency"]));

    private class Sandbox_Game_Entities_Cube_MyRefinery\u003C\u003EActor : IActivator, IActivator<MyRefinery>
    {
      object IActivator.CreateInstance() => (object) new MyRefinery();

      MyRefinery IActivator<MyRefinery>.CreateInstance() => new MyRefinery();
    }
  }
}
