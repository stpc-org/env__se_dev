// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.HudBlockInfoExtensions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities.Cube;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Gui
{
  public static class HudBlockInfoExtensions
  {
    public static void LoadDefinition(
      this MyHudBlockInfo blockInfo,
      MyCubeBlockDefinition definition,
      bool merge = true)
    {
      blockInfo.InitBlockInfo(definition);
      if (definition.MultiBlock != null)
      {
        MyMultiBlockDefinition multiBlockDefinition = MyDefinitionManager.Static.TryGetMultiBlockDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_MultiBlockDefinition), definition.MultiBlock));
        if (multiBlockDefinition != null)
        {
          foreach (MyMultiBlockDefinition.MyMultiBlockPartDefinition blockDefinition in multiBlockDefinition.BlockDefinitions)
          {
            MyCubeBlockDefinition definition1 = (MyCubeBlockDefinition) null;
            MyDefinitionManager.Static.TryGetDefinition<MyCubeBlockDefinition>(blockDefinition.Id, out definition1);
            if (definition1 != null)
              blockInfo.AddComponentsForBlock(definition1);
          }
        }
      }
      else
        blockInfo.AddComponentsForBlock(definition);
      if (!merge)
        return;
      blockInfo.MergeSameComponents();
    }

    public static void LoadDefinition(
      this MyHudBlockInfo blockInfo,
      MyCubeBlockDefinition definition,
      DictionaryReader<MyDefinitionId, int> materials,
      bool merge = true)
    {
      blockInfo.InitBlockInfo(definition);
      foreach (KeyValuePair<MyDefinitionId, int> material in materials)
      {
        MyDefinitionBase definition1 = MyDefinitionManager.Static.GetDefinition(material.Key);
        MyHudBlockInfo.ComponentInfo componentInfo = new MyHudBlockInfo.ComponentInfo();
        if (definition1 == null)
        {
          MyPhysicalItemDefinition definition2 = (MyPhysicalItemDefinition) null;
          if (MyDefinitionManager.Static.TryGetPhysicalItemDefinition(material.Key, out definition2))
          {
            componentInfo.ComponentName = definition2.DisplayNameText;
            componentInfo.Icons = definition2.Icons;
            componentInfo.DefinitionId = definition2.Id;
            componentInfo.TotalCount = 1;
          }
          else
            continue;
        }
        else
        {
          componentInfo.DefinitionId = definition1.Id;
          componentInfo.ComponentName = definition1.DisplayNameText;
          componentInfo.Icons = definition1.Icons;
          componentInfo.TotalCount = material.Value;
        }
        blockInfo.Components.Add(componentInfo);
      }
      if (!merge)
        return;
      blockInfo.MergeSameComponents();
    }

    public static void AddComponentsForBlock(
      this MyHudBlockInfo blockInfo,
      MyCubeBlockDefinition definition)
    {
      for (int index = 0; index < definition.Components.Length; ++index)
      {
        MyCubeBlockDefinition.Component component = definition.Components[index];
        blockInfo.Components.Add(new MyHudBlockInfo.ComponentInfo()
        {
          DefinitionId = component.Definition.Id,
          ComponentName = component.Definition.DisplayNameText,
          Icons = component.Definition.Icons,
          TotalCount = component.Count
        });
      }
    }

    public static void InitBlockInfo(
      this MyHudBlockInfo blockInfo,
      MyCubeBlockDefinition definition,
      MySlimBlock block = null)
    {
      blockInfo.DefinitionId = definition.Id;
      blockInfo.BlockName = definition.DisplayNameText;
      blockInfo.SetContextHelp((MyDefinitionBase) definition);
      blockInfo.PCUCost = definition.PCU;
      blockInfo.BlockIcons = definition.Icons;
      blockInfo.BlockIntegrity = 0.0f;
      blockInfo.CriticalComponentIndex = (int) definition.CriticalGroup;
      blockInfo.CriticalIntegrity = definition.CriticalIntegrityRatio;
      blockInfo.OwnershipIntegrity = definition.OwnershipIntegrityRatio;
      blockInfo.MissingComponentIndex = -1;
      blockInfo.GridSize = definition.CubeSize;
      blockInfo.Components.Clear();
      blockInfo.BlockBuiltBy = block != null ? block.BuiltBy : 0L;
    }

    public static void MergeSameComponents(this MyHudBlockInfo blockInfo)
    {
      for (int index1 = blockInfo.Components.Count - 1; index1 >= 0; --index1)
      {
        for (int index2 = index1 - 1; index2 >= 0; --index2)
        {
          if (blockInfo.Components[index1].DefinitionId == blockInfo.Components[index2].DefinitionId)
          {
            MyHudBlockInfo.ComponentInfo component = blockInfo.Components[index2];
            component.TotalCount += blockInfo.Components[index1].TotalCount;
            component.MountedCount += blockInfo.Components[index1].MountedCount;
            component.StockpileCount += blockInfo.Components[index1].StockpileCount;
            blockInfo.Components[index2] = component;
            blockInfo.Components.RemoveAt(index1);
            break;
          }
        }
      }
    }
  }
}
