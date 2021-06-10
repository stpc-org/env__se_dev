// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyResearchBlockDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ResearchBlockDefinition), null)]
  public class MyResearchBlockDefinition : MyDefinitionBase
  {
    public string[] UnlockedByGroups;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ResearchBlockDefinition researchBlockDefinition = builder as MyObjectBuilder_ResearchBlockDefinition;
      if (researchBlockDefinition.UnlockedByGroups == null)
        return;
      this.UnlockedByGroups = new string[researchBlockDefinition.UnlockedByGroups.Length];
      for (int index = 0; index < researchBlockDefinition.UnlockedByGroups.Length; ++index)
        this.UnlockedByGroups[index] = researchBlockDefinition.UnlockedByGroups[index];
    }

    private class Sandbox_Definitions_MyResearchBlockDefinition\u003C\u003EActor : IActivator, IActivator<MyResearchBlockDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyResearchBlockDefinition();

      MyResearchBlockDefinition IActivator<MyResearchBlockDefinition>.CreateInstance() => new MyResearchBlockDefinition();
    }
  }
}
