// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.ViewModels.MyPlayerTradeViewModel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.GameSystems.Trading;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Components.Trading;
using VRage.Input;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.Screens.ViewModels
{
  [StaticEventOwner]
  public class MyPlayerTradeViewModel : MyViewModelBase, IMyPlayerTradeViewModel
  {
    private bool m_isAcceptEnabled;
    private bool m_isAcceptVisible;
    private bool m_isSubmitOfferEnabled;
    private bool m_isPcuVisible;
    private bool m_isCancelVisible;
    private MyAccountInfo m_localPlayerAccountInfo;
    private int m_localAvailablePCU;
    private string m_localPlayerCurrency;
    private string m_localPlayerPcu;
    private float m_localPlayerOfferCurrency;
    private float m_localPlayerOfferCurrencyMaximum;
    private float m_localPlayerOfferPcu;
    private string m_otherPlayerOfferCurrency;
    private string m_otherPlayerAcceptState;
    private string m_otherPlayerOfferPcu;
    private string m_submitOfferLabel;
    private string m_acceptTradeLabel;
    private BitmapImage m_currencyIcon;
    private ICommand m_acceptCommand;
    private ICommand m_cancelCommand;
    private ICommand m_submitOfferCommand;
    private ICommand m_removeItemFromOfferCommand;
    private ICommand m_addItemToOfferCommand;
    private ICommand m_addStackTenToOfferCommand;
    private ICommand m_addStackHundredToOfferCommand;
    private ICommand m_removeStackTenToOfferCommand;
    private ICommand m_removeStackHundredToOfferCommand;
    private ICommand m_submitAcceptCommand;
    private ICommand m_addItemCommand;
    private ICommand m_removeItemCommand;
    private MyInventoryItemModel m_selectedInventoryItem;
    private MyInventoryItemModel m_selectedOfferItem;
    private ObservableCollection<MyInventoryItemModel> m_myInventoryItems = new ObservableCollection<MyInventoryItemModel>();
    private ObservableCollection<MyInventoryItemModel> m_localPlayerOfferItems = new ObservableCollection<MyInventoryItemModel>();
    private ObservableCollection<MyInventoryItemModel> m_otherPlayerOfferItems = new ObservableCollection<MyInventoryItemModel>();
    private ulong m_otherPlayerId;
    private bool m_waitingToAcceptTrade;
    private MyInventoryItemModel m_draggedItem;
    private bool m_isLeftToRight;
    private bool m_triggerCancelEvent = true;

    public BitmapImage CurrencyIcon
    {
      get => this.m_currencyIcon;
      set => this.SetProperty<BitmapImage>(ref this.m_currencyIcon, value, nameof (CurrencyIcon));
    }

    public string OtherPlayerOfferPcu
    {
      get => this.m_otherPlayerOfferPcu;
      set => this.SetProperty<string>(ref this.m_otherPlayerOfferPcu, value, nameof (OtherPlayerOfferPcu));
    }

    public string OtherPlayerOfferCurrency
    {
      get => this.m_otherPlayerOfferCurrency;
      set => this.SetProperty<string>(ref this.m_otherPlayerOfferCurrency, value, nameof (OtherPlayerOfferCurrency));
    }

    public string OtherPlayerAcceptState
    {
      get => this.m_otherPlayerAcceptState;
      set => this.SetProperty<string>(ref this.m_otherPlayerAcceptState, value, nameof (OtherPlayerAcceptState));
    }

    public float LocalPlayerOfferPcu
    {
      get => this.m_localPlayerOfferPcu;
      set
      {
        if ((double) this.m_localPlayerOfferPcu != (double) value)
          this.OnLocalPlayerOfferPCUChanged(value);
        this.SetProperty<float>(ref this.m_localPlayerOfferPcu, value, nameof (LocalPlayerOfferPcu));
      }
    }

    public float LocalPlayerOfferCurrency
    {
      get => this.m_localPlayerOfferCurrency;
      set
      {
        float num = float.IsNaN(value) ? 0.0f : value;
        if ((double) this.m_localPlayerOfferCurrency != (double) num)
          this.OnLocalPlayerOfferCurrencyChanged(num);
        this.SetProperty<float>(ref this.m_localPlayerOfferCurrency, num, nameof (LocalPlayerOfferCurrency));
      }
    }

    public float LocalPlayerOfferCurrencyMaximum
    {
      get => this.m_localPlayerOfferCurrencyMaximum;
      set => this.SetProperty<float>(ref this.m_localPlayerOfferCurrencyMaximum, value, nameof (LocalPlayerOfferCurrencyMaximum));
    }

    public string LocalPlayerPcu
    {
      get => this.m_localPlayerPcu;
      set => this.SetProperty<string>(ref this.m_localPlayerPcu, value, nameof (LocalPlayerPcu));
    }

    public string LocalPlayerCurrency
    {
      get => this.m_localPlayerCurrency;
      set => this.SetProperty<string>(ref this.m_localPlayerCurrency, value, nameof (LocalPlayerCurrency));
    }

    public bool IsAcceptEnabled
    {
      get => this.m_isAcceptEnabled;
      set => this.SetProperty<bool>(ref this.m_isAcceptEnabled, value, nameof (IsAcceptEnabled));
    }

    public bool IsAcceptVisible
    {
      get => this.m_isAcceptVisible;
      set => this.SetProperty<bool>(ref this.m_isAcceptVisible, value, nameof (IsAcceptVisible));
    }

    public bool IsSubmitOfferEnabled
    {
      get => this.m_isSubmitOfferEnabled;
      set => this.SetProperty<bool>(ref this.m_isSubmitOfferEnabled, value, nameof (IsSubmitOfferEnabled));
    }

    public bool IsPcuVisible
    {
      get => this.m_isPcuVisible;
      set => this.SetProperty<bool>(ref this.m_isPcuVisible, value, nameof (IsPcuVisible));
    }

    public bool IsCancelVisible
    {
      get => this.m_isCancelVisible;
      set => this.SetProperty<bool>(ref this.m_isCancelVisible, value, nameof (IsCancelVisible));
    }

    public string SubmitOfferLabel
    {
      get => this.m_submitOfferLabel;
      set => this.SetProperty<string>(ref this.m_submitOfferLabel, value, nameof (SubmitOfferLabel));
    }

    public string AcceptTradeLabel
    {
      get => this.m_acceptTradeLabel;
      set => this.SetProperty<string>(ref this.m_acceptTradeLabel, value, nameof (AcceptTradeLabel));
    }

    public ICommand AcceptCommand
    {
      get => this.m_acceptCommand;
      set => this.SetProperty<ICommand>(ref this.m_acceptCommand, value, nameof (AcceptCommand));
    }

    public ICommand CancelCommand
    {
      get => this.m_cancelCommand;
      set => this.SetProperty<ICommand>(ref this.m_cancelCommand, value, nameof (CancelCommand));
    }

    public ICommand SubmitOfferCommand
    {
      get => this.m_submitOfferCommand;
      set => this.SetProperty<ICommand>(ref this.m_submitOfferCommand, value, nameof (SubmitOfferCommand));
    }

    public ICommand RemoveItemFromOfferCommand
    {
      get => this.m_removeItemFromOfferCommand;
      set => this.SetProperty<ICommand>(ref this.m_removeItemFromOfferCommand, value, nameof (RemoveItemFromOfferCommand));
    }

    public ICommand AddItemToOfferCommand
    {
      get => this.m_addItemToOfferCommand;
      set => this.SetProperty<ICommand>(ref this.m_addItemToOfferCommand, value, nameof (AddItemToOfferCommand));
    }

    public ICommand AddStackTenToOfferCommand
    {
      get => this.m_addStackTenToOfferCommand;
      set => this.SetProperty<ICommand>(ref this.m_addStackTenToOfferCommand, value, nameof (AddStackTenToOfferCommand));
    }

    public ICommand AddStackHundredToOfferCommand
    {
      get => this.m_addStackHundredToOfferCommand;
      set => this.SetProperty<ICommand>(ref this.m_addStackHundredToOfferCommand, value, nameof (AddStackHundredToOfferCommand));
    }

    public ICommand RemoveStackTenToOfferCommand
    {
      get => this.m_addStackTenToOfferCommand;
      set => this.SetProperty<ICommand>(ref this.m_removeStackTenToOfferCommand, value, nameof (RemoveStackTenToOfferCommand));
    }

    public ICommand RemoveStackHundredToOfferCommand
    {
      get => this.m_addStackHundredToOfferCommand;
      set => this.SetProperty<ICommand>(ref this.m_removeStackHundredToOfferCommand, value, nameof (RemoveStackHundredToOfferCommand));
    }

    public ICommand SubmitAcceptCommand
    {
      get => this.m_submitAcceptCommand;
      set => this.SetProperty<ICommand>(ref this.m_submitAcceptCommand, value, nameof (SubmitAcceptCommand));
    }

    public ICommand AddItemCommand
    {
      get => this.m_addItemCommand;
      set => this.SetProperty<ICommand>(ref this.m_addItemCommand, value, nameof (AddItemCommand));
    }

    public ICommand RemoveItemCommand
    {
      get => this.m_removeItemCommand;
      set => this.SetProperty<ICommand>(ref this.m_removeItemCommand, value, nameof (RemoveItemCommand));
    }

    public MyInventoryItemModel SelectedInventoryItem
    {
      get => this.m_selectedInventoryItem;
      set => this.SetProperty<MyInventoryItemModel>(ref this.m_selectedInventoryItem, value, nameof (SelectedInventoryItem));
    }

    public MyInventoryItemModel SelectedOfferItem
    {
      get => this.m_selectedOfferItem;
      set => this.SetProperty<MyInventoryItemModel>(ref this.m_selectedOfferItem, value, nameof (SelectedOfferItem));
    }

    public ObservableCollection<MyInventoryItemModel> InventoryItems
    {
      get => this.m_myInventoryItems;
      set => this.SetProperty<ObservableCollection<MyInventoryItemModel>>(ref this.m_myInventoryItems, value, nameof (InventoryItems));
    }

    public ObservableCollection<MyInventoryItemModel> LocalPlayerOfferItems
    {
      get => this.m_localPlayerOfferItems;
      set => this.SetProperty<ObservableCollection<MyInventoryItemModel>>(ref this.m_localPlayerOfferItems, value, nameof (LocalPlayerOfferItems));
    }

    public ObservableCollection<MyInventoryItemModel> OtherPlayerOfferItems
    {
      get => this.m_otherPlayerOfferItems;
      set => this.SetProperty<ObservableCollection<MyInventoryItemModel>>(ref this.m_otherPlayerOfferItems, value, nameof (OtherPlayerOfferItems));
    }

    public MyPlayerTradeViewModel(ulong otherPlayerId)
      : base()
    {
      this.m_otherPlayerId = otherPlayerId;
      this.InitData();
      this.AcceptCommand = (ICommand) new RelayCommand(new Action<object>(this.OnAccept));
      this.CancelCommand = (ICommand) new RelayCommand(new Action<object>(this.OnCancel));
      this.SubmitOfferCommand = (ICommand) new RelayCommand(new Action<object>(this.OnSubmitOffer));
      this.RemoveItemFromOfferCommand = (ICommand) new RelayCommand<MyInventoryItemModel>(new Action<MyInventoryItemModel>(this.OnRemoveItemFromOffer));
      this.AddItemToOfferCommand = (ICommand) new RelayCommand<MyInventoryItemModel>(new Action<MyInventoryItemModel>(this.OnAddItemToOffer));
      this.AddStackTenToOfferCommand = (ICommand) new RelayCommand<MyInventoryItemModel>(new Action<MyInventoryItemModel>(this.OnAddStackTenToOffer));
      this.AddStackHundredToOfferCommand = (ICommand) new RelayCommand<MyInventoryItemModel>(new Action<MyInventoryItemModel>(this.OnAddStackHundredToOffer));
      this.RemoveStackTenToOfferCommand = (ICommand) new RelayCommand<MyInventoryItemModel>(new Action<MyInventoryItemModel>(this.OnRemoveStackTenToOffer));
      this.RemoveStackHundredToOfferCommand = (ICommand) new RelayCommand<MyInventoryItemModel>(new Action<MyInventoryItemModel>(this.OnRemoveStackHundredToOffer));
      this.SubmitAcceptCommand = (ICommand) new RelayCommand(new Action<object>(this.OnSubmitAccept));
      this.AddItemCommand = (ICommand) new RelayCommand(new Action<object>(this.OnAddItem));
      this.RemoveItemCommand = (ICommand) new RelayCommand(new Action<object>(this.OnRemoveItem));
      BitmapImage bitmapImage = new BitmapImage();
      string[] icons = MyBankingSystem.BankingSystemDefinition.Icons;
      bitmapImage.TextureAsset = (icons != null ? ((uint) icons.Length > 0U ? 1 : 0) : 0) != 0 ? MyBankingSystem.BankingSystemDefinition.Icons[0] : string.Empty;
      this.CurrencyIcon = bitmapImage;
      this.IsSubmitOfferEnabled = true;
      ServiceManager.Instance.AddService<IMyPlayerTradeViewModel>((IMyPlayerTradeViewModel) this);
    }

    private void InitData()
    {
      MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      if (localCharacter == null)
        this.OnExit((object) this);
      else if (!(localCharacter.GetInventoryBase() is MyInventory inventoryBase))
      {
        this.OnExit((object) this);
      }
      else
      {
        List<MyInventoryItemModel> list = new List<MyInventoryItemModel>();
        foreach (MyPhysicalInventoryItem inventoryItem in inventoryBase.GetItems())
        {
          MyInventoryItemModel inventoryItemModel = new MyInventoryItemModel(inventoryItem, (MyInventoryBase) inventoryBase);
          list.Add(inventoryItemModel);
        }
        this.InventoryItems = new ObservableCollection<MyInventoryItemModel>(list);
        MyBankingSystem.Static.TryGetAccountInfo(localHumanPlayer.Identity.IdentityId, out this.m_localPlayerAccountInfo);
        this.LocalPlayerCurrency = MyBankingSystem.GetFormatedValue(this.m_localPlayerAccountInfo.Balance);
        this.LocalPlayerOfferCurrencyMaximum = (float) this.m_localPlayerAccountInfo.Balance;
        this.OtherPlayerOfferCurrency = MyBankingSystem.GetFormatedValue(0L);
        switch (MySession.Static.BlockLimitsEnabled)
        {
          case MyBlockLimitsEnabledEnum.NONE:
            this.IsPcuVisible = false;
            break;
          case MyBlockLimitsEnabledEnum.GLOBALLY:
            this.IsPcuVisible = false;
            break;
          case MyBlockLimitsEnabledEnum.PER_FACTION:
            MyFaction playerFaction1 = MySession.Static.Factions.GetPlayerFaction(localHumanPlayer.Identity.IdentityId);
            MyPlayer playerById = MySession.Static.Players.GetPlayerById(new MyPlayer.PlayerId(this.m_otherPlayerId));
            MyFaction playerFaction2 = MySession.Static.Factions.GetPlayerFaction(playerById.Identity.IdentityId);
            this.IsPcuVisible = ((playerFaction1 == null ? 0 : (playerFaction1.IsLeader(localHumanPlayer.Identity.IdentityId) ? 1 : 0)) & (playerFaction2 == null ? (false ? 1 : 0) : (playerFaction2.IsLeader(playerById.Identity.IdentityId) ? 1 : 0))) != 0 && MySession.Static.Settings.EnablePcuTrading;
            if (this.IsPcuVisible)
            {
              this.m_localAvailablePCU = playerFaction1.BlockLimits.PCU;
              this.LocalPlayerPcu = this.m_localAvailablePCU.ToString();
              break;
            }
            break;
          case MyBlockLimitsEnabledEnum.PER_PLAYER:
            this.IsPcuVisible = MySession.Static.Settings.EnablePcuTrading;
            this.m_localAvailablePCU = localHumanPlayer.Identity.BlockLimits.PCU;
            this.LocalPlayerPcu = this.m_localAvailablePCU.ToString();
            break;
        }
        this.SubmitOfferLabel = MyTexts.GetString(MySpaceTexts.TradeScreenSubmitOffer);
        this.IsAcceptVisible = true;
        this.OtherPlayerAcceptState = MyTexts.GetString(MySpaceTexts.TradeScreenNotAccepted);
        this.LocalPlayerOfferItems.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnLocalOfferItemsChanged);
        DragDrop.Instance.DropStarting += new DragDropEventHandler(this.DropStarting);
      }
    }

    private void OnLocalPlayerOfferCurrencyChanged(float value)
    {
      this.LocalPlayerCurrency = MyBankingSystem.GetFormatedValue(this.m_localPlayerAccountInfo.Balance - (long) value);
      this.UpdateButtons(false);
    }

    private void OnLocalPlayerOfferPCUChanged(float value)
    {
      this.LocalPlayerPcu = !float.IsNaN(value) ? (this.m_localAvailablePCU - (int) value).ToString() : "0";
      this.UpdateButtons(false);
    }

    private void OnAddItem(object obj)
    {
      if (this.SelectedInventoryItem == null)
        return;
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.LEFT_BUTTON, MyControlStateType.PRESSED))
        this.OnAddStackTenToOffer(this.SelectedInventoryItem);
      else if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.RIGHT_BUTTON, MyControlStateType.PRESSED))
        this.OnAddStackHundredToOffer(this.SelectedInventoryItem);
      else
        this.OnAddItemToOffer(this.SelectedInventoryItem);
    }

    private void OnAddItemToOffer(MyInventoryItemModel item)
    {
      this.InventoryItems.Remove(item);
      this.SelectedInventoryItem = (MyInventoryItemModel) null;
      this.LocalPlayerOfferItems.Add(item);
      this.UpdateButtons(false);
    }

    private void OnAddStackHundredToOffer(MyInventoryItemModel item)
    {
      this.m_isLeftToRight = true;
      this.m_draggedItem = item;
      if ((double) item.Amount >= 100.0)
        this.OnAmountChanged(100f);
      else
        this.OnAmountChanged(item.Amount);
    }

    private void OnAddStackTenToOffer(MyInventoryItemModel item)
    {
      this.m_isLeftToRight = true;
      this.m_draggedItem = item;
      if ((double) item.Amount >= 10.0)
        this.OnAmountChanged(10f);
      else
        this.OnAmountChanged(item.Amount);
    }

    private void OnRemoveItem(object obj)
    {
      if (this.SelectedOfferItem == null)
        return;
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.LEFT_BUTTON, MyControlStateType.PRESSED))
        this.OnRemoveStackTenToOffer(this.SelectedOfferItem);
      else if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.RIGHT_BUTTON, MyControlStateType.PRESSED))
        this.OnRemoveStackHundredToOffer(this.SelectedOfferItem);
      else
        this.OnRemoveItemFromOffer(this.SelectedOfferItem);
    }

    private void OnRemoveStackHundredToOffer(MyInventoryItemModel item)
    {
      this.m_isLeftToRight = false;
      this.m_draggedItem = item;
      if ((double) item.Amount >= 100.0)
        this.OnAmountChanged(100f);
      else
        this.OnAmountChanged(item.Amount);
    }

    private void OnRemoveStackTenToOffer(MyInventoryItemModel item)
    {
      this.m_isLeftToRight = false;
      this.m_draggedItem = item;
      if ((double) item.Amount >= 10.0)
        this.OnAmountChanged(10f);
      else
        this.OnAmountChanged(item.Amount);
    }

    private void OnRemoveItemFromOffer(MyInventoryItemModel item)
    {
      this.LocalPlayerOfferItems.Remove(item);
      this.SelectedOfferItem = (MyInventoryItemModel) null;
      this.InventoryItems.Add(item);
      this.UpdateButtons(false);
    }

    private void OnLocalOfferItemsChanged(object sender, NotifyCollectionChangedEventArgs e) => this.UpdateButtons(false);

    private void OnSubmitOffer(object obj)
    {
      this.UpdateButtons(true);
      this.SubmitOfferLabel = MyTexts.GetString(MySpaceTexts.TradeScreenOfferSubmited);
      List<MyObjectBuilder_InventoryItem> builderInventoryItemList = new List<MyObjectBuilder_InventoryItem>(this.LocalPlayerOfferItems.Count);
      foreach (MyInventoryItemModel localPlayerOfferItem in (Collection<MyInventoryItemModel>) this.LocalPlayerOfferItems)
        builderInventoryItemList.Add(localPlayerOfferItem.InventoryItem.GetObjectBuilder());
      MyTradingManager.Static.SubmitTradingOffer_Client(new MyObjectBuilder_SubmitOffer()
      {
        InventoryItems = builderInventoryItemList,
        CurrencyAmount = (long) this.LocalPlayerOfferCurrency,
        PCUAmount = !float.IsNaN(this.LocalPlayerOfferPcu) ? (int) this.LocalPlayerOfferPcu : 0
      });
    }

    private void OnSubmitAccept(object obj)
    {
      if (this.IsAcceptEnabled)
      {
        this.OnAccept((object) null);
      }
      else
      {
        if (!this.IsSubmitOfferEnabled)
          return;
        this.OnSubmitOffer((object) null);
      }
    }

    private void UpdateButtons(bool offerSubmitted)
    {
      this.IsSubmitOfferEnabled = !offerSubmitted;
      this.IsAcceptEnabled = offerSubmitted;
      this.SubmitOfferLabel = offerSubmitted ? MyTexts.GetString(MySpaceTexts.TradeScreenOfferSubmited) : MyTexts.GetString(MySpaceTexts.TradeScreenSubmitOffer);
      if (!this.m_waitingToAcceptTrade)
        return;
      this.AbortOffer();
    }

    internal void OnOfferRecieved(MyObjectBuilder_SubmitOffer obOffer)
    {
      List<MyInventoryItemModel> list = new List<MyInventoryItemModel>();
      foreach (MyObjectBuilder_InventoryItem inventoryItem in obOffer.InventoryItems)
      {
        MyInventoryItemModel inventoryItemModel = new MyInventoryItemModel(new MyPhysicalInventoryItem(inventoryItem), (MyInventoryBase) null);
        list.Add(inventoryItemModel);
      }
      this.OtherPlayerOfferItems = new ObservableCollection<MyInventoryItemModel>(list);
      this.OtherPlayerOfferCurrency = MyBankingSystem.GetFormatedValue(obOffer.CurrencyAmount);
      this.OtherPlayerOfferPcu = obOffer.PCUAmount.ToString();
      this.AbortOffer_Internal();
    }

    internal void OnOfferAbortedRecieved()
    {
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, messageText: new StringBuilder(MySession.Static.Players.GetPlayerById(new MyPlayer.PlayerId(this.m_otherPlayerId)).DisplayName + " inventory is full"), messageCaption: new StringBuilder("Offer No Accepted"), canHideOthers: false));
      this.UpdateButtons(false);
    }

    private void OnAccept(object obj)
    {
      if (!this.m_waitingToAcceptTrade)
      {
        this.IsCancelVisible = true;
        this.IsAcceptVisible = false;
        this.m_waitingToAcceptTrade = true;
        MyTradingManager.Static.AcceptOffer_Client(true);
      }
      else
        MyLog.Default.Error("Bad button state. Please check.");
    }

    private void OnCancel(object obj)
    {
      if (this.m_waitingToAcceptTrade)
        this.AbortOffer();
      else
        MyLog.Default.Error("Bad button state. Please check.");
    }

    private void AbortOffer()
    {
      this.AbortOffer_Internal();
      MyTradingManager.Static.AcceptOffer_Client(false);
    }

    private void AbortOffer_Internal()
    {
      this.m_waitingToAcceptTrade = false;
      this.IsCancelVisible = false;
      this.IsAcceptVisible = true;
    }

    internal void OnOfferAcceptStateChange(bool isAccepted)
    {
      if (isAccepted)
        this.OtherPlayerAcceptState = MyTexts.GetString(MySpaceTexts.TradeScreenAccepted);
      else
        this.OtherPlayerAcceptState = MyTexts.GetString(MySpaceTexts.TradeScreenNotAccepted);
    }

    public void CloseScreenLocal()
    {
      this.m_triggerCancelEvent = false;
      MyScreenManager.GetScreenWithFocus().CloseScreen();
    }

    public override void OnScreenClosing()
    {
      if (this.m_triggerCancelEvent)
        MyTradingManager.Static.TradeCancel_Client();
      ServiceManager.Instance.RemoveService<IMyPlayerTradeViewModel>();
      this.LocalPlayerOfferItems.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnLocalOfferItemsChanged);
      DragDrop.Instance.DropStarting -= new DragDropEventHandler(this.DropStarting);
      base.OnScreenClosing();
    }

    private void DropStarting(object sender, DragDropEventArgs e)
    {
      e.Cancel = true;
      this.m_isLeftToRight = (sender as ListBox).ItemsSource != this.m_myInventoryItems;
      MyInventoryItemModel data = e.Data as MyInventoryItemModel;
      this.m_draggedItem = data;
      if (e.MouseButton == MouseButton.Right)
      {
        MyObjectBuilderType typeId = data.InventoryItem.Content.TypeId;
        bool parseAsInteger = true;
        if (typeId == typeof (MyObjectBuilder_Ore) || typeId == typeof (MyObjectBuilder_Ingot))
          parseAsInteger = false;
        MyGuiScreenDialogAmount screenDialogAmount = new MyGuiScreenDialogAmount(0.0f, data.Amount, MyCommonTexts.DialogAmount_AddAmountCaption, 0, parseAsInteger, backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity);
        screenDialogAmount.OnConfirmed += new Action<float>(this.OnAmountChanged);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) screenDialogAmount);
      }
      else
        this.OnAmountChanged(this.m_draggedItem.Amount);
    }

    private void OnAmountChanged(float amount)
    {
      if ((double) amount == 0.0)
      {
        this.UpdateButtons(false);
        this.m_draggedItem = (MyInventoryItemModel) null;
      }
      else
      {
        ObservableCollection<MyInventoryItemModel> observableCollection1 = this.InventoryItems;
        ObservableCollection<MyInventoryItemModel> observableCollection2 = this.LocalPlayerOfferItems;
        if (this.m_isLeftToRight)
        {
          observableCollection1 = this.LocalPlayerOfferItems;
          observableCollection2 = this.InventoryItems;
        }
        float num = this.m_draggedItem.Amount - amount;
        if ((double) num == 0.0)
        {
          observableCollection2.Remove(this.m_draggedItem);
          if (observableCollection2 == this.LocalPlayerOfferItems)
            this.SelectedOfferItem = (MyInventoryItemModel) null;
          else
            this.SelectedInventoryItem = (MyInventoryItemModel) null;
        }
        else
          this.m_draggedItem.Amount = num;
        bool flag = false;
        foreach (MyInventoryItemModel inventoryItemModel in (Collection<MyInventoryItemModel>) observableCollection1)
        {
          if ((int) inventoryItemModel.InventoryItem.ItemId == (int) this.m_draggedItem.InventoryItem.ItemId)
          {
            inventoryItemModel.Amount += amount;
            flag = true;
            break;
          }
        }
        if (!flag)
          observableCollection1.Add(new MyInventoryItemModel(this.m_draggedItem.InventoryItem, (MyInventoryBase) null)
          {
            Amount = amount
          });
        this.UpdateButtons(false);
        this.m_draggedItem = (MyInventoryItemModel) null;
      }
    }
  }
}
