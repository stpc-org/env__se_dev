// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.ViewModels.MyContractsBlockAdministrationViewModel
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
using VRage.Collections;
using VRage.Game;
using VRage.Game.ObjectBuilders.Components.Contracts;

namespace Sandbox.Game.Screens.ViewModels
{
  public class MyContractsBlockAdministrationViewModel : ViewModelBase
  {
    private MyContractBlock m_contractBlock;
    private int m_createdContractCount;
    private int m_createdContractCountMax;
    private int m_tabIndexDown;
    private MyContractModel m_selectedAdministrableContract;
    private int m_selectedContractTypeIndex;
    private float m_newContractCurrencyReward;
    private float m_newContractStartDeposit;
    private float m_newContractDurationInMin;
    private string m_newContracSelectionName;
    private long m_newContracSelectionId;
    private MyDeliverItemModel m_newContractObtainAndDeliverSelectedItemType;
    private float m_newContractObtainAndDeliverItemAmount;
    private float m_newContractFindSearchRadius;
    private bool m_isVisibleAdminSelection;
    private string m_adminSelectionCaption;
    private string m_adminSelectionText;
    private int m_adminSelectedItemIndex;
    private bool m_isNoAdministrableContractVisible;
    private long m_currentMoney;
    private bool m_isDeleteEnabled;
    private BitmapImage m_currencyIcon;
    private MyContractsBlockAdministrationViewModel.MyAdminSelectionDialogTypes m_selectionDialogType;
    private ICommand m_newContractDeliverBlockSelectCommand;
    private ICommand m_newContractObtainAndDeliverBlockSelectCommand;
    private ICommand m_newContractFindGridSelectCommand;
    private ICommand m_newContractRepairGridSelectCommand;
    private ICommand m_adminSelectionConfirmCommand;
    private ICommand m_adminSelectionExitCommand;
    private ICommand m_deleteCommand;
    private ICommand m_refreshCommand;
    private ICommand m_createCommand;
    private ObservableCollection<MyContractModel> m_administrableContracts;
    private ObservableCollection<MyContractTypeModel> m_contractTypes;
    private ObservableCollection<MyDeliverItemModel> m_deliverableItems;
    private ObservableCollection<MyAdminSelectionItemModel> m_adminSelectionItems;
    public Action OnNewContractCreated;

    public MyContractModel SelectedAdministrableContract
    {
      get => this.m_selectedAdministrableContract;
      set
      {
        this.SetProperty<MyContractModel>(ref this.m_selectedAdministrableContract, value, nameof (SelectedAdministrableContract));
        this.UpdateDelete();
      }
    }

    public int SelectedContractTypeIndex
    {
      get => this.m_selectedContractTypeIndex;
      set
      {
        this.SetProperty<int>(ref this.m_selectedContractTypeIndex, value, nameof (SelectedContractTypeIndex));
        switch (value)
        {
          case 0:
            this.SelectionDialogType = MyContractsBlockAdministrationViewModel.MyAdminSelectionDialogTypes.DeliverBlock;
            this.TabIndexDown = 107;
            break;
          case 1:
            this.SelectionDialogType = MyContractsBlockAdministrationViewModel.MyAdminSelectionDialogTypes.ObtainAndDeliverBlock;
            this.TabIndexDown = 108;
            break;
          case 2:
            this.SelectionDialogType = MyContractsBlockAdministrationViewModel.MyAdminSelectionDialogTypes.FindGrid;
            this.TabIndexDown = 111;
            break;
          case 3:
            this.SelectionDialogType = MyContractsBlockAdministrationViewModel.MyAdminSelectionDialogTypes.Repair;
            this.TabIndexDown = 113;
            break;
          default:
            this.SelectionDialogType = MyContractsBlockAdministrationViewModel.MyAdminSelectionDialogTypes.None;
            this.TabIndexDown = 0;
            break;
        }
        this.RaisePropertyChanged("IsContractSelected_Deliver");
        this.RaisePropertyChanged("IsContractSelected_ObtainAndDeliver");
        this.RaisePropertyChanged("IsContractSelected_Find");
        this.RaisePropertyChanged("IsContractSelected_Repair");
      }
    }

    public bool IsContractSelected_Deliver => this.SelectedContractTypeIndex == 0;

