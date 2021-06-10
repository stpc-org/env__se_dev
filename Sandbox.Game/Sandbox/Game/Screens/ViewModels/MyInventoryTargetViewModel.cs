// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.ViewModels.MyInventoryTargetViewModel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Utils;

namespace Sandbox.Game.Screens.ViewModels
{
  public class MyInventoryTargetViewModel : ViewModelBase
  {
    private MyStoreBlock m_storeBlock;
    private ObservableCollection<MyInventoryTargetModel> m_inventories;
    private ObservableCollection<MyInventoryItemModel> m_inventoryItems;
    private int m_selectedInventoryIndex;
    private MyInventoryItemModel m_selectedInventoryItem;
    private string m_localPlayerCurrency;
    private string m_currentInventoryVolume;
    private string m_maxInventoryVolume;
    private MyAccountInfo m_localPlayerAccountInfo;
    private BitmapImage m_currencyIcon;
    private float m_balanceChangeValue;
    private ICommand m_withdrawCommand;
    private ICommand m_depositCommand;
    private ICommand m_showHelpCommand;

    public BitmapImage CurrencyIcon
    {
      get => this.m_currencyIcon;
      set => this.SetProperty<BitmapImage>(ref this.m_currencyIcon, value, nameof (CurrencyIcon));
    }

    internal MyAccountInfo LocalPlayerAccountInfo => this.m_localPlayerAccountInfo;

    public int SelectedInventoryIndex
    {
      get => this.m_selectedInventoryIndex;
      set
      {
        this.SetProperty<int>(ref this.m_selectedInventoryIndex, value, nameof (SelectedInventoryIndex));
        this.UpdateVolumeText();
        this.UpdateInventoryContent();
      }
    }

    public MyInventoryItemModel SelectedInventoryItem
    {
      get => this.m_selectedInventoryItem;
      set
      {
        this.SetProperty<MyInventoryItemModel>(ref this.m_selectedInventoryItem, value, nameof (SelectedInventoryItem));
        this.OnSelectedInventoryItemChanged();
      }
    }

    private void OnSelectedInventoryItemChanged()
    {
      if (this.SelectedInventoryItemChanged == null)
        return;
      this.SelectedInventoryItemChanged((object) this, System.EventArgs.Empty);
    }

    public ObservableCollection<MyInventoryTargetModel> Inventories
    {
      get => this.m_inventories;
      set => this.SetProperty<ObservableCollection<MyInventoryTargetModel>>(ref this.m_inventories, value, nameof (Inventories));
    }

    public ObservableCollection<MyInventoryItemModel> InventoryItems
    {
      get => this.m_inventoryItems;
      set => this.SetProperty<ObservableCollection<MyInventoryItemModel>>(ref this.m_inventoryItems, value, nameof (InventoryItems));
    }

    public string LocalPlayerCurrency
    {
      get => this.m_localPlayerCurrency;
      set => this.SetProperty<string>(ref this.m_localPlayerCurrency, value, nameof (LocalPlayerCurrency));
    }

    public string MaxInventoryVolume
    {
      get => this.m_maxInventoryVolume;
      set => this.SetProperty<string>(ref this.m_maxInventoryVolume, value, nameof (MaxInventoryVolume));
    }

    public string CurrentInventoryVolume
    {
      get => this.m_currentInventoryVolume;
      set => this.SetProperty<string>(ref this.m_currentInventoryVolume, value, nameof (CurrentInventoryVolume));
    }

    public float BalanceChangeValue
    {
      get => this.m_balanceChangeValue;
      set => this.SetProperty<float>(ref this.m_balanceChangeValue, value, nameof (BalanceChangeValue));
    }

    public ICommand DepositCommand
    {
      get => this.m_depositCommand;
      set => this.SetProperty<ICommand>(ref this.m_depositCommand, value, nameof (DepositCommand));
    }

    public ICommand WithdrawCommand
    {
      get => this.m_withdrawCommand;
      set => this.SetProperty<ICommand>(ref this.m_withdrawCommand, value, nameof (WithdrawCommand));
    }

    public event EventHandler SelectedInventoryItemChanged;

