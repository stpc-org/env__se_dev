// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyGuiControlBlockGroupInfo
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.GameServices;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
  [MyGuiControlType(typeof (MyObjectBuilder_GuiControlBlockGroupInfo))]
  public class MyGuiControlBlockGroupInfo : MyGuiControlStackPanel
  {
    private MyGuiControlLabel m_title;
    private MyGuiControlGrid m_blockVariantGrid;
    private MyGuiControlMultilineText m_helpText;
    private MyGuiControlBlockInfo m_componentsInfo;
    private MyGuiControlPanel m_helpTextBackground;
    private MyGuiControlPanel m_componentsBackground;
    private MyGuiControlButton m_blockTypeIconSmall;
    private MyGuiControlButton m_blockTypeIconLarge;
    private MyGuiControlGrid m_blocksBuildPlanner;
    private MyCubeSize m_userSizeChoice;
    private float m_blockNameOriginalOffset;
    private int m_rowsCount;
    private const float BLOCK_VARIANT_GRID_HEIGHT = 0.04f;

    public MyCubeBlockDefinition SelectedDefinition { get; private set; }

    public override void Init(MyObjectBuilder_GuiControlBase builder)
    {
      base.Init(builder);
      this.m_userSizeChoice = MyCubeBuilder.Static.CubeBuilderState.CubeSizeMode;
      this.m_title = new MyGuiControlLabel();
      this.m_title.Size = new Vector2(0.77f, 1f);
      MyGuiControlButton.StyleDefinition styleDefinition1 = new MyGuiControlButton.StyleDefinition()
      {
        NormalFont = "White",
        HighlightFont = "White",
        NormalTexture = MyGuiConstants.TEXTURE_HUD_GRID_LARGE_FIT,
        HighlightTexture = MyGuiConstants.TEXTURE_HUD_GRID_LARGE_FIT
      };
      MyGuiControlButton.StyleDefinition styleDefinition2 = new MyGuiControlButton.StyleDefinition()
      {
        NormalFont = "White",
        HighlightFont = "White",
        NormalTexture = MyGuiConstants.TEXTURE_HUD_GRID_SMALL_FIT,
        HighlightTexture = MyGuiConstants.TEXTURE_HUD_GRID_SMALL_FIT
      };
      this.m_blockTypeIconLarge = new MyGuiControlButton();
      this.m_blockTypeIconLarge.CustomStyle = styleDefinition1;
      this.m_blockTypeIconLarge.Size = new Vector2(0.0f, 0.7f);
      Thickness thickness = new Thickness(0.01f, 0.15f, 0.0f, 0.0f);
      this.m_blockTypeIconLarge.Margin = thickness;
      this.m_blockTypeIconSmall = new MyGuiControlButton();
      this.m_blockTypeIconSmall.CustomStyle = styleDefinition2;
      this.m_blockTypeIconSmall.Size = this.m_blockTypeIconLarge.Size;
      thickness.Left = 0.05f;
      this.m_blockTypeIconSmall.Margin = thickness;
      this.m_blockTypeIconSmall.ClickCallbackRespectsEnabledState = false;
      this.m_blockTypeIconLarge.ClickCallbackRespectsEnabledState = false;
      this.m_blockTypeIconLarge.ButtonClicked += new Action<MyGuiControlButton>(this.OnSizeSelectorClicked);
      this.m_blockTypeIconSmall.ButtonClicked += new Action<MyGuiControlButton>(this.OnSizeSelectorClicked);
      MyGuiControlStackPanel controlStackPanel = new MyGuiControlStackPanel();
      controlStackPanel.Orientation = MyGuiOrientation.Horizontal;
      controlStackPanel.Size = new Vector2(0.95f, 0.06f);
      controlStackPanel.Margin = new Thickness(0.025f, 0.0f, 0.0f, 0.0f);
      controlStackPanel.Add((MyGuiControlBase) this.m_title);
      controlStackPanel.Add((MyGuiControlBase) this.m_blockTypeIconSmall);
      controlStackPanel.Add((MyGuiControlBase) this.m_blockTypeIconLarge);
      this.Add((MyGuiControlBase) controlStackPanel);
      this.m_blockVariantGrid = new MyGuiControlGrid();
      this.m_blockVariantGrid.VisualStyle = MyGuiControlGridStyleEnum.BlockInfo;
      this.m_blockVariantGrid.RowsCount = 1;
      this.m_blockVariantGrid.ColumnsCount = 8;
      this.m_blockVariantGrid.Size = new Vector2(1f, 0.073f);
      this.m_blockVariantGrid.Margin = new Thickness(0.013f, 0.0f, 0.0f, 0.0f);
      this.m_blockVariantGrid.ItemSelected += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.OnBlockVariantSelected);
      this.m_blockVariantGrid.ItemClicked += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.OnBlockVariantGrid_ItemClicked);
      this.m_blockVariantGrid.ItemAccepted += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.OnBlockVariantGrid_ItemClicked);
      this.Add((MyGuiControlBase) this.m_blockVariantGrid);
      this.m_helpTextBackground = new MyGuiControlPanel();
      this.m_helpTextBackground.Size = new Vector2(0.95f, 0.23f);
      this.m_helpTextBackground.Margin = new Thickness(0.025f, 0.0f, 0.0f, 0.01f);
      this.m_helpTextBackground.ColorMask = new Vector4(0.1333333f, 0.1803922f, 0.2039216f, 0.9f);
      this.m_helpTextBackground.BackgroundTexture = new MyGuiCompositeTexture("Textures\\GUI\\Blank.dds");
      this.m_helpText = new MyGuiControlMultilineText(textScale: 0.64f, textBoxAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      this.m_helpText.Size = new Vector2(1f, 1f);
      this.m_helpText.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_helpText.OnLinkClicked += new LinkClicked(this.OnLinkClicked);
      this.Add((MyGuiControlBase) this.m_helpTextBackground);
      this.m_componentsBackground = new MyGuiControlPanel();
      this.m_componentsBackground.Size = new Vector2(0.95f, 0.484f);
      this.m_componentsBackground.Margin = new Thickness(0.025f, 0.0f, 0.0f, 0.0f);
      this.m_componentsBackground.ColorMask = new Vector4(0.1333333f, 0.1803922f, 0.2039216f, 0.9f);
      this.m_componentsBackground.BackgroundTexture = new MyGuiCompositeTexture("Textures\\GUI\\Blank.dds");
      this.Add((MyGuiControlBase) this.m_componentsBackground);
      this.m_componentsInfo = MyGuiControlBlockGroupInfo.CreateBlockInfoControl();
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel();
      myGuiControlLabel.Size = new Vector2(0.77f, 1f);
      myGuiControlLabel.Text = MyTexts.Get(MySpaceTexts.BuildPlanner).ToString();
      myGuiControlLabel.Margin = new Thickness(0.025f, 0.015f, 0.025f, 0.015f);
      this.Add((MyGuiControlBase) myGuiControlLabel);
      this.m_blocksBuildPlanner = new MyGuiControlGrid();
      this.m_blocksBuildPlanner.VisualStyle = MyGuiControlGridStyleEnum.BlockInfo;
      this.m_blocksBuildPlanner.RowsCount = 1;
      this.m_blocksBuildPlanner.ColumnsCount = 1;
      this.m_blocksBuildPlanner.Size = new Vector2(0.975f, 0.073f);
      this.m_blocksBuildPlanner.Margin = new Thickness(0.013f, 0.0f, 0.025f, 0.0f);
      this.m_blocksBuildPlanner.HighlightType = MyGuiControlHighlightType.WHEN_CURSOR_OVER;
      this.m_blocksBuildPlanner.ItemDoubleClicked += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.blocksToDo_ItemDoubleClicked);
      this.m_blocksBuildPlanner.ItemClicked += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.blocksToDo_ItemClicked);
      this.m_blocksBuildPlanner.ItemAccepted += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.blocksToDo_ItemClicked);
      this.m_blocksBuildPlanner.ItemBackgroundColorMask = new Vector4(0.8823529f, 0.9372549f, 1f, 1f);
      this.Add((MyGuiControlBase) this.m_blocksBuildPlanner);
      this.UpdateBuildPlanner();
      this.ForEachChild((Action<MyGuiControlStackPanel, MyGuiControlBase>) ((parent, control) =>
      {
        Vector2 size = parent.Size;
        Vector2 vector2 = control.Size * size;
        if ((double) vector2.X == 0.0)
          vector2.X = vector2.Y;
        else if ((double) vector2.Y == 0.0)
          vector2.Y = vector2.X;
        if (control is MyGuiControlButton)
          vector2 *= new Vector2(0.75f, 1f);
        control.Size = vector2;
        control.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
        Thickness margin = control.Margin;
        control.Margin = new Thickness(margin.Left * size.X, margin.Top * size.Y, margin.Right * size.X, margin.Bottom * size.Y);
      }));
    }

    private void OnLinkClicked(MyGuiControlBase sender, string url) => MyGameService.OpenInShop(url);

    private void OnBlockVariantGrid_ItemClicked(
      MyGuiControlGrid arg1,
      MyGuiControlGrid.EventArgs arg2)
    {
      if (arg2.Button != MySharedButtonsEnum.Ternary)
        return;
      this.m_blockVariantGrid.SelectedIndex = new int?(arg2.ItemIndex);
      if (this.SelectedDefinition == null || !MySession.Static.LocalCharacter.AddToBuildPlanner(this.SelectedDefinition))
        return;
      this.UpdateBuildPlanner();
      MyGuiAudio.PlaySound(MyGuiSounds.HudItem);
    }

    private void blocksToDo_ItemDoubleClicked(
      MyGuiControlGrid arg1,
      MyGuiControlGrid.EventArgs arg2)
    {
      MySession.Static.LocalCharacter.RemoveAtBuildPlanner(arg2.ColumnIndex);
      this.UpdateBuildPlanner();
    }

    private void blocksToDo_ItemClicked(MyGuiControlGrid arg1, MyGuiControlGrid.EventArgs arg2)
    {
      if (arg2.Button == MySharedButtonsEnum.Primary)
      {
        if (this.SelectedDefinition != null && arg2.ColumnIndex == MySession.Static.LocalCharacter.BuildPlanner.Count && MySession.Static.LocalCharacter.AddToBuildPlanner(this.SelectedDefinition))
        {
          this.UpdateBuildPlanner();
          MyGuiAudio.PlaySound(MyGuiSounds.HudItem);
        }
      }
      else if (arg2.Button == MySharedButtonsEnum.Secondary)
      {
        MySession.Static.LocalCharacter.RemoveAtBuildPlanner(arg2.ColumnIndex);
        MyGuiAudio.PlaySound(MyGuiSounds.HudItem);
      }
      this.UpdateBuildPlanner();
    }

    private void OnSizeSelectorClicked(MyGuiControlButton x) => this.OnUserSizePreferenceChanged(x == this.m_blockTypeIconLarge ? MyCubeSize.Large : MyCubeSize.Small);

    public void OnUserSizePreferenceChanged(MyCubeSize targetSize)
    {
      if (this.m_userSizeChoice == targetSize)
        return;
      MyGuiControlButton guiControlButton1 = targetSize == MyCubeSize.Small ? this.m_blockTypeIconSmall : this.m_blockTypeIconLarge;
      MyGuiControlButton guiControlButton2 = targetSize == MyCubeSize.Large ? this.m_blockTypeIconSmall : this.m_blockTypeIconLarge;
      if (!guiControlButton2.Visible)
        return;
      this.m_userSizeChoice = targetSize;
      guiControlButton1.Enabled = !guiControlButton1.Enabled;
      guiControlButton2.Enabled = !guiControlButton1.Enabled;
      this.RecreateDetail();
    }

    private void OnBlockVariantSelected(MyGuiControlGrid _, MyGuiControlGrid.EventArgs args) => this.RecreateDetail();

    public void SelectNextVariant()
    {
      int itemsCount = this.m_blockVariantGrid.GetItemsCount();
      int? nullable1 = this.m_blockVariantGrid.SelectedIndex;
      int? nullable2 = nullable1.HasValue ? new int?(nullable1.GetValueOrDefault() + 1) : new int?();
      int valueOrDefault = nullable2.GetValueOrDefault();
      if (itemsCount > valueOrDefault & nullable2.HasValue)
      {
        MyGuiControlGrid blockVariantGrid = this.m_blockVariantGrid;
        nullable2 = blockVariantGrid.SelectedIndex;
        int? nullable3;
        if (!nullable2.HasValue)
        {
          nullable1 = new int?();
          nullable3 = nullable1;
        }
        else
          nullable3 = new int?(nullable2.GetValueOrDefault() + 1);
        blockVariantGrid.SelectedIndex = nullable3;
      }
      else
        this.m_blockVariantGrid.SelectedIndex = new int?(0);
    }

    private void RecreateDetail()
    {
      MyGuiGridItem selectedItem = this.m_blockVariantGrid.SelectedItem;
      if (selectedItem == null)
        this.m_blockVariantGrid.SelectedIndex = new int?(0);
      else
        (selectedItem.UserData as MyGuiScreenToolbarConfigBase.GridItemUserData).Action();
    }

    public MyGuiGridItem GetSelectedVariant() => this.m_blockVariantGrid.SelectedItem;

    private void SetBlockDetail(MyCubeBlockDefinition[] definitions)
    {
      foreach (MyCubeBlockDefinition definition in definitions)
      {
        if (definition != null)
        {
          this.SetTexts((MyDefinitionBase) definition);
          this.m_helpText.ResetScrollBarValues();
          MyGuiControlButton guiControlButton = definition.CubeSize == MyCubeSize.Large ? this.m_blockTypeIconLarge : this.m_blockTypeIconSmall;
          if (guiControlButton.Enabled && guiControlButton.Visible)
          {
            this.SelectedDefinition = definition;
            this.m_componentsInfo.BlockInfo.LoadDefinition(definition);
            break;
          }
        }
      }
    }

    private void SetGeneralDefinitionDetail(MyDefinitionBase definition)
    {
      this.SelectedDefinition = definition as MyCubeBlockDefinition;
      this.SetTexts(definition);
    }

    private void SetTexts(MyDefinitionBase definition)
    {
      StringBuilder text = definition.DisplayNameEnum.HasValue ? MyTexts.Get(definition.DisplayNameEnum.Value) : new StringBuilder(definition.DisplayNameText);
      Vector2 vector2 = MyGuiManager.MeasureString(this.m_title.Font, text, 1f);
      float num = Math.Min(this.m_title.Size.X / vector2.X, 1f);
      Vector2 size = this.m_title.Size;
      this.m_title.TextToDraw = text;
      this.m_title.TextScale = num / MyGuiManager.LanguageTextScale;
      this.m_title.Size = size;
      this.m_title.PositionY = this.m_blockNameOriginalOffset + (float) (((double) size.Y - (double) vector2.Y * (double) this.m_title.TextScaleWithLanguage) / 2.0);
      this.m_helpTextBackground.Position = this.GetHelpTextBackgroundPosition();
      this.m_helpTextBackground.Size = this.GetHelpTextBackgroundSize();
      this.m_helpText.Position = this.GetHelpTextControlPosition();
      this.m_helpText.Size = this.GetHelpTextControlSize();
      this.m_helpText.Text = new StringBuilder();
      if (!string.IsNullOrWhiteSpace(definition.DescriptionText))
        this.m_helpText.AppendText(definition.DescriptionText);
      if (definition is MyCubeBlockDefinition && !MySession.Static.CreativeToolsEnabled(Sync.MyId) && !MySessionComponentResearch.Static.CanUse(MySession.Static.LocalPlayerId, definition.Id))
      {
        AppendSpacingIfNeeded();
        this.m_helpText.AppendText(MyTexts.Get(MySpaceTexts.NotificationBlockNotResearched));
      }
      else
      {
        MyDLCs.MyDLC missingDefinitionDlc = MySession.Static.GetComponent<MySessionComponentDLC>().GetFirstMissingDefinitionDLC(definition, Sync.MyId);
        if (missingDefinitionDlc != null)
        {
          AppendSpacingIfNeeded();
          this.m_helpText.AppendImage(missingDefinitionDlc.Icon, new Vector2(20f, 20f) / MyGuiConstants.GUI_OPTIMAL_SIZE, (Vector4) Color.White);
          this.m_helpText.AppendText("     ");
          this.m_helpText.AppendLink("app:" + (object) missingDefinitionDlc.AppId, MyDLCs.GetRequiredDLCStoreHint(missingDefinitionDlc.AppId));
        }
        else
        {
          MyGameInventoryItemDefinition inventoryItemDefinition = MyGameService.GetInventoryItemDefinition(definition.Id.SubtypeName);
          if (inventoryItemDefinition == null || !inventoryItemDefinition.CanBePurchased || inventoryItemDefinition.ItemSlot != MyGameInventoryItemSlot.Emote && inventoryItemDefinition.ItemSlot != MyGameInventoryItemSlot.Armor)
            return;
          AppendSpacingIfNeeded();
          this.m_helpText.AppendImage(MyGuiConstants.TEXTURE_ICON_FAKE.Texture, new Vector2(20f, 20f) / MyGuiConstants.GUI_OPTIMAL_SIZE, (Vector4) Color.White);
          this.m_helpText.AppendText("     ");
          this.m_helpText.AppendLink("item:" + (object) inventoryItemDefinition.ID, MyTexts.GetString(MyCommonTexts.ShowInGameInventoryStore));
        }
      }

      void AppendSpacingIfNeeded()
      {
        if (string.IsNullOrWhiteSpace(definition.DescriptionText))
          return;
        this.m_helpText.AppendText("\n");
        this.m_helpText.AppendText("\n");
      }
    }

    private Vector2 GetHelpTextBackgroundSize() => new Vector2(0.256f, (float) (0.157000005245209 - (double) (this.m_rowsCount - 1) * 0.0430000014603138));

    private Vector2 GetHelpTextBackgroundPosition() => new Vector2(this.m_helpTextBackground.Position.X, (float) ((double) this.m_blockVariantGrid.PositionY + (double) this.m_blockVariantGrid.Size.Y + (double) (this.m_rowsCount - 1) * 0.0430000014603138));

    private Vector2 GetHelpTextControlPosition(float margin = 0.05f) => this.m_helpTextBackground.Position + this.m_helpTextBackground.Size * margin;

    private Vector2 GetHelpTextControlSize(float margin = 0.05f) => this.m_helpTextBackground.Size * (float) (1.0 - (double) margin * 2.0);

    public override void UpdateArrange()
    {
      base.UpdateArrange();
      this.ForEachChild((Action<MyGuiControlStackPanel, MyGuiControlBase>) ((_, child) =>
      {
        if (!(child is MyGuiControlStackPanel controlStackPanel))
          return;
        controlStackPanel.UpdateArrange();
      }));
      this.m_helpText.Size = this.GetHelpTextControlSize();
      this.m_helpText.Position = this.GetHelpTextControlPosition();
      this.m_blockNameOriginalOffset = this.m_title.PositionY;
      this.m_componentsInfo.Size = this.m_componentsBackground.Size;
      this.m_componentsInfo.Position = this.m_componentsBackground.Position;
    }

    public void SetBlockGroup(MyCubeBlockDefinitionGroup group)
    {
      if (group == null || this.m_blockVariantGrid == null)
        return;
      MyCubeBlockDefinitionGroup blockDefinitionGroup1 = group;
      MyBlockVariantGroup blockVariantsGroup = group.AnyPublic?.BlockVariantsGroup;
      if (blockVariantsGroup != null && blockVariantsGroup.BlockGroups.Length != 0)
        group = blockVariantsGroup.BlockGroups[0];
      this.ClearGrid();
      this.SetBlockModeEnabled(true);
      HashSet<MyCubeBlockDefinitionGroup> blockGroups = new HashSet<MyCubeBlockDefinitionGroup>();
      if (blockDefinitionGroup1 == group)
      {
        if (blockVariantsGroup != null && blockVariantsGroup.Blocks != null)
        {
          foreach (MyCubeBlockDefinition block in blockVariantsGroup.Blocks)
          {
            string blockPairName = block.BlockPairName;
            if (blockPairName != null)
              blockGroups.Add(MyDefinitionManager.Static.GetDefinitionGroup(blockPairName));
          }
        }
        else
        {
          blockGroups.Add(group);
          AddStages(group.Small);
          AddStages(group.Large);
        }
      }
      else
        blockGroups.Add(blockDefinitionGroup1);
      int val2 = 8;
      this.m_blockVariantGrid.ColumnsCount = Math.Min(blockGroups.Count, val2);
      this.m_rowsCount = (int) Math.Ceiling((double) blockGroups.Count / (double) val2);
      int num1 = 0;
      int num2 = 0;
      foreach (MyCubeBlockDefinitionGroup blockDefinitionGroup2 in blockGroups)
      {
        if (blockDefinitionGroup2 != null)
        {
          MyCubeBlockDefinition small = blockDefinitionGroup2.Small;
          MyCubeBlockDefinition large = blockDefinitionGroup2.Large;
          if (this.m_userSizeChoice == MyCubeSize.Large)
            MyUtils.Swap<MyCubeBlockDefinition>(ref small, ref large);
          this.AddItemVariantDefinition(small, large, num1 / val2);
          if (blockDefinitionGroup2 == blockDefinitionGroup1)
            num2 = num1;
          ++num1;
        }
      }
      this.m_blockVariantGrid.SelectedIndex = new int?(num2);

      void AddStages(MyCubeBlockDefinition block)
      {
        if (block == null || block.BlockStages == null)
          return;
        foreach (MyDefinitionId blockStage in block.BlockStages)
        {
          MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(blockStage);
          if (cubeBlockDefinition != null)
          {
            MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(cubeBlockDefinition.BlockPairName);
            if (blockGroups.Add(definitionGroup))
            {
              AddStages(definitionGroup.Small);
              AddStages(definitionGroup.Large);
            }
          }
        }
      }
    }

    private void UpdateSizeIcons(bool smallExists, bool largeExists)
    {
      this.m_blockTypeIconSmall.Visible = smallExists;
      this.m_blockTypeIconLarge.Visible = largeExists;
      MyGuiControlButton blockTypeIconSmall = this.m_blockTypeIconSmall;
      MyGuiControlButton blockTypeIconLarge = this.m_blockTypeIconLarge;
      if (this.m_userSizeChoice == MyCubeSize.Large)
        MyUtils.Swap<MyGuiControlButton>(ref blockTypeIconSmall, ref blockTypeIconLarge);
      if (!blockTypeIconSmall.Visible)
        MyUtils.Swap<MyGuiControlButton>(ref blockTypeIconSmall, ref blockTypeIconLarge);
      blockTypeIconSmall.Enabled = true;
      blockTypeIconLarge.Enabled = false;
    }

    public void SetGeneralDefinition(MyDefinitionBase definition)
    {
      this.ClearGrid();
      this.SetBlockModeEnabled(false);
      this.m_blockVariantGrid.Add(new MyGuiGridItem(definition.Icons, (string) null, (string) null, (object) new MyGuiScreenToolbarConfigBase.GridItemUserData()
      {
        Action = (Action) (() => this.SetGeneralDefinitionDetail(definition)),
        ItemData = (Func<MyObjectBuilder_ToolbarItem>) (() => MyToolbarItemFactory.ObjectBuilderFromDefinition(definition))
      }, true, 1f));
      this.m_blockVariantGrid.SelectedIndex = new int?(0);
      this.m_blockVariantGrid.ColumnsCount = 1;
    }

    private void ClearGrid()
    {
      this.m_blockVariantGrid.SelectedIndex = new int?();
      this.m_blockVariantGrid.SetItemsToDefault();
    }

    public void SetBlockModeEnabled(bool enabled)
    {
      this.m_componentsInfo.Visible = enabled;
      this.m_blockTypeIconLarge.Visible = false;
      this.m_blockTypeIconSmall.Visible = false;
      this.m_componentsBackground.Visible = enabled;
    }

    private void AddItemVariantDefinition(
      MyCubeBlockDefinition primary,
      MyCubeBlockDefinition secondary,
      int row)
    {
      if (primary != null)
      {
        MyCubeBlockDefinition cubeBlockDefinition1 = secondary;
      }
      string str = (string) null;
      string[] icons = (string[]) null;
      if (MyGuiControlBlockGroupInfo.IsAllowed((MyDefinitionBase) primary))
      {
        icons = primary.Icons;
        str = GetSubIcon(primary);
      }
      else
        primary = (MyCubeBlockDefinition) null;
      if (MyGuiControlBlockGroupInfo.IsAllowed((MyDefinitionBase) secondary))
      {
        icons = secondary.Icons;
        str = GetSubIcon(secondary);
      }
      else
        secondary = (MyCubeBlockDefinition) null;
      if (primary == null && secondary == null)
        return;
      MyGuiControlGrid blockVariantGrid = this.m_blockVariantGrid;
      MyGuiGridItem myGuiGridItem = new MyGuiGridItem(icons, (string) null, (string) null, (object) new MyGuiScreenToolbarConfigBase.GridItemUserData()
      {
        Action = (Action) (() =>
        {
          bool smallExists = false;
          bool largeExists = false;
          MyCubeBlockDefinition[] definitions = new MyCubeBlockDefinition[2]
          {
            primary,
            secondary
          };
          foreach (MyCubeBlockDefinition cubeBlockDefinition2 in definitions)
          {
            if (cubeBlockDefinition2 != null)
            {
              if (cubeBlockDefinition2.CubeSize == MyCubeSize.Small)
                smallExists = true;
              else
                largeExists = true;
            }
          }
          this.UpdateSizeIcons(smallExists, largeExists);
          this.SetBlockDetail(definitions);
        }),
        ItemData = (Func<MyObjectBuilder_ToolbarItem>) (() => MyToolbarItemFactory.ObjectBuilderFromDefinition((MyDefinitionBase) this.SelectedDefinition))
      }, true, 1f);
      myGuiGridItem.SubIcon2 = str;
      myGuiGridItem.Enabled = string.IsNullOrEmpty(str);
      int startingRow = row;
      blockVariantGrid.Add(myGuiGridItem, startingRow);

      string GetSubIcon(MyCubeBlockDefinition block)
      {
        if (block == null)
          return (string) null;
        if (!MySession.Static.CreativeToolsEnabled(Sync.MyId) && !MySessionComponentResearch.Static.CanUse(MySession.Static.LocalPlayerId, block.Id))
          return "Textures\\GUI\\Icons\\HUD 2017\\ProgressionTree.png";
        return MySession.Static.GetComponent<MySessionComponentDLC>().GetFirstMissingDefinitionDLC((MyDefinitionBase) (primary ?? secondary), Sync.MyId)?.Icon;
      }
    }

    public void ForEachChild(
      Action<MyGuiControlStackPanel, MyGuiControlBase> action)
    {
      ForEachChildRecursive((MyGuiControlStackPanel) this);

      void ForEachChildRecursive(MyGuiControlStackPanel parent)
      {
        foreach (MyGuiControlBase control in parent.GetControls(false))
        {
          action(parent, control);
          if (control is MyGuiControlStackPanel parent)
            ForEachChildRecursive(parent);
        }
      }
    }

    private static bool IsAllowed(MyDefinitionBase blockDefinition) => blockDefinition != null && (blockDefinition.Public || MyFakes.ENABLE_NON_PUBLIC_BLOCKS) && (blockDefinition.AvailableInSurvival || !MySession.Static.SurvivalMode);

    private static MyGuiControlBlockInfo CreateBlockInfoControl()
    {
      MyGuiControlBlockInfo controlBlockInfo = new MyGuiControlBlockInfo(new MyGuiControlBlockInfo.MyControlBlockInfoStyle()
      {
        BackgroundColormask = new Vector4(0.1333333f, 0.1803922f, 0.2039216f, 1f),
        BlockNameLabelFont = "Blue",
        EnableBlockTypeLabel = false,
        ComponentsLabelText = MySpaceTexts.HudBlockInfo_Components,
        ComponentsLabelFont = "Blue",
        InstalledRequiredLabelText = MySpaceTexts.HudBlockInfo_Installed_Required,
        InstalledRequiredLabelFont = "Blue",
        RequiredLabelText = MyCommonTexts.HudBlockInfo_Required,
        IntegrityLabelFont = "White",
        IntegrityBackgroundColor = new Vector4(0.3058824f, 0.454902f, 0.5372549f, 1f),
        IntegrityForegroundColor = new Vector4(0.5f, 0.1f, 0.1f, 1f),
        IntegrityForegroundColorOverCritical = new Vector4(0.4627451f, 0.6509804f, 0.7529412f, 1f),
        LeftColumnBackgroundColor = new Vector4(0.0f, 0.0f, 1f, 0.0f),
        TitleBackgroundColor = new Vector4(0.2078431f, 0.2666667f, 0.2980392f, 1f),
        TitleSeparatorColor = new Vector4(0.4117647f, 0.4666667f, 0.5176471f, 1f),
        ComponentLineMissingFont = "Red",
        ComponentLineAllMountedFont = "White",
        ComponentLineAllInstalledFont = "Blue",
        ComponentLineDefaultFont = "Blue",
        ComponentLineDefaultColor = new Vector4(0.6f, 0.6f, 0.6f, 1f),
        ShowAvailableComponents = false,
        EnableBlockTypePanel = false,
        HiddenPCU = false,
        HiddenHeader = true
      }, false);
      controlBlockInfo.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlBlockInfo.BlockInfo = new MyHudBlockInfo();
      controlBlockInfo.BackgroundTexture = (MyGuiCompositeTexture) null;
      return controlBlockInfo;
    }

    public void RegisterAllControls(MyGuiControls controls)
    {
      this.ForEachChild((Action<MyGuiControlStackPanel, MyGuiControlBase>) ((_, x) =>
      {
        controls.Add(x);
        x.CanHaveFocus = false;
      }));
      controls.Add((MyGuiControlBase) this.m_helpText);
      controls.Add((MyGuiControlBase) this.m_componentsInfo);
    }

    protected override void OnVisibleChanged()
    {
      base.OnVisibleChanged();
      if (this.m_title == null)
        return;
      this.ForEachChild((Action<MyGuiControlStackPanel, MyGuiControlBase>) ((_, x) => x.Visible = this.Visible));
      this.m_helpText.Visible = this.Visible;
      this.m_componentsInfo.Visible = this.Visible;
    }

    public MyGuiControlGrid GetGridForDragAndDrop() => this.m_blockVariantGrid;

    public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
    {
      Vector2 positionAbsoluteTopLeft = this.GetPositionAbsoluteTopLeft();
      MyGuiConstants.TEXTURE_WBORDER_LIST.Draw(positionAbsoluteTopLeft, this.Size, MyGuiControlBase.ApplyColorMaskModifiers(this.ColorMask, this.Enabled, backgroundTransitionAlpha));
      base.Draw(transitionAlpha, backgroundTransitionAlpha);
    }

    public bool IsBuildPlannerGrid(MyGuiControlGrid grid) => this.m_blocksBuildPlanner == grid;

    public void UpdateBuildPlanner()
    {
      this.m_blocksBuildPlanner.Items.Clear();
      if (MySession.Static == null || MySession.Static.LocalCharacter == null || MySession.Static.LocalCharacter.BuildPlanner == null)
        return;
      foreach (MyIdentity.BuildPlanItem buildPlanItem in (IEnumerable<MyIdentity.BuildPlanItem>) MySession.Static.LocalCharacter.BuildPlanner)
      {
        if (buildPlanItem.BlockDefinition != null)
        {
          MyToolTips myToolTips1 = new MyToolTips();
          myToolTips1.AddToolTip(buildPlanItem.BlockDefinition.DisplayNameText, 0.8f);
          string icon = buildPlanItem.BlockDefinition.Icons[0];
          MyToolTips myToolTips2 = myToolTips1;
          object obj = (object) buildPlanItem;
          string subicon = buildPlanItem.BlockDefinition.CubeSize == MyCubeSize.Large ? MyGuiConstants.TEXTURE_HUD_GRID_LARGE_FIT.Center.Texture : MyGuiConstants.TEXTURE_HUD_GRID_SMALL_FIT.Center.Texture;
          MyToolTips toolTips = myToolTips2;
          object userData = obj;
          MyGuiGridItem myGuiGridItem = new MyGuiGridItem(icon, subicon, toolTips, userData);
          if (buildPlanItem.IsInProgress)
            myGuiGridItem.OverlayPercent = buildPlanItem.IsInProgress ? 0.5f : 0.0f;
          foreach (MyIdentity.BuildPlanItem.Component component in buildPlanItem.Components)
            myGuiGridItem.ToolTip.AddToolTip(component.Count.ToString() + "x " + component.ComponentDefinition.DisplayNameText);
          this.m_blocksBuildPlanner.Items.Add(myGuiGridItem);
        }
      }
      this.m_blocksBuildPlanner.Items.Add(new MyGuiGridItem(new string[1]
      {
        "Textures\\GUI\\Controls\\button_increase.dds"
      }, (string) null, MyTexts.Get(MySpaceTexts.TooltipBuildScreen_BuildPlanner).ToString(), (object) null, true, 1f));
      this.m_blocksBuildPlanner.ColumnsCount = Math.Min(MySession.Static.LocalCharacter.BuildPlanner.Count + 1, 8);
    }
  }
}