    public bool IsContractSelected_ObtainAndDeliver => this.SelectedContractTypeIndex == 1;

    public bool IsContractSelected_Find => this.SelectedContractTypeIndex == 2;

    public bool IsContractSelected_Repair => this.SelectedContractTypeIndex == 3;

    public int CreatedContractCount
    {
      get => this.m_createdContractCount;
      set => this.SetProperty<int>(ref this.m_createdContractCount, value, nameof (CreatedContractCount));
    }

    public int CreatedContractCountMax
    {
      get => this.m_createdContractCountMax;
      set => this.SetProperty<int>(ref this.m_createdContractCountMax, value, nameof (CreatedContractCountMax));
    }

    public float NewContractCurrencyReward
    {
      get => this.m_newContractCurrencyReward;
      set => this.SetProperty<float>(ref this.m_newContractCurrencyReward, value, nameof (NewContractCurrencyReward));
    }

    public float NewContractStartDeposit
    {
      get => this.m_newContractStartDeposit;
      set => this.SetProperty<float>(ref this.m_newContractStartDeposit, value, nameof (NewContractStartDeposit));
    }

    public float NewContractDurationInMin
    {
      get => this.m_newContractDurationInMin;
      set => this.SetProperty<float>(ref this.m_newContractDurationInMin, value, nameof (NewContractDurationInMin));
    }

    public BitmapImage CurrencyIcon
    {
      get => this.m_currencyIcon;
      set => this.SetProperty<BitmapImage>(ref this.m_currencyIcon, value, nameof (CurrencyIcon));
    }

    public string NewContractSelectionName
    {
      get => this.m_newContracSelectionName;
      set => this.SetProperty<string>(ref this.m_newContracSelectionName, value, nameof (NewContractSelectionName));
    }

    public long NewContractSelectionId
    {
      get => this.m_newContracSelectionId;
      set => this.SetProperty<long>(ref this.m_newContracSelectionId, value, nameof (NewContractSelectionId));
    }

    public MyDeliverItemModel NewContractObtainAndDeliverSelectedItemType
    {
      get => this.m_newContractObtainAndDeliverSelectedItemType;
      set => this.SetProperty<MyDeliverItemModel>(ref this.m_newContractObtainAndDeliverSelectedItemType, value, nameof (NewContractObtainAndDeliverSelectedItemType));
    }

    public float NewContractObtainAndDeliverItemAmount
    {
      get => this.m_newContractObtainAndDeliverItemAmount;
      set => this.SetProperty<float>(ref this.m_newContractObtainAndDeliverItemAmount, value, nameof (NewContractObtainAndDeliverItemAmount));
    }

    public float NewContractFindSearchRadius
    {
      get => this.m_newContractFindSearchRadius;
      set => this.SetProperty<float>(ref this.m_newContractFindSearchRadius, value, nameof (NewContractFindSearchRadius));
    }

    public bool IsVisibleAdminSelection
    {
      get => this.m_isVisibleAdminSelection;
      set => this.SetProperty<bool>(ref this.m_isVisibleAdminSelection, value, nameof (IsVisibleAdminSelection));
    }

    public string AdminSelectionCaption
    {
      get => this.m_adminSelectionCaption;
      set => this.SetProperty<string>(ref this.m_adminSelectionCaption, value, nameof (AdminSelectionCaption));
    }

    public string AdminSelectionText
    {
      get => this.m_adminSelectionText;
      set => this.SetProperty<string>(ref this.m_adminSelectionText, value, nameof (AdminSelectionText));
    }

    public int AdminSelectedItemIndex
    {
      get => this.m_adminSelectedItemIndex;
      set => this.SetProperty<int>(ref this.m_adminSelectedItemIndex, value, nameof (AdminSelectedItemIndex));
    }

    public long CurrentMoney
    {
      get => this.m_currentMoney;
      set
      {
        this.SetProperty<long>(ref this.m_currentMoney, value, nameof (CurrentMoney));
        this.RaisePropertyChanged("CurrentMoneyFormated");
      }
    }

    public string CurrentMoneyFormated => MyBankingSystem.GetFormatedValue(this.CurrentMoney);

