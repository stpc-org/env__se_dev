// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MyEntityController
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Multiplayer;
using System;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;

namespace Sandbox.Game.World
{
  public class MyEntityController : IMyEntityController
  {
    private Action<MyEntity> m_controlledEntityClosing;

    public Sandbox.Game.Entities.IMyControllableEntity ControlledEntity { get; protected set; }

    public event Action<Sandbox.Game.Entities.IMyControllableEntity, Sandbox.Game.Entities.IMyControllableEntity> ControlledEntityChanged;

    public MyPlayer Player { get; private set; }

    public MyEntityController(MyPlayer parent)
    {
      this.Player = parent;
      this.m_controlledEntityClosing = new Action<MyEntity>(this.ControlledEntity_OnClosing);
    }

    public void SaveCamera()
    {
      if (this.ControlledEntity == null || Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      bool isLocalCharacter = this.ControlledEntity is MyCharacter && MySession.Static.LocalCharacter == this.ControlledEntity;
      if (this.ControlledEntity is MyCharacter && MySession.Static.LocalCharacter != this.ControlledEntity)
        return;
      MyEntityCameraSettings cameraEntitySettings = this.ControlledEntity.GetCameraEntitySettings();
      float headLocalXangle = this.ControlledEntity.HeadLocalXAngle;
      float headLocalYangle = this.ControlledEntity.HeadLocalYAngle;
      MySession.Static.Cameras.SaveEntityCameraSettings(this.Player.Id, this.ControlledEntity.Entity.EntityId, cameraEntitySettings != null ? cameraEntitySettings.IsFirstPerson : MySession.Static.GetCameraControllerEnum() != MyCameraControllerEnum.ThirdPersonSpectator, MyThirdPersonSpectator.Static.GetViewerDistance(), isLocalCharacter, headLocalXangle, headLocalYangle);
    }

    public void TakeControl(Sandbox.Game.Entities.IMyControllableEntity entity)
    {
      if (this.ControlledEntity == entity || entity != null && entity.ControllerInfo.Controller != null)
        return;
      Sandbox.Game.Entities.IMyControllableEntity controlledEntity = this.ControlledEntity;
      this.SaveCamera();
      if (this.ControlledEntity != null)
      {
        this.ControlledEntity.Entity.OnClosing -= this.m_controlledEntityClosing;
        this.ControlledEntity.ControllerInfo.Controller = (MyEntityController) null;
      }
      this.ControlledEntity = entity;
      if (entity != null)
      {
        this.ControlledEntity.Entity.OnClosing += this.m_controlledEntityClosing;
        this.ControlledEntity.ControllerInfo.Controller = this;
        this.SetCamera();
      }
      if (controlledEntity == entity)
        return;
      this.RaiseControlledEntityChanged(controlledEntity, entity);
    }

    private void RaiseControlledEntityChanged(
      Sandbox.Game.Entities.IMyControllableEntity old,
      Sandbox.Game.Entities.IMyControllableEntity entity)
    {
      Action<Sandbox.Game.Entities.IMyControllableEntity, Sandbox.Game.Entities.IMyControllableEntity> controlledEntityChanged = this.ControlledEntityChanged;
      if (controlledEntityChanged == null)
        return;
      controlledEntityChanged(old, entity);
    }

    private void ControlledEntity_OnClosing(MyEntity entity)
    {
      if (this.ControlledEntity == null)
        return;
      this.TakeControl((Sandbox.Game.Entities.IMyControllableEntity) null);
    }

    public void SetCamera()
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated || !(this.ControlledEntity.Entity is IMyCameraController))
        return;
      MySession.Static.SetEntityCameraPosition(this.Player.Id, (IMyEntity) this.ControlledEntity.Entity);
    }

    void IMyEntityController.TakeControl(VRage.Game.ModAPI.Interfaces.IMyControllableEntity entity)
    {
      if (!(entity is Sandbox.Game.Entities.IMyControllableEntity))
        return;
      this.TakeControl(entity as Sandbox.Game.Entities.IMyControllableEntity);
    }

    private Action<Sandbox.Game.Entities.IMyControllableEntity, Sandbox.Game.Entities.IMyControllableEntity> GetDelegate(
      Action<VRage.Game.ModAPI.Interfaces.IMyControllableEntity, VRage.Game.ModAPI.Interfaces.IMyControllableEntity> value)
    {
      return (Action<Sandbox.Game.Entities.IMyControllableEntity, Sandbox.Game.Entities.IMyControllableEntity>) Delegate.CreateDelegate(typeof (Action<Sandbox.Game.Entities.IMyControllableEntity, Sandbox.Game.Entities.IMyControllableEntity>), value.Target, value.Method);
    }

    event Action<VRage.Game.ModAPI.Interfaces.IMyControllableEntity, VRage.Game.ModAPI.Interfaces.IMyControllableEntity> IMyEntityController.ControlledEntityChanged
    {
      add => this.ControlledEntityChanged += this.GetDelegate(value);
      remove => this.ControlledEntityChanged -= this.GetDelegate(value);
    }

    VRage.Game.ModAPI.Interfaces.IMyControllableEntity IMyEntityController.ControlledEntity => (VRage.Game.ModAPI.Interfaces.IMyControllableEntity) this.ControlledEntity;
  }
}
