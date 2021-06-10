// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MySpaceControlMenuInitializer
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using SpaceEngineers.Game.Entities.Cube;
using SpaceEngineers.Game.Entities.UseObjects;
using System;

namespace Sandbox.Game.Screens.Helpers
{
  public class MySpaceControlMenuInitializer : IMyControlMenuInitializer
  {
    private MyGuiScreenControlMenu m_controlMenu;
    private MyControllableEntityControlHelper m_lightsControlHelper;
    private MyControllableEntityControlHelper m_helmetControlHelper;
    private MyControllableEntityControlHelper m_dampingControlHelper;
    private MyControllableEntityControlHelper m_broadcastingControlHelper;
    private MyControllableEntityControlHelper m_reactorsControlHelper;
    private MyControllableEntityControlHelper m_jetpackControlHelper;
    private MyControllableEntityControlHelper m_buildModeControlHelper;
    private MyLandingGearControlHelper m_landingGearsControlHelper;
    private MyQuickLoadControlHelper m_quickLoadControlHelper;
    private MyHudToggleControlHelper m_hudToggleControlHelper;
    private MyCameraModeControlHelper m_cameraModeControlHelper;
    private MyShowTerminalControlHelper m_showTerminalControlHelper;
    private MyShowBuildScreenControlHelper m_showBuildScreenControlHelper;
    private MyColorPickerControlHelper m_colorPickerControlHelper;
    private MySuicideControlHelper m_suicideControlHelper;
    private MyUseTerminalControlHelper m_terminalControlHelper;
    private MyEnableStationRotationControlHelper m_enableStationRotationControlHelper;
    private MyConnectorControlHelper m_connectorControlHelper;
    private MyBlueprintMenuControlHelper m_blueprintControlHelper;
    private MyInventoryMenuControlHelper m_inventoryControlHelper;
    private MyPlayersMenuControlHelper m_playersControlHelper;
    private MyHelpMenuControlHelper m_helpControlHelper;
    private MySpawnMenuControlHelper m_spawnControlHelper;
    private MyAdminMenuControlHelper m_adminControlHelper;

    private bool IsControlMenuInitialized => this.m_controlMenu != null;

    public MySpaceControlMenuInitializer()
    {
      this.m_lightsControlHelper = new MyControllableEntityControlHelper(MyControlsSpace.HEADLIGHTS, (Action<IMyControllableEntity>) (x => x.SwitchLights()), (Func<IMyControllableEntity, bool>) (x => x.EnabledLights), MySpaceTexts.ControlMenuItemLabel_Lights);
      this.m_helmetControlHelper = new MyControllableEntityControlHelper(MyControlsSpace.HELMET, (Action<IMyControllableEntity>) (x => x.SwitchHelmet()), (Func<IMyControllableEntity, bool>) (x => x.EnabledHelmet), MySpaceTexts.ControlMenuItemLabel_Helmet);
      this.m_dampingControlHelper = new MyControllableEntityControlHelper(MyControlsSpace.DAMPING, (Action<IMyControllableEntity>) (x => x.SwitchDamping()), (Func<IMyControllableEntity, bool>) (x => x.EnabledDamping), MySpaceTexts.ControlMenuItemLabel_Dampeners);
      this.m_broadcastingControlHelper = new MyControllableEntityControlHelper(MyControlsSpace.BROADCASTING, (Action<IMyControllableEntity>) (x => x.SwitchBroadcasting()), (Func<IMyControllableEntity, bool>) (x => x.EnabledBroadcasting), MySpaceTexts.ControlMenuItemLabel_Broadcasting);
      this.m_landingGearsControlHelper = new MyLandingGearControlHelper();
      this.m_connectorControlHelper = new MyConnectorControlHelper();
      this.m_reactorsControlHelper = new MyControllableEntityControlHelper(MyControlsSpace.TOGGLE_REACTORS, (Action<IMyControllableEntity>) (x => x.SwitchReactors()), (Func<IMyControllableEntity, bool>) (x => x.EnabledReactors), MySpaceTexts.ControlMenuItemLabel_Reactors);
      this.m_jetpackControlHelper = new MyControllableEntityControlHelper(MyControlsSpace.THRUSTS, (Action<IMyControllableEntity>) (x => x.SwitchThrusts()), (Func<IMyControllableEntity, bool>) (x => x.EnabledThrusts), MySpaceTexts.ControlMenuItemLabel_Jetpack);
      this.m_buildModeControlHelper = new MyControllableEntityControlHelper(MyControlsSpace.BUILD_MODE, (Action<IMyControllableEntity>) (x => MyCubeBuilder.Static.IsBuildMode = !MyCubeBuilder.Static.IsBuildMode), (Func<IMyControllableEntity, bool>) (x => MyCubeBuilder.Static.IsBuildMode), MySpaceTexts.ControlMenuItemLabel_BuildMode);
      this.m_quickLoadControlHelper = new MyQuickLoadControlHelper();
      this.m_hudToggleControlHelper = new MyHudToggleControlHelper();
      this.m_cameraModeControlHelper = new MyCameraModeControlHelper();
      this.m_showTerminalControlHelper = new MyShowTerminalControlHelper();
      this.m_showBuildScreenControlHelper = new MyShowBuildScreenControlHelper();
      this.m_colorPickerControlHelper = new MyColorPickerControlHelper();
      this.m_suicideControlHelper = new MySuicideControlHelper();
      this.m_terminalControlHelper = new MyUseTerminalControlHelper();
      this.m_blueprintControlHelper = new MyBlueprintMenuControlHelper();
      this.m_inventoryControlHelper = new MyInventoryMenuControlHelper();
      this.m_playersControlHelper = new MyPlayersMenuControlHelper();
      this.m_helpControlHelper = new MyHelpMenuControlHelper();
      this.m_spawnControlHelper = new MySpawnMenuControlHelper();
      this.m_adminControlHelper = new MyAdminMenuControlHelper();
      this.m_enableStationRotationControlHelper = new MyEnableStationRotationControlHelper();
    }

