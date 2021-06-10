// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.ViewModels.MyModIoConsentViewModel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.GameServices;
using VRage.Input;
using VRageRender;

namespace Sandbox.Game.Screens.ViewModels
{
  public class MyModIoConsentViewModel : MyViewModelBase
  {
    private string m_consentCaption;
    private string m_consentTextPart1;
    private string m_consentTextPart2;
    private string m_consentTextPart3;
    private bool m_steamControlsVisible;
    private bool m_steamTOURequired;
    private bool m_modioTOURequired;
    private bool m_agreeButtonEnabled;
    private ICommand m_agreeCommand;
    private ICommand m_optOutCommand;
    private ICommand m_modioTermsOfUseCommand;
    private ICommand m_modioPrivacyPolicyCommand;
    private ICommand m_steamTermsOfUseCommand;
    private ICommand m_steamPrivacyPolicyCommand;
    private Action m_onConsentAgree;
    private Action m_onConsentOptOut;
    private float m_width;
    private float m_height;
    private bool m_leaveActionStarted;

    public float Width
    {
      get => this.m_width;
      set => this.SetProperty<float>(ref this.m_width, value, nameof (Width));
    }

    public float Height
    {
      get => this.m_height;
      set => this.SetProperty<float>(ref this.m_height, value, nameof (Height));
    }

    public ICommand AgreeCommand
    {
      get => this.m_agreeCommand;
      set => this.SetProperty<ICommand>(ref this.m_agreeCommand, value, nameof (AgreeCommand));
    }

    public ICommand OptOutCommand
    {
      get => this.m_optOutCommand;
      set => this.SetProperty<ICommand>(ref this.m_optOutCommand, value, nameof (OptOutCommand));
    }

    public ICommand ModioTermsOfUseCommand
    {
      get => this.m_modioTermsOfUseCommand;
      set => this.SetProperty<ICommand>(ref this.m_modioTermsOfUseCommand, value, nameof (ModioTermsOfUseCommand));
    }

    public ICommand ModioPrivacyPolicyCommand
    {
      get => this.m_modioPrivacyPolicyCommand;
      set => this.SetProperty<ICommand>(ref this.m_modioPrivacyPolicyCommand, value, nameof (ModioPrivacyPolicyCommand));
    }

    public ICommand SteamTermsOfUseCommand
    {
      get => this.m_steamTermsOfUseCommand;
      set => this.SetProperty<ICommand>(ref this.m_steamTermsOfUseCommand, value, nameof (SteamTermsOfUseCommand));
    }

    public ICommand SteamPrivacyPolicyCommand
    {
      get => this.m_steamPrivacyPolicyCommand;
      set => this.SetProperty<ICommand>(ref this.m_steamPrivacyPolicyCommand, value, nameof (SteamPrivacyPolicyCommand));
    }

    public string ConsentCaption
    {
      get => this.m_consentCaption;
      private set
      {
        this.m_consentCaption = value;
        this.RaisePropertyChanged(nameof (ConsentCaption));
      }
    }

    public string ConsentTextPart1
    {
      get => this.m_consentTextPart1;
      private set
      {
        this.m_consentTextPart1 = value;
        this.RaisePropertyChanged(nameof (ConsentTextPart1));
      }
    }

    public string ConsentTextPart2
    {
      get => this.m_consentTextPart2;
      private set
      {
        this.m_consentTextPart2 = value;
        this.RaisePropertyChanged(nameof (ConsentTextPart2));
      }
    }

    public string ConsentTextPart3
    {
      get => this.m_consentTextPart3;
      private set
      {
        this.m_consentTextPart3 = value;
        this.RaisePropertyChanged(nameof (ConsentTextPart3));
      }
    }

    public bool SteamControls
    {
      get => this.m_steamControlsVisible;
      private set
      {
        this.m_steamControlsVisible = value;
        this.RaisePropertyChanged(nameof (SteamControls));
      }
    }

    public bool SteamTOURequired
    {
      get => this.m_steamTOURequired;
      set
      {
        this.m_steamTOURequired = value;
        this.RaisePropertyChanged(nameof (SteamTOURequired));
        this.EnableAgreeButtonChecked();
      }
    }

