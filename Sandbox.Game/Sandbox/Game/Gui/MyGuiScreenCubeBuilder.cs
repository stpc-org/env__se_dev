// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenCubeBuilder
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using System;
using VRage.Game;
using VRage.Game.Definitions.Animation;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyGuiScreenCubeBuilder : MyGuiScreenToolbarConfigBase
  {
    private MyGuiGridItem m_lastGridBlocksMouseOverItem;

    public MyGuiScreenCubeBuilder(int scrollOffset = 0, MyCubeBlock owner = null, int? gamepadSlot = null)
      : this(scrollOffset, owner, (string) null, false, gamepadSlot)
    {
    }

    public MyGuiScreenCubeBuilder(
      int scrollOffset,
      MyCubeBlock owner,
      string selectedPage,
      bool hideOtherPages,
      int? gamepadSlot = null)
      : base(MyHud.HudDefinition.Toolbar, scrollOffset, owner, gamepadSlot)
    {
      MySandboxGame.Log.WriteLine("MyGuiScreenCubeBuilder.ctor START");
      MyGuiScreenToolbarConfigBase.Static = (MyGuiScreenToolbarConfigBase) this;
      this.m_scrollOffset = (float) scrollOffset / 6.5f;
      this.m_size = new Vector2?(new Vector2(1f, 1f));
      this.m_canShareInput = true;
      this.m_drawEvenWithoutFocus = true;
      this.EnabledBackgroundFade = true;
      this.m_screenOwner = owner;
      this.RecreateControls(true);
      if (!string.IsNullOrWhiteSpace(selectedPage))
      {
        foreach (MyGuiControlTabPage page in this.m_tabControl.Pages)
        {
          if (page.Name == selectedPage)
          {
            this.m_tabControl.SelectedPage = page.PageKey;
            break;
          }
        }
      }
      if (hideOtherPages)
      {
        foreach (MyGuiControlTabPage page in this.m_tabControl.Pages)
        {
          int num;
          bool flag = (num = page.PageKey == this.m_tabControl.SelectedPage ? 1 : 0) != 0;
          page.Enabled = num != 0;
          page.IsTabVisible = flag;
        }
      }
      MySandboxGame.Log.WriteLine("MyGuiScreenCubeBuilder.ctor END");
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenCubeBuilder);

    public override void RecreateControls(bool contructor)
    {
      base.RecreateControls(contructor);
      this.m_gridBlocks.MouseOverIndexChanged += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.OnGridMouseOverIndexChanged);
      this.m_gridBlocks.ItemSelected += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.OnSelectedItemChanged);
      this.m_researchGraph.MouseOverItemChanged += new EventHandler(this.m_researchGraph_MouseOverItemChanged);
      this.m_researchGraph.SelectedItemChanged += new EventHandler(this.m_researchGraph_SelectedItemChanged);
      this.m_blockGroupInfo = (MyGuiControlBlockGroupInfo) this.Controls.GetControlByName("BlockInfoPanel");
      this.m_blockGroupInfo.RegisterAllControls(this.Controls);
      this.m_blockGroupInfo.ColorMask = this.m_gridBlocks.ColorMask;
      this.m_blockGroupInfo.GetGridForDragAndDrop().ItemDragged += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(((MyGuiScreenToolbarConfigBase) this).grid_OnDrag);
      this.m_blockGroupInfo.UpdateArrange();
      this.CloseButtonStyle = MyGuiControlButtonStyleEnum.CloseBackground;
      this.m_blockGroupInfo.SetBlockModeEnabled(!this.m_shipMode);
      foreach (MyGuiControlBase control1 in this.m_blockGroupInfo.GetControls())
      {
        control1.Visible = !this.m_shipMode;
        if (control1 is MyGuiControlStackPanel controlStackPanel)
        {
          foreach (MyGuiControlBase control2 in controlStackPanel.GetControls())
            control2.Visible = !this.m_shipMode;
        }
      }
      this.m_blockGroupInfo.Visible = false;
    }

    private void m_researchGraph_SelectedItemChanged(object sender, System.EventArgs e)
    {
      if (!this.m_researchGraph.Visible)
        return;
      this.ShowItem(this.m_researchGraph.SelectedItem);
    }

    private void OnSelectedItemChanged(MyGuiControlGrid arg1, MyGuiControlGrid.EventArgs arg2) => this.OnGridMouseOverIndexChanged(arg1, arg2);

    private void m_researchGraph_MouseOverItemChanged(object sender, System.EventArgs e)
    {
      if (!this.m_researchGraph.Visible)
        return;
      this.ShowItem(this.m_researchGraph.MouseOverItem ?? this.m_researchGraph.SelectedItem);
    }

    private void OnGridMouseOverIndexChanged(
      MyGuiControlGrid myGuiControlGrid,
      MyGuiControlGrid.EventArgs eventArgs)
    {
      if (!this.m_gridBlocks.Visible || this.m_dragAndDrop.IsActive())
        return;
      this.ShowItem(this.m_gridBlocks.MouseOverItem ?? this.m_gridBlocks.SelectedItem);
    }

    private void ShowItem(MyGuiGridItem gridItem)
    {
      MyDefinitionBase definition;
      if (this.m_gamepadSlot.HasValue || gridItem == null || this.m_lastGridBlocksMouseOverItem == gridItem || (!((gridItem.UserData is MyGuiScreenToolbarConfigBase.GridItemUserData userData ? userData.ItemData() : (MyObjectBuilder_ToolbarItem) null) is MyObjectBuilder_ToolbarItemDefinition toolbarItemDefinition) || !MyDefinitionManager.Static.TryGetDefinition<MyDefinitionBase>((MyDefinitionId) toolbarItemDefinition.DefinitionId, out definition)))
        return;
      this.m_blockGroupInfo.Visible = true;
      this.m_lastGridBlocksMouseOverItem = gridItem;
      switch (definition)
      {
        case MyCubeBlockDefinition cubeBlockDefinition:
          this.m_blockGroupInfo.Visible = true;
          this.m_blockGroupInfo.SetBlockGroup(MyDefinitionManager.Static.GetDefinitionGroup(cubeBlockDefinition.BlockPairName));
          break;
        case MyPhysicalItemDefinition physicalItemDefinition:
          this.m_blockGroupInfo.SetGeneralDefinition((MyDefinitionBase) physicalItemDefinition);
          break;
        case MyAnimationDefinition animationDefinition:
          this.m_blockGroupInfo.SetGeneralDefinition((MyDefinitionBase) animationDefinition);
          break;
        case MyEmoteDefinition myEmoteDefinition:
          this.m_blockGroupInfo.SetGeneralDefinition((MyDefinitionBase) myEmoteDefinition);
          break;
        case MyVoxelHandDefinition voxelHandDefinition:
          this.m_blockGroupInfo.SetGeneralDefinition((MyDefinitionBase) voxelHandDefinition);
          break;
      }
    }
  }
}
