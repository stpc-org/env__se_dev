// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpOxygen
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_Oxygen", 130)]
  internal class MyIngameHelpOxygen : MyIngameHelpObjective
  {
    public MyIngameHelpOxygen()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_Oxygen_Title;
      this.RequiredIds = new string[1]{ "IngameHelp_Intro" };
      this.RequiredCondition = this.RequiredCondition + new Func<bool>(this.LowOxygen);
      this.Details = new MyIngameHelpDetail[2]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Oxygen_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Oxygen_Detail2,
          FinishCondition = new Func<bool>(this.OxygenReplenished)
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
      this.FollowingId = "IngameHelp_OxygenTip";
    }

    private bool LowOxygen()
    {
      MyCharacter myCharacter = MySession.Static != null ? MySession.Static.LocalCharacter : (MyCharacter) null;
      return myCharacter != null && myCharacter.OxygenComponent != null && (double) myCharacter.OxygenComponent.GetGasFillLevel(MyCharacterOxygenComponent.OxygenId) > 0.0 && (double) myCharacter.OxygenComponent.GetGasFillLevel(MyCharacterOxygenComponent.OxygenId) < 0.5;
    }

    private bool OxygenReplenished()
    {
      MyCharacter myCharacter = MySession.Static != null ? MySession.Static.LocalCharacter : (MyCharacter) null;
      return myCharacter != null && myCharacter.OxygenComponent != null && (double) myCharacter.OxygenComponent.GetGasFillLevel(MyCharacterOxygenComponent.OxygenId) > 0.990000009536743;
    }
  }
}
