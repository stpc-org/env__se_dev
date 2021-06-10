// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MySolarPanelDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_SolarPanelDefinition), null)]
  public class MySolarPanelDefinition : MyPowerProducerDefinition
  {
    public Vector3 PanelOrientation;
    public bool IsTwoSided;
    public float PanelOffset;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_SolarPanelDefinition solarPanelDefinition = builder as MyObjectBuilder_SolarPanelDefinition;
      this.PanelOrientation = solarPanelDefinition.PanelOrientation;
      this.IsTwoSided = solarPanelDefinition.TwoSidedPanel;
      this.PanelOffset = solarPanelDefinition.PanelOffset;
    }

    private class Sandbox_Definitions_MySolarPanelDefinition\u003C\u003EActor : IActivator, IActivator<MySolarPanelDefinition>
    {
      object IActivator.CreateInstance() => (object) new MySolarPanelDefinition();

      MySolarPanelDefinition IActivator<MySolarPanelDefinition>.CreateInstance() => new MySolarPanelDefinition();
    }
  }
}
