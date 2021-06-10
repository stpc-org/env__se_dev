// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.ViewModels.MyStoreBlockAdministrationViewModel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Game.World.Generator;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using VRage;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.Screens.ViewModels
{
  public class MyStoreBlockAdministrationViewModel : ViewModelBase
  {
    private MyStoreBlock m_storeBlock;
    private bool m_isCreateOfferEnabled;
    private bool m_isCreateOrderEnabled;
    private bool m_isTabNewOrderVisible = true;
    private float m_offerAmount;
    private float m_offerPricePerUnit;
    private float m_offerAmountMaximum;
    private float m_offerPricePerUnitMinimum;
    private float m_orderAmount;
    private float m_orderPricePerUnitMaximum;
    private float m_orderPricePerUnit;
    private string m_orderTotalPrice;
    private string m_offerTotalPrice;
    private string m_offerListingFee;
    private string m_orderListingFee;
    private string m_offerTransactionFee;
    private string m_localPlayerCurrency;
    public int m_orderCount;
    public int m_offerCount;
    public int m_orderOfferCountMax;
    private ICommand m_createOfferCommand;
    private ICommand m_createOrderCommand;
    private ICommand m_removeStoreItemCommand;
    private MyOrderItemModel m_selectedOrderItem;
    private MyStoreItemModel m_selectedStoreItem;
    private MyOrderItemModel m_selectedOfferItem;
    private ObservableCollection<MyStoreItemModel> m_storeItems = new ObservableCollection<MyStoreItemModel>();
    private ObservableCollection<MyOrderItemModel> m_orderItems = new ObservableCollection<MyOrderItemModel>();
    private MyMinimalPriceCalculator m_priceCalculator = new MyMinimalPriceCalculator();
    private MyAccountInfo m_localPlayerAccountInfo;
    private BitmapImage m_currencyIcon;

    public BitmapImage CurrencyIcon
    {
      get => this.m_currencyIcon;
      set => this.SetProperty<BitmapImage>(ref this.m_currencyIcon, value, nameof (CurrencyIcon));
    }

    public bool IsCreateOrderEnabled
    {
      get => this.m_isCreateOrderEnabled;
      set => this.SetProperty<bool>(ref this.m_isCreateOrderEnabled, value, nameof (IsCreateOrderEnabled));
    }

    public bool IsCreateOfferEnabled
    {
      get => this.m_isCreateOfferEnabled;
      set => this.SetProperty<bool>(ref this.m_isCreateOfferEnabled, value, nameof (IsCreateOfferEnabled));
    }

    public float OfferAmount
    {
      get => this.m_offerAmount;
      set
      {
        this.SetProperty<float>(ref this.m_offerAmount, value, nameof (OfferAmount));
        this.UpdateOffer();
      }
    }

    public float OfferPricePerUnit
    {
      get => this.m_offerPricePerUnit;
      set
      {
        this.SetProperty<float>(ref this.m_offerPricePerUnit, value, nameof (OfferPricePerUnit));
        this.UpdateOffer();
      }
    }

    public string OfferTotalPrice
    {
      get => this.m_offerTotalPrice;
      set => this.SetProperty<string>(ref this.m_offerTotalPrice, value, nameof (OfferTotalPrice));
    }

    public float OfferAmountMaximum
    {
      get => this.m_offerAmountMaximum;
      set => this.SetProperty<float>(ref this.m_offerAmountMaximum, value, nameof (OfferAmountMaximum));
    }

    public float OfferPricePerUnitMinimum
    {
      get => this.m_offerPricePerUnitMinimum;
      set => this.SetProperty<float>(ref this.m_offerPricePerUnitMinimum, value, nameof (OfferPricePerUnitMinimum));
    }

    public string OfferListingFee
    {
      get => this.m_offerListingFee;
      set => this.SetProperty<string>(ref this.m_offerListingFee, value, nameof (OfferListingFee));
    }

    public string OfferTransactionFee
    {
      get => this.m_offerTransactionFee;
      set => this.SetProperty<string>(ref this.m_offerTransactionFee, value, nameof (OfferTransactionFee));
    }

    public float OrderAmount
    {
      get => this.m_orderAmount;
      set
      {
        this.SetProperty<float>(ref this.m_orderAmount, value, nameof (OrderAmount));
        this.UpdateOrder();
      }
    }

    public float OrderPricePerUnitMaximum
    {
      get => this.m_orderPricePerUnitMaximum;
      set => this.SetProperty<float>(ref this.m_orderPricePerUnitMaximum, value, nameof (OrderPricePerUnitMaximum));
    }

    public float OrderPricePerUnit
    {
      get => this.m_orderPricePerUnit;
      set
      {
        this.SetProperty<float>(ref this.m_orderPricePerUnit, value, nameof (OrderPricePerUnit));
        this.UpdateOrder();
      }
    }

    public string OrderTotalPrice
    {
      get => this.m_orderTotalPrice;
      set => this.SetProperty<string>(ref this.m_orderTotalPrice, value, nameof (OrderTotalPrice));
    }

    public string OrderListingFee
    {
      get => this.m_orderListingFee;
      set => this.SetProperty<string>(ref this.m_orderListingFee, value, nameof (OrderListingFee));
    }

    public ICommand CreateOfferCommand
    {
      get => this.m_createOfferCommand;
      set => this.SetProperty<ICommand>(ref this.m_createOfferCommand, value, nameof (CreateOfferCommand));
    }

    public ICommand CreateOrderCommand
    {
      get => this.m_createOrderCommand;
      set => this.SetProperty<ICommand>(ref this.m_createOrderCommand, value, nameof (CreateOrderCommand));
    }

    public ICommand RemoveStoreItemCommand
    {
      get => this.m_removeStoreItemCommand;
      set => this.SetProperty<ICommand>(ref this.m_removeStoreItemCommand, value, nameof (RemoveStoreItemCommand));
    }

    public ObservableCollection<MyOrderItemModel> OrderItems
    {
      get => this.m_orderItems;
      set => this.SetProperty<ObservableCollection<MyOrderItemModel>>(ref this.m_orderItems, value, nameof (OrderItems));
    }

    public MyOrderItemModel SelectedOrderItem
    {
      get => this.m_selectedOrderItem;
      set
      {
        this.SetProperty<MyOrderItemModel>(ref this.m_selectedOrderItem, value, nameof (SelectedOrderItem));
        this.UpdateMaximumOrderPrice();
      }
    }

    public MyOrderItemModel SelectedOfferItem
    {
      get => this.m_selectedOfferItem;
      set
      {
        this.SetProperty<MyOrderItemModel>(ref this.m_selectedOfferItem, value, nameof (SelectedOfferItem));
        this.UpdateMinimumOfferPrice();
      }
    }

    public string LocalPlayerCurrency
    {
      get => this.m_localPlayerCurrency;
      set => this.SetProperty<string>(ref this.m_localPlayerCurrency, value, nameof (LocalPlayerCurrency));
    }

    public int OrderCount
    {
      get => this.m_orderCount;
      set => this.SetProperty<int>(ref this.m_orderCount, value, nameof (OrderCount));
    }

    public int OfferCount
    {
      get => this.m_offerCount;
      set => this.SetProperty<int>(ref this.m_offerCount, value, nameof (OfferCount));
    }

    public int OrderOfferCountMax
    {
      get => this.m_orderOfferCountMax;
      set => this.SetProperty<int>(ref this.m_orderOfferCountMax, value, nameof (OrderOfferCountMax));
    }

    public ObservableCollection<MyStoreItemModel> StoreItems
    {
      get => this.m_storeItems;
      set => this.SetProperty<ObservableCollection<MyStoreItemModel>>(ref this.m_storeItems, value, nameof (StoreItems));
    }

    public MyStoreItemModel SelectedStoreItem
    {
      get => this.m_selectedStoreItem;
      set => this.SetProperty<MyStoreItemModel>(ref this.m_selectedStoreItem, value, nameof (SelectedStoreItem));
    }

    public bool IsTabNewOrderVisible
    {
      get => this.m_isTabNewOrderVisible;
      set => this.SetProperty<bool>(ref this.m_isTabNewOrderVisible, value, nameof (IsTabNewOrderVisible));
    }

    public event EventHandler OfferCreated;

    public event EventHandler OrderCreated;

    public event EventHandler StoreItemRemoved;

    public MyStoreBlockAdministrationViewModel(MyStoreBlock storeBlock)
    {
      this.m_storeBlock = storeBlock;
      this.CreateOfferCommand = (ICommand) new RelayCommand(new Action<object>(this.OnCreateOffer));
      this.CreateOrderCommand = (ICommand) new RelayCommand(new Action<object>(this.OnCreateOrder));
      this.RemoveStoreItemCommand = (ICommand) new RelayCommand(new Action<object>(this.OnRemoveStoreItem));
      this.OrderOfferCountMax = MySession.Static.GetComponent<MySessionComponentEconomy>().GetStoreCreationLimitPerPlayer();
      this.UpdateLocalPlayerAccountBalance();
      BitmapImage bitmapImage = new BitmapImage();
      string[] icons = MyBankingSystem.BankingSystemDefinition.Icons;
      bitmapImage.TextureAsset = (icons != null ? ((uint) icons.Length > 0U ? 1 : 0) : 0) != 0 ? MyBankingSystem.BankingSystemDefinition.Icons[0] : string.Empty;
      this.CurrencyIcon = bitmapImage;
    }

    internal void Initialize()
    {
      List<MyOrderItemModel> list = new List<MyOrderItemModel>();
      foreach (MyPhysicalItemDefinition physicalItemDefinition in MyDefinitionManager.Static.GetPhysicalItemDefinitions())
      {
        if (physicalItemDefinition.Public && physicalItemDefinition.Enabled && physicalItemDefinition.CanPlayerOrder)
          list.Add(new MyOrderItemModel(physicalItemDefinition));
      }
      list.SortNoAlloc<MyOrderItemModel>((Comparison<MyOrderItemModel>) ((x, y) => string.Compare(x.Name, y.Name, StringComparison.InvariantCultureIgnoreCase)));
      this.OrderItems = new ObservableCollection<MyOrderItemModel>(list);
    }

    private void OnCreateOffer(object obj)
    {
      if (this.SelectedOfferItem == null)
        return;
      if (this.OrderCount + this.OfferCount >= this.OrderOfferCountMax)
      {
        this.ShowErrorFailNotification(MyStoreCreationResult.Fail_CreationLimitHard);
      }
      else
      {
        int minimumOfferPrice = this.GetMinimumOfferPrice();
        if (float.IsNaN(this.OfferPricePerUnit) || (double) minimumOfferPrice > (double) this.OfferPricePerUnit)
        {
          string formatedValue = MyBankingSystem.GetFormatedValue((long) minimumOfferPrice);
          StringBuilder text = new StringBuilder().AppendFormat(MySpaceTexts.StoreBuy_Error_Text_WrongOfferPricePerUnit, (object) formatedValue);
          this.ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_WrongOfferPricePerUnit), text);
        }
        else
          this.m_storeBlock.CreateNewOfferRequest((SerializableDefinitionId) this.SelectedOfferItem.ItemDefinition.Id, (int) this.OfferAmount, (int) this.OfferPricePerUnit, new Action<MyStoreCreationResult>(this.OnCreateOfferResult));
      }
    }

    private void ShowMessageBoxError(StringBuilder caption, StringBuilder text)
    {
      StringBuilder messageCaption = caption;
      MyScreenManager.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: text, messageCaption: messageCaption, canHideOthers: false));
    }

    private void OnCreateOfferResult(MyStoreCreationResult result)
    {
      if (result == MyStoreCreationResult.Success)
      {
        this.UpdateLocalPlayerAccountBalance();
        EventHandler offerCreated = this.OfferCreated;
        if (offerCreated == null)
          return;
        offerCreated((object) this, System.EventArgs.Empty);
      }
      else
        this.ShowErrorFailNotification(result);
    }

    private void UpdateLocalPlayerAccountBalance()
    {
      MyBankingSystem.Static.TryGetAccountInfo(MySession.Static.LocalPlayerId, out this.m_localPlayerAccountInfo);
      this.LocalPlayerCurrency = MyBankingSystem.GetFormatedValue(this.m_localPlayerAccountInfo.Balance);
    }

    private void UpdateMinimumOfferPrice()
    {
      int minimumOfferPrice = this.GetMinimumOfferPrice();
      this.OfferPricePerUnitMinimum = (float) minimumOfferPrice;
      if (!float.IsNaN(this.OfferPricePerUnit) && (double) minimumOfferPrice <= (double) this.OfferPricePerUnit)
        return;
      this.OfferPricePerUnit = (float) minimumOfferPrice;
    }

    private int GetMinimumOfferPrice()
    {
      if (this.SelectedOfferItem == null)
        return 0;
      int num = 0;
      MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
      if (component != null)
      {
        MyDefinitionId id = this.SelectedOfferItem.ItemDefinition.Id;
        num = component.GetMinimumItemPrice((SerializableDefinitionId) id);
      }
      return num;
    }

    private void UpdateOffer()
    {
      long valueToFormat = 0;
      if (!float.IsNaN(this.OfferAmount) && !float.IsNaN(this.OfferPricePerUnit))
        valueToFormat = (long) ((double) this.OfferAmount * (double) this.OfferPricePerUnit);
      if (valueToFormat < 0L)
        valueToFormat = 0L;
      MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
      if (component != null)
      {
        this.OfferListingFee = MyBankingSystem.GetFormatedValue((long) ((double) valueToFormat * (double) component.EconomyDefinition.ListingFee));
        this.OfferTransactionFee = MyBankingSystem.GetFormatedValue((long) ((double) valueToFormat * (double) component.EconomyDefinition.TransactionFee));
      }
      this.IsCreateOfferEnabled = valueToFormat > 0L && this.SelectedOfferItem != null;
      this.OfferTotalPrice = MyBankingSystem.GetFormatedValue(valueToFormat);
    }

    private void UpdateMaximumOrderPrice() => this.OrderPricePerUnitMaximum = 1E+09f;

    private void UpdateOrder()
    {
      long valueToFormat = 0;
      if (!float.IsNaN(this.OrderAmount) && !float.IsNaN(this.OrderPricePerUnit))
        valueToFormat = (long) ((double) this.OrderAmount * (double) this.OrderPricePerUnit);
      if (valueToFormat < 0L)
        valueToFormat = 0L;
      MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
      if (component != null)
        this.OrderListingFee = MyBankingSystem.GetFormatedValue((long) ((double) valueToFormat * (double) component.EconomyDefinition.ListingFee));
      this.IsCreateOrderEnabled = valueToFormat > 0L && this.SelectedOrderItem != null;
      this.OrderTotalPrice = MyBankingSystem.GetFormatedValue(valueToFormat);
    }

    private void OnCreateOrder(object obj)
    {
      if (this.SelectedOrderItem == null)
        return;
      if (this.OrderCount + this.OfferCount >= this.OrderOfferCountMax)
        this.ShowErrorFailNotification(MyStoreCreationResult.Fail_CreationLimitHard);
      else
        this.m_storeBlock.CreateNewOrderRequest((SerializableDefinitionId) this.SelectedOrderItem.ItemDefinition.Id, (int) this.OrderAmount, (int) this.OrderPricePerUnit, new Action<MyStoreCreationResult>(this.OnCreateOrderResult));
    }

    private void OnCreateOrderResult(MyStoreCreationResult result)
    {
      if (result == MyStoreCreationResult.Success)
      {
        this.UpdateLocalPlayerAccountBalance();
        EventHandler orderCreated = this.OrderCreated;
        if (orderCreated == null)
          return;
        orderCreated((object) this, System.EventArgs.Empty);
      }
      else
        this.ShowErrorFailNotification(result);
    }

    private void ShowErrorFailNotification(MyStoreCreationResult state)
    {
      switch (state)
      {
        case MyStoreCreationResult.Fail_CreationLimitHard:
          this.ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Store_Error_Caption_OrderOfferLimitReachedHard), MyTexts.Get(MySpaceTexts.Store_Error_Text_OrderOfferLimitReachedHard));
          break;
        case MyStoreCreationResult.Error:
          MyLog.Default.Error(new StringBuilder("Contracts - error result: " + state.ToString()));
          break;
      }
    }

    private void ShowMessageBoxOk(StringBuilder caption, StringBuilder text)
    {
      StringBuilder messageCaption = caption;
      MyScreenManager.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: text, messageCaption: messageCaption, canHideOthers: false));
    }

    private void ShowMessageBoxYesNo(
      StringBuilder caption,
      StringBuilder text,
      Action<MyGuiScreenMessageBox.ResultEnum> callback)
    {
      StringBuilder messageCaption = caption;
      MyScreenManager.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: text, messageCaption: messageCaption, callback: callback, canHideOthers: false));
    }

    private void OnRemoveStoreItem(object obj)
    {
      if (this.SelectedStoreItem == null)
        return;
      this.m_storeBlock.CreateCancelStoreItemRequest(this.SelectedStoreItem.Id, new Action<bool>(this.OnCancelStoreItemResult));
    }

    private void OnCancelStoreItemResult(bool result)
    {
      EventHandler storeItemRemoved = this.StoreItemRemoved;
      if (storeItemRemoved == null)
        return;
      storeItemRemoved((object) this, System.EventArgs.Empty);
    }
  }
}
