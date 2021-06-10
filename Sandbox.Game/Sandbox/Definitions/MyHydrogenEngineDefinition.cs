// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyHydrogenEngineDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_HydrogenEngineDefinition), null)]
  public class MyHydrogenEngineDefinition : MyGasFueledPowerProducerDefinition
  {
    public float AnimationSpeed;
    public float PistonAnimationMin;
    public float PistonAnimationMax;
    public float AnimationSpinUpSpeed;
    public float AnimationSpinDownSpeed;
    public float[] PistonAnimationOffsets;
    public float AnimationVisibilityDistanceSq;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_HydrogenEngineDefinition engineDefinition = (MyObjectBuilder_HydrogenEngineDefinition) builder;
      this.AnimationSpeed = engineDefinition.AnimationSpeed;
      this.PistonAnimationMin = engineDefinition.PistonAnimationMin;
      this.PistonAnimationMax = engineDefinition.PistonAnimationMax;
      this.AnimationSpinUpSpeed = engineDefinition.AnimationSpinUpSpeed;
      this.AnimationSpinDownSpeed = engineDefinition.AnimationSpinDownSpeed;
      this.PistonAnimationOffsets = engineDefinition.PistonAnimationOffsets;
      this.AnimationVisibilityDistanceSq = engineDefinition.AnimationVisibilityDistance * engineDefinition.AnimationVisibilityDistance;
    }

    private class Sandbox_Definitions_MyHydrogenEngineDefinition\u003C\u003EActor : IActivator, IActivator<MyHydrogenEngineDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyHydrogenEngineDefinition();

      MyHydrogenEngineDefinition IActivator<MyHydrogenEngineDefinition>.CreateInstance() => new MyHydrogenEngineDefinition();
    }
  }
}
