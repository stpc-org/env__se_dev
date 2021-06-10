// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyGridCameraSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Interfaces;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GameSystems
{
  public class MyGridCameraSystem : MyUpdateableGridSystem
  {
    public static float GAMEPAD_ZOOM_SPEED = 0.02f;
    private readonly List<MyCameraBlock> m_cameras;
    private readonly List<MyCameraBlock> m_relayedCameras;
    private MyCameraBlock m_currentCamera;
    private bool m_ignoreNextInput;
    private static MyHudCameraOverlay m_cameraOverlay;

    public int CameraCount => this.m_cameras.Count;

    public MyCameraBlock CurrentCamera => this.m_currentCamera;

    public static IMyCameraController PreviousNonCameraBlockController { get; set; }

    public MyGridCameraSystem(MyCubeGrid grid)
      : base(grid)
    {
      this.m_cameras = new List<MyCameraBlock>();
      this.m_relayedCameras = new List<MyCameraBlock>();
    }

    public void Register(MyCameraBlock camera) => this.m_cameras.Add(camera);

    public void Unregister(MyCameraBlock camera)
    {
      if (camera == this.m_currentCamera)
        this.ResetCamera();
      this.m_cameras.Remove(camera);
    }

    public void CheckCurrentCameraStillValid()
    {
      if (this.m_currentCamera == null || this.m_currentCamera.IsWorking)
        return;
      this.ResetCamera();
    }

    public void SetAsCurrent(MyCameraBlock newCamera)
    {
      if (this.m_currentCamera == newCamera)
        return;
      if (newCamera.BlockDefinition.OverlayTexture != null)
      {
        MyHudCameraOverlay.TextureName = newCamera.BlockDefinition.OverlayTexture;
        MyHudCameraOverlay.Enabled = true;
      }
      else
        MyHudCameraOverlay.Enabled = false;
      string shipName = "";
      if (MyAntennaSystem.Static != null)
        shipName = MyAntennaSystem.Static.GetLogicalGroupRepresentative(this.Grid).DisplayName ?? "";
      string displayNameText = newCamera.DisplayNameText;
      MyHud.CameraInfo.Enable(shipName, displayNameText);
      this.m_currentCamera = newCamera;
      this.m_ignoreNextInput = true;
      MySessionComponentVoxelHand.Static.Enabled = false;
      MySession.Static.GameFocusManager.Clear();
      this.Schedule();
    }

    protected override void Update()
    {
      if (this.m_currentCamera == null)
        return;
      if (MySession.Static.CameraController != this.m_currentCamera)
      {
        if (!(MySession.Static.CameraController is MyCameraBlock))
          this.DisableCameraEffects();
        this.ResetCurrentCamera();
      }
      else if (this.m_ignoreNextInput)
      {
        this.m_ignoreNextInput = false;
      }
      else
      {
        if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SWITCH_LEFT) && MyScreenManager.GetScreenWithFocus() is MyGuiScreenGamePlay)
        {
          MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
          this.SetPrev();
        }
        if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SWITCH_RIGHT) && MyScreenManager.GetScreenWithFocus() is MyGuiScreenGamePlay)
        {
          MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
          this.SetNext();
        }
        if (MyInput.Static.DeltaMouseScrollWheelValue() != 0 && MyGuiScreenToolbarConfigBase.Static == null && !MyGuiScreenTerminal.IsOpen)
          this.m_currentCamera.ChangeZoom(MyInput.Static.DeltaMouseScrollWheelValue());
        Sandbox.Game.Entities.IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
        MyStringId context = controlledEntity != null ? controlledEntity.ControlContext : MyStringId.NullOrEmpty;
        if (MyControllerHelper.IsControl(context, MyControlsSpace.CAMERA_ZOOM_IN, MyControlStateType.PRESSED))
        {
          this.m_currentCamera.ChangeZoomPrecise(-MyGridCameraSystem.GAMEPAD_ZOOM_SPEED);
        }
        else
        {
          if (!MyControllerHelper.IsControl(context, MyControlsSpace.CAMERA_ZOOM_OUT, MyControlStateType.PRESSED))
            return;
          this.m_currentCamera.ChangeZoomPrecise(MyGridCameraSystem.GAMEPAD_ZOOM_SPEED);
        }
      }
    }

    public void UpdateBeforeSimulation10()
    {
      if (this.m_currentCamera == null || MyGridCameraSystem.CameraIsInRangeAndPlayerHasAccess(this.m_currentCamera))
        return;
      this.ResetCamera();
    }

    public static bool CameraIsInRangeAndPlayerHasAccess(MyCameraBlock camera)
    {
      MyIDModule component;
      if (MySession.Static.ControlledEntity != null && (!((IMyComponentOwner<MyIDModule>) camera).GetComponent(out component) || camera.HasPlayerAccess(MySession.Static.LocalPlayerId) || component.Owner == 0L))
      {
        if (MySession.Static.ControlledEntity is MyCharacter)
          return MyAntennaSystem.Static.CheckConnection((MyEntity) MySession.Static.LocalCharacter, (MyEntity) camera.CubeGrid, MySession.Static.LocalHumanPlayer);
        if (MySession.Static.ControlledEntity is MyShipController)
          return MyAntennaSystem.Static.CheckConnection((MyEntity) (MySession.Static.ControlledEntity as MyShipController).CubeGrid, (MyEntity) camera.CubeGrid, MySession.Static.LocalHumanPlayer);
        if (MySession.Static.ControlledEntity is MyCubeBlock)
          return MyAntennaSystem.Static.CheckConnection((MyEntity) (MySession.Static.ControlledEntity as MyCubeBlock).CubeGrid, (MyEntity) camera.CubeGrid, MySession.Static.LocalHumanPlayer);
      }
      return false;
    }

    public void ResetCamera()
    {
      this.ResetCurrentCamera();
      this.DisableCameraEffects();
      bool flag = false;
      if (MyGridCameraSystem.PreviousNonCameraBlockController != null)
      {
        if (MyGridCameraSystem.PreviousNonCameraBlockController is MyShipController cameraBlockController)
          cameraBlockController.RefreshControlNotifications();
        if (MyGridCameraSystem.PreviousNonCameraBlockController is MyEntity cameraBlockController && !cameraBlockController.Closed)
        {
          MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, (IMyEntity) cameraBlockController, new Vector3D?());
          MyGridCameraSystem.PreviousNonCameraBlockController = (IMyCameraController) null;
          flag = true;
        }
      }
      if (flag || MySession.Static.LocalCharacter == null)
        return;
      MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, (IMyEntity) MySession.Static.LocalCharacter, new Vector3D?());
    }

    private void DisableCameraEffects()
    {
      MyHudCameraOverlay.Enabled = false;
      MyHud.CameraInfo.Disable();
      MySector.MainCamera.FieldOfView = MySandboxGame.Config.FieldOfView;
    }

    public void ResetCurrentCamera()
    {
      if (this.m_currentCamera == null)
        return;
      this.m_currentCamera.OnExitView();
      this.m_currentCamera = (MyCameraBlock) null;
      this.DeSchedule();
    }

    private void SetNext()
    {
      this.UpdateRelayedCameras();
      MyCameraBlock next = this.GetNext(this.m_currentCamera);
      if (next == null)
        return;
      this.SetCamera(next);
    }

    private void SetPrev()
    {
      this.UpdateRelayedCameras();
      MyCameraBlock prev = this.GetPrev(this.m_currentCamera);
      if (prev == null)
        return;
      this.SetCamera(prev);
    }

    private void SetCamera(MyCameraBlock newCamera)
    {
      if (newCamera == this.m_currentCamera)
        return;
      if (this.m_cameras.Contains(newCamera))
      {
        this.SetAsCurrent(newCamera);
        newCamera.SetView();
      }
      else
      {
        MyHudCameraOverlay.Enabled = false;
        MyHud.CameraInfo.Disable();
        this.ResetCurrentCamera();
        newCamera.RequestSetView();
      }
    }

    private void UpdateRelayedCameras()
    {
      List<MyAntennaSystem.BroadcasterInfo> list = MyAntennaSystem.Static.GetConnectedGridsInfo((MyEntity) this.Grid).ToList<MyAntennaSystem.BroadcasterInfo>();
      list.Sort((Comparison<MyAntennaSystem.BroadcasterInfo>) ((b1, b2) => b1.EntityId.CompareTo(b2.EntityId)));
      this.m_relayedCameras.Clear();
      foreach (MyAntennaSystem.BroadcasterInfo broadcasterInfo in list)
        this.AddValidCamerasFromGridToRelayed(broadcasterInfo.EntityId);
      if (this.m_relayedCameras.Count != 0)
        return;
      this.AddValidCamerasFromGridToRelayed(this.Grid);
    }

    private void AddValidCamerasFromGridToRelayed(long gridId)
    {
      MyCubeGrid entity;
      Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyCubeGrid>(gridId, out entity);
      if (entity == null)
        return;
      this.AddValidCamerasFromGridToRelayed(entity);
    }

    private void AddValidCamerasFromGridToRelayed(MyCubeGrid grid)
    {
      foreach (MyTerminalBlock block in grid.GridSystems.TerminalSystem.Blocks)
      {
        if (block is MyCameraBlock myCameraBlock && myCameraBlock.IsWorking && myCameraBlock.HasLocalPlayerAccess())
          this.m_relayedCameras.Add(myCameraBlock);
      }
    }

    private MyCameraBlock GetNext(MyCameraBlock current)
    {
      if (this.m_relayedCameras.Count == 1)
        return current;
      int num = this.m_relayedCameras.IndexOf(current);
      if (num != -1)
        return this.m_relayedCameras[(num + 1) % this.m_relayedCameras.Count];
      this.ResetCamera();
      return (MyCameraBlock) null;
    }

    private MyCameraBlock GetPrev(MyCameraBlock current)
    {
      if (this.m_relayedCameras.Count == 1)
        return current;
      int num = this.m_relayedCameras.IndexOf(current);
      if (num == -1)
      {
        this.ResetCamera();
        return (MyCameraBlock) null;
      }
      int index = num - 1;
      if (index < 0)
        index = this.m_relayedCameras.Count - 1;
      return this.m_relayedCameras[index];
    }

    public override MyCubeGrid.UpdateQueue Queue => MyCubeGrid.UpdateQueue.BeforeSimulation;
  }
}
