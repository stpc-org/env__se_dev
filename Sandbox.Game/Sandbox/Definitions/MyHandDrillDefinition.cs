// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyHandDrillDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_HandDrillDefinition), null)]
  public class MyHandDrillDefinition : MyEngineerToolBaseDefinition
  {
    public float HarvestRatioMultiplier;
    public Vector3D ParticleOffset;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_HandDrillDefinition handDrillDefinition = builder as MyObjectBuilder_HandDrillDefinition;
      this.HarvestRatioMultiplier = handDrillDefinition.HarvestRatioMultiplier;
      Vector3D particleOffset = handDrillDefinition.ParticleOffset;
      this.ParticleOffset = handDrillDefinition.ParticleOffset;
    }

    public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
    {
      MyObjectBuilder_HandDrillDefinition objectBuilder = (MyObjectBuilder_HandDrillDefinition) base.GetObjectBuilder();
      objectBuilder.HarvestRatioMultiplier = this.HarvestRatioMultiplier;
      objectBuilder.ParticleOffset = this.ParticleOffset;
      return (MyObjectBuilder_DefinitionBase) objectBuilder;
    }

    private class Sandbox_Definitions_MyHandDrillDefinition\u003C\u003EActor : IActivator, IActivator<MyHandDrillDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyHandDrillDefinition();

      MyHandDrillDefinition IActivator<MyHandDrillDefinition>.CreateInstance() => new MyHandDrillDefinition();
    }
  }
}
