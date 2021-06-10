// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.ViewModels.MyStoreBlockViewModel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.ObjectBuilders.Definitions;

namespace Sandbox.Game.Screens.ViewModels
{
  public class MyStoreBlockViewModel : MyViewModelBase, IMyStoreBlockViewModel
  {
    private bool m_isBuyEnabled;
    private bool m_isSellEnabled;
    private bool m_isRefreshEnabled;
    private bool m_isAdministrationVisible;
    private bool m_isPreviewEnabled;
    private bool m_isBuyTabVisible = true;
    private bool m_isSellTabVisible = true;
    private long m_lastEconomyTick;
    private float m_amountToSell;
    private float m_amountToBuy;
    private float m_amountToBuyMaximum;
    private int m_tabSelectedIndex;
    private string m_totalPriceToBuy;
    private string m_totalPriceToSell;
    private string m_sortModeOffersText;
    private string m_sortModeOrdersText;
    private MyStoreBlockViewModel.SortBy m_sortModeOffers;
    private MyStoreBlockViewModel.SortBy m_sortModeOrders;
    private ICommand m_refreshCommand;
    private ICommand m_sellCommand;
    private ICommand m_buyCommand;
    private ICommand m_sortingOfferedItemsCommand;
    private ICommand m_sortingDemandedItemsCommand;
    private ICommand m_onBuyItemDoubleClickCommand;
    private ICommand m_setAllAmountOfferCommand;
    private ICommand m_setAllAmountOrderCommand;
    private ICommand m_showPreviewCommand;
    private ICommand m_switchSortOffersCommand;
    private ICommand m_previousInventoryCommand;
    private ICommand m_nextInventoryCommand;
    private ICommand m_switchSortOrdersCommand;
    private ObservableCollection<MyStoreItemModel> m_offeredItems = new ObservableCollection<MyStoreItemModel>();
    private ObservableCollection<MyStoreItemModel> m_orderedItems = new ObservableCollection<MyStoreItemModel>();
    private MyStoreItemModel m_selectedOfferItem;
    private MyStoreItemModel m_selectedOrderItem;
    private MyStoreBlock m_storeBlock;

    public bool IsPreviewEnabled
    {
      get => this.m_isPreviewEnabled;
      set => this.SetProperty<bool>(ref this.m_isPreviewEnabled, value, nameof (IsPreviewEnabled));
    }

    public bool IsAdministrationVisible
    {
      get => this.m_isAdministrationVisible;
      set => this.SetProperty<bool>(ref this.m_isAdministrationVisible, value, nameof (IsAdministrationVisible));
    }

    public bool IsBuyTabVisible
    {
      get => this.m_isBuyTabVisible;
      set => this.SetProperty<bool>(ref this.m_isBuyTabVisible, value, nameof (IsBuyTabVisible));
    }

    public bool IsSellTabVisible
    {
      get => this.m_isSellTabVisible;
      set => this.SetProperty<bool>(ref this.m_isSellTabVisible, value, nameof (IsSellTabVisible));
    }

    public int TabSelectedIndex
    {
      get => this.m_tabSelectedIndex;
      set
      {
        if (this.m_tabSelectedIndex == value)
          return;
        this.SetProperty<int>(ref this.m_tabSelectedIndex, value, nameof (TabSelectedIndex));
      }
    }

    public bool IsBuyEnabled
    {
      get => this.m_isBuyEnabled;
      set => this.SetProperty<bool>(ref this.m_isBuyEnabled, value, nameof (IsBuyEnabled));
    }

    public bool IsSellEnabled
    {
      get => this.m_isSellEnabled;
      set => this.SetProperty<bool>(ref this.m_isSellEnabled, value, nameof (IsSellEnabled));
    }

    public bool IsRefreshEnabled
    {
      get => this.m_isRefreshEnabled;
      set => this.SetProperty<bool>(ref this.m_isRefreshEnabled, value, nameof (IsRefreshEnabled));
    }

    public MyStoreItemModel SelectedOfferItem
    {
      get => this.m_selectedOfferItem;
      set
      {
        this.SetProperty<MyStoreItemModel>(ref this.m_selectedOfferItem, value, nameof (SelectedOfferItem));
        this.UpdateOfferedInfo();
      }
    }

    public MyStoreItemModel SelectedOrderItem
    {
      get => this.m_selectedOrderItem;
      set
      {
        this.SetProperty<MyStoreItemModel>(ref this.m_selectedOrderItem, value, nameof (SelectedOrderItem));
        this.UpdateOrderedInfo();
      }
    }

