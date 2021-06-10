// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MySpaceBallDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_SpaceBallDefinition), null)]
  public class MySpaceBallDefinition : MyCubeBlockDefinition
  {
    public float MaxVirtualMass;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      this.MaxVirtualMass = (builder as MyObjectBuilder_SpaceBallDefinition).MaxVirtualMass;
    }

    private class Sandbox_Definitions_MySpaceBallDefinition\u003C\u003EActor : IActivator, IActivator<MySpaceBallDefinition>
    {
      object IActivator.CreateInstance() => (object) new MySpaceBallDefinition();

      MySpaceBallDefinition IActivator<MySpaceBallDefinition>.CreateInstance() => new MySpaceBallDefinition();
    }
  }
}
