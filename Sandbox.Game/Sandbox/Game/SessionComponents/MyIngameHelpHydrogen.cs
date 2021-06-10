// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpHydrogen
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
  [IngameObjective("IngameHelp_Hydrogen", 150)]
  internal class MyIngameHelpHydrogen : MyIngameHelpObjective
  {
    public MyIngameHelpHydrogen()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_Hydrogen_Title;
      this.RequiredIds = new string[1]{ "IngameHelp_Intro" };
      this.RequiredCondition = this.RequiredCondition + new Func<bool>(this.LowHydrogen);
      this.Details = new MyIngameHelpDetail[2]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Hydrogen_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Hydrogen_Detail2,
          FinishCondition = new Func<bool>(this.HydrogenReplenished)
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
      this.FollowingId = "IngameHelp_HydrogenTip";
    }

    private bool LowHydrogen()
    {
      MyCharacter myCharacter = MySession.Static != null ? MySession.Static.LocalCharacter : (MyCharacter) null;
      return myCharacter != null && myCharacter.OxygenComponent != null && (double) myCharacter.OxygenComponent.GetGasFillLevel(MyCharacterOxygenComponent.HydrogenId) > 0.0 && (double) myCharacter.OxygenComponent.GetGasFillLevel(MyCharacterOxygenComponent.HydrogenId) < 0.5;
    }

    private bool HydrogenReplenished()
    {
      MyCharacter myCharacter = MySession.Static != null ? MySession.Static.LocalCharacter : (MyCharacter) null;
      return myCharacter != null && myCharacter.OxygenComponent != null && (double) myCharacter.OxygenComponent.GetGasFillLevel(MyCharacterOxygenComponent.HydrogenId) > 0.990000009536743;
    }
  }
}
