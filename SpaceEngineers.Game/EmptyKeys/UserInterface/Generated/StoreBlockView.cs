// Decompiled with JetBrains decompiler
// Type: EmptyKeys.UserInterface.Generated.StoreBlockView
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Controls.Primitives;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.StoreBlockView_Bindings;
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
  public class StoreBlockView : UIRoot
  {
    private Grid rootGrid;
    private Image e_0;
    private Grid e_1;
    private ImageButton e_2;
    private StackPanel e_3;
    private TextBlock e_4;
    private Border e_5;
    private TabControl e_6;

    public StoreBlockView() => this.Initialize();

    public StoreBlockView(int width, int height)
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
      StoreBlockView.InitializeElementResources((UIElement) this);
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
      this.e_6 = new TabControl();
      this.e_1.Children.Add((UIElement) this.e_6);
      this.e_6.Name = "e_6";
      this.e_6.TabIndex = 0;
      this.e_6.ItemsSource = (IEnumerable) StoreBlockView.Get_e_6_Items();
      Grid.SetColumn((UIElement) this.e_6, 1);
      Grid.SetRow((UIElement) this.e_6, 3);
      this.e_6.SetBinding(Selector.SelectedIndexProperty, new Binding("TabSelectedIndex")
      {
        UseGeneratedBindings = true
      });
      ImageManager.Instance.AddImage("Textures\\GUI\\Screens\\screen_background.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol_highlight.dds");
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "MaxWidth", typeof (MyStoreBlockViewModel_MaxWidth_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "BackgroundOverlay", typeof (MyStoreBlockViewModel_BackgroundOverlay_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "ExitCommand", typeof (MyStoreBlockViewModel_ExitCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "IsBuyTabVisible", typeof (MyStoreBlockViewModel_IsBuyTabVisible_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "BuyCommand", typeof (MyStoreBlockViewModel_BuyCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "RefreshCommand", typeof (MyStoreBlockViewModel_RefreshCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "OfferedItems", typeof (MyStoreBlockViewModel_OfferedItems_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "SelectedOfferItem", typeof (MyStoreBlockViewModel_SelectedOfferItem_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "SortingOfferedItemsCommand", typeof (MyStoreBlockViewModel_SortingOfferedItemsCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "InventoryTargetViewModel", typeof (MyStoreBlockViewModel_InventoryTargetViewModel_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryTargetViewModel), "Inventories", typeof (MyInventoryTargetViewModel_Inventories_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryTargetViewModel), "SelectedInventoryIndex", typeof (MyInventoryTargetViewModel_SelectedInventoryIndex_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryTargetViewModel), "InventoryItems", typeof (MyInventoryTargetViewModel_InventoryItems_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryTargetViewModel), "CurrentInventoryVolume", typeof (MyInventoryTargetViewModel_CurrentInventoryVolume_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryTargetViewModel), "MaxInventoryVolume", typeof (MyInventoryTargetViewModel_MaxInventoryVolume_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryTargetViewModel), "LocalPlayerCurrency", typeof (MyInventoryTargetViewModel_LocalPlayerCurrency_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryTargetViewModel), "CurrencyIcon", typeof (MyInventoryTargetViewModel_CurrencyIcon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryTargetViewModel), "BalanceChangeValue", typeof (MyInventoryTargetViewModel_BalanceChangeValue_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryTargetViewModel), "DepositCommand", typeof (MyInventoryTargetViewModel_DepositCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryTargetViewModel), "WithdrawCommand", typeof (MyInventoryTargetViewModel_WithdrawCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "IsPreviewEnabled", typeof (MyStoreBlockViewModel_IsPreviewEnabled_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "ShowPreviewCommand", typeof (MyStoreBlockViewModel_ShowPreviewCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "AmountToBuyMaximum", typeof (MyStoreBlockViewModel_AmountToBuyMaximum_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "AmountToBuy", typeof (MyStoreBlockViewModel_AmountToBuy_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "SetAllAmountOfferCommand", typeof (MyStoreBlockViewModel_SetAllAmountOfferCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "TotalPriceToBuy", typeof (MyStoreBlockViewModel_TotalPriceToBuy_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "IsBuyEnabled", typeof (MyStoreBlockViewModel_IsBuyEnabled_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "IsJoystickLastUsed", typeof (MyStoreBlockViewModel_IsJoystickLastUsed_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "IsRefreshEnabled", typeof (MyStoreBlockViewModel_IsRefreshEnabled_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "IsSellTabVisible", typeof (MyStoreBlockViewModel_IsSellTabVisible_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "OrderedItems", typeof (MyStoreBlockViewModel_OrderedItems_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "SelectedOrderItem", typeof (MyStoreBlockViewModel_SelectedOrderItem_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "SortingDemandedItemsCommand", typeof (MyStoreBlockViewModel_SortingDemandedItemsCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "AmountToSell", typeof (MyStoreBlockViewModel_AmountToSell_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "SetAllAmountOrderCommand", typeof (MyStoreBlockViewModel_SetAllAmountOrderCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "TotalPriceToSell", typeof (MyStoreBlockViewModel_TotalPriceToSell_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "IsSellEnabled", typeof (MyStoreBlockViewModel_IsSellEnabled_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "SellCommand", typeof (MyStoreBlockViewModel_SellCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "IsAdministrationVisible", typeof (MyStoreBlockViewModel_IsAdministrationVisible_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "AdministrationViewModel", typeof (MyStoreBlockViewModel_AdministrationViewModel_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "StoreItems", typeof (MyStoreBlockAdministrationViewModel_StoreItems_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "SelectedStoreItem", typeof (MyStoreBlockAdministrationViewModel_SelectedStoreItem_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "RemoveStoreItemCommand", typeof (MyStoreBlockAdministrationViewModel_RemoveStoreItemCommand_PropertyInfo));
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
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "IsCreateOfferEnabled", typeof (MyStoreBlockAdministrationViewModel_IsCreateOfferEnabled_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "CreateOfferCommand", typeof (MyStoreBlockAdministrationViewModel_CreateOfferCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "IsTabNewOrderVisible", typeof (MyStoreBlockAdministrationViewModel_IsTabNewOrderVisible_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "SelectedOrderItem", typeof (MyStoreBlockAdministrationViewModel_SelectedOrderItem_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "OrderAmount", typeof (MyStoreBlockAdministrationViewModel_OrderAmount_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "OrderPricePerUnitMaximum", typeof (MyStoreBlockAdministrationViewModel_OrderPricePerUnitMaximum_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "OrderPricePerUnit", typeof (MyStoreBlockAdministrationViewModel_OrderPricePerUnit_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "OrderTotalPrice", typeof (MyStoreBlockAdministrationViewModel_OrderTotalPrice_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "OrderListingFee", typeof (MyStoreBlockAdministrationViewModel_OrderListingFee_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "IsCreateOrderEnabled", typeof (MyStoreBlockAdministrationViewModel_IsCreateOrderEnabled_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockAdministrationViewModel), "CreateOrderCommand", typeof (MyStoreBlockAdministrationViewModel_CreateOrderCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreBlockViewModel), "TabSelectedIndex", typeof (MyStoreBlockViewModel_TabSelectedIndex_PropertyInfo));
    }

    private static void InitializeElementResources(UIElement elem)
    {
      elem.Resources.MergedDictionaries.Add((ResourceDictionary) Styles.Instance);
      elem.Resources.MergedDictionaries.Add((ResourceDictionary) DataTemplatesStoreBlock.Instance);
    }

    private static ObservableCollection<object> Get_e_6_Items()
    {
      ObservableCollection<object> observableCollection = new ObservableCollection<object>();
      TabItem tabItem1 = new TabItem();
      tabItem1.Name = "e_7";
      tabItem1.SetBinding(UIElement.VisibilityProperty, new Binding("IsBuyTabVisible")
      {
        UseGeneratedBindings = true
      });
      tabItem1.SetResourceReference(HeaderedContentControl.HeaderProperty, (object) "StoreScreenBuyHeader");
      Grid grid1 = new Grid();
      tabItem1.Content = (object) grid1;
      grid1.Name = "e_8";
      GamepadBinding gamepadBinding1 = new GamepadBinding();
      gamepadBinding1.Gesture = (InputGesture) new GamepadGesture(GamepadInput.CButton);
      gamepadBinding1.SetBinding(InputBinding.CommandProperty, new Binding("BuyCommand")
      {
        UseGeneratedBindings = true
      });
      grid1.InputBindings.Add((InputBinding) gamepadBinding1);
      gamepadBinding1.Parent = (UIElement) grid1;
      GamepadBinding gamepadBinding2 = new GamepadBinding();
      gamepadBinding2.Gesture = (InputGesture) new GamepadGesture(GamepadInput.DButton);
      gamepadBinding2.SetBinding(InputBinding.CommandProperty, new Binding("RefreshCommand")
      {
        UseGeneratedBindings = true
      });
      grid1.InputBindings.Add((InputBinding) gamepadBinding2);
      gamepadBinding2.Parent = (UIElement) grid1;
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
        Width = new GridLength(1f, GridUnitType.Star)
      });
      DataGrid dataGrid1 = new DataGrid();
      grid1.Children.Add((UIElement) dataGrid1);
      dataGrid1.Name = "e_9";
      dataGrid1.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      dataGrid1.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      dataGrid1.TabIndex = 1;
      dataGrid1.SelectionMode = DataGridSelectionMode.Single;
      dataGrid1.AutoGenerateColumns = false;
      DataGridTemplateColumn gridTemplateColumn1 = new DataGridTemplateColumn();
      gridTemplateColumn1.Width = (DataGridLength) 64f;
      gridTemplateColumn1.SortMemberPath = "Name";
      TextBlock textBlock1 = new TextBlock();
      textBlock1.Name = "e_10";
      textBlock1.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock1.Text = "";
      gridTemplateColumn1.Header = (object) textBlock1;
      Func<UIElement, UIElement> createMethod1 = new Func<UIElement, UIElement>(StoreBlockView.e_9_Col0_ct_dtMethod);
      gridTemplateColumn1.CellTemplate = new DataTemplate(createMethod1);
      dataGrid1.Columns.Add((DataGridColumn) gridTemplateColumn1);
      DataGridTemplateColumn gridTemplateColumn2 = new DataGridTemplateColumn();
      gridTemplateColumn2.Width = new DataGridLength(1f, DataGridLengthUnitType.Star);
      gridTemplateColumn2.SortMemberPath = "Name";
      TextBlock textBlock2 = new TextBlock();
      textBlock2.Name = "e_22";
      textBlock2.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock2.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlock_Column_Name");
      gridTemplateColumn2.Header = (object) textBlock2;
      Func<UIElement, UIElement> createMethod2 = new Func<UIElement, UIElement>(StoreBlockView.e_9_Col1_ct_dtMethod);
      gridTemplateColumn2.CellTemplate = new DataTemplate(createMethod2);
      dataGrid1.Columns.Add((DataGridColumn) gridTemplateColumn2);
      DataGridTemplateColumn gridTemplateColumn3 = new DataGridTemplateColumn();
      gridTemplateColumn3.Width = (DataGridLength) 150f;
      gridTemplateColumn3.SortMemberPath = "Amount";
      TextBlock textBlock3 = new TextBlock();
      textBlock3.Name = "e_24";
      textBlock3.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock3.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlock_Column_Amount");
      gridTemplateColumn3.Header = (object) textBlock3;
      Func<UIElement, UIElement> createMethod3 = new Func<UIElement, UIElement>(StoreBlockView.e_9_Col2_ct_dtMethod);
      gridTemplateColumn3.CellTemplate = new DataTemplate(createMethod3);
      dataGrid1.Columns.Add((DataGridColumn) gridTemplateColumn3);
      DataGridTemplateColumn gridTemplateColumn4 = new DataGridTemplateColumn();
      gridTemplateColumn4.Width = (DataGridLength) 200f;
      gridTemplateColumn4.SortMemberPath = "PricePerUnit";
      TextBlock textBlock4 = new TextBlock();
      textBlock4.Name = "e_26";
      textBlock4.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock4.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlock_Column_PricePerUnit");
      gridTemplateColumn4.Header = (object) textBlock4;
      Func<UIElement, UIElement> createMethod4 = new Func<UIElement, UIElement>(StoreBlockView.e_9_Col3_ct_dtMethod);
      gridTemplateColumn4.CellTemplate = new DataTemplate(createMethod4);
      dataGrid1.Columns.Add((DataGridColumn) gridTemplateColumn4);
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
      StoreBlockView.InitializeElemente_9Resources((UIElement) dataGrid1);
      Grid grid2 = new Grid();
      grid1.Children.Add((UIElement) grid2);
      grid2.Name = "e_34";
      grid2.Margin = new Thickness(10f, 0.0f, 0.0f, 0.0f);
      Grid.SetColumn((UIElement) grid2, 1);
      Grid.SetRow((UIElement) grid2, 0);
      grid2.SetBinding(UIElement.DataContextProperty, new Binding("InventoryTargetViewModel")
      {
        UseGeneratedBindings = true
      });
      Grid grid3 = new Grid();
      grid2.Children.Add((UIElement) grid3);
      grid3.Name = "e_35";
      grid3.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid3.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      RowDefinition rowDefinition2 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition2);
      grid3.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid3.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      TextBlock textBlock5 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock5);
      textBlock5.Name = "e_36";
      textBlock5.Margin = new Thickness(0.0f, 0.0f, 0.0f, 5f);
      textBlock5.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetRow((UIElement) textBlock5, 0);
      textBlock5.SetResourceReference(TextBlock.TextProperty, (object) "StoreScreen_SelectInventory");
      ComboBox comboBox1 = new ComboBox();
      grid3.Children.Add((UIElement) comboBox1);
      comboBox1.Name = "e_37";
      comboBox1.TabIndex = 4;
      comboBox1.MaxDropDownHeight = 300f;
      Grid.SetRow((UIElement) comboBox1, 1);
      comboBox1.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("Inventories")
      {
        UseGeneratedBindings = true
      });
      comboBox1.SetBinding(Selector.SelectedIndexProperty, new Binding("SelectedInventoryIndex")
      {
        UseGeneratedBindings = true
      });
      ListBox listBox1 = new ListBox();
      grid3.Children.Add((UIElement) listBox1);
      listBox1.Name = "e_38";
      listBox1.Margin = new Thickness(0.0f, 4f, 0.0f, 0.0f);
      Style style1 = new Style(typeof (ListBox));
      Setter setter1 = new Setter(UIElement.MinHeightProperty, (object) 80f);
      style1.Setters.Add(setter1);
      ControlTemplate controlTemplate1 = new ControlTemplate(typeof (ListBox), new Func<UIElement, UIElement>(StoreBlockView.e_38_s_S_1_ctMethod));
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
      DragDrop.SetIsDropTarget((UIElement) listBox1, true);
      listBox1.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("InventoryItems")
      {
        UseGeneratedBindings = true
      });
      StoreBlockView.InitializeElemente_38Resources((UIElement) listBox1);
      Grid grid4 = new Grid();
      grid3.Children.Add((UIElement) grid4);
      grid4.Name = "e_43";
      grid4.Margin = new Thickness(2f, 2f, 2f, 2f);
      grid4.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition1 = new ColumnDefinition();
      grid4.ColumnDefinitions.Add(columnDefinition1);
      Grid.SetRow((UIElement) grid4, 3);
      TextBlock textBlock6 = new TextBlock();
      grid4.Children.Add((UIElement) textBlock6);
      textBlock6.Name = "e_44";
      textBlock6.Margin = new Thickness(0.0f, 5f, 5f, 5f);
      textBlock6.VerticalAlignment = VerticalAlignment.Center;
      textBlock6.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      textBlock6.SetResourceReference(TextBlock.TextProperty, (object) "ScreenTerminalInventory_Volume");
      StackPanel stackPanel1 = new StackPanel();
      grid4.Children.Add((UIElement) stackPanel1);
      stackPanel1.Name = "e_45";
      stackPanel1.Margin = new Thickness(5f, 5f, 0.0f, 5f);
      stackPanel1.HorizontalAlignment = HorizontalAlignment.Right;
      stackPanel1.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel1, 1);
      TextBlock textBlock7 = new TextBlock();
      stackPanel1.Children.Add((UIElement) textBlock7);
      textBlock7.Name = "e_46";
      textBlock7.VerticalAlignment = VerticalAlignment.Center;
      textBlock7.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      textBlock7.SetBinding(TextBlock.TextProperty, new Binding("CurrentInventoryVolume")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock8 = new TextBlock();
      stackPanel1.Children.Add((UIElement) textBlock8);
      textBlock8.Name = "e_47";
      textBlock8.Margin = new Thickness(10f, 0.0f, 10f, 0.0f);
      textBlock8.VerticalAlignment = VerticalAlignment.Center;
      textBlock8.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      textBlock8.Text = "/";
      TextBlock textBlock9 = new TextBlock();
      stackPanel1.Children.Add((UIElement) textBlock9);
      textBlock9.Name = "e_48";
      textBlock9.VerticalAlignment = VerticalAlignment.Center;
      textBlock9.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      textBlock9.SetBinding(TextBlock.TextProperty, new Binding("MaxInventoryVolume")
      {
        UseGeneratedBindings = true
      });
      Grid grid5 = new Grid();
      grid3.Children.Add((UIElement) grid5);
      grid5.Name = "e_49";
      grid5.Margin = new Thickness(2f, 2f, 2f, 2f);
      RowDefinition rowDefinition3 = new RowDefinition();
      grid5.RowDefinitions.Add(rowDefinition3);
      RowDefinition rowDefinition4 = new RowDefinition();
      grid5.RowDefinitions.Add(rowDefinition4);
      RowDefinition rowDefinition5 = new RowDefinition();
      grid5.RowDefinitions.Add(rowDefinition5);
      grid5.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition2 = new ColumnDefinition();
      grid5.ColumnDefinitions.Add(columnDefinition2);
      grid5.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      Grid.SetRow((UIElement) grid5, 4);
      TextBlock textBlock10 = new TextBlock();
      grid5.Children.Add((UIElement) textBlock10);
      textBlock10.Name = "e_50";
      textBlock10.Margin = new Thickness(0.0f, 5f, 5f, 5f);
      textBlock10.VerticalAlignment = VerticalAlignment.Center;
      textBlock10.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      textBlock10.SetResourceReference(TextBlock.TextProperty, (object) "Currency_Default_Account_Label");
      TextBlock textBlock11 = new TextBlock();
      grid5.Children.Add((UIElement) textBlock11);
      textBlock11.Name = "e_51";
      textBlock11.Margin = new Thickness(5f, 5f, 0.0f, 5f);
      textBlock11.HorizontalAlignment = HorizontalAlignment.Right;
      textBlock11.VerticalAlignment = VerticalAlignment.Center;
      textBlock11.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock11, 1);
      textBlock11.SetBinding(TextBlock.TextProperty, new Binding("LocalPlayerCurrency")
      {
        UseGeneratedBindings = true
      });
      Image image1 = new Image();
      grid5.Children.Add((UIElement) image1);
      image1.Name = "e_52";
      image1.Height = 20f;
      image1.Margin = new Thickness(4f, 2f, 2f, 2f);
      image1.VerticalAlignment = VerticalAlignment.Center;
      image1.Stretch = Stretch.Uniform;
      Grid.SetColumn((UIElement) image1, 2);
      image1.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock12 = new TextBlock();
      grid5.Children.Add((UIElement) textBlock12);
      textBlock12.Name = "e_53";
      textBlock12.Margin = new Thickness(0.0f, 5f, 5f, 5f);
      textBlock12.VerticalAlignment = VerticalAlignment.Center;
      textBlock12.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetRow((UIElement) textBlock12, 1);
      textBlock12.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlockView_CashBack");
      NumericTextBox numericTextBox1 = new NumericTextBox();
      grid5.Children.Add((UIElement) numericTextBox1);
      numericTextBox1.Name = "e_54";
      numericTextBox1.Margin = new Thickness(5f, 5f, 5f, 5f);
      numericTextBox1.VerticalAlignment = VerticalAlignment.Center;
      numericTextBox1.TabIndex = 6;
      numericTextBox1.MaxLength = 7;
      numericTextBox1.Minimum = 0.0f;
      numericTextBox1.Maximum = 1000000f;
      Grid.SetColumn((UIElement) numericTextBox1, 1);
      Grid.SetRow((UIElement) numericTextBox1, 1);
      numericTextBox1.SetBinding(NumericTextBox.ValueProperty, new Binding("BalanceChangeValue")
      {
        UseGeneratedBindings = true
      });
      Image image2 = new Image();
      grid5.Children.Add((UIElement) image2);
      image2.Name = "e_55";
      image2.Height = 20f;
      image2.Margin = new Thickness(4f, 2f, 2f, 2f);
      image2.VerticalAlignment = VerticalAlignment.Center;
      image2.Stretch = Stretch.Uniform;
      Grid.SetColumn((UIElement) image2, 2);
      Grid.SetRow((UIElement) image2, 1);
      image2.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      Grid grid6 = new Grid();
      grid5.Children.Add((UIElement) grid6);
      grid6.Name = "e_56";
      ColumnDefinition columnDefinition3 = new ColumnDefinition();
      grid6.ColumnDefinitions.Add(columnDefinition3);
      ColumnDefinition columnDefinition4 = new ColumnDefinition();
      grid6.ColumnDefinitions.Add(columnDefinition4);
      Grid.SetRow((UIElement) grid6, 2);
      Grid.SetColumnSpan((UIElement) grid6, 3);
      Button button1 = new Button();
      grid6.Children.Add((UIElement) button1);
      button1.Name = "e_57";
      button1.Margin = new Thickness(0.0f, 5f, 5f, 5f);
      button1.VerticalAlignment = VerticalAlignment.Center;
      button1.TabIndex = 7;
      button1.SetBinding(Button.CommandProperty, new Binding("DepositCommand")
      {
        UseGeneratedBindings = true
      });
      button1.SetResourceReference(ContentControl.ContentProperty, (object) "FactionTerminal_Deposit_Currency");
      Button button2 = new Button();
      grid6.Children.Add((UIElement) button2);
      button2.Name = "e_58";
      button2.Margin = new Thickness(5f, 5f, 0.0f, 5f);
      button2.VerticalAlignment = VerticalAlignment.Center;
      button2.TabIndex = 8;
      Grid.SetColumn((UIElement) button2, 1);
      button2.SetBinding(Button.CommandProperty, new Binding("WithdrawCommand")
      {
        UseGeneratedBindings = true
      });
      button2.SetResourceReference(ContentControl.ContentProperty, (object) "FactionTerminal_Withdraw_Currency");
      Border border1 = new Border();
      grid1.Children.Add((UIElement) border1);
      border1.Name = "e_59";
      border1.Height = 2f;
      border1.Margin = new Thickness(0.0f, 10f, 0.0f, 10f);
      border1.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      Grid.SetRow((UIElement) border1, 1);
      Grid.SetColumnSpan((UIElement) border1, 2);
      Grid grid7 = new Grid();
      grid1.Children.Add((UIElement) grid7);
      grid7.Name = "e_60";
      grid7.Margin = new Thickness(0.0f, 0.0f, 0.0f, 30f);
      ColumnDefinition columnDefinition5 = new ColumnDefinition();
      grid7.ColumnDefinitions.Add(columnDefinition5);
      grid7.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid7.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(200f, GridUnitType.Pixel)
      });
      grid7.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(150f, GridUnitType.Pixel)
      });
      grid7.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(150f, GridUnitType.Pixel)
      });
      Grid.SetRow((UIElement) grid7, 2);
      Grid.SetColumnSpan((UIElement) grid7, 2);
      Button button3 = new Button();
      grid7.Children.Add((UIElement) button3);
      button3.Name = "e_61";
      button3.Width = 200f;
      button3.Visibility = Visibility.Collapsed;
      button3.Margin = new Thickness(0.0f, 10f, 10f, 10f);
      button3.VerticalAlignment = VerticalAlignment.Center;
      button3.SetBinding(UIElement.IsEnabledProperty, new Binding("IsPreviewEnabled")
      {
        UseGeneratedBindings = true
      });
      button3.SetBinding(Button.CommandProperty, new Binding("ShowPreviewCommand")
      {
        UseGeneratedBindings = true
      });
      button3.SetResourceReference(ContentControl.ContentProperty, (object) "StoreScreen_Preview");
      StackPanel stackPanel2 = new StackPanel();
      grid7.Children.Add((UIElement) stackPanel2);
      stackPanel2.Name = "e_62";
      stackPanel2.HorizontalAlignment = HorizontalAlignment.Stretch;
      stackPanel2.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel2, 1);
      TextBlock textBlock13 = new TextBlock();
      stackPanel2.Children.Add((UIElement) textBlock13);
      textBlock13.Name = "e_63";
      textBlock13.Margin = new Thickness(5f, 5f, 0.0f, 5f);
      textBlock13.VerticalAlignment = VerticalAlignment.Center;
      textBlock13.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock13, 0);
      textBlock13.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlockView_AmountLabel");
      NumericTextBox numericTextBox2 = new NumericTextBox();
      stackPanel2.Children.Add((UIElement) numericTextBox2);
      numericTextBox2.Name = "e_64";
      numericTextBox2.Width = 150f;
      numericTextBox2.Margin = new Thickness(5f, 5f, 5f, 5f);
      numericTextBox2.VerticalAlignment = VerticalAlignment.Center;
      numericTextBox2.TabIndex = 2;
      numericTextBox2.MaxLength = 5;
      numericTextBox2.Minimum = 0.0f;
      numericTextBox2.SetBinding(NumericTextBox.MaximumProperty, new Binding("AmountToBuyMaximum")
      {
        UseGeneratedBindings = true
      });
      numericTextBox2.SetBinding(NumericTextBox.ValueProperty, new Binding("AmountToBuy")
      {
        UseGeneratedBindings = true
      });
      Button button4 = new Button();
      stackPanel2.Children.Add((UIElement) button4);
      button4.Name = "e_65";
      button4.Width = 75f;
      button4.Margin = new Thickness(5f, 5f, 5f, 5f);
      button4.VerticalAlignment = VerticalAlignment.Center;
      button4.TabIndex = 3;
      button4.SetBinding(Button.CommandProperty, new Binding("SetAllAmountOfferCommand")
      {
        UseGeneratedBindings = true
      });
      button4.SetResourceReference(ContentControl.ContentProperty, (object) "StoreScreen_AllButton");
      StackPanel stackPanel3 = new StackPanel();
      grid7.Children.Add((UIElement) stackPanel3);
      stackPanel3.Name = "e_66";
      stackPanel3.Margin = new Thickness(5f, 5f, 5f, 5f);
      stackPanel3.HorizontalAlignment = HorizontalAlignment.Right;
      stackPanel3.VerticalAlignment = VerticalAlignment.Center;
      stackPanel3.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel3, 2);
      TextBlock textBlock14 = new TextBlock();
      stackPanel3.Children.Add((UIElement) textBlock14);
      textBlock14.Name = "e_67";
      textBlock14.HorizontalAlignment = HorizontalAlignment.Right;
      textBlock14.VerticalAlignment = VerticalAlignment.Center;
      textBlock14.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      textBlock14.SetBinding(TextBlock.TextProperty, new Binding("TotalPriceToBuy")
      {
        UseGeneratedBindings = true
      });
      Image image3 = new Image();
      stackPanel3.Children.Add((UIElement) image3);
      image3.Name = "e_68";
      image3.Width = 20f;
      image3.Margin = new Thickness(4f, 2f, 2f, 2f);
      image3.VerticalAlignment = VerticalAlignment.Center;
      image3.Stretch = Stretch.Uniform;
      image3.SetBinding(Image.SourceProperty, new Binding("InventoryTargetViewModel.CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      Button button5 = new Button();
      grid7.Children.Add((UIElement) button5);
      button5.Name = "e_69";
      button5.Margin = new Thickness(10f, 10f, 0.0f, 10f);
      button5.VerticalAlignment = VerticalAlignment.Center;
      button5.TabIndex = 9;
      Grid.SetColumn((UIElement) button5, 3);
      button5.SetBinding(UIElement.IsEnabledProperty, new Binding("IsBuyEnabled")
      {
        UseGeneratedBindings = true
      });
      button5.SetBinding(Button.CommandProperty, new Binding("BuyCommand")
      {
        UseGeneratedBindings = true
      });
      StackPanel stackPanel4 = new StackPanel();
      button5.Content = (object) stackPanel4;
      stackPanel4.Name = "e_70";
      stackPanel4.HorizontalAlignment = HorizontalAlignment.Center;
      stackPanel4.VerticalAlignment = VerticalAlignment.Center;
      stackPanel4.Orientation = Orientation.Horizontal;
      TextBlock textBlock15 = new TextBlock();
      stackPanel4.Children.Add((UIElement) textBlock15);
      textBlock15.Name = "e_71";
      textBlock15.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      textBlock15.Text = "\xE002";
      textBlock15.SetBinding(UIElement.VisibilityProperty, new Binding("IsJoystickLastUsed")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock16 = new TextBlock();
      stackPanel4.Children.Add((UIElement) textBlock16);
      textBlock16.Name = "e_72";
      textBlock16.SetResourceReference(TextBlock.TextProperty, (object) "StoreScreen_BuyButton");
      Button button6 = new Button();
      grid7.Children.Add((UIElement) button6);
      button6.Name = "e_73";
      button6.Margin = new Thickness(10f, 10f, 0.0f, 10f);
      button6.VerticalAlignment = VerticalAlignment.Center;
      button6.TabIndex = 10;
      Grid.SetColumn((UIElement) button6, 4);
      button6.SetBinding(UIElement.IsEnabledProperty, new Binding("IsRefreshEnabled")
      {
        UseGeneratedBindings = true
      });
      button6.SetBinding(Button.CommandProperty, new Binding("RefreshCommand")
      {
        UseGeneratedBindings = true
      });
      StackPanel stackPanel5 = new StackPanel();
      button6.Content = (object) stackPanel5;
      stackPanel5.Name = "e_74";
      stackPanel5.HorizontalAlignment = HorizontalAlignment.Center;
      stackPanel5.VerticalAlignment = VerticalAlignment.Center;
      stackPanel5.Orientation = Orientation.Horizontal;
      TextBlock textBlock17 = new TextBlock();
      stackPanel5.Children.Add((UIElement) textBlock17);
      textBlock17.Name = "e_75";
      textBlock17.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      textBlock17.Text = "\xE004";
      textBlock17.SetBinding(UIElement.VisibilityProperty, new Binding("IsJoystickLastUsed")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock18 = new TextBlock();
      stackPanel5.Children.Add((UIElement) textBlock18);
      textBlock18.Name = "e_76";
      textBlock18.SetResourceReference(TextBlock.TextProperty, (object) "buttonRefresh");
      observableCollection.Add((object) tabItem1);
      TabItem tabItem2 = new TabItem();
      tabItem2.Name = "e_77";
      tabItem2.SetBinding(UIElement.VisibilityProperty, new Binding("IsSellTabVisible")
      {
        UseGeneratedBindings = true
      });
      tabItem2.SetResourceReference(HeaderedContentControl.HeaderProperty, (object) "StoreScreenSellHeader");
      Grid grid8 = new Grid();
      tabItem2.Content = (object) grid8;
      grid8.Name = "e_78";
      RowDefinition rowDefinition6 = new RowDefinition();
      grid8.RowDefinitions.Add(rowDefinition6);
      grid8.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid8.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid8.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(2f, GridUnitType.Star)
      });
      grid8.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Star)
      });
      DataGrid dataGrid2 = new DataGrid();
      grid8.Children.Add((UIElement) dataGrid2);
      dataGrid2.Name = "e_79";
      dataGrid2.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      dataGrid2.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      dataGrid2.TabIndex = 10;
      dataGrid2.SelectionMode = DataGridSelectionMode.Single;
      dataGrid2.AutoGenerateColumns = false;
      DataGridTemplateColumn gridTemplateColumn5 = new DataGridTemplateColumn();
      gridTemplateColumn5.Width = (DataGridLength) 64f;
      gridTemplateColumn5.SortMemberPath = "Name";
      TextBlock textBlock19 = new TextBlock();
      textBlock19.Name = "e_80";
      textBlock19.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock19.Text = "";
      gridTemplateColumn5.Header = (object) textBlock19;
      Func<UIElement, UIElement> createMethod5 = new Func<UIElement, UIElement>(StoreBlockView.e_79_Col0_ct_dtMethod);
      gridTemplateColumn5.CellTemplate = new DataTemplate(createMethod5);
      dataGrid2.Columns.Add((DataGridColumn) gridTemplateColumn5);
      DataGridTemplateColumn gridTemplateColumn6 = new DataGridTemplateColumn();
      gridTemplateColumn6.Width = new DataGridLength(1f, DataGridLengthUnitType.Star);
      gridTemplateColumn6.SortMemberPath = "Name";
      TextBlock textBlock20 = new TextBlock();
      textBlock20.Name = "e_82";
      textBlock20.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock20.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlock_Column_Name");
      gridTemplateColumn6.Header = (object) textBlock20;
      Func<UIElement, UIElement> createMethod6 = new Func<UIElement, UIElement>(StoreBlockView.e_79_Col1_ct_dtMethod);
      gridTemplateColumn6.CellTemplate = new DataTemplate(createMethod6);
      dataGrid2.Columns.Add((DataGridColumn) gridTemplateColumn6);
      DataGridTemplateColumn gridTemplateColumn7 = new DataGridTemplateColumn();
      gridTemplateColumn7.Width = (DataGridLength) 150f;
      gridTemplateColumn7.SortMemberPath = "Amount";
      TextBlock textBlock21 = new TextBlock();
      textBlock21.Name = "e_84";
      textBlock21.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock21.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlock_Column_Amount");
      gridTemplateColumn7.Header = (object) textBlock21;
      Func<UIElement, UIElement> createMethod7 = new Func<UIElement, UIElement>(StoreBlockView.e_79_Col2_ct_dtMethod);
      gridTemplateColumn7.CellTemplate = new DataTemplate(createMethod7);
      dataGrid2.Columns.Add((DataGridColumn) gridTemplateColumn7);
      DataGridTemplateColumn gridTemplateColumn8 = new DataGridTemplateColumn();
      gridTemplateColumn8.Width = (DataGridLength) 200f;
      gridTemplateColumn8.SortMemberPath = "PricePerUnit";
      TextBlock textBlock22 = new TextBlock();
      textBlock22.Name = "e_86";
      textBlock22.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock22.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlock_Column_PricePerUnit");
      gridTemplateColumn8.Header = (object) textBlock22;
      Func<UIElement, UIElement> createMethod8 = new Func<UIElement, UIElement>(StoreBlockView.e_79_Col3_ct_dtMethod);
      gridTemplateColumn8.CellTemplate = new DataTemplate(createMethod8);
      dataGrid2.Columns.Add((DataGridColumn) gridTemplateColumn8);
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
      Grid grid9 = new Grid();
      grid8.Children.Add((UIElement) grid9);
      grid9.Name = "e_90";
      grid9.Margin = new Thickness(10f, 0.0f, 0.0f, 0.0f);
      Grid.SetColumn((UIElement) grid9, 1);
      Grid.SetRow((UIElement) grid9, 0);
      grid9.SetBinding(UIElement.DataContextProperty, new Binding("InventoryTargetViewModel")
      {
        UseGeneratedBindings = true
      });
      Grid grid10 = new Grid();
      grid9.Children.Add((UIElement) grid10);
      grid10.Name = "e_91";
      grid10.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid10.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      RowDefinition rowDefinition7 = new RowDefinition();
      grid10.RowDefinitions.Add(rowDefinition7);
      grid10.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid10.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      TextBlock textBlock23 = new TextBlock();
      grid10.Children.Add((UIElement) textBlock23);
      textBlock23.Name = "e_92";
      textBlock23.Margin = new Thickness(0.0f, 0.0f, 0.0f, 5f);
      textBlock23.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetRow((UIElement) textBlock23, 0);
      textBlock23.SetResourceReference(TextBlock.TextProperty, (object) "StoreScreen_SelectInventory");
      ComboBox comboBox2 = new ComboBox();
      grid10.Children.Add((UIElement) comboBox2);
      comboBox2.Name = "e_93";
      comboBox2.TabIndex = 13;
      comboBox2.MaxDropDownHeight = 300f;
      Grid.SetRow((UIElement) comboBox2, 1);
      comboBox2.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("Inventories")
      {
        UseGeneratedBindings = true
      });
      comboBox2.SetBinding(Selector.SelectedIndexProperty, new Binding("SelectedInventoryIndex")
      {
        UseGeneratedBindings = true
      });
      ListBox listBox2 = new ListBox();
      grid10.Children.Add((UIElement) listBox2);
      listBox2.Name = "e_94";
      listBox2.Margin = new Thickness(0.0f, 4f, 0.0f, 0.0f);
      Style style2 = new Style(typeof (ListBox));
      Setter setter4 = new Setter(UIElement.MinHeightProperty, (object) 80f);
      style2.Setters.Add(setter4);
      ControlTemplate controlTemplate2 = new ControlTemplate(typeof (ListBox), new Func<UIElement, UIElement>(StoreBlockView.e_94_s_S_1_ctMethod));
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
      DragDrop.SetIsDropTarget((UIElement) listBox2, true);
      listBox2.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("InventoryItems")
      {
        UseGeneratedBindings = true
      });
      StoreBlockView.InitializeElemente_94Resources((UIElement) listBox2);
      Grid grid11 = new Grid();
      grid10.Children.Add((UIElement) grid11);
      grid11.Name = "e_99";
      grid11.Margin = new Thickness(2f, 2f, 2f, 2f);
      grid11.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition6 = new ColumnDefinition();
      grid11.ColumnDefinitions.Add(columnDefinition6);
      Grid.SetRow((UIElement) grid11, 3);
      TextBlock textBlock24 = new TextBlock();
      grid11.Children.Add((UIElement) textBlock24);
      textBlock24.Name = "e_100";
      textBlock24.Margin = new Thickness(0.0f, 5f, 5f, 5f);
      textBlock24.VerticalAlignment = VerticalAlignment.Center;
      textBlock24.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      textBlock24.SetResourceReference(TextBlock.TextProperty, (object) "ScreenTerminalInventory_Volume");
      StackPanel stackPanel6 = new StackPanel();
      grid11.Children.Add((UIElement) stackPanel6);
      stackPanel6.Name = "e_101";
      stackPanel6.Margin = new Thickness(5f, 5f, 0.0f, 5f);
      stackPanel6.HorizontalAlignment = HorizontalAlignment.Right;
      stackPanel6.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel6, 1);
      TextBlock textBlock25 = new TextBlock();
      stackPanel6.Children.Add((UIElement) textBlock25);
      textBlock25.Name = "e_102";
      textBlock25.VerticalAlignment = VerticalAlignment.Center;
      textBlock25.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      textBlock25.SetBinding(TextBlock.TextProperty, new Binding("CurrentInventoryVolume")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock26 = new TextBlock();
      stackPanel6.Children.Add((UIElement) textBlock26);
      textBlock26.Name = "e_103";
      textBlock26.Margin = new Thickness(10f, 0.0f, 10f, 0.0f);
      textBlock26.VerticalAlignment = VerticalAlignment.Center;
      textBlock26.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      textBlock26.Text = "/";
      TextBlock textBlock27 = new TextBlock();
      stackPanel6.Children.Add((UIElement) textBlock27);
      textBlock27.Name = "e_104";
      textBlock27.VerticalAlignment = VerticalAlignment.Center;
      textBlock27.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      textBlock27.SetBinding(TextBlock.TextProperty, new Binding("MaxInventoryVolume")
      {
        UseGeneratedBindings = true
      });
      Grid grid12 = new Grid();
      grid10.Children.Add((UIElement) grid12);
      grid12.Name = "e_105";
      grid12.Margin = new Thickness(2f, 2f, 2f, 2f);
      grid12.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition7 = new ColumnDefinition();
      grid12.ColumnDefinitions.Add(columnDefinition7);
      grid12.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      Grid.SetRow((UIElement) grid12, 4);
      TextBlock textBlock28 = new TextBlock();
      grid12.Children.Add((UIElement) textBlock28);
      textBlock28.Name = "e_106";
      textBlock28.Margin = new Thickness(0.0f, 5f, 5f, 5f);
      textBlock28.VerticalAlignment = VerticalAlignment.Center;
      textBlock28.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      textBlock28.SetResourceReference(TextBlock.TextProperty, (object) "Currency_Default_Account_Label");
      TextBlock textBlock29 = new TextBlock();
      grid12.Children.Add((UIElement) textBlock29);
      textBlock29.Name = "e_107";
      textBlock29.Margin = new Thickness(5f, 5f, 0.0f, 5f);
      textBlock29.HorizontalAlignment = HorizontalAlignment.Right;
      textBlock29.VerticalAlignment = VerticalAlignment.Center;
      textBlock29.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock29, 1);
      textBlock29.SetBinding(TextBlock.TextProperty, new Binding("LocalPlayerCurrency")
      {
        UseGeneratedBindings = true
      });
      Image image4 = new Image();
      grid12.Children.Add((UIElement) image4);
      image4.Name = "e_108";
      image4.Height = 20f;
      image4.Margin = new Thickness(4f, 2f, 2f, 2f);
      image4.VerticalAlignment = VerticalAlignment.Center;
      image4.Stretch = Stretch.Uniform;
      Grid.SetColumn((UIElement) image4, 2);
      image4.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      Border border2 = new Border();
      grid8.Children.Add((UIElement) border2);
      border2.Name = "e_109";
      border2.Height = 2f;
      border2.Margin = new Thickness(0.0f, 10f, 0.0f, 10f);
      border2.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      Grid.SetRow((UIElement) border2, 1);
      Grid.SetColumnSpan((UIElement) border2, 2);
      Grid grid13 = new Grid();
      grid8.Children.Add((UIElement) grid13);
      grid13.Name = "e_110";
      grid13.Margin = new Thickness(0.0f, 0.0f, 0.0f, 30f);
      ColumnDefinition columnDefinition8 = new ColumnDefinition();
      grid13.ColumnDefinitions.Add(columnDefinition8);
      grid13.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid13.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(200f, GridUnitType.Pixel)
      });
      grid13.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(150f, GridUnitType.Pixel)
      });
      grid13.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(150f, GridUnitType.Pixel)
      });
      Grid.SetRow((UIElement) grid13, 2);
      Grid.SetColumnSpan((UIElement) grid13, 2);
      StackPanel stackPanel7 = new StackPanel();
      grid13.Children.Add((UIElement) stackPanel7);
      stackPanel7.Name = "e_111";
      stackPanel7.HorizontalAlignment = HorizontalAlignment.Stretch;
      stackPanel7.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel7, 1);
      TextBlock textBlock30 = new TextBlock();
      stackPanel7.Children.Add((UIElement) textBlock30);
      textBlock30.Name = "e_112";
      textBlock30.Margin = new Thickness(5f, 5f, 0.0f, 5f);
      textBlock30.VerticalAlignment = VerticalAlignment.Center;
      textBlock30.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock30, 0);
      textBlock30.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlockView_AmountLabel");
      NumericTextBox numericTextBox3 = new NumericTextBox();
      stackPanel7.Children.Add((UIElement) numericTextBox3);
      numericTextBox3.Name = "e_113";
      numericTextBox3.Width = 150f;
      numericTextBox3.Margin = new Thickness(5f, 5f, 5f, 5f);
      numericTextBox3.VerticalAlignment = VerticalAlignment.Center;
      numericTextBox3.TabIndex = 11;
      numericTextBox3.MaxLength = 5;
      numericTextBox3.Minimum = 0.0f;
      numericTextBox3.Maximum = 99999f;
      numericTextBox3.SetBinding(NumericTextBox.ValueProperty, new Binding("AmountToSell")
      {
        UseGeneratedBindings = true
      });
      Button button7 = new Button();
      stackPanel7.Children.Add((UIElement) button7);
      button7.Name = "e_114";
      button7.Width = 75f;
      button7.Margin = new Thickness(5f, 5f, 5f, 5f);
      button7.VerticalAlignment = VerticalAlignment.Center;
      button7.TabIndex = 12;
      button7.SetBinding(Button.CommandProperty, new Binding("SetAllAmountOrderCommand")
      {
        UseGeneratedBindings = true
      });
      button7.SetResourceReference(ContentControl.ContentProperty, (object) "StoreScreen_AllButton");
      StackPanel stackPanel8 = new StackPanel();
      grid13.Children.Add((UIElement) stackPanel8);
      stackPanel8.Name = "e_115";
      stackPanel8.Margin = new Thickness(5f, 5f, 5f, 5f);
      stackPanel8.HorizontalAlignment = HorizontalAlignment.Right;
      stackPanel8.VerticalAlignment = VerticalAlignment.Center;
      stackPanel8.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel8, 2);
      TextBlock textBlock31 = new TextBlock();
      stackPanel8.Children.Add((UIElement) textBlock31);
      textBlock31.Name = "e_116";
      textBlock31.HorizontalAlignment = HorizontalAlignment.Right;
      textBlock31.VerticalAlignment = VerticalAlignment.Center;
      textBlock31.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      textBlock31.SetBinding(TextBlock.TextProperty, new Binding("TotalPriceToSell")
      {
        UseGeneratedBindings = true
      });
      Image image5 = new Image();
      stackPanel8.Children.Add((UIElement) image5);
      image5.Name = "e_117";
      image5.Width = 20f;
      image5.Margin = new Thickness(4f, 2f, 2f, 2f);
      image5.VerticalAlignment = VerticalAlignment.Center;
      image5.Stretch = Stretch.Uniform;
      image5.SetBinding(Image.SourceProperty, new Binding("InventoryTargetViewModel.CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      Button button8 = new Button();
      grid13.Children.Add((UIElement) button8);
      button8.Name = "e_118";
      button8.Margin = new Thickness(10f, 10f, 0.0f, 10f);
      button8.VerticalAlignment = VerticalAlignment.Center;
      button8.TabIndex = 15;
      Grid.SetColumn((UIElement) button8, 3);
      button8.SetBinding(UIElement.IsEnabledProperty, new Binding("IsSellEnabled")
      {
        UseGeneratedBindings = true
      });
      button8.SetBinding(Button.CommandProperty, new Binding("SellCommand")
      {
        UseGeneratedBindings = true
      });
      button8.SetResourceReference(ContentControl.ContentProperty, (object) "StoreScreen_SellButton");
      Button button9 = new Button();
      grid13.Children.Add((UIElement) button9);
      button9.Name = "e_119";
      button9.Margin = new Thickness(10f, 10f, 0.0f, 10f);
      button9.VerticalAlignment = VerticalAlignment.Center;
      button9.TabIndex = 16;
      Grid.SetColumn((UIElement) button9, 4);
      button9.SetBinding(UIElement.IsEnabledProperty, new Binding("IsRefreshEnabled")
      {
        UseGeneratedBindings = true
      });
      button9.SetBinding(Button.CommandProperty, new Binding("RefreshCommand")
      {
        UseGeneratedBindings = true
      });
      button9.SetResourceReference(ContentControl.ContentProperty, (object) "buttonRefresh");
      observableCollection.Add((object) tabItem2);
      TabItem tabItem3 = new TabItem();
      tabItem3.Name = "e_120";
      tabItem3.IsTabStop = false;
      tabItem3.SetBinding(UIElement.VisibilityProperty, new Binding("IsAdministrationVisible")
      {
        UseGeneratedBindings = true
      });
      tabItem3.SetResourceReference(HeaderedContentControl.HeaderProperty, (object) "StoreAdministration");
      Grid grid14 = new Grid();
      tabItem3.Content = (object) grid14;
      grid14.Name = "e_121";
      grid14.Margin = new Thickness(0.0f, 0.0f, 0.0f, 30f);
      RowDefinition rowDefinition8 = new RowDefinition();
      grid14.RowDefinitions.Add(rowDefinition8);
      grid14.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition9 = new ColumnDefinition();
      grid14.ColumnDefinitions.Add(columnDefinition9);
      ColumnDefinition columnDefinition10 = new ColumnDefinition();
      grid14.ColumnDefinitions.Add(columnDefinition10);
      grid14.SetBinding(UIElement.DataContextProperty, new Binding("AdministrationViewModel")
      {
        UseGeneratedBindings = true
      });
      ListBox listBox3 = new ListBox();
      grid14.Children.Add((UIElement) listBox3);
      listBox3.Name = "e_122";
      listBox3.Margin = new Thickness(0.0f, 10f, 10f, 0.0f);
      listBox3.Background = (Brush) new SolidColorBrush(new ColorW(41, 54, 62, (int) byte.MaxValue));
      listBox3.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      listBox3.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      listBox3.TabIndex = 30;
      listBox3.SelectionMode = SelectionMode.Single;
      Grid.SetColumn((UIElement) listBox3, 0);
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
      Button button10 = new Button();
      grid14.Children.Add((UIElement) button10);
      button10.Name = "e_123";
      button10.Width = 140f;
      button10.Margin = new Thickness(0.0f, 10f, 10f, 20f);
      button10.HorizontalAlignment = HorizontalAlignment.Right;
      button10.VerticalAlignment = VerticalAlignment.Center;
      button10.TabIndex = 31;
      Grid.SetRow((UIElement) button10, 1);
      GamepadHelp.SetTabIndexUp((DependencyObject) button10, 30);
      button10.SetBinding(Button.CommandProperty, new Binding("RemoveStoreItemCommand")
      {
        UseGeneratedBindings = true
      });
      button10.SetResourceReference(ContentControl.ContentProperty, (object) "StoreScreen_CancelButton");
      TabControl tabControl = new TabControl();
      grid14.Children.Add((UIElement) tabControl);
      tabControl.Name = "e_124";
      tabControl.Margin = new Thickness(2f, 2f, 2f, 2f);
      tabControl.TabIndex = 32;
      tabControl.ItemsSource = (IEnumerable) StoreBlockView.Get_e_124_Items();
      Grid.SetColumn((UIElement) tabControl, 1);
      Grid.SetRowSpan((UIElement) tabControl, 2);
      GamepadHelp.SetTabIndexLeft((DependencyObject) tabControl, 30);
      StoreBlockView.InitializeElemente_124Resources((UIElement) tabControl);
      observableCollection.Add((object) tabItem3);
      return observableCollection;
    }

    private static UIElement tt_e_13_Method()
    {
      Grid grid = new Grid();
      grid.Name = "e_14";
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
      border.Name = "e_15";
      border.Margin = new Thickness(2f, 2f, 2f, 2f);
      border.Background = (Brush) new SolidColorBrush(new ColorW(41, 54, 62, (int) byte.MaxValue));
      TextBlock textBlock1 = new TextBlock();
      grid.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_16";
      textBlock1.Margin = new Thickness(5f, 5f, 5f, 5f);
      textBlock1.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock1.VerticalAlignment = VerticalAlignment.Center;
      textBlock1.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Binding binding1 = new Binding("Name");
      textBlock1.SetBinding(TextBlock.TextProperty, binding1);
      Image image = new Image();
      grid.Children.Add((UIElement) image);
      image.Name = "e_17";
      image.Margin = new Thickness(2f, 2f, 2f, 2f);
      image.Stretch = Stretch.Uniform;
      Grid.SetRow((UIElement) image, 1);
      Binding binding2 = new Binding("TooltipImage");
      image.SetBinding(Image.SourceProperty, binding2);
      StackPanel stackPanel = new StackPanel();
      grid.Children.Add((UIElement) stackPanel);
      stackPanel.Name = "e_18";
      stackPanel.Margin = new Thickness(5f, 5f, 5f, 5f);
      stackPanel.Orientation = Orientation.Horizontal;
      Grid.SetRow((UIElement) stackPanel, 2);
      TextBlock textBlock2 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock2);
      textBlock2.Name = "e_19";
      textBlock2.VerticalAlignment = VerticalAlignment.Center;
      textBlock2.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      textBlock2.SetResourceReference(TextBlock.TextProperty, (object) "StoreScreen_GridTooltip_Pcu");
      TextBlock textBlock3 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock3);
      textBlock3.Name = "e_20";
      textBlock3.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock3.VerticalAlignment = VerticalAlignment.Center;
      textBlock3.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Binding binding3 = new Binding("PrefabTotalPcu");
      textBlock3.SetBinding(TextBlock.TextProperty, binding3);
      TextBlock textBlock4 = new TextBlock();
      grid.Children.Add((UIElement) textBlock4);
      textBlock4.Name = "e_21";
      textBlock4.Margin = new Thickness(5f, 5f, 5f, 5f);
      textBlock4.TextWrapping = TextWrapping.Wrap;
      Grid.SetRow((UIElement) textBlock4, 3);
      Binding binding4 = new Binding("Description");
      textBlock4.SetBinding(TextBlock.TextProperty, binding4);
      return (UIElement) grid;
    }

    private static UIElement e_9_Col0_ct_dtMethod(UIElement parent)
    {
      Grid grid1 = new Grid();
      grid1.Parent = parent;
      grid1.Name = "e_11";
      grid1.HorizontalAlignment = HorizontalAlignment.Stretch;
      grid1.VerticalAlignment = VerticalAlignment.Stretch;
      Image image = new Image();
      grid1.Children.Add((UIElement) image);
      image.Name = "e_12";
      image.Stretch = Stretch.Uniform;
      Binding binding1 = new Binding("Icon");
      image.SetBinding(Image.SourceProperty, binding1);
      Grid grid2 = new Grid();
      grid1.Children.Add((UIElement) grid2);
      grid2.Name = "e_13";
      grid2.HorizontalAlignment = HorizontalAlignment.Stretch;
      grid2.VerticalAlignment = VerticalAlignment.Stretch;
      ToolTip toolTip = new ToolTip();
      grid2.ToolTip = (object) toolTip;
      toolTip.Content = (object) StoreBlockView.tt_e_13_Method();
      Binding binding2 = new Binding("HasTooltip");
      grid2.SetBinding(UIElement.VisibilityProperty, binding2);
      return (UIElement) grid1;
    }

    private static UIElement e_9_Col1_ct_dtMethod(UIElement parent)
    {
      TextBlock textBlock = new TextBlock();
      textBlock.Parent = parent;
      textBlock.Name = "e_23";
      textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock.VerticalAlignment = VerticalAlignment.Center;
      Binding binding = new Binding("Name");
      textBlock.SetBinding(TextBlock.TextProperty, binding);
      return (UIElement) textBlock;
    }

    private static UIElement e_9_Col2_ct_dtMethod(UIElement parent)
    {
      TextBlock textBlock = new TextBlock();
      textBlock.Parent = parent;
      textBlock.Name = "e_25";
      textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock.HorizontalAlignment = HorizontalAlignment.Right;
      textBlock.VerticalAlignment = VerticalAlignment.Center;
      Binding binding = new Binding("AmountFormatted");
      textBlock.SetBinding(TextBlock.TextProperty, binding);
      return (UIElement) textBlock;
    }

    private static UIElement tt_e_29_Method()
    {
      StackPanel stackPanel = new StackPanel();
      stackPanel.Name = "e_30";
      stackPanel.Orientation = Orientation.Horizontal;
      Binding binding = new Binding("HasPricePerUnitDiscount");
      stackPanel.SetBinding(UIElement.VisibilityProperty, binding);
      TextBlock textBlock1 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_31";
      textBlock1.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock1.VerticalAlignment = VerticalAlignment.Center;
      textBlock1.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlock_OfferDiscount");
      TextBlock textBlock2 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock2);
      textBlock2.Name = "e_32";
      textBlock2.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock2.VerticalAlignment = VerticalAlignment.Center;
      textBlock2.SetBinding(TextBlock.TextProperty, new Binding("PricePerUnitDiscount")
      {
        StringFormat = "{0}%"
      });
      return (UIElement) stackPanel;
    }

    private static UIElement e_9_Col3_ct_dtMethod(UIElement parent)
    {
      StackPanel stackPanel = new StackPanel();
      stackPanel.Parent = parent;
      stackPanel.Name = "e_27";
      stackPanel.Margin = new Thickness(0.0f, 0.0f, 4f, 0.0f);
      stackPanel.HorizontalAlignment = HorizontalAlignment.Right;
      stackPanel.Orientation = Orientation.Horizontal;
      TextBlock textBlock1 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_28";
      textBlock1.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock1.VerticalAlignment = VerticalAlignment.Center;
      Binding binding1 = new Binding("HasNormalPrice");
      textBlock1.SetBinding(UIElement.VisibilityProperty, binding1);
      Binding binding2 = new Binding("PricePerUnitFormatted");
      textBlock1.SetBinding(TextBlock.TextProperty, binding2);
      TextBlock textBlock2 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock2);
      textBlock2.Name = "e_29";
      textBlock2.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock2.VerticalAlignment = VerticalAlignment.Center;
      ToolTip toolTip = new ToolTip();
      textBlock2.ToolTip = (object) toolTip;
      toolTip.Content = (object) StoreBlockView.tt_e_29_Method();
      textBlock2.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 44, 20, (int) byte.MaxValue));
      Binding binding3 = new Binding("HasPricePerUnitDiscount");
      textBlock2.SetBinding(UIElement.VisibilityProperty, binding3);
      Binding binding4 = new Binding("PricePerUnitFormatted");
      textBlock2.SetBinding(TextBlock.TextProperty, binding4);
      Image image = new Image();
      stackPanel.Children.Add((UIElement) image);
      image.Name = "e_33";
      image.Width = 20f;
      image.Margin = new Thickness(2f, 2f, 2f, 2f);
      image.VerticalAlignment = VerticalAlignment.Center;
      image.Stretch = Stretch.Uniform;
      Binding binding5 = new Binding("CurrencyIcon");
      image.SetBinding(Image.SourceProperty, binding5);
      return (UIElement) stackPanel;
    }

    private static void InitializeElemente_9Resources(UIElement elem)
    {
      Style style = new Style(typeof (DataGridRow), elem.Resources[(object) typeof (DataGridRow)] as Style);
      EventTrigger eventTrigger = new EventTrigger(Control.MouseDoubleClickEvent);
      style.Triggers.Add((TriggerBase) eventTrigger);
      eventTrigger.SetBinding(EventTrigger.CommandProperty, new Binding("ViewModel.OnBuyItemDoubleClickCommand")
      {
        Source = (object) new MyStoreBlockViewModelLocator(false)
      });
      elem.Resources.Add((object) typeof (DataGridRow), (object) style);
    }

    private static UIElement e_38_s_S_1_ctMethod(UIElement parent)
    {
      Border border = new Border();
      border.Parent = parent;
      border.Name = "e_39";
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
      wrapPanel.Name = "e_40";
      wrapPanel.Margin = new Thickness(4f, 4f, 4f, 4f);
      wrapPanel.IsItemsHost = true;
      wrapPanel.ItemHeight = 64f;
      wrapPanel.ItemWidth = 64f;
      return (UIElement) border;
    }

    private static void InitializeElemente_38Resources(UIElement elem)
    {
      Style style = new Style(typeof (ListBoxItem));
      Setter setter1 = new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "GridItem"));
      style.Setters.Add(setter1);
      ControlTemplate controlTemplate = new ControlTemplate(typeof (ListBoxItem), new Func<UIElement, UIElement>(StoreBlockView.e_38r_0_s_S_1_ctMethod));
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

    private static UIElement e_38r_0_s_S_1_ctMethod(UIElement parent)
    {
      Border border = new Border();
      border.Parent = parent;
      border.Name = "e_41";
      border.SetBinding(Control.BackgroundProperty, new Binding("Background")
      {
        Source = (object) parent
      });
      ContentPresenter contentPresenter = new ContentPresenter();
      border.Child = (UIElement) contentPresenter;
      contentPresenter.Name = "e_42";
      contentPresenter.HorizontalAlignment = HorizontalAlignment.Stretch;
      contentPresenter.VerticalAlignment = VerticalAlignment.Stretch;
      Binding binding = new Binding();
      contentPresenter.SetBinding(ContentPresenter.ContentProperty, binding);
      return (UIElement) border;
    }

    private static UIElement e_79_Col0_ct_dtMethod(UIElement parent)
    {
      Image image = new Image();
      image.Parent = parent;
      image.Name = "e_81";
      image.Stretch = Stretch.Fill;
      Binding binding = new Binding("Icon");
      image.SetBinding(Image.SourceProperty, binding);
      return (UIElement) image;
    }

    private static UIElement e_79_Col1_ct_dtMethod(UIElement parent)
    {
      TextBlock textBlock = new TextBlock();
      textBlock.Parent = parent;
      textBlock.Name = "e_83";
      textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock.VerticalAlignment = VerticalAlignment.Center;
      Binding binding = new Binding("Name");
      textBlock.SetBinding(TextBlock.TextProperty, binding);
      return (UIElement) textBlock;
    }

    private static UIElement e_79_Col2_ct_dtMethod(UIElement parent)
    {
      TextBlock textBlock = new TextBlock();
      textBlock.Parent = parent;
      textBlock.Name = "e_85";
      textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock.HorizontalAlignment = HorizontalAlignment.Right;
      textBlock.VerticalAlignment = VerticalAlignment.Center;
      Binding binding = new Binding("AmountFormatted");
      textBlock.SetBinding(TextBlock.TextProperty, binding);
      return (UIElement) textBlock;
    }

    private static UIElement e_79_Col3_ct_dtMethod(UIElement parent)
    {
      StackPanel stackPanel = new StackPanel();
      stackPanel.Parent = parent;
      stackPanel.Name = "e_87";
      stackPanel.Margin = new Thickness(0.0f, 0.0f, 4f, 0.0f);
      stackPanel.HorizontalAlignment = HorizontalAlignment.Right;
      stackPanel.Orientation = Orientation.Horizontal;
      TextBlock textBlock = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock);
      textBlock.Name = "e_88";
      textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock.VerticalAlignment = VerticalAlignment.Center;
      Binding binding1 = new Binding("PricePerUnitFormatted");
      textBlock.SetBinding(TextBlock.TextProperty, binding1);
      Image image = new Image();
      stackPanel.Children.Add((UIElement) image);
      image.Name = "e_89";
      image.Width = 20f;
      image.Margin = new Thickness(2f, 2f, 2f, 2f);
      image.VerticalAlignment = VerticalAlignment.Center;
      image.Stretch = Stretch.Uniform;
      Binding binding2 = new Binding("CurrencyIcon");
      image.SetBinding(Image.SourceProperty, binding2);
      return (UIElement) stackPanel;
    }

    private static UIElement e_94_s_S_1_ctMethod(UIElement parent)
    {
      Border border = new Border();
      border.Parent = parent;
      border.Name = "e_95";
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
      wrapPanel.Name = "e_96";
      wrapPanel.Margin = new Thickness(4f, 4f, 4f, 4f);
      wrapPanel.IsItemsHost = true;
      wrapPanel.ItemHeight = 64f;
      wrapPanel.ItemWidth = 64f;
      return (UIElement) border;
    }

    private static void InitializeElemente_94Resources(UIElement elem)
    {
      Style style = new Style(typeof (ListBoxItem));
      Setter setter1 = new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "GridItem"));
      style.Setters.Add(setter1);
      ControlTemplate controlTemplate = new ControlTemplate(typeof (ListBoxItem), new Func<UIElement, UIElement>(StoreBlockView.e_94r_0_s_S_1_ctMethod));
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

    private static UIElement e_94r_0_s_S_1_ctMethod(UIElement parent)
    {
      Border border = new Border();
      border.Parent = parent;
      border.Name = "e_97";
      border.SetBinding(Control.BackgroundProperty, new Binding("Background")
      {
        Source = (object) parent
      });
      ContentPresenter contentPresenter = new ContentPresenter();
      border.Child = (UIElement) contentPresenter;
      contentPresenter.Name = "e_98";
      contentPresenter.HorizontalAlignment = HorizontalAlignment.Stretch;
      contentPresenter.VerticalAlignment = VerticalAlignment.Stretch;
      Binding binding = new Binding();
      contentPresenter.SetBinding(ContentPresenter.ContentProperty, binding);
      return (UIElement) border;
    }

    private static ObservableCollection<object> Get_e_124_Items()
    {
      ObservableCollection<object> observableCollection = new ObservableCollection<object>();
      TabItem tabItem1 = new TabItem();
      tabItem1.Name = "e_125";
      tabItem1.SetResourceReference(HeaderedContentControl.HeaderProperty, (object) "StoreAdministration_NewOffer");
      Grid grid1 = new Grid();
      tabItem1.Content = (object) grid1;
      grid1.Name = "e_126";
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
      grid1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      ListBox listBox1 = new ListBox();
      grid1.Children.Add((UIElement) listBox1);
      listBox1.Name = "e_127";
      listBox1.Margin = new Thickness(0.0f, 10f, 10f, 10f);
      listBox1.Background = (Brush) new SolidColorBrush(new ColorW(41, 54, 62, (int) byte.MaxValue));
      listBox1.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      listBox1.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      listBox1.TabIndex = 130;
      Grid.SetColumn((UIElement) listBox1, 0);
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
      grid2.Name = "e_128";
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
      textBlock1.Name = "e_129";
      textBlock1.Margin = new Thickness(5f, 5f, 5f, 5f);
      textBlock1.VerticalAlignment = VerticalAlignment.Center;
      textBlock1.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      textBlock1.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlockView_AmountLabel");
      NumericTextBox numericTextBox1 = new NumericTextBox();
      grid2.Children.Add((UIElement) numericTextBox1);
      numericTextBox1.Name = "e_130";
      numericTextBox1.Margin = new Thickness(5f, 5f, 5f, 5f);
      numericTextBox1.VerticalAlignment = VerticalAlignment.Center;
      numericTextBox1.TabIndex = 131;
      numericTextBox1.MaxLength = 5;
      numericTextBox1.Minimum = 0.0f;
      numericTextBox1.Maximum = 99999f;
      Grid.SetColumn((UIElement) numericTextBox1, 1);
      GamepadHelp.SetTabIndexLeft((DependencyObject) numericTextBox1, 30);
      GamepadHelp.SetTabIndexUp((DependencyObject) numericTextBox1, 130);
      GamepadHelp.SetTabIndexDown((DependencyObject) numericTextBox1, 132);
      numericTextBox1.SetBinding(NumericTextBox.ValueProperty, new Binding("OfferAmount")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock2 = new TextBlock();
      grid2.Children.Add((UIElement) textBlock2);
      textBlock2.Name = "e_131";
      textBlock2.Margin = new Thickness(5f, 5f, 5f, 5f);
      textBlock2.VerticalAlignment = VerticalAlignment.Center;
      textBlock2.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetRow((UIElement) textBlock2, 1);
      textBlock2.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlockView_PricePerUnitLabel");
      NumericTextBox numericTextBox2 = new NumericTextBox();
      grid2.Children.Add((UIElement) numericTextBox2);
      numericTextBox2.Name = "e_132";
      numericTextBox2.Margin = new Thickness(5f, 5f, 5f, 5f);
      numericTextBox2.VerticalAlignment = VerticalAlignment.Center;
      numericTextBox2.TabIndex = 132;
      numericTextBox2.MaxLength = 9;
      numericTextBox2.Maximum = 1E+09f;
      Grid.SetColumn((UIElement) numericTextBox2, 1);
      Grid.SetRow((UIElement) numericTextBox2, 1);
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
      textBlock3.Name = "e_133";
      textBlock3.Margin = new Thickness(5f, 5f, 5f, 5f);
      textBlock3.VerticalAlignment = VerticalAlignment.Center;
      textBlock3.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock3, 0);
      Grid.SetRow((UIElement) textBlock3, 2);
      textBlock3.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlockView_TotalPriceLabel");
      StackPanel stackPanel1 = new StackPanel();
      grid2.Children.Add((UIElement) stackPanel1);
      stackPanel1.Name = "e_134";
      stackPanel1.Margin = new Thickness(5f, 5f, 5f, 5f);
      stackPanel1.HorizontalAlignment = HorizontalAlignment.Right;
      stackPanel1.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel1, 1);
      Grid.SetRow((UIElement) stackPanel1, 2);
      TextBlock textBlock4 = new TextBlock();
      stackPanel1.Children.Add((UIElement) textBlock4);
      textBlock4.Name = "e_135";
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
      image1.Name = "e_136";
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
      textBlock5.Name = "e_137";
      textBlock5.Margin = new Thickness(5f, 5f, 5f, 5f);
      textBlock5.VerticalAlignment = VerticalAlignment.Center;
      textBlock5.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock5, 0);
      Grid.SetRow((UIElement) textBlock5, 3);
      textBlock5.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlockView_ListingFee");
      StackPanel stackPanel2 = new StackPanel();
      grid2.Children.Add((UIElement) stackPanel2);
      stackPanel2.Name = "e_138";
      stackPanel2.Margin = new Thickness(5f, 5f, 5f, 5f);
      stackPanel2.HorizontalAlignment = HorizontalAlignment.Right;
      stackPanel2.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel2, 1);
      Grid.SetRow((UIElement) stackPanel2, 3);
      TextBlock textBlock6 = new TextBlock();
      stackPanel2.Children.Add((UIElement) textBlock6);
      textBlock6.Name = "e_139";
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
      image2.Name = "e_140";
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
      textBlock7.Name = "e_141";
      textBlock7.Margin = new Thickness(5f, 5f, 5f, 5f);
      textBlock7.VerticalAlignment = VerticalAlignment.Center;
      textBlock7.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock7, 0);
      Grid.SetRow((UIElement) textBlock7, 4);
      textBlock7.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlockView_TransactionFee");
      StackPanel stackPanel3 = new StackPanel();
      grid2.Children.Add((UIElement) stackPanel3);
      stackPanel3.Name = "e_142";
      stackPanel3.Margin = new Thickness(5f, 5f, 5f, 5f);
      stackPanel3.HorizontalAlignment = HorizontalAlignment.Right;
      stackPanel3.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel3, 1);
      Grid.SetRow((UIElement) stackPanel3, 4);
      TextBlock textBlock8 = new TextBlock();
      stackPanel3.Children.Add((UIElement) textBlock8);
      textBlock8.Name = "e_143";
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
      image3.Name = "e_144";
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
      grid3.Name = "e_145";
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
      textBlock9.Name = "e_146";
      textBlock9.Margin = new Thickness(0.0f, 5f, 5f, 5f);
      textBlock9.VerticalAlignment = VerticalAlignment.Bottom;
      textBlock9.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      textBlock9.SetResourceReference(TextBlock.TextProperty, (object) "Currency_Default_Account_Label");
      TextBlock textBlock10 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock10);
      textBlock10.Name = "e_147";
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
      image4.Name = "e_148";
      image4.Height = 20f;
      image4.Margin = new Thickness(4f, 2f, 2f, 2f);
      image4.VerticalAlignment = VerticalAlignment.Bottom;
      image4.Stretch = Stretch.Uniform;
      Grid.SetColumn((UIElement) image4, 2);
      image4.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      Button button1 = new Button();
      grid1.Children.Add((UIElement) button1);
      button1.Name = "e_149";
      button1.Width = 150f;
      button1.Margin = new Thickness(10f, 10f, 0.0f, 10f);
      button1.HorizontalAlignment = HorizontalAlignment.Right;
      button1.VerticalAlignment = VerticalAlignment.Center;
      button1.TabIndex = 133;
      Grid.SetColumn((UIElement) button1, 0);
      Grid.SetRow((UIElement) button1, 3);
      GamepadHelp.SetTabIndexLeft((DependencyObject) button1, 30);
      GamepadHelp.SetTabIndexUp((DependencyObject) button1, 132);
      button1.SetBinding(UIElement.IsEnabledProperty, new Binding("IsCreateOfferEnabled")
      {
        UseGeneratedBindings = true
      });
      button1.SetBinding(Button.CommandProperty, new Binding("CreateOfferCommand")
      {
        UseGeneratedBindings = true
      });
      button1.SetResourceReference(ContentControl.ContentProperty, (object) "StoreBlockView_CreateOfferButton");
      observableCollection.Add((object) tabItem1);
      TabItem tabItem2 = new TabItem();
      tabItem2.Name = "e_150";
      tabItem2.SetBinding(UIElement.VisibilityProperty, new Binding("IsTabNewOrderVisible")
      {
        UseGeneratedBindings = true
      });
      tabItem2.SetResourceReference(HeaderedContentControl.HeaderProperty, (object) "StoreAdministration_NewOrder");
      Grid grid4 = new Grid();
      tabItem2.Content = (object) grid4;
      grid4.Name = "e_151";
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
      grid4.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      ListBox listBox2 = new ListBox();
      grid4.Children.Add((UIElement) listBox2);
      listBox2.Name = "e_152";
      listBox2.Margin = new Thickness(0.0f, 10f, 10f, 10f);
      listBox2.Background = (Brush) new SolidColorBrush(new ColorW(41, 54, 62, (int) byte.MaxValue));
      listBox2.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      listBox2.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      listBox2.TabIndex = 230;
      Grid.SetColumn((UIElement) listBox2, 0);
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
      grid5.Name = "e_153";
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
      textBlock11.Name = "e_154";
      textBlock11.Margin = new Thickness(5f, 5f, 5f, 5f);
      textBlock11.VerticalAlignment = VerticalAlignment.Center;
      textBlock11.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock11, 0);
      textBlock11.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlockView_AmountLabel");
      NumericTextBox numericTextBox3 = new NumericTextBox();
      grid5.Children.Add((UIElement) numericTextBox3);
      numericTextBox3.Name = "e_155";
      numericTextBox3.Margin = new Thickness(5f, 5f, 5f, 5f);
      numericTextBox3.VerticalAlignment = VerticalAlignment.Center;
      numericTextBox3.TabIndex = 231;
      numericTextBox3.MaxLength = 5;
      numericTextBox3.Minimum = 0.0f;
      numericTextBox3.Maximum = 99999f;
      Grid.SetColumn((UIElement) numericTextBox3, 1);
      GamepadHelp.SetTabIndexLeft((DependencyObject) numericTextBox3, 30);
      GamepadHelp.SetTabIndexUp((DependencyObject) numericTextBox3, 230);
      GamepadHelp.SetTabIndexDown((DependencyObject) numericTextBox3, 232);
      numericTextBox3.SetBinding(NumericTextBox.ValueProperty, new Binding("OrderAmount")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock12 = new TextBlock();
      grid5.Children.Add((UIElement) textBlock12);
      textBlock12.Name = "e_156";
      textBlock12.Margin = new Thickness(5f, 5f, 5f, 5f);
      textBlock12.VerticalAlignment = VerticalAlignment.Center;
      textBlock12.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetRow((UIElement) textBlock12, 1);
      textBlock12.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlockView_PricePerUnitLabel");
      NumericTextBox numericTextBox4 = new NumericTextBox();
      grid5.Children.Add((UIElement) numericTextBox4);
      numericTextBox4.Name = "e_157";
      numericTextBox4.Margin = new Thickness(5f, 5f, 5f, 5f);
      numericTextBox4.VerticalAlignment = VerticalAlignment.Center;
      numericTextBox4.TabIndex = 232;
      numericTextBox4.MaxLength = 9;
      numericTextBox4.Minimum = 0.0f;
      Grid.SetColumn((UIElement) numericTextBox4, 1);
      Grid.SetRow((UIElement) numericTextBox4, 1);
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
      textBlock13.Name = "e_158";
      textBlock13.Margin = new Thickness(5f, 5f, 5f, 5f);
      textBlock13.VerticalAlignment = VerticalAlignment.Center;
      textBlock13.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock13, 0);
      Grid.SetRow((UIElement) textBlock13, 2);
      textBlock13.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlockView_TotalPriceLabel");
      StackPanel stackPanel4 = new StackPanel();
      grid5.Children.Add((UIElement) stackPanel4);
      stackPanel4.Name = "e_159";
      stackPanel4.Margin = new Thickness(5f, 5f, 5f, 5f);
      stackPanel4.HorizontalAlignment = HorizontalAlignment.Right;
      stackPanel4.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel4, 1);
      Grid.SetRow((UIElement) stackPanel4, 2);
      TextBlock textBlock14 = new TextBlock();
      stackPanel4.Children.Add((UIElement) textBlock14);
      textBlock14.Name = "e_160";
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
      image5.Name = "e_161";
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
      textBlock15.Name = "e_162";
      textBlock15.Margin = new Thickness(5f, 5f, 5f, 5f);
      textBlock15.VerticalAlignment = VerticalAlignment.Center;
      textBlock15.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock15, 0);
      Grid.SetRow((UIElement) textBlock15, 3);
      textBlock15.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlockView_ListingFee");
      StackPanel stackPanel5 = new StackPanel();
      grid5.Children.Add((UIElement) stackPanel5);
      stackPanel5.Name = "e_163";
      stackPanel5.Margin = new Thickness(5f, 5f, 5f, 5f);
      stackPanel5.HorizontalAlignment = HorizontalAlignment.Right;
      stackPanel5.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel5, 1);
      Grid.SetRow((UIElement) stackPanel5, 3);
      TextBlock textBlock16 = new TextBlock();
      stackPanel5.Children.Add((UIElement) textBlock16);
      textBlock16.Name = "e_164";
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
      image6.Name = "e_165";
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
      grid6.Name = "e_166";
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
      textBlock17.Name = "e_167";
      textBlock17.Margin = new Thickness(0.0f, 5f, 5f, 5f);
      textBlock17.VerticalAlignment = VerticalAlignment.Bottom;
      textBlock17.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      textBlock17.SetResourceReference(TextBlock.TextProperty, (object) "Currency_Default_Account_Label");
      TextBlock textBlock18 = new TextBlock();
      grid6.Children.Add((UIElement) textBlock18);
      textBlock18.Name = "e_168";
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
      image7.Name = "e_169";
      image7.Height = 20f;
      image7.Margin = new Thickness(4f, 2f, 2f, 2f);
      image7.VerticalAlignment = VerticalAlignment.Bottom;
      image7.Stretch = Stretch.Uniform;
      Grid.SetColumn((UIElement) image7, 2);
      image7.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      Button button2 = new Button();
      grid4.Children.Add((UIElement) button2);
      button2.Name = "e_170";
      button2.Width = 150f;
      button2.Margin = new Thickness(10f, 10f, 0.0f, 10f);
      button2.HorizontalAlignment = HorizontalAlignment.Right;
      button2.VerticalAlignment = VerticalAlignment.Center;
      button2.TabIndex = 233;
      Grid.SetColumn((UIElement) button2, 0);
      Grid.SetRow((UIElement) button2, 3);
      GamepadHelp.SetTabIndexLeft((DependencyObject) button2, 30);
      GamepadHelp.SetTabIndexUp((DependencyObject) button2, 232);
      button2.SetBinding(UIElement.IsEnabledProperty, new Binding("IsCreateOrderEnabled")
      {
        UseGeneratedBindings = true
      });
      button2.SetBinding(Button.CommandProperty, new Binding("CreateOrderCommand")
      {
        UseGeneratedBindings = true
      });
      button2.SetResourceReference(ContentControl.ContentProperty, (object) "StoreBlockView_CreateOrderButton");
      observableCollection.Add((object) tabItem2);
      return observableCollection;
    }

    private static void InitializeElemente_124Resources(UIElement elem)
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
