// Decompiled with JetBrains decompiler
// Type: EmptyKeys.UserInterface.Generated.EditFactionIconView_Gamepad
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Controls.Primitives;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.EditFactionIconView_Gamepad_Bindings;
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
  public class EditFactionIconView_Gamepad : UIRoot
  {
    private Grid rootGrid;
    private Grid e_0;
    private Image e_1;
    private Grid e_2;
    private ImageButton e_3;
    private StackPanel e_4;
    private TextBlock e_5;
    private Border e_6;
    private Grid e_7;
    private Grid e_8;
    private Border e_9;
    private Image FactionIcon;
    private ListBox ListBoxIcons;
    private Grid e_14;
    private StackPanel e_15;
    private TextBlock e_16;
    private Slider e_17;
    private StackPanel e_18;
    private TextBlock e_19;
    private Slider e_20;
    private StackPanel e_21;
    private TextBlock e_22;
    private Slider e_23;
    private Grid e_24;
    private StackPanel e_25;
    private TextBlock e_26;
    private Slider e_27;
    private StackPanel e_28;
    private TextBlock e_29;
    private Slider e_30;
    private StackPanel e_31;
    private TextBlock e_32;
    private Slider e_33;
    private Border e_34;
    private Grid SelectionHelp;
    private TextBlock e_35;
    private TextBlock e_36;
    private Grid ColorHelp;
    private TextBlock e_37;
    private TextBlock e_38;
    private TextBlock e_39;

    public EditFactionIconView_Gamepad() => this.Initialize();

    public EditFactionIconView_Gamepad(int width, int height)
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
      EditFactionIconView_Gamepad.InitializeElementResources((UIElement) this);
      this.rootGrid = new Grid();
      this.Content = (object) this.rootGrid;
      this.rootGrid.Name = "rootGrid";
      this.rootGrid.HorizontalAlignment = HorizontalAlignment.Center;
      this.rootGrid.SetBinding(UIElement.MaxWidthProperty, new Binding("MaxWidth")
      {
        UseGeneratedBindings = true
      });
      this.e_0 = new Grid();
      this.rootGrid.Children.Add((UIElement) this.e_0);
      this.e_0.Name = "e_0";
      this.e_0.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(3f, GridUnitType.Star)
      });
      this.e_0.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(11f, GridUnitType.Star)
      });
      this.e_0.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(3f, GridUnitType.Star)
      });
      this.e_0.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(6f, GridUnitType.Star)
      });
      this.e_0.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(16f, GridUnitType.Star)
      });
      this.e_0.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(6f, GridUnitType.Star)
      });
      this.e_1 = new Image();
      this.e_0.Children.Add((UIElement) this.e_1);
      this.e_1.Name = "e_1";
      this.e_1.Source = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Screens\\screen_background.dds"
      };
      this.e_1.Stretch = Stretch.Fill;
      Grid.SetColumn((UIElement) this.e_1, 1);
      Grid.SetRow((UIElement) this.e_1, 1);
      this.e_1.SetBinding(ImageBrush.ColorOverlayProperty, new Binding("BackgroundOverlay")
      {
        UseGeneratedBindings = true
      });
      this.e_2 = new Grid();
      this.e_0.Children.Add((UIElement) this.e_2);
      this.e_2.Name = "e_2";
      this.e_2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_2.RowDefinitions.Add(new RowDefinition());
      this.e_2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(64f, GridUnitType.Pixel)
      });
      this.e_2.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(75f, GridUnitType.Pixel)
      });
      this.e_2.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      this.e_2.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(75f, GridUnitType.Pixel)
      });
      Grid.SetColumn((UIElement) this.e_2, 1);
      Grid.SetRow((UIElement) this.e_2, 1);
      this.e_3 = new ImageButton();
      this.e_2.Children.Add((UIElement) this.e_3);
      this.e_3.Name = "e_3";
      this.e_3.Height = 24f;
      this.e_3.Width = 24f;
      this.e_3.Margin = new Thickness(16f, 16f, 16f, 16f);
      this.e_3.HorizontalAlignment = HorizontalAlignment.Right;
      this.e_3.VerticalAlignment = VerticalAlignment.Center;
      this.e_3.IsTabStop = false;
      this.e_3.ImageStretch = Stretch.Uniform;
      this.e_3.ImageNormal = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol.dds"
      };
      this.e_3.ImageHover = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds"
      };
      this.e_3.ImagePressed = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds"
      };
      Grid.SetColumn((UIElement) this.e_3, 2);
      Grid.SetRow((UIElement) this.e_3, 0);
      this.e_3.SetBinding(Button.CommandProperty, new Binding("ExitCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_4 = new StackPanel();
      this.e_2.Children.Add((UIElement) this.e_4);
      this.e_4.Name = "e_4";
      Grid.SetColumn((UIElement) this.e_4, 1);
      Grid.SetRow((UIElement) this.e_4, 1);
      Grid.SetColumnSpan((UIElement) this.e_4, 1);
      this.e_5 = new TextBlock();
      this.e_4.Children.Add((UIElement) this.e_5);
      this.e_5.Name = "e_5";
      this.e_5.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_5.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      this.e_5.SetResourceReference(TextBlock.TextProperty, (object) "ScreenCaptionEditFaction");
      this.e_6 = new Border();
      this.e_4.Children.Add((UIElement) this.e_6);
      this.e_6.Name = "e_6";
      this.e_6.Height = 2f;
      this.e_6.Margin = new Thickness(0.0f, 10f, 0.0f, 10f);
      this.e_6.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      this.e_7 = new Grid();
      this.e_2.Children.Add((UIElement) this.e_7);
      this.e_7.Name = "e_7";
      this.e_7.Margin = new Thickness(0.0f, 0.0f, 0.0f, 0.0f);
      this.e_7.HorizontalAlignment = HorizontalAlignment.Stretch;
      this.e_7.VerticalAlignment = VerticalAlignment.Stretch;
      GamepadBinding gamepadBinding = new GamepadBinding();
      gamepadBinding.Gesture = (InputGesture) new GamepadGesture(GamepadInput.AButton);
      gamepadBinding.SetBinding(InputBinding.CommandProperty, new Binding("OkCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_7.InputBindings.Add((InputBinding) gamepadBinding);
      gamepadBinding.Parent = (UIElement) this.e_7;
      this.e_7.RowDefinitions.Add(new RowDefinition());
      this.e_7.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_7.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      Grid.SetColumn((UIElement) this.e_7, 1);
      Grid.SetRow((UIElement) this.e_7, 2);
      this.e_8 = new Grid();
      this.e_7.Children.Add((UIElement) this.e_8);
      this.e_8.Name = "e_8";
      this.e_8.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(0.1f, GridUnitType.Star)
      });
      this.e_8.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_8.RowDefinitions.Add(new RowDefinition());
      this.e_8.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(0.2f, GridUnitType.Star)
      });
      this.e_8.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_8.ColumnDefinitions.Add(new ColumnDefinition());
      this.e_9 = new Border();
      this.e_8.Children.Add((UIElement) this.e_9);
      this.e_9.Name = "e_9";
      this.e_9.Margin = new Thickness(0.0f, 0.0f, 0.0f, 0.0f);
      this.e_9.HorizontalAlignment = HorizontalAlignment.Left;
      this.e_9.VerticalAlignment = VerticalAlignment.Top;
      this.e_9.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      this.e_9.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      Grid.SetColumn((UIElement) this.e_9, 0);
      Grid.SetRow((UIElement) this.e_9, 1);
      this.e_9.SetBinding(Control.BackgroundProperty, new Binding("FactionColor")
      {
        UseGeneratedBindings = true
      });
      this.FactionIcon = new Image();
      this.e_9.Child = (UIElement) this.FactionIcon;
      this.FactionIcon.Name = "FactionIcon";
      this.FactionIcon.Height = 128f;
      this.FactionIcon.Margin = new Thickness(0.0f, 0.0f, 0.0f, 0.0f);
      this.FactionIcon.Stretch = Stretch.Uniform;
      this.FactionIcon.SetBinding(Image.SourceProperty, new Binding("FactionIconBitmap")
      {
        UseGeneratedBindings = true
      });
      this.FactionIcon.SetBinding(ImageBrush.ColorOverlayProperty, new Binding("IconColorInternal")
      {
        UseGeneratedBindings = true
      });
      this.ListBoxIcons = new ListBox();
      this.e_8.Children.Add((UIElement) this.ListBoxIcons);
      this.ListBoxIcons.Name = "ListBoxIcons";
      this.ListBoxIcons.Margin = new Thickness(20f, 0.0f, 0.0f, 0.0f);
      Style style1 = new Style(typeof (ListBox));
      Setter setter1 = new Setter(UIElement.MinHeightProperty, (object) 80f);
      style1.Setters.Add(setter1);
      ControlTemplate controlTemplate = new ControlTemplate(typeof (ListBox), new Func<UIElement, UIElement>(EditFactionIconView_Gamepad.ListBoxIcons_s_S_1_ctMethod));
      Setter setter2 = new Setter(Control.TemplateProperty, (object) controlTemplate);
      style1.Setters.Add(setter2);
      Trigger trigger = new Trigger();
      trigger.Property = UIElement.IsFocusedProperty;
      trigger.Value = (object) true;
      Setter setter3 = new Setter(Control.BackgroundProperty, (object) new SolidColorBrush(new ColorW(67, 81, 92, 160)));
      trigger.Setters.Add(setter3);
      style1.Triggers.Add((TriggerBase) trigger);
      this.ListBoxIcons.Style = style1;
      this.ListBoxIcons.Background = (Brush) new SolidColorBrush(new ColorW(33, 44, 53, (int) byte.MaxValue));
      this.ListBoxIcons.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      this.ListBoxIcons.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      this.ListBoxIcons.TabIndex = 0;
      Grid.SetColumn((UIElement) this.ListBoxIcons, 1);
      Grid.SetRow((UIElement) this.ListBoxIcons, 1);
      Grid.SetRowSpan((UIElement) this.ListBoxIcons, 2);
      GamepadHelp.SetTargetName((DependencyObject) this.ListBoxIcons, "SelectionHelp");
      GamepadHelp.SetTabIndexDown((DependencyObject) this.ListBoxIcons, 1);
      this.ListBoxIcons.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("FactionIcons")
      {
        UseGeneratedBindings = true
      });
      this.ListBoxIcons.SetBinding(Selector.SelectedItemProperty, new Binding("SelectedIcon")
      {
        UseGeneratedBindings = true
      });
      EditFactionIconView_Gamepad.InitializeElementListBoxIconsResources((UIElement) this.ListBoxIcons);
      this.e_14 = new Grid();
      this.e_7.Children.Add((UIElement) this.e_14);
      this.e_14.Name = "e_14";
      this.e_14.HorizontalAlignment = HorizontalAlignment.Stretch;
      this.e_14.VerticalAlignment = VerticalAlignment.Stretch;
      this.e_14.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      this.e_14.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      this.e_14.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      Grid.SetRow((UIElement) this.e_14, 1);
      this.e_15 = new StackPanel();
      this.e_14.Children.Add((UIElement) this.e_15);
      this.e_15.Name = "e_15";
      this.e_15.Margin = new Thickness(0.0f, 0.0f, 10f, 0.0f);
      this.e_15.HorizontalAlignment = HorizontalAlignment.Stretch;
      this.e_15.VerticalAlignment = VerticalAlignment.Stretch;
      Grid.SetColumn((UIElement) this.e_15, 0);
      this.e_16 = new TextBlock();
      this.e_15.Children.Add((UIElement) this.e_16);
      this.e_16.Name = "e_16";
      this.e_16.Margin = new Thickness(2f, 0.0f, 0.0f, 0.0f);
      this.e_16.SetResourceReference(TextBlock.TextProperty, (object) "EditFaction_HueSliderText");
      this.e_17 = new Slider();
      this.e_15.Children.Add((UIElement) this.e_17);
      this.e_17.Name = "e_17";
      this.e_17.HorizontalAlignment = HorizontalAlignment.Stretch;
      this.e_17.VerticalAlignment = VerticalAlignment.Stretch;
      Style style2 = new Style(typeof (Slider));
      Setter setter4 = new Setter(UIElement.SnapsToDevicePixelsProperty, (object) false);
      style2.Setters.Add(setter4);
      Setter setter5 = new Setter(Control.TemplateProperty, (object) new ResourceReferenceExpression((object) "HorizontalSliderHuePicker"));
      style2.Setters.Add(setter5);
      this.e_17.Style = style2;
      this.e_17.TabIndex = 1;
      this.e_17.TickFrequency = 0.0027f;
      GamepadHelp.SetTargetName((DependencyObject) this.e_17, "ColorHelp");
      GamepadHelp.SetTabIndexRight((DependencyObject) this.e_17, 2);
      GamepadHelp.SetTabIndexUp((DependencyObject) this.e_17, 0);
      GamepadHelp.SetTabIndexDown((DependencyObject) this.e_17, 4);
      this.e_17.SetBinding(RangeBase.ValueProperty, new Binding("Hue")
      {
        UseGeneratedBindings = true
      });
      this.e_18 = new StackPanel();
      this.e_14.Children.Add((UIElement) this.e_18);
      this.e_18.Name = "e_18";
      this.e_18.Margin = new Thickness(0.0f, 0.0f, 10f, 0.0f);
      this.e_18.HorizontalAlignment = HorizontalAlignment.Stretch;
      this.e_18.VerticalAlignment = VerticalAlignment.Stretch;
      Grid.SetColumn((UIElement) this.e_18, 1);
      this.e_19 = new TextBlock();
      this.e_18.Children.Add((UIElement) this.e_19);
      this.e_19.Name = "e_19";
      this.e_19.Margin = new Thickness(2f, 0.0f, 0.0f, 0.0f);
      this.e_19.SetResourceReference(TextBlock.TextProperty, (object) "EditFaction_SaturationSliderText");
      this.e_20 = new Slider();
      this.e_18.Children.Add((UIElement) this.e_20);
      this.e_20.Name = "e_20";
      this.e_20.HorizontalAlignment = HorizontalAlignment.Stretch;
      this.e_20.VerticalAlignment = VerticalAlignment.Stretch;
      this.e_20.TabIndex = 2;
      this.e_20.TickFrequency = 0.01f;
      GamepadHelp.SetTargetName((DependencyObject) this.e_20, "ColorHelp");
      GamepadHelp.SetTabIndexLeft((DependencyObject) this.e_20, 1);
      GamepadHelp.SetTabIndexRight((DependencyObject) this.e_20, 3);
      GamepadHelp.SetTabIndexUp((DependencyObject) this.e_20, 0);
      GamepadHelp.SetTabIndexDown((DependencyObject) this.e_20, 5);
      this.e_20.SetBinding(RangeBase.ValueProperty, new Binding("Saturation")
      {
        UseGeneratedBindings = true
      });
      this.e_21 = new StackPanel();
      this.e_14.Children.Add((UIElement) this.e_21);
      this.e_21.Name = "e_21";
      this.e_21.HorizontalAlignment = HorizontalAlignment.Stretch;
      this.e_21.VerticalAlignment = VerticalAlignment.Stretch;
      Grid.SetColumn((UIElement) this.e_21, 2);
      this.e_22 = new TextBlock();
      this.e_21.Children.Add((UIElement) this.e_22);
      this.e_22.Name = "e_22";
      this.e_22.Margin = new Thickness(2f, 0.0f, 0.0f, 0.0f);
      this.e_22.SetResourceReference(TextBlock.TextProperty, (object) "EditFaction_ValueSliderText");
      this.e_23 = new Slider();
      this.e_21.Children.Add((UIElement) this.e_23);
      this.e_23.Name = "e_23";
      this.e_23.HorizontalAlignment = HorizontalAlignment.Stretch;
      this.e_23.VerticalAlignment = VerticalAlignment.Stretch;
      this.e_23.TabIndex = 3;
      this.e_23.TickFrequency = 0.01f;
      GamepadHelp.SetTargetName((DependencyObject) this.e_23, "ColorHelp");
      GamepadHelp.SetTabIndexLeft((DependencyObject) this.e_23, 2);
      GamepadHelp.SetTabIndexUp((DependencyObject) this.e_23, 0);
      GamepadHelp.SetTabIndexDown((DependencyObject) this.e_23, 6);
      this.e_23.SetBinding(RangeBase.ValueProperty, new Binding("ColorValue")
      {
        UseGeneratedBindings = true
      });
      this.e_24 = new Grid();
      this.e_7.Children.Add((UIElement) this.e_24);
      this.e_24.Name = "e_24";
      this.e_24.HorizontalAlignment = HorizontalAlignment.Stretch;
      this.e_24.VerticalAlignment = VerticalAlignment.Stretch;
      this.e_24.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      this.e_24.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      this.e_24.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      Grid.SetRow((UIElement) this.e_24, 2);
      this.e_25 = new StackPanel();
      this.e_24.Children.Add((UIElement) this.e_25);
      this.e_25.Name = "e_25";
      this.e_25.Margin = new Thickness(0.0f, 0.0f, 10f, 0.0f);
      this.e_25.HorizontalAlignment = HorizontalAlignment.Stretch;
      this.e_25.VerticalAlignment = VerticalAlignment.Stretch;
      Grid.SetColumn((UIElement) this.e_25, 0);
      this.e_26 = new TextBlock();
      this.e_25.Children.Add((UIElement) this.e_26);
      this.e_26.Name = "e_26";
      this.e_26.Margin = new Thickness(2f, 0.0f, 0.0f, 0.0f);
      this.e_26.SetResourceReference(TextBlock.TextProperty, (object) "EditFaction_HueIconSliderText");
      this.e_27 = new Slider();
      this.e_25.Children.Add((UIElement) this.e_27);
      this.e_27.Name = "e_27";
      this.e_27.HorizontalAlignment = HorizontalAlignment.Stretch;
      this.e_27.VerticalAlignment = VerticalAlignment.Stretch;
      Style style3 = new Style(typeof (Slider));
      Setter setter6 = new Setter(UIElement.SnapsToDevicePixelsProperty, (object) false);
      style3.Setters.Add(setter6);
      Setter setter7 = new Setter(Control.TemplateProperty, (object) new ResourceReferenceExpression((object) "HorizontalSliderHuePicker"));
      style3.Setters.Add(setter7);
      this.e_27.Style = style3;
      this.e_27.TabIndex = 4;
      this.e_27.TickFrequency = 0.0027f;
      GamepadHelp.SetTargetName((DependencyObject) this.e_27, "ColorHelp");
      GamepadHelp.SetTabIndexRight((DependencyObject) this.e_27, 5);
      GamepadHelp.SetTabIndexUp((DependencyObject) this.e_27, 1);
      this.e_27.SetBinding(RangeBase.ValueProperty, new Binding("HueIcon")
      {
        UseGeneratedBindings = true
      });
      this.e_28 = new StackPanel();
      this.e_24.Children.Add((UIElement) this.e_28);
      this.e_28.Name = "e_28";
      this.e_28.Margin = new Thickness(0.0f, 0.0f, 10f, 0.0f);
      this.e_28.HorizontalAlignment = HorizontalAlignment.Stretch;
      this.e_28.VerticalAlignment = VerticalAlignment.Stretch;
      Grid.SetColumn((UIElement) this.e_28, 1);
      this.e_29 = new TextBlock();
      this.e_28.Children.Add((UIElement) this.e_29);
      this.e_29.Name = "e_29";
      this.e_29.Margin = new Thickness(2f, 0.0f, 0.0f, 0.0f);
      this.e_29.SetResourceReference(TextBlock.TextProperty, (object) "EditFaction_SaturationIconSliderText");
      this.e_30 = new Slider();
      this.e_28.Children.Add((UIElement) this.e_30);
      this.e_30.Name = "e_30";
      this.e_30.HorizontalAlignment = HorizontalAlignment.Stretch;
      this.e_30.VerticalAlignment = VerticalAlignment.Stretch;
      this.e_30.TabIndex = 5;
      this.e_30.TickFrequency = 0.01f;
      GamepadHelp.SetTargetName((DependencyObject) this.e_30, "ColorHelp");
      GamepadHelp.SetTabIndexLeft((DependencyObject) this.e_30, 4);
      GamepadHelp.SetTabIndexRight((DependencyObject) this.e_30, 6);
      GamepadHelp.SetTabIndexUp((DependencyObject) this.e_30, 2);
      this.e_30.SetBinding(RangeBase.ValueProperty, new Binding("SaturationIcon")
      {
        UseGeneratedBindings = true
      });
      this.e_31 = new StackPanel();
      this.e_24.Children.Add((UIElement) this.e_31);
      this.e_31.Name = "e_31";
      this.e_31.HorizontalAlignment = HorizontalAlignment.Stretch;
      this.e_31.VerticalAlignment = VerticalAlignment.Stretch;
      Grid.SetColumn((UIElement) this.e_31, 2);
      this.e_32 = new TextBlock();
      this.e_31.Children.Add((UIElement) this.e_32);
      this.e_32.Name = "e_32";
      this.e_32.Margin = new Thickness(2f, 0.0f, 0.0f, 0.0f);
      this.e_32.SetResourceReference(TextBlock.TextProperty, (object) "EditFaction_ValueIconSliderText");
      this.e_33 = new Slider();
      this.e_31.Children.Add((UIElement) this.e_33);
      this.e_33.Name = "e_33";
      this.e_33.HorizontalAlignment = HorizontalAlignment.Stretch;
      this.e_33.VerticalAlignment = VerticalAlignment.Stretch;
      this.e_33.TabIndex = 6;
      this.e_33.TickFrequency = 0.01f;
      GamepadHelp.SetTargetName((DependencyObject) this.e_33, "ColorHelp");
      GamepadHelp.SetTabIndexLeft((DependencyObject) this.e_33, 5);
      GamepadHelp.SetTabIndexUp((DependencyObject) this.e_33, 3);
      this.e_33.SetBinding(RangeBase.ValueProperty, new Binding("ColorValueIcon")
      {
        UseGeneratedBindings = true
      });
      this.e_34 = new Border();
      this.e_2.Children.Add((UIElement) this.e_34);
      this.e_34.Name = "e_34";
      this.e_34.Height = 2f;
      this.e_34.Margin = new Thickness(0.0f, 30f, 0.0f, 0.0f);
      this.e_34.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) this.e_34, 1);
      Grid.SetRow((UIElement) this.e_34, 3);
      Grid.SetColumnSpan((UIElement) this.e_34, 1);
      this.SelectionHelp = new Grid();
      this.e_2.Children.Add((UIElement) this.SelectionHelp);
      this.SelectionHelp.Name = "SelectionHelp";
      this.SelectionHelp.VerticalAlignment = VerticalAlignment.Center;
      this.SelectionHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.SelectionHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.SelectionHelp.ColumnDefinitions.Add(new ColumnDefinition());
      Grid.SetColumn((UIElement) this.SelectionHelp, 1);
      Grid.SetRow((UIElement) this.SelectionHelp, 4);
      this.e_35 = new TextBlock();
      this.SelectionHelp.Children.Add((UIElement) this.e_35);
      this.e_35.Name = "e_35";
      this.e_35.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      this.e_35.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_35.VerticalAlignment = VerticalAlignment.Center;
      this.e_35.SetResourceReference(TextBlock.TextProperty, (object) "EditFactionLogoScreen_Help_Selection");
      this.e_36 = new TextBlock();
      this.SelectionHelp.Children.Add((UIElement) this.e_36);
      this.e_36.Name = "e_36";
      this.e_36.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      this.e_36.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_36.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) this.e_36, 1);
      this.e_36.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Back");
      this.ColorHelp = new Grid();
      this.e_2.Children.Add((UIElement) this.ColorHelp);
      this.ColorHelp.Name = "ColorHelp";
      this.ColorHelp.Visibility = Visibility.Collapsed;
      this.ColorHelp.VerticalAlignment = VerticalAlignment.Center;
      this.ColorHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.ColorHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.ColorHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.ColorHelp.ColumnDefinitions.Add(new ColumnDefinition());
      Grid.SetColumn((UIElement) this.ColorHelp, 1);
      Grid.SetRow((UIElement) this.ColorHelp, 4);
      this.e_37 = new TextBlock();
      this.ColorHelp.Children.Add((UIElement) this.e_37);
      this.e_37.Name = "e_37";
      this.e_37.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      this.e_37.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_37.VerticalAlignment = VerticalAlignment.Center;
      this.e_37.SetResourceReference(TextBlock.TextProperty, (object) "EditFactionLogoScreen_Help_Selection");
      this.e_38 = new TextBlock();
      this.ColorHelp.Children.Add((UIElement) this.e_38);
      this.e_38.Name = "e_38";
      this.e_38.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      this.e_38.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_38.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) this.e_38, 1);
      this.e_38.SetResourceReference(TextBlock.TextProperty, (object) "EditFactionLogoScreen_Help_ChangeColor");
      this.e_39 = new TextBlock();
      this.ColorHelp.Children.Add((UIElement) this.e_39);
      this.e_39.Name = "e_39";
      this.e_39.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      this.e_39.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_39.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) this.e_39, 2);
      this.e_39.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Back");
      ImageManager.Instance.AddImage("Textures\\GUI\\Screens\\screen_background.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol_highlight.dds");
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyEditFactionIconViewModel), "MaxWidth", typeof (MyEditFactionIconViewModel_MaxWidth_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyEditFactionIconViewModel), "BackgroundOverlay", typeof (MyEditFactionIconViewModel_BackgroundOverlay_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyEditFactionIconViewModel), "ExitCommand", typeof (MyEditFactionIconViewModel_ExitCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyEditFactionIconViewModel), "OkCommand", typeof (MyEditFactionIconViewModel_OkCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyEditFactionIconViewModel), "FactionColor", typeof (MyEditFactionIconViewModel_FactionColor_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyEditFactionIconViewModel), "FactionIconBitmap", typeof (MyEditFactionIconViewModel_FactionIconBitmap_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyEditFactionIconViewModel), "IconColorInternal", typeof (MyEditFactionIconViewModel_IconColorInternal_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyEditFactionIconViewModel), "FactionIcons", typeof (MyEditFactionIconViewModel_FactionIcons_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyEditFactionIconViewModel), "SelectedIcon", typeof (MyEditFactionIconViewModel_SelectedIcon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyEditFactionIconViewModel), "Hue", typeof (MyEditFactionIconViewModel_Hue_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyEditFactionIconViewModel), "Saturation", typeof (MyEditFactionIconViewModel_Saturation_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyEditFactionIconViewModel), "ColorValue", typeof (MyEditFactionIconViewModel_ColorValue_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyEditFactionIconViewModel), "HueIcon", typeof (MyEditFactionIconViewModel_HueIcon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyEditFactionIconViewModel), "SaturationIcon", typeof (MyEditFactionIconViewModel_SaturationIcon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyEditFactionIconViewModel), "ColorValueIcon", typeof (MyEditFactionIconViewModel_ColorValueIcon_PropertyInfo));
    }

    private static void InitializeElementResources(UIElement elem)
    {
      elem.Resources.MergedDictionaries.Add((ResourceDictionary) Styles.Instance);
      elem.Resources.MergedDictionaries.Add((ResourceDictionary) DataTemplatesEditFaction.Instance);
    }

    private static UIElement ListBoxIcons_s_S_1_ctMethod(UIElement parent)
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
      scrollViewer.HorizontalContentAlignment = HorizontalAlignment.Center;
      scrollViewer.VerticalContentAlignment = VerticalAlignment.Top;
      UniformGrid uniformGrid = new UniformGrid();
      scrollViewer.Content = (object) uniformGrid;
      uniformGrid.Name = "e_11";
      uniformGrid.Margin = new Thickness(6f, 6f, 6f, 6f);
      uniformGrid.IsItemsHost = true;
      uniformGrid.Columns = 6;
      return (UIElement) border;
    }

    private static void InitializeElementListBoxIconsResources(UIElement elem)
    {
      Style style = new Style(typeof (ListBoxItem));
      Setter setter1 = new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "GridItem"));
      style.Setters.Add(setter1);
      ControlTemplate controlTemplate = new ControlTemplate(typeof (ListBoxItem), new Func<UIElement, UIElement>(EditFactionIconView_Gamepad.ListBoxIconsr_0_s_S_1_ctMethod));
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

    private static UIElement ListBoxIconsr_0_s_S_1_ctMethod(UIElement parent)
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
