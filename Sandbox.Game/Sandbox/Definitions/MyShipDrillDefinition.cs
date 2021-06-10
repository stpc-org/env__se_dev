// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyShipDrillDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ShipDrillDefinition), null)]
  public class MyShipDrillDefinition : MyShipToolDefinition
  {
    public MyStringHash ResourceSinkGroup;
    public float CutOutOffset;
    public float CutOutRadius;
    public Vector3D ParticleOffset;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ShipDrillDefinition shipDrillDefinition = builder as MyObjectBuilder_ShipDrillDefinition;
      this.ResourceSinkGroup = MyStringHash.GetOrCompute(shipDrillDefinition.ResourceSinkGroup);
      this.CutOutOffset = shipDrillDefinition.CutOutOffset;
      this.CutOutRadius = shipDrillDefinition.CutOutRadius;
      this.ParticleOffset = shipDrillDefinition.ParticleOffset;
    }

    private class Sandbox_Definitions_MyShipDrillDefinition\u003C\u003EActor : IActivator, IActivator<MyShipDrillDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyShipDrillDefinition();

      MyShipDrillDefinition IActivator<MyShipDrillDefinition>.CreateInstance() => new MyShipDrillDefinition();
    }
  }
}
