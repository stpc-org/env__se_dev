// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyShipToolDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ShipToolDefinition), null)]
  public class MyShipToolDefinition : MyCubeBlockDefinition
  {
    public string Flare;
    public float SensorRadius;
    public float SensorOffset;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ShipToolDefinition shipToolDefinition = builder as MyObjectBuilder_ShipToolDefinition;
      this.SensorRadius = shipToolDefinition.SensorRadius;
      this.SensorOffset = shipToolDefinition.SensorOffset;
      this.Flare = shipToolDefinition.Flare;
    }

    private class Sandbox_Definitions_MyShipToolDefinition\u003C\u003EActor : IActivator, IActivator<MyShipToolDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyShipToolDefinition();

      MyShipToolDefinition IActivator<MyShipToolDefinition>.CreateInstance() => new MyShipToolDefinition();
    }
  }
}
