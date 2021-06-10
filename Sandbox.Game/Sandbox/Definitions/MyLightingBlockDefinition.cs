// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyLightingBlockDefinition
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
  [MyDefinitionType(typeof (MyObjectBuilder_LightingBlockDefinition), null)]
  public class MyLightingBlockDefinition : MyCubeBlockDefinition
  {
    public MyBounds LightRadius;
    public MyBounds LightReflectorRadius;
    public MyBounds LightFalloff;
    public MyBounds LightIntensity;
    public MyBounds LightOffset;
    public MyBounds BlinkIntervalSeconds;
    public MyBounds BlinkLenght;
    public MyBounds BlinkOffset;
    public MyStringHash ResourceSinkGroup;
    public float RequiredPowerInput;
    public string Flare;
    public float ReflectorConeDegrees;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_LightingBlockDefinition lightingBlockDefinition = (MyObjectBuilder_LightingBlockDefinition) builder;
      this.BlinkIntervalSeconds = (MyBounds) lightingBlockDefinition.LightBlinkIntervalSeconds;
      this.BlinkLenght = (MyBounds) lightingBlockDefinition.LightBlinkLenght;
      this.BlinkOffset = (MyBounds) lightingBlockDefinition.LightBlinkOffset;
      this.LightRadius = (MyBounds) lightingBlockDefinition.LightRadius;
      this.LightReflectorRadius = (MyBounds) lightingBlockDefinition.LightReflectorRadius;
      this.LightFalloff = (MyBounds) lightingBlockDefinition.LightFalloff;
      this.LightIntensity = (MyBounds) lightingBlockDefinition.LightIntensity;
      this.LightOffset = (MyBounds) lightingBlockDefinition.LightOffset;
      this.ResourceSinkGroup = MyStringHash.GetOrCompute(lightingBlockDefinition.ResourceSinkGroup);
      this.RequiredPowerInput = lightingBlockDefinition.RequiredPowerInput;
      this.Flare = lightingBlockDefinition.Flare;
    }

    private class Sandbox_Definitions_MyLightingBlockDefinition\u003C\u003EActor : IActivator, IActivator<MyLightingBlockDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyLightingBlockDefinition();

      MyLightingBlockDefinition IActivator<MyLightingBlockDefinition>.CreateInstance() => new MyLightingBlockDefinition();
    }
  }
}
