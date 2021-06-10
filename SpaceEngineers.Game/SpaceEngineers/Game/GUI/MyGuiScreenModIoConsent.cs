// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.GUI.MyGuiScreenModIoConsent
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Generated;
using Sandbox;
using Sandbox.Game.Screens.ViewModels;
using VRage.Input;
using VRage.Utils;

namespace SpaceEngineers.Game.GUI
{
  public class MyGuiScreenModIoConsent : MyGuiScreenMvvmBase
  {
    private int m_pauseInput;

    public MyGuiScreenModIoConsent(MyModIoConsentViewModel viewModel)
      : base((MyViewModelBase) viewModel)
      => MyLog.Default.WriteLine("MyGuiScreenModIoConsent OPEN");

    public override string GetFriendlyName() => nameof (MyGuiScreenModIoConsent);

    public override UIRoot CreateView() => MyInput.Static.IsJoystickLastUsed ? (UIRoot) new ModIoConsentView_Gamepad(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y) : (UIRoot) new ModIoConsentView(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y);

    public override bool Update(bool hasFocus)
    {
      int num = MySandboxGame.Static.PauseInput ? 1 : 0;
      bool flag = MyInput.Static.IsKeyPress(MyKeys.Escape);
      if (num != 0)
        this.m_pauseInput = 10;
      else if (this.m_pauseInput > 0 && !flag)
        --this.m_pauseInput;
      return base.Update(hasFocus);
    }

    protected override void Canceling()
    {
      if (this.m_pauseInput > 0)
        return;
      base.Canceling();
    }

    public override bool CloseScreen(bool isUnloading = false)
    {
      MyLog.Default.WriteLine("MyGuiScreenModIoConsent CLOSE");
      return base.CloseScreen(isUnloading);
    }
  }
}
