// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.GUI.MyGuiScreenFactoryService
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Game.Screens.ViewModels;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;

namespace SpaceEngineers.Game.GUI
{
  public class MyGuiScreenFactoryService : IMyGuiScreenFactoryService
  {
    public bool IsAnyScreenOpen { get; set; }

    public void CreateScreen(ViewModelBase viewModel)
    {
      if (this.IsAnyScreenOpen)
        return;
      MyGuiScreenMvvmBase guiScreenMvvmBase = (MyGuiScreenMvvmBase) null;
      if (viewModel.GetType() == typeof (MyPlayerTradeViewModel))
        guiScreenMvvmBase = (MyGuiScreenMvvmBase) MyGuiSandbox.CreateScreen<MyGuiScreenTradePlayer>((object) viewModel);
      else if (viewModel.GetType() == typeof (MyEditFactionIconViewModel))
        guiScreenMvvmBase = (MyGuiScreenMvvmBase) MyGuiSandbox.CreateScreen<MyGuiScreenEditFactionIcon>((object) viewModel);
      else if (viewModel.GetType() == typeof (MyContractsActiveViewModel))
        guiScreenMvvmBase = (MyGuiScreenMvvmBase) MyGuiSandbox.CreateScreen<MyGuiScreenActiveContracts>((object) viewModel);
      else if (viewModel.GetType() == typeof (MyWorkshopBrowserViewModel))
        guiScreenMvvmBase = (MyGuiScreenMvvmBase) MyGuiSandbox.CreateScreen<MyGuiScreenWorkshopBrowser>((object) viewModel);
      else if (viewModel.GetType() == typeof (MyModIoConsentViewModel))
        guiScreenMvvmBase = (MyGuiScreenMvvmBase) MyGuiSandbox.CreateScreen<MyGuiScreenModIoConsent>((object) viewModel);
      if (viewModel is MyViewModelBase)
        ((MyViewModelBase) viewModel).ScreenBase = (MyGuiScreenBase) guiScreenMvvmBase;
      this.AddScreen((MyGuiScreenBase) guiScreenMvvmBase);
    }

    public Type GetMyGuiScreenBase(Type viewModelType)
    {
      if (viewModelType == typeof (MyPlayerTradeViewModel))
        return typeof (MyGuiScreenTradePlayer);
      if (viewModelType == typeof (MyEditFactionIconViewModel))
        return typeof (MyGuiScreenEditFactionIcon);
      if (viewModelType == typeof (MyContractsActiveViewModel))
        return typeof (MyGuiScreenActiveContracts);
      if (viewModelType == typeof (MyWorkshopBrowserViewModel))
        return typeof (MyGuiScreenWorkshopBrowser);
      return viewModelType == typeof (MyModIoConsentViewModel) ? typeof (MyGuiScreenModIoConsent) : (Type) null;
    }

    private void AddScreen(MyGuiScreenBase screen)
    {
      MyGuiSandbox.AddScreen(screen);
      this.IsAnyScreenOpen = true;
      screen.Closed += new MyGuiScreenBase.ScreenHandler(this.Screen_Closed);
    }

    private void Screen_Closed(MyGuiScreenBase screen, bool isUnloading)
    {
      this.IsAnyScreenOpen = false;
      screen.Closed -= new MyGuiScreenBase.ScreenHandler(this.Screen_Closed);
    }
  }
}
