// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.ViewModels.MyContractsBlockActiveViewModel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Input;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.GameSystems.BankingAndCurrency;
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
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Input;
using VRage.Utils;

namespace Sandbox.Game.Screens.ViewModels
{
  public class MyContractsBlockActiveViewModel : MyViewModelBase
  {
    private MyContractBlock m_contractBlock;
    private long m_stationId;
    private long m_blockId;
    private int m_selectedActiveContractIndex;
    private int? m_previousSelectedActiveContractIndex;
    private int m_selectedGridIndex;
    private int m_activeContractCount;
    private int m_activeContractCountMax;
    private bool m_isFinishEnabled;
    private bool m_isAbandonEnabled;
    private bool m_isVisibleGridSelection;
    private bool m_isGridConfirmEnabled;
    private bool m_isNoActiveContractVisible;
    private ICommand m_sortingActiveContractCommand;
    private ICommand m_finishCommand;
    private ICommand m_refreshActiveCommand;
    private ICommand m_abandonCommand;
    private ICommand m_confirmGridSelectionCommand;
    private ICommand m_exitGridSelectionCommand;
    private MyContractModel m_selectedActiveContract;
    private ObservableCollection<MyContractModel> m_activeContracts;
    private ObservableCollection<MyContractConditionModel> m_activeConditions;
    private ObservableCollection<MySimpleSelectableItemModel> m_selectableGrids;
    private long m_gridSelectionContractId;
    private string m_lastSortActive = string.Empty;
    private bool m_isLastSortActiveAsc;
    private bool m_isWaitingForAbandon;
    private bool m_isWaitingForFinish;
    private string m_finishTooltipText;

    public int SelectedActiveContractIndex
    {
      get => this.m_selectedActiveContractIndex;
      set
      {
        this.SetProperty<int>(ref this.m_selectedActiveContractIndex, value, nameof (SelectedActiveContractIndex));
        this.UpdateFinish();
        this.UpdateAbandon();
      }
    }

    public MyContractModel SelectedActiveContract
    {
      get => this.m_selectedActiveContract;
      set => this.SetProperty<MyContractModel>(ref this.m_selectedActiveContract, value, nameof (SelectedActiveContract));
    }

    public int SelectedTargetIndex
    {
      get => this.m_selectedGridIndex;
      set
      {
        this.SetProperty<int>(ref this.m_selectedGridIndex, value, nameof (SelectedTargetIndex));
        this.UpdateGridConfirm();
      }
    }

    private bool IsWaitingForAbandon
    {
      get => this.m_isWaitingForAbandon;
      set
      {
        this.SetProperty<bool>(ref this.m_isWaitingForAbandon, value, nameof (IsWaitingForAbandon));
        this.UpdateAbandon();
      }
    }

    private bool IsWaitingForFinish
    {
      get => this.m_isWaitingForFinish;
      set
      {
        this.SetProperty<bool>(ref this.m_isWaitingForFinish, value, nameof (IsWaitingForFinish));
        this.UpdateFinish();
      }
    }

    public string FinishTooltipText
    {
      get => this.m_finishTooltipText;
      set => this.SetProperty<string>(ref this.m_finishTooltipText, value, nameof (FinishTooltipText));
    }

    public int ActiveContractCount
    {
      get => this.m_activeContractCount;
      set
      {
        this.SetProperty<int>(ref this.m_activeContractCount, value, nameof (ActiveContractCount));
        this.RaisePropertyChanged("ActiveContractCountStatus");
      }
    }

    public int ActiveContractCountMax
    {
      get => this.m_activeContractCountMax;
      set
      {
        this.SetProperty<int>(ref this.m_activeContractCountMax, value, nameof (ActiveContractCountMax));
        this.RaisePropertyChanged("ActiveContractCountStatus");
      }
    }

    public bool IsNoActiveContractVisible
    {
      get => this.m_isNoActiveContractVisible;
      set => this.SetProperty<bool>(ref this.m_isNoActiveContractVisible, value, nameof (IsNoActiveContractVisible));
    }

