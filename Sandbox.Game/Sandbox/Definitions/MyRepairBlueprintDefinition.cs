// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyRepairBlueprintDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_RepairBlueprintDefinition), null)]
  public class MyRepairBlueprintDefinition : MyBlueprintDefinition
  {
    public float RepairAmount;

    protected override void Init(MyObjectBuilder_DefinitionBase ob)
    {
      base.Init(ob);
      this.RepairAmount = (ob as MyObjectBuilder_RepairBlueprintDefinition).RepairAmount;
    }

    private class Sandbox_Definitions_MyRepairBlueprintDefinition\u003C\u003EActor : IActivator, IActivator<MyRepairBlueprintDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyRepairBlueprintDefinition();

      MyRepairBlueprintDefinition IActivator<MyRepairBlueprintDefinition>.CreateInstance() => new MyRepairBlueprintDefinition();
    }
  }
}
