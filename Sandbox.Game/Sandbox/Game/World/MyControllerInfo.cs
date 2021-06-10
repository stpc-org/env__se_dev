// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MyControllerInfo
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Multiplayer;
using System;
using VRage.Game.ModAPI;

namespace Sandbox.Game.World
{
  public class MyControllerInfo : IMyControllerInfo
  {
    private MyEntityController m_controller;

    public MyEntityController Controller
    {
      get => this.m_controller;
      set
      {
        if (this.m_controller == value)
          return;
        this.ReleaseControls();
        this.m_controller = value;
        this.AcquireControls();
      }
    }

    public long ControllingIdentityId => this.Controller == null ? 0L : this.Controller.Player.Identity.IdentityId;

    public event Action<MyEntityController> ControlAcquired;

    public event Action<MyEntityController> ControlReleased;

    public bool IsLocallyControlled() => this.Controller != null && Sync.Clients != null && this.Controller.Player.Client == Sync.Clients.LocalClient;

    public bool IsLocallyHumanControlled() => this.Controller != null && Sync.Clients != null && Sync.Clients.LocalClient != null && this.Controller.Player == Sync.Clients.LocalClient.FirstPlayer;

    public bool IsRemotelyControlled() => this.Controller != null && this.Controller.Player.Client != Sync.Clients.LocalClient;

    private Action<MyEntityController> GetDelegate(
      Action<IMyEntityController> value)
    {
      return (Action<MyEntityController>) Delegate.CreateDelegate(typeof (Action<MyEntityController>), value.Target, value.Method);
    }

    IMyEntityController IMyControllerInfo.Controller => (IMyEntityController) this.Controller;

    long IMyControllerInfo.ControllingIdentityId => this.ControllingIdentityId;

    event Action<IMyEntityController> IMyControllerInfo.ControlAcquired
    {
      add => this.ControlAcquired += this.GetDelegate(value);
      remove => this.ControlAcquired -= this.GetDelegate(value);
    }

    event Action<IMyEntityController> IMyControllerInfo.ControlReleased
    {
      add => this.ControlReleased += this.GetDelegate(value);
      remove => this.ControlReleased -= this.GetDelegate(value);
    }

    bool IMyControllerInfo.IsLocallyControlled() => this.IsLocallyControlled();

    bool IMyControllerInfo.IsLocallyHumanControlled() => this.IsLocallyHumanControlled();

    bool IMyControllerInfo.IsRemotelyControlled() => this.IsRemotelyControlled();

    public void ReleaseControls()
    {
      if (this.m_controller == null)
        return;
      Action<MyEntityController> controlReleased = this.ControlReleased;
      if (controlReleased == null)
        return;
      controlReleased(this.m_controller);
    }

    public void AcquireControls()
    {
      if (this.m_controller == null)
        return;
      Action<MyEntityController> controlAcquired = this.ControlAcquired;
      if (controlAcquired == null)
        return;
      controlAcquired(this.m_controller);
    }
  }
}
