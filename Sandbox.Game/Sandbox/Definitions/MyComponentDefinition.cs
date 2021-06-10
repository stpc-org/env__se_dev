// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyComponentDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ComponentDefinition), null)]
  public class MyComponentDefinition : MyPhysicalItemDefinition
  {
    public int MaxIntegrity;
    public float DropProbability;
    public float DeconstructionEfficiency;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ComponentDefinition componentDefinition = builder as MyObjectBuilder_ComponentDefinition;
      this.MaxIntegrity = componentDefinition.MaxIntegrity;
      this.DropProbability = componentDefinition.DropProbability;
      this.DeconstructionEfficiency = componentDefinition.DeconstructionEfficiency;
    }

    private class Sandbox_Definitions_MyComponentDefinition\u003C\u003EActor : IActivator, IActivator<MyComponentDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyComponentDefinition();

      MyComponentDefinition IActivator<MyComponentDefinition>.CreateInstance() => new MyComponentDefinition();
    }
  }
}
