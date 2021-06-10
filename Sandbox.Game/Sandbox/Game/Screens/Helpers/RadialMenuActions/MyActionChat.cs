// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.RadialMenuActions.MyActionChat
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
  public class MyActionChat : MyActionBase
  {
    public override void ExecuteAction()
    {
      if (MyGuiScreenChat.Static != null)
        return;
      MyGuiScreenHudSpace.Static.VisibleChanged += new VisibleChangedDelegate(this.PostponedOpenChatScreen);
    }

    public void PostponedOpenChatScreen(object sender, bool state)
    {
      if (!state)
        return;
      this.OpenChatScreen();
      MyGuiScreenHudSpace.Static.VisibleChanged -= new VisibleChangedDelegate(this.PostponedOpenChatScreen);
    }

    public void OpenChatScreen()
    {
      Vector2 hudPos = new Vector2(0.029f, 0.8f);
      hudPos = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenChat(hudPos));
    }
  }
}
