// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyTerminalFactionController
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.GUI;
using Sandbox.Game.Gui.FactionTerminal;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  [StaticEventOwner]
  internal class MyTerminalFactionController : MyTerminalController
  {
    internal static readonly Color COLOR_CUSTOM_RED = new Color(228, 62, 62);
    internal static readonly Color COLOR_CUSTOM_GREEN = new Color(101, 178, 91);
    internal static readonly Color COLOR_CUSTOM_GREY = new Color(149, 169, 179);
    internal static readonly Color COLOR_CUSTOM_BLUE = new Color(117, 201, 241);
    private IMyGuiControlsParent m_controlsParent;
    private bool m_userIsFounder;
    private bool m_userIsLeader;
    private long m_selectedUserId;
    private string m_selectedUserName;
    private IMyFaction m_userFaction;
    private IMyFaction m_selectedFaction;
    private MyGuiControlTable m_tableFactions;
    private MyGuiControlCombobox m_tableFactionsFilter;
    private MyGuiControlButton m_buttonCreate;
    private MyGuiControlButton m_buttonJoin;
    private MyGuiControlButton m_buttonCancelJoin;
    private MyGuiControlButton m_buttonLeave;
    private MyGuiControlButton m_buttonSendPeace;
    private MyGuiControlButton m_buttonCancelPeace;
    private MyGuiControlButton m_buttonAcceptPeace;
    private MyGuiControlButton m_buttonMakeEnemy;
    private MyGuiControlLabel m_labelFactionDesc;
    private MyGuiControlLabel m_labelFactionPriv;
    private MyGuiControlLabel m_labelReputation;
    private MyGuiControlLabel m_textReputation;
    private MyGuiControlLabel m_labelMembers;
    private MyGuiControlLabel m_labelAutoAcceptMember;
    private MyGuiControlLabel m_labelAutoAcceptPeace;
    private MyGuiControlCheckbox m_checkAutoAcceptMember;
    private MyGuiControlCheckbox m_checkAutoAcceptPeace;
    private MyGuiControlMultilineText m_textFactionDesc;
    private MyGuiControlImage m_factionIcon;
    private MyGuiControlMultilineText m_textFactionPriv;
    private MyGuiControlTable m_tableMembers;
    private MyGuiControlButton m_buttonEdit;
    private MyGuiControlButton m_buttonKick;
    private MyGuiControlButton m_buttonPromote;
    private MyGuiControlButton m_buttonDemote;
    private MyGuiControlButton m_buttonAcceptJoin;
    private MyGuiControlButton m_buttonShareProgress;
    private MyGuiControlButton m_buttonAddNpc;
    private MyGuiControlButton m_btnWithdraw;
    private MyGuiControlButton m_btnDeposit;
    private MyGuiReputationProgressBar m_progressReputation;

    private IMyFaction TargetFaction => this.m_selectedFaction != null && MySession.Static.IsUserAdmin(Sync.MyId) ? this.m_selectedFaction : this.m_userFaction;

    public void Init(IMyGuiControlsParent controlsParent)
    {
      this.m_controlsParent = controlsParent;
      this.RefreshUserInfo();
      this.m_tableFactions = (MyGuiControlTable) controlsParent.Controls.GetControlByName("FactionsTable");
      this.m_tableFactions.SetColumnComparison(0, (Comparison<MyGuiControlTable.Cell>) ((a, b) => ((StringBuilder) a.UserData).CompareToIgnoreCase((StringBuilder) b.UserData)));
      this.m_tableFactions.SetColumnComparison(1, (Comparison<MyGuiControlTable.Cell>) ((a, b) => ((StringBuilder) a.UserData).CompareToIgnoreCase((StringBuilder) b.UserData)));
      this.m_tableFactions.ItemSelected += new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.OnFactionsTableItemSelected);
      this.RefreshTableFactions(MyTerminalFactionController.MyFactionFilter.None);
      this.m_tableFactions.SortByColumn(1);
      this.m_tableFactionsFilter = (MyGuiControlCombobox) controlsParent.Controls.GetControlByName("FactionFilters");
      this.RefreshFilterCombo();
      this.m_tableFactionsFilter.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.OnFactionFilterItemSelected);
      this.m_buttonCreate = (MyGuiControlButton) controlsParent.Controls.GetControlByName("buttonCreate");
      this.m_buttonJoin = (MyGuiControlButton) controlsParent.Controls.GetControlByName("buttonJoin");
      this.m_buttonCancelJoin = (MyGuiControlButton) controlsParent.Controls.GetControlByName("buttonCancelJoin");
      this.m_buttonLeave = (MyGuiControlButton) controlsParent.Controls.GetControlByName("buttonLeave");
      this.m_buttonSendPeace = (MyGuiControlButton) controlsParent.Controls.GetControlByName("buttonSendPeace");
      this.m_buttonCancelPeace = (MyGuiControlButton) controlsParent.Controls.GetControlByName("buttonCancelPeace");
      this.m_buttonAcceptPeace = (MyGuiControlButton) controlsParent.Controls.GetControlByName("buttonAcceptPeace");
      this.m_buttonMakeEnemy = (MyGuiControlButton) controlsParent.Controls.GetControlByName("buttonEnemy");
      this.m_buttonMakeEnemy.SetToolTip(MyTexts.GetString(MySpaceTexts.TerminalTab_Factions_EnemyToolTip));
      if (MySession.Static.Settings.EnableTeamBalancing && !MySession.Static.IsUserSpaceMaster(Sync.MyId))
        this.m_buttonLeave.SetToolTip(MyTexts.GetString(MySpaceTexts.TerminalTab_Factions_LeaveToolTip_Balancing));
      else
        this.m_buttonLeave.SetToolTip(MyTexts.GetString(MySpaceTexts.TerminalTab_Factions_LeaveToolTip));
      this.m_buttonCreate.ShowTooltipWhenDisabled = true;
      this.m_buttonCreate.TextEnum = MySpaceTexts.TerminalTab_Factions_Create;
      this.m_buttonJoin.TextEnum = MySpaceTexts.TerminalTab_Factions_Join;
      this.m_buttonCancelJoin.TextEnum = MySpaceTexts.TerminalTab_Factions_CancelJoin;
      this.m_buttonLeave.TextEnum = MySpaceTexts.TerminalTab_Factions_Leave;
      this.m_buttonSendPeace.TextEnum = MySpaceTexts.TerminalTab_Factions_Friend;
      this.m_buttonCancelPeace.TextEnum = MySpaceTexts.TerminalTab_Factions_CancelPeaceRequest;
      this.m_buttonAcceptPeace.TextEnum = MySpaceTexts.TerminalTab_Factions_AcceptPeaceRequest;
      this.m_buttonMakeEnemy.TextEnum = MySpaceTexts.TerminalTab_Factions_Enemy;
      this.m_buttonJoin.SetToolTip(MySpaceTexts.TerminalTab_Factions_JoinToolTip);
      this.m_buttonSendPeace.SetToolTip(MySpaceTexts.TerminalTab_Factions_FriendToolTip);
      this.m_buttonCreate.ButtonClicked += new Action<MyGuiControlButton>(this.OnCreateClicked);
      this.m_buttonJoin.ButtonClicked += new Action<MyGuiControlButton>(this.OnJoinClicked);
      this.m_buttonCancelJoin.ButtonClicked += new Action<MyGuiControlButton>(this.OnCancelJoinClicked);
      this.m_buttonLeave.ButtonClicked += new Action<MyGuiControlButton>(this.OnLeaveClicked);
      this.m_buttonSendPeace.ButtonClicked += new Action<MyGuiControlButton>(this.OnFriendClicked);
      this.m_buttonCancelPeace.ButtonClicked += new Action<MyGuiControlButton>(this.OnCancelPeaceRequestClicked);
      this.m_buttonAcceptPeace.ButtonClicked += new Action<MyGuiControlButton>(this.OnAcceptFriendClicked);
      this.m_buttonMakeEnemy.ButtonClicked += new Action<MyGuiControlButton>(this.OnEnemyClicked);
      this.m_labelFactionDesc = (MyGuiControlLabel) controlsParent.Controls.GetControlByName("labelFactionDesc");
      this.m_labelFactionPriv = (MyGuiControlLabel) controlsParent.Controls.GetControlByName("labelFactionPrivate");
      this.m_textReputation = (MyGuiControlLabel) controlsParent.Controls.GetControlByName("textReputation");
      this.m_labelReputation = (MyGuiControlLabel) controlsParent.Controls.GetControlByName("labelReputation");
      this.m_labelMembers = (MyGuiControlLabel) controlsParent.Controls.GetControlByName("labelFactionMembers");
      this.m_labelAutoAcceptMember = (MyGuiControlLabel) controlsParent.Controls.GetControlByName("labelFactionMembersAcceptEveryone");
      this.m_labelAutoAcceptPeace = (MyGuiControlLabel) controlsParent.Controls.GetControlByName("labelFactionMembersAcceptPeace");
      this.m_labelFactionDesc.Text = MyTexts.Get(MySpaceTexts.TerminalTab_Factions_CreateFactionDescription).ToString();
      this.m_labelFactionPriv.Text = MyTexts.Get(MySpaceTexts.TerminalTab_Factions_Private).ToString();
      this.m_labelReputation.Text = MyTexts.Get(MySpaceTexts.TerminalTab_Factions_Reputation).ToString();
      this.m_textReputation.Text = string.Empty;
      this.m_labelMembers.Text = MyTexts.Get(MySpaceTexts.TerminalTab_Factions_Members).ToString();
      this.m_labelAutoAcceptMember.Text = MyTexts.Get(MySpaceTexts.TerminalTab_Factions_AutoAccept).ToString();
      this.m_labelAutoAcceptPeace.Text = MyTexts.Get(MySpaceTexts.TerminalTab_Factions_AutoAcceptRequest).ToString();
      this.m_labelAutoAcceptMember.SetToolTip(MySpaceTexts.TerminalTab_Factions_AutoAcceptToolTip);
      this.m_labelAutoAcceptPeace.SetToolTip(MySpaceTexts.TerminalTab_Factions_AutoAcceptRequestToolTip);
      this.m_textFactionDesc = (MyGuiControlMultilineText) controlsParent.Controls.GetControlByName("textFactionDesc");
      this.m_textFactionPriv = (MyGuiControlMultilineText) controlsParent.Controls.GetControlByName("textFactionPrivate");
      this.m_factionIcon = (MyGuiControlImage) controlsParent.Controls.GetControlByName("factionIcon");
      this.m_progressReputation = (MyGuiReputationProgressBar) controlsParent.Controls.GetControlByName("progressReputation");
      this.m_tableMembers = (MyGuiControlTable) controlsParent.Controls.GetControlByName("tableMembers");
      this.m_tableMembers.SetColumnComparison(1, (Comparison<MyGuiControlTable.Cell>) ((a, b) => ((int) a.UserData).CompareTo((int) b.UserData)));
      this.m_tableMembers.ItemSelected += new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.OnTableItemSelected);
      this.m_checkAutoAcceptMember = (MyGuiControlCheckbox) controlsParent.Controls.GetControlByName("checkFactionMembersAcceptEveryone");
      this.m_checkAutoAcceptPeace = (MyGuiControlCheckbox) controlsParent.Controls.GetControlByName("checkFactionMembersAcceptPeace");
      this.m_checkAutoAcceptMember.SetToolTip(MySpaceTexts.TerminalTab_Factions_AutoAcceptToolTip);
      this.m_checkAutoAcceptPeace.SetToolTip(MySpaceTexts.TerminalTab_Factions_AutoAcceptRequestToolTip);
      this.m_checkAutoAcceptMember.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.OnAutoAcceptChanged);
      this.m_checkAutoAcceptPeace.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.OnAutoAcceptChanged);
      this.m_buttonEdit = (MyGuiControlButton) controlsParent.Controls.GetControlByName("buttonEdit");
      this.m_buttonPromote = (MyGuiControlButton) controlsParent.Controls.GetControlByName("buttonPromote");
      this.m_buttonKick = (MyGuiControlButton) controlsParent.Controls.GetControlByName("buttonKick");
      this.m_buttonAcceptJoin = (MyGuiControlButton) controlsParent.Controls.GetControlByName("buttonAcceptJoin");
      this.m_buttonDemote = (MyGuiControlButton) controlsParent.Controls.GetControlByName("buttonDemote");
      this.m_buttonShareProgress = (MyGuiControlButton) controlsParent.Controls.GetControlByName("buttonShareProgress");
      this.m_buttonAddNpc = (MyGuiControlButton) controlsParent.Controls.GetControlByName("buttonAddNpc");
      MyGuiControlImage controlByName = (MyGuiControlImage) controlsParent.Controls.GetControlByName("imageCurrency");
      string[] icons = MyBankingSystem.BankingSystemDefinition.Icons;
      string texture = (icons != null ? ((uint) icons.Length > 0U ? 1 : 0) : 0) != 0 ? MyBankingSystem.BankingSystemDefinition.Icons[0] : string.Empty;
      controlByName.SetTexture(texture);
      this.m_buttonEdit.SetToolTip(MySpaceTexts.TerminalTab_Factions_FriendToolTip);
      this.m_buttonPromote.SetToolTip(MySpaceTexts.TerminalTab_Factions_PromoteToolTip);
      this.m_buttonKick.SetToolTip(MySpaceTexts.TerminalTab_Factions_KickToolTip);
      this.m_buttonDemote.SetToolTip(MySpaceTexts.TerminalTab_Factions_DemoteToolTip);
      this.m_buttonAcceptJoin.SetToolTip(MySpaceTexts.TerminalTab_Factions_JoinToolTip);
      this.m_buttonShareProgress.SetToolTip(MySpaceTexts.TerminalTab_Factions_ShareProgressToolTip);
      this.m_buttonAddNpc.SetToolTip(MySpaceTexts.AddNpcToFactionHelp);
      this.m_buttonEdit.TextEnum = MyCommonTexts.Edit;
      this.m_buttonPromote.TextEnum = MyCommonTexts.Promote;
      this.m_buttonKick.TextEnum = MyCommonTexts.Kick;
      this.m_buttonAcceptJoin.TextEnum = MyCommonTexts.Accept;
      this.m_buttonDemote.TextEnum = MyCommonTexts.Demote;
      this.m_buttonShareProgress.TextEnum = MySpaceTexts.ShareProgress;
      this.m_buttonAddNpc.TextEnum = MySpaceTexts.AddNpcToFaction;
      this.m_buttonEdit.ButtonClicked += new Action<MyGuiControlButton>(this.OnEditClicked);
      this.m_buttonPromote.ButtonClicked += new Action<MyGuiControlButton>(this.OnPromotePlayerClicked);
      this.m_buttonKick.ButtonClicked += new Action<MyGuiControlButton>(this.OnKickPlayerClicked);
      this.m_buttonAcceptJoin.ButtonClicked += new Action<MyGuiControlButton>(this.OnAcceptJoinClicked);
      this.m_buttonDemote.ButtonClicked += new Action<MyGuiControlButton>(this.OnDemoteClicked);
      this.m_buttonShareProgress.ButtonClicked += new Action<MyGuiControlButton>(this.OnShareProgressClicked);
      this.m_buttonAddNpc.ButtonClicked += new Action<MyGuiControlButton>(this.OnNewNpcClicked);
      this.m_buttonShareProgress.IsAutoScaleEnabled = true;
      this.m_buttonShareProgress.IsAutoEllipsisEnabled = true;
      this.m_btnWithdraw = (MyGuiControlButton) controlsParent.Controls.GetControlByName("withdrawBtn");
      this.m_btnWithdraw.TextEnum = MySpaceTexts.FactionTerminal_Withdraw_Currency;
      this.m_btnWithdraw.SetToolTip(MySpaceTexts.FactionTerminal_Withdraw_Currency_TTIP);
      this.m_btnWithdraw.ButtonClicked += new Action<MyGuiControlButton>(this.OnWithdrawDepositBntClicked);
      this.m_btnWithdraw.UserData = (object) MyTransactionType.Withdraw;
      this.m_btnDeposit = (MyGuiControlButton) controlsParent.Controls.GetControlByName("depositBtn");
      this.m_btnDeposit.TextEnum = MySpaceTexts.FactionTerminal_Deposit_Currency;
      this.m_btnDeposit.SetToolTip(MySpaceTexts.FactionTerminal_Deposit_Currency_TTIP);
      this.m_btnDeposit.ButtonClicked += new Action<MyGuiControlButton>(this.OnWithdrawDepositBntClicked);
      this.m_btnDeposit.UserData = (object) MyTransactionType.Deposit;
      MySession.Static.Factions.FactionCreated += new Action<long>(this.OnFactionCreated);
      MySession.Static.Factions.FactionEdited += new Action<long>(this.OnFactionEdited);
      MySession.Static.Factions.FactionStateChanged += new Action<MyFactionStateChange, long, long, long, long>(this.OnFactionsStateChanged);
      MySession.Static.Factions.FactionAutoAcceptChanged += new Action<long, bool, bool>(this.OnAutoAcceptChanged);
      MySession.Static.OnUserPromoteLevelChanged += new Action<ulong, MyPromoteLevel>(this.OnUserPromoteLevelChanged);
      MyBankingSystem.Static.OnAccountBalanceChanged += new MyBankingSystem.AccountBalanceChanged(this.OnAccountBalanceChanged);
      this.Refresh();
    }

    public void Close()
    {
      this.UnregisterEvents();
      this.m_selectedFaction = (IMyFaction) null;
      this.m_tableFactions = (MyGuiControlTable) null;
      this.m_buttonCreate = (MyGuiControlButton) null;
      this.m_buttonJoin = (MyGuiControlButton) null;
      this.m_buttonCancelJoin = (MyGuiControlButton) null;
      this.m_buttonLeave = (MyGuiControlButton) null;
      this.m_buttonSendPeace = (MyGuiControlButton) null;
      this.m_buttonCancelPeace = (MyGuiControlButton) null;
      this.m_buttonAcceptPeace = (MyGuiControlButton) null;
      this.m_buttonMakeEnemy = (MyGuiControlButton) null;
      this.m_labelFactionDesc = (MyGuiControlLabel) null;
      this.m_labelReputation = (MyGuiControlLabel) null;
      this.m_textReputation = (MyGuiControlLabel) null;
      this.m_labelFactionPriv = (MyGuiControlLabel) null;
      this.m_labelMembers = (MyGuiControlLabel) null;
      this.m_labelAutoAcceptMember = (MyGuiControlLabel) null;
      this.m_labelAutoAcceptPeace = (MyGuiControlLabel) null;
      this.m_checkAutoAcceptMember = (MyGuiControlCheckbox) null;
      this.m_checkAutoAcceptPeace = (MyGuiControlCheckbox) null;
      this.m_textFactionDesc = (MyGuiControlMultilineText) null;
      this.m_factionIcon = (MyGuiControlImage) null;
      this.m_textFactionPriv = (MyGuiControlMultilineText) null;
      this.m_tableMembers = (MyGuiControlTable) null;
      this.m_buttonKick = (MyGuiControlButton) null;
      this.m_buttonAcceptJoin = (MyGuiControlButton) null;
      this.m_buttonShareProgress = (MyGuiControlButton) null;
      this.m_controlsParent = (IMyGuiControlsParent) null;
    }

    private void UnregisterEvents()
    {
      if (this.m_controlsParent == null)
        return;
      MyBankingSystem.Static.OnAccountBalanceChanged -= new MyBankingSystem.AccountBalanceChanged(this.OnAccountBalanceChanged);
      MySession.Static.OnUserPromoteLevelChanged -= new Action<ulong, MyPromoteLevel>(this.OnUserPromoteLevelChanged);
      MySession.Static.Factions.FactionCreated -= new Action<long>(this.OnFactionCreated);
      MySession.Static.Factions.FactionEdited -= new Action<long>(this.OnFactionEdited);
      MySession.Static.Factions.FactionStateChanged -= new Action<MyFactionStateChange, long, long, long, long>(this.OnFactionsStateChanged);
      MySession.Static.Factions.FactionAutoAcceptChanged -= new Action<long, bool, bool>(this.OnAutoAcceptChanged);
      this.m_tableFactions.ItemSelected -= new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.OnFactionsTableItemSelected);
      this.m_tableFactionsFilter.ItemSelected -= new MyGuiControlCombobox.ItemSelectedDelegate(this.OnFactionFilterItemSelected);
      this.m_tableMembers.ItemSelected -= new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.OnTableItemSelected);
      this.m_checkAutoAcceptMember.IsCheckedChanged -= new Action<MyGuiControlCheckbox>(this.OnAutoAcceptChanged);
      this.m_checkAutoAcceptPeace.IsCheckedChanged -= new Action<MyGuiControlCheckbox>(this.OnAutoAcceptChanged);
      this.m_buttonCreate.ButtonClicked -= new Action<MyGuiControlButton>(this.OnCreateClicked);
      this.m_buttonJoin.ButtonClicked -= new Action<MyGuiControlButton>(this.OnJoinClicked);
      this.m_buttonCancelJoin.ButtonClicked -= new Action<MyGuiControlButton>(this.OnCancelJoinClicked);
      this.m_buttonLeave.ButtonClicked -= new Action<MyGuiControlButton>(this.OnLeaveClicked);
      this.m_buttonSendPeace.ButtonClicked -= new Action<MyGuiControlButton>(this.OnFriendClicked);
      this.m_buttonAcceptPeace.ButtonClicked -= new Action<MyGuiControlButton>(this.OnAcceptFriendClicked);
      this.m_buttonMakeEnemy.ButtonClicked -= new Action<MyGuiControlButton>(this.OnEnemyClicked);
      this.m_buttonEdit.ButtonClicked -= new Action<MyGuiControlButton>(this.OnEditClicked);
      this.m_buttonPromote.ButtonClicked -= new Action<MyGuiControlButton>(this.OnPromotePlayerClicked);
      this.m_buttonKick.ButtonClicked -= new Action<MyGuiControlButton>(this.OnKickPlayerClicked);
      this.m_buttonAcceptJoin.ButtonClicked -= new Action<MyGuiControlButton>(this.OnAcceptJoinClicked);
      this.m_buttonDemote.ButtonClicked -= new Action<MyGuiControlButton>(this.OnDemoteClicked);
      this.m_buttonShareProgress.ButtonClicked -= new Action<MyGuiControlButton>(this.OnShareProgressClicked);
      this.m_buttonAddNpc.ButtonClicked -= new Action<MyGuiControlButton>(this.OnNewNpcClicked);
      this.m_btnWithdraw.ButtonClicked -= new Action<MyGuiControlButton>(this.OnWithdrawDepositBntClicked);
      this.m_btnDeposit.ButtonClicked -= new Action<MyGuiControlButton>(this.OnWithdrawDepositBntClicked);
    }

    private void OnFactionsTableItemSelected(
      MyGuiControlTable sender,
      MyGuiControlTable.EventArgs args)
    {
      if (sender.SelectedRow != null)
      {
        this.m_selectedFaction = (IMyFaction) sender.SelectedRow.UserData;
        bool isFactionDiscovered = this.IsFactionDiscovered(this.m_selectedFaction);
        if (isFactionDiscovered)
        {
          this.m_textFactionDesc.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
          this.m_textFactionDesc.Text = new StringBuilder(this.m_selectedFaction.Description);
          this.m_textFactionPriv.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
          this.m_textFactionPriv.Text = new StringBuilder(this.m_selectedFaction.PrivateInfo);
          MyGuiControlImage.MyDrawTexture backgroundTexture;
          MyGuiControlImage.MyDrawTexture iconFaction;
          this.GetFactionIconTextures(isFactionDiscovered, out backgroundTexture, out iconFaction);
          this.m_factionIcon.SetTextures(new MyGuiControlImage.MyDrawTexture[2]
          {
            backgroundTexture,
            iconFaction
          });
        }
        else
        {
          this.m_textFactionDesc.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
          this.m_textFactionDesc.Text = MyTexts.Get(MySpaceTexts.Terminal_Factions_DataNotAvailable);
          this.m_textFactionDesc.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
          this.m_textFactionPriv.Text = MyTexts.Get(MySpaceTexts.Terminal_Factions_DataNotAvailable);
          this.m_factionIcon.SetTexture();
          this.m_factionIcon.ColorMask = new Vector4(1f, 1f, 1f, 1f);
        }
        this.UpdateAccountControls();
        this.RefreshTableMembers();
      }
      else
        this.m_selectedFaction = (IMyFaction) null;
      this.m_tableMembers.Sort(false);
      this.RefreshJoinButton();
      this.RefreshDiplomacyButtons();
      this.RefreshFactionProperties();
    }

    private void UpdateAccountControls()
    {
      if (this.m_selectedFaction != null && this.m_tableFactions.SelectedRow != null)
      {
        bool flag = this.m_selectedFaction == this.m_userFaction || MySession.Static.CreativeToolsEnabled(MySession.Static.LocalHumanPlayer.Client.SteamUserId);
        MyGuiControlLabel controlByName = (MyGuiControlLabel) this.m_controlsParent.Controls.GetControlByName("labelBalanceValue");
        controlByName.UserData = (object) null;
        controlByName.Text = flag ? MyBankingSystem.Static.GetBalanceShortString(this.m_selectedFaction.FactionId, false) : MyTexts.GetString(MySpaceTexts.Terminal_Factions_DataNotAvailable);
        this.m_controlsParent.Controls.GetControlByName("imageCurrency").Visible = flag;
        if (this.m_tableFactions.SelectedRow != null & flag)
        {
          if (((this.m_userIsLeader ? 1 : (MySession.Static.CreativeToolsEnabled(MySession.Static.LocalHumanPlayer.Client.SteamUserId) ? 1 : 0)) & (flag ? 1 : 0)) != 0)
            this.m_btnWithdraw.Enabled = true;
          else
            this.m_btnWithdraw.Enabled = false;
          this.m_btnDeposit.Enabled = flag;
        }
        else
        {
          this.m_btnWithdraw.Enabled = false;
          this.m_btnDeposit.Enabled = false;
        }
      }
      else
      {
        this.m_btnWithdraw.Enabled = false;
        this.m_btnDeposit.Enabled = false;
      }
    }

    internal void Update()
    {
      if (this.m_selectedFaction == null)
        return;
      MyGuiControlLabel controlByName = (MyGuiControlLabel) this.m_controlsParent.Controls.GetControlByName("labelBalanceValue");
      if (controlByName.UserData == null)
        return;
      MyTerminalFactionController.AccountBalanceAnimationInfo userData = (MyTerminalFactionController.AccountBalanceAnimationInfo) controlByName.UserData;
      long num1;
      long num2 = (long) ((double) (num1 = userData.NewBalance - userData.OldBalance) * 0.200000002980232);
      long valueToFormat = userData.OldBalance + num2;
      if (Math.Abs(num1) < 10L)
      {
        valueToFormat = userData.NewBalance;
        controlByName.UserData = (object) null;
      }
      else
      {
        userData.OldBalance = valueToFormat;
        controlByName.UserData = (object) userData;
      }
      controlByName.Text = MyBankingSystem.GetFormatedValue(valueToFormat);
    }

    private void OnAccountBalanceChanged(MyAccountInfo accOldInfo, MyAccountInfo accInfo)
    {
      if (this.m_selectedFaction == null || this.m_selectedFaction.FactionId != accInfo.OwnerIdentifier)
        return;
      this.m_controlsParent.Controls.GetControlByName("labelBalanceValue").UserData = (object) new MyTerminalFactionController.AccountBalanceAnimationInfo()
      {
        OldBalance = accOldInfo.Balance,
        NewBalance = accInfo.Balance
      };
    }

    private void OnTableItemSelected(MyGuiControlTable sender, MyGuiControlTable.EventArgs args) => this.RefreshRightSideButtons(sender.SelectedRow);

    private void OnFactionFilterItemSelected() => this.RefreshTableFactions((MyTerminalFactionController.MyFactionFilter) this.m_tableFactionsFilter.GetSelectedKey());

    private void OnCreateClicked(MyGuiControlButton sender)
    {
      MyGuiScreenCreateOrEditFaction screen = (MyGuiScreenCreateOrEditFaction) MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.CreateFactionScreen);
      screen.Init(ref this.m_userFaction);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) screen);
    }

    private void OnEditClicked(MyGuiControlButton sender)
    {
      IMyFaction targetFaction = this.TargetFaction;
      if (targetFaction == null)
        return;
      MyGuiScreenCreateOrEditFaction screen = (MyGuiScreenCreateOrEditFaction) MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.CreateFactionScreen);
      screen.Init(ref targetFaction);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) screen);
    }

    private void OnJoinClicked(MyGuiControlButton sender)
    {
      if (MySession.Static.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.PER_FACTION && !MyBlockLimits.IsFactionChangePossible(MySession.Static.LocalPlayerId, this.m_selectedFaction.FactionId))
        this.ShowErrorBox(new StringBuilder(MyTexts.GetString(MySpaceTexts.TerminalTab_Factions_JoinLimitsExceeded)));
      MyFactionCollection.SendJoinRequest(this.m_selectedFaction.FactionId, MySession.Static.LocalPlayerId);
    }

    private void OnCancelJoinClicked(MyGuiControlButton sender) => MyFactionCollection.CancelJoinRequest(this.m_selectedFaction.FactionId, MySession.Static.LocalPlayerId);

    private void OnLeaveClicked(MyGuiControlButton sender)
    {
      if (this.m_selectedFaction.FactionId != this.m_userFaction.FactionId)
        return;
      this.ShowConfirmBox(new StringBuilder().AppendFormat(MyCommonTexts.MessageBoxConfirmFactionsLeave, (object) MyStatControlText.SubstituteTexts(this.m_userFaction.Name)), new Action(this.LeaveFaction));
    }

    private void LeaveFaction()
    {
      if (this.m_userFaction == null)
        return;
      MyFactionCollection.MemberLeaves(this.m_userFaction.FactionId, MySession.Static.LocalPlayerId);
      this.m_userFaction = (IMyFaction) null;
      this.Refresh();
    }

    private void OnFriendClicked(MyGuiControlButton sender) => MyFactionCollection.SendPeaceRequest(this.m_userFaction.FactionId, this.m_selectedFaction.FactionId);

    private void OnAcceptFriendClicked(MyGuiControlButton sender) => MyFactionCollection.AcceptPeace(this.m_userFaction.FactionId, this.m_selectedFaction.FactionId);

    private void OnCancelPeaceRequestClicked(MyGuiControlButton sender) => MyFactionCollection.CancelPeaceRequest(this.m_userFaction.FactionId, this.m_selectedFaction.FactionId);

    private void OnEnemyClicked(MyGuiControlButton sender) => this.ShowConfirmBox(new StringBuilder().AppendFormat(MyCommonTexts.MessageBoxConfirmWarDeclaration, (object) MyStatControlText.SubstituteTexts(this.m_selectedFaction.Name)), (Action) (() => MyFactionCollection.DeclareWar(this.m_userFaction.FactionId, this.m_selectedFaction.FactionId)));

    private void OnAutoAcceptChanged(MyGuiControlCheckbox sender)
    {
      IMyFaction targetFaction = this.TargetFaction;
      if (targetFaction == null)
        return;
      MySession.Static.Factions.ChangeAutoAccept(targetFaction.FactionId, this.m_checkAutoAcceptMember.IsChecked, this.m_checkAutoAcceptPeace.IsChecked);
    }

    private void OnAutoAcceptChanged(long factionId, bool autoAcceptMember, bool autoAcceptPeace) => this.RefreshFactionProperties();

    private void OnPromotePlayerClicked(MyGuiControlButton sender)
    {
      if (this.m_tableMembers.SelectedRow == null)
        return;
      this.ShowConfirmBox(new StringBuilder().AppendFormat(MyCommonTexts.MessageBoxConfirmFactionsPromote, (object) this.m_selectedUserName), new Action(this.PromotePlayer));
    }

    private void PromotePlayer() => MyFactionCollection.PromoteMember(this.TargetFaction.FactionId, this.m_selectedUserId);

    private void OnKickPlayerClicked(MyGuiControlButton sender)
    {
      if (this.m_tableMembers.SelectedRow == null)
        return;
      this.ShowConfirmBox(new StringBuilder().AppendFormat(MyCommonTexts.MessageBoxConfirmFactionsKickPlayer, (object) this.m_selectedUserName), new Action(this.KickPlayer));
    }

    private void KickPlayer()
    {
      if (this.TargetFaction == null)
        return;
      MyFactionCollection.KickMember(this.TargetFaction.FactionId, this.m_selectedUserId);
    }

    private void OnAcceptJoinClicked(MyGuiControlButton sender)
    {
      if (this.m_tableMembers.SelectedRow == null)
        return;
      this.ShowConfirmBox(new StringBuilder().AppendFormat(MyCommonTexts.MessageBoxConfirmFactionsAcceptJoin, (object) this.m_selectedUserName), new Action(this.AcceptJoin));
    }

    private void AcceptJoin() => MyFactionCollection.AcceptJoin(this.TargetFaction.FactionId, this.m_selectedUserId);

    private void OnDemoteClicked(MyGuiControlButton sender)
    {
      if (this.m_tableMembers.SelectedRow == null)
        return;
      this.ShowConfirmBox(new StringBuilder().AppendFormat(MyCommonTexts.MessageBoxConfirmFactionsDemote, (object) this.m_selectedUserName), new Action(this.Demote));
    }

    private void Demote() => MyFactionCollection.DemoteMember(this.TargetFaction.FactionId, this.m_selectedUserId);

    private void OnShareProgressClicked(MyGuiControlButton sender)
    {
      if (this.m_tableMembers.SelectedRow == null)
        return;
      this.ShowConfirmBox(new StringBuilder().AppendFormat(MyCommonTexts.MessageBoxConfirmShareResearch, (object) this.m_selectedUserName), new Action(this.ShareProgress));
    }

    private void ShareProgress() => MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (x => new Action<long>(MySessionComponentResearch.CallShareResearch)), this.m_selectedUserId);

    private void OnNewNpcClicked(MyGuiControlButton sender) => MyMultiplayer.RaiseStaticEvent<long, string>((Func<IMyEventOwner, Action<long, string>>) (x => new Action<long, string>(MyTerminalFactionController.NewNpcClickedInternal)), this.TargetFaction.FactionId, this.TargetFaction.Tag + " NPC" + (object) MyRandom.Instance.Next(1000, 9999));

    [Event(null, 666)]
    [Reliable]
    [Server]
    public static void NewNpcClickedInternal(long factionId, string npcName)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
      {
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        MyIdentity newNpcIdentity = Sync.Players.CreateNewNpcIdentity(npcName);
        MyFactionCollection.SendJoinRequest(factionId, newNpcIdentity.IdentityId);
      }
    }

    private void OnWithdrawDepositBntClicked(MyGuiControlButton obj)
    {
      MyIdentity playerIdentity = MySession.Static.Players.TryGetPlayerIdentity(MySession.Static.LocalHumanPlayer.Id);
      long num1;
      long num2;
      if ((MyTransactionType) obj.UserData == MyTransactionType.Deposit)
      {
        num1 = playerIdentity.IdentityId;
        num2 = this.m_selectedFaction.FactionId;
      }
      else
      {
        num1 = this.m_selectedFaction.FactionId;
        num2 = playerIdentity.IdentityId;
      }
      MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(typeof (MyGuiScreenTransaction), (object) (MyTransactionType) obj.UserData, (object) num1, (object) num2));
    }

    private void Refresh()
    {
      this.RefreshUserInfo();
      this.RefreshCreateButton();
      this.RefreshJoinButton();
      this.RefreshDiplomacyButtons();
      this.RefreshRightSideButtons((MyGuiControlTable.Row) null);
      this.RefreshFactionProperties();
      this.UpdateAccountControls();
    }

    private void RefreshUserInfo()
    {
      this.m_userIsFounder = false;
      this.m_userIsLeader = false;
      this.m_userFaction = MySession.Static.Factions.TryGetPlayerFaction(MySession.Static.LocalPlayerId);
      if (this.m_userFaction == null)
        return;
      this.m_userIsFounder = this.m_userFaction.IsFounder(MySession.Static.LocalPlayerId);
      this.m_userIsLeader = this.m_userFaction.IsLeader(MySession.Static.LocalPlayerId);
    }

    private void RefreshCreateButton()
    {
      if (this.m_buttonCreate == null)
        return;
      if (this.m_userFaction != null)
      {
        this.m_buttonCreate.Enabled = false;
        this.m_buttonCreate.SetToolTip(MySpaceTexts.TerminalTab_Factions_BeforeCreateLeave);
      }
      else if (MySession.Static.MaxFactionsCount == 0 || MySession.Static.MaxFactionsCount > 0 && MySession.Static.Factions.HumansCount() < MySession.Static.MaxFactionsCount)
      {
        this.m_buttonCreate.Enabled = true;
        this.m_buttonCreate.SetToolTip(MySpaceTexts.TerminalTab_Factions_CreateToolTip);
      }
      else
      {
        this.m_buttonCreate.Enabled = false;
        this.m_buttonCreate.SetToolTip(MySpaceTexts.TerminalTab_Factions_MaxCountReachedToolTip);
      }
    }

    private void RefreshJoinButton()
    {
      this.m_buttonLeave.Visible = false;
      this.m_buttonJoin.Visible = false;
      this.m_buttonCancelJoin.Visible = false;
      this.m_buttonLeave.Enabled = false;
      this.m_buttonJoin.Enabled = false;
      this.m_buttonCancelJoin.Enabled = false;
      if (this.m_userFaction != null && MySession.Static.BlockLimitsEnabled != MyBlockLimitsEnabledEnum.PER_FACTION)
      {
        this.m_buttonLeave.Visible = true;
        if (MySession.Static.Settings.EnableTeamBalancing && !MySession.Static.IsUserSpaceMaster(Sync.MyId))
          this.m_buttonLeave.Enabled = false;
        else
          this.m_buttonLeave.Enabled = this.m_tableFactions.SelectedRow != null && this.m_tableFactions.SelectedRow.UserData == this.m_userFaction;
      }
      else if (this.m_tableFactions.SelectedRow != null && this.IsFactionDiscovered(this.m_selectedFaction))
      {
        if (this.m_selectedFaction.JoinRequests.ContainsKey(MySession.Static.LocalPlayerId))
        {
          this.m_buttonCancelJoin.Visible = true;
          this.m_buttonCancelJoin.Enabled = true;
          this.m_buttonJoin.Visible = false;
        }
        else if (MySession.Static.CreativeToolsEnabled(MySession.Static.LocalHumanPlayer.Client.SteamUserId) || this.m_selectedFaction.AcceptHumans && this.m_userFaction != this.m_selectedFaction)
        {
          this.m_buttonJoin.Visible = true;
          this.m_buttonJoin.Enabled = true;
        }
        else
        {
          this.m_buttonJoin.Visible = true;
          this.m_buttonJoin.Enabled = false;
        }
      }
      else
      {
        this.m_buttonJoin.Visible = true;
        this.m_buttonJoin.Enabled = false;
      }
    }

    private void RefreshDiplomacyButtons()
    {
      this.m_buttonSendPeace.Enabled = false;
      this.m_buttonCancelPeace.Enabled = false;
      this.m_buttonAcceptPeace.Enabled = false;
      this.m_buttonMakeEnemy.Enabled = false;
      this.m_buttonCancelPeace.Visible = false;
      this.m_buttonAcceptPeace.Visible = false;
      if (this.m_userIsLeader && this.m_selectedFaction != null && this.m_selectedFaction.FactionId != this.m_userFaction.FactionId)
      {
        if (MySession.Static.Factions.AreFactionsEnemies(this.m_userFaction.FactionId, this.m_selectedFaction.FactionId))
        {
          if (MySession.Static.Factions.IsPeaceRequestStateSent(this.m_userFaction.FactionId, this.m_selectedFaction.FactionId))
          {
            this.m_buttonSendPeace.Visible = false;
            this.m_buttonCancelPeace.Visible = true;
            this.m_buttonCancelPeace.Enabled = true;
          }
          else if (MySession.Static.Factions.IsPeaceRequestStatePending(this.m_userFaction.FactionId, this.m_selectedFaction.FactionId))
          {
            this.m_buttonSendPeace.Visible = false;
            this.m_buttonAcceptPeace.Visible = true;
            this.m_buttonAcceptPeace.Enabled = true;
          }
          else
          {
            this.m_buttonSendPeace.Visible = true;
            this.m_buttonSendPeace.Enabled = true;
          }
        }
        else
          this.m_buttonMakeEnemy.Enabled = true;
      }
      else
        this.m_buttonSendPeace.Visible = true;
    }

    private void RefreshRightSideButtons(MyGuiControlTable.Row selected)
    {
      bool flag = MySession.Static.IsUserAdmin(Sync.MyId);
      this.m_buttonPromote.Enabled = false;
      this.m_buttonKick.Enabled = false;
      this.m_buttonAcceptJoin.Enabled = false;
      this.m_buttonDemote.Enabled = false;
      this.m_buttonShareProgress.Enabled = false;
      if (selected == null || selected.UserData == null)
        return;
      MyFactionMember userData = (MyFactionMember) selected.UserData;
      this.m_selectedUserId = userData.PlayerId;
      MyIdentity identity = Sync.Players.TryGetIdentity(userData.PlayerId);
      this.m_selectedUserName = identity != null ? identity.DisplayName : string.Empty;
      if (!(this.m_selectedUserId != MySession.Static.LocalPlayerId | flag))
        return;
      if (this.m_userIsFounder | flag && this.TargetFaction.IsLeader(this.m_selectedUserId))
      {
        this.m_buttonKick.Enabled = true;
        this.m_buttonDemote.Enabled = true;
      }
      else if (this.m_userIsFounder | flag && this.TargetFaction.IsMember(this.m_selectedUserId))
      {
        this.m_buttonKick.Enabled = true;
        this.m_buttonPromote.Enabled = true;
      }
      else if (this.m_userIsLeader | flag && this.TargetFaction.IsMember(this.m_selectedUserId) && (!this.TargetFaction.IsLeader(this.m_selectedUserId) && !this.TargetFaction.IsFounder(this.m_selectedUserId)))
        this.m_buttonKick.Enabled = true;
      else if (((this.m_userIsLeader ? 1 : (this.m_userIsFounder ? 1 : 0)) | (flag ? 1 : 0)) != 0 && this.TargetFaction.JoinRequests.ContainsKey(this.m_selectedUserId))
        this.m_buttonAcceptJoin.Enabled = true;
      if (this.m_userFaction == null || !this.TargetFaction.IsMember(this.m_selectedUserId))
        return;
      this.m_buttonShareProgress.Enabled = true;
    }

    private void OnUserPromoteLevelChanged(ulong arg1, MyPromoteLevel arg2)
    {
      if ((long) arg1 != (long) Sync.MyId)
        return;
      this.RefreshFactionProperties();
    }

    private void RefreshFactionProperties()
    {
      bool flag = MySession.Static.IsUserAdmin(Sync.MyId);
      this.m_checkAutoAcceptMember.IsCheckedChanged -= new Action<MyGuiControlCheckbox>(this.OnAutoAcceptChanged);
      this.m_checkAutoAcceptPeace.IsCheckedChanged -= new Action<MyGuiControlCheckbox>(this.OnAutoAcceptChanged);
      this.m_checkAutoAcceptMember.Enabled = false;
      this.m_checkAutoAcceptPeace.Enabled = false;
      this.m_buttonEdit.Enabled = false;
      this.m_buttonKick.Enabled = false;
      this.m_buttonPromote.Enabled = false;
      this.m_buttonDemote.Enabled = false;
      this.m_buttonAcceptJoin.Enabled = false;
      this.m_buttonShareProgress.Enabled = false;
      this.m_buttonAddNpc.Enabled = false;
      if (this.m_tableFactions.SelectedRow != null)
      {
        this.m_selectedFaction = (IMyFaction) this.m_tableFactions.SelectedRow.UserData;
        bool isFactionDiscovered = this.IsFactionDiscovered(this.m_selectedFaction);
        this.m_textFactionDesc.Text = isFactionDiscovered ? new StringBuilder(this.m_selectedFaction.Description) : MyTexts.Get(MySpaceTexts.Terminal_Factions_DataNotAvailable);
        this.m_textFactionDesc.TextBoxAlign = isFactionDiscovered ? MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP : MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
        if (isFactionDiscovered)
        {
          MyGuiControlImage.MyDrawTexture backgroundTexture;
          MyGuiControlImage.MyDrawTexture iconFaction;
          this.GetFactionIconTextures(isFactionDiscovered, out backgroundTexture, out iconFaction);
          this.m_factionIcon.SetTextures(new MyGuiControlImage.MyDrawTexture[2]
          {
            backgroundTexture,
            iconFaction
          });
        }
        else
          this.m_factionIcon.SetTexture();
        this.UpdateAccountControls();
        this.m_checkAutoAcceptMember.IsChecked = this.m_selectedFaction.AutoAcceptMember;
        this.m_checkAutoAcceptPeace.IsChecked = this.m_selectedFaction.AutoAcceptPeace;
        Tuple<MyRelationsBetweenFactions, int> playerAndFaction = MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(MySession.Static.LocalPlayerId, this.m_selectedFaction.FactionId);
        int relationValue = playerAndFaction.Item2;
        this.m_progressReputation.SetCurrentValue(relationValue);
        this.m_textReputation.Text = relationValue.ToString();
        MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
        if (component != null)
        {
          if (playerAndFaction.Item1 == MyRelationsBetweenFactions.Friends)
            this.m_progressReputation.SetBonusValues(this.MultiplierToPercentage(component.GetOffersFriendlyBonus(relationValue)), this.MultiplierToPercentage(component.GetOrdersFriendlyBonus(relationValue)), this.BonusToPercentage(component.GetOffersFriendlyBonusMax()), this.BonusToPercentage(component.GetOrdersFriendlyBonusMax()));
          else
            this.m_progressReputation.SetBonusValues(0, 0, this.BonusToPercentage(component.GetOffersFriendlyBonusMax()), this.BonusToPercentage(component.GetOrdersFriendlyBonusMax()));
        }
        if (flag || this.m_userFaction != null && this.m_userFaction.FactionId == this.m_selectedFaction.FactionId)
        {
          this.m_textFactionPriv.Text = isFactionDiscovered ? new StringBuilder(this.m_selectedFaction.PrivateInfo) : MyTexts.Get(MySpaceTexts.Terminal_Factions_DataNotAvailable);
          this.m_textFactionPriv.TextBoxAlign = isFactionDiscovered ? MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP : MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
          if (isFactionDiscovered & (this.m_userIsLeader | flag))
          {
            this.m_checkAutoAcceptMember.Enabled = true;
            this.m_checkAutoAcceptPeace.Enabled = true;
            this.m_buttonEdit.Enabled = true;
          }
          if (isFactionDiscovered && MySession.Static.IsUserSpaceMaster(MySession.Static.LocalHumanPlayer.Client.SteamUserId) | flag)
            this.m_buttonAddNpc.Enabled = true;
        }
        else
          this.m_textFactionPriv.Text = (StringBuilder) null;
      }
      else
        this.m_tableMembers.Clear();
      if (this.m_selectedFaction == null)
      {
        this.m_labelReputation.Visible = false;
        this.m_textReputation.Visible = false;
        this.m_progressReputation.Visible = false;
        this.m_labelFactionPriv.Visible = false;
        this.m_textFactionPriv.Visible = false;
        this.m_factionIcon.SetTexture();
        this.m_btnWithdraw.Enabled = false;
        this.m_btnDeposit.Enabled = false;
        this.HideCurrencyIcon();
        this.AccountBalanceNotApplicable();
      }
      else if (this.m_userFaction != null && this.m_userFaction == this.m_selectedFaction)
      {
        this.m_labelReputation.Visible = false;
        this.m_textReputation.Visible = false;
        this.m_progressReputation.Visible = false;
        this.m_labelFactionPriv.Visible = true;
        this.m_textFactionPriv.Visible = true;
      }
      else
      {
        this.m_labelReputation.Visible = true;
        this.m_textReputation.Visible = true;
        this.m_progressReputation.Visible = true;
        this.m_labelFactionPriv.Visible = false;
        this.m_textFactionPriv.Visible = false;
      }
      this.m_checkAutoAcceptMember.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.OnAutoAcceptChanged);
      this.m_checkAutoAcceptPeace.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.OnAutoAcceptChanged);
    }

    private void HideCurrencyIcon() => this.m_controlsParent.Controls.GetControlByName("imageCurrency").Visible = false;

    private void AccountBalanceNotApplicable()
    {
      MyGuiControlLabel controlByName = (MyGuiControlLabel) this.m_controlsParent.Controls.GetControlByName("labelBalanceValue");
      controlByName.UserData = (object) null;
      controlByName.Text = MyTexts.GetString(MySpaceTexts.Terminal_Factions_DataNotAvailable);
    }

    private int BonusToPercentage(float bonus) => (int) (Math.Round((double) bonus, 2) * 100.0);

    private int MultiplierToPercentage(float bonus) => (int) (Math.Round((double) bonus - 1.0, 2) * 100.0);

    private void GetFactionIconTextures(
      bool isFactionDiscovered,
      out MyGuiControlImage.MyDrawTexture backgroundTexture,
      out MyGuiControlImage.MyDrawTexture iconFaction)
    {
      Vector3 hsv1 = MyColorPickerConstants.HSVOffsetToHSV(this.m_selectedFaction.CustomColor);
      backgroundTexture = new MyGuiControlImage.MyDrawTexture()
      {
        Texture = "Textures\\GUI\\Blank.dds",
        ColorMask = new Vector4?(hsv1.HSVtoColor().ToVector4())
      };
      string str1;
      if (isFactionDiscovered)
      {
        MyStringId? factionIcon = this.m_selectedFaction.FactionIcon;
        if (factionIcon.HasValue)
        {
          factionIcon = this.m_selectedFaction.FactionIcon;
          str1 = factionIcon.Value.ToString();
          goto label_4;
        }
      }
      str1 = (string) null;
label_4:
      string str2 = str1;
      Vector3 hsv2 = MyColorPickerConstants.HSVOffsetToHSV(this.m_selectedFaction.IconColor);
      iconFaction = new MyGuiControlImage.MyDrawTexture()
      {
        Texture = str2,
        ColorMask = new Vector4?(hsv2.HSVtoColor().ToVector4())
      };
    }

    private bool IsFactionDiscovered(IMyFaction faction) => ((MySession.Static.Factions.IsFactionDiscovered(MySession.Static.LocalHumanPlayer.Id, faction.FactionId) || !MySession.Static.Factions.IsNpcFaction(faction.Tag) ? 1 : (MySession.Static.Factions.IsDiscoveredByDefault(faction.Tag) ? 1 : 0)) | (faction == this.m_userFaction ? 1 : 0) | (MySession.Static.CreativeToolsEnabled(MySession.Static.LocalHumanPlayer.Client.SteamUserId) ? 1 : 0)) != 0;

    private void RefreshTableFactions(
      MyTerminalFactionController.MyFactionFilter factionFilter)
    {
      this.m_tableFactions.Clear();
      foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
      {
        MyFaction myFaction = faction.Value;
        bool flag1 = MySession.Static.Factions.IsNpcFaction(myFaction.Tag);
        bool flag2 = this.m_userFaction == null || flag1 ? MySession.Static.Factions.IsFactionWithPlayerEnemy(MySession.Static.LocalPlayerId, myFaction.FactionId) : MySession.Static.Factions.AreFactionsEnemies(this.m_userFaction.FactionId, myFaction.FactionId);
        bool flag3 = this.m_userFaction == null || flag1 ? MySession.Static.Factions.IsFactionWithPlayerFriend(MySession.Static.LocalPlayerId, myFaction.FactionId) : MySession.Static.Factions.AreFactionsFriends(this.m_userFaction.FactionId, myFaction.FactionId);
        bool flag4 = this.IsFactionDiscovered((IMyFaction) myFaction);
        bool flag5 = this.m_userFaction != null && this.m_userFaction.FactionId == myFaction.FactionId;
        if ((factionFilter != MyTerminalFactionController.MyFactionFilter.Enemy || flag2) && (factionFilter != MyTerminalFactionController.MyFactionFilter.Neutral || !(flag2 | flag5)) && ((factionFilter != MyTerminalFactionController.MyFactionFilter.Friend || flag3) && (factionFilter != MyTerminalFactionController.MyFactionFilter.Player || flag5)) && ((factionFilter != MyTerminalFactionController.MyFactionFilter.NPC || flag1) && !(factionFilter == MyTerminalFactionController.MyFactionFilter.Unknown & flag4) && ((factionFilter != MyTerminalFactionController.MyFactionFilter.Discovered || flag4) && !(factionFilter == MyTerminalFactionController.MyFactionFilter.PlayersFactions & flag1))))
        {
          Color? color = new Color?();
          MyGuiHighlightTexture? icon = new MyGuiHighlightTexture?();
          string iconToolTip = (string) null;
          if (this.m_userFaction == null)
          {
            if (myFaction.JoinRequests.ContainsKey(MySession.Static.LocalPlayerId))
            {
              icon = new MyGuiHighlightTexture?(MyGuiConstants.TEXTURE_ICON_SENT_JOIN_REQUEST);
              iconToolTip = MyTexts.GetString(MySpaceTexts.TerminalTab_Factions_SentJoinToolTip);
            }
          }
          else if (MySession.Static.Factions.IsPeaceRequestStateSent(this.m_userFaction.FactionId, myFaction.FactionId))
          {
            icon = new MyGuiHighlightTexture?(MyGuiConstants.TEXTURE_ICON_SENT_WHITE_FLAG);
            iconToolTip = MyTexts.GetString(MySpaceTexts.TerminalTab_Factions_SentPeace);
          }
          else if (MySession.Static.Factions.IsPeaceRequestStatePending(this.m_userFaction.FactionId, myFaction.FactionId))
          {
            icon = new MyGuiHighlightTexture?(MyGuiConstants.TEXTURE_ICON_WHITE_FLAG);
            iconToolTip = MyTexts.GetString(MySpaceTexts.TerminalTab_Factions_PendingPeace);
          }
          if (!flag4 & flag1)
            color = new Color?(Color.Yellow);
          else if (flag5)
            color = new Color?(MyTerminalFactionController.COLOR_CUSTOM_BLUE);
          else if (flag2)
            color = new Color?(MyTerminalFactionController.COLOR_CUSTOM_RED);
          else if (flag3)
            color = new Color?(MyTerminalFactionController.COLOR_CUSTOM_GREEN);
          this.AddFaction((IMyFaction) myFaction, color, icon, iconToolTip, !flag4);
        }
      }
      this.m_tableFactions.Sort(false);
    }

    private void RefreshFilterCombo()
    {
      this.m_tableFactionsFilter.ClearItems();
      this.m_tableFactionsFilter.AddItem(0L, MyTexts.Get(MySpaceTexts.Faction_Filter_None));
      this.m_tableFactionsFilter.AddItem(1L, MyTexts.Get(MySpaceTexts.Faction_Filter_Player));
      this.m_tableFactionsFilter.AddItem(5L, MyTexts.Get(MySpaceTexts.Faction_Filter_NPC));
      this.m_tableFactionsFilter.AddItem(2L, MyTexts.Get(MySpaceTexts.Faction_Filter_Friend));
      this.m_tableFactionsFilter.AddItem(4L, MyTexts.Get(MySpaceTexts.Faction_Filter_Enemy));
      this.m_tableFactionsFilter.AddItem(3L, MyTexts.Get(MySpaceTexts.Faction_Filter_Neutral));
      this.m_tableFactionsFilter.AddItem(6L, MyTexts.Get(MySpaceTexts.Faction_Filter_Unknown));
      this.m_tableFactionsFilter.AddItem(8L, MyTexts.Get(MySpaceTexts.Faction_Filter_Discovered));
      this.m_tableFactionsFilter.AddItem(7L, MyTexts.Get(MySpaceTexts.Faction_Filter_PlayersFactions));
      this.m_tableFactionsFilter.SelectItemByKey(0L);
    }

    private void RefreshTableMembers()
    {
      this.m_tableMembers.Clear();
      if (this.IsFactionDiscovered(this.m_selectedFaction))
      {
        foreach (KeyValuePair<long, MyFactionMember> member in this.m_selectedFaction.Members)
        {
          MyFactionMember myFactionMember = member.Value;
          MyIdentity identity = Sync.Players.TryGetIdentity(myFactionMember.PlayerId);
          if (identity != null)
          {
            MyGuiControlTable.Row row1 = new MyGuiControlTable.Row((object) myFactionMember);
            MyTerminalFactionController.MyMemberComparerEnum memberComparerEnum = MyTerminalFactionController.MyMemberComparerEnum.Member;
            MyStringId id = MyCommonTexts.Member;
            Color? nullable = new Color?();
            if (this.m_selectedFaction.IsFounder(myFactionMember.PlayerId))
            {
              memberComparerEnum = MyTerminalFactionController.MyMemberComparerEnum.Founder;
              id = MyCommonTexts.Founder;
            }
            else if (this.m_selectedFaction.IsLeader(myFactionMember.PlayerId))
            {
              memberComparerEnum = MyTerminalFactionController.MyMemberComparerEnum.Leader;
              id = MyCommonTexts.Leader;
            }
            else if (this.m_selectedFaction.JoinRequests.ContainsKey(myFactionMember.PlayerId))
            {
              nullable = new Color?(MyTerminalFactionController.COLOR_CUSTOM_GREY);
              memberComparerEnum = MyTerminalFactionController.MyMemberComparerEnum.Applicant;
              id = MyCommonTexts.Applicant;
            }
            MyGuiControlTable.Row row2 = row1;
            StringBuilder text1 = new StringBuilder(identity.DisplayName);
            string displayName = identity.DisplayName;
            // ISSUE: variable of a boxed type
            __Boxed<KeyValuePair<long, MyFactionMember>> local1 = (ValueType) member;
            string toolTip1 = displayName;
            Color? textColor1 = nullable;
            MyGuiHighlightTexture? icon1 = new MyGuiHighlightTexture?();
            MyGuiControlTable.Cell cell1 = new MyGuiControlTable.Cell(text1, (object) local1, toolTip1, textColor1, icon1);
            row2.AddCell(cell1);
            MyGuiControlTable.Row row3 = row1;
            StringBuilder text2 = MyTexts.Get(id);
            string str = MyTexts.GetString(id);
            // ISSUE: variable of a boxed type
            __Boxed<MyTerminalFactionController.MyMemberComparerEnum> local2 = (Enum) memberComparerEnum;
            string toolTip2 = str;
            Color? textColor2 = nullable;
            MyGuiHighlightTexture? icon2 = new MyGuiHighlightTexture?();
            MyGuiControlTable.Cell cell2 = new MyGuiControlTable.Cell(text2, (object) local2, toolTip2, textColor2, icon2);
            row3.AddCell(cell2);
            this.m_tableMembers.Add(row1);
          }
        }
        foreach (KeyValuePair<long, MyFactionMember> joinRequest in this.m_selectedFaction.JoinRequests)
        {
          MyFactionMember myFactionMember = joinRequest.Value;
          MyGuiControlTable.Row row1 = new MyGuiControlTable.Row((object) myFactionMember);
          MyIdentity identity = Sync.Players.TryGetIdentity(myFactionMember.PlayerId);
          if (identity != null)
          {
            MyGuiControlTable.Row row2 = row1;
            StringBuilder text = new StringBuilder(identity.DisplayName);
            string displayName = identity.DisplayName;
            // ISSUE: variable of a boxed type
            __Boxed<KeyValuePair<long, MyFactionMember>> local = (ValueType) joinRequest;
            string toolTip = displayName;
            Color? textColor = new Color?(MyTerminalFactionController.COLOR_CUSTOM_GREY);
            MyGuiHighlightTexture? icon = new MyGuiHighlightTexture?();
            MyGuiControlTable.Cell cell = new MyGuiControlTable.Cell(text, (object) local, toolTip, textColor, icon);
            row2.AddCell(cell);
            row1.AddCell(new MyGuiControlTable.Cell(MyTexts.Get(MyCommonTexts.Applicant), (object) MyTerminalFactionController.MyMemberComparerEnum.Applicant, MyTexts.GetString(MyCommonTexts.Applicant), new Color?(MyTerminalFactionController.COLOR_CUSTOM_GREY)));
            this.m_tableMembers.Add(row1);
          }
        }
      }
      else
      {
        MyGuiControlTable.Row row = new MyGuiControlTable.Row();
        MyGuiControlTable.Cell cell = new MyGuiControlTable.Cell(MyTexts.Get(MySpaceTexts.Terminal_Factions_DataNotAvailable), toolTip: MyTexts.GetString(MySpaceTexts.Terminal_Factions_DataNotAvailable), textColor: new Color?(MyTerminalFactionController.COLOR_CUSTOM_GREY));
        row.AddCell(cell);
        this.m_tableMembers.Add(row);
      }
    }

    private void OnFactionCreated(long insertedId)
    {
      IMyFaction factionById = MySession.Static.Factions.TryGetFactionById(insertedId);
      this.AddFaction(factionById, new Color?(factionById.IsMember(MySession.Static.LocalPlayerId) ? MyTerminalFactionController.COLOR_CUSTOM_GREEN : MyTerminalFactionController.COLOR_CUSTOM_RED));
      this.Refresh();
      this.RefreshTableFactions((MyTerminalFactionController.MyFactionFilter) this.m_tableFactionsFilter.GetSelectedKey());
      this.m_tableFactions.Sort(false);
      this.m_tableFactions.SelectedRowIndex = new int?(this.m_tableFactions.FindIndex((Predicate<MyGuiControlTable.Row>) (row => ((MyFaction) row.UserData).FactionId == insertedId)));
      this.OnFactionsTableItemSelected(this.m_tableFactions, new MyGuiControlTable.EventArgs());
    }

    private void OnFactionEdited(long editedId)
    {
      this.RefreshTableFactions((MyTerminalFactionController.MyFactionFilter) this.m_tableFactionsFilter.GetSelectedKey());
      this.m_tableFactions.SelectedRowIndex = new int?(this.m_tableFactions.FindIndex((Predicate<MyGuiControlTable.Row>) (row => ((MyFaction) row.UserData).FactionId == editedId)));
      this.OnFactionsTableItemSelected(this.m_tableFactions, new MyGuiControlTable.EventArgs());
      this.Refresh();
    }

    private void OnFactionsStateChanged(
      MyFactionStateChange action,
      long fromFactionId,
      long toFactionId,
      long playerId,
      long senderId)
    {
      if (MySession.Static == null || MySession.Static.Factions == null || this.m_tableFactions == null)
        return;
      IMyFaction factionById1 = MySession.Static.Factions.TryGetFactionById(fromFactionId);
      bool flag1 = false;
      if (factionById1 != null)
        flag1 = ((flag1 | MySession.Static.Factions.IsFactionDiscovered(MySession.Static.LocalHumanPlayer.Id, factionById1.FactionId) ? 1 : 0) | (!MySession.Static.Factions.IsNpcFaction(factionById1.Tag) ? 1 : (MySession.Static.Factions.IsDiscoveredByDefault(factionById1.Tag) ? 1 : 0))) != 0 | factionById1 == this.m_userFaction;
      IMyFaction factionById2 = MySession.Static.Factions.TryGetFactionById(toFactionId);
      bool flag2 = false;
      if (factionById2 != null)
        flag2 = ((flag2 | MySession.Static.Factions.IsFactionDiscovered(MySession.Static.LocalHumanPlayer.Id, factionById2.FactionId) ? 1 : 0) | (!MySession.Static.Factions.IsNpcFaction(factionById2.Tag) ? 1 : (MySession.Static.Factions.IsDiscoveredByDefault(factionById2.Tag) ? 1 : 0))) != 0 | factionById2 == this.m_userFaction;
      switch (action)
      {
        case MyFactionStateChange.RemoveFaction:
          this.RemoveFaction(toFactionId);
          break;
        case MyFactionStateChange.SendPeaceRequest:
          if (this.m_userFaction == null)
            return;
          if (this.m_userFaction.FactionId == fromFactionId)
          {
            this.RemoveFaction(toFactionId);
            this.AddFaction(factionById2, new Color?(MyTerminalFactionController.COLOR_CUSTOM_RED), new MyGuiHighlightTexture?(MyGuiConstants.TEXTURE_ICON_SENT_WHITE_FLAG), MyTexts.GetString(MySpaceTexts.TerminalTab_Factions_SentPeace), !flag2);
            break;
          }
          if (this.m_userFaction.FactionId == toFactionId)
          {
            this.RemoveFaction(fromFactionId);
            this.AddFaction(factionById1, new Color?(MyTerminalFactionController.COLOR_CUSTOM_RED), new MyGuiHighlightTexture?(MyGuiConstants.TEXTURE_ICON_WHITE_FLAG), MyTexts.GetString(MySpaceTexts.TerminalTab_Factions_PendingPeace), !flag1);
            break;
          }
          break;
        case MyFactionStateChange.CancelPeaceRequest:
        case MyFactionStateChange.DeclareWar:
          if (this.m_userFaction == null)
            return;
          if (this.m_userFaction.FactionId == fromFactionId)
          {
            this.RemoveFaction(toFactionId);
            this.AddFaction(factionById2, new Color?(MyTerminalFactionController.COLOR_CUSTOM_RED), setAnonymous: (!flag2));
            break;
          }
          if (this.m_userFaction.FactionId == toFactionId)
          {
            this.RemoveFaction(fromFactionId);
            this.AddFaction(factionById1, new Color?(MyTerminalFactionController.COLOR_CUSTOM_RED), setAnonymous: (!flag1));
            break;
          }
          break;
        case MyFactionStateChange.AcceptPeace:
        case MyFactionStateChange.CancelFriendRequest:
          if (this.m_userFaction == null)
            return;
          if (this.m_userFaction.FactionId == fromFactionId)
          {
            this.RemoveFaction(toFactionId);
            this.AddFaction(factionById2, setAnonymous: (!flag2));
            break;
          }
          if (this.m_userFaction.FactionId == toFactionId)
          {
            this.RemoveFaction(fromFactionId);
            this.AddFaction(factionById1, setAnonymous: (!flag1));
            break;
          }
          break;
        case MyFactionStateChange.SendFriendRequest:
          if (this.m_userFaction == null)
            return;
          if (this.m_userFaction.FactionId == fromFactionId)
          {
            this.RemoveFaction(toFactionId);
            this.AddFaction(factionById2, icon: new MyGuiHighlightTexture?(MyGuiConstants.TEXTURE_ICON_SENT_WHITE_FLAG), iconToolTip: MyTexts.GetString(MySpaceTexts.TerminalTab_Factions_SentPeace), setAnonymous: (!flag2));
            break;
          }
          if (this.m_userFaction.FactionId == toFactionId)
          {
            this.RemoveFaction(fromFactionId);
            this.AddFaction(factionById1, icon: new MyGuiHighlightTexture?(MyGuiConstants.TEXTURE_ICON_WHITE_FLAG), iconToolTip: MyTexts.GetString(MySpaceTexts.TerminalTab_Factions_PendingPeace), setAnonymous: (!flag1));
            break;
          }
          break;
        case MyFactionStateChange.AcceptFriendRequest:
          if (this.m_userFaction == null)
            return;
          if (this.m_userFaction.FactionId == fromFactionId)
          {
            this.RemoveFaction(toFactionId);
            this.AddFaction(factionById2, new Color?(MyTerminalFactionController.COLOR_CUSTOM_GREEN), setAnonymous: (!flag2));
            break;
          }
          if (this.m_userFaction.FactionId == toFactionId)
          {
            this.RemoveFaction(fromFactionId);
            this.AddFaction(factionById1, new Color?(MyTerminalFactionController.COLOR_CUSTOM_GREEN), setAnonymous: (!flag1));
            break;
          }
          break;
        default:
          this.OnMemberStateChanged(action, factionById1, playerId);
          break;
      }
      this.m_tableFactions.Sort(false);
      this.m_tableFactions.SelectedRowIndex = new int?(this.m_tableFactions.FindIndex((Predicate<MyGuiControlTable.Row>) (row => ((MyFaction) row.UserData).FactionId == toFactionId)));
      this.OnFactionsTableItemSelected(this.m_tableFactions, new MyGuiControlTable.EventArgs());
      this.Refresh();
    }

    private void RemoveFaction(long factionId)
    {
      if (this.m_tableFactions == null)
        return;
      this.m_tableFactions.Remove((Predicate<MyGuiControlTable.Row>) (row => ((MyFaction) row.UserData).FactionId == factionId));
    }

    private void AddFaction(
      IMyFaction faction,
      Color? color = null,
      MyGuiHighlightTexture? icon = null,
      string iconToolTip = null,
      bool setAnonymous = false)
    {
      if (this.m_tableFactions == null)
        return;
      MyGuiControlTable.Row row = new MyGuiControlTable.Row((object) faction);
      StringBuilder stringBuilder1 = setAnonymous ? MyTexts.Get(MySpaceTexts.Terminal_Factions_Unknown_Tag) : new StringBuilder(faction.Tag);
      StringBuilder text1 = setAnonymous ? MyTexts.Get(MySpaceTexts.Terminal_Factions_Unknown_Label) : new StringBuilder(MyStatControlText.SubstituteTexts(faction.Name));
      StringBuilder text2 = stringBuilder1;
      StringBuilder stringBuilder2 = stringBuilder1;
      Color? nullable1 = color;
      string toolTip1 = stringBuilder1.ToString();
      Color? textColor1 = nullable1;
      MyGuiHighlightTexture? icon1 = new MyGuiHighlightTexture?();
      MyGuiControlTable.Cell cell1 = new MyGuiControlTable.Cell(text2, (object) stringBuilder2, toolTip1, textColor1, icon1);
      row.AddCell(cell1);
      string toolTip2 = string.Empty;
      if (setAnonymous)
        toolTip2 = MyTexts.GetString(MySpaceTexts.Terminal_Factions_Unknown_Label_TTIP);
      else if (MySession.Static.Factions.IsNpcFaction(faction.Tag))
      {
        MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
        if (component != null)
          toolTip2 = component.GetFactionFriendTooltip(faction.FactionId);
      }
      else
        toolTip2 = MyStatControlText.SubstituteTexts(faction.Name);
      Color? textColor2 = color;
      MyGuiControlTable.Cell cell2 = new MyGuiControlTable.Cell(text1, (object) text1, toolTip2, textColor2);
      row.AddCell(cell2);
      StringBuilder text3 = new StringBuilder();
      MyGuiHighlightTexture? nullable2 = icon;
      string toolTip3 = iconToolTip;
      Color? textColor3 = new Color?();
      MyGuiHighlightTexture? icon2 = nullable2;
      MyGuiControlTable.Cell cell3 = new MyGuiControlTable.Cell(text3, toolTip: toolTip3, textColor: textColor3, icon: icon2);
      row.AddCell(cell3);
      this.m_tableFactions.Add(row);
    }

    private void OnMemberStateChanged(
      MyFactionStateChange action,
      IMyFaction fromFaction,
      long playerId)
    {
      MyIdentity identity = Sync.Players.TryGetIdentity(playerId);
      if (identity == null)
      {
        MyLog.Default.WriteLine("ERROR: Faction " + MyStatControlText.SubstituteTexts(fromFaction.Name) + " member " + (object) playerId + " does not exists! ");
      }
      else
      {
        this.RemoveMember(playerId);
        switch (action)
        {
          case MyFactionStateChange.FactionMemberSendJoin:
            this.AddMember(playerId, identity.DisplayName, false, MyTerminalFactionController.MyMemberComparerEnum.Applicant, MyCommonTexts.Applicant, new Color?(MyTerminalFactionController.COLOR_CUSTOM_GREY));
            break;
          case MyFactionStateChange.FactionMemberAcceptJoin:
          case MyFactionStateChange.FactionMemberDemote:
            this.AddMember(playerId, identity.DisplayName, false, MyTerminalFactionController.MyMemberComparerEnum.Member, MyCommonTexts.Member);
            break;
          case MyFactionStateChange.FactionMemberPromote:
            this.AddMember(playerId, identity.DisplayName, true, MyTerminalFactionController.MyMemberComparerEnum.Leader, MyCommonTexts.Leader);
            break;
        }
        this.RefreshUserInfo();
        this.RefreshTableFactions((MyTerminalFactionController.MyFactionFilter) this.m_tableFactionsFilter.GetSelectedKey());
        this.m_tableMembers.Sort(false);
      }
    }

    private void RemoveMember(long playerId) => this.m_tableMembers.Remove((Predicate<MyGuiControlTable.Row>) (row =>
    {
      if (row == null)
        return false;
      MyFactionMember? userData = row.UserData as MyFactionMember?;
      return userData.HasValue && userData.Value.PlayerId == playerId;
    }));

    private void AddMember(
      long playerId,
      string playerName,
      bool isLeader,
      MyTerminalFactionController.MyMemberComparerEnum status,
      MyStringId textEnum,
      Color? color = null)
    {
      MyGuiControlTable.Row row1 = new MyGuiControlTable.Row((object) new MyFactionMember(playerId, isLeader));
      MyGuiControlTable.Row row2 = row1;
      StringBuilder text1 = new StringBuilder(playerName);
      string str1 = playerName;
      // ISSUE: variable of a boxed type
      __Boxed<long> local1 = (ValueType) playerId;
      string toolTip1 = str1;
      Color? textColor1 = color;
      MyGuiHighlightTexture? icon1 = new MyGuiHighlightTexture?();
      MyGuiControlTable.Cell cell1 = new MyGuiControlTable.Cell(text1, (object) local1, toolTip1, textColor1, icon1);
      row2.AddCell(cell1);
      MyGuiControlTable.Row row3 = row1;
      StringBuilder text2 = MyTexts.Get(textEnum);
      string str2 = MyTexts.GetString(textEnum);
      // ISSUE: variable of a boxed type
      __Boxed<MyTerminalFactionController.MyMemberComparerEnum> local2 = (Enum) status;
      string toolTip2 = str2;
      Color? textColor2 = color;
      MyGuiHighlightTexture? icon2 = new MyGuiHighlightTexture?();
      MyGuiControlTable.Cell cell2 = new MyGuiControlTable.Cell(text2, (object) local2, toolTip2, textColor2, icon2);
      row3.AddCell(cell2);
      this.m_tableMembers.Add(row1);
    }

    private void ShowErrorBox(StringBuilder text)
    {
      StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
      MyGuiScreenMessageBox messageBox = MyGuiSandbox.CreateMessageBox(messageText: text, messageCaption: messageCaption);
      messageBox.SkipTransition = true;
      messageBox.CloseBeforeCallback = true;
      messageBox.CanHideOthers = false;
      MyGuiSandbox.AddScreen((MyGuiScreenBase) messageBox);
    }

    private void ShowConfirmBox(StringBuilder text, Action callback)
    {
      StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm);
      MyGuiScreenMessageBox messageBox = MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: text, messageCaption: messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (retval =>
      {
        if (retval != MyGuiScreenMessageBox.ResultEnum.YES)
          return;
        callback();
      })), focusedResult: MyGuiScreenMessageBox.ResultEnum.NO);
      messageBox.SkipTransition = true;
      messageBox.CloseBeforeCallback = true;
      messageBox.CanHideOthers = false;
      MyGuiSandbox.AddScreen((MyGuiScreenBase) messageBox);
    }

    internal enum MyMemberComparerEnum
    {
      Founder,
      Leader,
      Member,
      Applicant,
    }

    internal enum MyFactionFilter : byte
    {
      None,
      Player,
      Friend,
      Neutral,
      Enemy,
      NPC,
      Unknown,
      PlayersFactions,
      Discovered,
    }

    private struct AccountBalanceAnimationInfo
    {
      internal long OldBalance;
      internal long NewBalance;
    }

    protected sealed class NewNpcClickedInternal\u003C\u003ESystem_Int64\u0023System_String : ICallSite<IMyEventOwner, long, string, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long factionId,
        in string npcName,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyTerminalFactionController.NewNpcClickedInternal(factionId, npcName);
      }
    }
  }
}
