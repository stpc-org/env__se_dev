// Decompiled with JetBrains decompiler
// Type: EmptyKeys.UserInterface.Generated.PlayerTradeView_Gamepad
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Controls.Primitives;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.PlayerTradeView_Gamepad_Bindings;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Themes;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.Screens.ViewModels;
using System;
using System.CodeDom.Compiler;

namespace EmptyKeys.UserInterface.Generated
{
  [GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
  public class PlayerTradeView_Gamepad : UIRoot
  {
    private Grid rootGrid;
    private Image e_2;
    private Grid e_3;
    private ImageButton e_4;
    private StackPanel e_5;
    private TextBlock e_6;
    private Border e_7;
    private TextBlock e_8;
    private TextBlock e_9;
    private TextBlock e_10;
    private ListBox e_11;
    private ListBox e_16;
    private Grid e_23;
    private Slider e_24;
    private StackPanel e_25;
    private TextBlock e_26;
    private Image e_27;
    private Grid e_28;
    private Slider e_29;
    private TextBlock e_30;
    private Border e_31;
    private Grid e_32;
    private ListBox e_33;
    private StackPanel e_40;
    private TextBlock e_41;
    private TextBlock e_42;
    private StackPanel e_43;
    private TextBlock e_44;
    private TextBlock e_45;
    private Image e_46;
    private StackPanel e_47;
    private TextBlock e_48;
    private TextBlock e_49;
    private Grid e_50;
    private StackPanel e_51;
    private TextBlock e_52;
    private TextBlock e_53;
    private Image e_54;
    private StackPanel e_55;
    private TextBlock e_56;
    private TextBlock e_57;
    private Border e_58;
    private Grid ListHelp;
    private TextBlock e_59;
    private TextBlock e_60;
    private Grid e_61;
    private TextBlock e_62;
    private TextBlock e_63;
    private Grid SlidersHelp;
    private TextBlock e_64;
    private TextBlock e_65;
    private Grid e_66;
    private TextBlock e_67;
    private TextBlock e_68;

    public PlayerTradeView_Gamepad() => this.Initialize();

    public PlayerTradeView_Gamepad(int width, int height)
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
      PlayerTradeView_Gamepad.InitializeElementResources((UIElement) this);
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
      this.e_2 = new Image();
      this.rootGrid.Children.Add((UIElement) this.e_2);
      this.e_2.Name = "e_2";
      this.e_2.Source = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Screens\\screen_background.dds"
      };
      this.e_2.Stretch = Stretch.Fill;
      Grid.SetRow((UIElement) this.e_2, 1);
      this.e_2.SetBinding(ImageBrush.ColorOverlayProperty, new Binding("BackgroundOverlay")
      {
        UseGeneratedBindings = true
      });
      this.e_3 = new Grid();
      this.rootGrid.Children.Add((UIElement) this.e_3);
      this.e_3.Name = "e_3";
      GamepadBinding gamepadBinding1 = new GamepadBinding();
      gamepadBinding1.Gesture = (InputGesture) new GamepadGesture(GamepadInput.CButton);
      gamepadBinding1.SetBinding(InputBinding.CommandProperty, new Binding("SubmitAcceptCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_3.InputBindings.Add((InputBinding) gamepadBinding1);
      gamepadBinding1.Parent = (UIElement) this.e_3;
      this.e_3.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_3.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_3.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_3.RowDefinitions.Add(new RowDefinition());
      this.e_3.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_3.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_3.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_3.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(65f, GridUnitType.Pixel)
      });
      this.e_3.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(75f, GridUnitType.Pixel)
      });
      this.e_3.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(4f, GridUnitType.Star)
      });
      this.e_3.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(2f, GridUnitType.Star)
      });
      this.e_3.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_3.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(2f, GridUnitType.Star)
      });
      this.e_3.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(75f, GridUnitType.Pixel)
      });
      Grid.SetRow((UIElement) this.e_3, 1);
      this.e_4 = new ImageButton();
      this.e_3.Children.Add((UIElement) this.e_4);
      this.e_4.Name = "e_4";
      this.e_4.Height = 24f;
      this.e_4.Width = 24f;
      this.e_4.Margin = new Thickness(16f, 16f, 16f, 16f);
      this.e_4.HorizontalAlignment = HorizontalAlignment.Right;
      this.e_4.VerticalAlignment = VerticalAlignment.Center;
      this.e_4.IsTabStop = false;
      this.e_4.ImageStretch = Stretch.Uniform;
      this.e_4.ImageNormal = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol.dds"
      };
      this.e_4.ImageHover = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds"
      };
      this.e_4.ImagePressed = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds"
      };
      Grid.SetColumn((UIElement) this.e_4, 5);
      this.e_4.SetBinding(Button.CommandProperty, new Binding("ExitCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_5 = new StackPanel();
      this.e_3.Children.Add((UIElement) this.e_5);
      this.e_5.Name = "e_5";
      Grid.SetColumn((UIElement) this.e_5, 1);
      Grid.SetRow((UIElement) this.e_5, 1);
      Grid.SetColumnSpan((UIElement) this.e_5, 4);
      this.e_6 = new TextBlock();
      this.e_5.Children.Add((UIElement) this.e_6);
      this.e_6.Name = "e_6";
      this.e_6.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_6.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      this.e_6.SetResourceReference(TextBlock.TextProperty, (object) "ScreenCaptionPlayerTrade");
      this.e_7 = new Border();
      this.e_5.Children.Add((UIElement) this.e_7);
      this.e_7.Name = "e_7";
      this.e_7.Height = 2f;
      this.e_7.Margin = new Thickness(0.0f, 10f, 0.0f, 10f);
      this.e_7.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      this.e_8 = new TextBlock();
      this.e_3.Children.Add((UIElement) this.e_8);
      this.e_8.Name = "e_8";
      this.e_8.Margin = new Thickness(5f, 5f, 5f, 5f);
      this.e_8.HorizontalAlignment = HorizontalAlignment.Left;
      this.e_8.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) this.e_8, 1);
      Grid.SetRow((UIElement) this.e_8, 2);
      this.e_8.SetResourceReference(TextBlock.TextProperty, (object) "TradeScreenYoursInventory");
      this.e_9 = new TextBlock();
      this.e_3.Children.Add((UIElement) this.e_9);
      this.e_9.Name = "e_9";
      this.e_9.Margin = new Thickness(5f, 5f, 5f, 5f);
      this.e_9.HorizontalAlignment = HorizontalAlignment.Left;
      this.e_9.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) this.e_9, 2);
      Grid.SetRow((UIElement) this.e_9, 2);
      this.e_9.SetResourceReference(TextBlock.TextProperty, (object) "TradeScreenYoursOffer");
      this.e_10 = new TextBlock();
      this.e_3.Children.Add((UIElement) this.e_10);
      this.e_10.Name = "e_10";
      this.e_10.Margin = new Thickness(5f, 5f, 5f, 5f);
      this.e_10.HorizontalAlignment = HorizontalAlignment.Left;
      this.e_10.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) this.e_10, 4);
      Grid.SetRow((UIElement) this.e_10, 2);
      this.e_10.SetResourceReference(TextBlock.TextProperty, (object) "TradeScreenOtherOffer");
      this.e_11 = new ListBox();
      this.e_3.Children.Add((UIElement) this.e_11);
      this.e_11.Name = "e_11";
      this.e_11.Margin = new Thickness(0.0f, 10f, 10f, 10f);
      Style style = new Style(typeof (ListBox));
      Setter setter1 = new Setter(UIElement.MinHeightProperty, (object) 80f);
      style.Setters.Add(setter1);
      ControlTemplate controlTemplate = new ControlTemplate(typeof (ListBox), new Func<UIElement, UIElement>(PlayerTradeView_Gamepad.e_11_s_S_1_ctMethod));
      Setter setter2 = new Setter(Control.TemplateProperty, (object) controlTemplate);
      style.Setters.Add(setter2);
      Trigger trigger = new Trigger();
      trigger.Property = UIElement.IsFocusedProperty;
      trigger.Value = (object) true;
      Setter setter3 = new Setter(Control.BackgroundProperty, (object) new SolidColorBrush(new ColorW(67, 81, 92, 160)));
      trigger.Setters.Add(setter3);
      style.Triggers.Add((TriggerBase) trigger);
      this.e_11.Style = style;
      GamepadBinding gamepadBinding2 = new GamepadBinding();
      gamepadBinding2.Gesture = (InputGesture) new GamepadGesture(GamepadInput.AButton);
      gamepadBinding2.SetBinding(InputBinding.CommandProperty, new Binding("AddItemCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_11.InputBindings.Add((InputBinding) gamepadBinding2);
      gamepadBinding2.Parent = (UIElement) this.e_11;
      this.e_11.Background = (Brush) new SolidColorBrush(new ColorW(33, 44, 53, (int) byte.MaxValue));
      this.e_11.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      this.e_11.BorderThickness = new Thickness(2f, 2f, 2f, 2f);
      this.e_11.TabIndex = 0;
      Grid.SetColumn((UIElement) this.e_11, 1);
      Grid.SetRow((UIElement) this.e_11, 3);
      Grid.SetRowSpan((UIElement) this.e_11, 2);
      GamepadHelp.SetTargetName((DependencyObject) this.e_11, "ListHelp");
      GamepadHelp.SetTabIndexRight((DependencyObject) this.e_11, 1);
      DragDrop.SetIsDragSource((UIElement) this.e_11, true);
      DragDrop.SetIsDropTarget((UIElement) this.e_11, true);
      this.e_11.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("InventoryItems")
      {
        UseGeneratedBindings = true
      });
      this.e_11.SetBinding(Selector.SelectedItemProperty, new Binding("SelectedInventoryItem")
      {
        UseGeneratedBindings = true
      });
      PlayerTradeView_Gamepad.InitializeElemente_11Resources((UIElement) this.e_11);
      this.e_16 = new ListBox();
      this.e_3.Children.Add((UIElement) this.e_16);
      this.e_16.Name = "e_16";
      this.e_16.Margin = new Thickness(0.0f, 10f, 0.0f, 10f);
      GamepadBinding gamepadBinding3 = new GamepadBinding();
      gamepadBinding3.Gesture = (InputGesture) new GamepadGesture(GamepadInput.AButton);
      gamepadBinding3.SetBinding(InputBinding.CommandProperty, new Binding("RemoveItemCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_16.InputBindings.Add((InputBinding) gamepadBinding3);
      gamepadBinding3.Parent = (UIElement) this.e_16;
      this.e_16.Background = (Brush) new SolidColorBrush(new ColorW(33, 44, 53, (int) byte.MaxValue));
      this.e_16.TabIndex = 1;
      this.e_16.ItemTemplate = new DataTemplate((object) typeof (MyInventoryItemModel), new Func<UIElement, UIElement>(PlayerTradeView_Gamepad.e_16_dtMethod));
      Grid.SetColumn((UIElement) this.e_16, 2);
      Grid.SetRow((UIElement) this.e_16, 3);
      GamepadHelp.SetTargetName((DependencyObject) this.e_16, "ListHelp");
      GamepadHelp.SetTabIndexLeft((DependencyObject) this.e_16, 0);
      GamepadHelp.SetTabIndexDown((DependencyObject) this.e_16, 2);
      DragDrop.SetIsDragSource((UIElement) this.e_16, true);
      DragDrop.SetIsDropTarget((UIElement) this.e_16, true);
      this.e_16.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("LocalPlayerOfferItems")
      {
        UseGeneratedBindings = true
      });
      this.e_16.SetBinding(Selector.SelectedItemProperty, new Binding("SelectedOfferItem")
      {
        UseGeneratedBindings = true
      });
      this.e_23 = new Grid();
      this.e_3.Children.Add((UIElement) this.e_23);
      this.e_23.Name = "e_23";
      this.e_23.RowDefinitions.Add(new RowDefinition());
      this.e_23.RowDefinitions.Add(new RowDefinition());
      Grid.SetColumn((UIElement) this.e_23, 2);
      Grid.SetRow((UIElement) this.e_23, 4);
      this.e_24 = new Slider();
      this.e_23.Children.Add((UIElement) this.e_24);
      this.e_24.Name = "e_24";
      this.e_24.Margin = new Thickness(5f, 5f, 10f, 5f);
      this.e_24.TabIndex = 2;
      this.e_24.Minimum = 0.0f;
      this.e_24.IsSnapToTickEnabled = true;
      this.e_24.TickFrequency = 1f;
      GamepadHelp.SetTargetName((DependencyObject) this.e_24, "SlidersHelp");
      GamepadHelp.SetTabIndexLeft((DependencyObject) this.e_24, 0);
      GamepadHelp.SetTabIndexUp((DependencyObject) this.e_24, 1);
      GamepadHelp.SetTabIndexDown((DependencyObject) this.e_24, 3);
      this.e_24.SetBinding(RangeBase.MaximumProperty, new Binding("LocalPlayerOfferCurrencyMaximum")
      {
        UseGeneratedBindings = true
      });
      this.e_24.SetBinding(RangeBase.ValueProperty, new Binding("LocalPlayerOfferCurrency")
      {
        UseGeneratedBindings = true
      });
      this.e_25 = new StackPanel();
      this.e_23.Children.Add((UIElement) this.e_25);
      this.e_25.Name = "e_25";
      this.e_25.HorizontalAlignment = HorizontalAlignment.Right;
      this.e_25.Orientation = Orientation.Horizontal;
      Grid.SetRow((UIElement) this.e_25, 1);
      this.e_26 = new TextBlock();
      this.e_25.Children.Add((UIElement) this.e_26);
      this.e_26.Name = "e_26";
      this.e_26.Margin = new Thickness(5f, 5f, 0.0f, 5f);
      this.e_26.HorizontalAlignment = HorizontalAlignment.Right;
      this.e_26.VerticalAlignment = VerticalAlignment.Center;
      this.e_26.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      this.e_26.SetBinding(TextBlock.TextProperty, new Binding("LocalPlayerOfferCurrency")
      {
        UseGeneratedBindings = true
      });
      this.e_27 = new Image();
      this.e_25.Children.Add((UIElement) this.e_27);
      this.e_27.Name = "e_27";
      this.e_27.Height = 20f;
      this.e_27.Margin = new Thickness(4f, 2f, 2f, 2f);
      this.e_27.VerticalAlignment = VerticalAlignment.Center;
      this.e_27.Stretch = Stretch.Uniform;
      this.e_27.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      this.e_28 = new Grid();
      this.e_3.Children.Add((UIElement) this.e_28);
      this.e_28.Name = "e_28";
      this.e_28.ColumnDefinitions.Add(new ColumnDefinition());
      this.e_28.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(60f, GridUnitType.Pixel)
      });
      Grid.SetColumn((UIElement) this.e_28, 2);
      Grid.SetRow((UIElement) this.e_28, 5);
      this.e_28.SetBinding(UIElement.VisibilityProperty, new Binding("IsPcuVisible")
      {
        UseGeneratedBindings = true
      });
      this.e_29 = new Slider();
      this.e_28.Children.Add((UIElement) this.e_29);
      this.e_29.Name = "e_29";
      this.e_29.Margin = new Thickness(5f, 5f, 10f, 5f);
      this.e_29.TabIndex = 3;
      this.e_29.Minimum = 0.0f;
      this.e_29.Maximum = 99999f;
      this.e_29.IsSnapToTickEnabled = true;
      this.e_29.TickFrequency = 1f;
      GamepadHelp.SetTargetName((DependencyObject) this.e_29, "SlidersHelp");
      GamepadHelp.SetTabIndexLeft((DependencyObject) this.e_29, 0);
      GamepadHelp.SetTabIndexUp((DependencyObject) this.e_29, 2);
      this.e_29.SetBinding(RangeBase.ValueProperty, new Binding("LocalPlayerOfferPcu")
      {
        UseGeneratedBindings = true
      });
      this.e_30 = new TextBlock();
      this.e_28.Children.Add((UIElement) this.e_30);
      this.e_30.Name = "e_30";
      this.e_30.Margin = new Thickness(5f, 5f, 0.0f, 5f);
      this.e_30.HorizontalAlignment = HorizontalAlignment.Right;
      this.e_30.VerticalAlignment = VerticalAlignment.Center;
      this.e_30.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) this.e_30, 1);
      this.e_30.SetBinding(TextBlock.TextProperty, new Binding("LocalPlayerOfferPcu")
      {
        UseGeneratedBindings = true
      });
      this.e_31 = new Border();
      this.e_3.Children.Add((UIElement) this.e_31);
      this.e_31.Name = "e_31";
      this.e_31.Width = 2f;
      this.e_31.Margin = new Thickness(5f, 10f, 5f, 10f);
      this.e_31.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) this.e_31, 3);
      Grid.SetRow((UIElement) this.e_31, 2);
      Grid.SetRowSpan((UIElement) this.e_31, 4);
      this.e_32 = new Grid();
      this.e_3.Children.Add((UIElement) this.e_32);
      this.e_32.Name = "e_32";
      this.e_32.RowDefinitions.Add(new RowDefinition());
      this.e_32.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      Grid.SetColumn((UIElement) this.e_32, 4);
      Grid.SetRow((UIElement) this.e_32, 3);
      this.e_33 = new ListBox();
      this.e_32.Children.Add((UIElement) this.e_33);
      this.e_33.Name = "e_33";
      this.e_33.Margin = new Thickness(0.0f, 10f, 0.0f, 10f);
      this.e_33.Background = (Brush) new SolidColorBrush(new ColorW(33, 44, 53, (int) byte.MaxValue));
      this.e_33.TabIndex = 4;
      this.e_33.ItemTemplate = new DataTemplate((object) typeof (MyInventoryItemModel), new Func<UIElement, UIElement>(PlayerTradeView_Gamepad.e_33_dtMethod));
      Grid.SetRow((UIElement) this.e_33, 0);
      this.e_33.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("OtherPlayerOfferItems")
      {
        UseGeneratedBindings = true
      });
      this.e_40 = new StackPanel();
      this.e_32.Children.Add((UIElement) this.e_40);
      this.e_40.Name = "e_40";
      this.e_40.VerticalAlignment = VerticalAlignment.Bottom;
      this.e_40.Orientation = Orientation.Horizontal;
      Grid.SetRow((UIElement) this.e_40, 1);
      this.e_41 = new TextBlock();
      this.e_40.Children.Add((UIElement) this.e_41);
      this.e_41.Name = "e_41";
      this.e_41.Margin = new Thickness(5f, 5f, 5f, 5f);
      this.e_41.VerticalAlignment = VerticalAlignment.Center;
      this.e_41.SetResourceReference(TextBlock.TextProperty, (object) "TradeScreenOfferState");
      this.e_42 = new TextBlock();
      this.e_40.Children.Add((UIElement) this.e_42);
      this.e_42.Name = "e_42";
      this.e_42.Margin = new Thickness(5f, 5f, 10f, 5f);
      this.e_42.VerticalAlignment = VerticalAlignment.Center;
      this.e_42.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      this.e_42.SetBinding(TextBlock.TextProperty, new Binding("OtherPlayerAcceptState")
      {
        UseGeneratedBindings = true
      });
      this.e_43 = new StackPanel();
      this.e_3.Children.Add((UIElement) this.e_43);
      this.e_43.Name = "e_43";
      this.e_43.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) this.e_43, 4);
      Grid.SetRow((UIElement) this.e_43, 4);
      this.e_44 = new TextBlock();
      this.e_43.Children.Add((UIElement) this.e_44);
      this.e_44.Name = "e_44";
      this.e_44.Margin = new Thickness(5f, 5f, 5f, 5f);
      this.e_44.VerticalAlignment = VerticalAlignment.Center;
      this.e_44.SetResourceReference(TextBlock.TextProperty, (object) "TradeScreenMoney");
      this.e_45 = new TextBlock();
      this.e_43.Children.Add((UIElement) this.e_45);
      this.e_45.Name = "e_45";
      this.e_45.Margin = new Thickness(5f, 5f, 0.0f, 5f);
      this.e_45.VerticalAlignment = VerticalAlignment.Center;
      this.e_45.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      this.e_45.SetBinding(TextBlock.TextProperty, new Binding("OtherPlayerOfferCurrency")
      {
        UseGeneratedBindings = true
      });
      this.e_46 = new Image();
      this.e_43.Children.Add((UIElement) this.e_46);
      this.e_46.Name = "e_46";
      this.e_46.Height = 20f;
      this.e_46.Margin = new Thickness(3f, 2f, 2f, 0.0f);
      this.e_46.VerticalAlignment = VerticalAlignment.Center;
      this.e_46.Stretch = Stretch.Uniform;
      this.e_46.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      this.e_47 = new StackPanel();
      this.e_3.Children.Add((UIElement) this.e_47);
      this.e_47.Name = "e_47";
      this.e_47.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) this.e_47, 4);
      Grid.SetRow((UIElement) this.e_47, 5);
      this.e_47.SetBinding(UIElement.VisibilityProperty, new Binding("IsPcuVisible")
      {
        UseGeneratedBindings = true
      });
      this.e_48 = new TextBlock();
      this.e_47.Children.Add((UIElement) this.e_48);
      this.e_48.Name = "e_48";
      this.e_48.Margin = new Thickness(5f, 5f, 5f, 5f);
      this.e_48.VerticalAlignment = VerticalAlignment.Center;
      this.e_48.SetResourceReference(TextBlock.TextProperty, (object) "TradeScreenPcu");
      this.e_49 = new TextBlock();
      this.e_47.Children.Add((UIElement) this.e_49);
      this.e_49.Name = "e_49";
      this.e_49.Margin = new Thickness(5f, 5f, 10f, 5f);
      this.e_49.VerticalAlignment = VerticalAlignment.Center;
      this.e_49.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      this.e_49.SetBinding(TextBlock.TextProperty, new Binding("OtherPlayerOfferPcu")
      {
        UseGeneratedBindings = true
      });
      this.e_50 = new Grid();
      this.e_3.Children.Add((UIElement) this.e_50);
      this.e_50.Name = "e_50";
      this.e_50.ColumnDefinitions.Add(new ColumnDefinition());
      this.e_50.ColumnDefinitions.Add(new ColumnDefinition());
      Grid.SetColumn((UIElement) this.e_50, 1);
      Grid.SetRow((UIElement) this.e_50, 5);
      this.e_51 = new StackPanel();
      this.e_50.Children.Add((UIElement) this.e_51);
      this.e_51.Name = "e_51";
      this.e_51.HorizontalAlignment = HorizontalAlignment.Left;
      this.e_51.Orientation = Orientation.Horizontal;
      this.e_52 = new TextBlock();
      this.e_51.Children.Add((UIElement) this.e_52);
      this.e_52.Name = "e_52";
      this.e_52.Margin = new Thickness(0.0f, 5f, 5f, 5f);
      this.e_52.VerticalAlignment = VerticalAlignment.Center;
      this.e_52.SetResourceReference(TextBlock.TextProperty, (object) "TradeScreenMoney");
      this.e_53 = new TextBlock();
      this.e_51.Children.Add((UIElement) this.e_53);
      this.e_53.Name = "e_53";
      this.e_53.Margin = new Thickness(5f, 5f, 0.0f, 5f);
      this.e_53.VerticalAlignment = VerticalAlignment.Center;
      this.e_53.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      this.e_53.SetBinding(TextBlock.TextProperty, new Binding("LocalPlayerCurrency")
      {
        UseGeneratedBindings = true
      });
      this.e_54 = new Image();
      this.e_51.Children.Add((UIElement) this.e_54);
      this.e_54.Name = "e_54";
      this.e_54.Height = 20f;
      this.e_54.Margin = new Thickness(3f, 2f, 2f, 0.0f);
      this.e_54.VerticalAlignment = VerticalAlignment.Center;
      this.e_54.Stretch = Stretch.Uniform;
      this.e_54.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      this.e_55 = new StackPanel();
      this.e_50.Children.Add((UIElement) this.e_55);
      this.e_55.Name = "e_55";
      this.e_55.HorizontalAlignment = HorizontalAlignment.Left;
      this.e_55.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) this.e_55, 1);
      this.e_55.SetBinding(UIElement.VisibilityProperty, new Binding("IsPcuVisible")
      {
        UseGeneratedBindings = true
      });
      this.e_56 = new TextBlock();
      this.e_55.Children.Add((UIElement) this.e_56);
      this.e_56.Name = "e_56";
      this.e_56.Margin = new Thickness(0.0f, 5f, 5f, 5f);
      this.e_56.VerticalAlignment = VerticalAlignment.Center;
      this.e_56.SetResourceReference(TextBlock.TextProperty, (object) "TradeScreenPcu");
      this.e_57 = new TextBlock();
      this.e_55.Children.Add((UIElement) this.e_57);
      this.e_57.Name = "e_57";
      this.e_57.Margin = new Thickness(5f, 5f, 5f, 5f);
      this.e_57.VerticalAlignment = VerticalAlignment.Center;
      this.e_57.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      this.e_57.SetBinding(TextBlock.TextProperty, new Binding("LocalPlayerPcu")
      {
        UseGeneratedBindings = true
      });
      this.e_58 = new Border();
      this.e_3.Children.Add((UIElement) this.e_58);
      this.e_58.Name = "e_58";
      this.e_58.Height = 2f;
      this.e_58.Margin = new Thickness(0.0f, 10f, 0.0f, 0.0f);
      this.e_58.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) this.e_58, 1);
      Grid.SetRow((UIElement) this.e_58, 6);
      Grid.SetColumnSpan((UIElement) this.e_58, 4);
      this.ListHelp = new Grid();
      this.e_3.Children.Add((UIElement) this.ListHelp);
      this.ListHelp.Name = "ListHelp";
      this.ListHelp.Visibility = Visibility.Collapsed;
      this.ListHelp.VerticalAlignment = VerticalAlignment.Center;
      this.ListHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.ListHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.ListHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.ListHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.ListHelp.ColumnDefinitions.Add(new ColumnDefinition());
      Grid.SetColumn((UIElement) this.ListHelp, 1);
      Grid.SetRow((UIElement) this.ListHelp, 7);
      Grid.SetColumnSpan((UIElement) this.ListHelp, 3);
      this.e_59 = new TextBlock();
      this.ListHelp.Children.Add((UIElement) this.e_59);
      this.e_59.Name = "e_59";
      this.e_59.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      this.e_59.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_59.VerticalAlignment = VerticalAlignment.Center;
      this.e_59.SetResourceReference(TextBlock.TextProperty, (object) "PlayerTrade_Help_Transfer");
      this.e_60 = new TextBlock();
      this.ListHelp.Children.Add((UIElement) this.e_60);
      this.e_60.Name = "e_60";
      this.e_60.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      this.e_60.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_60.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) this.e_60, 1);
      this.e_60.SetBinding(UIElement.VisibilityProperty, new Binding("IsSubmitOfferEnabled")
      {
        UseGeneratedBindings = true
      });
      this.e_60.SetResourceReference(TextBlock.TextProperty, (object) "PlayerTrade_Help_SubmitOffer");
      this.e_61 = new Grid();
      this.ListHelp.Children.Add((UIElement) this.e_61);
      this.e_61.Name = "e_61";
      Grid.SetColumn((UIElement) this.e_61, 1);
      this.e_61.SetBinding(UIElement.VisibilityProperty, new Binding("IsAcceptVisible")
      {
        UseGeneratedBindings = true
      });
      this.e_62 = new TextBlock();
      this.e_61.Children.Add((UIElement) this.e_62);
      this.e_62.Name = "e_62";
      this.e_62.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      this.e_62.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_62.VerticalAlignment = VerticalAlignment.Center;
      this.e_62.SetBinding(UIElement.VisibilityProperty, new Binding("IsAcceptEnabled")
      {
        UseGeneratedBindings = true
      });
      this.e_62.SetResourceReference(TextBlock.TextProperty, (object) "PlayerTrade_Help_AcceptOffer");
      this.e_63 = new TextBlock();
      this.ListHelp.Children.Add((UIElement) this.e_63);
      this.e_63.Name = "e_63";
      this.e_63.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      this.e_63.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_63.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) this.e_63, 2);
      this.e_63.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Back");
      this.SlidersHelp = new Grid();
      this.e_3.Children.Add((UIElement) this.SlidersHelp);
      this.SlidersHelp.Name = "SlidersHelp";
      this.SlidersHelp.Visibility = Visibility.Collapsed;
      this.SlidersHelp.VerticalAlignment = VerticalAlignment.Center;
      this.SlidersHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.SlidersHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.SlidersHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.SlidersHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.SlidersHelp.ColumnDefinitions.Add(new ColumnDefinition());
      Grid.SetColumn((UIElement) this.SlidersHelp, 1);
      Grid.SetRow((UIElement) this.SlidersHelp, 7);
      Grid.SetColumnSpan((UIElement) this.SlidersHelp, 3);
      this.e_64 = new TextBlock();
      this.SlidersHelp.Children.Add((UIElement) this.e_64);
      this.e_64.Name = "e_64";
      this.e_64.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      this.e_64.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_64.VerticalAlignment = VerticalAlignment.Center;
      this.e_64.SetResourceReference(TextBlock.TextProperty, (object) "PlayerTrade_Help_ChangeValue");
      this.e_65 = new TextBlock();
      this.SlidersHelp.Children.Add((UIElement) this.e_65);
      this.e_65.Name = "e_65";
      this.e_65.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      this.e_65.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_65.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) this.e_65, 1);
      this.e_65.SetBinding(UIElement.VisibilityProperty, new Binding("IsSubmitOfferEnabled")
      {
        UseGeneratedBindings = true
      });
      this.e_65.SetResourceReference(TextBlock.TextProperty, (object) "PlayerTrade_Help_SubmitOffer");
      this.e_66 = new Grid();
      this.SlidersHelp.Children.Add((UIElement) this.e_66);
      this.e_66.Name = "e_66";
      Grid.SetColumn((UIElement) this.e_66, 1);
      this.e_66.SetBinding(UIElement.VisibilityProperty, new Binding("IsAcceptVisible")
      {
        UseGeneratedBindings = true
      });
      this.e_67 = new TextBlock();
      this.e_66.Children.Add((UIElement) this.e_67);
      this.e_67.Name = "e_67";
      this.e_67.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      this.e_67.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_67.VerticalAlignment = VerticalAlignment.Center;
      this.e_67.SetBinding(UIElement.VisibilityProperty, new Binding("IsAcceptEnabled")
      {
        UseGeneratedBindings = true
      });
      this.e_67.SetResourceReference(TextBlock.TextProperty, (object) "PlayerTrade_Help_AcceptOffer");
      this.e_68 = new TextBlock();
      this.SlidersHelp.Children.Add((UIElement) this.e_68);
      this.e_68.Name = "e_68";
      this.e_68.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      this.e_68.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_68.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) this.e_68, 2);
      this.e_68.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Back");
      ImageManager.Instance.AddImage("Textures\\GUI\\Screens\\screen_background.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol_highlight.dds");
      FontManager.Instance.AddFont("InventorySmall", 10f, FontStyle.Regular, "InventorySmall_7.5_Regular");
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "MaxWidth", typeof (MyPlayerTradeViewModel_MaxWidth_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "BackgroundOverlay", typeof (MyPlayerTradeViewModel_BackgroundOverlay_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "SubmitAcceptCommand", typeof (MyPlayerTradeViewModel_SubmitAcceptCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "ExitCommand", typeof (MyPlayerTradeViewModel_ExitCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "AddItemCommand", typeof (MyPlayerTradeViewModel_AddItemCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "InventoryItems", typeof (MyPlayerTradeViewModel_InventoryItems_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "SelectedInventoryItem", typeof (MyPlayerTradeViewModel_SelectedInventoryItem_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "RemoveItemCommand", typeof (MyPlayerTradeViewModel_RemoveItemCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryItemModel), "Icon", typeof (MyInventoryItemModel_Icon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryItemModel), "AmountFormatted", typeof (MyInventoryItemModel_AmountFormatted_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryItemModel), "IconSymbol", typeof (MyInventoryItemModel_IconSymbol_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryItemModel), "Name", typeof (MyInventoryItemModel_Name_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "LocalPlayerOfferItems", typeof (MyPlayerTradeViewModel_LocalPlayerOfferItems_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "SelectedOfferItem", typeof (MyPlayerTradeViewModel_SelectedOfferItem_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "LocalPlayerOfferCurrencyMaximum", typeof (MyPlayerTradeViewModel_LocalPlayerOfferCurrencyMaximum_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "LocalPlayerOfferCurrency", typeof (MyPlayerTradeViewModel_LocalPlayerOfferCurrency_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "CurrencyIcon", typeof (MyPlayerTradeViewModel_CurrencyIcon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "IsPcuVisible", typeof (MyPlayerTradeViewModel_IsPcuVisible_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "LocalPlayerOfferPcu", typeof (MyPlayerTradeViewModel_LocalPlayerOfferPcu_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "OtherPlayerOfferItems", typeof (MyPlayerTradeViewModel_OtherPlayerOfferItems_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "OtherPlayerAcceptState", typeof (MyPlayerTradeViewModel_OtherPlayerAcceptState_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "OtherPlayerOfferCurrency", typeof (MyPlayerTradeViewModel_OtherPlayerOfferCurrency_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "OtherPlayerOfferPcu", typeof (MyPlayerTradeViewModel_OtherPlayerOfferPcu_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "LocalPlayerCurrency", typeof (MyPlayerTradeViewModel_LocalPlayerCurrency_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "LocalPlayerPcu", typeof (MyPlayerTradeViewModel_LocalPlayerPcu_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "IsSubmitOfferEnabled", typeof (MyPlayerTradeViewModel_IsSubmitOfferEnabled_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "IsAcceptVisible", typeof (MyPlayerTradeViewModel_IsAcceptVisible_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "IsAcceptEnabled", typeof (MyPlayerTradeViewModel_IsAcceptEnabled_PropertyInfo));
    }

    private static void InitializeElementResources(UIElement elem)
    {
      elem.Resources.MergedDictionaries.Add((ResourceDictionary) Styles.Instance);
      elem.Resources.MergedDictionaries.Add((ResourceDictionary) DataTemplates.Instance);
      Style style = new Style(typeof (ListBox));
      Setter setter1 = new Setter(UIElement.MinHeightProperty, (object) 80f);
      style.Setters.Add(setter1);
      ControlTemplate controlTemplate = new ControlTemplate(typeof (ListBox), new Func<UIElement, UIElement>(PlayerTradeView_Gamepad.r_0_s_S_1_ctMethod));
      Setter setter2 = new Setter(Control.TemplateProperty, (object) controlTemplate);
      style.Setters.Add(setter2);
      Trigger trigger = new Trigger();
      trigger.Property = UIElement.IsFocusedProperty;
      trigger.Value = (object) true;
      Setter setter3 = new Setter(Control.BackgroundProperty, (object) new SolidColorBrush(new ColorW(67, 81, 92, 160)));
      trigger.Setters.Add(setter3);
      style.Triggers.Add((TriggerBase) trigger);
      elem.Resources.Add((object) "ListBoxGridLarge", (object) style);
    }

    private static UIElement r_0_s_S_1_ctMethod(UIElement parent)
    {
      Border border = new Border();
      border.Parent = parent;
      border.Name = "e_0";
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
      uniformGrid.Name = "e_1";
      uniformGrid.Margin = new Thickness(4f, 4f, 4f, 4f);
      uniformGrid.VerticalAlignment = VerticalAlignment.Top;
      uniformGrid.IsItemsHost = true;
      uniformGrid.Columns = 7;
      return (UIElement) border;
    }

    private static UIElement e_11_s_S_1_ctMethod(UIElement parent)
    {
      Border border = new Border();
      border.Parent = parent;
      border.Name = "e_12";
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
      uniformGrid.Name = "e_13";
      uniformGrid.Margin = new Thickness(4f, 4f, 4f, 4f);
      uniformGrid.VerticalAlignment = VerticalAlignment.Top;
      uniformGrid.IsItemsHost = true;
      uniformGrid.Columns = 7;
      return (UIElement) border;
    }

    private static void InitializeElemente_11Resources(UIElement elem)
    {
      Style style = new Style(typeof (ListBoxItem));
      Setter setter1 = new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "GridItem"));
      style.Setters.Add(setter1);
      ControlTemplate controlTemplate = new ControlTemplate(typeof (ListBoxItem), new Func<UIElement, UIElement>(PlayerTradeView_Gamepad.e_11r_0_s_S_1_ctMethod));
      Trigger trigger1 = new Trigger();
      trigger1.Property = UIElement.IsMouseOverProperty;
      trigger1.Value = (object) true;
      Setter setter2 = new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "GridItemHover"));
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

    private static UIElement e_11r_0_s_S_1_ctMethod(UIElement parent)
    {
      Border border = new Border();
      border.Parent = parent;
      border.Name = "e_14";
      border.SetBinding(Control.BackgroundProperty, new Binding("Background")
      {
        Source = (object) parent
      });
      ContentPresenter contentPresenter = new ContentPresenter();
      border.Child = (UIElement) contentPresenter;
      contentPresenter.Name = "e_15";
      contentPresenter.HorizontalAlignment = HorizontalAlignment.Stretch;
      contentPresenter.VerticalAlignment = VerticalAlignment.Stretch;
      Binding binding = new Binding();
      contentPresenter.SetBinding(ContentPresenter.ContentProperty, binding);
      return (UIElement) border;
    }

    private static UIElement e_16_dtMethod(UIElement parent)
    {
      Grid grid = new Grid();
      grid.Parent = parent;
      grid.Name = "e_17";
      grid.HorizontalAlignment = HorizontalAlignment.Stretch;
      MouseBinding mouseBinding1 = new MouseBinding();
      mouseBinding1.Gesture = (InputGesture) new MouseGesture(MouseAction.LeftDoubleClick, ModifierKeys.None);
      mouseBinding1.SetBinding(InputBinding.CommandProperty, new Binding("ViewModel.RemoveItemFromOfferCommand")
      {
        Source = (object) new MyPlayerTradeViewModelLocator(false)
      });
      Binding binding1 = new Binding();
      mouseBinding1.SetBinding(InputBinding.CommandParameterProperty, binding1);
      grid.InputBindings.Add((InputBinding) mouseBinding1);
      mouseBinding1.Parent = (UIElement) grid;
      MouseBinding mouseBinding2 = new MouseBinding();
      mouseBinding2.Gesture = (InputGesture) new MouseGesture(MouseAction.LeftDoubleClick, ModifierKeys.Control);
      mouseBinding2.SetBinding(InputBinding.CommandProperty, new Binding("ViewModel.RemoveStackTenToOfferCommand")
      {
        Source = (object) new MyPlayerTradeViewModelLocator(false)
      });
      Binding binding2 = new Binding();
      mouseBinding2.SetBinding(InputBinding.CommandParameterProperty, binding2);
      grid.InputBindings.Add((InputBinding) mouseBinding2);
      mouseBinding2.Parent = (UIElement) grid;
      MouseBinding mouseBinding3 = new MouseBinding();
      mouseBinding3.Gesture = (InputGesture) new MouseGesture(MouseAction.LeftDoubleClick, ModifierKeys.Shift);
      mouseBinding3.SetBinding(InputBinding.CommandProperty, new Binding("ViewModel.RemoveStackHundredToOfferCommand")
      {
        Source = (object) new MyPlayerTradeViewModelLocator(false)
      });
      Binding binding3 = new Binding();
      mouseBinding3.SetBinding(InputBinding.CommandParameterProperty, binding3);
      grid.InputBindings.Add((InputBinding) mouseBinding3);
      mouseBinding3.Parent = (UIElement) grid;
      grid.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition = new ColumnDefinition();
      grid.ColumnDefinitions.Add(columnDefinition);
      Border border = new Border();
      grid.Children.Add((UIElement) border);
      border.Name = "e_18";
      border.Height = 64f;
      border.Width = 64f;
      border.SetResourceReference(Control.BackgroundProperty, (object) "GridItem");
      Image image = new Image();
      border.Child = (UIElement) image;
      image.Name = "e_19";
      image.Margin = new Thickness(5f, 5f, 5f, 5f);
      image.Stretch = Stretch.Fill;
      image.SetBinding(Image.SourceProperty, new Binding("Icon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock1 = new TextBlock();
      grid.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_20";
      textBlock1.Margin = new Thickness(6f, 0.0f, 0.0f, 4f);
      textBlock1.HorizontalAlignment = HorizontalAlignment.Left;
      textBlock1.VerticalAlignment = VerticalAlignment.Bottom;
      textBlock1.TextAlignment = TextAlignment.Left;
      textBlock1.FontFamily = new FontFamily("InventorySmall");
      textBlock1.FontSize = 10f;
      textBlock1.FontStyle = FontStyle.Regular;
      textBlock1.SetBinding(TextBlock.TextProperty, new Binding("AmountFormatted")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock2 = new TextBlock();
      grid.Children.Add((UIElement) textBlock2);
      textBlock2.Name = "e_21";
      textBlock2.Margin = new Thickness(6f, 4f, 0.0f, 0.0f);
      textBlock2.HorizontalAlignment = HorizontalAlignment.Left;
      textBlock2.VerticalAlignment = VerticalAlignment.Top;
      textBlock2.TextAlignment = TextAlignment.Left;
      textBlock2.FontFamily = new FontFamily("InventorySmall");
      textBlock2.FontSize = 10f;
      textBlock2.SetBinding(TextBlock.TextProperty, new Binding("IconSymbol")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock3 = new TextBlock();
      grid.Children.Add((UIElement) textBlock3);
      textBlock3.Name = "e_22";
      textBlock3.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock3, 1);
      textBlock3.SetBinding(TextBlock.TextProperty, new Binding("Name")
      {
        UseGeneratedBindings = true
      });
      return (UIElement) grid;
    }

    private static UIElement e_33_dtMethod(UIElement parent)
    {
      Grid grid = new Grid();
      grid.Parent = parent;
      grid.Name = "e_34";
      grid.HorizontalAlignment = HorizontalAlignment.Stretch;
      grid.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid.ColumnDefinitions.Add(new ColumnDefinition());
      Border border = new Border();
      grid.Children.Add((UIElement) border);
      border.Name = "e_35";
      border.Height = 64f;
      border.Width = 64f;
      border.SetResourceReference(Control.BackgroundProperty, (object) "GridItem");
      Image image = new Image();
      border.Child = (UIElement) image;
      image.Name = "e_36";
      image.Margin = new Thickness(5f, 5f, 5f, 5f);
      image.Stretch = Stretch.Fill;
      image.SetBinding(Image.SourceProperty, new Binding("Icon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock1 = new TextBlock();
      grid.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_37";
      textBlock1.Margin = new Thickness(6f, 0.0f, 0.0f, 4f);
      textBlock1.HorizontalAlignment = HorizontalAlignment.Left;
      textBlock1.VerticalAlignment = VerticalAlignment.Bottom;
      textBlock1.TextAlignment = TextAlignment.Left;
      textBlock1.FontFamily = new FontFamily("InventorySmall");
      textBlock1.FontSize = 10f;
      textBlock1.FontStyle = FontStyle.Regular;
      textBlock1.SetBinding(TextBlock.TextProperty, new Binding("AmountFormatted")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock2 = new TextBlock();
      grid.Children.Add((UIElement) textBlock2);
      textBlock2.Name = "e_38";
      textBlock2.Margin = new Thickness(6f, 4f, 0.0f, 0.0f);
      textBlock2.HorizontalAlignment = HorizontalAlignment.Left;
      textBlock2.VerticalAlignment = VerticalAlignment.Top;
      textBlock2.TextAlignment = TextAlignment.Left;
      textBlock2.FontFamily = new FontFamily("InventorySmall");
      textBlock2.FontSize = 10f;
      textBlock2.SetBinding(TextBlock.TextProperty, new Binding("IconSymbol")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock3 = new TextBlock();
      grid.Children.Add((UIElement) textBlock3);
      textBlock3.Name = "e_39";
      textBlock3.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock3, 1);
      textBlock3.SetBinding(TextBlock.TextProperty, new Binding("Name")
      {
        UseGeneratedBindings = true
      });
      return (UIElement) grid;
    }
  }
}
