// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyContractTypeBaseStrategy
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Contracts;
using VRage.Game.Definitions.SessionComponents;
using VRage.Library.Utils;

namespace Sandbox.Game.World.Generator
{
  public abstract class MyContractTypeBaseStrategy
  {
    public static readonly int TICKS_TO_LIVE = 1;
    protected MySessionComponentEconomyDefinition m_economyDefinition;

    public MyContractTypeBaseStrategy(
      MySessionComponentEconomyDefinition economyDefinition)
    {
      this.m_economyDefinition = economyDefinition;
    }

    public abstract MyContractCreationResults GenerateContract(
      out MyContract contract,
      long factionId,
      long stationId,
      MyMinimalPriceCalculator calculator,
      MyTimeSpan now);

    public abstract bool CanBeGenerated(MyStation station, MyFaction faction);
  }
}