    public bool IsDeleteEnabled
    {
      get => this.m_isDeleteEnabled;
      set => this.SetProperty<bool>(ref this.m_isDeleteEnabled, value, nameof (IsDeleteEnabled));
    }

    public bool IsOwner { get; private set; }

    public bool IsNoAdministrableContractVisible
    {
      get => this.m_isNoAdministrableContractVisible;
      set => this.SetProperty<bool>(ref this.m_isNoAdministrableContractVisible, value, nameof (IsNoAdministrableContractVisible));
    }

    public MyContractsBlockAdministrationViewModel.MyAdminSelectionDialogTypes SelectionDialogType
    {
      get => this.m_selectionDialogType;
      set
      {
        this.SetProperty<MyContractsBlockAdministrationViewModel.MyAdminSelectionDialogTypes>(ref this.m_selectionDialogType, value, nameof (SelectionDialogType));
        this.IsVisibleAdminSelection = false;
        switch (value)
        {
          case MyContractsBlockAdministrationViewModel.MyAdminSelectionDialogTypes.DeliverBlock:
            this.AdminSelectionCaption = MyTexts.GetString(MySpaceTexts.ContractScreen_Administration_SelectionCaption_DeliverBlock);
            this.AdminSelectionText = MyTexts.GetString(MySpaceTexts.ContractScreen_Administration_SelectionText_DeliverBlock);
            break;
          case MyContractsBlockAdministrationViewModel.MyAdminSelectionDialogTypes.ObtainAndDeliverBlock:
            this.AdminSelectionCaption = MyTexts.GetString(MySpaceTexts.ContractScreen_Administration_SelectionCaption_ObtainAndDeliverBlock);
            this.AdminSelectionText = MyTexts.GetString(MySpaceTexts.ContractScreen_Administration_SelectionText_ObtainAndDeliverBlock);
            break;
          case MyContractsBlockAdministrationViewModel.MyAdminSelectionDialogTypes.FindGrid:
            this.AdminSelectionCaption = MyTexts.GetString(MySpaceTexts.ContractScreen_Administration_SelectionCaption_FindGrid);
            this.AdminSelectionText = MyTexts.GetString(MySpaceTexts.ContractScreen_Administration_SelectionText_FindGrid);
            break;
          case MyContractsBlockAdministrationViewModel.MyAdminSelectionDialogTypes.Repair:
            this.AdminSelectionCaption = MyTexts.GetString(MySpaceTexts.ContractScreen_Administration_SelectionCaption_Repair);
            this.AdminSelectionText = MyTexts.GetString(MySpaceTexts.ContractScreen_Administration_SelectionText_Repair);
            break;
          default:
            this.AdminSelectionCaption = string.Empty;
            this.AdminSelectionText = string.Empty;
            break;
        }
        this.AdminSelectedItemIndex = -1;
        this.AdminSelectionItems = new ObservableCollection<MyAdminSelectionItemModel>();
        this.ResetSelection();
      }
    }

    public ICommand NewContractDeliverBlockSelectCommand
    {
      get => this.m_newContractDeliverBlockSelectCommand;
      set => this.SetProperty<ICommand>(ref this.m_newContractDeliverBlockSelectCommand, value, nameof (NewContractDeliverBlockSelectCommand));
    }

    public ICommand NewContractObtainAndDeliverBlockSelectCommand
    {
      get => this.m_newContractObtainAndDeliverBlockSelectCommand;
      set => this.SetProperty<ICommand>(ref this.m_newContractObtainAndDeliverBlockSelectCommand, value, nameof (NewContractObtainAndDeliverBlockSelectCommand));
    }

    public ICommand NewContractFindGridSelectCommand
    {
      get => this.m_newContractFindGridSelectCommand;
      set => this.SetProperty<ICommand>(ref this.m_newContractFindGridSelectCommand, value, nameof (NewContractFindGridSelectCommand));
    }

    public ICommand NewContractRepairGridSelectCommand
    {
      get => this.m_newContractRepairGridSelectCommand;
      set => this.SetProperty<ICommand>(ref this.m_newContractRepairGridSelectCommand, value, nameof (NewContractRepairGridSelectCommand));
    }

    public ICommand AdminSelectionConfirmCommand
    {
      get => this.m_adminSelectionConfirmCommand;
      set => this.SetProperty<ICommand>(ref this.m_adminSelectionConfirmCommand, value, nameof (AdminSelectionConfirmCommand));
    }

