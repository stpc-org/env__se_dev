// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyAssemblerDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_AssemblerDefinition), null)]
  public class MyAssemblerDefinition : MyProductionBlockDefinition
  {
    private float m_assemblySpeed;

    public float AssemblySpeed => this.m_assemblySpeed;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      this.m_assemblySpeed = (builder as MyObjectBuilder_AssemblerDefinition).AssemblySpeed;
    }

    protected override void InitializeLegacyBlueprintClasses(
      MyObjectBuilder_ProductionBlockDefinition ob)
    {
      ob.BlueprintClasses = new string[4]
      {
        "LargeBlocks",
        "SmallBlocks",
        "Components",
        "Tools"
      };
    }

    private class Sandbox_Definitions_MyAssemblerDefinition\u003C\u003EActor : IActivator, IActivator<MyAssemblerDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyAssemblerDefinition();

      MyAssemblerDefinition IActivator<MyAssemblerDefinition>.CreateInstance() => new MyAssemblerDefinition();
    }
  }
}
