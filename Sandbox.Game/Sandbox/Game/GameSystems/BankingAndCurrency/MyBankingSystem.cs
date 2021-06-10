// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.BankingAndCurrency.MyBankingSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Components.Session;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components.BankingAndCurrency;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Game.GameSystems.BankingAndCurrency
{
  [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 1000, typeof (MyObjectBuilder_BankingSystem), null, false)]
  [StaticEventOwner]
  public class MyBankingSystem : MySessionComponentBase
  {
    private const long ACHIEVEMENT_CURRENCY_THRESHOLD_MILIONAIRE = 1000000;
    private const string ACHIEVEMENT_KEY_MILIONAIRE = "MillionaireClub";
    private Dictionary<long, MyAccount> m_accounts = new Dictionary<long, MyAccount>();

    public event MyBankingSystem.AccountBalanceChanged OnAccountBalanceChanged;

    public static MyBankingSystem Static { get; private set; }

    public static MyBankingSystemDefinition BankingSystemDefinition { get; private set; }

    public long OverallBalance { get; private set; }

    public MyBankingSystem() => MyBankingSystem.Static = this;

    public override void InitFromDefinition(MySessionComponentDefinition definition)
    {
      base.InitFromDefinition(definition);
      MyBankingSystem.BankingSystemDefinition = definition as MyBankingSystemDefinition;
    }

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      base.Init(sessionComponent);
      MyObjectBuilder_BankingSystem builderBankingSystem = sessionComponent as MyObjectBuilder_BankingSystem;
      if (builderBankingSystem.Accounts == null)
        return;
      foreach (MyObjectBuilder_BankingSystem.MyObjectBuilder_AccountEntry account in builderBankingSystem.Accounts)
      {
        MyAccount myAccount = new MyAccount();
        myAccount.Init(account.Account);
        this.m_accounts.Add(account.OwnerIdentifier, myAccount);
      }
      this.OverallBalance = builderBankingSystem.OverallBalance;
    }

    public override MyObjectBuilder_SessionComponent GetObjectBuilder()
    {
      MyObjectBuilder_BankingSystem objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_BankingSystem;
      if (this.m_accounts.Count > 0)
      {
        objectBuilder.Accounts = new List<MyObjectBuilder_BankingSystem.MyObjectBuilder_AccountEntry>(this.m_accounts.Count);
        foreach (KeyValuePair<long, MyAccount> account in this.m_accounts)
          objectBuilder.Accounts.Add(new MyObjectBuilder_BankingSystem.MyObjectBuilder_AccountEntry()
          {
            OwnerIdentifier = account.Key,
            Account = account.Value.GetObjectBuilder()
          });
        objectBuilder.OverallBalance = this.OverallBalance;
      }
      return (MyObjectBuilder_SessionComponent) objectBuilder;
    }

    public void CreateAccount(long ownerIdentifier) => this.CreateAccount(ownerIdentifier, MyBankingSystem.BankingSystemDefinition.StartingBalance);

    public void CreateAccount(long ownerIdentifier, long startingBalance)
    {
      if (!Sync.IsServer || this.m_accounts.ContainsKey(ownerIdentifier))
        return;
      MyAccount myAccount = new MyAccount(ownerIdentifier, startingBalance);
      this.m_accounts.Add(ownerIdentifier, myAccount);
      this.OverallBalance += startingBalance;
      MyMultiplayer.RaiseStaticEvent<long, long>((Func<IMyEventOwner, Action<long, long>>) (x => new Action<long, long>(MyBankingSystem.CreateAccount_Clients)), ownerIdentifier, startingBalance);
    }

    [Event(null, 133)]
    [Reliable]
    [Broadcast]
    public static void CreateAccount_Clients(long ownerIdentifier, long startingBalance)
    {
      if (MyBankingSystem.Static.m_accounts.ContainsKey(ownerIdentifier))
        return;
      MyAccount myAccount = new MyAccount(ownerIdentifier, startingBalance);
      MyBankingSystem.Static.m_accounts.Add(ownerIdentifier, myAccount);
    }

    public bool RemoveAccount(long ownerIdentifier)
    {
      if (!Sync.IsServer)
        return false;
      int num = this.m_accounts.ContainsKey(ownerIdentifier) ? (this.m_accounts.Remove(ownerIdentifier) ? 1 : 0) : 0;
      if (num == 0)
        return num != 0;
      MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (x => new Action<long>(MyBankingSystem.RemoveAccount_Clients)), ownerIdentifier);
      return num != 0;
    }

    [Event(null, 169)]
    [Reliable]
    [Broadcast]
    public static void RemoveAccount_Clients(long ownerIdentifier) => MyBankingSystem.Static.m_accounts.Remove(ownerIdentifier);

    public static long GetBalance(long identifierId)
    {
      MyAccountInfo account;
      return !Sync.IsServer || !MyBankingSystem.Static.TryGetAccountInfo(identifierId, out account) ? -1L : account.Balance;
    }

    public static bool ChangeBalance(long identifierId, long amount)
    {
      MyAccount ToAccount;
      if (!Sync.IsServer || !MyBankingSystem.Static.ChangeBalanceInternal(identifierId, amount, out ToAccount))
        return false;
      MyLog.Default.WriteLine(string.Format("Balance change of {0} to account owner {1} with new balance of {2}", (object) amount, (object) ToAccount.OwnerIdentifier, (object) ToAccount.Balance));
      MyMultiplayer.RaiseStaticEvent<long, long, long>((Func<IMyEventOwner, Action<long, long, long>>) (x => new Action<long, long, long>(MyBankingSystem.ChangeBalanceBroadcastToClients)), identifierId, amount, ToAccount.Balance);
      return true;
    }

    private bool ChangeBalanceInternal(long identifierId, long amount, out MyAccount ToAccount)
    {
      ToAccount = (MyAccount) null;
      MyAccount account;
      if (!this.m_accounts.TryGetValue(identifierId, out account))
      {
        MyLog.Default.Error(string.Format("Target Identifier {0} does not contain account.", (object) identifierId));
        return false;
      }
      if (amount < 0L && account + amount < 0L)
      {
        MyLog.Default.Error(string.Format("Identifier {0} does contain enough currency to do the subtraction.", (object) identifierId));
        return false;
      }
      MyAccountInfo accountInfo = account.GetAccountInfo();
      if (accountInfo.Balance + amount < 0L)
        amount = 0L;
      if (amount >= 0L)
      {
        account.Add(amount);
        if (MySession.Static.IsServer && MySession.Static.Players.TryGetIdentity(account.OwnerIdentifier) != null)
          this.CheckBalanceIncreaseAchievements(account, amount);
      }
      else
        account.Subtract(-amount);
      ToAccount = account;
      this.OverallBalance += amount;
      MyBankingSystem.AccountBalanceChanged accountBalanceChanged = this.OnAccountBalanceChanged;
      if (accountBalanceChanged != null)
        accountBalanceChanged(accountInfo, account.GetAccountInfo());
      return true;
    }

    private void CheckBalanceIncreaseAchievements(MyAccount account, long change)
    {
      if (account.Balance < 1000000L || account.Balance - change >= 1000000L)
        return;
      ulong steamId = MySession.Static.Players.TryGetSteamId(account.OwnerIdentifier);
      if (steamId == 0UL)
        return;
      if (MySession.Static.LocalPlayerId == account.OwnerIdentifier)
        MyBankingSystem.UnlockAchievement_Internal("MillionaireClub");
      else
        MyMultiplayer.RaiseStaticEvent<string>((Func<IMyEventOwner, Action<string>>) (x => new Action<string>(MyBankingSystem.UnlockAchievementForClient)), "MillionaireClub", new EndpointId(steamId));
    }

    [Event(null, 268)]
    [Reliable]
    [Client]
    private static void UnlockAchievementForClient(string achievement) => MyBankingSystem.UnlockAchievement_Internal(achievement);

    private static void UnlockAchievement_Internal(string achievement) => MyGameService.GetAchievement(achievement, (string) null, 0.0f).Unlock();

    [Event(null, 279)]
    [Reliable]
    [Broadcast]
    public static void ChangeBalanceBroadcastToClients(
      long identifierId,
      long amount,
      long finalToBalance)
    {
      MyAccount ToAccount;
      MyBankingSystem.Static.ChangeBalanceInternal(identifierId, amount, out ToAccount);
      if (finalToBalance == ToAccount.Balance)
        return;
      MyLog.Default.Error("Server and client data do not match. Reseting client data");
      ToAccount.ResetBalance(finalToBalance);
    }

    public static void RequestTransfer(long fromIdentifier, long toIdentifier, long amount) => MyMultiplayer.RaiseStaticEvent<long, long, long>((Func<IMyEventOwner, Action<long, long, long>>) (x => new Action<long, long, long>(MyBankingSystem.RequestTransfer_Server)), fromIdentifier, toIdentifier, amount);

    [Event(null, 302)]
    [Reliable]
    [Server]
    private static void RequestTransfer_Server(long fromIdentifier, long toIdentifier, long amount)
    {
      MyAccount fromAccount;
      MyAccount ToAccount;
      if (!MyBankingSystem.Static.Transfer_Internal(fromIdentifier, toIdentifier, amount, true, out fromAccount, out ToAccount))
        return;
      MyLog.Default.WriteLine(string.Format("Transfer of {0} from {1} with new balance of {2} to {3} with new balance of {4}", (object) amount, (object) fromIdentifier, (object) fromAccount.Balance, (object) toIdentifier, (object) ToAccount.Balance));
      MyMultiplayer.RaiseStaticEvent<long, long, long, long, long>((Func<IMyEventOwner, Action<long, long, long, long, long>>) (x => new Action<long, long, long, long, long>(MyBankingSystem.RequestTransfer_BroadcastToClients)), fromIdentifier, toIdentifier, amount, fromAccount.Balance, ToAccount.Balance);
    }

    internal void Transfer_Server(long fromIdentifier, long toIdentifier, long amount)
    {
      MyAccount fromAccount;
      MyAccount ToAccount;
      if (!Sync.IsServer || !MyBankingSystem.Static.Transfer_Internal(fromIdentifier, toIdentifier, amount, false, out fromAccount, out ToAccount))
        return;
      MyLog.Default.WriteLine(string.Format("Transfer of {0} from {1} with new balance of {2} to {3} with new balance of {4}", (object) amount, (object) fromIdentifier, (object) fromAccount.Balance, (object) toIdentifier, (object) ToAccount.Balance));
      MyMultiplayer.RaiseStaticEvent<long, long, long, long, long>((Func<IMyEventOwner, Action<long, long, long, long, long>>) (x => new Action<long, long, long, long, long>(MyBankingSystem.RequestTransfer_BroadcastToClients)), fromIdentifier, toIdentifier, amount, fromAccount.Balance, ToAccount.Balance);
    }

    [Event(null, 350)]
    [Reliable]
    [Broadcast]
    public static void RequestTransfer_BroadcastToClients(
      long fromIdentifier,
      long toIdentifier,
      long amount,
      long finalFromBalance,
      long finalToBalance)
    {
      MyAccount fromAccount;
      MyAccount ToAccount;
      MyBankingSystem.Static.Transfer_Internal(fromIdentifier, toIdentifier, amount, false, out fromAccount, out ToAccount);
      if (finalFromBalance == fromAccount.Balance && finalToBalance == ToAccount.Balance)
        return;
      MyLog.Default.Error("Server and client data do not match. Reseting client data");
      fromAccount.ResetBalance(finalFromBalance);
      ToAccount.ResetBalance(finalToBalance);
    }

    private bool Transfer_Internal(
      long fromIdentifier,
      long toIdentifier,
      long amount,
      bool validate,
      out MyAccount fromAccount,
      out MyAccount ToAccount)
    {
      fromAccount = ToAccount = (MyAccount) null;
      if (validate)
      {
        bool flag1 = Sync.Players.TryGetIdentity(fromIdentifier) != null;
        if (flag1 && !this.CheckIsOnline(fromIdentifier))
          return false;
        bool flag2 = Sync.Players.TryGetIdentity(toIdentifier) != null;
        if (flag2 && !this.CheckIsOnline(toIdentifier))
          return false;
        if (flag2 && !flag1)
        {
          if (!this.IsFactionValid(toIdentifier, fromIdentifier))
            return false;
        }
        else if (flag1)
        {
          MyPlayer.PlayerId result;
          Sync.Players.TryGetPlayerId(fromIdentifier, out result);
          if ((long) result.SteamId != (long) MyEventContext.Current.Sender.Value)
          {
            MyLog.Default.Error("Transfer from player that is not the sender of the message is not allowed!");
            return false;
          }
        }
      }
      MyAccount myAccount;
      if (!MyBankingSystem.Static.m_accounts.TryGetValue(fromIdentifier, out myAccount))
      {
        MyLog.Default.Error(string.Format("Source Identifier {0} does not contain account.", (object) fromIdentifier));
        return false;
      }
      MyAccount account;
      if (!MyBankingSystem.Static.m_accounts.TryGetValue(toIdentifier, out account))
      {
        MyLog.Default.Error(string.Format("Target Identifier {0} does not contain account.", (object) toIdentifier));
        return false;
      }
      if (myAccount - amount < 0L)
      {
        MyLog.Default.Error(string.Format("Identifier {0} does contain enough currency to do the transfer.", (object) fromIdentifier));
        return false;
      }
      MyAccountInfo accountInfo1 = myAccount.GetAccountInfo();
      MyAccountInfo accountInfo2 = account.GetAccountInfo();
      myAccount.Subtract(amount);
      MyAccountLogEntry myAccountLogEntry1 = new MyAccountLogEntry()
      {
        ChangeIdentifier = toIdentifier,
        Amount = -amount,
        DateTime = DateTime.Now
      };
      if ((long) myAccount.Log.Count > (long) MyBankingSystem.BankingSystemDefinition.AccountLogLen)
        myAccount.Log.RemoveAt(0);
      myAccount.Log.Add(myAccountLogEntry1);
      account.Add(amount);
      MyAccountLogEntry myAccountLogEntry2 = new MyAccountLogEntry()
      {
        ChangeIdentifier = fromIdentifier,
        Amount = amount,
        DateTime = DateTime.Now
      };
      if ((long) account.Log.Count > (long) MyBankingSystem.BankingSystemDefinition.AccountLogLen)
        account.Log.RemoveAt(0);
      account.Log.Add(myAccountLogEntry2);
      if (MySession.Static.IsServer && MySession.Static.Players.TryGetIdentity(account.OwnerIdentifier) != null)
        this.CheckBalanceIncreaseAchievements(account, amount);
      fromAccount = myAccount;
      ToAccount = account;
      MyBankingSystem.AccountBalanceChanged accountBalanceChanged1 = this.OnAccountBalanceChanged;
      if (accountBalanceChanged1 != null)
        accountBalanceChanged1(accountInfo1, myAccount.GetAccountInfo());
      MyBankingSystem.AccountBalanceChanged accountBalanceChanged2 = this.OnAccountBalanceChanged;
      if (accountBalanceChanged2 != null)
        accountBalanceChanged2(accountInfo2, account.GetAccountInfo());
      return true;
    }

    internal void GetPerPlayerBalances(ref Dictionary<long, long> result)
    {
      if (result == null)
        result = new Dictionary<long, long>();
      if (MySession.Static.Players == null)
        return;
      result.Clear();
      foreach (MyIdentity allIdentity in (IEnumerable<MyIdentity>) MySession.Static.Players.GetAllIdentities())
      {
        MyAccountInfo account;
        if (this.TryGetAccountInfo(allIdentity.IdentityId, out account))
          result.Add(allIdentity.IdentityId, account.Balance);
      }
    }

    internal void GetPerFactionBalances(ref Dictionary<long, long> result)
    {
      if (result == null)
        result = new Dictionary<long, long>();
      if (MySession.Static.Factions == null)
        return;
      result.Clear();
      foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
      {
        MyAccountInfo account;
        if (this.TryGetAccountInfo(faction.Value.FactionId, out account))
          result.Add(faction.Value.FactionId, account.Balance);
      }
    }

    private bool IsFactionValid(long playerIdentity, long factionId)
    {
      IMyFaction factionById = MySession.Static.Factions.TryGetFactionById(factionId);
      if (factionById == null)
      {
        MyLog.Default.Error(string.Format("Faction {0} does not exist. Transfer impossible.", (object) factionId));
        return false;
      }
      if (factionById.IsFounder(playerIdentity) || factionById.IsLeader(playerIdentity))
        return true;
      MyLog.Default.Error(string.Format("Player of identity {0} does not have rights to transfer from Faction {1}. Transfer impossible.", (object) playerIdentity, (object) factionById.Name));
      return false;
    }

    private bool CheckIsOnline(long identifier)
    {
      bool flag = false;
      MyPlayer.PlayerId result;
      if (Sync.Players.TryGetPlayerId(identifier, out result))
        flag = Sync.Players.IsPlayerOnline(ref result);
      if (flag)
        return true;
      MyLog.Default.Error(string.Format("Identity {0} does not have online player. Transfer not possible", (object) identifier));
      return false;
    }

    public bool TryGetAccountInfo(long ownerIdentifier, out MyAccountInfo account)
    {
      MyAccount myAccount;
      if (!this.m_accounts.TryGetValue(ownerIdentifier, out myAccount))
      {
        account = new MyAccountInfo();
        return false;
      }
      account = myAccount.GetAccountInfo();
      return true;
    }

    public string GetBalanceShortString(long ownerIdentidier, bool addCurrencyShortName = true)
    {
      string empty = string.Empty;
      MyAccount myAccount;
      return !this.m_accounts.TryGetValue(ownerIdentidier, out myAccount) ? "Error, Account Not Found" : MyBankingSystem.GetFormatedValue(myAccount.Balance, addCurrencyShortName);
    }

    public static string GetFormatedValue(long valueToFormat, bool addCurrencyShortName = false)
    {
      if (valueToFormat > 1000000000000L || valueToFormat < -1000000000000L)
      {
        valueToFormat /= 1000000000000L;
        return addCurrencyShortName ? valueToFormat.ToString("N0") + " T " + MyBankingSystem.BankingSystemDefinition.CurrencyShortName.ToString() : valueToFormat.ToString("N0") + " T";
      }
      return addCurrencyShortName ? valueToFormat.ToString("N0") + " " + MyBankingSystem.BankingSystemDefinition.CurrencyShortName.ToString() : valueToFormat.ToString("N0");
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      MyBankingSystem.Static = (MyBankingSystem) null;
    }

    public delegate void AccountBalanceChanged(
      MyAccountInfo oldAccountInfo,
      MyAccountInfo newAccountInfo);

    protected sealed class CreateAccount_Clients\u003C\u003ESystem_Int64\u0023System_Int64 : ICallSite<IMyEventOwner, long, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long ownerIdentifier,
        in long startingBalance,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyBankingSystem.CreateAccount_Clients(ownerIdentifier, startingBalance);
      }
    }

    protected sealed class RemoveAccount_Clients\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long ownerIdentifier,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyBankingSystem.RemoveAccount_Clients(ownerIdentifier);
      }
    }

    protected sealed class UnlockAchievementForClient\u003C\u003ESystem_String : ICallSite<IMyEventOwner, string, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string achievement,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyBankingSystem.UnlockAchievementForClient(achievement);
      }
    }

    protected sealed class ChangeBalanceBroadcastToClients\u003C\u003ESystem_Int64\u0023System_Int64\u0023System_Int64 : ICallSite<IMyEventOwner, long, long, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long identifierId,
        in long amount,
        in long finalToBalance,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyBankingSystem.ChangeBalanceBroadcastToClients(identifierId, amount, finalToBalance);
      }
    }

    protected sealed class RequestTransfer_Server\u003C\u003ESystem_Int64\u0023System_Int64\u0023System_Int64 : ICallSite<IMyEventOwner, long, long, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long fromIdentifier,
        in long toIdentifier,
        in long amount,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyBankingSystem.RequestTransfer_Server(fromIdentifier, toIdentifier, amount);
      }
    }

    protected sealed class RequestTransfer_BroadcastToClients\u003C\u003ESystem_Int64\u0023System_Int64\u0023System_Int64\u0023System_Int64\u0023System_Int64 : ICallSite<IMyEventOwner, long, long, long, long, long, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long fromIdentifier,
        in long toIdentifier,
        in long amount,
        in long finalFromBalance,
        in long finalToBalance,
        in DBNull arg6)
      {
        MyBankingSystem.RequestTransfer_BroadcastToClients(fromIdentifier, toIdentifier, amount, finalFromBalance, finalToBalance);
      }
    }
  }
}
