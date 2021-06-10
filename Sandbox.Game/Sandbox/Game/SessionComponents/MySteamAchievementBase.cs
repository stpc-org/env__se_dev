// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MySteamAchievementBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Networking;
using Sandbox.ModAPI;
using System;
using VRage.GameServices;

namespace Sandbox.Game.SessionComponents
{
  public abstract class MySteamAchievementBase
  {
    protected IMyAchievement m_remoteAchievement;

    public event Action<MySteamAchievementBase> Achieved;

    public bool IsAchieved { get; protected set; }

    public abstract bool NeedsUpdate { get; }

    public virtual void SessionLoad()
    {
    }

    public virtual void SessionUpdate()
    {
    }

    public virtual void SessionSave()
    {
    }

    public virtual void SessionUnload()
    {
    }

    public virtual void SessionBeforeStart()
    {
    }

    protected MySteamAchievementBase()
    {
      (string str1, string str2, float statMaxValue) = this.GetAchievementInfo();
      this.m_remoteAchievement = MyGameService.GetAchievement(str1, str2, statMaxValue);
      if (string.IsNullOrEmpty(str2))
        return;
      this.m_remoteAchievement.OnStatValueChanged += new Action(this.LoadStatValue);
    }

    protected void NotifyAchieved()
    {
      this.m_remoteAchievement.Unlock();
      if (MySteamAchievements.OFFLINE_ACHIEVEMENT_INFO)
      {
        string achievementId = this.GetAchievementInfo().achievementId;
        MyAPIGateway.Utilities.ShowNotification("Achievement Unlocked: " + achievementId, 10000, "Red");
      }
      this.IsAchieved = true;
      this.Achieved.InvokeIfNotNull<MySteamAchievementBase>(this);
      this.Achieved = (Action<MySteamAchievementBase>) null;
    }

    public virtual void Init()
    {
      this.IsAchieved = this.m_remoteAchievement.IsUnlocked;
      if (this.IsAchieved)
        return;
      this.LoadStatValue();
    }

    protected abstract (string achievementId, string statTag, float statMaxValue) GetAchievementInfo();

    protected virtual void LoadStatValue()
    {
    }
  }
}
