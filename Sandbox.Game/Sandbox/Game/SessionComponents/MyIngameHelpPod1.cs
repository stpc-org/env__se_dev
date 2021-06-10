// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpPod1
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;
using System;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_Pod1", 23)]
  internal class MyIngameHelpPod1 : MyIngameHelpObjective
  {
    public static bool StartingInPod;

    public MyIngameHelpPod1()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_Pod_Title;
      this.RequiredIds = new string[1]
      {
        "IngameHelp_Camera"
      };
      this.Details = new MyIngameHelpDetail[1]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Pod1_Detail1
        }
      };
      this.DelayToHide = 4f * MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
      this.RequiredCondition = new Func<bool>(this.PlayerInPod);
      this.FollowingId = "IngameHelp_Pod2";
    }

    private bool PlayerInPod() => MyIngameHelpPod1.StartingInPod;
  }
}
