// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyAirVentDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game.Entities;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_AirVentDefinition), null)]
  public class MyAirVentDefinition : MyCubeBlockDefinition
  {
    public MyStringHash ResourceSinkGroup;
    public MyStringHash ResourceSourceGroup;
    public float StandbyPowerConsumption;
    public float OperationalPowerConsumption;
    public float VentilationCapacityPerSecond;
    public MySoundPair PressurizeSound;
    public MySoundPair DepressurizeSound;
    public MySoundPair IdleSound;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_AirVentDefinition airVentDefinition = builder as MyObjectBuilder_AirVentDefinition;
      this.ResourceSinkGroup = MyStringHash.GetOrCompute(airVentDefinition.ResourceSinkGroup);
      this.ResourceSourceGroup = MyStringHash.GetOrCompute(airVentDefinition.ResourceSourceGroup);
      this.StandbyPowerConsumption = airVentDefinition.StandbyPowerConsumption;
      this.OperationalPowerConsumption = airVentDefinition.OperationalPowerConsumption;
      this.VentilationCapacityPerSecond = airVentDefinition.VentilationCapacityPerSecond;
      this.PressurizeSound = new MySoundPair(airVentDefinition.PressurizeSound);
      this.DepressurizeSound = new MySoundPair(airVentDefinition.DepressurizeSound);
      this.IdleSound = new MySoundPair(airVentDefinition.IdleSound);
    }

    private class Sandbox_Definitions_MyAirVentDefinition\u003C\u003EActor : IActivator, IActivator<MyAirVentDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyAirVentDefinition();

      MyAirVentDefinition IActivator<MyAirVentDefinition>.CreateInstance() => new MyAirVentDefinition();
    }
  }
}
