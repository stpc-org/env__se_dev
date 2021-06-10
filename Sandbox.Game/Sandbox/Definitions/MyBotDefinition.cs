// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyBotDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Character;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_BotDefinition), null)]
  public class MyBotDefinition : MyDefinitionBase
  {
    public MyDefinitionId BotBehaviorTree;
    public string BehaviorType;
    public string BehaviorSubtype;
    public MyDefinitionId TypeDefinitionId;
    public bool Commandable;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_BotDefinition builderBotDefinition = builder as MyObjectBuilder_BotDefinition;
      this.BotBehaviorTree = new MyDefinitionId(builderBotDefinition.BotBehaviorTree.Type, builderBotDefinition.BotBehaviorTree.Subtype);
      this.BehaviorType = builderBotDefinition.BehaviorType;
      this.TypeDefinitionId = new MyDefinitionId(builderBotDefinition.TypeId, builderBotDefinition.SubtypeName);
      this.BehaviorSubtype = !string.IsNullOrWhiteSpace(builderBotDefinition.BehaviorSubtype) ? builderBotDefinition.BehaviorSubtype : builderBotDefinition.BehaviorType;
      this.Commandable = builderBotDefinition.Commandable;
    }

    public virtual void AddItems(MyCharacter character)
    {
    }

    private class Sandbox_Definitions_MyBotDefinition\u003C\u003EActor : IActivator, IActivator<MyBotDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyBotDefinition();

      MyBotDefinition IActivator<MyBotDefinition>.CreateInstance() => new MyBotDefinition();
    }
  }
}
