// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpFlashlight
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_Flashlight", 584)]
  internal class MyIngameHelpFlashlight : MyIngameHelpObjective
  {
    private bool m_flashlightChanged;
    private bool m_originalFlashlight;

    public MyIngameHelpFlashlight()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_Flashlight_Title;
      this.RequiredIds = new string[1]{ "IngameHelp_Intro" };
      this.Details = new MyIngameHelpDetail[2]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Flashlight_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Flashlight_Detail2,
          FinishCondition = new Func<bool>(this.SwitchedLights)
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
      this.DelayToAppear = (float) TimeSpan.FromMinutes(4.0).TotalSeconds;
      this.FollowingId = "IngameHelp_FlashlightTip";
    }

    public override void OnActivated()
    {
      base.OnActivated();
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      if (controlledEntity == null)
        return;
      this.m_originalFlashlight = controlledEntity.EnabledLights;
    }

    private bool SwitchedLights()
    {
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      if (controlledEntity != null)
        this.m_flashlightChanged = this.m_originalFlashlight != controlledEntity.EnabledLights;
      return this.m_flashlightChanged;
    }
  }
}
