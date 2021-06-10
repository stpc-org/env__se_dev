// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenServerSearchBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Game;
using VRage.GameServices;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenServerSearchBase : MyGuiScreenBase
  {
    private static List<MyWorkshopItem> m_subscribedMods;
    private static List<MyWorkshopItem> m_settingsMods;
    private static bool m_needsModRefresh = true;
    private MyGuiControlRotatingWheel m_loadingWheel;
    protected Vector2 m_currentPosition;
    protected MyGuiScreenJoinGame m_joinScreen;
    protected float m_padding = 0.02f;
    protected MyGuiControlScrollablePanel m_panel;
    protected MyGuiControlParent m_parent;
    protected MyGuiControlCheckbox m_advancedCheckbox;
    protected MyGuiControlButton m_searchButton;
    protected MyGuiControlButton m_settingsButton;
    protected MyGuiControlButton m_advancedButton;
    protected MyGuiControlButton m_modsButton;
    private MyGuiControlButton m_btnDefault;
    private MyGuiScreenServerSearchBase.SearchPageEnum m_currentPage;

    protected MyGuiScreenServerSearchBase.SearchPageEnum CurrentPage
    {
      get => this.m_currentPage;
      set
      {
        if (this.m_currentPage == value)
          return;
        this.m_currentPage = value;
        if (this.m_currentPage == MyGuiScreenServerSearchBase.SearchPageEnum.Mods)
          this.PrepareModPage();
        else
          this.RecreateControls(false);
      }
    }

    protected bool EnableAdvanced => this.FilterOptions.AdvancedFilter && this.m_joinScreen.EnableAdvancedSearch;

    protected Vector2 WindowSize
    {
      get
      {
        Vector2? size = this.Size;
        double num1 = (double) size.Value.X - 0.100000001490116;
        size = this.Size;
        double num2 = (double) size.Value.Y - (double) this.m_settingsButton.Size.Y * 2.0 - (double) this.m_padding * 16.0;
        return new Vector2((float) num1, (float) num2);
      }
    }

    protected MyServerFilterOptions FilterOptions
    {
      get => this.m_joinScreen.FilterOptions;
      set => this.m_joinScreen.FilterOptions = value;
    }

    public MyGuiScreenServerSearchBase(MyGuiScreenJoinGame joinScreen)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.6535714f, 0.9398855f)), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      this.m_joinScreen = joinScreen;
      this.CreateScreen();
    }

    private void CreateScreen()
    {
      this.CanHideOthers = true;
      this.CanBeHidden = true;
      this.EnabledBackgroundFade = true;
      this.CloseButtonEnabled = true;
      this.RecreateControls(true);
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.AddCaption(MyCommonTexts.ServerSearch, captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      MyGuiControlSeparatorList controlSeparatorList1 = new MyGuiControlSeparatorList();
      controlSeparatorList1.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.834999978542328 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.835f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList1);
      MyGuiControlSeparatorList controlSeparatorList2 = new MyGuiControlSeparatorList();
      controlSeparatorList2.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.834999978542328 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.123000003397465)), this.m_size.Value.X * 0.835f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList2);
      MyGuiControlSeparatorList controlSeparatorList3 = new MyGuiControlSeparatorList();
      controlSeparatorList3.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.834999978542328 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.150000005960464)), this.m_size.Value.X * 0.835f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList3);
      this.m_currentPosition = new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.834999978542328 / 2.0 - 3.0 / 1000.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0949999988079071));
      float y = this.m_currentPosition.Y;
      this.m_settingsButton = this.AddButton(MyCommonTexts.ServerDetails_Settings, new Action<MyGuiControlButton>(this.SettingsButtonClick), addToParent: false);
      this.m_settingsButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerSearch_Settings));
      this.m_currentPosition.Y = y;
      this.m_currentPosition.X += this.m_settingsButton.Size.X + this.m_padding / 3.6f;
      this.m_advancedButton = this.AddButton(MyCommonTexts.Advanced, new Action<MyGuiControlButton>(this.AdvancedButtonClick), addToParent: false);
      this.m_advancedButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerSearch_Advanced));
      this.m_currentPosition.Y = y;
      this.m_currentPosition.X += this.m_settingsButton.Size.X + this.m_padding / 3.6f;
      Vector2 vector2;
      if (MyPlatformGameSettings.IsModdingAllowed && MyFakes.ENABLE_WORKSHOP_MODS)
      {
        this.m_modsButton = this.AddButton(MyCommonTexts.WorldSettings_Mods, new Action<MyGuiControlButton>(this.ModsButtonClick), addToParent: false);
        this.m_modsButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerSearch_Mods));
        this.m_currentPosition.Y = y;
        this.m_currentPosition.X += this.m_settingsButton.Size.X + this.m_padding;
        vector2 = this.m_modsButton.Position;
      }
      else
        vector2 = this.m_currentPosition;
      this.m_loadingWheel = new MyGuiControlRotatingWheel(new Vector2?(vector2 + new Vector2(0.137f, -0.004f)), new Vector4?(MyGuiConstants.ROTATING_WHEEL_COLOR), 0.2f);
      this.Controls.Add((MyGuiControlBase) this.m_loadingWheel);
      this.m_loadingWheel.Visible = false;
      this.m_btnDefault = new MyGuiControlButton(new Vector2?(new Vector2(0.0f, 0.0f) - new Vector2(-3f / 1000f, (float) (-(double) this.m_size.Value.Y / 2.0 + 0.0710000023245811))), text: MyTexts.Get(MyCommonTexts.ServerSearch_Defaults));
      this.m_btnDefault.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerSearch_Defaults));
      this.m_btnDefault.ButtonClicked += new Action<MyGuiControlButton>(this.DefaultSettingsClick);
      this.m_btnDefault.ButtonClicked += new Action<MyGuiControlButton>(this.DefaultModsClick);
      this.m_btnDefault.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.Controls.Add((MyGuiControlBase) this.m_btnDefault);
      this.m_searchButton = new MyGuiControlButton(new Vector2?(new Vector2(0.0f, 0.0f) - new Vector2(0.18f, (float) (-(double) this.m_size.Value.Y / 2.0 + 0.0710000023245811))), text: MyTexts.Get(MyCommonTexts.ScreenMods_SearchLabel));
      this.m_searchButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerSearch_Search));
      this.m_searchButton.ButtonClicked += new Action<MyGuiControlButton>(this.SearchClick);
      this.m_searchButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.Controls.Add((MyGuiControlBase) this.m_searchButton);
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(new Vector2(this.m_searchButton.Position.X - MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui.X / 2f, this.m_searchButton.Position.Y)));
      myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.ServerSearch_Help_Screen);
      this.m_currentPosition = -this.WindowSize / 2f;
      switch (this.CurrentPage)
      {
        case MyGuiScreenServerSearchBase.SearchPageEnum.Settings:
          this.FocusedControl = (MyGuiControlBase) this.m_settingsButton;
          this.m_settingsButton.Checked = true;
          this.m_settingsButton.Selected = true;
          this.m_currentPosition.Y += this.m_padding * 2f;
          this.DrawSettingsSelector();
          this.DrawTopControls();
          this.DrawMidControls();
          break;
        case MyGuiScreenServerSearchBase.SearchPageEnum.Advanced:
          this.FocusedControl = (MyGuiControlBase) this.m_advancedButton;
          this.m_advancedButton.Checked = true;
          this.m_advancedButton.Selected = true;
          this.DrawAdvancedSelector();
          this.DrawBottomControls();
          break;
        case MyGuiScreenServerSearchBase.SearchPageEnum.Mods:
          this.FocusedControl = (MyGuiControlBase) this.m_modsButton;
          this.m_modsButton.Checked = true;
          this.m_modsButton.Selected = true;
          this.DrawModSelector();
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private void DefaultSettingsClick(MyGuiControlButton myGuiControlButton)
    {
      this.FilterOptions.SetDefaults();
      this.RecreateControls(false);
    }

    private void DefaultModsClick(MyGuiControlButton myGuiControlButton)
    {
      this.FilterOptions.Mods.Clear();
      this.RecreateControls(false);
    }

    private void CancelButtonClick(MyGuiControlButton myGuiControlButton) => this.CloseScreen();

    private void PrepareModPage()
    {
      if (MyGuiScreenServerSearchBase.m_needsModRefresh)
      {
        MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, new MyStringId?(), new Func<IMyAsyncResult>(this.LoadModsBeginAction), new Action<IMyAsyncResult, MyGuiScreenProgressAsync>(this.LoadModsEndAction)));
        MyGuiScreenServerSearchBase.m_needsModRefresh = false;
      }
      else
      {
        this.RecreateControls(false);
        this.m_loadingWheel.Visible = false;
      }
    }

    private void ModsButtonClick(MyGuiControlButton myGuiControlButton)
    {
      this.CurrentPage = MyGuiScreenServerSearchBase.SearchPageEnum.Mods;
      this.FocusedControl = (MyGuiControlBase) this.m_modsButton;
    }

    private void SettingsButtonClick(MyGuiControlButton myGuiControlButton)
    {
      this.CurrentPage = MyGuiScreenServerSearchBase.SearchPageEnum.Settings;
      this.FocusedControl = (MyGuiControlBase) this.m_settingsButton;
    }

    private void AdvancedButtonClick(MyGuiControlButton myGuiControlButton)
    {
      this.CurrentPage = MyGuiScreenServerSearchBase.SearchPageEnum.Advanced;
      this.FocusedControl = (MyGuiControlBase) this.m_advancedButton;
    }

    private void DrawModSelector()
    {
      this.m_parent = new MyGuiControlParent();
      this.m_panel = new MyGuiControlScrollablePanel((MyGuiControlBase) this.m_parent);
      this.m_panel.ScrollbarVEnabled = true;
      this.m_panel.PositionX += 0.0075f;
      this.m_panel.PositionY += (float) ((double) this.m_settingsButton.Size.Y / 2.0 + (double) this.m_padding * 1.70000004768372);
      MyGuiControlScrollablePanel panel = this.m_panel;
      Vector2? size = this.Size;
      double num1 = (double) size.Value.X - 0.100000001490116;
      size = this.Size;
      double num2 = (double) size.Value.Y - (double) this.m_settingsButton.Size.Y * 2.0 - (double) this.m_padding * 13.6999998092651;
      Vector2 vector2 = new Vector2((float) num1, (float) num2);
      panel.Size = vector2;
      this.Controls.Add((MyGuiControlBase) this.m_panel);
      this.m_advancedCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(-0.0435f, -0.279f)), toolTip: MyTexts.GetString(MyCommonTexts.ServerSearch_EnableAdvancedTooltip));
      this.m_advancedCheckbox.IsChecked = this.FilterOptions.AdvancedFilter;
      this.m_advancedCheckbox.IsCheckedChanged += (Action<MyGuiControlCheckbox>) (c =>
      {
        this.FilterOptions.AdvancedFilter = c.IsChecked;
        this.RecreateControls(false);
      });
      this.m_advancedCheckbox.Enabled = this.m_joinScreen.EnableAdvancedSearch;
      this.Controls.Add((MyGuiControlBase) this.m_advancedCheckbox);
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel(new Vector2?(this.m_advancedCheckbox.Position - new Vector2((float) ((double) this.m_advancedCheckbox.Size.X / 2.0 + (double) this.m_padding * 10.4499998092651), 0.0f)), text: MyTexts.GetString(MyCommonTexts.ServerSearch_EnableAdvanced));
      myGuiControlLabel1.SetToolTip(MyCommonTexts.ServerSearch_EnableAdvancedTooltip);
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      myGuiControlLabel1.Enabled = this.m_joinScreen.EnableAdvancedSearch;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel1);
      MyGuiControlCheckbox guiControlCheckbox1 = new MyGuiControlCheckbox(new Vector2?(new Vector2(0.2465f, -0.279f)));
      guiControlCheckbox1.IsChecked = this.FilterOptions.ModsExclusive;
      guiControlCheckbox1.SetToolTip(MyCommonTexts.ServerSearch_ExclusiveTooltip);
      guiControlCheckbox1.IsCheckedChanged = (Action<MyGuiControlCheckbox>) (c => this.FilterOptions.ModsExclusive = c.IsChecked);
      guiControlCheckbox1.Enabled = true;
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(new Vector2?(guiControlCheckbox1.Position - new Vector2((float) ((double) guiControlCheckbox1.Size.X / 2.0 + (double) this.m_padding * 10.4499998092651), 0.0f)), text: MyTexts.GetString(MyCommonTexts.ServerSearch_Exclusive));
      myGuiControlLabel2.SetToolTip(MyCommonTexts.ServerSearch_ExclusiveTooltip);
      myGuiControlLabel2.Enabled = true;
      myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      this.Controls.Add((MyGuiControlBase) guiControlCheckbox1);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.834999978542328 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.230000004172325)), this.m_size.Value.X * 0.835f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList);
      MyGuiControlButton guiControlButton = new MyGuiControlButton(visualStyle: MyGuiControlButtonStyleEnum.Small, text: MyTexts.Get(MyCommonTexts.ServerSearch_Clear));
      MyGuiControlCheckbox guiControlCheckbox2 = new MyGuiControlCheckbox(new Vector2?(this.m_currentPosition));
      float y = (float) ((double) guiControlCheckbox2.Size.Y * (double) (MyGuiScreenServerSearchBase.m_subscribedMods.Count + MyGuiScreenServerSearchBase.m_settingsMods.Count) + (double) guiControlButton.Size.Y / 2.0) + this.m_padding;
      this.m_currentPosition = -this.m_panel.Size / 2f;
      this.m_currentPosition.Y = (float) (-(double) y / 2.0 + (double) guiControlCheckbox2.Size.Y / 2.0 - 0.00499999988824129);
      this.m_currentPosition.X -= 0.0225f;
      this.m_parent.Size = new Vector2(this.m_panel.Size.X, y);
      MyGuiScreenServerSearchBase.m_subscribedMods.Sort((Comparison<MyWorkshopItem>) ((a, b) => a.Title.CompareTo(b.Title)));
      MyGuiScreenServerSearchBase.m_settingsMods.Sort((Comparison<MyWorkshopItem>) ((a, b) => a.Title.CompareTo(b.Title)));
      guiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      guiControlButton.ButtonClicked += new Action<MyGuiControlButton>(this.DefaultModsClick);
      guiControlButton.Position = this.m_currentPosition + new Vector2(this.m_padding, (float) (-(double) this.m_padding * 6.0));
      this.m_parent.Controls.Add((MyGuiControlBase) guiControlButton);
      foreach (MyWorkshopItem subscribedMod in MyGuiScreenServerSearchBase.m_subscribedMods)
      {
        MyWorkshopItem mod = subscribedMod;
        int num3 = Math.Min(mod.Description.Length, 128);
        int num4 = mod.Description.IndexOf("\n");
        if (num4 > 0)
          num3 = Math.Min(num3, num4 - 1);
        MyGuiControlCheckbox guiControlCheckbox3 = this.AddCheckbox(mod.Title, (Action<MyGuiControlCheckbox>) (c => this.ModCheckboxClick(c, mod.Id, mod.ServiceName)), mod.Description.Substring(0, num3), isAutoEllipsisEnabled: true, isAutoScaleEnabled: true, maxWidth: 0.5f);
        guiControlCheckbox3.IsChecked = this.FilterOptions.Mods.Contains(new WorkshopId(mod.Id, mod.ServiceName));
        guiControlCheckbox3.Enabled = this.FilterOptions.AdvancedFilter;
      }
      foreach (MyWorkshopItem settingsMod in MyGuiScreenServerSearchBase.m_settingsMods)
      {
        MyWorkshopItem mod = settingsMod;
        int num3 = Math.Min(mod.Description.Length, 128);
        int num4 = mod.Description.IndexOf("\n");
        if (num4 > 0)
          num3 = Math.Min(num3, num4 - 1);
        MyGuiControlCheckbox guiControlCheckbox3 = this.AddCheckbox(mod.Title, (Action<MyGuiControlCheckbox>) (c => this.ModCheckboxClick(c, mod.Id, mod.ServiceName)), mod.Description.Substring(0, num3), "DarkBlue", this.EnableAdvanced, true, true, 0.5f);
        guiControlCheckbox3.IsChecked = this.FilterOptions.Mods.Contains(new WorkshopId(mod.Id, mod.ServiceName));
        guiControlCheckbox3.Enabled = this.FilterOptions.AdvancedFilter;
      }
    }

    private void DrawAdvancedSelector()
    {
      this.m_advancedCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(-0.0435f, -0.279f)), toolTip: MyTexts.GetString(MyCommonTexts.ServerSearch_EnableAdvancedTooltip));
      this.m_advancedCheckbox.IsChecked = this.FilterOptions.AdvancedFilter;
      this.m_advancedCheckbox.IsCheckedChanged += (Action<MyGuiControlCheckbox>) (c =>
      {
        this.FilterOptions.AdvancedFilter = c.IsChecked;
        this.RecreateControls(false);
      });
      this.m_advancedCheckbox.Enabled = this.m_joinScreen.EnableAdvancedSearch;
      this.Controls.Add((MyGuiControlBase) this.m_advancedCheckbox);
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(this.m_advancedCheckbox.Position - new Vector2((float) ((double) this.m_advancedCheckbox.Size.X / 2.0 + (double) this.m_padding * 10.4499998092651), 0.0f)), text: MyTexts.GetString(MyCommonTexts.ServerSearch_EnableAdvanced));
      myGuiControlLabel.SetToolTip(MyCommonTexts.ServerSearch_EnableAdvancedTooltip);
      myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      myGuiControlLabel.Enabled = this.m_joinScreen.EnableAdvancedSearch;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.834999978542328 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.230000004172325)), this.m_size.Value.X * 0.835f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList);
      this.m_currentPosition.Y += 0.07f;
    }

    private void DrawSettingsSelector()
    {
      this.m_currentPosition.Y = -0.279f;
      this.AddCheckboxDuo(new MyStringId?[2]
      {
        new MyStringId?(MyCommonTexts.WorldSettings_GameModeCreative),
        new MyStringId?(MyCommonTexts.WorldSettings_GameModeSurvival)
      }, new Action<MyGuiControlCheckbox>[2]
      {
        (Action<MyGuiControlCheckbox>) (c => this.FilterOptions.CreativeMode = c.IsChecked),
        (Action<MyGuiControlCheckbox>) (c => this.FilterOptions.SurvivalMode = c.IsChecked)
      }, new MyStringId?[2]
      {
        new MyStringId?(MySpaceTexts.ToolTipJoinGameServerSearch_Creative),
        new MyStringId?(MySpaceTexts.ToolTipJoinGameServerSearch_Survival)
      }, new bool[2]
      {
        this.FilterOptions.CreativeMode,
        this.FilterOptions.SurvivalMode
      });
      this.AddCheckboxDuo(new MyStringId?[2]
      {
        new MyStringId?(MyCommonTexts.MultiplayerCompatibleVersions),
        new MyStringId?(MyCommonTexts.MultiplayerJoinSameGameData)
      }, new Action<MyGuiControlCheckbox>[2]
      {
        (Action<MyGuiControlCheckbox>) (c => this.FilterOptions.SameVersion = c.IsChecked),
        (Action<MyGuiControlCheckbox>) (c => this.FilterOptions.SameData = c.IsChecked)
      }, new MyStringId?[2]
      {
        new MyStringId?(MySpaceTexts.ToolTipJoinGameServerSearch_CompatibleVersions),
        new MyStringId?(MySpaceTexts.ToolTipJoinGameServerSearch_SameGameData)
      }, new bool[2]
      {
        this.FilterOptions.SameVersion,
        this.FilterOptions.SameData
      });
      Vector2 currentPosition = this.m_currentPosition;
      this.AddCheckboxDuo(new string[2]
      {
        MyTexts.GetString(MyCommonTexts.MultiplayerJoinAllowedGroups),
        null
      }, new Action<MyGuiControlCheckbox>[1]
      {
        (Action<MyGuiControlCheckbox>) (c => this.FilterOptions.AllowedGroups = c.IsChecked)
      }, new string[1]
      {
        string.Format(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerSearch_AllowedGroups), (object) MyGameService.Service.ServiceName)
      }, new bool[1]{ this.FilterOptions.AllowedGroups }, this.m_joinScreen.SupportsGroups);
      this.m_currentPosition = currentPosition;
      MyStringId?[] text = new MyStringId?[2];
      text[1] = new MyStringId?(MyCommonTexts.MultiplayerJoinHasPassword);
      Action<MyGuiControlIndeterminateCheckbox>[] onClick = new Action<MyGuiControlIndeterminateCheckbox>[2]
      {
        null,
        (Action<MyGuiControlIndeterminateCheckbox>) (c =>
        {
          switch (c.State)
          {
            case CheckStateEnum.Checked:
              this.FilterOptions.HasPassword = new bool?(true);
              break;
            case CheckStateEnum.Unchecked:
              this.FilterOptions.HasPassword = new bool?(false);
              break;
            case CheckStateEnum.Indeterminate:
              this.FilterOptions.HasPassword = new bool?();
              break;
          }
        })
      };
      MyStringId?[] tooltip = new MyStringId?[2];
      tooltip[1] = new MyStringId?(MySpaceTexts.ToolTipJoinGameServerSearch_HasPassword);
      CheckStateEnum[] values = new CheckStateEnum[2]
      {
        CheckStateEnum.Indeterminate,
        !this.FilterOptions.HasPassword.HasValue || !this.FilterOptions.HasPassword.Value ? (this.FilterOptions.HasPassword.HasValue ? CheckStateEnum.Unchecked : CheckStateEnum.Indeterminate) : CheckStateEnum.Checked
      };
      this.AddIndeterminateDuo(text, onClick, tooltip, values);
      MyGuiControlSeparatorList controlSeparatorList1 = new MyGuiControlSeparatorList();
      controlSeparatorList1.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.834999978542328 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.324999988079071)), this.m_size.Value.X * 0.835f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList1);
      MyGuiControlSeparatorList controlSeparatorList2 = new MyGuiControlSeparatorList();
      controlSeparatorList2.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.834999978542328 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.409000009298325)), this.m_size.Value.X * 0.835f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList2);
      MyGuiControlSeparatorList controlSeparatorList3 = new MyGuiControlSeparatorList();
      controlSeparatorList3.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.834999978542328 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.657000005245209)), this.m_size.Value.X * 0.835f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList3);
    }

    private void ModCheckboxClick(MyGuiControlCheckbox c, ulong modId, string serviceName)
    {
      if (c.IsChecked)
        this.FilterOptions.Mods.Add(new WorkshopId(modId, serviceName));
      else
        this.FilterOptions.Mods.Remove(new WorkshopId(modId, serviceName));
    }

    protected virtual void DrawTopControls()
    {
      this.m_currentPosition.Y = -0.0225f;
      this.AddNumericRangeOption(MyCommonTexts.MultiplayerJoinOnlinePlayers, (Action<SerializableRange>) (r => this.FilterOptions.PlayerCount = r), this.FilterOptions.PlayerCount, this.FilterOptions.CheckPlayer, (Action<MyGuiControlCheckbox>) (c => this.FilterOptions.CheckPlayer = c.IsChecked));
      this.AddNumericRangeOption(MyCommonTexts.JoinGame_ColumnTitle_Mods, (Action<SerializableRange>) (r => this.FilterOptions.ModCount = r), this.FilterOptions.ModCount, this.FilterOptions.CheckMod, (Action<MyGuiControlCheckbox>) (c => this.FilterOptions.CheckMod = c.IsChecked), MyPlatformGameSettings.IsModdingAllowed);
      this.AddNumericRangeOption(MySpaceTexts.WorldSettings_ViewDistance, (Action<SerializableRange>) (r => this.FilterOptions.ViewDistance = r), this.FilterOptions.ViewDistance, this.FilterOptions.CheckDistance, (Action<MyGuiControlCheckbox>) (c => this.FilterOptions.CheckDistance = c.IsChecked));
    }

    protected virtual void DrawMidControls()
    {
      Vector2 currentPosition = this.m_currentPosition;
      this.m_currentPosition.Y += this.m_padding * 1.32f;
      this.m_currentPosition.X += this.m_padding / 2.4f;
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(this.m_currentPosition), text: MyTexts.GetString(MyCommonTexts.JoinGame_ColumnTitle_Ping));
      myGuiControlLabel.Enabled = this.m_joinScreen.EnableAdvancedSearch;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      this.m_currentPosition.X += this.m_padding * 2.3f;
      MyGuiControlSlider guiControlSlider = new MyGuiControlSlider(new Vector2?(this.m_currentPosition + new Vector2(0.215f, 0.0f)), -1f, 1000f, defaultValue: new float?((float) this.FilterOptions.Ping), labelText: string.Empty, labelScale: 0.0f);
      guiControlSlider.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      guiControlSlider.LabelDecimalPlaces = 0;
      guiControlSlider.IntValue = true;
      guiControlSlider.Size = new Vector2(0.45f - myGuiControlLabel.Size.X, 1f);
      guiControlSlider.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerSearch_Ping));
      guiControlSlider.PositionX += guiControlSlider.Size.X / 2f;
      guiControlSlider.Enabled = this.m_joinScreen.SupportsPing;
      this.Controls.Add((MyGuiControlBase) guiControlSlider);
      this.m_currentPosition.X += (float) ((double) guiControlSlider.Size.X / 2.0 + (double) this.m_padding * 14.0);
      MyGuiControlLabel val = new MyGuiControlLabel(new Vector2?(this.m_currentPosition), text: ("<" + (object) guiControlSlider.Value + "ms"), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
      guiControlSlider.ValueChanged += (Action<MyGuiControlSlider>) (x =>
      {
        val.Text = "<" + (object) x.Value + "ms";
        this.FilterOptions.Ping = (int) x.Value;
      });
      val.Enabled = this.m_joinScreen.EnableAdvancedSearch;
      this.Controls.Add((MyGuiControlBase) val);
      this.m_currentPosition = currentPosition;
      this.m_currentPosition.Y += 0.04f;
    }

    protected virtual void DrawBottomControls()
    {
    }

    public override bool Draw()
    {
      base.Draw();
      MyGuiDrawAlignEnum drawAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      Vector2 positionAbsoluteTopLeft = this.m_settingsButton.GetPositionAbsoluteTopLeft();
      Vector2 size = this.m_settingsButton.Size;
      Vector2 normalizedCoord1 = positionAbsoluteTopLeft;
      normalizedCoord1.Y += size.Y / 2f;
      normalizedCoord1.X -= size.X / 6f;
      Vector2 normalizedCoord2 = positionAbsoluteTopLeft;
      normalizedCoord2.Y = normalizedCoord1.Y;
      int num = this.m_modsButton == null ? 2 : 3;
      Color color = MyGuiControlBase.ApplyColorMaskModifiers(MyGuiConstants.LABEL_TEXT_COLOR, true, this.m_transitionAlpha);
      normalizedCoord2.X += (float) ((double) num * (double) size.X + (double) size.X / 6.0);
      MyGuiManager.DrawString("Blue", MyTexts.GetString(MyCommonTexts.Gamepad_Help_TabControl_Left), normalizedCoord1, 1f, new Color?(color), drawAlign);
      MyGuiManager.DrawString("Blue", MyTexts.GetString(MyCommonTexts.Gamepad_Help_TabControl_Right), normalizedCoord2, 1f, new Color?(color), drawAlign);
      return true;
    }

    private void SearchClick(MyGuiControlButton myGuiControlButton) => this.CloseScreen();

    public override string GetFriendlyName() => nameof (MyGuiScreenServerSearchBase);

    private IMyAsyncResult LoadModsBeginAction() => (IMyAsyncResult) new MyModsLoadListResult(this.FilterOptions.Mods);

    private void LoadModsEndAction(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
    {
      MyModsLoadListResult modsLoadListResult = (MyModsLoadListResult) result;
      MyGuiScreenServerSearchBase.m_subscribedMods = modsLoadListResult.SubscribedMods;
      MyGuiScreenServerSearchBase.m_settingsMods = modsLoadListResult.SetMods;
      screen.CloseScreen();
      this.m_loadingWheel.Visible = false;
      this.RecreateControls(false);
    }

    protected MyGuiControlButton AddButton(
      MyStringId text,
      Action<MyGuiControlButton> onClick,
      MyStringId? tooltip = null,
      bool enabled = true,
      bool addToParent = true)
    {
      Vector2? position = new Vector2?(this.m_currentPosition);
      Vector2? size = new Vector2?();
      StringBuilder stringBuilder = MyTexts.Get(text);
      string str = tooltip.HasValue ? MyTexts.GetString(tooltip.Value) : string.Empty;
      Action<MyGuiControlButton> action = onClick;
      Vector4? colorMask = new Vector4?(Color.Yellow.ToVector4());
      string toolTip = str;
      StringBuilder text1 = stringBuilder;
      Action<MyGuiControlButton> onButtonClick = action;
      int? buttonIndex = new int?();
      MyGuiControlButton guiControlButton = new MyGuiControlButton(position, MyGuiControlButtonStyleEnum.ToolbarButton, size, colorMask, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP, toolTip, text1, onButtonClick: onButtonClick, buttonIndex: buttonIndex);
      guiControlButton.Enabled = enabled;
      guiControlButton.PositionX += guiControlButton.Size.X / 2f;
      if (addToParent)
        this.Controls.Add((MyGuiControlBase) guiControlButton);
      else
        this.Controls.Add((MyGuiControlBase) guiControlButton);
      this.m_currentPosition.Y += guiControlButton.Size.Y + this.m_padding;
      return guiControlButton;
    }

    protected void AddNumericRangeOption(
      MyStringId text,
      Action<SerializableRange> onEntry,
      SerializableRange currentRange,
      bool active,
      Action<MyGuiControlCheckbox> onEnable,
      bool enabled = true)
    {
      float y = 0.004f;
      float x = this.m_currentPosition.X;
      this.m_currentPosition.X = (float) (-(double) this.WindowSize.X / 2.0 + (double) this.m_padding * 12.6000003814697);
      MyGuiControlCheckbox guiControlCheckbox = new MyGuiControlCheckbox(new Vector2?(this.m_currentPosition + new Vector2(0.0f, y)), toolTip: MyTexts.GetString(MyCommonTexts.ServerSearch_EnableNumericTooltip));
      guiControlCheckbox.PositionX += guiControlCheckbox.Size.X / 2f;
      guiControlCheckbox.IsChecked = active & enabled;
      guiControlCheckbox.Enabled = enabled;
      guiControlCheckbox.IsCheckedChanged += onEnable;
      this.Controls.Add((MyGuiControlBase) guiControlCheckbox);
      this.m_currentPosition.X += guiControlCheckbox.Size.X / 2f + this.m_padding;
      MyGuiControlTextbox minText = new MyGuiControlTextbox(new Vector2?(this.m_currentPosition), currentRange.Min.ToString(), 6, type: MyGuiControlTextboxType.DigitsOnly);
      minText.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      minText.Size = new Vector2(0.12f, minText.Size.Y);
      minText.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerSearch_MinimumFilterValue));
      minText.Enabled = guiControlCheckbox.IsChecked;
      this.Controls.Add((MyGuiControlBase) minText);
      this.m_currentPosition.X += (float) ((double) minText.Size.X / 1.5 + (double) this.m_padding + 0.0280000008642673);
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel(new Vector2?(this.m_currentPosition), text: "-");
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel1);
      this.m_currentPosition.X += (float) ((double) myGuiControlLabel1.Size.X / 2.0 + (double) this.m_padding / 2.0);
      MyGuiControlTextbox maxText = new MyGuiControlTextbox(new Vector2?(this.m_currentPosition), float.IsInfinity(currentRange.Max) ? "-1" : currentRange.Max.ToString(), 6, type: MyGuiControlTextboxType.DigitsOnly);
      maxText.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      maxText.Size = new Vector2(0.12f, maxText.Size.Y);
      maxText.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerSearch_MaximumFilterValue));
      maxText.Enabled = guiControlCheckbox.IsChecked;
      this.Controls.Add((MyGuiControlBase) maxText);
      this.m_currentPosition.X += (float) ((double) maxText.Size.X / 1.5 + (double) this.m_padding + 0.00999999977648258);
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(new Vector2?(new Vector2(-0.27f, this.m_currentPosition.Y)), text: MyTexts.GetString(text));
      myGuiControlLabel2.Enabled = true;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      this.m_currentPosition.X = x;
      this.m_currentPosition.Y += myGuiControlLabel2.Size.Y + this.m_padding + y;
      guiControlCheckbox.IsCheckedChanged += (Action<MyGuiControlCheckbox>) (c => minText.Enabled = maxText.Enabled = c.IsChecked);
      if (onEntry == null)
        return;
      maxText.TextChanged += (Action<MyGuiControlTextbox>) (t =>
      {
        float result1;
        float result2;
        if (!float.TryParse(minText.Text, out result1) || !float.TryParse(maxText.Text, out result2))
          return;
        if ((double) result2 == -1.0)
          result2 = float.PositiveInfinity;
        if ((double) result1 < 0.0)
          result1 = 0.0f;
        onEntry(new SerializableRange(result1, result2));
      });
      minText.TextChanged += (Action<MyGuiControlTextbox>) (t =>
      {
        float result1;
        float result2;
        if (!float.TryParse(minText.Text, out result1) || !float.TryParse(maxText.Text, out result2))
          return;
        if ((double) result2 == -1.0)
          result2 = float.PositiveInfinity;
        if ((double) result1 < 0.0)
          result1 = 0.0f;
        onEntry(new SerializableRange(result1, result2));
      });
    }

    protected MyGuiControlCheckbox AddCheckbox(
      MyStringId text,
      Action<MyGuiControlCheckbox> onClick,
      MyStringId? tooltip = null,
      string font = null,
      bool enabled = true)
    {
      return this.AddCheckbox(MyTexts.GetString(text), onClick, tooltip.HasValue ? MyTexts.GetString(tooltip.Value) : (string) null, font, enabled);
    }

    protected MyGuiControlCheckbox AddCheckbox(
      string text,
      Action<MyGuiControlCheckbox> onClick,
      string tooltip = null,
      string font = null,
      bool enabled = true,
      bool isAutoEllipsisEnabled = false,
      bool isAutoScaleEnabled = false,
      float maxWidth = float.PositiveInfinity)
    {
      MyGuiControlCheckbox guiControlCheckbox = new MyGuiControlCheckbox(new Vector2?(this.m_currentPosition), toolTip: (tooltip ?? string.Empty));
      guiControlCheckbox.PositionX += (float) ((double) guiControlCheckbox.Size.X / 2.0 + (double) this.m_padding * 26.0);
      this.m_parent.Controls.Add((MyGuiControlBase) guiControlCheckbox);
      if (onClick != null)
        guiControlCheckbox.IsCheckedChanged += onClick;
      Vector2? position = new Vector2?(this.m_currentPosition);
      Vector2? size = new Vector2?();
      string text1 = MyTexts.GetString(text);
      Vector4? colorMask = new Vector4?();
      int num1 = isAutoEllipsisEnabled ? 1 : 0;
      bool flag = isAutoScaleEnabled;
      double num2 = (double) maxWidth;
      int num3 = flag ? 1 : 0;
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(position, size, text1, colorMask, isAutoEllipsisEnabled: (num1 != 0), maxWidth: ((float) num2), isAutoScaleEnabled: (num3 != 0));
      myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      myGuiControlLabel.PositionX = guiControlCheckbox.PositionX - this.m_padding * 25.8f;
      if (!string.IsNullOrEmpty(tooltip))
        myGuiControlLabel.SetToolTip(tooltip);
      if (!string.IsNullOrEmpty(font))
        myGuiControlLabel.Font = font;
      this.m_parent.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      this.m_currentPosition.Y += guiControlCheckbox.Size.Y;
      guiControlCheckbox.Enabled = enabled;
      myGuiControlLabel.Enabled = enabled;
      return guiControlCheckbox;
    }

    protected MyGuiControlCheckbox[] AddCheckboxDuo(
      MyStringId?[] text,
      Action<MyGuiControlCheckbox>[] onClick,
      MyStringId?[] tooltip,
      bool[] values)
    {
      string[] text1 = new string[text.Length];
      string[] tooltip1 = new string[tooltip.Length];
      for (int index = 0; index < text.Length; ++index)
        text1[index] = text[index].HasValue ? MyTexts.GetString(text[index].Value) : string.Empty;
      for (int index = 0; index < tooltip.Length; ++index)
        tooltip1[index] = tooltip[index].HasValue ? MyTexts.GetString(tooltip[index].Value) : string.Empty;
      return this.AddCheckboxDuo(text1, onClick, tooltip1, values);
    }

    protected MyGuiControlCheckbox[] AddCheckboxDuo(
      string[] text,
      Action<MyGuiControlCheckbox>[] onClick,
      string[] tooltip,
      bool[] values,
      bool enabled = true)
    {
      MyGuiControlCheckbox[] guiControlCheckboxArray = new MyGuiControlCheckbox[2];
      float x = this.m_currentPosition.X;
      if (!string.IsNullOrEmpty(text[0]))
      {
        MyGuiControlCheckbox guiControlCheckbox = new MyGuiControlCheckbox(new Vector2?(this.m_currentPosition), toolTip: (!string.IsNullOrEmpty(tooltip[0]) ? MyTexts.GetString(tooltip[0]) : string.Empty));
        guiControlCheckbox.PositionX = -0.0435f;
        guiControlCheckbox.IsChecked = values[0];
        guiControlCheckbox.Enabled = enabled;
        guiControlCheckboxArray[0] = guiControlCheckbox;
        if (onClick[0] != null)
          guiControlCheckbox.IsCheckedChanged += onClick[0];
        this.m_currentPosition.X = (float) ((double) guiControlCheckbox.PositionX + (double) guiControlCheckbox.Size.X / 2.0 + (double) this.m_padding / 3.0);
        MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(guiControlCheckbox.Position - new Vector2((float) ((double) guiControlCheckbox.Size.X / 2.0 + (double) this.m_padding * 10.4499998092651), 0.0f)), text: MyTexts.GetString(text[0]));
        this.Controls.Add((MyGuiControlBase) guiControlCheckbox);
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      }
      if (!string.IsNullOrEmpty(text[1]))
      {
        MyGuiControlCheckbox guiControlCheckbox = new MyGuiControlCheckbox(new Vector2?(this.m_currentPosition), toolTip: (!string.IsNullOrEmpty(tooltip[1]) ? MyTexts.GetString(tooltip[1]) : string.Empty));
        guiControlCheckbox.PositionX = 0.262f;
        guiControlCheckbox.IsChecked = values[1];
        guiControlCheckbox.Enabled = enabled;
        guiControlCheckboxArray[1] = guiControlCheckbox;
        if (onClick[1] != null)
          guiControlCheckbox.IsCheckedChanged += onClick[1];
        this.m_currentPosition.X = (float) ((double) guiControlCheckbox.PositionX + (double) guiControlCheckbox.Size.X / 2.0 + (double) this.m_padding / 2.0);
        MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(guiControlCheckbox.Position - new Vector2((float) ((double) guiControlCheckbox.Size.X / 2.0 + (double) this.m_padding * 10.4499998092651), 0.0f)), text: MyTexts.GetString(text[1]));
        this.Controls.Add((MyGuiControlBase) guiControlCheckbox);
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      }
      this.m_currentPosition.X = x;
      this.m_currentPosition.Y += (float) ((double) ((IEnumerable<MyGuiControlCheckbox>) guiControlCheckboxArray).First<MyGuiControlCheckbox>((Func<MyGuiControlCheckbox, bool>) (c => c != null)).Size.Y / 2.0 + (double) this.m_padding + 0.00499999988824129);
      return guiControlCheckboxArray;
    }

    protected MyGuiControlIndeterminateCheckbox[] AddIndeterminateDuo(
      MyStringId?[] text,
      Action<MyGuiControlIndeterminateCheckbox>[] onClick,
      MyStringId?[] tooltip,
      CheckStateEnum[] values,
      bool enabled = true,
      float maxLabelWidth = float.PositiveInfinity)
    {
      MyGuiControlIndeterminateCheckbox[] indeterminateCheckboxArray = new MyGuiControlIndeterminateCheckbox[2];
      float x = this.m_currentPosition.X;
      if (text[0].HasValue)
      {
        MyGuiControlIndeterminateCheckbox indeterminateCheckbox = new MyGuiControlIndeterminateCheckbox(new Vector2?(this.m_currentPosition), toolTip: (tooltip[0].HasValue ? MyTexts.GetString(tooltip[0].Value) : string.Empty));
        indeterminateCheckbox.PositionX = -0.0435f;
        indeterminateCheckbox.State = values[0];
        indeterminateCheckboxArray[0] = indeterminateCheckbox;
        if (onClick[0] != null)
          indeterminateCheckbox.IsCheckedChanged += onClick[0];
        this.m_currentPosition.X = (float) ((double) indeterminateCheckbox.PositionX + (double) indeterminateCheckbox.Size.X / 2.0 + (double) this.m_padding / 3.0);
        MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(indeterminateCheckbox.Position - new Vector2((float) ((double) indeterminateCheckbox.Size.X / 2.0 + (double) this.m_padding * 10.4499998092651), 0.0f)), text: MyTexts.GetString(text[0].Value));
        indeterminateCheckbox.Enabled = enabled;
        myGuiControlLabel.Enabled = enabled;
        this.Controls.Add((MyGuiControlBase) indeterminateCheckbox);
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      }
      if (text[1].HasValue)
      {
        MyGuiControlIndeterminateCheckbox indeterminateCheckbox = new MyGuiControlIndeterminateCheckbox(new Vector2?(this.m_currentPosition), toolTip: (tooltip[1].HasValue ? MyTexts.GetString(tooltip[1].Value) : string.Empty));
        indeterminateCheckbox.PositionX = 0.262f;
        indeterminateCheckbox.State = values[1];
        indeterminateCheckboxArray[1] = indeterminateCheckbox;
        if (onClick[1] != null)
          indeterminateCheckbox.IsCheckedChanged += onClick[1];
        this.m_currentPosition.X = (float) ((double) indeterminateCheckbox.PositionX + (double) indeterminateCheckbox.Size.X / 2.0 + (double) this.m_padding / 2.0);
        MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(indeterminateCheckbox.Position - new Vector2((float) ((double) indeterminateCheckbox.Size.X / 2.0 + (double) this.m_padding * 10.4499998092651), 0.0f)), text: MyTexts.GetString(text[1].Value));
        if ((double) maxLabelWidth != double.PositiveInfinity)
        {
          myGuiControlLabel.IsAutoScaleEnabled = true;
          myGuiControlLabel.SetMaxWidth(maxLabelWidth);
        }
        indeterminateCheckbox.Enabled = enabled;
        myGuiControlLabel.Enabled = enabled;
        this.Controls.Add((MyGuiControlBase) indeterminateCheckbox);
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      }
      this.m_currentPosition.X = x;
      this.m_currentPosition.Y += (float) ((double) ((IEnumerable<MyGuiControlIndeterminateCheckbox>) indeterminateCheckboxArray).First<MyGuiControlIndeterminateCheckbox>((Func<MyGuiControlIndeterminateCheckbox, bool>) (c => c != null)).Size.Y / 2.0 + (double) this.m_padding + 0.00499999988824129);
      return indeterminateCheckboxArray;
    }

    public override bool Update(bool hasFocus)
    {
      int num = base.Update(hasFocus) ? 1 : 0;
      if (!hasFocus)
        return num != 0;
      this.m_btnDefault.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_searchButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      return num != 0;
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
        this.SearchClick((MyGuiControlButton) null);
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y))
      {
        this.DefaultSettingsClick((MyGuiControlButton) null);
        this.DefaultModsClick((MyGuiControlButton) null);
      }
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_LEFT))
        this.SwitchSelectedTab(false);
      if (!MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_RIGHT))
        return;
      this.SwitchSelectedTab(true);
    }

    private void SwitchSelectedTab(bool right)
    {
      bool flag = MyPlatformGameSettings.IsModdingAllowed && MyFakes.ENABLE_WORKSHOP_MODS;
      switch (this.CurrentPage)
      {
        case MyGuiScreenServerSearchBase.SearchPageEnum.Settings:
          if (right)
          {
            this.CurrentPage = MyGuiScreenServerSearchBase.SearchPageEnum.Advanced;
            break;
          }
          if (flag)
          {
            this.CurrentPage = MyGuiScreenServerSearchBase.SearchPageEnum.Mods;
            break;
          }
          this.CurrentPage = MyGuiScreenServerSearchBase.SearchPageEnum.Advanced;
          break;
        case MyGuiScreenServerSearchBase.SearchPageEnum.Advanced:
          if (right)
          {
            if (flag)
            {
              this.CurrentPage = MyGuiScreenServerSearchBase.SearchPageEnum.Mods;
              break;
            }
            this.CurrentPage = MyGuiScreenServerSearchBase.SearchPageEnum.Settings;
            break;
          }
          this.CurrentPage = MyGuiScreenServerSearchBase.SearchPageEnum.Settings;
          break;
        case MyGuiScreenServerSearchBase.SearchPageEnum.Mods:
          if (right)
          {
            this.CurrentPage = MyGuiScreenServerSearchBase.SearchPageEnum.Settings;
            break;
          }
          this.CurrentPage = MyGuiScreenServerSearchBase.SearchPageEnum.Advanced;
          break;
      }
    }

    protected enum SearchPageEnum
    {
      Settings,
      Advanced,
      Mods,
    }
  }
}
