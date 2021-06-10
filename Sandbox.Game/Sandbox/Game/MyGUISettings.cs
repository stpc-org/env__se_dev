// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.MyGUISettings
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRageMath;

namespace Sandbox.Game
{
  public struct MyGUISettings
  {
    public bool EnableToolbarConfigScreen;
    public bool EnableTerminalScreen;
    public bool MultipleSpinningWheels;
    public Type HUDScreen;
    public Type ToolbarConfigScreen;
    public Type ToolbarControl;
    public Type OptionsScreen;
    public Type ScenarioScreen;
    public Type EditWorldSettingsScreen;
    public Type HelpScreen;
    public Type VoxelMapEditingScreen;
    public Type GameplayOptionsScreen;
    public Type InventoryScreen;
    public Type AdminMenuScreen;
    public Type FactionScreen;
    public Type CreateFactionScreen;
    public Type PlayersScreen;
    public Type MainMenu;
    public Type PerformanceWarningScreen;
    public string[] MainMenuBackgroundVideos;
    public Vector2I LoadingScreenIndexRange;
  }
}
