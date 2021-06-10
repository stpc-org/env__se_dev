// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MySessionComponentIngameHelp
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;

namespace Sandbox.Game.SessionComponents
{
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 2000)]
  public class MySessionComponentIngameHelp : MySessionComponentBase
  {
    public static float DEFAULT_INITIAL_DELAY = 5f;
    public static float DEFAULT_OBJECTIVE_DELAY = 5f;
    public static float TIMEOUT_DELAY = 120f;
    public static readonly HashSet<string> EssentialObjectiveIds = new HashSet<string>();
    private static List<MySessionComponentIngameHelp.ObjectiveDescription> m_objectiveDescriptions = new List<MySessionComponentIngameHelp.ObjectiveDescription>();
    private Dictionary<MyStringHash, MyIngameHelpObjective> m_availableObjectives = new Dictionary<MyStringHash, MyIngameHelpObjective>();
    private MyIngameHelpObjective m_currentObjective;
    private MyIngameHelpObjective m_nextObjective;
    private float m_currentDelayCounter = MySessionComponentIngameHelp.DEFAULT_INITIAL_DELAY;
    private float m_currentTimeoutCounter = MySessionComponentIngameHelp.TIMEOUT_DELAY;
    private float m_timeSinceLastObjective;
    private bool m_hintsEnabled = true;
    private MyCueId m_newObjectiveCue = MySoundPair.GetCueId("HudGPSNotification3");
    private MyCueId m_detailFinishedCue = MySoundPair.GetCueId("HudGPSNotification2");
    private MyCueId m_objectiveFinishedCue = MySoundPair.GetCueId("HudGPSNotification1");

    public static void RegisterFromAssembly(Assembly assembly)
    {
      if (assembly == (Assembly) null)
        return;
      foreach (Type type in assembly.GetTypes())
      {
        foreach (IngameObjectiveAttribute customAttribute in type.GetCustomAttributes(typeof (IngameObjectiveAttribute), false))
          MySessionComponentIngameHelp.m_objectiveDescriptions.Add(new MySessionComponentIngameHelp.ObjectiveDescription(customAttribute.Id, type, customAttribute.Priority));
      }
      MySessionComponentIngameHelp.m_objectiveDescriptions.Sort((Comparison<MySessionComponentIngameHelp.ObjectiveDescription>) ((x, y) => x.Priority.CompareTo(y.Priority)));
    }

    static MySessionComponentIngameHelp()
    {
      MySessionComponentIngameHelp.EssentialObjectiveIds.Add("IngameHelp_Movement");
      MySessionComponentIngameHelp.EssentialObjectiveIds.Add("IngameHelp_Camera");
      MySessionComponentIngameHelp.EssentialObjectiveIds.Add("IngameHelp_Intro");
      MySessionComponentIngameHelp.EssentialObjectiveIds.Add("IngameHelp_Jetpack");
      MySessionComponentIngameHelp.RegisterFromAssembly(Assembly.GetAssembly(typeof (MySessionComponentIngameHelp)));
    }

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      base.Init(sessionComponent);
      if (Sandbox.Engine.Platform.Game.IsDedicated)
      {
        this.m_hintsEnabled = false;
        this.SetUpdateOrder(MyUpdateOrder.NoUpdate);
      }
      else
      {
        this.m_hintsEnabled = MySandboxGame.Config.GoodBotHints;
        this.Init();
      }
    }

    private void Init()
    {
      this.m_currentObjective = (MyIngameHelpObjective) null;
      this.m_currentDelayCounter = MySessionComponentIngameHelp.DEFAULT_INITIAL_DELAY;
      this.m_availableObjectives.Clear();
      foreach (MySessionComponentIngameHelp.ObjectiveDescription objectiveDescription in MySessionComponentIngameHelp.m_objectiveDescriptions)
      {
        if (!MySandboxGame.Config.TutorialsFinished.Contains(objectiveDescription.Id))
        {
          MyIngameHelpObjective instance = (MyIngameHelpObjective) Activator.CreateInstance(objectiveDescription.Type);
          instance.Id = objectiveDescription.Id;
          this.m_availableObjectives.Add(MyStringHash.GetOrCompute(instance.Id), instance);
        }
      }
    }

    protected override void UnloadData()
    {
      foreach (KeyValuePair<MyStringHash, MyIngameHelpObjective> availableObjective in this.m_availableObjectives)
        availableObjective.Value.CleanUp();
      base.UnloadData();
    }

    private void CheckGoodBot()
    {
      if (!MyHud.Questlog.Visible || MySandboxGame.Config.GoodBotHints || MyHud.Questlog.QuestTitle == null)
        return;
      foreach (KeyValuePair<MyStringHash, MyIngameHelpObjective> availableObjective in this.m_availableObjectives)
      {
        if (availableObjective.Value != null)
        {
          MyStringId titleEnum = availableObjective.Value.TitleEnum;
          if (availableObjective.Value.Id != null && availableObjective.Value.Id.StartsWith("IngameHelp"))
          {
            string str = MyTexts.GetString(availableObjective.Value.TitleEnum);
            if (!string.IsNullOrEmpty(str) && str == MyHud.Questlog.QuestTitle)
            {
              MyHud.Questlog.CleanDetails();
              MyHud.Questlog.Visible = false;
              break;
            }
          }
        }
      }
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (this.m_hintsEnabled != MySandboxGame.Config.GoodBotHints)
      {
        this.m_hintsEnabled = MySandboxGame.Config.GoodBotHints;
        if (!this.m_hintsEnabled)
        {
          this.m_currentObjective = (MyIngameHelpObjective) null;
          MyHud.Questlog.CleanDetails();
          MyHud.Questlog.Visible = false;
          return;
        }
      }
      this.CheckGoodBot();
      if (!this.m_hintsEnabled || MySession.Static == null || (!MySession.Static.Ready || !MySession.Static.Settings.EnableGoodBotHints) || MyHud.Questlog.IsUsedByVisualScripting)
        return;
      if (MyGuiScreenGamePlay.ActiveGameplayScreen == null)
      {
        if (this.m_availableObjectives.Count > 0 && (this.m_currentObjective == null || !this.m_currentObjective.IsCritical()))
        {
          MyIngameHelpObjective findObjective = this.TryToFindObjective(true);
          if (findObjective != null)
          {
            if (this.m_currentObjective != null)
              this.CancelObjective();
            this.SetObjective(findObjective);
            return;
          }
        }
        if (this.m_currentObjective == null && (double) this.m_currentDelayCounter > 0.0)
        {
          this.m_currentDelayCounter -= 0.01666667f;
          if ((double) this.m_currentDelayCounter >= 0.0)
            return;
          this.m_currentDelayCounter = 0.0f;
          MyHud.Questlog.Visible = false;
          return;
        }
      }
      if (MyGuiScreenGamePlay.ActiveGameplayScreen == null)
      {
        if (this.m_currentObjective != null && (double) this.m_currentTimeoutCounter > 0.0)
        {
          this.m_currentTimeoutCounter -= 0.01666667f;
          if ((double) this.m_currentTimeoutCounter <= 0.0)
          {
            this.m_currentTimeoutCounter = 0.0f;
            this.m_currentDelayCounter = (float) TimeSpan.FromMinutes(5.0).TotalSeconds;
            this.CancelObjective();
            return;
          }
        }
        if (this.m_currentObjective == null && this.m_availableObjectives.Count > 0)
        {
          MyIngameHelpObjective findObjective = this.TryToFindObjective();
          if (findObjective != null)
            this.SetObjective(findObjective);
        }
      }
      if (this.m_currentObjective != null)
      {
        bool flag1 = true;
        int id = 0;
        foreach (MyIngameHelpDetail detail in this.m_currentObjective.Details)
        {
          if (detail.FinishCondition != null)
          {
            bool flag2 = detail.FinishCondition();
            if (flag2 && !MyHud.Questlog.IsCompleted(id))
            {
              MyGuiAudio.PlaySound(MyGuiSounds.HudObjectiveComplete);
              MyHud.Questlog.SetCompleted(id);
            }
            flag1 &= flag2;
          }
          ++id;
        }
        if (!flag1)
          return;
        this.FinishObjective();
      }
      else
        this.m_timeSinceLastObjective += 0.01666667f;
    }

    public IEnumerable<MyStringHash> AvailableObjectives => MySessionComponentIngameHelp.m_objectiveDescriptions.Select<MySessionComponentIngameHelp.ObjectiveDescription, MyStringHash>((Func<MySessionComponentIngameHelp.ObjectiveDescription, MyStringHash>) (x => MyStringHash.GetOrCompute(x.Id)));

    public void ForceObjective(MyStringHash id)
    {
      if (this.m_currentObjective != null)
        this.CancelObjective();
      MyIngameHelpObjective instance;
      if (!this.m_availableObjectives.TryGetValue(id, out instance))
        instance = (MyIngameHelpObjective) Activator.CreateInstance(MySessionComponentIngameHelp.m_objectiveDescriptions.First<MySessionComponentIngameHelp.ObjectiveDescription>((Func<MySessionComponentIngameHelp.ObjectiveDescription, bool>) (x => x.Id == id.String)).Type);
      this.SetObjective(instance);
    }

    private void SetObjective(MyIngameHelpObjective objective)
    {
      MyAudio.Static.PlaySound(this.m_newObjectiveCue);
      objective.OnBeforeActivate();
      MyHud.Questlog.CleanDetails();
      MyHud.Questlog.Visible = true;
      MyHud.Questlog.QuestTitle = MyTexts.GetString(objective.TitleEnum);
      foreach (MyIngameHelpDetail detail in objective.Details)
      {
        string str = detail.Args == null ? MyTexts.GetString(detail.TextEnum) : string.Format(MyTexts.GetString(detail.TextEnum), detail.Args);
        MyHud.Questlog.AddDetail(str, isObjective: (detail.FinishCondition != null));
      }
      this.m_currentDelayCounter = objective.DelayToHide;
      this.m_currentObjective = objective;
      this.m_currentTimeoutCounter = MySessionComponentIngameHelp.TIMEOUT_DELAY;
      this.m_currentObjective.OnActivated();
    }

    private void FinishObjective()
    {
      MySandboxGame.Config.TutorialsFinished.Add(this.m_currentObjective.Id);
      MySandboxGame.Config.Save();
      this.m_availableObjectives.Remove(MyStringHash.GetOrCompute(this.m_currentObjective.Id));
      MyIngameHelpObjective ingameHelpObjective = (MyIngameHelpObjective) null;
      MyAudio.Static.PlaySound(this.m_objectiveFinishedCue);
      if (!string.IsNullOrEmpty(this.m_currentObjective.FollowingId) && this.m_availableObjectives.TryGetValue(MyStringHash.GetOrCompute(this.m_currentObjective.FollowingId), out ingameHelpObjective))
        this.m_nextObjective = ingameHelpObjective;
      this.m_currentObjective.CleanUp();
      this.m_currentObjective = (MyIngameHelpObjective) null;
      this.m_timeSinceLastObjective = 0.0f;
    }

    private void CancelObjective()
    {
      this.m_currentObjective = (MyIngameHelpObjective) null;
      this.m_timeSinceLastObjective = 0.0f;
      MyHud.Questlog.Visible = false;
    }

    public bool TryCancelObjective()
    {
      this.m_currentDelayCounter = 0.0f;
      if (this.m_currentObjective == null)
        return false;
      this.CancelObjective();
      this.m_currentDelayCounter = 0.0f;
      return true;
    }

    private MyIngameHelpObjective TryToFindObjective(bool onlyCritical = false)
    {
      if (onlyCritical)
      {
        foreach (MyIngameHelpObjective ingameHelpObjective in this.m_availableObjectives.Values)
        {
          bool flag = true;
          if (ingameHelpObjective.RequiredIds != null)
          {
            foreach (string requiredId in ingameHelpObjective.RequiredIds)
            {
              if (this.m_availableObjectives.ContainsKey(MyStringHash.GetOrCompute(requiredId)))
              {
                flag = false;
                break;
              }
            }
          }
          if (flag && ingameHelpObjective.IsCritical())
            return ingameHelpObjective;
        }
        return (MyIngameHelpObjective) null;
      }
      if (this.m_nextObjective != null)
      {
        MyIngameHelpObjective nextObjective = this.m_nextObjective;
        this.m_nextObjective = (MyIngameHelpObjective) null;
        return nextObjective;
      }
      foreach (MyIngameHelpObjective ingameHelpObjective in this.m_availableObjectives.Values)
      {
        bool flag = true;
        if (ingameHelpObjective.RequiredIds != null)
        {
          foreach (string requiredId in ingameHelpObjective.RequiredIds)
          {
            if (this.m_availableObjectives.ContainsKey(MyStringHash.GetOrCompute(requiredId)))
            {
              flag = false;
              break;
            }
          }
        }
        if ((double) ingameHelpObjective.DelayToAppear <= 0.0 || (double) this.m_timeSinceLastObjective < (double) ingameHelpObjective.DelayToAppear)
        {
          if (ingameHelpObjective.RequiredCondition != null)
            flag &= ingameHelpObjective.RequiredCondition();
          else if ((double) ingameHelpObjective.DelayToAppear > 0.0)
            flag = false;
        }
        if (flag)
          return ingameHelpObjective;
      }
      return (MyIngameHelpObjective) null;
    }

    public void Reset()
    {
      MySandboxGame.Config.TutorialsFinished.Clear();
      MySandboxGame.Config.Save();
      this.Init();
    }

    internal static IReadOnlyList<MyIngameHelpObjective> GetFinishedObjectives()
    {
      List<MyIngameHelpObjective> ingameHelpObjectiveList = new List<MyIngameHelpObjective>();
      foreach (string str in MySandboxGame.Config.TutorialsFinished)
      {
        string id = str;
        MySessionComponentIngameHelp.ObjectiveDescription objectiveDescription = MySessionComponentIngameHelp.m_objectiveDescriptions.FirstOrDefault<MySessionComponentIngameHelp.ObjectiveDescription>((Func<MySessionComponentIngameHelp.ObjectiveDescription, bool>) (x => x.Id == id));
        if (!string.IsNullOrEmpty(objectiveDescription.Id))
        {
          MyIngameHelpObjective instance = (MyIngameHelpObjective) Activator.CreateInstance(objectiveDescription.Type);
          instance.Id = objectiveDescription.Id;
          ingameHelpObjectiveList.Add(instance);
        }
      }
      return (IReadOnlyList<MyIngameHelpObjective>) ingameHelpObjectiveList;
    }

    private struct ObjectiveDescription
    {
      public string Id;
      public Type Type;
      public int Priority;

      public ObjectiveDescription(string id, Type type, int priority)
      {
        this.Id = id;
        this.Type = type;
        this.Priority = priority;
      }

      public override string ToString() => this.Id;
    }
  }
}
