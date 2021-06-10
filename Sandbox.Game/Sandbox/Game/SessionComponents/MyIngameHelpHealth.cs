// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpHealth
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_Health", 120)]
  internal class MyIngameHelpHealth : MyIngameHelpObjective
  {
    public MyIngameHelpHealth()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_Health_Title;
      this.RequiredIds = new string[1]{ "IngameHelp_Intro" };
      this.RequiredCondition = this.RequiredCondition + new Func<bool>(this.LowHealth);
      this.Details = new MyIngameHelpDetail[2]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Health_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Health_Detail2,
          FinishCondition = new Func<bool>(this.HealthReplenished)
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
      this.FollowingId = "IngameHelp_HealthTip";
    }

    private bool LowHealth()
    {
      MyCharacter myCharacter = MySession.Static != null ? MySession.Static.LocalCharacter : (MyCharacter) null;
      return myCharacter != null && myCharacter.StatComp != null && (double) myCharacter.StatComp.HealthRatio > 0.0 && (double) myCharacter.StatComp.HealthRatio < 0.899999976158142;
    }

    private bool HealthReplenished()
    {
      MyCharacter myCharacter = MySession.Static != null ? MySession.Static.LocalCharacter : (MyCharacter) null;
      return myCharacter != null && myCharacter.StatComp != null && (double) myCharacter.StatComp.HealthRatio > 0.990000009536743;
    }
  }
}
