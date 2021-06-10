// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Logic.MyBotLogic
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;

namespace Sandbox.Game.AI.Logic
{
  public abstract class MyBotLogic
  {
    protected IMyBot m_bot;

    public abstract BotType BotType { get; }

    protected MyBotLogic(IMyBot bot) => this.m_bot = bot;

    public virtual void Init()
    {
    }

    public virtual void Cleanup()
    {
    }

    public virtual void Update()
    {
    }

    public virtual void OnControlledEntityChanged(IMyControllableEntity newEntity)
    {
    }

    public virtual void DebugDraw()
    {
    }
  }
}
