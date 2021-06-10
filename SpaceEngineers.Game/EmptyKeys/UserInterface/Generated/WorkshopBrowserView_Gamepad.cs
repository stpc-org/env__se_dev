// Decompiled with JetBrains decompiler
// Type: EmptyKeys.UserInterface.Generated.WorkshopBrowserView_Gamepad
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Controls.Primitives;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.WorkshopBrowserView_Gamepad_Bindings;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Themes;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.Screens.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.ObjectModel;

namespace EmptyKeys.UserInterface.Generated
{
  [GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
  public class WorkshopBrowserView_Gamepad : UIRoot
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
    private ComboBox e_8;
    private ComboBox e_13;
    private AnimatedImage e_17;
    private CheckBox e_18;
    private CheckBox e_22;
    private Grid e_26;
    private TextBox e_27;
    private TextBlock e_28;
    private TextBlock e_29;
    private ListBox e_30;
    private Grid e_41;
    private Grid e_42;
    private ItemsControl e_43;
    private TextBlock e_46;
    private TextBlock e_47;
    private TextBlock e_48;
    private TextBlock e_49;
    private TextBlock e_50;
    private TextBlock e_51;
    private TextBlock e_52;
    private TextBlock e_53;
    private TextBlock e_54;
    private ScrollViewer e_55;
    private Border e_68;
    private TextBlock e_69;
    private Grid e_70;
    private ImageButton e_71;
    private StackPanel e_72;
    private TextBlock e_73;
    private TextBlock e_74;
    private TextBlock e_75;
    private ImageButton e_76;
    private Border e_77;
    private Grid ListHelp;
    private TextBlock e_78;
    private TextBlock e_79;
    private TextBlock e_80;
    private TextBlock e_81;
    private TextBlock e_82;
    private TextBlock e_83;
    private Grid SortHelp;
    private TextBlock e_84;
    private TextBlock e_85;
    private TextBlock e_86;
    private Grid CategoryHelp;
    private TextBlock e_87;
    private TextBlock e_88;
    private TextBlock e_89;
    private TextBlock e_90;

    public WorkshopBrowserView_Gamepad() => this.Initialize();

    public WorkshopBrowserView_Gamepad(int width, int height)
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
      WorkshopBrowserView_Gamepad.InitializeElementResources((UIElement) this);
      this.rootGrid = new Grid();
      this.Content = (object) this.rootGrid;
      this.rootGrid.Name = "rootGrid";
      this.rootGrid.HorizontalAlignment = HorizontalAlignment.Center;
      this.rootGrid.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(0.04f, GridUnitType.Star)
      });
      this.rootGrid.RowDefinitions.Add(new RowDefinition());
      this.rootGrid.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(0.04f, GridUnitType.Star)
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
        Width = new GridLength(70f, GridUnitType.Pixel)
      });
      this.e_1.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(4f, GridUnitType.Star)
      });
      this.e_1.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(70f, GridUnitType.Pixel)
      });
      Grid.SetRow((UIElement) this.e_1, 1);
      this.e_2 = new ImageButton();
      this.e_1.Children.Add((UIElement) this.e_2);
      this.e_2.Name = "e_2";
      this.e_2.Height = 24f;
      this.e_2.Width = 24f;
      this.e_2.Margin = new Thickness(16f, 16f, 16f, 5f);
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
      this.e_4.SetResourceReference(TextBlock.TextProperty, (object) "ScreenCaptionWorkshopBrowser");
      this.e_5 = new Border();
      this.e_3.Children.Add((UIElement) this.e_5);
      this.e_5.Name = "e_5";
      this.e_5.Height = 2f;
      this.e_5.Margin = new Thickness(0.0f, 10f, 0.0f, 10f);
      this.e_5.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      this.e_6 = new Grid();
      this.e_1.Children.Add((UIElement) this.e_6);
      this.e_6.Name = "e_6";
      GamepadBinding gamepadBinding1 = new GamepadBinding();
      gamepadBinding1.Gesture = (InputGesture) new GamepadGesture(GamepadInput.DButton);
      gamepadBinding1.SetBinding(InputBinding.CommandProperty, new Binding("RefreshCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_6.InputBindings.Add((InputBinding) gamepadBinding1);
      gamepadBinding1.Parent = (UIElement) this.e_6;
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
      this.e_6.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(90f, GridUnitType.Pixel)
      });
      this.e_6.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(3f, GridUnitType.Star)
      });
      this.e_6.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      Grid.SetColumn((UIElement) this.e_6, 1);
      Grid.SetRow((UIElement) this.e_6, 3);
      this.e_7 = new Grid();
      this.e_6.Children.Add((UIElement) this.e_7);
      this.e_7.Name = "e_7";
      this.e_7.Margin = new Thickness(0.0f, 0.0f, 10f, 0.0f);
      this.e_7.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(200f, GridUnitType.Pixel)
      });
      this.e_7.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(200f, GridUnitType.Pixel)
      });
      this.e_7.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_7.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(26f, GridUnitType.Pixel)
      });
      this.e_7.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(38f, GridUnitType.Pixel)
      });
      this.e_7.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(38f, GridUnitType.Pixel)
      });
      this.e_7.ColumnDefinitions.Add(new ColumnDefinition());
      this.e_8 = new ComboBox();
      this.e_7.Children.Add((UIElement) this.e_8);
      this.e_8.Name = "e_8";
      this.e_8.VerticalAlignment = VerticalAlignment.Center;
      this.e_8.TabIndex = 1;
      this.e_8.ItemsSource = (IEnumerable) WorkshopBrowserView_Gamepad.Get_e_8_Items();
      this.e_8.MaxDropDownHeight = 120f;
      GamepadHelp.SetTargetName((DependencyObject) this.e_8, "SortHelp");
      GamepadHelp.SetTabIndexRight((DependencyObject) this.e_8, 3);
      GamepadHelp.SetTabIndexDown((DependencyObject) this.e_8, 5);
      this.e_8.SetBinding(Selector.SelectedIndexProperty, new Binding("SelectedSortIndex")
      {
        UseGeneratedBindings = true
      });
      this.e_13 = new ComboBox();
      this.e_7.Children.Add((UIElement) this.e_13);
      this.e_13.Name = "e_13";
      this.e_13.Margin = new Thickness(5f, 0.0f, 0.0f, 0.0f);
      this.e_13.VerticalAlignment = VerticalAlignment.Center;
      this.e_13.TabIndex = 3;
      this.e_13.ItemTemplate = new DataTemplate(new Func<UIElement, UIElement>(WorkshopBrowserView_Gamepad.e_13_dtMethod));
      this.e_13.MaxDropDownHeight = 300f;
      Grid.SetColumn((UIElement) this.e_13, 1);
      GamepadHelp.SetTargetName((DependencyObject) this.e_13, "CategoryHelp");
      GamepadHelp.SetTabIndexLeft((DependencyObject) this.e_13, 1);
      GamepadHelp.SetTabIndexDown((DependencyObject) this.e_13, 5);
      this.e_13.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("Categories")
      {
        UseGeneratedBindings = true
      });
      this.e_13.SetBinding(Selector.SelectedIndexProperty, new Binding("SelectedCategoryIndex")
      {
        UseGeneratedBindings = true
      });
      this.e_13.SetBinding(GamepadHelp.TabIndexRightProperty, new Binding("CategoryControlTabIndexRight")
      {
        UseGeneratedBindings = true
      });
      this.e_17 = new AnimatedImage();
      this.e_7.Children.Add((UIElement) this.e_17);
      this.e_17.Name = "e_17";
      this.e_17.Width = 24f;
      this.e_17.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_17.VerticalAlignment = VerticalAlignment.Center;
      this.e_17.Source = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\LoadingIconAnimated.png"
      };
      this.e_17.FrameWidth = 128;
      this.e_17.FrameHeight = 128;
      this.e_17.FramesPerSecond = 20;
      Grid.SetColumn((UIElement) this.e_17, 3);
      this.e_17.SetBinding(UIElement.VisibilityProperty, new Binding("IsRefreshing")
      {
        UseGeneratedBindings = true
      });
      this.e_18 = new CheckBox();
      this.e_7.Children.Add((UIElement) this.e_18);
      this.e_18.Name = "e_18";
      this.e_18.IsEnabled = true;
      this.e_18.Margin = new Thickness(0.0f, 0.0f, 0.0f, 0.0f);
      this.e_18.VerticalAlignment = VerticalAlignment.Center;
      Style style1 = new Style(typeof (CheckBox));
      ControlTemplate controlTemplate1 = new ControlTemplate(typeof (CheckBox), new Func<UIElement, UIElement>(WorkshopBrowserView_Gamepad.e_18_s_S_0_ctMethod));
      Trigger trigger1 = new Trigger();
      trigger1.Property = UIElement.IsMouseOverProperty;
      trigger1.Value = (object) true;
      trigger1.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "CheckServiceBackHighlighted"))
      {
        TargetName = "PART_Background"
      });
      controlTemplate1.Triggers.Add((TriggerBase) trigger1);
      Trigger trigger2 = new Trigger();
      trigger2.Property = UIElement.IsFocusedProperty;
      trigger2.Value = (object) true;
      trigger2.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "CheckServiceBackFocused"))
      {
        TargetName = "PART_Background"
      });
      trigger2.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "CheckService0Focused"))
      {
        TargetName = "PART_Icon"
      });
      controlTemplate1.Triggers.Add((TriggerBase) trigger2);
      Trigger trigger3 = new Trigger();
      trigger3.Property = ToggleButton.IsCheckedProperty;
      trigger3.Value = (object) true;
      trigger3.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "CheckServiceBackChecked"))
      {
        TargetName = "PART_Background"
      });
      controlTemplate1.Triggers.Add((TriggerBase) trigger3);
      Setter setter1 = new Setter(Control.TemplateProperty, (object) controlTemplate1);
      style1.Setters.Add(setter1);
      this.e_18.Style = style1;
      this.e_18.TabIndex = 11;
      Grid.SetColumn((UIElement) this.e_18, 4);
      GamepadHelp.SetTargetName((DependencyObject) this.e_18, "SortHelp");
      GamepadHelp.SetTabIndexLeft((DependencyObject) this.e_18, 3);
      GamepadHelp.SetTabIndexRight((DependencyObject) this.e_18, 12);
      GamepadHelp.SetTabIndexDown((DependencyObject) this.e_18, 5);
      this.e_18.SetBinding(UIElement.VisibilityProperty, new Binding("IsWorkshopAggregator")
      {
        UseGeneratedBindings = true
      });
      this.e_18.SetBinding(Button.CommandProperty, new Binding("ServiceCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_18.SetBinding(ToggleButton.IsCheckedProperty, new Binding("Service0IsChecked")
      {
        UseGeneratedBindings = true
      });
      this.e_18.SetResourceReference(UIElement.ToolTipProperty, (object) "WorkshopBrowser_Service0");
      this.e_22 = new CheckBox();
      this.e_7.Children.Add((UIElement) this.e_22);
      this.e_22.Name = "e_22";
      this.e_22.IsEnabled = true;
      this.e_22.Margin = new Thickness(0.0f, 0.0f, 0.0f, 0.0f);
      this.e_22.VerticalAlignment = VerticalAlignment.Center;
      Style style2 = new Style(typeof (CheckBox));
      ControlTemplate controlTemplate2 = new ControlTemplate(typeof (CheckBox), new Func<UIElement, UIElement>(WorkshopBrowserView_Gamepad.e_22_s_S_0_ctMethod));
      Trigger trigger4 = new Trigger();
      trigger4.Property = UIElement.IsMouseOverProperty;
      trigger4.Value = (object) true;
      trigger4.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "CheckServiceBackHighlighted"))
      {
        TargetName = "PART_Background"
      });
      controlTemplate2.Triggers.Add((TriggerBase) trigger4);
      Trigger trigger5 = new Trigger();
      trigger5.Property = UIElement.IsFocusedProperty;
      trigger5.Value = (object) true;
      trigger5.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Icons\\Browser\\BackgroundFocused.png"
        }
      })
      {
        TargetName = "PART_Background"
      });
      trigger5.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Icons\\Browser\\ModioCBFocused.png"
        }
      })
      {
        TargetName = "PART_Icon"
      });
      controlTemplate2.Triggers.Add((TriggerBase) trigger5);
      Trigger trigger6 = new Trigger();
      trigger6.Property = ToggleButton.IsCheckedProperty;
      trigger6.Value = (object) true;
      trigger6.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "CheckServiceBackChecked"))
      {
        TargetName = "PART_Background"
      });
      controlTemplate2.Triggers.Add((TriggerBase) trigger6);
      Setter setter2 = new Setter(Control.TemplateProperty, (object) controlTemplate2);
      style2.Setters.Add(setter2);
      this.e_22.Style = style2;
      this.e_22.TabIndex = 12;
      Grid.SetColumn((UIElement) this.e_22, 5);
      GamepadHelp.SetTargetName((DependencyObject) this.e_22, "SortHelp");
      GamepadHelp.SetTabIndexLeft((DependencyObject) this.e_22, 11);
      GamepadHelp.SetTabIndexRight((DependencyObject) this.e_22, 4);
      GamepadHelp.SetTabIndexDown((DependencyObject) this.e_22, 5);
      this.e_22.SetBinding(UIElement.VisibilityProperty, new Binding("IsWorkshopAggregator")
      {
        UseGeneratedBindings = true
      });
      this.e_22.SetBinding(Button.CommandProperty, new Binding("ServiceCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_22.SetBinding(ToggleButton.IsCheckedProperty, new Binding("Service1IsChecked")
      {
        UseGeneratedBindings = true
      });
      this.e_22.SetResourceReference(UIElement.ToolTipProperty, (object) "WorkshopBrowser_Service1");
      this.e_26 = new Grid();
      this.e_7.Children.Add((UIElement) this.e_26);
      this.e_26.Name = "e_26";
      Grid.SetColumn((UIElement) this.e_26, 6);
      this.e_27 = new TextBox();
      this.e_26.Children.Add((UIElement) this.e_27);
      this.e_27.Name = "e_27";
      this.e_27.VerticalAlignment = VerticalAlignment.Center;
      this.e_27.TabIndex = 4;
      GamepadHelp.SetTargetName((DependencyObject) this.e_27, "SortHelp");
      GamepadHelp.SetTabIndexDown((DependencyObject) this.e_27, 5);
      this.e_27.SetBinding(TextBox.TextProperty, new Binding("SearchText")
      {
        UseGeneratedBindings = true
      });
      this.e_27.SetBinding(GamepadHelp.TabIndexLeftProperty, new Binding("SearchControlTabIndexLeft")
      {
        UseGeneratedBindings = true
      });
      this.e_28 = new TextBlock();
      this.e_26.Children.Add((UIElement) this.e_28);
      this.e_28.Name = "e_28";
      this.e_28.IsHitTestVisible = false;
      this.e_28.Margin = new Thickness(4f, 4f, 4f, 4f);
      this.e_28.VerticalAlignment = VerticalAlignment.Center;
      this.e_28.Foreground = (Brush) new SolidColorBrush(new ColorW(94, 115, (int) sbyte.MaxValue, (int) byte.MaxValue));
      this.e_28.SetBinding(UIElement.VisibilityProperty, new Binding("IsSearchLabelVisible")
      {
        UseGeneratedBindings = true
      });
      this.e_28.SetResourceReference(TextBlock.TextProperty, (object) "WorkshopBrowser_Search");
      this.e_29 = new TextBlock();
      this.e_6.Children.Add((UIElement) this.e_29);
      this.e_29.Name = "e_29";
      this.e_29.Margin = new Thickness(0.0f, 10f, 10f, 10f);
      this.e_29.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_29.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetRow((UIElement) this.e_29, 1);
      Grid.SetColumnSpan((UIElement) this.e_29, 2);
      this.e_29.SetBinding(UIElement.VisibilityProperty, new Binding("IsNotFoundTextVisible")
      {
        UseGeneratedBindings = true
      });
      this.e_29.SetResourceReference(TextBlock.TextProperty, (object) "WorkshopBrowser_NotFound");
      this.e_30 = new ListBox();
      this.e_6.Children.Add((UIElement) this.e_30);
      this.e_30.Name = "e_30";
      this.e_30.Margin = new Thickness(0.0f, 10f, 10f, 10f);
      this.e_30.HorizontalAlignment = HorizontalAlignment.Stretch;
      this.e_30.VerticalAlignment = VerticalAlignment.Stretch;
      Style style3 = new Style(typeof (ListBox));
      Setter setter3 = new Setter(UIElement.MinHeightProperty, (object) 80f);
      style3.Setters.Add(setter3);
      ControlTemplate controlTemplate3 = new ControlTemplate(typeof (ListBox), new Func<UIElement, UIElement>(WorkshopBrowserView_Gamepad.e_30_s_S_1_ctMethod));
      Setter setter4 = new Setter(Control.TemplateProperty, (object) controlTemplate3);
      style3.Setters.Add(setter4);
      Trigger trigger7 = new Trigger();
      trigger7.Property = UIElement.IsFocusedProperty;
      trigger7.Value = (object) true;
      Setter setter5 = new Setter(Control.BackgroundProperty, (object) new SolidColorBrush(new ColorW(67, 81, 92, 160)));
      trigger7.Setters.Add(setter5);
      style3.Triggers.Add((TriggerBase) trigger7);
      this.e_30.Style = style3;
      GamepadBinding gamepadBinding2 = new GamepadBinding();
      gamepadBinding2.Gesture = (InputGesture) new GamepadGesture(GamepadInput.AButton);
      gamepadBinding2.SetBinding(InputBinding.CommandProperty, new Binding("ToggleSubscriptionCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_30.InputBindings.Add((InputBinding) gamepadBinding2);
      gamepadBinding2.Parent = (UIElement) this.e_30;
      GamepadBinding gamepadBinding3 = new GamepadBinding();
      gamepadBinding3.Gesture = (InputGesture) new GamepadGesture(GamepadInput.LeftShoulderButton);
      gamepadBinding3.SetBinding(InputBinding.CommandProperty, new Binding("PreviousPageCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_30.InputBindings.Add((InputBinding) gamepadBinding3);
      gamepadBinding3.Parent = (UIElement) this.e_30;
      GamepadBinding gamepadBinding4 = new GamepadBinding();
      gamepadBinding4.Gesture = (InputGesture) new GamepadGesture(GamepadInput.RightShoulderButton);
      gamepadBinding4.SetBinding(InputBinding.CommandProperty, new Binding("NextPageCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_30.InputBindings.Add((InputBinding) gamepadBinding4);
      gamepadBinding4.Parent = (UIElement) this.e_30;
      GamepadBinding gamepadBinding5 = new GamepadBinding();
      gamepadBinding5.Gesture = (InputGesture) new GamepadGesture(GamepadInput.SelectButton);
      gamepadBinding5.SetBinding(InputBinding.CommandProperty, new Binding("BrowseWorkshopCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_30.InputBindings.Add((InputBinding) gamepadBinding5);
      gamepadBinding5.Parent = (UIElement) this.e_30;
      GamepadBinding gamepadBinding6 = new GamepadBinding();
      gamepadBinding6.Gesture = (InputGesture) new GamepadGesture(GamepadInput.StartButton);
      gamepadBinding6.SetBinding(InputBinding.CommandProperty, new Binding("OpenItemInWorkshopCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_30.InputBindings.Add((InputBinding) gamepadBinding6);
      gamepadBinding6.Parent = (UIElement) this.e_30;
      this.e_30.TabIndex = 5;
      this.e_30.ItemTemplate = new DataTemplate(new Func<UIElement, UIElement>(WorkshopBrowserView_Gamepad.e_30_dtMethod));
      Grid.SetRow((UIElement) this.e_30, 1);
      GamepadHelp.SetTargetName((DependencyObject) this.e_30, "ListHelp");
      GamepadHelp.SetTabIndexRight((DependencyObject) this.e_30, 8);
      GamepadHelp.SetTabIndexUp((DependencyObject) this.e_30, 3);
      this.e_30.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("WorkshopItems")
      {
        UseGeneratedBindings = true
      });
      this.e_30.SetBinding(Selector.SelectedItemProperty, new Binding("SelectedWorkshopItem")
      {
        UseGeneratedBindings = true
      });
      this.e_41 = new Grid();
      this.e_6.Children.Add((UIElement) this.e_41);
      this.e_41.Name = "e_41";
      this.e_41.Background = (Brush) new SolidColorBrush(new ColorW(41, 54, 62, (int) byte.MaxValue));
      this.e_41.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_41.RowDefinitions.Add(new RowDefinition());
      this.e_41.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      Grid.SetColumn((UIElement) this.e_41, 1);
      Grid.SetRow((UIElement) this.e_41, 0);
      Grid.SetRowSpan((UIElement) this.e_41, 3);
      this.e_41.SetBinding(UIElement.VisibilityProperty, new Binding("IsDetailVisible")
      {
        UseGeneratedBindings = true
      });
      this.e_42 = new Grid();
      this.e_41.Children.Add((UIElement) this.e_42);
      this.e_42.Name = "e_42";
      this.e_42.Margin = new Thickness(10f, 10f, 10f, 10f);
      this.e_42.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_42.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_42.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_42.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_42.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_42.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_42.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_42.ColumnDefinitions.Add(new ColumnDefinition());
      this.e_43 = new ItemsControl();
      this.e_42.Children.Add((UIElement) this.e_43);
      this.e_43.Name = "e_43";
      this.e_43.ItemsPanel = new ControlTemplate(new Func<UIElement, UIElement>(WorkshopBrowserView_Gamepad.e_43_iptMethod));
      this.e_43.ItemTemplate = new DataTemplate(new Func<UIElement, UIElement>(WorkshopBrowserView_Gamepad.e_43_dtMethod));
      Grid.SetRow((UIElement) this.e_43, 0);
      Grid.SetColumnSpan((UIElement) this.e_43, 2);
      this.e_43.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("SelectedWorkshopItem.Rating")
      {
        UseGeneratedBindings = true
      });
      this.e_46 = new TextBlock();
      this.e_42.Children.Add((UIElement) this.e_46);
      this.e_46.Name = "e_46";
      this.e_46.Margin = new Thickness(0.0f, 0.0f, 0.0f, 10f);
      this.e_46.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      this.e_46.TextWrapping = TextWrapping.Wrap;
      Grid.SetRow((UIElement) this.e_46, 1);
      Grid.SetColumnSpan((UIElement) this.e_46, 2);
      this.e_46.SetBinding(TextBlock.TextProperty, new Binding("SelectedWorkshopItem.Title")
      {
        UseGeneratedBindings = true
      });
      this.e_47 = new TextBlock();
      this.e_42.Children.Add((UIElement) this.e_47);
      this.e_47.Name = "e_47";
      this.e_47.Margin = new Thickness(0.0f, 0.0f, 0.0f, 2f);
      this.e_47.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      this.e_47.FontFamily = new FontFamily("InventorySmall");
      this.e_47.FontSize = 10f;
      Grid.SetRow((UIElement) this.e_47, 2);
      this.e_47.SetResourceReference(TextBlock.TextProperty, (object) "WorkshopBrowser_FileSize");
      this.e_48 = new TextBlock();
      this.e_42.Children.Add((UIElement) this.e_48);
      this.e_48.Name = "e_48";
      this.e_48.Margin = new Thickness(20f, 0.0f, 0.0f, 2f);
      this.e_48.FontFamily = new FontFamily("InventorySmall");
      this.e_48.FontSize = 10f;
      Grid.SetColumn((UIElement) this.e_48, 1);
      Grid.SetRow((UIElement) this.e_48, 2);
      this.e_48.SetBinding(TextBlock.TextProperty, new Binding("SelectedWorkshopItem.Size")
      {
        UseGeneratedBindings = true,
        StringFormat = "{0:N0} B"
      });
      this.e_49 = new TextBlock();
      this.e_42.Children.Add((UIElement) this.e_49);
      this.e_49.Name = "e_49";
      this.e_49.Margin = new Thickness(0.0f, 0.0f, 0.0f, 2f);
      this.e_49.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      this.e_49.FontFamily = new FontFamily("InventorySmall");
      this.e_49.FontSize = 10f;
      Grid.SetRow((UIElement) this.e_49, 3);
      this.e_49.SetResourceReference(TextBlock.TextProperty, (object) "WorkshopBrowser_Subscribers");
      this.e_50 = new TextBlock();
      this.e_42.Children.Add((UIElement) this.e_50);
      this.e_50.Name = "e_50";
      this.e_50.Margin = new Thickness(20f, 0.0f, 0.0f, 2f);
      this.e_50.FontFamily = new FontFamily("InventorySmall");
      this.e_50.FontSize = 10f;
      Grid.SetColumn((UIElement) this.e_50, 1);
      Grid.SetRow((UIElement) this.e_50, 3);
      this.e_50.SetBinding(TextBlock.TextProperty, new Binding("SelectedWorkshopItem.NumSubscriptions")
      {
        UseGeneratedBindings = true,
        StringFormat = "{0:N0}"
      });
      this.e_51 = new TextBlock();
      this.e_42.Children.Add((UIElement) this.e_51);
      this.e_51.Name = "e_51";
      this.e_51.Margin = new Thickness(0.0f, 0.0f, 0.0f, 2f);
      this.e_51.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      this.e_51.FontFamily = new FontFamily("InventorySmall");
      this.e_51.FontSize = 10f;
      Grid.SetRow((UIElement) this.e_51, 4);
      this.e_51.SetResourceReference(TextBlock.TextProperty, (object) "WorkshopBrowser_Created");
      this.e_52 = new TextBlock();
      this.e_42.Children.Add((UIElement) this.e_52);
      this.e_52.Name = "e_52";
      this.e_52.Margin = new Thickness(20f, 0.0f, 0.0f, 2f);
      this.e_52.FontFamily = new FontFamily("InventorySmall");
      this.e_52.FontSize = 10f;
      Grid.SetColumn((UIElement) this.e_52, 1);
      Grid.SetRow((UIElement) this.e_52, 4);
      this.e_52.SetBinding(TextBlock.TextProperty, new Binding("SelectedWorkshopItem.TimeCreated")
      {
        UseGeneratedBindings = true
      });
      this.e_53 = new TextBlock();
      this.e_42.Children.Add((UIElement) this.e_53);
      this.e_53.Name = "e_53";
      this.e_53.Margin = new Thickness(0.0f, 0.0f, 0.0f, 2f);
      this.e_53.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      this.e_53.FontFamily = new FontFamily("InventorySmall");
      this.e_53.FontSize = 10f;
      Grid.SetRow((UIElement) this.e_53, 5);
      this.e_53.SetResourceReference(TextBlock.TextProperty, (object) "WorkshopBrowser_Updated");
      this.e_54 = new TextBlock();
      this.e_42.Children.Add((UIElement) this.e_54);
      this.e_54.Name = "e_54";
      this.e_54.Margin = new Thickness(20f, 0.0f, 0.0f, 2f);
      this.e_54.FontFamily = new FontFamily("InventorySmall");
      this.e_54.FontSize = 10f;
      Grid.SetColumn((UIElement) this.e_54, 1);
      Grid.SetRow((UIElement) this.e_54, 5);
      this.e_54.SetBinding(TextBlock.TextProperty, new Binding("SelectedWorkshopItem.TimeUpdated")
      {
        UseGeneratedBindings = true
      });
      this.e_55 = new ScrollViewer();
      this.e_41.Children.Add((UIElement) this.e_55);
      this.e_55.Name = "e_55";
      this.e_55.Margin = new Thickness(10f, 10f, 10f, 10f);
      this.e_55.HorizontalAlignment = HorizontalAlignment.Stretch;
      this.e_55.VerticalAlignment = VerticalAlignment.Stretch;
      Style style4 = new Style(typeof (ScrollViewer));
      Setter setter6 = new Setter(UIElement.SnapsToDevicePixelsProperty, (object) true);
      style4.Setters.Add(setter6);
      Setter setter7 = new Setter(Control.BorderBrushProperty, (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\button_default.dds"
        }
      });
      style4.Setters.Add(setter7);
      ControlTemplate controlTemplate4 = new ControlTemplate(typeof (ScrollViewer), new Func<UIElement, UIElement>(WorkshopBrowserView_Gamepad.e_55_s_S_2_ctMethod));
      Trigger trigger8 = new Trigger();
      trigger8.Property = UIElement.IsFocusedProperty;
      trigger8.Value = (object) true;
      Setter setter8 = new Setter(Control.BorderBrushProperty, (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\button_default_focus.dds"
        }
      });
      trigger8.Setters.Add(setter8);
      controlTemplate4.Triggers.Add((TriggerBase) trigger8);
      Trigger trigger9 = new Trigger();
      trigger9.Property = UIElement.IsFocusedProperty;
      trigger9.Value = (object) false;
      Setter setter9 = new Setter(Control.BorderBrushProperty, (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\button_default.dds"
        }
      });
      trigger9.Setters.Add(setter9);
      controlTemplate4.Triggers.Add((TriggerBase) trigger9);
      Setter setter10 = new Setter(Control.TemplateProperty, (object) controlTemplate4);
      style4.Setters.Add(setter10);
      this.e_55.Style = style4;
      this.e_55.HorizontalContentAlignment = HorizontalAlignment.Stretch;
      this.e_55.VerticalContentAlignment = VerticalAlignment.Stretch;
      this.e_55.IsTabStop = true;
      this.e_55.TabIndex = 8;
      Grid.SetRow((UIElement) this.e_55, 1);
      GamepadHelp.SetTargetName((DependencyObject) this.e_55, "SortHelp");
      GamepadHelp.SetTabIndexLeft((DependencyObject) this.e_55, 5);
      this.e_68 = new Border();
      this.e_55.Content = (object) this.e_68;
      this.e_68.Name = "e_68";
      this.e_68.HorizontalAlignment = HorizontalAlignment.Stretch;
      this.e_68.VerticalAlignment = VerticalAlignment.Stretch;
      this.e_68.Background = (Brush) new SolidColorBrush(new ColorW(33, 44, 53, (int) byte.MaxValue));
      this.e_69 = new TextBlock();
      this.e_68.Child = (UIElement) this.e_69;
      this.e_69.Name = "e_69";
      this.e_69.Margin = new Thickness(5f, 5f, 5f, 5f);
      this.e_69.TextWrapping = TextWrapping.Wrap;
      this.e_69.FontFamily = new FontFamily("InventorySmall");
      this.e_69.FontSize = 10f;
      this.e_69.SetBinding(TextBlock.TextProperty, new Binding("SelectedWorkshopItem.Description")
      {
        UseGeneratedBindings = true
      });
      this.e_70 = new Grid();
      this.e_6.Children.Add((UIElement) this.e_70);
      this.e_70.Name = "e_70";
      this.e_70.ColumnDefinitions.Add(new ColumnDefinition());
      this.e_70.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_70.ColumnDefinitions.Add(new ColumnDefinition());
      this.e_70.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_70.ColumnDefinitions.Add(new ColumnDefinition());
      Grid.SetRow((UIElement) this.e_70, 2);
      this.e_70.SetBinding(UIElement.VisibilityProperty, new Binding("IsPagingInfoVisible")
      {
        UseGeneratedBindings = true
      });
      this.e_71 = new ImageButton();
      this.e_70.Children.Add((UIElement) this.e_71);
      this.e_71.Name = "e_71";
      this.e_71.Width = 16f;
      this.e_71.HorizontalAlignment = HorizontalAlignment.Right;
      this.e_71.TabIndex = 6;
      this.e_71.ImageNormal = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_arrow_left.dds"
      };
      this.e_71.ImageDisabled = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_arrow_left.dds"
      };
      this.e_71.ImageHover = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_arrow_left_highlight.dds"
      };
      Grid.SetColumn((UIElement) this.e_71, 1);
      this.e_71.SetBinding(UIElement.IsEnabledProperty, new Binding("IsQueryFinished")
      {
        UseGeneratedBindings = true
      });
      this.e_71.SetBinding(Button.CommandProperty, new Binding("PreviousPageCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_71.SetResourceReference(UIElement.ToolTipProperty, (object) "WorkshopBrowser_PreviousPage");
      this.e_72 = new StackPanel();
      this.e_70.Children.Add((UIElement) this.e_72);
      this.e_72.Name = "e_72";
      this.e_72.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_72.VerticalAlignment = VerticalAlignment.Center;
      this.e_72.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) this.e_72, 2);
      this.e_73 = new TextBlock();
      this.e_72.Children.Add((UIElement) this.e_73);
      this.e_73.Name = "e_73";
      this.e_73.SetBinding(TextBlock.TextProperty, new Binding("CurrentPage")
      {
        UseGeneratedBindings = true
      });
      this.e_74 = new TextBlock();
      this.e_72.Children.Add((UIElement) this.e_74);
      this.e_74.Name = "e_74";
      this.e_74.Text = "/";
      this.e_75 = new TextBlock();
      this.e_72.Children.Add((UIElement) this.e_75);
      this.e_75.Name = "e_75";
      this.e_75.SetBinding(TextBlock.TextProperty, new Binding("TotalPages")
      {
        UseGeneratedBindings = true
      });
      this.e_76 = new ImageButton();
      this.e_70.Children.Add((UIElement) this.e_76);
      this.e_76.Name = "e_76";
      this.e_76.Width = 16f;
      this.e_76.HorizontalAlignment = HorizontalAlignment.Left;
      this.e_76.TabIndex = 7;
      this.e_76.ImageNormal = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_arrow_right.dds"
      };
      this.e_76.ImageDisabled = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_arrow_right.dds"
      };
      this.e_76.ImageHover = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_arrow_right_highlight.dds"
      };
      Grid.SetColumn((UIElement) this.e_76, 3);
      this.e_76.SetBinding(UIElement.IsEnabledProperty, new Binding("IsQueryFinished")
      {
        UseGeneratedBindings = true
      });
      this.e_76.SetBinding(Button.CommandProperty, new Binding("NextPageCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_76.SetResourceReference(UIElement.ToolTipProperty, (object) "WorkshopBrowser_NextPage");
      this.e_77 = new Border();
      this.e_6.Children.Add((UIElement) this.e_77);
      this.e_77.Name = "e_77";
      this.e_77.Height = 2f;
      this.e_77.Margin = new Thickness(0.0f, 10f, 0.0f, 0.0f);
      this.e_77.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      Grid.SetRow((UIElement) this.e_77, 3);
      Grid.SetColumnSpan((UIElement) this.e_77, 2);
      this.ListHelp = new Grid();
      this.e_6.Children.Add((UIElement) this.ListHelp);
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
      this.ListHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.ListHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.ListHelp.ColumnDefinitions.Add(new ColumnDefinition());
      Grid.SetRow((UIElement) this.ListHelp, 4);
      this.e_78 = new TextBlock();
      this.ListHelp.Children.Add((UIElement) this.e_78);
      this.e_78.Name = "e_78";
      this.e_78.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      this.e_78.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_78.VerticalAlignment = VerticalAlignment.Center;
      this.e_78.SetResourceReference(TextBlock.TextProperty, (object) "WorkshopBrowser_Help_ToggleSubscribe");
      this.e_79 = new TextBlock();
      this.ListHelp.Children.Add((UIElement) this.e_79);
      this.e_79.Name = "e_79";
      this.e_79.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      this.e_79.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_79.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) this.e_79, 1);
      this.e_79.SetResourceReference(TextBlock.TextProperty, (object) "WorkshopBrowser_Help_Paging");
      this.e_80 = new TextBlock();
      this.ListHelp.Children.Add((UIElement) this.e_80);
      this.e_80.Name = "e_80";
      this.e_80.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      this.e_80.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_80.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) this.e_80, 2);
      this.e_80.SetResourceReference(TextBlock.TextProperty, (object) "WorkshopBrowser_Help_OpenItem");
      this.e_81 = new TextBlock();
      this.ListHelp.Children.Add((UIElement) this.e_81);
      this.e_81.Name = "e_81";
      this.e_81.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      this.e_81.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_81.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) this.e_81, 3);
      this.e_81.SetResourceReference(TextBlock.TextProperty, (object) "WorkshopBrowser_Help_OpenWorkshop");
      this.e_82 = new TextBlock();
      this.ListHelp.Children.Add((UIElement) this.e_82);
      this.e_82.Name = "e_82";
      this.e_82.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      this.e_82.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_82.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) this.e_82, 4);
      this.e_82.SetResourceReference(TextBlock.TextProperty, (object) "WorkshopBrowser_Help_Refresh");
      this.e_83 = new TextBlock();
      this.ListHelp.Children.Add((UIElement) this.e_83);
      this.e_83.Name = "e_83";
      this.e_83.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      this.e_83.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_83.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) this.e_83, 5);
      this.e_83.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Back");
      this.SortHelp = new Grid();
      this.e_6.Children.Add((UIElement) this.SortHelp);
      this.SortHelp.Name = "SortHelp";
      this.SortHelp.Visibility = Visibility.Collapsed;
      this.SortHelp.VerticalAlignment = VerticalAlignment.Center;
      this.SortHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.SortHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.SortHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.SortHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.SortHelp.ColumnDefinitions.Add(new ColumnDefinition());
      Grid.SetRow((UIElement) this.SortHelp, 4);
      this.e_84 = new TextBlock();
      this.SortHelp.Children.Add((UIElement) this.e_84);
      this.e_84.Name = "e_84";
      this.e_84.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      this.e_84.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_84.VerticalAlignment = VerticalAlignment.Center;
      this.e_84.SetResourceReference(TextBlock.TextProperty, (object) "WorkshopBrowser_Help_Select");
      this.e_85 = new TextBlock();
      this.SortHelp.Children.Add((UIElement) this.e_85);
      this.e_85.Name = "e_85";
      this.e_85.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      this.e_85.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_85.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) this.e_85, 2);
      this.e_85.SetResourceReference(TextBlock.TextProperty, (object) "WorkshopBrowser_Help_Refresh");
      this.e_86 = new TextBlock();
      this.SortHelp.Children.Add((UIElement) this.e_86);
      this.e_86.Name = "e_86";
      this.e_86.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      this.e_86.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_86.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) this.e_86, 3);
      this.e_86.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Back");
      this.CategoryHelp = new Grid();
      this.e_6.Children.Add((UIElement) this.CategoryHelp);
      this.CategoryHelp.Name = "CategoryHelp";
      this.CategoryHelp.Visibility = Visibility.Collapsed;
      this.CategoryHelp.VerticalAlignment = VerticalAlignment.Center;
      this.CategoryHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.CategoryHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.CategoryHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.CategoryHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.CategoryHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.CategoryHelp.ColumnDefinitions.Add(new ColumnDefinition());
      Grid.SetRow((UIElement) this.CategoryHelp, 4);
      this.e_87 = new TextBlock();
      this.CategoryHelp.Children.Add((UIElement) this.e_87);
      this.e_87.Name = "e_87";
      this.e_87.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      this.e_87.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_87.VerticalAlignment = VerticalAlignment.Center;
      this.e_87.SetResourceReference(TextBlock.TextProperty, (object) "WorkhopBrowser_Help_OpenClose");
      this.e_88 = new TextBlock();
      this.CategoryHelp.Children.Add((UIElement) this.e_88);
      this.e_88.Name = "e_88";
      this.e_88.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      this.e_88.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_88.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) this.e_88, 1);
      this.e_88.SetResourceReference(TextBlock.TextProperty, (object) "WorkshopBrowser_Help_ToggleCategory");
      this.e_89 = new TextBlock();
      this.CategoryHelp.Children.Add((UIElement) this.e_89);
      this.e_89.Name = "e_89";
      this.e_89.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      this.e_89.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_89.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) this.e_89, 3);
      this.e_89.SetResourceReference(TextBlock.TextProperty, (object) "WorkshopBrowser_Help_Refresh");
      this.e_90 = new TextBlock();
      this.CategoryHelp.Children.Add((UIElement) this.e_90);
      this.e_90.Name = "e_90";
      this.e_90.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      this.e_90.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_90.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) this.e_90, 4);
      this.e_90.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Back");
      ImageManager.Instance.AddImage("Textures\\GUI\\Screens\\screen_background.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol_highlight.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\LoadingIconAnimated.png");
      ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Browser\\BackgroundFocused.png");
      ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Browser\\ModioCBFocused.png");
      ImageManager.Instance.AddImage("Textures\\GUI\\Bg16x9.png");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_default.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_default_focus.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_arrow_left.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_arrow_left_highlight.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_arrow_right.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_arrow_right_highlight.dds");
      FontManager.Instance.AddFont("InventorySmall", 10f, FontStyle.Regular, "InventorySmall_7.5_Regular");
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "MaxWidth", typeof (MyWorkshopBrowserViewModel_MaxWidth_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "BackgroundOverlay", typeof (MyWorkshopBrowserViewModel_BackgroundOverlay_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "ExitCommand", typeof (MyWorkshopBrowserViewModel_ExitCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "RefreshCommand", typeof (MyWorkshopBrowserViewModel_RefreshCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "SelectedSortIndex", typeof (MyWorkshopBrowserViewModel_SelectedSortIndex_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "Categories", typeof (MyWorkshopBrowserViewModel_Categories_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "SelectedCategoryIndex", typeof (MyWorkshopBrowserViewModel_SelectedCategoryIndex_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "CategoryControlTabIndexRight", typeof (MyWorkshopBrowserViewModel_CategoryControlTabIndexRight_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "IsRefreshing", typeof (MyWorkshopBrowserViewModel_IsRefreshing_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "IsWorkshopAggregator", typeof (MyWorkshopBrowserViewModel_IsWorkshopAggregator_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "ServiceCommand", typeof (MyWorkshopBrowserViewModel_ServiceCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "Service0IsChecked", typeof (MyWorkshopBrowserViewModel_Service0IsChecked_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "Service1IsChecked", typeof (MyWorkshopBrowserViewModel_Service1IsChecked_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "SearchText", typeof (MyWorkshopBrowserViewModel_SearchText_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "SearchControlTabIndexLeft", typeof (MyWorkshopBrowserViewModel_SearchControlTabIndexLeft_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "IsSearchLabelVisible", typeof (MyWorkshopBrowserViewModel_IsSearchLabelVisible_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "IsNotFoundTextVisible", typeof (MyWorkshopBrowserViewModel_IsNotFoundTextVisible_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "ToggleSubscriptionCommand", typeof (MyWorkshopBrowserViewModel_ToggleSubscriptionCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "PreviousPageCommand", typeof (MyWorkshopBrowserViewModel_PreviousPageCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "NextPageCommand", typeof (MyWorkshopBrowserViewModel_NextPageCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "BrowseWorkshopCommand", typeof (MyWorkshopBrowserViewModel_BrowseWorkshopCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "OpenItemInWorkshopCommand", typeof (MyWorkshopBrowserViewModel_OpenItemInWorkshopCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "WorkshopItems", typeof (MyWorkshopBrowserViewModel_WorkshopItems_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "SelectedWorkshopItem", typeof (MyWorkshopBrowserViewModel_SelectedWorkshopItem_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "IsDetailVisible", typeof (MyWorkshopBrowserViewModel_IsDetailVisible_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopItemModel), "Rating", typeof (MyWorkshopItemModel_Rating_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopItemModel), "Title", typeof (MyWorkshopItemModel_Title_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopItemModel), "Size", typeof (MyWorkshopItemModel_Size_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopItemModel), "NumSubscriptions", typeof (MyWorkshopItemModel_NumSubscriptions_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopItemModel), "TimeCreated", typeof (MyWorkshopItemModel_TimeCreated_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopItemModel), "TimeUpdated", typeof (MyWorkshopItemModel_TimeUpdated_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopItemModel), "Description", typeof (MyWorkshopItemModel_Description_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "IsPagingInfoVisible", typeof (MyWorkshopBrowserViewModel_IsPagingInfoVisible_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "IsQueryFinished", typeof (MyWorkshopBrowserViewModel_IsQueryFinished_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "CurrentPage", typeof (MyWorkshopBrowserViewModel_CurrentPage_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "TotalPages", typeof (MyWorkshopBrowserViewModel_TotalPages_PropertyInfo));
    }

    private static void InitializeElementResources(UIElement elem)
    {
      elem.Resources.MergedDictionaries.Add((ResourceDictionary) Styles.Instance);
      Style style = new Style(typeof (ListBoxItem), elem.Resources[(object) typeof (ListBoxItem)] as Style);
      Setter setter = new Setter(UIElement.MarginProperty, (object) new Thickness(0.0f, 8f, 8f, 0.0f));
      style.Setters.Add(setter);
      elem.Resources.Add((object) typeof (ListBoxItem), (object) style);
    }

    private static ObservableCollection<object> Get_e_8_Items()
    {
      ObservableCollection<object> observableCollection = new ObservableCollection<object>();
      ComboBoxItem comboBoxItem1 = new ComboBoxItem();
      comboBoxItem1.Name = "e_9";
      comboBoxItem1.SetResourceReference(ContentControl.ContentProperty, (object) "WorkshopBrowser_MostPopular");
      observableCollection.Add((object) comboBoxItem1);
      ComboBoxItem comboBoxItem2 = new ComboBoxItem();
      comboBoxItem2.Name = "e_10";
      comboBoxItem2.SetResourceReference(ContentControl.ContentProperty, (object) "WorkshopBrowser_MostRecent");
      observableCollection.Add((object) comboBoxItem2);
      ComboBoxItem comboBoxItem3 = new ComboBoxItem();
      comboBoxItem3.Name = "e_11";
      comboBoxItem3.SetResourceReference(ContentControl.ContentProperty, (object) "WorkshopBrowser_MostSubscribed");
      observableCollection.Add((object) comboBoxItem3);
      ComboBoxItem comboBoxItem4 = new ComboBoxItem();
      comboBoxItem4.Name = "e_12";
      comboBoxItem4.SetResourceReference(ContentControl.ContentProperty, (object) "WorkshopBrowser_Subscribed");
      observableCollection.Add((object) comboBoxItem4);
      return observableCollection;
    }

    private static UIElement e_13_dtMethod(UIElement parent)
    {
      Grid grid = new Grid();
      grid.Parent = parent;
      grid.Name = "e_14";
      grid.Margin = new Thickness(2f, 2f, 2f, 2f);
      GamepadBinding gamepadBinding = new GamepadBinding();
      gamepadBinding.Gesture = (InputGesture) new GamepadGesture(GamepadInput.CButton);
      Binding binding1 = new Binding("ToggleCategoryCommand");
      gamepadBinding.SetBinding(InputBinding.CommandProperty, binding1);
      grid.InputBindings.Add((InputBinding) gamepadBinding);
      gamepadBinding.Parent = (UIElement) grid;
      grid.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition = new ColumnDefinition();
      grid.ColumnDefinitions.Add(columnDefinition);
      CheckBox checkBox = new CheckBox();
      grid.Children.Add((UIElement) checkBox);
      checkBox.Name = "e_15";
      checkBox.VerticalAlignment = VerticalAlignment.Center;
      Binding binding2 = new Binding("IsChecked");
      checkBox.SetBinding(ToggleButton.IsCheckedProperty, binding2);
      TextBlock textBlock = new TextBlock();
      grid.Children.Add((UIElement) textBlock);
      textBlock.Name = "e_16";
      textBlock.Margin = new Thickness(2f, 0.0f, 0.0f, 0.0f);
      textBlock.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock, 1);
      Binding binding3 = new Binding("LocalizedName");
      textBlock.SetBinding(TextBlock.TextProperty, binding3);
      return (UIElement) grid;
    }

    private static UIElement e_18_s_S_0_ctMethod(UIElement parent)
    {
      StackPanel stackPanel = new StackPanel();
      stackPanel.Parent = parent;
      stackPanel.Name = "e_19";
      stackPanel.Orientation = Orientation.Horizontal;
      Grid grid = new Grid();
      stackPanel.Children.Add((UIElement) grid);
      grid.Name = "e_20";
      Border border1 = new Border();
      grid.Children.Add((UIElement) border1);
      border1.Name = "PART_Background";
      border1.Height = 38f;
      border1.Width = 38f;
      border1.SetResourceReference(Control.BackgroundProperty, (object) "CheckServiceBack");
      Border border2 = new Border();
      grid.Children.Add((UIElement) border2);
      border2.Name = "PART_Icon";
      border2.Height = 38f;
      border2.Width = 38f;
      border2.SetResourceReference(Control.BackgroundProperty, (object) "CheckService0");
      ContentPresenter contentPresenter = new ContentPresenter();
      stackPanel.Children.Add((UIElement) contentPresenter);
      contentPresenter.Name = "e_21";
      contentPresenter.VerticalAlignment = VerticalAlignment.Center;
      contentPresenter.SetBinding(ContentPresenter.ContentProperty, new Binding("Content")
      {
        Source = (object) parent
      });
      return (UIElement) stackPanel;
    }

    private static UIElement e_22_s_S_0_ctMethod(UIElement parent)
    {
      StackPanel stackPanel = new StackPanel();
      stackPanel.Parent = parent;
      stackPanel.Name = "e_23";
      stackPanel.Orientation = Orientation.Horizontal;
      Grid grid = new Grid();
      stackPanel.Children.Add((UIElement) grid);
      grid.Name = "e_24";
      Border border1 = new Border();
      grid.Children.Add((UIElement) border1);
      border1.Name = "PART_Background";
      border1.Height = 38f;
      border1.Width = 38f;
      border1.SetResourceReference(Control.BackgroundProperty, (object) "CheckServiceBack");
      Border border2 = new Border();
      grid.Children.Add((UIElement) border2);
      border2.Name = "PART_Icon";
      border2.Height = 38f;
      border2.Width = 38f;
      border2.SetResourceReference(Control.BackgroundProperty, (object) "CheckService1");
      ContentPresenter contentPresenter = new ContentPresenter();
      stackPanel.Children.Add((UIElement) contentPresenter);
      contentPresenter.Name = "e_25";
      contentPresenter.VerticalAlignment = VerticalAlignment.Center;
      contentPresenter.SetBinding(ContentPresenter.ContentProperty, new Binding("Content")
      {
        Source = (object) parent
      });
      return (UIElement) stackPanel;
    }

    private static UIElement e_30_s_S_1_ctMethod(UIElement parent)
    {
      Border border = new Border();
      border.Parent = parent;
      border.Name = "e_31";
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
      UniformGrid uniformGrid = new UniformGrid();
      border.Child = (UIElement) uniformGrid;
      uniformGrid.Name = "e_32";
      uniformGrid.Margin = new Thickness(5f, 5f, 5f, 5f);
      uniformGrid.IsItemsHost = true;
      uniformGrid.Rows = 3;
      uniformGrid.Columns = 3;
      return (UIElement) border;
    }

    private static UIElement e_37_iptMethod(UIElement parent)
    {
      StackPanel stackPanel = new StackPanel();
      stackPanel.Parent = parent;
      stackPanel.Name = "e_38";
      stackPanel.IsItemsHost = true;
      stackPanel.Orientation = Orientation.Horizontal;
      return (UIElement) stackPanel;
    }

    private static UIElement e_37_dtMethod(UIElement parent)
    {
      Image image = new Image();
      image.Parent = parent;
      image.Name = "e_39";
      image.Height = 16f;
      image.Width = 16f;
      image.Margin = new Thickness(2f, 2f, 2f, 2f);
      Binding binding = new Binding();
      image.SetBinding(Image.SourceProperty, binding);
      return (UIElement) image;
    }

    private static UIElement e_30_dtMethod(UIElement parent)
    {
      Grid grid = new Grid();
      grid.Parent = parent;
      grid.Name = "e_33";
      grid.Margin = new Thickness(0.0f, 4f, 0.0f, 0.0f);
      grid.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Star)
      });
      grid.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid.ColumnDefinitions.Add(new ColumnDefinition());
      Image image1 = new Image();
      grid.Children.Add((UIElement) image1);
      image1.Name = "e_34";
      image1.HorizontalAlignment = HorizontalAlignment.Center;
      image1.Source = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Bg16x9.png"
      };
      image1.Stretch = Stretch.Uniform;
      Image image2 = new Image();
      grid.Children.Add((UIElement) image2);
      image2.Name = "e_35";
      image2.HorizontalAlignment = HorizontalAlignment.Center;
      image2.Stretch = Stretch.Uniform;
      Binding binding1 = new Binding("PreviewImage");
      image2.SetBinding(Image.SourceProperty, binding1);
      CheckBox checkBox = new CheckBox();
      grid.Children.Add((UIElement) checkBox);
      checkBox.Name = "e_36";
      checkBox.Height = 24f;
      checkBox.Width = 24f;
      checkBox.HorizontalAlignment = HorizontalAlignment.Right;
      checkBox.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetRow((UIElement) checkBox, 1);
      Binding binding2 = new Binding("IsSubscribed");
      checkBox.SetBinding(ToggleButton.IsCheckedProperty, binding2);
      checkBox.SetResourceReference(UIElement.ToolTipProperty, (object) "WorkshopBrowser_Subscribe");
      ItemsControl itemsControl = new ItemsControl();
      grid.Children.Add((UIElement) itemsControl);
      itemsControl.Name = "e_37";
      itemsControl.Margin = new Thickness(4f, 2f, 2f, 2f);
      itemsControl.HorizontalAlignment = HorizontalAlignment.Left;
      itemsControl.VerticalAlignment = VerticalAlignment.Center;
      ControlTemplate controlTemplate = new ControlTemplate(new Func<UIElement, UIElement>(WorkshopBrowserView_Gamepad.e_37_iptMethod));
      itemsControl.ItemsPanel = controlTemplate;
      Func<UIElement, UIElement> createMethod = new Func<UIElement, UIElement>(WorkshopBrowserView_Gamepad.e_37_dtMethod);
      itemsControl.ItemTemplate = new DataTemplate(createMethod);
      Grid.SetRow((UIElement) itemsControl, 1);
      Binding binding3 = new Binding("Rating");
      itemsControl.SetBinding(ItemsControl.ItemsSourceProperty, binding3);
      TextBlock textBlock = new TextBlock();
      grid.Children.Add((UIElement) textBlock);
      textBlock.Name = "e_40";
      textBlock.Margin = new Thickness(4f, 0.0f, 2f, 2f);
      Grid.SetRow((UIElement) textBlock, 2);
      Binding binding4 = new Binding("Title");
      textBlock.SetBinding(TextBlock.TextProperty, binding4);
      return (UIElement) grid;
    }

    private static UIElement e_43_iptMethod(UIElement parent)
    {
      StackPanel stackPanel = new StackPanel();
      stackPanel.Parent = parent;
      stackPanel.Name = "e_44";
      stackPanel.IsItemsHost = true;
      stackPanel.Orientation = Orientation.Horizontal;
      return (UIElement) stackPanel;
    }

    private static UIElement e_43_dtMethod(UIElement parent)
    {
      Image image = new Image();
      image.Parent = parent;
      image.Name = "e_45";
      image.Height = 24f;
      image.Width = 24f;
      image.Margin = new Thickness(2f, 2f, 2f, 2f);
      Binding binding = new Binding();
      image.SetBinding(Image.SourceProperty, binding);
      return (UIElement) image;
    }

    private static UIElement e_55_s_S_2_ctMethod(UIElement parent)
    {
      Border border1 = new Border();
      border1.Parent = parent;
      border1.Name = "e_56";
      border1.BorderThickness = new Thickness(2f, 2f, 2f, 2f);
      border1.SetBinding(Control.BorderBrushProperty, new Binding("BorderBrush")
      {
        Source = (object) parent
      });
      Grid grid1 = new Grid();
      border1.Child = (UIElement) grid1;
      grid1.Name = "e_57";
      RowDefinition rowDefinition1 = new RowDefinition();
      grid1.RowDefinitions.Add(rowDefinition1);
      grid1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition1 = new ColumnDefinition();
      grid1.ColumnDefinitions.Add(columnDefinition1);
      grid1.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      Grid grid2 = new Grid();
      grid1.Children.Add((UIElement) grid2);
      grid2.Name = "e_58";
      grid2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(4f, GridUnitType.Pixel)
      });
      RowDefinition rowDefinition2 = new RowDefinition();
      grid2.RowDefinitions.Add(rowDefinition2);
      grid2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(4f, GridUnitType.Pixel)
      });
      grid2.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(4f, GridUnitType.Pixel)
      });
      ColumnDefinition columnDefinition2 = new ColumnDefinition();
      grid2.ColumnDefinitions.Add(columnDefinition2);
      grid2.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(32f, GridUnitType.Pixel)
      });
      Grid.SetColumnSpan((UIElement) grid2, 2);
      Grid.SetRowSpan((UIElement) grid2, 2);
      Border border2 = new Border();
      grid2.Children.Add((UIElement) border2);
      border2.Name = "e_59";
      border2.IsHitTestVisible = false;
      border2.SnapsToDevicePixels = false;
      border2.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListLeftTop");
      Border border3 = new Border();
      grid2.Children.Add((UIElement) border3);
      border3.Name = "e_60";
      border3.IsHitTestVisible = false;
      border3.SnapsToDevicePixels = false;
      Grid.SetColumn((UIElement) border3, 1);
      border3.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListCenterTop");
      Border border4 = new Border();
      grid2.Children.Add((UIElement) border4);
      border4.Name = "e_61";
      border4.IsHitTestVisible = false;
      border4.SnapsToDevicePixels = false;
      Grid.SetColumn((UIElement) border4, 2);
      border4.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListRightTop");
      Border border5 = new Border();
      grid2.Children.Add((UIElement) border5);
      border5.Name = "e_62";
      border5.IsHitTestVisible = false;
      border5.SnapsToDevicePixels = false;
      Grid.SetRow((UIElement) border5, 1);
      border5.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListLeftCenter");
      Border border6 = new Border();
      grid2.Children.Add((UIElement) border6);
      border6.Name = "e_63";
      border6.IsHitTestVisible = false;
      border6.SnapsToDevicePixels = false;
      Grid.SetColumn((UIElement) border6, 1);
      Grid.SetRow((UIElement) border6, 1);
      border6.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListCenter");
      Border border7 = new Border();
      grid2.Children.Add((UIElement) border7);
      border7.Name = "e_64";
      border7.IsHitTestVisible = false;
      border7.SnapsToDevicePixels = false;
      Grid.SetColumn((UIElement) border7, 2);
      Grid.SetRow((UIElement) border7, 1);
      border7.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListRightCenter");
      Border border8 = new Border();
      grid2.Children.Add((UIElement) border8);
      border8.Name = "e_65";
      border8.IsHitTestVisible = false;
      border8.SnapsToDevicePixels = false;
      Grid.SetRow((UIElement) border8, 2);
      border8.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListLeftBottom");
      Border border9 = new Border();
      grid2.Children.Add((UIElement) border9);
      border9.Name = "e_66";
      border9.IsHitTestVisible = false;
      border9.SnapsToDevicePixels = false;
      Grid.SetColumn((UIElement) border9, 1);
      Grid.SetRow((UIElement) border9, 2);
      border9.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListCenterBottom");
      Border border10 = new Border();
      grid2.Children.Add((UIElement) border10);
      border10.Name = "e_67";
      border10.IsHitTestVisible = false;
      border10.SnapsToDevicePixels = false;
      Grid.SetColumn((UIElement) border10, 2);
      Grid.SetRow((UIElement) border10, 2);
      border10.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListRightBottom");
      ScrollContentPresenter contentPresenter = new ScrollContentPresenter();
      grid1.Children.Add((UIElement) contentPresenter);
      contentPresenter.Name = "PART_ScrollContentPresenter";
      contentPresenter.SnapsToDevicePixels = true;
      contentPresenter.SetBinding(UIElement.MarginProperty, new Binding("Padding")
      {
        Source = (object) parent
      });
      contentPresenter.SetBinding(UIElement.HorizontalAlignmentProperty, new Binding("HorizontalContentAlignment")
      {
        Source = (object) parent
      });
      contentPresenter.SetBinding(UIElement.VerticalAlignmentProperty, new Binding("VerticalContentAlignment")
      {
        Source = (object) parent
      });
      contentPresenter.SetBinding(ContentPresenter.ContentProperty, new Binding("Content")
      {
        Source = (object) parent
      });
      ScrollBar scrollBar1 = new ScrollBar();
      grid1.Children.Add((UIElement) scrollBar1);
      scrollBar1.Name = "PART_VerticalScrollBar";
      scrollBar1.Width = 32f;
      scrollBar1.Minimum = 0.0f;
      scrollBar1.Orientation = Orientation.Vertical;
      Grid.SetColumn((UIElement) scrollBar1, 1);
      scrollBar1.SetBinding(UIElement.VisibilityProperty, new Binding("ComputedVerticalScrollBarVisibility")
      {
        Source = (object) parent
      });
      scrollBar1.SetBinding(ScrollBar.ViewportSizeProperty, new Binding("ViewportHeight")
      {
        Source = (object) parent
      });
      scrollBar1.SetBinding(RangeBase.MaximumProperty, new Binding("ScrollableHeight")
      {
        Source = (object) parent
      });
      scrollBar1.SetBinding(RangeBase.ValueProperty, new Binding("VerticalOffset")
      {
        Source = (object) parent
      });
      ScrollBar scrollBar2 = new ScrollBar();
      grid1.Children.Add((UIElement) scrollBar2);
      scrollBar2.Name = "PART_HorizontalScrollBar";
      scrollBar2.Height = 32f;
      scrollBar2.Minimum = 0.0f;
      scrollBar2.Orientation = Orientation.Horizontal;
      Grid.SetRow((UIElement) scrollBar2, 1);
      scrollBar2.SetBinding(UIElement.VisibilityProperty, new Binding("ComputedHorizontalScrollBarVisibility")
      {
        Source = (object) parent
      });
      scrollBar2.SetBinding(ScrollBar.ViewportSizeProperty, new Binding("ViewportWidth")
      {
        Source = (object) parent
      });
      scrollBar2.SetBinding(RangeBase.MaximumProperty, new Binding("ScrollableWidth")
      {
        Source = (object) parent
      });
      scrollBar2.SetBinding(RangeBase.ValueProperty, new Binding("HorizontalOffset")
      {
        Source = (object) parent
      });
      return (UIElement) border1;
    }
  }
}
