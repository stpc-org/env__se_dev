// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyAdminMenuControlHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using VRage;

namespace Sandbox.Game.Screens.Helpers
{
  public class MyAdminMenuControlHelper : MyAbstractControlMenuItem
  {
    public override string Label => MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ShowAdminMenu);

    public MyAdminMenuControlHelper()
      : base("F10", MySupportKeysEnum.ALT)
    {
    }

    public override void Activate()
    {
      MyScreenManager.CloseScreen(typeof (MyGuiScreenControlMenu));
      MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.AdminMenuScreen));
    }
  }
}
