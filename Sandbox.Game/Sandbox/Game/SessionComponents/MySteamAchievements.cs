// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MySteamAchievements
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Networking;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Plugins;

namespace Sandbox.Game.SessionComponents
{
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 2000)]
  public class MySteamAchievements : MySessionComponentBase
  {
    public static readonly bool OFFLINE_ACHIEVEMENT_INFO = false;
    private static readonly List<MySteamAchievementBase> m_achievements = new List<MySteamAchievementBase>();
    private static bool m_initialized = false;
    private static bool m_achievementsLoaded = false;
    private double m_lastTimestamp;

    private static void Init()
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated || !MyGameService.IsActive)
        return;
      MyGameService.OnUserChanged += (Action<bool>) (_ =>
      {
        int num = MySteamAchievements.m_achievementsLoaded ? 1 : 0;
        if (num != 0)
          MySteamAchievements.UnloadAchievements();
        MySteamAchievements.m_achievements.Clear();
        MySteamAchievements.InitializeAchievements();
        if (num == 0)
          return;
        MySteamAchievements.LoadAchievements();
      });
      MyGameService.LoadStats();
      MySteamAchievements.InitializeAchievements();
      MySteamAchievements.m_initialized = true;
    }

    private static void InitializeAchievements()
    {
      List<MySteamAchievementBase> steamAchievementBaseList = new List<MySteamAchievementBase>();
      foreach (Type type1 in MyPlugins.GameAssembly.GetTypes())
      {
        try
        {
          if (typeof (MySteamAchievementBase).IsAssignableFrom(type1))
            steamAchievementBaseList.Add((MySteamAchievementBase) Activator.CreateInstance(type1));
        }
        catch (Exception ex)
        {
          Type type2 = type1;
          ReportAchievementException(ex, type2);
        }
      }
      foreach (MySteamAchievementBase steamAchievementBase in steamAchievementBaseList)
      {
        try
        {
          steamAchievementBase.Init();
          if (!steamAchievementBase.IsAchieved)
          {
            steamAchievementBase.Achieved += (Action<MySteamAchievementBase>) (x => MyGameService.StoreStats());
            MySteamAchievements.m_achievements.Add(steamAchievementBase);
          }
        }
        catch (Exception ex)
        {
          Type type = steamAchievementBase.GetType();
          ReportAchievementException(ex, type);
        }
      }

      void ReportAchievementException(Exception e, Type type)
      {
        MySandboxGame.Log.WriteLine("Initialization of achievement failed: " + type.Name);
        MySandboxGame.Log.IncreaseIndent();
        MySandboxGame.Log.WriteLine(e);
        MySandboxGame.Log.DecreaseIndent();
      }
    }

    public override void UpdateAfterSimulation()
    {
      if (!MySteamAchievements.m_initialized)
        return;
      foreach (MySteamAchievementBase achievement in MySteamAchievements.m_achievements)
      {
        if (achievement.NeedsUpdate && !achievement.IsAchieved)
          achievement.SessionUpdate();
      }
      if ((double) MySession.Static.ElapsedPlayTime.Minutes <= this.m_lastTimestamp)
        return;
      this.m_lastTimestamp = (double) MySession.Static.ElapsedPlayTime.Minutes;
      MyGameService.StoreStats();
    }

    public override void LoadData()
    {
      if (!MySteamAchievements.m_initialized)
        MySteamAchievements.Init();
      if (!MySteamAchievements.m_initialized)
        return;
      MySteamAchievements.LoadAchievements();
    }

    private static void LoadAchievements()
    {
      MySteamAchievements.m_achievementsLoaded = true;
      foreach (MySteamAchievementBase achievement in MySteamAchievements.m_achievements)
      {
        if (!achievement.IsAchieved)
          achievement.SessionLoad();
      }
    }

    public override void SaveData()
    {
      if (!MySteamAchievements.m_initialized)
        return;
      foreach (MySteamAchievementBase achievement in MySteamAchievements.m_achievements)
      {
        if (!achievement.IsAchieved)
          achievement.SessionSave();
      }
      MyGameService.StoreStats();
    }

    protected override void UnloadData()
    {
      if (!MySteamAchievements.m_initialized)
        return;
      MySteamAchievements.UnloadAchievements();
      MyGameService.StoreStats();
    }

    private static void UnloadAchievements()
    {
      MySteamAchievements.m_achievementsLoaded = false;
      foreach (MySteamAchievementBase achievement in MySteamAchievements.m_achievements)
      {
        if (!achievement.IsAchieved)
          achievement.SessionUnload();
      }
    }

    public override void BeforeStart()
    {
      if (!MySteamAchievements.m_initialized)
        return;
      foreach (MySteamAchievementBase achievement in MySteamAchievements.m_achievements)
      {
        if (!achievement.IsAchieved)
          achievement.SessionBeforeStart();
      }
    }
  }
}
