// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.IMyFactionCollection
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;

namespace VRage.Game.ModAPI
{
  public interface IMyFactionCollection
  {
    bool FactionTagExists(string tag, IMyFaction doNotCheck = null);

    bool FactionNameExists(string name, IMyFaction doNotCheck = null);

    IMyFaction TryGetFactionById(long factionId);

    IMyFaction TryGetPlayerFaction(long playerId);

    IMyFaction TryGetFactionByTag(string tag);

    IMyFaction TryGetFactionByName(string name);

    [Obsolete("Use SendJoinRequest instead, this will be removed in future")]
    void AddPlayerToFaction(long playerId, long factionId);

    [Obsolete("Use KickMember instead, this will be removed in future")]
    void KickPlayerFromFaction(long playerId);

    MyRelationsBetweenFactions GetRelationBetweenFactions(
      long factionId1,
      long factionId2);

    int GetReputationBetweenFactions(long factionId1, long factionId2);

    void SetReputation(long fromFactionId, long toFactionId, int reputation);

    int GetReputationBetweenPlayerAndFaction(long factionId1, long factionId2);

    void SetReputationBetweenPlayerAndFaction(long identityId, long factionId, int reputation);

    bool AreFactionsEnemies(long factionId1, long factionId2);

    bool IsPeaceRequestStateSent(long myFactionId, long foreignFactionId);

    bool IsPeaceRequestStatePending(long myFactionId, long foreignFactionId);

    void RemoveFaction(long factionId);

    void SendPeaceRequest(long fromFactionId, long toFactionId);

    void CancelPeaceRequest(long fromFactionId, long toFactionId);

    void AcceptPeace(long fromFactionId, long toFactionId);

    void DeclareWar(long fromFactionId, long toFactionId);

    void SendJoinRequest(long factionId, long playerId);

    void CancelJoinRequest(long factionId, long playerId);

    void AcceptJoin(long factionId, long playerId);

    void KickMember(long factionId, long playerId);

    void PromoteMember(long factionId, long playerId);

    void DemoteMember(long factionId, long playerId);

    void MemberLeaves(long factionId, long playerId);

    event Action<long, bool, bool> FactionAutoAcceptChanged;

    void ChangeAutoAccept(
      long factionId,
      long playerId,
      bool autoAcceptMember,
      bool autoAcceptPeace);

    event Action<long> FactionEdited;

    void EditFaction(long factionId, string tag, string name, string desc, string privateInfo);

    void CreateFaction(long founderId, string tag, string name, string desc, string privateInfo);

    void CreateFaction(
      long founderId,
      string tag,
      string name,
      string desc,
      string privateInfo,
      MyFactionTypes type);

    void CreateNPCFaction(string tag, string name, string desc, string privateInfo);

    event Action<long> FactionCreated;

    MyObjectBuilder_FactionCollection GetObjectBuilder();

    Dictionary<long, IMyFaction> Factions { get; }

    void AddNewNPCToFaction(long factionId);

    void AddNewNPCToFaction(long factionId, string npcName);

    event Action<MyFactionStateChange, long, long, long, long> FactionStateChanged;
  }
}