    public string TotalPriceToSell
    {
      get => this.m_totalPriceToSell;
      set => this.SetProperty<string>(ref this.m_totalPriceToSell, value, nameof (TotalPriceToSell));
    }

    public string TotalPriceToBuy
    {
      get => this.m_totalPriceToBuy;
      set => this.SetProperty<string>(ref this.m_totalPriceToBuy, value, nameof (TotalPriceToBuy));
    }

    public float AmountToSell
    {
      get => this.m_amountToSell;
      set
      {
        if (this.SelectedOrderItem != null)
          this.UpdateTotalPriceToSell(value);
        this.SetProperty<float>(ref this.m_amountToSell, value, nameof (AmountToSell));
      }
    }

    public float AmountToBuyMaximum
    {
      get => this.m_amountToBuyMaximum;
      set => this.SetProperty<float>(ref this.m_amountToBuyMaximum, value, nameof (AmountToBuyMaximum));
    }

    public float AmountToBuy
    {
      get => this.m_amountToBuy;
      set
      {
        if (this.SelectedOfferItem != null)
          this.UpdateTotalPriceToBuy(value);
        this.SetProperty<float>(ref this.m_amountToBuy, value, nameof (AmountToBuy));
      }
    }

    public ICommand SellCommand
    {
      get => this.m_sellCommand;
      set => this.SetProperty<ICommand>(ref this.m_sellCommand, value, nameof (SellCommand));
    }

    public ICommand BuyCommand
    {
      get => this.m_buyCommand;
      set => this.SetProperty<ICommand>(ref this.m_buyCommand, value, nameof (BuyCommand));
    }

    public ICommand SortingOfferedItemsCommand
    {
      get => this.m_sortingOfferedItemsCommand;
      set => this.SetProperty<ICommand>(ref this.m_sortingOfferedItemsCommand, value, nameof (SortingOfferedItemsCommand));
    }

    public ICommand SortingDemandedItemsCommand
    {
      get => this.m_sortingDemandedItemsCommand;
      set => this.SetProperty<ICommand>(ref this.m_sortingDemandedItemsCommand, value, nameof (SortingDemandedItemsCommand));
    }

    public ICommand RefreshCommand
    {
      get => this.m_refreshCommand;
      set => this.SetProperty<ICommand>(ref this.m_refreshCommand, value, nameof (RefreshCommand));
    }

    public ICommand OnBuyItemDoubleClickCommand
    {
      get => this.m_onBuyItemDoubleClickCommand;
      set => this.SetProperty<ICommand>(ref this.m_onBuyItemDoubleClickCommand, value, nameof (OnBuyItemDoubleClickCommand));
    }

    public ICommand SetAllAmountOfferCommand
    {
      get => this.m_setAllAmountOfferCommand;
      set => this.SetProperty<ICommand>(ref this.m_setAllAmountOfferCommand, value, nameof (SetAllAmountOfferCommand));
    }

    public ICommand SetAllAmountOrderCommand
    {
      get => this.m_setAllAmountOrderCommand;
      set => this.SetProperty<ICommand>(ref this.m_setAllAmountOrderCommand, value, nameof (SetAllAmountOrderCommand));
    }

    public ICommand ShowPreviewCommand
    {
      get => this.m_showPreviewCommand;
      set => this.SetProperty<ICommand>(ref this.m_showPreviewCommand, value, nameof (ShowPreviewCommand));
    }

    public ObservableCollection<MyStoreItemModel> OfferedItems
    {
      get => this.m_offeredItems;
      set => this.SetProperty<ObservableCollection<MyStoreItemModel>>(ref this.m_offeredItems, value, nameof (OfferedItems));
    }

    public ObservableCollection<MyStoreItemModel> OrderedItems
    {
      get => this.m_orderedItems;
      set => this.SetProperty<ObservableCollection<MyStoreItemModel>>(ref this.m_orderedItems, value, nameof (OrderedItems));
    }

    public MyInventoryTargetViewModel InventoryTargetViewModel { get; private set; }

    public MyStoreBlockAdministrationViewModel AdministrationViewModel { get; private set; }

    public string SortModeOffersText
    {
      get => this.m_sortModeOffersText;
      set => this.SetProperty<string>(ref this.m_sortModeOffersText, value, nameof (SortModeOffersText));
    }

    public ICommand SwitchSortOffersCommand
    {
      get => this.m_switchSortOffersCommand;
      set => this.SetProperty<ICommand>(ref this.m_switchSortOffersCommand, value, nameof (SwitchSortOffersCommand));
    }

