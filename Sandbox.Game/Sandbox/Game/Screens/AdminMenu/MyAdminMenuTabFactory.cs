// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.AdminMenu.MyAdminMenuTabFactory
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics.GUI;

namespace Sandbox.Game.Screens.AdminMenu
{
  internal static class MyAdminMenuTabFactory
  {
    public static MyTabContainer CreateTab(
      MyGuiScreenBase parentScreen,
      MyTabControlEnum tabType)
    {
      MyTabContainer myTabContainer = (MyTabContainer) null;
      switch (tabType)
      {
        case MyTabControlEnum.General:
          myTabContainer = (MyTabContainer) new MyTrashGeneralTabContainer(parentScreen);
          break;
        case MyTabControlEnum.Voxel:
          myTabContainer = (MyTabContainer) new MyTrashVoxelTabContainer(parentScreen);
          break;
        case MyTabControlEnum.Other:
          myTabContainer = (MyTabContainer) new MyTrashOtherTabContainer(parentScreen);
          break;
      }
      myTabContainer.Control.Visible = false;
      return myTabContainer;
    }
  }
}
