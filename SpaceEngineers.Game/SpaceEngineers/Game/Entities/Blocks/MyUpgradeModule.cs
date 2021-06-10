// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.Blocks.MyUpgradeModule
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Graphics;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ModAPI;
using VRageMath;
using VRageRender.Import;

namespace SpaceEngineers.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_UpgradeModule))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyUpgradeModule), typeof (Sandbox.ModAPI.Ingame.IMyUpgradeModule)})]
  public class MyUpgradeModule : MyFunctionalBlock, Sandbox.ModAPI.IMyUpgradeModule, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyUpgradeModule
  {
    private ConveyorLinePosition[] m_connectionPositions;
    private Dictionary<ConveyorLinePosition, MyCubeBlock> m_connectedBlocks;
    private MyUpgradeModuleInfo[] m_upgrades;
    private int m_connectedBlockCount;
    private SortedDictionary<string, MyModelDummy> m_dummies;
    private bool m_needsRefresh;
    private MyResourceStateEnum m_oldResourceState = MyResourceStateEnum.NoPower;

    private MyUpgradeModuleDefinition BlockDefinition => (MyUpgradeModuleDefinition) base.BlockDefinition;

    public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
    {
      base.Init(builder, cubeGrid);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
      this.m_connectedBlocks = new Dictionary<ConveyorLinePosition, MyCubeBlock>();
      this.m_dummies = new SortedDictionary<string, MyModelDummy>((IDictionary<string, MyModelDummy>) MyModels.GetModelOnlyDummies(this.BlockDefinition.Model).Dummies);
      this.InitDummies();
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.IsWorkingChanged += new Action<MyCubeBlock>(this.MyUpgradeModule_IsWorkingChanged);
      this.m_upgrades = this.BlockDefinition.Upgrades;
      this.UpdateIsWorking();
    }

    private void MyUpgradeModule_IsWorkingChanged(MyCubeBlock obj)
    {
      this.RefreshEffects();
      this.UpdateEmissivity();
    }

    private void ComponentStack_IsFunctionalChanged()
    {
      this.UpdateIsWorking();
      this.UpdateEmissivity();
    }

    private void CubeGrid_OnBlockRemoved(MySlimBlock obj)
    {
      if (obj == this.SlimBlock)
        return;
      this.m_needsRefresh = true;
    }

    private void CubeGrid_OnBlockAdded(MySlimBlock obj)
    {
      if (obj == this.SlimBlock)
        return;
      this.m_needsRefresh = true;
    }

    public override void OnRegisteredToGridSystems()
    {
      base.OnRegisteredToGridSystems();
      this.InitDummies();
      this.m_needsRefresh = true;
      this.UpdateEmissivity();
      this.CubeGrid.OnBlockAdded += new Action<MySlimBlock>(this.CubeGrid_OnBlockAdded);
      this.CubeGrid.OnBlockRemoved += new Action<MySlimBlock>(this.CubeGrid_OnBlockRemoved);
    }

    public override void UpdateBeforeSimulation100()
    {
      base.UpdateBeforeSimulation100();
      if (this.m_needsRefresh)
      {
        this.RefreshConnections();
        this.m_needsRefresh = false;
      }
      if (Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      if (this.CubeGrid.GridSystems.ResourceDistributor.ResourceState != this.m_oldResourceState)
      {
        this.m_oldResourceState = this.CubeGrid.GridSystems.ResourceDistributor.ResourceState;
        this.UpdateEmissivity();
      }
      this.m_oldResourceState = this.CubeGrid.GridSystems.ResourceDistributor.ResourceState;
      if (this.m_soundEmitter == null)
        return;
      bool flag1 = false;
      foreach (MyCubeBlock myCubeBlock in this.m_connectedBlocks.Values)
      {
        flag1 = ((flag1 ? 1 : 0) | (myCubeBlock == null || myCubeBlock.ResourceSink == null || !myCubeBlock.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) ? 0 : (myCubeBlock.IsWorking ? 1 : 0))) != 0;
        if (flag1)
          break;
      }
      bool flag2 = flag1 & this.IsWorking;
      if (flag2 && this.m_connectedBlockCount > 0 && (!this.m_soundEmitter.IsPlaying || this.m_soundEmitter.SoundPair != this.m_baseIdleSound))
      {
        this.m_soundEmitter.PlaySound(this.m_baseIdleSound, true);
      }
      else
      {
        if (flag2 && this.m_connectedBlockCount != 0 || (!this.m_soundEmitter.IsPlaying || this.m_soundEmitter.SoundPair != this.m_baseIdleSound))
          return;
        this.m_soundEmitter.StopSound(false);
      }
    }

    private void InitDummies()
    {
      this.m_connectedBlocks.Clear();
      this.m_connectionPositions = MyMultilineConveyorEndpoint.GetLinePositions((MyCubeBlock) this, (IDictionary<string, MyModelDummy>) this.m_dummies, "detector_upgrade");
      for (int index = 0; index < this.m_connectionPositions.Length; ++index)
      {
        this.m_connectionPositions[index] = MyMultilineConveyorEndpoint.PositionToGridCoords(this.m_connectionPositions[index], (MyCubeBlock) this);
        this.m_connectedBlocks.Add(this.m_connectionPositions[index], (MyCubeBlock) null);
      }
    }

    public override void OnModelChange()
    {
      base.OnModelChange();
      this.UpdateEmissivity();
    }

    private void RefreshConnections()
    {
      foreach (ConveyorLinePosition connectionPosition in this.m_connectionPositions)
      {
        ConveyorLinePosition connectingPosition = connectionPosition.GetConnectingPosition();
        MySlimBlock cubeBlock = this.CubeGrid.GetCubeBlock(connectingPosition.LocalGridPosition);
        if (cubeBlock != null && cubeBlock.FatBlock != null)
        {
          MyCubeBlock block1 = cubeBlock.FatBlock;
          MyCubeBlock block2 = (MyCubeBlock) null;
          this.m_connectedBlocks.TryGetValue(connectionPosition, out block2);
          if (block1 != null && !block1.GetComponent().ConnectionPositions.Contains(connectingPosition))
            block1 = (MyCubeBlock) null;
          if (block1 != block2)
          {
            if (block2 != null && block2.CurrentAttachedUpgradeModules != null)
              block2.CurrentAttachedUpgradeModules.Remove(this.EntityId);
            if (block1 != null)
            {
              if (block1.CurrentAttachedUpgradeModules == null)
                block1.CurrentAttachedUpgradeModules = new Dictionary<long, MyCubeBlock.AttachedUpgradeModule>();
              if (block1.CurrentAttachedUpgradeModules.ContainsKey(this.EntityId))
                ++block1.CurrentAttachedUpgradeModules[this.EntityId].SlotCount;
              else
                block1.CurrentAttachedUpgradeModules.Add(this.EntityId, new MyCubeBlock.AttachedUpgradeModule((Sandbox.ModAPI.IMyUpgradeModule) this, 1, this.CanAffectBlock(block1)));
            }
            if (this.IsWorking)
            {
              if (block2 != null)
                this.RemoveEffectFromBlock(block2);
              if (block1 != null)
                this.AddEffectToBlock(block1);
            }
            this.m_connectedBlocks[connectionPosition] = block1;
          }
        }
        else
        {
          MyCubeBlock block = (MyCubeBlock) null;
          this.m_connectedBlocks.TryGetValue(connectionPosition, out block);
          if (block != null)
          {
            if (block != null && block.CurrentAttachedUpgradeModules != null)
              block.CurrentAttachedUpgradeModules.Remove(this.EntityId);
            if (this.IsWorking)
              this.RemoveEffectFromBlock(block);
            this.m_connectedBlocks[connectionPosition] = (MyCubeBlock) null;
          }
        }
      }
      this.UpdateEmissivity();
    }

    private void RefreshEffects()
    {
      foreach (MyCubeBlock block in this.m_connectedBlocks.Values)
      {
        if (block != null)
        {
          if (this.IsWorking)
            this.AddEffectToBlock(block);
          else
            this.RemoveEffectFromBlock(block);
        }
      }
    }

    private bool CanAffectBlock(MyCubeBlock block)
    {
      foreach (MyUpgradeModuleInfo upgrade in this.m_upgrades)
      {
        if (block.UpgradeValues.ContainsKey(upgrade.UpgradeType))
          return true;
      }
      return false;
    }

    private void RemoveEffectFromBlock(MyCubeBlock block)
    {
      foreach (MyUpgradeModuleInfo upgrade in this.m_upgrades)
      {
        float num1;
        if (block.UpgradeValues.TryGetValue(upgrade.UpgradeType, out num1))
        {
          double num2 = (double) num1;
          double num3;
          if (upgrade.ModifierType == MyUpgradeModifierType.Additive)
          {
            num3 = num2 - (double) upgrade.Modifier;
            if (num3 < 0.0)
              num3 = 0.0;
          }
          else
          {
            num3 = num2 / (double) upgrade.Modifier;
            if (num3 < 1.0)
            {
              double num4 = num3 + 1E-07;
              num3 = 1.0;
            }
          }
          block.UpgradeValues[upgrade.UpgradeType] = (float) num3;
        }
      }
      block.CommitUpgradeValues();
    }

    private void AddEffectToBlock(MyCubeBlock block)
    {
      foreach (MyUpgradeModuleInfo upgrade in this.m_upgrades)
      {
        float num1;
        if (block.UpgradeValues.TryGetValue(upgrade.UpgradeType, out num1))
        {
          double num2 = (double) num1;
          double num3 = upgrade.ModifierType != MyUpgradeModifierType.Additive ? num2 * (double) upgrade.Modifier : num2 + (double) upgrade.Modifier;
          block.UpgradeValues[upgrade.UpgradeType] = (float) num3;
        }
      }
      block.CommitUpgradeValues();
    }

    public override bool SetEmissiveStateWorking() => false;

    public override bool SetEmissiveStateDamaged() => false;

    public override bool SetEmissiveStateDisabled() => false;

    private void UpdateEmissivity()
    {
      this.m_connectedBlockCount = 0;
      if (this.m_connectedBlocks == null)
        return;
      for (int index = 0; index < this.m_connectionPositions.Length; ++index)
      {
        string emissiveName = "Emissive" + index.ToString();
        Color emissivePartColor = Color.Green;
        float emissivity = 1f;
        MyCubeBlock myCubeBlock = (MyCubeBlock) null;
        this.m_connectedBlocks.TryGetValue(this.m_connectionPositions[index], out myCubeBlock);
        if (myCubeBlock != null)
          ++this.m_connectedBlockCount;
        MyEmissiveColorStateResult result;
        if (this.IsWorking && this.m_oldResourceState != MyResourceStateEnum.NoPower)
        {
          if (myCubeBlock != null)
          {
            if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Working, out result))
              emissivePartColor = result.EmissiveColor;
          }
          else
          {
            emissivePartColor = Color.Yellow;
            if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Warning, out result))
              emissivePartColor = result.EmissiveColor;
          }
        }
        else if (this.IsFunctional)
        {
          emissivePartColor = Color.Red;
          if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Disabled, out result))
            emissivePartColor = result.EmissiveColor;
        }
        else
        {
          emissivePartColor = Color.Black;
          emissivity = 0.0f;
          if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Damaged, out result))
            emissivePartColor = result.EmissiveColor;
        }
        if (this.Render.RenderObjectIDs[0] != uint.MaxValue)
          MyEntity.UpdateNamedEmissiveParts(this.Render.RenderObjectIDs[0], emissiveName, emissivePartColor, emissivity);
      }
    }

    public override void OnUnregisteredFromGridSystems()
    {
      base.OnUnregisteredFromGridSystems();
      this.CubeGrid.OnBlockAdded -= new Action<MySlimBlock>(this.CubeGrid_OnBlockAdded);
      this.CubeGrid.OnBlockRemoved -= new Action<MySlimBlock>(this.CubeGrid_OnBlockRemoved);
      this.SlimBlock.ComponentStack.IsFunctionalChanged -= new Action(this.ComponentStack_IsFunctionalChanged);
      this.ClearConnectedBlocks();
    }

    private void ClearConnectedBlocks()
    {
      foreach (MyCubeBlock block in this.m_connectedBlocks.Values)
      {
        if (block != null && this.IsWorking)
          this.RemoveEffectFromBlock(block);
        if (block != null && block.CurrentAttachedUpgradeModules != null)
          block.CurrentAttachedUpgradeModules.Remove(this.EntityId);
      }
      this.m_connectedBlocks.Clear();
    }

    protected int GetBlockConnectionCount(MyCubeBlock cubeBlock)
    {
      int num = 0;
      foreach (MyCubeBlock myCubeBlock in this.m_connectedBlocks.Values)
      {
        if (myCubeBlock == cubeBlock)
          ++num;
      }
      return num;
    }

    void Sandbox.ModAPI.Ingame.IMyUpgradeModule.GetUpgradeList(
      out List<MyUpgradeModuleInfo> upgradelist)
    {
      upgradelist = new List<MyUpgradeModuleInfo>();
      foreach (MyUpgradeModuleInfo upgrade in this.m_upgrades)
        upgradelist.Add(upgrade);
    }

    uint Sandbox.ModAPI.Ingame.IMyUpgradeModule.UpgradeCount => (uint) this.m_upgrades.Length;

    uint Sandbox.ModAPI.Ingame.IMyUpgradeModule.Connections
    {
      get
      {
        uint num = 0;
        MyCubeBlock myCubeBlock1 = (MyCubeBlock) null;
        foreach (MyCubeBlock myCubeBlock2 in this.m_connectedBlocks.Values)
        {
          if (myCubeBlock1 != myCubeBlock2 && myCubeBlock2 != null)
          {
            ++num;
            myCubeBlock1 = myCubeBlock2;
          }
        }
        return num;
      }
    }
  }
}
