// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.RadialMenuActions.MyActionActiveContractsScreen
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Game.Gui;
using Sandbox.Game.Screens.ViewModels;
using Sandbox.Graphics;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
  public class MyActionActiveContractsScreen : MyActionBase
  {
    public override void ExecuteAction()
    {
      if (MyGuiScreenGamePlay.ActiveGameplayScreen != null)
        return;
      MyContractsActiveViewModel contractsActiveViewModel = new MyContractsActiveViewModel();
      ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>().CreateScreen((ViewModelBase) contractsActiveViewModel);
    }
  }
}
