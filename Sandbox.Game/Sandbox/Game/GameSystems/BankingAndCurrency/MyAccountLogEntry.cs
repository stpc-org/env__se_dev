// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.BankingAndCurrency.MyAccountLogEntry
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;

namespace Sandbox.Game.GameSystems.BankingAndCurrency
{
  public struct MyAccountLogEntry
  {
    public long ChangeIdentifier { get; set; }

    public long Amount { get; set; }

    public DateTime DateTime { get; set; }
  }
}
