// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyGuiControlToolbar
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.GUI;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Input;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
  public class MyGuiControlToolbar : MyGuiControlBase
  {
    protected static StringBuilder m_textCache = new StringBuilder();
    protected MyGuiControlGrid m_toolbarItemsGrid;
    protected MyGuiControlLabel m_selectedItemLabel;
    protected MyGuiControlPanel m_colorVariantPanel;
    protected MyGuiControlPanel m_skinVariantPanel;
    protected MyGuiControlContextMenu m_contextMenu;
    protected List<MyGuiControlLabel> m_pageLabelList = new List<MyGuiControlLabel>();
    protected MyToolbar m_shownToolbar;
    protected MyObjectBuilder_ToolbarControlVisualStyle m_style;
    protected MyObjectBuilder_GuiTexture m_itemVarianTtexture;
    protected List<MyStatControls> m_statControls = new List<MyStatControls>();
    protected MyGuiCompositeTexture m_pageCompositeTexture;
    protected MyGuiCompositeTexture m_pageHighlightCompositeTexture;
    private bool m_gridSizeLargeBlock = true;
    protected int m_contextMenuItemIndex = -1;
    public bool UseContextMenu = true;
    public bool m_blockPlayerUseForShownToolbar;

    public MyToolbar ShownToolbar => this.m_shownToolbar;

    public MyGuiControlGrid ToolbarGrid => this.m_toolbarItemsGrid;

    public bool DrawNumbers => MyToolbarComponent.CurrentToolbar.DrawNumbers;

    public Func<int, ColoredIcon> GetSymbol => MyToolbarComponent.CurrentToolbar.GetSymbol;

    public MyGuiControlToolbar(
      MyObjectBuilder_ToolbarControlVisualStyle toolbarStyle,
      bool blockPlayerUseForShownToolbar)
      : base()
    {
      MyToolbarComponent.CurrentToolbarChanged += new Action<MyToolbar, MyToolbar>(this.ToolbarComponent_CurrentToolbarChanged);
      this.m_blockPlayerUseForShownToolbar = blockPlayerUseForShownToolbar;
      this.m_style = toolbarStyle;
      this.RecreateControls(true);
      this.ShowToolbar(MyToolbarComponent.CurrentToolbar);
      this.CanFocusChildren = true;
    }

    protected override void OnVisibleChanged()
    {
      base.OnVisibleChanged();
      MyToolbarComponent.IsToolbarControlShown = this.Visible;
    }

    public override void OnRemoving()
    {
      MyToolbarComponent.CurrentToolbarChanged -= new Action<MyToolbar, MyToolbar>(this.ToolbarComponent_CurrentToolbarChanged);
      if (this.m_shownToolbar != null)
      {
        this.m_shownToolbar.ItemChanged -= new Action<MyToolbar, MyToolbar.IndexArgs, bool>(this.Toolbar_ItemChanged);
        this.m_shownToolbar.ItemUpdated -= new Action<MyToolbar, MyToolbar.IndexArgs, MyToolbarItem.ChangeInfo>(this.Toolbar_ItemUpdated);
        this.m_shownToolbar.SelectedSlotChanged -= new Action<MyToolbar, MyToolbar.SlotArgs>(this.Toolbar_SelectedSlotChanged);
        this.m_shownToolbar.SlotActivated -= new Action<MyToolbar, MyToolbar.SlotArgs, bool>(this.Toolbar_SlotActivated);
        this.m_shownToolbar.ItemEnabledChanged -= new Action<MyToolbar, MyToolbar.SlotArgs>(this.Toolbar_ItemEnabledChanged);
        this.m_shownToolbar.CurrentPageChanged -= new Action<MyToolbar, MyToolbar.PageChangeArgs>(this.Toolbar_CurrentPageChanged);
        if (this.m_blockPlayerUseForShownToolbar)
          this.m_shownToolbar.CanPlayerActivateItems = true;
        this.m_shownToolbar = (MyToolbar) null;
      }
      MyToolbarComponent.IsToolbarControlShown = false;
      base.OnRemoving();
    }

    public override MyGuiControlBase HandleInput()
    {
      MyGuiControlBase myGuiControlBase = base.HandleInput() ?? this.HandleInputElements();
      if (this.UseContextMenu && MyInput.Static.IsMouseReleased(MyMouseButtonsEnum.Right) && this.m_contextMenu.Enabled)
      {
        this.m_contextMenu.ItemList_UseSimpleItemListMouseOverCheck = true;
        this.m_contextMenu.Enabled = false;
        this.m_contextMenu.Activate();
        MyGuiScreenBase myGuiScreenBase1 = (MyGuiScreenBase) null;
        IMyGuiControlsOwner guiControlsOwner = (IMyGuiControlsOwner) this.m_contextMenu;
        while (guiControlsOwner.Owner != null)
        {
          guiControlsOwner = guiControlsOwner.Owner;
          if (guiControlsOwner is MyGuiScreenBase myGuiScreenBase)
          {
            myGuiScreenBase1 = myGuiScreenBase;
            break;
          }
        }
        if (myGuiScreenBase1 != null)
          myGuiScreenBase1.FocusedControl = this.m_contextMenu.GetInnerList();
      }
      return myGuiControlBase;
    }

    public override void Update() => base.Update();

    public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
    {
      if (this.m_style.VisibleCondition != null && !this.m_style.VisibleCondition.Eval())
        return;
      this.m_colorVariantPanel.ColorMask = new Vector3(MyPlayer.SelectedColor.X, MathHelper.Clamp(MyPlayer.SelectedColor.Y + 0.8f, 0.0f, 1f), MathHelper.Clamp(MyPlayer.SelectedColor.Z + 0.55f, 0.0f, 1f)).HSVtoColor().ToVector4();
      if (!string.IsNullOrEmpty(MyPlayer.SelectedArmorSkin))
      {
        MyAssetModifierDefinition modifierDefinition = MyDefinitionManager.Static.GetAssetModifierDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_AssetModifierDefinition), MyPlayer.SelectedArmorSkin));
        if (modifierDefinition != null && !modifierDefinition.Icons.IsNullOrEmpty<string>())
          this.m_skinVariantPanel.BackgroundTexture = new MyGuiCompositeTexture(modifierDefinition.Icons[0]);
      }
      else
        this.m_skinVariantPanel.BackgroundTexture = (MyGuiCompositeTexture) null;
      this.m_statControls.ForEach((Action<MyStatControls>) (x => x.Draw(transitionAlpha, backgroundTransitionAlpha)));
      base.Draw(transitionAlpha, backgroundTransitionAlpha);
    }

    protected override void OnPositionChanged() => this.m_statControls.ForEach((Action<MyStatControls>) (x => x.Position = this.Position));

    protected override void OnSizeChanged()
    {
      this.RefreshInternals();
      base.OnSizeChanged();
    }

    private void RefreshInternals() => this.RepositionControls();

    private void RepositionControls()
    {
      this.m_toolbarItemsGrid.Position = this.Size * 0.5f;
      this.m_selectedItemLabel.Position = this.m_style.SelectedItemPosition;
      if (this.m_style.SelectedItemTextScale.HasValue)
        this.m_selectedItemLabel.TextScale = this.m_style.SelectedItemTextScale.Value;
      this.m_colorVariantPanel.Position = this.m_style.ColorPanelStyle.Offset;
      this.m_skinVariantPanel.Position = this.m_colorVariantPanel.Position + new Vector2(0.72f * this.m_colorVariantPanel.Size.X, 0.0f);
      Vector2 pagesOffset = this.m_style.PageStyle.PagesOffset;
      foreach (MyGuiControlLabel pageLabel in this.m_pageLabelList)
      {
        pageLabel.Position = pagesOffset + new Vector2(pageLabel.Size.X * 0.5f, (float) (-(double) pageLabel.Size.Y * 0.5));
        pagesOffset.X += pageLabel.Size.X + 1f / 1000f;
      }
      if (!this.UseContextMenu)
        return;
      this.Elements.Remove((MyGuiControlBase) this.m_contextMenu);
      this.Elements.Add((MyGuiControlBase) this.m_contextMenu);
    }

    private void RecreateControls(bool contructor)
    {
      if (this.m_style.VisibleCondition != null)
        this.InitStatConditions(this.m_style.VisibleCondition);
      MyGuiControlGrid myGuiControlGrid = new MyGuiControlGrid();
      myGuiControlGrid.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
      myGuiControlGrid.VisualStyle = MyGuiControlGridStyleEnum.Toolbar;
      myGuiControlGrid.ColumnsCount = MyToolbarComponent.CurrentToolbar.SlotCount + 1;
      myGuiControlGrid.RowsCount = 1;
      this.m_toolbarItemsGrid = myGuiControlGrid;
      this.m_toolbarItemsGrid.ItemDoubleClicked += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.grid_ItemDoubleClicked);
      this.m_toolbarItemsGrid.ItemClickedWithoutDoubleClick += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.grid_ItemClicked);
      this.m_toolbarItemsGrid.ItemAccepted += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.grid_ItemDoubleClicked);
      this.m_selectedItemLabel = new MyGuiControlLabel();
      this.m_colorVariantPanel = new MyGuiControlPanel(size: new Vector2?(this.m_style.ColorPanelStyle.Size));
      this.m_colorVariantPanel.BackgroundTexture = MyGuiConstants.TEXTURE_GUI_BLANK;
      this.m_skinVariantPanel = new MyGuiControlPanel(size: new Vector2?(new Vector2(this.m_style.ColorPanelStyle.Size.X * 0.66f) * new Vector2(0.75f, 1f)));
      this.m_skinVariantPanel.BackgroundTexture = MyGuiConstants.TEXTURE_GUI_BLANK;
      this.m_contextMenu = new MyGuiControlContextMenu();
      this.m_contextMenu.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
      this.m_contextMenu.Deactivate();
      this.m_contextMenu.ItemClicked += new Action<MyGuiControlContextMenu, MyGuiControlContextMenu.EventArgs>(this.contextMenu_ItemClicked);
      this.Elements.Add((MyGuiControlBase) this.m_colorVariantPanel);
      this.Elements.Add((MyGuiControlBase) this.m_skinVariantPanel);
      this.Elements.Add((MyGuiControlBase) this.m_selectedItemLabel);
      this.Elements.Add((MyGuiControlBase) this.m_toolbarItemsGrid);
      this.Elements.Add((MyGuiControlBase) this.m_contextMenu);
      this.m_colorVariantPanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
      this.m_skinVariantPanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
      this.m_selectedItemLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;
      this.m_toolbarItemsGrid.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
      this.m_contextMenu.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
      this.SetupToolbarStyle();
      this.RefreshInternals();
    }

    public bool IsToolbarGrid(MyGuiControlGrid grid) => this.m_toolbarItemsGrid == grid;

    public void ShowToolbar(MyToolbar toolbar)
    {
      if (this.m_shownToolbar != null)
      {
        this.m_shownToolbar.ItemChanged -= new Action<MyToolbar, MyToolbar.IndexArgs, bool>(this.Toolbar_ItemChanged);
        this.m_shownToolbar.ItemUpdated -= new Action<MyToolbar, MyToolbar.IndexArgs, MyToolbarItem.ChangeInfo>(this.Toolbar_ItemUpdated);
        this.m_shownToolbar.SelectedSlotChanged -= new Action<MyToolbar, MyToolbar.SlotArgs>(this.Toolbar_SelectedSlotChanged);
        this.m_shownToolbar.SlotActivated -= new Action<MyToolbar, MyToolbar.SlotArgs, bool>(this.Toolbar_SlotActivated);
        this.m_shownToolbar.ItemEnabledChanged -= new Action<MyToolbar, MyToolbar.SlotArgs>(this.Toolbar_ItemEnabledChanged);
        this.m_shownToolbar.CurrentPageChanged -= new Action<MyToolbar, MyToolbar.PageChangeArgs>(this.Toolbar_CurrentPageChanged);
        if (this.m_blockPlayerUseForShownToolbar)
          this.m_shownToolbar.CanPlayerActivateItems = true;
        foreach (MyGuiControlBase pageLabel in this.m_pageLabelList)
          this.Elements.Remove(pageLabel);
        this.m_pageLabelList.Clear();
      }
      this.m_shownToolbar = toolbar;
      if (this.m_shownToolbar == null)
      {
        this.m_toolbarItemsGrid.Enabled = false;
        this.m_toolbarItemsGrid.Visible = false;
      }
      else
      {
        int slotCount = toolbar.SlotCount;
        this.m_toolbarItemsGrid.ColumnsCount = slotCount + (toolbar.ShowHolsterSlot ? 1 : 0);
        for (int slot = 0; slot < slotCount; ++slot)
          this.SetGridItemAt(slot, toolbar.GetSlotItem(slot), true);
        this.m_selectedItemLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;
        this.m_colorVariantPanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
        this.m_colorVariantPanel.Visible = MyFakes.ENABLE_BLOCK_COLORING;
        this.m_skinVariantPanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
        this.m_skinVariantPanel.Visible = MyFakes.ENABLE_BLOCK_COLORING;
        if (toolbar.ShowHolsterSlot)
          this.SetGridItemAt(slotCount, (MyToolbarItem) new MyToolbarItemEmpty(), new string[1]
          {
            "Textures\\GUI\\Icons\\HideWeapon.dds"
          }, (string) null, MyTexts.GetString(MyCommonTexts.HideWeapon));
        if (toolbar.PageCount > 1)
        {
          for (int slotIndex = 0; slotIndex < toolbar.PageCount; ++slotIndex)
          {
            MyGuiControlToolbar.m_textCache.Clear();
            MyGuiControlToolbar.m_textCache.AppendInt32(slotIndex + 1);
            MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel(text: (MyToolbarComponent.GetSlotControlText(slotIndex).ToString() ?? MyGuiControlToolbar.m_textCache.ToString()));
            myGuiControlLabel1.BackgroundTexture = this.m_pageCompositeTexture;
            MyGuiControlLabel myGuiControlLabel2 = myGuiControlLabel1;
            float? numberSize = this.m_style.PageStyle.NumberSize;
            double num = numberSize.HasValue ? (double) numberSize.GetValueOrDefault() : 0.699999988079071;
            myGuiControlLabel2.TextScale = (float) num;
            myGuiControlLabel1.Size = this.m_toolbarItemsGrid.ItemSize * new Vector2(0.5f, 0.35f);
            myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
            this.m_pageLabelList.Add(myGuiControlLabel1);
            this.Elements.Add((MyGuiControlBase) myGuiControlLabel1);
          }
        }
        this.RepositionControls();
        this.HighlightCurrentPageLabel();
        this.RefreshSelectedItem(toolbar);
        this.m_shownToolbar.ItemChanged -= new Action<MyToolbar, MyToolbar.IndexArgs, bool>(this.Toolbar_ItemChanged);
        this.m_shownToolbar.ItemChanged += new Action<MyToolbar, MyToolbar.IndexArgs, bool>(this.Toolbar_ItemChanged);
        this.m_shownToolbar.ItemUpdated -= new Action<MyToolbar, MyToolbar.IndexArgs, MyToolbarItem.ChangeInfo>(this.Toolbar_ItemUpdated);
        this.m_shownToolbar.ItemUpdated += new Action<MyToolbar, MyToolbar.IndexArgs, MyToolbarItem.ChangeInfo>(this.Toolbar_ItemUpdated);
        this.m_shownToolbar.SelectedSlotChanged -= new Action<MyToolbar, MyToolbar.SlotArgs>(this.Toolbar_SelectedSlotChanged);
        this.m_shownToolbar.SelectedSlotChanged += new Action<MyToolbar, MyToolbar.SlotArgs>(this.Toolbar_SelectedSlotChanged);
        this.m_shownToolbar.SlotActivated -= new Action<MyToolbar, MyToolbar.SlotArgs, bool>(this.Toolbar_SlotActivated);
        this.m_shownToolbar.SlotActivated += new Action<MyToolbar, MyToolbar.SlotArgs, bool>(this.Toolbar_SlotActivated);
        this.m_shownToolbar.ItemEnabledChanged -= new Action<MyToolbar, MyToolbar.SlotArgs>(this.Toolbar_ItemEnabledChanged);
        this.m_shownToolbar.ItemEnabledChanged += new Action<MyToolbar, MyToolbar.SlotArgs>(this.Toolbar_ItemEnabledChanged);
        this.m_shownToolbar.CurrentPageChanged -= new Action<MyToolbar, MyToolbar.PageChangeArgs>(this.Toolbar_CurrentPageChanged);
        this.m_shownToolbar.CurrentPageChanged += new Action<MyToolbar, MyToolbar.PageChangeArgs>(this.Toolbar_CurrentPageChanged);
        if (this.m_blockPlayerUseForShownToolbar)
          this.m_shownToolbar.CanPlayerActivateItems = false;
        Vector2 vector2 = new Vector2(this.m_toolbarItemsGrid.Size.X, this.m_toolbarItemsGrid.Size.Y + this.m_selectedItemLabel.Size.Y + this.m_colorVariantPanel.Size.Y);
        this.MinSize = vector2;
        this.MaxSize = vector2;
        this.m_toolbarItemsGrid.Enabled = true;
        this.m_toolbarItemsGrid.Visible = true;
      }
    }

    private void SetupToolbarStyle()
    {
      MyGuiBorderThickness guiBorderThickness = !this.m_style.ItemStyle.Margin.HasValue ? new MyGuiBorderThickness(2f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 2f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y) : new MyGuiBorderThickness(this.m_style.ItemStyle.Margin.Value.Left, this.m_style.ItemStyle.Margin.Value.Right, this.m_style.ItemStyle.Margin.Value.Top, this.m_style.ItemStyle.Margin.Value.Botton);
      MyObjectBuilder_GuiTexture texture1 = MyGuiTextures.Static.GetTexture(this.m_style.ItemStyle.Texture);
      MyObjectBuilder_GuiTexture texture2 = MyGuiTextures.Static.GetTexture(this.m_style.ItemStyle.TextureHighlight);
      MyGuiTextures.Static.GetTexture(this.m_style.ItemStyle.TextureFocus);
      MyGuiTextures.Static.GetTexture(this.m_style.ItemStyle.TextureActive);
      Vector2 vector2 = new Vector2((float) texture1.SizePx.X * this.m_style.ItemStyle.ItemTextureScale.X, (float) texture1.SizePx.Y * this.m_style.ItemStyle.ItemTextureScale.Y);
      MyGuiHighlightTexture highlightTexture = new MyGuiHighlightTexture()
      {
        Normal = texture1.Path,
        Highlight = texture2.Path,
        Focus = texture2.Path,
        Active = texture2.Path,
        SizePx = vector2
      };
      this.m_toolbarItemsGrid.SetCustomStyleDefinition(new MyGuiStyleDefinition()
      {
        ItemTexture = highlightTexture,
        ItemFontNormal = this.m_style.ItemStyle.FontNormal,
        ItemFontHighlight = this.m_style.ItemStyle.FontHighlight,
        SizeOverride = new Vector2?(highlightTexture.SizeGui * new Vector2(10f, 1f)),
        ItemMargin = guiBorderThickness,
        ItemTextScale = this.m_style.ItemStyle.TextScale,
        FitSizeToItems = true
      });
      this.m_pageCompositeTexture = MyGuiCompositeTexture.CreateFromDefinition(this.m_style.PageStyle.PageCompositeTexture);
      this.m_pageHighlightCompositeTexture = MyGuiCompositeTexture.CreateFromDefinition(this.m_style.PageStyle.PageHighlightCompositeTexture);
      this.m_itemVarianTtexture = MyGuiTextures.Static.GetTexture(this.m_style.ItemStyle.VariantTexture);
      this.m_colorVariantPanel.BackgroundTexture = MyGuiCompositeTexture.CreateFromDefinition(this.m_style.ColorPanelStyle.Texture);
      this.m_skinVariantPanel.BackgroundTexture = MyGuiCompositeTexture.CreateFromDefinition(this.m_style.ColorPanelStyle.Texture);
      this.InitStatControls();
    }

    private void InitStatControls()
    {
      Rectangle fullscreenRectangle = MyGuiManager.GetFullscreenRectangle();
      Vector2 size = new Vector2((float) fullscreenRectangle.Width, (float) fullscreenRectangle.Height);
      if (this.m_style.StatControls == null)
        return;
      foreach (MyObjectBuilder_StatControls statControl in this.m_style.StatControls)
      {
        float uiScale = statControl.ApplyHudScale ? MyGuiManager.GetSafeScreenScale() * MyHud.HudElementsScaleMultiplier : MyGuiManager.GetSafeScreenScale();
        MyStatControls myStatControls = new MyStatControls(statControl, uiScale);
        Vector2 coordScreen = statControl.Position * size;
        myStatControls.Position = MyUtils.AlignCoord(coordScreen, size, statControl.OriginAlign);
        this.m_statControls.Add(myStatControls);
      }
    }

    private void RefreshSelectedItem(MyToolbar toolbar)
    {
      this.m_toolbarItemsGrid.SelectedIndex = toolbar.SelectedSlot;
      MyToolbarItem selectedItem = toolbar.SelectedItem;
      if (selectedItem != null)
      {
        this.m_selectedItemLabel.Text = selectedItem.DisplayName.ToString();
        this.m_colorVariantPanel.Visible = selectedItem is MyToolbarItemCubeBlock && MyFakes.ENABLE_BLOCK_COLORING;
        this.m_skinVariantPanel.Visible = this.m_colorVariantPanel.Visible;
      }
      else
      {
        this.m_colorVariantPanel.Visible = false;
        this.m_skinVariantPanel.Visible = false;
        this.m_selectedItemLabel.Text = string.Empty;
      }
    }

    private void HighlightCurrentPageLabel()
    {
      int currentPage = this.m_shownToolbar.CurrentPage;
      for (int index = 0; index < this.m_pageLabelList.Count; ++index)
      {
        if (index != currentPage && this.m_pageLabelList[index].BackgroundTexture == this.m_pageHighlightCompositeTexture)
          this.m_pageLabelList[index].BackgroundTexture = this.m_pageCompositeTexture;
        else if (index == currentPage && this.m_pageLabelList[index].BackgroundTexture == this.m_pageCompositeTexture)
          this.m_pageLabelList[index].BackgroundTexture = this.m_pageHighlightCompositeTexture;
      }
    }

    private void SetGridItemAt(int slot, MyToolbarItem item, bool clear = false)
    {
      if (item != null)
        this.SetGridItemAt(slot, item, item.Icons, item.SubIcon, item.DisplayName.ToString(), new ColoredIcon?(this.GetSymbol(slot)), clear);
      else
        this.SetGridItemAt(slot, (MyToolbarItem) null, (string[]) null, (string) null, (string) null, new ColoredIcon?(this.GetSymbol(slot)), clear);
    }

    protected virtual void SetGridItemAt(
      int slot,
      MyToolbarItem item,
      string[] icons,
      string subicon,
      string tooltip,
      ColoredIcon? symbol = null,
      bool clear = false)
    {
      MyGuiGridItem gridItem = this.m_toolbarItemsGrid.GetItemAt(slot);
      if (gridItem == null)
      {
        gridItem = new MyGuiGridItem(icons, subicon, tooltip, (object) item)
        {
          SubIconOffset = this.m_style.ItemStyle.VariantOffset
        };
        this.m_toolbarItemsGrid.SetItemAt(slot, gridItem);
      }
      else
      {
        gridItem.UserData = (object) item;
        gridItem.Icons = icons;
        gridItem.SubIcon = subicon;
        gridItem.SubIconOffset = this.m_style.ItemStyle.VariantOffset;
        if (gridItem.ToolTip == null)
          gridItem.ToolTip = new MyToolTips();
        gridItem.ToolTip.ToolTips.Clear();
        gridItem.ToolTip.AddToolTip(tooltip);
      }
      if (item == null | clear)
        gridItem.ClearAllText();
      if (this.DrawNumbers)
        gridItem.AddText(MyToolbarComponent.GetSlotControlText(slot));
      item?.FillGridItem(gridItem);
      gridItem.Enabled = item == null || item.Enabled;
      if (!symbol.HasValue)
        return;
      gridItem.AddIcon(symbol.Value, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
    }

    private void RemoveToolbarItem(int slot)
    {
      if (slot >= MyToolbarComponent.CurrentToolbar.SlotCount)
        return;
      MyToolbarComponent.CurrentToolbar.SetItemAtSlot(slot, (MyToolbarItem) null);
    }

    private void InitStatConditions(ConditionBase conditionBase)
    {
      switch (conditionBase)
      {
        case StatCondition statCondition:
          IMyHudStat stat = MyHud.Stats.GetStat(statCondition.StatId);
          statCondition.SetStat(stat);
          break;
        case Condition condition:
          foreach (ConditionBase term in condition.Terms)
            this.InitStatConditions(term);
          break;
      }
    }

    private void ToolbarComponent_CurrentToolbarChanged(MyToolbar old, MyToolbar current) => this.ShowToolbar(MyToolbarComponent.CurrentToolbar);

    private void Toolbar_SelectedSlotChanged(MyToolbar toolbar, MyToolbar.SlotArgs args) => this.RefreshSelectedItem(toolbar);

    private void Toolbar_SlotActivated(
      MyToolbar toolbar,
      MyToolbar.SlotArgs args,
      bool userActivated)
    {
      this.m_toolbarItemsGrid.blinkSlot(args.SlotNumber);
    }

    private void Toolbar_ItemChanged(MyToolbar toolbar, MyToolbar.IndexArgs args, bool isGamepad) => this.UpdateItemAtIndex(toolbar, args.ItemIndex);

    private void Toolbar_ItemUpdated(
      MyToolbar toolbar,
      MyToolbar.IndexArgs args,
      MyToolbarItem.ChangeInfo changes)
    {
      if (changes == MyToolbarItem.ChangeInfo.Icon)
        this.UpdateItemIcon(toolbar, args);
      else
        this.UpdateItemAtIndex(toolbar, args.ItemIndex);
    }

    private void UpdateItemAtIndex(MyToolbar toolbar, int index)
    {
      int slot = toolbar.IndexToSlot(index);
      if (!toolbar.IsValidIndex(index) || !toolbar.IsValidSlot(slot))
        return;
      this.SetGridItemAt(slot, toolbar[index], true);
      int? selectedSlot = toolbar.SelectedSlot;
      int num = slot;
      if (!(selectedSlot.GetValueOrDefault() == num & selectedSlot.HasValue))
        return;
      this.RefreshSelectedItem(toolbar);
    }

    private void Toolbar_ItemEnabledChanged(MyToolbar toolbar, MyToolbar.SlotArgs args)
    {
      if (args.SlotNumber.HasValue)
      {
        int num = args.SlotNumber.Value;
        MyGuiGridItem itemAt = this.m_toolbarItemsGrid.GetItemAt(num);
        if (itemAt == null)
          return;
        itemAt.Enabled = toolbar.IsEnabled(toolbar.SlotToIndex(num));
      }
      else
      {
        for (int index = 0; index < this.m_toolbarItemsGrid.ColumnsCount; ++index)
        {
          MyGuiGridItem itemAt = this.m_toolbarItemsGrid.GetItemAt(index);
          if (itemAt != null)
            itemAt.Enabled = toolbar.IsEnabled(toolbar.SlotToIndex(index));
        }
      }
    }

    private void UpdateItemIcon(MyToolbar toolbar, MyToolbar.IndexArgs args)
    {
      if (toolbar.IsValidIndex(args.ItemIndex))
      {
        int slot = toolbar.IndexToSlot(args.ItemIndex);
        if (slot == -1)
          return;
        MyGuiGridItem itemAt = this.m_toolbarItemsGrid.GetItemAt(slot);
        if (itemAt == null)
          return;
        itemAt.Icons = toolbar.GetItemIcons(args.ItemIndex);
      }
      else
      {
        for (int index = 0; index < this.m_toolbarItemsGrid.ColumnsCount; ++index)
        {
          MyGuiGridItem itemAt = this.m_toolbarItemsGrid.GetItemAt(index);
          if (itemAt != null)
            itemAt.Icons = toolbar.GetItemIcons(toolbar.SlotToIndex(index));
        }
      }
    }

    private void Toolbar_CurrentPageChanged(MyToolbar toolbar, MyToolbar.PageChangeArgs args)
    {
      if (this.UseContextMenu)
        this.m_contextMenu.Deactivate();
      this.HighlightCurrentPageLabel();
      for (int slot = 0; slot < MyToolbarComponent.CurrentToolbar.SlotCount; ++slot)
        this.SetGridItemAt(slot, toolbar.GetSlotItem(slot), true);
    }

    private void grid_ItemClicked(MyGuiControlGrid sender, MyGuiControlGrid.EventArgs eventArgs)
    {
      if (eventArgs.Button == MySharedButtonsEnum.Secondary)
      {
        int columnIndex = eventArgs.ColumnIndex;
        MyToolbar currentToolbar = MyToolbarComponent.CurrentToolbar;
        MyToolbarItem slotItem = currentToolbar.GetSlotItem(columnIndex);
        if (slotItem == null)
          return;
        if (slotItem is MyToolbarItemActions)
        {
          ListReader<ITerminalAction> listReader = (slotItem as MyToolbarItemActions).PossibleActions(this.ShownToolbar.ToolbarType);
          if (this.UseContextMenu && listReader.Count > 0)
          {
            this.m_contextMenu.CreateNewContextMenu();
            foreach (ITerminalAction terminalAction in listReader)
              this.m_contextMenu.AddItem(terminalAction.Name, "", terminalAction.Icon, (object) terminalAction.Id);
            this.m_contextMenu.AddItem(MyTexts.Get(MySpaceTexts.BlockAction_RemoveFromToolbar), "", "", (object) null);
            this.m_contextMenu.Enabled = true;
            this.m_contextMenuItemIndex = currentToolbar.SlotToIndex(columnIndex);
          }
          else
            this.RemoveToolbarItem(eventArgs.ColumnIndex);
        }
        else
          this.RemoveToolbarItem(eventArgs.ColumnIndex);
      }
      if (!this.m_shownToolbar.IsValidIndex(eventArgs.ColumnIndex))
        return;
      this.m_shownToolbar.ActivateItemAtSlot(eventArgs.ColumnIndex, true);
    }

    private void grid_ItemDoubleClicked(
      MyGuiControlGrid sender,
      MyGuiControlGrid.EventArgs eventArgs)
    {
      this.RemoveToolbarItem(eventArgs.ColumnIndex);
      if (!this.m_shownToolbar.IsValidIndex(eventArgs.ColumnIndex))
        return;
      this.m_shownToolbar.ActivateItemAtSlot(eventArgs.ColumnIndex);
    }

    private void contextMenu_ItemClicked(
      MyGuiControlContextMenu sender,
      MyGuiControlContextMenu.EventArgs args)
    {
      int itemIndex = args.ItemIndex;
      MyToolbar currentToolbar = MyToolbarComponent.CurrentToolbar;
      if (currentToolbar == null)
        return;
      int slot1 = currentToolbar.IndexToSlot(this.m_contextMenuItemIndex);
      if (currentToolbar.IsValidSlot(slot1))
      {
        MyToolbarItem slotItem1 = currentToolbar.GetSlotItem(slot1);
        MyToolbarItemActions toolbarItemActions = slotItem1 as MyToolbarItemActions;
        if (slotItem1 != null)
        {
          if (itemIndex < 0 || itemIndex >= toolbarItemActions.PossibleActions(this.ShownToolbar.ToolbarType).Count)
          {
            this.RemoveToolbarItem(slot1);
          }
          else
          {
            toolbarItemActions.ActionId = (string) args.UserData;
            for (int slot2 = 0; slot2 < MyToolbarComponent.CurrentToolbar.SlotCount; ++slot2)
            {
              MyToolbarItem slotItem2 = currentToolbar.GetSlotItem(slot2);
              if (slotItem2 != null && slotItem2.Equals((object) toolbarItemActions))
                MyToolbarComponent.CurrentToolbar.SetItemAtSlot(slot2, (MyToolbarItem) null);
            }
            MyToolbarComponent.CurrentToolbar.SetItemAtSlot(slot1, (MyToolbarItem) toolbarItemActions);
          }
        }
      }
      this.m_contextMenuItemIndex = -1;
    }

    public void HandleDragAndDrop(object sender, MyDragAndDropEventArgs eventArgs)
    {
      if (!(eventArgs.Item.UserData is MyToolbarItem userData))
        return;
      int itemIndex = MyToolbarComponent.CurrentToolbar.GetItemIndex(userData);
      if (eventArgs.DropTo != null && this.IsToolbarGrid(eventArgs.DropTo.Grid))
      {
        MyToolbarItem itemAtSlot = MyToolbarComponent.CurrentToolbar.GetItemAtSlot(eventArgs.DropTo.ItemIndex);
        int slot = MyToolbarComponent.CurrentToolbar.IndexToSlot(itemIndex);
        MyToolbarComponent.CurrentToolbar.SetItemAtSlot(eventArgs.DropTo.ItemIndex, userData);
        MyToolbarComponent.CurrentToolbar.SetItemAtSlot(slot, itemAtSlot);
      }
      else
        MyToolbarComponent.CurrentToolbar.SetItemAtIndex(itemIndex, (MyToolbarItem) null);
    }
  }
}