    public bool ModioTOURequired
    {
      get => this.m_modioTOURequired;
      set
      {
        this.m_modioTOURequired = value;
        this.RaisePropertyChanged(nameof (ModioTOURequired));
        this.EnableAgreeButtonChecked();
      }
    }

    public bool AgreeButtonEnabled
    {
      get => this.m_agreeButtonEnabled;
      set
      {
        this.m_agreeButtonEnabled = value;
        this.RaisePropertyChanged(nameof (AgreeButtonEnabled));
        this.WarningVisible = this.m_agreeButtonEnabled ? Visibility.Hidden : Visibility.Visible;
        this.RaisePropertyChanged("WarningVisible");
        this.AgreeHelpTextForeground = (Brush) new SolidColorBrush(new ColorW(this.m_agreeButtonEnabled ? 4291222756U : 4284311918U));
        this.RaisePropertyChanged("AgreeHelpTextForeground");
      }
    }

    public Brush AgreeHelpTextForeground { get; private set; }

    public Visibility WarningVisible { get; private set; }

    public MyModIoConsentViewModel(Action onConsentAgree = null, Action onConsentOptOut = null)
      : base()
    {
      this.AgreeCommand = (ICommand) new RelayCommand(new Action<object>(this.OnAgree));
      this.OptOutCommand = (ICommand) new RelayCommand(new Action<object>(this.OnOptOut));
      this.ModioTermsOfUseCommand = (ICommand) new RelayCommand(new Action<object>(this.OnModioTermsOfUse));
      this.ModioPrivacyPolicyCommand = (ICommand) new RelayCommand(new Action<object>(this.OnModioPrivacyPolicy));
      this.SteamTermsOfUseCommand = (ICommand) new RelayCommand(new Action<object>(this.OnSteamTermsOfUse));
      this.SteamPrivacyPolicyCommand = (ICommand) new RelayCommand(new Action<object>(this.OnSteamPrivacyPolicy));
      this.m_onConsentAgree = onConsentAgree;
      this.m_onConsentOptOut = onConsentOptOut;
      this.SteamControls = false;
      this.ConsentCaption = string.Format(MyTexts.GetString(this.SteamControls ? MySpaceTexts.ScreenCaptionSteamAndModIoConsent : MySpaceTexts.ScreenCaptionModIoConsent));
      this.ConsentTextPart1 = string.Format(MyTexts.GetString(this.SteamControls ? MySpaceTexts.ScreenSteamAndModIoConsent_ConsentTextPart1 : MySpaceTexts.ScreenModIoConsent_ConsentTextPart1));
      this.ConsentTextPart2 = string.Format(MyTexts.GetString(this.SteamControls ? MySpaceTexts.ScreenSteamAndModIoConsent_ConsentTextPart2 : MySpaceTexts.ScreenModIoConsent_ConsentTextPart2), (object) MyGameService.Service.ServiceName);
      this.ConsentTextPart3 = string.Format(MyTexts.GetString(this.SteamControls ? MySpaceTexts.ScreenSteamAndModIoConsent_ConsentTextPart3 : MySpaceTexts.ScreenModIoConsent_ConsentTextPart3));
      this.m_steamTOURequired = this.SteamControls && !MySandboxGame.Config.SteamConsent;
      this.m_modioTOURequired = !MySandboxGame.Config.ModIoConsent;
      this.AgreeButtonEnabled = false;
      this.EnableAgreeButtonChecked();
      this.SetScreenSize();
    }

    public override void OnScreenClosed()
    {
      if (!this.m_leaveActionStarted)
      {
        this.m_leaveActionStarted = true;
        this.OnExit((object) this);
        Action onConsentOptOut = this.m_onConsentOptOut;
        if (onConsentOptOut != null)
          onConsentOptOut();
      }
      base.OnScreenClosed();
    }

