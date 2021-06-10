// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyRadialMenuItemFactory
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Screens.Helpers.RadialMenuActions;
using System.Collections.Generic;
using System.Reflection;
using VRage.Game;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Screens.Helpers
{
  internal static class MyRadialMenuItemFactory
  {
    private static MyObjectFactory<MyRadialMenuItemDescriptor, MyRadialMenuItem> m_objectFactory = new MyObjectFactory<MyRadialMenuItemDescriptor, MyRadialMenuItem>();
    private static Dictionary<MySystemAction, IMyRadialMenuSystemAction> m_systemActions;

    static MyRadialMenuItemFactory()
    {
      MyRadialMenuItemFactory.m_objectFactory.RegisterFromAssembly(Assembly.GetAssembly(typeof (MyRadialMenuItem)));
      MyRadialMenuItemFactory.m_systemActions = new Dictionary<MySystemAction, IMyRadialMenuSystemAction>();
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.ActiveContractsScreen, (IMyRadialMenuSystemAction) new MyActionActiveContractsScreen());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.AdminMenu, (IMyRadialMenuSystemAction) new MyActionAdminMenu());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.BlueprintsScreen, (IMyRadialMenuSystemAction) new MyActionBlueprintScreen());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.Chat, (IMyRadialMenuSystemAction) new MyActionChat());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.ColorPicker, (IMyRadialMenuSystemAction) new MyActionColorPicker());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.ColorTool, (IMyRadialMenuSystemAction) new MyActionColorTool());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.CopyGrid, (IMyRadialMenuSystemAction) new MyActionCopyGrid());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.CreateBlueprint, (IMyRadialMenuSystemAction) new MyActionCreateBlueprint());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.CutGrid, (IMyRadialMenuSystemAction) new MyActionCutGrid());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.OpenInventory, (IMyRadialMenuSystemAction) new MyActionOpenInventory());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.PasteGrid, (IMyRadialMenuSystemAction) new MyActionPasteGrid());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.PlacementMode, (IMyRadialMenuSystemAction) new MyActionPlacementMode());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.ReloadSession, (IMyRadialMenuSystemAction) new MyActionReloadSession());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.Respawn, (IMyRadialMenuSystemAction) new MyActionRespawn());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.ShowHelpScreen, (IMyRadialMenuSystemAction) new MyActionShowHelpScreen());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.ShowPlayers, (IMyRadialMenuSystemAction) new MyActionShowPlayers());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.ShowProgressionTree, (IMyRadialMenuSystemAction) new MyActionShowProgressionTree());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.SpawnMenu, (IMyRadialMenuSystemAction) new MyActionSpawnMenu());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.SwitchCamera, (IMyRadialMenuSystemAction) new MyActionSwitchCamera());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.SymmetrySetup, (IMyRadialMenuSystemAction) new MyActionSymmetrySetup());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.ToggleAutoRotation, (IMyRadialMenuSystemAction) new MyActionToggleAutoRotation());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.ToggleBroadcasting, (IMyRadialMenuSystemAction) new MyActionToggleBroadcasting());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.ToggleConnectors, (IMyRadialMenuSystemAction) new MyActionToggleConnectors());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.ToggleDampeners, (IMyRadialMenuSystemAction) new MyActionToggleDampeners());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.ToggleHud, (IMyRadialMenuSystemAction) new MyActionToggleHud());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.ToggleLights, (IMyRadialMenuSystemAction) new MyActionToggleLights());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.ToggleMultiBlock, (IMyRadialMenuSystemAction) new MyActionToggleMultiBlock());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.TogglePower, (IMyRadialMenuSystemAction) new MyActionTogglePower());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.ToggleSignals, (IMyRadialMenuSystemAction) new MyActionToggleSignals());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.ToggleSymmetry, (IMyRadialMenuSystemAction) new MyActionToggleSymmetry());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.ToggleVisor, (IMyRadialMenuSystemAction) new MyActionToggleVisor());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.Unequip, (IMyRadialMenuSystemAction) new MyActionUnequip());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.ViewMode, (IMyRadialMenuSystemAction) new MyActionViewMode());
      MyRadialMenuItemFactory.m_systemActions.Add(MySystemAction.ToggleHandbrake, (IMyRadialMenuSystemAction) new MyActionToggleHandbrake());
    }

    public static MyRadialMenuItem CreateRadialMenuItem(
      MyObjectBuilder_RadialMenuItem data)
    {
      MyRadialMenuItem instance = MyRadialMenuItemFactory.m_objectFactory.CreateInstance(data.TypeId);
      instance.Init(data);
      return instance;
    }

    internal static IMyRadialMenuSystemAction GetSystemMenuAction(
      MySystemAction actionKey)
    {
      IMyRadialMenuSystemAction menuSystemAction;
      return !MyRadialMenuItemFactory.m_systemActions.TryGetValue(actionKey, out menuSystemAction) ? (IMyRadialMenuSystemAction) null : menuSystemAction;
    }
  }
}
