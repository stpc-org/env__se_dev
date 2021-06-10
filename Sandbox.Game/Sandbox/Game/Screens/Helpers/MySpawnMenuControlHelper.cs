// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MySpawnMenuControlHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;

namespace Sandbox.Game.Screens.Helpers
{
  public class MySpawnMenuControlHelper : MyAbstractControlMenuItem
  {
    public override string Label => MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ShowSpawnMenu);

    public MySpawnMenuControlHelper()
      : base("F10", MySupportKeysEnum.SHIFT)
    {
    }

    public override bool Enabled => MySession.Static.IsAdminMenuEnabled && MyPerGameSettings.Game != GameEnum.UNKNOWN_GAME;

    public override void Activate()
    {
      if (!MySession.Static.IsAdminMenuEnabled || MyPerGameSettings.Game == GameEnum.UNKNOWN_GAME)
      {
        MyHud.Notifications.Add(MyNotificationSingletons.AdminMenuNotAvailable);
      }
      else
      {
        MyScreenManager.CloseScreen(typeof (MyGuiScreenControlMenu));
        MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.VoxelMapEditingScreen));
      }
    }
  }
}
