// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpBuilding
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Graphics.GUI;
using System;
using System.Linq;
using VRage.Input;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_Building", 60)]
  internal class MyIngameHelpBuilding : MyIngameHelpObjective
  {
    private MyIngameHelpObjective.IHelplet m_helplet;

    public MyIngameHelpBuilding()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_Building_Title;
      this.RequiredIds = new string[1]{ "IngameHelp_Intro" };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
      this.FollowingId = "IngameHelp_BuildingTip";
      this.DelayToAppear = (float) TimeSpan.FromMinutes(3.0).TotalSeconds;
    }

    public override void OnBeforeActivate()
    {
      base.OnBeforeActivate();
      if (MyInput.Static.IsJoystickLastUsed)
        this.m_helplet = (MyIngameHelpObjective.IHelplet) new MyIngameHelpBuilding.GamepadVersion((MyIngameHelpObjective) this);
      else
        this.m_helplet = (MyIngameHelpObjective.IHelplet) new MyIngameHelpBuilding.MouseAndKeyboardVersion((MyIngameHelpObjective) this);
    }

    public override void CleanUp()
    {
      if (this.m_helplet == null)
        return;
      this.m_helplet.CleanUp();
    }

    public override void OnActivated()
    {
      base.OnActivated();
      this.m_helplet.OnActivated();
    }

    private class MouseAndKeyboardVersion : MyIngameHelpObjective.IHelplet
    {
      private bool m_blockSelected;
      private bool m_gPressed;
      private bool m_toolbarDrop;

      public MouseAndKeyboardVersion(MyIngameHelpObjective help)
      {
        help.RequiredCondition = new Func<bool>(this.BlockInToolbarSelected);
        help.Details = new MyIngameHelpDetail[4]
        {
          new MyIngameHelpDetail()
          {
            TextEnum = MySpaceTexts.IngameHelp_Building_Detail1
          },
          new MyIngameHelpDetail()
          {
            TextEnum = MySpaceTexts.IngameHelp_Building_Detail2,
            FinishCondition = new Func<bool>(this.ToolbarConfigScreenCondition)
          },
          new MyIngameHelpDetail()
          {
            TextEnum = MySpaceTexts.IngameHelp_Building_Detail3,
            FinishCondition = new Func<bool>(this.ToolbarDropCondition)
          },
          new MyIngameHelpDetail()
          {
            TextEnum = MySpaceTexts.IngameHelp_Building_Detail4,
            FinishCondition = new Func<bool>(this.BlockInToolbarSelected)
          }
        };
        if (MyToolbarComponent.CurrentToolbar == null)
          return;
        MyToolbarComponent.CurrentToolbar.SlotActivated += new Action<MyToolbar, MyToolbar.SlotArgs, bool>(this.CurrentToolbar_SlotActivated);
        MyToolbarComponent.CurrentToolbarChanged += new Action<MyToolbar, MyToolbar>(this.MyToolbarComponent_CurrentToolbarChanged);
      }

      public void OnActivated() => MyToolbarComponent.CurrentToolbar.ItemChanged += new Action<MyToolbar, MyToolbar.IndexArgs, bool>(this.CurrentToolbar_ItemChanged);

      private void MyToolbarComponent_CurrentToolbarChanged(MyToolbar old, MyToolbar current)
      {
        if (old != null)
        {
          old.SlotActivated -= new Action<MyToolbar, MyToolbar.SlotArgs, bool>(this.CurrentToolbar_SlotActivated);
          old.ItemChanged -= new Action<MyToolbar, MyToolbar.IndexArgs, bool>(this.CurrentToolbar_ItemChanged);
        }
        MyToolbarComponent.CurrentToolbar.SlotActivated += new Action<MyToolbar, MyToolbar.SlotArgs, bool>(this.CurrentToolbar_SlotActivated);
        MyToolbarComponent.CurrentToolbar.ItemChanged += new Action<MyToolbar, MyToolbar.IndexArgs, bool>(this.CurrentToolbar_ItemChanged);
      }

      public void CleanUp()
      {
        if (MyToolbarComponent.CurrentToolbar != null)
        {
          MyToolbarComponent.CurrentToolbar.SlotActivated -= new Action<MyToolbar, MyToolbar.SlotArgs, bool>(this.CurrentToolbar_SlotActivated);
          MyToolbarComponent.CurrentToolbar.ItemChanged -= new Action<MyToolbar, MyToolbar.IndexArgs, bool>(this.CurrentToolbar_ItemChanged);
        }
        MyToolbarComponent.CurrentToolbarChanged -= new Action<MyToolbar, MyToolbar>(this.MyToolbarComponent_CurrentToolbarChanged);
      }

      private void CurrentToolbar_ItemChanged(
        MyToolbar arg1,
        MyToolbar.IndexArgs arg2,
        bool isGamepad)
      {
        this.m_toolbarDrop = true;
      }

      private void CurrentToolbar_SlotActivated(
        MyToolbar toolbar,
        MyToolbar.SlotArgs args,
        bool userActivated)
      {
        if (!(toolbar.SelectedItem is MyToolbarItemCubeBlock & userActivated))
          return;
        this.m_blockSelected = true;
      }

      private bool BlockInToolbarSelected() => this.m_blockSelected;

      private bool ToolbarConfigScreenCondition()
      {
        this.m_gPressed |= MyScreenManager.Screens.Any<MyGuiScreenBase>((Func<MyGuiScreenBase, bool>) (x => x is MyGuiScreenToolbarConfigBase));
        return this.m_gPressed;
      }

      private bool ToolbarDropCondition() => this.m_toolbarDrop;
    }

    private class GamepadVersion : MyIngameHelpObjective.IHelplet
    {
      private bool m_blockSelected;
      private bool m_radialMenuOpened;
      private bool m_radialMenuTabsSwitched;
      private int m_initialRadialMenuTab = -1;

      public GamepadVersion(MyIngameHelpObjective help)
      {
        help.RequiredCondition = new Func<bool>(this.BlockInToolbarSelected);
        help.Details = new MyIngameHelpDetail[4]
        {
          new MyIngameHelpDetail()
          {
            TextEnum = MySpaceTexts.IngameHelp_Building_Detail1
          },
          new MyIngameHelpDetail()
          {
            TextEnum = MySpaceTexts.IngameHelp_Building_Detail2_Gamepad,
            FinishCondition = new Func<bool>(this.BuildingRadialMenuCondition)
          },
          new MyIngameHelpDetail()
          {
            TextEnum = MySpaceTexts.IngameHelp_Building_Detail3_Gamepad,
            FinishCondition = new Func<bool>(this.RadialMenuTabCondition)
          },
          new MyIngameHelpDetail()
          {
            TextEnum = MySpaceTexts.IngameHelp_Building_Detail4_Gamepad,
            FinishCondition = new Func<bool>(this.BlockInToolbarSelected)
          }
        };
      }

      public void OnActivated()
      {
      }

      public void CleanUp()
      {
      }

      private bool RadialMenuTabCondition()
      {
        MyGuiControlRadialMenuBlock controlRadialMenuBlock = MyScreenManager.Screens.OfType<MyGuiControlRadialMenuBlock>().FirstOrDefault<MyGuiControlRadialMenuBlock>();
        if (controlRadialMenuBlock != null)
          this.m_radialMenuTabsSwitched |= this.m_initialRadialMenuTab != controlRadialMenuBlock.CurrentTabIndex;
        return this.m_radialMenuTabsSwitched;
      }

      private bool BlockInToolbarSelected()
      {
        this.m_blockSelected |= MyCubeBuilder.Static.CurrentBlockDefinition != null;
        return this.m_blockSelected;
      }

      private bool BuildingRadialMenuCondition()
      {
        MyGuiControlRadialMenuBlock controlRadialMenuBlock = MyScreenManager.Screens.OfType<MyGuiControlRadialMenuBlock>().FirstOrDefault<MyGuiControlRadialMenuBlock>();
        if (controlRadialMenuBlock != null)
        {
          this.m_radialMenuOpened = true;
          if (this.m_initialRadialMenuTab != -1)
            this.m_initialRadialMenuTab = controlRadialMenuBlock.CurrentTabIndex;
        }
        return this.m_radialMenuOpened;
      }
    }
  }
}
