// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenLoadSubscribedWorld
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Engine.Networking;
using Sandbox.Game.GUI;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VRage;
using VRage.Game;
using VRage.GameServices;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenLoadSubscribedWorld : MyGuiScreenBase
  {
    private MyGuiControlTable m_worldsTable;
    private MyGuiControlButton m_loadButton;
    private MyGuiControlButton m_openInWorkshopButton;
    private MyGuiControlButton m_refreshButton;
    private MyGuiControlButton m_browseWorkshopButton;
    private MyGuiControlButton m_copyButton;
    private MyGuiControlButton m_currentButton;
    private int m_selectedRow;
    private bool m_listNeedsReload;
    private List<MyWorkshopItem> m_subscribedWorlds;
    private MyGuiControlTextbox m_searchBox;
    private MyGuiControlLabel m_searchBoxLabel;
    private MyGuiControlButton m_searchClear;
    private MyGuiControlRotatingWheel m_loadingWheel;
    private bool m_displayTabScenario;
    private bool m_displayTabWorkshop;
    private bool m_displayTabCustom;

    public MyGuiScreenLoadSubscribedWorld(
      bool displayTabScenario = true,
      bool displayTabWorkshop = true,
      bool displayTabCustom = true)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.878f, 0.97f)), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      this.m_displayTabScenario = displayTabScenario;
      this.m_displayTabWorkshop = displayTabWorkshop;
      this.m_displayTabCustom = displayTabCustom;
      this.EnabledBackgroundFade = true;
      this.m_listNeedsReload = true;
      this.RecreateControls(true);
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      MyGuiControlScreenSwitchPanel screenSwitchPanel = new MyGuiControlScreenSwitchPanel((MyGuiScreenBase) this, MyTexts.Get(MyCommonTexts.WorkshopScreen_Description), this.m_displayTabScenario, this.m_displayTabWorkshop, this.m_displayTabCustom);
      this.AddCaption(MyCommonTexts.ScreenMenuButtonCampaign);
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.871999979019165 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.123000003397465)), this.m_size.Value.X * 0.872f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList);
      float y = 0.216f;
      float num1 = 50f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y;
      float num2 = 15f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
      float num3 = 50f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
      float x = 93f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
      Vector2 vector2_1 = -this.m_size.Value / 2f + new Vector2(x, y + 0.199f);
      Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
      Vector2 vector2_2 = -this.m_size.Value / 2f + new Vector2(x + minSizeGui.X + num2, y);
      Vector2 vector2_3 = this.m_size.Value / 2f - vector2_2;
      vector2_3.X -= num3;
      vector2_3.Y -= num1;
      this.m_searchBoxLabel = new MyGuiControlLabel();
      this.m_searchBoxLabel.Text = MyTexts.Get(MyCommonTexts.Search).ToString() + ":";
      this.m_searchBoxLabel.Position = new Vector2(-0.188f, -0.244f);
      this.Controls.Add((MyGuiControlBase) this.m_searchBoxLabel);
      this.m_searchBox = new MyGuiControlTextbox(new Vector2?(new Vector2(0.382f, -0.247f)));
      this.m_searchBox.TextChanged += new Action<MyGuiControlTextbox>(this.OnSearchTextChange);
      this.m_searchBox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      this.m_searchBox.Size = new Vector2(0.56f - this.m_searchBoxLabel.Size.X, 1f);
      this.Controls.Add((MyGuiControlBase) this.m_searchBox);
      MyGuiControlButton guiControlButton = new MyGuiControlButton();
      guiControlButton.Position = this.m_searchBox.Position + new Vector2(-0.027f, 0.004f);
      guiControlButton.Size = new Vector2(0.045f, 0.05666667f);
      guiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      guiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.Close;
      guiControlButton.ActivateOnMouseRelease = true;
      this.m_searchClear = guiControlButton;
      this.m_searchClear.ButtonClicked += new Action<MyGuiControlButton>(this.OnSearchClear);
      this.Controls.Add((MyGuiControlBase) this.m_searchClear);
      this.m_worldsTable = new MyGuiControlTable();
      this.m_worldsTable.Position = vector2_2 + new Vector2(0.0055f, 0.065f);
      this.m_worldsTable.Size = new Vector2((float) (1075.0 / (double) MyGuiConstants.GUI_OPTIMAL_SIZE.X * 0.851999998092651), 0.15f);
      this.m_worldsTable.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_worldsTable.ColumnsCount = 1;
      this.m_worldsTable.VisibleRowsCount = 15;
      this.m_worldsTable.ItemSelected += new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.OnTableItemSelected);
      this.m_worldsTable.ItemDoubleClicked += new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.OnTableItemConfirmedOrDoubleClick);
      this.m_worldsTable.ItemConfirmed += new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.OnTableItemConfirmedOrDoubleClick);
      this.m_worldsTable.SetCustomColumnWidths(new float[1]
      {
        1f
      });
      this.m_worldsTable.SetColumnComparison(0, (Comparison<MyGuiControlTable.Cell>) ((a, b) => ((StringBuilder) a.UserData).CompareToIgnoreCase((StringBuilder) b.UserData)));
      this.Controls.Add((MyGuiControlBase) this.m_worldsTable);
      Vector2 vector2_4 = vector2_1 + minSizeGui * 0.5f;
      Vector2 buttonsPositionDelta = MyGuiConstants.MENU_BUTTONS_POSITION_DELTA;
      this.Controls.Add((MyGuiControlBase) (this.m_copyButton = this.MakeButton(vector2_4 + buttonsPositionDelta * -3.21f, MyCommonTexts.ScreenLoadSubscribedWorldCopyWorld, MyCommonTexts.ToolTipWorkshopCopyWorld, new Action<MyGuiControlButton>(this.OnCopyClick))));
      this.Controls.Add((MyGuiControlBase) (this.m_openInWorkshopButton = this.MakeButton(vector2_4 + buttonsPositionDelta * -2.21f, MyCommonTexts.ScreenLoadSubscribedWorldOpenInWorkshop, MyCommonTexts.ToolTipWorkshopOpenInWorkshop, new Action<MyGuiControlButton>(this.OnOpenInWorkshopClick))));
      this.Controls.Add((MyGuiControlBase) (this.m_browseWorkshopButton = this.MakeButton(vector2_4 + buttonsPositionDelta * -1.21f, MyCommonTexts.ScreenLoadSubscribedWorldBrowseWorkshop, MyCommonTexts.ToolTipWorkshopBrowseWorkshop, new Action<MyGuiControlButton>(this.OnBrowseWorkshopClick))));
      this.Controls.Add((MyGuiControlBase) (this.m_refreshButton = this.MakeButton(vector2_4 + buttonsPositionDelta * -0.21f, MyCommonTexts.ScreenLoadSubscribedWorldRefresh, MyCommonTexts.ToolTipWorkshopRefresh, new Action<MyGuiControlButton>(this.OnRefreshClick))));
      this.Controls.Add((MyGuiControlBase) (this.m_loadButton = this.MakeButton(new Vector2(0.0f, 0.0f) - new Vector2(-0.109f, (float) (-(double) this.m_size.Value.Y / 2.0 + 0.0710000023245811)), MyCommonTexts.ScreenLoadSubscribedWorldCopyAndLoad, MyCommonTexts.ToolTipWorkshopCopyAndLoad, new Action<MyGuiControlButton>(this.OnLoadClick))));
      this.m_loadingWheel = new MyGuiControlRotatingWheel(new Vector2?(new Vector2((float) ((double) this.m_size.Value.X / 2.0 - 0.0769999995827675), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.108000002801418))), new Vector4?(MyGuiConstants.ROTATING_WHEEL_COLOR), 0.2f);
      this.Controls.Add((MyGuiControlBase) this.m_loadingWheel);
      this.m_loadingWheel.Visible = false;
      this.m_loadButton.DrawCrossTextureWhenDisabled = false;
      this.m_openInWorkshopButton.DrawCrossTextureWhenDisabled = false;
      this.CloseButtonEnabled = true;
    }

    private bool SearchFilterTest(string testString)
    {
      if (this.m_searchBox.Text != null && this.m_searchBox.Text.Length != 0)
      {
        string[] strArray = this.m_searchBox.Text.Split(' ');
        string lower = testString.ToLower();
        foreach (string str in strArray)
        {
          if (!lower.Contains(str.ToLower()))
            return false;
        }
      }
      return true;
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenLoadSubscribedWorld);

    public override bool RegisterClicks() => true;

    private void OnSearchTextChange(MyGuiControlTextbox box) => this.RefreshGameList();

    private void OnSearchClear(MyGuiControlButton sender)
    {
      this.m_searchBox.Text = "";
      this.RefreshGameList();
    }

    private void OnOpenInWorkshopClick(MyGuiControlButton obj)
    {
      MyGuiControlTable.Row selectedRow = this.m_worldsTable.SelectedRow;
      if (selectedRow == null || !(selectedRow.UserData is MyWorkshopItem userData))
        return;
      MyGuiSandbox.OpenUrlWithFallback(userData.GetItemUrl(), userData.ServiceName + " Workshop");
    }

    private void OnBrowseWorkshopClick(MyGuiControlButton obj) => MyWorkshop.OpenWorkshopBrowser(MySteamConstants.TAG_WORLDS);

    private void OnRefreshClick(MyGuiControlButton obj)
    {
      if (this.m_listNeedsReload)
        return;
      this.m_listNeedsReload = true;
      this.FillList();
    }

    private void OnBackClick(MyGuiControlButton sender) => this.CloseScreen();

    private void OnTableItemSelected(
      MyGuiControlTable sender,
      MyGuiControlTable.EventArgs eventArgs)
    {
      this.m_selectedRow = eventArgs.RowIndex;
    }

    private void OnLoadClick(MyGuiControlButton sender)
    {
      this.m_currentButton = this.m_loadButton;
      this.CreateAndLoadFromSubscribedWorld();
    }

    private void OnTableItemConfirmedOrDoubleClick(
      MyGuiControlTable sender,
      MyGuiControlTable.EventArgs eventArgs)
    {
      this.m_currentButton = this.m_loadButton;
      this.CreateAndLoadFromSubscribedWorld();
    }

    private void OnCopyClick(MyGuiControlButton sender)
    {
      this.m_currentButton = this.m_copyButton;
      this.CopyWorldAndGoToLoadScreen();
    }

    private void CreateAndLoadFromSubscribedWorld()
    {
      MyGuiControlTable.Row selectedRow = this.m_worldsTable.SelectedRow;
      if (selectedRow == null || !(selectedRow.UserData is MyWorkshopItem))
        return;
      MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, new MyStringId?(), new Func<IMyAsyncResult>(this.beginActionLoadSaves), new Action<IMyAsyncResult, MyGuiScreenProgressAsync>(this.endActionLoadSaves)));
    }

    private void CopyWorldAndGoToLoadScreen()
    {
      MyGuiControlTable.Row selectedRow = this.m_worldsTable.SelectedRow;
      if (selectedRow == null || !(selectedRow.UserData is MyWorkshopItem))
        return;
      MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, new MyStringId?(), new Func<IMyAsyncResult>(this.beginActionLoadSaves), new Action<IMyAsyncResult, MyGuiScreenProgressAsync>(this.endActionLoadSaves)));
    }

    private void OnSuccess(string sessionPath)
    {
      if (this.m_currentButton == this.m_copyButton)
      {
        MyGuiScreenLoadSandbox screenLoadSandbox = new MyGuiScreenLoadSandbox();
        MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenLoadSandbox());
      }
      else if (this.m_currentButton == this.m_loadButton)
        MySessionLoader.LoadSingleplayerSession(sessionPath);
      this.m_currentButton = (MyGuiControlButton) null;
    }

    private void OverwriteWorldDialog() => MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(this.m_currentButton == this.m_loadButton ? MyCommonTexts.MessageBoxTextWorldExistsDownloadOverwrite : MyCommonTexts.MessageBoxTextWorldExistsOverwrite), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), callback: new Action<MyGuiScreenMessageBox.ResultEnum>(this.OnOverwriteWorld)));

    private void OnOverwriteWorld(MyGuiScreenMessageBox.ResultEnum callbackReturn)
    {
      if (callbackReturn != MyGuiScreenMessageBox.ResultEnum.YES)
        return;
      MyWorkshop.CreateWorldInstanceAsync(this.m_worldsTable.SelectedRow.UserData as MyWorkshopItem, MyWorkshop.MyWorkshopPathInfo.CreateWorldInfo(), true, (Action<bool, string>) ((success, sessionPath) =>
      {
        if (!success)
          return;
        this.OnSuccess(sessionPath);
      }));
    }

    public override bool Update(bool hasFocus)
    {
      if (this.m_worldsTable.SelectedRow != null)
      {
        this.m_loadButton.Enabled = true;
        this.m_copyButton.Enabled = true;
        this.m_openInWorkshopButton.Enabled = true;
      }
      else
      {
        this.m_loadButton.Enabled = false;
        this.m_copyButton.Enabled = false;
        this.m_openInWorkshopButton.Enabled = false;
      }
      return base.Update(hasFocus);
    }

    protected override void OnClosed() => base.OnClosed();

    protected override void OnShow()
    {
      base.OnShow();
      if (!this.m_listNeedsReload)
        return;
      this.FillList();
    }

    private MyGuiControlButton MakeButton(
      Vector2 position,
      MyStringId text,
      MyStringId toolTip,
      Action<MyGuiControlButton> onClick)
    {
      Vector2? position1 = new Vector2?(position);
      Vector2? size = new Vector2?();
      Vector4? colorMask = new Vector4?();
      StringBuilder stringBuilder = MyTexts.Get(text);
      string toolTip1 = MyTexts.GetString(toolTip);
      StringBuilder text1 = stringBuilder;
      Action<MyGuiControlButton> onButtonClick = onClick;
      int? buttonIndex = new int?();
      return new MyGuiControlButton(position1, size: size, colorMask: colorMask, toolTip: toolTip1, text: text1, onButtonClick: onButtonClick, buttonIndex: buttonIndex);
    }

    private MyGuiControlButton MakeButton(
      Vector2 position,
      MyStringId text,
      string toolTip,
      Action<MyGuiControlButton> onClick)
    {
      Vector2? position1 = new Vector2?(position);
      Vector2? size = new Vector2?();
      Vector4? colorMask = new Vector4?();
      StringBuilder stringBuilder = MyTexts.Get(text);
      string toolTip1 = toolTip;
      StringBuilder text1 = stringBuilder;
      Action<MyGuiControlButton> onButtonClick = onClick;
      int? buttonIndex = new int?();
      return new MyGuiControlButton(position1, size: size, colorMask: colorMask, toolTip: toolTip1, text: text1, onButtonClick: onButtonClick, buttonIndex: buttonIndex);
    }

    private void FillList() => MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, new MyStringId?(), new Func<IMyAsyncResult>(this.beginAction), new Action<IMyAsyncResult, MyGuiScreenProgressAsync>(this.endAction)));

    private void AddHeaders() => this.m_worldsTable.SetColumnName(0, MyTexts.Get(MyCommonTexts.Name));

    private void RefreshGameList()
    {
      this.m_worldsTable.Clear();
      this.AddHeaders();
      if (this.m_subscribedWorlds != null)
      {
        for (int index = 0; index < this.m_subscribedWorlds.Count; ++index)
        {
          MyWorkshopItem subscribedWorld = this.m_subscribedWorlds[index];
          MyGuiControlTable.Row row = new MyGuiControlTable.Row((object) subscribedWorld);
          StringBuilder stringBuilder = new StringBuilder(subscribedWorld.Title);
          if (this.SearchFilterTest(stringBuilder.ToString()))
          {
            row.AddCell(new MyGuiControlTable.Cell(stringBuilder.ToString(), (object) stringBuilder));
            row.AddCell(new MyGuiControlTable.Cell());
            this.m_worldsTable.Add(row);
          }
        }
      }
      this.m_worldsTable.SelectedRowIndex = new int?();
    }

    private IMyAsyncResult beginAction() => (IMyAsyncResult) new MyGuiScreenLoadSubscribedWorld.LoadListResult();

    private void endAction(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
    {
      this.m_listNeedsReload = false;
      this.m_subscribedWorlds = ((MyGuiScreenLoadSubscribedWorld.LoadListResult) result).SubscribedWorlds;
      this.RefreshGameList();
      screen.CloseScreen();
      this.m_loadingWheel.Visible = false;
    }

    private IMyAsyncResult beginActionLoadSaves() => (IMyAsyncResult) new MyLoadWorldInfoListResult();

    private void endActionLoadSaves(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
    {
      screen.CloseScreen();
      this.m_loadingWheel.Visible = false;
      MyWorkshopItem userData = this.m_worldsTable.SelectedRow.UserData as MyWorkshopItem;
      if (Directory.Exists(MyLocalCache.GetSessionSavesPath(MyUtils.StripInvalidChars(userData.Title), false, false)))
        this.OverwriteWorldDialog();
      else
        MyWorkshop.CreateWorldInstanceAsync(userData, MyWorkshop.MyWorkshopPathInfo.CreateWorldInfo(), false, (Action<bool, string>) ((success, sessionPath) =>
        {
          if (!success)
            return;
          this.OnSuccess(sessionPath);
        }));
    }

    private class LoadListResult : IMyAsyncResult
    {
      public List<MyWorkshopItem> SubscribedWorlds;

      public bool IsCompleted => this.Task.IsComplete;

      public Task Task { get; private set; }

      public LoadListResult() => this.Task = Parallel.Start((Action) (() => this.LoadListAsync(out this.SubscribedWorlds)));

      private void LoadListAsync(out List<MyWorkshopItem> list)
      {
        List<MyWorkshopItem> results1 = new List<MyWorkshopItem>();
        (MyGameServiceCallResult, string) result = MyWorkshop.GetSubscribedWorldsBlocking(results1);
        list = results1;
        List<MyWorkshopItem> results2 = new List<MyWorkshopItem>();
        (MyGameServiceCallResult, string) scenariosBlocking = MyWorkshop.GetSubscribedScenariosBlocking(results2);
        if (results2.Count > 0)
          list.InsertRange(list.Count, (IEnumerable<MyWorkshopItem>) results2);
        if (result.Item1 == MyGameServiceCallResult.OK)
          result = scenariosBlocking;
        if (result.Item1 == MyGameServiceCallResult.OK)
          return;
        MySandboxGame.Static.Invoke((Action) (() => MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder(MyWorkshop.GetWorkshopErrorText(result.Item1, result.Item2, true)), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)))), "LoadListAsyncError");
      }
    }
  }
}
