// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpInventory
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using System.Linq;
using VRage.Game.Entity.UseObject;
using VRage.Game.ModAPI;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_Inventory", 110)]
  internal class MyIngameHelpInventory : MyIngameHelpObjective
  {
    private IMyUseObject m_interactiveObject;
    private bool m_fPressed;
    private bool m_iPressed;

    public MyIngameHelpInventory()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_Inventory_Title;
      this.RequiredIds = new string[2]
      {
        "IngameHelp_Movement",
        "IngameHelp_Jetpack2"
      };
      this.RequiredCondition = this.RequiredCondition + new Func<bool>(this.LookingOnInteractiveObject);
      this.Details = new MyIngameHelpDetail[3]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Inventory_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Inventory_Detail2,
          FinishCondition = new Func<bool>(this.UsePressed)
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Inventory_Detail3,
          FinishCondition = new Func<bool>(this.IPressed)
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
      this.FollowingId = "IngameHelp_InventoryTip";
      MyCharacterDetectorComponent.OnInteractiveObjectChanged += new Action<IMyUseObject>(this.MyCharacterDetectorComponent_OnInteractiveObjectChanged);
    }

    public override void OnActivated()
    {
      base.OnActivated();
      MyCharacterDetectorComponent.OnInteractiveObjectUsed += new Action<IMyUseObject>(this.MyCharacterDetectorComponent_OnInteractiveObjectUsed);
    }

    public override void CleanUp()
    {
      MyCharacterDetectorComponent.OnInteractiveObjectChanged -= new Action<IMyUseObject>(this.MyCharacterDetectorComponent_OnInteractiveObjectChanged);
      MyCharacterDetectorComponent.OnInteractiveObjectUsed -= new Action<IMyUseObject>(this.MyCharacterDetectorComponent_OnInteractiveObjectUsed);
    }

    private void MyCharacterDetectorComponent_OnInteractiveObjectChanged(IMyUseObject obj)
    {
      if (obj is MyFloatingObject)
        this.m_interactiveObject = obj;
      else
        this.m_interactiveObject = (IMyUseObject) null;
    }

    private void MyCharacterDetectorComponent_OnInteractiveObjectUsed(IMyUseObject obj)
    {
      if (this.m_interactiveObject != obj)
        return;
      this.m_fPressed = true;
    }

    private bool LookingOnInteractiveObject() => this.m_interactiveObject != null;

    private bool UsePressed() => this.m_fPressed;

    private bool IPressed()
    {
      if (MyScreenManager.Screens.Any<MyGuiScreenBase>((Func<MyGuiScreenBase, bool>) (x => x is MyGuiScreenTerminal)) && MyGuiScreenTerminal.GetCurrentScreen() == MyTerminalPageEnum.Inventory)
        this.m_iPressed = true;
      return this.m_iPressed;
    }
  }
}