    public ICommand ExitGridSelectionCommand
    {
      get => this.m_exitGridSelectionCommand;
      set => this.SetProperty<ICommand>(ref this.m_exitGridSelectionCommand, value, nameof (ExitGridSelectionCommand));
    }

    public ICommand SortingActiveContractCommand
    {
      get => this.m_sortingActiveContractCommand;
      set => this.SetProperty<ICommand>(ref this.m_sortingActiveContractCommand, value, nameof (SortingActiveContractCommand));
    }

    public ICommand FinishCommand
    {
      get => this.m_finishCommand;
      set => this.SetProperty<ICommand>(ref this.m_finishCommand, value, nameof (FinishCommand));
    }

    public ICommand RefreshActiveCommand
    {
      get => this.m_refreshActiveCommand;
      set => this.SetProperty<ICommand>(ref this.m_refreshActiveCommand, value, nameof (RefreshActiveCommand));
    }

    public ICommand AbandonCommand
    {
      get => this.m_abandonCommand;
      set => this.SetProperty<ICommand>(ref this.m_abandonCommand, value, nameof (AbandonCommand));
    }

    public ICommand ConfirmGridSelectionCommand
    {
      get => this.m_confirmGridSelectionCommand;
      set => this.SetProperty<ICommand>(ref this.m_confirmGridSelectionCommand, value, nameof (ConfirmGridSelectionCommand));
    }

    public bool IsFinishEnabled
    {
      get => this.m_isFinishEnabled;
      set => this.SetProperty<bool>(ref this.m_isFinishEnabled, value, nameof (IsFinishEnabled));
    }

    public bool IsAbandonEnabled
    {
      get => this.m_isAbandonEnabled;
      set => this.SetProperty<bool>(ref this.m_isAbandonEnabled, value, nameof (IsAbandonEnabled));
    }

    public bool IsVisibleGridSelection
    {
      get => this.m_isVisibleGridSelection;
      set => this.SetProperty<bool>(ref this.m_isVisibleGridSelection, value, nameof (IsVisibleGridSelection));
    }

    public bool IsGridConfirmEnabled
    {
      get => this.m_isGridConfirmEnabled;
      set => this.SetProperty<bool>(ref this.m_isGridConfirmEnabled, value, nameof (IsGridConfirmEnabled));
    }

    public ObservableCollection<MyContractModel> ActiveContracts
    {
      get => this.m_activeContracts;
      set
      {
        this.SetProperty<ObservableCollection<MyContractModel>>(ref this.m_activeContracts, value, nameof (ActiveContracts));
        this.IsNoActiveContractVisible = value == null || value.Count == 0;
      }
    }

    public ObservableCollection<MySimpleSelectableItemModel> SelectableTargets
    {
      get => this.m_selectableGrids;
      set => this.SetProperty<ObservableCollection<MySimpleSelectableItemModel>>(ref this.m_selectableGrids, value, nameof (SelectableTargets));
    }

    public long GridSelectionContractId
    {
      get => this.m_gridSelectionContractId;
      set => this.SetProperty<long>(ref this.m_gridSelectionContractId, value, nameof (GridSelectionContractId));
    }

    public string ActiveContractCountStatus => string.Format("{0}/{1}", (object) this.ActiveContractCount, (object) this.ActiveContractCountMax);

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

    public MyContractsBlockActiveViewModel(MyContractBlock contractBlock)
      : base()
    {
      this.m_contractBlock = contractBlock;
      this.SortingActiveContractCommand = (ICommand) new RelayCommand<DataGridSortingEventArgs>(new Action<DataGridSortingEventArgs>(this.OnSortingActiveContract));
      this.FinishCommand = (ICommand) new RelayCommand(new Action<object>(this.OnFinish));
      this.RefreshActiveCommand = (ICommand) new RelayCommand(new Action<object>(this.OnRefreshActive));
      this.AbandonCommand = (ICommand) new RelayCommand(new Action<object>(this.OnAbandon));
      this.ConfirmGridSelectionCommand = (ICommand) new RelayCommand(new Action<object>(this.OnConfirmGridSelection));
      this.ExitGridSelectionCommand = (ICommand) new RelayCommand(new Action<object>(this.OnExitGridSelection));
      this.ActiveContractCountMax = MySession.Static.GetComponent<MySessionComponentContractSystem>().GetContractLimitPerPlayer();
      this.IsVisibleGridSelection = false;
      this.SetDefaultActiveContractIndex();
    }

