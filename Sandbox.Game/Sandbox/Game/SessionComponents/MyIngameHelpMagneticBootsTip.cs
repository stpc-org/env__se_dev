﻿// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpMagneticBootsTip
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_MagneticBootsTip", 160)]
  internal class MyIngameHelpMagneticBootsTip : MyIngameHelpObjective
  {
    public MyIngameHelpMagneticBootsTip()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_MagneticBoots_Title;
      this.RequiredIds = new string[1]
      {
        "IngameHelp_MagneticBoots"
      };
      this.Details = new MyIngameHelpDetail[2]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_MagneticBootsTip_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_MagneticBootsTip_Detail2
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 4f;
    }
  }
}
