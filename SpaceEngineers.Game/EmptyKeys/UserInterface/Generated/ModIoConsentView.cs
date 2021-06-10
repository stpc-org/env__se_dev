// Decompiled with JetBrains decompiler
// Type: EmptyKeys.UserInterface.Generated.ModIoConsentView
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.ModIoConsentView_Bindings;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Themes;
using Sandbox.Game.Screens.ViewModels;
using System.CodeDom.Compiler;

namespace EmptyKeys.UserInterface.Generated
{
  [GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
  public class ModIoConsentView : UIRoot
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
    private TextBlock e_12;
    private Button e_13;
    private StackPanel e_14;
    private Image e_15;
    private TextBlock e_16;
    private Button e_17;
    private StackPanel e_18;
    private TextBlock e_19;
    private Button e_20;
    private StackPanel e_21;
    private Image e_22;
    private TextBlock e_23;
    private Button e_24;
    private StackPanel e_25;
    private Border e_26;
    private Button e_27;
    private Button e_28;

    public ModIoConsentView() => this.Initialize();

    public ModIoConsentView(int width, int height)
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
      ModIoConsentView.InitializeElementResources((UIElement) this);
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
      this.rootGrid.SetBinding(UIElement.MaxWidthProperty, new Binding("MaxWidth")
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
      this.e_1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(10f, GridUnitType.Pixel)
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
      this.e_2.SetBinding(Button.CommandProperty, new Binding("ExitCommand")
      {
        UseGeneratedBindings = true
      });
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
      this.e_6.RowDefinitions.Add(new RowDefinition());
      this.e_6.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_6.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_6.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      this.e_6.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      Grid.SetColumn((UIElement) this.e_6, 1);
      Grid.SetRow((UIElement) this.e_6, 4);
      this.e_7 = new TextBlock();
      this.e_6.Children.Add((UIElement) this.e_7);
      this.e_7.Name = "e_7";
      this.e_7.Margin = new Thickness(0.0f, 20f, 0.0f, 20f);
      this.e_7.TextWrapping = TextWrapping.Wrap;
      Grid.SetRow((UIElement) this.e_7, 0);
      Grid.SetColumnSpan((UIElement) this.e_7, 2);
      this.e_7.SetBinding(TextBlock.TextProperty, new Binding("ConsentTextPart1")
      {
        UseGeneratedBindings = true
      });
      this.e_8 = new TextBlock();
      this.e_6.Children.Add((UIElement) this.e_8);
      this.e_8.Name = "e_8";
      this.e_8.Margin = new Thickness(20f, 20f, 20f, 20f);
      Grid.SetColumn((UIElement) this.e_8, 0);
      Grid.SetRow((UIElement) this.e_8, 1);
      Grid.SetColumnSpan((UIElement) this.e_8, 2);
      this.e_8.SetBinding(TextBlock.TextProperty, new Binding("ConsentTextPart2")
      {
        UseGeneratedBindings = true
      });
      this.e_9 = new TextBlock();
      this.e_6.Children.Add((UIElement) this.e_9);
      this.e_9.Name = "e_9";
      this.e_9.Margin = new Thickness(0.0f, 20f, 0.0f, 20f);
      this.e_9.TextWrapping = TextWrapping.Wrap;
      Grid.SetColumn((UIElement) this.e_9, 0);
      Grid.SetRow((UIElement) this.e_9, 2);
      Grid.SetColumnSpan((UIElement) this.e_9, 2);
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
        Height = new GridLength(10f, GridUnitType.Pixel)
      });
      this.e_10.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(100f, GridUnitType.Pixel)
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
      Grid.SetRow((UIElement) this.e_10, 4);
      Grid.SetColumnSpan((UIElement) this.e_10, 2);
      this.e_11 = new TextBlock();
      this.e_10.Children.Add((UIElement) this.e_11);
      this.e_11.Name = "e_11";
      this.e_11.Margin = new Thickness(3f, 10f, 3f, 10f);
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
      this.e_12 = new TextBlock();
      this.e_10.Children.Add((UIElement) this.e_12);
      this.e_12.Name = "e_12";
      this.e_12.Margin = new Thickness(3f, 10f, 3f, 10f);
      this.e_12.HorizontalAlignment = HorizontalAlignment.Right;
      this.e_12.TextWrapping = TextWrapping.NoWrap;
      Grid.SetColumn((UIElement) this.e_12, 0);
      Grid.SetRow((UIElement) this.e_12, 1);
      this.e_12.SetResourceReference(TextBlock.TextProperty, (object) "ScreenModIoConsent_LabelModIo");
      this.e_13 = new Button();
      this.e_10.Children.Add((UIElement) this.e_13);
      this.e_13.Name = "e_13";
      this.e_13.Margin = new Thickness(3f, 0.0f, 3f, 0.0f);
      this.e_13.TabIndex = 2;
      Grid.SetColumn((UIElement) this.e_13, 1);
      Grid.SetRow((UIElement) this.e_13, 1);
      GamepadHelp.SetTabIndexUp((DependencyObject) this.e_13, 0);
      GamepadHelp.SetTabIndexDown((DependencyObject) this.e_13, 3);
      this.e_13.SetBinding(Button.CommandProperty, new Binding("ModioTermsOfUseCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_14 = new StackPanel();
      this.e_13.Content = (object) this.e_14;
      this.e_14.Name = "e_14";
      this.e_14.Orientation = Orientation.Horizontal;
      this.e_15 = new Image();
      this.e_14.Children.Add((UIElement) this.e_15);
      this.e_15.Name = "e_15";
      this.e_15.Height = 20f;
      this.e_15.Width = 20f;
      this.e_15.Source = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Icons\\HUD 2017\\Notification_badge.png"
      };
      this.e_15.Stretch = Stretch.Uniform;
      this.e_15.SetBinding(UIElement.VisibilityProperty, new Binding("ModioTOURequired")
      {
        UseGeneratedBindings = true
      });
      this.e_16 = new TextBlock();
      this.e_14.Children.Add((UIElement) this.e_16);
      this.e_16.Name = "e_16";
      this.e_16.SetResourceReference(TextBlock.TextProperty, (object) "ScreenModIoConsent_ButtonTermsOfUse");
      this.e_17 = new Button();
      this.e_10.Children.Add((UIElement) this.e_17);
      this.e_17.Name = "e_17";
      this.e_17.Margin = new Thickness(3f, 0.0f, 0.0f, 0.0f);
      this.e_17.TabIndex = 3;
      Grid.SetColumn((UIElement) this.e_17, 2);
      Grid.SetRow((UIElement) this.e_17, 1);
      GamepadHelp.SetTabIndexUp((DependencyObject) this.e_17, 2);
      this.e_17.SetBinding(Button.CommandProperty, new Binding("ModioPrivacyPolicyCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_17.SetResourceReference(ContentControl.ContentProperty, (object) "ScreenModIoConsent_ButtonPrivacyPolicy");
      this.e_18 = new StackPanel();
      this.e_10.Children.Add((UIElement) this.e_18);
      this.e_18.Name = "e_18";
      this.e_18.Height = 15f;
      Grid.SetColumn((UIElement) this.e_18, 0);
      Grid.SetRow((UIElement) this.e_18, 2);
      this.e_18.SetBinding(UIElement.VisibilityProperty, new Binding("SteamControls")
      {
        UseGeneratedBindings = true
      });
      this.e_19 = new TextBlock();
      this.e_10.Children.Add((UIElement) this.e_19);
      this.e_19.Name = "e_19";
      this.e_19.Margin = new Thickness(3f, 10f, 3f, 10f);
      this.e_19.HorizontalAlignment = HorizontalAlignment.Right;
      this.e_19.TextWrapping = TextWrapping.NoWrap;
      Grid.SetColumn((UIElement) this.e_19, 0);
      Grid.SetRow((UIElement) this.e_19, 3);
      this.e_19.SetBinding(UIElement.VisibilityProperty, new Binding("SteamControls")
      {
        UseGeneratedBindings = true
      });
      this.e_19.SetResourceReference(TextBlock.TextProperty, (object) "ScreenModIoConsent_LabelSteam");
      this.e_20 = new Button();
      this.e_10.Children.Add((UIElement) this.e_20);
      this.e_20.Name = "e_20";
      this.e_20.Margin = new Thickness(3f, 0.0f, 3f, 0.0f);
      this.e_20.TabIndex = 2;
      Grid.SetColumn((UIElement) this.e_20, 1);
      Grid.SetRow((UIElement) this.e_20, 3);
      GamepadHelp.SetTabIndexUp((DependencyObject) this.e_20, 0);
      GamepadHelp.SetTabIndexDown((DependencyObject) this.e_20, 3);
      this.e_20.SetBinding(UIElement.VisibilityProperty, new Binding("SteamControls")
      {
        UseGeneratedBindings = true
      });
      this.e_20.SetBinding(Button.CommandProperty, new Binding("SteamTermsOfUseCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_21 = new StackPanel();
      this.e_20.Content = (object) this.e_21;
      this.e_21.Name = "e_21";
      this.e_21.Orientation = Orientation.Horizontal;
      this.e_22 = new Image();
      this.e_21.Children.Add((UIElement) this.e_22);
      this.e_22.Name = "e_22";
      this.e_22.Height = 20f;
      this.e_22.Width = 20f;
      this.e_22.Source = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Icons\\HUD 2017\\Notification_badge.png"
      };
      this.e_22.Stretch = Stretch.Uniform;
      this.e_22.SetBinding(UIElement.VisibilityProperty, new Binding("SteamTOURequired")
      {
        UseGeneratedBindings = true
      });
      this.e_23 = new TextBlock();
      this.e_21.Children.Add((UIElement) this.e_23);
      this.e_23.Name = "e_23";
      this.e_23.SetResourceReference(TextBlock.TextProperty, (object) "ScreenModIoConsent_ButtonTermsOfUse");
      this.e_24 = new Button();
      this.e_10.Children.Add((UIElement) this.e_24);
      this.e_24.Name = "e_24";
      this.e_24.Margin = new Thickness(3f, 0.0f, 0.0f, 0.0f);
      this.e_24.TabIndex = 3;
      Grid.SetColumn((UIElement) this.e_24, 2);
      Grid.SetRow((UIElement) this.e_24, 3);
      GamepadHelp.SetTabIndexUp((DependencyObject) this.e_24, 2);
      this.e_24.SetBinding(UIElement.VisibilityProperty, new Binding("SteamControls")
      {
        UseGeneratedBindings = true
      });
      this.e_24.SetBinding(Button.CommandProperty, new Binding("SteamPrivacyPolicyCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_24.SetResourceReference(ContentControl.ContentProperty, (object) "ScreenModIoConsent_ButtonPrivacyPolicy");
      this.e_25 = new StackPanel();
      this.e_6.Children.Add((UIElement) this.e_25);
      this.e_25.Name = "e_25";
      Grid.SetColumn((UIElement) this.e_25, 0);
      Grid.SetRow((UIElement) this.e_25, 5);
      Grid.SetColumnSpan((UIElement) this.e_25, 2);
      this.e_26 = new Border();
      this.e_25.Children.Add((UIElement) this.e_26);
      this.e_26.Name = "e_26";
      this.e_26.Height = 2f;
      this.e_26.Margin = new Thickness(0.0f, 10f, 0.0f, 10f);
      this.e_26.VerticalAlignment = VerticalAlignment.Bottom;
      this.e_26.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      this.e_27 = new Button();
      this.e_6.Children.Add((UIElement) this.e_27);
      this.e_27.Name = "e_27";
      this.e_27.Margin = new Thickness(0.0f, 10f, 20f, 10f);
      this.e_27.TabIndex = 0;
      Grid.SetColumn((UIElement) this.e_27, 0);
      Grid.SetRow((UIElement) this.e_27, 6);
      GamepadHelp.SetTabIndexRight((DependencyObject) this.e_27, 1);
      GamepadHelp.SetTabIndexDown((DependencyObject) this.e_27, 2);
      this.e_27.SetBinding(UIElement.IsEnabledProperty, new Binding("AgreeButtonEnabled")
      {
        UseGeneratedBindings = true
      });
      this.e_27.SetBinding(Button.CommandProperty, new Binding("AgreeCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_27.SetResourceReference(ContentControl.ContentProperty, (object) "ScreenModIoConsent_ButtonAgree");
      this.e_28 = new Button();
      this.e_6.Children.Add((UIElement) this.e_28);
      this.e_28.Name = "e_28";
      this.e_28.Margin = new Thickness(20f, 10f, 0.0f, 10f);
      this.e_28.TabIndex = 1;
      Grid.SetColumn((UIElement) this.e_28, 1);
      Grid.SetRow((UIElement) this.e_28, 6);
      GamepadHelp.SetTabIndexLeft((DependencyObject) this.e_28, 0);
      GamepadHelp.SetTabIndexDown((DependencyObject) this.e_28, 2);
      this.e_28.SetBinding(Button.CommandProperty, new Binding("OptOutCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_28.SetResourceReference(ContentControl.ContentProperty, (object) "ScreenModIoConsent_ButtonOptOut");
      ImageManager.Instance.AddImage("Textures\\GUI\\Screens\\screen_background.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol_highlight.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\HUD 2017\\Notification_badge.png");
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "Width", typeof (MyModIoConsentViewModel_Width_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "MaxWidth", typeof (MyModIoConsentViewModel_MaxWidth_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "BackgroundOverlay", typeof (MyModIoConsentViewModel_BackgroundOverlay_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "ExitCommand", typeof (MyModIoConsentViewModel_ExitCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "ConsentCaption", typeof (MyModIoConsentViewModel_ConsentCaption_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "ConsentTextPart1", typeof (MyModIoConsentViewModel_ConsentTextPart1_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "ConsentTextPart2", typeof (MyModIoConsentViewModel_ConsentTextPart2_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "ConsentTextPart3", typeof (MyModIoConsentViewModel_ConsentTextPart3_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "WarningVisible", typeof (MyModIoConsentViewModel_WarningVisible_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "ModioTermsOfUseCommand", typeof (MyModIoConsentViewModel_ModioTermsOfUseCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "ModioTOURequired", typeof (MyModIoConsentViewModel_ModioTOURequired_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "ModioPrivacyPolicyCommand", typeof (MyModIoConsentViewModel_ModioPrivacyPolicyCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "SteamControls", typeof (MyModIoConsentViewModel_SteamControls_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "SteamTermsOfUseCommand", typeof (MyModIoConsentViewModel_SteamTermsOfUseCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "SteamTOURequired", typeof (MyModIoConsentViewModel_SteamTOURequired_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "SteamPrivacyPolicyCommand", typeof (MyModIoConsentViewModel_SteamPrivacyPolicyCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "AgreeButtonEnabled", typeof (MyModIoConsentViewModel_AgreeButtonEnabled_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "AgreeCommand", typeof (MyModIoConsentViewModel_AgreeCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyModIoConsentViewModel), "OptOutCommand", typeof (MyModIoConsentViewModel_OptOutCommand_PropertyInfo));
    }

    private static void InitializeElementResources(UIElement elem) => elem.Resources.MergedDictionaries.Add((ResourceDictionary) Styles.Instance);
  }
}
