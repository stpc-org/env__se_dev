// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyRadialMenuComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.Screens.Helpers
{
  [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
  internal class MyRadialMenuComponent : MySessionComponentBase
  {
    private MyRadialMenuSection m_lastUsedBlocks;
    private MyRadialMenuSection m_lastUsedVoxel;

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      base.Init(sessionComponent);
      this.m_lastUsedBlocks = new MyRadialMenuSection(new List<MyRadialMenuItem>(), MyStringId.GetOrCompute("RadialMenuGroupTitle_LastUsed"));
      MyRadialMenu radialMenuDefinition1 = MyDefinitionManager.Static.GetRadialMenuDefinition("Toolbar");
      radialMenuDefinition1.SectionsComplete.Insert(0, this.m_lastUsedBlocks);
      radialMenuDefinition1.SectionsSurvival.Insert(0, this.m_lastUsedBlocks);
      radialMenuDefinition1.SectionsCreative.Insert(0, this.m_lastUsedBlocks);
      this.m_lastUsedVoxel = new MyRadialMenuSection(new List<MyRadialMenuItem>(), MyStringId.GetOrCompute("RadialMenuGroupTitle_LastUsedVoxels"));
      MyRadialMenu radialMenuDefinition2 = MyDefinitionManager.Static.GetRadialMenuDefinition("VoxelHand");
      radialMenuDefinition2.SectionsComplete.Insert(0, this.m_lastUsedVoxel);
      radialMenuDefinition2.SectionsSurvival.Insert(0, this.m_lastUsedVoxel);
      radialMenuDefinition2.SectionsCreative.Insert(0, this.m_lastUsedVoxel);
    }

    public void InitDefaultLastUsed(MyObjectBuilder_Toolbar toolbar)
    {
      foreach (MyObjectBuilder_Toolbar.Slot slot in toolbar.Slots)
      {
        if (this.m_lastUsedBlocks.Items.Count >= 8)
          break;
        if (slot.Data is MyObjectBuilder_ToolbarItemCubeBlock data)
        {
          MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition((MyDefinitionId) data.DefinitionId);
          MyBlockVariantGroup blockVariantsGroup = cubeBlockDefinition.BlockVariantsGroup;
          int num1 = 0;
          int num2 = 0;
          foreach (MyCubeBlockDefinitionGroup blockGroup in blockVariantsGroup.BlockGroups)
          {
            if (blockGroup.Contains(cubeBlockDefinition, false))
            {
              num1 = num2;
              break;
            }
            ++num2;
          }
          MyObjectBuilder_RadialMenuItemCubeBlock menuItemCubeBlock = new MyObjectBuilder_RadialMenuItemCubeBlock()
          {
            Id = (SerializableDefinitionId) blockVariantsGroup.Id
          };
          MyRadialMenuItemCubeBlock block = new MyRadialMenuItemCubeBlock();
          block.Init((MyObjectBuilder_RadialMenuItem) menuItemCubeBlock);
          this.PushLastUsedBlock(block, new int?(num1));
        }
      }
    }

    public void PushLastUsedBlock(
      MyRadialMenuItemCubeBlock block,
      int? idx = null,
      bool pushOnlyNewGroups = false)
    {
      if (block == null)
        return;
      int? nullable = idx;
      int num = 0;
      if (nullable.GetValueOrDefault() < num & nullable.HasValue || idx.HasValue && this.LastBlocksContainBlock(block, idx.Value) || pushOnlyNewGroups && idx.HasValue && this.LastBlocksContainGroup(block, idx.Value))
        return;
      this.m_lastUsedBlocks.Items.Insert(0, (MyRadialMenuItem) block.BuildRecentMenuItem(idx));
      if (this.m_lastUsedBlocks.Items.Count <= 8)
        return;
      this.m_lastUsedBlocks.Items.RemoveAt(8);
    }

    public bool LastBlocksContainBlock(MyRadialMenuItemCubeBlock block, int id = 0)
    {
      foreach (MyRadialMenuItem myRadialMenuItem in this.m_lastUsedBlocks.Items)
      {
        if (myRadialMenuItem is MyRadialMenuItemCubeBlockSingle itemCubeBlockSingle)
        {
          if (itemCubeBlockSingle.BlockVariantGroup.Id == block.BlockVariantGroup.Id && itemCubeBlockSingle.CurrentIndex == id)
            return true;
        }
        else if (myRadialMenuItem is MyRadialMenuItemCubeBlock menuItemCubeBlock && menuItemCubeBlock.BlockVariantGroup.Id == block.BlockVariantGroup.Id)
          return true;
      }
      return false;
    }

    public bool LastBlocksContainGroup(MyRadialMenuItemCubeBlock block, int id = 0)
    {
      foreach (MyRadialMenuItem myRadialMenuItem in this.m_lastUsedBlocks.Items)
      {
        if (myRadialMenuItem is MyRadialMenuItemCubeBlockSingle itemCubeBlockSingle)
        {
          if (itemCubeBlockSingle.BlockVariantGroup.Id == block.BlockVariantGroup.Id)
            return true;
        }
        else if (myRadialMenuItem is MyRadialMenuItemCubeBlock menuItemCubeBlock && menuItemCubeBlock.BlockVariantGroup.Id == block.BlockVariantGroup.Id)
          return true;
      }
      return false;
    }

    public void PushLastUsedVoxel(MyRadialMenuItemVoxelHand voxel)
    {
      if (voxel == null)
        return;
      if (this.m_lastUsedVoxel.Items.Contains((MyRadialMenuItem) voxel))
        this.m_lastUsedVoxel.Items.Remove((MyRadialMenuItem) voxel);
      this.m_lastUsedVoxel.Items.Insert(0, (MyRadialMenuItem) voxel);
      if (this.m_lastUsedVoxel.Items.Count <= 8)
        return;
      this.m_lastUsedVoxel.Items.RemoveAt(8);
    }

    public void ShowSystemRadialMenu(MyStringId context, Func<bool> inputCallback) => MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiControlRadialMenuSystem(MyDefinitionManager.Static.GetRadialMenuDefinition(!(context == MySpaceBindingCreator.AX_ACTIONS) ? (context == MySpaceBindingCreator.AX_BUILD || context == MySpaceBindingCreator.AX_SYMMETRY ? "SystemBuild" : "SystemDefault") : "SystemShip"), MyControlsSpace.SYSTEM_RADIAL_MENU, inputCallback));
  }
}
