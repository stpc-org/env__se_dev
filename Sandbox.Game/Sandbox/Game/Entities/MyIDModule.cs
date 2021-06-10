// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyIDModule
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Utils;

namespace Sandbox.Game.Entities
{
  public class MyIDModule
  {
    public long Owner { get; set; }

    public MyOwnershipShareModeEnum ShareMode { get; set; }

    public MyIDModule()
      : this(0L, MyOwnershipShareModeEnum.None)
    {
    }

    public MyIDModule(long owner, MyOwnershipShareModeEnum shareMode)
    {
      this.Owner = owner;
      this.ShareMode = shareMode;
    }

    public MyRelationsBetweenPlayerAndBlock GetUserRelationToOwner(
      long identityId)
    {
      return MyIDModule.GetRelationPlayerBlock(this.Owner, identityId, this.ShareMode);
    }

    public static MyRelationsBetweenPlayers GetRelationPlayerPlayer(
      long owner,
      long user,
      MyRelationsBetweenFactions defaultFactionRelations = MyRelationsBetweenFactions.Enemies,
      MyRelationsBetweenPlayers defaultNoFactionRelation = MyRelationsBetweenPlayers.Enemies)
    {
      if (owner == user)
        return MyRelationsBetweenPlayers.Self;
      IMyFaction playerFaction1 = MySession.Static.Factions.TryGetPlayerFaction(user);
      IMyFaction playerFaction2 = MySession.Static.Factions.TryGetPlayerFaction(owner);
      if (playerFaction1 == null && playerFaction2 == null)
        return defaultNoFactionRelation;
      if (playerFaction1 == playerFaction2)
        return MyRelationsBetweenPlayers.Allies;
      if (playerFaction1 == null)
        return MyIDModule.ConvertToPlayerRelation(MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(user, playerFaction2.FactionId).Item1);
      if (playerFaction2 == null)
        return MyIDModule.ConvertToPlayerRelation(MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(owner, playerFaction1.FactionId).Item1);
      int num = 0;
      if (MySession.Static != null)
      {
        MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
        if (component != null)
          num = component.TranslateRelationToReputation(defaultFactionRelations);
      }
      bool flag1 = MySession.Static.Factions.IsNpcFaction(playerFaction1.Tag);
      bool flag2 = MySession.Static.Factions.IsNpcFaction(playerFaction2.Tag);
      if (flag1 != flag2)
      {
        if (flag1)
          return MyIDModule.ConvertToPlayerRelation(MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(owner, playerFaction1.FactionId).Item1);
        if (flag2)
          return MyIDModule.ConvertToPlayerRelation(MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(user, playerFaction2.FactionId).Item1);
      }
      return MyIDModule.ConvertToPlayerRelation(MySession.Static.Factions.GetRelationBetweenFactions(playerFaction2.FactionId, playerFaction1.FactionId, new Tuple<MyRelationsBetweenFactions, int>(defaultFactionRelations, num)).Item1);
    }

    public static MyRelationsBetweenPlayerAndBlock GetRelationPlayerBlock(
      long owner,
      long user,
      MyOwnershipShareModeEnum share = MyOwnershipShareModeEnum.None,
      MyRelationsBetweenPlayerAndBlock noFactionResult = MyRelationsBetweenPlayerAndBlock.Enemies,
      MyRelationsBetweenFactions defaultFactionRelations = MyRelationsBetweenFactions.Enemies,
      MyRelationsBetweenPlayerAndBlock defaultShareWithAllRelations = MyRelationsBetweenPlayerAndBlock.FactionShare)
    {
      if (!MyFakes.SHOW_FACTIONS_GUI)
        return MyRelationsBetweenPlayerAndBlock.NoOwnership;
      if (owner == user)
        return MyRelationsBetweenPlayerAndBlock.Owner;
      if (owner == 0L || user == 0L)
        return MyRelationsBetweenPlayerAndBlock.NoOwnership;
      if (MySession.Static == null || MySession.Static.Factions == null)
      {
        MyLog.Default.Error("Session or factions not ready - GetRelationshipPlayerBlock");
        return MyRelationsBetweenPlayerAndBlock.Enemies;
      }
      IMyFaction playerFaction1 = MySession.Static.Factions.TryGetPlayerFaction(user);
      IMyFaction playerFaction2 = MySession.Static.Factions.TryGetPlayerFaction(owner);
      if (playerFaction1 != null && playerFaction1 == playerFaction2 && share == MyOwnershipShareModeEnum.Faction)
        return MyRelationsBetweenPlayerAndBlock.FactionShare;
      if (share == MyOwnershipShareModeEnum.All)
        return defaultShareWithAllRelations;
      return playerFaction1 == null && playerFaction2 == null ? noFactionResult : MyIDModule.ConvertToPlayerBlockRelation(MyIDModule.GetRelationPlayerPlayer(user, owner, defaultFactionRelations, MyIDModule.ConvertToPlayerRelation(noFactionResult)));
    }

