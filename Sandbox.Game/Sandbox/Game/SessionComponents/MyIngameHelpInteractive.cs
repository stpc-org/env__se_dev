// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpInteractive
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using System;
using VRage.Game;
using VRage.Game.Entity.UseObject;
using VRage.Game.ModAPI;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_Interactive", 23)]
  internal class MyIngameHelpInteractive : MyIngameHelpObjective
  {
    private float LOOKING_TIME = 1f;
    private IMyUseObject m_interactiveObject;
    private bool m_fPressed;
    private float m_lookingCounter;
    private bool m_kPressed;
    private bool m_iPressed;

    public MyIngameHelpInteractive()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_Interactive_Title;
      this.RequiredIds = new string[1]{ "IngameHelp_Intro" };
      this.Details = new MyIngameHelpDetail[4]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Interactive_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Interactive_Detail2,
          FinishCondition = new Func<bool>(this.UsePressed)
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Interactive_Detail3,
          FinishCondition = new Func<bool>(this.KPressed)
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Interactive_Detail4,
          FinishCondition = new Func<bool>(this.IPressed)
        }
      };
      this.RequiredCondition = this.RequiredCondition + new Func<bool>(this.LookingOnInteractiveObjectDelayed);
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
      this.FollowingId = "IngameHelp_InteractiveTip";
      MyCharacterDetectorComponent.OnInteractiveObjectChanged += new Action<IMyUseObject>(this.MyCharacterDetectorComponent_OnInteractiveObjectChanged);
    }

    public override void CleanUp()
    {
      MyCharacterDetectorComponent.OnInteractiveObjectChanged -= new Action<IMyUseObject>(this.MyCharacterDetectorComponent_OnInteractiveObjectChanged);
      MyCharacterDetectorComponent.OnInteractiveObjectUsed -= new Action<IMyUseObject>(this.MyCharacterDetectorComponent_OnInteractiveObjectUsed);
    }

    private void MyCharacterDetectorComponent_OnInteractiveObjectChanged(IMyUseObject obj)
    {
      if (obj is MyUseObjectBase)
      {
        if (!(obj.Owner is MyCubeBlock owner) || owner.GetPlayerRelationToOwner() == MyRelationsBetweenPlayerAndBlock.Enemies)
          return;
        this.m_interactiveObject = obj;
      }
      else
        this.m_interactiveObject = (IMyUseObject) null;
    }

    private bool IsFriendly() => this.m_interactiveObject != null && this.m_interactiveObject.Owner is MyCubeBlock owner && owner.GetPlayerRelationToOwner() != MyRelationsBetweenPlayerAndBlock.Enemies;

    public override void OnActivated()
    {
      base.OnActivated();
      MyCharacterDetectorComponent.OnInteractiveObjectUsed += new Action<IMyUseObject>(this.MyCharacterDetectorComponent_OnInteractiveObjectUsed);
    }

    private bool LookingOnInteractiveObject() => this.m_interactiveObject != null && this.IsFriendly();

    private bool LookingOnInteractiveObjectDelayed()
    {
      if (this.LookingOnInteractiveObject())
        this.m_lookingCounter += 0.01666667f;
      else
        this.m_lookingCounter = 0.0f;
      return (double) this.m_lookingCounter > (double) this.LOOKING_TIME;
    }

    private void MyCharacterDetectorComponent_OnInteractiveObjectUsed(IMyUseObject obj)
    {
      if (this.m_interactiveObject != obj)
        return;
      this.m_fPressed = true;
    }

    private bool UsePressed() => this.m_fPressed;

    private bool KPressed()
    {
      if (this.LookingOnInteractiveObject() && this.m_interactiveObject.SupportedActions.HasFlag((Enum) UseActionEnum.OpenTerminal) && (MyGuiScreenTerminal.GetCurrentScreen() != MyTerminalPageEnum.None && MyGuiScreenTerminal.GetCurrentScreen() != MyTerminalPageEnum.Inventory))
        this.m_kPressed = true;
      return this.m_kPressed;
    }

    private bool IPressed()
    {
      if (this.LookingOnInteractiveObject() && this.m_interactiveObject.SupportedActions.HasFlag((Enum) UseActionEnum.OpenTerminal) && MyGuiScreenTerminal.GetCurrentScreen() == MyTerminalPageEnum.Inventory)
        this.m_iPressed = true;
      return this.m_iPressed;
    }
  }
}
