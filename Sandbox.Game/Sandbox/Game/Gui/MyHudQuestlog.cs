// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHudQuestlog
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game.ObjectBuilders.Gui;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyHudQuestlog
  {
    private bool m_isVisible;
    private bool m_isUsedByVisualScripting;
    private string m_questTitle;
    private List<MultilineData> m_content = new List<MultilineData>();
    public readonly Vector2 QuestlogSize = new Vector2(0.4f, 0.22f);
    public bool HighlightChanges = true;

    public event Action ValueChanged;

    public string QuestTitle
    {
      get => this.m_questTitle;
      set
      {
        this.m_questTitle = value;
        this.RaiseValueChanged();
      }
    }

    private void RaiseValueChanged()
    {
      if (this.ValueChanged == null)
        return;
      this.ValueChanged();
    }

    public MultilineData[] GetQuestGetails() => this.m_content.ToArray();

    public bool Visible
    {
      get => this.m_isVisible;
      set
      {
        this.m_isVisible = value;
        if (this.m_isVisible)
          return;
        this.IsUsedByVisualScripting = false;
      }
    }

    public bool IsUsedByVisualScripting
    {
      get => this.m_isUsedByVisualScripting;
      set => this.m_isUsedByVisualScripting = value;
    }

    public void CleanDetails()
    {
      this.m_content.Clear();
      this.RaiseValueChanged();
    }

    public void AddDetail(string value, bool useTyping = true, bool isObjective = false)
    {
      MultilineData multilineData = new MultilineData();
      multilineData.Data = value;
      multilineData.IsObjective = isObjective;
      if (!useTyping)
        multilineData.CharactersDisplayed = -1;
      this.m_content.Add(multilineData);
      this.RaiseValueChanged();
    }

    public bool IsCompleted(int id) => id < this.m_content.Count && id >= 0 && this.m_content[id].Completed;

    public bool SetCompleted(int id, bool completed = true)
    {
      if (id >= this.m_content.Count || id < 0 || this.m_content[id].Completed == completed)
        return false;
      this.m_content[id].Completed = completed;
      this.RaiseValueChanged();
      return true;
    }

    public bool SetAllCompleted(bool completed = true)
    {
      foreach (MultilineData multilineData in this.m_content)
        multilineData.Completed = completed;
      this.RaiseValueChanged();
      return true;
    }

    public void ModifyDetail(int id, string value, bool useTyping = true)
    {
      if (id >= this.m_content.Count || id < 0)
        return;
      MultilineData multilineData = this.m_content[id];
      multilineData.Data = value;
      this.m_content[id] = multilineData;
      if (!useTyping)
        this.m_content[id].CharactersDisplayed = -1;
      this.RaiseValueChanged();
    }

    public void Save()
    {
      MyVisualScriptManagerSessionComponent component = MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>();
      if (component == null)
        return;
      component.QuestlogData = this.GetObjectBuilder();
    }

    public MyObjectBuilder_Questlog GetObjectBuilder()
    {
      MyObjectBuilder_Questlog objectBuilderQuestlog = new MyObjectBuilder_Questlog();
      objectBuilderQuestlog.Title = this.QuestTitle;
      objectBuilderQuestlog.Visible = this.Visible;
      objectBuilderQuestlog.IsUsedByVisualScripting = this.IsUsedByVisualScripting;
      objectBuilderQuestlog.LineData.Capacity = this.m_content.Count;
      objectBuilderQuestlog.LineData.AddRange((IEnumerable<MultilineData>) this.m_content);
      return objectBuilderQuestlog;
    }

    public void Init()
    {
      MyVisualScriptManagerSessionComponent component = MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>();
      if (component == null)
        return;
      MyObjectBuilder_Questlog questlogData = component.QuestlogData;
      if (questlogData == null)
        return;
      this.m_content.Clear();
      this.m_content.AddRange((IEnumerable<MultilineData>) questlogData.LineData);
      this.QuestTitle = questlogData.Title;
      this.Visible = questlogData.LineData.Count > 0 && questlogData.Visible;
      this.IsUsedByVisualScripting = questlogData.IsUsedByVisualScripting;
    }
  }
}
