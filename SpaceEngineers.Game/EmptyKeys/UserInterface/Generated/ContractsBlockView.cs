// Decompiled with JetBrains decompiler
// Type: EmptyKeys.UserInterface.Generated.ContractsBlockView
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Controls.Primitives;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.ContractsBlockView_Bindings;
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
  public class ContractsBlockView : UIRoot
  {
    private Grid rootGrid;
    private Image e_0;
    private Grid e_1;
    private ImageButton e_2;
    private StackPanel e_3;
    private TextBlock e_4;
    private Border e_5;
    private TabControl e_6;

    public ContractsBlockView() => this.Initialize();

    public ContractsBlockView(int width, int height)
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
      ContractsBlockView.InitializeElementResources((UIElement) this);
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
      this.e_6 = new TabControl();
      this.e_1.Children.Add((UIElement) this.e_6);
      this.e_6.Name = "e_6";
      this.e_6.TabIndex = 0;
      this.e_6.ItemsSource = (IEnumerable) ContractsBlockView.Get_e_6_Items();
      Grid.SetColumn((UIElement) this.e_6, 1);
      Grid.SetRow((UIElement) this.e_6, 3);
      ImageManager.Instance.AddImage("Textures\\GUI\\Screens\\screen_background.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol_highlight.dds");
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsBlockViewModel), "MaxWidth", typeof (MyContractsBlockViewModel_MaxWidth_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsBlockViewModel), "BackgroundOverlay", typeof (MyContractsBlockViewModel_BackgroundOverlay_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsBlockViewModel), "ExitCommand", typeof (MyContractsBlockViewModel_ExitCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsBlockViewModel), "FilterTargets", typeof (MyContractsBlockViewModel_FilterTargets_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsBlockViewModel), "FilterTargetIndex", typeof (MyContractsBlockViewModel_FilterTargetIndex_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsBlockViewModel), "AvailableContracts", typeof (MyContractsBlockViewModel_AvailableContracts_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsBlockViewModel), "SelectedAvailableContractIndex", typeof (MyContractsBlockViewModel_SelectedAvailableContractIndex_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsBlockViewModel), "SelectedAvailableContract", typeof (MyContractsBlockViewModel_SelectedAvailableContract_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsBlockViewModel), "IsNoAvailableContractVisible", typeof (MyContractsBlockViewModel_IsNoAvailableContractVisible_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsBlockViewModel), "RefreshAvailableCommand", typeof (MyContractsBlockViewModel_RefreshAvailableCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsBlockViewModel), "IsAcceptEnabled", typeof (MyContractsBlockViewModel_IsAcceptEnabled_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsBlockViewModel), "AcceptCommand", typeof (MyContractsBlockViewModel_AcceptCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsBlockViewModel), "ActiveViewModel", typeof (MyContractsBlockViewModel_ActiveViewModel_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsBlockViewModel), "IsAdministrationVisible", typeof (MyContractsBlockViewModel_IsAdministrationVisible_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsBlockViewModel), "AdministrationViewModel", typeof (MyContractsBlockViewModel_AdministrationViewModel_PropertyInfo));
    }

    private static void InitializeElementResources(UIElement elem)
    {
      elem.Resources.MergedDictionaries.Add((ResourceDictionary) Styles.Instance);
      elem.Resources.MergedDictionaries.Add((ResourceDictionary) DataTemplatesContracts.Instance);
    }

    private static ObservableCollection<object> Get_e_6_Items()
    {
      ObservableCollection<object> observableCollection = new ObservableCollection<object>();
      TabItem tabItem1 = new TabItem();
      tabItem1.Name = "e_7";
      tabItem1.SetResourceReference(HeaderedContentControl.HeaderProperty, (object) "ContractScreen_Tab_AvailableContracts");
      Grid grid1 = new Grid();
      tabItem1.Content = (object) grid1;
      grid1.Name = "e_8";
      RowDefinition rowDefinition1 = new RowDefinition();
      grid1.RowDefinitions.Add(rowDefinition1);
      grid1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
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
      grid2.Name = "e_9";
      grid2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      RowDefinition rowDefinition2 = new RowDefinition();
      grid2.RowDefinitions.Add(rowDefinition2);
      Grid grid3 = new Grid();
      grid2.Children.Add((UIElement) grid3);
      grid3.Name = "e_10";
      grid3.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition1 = new ColumnDefinition();
      grid3.ColumnDefinitions.Add(columnDefinition1);
      TextBlock textBlock1 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_11";
      textBlock1.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock1, 0);
      textBlock1.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_ContractFilterTitle");
      ComboBox comboBox1 = new ComboBox();
      grid3.Children.Add((UIElement) comboBox1);
      comboBox1.Name = "e_12";
      comboBox1.Margin = new Thickness(5f, 10f, 5f, 10f);
      comboBox1.VerticalAlignment = VerticalAlignment.Center;
      comboBox1.TabIndex = 1;
      comboBox1.MaxDropDownHeight = 240f;
      Grid.SetColumn((UIElement) comboBox1, 1);
      comboBox1.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("FilterTargets")
      {
        UseGeneratedBindings = true
      });
      comboBox1.SetBinding(Selector.SelectedIndexProperty, new Binding("FilterTargetIndex")
      {
        UseGeneratedBindings = true
      });
      Grid grid4 = new Grid();
      grid2.Children.Add((UIElement) grid4);
      grid4.Name = "e_13";
      Grid.SetRow((UIElement) grid4, 1);
      ListBox listBox1 = new ListBox();
      grid4.Children.Add((UIElement) listBox1);
      listBox1.Name = "e_14";
      listBox1.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      listBox1.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      listBox1.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      listBox1.TabIndex = 2;
      listBox1.SelectionMode = SelectionMode.Single;
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
      ContractsBlockView.InitializeElemente_14Resources((UIElement) listBox1);
      Grid grid5 = new Grid();
      grid4.Children.Add((UIElement) grid5);
      grid5.Name = "e_15";
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
      textBlock2.Name = "e_16";
      textBlock2.Margin = new Thickness(5f, 5f, 5f, 5f);
      textBlock2.HorizontalAlignment = HorizontalAlignment.Center;
      Grid.SetRow((UIElement) textBlock2, 1);
      textBlock2.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_NoAvailableContracts");
      Border border1 = new Border();
      grid1.Children.Add((UIElement) border1);
      border1.Name = "e_17";
      border1.VerticalAlignment = VerticalAlignment.Stretch;
      border1.Background = (Brush) new SolidColorBrush(new ColorW(33, 44, 53, (int) byte.MaxValue));
      border1.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      border1.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      Grid.SetColumn((UIElement) border1, 1);
      ContentPresenter contentPresenter1 = new ContentPresenter();
      border1.Child = (UIElement) contentPresenter1;
      contentPresenter1.Name = "e_18";
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
      border2.Name = "e_19";
      border2.Height = 2f;
      border2.Margin = new Thickness(0.0f, 10f, 0.0f, 10f);
      border2.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      Grid.SetRow((UIElement) border2, 1);
      Grid.SetColumnSpan((UIElement) border2, 2);
      Grid grid6 = new Grid();
      grid1.Children.Add((UIElement) grid6);
      grid6.Name = "e_20";
      grid6.Margin = new Thickness(0.0f, 0.0f, 0.0f, 30f);
      ColumnDefinition columnDefinition2 = new ColumnDefinition();
      grid6.ColumnDefinitions.Add(columnDefinition2);
      grid6.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(150f, GridUnitType.Pixel)
      });
      grid6.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(150f, GridUnitType.Pixel)
      });
      Grid.SetRow((UIElement) grid6, 2);
      Grid.SetColumnSpan((UIElement) grid6, 2);
      Button button1 = new Button();
      grid6.Children.Add((UIElement) button1);
      button1.Name = "e_21";
      button1.Margin = new Thickness(10f, 10f, 0.0f, 10f);
      button1.VerticalAlignment = VerticalAlignment.Center;
      button1.TabIndex = 3;
      Grid.SetColumn((UIElement) button1, 1);
      button1.SetBinding(Button.CommandProperty, new Binding("RefreshAvailableCommand")
      {
        UseGeneratedBindings = true
      });
      button1.SetResourceReference(ContentControl.ContentProperty, (object) "ContractScreen_Button_Refresh");
      Button button2 = new Button();
      grid6.Children.Add((UIElement) button2);
      button2.Name = "e_22";
      button2.Margin = new Thickness(10f, 10f, 0.0f, 10f);
      button2.VerticalAlignment = VerticalAlignment.Center;
      button2.TabIndex = 4;
      Grid.SetColumn((UIElement) button2, 2);
      button2.SetBinding(UIElement.IsEnabledProperty, new Binding("IsAcceptEnabled")
      {
        UseGeneratedBindings = true
      });
      button2.SetBinding(Button.CommandProperty, new Binding("AcceptCommand")
      {
        UseGeneratedBindings = true
      });
      button2.SetResourceReference(ContentControl.ContentProperty, (object) "ContractScreen_Button_Accept");
      observableCollection.Add((object) tabItem1);
      TabItem tabItem2 = new TabItem();
      tabItem2.Name = "e_23";
      tabItem2.SetResourceReference(HeaderedContentControl.HeaderProperty, (object) "ContractScreen_Tab_AcceptedContracts");
      Grid grid7 = new Grid();
      tabItem2.Content = (object) grid7;
      grid7.Name = "e_24";
      grid7.SetBinding(UIElement.DataContextProperty, new Binding("ActiveViewModel")
      {
        UseGeneratedBindings = true
      });
      Grid grid8 = new Grid();
      grid7.Children.Add((UIElement) grid8);
      grid8.Name = "e_25";
      Grid grid9 = new Grid();
      grid8.Children.Add((UIElement) grid9);
      grid9.Name = "e_26";
      RowDefinition rowDefinition3 = new RowDefinition();
      grid9.RowDefinitions.Add(rowDefinition3);
      grid9.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid9.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid9.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(2f, GridUnitType.Star)
      });
      grid9.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(3f, GridUnitType.Star)
      });
      ListBox listBox2 = new ListBox();
      grid9.Children.Add((UIElement) listBox2);
      listBox2.Name = "e_27";
      listBox2.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      listBox2.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      listBox2.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      listBox2.TabIndex = 50;
      listBox2.SelectionMode = SelectionMode.Single;
      Binding binding1 = new Binding("ActiveContracts");
      listBox2.SetBinding(ItemsControl.ItemsSourceProperty, binding1);
      Binding binding2 = new Binding("SelectedActiveContractIndex");
      listBox2.SetBinding(Selector.SelectedIndexProperty, binding2);
      Binding binding3 = new Binding("SelectedActiveContract");
      listBox2.SetBinding(Selector.SelectedItemProperty, binding3);
      ContractsBlockView.InitializeElemente_27Resources((UIElement) listBox2);
      Grid grid10 = new Grid();
      grid9.Children.Add((UIElement) grid10);
      grid10.Name = "e_28";
      grid10.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(30f, GridUnitType.Pixel)
      });
      grid10.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid10.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Star)
      });
      Grid.SetColumn((UIElement) grid10, 0);
      Grid.SetRow((UIElement) grid10, 0);
      Binding binding4 = new Binding("IsNoActiveContractVisible");
      grid10.SetBinding(UIElement.VisibilityProperty, binding4);
      TextBlock textBlock3 = new TextBlock();
      grid10.Children.Add((UIElement) textBlock3);
      textBlock3.Name = "e_29";
      textBlock3.Margin = new Thickness(5f, 5f, 5f, 5f);
      textBlock3.HorizontalAlignment = HorizontalAlignment.Center;
      Grid.SetRow((UIElement) textBlock3, 1);
      textBlock3.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_NoActiveContracts");
      Border border3 = new Border();
      grid9.Children.Add((UIElement) border3);
      border3.Name = "e_30";
      border3.VerticalAlignment = VerticalAlignment.Stretch;
      border3.Background = (Brush) new SolidColorBrush(new ColorW(33, 44, 53, (int) byte.MaxValue));
      border3.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      border3.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      Grid.SetColumn((UIElement) border3, 1);
      ContentPresenter contentPresenter2 = new ContentPresenter();
      border3.Child = (UIElement) contentPresenter2;
      contentPresenter2.Name = "e_31";
      contentPresenter2.Margin = new Thickness(5f, 0.0f, 0.0f, 0.0f);
      Grid.SetColumn((UIElement) contentPresenter2, 1);
      Binding binding5 = new Binding("SelectedActiveContract");
      contentPresenter2.SetBinding(UIElement.DataContextProperty, binding5);
      Binding binding6 = new Binding("SelectedActiveContract");
      contentPresenter2.SetBinding(ContentPresenter.ContentProperty, binding6);
      Border border4 = new Border();
      grid9.Children.Add((UIElement) border4);
      border4.Name = "e_32";
      border4.Height = 2f;
      border4.Margin = new Thickness(0.0f, 10f, 0.0f, 10f);
      border4.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      Grid.SetRow((UIElement) border4, 1);
      Grid.SetColumnSpan((UIElement) border4, 2);
      Grid grid11 = new Grid();
      grid9.Children.Add((UIElement) grid11);
      grid11.Name = "e_33";
      grid11.Margin = new Thickness(0.0f, 0.0f, 0.0f, 30f);
      ColumnDefinition columnDefinition3 = new ColumnDefinition();
      grid11.ColumnDefinitions.Add(columnDefinition3);
      grid11.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(150f, GridUnitType.Pixel)
      });
      grid11.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(150f, GridUnitType.Pixel)
      });
      grid11.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(150f, GridUnitType.Pixel)
      });
      Grid.SetRow((UIElement) grid11, 2);
      Grid.SetColumnSpan((UIElement) grid11, 2);
      Button button3 = new Button();
      grid11.Children.Add((UIElement) button3);
      button3.Name = "e_34";
      button3.Margin = new Thickness(10f, 10f, 0.0f, 10f);
      button3.VerticalAlignment = VerticalAlignment.Center;
      button3.TabIndex = 51;
      Grid.SetColumn((UIElement) button3, 1);
      Binding binding7 = new Binding("RefreshActiveCommand");
      button3.SetBinding(Button.CommandProperty, binding7);
      button3.SetResourceReference(ContentControl.ContentProperty, (object) "ContractScreen_Button_Refresh");
      Button button4 = new Button();
      grid11.Children.Add((UIElement) button4);
      button4.Name = "e_35";
      button4.Margin = new Thickness(10f, 10f, 0.0f, 10f);
      button4.VerticalAlignment = VerticalAlignment.Center;
      button4.TabIndex = 52;
      Grid.SetColumn((UIElement) button4, 2);
      Binding binding8 = new Binding("IsAbandonEnabled");
      button4.SetBinding(UIElement.IsEnabledProperty, binding8);
      Binding binding9 = new Binding("AbandonCommand");
      button4.SetBinding(Button.CommandProperty, binding9);
      button4.SetResourceReference(ContentControl.ContentProperty, (object) "ContractScreen_Button_Abandon");
      Button button5 = new Button();
      grid11.Children.Add((UIElement) button5);
      button5.Name = "e_36";
      button5.Margin = new Thickness(10f, 10f, 0.0f, 10f);
      button5.VerticalAlignment = VerticalAlignment.Center;
      button5.TabIndex = 53;
      Grid.SetColumn((UIElement) button5, 3);
      Binding binding10 = new Binding("FinishTooltipText");
      button5.SetBinding(UIElement.ToolTipProperty, binding10);
      Binding binding11 = new Binding("IsFinishEnabled");
      button5.SetBinding(UIElement.IsEnabledProperty, binding11);
      Binding binding12 = new Binding("FinishCommand");
      button5.SetBinding(Button.CommandProperty, binding12);
      button5.SetResourceReference(ContentControl.ContentProperty, (object) "ContractScreen_Button_Finish");
      Grid grid12 = new Grid();
      grid8.Children.Add((UIElement) grid12);
      grid12.Name = "e_37";
      KeyBinding keyBinding1 = new KeyBinding();
      keyBinding1.Gesture = (InputGesture) new KeyGesture(KeyCode.Escape, ModifierKeys.None, "");
      Binding binding13 = new Binding("ExitGridSelectionCommand");
      keyBinding1.SetBinding(InputBinding.CommandProperty, binding13);
      grid12.InputBindings.Add((InputBinding) keyBinding1);
      keyBinding1.Parent = (UIElement) grid12;
      grid12.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(0.5f, GridUnitType.Star)
      });
      grid12.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(2f, GridUnitType.Star)
      });
      grid12.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Star)
      });
      grid12.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      grid12.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(3f, GridUnitType.Star)
      });
      grid12.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      Grid.SetColumn((UIElement) grid12, 1);
      Grid.SetRow((UIElement) grid12, 3);
      Binding binding14 = new Binding("IsVisibleGridSelection");
      grid12.SetBinding(UIElement.VisibilityProperty, binding14);
      Image image1 = new Image();
      grid12.Children.Add((UIElement) image1);
      image1.Name = "e_38";
      image1.Source = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Screens\\screen_background.dds"
      };
      image1.Stretch = Stretch.Fill;
      Grid.SetColumn((UIElement) image1, 1);
      Grid.SetRow((UIElement) image1, 1);
      ImageBrush.SetColorOverlay((DependencyObject) image1, new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid grid13 = new Grid();
      grid12.Children.Add((UIElement) grid13);
      grid13.Name = "e_39";
      grid13.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid13.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid13.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid13.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(2.5f, GridUnitType.Star)
      });
      grid13.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid13.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid13.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Star)
      });
      grid13.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      grid13.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(12f, GridUnitType.Star)
      });
      grid13.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      Grid.SetColumn((UIElement) grid13, 1);
      Grid.SetRow((UIElement) grid13, 1);
      TextBlock textBlock4 = new TextBlock();
      grid13.Children.Add((UIElement) textBlock4);
      textBlock4.Name = "e_40";
      textBlock4.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock4.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock4.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock4, 1);
      Grid.SetRow((UIElement) textBlock4, 1);
      textBlock4.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_GridSelection_Caption");
      Border border5 = new Border();
      grid13.Children.Add((UIElement) border5);
      border5.Name = "e_41";
      border5.Height = 2f;
      border5.Margin = new Thickness(10f, 10f, 10f, 10f);
      border5.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) border5, 1);
      Grid.SetRow((UIElement) border5, 2);
      TextBlock textBlock5 = new TextBlock();
      grid13.Children.Add((UIElement) textBlock5);
      textBlock5.Name = "e_42";
      textBlock5.Margin = new Thickness(10f, 10f, 10f, 10f);
      textBlock5.HorizontalAlignment = HorizontalAlignment.Left;
      textBlock5.VerticalAlignment = VerticalAlignment.Center;
      textBlock5.TextWrapping = TextWrapping.Wrap;
      Grid.SetColumn((UIElement) textBlock5, 1);
      Grid.SetRow((UIElement) textBlock5, 3);
      textBlock5.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_GridSelection_Text");
      ComboBox comboBox2 = new ComboBox();
      grid13.Children.Add((UIElement) comboBox2);
      comboBox2.Name = "e_43";
      comboBox2.Width = float.NaN;
      comboBox2.Margin = new Thickness(10f, 10f, 10f, 10f);
      comboBox2.HorizontalAlignment = HorizontalAlignment.Stretch;
      Grid.SetColumn((UIElement) comboBox2, 1);
      Grid.SetRow((UIElement) comboBox2, 4);
      Binding binding15 = new Binding("SelectableTargets");
      comboBox2.SetBinding(ItemsControl.ItemsSourceProperty, binding15);
      Binding binding16 = new Binding("SelectedTargetIndex");
      comboBox2.SetBinding(Selector.SelectedIndexProperty, binding16);
      Button button6 = new Button();
      grid13.Children.Add((UIElement) button6);
      button6.Name = "e_44";
      button6.Width = 150f;
      button6.Margin = new Thickness(10f, 10f, 10f, 10f);
      button6.HorizontalAlignment = HorizontalAlignment.Right;
      button6.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) button6, 1);
      Grid.SetRow((UIElement) button6, 5);
      Binding binding17 = new Binding("ConfirmGridSelectionCommand");
      button6.SetBinding(Button.CommandProperty, binding17);
      button6.SetResourceReference(ContentControl.ContentProperty, (object) "ContractScreen_Button_Confirm");
      ImageButton imageButton1 = new ImageButton();
      grid13.Children.Add((UIElement) imageButton1);
      imageButton1.Name = "e_45";
      imageButton1.Height = 24f;
      imageButton1.Width = 24f;
      imageButton1.Margin = new Thickness(16f, 16f, 16f, 16f);
      imageButton1.HorizontalAlignment = HorizontalAlignment.Right;
      imageButton1.VerticalAlignment = VerticalAlignment.Center;
      imageButton1.ImageStretch = Stretch.Uniform;
      imageButton1.ImageNormal = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol.dds"
      };
      imageButton1.ImageHover = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds"
      };
      imageButton1.ImagePressed = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds"
      };
      Grid.SetColumn((UIElement) imageButton1, 2);
      Binding binding18 = new Binding("ExitGridSelectionCommand");
      imageButton1.SetBinding(Button.CommandProperty, binding18);
      observableCollection.Add((object) tabItem2);
      TabItem tabItem3 = new TabItem();
      tabItem3.Name = "e_46";
      tabItem3.SetBinding(UIElement.VisibilityProperty, new Binding("IsAdministrationVisible")
      {
        UseGeneratedBindings = true
      });
      tabItem3.SetResourceReference(HeaderedContentControl.HeaderProperty, (object) "ContractScreen_Tab_Administration");
      Grid grid14 = new Grid();
      tabItem3.Content = (object) grid14;
      grid14.Name = "e_47";
      grid14.SetBinding(UIElement.DataContextProperty, new Binding("AdministrationViewModel")
      {
        UseGeneratedBindings = true
      });
      Grid grid15 = new Grid();
      grid14.Children.Add((UIElement) grid15);
      grid15.Name = "e_48";
      Grid grid16 = new Grid();
      grid15.Children.Add((UIElement) grid16);
      grid16.Name = "e_49";
      RowDefinition rowDefinition4 = new RowDefinition();
      grid16.RowDefinitions.Add(rowDefinition4);
      grid16.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid16.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid16.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid16.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(2f, GridUnitType.Star)
      });
      grid16.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(3f, GridUnitType.Star)
      });
      ListBox listBox3 = new ListBox();
      grid16.Children.Add((UIElement) listBox3);
      listBox3.Name = "e_50";
      listBox3.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      listBox3.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      listBox3.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      listBox3.SelectionMode = SelectionMode.Single;
      Binding binding19 = new Binding("AdministrableContracts");
      listBox3.SetBinding(ItemsControl.ItemsSourceProperty, binding19);
      Binding binding20 = new Binding("SelectedAdministrableContract");
      listBox3.SetBinding(Selector.SelectedItemProperty, binding20);
      ContractsBlockView.InitializeElemente_50Resources((UIElement) listBox3);
      Grid grid17 = new Grid();
      grid16.Children.Add((UIElement) grid17);
      grid17.Name = "e_51";
      grid17.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(30f, GridUnitType.Pixel)
      });
      grid17.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid17.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Star)
      });
      Grid.SetColumn((UIElement) grid17, 0);
      Grid.SetRow((UIElement) grid17, 0);
      Binding binding21 = new Binding("IsNoAdministrableContractVisible");
      grid17.SetBinding(UIElement.VisibilityProperty, binding21);
      TextBlock textBlock6 = new TextBlock();
      grid17.Children.Add((UIElement) textBlock6);
      textBlock6.Name = "e_52";
      textBlock6.Margin = new Thickness(5f, 5f, 5f, 5f);
      textBlock6.HorizontalAlignment = HorizontalAlignment.Center;
      Grid.SetRow((UIElement) textBlock6, 1);
      textBlock6.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_NoAdministrableContracts");
      Grid grid18 = new Grid();
      grid16.Children.Add((UIElement) grid18);
      grid18.Name = "e_53";
      grid18.Margin = new Thickness(0.0f, 0.0f, 0.0f, 30f);
      ColumnDefinition columnDefinition4 = new ColumnDefinition();
      grid18.ColumnDefinitions.Add(columnDefinition4);
      grid18.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(150f, GridUnitType.Pixel)
      });
      grid18.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(150f, GridUnitType.Pixel)
      });
      Grid.SetColumn((UIElement) grid18, 0);
      Grid.SetRow((UIElement) grid18, 2);
      Button button7 = new Button();
      grid18.Children.Add((UIElement) button7);
      button7.Name = "e_54";
      button7.Margin = new Thickness(10f, 10f, 0.0f, 10f);
      button7.VerticalAlignment = VerticalAlignment.Center;
      button7.TabIndex = 101;
      Grid.SetColumn((UIElement) button7, 1);
      Binding binding22 = new Binding("RefreshCommand");
      button7.SetBinding(Button.CommandProperty, binding22);
      button7.SetResourceReference(ContentControl.ContentProperty, (object) "ContractScreen_Button_RefreshContracts");
      Button button8 = new Button();
      grid18.Children.Add((UIElement) button8);
      button8.Name = "e_55";
      button8.Margin = new Thickness(10f, 10f, 5f, 10f);
      button8.VerticalAlignment = VerticalAlignment.Center;
      button8.TabIndex = 102;
      Grid.SetColumn((UIElement) button8, 2);
      Binding binding23 = new Binding("IsDeleteEnabled");
      button8.SetBinding(UIElement.IsEnabledProperty, binding23);
      Binding binding24 = new Binding("DeleteCommand");
      button8.SetBinding(Button.CommandProperty, binding24);
      button8.SetResourceReference(ContentControl.ContentProperty, (object) "ContractScreen_Button_DeleteContract");
      Border border6 = new Border();
      grid16.Children.Add((UIElement) border6);
      border6.Name = "e_56";
      border6.VerticalAlignment = VerticalAlignment.Stretch;
      border6.Background = (Brush) new SolidColorBrush(new ColorW(33, 44, 53, (int) byte.MaxValue));
      border6.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      border6.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      Grid.SetColumn((UIElement) border6, 1);
      Grid grid19 = new Grid();
      border6.Child = (UIElement) grid19;
      grid19.Name = "e_57";
      grid19.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Star)
      });
      grid19.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid19.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      StackPanel stackPanel1 = new StackPanel();
      grid19.Children.Add((UIElement) stackPanel1);
      stackPanel1.Name = "e_58";
      stackPanel1.Margin = new Thickness(10f, 10f, 0.0f, 0.0f);
      stackPanel1.Orientation = Orientation.Vertical;
      Grid.SetRow((UIElement) stackPanel1, 0);
      TextBlock textBlock7 = new TextBlock();
      stackPanel1.Children.Add((UIElement) textBlock7);
      textBlock7.Name = "e_59";
      textBlock7.Margin = new Thickness(15f, 0.0f, 0.0f, 0.0f);
      textBlock7.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      textBlock7.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_NewContract");
      Grid grid20 = new Grid();
      stackPanel1.Children.Add((UIElement) grid20);
      grid20.Name = "e_60";
      grid20.Margin = new Thickness(15f, 0.0f, 0.0f, 0.0f);
      grid20.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Star)
      });
      grid20.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Star)
      });
      grid20.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Star)
      });
      grid20.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Star)
      });
      grid20.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(44f, GridUnitType.Pixel)
      });
      grid20.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(156f, GridUnitType.Pixel)
      });
      grid20.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      TextBlock textBlock8 = new TextBlock();
      grid20.Children.Add((UIElement) textBlock8);
      textBlock8.Name = "e_61";
      textBlock8.Margin = new Thickness(0.0f, 15f, 0.0f, 15f);
      textBlock8.VerticalAlignment = VerticalAlignment.Center;
      textBlock8.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock8, 0);
      Grid.SetRow((UIElement) textBlock8, 0);
      Grid.SetColumnSpan((UIElement) textBlock8, 2);
      textBlock8.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_MoneyReward");
      TextBlock textBlock9 = new TextBlock();
      grid20.Children.Add((UIElement) textBlock9);
      textBlock9.Name = "e_62";
      textBlock9.Margin = new Thickness(0.0f, 15f, 0.0f, 15f);
      textBlock9.VerticalAlignment = VerticalAlignment.Center;
      textBlock9.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock9, 0);
      Grid.SetRow((UIElement) textBlock9, 1);
      Grid.SetColumnSpan((UIElement) textBlock9, 2);
      textBlock9.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_StartingDeposit");
      TextBlock textBlock10 = new TextBlock();
      grid20.Children.Add((UIElement) textBlock10);
      textBlock10.Name = "e_63";
      textBlock10.Margin = new Thickness(0.0f, 15f, 0.0f, 15f);
      textBlock10.VerticalAlignment = VerticalAlignment.Center;
      textBlock10.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock10, 0);
      Grid.SetRow((UIElement) textBlock10, 2);
      Grid.SetColumnSpan((UIElement) textBlock10, 2);
      textBlock10.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_Duration");
      TextBlock textBlock11 = new TextBlock();
      grid20.Children.Add((UIElement) textBlock11);
      textBlock11.Name = "e_64";
      textBlock11.Margin = new Thickness(0.0f, 15f, 0.0f, 15f);
      textBlock11.VerticalAlignment = VerticalAlignment.Center;
      textBlock11.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock11, 0);
      Grid.SetRow((UIElement) textBlock11, 3);
      Grid.SetColumnSpan((UIElement) textBlock11, 2);
      textBlock11.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_Type");
      NumericTextBox numericTextBox1 = new NumericTextBox();
      grid20.Children.Add((UIElement) numericTextBox1);
      numericTextBox1.Name = "e_65";
      numericTextBox1.Margin = new Thickness(0.0f, 5f, 15f, 0.0f);
      numericTextBox1.VerticalAlignment = VerticalAlignment.Center;
      numericTextBox1.TabIndex = 103;
      numericTextBox1.MaxLength = 7;
      numericTextBox1.Minimum = 0.0f;
      numericTextBox1.Maximum = 9999999f;
      Grid.SetColumn((UIElement) numericTextBox1, 2);
      Grid.SetRow((UIElement) numericTextBox1, 0);
      Binding binding25 = new Binding("NewContractCurrencyReward");
      numericTextBox1.SetBinding(NumericTextBox.ValueProperty, binding25);
      NumericTextBox numericTextBox2 = new NumericTextBox();
      grid20.Children.Add((UIElement) numericTextBox2);
      numericTextBox2.Name = "e_66";
      numericTextBox2.Margin = new Thickness(0.0f, 5f, 15f, 0.0f);
      numericTextBox2.VerticalAlignment = VerticalAlignment.Center;
      numericTextBox2.TabIndex = 104;
      numericTextBox2.MaxLength = 7;
      numericTextBox2.Minimum = 0.0f;
      numericTextBox2.Maximum = 9999999f;
      Grid.SetColumn((UIElement) numericTextBox2, 2);
      Grid.SetRow((UIElement) numericTextBox2, 1);
      Binding binding26 = new Binding("NewContractStartDeposit");
      numericTextBox2.SetBinding(NumericTextBox.ValueProperty, binding26);
      NumericTextBox numericTextBox3 = new NumericTextBox();
      grid20.Children.Add((UIElement) numericTextBox3);
      numericTextBox3.Name = "e_67";
      numericTextBox3.Margin = new Thickness(0.0f, 5f, 15f, 0.0f);
      numericTextBox3.VerticalAlignment = VerticalAlignment.Center;
      numericTextBox3.TabIndex = 105;
      numericTextBox3.MaxLength = 5;
      numericTextBox3.Minimum = 0.0f;
      numericTextBox3.Maximum = 99999f;
      Grid.SetColumn((UIElement) numericTextBox3, 2);
      Grid.SetRow((UIElement) numericTextBox3, 2);
      Binding binding27 = new Binding("NewContractDurationInMin");
      numericTextBox3.SetBinding(NumericTextBox.ValueProperty, binding27);
      ComboBox comboBox3 = new ComboBox();
      grid20.Children.Add((UIElement) comboBox3);
      comboBox3.Name = "e_68";
      comboBox3.Margin = new Thickness(0.0f, 4f, 15f, 4f);
      comboBox3.TabIndex = 106;
      Grid.SetColumn((UIElement) comboBox3, 2);
      Grid.SetRow((UIElement) comboBox3, 3);
      Binding binding28 = new Binding("ContractTypes");
      comboBox3.SetBinding(ItemsControl.ItemsSourceProperty, binding28);
      Binding binding29 = new Binding("SelectedContractTypeIndex");
      comboBox3.SetBinding(Selector.SelectedIndexProperty, binding29);
      Border border7 = new Border();
      stackPanel1.Children.Add((UIElement) border7);
      border7.Name = "e_69";
      border7.Height = 2f;
      border7.Margin = new Thickness(15f, 0.0f, 15f, 0.0f);
      border7.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      Grid grid21 = new Grid();
      stackPanel1.Children.Add((UIElement) grid21);
      grid21.Name = "e_70";
      grid21.Margin = new Thickness(15f, 0.0f, 0.0f, 0.0f);
      Grid grid22 = new Grid();
      grid21.Children.Add((UIElement) grid22);
      grid22.Name = "e_71";
      grid22.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid22.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(200f, GridUnitType.Pixel)
      });
      grid22.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      Binding binding30 = new Binding("IsContractSelected_Deliver");
      grid22.SetBinding(UIElement.VisibilityProperty, binding30);
      TextBlock textBlock12 = new TextBlock();
      grid22.Children.Add((UIElement) textBlock12);
      textBlock12.Name = "e_72";
      textBlock12.Margin = new Thickness(0.0f, 15f, 0.0f, 15f);
      textBlock12.VerticalAlignment = VerticalAlignment.Center;
      textBlock12.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock12, 0);
      Grid.SetRow((UIElement) textBlock12, 0);
      textBlock12.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_TargetBlock");
      Grid grid23 = new Grid();
      grid22.Children.Add((UIElement) grid23);
      grid23.Name = "e_73";
      grid23.Margin = new Thickness(5f, 0.0f, 0.0f, 0.0f);
      grid23.HorizontalAlignment = HorizontalAlignment.Right;
      grid23.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      grid23.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      Grid.SetColumn((UIElement) grid23, 1);
      Grid.SetRow((UIElement) grid23, 0);
      TextBlock textBlock13 = new TextBlock();
      grid23.Children.Add((UIElement) textBlock13);
      textBlock13.Name = "e_74";
      textBlock13.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock13, 0);
      Binding binding31 = new Binding("NewContractSelectionName");
      textBlock13.SetBinding(TextBlock.TextProperty, binding31);
      Button button9 = new Button();
      grid23.Children.Add((UIElement) button9);
      button9.Name = "e_75";
      button9.Width = 150f;
      button9.Margin = new Thickness(10f, 5f, 15f, 0.0f);
      button9.TabIndex = 107;
      Grid.SetColumn((UIElement) button9, 1);
      Binding binding32 = new Binding("NewContractDeliverBlockSelectCommand");
      button9.SetBinding(Button.CommandProperty, binding32);
      button9.SetResourceReference(ContentControl.ContentProperty, (object) "ContractScreen_Button_SelectBlock");
      Grid grid24 = new Grid();
      grid21.Children.Add((UIElement) grid24);
      grid24.Name = "e_76";
      grid24.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid24.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid24.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid24.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(200f, GridUnitType.Pixel)
      });
      grid24.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      Binding binding33 = new Binding("IsContractSelected_ObtainAndDeliver");
      grid24.SetBinding(UIElement.VisibilityProperty, binding33);
      TextBlock textBlock14 = new TextBlock();
      grid24.Children.Add((UIElement) textBlock14);
      textBlock14.Name = "e_77";
      textBlock14.Margin = new Thickness(0.0f, 15f, 0.0f, 15f);
      textBlock14.VerticalAlignment = VerticalAlignment.Center;
      textBlock14.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock14, 0);
      Grid.SetRow((UIElement) textBlock14, 0);
      textBlock14.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_TargetBlock");
      TextBlock textBlock15 = new TextBlock();
      grid24.Children.Add((UIElement) textBlock15);
      textBlock15.Name = "e_78";
      textBlock15.Margin = new Thickness(0.0f, 15f, 0.0f, 15f);
      textBlock15.VerticalAlignment = VerticalAlignment.Center;
      textBlock15.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock15, 0);
      Grid.SetRow((UIElement) textBlock15, 1);
      textBlock15.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_ItemType");
      TextBlock textBlock16 = new TextBlock();
      grid24.Children.Add((UIElement) textBlock16);
      textBlock16.Name = "e_79";
      textBlock16.Margin = new Thickness(0.0f, 15f, 0.0f, 15f);
      textBlock16.VerticalAlignment = VerticalAlignment.Center;
      textBlock16.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock16, 0);
      Grid.SetRow((UIElement) textBlock16, 2);
      textBlock16.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_ItemAmount");
      Grid grid25 = new Grid();
      grid24.Children.Add((UIElement) grid25);
      grid25.Name = "e_80";
      grid25.Margin = new Thickness(5f, 0.0f, 0.0f, 0.0f);
      grid25.HorizontalAlignment = HorizontalAlignment.Right;
      grid25.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      grid25.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      Grid.SetColumn((UIElement) grid25, 1);
      Grid.SetRow((UIElement) grid25, 0);
      TextBlock textBlock17 = new TextBlock();
      grid25.Children.Add((UIElement) textBlock17);
      textBlock17.Name = "e_81";
      textBlock17.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock17, 0);
      Binding binding34 = new Binding("NewContractSelectionName");
      textBlock17.SetBinding(TextBlock.TextProperty, binding34);
      Button button10 = new Button();
      grid25.Children.Add((UIElement) button10);
      button10.Name = "e_82";
      button10.Width = 150f;
      button10.Margin = new Thickness(10f, 5f, 15f, 0.0f);
      button10.TabIndex = 108;
      Grid.SetColumn((UIElement) button10, 1);
      Binding binding35 = new Binding("NewContractObtainAndDeliverBlockSelectCommand");
      button10.SetBinding(Button.CommandProperty, binding35);
      button10.SetResourceReference(ContentControl.ContentProperty, (object) "ContractScreen_Button_SelectBlock");
      ComboBox comboBox4 = new ComboBox();
      grid24.Children.Add((UIElement) comboBox4);
      comboBox4.Name = "e_83";
      comboBox4.Margin = new Thickness(0.0f, 0.0f, 15f, 0.0f);
      comboBox4.TabIndex = 109;
      Grid.SetColumn((UIElement) comboBox4, 1);
      Grid.SetRow((UIElement) comboBox4, 1);
      Binding binding36 = new Binding("DeliverableItems");
      comboBox4.SetBinding(ItemsControl.ItemsSourceProperty, binding36);
      Binding binding37 = new Binding("NewContractObtainAndDeliverSelectedItemType");
      comboBox4.SetBinding(Selector.SelectedItemProperty, binding37);
      NumericTextBox numericTextBox4 = new NumericTextBox();
      grid24.Children.Add((UIElement) numericTextBox4);
      numericTextBox4.Name = "e_84";
      numericTextBox4.Margin = new Thickness(0.0f, 0.0f, 15f, 0.0f);
      numericTextBox4.VerticalAlignment = VerticalAlignment.Center;
      numericTextBox4.TabIndex = 110;
      numericTextBox4.MaxLength = 5;
      numericTextBox4.Minimum = 0.0f;
      numericTextBox4.Maximum = 99999f;
      Grid.SetColumn((UIElement) numericTextBox4, 1);
      Grid.SetRow((UIElement) numericTextBox4, 2);
      Binding binding38 = new Binding("NewContractObtainAndDeliverItemAmount");
      numericTextBox4.SetBinding(NumericTextBox.ValueProperty, binding38);
      Grid grid26 = new Grid();
      grid21.Children.Add((UIElement) grid26);
      grid26.Name = "e_85";
      grid26.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid26.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid26.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(200f, GridUnitType.Pixel)
      });
      grid26.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      Binding binding39 = new Binding("IsContractSelected_Find");
      grid26.SetBinding(UIElement.VisibilityProperty, binding39);
      TextBlock textBlock18 = new TextBlock();
      grid26.Children.Add((UIElement) textBlock18);
      textBlock18.Name = "e_86";
      textBlock18.Margin = new Thickness(0.0f, 15f, 0.0f, 15f);
      textBlock18.VerticalAlignment = VerticalAlignment.Center;
      textBlock18.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock18, 0);
      Grid.SetRow((UIElement) textBlock18, 0);
      textBlock18.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_TargetGrid");
      TextBlock textBlock19 = new TextBlock();
      grid26.Children.Add((UIElement) textBlock19);
      textBlock19.Name = "e_87";
      textBlock19.Margin = new Thickness(0.0f, 15f, 0.0f, 15f);
      textBlock19.VerticalAlignment = VerticalAlignment.Center;
      textBlock19.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock19, 0);
      Grid.SetRow((UIElement) textBlock19, 1);
      textBlock19.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_SearchRadius");
      Grid grid27 = new Grid();
      grid26.Children.Add((UIElement) grid27);
      grid27.Name = "e_88";
      grid27.Margin = new Thickness(5f, 0.0f, 0.0f, 0.0f);
      grid27.HorizontalAlignment = HorizontalAlignment.Right;
      grid27.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      grid27.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      Grid.SetColumn((UIElement) grid27, 1);
      Grid.SetRow((UIElement) grid27, 0);
      TextBlock textBlock20 = new TextBlock();
      grid27.Children.Add((UIElement) textBlock20);
      textBlock20.Name = "e_89";
      textBlock20.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock20, 0);
      Binding binding40 = new Binding("NewContractSelectionName");
      textBlock20.SetBinding(TextBlock.TextProperty, binding40);
      Button button11 = new Button();
      grid27.Children.Add((UIElement) button11);
      button11.Name = "e_90";
      button11.Width = 150f;
      button11.Margin = new Thickness(10f, 5f, 15f, 0.0f);
      button11.VerticalAlignment = VerticalAlignment.Center;
      button11.TabIndex = 111;
      Grid.SetColumn((UIElement) button11, 1);
      Binding binding41 = new Binding("NewContractFindGridSelectCommand");
      button11.SetBinding(Button.CommandProperty, binding41);
      button11.SetResourceReference(ContentControl.ContentProperty, (object) "ContractScreen_Button_SelectGrid");
      NumericTextBox numericTextBox5 = new NumericTextBox();
      grid26.Children.Add((UIElement) numericTextBox5);
      numericTextBox5.Name = "e_91";
      numericTextBox5.Margin = new Thickness(0.0f, 0.0f, 15f, 0.0f);
      numericTextBox5.VerticalAlignment = VerticalAlignment.Center;
      numericTextBox5.TabIndex = 112;
      numericTextBox5.MaxLength = 5;
      numericTextBox5.Minimum = 0.0f;
      numericTextBox5.Maximum = 99999f;
      Grid.SetColumn((UIElement) numericTextBox5, 1);
      Grid.SetRow((UIElement) numericTextBox5, 1);
      Binding binding42 = new Binding("NewContractFindSearchRadius");
      numericTextBox5.SetBinding(NumericTextBox.ValueProperty, binding42);
      Grid grid28 = new Grid();
      grid21.Children.Add((UIElement) grid28);
      grid28.Name = "e_92";
      grid28.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid28.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(200f, GridUnitType.Pixel)
      });
      grid28.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      Binding binding43 = new Binding("IsContractSelected_Repair");
      grid28.SetBinding(UIElement.VisibilityProperty, binding43);
      TextBlock textBlock21 = new TextBlock();
      grid28.Children.Add((UIElement) textBlock21);
      textBlock21.Name = "e_93";
      textBlock21.Margin = new Thickness(0.0f, 15f, 0.0f, 15f);
      textBlock21.VerticalAlignment = VerticalAlignment.Center;
      textBlock21.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock21, 0);
      Grid.SetRow((UIElement) textBlock21, 0);
      textBlock21.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_TargetGrid");
      Grid grid29 = new Grid();
      grid28.Children.Add((UIElement) grid29);
      grid29.Name = "e_94";
      grid29.Margin = new Thickness(5f, 0.0f, 0.0f, 0.0f);
      grid29.HorizontalAlignment = HorizontalAlignment.Right;
      grid29.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      grid29.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      Grid.SetColumn((UIElement) grid29, 1);
      Grid.SetRow((UIElement) grid29, 0);
      TextBlock textBlock22 = new TextBlock();
      grid29.Children.Add((UIElement) textBlock22);
      textBlock22.Name = "e_95";
      textBlock22.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock22, 0);
      Binding binding44 = new Binding("NewContractSelectionName");
      textBlock22.SetBinding(TextBlock.TextProperty, binding44);
      Button button12 = new Button();
      grid29.Children.Add((UIElement) button12);
      button12.Name = "e_96";
      button12.Width = 150f;
      button12.Margin = new Thickness(10f, 5f, 15f, 0.0f);
      button12.TabIndex = 113;
      Grid.SetColumn((UIElement) button12, 1);
      Binding binding45 = new Binding("NewContractRepairGridSelectCommand");
      button12.SetBinding(Button.CommandProperty, binding45);
      button12.SetResourceReference(ContentControl.ContentProperty, (object) "ContractScreen_Button_SelectGrid");
      Border border8 = new Border();
      grid19.Children.Add((UIElement) border8);
      border8.Name = "e_97";
      border8.Height = 2f;
      border8.Margin = new Thickness(25f, 0.0f, 15f, 0.0f);
      border8.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      Grid.SetRow((UIElement) border8, 1);
      Grid grid30 = new Grid();
      grid19.Children.Add((UIElement) grid30);
      grid30.Name = "e_98";
      grid30.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(200f, GridUnitType.Pixel)
      });
      grid30.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      Grid.SetRow((UIElement) grid30, 2);
      TextBlock textBlock23 = new TextBlock();
      grid30.Children.Add((UIElement) textBlock23);
      textBlock23.Name = "e_99";
      textBlock23.Margin = new Thickness(25f, 10f, 0.0f, 10f);
      textBlock23.VerticalAlignment = VerticalAlignment.Center;
      textBlock23.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock23, 0);
      Grid.SetRow((UIElement) textBlock23, 0);
      textBlock23.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_CurrentMoney");
      StackPanel stackPanel2 = new StackPanel();
      grid30.Children.Add((UIElement) stackPanel2);
      stackPanel2.Name = "e_100";
      stackPanel2.Margin = new Thickness(0.0f, 10f, 15f, 10f);
      stackPanel2.HorizontalAlignment = HorizontalAlignment.Right;
      stackPanel2.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel2, 1);
      Grid.SetRow((UIElement) stackPanel2, 0);
      TextBlock textBlock24 = new TextBlock();
      stackPanel2.Children.Add((UIElement) textBlock24);
      textBlock24.Name = "e_101";
      textBlock24.Margin = new Thickness(30f, 0.0f, 0.0f, 0.0f);
      textBlock24.HorizontalAlignment = HorizontalAlignment.Right;
      textBlock24.VerticalAlignment = VerticalAlignment.Center;
      textBlock24.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Binding binding46 = new Binding("CurrentMoneyFormated");
      textBlock24.SetBinding(TextBlock.TextProperty, binding46);
      Image image2 = new Image();
      stackPanel2.Children.Add((UIElement) image2);
      image2.Name = "e_102";
      image2.Height = 20f;
      image2.Margin = new Thickness(4f, 2f, 2f, 2f);
      image2.HorizontalAlignment = HorizontalAlignment.Right;
      image2.VerticalAlignment = VerticalAlignment.Center;
      image2.Stretch = Stretch.Uniform;
      Grid.SetColumn((UIElement) image2, 2);
      Binding binding47 = new Binding("CurrencyIcon");
      image2.SetBinding(Image.SourceProperty, binding47);
      Border border9 = new Border();
      grid16.Children.Add((UIElement) border9);
      border9.Name = "e_103";
      border9.Height = 2f;
      border9.Margin = new Thickness(0.0f, 10f, 0.0f, 10f);
      border9.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      Grid.SetRow((UIElement) border9, 1);
      Grid.SetColumnSpan((UIElement) border9, 2);
      Grid grid31 = new Grid();
      grid16.Children.Add((UIElement) grid31);
      grid31.Name = "e_104";
      grid31.Margin = new Thickness(0.0f, 0.0f, 0.0f, 30f);
      ColumnDefinition columnDefinition5 = new ColumnDefinition();
      grid31.ColumnDefinitions.Add(columnDefinition5);
      grid31.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(150f, GridUnitType.Pixel)
      });
      Grid.SetColumn((UIElement) grid31, 1);
      Grid.SetRow((UIElement) grid31, 2);
      Button button13 = new Button();
      grid31.Children.Add((UIElement) button13);
      button13.Name = "e_105";
      button13.Margin = new Thickness(10f, 10f, 0.0f, 10f);
      button13.VerticalAlignment = VerticalAlignment.Center;
      button13.TabIndex = 114;
      Grid.SetColumn((UIElement) button13, 1);
      Binding binding48 = new Binding("CreateCommand");
      button13.SetBinding(Button.CommandProperty, binding48);
      button13.SetResourceReference(ContentControl.ContentProperty, (object) "ContractScreen_Button_CreateContract");
      Grid grid32 = new Grid();
      grid15.Children.Add((UIElement) grid32);
      grid32.Name = "e_106";
      KeyBinding keyBinding2 = new KeyBinding();
      keyBinding2.Gesture = (InputGesture) new KeyGesture(KeyCode.Escape, ModifierKeys.None, "");
      Binding binding49 = new Binding("AdminSelectionExitCommand");
      keyBinding2.SetBinding(InputBinding.CommandProperty, binding49);
      grid32.InputBindings.Add((InputBinding) keyBinding2);
      keyBinding2.Parent = (UIElement) grid32;
      grid32.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(0.5f, GridUnitType.Star)
      });
      grid32.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(2f, GridUnitType.Star)
      });
      grid32.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Star)
      });
      grid32.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      grid32.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(3f, GridUnitType.Star)
      });
      grid32.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      Grid.SetColumn((UIElement) grid32, 1);
      Grid.SetRow((UIElement) grid32, 3);
      Binding binding50 = new Binding("IsVisibleAdminSelection");
      grid32.SetBinding(UIElement.VisibilityProperty, binding50);
      Image image3 = new Image();
      grid32.Children.Add((UIElement) image3);
      image3.Name = "e_107";
      image3.Source = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Screens\\screen_background.dds"
      };
      image3.Stretch = Stretch.Fill;
      Grid.SetColumn((UIElement) image3, 1);
      Grid.SetRow((UIElement) image3, 1);
      Binding binding51 = new Binding("BackgroundOverlay");
      image3.SetBinding(ImageBrush.ColorOverlayProperty, binding51);
      Grid grid33 = new Grid();
      grid32.Children.Add((UIElement) grid33);
      grid33.Name = "e_108";
      grid33.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid33.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid33.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid33.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(2.5f, GridUnitType.Star)
      });
      grid33.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid33.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid33.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Star)
      });
      grid33.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      grid33.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(12f, GridUnitType.Star)
      });
      grid33.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      Grid.SetColumn((UIElement) grid33, 1);
      Grid.SetRow((UIElement) grid33, 1);
      TextBlock textBlock25 = new TextBlock();
      grid33.Children.Add((UIElement) textBlock25);
      textBlock25.Name = "e_109";
      textBlock25.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock25.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock25.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock25, 1);
      Grid.SetRow((UIElement) textBlock25, 1);
      Binding binding52 = new Binding("AdminSelectionCaption");
      textBlock25.SetBinding(TextBlock.TextProperty, binding52);
      Border border10 = new Border();
      grid33.Children.Add((UIElement) border10);
      border10.Name = "e_110";
      border10.Height = 2f;
      border10.Margin = new Thickness(10f, 10f, 10f, 10f);
      border10.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) border10, 1);
      Grid.SetRow((UIElement) border10, 2);
      TextBlock textBlock26 = new TextBlock();
      grid33.Children.Add((UIElement) textBlock26);
      textBlock26.Name = "e_111";
      textBlock26.Margin = new Thickness(10f, 10f, 10f, 10f);
      textBlock26.HorizontalAlignment = HorizontalAlignment.Left;
      textBlock26.VerticalAlignment = VerticalAlignment.Center;
      textBlock26.TextWrapping = TextWrapping.Wrap;
      Grid.SetColumn((UIElement) textBlock26, 1);
      Grid.SetRow((UIElement) textBlock26, 3);
      Binding binding53 = new Binding("AdminSelectionText");
      textBlock26.SetBinding(TextBlock.TextProperty, binding53);
      ComboBox comboBox5 = new ComboBox();
      grid33.Children.Add((UIElement) comboBox5);
      comboBox5.Name = "e_112";
      comboBox5.Margin = new Thickness(10f, 10f, 10f, 10f);
      Grid.SetColumn((UIElement) comboBox5, 1);
      Grid.SetRow((UIElement) comboBox5, 4);
      Binding binding54 = new Binding("AdminSelectionItems");
      comboBox5.SetBinding(ItemsControl.ItemsSourceProperty, binding54);
      Binding binding55 = new Binding("AdminSelectedItemIndex");
      comboBox5.SetBinding(Selector.SelectedIndexProperty, binding55);
      Button button14 = new Button();
      grid33.Children.Add((UIElement) button14);
      button14.Name = "e_113";
      button14.Width = 150f;
      button14.Margin = new Thickness(10f, 10f, 10f, 10f);
      button14.HorizontalAlignment = HorizontalAlignment.Right;
      button14.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) button14, 1);
      Grid.SetRow((UIElement) button14, 5);
      Binding binding56 = new Binding("AdminSelectionConfirmCommand");
      button14.SetBinding(Button.CommandProperty, binding56);
      button14.SetResourceReference(ContentControl.ContentProperty, (object) "ContractScreen_Button_Confirm");
      ImageButton imageButton2 = new ImageButton();
      grid33.Children.Add((UIElement) imageButton2);
      imageButton2.Name = "e_114";
      imageButton2.Height = 24f;
      imageButton2.Width = 24f;
      imageButton2.Margin = new Thickness(16f, 16f, 16f, 16f);
      imageButton2.HorizontalAlignment = HorizontalAlignment.Right;
      imageButton2.VerticalAlignment = VerticalAlignment.Center;
      imageButton2.ImageStretch = Stretch.Uniform;
      imageButton2.ImageNormal = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol.dds"
      };
      imageButton2.ImageHover = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds"
      };
      imageButton2.ImagePressed = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds"
      };
      Grid.SetColumn((UIElement) imageButton2, 2);
      Binding binding57 = new Binding("AdminSelectionExitCommand");
      imageButton2.SetBinding(Button.CommandProperty, binding57);
      observableCollection.Add((object) tabItem3);
      return observableCollection;
    }

    private static void InitializeElemente_14Resources(UIElement elem) => elem.Resources.MergedDictionaries.Add((ResourceDictionary) DataTemplatesContractsDataGrid.Instance);

    private static void InitializeElemente_27Resources(UIElement elem) => elem.Resources.MergedDictionaries.Add((ResourceDictionary) DataTemplatesContractsDataGrid.Instance);

    private static void InitializeElemente_50Resources(UIElement elem) => elem.Resources.MergedDictionaries.Add((ResourceDictionary) DataTemplatesContractsDataGrid.Instance);
  }
}