    private void SetScreenSize()
    {
      this.Width = 700f;
      int num = MyInput.Static.IsJoystickLastUsed ? 145 : 190;
      if (!this.m_steamControlsVisible)
        num -= 70;
      MyRenderDeviceSettings currentDeviceSettings = MyVideoSettingsManager.CurrentDeviceSettings;
      switch (MyVideoSettingsManager.GetClosestAspectRatio((float) currentDeviceSettings.BackBufferWidth / (float) currentDeviceSettings.BackBufferHeight))
      {
        case MyAspectRatioEnum.Normal_4_3:
        case MyAspectRatioEnum.Dual_4_3:
        case MyAspectRatioEnum.Triple_4_3:
          this.Width = 800f;
          this.Height = (float) (775 + num);
          break;
        case MyAspectRatioEnum.Normal_16_9:
        case MyAspectRatioEnum.Dual_16_9:
        case MyAspectRatioEnum.Triple_16_9:
          this.Height = (float) (600 + num);
          break;
        case MyAspectRatioEnum.Normal_16_10:
        case MyAspectRatioEnum.Dual_16_10:
        case MyAspectRatioEnum.Triple_16_10:
          this.Height = (float) (700 + num);
          break;
        case MyAspectRatioEnum.Unsupported_5_4:
          this.Width = 850f;
          this.Height = (float) (775 + num);
          break;
        default:
          this.Height = (float) (700 + num);
          break;
      }
    }

    private void OnModioPrivacyPolicy(object obj) => MyGuiSandbox.OpenUrlWithFallback("https://mod.io/privacy", MyTexts.GetString(MySpaceTexts.ScreenModIoConsent_PrivacyPolicy_UrlFriendlyName));

    private void OnModioTermsOfUse(object obj) => MyGuiSandbox.OpenUrlWithFallback("https://mod.io/terms", MyTexts.GetString(MySpaceTexts.ScreenModIoConsent_TermsOfUse_UrlFriendlyName), onDone: ((Action<bool>) (success =>
    {
      if (!success)
        return;
      this.ModioTOURequired = false;
    })));

    private void OnSteamPrivacyPolicy(object obj) => MyGuiSandbox.OpenUrlWithFallback("https://store.steampowered.com/privacy_agreement/", MyTexts.GetString(MySpaceTexts.ScreenModIoConsent_SteamPrivacyPolicy_UrlFriendlyName));

    private void OnSteamTermsOfUse(object obj) => MyGuiSandbox.OpenUrlWithFallback("http://steamcommunity.com/sharedfiles/workshoplegalagreement", MyTexts.GetString(MySpaceTexts.ScreenModIoConsent_SteamTermsOfUse_UrlFriendlyName), onDone: ((Action<bool>) (success =>
    {
      if (!success)
        return;
      this.SteamTOURequired = false;
    })));

    private void OnOptOut(object obj)
    {
      IMyUGCService aggregate = MyGameService.WorkshopService.GetAggregate("mod.io");
      if (aggregate != null)
        aggregate.IsConsentGiven = false;
      MySandboxGame.Config.ModIoConsent = false;
      if (this.SteamControls)
        MySandboxGame.Config.SteamConsent = false;
      MySandboxGame.Config.Save();
      Action onConsentOptOut = this.m_onConsentOptOut;
      this.m_onConsentOptOut = (Action) null;
      this.m_leaveActionStarted = true;
      this.OnExit((object) this);
      if (onConsentOptOut == null)
        return;
      onConsentOptOut();
    }

    private void OnAgree(object obj)
    {
      if (!this.AgreeButtonEnabled)
        return;
      IMyUGCService aggregate = MyGameService.WorkshopService.GetAggregate("mod.io");
      if (aggregate != null)
        aggregate.IsConsentGiven = true;
      MySandboxGame.Config.ModIoConsent = true;
      if (this.SteamControls)
        MySandboxGame.Config.SteamConsent = true;
      MySandboxGame.Config.Save();
      this.m_leaveActionStarted = true;
      if (this.m_onConsentAgree != null)
        this.m_onConsentAgree();
      this.OnExit((object) this);
    }

    private void EnableAgreeButtonChecked()
    {
      if (this.m_modioTOURequired || this.m_steamTOURequired)
        return;
      this.AgreeButtonEnabled = true;
    }
  }
}
