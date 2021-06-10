// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyResearchDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ResearchDefinition), null)]
  public class MyResearchDefinition : MyDefinitionBase
  {
    public List<MyDefinitionId> Entries;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ResearchDefinition researchDefinition = builder as MyObjectBuilder_ResearchDefinition;
      this.Entries = new List<MyDefinitionId>();
      foreach (SerializableDefinitionId entry in researchDefinition.Entries)
        this.Entries.Add((MyDefinitionId) entry);
    }

    private class Sandbox_Definitions_MyResearchDefinition\u003C\u003EActor : IActivator, IActivator<MyResearchDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyResearchDefinition();

      MyResearchDefinition IActivator<MyResearchDefinition>.CreateInstance() => new MyResearchDefinition();
    }
  }
}