    public void OpenControlMenu(IMyControllableEntity controlledEntity)
    {
      this.m_controlMenu = (MyGuiScreenControlMenu) null;
      if (controlledEntity is MyCharacter)
        this.SetupCharacterScreen(controlledEntity as MyCharacter);
      else if (controlledEntity is MyShipController)
        this.SetupSpaceshipScreen(controlledEntity as MyShipController);
      if (!this.IsControlMenuInitialized)
        return;
      this.m_controlMenu.RecreateControls(false);
      MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) this.m_controlMenu);
    }

    private void SetupCharacterScreen(MyCharacter character)
    {
      this.m_lightsControlHelper.SetEntity((IMyControllableEntity) character);
      this.m_dampingControlHelper.SetEntity((IMyControllableEntity) character);
      this.m_broadcastingControlHelper.SetEntity((IMyControllableEntity) character);
      this.m_helmetControlHelper.SetEntity((IMyControllableEntity) character);
      this.m_jetpackControlHelper.SetEntity((IMyControllableEntity) character);
      this.m_showBuildScreenControlHelper.SetEntity((IMyControllableEntity) character);
      this.m_showTerminalControlHelper.SetEntity((IMyControllableEntity) character);
      this.m_suicideControlHelper.SetCharacter(character);
      this.m_terminalControlHelper.SetCharacter(character);
      this.m_buildModeControlHelper.SetEntity((IMyControllableEntity) character);
      this.m_inventoryControlHelper.SetEntity((IMyControllableEntity) character);
      this.m_controlMenu = new MyGuiScreenControlMenu();
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_showTerminalControlHelper);
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_showBuildScreenControlHelper);
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_inventoryControlHelper);
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_buildModeControlHelper);
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_hudToggleControlHelper);
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_jetpackControlHelper);
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_lightsControlHelper);
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_dampingControlHelper);
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_helmetControlHelper);
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_broadcastingControlHelper);
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_cameraModeControlHelper);
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_quickLoadControlHelper);
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_colorPickerControlHelper);
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_helpControlHelper);
      if (MyMultiplayer.Static != null)
        this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_playersControlHelper);
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_blueprintControlHelper);
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_spawnControlHelper);
      if (MySession.Static.IsUserModerator(Sync.MyId))
        this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_adminControlHelper);
      this.AddUseObjectControl(character);
      if (!MySession.Static.SurvivalMode)
        return;
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_suicideControlHelper);
    }

    private void SetupSpaceshipScreen(MyShipController ship)
    {
      this.m_lightsControlHelper.SetEntity((IMyControllableEntity) ship);
      this.m_dampingControlHelper.SetEntity((IMyControllableEntity) ship);
      this.m_landingGearsControlHelper.SetEntity((IMyControllableEntity) ship);
      this.m_connectorControlHelper.SetEntity((IMyControllableEntity) ship);
      this.m_reactorsControlHelper.SetEntity((IMyControllableEntity) ship);
      this.m_showBuildScreenControlHelper.SetEntity((IMyControllableEntity) ship);
      this.m_showTerminalControlHelper.SetEntity((IMyControllableEntity) ship);
      this.m_buildModeControlHelper.SetEntity((IMyControllableEntity) ship);
      this.m_inventoryControlHelper.SetEntity((IMyControllableEntity) ship);
      this.m_controlMenu = new MyGuiScreenControlMenu();
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_showTerminalControlHelper);
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_showBuildScreenControlHelper);
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_inventoryControlHelper);
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_quickLoadControlHelper);
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_hudToggleControlHelper);
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_lightsControlHelper);
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_dampingControlHelper);
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_landingGearsControlHelper);
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_connectorControlHelper);
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_reactorsControlHelper);
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_cameraModeControlHelper);
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_helpControlHelper);
      if (MyMultiplayer.Static != null)
        this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_playersControlHelper);
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_blueprintControlHelper);
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_spawnControlHelper);
      if (!MySession.Static.IsUserModerator(Sync.MyId))
        return;
      this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_adminControlHelper);
    }

    private void AddUseObjectControl(MyCharacter character)
    {
      MyCharacterDetectorComponent detectorComponent = character.Components.Get<MyCharacterDetectorComponent>();
      if (detectorComponent == null)
        return;
      if (detectorComponent.UseObject is MyUseObjectDoorTerminal || detectorComponent.UseObject is MyUseObjectTerminal || detectorComponent.UseObject is MyUseObjectTextPanel)
      {
        this.m_terminalControlHelper.SetLabel(MySpaceTexts.ControlMenuItemLabel_ShowControlPanel);
        this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_terminalControlHelper);
      }
      else if (detectorComponent.UseObject is MyUseObjectInventory)
      {
        this.m_terminalControlHelper.SetLabel(MySpaceTexts.ControlMenuItemLabel_OpenInventory);
        this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_terminalControlHelper);
      }
      else
      {
        if (!(detectorComponent.UseObject is MyUseObjectPanelButton))
          return;
        this.m_terminalControlHelper.SetLabel(MySpaceTexts.ControlMenuItemLabel_SetupButtons);
        this.m_controlMenu.AddItem((MyAbstractControlMenuItem) this.m_terminalControlHelper);
      }
    }
  }
}
