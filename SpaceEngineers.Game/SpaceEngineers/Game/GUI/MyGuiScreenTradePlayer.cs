// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.GUI.MyGuiScreenTradePlayer
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
  public class MyGuiScreenTradePlayer : MyGuiScreenMvvmBase
  {
    public MyGuiScreenTradePlayer(MyPlayerTradeViewModel viewModel)
      : base((MyViewModelBase) viewModel)
      => MyLog.Default.WriteLine("MyGuiScreenTradePlayer OPEN");

    public override string GetFriendlyName() => nameof (MyGuiScreenTradePlayer);

    public override UIRoot CreateView() => MyInput.Static.IsJoystickLastUsed ? (UIRoot) new PlayerTradeView_Gamepad(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y) : (UIRoot) new PlayerTradeView(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y);

    public override bool CloseScreen(bool isUnloading = false)
    {
      MyLog.Default.WriteLine("MyGuiScreenTradePlayer CLOSE");
      return base.CloseScreen(isUnloading);
    }
  }
}
