// Decompiled with JetBrains decompiler
// Type: EmptyKeys.UserInterface.Generated.MainMenu
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Controls.Primitives;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Animation;
using EmptyKeys.UserInterface.Themes;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.ObjectModel;

namespace EmptyKeys.UserInterface.Generated
{
  [GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
  public class MainMenu : UIRoot
  {
    private Grid rootGrid;
    private Grid main;
    private StackPanel e_0;
    private Border brushTest;
    private GroupBox e_1;
    private StackPanel e_2;
    private CheckBox e_3;
    private TextBox e_4;
    private Slider e_5;
    private TabControl e_6;
    private DataGrid e_10;
    private ComboBox e_11;
    private ProgressBar progressBar;
    private ScrollViewer e_18;
    private TextBlock e_19;

    public MainMenu() => this.Initialize();

    public MainMenu(int width, int height)
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
      MainMenu.InitializeElementResources((UIElement) this);
      this.rootGrid = new Grid();
      this.Content = (object) this.rootGrid;
      this.rootGrid.Name = "rootGrid";
      this.rootGrid.Margin = new Thickness(35f, 35f, 35f, 35f);
      this.rootGrid.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      this.rootGrid.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(5.6f, GridUnitType.Star)
      });
      this.main = new Grid();
      this.rootGrid.Children.Add((UIElement) this.main);
      this.main.Name = "main";
      this.main.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(0.1f, GridUnitType.Star)
      });
      this.main.RowDefinitions.Add(new RowDefinition());
      this.main.ColumnDefinitions.Add(new ColumnDefinition());
      this.main.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(0.15f, GridUnitType.Star)
      });
      Grid.SetColumn((UIElement) this.main, 1);
      this.e_0 = new StackPanel();
      this.main.Children.Add((UIElement) this.e_0);
      this.e_0.Name = "e_0";
      this.e_0.Width = 500f;
      this.e_0.Margin = new Thickness(0.0f, 100f, 0.0f, 0.0f);
      Grid.SetRow((UIElement) this.e_0, 1);
      this.brushTest = new Border();
      this.e_0.Children.Add((UIElement) this.brushTest);
      this.brushTest.Name = "brushTest";
      this.brushTest.Height = 50f;
      this.brushTest.Width = 100f;
      EventTrigger eventTrigger1 = new EventTrigger(UIElement.LoadedEvent, (UIElement) this.brushTest);
      this.brushTest.Triggers.Add((TriggerBase) eventTrigger1);
      BeginStoryboard beginStoryboard1 = new BeginStoryboard();
      beginStoryboard1.Name = "brushTest_ET_0_AC_0";
      eventTrigger1.AddAction((TriggerAction) beginStoryboard1);
      Storyboard storyboard1 = new Storyboard();
      beginStoryboard1.Storyboard = storyboard1;
      storyboard1.Name = "brushTest_ET_0_AC_0_SB";
      SolidColorBrushAnimation colorBrushAnimation = new SolidColorBrushAnimation();
      colorBrushAnimation.Name = "brushTest_ET_0_AC_0_SB_TL_0";
      colorBrushAnimation.AutoReverse = true;
      colorBrushAnimation.Duration = new Duration(new TimeSpan(0, 0, 0, 5, 0));
      colorBrushAnimation.RepeatBehavior = RepeatBehavior.Forever;
      colorBrushAnimation.From = new ColorW?(new ColorW((int) byte.MaxValue, 0, 0, (int) byte.MaxValue));
      colorBrushAnimation.To = new ColorW?(new ColorW(0, 0, (int) byte.MaxValue, (int) byte.MaxValue));
      SineEase sineEase1 = new SineEase();
      colorBrushAnimation.EasingFunction = (EasingFunctionBase) sineEase1;
      Storyboard.SetTargetName((DependencyObject) colorBrushAnimation, "brushTest");
      Storyboard.SetTargetProperty((DependencyObject) colorBrushAnimation, Control.BackgroundProperty);
      storyboard1.Children.Add((Timeline) colorBrushAnimation);
      this.brushTest.Background = (Brush) new SolidColorBrush(new ColorW(0, 128, 0, (int) byte.MaxValue));
      this.brushTest.BorderBrush = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, 0, 0, (int) byte.MaxValue));
      this.brushTest.BorderThickness = new Thickness(3f, 3f, 3f, 3f);
      this.e_1 = new GroupBox();
      this.e_0.Children.Add((UIElement) this.e_1);
      this.e_1.Name = "e_1";
      this.e_1.Header = (object) "Group";
      this.e_2 = new StackPanel();
      this.e_1.Content = (object) this.e_2;
      this.e_2.Name = "e_2";
      this.e_3 = new CheckBox();
      this.e_2.Children.Add((UIElement) this.e_3);
      this.e_3.Name = "e_3";
      this.e_3.Content = (object) "Test";
      this.e_4 = new TextBox();
      this.e_2.Children.Add((UIElement) this.e_4);
      this.e_4.Name = "e_4";
      this.e_5 = new Slider();
      this.e_2.Children.Add((UIElement) this.e_5);
      this.e_5.Name = "e_5";
      this.e_5.Minimum = 0.0f;
      this.e_5.Maximum = 100f;
      this.e_5.Value = 50f;
      this.e_6 = new TabControl();
      this.e_2.Children.Add((UIElement) this.e_6);
      this.e_6.Name = "e_6";
      this.e_6.ItemsSource = (IEnumerable) MainMenu.Get_e_6_Items();
      this.e_10 = new DataGrid();
      this.e_2.Children.Add((UIElement) this.e_10);
      this.e_10.Name = "e_10";
      this.e_10.Height = 150f;
      this.e_10.AutoGenerateColumns = true;
      Binding binding = new Binding("GridData");
      this.e_10.SetBinding(ItemsControl.ItemsSourceProperty, binding);
      this.e_11 = new ComboBox();
      this.e_2.Children.Add((UIElement) this.e_11);
      this.e_11.Name = "e_11";
      this.e_11.ItemsSource = (IEnumerable) MainMenu.Get_e_11_Items();
      this.progressBar = new ProgressBar();
      this.e_2.Children.Add((UIElement) this.progressBar);
      this.progressBar.Name = "progressBar";
      this.progressBar.Height = 30f;
      EventTrigger eventTrigger2 = new EventTrigger(UIElement.LoadedEvent, (UIElement) this.progressBar);
      this.progressBar.Triggers.Add((TriggerBase) eventTrigger2);
      BeginStoryboard beginStoryboard2 = new BeginStoryboard();
      beginStoryboard2.Name = "progressBar_ET_0_AC_0";
      eventTrigger2.AddAction((TriggerAction) beginStoryboard2);
      Storyboard storyboard2 = new Storyboard();
      beginStoryboard2.Storyboard = storyboard2;
      storyboard2.Name = "progressBar_ET_0_AC_0_SB";
      FloatAnimation floatAnimation = new FloatAnimation();
      floatAnimation.Name = "progressBar_ET_0_AC_0_SB_TL_0";
      floatAnimation.AutoReverse = true;
      floatAnimation.Duration = new Duration(new TimeSpan(0, 0, 0, 5, 0));
      floatAnimation.RepeatBehavior = RepeatBehavior.Forever;
      floatAnimation.From = new float?(0.0f);
      floatAnimation.To = new float?(100f);
      SineEase sineEase2 = new SineEase();
      floatAnimation.EasingFunction = (EasingFunctionBase) sineEase2;
      Storyboard.SetTargetName((DependencyObject) floatAnimation, "progressBar");
      Storyboard.SetTargetProperty((DependencyObject) floatAnimation, RangeBase.ValueProperty);
      storyboard2.Children.Add((Timeline) floatAnimation);
      this.progressBar.Minimum = 0.0f;
      this.progressBar.Maximum = 100f;
      this.progressBar.Value = 50f;
      this.e_18 = new ScrollViewer();
      this.e_0.Children.Add((UIElement) this.e_18);
      this.e_18.Name = "e_18";
      this.e_18.Height = 100f;
      this.e_18.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
      this.e_18.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
      this.e_19 = new TextBlock();
      this.e_18.Content = (object) this.e_19;
      this.e_19.Name = "e_19";
      this.e_19.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
    }

    private static void InitializeElementResources(UIElement elem) => elem.Resources.MergedDictionaries.Add((ResourceDictionary) Styles.Instance);

    private static ObservableCollection<object> Get_e_6_Items()
    {
      ObservableCollection<object> observableCollection = new ObservableCollection<object>();
      TabItem tabItem1 = new TabItem();
      tabItem1.Name = "e_7";
      tabItem1.Content = (object) "TabItem1";
      tabItem1.Header = (object) "Tab Item 1";
      observableCollection.Add((object) tabItem1);
      TabItem tabItem2 = new TabItem();
      tabItem2.Name = "e_8";
      tabItem2.Content = (object) "TabItem2";
      tabItem2.Header = (object) "Tab Item 2";
      observableCollection.Add((object) tabItem2);
      TabItem tabItem3 = new TabItem();
      tabItem3.Name = "e_9";
      tabItem3.Content = (object) "TabItem3";
      tabItem3.Header = (object) "Tab Item 3";
      observableCollection.Add((object) tabItem3);
      return observableCollection;
    }

    private static ObservableCollection<object> Get_e_11_Items()
    {
      ObservableCollection<object> observableCollection = new ObservableCollection<object>();
      ComboBoxItem comboBoxItem1 = new ComboBoxItem();
      comboBoxItem1.Name = "e_12";
      comboBoxItem1.Content = (object) "Test1";
      observableCollection.Add((object) comboBoxItem1);
      ComboBoxItem comboBoxItem2 = new ComboBoxItem();
      comboBoxItem2.Name = "e_13";
      comboBoxItem2.Content = (object) "Test2";
      observableCollection.Add((object) comboBoxItem2);
      ComboBoxItem comboBoxItem3 = new ComboBoxItem();
      comboBoxItem3.Name = "e_14";
      comboBoxItem3.Content = (object) "Test3";
      observableCollection.Add((object) comboBoxItem3);
      ComboBoxItem comboBoxItem4 = new ComboBoxItem();
      comboBoxItem4.Name = "e_15";
      comboBoxItem4.Content = (object) "Test4";
      observableCollection.Add((object) comboBoxItem4);
      ComboBoxItem comboBoxItem5 = new ComboBoxItem();
      comboBoxItem5.Name = "e_16";
      comboBoxItem5.Content = (object) "Test5";
      observableCollection.Add((object) comboBoxItem5);
      ComboBoxItem comboBoxItem6 = new ComboBoxItem();
      comboBoxItem6.Name = "e_17";
      comboBoxItem6.Content = (object) "Test6";
      observableCollection.Add((object) comboBoxItem6);
      return observableCollection;
    }
  }
}
