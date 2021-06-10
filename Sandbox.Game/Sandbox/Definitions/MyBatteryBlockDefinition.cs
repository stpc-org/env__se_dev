// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyBatteryBlockDefinition
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
  [MyDefinitionType(typeof (MyObjectBuilder_BatteryBlockDefinition), null)]
  public class MyBatteryBlockDefinition : MyPowerProducerDefinition
  {
    public float MaxStoredPower;
    public float InitialStoredPowerRatio;
    public MyStringHash ResourceSinkGroup;
    public float RequiredPowerInput;
    public bool AdaptibleInput;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      if (!(builder is MyObjectBuilder_BatteryBlockDefinition batteryBlockDefinition))
        return;
      this.MaxStoredPower = batteryBlockDefinition.MaxStoredPower;
      this.InitialStoredPowerRatio = batteryBlockDefinition.InitialStoredPowerRatio;
      this.ResourceSinkGroup = MyStringHash.GetOrCompute(batteryBlockDefinition.ResourceSinkGroup);
      this.RequiredPowerInput = batteryBlockDefinition.RequiredPowerInput;
      this.AdaptibleInput = batteryBlockDefinition.AdaptibleInput;
    }

    private class Sandbox_Definitions_MyBatteryBlockDefinition\u003C\u003EActor : IActivator, IActivator<MyBatteryBlockDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyBatteryBlockDefinition();

      MyBatteryBlockDefinition IActivator<MyBatteryBlockDefinition>.CreateInstance() => new MyBatteryBlockDefinition();
    }
  }
}
