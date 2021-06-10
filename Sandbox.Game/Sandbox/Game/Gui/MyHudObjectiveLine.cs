// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHudObjectiveLine
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Game.ModAPI;

namespace Sandbox.Game.Gui
{
  public class MyHudObjectiveLine : IMyHudObjectiveLine
  {
    private string m_missionTitle = "";
    private int m_currentObjective;
    private List<string> m_objectives = new List<string>();

    public bool Visible { get; private set; }

    public string Title
    {
      get => this.m_missionTitle;
      set => this.m_missionTitle = value;
    }

    public string CurrentObjective => this.m_objectives[this.m_currentObjective];

    public MyHudObjectiveLine() => this.Visible = false;

    public void Show() => this.Visible = true;

    public void Hide() => this.Visible = false;

    public void AdvanceObjective()
    {
      if (this.m_currentObjective >= this.m_objectives.Count - 1)
        return;
      ++this.m_currentObjective;
    }

    public void ResetObjectives() => this.m_currentObjective = 0;

    public List<string> Objectives
    {
      get => this.m_objectives;
      set => this.m_objectives = value;
    }

    public void Clear()
    {
      this.m_missionTitle = "";
      this.m_currentObjective = 0;
      this.m_objectives.Clear();
      this.Visible = false;
    }
  }
}
