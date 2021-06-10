// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyGravityGeneratorBaseDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_GravityGeneratorBaseDefinition), null)]
  public class MyGravityGeneratorBaseDefinition : MyCubeBlockDefinition
  {
    public MyStringHash ResourceSinkGroup;
    public float MinGravityAcceleration;
    public float MaxGravityAcceleration;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_GravityGeneratorBaseDefinition generatorBaseDefinition = builder as MyObjectBuilder_GravityGeneratorBaseDefinition;
      this.ResourceSinkGroup = MyStringHash.GetOrCompute(generatorBaseDefinition.ResourceSinkGroup);
      this.MinGravityAcceleration = generatorBaseDefinition.MinGravityAcceleration;
      this.MaxGravityAcceleration = generatorBaseDefinition.MaxGravityAcceleration;
    }

    private class Sandbox_Definitions_MyGravityGeneratorBaseDefinition\u003C\u003EActor : IActivator, IActivator<MyGravityGeneratorBaseDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyGravityGeneratorBaseDefinition();

      MyGravityGeneratorBaseDefinition IActivator<MyGravityGeneratorBaseDefinition>.CreateInstance() => new MyGravityGeneratorBaseDefinition();
    }
  }
}
