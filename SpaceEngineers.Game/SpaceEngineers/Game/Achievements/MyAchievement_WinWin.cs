// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Achievements.MyAchievement_WinWin
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using VRage.Game;
using VRage.Game.ModAPI;

namespace SpaceEngineers.Game.Achievements
{
  internal class MyAchievement_WinWin : MySteamAchievementBase
  {
    protected override (string, string, float) GetAchievementInfo() => (nameof (MyAchievement_WinWin), (string) null, 0.0f);

    public override bool NeedsUpdate => false;

    public override void SessionBeforeStart()
    {
      if (this.IsAchieved)
        return;
      MySession.Static.Factions.FactionStateChanged += new Action<MyFactionStateChange, long, long, long, long>(this.Factions_FactionStateChanged);
    }

    private void Factions_FactionStateChanged(
      MyFactionStateChange action,
      long fromFactionId,
      long toFactionId,
      long playerId,
      long senderId)
    {
      if (MySession.Static.LocalHumanPlayer == null)
        return;
      long identityId = MySession.Static.LocalHumanPlayer.Identity.IdentityId;
      IMyFaction playerFaction = MySession.Static.Factions.TryGetPlayerFaction(identityId);
      if (playerFaction == null || !(MySession.Static.Factions.TryGetFactionById(toFactionId) is MyFaction factionById) || (factionById.FactionType != MyFactionTypes.PlayerMade || factionById.IsEveryoneNpc()) || (!playerFaction.IsLeader(identityId) && !playerFaction.IsFounder(identityId) || playerFaction.FactionId != fromFactionId && playerFaction.FactionId != toFactionId) || action != MyFactionStateChange.AcceptPeace)
        return;
      this.NotifyAchieved();
      MySession.Static.Factions.FactionStateChanged -= new Action<MyFactionStateChange, long, long, long, long>(this.Factions_FactionStateChanged);
    }
  }
}
