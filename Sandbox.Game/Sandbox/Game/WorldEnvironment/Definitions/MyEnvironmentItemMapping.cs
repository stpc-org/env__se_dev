// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.Definitions.MyEnvironmentItemMapping
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Utils;

namespace Sandbox.Game.WorldEnvironment.Definitions
{
  public class MyEnvironmentItemMapping
  {
    public MyDiscreteSampler<MyRuntimeEnvironmentItemInfo>[] Samplers;
    public int[] Keys;
    public MyEnvironmentRule Rule;

    public MyEnvironmentItemMapping(
      MyRuntimeEnvironmentItemInfo[] map,
      MyEnvironmentRule rule,
      MyProceduralEnvironmentDefinition env)
    {
      this.Rule = rule;
      SortedDictionary<int, List<MyRuntimeEnvironmentItemInfo>> sortedDictionary = new SortedDictionary<int, List<MyRuntimeEnvironmentItemInfo>>();
      foreach (MyRuntimeEnvironmentItemInfo environmentItemInfo in map)
      {
        MyItemTypeDefinition type = environmentItemInfo.Type;
        List<MyRuntimeEnvironmentItemInfo> environmentItemInfoList;
        if (!sortedDictionary.TryGetValue(type.LodFrom + 1, out environmentItemInfoList))
        {
          environmentItemInfoList = new List<MyRuntimeEnvironmentItemInfo>();
          sortedDictionary[type.LodFrom + 1] = environmentItemInfoList;
        }
        environmentItemInfoList.Add(environmentItemInfo);
      }
      this.Keys = sortedDictionary.Keys.ToArray<int>();
      List<MyRuntimeEnvironmentItemInfo>[] array = sortedDictionary.Values.ToArray<List<MyRuntimeEnvironmentItemInfo>>();
      this.Samplers = new MyDiscreteSampler<MyRuntimeEnvironmentItemInfo>[this.Keys.Length];
      for (int start = 0; start < this.Keys.Length; ++start)
        this.Samplers[start] = this.PrepareSampler(array.Range<List<MyRuntimeEnvironmentItemInfo>>(start, array.Length).SelectMany<List<MyRuntimeEnvironmentItemInfo>, MyRuntimeEnvironmentItemInfo>((Func<List<MyRuntimeEnvironmentItemInfo>, IEnumerable<MyRuntimeEnvironmentItemInfo>>) (x => (IEnumerable<MyRuntimeEnvironmentItemInfo>) x)));
    }

    public MyDiscreteSampler<MyRuntimeEnvironmentItemInfo> PrepareSampler(
      IEnumerable<MyRuntimeEnvironmentItemInfo> items)
    {
      float num = 0.0f;
      foreach (MyRuntimeEnvironmentItemInfo environmentItemInfo in items)
        num += environmentItemInfo.Density;
      if ((double) num >= 1.0)
        return new MyDiscreteSampler<MyRuntimeEnvironmentItemInfo>(items, items.Select<MyRuntimeEnvironmentItemInfo, float>((Func<MyRuntimeEnvironmentItemInfo, float>) (x => x.Density)));
      return new MyDiscreteSampler<MyRuntimeEnvironmentItemInfo>(items.Concat<MyRuntimeEnvironmentItemInfo>((IEnumerable<MyRuntimeEnvironmentItemInfo>) new MyRuntimeEnvironmentItemInfo[1]), items.Select<MyRuntimeEnvironmentItemInfo, float>((Func<MyRuntimeEnvironmentItemInfo, float>) (x => x.Density)).Concat<float>((IEnumerable<float>) new float[1]
      {
        1f - num
      }));
    }

    public MyRuntimeEnvironmentItemInfo GetItemRated(
      int lod,
      float rate)
    {
      int index = this.Keys.BinaryIntervalSearch<int>(lod);
      return index > this.Samplers.Length ? (MyRuntimeEnvironmentItemInfo) null : this.Samplers[index].Sample(rate);
    }

    public bool Valid => this.Samplers != null;

    public bool ValidForLod(int lod) => this.Keys.BinaryIntervalSearch<int>(lod) <= this.Samplers.Length;

    public MyDiscreteSampler<MyRuntimeEnvironmentItemInfo> Sampler(
      int lod)
    {
      int index = this.Keys.BinaryIntervalSearch<int>(lod);
      return index >= this.Samplers.Length ? (MyDiscreteSampler<MyRuntimeEnvironmentItemInfo>) null : this.Samplers[index];
    }
  }
}
