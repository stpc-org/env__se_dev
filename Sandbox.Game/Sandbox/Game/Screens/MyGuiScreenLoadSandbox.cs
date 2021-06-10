// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenLoadSandbox
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.GameServices;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenLoadSandbox : MyGuiScreenBase
  {
    public static readonly string CONST_BACKUP = "//Backup";
    public static bool ENABLE_SCENARIO_EDIT = false;
    private MyGuiControlSaveBrowser m_saveBrowser;
    private MyGuiControlButton m_continueLastSave;
    private MyGuiControlButton m_loadButton;
    private MyGuiControlButton m_editButton;
    private MyGuiControlButton m_saveButton;
    private MyGuiControlButton m_deleteButton;
    private MyGuiControlButton m_publishButton;
    private MyGuiControlButton m_backupsButton;
    private int m_selectedRow;
    private int m_lastSelectedRow;
    private bool m_rowAutoSelect = true;
    private MyGuiControlRotatingWheel m_loadingWheel;
    private MyGuiControlImage m_levelImage;
    private bool m_parallelLoadIsRunning;
    private MyGuiControlSearchBox m_searchBox;
    private bool m_isEditable;

    public MyGuiScreenLoadSandbox()
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.924f, 0.97f)), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      this.EnabledBackgroundFade = true;
      this.RecreateControls(true);
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.AddCaption(MyCommonTexts.ScreenMenuButtonLoadGame, captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      MyGuiControlSeparatorList controlSeparatorList1 = new MyGuiControlSeparatorList();
      controlSeparatorList1.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.871999979019165 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.859f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList1);
      MyGuiControlSeparatorList controlSeparatorList2 = new MyGuiControlSeparatorList();
      Vector2 start = new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.871999979019165 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.123000003397465));
      controlSeparatorList2.AddHorizontal(start, this.m_size.Value.X * 0.859f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList2);
      MyGuiControlSeparatorList controlSeparatorList3 = new MyGuiControlSeparatorList();
      controlSeparatorList3.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.870000004768372 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.25)), this.m_size.Value.X * 0.188f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList3);
      Vector2 vector2_1 = new Vector2(-0.401f, -0.39f);
      Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
      this.m_searchBox = new MyGuiControlSearchBox(new Vector2?(vector2_1 + new Vector2((float) ((double) minSizeGui.X * 1.10000002384186 - 0.00400000018998981), 0.017f)));
      this.m_searchBox.OnTextChanged += new MyGuiControlSearchBox.TextChangedDelegate(this.OnSearchTextChange);
      this.m_searchBox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      this.m_searchBox.Size = new Vector2((float) (1075.0 / (double) MyGuiConstants.GUI_OPTIMAL_SIZE.X * 0.898000001907349), this.m_searchBox.Size.Y);
      this.Controls.Add((MyGuiControlBase) this.m_searchBox);
      this.m_saveBrowser = new MyGuiControlSaveBrowser();
      this.m_saveBrowser.Position = vector2_1 + new Vector2((float) ((double) minSizeGui.X * 1.10000002384186 - 0.00400000018998981), 0.055f);
      this.m_saveBrowser.Size = new Vector2((float) (1075.0 / (double) MyGuiConstants.GUI_OPTIMAL_SIZE.X * 0.898000001907349), 0.15f);
      this.m_saveBrowser.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_saveBrowser.VisibleRowsCount = 19;
      this.m_saveBrowser.ItemSelected += new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.OnTableItemSelected);
      this.m_saveBrowser.BrowserItemConfirmed += new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.OnTableItemConfirmedOrDoubleClick);
      this.m_saveBrowser.SelectedRowIndex = new int?(0);
      this.m_saveBrowser.GamepadHelpTextId = MySpaceTexts.LoadScreen_Help_Load;
      this.m_saveBrowser.UpdateTableSortHelpText();
      this.Controls.Add((MyGuiControlBase) this.m_saveBrowser);
      Vector2 vector2_2 = vector2_1 + minSizeGui * 0.5f;
      vector2_2.Y += 1f / 500f;
      Vector2 buttonsPositionDelta = MyGuiConstants.MENU_BUTTONS_POSITION_DELTA;
      vector2_2.Y += 0.192f;
      this.m_editButton = this.MakeButton(vector2_2 + buttonsPositionDelta * -0.25f, MyCommonTexts.LoadScreenButtonEditSettings, new Action<MyGuiControlButton>(this.OnEditClick));
      this.m_editButton.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipLoadGame_EditSettings));
      this.Controls.Add((MyGuiControlBase) this.m_editButton);
      this.m_publishButton = this.MakeButton(vector2_2 + buttonsPositionDelta * 0.75f, MyCommonTexts.LoadScreenButtonPublish, new Action<MyGuiControlButton>(this.OnPublishClick));
      this.m_publishButton.SetToolTip(MyCommonTexts.LoadScreenButtonTooltipPublish);
      this.Controls.Add((MyGuiControlBase) this.m_publishButton);
      this.m_backupsButton = this.MakeButton(vector2_2 + buttonsPositionDelta * 1.75f, MyCommonTexts.LoadScreenButtonBackups, new Action<MyGuiControlButton>(this.OnBackupsButtonClick));
      this.m_backupsButton.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipLoadGame_Backups));
      this.Controls.Add((MyGuiControlBase) this.m_backupsButton);
      this.m_saveButton = this.MakeButton(vector2_2 + buttonsPositionDelta * 2.75f, MyCommonTexts.LoadScreenButtonSaveAs, new Action<MyGuiControlButton>(this.OnSaveAsClick));
      this.m_saveButton.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipLoadGame_SaveAs));
      this.Controls.Add((MyGuiControlBase) this.m_saveButton);
      this.m_deleteButton = this.MakeButton(vector2_2 + buttonsPositionDelta * 3.75f, MyCommonTexts.LoadScreenButtonDelete, new Action<MyGuiControlButton>(this.OnDeleteClick));
      this.m_deleteButton.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipLoadGame_Delete));
      this.Controls.Add((MyGuiControlBase) this.m_deleteButton);
      Vector2 vector2_3 = vector2_2 + buttonsPositionDelta * -3.65f;
      vector2_3.X -= (float) ((double) this.m_publishButton.Size.X / 2.0 + 1.0 / 500.0);
      MyGuiControlImage myGuiControlImage = new MyGuiControlImage();
      myGuiControlImage.Size = new Vector2(this.m_publishButton.Size.X, (float) ((double) this.m_publishButton.Size.X / 4.0 * 3.0));
      myGuiControlImage.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlImage.Position = vector2_3;
      myGuiControlImage.BorderEnabled = true;
      myGuiControlImage.BorderSize = 1;
      myGuiControlImage.BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f);
      this.m_levelImage = myGuiControlImage;
      this.m_levelImage.SetTexture("Textures\\GUI\\Screens\\image_background.dds");
      this.Controls.Add((MyGuiControlBase) this.m_levelImage);
      this.m_loadButton = this.MakeButton(new Vector2(0.0f, 0.0f) - new Vector2(-0.307f, (float) (-(double) this.m_size.Value.Y / 2.0 + 0.0710000023245811)), MyCommonTexts.LoadScreenButtonLoad, new Action<MyGuiControlButton>(this.OnLoadClick));
      this.m_loadButton.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipLoadGame_Load));
      this.Controls.Add((MyGuiControlBase) this.m_loadButton);
      this.m_loadingWheel = new MyGuiControlRotatingWheel(new Vector2?(this.m_loadButton.Position + new Vector2(0.273f, -0.008f)), new Vector4?(MyGuiConstants.ROTATING_WHEEL_COLOR), 0.22f);
      this.Controls.Add((MyGuiControlBase) this.m_loadingWheel);
      this.m_loadingWheel.Visible = false;
      this.m_loadButton.DrawCrossTextureWhenDisabled = false;
      this.m_editButton.DrawCrossTextureWhenDisabled = false;
      this.m_deleteButton.DrawCrossTextureWhenDisabled = false;
      this.m_saveButton.DrawCrossTextureWhenDisabled = false;
      this.m_publishButton.DrawCrossTextureWhenDisabled = false;
      this.SetButtonsVisibility(!MyInput.Static.IsJoystickLastUsed);
      if (MyPlatformGameSettings.GAME_SAVES_TO_CLOUD)
        this.Controls.Add((MyGuiControlBase) new MySCloudStorageQuotaBar(new Vector2(-0.3145f, 0.3175f)));
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(new Vector2(start.X, this.m_loadButton.Position.Y)));
      myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.LoadScreen_Help_Screen);
      this.CloseButtonEnabled = true;
      this.FocusedControl = (MyGuiControlBase) this.m_searchBox.TextBox;
      this.m_saveBrowser.Sort(false);
    }

    private void DebugOverrideAutosaveCheckboxIsCheckChanged(MyGuiControlCheckbox checkbox)
    {
      MySandboxGame.Config.DebugOverrideAutosave = checkbox.IsChecked;
      MySandboxGame.Config.Save();
    }

    private void DebugWorldCheckboxIsCheckChanged(MyGuiControlCheckbox checkbox)
    {
      this.m_saveBrowser.SetTopMostAndCurrentDir(checkbox.IsChecked ? Path.Combine(MyFileSystem.ContentPath, "Worlds") : MyFileSystem.SavesPath);
      this.m_saveBrowser.Refresh();
    }

    private void OnBackupsButtonClick(MyGuiControlButton myGuiControlButton) => this.m_saveBrowser.AccessBackups();

    private void OnTableItemConfirmedOrDoubleClick(
      MyGuiControlTable table,
      MyGuiControlTable.EventArgs args)
    {
      this.LoadSandbox();
    }

    private MyGuiControlButton MakeButton(
      Vector2 position,
      MyStringId text,
      Action<MyGuiControlButton> onClick)
    {
      return new MyGuiControlButton(new Vector2?(position), text: MyTexts.Get(text), onButtonClick: onClick);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenLoadSandbox);

    private void OnSearchTextChange(string text)
    {
      this.m_saveBrowser.SearchTextFilter = text;
      this.m_saveBrowser.RefreshAfterLoaded();
    }

    private void OnLoadClick(MyGuiControlButton sender) => this.LoadSandbox();

    private void OnBackClick(MyGuiControlButton sender) => this.CloseScreen(false);

    private void DeleteThumbCache()
    {
      string path = Path.Combine(MyFileSystem.TempPath, "thumbs");
      if (!Directory.Exists(path))
        return;
      try
      {
        Directory.Delete(path, true);
      }
      catch
      {
      }
    }

    public override bool CloseScreen(bool isUnloading = false)
    {
      this.DeleteThumbCache();
      return base.CloseScreen(isUnloading);
    }

    private void OnEditClick(MyGuiControlButton sender)
    {
      MyGuiControlTable.Row selectedRow = this.m_saveBrowser.SelectedRow;
      if (selectedRow == null)
        return;
      MySaveInfo info;
      this.m_saveBrowser.GetSave(selectedRow, out info);
      if (info.Valid && !info.WorldInfo.IsCorrupted)
      {
        ulong sizeInBytes;
        MyObjectBuilder_Checkpoint builderCheckpoint = !info.IsCloud ? MyLocalCache.LoadCheckpoint(info.Name, out sizeInBytes) : MyLocalCache.LoadCheckpointFromCloud(info.Name, out sizeInBytes);
        if (builderCheckpoint != null)
        {
          if (!this.m_isEditable)
          {
            StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
            MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MySpaceTexts.WorldFileCouldNotBeEdited), messageCaption: messageCaption));
            return;
          }
          MyGuiScreenBase screen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.EditWorldSettingsScreen, (object) builderCheckpoint, (object) info.Name, (object) true, (object) true, (object) true, (object) info.IsCloud);
          screen.Closed += (MyGuiScreenBase.ScreenHandler) ((source, isUnloading) => this.m_saveBrowser.ForceRefresh());
          MyGuiSandbox.AddScreen(screen);
          this.m_rowAutoSelect = true;
          return;
        }
      }
      StringBuilder messageCaption1 = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.WorldFileCouldNotBeLoaded), messageCaption: messageCaption1));
    }

    private void OnSaveAsClick(MyGuiControlButton sender)
    {
      MyGuiControlTable.Row selectedRow = this.m_saveBrowser.SelectedRow;
      if (selectedRow == null)
        return;
      MySaveInfo info;
      this.m_saveBrowser.GetSave(selectedRow, out info);
      if (!info.Valid)
        return;
      MyGuiScreenSaveAs myGuiScreenSaveAs = new MyGuiScreenSaveAs(info.WorldInfo, info.Name, (List<string>) null, info.IsCloud);
      myGuiScreenSaveAs.SaveAsConfirm += new Action<CloudResult>(this.OnSaveAsConfirm);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) myGuiScreenSaveAs);
    }

    private void OnSaveAsConfirm(CloudResult result)
    {
      MyStringId errorMessage;
      if (MyCloudHelper.IsError(result, out errorMessage))
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(errorMessage), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
      else
        this.m_saveBrowser.ForceRefresh();
    }

    private void OnDeleteClick(MyGuiControlButton sender)
    {
      MyGuiControlTable.Row selectedRow = this.m_saveBrowser.SelectedRow;
      if (selectedRow == null)
        return;
      MySaveInfo info;
      this.m_saveBrowser.GetSave(selectedRow, out info);
      if (info.Valid)
      {
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: new StringBuilder().AppendFormat(MyCommonTexts.MessageBoxTextAreYouSureYouWantToDeleteSave, (object) info.WorldInfo.SessionName), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), callback: new Action<MyGuiScreenMessageBox.ResultEnum>(this.OnDeleteConfirm)));
      }
      else
      {
        DirectoryInfo directory = this.m_saveBrowser.GetDirectory(selectedRow);
        if (directory == null)
          return;
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: new StringBuilder().AppendFormat(MyCommonTexts.MessageBoxTextAreYouSureYouWantToDeleteSave, (object) directory.Name), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), callback: new Action<MyGuiScreenMessageBox.ResultEnum>(this.OnDeleteConfirm)));
      }
    }

    private void OnDeleteConfirm(MyGuiScreenMessageBox.ResultEnum callbackReturn)
    {
      if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
      {
        MyGuiControlTable.Row selectedRow = this.m_saveBrowser.SelectedRow;
        if (selectedRow == null)
          return;
        MySaveInfo info;
        this.m_saveBrowser.GetSave(selectedRow, out info);
        if (info.Valid)
        {
          try
          {
            if (info.IsCloud)
            {
              MyCloudHelper.Delete(info.Name);
              Directory.Delete(MyCloudHelper.CloudToLocalWorldPath(info.Name), true);
            }
            else
              Directory.Delete(info.Name, true);
            this.m_saveBrowser.RemoveSelectedRow();
            this.m_saveBrowser.SelectedRowIndex = new int?(this.m_selectedRow);
            this.m_saveBrowser.Refresh();
            this.m_levelImage.SetTexture();
          }
          catch (Exception ex)
          {
            MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.SessionDeleteFailed)));
          }
        }
        else
        {
          try
          {
            DirectoryInfo directory = this.m_saveBrowser.GetDirectory(selectedRow);
            if (directory != null)
            {
              directory.Delete(true);
              this.m_saveBrowser.Refresh();
            }
          }
          catch (Exception ex)
          {
            MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.SessionDeleteFailed)));
          }
        }
      }
      this.m_rowAutoSelect = true;
    }

    private void OnContinueLastGameClick(MyGuiControlButton sender)
    {
      MySessionLoader.LoadLastSession();
      this.m_continueLastSave.Enabled = false;
    }

    private void OnPublishClick(MyGuiControlButton sender)
    {
      MyGuiControlTable.Row selectedRow = this.m_saveBrowser.SelectedRow;
      if (selectedRow == null)
        return;
      MySaveInfo info;
      this.m_saveBrowser.GetSave(selectedRow, out info);
      if (!info.Valid)
        return;
      MyGuiScreenLoadSandbox.Publish(info.Name, info.WorldInfo, info.IsCloud);
    }

    public static void Publish(string sessionPath, MyWorldInfo worlInfo, bool isCloud)
    {
      WorkshopId[] workshopIds = worlInfo.WorkshopIds;
      if ((workshopIds != null ? ((uint) workshopIds.Length > 0U ? 1 : 0) : 0) != 0)
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, new StringBuilder(MyTexts.GetString(MyCommonTexts.MessageBoxTextDoYouWishToUpdateWorld)), MyTexts.Get(MyCommonTexts.MessageBoxCaptionDoYouWishToUpdateWorld), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (val =>
        {
          if (val != MyGuiScreenMessageBox.ResultEnum.YES)
            return;
          MyGuiScreenLoadSandbox.UploadToWorkshop(sessionPath, worlInfo, isCloud);
        }))));
      else
        MyGuiScreenLoadSandbox.UploadToWorkshop(sessionPath, worlInfo, isCloud);
    }

    private static void UploadToWorkshop(string sessionPath, MyWorldInfo worldInfo, bool isCloud)
    {
      Action<MyGuiScreenMessageBox.ResultEnum, string[], string[]> callback = (Action<MyGuiScreenMessageBox.ResultEnum, string[], string[]>) ((tagsResult, outTags, serviceNames) =>
      {
        if (tagsResult != MyGuiScreenMessageBox.ResultEnum.YES || serviceNames == null || serviceNames.Length == 0)
          return;
        string originalSessionPath = sessionPath;
        if (isCloud)
        {
          string filePath = Path.Combine(MyFileSystem.TempPath, Path.GetFileName(Path.GetDirectoryName(sessionPath)));
          if (!MyCloudHelper.ExtractFilesTo(sessionPath, filePath, true))
          {
            MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextCloudExtractError), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionWorldPublishFailed)));
            return;
          }
          sessionPath = filePath;
        }
        MyObjectBuilder_Checkpoint checkpoint = MyLocalCache.LoadCheckpoint(sessionPath, out ulong _);
        if (checkpoint == null)
        {
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextLoadWorldError), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionWorldPublishFailed)));
        }
        else
        {
          Array.Resize<string>(ref outTags, 1 + (outTags != null ? outTags.Length : 0));
          outTags[outTags.Length - 1] = !checkpoint.Settings.IsSettingsExperimental(true) ? MySteamConstants.TAG_SAFE : MySteamConstants.TAG_EXPERIMENTAL;
          WorkshopId[] workshopIds = MyWorkshop.FilterWorkshopIds(worldInfo.WorkshopIds, serviceNames);
          foreach (WorkshopId workshopId in workshopIds)
          {
            IMyUGCService aggregate = MyGameService.WorkshopService.GetAggregate(workshopId.ServiceName);
            if (aggregate != null && !aggregate.IsConsentGiven)
            {
              MySessionLoader.ShowUGCConsentNeededForThisServiceWarning();
              return;
            }
          }
          MyWorkshop.PublishWorldAsync(sessionPath, worldInfo.SessionName, worldInfo.Description, workshopIds, outTags, MyPublishedFileVisibility.Public, (Action<bool, MyGameServiceCallResult, string, MyWorkshopItem[]>) ((success, result, resultServiceName, publishedFiles) =>
          {
            if (publishedFiles != null && publishedFiles.Length != 0)
            {
              worldInfo.WorkshopIds = publishedFiles.ToWorkshopIds();
              checkpoint.WorkshopId = new ulong?(publishedFiles[0].Id);
              checkpoint.WorkshopServiceName = publishedFiles[0].ServiceName;
              if (publishedFiles.Length > 1)
              {
                checkpoint.WorkshopId1 = new ulong?(publishedFiles[1].Id);
                checkpoint.WorkshopServiceName1 = publishedFiles[1].ServiceName;
              }
              if (isCloud)
              {
                int cloud = (int) MyLocalCache.SaveCheckpointToCloud(checkpoint, originalSessionPath);
              }
              else
                MyLocalCache.SaveCheckpoint(checkpoint, sessionPath);
            }
            MyWorkshop.ReportPublish(publishedFiles, result, resultServiceName);
          }));
        }
      });
      if (MyWorkshop.WorldCategories.Length != 0)
        MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenWorkshopTags("world", MyWorkshop.WorldCategories, (string[]) null, callback));
      else
        callback(MyGuiScreenMessageBox.ResultEnum.YES, new string[1]
        {
          "world"
        }, MyGameService.GetUGCNamesList());
    }

    private void OnTableItemSelected(
      MyGuiControlTable sender,
      MyGuiControlTable.EventArgs eventArgs)
    {
      sender.CanHaveFocus = true;
      this.FocusedControl = (MyGuiControlBase) sender;
      this.m_selectedRow = eventArgs.RowIndex;
      this.m_lastSelectedRow = this.m_selectedRow;
      this.LoadImagePreview();
    }

    private void LoadImagePreview()
    {
      MyGuiControlTable.Row selectedRow = this.m_saveBrowser.SelectedRow;
      if (selectedRow == null)
        return;
      MySaveInfo info;
      this.m_saveBrowser.GetSave(selectedRow, out info);
      if (!info.Valid || info.WorldInfo.IsCorrupted)
        this.m_levelImage.SetTexture("Textures\\GUI\\Screens\\image_background.dds");
      else if (info.IsCloud)
      {
        string thumbDir = Path.Combine(MyFileSystem.TempPath, "thumbs/");
        string thumbImage = Path.Combine(thumbDir, info.Name.GetHashCode().ToString());
        thumbImage += ".jpg";
        if (File.Exists(thumbImage))
          this.m_levelImage.SetTexture(thumbImage);
        else
          MyGameService.LoadFromCloudAsync(MyCloudHelper.Combine(info.Name, MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION), (Action<byte[]>) (data =>
          {
            if (data != null)
            {
              try
              {
                Directory.CreateDirectory(thumbDir);
                File.WriteAllBytes(thumbImage, data);
                MyRenderProxy.UnloadTexture(thumbImage);
                this.m_levelImage.SetTexture(thumbImage);
                return;
              }
              catch
              {
              }
            }
            this.m_levelImage.SetTexture("Textures\\GUI\\Screens\\image_background.dds");
          }));
      }
      else
      {
        string name = info.Name;
        if (Directory.Exists(name + MyGuiScreenLoadSandbox.CONST_BACKUP))
        {
          string[] directories = Directory.GetDirectories(name + MyGuiScreenLoadSandbox.CONST_BACKUP);
          if (((IEnumerable<string>) directories).Any<string>())
          {
            string str = Path.Combine(((IEnumerable<string>) directories).Last<string>(), MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION);
            if (File.Exists(str) && new FileInfo(str).Length > 0L)
            {
              this.m_levelImage.SetTexture(Path.Combine(((IEnumerable<string>) Directory.GetDirectories(name + MyGuiScreenLoadSandbox.CONST_BACKUP)).Last<string>().ToString(), MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION));
              return;
            }
          }
        }
        string str1 = Path.Combine(name, MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION);
        if (File.Exists(str1) && new FileInfo(str1).Length > 0L)
        {
          this.m_levelImage.SetTexture();
          this.m_levelImage.SetTexture(Path.Combine(name, MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION));
        }
        else
          this.m_levelImage.SetTexture("Textures\\GUI\\Screens\\image_background.dds");
      }
    }

    private void LoadSandbox()
    {
      if (this.m_parallelLoadIsRunning)
        return;
      this.m_parallelLoadIsRunning = true;
      MyGuiScreenProgress progressScreen = new MyGuiScreenProgress(MyTexts.Get(MySpaceTexts.ProgressScreen_LoadingWorld));
      MyScreenManager.AddScreen((MyGuiScreenBase) progressScreen);
      Parallel.StartBackground((Action) (() => this.LoadSandboxInternal()), (Action) (() =>
      {
        progressScreen.CloseScreen();
        this.m_parallelLoadIsRunning = false;
      }));
    }

    private void LoadSandboxInternal()
    {
      MyGuiControlTable.Row selectedRow = this.m_saveBrowser.SelectedRow;
      if (selectedRow == null)
        return;
      MySaveInfo save;
      this.m_saveBrowser.GetSave(selectedRow, out save);
      if (this.m_saveBrowser.GetDirectory(selectedRow) != null)
        return;
      if (!save.Valid || save.WorldInfo.IsCorrupted)
      {
        MySandboxGame.Static.Invoke((Action) (() =>
        {
          StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.WorldFileCouldNotBeLoaded), messageCaption: messageCaption));
        }), "New Game screen");
      }
      else
      {
        ulong sizeInBytes;
        MyObjectBuilder_Checkpoint checkpoint = !save.IsCloud ? MyLocalCache.LoadCheckpoint(save.Name, out sizeInBytes) : MyLocalCache.LoadCheckpointFromCloud(save.Name, out sizeInBytes);
        if (MySessionLoader.HasOnlyModsFromConsentedUGCs(checkpoint))
        {
          if (checkpoint != null && checkpoint.OnlineMode != MyOnlineModeEnum.OFFLINE)
          {
            MyGameService.Service.RequestPermissions(Permissions.Multiplayer, true, (Action<PermissionResult>) (granted =>
            {
              switch (granted)
              {
                case PermissionResult.Granted:
                  MyGameService.Service.RequestPermissions(Permissions.UGC, true, (Action<PermissionResult>) (ugcGranted =>
                  {
                    switch (ugcGranted)
                    {
                      case PermissionResult.Granted:
                        int num = (int) this.BackupAndLoadSandbox(ref save);
                        break;
                      case PermissionResult.Error:
                        MySandboxGame.Static.Invoke((Action) (() => MyGuiSandbox.Show(MyCommonTexts.XBoxPermission_MultiplayerError, type: MyMessageBoxStyleEnum.Info)), "New Game screen");
                        break;
                    }
                  }));
                  break;
                case PermissionResult.Error:
                  MySandboxGame.Static.Invoke((Action) (() => MyGuiSandbox.Show(MyCommonTexts.XBoxPermission_MultiplayerError, type: MyMessageBoxStyleEnum.Info)), "New Game screen");
                  break;
              }
            }));
          }
          else
          {
            MyStringId errorMessage;
            if (!MyCloudHelper.IsError(this.BackupAndLoadSandbox(ref save), out errorMessage, new MyStringId?(MySpaceTexts.WorldSettings_Error_SavingFailed)))
              return;
            MySandboxGame.Static.Invoke((Action) (() =>
            {
              MyGuiScreenMessageBox messageBox = MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(errorMessage), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
              messageBox.SkipTransition = true;
              messageBox.InstantClose = false;
              MyGuiSandbox.AddScreen((MyGuiScreenBase) messageBox);
            }), "New Game screen");
          }
        }
        else
          MySessionLoader.ShowUGCConsentNotAcceptedWarning(MySessionLoader.GetNonConsentedServiceNameInCheckpoint(checkpoint));
      }
    }

    private CloudResult BackupAndLoadSandbox(ref MySaveInfo saveInfo)
    {
      this.DeleteThumbCache();
      bool gameSavesToCloud = MyPlatformGameSettings.GAME_SAVES_TO_CLOUD;
      MyLog.Default.WriteLine("LoadSandbox() - Start");
      string saveFilePath = saveInfo.Name;
      if (this.m_saveBrowser.InBackupsFolder)
      {
        if (gameSavesToCloud)
        {
          CloudResult cloud = this.CopyBackupToCloud(saveFilePath);
          if (cloud != CloudResult.Ok)
            return cloud;
        }
        this.CopyBackupUpALevel(ref saveFilePath, saveInfo.WorldInfo);
      }
      else if (saveInfo.IsCloud)
      {
        string localWorldPath = MyCloudHelper.CloudToLocalWorldPath(saveFilePath);
        if (!MyCloudHelper.ExtractFilesTo(saveFilePath, localWorldPath, false))
          return CloudResult.Failed;
        saveFilePath = localWorldPath;
      }
      MySessionLoader.LoadSingleplayerSession(saveFilePath);
      MyLog.Default.WriteLine("LoadSandbox() - End");
      return CloudResult.Ok;
    }

    private CloudResult CopyBackupToCloud(string saveFilePath)
    {
      try
      {
        DirectoryInfo sourceDirectory = new DirectoryInfo(saveFilePath);
        return MyCloudHelper.UploadFiles(MyCloudHelper.LocalToCloudWorldPath(sourceDirectory.Parent.Parent.FullName + "\\"), sourceDirectory, false);
      }
      catch
      {
        return CloudResult.Failed;
      }
    }

    private void CopyBackupUpALevel(ref string saveFilePath, MyWorldInfo worldInfo)
    {
      DirectoryInfo directoryInfo = new DirectoryInfo(saveFilePath);
      DirectoryInfo targetDirectory = directoryInfo.Parent.Parent;
      ((IEnumerable<FileInfo>) targetDirectory.GetFiles()).ForEach<FileInfo>((Action<FileInfo>) (file => file.Delete()));
      ((IEnumerable<FileInfo>) directoryInfo.GetFiles()).ForEach<FileInfo>((Action<FileInfo>) (file => file.CopyTo(Path.Combine(targetDirectory.FullName, file.Name))));
      saveFilePath = targetDirectory.FullName;
    }

    public override bool Update(bool hasFocus)
    {
      if (this.m_saveBrowser != null & hasFocus && this.m_saveBrowser.RowsCount != 0 && this.m_rowAutoSelect)
      {
        if (this.m_lastSelectedRow < this.m_saveBrowser.RowsCount)
        {
          this.m_saveBrowser.SelectedRow = this.m_saveBrowser.GetRow(this.m_lastSelectedRow);
          this.m_selectedRow = this.m_lastSelectedRow;
        }
        else
        {
          this.m_saveBrowser.SelectedRow = this.m_saveBrowser.GetRow(0);
          this.m_selectedRow = this.m_lastSelectedRow = 0;
        }
        this.m_rowAutoSelect = false;
        this.m_saveBrowser.ScrollToSelection();
        this.LoadImagePreview();
      }
      MySaveInfo info;
      this.m_saveBrowser.GetSave(this.m_saveBrowser.SelectedRow, out info);
      if (info.Valid)
      {
        this.m_loadButton.Enabled = true;
        this.m_editButton.Enabled = true;
        this.m_saveButton.Enabled = true;
        this.m_publishButton.Enabled = MyFakes.ENABLE_WORKSHOP_PUBLISH && !info.WorldInfo.IsCampaign;
        this.m_backupsButton.Enabled = true;
        this.m_isEditable = !info.WorldInfo.IsCampaign || MyGuiScreenLoadSandbox.ENABLE_SCENARIO_EDIT || MySandboxGame.Config.ExperimentalMode;
      }
      else
      {
        this.m_loadButton.Enabled = false;
        this.m_editButton.Enabled = false;
        this.m_saveButton.Enabled = false;
        this.m_publishButton.Enabled = false;
        this.m_backupsButton.Enabled = false;
      }
      this.m_deleteButton.Enabled = this.m_saveBrowser.SelectedRow != null;
      if (hasFocus)
      {
        if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y))
          this.OnDeleteClick((MyGuiControlButton) null);
        if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.VIEW))
          this.OnBackupsButtonClick((MyGuiControlButton) null);
        if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.MAIN_MENU))
          this.OnPublishClick((MyGuiControlButton) null);
        if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.RIGHT_STICK_BUTTON))
          this.OnSaveAsClick((MyGuiControlButton) null);
        if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
          this.OnEditClick((MyGuiControlButton) null);
        this.SetButtonsVisibility(!MyInput.Static.IsJoystickLastUsed);
      }
      return base.Update(hasFocus);
    }

    private void SetButtonsVisibility(bool visible)
    {
      this.m_loadButton.Visible = visible;
      this.m_deleteButton.Visible = visible;
      this.m_editButton.Visible = visible;
      this.m_backupsButton.Visible = visible;
      this.m_publishButton.Visible = visible;
      this.m_saveButton.Visible = visible;
    }
  }
}