    public override void InitializeData() => this.m_contractBlock.GetActiveContracts(MySession.Static.LocalPlayerId, new Action<List<MyObjectBuilder_Contract>, long, long>(this.OnGetActiveContracts));

    private void SetDefaultActiveContractIndex()
    {
      if (this.m_previousSelectedActiveContractIndex.HasValue)
      {
        if (this.ActiveContracts == null || this.ActiveContracts.Count == 0)
        {
          this.SelectedActiveContractIndex = -1;
        }
        else
        {
          int? activeContractIndex = this.m_previousSelectedActiveContractIndex;
          int count = this.ActiveContracts.Count;
          this.SelectedActiveContractIndex = !(activeContractIndex.GetValueOrDefault() >= count & activeContractIndex.HasValue) ? this.m_previousSelectedActiveContractIndex.Value : this.ActiveContracts.Count - 1;
        }
        this.m_previousSelectedActiveContractIndex = new int?();
      }
      else
        this.SelectedActiveContractIndex = this.ActiveContracts == null || this.ActiveContracts.Count <= 0 ? -1 : 0;
      if (this.SelectedActiveContractIndex > -1)
        this.SelectedActiveContract = this.ActiveContracts[this.SelectedActiveContractIndex];
      else
        this.SelectedActiveContract = (MyContractModel) null;
    }

    public void OnGetActiveContracts(
      List<MyObjectBuilder_Contract> contracts,
      long stationId,
      long blockId)
    {
      this.StationId = stationId;
      this.BlockId = blockId;
      ObservableCollection<MyContractModel> toSort = new ObservableCollection<MyContractModel>();
      foreach (MyObjectBuilder_Contract contract in contracts)
      {
        MyContractModel instance = MyContractModelFactory.CreateInstance(contract);
        toSort.Add(instance);
      }
      if (string.IsNullOrEmpty(this.m_lastSortActive))
      {
        this.ActiveContracts = toSort;
      }
      else
      {
        DataGridSortingEventArgs sortingArgs = new DataGridSortingEventArgs(new DataGridColumn()
        {
          SortMemberPath = this.m_lastSortActive,
          SortDirection = new ListSortDirection?(this.m_isLastSortActiveAsc ? ListSortDirection.Ascending : ListSortDirection.Descending)
        });
        this.ActiveContracts = new ObservableCollection<MyContractModel>(this.SortContracts(toSort, sortingArgs, out this.m_lastSortActive, out this.m_isLastSortActiveAsc));
      }
      this.SetDefaultActiveContractIndex();
      this.ActiveContractCount = toSort.Count;
    }

    public void OnRefreshActive(object obj) => this.m_contractBlock.GetActiveContracts(MySession.Static.LocalPlayerId, new Action<List<MyObjectBuilder_Contract>, long, long>(this.OnGetActiveContracts));

