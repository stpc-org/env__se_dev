// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.ViewModels.MyContractsActiveViewModel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Input;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using VRage;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Utils;

namespace Sandbox.Game.Screens.ViewModels
{
  public class MyContractsActiveViewModel : MyViewModelBase
  {
    private int m_selectedActiveContractIndex;
    private int m_activeContractCount;
    private int m_activeContractCountMax;
    private bool m_isAbandonEnabled;
    private ICommand m_refreshActiveCommand;
    private ICommand m_abandonCommand;
    private MyContractModel m_selectedActiveContract;
    private ObservableCollection<MyContractModel> m_activeContracts;
    private bool m_isWaitingForAbandon;
    private bool m_isNoActiveContractVisible;

    public int SelectedActiveContractIndex
    {
      get => this.m_selectedActiveContractIndex;
      set
      {
        this.SetProperty<int>(ref this.m_selectedActiveContractIndex, value, nameof (SelectedActiveContractIndex));
        this.UpdateAbandon();
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

    public int ActiveContractCount
    {
      get => this.m_activeContractCount;
      set => this.SetProperty<int>(ref this.m_activeContractCount, value, nameof (ActiveContractCount));
    }

    public int ActiveContractCountMax
    {
      get => this.m_activeContractCountMax;
      set => this.SetProperty<int>(ref this.m_activeContractCountMax, value, nameof (ActiveContractCountMax));
    }

    public bool IsAbandonEnabled
    {
      get => this.m_isAbandonEnabled;
      set => this.SetProperty<bool>(ref this.m_isAbandonEnabled, value, nameof (IsAbandonEnabled));
    }

    public bool IsNoActiveContractVisible
    {
      get => this.m_isNoActiveContractVisible;
      set => this.SetProperty<bool>(ref this.m_isNoActiveContractVisible, value, nameof (IsNoActiveContractVisible));
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

    public MyContractModel SelectedActiveContract
    {
      get => this.m_selectedActiveContract;
      set => this.SetProperty<MyContractModel>(ref this.m_selectedActiveContract, value, nameof (SelectedActiveContract));
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

    public MyContractsActiveViewModel()
      : base()
    {
      this.RefreshActiveCommand = (ICommand) new RelayCommand(new Action<object>(this.OnRefreshActive));
      this.AbandonCommand = (ICommand) new RelayCommand(new Action<object>(this.OnAbandon), new Predicate<object>(this.CanAbadon));
      this.ActiveContractCountMax = MySession.Static.GetComponent<MySessionComponentContractSystem>().GetContractLimitPerPlayer();
    }

    private bool CanAbadon(object obj) => this.IsAbandonEnabled;

    public override void InitializeData() => MyContractBlock.GetActiveContractsStatic(new Action<List<MyObjectBuilder_Contract>>(this.OnGetActiveContracts));

    public void OnRefreshActive(object obj) => MyContractBlock.GetActiveContractsStatic(new Action<List<MyObjectBuilder_Contract>>(this.OnGetActiveContracts));

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
        MyContractBlock.AbandonContractStatic(cont.Id, new Action<MyContractResults>(this.OnAbandonCallback));
      }));
    }

    private void SetDefaultActiveContractIndex()
    {
      this.SelectedActiveContractIndex = this.ActiveContracts == null || this.ActiveContracts.Count <= 0 ? -1 : 0;
      if (this.SelectedActiveContractIndex > -1)
        this.SelectedActiveContract = this.ActiveContracts[this.SelectedActiveContractIndex];
      else
        this.SelectedActiveContract = (MyContractModel) null;
    }

    private void OnGetActiveContracts(List<MyObjectBuilder_Contract> contracts)
    {
      ObservableCollection<MyContractModel> observableCollection = new ObservableCollection<MyContractModel>();
      foreach (MyObjectBuilder_Contract contract in contracts)
      {
        MyContractModel instance = MyContractModelFactory.CreateInstance(contract);
        observableCollection.Add(instance);
      }
      this.ActiveContracts = observableCollection;
      this.SetDefaultActiveContractIndex();
      this.ActiveContractCount = observableCollection.Count;
    }

    private void OnAbandonCallback(MyContractResults result)
    {
      if (result != MyContractResults.Success)
        this.ShowErrorFailNotification(result);
      this.OnRefreshActive((object) null);
      this.IsWaitingForAbandon = false;
    }

    private void ShowErrorFailNotification(MyContractResults state)
    {
      switch (state)
      {
        case MyContractResults.Success:
          MyLog.Default.WriteToLogAndAssert("Why showing error/fail message for success?");
          break;
        case MyContractResults.Error_Unknown:
        case MyContractResults.Error_MissingKeyStructure:
        case MyContractResults.Error_InvalidData:
          MyLog.Default.WriteToLogAndAssert("Contracts - error result: " + state.ToString());
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
        default:
          MyLog.Default.WriteToLogAndAssert("Missing case in switch.");
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

    private void UpdateAbandon()
    {
      if (this.IsWaitingForAbandon || this.SelectedActiveContractIndex < 0 || this.SelectedActiveContractIndex >= this.ActiveContracts.Count)
        this.IsAbandonEnabled = false;
      else
        this.IsAbandonEnabled = true;
    }
  }
}
