// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyGravityGeneratorSphereDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_GravityGeneratorSphereDefinition), null)]
  public class MyGravityGeneratorSphereDefinition : MyGravityGeneratorBaseDefinition
  {
    public float MinRadius;
    public float MaxRadius;
    public float BasePowerInput;
    public float ConsumptionPower;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_GravityGeneratorSphereDefinition sphereDefinition = builder as MyObjectBuilder_GravityGeneratorSphereDefinition;
      this.MinRadius = sphereDefinition.MinRadius;
      this.MaxRadius = sphereDefinition.MaxRadius;
      this.BasePowerInput = sphereDefinition.BasePowerInput;
      this.ConsumptionPower = sphereDefinition.ConsumptionPower;
    }

    private class Sandbox_Definitions_MyGravityGeneratorSphereDefinition\u003C\u003EActor : IActivator, IActivator<MyGravityGeneratorSphereDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyGravityGeneratorSphereDefinition();

      MyGravityGeneratorSphereDefinition IActivator<MyGravityGeneratorSphereDefinition>.CreateInstance() => new MyGravityGeneratorSphereDefinition();
    }
  }
}
