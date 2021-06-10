// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpMagneticBoots
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_MagneticBoots", 160)]
  internal class MyIngameHelpMagneticBoots : MyIngameHelpObjective
  {
    private Queue<float> m_averageGravity = new Queue<float>();

    public MyIngameHelpMagneticBoots()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_MagneticBoots_Title;
      this.RequiredIds = new string[1]
      {
        "IngameHelp_Jetpack2"
      };
      this.RequiredCondition = this.RequiredCondition + new Func<bool>(this.ZeroGravity);
      this.Details = new MyIngameHelpDetail[2]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_MagneticBoots_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_MagneticBoots_Detail2,
          FinishCondition = new Func<bool>(this.BootsLocked)
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
      this.FollowingId = "IngameHelp_MagneticBootsTip";
    }

    private bool ZeroGravity()
    {
      int num1 = 5;
      MyCharacter myCharacter = MySession.Static != null ? MySession.Static.LocalCharacter : (MyCharacter) null;
      if (myCharacter == null || myCharacter.CurrentMovementState != MyCharacterMovementEnum.Flying)
        return false;
      this.m_averageGravity.Enqueue(myCharacter.Gravity.LengthSquared());
      if (this.m_averageGravity.Count < num1)
        return false;
      if (this.m_averageGravity.Count > num1)
      {
        double num2 = (double) this.m_averageGravity.Dequeue();
      }
      return (double) this.m_averageGravity.Average() < 1.0 / 1000.0;
    }

    private bool BootsLocked()
    {
      MyCharacter myCharacter = MySession.Static != null ? MySession.Static.LocalCharacter : (MyCharacter) null;
      return myCharacter != null && myCharacter.IsMagneticBootsActive;
    }
  }
}
