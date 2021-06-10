// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpIntro
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using System.Linq;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_Intro", 10)]
  internal class MyIngameHelpIntro : MyIngameHelpObjective
  {
    private bool m_F1pressed;

    public MyIngameHelpIntro()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_Intro_Title;
      this.Details = new MyIngameHelpDetail[2]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Intro_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Intro_Detail2,
          Args = new object[1]
          {
            MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.HELP_SCREEN)
          },
          FinishCondition = new Func<bool>(this.F1Condition)
        }
      };
      this.FollowingId = "IngameHelp_IntroTip";
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
    }

    private bool F1Condition()
    {
      if (MyScreenManager.Screens.Any<MyGuiScreenBase>((Func<MyGuiScreenBase, bool>) (x => x is MyGuiScreenHelpSpace)))
        this.m_F1pressed = true;
      return this.m_F1pressed;
    }
  }
}
