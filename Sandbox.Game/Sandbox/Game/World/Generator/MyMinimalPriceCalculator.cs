// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyMinimalPriceCalculator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.World.Generator
{
  public sealed class MyMinimalPriceCalculator
  {
    public const float BLOCK_INTEGRITY_THRESHOLD = 0.5f;
    private Dictionary<MyDefinitionId, int> m_minimalPrices = new Dictionary<MyDefinitionId, int>();
    private Dictionary<MyDefinitionId, MyMinimalPriceCalculator.MyBlockInfo> m_blocksInfo = new Dictionary<MyDefinitionId, MyMinimalPriceCalculator.MyBlockInfo>();
    private Dictionary<string, MyMinimalPriceCalculator.MyPrefabInfo> m_prefabsInfo = new Dictionary<string, MyMinimalPriceCalculator.MyPrefabInfo>();

    public bool TryGetItemMinimalPrice(MyDefinitionId itemId, out int minimalPrice) => this.m_minimalPrices.TryGetValue(itemId, out minimalPrice);

    public bool TryGetPrefabMinimalPrice(string prefabName, out int minimalPrice)
    {
      MyMinimalPriceCalculator.MyPrefabInfo myPrefabInfo;
      int num = this.m_prefabsInfo.TryGetValue(prefabName, out myPrefabInfo) ? 1 : 0;
      minimalPrice = myPrefabInfo.MinimalPrice;
      return num != 0;
    }

    public bool TryGetPrefabMinimalRepairPrice(string prefabName, out int minimalRepairPrice)
    {
      MyMinimalPriceCalculator.MyPrefabInfo myPrefabInfo;
      int num = this.m_prefabsInfo.TryGetValue(prefabName, out myPrefabInfo) ? 1 : 0;
      minimalRepairPrice = myPrefabInfo.MinimalRepairPrice;
      return num != 0;
    }

    public bool TryGetPrefabTotalPcu(string prefabName, out int totalPcu)
    {
      MyMinimalPriceCalculator.MyPrefabInfo myPrefabInfo;
      int num = this.m_prefabsInfo.TryGetValue(prefabName, out myPrefabInfo) ? 1 : 0;
      totalPcu = myPrefabInfo.TotalPcu;
      return num != 0;
    }

    public bool TryGetPrefabEnvironmentType(
      string prefabName,
      out MyEnvironmentTypes environmentType)
    {
      MyMinimalPriceCalculator.MyPrefabInfo myPrefabInfo;
      int num = this.m_prefabsInfo.TryGetValue(prefabName, out myPrefabInfo) ? 1 : 0;
      environmentType = myPrefabInfo.EnvironmentType;
      return num != 0;
    }

    public void CalculatePrefabInformation(
      string[] prefabNames,
      float baseCostProductionSpeedMultiplier = 1f)
    {
      foreach (string prefabName in prefabNames)
      {
        int num1 = 0;
        int num2 = 0;
        MyPrefabDefinition prefabDefinition = MyDefinitionManager.Static.GetPrefabDefinition(prefabName);
        if (prefabDefinition != null && prefabDefinition.CubeGrids != null && (prefabDefinition.CubeGrids.Length != 0 && !string.IsNullOrEmpty(prefabDefinition.CubeGrids[0].DisplayName)))
        {
          int num3 = 0;
          foreach (MyObjectBuilder_CubeGrid cubeGrid in prefabDefinition.CubeGrids)
          {
            foreach (MyObjectBuilder_CubeBlock cubeBlock in cubeGrid.CubeBlocks)
            {
              MyDefinitionId myDefinitionId = new MyDefinitionId(cubeBlock.TypeId, cubeBlock.SubtypeName);
              MyMinimalPriceCalculator.MyBlockInfo myBlockInfo;
              if (!this.m_blocksInfo.TryGetValue(myDefinitionId, out myBlockInfo))
              {
                int minimalPrice = 0;
                int pcu = 0;
                this.CalculateBlockMinimalPriceAndPcu(myDefinitionId, baseCostProductionSpeedMultiplier, ref minimalPrice, ref pcu);
                myBlockInfo = new MyMinimalPriceCalculator.MyBlockInfo()
                {
                  MinimalPrice = minimalPrice,
                  Pcu = pcu
                };
              }
              num3 += myBlockInfo.Pcu;
              num1 += myBlockInfo.MinimalPrice;
              if ((double) cubeBlock.IntegrityPercent <= 0.5)
                num2 += myBlockInfo.MinimalPrice;
            }
          }
          this.m_prefabsInfo[prefabName] = new MyMinimalPriceCalculator.MyPrefabInfo()
          {
            MinimalPrice = num1,
            MinimalRepairPrice = num2,
            TotalPcu = num3,
            EnvironmentType = prefabDefinition.EnvironmentType
          };
        }
      }
    }

    public void CalculateMinimalPrices(
      SerializableDefinitionId[] itemsList,
      float baseCostProductionSpeedMultiplier = 1f)
    {
      if (itemsList == null)
        return;
      foreach (SerializableDefinitionId items in itemsList)
      {
        if (!this.m_minimalPrices.ContainsKey((MyDefinitionId) items))
        {
          int minimalPrice = 0;
          this.CalculateItemMinimalPrice((MyDefinitionId) items, baseCostProductionSpeedMultiplier, ref minimalPrice);
          this.m_minimalPrices[(MyDefinitionId) items] = minimalPrice;
        }
      }
    }

    private void CalculateItemMinimalPrice(
      MyDefinitionId itemId,
      float baseCostProductionSpeedMultiplier,
      ref int minimalPrice)
    {
      MyPhysicalItemDefinition definition1 = (MyPhysicalItemDefinition) null;
      if (MyDefinitionManager.Static.TryGetDefinition<MyPhysicalItemDefinition>(itemId, out definition1) && definition1.MinimalPricePerUnit != -1)
      {
        minimalPrice += definition1.MinimalPricePerUnit;
      }
      else
      {
        MyBlueprintDefinitionBase definition2 = (MyBlueprintDefinitionBase) null;
        if (!MyDefinitionManager.Static.TryGetBlueprintDefinitionByResultId(itemId, out definition2))
          return;
        float num1 = definition1.IsIngot ? 1f : MySession.Static.AssemblerEfficiencyMultiplier;
        int num2 = 0;
        foreach (MyBlueprintDefinitionBase.Item prerequisite in definition2.Prerequisites)
        {
          int minimalPrice1 = 0;
          this.CalculateItemMinimalPrice(prerequisite.Id, baseCostProductionSpeedMultiplier, ref minimalPrice1);
          float num3 = (float) prerequisite.Amount / num1;
          num2 += (int) ((double) minimalPrice1 * (double) num3);
        }
        float num4 = definition1.IsIngot ? MySession.Static.RefinerySpeedMultiplier : MySession.Static.AssemblerSpeedMultiplier;
        for (int index = 0; index < definition2.Results.Length; ++index)
        {
          MyBlueprintDefinitionBase.Item result = definition2.Results[index];
          if (result.Id == itemId)
          {
            float amount = (float) result.Amount;
            if ((double) amount == 0.0)
            {
              MyLog.Default.WriteToLogAndAssert("Amount is 0 for - " + (object) result.Id);
            }
            else
            {
              float num3 = (float) (1.0 + Math.Log((double) definition2.BaseProductionTimeInSeconds + 1.0) * (double) baseCostProductionSpeedMultiplier / (double) num4);
              minimalPrice += (int) ((double) num2 * (1.0 / (double) amount) * (double) num3);
              break;
            }
          }
        }
      }
    }

    private void CalculateBlockMinimalPriceAndPcu(
      MyDefinitionId blockId,
      float baseCostProductionSpeedMultiplier,
      ref int minimalPrice,
      ref int pcu)
    {
      minimalPrice = 0;
      pcu = 0;
      MyCubeBlockDefinition blockDefinition;
      if (!MyDefinitionManager.Static.TryGetCubeBlockDefinition(blockId, out blockDefinition))
        return;
      pcu += blockDefinition.PCU;
      foreach (MyCubeBlockDefinition.Component component in blockDefinition.Components)
      {
        int minimalPrice1 = 0;
        if (!this.m_minimalPrices.TryGetValue(component.Definition.Id, out minimalPrice1))
        {
          this.CalculateItemMinimalPrice(component.Definition.Id, baseCostProductionSpeedMultiplier, ref minimalPrice1);
          this.m_minimalPrices[component.Definition.Id] = minimalPrice1;
        }
        minimalPrice += minimalPrice1 * component.Count;
      }
    }

    private struct MyPrefabInfo
    {
      public int MinimalPrice;
      public int MinimalRepairPrice;
      public int TotalPcu;
      public MyEnvironmentTypes EnvironmentType;
    }

    private struct MyBlockInfo
    {
      public int MinimalPrice;
      public int Pcu;
    }
  }
}
