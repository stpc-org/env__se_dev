// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyChatBotResponseDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ChatBotResponseDefinition), null)]
  public class MyChatBotResponseDefinition : MyDefinitionBase
  {
    public string Response;
    public string[] Questions;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ChatBotResponseDefinition responseDefinition = (MyObjectBuilder_ChatBotResponseDefinition) builder;
      this.Response = responseDefinition.Response;
      this.Questions = responseDefinition.Questions;
    }

    private class Sandbox_Definitions_MyChatBotResponseDefinition\u003C\u003EActor : IActivator, IActivator<MyChatBotResponseDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyChatBotResponseDefinition();

      MyChatBotResponseDefinition IActivator<MyChatBotResponseDefinition>.CreateInstance() => new MyChatBotResponseDefinition();
    }
  }
}
