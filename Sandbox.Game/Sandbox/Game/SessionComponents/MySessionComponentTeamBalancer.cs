// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MySessionComponentTeamBalancer
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;

namespace Sandbox.Game.SessionComponents
{
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 887)]
  public class MySessionComponentTeamBalancer : MySessionComponentBase
  {
    private bool m_initialized;
    private bool m_enabled;
    private Dictionary<long, MyFaction> m_factions = new Dictionary<long, MyFaction>();

    public bool Enabled
    {
      get => this.m_enabled;
      set
      {
        if (this.m_enabled == value || !Sync.IsServer)
          return;
        this.m_enabled = value;
        if (!this.m_enabled || this.m_initialized)
          return;
        this.Initialize();
      }
    }

    public override void BeforeStart()
    {
      base.BeforeStart();
      if (!MySession.Static.Settings.EnableTeamBalancing || !Sync.IsServer)
        return;
      this.Enabled = true;
    }

    private void Initialize()
    {
      this.m_initialized = true;
      this.m_factions.Clear();
      this.GatherFactions();
      MyVisualScriptLogicProvider.PlayerSpawned += new SingleKeyPlayerEvent(this.OnPlayerSpawned);
    }

    protected override void UnloadData() => MyVisualScriptLogicProvider.PlayerSpawned -= new SingleKeyPlayerEvent(this.OnPlayerSpawned);

    private void OnPlayerSpawned(long playerId)
    {
      if (MySession.Static.Factions.TryGetPlayerFaction(playerId) != null)
        return;
      MyFaction emptiestFaction = this.GetEmptiestFaction();
      if (emptiestFaction == null)
        return;
      MyFactionCollection.SendJoinRequest(emptiestFaction.FactionId, playerId);
      if (MyVisualScriptLogicProvider.TeamBalancerPlayerSorted == null)
        return;
      MyVisualScriptLogicProvider.TeamBalancerPlayerSorted(playerId, emptiestFaction.Tag);
    }

    private MyFaction GetEmptiestFaction()
    {
      MyFaction myFaction = (MyFaction) null;
      int num = int.MaxValue;
      foreach (KeyValuePair<long, MyFaction> faction in this.m_factions)
      {
        DictionaryReader<long, MyFactionMember> members = faction.Value.Members;
        if (members.Count < num)
        {
          members = faction.Value.Members;
          num = members.Count;
          myFaction = faction.Value;
        }
      }
      return myFaction;
    }

    private void GatherFactions()
    {
      MyFactionCollection factions = MySession.Static.Factions;
      foreach (KeyValuePair<long, IMyFaction> faction in factions.Factions)
      {
        if (faction.Value is MyFaction myFaction && !factions.IsNpcFaction(myFaction.FactionId) && MyDefinitionManager.Static.TryGetFactionDefinition(myFaction.Tag) == null && (myFaction.FactionType == MyFactionTypes.PlayerMade || myFaction.FactionType == MyFactionTypes.None))
        {
          myFaction.AutoAcceptMember = true;
          this.m_factions.Add(myFaction.FactionId, myFaction);
        }
      }
    }
  }
}
