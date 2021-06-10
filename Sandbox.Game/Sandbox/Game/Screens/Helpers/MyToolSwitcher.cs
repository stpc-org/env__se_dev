// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyToolSwitcher
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Input;
using VRage.Library.Utils;
using VRage.Utils;

namespace Sandbox.Game.Screens.Helpers
{
  [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
  public class MyToolSwitcher : MySessionComponentBase
  {
    private static readonly MyTimeSpan CONFIG_PRESS_LENGTH = MyTimeSpan.FromMilliseconds(660.0);
    private List<MyHandDrillDefinition> m_availableDrills = new List<MyHandDrillDefinition>();
    private List<MyWelderDefinition> m_availableWelders = new List<MyWelderDefinition>();
    private List<MyAngleGrinderDefinition> m_availableGrinders = new List<MyAngleGrinderDefinition>();
    private List<MyPhysicalItemDefinition> m_availableWeapons = new List<MyPhysicalItemDefinition>();
    private MyStringId m_lastShipControl = MyStringId.NullOrEmpty;
    private MyTimeSpan m_lastShipControlPressed = MyTimeSpan.Zero;
    private MyDefinitionId? m_lastWeaponId;

    public event Action<bool> ToolSwitched;

    public event Action ToolsRefreshed;

    public bool SwitchingEnabled { get; set; }

    public void RefreshAvailableTools()
    {
      this.m_availableDrills.Clear();
      this.m_availableWelders.Clear();
      this.m_availableGrinders.Clear();
      this.m_availableWeapons.Clear();
      foreach (MyPhysicalItemDefinition weaponDefinition in MyDefinitionManager.Static.GetWeaponDefinitions())
      {
        if (weaponDefinition.Public)
        {
          MyCharacter localCharacter = MySession.Static.LocalCharacter;
          if ((localCharacter != null ? (localCharacter.FindWeaponItemByDefinition(weaponDefinition.Id).HasValue ? 1 : 0) : 0) != 0 || MySession.Static.CreativeMode)
          {
            switch (MyDefinitionManager.Static.TryGetHandItemForPhysicalItem(weaponDefinition.Id))
            {
              case MyHandDrillDefinition handDrillDefinition:
                this.m_availableDrills.Add(handDrillDefinition);
                continue;
              case MyWelderDefinition welderDefinition:
                this.m_availableWelders.Add(welderDefinition);
                continue;
              case MyAngleGrinderDefinition grinderDefinition:
                this.m_availableGrinders.Add(grinderDefinition);
                continue;
              default:
                this.m_availableWeapons.Add(weaponDefinition);
                continue;
            }
          }
        }
      }
      this.m_availableDrills.SortNoAlloc<MyHandDrillDefinition>((Comparison<MyHandDrillDefinition>) ((x, y) => y.SpeedMultiplier.CompareTo(x.DistanceMultiplier)));
      this.m_availableWelders.SortNoAlloc<MyWelderDefinition>((Comparison<MyWelderDefinition>) ((x, y) => y.SpeedMultiplier.CompareTo(x.SpeedMultiplier)));
      this.m_availableGrinders.SortNoAlloc<MyAngleGrinderDefinition>((Comparison<MyAngleGrinderDefinition>) ((x, y) => y.SpeedMultiplier.CompareTo(x.SpeedMultiplier)));
      Action toolsRefreshed = this.ToolsRefreshed;
      if (toolsRefreshed == null)
        return;
      toolsRefreshed();
    }

    public override void HandleInput()
    {
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      if (localCharacter != null)
      {
        IMyHandheldGunObject<MyDeviceBase> currentWeapon = localCharacter.CurrentWeapon;
      }
      if (MyScreenManager.GetScreenWithFocus() != MyGuiScreenGamePlay.Static)
        return;
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      MyStringId context = controlledEntity != null ? controlledEntity.AuxiliaryContext : MyStringId.NullOrEmpty;
      if (context != MySpaceBindingCreator.AX_TOOLS && context != MySpaceBindingCreator.AX_ACTIONS)
        return;
      if (MyControllerHelper.IsControl(context, MyControlsSpace.SLOT0))
      {
        MySession.Static.LocalCharacter?.SwitchToWeapon((MyToolbarItemWeapon) null);
      }
      else
      {
        this.RefreshAvailableTools();
        if (MyControllerHelper.IsControl(context, MyControlsSpace.TOOL_UP))
          this.SwitchToDrill();
        if (MyControllerHelper.IsControl(context, MyControlsSpace.TOOL_LEFT))
          this.SwitchToGrinder();
        if (MyControllerHelper.IsControl(context, MyControlsSpace.TOOL_RIGHT))
          this.SwitchToWelder();
        if (MyControllerHelper.IsControl(context, MyControlsSpace.TOOL_DOWN))
          this.SwitchToWeapon();
        MyTimeSpan myTimeSpan = MyTimeSpan.FromMilliseconds(MySession.Static.ElapsedGameTime.TotalMilliseconds);
        if (MyControllerHelper.IsControl(context, MyControlsSpace.ACTION_UP))
        {
          this.m_lastShipControl = MyControlsSpace.ACTION_UP;
          this.m_lastShipControlPressed = myTimeSpan;
        }
        else if (MyControllerHelper.IsControl(context, MyControlsSpace.ACTION_UP, MyControlStateType.NEW_RELEASED))
          this.ActivateGamepadToolbar(0);
        else if (MyControllerHelper.IsControl(context, MyControlsSpace.ACTION_UP, MyControlStateType.PRESSED) && this.m_lastShipControl == MyControlsSpace.ACTION_UP && myTimeSpan - this.m_lastShipControlPressed > MyToolSwitcher.CONFIG_PRESS_LENGTH)
          this.OpenToolbarConfig(0);
        if (MyControllerHelper.IsControl(context, MyControlsSpace.ACTION_LEFT))
        {
          this.m_lastShipControl = MyControlsSpace.ACTION_LEFT;
          this.m_lastShipControlPressed = myTimeSpan;
        }
        else if (MyControllerHelper.IsControl(context, MyControlsSpace.ACTION_LEFT, MyControlStateType.NEW_RELEASED))
          this.ActivateGamepadToolbar(1);
        else if (MyControllerHelper.IsControl(context, MyControlsSpace.ACTION_LEFT, MyControlStateType.PRESSED) && this.m_lastShipControl == MyControlsSpace.ACTION_LEFT && myTimeSpan - this.m_lastShipControlPressed > MyToolSwitcher.CONFIG_PRESS_LENGTH)
          this.OpenToolbarConfig(1);
        if (MyControllerHelper.IsControl(context, MyControlsSpace.ACTION_RIGHT))
        {
          this.m_lastShipControl = MyControlsSpace.ACTION_RIGHT;
          this.m_lastShipControlPressed = myTimeSpan;
        }
        else if (MyControllerHelper.IsControl(context, MyControlsSpace.ACTION_RIGHT, MyControlStateType.NEW_RELEASED))
          this.ActivateGamepadToolbar(2);
        else if (MyControllerHelper.IsControl(context, MyControlsSpace.ACTION_RIGHT, MyControlStateType.PRESSED) && this.m_lastShipControl == MyControlsSpace.ACTION_RIGHT && myTimeSpan - this.m_lastShipControlPressed > MyToolSwitcher.CONFIG_PRESS_LENGTH)
          this.OpenToolbarConfig(2);
        if (MyControllerHelper.IsControl(context, MyControlsSpace.ACTION_DOWN))
        {
          this.m_lastShipControl = MyControlsSpace.ACTION_DOWN;
          this.m_lastShipControlPressed = myTimeSpan;
        }
        else if (MyControllerHelper.IsControl(context, MyControlsSpace.ACTION_DOWN, MyControlStateType.NEW_RELEASED))
          this.ActivateGamepadToolbar(3);
        else if (MyControllerHelper.IsControl(context, MyControlsSpace.ACTION_DOWN, MyControlStateType.PRESSED) && this.m_lastShipControl == MyControlsSpace.ACTION_DOWN && myTimeSpan - this.m_lastShipControlPressed > MyToolSwitcher.CONFIG_PRESS_LENGTH)
          this.OpenToolbarConfig(3);
        if (MyControllerHelper.IsControl(context, MyControlsSpace.TOOLBAR_PREVIOUS))
          MyToolbarComponent.CurrentToolbar.PageDownGamepad();
        if (MyControllerHelper.IsControl(context, MyControlsSpace.TOOLBAR_NEXT))
          MyToolbarComponent.CurrentToolbar.PageUpGamepad();
        base.HandleInput();
      }
    }

    private void OpenToolbarConfig(int id)
    {
      int toolbarId = this.ComputeToolbarId(id);
      MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.ToolbarConfigScreen, (object) 0, (object) (MySession.Static.ControlledEntity as MyShipController), (object) toolbarId));
    }

