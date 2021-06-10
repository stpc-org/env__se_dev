// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenSimpleNewGame
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Mvvm;
using ParallelTasks;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.Screens.ViewModels;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using VRage;
using VRage.Audio;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Localization;
using VRage.Game.ObjectBuilders.Campaign;
using VRage.GameServices;
using VRage.Input;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public sealed class MyGuiScreenSimpleNewGame : MyGuiScreenBase
  {
    private static MyGuiCompositeTexture BUTTON_TEXTURE_LEFT = new MyGuiCompositeTexture("Textures\\GUI\\Controls\\LeftArrow_focus.png");
    private static MyGuiCompositeTexture BUTTON_TEXTURE_RIGHT = new MyGuiCompositeTexture("Textures\\GUI\\Controls\\RightArrow_focus.png");
    private static MyGuiControlImageButton.StyleDefinition STYLE_BUTTON_LEFT = new MyGuiControlImageButton.StyleDefinition()
    {
      Active = new MyGuiControlImageButton.StateDefinition()
      {
        Texture = MyGuiScreenSimpleNewGame.BUTTON_TEXTURE_LEFT
      },
      Disabled = new MyGuiControlImageButton.StateDefinition()
      {
        Texture = MyGuiScreenSimpleNewGame.BUTTON_TEXTURE_LEFT
      },
      Normal = new MyGuiControlImageButton.StateDefinition()
      {
        Texture = MyGuiScreenSimpleNewGame.BUTTON_TEXTURE_LEFT
      },
      Highlight = new MyGuiControlImageButton.StateDefinition()
      {
        Texture = MyGuiScreenSimpleNewGame.BUTTON_TEXTURE_LEFT
      },
      ActiveHighlight = new MyGuiControlImageButton.StateDefinition()
      {
        Texture = MyGuiScreenSimpleNewGame.BUTTON_TEXTURE_LEFT
      },
      Padding = new MyGuiBorderThickness(0.005f, 0.005f)
    };
    private static MyGuiControlImageButton.StyleDefinition STYLE_BUTTON_RIGHT = new MyGuiControlImageButton.StyleDefinition()
    {
      Active = new MyGuiControlImageButton.StateDefinition()
      {
        Texture = MyGuiScreenSimpleNewGame.BUTTON_TEXTURE_RIGHT
      },
      Disabled = new MyGuiControlImageButton.StateDefinition()
      {
        Texture = MyGuiScreenSimpleNewGame.BUTTON_TEXTURE_RIGHT
      },
      Normal = new MyGuiControlImageButton.StateDefinition()
      {
        Texture = MyGuiScreenSimpleNewGame.BUTTON_TEXTURE_RIGHT
      },
      Highlight = new MyGuiControlImageButton.StateDefinition()
      {
        Texture = MyGuiScreenSimpleNewGame.BUTTON_TEXTURE_RIGHT
      },
      ActiveHighlight = new MyGuiControlImageButton.StateDefinition()
      {
        Texture = MyGuiScreenSimpleNewGame.BUTTON_TEXTURE_RIGHT
      },
      Padding = new MyGuiBorderThickness(0.005f, 0.005f)
    };
    private static readonly float ITEM_SCALE = 1.165f;
    private static readonly float ITEM_SPACING = 0.02f;
    private static readonly float IMAGE_SCALE = 0.965f;
    private static readonly Vector2 ITEM_SIZE = new Vector2(0.3f, 0.55f) * MyGuiScreenSimpleNewGame.ITEM_SCALE;
    private static readonly Vector2 ITEM_POSITION_OFFSET = new Vector2(0.0f, -0.087f);
    private static readonly Vector2 ITEM_IMAGE_POSITION = new Vector2(0.0f, 0.166f);
    private static readonly Vector2 ITEM_CAPTION_POSITION = new Vector2(0.0f, 0.0f);
    private static readonly Vector2 ITEM_UPPER_BACKGROUND_POSITION = new Vector2(0.0f, -0.2f);
    private static readonly float ITEM_BRIGHT_BACKGROUND_HEIGHT = 0.21f * MyGuiScreenSimpleNewGame.ITEM_SCALE;
    private static readonly float ITEM_COMPLETE_BACKGROUND_HEIGHT = 0.29f * MyGuiScreenSimpleNewGame.ITEM_SCALE;
    private static readonly Vector2 ITEM_COMPLETE_BACKGROUND_POSITION = new Vector2(0.0f, -0.041f);
    private static readonly float START_BUTTON_HEIGHT = 0.07f;
    private static readonly float ITEM_CAPTION_SCALE = 1.25f;
    private static readonly MyGuiCompositeTexture DARK_BACKGROUND_TEXTURE = new MyGuiCompositeTexture("Textures\\GUI\\Controls\\DarkBlueBackground.png");
    private static readonly MyGuiCompositeTexture BRIGHT_BACKGROUND_TEXTURE = new MyGuiCompositeTexture("Textures\\GUI\\Controls\\rectangle_button_focus_center.dds");
    private static readonly float DEFAULT_OPACITY = 0.92f;
    private readonly int ITEM_COUNT = 9;
    private readonly float SHIFT_SPEED = 0.045f;
    private MyGuiScreenSimpleNewGame.DataItem m_activeItem;
    private int m_activeIndex;
    private int m_nextIndex;
    private List<MyGuiScreenSimpleNewGame.DataItem> m_items = new List<MyGuiScreenSimpleNewGame.DataItem>();
    private List<MyGuiScreenSimpleNewGame.Item> m_guiItems = new List<MyGuiScreenSimpleNewGame.Item>();
    private int m_activeGuiItem;
    private MyGuiControlButton m_buttonStart;
    private MyGuiControlImageButton m_buttonLeft;
    private MyGuiControlImageButton m_buttonRight;
    private MyGuiScreenSimpleNewGame.DataItem m_campaignTheFirstJump;
    private MyGuiScreenSimpleNewGame.DataItem m_campaignLearningToSurvive;
    private MyGuiScreenSimpleNewGame.DataItem m_campaignNeverSurrender;
    private MyGuiScreenSimpleNewGame.DataItem m_campaignFrostbite;
    private MyGuiScreenSimpleNewGame.DataItem m_campaignScrapRace;
    private MyGuiScreenSimpleNewGame.DataItem m_campaignLostColony;
    private MyGuiControlMultilineText m_description;
    private float m_animationValueCurrent;
    private float m_animationLinearCurrent;
    private float m_animationLinearNext;
    private float m_animationSpeed;
    private float m_animationDelinearizingValue;
    private int m_guiItemsMiddle;
    private bool m_parallelLoadIsRunning;

    public MyGuiScreenSimpleNewGame()
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.8f, 0.7f)), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      this.EnabledBackgroundFade = true;
      this.RecreateControls(true);
      this.m_backgroundColor = new Vector4?((Vector4) Color.Transparent);
      this.m_backgroundFadeColor = (Color) Vector4.Zero;
      this.SetVideoOverlayColor(new Vector4(0.0f, 0.0f, 0.0f, 1f));
    }

    private void CampaignStarted(string name)
    {
      if (string.IsNullOrEmpty(name))
        return;
      List<string> campaignsStarted = MySandboxGame.Config.CampaignsStarted;
      if (campaignsStarted.Contains(name))
        return;
      campaignsStarted.Add(name);
      MySandboxGame.Config.Save();
    }

    private bool WasCampaignStarted(string name) => !string.IsNullOrEmpty(name) && MySandboxGame.Config.CampaignsStarted.Contains(name);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      MyObjectBuilder_Campaign leaningToSurvive = (MyObjectBuilder_Campaign) null;
      MyObjectBuilder_Campaign neverSurrender = (MyObjectBuilder_Campaign) null;
      MyObjectBuilder_Campaign campaign = (MyObjectBuilder_Campaign) null;
      MyObjectBuilder_Campaign frostbite = (MyObjectBuilder_Campaign) null;
      MyObjectBuilder_Campaign scrapRace = (MyObjectBuilder_Campaign) null;
      foreach (MyObjectBuilder_Campaign campaign1 in MyCampaignManager.Static.Campaigns)
      {
        string name = campaign1.Name;
        if (!(name == "The First Jump"))
        {
          if (!(name == "Learning to Survive"))
          {
            if (!(name == "Never Surrender"))
            {
              if (!(name == "Frostbite"))
              {
                if (!(name == "Scrap Race"))
                {
                  if (name == "Lost Colony")
                    ;
                }
                else
                  scrapRace = campaign1;
              }
              else
                frostbite = campaign1;
            }
            else
              neverSurrender = campaign1;
          }
          else
            leaningToSurvive = campaign1;
        }
        else
          campaign = campaign1;
      }
      this.m_campaignTheFirstJump = this.AddItem(campaign.Name, MyCommonTexts.SimpleNewGame_TheFirstJump, MyCommonTexts.SimpleNewGame_TheFirstJump_Description, MyCommonTexts.SimpleNewGame_Start, campaign.ImagePath, (Action<MyGuiScreenSimpleNewGame.DataItem>) (x =>
      {
        if (this.WasCampaignStarted(campaign.Name) || this.m_campaignLearningToSurvive == null)
        {
          MySandboxGame.Config.NewNewGameScreenLastSelection = x.Id;
          MySandboxGame.Config.Save();
        }
        else
        {
          MySandboxGame.Config.NewNewGameScreenLastSelection = this.m_campaignLearningToSurvive.Id;
          MySandboxGame.Config.Save();
          this.CampaignStarted(campaign.Name);
        }
        this.StartScenario(campaign, false);
      }));
      this.m_campaignLearningToSurvive = this.AddItem(leaningToSurvive.Name, MyCommonTexts.SimpleNewGame_LearningToSurvive, MyCommonTexts.SimpleNewGame_LearningToSurvive_Description, MyCommonTexts.SimpleNewGame_Start, leaningToSurvive.ImagePath, (Action<MyGuiScreenSimpleNewGame.DataItem>) (x =>
      {
        if (this.WasCampaignStarted(leaningToSurvive.Name) || this.m_campaignNeverSurrender == null)
        {
          MySandboxGame.Config.NewNewGameScreenLastSelection = x.Id;
          MySandboxGame.Config.Save();
        }
        else
        {
          MySandboxGame.Config.NewNewGameScreenLastSelection = this.m_campaignNeverSurrender.Id;
          MySandboxGame.Config.Save();
          this.CampaignStarted(leaningToSurvive.Name);
        }
        this.StartScenario(leaningToSurvive, false);
      }));
      if (frostbite != null)
        this.m_campaignFrostbite = this.AddItem(frostbite.Name, MySpaceTexts.DisplayName_DLC_Frostbite, MySpaceTexts.SimpleNewGame_Frostbite_Description, MyCommonTexts.SimpleNewGame_Start, frostbite.ImagePath, (Action<MyGuiScreenSimpleNewGame.DataItem>) (x =>
        {
          if (x.IsEnabled)
          {
            MySandboxGame.Config.NewNewGameScreenLastSelection = x.Id;
            MySandboxGame.Config.Save();
            this.CampaignStarted(frostbite.Name);
            this.StartScenario(frostbite, MyPlatformGameSettings.PREFER_ONLINE);
          }
          else
            MyGameService.OpenDlcInShop(MyDLCs.GetDLC(MyDLCs.MyDLC.DLC_NAME_Frostbite).AppId);
        }), new MyStringId?(MySpaceTexts.OpenDlcShop), (Func<bool>) (() => MyGameService.IsDlcInstalled(MyDLCs.GetDLC(MyDLCs.MyDLC.DLC_NAME_Frostbite).AppId)));
      if (MySandboxGame.Config.ExperimentalMode)
        this.m_campaignScrapRace = this.AddItem(scrapRace.Name, MyCommonTexts.SimpleNewGame_ScrapRace, MyCommonTexts.SimpleNewGame_ScrapRace_Description, MyCommonTexts.SimpleNewGame_Start, scrapRace.ImagePath, (Action<MyGuiScreenSimpleNewGame.DataItem>) (x =>
        {
          MySandboxGame.Config.NewNewGameScreenLastSelection = x.Id;
          MySandboxGame.Config.Save();
          this.CampaignStarted(scrapRace.Name);
          this.StartScenario(scrapRace, MyPlatformGameSettings.PREFER_ONLINE);
        }));
      this.m_campaignNeverSurrender = this.AddItem(neverSurrender.Name, MyCommonTexts.SimpleNewGame_NeverSurrender, MyCommonTexts.SimpleNewGame_NeverSurrender_Description, MyCommonTexts.SimpleNewGame_Start, neverSurrender.ImagePath, (Action<MyGuiScreenSimpleNewGame.DataItem>) (x =>
      {
        MySandboxGame.Config.NewNewGameScreenLastSelection = x.Id;
        MySandboxGame.Config.Save();
        this.CampaignStarted(neverSurrender.Name);
        this.StartScenario(neverSurrender, MyPlatformGameSettings.PREFER_ONLINE);
      }));
      AddWorld(MyCommonTexts.SimpleNewGame_Creative, MyCommonTexts.SimpleNewGame_Creative_Description, MyCommonTexts.SimpleNewGame_Start, MyGameModeEnum.Creative, MyPlatformGameSettings.PREFER_ONLINE, "Red Ship");
      this.AddItem(string.Empty, MyCommonTexts.SimpleNewGame_Workshop, MyCommonTexts.WorkshopScreen_Description, MyCommonTexts.SimpleNewGame_Open, "Textures\\GUI\\Icons\\Workshop.jpg", (Action<MyGuiScreenSimpleNewGame.DataItem>) (x =>
      {
        MySandboxGame.Config.NewNewGameScreenLastSelection = x.Id;
        if (!MyGameService.AtLeastOneUGCServiceConsented)
        {
          MyModIoConsentViewModel consentViewModel = new MyModIoConsentViewModel(new Action(this.ShowNewWorkshopGameScreen));
          IMyGuiScreenFactoryService service = ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>();
          MyScreenManager.CloseScreenNow(service.GetMyGuiScreenBase(typeof (MyWorkshopBrowserViewModel)));
          service.CreateScreen((ViewModelBase) consentViewModel);
        }
        else
          this.ShowNewWorkshopGameScreen();
      }));
      this.AddItem(string.Empty, MyCommonTexts.SimpleNewGame_Custom, MyCommonTexts.WorldSettingsScreen_Description, MyCommonTexts.SimpleNewGame_Open, "Textures\\GUI\\Icons\\scenarios\\PreviewEarth.jpg", (Action<MyGuiScreenSimpleNewGame.DataItem>) (x =>
      {
        MySandboxGame.Config.NewNewGameScreenLastSelection = x.Id;
        MySandboxGame.Config.Save();
        MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenWorldSettings());
      }));
      Vector2 vector2_1 = new Vector2(MyGuiScreenSimpleNewGame.ITEM_SIZE.X, MyGuiScreenSimpleNewGame.START_BUTTON_HEIGHT);
      Vector2 vector2_2 = new Vector2(0.03f, 0.04f);
      this.m_buttonStart = new MyGuiControlButton(new Vector2?(new Vector2(0.0017f, 0.185f) + new Vector2(0.0f, 1f / 400f)), text: MyTexts.Get(MyCommonTexts.SimpleNewGame_Start));
      this.m_buttonStart.VisualStyle = MyGuiControlButtonStyleEnum.RectangularBorderLess;
      this.m_buttonStart.Size = vector2_1;
      this.m_buttonStart.ButtonClicked += new Action<MyGuiControlButton>(this.OnStartClicked);
      this.m_buttonStart.ColorMask = new Vector4(MyGuiScreenSimpleNewGame.DEFAULT_OPACITY);
      this.m_buttonStart.TextScale = MyGuiScreenSimpleNewGame.ITEM_CAPTION_SCALE;
      this.m_buttonStart.BorderSize = 0;
      this.m_buttonStart.BorderEnabled = false;
      double startButtonHeight1 = (double) MyGuiScreenSimpleNewGame.START_BUTTON_HEIGHT;
      double startButtonHeight2 = (double) MyGuiScreenSimpleNewGame.START_BUTTON_HEIGHT;
      Vector2 vector2_3 = new Vector2(0.02f, 0.0f);
      MyGuiScreenSimpleNewGame.BUTTON_TEXTURE_LEFT.MarkDirty();
      MyGuiScreenSimpleNewGame.BUTTON_TEXTURE_RIGHT.MarkDirty();
      Vector2 vector2_4 = new Vector2(0.0f, 0.02f);
      MyGuiControlImageButton controlImageButton1 = new MyGuiControlImageButton(position: new Vector2?(vector2_4 - (new Vector2(0.5f * this.m_buttonStart.Size.X, 0.0f) + vector2_3)));
      controlImageButton1.CanHaveFocus = false;
      this.m_buttonLeft = controlImageButton1;
      this.m_buttonLeft.Text = string.Empty;
      this.m_buttonLeft.ApplyStyle(MyGuiScreenSimpleNewGame.STYLE_BUTTON_LEFT);
      this.m_buttonLeft.Size = new Vector2(0.75f, 1f) * 0.035f;
      this.m_buttonLeft.ButtonClicked += new Action<MyGuiControlImageButton>(this.OnLeftClicked);
      this.m_buttonLeft.ColorMask = new Vector4(MyGuiScreenSimpleNewGame.DEFAULT_OPACITY);
      MyGuiControlImageButton controlImageButton2 = new MyGuiControlImageButton(position: new Vector2?(vector2_4 + (new Vector2(0.5f * this.m_buttonStart.Size.X, 0.0f) + vector2_3)));
      controlImageButton2.CanHaveFocus = false;
      this.m_buttonRight = controlImageButton2;
      this.m_buttonRight.Text = string.Empty;
      this.m_buttonRight.ApplyStyle(MyGuiScreenSimpleNewGame.STYLE_BUTTON_RIGHT);
      this.m_buttonRight.Size = new Vector2(0.75f, 1f) * 0.035f;
      this.m_buttonRight.ButtonClicked += new Action<MyGuiControlImageButton>(this.OnRightClicked);
      this.m_buttonRight.ColorMask = new Vector4(MyGuiScreenSimpleNewGame.DEFAULT_OPACITY);
      this.Controls.Add((MyGuiControlBase) this.m_buttonLeft);
      this.Controls.Add((MyGuiControlBase) this.m_buttonRight);
      MyGuiControlMultilineText controlMultilineText = new MyGuiControlMultilineText(new Vector2?(new Vector2(0.0f, 0.35f)), new Vector2?(new Vector2(0.7f, 0.12f)));
      controlMultilineText.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      controlMultilineText.VisualStyle = MyGuiControlMultilineStyleEnum.BackgroundBorderless;
      this.m_description = controlMultilineText;
      this.m_description.TextScale = 0.875f;
      this.m_description.TextPadding = new MyGuiBorderThickness(0.01f, 0.01f);
      for (int index = 0; index < this.m_items.Count; ++index)
        this.m_guiItems.Add(this.BuildGuiItem());
      this.m_guiItemsMiddle = 0;
      this.Controls.Add((MyGuiControlBase) this.m_description);
      this.InitialWorldSelection();
      this.Controls.Add((MyGuiControlBase) this.m_buttonStart);
      this.FocusedControl = (MyGuiControlBase) this.m_buttonStart;

      void AddWorld(
        MyStringId captionText,
        MyStringId descriptionText,
        MyStringId buttonText,
        MyGameModeEnum mode,
        bool preferOnline,
        string world)
      {
        string worldPath = Path.Combine(MyFileSystem.ContentPath, "CustomWorlds", world);
        this.AddItem(string.Empty, captionText, descriptionText, buttonText, Path.Combine(worldPath, MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION), (Action<MyGuiScreenSimpleNewGame.DataItem>) (x =>
        {
          MySandboxGame.Config.NewNewGameScreenLastSelection = x.Id;
          MySandboxGame.Config.Save();
          this.StartWorld(worldPath, mode, preferOnline);
        }));
      }
    }

    private void ShowNewWorkshopGameScreen()
    {
      MySandboxGame.Config.Save();
      MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenNewWorkshopGame(displayTabWorkshop: MyGameService.AtLeastOneUGCServiceConsented));
    }

    private void InitialWorldSelection()
    {
      if (MySandboxGame.Config.NewNewGameScreenLastSelection >= 0 && MySandboxGame.Config.NewNewGameScreenLastSelection < this.m_items.Count)
      {
        int screenLastSelection = MySandboxGame.Config.NewNewGameScreenLastSelection;
        this.ResetActiveIndex(screenLastSelection);
        this.m_guiItemsMiddle = screenLastSelection;
      }
      else if (this.m_campaignTheFirstJump != null)
        this.ResetActiveItem(this.m_campaignTheFirstJump);
      else
        this.ResetActiveIndex(0);
    }

    private void OnStartClicked(MyGuiControlButton obj)
    {
      if (this.m_activeItem == null)
        return;
      this.m_activeItem.Action(this.m_activeItem);
    }

    private void OnLeftClicked(MyGuiControlImageButton obj) => this.ShiftItems(1, this.SHIFT_SPEED);

    private void OnRightClicked(MyGuiControlImageButton obj) => this.ShiftItems(-1, this.SHIFT_SPEED);

    private MyGuiScreenSimpleNewGame.DataItem AddItem(
      string campaignName,
      MyObjectBuilder_Campaign campaign,
      Action<MyGuiScreenSimpleNewGame.DataItem> action)
    {
      MyCampaignManager.Static.ReloadMenuLocalization(campaign.Name);
      MyLocalizationContext localizationContext = MyLocalization.Static[campaign.Name];
      StringBuilder captionText;
      StringBuilder descriptionText;
      if (localizationContext != null)
      {
        captionText = localizationContext["Name"];
        descriptionText = localizationContext["Description"];
      }
      else
      {
        captionText = new StringBuilder(campaign.Name);
        descriptionText = new StringBuilder(campaign.Description);
      }
      MyGuiScreenSimpleNewGame.DataItem dataItem = new MyGuiScreenSimpleNewGame.DataItem(campaignName, captionText, descriptionText, MyTexts.Get(MyCommonTexts.SimpleNewGame_Start), campaign.ImagePath, action, this.m_items.Count);
      this.m_items.Add(dataItem);
      return dataItem;
    }

    private MyGuiScreenSimpleNewGame.DataItem AddItem(
      string name,
      MyStringId captionText,
      MyStringId descriptionText,
      MyStringId buttonText,
      string texture,
      Action<MyGuiScreenSimpleNewGame.DataItem> action,
      MyStringId? buttonTextDisabled = null,
      Func<bool> isEnabled = null)
    {
      MyGuiScreenSimpleNewGame.DataItem dataItem = new MyGuiScreenSimpleNewGame.DataItem(name, MyTexts.Get(captionText), MyTexts.Get(descriptionText), MyTexts.Get(buttonText), texture, action, this.m_items.Count, buttonTextDisabled.HasValue ? MyTexts.Get(buttonTextDisabled.Value) : (StringBuilder) null, isEnabled);
      this.m_items.Add(dataItem);
      return dataItem;
    }

    private void StartScenario(MyObjectBuilder_Campaign scenario, bool preferOnline)
    {
      if (this.m_parallelLoadIsRunning)
        return;
      this.m_parallelLoadIsRunning = true;
      MyGuiScreenProgress progressScreen = new MyGuiScreenProgress(MyTexts.Get(MySpaceTexts.ProgressScreen_LoadingWorld));
      MyScreenManager.AddScreen((MyGuiScreenBase) progressScreen);
      Parallel.StartBackground((Action) (() => this.StartScenarioInternal(scenario, preferOnline)), (Action) (() =>
      {
        progressScreen.CloseScreen();
        this.m_parallelLoadIsRunning = false;
      }));
    }

    private void StartScenarioInternal(MyObjectBuilder_Campaign scenario, bool preferOnline)
    {
      MyCampaignManager.Static.SwitchCampaign(scenario.Name, scenario.IsVanilla, scenario.PublishedFileId, scenario.PublishedServiceName, scenario.ModFolderPath);
      if (!preferOnline || !MyGameService.IsActive)
      {
        Run(false);
      }
      else
      {
        bool granted = false;
        int done = 0;
        MyGameService.Service.RequestPermissions(Permissions.Multiplayer, false, (Action<bool>) (x =>
        {
          if (x)
          {
            MyGameService.Service.RequestPermissions(Permissions.UGC, false, (Action<bool>) (ugcGranted =>
            {
              granted = ugcGranted;
              done = 1;
            }));
          }
          else
          {
            granted = false;
            done = 1;
          }
        }));
        this.WaitFor(ref done);
        Run(granted);
      }

      void Run(bool granted)
      {
        MyStringId errorMessage;
        if (!MyCloudHelper.IsError(MyCampaignManager.Static.RunNewCampaign(scenario.Name, granted ? MyOnlineModeEnum.FRIENDS : MyOnlineModeEnum.OFFLINE, MyMultiplayerLobby.MAX_PLAYERS, MyPlatformGameSettings.CONSOLE_COMPATIBLE ? "XBox" : (string) null), out errorMessage))
          return;
        MyGuiScreenMessageBox messageBox = MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(errorMessage), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
        messageBox.SkipTransition = true;
        messageBox.InstantClose = false;
        MyGuiSandbox.AddScreen((MyGuiScreenBase) messageBox);
      }
    }

    private void WaitFor(ref int done)
    {
      while (Interlocked.CompareExchange(ref done, 0, 0) == 0)
        Thread.Sleep(10);
    }

    private void StartWorld(string sessionPath, MyGameModeEnum gameMode, bool preferOnline)
    {
      if (this.m_parallelLoadIsRunning)
        return;
      this.m_parallelLoadIsRunning = true;
      MyGuiScreenProgress progressScreen = new MyGuiScreenProgress(MyTexts.Get(MySpaceTexts.ProgressScreen_LoadingWorld));
      MyScreenManager.AddScreen((MyGuiScreenBase) progressScreen);
      Parallel.StartBackground((Action) (() => StartWorldInner()), (Action) (() =>
      {
        progressScreen.CloseScreen();
        this.m_parallelLoadIsRunning = false;
      }));

      void StartWorldInner()
      {
        bool granted = false;
        if (preferOnline && MyGameService.IsActive)
        {
          int done = 0;
          MyGameService.Service.RequestPermissions(Permissions.Multiplayer, false, (Action<bool>) (x =>
          {
            if (x)
            {
              MyGameService.Service.RequestPermissions(Permissions.UGC, false, (Action<bool>) (ugcGranted =>
              {
                granted = ugcGranted;
                done = 1;
              }));
            }
            else
            {
              granted = false;
              done = 1;
            }
          }));
          this.WaitFor(ref done);
        }
        this.StartWorld(sessionPath, gameMode, granted ? MyOnlineModeEnum.FRIENDS : MyOnlineModeEnum.OFFLINE);
      }
    }

    private void StartWorld(
      string sessionPath,
      MyGameModeEnum gameMode,
      MyOnlineModeEnum onlineMode)
    {
      ulong sizeInBytes;
      MyObjectBuilder_Checkpoint checkpoint = MyLocalCache.LoadCheckpoint(sessionPath, out sizeInBytes, new MyGameModeEnum?(gameMode), new MyOnlineModeEnum?(onlineMode));
      if (checkpoint == null)
        return;
      if (!MySessionLoader.HasOnlyModsFromConsentedUGCs(checkpoint))
      {
        MySessionLoader.ShowUGCConsentNotAcceptedWarning(MySessionLoader.GetNonConsentedServiceNameInCheckpoint(checkpoint));
      }
      else
      {
        string saveName = MyStatControlText.SubstituteTexts(checkpoint.SessionName) + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        MySessionLoader.LoadSingleplayerSession(checkpoint, sessionPath, sizeInBytes, (Action) (() =>
        {
          string saveName = Path.Combine(MyFileSystem.SavesPath, saveName.Replace(':', '-'));
          MySession.Static.CurrentPath = saveName;
          MyAsyncSaving.DelayedSaveAfterLoad(saveName);
        }));
      }
    }

    private void ResetActiveIndex(int i)
    {
      if (this.m_items.Count <= i || i < 0)
        return;
      this.SetActiveIndex(i);
      this.BindItemsToGUI();
    }

    private void ResetActiveItem(MyGuiScreenSimpleNewGame.DataItem item)
    {
      this.BindItemsToGUI();
      int i = -1;
      for (int index = 0; index < this.m_items.Count; ++index)
      {
        if (this.m_items[index] == item)
        {
          i = index;
          break;
        }
      }
      if (i == -1)
        return;
      this.SetActiveIndex(i);
      this.m_guiItemsMiddle = i;
    }

    private void SetActiveIndex(int i)
    {
      if (this.m_items.Count <= i || i < 0)
        return;
      this.m_activeIndex = i;
      this.m_activeItem = this.m_items[i];
      this.m_description.Text = this.m_activeItem.DescriptionText;
      this.m_description.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      this.m_buttonStart.Text = this.m_activeItem.ButtonText.ToString();
      if (i <= 0)
        this.m_buttonLeft.Visible = false;
      else
        this.m_buttonLeft.Visible = true;
      if (i >= this.m_items.Count - 1)
        this.m_buttonRight.Visible = false;
      else
        this.m_buttonRight.Visible = true;
    }

    private void BindItemsToGUI()
    {
      int num = Math.Min(this.m_items.Count, this.m_guiItems.Count);
      for (int index = 0; index < num; ++index)
        this.m_guiItems[index].SetData(this.m_items[index], index);
    }

    private bool IsAnimating => (double) this.m_animationLinearCurrent != (double) this.m_animationLinearNext || (double) this.m_animationSpeed != 0.0;

    private void ShiftItems(int amount, float speed)
    {
      if (this.m_activeIndex - amount < 0 || this.m_activeIndex - amount >= this.m_items.Count)
        return;
      if (this.IsAnimating)
      {
        if (Math.Sign(amount) != Math.Sign(this.m_animationLinearNext))
          return;
        float num = this.m_animationLinearNext + (amount > 0 ? 1f : -1f);
        if ((double) Math.Abs(num) >= 2.0)
          return;
        this.m_animationSpeed = this.m_animationSpeed * ((this.m_animationLinearNext - this.m_animationLinearCurrent) / this.m_animationLinearNext) + speed;
        this.m_animationLinearNext = (float) Math.Sign(num - this.m_animationLinearCurrent);
        this.m_animationDelinearizingValue = Math.Abs(num - this.m_animationValueCurrent);
        this.m_animationLinearCurrent = 0.0f;
        this.SetActiveIndex((this.m_activeIndex + -amount % this.m_items.Count + this.m_items.Count) % this.m_items.Count);
      }
      else
      {
        this.m_animationDelinearizingValue = (float) Math.Abs(amount);
        this.m_animationSpeed = speed;
        this.m_animationLinearCurrent = 0.0f;
        this.m_animationLinearNext = amount > 0 ? 1f : -1f;
        this.SetActiveIndex((this.m_activeIndex + -amount % this.m_items.Count + this.m_items.Count) % this.m_items.Count);
      }
      MyGuiSoundManager.PlaySound(GuiSounds.MouseOver);
    }

    public override bool Update(bool hasFocus)
    {
      bool flag = base.Update(hasFocus);
      if (this.IsAnimating)
      {
        if ((double) this.m_animationLinearCurrent < (double) this.m_animationLinearNext)
        {
          if ((double) this.m_animationLinearCurrent + (double) this.m_animationSpeed >= (double) this.m_animationLinearNext)
          {
            this.m_animationLinearCurrent = this.m_animationLinearNext = 0.0f;
            this.m_animationSpeed = 0.0f;
            this.m_animationValueCurrent = (float) Math.Round((double) this.m_animationValueCurrent);
          }
          else
          {
            float num = this.RescaleTransitionSineSymmetric(this.m_animationLinearCurrent);
            this.m_animationLinearCurrent += this.m_animationSpeed;
            this.m_animationValueCurrent += this.m_animationDelinearizingValue * (this.RescaleTransitionSineSymmetric(this.m_animationLinearCurrent) - num);
          }
        }
        else if ((double) this.m_animationLinearCurrent > (double) this.m_animationLinearNext)
        {
          if ((double) this.m_animationLinearCurrent - (double) this.m_animationSpeed <= (double) this.m_animationLinearNext)
          {
            this.m_animationLinearCurrent = this.m_animationLinearNext = 0.0f;
            this.m_animationSpeed = 0.0f;
            this.m_animationValueCurrent = (float) Math.Round((double) this.m_animationValueCurrent);
          }
          else
          {
            float num = this.RescaleTransitionSineSymmetric(this.m_animationLinearCurrent);
            this.m_animationLinearCurrent -= this.m_animationSpeed;
            this.m_animationValueCurrent += this.m_animationDelinearizingValue * (this.RescaleTransitionSineSymmetric(this.m_animationLinearCurrent) - num);
          }
        }
      }
      if ((double) this.m_animationValueCurrent <= -1.0)
      {
        ++this.m_animationValueCurrent;
        ++this.m_guiItemsMiddle;
      }
      else if ((double) this.m_animationValueCurrent >= 1.0)
      {
        --this.m_animationValueCurrent;
        --this.m_guiItemsMiddle;
      }
      for (int index = 0; index < this.m_guiItems.Count; ++index)
      {
        float scale = this.ComputeScale((float) index + this.m_animationValueCurrent);
        float scale2 = this.ComputeScale2((float) index + this.m_animationValueCurrent);
        Vector2 position = this.ComputePosition((float) index + this.m_animationValueCurrent, scale);
        this.m_guiItems[index].SetScale(scale);
        this.m_guiItems[index].SetOpacity(this.m_guiItems[index].GetData().IsEnabled, Math.Abs(index - this.m_guiItemsMiddle), scale * MyGuiScreenSimpleNewGame.DEFAULT_OPACITY, scale2, scale * ((1f - scale2) * MyGuiScreenSimpleNewGame.DEFAULT_OPACITY + scale2), scale * (1f - scale2) * MyGuiScreenSimpleNewGame.DEFAULT_OPACITY);
        this.m_guiItems[index].SetPosition(position);
      }
      float num1 = Math.Abs(this.m_animationLinearCurrent % 1f);
      if ((double) num1 > 0.5)
        num1 = 1f - num1;
      float num2 = 0.0f;
      if ((double) num1 < 0.200000002980232)
        num2 = (float) (1.0 - 5.0 * (double) num1);
      Vector4 vector4 = new Vector4(num2);
      this.m_buttonLeft.ColorMask = vector4;
      this.m_buttonRight.ColorMask = vector4;
      return flag;
    }

    private void AddItemToStartRemoveFromEnd()
    {
      int previoudIdx = 0;
      MyGuiScreenSimpleNewGame.DataItem previousData;
      this.GetDataItemPrevious(this.m_guiItems[0], out previousData, out previoudIdx);
      MyGuiScreenSimpleNewGame.Item obj = this.BuildGuiItem(previousData);
      obj.SetData(previousData, previoudIdx);
      this.m_guiItems.Insert(0, obj);
      this.Controls.Remove((MyGuiControlBase) this.m_guiItems[this.m_guiItems.Count - 1]);
      this.m_guiItems.RemoveAt(this.m_guiItems.Count - 1);
    }

    private void AddItemToEndRemoveFromStart()
    {
      int nextIdx = 0;
      MyGuiScreenSimpleNewGame.DataItem nextData;
      this.GetDataItemNext(this.m_guiItems[this.m_guiItems.Count - 1], out nextData, out nextIdx);
      MyGuiScreenSimpleNewGame.Item obj = this.BuildGuiItem(nextData);
      obj.SetData(nextData, nextIdx);
      this.m_guiItems.Insert(this.m_guiItems.Count, obj);
      this.Controls.Remove((MyGuiControlBase) this.m_guiItems[0]);
      this.m_guiItems.RemoveAt(0);
    }

    private void GetDataItemPrevious(
      MyGuiScreenSimpleNewGame.Item item,
      out MyGuiScreenSimpleNewGame.DataItem previousData,
      out int previoudIdx)
    {
      int index = (item.GetDataIndex() + this.m_items.Count - 1) % this.m_items.Count;
      previousData = this.m_items[index];
      previoudIdx = index;
    }

    private void GetDataItemNext(
      MyGuiScreenSimpleNewGame.Item item,
      out MyGuiScreenSimpleNewGame.DataItem nextData,
      out int nextIdx)
    {
      int index = (item.GetDataIndex() + 1) % this.m_items.Count;
      nextData = this.m_items[index];
      nextIdx = index;
    }

    private MyGuiScreenSimpleNewGame.Item BuildGuiItem(
      MyGuiScreenSimpleNewGame.DataItem data = null)
    {
      MyGuiScreenSimpleNewGame.Item obj = new MyGuiScreenSimpleNewGame.Item(MyGuiScreenSimpleNewGame.ITEM_SIZE, MyGuiScreenSimpleNewGame.ITEM_SPACING, MyGuiScreenSimpleNewGame.ITEM_POSITION_OFFSET);
      obj.OnItemClicked += new Action<MyGuiScreenSimpleNewGame.Item>(this.OnItemClicked);
      obj.OnItemDoubleClicked += new Action<MyGuiScreenSimpleNewGame.Item>(this.OnItemDoubleClicked);
      this.Controls.Add((MyGuiControlBase) obj);
      return obj;
    }

    private void OnItemClicked(MyGuiScreenSimpleNewGame.Item item)
    {
      if (this.IsAnimating)
        return;
      if (item.GetData() != this.m_activeItem)
        item.SuppressDoubleClick();
      int num1 = -1;
      for (int index = 0; index < this.m_guiItems.Count; ++index)
      {
        if (this.m_guiItems[index] == item)
        {
          num1 = index;
          break;
        }
      }
      if (num1 == -1)
        return;
      int num2 = num1 - this.m_guiItemsMiddle;
      if (num2 == 0)
        return;
      this.ShiftItems(-num2, this.SHIFT_SPEED);
    }

    private void OnItemDoubleClicked(MyGuiScreenSimpleNewGame.Item item)
    {
      if (this.m_activeItem != item.GetData())
        return;
      this.m_activeItem.Action(this.m_activeItem);
    }

    public float RescaleTransitionSineSymmetric(float input) => (float) Math.Sign(input) * this.RescaleTransitionSine(Math.Abs(input));

    public float RescaleTransitionSine(float input) => (float) Math.Sin((double) input * Math.PI * 0.5);

    public float ComputeScale(float coef)
    {
      float num = Math.Abs(coef - (float) this.m_guiItemsMiddle);
      return Math.Max((float) (-0.00700000021606684 * (double) num * (double) num * (double) num + 0.100000001490116 * (double) num * (double) num + -0.409999996423721 * (double) num + 1.0), 0.0f);
    }

    public float ComputeScale2(float coef) => Math.Max(1f - Math.Min(Math.Abs(coef - (float) this.m_guiItemsMiddle), 1f), 0.0f);

    public Vector2 ComputePosition(float coef, float scale)
    {
      float num1 = coef - (float) this.m_guiItemsMiddle;
      float num2 = Math.Abs(num1);
      return new Vector2((float) Math.Sign(num1) * (float) (0.0094999996945262 * (double) num2 * (double) num2 * (double) num2 + -0.0750000029802322 * (double) num2 * (double) num2 + 0.41100001335144 * (double) num2 + 0.0), (float) (0.00359999993816018 * (double) num2 * (double) num2 * (double) num2 + -0.0359999984502792 * (double) num2 * (double) num2 + 0.120999999344349 * (double) num2 - 0.0579999983310699));
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SWITCH_GUI_LEFT, MyControlStateType.PRESSED) || MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.MOVE_LEFT, MyControlStateType.PRESSED) || (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.PAGE_LEFT, MyControlStateType.PRESSED) || this.m_keyThrottler.GetKeyStatus(MyKeys.Left) == ThrottledKeyStatus.PRESSED_AND_READY))
        this.ShiftItems(1, this.SHIFT_SPEED);
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SWITCH_GUI_RIGHT, MyControlStateType.PRESSED) || MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.MOVE_RIGHT, MyControlStateType.PRESSED) || (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.PAGE_RIGHT, MyControlStateType.PRESSED) || this.m_keyThrottler.GetKeyStatus(MyKeys.Right) == ThrottledKeyStatus.PRESSED_AND_READY))
        this.ShiftItems(-1, this.SHIFT_SPEED);
      int num = MyInput.Static.DeltaMouseScrollWheelValue();
      if (num != 0)
      {
        if (!this.IsAnimating)
          this.ShiftItems(num / 120, this.SHIFT_SPEED);
        else
          this.m_animationSpeed *= 2f;
      }
      base.HandleInput(receivedFocusInThisUpdate);
    }

    public override bool Draw()
    {
      int num = base.Draw() ? 1 : 0;
      MyGuiSandbox.DrawGameLogoHandler(this.m_transitionAlpha, MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, 44, 68));
      return num != 0;
    }

    public override string GetFriendlyName() => "SimpleNewGame";

    public override bool CloseScreen(bool isUnloading = false)
    {
      this.SetVideoOverlayColor(new Vector4(1f, 1f, 1f, 1f));
      return base.CloseScreen(isUnloading);
    }

    private void SetVideoOverlayColor(Vector4 color)
    {
      MyGuiScreenIntroVideo firstScreenOfType = MyScreenManager.GetFirstScreenOfType<MyGuiScreenIntroVideo>();
      if (firstScreenOfType == null)
        return;
      firstScreenOfType.OverlayColorMask = color;
    }

    private class DataItem
    {
      public string Name;
      public Action<MyGuiScreenSimpleNewGame.DataItem> Action;
      public StringBuilder CaptionText;
      public StringBuilder DescriptionText;
      public StringBuilder ButtonTextEnabled;
      public StringBuilder ButtonTextDisabled;
      public string Texture;
      public int Id;
      private Func<bool> m_isEnabledFunc;

      public bool IsEnabled => this.m_isEnabledFunc == null || this.m_isEnabledFunc();

      public StringBuilder ButtonText => (this.IsEnabled || this.ButtonTextDisabled == null ? this.ButtonTextEnabled : this.ButtonTextDisabled) ?? new StringBuilder(string.Empty);

      public DataItem(
        string name,
        StringBuilder captionText,
        StringBuilder descriptionText,
        StringBuilder buttonTextEnabled,
        string texture,
        Action<MyGuiScreenSimpleNewGame.DataItem> action,
        int id,
        StringBuilder buttonTextDisabled = null,
        Func<bool> isEnabled = null)
      {
        this.Name = name;
        this.Action = action;
        this.CaptionText = captionText;
        this.DescriptionText = descriptionText;
        this.ButtonTextEnabled = buttonTextEnabled;
        this.ButtonTextDisabled = buttonTextDisabled;
        this.Texture = texture;
        this.Id = id;
        this.m_isEnabledFunc = isEnabled;
      }
    }

    private class Item : MyGuiControlParent
    {
      private float m_currentScale;
      private MyGuiControlLabel m_text;
      private MyGuiControlImage m_image;
      private MyGuiControlParent m_brightBackground;
      private MyGuiControlParent m_completeBackground;
      private Vector2 m_baseSize;
      private float m_baseCaptionSize;
      private float m_space;
      private Vector2 m_offset;
      private int m_dataIndex;
      private MyGuiScreenSimpleNewGame.DataItem m_data;
      private Vector2 m_imageOffset = MyGuiScreenSimpleNewGame.ITEM_IMAGE_POSITION;
      private Vector2 m_captionOffset = MyGuiScreenSimpleNewGame.ITEM_CAPTION_POSITION;
      private Vector2 m_upperImageOffset = MyGuiScreenSimpleNewGame.ITEM_UPPER_BACKGROUND_POSITION;
      private MyTimeSpan m_lastClickTime = new MyTimeSpan(0L);
      public Action<MyGuiScreenSimpleNewGame.Item> OnItemClicked;
      public Action<MyGuiScreenSimpleNewGame.Item> OnItemDoubleClicked;

      public Item(Vector2 size, float space, Vector2 offset)
      {
        this.m_baseSize = size;
        this.m_space = space;
        this.m_offset = offset;
        this.m_baseCaptionSize = MyGuiScreenSimpleNewGame.ITEM_CAPTION_SCALE;
        this.Size = this.m_baseSize;
        this.Position = this.m_offset;
        this.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
        this.m_image = new MyGuiControlImage();
        this.m_image.SetTexture("Textures\\GUI\\Icons\\scenarios\\PreviewCustomWorld.jpg");
        this.m_image.Position = MyGuiScreenSimpleNewGame.ITEM_IMAGE_POSITION;
        MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel();
        myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
        this.m_text = myGuiControlLabel;
        this.m_text.Size = this.m_text.GetTextSize();
        this.m_text.TextEnum = MyCommonTexts.SimpleNewGame_TheFirstJump;
        this.m_text.Position = this.m_captionOffset;
        MyGuiControlParent guiControlParent1 = new MyGuiControlParent(new Vector2?(this.m_image.Position), new Vector2?(new Vector2(this.m_baseSize.X, MyGuiScreenSimpleNewGame.ITEM_BRIGHT_BACKGROUND_HEIGHT)));
        guiControlParent1.BackgroundTexture = MyGuiScreenSimpleNewGame.BRIGHT_BACKGROUND_TEXTURE;
        this.m_brightBackground = guiControlParent1;
        MyGuiControlParent guiControlParent2 = new MyGuiControlParent(new Vector2?(this.m_image.Position), new Vector2?(new Vector2(this.m_baseSize.X, MyGuiScreenSimpleNewGame.ITEM_COMPLETE_BACKGROUND_HEIGHT)));
        guiControlParent2.BackgroundTexture = MyGuiScreenSimpleNewGame.DARK_BACKGROUND_TEXTURE;
        this.m_completeBackground = guiControlParent2;
        this.Controls.Add((MyGuiControlBase) this.m_completeBackground);
        this.Controls.Add((MyGuiControlBase) this.m_brightBackground);
        this.Controls.Add((MyGuiControlBase) this.m_image);
        this.Controls.Add((MyGuiControlBase) this.m_text);
      }

      public void SetData(MyGuiScreenSimpleNewGame.DataItem data, int index)
      {
        this.m_data = data;
        this.m_dataIndex = index;
        this.m_image.SetTexture(data.Texture);
        float num1 = MyGuiConstants.GUI_OPTIMAL_SIZE.X / MyGuiConstants.GUI_OPTIMAL_SIZE.Y;
        float num2 = 0.935f;
        Vector2 vector2 = new Vector2(1f, 0.5f);
        this.m_image.Size = new Vector2(this.Size.X * num2, num1 * this.Size.X * num2) * vector2;
        this.m_text.Text = data.CaptionText.ToString();
      }

      public MyGuiScreenSimpleNewGame.DataItem GetData() => this.m_data;

      public int GetDataIndex() => this.m_dataIndex;

      public void ActivateAction()
      {
        if (this.m_data == null || this.m_data.Action == null)
          return;
        this.m_data.Action(this.m_data);
      }

      public override MyGuiControlBase HandleInput()
      {
        if (MyInput.Static.IsNewPrimaryButtonPressed())
        {
          Vector2 positionAbsoluteTopLeft = this.GetPositionAbsoluteTopLeft();
          Vector2 size = this.GetPositionAbsoluteBottomRight() - positionAbsoluteTopLeft;
          if (new RectangleF(positionAbsoluteTopLeft, size).Contains(MyGuiManager.MouseCursorPosition))
          {
            bool flag = false;
            MyTimeSpan myTimeSpan = MyTimeSpan.FromMilliseconds((double) MySandboxGame.TotalTimeInMilliseconds);
            if (myTimeSpan - this.m_lastClickTime < MyTimeSpan.FromMilliseconds(200.0))
            {
              if (this.OnItemDoubleClicked != null)
                this.OnItemDoubleClicked(this);
              flag = true;
            }
            this.m_lastClickTime = myTimeSpan;
            if (!flag && this.OnItemClicked != null)
              this.OnItemClicked(this);
          }
        }
        return base.HandleInput();
      }

      public void SetScale(float scale)
      {
        this.Size = new Vector2(this.m_baseSize.X * scale, this.m_baseSize.Y * scale);
        float num = MyGuiConstants.GUI_OPTIMAL_SIZE.X / MyGuiConstants.GUI_OPTIMAL_SIZE.Y;
        Vector2 vector2 = new Vector2(1f, 0.5f);
        this.m_image.Size = new Vector2(this.Size.X * MyGuiScreenSimpleNewGame.IMAGE_SCALE, num * this.Size.X * MyGuiScreenSimpleNewGame.IMAGE_SCALE) * vector2;
        this.m_text.Size = this.m_text.GetTextSize() * scale;
        this.m_text.TextScale = this.m_baseCaptionSize * scale;
        this.m_image.Position = MyGuiScreenSimpleNewGame.ITEM_IMAGE_POSITION * scale;
        this.m_text.Position = this.m_captionOffset * scale;
        this.m_brightBackground.Size = new Vector2(this.m_baseSize.X, MyGuiScreenSimpleNewGame.ITEM_BRIGHT_BACKGROUND_HEIGHT) * scale;
        this.m_brightBackground.Position = this.m_image.Position;
        this.m_completeBackground.Size = new Vector2(this.m_baseSize.X, MyGuiScreenSimpleNewGame.ITEM_COMPLETE_BACKGROUND_HEIGHT) * scale;
        this.m_completeBackground.Position = this.m_image.Position + MyGuiScreenSimpleNewGame.ITEM_COMPLETE_BACKGROUND_POSITION * scale;
      }

      public void SetOpacity(
        bool enabled,
        int distanceFromPivot,
        float opacity,
        float opacity2,
        float opacityImg,
        float opacityBackground)
      {
        float num = 1f;
        Vector4 vector4_1 = new Vector4(opacity * num);
        Vector4 vector4_2 = new Vector4(opacity2 * num);
        Vector4 vector4_3 = new Vector4(opacityImg * num);
        Vector4 vector4_4 = new Vector4(opacityBackground * num);
        this.m_image.ColorMask = vector4_3;
        this.m_brightBackground.ColorMask = vector4_2;
        this.m_completeBackground.ColorMask = vector4_4;
        this.m_text.ColorMask = vector4_1;
      }

      public void SetPosition(Vector2 position) => this.Position = position + this.m_offset;

      internal void SuppressDoubleClick() => this.m_lastClickTime = new MyTimeSpan(0L);
    }
  }
}
