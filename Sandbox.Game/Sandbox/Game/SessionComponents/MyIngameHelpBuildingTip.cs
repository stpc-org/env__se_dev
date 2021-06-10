// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpBuildingTip
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_BuildingTip", 60)]
  internal class MyIngameHelpBuildingTip : MyIngameHelpObjective
  {
    private bool m_blockSelected;
    private bool m_gPressed;
    private bool m_toolbarDrop;

    public MyIngameHelpBuildingTip()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_Building_Title;
      this.RequiredIds = new string[1]
      {
        "IngameHelp_Building"
      };
      this.Details = new MyIngameHelpDetail[2]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_BuildingTip_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_BuildingTip_Detail2,
          Args = new object[1]
          {
            MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.SECONDARY_TOOL_ACTION)
          }
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 4f;
    }
  }
}
