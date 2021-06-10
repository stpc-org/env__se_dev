// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyToolbarItemCubeBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.GUI;
using VRage.Game.ObjectBuilders.Components;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.Screens.Helpers
{
  [MyToolbarItemDescriptor(typeof (MyObjectBuilder_ToolbarItemCubeBlock))]
  public class MyToolbarItemCubeBlock : MyToolbarItemDefinition
  {
    private MyFixedPoint m_lastAmount = (MyFixedPoint) 0;

    public MyFixedPoint Amount => this.m_lastAmount;

    public MyCubeBlockDefinition BlockDefinition => (MyCubeBlockDefinition) this.Definition;

    public override bool Equals(object obj)
    {
      if (obj is MyToolbarItemCubeBlock toolbarItemCubeBlock)
      {
        MyCubeBlockDefinition blockDefinition1 = this.BlockDefinition;
        MyCubeBlockDefinition blockDefinition2 = toolbarItemCubeBlock.BlockDefinition;
        if (blockDefinition1 == blockDefinition2 || blockDefinition1.BlockPairName == blockDefinition2.BlockPairName || blockDefinition1.BlockVariantsGroup != null && blockDefinition1.BlockVariantsGroup == blockDefinition2.BlockVariantsGroup && (blockDefinition1.BlockStages != null && blockDefinition2.BlockStages != null))
          return true;
      }
      return base.Equals(obj);
    }

    public override bool Activate()
    {
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      MyDefinitionId weaponDefinition = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_CubePlacer));
      if (localCharacter != null)
      {
        if (!MySessionComponentSafeZones.IsActionAllowed((MyEntity) localCharacter, MySafeZoneAction.Building))
          return false;
        if (localCharacter.CurrentWeapon == null || !(localCharacter.CurrentWeapon.DefinitionId == weaponDefinition))
          localCharacter.SwitchToWeapon(weaponDefinition);
        MyCubeBuilder.Static.Activate(new MyDefinitionId?(this.Definition.Id));
      }
      else if (MyBlockBuilderBase.SpectatorIsBuilding)
        MyCubeBuilder.Static.Activate(new MyDefinitionId?(this.Definition.Id));
      return true;
    }

    public override bool AllowedInToolbarType(MyToolbarType type) => type == MyToolbarType.Character || type == MyToolbarType.Spectator || type == MyToolbarType.BuildCockpit;

    public override bool Init(MyObjectBuilder_ToolbarItem data)
    {
      bool flag = base.Init(data);
      this.ActivateOnClick = false;
      if (flag && MyHud.HudDefinition != null && !Sandbox.Engine.Platform.Game.IsDedicated)
      {
        MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(this.BlockDefinition.BlockPairName);
        foreach (MyCubeSize size in MyEnum<MyCubeSize>.Values)
        {
          MyCubeBlockDefinition cubeBlockDefinition = definitionGroup[size];
          if ((cubeBlockDefinition != null ? (!cubeBlockDefinition.BlockStages.IsNullOrEmpty<MyDefinitionId>() ? 1 : 0) : 0) != 0)
          {
            int num = (int) this.SetSubIcon(MyGuiTextures.Static.GetTexture(MyHud.HudDefinition.Toolbar.ItemStyle.VariantTexture).Path);
          }
        }
      }
      return flag;
    }

    public override MyToolbarItem.ChangeInfo Update(MyEntity owner, long playerID = 0)
    {
      MyToolbarItem.ChangeInfo changeInfo = MyToolbarItem.ChangeInfo.None;
      bool newEnabled = true;
      if (MyCubeBuilder.Static == null)
        return changeInfo;
      MyCubeBlockDefinition cubeBlockDefinition = MyCubeBuilder.Static.IsActivated ? MyCubeBuilder.Static.ToolbarBlockDefinition : (MyCubeBlockDefinition) null;
      MyCubeBlockDefinition definition = this.Definition as MyCubeBlockDefinition;
      if (MyCubeBuilder.Static.IsActivated && cubeBlockDefinition != null)
      {
        if (cubeBlockDefinition.BlockPairName == definition.BlockPairName)
          this.WantsToBeSelected = true;
        else
          this.WantsToBeSelected = false;
      }
      else
        this.WantsToBeSelected = false;
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      if (MyFakes.ENABLE_GATHERING_SMALL_BLOCK_FROM_GRID && definition.CubeSize == MyCubeSize.Small && localCharacter != null)
      {
        MyInventory inventory = MyEntityExtensions.GetInventory(localCharacter);
        MyFixedPoint myFixedPoint = inventory != null ? inventory.GetItemAmount(this.Definition.Id, MyItemFlags.None, false) : (MyFixedPoint) 0;
        if (this.m_lastAmount != myFixedPoint)
        {
          this.m_lastAmount = myFixedPoint;
          changeInfo |= MyToolbarItem.ChangeInfo.IconText;
        }
        if (MySession.Static.SurvivalMode)
          newEnabled &= this.m_lastAmount > (MyFixedPoint) 0;
        else
          changeInfo |= MyToolbarItem.ChangeInfo.IconText;
      }
      MySessionComponentResearch componentResearch;
      if (MySession.Static.ResearchEnabled && !MySession.Static.CreativeToolsEnabled(Sync.MyId) && (componentResearch = MySessionComponentResearch.Static) != null)
      {
        bool flag = false;
        if (this.BlockDefinition.BlockVariantsGroup != null)
        {
          foreach (MyCubeBlockDefinition block in this.BlockDefinition.BlockVariantsGroup.Blocks)
          {
            if (componentResearch.CanUse(localCharacter, block.Id))
            {
              flag = true;
              break;
            }
          }
        }
        else
          flag = componentResearch.CanUse(localCharacter, this.BlockDefinition.Id);
        newEnabled &= flag;
      }
      if (this.Enabled != newEnabled)
        changeInfo |= this.SetEnabled(newEnabled);
      return changeInfo;
    }

    public override void FillGridItem(MyGuiGridItem gridItem)
    {
      if (!MyFakes.ENABLE_GATHERING_SMALL_BLOCK_FROM_GRID)
        return;
      if (this.m_lastAmount > (MyFixedPoint) 0)
        gridItem.AddText(string.Format("{0}x", (object) this.m_lastAmount), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
      else
        gridItem.ClearText(MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
    }
  }
}
