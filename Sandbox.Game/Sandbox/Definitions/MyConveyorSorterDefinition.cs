// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyConveyorSorterDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ConveyorSorterDefinition), null)]
  public class MyConveyorSorterDefinition : MyCubeBlockDefinition
  {
    public MyStringHash ResourceSinkGroup;
    public float PowerInput;
    public Vector3 InventorySize;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ConveyorSorterDefinition sorterDefinition = (MyObjectBuilder_ConveyorSorterDefinition) builder;
      this.ResourceSinkGroup = MyStringHash.GetOrCompute(sorterDefinition.ResourceSinkGroup);
      this.PowerInput = sorterDefinition.PowerInput;
      this.InventorySize = sorterDefinition.InventorySize;
    }

    private class Sandbox_Definitions_MyConveyorSorterDefinition\u003C\u003EActor : IActivator, IActivator<MyConveyorSorterDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyConveyorSorterDefinition();

      MyConveyorSorterDefinition IActivator<MyConveyorSorterDefinition>.CreateInstance() => new MyConveyorSorterDefinition();
    }
  }
}
