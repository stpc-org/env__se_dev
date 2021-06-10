// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenHelpSpace
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.GameSystems.Chat;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components.Session;
using VRage.Game.Definitions.Animation;
using VRage.GameServices;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyGuiScreenHelpSpace : MyGuiScreenBase
  {
    private static readonly MyGuiScreenHelpSpace.MyHackyQuestLogComparer m_hackyQuestComparer = new MyGuiScreenHelpSpace.MyHackyQuestLogComparer();
    public MyGuiControlList contentList;
    private MyGuiScreenHelpSpace.HelpPageEnum m_currentPage;
    private MyGuiControlMultilineText m_screenDescText;
    private MyGuiControlButton m_backButton;
    public static readonly List<string> TutorialPartsUrlsKeyboard = new List<string>()
    {
      "https://www.youtube.com/watch?v=wHa54ebUluE&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O",
      "https://www.youtube.com/watch?v=oDxa3vBbddE&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O",
      "https://www.youtube.com/watch?v=-pWmU06oT4s&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O",
      "https://www.youtube.com/watch?v=OsULjlQYWyg&index=4&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O",
      "https://www.youtube.com/watch?v=XvpNC9lkLwQ&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O",
      "https://www.youtube.com/watch?v=rCrnRxcwxKI&index=6&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O",
      "https://www.youtube.com/watch?v=23YHvmYLAuk&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O",
      "https://www.youtube.com/watch?v=q3jyDhIsMFw&index=8&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O",
      "https://www.youtube.com/watch?v=JvMbRMw_a2Q&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O",
      "https://www.youtube.com/watch?v=__G10lQXmXQ"
    };
    public static readonly List<string> TutorialPartsUrlsContoller = new List<string>()
    {
      "https://www.youtube.com/watch?v=wHa54ebUluE&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O",
      "https://www.youtube.com/watch?v=qdLi5V5dGH8",
      "https://www.youtube.com/watch?v=-pWmU06oT4s&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O",
      "https://www.youtube.com/watch?v=OsULjlQYWyg&index=4&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O",
      "https://www.youtube.com/watch?v=7_7bBwckAuw",
      "https://www.youtube.com/watch?v=rCrnRxcwxKI&index=6&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O",
      "https://www.youtube.com/watch?v=23YHvmYLAuk&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O",
      "https://www.youtube.com/watch?v=q3jyDhIsMFw&index=8&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O",
      "https://www.youtube.com/watch?v=JvMbRMw_a2Q&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O",
      "https://www.youtube.com/watch?v=__G10lQXmXQ"
    };

    public MyGuiScreenHelpSpace()
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.97f, 0.97f)), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      this.EnabledBackgroundFade = true;
      this.m_currentPage = MyGuiScreenHelpSpace.HelpPageEnum.Tutorials;
      this.CloseButtonEnabled = true;
      this.RecreateControls(true);
    }

    public override void RecreateControls(bool constructor)
    {
      int index1 = -1;
      for (int index2 = 0; index2 < this.Controls.Count; ++index2)
      {
        if (this.Controls[index2].HasFocus)
        {
          index1 = index2;
          if (this.Controls[index2] is MyGuiControlTable control)
          {
            int? selectedRowIndex = control.SelectedRowIndex;
          }
        }
      }
      base.RecreateControls(constructor);
      this.AddCaption(MyTexts.GetString(MyCommonTexts.HelpScreenHeader), captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.870000004768372 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.87f);
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.870000004768372 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.847000002861023)), this.m_size.Value.X * 0.87f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList);
      StringBuilder output1 = new StringBuilder();
      MyInput.Static.GetGameControl(MyControlsSpace.HELP_SCREEN).AppendBoundButtonNames(ref output1, ",", MyInput.Static.GetUnassignedName(), false);
      StringBuilder contents = new StringBuilder();
      contents.AppendFormat(MyTexts.GetString(MyCommonTexts.HelpScreen_Description), (object) output1);
      this.m_screenDescText = new MyGuiControlMultilineText(new Vector2?(new Vector2(-0.42f, 0.381f)), new Vector2?(new Vector2(0.4f, 0.2f)), contents: contents);
      this.m_screenDescText.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_screenDescText.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_screenDescText.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.Controls.Add((MyGuiControlBase) this.m_screenDescText);
      Vector2? position = new Vector2?(new Vector2(0.336f, 0.415f));
      Vector2? size = new Vector2?();
      Vector4? colorMask = new Vector4?();
      StringBuilder stringBuilder = MyTexts.Get(MyCommonTexts.ScreenMenuButtonBack);
      string toolTip = MyTexts.GetString(MySpaceTexts.ToolTipNewsletter_Close);
      StringBuilder text = stringBuilder;
      int? buttonIndex = new int?();
      this.m_backButton = new MyGuiControlButton(position, size: size, colorMask: colorMask, toolTip: toolTip, text: text, buttonIndex: buttonIndex);
      this.m_backButton.ButtonClicked += new Action<MyGuiControlButton>(this.backButton_ButtonClicked);
      this.Controls.Add((MyGuiControlBase) this.m_backButton);
      MyGuiControlPanel myGuiControlPanel1 = new MyGuiControlPanel(new Vector2?(new Vector2(-0.422f, -0.39f)), new Vector2?(new Vector2(0.211f, 0.035f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      myGuiControlPanel1.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
      MyGuiControlPanel myGuiControlPanel2 = myGuiControlPanel1;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.Position = myGuiControlPanel2.Position + new Vector2(0.01f, 0.005f);
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel1.Text = MyTexts.GetString(MyCommonTexts.HelpScreen_HomeSelectCategory);
      MyGuiControlLabel myGuiControlLabel2 = myGuiControlLabel1;
      this.Controls.Add((MyGuiControlBase) myGuiControlPanel2);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      MyGuiControlTable myGuiControlTable = new MyGuiControlTable();
      myGuiControlTable.Position = myGuiControlPanel2.Position + new Vector2(0.0f, 0.033f);
      myGuiControlTable.Size = new Vector2(0.211f, 0.5f);
      myGuiControlTable.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlTable.ColumnsCount = 1;
      myGuiControlTable.VisibleRowsCount = 20;
      myGuiControlTable.HeaderVisible = false;
      MyGuiControlTable table = myGuiControlTable;
      table.SetCustomColumnWidths(new float[1]{ 1f });
      table.ItemSelected += new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.OnTableItemSelected);
      this.Controls.Add((MyGuiControlBase) table);
      this.AddHelpScreenCategory(table, MyTexts.GetString(MyCommonTexts.HelpScreen_Tutorials), MyGuiScreenHelpSpace.HelpPageEnum.Tutorials);
      this.AddHelpScreenCategory(table, MyTexts.GetString(MyCommonTexts.HelpScreen_BasicControls), MyGuiScreenHelpSpace.HelpPageEnum.BasicControls);
      this.AddHelpScreenCategory(table, MyTexts.GetString(MyCommonTexts.HelpScreen_AdvancedControls), MyGuiScreenHelpSpace.HelpPageEnum.AdvancedControls);
      this.AddHelpScreenCategory(table, MyTexts.GetString(MyCommonTexts.HelpScreen_Gamepad), MyGuiScreenHelpSpace.HelpPageEnum.Controller);
      this.AddHelpScreenCategory(table, MyTexts.GetString(MyCommonTexts.HelpScreen_GamepadAdvanced), MyGuiScreenHelpSpace.HelpPageEnum.ControllerAdvanced);
      this.AddHelpScreenCategory(table, MyTexts.GetString(MyCommonTexts.HelpScreen_Chat), MyGuiScreenHelpSpace.HelpPageEnum.Chat);
      this.AddHelpScreenCategory(table, MyTexts.GetString(MyCommonTexts.HelpScreen_Support), MyGuiScreenHelpSpace.HelpPageEnum.Support);
      this.AddHelpScreenCategory(table, MyTexts.GetString(MyCommonTexts.HelpScreen_IngameHelp), MyGuiScreenHelpSpace.HelpPageEnum.IngameHelp);
      this.AddHelpScreenCategory(table, MyTexts.GetString(MyCommonTexts.HelpScreen_Welcome), MyGuiScreenHelpSpace.HelpPageEnum.Welcome);
      this.AddHelpScreenCategory(table, MyTexts.GetString(MyCommonTexts.HelpScreen_ReportIssue), MyGuiScreenHelpSpace.HelpPageEnum.ReportIssue);
      table.SelectedRow = table.GetRow((int) this.m_currentPage);
      this.contentList = new MyGuiControlList(new Vector2?(myGuiControlPanel2.Position + new Vector2(0.22f, 0.0f)), new Vector2?(new Vector2(0.624f, 0.74f)));
      this.contentList.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.contentList.VisualStyle = MyGuiControlListStyleEnum.Dark;
      this.Controls.Add((MyGuiControlBase) this.contentList);
      switch (this.m_currentPage)
      {
        case MyGuiScreenHelpSpace.HelpPageEnum.Tutorials:
          this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          bool joystickLastUsed = MyInput.Static.IsJoystickLastUsed;
          if (!MyPlatformGameSettings.LIMITED_MAIN_MENU)
          {
            this.contentList.Controls.Add((MyGuiControlBase) this.AddImageLinkPanel("Textures\\GUI\\HelpScreen\\Intro.dds", MyTexts.GetString(MyCommonTexts.HelpScreen_Introduction), MyGuiScreenHelpSpace.GetTutorialPartUrl(0, joystickLastUsed)));
            this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          }
          this.contentList.Controls.Add((MyGuiControlBase) this.AddImageLinkPanel("Textures\\GUI\\HelpScreen\\BasicControls.dds", MyTexts.GetString(MyCommonTexts.HelpScreen_BasicControls), MyGuiScreenHelpSpace.GetTutorialPartUrl(1, joystickLastUsed)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddImageLinkPanel("Textures\\GUI\\HelpScreen\\GameModePossibilities.dds", MyTexts.GetString(MyCommonTexts.HelpScreen_PossibilitiesWithinTheGameModes), MyGuiScreenHelpSpace.GetTutorialPartUrl(2, joystickLastUsed)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddImageLinkPanel("Textures\\GUI\\HelpScreen\\DrillingRefiningAssembling.dds", MyTexts.GetString(MyCommonTexts.HelpScreen_DrillingRefiningAssemblingSurvival), MyGuiScreenHelpSpace.GetTutorialPartUrl(3, joystickLastUsed)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddImageLinkPanel("Textures\\GUI\\HelpScreen\\Building1stShip.dds", MyTexts.GetString(MyCommonTexts.HelpScreen_BuildingYour1stShipCreative), MyGuiScreenHelpSpace.GetTutorialPartUrl(4, joystickLastUsed)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddImageLinkPanel("Textures\\GUI\\HelpScreen\\Survival.dds", MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeSurvival), MyGuiScreenHelpSpace.GetTutorialPartUrl(9, joystickLastUsed)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          if (!MyPlatformGameSettings.LIMITED_MAIN_MENU)
          {
            this.contentList.Controls.Add((MyGuiControlBase) this.AddImageLinkPanel("Textures\\GUI\\HelpScreen\\ExperimentalMode.dds", MyTexts.GetString(MyCommonTexts.ExperimentalMode), MyGuiScreenHelpSpace.GetTutorialPartUrl(5, joystickLastUsed)));
            this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          }
          this.contentList.Controls.Add((MyGuiControlBase) this.AddImageLinkPanel("Textures\\GUI\\HelpScreen\\Building1stVehicle.dds", MyTexts.GetString(MyCommonTexts.HelpScreen_BuildingYour1stGroundVehicle), MyGuiScreenHelpSpace.GetTutorialPartUrl(6, joystickLastUsed)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          if (!MyPlatformGameSettings.LIMITED_MAIN_MENU)
          {
            this.contentList.Controls.Add((MyGuiControlBase) this.AddImageLinkPanel("Textures\\GUI\\HelpScreen\\SteamWorkshopBlueprints.dds", MyTexts.GetString(MyCommonTexts.HelpScreen_SteamWorkshopAndBlueprints), MyGuiScreenHelpSpace.GetTutorialPartUrl(7, joystickLastUsed)));
            this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          }
          this.contentList.Controls.Add((MyGuiControlBase) this.AddImageLinkPanel("Textures\\GUI\\HelpScreen\\OtherAdvice.dds", MyTexts.GetString(MyCommonTexts.HelpScreen_OtherAdviceClosingThoughts), MyGuiScreenHelpSpace.GetTutorialPartUrl(8, joystickLastUsed)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          if (!MyPlatformGameSettings.LIMITED_MAIN_MENU)
          {
            this.contentList.Controls.Add((MyGuiControlBase) this.AddImageLinkPanel("Textures\\GUI\\HelpScreen\\SteamLink.dds", string.Format(MyTexts.GetString(MyCommonTexts.HelpScreen_TutorialsLinkSteam), (object) MyGameService.Service.ServiceName), "http://steamcommunity.com/app/244850/guides"));
            this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          }
          this.contentList.Controls.Add((MyGuiControlBase) this.AddImageLinkPanel("Textures\\GUI\\HelpScreen\\WikiLink.dds", MyTexts.GetString(MyCommonTexts.HelpScreen_TutorialsLinkWiki), "http://spaceengineerswiki.com/Main_Page"));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          break;
        case MyGuiScreenHelpSpace.HelpPageEnum.BasicControls:
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTextPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_BasicDescription)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeNavigation) + ":"));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.AddControlsByType(MyGuiControlTypeEnum.Navigation);
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeSystems1) + ":"));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.AddControlsByType(MyGuiControlTypeEnum.Systems1);
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("CTRL + " + this.GetControlButtonName(MyControlsSpace.DAMPING), MyTexts.GetString(MySpaceTexts.ControlName_RelativeDampening)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeSystems2) + ":"));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.AddControlsByType(MyGuiControlTypeEnum.Systems2);
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeSystems3) + ":"));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.AddControlsByType(MyGuiControlTypeEnum.Systems3);
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeToolsOrWeapons) + ":"));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.AddControlsByType(MyGuiControlTypeEnum.ToolsOrWeapons);
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeView) + ":"));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.AddControlsByType(MyGuiControlTypeEnum.Spectator);
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          break;
        case MyGuiScreenHelpSpace.HelpPageEnum.AdvancedControls:
          StringBuilder output2 = (StringBuilder) null;
          MyInput.Static.GetGameControl(MyControlsSpace.CUBE_COLOR_CHANGE).AppendBoundButtonNames(ref output2, unassignedText: MyInput.Static.GetUnassignedName());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTextPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_AdvancedDescription)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_AdvancedGeneral)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("F10", MyTexts.Get(MySpaceTexts.OpenBlueprints).ToString()));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("SHIFT + F10", MyTexts.Get(MySpaceTexts.OpenSpawnScreen).ToString()));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("ALT + F10", MyTexts.Get(MySpaceTexts.OpenAdminScreen).ToString()));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("F5", MyTexts.GetString(MyCommonTexts.ControlDescQuickLoad)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("SHIFT + F5", MyTexts.GetString(MyCommonTexts.ControlDescQuickSave)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("CTRL + H", MyTexts.GetString(MySpaceTexts.ControlDescNetgraph)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("F3", MyTexts.GetString(MyCommonTexts.ControlDescPlayersList)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_AdvancedGridsAndBlueprints)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("CTRL + B", MyTexts.Get(MySpaceTexts.CreateManageBlueprints).ToString()));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyTexts.GetString(MyCommonTexts.MouseWheel), MyTexts.GetString(MyCommonTexts.ControlName_ChangeBlockVariants)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("Ctrl + " + MyTexts.GetString(MyCommonTexts.MouseWheel), MyTexts.GetString(MyCommonTexts.ControlDescCopyPasteMove)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("CTRL + C", MyTexts.Get(MySpaceTexts.CopyObject).ToString()));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("CTRL + SHIFT + C", MyTexts.Get(MySpaceTexts.CopyObjectDetached).ToString()));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("CTRL + V", MyTexts.Get(MySpaceTexts.PasteObject).ToString()));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("CTRL + X", MyTexts.Get(MySpaceTexts.CutObject).ToString()));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("CTRL + Del", MyTexts.Get(MySpaceTexts.DeleteObject).ToString()));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("CTRL + ALT + E", MyTexts.GetString(MyCommonTexts.ControlDescExportModel)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_AdvancedCamera)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("Alt + " + MyTexts.Get(MyCommonTexts.MouseWheel).ToString(), MyTexts.Get(MySpaceTexts.ControlDescZoom).ToString()));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(this.GetControlButtonName(MyControlsSpace.SWITCH_LEFT), this.GetControlButtonDescription(MyControlsSpace.SWITCH_LEFT)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(this.GetControlButtonName(MyControlsSpace.SWITCH_RIGHT), this.GetControlButtonDescription(MyControlsSpace.SWITCH_RIGHT)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_AdvancedColorPicker)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(this.GetControlButtonName(MyControlsSpace.LANDING_GEAR), this.GetControlButtonDescription(MyControlsSpace.LANDING_GEAR)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("SHIFT + P", MyTexts.GetString(MySpaceTexts.PickColorFromCube)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(output2.ToString(), MyTexts.GetString(MySpaceTexts.ControlDescHoldToColor)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("CTRL + " + output2.ToString(), MyTexts.GetString(MySpaceTexts.ControlDescMediumBrush)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("SHIFT + " + output2.ToString(), MyTexts.GetString(MySpaceTexts.ControlDescLargeBrush)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("CTRL + SHIFT + " + output2.ToString(), MyTexts.GetString(MySpaceTexts.ControlDescWholeBrush)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_AdvancedVoxelHands)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(this.GetControlButtonName(MyControlsSpace.VOXEL_HAND_SETTINGS), MyTexts.GetString(MyCommonTexts.ControlDescOpenVoxelHandSettings)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("[", MyTexts.GetString(MyCommonTexts.ControlDescNextVoxelMaterial)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("]", MyTexts.GetString(MyCommonTexts.ControlDescPreviousVoxelMaterial)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("MMB", MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerPaint)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("CTRL + " + MyTexts.GetString(MyCommonTexts.RightMouseButton), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerRevert)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_AdvancedSpectator)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("CTRL + SPACE", MyTexts.GetString(MyCommonTexts.ControlDescMoveToSpectator)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("SHIFT + " + MyTexts.GetString(MyCommonTexts.MouseWheel), MyTexts.GetString(MySpaceTexts.ControlDescSpectatorSpeed)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_BuildPlanner)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          StringBuilder output3 = (StringBuilder) null;
          MyInput.Static.GetGameControl(MyControlsSpace.BUILD_PLANNER).AppendBoundButtonNames(ref output3, unassignedText: MyInput.Static.GetUnassignedName());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(output3.ToString(), MyTexts.GetString(MySpaceTexts.BuildPlanner_Withdraw)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("ALT + CTRL + " + output3.ToString(), MyTexts.GetString(MySpaceTexts.BuildPlanner_WithdrawKeep)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("CTRL + " + output3.ToString(), MyTexts.GetString(MySpaceTexts.BuildPlanner_Withdraw10Keep)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("SHIFT + " + output3.ToString(), MyTexts.GetString(MySpaceTexts.BuildPlanner_PutToProduction)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("SHIFT + CTRL + " + output3.ToString(), MyTexts.GetString(MySpaceTexts.BuildPlanner_Put10ToProduction)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("ALT + " + output3.ToString(), MyTexts.GetString(MySpaceTexts.BuildPlanner_DepositAll)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          break;
        case MyGuiScreenHelpSpace.HelpPageEnum.Controller:
          switch (MySandboxGame.Config.GamepadSchemeId)
          {
            case 1:
              this.contentList.Controls.Add((MyGuiControlBase) new MyGuiControlGamepadBindings(BindingType.Character, ControlScheme.Alternative));
              this.contentList.Controls.Add((MyGuiControlBase) new MyGuiControlGamepadBindings(BindingType.Jetpack, ControlScheme.Alternative));
              this.contentList.Controls.Add((MyGuiControlBase) new MyGuiControlGamepadBindings(BindingType.Ship, ControlScheme.Alternative));
              break;
            default:
              this.contentList.Controls.Add((MyGuiControlBase) new MyGuiControlGamepadBindings(BindingType.Character, ControlScheme.Default));
              this.contentList.Controls.Add((MyGuiControlBase) new MyGuiControlGamepadBindings(BindingType.Jetpack, ControlScheme.Default));
              this.contentList.Controls.Add((MyGuiControlBase) new MyGuiControlGamepadBindings(BindingType.Ship, ControlScheme.Default));
              break;
          }
          break;
        case MyGuiScreenHelpSpace.HelpPageEnum.ControllerAdvanced:
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTextPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_AdvancedDescription)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerShipControl)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          switch (MySandboxGame.Config.GamepadSchemeId)
          {
            case 1:
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPACESHIP, MyControlsSpace.FAKE_MOVEMENT_H), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHorizontalMover_Forward), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPACESHIP, MyControlsSpace.FAKE_MOVEMENT_V), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerVerticalMover_Up), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.FAKE_RB_V), MyTexts.GetString(MySpaceTexts.HelpScreen_ZoomCamera), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.DAMPING), MyTexts.GetString(MySpaceTexts.Dampeners), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.DAMPING_RELATIVE), MyTexts.GetString(MySpaceTexts.Dampeners_Relative), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPACESHIP, MyControlsSpace.WHEEL_JUMP), MyTexts.GetString(MySpaceTexts.BlockActionTitle_Jump), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.HEADLIGHTS), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_Lights), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.CAMERA_MODE), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_CameraMode), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPACESHIP, MyControlsSpace.TOGGLE_REACTORS), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_Reactors), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.TOOLBAR_NEXT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleShipToolbar), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.TOOLBAR_PREVIOUS), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleShipToolbar), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SWITCHER_LEFT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleEmoteToolbar), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SWITCHER_RIGHT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleEmoteToolbar), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(string.Format("{0} + {1} + {2}", (object) "\xE005", (object) "\xE006", (object) MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_DPAD", (string) null)), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerEmoteToolbarActions), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.ACTIVE_CONTRACT_SCREEN), MyTexts.GetString(MySpaceTexts.HelpScreen_Contracts), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.ADMIN_MENU), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ShowAdminMenu), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.TOGGLE_HUD), MyTexts.GetString(MySpaceTexts.HelpScreen_ToggleHud), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.CHAT_SCREEN), MyTexts.GetString(MySpaceTexts.HelpScreen_Chat), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.FAKE_CAMERA_ZOOM), MyTexts.GetString(MySpaceTexts.HelpScreen_ZoomCamera), new Color?(Color.White), true));
              break;
            default:
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.ROLL) + " + " + MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_ROTATION", (string) null), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerRoll), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.DAMPING), MyTexts.GetString(MySpaceTexts.Dampeners), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.DAMPING_RELATIVE), MyTexts.GetString(MySpaceTexts.Dampeners_Relative), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPACESHIP, MyControlsSpace.WHEEL_JUMP), MyTexts.GetString(MySpaceTexts.BlockActionTitle_Jump), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.HEADLIGHTS), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_Lights), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.CAMERA_MODE), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_CameraMode), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPACESHIP, MyControlsSpace.TOGGLE_REACTORS), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_Reactors), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.TOOLBAR_NEXT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleShipToolbar), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.TOOLBAR_PREVIOUS), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleShipToolbar), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SWITCHER_LEFT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleEmoteToolbar), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SWITCHER_RIGHT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleEmoteToolbar), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(string.Format("{0} + {1} + {2}", (object) "\xE005", (object) "\xE006", (object) MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_DPAD", (string) null)), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerEmoteToolbarActions), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.ACTIVE_CONTRACT_SCREEN), MyTexts.GetString(MySpaceTexts.HelpScreen_Contracts), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.ADMIN_MENU), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ShowAdminMenu), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.TOGGLE_HUD), MyTexts.GetString(MySpaceTexts.HelpScreen_ToggleHud), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.CHAT_SCREEN), MyTexts.GetString(MySpaceTexts.HelpScreen_Chat), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.FAKE_CAMERA_ZOOM), MyTexts.GetString(MySpaceTexts.HelpScreen_ZoomCamera), new Color?(Color.White), true));
              break;
          }
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.BLUEPRINTS_MENU), MyTexts.GetString(MySpaceTexts.BlueprintsScreen), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.WARNING_SCREEN), MyTexts.GetString(MySpaceTexts.HelpScreen_Warnings), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_TOOLS, MyControlsSpace.BROADCASTING), MyTexts.GetString(MySpaceTexts.ControlName_Broadcasting), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPACESHIP, MyControlsSpace.LOOKAROUND) + " + " + MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_ROTATION", (string) null), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerLookAround), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_ROTATION", (string) null), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerLookAround_PassengerSeat), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerTurretControl)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_ROTATION", (string) null), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerLookAround), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCharacterControl)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          switch (MySandboxGame.Config.GamepadSchemeId)
          {
            case 1:
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.FAKE_RB_V), MyTexts.GetString(MySpaceTexts.HelpScreen_ZoomCamera), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.DAMPING), MyTexts.GetString(MySpaceTexts.Dampeners), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.DAMPING_RELATIVE), MyTexts.GetString(MySpaceTexts.Dampeners_Relative), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.HELMET), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_Helmet), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.HEADLIGHTS), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_Lights), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.CAMERA_MODE), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_CameraMode), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.COLOR_TOOL), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerColorTool), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SWITCHER_LEFT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleEmoteToolbar), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SWITCHER_RIGHT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleEmoteToolbar), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.CONSUME_HEALTH), MyTexts.GetString(MySpaceTexts.DisplayName_Item_Medkit), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.ADMIN_MENU), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ShowAdminMenu), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(string.Format("{0} + {1} + {2}", (object) "\xE005", (object) "\xE006", (object) MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_DPAD", (string) null)), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerEmoteToolbarActions), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.FAKE_CAMERA_ZOOM), MyTexts.GetString(MySpaceTexts.HelpScreen_ZoomCamera), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.FAKE_RB_LS_H), MyTexts.GetString(MySpaceTexts.HelpScreen_Strafe), new Color?(Color.White), true));
              break;
            default:
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.DAMPING), MyTexts.GetString(MySpaceTexts.Dampeners), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.DAMPING_RELATIVE), MyTexts.GetString(MySpaceTexts.Dampeners_Relative), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.HELMET), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_Helmet), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.HEADLIGHTS), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_Lights), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.CAMERA_MODE), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_CameraMode), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.COLOR_TOOL), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerColorTool), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SWITCHER_LEFT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleEmoteToolbar), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SWITCHER_RIGHT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleEmoteToolbar), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.CONSUME_HEALTH), MyTexts.GetString(MySpaceTexts.DisplayName_Item_Medkit), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.ADMIN_MENU), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ShowAdminMenu), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(string.Format("{0} + {1} + {2}", (object) "\xE005", (object) "\xE006", (object) MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_DPAD", (string) null)), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerEmoteToolbarActions), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.FAKE_CAMERA_ZOOM), MyTexts.GetString(MySpaceTexts.HelpScreen_ZoomCamera), new Color?(Color.White), true));
              break;
          }
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.BLUEPRINTS_MENU), MyTexts.GetString(MySpaceTexts.BlueprintsScreen), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.WARNING_SCREEN), MyTexts.GetString(MySpaceTexts.HelpScreen_Warnings), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_TOOLS, MyControlsSpace.BROADCASTING), MyTexts.GetString(MySpaceTexts.ControlName_Broadcasting), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerJetpackControl)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          switch (MySandboxGame.Config.GamepadSchemeId)
          {
            case 1:
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPACESHIP, MyControlsSpace.FAKE_MOVEMENT_H), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHorizontalMover_Forward), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPACESHIP, MyControlsSpace.FAKE_MOVEMENT_V), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerVerticalMover_Up), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.FAKE_RB_V), MyTexts.GetString(MySpaceTexts.HelpScreen_ZoomCamera), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.DAMPING), MyTexts.GetString(MySpaceTexts.Dampeners), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.DAMPING_RELATIVE), MyTexts.GetString(MySpaceTexts.Dampeners_Relative), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.HELMET), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_Helmet), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.HEADLIGHTS), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_Lights), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.CAMERA_MODE), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_CameraMode), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.COLOR_TOOL), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerColorTool), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SWITCHER_LEFT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleEmoteToolbar), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SWITCHER_RIGHT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleEmoteToolbar), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.CONSUME_HEALTH), MyTexts.GetString(MySpaceTexts.DisplayName_Item_Medkit), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.ADMIN_MENU), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ShowAdminMenu), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(string.Format("{0} + {1} + {2}", (object) "\xE005", (object) "\xE006", (object) MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_DPAD", (string) null)), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerEmoteToolbarActions), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.FAKE_CAMERA_ZOOM), MyTexts.GetString(MySpaceTexts.HelpScreen_ZoomCamera), new Color?(Color.White), true));
              break;
            default:
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.ROLL) + " + " + MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_ROTATION", (string) null), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerRoll), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.DAMPING), MyTexts.GetString(MySpaceTexts.Dampeners), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.DAMPING_RELATIVE), MyTexts.GetString(MySpaceTexts.Dampeners_Relative), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.HELMET), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_Helmet), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.HEADLIGHTS), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_Lights), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.CAMERA_MODE), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_CameraMode), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.COLOR_TOOL), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerColorTool), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SWITCHER_LEFT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleEmoteToolbar), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SWITCHER_RIGHT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleEmoteToolbar), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.CONSUME_HEALTH), MyTexts.GetString(MySpaceTexts.DisplayName_Item_Medkit), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.ADMIN_MENU), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ShowAdminMenu), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(string.Format("{0} + {1} + {2}", (object) "\xE005", (object) "\xE006", (object) MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_DPAD", (string) null)), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerEmoteToolbarActions), new Color?(Color.White), true));
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.FAKE_CAMERA_ZOOM), MyTexts.GetString(MySpaceTexts.HelpScreen_ZoomCamera), new Color?(Color.White), true));
              break;
          }
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.BLUEPRINTS_MENU), MyTexts.GetString(MySpaceTexts.BlueprintsScreen), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.WARNING_SCREEN), MyTexts.GetString(MySpaceTexts.HelpScreen_Warnings), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_TOOLS, MyControlsSpace.BROADCASTING), MyTexts.GetString(MySpaceTexts.ControlName_Broadcasting), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_Tools)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_TOOLS, MyControlsSpace.ACTIVE_CONTRACT_SCREEN), MyTexts.GetString(MySpaceTexts.HelpScreen_Contracts), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_TOOLS, MyControlsSpace.TOGGLE_HUD), MyTexts.GetString(MySpaceTexts.HelpScreen_ToggleHud), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_TOOLS, MyControlsSpace.CHAT_SCREEN), MyTexts.GetString(MySpaceTexts.HelpScreen_Chat), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_TOOLS, MyControlsSpace.PROGRESSION_MENU), MyTexts.GetString(MySpaceTexts.HelpScreen_Progression), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCharacterSurvival)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.TOGGLE_SIGNALS), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ToggleSignals), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCharacterCreative)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.VOXEL_SELECT_SPHERE), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_EquipVoxelhand), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.BuildPlanner)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.BUILD_PLANNER_DEPOSIT_ORE), MyTexts.GetString(MySpaceTexts.BuildPlanner_DepositAll), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.BUILD_PLANNER_ADD_COMPONNETS), MyTexts.GetString(MySpaceTexts.BuildPlanner_PutToProduction), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.BUILD_PLANNER_WITHDRAW_COMPONENTS), MyTexts.GetString(MySpaceTexts.BuildPlanner_Withdraw), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.TERMINAL), MyTexts.GetString(MySpaceTexts.TerminalAccess), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.Spectator)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.FAKE_LS), MyTexts.GetString(MySpaceTexts.Spectator_HorizontalMovement), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.FAKE_LB_ROTATION_H), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerRoll), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.FAKE_RS), MyTexts.GetString(MySpaceTexts.Spectator_Rotation), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.FAKE_LS_PRESS), MyTexts.GetString(MySpaceTexts.Spectator_BlockRadialMenu), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.FAKE_RS_PRESS), MyTexts.GetString(MySpaceTexts.Spectator_SystemRadialMenu), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.SPECTATOR_FOCUS_PLAYER), MyTexts.GetString(MySpaceTexts.Spectator_FocusPlayer), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.SPECTATOR_PLAYER_CONTROL), MyTexts.GetString(MySpaceTexts.Spectator_PlayerControl), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.SPECTATOR_LOCK), MyTexts.GetString(MyCommonTexts.ControlName_SpectatorLock), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.SPECTATOR_TELEPORT), MyTexts.GetString(MySpaceTexts.Spectator_Teleport), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.SPECTATOR_SPEED_BOOST), MyTexts.GetString(MySpaceTexts.Spectator_SpeedBoost), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.SPECTATOR_CHANGE_SPEED_UP), MyTexts.GetString(MySpaceTexts.Spectator_SpeedUp), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.SPECTATOR_CHANGE_SPEED_DOWN), MyTexts.GetString(MySpaceTexts.Spectator_SpeedDown), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.SPECTATOR_CHANGE_ROTATION_SPEED_UP), MyTexts.GetString(MySpaceTexts.Spectator_RotationSpeedUp), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.SPECTATOR_CHANGE_ROTATION_SPEED_DOWN), MyTexts.GetString(MySpaceTexts.Spectator_RotationSpeedDown), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerBuilding)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.NEXT_BLOCK_STAGE), MyTexts.GetString(MyCommonTexts.ControlName_ChangeBlockVariants), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.CUBE_BUILDER_CUBESIZE_MODE), MyTexts.GetString(MyCommonTexts.ControlName_CubeSizeMode), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.CUBE_DEFAULT_MOUNTPOINT), MyTexts.GetString(MySpaceTexts.ControlName_BlockAutorotation), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerBuildingSurvival)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.SECONDARY_TOOL_ACTION), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerSecondaryBuildSurvival), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerBuildingCreative)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.SECONDARY_TOOL_ACTION), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerSecondayBuildCreative), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.SYMMETRY_SWITCH), MyTexts.GetString(MySpaceTexts.ControlName_UseSymmetry), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerPlacing)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.PRIMARY_TOOL_ACTION), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerPlace), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.FREE_ROTATION), MyTexts.GetString(MySpaceTexts.StationRotation_Static), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.SECONDARY_TOOL_ACTION), MyTexts.GetString(MyCommonTexts.Cancel), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.ROTATE_AXIS_LEFT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerRotateCw), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.ROTATE_AXIS_RIGHT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerRotateCcw), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.CHANGE_ROTATION_AXIS), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerChangeRotationAxis), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.MOVE_FURTHER), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerFurther), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.MOVE_CLOSER), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCloser), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.SLOT0), MyTexts.GetString(MySpaceTexts.HelpScreen_SymmetryUnequip), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.ControlName_SymmetrySwitch)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_SYMMETRY, MyControlsSpace.SECONDARY_TOOL_ACTION), MyTexts.GetString(MySpaceTexts.HelpScreen_ResetPlane), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_SYMMETRY, MyControlsSpace.PRIMARY_TOOL_ACTION), MyTexts.GetString(MySpaceTexts.HelpScreen_SetPlane), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_SYMMETRY, MyControlsSpace.NEXT_BLOCK_STAGE), MyTexts.GetString(MySpaceTexts.HelpScreen_SymmetryTurnOffSetup), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_SYMMETRY, MyControlsSpace.CHANGE_ROTATION_AXIS), MyTexts.GetString(MySpaceTexts.HelpScreen_SymmetryNextPlane), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_SYMMETRY, MyControlsSpace.SLOT0), MyTexts.GetString(MySpaceTexts.HelpScreen_SymmetryUnequip), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.RadialMenuGroupTitle_VoxelHandBrushes)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.PRIMARY_TOOL_ACTION), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerPlace), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.SECONDARY_TOOL_ACTION), MyTexts.GetString(MySpaceTexts.Remove), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.VOXEL_PAINT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerPaint), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.VOXEL_REVERT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerRevert), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.VOXEL_SCALE_DOWN), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerScaleDown), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.VOXEL_SCALE_UP), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerScaleUp), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.VOXEL_MATERIAL_SELECT), MyTexts.GetString(MySpaceTexts.RadialMenu_Materials), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.VOXEL_HAND_SETTINGS), MyTexts.GetString(MyCommonTexts.ControlDescOpenVoxelHandSettings), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.ROTATE_AXIS_RIGHT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerRotateCw), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.CHANGE_ROTATION_AXIS), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerChangeRotationAxis), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.MOVE_FURTHER), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerFurther), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.MOVE_CLOSER), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCloser), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.SLOT0), MyTexts.GetString(MySpaceTexts.HelpScreen_SymmetryUnequip), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerColorTool)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.PRIMARY_TOOL_ACTION), MyTexts.GetString(MySpaceTexts.ControlDescHoldToColor), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.SECONDARY_TOOL_ACTION), MyTexts.GetString(MySpaceTexts.PickColorFromCube), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.MEDIUM_COLOR_BRUSH), MyTexts.GetString(MySpaceTexts.ControlDescMediumBrush), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.LARGE_COLOR_BRUSH), MyTexts.GetString(MySpaceTexts.ControlDescLargeBrush), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.RECOLOR_WHOLE_GRID), MyTexts.GetString(MySpaceTexts.ControlDescWholeBrush), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.CYCLE_COLOR_LEFT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerColorPrevious), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.CYCLE_COLOR_RIGHT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerColorNext), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.CYCLE_SKIN_LEFT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerSkinPrevious), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.CYCLE_SKIN_RIGHT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerSkinNext), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.SLOT0), MyTexts.GetString(MySpaceTexts.HelpScreen_SymmetryUnequip), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_Inventory)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_A), MyTexts.GetString(MyCommonTexts.HelpScreen_Inventory_Transfer), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(string.Format("{0} + {1}", (object) MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_LEFT), (object) MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_A)), MyTexts.GetString(MyCommonTexts.HelpScreen_Inventory_Transfer10), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(string.Format("{0} + {1}", (object) MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT), (object) MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_A)), MyTexts.GetString(MyCommonTexts.HelpScreen_Inventory_Transfer100), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(string.Format("{0} + {1} + {2}", (object) MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_LEFT), (object) MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT), (object) MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_A)), MyTexts.GetString(MyCommonTexts.HelpScreen_Inventory_Transfer1000), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_A), MyTexts.GetString(MyCommonTexts.HelpScreen_Inventory_Split), new Color?(Color.White), true));
          MyGuiControls controls1 = this.contentList.Controls;
          string codeForControl1 = MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT);
          char ch = '\xE00F';
          string str1 = ch.ToString();
          MyGuiControlParent guiControlParent1 = this.AddKeyPanel(string.Format("{0} + {1}", (object) codeForControl1, (object) str1), MyTexts.GetString(MyCommonTexts.HelpScreen_Inventory_MoveItem), new Color?(Color.White), true);
          controls1.Add((MyGuiControlBase) guiControlParent1);
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y), MyTexts.GetString(MyCommonTexts.HelpScreen_Inventory_UseItem), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_Production)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACCEPT), MyTexts.GetString(MyCommonTexts.HelpScreen_Production_Queue1), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(string.Format("{0} + {1}", (object) MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_LEFT), (object) MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACCEPT)), MyTexts.GetString(MyCommonTexts.HelpScreen_Production_Queue10), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(string.Format("{0} + {1}", (object) MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT), (object) MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACCEPT)), MyTexts.GetString(MyCommonTexts.HelpScreen_Production_Queue100), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(string.Format("{0} + {1} + {2}", (object) MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_LEFT), (object) MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT), (object) MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACCEPT)), MyTexts.GetString(MyCommonTexts.HelpScreen_Production_Queue1000), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACCEPT), MyTexts.GetString(MyCommonTexts.HelpScreen_Production_Dequeue), new Color?(Color.White), true));
          MyGuiControls controls2 = this.contentList.Controls;
          string codeForControl2 = MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT);
          ch = '\xE00F';
          string str2 = ch.ToString();
          MyGuiControlParent guiControlParent2 = this.AddKeyPanel(string.Format("{0} + {1}", (object) codeForControl2, (object) str2), MyTexts.GetString(MyCommonTexts.HelpScreen_Production_MoveItem), new Color?(Color.White), true);
          controls2.Add((MyGuiControlBase) guiControlParent2);
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_GeneralUI)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_Slider), string.Empty, new Color?(Color.White)));
          MyGuiControls controls3 = this.contentList.Controls;
          ch = '\xE026';
          MyGuiControlParent guiControlParent3 = this.AddKeyPanel(ch.ToString(), MyTexts.GetString(MyCommonTexts.HelpScreen_Slider_Move1), new Color?(Color.White), true);
          controls3.Add((MyGuiControlBase) guiControlParent3);
          MyGuiControls controls4 = this.contentList.Controls;
          string codeForControl3 = MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_LEFT);
          ch = '\xE026';
          string str3 = ch.ToString();
          MyGuiControlParent guiControlParent4 = this.AddKeyPanel(string.Format("{0} + {1}", (object) codeForControl3, (object) str3), MyTexts.GetString(MyCommonTexts.HelpScreen_Slider_Move10), new Color?(Color.White), true);
          controls4.Add((MyGuiControlBase) guiControlParent4);
          MyGuiControls controls5 = this.contentList.Controls;
          string codeForControl4 = MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT);
          ch = '\xE026';
          string str4 = ch.ToString();
          MyGuiControlParent guiControlParent5 = this.AddKeyPanel(string.Format("{0} + {1}", (object) codeForControl4, (object) str4), MyTexts.GetString(MyCommonTexts.HelpScreen_Slider_Move100), new Color?(Color.White), true);
          controls5.Add((MyGuiControlBase) guiControlParent5);
          MyGuiControls controls6 = this.contentList.Controls;
          string codeForControl5 = MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_LEFT);
          string codeForControl6 = MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT);
          ch = '\xE026';
          string str5 = ch.ToString();
          MyGuiControlParent guiControlParent6 = this.AddKeyPanel(string.Format("{0} + {1} + {2}", (object) codeForControl5, (object) codeForControl6, (object) str5), MyTexts.GetString(MyCommonTexts.HelpScreen_Slider_MoveHalf), new Color?(Color.White), true);
          controls6.Add((MyGuiControlBase) guiControlParent6);
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_Numeric), string.Empty, new Color?(Color.White)));
          MyGuiControls controls7 = this.contentList.Controls;
          ch = '\xE026';
          MyGuiControlParent guiControlParent7 = this.AddKeyPanel(ch.ToString(), MyTexts.GetString(MyCommonTexts.HelpScreen_Numeric_Move1), new Color?(Color.White), true);
          controls7.Add((MyGuiControlBase) guiControlParent7);
          MyGuiControls controls8 = this.contentList.Controls;
          string codeForControl7 = MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_LEFT);
          ch = '\xE026';
          string str6 = ch.ToString();
          MyGuiControlParent guiControlParent8 = this.AddKeyPanel(string.Format("{0} + {1}", (object) codeForControl7, (object) str6), MyTexts.GetString(MyCommonTexts.HelpScreen_Numeric_Move10), new Color?(Color.White), true);
          controls8.Add((MyGuiControlBase) guiControlParent8);
          MyGuiControls controls9 = this.contentList.Controls;
          string codeForControl8 = MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT);
          ch = '\xE026';
          string str7 = ch.ToString();
          MyGuiControlParent guiControlParent9 = this.AddKeyPanel(string.Format("{0} + {1}", (object) codeForControl8, (object) str7), MyTexts.GetString(MyCommonTexts.HelpScreen_Numeric_Move100), new Color?(Color.White), true);
          controls9.Add((MyGuiControlBase) guiControlParent9);
          MyGuiControls controls10 = this.contentList.Controls;
          string codeForControl9 = MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_LEFT);
          string codeForControl10 = MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT);
          ch = '\xE026';
          string str8 = ch.ToString();
          MyGuiControlParent guiControlParent10 = this.AddKeyPanel(string.Format("{0} + {1} + {2}", (object) codeForControl9, (object) codeForControl10, (object) str8), MyTexts.GetString(MyCommonTexts.HelpScreen_Numeric_Move1000), new Color?(Color.White), true);
          controls10.Add((MyGuiControlBase) guiControlParent10);
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_Listbox), string.Empty, new Color?(Color.White)));
          MyGuiControls controls11 = this.contentList.Controls;
          ch = '\xE027';
          MyGuiControlParent guiControlParent11 = this.AddKeyPanel(ch.ToString(), MyTexts.GetString(MyCommonTexts.HelpScreen_Listbox_SelectMove), new Color?(Color.White), true);
          controls11.Add((MyGuiControlBase) guiControlParent11);
          MyGuiControls controls12 = this.contentList.Controls;
          string codeForControl11 = MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_LEFT);
          ch = '\xE027';
          string str9 = ch.ToString();
          MyGuiControlParent guiControlParent12 = this.AddKeyPanel(string.Format("{0} + {1}", (object) codeForControl11, (object) str9), MyTexts.GetString(MyCommonTexts.HelpScreen_Listbox_AddMove), new Color?(Color.White), true);
          controls12.Add((MyGuiControlBase) guiControlParent12);
          MyGuiControls controls13 = this.contentList.Controls;
          string codeForControl12 = MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT);
          ch = '\xE027';
          string str10 = ch.ToString();
          MyGuiControlParent guiControlParent13 = this.AddKeyPanel(string.Format("{0} + {1}", (object) codeForControl12, (object) str10), MyTexts.GetString(MyCommonTexts.HelpScreen_Listbox_ToggleMove), new Color?(Color.White), true);
          controls13.Add((MyGuiControlBase) guiControlParent13);
          MyGuiControls controls14 = this.contentList.Controls;
          string codeForControl13 = MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_LEFT);
          string codeForControl14 = MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT);
          ch = '\xE027';
          string str11 = ch.ToString();
          MyGuiControlParent guiControlParent14 = this.AddKeyPanel(string.Format("{0} + {1} + {2}", (object) codeForControl13, (object) codeForControl14, (object) str11), MyTexts.GetString(MyCommonTexts.HelpScreen_Listbox_Move), new Color?(Color.White), true);
          controls14.Add((MyGuiControlBase) guiControlParent14);
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_A), MyTexts.GetString(MyCommonTexts.HelpScreen_Listbox_SelectCurrent), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(string.Format("{0} + {1}", (object) MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_LEFT), (object) MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_A)), MyTexts.GetString(MyCommonTexts.HelpScreen_Listbox_SelectRange), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(string.Format("{0} + {1}", (object) MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT), (object) MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_A)), MyTexts.GetString(MyCommonTexts.HelpScreen_Listbox_ToggleCurrent), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(string.Format("{0} + {1} + {2}", (object) MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_LEFT), (object) MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT), (object) MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_A)), MyTexts.GetString(MyCommonTexts.HelpScreen_Listbox_DeSelectAll), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_ActionSetup)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.PRIMARY_TOOL_ACTION), MyTexts.GetString(MyCommonTexts.HelpScreen_GamepadAdvanced_NextToolbar), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.SECONDARY_TOOL_ACTION), MyTexts.GetString(MyCommonTexts.HelpScreen_GamepadAdvanced_PreviousToolbar), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_GeneralGame)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(string.Format("{0} + {1} + {2}", (object) MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.LEFT_BUTTON), (object) MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.RIGHT_BUTTON), (object) MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X)), MyTexts.GetString(MyCommonTexts.ControlDescQuickSave), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(string.Format("{0} + {1} + {2}", (object) MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.LEFT_BUTTON), (object) MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.RIGHT_BUTTON), (object) MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y)), MyTexts.GetString(MyCommonTexts.ControlDescQuickLoad), new Color?(Color.White), true));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTextPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHint1)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTextPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHint2)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTextPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHint3)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTextPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHint4)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTextPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHint5)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTextPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHint6)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTextPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHint7)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTextPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHint8)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTextPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHint10)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          break;
        case MyGuiScreenHelpSpace.HelpPageEnum.Chat:
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTextPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_ChatDescription)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_Header_Name) + ":"));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.AddChatColors_Name();
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_Header_Text) + ":"));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.AddChatColors_Text();
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Controls) + ":"));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.AddChatControls();
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Commands) + ":"));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.AddChatCommands();
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Emotes) + ":"));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.AddEmoteCommands();
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          break;
        case MyGuiScreenHelpSpace.HelpPageEnum.Support:
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTextPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_SupportDescription)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddImageLinkPanel("Textures\\GUI\\HelpScreen\\KSWLink.dds", MyTexts.GetString(MyCommonTexts.HelpScreen_SupportLinkUserResponse), "https://support.keenswh.com/"));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddImageLinkPanel("Textures\\GUI\\HelpScreen\\KSWLink.dds", MyTexts.GetString(MyCommonTexts.HelpScreen_SupportLinkForum), "http://forums.keenswh.com/"));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTextPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_SupportContactDescription)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddLinkPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_SupportContact), "mailto:support@keenswh.com"));
          break;
        case MyGuiScreenHelpSpace.HelpPageEnum.IngameHelp:
          this.AddIngameHelpContent(this.contentList);
          break;
        case MyGuiScreenHelpSpace.HelpPageEnum.Welcome:
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTextPanel(MyTexts.GetString(MyCommonTexts.ScreenCaptionWelcomeScreen)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTextPanel(MyTexts.GetString(MySpaceTexts.WelcomeScreen_Text1)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTextPanel(MyTexts.GetString(MySpaceTexts.WelcomeScreen_Text2)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTextPanel(string.Format(MyTexts.GetString(MySpaceTexts.WelcomeScreen_Text3), (object) MyGameService.Service.ServiceName)));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddSignaturePanel());
          break;
        case MyGuiScreenHelpSpace.HelpPageEnum.ReportIssue:
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTextPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_ReportIssue_Description)));
          MyGuiControlMultilineEditableText feedbackBox;
          this.contentList.Controls.Add((MyGuiControlBase) this.AddMultilineTextBox(out feedbackBox));
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTextPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_ReportIssue_Email)));
          MyGuiControlTextbox emailBox;
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTextBox(out emailBox));
          this.contentList.Controls.Add((MyGuiControlBase) this.MakeButton(new Vector2(0.0f, 0.0f), MyCommonTexts.HelpScreen_ReportIssue_SendReport, (Action<MyGuiControlButton>) (x => this.SendIssueReport(emailBox.Text, feedbackBox.Text.ToString()))));
          break;
        default:
          this.contentList.Controls.Add((MyGuiControlBase) this.AddTextPanel("Incorrect page selected"));
          break;
      }
      Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(new Vector2?(new Vector2(this.m_screenDescText.Position.X, this.m_backButton.Position.Y)));
      myGuiControlLabel3.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel3);
      this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.Gamepad_Help_Back);
      if (index1 != -1)
      {
        if (index1 >= this.Controls.Count)
          index1 = this.Controls.Count - 1;
        this.FocusedControl = this.Controls[index1];
      }
      else
        this.FocusedControl = (MyGuiControlBase) table;
    }

    public static string GetTutorialPartUrl(int index, bool forController)
    {
      if (index < 0)
        return string.Empty;
      if (forController && MyGuiScreenHelpSpace.TutorialPartsUrlsContoller.Count > index)
        return MyGuiScreenHelpSpace.TutorialPartsUrlsContoller[index];
      return !forController && MyGuiScreenHelpSpace.TutorialPartsUrlsKeyboard.Count > index ? MyGuiScreenHelpSpace.TutorialPartsUrlsKeyboard[index] : string.Empty;
    }

    private void SendIssueReport(string email, string message)
    {
      MyLog.Default.WriteLine("User" + (string.IsNullOrWhiteSpace(email) ? "" : " " + email) + " is Reporting issue:\n " + message);
      MyGuiScreenProgress waitScreen = new MyGuiScreenProgress(MyTexts.Get(MyCommonTexts.HelpScreen_ReportIssue_WaitForSending));
      MyGuiSandbox.AddScreen((MyGuiScreenBase) waitScreen);
      Parallel.Start((Action) (() =>
      {
        CrashInfo info = MyErrorReporter.BuildCrashInfo();
        MySandboxGame.Log.WriteLine(string.Format("\n{0}", (object) info));
        MyErrorReporter.ReportNotInteractive(MyLog.Default.GetFilePath(), MyPerGameSettings.BasicGameInfo.GameAcronym, true, (IEnumerable<string>) null, false, email, message, info);
      }), (Action) (() =>
      {
        waitScreen.CloseScreen();
        MyGuiSandbox.Show(MyCommonTexts.HelpScreen_ReportIssue_ThanksForSending, MyCommonTexts.HelpScreen_ReportIssue_ThanksForSendingCaption, MyMessageBoxStyleEnum.Info);
      }), WorkPriority.VeryHigh);
    }

    private void AddIngameHelpContent(MyGuiControlList contentList)
    {
      foreach (MyIngameHelpObjective ingameHelpObjective in MySessionComponentIngameHelp.GetFinishedObjectives().Reverse<MyIngameHelpObjective>())
      {
        contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyTexts.GetString(ingameHelpObjective.TitleEnum)));
        contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
        foreach (MyIngameHelpDetail detail in ingameHelpObjective.Details)
          contentList.Controls.Add((MyGuiControlBase) this.AddTextPanel(detail.Args == null ? MyTexts.GetString(detail.TextEnum) : string.Format(MyTexts.GetString(detail.TextEnum), detail.Args), 0.9f));
        contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
        contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
        contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
      }
      this.LearningToSurviveQuestLog(contentList);
    }

    private void LearningToSurviveQuestLog(MyGuiControlList contentList)
    {
      if (MySessionComponentScriptSharedStorage.Instance == null)
        return;
      Regex nameRegex1 = new Regex("O_..x.._IsFinished");
      Regex nameRegex2 = new Regex("O_..x.._IsFailed");
      string str1 = "Caption";
      List<KeyValuePair<string, bool>> list1 = MySessionComponentScriptSharedStorage.Instance.GetBoolsByRegex(nameRegex1).ToList<KeyValuePair<string, bool>>();
      List<KeyValuePair<string, bool>> list2 = MySessionComponentScriptSharedStorage.Instance.GetBoolsByRegex(nameRegex2).ToList<KeyValuePair<string, bool>>();
      list1.Sort((IComparer<KeyValuePair<string, bool>>) MyGuiScreenHelpSpace.m_hackyQuestComparer);
      list2.Sort((IComparer<KeyValuePair<string, bool>>) MyGuiScreenHelpSpace.m_hackyQuestComparer);
      int index = -1;
      foreach (KeyValuePair<string, bool> keyValuePair in list1)
      {
        ++index;
        if (keyValuePair.Value)
        {
          string str2 = keyValuePair.Key.Substring(0, 8);
          contentList.Controls.Add((MyGuiControlBase) this.AddKeyCategoryPanel(MyStatControlText.SubstituteTexts("{LOCC:" + MyTexts.GetString(str2 + str1) + "}")));
          contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          contentList.Controls.Add((MyGuiControlBase) this.AddTextPanel(MyStatControlText.SubstituteTexts("{LOCC:" + (list2[index].Value ? MyTexts.GetString("QuestlogDetail_Failed") : MyTexts.GetString("QuestlogDetail_Success")) + "}"), 0.9f));
          contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          contentList.Controls.Add((MyGuiControlBase) this.AddSeparatorPanel());
          contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
        }
      }
    }

    private MyGuiControlTable.Row AddHelpScreenCategory(
      MyGuiControlTable table,
      string rowName,
      MyGuiScreenHelpSpace.HelpPageEnum pageEnum)
    {
      MyGuiControlTable.Row row = new MyGuiControlTable.Row((object) pageEnum);
      StringBuilder text = new StringBuilder(rowName);
      row.AddCell(new MyGuiControlTable.Cell(text, toolTip: text.ToString(), textColor: new Color?(Color.White))
      {
        IsAutoScaleEnabled = true
      });
      table.Add(row);
      return row;
    }

    private MyGuiControlButton MakeButton(
      Vector2 position,
      MyStringId text,
      Action<MyGuiControlButton> onClick)
    {
      Vector2 backButtonSize = MyGuiConstants.BACK_BUTTON_SIZE;
      Vector4 buttonBackgroundColor = MyGuiConstants.BACK_BUTTON_BACKGROUND_COLOR;
      Vector4 backButtonTextColor = MyGuiConstants.BACK_BUTTON_TEXT_COLOR;
      float textScale = 0.8f;
      return new MyGuiControlButton(new Vector2?(position), size: new Vector2?(backButtonSize), colorMask: new Vector4?(buttonBackgroundColor), text: MyTexts.Get(text), textScale: textScale, onButtonClick: onClick);
    }

    private void AddControlsByType(MyGuiControlTypeEnum type)
    {
      DictionaryValuesReader<MyStringId, MyControl> gameControlsList = MyInput.Static.GetGameControlsList();
      int num = 0;
      foreach (MyControl control in gameControlsList)
      {
        if (control.GetControlTypeEnum() == type)
        {
          ++num;
          if (num % 5 == 0)
            this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(this.GetControlButtonName(control), this.GetControlButtonDescription(control)));
        }
      }
    }

    private void AddChatColors_Name()
    {
      this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_Name_Self), MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_NameDesc_Self), new Color?(Color.CornflowerBlue)));
      this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_Name_Ally), MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_NameDesc_Ally), new Color?(Color.LightGreen)));
      this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_Name_Neutral), MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_NameDesc_Neutral), new Color?(Color.PaleGoldenrod)));
      this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_Name_Enemy), MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_NameDesc_Enemy), new Color?(Color.Crimson)));
      this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_Name_Admin), MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_NameDesc_Admin), new Color?(Color.Purple)));
    }

    private void AddChatColors_Text()
    {
      this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_Text_Faction), MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_TextDesc_Faction), new Color?(Color.LimeGreen)));
      this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_Text_Private), MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_TextDesc_Private), new Color?(Color.Violet)));
      this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_Text_Global), MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_TextDesc_Global), new Color?(Color.White)));
    }

    private void AddChatControls()
    {
      this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("PageUp", MyTexts.GetString(MyCommonTexts.ChatCommand_HelpSimple_PageUp)));
      this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("PageDown", MyTexts.GetString(MyCommonTexts.ChatCommand_HelpSimple_PageDown)));
    }

    private void AddChatCommands()
    {
      if (MySession.Static == null)
      {
        this.contentList.Controls.Add((MyGuiControlBase) this.AddTextPanel(MyTexts.GetString(MyCommonTexts.ChatCommands_Menu)));
      }
      else
      {
        this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel("/? <question>", MyTexts.GetString(MyCommonTexts.ChatCommand_HelpSimple_Question)));
        int num = 1;
        foreach (KeyValuePair<string, IMyChatCommand> chatCommand in MySession.Static.ChatSystem.CommandSystem.ChatCommands)
        {
          this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyTexts.GetString(MyStringId.GetOrCompute(chatCommand.Value.CommandText)), MyTexts.GetString(MyStringId.GetOrCompute(chatCommand.Value.HelpSimpleText))));
          ++num;
          if (num % 5 == 0)
            this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
        }
      }
    }

    private void AddEmoteCommands()
    {
      if (MySession.Static == null)
      {
        this.contentList.Controls.Add((MyGuiControlBase) this.AddTextPanel(MyTexts.GetString(MyCommonTexts.ChatCommands_Menu)));
      }
      else
      {
        int num = 0;
        foreach (MyAnimationDefinition animationDefinition in MyDefinitionManager.Static.GetAnimationDefinitions())
        {
          if (!string.IsNullOrEmpty(animationDefinition.ChatCommandName))
          {
            this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyTexts.GetString(MyStringId.GetOrCompute(animationDefinition.ChatCommand)), MyTexts.GetString(MyStringId.GetOrCompute(animationDefinition.ChatCommandDescription))));
            ++num;
            if (num % 5 == 0)
              this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
          }
        }
        foreach (MyGameInventoryItem gameInventoryItem in MyGameService.InventoryItems.GroupBy<MyGameInventoryItem, MyGameInventoryItemDefinition>((Func<MyGameInventoryItem, MyGameInventoryItemDefinition>) (x => x.ItemDefinition)).Select<IGrouping<MyGameInventoryItemDefinition, MyGameInventoryItem>, MyGameInventoryItem>((Func<IGrouping<MyGameInventoryItemDefinition, MyGameInventoryItem>, MyGameInventoryItem>) (y => y.First<MyGameInventoryItem>())))
        {
          if (gameInventoryItem != null && gameInventoryItem.ItemDefinition != null && gameInventoryItem.ItemDefinition.ItemSlot == MyGameInventoryItemSlot.Emote)
          {
            MyEmoteDefinition definition = MyDefinitionManager.Static.GetDefinition<MyEmoteDefinition>(gameInventoryItem.ItemDefinition.AssetModifierId);
            if (definition != null && !string.IsNullOrWhiteSpace(definition.ChatCommandName))
            {
              this.contentList.Controls.Add((MyGuiControlBase) this.AddKeyPanel(MyTexts.GetString(MyStringId.GetOrCompute(definition.ChatCommand)), MyTexts.GetString(MyStringId.GetOrCompute(definition.ChatCommandDescription))));
              ++num;
              if (num % 5 == 0)
                this.contentList.Controls.Add((MyGuiControlBase) this.AddTinySpacePanel());
            }
          }
        }
      }
    }

    public string GetControlButtonName(MyStringId control)
    {
      MyControl gameControl = MyInput.Static.GetGameControl(control);
      StringBuilder stringBuilder = new StringBuilder();
      ref StringBuilder local = ref stringBuilder;
      string unassignedName = MyInput.Static.GetUnassignedName();
      gameControl.AppendBoundButtonNames(ref local, unassignedText: unassignedName);
      return stringBuilder.ToString();
    }

    public string GetControlButtonName(MyControl control)
    {
      StringBuilder output = new StringBuilder();
      control.AppendBoundButtonNames(ref output, unassignedText: MyInput.Static.GetUnassignedName());
      return output.ToString();
    }

    public string GetControlButtonDescription(MyStringId control) => MyTexts.GetString(MyInput.Static.GetGameControl(control).GetControlName());

    public string GetControlButtonDescription(MyControl control) => MyTexts.GetString(control.GetControlName());

    public override string GetFriendlyName() => "MyGuiScreenHelp";

    private void OnCloseClick(MyGuiControlButton sender) => this.CloseScreen();

    protected override void OnClosed()
    {
      base.OnClosed();
      MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) null;
    }

    private void backButton_ButtonClicked(MyGuiControlButton obj) => this.CloseScreen();

    private void OnTableItemSelected(MyGuiControlTable sender, MyGuiControlTable.EventArgs args)
    {
      if (sender.SelectedRow == null)
        return;
      this.m_currentPage = (MyGuiScreenHelpSpace.HelpPageEnum) sender.SelectedRow.UserData;
      this.RecreateControls(false);
    }

    private MyGuiControlParent AddTextPanel(
      string text,
      float textScaleMultiplier = 1f)
    {
      MyGuiControlMultilineText controlMultilineText = new MyGuiControlMultilineText();
      controlMultilineText.Size = new Vector2(0.588f, 0.5f);
      controlMultilineText.TextScale *= textScaleMultiplier;
      controlMultilineText.Text = new StringBuilder(text);
      controlMultilineText.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      controlMultilineText.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      controlMultilineText.PositionX += 0.015f;
      controlMultilineText.Parse();
      controlMultilineText.DisableLabelScissor();
      MyGuiControlParent guiControlParent = new MyGuiControlParent();
      guiControlParent.Size = new Vector2(0.588f, controlMultilineText.TextSize.Y + 0.01f);
      guiControlParent.Controls.Add((MyGuiControlBase) controlMultilineText);
      return guiControlParent;
    }

    private MyGuiControlParent AddTextBox(
      out MyGuiControlTextbox textBox,
      float textScaleMultiplier = 1f)
    {
      textBox = new MyGuiControlTextbox();
      textBox.Size = new Vector2(0.445f, 0.5f);
      textBox.TextScale *= textScaleMultiplier;
      MyGuiControlParent guiControlParent = new MyGuiControlParent();
      guiControlParent.Size = textBox.Size + 0.01f;
      guiControlParent.Controls.Add((MyGuiControlBase) textBox);
      return guiControlParent;
    }

    private MyGuiControlParent AddMultilineTextBox(
      out MyGuiControlMultilineEditableText textBox,
      float textScaleMultiplier = 1f)
    {
      textBox = new MyGuiControlMultilineEditableText();
      textBox.Size = new Vector2(0.445f, 0.13f);
      textBox.TextScale *= textScaleMultiplier;
      textBox.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      textBox.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      textBox.TextWrap = true;
      MyGuiControlParent guiControlParent = new MyGuiControlParent();
      guiControlParent.Size = textBox.Size + 0.01f;
      guiControlParent.Controls.Add((MyGuiControlBase) textBox);
      return guiControlParent;
    }

    private MyGuiControlParent AddSeparatorPanel()
    {
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddHorizontal(new Vector2(-0.278f, 0.0f), 0.557f);
      MyGuiControlParent guiControlParent = new MyGuiControlParent();
      guiControlParent.Size = new Vector2(0.2f, 1f / 1000f);
      guiControlParent.Controls.Add((MyGuiControlBase) controlSeparatorList);
      return guiControlParent;
    }

    private MyGuiControlParent AddImageLinkPanel(
      string imagePath,
      string text,
      string url)
    {
      MyGuiControlImage myGuiControlImage1 = new MyGuiControlImage();
      myGuiControlImage1.Size = new Vector2(0.158f, 0.108f);
      myGuiControlImage1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      myGuiControlImage1.Position = new Vector2(-0.279f, 3f / 1000f);
      myGuiControlImage1.BorderEnabled = true;
      myGuiControlImage1.BorderSize = 1;
      myGuiControlImage1.BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f);
      MyGuiControlImage myGuiControlImage2 = myGuiControlImage1;
      myGuiControlImage2.SetTexture("Textures\\GUI\\Screens\\image_background.dds");
      MyGuiControlImage myGuiControlImage3 = new MyGuiControlImage();
      myGuiControlImage3.Size = new Vector2(0.158f, 0.108f);
      myGuiControlImage3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      myGuiControlImage3.Position = new Vector2(-0.279f, 3f / 1000f);
      myGuiControlImage3.BorderEnabled = true;
      myGuiControlImage3.BorderSize = 1;
      myGuiControlImage3.BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f);
      MyGuiControlImage control = myGuiControlImage3;
      control.SetTexture(imagePath);
      control.SetTooltip(url);
      MyGuiControlMultilineText controlMultilineText = new MyGuiControlMultilineText();
      controlMultilineText.Size = new Vector2(0.4f, 0.1f);
      controlMultilineText.Text = new StringBuilder(text);
      controlMultilineText.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlMultilineText.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlMultilineText.Position = new Vector2(0.12f, -0.005f);
      MyGuiControlButton guiControlButton = this.MakeButton(new Vector2(0.08f, 0.0f), MySpaceTexts.Blank, (Action<MyGuiControlButton>) (x => MyGuiSandbox.OpenUrl(url, UrlOpenMode.SteamOrExternalWithConfirm)));
      guiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
      guiControlButton.TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
      guiControlButton.Text = string.Format(MyTexts.GetString(MyCommonTexts.HelpScreen_HomeSteamOverlay), (object) MyGameService.Service.ServiceName);
      guiControlButton.Alpha = 1f;
      guiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.ClickableText;
      guiControlButton.Size = new Vector2(0.22f, 0.13f);
      guiControlButton.TextScale = 0.736f;
      guiControlButton.CanHaveFocus = true;
      guiControlButton.PositionY += 0.05f;
      guiControlButton.PositionX += 0.175f;
      MyGuiControlImage myGuiControlImage4 = new MyGuiControlImage();
      myGuiControlImage4.Size = new Vector2(0.0128f, 11f / 625f);
      myGuiControlImage4.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      myGuiControlImage4.Position = guiControlButton.Position + new Vector2(0.01f, -0.01f);
      myGuiControlImage4.BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f);
      MyGuiControlImage myGuiControlImage5 = myGuiControlImage4;
      myGuiControlImage5.SetTexture("Textures\\GUI\\link.dds");
      MyGuiControlParent guiControlParent = new MyGuiControlParent();
      guiControlParent.Size = new Vector2(0.5342f, 0.12f);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlImage2);
      guiControlParent.Controls.Add((MyGuiControlBase) control);
      guiControlParent.Controls.Add((MyGuiControlBase) controlMultilineText);
      guiControlParent.Controls.Add((MyGuiControlBase) guiControlButton);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlImage5);
      return guiControlParent;
    }

    private MyGuiControlParent AddLinkPanel(string text, string url)
    {
      MyGuiControlButton guiControlButton = this.MakeButton(new Vector2(0.08f, 0.0f), MySpaceTexts.Blank, (Action<MyGuiControlButton>) (x => MyGuiSandbox.OpenUrl(url, UrlOpenMode.ExternalWithConfirm)));
      guiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
      guiControlButton.TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
      guiControlButton.Text = text;
      guiControlButton.Alpha = 1f;
      guiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.ClickableText;
      guiControlButton.Size = new Vector2(0.22f, 0.13f);
      guiControlButton.TextScale = 0.736f;
      guiControlButton.CanHaveFocus = false;
      guiControlButton.PositionY += 0.01f;
      guiControlButton.PositionX += 0.175f;
      MyGuiControlImage myGuiControlImage1 = new MyGuiControlImage();
      myGuiControlImage1.Size = new Vector2(0.0128f, 11f / 625f);
      myGuiControlImage1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      myGuiControlImage1.Position = guiControlButton.Position + new Vector2(0.01f, -0.01f);
      myGuiControlImage1.BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f);
      MyGuiControlImage myGuiControlImage2 = myGuiControlImage1;
      myGuiControlImage2.SetTexture("Textures\\GUI\\link.dds");
      MyGuiControlParent guiControlParent = new MyGuiControlParent();
      guiControlParent.Size = new Vector2(0.4645f, 0.024f);
      guiControlParent.Controls.Add((MyGuiControlBase) guiControlButton);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlImage2);
      return guiControlParent;
    }

    private MyGuiControlParent AddKeyCategoryPanel(string text)
    {
      MyGuiControlPanel myGuiControlPanel = new MyGuiControlPanel(texture: "Textures\\GUI\\Controls\\item_highlight_dark.dds");
      myGuiControlPanel.Size = new Vector2(0.557f, 0.035f);
      myGuiControlPanel.BorderEnabled = true;
      myGuiControlPanel.BorderSize = 1;
      myGuiControlPanel.BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f);
      MyGuiControlMultilineText controlMultilineText = new MyGuiControlMultilineText();
      controlMultilineText.Size = new Vector2(0.5881f, 0.5f);
      controlMultilineText.Text = new StringBuilder(text);
      controlMultilineText.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      controlMultilineText.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      controlMultilineText.PositionX += 0.02f;
      MyGuiControlParent guiControlParent = new MyGuiControlParent();
      guiControlParent.Size = new Vector2(0.2f, controlMultilineText.TextSize.Y + 0.01f);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlPanel);
      guiControlParent.Controls.Add((MyGuiControlBase) controlMultilineText);
      return guiControlParent;
    }

    private MyGuiControlParent AddKeyPanel(
      string key,
      string description,
      Color? color = null,
      bool isGamepadKey = false)
    {
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.Text = key;
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      myGuiControlLabel1.Font = color.HasValue ? "White" : "Red";
      myGuiControlLabel1.PositionX -= 0.25f;
      if (color.HasValue)
      {
        MyGuiControlLabel myGuiControlLabel2 = myGuiControlLabel1;
        Color color1 = color.Value;
        double num1 = (double) color1.X / 256.0;
        color1 = color.Value;
        double num2 = (double) color1.Y / 256.0;
        color1 = color.Value;
        double num3 = (double) color1.Z / 256.0;
        color1 = color.Value;
        double num4 = (double) color1.A / 256.0;
        Vector4 vector4 = new Vector4((float) num1, (float) num2, (float) num3, (float) num4);
        myGuiControlLabel2.ColorMask = vector4;
      }
      if (isGamepadKey)
        myGuiControlLabel1.TextScale = 1f;
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel();
      myGuiControlLabel3.Text = description;
      myGuiControlLabel3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      myGuiControlLabel3.PositionX += 0.25f;
      MyGuiControlParent guiControlParent = new MyGuiControlParent();
      guiControlParent.Size = new Vector2(0.5f, isGamepadKey ? 0.025f : 0.013f);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel1);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel3);
      return guiControlParent;
    }

    private MyGuiControlParent AddTinySpacePanel()
    {
      MyGuiControlParent guiControlParent = new MyGuiControlParent();
      guiControlParent.Size = new Vector2(0.2f, 0.005f);
      return guiControlParent;
    }

    private MyGuiControlParent AddSignaturePanel()
    {
      MyGuiControlPanel myGuiControlPanel = new MyGuiControlPanel(new Vector2?(new Vector2(-0.08f, -0.04f)), new Vector2?(MyGuiConstants.TEXTURE_KEEN_LOGO.MinSizeGui), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      myGuiControlPanel.BackgroundTexture = MyGuiConstants.TEXTURE_KEEN_LOGO;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel(new Vector2?(new Vector2(0.19f, -0.01f)), text: MyTexts.GetString(MySpaceTexts.WelcomeScreen_SignatureTitle));
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(new Vector2?(new Vector2(0.19f, 0.015f)), text: MyTexts.GetString(MySpaceTexts.WelcomeScreen_Signature));
      myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
      MyGuiControlParent guiControlParent = new MyGuiControlParent();
      guiControlParent.Size = new Vector2(0.2f, 0.1f);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel1);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlPanel);
      return guiControlParent;
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      float num = MyControllerHelper.IsControlAnalog(MyControllerHelper.CX_GUI, MyControlsGUI.SCROLL_DOWN) - MyControllerHelper.IsControlAnalog(MyControllerHelper.CX_GUI, MyControlsGUI.SCROLL_UP);
      this.contentList.GetScrollBar().Value += MyControllerHelper.GAMEPAD_ANALOG_SCROLL_SPEED * num;
      this.m_backButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_screenDescText.Visible = !MyInput.Static.IsJoystickLastUsed;
    }

    protected class MyHackyQuestLogComparer : IComparer<KeyValuePair<string, bool>>
    {
      int IComparer<KeyValuePair<string, bool>>.Compare(
        KeyValuePair<string, bool> x,
        KeyValuePair<string, bool> y)
      {
        return string.Compare(x.Key, y.Key);
      }
    }

    private enum HelpPageEnum
    {
      Tutorials,
      BasicControls,
      AdvancedControls,
      Controller,
      ControllerAdvanced,
      Chat,
      Support,
      IngameHelp,
      Welcome,
      ReportIssue,
    }
  }
}
