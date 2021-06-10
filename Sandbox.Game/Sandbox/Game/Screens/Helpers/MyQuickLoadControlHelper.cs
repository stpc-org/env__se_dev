// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyQuickLoadControlHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System.IO;
using VRage;

namespace Sandbox.Game.Screens.Helpers
{
  public class MyQuickLoadControlHelper : MyAbstractControlMenuItem
  {
    public MyQuickLoadControlHelper()
      : base("F5")
    {
    }

    public override string Label => MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_QuickLoad);

    public override void Activate()
    {
      MyScreenManager.CloseScreen(typeof (MyGuiScreenControlMenu));
      if (Sync.IsServer)
      {
        if (MyAsyncSaving.InProgress)
        {
          MyGuiScreenMessageBox messageBox = MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextSavingInProgress), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
          messageBox.SkipTransition = true;
          messageBox.InstantClose = false;
          MyGuiSandbox.AddScreen((MyGuiScreenBase) messageBox);
        }
        else
        {
          if (!Directory.Exists(MySession.Static.CurrentPath))
            return;
          MyGuiScreenGamePlay.Static.ShowLoadMessageBox(MySession.Static.CurrentPath);
        }
      }
      else
        MyGuiScreenGamePlay.Static.ShowReconnectMessageBox();
    }
  }
}
