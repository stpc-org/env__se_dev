// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyFactionNameDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_FactionNameDefinition), null)]
  public class MyFactionNameDefinition : MyDefinitionBase
  {
    public MyLanguagesEnum LanguageId;
    public MyFactionNameTypeEnum Type;
    public List<string> Names;
    public List<string> Tags;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      if (!(builder is MyObjectBuilder_FactionNameDefinition factionNameDefinition))
        return;
      this.LanguageId = factionNameDefinition.LanguageId;
      this.Type = factionNameDefinition.Type;
      this.Names = (List<string>) factionNameDefinition.Names;
      this.Tags = (List<string>) factionNameDefinition.Tags;
    }

    private class Sandbox_Definitions_MyFactionNameDefinition\u003C\u003EActor : IActivator, IActivator<MyFactionNameDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyFactionNameDefinition();

      MyFactionNameDefinition IActivator<MyFactionNameDefinition>.CreateInstance() => new MyFactionNameDefinition();
    }
  }
}
