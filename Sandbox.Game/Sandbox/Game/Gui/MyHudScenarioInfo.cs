// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHudScenarioInfo
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;
using System;
using System.Text;
using VRage;

namespace Sandbox.Game.Gui
{
  public class MyHudScenarioInfo
  {
    private int m_livesLeft = -1;
    private int m_timeLeftMin = -1;
    private int m_timeLeftSec = -1;
    private bool m_needsRefresh = true;
    private MyHudNameValueData m_data;
    private bool m_visible;

    public int LivesLeft
    {
      get => this.m_livesLeft;
      set
      {
        if (this.m_livesLeft == value)
          return;
        this.m_livesLeft = value;
        this.m_needsRefresh = true;
        this.Visible = true;
      }
    }

    public int TimeLeftMin
    {
      get => this.m_timeLeftMin;
      set
      {
        if (this.m_timeLeftMin == value)
          return;
        this.m_timeLeftMin = value;
        this.m_needsRefresh = true;
        this.Visible = true;
      }
    }

    public int TimeLeftSec
    {
      get => this.m_timeLeftSec;
      set
      {
        if (this.m_timeLeftSec == value)
          return;
        this.m_timeLeftSec = value;
        this.m_needsRefresh = true;
        this.Visible = true;
      }
    }

    public MyHudNameValueData Data
    {
      get
      {
        if (this.m_needsRefresh)
          this.Refresh();
        return this.m_data;
      }
    }

    public MyHudScenarioInfo()
    {
      this.m_data = new MyHudNameValueData(typeof (MyHudScenarioInfo.LineEnum).GetEnumValues().Length);
      this.Reload();
    }

    public void Reload()
    {
      MyHudNameValueData data = this.m_data;
      data[1].Name.Clear().AppendStringBuilder(MyTexts.Get(MySpaceTexts.HudScenarioInfoLivesLeft));
      data[0].Name.Clear().AppendStringBuilder(MyTexts.Get(MySpaceTexts.HudScenarioInfoTimeLeft));
      this.m_livesLeft = -1;
      this.m_timeLeftMin = -1;
      this.m_timeLeftSec = -1;
      this.m_needsRefresh = true;
    }

    public void Refresh()
    {
      this.m_needsRefresh = false;
      if (this.LivesLeft >= 0)
      {
        this.Data[1].Value.Clear().AppendInt32(this.LivesLeft);
        this.Data[1].Visible = true;
      }
      else
        this.Data[1].Visible = false;
      if (this.TimeLeftMin > 0 || this.TimeLeftSec >= 0)
      {
        this.Data[0].Value.Clear().AppendInt32(this.TimeLeftMin).Append(":").AppendFormat("{0:D2}", (object) this.TimeLeftSec);
        this.Data[0].Visible = true;
      }
      else
        this.Data[0].Visible = false;
      if (this.Data.GetVisibleCount() == 0)
        this.Visible = false;
      else
        this.Visible = true;
    }

    public bool Visible
    {
      get => this.m_visible;
      set => this.m_visible = value;
    }

    public void Show(Action<MyHudScenarioInfo> propertiesInit)
    {
      this.Refresh();
      if (propertiesInit == null)
        return;
      propertiesInit(this);
    }

    public void Hide() => this.Visible = false;

    private enum LineEnum
    {
      TimeLeft,
      LivesLeft,
    }
  }
}