    public MyInventoryTargetViewModel(MyStoreBlock storeBlock)
    {
      BitmapImage bitmapImage = new BitmapImage();
      string[] icons = MyBankingSystem.BankingSystemDefinition.Icons;
      bitmapImage.TextureAsset = (icons != null ? ((uint) icons.Length > 0U ? 1 : 0) : 0) != 0 ? MyBankingSystem.BankingSystemDefinition.Icons[0] : string.Empty;
      this.CurrencyIcon = bitmapImage;
      this.DepositCommand = (ICommand) new RelayCommand(new Action<object>(this.OnDeposit));
      this.WithdrawCommand = (ICommand) new RelayCommand(new Action<object>(this.OnWithdraw));
      this.m_storeBlock = storeBlock;
    }

    private void OnWithdraw(object obj)
    {
      if (this.Inventories == null || this.SelectedInventoryIndex < 0 || (this.SelectedInventoryIndex >= this.Inventories.Count || float.IsNaN(this.BalanceChangeValue)))
        return;
      this.m_storeBlock.CreateChangeBalanceRequest(-(int) this.BalanceChangeValue, this.Inventories[this.SelectedInventoryIndex].EntityId, new Action<MyStoreBuyItemResults>(this.OnWithdrawCompleted));
    }

    private void OnWithdrawCompleted(MyStoreBuyItemResults result) => this.ProcessResult(result);

