// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyToolbarComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Input;
using VRage.Utils;
using VRageRender.Utils;

namespace Sandbox.Game.Screens.Helpers
{
  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
  public class MyToolbarComponent : MySessionComponentBase
  {
    private static readonly MyStringId[] m_slotControls = new MyStringId[10]
    {
      MyControlsSpace.SLOT1,
      MyControlsSpace.SLOT2,
      MyControlsSpace.SLOT3,
      MyControlsSpace.SLOT4,
      MyControlsSpace.SLOT5,
      MyControlsSpace.SLOT6,
      MyControlsSpace.SLOT7,
      MyControlsSpace.SLOT8,
      MyControlsSpace.SLOT9,
      MyControlsSpace.SLOT0
    };
    private static MyToolbarComponent m_instance;
    private MyToolbar m_currentToolbar;
    private MyToolbar m_universalCharacterToolbar;
    private bool m_toolbarControlIsShown;
    private static StringBuilder m_slotControlTextCache = new StringBuilder();

    public static bool IsToolbarControlShown
    {
      get => MyToolbarComponent.m_instance != null && MyToolbarComponent.m_instance.m_toolbarControlIsShown;
      set
      {
        if (MyToolbarComponent.m_instance == null)
          return;
        MyToolbarComponent.m_instance.m_toolbarControlIsShown = value;
      }
    }

    public static MyToolbar CurrentToolbar
    {
      get => MyToolbarComponent.m_instance == null ? (MyToolbar) null : MyToolbarComponent.m_instance.m_currentToolbar;
      set
      {
        if (MyToolbarComponent.m_instance.m_currentToolbar == value)
          return;
        MyToolbar currentToolbar = MyToolbarComponent.m_instance.m_currentToolbar;
        MyToolbarComponent.m_instance.m_currentToolbar = value;
        if (MyToolbarComponent.CurrentToolbarChanged == null)
          return;
        MyToolbarComponent.CurrentToolbarChanged(currentToolbar, value);
      }
    }

    public static MyToolbar CharacterToolbar => MyToolbarComponent.m_instance == null ? (MyToolbar) null : MyToolbarComponent.m_instance.m_universalCharacterToolbar;

    public static void UpdateCurrentToolbar()
    {
      if (!MyToolbarComponent.AutoUpdate || MySession.Static.ControlledEntity == null || (MySession.Static.ControlledEntity.Toolbar == null || MyToolbarComponent.m_instance.m_currentToolbar == MySession.Static.ControlledEntity.Toolbar))
        return;
      MyToolbar currentToolbar = MyToolbarComponent.m_instance.m_currentToolbar;
      MyToolbarComponent.m_instance.m_currentToolbar = MySession.Static.ControlledEntity.Toolbar;
      MyToolbarComponent.m_instance.m_currentToolbar.ShareToolbarItems();
      if (MyToolbarComponent.CurrentToolbarChanged == null)
        return;
      MyToolbarComponent.CurrentToolbarChanged(currentToolbar, MyToolbarComponent.m_instance.m_currentToolbar);
    }

    public static bool GlobalBuilding
    {
      get
      {
        int num = Sandbox.Engine.Platform.Game.IsDedicated ? 1 : 0;
        return MySession.Static.IsCameraUserControlledSpectator() && false;
      }
    }

    public static bool CreativeModeEnabled => MyFakes.UNLIMITED_CHARACTER_BUILDING || MySession.Static.CreativeMode;

    public static event Action<MyToolbar, MyToolbar> CurrentToolbarChanged;

    public MyToolbarComponent()
    {
      if (Sync.IsDedicated)
        this.UpdateOrder = MyUpdateOrder.NoUpdate;
      this.m_universalCharacterToolbar = new MyToolbar(MyToolbarType.Character);
      this.m_currentToolbar = this.m_universalCharacterToolbar;
      MyToolbarComponent.AutoUpdate = true;
    }

    public override void LoadData()
    {
      MyToolbarComponent.m_instance = this;
      base.LoadData();
    }