    private void ActivateGamepadToolbar(int id) => MyToolbarComponent.CurrentToolbar.ActivateGamepadItemAtIndex(this.ComputeToolbarId(id));

    private int ComputeToolbarId(int slot) => MyToolbarComponent.CurrentToolbar.SlotToIndexGamepad(slot);

    public void SwitchToDrill() => this.SwitchToTool<MyHandDrillDefinition>(this.m_availableDrills);

    public void SwitchToWelder() => this.SwitchToTool<MyWelderDefinition>(this.m_availableWelders);

    public void SwitchToGrinder() => this.SwitchToTool<MyAngleGrinderDefinition>(this.m_availableGrinders);

    public void SwitchToWeapon() => this.SwitchToTool<MyPhysicalItemDefinition>(this.m_availableWeapons, true);

    public MyDefinitionId? GetCurrentOrNextTool(MyToolSwitcher.ToolType type)
    {
      switch (type)
      {
        case MyToolSwitcher.ToolType.Drill:
          return this.GetTool<MyHandDrillDefinition>(this.m_availableDrills, false);
        case MyToolSwitcher.ToolType.Welder:
          return this.GetTool<MyWelderDefinition>(this.m_availableWelders, false);
        case MyToolSwitcher.ToolType.Grinder:
          return this.GetTool<MyAngleGrinderDefinition>(this.m_availableGrinders, false);
        case MyToolSwitcher.ToolType.Weapon:
          return this.GetTool<MyPhysicalItemDefinition>(this.m_availableWeapons, false, true);
        default:
          return new MyDefinitionId?();
      }
    }

