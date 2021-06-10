// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyGuiControlRadialMenuBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Components;
using VRage.Input;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
  internal class MyGuiControlRadialMenuBlock : MyGuiControlRadialMenuBase
  {
    private MyCubeSize m_currentSizeSelection;
    private MyCubeBlockDefinition m_currentBlock;
    private readonly MyGuiControlPcuBar m_pcuBar;
    private readonly MyGuiControlImage m_infoHintsBk;
    private readonly MyGuiControlImage m_blockSizeSmall;
    private readonly MyGuiControlImage m_blockSizeLarge;
    private readonly MyGuiControlBlockGroupInfo m_blockDetail;
    private readonly MyGuiControlMultilineText m_buildPlannerHint;
    private readonly MyGuiControlMultilineText m_cycleBlocksHint;
    private MyGuiControlImage[] m_missingRequirementIcons;

    public static event Action<MyGuiControlRadialMenuBlock> OnSelectionConfirmed;

    public MyGuiControlRadialMenuBlock(
      MyRadialMenu data,
      MyStringId closingControl,
      Func<bool> handleInputCallback)
      : base(data, closingControl, handleInputCallback)
    {
      Vector2 vector2 = new Vector2(90f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      Vector4? backgroundColor1 = new Vector4?(new Vector4(1f, 1f, 1f, 0.8f));
      MyGuiControlImage myGuiControlImage = new MyGuiControlImage(new Vector2?(new Vector2(0.0f, 0.365f)), new Vector2?(MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.MinSizeGui), backgroundColor1, textures: new string[1]
      {
        "Textures\\GUI\\Controls\\button_default_outlineless.dds"
      });
      this.AddControl((IVRageGuiControl) myGuiControlImage);
      this.AddControl((IVRageGuiControl) (this.m_blockSizeLarge = new MyGuiControlImage(size: new Vector2?(vector2 * 0.7f))));
      this.AddControl((IVRageGuiControl) (this.m_blockSizeSmall = new MyGuiControlImage(size: new Vector2?(vector2 * 0.7f))));
      this.m_blockSizeLarge.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\GridSizeLarge.png");
      this.m_blockSizeSmall.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\GridSizeSmall.png");
      Vector2? position = new Vector2?(new Vector2(0.028f, 0.365f + 3f / 1000f));
      Vector2? size = new Vector2?();
      string codeForControl = MyControllerHelper.GetCodeForControl(MyControllerHelper.CX_GUI, MyControlsGUI.ACTION1_MOD1);
      Vector4? backgroundColor2 = new Vector4?();
      Vector4? colorMask = backgroundColor2;
      this.AddControl((IVRageGuiControl) new MyGuiControlLabel(position, size, codeForControl, colorMask, originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
      this.m_blockDetail = new MyGuiControlBlockGroupInfo();
      MyGuiControlBlockGroupInfo blockDetail = this.m_blockDetail;
      MyObjectBuilder_GuiControlBlockGroupInfo controlBlockGroupInfo = new MyObjectBuilder_GuiControlBlockGroupInfo();
      controlBlockGroupInfo.Name = string.Empty;
      controlBlockGroupInfo.Size = new Vector2(0.27f, 0.68f);
      controlBlockGroupInfo.Position = new Vector2(0.3f, -0.355f);
      controlBlockGroupInfo.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      blockDetail.Init((MyObjectBuilder_GuiControlBase) controlBlockGroupInfo);
      this.AddControl((IVRageGuiControl) this.m_blockDetail);
      this.m_blockDetail.RegisterAllControls(this.Controls);
      this.m_blockDetail.UpdateArrange();
      float x1 = this.m_blockDetail.Position.X + this.m_blockDetail.Size.X;
      float x2 = this.m_blockDetail.Size.X;
      backgroundColor2 = new Vector4?(new Vector4(1f, 1f, 1f, 0.8f));
      this.m_infoHintsBk = new MyGuiControlImage(new Vector2?(new Vector2(x1, (float) (0.365000009536743 - (double) myGuiControlImage.Size.Y / 2.0))), new Vector2?(new Vector2(x2, 0.12f)), backgroundColor2, textures: new string[1]
      {
        "Textures\\GUI\\Controls\\button_default_outlineless.dds"
      }, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.m_infoHintsBk.Visible = false;
      this.AddControl((IVRageGuiControl) this.m_infoHintsBk);
      this.m_buildPlannerHint = new MyGuiControlMultilineText();
      this.m_buildPlannerHint.Position = this.m_infoHintsBk.Position - new Vector2(this.m_infoHintsBk.Size.X, 0.0f) + new Vector2(0.01f, 0.01f);
      this.m_buildPlannerHint.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_buildPlannerHint.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_buildPlannerHint.Size = this.m_infoHintsBk.Size;
      this.m_buildPlannerHint.Margin = new Thickness(0.015f, 0.005f, 0.015f, 0.005f);
      this.Controls.Add((MyGuiControlBase) this.m_buildPlannerHint);
      this.m_buildPlannerHint.Parse();
      this.m_cycleBlocksHint = new MyGuiControlMultilineText();
      this.m_cycleBlocksHint.Position = this.m_infoHintsBk.Position - new Vector2(this.m_infoHintsBk.Size.X, 0.0f) + new Vector2(0.01f, 0.09f);
      this.m_cycleBlocksHint.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_cycleBlocksHint.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_cycleBlocksHint.Size = this.m_infoHintsBk.Size + new Vector2(0.01f, 0.01f);
      this.RegenerateBlockHints();
      this.m_cycleBlocksHint.Margin = new Thickness(0.015f, 0.005f, 0.015f, 0.005f);
      this.AddControl((IVRageGuiControl) this.m_cycleBlocksHint);
      this.m_cycleBlocksHint.Parse();
      MyGuiControlPcuBar guiControlPcuBar = new MyGuiControlPcuBar(new Vector2?(new Vector2(0.0f, 0.459f)), new float?(0.548f));
      guiControlPcuBar.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;
      this.m_pcuBar = guiControlPcuBar;
      this.m_pcuBar.UpdatePCU(MyGuiControlRadialMenuBlock.GetIdentity(), false);
      this.AddControl((IVRageGuiControl) this.m_pcuBar);
      this.UpdateBuildPlanner();
      this.RefreshBlockSize(MyCubeBuilder.Static.CubeBuilderState.CubeSizeMode);
      MyCubeBuilder.Static.CubeBuilderState.OnBlockSizeChanged += new Action<MyCubeSize>(this.RefreshBlockSize);
      this.SwitchSection(0);
    }

    protected override void RegenerateBlockHints()
    {
      if (this.m_currentSection < 0 || this.m_currentSection >= this.m_data.CurrentSections.Count)
      {
        this.m_cycleBlocksHint.Text = new StringBuilder();
      }
      else
      {
        List<MyRadialMenuItem> items = this.m_data.CurrentSections[this.m_currentSection].Items;
        if (this.m_selectedButton < 0 || this.m_selectedButton >= items.Count)
          this.m_cycleBlocksHint.Text = new StringBuilder();
        else if (items[this.m_selectedButton] is MyRadialMenuItemCubeBlockSingle)
          this.m_cycleBlocksHint.Text = new StringBuilder();
        else
          this.m_cycleBlocksHint.Text = new StringBuilder(string.Format(MyTexts.GetString(MySpaceTexts.RadialMenu_HintCycleBlocks) + "\n", (object) MyControllerHelper.GetCodeForControl(MyControllerHelper.CX_GUI, MyControlsGUI.ACCEPT)));
      }
    }

    private static MyIdentity GetIdentity()
    {
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      MyPlayer myPlayer = (MyPlayer) null;
      if (localCharacter != null)
        myPlayer = MyPlayer.GetPlayerFromCharacter(localCharacter);
      if (myPlayer == null && ((MyToolbarComponent.CurrentToolbar.Owner is MyShipController owner ? owner.Pilot : (MyCharacter) null) != null && owner.ControllerInfo.Controller != null))
        myPlayer = owner.ControllerInfo.Controller.Player;
      return myPlayer?.Identity;
    }

    public override bool Update(bool hasFocus)
    {
      this.m_pcuBar.UpdatePCU(MyGuiControlRadialMenuBlock.GetIdentity(), true);
      return base.Update(hasFocus);
    }

    protected override void ActivateItem(MyRadialMenuItem item)
    {
      item.Activate((object) this.m_currentSizeSelection);
      if (item is MyRadialMenuItemCubeBlock block)
        MySession.Static.GetComponent<MyRadialMenuComponent>().PushLastUsedBlock(block, new int?(block.GetCurrentIndex()));
      MyGuiControlRadialMenuBlock.OnSelectionConfirmed.InvokeIfNotNull<MyGuiControlRadialMenuBlock>(this);
    }

    protected override void OnClosed()
    {
      MyCubeBuilder.Static.CubeBuilderState.OnBlockSizeChanged -= new Action<MyCubeSize>(this.RefreshBlockSize);
      base.OnClosed();
    }

    private void RefreshBlockSize(MyCubeSize size)
    {
      this.m_currentSizeSelection = size;
      this.UpdateTooltip();
      this.m_blockDetail.OnUserSizePreferenceChanged(size);
    }

    public void UpdateBuildPlanner() => this.m_blockDetail.UpdateBuildPlanner();

    protected override void UpdateTooltip()
    {
      bool flag = false;
      MyDLCs.MyDLC missingDLC = (MyDLCs.MyDLC) null;
      int allowedCubeSizes = 0;
      MyCubeSize cubeSizeMode = MyCubeBuilder.Static.CubeBuilderState.CubeSizeMode;
      if (MyCubeBuilder.Static.IsActivated)
        this.m_currentSizeSelection = cubeSizeMode;
      List<MyRadialMenuItem> items = this.m_data.CurrentSections[this.m_currentSection].Items;
      if (this.m_selectedButton >= 0 && this.m_selectedButton < items.Count)
      {
        MyRadialMenuItem myRadialMenuItem = items[this.m_selectedButton];
        MyCubeBlockDefinitionGroup blockDefinitionGroup = (MyCubeBlockDefinitionGroup) null;
        if ((myRadialMenuItem is MyRadialMenuItemCubeBlock menuItemCubeBlock ? menuItemCubeBlock.BlockVariantGroup?.BlockGroups : (MyCubeBlockDefinitionGroup[]) null) != null)
        {
          blockDefinitionGroup = myRadialMenuItem is MyRadialMenuItemCubeBlock menuItemCubeBlock ? menuItemCubeBlock.GetFirstGroup() : (MyCubeBlockDefinitionGroup) null;
          foreach (MyCubeSize size in MyEnum<MyCubeSize>.Values)
          {
            if (blockDefinitionGroup[size] != null)
              allowedCubeSizes |= 1 << (int) (size & (MyCubeSize) 31);
          }
          if (blockDefinitionGroup[this.m_currentSizeSelection] == null)
            this.m_currentSizeSelection = blockDefinitionGroup.Any.CubeSize;
        }
        flag = MyRadialMenuItemCubeBlock.IsBlockGroupEnabled(blockDefinitionGroup, out missingDLC) >= MyRadialMenuItemCubeBlock.EnabledState.Research;
        this.m_infoHintsBk.Visible = true;
        this.m_blockDetail.Visible = true;
        this.m_blockDetail.SetBlockGroup(blockDefinitionGroup);
        this.m_currentBlock = blockDefinitionGroup[this.m_currentSizeSelection];
        this.m_buildPlannerHint.Visible = true;
        this.m_cycleBlocksHint.Visible = true;
      }
      else
      {
        allowedCubeSizes = int.MaxValue;
        this.m_currentBlock = (MyCubeBlockDefinition) null;
        this.m_blockDetail.Visible = false;
        this.m_infoHintsBk.Visible = false;
        this.m_buildPlannerHint.Visible = false;
        this.m_cycleBlocksHint.Visible = false;
      }
      SetCubeSizeIconVisibility(MyCubeSize.Small, this.m_blockSizeSmall);
      SetCubeSizeIconVisibility(MyCubeSize.Large, this.m_blockSizeLarge);
      StringBuilder stringBuilder = this.m_buildPlannerHint.Text.Clear();
      string codeForControl1 = MyControllerHelper.GetCodeForControl(MyControllerHelper.CX_GUI, MyControlsGUI.ACTION1);
      string codeForControl2 = MyControllerHelper.GetCodeForControl(MyControllerHelper.CX_GUI, MyControlsGUI.ACTION2);
      if (flag || missingDLC != null)
      {
        if (flag)
        {
          stringBuilder.Append(codeForControl1).Append(' ');
          stringBuilder.Append(MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ShowProgressionTree));
          stringBuilder.AppendLine();
        }
        if (missingDLC != null)
        {
          stringBuilder.Append(codeForControl2).Append(' ');
          MyTexts.AppendFormat(stringBuilder, MyCommonTexts.ShowDlcStore, missingDLC.DisplayName);
          stringBuilder.AppendLine();
        }
      }
      else
        stringBuilder.AppendFormat(MySpaceTexts.BuildPlannerHint, (object) codeForControl1, (object) codeForControl2);
      this.m_buildPlannerHint.RefreshText(false);

      void SetCubeSizeIconVisibility(MyCubeSize size, MyGuiControlImage icon)
      {
        if (icon == null)
          return;
        int num1 = 1 << (int) (size & (MyCubeSize) 31);
        bool flag = (uint) (allowedCubeSizes & ~num1) > 0U;
        icon.Visible = (uint) (allowedCubeSizes & num1) > 0U;
        icon.ColorMask = new Vector4(1f, 1f, 1f, this.m_currentSizeSelection != size & flag ? 0.2f : 1f);
        float x = -0.025f;
        if (!flag)
        {
          float num2 = icon.Size.X / 4f;
          if (size == MyCubeSize.Large)
            x -= num2;
          else
            x += num2;
        }
        icon.Position = new Vector2(x, 0.365f);
      }
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      bool flag = false;
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT))
      {
        if (this.m_data == null)
          MyLog.Default.Error("RadialMenuBlock m_data are null!");
        if (this.m_data.CurrentSections.Count < this.m_currentSection || this.m_data.CurrentSections[this.m_currentSection] == null)
          MyLog.Default.Error("Section not present!");
        List<MyRadialMenuItem> items = this.m_data.CurrentSections[this.m_currentSection].Items;
        MyDefinitionBase definition;
        if (items != null && this.m_selectedButton >= 0 && this.m_selectedButton < items.Count && ((this.m_blockDetail.GetSelectedVariant().UserData is MyGuiScreenToolbarConfigBase.GridItemUserData userData ? userData.ItemData() : (MyObjectBuilder_ToolbarItem) null) is MyObjectBuilder_ToolbarItemDefinition toolbarItemDefinition && MyDefinitionManager.Static.TryGetDefinition<MyDefinitionBase>((MyDefinitionId) toolbarItemDefinition.DefinitionId, out definition)))
        {
          MyCharacter localCharacter = MySession.Static.LocalCharacter;
          MyDefinitionId weaponDefinition = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_CubePlacer));
          if (localCharacter != null && MySessionComponentSafeZones.IsActionAllowed((MyEntity) localCharacter, MySafeZoneAction.Building))
          {
            if (localCharacter.CurrentWeapon == null || !(localCharacter.CurrentWeapon.DefinitionId == weaponDefinition))
              localCharacter.SwitchToWeapon(weaponDefinition);
            MyCubeBuilder.Static.CubeBuilderState.SetCubeSize(this.m_currentSizeSelection == MyCubeSize.Large ? MyCubeSize.Small : MyCubeSize.Large);
            MyCubeBuilder.Static.ActivateFromRadialMenu(new MyDefinitionId?(definition.Id));
            MyCubeBuilder.Static.SetToolType(MyCubeBuilderToolType.BuildTool);
            if (items[this.m_selectedButton] is MyRadialMenuItemCubeBlock block)
              MySession.Static.GetComponent<MyRadialMenuComponent>().PushLastUsedBlock(block, new int?(block.GetCurrentIndex()), true);
            MyDefinitionId[] blockStages = this.m_currentBlock?.BlockStages;
            if (blockStages == null)
            {
              if (this.m_currentBlock != null && this.m_currentBlock.BlockVariantsGroup != null && (this.m_currentBlock.BlockVariantsGroup.PrimaryGUIBlock != null && this.m_currentBlock.BlockVariantsGroup.PrimaryGUIBlock.BlockStages != null))
                blockStages = this.m_currentBlock.BlockVariantsGroup.PrimaryGUIBlock.BlockStages;
            }
            else
              MyCubeBuilder.Static.CubeBuilderState.CurrentBlockDefinitionStages.Add(this.m_currentBlock);
            if (blockStages != null)
            {
              foreach (MyDefinitionId defId in blockStages)
              {
                MyCubeBlockDefinition blockDefinition;
                MyDefinitionManager.Static.TryGetCubeBlockDefinition(defId, out blockDefinition);
                if (blockDefinition != null)
                  MyCubeBuilder.Static.CubeBuilderState.CurrentBlockDefinitionStages.Add(blockDefinition);
              }
            }
          }
          this.CloseScreen();
          flag = true;
        }
      }
      if (!flag)
        base.HandleInput(receivedFocusInThisUpdate);
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.ACTION1_MOD1))
      {
        MyCubeSize myCubeSize = this.m_currentSizeSelection;
        if (MyCubeBuilder.Static.IsActivated)
          myCubeSize = MyCubeBuilder.Static.CubeBuilderState.CubeSizeMode;
        MyCubeBuilder.Static.CubeBuilderState.SetCubeSize(myCubeSize == MyCubeSize.Large ? MyCubeSize.Small : MyCubeSize.Large);
        MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
      }
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.CANCEL_MOD1))
        this.Cancel();
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.ACTION1) && this.m_currentBlock != null)
      {
        List<MyRadialMenuItem> items = this.m_data.CurrentSections[this.m_currentSection].Items;
        MyCubeBlockDefinition definition;
        if (items != null && this.m_selectedButton >= 0 && this.m_selectedButton < items.Count && ((this.m_blockDetail.GetSelectedVariant().UserData is MyGuiScreenToolbarConfigBase.GridItemUserData userData ? userData.ItemData() : (MyObjectBuilder_ToolbarItem) null) is MyObjectBuilder_ToolbarItemDefinition toolbarItemDefinition && MyDefinitionManager.Static.TryGetDefinition<MyCubeBlockDefinition>((MyDefinitionId) toolbarItemDefinition.DefinitionId, out definition)))
        {
          MyRadialMenuItemCubeBlock.EnabledState enabledState = MyRadialMenuItemCubeBlock.IsBlockEnabled(definition, out MyDLCs.MyDLC _);
          if (enabledState == MyRadialMenuItemCubeBlock.EnabledState.Enabled)
          {
            if (MySession.Static.LocalCharacter.AddToBuildPlanner(definition))
              this.UpdateBuildPlanner();
          }
          else if ((enabledState & MyRadialMenuItemCubeBlock.EnabledState.Research) != MyRadialMenuItemCubeBlock.EnabledState.Enabled)
          {
            this.CloseScreen();
            this.OpenPrograssionTree();
          }
        }
      }
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.ACTION2) && this.m_currentBlock != null)
      {
        MyDLCs.MyDLC missingDLC;
        MyRadialMenuItemCubeBlock.EnabledState enabledState = MyRadialMenuItemCubeBlock.IsBlockEnabled(this.m_currentBlock, out missingDLC);
        if (enabledState == MyRadialMenuItemCubeBlock.EnabledState.Enabled)
        {
          MyCharacter localCharacter = MySession.Static.LocalCharacter;
          if ((localCharacter != null ? (localCharacter.BuildPlanner.Count > 0 ? 1 : 0) : 0) != 0)
          {
            int index = -1;
            int num = 0;
            foreach (MyIdentity.BuildPlanItem buildPlanItem in (IEnumerable<MyIdentity.BuildPlanItem>) MySession.Static.LocalCharacter.BuildPlanner)
            {
              if (buildPlanItem.BlockDefinition == this.m_currentBlock)
                index = num;
              ++num;
            }
            if (index >= 0)
              MySession.Static.LocalCharacter.RemoveAtBuildPlanner(index);
            else
              MySession.Static.LocalCharacter.RemoveAtBuildPlanner(MySession.Static.LocalCharacter.BuildPlanner.Count - 1);
            this.UpdateBuildPlanner();
          }
        }
        else if ((enabledState & MyRadialMenuItemCubeBlock.EnabledState.DLC) != MyRadialMenuItemCubeBlock.EnabledState.Enabled)
          MyGameService.OpenDlcInShop(missingDLC.AppId);
      }
      if (!MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.ACCEPT))
        return;
      this.m_blockDetail.SelectNextVariant();
    }

    private void OpenPrograssionTree() => MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.ToolbarConfigScreen, (object) 0, (object) (MySession.Static.ControlledEntity as MyShipController), (object) "ResearchPage", (object) true, null));

    protected override void GenerateIcons(int maxSize)
    {
      base.GenerateIcons(maxSize);
      this.m_missingRequirementIcons = new MyGuiControlImage[maxSize];
      for (int index = 0; index < maxSize; ++index)
      {
        MyGuiControlImage myGuiControlImage = new MyGuiControlImage(size: new Vector2?(new Vector2(40f) / MyGuiConstants.GUI_OPTIMAL_SIZE), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
        this.AddControl((IVRageGuiControl) myGuiControlImage);
        this.m_missingRequirementIcons[index] = myGuiControlImage;
        MyGuiControlImage icon = this.m_icons[index];
        myGuiControlImage.Position = icon.Position + icon.Size / 2f + new Vector2(0.004f, 0.004f);
      }
    }

    protected override void SetIconTextures(MyRadialMenuSection selectedSection)
    {
      base.SetIconTextures(selectedSection);
      List<MyRadialMenuItem> items = selectedSection.Items;
      for (int index = 0; index < this.m_missingRequirementIcons.Length; ++index)
      {
        MyGuiControlImage missingRequirementIcon = this.m_missingRequirementIcons[index];
        string texture = (string) null;
        if (index < items.Count)
        {
          MyRadialMenuItemCubeBlock menuItemCubeBlock = (MyRadialMenuItemCubeBlock) items[index];
          if (!menuItemCubeBlock.Enabled())
          {
            foreach (MyCubeBlockDefinitionGroup blockGroup in menuItemCubeBlock.BlockVariantGroup.BlockGroups)
            {
              MyDLCs.MyDLC missingDLC;
              MyRadialMenuItemCubeBlock.EnabledState enabledState = MyRadialMenuItemCubeBlock.IsBlockGroupEnabled(blockGroup, out missingDLC);
              if ((enabledState & MyRadialMenuItemCubeBlock.EnabledState.Research) != MyRadialMenuItemCubeBlock.EnabledState.Enabled)
              {
                texture = "Textures\\GUI\\Icons\\HUD 2017\\ProgressionTree.png";
                break;
              }
              if ((enabledState & MyRadialMenuItemCubeBlock.EnabledState.DLC) != MyRadialMenuItemCubeBlock.EnabledState.Enabled && texture == null)
                texture = missingDLC.Icon;
            }
          }
        }
        if (texture == null)
        {
          missingRequirementIcon.Visible = false;
        }
        else
        {
          missingRequirementIcon.Visible = true;
          missingRequirementIcon.SetTexture(texture);
        }
      }
    }
  }
}
