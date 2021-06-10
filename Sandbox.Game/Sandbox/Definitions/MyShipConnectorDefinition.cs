// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyShipConnectorDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ShipConnectorDefinition), null)]
  public class MyShipConnectorDefinition : MyCubeBlockDefinition
  {
    public float AutoUnlockTime_Min;
    public float AutoUnlockTime_Max;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      if (!(builder is MyObjectBuilder_ShipConnectorDefinition connectorDefinition))
        return;
      this.AutoUnlockTime_Min = connectorDefinition.AutoUnlockTime_Min;
      this.AutoUnlockTime_Max = connectorDefinition.AutoUnlockTime_Max;
    }

    private class Sandbox_Definitions_MyShipConnectorDefinition\u003C\u003EActor : IActivator, IActivator<MyShipConnectorDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyShipConnectorDefinition();

      MyShipConnectorDefinition IActivator<MyShipConnectorDefinition>.CreateInstance() => new MyShipConnectorDefinition();
    }
  }
}