    private void ProcessResult(MyStoreBuyItemResults result)
    {
      switch (result)
      {
        case MyStoreBuyItemResults.WrongAmount:
          this.ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_WrongAmount), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_WrongAmount));
          break;
        case MyStoreBuyItemResults.NotEnoughMoney:
          this.ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_NotEnoughMoney), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_NotEnoughMoney));
          break;
        case MyStoreBuyItemResults.NotEnoughInventorySpace:
          this.ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_NotEnoughInventorySpace), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_NotEnoughInventorySpace));
          break;
      }
      this.UpdateLocalPlayerCurrency();
    }

    private void ShowMessageBoxError(StringBuilder caption, StringBuilder text)
    {
      StringBuilder messageCaption = caption;
      MyScreenManager.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: text, messageCaption: messageCaption, canHideOthers: false));
    }

    private void OnDeposit(object obj)
    {
      if (this.Inventories == null || this.SelectedInventoryIndex < 0 || (this.SelectedInventoryIndex >= this.Inventories.Count || float.IsNaN(this.BalanceChangeValue)))
        return;
      this.m_storeBlock.CreateChangeBalanceRequest((int) this.BalanceChangeValue, this.Inventories[this.SelectedInventoryIndex].EntityId, new Action<MyStoreBuyItemResults>(this.OnDepositCompleted));
    }

    private void OnDepositCompleted(MyStoreBuyItemResults result) => this.ProcessResult(result);

    public void Initialize(
      List<long> inventoryEntities,
      bool includeCharacterInventory,
      bool showAllOption)
    {
      List<MyInventoryTargetModel> list = new List<MyInventoryTargetModel>();
      foreach (long inventoryEntity in inventoryEntities)
      {
        MyCubeBlock entity;
        if (MyEntities.TryGetEntityById<MyCubeBlock>(inventoryEntity, out entity))
        {
          string str = entity.CubeGrid.DisplayName + " - " + entity.DisplayNameText;
          for (int index = 0; index < entity.InventoryCount; ++index)
          {
            MyInventory inventory = MyEntityExtensions.GetInventory(entity, index);
            MyInventoryTargetModel inventoryModel = new MyInventoryTargetModel((MyInventoryBase) inventory)
            {
              Name = str,
              EntityId = inventoryEntity,
              Volume = (float) inventory.CurrentVolume * 1000f,
              MaxVolume = (float) inventory.MaxVolume * 1000f
            };
            MyGasTank gasTank;
            if ((gasTank = entity as MyGasTank) != null)
            {
              inventoryModel.GasTank = gasTank;
              this.UpdateGasTankVolume(inventoryModel, gasTank);
              gasTank.FilledRatioChanged = (Action) (() =>
              {
                this.UpdateGasTankVolume(inventoryModel, gasTank);
                this.UpdateVolumeText();
              });
            }
            MyCubeBlockDefinition blockDefinition = entity.BlockDefinition;
            int num1;
            if (blockDefinition == null)
            {
              num1 = 0;
            }
            else
            {
              int? length = blockDefinition.Icons?.Length;
              int num2 = 0;
              num1 = length.GetValueOrDefault() > num2 & length.HasValue ? 1 : 0;
            }
            if (num1 != 0)
              inventoryModel.Icon = new BitmapImage()
              {
                TextureAsset = entity.BlockDefinition.Icons[0]
              };
            list.Add(inventoryModel);
            inventory.InventoryContentChanged += new Action<MyInventoryBase, MyPhysicalInventoryItem, MyFixedPoint>(this.OnInventoryContentChanged);
            inventory.ContentsChanged += new Action<MyInventoryBase>(this.Inventory_ContentsChanged);
          }
        }
      }
      list.SortNoAlloc<MyInventoryTargetModel>((Comparison<MyInventoryTargetModel>) ((x, y) => string.Compare(x.Name, y.Name, StringComparison.InvariantCultureIgnoreCase)));
      if (showAllOption)
        list.Insert(0, new MyInventoryTargetModel((MyInventoryBase) null)
        {
          Name = MyTexts.GetString(MySpaceTexts.InventorySelection_All),
          AllInventories = true
        });
      if (includeCharacterInventory)
      {
        MyInventory inventory = MyEntityExtensions.GetInventory(MySession.Static.LocalCharacter);
        if (inventory != null)
        {
          list.Insert(0, new MyInventoryTargetModel((MyInventoryBase) inventory)
          {
            Name = MyTexts.GetString(MySpaceTexts.InventorySelection_Character),
            EntityId = MySession.Static.LocalCharacterEntityId,
            Volume = (float) inventory.CurrentVolume * 1000f,
            MaxVolume = (float) inventory.MaxVolume * 1000f
          });
          inventory.InventoryContentChanged += new Action<MyInventoryBase, MyPhysicalInventoryItem, MyFixedPoint>(this.OnInventoryContentChanged);
          inventory.ContentsChanged += new Action<MyInventoryBase>(this.Inventory_ContentsChanged);
        }
      }
      this.UnsubscribeEvents();
      this.Inventories = new ObservableCollection<MyInventoryTargetModel>(list);
      if (this.Inventories.Count > 0)
        this.SelectedInventoryIndex = 0;
      this.UpdateLocalPlayerCurrency();
    }

    private void UpdateGasTankVolume(MyInventoryTargetModel inventoryModel, MyGasTank gasTank)
    {
      inventoryModel.Volume = (float) Math.Round(gasTank.FilledRatio * (double) gasTank.Capacity, 0);
      inventoryModel.MaxVolume = gasTank.Capacity;
    }

    private void Inventory_ContentsChanged(MyInventoryBase inventory)
    {
      if (this.SelectedInventoryIndex < 0 || this.SelectedInventoryIndex >= this.Inventories.Count)
        return;
      MyInventoryTargetModel inventory1 = this.Inventories[this.SelectedInventoryIndex];
      if (inventory1.Inventory != inventory)
        return;
      inventory1.Volume = (float) inventory.CurrentVolume * 1000f;
      this.UpdateVolumeText();
    }

    private void OnInventoryContentChanged(
      MyInventoryBase inventory,
      MyPhysicalInventoryItem item,
      MyFixedPoint amount)
    {
      if (this.SelectedInventoryIndex < 0 || this.SelectedInventoryIndex >= this.Inventories.Count || this.Inventories[this.SelectedInventoryIndex].Inventory != inventory)
        return;
      MyInventoryItemModel inventoryItemModel = (MyInventoryItemModel) null;
      bool flag = true;
      foreach (MyInventoryItemModel inventoryItem in (Collection<MyInventoryItemModel>) this.InventoryItems)
      {
        if (inventoryItem.Inventory == inventory && (int) item.ItemId == (int) inventoryItem.InventoryItem.ItemId)
        {
          if (item.Amount == (MyFixedPoint) 0)
            inventoryItemModel = inventoryItem;
          else
            inventoryItem.Amount += (float) amount;
          flag = false;
          break;
        }
      }
      if (flag)
        this.InventoryItems.Add(new MyInventoryItemModel(item, inventory));
      if (inventoryItemModel == null)
        return;
      this.InventoryItems.Remove(inventoryItemModel);
    }

    internal void UnsubscribeEvents()
    {
      ObservableCollection<MyInventoryTargetModel> inventories = this.Inventories;
      // ISSUE: explicit non-virtual call
      if ((inventories != null ? (__nonvirtual (inventories.Count) > 0 ? 1 : 0) : 0) == 0)
        return;
      foreach (MyInventoryTargetModel inventory in (Collection<MyInventoryTargetModel>) this.Inventories)
      {
        if (inventory.Inventory != null)
        {
          inventory.Inventory.InventoryContentChanged -= new Action<MyInventoryBase, MyPhysicalInventoryItem, MyFixedPoint>(this.OnInventoryContentChanged);
          inventory.Inventory.ContentsChanged -= new Action<MyInventoryBase>(this.Inventory_ContentsChanged);
          if (inventory.GasTank != null)
            inventory.GasTank.FilledRatioChanged = (Action) null;
        }
      }
    }

    private void UpdateVolumeText()
    {
      if (this.SelectedInventoryIndex < 0 || this.SelectedInventoryIndex >= this.Inventories.Count)
        return;
      MyInventoryTargetModel inventory1 = this.Inventories[this.SelectedInventoryIndex];
      string format = MyTexts.GetString(MySpaceTexts.ScreenTerminalInventory_VolumeValue);
      if (MySession.Static.CreativeMode && inventory1.Inventory is MyInventory inventory2 && inventory2.IsCharacterOwner)
      {
        this.CurrentInventoryVolume = string.Format(format, (object) MyValueFormatter.GetFormatedFloat(inventory1.Volume, 2, ","));
        this.MaxInventoryVolume = MyTexts.GetString(MySpaceTexts.ScreenTerminalInventory_UnlimitedVolume);
      }
      else
      {
        this.CurrentInventoryVolume = string.Format(format, (object) MyValueFormatter.GetFormatedFloat(inventory1.Volume, 2, ","));
        this.MaxInventoryVolume = string.Format(format, (object) MyValueFormatter.GetFormatedFloat(inventory1.MaxVolume, 2, ","));
      }
    }

    public void UpdateInventoryContent()
    {
      if (this.SelectedInventoryIndex < 0 || this.SelectedInventoryIndex >= this.Inventories.Count)
        return;
      MyInventoryTargetModel inventory1 = this.Inventories[this.SelectedInventoryIndex];
      List<MyInventoryItemModel> inventoryItemModelList = new List<MyInventoryItemModel>();
      if (inventory1.AllInventories)
      {
        foreach (MyInventoryTargetModel inventory2 in (Collection<MyInventoryTargetModel>) this.Inventories)
        {
          if (!inventory2.AllInventories)
          {
            long entityId = inventory2.EntityId;
            MyInventoryTargetViewModel.AddInventoryItems(inventory2.Inventory, entityId, inventoryItemModelList);
          }
        }
      }
      else
      {
        long entityId = inventory1.EntityId;
        MyInventoryTargetViewModel.AddInventoryItems(inventory1.Inventory, entityId, inventoryItemModelList);
      }
      this.InventoryItems = new ObservableCollection<MyInventoryItemModel>(inventoryItemModelList);
      this.UpdateLocalPlayerCurrency();
    }

    internal void UpdateLocalPlayerCurrency()
    {
      MyBankingSystem.Static.TryGetAccountInfo(MySession.Static.LocalPlayerId, out this.m_localPlayerAccountInfo);
      this.LocalPlayerCurrency = MyBankingSystem.GetFormatedValue(this.m_localPlayerAccountInfo.Balance);
    }

    private static void AddInventoryItems(
      MyInventoryBase inventory,
      long entityId,
      List<MyInventoryItemModel> invItems)
    {
      MyEntity entity = (MyEntity) null;
      if (!MyEntities.TryGetEntityById(entityId, out entity))
        return;
      foreach (MyPhysicalInventoryItem inventoryItem in inventory.GetItems())
      {
        MyInventoryItemModel inventoryItemModel = new MyInventoryItemModel(inventoryItem, inventory);
        invItems.Add(inventoryItemModel);
      }
    }

    internal float GetItemAmount(MyDefinitionId itemDefinitionId)
    {
      float num = 0.0f;
      foreach (MyInventoryItemModel inventoryItem in (Collection<MyInventoryItemModel>) this.InventoryItems)
      {
        if (inventoryItem.InventoryItem.Content.GetId() == itemDefinitionId)
          num += inventoryItem.Amount;
      }
      return num;
    }

    public void ShowNextInventory()
    {
      int num = this.SelectedInventoryIndex + 1;
      if (num >= this.Inventories.Count)
        num = 0;
      this.SelectedInventoryIndex = num;
    }

    public void ShowPreviousInventory()
    {
      int num = this.SelectedInventoryIndex - 1;
      if (num < 0)
        num = this.Inventories.Count - 1;
      this.SelectedInventoryIndex = num;
    }
  }
}
