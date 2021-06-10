// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyWindTurbineDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_WindTurbineDefinition), null)]
  public class MyWindTurbineDefinition : MyPowerProducerDefinition
  {
    public int RaycasterSize;
    public int RaycastersCount;
    public float MinRaycasterClearance;
    public float OptimalGroundClearance;
    public float RaycastersToFullEfficiency;
    public float OptimalWindSpeed;
    public float TurbineSpinUpSpeed;
    public float TurbineSpinDownSpeed;
    public float TurbineRotationSpeed;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_WindTurbineDefinition turbineDefinition = (MyObjectBuilder_WindTurbineDefinition) builder;
      this.RaycasterSize = turbineDefinition.RaycasterSize;
      this.RaycastersCount = turbineDefinition.RaycastersCount;
      this.MinRaycasterClearance = turbineDefinition.MinRaycasterClearance;
      this.RaycastersToFullEfficiency = turbineDefinition.RaycastersToFullEfficiency;
      this.OptimalWindSpeed = turbineDefinition.OptimalWindSpeed;
      this.TurbineSpinUpSpeed = turbineDefinition.TurbineSpinUpSpeed;
      this.TurbineSpinDownSpeed = turbineDefinition.TurbineSpinDownSpeed;
      this.TurbineRotationSpeed = turbineDefinition.TurbineRotationSpeed;
      this.OptimalGroundClearance = turbineDefinition.OptimalGroundClearance;
    }

    private class Sandbox_Definitions_MyWindTurbineDefinition\u003C\u003EActor : IActivator, IActivator<MyWindTurbineDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyWindTurbineDefinition();

      MyWindTurbineDefinition IActivator<MyWindTurbineDefinition>.CreateInstance() => new MyWindTurbineDefinition();
    }
  }
}
