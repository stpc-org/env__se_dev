// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyOffensiveWordsDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_OffensiveWords), null)]
  public class MyOffensiveWordsDefinition : MyDefinitionBase
  {
    public List<string> Blacklist = new List<string>();
    public List<string> Whitelist = new List<string>();

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_OffensiveWords builderOffensiveWords = (MyObjectBuilder_OffensiveWords) builder;
      if (builderOffensiveWords.Blacklist != null)
        this.Blacklist = ((IEnumerable<string>) builderOffensiveWords.Blacklist).ToList<string>();
      if (builderOffensiveWords.Whitelist == null)
        return;
      this.Whitelist = ((IEnumerable<string>) builderOffensiveWords.Whitelist).ToList<string>();
    }

    private class Sandbox_Definitions_MyOffensiveWordsDefinition\u003C\u003EActor : IActivator, IActivator<MyOffensiveWordsDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyOffensiveWordsDefinition();

      MyOffensiveWordsDefinition IActivator<MyOffensiveWordsDefinition>.CreateInstance() => new MyOffensiveWordsDefinition();
    }
  }
}
