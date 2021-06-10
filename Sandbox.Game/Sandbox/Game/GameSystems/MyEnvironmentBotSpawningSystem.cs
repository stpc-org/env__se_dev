// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyEnvironmentBotSpawningSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.AI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment.Modules;
using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Components;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GameSystems
{
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 1111, typeof (MyObjectBuilder_EnvironmentBotSpawningSystem), null, false)]
  public class MyEnvironmentBotSpawningSystem : MySessionComponentBase
  {
    private static readonly int DELAY_BETWEEN_TICKS_IN_MS = 120000;
    private static readonly float BOT_SPAWN_RANGE_MIN = 80f;
    private static readonly float BOT_SPAWN_RANGE_MIN_SQ = MyEnvironmentBotSpawningSystem.BOT_SPAWN_RANGE_MIN * MyEnvironmentBotSpawningSystem.BOT_SPAWN_RANGE_MIN;
    private static readonly float BOT_DESPAWN_DISTANCE = 400f;
    private static readonly float BOT_DESPAWN_DISTANCE_SQ = MyEnvironmentBotSpawningSystem.BOT_DESPAWN_DISTANCE * MyEnvironmentBotSpawningSystem.BOT_DESPAWN_DISTANCE;
    private static readonly int MAX_SPAWN_ATTEMPTS = 5;
    public static MyEnvironmentBotSpawningSystem Static;
    private MyRandom m_random = new MyRandom();
    private List<Vector3D> m_tmpPlayerPositions;
    private HashSet<MyBotSpawningEnvironmentProxy> m_activeBotSpawningProxies;
    private int m_lastSpawnEventTimeInMs;
    private int m_timeSinceLastEventInMs;
    private int m_tmpSpawnAttempts;

    public override Type[] Dependencies => new Type[1]
    {
      typeof (MyAIComponent)
    };

    public override bool IsRequiredByGame => MyPerGameSettings.EnableAi;

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      base.Init(sessionComponent);
      this.m_timeSinceLastEventInMs = (sessionComponent as MyObjectBuilder_EnvironmentBotSpawningSystem).TimeSinceLastEventInMs;
      this.m_lastSpawnEventTimeInMs = MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_timeSinceLastEventInMs;
    }

    public override MyObjectBuilder_SessionComponent GetObjectBuilder()
    {
      MyObjectBuilder_EnvironmentBotSpawningSystem objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_EnvironmentBotSpawningSystem;
      objectBuilder.TimeSinceLastEventInMs = this.m_timeSinceLastEventInMs;
      return (MyObjectBuilder_SessionComponent) objectBuilder;
    }

    public override void LoadData()
    {
      base.LoadData();
      MyEnvironmentBotSpawningSystem.Static = this;
      this.m_tmpPlayerPositions = new List<Vector3D>();
      this.m_activeBotSpawningProxies = new HashSet<MyBotSpawningEnvironmentProxy>();
      int num = Sync.IsServer ? 1 : 0;
    }

    public override void BeforeStart()
    {
      base.BeforeStart();
      int num = Sync.IsServer ? 1 : 0;
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      MyEnvironmentBotSpawningSystem.Static = (MyEnvironmentBotSpawningSystem) null;
      this.m_tmpPlayerPositions = (List<Vector3D>) null;
      this.m_activeBotSpawningProxies = (HashSet<MyBotSpawningEnvironmentProxy>) null;
      int num = Sync.IsServer ? 1 : 0;
    }

    public override void Draw() => base.Draw();

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (!Sync.IsServer)
        return;
      this.m_timeSinceLastEventInMs = MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastSpawnEventTimeInMs;
      if (this.m_timeSinceLastEventInMs < MyEnvironmentBotSpawningSystem.DELAY_BETWEEN_TICKS_IN_MS)
        return;
      this.RemoveDistantBots();
      MyAIComponent.Static.CleanUnusedIdentities();
      this.m_tmpSpawnAttempts = 0;
      this.SpawnTick();
      this.m_lastSpawnEventTimeInMs = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      this.m_timeSinceLastEventInMs = 0;
    }

    public void RemoveDistantBots()
    {
      ICollection<MyPlayer> onlinePlayers = Sync.Players.GetOnlinePlayers();
      this.m_tmpPlayerPositions.Capacity = Math.Max(this.m_tmpPlayerPositions.Capacity, onlinePlayers.Count);
      this.m_tmpPlayerPositions.Clear();
      foreach (MyPlayer myPlayer in (IEnumerable<MyPlayer>) onlinePlayers)
      {
        if (myPlayer.Id.SerialId == 0 && myPlayer.Controller.ControlledEntity != null)
          this.m_tmpPlayerPositions.Add(myPlayer.GetPosition());
      }
      foreach (MyPlayer myPlayer in (IEnumerable<MyPlayer>) onlinePlayers)
      {
        if (myPlayer.Controller.ControlledEntity != null && myPlayer.Id.SerialId != 0)
        {
          bool flag = true;
          Vector3D position = myPlayer.GetPosition();
          foreach (Vector3D tmpPlayerPosition in this.m_tmpPlayerPositions)
          {
            if (Vector3D.DistanceSquared(position, tmpPlayerPosition) < (double) MyEnvironmentBotSpawningSystem.BOT_DESPAWN_DISTANCE_SQ)
              flag = false;
          }
          if (flag)
            MyAIComponent.Static.RemoveBot(myPlayer.Id.SerialId, true);
        }
      }
    }

    public void SpawnTick()
    {
      if (this.m_activeBotSpawningProxies.Count == 0 || this.m_tmpSpawnAttempts > MyEnvironmentBotSpawningSystem.MAX_SPAWN_ATTEMPTS)
        return;
      ++this.m_tmpSpawnAttempts;
      if (this.m_activeBotSpawningProxies.ElementAt<MyBotSpawningEnvironmentProxy>(MyUtils.GetRandomInt(0, this.m_activeBotSpawningProxies.Count)).OnSpawnTick())
        return;
      this.SpawnTick();
    }

    public void RegisterBotSpawningProxy(MyBotSpawningEnvironmentProxy proxy) => this.m_activeBotSpawningProxies.Add(proxy);

    public void UnregisterBotSpawningProxy(MyBotSpawningEnvironmentProxy proxy) => this.m_activeBotSpawningProxies.Remove(proxy);

    public bool IsHumanPlayerWithinRange(Vector3 position)
    {
      foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) Sync.Players.GetOnlinePlayers())
      {
        if (onlinePlayer.Id.SerialId == 0 && onlinePlayer.Controller.ControlledEntity != null && (double) Vector3.DistanceSquared((Vector3) onlinePlayer.GetPosition(), position) < (double) MyEnvironmentBotSpawningSystem.BOT_SPAWN_RANGE_MIN_SQ)
          return false;
      }
      return true;
    }
  }
}
