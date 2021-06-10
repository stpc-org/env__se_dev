// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenColorPicker
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.GameServices;
using VRage.Input;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  internal class MyGuiScreenColorPicker : MyGuiScreenBase
  {
    private static readonly Vector2 SCREEN_SIZE = new Vector2(0.37f, 1.2f);
    private static readonly float HIDDEN_PART_RIGHT = 0.04f;
    private float m_textScale = 0.8f;
    private Vector2 m_controlPadding = new Vector2(0.02f, 0.02f);
    private new MyGuiControlButton m_closeButton;
    private MyGuiControlButton m_okButton;
    private MyGuiControlButton m_defaultsButton;
    private MyGuiControlSlider m_hueSlider;
    private MyGuiControlSlider m_saturationSlider;
    private MyGuiControlSlider m_valueSlider;
    private MyGuiControlTextbox m_hueTextbox;
    private MyGuiControlTextbox m_saturationTextbox;
    private MyGuiControlTextbox m_valueTextbox;
    private MyGuiControlTextbox m_hexTextbox;
    private MyGuiControlPanel m_colorVariantPanel;
    private MyGuiControlPanel m_highlightControlPanel;
    private List<MyGuiControlPanel> m_colorPaletteControlsList = new List<MyGuiControlPanel>();
    private List<MyGameInventoryItemDefinition> m_userItems;
    private MyGuiControlGrid m_itemGrid;
    private List<Vector3> m_oldPaletteList;
    private MyStringHash m_oldArmorSkin;
    private int m_oldColorSlot;
    private bool m_oldApplyColor;
    private bool m_oldApplySkin;
    private MyGuiStyleDefinition m_armorGridStyle = new MyGuiStyleDefinition()
    {
      BackgroundTexture = (MyGuiCompositeTexture) null,
      BackgroundPaddingSize = Vector2.Zero,
      ItemTexture = MyGuiConstants.TEXTURE_GRID_ITEM_WHITE,
      ItemFontNormal = "Blue",
      ItemFontHighlight = "White",
      ItemPadding = new MyGuiBorderThickness(4f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y),
      ItemMargin = new MyGuiBorderThickness(4f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y),
      FitSizeToItems = true
    };
    private static readonly Regex HEX_REGEX = new Regex("^(#{0,1})([0-9A-Fa-f]{6})$");
    private readonly StringBuilder m_hexSb = new StringBuilder();
    private MySessionComponentGameInventory m_gameInventoryComp;
    private MySessionComponentDLC m_dlcComp;
    private MyGuiControlCheckbox m_applyColorCheckbox;
    private MyGuiControlCheckbox m_applySkinCheckbox;
    private const int x = -170;
    private const int y = -250;
    private const int defColLine = -230;
    private const int defColCol = -42;
    private const string m_hueScaleTexture = "Textures\\GUI\\HueScale.png";

    public static bool ApplyColor { get; private set; } = true;

    public static bool ApplySkin { get; private set; } = true;

    public MyGuiScreenColorPicker()
      : base(new Vector2?(new Vector2(MyGuiManager.GetMaxMouseCoord().X - MyGuiScreenColorPicker.SCREEN_SIZE.X * 0.5f + MyGuiScreenColorPicker.HIDDEN_PART_RIGHT, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR * MySandboxGame.Config.UIBkOpacity), new Vector2?(MyGuiScreenColorPicker.SCREEN_SIZE))
    {
      this.CanHideOthers = false;
      this.m_dlcComp = MySession.Static?.GetComponent<MySessionComponentDLC>();
      this.m_gameInventoryComp = MySession.Static?.GetComponent<MySessionComponentGameInventory>();
      this.RecreateControls(true);
      this.m_oldPaletteList = new List<Vector3>();
      foreach (Vector3 buildColorSlot in MySession.Static.LocalHumanPlayer.BuildColorSlots)
        this.m_oldPaletteList.Add(buildColorSlot);
      this.m_oldArmorSkin = MyStringHash.GetOrCompute(MySession.Static.LocalHumanPlayer.BuildArmorSkin);
      this.m_oldColorSlot = MySession.Static.LocalHumanPlayer.SelectedBuildColorSlot;
      this.m_oldApplySkin = MyGuiScreenColorPicker.ApplySkin;
      this.m_oldApplyColor = MyGuiScreenColorPicker.ApplyColor;
      this.UpdateSliders(MyPlayer.SelectedColor);
      this.UpdateLabels();
      if (MyGuiScreenHudSpace.Static == null)
        return;
      MyGuiScreenHudSpace.Static.HideScreen();
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      float num1 = (float) (((double) MyGuiScreenColorPicker.SCREEN_SIZE.Y - 1.0) / 2.0);
      this.AddCaption(MyTexts.Get(MyCommonTexts.ColorPicker).ToString(), new Vector4?(Color.White.ToVector4()), new Vector2?(this.m_controlPadding + new Vector2(-MyGuiScreenColorPicker.HIDDEN_PART_RIGHT, num1 - 0.03f)));
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), 0.44f), this.m_size.Value.X * 0.73f);
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), 0.316f), this.m_size.Value.X * 0.73f);
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), 0.055f), this.m_size.Value.X * 0.73f);
      Vector2 start = new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), -0.362f);
      controlSeparatorList.AddHorizontal(start, this.m_size.Value.X * 0.73f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList);
      Vector2? position = new Vector2?(new Vector2((float) (-(double) this.m_size.Value.X * 0.829999983310699 / 2.0), -0.4166667f));
      Vector2? size1 = new Vector2?();
      Vector2? size2 = size1;
      string text1 = MyTexts.Get(MyCommonTexts.ApplyColor).ToString();
      Vector4? colorMask = new Vector4?();
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(position, size2, text1, colorMask));
      this.m_applyColorCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2((float) (-(double) this.m_size.Value.X * 0.829999983310699 / 2.0 + (double) this.m_size.Value.X * 0.730000019073486), -0.4333333f)), isChecked: MyGuiScreenColorPicker.ApplyColor, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP)
      {
        IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.OnColorCheckedChanged)
      };
      this.m_applyColorCheckbox.SetTooltip(MyTexts.Get(MyCommonTexts.ApplyColorTooltip).ToString());
      this.Controls.Add((MyGuiControlBase) this.m_applyColorCheckbox);
      Color white = Color.White;
      int num2 = 0;
      size1 = new Vector2?(new Vector2(0.04f, 0.035f));
      this.m_highlightControlPanel = new MyGuiControlPanel(new Vector2?(new Vector2((float) ((double) (MyPlayer.SelectedColorSlot % 7) * 0.0379999987781048 - 0.132499992847443), (float) ((double) (MyPlayer.SelectedColorSlot / 7) * 0.0350000001490116 - 0.375))), size1);
      this.m_highlightControlPanel.ColorMask = white.ToVector4();
      this.m_highlightControlPanel.BackgroundTexture = MyGuiConstants.TEXTURE_GUI_BLANK;
      this.Controls.Add((MyGuiControlBase) this.m_highlightControlPanel);
      int index1 = 0;
      while (index1 < 14)
      {
        size1 = new Vector2?(new Vector2(0.034f, 0.03f));
        MyGuiControlPanel myGuiControlPanel = new MyGuiControlPanel(new Vector2?(new Vector2((float) ((double) (index1 % 7) * 0.0379999987781048 - 0.132499992847443), (float) ((double) num2 * 0.0350000001490116 - 0.375))), size1);
        myGuiControlPanel.ColorMask = this.prev(MyPlayer.ColorSlots.ItemAt(index1)).HSVtoColor().ToVector4();
        myGuiControlPanel.BackgroundTexture = MyGuiConstants.TEXTURE_GUI_BLANK;
        myGuiControlPanel.CanHaveFocus = true;
        myGuiControlPanel.BorderHighlightEnabled = true;
        myGuiControlPanel.BorderMargin = new Vector2(0.034f, 0.031f);
        myGuiControlPanel.BorderSize = 2;
        myGuiControlPanel.HighlightType = MyGuiControlHighlightType.WHEN_CURSOR_OVER_OR_FOCUS;
        this.m_colorPaletteControlsList.Add(myGuiControlPanel);
        this.Controls.Add((MyGuiControlBase) myGuiControlPanel);
        ++index1;
        if (index1 % 7 == 0)
          ++num2;
      }
      float x1 = -0.153125f;
      float x2 = -0.13625f;
      float x3 = 0.11375f;
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(x1, -0.275f)), text: "H:"));
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(x3, -0.275f)), text: "°", originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER));
      MyGuiControlTextbox guiControlTextbox1 = new MyGuiControlTextbox();
      guiControlTextbox1.Position = new Vector2(x3 - 0.0125f, -0.2783333f);
      guiControlTextbox1.Size = new Vector2(0.053f, 0.04583333f);
      guiControlTextbox1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      guiControlTextbox1.Type = MyGuiControlTextboxType.DigitsOnly;
      guiControlTextbox1.TruncateDecimalDigits = false;
      this.m_hueTextbox = guiControlTextbox1;
      this.m_hueSlider = new MyGuiControlSlider(new Vector2?(new Vector2(x2, -0.275f)), maxValue: 360f, width: 0.18f, labelText: string.Empty, labelDecimalPlaces: 0, visualStyle: MyGuiControlSliderStyleEnum.Hue, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      this.m_hueSlider.ValueChanged += new Action<MyGuiControlSlider>(this.OnValueChange);
      this.Controls.Add((MyGuiControlBase) this.m_hueSlider);
      this.m_hueTextbox.MaxLength = 5;
      this.m_hueTextbox.EnterPressed += new Action<MyGuiControlTextbox>(this.HueTextboxOnEnterPressed);
      this.m_hueTextbox.FocusChanged += new Action<MyGuiControlBase, bool>(this.HueTextboxOnFocusChanged);
      this.Controls.Add((MyGuiControlBase) this.m_hueTextbox);
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(x1, -0.2166667f)), text: "S:"));
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(x3, -0.2166667f)), text: "%", originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER));
      MyGuiControlTextbox guiControlTextbox2 = new MyGuiControlTextbox();
      guiControlTextbox2.Position = new Vector2(x3 - 0.0125f, -0.22f);
      guiControlTextbox2.Size = new Vector2(0.053f, 0.232f);
      guiControlTextbox2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      guiControlTextbox2.Type = MyGuiControlTextboxType.DigitsOnly;
      guiControlTextbox2.TruncateDecimalDigits = false;
      this.m_saturationTextbox = guiControlTextbox2;
      this.m_saturationSlider = new MyGuiControlSlider(new Vector2?(new Vector2(x2, -0.2166667f)), width: 0.18f, defaultValue: new float?(0.0f), labelText: string.Empty, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      this.m_saturationSlider.ValueChanged += new Action<MyGuiControlSlider>(this.OnValueChange);
      this.Controls.Add((MyGuiControlBase) this.m_saturationSlider);
      this.m_saturationTextbox.MaxLength = 5;
      this.m_saturationTextbox.EnterPressed += new Action<MyGuiControlTextbox>(this.SaturationTextboxOnEnterPressed);
      this.m_saturationTextbox.FocusChanged += new Action<MyGuiControlBase, bool>(this.SaturationTextboxOnFocusChanged);
      this.Controls.Add((MyGuiControlBase) this.m_saturationTextbox);
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(x1, -0.1583333f)), text: "V:"));
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(x3, -0.1583333f)), text: "%", originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER));
      MyGuiControlTextbox guiControlTextbox3 = new MyGuiControlTextbox();
      guiControlTextbox3.Position = new Vector2(x3 - 0.0125f, -0.1616667f);
      guiControlTextbox3.Size = new Vector2(0.053f, 0.232f);
      guiControlTextbox3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      guiControlTextbox3.Type = MyGuiControlTextboxType.DigitsOnly;
      guiControlTextbox3.TruncateDecimalDigits = false;
      this.m_valueTextbox = guiControlTextbox3;
      this.m_valueSlider = new MyGuiControlSlider(new Vector2?(new Vector2(x2, -0.1583333f)), width: 0.18f, defaultValue: new float?(0.0f), labelText: string.Empty, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      this.m_valueSlider.ValueChanged += new Action<MyGuiControlSlider>(this.OnValueChange);
      this.Controls.Add((MyGuiControlBase) this.m_valueSlider);
      this.m_valueTextbox.MaxLength = 5;
      this.m_valueTextbox.EnterPressed += new Action<MyGuiControlTextbox>(this.ValueTextboxOnEnterPressed);
      this.m_valueTextbox.FocusChanged += new Action<MyGuiControlBase, bool>(this.ValueTextboxOnFocusChanged);
      this.Controls.Add((MyGuiControlBase) this.m_valueTextbox);
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(x1, -0.1f)), text: "Hex:"));
      MyGuiControlTextbox guiControlTextbox4 = new MyGuiControlTextbox();
      guiControlTextbox4.Position = new Vector2(x3 - 0.0125f, -0.1f);
      guiControlTextbox4.Size = new Vector2(0.222f, 0.232f);
      guiControlTextbox4.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      this.m_hexTextbox = guiControlTextbox4;
      this.m_hexTextbox.MaxLength = 7;
      this.Controls.Add((MyGuiControlBase) this.m_hexTextbox);
      this.m_hexSb.Clear();
      this.m_hexTextbox.SetText(this.m_hexSb);
      this.m_hexTextbox.EnterPressed += new Action<MyGuiControlTextbox>(this.HexTextboxOnEnterPressed);
      this.m_hexTextbox.FocusChanged += new Action<MyGuiControlBase, bool>(this.HexTextboxOnFocusChanged);
      this.m_userItems = this.GetInventoryItems();
      this.m_itemGrid = new MyGuiControlGrid()
      {
        ColumnsCount = 5,
        RowsCount = 5
      };
      this.m_itemGrid.SetCustomStyleDefinition(this.m_armorGridStyle);
      this.m_itemGrid.ItemClicked += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.ItemGridOnItemClicked);
      this.m_itemGrid.ItemAccepted += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.ItemGridOnItemClicked);
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(-0.15f, -0.02916667f)), text: MyTexts.Get(MyCommonTexts.ApplySkin).ToString()));
      this.m_applySkinCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(0.125f, -0.04833333f)), isChecked: MyGuiScreenColorPicker.ApplySkin, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP)
      {
        IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.OnSkinCheckedChanged)
      };
      this.m_applySkinCheckbox.SetTooltip(MyTexts.Get(MyCommonTexts.ApplySkinTooltip).ToString());
      this.Controls.Add((MyGuiControlBase) this.m_applySkinCheckbox);
      MyGuiControlScrollablePanel controlScrollablePanel = new MyGuiControlScrollablePanel((MyGuiControlBase) this.m_itemGrid);
      controlScrollablePanel.Position = new Vector2(-0.15f, -1f / 300f);
      controlScrollablePanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlScrollablePanel.DrawScrollBarSeparator = true;
      controlScrollablePanel.FitSizeToScrolledControl();
      this.Controls.Add((MyGuiControlBase) controlScrollablePanel);
      this.m_itemGrid.Add(new MyGuiGridItem("Textures\\Sprites\\Cross.dds", (string) null, "None", (object) string.Empty, true, 1f));
      int num3 = 0;
      int num4 = 0;
      for (int index2 = 0; index2 < this.m_userItems.Count; ++index2)
      {
        MyGameInventoryItemDefinition userItem = this.m_userItems[index2];
        MyAssetModifierDefinition modifierDefinition = MyDefinitionManager.Static.GetAssetModifierDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_AssetModifierDefinition), userItem.AssetModifierId));
        if (modifierDefinition != null)
        {
          this.m_itemGrid.Add(this.CreateArmorGridItem(userItem, modifierDefinition));
          ++num4;
          if (userItem.AssetModifierId == MyPlayer.SelectedArmorSkin)
            num3 = num4;
        }
      }
      this.m_itemGrid.SelectedIndex = new int?(num3);
      Vector2 vector2_1 = new Vector2(-0.083f, 0.36f);
      Vector2 vector2_2 = new Vector2(0.134f, 0.038f);
      float num5 = 0.265f;
      double num6 = (double) num5;
      StringBuilder text2 = MyTexts.Get(MyCommonTexts.Defaults);
      Action<MyGuiControlButton> onClick1 = new Action<MyGuiControlButton>(this.OnDefaultsClick);
      float textScale1 = this.m_textScale;
      MyStringId? tooltip1 = new MyStringId?(MySpaceTexts.ToolTipOptionsControls_Defaults);
      double num7 = (double) textScale1;
      this.m_defaultsButton = this.CreateButton((float) num6, text2, onClick1, tooltip: tooltip1, textScale: ((float) num7));
      this.m_defaultsButton.Position = vector2_1 + new Vector2(0.0f, 1f) * vector2_2;
      this.m_defaultsButton.PositionX += vector2_2.X / 2f;
      this.Controls.Add((MyGuiControlBase) this.m_defaultsButton);
      double num8 = (double) num5;
      StringBuilder text3 = MyTexts.Get(MyCommonTexts.Ok);
      Action<MyGuiControlButton> onClick2 = new Action<MyGuiControlButton>(this.OnOkClick);
      float textScale2 = this.m_textScale;
      MyStringId? tooltip2 = new MyStringId?(MySpaceTexts.ToolTipOptionsSpace_Ok);
      double num9 = (double) textScale2;
      this.m_okButton = this.CreateButton((float) num8, text3, onClick2, tooltip: tooltip2, textScale: ((float) num9));
      this.m_okButton.Position = vector2_1 + new Vector2(vector2_2.X / 2f, vector2_2.Y * 2f);
      this.m_okButton.ShowTooltipWhenDisabled = true;
      this.Controls.Add((MyGuiControlBase) this.m_okButton);
      Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(new Vector2(start.X, this.m_okButton.Position.Y - minSizeGui.Y / 2f)));
      myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.ColorTool_Help_Screen);
      this.FocusedControl = (MyGuiControlBase) this.m_highlightControlPanel;
    }

    private void HueTextboxOnFocusChanged(MyGuiControlBase obj, bool state)
    {
      if (state || !(obj is MyGuiControlTextbox guiControlTextbox))
        return;
      this.HueTextboxOnEnterPressed(guiControlTextbox);
    }

    private MyGuiGridItem CreateArmorGridItem(
      MyGameInventoryItemDefinition def,
      MyAssetModifierDefinition definition)
    {
      StringBuilder stringBuilder = new StringBuilder(def.Name);
      string icon = def.IconTexture ?? MyGuiConstants.TEXTURE_ICON_FAKE.Texture;
      if (!definition.Icons.IsNullOrEmpty<string>())
        icon = definition.Icons[0];
      bool enabled = this.m_gameInventoryComp != null && this.m_gameInventoryComp.HasArmor(MyStringHash.GetOrCompute(def.AssetModifierId), Sync.MyId);
      string str = (string) null;
      if (!definition.DLCs.IsNullOrEmpty<string>())
      {
        MyDLCs.MyDLC missingDefinitionDlc = this.m_dlcComp.GetFirstMissingDefinitionDLC((MyDefinitionBase) definition, Sync.MyId);
        if (missingDefinitionDlc != null)
        {
          enabled = false;
          str = missingDefinitionDlc.Icon;
          stringBuilder.Append("\n");
          for (int index = 0; index < definition.DLCs.Length; ++index)
          {
            stringBuilder.Append("\n");
            stringBuilder.Append(MyDLCs.GetRequiredDLCTooltip(definition.DLCs[index]));
          }
        }
        else
        {
          MyDLCs.MyDLC dlc;
          if (MyDLCs.TryGetDLC(definition.DLCs[0], out dlc))
            str = dlc.Icon;
        }
      }
      else if (!enabled)
      {
        str = MyGuiConstants.TEXTURE_ICON_FAKE.Texture;
        stringBuilder.Append("\n");
        stringBuilder.Append("\n");
        stringBuilder.AppendFormat(MyTexts.GetString(MyCommonTexts.RequiresGameInventoryItem), (object) def.Name, (object) MySession.GameServiceName);
      }
      Vector4 one = Vector4.One;
      if (!enabled)
        one.W = 0.5f;
      return new MyGuiGridItem(icon, (string) null, stringBuilder.ToString(), (object) def.AssetModifierId, enabled, 0.85f)
      {
        SubIcon2 = str,
        MainIconColorMask = one
      };
    }

    private void HueTextboxOnEnterPressed(MyGuiControlTextbox obj)
    {
      float result;
      if (float.TryParse(obj.Text, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result))
        this.m_hueSlider.Value = result;
      this.UpdateLabels();
    }

    private void SaturationTextboxOnFocusChanged(MyGuiControlBase obj, bool state)
    {
      if (state || !(obj is MyGuiControlTextbox guiControlTextbox))
        return;
      this.SaturationTextboxOnEnterPressed(guiControlTextbox);
    }

    private void SaturationTextboxOnEnterPressed(MyGuiControlTextbox obj)
    {
      float result;
      if (float.TryParse(obj.Text, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result))
        this.m_saturationSlider.Value = result * 0.01f;
      this.UpdateLabels();
    }

    private void ValueTextboxOnFocusChanged(MyGuiControlBase obj, bool state)
    {
      if (state || !(obj is MyGuiControlTextbox guiControlTextbox))
        return;
      this.ValueTextboxOnEnterPressed(guiControlTextbox);
    }

    private void ValueTextboxOnEnterPressed(MyGuiControlTextbox obj)
    {
      float result;
      if (float.TryParse(obj.Text, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result))
        this.m_valueSlider.Value = result * 0.01f;
      this.UpdateLabels();
    }

    private void HexTextboxOnFocusChanged(MyGuiControlBase obj, bool state)
    {
      if (state || !(obj is MyGuiControlTextbox guiControlTextbox))
        return;
      this.HexTextboxOnEnterPressed(guiControlTextbox);
    }

    private void HexTextboxOnEnterPressed(MyGuiControlTextbox obj)
    {
      if (MySession.Static.LocalHumanPlayer == null)
        return;
      this.m_hexSb.Clear();
      obj.GetText(this.m_hexSb);
      Match match = MyGuiScreenColorPicker.HEX_REGEX.Match(this.m_hexSb.ToString());
      if (!match.Success || match.Length == 0)
      {
        Color currentColor = this.GetCurrentColor();
        this.m_hexSb.Clear();
        this.m_hexSb.AppendFormat("#{0:X2}{1:X2}{2:X2}", (object) currentColor.R, (object) currentColor.G, (object) currentColor.B);
        this.m_hexTextbox.SetText(this.m_hexSb);
      }
      else
      {
        string str = match.Value;
        if (str.Length > 6)
          str = str.Substring(1);
        Vector3 hsv = new Color((int) byte.Parse(str.Substring(0, 2), NumberStyles.HexNumber), (int) byte.Parse(str.Substring(2, 2), NumberStyles.HexNumber), (int) byte.Parse(str.Substring(4, 2), NumberStyles.HexNumber)).ColorToHSV();
        this.m_hueSlider.Value = hsv.X * 360f;
        this.m_saturationSlider.Value = hsv.Y;
        this.m_valueSlider.Value = hsv.Z;
        this.UpdateLabels();
      }
    }

    private void OnColorCheckedChanged(MyGuiControlCheckbox obj) => MyGuiScreenColorPicker.ApplyColor = obj.IsChecked;

    private void OnSkinCheckedChanged(MyGuiControlCheckbox obj) => MyGuiScreenColorPicker.ApplySkin = obj.IsChecked;

    private void ItemGridOnItemClicked(MyGuiControlGrid grid, MyGuiControlGrid.EventArgs eventArgs)
    {
      MyGuiGridItem itemAt = grid.GetItemAt(eventArgs.RowIndex, eventArgs.ColumnIndex);
      if (itemAt == null || MySession.Static == null || MySession.Static.LocalHumanPlayer == null)
        return;
      MyStringHash orCompute = MyStringHash.GetOrCompute((string) itemAt.UserData);
      if (orCompute != MyStringHash.NullOrEmpty)
      {
        MyAssetModifierDefinition modifierDefinition = MyDefinitionManager.Static.GetAssetModifierDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_AssetModifierDefinition), orCompute));
        if (!modifierDefinition.DLCs.IsNullOrEmpty<string>())
          this.ShowDLCStorePage(modifierDefinition);
        if (this.m_gameInventoryComp == null || !this.m_gameInventoryComp.HasArmor(orCompute, Sync.MyId))
          return;
      }
      MySession.Static.LocalHumanPlayer.BuildArmorSkin = (string) itemAt.UserData;
    }

    private MyGuiControlButton CreateButton(
      float usableWidth,
      StringBuilder text,
      Action<MyGuiControlButton> onClick,
      bool enabled = true,
      MyStringId? tooltip = null,
      float textScale = 1f)
    {
      Vector2? position = new Vector2?();
      Vector2? size = new Vector2?();
      Vector4? colorMask = new Vector4?();
      StringBuilder text1 = text;
      Action<MyGuiControlButton> action = onClick;
      double num = (double) textScale;
      Action<MyGuiControlButton> onButtonClick = action;
      int? buttonIndex = new int?();
      MyGuiControlButton guiControlButton = new MyGuiControlButton(position, MyGuiControlButtonStyleEnum.Rectangular, size, colorMask, text: text1, textScale: ((float) num), onButtonClick: onButtonClick, buttonIndex: buttonIndex);
      guiControlButton.Size = new Vector2(usableWidth, 0.034f);
      guiControlButton.Position = guiControlButton.Position + new Vector2(-0.02f, 0.0f);
      if (tooltip.HasValue)
        guiControlButton.SetToolTip(tooltip.Value);
      return guiControlButton;
    }

    protected override void OnShow()
    {
      base.OnShow();
      this.OnSetVisible(true);
    }

    protected override void OnClosed()
    {
      base.OnClosed();
      MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) null;
      this.OnSetVisible(false);
    }

    protected override void OnHide()
    {
      base.OnHide();
      this.OnSetVisible(false);
    }

    private void OnSetVisible(bool visible)
    {
      if (MyCubeBuilder.Static == null)
        return;
      MyCubeBuilder.Static.UseTransparency = !visible;
    }

    private void UpdateLabels()
    {
      this.m_hexSb.Clear();
      this.m_hexSb.Append(string.Format("{0:F1}", (object) this.m_hueSlider.Value));
      this.m_hueTextbox.SetText(this.m_hexSb);
      this.m_hexSb.Clear();
      this.m_hexSb.Append(string.Format("{0:F1}", (object) (float) ((double) this.m_saturationSlider.Value * 100.0)));
      this.m_saturationTextbox.SetText(this.m_hexSb);
      this.m_hexSb.Clear();
      this.m_hexSb.Append(string.Format("{0:F1}", (object) (float) ((double) this.m_valueSlider.Value * 100.0)));
      this.m_valueTextbox.SetText(this.m_hexSb);
    }

    private void UpdateSliders(Vector3 colorValue)
    {
      this.m_hueSlider.Value = colorValue.X * 360f;
      this.m_saturationSlider.Value = MathHelper.Clamp(colorValue.Y + MyColorPickerConstants.SATURATION_DELTA, 0.0f, 1f);
      this.m_valueSlider.Value = MathHelper.Clamp(colorValue.Z + MyColorPickerConstants.VALUE_DELTA - MyColorPickerConstants.VALUE_COLORIZE_DELTA, 0.0f, 1f);
    }

    private Vector3 prev(Vector3 HSV) => new Vector3(HSV.X, MathHelper.Clamp(HSV.Y + MyColorPickerConstants.SATURATION_DELTA, 0.0f, 1f), MathHelper.Clamp(HSV.Z + MyColorPickerConstants.VALUE_DELTA - MyColorPickerConstants.VALUE_COLORIZE_DELTA, 0.0f, 1f));

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.COLOR_PICKER))
        this.CloseScreenNow();
      MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
      if (localHumanPlayer != null && (MyInput.Static.IsNewLeftMousePressed() || MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACCEPT)))
      {
        for (int index = 0; index < this.m_colorPaletteControlsList.Count; ++index)
        {
          if (this.m_colorPaletteControlsList[index].IsMouseOver)
          {
            this.FocusedControl = (MyGuiControlBase) this.m_colorPaletteControlsList[index];
            break;
          }
        }
        for (int index = 0; index < this.m_colorPaletteControlsList.Count; ++index)
        {
          if (this.m_colorPaletteControlsList[index].HasFocus)
          {
            MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
            localHumanPlayer.SelectedBuildColorSlot = index;
            this.m_highlightControlPanel.Position = new Vector2((float) ((double) (localHumanPlayer.SelectedBuildColorSlot % 7) * 0.0379999987781048 - 0.132499992847443), (float) ((double) (localHumanPlayer.SelectedBuildColorSlot / 7) * 0.0350000001490116 - 0.375));
            this.UpdateSliders(localHumanPlayer.SelectedBuildColor);
          }
        }
      }
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y))
        this.OnDefaultsClick((MyGuiControlButton) null);
      base.HandleInput(receivedFocusInThisUpdate);
    }

    private void OnValueChange(MyGuiControlSlider sender)
    {
      MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
      if (localHumanPlayer == null)
        return;
      this.UpdateLabels();
      double num1 = (double) this.m_saturationSlider.Value;
      float num2 = this.m_valueSlider.Value;
      Color currentColor = this.GetCurrentColor();
      this.m_colorPaletteControlsList[localHumanPlayer.SelectedBuildColorSlot].ColorMask = currentColor.ToVector4();
      double saturationDelta = (double) MyColorPickerConstants.SATURATION_DELTA;
      float y = (float) (num1 - saturationDelta);
      float z = num2 - MyColorPickerConstants.VALUE_DELTA + MyColorPickerConstants.VALUE_COLORIZE_DELTA;
      localHumanPlayer.SelectedBuildColor = new Vector3(this.m_hueSlider.Value / 360f, y, z);
      this.m_hexSb.Clear();
      this.m_hexSb.AppendFormat("#{0:X2}{1:X2}{2:X2}", (object) currentColor.R, (object) currentColor.G, (object) currentColor.B);
      this.m_hexTextbox.SetText(this.m_hexSb);
    }

    private Color GetCurrentColor() => new Vector3(this.m_hueSlider.Value / 360f, this.m_saturationSlider.Value, this.m_valueSlider.Value).HSVtoColor();

    private void OnDefaultsClick(MyGuiControlButton sender)
    {
      MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
      if (localHumanPlayer == null)
        return;
      localHumanPlayer.SetDefaultColors();
      Color white = Color.White;
      for (int index = 0; index < 14; ++index)
        this.m_colorPaletteControlsList[index].ColorMask = this.prev(localHumanPlayer.BuildColorSlots[index]).HSVtoColor().ToVector4();
      localHumanPlayer.SelectedBuildColorSlot = 0;
      this.m_highlightControlPanel.Position = new Vector2((float) ((double) (localHumanPlayer.SelectedBuildColorSlot % 7) * 0.0379999987781048 - 0.132499992847443), (float) ((double) (localHumanPlayer.SelectedBuildColorSlot / 7) * 0.0350000001490116 - 0.375));
      this.UpdateSliders(localHumanPlayer.SelectedBuildColor);
      localHumanPlayer.BuildArmorSkin = string.Empty;
      this.m_itemGrid.SelectedIndex = new int?(0);
      this.m_applyColorCheckbox.IsChecked = true;
      this.m_applySkinCheckbox.IsChecked = true;
    }

    public override bool CloseScreen(bool isUnloading = false)
    {
      this.OnOkClick(this.m_okButton);
      return base.CloseScreen(isUnloading);
    }

    private void OnOkClick(MyGuiControlButton sender)
    {
      MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
      if (localHumanPlayer != null)
      {
        bool flag = false;
        int index = 0;
        foreach (Vector3 buildColorSlot in localHumanPlayer.BuildColorSlots)
        {
          if (this.m_oldPaletteList[index] != buildColorSlot)
          {
            flag = true;
            this.m_oldPaletteList[index] = buildColorSlot;
          }
          ++index;
        }
        if (flag)
          Sync.Players.RequestPlayerColorsChanged(localHumanPlayer.Id.SerialId, this.m_oldPaletteList);
      }
      MyCubeBuilder.Static.ColorPickerOk();
      MyGuiGridItem selectedItem = this.m_itemGrid.SelectedItem;
      if (MyGuiScreenColorPicker.ApplySkin && selectedItem != null && (MySession.Static != null && MySession.Static.LocalHumanPlayer != null))
      {
        MyStringHash orCompute = MyStringHash.GetOrCompute((string) selectedItem.UserData);
        if (orCompute != MyStringHash.NullOrEmpty && (this.m_gameInventoryComp == null || !this.m_gameInventoryComp.HasArmor(orCompute, Sync.MyId)))
        {
          this.CloseScreenNow();
          return;
        }
        int num = this.m_gameInventoryComp == null ? 0 : (this.m_gameInventoryComp.HasArmor(orCompute, Sync.MyId) ? 1 : 0);
        MyAssetModifierDefinition modifierDefinition = MyDefinitionManager.Static.GetAssetModifierDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_AssetModifierDefinition), orCompute));
        if (modifierDefinition == null)
        {
          this.CloseScreenNow();
          return;
        }
        if (!modifierDefinition.DLCs.IsNullOrEmpty<string>())
          this.ShowDLCStorePage(modifierDefinition);
      }
      this.CloseScreenNow();
    }

    private void ShowDLCStorePage(MyAssetModifierDefinition assetModDef)
    {
      MyDLCs.MyDLC missing = this.m_dlcComp.GetFirstMissingDefinitionDLC((MyDefinitionBase) assetModDef, Sync.MyId);
      if (missing == null)
        return;
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MySpaceTexts.SkinNotOwned), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionInfo), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result =>
      {
        if (result != MyGuiScreenMessageBox.ResultEnum.YES)
          return;
        MyGameService.OpenDlcInShop(missing.AppId);
      }))));
    }

    private void OnCancelClick(MyGuiControlButton sender)
    {
      MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
      if (localHumanPlayer != null)
      {
        localHumanPlayer.SetBuildColorSlots(this.m_oldPaletteList);
        localHumanPlayer.BuildArmorSkin = this.m_oldArmorSkin.String;
        localHumanPlayer.SelectedBuildColorSlot = this.m_oldColorSlot;
      }
      MyGuiScreenColorPicker.ApplySkin = this.m_oldApplySkin;
      MyGuiScreenColorPicker.ApplyColor = this.m_oldApplyColor;
      MyCubeBuilder.Static.ColorPickerCancel();
      this.CloseScreenNow();
    }

    public override string GetFriendlyName() => "ColorPick";

    private List<MyGameInventoryItemDefinition> GetInventoryItems()
    {
      IEnumerable<MyGameInventoryItemDefinition> definitionsForSlot = MyGameService.GetDefinitionsForSlot(MyGameInventoryItemSlot.Armor);
      List<MyGameInventoryItemDefinition> inventoryItemDefinitionList;
      if (definitionsForSlot == null)
      {
        inventoryItemDefinitionList = (List<MyGameInventoryItemDefinition>) null;
      }
      else
      {
        IOrderedEnumerable<MyGameInventoryItemDefinition> source = definitionsForSlot.OrderBy<MyGameInventoryItemDefinition, string>((Func<MyGameInventoryItemDefinition, string>) (e => e.Name));
        inventoryItemDefinitionList = source != null ? source.ToList<MyGameInventoryItemDefinition>() : (List<MyGameInventoryItemDefinition>) null;
      }
      return inventoryItemDefinitionList ?? new List<MyGameInventoryItemDefinition>();
    }

    public override bool Update(bool hasFocus)
    {
      this.m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_defaultsButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      return base.Update(hasFocus);
    }

    private class MyGameInventoryItemDefinitionComparer : IEqualityComparer<MyGameInventoryItem>
    {
      public static MyGuiScreenColorPicker.MyGameInventoryItemDefinitionComparer Comparer = new MyGuiScreenColorPicker.MyGameInventoryItemDefinitionComparer();

      public bool Equals(MyGameInventoryItem x, MyGameInventoryItem y)
      {
        if (x == null && y == null)
          return true;
        return (x != null || y == null) && (x == null || y != null) && this.GetHashCode(x) == this.GetHashCode(y);
      }

      public int GetHashCode(MyGameInventoryItem obj) => obj.ItemDefinition.ID;
    }
  }
}
