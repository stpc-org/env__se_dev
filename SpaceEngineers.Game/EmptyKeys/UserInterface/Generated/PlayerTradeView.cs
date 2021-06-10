// Decompiled with JetBrains decompiler
// Type: EmptyKeys.UserInterface.Generated.PlayerTradeView
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.PlayerTradeView_Bindings;
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
  public class PlayerTradeView : UIRoot
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
    private TextBlock e_8;
    private ListBox e_9;
    private ListBox e_14;
    private StackPanel e_21;
    private NumericTextBox e_22;
    private StackPanel e_23;
    private NumericTextBox e_24;
    private Border e_25;
    private Grid e_26;
    private ListBox e_27;
    private StackPanel e_34;
    private TextBlock e_35;
    private TextBlock e_36;
    private StackPanel e_37;
    private TextBlock e_38;
    private TextBlock e_39;
    private Image e_40;
    private StackPanel e_41;
    private TextBlock e_42;
    private TextBlock e_43;
    private Grid e_44;
    private StackPanel e_45;
    private TextBlock e_46;
    private TextBlock e_47;
    private Image e_48;
    private StackPanel e_49;
    private TextBlock e_50;
    private TextBlock e_51;
    private Border e_52;
    private StackPanel e_53;
    private Button e_54;
    private Button e_55;
    private Button e_56;

    public PlayerTradeView() => this.Initialize();

    public PlayerTradeView(int width, int height)
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
      PlayerTradeView.InitializeElementResources((UIElement) this);
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
        Width = new GridLength(2f, GridUnitType.Star)
      });
      this.e_1.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
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
      Grid.SetColumn((UIElement) this.e_2, 5);
      this.e_2.SetBinding(Button.CommandProperty, new Binding("ExitCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_3 = new StackPanel();
      this.e_1.Children.Add((UIElement) this.e_3);
      this.e_3.Name = "e_3";
      Grid.SetColumn((UIElement) this.e_3, 1);
      Grid.SetRow((UIElement) this.e_3, 1);
      Grid.SetColumnSpan((UIElement) this.e_3, 4);
      this.e_4 = new TextBlock();
      this.e_3.Children.Add((UIElement) this.e_4);
      this.e_4.Name = "e_4";
      this.e_4.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_4.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      this.e_4.SetResourceReference(TextBlock.TextProperty, (object) "ScreenCaptionPlayerTrade");
      this.e_5 = new Border();
      this.e_3.Children.Add((UIElement) this.e_5);
      this.e_5.Name = "e_5";
      this.e_5.Height = 2f;
      this.e_5.Margin = new Thickness(0.0f, 10f, 0.0f, 10f);
      this.e_5.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      this.e_6 = new TextBlock();
      this.e_1.Children.Add((UIElement) this.e_6);
      this.e_6.Name = "e_6";
      this.e_6.Margin = new Thickness(5f, 5f, 5f, 5f);
      this.e_6.HorizontalAlignment = HorizontalAlignment.Left;
      this.e_6.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) this.e_6, 1);
      Grid.SetRow((UIElement) this.e_6, 2);
      this.e_6.SetResourceReference(TextBlock.TextProperty, (object) "TradeScreenYoursInventory");
      this.e_7 = new TextBlock();
      this.e_1.Children.Add((UIElement) this.e_7);
      this.e_7.Name = "e_7";
      this.e_7.Margin = new Thickness(5f, 5f, 5f, 5f);
      this.e_7.HorizontalAlignment = HorizontalAlignment.Left;
      this.e_7.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) this.e_7, 2);
      Grid.SetRow((UIElement) this.e_7, 2);
      this.e_7.SetResourceReference(TextBlock.TextProperty, (object) "TradeScreenYoursOffer");
      this.e_8 = new TextBlock();
      this.e_1.Children.Add((UIElement) this.e_8);
      this.e_8.Name = "e_8";
      this.e_8.Margin = new Thickness(5f, 5f, 5f, 5f);
      this.e_8.HorizontalAlignment = HorizontalAlignment.Left;
      this.e_8.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) this.e_8, 4);
      Grid.SetRow((UIElement) this.e_8, 2);
      this.e_8.SetResourceReference(TextBlock.TextProperty, (object) "TradeScreenOtherOffer");
      this.e_9 = new ListBox();
      this.e_1.Children.Add((UIElement) this.e_9);
      this.e_9.Name = "e_9";
      this.e_9.Margin = new Thickness(0.0f, 10f, 10f, 10f);
      Style style = new Style(typeof (ListBox));
      Setter setter1 = new Setter(UIElement.MinHeightProperty, (object) 80f);
      style.Setters.Add(setter1);
      ControlTemplate controlTemplate = new ControlTemplate(typeof (ListBox), new Func<UIElement, UIElement>(PlayerTradeView.e_9_s_S_1_ctMethod));
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
      this.e_9.BorderThickness = new Thickness(2f, 2f, 2f, 2f);
      this.e_9.TabIndex = 0;
      Grid.SetColumn((UIElement) this.e_9, 1);
      Grid.SetRow((UIElement) this.e_9, 3);
      Grid.SetRowSpan((UIElement) this.e_9, 2);
      DragDrop.SetIsDragSource((UIElement) this.e_9, true);
      DragDrop.SetIsDropTarget((UIElement) this.e_9, true);
      this.e_9.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("InventoryItems")
      {
        UseGeneratedBindings = true
      });
      PlayerTradeView.InitializeElemente_9Resources((UIElement) this.e_9);
      this.e_14 = new ListBox();
      this.e_1.Children.Add((UIElement) this.e_14);
      this.e_14.Name = "e_14";
      this.e_14.Margin = new Thickness(0.0f, 10f, 0.0f, 10f);
      this.e_14.Background = (Brush) new SolidColorBrush(new ColorW(33, 44, 53, (int) byte.MaxValue));
      this.e_14.TabIndex = 1;
      this.e_14.ItemTemplate = new DataTemplate((object) typeof (MyInventoryItemModel), new Func<UIElement, UIElement>(PlayerTradeView.e_14_dtMethod));
      Grid.SetColumn((UIElement) this.e_14, 2);
      Grid.SetRow((UIElement) this.e_14, 3);
      DragDrop.SetIsDragSource((UIElement) this.e_14, true);
      DragDrop.SetIsDropTarget((UIElement) this.e_14, true);
      this.e_14.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("LocalPlayerOfferItems")
      {
        UseGeneratedBindings = true
      });
      this.e_21 = new StackPanel();
      this.e_1.Children.Add((UIElement) this.e_21);
      this.e_21.Name = "e_21";
      Grid.SetColumn((UIElement) this.e_21, 2);
      Grid.SetRow((UIElement) this.e_21, 4);
      this.e_22 = new NumericTextBox();
      this.e_21.Children.Add((UIElement) this.e_22);
      this.e_22.Name = "e_22";
      this.e_22.Margin = new Thickness(5f, 5f, 10f, 5f);
      this.e_22.TabIndex = 2;
      this.e_22.MaxLength = 9;
      this.e_22.Minimum = 0.0f;
      this.e_22.SetBinding(NumericTextBox.MaximumProperty, new Binding("LocalPlayerOfferCurrencyMaximum")
      {
        UseGeneratedBindings = true
      });
      this.e_22.SetBinding(NumericTextBox.ValueProperty, new Binding("LocalPlayerOfferCurrency")
      {
        UseGeneratedBindings = true
      });
      this.e_23 = new StackPanel();
      this.e_1.Children.Add((UIElement) this.e_23);
      this.e_23.Name = "e_23";
      Grid.SetColumn((UIElement) this.e_23, 2);
      Grid.SetRow((UIElement) this.e_23, 5);
      this.e_23.SetBinding(UIElement.VisibilityProperty, new Binding("IsPcuVisible")
      {
        UseGeneratedBindings = true
      });
      this.e_24 = new NumericTextBox();
      this.e_23.Children.Add((UIElement) this.e_24);
      this.e_24.Name = "e_24";
      this.e_24.Margin = new Thickness(5f, 5f, 10f, 5f);
      this.e_24.TabIndex = 3;
      this.e_24.MaxLength = 5;
      this.e_24.Minimum = 0.0f;
      this.e_24.Maximum = 99999f;
      this.e_24.SetBinding(NumericTextBox.ValueProperty, new Binding("LocalPlayerOfferPcu")
      {
        UseGeneratedBindings = true
      });
      this.e_25 = new Border();
      this.e_1.Children.Add((UIElement) this.e_25);
      this.e_25.Name = "e_25";
      this.e_25.Width = 2f;
      this.e_25.Margin = new Thickness(5f, 10f, 5f, 10f);
      this.e_25.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) this.e_25, 3);
      Grid.SetRow((UIElement) this.e_25, 2);
      Grid.SetRowSpan((UIElement) this.e_25, 4);
      this.e_26 = new Grid();
      this.e_1.Children.Add((UIElement) this.e_26);
      this.e_26.Name = "e_26";
      this.e_26.RowDefinitions.Add(new RowDefinition());
      this.e_26.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      Grid.SetColumn((UIElement) this.e_26, 4);
      Grid.SetRow((UIElement) this.e_26, 3);
      this.e_27 = new ListBox();
      this.e_26.Children.Add((UIElement) this.e_27);
      this.e_27.Name = "e_27";
      this.e_27.Margin = new Thickness(0.0f, 10f, 0.0f, 10f);
      this.e_27.Background = (Brush) new SolidColorBrush(new ColorW(33, 44, 53, (int) byte.MaxValue));
      this.e_27.TabIndex = 4;
      this.e_27.ItemTemplate = new DataTemplate((object) typeof (MyInventoryItemModel), new Func<UIElement, UIElement>(PlayerTradeView.e_27_dtMethod));
      Grid.SetRow((UIElement) this.e_27, 0);
      this.e_27.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("OtherPlayerOfferItems")
      {
        UseGeneratedBindings = true
      });
      this.e_34 = new StackPanel();
      this.e_26.Children.Add((UIElement) this.e_34);
      this.e_34.Name = "e_34";
      this.e_34.VerticalAlignment = VerticalAlignment.Bottom;
      this.e_34.Orientation = Orientation.Horizontal;
      Grid.SetRow((UIElement) this.e_34, 1);
      this.e_35 = new TextBlock();
      this.e_34.Children.Add((UIElement) this.e_35);
      this.e_35.Name = "e_35";
      this.e_35.Margin = new Thickness(5f, 5f, 5f, 5f);
      this.e_35.VerticalAlignment = VerticalAlignment.Center;
      this.e_35.SetResourceReference(TextBlock.TextProperty, (object) "TradeScreenOfferState");
      this.e_36 = new TextBlock();
      this.e_34.Children.Add((UIElement) this.e_36);
      this.e_36.Name = "e_36";
      this.e_36.Margin = new Thickness(5f, 5f, 10f, 5f);
      this.e_36.VerticalAlignment = VerticalAlignment.Center;
      this.e_36.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      this.e_36.SetBinding(TextBlock.TextProperty, new Binding("OtherPlayerAcceptState")
      {
        UseGeneratedBindings = true
      });
      this.e_37 = new StackPanel();
      this.e_1.Children.Add((UIElement) this.e_37);
      this.e_37.Name = "e_37";
      this.e_37.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) this.e_37, 4);
      Grid.SetRow((UIElement) this.e_37, 4);
      this.e_38 = new TextBlock();
      this.e_37.Children.Add((UIElement) this.e_38);
      this.e_38.Name = "e_38";
      this.e_38.Margin = new Thickness(5f, 5f, 5f, 5f);
      this.e_38.VerticalAlignment = VerticalAlignment.Center;
      this.e_38.SetResourceReference(TextBlock.TextProperty, (object) "TradeScreenMoney");
      this.e_39 = new TextBlock();
      this.e_37.Children.Add((UIElement) this.e_39);
      this.e_39.Name = "e_39";
      this.e_39.Margin = new Thickness(5f, 5f, 0.0f, 5f);
      this.e_39.VerticalAlignment = VerticalAlignment.Center;
      this.e_39.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      this.e_39.SetBinding(TextBlock.TextProperty, new Binding("OtherPlayerOfferCurrency")
      {
        UseGeneratedBindings = true
      });
      this.e_40 = new Image();
      this.e_37.Children.Add((UIElement) this.e_40);
      this.e_40.Name = "e_40";
      this.e_40.Height = 20f;
      this.e_40.Margin = new Thickness(3f, 2f, 2f, 0.0f);
      this.e_40.VerticalAlignment = VerticalAlignment.Center;
      this.e_40.Stretch = Stretch.Uniform;
      this.e_40.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      this.e_41 = new StackPanel();
      this.e_1.Children.Add((UIElement) this.e_41);
      this.e_41.Name = "e_41";
      this.e_41.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) this.e_41, 4);
      Grid.SetRow((UIElement) this.e_41, 5);
      this.e_41.SetBinding(UIElement.VisibilityProperty, new Binding("IsPcuVisible")
      {
        UseGeneratedBindings = true
      });
      this.e_42 = new TextBlock();
      this.e_41.Children.Add((UIElement) this.e_42);
      this.e_42.Name = "e_42";
      this.e_42.Margin = new Thickness(5f, 5f, 5f, 5f);
      this.e_42.VerticalAlignment = VerticalAlignment.Center;
      this.e_42.SetResourceReference(TextBlock.TextProperty, (object) "TradeScreenPcu");
      this.e_43 = new TextBlock();
      this.e_41.Children.Add((UIElement) this.e_43);
      this.e_43.Name = "e_43";
      this.e_43.Margin = new Thickness(5f, 5f, 10f, 5f);
      this.e_43.VerticalAlignment = VerticalAlignment.Center;
      this.e_43.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      this.e_43.SetBinding(TextBlock.TextProperty, new Binding("OtherPlayerOfferPcu")
      {
        UseGeneratedBindings = true
      });
      this.e_44 = new Grid();
      this.e_1.Children.Add((UIElement) this.e_44);
      this.e_44.Name = "e_44";
      this.e_44.ColumnDefinitions.Add(new ColumnDefinition());
      this.e_44.ColumnDefinitions.Add(new ColumnDefinition());
      Grid.SetColumn((UIElement) this.e_44, 1);
      Grid.SetRow((UIElement) this.e_44, 5);
      this.e_45 = new StackPanel();
      this.e_44.Children.Add((UIElement) this.e_45);
      this.e_45.Name = "e_45";
      this.e_45.HorizontalAlignment = HorizontalAlignment.Left;
      this.e_45.Orientation = Orientation.Horizontal;
      this.e_46 = new TextBlock();
      this.e_45.Children.Add((UIElement) this.e_46);
      this.e_46.Name = "e_46";
      this.e_46.Margin = new Thickness(0.0f, 5f, 5f, 5f);
      this.e_46.VerticalAlignment = VerticalAlignment.Center;
      this.e_46.SetResourceReference(TextBlock.TextProperty, (object) "TradeScreenMoney");
      this.e_47 = new TextBlock();
      this.e_45.Children.Add((UIElement) this.e_47);
      this.e_47.Name = "e_47";
      this.e_47.Margin = new Thickness(5f, 5f, 0.0f, 5f);
      this.e_47.VerticalAlignment = VerticalAlignment.Center;
      this.e_47.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      this.e_47.SetBinding(TextBlock.TextProperty, new Binding("LocalPlayerCurrency")
      {
        UseGeneratedBindings = true
      });
      this.e_48 = new Image();
      this.e_45.Children.Add((UIElement) this.e_48);
      this.e_48.Name = "e_48";
      this.e_48.Height = 20f;
      this.e_48.Margin = new Thickness(3f, 2f, 2f, 0.0f);
      this.e_48.VerticalAlignment = VerticalAlignment.Center;
      this.e_48.Stretch = Stretch.Uniform;
      this.e_48.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      this.e_49 = new StackPanel();
      this.e_44.Children.Add((UIElement) this.e_49);
      this.e_49.Name = "e_49";
      this.e_49.HorizontalAlignment = HorizontalAlignment.Left;
      this.e_49.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) this.e_49, 1);
      this.e_49.SetBinding(UIElement.VisibilityProperty, new Binding("IsPcuVisible")
      {
        UseGeneratedBindings = true
      });
      this.e_50 = new TextBlock();
      this.e_49.Children.Add((UIElement) this.e_50);
      this.e_50.Name = "e_50";
      this.e_50.Margin = new Thickness(0.0f, 5f, 5f, 5f);
      this.e_50.VerticalAlignment = VerticalAlignment.Center;
      this.e_50.SetResourceReference(TextBlock.TextProperty, (object) "TradeScreenPcu");
      this.e_51 = new TextBlock();
      this.e_49.Children.Add((UIElement) this.e_51);
      this.e_51.Name = "e_51";
      this.e_51.Margin = new Thickness(5f, 5f, 5f, 5f);
      this.e_51.VerticalAlignment = VerticalAlignment.Center;
      this.e_51.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      this.e_51.SetBinding(TextBlock.TextProperty, new Binding("LocalPlayerPcu")
      {
        UseGeneratedBindings = true
      });
      this.e_52 = new Border();
      this.e_1.Children.Add((UIElement) this.e_52);
      this.e_52.Name = "e_52";
      this.e_52.Height = 2f;
      this.e_52.Margin = new Thickness(0.0f, 10f, 0.0f, 10f);
      this.e_52.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) this.e_52, 1);
      Grid.SetRow((UIElement) this.e_52, 6);
      Grid.SetColumnSpan((UIElement) this.e_52, 4);
      this.e_53 = new StackPanel();
      this.e_1.Children.Add((UIElement) this.e_53);
      this.e_53.Name = "e_53";
      this.e_53.Margin = new Thickness(0.0f, 0.0f, 0.0f, 30f);
      this.e_53.HorizontalAlignment = HorizontalAlignment.Right;
      this.e_53.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) this.e_53, 1);
      Grid.SetRow((UIElement) this.e_53, 7);
      Grid.SetColumnSpan((UIElement) this.e_53, 4);
      this.e_54 = new Button();
      this.e_53.Children.Add((UIElement) this.e_54);
      this.e_54.Name = "e_54";
      this.e_54.Width = 200f;
      this.e_54.Margin = new Thickness(0.0f, 10f, 0.0f, 10f);
      this.e_54.TabIndex = 5;
      this.e_54.SetBinding(UIElement.IsEnabledProperty, new Binding("IsSubmitOfferEnabled")
      {
        UseGeneratedBindings = true
      });
      this.e_54.SetBinding(ContentControl.ContentProperty, new Binding("SubmitOfferLabel")
      {
        UseGeneratedBindings = true
      });
      this.e_54.SetBinding(Button.CommandProperty, new Binding("SubmitOfferCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_55 = new Button();
      this.e_53.Children.Add((UIElement) this.e_55);
      this.e_55.Name = "e_55";
      this.e_55.Width = 150f;
      this.e_55.Margin = new Thickness(10f, 10f, 0.0f, 10f);
      this.e_55.TabIndex = 6;
      this.e_55.SetBinding(UIElement.VisibilityProperty, new Binding("IsAcceptVisible")
      {
        UseGeneratedBindings = true
      });
      this.e_55.SetBinding(UIElement.IsEnabledProperty, new Binding("IsAcceptEnabled")
      {
        UseGeneratedBindings = true
      });
      this.e_55.SetBinding(Button.CommandProperty, new Binding("AcceptCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_55.SetResourceReference(ContentControl.ContentProperty, (object) "TradeScreenAccept");
      this.e_56 = new Button();
      this.e_53.Children.Add((UIElement) this.e_56);
      this.e_56.Name = "e_56";
      this.e_56.Width = 150f;
      this.e_56.Margin = new Thickness(10f, 10f, 0.0f, 10f);
      this.e_56.TabIndex = 7;
      this.e_56.SetBinding(UIElement.VisibilityProperty, new Binding("IsCancelVisible")
      {
        UseGeneratedBindings = true
      });
      this.e_56.SetBinding(Button.CommandProperty, new Binding("CancelCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_56.SetResourceReference(ContentControl.ContentProperty, (object) "TradeScreenCancel");
      ImageManager.Instance.AddImage("Textures\\GUI\\Screens\\screen_background.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol_highlight.dds");
      FontManager.Instance.AddFont("InventorySmall", 10f, FontStyle.Regular, "InventorySmall_7.5_Regular");
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "MaxWidth", typeof (MyPlayerTradeViewModel_MaxWidth_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "BackgroundOverlay", typeof (MyPlayerTradeViewModel_BackgroundOverlay_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "ExitCommand", typeof (MyPlayerTradeViewModel_ExitCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "InventoryItems", typeof (MyPlayerTradeViewModel_InventoryItems_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryItemModel), "Icon", typeof (MyInventoryItemModel_Icon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryItemModel), "AmountFormatted", typeof (MyInventoryItemModel_AmountFormatted_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryItemModel), "IconSymbol", typeof (MyInventoryItemModel_IconSymbol_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryItemModel), "Name", typeof (MyInventoryItemModel_Name_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "LocalPlayerOfferItems", typeof (MyPlayerTradeViewModel_LocalPlayerOfferItems_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "LocalPlayerOfferCurrencyMaximum", typeof (MyPlayerTradeViewModel_LocalPlayerOfferCurrencyMaximum_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "LocalPlayerOfferCurrency", typeof (MyPlayerTradeViewModel_LocalPlayerOfferCurrency_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "IsPcuVisible", typeof (MyPlayerTradeViewModel_IsPcuVisible_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "LocalPlayerOfferPcu", typeof (MyPlayerTradeViewModel_LocalPlayerOfferPcu_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "OtherPlayerOfferItems", typeof (MyPlayerTradeViewModel_OtherPlayerOfferItems_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "OtherPlayerAcceptState", typeof (MyPlayerTradeViewModel_OtherPlayerAcceptState_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "OtherPlayerOfferCurrency", typeof (MyPlayerTradeViewModel_OtherPlayerOfferCurrency_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "CurrencyIcon", typeof (MyPlayerTradeViewModel_CurrencyIcon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "OtherPlayerOfferPcu", typeof (MyPlayerTradeViewModel_OtherPlayerOfferPcu_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "LocalPlayerCurrency", typeof (MyPlayerTradeViewModel_LocalPlayerCurrency_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "LocalPlayerPcu", typeof (MyPlayerTradeViewModel_LocalPlayerPcu_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "IsSubmitOfferEnabled", typeof (MyPlayerTradeViewModel_IsSubmitOfferEnabled_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "SubmitOfferLabel", typeof (MyPlayerTradeViewModel_SubmitOfferLabel_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "SubmitOfferCommand", typeof (MyPlayerTradeViewModel_SubmitOfferCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "IsAcceptVisible", typeof (MyPlayerTradeViewModel_IsAcceptVisible_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "IsAcceptEnabled", typeof (MyPlayerTradeViewModel_IsAcceptEnabled_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "AcceptCommand", typeof (MyPlayerTradeViewModel_AcceptCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "IsCancelVisible", typeof (MyPlayerTradeViewModel_IsCancelVisible_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyPlayerTradeViewModel), "CancelCommand", typeof (MyPlayerTradeViewModel_CancelCommand_PropertyInfo));
    }

    private static void InitializeElementResources(UIElement elem)
    {
      elem.Resources.MergedDictionaries.Add((ResourceDictionary) Styles.Instance);
      elem.Resources.MergedDictionaries.Add((ResourceDictionary) DataTemplates.Instance);
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
      WrapPanel wrapPanel = new WrapPanel();
      scrollViewer.Content = (object) wrapPanel;
      wrapPanel.Name = "e_11";
      wrapPanel.Margin = new Thickness(4f, 4f, 4f, 4f);
      wrapPanel.IsItemsHost = true;
      wrapPanel.ItemHeight = 64f;
      wrapPanel.ItemWidth = 64f;
      return (UIElement) border;
    }

    private static void InitializeElemente_9Resources(UIElement elem)
    {
      Style style = new Style(typeof (ListBoxItem));
      Setter setter1 = new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "GridItem"));
      style.Setters.Add(setter1);
      ControlTemplate controlTemplate = new ControlTemplate(typeof (ListBoxItem), new Func<UIElement, UIElement>(PlayerTradeView.e_9r_0_s_S_1_ctMethod));
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

    private static UIElement e_14_dtMethod(UIElement parent)
    {
      Grid grid = new Grid();
      grid.Parent = parent;
      grid.Name = "e_15";
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
      border.Name = "e_16";
      border.Height = 64f;
      border.Width = 64f;
      border.SetResourceReference(Control.BackgroundProperty, (object) "GridItem");
      Image image = new Image();
      border.Child = (UIElement) image;
      image.Name = "e_17";
      image.Margin = new Thickness(5f, 5f, 5f, 5f);
      image.Stretch = Stretch.Fill;
      image.SetBinding(Image.SourceProperty, new Binding("Icon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock1 = new TextBlock();
      grid.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_18";
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
      textBlock2.Name = "e_19";
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
      textBlock3.Name = "e_20";
      textBlock3.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock3, 1);
      textBlock3.SetBinding(TextBlock.TextProperty, new Binding("Name")
      {
        UseGeneratedBindings = true
      });
      return (UIElement) grid;
    }

    private static UIElement e_27_dtMethod(UIElement parent)
    {
      Grid grid = new Grid();
      grid.Parent = parent;
      grid.Name = "e_28";
      grid.HorizontalAlignment = HorizontalAlignment.Stretch;
      grid.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid.ColumnDefinitions.Add(new ColumnDefinition());
      Border border = new Border();
      grid.Children.Add((UIElement) border);
      border.Name = "e_29";
      border.Height = 64f;
      border.Width = 64f;
      border.SetResourceReference(Control.BackgroundProperty, (object) "GridItem");
      Image image = new Image();
      border.Child = (UIElement) image;
      image.Name = "e_30";
      image.Margin = new Thickness(5f, 5f, 5f, 5f);
      image.Stretch = Stretch.Fill;
      image.SetBinding(Image.SourceProperty, new Binding("Icon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock1 = new TextBlock();
      grid.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_31";
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
      textBlock2.Name = "e_32";
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
      textBlock3.Name = "e_33";
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
