// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.MyAgentBot
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.AI.Actions;
using Sandbox.Game.AI.Logic;
using Sandbox.Game.AI.Navigation;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI
{
  [MyBotType(typeof (MyObjectBuilder_AgentBot))]
  public class MyAgentBot : IMyEntityBot, IMyBot
  {
    protected MyPlayer m_player;
    protected MyBotNavigation m_navigation;
    protected ActionCollection m_actionCollection;
    protected MyBotMemory m_botMemory;
    protected MyAgentActions m_actions;
    protected MyAgentDefinition m_botDefinition;
    protected MyAgentLogic m_botLogic;
    private int m_deathCountdownMs;
    private int m_lastCountdownTime;
    private bool m_respawnRequestSent;
    private readonly bool m_removeAfterDeath;
    private bool m_botRemoved;
    private bool m_joinRequestSent;
    public MyAgentBot.MyLastActions LastActions = new MyAgentBot.MyLastActions();

    public MyPlayer Player => this.m_player;

    public MyBotNavigation Navigation => this.m_navigation;

    public MyCharacter AgentEntity => this.m_player.Controller.ControlledEntity as MyCharacter;

    public MyEntity BotEntity => (MyEntity) this.AgentEntity;

    public string BehaviorSubtypeName => MyAIComponent.Static.BehaviorTrees.GetBehaviorName((IMyBot) this);

    public ActionCollection ActionCollection => this.m_actionCollection;

    public MyBotMemory BotMemory => this.m_botMemory;

    public MyBotMemory LastBotMemory { get; set; }

    public void ReturnToLastMemory()
    {
      if (this.LastBotMemory == null)
        return;
      this.m_botMemory = this.LastBotMemory;
    }

    public MyAgentActions AgentActions => this.m_actions;

    public MyBotActionsBase BotActions
    {
      get => (MyBotActionsBase) this.m_actions;
      set => this.m_actions = value as MyAgentActions;
    }

    public MyBotDefinition BotDefinition => (MyBotDefinition) this.m_botDefinition;

    public MyAgentDefinition AgentDefinition => this.m_botDefinition;

    public MyBotLogic BotLogic => (MyBotLogic) this.m_botLogic;

    public MyAgentLogic AgentLogic => this.m_botLogic;

    public bool HasLogic => this.m_botLogic != null;

    public virtual bool IsValidForUpdate => this.m_player?.Controller.ControlledEntity?.Entity != null && this.AgentEntity != null && !this.AgentEntity.IsDead;

    public bool CreatedByPlayer { get; set; }

    public MyAgentBot(MyPlayer player, MyBotDefinition botDefinition)
    {
      this.m_player = player;
      this.m_navigation = new MyBotNavigation(player);
      this.m_actionCollection = (ActionCollection) null;
      this.m_botMemory = new MyBotMemory((IMyBot) this);
      this.m_botDefinition = botDefinition as MyAgentDefinition;
      this.m_removeAfterDeath = this.m_botDefinition.RemoveAfterDeath;
      this.m_respawnRequestSent = false;
      this.m_botRemoved = false;
      this.m_player.Controller.ControlledEntityChanged += new Action<IMyControllableEntity, IMyControllableEntity>(this.Controller_ControlledEntityChanged);
      this.m_navigation.ChangeEntity(this.m_player.Controller.ControlledEntity);
      MyCestmirDebugInputComponent.PlacedAction += new Action<Vector3D, MyEntity>(this.DebugGoto);
    }

    protected virtual void Controller_ControlledEntityChanged(
      IMyControllableEntity oldEntity,
      IMyControllableEntity newEntity)
    {
      if (oldEntity == null && newEntity is MyCharacter)
        this.EraseRespawn();
      this.m_navigation.ChangeEntity(newEntity);
      this.m_navigation.AimWithMovement();
      if (newEntity is MyCharacter myCharacter)
        myCharacter.JetpackComp?.TurnOnJetpack(false);
      if (!this.HasLogic)
        return;
      this.m_botLogic.OnControlledEntityChanged(newEntity);
    }

    public virtual void Init(MyObjectBuilder_Bot botBuilder)
    {
      if (!(botBuilder is MyObjectBuilder_AgentBot objectBuilderAgentBot))
        return;
      this.m_deathCountdownMs = objectBuilderAgentBot.RespawnCounter;
      if (this.AgentDefinition.FactionTag != null)
      {
        MyFaction factionByTag = MySession.Static.Factions.TryGetOrCreateFactionByTag(this.AgentDefinition.FactionTag);
        if (factionByTag != null)
        {
          MyFactionCollection.SendJoinRequest(factionByTag.FactionId, this.Player.Identity.IdentityId);
          this.m_joinRequestSent = true;
        }
      }
      if (objectBuilderAgentBot.AiTarget != null)
        this.AgentActions.AiTargetBase.Init(objectBuilderAgentBot.AiTarget);
      if (botBuilder.BotMemory != null)
        this.m_botMemory.Init(botBuilder.BotMemory);
      MyAIComponent.Static.BehaviorTrees.SetBehaviorName((IMyBot) this, objectBuilderAgentBot.LastBehaviorTree);
    }

    public virtual void InitActions(ActionCollection actionCollection) => this.m_actionCollection = actionCollection;

    public virtual void InitLogic(MyBotLogic botLogic)
    {
      this.m_botLogic = botLogic as MyAgentLogic;
      if (!this.HasLogic)
        return;
      this.m_botLogic.Init();
      if (this.AgentEntity == null)
        return;
      this.AgentLogic.OnCharacterControlAcquired(this.AgentEntity);
    }

    public virtual void Spawn(
      Vector3D? spawnPosition,
      Vector3? direction,
      Vector3? up,
      bool spawnedByPlayer)
    {
      this.CreatedByPlayer = spawnedByPlayer;
      if ((!(this.m_player.Controller.ControlledEntity is MyCharacter controlledEntity) || !controlledEntity.IsDead) && !this.m_player.Identity.IsDead || this.m_respawnRequestSent)
        return;
      this.m_respawnRequestSent = true;
      MyPlayerCollection.OnRespawnRequest(false, false, 0L, (string) null, spawnPosition, direction, up, new SerializableDefinitionId?((SerializableDefinitionId) this.BotDefinition.Id), false, this.m_player.Id.SerialId, (string) null, Color.Red);
    }

    public virtual void Cleanup()
    {
      MyCestmirDebugInputComponent.PlacedAction -= new Action<Vector3D, MyEntity>(this.DebugGoto);
      this.m_navigation.Cleanup();
      if (this.HasLogic)
        this.m_botLogic.Cleanup();
      this.m_player.Controller.ControlledEntityChanged -= new Action<IMyControllableEntity, IMyControllableEntity>(this.Controller_ControlledEntityChanged);
      this.m_player = (MyPlayer) null;
    }

    public void Update()
    {
      if (this.m_player.Controller.ControlledEntity != null)
      {
        if (this.AgentEntity != null && this.AgentEntity.IsDead && !this.m_respawnRequestSent)
        {
          this.HandleDeadBot();
        }
        else
        {
          if (this.AgentEntity != null && !this.AgentEntity.IsDead && this.m_respawnRequestSent)
            this.EraseRespawn();
          this.UpdateInternal();
        }
      }
      else
      {
        if (this.m_respawnRequestSent)
          return;
        this.HandleDeadBot();
      }
    }

    private void StartRespawn()
    {
      this.m_lastCountdownTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      if (this.m_removeAfterDeath)
        this.m_deathCountdownMs = this.AgentDefinition.RemoveTimeMs;
      else
        this.m_deathCountdownMs = this.AgentDefinition.RemoveTimeMs;
    }

    private void EraseRespawn()
    {
      this.m_deathCountdownMs = 0;
      this.m_respawnRequestSent = false;
    }

    protected virtual void UpdateInternal()
    {
      this.m_navigation.Update(this.m_botMemory.TickCounter);
      this.m_botLogic.Update();
      if (this.m_joinRequestSent || string.IsNullOrEmpty(this.m_botDefinition.FactionTag))
        return;
      MyFaction factionByTag = MySession.Static.Factions.TryGetFactionByTag(this.m_botDefinition.FactionTag.ToUpperInvariant(), (IMyFaction) null);
      if (factionByTag == null)
        return;
      long controllingIdentityId = this.AgentEntity.ControllerInfo.ControllingIdentityId;
      if (MySession.Static.Factions.TryGetPlayerFaction(controllingIdentityId) != null || this.m_joinRequestSent)
        return;
      MyFactionCollection.SendJoinRequest(factionByTag.FactionId, controllingIdentityId);
      this.m_joinRequestSent = true;
    }

    public virtual void Reset()
    {
      this.BotMemory.ResetMemory(true);
      this.m_navigation.StopImmediate(true);
      this.AgentActions.AiTargetBase.UnsetTarget();
    }

    public virtual MyObjectBuilder_Bot GetObjectBuilder()
    {
      MyObjectBuilder_AgentBot botObjectBuilder = MyAIComponent.BotFactory.GetBotObjectBuilder((IMyBot) this) as MyObjectBuilder_AgentBot;
      botObjectBuilder.BotDefId = (SerializableDefinitionId) this.BotDefinition.Id;
      botObjectBuilder.AiTarget = this.AgentActions.AiTargetBase.GetObjectBuilder();
      botObjectBuilder.BotMemory = this.m_botMemory.GetObjectBuilder();
      botObjectBuilder.LastBehaviorTree = this.BehaviorSubtypeName;
      botObjectBuilder.RemoveAfterDeath = this.m_removeAfterDeath;
      botObjectBuilder.RespawnCounter = this.m_deathCountdownMs;
      if (this.Player != null)
        botObjectBuilder.AsociatedMyPlayerId = this.Player.Id.SteamId;
      return (MyObjectBuilder_Bot) botObjectBuilder;
    }

    private void HandleDeadBot()
    {
      if (this.m_deathCountdownMs <= 0)
      {
        Vector3D spawnPosition;
        if (!this.m_removeAfterDeath && MyAIComponent.BotFactory.GetBotSpawnPosition(this.BotDefinition.BehaviorType, out spawnPosition))
        {
          MyPlayerCollection.OnRespawnRequest(false, false, 0L, (string) null, new Vector3D?(spawnPosition), new Vector3?(), new Vector3?(), new SerializableDefinitionId?((SerializableDefinitionId) this.BotDefinition.Id), false, this.Player.Id.SerialId, (string) null, Color.Red);
          this.m_respawnRequestSent = true;
        }
        else
        {
          if (this.m_botRemoved)
            return;
          this.m_botRemoved = true;
          MyAIComponent.Static.RemoveBot(this.Player.Id.SerialId);
        }
      }
      else
      {
        int timeInMilliseconds = MySandboxGame.TotalGamePlayTimeInMilliseconds;
        this.m_deathCountdownMs -= timeInMilliseconds - this.m_lastCountdownTime;
        this.m_lastCountdownTime = timeInMilliseconds;
      }
    }

    public virtual void DebugDraw()
    {
      if (this.AgentEntity == null)
        return;
      MyAiTargetBase aiTargetBase;
      if ((aiTargetBase = this.m_actions.AiTargetBase) != null && aiTargetBase.HasTarget())
      {
        MyRenderProxy.DebugDrawPoint(aiTargetBase.TargetPosition, Color.Aquamarine, false);
        if (this.BotEntity != null && aiTargetBase.TargetEntity != null)
        {
          string text = aiTargetBase.TargetType == MyAiTargetEnum.CUBE ? string.Format("Target:{0}", (object) aiTargetBase.GetTargetBlock()) : string.Format("Target:{0}", (object) aiTargetBase.TargetEntity);
          Vector3D center = this.BotEntity.PositionComp.WorldAABB.Center;
          center.Y += this.BotEntity.PositionComp.WorldAABB.HalfExtents.Y + 0.200000002980232;
          MyRenderProxy.DebugDrawText3D(center, text, Color.Red, 1f, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
        }
      }
      this.m_botLogic.DebugDraw();
    }

    public virtual void DebugGoto(Vector3D point, MyEntity entity = null)
    {
      if (this.m_player.Id.SerialId == 0)
        return;
      this.m_navigation.AimWithMovement();
      this.m_navigation.GotoNoPath(point, relativeEntity: entity);
    }

    public class SLastRunningState
    {
      public string actionName;
      public int counter;
    }

    public class MyLastActions
    {
      private readonly List<MyAgentBot.SLastRunningState> m_lastActions = new List<MyAgentBot.SLastRunningState>();
      private const int MAX_ACTIONS_COUNT = 5;

      private string GetLastAction() => this.m_lastActions.Last<MyAgentBot.SLastRunningState>().actionName;

      public void AddLastAction(string lastAction)
      {
        if (this.m_lastActions.Count != 0)
        {
          if (lastAction == this.GetLastAction())
          {
            ++this.m_lastActions.Last<MyAgentBot.SLastRunningState>().counter;
            return;
          }
          if (this.m_lastActions.Count == 5)
            this.m_lastActions.RemoveAt(0);
        }
        this.m_lastActions.Add(new MyAgentBot.SLastRunningState()
        {
          actionName = lastAction,
          counter = 1
        });
      }

      public void Clear() => this.m_lastActions.Clear();

      public string GetLastActionsString()
      {
        StringBuilder stringBuilder = new StringBuilder();
        for (int index = 0; index < this.m_lastActions.Count; ++index)
        {
          stringBuilder.AppendFormat("{0}-{1}", (object) this.m_lastActions[index].counter, (object) this.m_lastActions[index].actionName);
          if (index != this.m_lastActions.Count - 1)
            stringBuilder.AppendFormat(", ");
        }
        return stringBuilder.ToString();
      }
    }
  }
}