    private static MyRelationsBetweenPlayerAndBlock ConvertToPlayerBlockRelation(
      MyRelationsBetweenFactions factionRelation)
    {
      switch (factionRelation)
      {
        case MyRelationsBetweenFactions.Neutral:
          return MyRelationsBetweenPlayerAndBlock.Neutral;
        case MyRelationsBetweenFactions.Enemies:
          return MyRelationsBetweenPlayerAndBlock.Enemies;
        case MyRelationsBetweenFactions.Allies:
        case MyRelationsBetweenFactions.Friends:
          return MyRelationsBetweenPlayerAndBlock.Friends;
        default:
          return MyRelationsBetweenPlayerAndBlock.Enemies;
      }
    }

    private static MyRelationsBetweenPlayerAndBlock ConvertToPlayerBlockRelation(
      MyRelationsBetweenPlayers playerRelation)
    {
      switch (playerRelation)
      {
        case MyRelationsBetweenPlayers.Self:
        case MyRelationsBetweenPlayers.Allies:
          return MyRelationsBetweenPlayerAndBlock.Friends;
        case MyRelationsBetweenPlayers.Neutral:
          return MyRelationsBetweenPlayerAndBlock.Neutral;
        case MyRelationsBetweenPlayers.Enemies:
          return MyRelationsBetweenPlayerAndBlock.Enemies;
        default:
          return MyRelationsBetweenPlayerAndBlock.Enemies;
      }
    }

    private static MyRelationsBetweenPlayers ConvertToPlayerRelation(
      MyRelationsBetweenFactions factionRelation)
    {
      switch (factionRelation)
      {
        case MyRelationsBetweenFactions.Neutral:
          return MyRelationsBetweenPlayers.Neutral;
        case MyRelationsBetweenFactions.Enemies:
          return MyRelationsBetweenPlayers.Enemies;
        case MyRelationsBetweenFactions.Allies:
        case MyRelationsBetweenFactions.Friends:
          return MyRelationsBetweenPlayers.Allies;
        default:
          return MyRelationsBetweenPlayers.Enemies;
      }
    }

    private static MyRelationsBetweenPlayers ConvertToPlayerRelation(
      MyRelationsBetweenPlayerAndBlock blockRelation)
    {
      switch (blockRelation)
      {
        case MyRelationsBetweenPlayerAndBlock.NoOwnership:
        case MyRelationsBetweenPlayerAndBlock.Neutral:
          return MyRelationsBetweenPlayers.Neutral;
        case MyRelationsBetweenPlayerAndBlock.Owner:
        case MyRelationsBetweenPlayerAndBlock.FactionShare:
        case MyRelationsBetweenPlayerAndBlock.Friends:
          return MyRelationsBetweenPlayers.Allies;
        case MyRelationsBetweenPlayerAndBlock.Enemies:
          return MyRelationsBetweenPlayers.Enemies;
        default:
          return MyRelationsBetweenPlayers.Enemies;
      }
    }
  }
}