    public ICommand NextInventoryCommand
    {
      get => this.m_nextInventoryCommand;
      set => this.SetProperty<ICommand>(ref this.m_nextInventoryCommand, value, nameof (NextInventoryCommand));
    }

    public ICommand PreviousInventoryCommand
    {
      get => this.m_previousInventoryCommand;
      set => this.SetProperty<ICommand>(ref this.m_previousInventoryCommand, value, nameof (PreviousInventoryCommand));
    }

    public ICommand SwitchSortOrdersCommand
    {
      get => this.m_switchSortOrdersCommand;
      set => this.SetProperty<ICommand>(ref this.m_switchSortOrdersCommand, value, nameof (SwitchSortOrdersCommand));
    }

    public string SortModeOrdersText
    {
      get => this.m_sortModeOrdersText;
      set => this.SetProperty<string>(ref this.m_sortModeOrdersText, value, nameof (SortModeOrdersText));
    }

    public MyStoreBlockViewModel(MyStoreBlock storeBlock)
      : base()
    {
      this.BuyCommand = (ICommand) new RelayCommand(new Action<object>(this.OnBuy));
      this.SellCommand = (ICommand) new RelayCommand(new Action<object>(this.OnSell));
      this.RefreshCommand = (ICommand) new RelayCommand(new Action<object>(this.OnRefresh));
      this.OnBuyItemDoubleClickCommand = (ICommand) new RelayCommand(new Action<object>(this.OnBuyItemDoubleClick));
      this.SetAllAmountOfferCommand = (ICommand) new RelayCommand(new Action<object>(this.OnSetAllAmountOffer));
      this.SetAllAmountOrderCommand = (ICommand) new RelayCommand(new Action<object>(this.OnSetAllAmountOrder));
      this.ShowPreviewCommand = (ICommand) new RelayCommand(new Action<object>(this.OnShowPreview));
      this.NextInventoryCommand = (ICommand) new RelayCommand(new Action<object>(this.OnNextInventory));
      this.PreviousInventoryCommand = (ICommand) new RelayCommand(new Action<object>(this.OnPreviousInventory));
      this.SortingOfferedItemsCommand = (ICommand) new RelayCommand<DataGridSortingEventArgs>(new Action<DataGridSortingEventArgs>(this.OnSortingOfferedItems));
      this.SwitchSortOffersCommand = (ICommand) new RelayCommand(new Action<object>(this.OnSwitchSortOffers));
      this.SortingDemandedItemsCommand = (ICommand) new RelayCommand<DataGridSortingEventArgs>(new Action<DataGridSortingEventArgs>(this.OnSortingDemandedItems));
      this.SwitchSortOrdersCommand = (ICommand) new RelayCommand(new Action<object>(this.OnSwitchSortOrders));
      this.m_storeBlock = storeBlock;
      this.InventoryTargetViewModel = new MyInventoryTargetViewModel(storeBlock);
      this.IsAdministrationVisible = this.m_storeBlock.HasLocalPlayerAccess() && this.m_storeBlock.OwnerId == MySession.Static.LocalPlayerId;
      this.AdministrationViewModel = new MyStoreBlockAdministrationViewModel(this.m_storeBlock);
      if (this.IsAdministrationVisible)
      {
        this.AdministrationViewModel.Initialize();
        this.AdministrationViewModel.OfferCreated += new EventHandler(this.AdministrationViewModel_Changed);
        this.AdministrationViewModel.OrderCreated += new EventHandler(this.AdministrationViewModel_Changed);
        this.AdministrationViewModel.StoreItemRemoved += new EventHandler(this.AdministrationViewModel_Changed);
      }
      this.TotalPriceToBuy = "0";
      this.TotalPriceToSell = "0";
      this.SortModeOffersText = MyTexts.GetString("StoreBlock_Column_" + (object) this.m_sortModeOffers);
      this.SortModeOrdersText = MyTexts.GetString("StoreBlock_Column_" + (object) this.m_sortModeOrders);
      ServiceManager.Instance.AddService<IMyStoreBlockViewModel>((IMyStoreBlockViewModel) this);
    }

    private void OnPreviousInventory(object obj) => this.InventoryTargetViewModel.ShowPreviousInventory();

    private void OnNextInventory(object obj) => this.InventoryTargetViewModel.ShowNextInventory();

    public override void InitializeData()
    {
      this.m_storeBlock.CreateGetConnectedGridInventoriesRequest(new Action<List<long>>(this.OnGetInventories));
      this.RefreshStoreItems();
      if (this.IsBuyTabVisible || this.TabSelectedIndex != 0 || !this.IsAdministrationVisible)
        return;
      this.TabSelectedIndex = 2;
    }

