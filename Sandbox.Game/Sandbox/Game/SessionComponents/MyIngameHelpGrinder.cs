// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpGrinder
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
  [IngameObjective("IngameHelp_Grinder", 180)]
  internal class MyIngameHelpGrinder : MyIngameHelpObjective
  {
    public MyIngameHelpGrinder()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_Grinder_Title;
      this.RequiredIds = new string[1]{ "IngameHelp_Intro" };
      this.RequiredCondition = this.RequiredCondition + new Func<bool>(this.PlayerHasGrinder);
      this.Details = new MyIngameHelpDetail[2]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Grinder_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Grinder_Detail2,
          Args = new object[1]
          {
            MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.PRIMARY_TOOL_ACTION)
          },
          FinishCondition = new Func<bool>(this.PlayerIsGrinding)
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
      this.FollowingId = "IngameHelp_GrinderTip";
    }

    private bool PlayerHasGrinder()
    {
      MyCharacter myCharacter = MySession.Static != null ? MySession.Static.LocalCharacter : (MyCharacter) null;
      return myCharacter != null && myCharacter.EquippedTool is MyAngleGrinder;
    }

    private bool PlayerIsGrinding()
    {
      MyCharacter myCharacter = MySession.Static != null ? MySession.Static.LocalCharacter : (MyCharacter) null;
      return myCharacter != null && myCharacter.EquippedTool is MyAngleGrinder && !string.IsNullOrEmpty((myCharacter.EquippedTool as MyAngleGrinder).EffectId);
    }
  }
}
