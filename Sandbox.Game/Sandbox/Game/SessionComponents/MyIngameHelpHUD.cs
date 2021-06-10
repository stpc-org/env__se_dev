// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpHUD
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using Sandbox.Game.GUI.HudViewers;
using Sandbox.Game.Localization;
using System;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_HUD", 50)]
  internal class MyIngameHelpHUD : MyIngameHelpObjective
  {
    private bool m_hudStateChanged;
    private bool m_signalsChangedPressed;
    private int m_initialHudState;
    private MyHudMarkerRender.SignalMode m_initialSignalMode;

    public MyIngameHelpHUD()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_HUD_Title;
      this.RequiredIds = new string[1]{ "IngameHelp_Intro" };
      this.Details = new MyIngameHelpDetail[3]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_HUD_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_HUD_Detail2,
          FinishCondition = new Func<bool>(this.TabCondition)
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_HUD_Detail3,
          FinishCondition = new Func<bool>(this.SignalCondition)
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
      this.FollowingId = "IngameHelp_HUDTip";
      this.DelayToAppear = (float) TimeSpan.FromMinutes(3.0).TotalSeconds;
    }

    public override void OnActivated()
    {
      base.OnActivated();
      this.m_initialHudState = MyHud.HudState;
      this.m_initialSignalMode = MyHudMarkerRender.SignalDisplayMode;
    }

    private bool TabCondition()
    {
      this.m_hudStateChanged |= MyHud.HudState != this.m_initialHudState;
      return this.m_hudStateChanged;
    }

    private bool SignalCondition()
    {
      this.m_signalsChangedPressed |= MyHudMarkerRender.SignalDisplayMode != this.m_initialSignalMode;
      return this.m_signalsChangedPressed;
    }
  }
}
