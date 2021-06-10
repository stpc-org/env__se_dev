// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Achievements.MyAchievement_Colorblind
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;

namespace SpaceEngineers.Game.Achievements
{
  internal class MyAchievement_Colorblind : MySteamAchievementBase
  {
    private const int NUMBER_OF_COLORS_TO_ACHIEV = 20;
    private bool m_isUpdating = true;

    protected override (string, string, float) GetAchievementInfo() => ("MyAchievment_ColorBlind", (string) null, 0.0f);

    public override bool NeedsUpdate => this.m_isUpdating;

    public override void SessionLoad()
    {
      base.SessionLoad();
      if (this.IsAchieved)
        return;
      this.m_isUpdating = true;
    }

    public override void SessionUpdate()
    {
      base.SessionUpdate();
      if (!this.m_isUpdating || MySession.Static.LocalHumanPlayer == null)
        return;
      MySession.Static.LocalHumanPlayer.Controller.ControlledEntityChanged += new Action<IMyControllableEntity, IMyControllableEntity>(this.Controller_ControlledEntityChanged);
      this.m_isUpdating = false;
    }

    private void Controller_ControlledEntityChanged(
      IMyControllableEntity oldEnt,
      IMyControllableEntity newEnt)
    {
      if (newEnt == null || newEnt.Entity.Closed || (MyCampaignManager.Static.IsCampaignRunning || !(newEnt.Entity is MyCockpit)) || !(newEnt.Entity.Parent is MyCubeGrid parent))
        return;
      long builtBy = (newEnt.Entity as MyCockpit).BuiltBy;
      long? identityId = MySession.Static.LocalHumanPlayer?.Identity.IdentityId;
      long valueOrDefault = identityId.GetValueOrDefault();
      if (!(builtBy == valueOrDefault & identityId.HasValue) || parent.NumberOfGridColors < 20)
        return;
      this.NotifyAchieved();
      MySession.Static.LocalHumanPlayer.Controller.ControlledEntityChanged -= new Action<IMyControllableEntity, IMyControllableEntity>(this.Controller_ControlledEntityChanged);
    }
  }
}
