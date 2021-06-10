// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Achievements.MyAchievement_LostInSpace
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Achievements
{
  public class MyAchievement_LostInSpace : MySteamAchievementBase
  {
    public const int CHECK_INTERVAL_MS = 3000;
    public const int MAXIMUM_TIME_S = 3600;
    private int m_startedS;
    private double m_lastTimeChecked;
    private bool m_conditionsValid;
    private List<MyPhysics.HitInfo> m_hitInfoResults;

    protected override (string, string, float) GetAchievementInfo() => (nameof (MyAchievement_LostInSpace), "LostInSpace_LostInSpaceStartedS", 3600f);

    public override bool NeedsUpdate => this.m_conditionsValid;

    protected override void LoadStatValue() => this.m_startedS = this.m_remoteAchievement.StatValueInt;

    public override void SessionLoad()
    {
      this.m_conditionsValid = MyMultiplayer.Static != null;
      this.m_lastTimeChecked = 0.0;
    }

    public override void SessionUpdate()
    {
      if (!this.m_conditionsValid)
        return;
      double totalMilliseconds = MySession.Static.ElapsedPlayTime.TotalMilliseconds;
      double num = totalMilliseconds - this.m_lastTimeChecked;
      if (num <= 3000.0)
        return;
      this.m_lastTimeChecked = totalMilliseconds;
      this.m_startedS += (int) (num / 1000.0);
      if (MySession.Static.Players.GetOnlinePlayerCount() == 1)
      {
        this.LoadStatValue();
      }
      else
      {
        foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) MySession.Static.Players.GetOnlinePlayers())
        {
          if (onlinePlayer != MySession.Static.LocalHumanPlayer && this.IsThePlayerInSight(onlinePlayer))
          {
            this.LoadStatValue();
            return;
          }
        }
        this.m_remoteAchievement.StatValueInt = this.m_startedS;
        if (this.m_startedS < 3600)
          return;
        this.NotifyAchieved();
      }
    }

    private bool IsThePlayerInSight(MyPlayer player)
    {
      if (player.Character == null || MySession.Static.LocalCharacter == null)
        return false;
      Vector3D position1 = player.Character.PositionComp.GetPosition();
      Vector3D position2 = MySession.Static.LocalCharacter.PositionComp.GetPosition();
      if (Vector3D.DistanceSquared(position1, position2) > 4000000.0)
        return false;
      using (MyUtils.ReuseCollection<MyPhysics.HitInfo>(ref this.m_hitInfoResults))
      {
        MyPhysics.CastRay(position1, position2, this.m_hitInfoResults);
        foreach (MyPhysics.HitInfo hitInfoResult in this.m_hitInfoResults)
        {
          if (!(hitInfoResult.HkHitInfo.GetHitEntity() is MyCharacter))
            return false;
        }
        return true;
      }
    }
  }
}
