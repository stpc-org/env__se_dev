// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.ViewModels.MyContractsBlockViewModel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media.Imaging;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Utils;

namespace Sandbox.Game.Screens.ViewModels
{
  public class MyContractsBlockViewModel : MyViewModelBase
  {
    private MyContractBlock m_contractBlock;
    private bool m_isNpc;
    private long m_stationId;
    private long m_blockId;
    private int m_selectedAvailableContractIndex;
    private int? m_previousSelectedAvailableContractIndex;
    private bool m_isAcceptEnabled;
    private ICommand m_sortingAvailableContractCommand;
    private ICommand m_acceptCommand;
    private ICommand m_refreshAvailableCommand;
    private MyContractModel m_selectedAvailableContract;
    private ObservableCollection<MyContractModel> m_availableContracts;
    private ObservableCollection<MyContractModel> m_availableContractsComplete;
    private bool m_isWaitingForAccept;
    private string m_lastSortAvailable = string.Empty;
    private bool m_isLastSortAvailableAsc;
    private bool m_isAdministrationVisible;
    private ObservableCollection<MyContractTypeFilterItemModel> m_filterTargets;
    private int m_filterTargetIndex = -1;
    private MyContractTypeFilterItemModel m_selectedFilterTarget;
    private bool m_isNoAvailableContractVisible;

    public bool IsAdministrationVisible
    {
      get => this.m_isAdministrationVisible;
      set => this.SetProperty<bool>(ref this.m_isAdministrationVisible, value, nameof (IsAdministrationVisible));
    }

    public int SelectedAvailableContractIndex
    {
      get => this.m_selectedAvailableContractIndex;
      set
      {
        this.SetProperty<int>(ref this.m_selectedAvailableContractIndex, value, nameof (SelectedAvailableContractIndex));
        this.UpdateAccept();
      }
    }

    public MyContractModel SelectedAvailableContract
    {
      get => this.m_selectedAvailableContract;
      set => this.SetProperty<MyContractModel>(ref this.m_selectedAvailableContract, value, nameof (SelectedAvailableContract));
    }

    public ICommand SortingAvailableContractCommand
    {
      get => this.m_sortingAvailableContractCommand;
      set => this.SetProperty<ICommand>(ref this.m_sortingAvailableContractCommand, value, nameof (SortingAvailableContractCommand));
    }

    public ICommand AcceptCommand
    {
      get => this.m_acceptCommand;
      set => this.SetProperty<ICommand>(ref this.m_acceptCommand, value, nameof (AcceptCommand));
    }

    public ICommand RefreshAvailableCommand
    {
      get => this.m_refreshAvailableCommand;
      set => this.SetProperty<ICommand>(ref this.m_refreshAvailableCommand, value, nameof (RefreshAvailableCommand));
    }

    private bool IsWaitingForAccept
    {
      get => this.m_isWaitingForAccept;
      set
      {
        this.SetProperty<bool>(ref this.m_isWaitingForAccept, value, nameof (IsWaitingForAccept));
        this.UpdateAccept();
      }
    }

    public bool IsNoAvailableContractVisible
    {
      get => this.m_isNoAvailableContractVisible;
      set => this.SetProperty<bool>(ref this.m_isNoAvailableContractVisible, value, nameof (IsNoAvailableContractVisible));
    }

    public bool IsAcceptEnabled
    {
      get => this.m_isAcceptEnabled;
      set => this.SetProperty<bool>(ref this.m_isAcceptEnabled, value, nameof (IsAcceptEnabled));
    }

    public bool IsNpc
    {
      get => this.m_isNpc;
      set => this.SetProperty<bool>(ref this.m_isNpc, value, nameof (IsNpc));
    }

    public long StationId
    {
      get => this.m_stationId;
      set => this.SetProperty<long>(ref this.m_stationId, value, nameof (StationId));
    }

    public long BlockId
    {
      get => this.m_blockId;
      set => this.SetProperty<long>(ref this.m_blockId, value, nameof (BlockId));
    }

