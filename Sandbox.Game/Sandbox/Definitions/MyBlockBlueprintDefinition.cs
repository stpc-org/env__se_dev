// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyBlockBlueprintDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_BlockBlueprintDefinition), null)]
  public class MyBlockBlueprintDefinition : MyBlueprintDefinition
  {
    protected override void Init(MyObjectBuilder_DefinitionBase ob) => base.Init(ob);

    public override void Postprocess()
    {
      this.Atomic = false;
      float num = 0.0f;
      foreach (MyBlueprintDefinitionBase.Item result in this.Results)
      {
        MyCubeBlockDefinition blockDefinition;
        MyDefinitionManager.Static.TryGetCubeBlockDefinition(result.Id, out blockDefinition);
        if (blockDefinition == null)
          return;
        num += (float) result.Amount * blockDefinition.Mass;
      }
      this.OutputVolume = num;
      this.PostprocessNeeded = false;
    }

    private class Sandbox_Definitions_MyBlockBlueprintDefinition\u003C\u003EActor : IActivator, IActivator<MyBlockBlueprintDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyBlockBlueprintDefinition();

      MyBlockBlueprintDefinition IActivator<MyBlockBlueprintDefinition>.CreateInstance() => new MyBlockBlueprintDefinition();
    }
  }
}