    private void OnShowPreview(object obj)
    {
      if (this.SelectedOfferItem == null)
        return;
      this.m_storeBlock.ShowPreview(this.SelectedOfferItem.Id);
    }

    private void OnSetAllAmountOrder(object obj)
    {
      if (this.SelectedOrderItem == null)
        return;
      float itemAmount = this.InventoryTargetViewModel.GetItemAmount(this.SelectedOrderItem.ItemDefinitionId);
      if ((double) itemAmount == 0.0)
        return;
      this.AmountToSell = Math.Min((float) this.SelectedOrderItem.Amount, itemAmount);
    }

    private void OnSetAllAmountOffer(object obj) => this.AmountToBuy = this.AmountToBuyMaximum;

    private void OnBuyItemDoubleClick(object obj)
    {
      if (!this.IsBuyEnabled)
        return;
      this.OnBuy(obj);
    }

    private void OnGetInventories(List<long> inventoryEntities) => this.InventoryTargetViewModel.Initialize(inventoryEntities, true, false);

    private void UpdateLocalPlayerAccountBalance() => this.InventoryTargetViewModel.UpdateLocalPlayerCurrency();

    private void OnRefresh(object obj) => this.RefreshStoreItems();

    private void RefreshStoreItems()
    {
      this.IsRefreshEnabled = false;
      this.m_storeBlock.CreateGetStoreItemsRequest(MySession.Static.LocalPlayerId, new Action<List<MyStoreItem>, long, float, float>(this.OnGetStoreItems));
    }