    public ICommand AdminSelectionExitCommand
    {
      get => this.m_adminSelectionExitCommand;
      set => this.SetProperty<ICommand>(ref this.m_adminSelectionExitCommand, value, nameof (AdminSelectionExitCommand));
    }

    public ICommand DeleteCommand
    {
      get => this.m_deleteCommand;
      set => this.SetProperty<ICommand>(ref this.m_deleteCommand, value, nameof (DeleteCommand));
    }

    public ICommand RefreshCommand
    {
      get => this.m_refreshCommand;
      set => this.SetProperty<ICommand>(ref this.m_refreshCommand, value, nameof (RefreshCommand));
    }

    public ICommand CreateCommand
    {
      get => this.m_createCommand;
      set => this.SetProperty<ICommand>(ref this.m_createCommand, value, nameof (CreateCommand));
    }

    public ObservableCollection<MyContractModel> AdministrableContracts
    {
      get => this.m_administrableContracts;
      set
      {
        this.SetProperty<ObservableCollection<MyContractModel>>(ref this.m_administrableContracts, value, nameof (AdministrableContracts));
        this.IsNoAdministrableContractVisible = value == null || value.Count == 0;
      }
    }

    public ObservableCollection<MyContractTypeModel> ContractTypes
    {
      get => this.m_contractTypes;
      set
      {
        this.SetProperty<ObservableCollection<MyContractTypeModel>>(ref this.m_contractTypes, value, nameof (ContractTypes));
        if (value == null || value.Count <= 0)
          this.SelectedContractTypeIndex = -1;
        else
          this.SelectedContractTypeIndex = 0;
      }
    }

    public ObservableCollection<MyDeliverItemModel> DeliverableItems
    {
      get => this.m_deliverableItems;
      set => this.SetProperty<ObservableCollection<MyDeliverItemModel>>(ref this.m_deliverableItems, value, nameof (DeliverableItems));
    }

    public ObservableCollection<MyAdminSelectionItemModel> AdminSelectionItems
    {
      get => this.m_adminSelectionItems;
      set => this.SetProperty<ObservableCollection<MyAdminSelectionItemModel>>(ref this.m_adminSelectionItems, value, nameof (AdminSelectionItems));
    }

    public int TabIndexDown
    {
      get => this.m_tabIndexDown;
      set => this.SetProperty<int>(ref this.m_tabIndexDown, value, nameof (TabIndexDown));
    }

    public MyContractsBlockAdministrationViewModel(MyContractBlock contractBlock)
    {
      this.m_contractBlock = contractBlock;
      this.Initialize();
      BitmapImage bitmapImage = new BitmapImage();
      string[] icons = MyBankingSystem.BankingSystemDefinition.Icons;
      bitmapImage.TextureAsset = (icons != null ? ((uint) icons.Length > 0U ? 1 : 0) : 0) != 0 ? MyBankingSystem.BankingSystemDefinition.Icons[0] : string.Empty;
      this.CurrencyIcon = bitmapImage;
      this.NewContractDeliverBlockSelectCommand = (ICommand) new RelayCommand(new Action<object>(this.OnDeliverBlockSelection));
      this.NewContractObtainAndDeliverBlockSelectCommand = (ICommand) new RelayCommand(new Action<object>(this.OnObtainandDeliverBlockSelection));
      this.NewContractFindGridSelectCommand = (ICommand) new RelayCommand(new Action<object>(this.OnFindGridSelection));
      this.NewContractRepairGridSelectCommand = (ICommand) new RelayCommand(new Action<object>(this.OnRepairGridSelection));
      this.AdminSelectionConfirmCommand = (ICommand) new RelayCommand(new Action<object>(this.OnAdminSelectionConfirm));
      this.AdminSelectionExitCommand = (ICommand) new RelayCommand(new Action<object>(this.OnAdminSelectionExit));
      this.DeleteCommand = (ICommand) new RelayCommand(new Action<object>(this.OnDelete));
      this.RefreshCommand = (ICommand) new RelayCommand(new Action<object>(this.OnRefresh));
      this.CreateCommand = (ICommand) new RelayCommand(new Action<object>(this.OnCreate));
      this.CreatedContractCountMax = MySession.Static.GetComponent<MySessionComponentContractSystem>().GetContractCreationLimitPerPlayer();
      this.SelectionDialogType = MyContractsBlockAdministrationViewModel.MyAdminSelectionDialogTypes.DeliverBlock;
      this.IsVisibleAdminSelection = false;
      this.ResetSelection();
      this.UpdateMoney();
      this.TabIndexDown = 107;
    }

