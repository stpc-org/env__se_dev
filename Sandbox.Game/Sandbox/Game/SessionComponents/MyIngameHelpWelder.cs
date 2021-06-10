// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpWelder
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_Welder", 190)]
  internal class MyIngameHelpWelder : MyIngameHelpObjective
  {
    public MyIngameHelpWelder()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_Welder_Title;
      this.RequiredIds = new string[1]{ "IngameHelp_Intro" };
      this.RequiredCondition = this.RequiredCondition + new Func<bool>(this.PlayerHasWelder);
      this.Details = new MyIngameHelpDetail[2]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Welder_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Welder_Detail2,
          Args = new object[1]
          {
            MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.PRIMARY_TOOL_ACTION)
          },
          FinishCondition = new Func<bool>(this.PlayerIsWelding)
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
      this.FollowingId = "IngameHelp_WelderTip";
    }

    private bool PlayerHasWelder()
    {
      MyCharacter myCharacter = MySession.Static != null ? MySession.Static.LocalCharacter : (MyCharacter) null;
      return myCharacter != null && myCharacter.EquippedTool is MyWelder;
    }

    private bool PlayerIsWelding()
    {
      MyCharacter myCharacter = MySession.Static != null ? MySession.Static.LocalCharacter : (MyCharacter) null;
      return myCharacter != null && myCharacter.EquippedTool is MyWelder && (!string.IsNullOrEmpty((myCharacter.EquippedTool as MyWelder).EffectId) && (myCharacter.EquippedTool as MyWelder).IsShooting) && (myCharacter.EquippedTool as MyWelder).HasHitBlock;
    }
  }
}