    public ObservableCollection<MyContractModel> AvailableContractsComplete
    {
      get => this.m_availableContractsComplete;
      set
      {
        this.SetProperty<ObservableCollection<MyContractModel>>(ref this.m_availableContractsComplete, value, nameof (AvailableContractsComplete));
        this.RefilterAvailableContracts();
      }
    }

    public ObservableCollection<MyContractModel> AvailableContracts
    {
      get => this.m_availableContracts;
      set
      {
        this.SetProperty<ObservableCollection<MyContractModel>>(ref this.m_availableContracts, value, nameof (AvailableContracts));
        this.IsNoAvailableContractVisible = value == null || value.Count == 0;
      }
    }

    public ObservableCollection<MyContractTypeFilterItemModel> FilterTargets
    {
      get => this.m_filterTargets;
      set => this.SetProperty<ObservableCollection<MyContractTypeFilterItemModel>>(ref this.m_filterTargets, value, nameof (FilterTargets));
    }

    public int FilterTargetIndex
    {
      get => this.m_filterTargetIndex;
      set
      {
        this.SetProperty<int>(ref this.m_filterTargetIndex, value, nameof (FilterTargetIndex));
        this.RefilterAvailableContracts();
      }
    }

    public MyContractTypeFilterItemModel SelectedFilterTarget
    {
      get => this.m_selectedFilterTarget;
      set => this.SetProperty<MyContractTypeFilterItemModel>(ref this.m_selectedFilterTarget, value, nameof (SelectedFilterTarget));
    }

    private void RefilterAvailableContracts()
    {
      if (this.AvailableContractsComplete == null)
        return;
      MyDefinitionId? nullable = new MyDefinitionId?();
      if (this.FilterTargetIndex >= 0 && this.FilterTargetIndex < this.FilterTargets.Count)
        nullable = this.FilterTargets[this.FilterTargetIndex].ContractTypeId;
      ObservableCollection<MyContractModel> observableCollection = new ObservableCollection<MyContractModel>();
      foreach (MyContractModel myContractModel in (Collection<MyContractModel>) this.AvailableContractsComplete)
      {
        MyDefinitionId? definitionId = myContractModel.DefinitionId;
        if (!nullable.HasValue || !definitionId.HasValue || nullable.Value == definitionId.Value)
          observableCollection.Add(myContractModel);
      }
      this.AvailableContracts = observableCollection;
      if (observableCollection.Count <= 0)
        return;
      this.SelectedAvailableContract = observableCollection[0];
    }

    public MyContractsBlockAdministrationViewModel AdministrationViewModel { get; private set; }

    public MyContractsBlockActiveViewModel ActiveViewModel { get; private set; }

    public MyContractsBlockViewModel(MyContractBlock contractBlock)
      : base()
    {
      this.SortingAvailableContractCommand = (ICommand) new RelayCommand<DataGridSortingEventArgs>(new Action<DataGridSortingEventArgs>(this.OnSortingAvailableContract));
      this.AcceptCommand = (ICommand) new RelayCommand(new Action<object>(this.OnAccept));
      this.RefreshAvailableCommand = (ICommand) new RelayCommand(new Action<object>(this.OnRefreshAvailable));
      this.m_contractBlock = contractBlock;
      MySession.Static.GetComponent<MySessionComponentContractSystem>();
      this.IsAdministrationVisible = this.m_contractBlock.HasLocalPlayerAccess() && this.m_contractBlock.OwnerId == MySession.Static.LocalPlayerId;
      this.AdministrationViewModel = new MyContractsBlockAdministrationViewModel(this.m_contractBlock);
      this.AdministrationViewModel.OnNewContractCreated += new Action(this.NewContractCreatedCallback);
      this.ActiveViewModel = new MyContractsBlockActiveViewModel(this.m_contractBlock);
      this.SetDefaultAvailableContractIndex();
      this.PrepareContractTypeFilter();
    }

