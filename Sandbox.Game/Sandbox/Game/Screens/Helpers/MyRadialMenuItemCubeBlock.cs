// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyRadialMenuItemCubeBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
  [MyRadialMenuItemDescriptor(typeof (MyObjectBuilder_RadialMenuItemCubeBlock))]
  internal class MyRadialMenuItemCubeBlock : MyRadialMenuItem
  {
    public override bool CanBeActivated => MyRadialMenuItemCubeBlock.IsBlockGroupEnabled(MyCubeBuilder.Static.CubeBuilderState.GetCurrentBlockForBlockVariantGroup(this.BlockVariantGroup), out MyDLCs.MyDLC _) == MyRadialMenuItemCubeBlock.EnabledState.Enabled;

    public MyBlockVariantGroup BlockVariantGroup { get; protected set; }

    public override void Init(MyObjectBuilder_RadialMenuItem builder)
    {
      MyObjectBuilder_RadialMenuItemCubeBlock menuItemCubeBlock = (MyObjectBuilder_RadialMenuItemCubeBlock) builder;
      MyBlockVariantGroup blockVariantGroup;
      MyDefinitionManager.Static.GetBlockVariantGroupDefinitions().TryGetValue(menuItemCubeBlock.Id.SubtypeId, out blockVariantGroup);
      this.BlockVariantGroup = blockVariantGroup;
      this.Icons = new List<string>();
      this.LabelName = string.Empty;
      if (blockVariantGroup != null)
      {
        if (blockVariantGroup.DisplayNameEnum.HasValue)
          this.LabelName = MyTexts.GetString(blockVariantGroup.DisplayNameEnum.Value);
        else
          MyDefinitionErrors.Add(MyModContext.UnknownContext, "Block " + menuItemCubeBlock.Id.TypeIdString + "/" + menuItemCubeBlock.Id.SubtypeId + " block variant group doesn't have `DisplayNameEnum` property", TErrorSeverity.Warning);
        this.Icons.Add(blockVariantGroup.Icons[0]);
      }
      else
        MyDefinitionErrors.Add(MyModContext.UnknownContext, "Block " + menuItemCubeBlock.Id.TypeIdString + "/" + menuItemCubeBlock.Id.SubtypeId + " doesn't have block variant group!", TErrorSeverity.Warning);
      this.CloseMenu = builder.CloseMenu;
    }

    public override bool Enabled()
    {
      foreach (MyCubeBlockDefinition block in this.BlockVariantGroup.Blocks)
      {
        if (MyRadialMenuItemCubeBlock.IsBlockEnabled(block, out MyDLCs.MyDLC _) == MyRadialMenuItemCubeBlock.EnabledState.Enabled)
          return true;
      }
      return false;
    }

    public override void Activate(params object[] parameters)
    {
      MyCubeBlockDefinitionGroup blockVariantGroup = MyCubeBuilder.Static.CubeBuilderState.GetCurrentBlockForBlockVariantGroup(this.BlockVariantGroup);
      MyCubeSize cubeSize = MyCubeSize.Large;
      object parameter;
      if (parameters.Length != 0 && (parameter = parameters[0]) is MyCubeSize)
        cubeSize = (MyCubeSize) parameter;
      this.ActivateInner(blockVariantGroup, cubeSize, false);
    }

    public virtual void ActivateInner(
      MyCubeBlockDefinitionGroup targetPair,
      MyCubeSize cubeSize,
      bool useRepresentativeIfPossible = true)
    {
      MySession.Static.LocalCharacter?.SwitchToWeapon(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_CubePlacer)));
      MyCubeBuilder.Static.CubeBuilderState.SetCubeSize(cubeSize);
      MyCubeBlockDefinition cubeBlockDefinition = targetPair[cubeSize] ?? targetPair.AnyPublic;
      MyCubeBlockDefinition primaryGuiBlock = cubeBlockDefinition.BlockVariantsGroup?.PrimaryGUIBlock;
      if (useRepresentativeIfPossible && primaryGuiBlock != null)
        cubeBlockDefinition = primaryGuiBlock;
      MyCubeBuilder.Static.Activate(new MyDefinitionId?(cubeBlockDefinition.Id));
      MyCubeBuilder.Static.SetToolType(MyCubeBuilderToolType.BuildTool);
      if (!MySessionComponentVoxelHand.Static.Enabled)
        return;
      MySessionComponentVoxelHand.Static.Enabled = false;
    }

    public static MyRadialMenuItemCubeBlock.EnabledState IsBlockGroupEnabled(
      MyCubeBlockDefinitionGroup blocks,
      out MyDLCs.MyDLC missingDLC)
    {
      MyDLCs.MyDLC missingDLC1;
      MyRadialMenuItemCubeBlock.EnabledState enabledState1 = MyRadialMenuItemCubeBlock.IsBlockEnabled(blocks.Small, out missingDLC1);
      MyDLCs.MyDLC missingDLC2;
      MyRadialMenuItemCubeBlock.EnabledState enabledState2 = MyRadialMenuItemCubeBlock.IsBlockEnabled(blocks.Large, out missingDLC2);
      if (enabledState1 > enabledState2)
      {
        missingDLC = missingDLC1;
        return enabledState1;
      }
      missingDLC = missingDLC2;
      return enabledState2;
    }

    public static MyRadialMenuItemCubeBlock.EnabledState IsBlockEnabled(
      MyCubeBlockDefinition block,
      out MyDLCs.MyDLC missingDLC)
    {
      missingDLC = (MyDLCs.MyDLC) null;
      MyRadialMenuItemCubeBlock.EnabledState enabledState = MyRadialMenuItemCubeBlock.EnabledState.Enabled;
      if (block != null)
      {
        if (!block.Public)
          enabledState |= MyRadialMenuItemCubeBlock.EnabledState.Other;
        MySessionComponentDLC component = MySession.Static.GetComponent<MySessionComponentDLC>();
        missingDLC = component.GetFirstMissingDefinitionDLC((MyDefinitionBase) block, Sync.MyId);
        if (!MySessionComponentResearch.Static.CanUse(MySession.Static.LocalPlayerId, block.Id) && !MySession.Static.CreativeToolsEnabled(Sync.MyId))
          enabledState |= MyRadialMenuItemCubeBlock.EnabledState.Research;
        if (missingDLC != null)
          enabledState |= MyRadialMenuItemCubeBlock.EnabledState.DLC;
      }
      return enabledState;
    }

    public virtual bool MoveItemIndex(int shift)
    {
      MyCubeBlockDefinitionGroup activeGroup = this.GetActiveGroup();
      MyCubeBlockDefinitionGroup[] blockGroups = this.BlockVariantGroup.BlockGroups;
      int num = Array.IndexOf<MyCubeBlockDefinitionGroup>(blockGroups, activeGroup);
      int index = num;
      do
      {
        index = MyMath.Mod(index + shift, blockGroups.Length);
        MyCubeBlockDefinitionGroup blockDefinitionGroup = blockGroups[index];
        if (MyRadialMenuItemCubeBlock.IsBlockGroupEnabled(blockDefinitionGroup, out MyDLCs.MyDLC _) < MyRadialMenuItemCubeBlock.EnabledState.Other)
        {
          MyCubeBuilder.Static.CubeBuilderState.SetCurrentBlockForBlockVariantGroup(blockDefinitionGroup);
          return true;
        }
      }
      while (index != num);
      return false;
    }

    public virtual MyCubeBlockDefinitionGroup GetActiveGroup() => MyCubeBuilder.Static.CubeBuilderState.GetCurrentBlockForBlockVariantGroup(this.BlockVariantGroup);

    public virtual MyCubeBlockDefinitionGroup GetFirstGroup() => MyCubeBuilder.Static.CubeBuilderState.GetFirstBlockForBlockVariantGroup(this.BlockVariantGroup);

    public virtual int GetCurrentIndex() => Array.IndexOf<MyCubeBlockDefinitionGroup>(this.BlockVariantGroup.BlockGroups, this.GetActiveGroup());

    public virtual MyRadialMenuItemCubeBlockSingle BuildRecentMenuItem(
      int? idx = null)
    {
      return MyRadialMenuItemCubeBlockSingle.BuildMenuItem(this, idx);
    }

    public override bool IsValid => MyDefinitionManager.Static.GetBlockVariantGroupDefinitions().ContainsKey(this.BlockVariantGroup.Id.SubtypeName);

    [Flags]
    public enum EnabledState
    {
      Enabled = 0,
      DLC = 1,
      Research = 2,
      Other = 4,
    }
  }
}
