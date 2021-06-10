// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpCamera
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_Camera", 20)]
  internal class MyIngameHelpCamera : MyIngameHelpObjective
  {
    private bool m_cameraModeSwitched;
    private bool m_initialCameraMode;
    private bool m_cameraDistanceChanged;
    private double m_initialCameraDistance;

    public MyIngameHelpCamera()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_Camera_Title;
      this.RequiredIds = new string[1]{ "IngameHelp_Intro" };
      this.Details = new MyIngameHelpDetail[3]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Camera_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Camera_Detail2,
          FinishCondition = new Func<bool>(this.CameraModeCondition)
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Camera_Detail3,
          FinishCondition = new Func<bool>(this.AltWheelCondition)
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
      this.RequiredCondition = new Func<bool>(this.ThirdPersonEnabledCondition);
    }

    private bool ThirdPersonEnabledCondition() => MySession.Static.Settings.Enable3rdPersonView;

    public override void OnActivated()
    {
      base.OnActivated();
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      if (localCharacter == null)
        return;
      this.m_initialCameraDistance = MyThirdPersonSpectator.Static.GetViewerDistance();
      this.m_initialCameraMode = localCharacter.IsInFirstPersonView;
    }

    private bool CameraModeCondition()
    {
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      if (localCharacter != null)
        this.m_cameraModeSwitched |= localCharacter.IsInFirstPersonView != this.m_initialCameraMode;
      return this.m_cameraModeSwitched;
    }

    private bool AltWheelCondition()
    {
      this.m_cameraDistanceChanged |= this.m_initialCameraDistance != MyThirdPersonSpectator.Static.GetViewerDistance();
      return this.m_cameraDistanceChanged;
    }
  }
}