    public override void InitializeData()
    {
      this.m_contractBlock.GetContractBlockStatus(new Action<bool>(this.OnGetContractBlockStatus));
      this.m_contractBlock.GetAvailableContracts(new Action<List<MyObjectBuilder_Contract>>(this.OnGetAvailableContracts));
      if (this.AdministrationViewModel != null)
        this.AdministrationViewModel.InitializeData();
      this.ActiveViewModel.InitializeData();
    }

    public override bool CanExit(object parameter) => !this.ActiveViewModel.IsVisibleGridSelection && !this.AdministrationViewModel.IsVisibleAdminSelection;

    private void PrepareContractTypeFilter()
    {
      ObservableCollection<MyContractTypeFilterItemModel> observableCollection = new ObservableCollection<MyContractTypeFilterItemModel>();
      DictionaryReader<MyDefinitionId, MyContractTypeDefinition> contractTypeDefinitions = MyDefinitionManager.Static.GetContractTypeDefinitions();
      MyContractTypeFilterItemModel typeFilterItemModel1 = new MyContractTypeFilterItemModel()
      {
        ContractTypeId = new MyDefinitionId?(),
        Name = "All",
        LocalizedName = MyTexts.GetString(MySpaceTexts.ContractType_NameLocalizationKey_All)
      };
      observableCollection.Add(typeFilterItemModel1);
      List<MyContractTypeFilterItemModel> typeFilterItemModelList = new List<MyContractTypeFilterItemModel>();
      foreach (KeyValuePair<MyDefinitionId, MyContractTypeDefinition> keyValuePair in contractTypeDefinitions)
      {
        string subtypeName = keyValuePair.Key.SubtypeName;
        string str = MyTexts.GetString(string.Format("ContractType_NameLocalizationKey_{0}", (object) subtypeName));
        BitmapImage bitmapImage = new BitmapImage();
        string[] icons = keyValuePair.Value.Icons;
        if ((icons != null ? ((uint) icons.Length > 0U ? 1 : 0) : 0) != 0)
          bitmapImage.TextureAsset = keyValuePair.Value.Icons[0];
        typeFilterItemModelList.Add(new MyContractTypeFilterItemModel()
        {
          ContractTypeId = new MyDefinitionId?(keyValuePair.Key),
          Name = subtypeName,
          LocalizedName = string.IsNullOrEmpty(str) ? subtypeName : str,
          Icon = bitmapImage
        });
      }
      typeFilterItemModelList.Sort((IComparer<MyContractTypeFilterItemModel>) new MyContractTypeFilterItemModel.MyComparator_LocalizedName());
      foreach (MyContractTypeFilterItemModel typeFilterItemModel2 in typeFilterItemModelList)
        observableCollection.Add(typeFilterItemModel2);
      this.FilterTargets = observableCollection;
      this.FilterTargetIndex = 0;
    }

    private void NewContractCreatedCallback() => this.OnRefreshAvailable((object) null);

    private void SetDefaultAvailableContractIndex()
    {
      if (this.m_previousSelectedAvailableContractIndex.HasValue)
      {
        if (this.AvailableContracts == null || this.AvailableContracts.Count == 0)
        {
          this.SelectedAvailableContractIndex = -1;
        }
        else
        {
          int? availableContractIndex = this.m_previousSelectedAvailableContractIndex;
          int count = this.AvailableContracts.Count;
          this.SelectedAvailableContractIndex = !(availableContractIndex.GetValueOrDefault() >= count & availableContractIndex.HasValue) ? this.m_previousSelectedAvailableContractIndex.Value : this.AvailableContracts.Count - 1;
        }
        this.m_previousSelectedAvailableContractIndex = new int?();
      }
      else
        this.SelectedAvailableContractIndex = this.AvailableContracts == null || this.AvailableContracts.Count <= 0 ? -1 : 0;
      if (this.m_selectedAvailableContractIndex > -1)
        this.SelectedAvailableContract = this.AvailableContracts[this.SelectedAvailableContractIndex];
      else
        this.SelectedAvailableContract = (MyContractModel) null;
    }

    public void OnGetContractBlockStatus(bool isNpc) => this.IsNpc = isNpc;

