// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.GUI.MyBadgeHelper
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Engine.Networking;
using Sandbox.Game;
using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using VRageMath;

namespace SpaceEngineers.Game.GUI
{
  public class MyBadgeHelper
  {
    private List<MyBadgeHelper.MyBadge> m_badges;

    private void InitializeBadges()
    {
      this.m_badges = new List<MyBadgeHelper.MyBadge>()
      {
        new MyBadgeHelper.MyBadge()
        {
          Status = MyBadgeHelper.MyBannerStatus.Offline,
          Texture = "Textures\\GUI\\PromotedEngineer.dds",
          DLCId = 0U,
          AchievementName = "Promoted_engineer"
        }
      };
      foreach (KeyValuePair<uint, MyDLCs.MyDLC> dlC in MyDLCs.DLCs)
        this.m_badges.Add(new MyBadgeHelper.MyBadge()
        {
          Status = MyBadgeHelper.MyBannerStatus.Offline,
          Texture = dlC.Value.Badge,
          DLCId = dlC.Value.AppId,
          AchievementName = ""
        });
    }

    public void DrawGameLogo(float transitionAlpha, Vector2 position)
    {
      if (this.m_badges == null)
        this.InitializeBadges();
      MyGuiSandbox.DrawGameLogo(transitionAlpha, position);
      position.X += 0.005f;
      position.Y += 0.19f;
      Vector2 vector2 = position;
      Vector2 size = new Vector2(114f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      int num = 0;
      foreach (MyBadgeHelper.MyBadge badge in this.m_badges)
      {
        if (badge.Status == MyBadgeHelper.MyBannerStatus.Installed)
        {
          MyGuiSandbox.DrawBadge(badge.Texture, transitionAlpha, position, size);
          position.X += size.X;
          ++num;
          if (num >= 6)
          {
            vector2.Y += size.Y;
            position = vector2;
            num = 0;
          }
        }
      }
    }

    public void RefreshGameLogo()
    {
      if (this.m_badges == null)
        this.InitializeBadges();
      foreach (MyBadgeHelper.MyBadge badge in this.m_badges)
        badge.Status = !MyGameService.IsActive ? MyBadgeHelper.MyBannerStatus.Offline : (badge.DLCId == 0U || !MyGameService.IsDlcInstalled(badge.DLCId) ? (string.IsNullOrEmpty(badge.AchievementName) || !MyGameService.GetAchievement(badge.AchievementName, (string) null, 0.0f).IsUnlocked ? MyBadgeHelper.MyBannerStatus.NotInstalled : MyBadgeHelper.MyBannerStatus.Installed) : MyBadgeHelper.MyBannerStatus.Installed);
    }

    private enum MyBannerStatus
    {
      Offline,
      Installed,
      NotInstalled,
    }

    private class MyBadge
    {
      public MyBadgeHelper.MyBannerStatus Status;
      public uint DLCId;
      public string AchievementName;
      public string Texture;
    }
  }
}
