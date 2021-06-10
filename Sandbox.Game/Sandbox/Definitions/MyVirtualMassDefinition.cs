// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyVirtualMassDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_VirtualMassDefinition), null)]
  public class MyVirtualMassDefinition : MyCubeBlockDefinition
  {
    public MyStringHash ResourceSinkGroup;
    public float RequiredPowerInput;
    public float VirtualMass;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_VirtualMassDefinition virtualMassDefinition = builder as MyObjectBuilder_VirtualMassDefinition;
      this.ResourceSinkGroup = MyStringHash.GetOrCompute(virtualMassDefinition.ResourceSinkGroup);
      this.RequiredPowerInput = virtualMassDefinition.RequiredPowerInput;
      this.VirtualMass = virtualMassDefinition.VirtualMass;
    }

    private class Sandbox_Definitions_MyVirtualMassDefinition\u003C\u003EActor : IActivator, IActivator<MyVirtualMassDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyVirtualMassDefinition();

      MyVirtualMassDefinition IActivator<MyVirtualMassDefinition>.CreateInstance() => new MyVirtualMassDefinition();
    }
  }
}
