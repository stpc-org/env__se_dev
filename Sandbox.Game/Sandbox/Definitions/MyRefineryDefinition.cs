// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyRefineryDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_RefineryDefinition), null)]
  public class MyRefineryDefinition : MyProductionBlockDefinition
  {
    public float RefineSpeed;
    public float MaterialEfficiency;
    public MyFixedPoint? OreAmountPerPullRequest;

    public float InventoryFillFactorMin { get; set; }

    public float InventoryFillFactorMax { get; set; }

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_RefineryDefinition refineryDefinition = builder as MyObjectBuilder_RefineryDefinition;
      this.RefineSpeed = refineryDefinition.RefineSpeed;
      this.MaterialEfficiency = refineryDefinition.MaterialEfficiency;
      this.OreAmountPerPullRequest = refineryDefinition.OreAmountPerPullRequest;
      this.InventoryFillFactorMin = refineryDefinition.InventoryFillFactorMin;
      this.InventoryFillFactorMax = refineryDefinition.InventoryFillFactorMax;
    }

    protected override bool BlueprintClassCanBeUsed(MyBlueprintClassDefinition blueprintClass)
    {
      foreach (MyBlueprintDefinitionBase blueprintDefinitionBase in blueprintClass)
      {
        if (blueprintDefinitionBase.Atomic)
        {
          MySandboxGame.Log.WriteLine("Blueprint " + blueprintDefinitionBase.DisplayNameText + " is atomic, but it is in a class used by refinery block");
          return false;
        }
      }
      return base.BlueprintClassCanBeUsed(blueprintClass);
    }

    protected override void InitializeLegacyBlueprintClasses(
      MyObjectBuilder_ProductionBlockDefinition ob)
    {
      ob.BlueprintClasses = new string[1]{ "Ingots" };
    }

    private class Sandbox_Definitions_MyRefineryDefinition\u003C\u003EActor : IActivator, IActivator<MyRefineryDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyRefineryDefinition();

      MyRefineryDefinition IActivator<MyRefineryDefinition>.CreateInstance() => new MyRefineryDefinition();
    }
  }
}
