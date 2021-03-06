// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpPod3
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_Pod3", 25)]
  internal class MyIngameHelpPod3 : MyIngameHelpObjective
  {
    public MyIngameHelpPod3()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_Pod_Title;
      this.RequiredIds = new string[1]{ "IngameHelp_Pod1" };
      this.Details = new MyIngameHelpDetail[1]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Pod3_Detail1
        }
      };
      this.DelayToHide = 4f * MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
    }
  }
}
