// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpFlyingAShipLGTip
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_FlyingAShipLGTip", 165)]
  internal class MyIngameHelpFlyingAShipLGTip : MyIngameHelpObjective
  {
    public MyIngameHelpFlyingAShipLGTip()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_FlyingAShip_Title;
      this.RequiredIds = new string[1]
      {
        "IngameHelp_FlyingAShipLG"
      };
      this.Details = new MyIngameHelpDetail[2]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_FlyingAShipLGTip_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_FlyingAShipLGTip_Detail2
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 4f;
    }
  }
}