    public override void HandleInput()
    {
      try
      {
        MyStringId context = MySession.Static.ControlledEntity != null ? MySession.Static.ControlledEntity.ControlContext : MyStringId.NullOrEmpty;
        MyGuiScreenBase screenWithFocus = MyScreenManager.GetScreenWithFocus();
        if (screenWithFocus != MyGuiScreenGamePlay.Static)
        {
          if (!MyToolbarComponent.IsToolbarControlShown)
            goto label_31;
        }
        if (MyToolbarComponent.CurrentToolbar != null)
        {
          if (!MyGuiScreenGamePlay.DisableInput)
          {
            for (int index = 0; index < MyToolbarComponent.m_slotControls.Length; ++index)
            {
              if (MyControllerHelper.IsControl(context, MyToolbarComponent.m_slotControls[index]))
              {
                if (!MyInput.Static.IsAnyCtrlKeyPressed())
                {
                  if (!(screenWithFocus is MyGuiScreenScriptingTools) && screenWithFocus != MyGuiScreenGamePlay.Static)
                  {
                    switch (screenWithFocus)
                    {
                      case MyGuiScreenCubeBuilder _:
                      case MyGuiScreenToolbarConfigBase _:
                        if (!((MyGuiScreenToolbarConfigBase) screenWithFocus).AllowToolbarKeys())
                          continue;
                        break;
                      default:
                        continue;
                    }
                  }
                  if (MyToolbarComponent.CurrentToolbar != null && MyToolbarComponent.CurrentToolbar.CanPlayerActivateItems)
                    MyToolbarComponent.CurrentToolbar.ActivateItemAtSlot(index);
                }
                else if (index < MyToolbarComponent.CurrentToolbar.PageCount)
                {
                  MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
                  MyToolbarComponent.CurrentToolbar.SwitchToPage(index);
                  ++MySession.Static.ToolbarPageSwitches;
                }
              }
            }
            if (screenWithFocus != MyGuiScreenGamePlay.Static)
            {
              switch (screenWithFocus)
              {
                case MyGuiScreenCubeBuilder _:
                case MyGuiScreenToolbarConfigBase _:
                  if (!((MyGuiScreenToolbarConfigBase) screenWithFocus).AllowToolbarKeys())
                    goto label_31;
                  else
                    break;
                default:
                  goto label_31;
              }
            }
            if (MyToolbarComponent.CurrentToolbar != null)
            {
              if (MyControllerHelper.IsControl(context, MyControlsSpace.TOOLBAR_NEXT_ITEM))
                MyToolbarComponent.CurrentToolbar.SelectNextSlot();
              else if (MyControllerHelper.IsControl(context, MyControlsSpace.TOOLBAR_PREV_ITEM))
                MyToolbarComponent.CurrentToolbar.SelectPreviousSlot();
              if (MySpectator.Static.SpectatorCameraMovement != MySpectatorCameraMovementEnum.ConstantDelta)
              {
                if (MyGuiScreenToolbarConfigBase.Static != null && MyGuiScreenToolbarConfigBase.Static.Visible && (MyInput.Static.IsJoystickLastUsed && MyInput.Static.IsJoystickAxisNewPressed(MyJoystickAxesEnum.Zneg)) || MyControllerHelper.IsControl(context, MyControlsSpace.TOOLBAR_UP))
                {
                  MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
                  MyToolbarComponent.CurrentToolbar.PageUp();
                  ++MySession.Static.ToolbarPageSwitches;
                }
                if (MyGuiScreenToolbarConfigBase.Static == null || !MyGuiScreenToolbarConfigBase.Static.Visible || (!MyInput.Static.IsJoystickLastUsed || !MyInput.Static.IsJoystickAxisNewPressed(MyJoystickAxesEnum.Zpos)))
                {
                  if (!MyControllerHelper.IsControl(context, MyControlsSpace.TOOLBAR_DOWN))
                    goto label_31;
                }
                MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
                MyToolbarComponent.CurrentToolbar.PageDown();
                ++MySession.Static.ToolbarPageSwitches;
              }
            }
          }
        }
      }
      finally
      {
      }
label_31:
      base.HandleInput();
    }

    public override void UpdateBeforeSimulation()
    {
      if (!Sync.IsDedicated)
      {
        try
        {
          using (Stats.Generic.Measure("Toolbar.Update()"))
          {
            MyToolbarComponent.UpdateCurrentToolbar();
            if (MyToolbarComponent.CurrentToolbar != null)
              MyToolbarComponent.CurrentToolbar.Update();
          }
        }
        finally
        {
        }
      }
      base.UpdateBeforeSimulation();
    }

    protected override void UnloadData()
    {
      MyToolbarComponent.m_instance = (MyToolbarComponent) null;
      base.UnloadData();
    }

    private static MyToolbar GetToolbar() => MyToolbarComponent.m_instance.m_currentToolbar;

    public static void InitCharacterToolbar(MyObjectBuilder_Toolbar characterToolbar) => MyToolbarComponent.m_instance.m_universalCharacterToolbar.Init(characterToolbar, (MyEntity) null, true);

    public static void InitToolbar(MyToolbarType type, MyObjectBuilder_Toolbar builder)
    {
      if (builder != null && builder.ToolbarType != type)
        builder.ToolbarType = type;
      MyToolbarComponent.m_instance.m_currentToolbar.Init(builder, (MyEntity) null, true);
    }

    public static MyObjectBuilder_Toolbar GetObjectBuilder(MyToolbarType type)
    {
      MyObjectBuilder_Toolbar objectBuilder = MyToolbarComponent.m_instance.m_currentToolbar.GetObjectBuilder();
      objectBuilder.ToolbarType = type;
      return objectBuilder;
    }

    public static StringBuilder GetSlotControlText(int slotIndex)
    {
      if (!MyToolbarComponent.m_slotControls.IsValidIndex<MyStringId>(slotIndex))
        return (StringBuilder) null;
      MyToolbarComponent.m_slotControlTextCache.Clear();
      MyInput.Static.GetGameControl(MyToolbarComponent.m_slotControls[slotIndex]).AppendBoundKeyJustOne(ref MyToolbarComponent.m_slotControlTextCache);
      return MyToolbarComponent.m_slotControlTextCache;
    }

    private MyToolbarType GetCurrentToolbarType() => MyBlockBuilderBase.SpectatorIsBuilding || MySession.Static.ControlledEntity == null ? MyToolbarType.Spectator : MySession.Static.ControlledEntity.ToolbarType;

    public static bool AutoUpdate { get; set; }
  }
}