    private void OnGetStoreItems(
      List<MyStoreItem> storeItems,
      long lastEconomyTick,
      float offersBonus,
      float ordersBonus)
    {
      this.m_lastEconomyTick = lastEconomyTick;
      this.IsRefreshEnabled = true;
      ObservableCollection<MyStoreItemModel> toSort1 = new ObservableCollection<MyStoreItemModel>();
      ObservableCollection<MyStoreItemModel> toSort2 = new ObservableCollection<MyStoreItemModel>();
      ObservableCollection<MyStoreItemModel> observableCollection = new ObservableCollection<MyStoreItemModel>();
      MyStoreItemModel myStoreItemModel1 = (MyStoreItemModel) null;
      MyStoreItemModel myStoreItemModel2 = (MyStoreItemModel) null;
      foreach (MyStoreItem storeItem in storeItems)
      {
        if (storeItem.Amount != 0)
        {
          MyStoreItemModel myStoreItemModel3 = new MyStoreItemModel()
          {
            Id = storeItem.Id,
            Amount = storeItem.Amount,
            ItemType = storeItem.ItemType,
            StoreItemType = storeItem.StoreItemType
          };
          switch (storeItem.ItemType)
          {
            case ItemTypes.PhysicalItem:
              MyPhysicalItemDefinition definition = (MyPhysicalItemDefinition) null;
              if (MyDefinitionManager.Static.TryGetDefinition<MyPhysicalItemDefinition>((MyDefinitionId) storeItem.Item.Value, out definition))
              {
                myStoreItemModel3.Name = definition.DisplayNameText;
                myStoreItemModel3.Description = definition.DescriptionText;
                string[] icons = definition.Icons;
                if ((icons != null ? ((uint) icons.Length > 0U ? 1 : 0) : 0) != 0)
                  myStoreItemModel3.SetIcon(definition.Icons[0]);
                myStoreItemModel3.IsOre = definition.IsOre;
                myStoreItemModel3.ItemDefinitionId = (MyDefinitionId) storeItem.Item.Value;
                break;
              }
              continue;
            case ItemTypes.Oxygen:
              myStoreItemModel3.Name = MyTexts.GetString(MySpaceTexts.DisplayName_Item_Oxygen);
              myStoreItemModel3.SetIcon("Textures\\GUI\\Icons\\OxygenIcon.dds");
              break;
            case ItemTypes.Hydrogen:
              myStoreItemModel3.Name = MyTexts.GetString(MySpaceTexts.DisplayName_Item_Hydrogen);
              myStoreItemModel3.SetIcon("Textures\\GUI\\Icons\\HydrogenIcon.dds");
              break;
            case ItemTypes.Grid:
              MyPrefabDefinition prefabDefinition = MyDefinitionManager.Static.GetPrefabDefinition(storeItem.PrefabName);
              if (prefabDefinition != null)
              {
                if (!string.IsNullOrEmpty(prefabDefinition.DisplayNameString))
                {
                  myStoreItemModel3.Name = prefabDefinition.DisplayNameString;
                  string[] icons = prefabDefinition.Icons;
                  string iconPath = (icons != null ? ((uint) icons.Length > 0U ? 1 : 0) : 0) != 0 ? prefabDefinition.Icons[0] : string.Empty;
                  myStoreItemModel3.SetIcon(iconPath);
                  myStoreItemModel3.Description = prefabDefinition.DescriptionString;
                  myStoreItemModel3.HasTooltip = true;
                  string tooltipImage = prefabDefinition.TooltipImage;
                  myStoreItemModel3.SetTooltipImage(tooltipImage);
                }
                else
                {
                  MyStoreItemModel myStoreItemModel4 = myStoreItemModel3;
                  MyObjectBuilder_CubeGrid[] cubeGrids = prefabDefinition.CubeGrids;
                  string str = (cubeGrids != null ? ((uint) cubeGrids.Length > 0U ? 1 : 0) : 0) != 0 ? prefabDefinition.CubeGrids[0].DisplayName : string.Empty;
                  myStoreItemModel4.Name = str;
                }
              }
              myStoreItemModel3.PrefabTotalPcu = storeItem.PrefabTotalPcu;
              break;
          }
          switch (storeItem.StoreItemType)
          {
            case StoreItemTypes.Offer:
              myStoreItemModel3.PricePerUnit = (int) ((double) storeItem.PricePerUnit * (double) offersBonus);
              if ((double) offersBonus < 1.0)
              {
                float num1 = (float) storeItem.PricePerUnit * (1f + storeItem.PricePerUnitDiscount);
                float num2 = (double) num1 > (double) myStoreItemModel3.PricePerUnit ? (float) (1.0 - (double) myStoreItemModel3.PricePerUnit / (double) num1) : 0.0f;
                myStoreItemModel3.PricePerUnitDiscount = (float) Math.Round((double) num2 * 100.0, 1);
              }
              else
                myStoreItemModel3.PricePerUnitDiscount = (float) Math.Round((double) storeItem.PricePerUnitDiscount * 100.0, 1);
              toSort1.Add(myStoreItemModel3);
              if (this.SelectedOfferItem != null && this.SelectedOfferItem.Id == storeItem.Id)
              {
                myStoreItemModel1 = myStoreItemModel3;
                break;
              }
              break;
            case StoreItemTypes.Order:
              myStoreItemModel3.PricePerUnit = (int) ((double) storeItem.PricePerUnit * (double) ordersBonus);
              toSort2.Add(myStoreItemModel3);
              if (this.SelectedOrderItem != null && this.SelectedOrderItem.Id == storeItem.Id)
              {
                myStoreItemModel2 = myStoreItemModel3;
                break;
              }
              break;
          }
          myStoreItemModel3.TotalPrice = (long) (storeItem.Amount * myStoreItemModel3.PricePerUnit);
          observableCollection.Add(myStoreItemModel3);
        }
      }
      this.SortOffers(toSort1);
      this.SortOrders(toSort2);
      if (this.IsAdministrationVisible)
      {
        this.AdministrationViewModel.OfferCount = toSort1.Count;
        this.AdministrationViewModel.OrderCount = toSort2.Count;
        this.AdministrationViewModel.StoreItems = observableCollection;
      }
      this.SelectedOfferItem = myStoreItemModel1;
      this.SelectedOrderItem = myStoreItemModel2;
    }

    private void UpdateOfferedInfo()
    {
      if (this.SelectedOfferItem == null)
      {
        this.AmountToBuy = 0.0f;
        this.AmountToBuyMaximum = 1f;
        this.IsBuyEnabled = false;
        this.IsPreviewEnabled = false;
      }
      else
      {
        this.AmountToBuyMaximum = (float) this.SelectedOfferItem.Amount;
        if ((double) this.AmountToBuy == 0.0)
          this.AmountToBuy = 1f;
        if ((double) this.AmountToBuy > (double) this.SelectedOfferItem.Amount)
          this.AmountToBuy = (float) this.SelectedOfferItem.Amount;
        else
          this.UpdateTotalPriceToBuy(this.AmountToBuy);
        this.IsPreviewEnabled = this.SelectedOfferItem.ItemType == ItemTypes.Grid;
      }
    }

