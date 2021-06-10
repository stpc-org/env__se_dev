// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.MyAIComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.AI.BehaviorTree;
using Sandbox.Game.AI.Commands;
using Sandbox.Game.AI.Pathfinding;
using Sandbox.Game.AI.Pathfinding.RecastDetour;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI
{
  [MySessionComponentDescriptor(MyUpdateOrder.Simulation | MyUpdateOrder.AfterSimulation, 1000, typeof (MyObjectBuilder_AIComponent), null, false)]
  public class MyAIComponent : MySessionComponentBase
  {
    private Dictionary<int, MyObjectBuilder_Bot> m_loadedBotObjectBuildersByHandle;
    private List<int> m_loadedLocalPlayers;
    private readonly List<Vector3D> m_tmpSpawnPoints = new List<Vector3D>();
    public static MyAIComponent Static;
    public static MyBotFactoryBase BotFactory;
    private int m_lastBotId;
    private Dictionary<int, MyAIComponent.AgentSpawnData> m_agentsToSpawn;
    private MyHudNotification m_maxBotNotification;
    private bool m_debugDrawPathfinding;
    public MyAgentDefinition BotToSpawn;
    public MyAiCommandDefinition CommandDefinition;
    private MyConcurrentQueue<MyAIComponent.BotRemovalRequest> m_removeQueue;
    private MyConcurrentQueue<MyAIComponent.AgentSpawnData> m_processQueue;
    private FastResourceLock m_lock;
    private BoundingBoxD m_debugTargetAABB;

    public MyBotCollection Bots { get; private set; }

    public IMyPathfinding Pathfinding { get; private set; }

    public MyBehaviorTreeCollection BehaviorTrees { get; private set; }

    public event Action<int, MyBotDefinition> BotCreatedEvent;

    public MyAIComponent()
    {
      MyAIComponent.Static = this;
      MyAIComponent.BotFactory = Activator.CreateInstance(MyPerGameSettings.BotFactoryType) as MyBotFactoryBase;
    }

    public override void LoadData()
    {
      base.LoadData();
      if (!MyPerGameSettings.EnableAi)
        return;
      Sync.Players.NewPlayerRequestSucceeded += new Action<MyPlayer.PlayerId>(this.PlayerCreated);
      Sync.Players.LocalPlayerLoaded += new Action<int>(this.LocalPlayerLoaded);
      Sync.Players.NewPlayerRequestFailed += new Action<int>(this.Players_NewPlayerRequestFailed);
      if (Sync.IsServer)
      {
        Sync.Players.PlayerRemoved += new Action<MyPlayer.PlayerId>(this.Players_PlayerRemoved);
        Sync.Players.PlayerRequesting += new PlayerRequestDelegate(this.Players_PlayerRequesting);
      }
      if (MyPerGameSettings.PathfindingType != (Type) null)
        this.Pathfinding = Activator.CreateInstance(MyPerGameSettings.PathfindingType) as IMyPathfinding;
      this.BehaviorTrees = new MyBehaviorTreeCollection();
      this.Bots = new MyBotCollection(this.BehaviorTrees);
      this.m_loadedLocalPlayers = new List<int>();
      this.m_loadedBotObjectBuildersByHandle = new Dictionary<int, MyObjectBuilder_Bot>();
      this.m_agentsToSpawn = new Dictionary<int, MyAIComponent.AgentSpawnData>();
      this.m_removeQueue = new MyConcurrentQueue<MyAIComponent.BotRemovalRequest>();
      this.m_maxBotNotification = new MyHudNotification(MyCommonTexts.NotificationMaximumNumberBots, 2000, "Red");
      this.m_processQueue = new MyConcurrentQueue<MyAIComponent.AgentSpawnData>();
      this.m_lock = new FastResourceLock();
      if (MyPlatformGameSettings.ENABLE_BEHAVIOR_TREE_TOOL_COMMUNICATION && MyVRage.Platform.Windows.Window != null)
      {
        MyVRage.Platform.Windows.Window.AddMessageHandler(1034U, new ActionRef<MyMessage>(this.OnUploadNewTree));
        MyVRage.Platform.Windows.Window.AddMessageHandler(1036U, new ActionRef<MyMessage>(this.OnBreakDebugging));
        MyVRage.Platform.Windows.Window.AddMessageHandler(1035U, new ActionRef<MyMessage>(this.OnResumeDebugging));
      }
      MyToolbarComponent.CurrentToolbar.SelectedSlotChanged += new Action<MyToolbar, MyToolbar.SlotArgs>(this.CurrentToolbar_SelectedSlotChanged);
      MyToolbarComponent.CurrentToolbar.SlotActivated += new Action<MyToolbar, MyToolbar.SlotArgs, bool>(this.CurrentToolbar_SlotActivated);
      MyToolbarComponent.CurrentToolbar.Unselected += new Action<MyToolbar>(this.CurrentToolbar_Unselected);
    }

    public override Type[] Dependencies => new Type[1]
    {
      typeof (MyToolbarComponent)
    };

    public override void Init(
      MyObjectBuilder_SessionComponent sessionComponentBuilder)
    {
      if (!MyPerGameSettings.EnableAi)
        return;
      base.Init(sessionComponentBuilder);
      MyObjectBuilder_AIComponent builderAiComponent = (MyObjectBuilder_AIComponent) sessionComponentBuilder;
      if (builderAiComponent.BotBrains == null)
        return;
      foreach (MyObjectBuilder_AIComponent.BotData botBrain in builderAiComponent.BotBrains)
        this.m_loadedBotObjectBuildersByHandle[botBrain.PlayerHandle] = botBrain.BotBrain;
    }

    public override void BeforeStart()
    {
      base.BeforeStart();
      if (!MyPerGameSettings.EnableAi)
        return;
      foreach (int loadedLocalPlayer in this.m_loadedLocalPlayers)
      {
        MyObjectBuilder_Bot botBuilder;
        this.m_loadedBotObjectBuildersByHandle.TryGetValue(loadedLocalPlayer, out botBuilder);
        if (botBuilder == null || botBuilder.TypeId == botBuilder.BotDefId.TypeId)
          this.CreateBot(loadedLocalPlayer, botBuilder);
      }
      this.m_loadedLocalPlayers.Clear();
      this.m_loadedBotObjectBuildersByHandle.Clear();
      Sync.Players.LocalPlayerRemoved += new Action<int>(this.LocalPlayerRemoved);
    }

    public override void Simulate()
    {
      if (!MyPerGameSettings.EnableAi)
        return;
      if (MyFakes.DEBUG_ONE_VOXEL_PATHFINDING_STEP_SETTING)
      {
        if (!MyFakes.DEBUG_ONE_VOXEL_PATHFINDING_STEP)
          return;
      }
      else if (MyFakes.DEBUG_ONE_AI_STEP_SETTING)
      {
        if (!MyFakes.DEBUG_ONE_AI_STEP)
          return;
        MyFakes.DEBUG_ONE_AI_STEP = false;
      }
      MySimpleProfiler.Begin("AI", callingMember: nameof (Simulate));
      this.Pathfinding?.Update();
      base.Simulate();
      this.BehaviorTrees.Update();
      this.Bots.Update();
      MySimpleProfiler.End(nameof (Simulate));
    }

    public void PathfindingSetDrawDebug(bool drawDebug) => this.m_debugDrawPathfinding = drawDebug;

    public void PathfindingSetDrawNavmesh(bool drawNavmesh)
    {
      if (!(this.Pathfinding is MyRDPathfinding pathfinding))
        return;
      pathfinding.SetDrawNavmesh(drawNavmesh);
    }

    public Vector3D? DebugTarget { get; private set; }

    public void GenerateNavmeshTile(Vector3D? target)
    {
      if (target.HasValue)
      {
        Vector3D worldCenter = target.Value + 0.1f;
        MyDestinationSphere destinationSphere = new MyDestinationSphere(ref worldCenter, 1f);
        MyAIComponent.Static.Pathfinding.FindPathGlobal(target.Value - 0.100000001490116, (IMyDestinationShape) destinationSphere, (MyEntity) null)?.GetNextTarget(target.Value, out Vector3D _, out float _, out IMyEntity _);
      }
      this.DebugTarget = target;
    }

    public void InvalidateNavmeshPosition(Vector3D? target)
    {
      if (target.HasValue)
      {
        MyRDPathfinding pathfinding = (MyRDPathfinding) MyAIComponent.Static.Pathfinding;
        if (pathfinding != null)
        {
          BoundingBoxD areaBox = new BoundingBoxD(target.Value - 0.1, target.Value + 0.1);
          pathfinding.InvalidateArea(areaBox);
        }
      }
      this.DebugTarget = target;
    }

    public void SetPathfindingDebugTarget(Vector3D? target)
    {
      if (this.Pathfinding is MyExternalPathfinding pathfinding)
        pathfinding.SetTarget(target);
      else if (target.HasValue)
      {
        this.m_debugTargetAABB = new MyOrientedBoundingBoxD(target.Value, new Vector3D(5.0, 5.0, 5.0), Quaternion.Identity).GetAABB();
        MyGamePruningStructure.GetAllEntitiesInBox(ref this.m_debugTargetAABB, new List<MyEntity>());
      }
      this.DebugTarget = target;
    }

    private void DrawDebugTarget()
    {
      if (!this.DebugTarget.HasValue)
        return;
      MyRenderProxy.DebugDrawSphere(this.DebugTarget.Value, 0.2f, Color.Red, 0.0f, false);
      MyRenderProxy.DebugDrawAABB(this.m_debugTargetAABB, Color.Green);
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (!MyPerGameSettings.EnableAi)
        return;
      this.PerformBotRemovals();
      MyAIComponent.AgentSpawnData instance;
      while (this.m_processQueue.TryDequeue(out instance))
        this.SpawnAgent(ref instance);
      if (this.m_debugDrawPathfinding)
        this.Pathfinding?.DebugDraw();
      this.Bots.DebugDraw();
      this.DebugDrawBots();
      this.DrawDebugTarget();
    }

    private long SpawnAgent(ref MyAIComponent.AgentSpawnData newBotData)
    {
      this.m_agentsToSpawn[newBotData.BotId] = newBotData;
      return Sync.Players.RequestNewPlayer(newBotData.SteamId, newBotData.BotId, string.IsNullOrEmpty(newBotData.Name) ? MyDefinitionManager.Static.GetRandomCharacterName() : newBotData.Name, newBotData.AgentDefinition.BotModel, false, false, newBotData.IsWildlifeAgent);
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      if (MyPerGameSettings.EnableAi)
      {
        Sync.Players.NewPlayerRequestSucceeded -= new Action<MyPlayer.PlayerId>(this.PlayerCreated);
        Sync.Players.LocalPlayerRemoved -= new Action<int>(this.LocalPlayerRemoved);
        Sync.Players.LocalPlayerLoaded -= new Action<int>(this.LocalPlayerLoaded);
        Sync.Players.NewPlayerRequestFailed -= new Action<int>(this.Players_NewPlayerRequestFailed);
        if (Sync.IsServer)
        {
          Sync.Players.PlayerRequesting -= new PlayerRequestDelegate(this.Players_PlayerRequesting);
          Sync.Players.PlayerRemoved -= new Action<MyPlayer.PlayerId>(this.Players_PlayerRemoved);
        }
        this.Pathfinding?.UnloadData();
        this.Bots.UnloadData();
        this.Bots = (MyBotCollection) null;
        this.Pathfinding = (IMyPathfinding) null;
        if (MyPlatformGameSettings.ENABLE_BEHAVIOR_TREE_TOOL_COMMUNICATION && MyVRage.Platform?.Windows.Window != null)
        {
          MyVRage.Platform.Windows.Window.RemoveMessageHandler(1034U, new ActionRef<MyMessage>(this.OnUploadNewTree));
          MyVRage.Platform.Windows.Window.RemoveMessageHandler(1036U, new ActionRef<MyMessage>(this.OnBreakDebugging));
          MyVRage.Platform.Windows.Window.RemoveMessageHandler(1035U, new ActionRef<MyMessage>(this.OnResumeDebugging));
        }
        if (MyToolbarComponent.CurrentToolbar != null)
        {
          MyToolbarComponent.CurrentToolbar.SelectedSlotChanged -= new Action<MyToolbar, MyToolbar.SlotArgs>(this.CurrentToolbar_SelectedSlotChanged);
          MyToolbarComponent.CurrentToolbar.SlotActivated -= new Action<MyToolbar, MyToolbar.SlotArgs, bool>(this.CurrentToolbar_SlotActivated);
          MyToolbarComponent.CurrentToolbar.Unselected -= new Action<MyToolbar>(this.CurrentToolbar_Unselected);
        }
      }
      MyAIComponent.Static = (MyAIComponent) null;
    }

    public override MyObjectBuilder_SessionComponent GetObjectBuilder()
    {
      if (!MyPerGameSettings.EnableAi)
        return (MyObjectBuilder_SessionComponent) null;
      MyObjectBuilder_AIComponent objectBuilder = (MyObjectBuilder_AIComponent) base.GetObjectBuilder();
      this.Bots.GetBotsData(objectBuilder.BotBrains, objectBuilder);
      return (MyObjectBuilder_SessionComponent) objectBuilder;
    }

    public int SpawnNewBot(MyAgentDefinition agentDefinition)
    {
      Vector3D spawnPosition = new Vector3D();
      return !MyAIComponent.BotFactory.GetBotSpawnPosition(agentDefinition.BehaviorType, out spawnPosition) ? 0 : this.SpawnNewBotInternal(agentDefinition, new Vector3D?(spawnPosition));
    }

    public int SpawnNewBot(
      MyAgentDefinition agentDefinition,
      Vector3D position,
      bool createdByPlayer = true,
      bool isWildlifeAgent = true)
    {
      return this.SpawnNewBotInternal(agentDefinition, new Vector3D?(position), createdByPlayer, isWildlifeAgent);
    }

    public bool SpawnNewBotGroup(
      string type,
      List<MyAIComponent.AgentGroupData> groupData,
      List<int> outIds)
    {
      int count1 = 0;
      foreach (MyAIComponent.AgentGroupData agentGroupData in groupData)
        count1 += agentGroupData.Count;
      MyAIComponent.BotFactory.GetBotGroupSpawnPositions(type, count1, this.m_tmpSpawnPoints);
      int count2 = this.m_tmpSpawnPoints.Count;
      int index1 = 0;
      int index2 = 0;
      int num1 = 0;
      for (; index1 < count2; ++index1)
      {
        int num2 = this.SpawnNewBotInternal(groupData[index2].AgentDefinition, new Vector3D?(this.m_tmpSpawnPoints[index1]));
        outIds?.Add(num2);
        if (groupData[index2].Count == ++num1)
        {
          num1 = 0;
          ++index2;
        }
      }
      this.m_tmpSpawnPoints.Clear();
      return count2 == count1;
    }

    private int SpawnNewBotInternal(
      MyAgentDefinition agentDefinition,
      Vector3D? spawnPosition = null,
      bool createdByPlayer = false,
      bool isWildlifeAgent = true)
    {
      int lastBotId;
      using (this.m_lock.AcquireExclusiveUsing())
      {
        foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) Sync.Players.GetOnlinePlayers())
        {
          if ((long) onlinePlayer.Id.SteamId == (long) Sync.MyId && onlinePlayer.Id.SerialId > this.m_lastBotId)
            this.m_lastBotId = onlinePlayer.Id.SerialId;
        }
        ++this.m_lastBotId;
        lastBotId = this.m_lastBotId;
      }
      this.m_processQueue.Enqueue(new MyAIComponent.AgentSpawnData(agentDefinition, Sync.MyId, lastBotId, spawnPosition, createdByPlayer, isWildlifeAgent));
      return lastBotId;
    }

    public long SpawnNewBotSync(
      MyAgentDefinition agentDefinition,
      ulong steamId,
      Vector3D spawnPosition,
      Vector3? direction,
      Vector3? up,
      string name)
    {
      int lastBotId;
      using (this.m_lock.AcquireExclusiveUsing())
      {
        foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) Sync.Players.GetOnlinePlayers())
        {
          if ((long) onlinePlayer.Id.SteamId == (long) Sync.MyId && onlinePlayer.Id.SerialId > this.m_lastBotId)
            this.m_lastBotId = onlinePlayer.Id.SerialId;
        }
        ++this.m_lastBotId;
        lastBotId = this.m_lastBotId;
      }
      MyAIComponent.AgentSpawnData newBotData = new MyAIComponent.AgentSpawnData(agentDefinition, steamId, lastBotId, new Vector3D?(spawnPosition), isWildlifeAgent: true, up: up, direction: direction, name: name);
      return this.SpawnAgent(ref newBotData);
    }

    public int SpawnNewBot(MyAgentDefinition agentDefinition, Vector3D? spawnPosition) => this.SpawnNewBotInternal(agentDefinition, spawnPosition, true);

    public bool CanSpawnMoreBots(MyPlayer.PlayerId pid)
    {
      if (!Sync.IsServer)
        return false;
      if (MyFakes.DEVELOPMENT_PRESET)
        return true;
      if ((long) Sync.MyId == (long) pid.SteamId)
      {
        MyAIComponent.AgentSpawnData agentSpawnData;
        if (!this.m_agentsToSpawn.TryGetValue(pid.SerialId, out agentSpawnData))
          return false;
        return agentSpawnData.CreatedByPlayer ? this.Bots.GetCreatedBotCount() < MyAIComponent.BotFactory.MaximumBotPerPlayer : this.Bots.GetGeneratedBotCount() < MyAIComponent.BotFactory.MaximumUncontrolledBotCount;
      }
      int num = 0;
      ulong steamId = pid.SteamId;
      foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) Sync.Players.GetOnlinePlayers())
      {
        if ((long) onlinePlayer.Id.SteamId == (long) steamId && onlinePlayer.Id.SerialId != 0)
          ++num;
      }
      return num < MyAIComponent.BotFactory.MaximumBotPerPlayer;
    }

    public int GetAvailableUncontrolledBotsCount() => MyAIComponent.BotFactory.MaximumUncontrolledBotCount - this.Bots.GetGeneratedBotCount();

    public int GetBotCount(string behaviorType) => this.Bots.GetCurrentBotsCount(behaviorType);

    public void CleanUnusedIdentities()
    {
      List<MyPlayer.PlayerId> playerIdList = new List<MyPlayer.PlayerId>();
      foreach (MyPlayer.PlayerId allPlayer in (IEnumerable<MyPlayer.PlayerId>) Sync.Players.GetAllPlayers())
        playerIdList.Add(allPlayer);
      foreach (MyPlayer.PlayerId playerId in playerIdList)
      {
        if ((long) playerId.SteamId == (long) Sync.MyId && playerId.SerialId != 0 && Sync.Players.GetPlayerById(playerId) == null)
        {
          long identityId = Sync.Players.TryGetIdentityId(playerId.SteamId, playerId.SerialId);
          if (identityId != 0L)
            Sync.Players.RemoveIdentity(identityId, playerId);
        }
      }
    }

    private void PlayerCreated(MyPlayer.PlayerId playerId)
    {
      if (Sync.Players.GetPlayerById(playerId) == null || Sync.Players.GetPlayerById(playerId).IsRealPlayer)
        return;
      this.CreateBot(playerId.SerialId);
    }

    private void LocalPlayerLoaded(int playerNumber)
    {
      if (playerNumber == 0 || this.m_loadedLocalPlayers.Contains(playerNumber))
        return;
      this.m_loadedLocalPlayers.Add(playerNumber);
    }

    private void Players_NewPlayerRequestFailed(int serialId)
    {
      if (serialId == 0 || !this.m_agentsToSpawn.ContainsKey(serialId))
        return;
      MyAIComponent.AgentSpawnData agentSpawnData = this.m_agentsToSpawn[serialId];
      this.m_agentsToSpawn.Remove(serialId);
      if (!agentSpawnData.CreatedByPlayer)
        return;
      MyHud.Notifications.Add((MyHudNotificationBase) this.m_maxBotNotification);
    }

    private void Players_PlayerRequesting(PlayerRequestArgs args)
    {
      if (args.PlayerId.SerialId == 0)
        return;
      if (!this.CanSpawnMoreBots(args.PlayerId))
        args.Cancel = true;
      else
        ++this.Bots.TotalBotCount;
    }

    private void Players_PlayerRemoved(MyPlayer.PlayerId pid)
    {
      if (!Sync.IsServer || pid.SerialId == 0)
        return;
      --this.Bots.TotalBotCount;
    }

    private void CreateBot(int playerNumber) => this.CreateBot(playerNumber, (MyObjectBuilder_Bot) null);

    private void CreateBot(int playerNumber, MyObjectBuilder_Bot botBuilder)
    {
      if (MyAIComponent.BotFactory == null || this.Bots == null)
        return;
      MyPlayer player1 = botBuilder != null ? Sync.Clients.LocalClient.GetPlayer(botBuilder.AsociatedMyPlayerId, playerNumber) : Sync.Clients.LocalClient.GetPlayer(playerNumber);
      if (player1 == null)
        return;
      player1.IsWildlifeAgent = true;
      bool flag = this.m_agentsToSpawn.ContainsKey(playerNumber);
      bool load = botBuilder != null;
      bool spawnedByPlayer = false;
      MyAIComponent.AgentSpawnData agentSpawnData = new MyAIComponent.AgentSpawnData();
      MyBotDefinition botDefinition;
      if (flag)
      {
        agentSpawnData = this.m_agentsToSpawn[playerNumber];
        spawnedByPlayer = agentSpawnData.CreatedByPlayer;
        botDefinition = (MyBotDefinition) agentSpawnData.AgentDefinition;
        this.m_agentsToSpawn.Remove(playerNumber);
      }
      else
      {
        if (botBuilder == null || botBuilder.BotDefId.TypeId.IsNull)
        {
          MyPlayer player2;
          if (!Sync.Players.TryGetPlayerById(new MyPlayer.PlayerId(Sync.MyId, playerNumber), out player2))
            return;
          Sync.Players.RemovePlayer(player2);
          return;
        }
        MyDefinitionManager.Static.TryGetBotDefinition((MyDefinitionId) botBuilder.BotDefId, out botDefinition);
      }
      if (botDefinition == null)
        return;
      if (((player1.Character == null || !player1.Character.IsDead ? (MyAIComponent.BotFactory.CanCreateBotOfType(botDefinition.BehaviorType, load) ? 1 : 0) : 0) | (spawnedByPlayer ? 1 : 0)) != 0)
      {
        IMyBot newBot = !flag ? MyAIComponent.BotFactory.CreateBot(player1, botBuilder, botDefinition) : MyAIComponent.BotFactory.CreateBot(player1, botBuilder, (MyBotDefinition) agentSpawnData.AgentDefinition);
        if (newBot == null)
        {
          MyLog.Default.WriteLine("Could not create a bot for player " + (object) player1 + "!");
        }
        else
        {
          this.Bots.AddBot(playerNumber, newBot);
          if (flag && newBot is IMyEntityBot myEntityBot)
          {
            myEntityBot.Spawn(agentSpawnData.SpawnPosition, agentSpawnData.Direction, agentSpawnData.Up, spawnedByPlayer);
            if (myEntityBot.BotEntity == null)
              MyLog.Default.WriteLine("Bot entity is null! bot: " + agentSpawnData.Name);
            myEntityBot.BotEntity.Name = agentSpawnData.Name;
          }
          if (newBot.BotDefinition == null)
            MyLog.Default.WriteLine("Bot definition is null! bot: " + agentSpawnData.Name);
          Action<int, MyBotDefinition> botCreatedEvent = this.BotCreatedEvent;
          if (botCreatedEvent == null)
            return;
          botCreatedEvent(playerNumber, newBot.BotDefinition);
        }
      }
      else
        Sync.Players.RemovePlayer(Sync.Players.GetPlayerById(new MyPlayer.PlayerId(Sync.MyId, playerNumber)));
    }

    public void DespawnBotsOfType(string botType)
    {
      foreach (KeyValuePair<int, IMyBot> allBot in this.Bots.GetAllBots())
      {
        if (allBot.Value.BotDefinition.BehaviorType == botType)
        {
          Sync.Players.GetPlayerById(new MyPlayer.PlayerId(Sync.MyId, allBot.Key));
          this.RemoveBot(allBot.Key, true);
        }
      }
      this.PerformBotRemovals();
    }

    private void PerformBotRemovals()
    {
      MyAIComponent.BotRemovalRequest instance;
      while (this.m_removeQueue.TryDequeue(out instance))
      {
        MyPlayer playerById = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(Sync.MyId, instance.SerialId));
        if (playerById != null)
          Sync.Players.RemovePlayer(playerById, instance.RemoveCharacter);
      }
    }

    public void RemoveBot(int playerNumber, bool removeCharacter = false) => this.m_removeQueue.Enqueue(new MyAIComponent.BotRemovalRequest()
    {
      SerialId = playerNumber,
      RemoveCharacter = removeCharacter
    });

    private void LocalPlayerRemoved(int playerNumber)
    {
      if (playerNumber == 0)
        return;
      this.Bots.TryRemoveBot(playerNumber);
    }

    public override void HandleInput()
    {
      base.HandleInput();
      if (!(MyScreenManager.GetScreenWithFocus() is MyGuiScreenGamePlay) || !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.PRIMARY_TOOL_ACTION))
        return;
      if (MySession.Static.ControlledEntity != null && this.BotToSpawn != null)
        this.TrySpawnBot();
      if (MySession.Static.ControlledEntity == null || this.CommandDefinition == null)
        return;
      this.UseCommand();
    }

    public void TrySpawnBot(MyAgentDefinition agentDefinition)
    {
      this.BotToSpawn = agentDefinition;
      this.TrySpawnBot();
    }

    private void CurrentToolbar_SelectedSlotChanged(MyToolbar toolbar, MyToolbar.SlotArgs args)
    {
      if (!(toolbar.SelectedItem is MyToolbarItemBot))
        this.BotToSpawn = (MyAgentDefinition) null;
      if (toolbar.SelectedItem is MyToolbarItemAiCommand)
        return;
      this.CommandDefinition = (MyAiCommandDefinition) null;
    }

    private void CurrentToolbar_SlotActivated(
      MyToolbar toolbar,
      MyToolbar.SlotArgs args,
      bool userActivated)
    {
      if (!(toolbar.GetItemAtIndex(toolbar.SlotToIndex(args.SlotNumber.Value)) is MyToolbarItemBot))
        this.BotToSpawn = (MyAgentDefinition) null;
      if (toolbar.GetItemAtIndex(toolbar.SlotToIndex(args.SlotNumber.Value)) is MyToolbarItemAiCommand)
        return;
      this.CommandDefinition = (MyAiCommandDefinition) null;
    }

    private void CurrentToolbar_Unselected(MyToolbar toolbar)
    {
      this.BotToSpawn = (MyAgentDefinition) null;
      this.CommandDefinition = (MyAiCommandDefinition) null;
    }

    private void TrySpawnBot()
    {
      Vector3D position1 = MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.ThirdPersonSpectator || MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Entity ? MySession.Static.ControlledEntity.GetHeadMatrix(true).Translation : MySector.MainCamera.Position;
      List<MyPhysics.HitInfo> toList = new List<MyPhysics.HitInfo>();
      LineD lineD = new LineD(MySector.MainCamera.Position, MySector.MainCamera.Position + MySector.MainCamera.ForwardVector * 1000f);
      MyPhysics.CastRay(lineD.From, lineD.To, toList, 15);
      if (toList.Count == 0)
      {
        MyAIComponent.Static.SpawnNewBot(this.BotToSpawn, position1);
      }
      else
      {
        MyPhysics.HitInfo? nullable = new MyPhysics.HitInfo?();
        foreach (MyPhysics.HitInfo hitInfo in toList)
        {
          switch (hitInfo.HkHitInfo.GetHitEntity())
          {
            case MyCubeGrid _:
              nullable = new MyPhysics.HitInfo?(hitInfo);
              goto label_10;
            case MyVoxelBase _:
              nullable = new MyPhysics.HitInfo?(hitInfo);
              goto label_10;
            case MyVoxelPhysics _:
              nullable = new MyPhysics.HitInfo?(hitInfo);
              goto label_10;
            default:
              continue;
          }
        }
label_10:
        Vector3D position2 = !nullable.HasValue ? MySector.MainCamera.Position : nullable.Value.Position;
        MyAIComponent.Static.SpawnNewBot(this.BotToSpawn, position2);
      }
    }

    private void UseCommand()
    {
      MyAiCommandBehavior aiCommandBehavior = new MyAiCommandBehavior();
      aiCommandBehavior.InitCommand(this.CommandDefinition);
      aiCommandBehavior.ActivateCommand();
    }

    public static int GenerateBotId(int lastSpawnedBot)
    {
      int val1 = lastSpawnedBot;
      foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) Sync.Players.GetOnlinePlayers())
      {
        if ((long) onlinePlayer.Id.SteamId == (long) Sync.MyId)
          val1 = Math.Max(val1, onlinePlayer.Id.SerialId);
      }
      return val1 + 1;
    }

    public static int GenerateBotId()
    {
      int lastBotId = MyAIComponent.Static.m_lastBotId;
      MyAIComponent.Static.m_lastBotId = MyAIComponent.GenerateBotId(lastBotId);
      return MyAIComponent.Static.m_lastBotId;
    }

    public void DebugDrawBots()
    {
      if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
        return;
      this.Bots.DebugDrawBots();
    }

    public void DebugSelectNextBot() => this.Bots.DebugSelectNextBot();

    public void DebugSelectPreviousBot() => this.Bots.DebugSelectPreviousBot();

    public void DebugRemoveFirstBot()
    {
      if (!this.Bots.HasBot)
        return;
      Sync.Players.RemovePlayer(Sync.Players.GetPlayerById(new MyPlayer.PlayerId(Sync.MyId, this.Bots.GetHandleToFirstBot())));
    }

    private void OnUploadNewTree(ref MyMessage msg)
    {
      if (this.BehaviorTrees == null)
        return;
      MyBehaviorTree outBehaviorTree = (MyBehaviorTree) null;
      MyBehaviorDefinition definition = (MyBehaviorDefinition) null;
      if (MyBehaviorTreeCollection.LoadUploadedBehaviorTree(out definition) && this.BehaviorTrees.HasBehavior(definition.Id.SubtypeId))
      {
        this.Bots.ResetBots(definition.Id.SubtypeName);
        this.BehaviorTrees.RebuildBehaviorTree(definition, out outBehaviorTree);
        this.Bots.CheckCompatibilityWithBots(outBehaviorTree);
      }
      IntPtr windowHandle = IntPtr.Zero;
      if (!this.BehaviorTrees.TryGetValidToolWindow(out windowHandle))
        return;
      MyVRage.Platform.Windows.PostMessage(windowHandle, 1028U, IntPtr.Zero, IntPtr.Zero);
    }

    private void OnBreakDebugging(ref MyMessage msg)
    {
      if (this.BehaviorTrees == null)
        return;
      this.BehaviorTrees.DebugBreakDebugging = true;
    }

    private void OnResumeDebugging(ref MyMessage msg)
    {
      if (this.BehaviorTrees == null)
        return;
      this.BehaviorTrees.DebugBreakDebugging = false;
    }

    private struct AgentSpawnData
    {
      public readonly MyAgentDefinition AgentDefinition;
      public readonly Vector3D? SpawnPosition;
      public readonly bool CreatedByPlayer;
      public readonly int BotId;
      public bool IsWildlifeAgent;
      public ulong SteamId;
      public string Name;
      public Vector3? Up;
      public Vector3? Direction;

      public AgentSpawnData(
        MyAgentDefinition agentDefinition,
        ulong steamId,
        int botId,
        Vector3D? spawnPosition = null,
        bool createAlways = false,
        bool isWildlifeAgent = false,
        Vector3? up = null,
        Vector3? direction = null,
        string name = null)
      {
        this.AgentDefinition = agentDefinition;
        this.SpawnPosition = spawnPosition;
        this.CreatedByPlayer = createAlways;
        this.BotId = botId;
        this.IsWildlifeAgent = isWildlifeAgent;
        this.SteamId = steamId;
        this.Direction = direction;
        this.Up = up;
        this.Name = name;
      }
    }

    public struct AgentGroupData
    {
      public MyAgentDefinition AgentDefinition;
      public int Count;

      public AgentGroupData(MyAgentDefinition agentDefinition, int count)
      {
        this.AgentDefinition = agentDefinition;
        this.Count = count;
      }
    }

    private struct BotRemovalRequest
    {
      public int SerialId;
      public bool RemoveCharacter;
    }
  }
}