    internal void InitializeData()
    {
      if (!this.IsOwner)
        return;
      this.m_contractBlock.GetAdministrableContracts(new Action<List<MyObjectBuilder_Contract>>(this.OnGetAdministrableContracts));
    }

    public void Initialize()
    {
      this.IsOwner = this.m_contractBlock != null && this.m_contractBlock.OwnerId == MySession.Static.LocalPlayerId;
      ObservableCollection<MyContractTypeModel> observableCollection = new ObservableCollection<MyContractTypeModel>();
      observableCollection.Add(new MyContractTypeModel()
      {
        Name = MyTexts.GetString(MySpaceTexts.ContractTypeNames_Deliver)
      });
      observableCollection.Add(new MyContractTypeModel()
      {
        Name = MyTexts.GetString(MySpaceTexts.ContractTypeNames_ObtainAndDeliver)
      });
      observableCollection.Add(new MyContractTypeModel()
      {
        Name = MyTexts.GetString(MySpaceTexts.ContractTypeNames_Find)
      });
      observableCollection.Add(new MyContractTypeModel()
      {
        Name = MyTexts.GetString(MySpaceTexts.ContractTypeNames_Repair)
      });
      this.ContractTypes = observableCollection;
      ListReader<MyPhysicalItemDefinition> physicalItemDefinitions = MyDefinitionManager.Static.GetPhysicalItemDefinitions();
      List<MyDeliverItemModel> list = new List<MyDeliverItemModel>();
      foreach (MyPhysicalItemDefinition itemDefinition in physicalItemDefinitions)
      {
        if (itemDefinition != null && itemDefinition.Public && (itemDefinition.Enabled && itemDefinition.GetObjectBuilder().Id.TypeIdString != typeof (MyObjectBuilder_TreeObject).Name))
          list.Add(new MyDeliverItemModel(itemDefinition));
      }
      list.SortNoAlloc<MyDeliverItemModel>((Comparison<MyDeliverItemModel>) ((x, y) => string.Compare(x.Name, y.Name, StringComparison.InvariantCultureIgnoreCase)));
      this.DeliverableItems = new ObservableCollection<MyDeliverItemModel>(list);
    }

    private void OnDeliverBlockSelection(object obj) => this.DisplaySelectionScreen(MyContractsBlockAdministrationViewModel.MyAdminSelectionDialogTypes.DeliverBlock);

    private void OnObtainandDeliverBlockSelection(object obj) => this.DisplaySelectionScreen(MyContractsBlockAdministrationViewModel.MyAdminSelectionDialogTypes.ObtainAndDeliverBlock);

    private void OnFindGridSelection(object obj) => this.DisplaySelectionScreen(MyContractsBlockAdministrationViewModel.MyAdminSelectionDialogTypes.FindGrid);

    private void OnRepairGridSelection(object obj) => this.DisplaySelectionScreen(MyContractsBlockAdministrationViewModel.MyAdminSelectionDialogTypes.Repair);

    private void OnAdminSelectionConfirm(object obj)
    {
      this.IsVisibleAdminSelection = false;
      if (this.AdminSelectionItems == null || this.AdminSelectedItemIndex < 0 || this.AdminSelectedItemIndex >= this.AdminSelectionItems.Count)
      {
        this.ResetSelection();
      }
      else
      {
        this.NewContractSelectionName = this.AdminSelectionItems[this.AdminSelectedItemIndex].NameCombinedShort;
        this.NewContractSelectionId = this.AdminSelectionItems[this.AdminSelectedItemIndex].Id;
      }
      InputManager.Current.NavigateTabIndex(this.TabIndexDown, false);
    }

    private void ResetSelection()
    {
      this.NewContractSelectionName = MyTexts.GetString(MySpaceTexts.ContractScreen_Administration_NoSelection);
      this.NewContractSelectionId = -1L;
    }

