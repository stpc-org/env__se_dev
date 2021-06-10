// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyPowerProducerDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_PowerProducerDefinition), null)]
  public class MyPowerProducerDefinition : MyCubeBlockDefinition
  {
    public MyStringHash ResourceSourceGroup;
    public float MaxPowerOutput;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      if (!(builder is MyObjectBuilder_PowerProducerDefinition producerDefinition))
        return;
      this.ResourceSourceGroup = MyStringHash.GetOrCompute(producerDefinition.ResourceSourceGroup);
      this.MaxPowerOutput = producerDefinition.MaxPowerOutput;
    }

    private class Sandbox_Definitions_MyPowerProducerDefinition\u003C\u003EActor : IActivator, IActivator<MyPowerProducerDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyPowerProducerDefinition();

      MyPowerProducerDefinition IActivator<MyPowerProducerDefinition>.CreateInstance() => new MyPowerProducerDefinition();
    }
  }
}