    private void UpdateOrderedInfo()
    {
      if (this.SelectedOrderItem == null)
      {
        this.AmountToSell = 0.0f;
        this.IsSellEnabled = false;
      }
      else
      {
        if ((double) this.AmountToSell == 0.0)
          this.AmountToSell = 1f;
        if ((double) this.AmountToSell > (double) this.SelectedOrderItem.Amount)
          this.AmountToSell = (float) this.SelectedOrderItem.Amount;
        else
          this.UpdateTotalPriceToSell(this.AmountToSell);
        this.IsSellEnabled = true;
      }
    }

    private void UpdateTotalPriceToBuy(float amount)
    {
      long valueToFormat = 0;
      if (!float.IsNaN(amount))
        valueToFormat = (long) ((int) amount * this.SelectedOfferItem.PricePerUnit);
      this.TotalPriceToBuy = MyBankingSystem.GetFormatedValue(valueToFormat);
      this.IsBuyEnabled = this.InventoryTargetViewModel.LocalPlayerAccountInfo.Balance >= valueToFormat && (double) amount != 0.0 && !float.IsNaN(amount);
    }

    private void UpdateTotalPriceToSell(float amount)
    {
      long valueToFormat = 0;
      if (!float.IsNaN(amount))
        valueToFormat = (long) ((int) amount * this.SelectedOrderItem.PricePerUnit);
      this.TotalPriceToSell = MyBankingSystem.GetFormatedValue(valueToFormat);
      this.IsSellEnabled = (double) this.SelectedOrderItem.Amount >= (double) amount && (double) amount != 0.0 && !float.IsNaN(amount);
    }

    private void OnSell(object obj)
    {
      if (!this.IsSellEnabled || this.SelectedOrderItem == null)
        return;
      int amountToSell = (int) this.AmountToSell;
      if (amountToSell > this.SelectedOrderItem.Amount || float.IsNaN((float) amountToSell) || (this.InventoryTargetViewModel.SelectedInventoryIndex < 0 || this.InventoryTargetViewModel.SelectedInventoryIndex >= this.InventoryTargetViewModel.Inventories.Count))
        return;
      this.IsSellEnabled = false;
      long entityId = this.InventoryTargetViewModel.Inventories[this.InventoryTargetViewModel.SelectedInventoryIndex].EntityId;
      this.m_storeBlock.CreateSellItemRequest(this.SelectedOrderItem.Id, amountToSell, entityId, this.m_lastEconomyTick, new Action<MyStoreSellItemResult>(this.OnSellResult));
    }