    private void OnAdminSelectionExit(object obj)
    {
      this.IsVisibleAdminSelection = false;
      this.ResetSelection();
      InputManager.Current.NavigateTabIndex(this.TabIndexDown, false);
    }

    private void OnDelete(object obj)
    {
      if (this.SelectedAdministrableContract == null)
        return;
      this.m_contractBlock.DeleteCustomContract(this.SelectedAdministrableContract.Id, new Action<bool>(this.OnDeleteCustomContractCallback));
    }

    private void OnRefresh(object obj) => this.OnRefreshAdministrable((object) null);

    private void OnCreate(object obj)
    {
      if (this.CreatedContractCount >= this.CreatedContractCountMax)
        this.OnCreateContractCallback(MyContractCreationResults.Fail_CreationLimitHard);
      else if (float.IsNaN(this.NewContractCurrencyReward))
        this.DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailCaption_IsNaN_MoneyReward), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailText_IsNaN_MoneyReward));
      else if (float.IsNaN(this.NewContractStartDeposit))
        this.DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailCaption_IsNaN_StartingDeposit), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailText_IsNaN_StartingDeposit));
      else if (float.IsNaN(this.NewContractDurationInMin))
      {
        this.DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailCaption_IsNaN_Duration), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailText_IsNaN_Duration));
      }
      else
      {
        int contractCurrencyReward = (int) this.NewContractCurrencyReward;
        int contractStartDeposit = (int) this.NewContractStartDeposit;
        int contractDurationInMin = (int) this.NewContractDurationInMin;
        switch (this.SelectionDialogType)
        {
          case MyContractsBlockAdministrationViewModel.MyAdminSelectionDialogTypes.DeliverBlock:
            if (this.AdminSelectionItems != null && this.AdminSelectedItemIndex >= 0 && this.AdminSelectedItemIndex < this.AdminSelectionItems.Count)
            {
              long id = this.AdminSelectionItems[this.AdminSelectedItemIndex].Id;
              this.m_contractBlock.CreateCustomContractDeliver(contractCurrencyReward, contractStartDeposit, contractDurationInMin, id, new Action<MyContractCreationResults>(this.OnCreateContractCallback));
              break;
            }
            this.DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailCaption_TargetContractBlockNotSelected), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailText_TargetContractBlockNotSelected));
            break;
          case MyContractsBlockAdministrationViewModel.MyAdminSelectionDialogTypes.ObtainAndDeliverBlock:
            if (this.AdminSelectionItems != null && this.AdminSelectedItemIndex >= 0 && this.AdminSelectedItemIndex < this.AdminSelectionItems.Count)
            {
              long id1 = this.AdminSelectionItems[this.AdminSelectedItemIndex].Id;
              if (this.NewContractObtainAndDeliverSelectedItemType != null)
              {
                MyDefinitionId id2 = this.NewContractObtainAndDeliverSelectedItemType.ItemDefinition.Id;
                if (float.IsNaN(this.NewContractObtainAndDeliverItemAmount))
                {
                  this.DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailCaption_IsNaN_ItemAmount), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailText_IsNaN_ItemAmount));
                  break;
                }
                int deliverItemAmount = (int) this.NewContractObtainAndDeliverItemAmount;
                this.m_contractBlock.CreateCustomContractObtainAndDeliver(contractCurrencyReward, contractStartDeposit, contractDurationInMin, id1, id2, deliverItemAmount, new Action<MyContractCreationResults>(this.OnCreateContractCallback));
                break;
              }
              this.DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailCaption_ItemTypeNotSelected), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailText_ItemTypeNotSelected));
              break;
            }
            this.DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailCaption_TargetContractBlockNotSelected), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailText_TargetContractBlockNotSelected));
            break;
          case MyContractsBlockAdministrationViewModel.MyAdminSelectionDialogTypes.FindGrid:
            if (this.AdminSelectionItems != null && this.AdminSelectedItemIndex >= 0 && this.AdminSelectedItemIndex < this.AdminSelectionItems.Count)
            {
              long id = this.AdminSelectionItems[this.AdminSelectedItemIndex].Id;
              if (float.IsNaN(this.NewContractFindSearchRadius))
              {
                this.DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailCaption_IsNaN_SearchRadius), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailText_IsNaN_SearchRadius));
                break;
              }
              double findSearchRadius = (double) this.NewContractFindSearchRadius;
              this.m_contractBlock.CreateCustomContractFind(contractCurrencyReward, contractStartDeposit, contractDurationInMin, id, findSearchRadius, new Action<MyContractCreationResults>(this.OnCreateContractCallback));
              break;
            }
            this.DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailCaption_TargetGridNotSelected), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailText_TargetGridNotSelected));
            break;
          case MyContractsBlockAdministrationViewModel.MyAdminSelectionDialogTypes.Repair:
            if (this.AdminSelectionItems != null && this.AdminSelectedItemIndex >= 0 && this.AdminSelectedItemIndex < this.AdminSelectionItems.Count)
            {
              long id = this.AdminSelectionItems[this.AdminSelectedItemIndex].Id;
              this.m_contractBlock.CreateCustomContractRepair(contractCurrencyReward, contractStartDeposit, contractDurationInMin, id, new Action<MyContractCreationResults>(this.OnCreateContractCallback));
              break;
            }
            this.DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailCaption_TargetGridNotSelected), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailText_TargetGridNotSelected));
            break;
        }
      }
    }

    private void UpdateDelete()
    {
      if (this.m_selectedAdministrableContract == null)
        this.IsDeleteEnabled = false;
      else
        this.IsDeleteEnabled = true;
    }

    private void UpdateMoney()
    {
      MyBankingSystem component = MySession.Static.GetComponent<MyBankingSystem>();
      MyAccountInfo account;
      if (component == null || !component.TryGetAccountInfo(MySession.Static.LocalPlayerId, out account))
        return;
      this.CurrentMoney = account.Balance;
    }

    public void OnCreateContractCallback(MyContractCreationResults result)
    {
      switch (result)
      {
        case MyContractCreationResults.Success:
          this.DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultCaption_Success), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultText_Success));
          break;
        case MyContractCreationResults.Fail_Common:
        case MyContractCreationResults.Fail_Impossible:
        case MyContractCreationResults.Fail_NoAccess:
          this.DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultCaption_Fail), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultText_Fail));
          break;
        case MyContractCreationResults.Fail_GridNotFound:
          this.DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultCaption_GridNotFound), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultText_GridNotFound));
          break;
        case MyContractCreationResults.Fail_BlockNotFound:
          this.DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultCaption_BlockNotFound), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultText_BlockNotFound));
          break;
        case MyContractCreationResults.Error:
        case MyContractCreationResults.Error_MissingKeyStructure:
          this.DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultCaption_Error), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultText_Error));
          break;
        case MyContractCreationResults.Fail_NotAnOwnerOfBlock:
          this.DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultCaption_NotAnOwnerOfBlock), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultText_NotAnOwnerOfBlock));
          break;
        case MyContractCreationResults.Fail_NotAnOwnerOfGrid:
          this.DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultCaption_NotAnOwnerOfGrid), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultText_NotAnOwnerOfGrid));
          break;
        case MyContractCreationResults.Fail_NotEnoughFunds:
          this.DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultCaption_NotEnoughFunds), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultText_NotEnoughFunds));
          break;
        case MyContractCreationResults.Fail_CreationLimitHard:
          this.DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultCaption_CreationLimitHard), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultText_CreationLimitHard));
          break;
      }
      if (result != MyContractCreationResults.Success)
        return;
      if (this.OnNewContractCreated != null)
        this.OnNewContractCreated();
      this.OnRefreshAdministrable((object) null);
      this.UpdateMoney();
    }

    public void OnDeleteCustomContractCallback(bool success)
    {
      this.OnRefreshAdministrable((object) null);
      this.UpdateMoney();
    }

    public void OnGetAdministrableContracts(List<MyObjectBuilder_Contract> contracts)
    {
      ObservableCollection<MyContractModel> observableCollection = new ObservableCollection<MyContractModel>();
      foreach (MyObjectBuilder_Contract contract in contracts)
      {
        MyContractModel instance = MyContractModelFactory.CreateInstance(contract);
        observableCollection.Add(instance);
      }
      this.AdministrableContracts = observableCollection;
      this.CreatedContractCount = observableCollection.Count;
      this.SetDefaultAdministrableContractIndex();
    }

    private void SetDefaultAdministrableContractIndex()
    {
      if (this.AdministrableContracts.Count > 0)
        this.SelectedAdministrableContract = this.AdministrableContracts[0];
      else
        this.SelectedAdministrableContract = (MyContractModel) null;
    }

    public void OnRefreshAdministrable(object obj)
    {
      if (!this.IsOwner)
        return;
      this.m_contractBlock.GetAdministrableContracts(new Action<List<MyObjectBuilder_Contract>>(this.OnGetAdministrableContracts));
    }

    private void DisplaySelectionScreen(
      MyContractsBlockAdministrationViewModel.MyAdminSelectionDialogTypes type)
    {
      this.SelectionDialogType = type;
      this.PopulateSelectionScreen(type);
      this.IsVisibleAdminSelection = true;
      InputManager.Current.NavigateTabIndex(200, false);
    }

    private void PopulateSelectionScreen(
      MyContractsBlockAdministrationViewModel.MyAdminSelectionDialogTypes type)
    {
      switch (type)
      {
        case MyContractsBlockAdministrationViewModel.MyAdminSelectionDialogTypes.DeliverBlock:
        case MyContractsBlockAdministrationViewModel.MyAdminSelectionDialogTypes.ObtainAndDeliverBlock:
          this.m_contractBlock.GetAllOwnedContractBlocks(MySession.Static.LocalPlayerId, new Action<List<MyContractBlock.MyEntityInfoWrapper>>(this.OnGetAllOwnedContractBlocks));
          break;
        case MyContractsBlockAdministrationViewModel.MyAdminSelectionDialogTypes.FindGrid:
        case MyContractsBlockAdministrationViewModel.MyAdminSelectionDialogTypes.Repair:
          this.m_contractBlock.GetAllOwnedGrids(MySession.Static.LocalPlayerId, new Action<List<MyContractBlock.MyEntityInfoWrapper>>(this.OnGetAllOwnedGrids));
          break;
      }
    }

    public void OnGetAllOwnedContractBlocks(List<MyContractBlock.MyEntityInfoWrapper> data)
    {
      ObservableCollection<MyAdminSelectionItemModel> observableCollection = new ObservableCollection<MyAdminSelectionItemModel>();
      foreach (MyContractBlock.MyEntityInfoWrapper entityInfoWrapper in data)
        observableCollection.Add(new MyAdminSelectionItemModel(entityInfoWrapper.NamePrefix, entityInfoWrapper.NameSuffix, entityInfoWrapper.Id));
      this.AdminSelectionItems = observableCollection;
      if (observableCollection.Count <= 0)
        return;
      this.AdminSelectedItemIndex = 0;
    }

    public void OnGetAllOwnedGrids(List<MyContractBlock.MyEntityInfoWrapper> data)
    {
      ObservableCollection<MyAdminSelectionItemModel> observableCollection = new ObservableCollection<MyAdminSelectionItemModel>();
      foreach (MyContractBlock.MyEntityInfoWrapper entityInfoWrapper in data)
        observableCollection.Add(new MyAdminSelectionItemModel(entityInfoWrapper.NamePrefix, entityInfoWrapper.NameSuffix, entityInfoWrapper.Id));
      this.AdminSelectionItems = observableCollection;
      if (observableCollection.Count <= 0)
        return;
      this.AdminSelectedItemIndex = 0;
    }

    private void DisplayMessageBoxOk(StringBuilder caption, StringBuilder text)
    {
      StringBuilder messageCaption = caption;
      MyScreenManager.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: text, messageCaption: messageCaption, canHideOthers: false, useOpacity: false));
    }

    private void ShowMessageBoxYesNo(
      StringBuilder caption,
      StringBuilder text,
      Action<MyGuiScreenMessageBox.ResultEnum> callback)
    {
      StringBuilder messageCaption = caption;
      MyScreenManager.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: text, messageCaption: messageCaption, callback: callback, canHideOthers: false, useOpacity: false));
    }

    public enum MyAdminSelectionDialogTypes
    {
      None,
      DeliverBlock,
      ObtainAndDeliverBlock,
      FindGrid,
      Repair,
    }
  }
}
