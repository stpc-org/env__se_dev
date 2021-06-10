// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MyFaction
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.World
{
  public class MyFaction : IMyFaction
  {
    private Dictionary<long, MyFactionMember> m_members;
    private Dictionary<long, MyFactionMember> m_joinRequests;
    private Dictionary<long, MyStation> m_stations;
    private MyBlockLimits m_blockLimits;
    public long FactionId;
    public int Score;
    public float ObjectivePercentageCompleted;
    public string Tag;
    public string Name;
    public string Description;
    public string PrivateInfo;
    public MyStringId? FactionIcon;
    public bool AutoAcceptMember;
    public bool AutoAcceptPeace;
    public bool AcceptHumans;
    public bool EnableFriendlyFire = true;
    public MyFactionTypes FactionType;

    public long FounderId { get; private set; }

    public Vector3 CustomColor { get; set; }

    public Vector3 IconColor { get; set; }

    public DictionaryReader<long, MyFactionMember> Members => new DictionaryReader<long, MyFactionMember>(this.m_members);

    public DictionaryReader<long, MyFactionMember> JoinRequests => new DictionaryReader<long, MyFactionMember>(this.m_joinRequests);

    public DictionaryValuesReader<long, MyStation> Stations => new DictionaryValuesReader<long, MyStation>(this.m_stations);

    public bool IsAnyLeaderOnline
    {
      get
      {
        ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
        foreach (KeyValuePair<long, MyFactionMember> member1 in this.m_members)
        {
          KeyValuePair<long, MyFactionMember> member = member1;
          if (member.Value.IsLeader && onlinePlayers.Any<MyPlayer>((Func<MyPlayer, bool>) (x => x.Identity.IdentityId == member.Value.PlayerId)))
            return true;
        }
        return false;
      }
    }

    public MyFaction(
      long id,
      string tag,
      string name,
      string desc,
      string privateInfo,
      long creatorId,
      MyFactionTypes factionType = MyFactionTypes.None,
      Vector3? customColor = null,
      Vector3? factionIconColor = null,
      string factionIcon = "",
      int score = 0,
      float objectivePercentageCompleted = 0.0f)
    {
      this.FactionId = id;
      this.Tag = tag;
      this.Name = name;
      this.Description = desc;
      this.PrivateInfo = privateInfo;
      this.FounderId = creatorId;
      this.FactionType = factionType;
      this.FactionIcon = new MyStringId?(MyStringId.GetOrCompute(factionIcon));
      this.CustomColor = customColor.HasValue ? customColor.Value : MyColorPickerConstants.HSVToHSVOffset(Vector3.Zero);
      this.IconColor = factionIconColor.HasValue ? factionIconColor.Value : MyColorPickerConstants.HSVToHSVOffset(Vector3.Zero);
      this.Score = score;
      this.ObjectivePercentageCompleted = objectivePercentageCompleted;
      this.AutoAcceptMember = false;
      this.AutoAcceptPeace = false;
      this.AcceptHumans = true;
      this.m_members = new Dictionary<long, MyFactionMember>();
      this.m_joinRequests = new Dictionary<long, MyFactionMember>();
      this.m_stations = new Dictionary<long, MyStation>();
      this.m_blockLimits = new MyBlockLimits(MyBlockLimits.GetInitialPCU(), 0);
      this.m_members.Add(creatorId, new MyFactionMember(creatorId, true, true));
    }

    public MyFaction(
      long id,
      long creatorId,
      string privateInfo,
      MyFactionDefinition factionDefinition,
      Vector3? customColor = null,
      Vector3? iconColor = null)
    {
      this.FactionId = id;
      this.Tag = factionDefinition.Tag;
      this.Name = factionDefinition.DisplayNameText;
      this.Description = factionDefinition.DescriptionText;
      this.PrivateInfo = privateInfo;
      this.FounderId = creatorId;
      this.FactionType = factionDefinition.Type;
      this.FactionIcon = new MyStringId?(factionDefinition.FactionIcon);
      this.CustomColor = customColor.HasValue ? customColor.Value : MyColorPickerConstants.HSVToHSVOffset(Vector3.Zero);
      this.IconColor = iconColor.HasValue ? iconColor.Value : MyColorPickerConstants.HSVToHSVOffset(Vector3.Zero);
      this.Score = factionDefinition.Score;
      this.ObjectivePercentageCompleted = factionDefinition.ObjectivePercentageCompleted;
      this.m_blockLimits = new MyBlockLimits(MyBlockLimits.GetInitialPCU(), 0);
      this.AutoAcceptMember = factionDefinition.AutoAcceptMember;
      this.AutoAcceptPeace = false;
      this.AcceptHumans = factionDefinition.AcceptHumans;
      this.EnableFriendlyFire = factionDefinition.EnableFriendlyFire;
      this.m_members = new Dictionary<long, MyFactionMember>();
      this.m_joinRequests = new Dictionary<long, MyFactionMember>();
      this.m_stations = new Dictionary<long, MyStation>();
      this.m_members.Add(creatorId, new MyFactionMember(creatorId, true, true));
    }

    public MyFaction(MyObjectBuilder_Faction obj)
    {
      this.FactionId = obj.FactionId;
      this.Tag = obj.Tag;
      this.Name = obj.Name;
      this.Description = obj.Description;
      this.PrivateInfo = obj.PrivateInfo;
      this.Score = obj.Score;
      this.ObjectivePercentageCompleted = obj.ObjectivePercentageCompleted;
      this.AutoAcceptMember = obj.AutoAcceptMember;
      this.AutoAcceptPeace = obj.AutoAcceptPeace;
      this.EnableFriendlyFire = obj.EnableFriendlyFire;
      this.AcceptHumans = obj.AcceptHumans;
      this.FactionType = obj.FactionType;
      this.m_blockLimits = new MyBlockLimits(MyBlockLimits.GetInitialPCU(), 0, obj.TransferedPCUDelta);
      this.m_members = new Dictionary<long, MyFactionMember>(obj.Members.Count);
      foreach (MyObjectBuilder_FactionMember member in obj.Members)
      {
        this.m_members.Add(member.PlayerId, (MyFactionMember) member);
        if (member.IsFounder)
          this.FounderId = member.PlayerId;
      }
      if (obj.JoinRequests != null)
      {
        this.m_joinRequests = new Dictionary<long, MyFactionMember>(obj.JoinRequests.Count);
        foreach (MyObjectBuilder_FactionMember joinRequest in obj.JoinRequests)
          this.m_joinRequests.Add(joinRequest.PlayerId, (MyFactionMember) joinRequest);
      }
      else
        this.m_joinRequests = new Dictionary<long, MyFactionMember>();
      MyFactionDefinition factionDefinition = MyDefinitionManager.Static.TryGetFactionDefinition(this.Tag);
      if (factionDefinition != null)
      {
        this.AutoAcceptMember = factionDefinition.AutoAcceptMember;
        this.AcceptHumans = factionDefinition.AcceptHumans;
        this.EnableFriendlyFire = factionDefinition.EnableFriendlyFire;
        this.Name = factionDefinition.DisplayNameText;
        this.Description = factionDefinition.DescriptionText;
        this.Score = factionDefinition.Score;
        this.ObjectivePercentageCompleted = factionDefinition.ObjectivePercentageCompleted;
      }
      this.m_stations = new Dictionary<long, MyStation>();
      foreach (MyObjectBuilder_Station station in obj.Stations)
      {
        MyStation myStation = new MyStation(station);
        this.m_stations.Add(myStation.Id, myStation);
      }
      this.CustomColor = (Vector3) obj.CustomColor;
      this.IconColor = (Vector3) obj.IconColor;
      if (obj.FactionIcon != null)
        this.FactionIcon = new MyStringId?(MyStringId.GetOrCompute(obj.FactionIcon));
      else if (factionDefinition == null)
        this.FactionIcon = new MyStringId?(MyStringId.GetOrCompute("Textures\\FactionLogo\\Empty.dds"));
      this.CheckAndFixFactionRanks();
    }

    public bool IsFounder(long playerId)
    {
      MyFactionMember myFactionMember;
      return this.m_members.TryGetValue(playerId, out myFactionMember) && myFactionMember.IsFounder;
    }

    public bool IsLeader(long playerId)
    {
      MyFactionMember myFactionMember;
      return this.m_members.TryGetValue(playerId, out myFactionMember) && myFactionMember.IsLeader;
    }

    public bool IsMember(long playerId) => this.m_members.TryGetValue(playerId, out MyFactionMember _);

    public bool IsNeutral(long playerId)
    {
      IMyFaction playerFaction = MySession.Static.Factions.TryGetPlayerFaction(playerId);
      return playerFaction != null ? MySession.Static.Factions.GetRelationBetweenFactions(playerFaction.FactionId, this.FactionId).Item1 == MyRelationsBetweenFactions.Neutral : MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(playerId, this.FactionId).Item1 == MyRelationsBetweenFactions.Neutral;
    }

    public bool IsEnemy(long playerId)
    {
      IMyFaction playerFaction = MySession.Static.Factions.TryGetPlayerFaction(playerId);
      return playerFaction != null ? MySession.Static.Factions.GetRelationBetweenFactions(playerFaction.FactionId, this.FactionId).Item1 == MyRelationsBetweenFactions.Enemies : MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(playerId, this.FactionId).Item1 == MyRelationsBetweenFactions.Enemies;
    }

    public bool IsFriendly(long playerId)
    {
      IMyFaction playerFaction = MySession.Static.Factions.TryGetPlayerFaction(playerId);
      return playerFaction != null ? MySession.Static.Factions.GetRelationBetweenFactions(playerFaction.FactionId, this.FactionId).Item1 == MyRelationsBetweenFactions.Friends : MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(playerId, this.FactionId).Item1 == MyRelationsBetweenFactions.Friends;
    }

    public bool IsEveryoneNpc()
    {
      foreach (KeyValuePair<long, MyFactionMember> member in this.m_members)
      {
        if (!Sync.Players.IdentityIsNpc(member.Key))
          return false;
      }
      return true;
    }

    public void AddJoinRequest(long playerId) => this.m_joinRequests[playerId] = new MyFactionMember(playerId, false);

    public void CancelJoinRequest(long playerId) => this.m_joinRequests.Remove(playerId);

    public void AcceptJoin(long playerId, bool autoaccept = false)
    {
      MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(playerId);
      if (playerFaction != null)
      {
        playerFaction.KickMember(playerId, false);
        if (MySession.Static.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.PER_FACTION)
          MyBlockLimits.TransferBlockLimits(playerId, playerFaction.BlockLimits, this.BlockLimits);
      }
      MySession.Static.Factions.AddPlayerToFactionInternal(playerId, this.FactionId);
      if (this.m_joinRequests.ContainsKey(playerId))
      {
        this.m_members[playerId] = this.m_joinRequests[playerId];
        this.m_joinRequests.Remove(playerId);
      }
      else if (this.AutoAcceptMember | autoaccept)
        this.m_members[playerId] = new MyFactionMember(playerId, false);
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(playerId);
      if (identity != null)
      {
        identity.BlockLimits.SetAllDirty();
        identity.RaiseFactionChanged(playerFaction, this);
      }
      MySession.Static.Factions.InvokePlayerJoined(this, playerId);
    }

    public void KickMember(long playerId, bool raiseChanged = true)
    {
      this.m_members.Remove(playerId);
      MySession.Static.Factions.KickPlayerFromFaction(playerId);
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(playerId);
      if (raiseChanged && identity != null)
        identity.RaiseFactionChanged(this, (MyFaction) null);
      this.CheckAndFixFactionRanks();
      MySession.Static.Factions.InvokePlayerLeft(this, playerId);
    }

    public void PromoteMember(long playerId)
    {
      MyFactionMember myFactionMember;
      if (!this.m_members.TryGetValue(playerId, out myFactionMember))
        return;
      myFactionMember.IsLeader = true;
      this.m_members[playerId] = myFactionMember;
    }

    public void DemoteMember(long playerId)
    {
      MyFactionMember myFactionMember;
      if (!this.m_members.TryGetValue(playerId, out myFactionMember))
        return;
      myFactionMember.IsLeader = false;
      this.m_members[playerId] = myFactionMember;
    }

    public void PromoteToFounder(long playerId)
    {
      MyFactionMember myFactionMember;
      if (!this.m_members.TryGetValue(playerId, out myFactionMember))
        return;
      myFactionMember.IsLeader = true;
      myFactionMember.IsFounder = true;
      this.m_members[playerId] = myFactionMember;
      this.FounderId = playerId;
    }

    public void CheckAndFixFactionRanks()
    {
      if (this.HasFounder())
        return;
      foreach (KeyValuePair<long, MyFactionMember> member in this.m_members)
      {
        if (member.Value.IsLeader)
        {
          this.PromoteToFounder(member.Key);
          return;
        }
      }
      if (this.m_members.Count <= 0)
        return;
      this.PromoteToFounder(this.m_members.Keys.FirstOrDefault<long>());
    }

    private bool HasFounder()
    {
      MyFactionMember myFactionMember;
      return this.m_members.TryGetValue(this.FounderId, out myFactionMember) && myFactionMember.IsFounder;
    }

    public MyObjectBuilder_Faction GetObjectBuilder()
    {
      MyObjectBuilder_Faction objectBuilderFaction = new MyObjectBuilder_Faction();
      objectBuilderFaction.FactionId = this.FactionId;
      objectBuilderFaction.Tag = this.Tag;
      objectBuilderFaction.Name = this.Name;
      objectBuilderFaction.Description = this.Description;
      objectBuilderFaction.PrivateInfo = this.PrivateInfo;
      objectBuilderFaction.AutoAcceptMember = this.AutoAcceptMember;
      objectBuilderFaction.AutoAcceptPeace = this.AutoAcceptPeace;
      objectBuilderFaction.EnableFriendlyFire = this.EnableFriendlyFire;
      objectBuilderFaction.AcceptHumans = this.AcceptHumans;
      objectBuilderFaction.FactionType = this.FactionType;
      objectBuilderFaction.Members = new List<MyObjectBuilder_FactionMember>(this.Members.Count);
      foreach (KeyValuePair<long, MyFactionMember> member in this.Members)
        objectBuilderFaction.Members.Add((MyObjectBuilder_FactionMember) member.Value);
      objectBuilderFaction.JoinRequests = new List<MyObjectBuilder_FactionMember>(this.JoinRequests.Count);
      foreach (KeyValuePair<long, MyFactionMember> joinRequest in this.JoinRequests)
        objectBuilderFaction.JoinRequests.Add((MyObjectBuilder_FactionMember) joinRequest.Value);
      objectBuilderFaction.Stations = new List<MyObjectBuilder_Station>(this.Stations.Count);
      foreach (MyStation station in this.Stations)
        objectBuilderFaction.Stations.Add(station.GetObjectBuilder());
      objectBuilderFaction.CustomColor = (SerializableVector3) this.CustomColor;
      objectBuilderFaction.IconColor = (SerializableVector3) this.IconColor;
      objectBuilderFaction.FactionIcon = this.FactionIcon.HasValue ? this.FactionIcon.Value.String : (string) null;
      objectBuilderFaction.TransferedPCUDelta = this.m_blockLimits.TransferedDelta;
      objectBuilderFaction.Score = this.Score;
      objectBuilderFaction.ObjectivePercentageCompleted = this.ObjectivePercentageCompleted;
      return objectBuilderFaction;
    }

    public MyBlockLimits BlockLimits => this.m_blockLimits;

    public void AddStation(MyStation station) => this.m_stations.Add(station.Id, station);

    public MyStation GetStationById(long stationId)
    {
      MyStation myStation;
      this.m_stations.TryGetValue(stationId, out myStation);
      return myStation;
    }

    long IMyFaction.FactionId => this.FactionId;

    string IMyFaction.Tag => this.Tag;

    string IMyFaction.Name => this.Name;

    string IMyFaction.Description => this.Description;

    string IMyFaction.PrivateInfo => this.PrivateInfo;

    MyStringId? IMyFaction.FactionIcon => this.FactionIcon;

    bool IMyFaction.AutoAcceptMember => this.AutoAcceptMember;

    bool IMyFaction.AutoAcceptPeace => this.AutoAcceptPeace;

    bool IMyFaction.AcceptHumans => this.AcceptHumans;

    long IMyFaction.FounderId => this.FounderId;

    int IMyFaction.Score
    {
      get => this.Score;
      set => this.Score = value;
    }

    float IMyFaction.ObjectivePercentageCompleted
    {
      get => this.ObjectivePercentageCompleted;
      set => this.ObjectivePercentageCompleted = value;
    }

    bool IMyFaction.IsFounder(long playerId) => this.IsFounder(playerId);

    bool IMyFaction.IsLeader(long playerId) => this.IsLeader(playerId);

    bool IMyFaction.IsMember(long playerId) => this.IsMember(playerId);

    bool IMyFaction.IsNeutral(long playerId) => this.IsNeutral(playerId);

    bool IMyFaction.IsEnemy(long playerId) => this.IsEnemy(playerId);

    bool IMyFaction.IsFriendly(long playerId) => this.IsFriendly(playerId);

    bool IMyFaction.TryGetBalanceInfo(out long balance)
    {
      balance = 0L;
      MyAccountInfo account;
      if (MyBankingSystem.Static == null || !MyBankingSystem.Static.TryGetAccountInfo(this.FactionId, out account))
        return false;
      balance = account.Balance;
      return true;
    }

    string IMyFaction.GetBalanceShortString() => MyBankingSystem.Static == null ? (string) null : MyBankingSystem.Static.GetBalanceShortString(this.FactionId);

    void IMyFaction.RequestChangeBalance(long amount)
    {
      if (MyBankingSystem.Static == null)
        return;
      MyBankingSystem.ChangeBalance(this.FactionId, amount);
    }

    DictionaryReader<long, MyFactionMember> IMyFaction.Members => this.Members;

    DictionaryReader<long, MyFactionMember> IMyFaction.JoinRequests => this.JoinRequests;
  }
}
