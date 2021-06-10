// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.GUI.MyGuiScreenContractsBlock
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
  public class MyGuiScreenContractsBlock : MyGuiScreenMvvmBase
  {
    public MyGuiScreenContractsBlock(MyContractsBlockViewModel viewModel)
      : base((MyViewModelBase) viewModel)
      => MyLog.Default.WriteLine("MyGuiScreenContractsBlock OPEN");

    public override string GetFriendlyName() => nameof (MyGuiScreenContractsBlock);

    public override UIRoot CreateView() => MyInput.Static.IsJoystickLastUsed ? (UIRoot) new ContractsBlockView_Gamepad(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y) : (UIRoot) new ContractsBlockView(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y);

    protected override bool CanExit(object parameter) => this.m_viewModel.CanExit(parameter);

    public override bool CloseScreen(bool isUnloading = false)
    {
      MyLog.Default.WriteLine("MyGuiScreenContractsBlock CLOSE");
      return base.CloseScreen(isUnloading);
    }
  }
}
