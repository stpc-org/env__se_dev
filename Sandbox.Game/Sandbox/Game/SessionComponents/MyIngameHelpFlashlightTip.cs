// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpFlashlightTip
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_FlashlightTip", 84)]
  internal class MyIngameHelpFlashlightTip : MyIngameHelpObjective
  {
    public MyIngameHelpFlashlightTip()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_Flashlight_Title;
      this.RequiredIds = new string[1]
      {
        "IngameHelp_Flashlight"
      };
      this.Details = new MyIngameHelpDetail[2]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_FlashlightTip_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_FlashlightTip_Detail2
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 4f;
    }
  }
}
