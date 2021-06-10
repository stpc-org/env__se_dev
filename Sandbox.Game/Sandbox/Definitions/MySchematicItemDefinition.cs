// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MySchematicItemDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_SchematicItemDefinition), null)]
  public class MySchematicItemDefinition : MyUsableItemDefinition
  {
    public MyDefinitionId Research;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      this.Research = (MyDefinitionId) (builder as MyObjectBuilder_SchematicItemDefinition).Research.Value;
    }

    private class Sandbox_Definitions_MySchematicItemDefinition\u003C\u003EActor : IActivator, IActivator<MySchematicItemDefinition>
    {
      object IActivator.CreateInstance() => (object) new MySchematicItemDefinition();

      MySchematicItemDefinition IActivator<MySchematicItemDefinition>.CreateInstance() => new MySchematicItemDefinition();
    }
  }
}
