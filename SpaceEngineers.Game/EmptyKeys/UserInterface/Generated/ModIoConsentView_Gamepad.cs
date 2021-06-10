// Decompiled with JetBrains decompiler
// Type: EmptyKeys.UserInterface.Generated.ModIoConsentView_Gamepad
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.ModIoConsentView_Gamepad_Bindings;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Themes;
using Sandbox.Game.Screens.ViewModels;
using System.CodeDom.Compiler;

namespace EmptyKeys.UserInterface.Generated
{
  [GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
  public class ModIoConsentView_Gamepad : UIRoot
  {
    private Grid rootGrid;
    private Image e_0;
    private Grid e_1;
    private ImageButton e_2;
    private StackPanel e_3;
    private TextBlock e_4;
    private Border e_5;
    private Grid e_6;
    private TextBlock e_7;
    private TextBlock e_8;
    private TextBlock e_9;
    private Grid e_10;
    private TextBlock e_11;
    private StackPanel e_12;
    private TextBlock e_13;
    private Image e_14;
    private TextBlock e_15;
    private TextBlock e_16;
    private StackPanel e_17;
    private StackPanel e_18;
    private TextBlock e_19;
    private Image e_20;
    private TextBlock e_21;
    private TextBlock e_22;
    private StackPanel e_23;
    private Border e_24;
    private StackPanel e_25;
    private TextBlock e_26;
    private TextBlock e_27;

    public ModIoConsentView_Gamepad() => this.Initialize();

    public ModIoConsentView_Gamepad(int width, int height)
      : base(width, height)
      => this.Initialize();

    private void Initialize()
    {
      Style rootStyle = RootStyle.CreateRootStyle();
      rootStyle.TargetType = this.GetType();
      this.Style = rootStyle;
      this.InitializeComponent();
    }

    private void InitializeComponent()
    {
      this.Background = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 0));
      this.MessageBoxOverlay = new ColorW(0, 0, 0, 240);
      ModIoConsentView_Gamepad.InitializeElementResources((UIElement) this);
      this.rootGrid = new Grid();
      this.Content = (object) this.rootGrid;
      this.rootGrid.Name = "rootGrid";
      this.rootGrid.HorizontalAlignment = HorizontalAlignment.Center;
      this.rootGrid.VerticalAlignment = VerticalAlignment.Center;
      this.rootGrid.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(0.1f, GridUnitType.Star)
      });
      this.rootGrid.RowDefinitions.Add(new RowDefinition());
      this.rootGrid.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(0.1f, GridUnitType.Star)
      });
      this.rootGrid.SetBinding(UIElement.WidthProperty, new Binding("Width")
      {
        UseGeneratedBindings = true
      });
      this.rootGrid.SetBinding(UIElement.HeightProperty, new Binding("Height")
      {
        UseGeneratedBindings = true
      });
      this.e_0 = new Image();
      this.rootGrid.Children.Add((UIElement) this.e_0);
      this.e_0.Name = "e_0";
      this.e_0.Source = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Screens\\screen_background.dds"
      };
      this.e_0.Stretch = Stretch.Fill;
      Grid.SetRow((UIElement) this.e_0, 1);
      this.e_0.SetBinding(ImageBrush.ColorOverlayProperty, new Binding("BackgroundOverlay")
      {
        UseGeneratedBindings = true
      });
      this.e_1 = new Grid();
      this.rootGrid.Children.Add((UIElement) this.e_1);
      this.e_1.Name = "e_1";
      GamepadBinding gamepadBinding1 = new GamepadBinding();
      gamepadBinding1.Gesture = (InputGesture) new GamepadGesture(GamepadInput.CButton);
      gamepadBinding1.SetBinding(InputBinding.CommandProperty, new Binding("AgreeCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_1.InputBindings.Add((InputBinding) gamepadBinding1);
      gamepadBinding1.Parent = (UIElement) this.e_1;
      GamepadBinding gamepadBinding2 = new GamepadBinding();
      gamepadBinding2.Gesture = (InputGesture) new GamepadGesture(GamepadInput.DButton);
      gamepadBinding2.SetBinding(InputBinding.CommandProperty, new Binding("OptOutCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_1.InputBindings.Add((InputBinding) gamepadBinding2);
      gamepadBinding2.Parent = (UIElement) this.e_1;
      GamepadBinding gamepadBinding3 = new GamepadBinding();
      gamepadBinding3.Gesture = (InputGesture) new GamepadGesture(GamepadInput.StartButton);
      gamepadBinding3.SetBinding(InputBinding.CommandProperty, new Binding("ModioTermsOfUseCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_1.InputBindings.Add((InputBinding) gamepadBinding3);
      gamepadBinding3.Parent = (UIElement) this.e_1;
      GamepadBinding gamepadBinding4 = new GamepadBinding();
      gamepadBinding4.Gesture = (InputGesture) new GamepadGesture(GamepadInput.SelectButton);
      gamepadBinding4.SetBinding(InputBinding.CommandProperty, new Binding("ModioPrivacyPolicyCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_1.InputBindings.Add((InputBinding) gamepadBinding4);
      gamepadBinding4.Parent = (UIElement) this.e_1;
      GamepadBinding gamepadBinding5 = new GamepadBinding();
      gamepadBinding5.Gesture = (InputGesture) new GamepadGesture(GamepadInput.LeftStickButton);
      gamepadBinding5.SetBinding(InputBinding.CommandProperty, new Binding("SteamTermsOfUseCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_1.InputBindings.Add((InputBinding) gamepadBinding5);
      gamepadBinding5.Parent = (UIElement) this.e_1;
      GamepadBinding gamepadBinding6 = new GamepadBinding();
      gamepadBinding6.Gesture = (InputGesture) new GamepadGesture(GamepadInput.RightStickButton);
      gamepadBinding6.SetBinding(InputBinding.CommandProperty, new Binding("SteamPrivacyPolicyCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_1.InputBindings.Add((InputBinding) gamepadBinding6);
      gamepadBinding6.Parent = (UIElement) this.e_1;
      this.e_1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_1.RowDefinitions.Add(new RowDefinition());
      this.e_1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_1.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(75f, GridUnitType.Pixel)
      });
      this.e_1.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(4f, GridUnitType.Star)
      });
      this.e_1.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(75f, GridUnitType.Pixel)
      });
      Grid.SetRow((UIElement) this.e_1, 1);
      this.e_2 = new ImageButton();
      this.e_1.Children.Add((UIElement) this.e_2);
      this.e_2.Name = "e_2";
      this.e_2.Height = 24f;
      this.e_2.Width = 24f;
      this.e_2.Margin = new Thickness(16f, 16f, 16f, 16f);
      this.e_2.HorizontalAlignment = HorizontalAlignment.Right;
      this.e_2.VerticalAlignment = VerticalAlignment.Center;
      this.e_2.TabIndex = 1;
      this.e_2.ImageStretch = Stretch.Uniform;
      this.e_2.ImageNormal = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol.dds"
      };
      this.e_2.ImageHover = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds"
      };
      this.e_2.ImagePressed = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds"
      };
      Grid.SetColumn((UIElement) this.e_2, 2);
      this.e_3 = new StackPanel();
      this.e_1.Children.Add((UIElement) this.e_3);
      this.e_3.Name = "e_3";
      Grid.SetColumn((UIElement) this.e_3, 1);
      Grid.SetRow((UIElement) this.e_3, 1);
      this.e_4 = new TextBlock();
      this.e_3.Children.Add((UIElement) this.e_4);
      this.e_4.Name = "e_4";
      this.e_4.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_4.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      this.e_4.SetBinding(TextBlock.TextProperty, new Binding("ConsentCaption")
      {
        UseGeneratedBindings = true
      });
      this.e_5 = new Border();
      this.e_3.Children.Add((UIElement) this.e_5);
      this.e_5.Name = "e_5";
      this.e_5.Height = 2f;
      this.e_5.Margin = new Thickness(0.0f, 10f, 0.0f, 10f);
      this.e_5.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      this.e_6 = new Grid();
      this.e_1.Children.Add((UIElement) this.e_6);
      this.e_6.Name = "e_6";
      this.e_6.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_6.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_6.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_6.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_6.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_6.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(48f, GridUnitType.Pixel)
      });
      Grid.SetColumn((UIElement) this.e_6, 1);
      Grid.SetRow((UIElement) this.e_6, 3);
      this.e_7 = new TextBlock();
      this.e_6.Children.Add((UIElement) this.e_7);
      this.e_7.Name = "e_7";
      this.e_7.Margin = new Thickness(0.0f, 20f, 0.0f, 20f);
      this.e_7.TextWrapping = TextWrapping.Wrap;
      Grid.SetRow((UIElement) this.e_7, 0);
      this.e_7.SetBinding(TextBlock.TextProperty, new Binding("ConsentTextPart1")
      {
        UseGeneratedBindings = true
      });
      this.e_8 = new TextBlock();
      this.e_6.Children.Add((UIElement) this.e_8);
      this.e_8.Name = "e_8";
      this.e_8.Margin = new Thickness(20f, 20f, 20f, 20f);
      Grid.SetRow((UIElement) this.e_8, 1);
      this.e_8.SetBinding(TextBlock.TextProperty, new Binding("ConsentTextPart2")
      {
        UseGeneratedBindings = true
      });
      this.e_9 = new TextBlock();
      this.e_6.Children.Add((UIElement) this.e_9);
      this.e_9.Name = "e_9";
      this.e_9.Margin = new Thickness(0.0f, 20f, 0.0f, 20f);
      this.e_9.TextWrapping = TextWrapping.Wrap;
      Grid.SetRow((UIElement) this.e_9, 2);
      this.e_9.SetBinding(TextBlock.TextProperty, new Binding("ConsentTextPart3")
      {
        UseGeneratedBindings = true
      });
      this.e_10 = new Grid();
      this.e_6.Children.Add((UIElement) this.e_10);
      this.e_10.Name = "e_10";
      this.e_10.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(40f, GridUnitType.Pixel)
      });
      this.e_10.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_10.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_10.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_10.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_10.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      this.e_10.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      this.e_10.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      Grid.SetColumn((UIElement) this.e_10, 0);
      Grid.SetRow((UIElement) this.e_10, 3);
      this.e_11 = new TextBlock();
      this.e_10.Children.Add((UIElement) this.e_11);
      this.e_11.Name = "e_11";
      this.e_11.Margin = new Thickness(3f, 10f, 3f, 10f);
      this.e_11.HorizontalAlignment = HorizontalAlignment.Right;
      this.e_11.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 44, 20, (int) byte.MaxValue));
      this.e_11.TextWrapping = TextWrapping.NoWrap;
      Grid.SetColumn((UIElement) this.e_11, 1);
      Grid.SetRow((UIElement) this.e_11, 0);
      Grid.SetColumnSpan((UIElement) this.e_11, 2);
      this.e_11.SetBinding(UIElement.VisibilityProperty, new Binding("WarningVisible")
      {
        UseGeneratedBindings = true
      });
      this.e_11.SetResourceReference(TextBlock.TextProperty, (object) "ScreenModIoConsent_LabelReadTOU");
      this.e_12 = new StackPanel();
      this.e_10.Children.Add((UIElement) this.e_12);
      this.e_12.Name = "e_12";
      this.e_12.Height = 28f;
      this.e_12.HorizontalAlignment = HorizontalAlignment.Right;
      this.e_12.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) this.e_12, 0);
      Grid.SetRow((UIElement) this.e_12, 1);
      Grid.SetColumnSpan((UIElement) this.e_12, 3);
      this.e_13 = new TextBlock();
      this.e_12.Children.Add((UIElement) this.e_13);
      this.e_13.Name = "e_13";
      this.e_13.Margin = new Thickness(0.0f, 0.0f, 15f, 0.0f);
      this.e_13.TextWrapping = TextWrapping.NoWrap;
      this.e_13.SetResourceReference(TextBlock.TextProperty, (object) "ScreenModIoConsent_LabelModIo");
      this.e_14 = new Image();
      this.e_12.Children.Add((UIElement) this.e_14);
      this.e_14.Name = "e_14";
      this.e_14.Height = 20f;
      this.e_14.Width = 20f;
      this.e_14.Margin = new Thickness(3f, 0.0f, 0.0f, 0.0f);
      this.e_14.Source = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Icons\\HUD 2017\\Notification_badge.png"
      };
      this.e_14.Stretch = Stretch.Uniform;
      this.e_14.SetBinding(UIElement.VisibilityProperty, new Binding("ModioTOURequired")
      {
        UseGeneratedBindings = true
      });
      this.e_15 = new TextBlock();
      this.e_12.Children.Add((UIElement) this.e_15);
      this.e_15.Name = "e_15";
      this.e_15.TextWrapping = TextWrapping.NoWrap;
      this.e_15.SetResourceReference(TextBlock.TextProperty, (object) "ScreenModIoConsent_TermsOfUseModioHelp");
      this.e_16 = new TextBlock();
      this.e_12.Children.Add((UIElement) this.e_16);
      this.e_16.Name = "e_16";
      this.e_16.Margin = new Thickness(20f, 0.0f, 0.0f, 0.0f);
      this.e_16.TextWrapping = TextWrapping.NoWrap;
      this.e_16.SetResourceReference(TextBlock.TextProperty, (object) "ScreenModIoConsent_PrivacyPolicyModioHelp");
      this.e_17 = new StackPanel();
      this.e_10.Children.Add((UIElement) this.e_17);
      this.e_17.Name = "e_17";
      this.e_17.Height = 20f;
      Grid.SetColumn((UIElement) this.e_17, 0);
      Grid.SetRow((UIElement) this.e_17, 2);
      this.e_17.SetBinding(UIElement.VisibilityProperty, new Binding("SteamControls")
      {
        UseGeneratedBindings = true
      });
      this.e_18 = new StackPanel();
      this.e_10.Children.Add((UIElement) this.e_18);
      this.e_18.Name = "e_18";
      this.e_18.HorizontalAlignment = HorizontalAlignment.Right;
      this.e_18.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) this.e_18, 0);
      Grid.SetRow((UIElement) this.e_18, 3);
      Grid.SetColumnSpan((UIElement) this.e_18, 3);
      this.e_19 = new TextBlock();
      this.e_18.Children.Add((UIElement) this.e_19);
      this.e_19.Name = "e_19";
      this.e_19.Height = 28f;
      this.e_19.Margin = new Thickness(0.0f, 0.0f, 15f, 0.0f);
      this.e_19.TextWrapping = TextWrapping.NoWrap;
      this.e_19.SetBinding(UIElement.VisibilityProperty, new Binding("SteamControls")
      {
        UseGeneratedBindings = true
      });
      this.e_19.SetResourceReference(TextBlock.TextProperty, (object) "ScreenModIoConsent_LabelSteam");
      this.e_20 = new Image();
      this.e_18.Children.Add((UIElement) this.e_20);
      this.e_20.Name = "e_20";
      this.e_20.Height = 20f;
      this.e_20.Width = 20f;
      this.e_20.Margin = new Thickness(3f, 0.0f, 0.0f, 0.0f);
      this.e_20.Source = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Icons\\HUD 2017\\Notification_badge.png"
      };
      this.e_20.Stretch = Stretch.Uniform;
      this.e_20.SetBinding(UIElement.VisibilityProperty, new Binding("SteamControls")
      {
        UseGeneratedBindings = true
      });
      this.e_21 = new TextBlock();
      this.e_18.Children.Add((UIElement) this.e_21);
      this.e_21.Name = "e_21";
      this.e_21.Height = 28f;
      this.e_21.TextWrapping = TextWrapping.NoWrap;
      this.e_21.SetBinding(UIElement.VisibilityProperty, new Binding("SteamControls")
      {
        UseGeneratedBindings = true
      });
      this.e_21.SetResourceReference(TextBlock.TextProperty, (object) "ScreenModIoConsent_TermsOfUseSteamHelp");
      this.e_22 = new TextBlock();
      this.e_18.Children.Add((UIElement) this.e_22);
      this.e_22.Name = "e_22";
      this.e_22.Height = 28f;
      this.e_22.Margin = new Thickness(20f, 0.0f, 0.0f, 0.0f);
      this.e_22.TextWrapping = TextWrapping.NoWrap;
      this.e_22.SetBinding(UIElement.VisibilityProperty, new Binding("SteamControls")
      {
        UseGeneratedBindings = true
      });
      this.e_22.SetResourceReference(TextBlock.TextProperty, (object) "ScreenModIoConsent_PrivacyPolicySteamHelp");
      this.e_23 = new StackPanel();
      this.e_6.Children.Add((UIElement) this.e_23);
      this.e_23.Name = "e_23";
      Grid.SetRow((UIElement) this.e_23, 4);
      this.e_24 = new Border();
      this.e_23.Children.Add((UIElement) this.e_24);
      this.e_24.Name = "e_24";
      this.e_24.Height = 2f;
      this.e_24.Margin = new Thickness(0.0f, 10f, 0.0f, 10f);
      this.e_24.VerticalAlignment = VerticalAlignment.Bottom;
      this.e_24.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      this.e_25 = new StackPanel();
      this.e_6.Children.Add((UIElement) this.e_25);
      this.e_25.Name = "e_25";
      this.e_25.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_25.VerticalAlignment = VerticalAlignment.Center;
      this.e_25.Orientation = Orientation.Horizontal;
      Grid.SetRow((UIElement) this.e_25, 5);
      this.e_26 = new TextBlock();
      this.e_25.Children.Add((UIElement) this.e_26);
      this.e_26.Name = "e_26";
      this.e_26.Margin = new Thickness(40f, 0.0f, 40f, 0.0f);
      this.e_26.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_26.SetBinding(Control.ForegroundProperty, new Binding("AgreeHelpTextForeground")
      {
        UseGeneratedBindings = true
      });
      this.e_26.SetResourceReference(TextBlock.TextProperty, (object) "ScreenModIoConsent_AgreeHelpText");
      this.e_27 = new TextBlock();
      this.e_25.Children.Add((UIElement) this.e_27);
      this.e_27.Name = "e_27";
      this.e_27.Margin = new Thickness(40f, 0.0f, 40f, 0.0f);
      this.e_27.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_27.SetResourceReference(TextBlock.TextProperty, (object) "ScreenModIoConsent_OptOutHelpText");
      ImageManager.Instance.AddImage("Textures\\GUI\\Screens\\screen_background.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol_highlight.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\HUD 2017\\Notification_badge.png");
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "Width", typeof (MyModIoConsentViewModel_Width_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "Height", typeof (MyModIoConsentViewModel_Height_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "BackgroundOverlay", typeof (MyModIoConsentViewModel_BackgroundOverlay_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "AgreeCommand", typeof (MyModIoConsentViewModel_AgreeCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "OptOutCommand", typeof (MyModIoConsentViewModel_OptOutCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "ModioTermsOfUseCommand", typeof (MyModIoConsentViewModel_ModioTermsOfUseCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "ModioPrivacyPolicyCommand", typeof (MyModIoConsentViewModel_ModioPrivacyPolicyCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "SteamTermsOfUseCommand", typeof (MyModIoConsentViewModel_SteamTermsOfUseCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "SteamPrivacyPolicyCommand", typeof (MyModIoConsentViewModel_SteamPrivacyPolicyCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "ConsentCaption", typeof (MyModIoConsentViewModel_ConsentCaption_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "ConsentTextPart1", typeof (MyModIoConsentViewModel_ConsentTextPart1_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "ConsentTextPart2", typeof (MyModIoConsentViewModel_ConsentTextPart2_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "ConsentTextPart3", typeof (MyModIoConsentViewModel_ConsentTextPart3_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "WarningVisible", typeof (MyModIoConsentViewModel_WarningVisible_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "ModioTOURequired", typeof (MyModIoConsentViewModel_ModioTOURequired_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "SteamControls", typeof (MyModIoConsentViewModel_SteamControls_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "AgreeHelpTextForeground", typeof (MyModIoConsentViewModel_AgreeHelpTextForeground_PropertyInfo));
    }

    private static void InitializeElementResources(UIElement elem) => elem.Resources.MergedDictionaries.Add((ResourceDictionary) Styles.Instance);
  }
}
