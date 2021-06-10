// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyCompositeBlueprintDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_CompositeBlueprintDefinition), null)]
  public class MyCompositeBlueprintDefinition : MyBlueprintDefinitionBase
  {
    private MyBlueprintDefinitionBase[] m_blueprints;
    private MyBlueprintDefinitionBase.Item[] m_items;
    private static List<MyBlueprintDefinitionBase.Item> m_tmpPrerequisiteList = new List<MyBlueprintDefinitionBase.Item>();
    private static List<MyBlueprintDefinitionBase.Item> m_tmpResultList = new List<MyBlueprintDefinitionBase.Item>();

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_CompositeBlueprintDefinition blueprintDefinition = builder as MyObjectBuilder_CompositeBlueprintDefinition;
      this.m_items = new MyBlueprintDefinitionBase.Item[blueprintDefinition.Blueprints == null ? 0 : blueprintDefinition.Blueprints.Length];
      for (int index = 0; index < this.m_items.Length; ++index)
        this.m_items[index] = MyBlueprintDefinitionBase.Item.FromObjectBuilder(blueprintDefinition.Blueprints[index]);
      this.PostprocessNeeded = true;
    }

    public override void Postprocess()
    {
      foreach (MyBlueprintDefinitionBase.Item obj in this.m_items)
      {
        if (!MyDefinitionManager.Static.HasBlueprint(obj.Id) || MyDefinitionManager.Static.GetBlueprintDefinition(obj.Id).PostprocessNeeded)
          return;
      }
      float num1 = 0.0f;
      bool flag = false;
      float num2 = 0.0f;
      this.m_blueprints = new MyBlueprintDefinitionBase[this.m_items.Length];
      MyCompositeBlueprintDefinition.m_tmpPrerequisiteList.Clear();
      MyCompositeBlueprintDefinition.m_tmpResultList.Clear();
      for (int index = 0; index < this.m_items.Length; ++index)
      {
        MyFixedPoint amount = this.m_items[index].Amount;
        MyBlueprintDefinitionBase blueprintDefinition = MyDefinitionManager.Static.GetBlueprintDefinition(this.m_items[index].Id);
        this.m_blueprints[index] = blueprintDefinition;
        flag = flag || blueprintDefinition.Atomic;
        num1 += blueprintDefinition.OutputVolume * (float) amount;
        num2 += blueprintDefinition.BaseProductionTimeInSeconds * (float) amount;
        this.PostprocessAddSubblueprint(blueprintDefinition, amount);
      }
      this.Prerequisites = MyCompositeBlueprintDefinition.m_tmpPrerequisiteList.ToArray();
      this.Results = MyCompositeBlueprintDefinition.m_tmpResultList.ToArray();
      MyCompositeBlueprintDefinition.m_tmpPrerequisiteList.Clear();
      MyCompositeBlueprintDefinition.m_tmpResultList.Clear();
      this.Atomic = flag;
      this.OutputVolume = num1;
      this.BaseProductionTimeInSeconds = num2;
      this.PostprocessNeeded = false;
    }

    private void PostprocessAddSubblueprint(
      MyBlueprintDefinitionBase blueprint,
      MyFixedPoint blueprintAmount)
    {
      for (int index = 0; index < blueprint.Prerequisites.Length; ++index)
      {
        MyBlueprintDefinitionBase.Item prerequisite = blueprint.Prerequisites[index];
        prerequisite.Amount *= blueprintAmount;
        this.AddToItemList(MyCompositeBlueprintDefinition.m_tmpPrerequisiteList, prerequisite);
      }
      for (int index = 0; index < blueprint.Results.Length; ++index)
      {
        MyBlueprintDefinitionBase.Item result = blueprint.Results[index];
        result.Amount *= blueprintAmount;
        this.AddToItemList(MyCompositeBlueprintDefinition.m_tmpResultList, result);
      }
    }

    private void AddToItemList(
      List<MyBlueprintDefinitionBase.Item> items,
      MyBlueprintDefinitionBase.Item toAdd)
    {
      MyBlueprintDefinitionBase.Item obj = new MyBlueprintDefinitionBase.Item();
      int index;
      for (index = 0; index < items.Count; ++index)
      {
        obj = items[index];
        if (obj.Id == toAdd.Id)
          break;
      }
      if (index >= items.Count)
      {
        items.Add(toAdd);
      }
      else
      {
        obj.Amount += toAdd.Amount;
        items[index] = obj;
      }
    }

    public override int GetBlueprints(
      List<MyBlueprintDefinitionBase.ProductionInfo> blueprints)
    {
      int num = 0;
      for (int index1 = 0; index1 < this.m_blueprints.Length; ++index1)
      {
        int blueprints1 = this.m_blueprints[index1].GetBlueprints(blueprints);
        int count = blueprints.Count;
        for (int index2 = count - 1; index2 >= count - blueprints1; --index2)
        {
          MyBlueprintDefinitionBase.ProductionInfo blueprint = blueprints[index2];
          blueprint.Amount *= this.m_items[index1].Amount;
          blueprints[index2] = blueprint;
        }
        num += blueprints1;
      }
      return num;
    }

    private class Sandbox_Definitions_MyCompositeBlueprintDefinition\u003C\u003EActor : IActivator, IActivator<MyCompositeBlueprintDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyCompositeBlueprintDefinition();

      MyCompositeBlueprintDefinition IActivator<MyCompositeBlueprintDefinition>.CreateInstance() => new MyCompositeBlueprintDefinition();
    }
  }
}
