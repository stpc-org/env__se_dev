// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MySessionComponentMatch
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.Components;
using VRage.Library.Utils;
using VRage.Network;

namespace Sandbox.Game.SessionComponents
{
  [StaticEventOwner]
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 2000, typeof (MyObjectBuilder_SessionComponentMatch), null, false)]
  public class MySessionComponentMatch : MySessionComponentBase
  {
    private bool m_isRunning;
    private MyTimeSpan m_stateRemainingTime = MyTimeSpan.FromTicks(long.MaxValue);
    private MyTimeSpan m_lastFrameTime;
    private MyMatchState m_state;
    public Action<MyMatchState> OnStateEnded;
    public Action<MyMatchState> OnStateStarted;
    public Action<MyMatchState, MyMatchState> OnStateChanged;

    public bool IsEnabled { get; private set; }

    public bool IsRunning
    {
      get => this.m_isRunning;
      private set
      {
        if (this.m_isRunning == value)
          return;
        this.m_isRunning = value;
        if (!this.m_isRunning)
          return;
        this.m_lastFrameTime = MyTimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds);
      }
    }

    public MyMatchState State
    {
      get => this.m_state;
      private set
      {
        if (this.m_state == value)
          return;
        bool interruptStateChange = false;
        if (MyVisualScriptLogicProvider.MatchStateEnding != null)
          MyVisualScriptLogicProvider.MatchStateEnding(this.m_state.ToString(), ref interruptStateChange);
        if (interruptStateChange)
          return;
        MyMatchState state = this.m_state;
        if (MyVisualScriptLogicProvider.MatchStateEnded != null)
          MyVisualScriptLogicProvider.MatchStateEnded(state.ToString());
        this.OnStateEnded.InvokeIfNotNull<MyMatchState>(state);
        this.m_state = value;
        if (MyVisualScriptLogicProvider.MatchStateChanged != null)
          MyVisualScriptLogicProvider.MatchStateChanged(state.ToString(), this.m_state.ToString());
        this.OnStateChanged.InvokeIfNotNull<MyMatchState, MyMatchState>(state, this.m_state);
        if (MyVisualScriptLogicProvider.MatchStateStarted != null)
          MyVisualScriptLogicProvider.MatchStateStarted(this.m_state.ToString());
        this.OnStateStarted.InvokeIfNotNull<MyMatchState>(this.m_state);
      }
    }

    public MyTimeSpan RemainingTime
    {
      get => this.m_stateRemainingTime;
      set
      {
        this.m_stateRemainingTime = value;
        MySessionComponentMatch.SyncRemainingTimeWithClients();
      }
    }

    public float RemainingMinutes => double.IsInfinity(this.m_stateRemainingTime.Minutes) ? 0.0f : (float) this.m_stateRemainingTime.Minutes;

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      base.Init(sessionComponent);
      this.IsEnabled = MySession.Static.Settings.EnableMatchComponent;
      this.SetInitState();
      if (sessionComponent is MyObjectBuilder_SessionComponentMatch sessionComponentMatch)
      {
        this.m_state = (MyMatchState) sessionComponentMatch.State;
        this.RemainingTime = MyTimeSpan.FromMinutes((double) sessionComponentMatch.RemainingTimeInMinutes);
        this.IsRunning = sessionComponentMatch.IsRunning;
      }
      if (!Sync.IsServer)
        return;
      MyVisualScriptLogicProvider.PlayerSpawned += new SingleKeyPlayerEvent(this.OnPlayerSpawned);
      MyVisualScriptLogicProvider.PlayerConnected += new SingleKeyPlayerEvent(this.OnPlayerConnected);
    }

    private void OnPlayerConnected(long playerId)
    {
      if (this.State != MyMatchState.Match)
        return;
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(playerId);
      if (identity == null || identity.Character == null || identity.Character.IsDead)
        return;
      MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (s => new Action(MySessionComponentMatch.CreateScoreScreenClient)), new EndpointId(MySession.Static.Players.TryGetSteamId(playerId)));
    }

    private void OnPlayerSpawned(long playerId)
    {
      if (this.State != MyMatchState.Match)
        return;
      MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (s => new Action(MySessionComponentMatch.CreateScoreScreenClient)), new EndpointId(MySession.Static.Players.TryGetSteamId(playerId)));
    }

    protected override void UnloadData()
    {
      if (!Sync.IsServer)
        return;
      MyVisualScriptLogicProvider.PlayerSpawned -= new SingleKeyPlayerEvent(this.OnPlayerSpawned);
      MyVisualScriptLogicProvider.PlayerConnected -= new SingleKeyPlayerEvent(this.OnPlayerConnected);
    }

    private void SetInitState()
    {
      this.m_state = MyMatchState.PreMatch;
      this.RemainingTime = MyTimeSpan.FromMinutes((double) MySession.Static.Settings.PreMatchDuration);
    }

    public void ResetToFirstState()
    {
      this.State = MyMatchState.PreMatch;
      if (this.State != MyMatchState.PreMatch)
        return;
      this.RemainingTime = MyTimeSpan.FromMinutes((double) MySession.Static.Settings.PreMatchDuration);
    }

    public override MyObjectBuilder_SessionComponent GetObjectBuilder()
    {
      MyObjectBuilder_SessionComponentMatch objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_SessionComponentMatch;
      objectBuilder.State = (int) this.m_state;
      objectBuilder.RemainingTimeInMinutes = this.RemainingMinutes;
      objectBuilder.IsRunning = this.IsRunning;
      return (MyObjectBuilder_SessionComponent) objectBuilder;
    }

    public override void UpdateAfterSimulation()
    {
      MyTimeSpan myTimeSpan = MyTimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds);
      this.m_stateRemainingTime -= myTimeSpan - this.m_lastFrameTime;
      this.m_lastFrameTime = myTimeSpan;
      if (!Sync.IsServer)
        return;
      base.UpdateAfterSimulation();
      if (this.RemainingTime.Ticks >= 0L)
        return;
      this.RemainingTime = MyTimeSpan.Zero;
      this.StateTimedOut();
    }

    [Event(null, 216)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void CreateScoreScreenSync()
    {
      if (Sync.IsDedicated || MyScreenManager.ExistsScreenOfType(typeof (MyGuiScreenWarfareFactionScore)))
        return;
      MyScreenManager.InsertScreen((MyGuiScreenBase) new MyGuiScreenWarfareFactionScore(), 1);
    }

    [Event(null, 232)]
    [Reliable]
    [Client]
    private static void CreateScoreScreenClient()
    {
      if (MyScreenManager.ExistsScreenOfType(typeof (MyGuiScreenWarfareFactionScore)))
        return;
      MyScreenManager.InsertScreen((MyGuiScreenBase) new MyGuiScreenWarfareFactionScore(), 1);
    }

    [Event(null, 243)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void RemoveScoreScreenSync()
    {
      if (Sync.IsDedicated)
        return;
      MyScreenManager.RemoveScreenByType(typeof (MyGuiScreenWarfareFactionScore));
    }

    public void SetIsRunning(bool isRuning) => this.IsRunning = isRuning;

    public void SetRemainingTime(float timeInMinutes)
    {
      if ((double) timeInMinutes > 0.0)
        this.RemainingTime = MyTimeSpan.FromMinutes((double) timeInMinutes);
      else
        this.AdvanceToNextState();
    }

    public void AddRemainingTime(float timeInMinutes)
    {
      this.RemainingTime += MyTimeSpan.FromMinutes((double) timeInMinutes);
      if (!(this.RemainingTime < MyTimeSpan.Zero))
        return;
      this.AdvanceToNextState();
    }

    public void AdvanceToNextState()
    {
      if (this.State == MyMatchState.Finished)
        return;
      this.RemainingTime = MyTimeSpan.Zero;
      this.StateTimedOut();
    }

    private void StateTimedOut()
    {
      switch (this.m_state)
      {
        case MyMatchState.PreMatch:
          MyMatchState myMatchState1 = MyMatchState.Match;
          this.State = myMatchState1;
          if (this.State == myMatchState1)
            this.RemainingTime = MyTimeSpan.FromMinutes((double) MySession.Static.Settings.MatchDuration);
          if (this.State != MyMatchState.Match)
            break;
          MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (s => new Action(MySessionComponentMatch.TurnOnGlobalDamage)));
          MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (s => new Action(MySessionComponentMatch.CreateScoreScreenSync)));
          break;
        case MyMatchState.Match:
          MyMatchState myMatchState2 = MyMatchState.PostMatch;
          this.State = myMatchState2;
          if (this.State == myMatchState2)
            this.RemainingTime = MyTimeSpan.FromMinutes((double) MySession.Static.Settings.PostMatchDuration);
          if (this.State != MyMatchState.PostMatch)
            break;
          MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (s => new Action(MySessionComponentMatch.RemoveScoreScreenSync)));
          break;
        case MyMatchState.PostMatch:
          this.State = MyMatchState.Finished;
          break;
      }
    }

    public static void SetTimeRemainingInternal(float syncTimeSeconds, float timeLeftSeconds) => MySession.Static.GetComponent<MySessionComponentMatch>().RemainingTime = MyTimeSpan.FromSeconds((double) timeLeftSeconds - MySession.Static.ElapsedGameTime.TotalSeconds + (double) syncTimeSeconds);

    [Event(null, 335)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void TurnOnGlobalDamage() => MySessionComponentSafeZones.AllowedActions |= MySafeZoneAction.Damage;

    [Event(null, 341)]
    [Reliable]
    [Server]
    private static void SyncRemainingTimeWithClients()
    {
      if (!MySession.Static.IsServer)
        return;
      MyMultiplayer.RaiseStaticEvent<float, float>((Func<IMyEventOwner, Action<float, float>>) (s => new Action<float, float>(MySessionComponentMatch.RecieveTimeSync)), (float) MySession.Static.ElapsedGameTime.TotalSeconds, (float) MySession.Static.GetComponent<MySessionComponentMatch>().RemainingTime.Seconds);
    }

    [Event(null, 351)]
    [Reliable]
    [Broadcast]
    private static void RecieveTimeSync(float syncTimeSeconds, float timeLeftSeconds) => MySessionComponentMatch.SetTimeRemainingInternal(syncTimeSeconds, timeLeftSeconds);

    protected sealed class CreateScoreScreenSync\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentMatch.CreateScoreScreenSync();
      }
    }

    protected sealed class CreateScoreScreenClient\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentMatch.CreateScoreScreenClient();
      }
    }

    protected sealed class RemoveScoreScreenSync\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentMatch.RemoveScoreScreenSync();
      }
    }

    protected sealed class TurnOnGlobalDamage\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentMatch.TurnOnGlobalDamage();
      }
    }

    protected sealed class SyncRemainingTimeWithClients\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentMatch.SyncRemainingTimeWithClients();
      }
    }

    protected sealed class RecieveTimeSync\u003C\u003ESystem_Single\u0023System_Single : ICallSite<IMyEventOwner, float, float, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in float syncTimeSeconds,
        in float timeLeftSeconds,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentMatch.RecieveTimeSync(syncTimeSeconds, timeLeftSeconds);
      }
    }
  }
}