    private void OnFinish(object obj)
    {
      if (!this.IsFinishEnabled || this.SelectedActiveContractIndex < 0 || this.SelectedActiveContractIndex >= this.ActiveContracts.Count)
        return;
      MyContractModel activeContract = this.m_activeContracts[this.SelectedActiveContractIndex];
      MyContractConditionModel contractConditionModel = (MyContractConditionModel) null;
      foreach (MyContractConditionModel condition in (Collection<MyContractConditionModel>) activeContract.Conditions)
      {
        if (this.StationId > 0L && this.StationId == condition.StationEndId || this.StationId <= 0L && condition.BlockEndId == this.BlockId)
        {
          contractConditionModel = condition;
          break;
        }
      }
      switch (contractConditionModel)
      {
        case MyContractConditionDeliverItemModel _:
          this.IsWaitingForFinish = true;
          this.m_contractBlock.GetConnectedEntities(MySession.Static.LocalPlayerId, activeContract.Id, new Action<bool, List<MyContractBlock.MyTargetEntityInfoWrapper>, long>(this.OnFinishTargetSelectionCallback));
          break;
        case MyContractConditionDeliverPackageModel _:
          this.IsWaitingForFinish = true;
          long targetEntityId1 = 0;
          if (MySession.Static.LocalCharacter != null)
            targetEntityId1 = MySession.Static.LocalCharacter.EntityId;
          this.m_contractBlock.FinishContract(MySession.Static.LocalPlayerId, activeContract.Id, targetEntityId1, new Action<MyContractResults>(this.OnFinishCallback));
          break;
        case MyContractConditionCustomModel _:
          this.IsWaitingForFinish = true;
          long targetEntityId2 = 0;
          if (MySession.Static.LocalCharacter != null)
            targetEntityId2 = MySession.Static.LocalCharacter.EntityId;
          this.m_contractBlock.FinishContract(MySession.Static.LocalPlayerId, activeContract.Id, targetEntityId2, new Action<MyContractResults>(this.OnFinishCallback));
          break;
      }
    }

    private void OnAbandon(object obj)
    {
      if (this.SelectedActiveContractIndex < 0 || this.SelectedActiveContractIndex >= this.ActiveContracts.Count)
        return;
      MyContractModel cont = this.m_activeContracts[this.SelectedActiveContractIndex];
      this.ShowMessageBoxYesNo(MyTexts.Get(MySpaceTexts.Contracts_AbandonConfirmation_Caption), MyTexts.Get(MySpaceTexts.Contracts_AbandonConfirmation_Text), (Action<MyGuiScreenMessageBox.ResultEnum>) (retval =>
      {
        if (retval != MyGuiScreenMessageBox.ResultEnum.YES)
          return;
        this.IsWaitingForAbandon = true;
        this.m_contractBlock.AbandonContract(MySession.Static.LocalPlayerId, cont.Id, new Action<MyContractResults>(this.OnAbandonCallback));
      }));
    }

    private void OnConfirmGridSelection(object obj)
    {
      if (this.SelectedTargetIndex < 0 || this.SelectedTargetIndex >= this.SelectableTargets.Count)
        return;
      MySimpleSelectableItemModel selectableTarget = this.SelectableTargets[this.SelectedTargetIndex];
      if (selectableTarget == null)
        return;
      this.IsWaitingForFinish = true;
      this.m_contractBlock.FinishContract(MySession.Static.LocalPlayerId, this.GridSelectionContractId, selectableTarget.Id, new Action<MyContractResults>(this.OnFinishCallback));
      this.IsVisibleGridSelection = false;
      InputManager.Current.NavigateTabIndex(50, false);
    }

    private void OnExitGridSelection(object obj)
    {
      this.IsVisibleGridSelection = false;
      InputManager.Current.NavigateTabIndex(50, false);
    }

