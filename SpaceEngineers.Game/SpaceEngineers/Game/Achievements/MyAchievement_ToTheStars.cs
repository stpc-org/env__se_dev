// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Achievements.MyAchievement_ToTheStars
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Game;
using Sandbox.Game.SessionComponents;
using System;

namespace SpaceEngineers.Game.Achievements
{
  internal class MyAchievement_ToTheStars : MySteamAchievementBase
  {
    protected override (string, string, float) GetAchievementInfo() => (nameof (MyAchievement_ToTheStars), (string) null, 0.0f);

    public override bool NeedsUpdate => false;

    public override void SessionBeforeStart()
    {
      if (this.IsAchieved)
        return;
      MyCampaignManager.Static.OnCampaignFinished += new Action(this.Static_OnCampaignFinished);
    }

    private void Static_OnCampaignFinished()
    {
      this.NotifyAchieved();
      MyCampaignManager.Static.OnCampaignFinished -= new Action(this.Static_OnCampaignFinished);
    }
  }
}
