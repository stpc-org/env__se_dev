// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Logic.MyAgentLogic
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Character;

namespace Sandbox.Game.AI.Logic
{
  public abstract class MyAgentLogic : MyBotLogic
  {
    protected IMyEntityBot m_entityBot;

    public MyAgentBot AgentBot => this.m_bot as MyAgentBot;

    public MyAiTargetBase AiTarget { get; private set; }

    public override BotType BotType => BotType.UNKNOWN;

    protected MyAgentLogic(IMyBot bot)
      : base(bot)
    {
      this.m_entityBot = this.m_bot as IMyEntityBot;
      this.AiTarget = MyAIComponent.BotFactory.CreateTargetForBot(this.AgentBot);
    }

    public override void Init()
    {
      base.Init();
      this.AiTarget = this.AgentBot.AgentActions.AiTargetBase;
    }

    public override void Cleanup()
    {
      base.Cleanup();
      this.AiTarget.Cleanup();
    }

    public override void Update()
    {
      base.Update();
      this.AiTarget.Update();
    }

    public virtual void OnCharacterControlAcquired(MyCharacter character)
    {
    }
  }
}
