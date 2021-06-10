// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyProductionBlockDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game;
using Sandbox.Game.Localization;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ProductionBlockDefinition), null)]
  public class MyProductionBlockDefinition : MyCubeBlockDefinition
  {
    public float InventoryMaxVolume;
    public Vector3 InventorySize;
    public MyStringHash ResourceSinkGroup;
    public float StandbyPowerConsumption;
    public float OperationalPowerConsumption;
    public List<MyBlueprintClassDefinition> BlueprintClasses;
    public MyInventoryConstraint InputInventoryConstraint;
    public MyInventoryConstraint OutputInventoryConstraint;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ProductionBlockDefinition ob = builder as MyObjectBuilder_ProductionBlockDefinition;
      this.InventoryMaxVolume = ob.InventoryMaxVolume;
      this.InventorySize = ob.InventorySize;
      this.ResourceSinkGroup = MyStringHash.GetOrCompute(ob.ResourceSinkGroup);
      this.StandbyPowerConsumption = ob.StandbyPowerConsumption;
      this.OperationalPowerConsumption = ob.OperationalPowerConsumption;
      if (ob.BlueprintClasses == null)
        this.InitializeLegacyBlueprintClasses(ob);
      this.BlueprintClasses = new List<MyBlueprintClassDefinition>();
      foreach (string blueprintClass1 in ob.BlueprintClasses)
      {
        MyBlueprintClassDefinition blueprintClass2 = MyDefinitionManager.Static.GetBlueprintClass(blueprintClass1);
        if (blueprintClass2 != null)
          this.BlueprintClasses.Add(blueprintClass2);
      }
    }

    protected virtual bool BlueprintClassCanBeUsed(MyBlueprintClassDefinition blueprintClass) => true;

    protected virtual void InitializeLegacyBlueprintClasses(
      MyObjectBuilder_ProductionBlockDefinition ob)
    {
      ob.BlueprintClasses = new string[0];
    }

    public void LoadPostProcess()
    {
      int index = 0;
      while (index < this.BlueprintClasses.Count)
      {
        if (!this.BlueprintClassCanBeUsed(this.BlueprintClasses[index]))
          this.BlueprintClasses.RemoveAt(index);
        else
          ++index;
      }
      this.InputInventoryConstraint = this.PrepareConstraint(MySpaceTexts.ToolTipItemFilter_GenericProductionBlockInput, (IEnumerable<MyBlueprintClassDefinition>) this.GetInputClasses(), true);
      this.OutputInventoryConstraint = this.PrepareConstraint(MySpaceTexts.ToolTipItemFilter_GenericProductionBlockOutput, (IEnumerable<MyBlueprintClassDefinition>) this.GetOutputClasses(), false);
    }

    private MyInventoryConstraint PrepareConstraint(
      MyStringId descriptionId,
      IEnumerable<MyBlueprintClassDefinition> classes,
      bool input)
    {
      string icon = (string) null;
      foreach (MyBlueprintClassDefinition blueprintClassDefinition in classes)
      {
        string str = input ? blueprintClassDefinition.InputConstraintIcon : blueprintClassDefinition.OutputConstraintIcon;
        if (str != null)
        {
          if (icon == null)
            icon = str;
          else if (icon != str)
          {
            icon = (string) null;
            break;
          }
        }
      }
      MyInventoryConstraint inventoryConstraint = new MyInventoryConstraint(string.Format(MyTexts.GetString(descriptionId), (object) this.DisplayNameText), icon);
      foreach (MyBlueprintClassDefinition blueprintClassDefinition in classes)
      {
        foreach (MyBlueprintDefinitionBase blueprintDefinitionBase in blueprintClassDefinition)
        {
          foreach (MyBlueprintDefinitionBase.Item obj in input ? blueprintDefinitionBase.Prerequisites : blueprintDefinitionBase.Results)
            inventoryConstraint.Add(obj.Id);
        }
      }
      return inventoryConstraint;
    }

    protected virtual List<MyBlueprintClassDefinition> GetInputClasses() => this.BlueprintClasses;

    protected virtual List<MyBlueprintClassDefinition> GetOutputClasses() => this.BlueprintClasses;

    private class Sandbox_Definitions_MyProductionBlockDefinition\u003C\u003EActor : IActivator, IActivator<MyProductionBlockDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyProductionBlockDefinition();

      MyProductionBlockDefinition IActivator<MyProductionBlockDefinition>.CreateInstance() => new MyProductionBlockDefinition();
    }
  }
}
