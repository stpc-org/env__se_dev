// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Achievements.MyAchievement_MasterEngineer
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;

namespace SpaceEngineers.Game.Achievements
{
  public class MyAchievement_MasterEngineer : MySteamAchievementBase
  {
    public const int EndValue = 9000;
    private int m_totalMinutesPlayed;
    private int m_lastLoggedMinute;

    public override bool NeedsUpdate => true;

    protected override (string, string, float) GetAchievementInfo() => (nameof (MyAchievement_MasterEngineer), "MasterEngineer_MinutesPlayed", 9000f);

    protected override void LoadStatValue() => this.m_totalMinutesPlayed = this.m_remoteAchievement.StatValueInt;

    public override void SessionLoad() => this.m_lastLoggedMinute = 0;

    public override void SessionUpdate()
    {
      int totalMinutes = (int) MySession.Static.ElapsedPlayTime.TotalMinutes;
      if (this.m_lastLoggedMinute >= totalMinutes)
        return;
      ++this.m_totalMinutesPlayed;
      this.m_lastLoggedMinute = totalMinutes;
      this.m_remoteAchievement.StatValueInt = this.m_totalMinutesPlayed;
      if (this.m_totalMinutesPlayed <= 9000)
        return;
      this.NotifyAchieved();
    }
  }
}
