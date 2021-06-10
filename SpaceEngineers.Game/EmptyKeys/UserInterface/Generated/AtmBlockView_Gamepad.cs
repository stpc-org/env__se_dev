// Decompiled with JetBrains decompiler
// Type: EmptyKeys.UserInterface.Generated.AtmBlockView_Gamepad
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Controls.Primitives;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.AtmBlockView_Gamepad_Bindings;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Themes;
using Sandbox.Game.Screens.ViewModels;
using System;
using System.CodeDom.Compiler;

namespace EmptyKeys.UserInterface.Generated
{
  [GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
  public class AtmBlockView_Gamepad : UIRoot
  {
    private Grid rootGrid;
    private Image e_0;
    private Grid e_1;
    private ImageButton e_2;
    private StackPanel e_3;
    private TextBlock e_4;
    private Border e_5;
    private Grid e_6;
    private Grid e_7;
    private Grid e_8;
    private ListBox e_9;
    private Grid e_14;
    private TextBlock e_15;
    private StackPanel e_16;
    private TextBlock e_17;
    private TextBlock e_18;
    private TextBlock e_19;
    private Grid e_20;
    private TextBlock e_21;
    private TextBlock e_22;
    private Image e_23;
    private TextBlock e_24;
    private Slider e_25;
    private TextBlock e_26;
    private Image e_27;
    private Border e_28;
    private Grid CashbackHelp;
    private TextBlock e_29;
    private TextBlock e_30;
    private TextBlock e_31;

    public AtmBlockView_Gamepad() => this.Initialize();

    public AtmBlockView_Gamepad(int width, int height)
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
      AtmBlockView_Gamepad.InitializeElementResources((UIElement) this);
      this.rootGrid = new Grid();
      this.Content = (object) this.rootGrid;
      this.rootGrid.Name = "rootGrid";
      this.rootGrid.Height = 700f;
      this.rootGrid.Width = 500f;
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
        Width = new GridLength(2f, GridUnitType.Star)
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
      this.e_4.SetResourceReference(TextBlock.TextProperty, (object) "ScreenCaptionATM");
      this.e_5 = new Border();
      this.e_3.Children.Add((UIElement) this.e_5);
      this.e_5.Name = "e_5";
      this.e_5.Height = 2f;
      this.e_5.Margin = new Thickness(0.0f, 10f, 0.0f, 10f);
      this.e_5.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      this.e_6 = new Grid();
      this.e_1.Children.Add((UIElement) this.e_6);
      this.e_6.Name = "e_6";
      this.e_6.RowDefinitions.Add(new RowDefinition());
      this.e_6.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_6.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_6.ColumnDefinitions.Add(new ColumnDefinition());
      Grid.SetColumn((UIElement) this.e_6, 1);
      Grid.SetRow((UIElement) this.e_6, 3);
      this.e_6.SetBinding(UIElement.DataContextProperty, new Binding("InventoryTargetViewModel")
      {
        UseGeneratedBindings = true
      });
      this.e_7 = new Grid();
      this.e_6.Children.Add((UIElement) this.e_7);
      this.e_7.Name = "e_7";
      this.e_7.Margin = new Thickness(0.0f, 0.0f, 0.0f, 0.0f);
      this.e_8 = new Grid();
      this.e_7.Children.Add((UIElement) this.e_8);
      this.e_8.Name = "e_8";
      GamepadBinding gamepadBinding1 = new GamepadBinding();
      gamepadBinding1.Gesture = (InputGesture) new GamepadGesture(GamepadInput.CButton);
      gamepadBinding1.SetBinding(InputBinding.CommandProperty, new Binding("DepositCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_8.InputBindings.Add((InputBinding) gamepadBinding1);
      gamepadBinding1.Parent = (UIElement) this.e_8;
      GamepadBinding gamepadBinding2 = new GamepadBinding();
      gamepadBinding2.Gesture = (InputGesture) new GamepadGesture(GamepadInput.DButton);
      gamepadBinding2.SetBinding(InputBinding.CommandProperty, new Binding("WithdrawCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_8.InputBindings.Add((InputBinding) gamepadBinding2);
      gamepadBinding2.Parent = (UIElement) this.e_8;
      this.e_8.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_8.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_8.RowDefinitions.Add(new RowDefinition());
      this.e_8.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_8.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_9 = new ListBox();
      this.e_8.Children.Add((UIElement) this.e_9);
      this.e_9.Name = "e_9";
      this.e_9.Margin = new Thickness(0.0f, 5f, 0.0f, 0.0f);
      Style style = new Style(typeof (ListBox));
      Setter setter1 = new Setter(UIElement.MinHeightProperty, (object) 80f);
      style.Setters.Add(setter1);
      ControlTemplate controlTemplate = new ControlTemplate(typeof (ListBox), new Func<UIElement, UIElement>(AtmBlockView_Gamepad.e_9_s_S_1_ctMethod));
      Setter setter2 = new Setter(Control.TemplateProperty, (object) controlTemplate);
      style.Setters.Add(setter2);
      Trigger trigger = new Trigger();
      trigger.Property = UIElement.IsFocusedProperty;
      trigger.Value = (object) true;
      Setter setter3 = new Setter(Control.BackgroundProperty, (object) new SolidColorBrush(new ColorW(67, 81, 92, 160)));
      trigger.Setters.Add(setter3);
      style.Triggers.Add((TriggerBase) trigger);
      this.e_9.Style = style;
      this.e_9.Background = (Brush) new SolidColorBrush(new ColorW(33, 44, 53, (int) byte.MaxValue));
      this.e_9.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      this.e_9.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      this.e_9.TabIndex = 0;
      Grid.SetRow((UIElement) this.e_9, 2);
      GamepadHelp.SetTargetName((DependencyObject) this.e_9, "CashbackHelp");
      GamepadHelp.SetTabIndexDown((DependencyObject) this.e_9, 1);
      this.e_9.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("InventoryItems")
      {
        UseGeneratedBindings = true
      });
      AtmBlockView_Gamepad.InitializeElemente_9Resources((UIElement) this.e_9);
      this.e_14 = new Grid();
      this.e_8.Children.Add((UIElement) this.e_14);
      this.e_14.Name = "e_14";
      this.e_14.Margin = new Thickness(2f, 2f, 2f, 2f);
      this.e_14.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_14.ColumnDefinitions.Add(new ColumnDefinition());
      Grid.SetRow((UIElement) this.e_14, 3);
      this.e_15 = new TextBlock();
      this.e_14.Children.Add((UIElement) this.e_15);
      this.e_15.Name = "e_15";
      this.e_15.Margin = new Thickness(0.0f, 5f, 5f, 5f);
      this.e_15.VerticalAlignment = VerticalAlignment.Center;
      this.e_15.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      this.e_15.SetResourceReference(TextBlock.TextProperty, (object) "ScreenTerminalInventory_Volume");
      this.e_16 = new StackPanel();
      this.e_14.Children.Add((UIElement) this.e_16);
      this.e_16.Name = "e_16";
      this.e_16.Margin = new Thickness(5f, 5f, 0.0f, 5f);
      this.e_16.HorizontalAlignment = HorizontalAlignment.Right;
      this.e_16.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) this.e_16, 1);
      this.e_17 = new TextBlock();
      this.e_16.Children.Add((UIElement) this.e_17);
      this.e_17.Name = "e_17";
      this.e_17.VerticalAlignment = VerticalAlignment.Center;
      this.e_17.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      this.e_17.SetBinding(TextBlock.TextProperty, new Binding("CurrentInventoryVolume")
      {
        UseGeneratedBindings = true
      });
      this.e_18 = new TextBlock();
      this.e_16.Children.Add((UIElement) this.e_18);
      this.e_18.Name = "e_18";
      this.e_18.Margin = new Thickness(10f, 0.0f, 10f, 0.0f);
      this.e_18.VerticalAlignment = VerticalAlignment.Center;
      this.e_18.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      this.e_18.Text = "/";
      this.e_19 = new TextBlock();
      this.e_16.Children.Add((UIElement) this.e_19);
      this.e_19.Name = "e_19";
      this.e_19.VerticalAlignment = VerticalAlignment.Center;
      this.e_19.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      this.e_19.SetBinding(TextBlock.TextProperty, new Binding("MaxInventoryVolume")
      {
        UseGeneratedBindings = true
      });
      this.e_20 = new Grid();
      this.e_8.Children.Add((UIElement) this.e_20);
      this.e_20.Name = "e_20";
      this.e_20.Margin = new Thickness(2f, 2f, 2f, 2f);
      this.e_20.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_20.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_20.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_20.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_20.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(65f, GridUnitType.Pixel)
      });
      this.e_20.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_20.ColumnDefinitions.Add(new ColumnDefinition());
      this.e_20.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      Grid.SetRow((UIElement) this.e_20, 4);
      this.e_21 = new TextBlock();
      this.e_20.Children.Add((UIElement) this.e_21);
      this.e_21.Name = "e_21";
      this.e_21.Margin = new Thickness(0.0f, 5f, 5f, 5f);
      this.e_21.VerticalAlignment = VerticalAlignment.Center;
      this.e_21.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      this.e_21.SetResourceReference(TextBlock.TextProperty, (object) "Currency_Default_Account_Label");
      this.e_22 = new TextBlock();
      this.e_20.Children.Add((UIElement) this.e_22);
      this.e_22.Name = "e_22";
      this.e_22.Margin = new Thickness(5f, 5f, 0.0f, 5f);
      this.e_22.HorizontalAlignment = HorizontalAlignment.Right;
      this.e_22.VerticalAlignment = VerticalAlignment.Center;
      this.e_22.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) this.e_22, 1);
      this.e_22.SetBinding(TextBlock.TextProperty, new Binding("LocalPlayerCurrency")
      {
        UseGeneratedBindings = true
      });
      this.e_23 = new Image();
      this.e_20.Children.Add((UIElement) this.e_23);
      this.e_23.Name = "e_23";
      this.e_23.Height = 20f;
      this.e_23.Margin = new Thickness(4f, 2f, 2f, 2f);
      this.e_23.VerticalAlignment = VerticalAlignment.Center;
      this.e_23.Stretch = Stretch.Uniform;
      Grid.SetColumn((UIElement) this.e_23, 2);
      this.e_23.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      this.e_24 = new TextBlock();
      this.e_20.Children.Add((UIElement) this.e_24);
      this.e_24.Name = "e_24";
      this.e_24.Margin = new Thickness(0.0f, 5f, 5f, 5f);
      this.e_24.VerticalAlignment = VerticalAlignment.Center;
      this.e_24.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetRow((UIElement) this.e_24, 1);
      this.e_24.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlockView_CashBack");
      this.e_25 = new Slider();
      this.e_20.Children.Add((UIElement) this.e_25);
      this.e_25.Name = "e_25";
      this.e_25.Margin = new Thickness(5f, 5f, 5f, 5f);
      this.e_25.HorizontalAlignment = HorizontalAlignment.Stretch;
      this.e_25.VerticalAlignment = VerticalAlignment.Center;
      this.e_25.TabIndex = 1;
      this.e_25.Minimum = 0.0f;
      this.e_25.Maximum = 1000000f;
      this.e_25.IsSnapToTickEnabled = true;
      this.e_25.TickFrequency = 1f;
      Grid.SetColumn((UIElement) this.e_25, 1);
      Grid.SetRow((UIElement) this.e_25, 1);
      Grid.SetColumnSpan((UIElement) this.e_25, 2);
      GamepadHelp.SetTargetName((DependencyObject) this.e_25, "CashbackHelp");
      GamepadHelp.SetTabIndexUp((DependencyObject) this.e_25, 0);
      this.e_25.SetBinding(RangeBase.ValueProperty, new Binding("BalanceChangeValue")
      {
        UseGeneratedBindings = true
      });
      this.e_26 = new TextBlock();
      this.e_20.Children.Add((UIElement) this.e_26);
      this.e_26.Name = "e_26";
      this.e_26.Margin = new Thickness(5f, 5f, 0.0f, 5f);
      this.e_26.HorizontalAlignment = HorizontalAlignment.Right;
      this.e_26.VerticalAlignment = VerticalAlignment.Center;
      this.e_26.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) this.e_26, 1);
      Grid.SetRow((UIElement) this.e_26, 2);
      this.e_26.SetBinding(TextBlock.TextProperty, new Binding("BalanceChangeValue")
      {
        UseGeneratedBindings = true
      });
      this.e_27 = new Image();
      this.e_20.Children.Add((UIElement) this.e_27);
      this.e_27.Name = "e_27";
      this.e_27.Height = 20f;
      this.e_27.Margin = new Thickness(4f, 2f, 2f, 2f);
      this.e_27.VerticalAlignment = VerticalAlignment.Center;
      this.e_27.Stretch = Stretch.Uniform;
      Grid.SetColumn((UIElement) this.e_27, 2);
      Grid.SetRow((UIElement) this.e_27, 2);
      this.e_27.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      this.e_28 = new Border();
      this.e_20.Children.Add((UIElement) this.e_28);
      this.e_28.Name = "e_28";
      this.e_28.Height = 2f;
      this.e_28.Margin = new Thickness(0.0f, 10f, 0.0f, 0.0f);
      this.e_28.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      Grid.SetRow((UIElement) this.e_28, 3);
      Grid.SetColumnSpan((UIElement) this.e_28, 3);
      this.CashbackHelp = new Grid();
      this.e_20.Children.Add((UIElement) this.CashbackHelp);
      this.CashbackHelp.Name = "CashbackHelp";
      this.CashbackHelp.VerticalAlignment = VerticalAlignment.Center;
      this.CashbackHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.CashbackHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.CashbackHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.CashbackHelp.ColumnDefinitions.Add(new ColumnDefinition());
      Grid.SetRow((UIElement) this.CashbackHelp, 4);
      Grid.SetColumnSpan((UIElement) this.CashbackHelp, 3);
      this.e_29 = new TextBlock();
      this.CashbackHelp.Children.Add((UIElement) this.e_29);
      this.e_29.Name = "e_29";
      this.e_29.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      this.e_29.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_29.VerticalAlignment = VerticalAlignment.Center;
      this.e_29.SetResourceReference(TextBlock.TextProperty, (object) "StoreScreen_Help_Deposit");
      this.e_30 = new TextBlock();
      this.CashbackHelp.Children.Add((UIElement) this.e_30);
      this.e_30.Name = "e_30";
      this.e_30.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      this.e_30.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_30.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) this.e_30, 1);
      this.e_30.SetResourceReference(TextBlock.TextProperty, (object) "StoreScreen_Help_Withdraw");
      this.e_31 = new TextBlock();
      this.CashbackHelp.Children.Add((UIElement) this.e_31);
      this.e_31.Name = "e_31";
      this.e_31.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      this.e_31.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_31.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) this.e_31, 2);
      this.e_31.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Back");
      ImageManager.Instance.AddImage("Textures\\GUI\\Screens\\screen_background.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol_highlight.dds");
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "MaxWidth", typeof (MyStoreBlockViewModel_MaxWidth_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "BackgroundOverlay", typeof (MyStoreBlockViewModel_BackgroundOverlay_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "ExitCommand", typeof (MyStoreBlockViewModel_ExitCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "InventoryTargetViewModel", typeof (MyStoreBlockViewModel_InventoryTargetViewModel_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryTargetViewModel), "DepositCommand", typeof (MyInventoryTargetViewModel_DepositCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryTargetViewModel), "WithdrawCommand", typeof (MyInventoryTargetViewModel_WithdrawCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryTargetViewModel), "InventoryItems", typeof (MyInventoryTargetViewModel_InventoryItems_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryTargetViewModel), "CurrentInventoryVolume", typeof (MyInventoryTargetViewModel_CurrentInventoryVolume_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryTargetViewModel), "MaxInventoryVolume", typeof (MyInventoryTargetViewModel_MaxInventoryVolume_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryTargetViewModel), "LocalPlayerCurrency", typeof (MyInventoryTargetViewModel_LocalPlayerCurrency_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryTargetViewModel), "CurrencyIcon", typeof (MyInventoryTargetViewModel_CurrencyIcon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryTargetViewModel), "BalanceChangeValue", typeof (MyInventoryTargetViewModel_BalanceChangeValue_PropertyInfo));
    }

    private static void InitializeElementResources(UIElement elem)
    {
      elem.Resources.MergedDictionaries.Add((ResourceDictionary) Styles.Instance);
      elem.Resources.MergedDictionaries.Add((ResourceDictionary) DataTemplatesStoreBlock.Instance);
    }

    private static UIElement e_9_s_S_1_ctMethod(UIElement parent)
    {
      Border border = new Border();
      border.Parent = parent;
      border.Name = "e_10";
      border.SnapsToDevicePixels = true;
      border.SetBinding(Control.BorderThicknessProperty, new Binding("BorderThickness")
      {
        Source = (object) parent
      });
      border.SetBinding(Control.BorderBrushProperty, new Binding("BorderBrush")
      {
        Source = (object) parent
      });
      border.SetBinding(Control.BackgroundProperty, new Binding("Background")
      {
        Source = (object) parent
      });
      ScrollViewer scrollViewer = new ScrollViewer();
      border.Child = (UIElement) scrollViewer;
      scrollViewer.Name = "PART_DataScrollViewer";
      scrollViewer.HorizontalContentAlignment = HorizontalAlignment.Stretch;
      scrollViewer.VerticalContentAlignment = VerticalAlignment.Stretch;
      UniformGrid uniformGrid = new UniformGrid();
      scrollViewer.Content = (object) uniformGrid;
      uniformGrid.Name = "e_11";
      uniformGrid.Margin = new Thickness(4f, 4f, 4f, 4f);
      uniformGrid.VerticalAlignment = VerticalAlignment.Top;
      uniformGrid.IsItemsHost = true;
      uniformGrid.Columns = 5;
      return (UIElement) border;
    }

    private static void InitializeElemente_9Resources(UIElement elem)
    {
      Style style = new Style(typeof (ListBoxItem));
      Setter setter1 = new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "GridItem"));
      style.Setters.Add(setter1);
      ControlTemplate controlTemplate = new ControlTemplate(typeof (ListBoxItem), new Func<UIElement, UIElement>(AtmBlockView_Gamepad.e_9r_0_s_S_1_ctMethod));
      Trigger trigger1 = new Trigger();
      trigger1.Property = UIElement.IsMouseOverProperty;
      trigger1.Value = (object) true;
      Setter setter2 = new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "GridItem"));
      trigger1.Setters.Add(setter2);
      controlTemplate.Triggers.Add((TriggerBase) trigger1);
      Trigger trigger2 = new Trigger();
      trigger2.Property = ListBoxItem.IsSelectedProperty;
      trigger2.Value = (object) true;
      Setter setter3 = new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "GridItemHover"));
      trigger2.Setters.Add(setter3);
      controlTemplate.Triggers.Add((TriggerBase) trigger2);
      Setter setter4 = new Setter(Control.TemplateProperty, (object) controlTemplate);
      style.Setters.Add(setter4);
      elem.Resources.Add((object) typeof (ListBoxItem), (object) style);
    }

    private static UIElement e_9r_0_s_S_1_ctMethod(UIElement parent)
    {
      Border border = new Border();
      border.Parent = parent;
      border.Name = "e_12";
      border.SetBinding(Control.BackgroundProperty, new Binding("Background")
      {
        Source = (object) parent
      });
      ContentPresenter contentPresenter = new ContentPresenter();
      border.Child = (UIElement) contentPresenter;
      contentPresenter.Name = "e_13";
      contentPresenter.HorizontalAlignment = HorizontalAlignment.Stretch;
      contentPresenter.VerticalAlignment = VerticalAlignment.Stretch;
      Binding binding = new Binding();
      contentPresenter.SetBinding(ContentPresenter.ContentProperty, binding);
      return (UIElement) border;
    }
  }
}
