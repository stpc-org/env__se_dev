// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Achievements.MyAchievement_DeathWish
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage.Game;

namespace SpaceEngineers.Game.Achievements
{
  public class MyAchievement_DeathWish : MySteamAchievementBase
  {
    private const float EndValue = 300f;
    private bool m_conditionsMet;
    private int m_lastElapsedMinutes;
    private int m_totalMinutesPlayedInArmageddonSettings;

    protected override (string, string, float) GetAchievementInfo() => (nameof (MyAchievement_DeathWish), "DeathWish_MinutesPlayed", 300f);

    public override bool NeedsUpdate => true;

    public override void SessionLoad()
    {
      this.m_conditionsMet = MySession.Static.Settings.EnvironmentHostility == MyEnvironmentHostilityEnum.CATACLYSM_UNREAL && !MySession.Static.CreativeMode;
      this.m_lastElapsedMinutes = 0;
    }

    protected override void LoadStatValue() => this.m_totalMinutesPlayedInArmageddonSettings = this.m_remoteAchievement.StatValueInt;

    public override void SessionUpdate()
    {
      if (!this.m_conditionsMet)
        return;
      int totalMinutes = (int) MySession.Static.ElapsedPlayTime.TotalMinutes;
      if (this.m_lastElapsedMinutes >= totalMinutes)
        return;
      this.m_lastElapsedMinutes = totalMinutes;
      ++this.m_totalMinutesPlayedInArmageddonSettings;
      this.m_remoteAchievement.StatValueInt = this.m_totalMinutesPlayedInArmageddonSettings;
      if ((double) this.m_totalMinutesPlayedInArmageddonSettings <= 300.0)
        return;
      this.NotifyAchieved();
    }
  }
}
