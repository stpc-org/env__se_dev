// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.BankingAndCurrency.MyAccount
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using VRage.Game.ObjectBuilders.Components.BankingAndCurrency;

namespace Sandbox.Game.GameSystems.BankingAndCurrency
{
  public class MyAccount
  {
    public List<MyAccountLogEntry> Log = new List<MyAccountLogEntry>();

    public long OwnerIdentifier { get; private set; }

    public long Balance { get; private set; }

    public MyAccount()
    {
    }

    public MyAccount(long ownerIdentifier, long startingBalance)
    {
      this.OwnerIdentifier = ownerIdentifier;
      this.Balance = startingBalance;
    }

    public void Init(MyObjectBuilder_Account obAccount)
    {
      this.OwnerIdentifier = obAccount.OwnerIdentifier;
      this.Balance = obAccount.Balance;
      this.Log = new List<MyAccountLogEntry>();
      foreach (MyObjectBuilder_Account.MyObjectBuilder_AccountLogEntry builderAccountLogEntry in obAccount.Log)
        this.Log.Add(new MyAccountLogEntry()
        {
          Amount = builderAccountLogEntry.Amount,
          ChangeIdentifier = builderAccountLogEntry.ChangeIdentifier,
          DateTime = new DateTime(builderAccountLogEntry.DateTime)
        });
    }

    public MyObjectBuilder_Account GetObjectBuilder()
    {
      MyObjectBuilder_Account objectBuilderAccount = new MyObjectBuilder_Account();
      objectBuilderAccount.OwnerIdentifier = this.OwnerIdentifier;
      objectBuilderAccount.Balance = this.Balance;
      objectBuilderAccount.Log = new List<MyObjectBuilder_Account.MyObjectBuilder_AccountLogEntry>();
      foreach (MyAccountLogEntry myAccountLogEntry in this.Log)
        objectBuilderAccount.Log.Add(new MyObjectBuilder_Account.MyObjectBuilder_AccountLogEntry()
        {
          Amount = myAccountLogEntry.Amount,
          ChangeIdentifier = myAccountLogEntry.ChangeIdentifier,
          DateTime = myAccountLogEntry.DateTime.Ticks
        });
      return objectBuilderAccount;
    }

    internal void Add(long valueToAdd) => this.Balance += valueToAdd;

    internal void Subtract(long valueToSubtract) => this.Balance -= valueToSubtract;

    internal MyAccountInfo GetAccountInfo()
    {
      MyAccountInfo myAccountInfo = new MyAccountInfo()
      {
        Balance = this.Balance,
        OwnerIdentifier = this.OwnerIdentifier
      };
      myAccountInfo.Log = this.Log.ToArray();
      return myAccountInfo;
    }

    internal void ResetBalance(long newBalance) => this.Balance = newBalance;

    public static long operator +(MyAccount account, long addValue) => account.Balance + addValue;

    public static long operator +(MyAccount account1, MyAccount account2) => account1.Balance + account2.Balance;

    public static long operator -(MyAccount account, long subtractValue) => account.Balance - subtractValue;

    public static long operator -(MyAccount account1, MyAccount account2) => account1.Balance - account2.Balance;
  }
}