    private MyDefinitionId? GetTool<T>(List<T> type, bool next, bool weapon = false) where T : MyDefinitionBase
    {
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      if (localCharacter == null || type.Count == 0)
        return new MyDefinitionId?();
      IMyHandheldGunObject<MyDeviceBase> currentWeapon = localCharacter.CurrentWeapon;
      MyDefinitionId currentTool = new MyDefinitionId();
      int index;
      if (weapon)
      {
        int num = next ? 1 : 0;
        if (currentWeapon == null || type.FindIndex((Predicate<T>) (x => x.Id == MyDefinitionManager.Static.GetPhysicalItemForHandItem(currentWeapon.DefinitionId).Id)) == -1)
        {
          if (this.m_lastWeaponId.HasValue)
          {
            currentTool = this.m_lastWeaponId.Value;
            index = Math.Max(type.FindIndex((Predicate<T>) (x => x.Id == currentTool)), 0);
          }
          else
            index = 0;
        }
        else
        {
          index = type.FindIndex((Predicate<T>) (x => x.Id == MyDefinitionManager.Static.GetPhysicalItemForHandItem(currentWeapon.DefinitionId).Id));
          if (next || index == -1)
            index = (index + 1) % type.Count;
        }
      }
      else
      {
        currentTool = currentWeapon != null ? currentWeapon.DefinitionId : new MyDefinitionId();
        index = type.FindIndex((Predicate<T>) (x => x.Id == currentTool));
        if (next || index == -1)
          index = (index + 1) % type.Count;
      }
      return new MyDefinitionId?(type[index].Id);
    }

    private void SwitchToTool<T>(List<T> type, bool weapon = false) where T : MyDefinitionBase
    {
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      if (localCharacter == null)
        return;
      this.RefreshAvailableTools();
      if (type.Count == 0)
        return;
      MyDefinitionId? tool = this.GetTool<T>(type, true, weapon);
      localCharacter.SwitchToWeapon(tool);
      if (weapon)
        this.m_lastWeaponId = tool;
      Action<bool> toolSwitched = this.ToolSwitched;
      if (toolSwitched == null)
        return;
      toolSwitched(false);
    }

    internal bool IsEquipped(MyToolSwitcher.ToolType tool)
    {
      switch (tool)
      {
        case MyToolSwitcher.ToolType.Drill:
          MyCharacter localCharacter1 = MySession.Static.LocalCharacter;
          if (localCharacter1 == null || localCharacter1.CurrentWeapon == null || this.m_availableDrills.Count <= 0)
            return false;
          MyDefinitionId toolId1 = localCharacter1.CurrentWeapon.DefinitionId;
          return this.m_availableDrills.FindIndex((Predicate<MyHandDrillDefinition>) (x => x.Id == toolId1)) != -1;
        case MyToolSwitcher.ToolType.Welder:
          MyCharacter localCharacter2 = MySession.Static.LocalCharacter;
          if (localCharacter2 == null || localCharacter2.CurrentWeapon == null || this.m_availableDrills.Count <= 0)
            return false;
          MyDefinitionId toolId2 = localCharacter2.CurrentWeapon.DefinitionId;
          return this.m_availableWelders.FindIndex((Predicate<MyWelderDefinition>) (x => x.Id == toolId2)) != -1;
        case MyToolSwitcher.ToolType.Grinder:
          MyCharacter localCharacter3 = MySession.Static.LocalCharacter;
          if (localCharacter3 == null || localCharacter3.CurrentWeapon == null || this.m_availableDrills.Count <= 0)
            return false;
          MyDefinitionId toolId3 = localCharacter3.CurrentWeapon.DefinitionId;
          return this.m_availableGrinders.FindIndex((Predicate<MyAngleGrinderDefinition>) (x => x.Id == toolId3)) != -1;
        case MyToolSwitcher.ToolType.Weapon:
          MyCharacter localCharacter4 = MySession.Static.LocalCharacter;
          if (localCharacter4 == null || localCharacter4.CurrentWeapon == null || this.m_availableDrills.Count <= 0)
            return false;
          MyDefinitionId toolId4 = MyDefinitionManager.Static.GetPhysicalItemForHandItem(localCharacter4.CurrentWeapon.DefinitionId).Id;
          return this.m_availableWeapons.FindIndex((Predicate<MyPhysicalItemDefinition>) (x => x.Id == toolId4)) != -1;
        default:
          return false;
      }
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      this.Session = (IMySession) null;
    }

    public enum ToolType
    {
      Drill,
      Welder,
      Grinder,
      Weapon,
    }
  }
}
