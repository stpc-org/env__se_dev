// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpIntroTip
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_IntroTip", 10)]
  internal class MyIngameHelpIntroTip : MyIngameHelpObjective
  {
    public MyIngameHelpIntroTip()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_Intro_Title;
      this.RequiredIds = new string[1]{ "IngameHelp_Intro" };
      this.Details = new MyIngameHelpDetail[2]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_IntroTip_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_IntroTip_Detail2,
          Args = new object[1]
          {
            MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.CHAT_SCREEN)
          }
        }
      };
      this.FollowingId = "IngameHelp_IntroTip2";
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 6f;
    }
  }
}
