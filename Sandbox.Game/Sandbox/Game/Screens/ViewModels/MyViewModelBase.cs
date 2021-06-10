// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.ViewModels.MyViewModelBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Graphics.GUI;
using System;
using VRage.Input;

namespace Sandbox.Game.Screens.ViewModels
{
  public abstract class MyViewModelBase : ViewModelBase
  {
    private ColorW m_backgroundOverlay;
    private ICommand m_exitCommand;
    private float m_maxWidth;
    private bool m_isJoystickLastUsed;
    private MyGuiScreenBase m_screenBase;

    public MyGuiScreenBase ScreenBase
    {
      get => this.m_screenBase;
      set => this.m_screenBase = value;
    }

    public ColorW BackgroundOverlay
    {
      get => this.m_backgroundOverlay;
      set => this.SetProperty<ColorW>(ref this.m_backgroundOverlay, value, nameof (BackgroundOverlay));
    }

    public ICommand ExitCommand
    {
      get => this.m_exitCommand;
      set => this.SetProperty<ICommand>(ref this.m_exitCommand, value, nameof (ExitCommand));
    }

    public float MaxWidth
    {
      get => this.m_maxWidth;
      set => this.SetProperty<float>(ref this.m_maxWidth, value, nameof (MaxWidth));
    }

    public bool IsJoystickLastUsed
    {
      get => this.m_isJoystickLastUsed;
      set => this.SetProperty<bool>(ref this.m_isJoystickLastUsed, value, nameof (IsJoystickLastUsed));
    }

    public MyViewModelBase(MyGuiScreenBase scrBase = null)
    {
      this.m_screenBase = scrBase;
      this.ExitCommand = (ICommand) new RelayCommand(new Action<object>(this.OnExit));
      this.IsJoystickLastUsed = MyInput.Static.IsJoystickLastUsed;
    }

    public virtual void InitializeData()
    {
    }

    public virtual bool CanExit(object parameter) => true;

    public void OnExit(object obj)
    {
      if (this.m_screenBase != null)
        this.m_screenBase.CloseScreenNow();
      else
        MyScreenManager.GetScreenWithFocus().CloseScreen();
    }

    public virtual void OnScreenClosing()
    {
    }

    public virtual void OnScreenClosed()
    {
    }

    public virtual void Update()
    {
    }
  }
}
