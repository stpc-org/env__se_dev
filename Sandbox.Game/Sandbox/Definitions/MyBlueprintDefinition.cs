// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyBlueprintDefinition
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
  [MyDefinitionType(typeof (MyObjectBuilder_BlueprintDefinition), null)]
  public class MyBlueprintDefinition : MyBlueprintDefinitionBase
  {
    protected override void Init(MyObjectBuilder_DefinitionBase ob)
    {
      base.Init(ob);
      MyObjectBuilder_BlueprintDefinition blueprintDefinition = (MyObjectBuilder_BlueprintDefinition) ob;
      this.Prerequisites = new MyBlueprintDefinitionBase.Item[blueprintDefinition.Prerequisites.Length];
      for (int index = 0; index < this.Prerequisites.Length; ++index)
        this.Prerequisites[index] = MyBlueprintDefinitionBase.Item.FromObjectBuilder(blueprintDefinition.Prerequisites[index]);
      if (blueprintDefinition.Result != null)
      {
        this.Results = new MyBlueprintDefinitionBase.Item[1];
        this.Results[0] = MyBlueprintDefinitionBase.Item.FromObjectBuilder(blueprintDefinition.Result);
      }
      else
      {
        this.Results = new MyBlueprintDefinitionBase.Item[blueprintDefinition.Results.Length];
        for (int index = 0; index < this.Results.Length; ++index)
          this.Results[index] = MyBlueprintDefinitionBase.Item.FromObjectBuilder(blueprintDefinition.Results[index]);
      }
      this.BaseProductionTimeInSeconds = blueprintDefinition.BaseProductionTimeInSeconds;
      this.PostprocessNeeded = true;
      this.ProgressBarSoundCue = blueprintDefinition.ProgressBarSoundCue;
      this.IsPrimary = blueprintDefinition.IsPrimary;
      this.Priority = blueprintDefinition.Priority;
    }

    public override void Postprocess()
    {
      bool flag = false;
      float num = 0.0f;
      foreach (MyBlueprintDefinitionBase.Item result in this.Results)
      {
        if (result.Id.TypeId != typeof (MyObjectBuilder_Ore) && result.Id.TypeId != typeof (MyObjectBuilder_Ingot))
          flag = true;
        MyPhysicalItemDefinition definition;
        MyDefinitionManager.Static.TryGetPhysicalItemDefinition(result.Id, out definition);
        if (definition == null)
          return;
        num += (float) result.Amount * definition.Volume;
      }
      this.Atomic = flag;
      this.OutputVolume = num;
      this.PostprocessNeeded = false;
    }

    public override int GetBlueprints(
      List<MyBlueprintDefinitionBase.ProductionInfo> blueprints)
    {
      blueprints.Add(new MyBlueprintDefinitionBase.ProductionInfo()
      {
        Blueprint = (MyBlueprintDefinitionBase) this,
        Amount = (MyFixedPoint) 1
      });
      return 1;
    }

    private class Sandbox_Definitions_MyBlueprintDefinition\u003C\u003EActor : IActivator, IActivator<MyBlueprintDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyBlueprintDefinition();

      MyBlueprintDefinition IActivator<MyBlueprintDefinition>.CreateInstance() => new MyBlueprintDefinition();
    }
  }
}
