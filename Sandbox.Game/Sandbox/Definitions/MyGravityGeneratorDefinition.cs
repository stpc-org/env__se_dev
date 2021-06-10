// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyGravityGeneratorDefinition
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
  [MyDefinitionType(typeof (MyObjectBuilder_GravityGeneratorDefinition), null)]
  public class MyGravityGeneratorDefinition : MyGravityGeneratorBaseDefinition
  {
    public float RequiredPowerInput;
    public Vector3 MinFieldSize;
    public Vector3 MaxFieldSize;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_GravityGeneratorDefinition generatorDefinition = builder as MyObjectBuilder_GravityGeneratorDefinition;
      this.RequiredPowerInput = generatorDefinition.RequiredPowerInput;
      this.MinFieldSize = (Vector3) generatorDefinition.MinFieldSize;
      this.MaxFieldSize = (Vector3) generatorDefinition.MaxFieldSize;
    }

    private class Sandbox_Definitions_MyGravityGeneratorDefinition\u003C\u003EActor : IActivator, IActivator<MyGravityGeneratorDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyGravityGeneratorDefinition();

      MyGravityGeneratorDefinition IActivator<MyGravityGeneratorDefinition>.CreateInstance() => new MyGravityGeneratorDefinition();
    }
  }
}