    public void OnFinishCallback(MyContractResults result)
    {
      if (result == MyContractResults.Success)
      {
        MyContractModel activeContract = this.m_activeContracts[this.SelectedActiveContractIndex];
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendFormat(MySpaceTexts.Contracts_Completed_Text, (object) activeContract.RewardReputation, (object) MyBankingSystem.GetFormatedValue(activeContract.RewardMoney, true));
        this.ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Completed_Caption), stringBuilder);
      }
      else
        this.ShowErrorFailNotification(result);
      this.m_previousSelectedActiveContractIndex = new int?(this.SelectedActiveContractIndex);
      this.OnRefreshActive((object) null);
      this.IsWaitingForFinish = false;
    }

    public void OnAbandonCallback(MyContractResults result)
    {
      if (result != MyContractResults.Success)
        this.ShowErrorFailNotification(result);
      this.m_previousSelectedActiveContractIndex = new int?(this.SelectedActiveContractIndex);
      this.OnRefreshActive((object) null);
      this.IsWaitingForAbandon = false;
    }

    public void OnFinishTargetSelectionCallback(
      bool isSuccessful,
      List<MyContractBlock.MyTargetEntityInfoWrapper> availableTargets,
      long contractId)
    {
      if (!isSuccessful)
      {
        this.ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_NoAccess), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_NoAccess));
      }
      else
      {
        ObservableCollection<MySimpleSelectableItemModel> observableCollection = new ObservableCollection<MySimpleSelectableItemModel>();
        foreach (MyContractBlock.MyTargetEntityInfoWrapper availableTarget in availableTargets)
          observableCollection.Add(new MySimpleSelectableItemModel(availableTarget.Id, availableTarget.Name, availableTarget.DisplayName));
        this.SelectableTargets = observableCollection;
        this.GridSelectionContractId = contractId;
        if (this.SelectableTargets.Count > 0)
          this.SelectedTargetIndex = 0;
        this.IsVisibleGridSelection = true;
        this.IsWaitingForFinish = false;
        InputManager.Current.NavigateTabIndex(500, false);
      }
    }

    private void UpdateFinish()
    {
      if (this.IsWaitingForFinish || this.SelectedActiveContractIndex < 0 || this.SelectedActiveContractIndex >= this.ActiveContracts.Count)
      {
        this.IsFinishEnabled = false;
      }
      else
      {
        MyContractModel activeContract = this.m_activeContracts[this.SelectedActiveContractIndex];
        if (activeContract.CanBeFinishedInTerminal)
        {
          bool flag = false;
          foreach (MyContractConditionModel condition in (Collection<MyContractConditionModel>) activeContract.Conditions)
          {
            if (this.StationId > 0L && this.StationId == condition.StationEndId || this.StationId <= 0L && condition.BlockEndId == this.BlockId)
            {
              flag = true;
              break;
            }
          }
          if (flag)
          {
            this.IsFinishEnabled = true;
            this.FinishTooltipText = MyTexts.GetString(MySpaceTexts.Economy_Contract_FinishTooltip_YouCanFinish);
          }
          else
          {
            this.IsFinishEnabled = false;
            this.FinishTooltipText = MyTexts.GetString(MySpaceTexts.Economy_Contract_FinishTooltip_NotAFinishPoint);
          }
        }
        else
        {
          this.IsFinishEnabled = false;
          this.FinishTooltipText = MyTexts.GetString(MySpaceTexts.Economy_Contract_FinishTooltip_CannotFinishInBlock);
        }
      }
    }

    private void UpdateAbandon()
    {
      if (this.IsWaitingForAbandon || this.SelectedActiveContractIndex < 0 || this.SelectedActiveContractIndex >= this.ActiveContracts.Count)
        this.IsAbandonEnabled = false;
      else
        this.IsAbandonEnabled = true;
    }

    private void UpdateGridConfirm()
    {
      if (this.IsWaitingForFinish || this.SelectedTargetIndex < 0 || this.SelectedTargetIndex >= this.SelectableTargets.Count)
        this.IsGridConfirmEnabled = false;
      else
        this.IsGridConfirmEnabled = true;
    }

    private void OnSortingActiveContract(DataGridSortingEventArgs sortingArgs)
    {
      this.ActiveContracts = new ObservableCollection<MyContractModel>(this.SortContracts(this.ActiveContracts, sortingArgs, out this.m_lastSortActive, out this.m_isLastSortActiveAsc));
      this.SetDefaultActiveContractIndex();
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
      MyGuiScreenMessageBox messageBox = MyGuiSandbox.CreateMessageBox(messageText: text, messageCaption: messageCaption, canHideOthers: false, useOpacity: false);
      messageBox.OkGamepadStateType = MyControlStateType.NEW_RELEASED;
      MyScreenManager.AddScreen((MyGuiScreenBase) messageBox);
    }

    private void ShowMessageBoxYesNo(
      StringBuilder caption,
      StringBuilder text,
      Action<MyGuiScreenMessageBox.ResultEnum> callback)
    {
      StringBuilder messageCaption = caption;
      MyScreenManager.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: text, messageCaption: messageCaption, callback: callback, canHideOthers: false, useOpacity: false));
    }
  }
}