    public void OnGetAvailableContracts(List<MyObjectBuilder_Contract> contracts)
    {
      ObservableCollection<MyContractModel> toSort = new ObservableCollection<MyContractModel>();
      foreach (MyObjectBuilder_Contract contract in contracts)
      {
        MyContractModel instance = MyContractModelFactory.CreateInstance(contract, false);
        toSort.Add(instance);
      }
      if (string.IsNullOrEmpty(this.m_lastSortAvailable))
      {
        this.AvailableContractsComplete = toSort;
      }
      else
      {
        DataGridSortingEventArgs sortingArgs = new DataGridSortingEventArgs(new DataGridColumn()
        {
          SortMemberPath = this.m_lastSortAvailable,
          SortDirection = new ListSortDirection?(this.m_isLastSortAvailableAsc ? ListSortDirection.Ascending : ListSortDirection.Descending)
        });
        this.AvailableContractsComplete = new ObservableCollection<MyContractModel>(this.SortContracts(toSort, sortingArgs, out this.m_lastSortAvailable, out this.m_isLastSortAvailableAsc));
      }
      this.SetDefaultAvailableContractIndex();
    }

    private void OnAccept(object obj)
    {
      if (this.SelectedAvailableContractIndex < 0 || this.SelectedAvailableContractIndex >= this.AvailableContracts.Count)
        return;
      if (this.ActiveViewModel.ActiveContractCount >= this.ActiveViewModel.ActiveContractCountMax)
      {
        this.ShowErrorFailNotification(MyContractResults.Fail_ActivationConditionsNotMet_ContractLimitReachedHard);
      }
      else
      {
        MyContractModel cont = this.m_availableContracts[this.SelectedAvailableContractIndex];
        this.ShowMessageBoxYesNo(MyTexts.Get(MySpaceTexts.Contracts_AcceptConfirmation_Caption), MyTexts.Get(MySpaceTexts.Contracts_AcceptConfirmation_Text), (Action<MyGuiScreenMessageBox.ResultEnum>) (retval =>
        {
          if (retval != MyGuiScreenMessageBox.ResultEnum.YES)
            return;
          this.IsWaitingForAccept = true;
          this.m_contractBlock.AcceptContract(MySession.Static.LocalPlayerId, cont.Id, new Action<MyContractResults>(this.OnAcceptCallback));
        }));
      }
    }

    private void OnRefreshAvailable(object obj) => this.m_contractBlock.GetAvailableContracts(new Action<List<MyObjectBuilder_Contract>>(this.OnGetAvailableContracts));

    public void OnAcceptCallback(MyContractResults result)
    {
      this.m_previousSelectedAvailableContractIndex = new int?(this.SelectedAvailableContractIndex);
      this.OnRefreshAvailable((object) null);
      this.ActiveViewModel.OnRefreshActive((object) null);
      this.AdministrationViewModel.OnRefreshAdministrable((object) null);
      if (result != MyContractResults.Success)
        this.ShowErrorFailNotification(result);
      this.IsWaitingForAccept = false;
    }

