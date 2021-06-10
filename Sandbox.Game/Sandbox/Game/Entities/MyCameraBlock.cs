// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyCameraBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Game.Utils;
using VRage.Input;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
  [MyCubeBlockType(typeof (MyObjectBuilder_CameraBlock))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyCameraBlock), typeof (Sandbox.ModAPI.Ingame.IMyCameraBlock)})]
  public class MyCameraBlock : MyFunctionalBlock, IMyCameraController, Sandbox.ModAPI.IMyCameraBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyCameraBlock
  {
    private const float MIN_FOV = 1E-05f;
    private const float MAX_FOV = 3.124139f;
    private int m_lastUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
    private double m_availableScanRange;
    private float m_fov;
    private float m_targetFov;
    private MyCameraBlock.RaycastInfo m_lastRay;
    private static MyHudNotification m_hudNotification;
    private bool m_requestActivateAfterLoad;
    private IMyCameraController m_previousCameraController;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_syncFov;

    public MyCameraBlockDefinition BlockDefinition => (MyCameraBlockDefinition) base.BlockDefinition;

    private double AvailableScanRange
    {
      get
      {
        if (this.IsWorking && this.EnableRaycast)
        {
          this.m_availableScanRange = Math.Min(double.MaxValue, this.m_availableScanRange + (double) (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastUpdateTime) * (double) this.BlockDefinition.RaycastTimeMultiplier);
          this.m_lastUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
        }
        return this.m_availableScanRange;
      }
      set => this.m_availableScanRange = value;
    }

    public bool EnableRaycast { get; set; }

    public bool IsActive { get; private set; }

    public bool IsInFirstPersonView
    {
      get => true;
      set
      {
      }
    }

    public bool ForceFirstPersonCamera { get; set; }

    public bool EnableFirstPersonView
    {
      get => true;
      set
      {
      }
    }

    public MyEntity Entity => (MyEntity) this;

    public MyCameraBlock()
    {
      this.CreateTerminalControls();
      this.m_syncFov.ValueChanged += (Action<SyncBase>) (x => this.OnSyncFov());
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyCameraBlock>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlButton<MyCameraBlock> button = new MyTerminalControlButton<MyCameraBlock>("View", MySpaceTexts.BlockActionTitle_View, MySpaceTexts.Blank, (Action<MyCameraBlock>) (b => b.RequestSetView()));
      button.Enabled = (Func<MyCameraBlock, bool>) (b => b.CanUse());
      button.SupportsMultipleBlocks = false;
      MyTerminalAction<MyCameraBlock> myTerminalAction = button.EnableAction<MyCameraBlock>(MyTerminalActionIcons.TOGGLE);
      if (myTerminalAction != null)
      {
        myTerminalAction.InvalidToolbarTypes = new List<MyToolbarType>()
        {
          MyToolbarType.ButtonPanel
        };
        myTerminalAction.ValidForGroups = false;
      }
      MyTerminalControlFactory.AddControl<MyCameraBlock>((MyTerminalControl<MyCameraBlock>) button);
      MyCameraBlock.m_hudNotification = new MyHudNotification(MySpaceTexts.NotificationHintPressToExitCamera, MyHudNotificationBase.INFINITE, level: MyNotificationLevel.Control);
    }

    public bool CanUse() => this.IsWorking && MyGridCameraSystem.CameraIsInRangeAndPlayerHasAccess(this);

    public void RequestSetView()
    {
      if (!this.IsWorking)
        return;
      string str = MyInput.Static.IsJoystickLastUsed ? MyControllerHelper.GetCodeForControl(MyControllerHelper.CX_BASE, MyControlsSpace.USE) : "[" + MyInput.Static.GetGameControl(MyControlsSpace.USE).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard) + "]";
      MyCameraBlock.m_hudNotification.SetTextFormatArguments((object) str);
      MyHud.Notifications.Remove((MyHudNotificationBase) MyCameraBlock.m_hudNotification);
      MyHud.Notifications.Add((MyHudNotificationBase) MyCameraBlock.m_hudNotification);
      this.CubeGrid.GridSystems.CameraSystem.SetAsCurrent(this);
      this.SetView();
      if (!MyGuiScreenTerminal.IsOpen)
        return;
      MyGuiScreenTerminal.Hide();
    }

    public void SetView()
    {
      if (MySession.Static.CameraController is MyCameraBlock cameraController)
        cameraController.IsActive = false;
      MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, (VRage.ModAPI.IMyEntity) this, new Vector3D?());
      MyHud.Crosshair.Recenter();
      MyCameraBlock.SetFov(this.m_fov);
      this.IsActive = true;
      this.CheckEmissiveState();
    }

    private static void SetFov(float fov)
    {
      fov = MathHelper.Clamp(fov, 1E-05f, 3.124139f);
      MySector.MainCamera.FieldOfView = fov;
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.SyncFlag = true;
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(MyStringHash.GetOrCompute(this.BlockDefinition.ResourceSinkGroup), this.BlockDefinition.RequiredPowerInput, new Func<float>(this.CalculateRequiredPowerInput));
      resourceSinkComponent.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
      resourceSinkComponent.RequiredInputChanged += new MyRequiredResourceChangeDelegate(this.Receiver_RequiredInputChanged);
      this.ResourceSink = resourceSinkComponent;
      base.Init(objectBuilder, cubeGrid);
      resourceSinkComponent.Update();
      MyObjectBuilder_CameraBlock builderCameraBlock = objectBuilder as MyObjectBuilder_CameraBlock;
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.IsWorkingChanged += new Action<MyCubeBlock>(this.MyCameraBlock_IsWorkingChanged);
      this.IsInFirstPersonView = true;
      if (builderCameraBlock.IsActive)
      {
        this.m_requestActivateAfterLoad = true;
        builderCameraBlock.IsActive = false;
      }
      this.OnChangeFov(builderCameraBlock.Fov);
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    private void MyCameraBlock_IsWorkingChanged(MyCubeBlock obj)
    {
      this.CubeGrid.GridSystems.CameraSystem.CheckCurrentCameraStillValid();
      if (!this.m_requestActivateAfterLoad || !this.IsWorking)
        return;
      this.m_requestActivateAfterLoad = false;
      this.RequestSetView();
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (this.CubeGrid.GridSystems.CameraSystem.CurrentCamera == this)
      {
        this.m_fov = MathHelper.Lerp(this.m_fov, this.m_targetFov, 0.5f);
        MyCameraBlock.SetFov(this.m_fov);
      }
      if ((double) Math.Abs(this.m_fov - this.m_targetFov) < 0.00999999977648258 && !MyDebugDrawSettings.ENABLE_DEBUG_DRAW && !this.HasDamageEffect)
        this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
      if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW || this.m_lastRay.Distance == 0.0)
        return;
      this.DrawDebug();
    }

    public override void UpdateAfterSimulation10()
    {
      base.UpdateAfterSimulation10();
      this.ResourceSink.Update();
    }

    private void DrawDebug()
    {
      MyRenderProxy.DebugDrawLine3D(this.m_lastRay.Start, this.m_lastRay.End, Color.Orange, Color.Orange, false);
      if (this.m_lastRay.Hit.HasValue)
        MyRenderProxy.DebugDrawSphere(this.m_lastRay.Hit.Value, 1f, Color.Orange, depthRead: false);
      double num = this.m_lastRay.Distance / Math.Cos((double) MathHelper.ToRadians(this.BlockDefinition.RaycastConeLimit));
      Vector3D[] vector3DArray = new Vector3D[4];
      MatrixD worldMatrix1 = this.WorldMatrix;
      Vector3D translation = worldMatrix1.Translation;
      worldMatrix1 = this.WorldMatrix;
      Vector3D forward = worldMatrix1.Forward;
      float radians1 = MathHelper.ToRadians(-this.BlockDefinition.RaycastConeLimit);
      float radians2 = MathHelper.ToRadians(-this.BlockDefinition.RaycastConeLimit);
      MatrixD worldMatrix2 = this.WorldMatrix;
      MatrixD fromAxisAngle1 = MatrixD.CreateFromAxisAngle(worldMatrix2.Right, (double) radians1);
      worldMatrix2 = this.WorldMatrix;
      MatrixD fromAxisAngle2 = MatrixD.CreateFromAxisAngle(worldMatrix2.Down, (double) radians2);
      Vector3D result1;
      Vector3D.RotateAndScale(ref forward, ref fromAxisAngle1, out result1);
      Vector3D result2;
      Vector3D.RotateAndScale(ref result1, ref fromAxisAngle2, out result2);
      vector3DArray[0] = translation + result2 * num;
      float radians3 = MathHelper.ToRadians(-this.BlockDefinition.RaycastConeLimit);
      float radians4 = MathHelper.ToRadians(this.BlockDefinition.RaycastConeLimit);
      worldMatrix2 = this.WorldMatrix;
      MatrixD fromAxisAngle3 = MatrixD.CreateFromAxisAngle(worldMatrix2.Right, (double) radians3);
      worldMatrix2 = this.WorldMatrix;
      MatrixD fromAxisAngle4 = MatrixD.CreateFromAxisAngle(worldMatrix2.Down, (double) radians4);
      Vector3D.RotateAndScale(ref forward, ref fromAxisAngle3, out result1);
      Vector3D.RotateAndScale(ref result1, ref fromAxisAngle4, out result2);
      vector3DArray[1] = translation + result2 * num;
      float radians5 = MathHelper.ToRadians(this.BlockDefinition.RaycastConeLimit);
      float radians6 = MathHelper.ToRadians(this.BlockDefinition.RaycastConeLimit);
      worldMatrix2 = this.WorldMatrix;
      MatrixD fromAxisAngle5 = MatrixD.CreateFromAxisAngle(worldMatrix2.Right, (double) radians5);
      worldMatrix2 = this.WorldMatrix;
      MatrixD fromAxisAngle6 = MatrixD.CreateFromAxisAngle(worldMatrix2.Down, (double) radians6);
      Vector3D.RotateAndScale(ref forward, ref fromAxisAngle5, out result1);
      Vector3D.RotateAndScale(ref result1, ref fromAxisAngle6, out result2);
      vector3DArray[2] = translation + result2 * num;
      float radians7 = MathHelper.ToRadians(this.BlockDefinition.RaycastConeLimit);
      float radians8 = MathHelper.ToRadians(-this.BlockDefinition.RaycastConeLimit);
      worldMatrix2 = this.WorldMatrix;
      MatrixD fromAxisAngle7 = MatrixD.CreateFromAxisAngle(worldMatrix2.Right, (double) radians7);
      worldMatrix2 = this.WorldMatrix;
      MatrixD fromAxisAngle8 = MatrixD.CreateFromAxisAngle(worldMatrix2.Down, (double) radians8);
      Vector3D.RotateAndScale(ref forward, ref fromAxisAngle7, out result1);
      Vector3D.RotateAndScale(ref result1, ref fromAxisAngle8, out result2);
      vector3DArray[3] = translation + result2 * num;
      MyRenderProxy.DebugDrawLine3D(translation, vector3DArray[0], Color.Blue, Color.Blue, false);
      MyRenderProxy.DebugDrawLine3D(translation, vector3DArray[1], Color.Blue, Color.Blue, false);
      MyRenderProxy.DebugDrawLine3D(translation, vector3DArray[2], Color.Blue, Color.Blue, false);
      MyRenderProxy.DebugDrawLine3D(translation, vector3DArray[3], Color.Blue, Color.Blue, false);
      MyRenderProxy.DebugDrawLine3D(vector3DArray[0], vector3DArray[1], Color.Blue, Color.Blue, false);
      MyRenderProxy.DebugDrawLine3D(vector3DArray[1], vector3DArray[2], Color.Blue, Color.Blue, false);
      MyRenderProxy.DebugDrawLine3D(vector3DArray[2], vector3DArray[3], Color.Blue, Color.Blue, false);
      MyRenderProxy.DebugDrawLine3D(vector3DArray[3], vector3DArray[0], Color.Blue, Color.Blue, false);
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      this.ResourceSink.Update();
    }

    public void OnExitView()
    {
      this.IsActive = false;
      this.m_syncFov.Value = this.m_fov;
      this.CheckEmissiveState();
      MyHud.Notifications.Remove((MyHudNotificationBase) MyCameraBlock.m_hudNotification);
    }

    public override bool SetEmissiveStateWorking() => this.IsActive ? this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Alternative, this.Render.RenderObjectIDs[0]) : base.SetEmissiveStateWorking();

    protected override void OnEnabledChanged()
    {
      this.ResourceSink.Update();
      base.OnEnabledChanged();
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_CameraBlock builderCubeBlock = (MyObjectBuilder_CameraBlock) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.IsActive = this.IsActive;
      builderCubeBlock.Fov = this.m_fov;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    private void ComponentStack_IsFunctionalChanged() => this.ResourceSink.Update();

    private void Receiver_IsPoweredChanged()
    {
      this.UpdateIsWorking();
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    protected override bool CheckIsWorking() => this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.CheckIsWorking();

    private void Receiver_RequiredInputChanged(
      MyDefinitionId resourceTypeId,
      MyResourceSinkComponent receiver,
      float oldRequirement,
      float newRequirement)
    {
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
    {
      base.UpdateDetailedInfo(detailedInfo);
      detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
      detailedInfo.Append(this.BlockDefinition.DisplayNameText);
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
      MyValueFormatter.AppendWorkInBestUnit(this.ResourceSink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId), detailedInfo);
    }

    private float CalculateRequiredPowerInput() => !this.EnableRaycast ? this.BlockDefinition.RequiredPowerInput : this.BlockDefinition.RequiredChargingInput;

    public override MatrixD GetViewMatrix()
    {
      MatrixD worldMatrix = this.WorldMatrix;
      worldMatrix.Translation += this.WorldMatrix.Forward * 0.200000002980232;
      MatrixD result;
      MatrixD.Invert(ref worldMatrix, out result);
      return result;
    }

    public void Rotate(Vector2 rotationIndicator, float rollIndicator)
    {
    }

    public void RotateStopped()
    {
    }

    public void OnAssumeControl(IMyCameraController previousCameraController)
    {
    }

    public void OnReleaseControl(IMyCameraController newCameraController)
    {
    }

    void IMyCameraController.ControlCamera(MyCamera currentCamera) => currentCamera.SetViewMatrix(this.GetViewMatrix());

    void IMyCameraController.Rotate(
      Vector2 rotationIndicator,
      float rollIndicator)
    {
      this.Rotate(rotationIndicator, rollIndicator);
    }

    void IMyCameraController.RotateStopped() => this.RotateStopped();

    void IMyCameraController.OnAssumeControl(
      IMyCameraController previousCameraController)
    {
      if (!(previousCameraController is MyCameraBlock))
        MyGridCameraSystem.PreviousNonCameraBlockController = previousCameraController;
      if (previousCameraController is MyShipController myShipController)
        myShipController.RemoveControlNotifications();
      this.OnAssumeControl(previousCameraController);
    }

    void IMyCameraController.OnReleaseControl(
      IMyCameraController newCameraController)
    {
      this.OnReleaseControl(newCameraController);
    }

    bool IMyCameraController.IsInFirstPersonView
    {
      get => this.IsInFirstPersonView;
      set => this.IsInFirstPersonView = value;
    }

    bool IMyCameraController.ForceFirstPersonCamera
    {
      get => this.ForceFirstPersonCamera;
      set => this.ForceFirstPersonCamera = value;
    }

    bool IMyCameraController.HandleUse()
    {
      MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
      this.CubeGrid.GridSystems.CameraSystem.ResetCamera();
      return true;
    }

    bool IMyCameraController.HandlePickUp() => false;

    bool IMyCameraController.AllowCubeBuilding => false;

    internal void ChangeZoom(int deltaZoom)
    {
      if (deltaZoom > 0)
      {
        this.m_targetFov -= 0.15f;
        if ((double) this.m_targetFov < (double) this.BlockDefinition.MinFov)
          this.m_targetFov = this.BlockDefinition.MinFov;
      }
      else
      {
        this.m_targetFov += 0.15f;
        if ((double) this.m_targetFov > (double) this.BlockDefinition.MaxFov)
          this.m_targetFov = this.BlockDefinition.MaxFov;
      }
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
      MyCameraBlock.SetFov(this.m_fov);
    }

    public void ChangeZoomPrecise(float deltaZoom)
    {
      this.m_targetFov += deltaZoom;
      if ((double) deltaZoom < 0.0)
      {
        if ((double) this.m_targetFov < (double) this.BlockDefinition.MinFov)
          this.m_targetFov = this.BlockDefinition.MinFov;
      }
      else if ((double) this.m_targetFov > (double) this.BlockDefinition.MaxFov)
        this.m_targetFov = this.BlockDefinition.MaxFov;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
      MyCameraBlock.SetFov(this.m_fov);
    }

    internal void OnChangeFov(float newFov)
    {
      this.m_fov = newFov;
      if ((double) this.m_fov > (double) this.BlockDefinition.MaxFov)
        this.m_fov = this.BlockDefinition.MaxFov;
      if ((double) this.m_fov < (double) this.BlockDefinition.MinFov)
        this.m_fov = this.BlockDefinition.MinFov;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
      this.m_targetFov = this.m_fov;
    }

    private void OnSyncFov()
    {
      if (this.IsActive)
        return;
      this.OnChangeFov((float) this.m_syncFov);
    }

    public bool CheckAngleLimits(Vector3D directionNormalized)
    {
      double azimuth;
      double elevation;
      Vector3D.GetAzimuthAndElevation(Vector3D.TransformNormal(directionNormalized, MatrixD.Transpose(this.WorldMatrix)), out azimuth, out elevation);
      double degrees = MathHelperD.ToDegrees(azimuth);
      return Math.Abs(MathHelperD.ToDegrees(elevation)) <= (double) this.BlockDefinition.RaycastConeLimit && Math.Abs(degrees) <= (double) this.BlockDefinition.RaycastConeLimit;
    }

    private Vector3D VectorProjection(Vector3D a, Vector3D b) => a.Dot(b) / b.LengthSquared() * b;

    MyDetectedEntityInfo Sandbox.ModAPI.Ingame.IMyCameraBlock.Raycast(
      double distance,
      Vector3D targetDirection)
    {
      targetDirection = !Vector3D.IsZero(targetDirection) ? Vector3D.TransformNormal(targetDirection, this.WorldMatrix) : throw new ArgumentOutOfRangeException(nameof (targetDirection), "Direction cannot be 0,0,0");
      targetDirection.Normalize();
      return this.CheckAngleLimits(targetDirection) ? this.Raycast(distance, targetDirection) : new MyDetectedEntityInfo();
    }

    MyDetectedEntityInfo Sandbox.ModAPI.Ingame.IMyCameraBlock.Raycast(
      Vector3D targetPos)
    {
      Vector3D vector3D = Vector3D.Normalize(targetPos - this.WorldMatrix.Translation);
      return this.CheckAngleLimits(vector3D) ? this.Raycast(Vector3D.Distance(targetPos, this.WorldMatrix.Translation), vector3D) : new MyDetectedEntityInfo();
    }

    MyDetectedEntityInfo Sandbox.ModAPI.Ingame.IMyCameraBlock.Raycast(
      double distance,
      float pitch,
      float yaw)
    {
      if ((double) Math.Abs(pitch) > (double) this.BlockDefinition.RaycastConeLimit || (double) Math.Abs(yaw) > (double) this.BlockDefinition.RaycastConeLimit)
        return new MyDetectedEntityInfo();
      pitch = MathHelper.ToRadians(pitch);
      yaw = MathHelper.ToRadians(yaw);
      Vector3D forward = this.WorldMatrix.Forward;
      MatrixD worldMatrix = this.WorldMatrix;
      MatrixD fromAxisAngle1 = MatrixD.CreateFromAxisAngle(worldMatrix.Right, (double) pitch);
      worldMatrix = this.WorldMatrix;
      MatrixD fromAxisAngle2 = MatrixD.CreateFromAxisAngle(worldMatrix.Down, (double) yaw);
      Vector3D result1;
      Vector3D.RotateAndScale(ref forward, ref fromAxisAngle1, out result1);
      Vector3D result2;
      Vector3D.RotateAndScale(ref result1, ref fromAxisAngle2, out result2);
      return this.Raycast(distance, result2);
    }

    public MyDetectedEntityInfo Raycast(double distance, Vector3D direction)
    {
      if (Vector3D.IsZero(direction))
        throw new ArgumentOutOfRangeException(nameof (direction), "Direction cannot be 0,0,0");
      if (distance <= 0.0 || this.BlockDefinition.RaycastDistanceLimit > -1.0 && distance > this.BlockDefinition.RaycastDistanceLimit)
        return new MyDetectedEntityInfo();
      if (this.AvailableScanRange < distance || !this.CheckIsWorking())
        return new MyDetectedEntityInfo();
      if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
        this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
      this.AvailableScanRange -= distance;
      Vector3D translation = this.WorldMatrix.Translation;
      Vector3D to = translation + direction * distance;
      List<MyPhysics.HitInfo> toList = new List<MyPhysics.HitInfo>();
      MyPhysics.CastRay(translation, to, toList);
      foreach (MyPhysics.HitInfo hitInfo in toList)
      {
        MyEntity hitEntity = (MyEntity) hitInfo.HkHitInfo.GetHitEntity();
        if (hitEntity != this)
        {
          this.m_lastRay = new MyCameraBlock.RaycastInfo()
          {
            Distance = distance,
            Start = translation,
            End = to,
            Hit = new Vector3D?(hitInfo.Position)
          };
          return MyDetectedEntityInfoHelper.Create(hitEntity, this.OwnerId, new Vector3D?(hitInfo.Position));
        }
      }
      LineD ray = new LineD(translation, to);
      List<MyLineSegmentOverlapResult<MyVoxelBase>> result1 = new List<MyLineSegmentOverlapResult<MyVoxelBase>>();
      MyGamePruningStructure.GetVoxelMapsOverlappingRay(ref ray, result1);
      foreach (MyLineSegmentOverlapResult<MyVoxelBase> segmentOverlapResult in result1)
      {
        if (segmentOverlapResult.Element is MyPlanet element)
        {
          double num = Vector3D.DistanceSquared(this.PositionComp.GetPosition(), element.PositionComp.GetPosition());
          MyGravityProviderComponent providerComponent = element.Components.Get<MyGravityProviderComponent>();
          if (providerComponent != null)
          {
            if (!providerComponent.IsPositionInRange(translation) && num > (double) element.MaximumRadius * (double) element.MaximumRadius)
            {
              double? nullable = new BoundingSphereD(element.PositionComp.GetPosition(), (double) element.MaximumRadius).Intersects(new RayD(translation, direction));
              if (nullable.HasValue && distance >= nullable.Value)
              {
                Vector3D vector3D = translation + direction * nullable.Value;
                this.m_lastRay = new MyCameraBlock.RaycastInfo()
                {
                  Distance = distance,
                  Start = translation,
                  End = to,
                  Hit = new Vector3D?(vector3D)
                };
                return MyDetectedEntityInfoHelper.Create((MyEntity) segmentOverlapResult.Element, this.OwnerId, new Vector3D?(vector3D));
              }
            }
            else if (element.RootVoxel.Storage != null)
            {
              Vector3 sizeInMetresHalf = element.SizeInMetresHalf;
              MatrixD matrix = element.PositionComp.WorldMatrixInvScaled;
              Line localLine = new Line((Vector3) (Vector3D.Transform(ray.From, matrix) + sizeInMetresHalf), (Vector3) (Vector3D.Transform(ray.To, matrix) + sizeInMetresHalf));
              MyIntersectionResultLineTriangle result2;
              if (element.RootVoxel.Storage.GetGeometry().Intersect(ref localLine, out result2, IntersectionFlags.DIRECT_TRIANGLES))
              {
                Vector3D vector3D = localLine.From + localLine.Direction * result2.Distance + element.PositionLeftBottomCorner;
                this.m_lastRay = new MyCameraBlock.RaycastInfo()
                {
                  Distance = distance,
                  Start = translation,
                  End = to,
                  Hit = new Vector3D?(vector3D)
                };
                return MyDetectedEntityInfoHelper.Create((MyEntity) segmentOverlapResult.Element, this.OwnerId, new Vector3D?(vector3D));
              }
            }
          }
        }
      }
      this.m_lastRay = new MyCameraBlock.RaycastInfo()
      {
        Distance = distance,
        Start = translation,
        End = to,
        Hit = new Vector3D?()
      };
      return new MyDetectedEntityInfo();
    }

    bool Sandbox.ModAPI.Ingame.IMyCameraBlock.CanScan(double distance)
    {
      if (this.BlockDefinition.RaycastDistanceLimit == -1.0)
        return distance <= this.AvailableScanRange;
      return distance <= this.AvailableScanRange && distance <= this.BlockDefinition.RaycastDistanceLimit;
    }

    bool Sandbox.ModAPI.Ingame.IMyCameraBlock.CanScan(
      double distance,
      Vector3D direction)
    {
      return this.BlockDefinition.RaycastDistanceLimit == -1.0 ? distance <= this.AvailableScanRange && this.CheckAngleLimits(Vector3D.Normalize(direction)) : distance <= this.AvailableScanRange && distance <= this.BlockDefinition.RaycastDistanceLimit && this.CheckAngleLimits(Vector3D.Normalize(direction));
    }

    bool Sandbox.ModAPI.Ingame.IMyCameraBlock.CanScan(Vector3D target)
    {
      Vector3D directionNormalized = Vector3D.Normalize(target - this.WorldMatrix.Translation);
      double num = Vector3D.Distance(target, this.WorldMatrix.Translation);
      return this.BlockDefinition.RaycastDistanceLimit == -1.0 ? num <= this.AvailableScanRange && this.CheckAngleLimits(directionNormalized) : num <= this.AvailableScanRange && num <= this.BlockDefinition.RaycastDistanceLimit && this.CheckAngleLimits(directionNormalized);
    }

    double Sandbox.ModAPI.Ingame.IMyCameraBlock.AvailableScanRange => this.AvailableScanRange;

    int Sandbox.ModAPI.Ingame.IMyCameraBlock.TimeUntilScan(double distance) => (int) Math.Max((distance - this.AvailableScanRange) / (double) this.BlockDefinition.RaycastTimeMultiplier, 0.0);

    bool Sandbox.ModAPI.Ingame.IMyCameraBlock.EnableRaycast
    {
      get => this.EnableRaycast;
      set
      {
        if (this.EnableRaycast != value)
          this.m_lastUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
        this.EnableRaycast = value;
        this.ResourceSink.Update();
      }
    }

    float Sandbox.ModAPI.Ingame.IMyCameraBlock.RaycastConeLimit => this.BlockDefinition.RaycastConeLimit;

    double Sandbox.ModAPI.Ingame.IMyCameraBlock.RaycastDistanceLimit => this.BlockDefinition.RaycastDistanceLimit;

    private struct RaycastInfo
    {
      public Vector3D Start;
      public Vector3D End;
      public Vector3D? Hit;
      public double Distance;
    }

    protected class m_syncFov\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyCameraBlock) obj0).m_syncFov = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_MyCameraBlock\u003C\u003EActor : IActivator, IActivator<MyCameraBlock>
    {
      object IActivator.CreateInstance() => (object) new MyCameraBlock();

      MyCameraBlock IActivator<MyCameraBlock>.CreateInstance() => new MyCameraBlock();
    }
  }
}
