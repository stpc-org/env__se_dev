// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyGasProperties
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_GasProperties), null)]
  public class MyGasProperties : MyDefinitionBase
  {
    public float EnergyDensity;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      this.EnergyDensity = (builder as MyObjectBuilder_GasProperties).EnergyDensity;
    }

    public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
    {
      MyObjectBuilder_GasProperties objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_GasProperties;
      objectBuilder.EnergyDensity = this.EnergyDensity;
      return (MyObjectBuilder_DefinitionBase) objectBuilder;
    }

    private class Sandbox_Definitions_MyGasProperties\u003C\u003EActor : IActivator, IActivator<MyGasProperties>
    {
      object IActivator.CreateInstance() => (object) new MyGasProperties();

      MyGasProperties IActivator<MyGasProperties>.CreateInstance() => new MyGasProperties();
    }
  }
}