    private void OnSellResult(MyStoreSellItemResult result)
    {
      switch (result.Result)
      {
        case MyStoreSellItemResults.ItemNotFound:
          this.ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_ItemNotFound), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_ItemNotFound));
          break;
        case MyStoreSellItemResults.WrongAmount:
          this.ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreSell_Error_Caption_WrongAmount), MyTexts.Get(MySpaceTexts.StoreSell_Error_Text_WrongAmount));
          break;
        case MyStoreSellItemResults.ItemsTimeout:
          this.ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_ItemsTimeout), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_ItemsTimeout));
          break;
        case MyStoreSellItemResults.NotEnoughAmount:
          this.ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreSell_Error_Caption_NotEnoughAmount), MyTexts.Get(MySpaceTexts.StoreSell_Error_Text_NotEnoughAmount));
          break;
        case MyStoreSellItemResults.NotEnoughMoney:
          this.ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreSell_Error_Caption_NotEnoughMoney), MyTexts.Get(MySpaceTexts.StoreSell_Error_Text_NotEnoughMoney));
          break;
        case MyStoreSellItemResults.NotEnoughInventorySpace:
          this.ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreSell_Error_Caption_NotEnoughInventorySpace), MyTexts.Get(MySpaceTexts.StoreSell_Error_Text_NotEnoughInventorySpace));
          break;
      }
      this.RefreshStoreItems();
      this.UpdateLocalPlayerAccountBalance();
    }

    private void ShowMessageBoxError(StringBuilder caption, StringBuilder text)
    {
      StringBuilder messageCaption = caption;
      MyScreenManager.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: text, messageCaption: messageCaption, canHideOthers: false));
    }

    private void OnBuy(object obj)
    {
      if (!this.IsBuyEnabled || this.SelectedOfferItem == null)
        return;
      int amountToBuy = (int) this.AmountToBuy;
      if (amountToBuy > this.SelectedOfferItem.Amount || float.IsNaN((float) amountToBuy) || ((long) (amountToBuy * this.SelectedOfferItem.PricePerUnit) > this.InventoryTargetViewModel.LocalPlayerAccountInfo.Balance || this.InventoryTargetViewModel.SelectedInventoryIndex < 0) || this.InventoryTargetViewModel.SelectedInventoryIndex >= this.InventoryTargetViewModel.Inventories.Count)
        return;
      this.IsBuyEnabled = false;
      long entityId = this.InventoryTargetViewModel.Inventories[this.InventoryTargetViewModel.SelectedInventoryIndex].EntityId;
      this.m_storeBlock.CreateBuyRequest(this.SelectedOfferItem.Id, amountToBuy, entityId, this.m_lastEconomyTick, new Action<MyStoreBuyItemResult>(this.OnBuyResult));
    }

    private void OnBuyResult(MyStoreBuyItemResult result)
    {
      switch (result.Result)
      {
        case MyStoreBuyItemResults.ItemNotFound:
          this.ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_ItemNotFound), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_ItemNotFound));
          break;
        case MyStoreBuyItemResults.WrongAmount:
          this.ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_WrongAmount), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_WrongAmount));
          break;
        case MyStoreBuyItemResults.NotEnoughMoney:
          this.ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_NotEnoughMoney), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_NotEnoughMoney));
          break;
        case MyStoreBuyItemResults.ItemsTimeout:
          this.ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_ItemsTimeout), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_ItemsTimeout));
          break;
        case MyStoreBuyItemResults.NotEnoughInventorySpace:
          this.ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_NotEnoughInventorySpace), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_NotEnoughInventorySpace));
          break;
        case MyStoreBuyItemResults.WrongInventory:
          this.ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_WrongInventory), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_WrongInventory));
          break;
        case MyStoreBuyItemResults.SpawnFailed:
          this.ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_SpawnFailed), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_SpawnFailed));
          break;
        case MyStoreBuyItemResults.FreePositionNotFound:
          this.ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_FreePositionNotFound), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_FreePositionNotFound));
          break;
        case MyStoreBuyItemResults.NotEnoughStoreBlockInventorySpace:
          this.ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_NotEnoughStoreBlockInventorySpace), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_NotEnoughStoreBlockInventorySpace));
          break;
        case MyStoreBuyItemResults.NotEnoughAmount:
          this.ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_NotEnoughAmount), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_NotEnoughAmount));
          break;
        case MyStoreBuyItemResults.NotEnoughPCU:
          this.ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_NotEnoughPCU), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_NotEnoughPCU));
          break;
        case MyStoreBuyItemResults.NotEnoughSpaceInTank:
          this.ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_NotEnoughSpaceInTank), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_NotEnoughSpaceInTank));
          break;
      }
      this.RefreshStoreItems();
      this.UpdateLocalPlayerAccountBalance();
    }

    private void OnSortingDemandedItems(DataGridSortingEventArgs sortingArgs) => this.OrderedItems = new ObservableCollection<MyStoreItemModel>(this.SortStoreItems(this.OrderedItems, sortingArgs));

    private void OnSortingOfferedItems(DataGridSortingEventArgs sortingArgs) => this.OfferedItems = new ObservableCollection<MyStoreItemModel>(this.SortStoreItems(this.OfferedItems, sortingArgs));

    private void OnSwitchSortOffers(object obj)
    {
      ++this.m_sortModeOffers;
      if (this.m_sortModeOffers > MyStoreBlockViewModel.SortBy.PricePerUnit)
        this.m_sortModeOffers = MyStoreBlockViewModel.SortBy.Name;
      this.SortOffers(this.OfferedItems);
    }

    private void SortOffers(ObservableCollection<MyStoreItemModel> toSort)
    {
      this.OfferedItems = new ObservableCollection<MyStoreItemModel>(MyStoreBlockViewModel.Sort(toSort, this.m_sortModeOffers.ToString(), new ListSortDirection?(ListSortDirection.Ascending)));
      this.SortModeOffersText = MyTexts.GetString("StoreBlock_Column_" + (object) this.m_sortModeOffers);
    }

    private void OnSwitchSortOrders(object obj)
    {
      ++this.m_sortModeOrders;
      if (this.m_sortModeOrders > MyStoreBlockViewModel.SortBy.PricePerUnit)
        this.m_sortModeOrders = MyStoreBlockViewModel.SortBy.Name;
      this.SortOrders(this.OrderedItems);
    }

    private void SortOrders(ObservableCollection<MyStoreItemModel> toSort)
    {
      this.OrderedItems = new ObservableCollection<MyStoreItemModel>(MyStoreBlockViewModel.Sort(toSort, this.m_sortModeOrders.ToString(), new ListSortDirection?(ListSortDirection.Ascending)));
      this.SortModeOrdersText = MyTexts.GetString("StoreBlock_Column_" + (object) this.m_sortModeOrders);
    }

    private IEnumerable<MyStoreItemModel> SortStoreItems(
      ObservableCollection<MyStoreItemModel> toSort,
      DataGridSortingEventArgs sortingArgs)
    {
      DataGridColumn column = sortingArgs.Column;
      ListSortDirection? sortDirection = column.SortDirection;
      if (sortDirection.HasValue)
      {
        ListSortDirection? nullable = sortDirection;
        ListSortDirection listSortDirection = ListSortDirection.Descending;
        if (!(nullable.GetValueOrDefault() == listSortDirection & nullable.HasValue))
        {
          column.SortDirection = new ListSortDirection?(ListSortDirection.Descending);
          goto label_4;
        }
      }
      column.SortDirection = new ListSortDirection?(ListSortDirection.Ascending);
label_4:
      return MyStoreBlockViewModel.Sort(toSort, column.SortMemberPath, column.SortDirection);
    }

    private static IEnumerable<MyStoreItemModel> Sort(
      ObservableCollection<MyStoreItemModel> toSort,
      string column,
      ListSortDirection? sortOrder)
    {
      IEnumerable<MyStoreItemModel> myStoreItemModels = (IEnumerable<MyStoreItemModel>) null;
      if (!(column == "Name"))
      {
        if (!(column == "Amount"))
        {
          if (column == "PricePerUnit")
          {
            if (sortOrder.HasValue)
            {
              ListSortDirection? nullable = sortOrder;
              ListSortDirection listSortDirection = ListSortDirection.Ascending;
              if (!(nullable.GetValueOrDefault() == listSortDirection & nullable.HasValue))
              {
                myStoreItemModels = (IEnumerable<MyStoreItemModel>) toSort.OrderByDescending<MyStoreItemModel, int>((Func<MyStoreItemModel, int>) (u => u.PricePerUnit));
                goto label_15;
              }
            }
            myStoreItemModels = (IEnumerable<MyStoreItemModel>) toSort.OrderBy<MyStoreItemModel, int>((Func<MyStoreItemModel, int>) (u => u.PricePerUnit));
          }
        }
        else
        {
          if (sortOrder.HasValue)
          {
            ListSortDirection? nullable = sortOrder;
            ListSortDirection listSortDirection = ListSortDirection.Ascending;
            if (!(nullable.GetValueOrDefault() == listSortDirection & nullable.HasValue))
            {
              myStoreItemModels = (IEnumerable<MyStoreItemModel>) toSort.OrderByDescending<MyStoreItemModel, int>((Func<MyStoreItemModel, int>) (u => u.Amount));
              goto label_15;
            }
          }
          myStoreItemModels = (IEnumerable<MyStoreItemModel>) toSort.OrderBy<MyStoreItemModel, int>((Func<MyStoreItemModel, int>) (u => u.Amount));
        }
      }
      else
      {
        if (sortOrder.HasValue)
        {
          ListSortDirection? nullable = sortOrder;
          ListSortDirection listSortDirection = ListSortDirection.Ascending;
          if (!(nullable.GetValueOrDefault() == listSortDirection & nullable.HasValue))
          {
            myStoreItemModels = (IEnumerable<MyStoreItemModel>) toSort.OrderByDescending<MyStoreItemModel, string>((Func<MyStoreItemModel, string>) (u => u.Name));
            goto label_15;
          }
        }
        myStoreItemModels = (IEnumerable<MyStoreItemModel>) toSort.OrderBy<MyStoreItemModel, string>((Func<MyStoreItemModel, string>) (u => u.Name));
      }
label_15:
      return myStoreItemModels;
    }

    private void AdministrationViewModel_Changed(object sender, System.EventArgs e)
    {
      this.RefreshStoreItems();
      this.UpdateLocalPlayerAccountBalance();
    }

    public override void OnScreenClosing()
    {
      this.AdministrationViewModel.OfferCreated -= new EventHandler(this.AdministrationViewModel_Changed);
      this.AdministrationViewModel.OrderCreated -= new EventHandler(this.AdministrationViewModel_Changed);
      this.AdministrationViewModel.StoreItemRemoved -= new EventHandler(this.AdministrationViewModel_Changed);
      this.InventoryTargetViewModel.UnsubscribeEvents();
      ServiceManager.Instance.RemoveService<IMyStoreBlockViewModel>();
      base.OnScreenClosing();
    }

    private enum SortBy
    {
      Name,
      Amount,
      PricePerUnit,
    }
  }
}
