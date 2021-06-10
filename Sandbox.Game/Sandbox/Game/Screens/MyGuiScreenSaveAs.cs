// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenSaveAs
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Engine.Networking;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using VRage;
using VRage.Game;
using VRage.GameServices;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenSaveAs : MyGuiScreenBase
  {
    private MyGuiControlTextbox m_nameTextbox;
    private MyGuiControlButton m_okButton;
    private MyGuiControlButton m_cancelButton;
    private MyWorldInfo m_copyFrom;
    private List<string> m_existingSessionNames;
    private string m_sessionPath;
    private bool m_fromMainMenu;
    private bool m_isCloud;

    public event Action<CloudResult> SaveAsConfirm;

    public MyGuiScreenSaveAs(
      MyWorldInfo copyFrom,
      string sessionPath,
      List<string> existingSessionNames,
      bool isCloud)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.4971429f, 0.2805344f)), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      this.EnabledBackgroundFade = true;
      this.AddCaption(MyCommonTexts.ScreenCaptionSaveAs, captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      MyGuiControlSeparatorList controlSeparatorList1 = new MyGuiControlSeparatorList();
      controlSeparatorList1.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.779999971389771 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.78f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList1);
      MyGuiControlSeparatorList controlSeparatorList2 = new MyGuiControlSeparatorList();
      controlSeparatorList2.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.779999971389771 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.123000003397465)), this.m_size.Value.X * 0.78f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList2);
      this.m_nameTextbox = new MyGuiControlTextbox(new Vector2?(new Vector2(0.0f, -0.027f)), copyFrom.SessionName, 90);
      this.m_nameTextbox.Size = new Vector2(0.385f, 1f);
      this.m_okButton = new MyGuiControlButton(originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER, text: MyTexts.Get(MyCommonTexts.Ok), onButtonClick: new Action<MyGuiControlButton>(this.OnOkButtonClick));
      this.m_cancelButton = new MyGuiControlButton(originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, text: MyTexts.Get(MyCommonTexts.Cancel), onButtonClick: new Action<MyGuiControlButton>(this.OnCancelButtonClick));
      Vector2 vector2_1 = new Vector2(1f / 500f, (float) ((double) this.m_size.Value.Y / 2.0 - 0.0710000023245811));
      Vector2 vector2_2 = new Vector2(0.018f, 0.0f);
      this.m_okButton.Position = vector2_1 - vector2_2;
      this.m_cancelButton.Position = vector2_1 + vector2_2;
      this.m_okButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipNewsletter_Ok));
      this.m_cancelButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Cancel));
      this.m_nameTextbox.SetToolTip(string.Format(MyTexts.GetString(MyCommonTexts.ToolTipWorldSettingsName), (object) 5, (object) 90));
      this.Controls.Add((MyGuiControlBase) this.m_nameTextbox);
      this.Controls.Add((MyGuiControlBase) this.m_okButton);
      this.Controls.Add((MyGuiControlBase) this.m_cancelButton);
      this.m_nameTextbox.MoveCarriageToEnd();
      this.m_copyFrom = copyFrom;
      this.m_sessionPath = sessionPath;
      this.m_existingSessionNames = existingSessionNames;
      this.CloseButtonEnabled = true;
      this.OnEnterCallback = new Action(this.OnEnterPressed);
      this.m_isCloud = isCloud;
      this.CreateGamepadHelp();
    }

    private void CreateGamepadHelp()
    {
      Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(new Vector2(this.m_okButton.Position.X - minSizeGui.X / 2f, this.m_okButton.Position.Y - minSizeGui.Y / 2f)));
      myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.SaveAs_Help_Screen);
    }

    public MyGuiScreenSaveAs(string sessionName)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.4971429f, 0.2805344f)), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      this.EnabledBackgroundFade = true;
      this.AddCaption(MyCommonTexts.ScreenCaptionSaveAs, captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      MyGuiControlSeparatorList controlSeparatorList1 = new MyGuiControlSeparatorList();
      controlSeparatorList1.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.779999971389771 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.78f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList1);
      MyGuiControlSeparatorList controlSeparatorList2 = new MyGuiControlSeparatorList();
      controlSeparatorList2.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.779999971389771 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.122000001370907)), this.m_size.Value.X * 0.78f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList2);
      this.m_existingSessionNames = (List<string>) null;
      this.m_fromMainMenu = true;
      this.m_nameTextbox = new MyGuiControlTextbox(new Vector2?(new Vector2(0.0f, -0.027f)), sessionName, 90);
      this.m_nameTextbox.Size = new Vector2(0.385f, 1f);
      this.m_okButton = new MyGuiControlButton(originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, text: MyTexts.Get(MyCommonTexts.Ok), onButtonClick: new Action<MyGuiControlButton>(this.OnOkButtonClick));
      this.m_cancelButton = new MyGuiControlButton(originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM, text: MyTexts.Get(MyCommonTexts.Cancel), onButtonClick: new Action<MyGuiControlButton>(this.OnCancelButtonClick));
      Vector2 vector2_1 = new Vector2(1f / 500f, (float) ((double) this.m_size.Value.Y / 2.0 - 0.0450000017881393));
      Vector2 vector2_2 = new Vector2(0.018f, 0.0f);
      this.m_okButton.Position = vector2_1 - vector2_2;
      this.m_cancelButton.Position = vector2_1 + vector2_2;
      this.m_okButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipNewsletter_Ok));
      this.m_cancelButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Cancel));
      this.m_nameTextbox.SetToolTip(string.Format(MyTexts.GetString(MyCommonTexts.ToolTipWorldSettingsName), (object) 5, (object) 90));
      this.Controls.Add((MyGuiControlBase) this.m_nameTextbox);
      this.Controls.Add((MyGuiControlBase) this.m_okButton);
      this.Controls.Add((MyGuiControlBase) this.m_cancelButton);
      this.m_nameTextbox.MoveCarriageToEnd();
      this.CloseButtonEnabled = true;
      this.OnEnterCallback = new Action(this.OnEnterPressed);
      this.CreateGamepadHelp();
    }

    public override void HandleUnhandledInput(bool receivedFocusInThisUpdate)
    {
      base.HandleUnhandledInput(receivedFocusInThisUpdate);
      if (!MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.BUTTON_X))
        return;
      this.TrySaveAs();
    }

    public override bool Update(bool hasFocus)
    {
      int num = base.Update(hasFocus) ? 1 : 0;
      if (!hasFocus)
        return num != 0;
      this.m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_cancelButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      return num != 0;
    }

    private void OnEnterPressed() => this.TrySaveAs();

    public override string GetFriendlyName() => nameof (MyGuiScreenSaveAs);

    private void OnCancelButtonClick(MyGuiControlButton sender) => this.CloseScreen();

    private void OnOkButtonClick(MyGuiControlButton sender) => this.TrySaveAs();

    private bool TrySaveAs()
    {
      MyStringId? nullable = new MyStringId?();
      if (this.m_nameTextbox.Text.ToString().Replace(':', '-').IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
        nullable = new MyStringId?(MyCommonTexts.ErrorNameInvalid);
      else if (this.m_nameTextbox.Text.Length < 5)
        nullable = new MyStringId?(MyCommonTexts.ErrorNameTooShort);
      else if (this.m_nameTextbox.Text.Length > 128)
        nullable = new MyStringId?(MyCommonTexts.ErrorNameTooLong);
      if (this.m_existingSessionNames != null)
      {
        foreach (string existingSessionName in this.m_existingSessionNames)
        {
          if (existingSessionName == this.m_nameTextbox.Text)
            nullable = new MyStringId?(MyCommonTexts.ErrorNameAlreadyExists);
        }
      }
      if (nullable.HasValue)
      {
        MyGuiScreenMessageBox messageBox = MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(nullable.Value), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
        messageBox.SkipTransition = true;
        messageBox.InstantClose = false;
        MyGuiSandbox.AddScreen((MyGuiScreenBase) messageBox);
        return false;
      }
      if (this.m_fromMainMenu)
      {
        string customName = MyUtils.StripInvalidChars(this.m_nameTextbox.Text);
        if (string.IsNullOrWhiteSpace(customName))
          customName = MyLocalCache.GetSessionSavesPath(customName + MyUtils.GetRandomInt(int.MaxValue).ToString("########"), false, false);
        MyAsyncSaving.Start(customName: customName);
        MySession.Static.Name = this.m_nameTextbox.Text;
        this.CloseScreen();
        return true;
      }
      this.m_copyFrom.SessionName = this.m_nameTextbox.Text;
      MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenProgressAsync(MyCommonTexts.SavingPleaseWait, new MyStringId?(), (Func<IMyAsyncResult>) (() => (IMyAsyncResult) new MyGuiScreenSaveAs.SaveResult(MyUtils.StripInvalidChars(this.m_nameTextbox.Text), this.m_sessionPath, this.m_copyFrom, this.m_isCloud)), (Action<IMyAsyncResult, MyGuiScreenProgressAsync>) ((result, screen) =>
      {
        screen.CloseScreen();
        this.CloseScreen();
        this.SaveAsConfirm.InvokeIfNotNull<CloudResult>(((MyGuiScreenSaveAs.SaveResult) result).Result);
      })));
      return true;
    }

    private class SaveResult : IMyAsyncResult
    {
      public bool IsCompleted => this.Task.IsComplete;

      public CloudResult Result { get; private set; }

      public Task Task { get; private set; }

      public SaveResult(string saveDir, string sessionPath, MyWorldInfo copyFrom, bool isCloud)
      {
        MyGuiScreenSaveAs.SaveResult saveResult = this;
        this.Task = Parallel.Start((Action) (() => saveResult.Result = saveResult.SaveAsync(saveDir, sessionPath, copyFrom, isCloud)));
      }

      private CloudResult SaveAsync(
        string newSaveName,
        string sessionPath,
        MyWorldInfo copyFrom,
        bool isCloud)
      {
        string sessionSavesPath = MyLocalCache.GetSessionSavesPath(newSaveName, false, false, isCloud);
        if (isCloud)
        {
          while (true)
          {
            List<MyCloudFileInfo> cloudFiles = MyGameService.GetCloudFiles(sessionSavesPath);
            if (cloudFiles != null && cloudFiles.Count != 0)
              sessionSavesPath = MyLocalCache.GetSessionSavesPath(newSaveName + MyUtils.GetRandomInt(int.MaxValue).ToString("########"), false, false, isCloud);
            else
              break;
          }
          CloudResult cloudResult = MyCloudHelper.CopyFiles(sessionPath, sessionSavesPath);
          if (cloudResult == CloudResult.Ok)
          {
            MyObjectBuilder_Checkpoint checkpoint = MyLocalCache.LoadCheckpointFromCloud(sessionSavesPath, out ulong _);
            checkpoint.SessionName = copyFrom.SessionName;
            checkpoint.WorkshopId = new ulong?();
            cloudResult = MyLocalCache.SaveCheckpointToCloud(checkpoint, sessionSavesPath);
          }
          return cloudResult;
        }
        try
        {
          while (Directory.Exists(sessionSavesPath))
            sessionSavesPath = MyLocalCache.GetSessionSavesPath(newSaveName + MyUtils.GetRandomInt(int.MaxValue).ToString("########"), false, false);
          Directory.CreateDirectory(sessionSavesPath);
          MyUtils.CopyDirectory(sessionPath, sessionSavesPath);
          MyObjectBuilder_Checkpoint checkpoint = MyLocalCache.LoadCheckpoint(sessionSavesPath, out ulong _);
          checkpoint.SessionName = copyFrom.SessionName;
          checkpoint.WorkshopId = new ulong?();
          MyLocalCache.SaveCheckpoint(checkpoint, sessionSavesPath);
        }
        catch (Exception ex)
        {
          return CloudResult.Failed;
        }
        return CloudResult.Ok;
      }
    }
  }
}
