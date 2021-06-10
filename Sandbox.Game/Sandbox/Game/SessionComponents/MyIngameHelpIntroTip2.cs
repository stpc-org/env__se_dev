// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpIntroTip2
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_IntroTip2", 10)]
  internal class MyIngameHelpIntroTip2 : MyIngameHelpObjective
  {
    public MyIngameHelpIntroTip2()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_Intro_Title;
      this.RequiredIds = new string[1]
      {
        "IngameHelp_IntroTip"
      };
      this.Details = new MyIngameHelpDetail[1]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_IntroTip2_Detail1
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 4f;
    }
  }
}