    private void ShowErrorFailNotification(MyContractResults state)
    {
      switch (state)
      {
        case MyContractResults.Error_Unknown:
        case MyContractResults.Error_MissingKeyStructure:
        case MyContractResults.Error_InvalidData:
          MyLog.Default.Error(new StringBuilder("Contracts - error result: " + state.ToString()));
          break;
        case MyContractResults.Fail_CannotAccess:
          this.ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_NoAccess), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_NoAccess));
          break;
        case MyContractResults.Fail_ActivationConditionsNotMet:
          this.ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_ActivationConditionNotMet), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_ActivationconditionNotMet));
          break;
        case MyContractResults.Fail_ActivationConditionsNotMet_InsufficientFunds:
          this.ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_ActivationConditionNotMet_InsufficientFunds), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_ActivationConditionNotMet_InsufficientFunds));
          break;
        case MyContractResults.Fail_ActivationConditionsNotMet_InsufficientSpace:
          this.ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_ActivationConditionNotMet_InsufficientSpace), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_ActivationConditionNotMet_InsufficientSpace));
          break;
        case MyContractResults.Fail_FinishConditionsNotMet:
          this.ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_FinishingCondition), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_FinishingCondition));
          break;
        case MyContractResults.Fail_FinishConditionsNotMet_MissingPackage:
          this.ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_FinishCondition_MissingPackage), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_FinishCondition_MissingPackage));
          break;
        case MyContractResults.Fail_FinishConditionsNotMet_IncorrectTargetEntity:
          this.ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_FinishCondition_IncorrectGrid), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_FinishCondition_IncorrectGrid));
          break;
        case MyContractResults.Fail_ContractNotFound_Activation:
          this.ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_Activation), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_Activation));
          break;
        case MyContractResults.Fail_ContractNotFound_Abandon:
          this.ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_Abandon), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_Abandon));
          break;
        case MyContractResults.Fail_ContractNotFound_Finish:
          this.ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_Finish), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_Finish));
          break;
        case MyContractResults.Fail_FinishConditionsNotMet_NotEnoughItems:
          this.ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_FinishCondition_NotEnoughItems), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_FinishCondition_NotEnoughItems));
          break;
        case MyContractResults.Fail_ActivationConditionsNotMet_ContractLimitReachedHard:
          this.ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_ActivationConditionNotMet_ContractLimitReached), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_ActivationConditionNotMet_ContractLimitReached));
          break;
        case MyContractResults.Fail_ActivationConditionsNotMet_TargetOffline:
          this.ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_ActivationConditionNotMet_TargetOffline), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_ActivationConditionNotMet_TargetOffline));
          break;
        case MyContractResults.Fail_FinishConditionsNotMet_NotEnoughSpace:
          this.ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_FinishCondition_NotEnoughSpace), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_FinishCondition_NotEnoughSpace));
          break;
        case MyContractResults.Fail_ActivationConditionsNotMet_YouAreTargetOfThisHunt:
          this.ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_ActivationConditionNotMet_YouAreTargetOfThisHunt), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_ActivationConditionNotMet_YouAreTargetOfThisHunt));
          break;
      }
    }

    private void ShowMessageBoxOk(StringBuilder caption, StringBuilder text)
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

    private void UpdateAccept()
    {
      if (this.IsWaitingForAccept || this.SelectedAvailableContractIndex < 0 || this.SelectedAvailableContractIndex >= this.AvailableContracts.Count)
        this.IsAcceptEnabled = false;
      else
        this.IsAcceptEnabled = true;
    }

    private bool CanAccept(object obj) => !this.IsWaitingForAccept && this.SelectedAvailableContractIndex >= 0 && this.SelectedAvailableContractIndex < this.AvailableContracts.Count;

    private void OnSortingAvailableContract(DataGridSortingEventArgs sortingArgs)
    {
      this.AvailableContracts = new ObservableCollection<MyContractModel>(this.SortContracts(this.AvailableContracts, sortingArgs, out this.m_lastSortAvailable, out this.m_isLastSortAvailableAsc));
      this.SetDefaultAvailableContractIndex();
    }

    private IEnumerable<MyContractModel> SortContracts(
      ObservableCollection<MyContractModel> toSort,
      DataGridSortingEventArgs sortingArgs,
      out string sortParam,
      out bool sortAsc)
    {
      sortParam = string.Empty;
      sortAsc = false;
      IEnumerable<MyContractModel> myContractModels = (IEnumerable<MyContractModel>) null;
      DataGridColumn column = sortingArgs.Column;
      ListSortDirection? sortDirection = column.SortDirection;
      ref bool local = ref sortAsc;
      int num;
      if (sortDirection.HasValue)
      {
        ListSortDirection? nullable = sortDirection;
        ListSortDirection listSortDirection = ListSortDirection.Ascending;
        num = nullable.GetValueOrDefault() == listSortDirection & nullable.HasValue ? 1 : 0;
      }
      else
        num = 0;
      local = num != 0;
      string sortMemberPath = column.SortMemberPath;
      if (!(sortMemberPath == "Name"))
      {
        if (!(sortMemberPath == "Currency"))
        {
          if (!(sortMemberPath == "Reputation"))
          {
            if (sortMemberPath == "Duration")
            {
              sortParam = "Duration";
              if (sortDirection.HasValue)
              {
                ListSortDirection? nullable = sortDirection;
                ListSortDirection listSortDirection = ListSortDirection.Descending;
                if (!(nullable.GetValueOrDefault() == listSortDirection & nullable.HasValue))
                {
                  myContractModels = (IEnumerable<MyContractModel>) toSort.OrderByDescending<MyContractModel, double>((Func<MyContractModel, double>) (u => u.RemainingTime));
                  column.SortDirection = new ListSortDirection?(ListSortDirection.Descending);
                  goto label_23;
                }
              }
              myContractModels = (IEnumerable<MyContractModel>) toSort.OrderBy<MyContractModel, double>((Func<MyContractModel, double>) (u => u.RemainingTime));
              column.SortDirection = new ListSortDirection?(ListSortDirection.Ascending);
            }
          }
          else
          {
            sortParam = "Reputation";
            if (sortDirection.HasValue)
            {
              ListSortDirection? nullable = sortDirection;
              ListSortDirection listSortDirection = ListSortDirection.Descending;
              if (!(nullable.GetValueOrDefault() == listSortDirection & nullable.HasValue))
              {
                myContractModels = (IEnumerable<MyContractModel>) toSort.OrderByDescending<MyContractModel, int>((Func<MyContractModel, int>) (u => u.RewardReputation));
                column.SortDirection = new ListSortDirection?(ListSortDirection.Descending);
                goto label_23;
              }
            }
            myContractModels = (IEnumerable<MyContractModel>) toSort.OrderBy<MyContractModel, int>((Func<MyContractModel, int>) (u => u.RewardReputation));
            column.SortDirection = new ListSortDirection?(ListSortDirection.Ascending);
          }
        }
        else
        {
          sortParam = "Currency";
          if (sortDirection.HasValue)
          {
            ListSortDirection? nullable = sortDirection;
            ListSortDirection listSortDirection = ListSortDirection.Descending;
            if (!(nullable.GetValueOrDefault() == listSortDirection & nullable.HasValue))
            {
              myContractModels = (IEnumerable<MyContractModel>) toSort.OrderByDescending<MyContractModel, long>((Func<MyContractModel, long>) (u => u.RewardMoney));
              column.SortDirection = new ListSortDirection?(ListSortDirection.Descending);
              goto label_23;
            }
          }
          myContractModels = (IEnumerable<MyContractModel>) toSort.OrderBy<MyContractModel, long>((Func<MyContractModel, long>) (u => u.RewardMoney));
          column.SortDirection = new ListSortDirection?(ListSortDirection.Ascending);
        }
      }
      else
      {
        sortParam = "Name";
        if (sortDirection.HasValue)
        {
          ListSortDirection? nullable = sortDirection;
          ListSortDirection listSortDirection = ListSortDirection.Descending;
          if (!(nullable.GetValueOrDefault() == listSortDirection & nullable.HasValue))
          {
            myContractModels = (IEnumerable<MyContractModel>) toSort.OrderByDescending<MyContractModel, string>((Func<MyContractModel, string>) (u => u.NameWithId));
            column.SortDirection = new ListSortDirection?(ListSortDirection.Descending);
            goto label_23;
          }
        }
        myContractModels = (IEnumerable<MyContractModel>) toSort.OrderBy<MyContractModel, string>((Func<MyContractModel, string>) (u => u.NameWithId));
        column.SortDirection = new ListSortDirection?(ListSortDirection.Ascending);
      }
label_23:
      return myContractModels;
    }

    public override void OnScreenClosing()
    {
      if (this.AdministrationViewModel != null)
        this.AdministrationViewModel.OnNewContractCreated -= new Action(this.NewContractCreatedCallback);
      base.OnScreenClosing();
    }
  }
}
