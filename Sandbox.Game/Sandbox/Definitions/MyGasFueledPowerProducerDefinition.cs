// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyGasFueledPowerProducerDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_GasFueledPowerProducerDefinition), null)]
  public class MyGasFueledPowerProducerDefinition : MyFueledPowerProducerDefinition
  {
    public MyGasFueledPowerProducerDefinition.FuelInfo Fuel;
    public float FuelCapacity;
    public MyStringHash ResourceSinkGroup;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      MyObjectBuilder_GasFueledPowerProducerDefinition producerDefinition = (MyObjectBuilder_GasFueledPowerProducerDefinition) builder;
      base.Init(builder);
      this.FuelCapacity = producerDefinition.FuelCapacity;
      this.Fuel = new MyGasFueledPowerProducerDefinition.FuelInfo(producerDefinition.FuelInfos[0]);
      this.ResourceSinkGroup = MyStringHash.GetOrCompute(producerDefinition.ResourceSinkGroup);
    }

    public struct FuelInfo
    {
      public readonly float Ratio;
      public readonly MyDefinitionId FuelId;

      public FuelInfo(
        MyObjectBuilder_FueledPowerProducerDefinition.FuelInfo fuelInfo)
      {
        this.FuelId = (MyDefinitionId) fuelInfo.Id;
        this.Ratio = fuelInfo.Ratio;
      }
    }

    private class Sandbox_Definitions_MyGasFueledPowerProducerDefinition\u003C\u003EActor : IActivator, IActivator<MyGasFueledPowerProducerDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyGasFueledPowerProducerDefinition();

      MyGasFueledPowerProducerDefinition IActivator<MyGasFueledPowerProducerDefinition>.CreateInstance() => new MyGasFueledPowerProducerDefinition();
    }
  }
}
