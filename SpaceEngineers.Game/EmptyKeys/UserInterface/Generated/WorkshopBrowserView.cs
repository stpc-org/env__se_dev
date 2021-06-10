// Decompiled with JetBrains decompiler
// Type: EmptyKeys.UserInterface.Generated.WorkshopBrowserView
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Controls.Primitives;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.WorkshopBrowserView_Bindings;
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
  public class WorkshopBrowserView : UIRoot
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
    private ImageButton e_17;
    private AnimatedImage e_18;
    private CheckBox e_19;
    private CheckBox e_23;
    private Grid e_27;
    private TextBox e_28;
    private TextBlock e_29;
    private ImageButton e_30;
    private TextBlock e_31;
    private ListBox e_32;
    private Grid e_43;
    private Grid e_44;
    private ItemsControl e_45;
    private TextBlock e_48;
    private TextBlock e_49;
    private TextBlock e_50;
    private TextBlock e_51;
    private TextBlock e_52;
    private TextBlock e_53;
    private TextBlock e_54;
    private TextBlock e_55;
    private TextBlock e_56;
    private ScrollViewer e_57;
    private Border e_70;
    private TextBlock e_71;
    private Grid e_72;
    private ImageButton e_73;
    private StackPanel e_74;
    private TextBlock e_75;
    private TextBlock e_76;
    private TextBlock e_77;
    private ImageButton e_78;
    private Border e_79;
    private Grid e_80;
    private Button e_81;
    private Button e_82;

    public WorkshopBrowserView() => this.Initialize();

    public WorkshopBrowserView(int width, int height)
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
      WorkshopBrowserView.InitializeElementResources((UIElement) this);
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
        Height = new GridLength(1f, GridUnitType.Auto)
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
      this.e_8.ItemsSource = (IEnumerable) WorkshopBrowserView.Get_e_8_Items();
      this.e_8.MaxDropDownHeight = 120f;
      GamepadHelp.SetTabIndexRight((DependencyObject) this.e_8, 2);
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
      this.e_13.TabIndex = 2;
      this.e_13.ItemTemplate = new DataTemplate(new Func<UIElement, UIElement>(WorkshopBrowserView.e_13_dtMethod));
      this.e_13.MaxDropDownHeight = 300f;
      Grid.SetColumn((UIElement) this.e_13, 1);
      GamepadHelp.SetTabIndexLeft((DependencyObject) this.e_13, 1);
      GamepadHelp.SetTabIndexRight((DependencyObject) this.e_13, 3);
      GamepadHelp.SetTabIndexDown((DependencyObject) this.e_13, 5);
      this.e_13.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("Categories")
      {
        UseGeneratedBindings = true
      });
      this.e_13.SetBinding(Selector.SelectedIndexProperty, new Binding("SelectedCategoryIndex")
      {
        UseGeneratedBindings = true
      });
      this.e_17 = new ImageButton();
      this.e_7.Children.Add((UIElement) this.e_17);
      this.e_17.Name = "e_17";
      this.e_17.Width = 24f;
      this.e_17.Margin = new Thickness(4f, 0.0f, 4f, 0.0f);
      this.e_17.VerticalAlignment = VerticalAlignment.Center;
      this.e_17.TabIndex = 3;
      this.e_17.ImageStretch = Stretch.Uniform;
      this.e_17.ImageNormal = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Icons\\Blueprints\\Refresh.png"
      };
      this.e_17.ImageHover = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Icons\\Blueprints\\Refresh.png"
      };
      Grid.SetColumn((UIElement) this.e_17, 2);
      GamepadHelp.SetTabIndexLeft((DependencyObject) this.e_17, 2);
      GamepadHelp.SetTabIndexDown((DependencyObject) this.e_17, 5);
      this.e_17.SetBinding(UIElement.IsEnabledProperty, new Binding("IsQueryFinished")
      {
        UseGeneratedBindings = true
      });
      this.e_17.SetBinding(Button.CommandProperty, new Binding("RefreshCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_17.SetBinding(GamepadHelp.TabIndexRightProperty, new Binding("CategoryControlTabIndexRight")
      {
        UseGeneratedBindings = true
      });
      this.e_17.SetResourceReference(UIElement.ToolTipProperty, (object) "WorkshopBrowser_Refresh");
      this.e_18 = new AnimatedImage();
      this.e_7.Children.Add((UIElement) this.e_18);
      this.e_18.Name = "e_18";
      this.e_18.Width = 24f;
      this.e_18.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_18.VerticalAlignment = VerticalAlignment.Center;
      this.e_18.Source = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\LoadingIconAnimated.png"
      };
      this.e_18.FrameWidth = 128;
      this.e_18.FrameHeight = 128;
      this.e_18.FramesPerSecond = 20;
      Grid.SetColumn((UIElement) this.e_18, 3);
      this.e_18.SetBinding(UIElement.VisibilityProperty, new Binding("IsRefreshing")
      {
        UseGeneratedBindings = true
      });
      this.e_19 = new CheckBox();
      this.e_7.Children.Add((UIElement) this.e_19);
      this.e_19.Name = "e_19";
      this.e_19.IsEnabled = true;
      this.e_19.Margin = new Thickness(0.0f, 0.0f, 0.0f, 0.0f);
      this.e_19.VerticalAlignment = VerticalAlignment.Center;
      Style style1 = new Style(typeof (CheckBox));
      ControlTemplate controlTemplate1 = new ControlTemplate(typeof (CheckBox), new Func<UIElement, UIElement>(WorkshopBrowserView.e_19_s_S_0_ctMethod));
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
      this.e_19.Style = style1;
      this.e_19.TabIndex = 11;
      Grid.SetColumn((UIElement) this.e_19, 4);
      GamepadHelp.SetTabIndexLeft((DependencyObject) this.e_19, 3);
      GamepadHelp.SetTabIndexRight((DependencyObject) this.e_19, 12);
      GamepadHelp.SetTabIndexDown((DependencyObject) this.e_19, 5);
      this.e_19.SetBinding(UIElement.VisibilityProperty, new Binding("IsWorkshopAggregator")
      {
        UseGeneratedBindings = true
      });
      this.e_19.SetBinding(Button.CommandProperty, new Binding("ServiceCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_19.SetBinding(ToggleButton.IsCheckedProperty, new Binding("Service0IsChecked")
      {
        UseGeneratedBindings = true
      });
      this.e_19.SetResourceReference(UIElement.ToolTipProperty, (object) "WorkshopBrowser_Service0");
      this.e_23 = new CheckBox();
      this.e_7.Children.Add((UIElement) this.e_23);
      this.e_23.Name = "e_23";
      this.e_23.IsEnabled = true;
      this.e_23.Margin = new Thickness(0.0f, 0.0f, 0.0f, 0.0f);
      this.e_23.VerticalAlignment = VerticalAlignment.Center;
      Style style2 = new Style(typeof (CheckBox));
      ControlTemplate controlTemplate2 = new ControlTemplate(typeof (CheckBox), new Func<UIElement, UIElement>(WorkshopBrowserView.e_23_s_S_0_ctMethod));
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
      this.e_23.Style = style2;
      this.e_23.TabIndex = 12;
      Grid.SetColumn((UIElement) this.e_23, 5);
      GamepadHelp.SetTabIndexLeft((DependencyObject) this.e_23, 11);
      GamepadHelp.SetTabIndexRight((DependencyObject) this.e_23, 4);
      GamepadHelp.SetTabIndexDown((DependencyObject) this.e_23, 5);
      this.e_23.SetBinding(UIElement.VisibilityProperty, new Binding("IsWorkshopAggregator")
      {
        UseGeneratedBindings = true
      });
      this.e_23.SetBinding(Button.CommandProperty, new Binding("ServiceCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_23.SetBinding(ToggleButton.IsCheckedProperty, new Binding("Service1IsChecked")
      {
        UseGeneratedBindings = true
      });
      this.e_23.SetResourceReference(UIElement.ToolTipProperty, (object) "WorkshopBrowser_Service1");
      this.e_27 = new Grid();
      this.e_7.Children.Add((UIElement) this.e_27);
      this.e_27.Name = "e_27";
      Grid.SetColumn((UIElement) this.e_27, 6);
      this.e_28 = new TextBox();
      this.e_27.Children.Add((UIElement) this.e_28);
      this.e_28.Name = "e_28";
      this.e_28.VerticalAlignment = VerticalAlignment.Center;
      this.e_28.TabIndex = 4;
      GamepadHelp.SetTabIndexDown((DependencyObject) this.e_28, 5);
      this.e_28.SetBinding(TextBox.TextProperty, new Binding("SearchText")
      {
        UseGeneratedBindings = true
      });
      this.e_28.SetBinding(GamepadHelp.TabIndexLeftProperty, new Binding("SearchControlTabIndexLeft")
      {
        UseGeneratedBindings = true
      });
      this.e_29 = new TextBlock();
      this.e_27.Children.Add((UIElement) this.e_29);
      this.e_29.Name = "e_29";
      this.e_29.IsHitTestVisible = false;
      this.e_29.Margin = new Thickness(4f, 4f, 4f, 4f);
      this.e_29.VerticalAlignment = VerticalAlignment.Center;
      this.e_29.Foreground = (Brush) new SolidColorBrush(new ColorW(94, 115, (int) sbyte.MaxValue, (int) byte.MaxValue));
      this.e_29.SetBinding(UIElement.VisibilityProperty, new Binding("IsSearchLabelVisible")
      {
        UseGeneratedBindings = true
      });
      this.e_29.SetResourceReference(TextBlock.TextProperty, (object) "WorkshopBrowser_Search");
      this.e_30 = new ImageButton();
      this.e_27.Children.Add((UIElement) this.e_30);
      this.e_30.Name = "e_30";
      this.e_30.Height = 24f;
      this.e_30.Margin = new Thickness(0.0f, 0.0f, 4f, 0.0f);
      this.e_30.HorizontalAlignment = HorizontalAlignment.Right;
      this.e_30.VerticalAlignment = VerticalAlignment.Center;
      this.e_30.ImageStretch = Stretch.Uniform;
      this.e_30.ImageNormal = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol.dds"
      };
      this.e_30.ImageHover = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds"
      };
      this.e_30.ImagePressed = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds"
      };
      this.e_30.SetBinding(Button.CommandProperty, new Binding("ClearSearchTextCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_31 = new TextBlock();
      this.e_6.Children.Add((UIElement) this.e_31);
      this.e_31.Name = "e_31";
      this.e_31.Margin = new Thickness(0.0f, 10f, 10f, 10f);
      this.e_31.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_31.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetRow((UIElement) this.e_31, 1);
      Grid.SetColumnSpan((UIElement) this.e_31, 2);
      this.e_31.SetBinding(UIElement.VisibilityProperty, new Binding("IsNotFoundTextVisible")
      {
        UseGeneratedBindings = true
      });
      this.e_31.SetResourceReference(TextBlock.TextProperty, (object) "WorkshopBrowser_NotFound");
      this.e_32 = new ListBox();
      this.e_6.Children.Add((UIElement) this.e_32);
      this.e_32.Name = "e_32";
      this.e_32.Margin = new Thickness(0.0f, 10f, 10f, 10f);
      this.e_32.HorizontalAlignment = HorizontalAlignment.Stretch;
      this.e_32.VerticalAlignment = VerticalAlignment.Stretch;
      Style style3 = new Style(typeof (ListBox));
      Setter setter3 = new Setter(UIElement.MinHeightProperty, (object) 80f);
      style3.Setters.Add(setter3);
      ControlTemplate controlTemplate3 = new ControlTemplate(typeof (ListBox), new Func<UIElement, UIElement>(WorkshopBrowserView.e_32_s_S_1_ctMethod));
      Setter setter4 = new Setter(Control.TemplateProperty, (object) controlTemplate3);
      style3.Setters.Add(setter4);
      Trigger trigger7 = new Trigger();
      trigger7.Property = UIElement.IsFocusedProperty;
      trigger7.Value = (object) true;
      Setter setter5 = new Setter(Control.BackgroundProperty, (object) new SolidColorBrush(new ColorW(67, 81, 92, 160)));
      trigger7.Setters.Add(setter5);
      style3.Triggers.Add((TriggerBase) trigger7);
      this.e_32.Style = style3;
      this.e_32.TabIndex = 5;
      this.e_32.ItemTemplate = new DataTemplate(new Func<UIElement, UIElement>(WorkshopBrowserView.e_32_dtMethod));
      Grid.SetRow((UIElement) this.e_32, 1);
      GamepadHelp.SetTabIndexRight((DependencyObject) this.e_32, 10);
      GamepadHelp.SetTabIndexUp((DependencyObject) this.e_32, 2);
      GamepadHelp.SetTabIndexDown((DependencyObject) this.e_32, 7);
      this.e_32.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("WorkshopItems")
      {
        UseGeneratedBindings = true
      });
      this.e_32.SetBinding(Selector.SelectedItemProperty, new Binding("SelectedWorkshopItem")
      {
        UseGeneratedBindings = true
      });
      this.e_43 = new Grid();
      this.e_6.Children.Add((UIElement) this.e_43);
      this.e_43.Name = "e_43";
      this.e_43.Background = (Brush) new SolidColorBrush(new ColorW(41, 54, 62, (int) byte.MaxValue));
      this.e_43.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_43.RowDefinitions.Add(new RowDefinition());
      Grid.SetColumn((UIElement) this.e_43, 1);
      Grid.SetRow((UIElement) this.e_43, 0);
      Grid.SetRowSpan((UIElement) this.e_43, 3);
      this.e_43.SetBinding(UIElement.VisibilityProperty, new Binding("IsDetailVisible")
      {
        UseGeneratedBindings = true
      });
      this.e_44 = new Grid();
      this.e_43.Children.Add((UIElement) this.e_44);
      this.e_44.Name = "e_44";
      this.e_44.Margin = new Thickness(10f, 10f, 10f, 10f);
      this.e_44.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_44.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_44.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_44.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_44.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_44.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_44.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_44.ColumnDefinitions.Add(new ColumnDefinition());
      this.e_45 = new ItemsControl();
      this.e_44.Children.Add((UIElement) this.e_45);
      this.e_45.Name = "e_45";
      this.e_45.ItemsPanel = new ControlTemplate(new Func<UIElement, UIElement>(WorkshopBrowserView.e_45_iptMethod));
      this.e_45.ItemTemplate = new DataTemplate(new Func<UIElement, UIElement>(WorkshopBrowserView.e_45_dtMethod));
      Grid.SetRow((UIElement) this.e_45, 0);
      Grid.SetColumnSpan((UIElement) this.e_45, 2);
      this.e_45.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("SelectedWorkshopItem.Rating")
      {
        UseGeneratedBindings = true
      });
      this.e_48 = new TextBlock();
      this.e_44.Children.Add((UIElement) this.e_48);
      this.e_48.Name = "e_48";
      this.e_48.Margin = new Thickness(0.0f, 0.0f, 0.0f, 10f);
      this.e_48.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      this.e_48.TextWrapping = TextWrapping.Wrap;
      Grid.SetRow((UIElement) this.e_48, 1);
      Grid.SetColumnSpan((UIElement) this.e_48, 2);
      this.e_48.SetBinding(TextBlock.TextProperty, new Binding("SelectedWorkshopItem.Title")
      {
        UseGeneratedBindings = true
      });
      this.e_49 = new TextBlock();
      this.e_44.Children.Add((UIElement) this.e_49);
      this.e_49.Name = "e_49";
      this.e_49.Margin = new Thickness(0.0f, 0.0f, 0.0f, 2f);
      this.e_49.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      this.e_49.FontFamily = new FontFamily("InventorySmall");
      this.e_49.FontSize = 10f;
      Grid.SetRow((UIElement) this.e_49, 2);
      this.e_49.SetResourceReference(TextBlock.TextProperty, (object) "WorkshopBrowser_FileSize");
      this.e_50 = new TextBlock();
      this.e_44.Children.Add((UIElement) this.e_50);
      this.e_50.Name = "e_50";
      this.e_50.Margin = new Thickness(20f, 0.0f, 0.0f, 2f);
      this.e_50.FontFamily = new FontFamily("InventorySmall");
      this.e_50.FontSize = 10f;
      Grid.SetColumn((UIElement) this.e_50, 1);
      Grid.SetRow((UIElement) this.e_50, 2);
      this.e_50.SetBinding(TextBlock.TextProperty, new Binding("SelectedWorkshopItem.Size")
      {
        UseGeneratedBindings = true,
        StringFormat = "{0:N0} B"
      });
      this.e_51 = new TextBlock();
      this.e_44.Children.Add((UIElement) this.e_51);
      this.e_51.Name = "e_51";
      this.e_51.Margin = new Thickness(0.0f, 0.0f, 0.0f, 2f);
      this.e_51.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      this.e_51.FontFamily = new FontFamily("InventorySmall");
      this.e_51.FontSize = 10f;
      Grid.SetRow((UIElement) this.e_51, 3);
      this.e_51.SetResourceReference(TextBlock.TextProperty, (object) "WorkshopBrowser_Subscribers");
      this.e_52 = new TextBlock();
      this.e_44.Children.Add((UIElement) this.e_52);
      this.e_52.Name = "e_52";
      this.e_52.Margin = new Thickness(20f, 0.0f, 0.0f, 2f);
      this.e_52.FontFamily = new FontFamily("InventorySmall");
      this.e_52.FontSize = 10f;
      Grid.SetColumn((UIElement) this.e_52, 1);
      Grid.SetRow((UIElement) this.e_52, 3);
      this.e_52.SetBinding(TextBlock.TextProperty, new Binding("SelectedWorkshopItem.NumSubscriptions")
      {
        UseGeneratedBindings = true,
        StringFormat = "{0:N0}"
      });
      this.e_53 = new TextBlock();
      this.e_44.Children.Add((UIElement) this.e_53);
      this.e_53.Name = "e_53";
      this.e_53.Margin = new Thickness(0.0f, 0.0f, 0.0f, 2f);
      this.e_53.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      this.e_53.FontFamily = new FontFamily("InventorySmall");
      this.e_53.FontSize = 10f;
      Grid.SetRow((UIElement) this.e_53, 4);
      this.e_53.SetResourceReference(TextBlock.TextProperty, (object) "WorkshopBrowser_Created");
      this.e_54 = new TextBlock();
      this.e_44.Children.Add((UIElement) this.e_54);
      this.e_54.Name = "e_54";
      this.e_54.Margin = new Thickness(20f, 0.0f, 0.0f, 2f);
      this.e_54.FontFamily = new FontFamily("InventorySmall");
      this.e_54.FontSize = 10f;
      Grid.SetColumn((UIElement) this.e_54, 1);
      Grid.SetRow((UIElement) this.e_54, 4);
      this.e_54.SetBinding(TextBlock.TextProperty, new Binding("SelectedWorkshopItem.TimeCreated")
      {
        UseGeneratedBindings = true
      });
      this.e_55 = new TextBlock();
      this.e_44.Children.Add((UIElement) this.e_55);
      this.e_55.Name = "e_55";
      this.e_55.Margin = new Thickness(0.0f, 0.0f, 0.0f, 2f);
      this.e_55.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      this.e_55.FontFamily = new FontFamily("InventorySmall");
      this.e_55.FontSize = 10f;
      Grid.SetRow((UIElement) this.e_55, 5);
      this.e_55.SetResourceReference(TextBlock.TextProperty, (object) "WorkshopBrowser_Updated");
      this.e_56 = new TextBlock();
      this.e_44.Children.Add((UIElement) this.e_56);
      this.e_56.Name = "e_56";
      this.e_56.Margin = new Thickness(20f, 0.0f, 0.0f, 2f);
      this.e_56.FontFamily = new FontFamily("InventorySmall");
      this.e_56.FontSize = 10f;
      Grid.SetColumn((UIElement) this.e_56, 1);
      Grid.SetRow((UIElement) this.e_56, 5);
      this.e_56.SetBinding(TextBlock.TextProperty, new Binding("SelectedWorkshopItem.TimeUpdated")
      {
        UseGeneratedBindings = true
      });
      this.e_57 = new ScrollViewer();
      this.e_43.Children.Add((UIElement) this.e_57);
      this.e_57.Name = "e_57";
      this.e_57.Margin = new Thickness(10f, 10f, 10f, 10f);
      this.e_57.HorizontalAlignment = HorizontalAlignment.Stretch;
      this.e_57.VerticalAlignment = VerticalAlignment.Stretch;
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
      ControlTemplate controlTemplate4 = new ControlTemplate(typeof (ScrollViewer), new Func<UIElement, UIElement>(WorkshopBrowserView.e_57_s_S_2_ctMethod));
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
      this.e_57.Style = style4;
      this.e_57.HorizontalContentAlignment = HorizontalAlignment.Stretch;
      this.e_57.VerticalContentAlignment = VerticalAlignment.Stretch;
      this.e_57.IsTabStop = true;
      this.e_57.TabIndex = 10;
      Grid.SetRow((UIElement) this.e_57, 1);
      GamepadHelp.SetTabIndexLeft((DependencyObject) this.e_57, 5);
      this.e_70 = new Border();
      this.e_57.Content = (object) this.e_70;
      this.e_70.Name = "e_70";
      this.e_70.HorizontalAlignment = HorizontalAlignment.Stretch;
      this.e_70.VerticalAlignment = VerticalAlignment.Stretch;
      this.e_70.Background = (Brush) new SolidColorBrush(new ColorW(33, 44, 53, (int) byte.MaxValue));
      this.e_71 = new TextBlock();
      this.e_70.Child = (UIElement) this.e_71;
      this.e_71.Name = "e_71";
      this.e_71.Margin = new Thickness(5f, 5f, 5f, 5f);
      this.e_71.TextWrapping = TextWrapping.Wrap;
      this.e_71.FontFamily = new FontFamily("InventorySmall");
      this.e_71.FontSize = 10f;
      this.e_71.SetBinding(TextBlock.TextProperty, new Binding("SelectedWorkshopItem.Description")
      {
        UseGeneratedBindings = true
      });
      this.e_72 = new Grid();
      this.e_6.Children.Add((UIElement) this.e_72);
      this.e_72.Name = "e_72";
      this.e_72.ColumnDefinitions.Add(new ColumnDefinition());
      this.e_72.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_72.ColumnDefinitions.Add(new ColumnDefinition());
      this.e_72.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_72.ColumnDefinitions.Add(new ColumnDefinition());
      Grid.SetRow((UIElement) this.e_72, 2);
      this.e_72.SetBinding(UIElement.VisibilityProperty, new Binding("IsPagingInfoVisible")
      {
        UseGeneratedBindings = true
      });
      this.e_73 = new ImageButton();
      this.e_72.Children.Add((UIElement) this.e_73);
      this.e_73.Name = "e_73";
      this.e_73.Width = 16f;
      this.e_73.HorizontalAlignment = HorizontalAlignment.Right;
      this.e_73.TabIndex = 6;
      this.e_73.ImageNormal = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_arrow_left.dds"
      };
      this.e_73.ImageDisabled = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_arrow_left.dds"
      };
      this.e_73.ImageHover = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_arrow_left_highlight.dds"
      };
      Grid.SetColumn((UIElement) this.e_73, 1);
      GamepadHelp.SetTabIndexRight((DependencyObject) this.e_73, 7);
      GamepadHelp.SetTabIndexUp((DependencyObject) this.e_73, 5);
      GamepadHelp.SetTabIndexDown((DependencyObject) this.e_73, 8);
      this.e_73.SetBinding(UIElement.IsEnabledProperty, new Binding("IsQueryFinished")
      {
        UseGeneratedBindings = true
      });
      this.e_73.SetBinding(Button.CommandProperty, new Binding("PreviousPageCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_73.SetResourceReference(UIElement.ToolTipProperty, (object) "WorkshopBrowser_PreviousPage");
      this.e_74 = new StackPanel();
      this.e_72.Children.Add((UIElement) this.e_74);
      this.e_74.Name = "e_74";
      this.e_74.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_74.VerticalAlignment = VerticalAlignment.Center;
      this.e_74.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) this.e_74, 2);
      this.e_75 = new TextBlock();
      this.e_74.Children.Add((UIElement) this.e_75);
      this.e_75.Name = "e_75";
      this.e_75.SetBinding(TextBlock.TextProperty, new Binding("CurrentPage")
      {
        UseGeneratedBindings = true
      });
      this.e_76 = new TextBlock();
      this.e_74.Children.Add((UIElement) this.e_76);
      this.e_76.Name = "e_76";
      this.e_76.Text = "/";
      this.e_77 = new TextBlock();
      this.e_74.Children.Add((UIElement) this.e_77);
      this.e_77.Name = "e_77";
      this.e_77.SetBinding(TextBlock.TextProperty, new Binding("TotalPages")
      {
        UseGeneratedBindings = true
      });
      this.e_78 = new ImageButton();
      this.e_72.Children.Add((UIElement) this.e_78);
      this.e_78.Name = "e_78";
      this.e_78.Width = 16f;
      this.e_78.HorizontalAlignment = HorizontalAlignment.Left;
      this.e_78.TabIndex = 7;
      this.e_78.ImageNormal = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_arrow_right.dds"
      };
      this.e_78.ImageDisabled = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_arrow_right.dds"
      };
      this.e_78.ImageHover = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_arrow_right_highlight.dds"
      };
      Grid.SetColumn((UIElement) this.e_78, 3);
      GamepadHelp.SetTabIndexLeft((DependencyObject) this.e_78, 6);
      GamepadHelp.SetTabIndexUp((DependencyObject) this.e_78, 5);
      GamepadHelp.SetTabIndexDown((DependencyObject) this.e_78, 8);
      this.e_78.SetBinding(UIElement.IsEnabledProperty, new Binding("IsQueryFinished")
      {
        UseGeneratedBindings = true
      });
      this.e_78.SetBinding(Button.CommandProperty, new Binding("NextPageCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_78.SetResourceReference(UIElement.ToolTipProperty, (object) "WorkshopBrowser_NextPage");
      this.e_79 = new Border();
      this.e_6.Children.Add((UIElement) this.e_79);
      this.e_79.Name = "e_79";
      this.e_79.Height = 2f;
      this.e_79.Margin = new Thickness(0.0f, 10f, 0.0f, 10f);
      this.e_79.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      Grid.SetRow((UIElement) this.e_79, 3);
      Grid.SetColumnSpan((UIElement) this.e_79, 2);
      this.e_80 = new Grid();
      this.e_6.Children.Add((UIElement) this.e_80);
      this.e_80.Name = "e_80";
      this.e_80.Margin = new Thickness(0.0f, 0.0f, 0.0f, 30f);
      this.e_80.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_80.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_80.ColumnDefinitions.Add(new ColumnDefinition());
      Grid.SetRow((UIElement) this.e_80, 4);
      Grid.SetColumnSpan((UIElement) this.e_80, 2);
      this.e_81 = new Button();
      this.e_80.Children.Add((UIElement) this.e_81);
      this.e_81.Name = "e_81";
      this.e_81.Margin = new Thickness(0.0f, 10f, 0.0f, 10f);
      this.e_81.VerticalAlignment = VerticalAlignment.Center;
      this.e_81.Padding = new Thickness(10f, 0.0f, 10f, 0.0f);
      this.e_81.TabIndex = 8;
      GamepadHelp.SetTabIndexRight((DependencyObject) this.e_81, 9);
      GamepadHelp.SetTabIndexUp((DependencyObject) this.e_81, 6);
      this.e_81.SetBinding(Button.CommandProperty, new Binding("BrowseWorkshopCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_81.SetResourceReference(ContentControl.ContentProperty, (object) "ScreenLoadSubscribedWorldBrowseWorkshop");
      this.e_82 = new Button();
      this.e_80.Children.Add((UIElement) this.e_82);
      this.e_82.Name = "e_82";
      this.e_82.Margin = new Thickness(10f, 10f, 0.0f, 10f);
      this.e_82.VerticalAlignment = VerticalAlignment.Center;
      this.e_82.Padding = new Thickness(10f, 0.0f, 10f, 0.0f);
      this.e_82.TabIndex = 9;
      Grid.SetColumn((UIElement) this.e_82, 1);
      GamepadHelp.SetTabIndexLeft((DependencyObject) this.e_82, 8);
      GamepadHelp.SetTabIndexRight((DependencyObject) this.e_82, 6);
      GamepadHelp.SetTabIndexUp((DependencyObject) this.e_82, 6);
      this.e_82.SetBinding(Button.CommandProperty, new Binding("OpenItemInWorkshopCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_82.SetResourceReference(ContentControl.ContentProperty, (object) "WorkshopBrowser_OpenItem");
      ImageManager.Instance.AddImage("Textures\\GUI\\Screens\\screen_background.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol_highlight.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Blueprints\\Refresh.png");
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
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "SelectedSortIndex", typeof (MyWorkshopBrowserViewModel_SelectedSortIndex_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "Categories", typeof (MyWorkshopBrowserViewModel_Categories_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "SelectedCategoryIndex", typeof (MyWorkshopBrowserViewModel_SelectedCategoryIndex_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "IsQueryFinished", typeof (MyWorkshopBrowserViewModel_IsQueryFinished_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "RefreshCommand", typeof (MyWorkshopBrowserViewModel_RefreshCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "CategoryControlTabIndexRight", typeof (MyWorkshopBrowserViewModel_CategoryControlTabIndexRight_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "IsRefreshing", typeof (MyWorkshopBrowserViewModel_IsRefreshing_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "IsWorkshopAggregator", typeof (MyWorkshopBrowserViewModel_IsWorkshopAggregator_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "ServiceCommand", typeof (MyWorkshopBrowserViewModel_ServiceCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "Service0IsChecked", typeof (MyWorkshopBrowserViewModel_Service0IsChecked_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "Service1IsChecked", typeof (MyWorkshopBrowserViewModel_Service1IsChecked_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "SearchText", typeof (MyWorkshopBrowserViewModel_SearchText_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "SearchControlTabIndexLeft", typeof (MyWorkshopBrowserViewModel_SearchControlTabIndexLeft_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "IsSearchLabelVisible", typeof (MyWorkshopBrowserViewModel_IsSearchLabelVisible_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "ClearSearchTextCommand", typeof (MyWorkshopBrowserViewModel_ClearSearchTextCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "IsNotFoundTextVisible", typeof (MyWorkshopBrowserViewModel_IsNotFoundTextVisible_PropertyInfo));
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
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "PreviousPageCommand", typeof (MyWorkshopBrowserViewModel_PreviousPageCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "CurrentPage", typeof (MyWorkshopBrowserViewModel_CurrentPage_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "TotalPages", typeof (MyWorkshopBrowserViewModel_TotalPages_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "NextPageCommand", typeof (MyWorkshopBrowserViewModel_NextPageCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "BrowseWorkshopCommand", typeof (MyWorkshopBrowserViewModel_BrowseWorkshopCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyWorkshopBrowserViewModel), "OpenItemInWorkshopCommand", typeof (MyWorkshopBrowserViewModel_OpenItemInWorkshopCommand_PropertyInfo));
    }

    private static void InitializeElementResources(UIElement elem)
    {
      elem.Resources.MergedDictionaries.Add((ResourceDictionary) Styles.Instance);
      Style style = new Style(typeof (ListBoxItem), elem.Resources[(object) typeof (ListBoxItem)] as Style);
      Setter setter = new Setter(UIElement.MarginProperty, (object) new Thickness(0.0f, 4f, 4f, 0.0f));
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
      grid.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid.ColumnDefinitions.Add(new ColumnDefinition());
      CheckBox checkBox = new CheckBox();
      grid.Children.Add((UIElement) checkBox);
      checkBox.Name = "e_15";
      checkBox.VerticalAlignment = VerticalAlignment.Center;
      Binding binding1 = new Binding("IsChecked");
      checkBox.SetBinding(ToggleButton.IsCheckedProperty, binding1);
      TextBlock textBlock = new TextBlock();
      grid.Children.Add((UIElement) textBlock);
      textBlock.Name = "e_16";
      textBlock.Margin = new Thickness(2f, 0.0f, 0.0f, 0.0f);
      textBlock.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock, 1);
      Binding binding2 = new Binding("LocalizedName");
      textBlock.SetBinding(TextBlock.TextProperty, binding2);
      return (UIElement) grid;
    }

    private static UIElement e_19_s_S_0_ctMethod(UIElement parent)
    {
      StackPanel stackPanel = new StackPanel();
      stackPanel.Parent = parent;
      stackPanel.Name = "e_20";
      stackPanel.Orientation = Orientation.Horizontal;
      Grid grid = new Grid();
      stackPanel.Children.Add((UIElement) grid);
      grid.Name = "e_21";
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
      contentPresenter.Name = "e_22";
      contentPresenter.VerticalAlignment = VerticalAlignment.Center;
      contentPresenter.SetBinding(ContentPresenter.ContentProperty, new Binding("Content")
      {
        Source = (object) parent
      });
      return (UIElement) stackPanel;
    }

    private static UIElement e_23_s_S_0_ctMethod(UIElement parent)
    {
      StackPanel stackPanel = new StackPanel();
      stackPanel.Parent = parent;
      stackPanel.Name = "e_24";
      stackPanel.Orientation = Orientation.Horizontal;
      Grid grid = new Grid();
      stackPanel.Children.Add((UIElement) grid);
      grid.Name = "e_25";
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
      contentPresenter.Name = "e_26";
      contentPresenter.VerticalAlignment = VerticalAlignment.Center;
      contentPresenter.SetBinding(ContentPresenter.ContentProperty, new Binding("Content")
      {
        Source = (object) parent
      });
      return (UIElement) stackPanel;
    }

    private static UIElement e_32_s_S_1_ctMethod(UIElement parent)
    {
      Border border = new Border();
      border.Parent = parent;
      border.Name = "e_33";
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
      uniformGrid.Name = "e_34";
      uniformGrid.Margin = new Thickness(5f, 5f, 5f, 5f);
      uniformGrid.IsItemsHost = true;
      uniformGrid.Rows = 3;
      uniformGrid.Columns = 3;
      return (UIElement) border;
    }

    private static UIElement e_39_iptMethod(UIElement parent)
    {
      StackPanel stackPanel = new StackPanel();
      stackPanel.Parent = parent;
      stackPanel.Name = "e_40";
      stackPanel.IsItemsHost = true;
      stackPanel.Orientation = Orientation.Horizontal;
      return (UIElement) stackPanel;
    }

    private static UIElement e_39_dtMethod(UIElement parent)
    {
      Image image = new Image();
      image.Parent = parent;
      image.Name = "e_41";
      image.Height = 16f;
      image.Width = 16f;
      image.Margin = new Thickness(2f, 2f, 2f, 2f);
      Binding binding = new Binding();
      image.SetBinding(Image.SourceProperty, binding);
      return (UIElement) image;
    }

    private static UIElement e_32_dtMethod(UIElement parent)
    {
      Grid grid = new Grid();
      grid.Parent = parent;
      grid.Name = "e_35";
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
      image1.Name = "e_36";
      image1.HorizontalAlignment = HorizontalAlignment.Center;
      image1.Source = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Bg16x9.png"
      };
      image1.Stretch = Stretch.Uniform;
      Image image2 = new Image();
      grid.Children.Add((UIElement) image2);
      image2.Name = "e_37";
      image2.HorizontalAlignment = HorizontalAlignment.Center;
      image2.Stretch = Stretch.Uniform;
      Binding binding1 = new Binding("PreviewImage");
      image2.SetBinding(Image.SourceProperty, binding1);
      CheckBox checkBox = new CheckBox();
      grid.Children.Add((UIElement) checkBox);
      checkBox.Name = "e_38";
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
      itemsControl.Name = "e_39";
      itemsControl.Margin = new Thickness(4f, 2f, 2f, 2f);
      itemsControl.HorizontalAlignment = HorizontalAlignment.Left;
      itemsControl.VerticalAlignment = VerticalAlignment.Center;
      ControlTemplate controlTemplate = new ControlTemplate(new Func<UIElement, UIElement>(WorkshopBrowserView.e_39_iptMethod));
      itemsControl.ItemsPanel = controlTemplate;
      Func<UIElement, UIElement> createMethod = new Func<UIElement, UIElement>(WorkshopBrowserView.e_39_dtMethod);
      itemsControl.ItemTemplate = new DataTemplate(createMethod);
      Grid.SetRow((UIElement) itemsControl, 1);
      Binding binding3 = new Binding("Rating");
      itemsControl.SetBinding(ItemsControl.ItemsSourceProperty, binding3);
      TextBlock textBlock = new TextBlock();
      grid.Children.Add((UIElement) textBlock);
      textBlock.Name = "e_42";
      textBlock.Margin = new Thickness(4f, 0.0f, 2f, 2f);
      Grid.SetRow((UIElement) textBlock, 2);
      Binding binding4 = new Binding("Title");
      textBlock.SetBinding(TextBlock.TextProperty, binding4);
      return (UIElement) grid;
    }

    private static UIElement e_45_iptMethod(UIElement parent)
    {
      StackPanel stackPanel = new StackPanel();
      stackPanel.Parent = parent;
      stackPanel.Name = "e_46";
      stackPanel.IsItemsHost = true;
      stackPanel.Orientation = Orientation.Horizontal;
      return (UIElement) stackPanel;
    }

    private static UIElement e_45_dtMethod(UIElement parent)
    {
      Image image = new Image();
      image.Parent = parent;
      image.Name = "e_47";
      image.Height = 24f;
      image.Width = 24f;
      image.Margin = new Thickness(2f, 2f, 2f, 2f);
      Binding binding = new Binding();
      image.SetBinding(Image.SourceProperty, binding);
      return (UIElement) image;
    }

    private static UIElement e_57_s_S_2_ctMethod(UIElement parent)
    {
      Border border1 = new Border();
      border1.Parent = parent;
      border1.Name = "e_58";
      border1.BorderThickness = new Thickness(2f, 2f, 2f, 2f);
      border1.SetBinding(Control.BorderBrushProperty, new Binding("BorderBrush")
      {
        Source = (object) parent
      });
      Grid grid1 = new Grid();
      border1.Child = (UIElement) grid1;
      grid1.Name = "e_59";
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
      grid2.Name = "e_60";
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
      border2.Name = "e_61";
      border2.IsHitTestVisible = false;
      border2.SnapsToDevicePixels = false;
      border2.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListLeftTop");
      Border border3 = new Border();
      grid2.Children.Add((UIElement) border3);
      border3.Name = "e_62";
      border3.IsHitTestVisible = false;
      border3.SnapsToDevicePixels = false;
      Grid.SetColumn((UIElement) border3, 1);
      border3.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListCenterTop");
      Border border4 = new Border();
      grid2.Children.Add((UIElement) border4);
      border4.Name = "e_63";
      border4.IsHitTestVisible = false;
      border4.SnapsToDevicePixels = false;
      Grid.SetColumn((UIElement) border4, 2);
      border4.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListRightTop");
      Border border5 = new Border();
      grid2.Children.Add((UIElement) border5);
      border5.Name = "e_64";
      border5.IsHitTestVisible = false;
      border5.SnapsToDevicePixels = false;
      Grid.SetRow((UIElement) border5, 1);
      border5.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListLeftCenter");
      Border border6 = new Border();
      grid2.Children.Add((UIElement) border6);
      border6.Name = "e_65";
      border6.IsHitTestVisible = false;
      border6.SnapsToDevicePixels = false;
      Grid.SetColumn((UIElement) border6, 1);
      Grid.SetRow((UIElement) border6, 1);
      border6.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListCenter");
      Border border7 = new Border();
      grid2.Children.Add((UIElement) border7);
      border7.Name = "e_66";
      border7.IsHitTestVisible = false;
      border7.SnapsToDevicePixels = false;
      Grid.SetColumn((UIElement) border7, 2);
      Grid.SetRow((UIElement) border7, 1);
      border7.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListRightCenter");
      Border border8 = new Border();
      grid2.Children.Add((UIElement) border8);
      border8.Name = "e_67";
      border8.IsHitTestVisible = false;
      border8.SnapsToDevicePixels = false;
      Grid.SetRow((UIElement) border8, 2);
      border8.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListLeftBottom");
      Border border9 = new Border();
      grid2.Children.Add((UIElement) border9);
      border9.Name = "e_68";
      border9.IsHitTestVisible = false;
      border9.SnapsToDevicePixels = false;
      Grid.SetColumn((UIElement) border9, 1);
      Grid.SetRow((UIElement) border9, 2);
      border9.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListCenterBottom");
      Border border10 = new Border();
      grid2.Children.Add((UIElement) border10);
      border10.Name = "e_69";
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
