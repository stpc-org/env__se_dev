// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenOptionsAudio
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Audio;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyGuiScreenOptionsAudio : MyGuiScreenBase
  {
    private MyGuiControlSlider m_gameVolumeSlider;
    private MyGuiControlSlider m_musicVolumeSlider;
    private MyGuiControlSlider m_voiceChatVolumeSlider;
    private MyGuiControlSlider m_voiceMicSensitivitySlider;
    private MyGuiControlCheckbox m_hudWarnings;
    private MyGuiControlCheckbox m_enableVoiceChat;
    private MyGuiControlCheckbox m_enableMuteWhenNotInFocus;
    private MyGuiControlCheckbox m_enableDynamicMusic;
    private MyGuiControlCheckbox m_enableReverb;
    private MyGuiControlCheckbox m_enableDoppler;
    private MyGuiControlCheckbox m_shipSoundsAreBasedOnSpeed;
    private MyGuiControlCheckbox m_pushToTalk;
    private MyGuiScreenOptionsAudio.MyGuiScreenOptionsAudioSettings m_settingsOld = new MyGuiScreenOptionsAudio.MyGuiScreenOptionsAudioSettings();
    private MyGuiScreenOptionsAudio.MyGuiScreenOptionsAudioSettings m_settingsNew = new MyGuiScreenOptionsAudio.MyGuiScreenOptionsAudioSettings();
    private bool m_gameAudioPausedWhenOpen;
    private MyGuiControlElementGroup m_elementGroup;
    private MyGuiControlButton m_buttonOk;
    private MyGuiControlButton m_buttonCancel;
    private bool m_isLimitedMenu;

    public MyGuiScreenOptionsAudio(bool isLimitedMenu = false)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(MyGuiScreenOptionsAudio.GetScreenSize(isLimitedMenu)), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      this.m_isLimitedMenu = isLimitedMenu;
      this.EnabledBackgroundFade = true;
      this.RecreateControls(true);
    }

    public override void RecreateControls(bool constructor)
    {
      if (!constructor)
        return;
      base.RecreateControls(constructor);
      this.m_elementGroup = new MyGuiControlElementGroup();
      this.AddCaption(MyCommonTexts.ScreenCaptionAudioOptions, captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
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
      float x1 = 455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
      float x2 = 25f;
      float y = MyGuiConstants.SCREEN_CAPTION_DELTA_Y * 0.5f;
      float num1 = 0.0015f;
      Vector2 vector2_3 = new Vector2(0.0f, 0.045f);
      float num2 = 0.0f;
      Vector2 vector2_4 = new Vector2(0.0f, 0.008f);
      Vector2 vector2_5 = (this.m_size.Value / 2f - vector2_1) * new Vector2(-1f, -1f) + new Vector2(0.0f, y);
      Vector2 vector2_6 = (this.m_size.Value / 2f - vector2_1) * new Vector2(1f, -1f) + new Vector2(0.0f, y);
      Vector2 vector2_7 = (this.m_size.Value / 2f - vector2_2) * new Vector2(0.0f, 1f);
      Vector2 vector2_8 = new Vector2(vector2_6.X - (x1 + num1), vector2_6.Y);
      float num3 = num2 - 0.045f;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.MusicVolume));
      myGuiControlLabel1.Position = vector2_5 + num3 * vector2_3 + vector2_4;
      myGuiControlLabel1.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel2 = myGuiControlLabel1;
      Vector2? position1 = new Vector2?();
      string str1 = MyTexts.GetString(MySpaceTexts.ToolTipOptionsAudio_MusicVolume);
      float? defaultValue1 = new float?(MySandboxGame.Config.MusicVolume);
      Vector4? color1 = new Vector4?();
      string toolTip1 = str1;
      MyGuiControlSlider guiControlSlider1 = new MyGuiControlSlider(position1, defaultValue: defaultValue1, color: color1, toolTip: toolTip1);
      guiControlSlider1.Position = vector2_6 + num3 * vector2_3;
      guiControlSlider1.OriginAlign = guiDrawAlignEnum2;
      guiControlSlider1.Size = new Vector2(x1, 0.0f);
      this.m_musicVolumeSlider = guiControlSlider1;
      this.m_musicVolumeSlider.ValueChanged = new Action<MyGuiControlSlider>(this.OnMusicVolumeChange);
      float num4 = num3 + 1.08f;
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.GameVolume));
      myGuiControlLabel3.Position = vector2_5 + num4 * vector2_3 + vector2_4;
      myGuiControlLabel3.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel4 = myGuiControlLabel3;
      Vector2? position2 = new Vector2?();
      string str2 = MyTexts.GetString(MySpaceTexts.ToolTipOptionsAudio_SoundVolume);
      float? defaultValue2 = new float?(MySandboxGame.Config.GameVolume);
      Vector4? color2 = new Vector4?();
      string toolTip2 = str2;
      MyGuiControlSlider guiControlSlider2 = new MyGuiControlSlider(position2, defaultValue: defaultValue2, color: color2, toolTip: toolTip2);
      guiControlSlider2.Position = vector2_6 + num4 * vector2_3;
      guiControlSlider2.OriginAlign = guiDrawAlignEnum2;
      guiControlSlider2.Size = new Vector2(x1, 0.0f);
      this.m_gameVolumeSlider = guiControlSlider2;
      this.m_gameVolumeSlider.ValueChanged = new Action<MyGuiControlSlider>(this.OnGameVolumeChange);
      float num5 = num4 + 1.08f;
      MyGuiControlLabel myGuiControlLabel5 = (MyGuiControlLabel) null;
      MyGuiControlLabel myGuiControlLabel6 = (MyGuiControlLabel) null;
      MyGuiControlLabel myGuiControlLabel7 = (MyGuiControlLabel) null;
      MyGuiControlLabel myGuiControlLabel8 = (MyGuiControlLabel) null;
      if (MyPerGameSettings.VoiceChatEnabled)
      {
        float num6 = num5 + 0.29f;
        if (!this.m_isLimitedMenu)
        {
          MyGuiControlLabel myGuiControlLabel9 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.EnableVoiceChat));
          myGuiControlLabel9.Position = vector2_5 + num6 * vector2_3 + vector2_4;
          myGuiControlLabel9.OriginAlign = guiDrawAlignEnum1;
          myGuiControlLabel5 = myGuiControlLabel9;
          MyGuiControlCheckbox guiControlCheckbox = new MyGuiControlCheckbox(toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsAudio_EnableVoiceChat));
          guiControlCheckbox.Position = vector2_8 + num6 * vector2_3;
          guiControlCheckbox.OriginAlign = guiDrawAlignEnum1;
          this.m_enableVoiceChat = guiControlCheckbox;
          this.m_enableVoiceChat.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.VoiceChatChecked);
          ++num6;
        }
        MyGuiControlLabel myGuiControlLabel10 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.VoiceChatVolume));
        myGuiControlLabel10.Position = vector2_5 + num6 * vector2_3 + vector2_4;
        myGuiControlLabel10.OriginAlign = guiDrawAlignEnum1;
        myGuiControlLabel6 = myGuiControlLabel10;
        Vector2? position3 = new Vector2?();
        string str3 = MyTexts.GetString(MySpaceTexts.ToolTipOptionsAudio_VoiceChatVolume);
        float? defaultValue3 = new float?(MySandboxGame.Config.VoiceChatVolume);
        Vector4? color3 = new Vector4?();
        string toolTip3 = str3;
        MyGuiControlSlider guiControlSlider3 = new MyGuiControlSlider(position3, maxValue: 5f, defaultValue: defaultValue3, color: color3, toolTip: toolTip3);
        guiControlSlider3.Position = vector2_6 + num6 * vector2_3;
        guiControlSlider3.OriginAlign = guiDrawAlignEnum2;
        guiControlSlider3.Size = new Vector2(x1, 0.0f);
        this.m_voiceChatVolumeSlider = guiControlSlider3;
        this.m_voiceChatVolumeSlider.ValueChanged = new Action<MyGuiControlSlider>(this.OnVoiceChatVolumeChange);
        float num7 = num6 + 1.08f;
        MyGuiControlLabel myGuiControlLabel11 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.PushToTalk));
        myGuiControlLabel11.Position = vector2_5 + num7 * vector2_3 + vector2_4;
        myGuiControlLabel11.OriginAlign = guiDrawAlignEnum1;
        myGuiControlLabel7 = myGuiControlLabel11;
        MyGuiControlCheckbox guiControlCheckbox1 = new MyGuiControlCheckbox(toolTip: MyTexts.GetString(MyCommonTexts.PushToTalk));
        guiControlCheckbox1.Position = vector2_8 + num7 * vector2_3;
        guiControlCheckbox1.OriginAlign = guiDrawAlignEnum1;
        this.m_pushToTalk = guiControlCheckbox1;
        this.m_pushToTalk.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.PushToTalkChecked);
        num5 = num7 + 1f;
        if (!MyGameService.IsMicrophoneFilteringSilence())
        {
          MyGuiControlLabel myGuiControlLabel9 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.MicSensitivity));
          myGuiControlLabel9.Position = vector2_5 + num5 * vector2_3 + vector2_4;
          myGuiControlLabel9.OriginAlign = guiDrawAlignEnum1;
          myGuiControlLabel8 = myGuiControlLabel9;
          Vector2? position4 = new Vector2?();
          string str4 = MyTexts.GetString(MyCommonTexts.MicSensitivity);
          float? defaultValue4 = new float?(MySandboxGame.Config.MicSensitivity);
          Vector4? color4 = new Vector4?();
          string toolTip4 = str4;
          MyGuiControlSlider guiControlSlider4 = new MyGuiControlSlider(position4, defaultValue: defaultValue4, color: color4, toolTip: toolTip4);
          guiControlSlider4.Position = vector2_6 + num5 * vector2_3;
          guiControlSlider4.OriginAlign = guiDrawAlignEnum2;
          guiControlSlider4.Size = new Vector2(x1, 0.0f);
          this.m_voiceMicSensitivitySlider = guiControlSlider4;
          this.m_voiceMicSensitivitySlider.ValueChanged = new Action<MyGuiControlSlider>(this.OnVoiceMicSensitivityChange);
          num5 += 1.08f;
        }
      }
      MyGuiControlLabel myGuiControlLabel12 = (MyGuiControlLabel) null;
      if (!this.m_isLimitedMenu)
      {
        MyGuiControlLabel myGuiControlLabel9 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.MuteWhenNotInFocus));
        myGuiControlLabel9.Position = vector2_5 + num5 * vector2_3 + vector2_4;
        myGuiControlLabel9.OriginAlign = guiDrawAlignEnum1;
        myGuiControlLabel12 = myGuiControlLabel9;
        MyGuiControlCheckbox guiControlCheckbox = new MyGuiControlCheckbox(toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsAudio_MuteWhenInactive));
        guiControlCheckbox.Position = vector2_8 + num5 * vector2_3;
        guiControlCheckbox.OriginAlign = guiDrawAlignEnum1;
        this.m_enableMuteWhenNotInFocus = guiControlCheckbox;
        this.m_enableMuteWhenNotInFocus.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.EnableMuteWhenNotInFocusChecked);
        ++num5;
      }
      MyGuiControlLabel myGuiControlLabel13 = (MyGuiControlLabel) null;
      if (MyPerGameSettings.UseReverbEffect && MyFakes.AUDIO_ENABLE_REVERB)
      {
        MyGuiControlLabel myGuiControlLabel9 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.AudioSettings_EnableReverb));
        myGuiControlLabel9.Position = vector2_5 + num5 * vector2_3 + vector2_4;
        myGuiControlLabel9.OriginAlign = guiDrawAlignEnum1;
        myGuiControlLabel13 = myGuiControlLabel9;
        MyGuiControlCheckbox guiControlCheckbox = new MyGuiControlCheckbox(toolTip: MyTexts.GetString(MySpaceTexts.ToolTipAudioOptionsEnableReverb));
        guiControlCheckbox.Position = vector2_8 + num5 * vector2_3;
        guiControlCheckbox.OriginAlign = guiDrawAlignEnum1;
        this.m_enableReverb = guiControlCheckbox;
        this.m_enableReverb.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.EnableReverbChecked);
        this.m_enableReverb.Enabled = MyAudio.Static.SampleRate <= MyAudio.MAX_SAMPLE_RATE;
        this.m_enableReverb.IsChecked = MyAudio.Static.EnableReverb && MyAudio.Static.SampleRate <= MyAudio.MAX_SAMPLE_RATE;
        ++num5;
      }
      MyGuiControlLabel myGuiControlLabel14 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.AudioSettings_EnableDoppler));
      myGuiControlLabel14.Position = vector2_5 + num5 * vector2_3 + vector2_4;
      myGuiControlLabel14.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel15 = myGuiControlLabel14;
      MyGuiControlCheckbox guiControlCheckbox2 = new MyGuiControlCheckbox(toolTip: MyTexts.GetString(MyCommonTexts.ToolTipAudioOptionsEnableDoppler));
      guiControlCheckbox2.Position = vector2_8 + num5 * vector2_3;
      guiControlCheckbox2.OriginAlign = guiDrawAlignEnum1;
      this.m_enableDoppler = guiControlCheckbox2;
      this.m_enableDoppler.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.EnableDopplerChecked);
      this.m_enableDoppler.Enabled = true;
      this.m_enableDoppler.IsChecked = MyAudio.Static.EnableDoppler;
      float num8 = num5 + 1f;
      MyGuiControlLabel myGuiControlLabel16 = (MyGuiControlLabel) null;
      if (MyPerGameSettings.EnableShipSoundSystem)
      {
        MyGuiControlLabel myGuiControlLabel9 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.AudioSettings_ShipSoundsBasedOnSpeed), isAutoEllipsisEnabled: true, maxWidth: 0.253f, isAutoScaleEnabled: true);
        myGuiControlLabel9.Position = vector2_5 + num8 * vector2_3 + vector2_4;
        myGuiControlLabel9.OriginAlign = guiDrawAlignEnum1;
        myGuiControlLabel16 = myGuiControlLabel9;
        MyGuiControlCheckbox guiControlCheckbox1 = new MyGuiControlCheckbox(toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsAudio_SpeedBasedSounds));
        guiControlCheckbox1.Position = vector2_8 + num8 * vector2_3;
        guiControlCheckbox1.OriginAlign = guiDrawAlignEnum1;
        this.m_shipSoundsAreBasedOnSpeed = guiControlCheckbox1;
        this.m_shipSoundsAreBasedOnSpeed.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.ShipSoundsAreBasedOnSpeedChecked);
        ++num8;
      }
      MyGuiControlLabel myGuiControlLabel17 = (MyGuiControlLabel) null;
      if (MyPerGameSettings.UseMusicController)
      {
        MyGuiControlLabel myGuiControlLabel9 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.AudioSettings_UseMusicController), isAutoEllipsisEnabled: true, maxWidth: 0.253f, isAutoScaleEnabled: true);
        myGuiControlLabel9.Position = vector2_5 + num8 * vector2_3 + vector2_4;
        myGuiControlLabel9.OriginAlign = guiDrawAlignEnum1;
        myGuiControlLabel17 = myGuiControlLabel9;
        MyGuiControlCheckbox guiControlCheckbox1 = new MyGuiControlCheckbox(toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsAudio_UseContextualMusic));
        guiControlCheckbox1.Position = vector2_8 + num8 * vector2_3;
        guiControlCheckbox1.OriginAlign = guiDrawAlignEnum1;
        this.m_enableDynamicMusic = guiControlCheckbox1;
        ++num8;
      }
      MyGuiControlLabel myGuiControlLabel18 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.HudWarnings));
      myGuiControlLabel18.Position = vector2_5 + num8 * vector2_3 + vector2_4;
      myGuiControlLabel18.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel19 = myGuiControlLabel18;
      MyGuiControlCheckbox guiControlCheckbox3 = new MyGuiControlCheckbox(toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsAudio_HudWarnings));
      guiControlCheckbox3.Position = vector2_8 + num8 * vector2_3;
      guiControlCheckbox3.OriginAlign = guiDrawAlignEnum1;
      this.m_hudWarnings = guiControlCheckbox3;
      this.m_hudWarnings.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.HudWarningsChecked);
      float num9 = num8 + 1f;
      this.m_buttonOk = new MyGuiControlButton(text: MyTexts.Get(MyCommonTexts.Ok), onButtonClick: new Action<MyGuiControlButton>(this.OnOkClick));
      this.m_buttonOk.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Ok));
      this.m_buttonOk.Position = vector2_7 + new Vector2(-x2, 0.0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      this.m_buttonOk.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
      this.m_buttonOk.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_buttonCancel = new MyGuiControlButton(text: MyTexts.Get(MyCommonTexts.Cancel), onButtonClick: new Action<MyGuiControlButton>(this.OnCancelClick));
      this.m_buttonCancel.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Cancel));
      this.m_buttonCancel.Position = vector2_7 + new Vector2(x2, 0.0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      this.m_buttonCancel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
      this.m_buttonCancel.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel4);
      this.Controls.Add((MyGuiControlBase) this.m_gameVolumeSlider);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      this.Controls.Add((MyGuiControlBase) this.m_musicVolumeSlider);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel19);
      this.Controls.Add((MyGuiControlBase) this.m_hudWarnings);
      if (!this.m_isLimitedMenu)
      {
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel12);
        this.Controls.Add((MyGuiControlBase) this.m_enableMuteWhenNotInFocus);
      }
      if (MyPerGameSettings.UseMusicController)
      {
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel17);
        this.Controls.Add((MyGuiControlBase) this.m_enableDynamicMusic);
      }
      if (MyPerGameSettings.EnableShipSoundSystem)
      {
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel16);
        this.Controls.Add((MyGuiControlBase) this.m_shipSoundsAreBasedOnSpeed);
      }
      if (MyPerGameSettings.UseReverbEffect && MyFakes.AUDIO_ENABLE_REVERB)
      {
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel13);
        this.Controls.Add((MyGuiControlBase) this.m_enableReverb);
      }
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel15);
      this.Controls.Add((MyGuiControlBase) this.m_enableDoppler);
      if (MyPerGameSettings.VoiceChatEnabled)
      {
        if (myGuiControlLabel5 != null)
          this.Controls.Add((MyGuiControlBase) myGuiControlLabel5);
        if (this.m_enableVoiceChat != null)
          this.Controls.Add((MyGuiControlBase) this.m_enableVoiceChat);
        if (this.m_voiceMicSensitivitySlider != null)
        {
          this.Controls.Add((MyGuiControlBase) this.m_voiceMicSensitivitySlider);
          this.Controls.Add((MyGuiControlBase) myGuiControlLabel8);
        }
        this.Controls.Add((MyGuiControlBase) this.m_pushToTalk);
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel6);
        this.Controls.Add((MyGuiControlBase) this.m_voiceChatVolumeSlider);
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel7);
      }
      this.Controls.Add((MyGuiControlBase) this.m_buttonOk);
      this.m_elementGroup.Add((MyGuiControlBase) this.m_buttonOk);
      this.Controls.Add((MyGuiControlBase) this.m_buttonCancel);
      this.m_elementGroup.Add((MyGuiControlBase) this.m_buttonCancel);
      Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
      MyGuiControlLabel myGuiControlLabel20 = new MyGuiControlLabel(new Vector2?(new Vector2(vector2_5.X, this.m_buttonOk.Position.Y - minSizeGui.Y / 2f)));
      myGuiControlLabel20.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel20);
      this.UpdateFromConfig(this.m_settingsOld);
      this.UpdateFromConfig(this.m_settingsNew);
      this.UpdateControls(this.m_settingsOld);
      this.FocusedControl = (MyGuiControlBase) this.m_buttonOk;
      this.CloseButtonEnabled = true;
      this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.AudioOptions_Help_Screen);
      this.m_gameAudioPausedWhenOpen = MyAudio.Static.GameSoundIsPaused;
      if (!this.m_gameAudioPausedWhenOpen)
        return;
      MyAudio.Static.ResumeGameSounds();
    }

    private void VoiceChatChecked(MyGuiControlCheckbox checkbox) => this.m_settingsNew.EnableVoiceChat = checkbox.IsChecked;

    private void PushToTalkChecked(MyGuiControlCheckbox checkbox)
    {
      this.m_settingsNew.PushToTalk = checkbox.IsChecked;
      if (this.m_voiceMicSensitivitySlider == null)
        return;
      this.m_voiceMicSensitivitySlider.Enabled = !this.m_settingsNew.PushToTalk;
    }

    private void HudWarningsChecked(MyGuiControlCheckbox obj) => this.m_settingsNew.HudWarnings = obj.IsChecked;

    private void EnableMuteWhenNotInFocusChecked(MyGuiControlCheckbox obj) => this.m_settingsNew.EnableMuteWhenNotInFocus = obj.IsChecked;

    private void EnableDynamicMusicChecked(MyGuiControlCheckbox obj) => this.m_settingsNew.EnableDynamicMusic = obj.IsChecked;

    private void ShipSoundsAreBasedOnSpeedChecked(MyGuiControlCheckbox obj) => this.m_settingsNew.ShipSoundsAreBasedOnSpeed = obj.IsChecked;

    private void EnableReverbChecked(MyGuiControlCheckbox obj) => this.m_settingsNew.EnableReverb = MyFakes.AUDIO_ENABLE_REVERB && MyAudio.Static.SampleRate <= MyAudio.MAX_SAMPLE_RATE && obj.IsChecked;

    private void EnableDopplerChecked(MyGuiControlCheckbox obj) => this.m_settingsNew.EnableDoppler = obj.IsChecked;

    public override string GetFriendlyName() => nameof (MyGuiScreenOptionsAudio);

    private void UpdateFromConfig(
      MyGuiScreenOptionsAudio.MyGuiScreenOptionsAudioSettings settings)
    {
      settings.GameVolume = MySandboxGame.Config.GameVolume;
      settings.MusicVolume = MySandboxGame.Config.MusicVolume;
      settings.VoiceChatVolume = MySandboxGame.Config.VoiceChatVolume;
      settings.HudWarnings = MySandboxGame.Config.HudWarnings;
      settings.EnableVoiceChat = MySandboxGame.Config.EnableVoiceChat;
      settings.EnableMuteWhenNotInFocus = MySandboxGame.Config.EnableMuteWhenNotInFocus;
      settings.EnableReverb = MyFakes.AUDIO_ENABLE_REVERB && MySandboxGame.Config.EnableReverb && MyAudio.Static.SampleRate <= MyAudio.MAX_SAMPLE_RATE;
      settings.EnableDynamicMusic = MySandboxGame.Config.EnableDynamicMusic;
      settings.ShipSoundsAreBasedOnSpeed = MySandboxGame.Config.ShipSoundsAreBasedOnSpeed;
      settings.EnableDoppler = MySandboxGame.Config.EnableDoppler;
      settings.MicSensitivity = MySandboxGame.Config.MicSensitivity;
      settings.PushToTalk = !MySandboxGame.Config.AutomaticVoiceChatActivation;
    }

    private void UpdateControls(
      MyGuiScreenOptionsAudio.MyGuiScreenOptionsAudioSettings settings)
    {
      this.m_gameVolumeSlider.Value = settings.GameVolume;
      this.m_musicVolumeSlider.Value = settings.MusicVolume;
      this.m_voiceChatVolumeSlider.Value = settings.VoiceChatVolume;
      this.m_hudWarnings.IsChecked = settings.HudWarnings;
      if (this.m_enableVoiceChat != null)
        this.m_enableVoiceChat.IsChecked = settings.EnableVoiceChat;
      if (this.m_voiceMicSensitivitySlider != null)
      {
        this.m_voiceMicSensitivitySlider.Value = settings.MicSensitivity;
        this.m_voiceMicSensitivitySlider.Enabled = !settings.PushToTalk;
      }
      if (this.m_pushToTalk != null)
        this.m_pushToTalk.IsChecked = settings.PushToTalk;
      if (this.m_enableMuteWhenNotInFocus != null)
        this.m_enableMuteWhenNotInFocus.IsChecked = settings.EnableMuteWhenNotInFocus;
      if (MyFakes.AUDIO_ENABLE_REVERB)
        this.m_enableReverb.IsChecked = settings.EnableReverb;
      this.m_enableDynamicMusic.IsChecked = settings.EnableDynamicMusic;
      this.m_shipSoundsAreBasedOnSpeed.IsChecked = settings.ShipSoundsAreBasedOnSpeed;
      this.m_enableDoppler.IsChecked = settings.EnableDoppler;
    }

    private void Save()
    {
      MySandboxGame.Config.GameVolume = MyAudio.Static.VolumeGame;
      MySandboxGame.Config.MusicVolume = MyAudio.Static.VolumeMusic;
      MySandboxGame.Config.VoiceChatVolume = this.m_voiceChatVolumeSlider.Value;
      MySandboxGame.Config.HudWarnings = this.m_hudWarnings.IsChecked;
      MySandboxGame.Config.MicSensitivity = MyFakes.VOICE_CHAT_MIC_SENSITIVITY;
      if (this.m_enableVoiceChat != null)
        MySandboxGame.Config.EnableVoiceChat = this.m_enableVoiceChat.IsChecked;
      if (this.m_pushToTalk != null)
        MySandboxGame.Config.AutomaticVoiceChatActivation = !this.m_pushToTalk.IsChecked;
      if (this.m_enableMuteWhenNotInFocus != null)
        MySandboxGame.Config.EnableMuteWhenNotInFocus = this.m_enableMuteWhenNotInFocus.IsChecked;
      MySandboxGame.Config.EnableReverb = MyFakes.AUDIO_ENABLE_REVERB && this.m_enableReverb.IsChecked && MyAudio.Static.SampleRate <= MyAudio.MAX_SAMPLE_RATE;
      MyAudio.Static.EnableReverb = MySandboxGame.Config.EnableReverb;
      MySandboxGame.Config.EnableDynamicMusic = this.m_enableDynamicMusic.IsChecked;
      MySandboxGame.Config.ShipSoundsAreBasedOnSpeed = this.m_shipSoundsAreBasedOnSpeed.IsChecked;
      MySandboxGame.Config.EnableDoppler = this.m_enableDoppler.IsChecked;
      MyAudio.Static.EnableDoppler = MySandboxGame.Config.EnableDoppler;
      MySandboxGame.Config.Save();
      if (MySession.Static == null || MyGuiScreenGamePlay.Static == null)
        return;
      if (MySandboxGame.Config.EnableDynamicMusic && MyMusicController.Static == null)
      {
        MyMusicController.Static = new MyMusicController(MyAudio.Static.GetAllMusicCues());
        MyMusicController.Static.Active = true;
        MyAudio.Static.MusicAllowed = false;
        MyAudio.Static.StopMusic();
      }
      else if (!MySandboxGame.Config.EnableDynamicMusic && MyMusicController.Static != null)
      {
        MyMusicController.Static.Unload();
        MyMusicController.Static = (MyMusicController) null;
        MyAudio.Static.MusicAllowed = true;
        MyAudio.Static.PlayMusic();
      }
      if (!MyFakes.AUDIO_ENABLE_REVERB || MyAudio.Static == null || (MyAudio.Static.EnableReverb == this.m_enableReverb.IsChecked || MyAudio.Static.SampleRate > MyAudio.MAX_SAMPLE_RATE))
        return;
      MyAudio.Static.EnableReverb = this.m_enableReverb.IsChecked;
    }

    private static void UpdateValues(
      MyGuiScreenOptionsAudio.MyGuiScreenOptionsAudioSettings settings)
    {
      MyAudio.Static.VolumeGame = settings.GameVolume;
      MyAudio.Static.VolumeMusic = settings.MusicVolume;
      MyAudio.Static.VolumeVoiceChat = settings.VoiceChatVolume;
      MyAudio.Static.VolumeHud = MyAudio.Static.VolumeGame;
      MyAudio.Static.EnableVoiceChat = settings.EnableVoiceChat;
      MyGuiAudio.HudWarnings = settings.HudWarnings;
      MyFakes.VOICE_CHAT_MIC_SENSITIVITY = settings.MicSensitivity;
      MyPlatformGameSettings.VOICE_CHAT_AUTOMATIC_ACTIVATION = !settings.PushToTalk;
    }

    public void OnOkClick(MyGuiControlButton sender)
    {
      this.Save();
      this.CloseScreen(false);
    }

    public void OnCancelClick(MyGuiControlButton sender)
    {
      MyGuiScreenOptionsAudio.UpdateValues(this.m_settingsOld);
      this.CloseScreen(false);
    }

    private void OnGameVolumeChange(MyGuiControlSlider sender)
    {
      this.m_settingsNew.GameVolume = this.m_gameVolumeSlider.Value;
      MyGuiScreenOptionsAudio.UpdateValues(this.m_settingsNew);
    }

    private void OnMusicVolumeChange(MyGuiControlSlider sender)
    {
      this.m_settingsNew.MusicVolume = this.m_musicVolumeSlider.Value;
      MyGuiScreenOptionsAudio.UpdateValues(this.m_settingsNew);
    }

    private void OnVoiceChatVolumeChange(MyGuiControlSlider sender)
    {
      this.m_settingsNew.VoiceChatVolume = this.m_voiceChatVolumeSlider.Value;
      MyGuiScreenOptionsAudio.UpdateValues(this.m_settingsNew);
    }

    private void OnVoiceMicSensitivityChange(MyGuiControlSlider sender)
    {
      this.m_settingsNew.MicSensitivity = this.m_voiceMicSensitivitySlider.Value;
      MyGuiScreenOptionsAudio.UpdateValues(this.m_settingsNew);
    }

    public override bool CloseScreen(bool isUnloading = false)
    {
      this.UpdateFromConfig(this.m_settingsOld);
      MyGuiScreenOptionsAudio.UpdateValues(this.m_settingsOld);
      if (this.m_gameAudioPausedWhenOpen)
        MyAudio.Static.PauseGameSounds();
      return base.CloseScreen(isUnloading);
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

    private static Vector2 GetScreenSize(bool isLimited)
    {
      Vector2 vector2 = new Vector2(0.6535714f, 0.6679389f);
      if (isLimited)
        vector2 -= new Vector2(0.0f, 0.07633588f);
      if (MyPerGameSettings.VoiceChatEnabled)
      {
        if (!MyGameService.IsMicrophoneFilteringSilence())
          vector2 += new Vector2(0.0f, 0.05f);
        vector2 += new Vector2(0.0f, 0.03f);
      }
      return vector2;
    }

    private class MyGuiScreenOptionsAudioSettings
    {
      public float GameVolume;
      public float MusicVolume;
      public float VoiceChatVolume;
      public float MicSensitivity;
      public bool HudWarnings;
      public bool EnableVoiceChat;
      public bool EnableMuteWhenNotInFocus;
      public bool EnableDynamicMusic;
      public bool EnableReverb;
      public bool ShipSoundsAreBasedOnSpeed;
      public bool EnableDoppler;
      public bool PushToTalk;

      public override bool Equals(object obj)
      {
        if (obj.GetType() != typeof (MyGuiScreenOptionsAudio.MyGuiScreenOptionsAudioSettings))
          return false;
        MyGuiScreenOptionsAudio.MyGuiScreenOptionsAudioSettings optionsAudioSettings = (MyGuiScreenOptionsAudio.MyGuiScreenOptionsAudioSettings) obj;
        return (double) this.GameVolume == (double) optionsAudioSettings.GameVolume && (double) this.MusicVolume == (double) optionsAudioSettings.MusicVolume && ((double) this.VoiceChatVolume == (double) optionsAudioSettings.VoiceChatVolume && this.HudWarnings == optionsAudioSettings.HudWarnings) && (this.EnableVoiceChat == optionsAudioSettings.EnableVoiceChat && this.EnableMuteWhenNotInFocus == optionsAudioSettings.EnableMuteWhenNotInFocus && (this.EnableDynamicMusic == optionsAudioSettings.EnableDynamicMusic && this.EnableReverb == optionsAudioSettings.EnableReverb)) && (this.ShipSoundsAreBasedOnSpeed == optionsAudioSettings.ShipSoundsAreBasedOnSpeed && this.EnableDoppler == optionsAudioSettings.EnableDoppler && this.PushToTalk == optionsAudioSettings.PushToTalk);
      }
    }
  }
}
