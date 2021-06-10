// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenOptionsDisplay
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
  public class MyGuiScreenOptionsDisplay : MyGuiScreenBase
  {
    private MyGuiControlLabel m_labelRecommendAspectRatio;
    private MyGuiControlLabel m_labelUnsupportedAspectRatio;
    private MyGuiControlCombobox m_comboVideoAdapter;
    private MyGuiControlCombobox m_comboResolution;
    private MyGuiControlCombobox m_comboWindowMode;
    private MyGuiControlCheckbox m_checkboxVSync;
    private MyGuiControlCheckbox m_checkboxCaptureMouse;
    private MyGuiControlCombobox m_comboScreenshotMultiplier;
    private MyRenderDeviceSettings m_settingsOld;
    private MyRenderDeviceSettings m_settingsNew;
    private bool m_waitingForConfirmation;
    private bool m_doRevert;
    private MyGuiControlElementGroup m_elementGroup;
    private MyGuiControlButton m_buttonOk;
    private MyGuiControlButton m_buttonCancel;

    public MyGuiScreenOptionsDisplay()
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.6535714f, 0.5696565f)), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      this.EnabledBackgroundFade = true;
      this.RecreateControls(true);
    }

    public override string GetFriendlyName() => "MyGuiScreenOptionsVideo";

    public override void RecreateControls(bool constructor)
    {
      if (!constructor)
        return;
      base.RecreateControls(constructor);
      this.m_elementGroup = new MyGuiControlElementGroup();
      this.AddCaption(MyCommonTexts.ScreenCaptionDisplay, captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      MyGuiControlSeparatorList controlSeparatorList1 = new MyGuiControlSeparatorList();
      controlSeparatorList1.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.83f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList1);
      MyGuiControlSeparatorList controlSeparatorList2 = new MyGuiControlSeparatorList();
      controlSeparatorList2.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.123000003397465)), this.m_size.Value.X * 0.83f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList2);
      MyGuiDrawAlignEnum guiDrawAlignEnum1 = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      MyGuiDrawAlignEnum guiDrawAlignEnum2 = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      Vector2 vector2_1 = new Vector2(90f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      Vector2 vector2_2 = new Vector2(54f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      float num1 = 455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
      float x = 25f;
      float y = MyGuiConstants.SCREEN_CAPTION_DELTA_Y * 0.5f;
      float num2 = 0.0015f;
      Vector2 vector2_3 = new Vector2(0.0f, 0.045f);
      float num3 = 0.0f;
      Vector2 vector2_4 = new Vector2(0.0f, 0.008f);
      Vector2 vector2_5 = (this.m_size.Value / 2f - vector2_1) * new Vector2(-1f, -1f) + new Vector2(0.0f, y);
      Vector2 vector2_6 = (this.m_size.Value / 2f - vector2_1) * new Vector2(1f, -1f) + new Vector2(0.0f, y);
      Vector2 vector2_7 = (this.m_size.Value / 2f - vector2_2) * new Vector2(0.0f, 1f);
      Vector2 vector2_8 = new Vector2(vector2_6.X - (num1 + num2), vector2_6.Y);
      float num4 = num3 - 0.045f;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.VideoAdapter));
      myGuiControlLabel1.Position = vector2_5 + num4 * vector2_3 + vector2_4;
      myGuiControlLabel1.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel2 = myGuiControlLabel1;
      MyGuiControlCombobox guiControlCombobox1 = new MyGuiControlCombobox(toolTip: MyTexts.GetString(MyCommonTexts.ToolTipVideoOptionsVideoAdapter));
      guiControlCombobox1.Position = vector2_6 + num4 * vector2_3;
      guiControlCombobox1.OriginAlign = guiDrawAlignEnum2;
      this.m_comboVideoAdapter = guiControlCombobox1;
      int num5 = 0;
      foreach (MyAdapterInfo adapter in MyVideoSettingsManager.Adapters)
        this.m_comboVideoAdapter.AddItem((long) num5++, new StringBuilder(adapter.Name));
      float num6 = num4 + 1f;
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.ScreenOptionsVideo_WindowMode));
      myGuiControlLabel3.Position = vector2_5 + num6 * vector2_3 + vector2_4;
      myGuiControlLabel3.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel4 = myGuiControlLabel3;
      MyGuiControlCombobox guiControlCombobox2 = new MyGuiControlCombobox(toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsDisplay_WindowMode));
      guiControlCombobox2.Position = vector2_6 + num6 * vector2_3;
      guiControlCombobox2.OriginAlign = guiDrawAlignEnum2;
      this.m_comboWindowMode = guiControlCombobox2;
      float num7 = num6 + 1f;
      MyGuiControlLabel myGuiControlLabel5 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.VideoMode));
      myGuiControlLabel5.Position = vector2_5 + num7 * vector2_3 + vector2_4;
      myGuiControlLabel5.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel6 = myGuiControlLabel5;
      MyGuiControlCombobox guiControlCombobox3 = new MyGuiControlCombobox(toolTip: MyTexts.GetString(MyCommonTexts.ToolTipVideoOptionsVideoMode));
      guiControlCombobox3.Position = vector2_6 + num7 * vector2_3;
      guiControlCombobox3.OriginAlign = guiDrawAlignEnum2;
      this.m_comboResolution = guiControlCombobox3;
      float num8 = num7 + 1f;
      MyGuiControlLabel myGuiControlLabel7 = new MyGuiControlLabel(colorMask: new Vector4?(MyGuiConstants.LABEL_TEXT_COLOR * 0.9f), textScale: 0.578f);
      myGuiControlLabel7.Position = new Vector2(vector2_6.X - (num1 - num2), vector2_6.Y) + num8 * vector2_3;
      myGuiControlLabel7.OriginAlign = guiDrawAlignEnum1;
      this.m_labelUnsupportedAspectRatio = myGuiControlLabel7;
      this.m_labelUnsupportedAspectRatio.Text = string.Format("* {0}", (object) MyTexts.Get(MyCommonTexts.UnsupportedAspectRatio));
      float num9 = num8 + 0.45f;
      MyGuiControlLabel myGuiControlLabel8 = new MyGuiControlLabel(colorMask: new Vector4?(MyGuiConstants.LABEL_TEXT_COLOR * 0.9f), textScale: 0.578f);
      myGuiControlLabel8.Position = new Vector2(vector2_6.X - (num1 - num2), vector2_6.Y) + num9 * vector2_3;
      myGuiControlLabel8.OriginAlign = guiDrawAlignEnum1;
      this.m_labelRecommendAspectRatio = myGuiControlLabel8;
      float num10 = num9 + 0.66f;
      MyGuiControlLabel myGuiControlLabel9 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.ScreenshotMultiplier));
      myGuiControlLabel9.Position = vector2_5 + num10 * vector2_3 + vector2_4;
      myGuiControlLabel9.OriginAlign = guiDrawAlignEnum1;
      myGuiControlLabel9.IsAutoEllipsisEnabled = true;
      myGuiControlLabel9.IsAutoScaleEnabled = true;
      MyGuiControlLabel myGuiControlLabel10 = myGuiControlLabel9;
      myGuiControlLabel10.SetMaxWidth(0.25f);
      MyGuiControlCombobox guiControlCombobox4 = new MyGuiControlCombobox(toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsDisplay_ScreenshotMultiplier));
      guiControlCombobox4.Position = vector2_6 + num10 * vector2_3;
      guiControlCombobox4.OriginAlign = guiDrawAlignEnum2;
      this.m_comboScreenshotMultiplier = guiControlCombobox4;
      float num11 = num10 + 1.26f;
      MyGuiControlLabel myGuiControlLabel11 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.VerticalSync));
      myGuiControlLabel11.Position = vector2_5 + num11 * vector2_3 + vector2_4;
      myGuiControlLabel11.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel12 = myGuiControlLabel11;
      MyGuiControlCheckbox guiControlCheckbox1 = new MyGuiControlCheckbox(toolTip: MyTexts.GetString(MyCommonTexts.ToolTipVideoOptionsVerticalSync));
      guiControlCheckbox1.Position = vector2_8 + num11 * vector2_3;
      guiControlCheckbox1.OriginAlign = guiDrawAlignEnum1;
      this.m_checkboxVSync = guiControlCheckbox1;
      float num12 = num11 + 1f;
      MyGuiControlLabel myGuiControlLabel13 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.CaptureMouse));
      myGuiControlLabel13.Position = vector2_5 + num12 * vector2_3 + vector2_4;
      myGuiControlLabel13.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel14 = myGuiControlLabel13;
      MyGuiControlCheckbox guiControlCheckbox2 = new MyGuiControlCheckbox(toolTip: MyTexts.GetString(MyCommonTexts.ToolTipVideoOptionsCaptureMouse));
      guiControlCheckbox2.Position = vector2_8 + num12 * vector2_3;
      guiControlCheckbox2.OriginAlign = guiDrawAlignEnum1;
      this.m_checkboxCaptureMouse = guiControlCheckbox2;
      float num13 = num12 + 1f;
      this.m_buttonOk = new MyGuiControlButton(text: MyTexts.Get(MyCommonTexts.Ok), onButtonClick: new Action<MyGuiControlButton>(this.OnOkClick));
      this.m_buttonOk.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Ok));
      this.m_buttonOk.Position = vector2_7 + new Vector2(-x, 0.0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      this.m_buttonOk.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
      this.m_buttonOk.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_buttonCancel = new MyGuiControlButton(text: MyTexts.Get(MyCommonTexts.Cancel), onButtonClick: new Action<MyGuiControlButton>(this.OnCancelClick));
      this.m_buttonCancel.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Cancel));
      this.m_buttonCancel.Position = vector2_7 + new Vector2(x, 0.0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      this.m_buttonCancel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
      this.m_buttonCancel.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      this.Controls.Add((MyGuiControlBase) this.m_comboVideoAdapter);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel6);
      this.Controls.Add((MyGuiControlBase) this.m_comboResolution);
      this.Controls.Add((MyGuiControlBase) this.m_labelUnsupportedAspectRatio);
      this.Controls.Add((MyGuiControlBase) this.m_labelRecommendAspectRatio);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel4);
      this.Controls.Add((MyGuiControlBase) this.m_comboWindowMode);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel10);
      this.Controls.Add((MyGuiControlBase) this.m_comboScreenshotMultiplier);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel14);
      this.Controls.Add((MyGuiControlBase) this.m_checkboxCaptureMouse);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel12);
      this.Controls.Add((MyGuiControlBase) this.m_checkboxVSync);
      this.Controls.Add((MyGuiControlBase) this.m_buttonOk);
      this.m_elementGroup.Add((MyGuiControlBase) this.m_buttonOk);
      this.Controls.Add((MyGuiControlBase) this.m_buttonCancel);
      this.m_elementGroup.Add((MyGuiControlBase) this.m_buttonCancel);
      Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
      MyGuiControlLabel myGuiControlLabel15 = new MyGuiControlLabel(new Vector2?(new Vector2(vector2_5.X, this.m_buttonOk.Position.Y - minSizeGui.Y / 2f)));
      myGuiControlLabel15.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel15);
      this.m_comboVideoAdapter.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.UpdateWindowModeComboBox);
      this.m_comboVideoAdapter.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.UpdateResolutionComboBox);
      this.m_comboWindowMode.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.UpdateRecommendecAspectRatioLabel);
      this.m_comboWindowMode.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.UpdateResolutionComboBox);
      this.m_settingsOld = MyVideoSettingsManager.CurrentDeviceSettings;
      this.m_settingsNew = this.m_settingsOld;
      this.WriteSettingsToControls(this.m_settingsOld);
      this.ReadSettingsFromControls(ref this.m_settingsOld);
      this.ReadSettingsFromControls(ref this.m_settingsNew);
      this.FocusedControl = (MyGuiControlBase) this.m_buttonOk;
      this.CloseButtonEnabled = true;
      this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.DisplayOptions_Help_Screen);
    }

    private MyAdapterInfo GetSelectedAdapter() => MyVideoSettingsManager.Adapters[(int) this.m_comboVideoAdapter.GetSelectedKey()];

    private MyWindowModeEnum GetSelectedWindowMode() => (MyWindowModeEnum) this.m_comboWindowMode.GetSelectedKey();

    private void SelectWindowMode(MyWindowModeEnum mode) => this.m_comboWindowMode.SelectItemByKey((long) mode);

    private long GetResolutionKey(Vector2I resolution) => (long) resolution.X << 32 | (long) resolution.Y;

    private Vector2I GetResolutionFromKey(long key) => new Vector2I((int) (key >> 32), (int) (key & (long) uint.MaxValue));

    private Vector2I GetSelectedResolution() => this.GetResolutionFromKey(this.m_comboResolution.GetSelectedKey());

    private void SelectResolution(Vector2I resolution)
    {
      int num1 = int.MaxValue;
      Vector2I resolution1 = resolution;
      for (int index = 0; index < this.m_comboResolution.GetItemsCount(); ++index)
      {
        Vector2I resolutionFromKey = this.GetResolutionFromKey(this.m_comboResolution.GetItemByIndex(index).Key);
        if (resolutionFromKey == resolution)
        {
          resolution1 = resolution;
          break;
        }
        int num2 = Math.Abs(resolutionFromKey.X * resolutionFromKey.Y - resolution.X * resolution.Y);
        if (num2 < num1)
        {
          num1 = num2;
          resolution1 = resolutionFromKey;
        }
      }
      this.m_comboResolution.SelectItemByKey(this.GetResolutionKey(resolution1));
    }

    private void UpdateAdapterComboBox()
    {
      long selectedKey = this.m_comboVideoAdapter.GetSelectedKey();
      this.m_comboVideoAdapter.ClearItems();
      int num = 0;
      foreach (MyAdapterInfo adapter in MyVideoSettingsManager.Adapters)
        this.m_comboVideoAdapter.AddItem((long) num++, new StringBuilder(adapter.Name));
      this.m_comboVideoAdapter.SelectItemByKey(selectedKey);
    }

    private void UpdateWindowModeComboBox()
    {
      MyWindowModeEnum myWindowModeEnum = (MyWindowModeEnum) this.m_comboWindowMode.GetSelectedKey();
      this.m_comboWindowMode.ClearItems();
      bool isOutputAttached = this.GetSelectedAdapter().IsOutputAttached;
      this.m_comboWindowMode.AddItem(0L, MyCommonTexts.ScreenOptionsVideo_WindowMode_Window);
      this.m_comboWindowMode.AddItem(1L, MyCommonTexts.ScreenOptionsVideo_WindowMode_FullscreenWindow);
      if (isOutputAttached)
        this.m_comboWindowMode.AddItem(2L, MyCommonTexts.ScreenOptionsVideo_WindowMode_Fullscreen);
      if (myWindowModeEnum == MyWindowModeEnum.Fullscreen && !isOutputAttached)
        myWindowModeEnum = MyWindowModeEnum.FullscreenWindow;
      this.m_comboWindowMode.SelectItemByKey((long) myWindowModeEnum);
    }

    private void UpdateResolutionComboBox()
    {
      Vector2I selectedResolution = this.GetSelectedResolution();
      MyWindowModeEnum selectedWindowMode = this.GetSelectedWindowMode();
      this.m_comboResolution.ClearItems();
      foreach (MyDisplayMode supportedDisplayMode in this.GetSelectedAdapter().SupportedDisplayModes)
      {
        Vector2I vector2I = new Vector2I(supportedDisplayMode.Width, supportedDisplayMode.Height);
        bool flag = true;
        if (selectedWindowMode == MyWindowModeEnum.Window)
        {
          Vector2I windowResolution = MyRenderProxyUtils.GetFixedWindowResolution(vector2I, this.GetSelectedAdapter());
          if (vector2I != windowResolution)
            flag = false;
        }
        if (this.m_comboResolution.TryGetItemByKey(this.GetResolutionKey(vector2I)) != null)
          flag = false;
        if (flag)
        {
          MyAspectRatio recommendedAspectRatio = MyVideoSettingsManager.GetRecommendedAspectRatio((int) this.m_comboVideoAdapter.GetSelectedKey());
          MyAspectRatioEnum closestAspectRatio = MyVideoSettingsManager.GetClosestAspectRatio((float) vector2I.X / (float) vector2I.Y);
          MyAspectRatio aspectRatio = MyVideoSettingsManager.GetAspectRatio(closestAspectRatio);
          string textShort = aspectRatio.TextShort;
          string str = aspectRatio.IsSupported ? (closestAspectRatio == recommendedAspectRatio.AspectRatioEnum ? " ***" : "") : " *";
          this.m_comboResolution.AddItem(this.GetResolutionKey(vector2I), new StringBuilder(string.Format("{0} x {1} - {2}{3}", (object) vector2I.X, (object) vector2I.Y, (object) textShort, (object) str)));
        }
      }
      this.SelectResolution(selectedResolution);
    }

    private void UpdateScreenshotMultiplierComboBox()
    {
      int selectedKey = (int) this.m_comboScreenshotMultiplier.GetSelectedKey();
      this.m_comboScreenshotMultiplier.ClearItems();
      this.m_comboScreenshotMultiplier.AddItem(1L, "1x");
      this.m_comboScreenshotMultiplier.AddItem(2L, "2x");
      this.m_comboScreenshotMultiplier.AddItem(4L, "4x");
      this.m_comboScreenshotMultiplier.AddItem(8L, "8x");
      this.m_comboScreenshotMultiplier.SelectItemByKey((long) selectedKey);
    }

    private void UpdateRecommendecAspectRatioLabel()
    {
      MyAspectRatio recommendedAspectRatio = MyVideoSettingsManager.GetRecommendedAspectRatio((int) this.m_comboVideoAdapter.GetSelectedKey());
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat(MyTexts.GetString(MyCommonTexts.RecommendedAspectRatio), (object) recommendedAspectRatio.TextShort);
      this.m_labelRecommendAspectRatio.Text = string.Format("*** {0}", (object) stringBuilder);
    }

    private bool ReadSettingsFromControls(ref MyRenderDeviceSettings deviceSettings)
    {
      bool flag1 = false;
      MyRenderDeviceSettings renderDeviceSettings = new MyRenderDeviceSettings()
      {
        AdapterOrdinal = deviceSettings.AdapterOrdinal
      };
      Vector2I selectedResolution = this.GetSelectedResolution();
      renderDeviceSettings.BackBufferWidth = selectedResolution.X;
      renderDeviceSettings.BackBufferHeight = selectedResolution.Y;
      renderDeviceSettings.WindowMode = this.GetSelectedWindowMode();
      renderDeviceSettings.NewAdapterOrdinal = (int) this.m_comboVideoAdapter.GetSelectedKey();
      bool flag2 = flag1 | renderDeviceSettings.NewAdapterOrdinal != renderDeviceSettings.AdapterOrdinal;
      renderDeviceSettings.VSync = this.m_checkboxVSync.IsChecked ? 1 : 0;
      renderDeviceSettings.RefreshRate = 0;
      if (this.m_checkboxCaptureMouse.IsChecked != MySandboxGame.Config.CaptureMouse)
      {
        MySandboxGame.Config.CaptureMouse = this.m_checkboxCaptureMouse.IsChecked;
        MySandboxGame.Static.UpdateMouseCapture();
      }
      foreach (MyDisplayMode supportedDisplayMode in MyVideoSettingsManager.Adapters[deviceSettings.AdapterOrdinal].SupportedDisplayModes)
      {
        if (supportedDisplayMode.Width == renderDeviceSettings.BackBufferWidth && supportedDisplayMode.Height == renderDeviceSettings.BackBufferHeight && renderDeviceSettings.RefreshRate < supportedDisplayMode.RefreshRate)
          renderDeviceSettings.RefreshRate = supportedDisplayMode.RefreshRate;
      }
      bool flag3 = flag2 || !renderDeviceSettings.Equals(ref deviceSettings);
      deviceSettings = renderDeviceSettings;
      return flag3;
    }

    private void WriteSettingsToControls(MyRenderDeviceSettings deviceSettings)
    {
      this.UpdateAdapterComboBox();
      this.m_comboVideoAdapter.SelectItemByKey((long) deviceSettings.NewAdapterOrdinal, false);
      this.UpdateWindowModeComboBox();
      this.UpdateResolutionComboBox();
      this.UpdateScreenshotMultiplierComboBox();
      this.m_comboScreenshotMultiplier.SelectItemByKey((long) (int) MySandboxGame.Config.ScreenshotSizeMultiplier);
      this.SelectResolution(new Vector2I(deviceSettings.BackBufferWidth, deviceSettings.BackBufferHeight));
      this.SelectWindowMode(deviceSettings.WindowMode);
      this.m_checkboxVSync.IsChecked = deviceSettings.VSync > 0;
      this.m_checkboxCaptureMouse.IsChecked = MySandboxGame.Config.CaptureMouse;
    }

    public void OnCancelClick(MyGuiControlButton sender) => this.CloseScreen(false);

    public void OnOkClick(MyGuiControlButton sender)
    {
      int num = this.ReadSettingsFromControls(ref this.m_settingsNew) ? 1 : 0;
      MySandboxGame.Config.ScreenshotSizeMultiplier = (float) this.m_comboScreenshotMultiplier.GetSelectedKey();
      if (num != 0)
        this.OnVideoModeChangedAndConfirm(MyVideoSettingsManager.Apply(this.m_settingsNew));
      else
        this.CloseScreen(false);
    }

    private void OnVideoModeChangedAndConfirm(MyVideoSettingsManager.ChangeResult result)
    {
      switch (result)
      {
        case MyVideoSettingsManager.ChangeResult.Success:
          this.m_waitingForConfirmation = true;
          StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO_TIMEOUT, messageText: MyTexts.Get(MyCommonTexts.DoYouWantToKeepTheseSettingsXSecondsRemaining), messageCaption: messageCaption, callback: new Action<MyGuiScreenMessageBox.ResultEnum>(this.OnMessageBoxCallback), timeoutInMiliseconds: 60000));
          break;
        case MyVideoSettingsManager.ChangeResult.Failed:
          this.m_doRevert = true;
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.SorryButSelectedSettingsAreNotSupportedByYourHardware), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
          break;
      }
    }

    private void OnVideoModeChanged(MyVideoSettingsManager.ChangeResult result)
    {
      this.WriteSettingsToControls(this.m_settingsOld);
      this.ReadSettingsFromControls(ref this.m_settingsNew);
    }

    public void OnMessageBoxCallback(MyGuiScreenMessageBox.ResultEnum callbackReturn)
    {
      if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
      {
        if (this.m_settingsNew.NewAdapterOrdinal != this.m_settingsNew.AdapterOrdinal)
          MySandboxGame.Config.DisableUpdateDriverNotification = false;
        MyVideoSettingsManager.SaveCurrentSettings();
        this.ReadSettingsFromControls(ref this.m_settingsOld);
        this.CloseScreenNow();
        if (this.m_settingsNew.NewAdapterOrdinal != this.m_settingsNew.AdapterOrdinal)
        {
          StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextRestartNeededAfterAdapterSwitch), messageCaption: messageCaption, callback: new Action<MyGuiScreenMessageBox.ResultEnum>(this.OnMessageBoxAdapterChangeCallback)));
        }
      }
      else
        this.m_doRevert = true;
      this.m_waitingForConfirmation = false;
    }

    public void OnMessageBoxAdapterChangeCallback(MyGuiScreenMessageBox.ResultEnum callbackReturn)
    {
      if (callbackReturn != MyGuiScreenMessageBox.ResultEnum.YES)
        return;
      MySessionLoader.ExitGame();
    }

    public override bool CloseScreen(bool isUnloading = false)
    {
      int num1 = base.CloseScreen(isUnloading) ? 1 : 0;
      if (num1 == 0)
        return num1 != 0;
      int num2 = this.m_waitingForConfirmation ? 1 : 0;
      return num1 != 0;
    }

    public override bool Draw()
    {
      if (!base.Draw())
        return false;
      if (this.m_doRevert)
      {
        this.OnVideoModeChanged(MyVideoSettingsManager.Apply(this.m_settingsOld));
        this.m_doRevert = false;
      }
      return true;
    }

    public override bool Update(bool hasFocus)
    {
      int num = base.Update(hasFocus) ? 1 : 0;
      if (!hasFocus)
        return num != 0;
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
        this.OnOkClick((MyGuiControlButton) null);
      this.m_buttonOk.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_buttonCancel.Visible = !MyInput.Static.IsJoystickLastUsed;
      return num != 0;
    }
  }
}
