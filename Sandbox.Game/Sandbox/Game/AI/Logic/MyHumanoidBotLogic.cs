// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Logic.MyHumanoidBotLogic
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

namespace Sandbox.Game.AI.Logic
{
  public abstract class MyHumanoidBotLogic : MyAgentLogic
  {
    public MyReservationStatus ReservationStatus;
    public MyAiTargetManager.ReservedEntityData ReservationEntityData;
    public MyAiTargetManager.ReservedAreaData ReservationAreaData;

    public MyHumanoidBot HumanoidBot => this.m_bot as MyHumanoidBot;

    protected MyHumanoidBotLogic(IMyBot bot)
      : base(bot)
    {
    }

    public override BotType BotType => BotType.HUMANOID;
  }
}
