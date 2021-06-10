// Decompiled with JetBrains decompiler
// Type: EmptyKeys.UserInterface.Generated.StoreBlockView_Gamepad
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Controls.Primitives;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.StoreBlockView_Gamepad_Bindings;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Themes;
using Sandbox.Game.Screens.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.ObjectModel;

namespace EmptyKeys.UserInterface.Generated
{
  [GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
  public class StoreBlockView_Gamepad : UIRoot
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

    public StoreBlockView_Gamepad() => this.Initialize();

    public StoreBlockView_Gamepad(int width, int height)
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
      StoreBlockView_Gamepad.InitializeElementResources((UIElement) this);
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
      this.e_4.SetResourceReference(TextBlock.TextProperty, (object) "ScreenCaptionStore");
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
      this.e_8.ItemsSource = (IEnumerable) StoreBlockView_Gamepad.Get_e_8_Items();
      Grid.SetColumn((UIElement) this.e_8, 1);
      Grid.SetRow((UIElement) this.e_8, 3);
      this.e_8.SetBinding(Selector.SelectedIndexProperty, new Binding("TabSelectedIndex")
      {
        UseGeneratedBindings = true
      });
      ImageManager.Instance.AddImage("Textures\\GUI\\Screens\\screen_background.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol_highlight.dds");
      FontManager.Instance.AddFont("LargeFont", 16.6f, FontStyle.Regular, "LargeFont_12.45_Regular");
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "MaxWidth", typeof (MyStoreBlockViewModel_MaxWidth_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "BackgroundOverlay", typeof (MyStoreBlockViewModel_BackgroundOverlay_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "ExitCommand", typeof (MyStoreBlockViewModel_ExitCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "IsBuyTabVisible", typeof (MyStoreBlockViewModel_IsBuyTabVisible_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "BuyCommand", typeof (MyStoreBlockViewModel_BuyCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "RefreshCommand", typeof (MyStoreBlockViewModel_RefreshCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "SwitchSortOffersCommand", typeof (MyStoreBlockViewModel_SwitchSortOffersCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "PreviousInventoryCommand", typeof (MyStoreBlockViewModel_PreviousInventoryCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "NextInventoryCommand", typeof (MyStoreBlockViewModel_NextInventoryCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "OfferedItems", typeof (MyStoreBlockViewModel_OfferedItems_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "SelectedOfferItem", typeof (MyStoreBlockViewModel_SelectedOfferItem_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "SortingOfferedItemsCommand", typeof (MyStoreBlockViewModel_SortingOfferedItemsCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "AmountToBuyMaximum", typeof (MyStoreBlockViewModel_AmountToBuyMaximum_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "AmountToBuy", typeof (MyStoreBlockViewModel_AmountToBuy_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "TotalPriceToBuy", typeof (MyStoreBlockViewModel_TotalPriceToBuy_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "InventoryTargetViewModel", typeof (MyStoreBlockViewModel_InventoryTargetViewModel_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryTargetViewModel), "CurrencyIcon", typeof (MyInventoryTargetViewModel_CurrencyIcon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryTargetViewModel), "DepositCommand", typeof (MyInventoryTargetViewModel_DepositCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryTargetViewModel), "WithdrawCommand", typeof (MyInventoryTargetViewModel_WithdrawCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryTargetViewModel), "BalanceChangeValue", typeof (MyInventoryTargetViewModel_BalanceChangeValue_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryTargetViewModel), "Inventories", typeof (MyInventoryTargetViewModel_Inventories_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryTargetViewModel), "SelectedInventoryIndex", typeof (MyInventoryTargetViewModel_SelectedInventoryIndex_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryTargetViewModel), "InventoryItems", typeof (MyInventoryTargetViewModel_InventoryItems_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryTargetViewModel), "CurrentInventoryVolume", typeof (MyInventoryTargetViewModel_CurrentInventoryVolume_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryTargetViewModel), "MaxInventoryVolume", typeof (MyInventoryTargetViewModel_MaxInventoryVolume_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryTargetViewModel), "LocalPlayerCurrency", typeof (MyInventoryTargetViewModel_LocalPlayerCurrency_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "SortModeOffersText", typeof (MyStoreBlockViewModel_SortModeOffersText_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "IsSellTabVisible", typeof (MyStoreBlockViewModel_IsSellTabVisible_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "SellCommand", typeof (MyStoreBlockViewModel_SellCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "SwitchSortOrdersCommand", typeof (MyStoreBlockViewModel_SwitchSortOrdersCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "OrderedItems", typeof (MyStoreBlockViewModel_OrderedItems_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "SelectedOrderItem", typeof (MyStoreBlockViewModel_SelectedOrderItem_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "SortingDemandedItemsCommand", typeof (MyStoreBlockViewModel_SortingDemandedItemsCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "AmountToSell", typeof (MyStoreBlockViewModel_AmountToSell_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "TotalPriceToSell", typeof (MyStoreBlockViewModel_TotalPriceToSell_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "SortModeOrdersText", typeof (MyStoreBlockViewModel_SortModeOrdersText_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "IsAdministrationVisible", typeof (MyStoreBlockViewModel_IsAdministrationVisible_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "AdministrationViewModel", typeof (MyStoreBlockViewModel_AdministrationViewModel_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "RemoveStoreItemCommand", typeof (MyStoreBlockAdministrationViewModel_RemoveStoreItemCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "StoreItems", typeof (MyStoreBlockAdministrationViewModel_StoreItems_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "SelectedStoreItem", typeof (MyStoreBlockAdministrationViewModel_SelectedStoreItem_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "CreateOfferCommand", typeof (MyStoreBlockAdministrationViewModel_CreateOfferCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "OrderItems", typeof (MyStoreBlockAdministrationViewModel_OrderItems_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "SelectedOfferItem", typeof (MyStoreBlockAdministrationViewModel_SelectedOfferItem_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "OfferAmount", typeof (MyStoreBlockAdministrationViewModel_OfferAmount_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "OfferPricePerUnitMinimum", typeof (MyStoreBlockAdministrationViewModel_OfferPricePerUnitMinimum_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "OfferPricePerUnit", typeof (MyStoreBlockAdministrationViewModel_OfferPricePerUnit_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "OfferTotalPrice", typeof (MyStoreBlockAdministrationViewModel_OfferTotalPrice_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "CurrencyIcon", typeof (MyStoreBlockAdministrationViewModel_CurrencyIcon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "OfferListingFee", typeof (MyStoreBlockAdministrationViewModel_OfferListingFee_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "OfferTransactionFee", typeof (MyStoreBlockAdministrationViewModel_OfferTransactionFee_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "LocalPlayerCurrency", typeof (MyStoreBlockAdministrationViewModel_LocalPlayerCurrency_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "IsTabNewOrderVisible", typeof (MyStoreBlockAdministrationViewModel_IsTabNewOrderVisible_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "CreateOrderCommand", typeof (MyStoreBlockAdministrationViewModel_CreateOrderCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "SelectedOrderItem", typeof (MyStoreBlockAdministrationViewModel_SelectedOrderItem_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "OrderAmount", typeof (MyStoreBlockAdministrationViewModel_OrderAmount_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "OrderPricePerUnitMaximum", typeof (MyStoreBlockAdministrationViewModel_OrderPricePerUnitMaximum_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "OrderPricePerUnit", typeof (MyStoreBlockAdministrationViewModel_OrderPricePerUnit_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "OrderTotalPrice", typeof (MyStoreBlockAdministrationViewModel_OrderTotalPrice_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "OrderListingFee", typeof (MyStoreBlockAdministrationViewModel_OrderListingFee_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "TabSelectedIndex", typeof (MyStoreBlockViewModel_TabSelectedIndex_PropertyInfo));
    }

    private static void InitializeElementResources(UIElement elem)
    {
      elem.Resources.MergedDictionaries.Add((ResourceDictionary) Styles.Instance);
      elem.Resources.MergedDictionaries.Add((ResourceDictionary) DataTemplatesStoreBlock.Instance);
    }

    private static ObservableCollection<object> Get_e_8_Items()
    {
      ObservableCollection<object> observableCollection = new ObservableCollection<object>();
      TabItem tabItem1 = new TabItem();
      tabItem1.Name = "e_9";
      tabItem1.IsTabStop = false;
      tabItem1.SetBinding(UIElement.VisibilityProperty, new Binding("IsBuyTabVisible")
      {
        UseGeneratedBindings = true
      });
      tabItem1.SetResourceReference(HeaderedContentControl.HeaderProperty, (object) "StoreScreenBuyHeader");
      Grid grid1 = new Grid();
      tabItem1.Content = (object) grid1;
      grid1.Name = "e_10";
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
        Width = new GridLength(1f, GridUnitType.Star)
      });
      Grid grid2 = new Grid();
      grid1.Children.Add((UIElement) grid2);
      grid2.Name = "e_11";
      GamepadBinding gamepadBinding1 = new GamepadBinding();
      gamepadBinding1.Gesture = (InputGesture) new GamepadGesture(GamepadInput.CButton);
      gamepadBinding1.SetBinding(InputBinding.CommandProperty, new Binding("BuyCommand")
      {
        UseGeneratedBindings = true
      });
      grid2.InputBindings.Add((InputBinding) gamepadBinding1);
      gamepadBinding1.Parent = (UIElement) grid2;
      GamepadBinding gamepadBinding2 = new GamepadBinding();
      gamepadBinding2.Gesture = (InputGesture) new GamepadGesture(GamepadInput.DButton);
      gamepadBinding2.SetBinding(InputBinding.CommandProperty, new Binding("RefreshCommand")
      {
        UseGeneratedBindings = true
      });
      grid2.InputBindings.Add((InputBinding) gamepadBinding2);
      gamepadBinding2.Parent = (UIElement) grid2;
      RowDefinition rowDefinition2 = new RowDefinition();
      grid2.RowDefinitions.Add(rowDefinition2);
      grid2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid2.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition1 = new ColumnDefinition();
      grid2.ColumnDefinitions.Add(columnDefinition1);
      DataGrid dataGrid1 = new DataGrid();
      grid2.Children.Add((UIElement) dataGrid1);
      dataGrid1.Name = "e_12";
      GamepadBinding gamepadBinding3 = new GamepadBinding();
      gamepadBinding3.Gesture = (InputGesture) new GamepadGesture(GamepadInput.LeftStickButton);
      gamepadBinding3.SetBinding(InputBinding.CommandProperty, new Binding("SwitchSortOffersCommand")
      {
        UseGeneratedBindings = true
      });
      dataGrid1.InputBindings.Add((InputBinding) gamepadBinding3);
      gamepadBinding3.Parent = (UIElement) dataGrid1;
      GamepadBinding gamepadBinding4 = new GamepadBinding();
      gamepadBinding4.Gesture = (InputGesture) new GamepadGesture(GamepadInput.LeftShoulderButton);
      gamepadBinding4.SetBinding(InputBinding.CommandProperty, new Binding("PreviousInventoryCommand")
      {
        UseGeneratedBindings = true
      });
      dataGrid1.InputBindings.Add((InputBinding) gamepadBinding4);
      gamepadBinding4.Parent = (UIElement) dataGrid1;
      GamepadBinding gamepadBinding5 = new GamepadBinding();
      gamepadBinding5.Gesture = (InputGesture) new GamepadGesture(GamepadInput.RightShoulderButton);
      gamepadBinding5.SetBinding(InputBinding.CommandProperty, new Binding("NextInventoryCommand")
      {
        UseGeneratedBindings = true
      });
      dataGrid1.InputBindings.Add((InputBinding) gamepadBinding5);
      gamepadBinding5.Parent = (UIElement) dataGrid1;
      dataGrid1.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      dataGrid1.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      dataGrid1.TabIndex = 1;
      dataGrid1.SelectionMode = DataGridSelectionMode.Single;
      dataGrid1.AutoGenerateColumns = false;
      DataGridTemplateColumn gridTemplateColumn1 = new DataGridTemplateColumn();
      gridTemplateColumn1.Width = (DataGridLength) 64f;
      gridTemplateColumn1.SortMemberPath = "Name";
      TextBlock textBlock1 = new TextBlock();
      textBlock1.Name = "e_13";
      textBlock1.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock1.Text = "";
      gridTemplateColumn1.Header = (object) textBlock1;
      Func<UIElement, UIElement> createMethod1 = new Func<UIElement, UIElement>(StoreBlockView_Gamepad.e_12_Col0_ct_dtMethod);
      gridTemplateColumn1.CellTemplate = new DataTemplate(createMethod1);
      dataGrid1.Columns.Add((DataGridColumn) gridTemplateColumn1);
      DataGridTemplateColumn gridTemplateColumn2 = new DataGridTemplateColumn();
      gridTemplateColumn2.Width = new DataGridLength(1f, DataGridLengthUnitType.Star);
      gridTemplateColumn2.SortMemberPath = "Name";
      TextBlock textBlock2 = new TextBlock();
      textBlock2.Name = "e_25";
      textBlock2.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock2.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlock_Column_Name");
      gridTemplateColumn2.Header = (object) textBlock2;
      Func<UIElement, UIElement> createMethod2 = new Func<UIElement, UIElement>(StoreBlockView_Gamepad.e_12_Col1_ct_dtMethod);
      gridTemplateColumn2.CellTemplate = new DataTemplate(createMethod2);
      dataGrid1.Columns.Add((DataGridColumn) gridTemplateColumn2);
      DataGridTemplateColumn gridTemplateColumn3 = new DataGridTemplateColumn();
      gridTemplateColumn3.Width = (DataGridLength) 150f;
      gridTemplateColumn3.SortMemberPath = "Amount";
      TextBlock textBlock3 = new TextBlock();
      textBlock3.Name = "e_27";
      textBlock3.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock3.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlock_Column_Amount");
      gridTemplateColumn3.Header = (object) textBlock3;
      Func<UIElement, UIElement> createMethod3 = new Func<UIElement, UIElement>(StoreBlockView_Gamepad.e_12_Col2_ct_dtMethod);
      gridTemplateColumn3.CellTemplate = new DataTemplate(createMethod3);
      dataGrid1.Columns.Add((DataGridColumn) gridTemplateColumn3);
      DataGridTemplateColumn gridTemplateColumn4 = new DataGridTemplateColumn();
      gridTemplateColumn4.Width = (DataGridLength) 200f;
      gridTemplateColumn4.SortMemberPath = "PricePerUnit";
      TextBlock textBlock4 = new TextBlock();
      textBlock4.Name = "e_29";
      textBlock4.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock4.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlock_Column_PricePerUnit");
      gridTemplateColumn4.Header = (object) textBlock4;
      Func<UIElement, UIElement> createMethod4 = new Func<UIElement, UIElement>(StoreBlockView_Gamepad.e_12_Col3_ct_dtMethod);
      gridTemplateColumn4.CellTemplate = new DataTemplate(createMethod4);
      dataGrid1.Columns.Add((DataGridColumn) gridTemplateColumn4);
      Grid.SetColumnSpan((UIElement) dataGrid1, 2);
      GamepadHelp.SetTargetName((DependencyObject) dataGrid1, "DataGridHelp");
      GamepadHelp.SetTabIndexRight((DependencyObject) dataGrid1, 4);
      GamepadHelp.SetTabIndexDown((DependencyObject) dataGrid1, 2);
      dataGrid1.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("OfferedItems")
      {
        UseGeneratedBindings = true
      });
      dataGrid1.SetBinding(Selector.SelectedItemProperty, new Binding("SelectedOfferItem")
      {
        UseGeneratedBindings = true
      });
      dataGrid1.SetBinding(DataGrid.SortingCommandProperty, new Binding("SortingOfferedItemsCommand")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock5 = new TextBlock();
      grid2.Children.Add((UIElement) textBlock5);
      textBlock5.Name = "e_37";
      textBlock5.Margin = new Thickness(0.0f, 5f, 0.0f, 5f);
      textBlock5.VerticalAlignment = VerticalAlignment.Center;
      textBlock5.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock5, 0);
      Grid.SetRow((UIElement) textBlock5, 1);
      textBlock5.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlockView_AmountLabel");
      Grid grid3 = new Grid();
      grid2.Children.Add((UIElement) grid3);
      grid3.Name = "e_38";
      grid3.HorizontalAlignment = HorizontalAlignment.Stretch;
      ColumnDefinition columnDefinition2 = new ColumnDefinition();
      grid3.ColumnDefinitions.Add(columnDefinition2);
      grid3.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(100f, GridUnitType.Pixel)
      });
      grid3.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(200f, GridUnitType.Pixel)
      });
      grid3.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      Grid.SetColumn((UIElement) grid3, 1);
      Grid.SetRow((UIElement) grid3, 1);
      Slider slider1 = new Slider();
      grid3.Children.Add((UIElement) slider1);
      slider1.Name = "e_39";
      slider1.Margin = new Thickness(5f, 5f, 5f, 5f);
      slider1.VerticalAlignment = VerticalAlignment.Center;
      slider1.TabIndex = 2;
      slider1.Minimum = 0.0f;
      slider1.IsSnapToTickEnabled = true;
      slider1.TickFrequency = 1f;
      Grid.SetColumn((UIElement) slider1, 0);
      GamepadHelp.SetTargetName((DependencyObject) slider1, "AmountHelp");
      GamepadHelp.SetTabIndexUp((DependencyObject) slider1, 1);
      GamepadHelp.SetTabIndexDown((DependencyObject) slider1, 3);
      slider1.SetBinding(RangeBase.MaximumProperty, new Binding("AmountToBuyMaximum")
      {
        UseGeneratedBindings = true
      });
      slider1.SetBinding(RangeBase.ValueProperty, new Binding("AmountToBuy")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock6 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock6);
      textBlock6.Name = "e_40";
      textBlock6.Margin = new Thickness(5f, 5f, 0.0f, 5f);
      textBlock6.VerticalAlignment = VerticalAlignment.Center;
      textBlock6.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock6, 1);
      textBlock6.SetBinding(TextBlock.TextProperty, new Binding("AmountToBuy")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock7 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock7);
      textBlock7.Name = "e_41";
      textBlock7.HorizontalAlignment = HorizontalAlignment.Right;
      textBlock7.VerticalAlignment = VerticalAlignment.Center;
      textBlock7.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock7, 2);
      textBlock7.SetBinding(TextBlock.TextProperty, new Binding("TotalPriceToBuy")
      {
        UseGeneratedBindings = true
      });
      Image image1 = new Image();
      grid3.Children.Add((UIElement) image1);
      image1.Name = "e_42";
      image1.Width = 20f;
      image1.Margin = new Thickness(4f, 2f, 2f, 2f);
      image1.VerticalAlignment = VerticalAlignment.Center;
      image1.Stretch = Stretch.Uniform;
      Grid.SetColumn((UIElement) image1, 3);
      image1.SetBinding(Image.SourceProperty, new Binding("InventoryTargetViewModel.CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock8 = new TextBlock();
      grid2.Children.Add((UIElement) textBlock8);
      textBlock8.Name = "e_43";
      textBlock8.Margin = new Thickness(0.0f, 5f, 0.0f, 5f);
      textBlock8.VerticalAlignment = VerticalAlignment.Center;
      textBlock8.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock8, 0);
      Grid.SetRow((UIElement) textBlock8, 2);
      textBlock8.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlockView_CashBack");
      Grid grid4 = new Grid();
      grid2.Children.Add((UIElement) grid4);
      grid4.Name = "e_44";
      Grid.SetColumn((UIElement) grid4, 1);
      Grid.SetRow((UIElement) grid4, 2);
      grid4.SetBinding(UIElement.DataContextProperty, new Binding("InventoryTargetViewModel")
      {
        UseGeneratedBindings = true
      });
      Grid grid5 = new Grid();
      grid4.Children.Add((UIElement) grid5);
      grid5.Name = "e_45";
      ColumnDefinition columnDefinition3 = new ColumnDefinition();
      grid5.ColumnDefinitions.Add(columnDefinition3);
      grid5.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(100f, GridUnitType.Pixel)
      });
      grid5.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(200f, GridUnitType.Pixel)
      });
      grid5.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      Slider slider2 = new Slider();
      grid5.Children.Add((UIElement) slider2);
      slider2.Name = "e_46";
      slider2.Margin = new Thickness(5f, 5f, 5f, 5f);
      slider2.VerticalAlignment = VerticalAlignment.Center;
      GamepadBinding gamepadBinding6 = new GamepadBinding();
      gamepadBinding6.Gesture = (InputGesture) new GamepadGesture(GamepadInput.CButton);
      gamepadBinding6.SetBinding(InputBinding.CommandProperty, new Binding("DepositCommand")
      {
        UseGeneratedBindings = true
      });
      slider2.InputBindings.Add((InputBinding) gamepadBinding6);
      gamepadBinding6.Parent = (UIElement) slider2;
      GamepadBinding gamepadBinding7 = new GamepadBinding();
      gamepadBinding7.Gesture = (InputGesture) new GamepadGesture(GamepadInput.DButton);
      gamepadBinding7.SetBinding(InputBinding.CommandProperty, new Binding("WithdrawCommand")
      {
        UseGeneratedBindings = true
      });
      slider2.InputBindings.Add((InputBinding) gamepadBinding7);
      gamepadBinding7.Parent = (UIElement) slider2;
      slider2.TabIndex = 3;
      slider2.Minimum = 0.0f;
      slider2.Maximum = 1000000f;
      slider2.IsSnapToTickEnabled = true;
      slider2.TickFrequency = 1f;
      Grid.SetColumn((UIElement) slider2, 0);
      GamepadHelp.SetTargetName((DependencyObject) slider2, "CashbackHelp");
      GamepadHelp.SetTabIndexUp((DependencyObject) slider2, 2);
      slider2.SetBinding(RangeBase.ValueProperty, new Binding("BalanceChangeValue")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock9 = new TextBlock();
      grid5.Children.Add((UIElement) textBlock9);
      textBlock9.Name = "e_47";
      textBlock9.Margin = new Thickness(5f, 5f, 0.0f, 5f);
      textBlock9.HorizontalAlignment = HorizontalAlignment.Right;
      textBlock9.VerticalAlignment = VerticalAlignment.Center;
      textBlock9.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock9, 2);
      textBlock9.SetBinding(TextBlock.TextProperty, new Binding("BalanceChangeValue")
      {
        UseGeneratedBindings = true
      });
      Image image2 = new Image();
      grid5.Children.Add((UIElement) image2);
      image2.Name = "e_48";
      image2.Height = 20f;
      image2.Margin = new Thickness(4f, 2f, 2f, 2f);
      image2.VerticalAlignment = VerticalAlignment.Center;
      image2.Stretch = Stretch.Uniform;
      Grid.SetColumn((UIElement) image2, 3);
      image2.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      Grid grid6 = new Grid();
      grid1.Children.Add((UIElement) grid6);
      grid6.Name = "e_49";
      grid6.Margin = new Thickness(10f, 0.0f, 0.0f, 0.0f);
      Grid.SetColumn((UIElement) grid6, 1);
      Grid.SetRow((UIElement) grid6, 0);
      grid6.SetBinding(UIElement.DataContextProperty, new Binding("InventoryTargetViewModel")
      {
        UseGeneratedBindings = true
      });
      Grid grid7 = new Grid();
      grid6.Children.Add((UIElement) grid7);
      grid7.Name = "e_50";
      grid7.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid7.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      RowDefinition rowDefinition3 = new RowDefinition();
      grid7.RowDefinitions.Add(rowDefinition3);
      grid7.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid7.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      TextBlock textBlock10 = new TextBlock();
      grid7.Children.Add((UIElement) textBlock10);
      textBlock10.Name = "e_51";
      textBlock10.Margin = new Thickness(0.0f, 0.0f, 0.0f, 5f);
      textBlock10.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetRow((UIElement) textBlock10, 0);
      textBlock10.SetResourceReference(TextBlock.TextProperty, (object) "StoreScreen_SelectInventory");
      ComboBox comboBox1 = new ComboBox();
      grid7.Children.Add((UIElement) comboBox1);
      comboBox1.Name = "e_52";
      comboBox1.TabIndex = 4;
      comboBox1.MaxDropDownHeight = 300f;
      Grid.SetRow((UIElement) comboBox1, 1);
      GamepadHelp.SetTargetName((DependencyObject) comboBox1, "SelectHelpOffers");
      GamepadHelp.SetTabIndexLeft((DependencyObject) comboBox1, 1);
      GamepadHelp.SetTabIndexDown((DependencyObject) comboBox1, 5);
      comboBox1.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("Inventories")
      {
        UseGeneratedBindings = true
      });
      comboBox1.SetBinding(Selector.SelectedIndexProperty, new Binding("SelectedInventoryIndex")
      {
        UseGeneratedBindings = true
      });
      ListBox listBox1 = new ListBox();
      grid7.Children.Add((UIElement) listBox1);
      listBox1.Name = "e_53";
      listBox1.Margin = new Thickness(0.0f, 4f, 0.0f, 5f);
      Style style1 = new Style(typeof (ListBox));
      Setter setter1 = new Setter(UIElement.MinHeightProperty, (object) 80f);
      style1.Setters.Add(setter1);
      ControlTemplate controlTemplate1 = new ControlTemplate(typeof (ListBox), new Func<UIElement, UIElement>(StoreBlockView_Gamepad.e_53_s_S_1_ctMethod));
      Setter setter2 = new Setter(Control.TemplateProperty, (object) controlTemplate1);
      style1.Setters.Add(setter2);
      Trigger trigger1 = new Trigger();
      trigger1.Property = UIElement.IsFocusedProperty;
      trigger1.Value = (object) true;
      Setter setter3 = new Setter(Control.BackgroundProperty, (object) new SolidColorBrush(new ColorW(67, 81, 92, 160)));
      trigger1.Setters.Add(setter3);
      style1.Triggers.Add((TriggerBase) trigger1);
      listBox1.Style = style1;
      listBox1.Background = (Brush) new SolidColorBrush(new ColorW(33, 44, 53, (int) byte.MaxValue));
      listBox1.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      listBox1.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      listBox1.TabIndex = 5;
      Grid.SetRow((UIElement) listBox1, 2);
      GamepadHelp.SetTargetName((DependencyObject) listBox1, "SelectHelpOffers");
      GamepadHelp.SetTabIndexLeft((DependencyObject) listBox1, 1);
      GamepadHelp.SetTabIndexUp((DependencyObject) listBox1, 4);
      DragDrop.SetIsDropTarget((UIElement) listBox1, true);
      listBox1.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("InventoryItems")
      {
        UseGeneratedBindings = true
      });
      StoreBlockView_Gamepad.InitializeElemente_53Resources((UIElement) listBox1);
      Grid grid8 = new Grid();
      grid7.Children.Add((UIElement) grid8);
      grid8.Name = "e_58";
      grid8.Margin = new Thickness(0.0f, 0.0f, 0.0f, 10f);
      grid8.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition4 = new ColumnDefinition();
      grid8.ColumnDefinitions.Add(columnDefinition4);
      Grid.SetRow((UIElement) grid8, 3);
      TextBlock textBlock11 = new TextBlock();
      grid8.Children.Add((UIElement) textBlock11);
      textBlock11.Name = "e_59";
      textBlock11.Margin = new Thickness(0.0f, 5f, 5f, 5f);
      textBlock11.VerticalAlignment = VerticalAlignment.Center;
      textBlock11.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      textBlock11.SetResourceReference(TextBlock.TextProperty, (object) "ScreenTerminalInventory_Volume");
      StackPanel stackPanel1 = new StackPanel();
      grid8.Children.Add((UIElement) stackPanel1);
      stackPanel1.Name = "e_60";
      stackPanel1.Margin = new Thickness(5f, 5f, 0.0f, 5f);
      stackPanel1.HorizontalAlignment = HorizontalAlignment.Right;
      stackPanel1.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel1, 1);
      TextBlock textBlock12 = new TextBlock();
      stackPanel1.Children.Add((UIElement) textBlock12);
      textBlock12.Name = "e_61";
      textBlock12.VerticalAlignment = VerticalAlignment.Center;
      textBlock12.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      textBlock12.SetBinding(TextBlock.TextProperty, new Binding("CurrentInventoryVolume")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock13 = new TextBlock();
      stackPanel1.Children.Add((UIElement) textBlock13);
      textBlock13.Name = "e_62";
      textBlock13.Margin = new Thickness(10f, 0.0f, 10f, 0.0f);
      textBlock13.VerticalAlignment = VerticalAlignment.Center;
      textBlock13.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      textBlock13.Text = "/";
      TextBlock textBlock14 = new TextBlock();
      stackPanel1.Children.Add((UIElement) textBlock14);
      textBlock14.Name = "e_63";
      textBlock14.VerticalAlignment = VerticalAlignment.Center;
      textBlock14.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      textBlock14.SetBinding(TextBlock.TextProperty, new Binding("MaxInventoryVolume")
      {
        UseGeneratedBindings = true
      });
      Grid grid9 = new Grid();
      grid7.Children.Add((UIElement) grid9);
      grid9.Name = "e_64";
      grid9.Margin = new Thickness(0.0f, 5f, 0.0f, 5f);
      RowDefinition rowDefinition4 = new RowDefinition();
      grid9.RowDefinitions.Add(rowDefinition4);
      RowDefinition rowDefinition5 = new RowDefinition();
      grid9.RowDefinitions.Add(rowDefinition5);
      RowDefinition rowDefinition6 = new RowDefinition();
      grid9.RowDefinitions.Add(rowDefinition6);
      grid9.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition5 = new ColumnDefinition();
      grid9.ColumnDefinitions.Add(columnDefinition5);
      grid9.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      Grid.SetRow((UIElement) grid9, 4);
      TextBlock textBlock15 = new TextBlock();
      grid9.Children.Add((UIElement) textBlock15);
      textBlock15.Name = "e_65";
      textBlock15.Margin = new Thickness(0.0f, 5f, 5f, 5f);
      textBlock15.VerticalAlignment = VerticalAlignment.Center;
      textBlock15.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      textBlock15.SetResourceReference(TextBlock.TextProperty, (object) "Currency_Default_Account_Label");
      TextBlock textBlock16 = new TextBlock();
      grid9.Children.Add((UIElement) textBlock16);
      textBlock16.Name = "e_66";
      textBlock16.Margin = new Thickness(5f, 5f, 0.0f, 5f);
      textBlock16.HorizontalAlignment = HorizontalAlignment.Right;
      textBlock16.VerticalAlignment = VerticalAlignment.Center;
      textBlock16.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock16, 1);
      textBlock16.SetBinding(TextBlock.TextProperty, new Binding("LocalPlayerCurrency")
      {
        UseGeneratedBindings = true
      });
      Image image3 = new Image();
      grid9.Children.Add((UIElement) image3);
      image3.Name = "e_67";
      image3.Height = 20f;
      image3.Margin = new Thickness(4f, 2f, 2f, 2f);
      image3.VerticalAlignment = VerticalAlignment.Center;
      image3.Stretch = Stretch.Uniform;
      Grid.SetColumn((UIElement) image3, 2);
      image3.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      Border border1 = new Border();
      grid1.Children.Add((UIElement) border1);
      border1.Name = "e_68";
      border1.Height = 2f;
      border1.Margin = new Thickness(0.0f, 10f, 0.0f, 0.0f);
      border1.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      Grid.SetRow((UIElement) border1, 1);
      Grid.SetColumnSpan((UIElement) border1, 2);
      Grid grid10 = new Grid();
      grid1.Children.Add((UIElement) grid10);
      grid10.Name = "DataGridHelp";
      grid10.Visibility = Visibility.Collapsed;
      grid10.VerticalAlignment = VerticalAlignment.Center;
      grid10.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid10.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid10.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid10.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid10.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition6 = new ColumnDefinition();
      grid10.ColumnDefinitions.Add(columnDefinition6);
      Grid.SetRow((UIElement) grid10, 2);
      TextBlock textBlock17 = new TextBlock();
      grid10.Children.Add((UIElement) textBlock17);
      textBlock17.Name = "e_69";
      textBlock17.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      textBlock17.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock17.VerticalAlignment = VerticalAlignment.Center;
      textBlock17.SetResourceReference(TextBlock.TextProperty, (object) "StoreScreen_Help_Buy");
      TextBlock textBlock18 = new TextBlock();
      grid10.Children.Add((UIElement) textBlock18);
      textBlock18.Name = "e_70";
      textBlock18.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock18.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock18.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock18, 1);
      textBlock18.SetResourceReference(TextBlock.TextProperty, (object) "StoreScreen_Help_Refresh");
      StackPanel stackPanel2 = new StackPanel();
      grid10.Children.Add((UIElement) stackPanel2);
      stackPanel2.Name = "e_71";
      stackPanel2.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      stackPanel2.HorizontalAlignment = HorizontalAlignment.Center;
      stackPanel2.VerticalAlignment = VerticalAlignment.Center;
      stackPanel2.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel2, 2);
      TextBlock textBlock19 = new TextBlock();
      stackPanel2.Children.Add((UIElement) textBlock19);
      textBlock19.Name = "e_72";
      textBlock19.SetResourceReference(TextBlock.TextProperty, (object) "StoreScreen_Help_SortBy");
      TextBlock textBlock20 = new TextBlock();
      stackPanel2.Children.Add((UIElement) textBlock20);
      textBlock20.Name = "e_73";
      textBlock20.SetBinding(TextBlock.TextProperty, new Binding("SortModeOffersText")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock21 = new TextBlock();
      grid10.Children.Add((UIElement) textBlock21);
      textBlock21.Name = "e_74";
      textBlock21.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock21.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock21.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock21, 3);
      textBlock21.SetResourceReference(TextBlock.TextProperty, (object) "StoreScreen_Help_ChangeInventory");
      TextBlock textBlock22 = new TextBlock();
      grid10.Children.Add((UIElement) textBlock22);
      textBlock22.Name = "e_75";
      textBlock22.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock22.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock22.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock22, 4);
      textBlock22.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Back");
      Grid grid11 = new Grid();
      grid1.Children.Add((UIElement) grid11);
      grid11.Name = "AmountHelp";
      grid11.Visibility = Visibility.Collapsed;
      grid11.VerticalAlignment = VerticalAlignment.Center;
      grid11.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid11.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid11.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid11.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition7 = new ColumnDefinition();
      grid11.ColumnDefinitions.Add(columnDefinition7);
      Grid.SetRow((UIElement) grid11, 2);
      TextBlock textBlock23 = new TextBlock();
      grid11.Children.Add((UIElement) textBlock23);
      textBlock23.Name = "e_76";
      textBlock23.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      textBlock23.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock23.VerticalAlignment = VerticalAlignment.Center;
      textBlock23.SetResourceReference(TextBlock.TextProperty, (object) "StoreScreen_Help_Buy");
      TextBlock textBlock24 = new TextBlock();
      grid11.Children.Add((UIElement) textBlock24);
      textBlock24.Name = "e_77";
      textBlock24.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock24.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock24.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock24, 1);
      textBlock24.SetResourceReference(TextBlock.TextProperty, (object) "StoreScreen_Help_Refresh");
      TextBlock textBlock25 = new TextBlock();
      grid11.Children.Add((UIElement) textBlock25);
      textBlock25.Name = "e_78";
      textBlock25.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock25.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock25.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock25, 2);
      textBlock25.SetResourceReference(TextBlock.TextProperty, (object) "StoreScreen_Help_ChangeAmount");
      TextBlock textBlock26 = new TextBlock();
      grid11.Children.Add((UIElement) textBlock26);
      textBlock26.Name = "e_79";
      textBlock26.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock26.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock26.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock26, 3);
      textBlock26.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Back");
      Grid grid12 = new Grid();
      grid1.Children.Add((UIElement) grid12);
      grid12.Name = "CashbackHelp";
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
      ColumnDefinition columnDefinition8 = new ColumnDefinition();
      grid12.ColumnDefinitions.Add(columnDefinition8);
      Grid.SetColumn((UIElement) grid12, 0);
      Grid.SetRow((UIElement) grid12, 2);
      TextBlock textBlock27 = new TextBlock();
      grid12.Children.Add((UIElement) textBlock27);
      textBlock27.Name = "e_80";
      textBlock27.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      textBlock27.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock27.VerticalAlignment = VerticalAlignment.Center;
      textBlock27.SetResourceReference(TextBlock.TextProperty, (object) "StoreScreen_Help_Deposit");
      TextBlock textBlock28 = new TextBlock();
      grid12.Children.Add((UIElement) textBlock28);
      textBlock28.Name = "e_81";
      textBlock28.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock28.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock28.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock28, 1);
      textBlock28.SetResourceReference(TextBlock.TextProperty, (object) "StoreScreen_Help_Withdraw");
      TextBlock textBlock29 = new TextBlock();
      grid12.Children.Add((UIElement) textBlock29);
      textBlock29.Name = "e_82";
      textBlock29.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock29.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock29.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock29, 2);
      textBlock29.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Back");
      Grid grid13 = new Grid();
      grid1.Children.Add((UIElement) grid13);
      grid13.Name = "SelectHelpOffers";
      grid13.Visibility = Visibility.Collapsed;
      grid13.VerticalAlignment = VerticalAlignment.Center;
      grid13.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid13.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition9 = new ColumnDefinition();
      grid13.ColumnDefinitions.Add(columnDefinition9);
      Grid.SetColumn((UIElement) grid13, 0);
      Grid.SetRow((UIElement) grid13, 2);
      TextBlock textBlock30 = new TextBlock();
      grid13.Children.Add((UIElement) textBlock30);
      textBlock30.Name = "e_83";
      textBlock30.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      textBlock30.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock30.VerticalAlignment = VerticalAlignment.Center;
      textBlock30.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Select");
      TextBlock textBlock31 = new TextBlock();
      grid13.Children.Add((UIElement) textBlock31);
      textBlock31.Name = "e_84";
      textBlock31.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock31.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock31.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock31, 1);
      textBlock31.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Back");
      observableCollection.Add((object) tabItem1);
      TabItem tabItem2 = new TabItem();
      tabItem2.Name = "e_85";
      tabItem2.IsTabStop = false;
      tabItem2.SetBinding(UIElement.VisibilityProperty, new Binding("IsSellTabVisible")
      {
        UseGeneratedBindings = true
      });
      tabItem2.SetResourceReference(HeaderedContentControl.HeaderProperty, (object) "StoreScreenSellHeader");
      Grid grid14 = new Grid();
      tabItem2.Content = (object) grid14;
      grid14.Name = "e_86";
      RowDefinition rowDefinition7 = new RowDefinition();
      grid14.RowDefinitions.Add(rowDefinition7);
      grid14.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid14.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(65f, GridUnitType.Pixel)
      });
      grid14.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(2f, GridUnitType.Star)
      });
      grid14.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      Grid grid15 = new Grid();
      grid14.Children.Add((UIElement) grid15);
      grid15.Name = "e_87";
      GamepadBinding gamepadBinding8 = new GamepadBinding();
      gamepadBinding8.Gesture = (InputGesture) new GamepadGesture(GamepadInput.CButton);
      gamepadBinding8.SetBinding(InputBinding.CommandProperty, new Binding("SellCommand")
      {
        UseGeneratedBindings = true
      });
      grid15.InputBindings.Add((InputBinding) gamepadBinding8);
      gamepadBinding8.Parent = (UIElement) grid15;
      GamepadBinding gamepadBinding9 = new GamepadBinding();
      gamepadBinding9.Gesture = (InputGesture) new GamepadGesture(GamepadInput.DButton);
      gamepadBinding9.SetBinding(InputBinding.CommandProperty, new Binding("RefreshCommand")
      {
        UseGeneratedBindings = true
      });
      grid15.InputBindings.Add((InputBinding) gamepadBinding9);
      gamepadBinding9.Parent = (UIElement) grid15;
      RowDefinition rowDefinition8 = new RowDefinition();
      grid15.RowDefinitions.Add(rowDefinition8);
      grid15.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid15.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition10 = new ColumnDefinition();
      grid15.ColumnDefinitions.Add(columnDefinition10);
      DataGrid dataGrid2 = new DataGrid();
      grid15.Children.Add((UIElement) dataGrid2);
      dataGrid2.Name = "e_88";
      GamepadBinding gamepadBinding10 = new GamepadBinding();
      gamepadBinding10.Gesture = (InputGesture) new GamepadGesture(GamepadInput.LeftStickButton);
      gamepadBinding10.SetBinding(InputBinding.CommandProperty, new Binding("SwitchSortOrdersCommand")
      {
        UseGeneratedBindings = true
      });
      dataGrid2.InputBindings.Add((InputBinding) gamepadBinding10);
      gamepadBinding10.Parent = (UIElement) dataGrid2;
      GamepadBinding gamepadBinding11 = new GamepadBinding();
      gamepadBinding11.Gesture = (InputGesture) new GamepadGesture(GamepadInput.LeftShoulderButton);
      gamepadBinding11.SetBinding(InputBinding.CommandProperty, new Binding("PreviousInventoryCommand")
      {
        UseGeneratedBindings = true
      });
      dataGrid2.InputBindings.Add((InputBinding) gamepadBinding11);
      gamepadBinding11.Parent = (UIElement) dataGrid2;
      GamepadBinding gamepadBinding12 = new GamepadBinding();
      gamepadBinding12.Gesture = (InputGesture) new GamepadGesture(GamepadInput.RightShoulderButton);
      gamepadBinding12.SetBinding(InputBinding.CommandProperty, new Binding("NextInventoryCommand")
      {
        UseGeneratedBindings = true
      });
      dataGrid2.InputBindings.Add((InputBinding) gamepadBinding12);
      gamepadBinding12.Parent = (UIElement) dataGrid2;
      dataGrid2.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      dataGrid2.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      dataGrid2.TabIndex = 10;
      dataGrid2.SelectionMode = DataGridSelectionMode.Single;
      dataGrid2.AutoGenerateColumns = false;
      DataGridTemplateColumn gridTemplateColumn5 = new DataGridTemplateColumn();
      gridTemplateColumn5.Width = (DataGridLength) 64f;
      gridTemplateColumn5.SortMemberPath = "Name";
      TextBlock textBlock32 = new TextBlock();
      textBlock32.Name = "e_89";
      textBlock32.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock32.Text = "";
      gridTemplateColumn5.Header = (object) textBlock32;
      Func<UIElement, UIElement> createMethod5 = new Func<UIElement, UIElement>(StoreBlockView_Gamepad.e_88_Col0_ct_dtMethod);
      gridTemplateColumn5.CellTemplate = new DataTemplate(createMethod5);
      dataGrid2.Columns.Add((DataGridColumn) gridTemplateColumn5);
      DataGridTemplateColumn gridTemplateColumn6 = new DataGridTemplateColumn();
      gridTemplateColumn6.Width = new DataGridLength(1f, DataGridLengthUnitType.Star);
      gridTemplateColumn6.SortMemberPath = "Name";
      TextBlock textBlock33 = new TextBlock();
      textBlock33.Name = "e_91";
      textBlock33.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock33.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlock_Column_Name");
      gridTemplateColumn6.Header = (object) textBlock33;
      Func<UIElement, UIElement> createMethod6 = new Func<UIElement, UIElement>(StoreBlockView_Gamepad.e_88_Col1_ct_dtMethod);
      gridTemplateColumn6.CellTemplate = new DataTemplate(createMethod6);
      dataGrid2.Columns.Add((DataGridColumn) gridTemplateColumn6);
      DataGridTemplateColumn gridTemplateColumn7 = new DataGridTemplateColumn();
      gridTemplateColumn7.Width = (DataGridLength) 150f;
      gridTemplateColumn7.SortMemberPath = "Amount";
      TextBlock textBlock34 = new TextBlock();
      textBlock34.Name = "e_93";
      textBlock34.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock34.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlock_Column_Amount");
      gridTemplateColumn7.Header = (object) textBlock34;
      Func<UIElement, UIElement> createMethod7 = new Func<UIElement, UIElement>(StoreBlockView_Gamepad.e_88_Col2_ct_dtMethod);
      gridTemplateColumn7.CellTemplate = new DataTemplate(createMethod7);
      dataGrid2.Columns.Add((DataGridColumn) gridTemplateColumn7);
      DataGridTemplateColumn gridTemplateColumn8 = new DataGridTemplateColumn();
      gridTemplateColumn8.Width = (DataGridLength) 200f;
      gridTemplateColumn8.SortMemberPath = "PricePerUnit";
      TextBlock textBlock35 = new TextBlock();
      textBlock35.Name = "e_95";
      textBlock35.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock35.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlock_Column_PricePerUnit");
      gridTemplateColumn8.Header = (object) textBlock35;
      Func<UIElement, UIElement> createMethod8 = new Func<UIElement, UIElement>(StoreBlockView_Gamepad.e_88_Col3_ct_dtMethod);
      gridTemplateColumn8.CellTemplate = new DataTemplate(createMethod8);
      dataGrid2.Columns.Add((DataGridColumn) gridTemplateColumn8);
      Grid.SetColumnSpan((UIElement) dataGrid2, 2);
      GamepadHelp.SetTargetName((DependencyObject) dataGrid2, "DataGridSellHelp");
      GamepadHelp.SetTabIndexRight((DependencyObject) dataGrid2, 13);
      GamepadHelp.SetTabIndexDown((DependencyObject) dataGrid2, 11);
      dataGrid2.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("OrderedItems")
      {
        UseGeneratedBindings = true
      });
      dataGrid2.SetBinding(Selector.SelectedItemProperty, new Binding("SelectedOrderItem")
      {
        UseGeneratedBindings = true
      });
      dataGrid2.SetBinding(DataGrid.SortingCommandProperty, new Binding("SortingDemandedItemsCommand")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock36 = new TextBlock();
      grid15.Children.Add((UIElement) textBlock36);
      textBlock36.Name = "e_99";
      textBlock36.Margin = new Thickness(0.0f, 5f, 0.0f, 5f);
      textBlock36.VerticalAlignment = VerticalAlignment.Center;
      textBlock36.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock36, 0);
      Grid.SetRow((UIElement) textBlock36, 1);
      textBlock36.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlockView_AmountLabel");
      Grid grid16 = new Grid();
      grid15.Children.Add((UIElement) grid16);
      grid16.Name = "e_100";
      grid16.HorizontalAlignment = HorizontalAlignment.Stretch;
      ColumnDefinition columnDefinition11 = new ColumnDefinition();
      grid16.ColumnDefinitions.Add(columnDefinition11);
      grid16.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(100f, GridUnitType.Pixel)
      });
      grid16.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(200f, GridUnitType.Pixel)
      });
      grid16.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      Grid.SetColumn((UIElement) grid16, 1);
      Grid.SetRow((UIElement) grid16, 1);
      Slider slider3 = new Slider();
      grid16.Children.Add((UIElement) slider3);
      slider3.Name = "e_101";
      slider3.Margin = new Thickness(5f, 5f, 5f, 5f);
      slider3.VerticalAlignment = VerticalAlignment.Center;
      slider3.TabIndex = 11;
      slider3.Minimum = 0.0f;
      slider3.Maximum = 99999f;
      slider3.IsSnapToTickEnabled = true;
      slider3.TickFrequency = 1f;
      Grid.SetColumn((UIElement) slider3, 0);
      GamepadHelp.SetTargetName((DependencyObject) slider3, "AmountSellHelp");
      GamepadHelp.SetTabIndexUp((DependencyObject) slider3, 10);
      slider3.SetBinding(RangeBase.ValueProperty, new Binding("AmountToSell")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock37 = new TextBlock();
      grid16.Children.Add((UIElement) textBlock37);
      textBlock37.Name = "e_102";
      textBlock37.Margin = new Thickness(5f, 5f, 0.0f, 5f);
      textBlock37.VerticalAlignment = VerticalAlignment.Center;
      textBlock37.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock37, 1);
      textBlock37.SetBinding(TextBlock.TextProperty, new Binding("AmountToSell")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock38 = new TextBlock();
      grid16.Children.Add((UIElement) textBlock38);
      textBlock38.Name = "e_103";
      textBlock38.HorizontalAlignment = HorizontalAlignment.Right;
      textBlock38.VerticalAlignment = VerticalAlignment.Center;
      textBlock38.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock38, 2);
      textBlock38.SetBinding(TextBlock.TextProperty, new Binding("TotalPriceToSell")
      {
        UseGeneratedBindings = true
      });
      Image image4 = new Image();
      grid16.Children.Add((UIElement) image4);
      image4.Name = "e_104";
      image4.Width = 20f;
      image4.Margin = new Thickness(4f, 2f, 2f, 2f);
      image4.VerticalAlignment = VerticalAlignment.Center;
      image4.Stretch = Stretch.Uniform;
      Grid.SetColumn((UIElement) image4, 3);
      image4.SetBinding(Image.SourceProperty, new Binding("InventoryTargetViewModel.CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      Grid grid17 = new Grid();
      grid14.Children.Add((UIElement) grid17);
      grid17.Name = "e_105";
      grid17.Margin = new Thickness(10f, 0.0f, 0.0f, 0.0f);
      Grid.SetColumn((UIElement) grid17, 1);
      Grid.SetRow((UIElement) grid17, 0);
      grid17.SetBinding(UIElement.DataContextProperty, new Binding("InventoryTargetViewModel")
      {
        UseGeneratedBindings = true
      });
      Grid grid18 = new Grid();
      grid17.Children.Add((UIElement) grid18);
      grid18.Name = "e_106";
      grid18.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid18.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      RowDefinition rowDefinition9 = new RowDefinition();
      grid18.RowDefinitions.Add(rowDefinition9);
      grid18.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid18.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      TextBlock textBlock39 = new TextBlock();
      grid18.Children.Add((UIElement) textBlock39);
      textBlock39.Name = "e_107";
      textBlock39.Margin = new Thickness(0.0f, 0.0f, 0.0f, 5f);
      textBlock39.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetRow((UIElement) textBlock39, 0);
      textBlock39.SetResourceReference(TextBlock.TextProperty, (object) "StoreScreen_SelectInventory");
      ComboBox comboBox2 = new ComboBox();
      grid18.Children.Add((UIElement) comboBox2);
      comboBox2.Name = "e_108";
      comboBox2.TabIndex = 13;
      comboBox2.MaxDropDownHeight = 300f;
      Grid.SetRow((UIElement) comboBox2, 1);
      GamepadHelp.SetTargetName((DependencyObject) comboBox2, "SelectHelpOrders");
      GamepadHelp.SetTabIndexLeft((DependencyObject) comboBox2, 10);
      GamepadHelp.SetTabIndexDown((DependencyObject) comboBox2, 14);
      comboBox2.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("Inventories")
      {
        UseGeneratedBindings = true
      });
      comboBox2.SetBinding(Selector.SelectedIndexProperty, new Binding("SelectedInventoryIndex")
      {
        UseGeneratedBindings = true
      });
      ListBox listBox2 = new ListBox();
      grid18.Children.Add((UIElement) listBox2);
      listBox2.Name = "e_109";
      listBox2.Margin = new Thickness(0.0f, 4f, 0.0f, 0.0f);
      Style style2 = new Style(typeof (ListBox));
      Setter setter4 = new Setter(UIElement.MinHeightProperty, (object) 80f);
      style2.Setters.Add(setter4);
      ControlTemplate controlTemplate2 = new ControlTemplate(typeof (ListBox), new Func<UIElement, UIElement>(StoreBlockView_Gamepad.e_109_s_S_1_ctMethod));
      Setter setter5 = new Setter(Control.TemplateProperty, (object) controlTemplate2);
      style2.Setters.Add(setter5);
      Trigger trigger2 = new Trigger();
      trigger2.Property = UIElement.IsFocusedProperty;
      trigger2.Value = (object) true;
      Setter setter6 = new Setter(Control.BackgroundProperty, (object) new SolidColorBrush(new ColorW(67, 81, 92, 160)));
      trigger2.Setters.Add(setter6);
      style2.Triggers.Add((TriggerBase) trigger2);
      listBox2.Style = style2;
      listBox2.Background = (Brush) new SolidColorBrush(new ColorW(33, 44, 53, (int) byte.MaxValue));
      listBox2.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      listBox2.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      listBox2.TabIndex = 14;
      Grid.SetRow((UIElement) listBox2, 2);
      GamepadHelp.SetTargetName((DependencyObject) listBox2, "SelectHelpOrders");
      GamepadHelp.SetTabIndexLeft((DependencyObject) listBox2, 10);
      GamepadHelp.SetTabIndexUp((DependencyObject) listBox2, 13);
      listBox2.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("InventoryItems")
      {
        UseGeneratedBindings = true
      });
      StoreBlockView_Gamepad.InitializeElemente_109Resources((UIElement) listBox2);
      Grid grid19 = new Grid();
      grid18.Children.Add((UIElement) grid19);
      grid19.Name = "e_114";
      grid19.Margin = new Thickness(2f, 2f, 2f, 2f);
      grid19.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition12 = new ColumnDefinition();
      grid19.ColumnDefinitions.Add(columnDefinition12);
      Grid.SetRow((UIElement) grid19, 3);
      TextBlock textBlock40 = new TextBlock();
      grid19.Children.Add((UIElement) textBlock40);
      textBlock40.Name = "e_115";
      textBlock40.Margin = new Thickness(0.0f, 5f, 5f, 5f);
      textBlock40.VerticalAlignment = VerticalAlignment.Center;
      textBlock40.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      textBlock40.SetResourceReference(TextBlock.TextProperty, (object) "ScreenTerminalInventory_Volume");
      StackPanel stackPanel3 = new StackPanel();
      grid19.Children.Add((UIElement) stackPanel3);
      stackPanel3.Name = "e_116";
      stackPanel3.Margin = new Thickness(5f, 5f, 0.0f, 5f);
      stackPanel3.HorizontalAlignment = HorizontalAlignment.Right;
      stackPanel3.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel3, 1);
      TextBlock textBlock41 = new TextBlock();
      stackPanel3.Children.Add((UIElement) textBlock41);
      textBlock41.Name = "e_117";
      textBlock41.VerticalAlignment = VerticalAlignment.Center;
      textBlock41.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      textBlock41.SetBinding(TextBlock.TextProperty, new Binding("CurrentInventoryVolume")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock42 = new TextBlock();
      stackPanel3.Children.Add((UIElement) textBlock42);
      textBlock42.Name = "e_118";
      textBlock42.Margin = new Thickness(10f, 0.0f, 10f, 0.0f);
      textBlock42.VerticalAlignment = VerticalAlignment.Center;
      textBlock42.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      textBlock42.Text = "/";
      TextBlock textBlock43 = new TextBlock();
      stackPanel3.Children.Add((UIElement) textBlock43);
      textBlock43.Name = "e_119";
      textBlock43.VerticalAlignment = VerticalAlignment.Center;
      textBlock43.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      textBlock43.SetBinding(TextBlock.TextProperty, new Binding("MaxInventoryVolume")
      {
        UseGeneratedBindings = true
      });
      Grid grid20 = new Grid();
      grid18.Children.Add((UIElement) grid20);
      grid20.Name = "e_120";
      grid20.Margin = new Thickness(2f, 2f, 2f, 2f);
      grid20.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition13 = new ColumnDefinition();
      grid20.ColumnDefinitions.Add(columnDefinition13);
      grid20.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      Grid.SetRow((UIElement) grid20, 4);
      TextBlock textBlock44 = new TextBlock();
      grid20.Children.Add((UIElement) textBlock44);
      textBlock44.Name = "e_121";
      textBlock44.Margin = new Thickness(0.0f, 5f, 5f, 5f);
      textBlock44.VerticalAlignment = VerticalAlignment.Center;
      textBlock44.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      textBlock44.SetResourceReference(TextBlock.TextProperty, (object) "Currency_Default_Account_Label");
      TextBlock textBlock45 = new TextBlock();
      grid20.Children.Add((UIElement) textBlock45);
      textBlock45.Name = "e_122";
      textBlock45.Margin = new Thickness(5f, 5f, 0.0f, 5f);
      textBlock45.HorizontalAlignment = HorizontalAlignment.Right;
      textBlock45.VerticalAlignment = VerticalAlignment.Center;
      textBlock45.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock45, 1);
      textBlock45.SetBinding(TextBlock.TextProperty, new Binding("LocalPlayerCurrency")
      {
        UseGeneratedBindings = true
      });
      Image image5 = new Image();
      grid20.Children.Add((UIElement) image5);
      image5.Name = "e_123";
      image5.Height = 20f;
      image5.Margin = new Thickness(4f, 2f, 2f, 2f);
      image5.VerticalAlignment = VerticalAlignment.Center;
      image5.Stretch = Stretch.Uniform;
      Grid.SetColumn((UIElement) image5, 2);
      image5.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      Border border2 = new Border();
      grid14.Children.Add((UIElement) border2);
      border2.Name = "e_124";
      border2.Height = 2f;
      border2.Margin = new Thickness(0.0f, 10f, 0.0f, 10f);
      border2.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      Grid.SetRow((UIElement) border2, 1);
      Grid.SetColumnSpan((UIElement) border2, 2);
      Grid grid21 = new Grid();
      grid14.Children.Add((UIElement) grid21);
      grid21.Name = "DataGridSellHelp";
      grid21.Visibility = Visibility.Collapsed;
      grid21.VerticalAlignment = VerticalAlignment.Center;
      grid21.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid21.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid21.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid21.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid21.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition14 = new ColumnDefinition();
      grid21.ColumnDefinitions.Add(columnDefinition14);
      Grid.SetRow((UIElement) grid21, 2);
      TextBlock textBlock46 = new TextBlock();
      grid21.Children.Add((UIElement) textBlock46);
      textBlock46.Name = "e_125";
      textBlock46.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      textBlock46.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock46.VerticalAlignment = VerticalAlignment.Center;
      textBlock46.SetResourceReference(TextBlock.TextProperty, (object) "StoreScreen_Help_Sell");
      TextBlock textBlock47 = new TextBlock();
      grid21.Children.Add((UIElement) textBlock47);
      textBlock47.Name = "e_126";
      textBlock47.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock47.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock47.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock47, 1);
      textBlock47.SetResourceReference(TextBlock.TextProperty, (object) "StoreScreen_Help_Refresh");
      StackPanel stackPanel4 = new StackPanel();
      grid21.Children.Add((UIElement) stackPanel4);
      stackPanel4.Name = "e_127";
      stackPanel4.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      stackPanel4.HorizontalAlignment = HorizontalAlignment.Center;
      stackPanel4.VerticalAlignment = VerticalAlignment.Center;
      stackPanel4.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel4, 2);
      TextBlock textBlock48 = new TextBlock();
      stackPanel4.Children.Add((UIElement) textBlock48);
      textBlock48.Name = "e_128";
      textBlock48.SetResourceReference(TextBlock.TextProperty, (object) "StoreScreen_Help_SortBy");
      TextBlock textBlock49 = new TextBlock();
      stackPanel4.Children.Add((UIElement) textBlock49);
      textBlock49.Name = "e_129";
      textBlock49.SetBinding(TextBlock.TextProperty, new Binding("SortModeOrdersText")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock50 = new TextBlock();
      grid21.Children.Add((UIElement) textBlock50);
      textBlock50.Name = "e_130";
      textBlock50.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock50.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock50.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock50, 3);
      textBlock50.SetResourceReference(TextBlock.TextProperty, (object) "StoreScreen_Help_ChangeInventory");
      TextBlock textBlock51 = new TextBlock();
      grid21.Children.Add((UIElement) textBlock51);
      textBlock51.Name = "e_131";
      textBlock51.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock51.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock51.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock51, 4);
      textBlock51.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Back");
      Grid grid22 = new Grid();
      grid14.Children.Add((UIElement) grid22);
      grid22.Name = "AmountSellHelp";
      grid22.Visibility = Visibility.Collapsed;
      grid22.VerticalAlignment = VerticalAlignment.Center;
      grid22.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid22.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid22.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid22.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition15 = new ColumnDefinition();
      grid22.ColumnDefinitions.Add(columnDefinition15);
      Grid.SetRow((UIElement) grid22, 2);
      TextBlock textBlock52 = new TextBlock();
      grid22.Children.Add((UIElement) textBlock52);
      textBlock52.Name = "e_132";
      textBlock52.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      textBlock52.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock52.VerticalAlignment = VerticalAlignment.Center;
      textBlock52.SetResourceReference(TextBlock.TextProperty, (object) "StoreScreen_Help_Sell");
      TextBlock textBlock53 = new TextBlock();
      grid22.Children.Add((UIElement) textBlock53);
      textBlock53.Name = "e_133";
      textBlock53.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock53.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock53.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock53, 1);
      textBlock53.SetResourceReference(TextBlock.TextProperty, (object) "StoreScreen_Help_Refresh");
      TextBlock textBlock54 = new TextBlock();
      grid22.Children.Add((UIElement) textBlock54);
      textBlock54.Name = "e_134";
      textBlock54.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock54.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock54.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock54, 2);
      textBlock54.SetResourceReference(TextBlock.TextProperty, (object) "StoreScreen_Help_ChangeAmount");
      TextBlock textBlock55 = new TextBlock();
      grid22.Children.Add((UIElement) textBlock55);
      textBlock55.Name = "e_135";
      textBlock55.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock55.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock55.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock55, 3);
      textBlock55.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Back");
      Grid grid23 = new Grid();
      grid14.Children.Add((UIElement) grid23);
      grid23.Name = "SelectHelpOrders";
      grid23.Visibility = Visibility.Collapsed;
      grid23.VerticalAlignment = VerticalAlignment.Center;
      grid23.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid23.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition16 = new ColumnDefinition();
      grid23.ColumnDefinitions.Add(columnDefinition16);
      Grid.SetColumn((UIElement) grid23, 0);
      Grid.SetRow((UIElement) grid23, 2);
      TextBlock textBlock56 = new TextBlock();
      grid23.Children.Add((UIElement) textBlock56);
      textBlock56.Name = "e_136";
      textBlock56.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      textBlock56.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock56.VerticalAlignment = VerticalAlignment.Center;
      textBlock56.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Select");
      TextBlock textBlock57 = new TextBlock();
      grid23.Children.Add((UIElement) textBlock57);
      textBlock57.Name = "e_137";
      textBlock57.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock57.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock57.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock57, 1);
      textBlock57.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Back");
      observableCollection.Add((object) tabItem2);
      TabItem tabItem3 = new TabItem();
      tabItem3.Name = "e_138";
      tabItem3.IsTabStop = false;
      tabItem3.SetBinding(UIElement.VisibilityProperty, new Binding("IsAdministrationVisible")
      {
        UseGeneratedBindings = true
      });
      tabItem3.SetResourceReference(HeaderedContentControl.HeaderProperty, (object) "StoreAdministration");
      Grid grid24 = new Grid();
      tabItem3.Content = (object) grid24;
      grid24.Name = "e_139";
      grid24.SetBinding(UIElement.DataContextProperty, new Binding("AdministrationViewModel")
      {
        UseGeneratedBindings = true
      });
      Grid grid25 = new Grid();
      grid24.Children.Add((UIElement) grid25);
      grid25.Name = "e_140";
      grid25.Margin = new Thickness(0.0f, 0.0f, 0.0f, 30f);
      RowDefinition rowDefinition10 = new RowDefinition();
      grid25.RowDefinitions.Add(rowDefinition10);
      grid25.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(65f, GridUnitType.Pixel)
      });
      ColumnDefinition columnDefinition17 = new ColumnDefinition();
      grid25.ColumnDefinitions.Add(columnDefinition17);
      ColumnDefinition columnDefinition18 = new ColumnDefinition();
      grid25.ColumnDefinitions.Add(columnDefinition18);
      ListBox listBox3 = new ListBox();
      grid25.Children.Add((UIElement) listBox3);
      listBox3.Name = "e_141";
      listBox3.Margin = new Thickness(0.0f, 10f, 10f, 0.0f);
      GamepadBinding gamepadBinding13 = new GamepadBinding();
      gamepadBinding13.Gesture = (InputGesture) new GamepadGesture(GamepadInput.CButton);
      gamepadBinding13.SetBinding(InputBinding.CommandProperty, new Binding("RemoveStoreItemCommand")
      {
        UseGeneratedBindings = true
      });
      listBox3.InputBindings.Add((InputBinding) gamepadBinding13);
      gamepadBinding13.Parent = (UIElement) listBox3;
      listBox3.Background = (Brush) new SolidColorBrush(new ColorW(41, 54, 62, (int) byte.MaxValue));
      listBox3.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      listBox3.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      listBox3.TabIndex = 30;
      listBox3.SelectionMode = SelectionMode.Single;
      Grid.SetColumn((UIElement) listBox3, 0);
      GamepadHelp.SetTargetName((DependencyObject) listBox3, "AdminListHelp");
      GamepadHelp.SetTabIndexRight((DependencyObject) listBox3, 32);
      GamepadHelp.SetTabIndexDown((DependencyObject) listBox3, 31);
      listBox3.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("StoreItems")
      {
        UseGeneratedBindings = true
      });
      listBox3.SetBinding(Selector.SelectedItemProperty, new Binding("SelectedStoreItem")
      {
        UseGeneratedBindings = true
      });
      Grid grid26 = new Grid();
      grid25.Children.Add((UIElement) grid26);
      grid26.Name = "AdminListHelp";
      grid26.Visibility = Visibility.Collapsed;
      grid26.VerticalAlignment = VerticalAlignment.Center;
      grid26.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid26.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition19 = new ColumnDefinition();
      grid26.ColumnDefinitions.Add(columnDefinition19);
      Grid.SetRow((UIElement) grid26, 1);
      TextBlock textBlock58 = new TextBlock();
      grid26.Children.Add((UIElement) textBlock58);
      textBlock58.Name = "e_142";
      textBlock58.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      textBlock58.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock58.VerticalAlignment = VerticalAlignment.Center;
      textBlock58.SetResourceReference(TextBlock.TextProperty, (object) "StoreScreenAdmin_Help_Delete");
      TextBlock textBlock59 = new TextBlock();
      grid26.Children.Add((UIElement) textBlock59);
      textBlock59.Name = "e_143";
      textBlock59.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock59.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock59.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock59, 1);
      textBlock59.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Back");
      Grid grid27 = new Grid();
      grid25.Children.Add((UIElement) grid27);
      grid27.Name = "OrderListHelp";
      grid27.Visibility = Visibility.Collapsed;
      grid27.VerticalAlignment = VerticalAlignment.Center;
      grid27.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid27.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid27.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition20 = new ColumnDefinition();
      grid27.ColumnDefinitions.Add(columnDefinition20);
      Grid.SetRow((UIElement) grid27, 1);
      TextBlock textBlock60 = new TextBlock();
      grid27.Children.Add((UIElement) textBlock60);
      textBlock60.Name = "e_144";
      textBlock60.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      textBlock60.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock60.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock60, 1);
      textBlock60.SetResourceReference(TextBlock.TextProperty, (object) "StoreScreenAdmin_Help_CreateOrder");
      TextBlock textBlock61 = new TextBlock();
      grid27.Children.Add((UIElement) textBlock61);
      textBlock61.Name = "e_145";
      textBlock61.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock61.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock61.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock61, 2);
      textBlock61.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Back");
      Grid grid28 = new Grid();
      grid25.Children.Add((UIElement) grid28);
      grid28.Name = "OrderNumericHelp";
      grid28.Visibility = Visibility.Collapsed;
      grid28.VerticalAlignment = VerticalAlignment.Center;
      grid28.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid28.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid28.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition21 = new ColumnDefinition();
      grid28.ColumnDefinitions.Add(columnDefinition21);
      Grid.SetRow((UIElement) grid28, 1);
      TextBlock textBlock62 = new TextBlock();
      grid28.Children.Add((UIElement) textBlock62);
      textBlock62.Name = "e_146";
      textBlock62.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      textBlock62.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock62.VerticalAlignment = VerticalAlignment.Center;
      textBlock62.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_ChangeValue");
      TextBlock textBlock63 = new TextBlock();
      grid28.Children.Add((UIElement) textBlock63);
      textBlock63.Name = "e_147";
      textBlock63.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock63.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock63.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock63, 1);
      textBlock63.SetResourceReference(TextBlock.TextProperty, (object) "StoreScreenAdmin_Help_CreateOrder");
      TextBlock textBlock64 = new TextBlock();
      grid28.Children.Add((UIElement) textBlock64);
      textBlock64.Name = "e_148";
      textBlock64.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock64.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock64.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock64, 2);
      textBlock64.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Back");
      Grid grid29 = new Grid();
      grid25.Children.Add((UIElement) grid29);
      grid29.Name = "OfferListHelp";
      grid29.Visibility = Visibility.Collapsed;
      grid29.VerticalAlignment = VerticalAlignment.Center;
      grid29.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid29.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid29.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition22 = new ColumnDefinition();
      grid29.ColumnDefinitions.Add(columnDefinition22);
      Grid.SetRow((UIElement) grid29, 1);
      TextBlock textBlock65 = new TextBlock();
      grid29.Children.Add((UIElement) textBlock65);
      textBlock65.Name = "e_149";
      textBlock65.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      textBlock65.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock65.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock65, 1);
      textBlock65.SetResourceReference(TextBlock.TextProperty, (object) "StoreScreenAdmin_Help_CreateOffer");
      TextBlock textBlock66 = new TextBlock();
      grid29.Children.Add((UIElement) textBlock66);
      textBlock66.Name = "e_150";
      textBlock66.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock66.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock66.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock66, 2);
      textBlock66.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Back");
      Grid grid30 = new Grid();
      grid25.Children.Add((UIElement) grid30);
      grid30.Name = "OfferNumericHelp";
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
      ColumnDefinition columnDefinition23 = new ColumnDefinition();
      grid30.ColumnDefinitions.Add(columnDefinition23);
      Grid.SetRow((UIElement) grid30, 1);
      TextBlock textBlock67 = new TextBlock();
      grid30.Children.Add((UIElement) textBlock67);
      textBlock67.Name = "e_151";
      textBlock67.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      textBlock67.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock67.VerticalAlignment = VerticalAlignment.Center;
      textBlock67.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_ChangeValue");
      TextBlock textBlock68 = new TextBlock();
      grid30.Children.Add((UIElement) textBlock68);
      textBlock68.Name = "e_152";
      textBlock68.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock68.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock68.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock68, 1);
      textBlock68.SetResourceReference(TextBlock.TextProperty, (object) "StoreScreenAdmin_Help_CreateOffer");
      TextBlock textBlock69 = new TextBlock();
      grid30.Children.Add((UIElement) textBlock69);
      textBlock69.Name = "e_153";
      textBlock69.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      textBlock69.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock69.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock69, 2);
      textBlock69.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Back");
      TabControl tabControl = new TabControl();
      grid25.Children.Add((UIElement) tabControl);
      tabControl.Name = "e_154";
      tabControl.Margin = new Thickness(2f, 2f, 2f, 2f);
      tabControl.TabIndex = 32;
      tabControl.ItemsSource = (IEnumerable) StoreBlockView_Gamepad.Get_e_154_Items();
      Grid.SetColumn((UIElement) tabControl, 1);
      Grid.SetRowSpan((UIElement) tabControl, 2);
      GamepadHelp.SetTabIndexLeft((DependencyObject) tabControl, 30);
      StoreBlockView_Gamepad.InitializeElemente_154Resources((UIElement) tabControl);
      observableCollection.Add((object) tabItem3);
      return observableCollection;
    }

    private static UIElement tt_e_16_Method()
    {
      Grid grid = new Grid();
      grid.Name = "e_17";
      grid.Width = 320f;
      grid.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid.RowDefinitions.Add(new RowDefinition());
      Border border = new Border();
      grid.Children.Add((UIElement) border);
      border.Name = "e_18";
      border.Margin = new Thickness(2f, 2f, 2f, 2f);
      border.Background = (Brush) new SolidColorBrush(new ColorW(41, 54, 62, (int) byte.MaxValue));
      TextBlock textBlock1 = new TextBlock();
      grid.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_19";
      textBlock1.Margin = new Thickness(5f, 5f, 5f, 5f);
      textBlock1.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock1.VerticalAlignment = VerticalAlignment.Center;
      textBlock1.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Binding binding1 = new Binding("Name");
      textBlock1.SetBinding(TextBlock.TextProperty, binding1);
      Image image = new Image();
      grid.Children.Add((UIElement) image);
      image.Name = "e_20";
      image.Margin = new Thickness(2f, 2f, 2f, 2f);
      image.Stretch = Stretch.Uniform;
      Grid.SetRow((UIElement) image, 1);
      Binding binding2 = new Binding("TooltipImage");
      image.SetBinding(Image.SourceProperty, binding2);
      StackPanel stackPanel = new StackPanel();
      grid.Children.Add((UIElement) stackPanel);
      stackPanel.Name = "e_21";
      stackPanel.Margin = new Thickness(5f, 5f, 5f, 5f);
      stackPanel.Orientation = Orientation.Horizontal;
      Grid.SetRow((UIElement) stackPanel, 2);
      TextBlock textBlock2 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock2);
      textBlock2.Name = "e_22";
      textBlock2.VerticalAlignment = VerticalAlignment.Center;
      textBlock2.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      textBlock2.SetResourceReference(TextBlock.TextProperty, (object) "StoreScreen_GridTooltip_Pcu");
      TextBlock textBlock3 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock3);
      textBlock3.Name = "e_23";
      textBlock3.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock3.VerticalAlignment = VerticalAlignment.Center;
      textBlock3.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Binding binding3 = new Binding("PrefabTotalPcu");
      textBlock3.SetBinding(TextBlock.TextProperty, binding3);
      TextBlock textBlock4 = new TextBlock();
      grid.Children.Add((UIElement) textBlock4);
      textBlock4.Name = "e_24";
      textBlock4.Margin = new Thickness(5f, 5f, 5f, 5f);
      textBlock4.TextWrapping = TextWrapping.Wrap;
      Grid.SetRow((UIElement) textBlock4, 3);
      Binding binding4 = new Binding("Description");
      textBlock4.SetBinding(TextBlock.TextProperty, binding4);
      return (UIElement) grid;
    }

    private static UIElement e_12_Col0_ct_dtMethod(UIElement parent)
    {
      Grid grid1 = new Grid();
      grid1.Parent = parent;
      grid1.Name = "e_14";
      grid1.HorizontalAlignment = HorizontalAlignment.Stretch;
      grid1.VerticalAlignment = VerticalAlignment.Stretch;
      Image image = new Image();
      grid1.Children.Add((UIElement) image);
      image.Name = "e_15";
      image.Stretch = Stretch.Uniform;
      Binding binding1 = new Binding("Icon");
      image.SetBinding(Image.SourceProperty, binding1);
      Grid grid2 = new Grid();
      grid1.Children.Add((UIElement) grid2);
      grid2.Name = "e_16";
      grid2.HorizontalAlignment = HorizontalAlignment.Stretch;
      grid2.VerticalAlignment = VerticalAlignment.Stretch;
      ToolTip toolTip = new ToolTip();
      grid2.ToolTip = (object) toolTip;
      toolTip.Content = (object) StoreBlockView_Gamepad.tt_e_16_Method();
      Binding binding2 = new Binding("HasTooltip");
      grid2.SetBinding(UIElement.VisibilityProperty, binding2);
      return (UIElement) grid1;
    }

    private static UIElement e_12_Col1_ct_dtMethod(UIElement parent)
    {
      TextBlock textBlock = new TextBlock();
      textBlock.Parent = parent;
      textBlock.Name = "e_26";
      textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock.VerticalAlignment = VerticalAlignment.Center;
      Binding binding = new Binding("Name");
      textBlock.SetBinding(TextBlock.TextProperty, binding);
      return (UIElement) textBlock;
    }

    private static UIElement e_12_Col2_ct_dtMethod(UIElement parent)
    {
      TextBlock textBlock = new TextBlock();
      textBlock.Parent = parent;
      textBlock.Name = "e_28";
      textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock.HorizontalAlignment = HorizontalAlignment.Right;
      textBlock.VerticalAlignment = VerticalAlignment.Center;
      Binding binding = new Binding("AmountFormatted");
      textBlock.SetBinding(TextBlock.TextProperty, binding);
      return (UIElement) textBlock;
    }

    private static UIElement tt_e_32_Method()
    {
      StackPanel stackPanel = new StackPanel();
      stackPanel.Name = "e_33";
      stackPanel.Orientation = Orientation.Horizontal;
      Binding binding = new Binding("HasPricePerUnitDiscount");
      stackPanel.SetBinding(UIElement.VisibilityProperty, binding);
      TextBlock textBlock1 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_34";
      textBlock1.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock1.VerticalAlignment = VerticalAlignment.Center;
      textBlock1.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlock_OfferDiscount");
      TextBlock textBlock2 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock2);
      textBlock2.Name = "e_35";
      textBlock2.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock2.VerticalAlignment = VerticalAlignment.Center;
      textBlock2.SetBinding(TextBlock.TextProperty, new Binding("PricePerUnitDiscount")
      {
        StringFormat = "{0}%"
      });
      return (UIElement) stackPanel;
    }

    private static UIElement e_12_Col3_ct_dtMethod(UIElement parent)
    {
      StackPanel stackPanel = new StackPanel();
      stackPanel.Parent = parent;
      stackPanel.Name = "e_30";
      stackPanel.Margin = new Thickness(0.0f, 0.0f, 4f, 0.0f);
      stackPanel.HorizontalAlignment = HorizontalAlignment.Right;
      stackPanel.Orientation = Orientation.Horizontal;
      TextBlock textBlock1 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_31";
      textBlock1.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock1.VerticalAlignment = VerticalAlignment.Center;
      Binding binding1 = new Binding("HasNormalPrice");
      textBlock1.SetBinding(UIElement.VisibilityProperty, binding1);
      Binding binding2 = new Binding("PricePerUnitFormatted");
      textBlock1.SetBinding(TextBlock.TextProperty, binding2);
      TextBlock textBlock2 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock2);
      textBlock2.Name = "e_32";
      textBlock2.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock2.VerticalAlignment = VerticalAlignment.Center;
      ToolTip toolTip = new ToolTip();
      textBlock2.ToolTip = (object) toolTip;
      toolTip.Content = (object) StoreBlockView_Gamepad.tt_e_32_Method();
      textBlock2.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 44, 20, (int) byte.MaxValue));
      Binding binding3 = new Binding("HasPricePerUnitDiscount");
      textBlock2.SetBinding(UIElement.VisibilityProperty, binding3);
      Binding binding4 = new Binding("PricePerUnitFormatted");
      textBlock2.SetBinding(TextBlock.TextProperty, binding4);
      Image image = new Image();
      stackPanel.Children.Add((UIElement) image);
      image.Name = "e_36";
      image.Width = 20f;
      image.Margin = new Thickness(2f, 2f, 2f, 2f);
      image.VerticalAlignment = VerticalAlignment.Center;
      image.Stretch = Stretch.Uniform;
      Binding binding5 = new Binding("CurrencyIcon");
      image.SetBinding(Image.SourceProperty, binding5);
      return (UIElement) stackPanel;
    }

    private static UIElement e_53_s_S_1_ctMethod(UIElement parent)
    {
      Border border = new Border();
      border.Parent = parent;
      border.Name = "e_54";
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
      uniformGrid.Name = "e_55";
      uniformGrid.Margin = new Thickness(4f, 4f, 4f, 4f);
      uniformGrid.VerticalAlignment = VerticalAlignment.Top;
      uniformGrid.IsItemsHost = true;
      uniformGrid.Columns = 5;
      return (UIElement) border;
    }

    private static void InitializeElemente_53Resources(UIElement elem)
    {
      Style style = new Style(typeof (ListBoxItem));
      Setter setter1 = new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "GridItem"));
      style.Setters.Add(setter1);
      Setter setter2 = new Setter(Control.IsTabStopProperty, (object) false);
      style.Setters.Add(setter2);
      ControlTemplate controlTemplate = new ControlTemplate(typeof (ListBoxItem), new Func<UIElement, UIElement>(StoreBlockView_Gamepad.e_53r_0_s_S_2_ctMethod));
      Trigger trigger1 = new Trigger();
      trigger1.Property = UIElement.IsMouseOverProperty;
      trigger1.Value = (object) true;
      Setter setter3 = new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "GridItem"));
      trigger1.Setters.Add(setter3);
      controlTemplate.Triggers.Add((TriggerBase) trigger1);
      Trigger trigger2 = new Trigger();
      trigger2.Property = ListBoxItem.IsSelectedProperty;
      trigger2.Value = (object) true;
      Setter setter4 = new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "GridItemHover"));
      trigger2.Setters.Add(setter4);
      controlTemplate.Triggers.Add((TriggerBase) trigger2);
      Setter setter5 = new Setter(Control.TemplateProperty, (object) controlTemplate);
      style.Setters.Add(setter5);
      elem.Resources.Add((object) typeof (ListBoxItem), (object) style);
    }

    private static UIElement e_53r_0_s_S_2_ctMethod(UIElement parent)
    {
      Border border = new Border();
      border.Parent = parent;
      border.Name = "e_56";
      border.SetBinding(Control.BackgroundProperty, new Binding("Background")
      {
        Source = (object) parent
      });
      ContentPresenter contentPresenter = new ContentPresenter();
      border.Child = (UIElement) contentPresenter;
      contentPresenter.Name = "e_57";
      contentPresenter.HorizontalAlignment = HorizontalAlignment.Stretch;
      contentPresenter.VerticalAlignment = VerticalAlignment.Stretch;
      Binding binding = new Binding();
      contentPresenter.SetBinding(ContentPresenter.ContentProperty, binding);
      return (UIElement) border;
    }

    private static UIElement e_88_Col0_ct_dtMethod(UIElement parent)
    {
      Image image = new Image();
      image.Parent = parent;
      image.Name = "e_90";
      image.Stretch = Stretch.Fill;
      Binding binding = new Binding("Icon");
      image.SetBinding(Image.SourceProperty, binding);
      return (UIElement) image;
    }

    private static UIElement e_88_Col1_ct_dtMethod(UIElement parent)
    {
      TextBlock textBlock = new TextBlock();
      textBlock.Parent = parent;
      textBlock.Name = "e_92";
      textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock.VerticalAlignment = VerticalAlignment.Center;
      Binding binding = new Binding("Name");
      textBlock.SetBinding(TextBlock.TextProperty, binding);
      return (UIElement) textBlock;
    }

    private static UIElement e_88_Col2_ct_dtMethod(UIElement parent)
    {
      TextBlock textBlock = new TextBlock();
      textBlock.Parent = parent;
      textBlock.Name = "e_94";
      textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock.HorizontalAlignment = HorizontalAlignment.Right;
      textBlock.VerticalAlignment = VerticalAlignment.Center;
      Binding binding = new Binding("AmountFormatted");
      textBlock.SetBinding(TextBlock.TextProperty, binding);
      return (UIElement) textBlock;
    }

    private static UIElement e_88_Col3_ct_dtMethod(UIElement parent)
    {
      StackPanel stackPanel = new StackPanel();
      stackPanel.Parent = parent;
      stackPanel.Name = "e_96";
      stackPanel.Margin = new Thickness(0.0f, 0.0f, 4f, 0.0f);
      stackPanel.HorizontalAlignment = HorizontalAlignment.Right;
      stackPanel.Orientation = Orientation.Horizontal;
      TextBlock textBlock = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock);
      textBlock.Name = "e_97";
      textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock.VerticalAlignment = VerticalAlignment.Center;
      Binding binding1 = new Binding("PricePerUnitFormatted");
      textBlock.SetBinding(TextBlock.TextProperty, binding1);
      Image image = new Image();
      stackPanel.Children.Add((UIElement) image);
      image.Name = "e_98";
      image.Width = 20f;
      image.Margin = new Thickness(2f, 2f, 2f, 2f);
      image.VerticalAlignment = VerticalAlignment.Center;
      image.Stretch = Stretch.Uniform;
      Binding binding2 = new Binding("CurrencyIcon");
      image.SetBinding(Image.SourceProperty, binding2);
      return (UIElement) stackPanel;
    }

    private static UIElement e_109_s_S_1_ctMethod(UIElement parent)
    {
      Border border = new Border();
      border.Parent = parent;
      border.Name = "e_110";
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
      uniformGrid.Name = "e_111";
      uniformGrid.Margin = new Thickness(4f, 4f, 4f, 4f);
      uniformGrid.VerticalAlignment = VerticalAlignment.Top;
      uniformGrid.IsItemsHost = true;
      uniformGrid.Columns = 5;
      return (UIElement) border;
    }

    private static void InitializeElemente_109Resources(UIElement elem)
    {
      Style style = new Style(typeof (ListBoxItem));
      Setter setter1 = new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "GridItem"));
      style.Setters.Add(setter1);
      Setter setter2 = new Setter(Control.IsTabStopProperty, (object) false);
      style.Setters.Add(setter2);
      ControlTemplate controlTemplate = new ControlTemplate(typeof (ListBoxItem), new Func<UIElement, UIElement>(StoreBlockView_Gamepad.e_109r_0_s_S_2_ctMethod));
      Trigger trigger1 = new Trigger();
      trigger1.Property = UIElement.IsMouseOverProperty;
      trigger1.Value = (object) true;
      Setter setter3 = new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "GridItem"));
      trigger1.Setters.Add(setter3);
      controlTemplate.Triggers.Add((TriggerBase) trigger1);
      Trigger trigger2 = new Trigger();
      trigger2.Property = ListBoxItem.IsSelectedProperty;
      trigger2.Value = (object) true;
      Setter setter4 = new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "GridItemHover"));
      trigger2.Setters.Add(setter4);
      controlTemplate.Triggers.Add((TriggerBase) trigger2);
      Setter setter5 = new Setter(Control.TemplateProperty, (object) controlTemplate);
      style.Setters.Add(setter5);
      elem.Resources.Add((object) typeof (ListBoxItem), (object) style);
    }

    private static UIElement e_109r_0_s_S_2_ctMethod(UIElement parent)
    {
      Border border = new Border();
      border.Parent = parent;
      border.Name = "e_112";
      border.SetBinding(Control.BackgroundProperty, new Binding("Background")
      {
        Source = (object) parent
      });
      ContentPresenter contentPresenter = new ContentPresenter();
      border.Child = (UIElement) contentPresenter;
      contentPresenter.Name = "e_113";
      contentPresenter.HorizontalAlignment = HorizontalAlignment.Stretch;
      contentPresenter.VerticalAlignment = VerticalAlignment.Stretch;
      Binding binding = new Binding();
      contentPresenter.SetBinding(ContentPresenter.ContentProperty, binding);
      return (UIElement) border;
    }

    private static ObservableCollection<object> Get_e_154_Items()
    {
      ObservableCollection<object> observableCollection = new ObservableCollection<object>();
      TabItem tabItem1 = new TabItem();
      tabItem1.Name = "e_155";
      tabItem1.SetResourceReference(HeaderedContentControl.HeaderProperty, (object) "StoreAdministration_NewOffer");
      Grid grid1 = new Grid();
      tabItem1.Content = (object) grid1;
      grid1.Name = "e_156";
      GamepadBinding gamepadBinding1 = new GamepadBinding();
      gamepadBinding1.Gesture = (InputGesture) new GamepadGesture(GamepadInput.CButton);
      gamepadBinding1.SetBinding(InputBinding.CommandProperty, new Binding("CreateOfferCommand")
      {
        UseGeneratedBindings = true
      });
      grid1.InputBindings.Add((InputBinding) gamepadBinding1);
      gamepadBinding1.Parent = (UIElement) grid1;
      RowDefinition rowDefinition1 = new RowDefinition();
      grid1.RowDefinitions.Add(rowDefinition1);
      RowDefinition rowDefinition2 = new RowDefinition();
      grid1.RowDefinitions.Add(rowDefinition2);
      grid1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      ListBox listBox1 = new ListBox();
      grid1.Children.Add((UIElement) listBox1);
      listBox1.Name = "e_157";
      listBox1.Margin = new Thickness(0.0f, 10f, 10f, 10f);
      listBox1.Background = (Brush) new SolidColorBrush(new ColorW(41, 54, 62, (int) byte.MaxValue));
      listBox1.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      listBox1.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      listBox1.TabIndex = 130;
      Grid.SetColumn((UIElement) listBox1, 0);
      GamepadHelp.SetTargetName((DependencyObject) listBox1, "OfferListHelp");
      GamepadHelp.SetTabIndexLeft((DependencyObject) listBox1, 30);
      GamepadHelp.SetTabIndexDown((DependencyObject) listBox1, 131);
      listBox1.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("OrderItems")
      {
        UseGeneratedBindings = true
      });
      listBox1.SetBinding(Selector.SelectedItemProperty, new Binding("SelectedOfferItem")
      {
        UseGeneratedBindings = true
      });
      Grid grid2 = new Grid();
      grid1.Children.Add((UIElement) grid2);
      grid2.Name = "e_158";
      grid2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      RowDefinition rowDefinition3 = new RowDefinition();
      grid2.RowDefinitions.Add(rowDefinition3);
      grid2.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition1 = new ColumnDefinition();
      grid2.ColumnDefinitions.Add(columnDefinition1);
      Grid.SetColumn((UIElement) grid2, 0);
      Grid.SetRow((UIElement) grid2, 1);
      TextBlock textBlock1 = new TextBlock();
      grid2.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_159";
      textBlock1.Margin = new Thickness(5f, 5f, 5f, 5f);
      textBlock1.VerticalAlignment = VerticalAlignment.Center;
      textBlock1.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      textBlock1.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlockView_AmountLabel");
      NumericTextBox numericTextBox1 = new NumericTextBox();
      grid2.Children.Add((UIElement) numericTextBox1);
      numericTextBox1.Name = "e_160";
      numericTextBox1.Margin = new Thickness(5f, 5f, 5f, 5f);
      numericTextBox1.VerticalAlignment = VerticalAlignment.Center;
      numericTextBox1.TabIndex = 131;
      numericTextBox1.MaxLength = 5;
      numericTextBox1.Minimum = 0.0f;
      numericTextBox1.Maximum = 99999f;
      Grid.SetColumn((UIElement) numericTextBox1, 1);
      GamepadHelp.SetTargetName((DependencyObject) numericTextBox1, "OfferNumericHelp");
      GamepadHelp.SetTabIndexLeft((DependencyObject) numericTextBox1, 30);
      GamepadHelp.SetTabIndexUp((DependencyObject) numericTextBox1, 130);
      GamepadHelp.SetTabIndexDown((DependencyObject) numericTextBox1, 132);
      numericTextBox1.SetBinding(NumericTextBox.ValueProperty, new Binding("OfferAmount")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock2 = new TextBlock();
      grid2.Children.Add((UIElement) textBlock2);
      textBlock2.Name = "e_161";
      textBlock2.Margin = new Thickness(5f, 5f, 5f, 5f);
      textBlock2.VerticalAlignment = VerticalAlignment.Center;
      textBlock2.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetRow((UIElement) textBlock2, 1);
      textBlock2.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlockView_PricePerUnitLabel");
      NumericTextBox numericTextBox2 = new NumericTextBox();
      grid2.Children.Add((UIElement) numericTextBox2);
      numericTextBox2.Name = "e_162";
      numericTextBox2.Margin = new Thickness(5f, 5f, 5f, 5f);
      numericTextBox2.VerticalAlignment = VerticalAlignment.Center;
      numericTextBox2.TabIndex = 132;
      numericTextBox2.MaxLength = 9;
      numericTextBox2.Maximum = 1E+09f;
      Grid.SetColumn((UIElement) numericTextBox2, 1);
      Grid.SetRow((UIElement) numericTextBox2, 1);
      GamepadHelp.SetTargetName((DependencyObject) numericTextBox2, "OfferNumericHelp");
      GamepadHelp.SetTabIndexLeft((DependencyObject) numericTextBox2, 30);
      GamepadHelp.SetTabIndexUp((DependencyObject) numericTextBox2, 131);
      GamepadHelp.SetTabIndexDown((DependencyObject) numericTextBox2, 133);
      numericTextBox2.SetBinding(NumericTextBox.MinimumProperty, new Binding("OfferPricePerUnitMinimum")
      {
        UseGeneratedBindings = true
      });
      numericTextBox2.SetBinding(NumericTextBox.ValueProperty, new Binding("OfferPricePerUnit")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock3 = new TextBlock();
      grid2.Children.Add((UIElement) textBlock3);
      textBlock3.Name = "e_163";
      textBlock3.Margin = new Thickness(5f, 5f, 5f, 5f);
      textBlock3.VerticalAlignment = VerticalAlignment.Center;
      textBlock3.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock3, 0);
      Grid.SetRow((UIElement) textBlock3, 2);
      textBlock3.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlockView_TotalPriceLabel");
      StackPanel stackPanel1 = new StackPanel();
      grid2.Children.Add((UIElement) stackPanel1);
      stackPanel1.Name = "e_164";
      stackPanel1.Margin = new Thickness(5f, 5f, 5f, 5f);
      stackPanel1.HorizontalAlignment = HorizontalAlignment.Right;
      stackPanel1.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel1, 1);
      Grid.SetRow((UIElement) stackPanel1, 2);
      TextBlock textBlock4 = new TextBlock();
      stackPanel1.Children.Add((UIElement) textBlock4);
      textBlock4.Name = "e_165";
      textBlock4.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock4.HorizontalAlignment = HorizontalAlignment.Right;
      textBlock4.VerticalAlignment = VerticalAlignment.Center;
      textBlock4.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      textBlock4.SetBinding(TextBlock.TextProperty, new Binding("OfferTotalPrice")
      {
        UseGeneratedBindings = true
      });
      Image image1 = new Image();
      stackPanel1.Children.Add((UIElement) image1);
      image1.Name = "e_166";
      image1.Height = 20f;
      image1.Margin = new Thickness(2f, 2f, 2f, 2f);
      image1.VerticalAlignment = VerticalAlignment.Center;
      image1.Stretch = Stretch.Uniform;
      image1.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock5 = new TextBlock();
      grid2.Children.Add((UIElement) textBlock5);
      textBlock5.Name = "e_167";
      textBlock5.Margin = new Thickness(5f, 5f, 5f, 5f);
      textBlock5.VerticalAlignment = VerticalAlignment.Center;
      textBlock5.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock5, 0);
      Grid.SetRow((UIElement) textBlock5, 3);
      textBlock5.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlockView_ListingFee");
      StackPanel stackPanel2 = new StackPanel();
      grid2.Children.Add((UIElement) stackPanel2);
      stackPanel2.Name = "e_168";
      stackPanel2.Margin = new Thickness(5f, 5f, 5f, 5f);
      stackPanel2.HorizontalAlignment = HorizontalAlignment.Right;
      stackPanel2.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel2, 1);
      Grid.SetRow((UIElement) stackPanel2, 3);
      TextBlock textBlock6 = new TextBlock();
      stackPanel2.Children.Add((UIElement) textBlock6);
      textBlock6.Name = "e_169";
      textBlock6.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock6.HorizontalAlignment = HorizontalAlignment.Right;
      textBlock6.VerticalAlignment = VerticalAlignment.Center;
      textBlock6.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      textBlock6.SetBinding(TextBlock.TextProperty, new Binding("OfferListingFee")
      {
        UseGeneratedBindings = true
      });
      Image image2 = new Image();
      stackPanel2.Children.Add((UIElement) image2);
      image2.Name = "e_170";
      image2.Height = 20f;
      image2.Margin = new Thickness(2f, 2f, 2f, 2f);
      image2.VerticalAlignment = VerticalAlignment.Center;
      image2.Stretch = Stretch.Uniform;
      image2.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock7 = new TextBlock();
      grid2.Children.Add((UIElement) textBlock7);
      textBlock7.Name = "e_171";
      textBlock7.Margin = new Thickness(5f, 5f, 5f, 5f);
      textBlock7.VerticalAlignment = VerticalAlignment.Center;
      textBlock7.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock7, 0);
      Grid.SetRow((UIElement) textBlock7, 4);
      textBlock7.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlockView_TransactionFee");
      StackPanel stackPanel3 = new StackPanel();
      grid2.Children.Add((UIElement) stackPanel3);
      stackPanel3.Name = "e_172";
      stackPanel3.Margin = new Thickness(5f, 5f, 5f, 5f);
      stackPanel3.HorizontalAlignment = HorizontalAlignment.Right;
      stackPanel3.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel3, 1);
      Grid.SetRow((UIElement) stackPanel3, 4);
      TextBlock textBlock8 = new TextBlock();
      stackPanel3.Children.Add((UIElement) textBlock8);
      textBlock8.Name = "e_173";
      textBlock8.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock8.HorizontalAlignment = HorizontalAlignment.Right;
      textBlock8.VerticalAlignment = VerticalAlignment.Center;
      textBlock8.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      textBlock8.SetBinding(TextBlock.TextProperty, new Binding("OfferTransactionFee")
      {
        UseGeneratedBindings = true
      });
      Image image3 = new Image();
      stackPanel3.Children.Add((UIElement) image3);
      image3.Name = "e_174";
      image3.Height = 20f;
      image3.Margin = new Thickness(2f, 2f, 2f, 2f);
      image3.VerticalAlignment = VerticalAlignment.Center;
      image3.Stretch = Stretch.Uniform;
      image3.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      Grid grid3 = new Grid();
      grid2.Children.Add((UIElement) grid3);
      grid3.Name = "e_175";
      grid3.Margin = new Thickness(2f, 2f, 2f, 2f);
      grid3.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition2 = new ColumnDefinition();
      grid3.ColumnDefinitions.Add(columnDefinition2);
      grid3.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      Grid.SetRow((UIElement) grid3, 5);
      Grid.SetColumnSpan((UIElement) grid3, 2);
      TextBlock textBlock9 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock9);
      textBlock9.Name = "e_176";
      textBlock9.Margin = new Thickness(0.0f, 5f, 5f, 5f);
      textBlock9.VerticalAlignment = VerticalAlignment.Bottom;
      textBlock9.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      textBlock9.SetResourceReference(TextBlock.TextProperty, (object) "Currency_Default_Account_Label");
      TextBlock textBlock10 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock10);
      textBlock10.Name = "e_177";
      textBlock10.Margin = new Thickness(5f, 5f, 0.0f, 5f);
      textBlock10.HorizontalAlignment = HorizontalAlignment.Right;
      textBlock10.VerticalAlignment = VerticalAlignment.Bottom;
      textBlock10.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock10, 1);
      textBlock10.SetBinding(TextBlock.TextProperty, new Binding("LocalPlayerCurrency")
      {
        UseGeneratedBindings = true
      });
      Image image4 = new Image();
      grid3.Children.Add((UIElement) image4);
      image4.Name = "e_178";
      image4.Height = 20f;
      image4.Margin = new Thickness(4f, 2f, 2f, 2f);
      image4.VerticalAlignment = VerticalAlignment.Bottom;
      image4.Stretch = Stretch.Uniform;
      Grid.SetColumn((UIElement) image4, 2);
      image4.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      observableCollection.Add((object) tabItem1);
      TabItem tabItem2 = new TabItem();
      tabItem2.Name = "e_179";
      tabItem2.SetBinding(UIElement.VisibilityProperty, new Binding("IsTabNewOrderVisible")
      {
        UseGeneratedBindings = true
      });
      tabItem2.SetResourceReference(HeaderedContentControl.HeaderProperty, (object) "StoreAdministration_NewOrder");
      Grid grid4 = new Grid();
      tabItem2.Content = (object) grid4;
      grid4.Name = "e_180";
      GamepadBinding gamepadBinding2 = new GamepadBinding();
      gamepadBinding2.Gesture = (InputGesture) new GamepadGesture(GamepadInput.CButton);
      gamepadBinding2.SetBinding(InputBinding.CommandProperty, new Binding("CreateOrderCommand")
      {
        UseGeneratedBindings = true
      });
      grid4.InputBindings.Add((InputBinding) gamepadBinding2);
      gamepadBinding2.Parent = (UIElement) grid4;
      RowDefinition rowDefinition4 = new RowDefinition();
      grid4.RowDefinitions.Add(rowDefinition4);
      RowDefinition rowDefinition5 = new RowDefinition();
      grid4.RowDefinitions.Add(rowDefinition5);
      grid4.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid4.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      ListBox listBox2 = new ListBox();
      grid4.Children.Add((UIElement) listBox2);
      listBox2.Name = "e_181";
      listBox2.Margin = new Thickness(0.0f, 10f, 10f, 10f);
      listBox2.Background = (Brush) new SolidColorBrush(new ColorW(41, 54, 62, (int) byte.MaxValue));
      listBox2.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      listBox2.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      listBox2.TabIndex = 230;
      Grid.SetColumn((UIElement) listBox2, 0);
      GamepadHelp.SetTargetName((DependencyObject) listBox2, "OrderListHelp");
      GamepadHelp.SetTabIndexLeft((DependencyObject) listBox2, 30);
      GamepadHelp.SetTabIndexDown((DependencyObject) listBox2, 231);
      listBox2.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("OrderItems")
      {
        UseGeneratedBindings = true
      });
      listBox2.SetBinding(Selector.SelectedItemProperty, new Binding("SelectedOrderItem")
      {
        UseGeneratedBindings = true
      });
      Grid grid5 = new Grid();
      grid4.Children.Add((UIElement) grid5);
      grid5.Name = "e_182";
      grid5.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid5.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid5.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid5.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      RowDefinition rowDefinition6 = new RowDefinition();
      grid5.RowDefinitions.Add(rowDefinition6);
      grid5.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition3 = new ColumnDefinition();
      grid5.ColumnDefinitions.Add(columnDefinition3);
      Grid.SetColumn((UIElement) grid5, 0);
      Grid.SetRow((UIElement) grid5, 1);
      TextBlock textBlock11 = new TextBlock();
      grid5.Children.Add((UIElement) textBlock11);
      textBlock11.Name = "e_183";
      textBlock11.Margin = new Thickness(5f, 5f, 5f, 5f);
      textBlock11.VerticalAlignment = VerticalAlignment.Center;
      textBlock11.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock11, 0);
      textBlock11.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlockView_AmountLabel");
      NumericTextBox numericTextBox3 = new NumericTextBox();
      grid5.Children.Add((UIElement) numericTextBox3);
      numericTextBox3.Name = "e_184";
      numericTextBox3.Margin = new Thickness(5f, 5f, 5f, 5f);
      numericTextBox3.VerticalAlignment = VerticalAlignment.Center;
      numericTextBox3.TabIndex = 231;
      numericTextBox3.MaxLength = 5;
      numericTextBox3.Minimum = 0.0f;
      numericTextBox3.Maximum = 99999f;
      Grid.SetColumn((UIElement) numericTextBox3, 1);
      GamepadHelp.SetTargetName((DependencyObject) numericTextBox3, "OrderNumericHelp");
      GamepadHelp.SetTabIndexLeft((DependencyObject) numericTextBox3, 30);
      GamepadHelp.SetTabIndexUp((DependencyObject) numericTextBox3, 230);
      GamepadHelp.SetTabIndexDown((DependencyObject) numericTextBox3, 232);
      numericTextBox3.SetBinding(NumericTextBox.ValueProperty, new Binding("OrderAmount")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock12 = new TextBlock();
      grid5.Children.Add((UIElement) textBlock12);
      textBlock12.Name = "e_185";
      textBlock12.Margin = new Thickness(5f, 5f, 5f, 5f);
      textBlock12.VerticalAlignment = VerticalAlignment.Center;
      textBlock12.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetRow((UIElement) textBlock12, 1);
      textBlock12.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlockView_PricePerUnitLabel");
      NumericTextBox numericTextBox4 = new NumericTextBox();
      grid5.Children.Add((UIElement) numericTextBox4);
      numericTextBox4.Name = "e_186";
      numericTextBox4.Margin = new Thickness(5f, 5f, 5f, 5f);
      numericTextBox4.VerticalAlignment = VerticalAlignment.Center;
      numericTextBox4.TabIndex = 232;
      numericTextBox4.MaxLength = 9;
      numericTextBox4.Minimum = 0.0f;
      Grid.SetColumn((UIElement) numericTextBox4, 1);
      Grid.SetRow((UIElement) numericTextBox4, 1);
      GamepadHelp.SetTargetName((DependencyObject) numericTextBox4, "OrderNumericHelp");
      GamepadHelp.SetTabIndexLeft((DependencyObject) numericTextBox4, 30);
      GamepadHelp.SetTabIndexUp((DependencyObject) numericTextBox4, 231);
      GamepadHelp.SetTabIndexDown((DependencyObject) numericTextBox4, 233);
      numericTextBox4.SetBinding(NumericTextBox.MaximumProperty, new Binding("OrderPricePerUnitMaximum")
      {
        UseGeneratedBindings = true
      });
      numericTextBox4.SetBinding(NumericTextBox.ValueProperty, new Binding("OrderPricePerUnit")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock13 = new TextBlock();
      grid5.Children.Add((UIElement) textBlock13);
      textBlock13.Name = "e_187";
      textBlock13.Margin = new Thickness(5f, 5f, 5f, 5f);
      textBlock13.VerticalAlignment = VerticalAlignment.Center;
      textBlock13.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock13, 0);
      Grid.SetRow((UIElement) textBlock13, 2);
      textBlock13.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlockView_TotalPriceLabel");
      StackPanel stackPanel4 = new StackPanel();
      grid5.Children.Add((UIElement) stackPanel4);
      stackPanel4.Name = "e_188";
      stackPanel4.Margin = new Thickness(5f, 5f, 5f, 5f);
      stackPanel4.HorizontalAlignment = HorizontalAlignment.Right;
      stackPanel4.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel4, 1);
      Grid.SetRow((UIElement) stackPanel4, 2);
      TextBlock textBlock14 = new TextBlock();
      stackPanel4.Children.Add((UIElement) textBlock14);
      textBlock14.Name = "e_189";
      textBlock14.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock14.HorizontalAlignment = HorizontalAlignment.Right;
      textBlock14.VerticalAlignment = VerticalAlignment.Center;
      textBlock14.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      textBlock14.SetBinding(TextBlock.TextProperty, new Binding("OrderTotalPrice")
      {
        UseGeneratedBindings = true
      });
      Image image5 = new Image();
      stackPanel4.Children.Add((UIElement) image5);
      image5.Name = "e_190";
      image5.Height = 20f;
      image5.Margin = new Thickness(2f, 2f, 2f, 2f);
      image5.VerticalAlignment = VerticalAlignment.Center;
      image5.Stretch = Stretch.Uniform;
      image5.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock15 = new TextBlock();
      grid5.Children.Add((UIElement) textBlock15);
      textBlock15.Name = "e_191";
      textBlock15.Margin = new Thickness(5f, 5f, 5f, 5f);
      textBlock15.VerticalAlignment = VerticalAlignment.Center;
      textBlock15.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock15, 0);
      Grid.SetRow((UIElement) textBlock15, 3);
      textBlock15.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlockView_ListingFee");
      StackPanel stackPanel5 = new StackPanel();
      grid5.Children.Add((UIElement) stackPanel5);
      stackPanel5.Name = "e_192";
      stackPanel5.Margin = new Thickness(5f, 5f, 5f, 5f);
      stackPanel5.HorizontalAlignment = HorizontalAlignment.Right;
      stackPanel5.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel5, 1);
      Grid.SetRow((UIElement) stackPanel5, 3);
      TextBlock textBlock16 = new TextBlock();
      stackPanel5.Children.Add((UIElement) textBlock16);
      textBlock16.Name = "e_193";
      textBlock16.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock16.HorizontalAlignment = HorizontalAlignment.Right;
      textBlock16.VerticalAlignment = VerticalAlignment.Center;
      textBlock16.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      textBlock16.SetBinding(TextBlock.TextProperty, new Binding("OrderListingFee")
      {
        UseGeneratedBindings = true
      });
      Image image6 = new Image();
      stackPanel5.Children.Add((UIElement) image6);
      image6.Name = "e_194";
      image6.Height = 20f;
      image6.Margin = new Thickness(2f, 2f, 2f, 2f);
      image6.VerticalAlignment = VerticalAlignment.Center;
      image6.Stretch = Stretch.Uniform;
      image6.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      Grid grid6 = new Grid();
      grid5.Children.Add((UIElement) grid6);
      grid6.Name = "e_195";
      grid6.Margin = new Thickness(2f, 2f, 2f, 2f);
      grid6.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition4 = new ColumnDefinition();
      grid6.ColumnDefinitions.Add(columnDefinition4);
      grid6.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      Grid.SetRow((UIElement) grid6, 4);
      Grid.SetColumnSpan((UIElement) grid6, 2);
      TextBlock textBlock17 = new TextBlock();
      grid6.Children.Add((UIElement) textBlock17);
      textBlock17.Name = "e_196";
      textBlock17.Margin = new Thickness(0.0f, 5f, 5f, 5f);
      textBlock17.VerticalAlignment = VerticalAlignment.Bottom;
      textBlock17.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      textBlock17.SetResourceReference(TextBlock.TextProperty, (object) "Currency_Default_Account_Label");
      TextBlock textBlock18 = new TextBlock();
      grid6.Children.Add((UIElement) textBlock18);
      textBlock18.Name = "e_197";
      textBlock18.Margin = new Thickness(5f, 5f, 0.0f, 5f);
      textBlock18.HorizontalAlignment = HorizontalAlignment.Right;
      textBlock18.VerticalAlignment = VerticalAlignment.Bottom;
      textBlock18.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock18, 1);
      textBlock18.SetBinding(TextBlock.TextProperty, new Binding("LocalPlayerCurrency")
      {
        UseGeneratedBindings = true
      });
      Image image7 = new Image();
      grid6.Children.Add((UIElement) image7);
      image7.Name = "e_198";
      image7.Height = 20f;
      image7.Margin = new Thickness(4f, 2f, 2f, 2f);
      image7.VerticalAlignment = VerticalAlignment.Bottom;
      image7.Stretch = Stretch.Uniform;
      Grid.SetColumn((UIElement) image7, 2);
      image7.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      observableCollection.Add((object) tabItem2);
      return observableCollection;
    }

    private static void InitializeElemente_154Resources(UIElement elem)
    {
      Style style = new Style(typeof (TabControl), elem.Resources[(object) typeof (TabControl)] as Style);
      Setter setter1 = new Setter(Control.BorderThicknessProperty, (object) new Thickness(0.0f));
      style.Setters.Add(setter1);
      Setter setter2 = new Setter(Control.PaddingProperty, (object) new Thickness(0.0f));
      style.Setters.Add(setter2);
      Setter setter3 = new Setter(Control.BorderBrushProperty, (object) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 0)));
      style.Setters.Add(setter3);
      elem.Resources.Add((object) typeof (TabControl), (object) style);
    }
  }
}
