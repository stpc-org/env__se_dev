// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Commands.MyAiCommandBehavior
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.AI.Commands
{
  public class MyAiCommandBehavior : IMyAiCommand
  {
    private static readonly List<MyPhysics.HitInfo> m_tmpHitInfos = new List<MyPhysics.HitInfo>();

    public MyAiCommandBehaviorDefinition Definition { get; private set; }

    public void InitCommand(MyAiCommandDefinition definition) => this.Definition = definition as MyAiCommandBehaviorDefinition;

    public void ActivateCommand()
    {
      if (this.Definition.CommandEffect == MyAiCommandEffect.TARGET)
      {
        this.ChangeTarget();
      }
      else
      {
        if (this.Definition.CommandEffect != MyAiCommandEffect.OWNED_BOTS)
          return;
        this.ChangeAllBehaviors();
      }
    }

    private void ChangeTarget()
    {
      Vector3D from;
      Vector3D forward;
      if (MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.ThirdPersonSpectator || MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Entity)
      {
        MatrixD headMatrix = MySession.Static.ControlledEntity.GetHeadMatrix(true);
        from = headMatrix.Translation;
        forward = headMatrix.Forward;
      }
      else
      {
        from = MySector.MainCamera.Position;
        forward = MySector.MainCamera.WorldMatrix.Forward;
      }
      MyAiCommandBehavior.m_tmpHitInfos.Clear();
      MyPhysics.CastRay(from, from + forward * 20.0, MyAiCommandBehavior.m_tmpHitInfos, 24);
      if (MyAiCommandBehavior.m_tmpHitInfos.Count == 0)
        return;
      foreach (MyPhysics.HitInfo tmpHitInfo in MyAiCommandBehavior.m_tmpHitInfos)
      {
        MyAgentBot bot;
        if (tmpHitInfo.HkHitInfo.GetHitEntity() is MyCharacter hitEntity && MyAiCommandBehavior.TryGetBotForCharacter(hitEntity, out bot) && bot.BotDefinition.Commandable)
          this.ChangeBotBehavior(bot);
      }
    }

    private void ChangeAllBehaviors()
    {
      foreach (KeyValuePair<int, IMyBot> allBot in MyAIComponent.Static.Bots.GetAllBots())
      {
        if (allBot.Value is MyAgentBot bot && bot.BotDefinition.Commandable)
          this.ChangeBotBehavior(bot);
      }
    }

    private static bool TryGetBotForCharacter(MyCharacter character, out MyAgentBot bot)
    {
      bot = (MyAgentBot) null;
      foreach (KeyValuePair<int, IMyBot> allBot in MyAIComponent.Static.Bots.GetAllBots())
      {
        if (allBot.Value is MyAgentBot myAgentBot && myAgentBot.AgentEntity == character)
        {
          bot = myAgentBot;
          return true;
        }
      }
      return false;
    }

    private void ChangeBotBehavior(MyAgentBot bot) => MyAIComponent.Static.BehaviorTrees.ChangeBehaviorTree(this.Definition.BehaviorTreeName, (IMyBot) bot);
  }
}
