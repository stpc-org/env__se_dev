// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Multiplayer.MyFactionCollection
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ProtoBuf;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Definitions.Reputation;
using VRage.Game.Factions.Definitions;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Definitions.Reputation;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Multiplayer
{
  [StaticEventOwner]
  public class MyFactionCollection : IEnumerable<KeyValuePair<long, MyFaction>>, IEnumerable, IMyFactionCollection
  {
    public const int MAX_CHARACTER_FACTION = 512;
    public const string DLC_ECONOMY_ICON_CATEGORY = "Other";
    public const int ACHIEVEMENT_FRIEND_OF_FACTION_COUNT = 3;
    public const string ACHIEVEMENT_KEY_FRIEND_OF_FACTION = "FriendOfFactions";
    private const string SPIDER_FACTION_TAG = "SPID";
    public Action<long, long> PlayerKilledByPlayer;
    public Action<long> PlayerKilledByUnknown;
    private Dictionary<long, MyFaction> m_factions = new Dictionary<long, MyFaction>();
    private Dictionary<string, MyFaction> m_factionsByTag = new Dictionary<string, MyFaction>((IEqualityComparer<string>) StringComparer.InvariantCultureIgnoreCase);
    private Dictionary<long, HashSet<long>> m_factionRequests = new Dictionary<long, HashSet<long>>();
    private Dictionary<MyFactionCollection.MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>> m_relationsBetweenFactions = new Dictionary<MyFactionCollection.MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>>((IEqualityComparer<MyFactionCollection.MyRelatablePair>) MyFactionCollection.MyRelatablePair.Comparer);
    private Dictionary<MyFactionCollection.MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>> m_relationsBetweenPlayersAndFactions = new Dictionary<MyFactionCollection.MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>>((IEqualityComparer<MyFactionCollection.MyRelatablePair>) MyFactionCollection.MyRelatablePair.Comparer);
    private Dictionary<long, long> m_playerFaction = new Dictionary<long, long>();
    private Dictionary<MyPlayer.PlayerId, List<long>> m_playerToFactionsVis = new Dictionary<MyPlayer.PlayerId, List<long>>();
    private MyReputationSettingsDefinition m_reputationSettings;
    private Dictionary<long, Tuple<int, TimeSpan>> m_playerToReputationLimits = new Dictionary<long, Tuple<int, TimeSpan>>();
    private MyReputationNotification m_notificationRepInc;
    private MyReputationNotification m_notificationRepDec;

    public event Action<MyFaction, MyPlayer.PlayerId> OnFactionDiscovered;

    public event Action<MyFaction, long> OnPlayerJoined;

    public event Action<MyFaction, long> OnPlayerLeft;

    public bool JoinableFactionsPresent
    {
      get
      {
        foreach (KeyValuePair<long, MyFaction> faction in this.m_factions)
        {
          if (faction.Value.AcceptHumans)
            return true;
        }
        return false;
      }
    }

    public bool Contains(long factionId) => this.m_factions.ContainsKey(factionId);

    public bool FactionTagExists(string tag, IMyFaction doNotCheck = null) => this.TryGetFactionByTag(tag, doNotCheck) != null;

    public bool FactionNameExists(string name, IMyFaction doNotCheck = null)
    {
      foreach (KeyValuePair<long, MyFaction> faction in this.m_factions)
      {
        MyFaction myFaction = faction.Value;
        if ((doNotCheck == null || doNotCheck.FactionId != myFaction.FactionId) && string.Equals(name, myFaction.Name, StringComparison.OrdinalIgnoreCase))
          return true;
      }
      return false;
    }

    public IMyFaction TryGetFactionById(long factionId)
    {
      MyFaction myFaction;
      return this.m_factions.TryGetValue(factionId, out myFaction) ? (IMyFaction) myFaction : (IMyFaction) null;
    }

    public MyFaction TryGetOrCreateFactionByTag(string tag)
    {
      MyFaction factionByTag = this.TryGetFactionByTag(tag, (IMyFaction) null);
      if (factionByTag == null)
      {
        if (MyDefinitionManager.Static.TryGetFactionDefinition(tag) == null)
          return (MyFaction) null;
        MyMultiplayer.RaiseStaticEvent<string>((Func<IMyEventOwner, Action<string>>) (x => new Action<string>(MyFactionCollection.CreateFactionByDefinition)), tag);
        factionByTag = this.TryGetFactionByTag(tag, (IMyFaction) null);
      }
      return factionByTag;
    }

    public bool IsNpcFaction(string tag)
    {
      if (MyDefinitionManager.Static.TryGetFactionDefinition(tag) != null)
        return true;
      MyFaction factionByTag = this.TryGetFactionByTag(tag, (IMyFaction) null);
      if (factionByTag != null)
      {
        switch (factionByTag.FactionType)
        {
          case MyFactionTypes.Miner:
          case MyFactionTypes.Trader:
          case MyFactionTypes.Builder:
            return true;
        }
      }
      return false;
    }

    public bool IsNpcFaction(long factionId)
    {
      if (!this.m_factions.ContainsKey(factionId))
        return false;
      MyFaction faction = this.m_factions[factionId];
      if (faction != null)
      {
        switch (faction.FactionType)
        {
          case MyFactionTypes.Miner:
          case MyFactionTypes.Trader:
          case MyFactionTypes.Builder:
            return true;
        }
      }
      return false;
    }

    internal bool IsDiscoveredByDefault(string tag)
    {
      MyFactionDefinition factionDefinition = MyDefinitionManager.Static.TryGetFactionDefinition(tag);
      return factionDefinition != null && factionDefinition.DiscoveredByDefault;
    }

    [Event(null, 300)]
    [Reliable]
    [Server]
    public static void CreateFactionByDefinition(string tag)
    {
      if (MySession.Static.Factions.m_factionsByTag.ContainsKey(tag))
        return;
      MyFactionDefinition factionDefinition = MyDefinitionManager.Static.TryGetFactionDefinition(tag);
      if (factionDefinition == null)
        return;
      MyIdentity newIdentity = Sync.Players.CreateNewIdentity(factionDefinition.Founder, (string) null, new Vector3?(), false, false);
      Sync.Players.MarkIdentityAsNPC(newIdentity.IdentityId);
      MyFactionCollection.CreateFactionServer(newIdentity.IdentityId, tag.ToUpperInvariant(), factionDefinition.DisplayNameText, factionDefinition.DescriptionText, "", factionDefinition);
    }

    public void CreateDefaultFactions()
    {
      foreach (MyFactionDefinition defaultFaction in MyDefinitionManager.Static.GetDefaultFactions())
      {
        if (this.TryGetFactionByTag(defaultFaction.Tag, (IMyFaction) null) == null)
        {
          MyIdentity newIdentity = Sync.Players.CreateNewIdentity(defaultFaction.Founder, (string) null, new Vector3?(), false, false);
          if (newIdentity != null)
          {
            Sync.Players.MarkIdentityAsNPC(newIdentity.IdentityId);
            long num = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.FACTION);
            if (!MyFactionCollection.CreateFactionInternal(newIdentity.IdentityId, num, defaultFaction))
              Sync.Players.RemoveIdentity(newIdentity.IdentityId);
            MyBankingSystem.Static.CreateAccount(num, defaultFaction.StartingBalance);
          }
        }
      }
      MyPlayer.PlayerId? playerId = new MyPlayer.PlayerId?();
      if (MySession.Static.LocalHumanPlayer != null)
        playerId = new MyPlayer.PlayerId?(MySession.Static.LocalHumanPlayer.Id);
      this.CompatDefaultFactions(playerId);
    }

    internal void DeleteFactionRelations(long factionId)
    {
      if (!Sync.IsServer)
        return;
      List<MyFactionCollection.MyRelatablePair> myRelatablePairList = new List<MyFactionCollection.MyRelatablePair>();
      foreach (KeyValuePair<MyFactionCollection.MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>> playersAndFaction in this.m_relationsBetweenPlayersAndFactions)
      {
        if (playersAndFaction.Key.RelateeId2 == factionId)
          myRelatablePairList.Add(playersAndFaction.Key);
      }
      foreach (MyFactionCollection.MyRelatablePair key in myRelatablePairList)
        this.m_relationsBetweenPlayersAndFactions.Remove(key);
      myRelatablePairList.Clear();
      foreach (KeyValuePair<MyFactionCollection.MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>> relationsBetweenFaction in this.m_relationsBetweenFactions)
      {
        if (relationsBetweenFaction.Key.RelateeId1 == factionId || relationsBetweenFaction.Key.RelateeId2 == factionId)
          myRelatablePairList.Add(relationsBetweenFaction.Key);
      }
      foreach (MyFactionCollection.MyRelatablePair key in myRelatablePairList)
        this.m_relationsBetweenFactions.Remove(key);
    }

    internal void DeletePlayerRelations(long identityId)
    {
      if (!Sync.IsServer)
        return;
      List<MyFactionCollection.MyRelatablePair> myRelatablePairList = new List<MyFactionCollection.MyRelatablePair>();
      foreach (KeyValuePair<MyFactionCollection.MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>> playersAndFaction in this.m_relationsBetweenPlayersAndFactions)
      {
        if (playersAndFaction.Key.RelateeId1 == identityId)
          myRelatablePairList.Add(playersAndFaction.Key);
      }
      foreach (MyFactionCollection.MyRelatablePair key in myRelatablePairList)
        this.m_relationsBetweenPlayersAndFactions.Remove(key);
    }

    public void CompatDefaultFactions(MyPlayer.PlayerId? playerId = null)
    {
      foreach (MyFactionDefinition factionDefinition in MyDefinitionManager.Static.GetFactionsFromDefinition())
      {
        MyFaction factionByTag = this.TryGetFactionByTag(factionDefinition.Tag, (IMyFaction) null);
        if (factionByTag != null)
        {
          if (playerId.HasValue && this.IsDiscoveredByDefault(factionDefinition.Tag) && !this.IsFactionDiscovered(playerId.Value, factionByTag.FactionId))
            this.AddDiscoveredFaction(playerId.Value, factionByTag.FactionId, false);
          if (!MyBankingSystem.Static.TryGetAccountInfo(factionByTag.FactionId, out MyAccountInfo _))
            MyBankingSystem.Static.CreateAccount(factionByTag.FactionId, factionDefinition.StartingBalance);
          if (factionByTag.FactionType != factionDefinition.Type)
            factionByTag.FactionType = factionDefinition.Type;
          if (!factionByTag.FactionIcon.HasValue && !string.IsNullOrEmpty(factionDefinition.FactionIcon.String))
          {
            factionByTag.FactionIcon = new MyStringId?(factionDefinition.FactionIcon);
            factionByTag.CustomColor = MyColorPickerConstants.HSVToHSVOffset(Vector3.Zero);
          }
        }
      }
    }

    public MyFaction TryGetFactionByTag(string tag, IMyFaction doNotCheck = null)
    {
      MyFaction myFaction;
      this.m_factionsByTag.TryGetValue(tag, out myFaction);
      if (myFaction == null)
        return (MyFaction) null;
      return doNotCheck != null && myFaction.FactionId == doNotCheck.FactionId ? (MyFaction) null : myFaction;
    }

    private void UnregisterFactionTag(MyFaction faction)
    {
      if (faction == null)
        return;
      this.m_factionsByTag.Remove(faction.Tag);
    }

    private void RegisterFactionTag(MyFaction faction)
    {
      if (faction == null)
        return;
      string tag = faction.Tag;
      this.m_factionsByTag.TryGetValue(tag, out MyFaction _);
      this.m_factionsByTag[tag] = faction;
    }

    public IMyFaction TryGetPlayerFaction(long playerId) => (IMyFaction) this.GetPlayerFaction(playerId);

    public MyFaction GetPlayerFaction(long playerId)
    {
      MyFaction myFaction = (MyFaction) null;
      long key;
      if (this.m_playerFaction.TryGetValue(playerId, out key))
        this.m_factions.TryGetValue(key, out myFaction);
      return myFaction;
    }

    public void AddPlayerToFaction(long playerId, long factionId)
    {
      MyFaction myFaction;
      if (this.m_factions.TryGetValue(factionId, out myFaction))
        myFaction.AcceptJoin(playerId, true);
      else
        this.AddPlayerToFactionInternal(playerId, factionId);
      foreach (KeyValuePair<long, MyFaction> faction in this.m_factions)
        faction.Value.CancelJoinRequest(playerId);
    }

    public void AddNewNPCToFaction(long factionId)
    {
      string npcName = this.m_factions[factionId].Tag + " NPC" + (object) MyRandom.Instance.Next(1000, 9999);
      this.AddNewNPCToFaction(factionId, npcName);
    }

    public void AddNewNPCToFaction(long factionId, string npcName) => this.AddPlayerToFaction(Sync.Players.CreateNewIdentity(npcName, addToNpcs: true).IdentityId, factionId);

    internal void AddPlayerToFactionInternal(long playerId, long factionId) => this.m_playerFaction[playerId] = factionId;

    public void KickPlayerFromFaction(long playerId) => this.m_playerFaction.Remove(playerId);

    public Tuple<MyRelationsBetweenFactions, int> GetRelationBetweenFactions(
      long factionId1,
      long factionId2)
    {
      return this.GetRelationBetweenFactions(factionId1, factionId2, MyPerGameSettings.DefaultFactionRelationshipAndReputation);
    }

    public Tuple<MyRelationsBetweenFactions, int> GetRelationBetweenFactions(
      long factionId1,
      long factionId2,
      Tuple<MyRelationsBetweenFactions, int> defaultState)
    {
      return factionId1 == factionId2 && factionId1 != 0L ? new Tuple<MyRelationsBetweenFactions, int>(MyRelationsBetweenFactions.Neutral, 0) : this.m_relationsBetweenFactions.GetValueOrDefault<MyFactionCollection.MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>>(new MyFactionCollection.MyRelatablePair(factionId1, factionId2), defaultState);
    }

    public Tuple<MyRelationsBetweenFactions, int> GetRelationBetweenPlayerAndFaction(
      long playerId,
      long factionId)
    {
      return this.GetRelationBetweenPlayerAndFaction(playerId, factionId, MyPerGameSettings.DefaultFactionRelationshipAndReputation);
    }

    public Tuple<MyRelationsBetweenFactions, int> GetRelationBetweenPlayerAndFaction(
      long playerId,
      long factionId,
      Tuple<MyRelationsBetweenFactions, int> defaultState)
    {
      return this.m_relationsBetweenPlayersAndFactions.GetValueOrDefault<MyFactionCollection.MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>>(new MyFactionCollection.MyRelatablePair(playerId, factionId), defaultState);
    }

    public bool AreFactionsEnemies(long factionId1, long factionId2) => this.GetRelationBetweenFactions(factionId1, factionId2).Item1 == MyRelationsBetweenFactions.Enemies;

    public bool AreFactionsNeutrals(long factionId1, long factionId2) => this.GetRelationBetweenFactions(factionId1, factionId2).Item1 == MyRelationsBetweenFactions.Neutral;

    public bool AreFactionsFriends(long factionId1, long factionId2) => this.GetRelationBetweenFactions(factionId1, factionId2).Item1 == MyRelationsBetweenFactions.Friends;

    public bool IsFactionWithPlayerEnemy(long playerId, long factionId) => this.GetRelationBetweenPlayerAndFaction(playerId, factionId).Item1 == MyRelationsBetweenFactions.Enemies;

    public bool IsFactionWithPlayerNeutral(long playerId, long factionId) => this.GetRelationBetweenPlayerAndFaction(playerId, factionId).Item1 == MyRelationsBetweenFactions.Neutral;

    public bool IsFactionWithPlayerFriend(long playerId, long factionId) => this.GetRelationBetweenPlayerAndFaction(playerId, factionId).Item1 == MyRelationsBetweenFactions.Friends;

    public MyFactionCollection.MyFactionPeaceRequestState GetRequestState(
      long myFactionId,
      long foreignFactionId)
    {
      if (this.m_factionRequests.ContainsKey(myFactionId) && this.m_factionRequests[myFactionId].Contains(foreignFactionId))
        return MyFactionCollection.MyFactionPeaceRequestState.Sent;
      return this.m_factionRequests.ContainsKey(foreignFactionId) && this.m_factionRequests[foreignFactionId].Contains(myFactionId) ? MyFactionCollection.MyFactionPeaceRequestState.Pending : MyFactionCollection.MyFactionPeaceRequestState.None;
    }

    public bool IsPeaceRequestStateSent(long myFactionId, long foreignFactionId) => this.GetRequestState(myFactionId, foreignFactionId) == MyFactionCollection.MyFactionPeaceRequestState.Sent;

    public bool IsPeaceRequestStatePending(long myFactionId, long foreignFactionId) => this.GetRequestState(myFactionId, foreignFactionId) == MyFactionCollection.MyFactionPeaceRequestState.Pending;

    public event Action<MyFactionStateChange, long, long, long, long> FactionStateChanged;

    public static void RemoveFaction(long factionId) => MyFactionCollection.SendFactionChange(MyFactionStateChange.RemoveFaction, factionId, factionId, 0L);

    public static void SendPeaceRequest(long fromFactionId, long toFactionId) => MyFactionCollection.SendFactionChange(MyFactionStateChange.SendPeaceRequest, fromFactionId, toFactionId, 0L);

    public static void CancelPeaceRequest(long fromFactionId, long toFactionId) => MyFactionCollection.SendFactionChange(MyFactionStateChange.CancelPeaceRequest, fromFactionId, toFactionId, 0L);

    public static void AcceptPeace(long fromFactionId, long toFactionId) => MyFactionCollection.SendFactionChange(MyFactionStateChange.AcceptPeace, fromFactionId, toFactionId, 0L);

    public static void DeclareWar(long fromFactionId, long toFactionId) => MyFactionCollection.SendFactionChange(MyFactionStateChange.DeclareWar, fromFactionId, toFactionId, 0L);

    public static void SendJoinRequest(long factionId, long playerId) => MyFactionCollection.SendFactionChange(MyFactionStateChange.FactionMemberSendJoin, factionId, factionId, playerId);

    public static void CancelJoinRequest(long factionId, long playerId) => MyFactionCollection.SendFactionChange(MyFactionStateChange.FactionMemberCancelJoin, factionId, factionId, playerId);

    public static void AcceptJoin(long factionId, long playerId) => MyFactionCollection.SendFactionChange(MyFactionStateChange.FactionMemberAcceptJoin, factionId, factionId, playerId);

    public static void KickMember(long factionId, long playerId) => MyFactionCollection.SendFactionChange(MyFactionStateChange.FactionMemberKick, factionId, factionId, playerId);

    public static void PromoteMember(long factionId, long playerId) => MyFactionCollection.SendFactionChange(MyFactionStateChange.FactionMemberPromote, factionId, factionId, playerId);

    public static void DemoteMember(long factionId, long playerId) => MyFactionCollection.SendFactionChange(MyFactionStateChange.FactionMemberDemote, factionId, factionId, playerId);

    public static void MemberLeaves(long factionId, long playerId) => MyFactionCollection.SendFactionChange(MyFactionStateChange.FactionMemberLeave, factionId, factionId, playerId);

    private bool CheckFactionStateChange(
      MyFactionStateChange action,
      long fromFactionId,
      long toFactionId,
      long playerId,
      long senderId)
    {
      if (!Sync.IsServer || !this.m_factions.ContainsKey(fromFactionId) || !this.m_factions.ContainsKey(toFactionId))
        return false;
      if (senderId != 0L)
      {
        switch (action)
        {
          case MyFactionStateChange.RemoveFaction:
          case MyFactionStateChange.SendPeaceRequest:
          case MyFactionStateChange.CancelPeaceRequest:
          case MyFactionStateChange.AcceptPeace:
          case MyFactionStateChange.DeclareWar:
          case MyFactionStateChange.SendFriendRequest:
          case MyFactionStateChange.CancelFriendRequest:
          case MyFactionStateChange.AcceptFriendRequest:
          case MyFactionStateChange.FactionMemberAcceptJoin:
          case MyFactionStateChange.FactionMemberKick:
          case MyFactionStateChange.FactionMemberPromote:
          case MyFactionStateChange.FactionMemberDemote:
            if (!this.m_factions[fromFactionId].IsLeader(senderId) && !MySession.Static.IsUserAdmin(MySession.Static.Players.TryGetSteamId(senderId)))
            {
              MyLog.Default.Warning("Player is attempting a faction state change they have no rights to do - {0}", (object) senderId);
              return false;
            }
            break;
          case MyFactionStateChange.FactionMemberSendJoin:
          case MyFactionStateChange.FactionMemberCancelJoin:
          case MyFactionStateChange.FactionMemberLeave:
            if (playerId != senderId && !MySession.Static.IsUserAdmin(MySession.Static.Players.TryGetSteamId(senderId)))
            {
              MyLog.Default.Warning("Player is attempting a faction state change they have no rights to do - {0}", (object) senderId);
              return false;
            }
            break;
          case MyFactionStateChange.FactionMemberNotPossibleJoin:
            break;
          default:
            return false;
        }
      }
      switch (action)
      {
        case MyFactionStateChange.RemoveFaction:
          return true;
        case MyFactionStateChange.SendPeaceRequest:
          HashSet<long> longSet1;
          return (!this.m_factionRequests.TryGetValue(fromFactionId, out longSet1) || !longSet1.Contains(toFactionId)) && this.GetRelationBetweenFactions(fromFactionId, toFactionId).Item1 == MyRelationsBetweenFactions.Enemies;
        case MyFactionStateChange.CancelPeaceRequest:
          HashSet<long> longSet2;
          return this.m_factionRequests.TryGetValue(fromFactionId, out longSet2) && longSet2.Contains(toFactionId) && this.GetRelationBetweenFactions(fromFactionId, toFactionId).Item1 == MyRelationsBetweenFactions.Enemies;
        case MyFactionStateChange.AcceptPeace:
          return (uint) this.GetRelationBetweenFactions(fromFactionId, toFactionId).Item1 > 0U;
        case MyFactionStateChange.DeclareWar:
          return this.GetRelationBetweenFactions(fromFactionId, toFactionId).Item1 != MyRelationsBetweenFactions.Enemies;
        case MyFactionStateChange.SendFriendRequest:
          HashSet<long> longSet3;
          return (!this.m_factionRequests.TryGetValue(fromFactionId, out longSet3) || !longSet3.Contains(toFactionId)) && this.GetRelationBetweenFactions(fromFactionId, toFactionId).Item1 == MyRelationsBetweenFactions.Neutral;
        case MyFactionStateChange.CancelFriendRequest:
          HashSet<long> longSet4;
          return this.m_factionRequests.TryGetValue(fromFactionId, out longSet4) && longSet4.Contains(toFactionId) && this.GetRelationBetweenFactions(fromFactionId, toFactionId).Item1 == MyRelationsBetweenFactions.Neutral;
        case MyFactionStateChange.AcceptFriendRequest:
          return this.GetRelationBetweenFactions(fromFactionId, toFactionId).Item1 == MyRelationsBetweenFactions.Friends;
        case MyFactionStateChange.FactionMemberSendJoin:
          return (MySession.Static.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.PER_FACTION || !this.m_factions[fromFactionId].IsMember(playerId)) && !this.m_factions[fromFactionId].JoinRequests.ContainsKey(playerId);
        case MyFactionStateChange.FactionMemberCancelJoin:
          return !this.m_factions[fromFactionId].IsMember(playerId) && this.m_factions[fromFactionId].JoinRequests.ContainsKey(playerId);
        case MyFactionStateChange.FactionMemberAcceptJoin:
          return this.m_factions[fromFactionId].JoinRequests.ContainsKey(playerId);
        case MyFactionStateChange.FactionMemberKick:
          return this.m_factions[fromFactionId].IsMember(playerId);
        case MyFactionStateChange.FactionMemberPromote:
          return this.m_factions[fromFactionId].IsMember(playerId);
        case MyFactionStateChange.FactionMemberDemote:
          return this.m_factions[fromFactionId].IsLeader(playerId);
        case MyFactionStateChange.FactionMemberLeave:
          ulong steamId = MySession.Static.Players.TryGetSteamId(playerId);
          if (!this.m_factions[fromFactionId].IsMember(playerId))
            return false;
          return !MySession.Static.Settings.EnableTeamBalancing || MySession.Static.IsUserSpaceMaster(steamId);
        default:
          return false;
      }
    }

    private void ApplyFactionStateChange(
      MyFactionStateChange action,
      long fromFactionId,
      long toFactionId,
      long playerId,
      long senderId)
    {
      switch (action)
      {
        case MyFactionStateChange.RemoveFaction:
          if (this.m_factions[fromFactionId].IsMember(MySession.Static.LocalPlayerId))
            this.m_playerFaction.Remove(playerId);
          foreach (KeyValuePair<long, MyFaction> faction in this.m_factions)
          {
            if (faction.Key != fromFactionId)
            {
              this.ClearRequest(fromFactionId, faction.Key);
              this.RemoveRelation(fromFactionId, faction.Key);
            }
          }
          MyFaction faction1 = (MyFaction) null;
          this.m_factions.TryGetValue(fromFactionId, out faction1);
          this.UnregisterFactionTag(faction1);
          this.m_factions.Remove(fromFactionId);
          this.DeleteFactionRelations(fromFactionId);
          this.RemoveFactionFromVisibility(fromFactionId);
          break;
        case MyFactionStateChange.SendPeaceRequest:
        case MyFactionStateChange.SendFriendRequest:
          HashSet<long> longSet;
          if (this.m_factionRequests.TryGetValue(fromFactionId, out longSet))
          {
            longSet.Add(toFactionId);
            break;
          }
          longSet = new HashSet<long>();
          longSet.Add(toFactionId);
          this.m_factionRequests.Add(fromFactionId, longSet);
          break;
        case MyFactionStateChange.CancelPeaceRequest:
        case MyFactionStateChange.CancelFriendRequest:
          this.ClearRequest(fromFactionId, toFactionId);
          break;
        case MyFactionStateChange.AcceptPeace:
          this.ClearRequest(fromFactionId, toFactionId);
          this.ChangeFactionRelation(fromFactionId, toFactionId, MyRelationsBetweenFactions.Neutral);
          break;
        case MyFactionStateChange.DeclareWar:
          this.ClearRequest(fromFactionId, toFactionId);
          this.ChangeFactionRelation(fromFactionId, toFactionId, MyRelationsBetweenFactions.Enemies);
          break;
        case MyFactionStateChange.AcceptFriendRequest:
          this.ClearRequest(fromFactionId, toFactionId);
          this.ChangeFactionRelation(fromFactionId, toFactionId, MyRelationsBetweenFactions.Friends);
          break;
        case MyFactionStateChange.FactionMemberSendJoin:
          this.m_factions[fromFactionId].AddJoinRequest(playerId);
          break;
        case MyFactionStateChange.FactionMemberCancelJoin:
          this.m_factions[fromFactionId].CancelJoinRequest(playerId);
          break;
        case MyFactionStateChange.FactionMemberAcceptJoin:
          bool autoaccept = MySession.Static.IsUserSpaceMaster(MySession.Static.Players.TryGetSteamId(playerId)) || this.m_factions[fromFactionId].Members.Count == 0;
          if (autoaccept && this.m_factions[fromFactionId].IsEveryoneNpc())
          {
            this.m_factions[fromFactionId].AcceptJoin(playerId, autoaccept);
            this.m_factions[fromFactionId].PromoteMember(playerId);
            break;
          }
          this.m_factions[fromFactionId].AcceptJoin(playerId, autoaccept);
          break;
        case MyFactionStateChange.FactionMemberKick:
          if (Sync.IsServer && playerId != this.m_factions[fromFactionId].FounderId && MySession.Static.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.PER_FACTION)
            MyBlockLimits.TransferBlockLimits(playerId, this.m_factions[fromFactionId].FounderId);
          this.m_factions[fromFactionId].KickMember(playerId);
          break;
        case MyFactionStateChange.FactionMemberPromote:
          this.m_factions[fromFactionId].PromoteMember(playerId);
          break;
        case MyFactionStateChange.FactionMemberDemote:
          this.m_factions[fromFactionId].DemoteMember(playerId);
          break;
        case MyFactionStateChange.FactionMemberLeave:
          this.m_factions[fromFactionId].KickMember(playerId);
          break;
      }
    }

    private void ClearRequest(long fromFactionId, long toFactionId)
    {
      if (this.m_factionRequests.ContainsKey(fromFactionId))
        this.m_factionRequests[fromFactionId].Remove(toFactionId);
      if (!this.m_factionRequests.ContainsKey(toFactionId))
        return;
      this.m_factionRequests[toFactionId].Remove(fromFactionId);
    }

    private void ChangeFactionRelation(
      long fromFactionId,
      long toFactionId,
      MyRelationsBetweenFactions relation)
    {
      int reputation = this.TranslateRelationToReputation(relation);
      this.m_relationsBetweenFactions[new MyFactionCollection.MyRelatablePair(fromFactionId, toFactionId)] = new Tuple<MyRelationsBetweenFactions, int>(relation, reputation);
      foreach (KeyValuePair<long, MyFactionMember> member in this.TryGetFactionById(fromFactionId).Members)
        this.SetReputationBetweenPlayerAndFaction(member.Key, toFactionId, reputation);
      foreach (KeyValuePair<long, MyFactionMember> member in this.TryGetFactionById(toFactionId).Members)
        this.SetReputationBetweenPlayerAndFaction(member.Key, fromFactionId, reputation);
    }

    private void ChangeReputationBetweenFactions(
      long fromFactionId,
      long toFactionId,
      int reputation)
    {
      this.m_relationsBetweenFactions[new MyFactionCollection.MyRelatablePair(fromFactionId, toFactionId)] = new Tuple<MyRelationsBetweenFactions, int>(this.TranslateReputationToRelation(reputation), reputation);
    }

    public void SetReputationBetweenFactions(long fromFactionId, long toFactionId, int reputation) => this.ChangeReputationBetweenFactions(fromFactionId, toFactionId, reputation);

    public void SetReputationBetweenPlayerAndFaction(
      long identityId,
      long factionId,
      int reputation)
    {
      this.ChangeReputationWithPlayer(identityId, factionId, reputation);
    }

    public DictionaryReader<MyFactionCollection.MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>> GetAllFactionRelations() => new DictionaryReader<MyFactionCollection.MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>>(this.m_relationsBetweenFactions);

    public int TranslateRelationToReputation(MyRelationsBetweenFactions relation)
    {
      MySessionComponentEconomy component = MySession.Static?.GetComponent<MySessionComponentEconomy>();
      return component != null ? component.TranslateRelationToReputation(relation) : MyPerGameSettings.DefaultFactionReputation;
    }

    public MyRelationsBetweenFactions TranslateReputationToRelation(
      int reputation)
    {
      MySessionComponentEconomy component = MySession.Static?.GetComponent<MySessionComponentEconomy>();
      return component != null ? component.TranslateReputationToRelationship(reputation) : MyPerGameSettings.DefaultFactionRelationship;
    }

    public int ClampReputation(int reputation)
    {
      MySessionComponentEconomy component = MySession.Static?.GetComponent<MySessionComponentEconomy>();
      return component != null ? component.ClampReputation(reputation) : reputation;
    }

    private void ChangeRelationWithPlayer(
      long fromPlayerId,
      long toFactionId,
      MyRelationsBetweenFactions relation)
    {
      this.m_relationsBetweenPlayersAndFactions[new MyFactionCollection.MyRelatablePair(fromPlayerId, toFactionId)] = new Tuple<MyRelationsBetweenFactions, int>(relation, this.TranslateRelationToReputation(relation));
    }

    private void ChangeReputationWithPlayer(long fromPlayerId, long toFactionId, int reputation)
    {
      MyFactionCollection.MyRelatablePair key = new MyFactionCollection.MyRelatablePair(fromPlayerId, toFactionId);
      Tuple<MyRelationsBetweenFactions, int> tuple = new Tuple<MyRelationsBetweenFactions, int>(this.TranslateReputationToRelation(reputation), reputation);
      if (this.m_relationsBetweenPlayersAndFactions.ContainsKey(key))
      {
        Tuple<MyRelationsBetweenFactions, int> playersAndFaction = this.m_relationsBetweenPlayersAndFactions[key];
        this.m_relationsBetweenPlayersAndFactions[key] = tuple;
        if (playersAndFaction.Item1 == tuple.Item1)
          return;
        this.PlayerReputationLevelChanged(fromPlayerId, toFactionId, playersAndFaction.Item1, tuple.Item1);
      }
      else
        this.m_relationsBetweenPlayersAndFactions[key] = tuple;
    }

    private void PlayerReputationLevelChanged(
      long fromPlayerId,
      long toFactionId,
      MyRelationsBetweenFactions oldRel,
      MyRelationsBetweenFactions newRel)
    {
      this.CheckPlayerReputationAchievements(fromPlayerId, toFactionId, oldRel, newRel);
    }

    private void CheckPlayerReputationAchievements(
      long fromPlayerId,
      long toFactionId,
      MyRelationsBetweenFactions oldRel,
      MyRelationsBetweenFactions newRel)
    {
      if (newRel != MyRelationsBetweenFactions.Friends || oldRel != MyRelationsBetweenFactions.Neutral)
        return;
      int num = 0;
      foreach (KeyValuePair<long, MyFaction> keyValuePair in this)
      {
        MyFactionCollection.MyRelatablePair key = new MyFactionCollection.MyRelatablePair(fromPlayerId, keyValuePair.Key);
        if (keyValuePair.Value.FactionType != MyFactionTypes.PlayerMade && this.m_relationsBetweenPlayersAndFactions.ContainsKey(key) && this.m_relationsBetweenPlayersAndFactions[key].Item1 == MyRelationsBetweenFactions.Friends)
          ++num;
      }
      if (num != 3)
        return;
      if (Sync.IsServer)
        MyMultiplayer.RaiseStaticEvent<string>((Func<IMyEventOwner, Action<string>>) (x => new Action<string>(MyFactionCollection.UnlockAchievementForClient)), "FriendOfFactions", new EndpointId(MySession.Static.Players.TryGetSteamId(fromPlayerId)));
      else
        MyFactionCollection.UnlockAchievement_Internal("FriendOfFactions");
    }

    [Event(null, 1004)]
    [Reliable]
    [Client]
    private static void UnlockAchievementForClient(string achievement) => MyFactionCollection.UnlockAchievement_Internal(achievement);

    private static void UnlockAchievement_Internal(string achievement) => MyGameService.GetAchievement(achievement, (string) null, 0.0f).Unlock();

    public bool HasRelationWithPlayer(long fromPlayerId, long toFactionId) => this.m_relationsBetweenPlayersAndFactions.ContainsKey(new MyFactionCollection.MyRelatablePair(fromPlayerId, toFactionId));

    private void RemoveRelation(long fromFactionId, long toFactionId) => this.m_relationsBetweenFactions.Remove(new MyFactionCollection.MyRelatablePair(fromFactionId, toFactionId));

    public bool AddFactionPlayerReputation(
      long playerIdentityId,
      long factionId,
      int delta,
      bool propagate = true,
      bool adminChange = false)
    {
      if (!Sync.IsServer)
        return false;
      List<MyFactionCollection.MyReputationChangeWrapper> changes = this.GenerateChanges(playerIdentityId, factionId, delta, propagate, adminChange);
      MyMultiplayer.RaiseStaticEvent<long, List<MyFactionCollection.MyReputationChangeWrapper>>((Func<IMyEventOwner, Action<long, List<MyFactionCollection.MyReputationChangeWrapper>>>) (x => new Action<long, List<MyFactionCollection.MyReputationChangeWrapper>>(MyFactionCollection.AddFactionPlayerReputationSuccess)), playerIdentityId, changes);
      MyFactionCollection.AddFactionPlayerReputationSuccess(playerIdentityId, changes);
      return true;
    }

    public void DamageFactionPlayerReputation(
      long playerIdentityId,
      long attackedIdentityId,
      MyReputationDamageType repDamageType)
    {
      if (!Sync.IsServer || attackedIdentityId == 0L)
        return;
      if (MySession.Static == null || MySession.Static.Factions == null)
      {
        MyLog.Default.Error("Session.Static or MySession.Static.Factions is null. Should not happen!");
      }
      else
      {
        MyFaction playerFaction1 = MySession.Static.Factions.GetPlayerFaction(MyPirateAntennas.GetPiratesId());
        if (!(this.TryGetPlayerFaction(attackedIdentityId) is MyFaction playerFaction) && playerFaction1 != null)
        {
          int reputationDamageDelta = this.GetReputationDamageDelta(repDamageType, true);
          this.AddFactionPlayerReputation(playerIdentityId, playerFaction1.FactionId, reputationDamageDelta, false);
        }
        else
        {
          if (playerFaction == null || playerFaction.IsMember(playerIdentityId))
            return;
          int reputationDamageDelta = this.GetReputationDamageDelta(repDamageType, playerFaction1 == playerFaction);
          this.AddFactionPlayerReputation(playerIdentityId, playerFaction.FactionId, -reputationDamageDelta, false);
          if (playerFaction1 == null || playerFaction == playerFaction1)
            return;
          this.AddFactionPlayerReputation(playerIdentityId, playerFaction1.FactionId, reputationDamageDelta, false);
        }
      }
    }

    private int GetReputationDamageDelta(MyReputationDamageType repDamageType, bool isPirates = false)
    {
      MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings reputationDamageSettings = isPirates ? this.m_reputationSettings.PirateDamageSettings : this.m_reputationSettings.DamageSettings;
      int num = 0;
      switch (repDamageType)
      {
        case MyReputationDamageType.GrindingWelding:
          num = reputationDamageSettings.GrindingWelding;
          break;
        case MyReputationDamageType.Damaging:
          num = reputationDamageSettings.Damaging;
          break;
        case MyReputationDamageType.Stealing:
          num = reputationDamageSettings.Stealing;
          break;
        case MyReputationDamageType.Killing:
          num = reputationDamageSettings.Killing;
          break;
        default:
          MyLog.Default.Error("Reputation damage type not handled. Check and update.");
          break;
      }
      return num;
    }

    [Event(null, 1140)]
    [Reliable]
    [Server]
    public static void Invoke_AddRep_DEBUG(long playerId, long factionId, int delta)
    {
    }

    private List<MyFactionCollection.MyReputationChangeWrapper> GenerateChanges(
      long playerId,
      long factionId,
      int delta,
      bool propagate,
      bool adminChange = false)
    {
      List<MyFactionCollection.MyReputationChangeWrapper> reputationChangeWrapperList1 = new List<MyFactionCollection.MyReputationChangeWrapper>();
      MyFactionCollection.MyReputationModifiers reputationModifiers = this.GetReputationModifiers();
      IMyFaction factionById = MySession.Static.Factions.TryGetFactionById(factionId);
      if (factionById != null)
      {
        Tuple<MyRelationsBetweenFactions, int> playerAndFaction1 = this.GetRelationBetweenPlayerAndFaction(playerId, factionId);
        int num1 = this.ClampReputation(playerAndFaction1.Item2 + (int) ((double) reputationModifiers.Owner * (double) delta));
        int clampedDelta1 = num1 - playerAndFaction1.Item2;
        bool flag1 = !adminChange && this.CheckIfMaxPirateRep(playerId, factionById, clampedDelta1);
        MyFactionCollection.MyReputationChangeWrapper reputationChangeWrapper1;
        if (!flag1)
        {
          bool flag2 = !MyCampaignManager.Static.IsCampaignRunning;
          List<MyFactionCollection.MyReputationChangeWrapper> reputationChangeWrapperList2 = reputationChangeWrapperList1;
          reputationChangeWrapper1 = new MyFactionCollection.MyReputationChangeWrapper();
          reputationChangeWrapper1.FactionId = factionId;
          reputationChangeWrapper1.RepTotal = num1;
          reputationChangeWrapper1.Change = clampedDelta1;
          reputationChangeWrapper1.ShowNotification = flag2;
          MyFactionCollection.MyReputationChangeWrapper reputationChangeWrapper2 = reputationChangeWrapper1;
          reputationChangeWrapperList2.Add(reputationChangeWrapper2);
        }
        if (!propagate | flag1)
          return reputationChangeWrapperList1;
        foreach (KeyValuePair<long, MyFaction> keyValuePair in this)
        {
          if (keyValuePair.Value.FactionId != factionId && keyValuePair.Value.FactionType != MyFactionTypes.None && keyValuePair.Value.FactionType != MyFactionTypes.PlayerMade)
          {
            int num2;
            switch (this.GetRelationBetweenFactions(factionId, keyValuePair.Value.FactionId).Item1)
            {
              case MyRelationsBetweenFactions.Neutral:
                num2 = (int) ((double) reputationModifiers.Neutral * (double) delta);
                break;
              case MyRelationsBetweenFactions.Enemies:
                num2 = (int) ((double) reputationModifiers.Hostile * (double) delta);
                break;
              case MyRelationsBetweenFactions.Friends:
                num2 = (int) ((double) reputationModifiers.Friend * (double) delta);
                break;
              default:
                continue;
            }
            Tuple<MyRelationsBetweenFactions, int> playerAndFaction2 = this.GetRelationBetweenPlayerAndFaction(playerId, keyValuePair.Value.FactionId);
            int num3 = this.ClampReputation(playerAndFaction2.Item2 + num2);
            int clampedDelta2 = num3 - playerAndFaction2.Item2;
            bool flag2 = !adminChange && this.CheckIfMaxPirateRep(playerId, (IMyFaction) keyValuePair.Value, clampedDelta2);
            if (clampedDelta2 != 0 && !flag2)
            {
              List<MyFactionCollection.MyReputationChangeWrapper> reputationChangeWrapperList2 = reputationChangeWrapperList1;
              reputationChangeWrapper1 = new MyFactionCollection.MyReputationChangeWrapper();
              reputationChangeWrapper1.FactionId = keyValuePair.Value.FactionId;
              reputationChangeWrapper1.RepTotal = num3;
              reputationChangeWrapper1.Change = clampedDelta2;
              reputationChangeWrapper1.ShowNotification = false;
              MyFactionCollection.MyReputationChangeWrapper reputationChangeWrapper2 = reputationChangeWrapper1;
              reputationChangeWrapperList2.Add(reputationChangeWrapper2);
            }
          }
        }
      }
      return reputationChangeWrapperList1;
    }

    private bool CheckIfMaxPirateRep(long playerId, IMyFaction faction, int clampedDelta)
    {
      MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(MyPirateAntennas.GetPiratesId());
      if (clampedDelta <= 0 || faction != playerFaction)
        return false;
      Tuple<int, TimeSpan> tuple;
      if (!this.m_playerToReputationLimits.TryGetValue(playerId, out tuple))
      {
        tuple = new Tuple<int, TimeSpan>(clampedDelta, MySession.Static.ElapsedGameTime + TimeSpan.FromMinutes((double) this.m_reputationSettings.ResetTimeMinForRepGain));
        this.m_playerToReputationLimits.Add(playerId, tuple);
        return false;
      }
      if (tuple.Item2 > MySession.Static.ElapsedGameTime)
      {
        if (tuple.Item1 >= this.m_reputationSettings.MaxReputationGainInTime)
          return true;
        int num = Math.Min(tuple.Item1 + clampedDelta, this.m_reputationSettings.MaxReputationGainInTime);
        this.m_playerToReputationLimits[playerId] = new Tuple<int, TimeSpan>(num, tuple.Item2);
        return false;
      }
      int num1 = Math.Min(clampedDelta, this.m_reputationSettings.MaxReputationGainInTime);
      this.m_playerToReputationLimits[playerId] = new Tuple<int, TimeSpan>(num1, MySession.Static.ElapsedGameTime + TimeSpan.FromMinutes((double) this.m_reputationSettings.ResetTimeMinForRepGain));
      return false;
    }

    private MyFactionCollection.MyReputationModifiers GetReputationModifiers(
      bool positive = true)
    {
      MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
      return component == null ? new MyFactionCollection.MyReputationModifiers() : component.GetReputationModifiers(positive);
    }

    [Event(null, 1265)]
    [Reliable]
    [Broadcast]
    private static void AddFactionPlayerReputationSuccess(
      long playerId,
      List<MyFactionCollection.MyReputationChangeWrapper> changes)
    {
      MyFactionCollection factions = MySession.Static.Factions;
      bool flag = !Sandbox.Engine.Platform.Game.IsDedicated && playerId == MySession.Static.LocalPlayerId;
      foreach (MyFactionCollection.MyReputationChangeWrapper change in changes)
      {
        factions.ChangeReputationWithPlayer(playerId, change.FactionId, change.RepTotal);
        if (change.ShowNotification & flag)
          MySession.Static.Factions.DisplayReputationChangeNotification((MySession.Static.Factions.TryGetFactionById(change.FactionId) as MyFaction).Tag, change.Change);
      }
    }

    private void DisplayReputationChangeNotification(string tag, int value)
    {
      if (value > 0)
      {
        this.m_notificationRepInc.UpdateReputationNotification(in tag, in value);
      }
      else
      {
        if (value >= 0)
          return;
        this.m_notificationRepDec.UpdateReputationNotification(in tag, in value);
      }
    }

    private static void SendFactionChange(
      MyFactionStateChange action,
      long fromFactionId,
      long toFactionId,
      long playerId)
    {
      MyMultiplayer.RaiseStaticEvent<MyFactionStateChange, long, long, long>((Func<IMyEventOwner, Action<MyFactionStateChange, long, long, long>>) (s => new Action<MyFactionStateChange, long, long, long>(MyFactionCollection.FactionStateChangeRequest)), action, fromFactionId, toFactionId, playerId);
    }

    [Event(null, 1300)]
    [Reliable]
    [Server]
    private static void FactionStateChangeRequest(
      MyFactionStateChange action,
      long fromFactionId,
      long toFactionId,
      long playerId)
    {
      IMyFaction factionById1 = MySession.Static.Factions.TryGetFactionById(fromFactionId);
      IMyFaction factionById2 = MySession.Static.Factions.TryGetFactionById(toFactionId);
      long senderId = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value, 0);
      if (factionById1 == null || factionById2 == null || !MySession.Static.Factions.CheckFactionStateChange(action, fromFactionId, toFactionId, playerId, senderId))
        return;
      if ((action == MyFactionStateChange.FactionMemberKick || action == MyFactionStateChange.FactionMemberLeave) && (factionById1.Members.Count == 1 && MySession.Static.BlockLimitsEnabled != MyBlockLimitsEnabledEnum.PER_FACTION))
      {
        action = MyFactionStateChange.RemoveFaction;
      }
      else
      {
        switch (action)
        {
          case MyFactionStateChange.FactionMemberSendJoin:
            ulong steamId = MySession.Static.Players.TryGetSteamId(playerId);
            bool flag = MySession.Static.IsUserSpaceMaster(steamId);
            if (factionById2.AutoAcceptMember || factionById2.Members.Count == 0)
            {
              flag = true;
              if (!factionById2.AcceptHumans && steamId != 0UL && MySession.Static.Players.TryGetSerialId(playerId) == 0)
              {
                flag = false;
                action = MyFactionStateChange.FactionMemberCancelJoin;
              }
            }
            if (MySession.Static.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.PER_FACTION && !MyBlockLimits.IsFactionChangePossible(playerId, factionById2.FactionId))
            {
              flag = false;
              action = MyFactionStateChange.FactionMemberNotPossibleJoin;
            }
            if (flag)
            {
              action = MyFactionStateChange.FactionMemberAcceptJoin;
              break;
            }
            break;
          case MyFactionStateChange.FactionMemberAcceptJoin:
            if (!MyBlockLimits.IsFactionChangePossible(playerId, factionById2.FactionId))
            {
              action = MyFactionStateChange.FactionMemberNotPossibleJoin;
              break;
            }
            break;
          default:
            if (action == MyFactionStateChange.SendPeaceRequest && factionById2.AutoAcceptPeace)
            {
              action = MyFactionStateChange.AcceptPeace;
              senderId = 0L;
              break;
            }
            break;
        }
      }
      if (action == MyFactionStateChange.RemoveFaction)
        MyBankingSystem.Static.RemoveAccount(toFactionId);
      MyMultiplayer.RaiseStaticEvent<MyFactionStateChange, long, long, long, long>((Func<IMyEventOwner, Action<MyFactionStateChange, long, long, long, long>>) (s => new Action<MyFactionStateChange, long, long, long, long>(MyFactionCollection.FactionStateChangeSuccess)), action, fromFactionId, toFactionId, playerId, senderId);
    }

    [Event(null, 1375)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    private static void FactionStateChangeSuccess(
      MyFactionStateChange action,
      long fromFactionId,
      long toFactionId,
      long playerId,
      long senderId)
    {
      IMyFaction factionById1 = MySession.Static.Factions.TryGetFactionById(fromFactionId);
      IMyFaction factionById2 = MySession.Static.Factions.TryGetFactionById(toFactionId);
      if (factionById1 == null || factionById2 == null)
        return;
      MySession.Static.Factions.ApplyFactionStateChange(action, fromFactionId, toFactionId, playerId, senderId);
      Action<MyFactionStateChange, long, long, long, long> factionStateChanged = MySession.Static.Factions.FactionStateChanged;
      if (factionStateChanged == null)
        return;
      factionStateChanged(action, fromFactionId, toFactionId, playerId, senderId);
    }

    internal List<MyObjectBuilder_Faction> SaveFactions()
    {
      List<MyObjectBuilder_Faction> objectBuilderFactionList = new List<MyObjectBuilder_Faction>();
      foreach (KeyValuePair<long, MyFaction> faction in this.m_factions)
      {
        MyObjectBuilder_Faction objectBuilder = faction.Value.GetObjectBuilder();
        objectBuilderFactionList.Add(objectBuilder);
      }
      return objectBuilderFactionList;
    }

    internal void LoadFactions(List<MyObjectBuilder_Faction> factionBuilders, bool removeOldData = true)
    {
      if (removeOldData)
        this.Clear();
      if (factionBuilders == null)
        return;
      foreach (MyObjectBuilder_Faction factionBuilder in factionBuilders)
      {
        if (!this.m_factions.ContainsKey(factionBuilder.FactionId))
        {
          MyFaction faction = new MyFaction(factionBuilder);
          this.Add(faction);
          foreach (KeyValuePair<long, MyFactionMember> member in faction.Members)
            this.AddPlayerToFaction(member.Value.PlayerId, faction.FactionId);
        }
      }
    }

    public void InvokePlayerJoined(MyFaction faction, long identityId) => this.OnPlayerJoined.InvokeIfNotNull<MyFaction, long>(faction, identityId);

    public void InvokePlayerLeft(MyFaction faction, long identityId) => this.OnPlayerLeft.InvokeIfNotNull<MyFaction, long>(faction, identityId);

    public MyStation GetStationByGridId(long gridEntityId)
    {
      foreach (KeyValuePair<long, MyFaction> keyValuePair in this)
      {
        foreach (MyStation station in keyValuePair.Value.Stations)
        {
          if (station.StationEntityId == gridEntityId)
            return station;
        }
      }
      return (MyStation) null;
    }

    internal MyStation GetStationByStationId(long stationId)
    {
      foreach (KeyValuePair<long, MyFaction> keyValuePair in this)
      {
        MyStation stationById = keyValuePair.Value.GetStationById(stationId);
        if (stationById != null)
          return stationById;
      }
      return (MyStation) null;
    }

    public event Action<long, bool, bool> FactionAutoAcceptChanged;

    public void ChangeAutoAccept(long factionId, bool autoAcceptMember, bool autoAcceptPeace) => MyMultiplayer.RaiseStaticEvent<long, bool, bool>((Func<IMyEventOwner, Action<long, bool, bool>>) (s => new Action<long, bool, bool>(MyFactionCollection.ChangeAutoAcceptRequest)), factionId, autoAcceptMember, autoAcceptPeace);

    [Event(null, 1493)]
    [Reliable]
    [Server]
    private static void ChangeAutoAcceptRequest(
      long factionId,
      bool autoAcceptMember,
      bool autoAcceptPeace)
    {
      IMyFaction factionById = MySession.Static.Factions.TryGetFactionById(factionId);
      ulong num = MyEventContext.Current.Sender.Value;
      long identityId = MySession.Static.Players.TryGetIdentityId(num, 0);
      if (factionById != null && (factionById.IsLeader(identityId) || num != 0UL && MySession.Static.IsUserAdmin(num)))
      {
        MyMultiplayer.RaiseStaticEvent<long, bool, bool>((Func<IMyEventOwner, Action<long, bool, bool>>) (s => new Action<long, bool, bool>(MyFactionCollection.ChangeAutoAcceptSuccess)), factionId, autoAcceptMember, autoAcceptPeace);
      }
      else
      {
        if (MyEventContext.Current.IsLocallyInvoked)
          return;
        ((MyMultiplayerServerBase) MyMultiplayer.Static).ValidationFailed(num, true, (string) null, true);
      }
    }

    [Event(null, 1510)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    private static void ChangeAutoAcceptSuccess(
      long factionId,
      bool autoAcceptMember,
      bool autoAcceptPeace)
    {
      MyFactionCollection factions = MySession.Static.Factions;
      if (!(factions.TryGetFactionById(factionId) is MyFaction factionById))
        return;
      factionById.AutoAcceptPeace = autoAcceptPeace;
      factionById.AutoAcceptMember = autoAcceptMember;
      factions.FactionAutoAcceptChanged.InvokeIfNotNull<long, bool, bool>(factionId, autoAcceptMember, autoAcceptPeace);
    }

    public event Action<long> FactionEdited;

    public void EditFaction(
      long factionId,
      string tag,
      string name,
      string desc,
      string privateInfo,
      SerializableDefinitionId? factionIconGroupId = null,
      int factionIconId = 0,
      Vector3 factionColor = default (Vector3),
      Vector3 factionIconColor = default (Vector3),
      int score = 0,
      float objectivePercentage = 0.0f)
    {
      MyMultiplayer.RaiseStaticEvent<MyFactionCollection.AddFactionMsg>((Func<IMyEventOwner, Action<MyFactionCollection.AddFactionMsg>>) (s => new Action<MyFactionCollection.AddFactionMsg>(MyFactionCollection.EditFactionRequest)), new MyFactionCollection.AddFactionMsg()
      {
        FactionId = factionId,
        FactionTag = tag,
        FactionName = name,
        FactionDescription = string.IsNullOrEmpty(desc) ? string.Empty : (desc.Length > 512 ? desc.Substring(0, 512) : desc),
        FactionPrivateInfo = string.IsNullOrEmpty(privateInfo) ? string.Empty : (privateInfo.Length > 512 ? privateInfo.Substring(0, 512) : privateInfo),
        FactionColor = (SerializableVector3) factionColor,
        FactionIconColor = (SerializableVector3) factionIconColor,
        FactionIconGroupId = factionIconGroupId,
        FactionIconId = factionIconId,
        FactionScore = score,
        FactionObjectivePercentageCompleted = objectivePercentage
      });
    }

    [Event(null, 1551)]
    [Reliable]
    [Server]
    private static void EditFactionRequest(MyFactionCollection.AddFactionMsg msgEdit)
    {
      IMyFaction factionById = MySession.Static.Factions.TryGetFactionById(msgEdit.FactionId);
      long identityId = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value, 0);
      if (factionById != null && !MySession.Static.Factions.FactionTagExists(msgEdit.FactionTag, factionById) && !MySession.Static.Factions.FactionNameExists(msgEdit.FactionName, factionById) && (factionById.IsLeader(identityId) || MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value)))
      {
        MyMultiplayer.RaiseStaticEvent<MyFactionCollection.AddFactionMsg>((Func<IMyEventOwner, Action<MyFactionCollection.AddFactionMsg>>) (s => new Action<MyFactionCollection.AddFactionMsg>(MyFactionCollection.EditFactionSuccess)), msgEdit);
      }
      else
      {
        if (MyEventContext.Current.IsLocallyInvoked)
          return;
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
    }

    [Event(null, 1567)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    private static void EditFactionSuccess(MyFactionCollection.AddFactionMsg msgEdit)
    {
      if (!(MySession.Static.Factions.TryGetFactionById(msgEdit.FactionId) is MyFaction factionById))
        return;
      MySession.Static.Factions.UnregisterFactionTag(factionById);
      string str = string.Empty;
      if (msgEdit.FactionIconGroupId.HasValue)
        str = MyFactionCollection.GetFactionIcon(msgEdit.FactionIconGroupId.Value, msgEdit.FactionIconId);
      factionById.Tag = msgEdit.FactionTag;
      factionById.Name = msgEdit.FactionName;
      factionById.Description = msgEdit.FactionDescription;
      factionById.PrivateInfo = msgEdit.FactionPrivateInfo;
      factionById.FactionIcon = new MyStringId?(MyStringId.GetOrCompute(str));
      factionById.CustomColor = (Vector3) msgEdit.FactionColor;
      factionById.IconColor = (Vector3) msgEdit.FactionIconColor;
      factionById.Score = msgEdit.FactionScore;
      factionById.ObjectivePercentageCompleted = msgEdit.FactionObjectivePercentageCompleted;
      MySession.Static.Factions.RegisterFactionTag(factionById);
      Action<long> factionEdited = MySession.Static.Factions.FactionEdited;
      if (factionEdited == null)
        return;
      factionEdited(msgEdit.FactionId);
    }

    [Event(null, 1599)]
    [Reliable]
    [Server]
    public static void RequestFactionScoreAndPercentageUpdate(long factionId)
    {
      IMyFaction factionById = MySession.Static.Factions.TryGetFactionById(factionId);
      if (factionById == null)
        return;
      MyMultiplayer.RaiseStaticEvent<long, int, float>((Func<IMyEventOwner, Action<long, int, float>>) (s => new Action<long, int, float>(MyFactionCollection.RecieveFactionScoreAndPercentage)), factionId, factionById.Score, factionById.ObjectivePercentageCompleted, MyEventContext.Current.Sender);
    }

    [Event(null, 1608)]
    [Reliable]
    [Client]
    private static void RecieveFactionScoreAndPercentage(
      long factionId,
      int score,
      float percentageFinished)
    {
      IMyFaction factionById = MySession.Static.Factions.TryGetFactionById(factionId);
      if (factionById == null)
        return;
      factionById.Score = score;
      factionById.ObjectivePercentageCompleted = percentageFinished;
    }

    public event Action<long> FactionCreated;

    public void CreateFaction(
      long founderId,
      string tag,
      string name,
      string desc,
      string privateInfo,
      MyFactionTypes type,
      Vector3 factionColor = default (Vector3),
      Vector3 factionIconColor = default (Vector3),
      SerializableDefinitionId? factionIconGroupId = null,
      int factionIconId = 0)
    {
      this.SendCreateFaction(founderId, tag, name, desc, privateInfo, type, factionColor, factionIconColor, factionIconGroupId, factionIconId);
    }

    public void CreateNPCFaction(string tag, string name, string desc, string privateInfo)
    {
      MyIdentity newIdentity = Sync.Players.CreateNewIdentity(tag + " NPC" + (object) MyRandom.Instance.Next(1000, 9999), (string) null, new Vector3?(), false, false);
      Sync.Players.MarkIdentityAsNPC(newIdentity.IdentityId);
      this.SendCreateFaction(newIdentity.IdentityId, tag, name, desc, privateInfo, MyFactionTypes.None, new Vector3(), new Vector3());
    }

    private void Add(MyFaction faction)
    {
      this.m_factions.Add(faction.FactionId, faction);
      this.RegisterFactionTag(faction);
    }

    private void SendCreateFaction(
      long founderId,
      string factionTag,
      string factionName,
      string factionDesc,
      string factionPrivate,
      MyFactionTypes type,
      Vector3 factionColor,
      Vector3 factionIconColor,
      SerializableDefinitionId? factionIconGroupId = null,
      int factionIconId = 0)
    {
      MyMultiplayer.RaiseStaticEvent<MyFactionCollection.AddFactionMsg>((Func<IMyEventOwner, Action<MyFactionCollection.AddFactionMsg>>) (s => new Action<MyFactionCollection.AddFactionMsg>(MyFactionCollection.CreateFactionRequest)), new MyFactionCollection.AddFactionMsg()
      {
        FounderId = founderId,
        FactionTag = factionTag,
        FactionName = factionName,
        FactionDescription = factionDesc,
        FactionPrivateInfo = factionPrivate,
        Type = type,
        FactionColor = (SerializableVector3) factionColor,
        FactionIconColor = (SerializableVector3) factionIconColor,
        FactionIconGroupId = factionIconGroupId,
        FactionIconId = factionIconId
      });
    }

    [Event(null, 1663)]
    [Reliable]
    [Server]
    private static void CreateFactionRequest(MyFactionCollection.AddFactionMsg msg)
    {
      if (MySession.Static.MaxFactionsCount == 0 || MySession.Static.MaxFactionsCount > 0 && MySession.Static.Factions.HumansCount() < MySession.Static.MaxFactionsCount)
      {
        MyFactionCollection.CreateFactionServer(msg.FounderId, msg.FactionTag, msg.FactionName, msg.FactionDescription, msg.FactionPrivateInfo, type: msg.Type, factionIconGroupId: msg.FactionIconGroupId, factionIconId: msg.FactionIconId, factionColor: ((Vector3) msg.FactionColor), factionIconColor: ((Vector3) msg.FactionIconColor));
      }
      else
      {
        if (MyEventContext.Current.IsLocallyInvoked)
          return;
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
    }

    private static void CreateFactionServer(
      long founderId,
      string factionTag,
      string factionName,
      string description,
      string privateInfo,
      MyFactionDefinition factionDef = null,
      MyFactionTypes type = MyFactionTypes.None,
      SerializableDefinitionId? factionIconGroupId = null,
      int factionIconId = 0,
      Vector3 factionColor = default (Vector3),
      Vector3 factionIconColor = default (Vector3),
      int score = 0)
    {
      if (!Sync.IsServer)
        return;
      long num = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.FACTION);
      IMyFaction factionById = MySession.Static.Factions.TryGetFactionById(num);
      if (MySession.Static.Factions.TryGetPlayerFaction(founderId) != null || factionById != null || (MySession.Static.Factions.FactionTagExists(factionTag, (IMyFaction) null) || MySession.Static.Factions.FactionNameExists(factionName, (IMyFaction) null)) || !Sync.Players.HasIdentity(founderId))
        return;
      bool createFromDef = factionDef != null;
      if (!(!createFromDef ? MyFactionCollection.CreateFactionInternal(founderId, num, factionTag, factionName, description, privateInfo, type, factionColor, new Vector3?(factionIconColor), factionIconGroupId, factionIconId, score) : MyFactionCollection.CreateFactionInternal(founderId, num, factionDef)))
        return;
      MyBankingSystem.Static.CreateAccount(num, 0L);
      MyFactionCollection.FactionCreationFinished(num, founderId, factionTag, factionName, description, privateInfo, createFromDef, type, factionIconGroupId, factionIconId, factionColor, factionIconColor, score);
    }

    public static bool GetDefinitionIdsByIconName(
      string iconName,
      out SerializableDefinitionId? factionIconGroupId,
      out int factionIconId)
    {
      factionIconGroupId = new SerializableDefinitionId?();
      factionIconId = 0;
      IEnumerable<MyFactionIconsDefinition> allDefinitions = MyDefinitionManager.Static.GetAllDefinitions<MyFactionIconsDefinition>();
      if (allDefinitions == null)
        return false;
      foreach (MyFactionIconsDefinition factionIconsDefinition in allDefinitions)
      {
        int num = 0;
        foreach (string icon in factionIconsDefinition.Icons)
        {
          if (string.Equals(iconName, icon))
          {
            factionIconGroupId = new SerializableDefinitionId?((SerializableDefinitionId) factionIconsDefinition.Id);
            factionIconId = num;
            return true;
          }
          ++num;
        }
      }
      return false;
    }

    public static bool CanPlayerUseFactionIcon(
      SerializableDefinitionId factionIconGroupId,
      int factionIconId,
      long identityId,
      out string icon)
    {
      ulong steamId = MySession.Static.Players.TryGetSteamId(identityId);
      if (steamId != 0UL)
        return MyFactionCollection.CanPlayerUseFactionIcon(factionIconGroupId, factionIconId, steamId, out icon);
      icon = string.Empty;
      return false;
    }

    public static bool CanPlayerUseFactionIcon(
      SerializableDefinitionId factionIconGroupId,
      int factionIconId,
      ulong steamId,
      out string icon)
    {
      MyDefinitionBase definition = MyDefinitionManager.Static.GetDefinition((MyDefinitionId) factionIconGroupId);
      if (definition != null && ((IEnumerable<string>) definition.Icons).Count<string>() > factionIconId)
      {
        if (definition.Id.SubtypeId.String == "Other")
        {
          if (MySession.Static.GetComponent<MySessionComponentDLC>().HasDLC("Economy", steamId))
          {
            icon = definition.Icons[factionIconId];
            return true;
          }
        }
        else
        {
          icon = definition.Icons[factionIconId];
          return true;
        }
      }
      icon = "";
      return false;
    }

    public static string GetFactionIcon(
      SerializableDefinitionId factionIconGroupId,
      int factionIconId)
    {
      if (factionIconGroupId.IsNull())
        return string.Empty;
      MyDefinitionBase definition = MyDefinitionManager.Static.GetDefinition((MyDefinitionId) factionIconGroupId);
      return definition != null && ((IEnumerable<string>) definition.Icons).Count<string>() > factionIconId ? definition.Icons[factionIconId] : string.Empty;
    }

    public static void FactionCreationFinished(
      long factionId,
      long founderId,
      string factionTag,
      string factionName,
      string description,
      string privateInfo,
      bool createFromDef = false,
      MyFactionTypes type = MyFactionTypes.None,
      SerializableDefinitionId? factionIconGroupId = null,
      int factionIconId = 0,
      Vector3 factionColor = default (Vector3),
      Vector3 factionIconColor = default (Vector3),
      int score = 0)
    {
      MyMultiplayer.RaiseStaticEvent<MyFactionCollection.AddFactionMsg>((Func<IMyEventOwner, Action<MyFactionCollection.AddFactionMsg>>) (x => new Action<MyFactionCollection.AddFactionMsg>(MyFactionCollection.CreateFactionSuccess)), new MyFactionCollection.AddFactionMsg()
      {
        FactionId = factionId,
        FounderId = founderId,
        FactionTag = factionTag,
        FactionName = factionName,
        FactionDescription = description,
        FactionPrivateInfo = privateInfo,
        CreateFromDefinition = createFromDef,
        FactionColor = (SerializableVector3) factionColor,
        FactionIconColor = (SerializableVector3) factionIconColor,
        Type = type,
        FactionIconGroupId = factionIconGroupId,
        FactionIconId = factionIconId,
        FactionScore = score
      });
      MyFactionCollection.SetDefaultFactionStates(factionId);
      MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (x => new Action<long>(MyFactionCollection.SetDefaultFactionStates)), factionId);
    }

    [Event(null, 1820)]
    [Reliable]
    [Broadcast]
    private static void CreateFactionSuccess(MyFactionCollection.AddFactionMsg msg)
    {
      if (msg.CreateFromDefinition)
      {
        MyFactionDefinition factionDefinition = MyDefinitionManager.Static.TryGetFactionDefinition(msg.FactionTag);
        if (factionDefinition == null)
          return;
        MyFactionCollection.CreateFactionInternal(msg.FounderId, msg.FactionId, factionDefinition);
      }
      else
        MyFactionCollection.CreateFactionInternal(msg.FounderId, msg.FactionId, msg.FactionTag, msg.FactionName, msg.FactionDescription, msg.FactionPrivateInfo, msg.Type, (Vector3) msg.FactionColor, new Vector3?((Vector3) msg.FactionIconColor), msg.FactionIconGroupId, msg.FactionIconId, msg.FactionScore);
    }

    public static bool CreateFactionInternal(
      long founderId,
      long factionId,
      MyFactionDefinition factionDef,
      Vector3? customColor = null,
      Vector3? iconColor = null)
    {
      if (MySession.Static.Factions.Contains(factionId) || MySession.Static.MaxFactionsCount > 0 && MySession.Static.Factions.HumansCount() >= MySession.Static.MaxFactionsCount)
        return false;
      MySession.Static.Factions.Add(new MyFaction(factionId, founderId, "", factionDef, customColor, iconColor));
      MySession.Static.Factions.AddPlayerToFaction(founderId, factionId);
      MyFactionCollection.AfterFactionCreated(founderId, factionId);
      return true;
    }

    private static bool CreateFactionInternal(
      long founderId,
      long factionId,
      string factionTag,
      string factionName,
      string factionDescription,
      string factionPrivateInfo,
      MyFactionTypes type,
      Vector3 factionColor,
      Vector3? factionIconColor = null,
      SerializableDefinitionId? factionIconGroupId = null,
      int factionIconId = 0,
      int factionScore = 0)
    {
      if (MySession.Static.MaxFactionsCount > 0 && MySession.Static.Factions.HumansCount() >= MySession.Static.MaxFactionsCount)
        return false;
      string icon = string.Empty;
      if (factionIconGroupId.HasValue)
      {
        if (Sync.IsServer)
        {
          if (!MyFactionCollection.CanPlayerUseFactionIcon(factionIconGroupId.Value, factionIconId, founderId, out icon))
            return false;
        }
        else
          icon = MyFactionCollection.GetFactionIcon(factionIconGroupId.Value, factionIconId);
      }
      MySession.Static.Factions.AddPlayerToFaction(founderId, factionId);
      MySession.Static.Factions.Add(new MyFaction(factionId, factionTag, factionName, factionDescription, factionPrivateInfo, founderId, type, new Vector3?(factionColor), factionIconColor, icon, factionScore));
      MyFactionCollection.AfterFactionCreated(founderId, factionId);
      return true;
    }

    private static MyFactionStateChange DetermineRequestFromRelation(
      MyRelationsBetweenFactions relation)
    {
      MyFactionStateChange factionStateChange;
      switch (relation)
      {
        case MyRelationsBetweenFactions.Enemies:
          factionStateChange = MyFactionStateChange.DeclareWar;
          break;
        case MyRelationsBetweenFactions.Friends:
          factionStateChange = MyFactionStateChange.SendFriendRequest;
          break;
        default:
          factionStateChange = MyFactionStateChange.SendPeaceRequest;
          break;
      }
      return factionStateChange;
    }

    private static void AfterFactionCreated(long founderId, long factionId)
    {
      foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
        faction.Value.CancelJoinRequest(founderId);
      Action<long> factionCreated = MySession.Static.Factions.FactionCreated;
      if (factionCreated == null)
        return;
      factionCreated(factionId);
    }

    [Event(null, 1941)]
    [Reliable]
    [Broadcast]
    private static void SetDefaultFactionStates(long recivedFactionId)
    {
      IMyFaction factionById = MySession.Static.Factions.TryGetFactionById(recivedFactionId);
      MyFactionDefinition factionDefinition1 = MyDefinitionManager.Static.TryGetFactionDefinition(factionById.Tag);
      MyFaction myFaction1 = factionById as MyFaction;
      foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
      {
        MyFaction myFaction2 = faction.Value;
        if (myFaction2.FactionId != recivedFactionId)
        {
          if (MyFactionCollection.ShouldForceRelationToFactions(myFaction2, myFaction1))
            MySession.Static.Factions.ForceRelationToFactions(myFaction2, myFaction1);
          else if (factionDefinition1 != null)
          {
            MyFactionCollection.SetDefaultFactionStateInternal(myFaction2.FactionId, factionById, factionDefinition1);
          }
          else
          {
            MyFactionDefinition factionDefinition2 = MyDefinitionManager.Static.TryGetFactionDefinition(myFaction2.Tag);
            if (factionDefinition2 != null)
              MyFactionCollection.SetDefaultFactionStateInternal(recivedFactionId, (IMyFaction) myFaction2, factionDefinition2);
          }
        }
      }
      if (!MyFactionCollection.ShouldForceRelationToPlayers(myFaction1))
        return;
      foreach (MyIdentity allIdentity in (IEnumerable<MyIdentity>) MySession.Static.Players.GetAllIdentities())
      {
        if (!MySession.Static.Players.IdentityIsNpc(allIdentity.IdentityId))
          MySession.Static.Factions.ForceRelationToPlayers(myFaction1, allIdentity.IdentityId);
      }
    }

    public bool ForceRelationToPlayers(MyFaction faction, long playerId)
    {
      MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
      if (component == null)
        return false;
      MyRelationsBetweenFactions relation;
      bool flag;
      if ((MyDefinitionManager.Static.GetDefinition(component.EconomyDefinition.PirateId) as MyFactionDefinition).Tag == faction.Tag)
      {
        this.ChangeReputationWithPlayer(playerId, faction.FactionId, component.EconomyDefinition.ReputationHostileMin);
        relation = MyRelationsBetweenFactions.Enemies;
        flag = true;
      }
      else
      {
        switch (faction.FactionType)
        {
          case MyFactionTypes.PlayerMade:
            relation = MyRelationsBetweenFactions.Enemies;
            flag = true;
            break;
          case MyFactionTypes.Miner:
          case MyFactionTypes.Trader:
          case MyFactionTypes.Builder:
            if (component != null)
            {
              this.ChangeReputationWithPlayer(playerId, faction.FactionId, component.GetDefaultReputationPlayer());
              return true;
            }
            relation = MyRelationsBetweenFactions.Neutral;
            flag = true;
            break;
          default:
            relation = MyRelationsBetweenFactions.Enemies;
            flag = true;
            break;
        }
      }
      if (!flag)
        return false;
      this.ChangeRelationWithPlayer(playerId, faction.FactionId, relation);
      return true;
    }

    private static bool ShouldForceRelationToPlayers(MyFaction faction) => faction.FactionType == MyFactionTypes.Miner || faction.FactionType == MyFactionTypes.Trader || faction.FactionType == MyFactionTypes.Builder;

    public bool ForceRelationsOnNewIdentity(long identityId)
    {
      if (MySession.Static.Players.IdentityIsNpc(identityId))
        return false;
      foreach (KeyValuePair<long, MyFaction> keyValuePair in this)
      {
        if (!this.HasRelationWithPlayer(keyValuePair.Value.FactionId, identityId) && MyFactionCollection.ShouldForceRelationToPlayers(keyValuePair.Value))
          this.ForceRelationToPlayers(keyValuePair.Value, identityId);
      }
      return true;
    }

    private bool ForceRelationToFactions(MyFaction faction1, MyFaction faction2)
    {
      int num = faction1.FactionType == MyFactionTypes.PlayerMade ? 1 : 0;
      bool flag1 = faction2.FactionType == MyFactionTypes.PlayerMade;
      MyFaction myFaction1;
      MyFaction myFaction2;
      if (num != 0)
      {
        myFaction1 = faction1;
        myFaction2 = faction2;
      }
      else if (flag1)
      {
        myFaction1 = faction2;
        myFaction2 = faction1;
      }
      else
      {
        this.SetFactionStateInternal(faction1.FactionId, (IMyFaction) faction2, MyFactionStateChange.SendPeaceRequest);
        this.SetFactionStateInternal(faction2.FactionId, (IMyFaction) faction1, MyFactionStateChange.AcceptPeace);
        return true;
      }
      MyRelationsBetweenFactions relationsBetweenFactions1;
      this.GetRelationBetweenPlayerAndFaction(myFaction1.FounderId, myFaction2.FactionId).Deconstruct<MyRelationsBetweenFactions, int>(out relationsBetweenFactions1, out int _);
      MyRelationsBetweenFactions relationsBetweenFactions2 = relationsBetweenFactions1;
      MyFactionStateChange request1;
      MyFactionStateChange request2 = request1 = MyFactionStateChange.DeclareWar;
      bool flag2 = false;
      switch (relationsBetweenFactions2)
      {
        case MyRelationsBetweenFactions.Neutral:
          request2 = MyFactionStateChange.SendPeaceRequest;
          request1 = MyFactionStateChange.AcceptPeace;
          flag2 = true;
          break;
        case MyRelationsBetweenFactions.Enemies:
          request2 = MyFactionStateChange.DeclareWar;
          request1 = MyFactionStateChange.DeclareWar;
          flag2 = true;
          break;
        case MyRelationsBetweenFactions.Friends:
          request2 = MyFactionStateChange.SendFriendRequest;
          request1 = MyFactionStateChange.AcceptFriendRequest;
          flag2 = true;
          break;
      }
      if (!flag2)
        return false;
      this.SetFactionStateInternal(myFaction2.FactionId, (IMyFaction) myFaction1, request2);
      this.SetFactionStateInternal(myFaction1.FactionId, (IMyFaction) myFaction2, request1);
      return true;
    }

    private static bool ShouldForceRelationToFactions(MyFaction faction, MyFaction fac)
    {
      int num1 = faction.FactionType == MyFactionTypes.Miner || faction.FactionType == MyFactionTypes.Trader ? 1 : (faction.FactionType == MyFactionTypes.Builder ? 1 : 0);
      bool flag1 = fac.FactionType == MyFactionTypes.Miner || fac.FactionType == MyFactionTypes.Trader || fac.FactionType == MyFactionTypes.Builder;
      bool flag2 = faction.FactionType == MyFactionTypes.PlayerMade;
      bool flag3 = fac.FactionType == MyFactionTypes.PlayerMade;
      int num2 = flag1 ? 1 : 0;
      return num1 != num2 && flag2 != flag3;
    }

    private static void SetDefaultFactionStateInternal(
      long factionId,
      IMyFaction defaultFaction,
      MyFactionDefinition defaultFactionDef)
    {
      MyFactionStateChange requestFromRelation = MyFactionCollection.DetermineRequestFromRelation(defaultFactionDef.DefaultRelation);
      MySession.Static.Factions.ApplyFactionStateChange(requestFromRelation, defaultFaction.FactionId, factionId, defaultFaction.FounderId, defaultFaction.FounderId);
      Action<MyFactionStateChange, long, long, long, long> factionStateChanged = MySession.Static.Factions.FactionStateChanged;
      if (factionStateChanged == null)
        return;
      factionStateChanged(requestFromRelation, defaultFaction.FactionId, factionId, defaultFaction.FounderId, defaultFaction.FounderId);
    }

    private void SetFactionStateInternal(
      long factionId,
      IMyFaction faction,
      MyFactionStateChange request)
    {
      this.ApplyFactionStateChange(request, faction.FactionId, factionId, faction.FounderId, faction.FounderId);
      Action<MyFactionStateChange, long, long, long, long> factionStateChanged = MySession.Static.Factions.FactionStateChanged;
      if (factionStateChanged == null)
        return;
      factionStateChanged(request, faction.FactionId, factionId, faction.FounderId, faction.FounderId);
    }

    public int HumansCount() => this.Factions.Where<KeyValuePair<long, IMyFaction>>((Func<KeyValuePair<long, IMyFaction>, bool>) (x => x.Value.AcceptHumans)).Count<KeyValuePair<long, IMyFaction>>();

    public bool IsFactionDiscovered(MyPlayer.PlayerId playerId, long factionId)
    {
      List<long> longList;
      return this.m_playerToFactionsVis.TryGetValue(playerId, out longList) && longList.Contains(factionId);
    }

    public void AddDiscoveredFaction(MyPlayer.PlayerId playerId, long factionId, bool triggerEvent = true)
    {
      if (!Sync.IsServer)
      {
        MyLog.Default.Error("It is illegal to add discovered factions on clients.");
      }
      else
      {
        this.AddDiscoveredFaction_Internal(playerId, factionId, triggerEvent);
        MyMultiplayer.RaiseStaticEvent<ulong, int, long>((Func<IMyEventOwner, Action<ulong, int, long>>) (x => new Action<ulong, int, long>(MyFactionCollection.AddDiscoveredFaction_Clients)), playerId.SteamId, playerId.SerialId, factionId);
      }
    }

    public List<MyFaction> GetNpcFactions()
    {
      List<MyFaction> myFactionList = new List<MyFaction>();
      foreach (MyFaction myFaction in this.m_factions.Values)
      {
        if (this.IsNpcFaction(myFaction.Tag) && !(myFaction.Tag == "SPID"))
          myFactionList.Add(myFaction);
      }
      return myFactionList;
    }

    [Event(null, 2234)]
    [Reliable]
    [Broadcast]
    private static void AddDiscoveredFaction_Clients(ulong playerId, int serialId, long factionId) => MySession.Static.Factions.AddDiscoveredFaction_Internal(new MyPlayer.PlayerId(playerId, serialId), factionId);

    private void AddDiscoveredFaction_Internal(
      MyPlayer.PlayerId playerId,
      long factionId,
      bool triggerEvent = true)
    {
      List<long> longList;
      if (!this.m_playerToFactionsVis.TryGetValue(playerId, out longList))
      {
        longList = new List<long>();
        this.m_playerToFactionsVis.Add(playerId, longList);
      }
      if (longList.Contains(factionId))
        return;
      longList.Add(factionId);
      IMyFaction factionById = this.TryGetFactionById(factionId);
      if (!triggerEvent || !(factionById is MyFaction myFaction))
        return;
      Action<MyFaction, MyPlayer.PlayerId> factionDiscovered = this.OnFactionDiscovered;
      if (factionDiscovered == null)
        return;
      factionDiscovered(myFaction, playerId);
    }

    public void RemoveDiscoveredFaction(MyPlayer.PlayerId playerId, long factionId)
    {
      if (!Sync.IsServer)
      {
        MyLog.Default.Error("It is illegal to removed discovered factions on clients.");
      }
      else
      {
        this.RemoveDiscoveredFaction_Internal(playerId, factionId);
        MyMultiplayer.RaiseStaticEvent<ulong, int, long>((Func<IMyEventOwner, Action<ulong, int, long>>) (x => new Action<ulong, int, long>(MyFactionCollection.RemoveDiscoveredFaction_Clients)), playerId.SteamId, playerId.SerialId, factionId);
      }
    }

    [Event(null, 2279)]
    [Reliable]
    [Broadcast]
    private static void RemoveDiscoveredFaction_Clients(
      ulong playerId,
      int serialId,
      long factionId)
    {
      MySession.Static.Factions.RemoveDiscoveredFaction_Internal(new MyPlayer.PlayerId(playerId, serialId), factionId);
    }

    private void RemoveDiscoveredFaction_Internal(MyPlayer.PlayerId playerId, long factionId)
    {
      List<long> longList;
      if (!this.m_playerToFactionsVis.TryGetValue(playerId, out longList))
        return;
      longList.Remove(factionId);
      if (longList.Count != 0)
        return;
      this.m_playerToFactionsVis.Remove(playerId);
    }

    public void RemovePlayerFromVisibility(MyPlayer.PlayerId playerId)
    {
      if (!Sync.IsServer || !this.m_playerToFactionsVis.ContainsKey(playerId))
        return;
      this.RemovePlayerFromVisibility_Internal(playerId);
      MyMultiplayer.RaiseStaticEvent<MyPlayer.PlayerId>((Func<IMyEventOwner, Action<MyPlayer.PlayerId>>) (x => new Action<MyPlayer.PlayerId>(MyFactionCollection.RemovePlayerFromVisibility_Broadcast)), playerId);
    }

    [Event(null, 2312)]
    [Reliable]
    [Broadcast]
    public static void RemovePlayerFromVisibility_Broadcast(MyPlayer.PlayerId playerId) => MySession.Static?.Factions?.RemovePlayerFromVisibility_Internal(playerId);

    public void RemovePlayerFromVisibility_Internal(MyPlayer.PlayerId playerId)
    {
      if (!this.m_playerToFactionsVis.ContainsKey(playerId))
        return;
      this.m_playerToFactionsVis.Remove(playerId);
    }

    public void RemoveFactionFromVisibility(long factionId)
    {
      if (!Sync.IsServer)
        return;
      this.RemoveFactionFromVisibility_Internal(factionId);
    }

    public void RemoveFactionFromVisibility_Internal(long factionId)
    {
      foreach (KeyValuePair<MyPlayer.PlayerId, List<long>> playerToFactionsVi in this.m_playerToFactionsVis)
        playerToFactionsVi.Value.Remove(factionId);
    }

    public MyObjectBuilder_FactionCollection GetObjectBuilder()
    {
      MyObjectBuilder_FactionCollection factionCollection = new MyObjectBuilder_FactionCollection();
      factionCollection.Factions = new List<MyObjectBuilder_Faction>(this.m_factions.Count);
      foreach (KeyValuePair<long, MyFaction> faction in this.m_factions)
        factionCollection.Factions.Add(faction.Value.GetObjectBuilder());
      factionCollection.Players = new SerializableDictionary<long, long>();
      foreach (KeyValuePair<long, long> keyValuePair in this.m_playerFaction)
        factionCollection.Players.Dictionary.Add(keyValuePair.Key, keyValuePair.Value);
      factionCollection.Relations = new List<MyObjectBuilder_FactionRelation>(this.m_relationsBetweenFactions.Count);
      foreach (KeyValuePair<MyFactionCollection.MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>> relationsBetweenFaction in this.m_relationsBetweenFactions)
        factionCollection.Relations.Add(new MyObjectBuilder_FactionRelation()
        {
          FactionId1 = relationsBetweenFaction.Key.RelateeId1,
          FactionId2 = relationsBetweenFaction.Key.RelateeId2,
          Relation = relationsBetweenFaction.Value.Item1,
          Reputation = relationsBetweenFaction.Value.Item2
        });
      factionCollection.Requests = new List<MyObjectBuilder_FactionRequests>();
      foreach (KeyValuePair<long, HashSet<long>> factionRequest in this.m_factionRequests)
      {
        List<long> longList = new List<long>(factionRequest.Value.Count);
        foreach (long num in this.m_factionRequests[factionRequest.Key])
          longList.Add(num);
        factionCollection.Requests.Add(new MyObjectBuilder_FactionRequests()
        {
          FactionId = factionRequest.Key,
          FactionRequests = longList
        });
      }
      factionCollection.RelationsWithPlayers = new List<MyObjectBuilder_PlayerFactionRelation>();
      foreach (KeyValuePair<MyFactionCollection.MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>> playersAndFaction in this.m_relationsBetweenPlayersAndFactions)
        factionCollection.RelationsWithPlayers.Add(new MyObjectBuilder_PlayerFactionRelation()
        {
          PlayerId = playersAndFaction.Key.RelateeId1,
          FactionId = playersAndFaction.Key.RelateeId2,
          Relation = playersAndFaction.Value.Item1,
          Reputation = playersAndFaction.Value.Item2
        });
      factionCollection.PlayerToFactionsVis = new List<MyObjectBuilder_FactionsVisEntry>(this.m_playerToFactionsVis.Count);
      foreach (KeyValuePair<MyPlayer.PlayerId, List<long>> playerToFactionsVi in this.m_playerToFactionsVis)
      {
        MyObjectBuilder_FactionsVisEntry factionsVisEntry = new MyObjectBuilder_FactionsVisEntry();
        factionsVisEntry.IdentityId = Sync.Players.TryGetIdentityId(playerToFactionsVi.Key.SteamId, playerToFactionsVi.Key.SerialId);
        factionsVisEntry.DiscoveredFactions = new List<long>(playerToFactionsVi.Value.Count);
        foreach (long num in playerToFactionsVi.Value)
          factionsVisEntry.DiscoveredFactions.Add(num);
        if (factionsVisEntry.IdentityId != 0L)
          factionCollection.PlayerToFactionsVis.Add(factionsVisEntry);
      }
      return factionCollection;
    }

    public void Init(MyObjectBuilder_FactionCollection builder)
    {
      foreach (MyObjectBuilder_Faction faction in builder.Factions)
      {
        if (!MyBankingSystem.Static.TryGetAccountInfo(faction.FactionId, out MyAccountInfo _))
          MyBankingSystem.Static.CreateAccount(faction.FactionId, 0L);
        MySession.Static.Factions.Add(new MyFaction(faction));
      }
      foreach (KeyValuePair<long, long> keyValuePair in builder.Players.Dictionary)
        this.m_playerFaction.Add(keyValuePair.Key, keyValuePair.Value);
      MySessionComponentEconomy componentEconomy = (MySessionComponentEconomy) null;
      if (MySession.Static != null)
        componentEconomy = MySession.Static.GetComponent<MySessionComponentEconomy>();
      MyRelationsBetweenFactions relationsBetweenFactions;
      int num;
      foreach (MyObjectBuilder_FactionRelation relation1 in builder.Relations)
      {
        MyRelationsBetweenFactions relation2 = relation1.Relation;
        int reputation = relation1.Reputation;
        if (componentEconomy != null)
        {
          componentEconomy.ValidateReputationConsistency(relation2, reputation).Deconstruct<MyRelationsBetweenFactions, int>(out relationsBetweenFactions, out num);
          relation2 = relationsBetweenFactions;
          reputation = num;
        }
        this.m_relationsBetweenFactions.Add(new MyFactionCollection.MyRelatablePair(relation1.FactionId1, relation1.FactionId2), new Tuple<MyRelationsBetweenFactions, int>(relation2, reputation));
      }
      foreach (MyObjectBuilder_PlayerFactionRelation relationsWithPlayer in builder.RelationsWithPlayers)
      {
        MyRelationsBetweenFactions relation = relationsWithPlayer.Relation;
        int reputation = relationsWithPlayer.Reputation;
        if (componentEconomy != null)
        {
          componentEconomy.ValidateReputationConsistency(relation, reputation).Deconstruct<MyRelationsBetweenFactions, int>(out relationsBetweenFactions, out num);
          relation = relationsBetweenFactions;
          reputation = num;
        }
        this.m_relationsBetweenPlayersAndFactions.Add(new MyFactionCollection.MyRelatablePair(relationsWithPlayer.PlayerId, relationsWithPlayer.FactionId), new Tuple<MyRelationsBetweenFactions, int>(relation, reputation));
      }
      foreach (MyObjectBuilder_FactionRequests request in builder.Requests)
      {
        HashSet<long> longSet = new HashSet<long>();
        foreach (long factionRequest in request.FactionRequests)
          longSet.Add(factionRequest);
        this.m_factionRequests.Add(request.FactionId, longSet);
      }
      if (builder.PlayerToFactionsVis != null)
      {
        this.m_playerToFactionsVis.Clear();
        foreach (MyObjectBuilder_FactionsVisEntry playerToFactionsVi in builder.PlayerToFactionsVis)
        {
          List<long> longList = new List<long>();
          foreach (long discoveredFaction in playerToFactionsVi.DiscoveredFactions)
            longList.Add(discoveredFaction);
          if (playerToFactionsVi.IdentityId != 0L)
          {
            MyPlayer.PlayerId result;
            if (Sync.Players.TryGetPlayerId(playerToFactionsVi.IdentityId, out result))
              this.m_playerToFactionsVis.Add(result, longList);
          }
          else
          {
            MyPlayer.PlayerId key = new MyPlayer.PlayerId(playerToFactionsVi.PlayerId, playerToFactionsVi.SerialId);
            if (Sync.Players.TryGetIdentityId(key.SteamId, key.SerialId) != 0L)
              this.m_playerToFactionsVis.Add(key, longList);
          }
        }
      }
      this.m_reputationSettings = MyDefinitionManager.Static.GetDefinition<MyReputationSettingsDefinition>("DefaultReputationSettings");
      this.m_notificationRepInc = new MyReputationNotification(new MyHudNotification(MySpaceTexts.Economy_Notification_ReputationIncreased, font: "Green"));
      this.m_notificationRepDec = new MyReputationNotification(new MyHudNotification(MySpaceTexts.Economy_Notification_ReputationDecreased, font: "Red"));
      this.CompatDefaultFactions();
    }

    public MyFaction this[long factionId] => this.m_factions[factionId];

    public IEnumerator<KeyValuePair<long, MyFaction>> GetEnumerator() => (IEnumerator<KeyValuePair<long, MyFaction>>) this.m_factions.GetEnumerator();

    public MyFaction[] GetAllFactions() => this.m_factions.Values.ToArray<MyFaction>();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    IEnumerator<KeyValuePair<long, MyFaction>> IEnumerable<KeyValuePair<long, MyFaction>>.GetEnumerator() => this.GetEnumerator();

    public bool GetRandomFriendlyStation(
      long factionId,
      long stationId,
      out MyFaction friendlyFaction,
      out MyStation friendlyStation,
      bool includeSameFaction = false)
    {
      friendlyFaction = (MyFaction) null;
      friendlyStation = (MyStation) null;
      List<MyFaction> myFactionList = new List<MyFaction>();
      foreach (KeyValuePair<long, MyFaction> keyValuePair in this)
      {
        if (keyValuePair.Value.FactionType != MyFactionTypes.None && keyValuePair.Value.FactionType != MyFactionTypes.PlayerMade && (includeSameFaction && factionId == keyValuePair.Value.FactionId || this.GetRelationBetweenFactions(factionId, keyValuePair.Value.FactionId).Item1 == MyRelationsBetweenFactions.Friends))
          myFactionList.Add(keyValuePair.Value);
      }
      if (myFactionList.Count <= 0)
        return false;
      int num1 = 0;
      int num2 = 10;
      MyStation myStation;
      bool flag;
      MyFaction myFaction;
      do
      {
        ++num1;
        myStation = (MyStation) null;
        flag = false;
        myFaction = myFactionList[MyRandom.Instance.Next(0, myFactionList.Count)];
        DictionaryValuesReader<long, MyStation> stations1 = myFaction.Stations;
        if (stations1.Count > (myFaction.FactionId == factionId ? 1 : 0))
        {
          MyRandom instance = MyRandom.Instance;
          stations1 = myFaction.Stations;
          int count1 = stations1.Count;
          int index1 = instance.Next(0, count1);
          myStation = myFaction.Stations.ElementAt<MyStation>(index1);
          if (myStation.Id == stationId)
          {
            // ISSUE: variable of a boxed type
            __Boxed<DictionaryValuesReader<long, MyStation>> stations2 = (ValueType) myFaction.Stations;
            int num3 = index1 + 1;
            stations1 = myFaction.Stations;
            int count2 = stations1.Count;
            int index2 = num3 % count2;
            myStation = stations2.ElementAt<MyStation>(index2);
          }
          flag = true;
        }
      }
      while (!flag && num1 <= num2);
      if (!flag)
        return false;
      friendlyFaction = myFaction;
      friendlyStation = myStation;
      return true;
    }

    public void Clear()
    {
      this.m_factions.Clear();
      this.m_factionRequests.Clear();
      this.m_playerFaction.Clear();
      this.m_factionsByTag.Clear();
      this.m_playerToFactionsVis.Clear();
      this.m_playerToReputationLimits.Clear();
      this.m_relationsBetweenFactions.Clear();
      this.m_relationsBetweenPlayersAndFactions.Clear();
    }

    bool IMyFactionCollection.FactionTagExists(
      string tag,
      IMyFaction doNotCheck)
    {
      return this.FactionTagExists(tag, doNotCheck);
    }

    bool IMyFactionCollection.FactionNameExists(
      string name,
      IMyFaction doNotCheck)
    {
      return this.FactionNameExists(name, doNotCheck);
    }

    IMyFaction IMyFactionCollection.TryGetFactionById(long factionId) => this.TryGetFactionById(factionId);

    IMyFaction IMyFactionCollection.TryGetPlayerFaction(long playerId) => this.TryGetPlayerFaction(playerId);

    IMyFaction IMyFactionCollection.TryGetFactionByTag(string tag) => (IMyFaction) this.TryGetFactionByTag(tag, (IMyFaction) null);

    IMyFaction IMyFactionCollection.TryGetFactionByName(string name)
    {
      foreach (KeyValuePair<long, MyFaction> faction in this.m_factions)
      {
        MyFaction myFaction = faction.Value;
        if (string.Equals(name, myFaction.Name, StringComparison.OrdinalIgnoreCase))
          return (IMyFaction) myFaction;
      }
      return (IMyFaction) null;
    }

    void IMyFactionCollection.AddPlayerToFaction(long playerId, long factionId) => this.AddPlayerToFaction(playerId, factionId);

    void IMyFactionCollection.KickPlayerFromFaction(long playerId) => this.KickPlayerFromFaction(playerId);

    MyRelationsBetweenFactions IMyFactionCollection.GetRelationBetweenFactions(
      long factionId1,
      long factionId2)
    {
      return this.GetRelationBetweenFactions(factionId1, factionId2).Item1;
    }

    int IMyFactionCollection.GetReputationBetweenFactions(
      long factionId1,
      long factionId2)
    {
      return this.GetRelationBetweenFactions(factionId1, factionId2).Item2;
    }

    void IMyFactionCollection.SetReputation(
      long fromFactionId,
      long toFactionId,
      int reputation)
    {
      this.SetReputationBetweenFactions(fromFactionId, toFactionId, this.ClampReputation(reputation));
    }

    int IMyFactionCollection.GetReputationBetweenPlayerAndFaction(
      long identityId,
      long factionId)
    {
      return this.GetRelationBetweenPlayerAndFaction(identityId, factionId).Item2;
    }

    void IMyFactionCollection.SetReputationBetweenPlayerAndFaction(
      long identityId,
      long factionId,
      int reputation)
    {
      this.SetReputationBetweenPlayerAndFaction(identityId, factionId, this.ClampReputation(reputation));
    }

    bool IMyFactionCollection.AreFactionsEnemies(
      long factionId1,
      long factionId2)
    {
      return this.AreFactionsEnemies(factionId1, factionId2);
    }

    bool IMyFactionCollection.IsPeaceRequestStateSent(
      long myFactionId,
      long foreignFactionId)
    {
      return this.IsPeaceRequestStateSent(myFactionId, foreignFactionId);
    }

    bool IMyFactionCollection.IsPeaceRequestStatePending(
      long myFactionId,
      long foreignFactionId)
    {
      return this.IsPeaceRequestStatePending(myFactionId, foreignFactionId);
    }

    void IMyFactionCollection.RemoveFaction(long factionId) => MyFactionCollection.RemoveFaction(factionId);

    void IMyFactionCollection.SendPeaceRequest(
      long fromFactionId,
      long toFactionId)
    {
      MyFactionCollection.SendPeaceRequest(fromFactionId, toFactionId);
    }

    void IMyFactionCollection.CancelPeaceRequest(
      long fromFactionId,
      long toFactionId)
    {
      MyFactionCollection.CancelPeaceRequest(fromFactionId, toFactionId);
    }

    void IMyFactionCollection.AcceptPeace(long fromFactionId, long toFactionId) => MyFactionCollection.AcceptPeace(fromFactionId, toFactionId);

    void IMyFactionCollection.DeclareWar(long fromFactionId, long toFactionId) => MyFactionCollection.DeclareWar(fromFactionId, toFactionId);

    void IMyFactionCollection.SendJoinRequest(long factionId, long playerId) => MyFactionCollection.SendJoinRequest(factionId, playerId);

    void IMyFactionCollection.CancelJoinRequest(long factionId, long playerId) => MyFactionCollection.CancelJoinRequest(factionId, playerId);

    void IMyFactionCollection.AcceptJoin(long factionId, long playerId) => MyFactionCollection.AcceptJoin(factionId, playerId);

    void IMyFactionCollection.KickMember(long factionId, long playerId) => MyFactionCollection.KickMember(factionId, playerId);

    void IMyFactionCollection.PromoteMember(long factionId, long playerId) => MyFactionCollection.PromoteMember(factionId, playerId);

    void IMyFactionCollection.DemoteMember(long factionId, long playerId) => MyFactionCollection.DemoteMember(factionId, playerId);

    void IMyFactionCollection.MemberLeaves(long factionId, long playerId) => MyFactionCollection.MemberLeaves(factionId, playerId);

    event Action<long, bool, bool> IMyFactionCollection.FactionAutoAcceptChanged
    {
      add => this.FactionAutoAcceptChanged += value;
      remove => this.FactionAutoAcceptChanged -= value;
    }

    void IMyFactionCollection.ChangeAutoAccept(
      long factionId,
      long playerId,
      bool autoAcceptMember,
      bool autoAcceptPeace)
    {
      this.ChangeAutoAccept(factionId, autoAcceptMember, autoAcceptPeace);
    }

    void IMyFactionCollection.EditFaction(
      long factionId,
      string tag,
      string name,
      string desc,
      string privateInfo)
    {
      this.EditFaction(factionId, tag, name, desc, privateInfo, new SerializableDefinitionId?(), 0, new Vector3(), new Vector3(), 0, 0.0f);
    }

    void IMyFactionCollection.CreateFaction(
      long founderId,
      string tag,
      string name,
      string desc,
      string privateInfo)
    {
      this.CreateFaction(founderId, tag, name, desc, privateInfo, MyFactionTypes.None, new Vector3(), new Vector3(), new SerializableDefinitionId?(), 0);
    }

    void IMyFactionCollection.CreateFaction(
      long founderId,
      string tag,
      string name,
      string desc,
      string privateInfo,
      MyFactionTypes type)
    {
      this.CreateFaction(founderId, tag, name, desc, privateInfo, type, new Vector3(), new Vector3(), new SerializableDefinitionId?(), 0);
    }

    void IMyFactionCollection.CreateNPCFaction(
      string tag,
      string name,
      string desc,
      string privateInfo)
    {
      this.CreateNPCFaction(tag, name, desc, privateInfo);
    }

    event Action<long> IMyFactionCollection.FactionCreated
    {
      add => this.FactionCreated += value;
      remove => this.FactionCreated -= value;
    }

    MyObjectBuilder_FactionCollection IMyFactionCollection.GetObjectBuilder() => this.GetObjectBuilder();

    public Dictionary<long, IMyFaction> Factions => this.m_factions.ToDictionary<KeyValuePair<long, MyFaction>, long, IMyFaction>((Func<KeyValuePair<long, MyFaction>, long>) (e => e.Key), (Func<KeyValuePair<long, MyFaction>, IMyFaction>) (e => (IMyFaction) e.Value));

    event Action<long> IMyFactionCollection.FactionEdited
    {
      add => this.FactionEdited += value;
      remove => this.FactionEdited -= value;
    }

    event Action<MyFactionStateChange, long, long, long, long> IMyFactionCollection.FactionStateChanged
    {
      add => this.FactionStateChanged += value;
      remove => this.FactionStateChanged -= value;
    }

    public enum MyFactionPeaceRequestState
    {
      None,
      Pending,
      Sent,
    }

    public struct MyRelatablePair
    {
      public long RelateeId1;
      public long RelateeId2;
      public static readonly MyFactionCollection.MyRelatablePair.ComparerType Comparer = new MyFactionCollection.MyRelatablePair.ComparerType();

      public MyRelatablePair(long id1, long id2)
      {
        this.RelateeId1 = id1;
        this.RelateeId2 = id2;
      }

      public class ComparerType : IEqualityComparer<MyFactionCollection.MyRelatablePair>
      {
        public bool Equals(
          MyFactionCollection.MyRelatablePair x,
          MyFactionCollection.MyRelatablePair y)
        {
          if (x.RelateeId1 == y.RelateeId1 && x.RelateeId2 == y.RelateeId2)
            return true;
          return x.RelateeId1 == y.RelateeId2 && x.RelateeId2 == y.RelateeId1;
        }

        public int GetHashCode(MyFactionCollection.MyRelatablePair obj) => obj.RelateeId1.GetHashCode() ^ obj.RelateeId2.GetHashCode();
      }
    }

    [ProtoContract]
    public struct AddFactionMsg
    {
      [ProtoMember(1)]
      public long FounderId;
      [ProtoMember(4)]
      public long FactionId;
      [ProtoMember(7)]
      public string FactionTag;
      [ProtoMember(10)]
      public string FactionName;
      [Serialize(MyObjectFlags.DefaultZero)]
      [ProtoMember(13)]
      public string FactionDescription;
      [ProtoMember(16)]
      public string FactionPrivateInfo;
      [ProtoMember(19)]
      public bool CreateFromDefinition;
      [ProtoMember(22)]
      public MyFactionTypes Type;
      [ProtoMember(25)]
      public SerializableVector3 FactionColor;
      [ProtoMember(26)]
      public SerializableVector3 FactionIconColor;
      [ProtoMember(28)]
      public SerializableDefinitionId? FactionIconGroupId;
      [ProtoMember(31)]
      public int FactionIconId;
      [ProtoMember(35)]
      public int FactionScore;
      [ProtoMember(38)]
      public float FactionObjectivePercentageCompleted;

      protected class Sandbox_Game_Multiplayer_MyFactionCollection\u003C\u003EAddFactionMsg\u003C\u003EFounderId\u003C\u003EAccessor : IMemberAccessor<MyFactionCollection.AddFactionMsg, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyFactionCollection.AddFactionMsg owner, in long value) => owner.FounderId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyFactionCollection.AddFactionMsg owner, out long value) => value = owner.FounderId;
      }

      protected class Sandbox_Game_Multiplayer_MyFactionCollection\u003C\u003EAddFactionMsg\u003C\u003EFactionId\u003C\u003EAccessor : IMemberAccessor<MyFactionCollection.AddFactionMsg, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyFactionCollection.AddFactionMsg owner, in long value) => owner.FactionId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyFactionCollection.AddFactionMsg owner, out long value) => value = owner.FactionId;
      }

      protected class Sandbox_Game_Multiplayer_MyFactionCollection\u003C\u003EAddFactionMsg\u003C\u003EFactionTag\u003C\u003EAccessor : IMemberAccessor<MyFactionCollection.AddFactionMsg, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyFactionCollection.AddFactionMsg owner, in string value) => owner.FactionTag = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyFactionCollection.AddFactionMsg owner, out string value) => value = owner.FactionTag;
      }

      protected class Sandbox_Game_Multiplayer_MyFactionCollection\u003C\u003EAddFactionMsg\u003C\u003EFactionName\u003C\u003EAccessor : IMemberAccessor<MyFactionCollection.AddFactionMsg, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyFactionCollection.AddFactionMsg owner, in string value) => owner.FactionName = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyFactionCollection.AddFactionMsg owner, out string value) => value = owner.FactionName;
      }

      protected class Sandbox_Game_Multiplayer_MyFactionCollection\u003C\u003EAddFactionMsg\u003C\u003EFactionDescription\u003C\u003EAccessor : IMemberAccessor<MyFactionCollection.AddFactionMsg, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyFactionCollection.AddFactionMsg owner, in string value) => owner.FactionDescription = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyFactionCollection.AddFactionMsg owner, out string value) => value = owner.FactionDescription;
      }

      protected class Sandbox_Game_Multiplayer_MyFactionCollection\u003C\u003EAddFactionMsg\u003C\u003EFactionPrivateInfo\u003C\u003EAccessor : IMemberAccessor<MyFactionCollection.AddFactionMsg, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyFactionCollection.AddFactionMsg owner, in string value) => owner.FactionPrivateInfo = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyFactionCollection.AddFactionMsg owner, out string value) => value = owner.FactionPrivateInfo;
      }

      protected class Sandbox_Game_Multiplayer_MyFactionCollection\u003C\u003EAddFactionMsg\u003C\u003ECreateFromDefinition\u003C\u003EAccessor : IMemberAccessor<MyFactionCollection.AddFactionMsg, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyFactionCollection.AddFactionMsg owner, in bool value) => owner.CreateFromDefinition = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyFactionCollection.AddFactionMsg owner, out bool value) => value = owner.CreateFromDefinition;
      }

      protected class Sandbox_Game_Multiplayer_MyFactionCollection\u003C\u003EAddFactionMsg\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<MyFactionCollection.AddFactionMsg, MyFactionTypes>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyFactionCollection.AddFactionMsg owner,
          in MyFactionTypes value)
        {
          owner.Type = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyFactionCollection.AddFactionMsg owner,
          out MyFactionTypes value)
        {
          value = owner.Type;
        }
      }

      protected class Sandbox_Game_Multiplayer_MyFactionCollection\u003C\u003EAddFactionMsg\u003C\u003EFactionColor\u003C\u003EAccessor : IMemberAccessor<MyFactionCollection.AddFactionMsg, SerializableVector3>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyFactionCollection.AddFactionMsg owner,
          in SerializableVector3 value)
        {
          owner.FactionColor = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyFactionCollection.AddFactionMsg owner,
          out SerializableVector3 value)
        {
          value = owner.FactionColor;
        }
      }

      protected class Sandbox_Game_Multiplayer_MyFactionCollection\u003C\u003EAddFactionMsg\u003C\u003EFactionIconColor\u003C\u003EAccessor : IMemberAccessor<MyFactionCollection.AddFactionMsg, SerializableVector3>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyFactionCollection.AddFactionMsg owner,
          in SerializableVector3 value)
        {
          owner.FactionIconColor = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyFactionCollection.AddFactionMsg owner,
          out SerializableVector3 value)
        {
          value = owner.FactionIconColor;
        }
      }

      protected class Sandbox_Game_Multiplayer_MyFactionCollection\u003C\u003EAddFactionMsg\u003C\u003EFactionIconGroupId\u003C\u003EAccessor : IMemberAccessor<MyFactionCollection.AddFactionMsg, SerializableDefinitionId?>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyFactionCollection.AddFactionMsg owner,
          in SerializableDefinitionId? value)
        {
          owner.FactionIconGroupId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyFactionCollection.AddFactionMsg owner,
          out SerializableDefinitionId? value)
        {
          value = owner.FactionIconGroupId;
        }
      }

      protected class Sandbox_Game_Multiplayer_MyFactionCollection\u003C\u003EAddFactionMsg\u003C\u003EFactionIconId\u003C\u003EAccessor : IMemberAccessor<MyFactionCollection.AddFactionMsg, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyFactionCollection.AddFactionMsg owner, in int value) => owner.FactionIconId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyFactionCollection.AddFactionMsg owner, out int value) => value = owner.FactionIconId;
      }

      protected class Sandbox_Game_Multiplayer_MyFactionCollection\u003C\u003EAddFactionMsg\u003C\u003EFactionScore\u003C\u003EAccessor : IMemberAccessor<MyFactionCollection.AddFactionMsg, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyFactionCollection.AddFactionMsg owner, in int value) => owner.FactionScore = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyFactionCollection.AddFactionMsg owner, out int value) => value = owner.FactionScore;
      }

      protected class Sandbox_Game_Multiplayer_MyFactionCollection\u003C\u003EAddFactionMsg\u003C\u003EFactionObjectivePercentageCompleted\u003C\u003EAccessor : IMemberAccessor<MyFactionCollection.AddFactionMsg, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyFactionCollection.AddFactionMsg owner, in float value) => owner.FactionObjectivePercentageCompleted = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyFactionCollection.AddFactionMsg owner, out float value) => value = owner.FactionObjectivePercentageCompleted;
      }

      private class Sandbox_Game_Multiplayer_MyFactionCollection\u003C\u003EAddFactionMsg\u003C\u003EActor : IActivator, IActivator<MyFactionCollection.AddFactionMsg>
      {
        object IActivator.CreateInstance() => (object) new MyFactionCollection.AddFactionMsg();

        MyFactionCollection.AddFactionMsg IActivator<MyFactionCollection.AddFactionMsg>.CreateInstance() => new MyFactionCollection.AddFactionMsg();
      }
    }

    [Serializable]
    public struct MyReputationChangeWrapper
    {
      public long FactionId;
      public int RepTotal;
      public int Change;
      public bool ShowNotification;

      protected class Sandbox_Game_Multiplayer_MyFactionCollection\u003C\u003EMyReputationChangeWrapper\u003C\u003EFactionId\u003C\u003EAccessor : IMemberAccessor<MyFactionCollection.MyReputationChangeWrapper, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyFactionCollection.MyReputationChangeWrapper owner,
          in long value)
        {
          owner.FactionId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyFactionCollection.MyReputationChangeWrapper owner,
          out long value)
        {
          value = owner.FactionId;
        }
      }

      protected class Sandbox_Game_Multiplayer_MyFactionCollection\u003C\u003EMyReputationChangeWrapper\u003C\u003ERepTotal\u003C\u003EAccessor : IMemberAccessor<MyFactionCollection.MyReputationChangeWrapper, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyFactionCollection.MyReputationChangeWrapper owner,
          in int value)
        {
          owner.RepTotal = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyFactionCollection.MyReputationChangeWrapper owner,
          out int value)
        {
          value = owner.RepTotal;
        }
      }

      protected class Sandbox_Game_Multiplayer_MyFactionCollection\u003C\u003EMyReputationChangeWrapper\u003C\u003EChange\u003C\u003EAccessor : IMemberAccessor<MyFactionCollection.MyReputationChangeWrapper, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyFactionCollection.MyReputationChangeWrapper owner,
          in int value)
        {
          owner.Change = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyFactionCollection.MyReputationChangeWrapper owner,
          out int value)
        {
          value = owner.Change;
        }
      }

      protected class Sandbox_Game_Multiplayer_MyFactionCollection\u003C\u003EMyReputationChangeWrapper\u003C\u003EShowNotification\u003C\u003EAccessor : IMemberAccessor<MyFactionCollection.MyReputationChangeWrapper, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyFactionCollection.MyReputationChangeWrapper owner,
          in bool value)
        {
          owner.ShowNotification = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyFactionCollection.MyReputationChangeWrapper owner,
          out bool value)
        {
          value = owner.ShowNotification;
        }
      }
    }

    [Serializable]
    public struct MyReputationModifiers
    {
      public float Owner;
      public float Friend;
      public float Neutral;
      public float Hostile;

      protected class Sandbox_Game_Multiplayer_MyFactionCollection\u003C\u003EMyReputationModifiers\u003C\u003EOwner\u003C\u003EAccessor : IMemberAccessor<MyFactionCollection.MyReputationModifiers, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyFactionCollection.MyReputationModifiers owner,
          in float value)
        {
          owner.Owner = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyFactionCollection.MyReputationModifiers owner,
          out float value)
        {
          value = owner.Owner;
        }
      }

      protected class Sandbox_Game_Multiplayer_MyFactionCollection\u003C\u003EMyReputationModifiers\u003C\u003EFriend\u003C\u003EAccessor : IMemberAccessor<MyFactionCollection.MyReputationModifiers, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyFactionCollection.MyReputationModifiers owner,
          in float value)
        {
          owner.Friend = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyFactionCollection.MyReputationModifiers owner,
          out float value)
        {
          value = owner.Friend;
        }
      }

      protected class Sandbox_Game_Multiplayer_MyFactionCollection\u003C\u003EMyReputationModifiers\u003C\u003ENeutral\u003C\u003EAccessor : IMemberAccessor<MyFactionCollection.MyReputationModifiers, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyFactionCollection.MyReputationModifiers owner,
          in float value)
        {
          owner.Neutral = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyFactionCollection.MyReputationModifiers owner,
          out float value)
        {
          value = owner.Neutral;
        }
      }

      protected class Sandbox_Game_Multiplayer_MyFactionCollection\u003C\u003EMyReputationModifiers\u003C\u003EHostile\u003C\u003EAccessor : IMemberAccessor<MyFactionCollection.MyReputationModifiers, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyFactionCollection.MyReputationModifiers owner,
          in float value)
        {
          owner.Hostile = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyFactionCollection.MyReputationModifiers owner,
          out float value)
        {
          value = owner.Hostile;
        }
      }
    }

    protected sealed class CreateFactionByDefinition\u003C\u003ESystem_String : ICallSite<IMyEventOwner, string, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string tag,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyFactionCollection.CreateFactionByDefinition(tag);
      }
    }

    protected sealed class UnlockAchievementForClient\u003C\u003ESystem_String : ICallSite<IMyEventOwner, string, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string achievement,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyFactionCollection.UnlockAchievementForClient(achievement);
      }
    }

    protected sealed class Invoke_AddRep_DEBUG\u003C\u003ESystem_Int64\u0023System_Int64\u0023System_Int32 : ICallSite<IMyEventOwner, long, long, int, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long playerId,
        in long factionId,
        in int delta,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyFactionCollection.Invoke_AddRep_DEBUG(playerId, factionId, delta);
      }
    }

    protected sealed class AddFactionPlayerReputationSuccess\u003C\u003ESystem_Int64\u0023System_Collections_Generic_List`1\u003CSandbox_Game_Multiplayer_MyFactionCollection\u003C\u003EMyReputationChangeWrapper\u003E : ICallSite<IMyEventOwner, long, List<MyFactionCollection.MyReputationChangeWrapper>, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long playerId,
        in List<MyFactionCollection.MyReputationChangeWrapper> changes,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyFactionCollection.AddFactionPlayerReputationSuccess(playerId, changes);
      }
    }

    protected sealed class FactionStateChangeRequest\u003C\u003EVRage_Game_ModAPI_MyFactionStateChange\u0023System_Int64\u0023System_Int64\u0023System_Int64 : ICallSite<IMyEventOwner, MyFactionStateChange, long, long, long, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyFactionStateChange action,
        in long fromFactionId,
        in long toFactionId,
        in long playerId,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyFactionCollection.FactionStateChangeRequest(action, fromFactionId, toFactionId, playerId);
      }
    }

    protected sealed class FactionStateChangeSuccess\u003C\u003EVRage_Game_ModAPI_MyFactionStateChange\u0023System_Int64\u0023System_Int64\u0023System_Int64\u0023System_Int64 : ICallSite<IMyEventOwner, MyFactionStateChange, long, long, long, long, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyFactionStateChange action,
        in long fromFactionId,
        in long toFactionId,
        in long playerId,
        in long senderId,
        in DBNull arg6)
      {
        MyFactionCollection.FactionStateChangeSuccess(action, fromFactionId, toFactionId, playerId, senderId);
      }
    }

    protected sealed class ChangeAutoAcceptRequest\u003C\u003ESystem_Int64\u0023System_Boolean\u0023System_Boolean : ICallSite<IMyEventOwner, long, bool, bool, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long factionId,
        in bool autoAcceptMember,
        in bool autoAcceptPeace,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyFactionCollection.ChangeAutoAcceptRequest(factionId, autoAcceptMember, autoAcceptPeace);
      }
    }

    protected sealed class ChangeAutoAcceptSuccess\u003C\u003ESystem_Int64\u0023System_Boolean\u0023System_Boolean : ICallSite<IMyEventOwner, long, bool, bool, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long factionId,
        in bool autoAcceptMember,
        in bool autoAcceptPeace,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyFactionCollection.ChangeAutoAcceptSuccess(factionId, autoAcceptMember, autoAcceptPeace);
      }
    }

    protected sealed class EditFactionRequest\u003C\u003ESandbox_Game_Multiplayer_MyFactionCollection\u003C\u003EAddFactionMsg : ICallSite<IMyEventOwner, MyFactionCollection.AddFactionMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyFactionCollection.AddFactionMsg msgEdit,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyFactionCollection.EditFactionRequest(msgEdit);
      }
    }

    protected sealed class EditFactionSuccess\u003C\u003ESandbox_Game_Multiplayer_MyFactionCollection\u003C\u003EAddFactionMsg : ICallSite<IMyEventOwner, MyFactionCollection.AddFactionMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyFactionCollection.AddFactionMsg msgEdit,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyFactionCollection.EditFactionSuccess(msgEdit);
      }
    }

    protected sealed class RequestFactionScoreAndPercentageUpdate\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long factionId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyFactionCollection.RequestFactionScoreAndPercentageUpdate(factionId);
      }
    }

    protected sealed class RecieveFactionScoreAndPercentage\u003C\u003ESystem_Int64\u0023System_Int32\u0023System_Single : ICallSite<IMyEventOwner, long, int, float, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long factionId,
        in int score,
        in float percentageFinished,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyFactionCollection.RecieveFactionScoreAndPercentage(factionId, score, percentageFinished);
      }
    }

    protected sealed class CreateFactionRequest\u003C\u003ESandbox_Game_Multiplayer_MyFactionCollection\u003C\u003EAddFactionMsg : ICallSite<IMyEventOwner, MyFactionCollection.AddFactionMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyFactionCollection.AddFactionMsg msg,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyFactionCollection.CreateFactionRequest(msg);
      }
    }

    protected sealed class CreateFactionSuccess\u003C\u003ESandbox_Game_Multiplayer_MyFactionCollection\u003C\u003EAddFactionMsg : ICallSite<IMyEventOwner, MyFactionCollection.AddFactionMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyFactionCollection.AddFactionMsg msg,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyFactionCollection.CreateFactionSuccess(msg);
      }
    }

    protected sealed class SetDefaultFactionStates\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long recivedFactionId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyFactionCollection.SetDefaultFactionStates(recivedFactionId);
      }
    }

    protected sealed class AddDiscoveredFaction_Clients\u003C\u003ESystem_UInt64\u0023System_Int32\u0023System_Int64 : ICallSite<IMyEventOwner, ulong, int, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in ulong playerId,
        in int serialId,
        in long factionId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyFactionCollection.AddDiscoveredFaction_Clients(playerId, serialId, factionId);
      }
    }

    protected sealed class RemoveDiscoveredFaction_Clients\u003C\u003ESystem_UInt64\u0023System_Int32\u0023System_Int64 : ICallSite<IMyEventOwner, ulong, int, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in ulong playerId,
        in int serialId,
        in long factionId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyFactionCollection.RemoveDiscoveredFaction_Clients(playerId, serialId, factionId);
      }
    }

    protected sealed class RemovePlayerFromVisibility_Broadcast\u003C\u003ESandbox_Game_World_MyPlayer\u003C\u003EPlayerId : ICallSite<IMyEventOwner, MyPlayer.PlayerId, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyPlayer.PlayerId playerId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyFactionCollection.RemovePlayerFromVisibility_Broadcast(playerId);
      }
    }
  }
}
