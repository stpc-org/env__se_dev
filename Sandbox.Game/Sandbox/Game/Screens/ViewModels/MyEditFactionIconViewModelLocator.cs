﻿// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.ViewModels.MyEditFactionIconViewModelLocator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Mvvm;

namespace Sandbox.Game.Screens.ViewModels
{
  public class MyEditFactionIconViewModelLocator : ViewModelLocatorBase<IMyEditFactionIconViewModel>
  {
    public MyEditFactionIconViewModelLocator()
      : base()
    {
    }

    public MyEditFactionIconViewModelLocator(bool isDesignMode = true)
      : base()
    {
    }

    public override void Initialize(bool isDesignMode) => this.IsInDesignMode = false;
  }
}
