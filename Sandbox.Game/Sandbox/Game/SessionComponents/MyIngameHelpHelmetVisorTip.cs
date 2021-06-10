// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpHelmetVisorTip
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_HelmetVisorTip", 330)]
  internal class MyIngameHelpHelmetVisorTip : MyIngameHelpObjective
  {
    public MyIngameHelpHelmetVisorTip()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_HelmetVisor_Title;
      this.RequiredIds = new string[1]
      {
        "IngameHelp_HelmetVisor"
      };
      this.Details = new MyIngameHelpDetail[2]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_HelmetVisorTip_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_HelmetVisorTip_Detail2,
          Args = new object[1]
          {
            MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.HELP_SCREEN)
          }
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 4f;
    }
  }
}
