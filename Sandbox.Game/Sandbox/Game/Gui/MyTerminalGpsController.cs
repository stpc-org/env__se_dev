// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyTerminalGpsController
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using VRage;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  internal class MyTerminalGpsController : MyTerminalController
  {
    public static readonly Color ITEM_SHOWN_COLOR = Color.CornflowerBlue;
    private IMyGuiControlsParent m_controlsParent;
    private MyGuiControlSearchBox m_searchBox;
    private StringBuilder m_NameBuilder = new StringBuilder();
    private readonly StringBuilder m_hexSb = new StringBuilder();
    private readonly Regex HEX_REGEX = new Regex("^(#{0,1})([0-9A-Fa-f]{6})$");
    private MyGuiControlTable m_tableIns;
    private MyGuiControlLabel m_labelInsName;
    private MyGuiControlTextbox m_panelInsName;
    private MyGuiControlLabel m_labelInsDesc;
    private MyGuiControlMultilineEditableText m_panelInsDesc;
    private MyGuiControlLabel m_labelInsX;
    private MyGuiControlTextbox m_xCoord;
    private MyGuiControlLabel m_labelInsY;
    private MyGuiControlTextbox m_yCoord;
    private MyGuiControlLabel m_labelInsZ;
    private MyGuiControlTextbox m_zCoord;
    private MyGuiControlLabel m_labelColor;
    private MyGuiControlLabel m_labelHue;
    private MyGuiControlSlider m_sliderHue;
    private MyGuiControlLabel m_labelSaturation;
    private MyGuiControlSlider m_sliderSaturation;
    private MyGuiControlLabel m_labelValue;
    private MyGuiControlSlider m_sliderValue;
    private MyGuiControlLabel m_labelHex;
    private MyGuiControlTextbox m_textBoxHex;
    private MyGuiControlLabel m_labelInsShowOnHud;
    private MyGuiControlCheckbox m_checkInsShowOnHud;
    private MyGuiControlLabel m_labelInsAlwaysVisible;
    private MyGuiControlCheckbox m_checkInsAlwaysVisible;
    private MyGuiControlButton m_buttonAdd;
    private MyGuiControlButton m_buttonAddFromClipboard;
    private MyGuiControlButton m_buttonAddCurrent;
    private MyGuiControlButton m_buttonDelete;
    private MyGuiControlButton m_buttonCopy;
    private MyGuiControlLabel m_labelClipboardGamepadHelp;
    private MyGuiControlLabel m_labelSaveWarning;
    private int? m_previousHash;
    private bool m_needsSyncName;
    private bool m_needsSyncDesc;
    private bool m_needsSyncX;
    private bool m_needsSyncY;
    private bool m_needsSyncZ;
    private bool m_needsSyncColor;
    private string m_clipboardText;
    private bool m_nameOk;
    private bool m_xOk;
    private bool m_yOk;
    private bool m_zOk;
    private MyGps m_syncedGps;

    public void Init(IMyGuiControlsParent controlsParent)
    {
      this.m_controlsParent = controlsParent;
      this.m_searchBox = (MyGuiControlSearchBox) this.m_controlsParent.Controls.GetControlByName("SearchIns");
      this.m_searchBox.OnTextChanged += new MyGuiControlSearchBox.TextChangedDelegate(this.SearchIns_TextChanged);
      this.m_tableIns = (MyGuiControlTable) controlsParent.Controls.GetControlByName("TableINS");
      this.m_tableIns.SetColumnComparison(0, new Comparison<MyGuiControlTable.Cell>(this.TableSortingComparison));
      this.m_tableIns.ItemSelected += new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.OnTableItemSelected);
      this.m_tableIns.ItemDoubleClicked += new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.OnTableDoubleclick);
      this.m_buttonAdd = (MyGuiControlButton) this.m_controlsParent.Controls.GetControlByName("buttonAdd");
      this.m_buttonAddCurrent = (MyGuiControlButton) this.m_controlsParent.Controls.GetControlByName("buttonFromCurrent");
      this.m_buttonAddFromClipboard = (MyGuiControlButton) this.m_controlsParent.Controls.GetControlByName("buttonFromClipboard");
      this.m_buttonDelete = (MyGuiControlButton) this.m_controlsParent.Controls.GetControlByName("buttonDelete");
      this.m_buttonAdd.ButtonClicked += new Action<MyGuiControlButton>(this.OnButtonPressedNew);
      this.m_buttonAddFromClipboard.ButtonClicked += new Action<MyGuiControlButton>(this.OnButtonPressedNewFromClipboard);
      this.m_buttonAddCurrent.ButtonClicked += new Action<MyGuiControlButton>(this.OnButtonPressedNewFromCurrent);
      this.m_buttonDelete.ButtonClicked += new Action<MyGuiControlButton>(this.OnButtonPressedDelete);
      this.m_buttonAdd.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_buttonAddCurrent.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_buttonAddFromClipboard.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_buttonDelete.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_labelInsName = (MyGuiControlLabel) controlsParent.Controls.GetControlByName("labelInsName");
      this.m_panelInsName = (MyGuiControlTextbox) controlsParent.Controls.GetControlByName("panelInsName");
      this.m_labelInsDesc = (MyGuiControlLabel) controlsParent.Controls.GetControlByName("labelInsDesc");
      this.m_panelInsDesc = (MyGuiControlMultilineEditableText) controlsParent.Controls.GetControlByName("textInsDesc");
      this.m_labelInsX = (MyGuiControlLabel) controlsParent.Controls.GetControlByName("labelInsX");
      this.m_xCoord = (MyGuiControlTextbox) controlsParent.Controls.GetControlByName("textInsX");
      this.m_labelInsY = (MyGuiControlLabel) controlsParent.Controls.GetControlByName("labelInsY");
      this.m_yCoord = (MyGuiControlTextbox) controlsParent.Controls.GetControlByName("textInsY");
      this.m_labelInsZ = (MyGuiControlLabel) controlsParent.Controls.GetControlByName("labelInsZ");
      this.m_zCoord = (MyGuiControlTextbox) controlsParent.Controls.GetControlByName("textInsZ");
      this.m_labelInsShowOnHud = (MyGuiControlLabel) controlsParent.Controls.GetControlByName("labelInsShowOnHud");
      this.m_checkInsShowOnHud = (MyGuiControlCheckbox) controlsParent.Controls.GetControlByName("checkInsShowOnHud");
      this.m_checkInsShowOnHud.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_ShowOnHud_ToolTip));
      this.m_checkInsShowOnHud.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.OnShowOnHudChecked);
      this.m_labelInsAlwaysVisible = (MyGuiControlLabel) controlsParent.Controls.GetControlByName("labelInsAlwaysVisible");
      this.m_checkInsAlwaysVisible = (MyGuiControlCheckbox) controlsParent.Controls.GetControlByName("checkInsAlwaysVisible");
      this.m_checkInsAlwaysVisible.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.OnAlwaysVisibleChecked);
      this.m_buttonCopy = (MyGuiControlButton) this.m_controlsParent.Controls.GetControlByName("buttonToClipboard");
      this.m_buttonCopy.ButtonClicked += new Action<MyGuiControlButton>(this.OnButtonPressedCopy);
      this.m_labelClipboardGamepadHelp = (MyGuiControlLabel) this.m_controlsParent.Controls.GetControlByName("labelClipboardGamepadHelp");
      this.m_labelClipboardGamepadHelp.Visible = false;
      this.m_labelClipboardGamepadHelp.VisibleChanged += new VisibleChangedDelegate(this.LabelClipboardGamepadHelp_VisibleChanged);
      this.m_labelSaveWarning = (MyGuiControlLabel) controlsParent.Controls.GetControlByName("TerminalTab_GPS_SaveWarning");
      this.m_labelSaveWarning.Visible = false;
      this.m_labelColor = (MyGuiControlLabel) controlsParent.Controls.GetControlByName("gpsColorLabel");
      this.m_labelHue = (MyGuiControlLabel) controlsParent.Controls.GetControlByName("gpsHueLabel");
      this.m_sliderHue = (MyGuiControlSlider) controlsParent.Controls.GetControlByName("gpsHueSlider");
      this.m_labelSaturation = (MyGuiControlLabel) controlsParent.Controls.GetControlByName("gpsSaturationLabel");
      this.m_sliderSaturation = (MyGuiControlSlider) controlsParent.Controls.GetControlByName("gpsSaturationSlider");
      this.m_labelValue = (MyGuiControlLabel) controlsParent.Controls.GetControlByName("gpsValueLabel");
      this.m_sliderValue = (MyGuiControlSlider) controlsParent.Controls.GetControlByName("gpsValueSlider");
      this.m_labelHex = (MyGuiControlLabel) controlsParent.Controls.GetControlByName("textgpsHexLabelInsZ");
      this.m_textBoxHex = (MyGuiControlTextbox) controlsParent.Controls.GetControlByName("gpsColorHexTextbox");
      this.m_panelInsName.ShowTooltipWhenDisabled = true;
      this.m_panelInsDesc.ShowTooltipWhenDisabled = true;
      this.m_xCoord.ShowTooltipWhenDisabled = true;
      this.m_yCoord.ShowTooltipWhenDisabled = true;
      this.m_zCoord.ShowTooltipWhenDisabled = true;
      this.m_checkInsShowOnHud.ShowTooltipWhenDisabled = true;
      this.m_checkInsAlwaysVisible.ShowTooltipWhenDisabled = true;
      this.m_buttonCopy.ShowTooltipWhenDisabled = true;
      this.HookSyncEvents();
      MySession.Static.Gpss.GpsAdded += new Action<long, int>(this.OnGpsAdded);
      MySession.Static.Gpss.GpsChanged += new Action<long, int>(this.OnInsChanged);
      MySession.Static.Gpss.ListChanged += new Action<long>(this.OnListChanged);
      MySession.Static.Gpss.DiscardOld();
      this.PopulateList();
      this.m_previousHash = new int?();
      this.EnableEditBoxes(false);
      this.SetDeleteButtonEnabled(false);
    }

    private int TableSortingComparison(MyGuiControlTable.Cell a, MyGuiControlTable.Cell b)
    {
      if (((MyGps) a.UserData).DiscardAt.HasValue && ((MyGps) b.UserData).DiscardAt.HasValue || !((MyGps) a.UserData).DiscardAt.HasValue && !((MyGps) b.UserData).DiscardAt.HasValue)
        return a.Text.CompareToIgnoreCase(b.Text);
      return !((MyGps) a.UserData).DiscardAt.HasValue ? -1 : 1;
    }

    public void PopulateList() => this.PopulateList((string) null);

    public void PopulateList(string searchString)
    {
      object userData = this.m_tableIns.SelectedRow?.UserData;
      int? nullable1 = this.m_tableIns.SelectedRowIndex;
      this.ClearList();
      if (MySession.Static.Gpss.ExistsForPlayer(MySession.Static.LocalPlayerId))
      {
        foreach (KeyValuePair<int, MyGps> keyValuePair in MySession.Static.Gpss[MySession.Static.LocalPlayerId])
        {
          if (searchString != null)
          {
            string[] strArray = searchString.ToLower().Split(' ');
            string lower = keyValuePair.Value.Name.ToString().ToLower();
            bool flag = true;
            foreach (string str in strArray)
            {
              if (!lower.Contains(str.ToLower()))
              {
                flag = false;
                break;
              }
            }
            if (flag)
              this.AddToList(keyValuePair.Value);
          }
          else
            this.AddToList(keyValuePair.Value);
        }
      }
      this.m_tableIns.SortByColumn(0, new MyGuiControlTable.SortStateEnum?(MyGuiControlTable.SortStateEnum.Ascending));
      this.EnableEditBoxes(false);
      if (userData != null)
      {
        for (int index = 0; index < this.m_tableIns.RowsCount; ++index)
        {
          if (userData == this.m_tableIns.GetRow(index).UserData)
          {
            this.m_tableIns.SelectedRowIndex = new int?(index);
            this.EnableEditBoxes(true);
            this.SetDeleteButtonEnabled(true);
            break;
          }
        }
        if (this.m_tableIns.SelectedRow == null && userData != null)
        {
          int? nullable2 = nullable1;
          int rowsCount = this.m_tableIns.RowsCount;
          if (nullable2.GetValueOrDefault() >= rowsCount & nullable2.HasValue)
            nullable1 = new int?(this.m_tableIns.RowsCount - 1);
          this.m_tableIns.SelectedRowIndex = nullable1;
          if (this.m_tableIns.SelectedRow != null)
          {
            this.EnableEditBoxes(true);
            this.SetDeleteButtonEnabled(true);
            this.FillRight((MyGps) this.m_tableIns.SelectedRow.UserData);
          }
        }
      }
      this.m_tableIns.ScrollToSelection();
      if (userData != null)
        return;
      this.FillRight();
    }

    private MyGuiControlTable.Row AddToList(MyGps ins)
    {
      MyGuiControlTable.Row row1 = new MyGuiControlTable.Row((object) ins);
      StringBuilder stringBuilder = new StringBuilder(ins.Name);
      MyGuiControlTable.Row row2 = row1;
      StringBuilder text = stringBuilder;
      string str = stringBuilder.ToString();
      MyGps myGps = ins;
      string toolTip = str;
      Color? textColor = new Color?(ins.DiscardAt.HasValue ? Color.Gray : (ins.ShowOnHud ? ins.GPSColor : Color.White));
      MyGuiHighlightTexture? icon = new MyGuiHighlightTexture?();
      MyGuiControlTable.Cell cell = new MyGuiControlTable.Cell(text, (object) myGps, toolTip, textColor, icon);
      row2.AddCell(cell);
      this.m_tableIns.Add(row1);
      return row1;
    }

    public void ClearList()
    {
      if (this.m_tableIns == null)
        return;
      this.m_tableIns.Clear();
    }

    private void SearchIns_TextChanged(string text) => this.PopulateList(text);

    private void OnTableItemSelected(MyGuiControlTable sender, MyGuiControlTable.EventArgs args)
    {
      this.trySync();
      if (sender.SelectedRow != null)
      {
        this.EnableEditBoxes(true);
        this.SetDeleteButtonEnabled(true);
        this.FillRight((MyGps) sender.SelectedRow.UserData);
        MyGuiControlTable.Cell cell = sender.SelectedRow.GetCell(0);
        if (cell == null)
          return;
        MyGps userData = (MyGps) this.m_tableIns.SelectedRow.UserData;
        cell.TextColor = new Color?(userData.DiscardAt.HasValue ? Color.Gray : (userData.ShowOnHud ? userData.GPSColor : Color.White));
      }
      else
      {
        this.EnableEditBoxes(false);
        this.SetDeleteButtonEnabled(false);
        this.ClearRight();
      }
    }

    private void SetDeleteButtonEnabled(bool enabled)
    {
      if (enabled)
      {
        this.m_buttonDelete.Enabled = true;
        this.m_buttonDelete.SetToolTip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_Delete_ToolTip));
      }
      else
      {
        this.m_buttonDelete.Enabled = false;
        this.m_buttonDelete.SetToolTip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_Delete_Disabled_ToolTip));
      }
    }

    private void EnableEditBoxes(bool enable)
    {
      this.m_panelInsName.Enabled = enable;
      this.m_panelInsDesc.Enabled = enable;
      this.m_xCoord.Enabled = enable;
      this.m_yCoord.Enabled = enable;
      this.m_zCoord.Enabled = enable;
      this.m_sliderHue.Enabled = enable;
      this.m_sliderSaturation.Enabled = enable;
      this.m_sliderValue.Enabled = enable;
      this.m_textBoxHex.Enabled = enable;
      if (enable)
      {
        this.m_panelInsName.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_NewCoord_Name_ToolTip));
        this.m_panelInsDesc.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_NewCoord_Desc_ToolTip));
        this.m_xCoord.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_X_ToolTip));
        this.m_yCoord.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_Y_ToolTip));
        this.m_zCoord.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_Z_ToolTip));
        this.m_checkInsShowOnHud.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_ShowOnHud_ToolTip));
        this.m_checkInsAlwaysVisible.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_AlwaysVisible_Tooltip));
        this.m_buttonCopy.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_CopyToClipboard_ToolTip));
      }
      else
      {
        this.m_checkInsShowOnHud.ShowTooltipWhenDisabled = true;
        this.m_checkInsAlwaysVisible.ShowTooltipWhenDisabled = true;
        this.m_panelInsName.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_SelectGpsEntry));
        this.m_panelInsDesc.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_SelectGpsEntry));
        this.m_xCoord.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_SelectGpsEntry));
        this.m_yCoord.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_SelectGpsEntry));
        this.m_zCoord.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_SelectGpsEntry));
        this.m_checkInsShowOnHud.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_SelectGpsEntry));
        this.m_checkInsAlwaysVisible.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_SelectGpsEntry));
        this.m_buttonCopy.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_SelectGpsEntry));
      }
    }

    private void OnTableDoubleclick(MyGuiControlTable sender, MyGuiControlTable.EventArgs args) => MyTerminalGpsController.ToggleShowOnHud(sender);

    private static void ToggleShowOnHud(MyGuiControlTable sender)
    {
      if (sender.SelectedRow == null)
        return;
      MyGps userData = (MyGps) sender.SelectedRow.UserData;
      userData.ShowOnHud = !userData.ShowOnHud;
      MySession.Static.Gpss.ChangeShowOnHud(MySession.Static.LocalPlayerId, ((MyGps) sender.SelectedRow.UserData).Hash, ((MyGps) sender.SelectedRow.UserData).ShowOnHud);
    }

    private void HookSyncEvents()
    {
      this.m_panelInsName.TextChanged += new Action<MyGuiControlTextbox>(this.OnNameChanged);
      this.m_panelInsDesc.TextChanged += new Action<MyGuiControlMultilineEditableText>(this.OnDescChanged);
      this.m_xCoord.TextChanged += new Action<MyGuiControlTextbox>(this.OnXChanged);
      this.m_yCoord.TextChanged += new Action<MyGuiControlTextbox>(this.OnYChanged);
      this.m_zCoord.TextChanged += new Action<MyGuiControlTextbox>(this.OnZChanged);
      this.m_textBoxHex.TextChanged += new Action<MyGuiControlTextbox>(this.HexTextboxOnTextChanged);
      this.m_textBoxHex.FocusChanged += new Action<MyGuiControlBase, bool>(this.HexTextboxOnFocusChanged);
      this.m_sliderHue.ValueChanged += new Action<MyGuiControlSlider>(this.OnSliderValueChanged);
      this.m_sliderSaturation.ValueChanged += new Action<MyGuiControlSlider>(this.OnSliderValueChanged);
      this.m_sliderValue.ValueChanged += new Action<MyGuiControlSlider>(this.OnSliderValueChanged);
    }

    private void UnhookSyncEvents()
    {
      this.m_panelInsName.TextChanged -= new Action<MyGuiControlTextbox>(this.OnNameChanged);
      this.m_panelInsDesc.TextChanged -= new Action<MyGuiControlMultilineEditableText>(this.OnDescChanged);
      this.m_xCoord.TextChanged -= new Action<MyGuiControlTextbox>(this.OnXChanged);
      this.m_yCoord.TextChanged -= new Action<MyGuiControlTextbox>(this.OnYChanged);
      this.m_zCoord.TextChanged -= new Action<MyGuiControlTextbox>(this.OnZChanged);
      this.m_textBoxHex.TextChanged -= new Action<MyGuiControlTextbox>(this.HexTextboxOnTextChanged);
      this.m_textBoxHex.FocusChanged -= new Action<MyGuiControlBase, bool>(this.HexTextboxOnFocusChanged);
      this.m_sliderHue.ValueChanged -= new Action<MyGuiControlSlider>(this.OnSliderValueChanged);
      this.m_sliderSaturation.ValueChanged -= new Action<MyGuiControlSlider>(this.OnSliderValueChanged);
      this.m_sliderValue.ValueChanged -= new Action<MyGuiControlSlider>(this.OnSliderValueChanged);
    }

    public void OnNameChanged(MyGuiControlTextbox sender)
    {
      if (this.m_tableIns.SelectedRow == null)
        return;
      this.m_needsSyncName = true;
      if (this.IsNameOk(sender.Text))
      {
        this.m_nameOk = true;
        sender.ColorMask = Vector4.One;
        MyGuiControlTable.Row selectedRow = this.m_tableIns.SelectedRow;
        MyGuiControlTable.Cell cell = selectedRow.GetCell(0);
        if (cell != null)
        {
          MyGps userData = (MyGps) this.m_tableIns.SelectedRow.UserData;
          cell.Text.Clear().Append(sender.Text);
          cell.TextColor = new Color?(userData.DiscardAt.HasValue ? Color.Gray : (userData.ShowOnHud ? userData.GPSColor : Color.White));
          cell.ToolTip.ToolTips[0] = new MyColoredText(sender.Text);
        }
        this.m_tableIns.SortByColumn(0, new MyGuiControlTable.SortStateEnum?(MyGuiControlTable.SortStateEnum.Ascending));
        for (int index = 0; index < this.m_tableIns.RowsCount; ++index)
        {
          if (selectedRow == this.m_tableIns.GetRow(index))
          {
            this.m_tableIns.SelectedRowIndex = new int?(index);
            break;
          }
        }
        this.m_tableIns.ScrollToSelection();
      }
      else
      {
        this.m_nameOk = false;
        sender.ColorMask = Color.Red.ToVector4();
      }
      this.UpdateWarningLabel();
    }

    public bool IsNameOk(string str) => !str.Contains(":");

    public void OnDescChanged(MyGuiControlMultilineEditableText sender) => this.m_needsSyncDesc = true;

    private bool IsCoordOk(string str) => double.TryParse(str, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out double _);

    public void OnXChanged(MyGuiControlTextbox sender)
    {
      this.m_needsSyncX = true;
      if (this.IsCoordOk(sender.Text))
      {
        this.m_xOk = true;
        sender.ColorMask = Vector4.One;
      }
      else
      {
        this.m_xOk = false;
        sender.ColorMask = Color.Red.ToVector4();
      }
      this.UpdateWarningLabel();
    }

    public void OnYChanged(MyGuiControlTextbox sender)
    {
      this.m_needsSyncY = true;
      if (this.IsCoordOk(sender.Text))
      {
        this.m_yOk = true;
        sender.ColorMask = Vector4.One;
      }
      else
      {
        this.m_yOk = false;
        sender.ColorMask = Color.Red.ToVector4();
      }
      this.UpdateWarningLabel();
    }

    public void OnZChanged(MyGuiControlTextbox sender)
    {
      this.m_needsSyncZ = true;
      if (this.IsCoordOk(sender.Text))
      {
        this.m_zOk = true;
        sender.ColorMask = Vector4.One;
      }
      else
      {
        this.m_zOk = false;
        sender.ColorMask = Color.Red.ToVector4();
      }
      this.UpdateWarningLabel();
    }

    public void OnSliderValueChanged(MyGuiControlSlider slider)
    {
      if (slider.Name == this.m_sliderHue.Name)
        this.RefreshGPSColorControlsTooltips(refreshOnlyThisSlider: this.m_sliderHue);
      if (slider.Name == this.m_sliderSaturation.Name)
        this.RefreshGPSColorControlsTooltips(refreshOnlyThisSlider: this.m_sliderSaturation);
      if (slider.Name == this.m_sliderValue.Name)
        this.RefreshGPSColorControlsTooltips(refreshOnlyThisSlider: this.m_sliderValue);
      this.OnColorChanged();
    }

    public void OnColorChanged()
    {
      this.m_needsSyncColor = true;
      if (this.m_tableIns.SelectedRow == null)
        return;
      MyGuiControlTable.Cell cell = this.m_tableIns.SelectedRow.GetCell(0);
      if (cell == null)
        return;
      cell.TextColor = new Color?(new Vector3((double) this.m_sliderHue.Value / 360.0, (double) this.m_sliderSaturation.Value, (double) this.m_sliderValue.Value).HSVtoColor());
      this.m_hexSb.Clear();
      StringBuilder hexSb = this.m_hexSb;
      // ISSUE: variable of a boxed type
      __Boxed<byte> r = (ValueType) cell.TextColor.Value.R;
      Color color = cell.TextColor.Value;
      // ISSUE: variable of a boxed type
      __Boxed<byte> g = (ValueType) color.G;
      color = cell.TextColor.Value;
      // ISSUE: variable of a boxed type
      __Boxed<byte> b = (ValueType) color.B;
      hexSb.AppendFormat("#{0:X2}{1:X2}{2:X2}", (object) r, (object) g, (object) b);
      this.m_textBoxHex.TextChanged -= new Action<MyGuiControlTextbox>(this.HexTextboxOnTextChanged);
      this.m_textBoxHex.TextChanged -= new Action<MyGuiControlTextbox>(this.HexTextboxOnTextChanged);
      this.m_textBoxHex.SetText(this.m_hexSb);
      this.m_textBoxHex.TextChanged += new Action<MyGuiControlTextbox>(this.HexTextboxOnTextChanged);
    }

    private void RefreshGPSColorControlsTooltips(
      bool clear = false,
      MyGuiControlSlider refreshOnlyThisSlider = null)
    {
      if (!clear)
      {
        this.m_textBoxHex.Tooltips.ToolTips[0].SetText(MyTexts.GetString(MySpaceTexts.GPSScreen_hexTooltip) + (object) this.m_hexSb);
        this.m_textBoxHex.Tooltips.RecalculateSize();
        if (refreshOnlyThisSlider == null || refreshOnlyThisSlider == this.m_sliderHue)
        {
          this.m_sliderHue.Tooltips.ToolTips[0].SetText(MyTexts.GetString(MySpaceTexts.GPSScreen_hueTooltip) + (object) this.m_sliderHue.Value);
          this.m_sliderHue.Tooltips.RecalculateSize();
        }
        if (refreshOnlyThisSlider == null || refreshOnlyThisSlider == this.m_sliderSaturation)
        {
          this.m_sliderSaturation.Tooltips.ToolTips[0].SetText(MyTexts.GetString(MySpaceTexts.GPSScreen_saturationTooltip) + (object) this.m_sliderSaturation.Value);
          this.m_sliderSaturation.Tooltips.RecalculateSize();
        }
        if (refreshOnlyThisSlider != null && refreshOnlyThisSlider != this.m_sliderValue)
          return;
        this.m_sliderValue.Tooltips.ToolTips[0].SetText(MyTexts.GetString(MySpaceTexts.GPSScreen_valueTooltip) + (object) this.m_sliderValue.Value);
        this.m_sliderValue.Tooltips.RecalculateSize();
      }
      else
      {
        this.m_textBoxHex.Tooltips.ToolTips[0].SetText("");
        this.m_textBoxHex.Tooltips.RecalculateSize();
        this.m_sliderHue.Tooltips.ToolTips[0].SetText("");
        this.m_sliderHue.Tooltips.RecalculateSize();
        this.m_sliderSaturation.Tooltips.ToolTips[0].SetText("");
        this.m_sliderSaturation.Tooltips.RecalculateSize();
        this.m_sliderValue.Tooltips.ToolTips[0].SetText("");
        this.m_sliderValue.Tooltips.RecalculateSize();
      }
    }

    private void UpdateWarningLabel()
    {
      if (this.m_nameOk && this.m_xOk && (this.m_yOk && this.m_zOk))
      {
        this.m_labelSaveWarning.Visible = false;
        if (!this.m_panelInsName.Enabled)
          return;
        this.m_buttonCopy.Enabled = true;
      }
      else
      {
        this.m_labelSaveWarning.Visible = true;
        this.m_buttonCopy.Enabled = false;
      }
    }

    private bool trySync()
    {
      MyGps gps;
      if (!this.m_previousHash.HasValue || !this.m_needsSyncName && !this.m_needsSyncDesc && (!this.m_needsSyncX && !this.m_needsSyncY) && (!this.m_needsSyncZ && !this.m_needsSyncColor) || (!MySession.Static.Gpss.ExistsForPlayer(MySession.Static.LocalPlayerId) || !this.IsNameOk(this.m_panelInsName.Text) || (!this.IsCoordOk(this.m_xCoord.Text) || !this.IsCoordOk(this.m_yCoord.Text)) || (!this.IsCoordOk(this.m_zCoord.Text) || !MySession.Static.Gpss[MySession.Static.LocalPlayerId].TryGetValue(this.m_previousHash.Value, out gps))))
        return false;
      if (this.m_needsSyncName)
        gps.Name = this.m_panelInsName.Text;
      if (this.m_needsSyncDesc)
        gps.Description = this.m_panelInsDesc.Text.ToString();
      StringBuilder result = new StringBuilder();
      Vector3D coords = gps.Coords;
      if (this.m_needsSyncX)
      {
        this.m_xCoord.GetText(result);
        coords.X = Math.Round(double.Parse(result.ToString(), (IFormatProvider) CultureInfo.InvariantCulture), 2);
      }
      result.Clear();
      if (this.m_needsSyncY)
      {
        this.m_yCoord.GetText(result);
        coords.Y = Math.Round(double.Parse(result.ToString(), (IFormatProvider) CultureInfo.InvariantCulture), 2);
      }
      result.Clear();
      if (this.m_needsSyncZ)
      {
        this.m_zCoord.GetText(result);
        coords.Z = Math.Round(double.Parse(result.ToString(), (IFormatProvider) CultureInfo.InvariantCulture), 2);
      }
      if (this.m_needsSyncColor)
        gps.GPSColor = new Vector3((double) this.m_sliderHue.Value / 360.0, (double) this.m_sliderSaturation.Value, (double) this.m_sliderValue.Value).HSVtoColor();
      gps.Coords = coords;
      this.m_syncedGps = gps;
      MySession.Static.Gpss.SendModifyGps(MySession.Static.LocalPlayerId, gps);
      return true;
    }

    private void OnButtonPressedNew(MyGuiControlButton sender)
    {
      this.trySync();
      MyGps gps = new MyGps();
      gps.Name = MyTexts.Get(MySpaceTexts.TerminalTab_GPS_NewCoord_Name).ToString();
      gps.Description = MyTexts.Get(MySpaceTexts.TerminalTab_GPS_NewCoord_Desc).ToString();
      gps.Coords = new Vector3D(0.0, 0.0, 0.0);
      gps.ShowOnHud = true;
      gps.DiscardAt = new TimeSpan?();
      this.EnableEditBoxes(false);
      MySession.Static.Gpss.SendAddGps(MySession.Static.LocalPlayerId, ref gps);
      this.m_searchBox.SearchText = string.Empty;
    }

    private void OnButtonPressedNewFromCurrent(MyGuiControlButton sender)
    {
      this.trySync();
      MyGps gps = new MyGps();
      MySession.Static.Gpss.GetNameForNewCurrent(this.m_NameBuilder);
      gps.Name = this.m_NameBuilder.ToString();
      gps.Description = MyTexts.Get(MySpaceTexts.TerminalTab_GPS_NewFromCurrent_Desc).ToString();
      Vector3D position = MySession.Static.LocalHumanPlayer.GetPosition();
      position.X = Math.Round(position.X, 2);
      position.Y = Math.Round(position.Y, 2);
      position.Z = Math.Round(position.Z, 2);
      gps.Coords = position;
      gps.ShowOnHud = true;
      gps.DiscardAt = new TimeSpan?();
      this.EnableEditBoxes(false);
      MySession.Static.Gpss.SendAddGps(MySession.Static.LocalPlayerId, ref gps);
      this.m_searchBox.SearchText = string.Empty;
    }

    private void PasteFromClipboard() => this.m_clipboardText = MyVRage.Platform.System.Clipboard;

    private void OnButtonPressedNewFromClipboard(MyGuiControlButton sender)
    {
      Thread thread = new Thread((ThreadStart) (() => this.PasteFromClipboard()));
      thread.SetApartmentState(ApartmentState.STA);
      thread.Start();
      thread.Join();
      if (!string.IsNullOrEmpty(this.m_clipboardText))
        MySession.Static.Gpss.ScanText(this.m_clipboardText, MyTexts.Get(MySpaceTexts.TerminalTab_GPS_NewFromClipboard_Desc));
      this.m_searchBox.SearchText = string.Empty;
    }

    private void OnButtonPressedDelete(MyGuiControlButton sender)
    {
      if (this.m_tableIns.SelectedRow == null)
        return;
      this.Delete();
    }

    private void Delete()
    {
      MySession.Static.Gpss.SendDelete(MySession.Static.LocalPlayerId, ((MyGps) this.m_tableIns.SelectedRow.UserData).GetHashCode());
      this.EnableEditBoxes(false);
      this.SetDeleteButtonEnabled(false);
      this.PopulateList();
    }

    public void OnDelKeyPressed()
    {
      if (this.m_tableIns.SelectedRow == null || !this.m_tableIns.HasFocus)
        return;
      this.Delete();
    }

    private void OnButtonPressedCopy(MyGuiControlButton sender)
    {
      if (this.m_tableIns.SelectedRow == null)
        return;
      if (this.trySync())
        this.m_syncedGps.ToClipboard();
      else
        ((MyGps) this.m_tableIns.SelectedRow.UserData).ToClipboard();
    }

    private void OnGpsAdded(long id, int hash)
    {
      if (id != MySession.Static.LocalPlayerId)
        return;
      for (int index = 0; index < this.m_tableIns.RowsCount; ++index)
      {
        MyGps userData = (MyGps) this.m_tableIns.GetRow(index).UserData;
        if (userData.GetHashCode() == hash)
        {
          this.m_tableIns.SelectedRowIndex = new int?(index);
          if (this.m_tableIns.SelectedRow == null)
            break;
          this.EnableEditBoxes(true);
          this.SetDeleteButtonEnabled(true);
          this.FillRight((MyGps) this.m_tableIns.SelectedRow.UserData);
          this.m_tableIns.GetRow(index).GetCell(0).TextColor = new Color?(userData.DiscardAt.HasValue ? Color.Gray : (userData.ShowOnHud ? userData.GPSColor : Color.White));
          break;
        }
      }
    }

    private void OnInsChanged(long id, int hash)
    {
      if (id != MySession.Static.LocalPlayerId)
        return;
      if (this.m_tableIns.SelectedRow != null && ((MyGps) this.m_tableIns.SelectedRow.UserData).GetHashCode() == hash)
        this.FillRight();
      for (int index = 0; index < this.m_tableIns.RowsCount; ++index)
      {
        if (((MyGps) this.m_tableIns.GetRow(index).UserData).GetHashCode() == hash)
        {
          MyGuiControlTable.Cell cell = this.m_tableIns.GetRow(index).GetCell(0);
          if (cell == null)
            break;
          MyGps userData = (MyGps) this.m_tableIns.GetRow(index).UserData;
          cell.TextColor = new Color?(userData.DiscardAt.HasValue ? Color.Gray : (userData.ShowOnHud ? userData.GPSColor : Color.White));
          cell.Text.Clear().Append(((MyGps) this.m_tableIns.GetRow(index).UserData).Name);
          break;
        }
      }
    }

    private void OnListChanged(long id)
    {
      if (id != MySession.Static.LocalPlayerId)
        return;
      this.PopulateList();
    }

    private void OnShowOnHudChecked(MyGuiControlCheckbox sender)
    {
      if (this.m_tableIns.SelectedRow == null)
        return;
      MyGps userData = this.m_tableIns.SelectedRow.UserData as MyGps;
      userData.ShowOnHud = sender.IsChecked;
      if (!sender.IsChecked && userData.AlwaysVisible)
      {
        userData.AlwaysVisible = false;
        this.m_checkInsShowOnHud.IsCheckedChanged -= new Action<MyGuiControlCheckbox>(this.OnShowOnHudChecked);
        this.m_checkInsShowOnHud.IsChecked = false;
        this.m_checkInsShowOnHud.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.OnShowOnHudChecked);
      }
      if (this.trySync())
        return;
      MySession.Static.Gpss.ChangeShowOnHud(MySession.Static.LocalPlayerId, userData.Hash, sender.IsChecked);
    }

    private void OnAlwaysVisibleChecked(MyGuiControlCheckbox sender)
    {
      if (this.m_tableIns.SelectedRow == null)
        return;
      MyGps userData = this.m_tableIns.SelectedRow.UserData as MyGps;
      userData.AlwaysVisible = sender.IsChecked;
      userData.ShowOnHud = userData.ShowOnHud || userData.AlwaysVisible;
      this.m_checkInsShowOnHud.IsCheckedChanged -= new Action<MyGuiControlCheckbox>(this.OnShowOnHudChecked);
      this.m_checkInsShowOnHud.IsChecked = this.m_checkInsShowOnHud.IsChecked || sender.IsChecked;
      this.m_checkInsShowOnHud.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.OnShowOnHudChecked);
      if (this.trySync())
        return;
      MySession.Static.Gpss.ChangeAlwaysVisible(MySession.Static.LocalPlayerId, userData.Hash, sender.IsChecked);
    }

    private void FillRight()
    {
      if (this.m_tableIns.SelectedRow != null)
        this.FillRight((MyGps) this.m_tableIns.SelectedRow.UserData);
      else
        this.ClearRight();
      this.m_nameOk = this.m_xOk = this.m_yOk = this.m_zOk = true;
      this.UpdateWarningLabel();
    }

    private void HexTextboxOnFocusChanged(MyGuiControlBase obj, bool state)
    {
      if (state || !(obj is MyGuiControlTextbox guiControlTextbox))
        return;
      this.HexTextboxOnTextChanged(guiControlTextbox);
    }

    private Color GetCurrentColor() => new Vector3(this.m_sliderHue.Value / 360f, this.m_sliderSaturation.Value, this.m_sliderValue.Value).HSVtoColor();

    private void HexTextboxOnTextChanged(MyGuiControlTextbox obj)
    {
      if (MySession.Static.LocalHumanPlayer == null)
        return;
      this.UnhookSyncEvents();
      Match match = this.CheckHexTextbox();
      if (!match.Success || match.Length == 0)
      {
        this.HookSyncEvents();
      }
      else
      {
        string str = match.Value;
        if (str.Length > 6)
          str = str.Substring(1);
        Vector3 hsv = new Color((int) byte.Parse(str.Substring(0, 2), NumberStyles.HexNumber), (int) byte.Parse(str.Substring(2, 2), NumberStyles.HexNumber), (int) byte.Parse(str.Substring(4, 2), NumberStyles.HexNumber)).ColorToHSV();
        this.m_sliderHue.Value = hsv.X * 360f;
        this.m_sliderSaturation.Value = hsv.Y;
        this.m_sliderValue.Value = hsv.Z;
        this.OnColorChanged();
        this.HookSyncEvents();
      }
    }

    private Match CheckHexTextbox()
    {
      this.m_hexSb.Clear();
      this.m_textBoxHex.GetText(this.m_hexSb);
      Match match = this.HEX_REGEX.Match(this.m_hexSb.ToString());
      if (!match.Success || match.Length == 0)
      {
        Color currentColor = this.GetCurrentColor();
        this.m_hexSb.Clear();
        this.m_hexSb.AppendFormat("#{0:X2}{1:X2}{2:X2}", (object) currentColor.R, (object) currentColor.G, (object) currentColor.B);
        this.m_textBoxHex.ColorMask = Vector4.One;
        this.m_textBoxHex.ColorMask = Color.Red.ToVector4();
        return match;
      }
      if (this.m_textBoxHex.ColorMask == Color.Red.ToVector4())
        this.m_textBoxHex.ColorMask = this.m_buttonAdd.ColorMask;
      return match;
    }

    private void FillRight(MyGps ins)
    {
      this.UnhookSyncEvents();
      this.EnableEditBoxes(ins.EntityId == 0L && !ins.IsContainerGPS);
      this.m_panelInsName.SetText(new StringBuilder(ins.Name));
      this.m_panelInsDesc.Text = new StringBuilder(ins.Description);
      MyGuiControlTextbox xCoord = this.m_xCoord;
      Vector3D coords = ins.Coords;
      StringBuilder source1 = new StringBuilder(coords.X.ToString("F2", (IFormatProvider) CultureInfo.InvariantCulture));
      xCoord.SetText(source1);
      MyGuiControlTextbox yCoord = this.m_yCoord;
      coords = ins.Coords;
      StringBuilder source2 = new StringBuilder(coords.Y.ToString("F2", (IFormatProvider) CultureInfo.InvariantCulture));
      yCoord.SetText(source2);
      MyGuiControlTextbox zCoord = this.m_zCoord;
      coords = ins.Coords;
      StringBuilder source3 = new StringBuilder(coords.Z.ToString("F2", (IFormatProvider) CultureInfo.InvariantCulture));
      zCoord.SetText(source3);
      Vector3 hsv = ins.GPSColor.ColorToHSV();
      this.m_hexSb.Clear();
      StringBuilder hexSb = this.m_hexSb;
      // ISSUE: variable of a boxed type
      __Boxed<byte> r = (ValueType) ins.GPSColor.R;
      Color gpsColor = ins.GPSColor;
      // ISSUE: variable of a boxed type
      __Boxed<byte> g = (ValueType) gpsColor.G;
      gpsColor = ins.GPSColor;
      // ISSUE: variable of a boxed type
      __Boxed<byte> b = (ValueType) gpsColor.B;
      hexSb.AppendFormat("#{0:X2}{1:X2}{2:X2}", (object) r, (object) g, (object) b);
      this.m_textBoxHex.TextChanged -= new Action<MyGuiControlTextbox>(this.HexTextboxOnTextChanged);
      this.m_textBoxHex.SetText(this.m_hexSb);
      this.CheckHexTextbox();
      this.m_textBoxHex.TextChanged += new Action<MyGuiControlTextbox>(this.HexTextboxOnTextChanged);
      this.m_sliderHue.Value = hsv.X * 360f;
      this.m_sliderSaturation.Value = hsv.Y;
      this.m_sliderValue.Value = hsv.Z;
      this.m_checkInsShowOnHud.IsCheckedChanged -= new Action<MyGuiControlCheckbox>(this.OnShowOnHudChecked);
      this.m_checkInsShowOnHud.IsChecked = ins.ShowOnHud;
      this.m_checkInsShowOnHud.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.OnShowOnHudChecked);
      this.m_checkInsAlwaysVisible.IsCheckedChanged -= new Action<MyGuiControlCheckbox>(this.OnAlwaysVisibleChecked);
      this.m_checkInsAlwaysVisible.IsChecked = ins.AlwaysVisible;
      this.m_checkInsAlwaysVisible.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.OnAlwaysVisibleChecked);
      this.m_previousHash = new int?(ins.Hash);
      this.RefreshGPSColorControlsTooltips();
      this.HookSyncEvents();
      this.m_needsSyncName = false;
      this.m_needsSyncDesc = false;
      this.m_needsSyncX = false;
      this.m_needsSyncY = false;
      this.m_needsSyncZ = false;
      this.m_needsSyncColor = false;
      this.m_panelInsName.ColorMask = Vector4.One;
      this.m_xCoord.ColorMask = Vector4.One;
      this.m_yCoord.ColorMask = Vector4.One;
      this.m_zCoord.ColorMask = Vector4.One;
      this.m_nameOk = this.m_xOk = this.m_yOk = this.m_zOk = true;
      this.UpdateWarningLabel();
    }

    private void ClearRight()
    {
      this.UnhookSyncEvents();
      StringBuilder source = new StringBuilder("");
      this.m_panelInsName.SetText(source);
      this.m_panelInsDesc.Clear();
      this.m_xCoord.SetText(source);
      this.m_yCoord.SetText(source);
      this.m_zCoord.SetText(source);
      this.m_textBoxHex.TextChanged -= new Action<MyGuiControlTextbox>(this.HexTextboxOnTextChanged);
      this.m_textBoxHex.SetText(source);
      this.m_textBoxHex.TextChanged += new Action<MyGuiControlTextbox>(this.HexTextboxOnTextChanged);
      this.m_sliderValue.Value = 0.0f;
      this.m_sliderSaturation.Value = 0.0f;
      this.m_sliderHue.Value = 0.0f;
      this.m_checkInsShowOnHud.IsChecked = false;
      this.m_checkInsAlwaysVisible.IsChecked = false;
      this.m_previousHash = new int?();
      this.RefreshGPSColorControlsTooltips(true);
      this.HookSyncEvents();
      this.m_needsSyncName = false;
      this.m_needsSyncDesc = false;
      this.m_needsSyncX = false;
      this.m_needsSyncY = false;
      this.m_needsSyncZ = false;
      this.m_needsSyncColor = false;
    }

    private void LabelClipboardGamepadHelp_VisibleChanged(object sender, bool isVisible) => this.m_labelClipboardGamepadHelp.TextEnum = isVisible ? MySpaceTexts.TerminalTab_GPS_CopyToClipboard_GamepadHelp : MyStringId.NullOrEmpty;

    public void Close()
    {
      this.trySync();
      if (this.m_tableIns != null)
      {
        this.ClearList();
        this.m_tableIns.ItemSelected -= new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.OnTableItemSelected);
        this.m_tableIns.ItemDoubleClicked -= new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.OnTableDoubleclick);
      }
      this.m_syncedGps = (MyGps) null;
      MySession.Static.Gpss.GpsAdded -= new Action<long, int>(this.OnGpsAdded);
      MySession.Static.Gpss.GpsChanged -= new Action<long, int>(this.OnInsChanged);
      MySession.Static.Gpss.ListChanged -= new Action<long>(this.OnListChanged);
      this.UnhookSyncEvents();
      this.m_checkInsShowOnHud.IsCheckedChanged -= new Action<MyGuiControlCheckbox>(this.OnShowOnHudChecked);
      this.m_checkInsAlwaysVisible.IsCheckedChanged -= new Action<MyGuiControlCheckbox>(this.OnAlwaysVisibleChecked);
      this.m_buttonAdd.ButtonClicked -= new Action<MyGuiControlButton>(this.OnButtonPressedNew);
      this.m_buttonAddFromClipboard.ButtonClicked -= new Action<MyGuiControlButton>(this.OnButtonPressedNewFromClipboard);
      this.m_buttonAddCurrent.ButtonClicked -= new Action<MyGuiControlButton>(this.OnButtonPressedNewFromCurrent);
      this.m_buttonDelete.ButtonClicked -= new Action<MyGuiControlButton>(this.OnButtonPressedDelete);
      this.m_buttonCopy.ButtonClicked -= new Action<MyGuiControlButton>(this.OnButtonPressedCopy);
    }

    public override void HandleInput()
    {
      base.HandleInput();
      if (MyInput.Static.IsNewKeyPressed(MyKeys.Delete))
        this.OnDelKeyPressed();
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.BUTTON_Y))
        this.OnButtonPressedDelete((MyGuiControlButton) null);
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.BUTTON_X))
      {
        bool flag1 = MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MODIF_L, MyControlStateType.PRESSED);
        bool flag2 = MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MODIF_R, MyControlStateType.PRESSED);
        if (flag1 & !flag2)
          this.OnButtonPressedNew((MyGuiControlButton) null);
        else if (flag2 && !flag1)
          this.OnButtonPressedNewFromClipboard((MyGuiControlButton) null);
        else if (!flag2 && !flag1)
          this.OnButtonPressedNewFromCurrent((MyGuiControlButton) null);
      }
      if (this.m_tableIns != null && this.m_tableIns.HasFocus && MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.ACCEPT))
        MyTerminalGpsController.ToggleShowOnHud(this.m_tableIns);
      this.m_buttonAdd.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_buttonAddCurrent.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_buttonAddFromClipboard.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_buttonDelete.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_labelClipboardGamepadHelp.Visible = this.m_buttonCopy.Enabled && MyInput.Static.IsJoystickLastUsed;
    }
  }
}
