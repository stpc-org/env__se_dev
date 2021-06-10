// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpHelmetVisor
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_HelmetVisor", 330)]
  internal class MyIngameHelpHelmetVisor : MyIngameHelpObjective
  {
    private bool m_damageFromLowOxygen;
    private bool m_helmetClosed;

    public MyIngameHelpHelmetVisor()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_HelmetVisor_Title;
      this.RequiredIds = new string[1]{ "IngameHelp_Intro" };
      this.RequiredCondition = this.RequiredCondition + new Func<bool>(this.DamageFromLowOxygen);
      this.Details = new MyIngameHelpDetail[2]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_HelmetVisor_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_HelmetVisor_Detail2,
          FinishCondition = new Func<bool>(this.HelmetClosed)
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
      this.FollowingId = "IngameHelp_HelmetVisorTip";
    }

    public override bool IsCritical() => this.DamageFromLowOxygen();

    private bool DamageFromLowOxygen()
    {
      if (MySession.Static == null || MySession.Static.LocalCharacter == null || (MySession.Static.ControlledEntity != MySession.Static.LocalCharacter || MySession.Static.LocalCharacter.Breath == null))
        return false;
      return MySession.Static.LocalCharacter.Breath.CurrentState == MyCharacterBreath.State.Choking || MySession.Static.LocalCharacter.Breath.CurrentState == MyCharacterBreath.State.NoBreath;
    }

    private bool HelmetClosed()
    {
      MySession mySession = MySession.Static;
      int num;
      if (mySession == null)
      {
        num = 0;
      }
      else
      {
        bool? helmetEnabled = mySession.LocalCharacter?.OxygenComponent.HelmetEnabled;
        bool flag = true;
        num = helmetEnabled.GetValueOrDefault() == flag & helmetEnabled.HasValue ? 1 : 0;
      }
      if (num != 0)
        this.m_helmetClosed = true;
      return this.m_helmetClosed;
    }
  }
}
