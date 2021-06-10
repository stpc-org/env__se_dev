// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MySessionComponentResearchDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components.Session;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_SessionComponentResearchDefinition), null)]
  public class MySessionComponentResearchDefinition : MySessionComponentDefinition
  {
    public bool WhitelistMode;
    public List<MyDefinitionId> Researches = new List<MyDefinitionId>();

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_SessionComponentResearchDefinition researchDefinition = builder as MyObjectBuilder_SessionComponentResearchDefinition;
      this.WhitelistMode = researchDefinition.WhitelistMode;
      if (researchDefinition.Researches == null)
        return;
      foreach (SerializableDefinitionId research in researchDefinition.Researches)
        this.Researches.Add((MyDefinitionId) research);
    }

    private class Sandbox_Definitions_MySessionComponentResearchDefinition\u003C\u003EActor : IActivator, IActivator<MySessionComponentResearchDefinition>
    {
      object IActivator.CreateInstance() => (object) new MySessionComponentResearchDefinition();

      MySessionComponentResearchDefinition IActivator<MySessionComponentResearchDefinition>.CreateInstance() => new MySessionComponentResearchDefinition();
    }
  }
}
