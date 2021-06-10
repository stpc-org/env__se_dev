// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyResearchGroupDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ResearchGroupDefinition), null)]
  public class MyResearchGroupDefinition : MyDefinitionBase
  {
    public SerializableDefinitionId[] Members;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ResearchGroupDefinition researchGroupDefinition = builder as MyObjectBuilder_ResearchGroupDefinition;
      if (researchGroupDefinition.Members == null)
        return;
      this.Members = new SerializableDefinitionId[researchGroupDefinition.Members.Length];
      for (int index = 0; index < researchGroupDefinition.Members.Length; ++index)
        this.Members[index] = researchGroupDefinition.Members[index];
    }

    private class Sandbox_Definitions_MyResearchGroupDefinition\u003C\u003EActor : IActivator, IActivator<MyResearchGroupDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyResearchGroupDefinition();

      MyResearchGroupDefinition IActivator<MyResearchGroupDefinition>.CreateInstance() => new MyResearchGroupDefinition();
    }
  }
}
