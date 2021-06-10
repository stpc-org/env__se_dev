// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpTemperature
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;
using System;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_Temperature", 63)]
  internal class MyIngameHelpTemperature : MyIngameHelpObjective
  {
    public MyIngameHelpTemperature()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_Temperature_Title;
      this.RequiredIds = new string[1]
      {
        "IngameHelp_Camera"
      };
      this.Details = new MyIngameHelpDetail[1]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Temperature_Detail1
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
      this.DelayToAppear = (float) TimeSpan.FromMinutes(10.0).TotalSeconds;
    }
  }
}
