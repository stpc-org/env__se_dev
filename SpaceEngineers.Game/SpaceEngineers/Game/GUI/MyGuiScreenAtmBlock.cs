// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.GUI.MyGuiScreenAtmBlock
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
  public class MyGuiScreenAtmBlock : MyGuiScreenMvvmBase
  {
    public MyGuiScreenAtmBlock(MyStoreBlockViewModel viewModel)
      : base((MyViewModelBase) viewModel)
      => MyLog.Default.WriteLine("MyGuiScreenAtmBlock OPEN");

    public override string GetFriendlyName() => nameof (MyGuiScreenAtmBlock);

    public override UIRoot CreateView() => MyInput.Static.IsJoystickLastUsed ? (UIRoot) new AtmBlockView_Gamepad(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y) : (UIRoot) new AtmBlockView(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y);

    public override bool CloseScreen(bool isUnloading = false)
    {
      MyLog.Default.WriteLine("MyGuiScreenAtmBlock CLOSE");
      return base.CloseScreen(isUnloading);
    }
  }
}
