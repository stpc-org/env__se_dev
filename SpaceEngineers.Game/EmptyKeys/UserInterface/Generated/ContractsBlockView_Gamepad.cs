// Decompiled with JetBrains decompiler
// Type: EmptyKeys.UserInterface.Generated.ContractsBlockView_Gamepad
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Controls.Primitives;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.ContractsBlockView_Gamepad_Bindings;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Themes;
using Sandbox.Game.Screens.ViewModels;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.ObjectModel;

namespace EmptyKeys.UserInterface.Generated
{
  [GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
  public class ContractsBlockView_Gamepad : UIRoot
  {
    private Grid rootGrid;
    private Image e_0;
    private Grid e_1;
    private ImageButton e_2;
    private StackPanel e_3;
    private TextBlock e_4;
    private Border e_5;
    private TextBlock e_6;
    private TextBlock e_7;
    private TabControl e_8;
    private Grid e_106;
    private Grid e_107;
    private Grid e_108;
    private Image e_109;
    private Grid e_110;
    private TextBlock e_111;
    private Border e_112;
    private TextBlock e_113;
    private ComboBox e_114;
    private Grid AdminSelectionHelp;
    private TextBlock e_115;
    private TextBlock e_116;
    private TextBlock e_117;
    private ImageButton e_118;
    private Grid e_119;
    private Grid e_120;
    private Grid e_121;
    private Image e_122;
    private Grid e_123;
    private TextBlock e_124;
    private Border e_125;
    private TextBlock e_126;
    private ComboBox e_127;
    private Grid GridSelectionHelp;
    private TextBlock e_128;
    private TextBlock e_129;
    private TextBlock e_130;
    private ImageButton e_131;

    public ContractsBlockView_Gamepad() => this.Initialize();

    public ContractsBlockView_Gamepad(int width, int height)
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
      ContractsBlockView_Gamepad.InitializeElementResources((UIElement) this);
      this.rootGrid = new Grid();
      this.Content = (object) this.rootGrid;
      this.rootGrid.Name = "rootGrid";
      this.rootGrid.HorizontalAlignment = HorizontalAlignment.Center;
      this.rootGrid.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(0.1f, GridUnitType.Star)
      });
      this.rootGrid.RowDefinitions.Add(new RowDefinition());
      this.rootGrid.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(0.1f, GridUnitType.Star)
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
      this.e_2.IsTabStop = false;
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
      this.e_4.SetResourceReference(TextBlock.TextProperty, (object) "ScreenCaptionContracts");
      this.e_5 = new Border();
      this.e_3.Children.Add((UIElement) this.e_5);
      this.e_5.Name = "e_5";
      this.e_5.Height = 2f;
      this.e_5.Margin = new Thickness(0.0f, 10f, 0.0f, 10f);
      this.e_5.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      this.e_6 = new TextBlock();
      this.e_1.Children.Add((UIElement) this.e_6);
      this.e_6.Name = "e_6";
      this.e_6.Margin = new Thickness(0.0f, 5f, 10f, 0.0f);
      this.e_6.HorizontalAlignment = HorizontalAlignment.Right;
      this.e_6.VerticalAlignment = VerticalAlignment.Top;
      this.e_6.FontFamily = new FontFamily("LargeFont");
      this.e_6.FontSize = 16.6f;
      Grid.SetRow((UIElement) this.e_6, 3);
      this.e_6.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_TabControl_Left");
      this.e_7 = new TextBlock();
      this.e_1.Children.Add((UIElement) this.e_7);
      this.e_7.Name = "e_7";
      this.e_7.Margin = new Thickness(10f, 5f, 0.0f, 0.0f);
      this.e_7.HorizontalAlignment = HorizontalAlignment.Left;
      this.e_7.VerticalAlignment = VerticalAlignment.Top;
      this.e_7.FontFamily = new FontFamily("LargeFont");
      this.e_7.FontSize = 16.6f;
      Grid.SetColumn((UIElement) this.e_7, 2);
      Grid.SetRow((UIElement) this.e_7, 3);
      this.e_7.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_TabControl_Right");
      this.e_8 = new TabControl();
      this.e_1.Children.Add((UIElement) this.e_8);
      this.e_8.Name = "e_8";
      this.e_8.IsTabStop = false;
      this.e_8.ItemsSource = (IEnumerable) ContractsBlockView_Gamepad.Get_e_8_Items();
      Grid.SetColumn((UIElement) this.e_8, 1);
      Grid.SetRow((UIElement) this.e_8, 3);
      this.e_106 = new Grid();
      this.e_1.Children.Add((UIElement) this.e_106);
      this.e_106.Name = "e_106";
      this.e_106.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_106.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) this.e_106, 1);
      Grid.SetRow((UIElement) this.e_106, 3);
      this.e_106.SetBinding(UIElement.DataContextProperty, new Binding("AdministrationViewModel")
      {
        UseGeneratedBindings = true
      });
      this.e_107 = new Grid();
      this.e_106.Children.Add((UIElement) this.e_107);
      this.e_107.Name = "e_107";
      this.e_108 = new Grid();
      this.e_107.Children.Add((UIElement) this.e_108);
      this.e_108.Name = "e_108";
      GamepadBinding gamepadBinding1 = new GamepadBinding();
      gamepadBinding1.Gesture = (InputGesture) new GamepadGesture(GamepadInput.BButton);
      Binding binding1 = new Binding("AdminSelectionExitCommand");
      gamepadBinding1.SetBinding(InputBinding.CommandProperty, binding1);
      this.e_108.InputBindings.Add((InputBinding) gamepadBinding1);
      gamepadBinding1.Parent = (UIElement) this.e_108;
      GamepadBinding gamepadBinding2 = new GamepadBinding();
      gamepadBinding2.Gesture = (InputGesture) new GamepadGesture(GamepadInput.CButton);
      Binding binding2 = new Binding("AdminSelectionConfirmCommand");
      gamepadBinding2.SetBinding(InputBinding.CommandProperty, binding2);
      this.e_108.InputBindings.Add((InputBinding) gamepadBinding2);
      gamepadBinding2.Parent = (UIElement) this.e_108;
      KeyBinding keyBinding1 = new KeyBinding();
      keyBinding1.Gesture = (InputGesture) new KeyGesture(KeyCode.Escape, ModifierKeys.None, "");
      Binding binding3 = new Binding("AdminSelectionExitCommand");
      keyBinding1.SetBinding(InputBinding.CommandProperty, binding3);
      this.e_108.InputBindings.Add((InputBinding) keyBinding1);
      keyBinding1.Parent = (UIElement) this.e_108;
      this.e_108.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(0.5f, GridUnitType.Star)
      });
      this.e_108.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(2f, GridUnitType.Star)
      });
      this.e_108.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Star)
      });
      this.e_108.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      this.e_108.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(3f, GridUnitType.Star)
      });
      this.e_108.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      Binding binding4 = new Binding("IsVisibleAdminSelection");
      this.e_108.SetBinding(UIElement.VisibilityProperty, binding4);
      this.e_109 = new Image();
      this.e_108.Children.Add((UIElement) this.e_109);
      this.e_109.Name = "e_109";
      this.e_109.Source = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Screens\\screen_background.dds"
      };
      this.e_109.Stretch = Stretch.Fill;
      Grid.SetColumn((UIElement) this.e_109, 1);
      Grid.SetRow((UIElement) this.e_109, 1);
      Binding binding5 = new Binding("BackgroundOverlay");
      this.e_109.SetBinding(ImageBrush.ColorOverlayProperty, binding5);
      this.e_110 = new Grid();
      this.e_108.Children.Add((UIElement) this.e_110);
      this.e_110.Name = "e_110";
      this.e_110.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_110.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_110.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_110.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(2.5f, GridUnitType.Star)
      });
      this.e_110.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_110.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_110.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Star)
      });
      this.e_110.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      this.e_110.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(12f, GridUnitType.Star)
      });
      this.e_110.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      Grid.SetColumn((UIElement) this.e_110, 1);
      Grid.SetRow((UIElement) this.e_110, 1);
      this.e_111 = new TextBlock();
      this.e_110.Children.Add((UIElement) this.e_111);
      this.e_111.Name = "e_111";
      this.e_111.Margin = new Thickness(2f, 2f, 2f, 2f);
      this.e_111.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_111.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) this.e_111, 1);
      Grid.SetRow((UIElement) this.e_111, 1);
      Binding binding6 = new Binding("AdminSelectionCaption");
      this.e_111.SetBinding(TextBlock.TextProperty, binding6);
      this.e_112 = new Border();
      this.e_110.Children.Add((UIElement) this.e_112);
      this.e_112.Name = "e_112";
      this.e_112.Height = 2f;
      this.e_112.Margin = new Thickness(10f, 10f, 10f, 10f);
      this.e_112.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) this.e_112, 1);
      Grid.SetRow((UIElement) this.e_112, 2);
      this.e_113 = new TextBlock();
      this.e_110.Children.Add((UIElement) this.e_113);
      this.e_113.Name = "e_113";
      this.e_113.Margin = new Thickness(10f, 10f, 10f, 10f);
      this.e_113.HorizontalAlignment = HorizontalAlignment.Left;
      this.e_113.VerticalAlignment = VerticalAlignment.Center;
      this.e_113.TextWrapping = TextWrapping.Wrap;
      Grid.SetColumn((UIElement) this.e_113, 1);
      Grid.SetRow((UIElement) this.e_113, 3);
      Binding binding7 = new Binding("AdminSelectionText");
      this.e_113.SetBinding(TextBlock.TextProperty, binding7);
      this.e_114 = new ComboBox();
      this.e_110.Children.Add((UIElement) this.e_114);
      this.e_114.Name = "e_114";
      this.e_114.Margin = new Thickness(10f, 10f, 10f, 10f);
      this.e_114.TabIndex = 200;
      Grid.SetColumn((UIElement) this.e_114, 1);
      Grid.SetRow((UIElement) this.e_114, 4);
      Binding binding8 = new Binding("AdminSelectionItems");
      this.e_114.SetBinding(ItemsControl.ItemsSourceProperty, binding8);
      Binding binding9 = new Binding("AdminSelectedItemIndex");
      this.e_114.SetBinding(Selector.SelectedIndexProperty, binding9);
      this.AdminSelectionHelp = new Grid();
      this.e_110.Children.Add((UIElement) this.AdminSelectionHelp);
      this.AdminSelectionHelp.Name = "AdminSelectionHelp";
      this.AdminSelectionHelp.Margin = new Thickness(10f, 10f, 10f, 10f);
      this.AdminSelectionHelp.VerticalAlignment = VerticalAlignment.Center;
      this.AdminSelectionHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.AdminSelectionHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.AdminSelectionHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.AdminSelectionHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.AdminSelectionHelp.ColumnDefinitions.Add(new ColumnDefinition());
      Grid.SetColumn((UIElement) this.AdminSelectionHelp, 1);
      Grid.SetRow((UIElement) this.AdminSelectionHelp, 5);
      this.e_115 = new TextBlock();
      this.AdminSelectionHelp.Children.Add((UIElement) this.e_115);
      this.e_115.Name = "e_115";
      this.e_115.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      this.e_115.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_115.VerticalAlignment = VerticalAlignment.Center;
      this.e_115.SetResourceReference(TextBlock.TextProperty, (object) "ContractsScreen_Help_Select");
      this.e_116 = new TextBlock();
      this.AdminSelectionHelp.Children.Add((UIElement) this.e_116);
      this.e_116.Name = "e_116";
      this.e_116.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      this.e_116.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_116.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) this.e_116, 1);
      this.e_116.SetResourceReference(TextBlock.TextProperty, (object) "ContractsScreenGridSelection_Help_Confirm");
      this.e_117 = new TextBlock();
      this.AdminSelectionHelp.Children.Add((UIElement) this.e_117);
      this.e_117.Name = "e_117";
      this.e_117.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      this.e_117.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_117.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) this.e_117, 3);
      this.e_117.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Back");
      this.e_118 = new ImageButton();
      this.e_110.Children.Add((UIElement) this.e_118);
      this.e_118.Name = "e_118";
      this.e_118.Height = 24f;
      this.e_118.Width = 24f;
      this.e_118.Margin = new Thickness(16f, 16f, 16f, 16f);
      this.e_118.HorizontalAlignment = HorizontalAlignment.Right;
      this.e_118.VerticalAlignment = VerticalAlignment.Center;
      this.e_118.IsTabStop = false;
      this.e_118.ImageStretch = Stretch.Uniform;
      this.e_118.ImageNormal = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol.dds"
      };
      this.e_118.ImageHover = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds"
      };
      this.e_118.ImagePressed = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds"
      };
      Grid.SetColumn((UIElement) this.e_118, 2);
      Binding binding10 = new Binding("AdminSelectionExitCommand");
      this.e_118.SetBinding(Button.CommandProperty, binding10);
      this.e_119 = new Grid();
      this.e_1.Children.Add((UIElement) this.e_119);
      this.e_119.Name = "e_119";
      this.e_119.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_119.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) this.e_119, 1);
      Grid.SetRow((UIElement) this.e_119, 3);
      this.e_119.SetBinding(UIElement.DataContextProperty, new Binding("ActiveViewModel")
      {
        UseGeneratedBindings = true
      });
      this.e_120 = new Grid();
      this.e_119.Children.Add((UIElement) this.e_120);
      this.e_120.Name = "e_120";
      this.e_121 = new Grid();
      this.e_120.Children.Add((UIElement) this.e_121);
      this.e_121.Name = "e_121";
      GamepadBinding gamepadBinding3 = new GamepadBinding();
      gamepadBinding3.Gesture = (InputGesture) new GamepadGesture(GamepadInput.BButton);
      Binding binding11 = new Binding("ExitGridSelectionCommand");
      gamepadBinding3.SetBinding(InputBinding.CommandProperty, binding11);
      this.e_121.InputBindings.Add((InputBinding) gamepadBinding3);
      gamepadBinding3.Parent = (UIElement) this.e_121;
      GamepadBinding gamepadBinding4 = new GamepadBinding();
      gamepadBinding4.Gesture = (InputGesture) new GamepadGesture(GamepadInput.CButton);
      Binding binding12 = new Binding("ConfirmGridSelectionCommand");
      gamepadBinding4.SetBinding(InputBinding.CommandProperty, binding12);
      this.e_121.InputBindings.Add((InputBinding) gamepadBinding4);
      gamepadBinding4.Parent = (UIElement) this.e_121;
      KeyBinding keyBinding2 = new KeyBinding();
      keyBinding2.Gesture = (InputGesture) new KeyGesture(KeyCode.Escape, ModifierKeys.None, "");
      Binding binding13 = new Binding("ExitGridSelectionCommand");
      keyBinding2.SetBinding(InputBinding.CommandProperty, binding13);
      this.e_121.InputBindings.Add((InputBinding) keyBinding2);
      keyBinding2.Parent = (UIElement) this.e_121;
      this.e_121.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(0.5f, GridUnitType.Star)
      });
      this.e_121.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(2f, GridUnitType.Star)
      });
      this.e_121.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Star)
      });
      this.e_121.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      this.e_121.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(3f, GridUnitType.Star)
      });
      this.e_121.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      Binding binding14 = new Binding("IsVisibleGridSelection");
      this.e_121.SetBinding(UIElement.VisibilityProperty, binding14);
      this.e_122 = new Image();
      this.e_121.Children.Add((UIElement) this.e_122);
      this.e_122.Name = "e_122";
      this.e_122.Source = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Screens\\screen_background.dds"
      };
      this.e_122.Stretch = Stretch.Fill;
      Grid.SetColumn((UIElement) this.e_122, 1);
      Grid.SetRow((UIElement) this.e_122, 1);
      ImageBrush.SetColorOverlay((DependencyObject) this.e_122, new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      this.e_123 = new Grid();
      this.e_121.Children.Add((UIElement) this.e_123);
      this.e_123.Name = "e_123";
      this.e_123.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_123.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_123.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_123.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(2.5f, GridUnitType.Star)
      });
      this.e_123.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_123.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_123.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Star)
      });
      this.e_123.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      this.e_123.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(12f, GridUnitType.Star)
      });
      this.e_123.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      Grid.SetColumn((UIElement) this.e_123, 1);
      Grid.SetRow((UIElement) this.e_123, 1);
      this.e_124 = new TextBlock();
      this.e_123.Children.Add((UIElement) this.e_124);
      this.e_124.Name = "e_124";
      this.e_124.Margin = new Thickness(2f, 2f, 2f, 2f);
      this.e_124.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_124.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) this.e_124, 1);
      Grid.SetRow((UIElement) this.e_124, 1);
      this.e_124.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_GridSelection_Caption");
      this.e_125 = new Border();
      this.e_123.Children.Add((UIElement) this.e_125);
      this.e_125.Name = "e_125";
      this.e_125.Height = 2f;
      this.e_125.Margin = new Thickness(10f, 10f, 10f, 10f);
      this.e_125.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) this.e_125, 1);
      Grid.SetRow((UIElement) this.e_125, 2);
      this.e_126 = new TextBlock();
      this.e_123.Children.Add((UIElement) this.e_126);
      this.e_126.Name = "e_126";
      this.e_126.Margin = new Thickness(10f, 10f, 10f, 10f);
      this.e_126.HorizontalAlignment = HorizontalAlignment.Left;
      this.e_126.VerticalAlignment = VerticalAlignment.Center;
      this.e_126.TextWrapping = TextWrapping.Wrap;
      Grid.SetColumn((UIElement) this.e_126, 1);
      Grid.SetRow((UIElement) this.e_126, 3);
      this.e_126.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_GridSelection_Text");
      this.e_127 = new ComboBox();
      this.e_123.Children.Add((UIElement) this.e_127);
      this.e_127.Name = "e_127";
      this.e_127.Width = float.NaN;
      this.e_127.Margin = new Thickness(10f, 10f, 10f, 10f);
      this.e_127.HorizontalAlignment = HorizontalAlignment.Stretch;
      this.e_127.TabIndex = 500;
      Grid.SetColumn((UIElement) this.e_127, 1);
      Grid.SetRow((UIElement) this.e_127, 4);
      Binding binding15 = new Binding("SelectableTargets");
      this.e_127.SetBinding(ItemsControl.ItemsSourceProperty, binding15);
      Binding binding16 = new Binding("SelectedTargetIndex");
      this.e_127.SetBinding(Selector.SelectedIndexProperty, binding16);
      this.GridSelectionHelp = new Grid();
      this.e_123.Children.Add((UIElement) this.GridSelectionHelp);
      this.GridSelectionHelp.Name = "GridSelectionHelp";
      this.GridSelectionHelp.Margin = new Thickness(10f, 10f, 10f, 10f);
      this.GridSelectionHelp.VerticalAlignment = VerticalAlignment.Center;
      this.GridSelectionHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.GridSelectionHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.GridSelectionHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.GridSelectionHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.GridSelectionHelp.ColumnDefinitions.Add(new ColumnDefinition());
      Grid.SetColumn((UIElement) this.GridSelectionHelp, 1);
      Grid.SetRow((UIElement) this.GridSelectionHelp, 5);
      this.e_128 = new TextBlock();
      this.GridSelectionHelp.Children.Add((UIElement) this.e_128);
      this.e_128.Name = "e_128";
      this.e_128.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      this.e_128.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_128.VerticalAlignment = VerticalAlignment.Center;
      this.e_128.SetResourceReference(TextBlock.TextProperty, (object) "ContractsScreen_Help_Select");
      this.e_129 = new TextBlock();
      this.GridSelectionHelp.Children.Add((UIElement) this.e_129);
      this.e_129.Name = "e_129";
      this.e_129.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      this.e_129.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_129.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) this.e_129, 1);
      this.e_129.SetResourceReference(TextBlock.TextProperty, (object) "ContractsScreenGridSelection_Help_Confirm");
      this.e_130 = new TextBlock();
      this.GridSelectionHelp.Children.Add((UIElement) this.e_130);
      this.e_130.Name = "e_130";
      this.e_130.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      this.e_130.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_130.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) this.e_130, 3);
      this.e_130.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Back");
      this.e_131 = new ImageButton();
      this.e_123.Children.Add((UIElement) this.e_131);
      this.e_131.Name = "e_131";
      this.e_131.Height = 24f;
      this.e_131.Width = 24f;
      this.e_131.Margin = new Thickness(16f, 16f, 16f, 16f);
      this.e_131.HorizontalAlignment = HorizontalAlignment.Right;
      this.e_131.VerticalAlignment = VerticalAlignment.Center;
      this.e_131.IsTabStop = false;
      this.e_131.ImageStretch = Stretch.Uniform;
      this.e_131.ImageNormal = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol.dds"
      };
      this.e_131.ImageHover = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds"
      };
      this.e_131.ImagePressed = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds"
      };
      Grid.SetColumn((UIElement) this.e_131, 2);
      Binding binding17 = new Binding("ExitGridSelectionCommand");
      this.e_131.SetBinding(Button.CommandProperty, binding17);
      ImageManager.Instance.AddImage("Textures\\GUI\\Screens\\screen_background.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol_highlight.dds");
      FontManager.Instance.AddFont("LargeFont", 16.6f, FontStyle.Regular, "LargeFont_12.45_Regular");
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsBlockViewModel), "MaxWidth", typeof (MyContractsBlockViewModel_MaxWidth_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsBlockViewModel), "BackgroundOverlay", typeof (MyContractsBlockViewModel_BackgroundOverlay_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsBlockViewModel), "ExitCommand", typeof (MyContractsBlockViewModel_ExitCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsBlockViewModel), "RefreshAvailableCommand", typeof (MyContractsBlockViewModel_RefreshAvailableCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsBlockViewModel), "FilterTargets", typeof (MyContractsBlockViewModel_FilterTargets_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsBlockViewModel), "FilterTargetIndex", typeof (MyContractsBlockViewModel_FilterTargetIndex_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsBlockViewModel), "SelectedFilterTarget", typeof (MyContractsBlockViewModel_SelectedFilterTarget_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsBlockViewModel), "AcceptCommand", typeof (MyContractsBlockViewModel_AcceptCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsBlockViewModel), "AvailableContracts", typeof (MyContractsBlockViewModel_AvailableContracts_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsBlockViewModel), "SelectedAvailableContractIndex", typeof (MyContractsBlockViewModel_SelectedAvailableContractIndex_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsBlockViewModel), "SelectedAvailableContract", typeof (MyContractsBlockViewModel_SelectedAvailableContract_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsBlockViewModel), "IsNoAvailableContractVisible", typeof (MyContractsBlockViewModel_IsNoAvailableContractVisible_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsBlockViewModel), "IsAcceptEnabled", typeof (MyContractsBlockViewModel_IsAcceptEnabled_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsBlockViewModel), "ActiveViewModel", typeof (MyContractsBlockViewModel_ActiveViewModel_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsBlockViewModel), "IsAdministrationVisible", typeof (MyContractsBlockViewModel_IsAdministrationVisible_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsBlockViewModel), "AdministrationViewModel", typeof (MyContractsBlockViewModel_AdministrationViewModel_PropertyInfo));
    }

    private static void InitializeElementResources(UIElement elem)
    {
      elem.Resources.MergedDictionaries.Add((ResourceDictionary) Styles.Instance);
      elem.Resources.MergedDictionaries.Add((ResourceDictionary) DataTemplatesContracts.Instance);
    }

    private static ObservableCollection<object> Get_e_8_Items()
    {
      ObservableCollection<object> observableCollection = new ObservableCollection<object>();
      TabItem tabItem1 = new TabItem();
      tabItem1.Name = "e_9";
      tabItem1.IsTabStop = false;
      tabItem1.SetResourceReference(HeaderedContentControl.HeaderProperty, (object) "ContractScreen_Tab_AvailableContracts");
      Grid grid1 = new Grid();
      tabItem1.Content = (object) grid1;
      grid1.Name = "e_10";
      GamepadBinding gamepadBinding1 = new GamepadBinding();
      gamepadBinding1.Gesture = (InputGesture) new GamepadGesture(GamepadInput.DButton);
      gamepadBinding1.SetBinding(InputBinding.CommandProperty, new Binding("RefreshAvailableCommand")
      {
        UseGeneratedBindings = true
      });
      grid1.InputBindings.Add((InputBinding) gamepadBinding1);
      gamepadBinding1.Parent = (UIElement) grid1;
      RowDefinition rowDefinition1 = new RowDefinition();
      grid1.RowDefinitions.Add(rowDefinition1);
      grid1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(65f, GridUnitType.Pixel)
      });
      grid1.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(2f, GridUnitType.Star)
      });
      grid1.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(3f, GridUnitType.Star)
      });
      Grid grid2 = new Grid();
      grid1.Children.Add((UIElement) grid2);
      grid2.Name = "e_11";
      grid2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      RowDefinition rowDefinition2 = new RowDefinition();
      grid2.RowDefinitions.Add(rowDefinition2);
      Grid grid3 = new Grid();
      grid2.Children.Add((UIElement) grid3);
      grid3.Name = "e_12";
      grid3.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition1 = new ColumnDefinition();
      grid3.ColumnDefinitions.Add(columnDefinition1);
      TextBlock textBlock1 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_13";
      textBlock1.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock1, 0);
      textBlock1.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_ContractFilterTitle");
      ComboBox comboBox1 = new ComboBox();
      grid3.Children.Add((UIElement) comboBox1);
      comboBox1.Name = "e_14";
      comboBox1.Margin = new Thickness(5f, 10f, 5f, 10f);
      comboBox1.VerticalAlignment = VerticalAlignment.Center;
      comboBox1.TabIndex = 0;
      comboBox1.MaxDropDownHeight = 240f;
      Grid.SetColumn((UIElement) comboBox1, 1);
      GamepadHelp.SetTargetName((DependencyObject) comboBox1, "SelectTypeHelp");
      GamepadHelp.SetTabIndexDown((DependencyObject) comboBox1, 1);
      comboBox1.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("FilterTargets")
      {
        UseGeneratedBindings = true
      });
      comboBox1.SetBinding(Selector.SelectedIndexProperty, new Binding("FilterTargetIndex")
      {
        UseGeneratedBindings = true
      });
      comboBox1.SetBinding(Selector.SelectedItemProperty, new Binding("SelectedFilterTarget")
      {
        UseGeneratedBindings = true
      });
      Grid grid4 = new Grid();
      grid2.Children.Add((UIElement) grid4);
      grid4.Name = "e_15";
      Grid.SetRow((UIElement) grid4, 1);
      ListBox listBox1 = new ListBox();
      grid4.Children.Add((UIElement) listBox1);
      listBox1.Name = "e_16";
      listBox1.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      GamepadBinding gamepadBinding2 = new GamepadBinding();
      gamepadBinding2.Gesture = (InputGesture) new GamepadGesture(GamepadInput.AButton);
      gamepadBinding2.SetBinding(InputBinding.CommandProperty, new Binding("AcceptCommand")
      {
        UseGeneratedBindings = true
      });
      listBox1.InputBindings.Add((InputBinding) gamepadBinding2);
      gamepadBinding2.Parent = (UIElement) listBox1;
      listBox1.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      listBox1.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      listBox1.TabIndex = 1;
      listBox1.SelectionMode = SelectionMode.Single;
      GamepadHelp.SetTargetName((DependencyObject) listBox1, "ContractsHelp");
      GamepadHelp.SetTabIndexUp((DependencyObject) listBox1, 0);
      listBox1.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("AvailableContracts")
      {
        UseGeneratedBindings = true
      });
      listBox1.SetBinding(Selector.SelectedIndexProperty, new Binding("SelectedAvailableContractIndex")
      {
        UseGeneratedBindings = true
      });
      listBox1.SetBinding(Selector.SelectedItemProperty, new Binding("SelectedAvailableContract")
      {
        UseGeneratedBindings = true
      });
      ContractsBlockView_Gamepad.InitializeElemente_16Resources((UIElement) listBox1);
      Grid grid5 = new Grid();
      grid4.Children.Add((UIElement) grid5);
      grid5.Name = "e_17";
      grid5.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(30f, GridUnitType.Pixel)
      });
      grid5.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid5.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Star)
      });
      grid5.SetBinding(UIElement.VisibilityProperty, new Binding("IsNoAvailableContractVisible")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock2 = new TextBlock();
      grid5.Children.Add((UIElement) textBlock2);
      textBlock2.Name = "e_18";
      textBlock2.Margin = new Thickness(5f, 5f, 5f, 5f);
      textBlock2.HorizontalAlignment = HorizontalAlignment.Center;
      Grid.SetRow((UIElement) textBlock2, 1);
      textBlock2.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_NoAvailableContracts");
      Border border1 = new Border();
      grid1.Children.Add((UIElement) border1);
      border1.Name = "e_19";
      border1.VerticalAlignment = VerticalAlignment.Stretch;
      border1.Background = (Brush) new SolidColorBrush(new ColorW(33, 44, 53, (int) byte.MaxValue));
      border1.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      border1.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      Grid.SetColumn((UIElement) border1, 1);
      ContentPresenter contentPresenter1 = new ContentPresenter();
      border1.Child = (UIElement) contentPresenter1;
      contentPresenter1.Name = "e_20";
      contentPresenter1.Margin = new Thickness(5f, 0.0f, 0.0f, 0.0f);
      contentPresenter1.SetBinding(UIElement.DataContextProperty, new Binding("SelectedAvailableContract")
      {
        UseGeneratedBindings = true
      });
      contentPresenter1.SetBinding(ContentPresenter.ContentProperty, new Binding("SelectedAvailableContract")
      {
        UseGeneratedBindings = true
      });
      Border border2 = new Border();
      grid1.Children.Add((UIElement) border2);
      border2.Name = "e_21";
      border2.Height = 2f;
      border2.Margin = new Thickness(0.0f, 10f, 0.0f, 10f);
      border2.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      Grid.SetRow((UIElement) border2, 1);
      Grid.SetColumnSpan((UIElement) border2, 2);
      Grid grid6 = new Grid();
      grid1.Children.Add((UIElement) grid6);
      grid6.Name = "ContractsHelp";
      grid6.Visibility = Visibility.Collapsed;
      grid6.VerticalAlignment = VerticalAlignment.Center;
      grid6.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid6.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid6.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition2 = new ColumnDefinition();
      grid6.ColumnDefinitions.Add(columnDefinition2);
      Grid.SetRow((UIElement) grid6, 2);
      Grid.SetColumnSpan((UIElement) grid6, 2);
      TextBlock textBlock3 = new TextBlock();
      grid6.Children.Add((UIElement) textBlock3);
      textBlock3.Name = "e_22";
      textBlock3.Margin = new Thickness(0.0f, 0.0f, 15f, 0.0f);
      textBlock3.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock3.VerticalAlignment = VerticalAlignment.Center;
      textBlock3.SetBinding(UIElement.VisibilityProperty, new Binding("IsAcceptEnabled")
      {
        UseGeneratedBindings = true
      });
      textBlock3.SetResourceReference(TextBlock.TextProperty, (object) "ContractsScreen_Help_Accept");
      TextBlock textBlock4 = new TextBlock();
      grid6.Children.Add((UIElement) textBlock4);
      textBlock4.Name = "e_23";
      textBlock4.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      textBlock4.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock4.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock4, 1);
      textBlock4.SetResourceReference(TextBlock.TextProperty, (object) "ContractsScreen_Help_Refresh");
      TextBlock textBlock5 = new TextBlock();
      grid6.Children.Add((UIElement) textBlock5);
      textBlock5.Name = "e_24";
      textBlock5.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock5.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock5.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock5, 2);
      textBlock5.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Back");
      Grid grid7 = new Grid();
      grid1.Children.Add((UIElement) grid7);
      grid7.Name = "SelectTypeHelp";
      grid7.Visibility = Visibility.Collapsed;
      grid7.VerticalAlignment = VerticalAlignment.Center;
      grid7.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid7.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid7.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition3 = new ColumnDefinition();
      grid7.ColumnDefinitions.Add(columnDefinition3);
      Grid.SetRow((UIElement) grid7, 2);
      Grid.SetColumnSpan((UIElement) grid7, 2);
      TextBlock textBlock6 = new TextBlock();
      grid7.Children.Add((UIElement) textBlock6);
      textBlock6.Name = "e_25";
      textBlock6.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      textBlock6.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock6.VerticalAlignment = VerticalAlignment.Center;
      textBlock6.SetResourceReference(TextBlock.TextProperty, (object) "ContractsScreen_Help_Select");
      TextBlock textBlock7 = new TextBlock();
      grid7.Children.Add((UIElement) textBlock7);
      textBlock7.Name = "e_26";
      textBlock7.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock7.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock7.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock7, 1);
      textBlock7.SetResourceReference(TextBlock.TextProperty, (object) "ContractsScreen_Help_Refresh");
      TextBlock textBlock8 = new TextBlock();
      grid7.Children.Add((UIElement) textBlock8);
      textBlock8.Name = "e_27";
      textBlock8.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock8.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock8.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock8, 2);
      textBlock8.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Back");
      observableCollection.Add((object) tabItem1);
      TabItem tabItem2 = new TabItem();
      tabItem2.Name = "e_28";
      tabItem2.IsTabStop = false;
      tabItem2.SetResourceReference(HeaderedContentControl.HeaderProperty, (object) "ContractScreen_Tab_AcceptedContracts");
      Grid grid8 = new Grid();
      tabItem2.Content = (object) grid8;
      grid8.Name = "e_29";
      grid8.SetBinding(UIElement.DataContextProperty, new Binding("ActiveViewModel")
      {
        UseGeneratedBindings = true
      });
      Grid grid9 = new Grid();
      grid8.Children.Add((UIElement) grid9);
      grid9.Name = "e_30";
      Grid grid10 = new Grid();
      grid9.Children.Add((UIElement) grid10);
      grid10.Name = "e_31";
      GamepadBinding gamepadBinding3 = new GamepadBinding();
      gamepadBinding3.Gesture = (InputGesture) new GamepadGesture(GamepadInput.AButton);
      Binding binding1 = new Binding("AbandonCommand");
      gamepadBinding3.SetBinding(InputBinding.CommandProperty, binding1);
      grid10.InputBindings.Add((InputBinding) gamepadBinding3);
      gamepadBinding3.Parent = (UIElement) grid10;
      GamepadBinding gamepadBinding4 = new GamepadBinding();
      gamepadBinding4.Gesture = (InputGesture) new GamepadGesture(GamepadInput.CButton);
      Binding binding2 = new Binding("FinishCommand");
      gamepadBinding4.SetBinding(InputBinding.CommandProperty, binding2);
      grid10.InputBindings.Add((InputBinding) gamepadBinding4);
      gamepadBinding4.Parent = (UIElement) grid10;
      GamepadBinding gamepadBinding5 = new GamepadBinding();
      gamepadBinding5.Gesture = (InputGesture) new GamepadGesture(GamepadInput.DButton);
      Binding binding3 = new Binding("RefreshActiveCommand");
      gamepadBinding5.SetBinding(InputBinding.CommandProperty, binding3);
      grid10.InputBindings.Add((InputBinding) gamepadBinding5);
      gamepadBinding5.Parent = (UIElement) grid10;
      RowDefinition rowDefinition3 = new RowDefinition();
      grid10.RowDefinitions.Add(rowDefinition3);
      grid10.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid10.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(65f, GridUnitType.Pixel)
      });
      grid10.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(2f, GridUnitType.Star)
      });
      grid10.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(3f, GridUnitType.Star)
      });
      ListBox listBox2 = new ListBox();
      grid10.Children.Add((UIElement) listBox2);
      listBox2.Name = "e_32";
      listBox2.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      listBox2.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      listBox2.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      listBox2.TabIndex = 50;
      listBox2.SelectionMode = SelectionMode.Single;
      GamepadHelp.SetTargetName((DependencyObject) listBox2, "ActiveContractsHelp");
      Binding binding4 = new Binding("ActiveContracts");
      listBox2.SetBinding(ItemsControl.ItemsSourceProperty, binding4);
      Binding binding5 = new Binding("SelectedActiveContractIndex");
      listBox2.SetBinding(Selector.SelectedIndexProperty, binding5);
      Binding binding6 = new Binding("SelectedActiveContract");
      listBox2.SetBinding(Selector.SelectedItemProperty, binding6);
      ContractsBlockView_Gamepad.InitializeElemente_32Resources((UIElement) listBox2);
      Grid grid11 = new Grid();
      grid10.Children.Add((UIElement) grid11);
      grid11.Name = "e_33";
      grid11.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(30f, GridUnitType.Pixel)
      });
      grid11.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid11.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Star)
      });
      Grid.SetColumn((UIElement) grid11, 0);
      Grid.SetRow((UIElement) grid11, 0);
      Binding binding7 = new Binding("IsNoActiveContractVisible");
      grid11.SetBinding(UIElement.VisibilityProperty, binding7);
      TextBlock textBlock9 = new TextBlock();
      grid11.Children.Add((UIElement) textBlock9);
      textBlock9.Name = "e_34";
      textBlock9.Margin = new Thickness(5f, 5f, 5f, 5f);
      textBlock9.HorizontalAlignment = HorizontalAlignment.Center;
      Grid.SetRow((UIElement) textBlock9, 1);
      textBlock9.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_NoActiveContracts");
      Border border3 = new Border();
      grid10.Children.Add((UIElement) border3);
      border3.Name = "e_35";
      border3.VerticalAlignment = VerticalAlignment.Stretch;
      border3.Background = (Brush) new SolidColorBrush(new ColorW(33, 44, 53, (int) byte.MaxValue));
      border3.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      border3.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      Grid.SetColumn((UIElement) border3, 1);
      ContentPresenter contentPresenter2 = new ContentPresenter();
      border3.Child = (UIElement) contentPresenter2;
      contentPresenter2.Name = "e_36";
      contentPresenter2.Margin = new Thickness(5f, 0.0f, 0.0f, 0.0f);
      Grid.SetColumn((UIElement) contentPresenter2, 1);
      Binding binding8 = new Binding("SelectedActiveContract");
      contentPresenter2.SetBinding(UIElement.DataContextProperty, binding8);
      Binding binding9 = new Binding("SelectedActiveContract");
      contentPresenter2.SetBinding(ContentPresenter.ContentProperty, binding9);
      Border border4 = new Border();
      grid10.Children.Add((UIElement) border4);
      border4.Name = "e_37";
      border4.Height = 2f;
      border4.Margin = new Thickness(0.0f, 10f, 0.0f, 10f);
      border4.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      Grid.SetRow((UIElement) border4, 1);
      Grid.SetColumnSpan((UIElement) border4, 2);
      Grid grid12 = new Grid();
      grid10.Children.Add((UIElement) grid12);
      grid12.Name = "ActiveContractsHelp";
      grid12.Visibility = Visibility.Collapsed;
      grid12.VerticalAlignment = VerticalAlignment.Center;
      grid12.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid12.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid12.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid12.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition4 = new ColumnDefinition();
      grid12.ColumnDefinitions.Add(columnDefinition4);
      Grid.SetRow((UIElement) grid12, 2);
      Grid.SetColumnSpan((UIElement) grid12, 2);
      TextBlock textBlock10 = new TextBlock();
      grid12.Children.Add((UIElement) textBlock10);
      textBlock10.Name = "e_38";
      textBlock10.Margin = new Thickness(0.0f, 0.0f, 15f, 0.0f);
      textBlock10.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock10.VerticalAlignment = VerticalAlignment.Center;
      Binding binding10 = new Binding("IsAbandonEnabled");
      textBlock10.SetBinding(UIElement.VisibilityProperty, binding10);
      textBlock10.SetResourceReference(TextBlock.TextProperty, (object) "ActiveContractsScreen_Help_Abandon");
      TextBlock textBlock11 = new TextBlock();
      grid12.Children.Add((UIElement) textBlock11);
      textBlock11.Name = "e_39";
      textBlock11.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      textBlock11.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock11.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock11, 1);
      textBlock11.SetResourceReference(TextBlock.TextProperty, (object) "ActiveContractsScreen_Help_Refresh");
      TextBlock textBlock12 = new TextBlock();
      grid12.Children.Add((UIElement) textBlock12);
      textBlock12.Name = "e_40";
      textBlock12.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock12.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock12.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock12, 2);
      Binding binding11 = new Binding("IsFinishEnabled");
      textBlock12.SetBinding(UIElement.VisibilityProperty, binding11);
      textBlock12.SetResourceReference(TextBlock.TextProperty, (object) "ContractsScreen_Help_Finish");
      TextBlock textBlock13 = new TextBlock();
      grid12.Children.Add((UIElement) textBlock13);
      textBlock13.Name = "e_41";
      textBlock13.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock13.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock13.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock13, 3);
      textBlock13.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Back");
      observableCollection.Add((object) tabItem2);
      TabItem tabItem3 = new TabItem();
      tabItem3.Name = "e_42";
      tabItem3.IsTabStop = false;
      tabItem3.SetBinding(UIElement.VisibilityProperty, new Binding("IsAdministrationVisible")
      {
        UseGeneratedBindings = true
      });
      tabItem3.SetResourceReference(HeaderedContentControl.HeaderProperty, (object) "ContractScreen_Tab_Administration");
      Grid grid13 = new Grid();
      tabItem3.Content = (object) grid13;
      grid13.Name = "e_43";
      grid13.SetBinding(UIElement.DataContextProperty, new Binding("AdministrationViewModel")
      {
        UseGeneratedBindings = true
      });
      Grid grid14 = new Grid();
      grid13.Children.Add((UIElement) grid14);
      grid14.Name = "e_44";
      Grid grid15 = new Grid();
      grid14.Children.Add((UIElement) grid15);
      grid15.Name = "e_45";
      RowDefinition rowDefinition4 = new RowDefinition();
      grid15.RowDefinitions.Add(rowDefinition4);
      grid15.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid15.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(65f, GridUnitType.Pixel)
      });
      grid15.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(2f, GridUnitType.Star)
      });
      grid15.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(3f, GridUnitType.Star)
      });
      ListBox listBox3 = new ListBox();
      grid15.Children.Add((UIElement) listBox3);
      listBox3.Name = "e_46";
      listBox3.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      GamepadBinding gamepadBinding6 = new GamepadBinding();
      gamepadBinding6.Gesture = (InputGesture) new GamepadGesture(GamepadInput.CButton);
      Binding binding12 = new Binding("DeleteCommand");
      gamepadBinding6.SetBinding(InputBinding.CommandProperty, binding12);
      listBox3.InputBindings.Add((InputBinding) gamepadBinding6);
      gamepadBinding6.Parent = (UIElement) listBox3;
      GamepadBinding gamepadBinding7 = new GamepadBinding();
      gamepadBinding7.Gesture = (InputGesture) new GamepadGesture(GamepadInput.DButton);
      Binding binding13 = new Binding("RefreshCommand");
      gamepadBinding7.SetBinding(InputBinding.CommandProperty, binding13);
      listBox3.InputBindings.Add((InputBinding) gamepadBinding7);
      gamepadBinding7.Parent = (UIElement) listBox3;
      listBox3.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      listBox3.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      listBox3.TabIndex = 100;
      listBox3.SelectionMode = SelectionMode.Single;
      GamepadHelp.SetTargetName((DependencyObject) listBox3, "AdminContractsListHelp");
      GamepadHelp.SetTabIndexRight((DependencyObject) listBox3, 103);
      Binding binding14 = new Binding("AdministrableContracts");
      listBox3.SetBinding(ItemsControl.ItemsSourceProperty, binding14);
      Binding binding15 = new Binding("SelectedAdministrableContract");
      listBox3.SetBinding(Selector.SelectedItemProperty, binding15);
      ContractsBlockView_Gamepad.InitializeElemente_46Resources((UIElement) listBox3);
      Grid grid16 = new Grid();
      grid15.Children.Add((UIElement) grid16);
      grid16.Name = "e_47";
      grid16.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(30f, GridUnitType.Pixel)
      });
      grid16.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid16.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Star)
      });
      Binding binding16 = new Binding("IsNoAdministrableContractVisible");
      grid16.SetBinding(UIElement.VisibilityProperty, binding16);
      TextBlock textBlock14 = new TextBlock();
      grid16.Children.Add((UIElement) textBlock14);
      textBlock14.Name = "e_48";
      textBlock14.Margin = new Thickness(5f, 5f, 5f, 5f);
      textBlock14.HorizontalAlignment = HorizontalAlignment.Center;
      Grid.SetRow((UIElement) textBlock14, 1);
      textBlock14.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_NoAdministrableContracts");
      Grid grid17 = new Grid();
      grid15.Children.Add((UIElement) grid17);
      grid17.Name = "AdminContractsListHelp";
      grid17.Visibility = Visibility.Collapsed;
      grid17.VerticalAlignment = VerticalAlignment.Center;
      grid17.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid17.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid17.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition5 = new ColumnDefinition();
      grid17.ColumnDefinitions.Add(columnDefinition5);
      Grid.SetColumn((UIElement) grid17, 0);
      Grid.SetRow((UIElement) grid17, 2);
      TextBlock textBlock15 = new TextBlock();
      grid17.Children.Add((UIElement) textBlock15);
      textBlock15.Name = "e_49";
      textBlock15.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      textBlock15.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock15.VerticalAlignment = VerticalAlignment.Center;
      textBlock15.SetResourceReference(TextBlock.TextProperty, (object) "ContractsScreen_Help_Delete");
      TextBlock textBlock16 = new TextBlock();
      grid17.Children.Add((UIElement) textBlock16);
      textBlock16.Name = "e_50";
      textBlock16.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock16.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock16.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock16, 1);
      textBlock16.SetResourceReference(TextBlock.TextProperty, (object) "ContractsScreen_Help_Refresh");
      TextBlock textBlock17 = new TextBlock();
      grid17.Children.Add((UIElement) textBlock17);
      textBlock17.Name = "e_51";
      textBlock17.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock17.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock17.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock17, 2);
      textBlock17.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Back");
      Border border5 = new Border();
      grid15.Children.Add((UIElement) border5);
      border5.Name = "e_52";
      border5.VerticalAlignment = VerticalAlignment.Stretch;
      border5.Background = (Brush) new SolidColorBrush(new ColorW(33, 44, 53, (int) byte.MaxValue));
      border5.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      border5.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      Grid.SetColumn((UIElement) border5, 1);
      Grid grid18 = new Grid();
      border5.Child = (UIElement) grid18;
      grid18.Name = "e_53";
      GamepadBinding gamepadBinding8 = new GamepadBinding();
      gamepadBinding8.Gesture = (InputGesture) new GamepadGesture(GamepadInput.CButton);
      Binding binding17 = new Binding("CreateCommand");
      gamepadBinding8.SetBinding(InputBinding.CommandProperty, binding17);
      grid18.InputBindings.Add((InputBinding) gamepadBinding8);
      gamepadBinding8.Parent = (UIElement) grid18;
      grid18.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Star)
      });
      grid18.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid18.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      StackPanel stackPanel1 = new StackPanel();
      grid18.Children.Add((UIElement) stackPanel1);
      stackPanel1.Name = "e_54";
      stackPanel1.Margin = new Thickness(10f, 10f, 0.0f, 0.0f);
      stackPanel1.Orientation = Orientation.Vertical;
      Grid.SetRow((UIElement) stackPanel1, 0);
      TextBlock textBlock18 = new TextBlock();
      stackPanel1.Children.Add((UIElement) textBlock18);
      textBlock18.Name = "e_55";
      textBlock18.Margin = new Thickness(15f, 0.0f, 0.0f, 0.0f);
      textBlock18.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      textBlock18.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_NewContract");
      Grid grid19 = new Grid();
      stackPanel1.Children.Add((UIElement) grid19);
      grid19.Name = "e_56";
      grid19.Margin = new Thickness(15f, 0.0f, 0.0f, 0.0f);
      grid19.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Star)
      });
      grid19.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Star)
      });
      grid19.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Star)
      });
      grid19.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Star)
      });
      grid19.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(44f, GridUnitType.Pixel)
      });
      grid19.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(156f, GridUnitType.Pixel)
      });
      grid19.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      TextBlock textBlock19 = new TextBlock();
      grid19.Children.Add((UIElement) textBlock19);
      textBlock19.Name = "e_57";
      textBlock19.Margin = new Thickness(0.0f, 15f, 0.0f, 15f);
      textBlock19.VerticalAlignment = VerticalAlignment.Center;
      textBlock19.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock19, 0);
      Grid.SetRow((UIElement) textBlock19, 0);
      Grid.SetColumnSpan((UIElement) textBlock19, 2);
      textBlock19.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_MoneyReward");
      TextBlock textBlock20 = new TextBlock();
      grid19.Children.Add((UIElement) textBlock20);
      textBlock20.Name = "e_58";
      textBlock20.Margin = new Thickness(0.0f, 15f, 0.0f, 15f);
      textBlock20.VerticalAlignment = VerticalAlignment.Center;
      textBlock20.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock20, 0);
      Grid.SetRow((UIElement) textBlock20, 1);
      Grid.SetColumnSpan((UIElement) textBlock20, 2);
      textBlock20.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_StartingDeposit");
      TextBlock textBlock21 = new TextBlock();
      grid19.Children.Add((UIElement) textBlock21);
      textBlock21.Name = "e_59";
      textBlock21.Margin = new Thickness(0.0f, 15f, 0.0f, 15f);
      textBlock21.VerticalAlignment = VerticalAlignment.Center;
      textBlock21.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock21, 0);
      Grid.SetRow((UIElement) textBlock21, 2);
      Grid.SetColumnSpan((UIElement) textBlock21, 2);
      textBlock21.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_Duration");
      TextBlock textBlock22 = new TextBlock();
      grid19.Children.Add((UIElement) textBlock22);
      textBlock22.Name = "e_60";
      textBlock22.Margin = new Thickness(0.0f, 15f, 0.0f, 15f);
      textBlock22.VerticalAlignment = VerticalAlignment.Center;
      textBlock22.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock22, 0);
      Grid.SetRow((UIElement) textBlock22, 3);
      Grid.SetColumnSpan((UIElement) textBlock22, 2);
      textBlock22.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_Type");
      NumericTextBox numericTextBox1 = new NumericTextBox();
      grid19.Children.Add((UIElement) numericTextBox1);
      numericTextBox1.Name = "e_61";
      numericTextBox1.Margin = new Thickness(0.0f, 5f, 15f, 0.0f);
      numericTextBox1.VerticalAlignment = VerticalAlignment.Center;
      numericTextBox1.TabIndex = 103;
      numericTextBox1.MaxLength = 7;
      numericTextBox1.Minimum = 0.0f;
      numericTextBox1.Maximum = 9999999f;
      Grid.SetColumn((UIElement) numericTextBox1, 2);
      Grid.SetRow((UIElement) numericTextBox1, 0);
      GamepadHelp.SetTargetName((DependencyObject) numericTextBox1, "NumericHelp");
      GamepadHelp.SetTabIndexLeft((DependencyObject) numericTextBox1, 100);
      GamepadHelp.SetTabIndexDown((DependencyObject) numericTextBox1, 104);
      Binding binding18 = new Binding("NewContractCurrencyReward");
      numericTextBox1.SetBinding(NumericTextBox.ValueProperty, binding18);
      NumericTextBox numericTextBox2 = new NumericTextBox();
      grid19.Children.Add((UIElement) numericTextBox2);
      numericTextBox2.Name = "e_62";
      numericTextBox2.Margin = new Thickness(0.0f, 5f, 15f, 0.0f);
      numericTextBox2.VerticalAlignment = VerticalAlignment.Center;
      numericTextBox2.TabIndex = 104;
      numericTextBox2.MaxLength = 7;
      numericTextBox2.Minimum = 0.0f;
      numericTextBox2.Maximum = 9999999f;
      Grid.SetColumn((UIElement) numericTextBox2, 2);
      Grid.SetRow((UIElement) numericTextBox2, 1);
      GamepadHelp.SetTargetName((DependencyObject) numericTextBox2, "NumericHelp");
      GamepadHelp.SetTabIndexLeft((DependencyObject) numericTextBox2, 100);
      GamepadHelp.SetTabIndexUp((DependencyObject) numericTextBox2, 103);
      GamepadHelp.SetTabIndexDown((DependencyObject) numericTextBox2, 105);
      Binding binding19 = new Binding("NewContractStartDeposit");
      numericTextBox2.SetBinding(NumericTextBox.ValueProperty, binding19);
      NumericTextBox numericTextBox3 = new NumericTextBox();
      grid19.Children.Add((UIElement) numericTextBox3);
      numericTextBox3.Name = "e_63";
      numericTextBox3.Margin = new Thickness(0.0f, 5f, 15f, 0.0f);
      numericTextBox3.VerticalAlignment = VerticalAlignment.Center;
      numericTextBox3.TabIndex = 105;
      numericTextBox3.MaxLength = 5;
      numericTextBox3.Minimum = 0.0f;
      numericTextBox3.Maximum = 99999f;
      Grid.SetColumn((UIElement) numericTextBox3, 2);
      Grid.SetRow((UIElement) numericTextBox3, 2);
      GamepadHelp.SetTargetName((DependencyObject) numericTextBox3, "NumericHelp");
      GamepadHelp.SetTabIndexLeft((DependencyObject) numericTextBox3, 100);
      GamepadHelp.SetTabIndexUp((DependencyObject) numericTextBox3, 104);
      GamepadHelp.SetTabIndexDown((DependencyObject) numericTextBox3, 106);
      Binding binding20 = new Binding("NewContractDurationInMin");
      numericTextBox3.SetBinding(NumericTextBox.ValueProperty, binding20);
      ComboBox comboBox2 = new ComboBox();
      grid19.Children.Add((UIElement) comboBox2);
      comboBox2.Name = "e_64";
      comboBox2.Margin = new Thickness(0.0f, 4f, 15f, 4f);
      comboBox2.TabIndex = 106;
      Grid.SetColumn((UIElement) comboBox2, 2);
      Grid.SetRow((UIElement) comboBox2, 3);
      GamepadHelp.SetTargetName((DependencyObject) comboBox2, "SelectHelp");
      GamepadHelp.SetTabIndexLeft((DependencyObject) comboBox2, 100);
      GamepadHelp.SetTabIndexUp((DependencyObject) comboBox2, 105);
      Binding binding21 = new Binding("ContractTypes");
      comboBox2.SetBinding(ItemsControl.ItemsSourceProperty, binding21);
      Binding binding22 = new Binding("SelectedContractTypeIndex");
      comboBox2.SetBinding(Selector.SelectedIndexProperty, binding22);
      Binding binding23 = new Binding("TabIndexDown");
      comboBox2.SetBinding(GamepadHelp.TabIndexDownProperty, binding23);
      Border border6 = new Border();
      stackPanel1.Children.Add((UIElement) border6);
      border6.Name = "e_65";
      border6.Height = 2f;
      border6.Margin = new Thickness(15f, 0.0f, 15f, 0.0f);
      border6.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      Grid grid20 = new Grid();
      stackPanel1.Children.Add((UIElement) grid20);
      grid20.Name = "e_66";
      grid20.Margin = new Thickness(15f, 0.0f, 0.0f, 0.0f);
      Grid grid21 = new Grid();
      grid20.Children.Add((UIElement) grid21);
      grid21.Name = "e_67";
      grid21.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid21.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(200f, GridUnitType.Pixel)
      });
      grid21.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      Binding binding24 = new Binding("IsContractSelected_Deliver");
      grid21.SetBinding(UIElement.VisibilityProperty, binding24);
      TextBlock textBlock23 = new TextBlock();
      grid21.Children.Add((UIElement) textBlock23);
      textBlock23.Name = "e_68";
      textBlock23.Margin = new Thickness(0.0f, 15f, 0.0f, 15f);
      textBlock23.VerticalAlignment = VerticalAlignment.Center;
      textBlock23.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock23, 0);
      Grid.SetRow((UIElement) textBlock23, 0);
      textBlock23.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_TargetBlock");
      Grid grid22 = new Grid();
      grid21.Children.Add((UIElement) grid22);
      grid22.Name = "e_69";
      grid22.Margin = new Thickness(5f, 0.0f, 0.0f, 0.0f);
      grid22.HorizontalAlignment = HorizontalAlignment.Right;
      grid22.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      grid22.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      Grid.SetColumn((UIElement) grid22, 1);
      Grid.SetRow((UIElement) grid22, 0);
      TextBlock textBlock24 = new TextBlock();
      grid22.Children.Add((UIElement) textBlock24);
      textBlock24.Name = "e_70";
      textBlock24.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock24, 0);
      Binding binding25 = new Binding("NewContractSelectionName");
      textBlock24.SetBinding(TextBlock.TextProperty, binding25);
      Button button1 = new Button();
      grid22.Children.Add((UIElement) button1);
      button1.Name = "e_71";
      button1.Width = 150f;
      button1.Margin = new Thickness(10f, 5f, 15f, 0.0f);
      button1.TabIndex = 107;
      Grid.SetColumn((UIElement) button1, 1);
      GamepadHelp.SetTargetName((DependencyObject) button1, "SelectHelp");
      GamepadHelp.SetTabIndexLeft((DependencyObject) button1, 100);
      GamepadHelp.SetTabIndexUp((DependencyObject) button1, 106);
      GamepadHelp.SetTabIndexDown((DependencyObject) button1, 114);
      Binding binding26 = new Binding("NewContractDeliverBlockSelectCommand");
      button1.SetBinding(Button.CommandProperty, binding26);
      button1.SetResourceReference(ContentControl.ContentProperty, (object) "ContractScreen_Button_SelectBlock");
      Grid grid23 = new Grid();
      grid20.Children.Add((UIElement) grid23);
      grid23.Name = "e_72";
      grid23.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid23.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid23.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid23.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(200f, GridUnitType.Pixel)
      });
      grid23.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      Binding binding27 = new Binding("IsContractSelected_ObtainAndDeliver");
      grid23.SetBinding(UIElement.VisibilityProperty, binding27);
      TextBlock textBlock25 = new TextBlock();
      grid23.Children.Add((UIElement) textBlock25);
      textBlock25.Name = "e_73";
      textBlock25.Margin = new Thickness(0.0f, 15f, 0.0f, 15f);
      textBlock25.VerticalAlignment = VerticalAlignment.Center;
      textBlock25.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock25, 0);
      Grid.SetRow((UIElement) textBlock25, 0);
      textBlock25.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_TargetBlock");
      TextBlock textBlock26 = new TextBlock();
      grid23.Children.Add((UIElement) textBlock26);
      textBlock26.Name = "e_74";
      textBlock26.Margin = new Thickness(0.0f, 15f, 0.0f, 15f);
      textBlock26.VerticalAlignment = VerticalAlignment.Center;
      textBlock26.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock26, 0);
      Grid.SetRow((UIElement) textBlock26, 1);
      textBlock26.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_ItemType");
      TextBlock textBlock27 = new TextBlock();
      grid23.Children.Add((UIElement) textBlock27);
      textBlock27.Name = "e_75";
      textBlock27.Margin = new Thickness(0.0f, 15f, 0.0f, 15f);
      textBlock27.VerticalAlignment = VerticalAlignment.Center;
      textBlock27.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock27, 0);
      Grid.SetRow((UIElement) textBlock27, 2);
      textBlock27.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_ItemAmount");
      Grid grid24 = new Grid();
      grid23.Children.Add((UIElement) grid24);
      grid24.Name = "e_76";
      grid24.Margin = new Thickness(5f, 0.0f, 0.0f, 0.0f);
      grid24.HorizontalAlignment = HorizontalAlignment.Right;
      grid24.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      grid24.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      Grid.SetColumn((UIElement) grid24, 1);
      Grid.SetRow((UIElement) grid24, 0);
      TextBlock textBlock28 = new TextBlock();
      grid24.Children.Add((UIElement) textBlock28);
      textBlock28.Name = "e_77";
      textBlock28.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock28, 0);
      Binding binding28 = new Binding("NewContractSelectionName");
      textBlock28.SetBinding(TextBlock.TextProperty, binding28);
      Button button2 = new Button();
      grid24.Children.Add((UIElement) button2);
      button2.Name = "e_78";
      button2.Width = 150f;
      button2.Margin = new Thickness(10f, 5f, 15f, 0.0f);
      button2.TabIndex = 108;
      Grid.SetColumn((UIElement) button2, 1);
      GamepadHelp.SetTargetName((DependencyObject) button2, "SelectHelp");
      GamepadHelp.SetTabIndexLeft((DependencyObject) button2, 100);
      GamepadHelp.SetTabIndexUp((DependencyObject) button2, 106);
      GamepadHelp.SetTabIndexDown((DependencyObject) button2, 109);
      Binding binding29 = new Binding("NewContractObtainAndDeliverBlockSelectCommand");
      button2.SetBinding(Button.CommandProperty, binding29);
      button2.SetResourceReference(ContentControl.ContentProperty, (object) "ContractScreen_Button_SelectBlock");
      ComboBox comboBox3 = new ComboBox();
      grid23.Children.Add((UIElement) comboBox3);
      comboBox3.Name = "e_79";
      comboBox3.Margin = new Thickness(0.0f, 0.0f, 15f, 0.0f);
      comboBox3.TabIndex = 109;
      Grid.SetColumn((UIElement) comboBox3, 1);
      Grid.SetRow((UIElement) comboBox3, 1);
      GamepadHelp.SetTargetName((DependencyObject) comboBox3, "SelectHelp");
      GamepadHelp.SetTabIndexLeft((DependencyObject) comboBox3, 100);
      GamepadHelp.SetTabIndexUp((DependencyObject) comboBox3, 108);
      GamepadHelp.SetTabIndexDown((DependencyObject) comboBox3, 110);
      Binding binding30 = new Binding("DeliverableItems");
      comboBox3.SetBinding(ItemsControl.ItemsSourceProperty, binding30);
      Binding binding31 = new Binding("NewContractObtainAndDeliverSelectedItemType");
      comboBox3.SetBinding(Selector.SelectedItemProperty, binding31);
      NumericTextBox numericTextBox4 = new NumericTextBox();
      grid23.Children.Add((UIElement) numericTextBox4);
      numericTextBox4.Name = "e_80";
      numericTextBox4.Margin = new Thickness(0.0f, 0.0f, 15f, 0.0f);
      numericTextBox4.VerticalAlignment = VerticalAlignment.Center;
      numericTextBox4.TabIndex = 110;
      numericTextBox4.MaxLength = 5;
      numericTextBox4.Minimum = 0.0f;
      numericTextBox4.Maximum = 99999f;
      Grid.SetColumn((UIElement) numericTextBox4, 1);
      Grid.SetRow((UIElement) numericTextBox4, 2);
      GamepadHelp.SetTargetName((DependencyObject) numericTextBox4, "NumericHelp");
      GamepadHelp.SetTabIndexLeft((DependencyObject) numericTextBox4, 100);
      GamepadHelp.SetTabIndexUp((DependencyObject) numericTextBox4, 109);
      GamepadHelp.SetTabIndexDown((DependencyObject) numericTextBox4, 114);
      Binding binding32 = new Binding("NewContractObtainAndDeliverItemAmount");
      numericTextBox4.SetBinding(NumericTextBox.ValueProperty, binding32);
      Grid grid25 = new Grid();
      grid20.Children.Add((UIElement) grid25);
      grid25.Name = "e_81";
      grid25.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid25.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid25.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(200f, GridUnitType.Pixel)
      });
      grid25.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      Binding binding33 = new Binding("IsContractSelected_Find");
      grid25.SetBinding(UIElement.VisibilityProperty, binding33);
      TextBlock textBlock29 = new TextBlock();
      grid25.Children.Add((UIElement) textBlock29);
      textBlock29.Name = "e_82";
      textBlock29.Margin = new Thickness(0.0f, 15f, 0.0f, 15f);
      textBlock29.VerticalAlignment = VerticalAlignment.Center;
      textBlock29.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock29, 0);
      Grid.SetRow((UIElement) textBlock29, 0);
      textBlock29.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_TargetGrid");
      TextBlock textBlock30 = new TextBlock();
      grid25.Children.Add((UIElement) textBlock30);
      textBlock30.Name = "e_83";
      textBlock30.Margin = new Thickness(0.0f, 15f, 0.0f, 15f);
      textBlock30.VerticalAlignment = VerticalAlignment.Center;
      textBlock30.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock30, 0);
      Grid.SetRow((UIElement) textBlock30, 1);
      textBlock30.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_SearchRadius");
      Grid grid26 = new Grid();
      grid25.Children.Add((UIElement) grid26);
      grid26.Name = "e_84";
      grid26.Margin = new Thickness(5f, 0.0f, 0.0f, 0.0f);
      grid26.HorizontalAlignment = HorizontalAlignment.Right;
      grid26.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      grid26.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      Grid.SetColumn((UIElement) grid26, 1);
      Grid.SetRow((UIElement) grid26, 0);
      TextBlock textBlock31 = new TextBlock();
      grid26.Children.Add((UIElement) textBlock31);
      textBlock31.Name = "e_85";
      textBlock31.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock31, 0);
      Binding binding34 = new Binding("NewContractSelectionName");
      textBlock31.SetBinding(TextBlock.TextProperty, binding34);
      Button button3 = new Button();
      grid26.Children.Add((UIElement) button3);
      button3.Name = "e_86";
      button3.Width = 150f;
      button3.Margin = new Thickness(10f, 5f, 15f, 0.0f);
      button3.VerticalAlignment = VerticalAlignment.Center;
      button3.TabIndex = 111;
      Grid.SetColumn((UIElement) button3, 1);
      GamepadHelp.SetTargetName((DependencyObject) button3, "SelectHelp");
      GamepadHelp.SetTabIndexLeft((DependencyObject) button3, 100);
      GamepadHelp.SetTabIndexUp((DependencyObject) button3, 106);
      GamepadHelp.SetTabIndexDown((DependencyObject) button3, 112);
      Binding binding35 = new Binding("NewContractFindGridSelectCommand");
      button3.SetBinding(Button.CommandProperty, binding35);
      button3.SetResourceReference(ContentControl.ContentProperty, (object) "ContractScreen_Button_SelectGrid");
      NumericTextBox numericTextBox5 = new NumericTextBox();
      grid25.Children.Add((UIElement) numericTextBox5);
      numericTextBox5.Name = "e_87";
      numericTextBox5.Margin = new Thickness(0.0f, 0.0f, 15f, 0.0f);
      numericTextBox5.VerticalAlignment = VerticalAlignment.Center;
      numericTextBox5.TabIndex = 112;
      numericTextBox5.MaxLength = 5;
      numericTextBox5.Minimum = 0.0f;
      numericTextBox5.Maximum = 99999f;
      Grid.SetColumn((UIElement) numericTextBox5, 1);
      Grid.SetRow((UIElement) numericTextBox5, 1);
      GamepadHelp.SetTargetName((DependencyObject) numericTextBox5, "NumericHelp");
      GamepadHelp.SetTabIndexLeft((DependencyObject) numericTextBox5, 100);
      GamepadHelp.SetTabIndexUp((DependencyObject) numericTextBox5, 111);
      GamepadHelp.SetTabIndexDown((DependencyObject) numericTextBox5, 114);
      Binding binding36 = new Binding("NewContractFindSearchRadius");
      numericTextBox5.SetBinding(NumericTextBox.ValueProperty, binding36);
      Grid grid27 = new Grid();
      grid20.Children.Add((UIElement) grid27);
      grid27.Name = "e_88";
      grid27.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid27.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(200f, GridUnitType.Pixel)
      });
      grid27.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      Binding binding37 = new Binding("IsContractSelected_Repair");
      grid27.SetBinding(UIElement.VisibilityProperty, binding37);
      TextBlock textBlock32 = new TextBlock();
      grid27.Children.Add((UIElement) textBlock32);
      textBlock32.Name = "e_89";
      textBlock32.Margin = new Thickness(0.0f, 15f, 0.0f, 15f);
      textBlock32.VerticalAlignment = VerticalAlignment.Center;
      textBlock32.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock32, 0);
      Grid.SetRow((UIElement) textBlock32, 0);
      textBlock32.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_TargetGrid");
      Grid grid28 = new Grid();
      grid27.Children.Add((UIElement) grid28);
      grid28.Name = "e_90";
      grid28.Margin = new Thickness(5f, 0.0f, 0.0f, 0.0f);
      grid28.HorizontalAlignment = HorizontalAlignment.Right;
      grid28.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      grid28.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      Grid.SetColumn((UIElement) grid28, 1);
      Grid.SetRow((UIElement) grid28, 0);
      TextBlock textBlock33 = new TextBlock();
      grid28.Children.Add((UIElement) textBlock33);
      textBlock33.Name = "e_91";
      textBlock33.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock33, 0);
      Binding binding38 = new Binding("NewContractSelectionName");
      textBlock33.SetBinding(TextBlock.TextProperty, binding38);
      Button button4 = new Button();
      grid28.Children.Add((UIElement) button4);
      button4.Name = "e_92";
      button4.Width = 150f;
      button4.Margin = new Thickness(10f, 5f, 15f, 0.0f);
      button4.TabIndex = 113;
      Grid.SetColumn((UIElement) button4, 1);
      GamepadHelp.SetTargetName((DependencyObject) button4, "SelectHelp");
      GamepadHelp.SetTabIndexLeft((DependencyObject) button4, 100);
      GamepadHelp.SetTabIndexUp((DependencyObject) button4, 106);
      GamepadHelp.SetTabIndexDown((DependencyObject) button4, 114);
      Binding binding39 = new Binding("NewContractRepairGridSelectCommand");
      button4.SetBinding(Button.CommandProperty, binding39);
      button4.SetResourceReference(ContentControl.ContentProperty, (object) "ContractScreen_Button_SelectGrid");
      Border border7 = new Border();
      grid18.Children.Add((UIElement) border7);
      border7.Name = "e_93";
      border7.Height = 2f;
      border7.Margin = new Thickness(25f, 0.0f, 15f, 0.0f);
      border7.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      Grid.SetRow((UIElement) border7, 1);
      Grid grid29 = new Grid();
      grid18.Children.Add((UIElement) grid29);
      grid29.Name = "e_94";
      grid29.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(200f, GridUnitType.Pixel)
      });
      grid29.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      Grid.SetRow((UIElement) grid29, 2);
      TextBlock textBlock34 = new TextBlock();
      grid29.Children.Add((UIElement) textBlock34);
      textBlock34.Name = "e_95";
      textBlock34.Margin = new Thickness(25f, 10f, 0.0f, 10f);
      textBlock34.VerticalAlignment = VerticalAlignment.Center;
      textBlock34.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock34, 0);
      Grid.SetRow((UIElement) textBlock34, 0);
      textBlock34.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_CurrentMoney");
      StackPanel stackPanel2 = new StackPanel();
      grid29.Children.Add((UIElement) stackPanel2);
      stackPanel2.Name = "e_96";
      stackPanel2.Margin = new Thickness(0.0f, 10f, 15f, 10f);
      stackPanel2.HorizontalAlignment = HorizontalAlignment.Right;
      stackPanel2.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel2, 1);
      Grid.SetRow((UIElement) stackPanel2, 0);
      TextBlock textBlock35 = new TextBlock();
      stackPanel2.Children.Add((UIElement) textBlock35);
      textBlock35.Name = "e_97";
      textBlock35.Margin = new Thickness(30f, 0.0f, 0.0f, 0.0f);
      textBlock35.HorizontalAlignment = HorizontalAlignment.Right;
      textBlock35.VerticalAlignment = VerticalAlignment.Center;
      textBlock35.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Binding binding40 = new Binding("CurrentMoneyFormated");
      textBlock35.SetBinding(TextBlock.TextProperty, binding40);
      Image image = new Image();
      stackPanel2.Children.Add((UIElement) image);
      image.Name = "e_98";
      image.Height = 20f;
      image.Margin = new Thickness(4f, 2f, 2f, 2f);
      image.HorizontalAlignment = HorizontalAlignment.Right;
      image.VerticalAlignment = VerticalAlignment.Center;
      image.Stretch = Stretch.Uniform;
      Grid.SetColumn((UIElement) image, 2);
      Binding binding41 = new Binding("CurrencyIcon");
      image.SetBinding(Image.SourceProperty, binding41);
      Border border8 = new Border();
      grid15.Children.Add((UIElement) border8);
      border8.Name = "e_99";
      border8.Height = 2f;
      border8.Margin = new Thickness(0.0f, 10f, 0.0f, 10f);
      border8.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      Grid.SetRow((UIElement) border8, 1);
      Grid.SetColumnSpan((UIElement) border8, 2);
      Grid grid30 = new Grid();
      grid15.Children.Add((UIElement) grid30);
      grid30.Name = "NumericHelp";
      grid30.Visibility = Visibility.Collapsed;
      grid30.VerticalAlignment = VerticalAlignment.Center;
      grid30.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid30.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid30.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition6 = new ColumnDefinition();
      grid30.ColumnDefinitions.Add(columnDefinition6);
      Grid.SetColumn((UIElement) grid30, 0);
      Grid.SetRow((UIElement) grid30, 2);
      TextBlock textBlock36 = new TextBlock();
      grid30.Children.Add((UIElement) textBlock36);
      textBlock36.Name = "e_100";
      textBlock36.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      textBlock36.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock36.VerticalAlignment = VerticalAlignment.Center;
      textBlock36.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_ChangeValue");
      TextBlock textBlock37 = new TextBlock();
      grid30.Children.Add((UIElement) textBlock37);
      textBlock37.Name = "e_101";
      textBlock37.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock37.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock37.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock37, 1);
      textBlock37.SetResourceReference(TextBlock.TextProperty, (object) "ContractsScreen_Help_CreateContract");
      TextBlock textBlock38 = new TextBlock();
      grid30.Children.Add((UIElement) textBlock38);
      textBlock38.Name = "e_102";
      textBlock38.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock38.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock38.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock38, 2);
      textBlock38.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Back");
      Grid grid31 = new Grid();
      grid15.Children.Add((UIElement) grid31);
      grid31.Name = "SelectHelp";
      grid31.Visibility = Visibility.Collapsed;
      grid31.VerticalAlignment = VerticalAlignment.Center;
      grid31.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid31.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid31.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition7 = new ColumnDefinition();
      grid31.ColumnDefinitions.Add(columnDefinition7);
      Grid.SetColumn((UIElement) grid31, 0);
      Grid.SetRow((UIElement) grid31, 2);
      TextBlock textBlock39 = new TextBlock();
      grid31.Children.Add((UIElement) textBlock39);
      textBlock39.Name = "e_103";
      textBlock39.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      textBlock39.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock39.VerticalAlignment = VerticalAlignment.Center;
      textBlock39.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Select");
      TextBlock textBlock40 = new TextBlock();
      grid31.Children.Add((UIElement) textBlock40);
      textBlock40.Name = "e_104";
      textBlock40.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock40.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock40.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock40, 1);
      textBlock40.SetResourceReference(TextBlock.TextProperty, (object) "ContractsScreen_Help_CreateContract");
      TextBlock textBlock41 = new TextBlock();
      grid31.Children.Add((UIElement) textBlock41);
      textBlock41.Name = "e_105";
      textBlock41.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock41.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock41.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock41, 2);
      textBlock41.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Back");
      observableCollection.Add((object) tabItem3);
      return observableCollection;
    }

    private static void InitializeElemente_16Resources(UIElement elem) => elem.Resources.MergedDictionaries.Add((ResourceDictionary) DataTemplatesContractsDataGrid.Instance);

    private static void InitializeElemente_32Resources(UIElement elem) => elem.Resources.MergedDictionaries.Add((ResourceDictionary) DataTemplatesContractsDataGrid.Instance);

    private static void InitializeElemente_46Resources(UIElement elem) => elem.Resources.MergedDictionaries.Add((ResourceDictionary) DataTemplatesContractsDataGrid.Instance);
  }
}
