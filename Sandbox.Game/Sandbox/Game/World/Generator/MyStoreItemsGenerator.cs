// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyStoreItemsGenerator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Profiler;

namespace Sandbox.Game.World.Generator
{
  public class MyStoreItemsGenerator
  {
    private Dictionary<MyFactionTypes, IMyStoreItemsGeneratorFactionTypeStrategy> m_generatorStrategies = new Dictionary<MyFactionTypes, IMyStoreItemsGeneratorFactionTypeStrategy>();
    private Task m_initTask;

    public MyStoreItemsGenerator() => this.m_initTask = Parallel.Start((Action) (() =>
    {
      this.m_generatorStrategies.Add(MyFactionTypes.Miner, (IMyStoreItemsGeneratorFactionTypeStrategy) new MyMinersFactionTypeStrategy());
      this.m_generatorStrategies.Add(MyFactionTypes.Trader, (IMyStoreItemsGeneratorFactionTypeStrategy) new MyTradersFactionTypeStrategy());
      this.m_generatorStrategies.Add(MyFactionTypes.Builder, (IMyStoreItemsGeneratorFactionTypeStrategy) new MyBuildersFactionTypeStrategy());
    }), Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Loading, "MyStoreItemsGenerator.ctor"), WorkPriority.VeryLow);

    public void Update(MyFaction faction, bool firstGeneration)
    {
      if (this.m_initTask.valid)
      {
        this.m_initTask.WaitOrExecute();
        this.m_initTask = new Task();
      }
      IMyStoreItemsGeneratorFactionTypeStrategy factionTypeStrategy;
      if (this.m_generatorStrategies.TryGetValue(faction.FactionType, out factionTypeStrategy))
      {
        factionTypeStrategy.UpdateStationsStoreItems(faction, firstGeneration);
      }
      else
      {
        if (faction.FactionType == MyFactionTypes.None)
          return;
        int factionType = (int) faction.FactionType;
      }
    }
  }
}
