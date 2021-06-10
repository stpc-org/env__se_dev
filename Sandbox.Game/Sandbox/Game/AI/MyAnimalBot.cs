// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.MyAnimalBot
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage.Game.ObjectBuilders.AI.Bot;

namespace Sandbox.Game.AI
{
  [MyBotType(typeof (MyObjectBuilder_AnimalBot))]
  public class MyAnimalBot : MyAgentBot
  {
    public MyCharacter AnimalEntity => this.AgentEntity;

    public MyAnimalBotDefinition AnimalDefinition => this.m_botDefinition as MyAnimalBotDefinition;

    public MyAnimalBot(MyPlayer player, MyBotDefinition botDefinition)
      : base(player, botDefinition)
    {
    }
  }
}
