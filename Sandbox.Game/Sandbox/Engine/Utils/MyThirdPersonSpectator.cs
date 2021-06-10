// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Utils.MyThirdPersonSpectator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Utils;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Utils
{
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
  public class MyThirdPersonSpectator : MySessionComponentBase
  {
    public static MyThirdPersonSpectator Static;
    public const float MIN_VIEWER_DISTANCE = 2.5f;
    public const float MAX_VIEWER_DISTANCE = 200f;
    public const float DEFAULT_CAMERA_RADIUS_CUBEGRID_MUL = 1.5f;
    public const float DEFAULT_CAMERA_RADIUS_CHARACTER = 0.125f;
    public const float CAMERA_MAX_RAYCAST_DISTANCE = 500f;
    private readonly Vector3D m_initialLookAtDirection = Vector3D.Normalize(new Vector3D(0.0, 5.0, 12.0));
    private readonly Vector3D m_initialLookAtDirectionCharacter = Vector3D.Normalize(new Vector3D(0.0, 0.0, 12.0));
    private const float m_lookAtOffsetY = 0.0f;
    private const double m_lookAtDefaultLength = 2.59999990463257;
    private int m_positionSafeZoomingOutDefaultTimeoutMs = 700;
    private bool m_disableSpringThisFrame;
    private float m_currentCameraRadius = 0.125f;
    private bool m_zoomingOutSmoothly;
    private const int SAFE_START_FILTER_FRAME_COUNT = 20;
    private readonly MyAverageFiltering m_safeStartSmoothing = new MyAverageFiltering(20);
    private float m_safeStartSmoothingFiltering = 1f;
    private Vector3D m_lookAt;
    private Vector3D m_clampedlookAt;
    private Vector3D m_transformedLookAt;
    private Vector3D m_target;
    private Vector3D m_lastTarget;
    private MatrixD m_targetOrientation = MatrixD.Identity;
    private Vector3D m_position;
    private Vector3D m_desiredPosition;
    private Vector3D m_positionSafe;
    private float m_positionSafeZoomingOutParam;
    private int m_positionSafeZoomingOutTimeout;
    private float m_lastRaycastDist = float.PositiveInfinity;
    private TimeSpan m_positionCurrentIsSafeSinceTime = TimeSpan.Zero;
    private const int POSITION_IS_SAFE_TIMEOUT_MS = 1000;
    public MyThirdPersonSpectator.SpringInfo NormalSpring;
    public MyThirdPersonSpectator.SpringInfo NormalSpringCharacter;
    private float m_springChangeTime;
    private MyThirdPersonSpectator.SpringInfo m_currentSpring;
    private Vector3 m_velocity;
    private float m_angleVelocity;
    private Quaternion m_orientation;
    private Matrix m_orientationMatrix;
    private readonly List<MyPhysics.HitInfo> m_raycastList = new List<MyPhysics.HitInfo>(64);
    private readonly List<HkBodyCollision> m_collisionList = new List<HkBodyCollision>(64);
    private readonly List<MyEntity> m_entityList = new List<MyEntity>();
    private readonly List<MyVoxelBase> m_voxelList = new List<MyVoxelBase>();
    private bool m_saveSettings;
    private bool m_debugDraw;
    private bool m_enableDebugDrawTrail;
    private double m_safeMinimumDistance = 2.5;
    private double m_safeMaximumDistance = 200.0;
    private float m_safeMaximumDistanceTimeout;
    private Sandbox.Game.Entities.IMyControllableEntity m_lastControllerEntity;
    private List<Vector3D> m_debugLastSpectatorPositions;
    private List<Vector3D> m_debugLastSpectatorDesiredPositions;
    private float m_lastZoomingOutSpeed;
    private BoundingBoxD m_safeAABB;

    public bool EnableDebugDraw
    {
      get => this.m_debugDraw;
      set => this.m_debugDraw = value;
    }

    public MyThirdPersonSpectator()
    {
      this.NormalSpring = new MyThirdPersonSpectator.SpringInfo(14000f, 2000f, 94f, 0.05f);
      this.NormalSpringCharacter = new MyThirdPersonSpectator.SpringInfo(30000f, 2500f, 40f, 0.2f);
      this.m_currentSpring = this.NormalSpring;
      this.m_lookAt = this.m_initialLookAtDirectionCharacter * 2.59999990463257;
      this.m_clampedlookAt = this.m_lookAt;
      this.m_saveSettings = false;
      this.ResetViewerDistance();
    }

    public override void LoadData()
    {
      MyThirdPersonSpectator.Static = this;
      base.LoadData();
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      MyThirdPersonSpectator.Static = (MyThirdPersonSpectator) null;
    }

    public void ResetSpring()
    {
      this.m_position = this.m_desiredPosition;
      this.m_disableSpringThisFrame = true;
    }

    public void Update()
    {
      genericControlledEntity = MySession.Static.ControlledEntity;
      if ((genericControlledEntity == null || genericControlledEntity.Entity == null) && (!(MySession.Static.CameraController is Sandbox.Game.Entities.IMyControllableEntity genericControlledEntity) || genericControlledEntity.Entity == null))
        return;
      MyEntity controlledEntity = MyThirdPersonSpectator.GetControlledEntity(genericControlledEntity);
      this.m_lastTarget = this.m_target;
      if (controlledEntity != null && controlledEntity.PositionComp != null)
      {
        MyPositionComponentBase positionComp = controlledEntity.PositionComp;
        BoundingBox localAabb1 = positionComp.LocalAABB;
        BoundingBox localAabb2 = positionComp.LocalAABB;
        MatrixD headMatrix = genericControlledEntity.GetHeadMatrix(true);
        MyCharacter myCharacter = controlledEntity as MyCharacter;
        this.m_target = headMatrix.Translation;
        if (myCharacter != null)
        {
          this.m_currentCameraRadius = 0.125f;
          this.m_currentSpring = this.NormalSpringCharacter;
        }
        else
        {
          this.m_currentSpring = this.NormalSpring;
          if (controlledEntity is MyTerminalBlock)
            this.m_currentCameraRadius = 0.5f;
          else if (controlledEntity is MyCubeGrid myCubeGrid)
            this.m_currentCameraRadius = myCubeGrid.GridSize;
        }
        if (myCharacter == null || !myCharacter.IsDead)
        {
          bool flag = !this.m_disableSpringThisFrame & !MyDX9Gui.LookaroundEnabled;
          this.m_targetOrientation = MatrixD.Lerp(this.m_targetOrientation, headMatrix.GetOrientation(), flag ? (double) this.m_currentSpring.RotationGain : 1.0);
        }
        this.m_transformedLookAt = Vector3D.Transform(this.m_clampedlookAt, this.m_targetOrientation);
        this.m_desiredPosition = this.m_target + this.m_transformedLookAt;
        this.m_position += this.m_target - this.m_lastTarget;
        this.m_positionSafe += this.m_target - this.m_lastTarget;
      }
      else
      {
        MatrixD headMatrix = genericControlledEntity.GetHeadMatrix(true);
        this.m_target = headMatrix.Translation;
        this.m_targetOrientation = headMatrix.GetOrientation();
        this.m_transformedLookAt = Vector3D.Transform(this.m_clampedlookAt, this.m_targetOrientation);
        this.m_position = this.m_desiredPosition;
      }
      if (genericControlledEntity != this.m_lastControllerEntity)
      {
        this.m_disableSpringThisFrame = true;
        this.m_lastTarget = this.m_target;
        this.m_lastControllerEntity = genericControlledEntity;
      }
      Vector3D position = this.m_position;
      if (this.m_disableSpringThisFrame)
      {
        this.m_position = this.m_desiredPosition;
        this.m_velocity = Vector3.Zero;
      }
      else
      {
        this.m_position = this.m_desiredPosition;
        this.m_velocity = Vector3.Zero;
      }
      if (controlledEntity != null)
      {
        if (!controlledEntity.Closed)
          this.HandleIntersection(controlledEntity, ref this.m_target);
        else
          this.m_positionCurrentIsSafeSinceTime = TimeSpan.MaxValue;
      }
      if (this.m_saveSettings)
      {
        MySession.Static.SaveControlledEntityCameraSettings(false);
        this.m_saveSettings = false;
      }
      if (this.m_disableSpringThisFrame)
      {
        double amount = 0.8;
        this.m_position = Vector3D.Lerp(position, this.m_desiredPosition, amount);
        this.m_velocity = Vector3.Zero;
        this.m_disableSpringThisFrame = Vector3D.DistanceSquared(this.m_position, this.m_desiredPosition) > (double) this.m_currentCameraRadius * (double) this.m_currentCameraRadius;
      }
      this.DebugDrawTrail();
    }

    private static MyEntity GetControlledEntity(
      Sandbox.Game.Entities.IMyControllableEntity genericControlledEntity)
    {
      if (genericControlledEntity == null)
        return (MyEntity) null;
      MyRemoteControl myRemoteControl = genericControlledEntity as MyRemoteControl;
      MyEntity myEntity = genericControlledEntity.Entity;
      if (myRemoteControl != null)
      {
        if (myRemoteControl.PreviousControlledEntity is MyEntity controlledEntity)
          myEntity = controlledEntity;
        else if (myRemoteControl.Pilot != null)
          myEntity = (MyEntity) myRemoteControl.Pilot;
      }
      while (myEntity != null && myEntity.Parent is MyCockpit)
        myEntity = myEntity.Parent;
      return myEntity;
    }

    private void ProcessSpringCalculation()
    {
      this.m_velocity += (Vector3) (-(double) this.m_currentSpring.Stiffness * (this.m_position - this.m_desiredPosition) - this.m_currentSpring.Dampening * this.m_velocity) / this.m_currentSpring.Mass * 0.01666667f;
      this.m_position += this.m_velocity * 0.01666667f;
    }

    public void Rotate(Vector2 rotationIndicator, float rollIndicator) => this.MoveAndRotate(Vector3.Zero, rotationIndicator, rollIndicator);

    public void MoveAndRotate(
      Vector3 moveIndicator,
      Vector2 rotationIndicator,
      float rollIndicator)
    {
      this.UpdateZoom();
    }

    private void SetPositionAndLookAt(Vector3D lookAt)
    {
      genericControlledEntity = MySession.Static.ControlledEntity;
      if (genericControlledEntity == null && !(MySession.Static.CameraController is Sandbox.Game.Entities.IMyControllableEntity genericControlledEntity))
        return;
      MyEntity controlledEntity = MyThirdPersonSpectator.GetControlledEntity(genericControlledEntity);
      MyPositionComponentBase positionComp = controlledEntity.PositionComp;
      BoundingBox localAabb1 = positionComp.LocalAABB;
      BoundingBox localAabb2 = positionComp.LocalAABB;
      this.m_target = genericControlledEntity.GetHeadMatrix(true).Translation;
      double num = lookAt.Length();
      this.m_lookAt = (MySession.Static == null || MySession.Static.ControlledEntity != null && !(MySession.Static.CameraController is MyCharacter) ? this.m_initialLookAtDirection : this.m_initialLookAtDirectionCharacter) * num;
      this.m_lastTarget = this.m_target;
      if (controlledEntity.Physics != null)
        this.m_target += controlledEntity.Physics.LinearVelocity * 60f;
      this.m_transformedLookAt = Vector3D.Transform(lookAt, this.m_targetOrientation);
      this.m_positionSafe = this.m_target + this.m_transformedLookAt;
      if (controlledEntity.Physics != null)
        this.m_positionSafe -= controlledEntity.Physics.LinearVelocity * 60f;
      this.m_desiredPosition = this.m_positionSafe;
      this.m_position = this.m_positionSafe;
      this.m_velocity = Vector3.Zero;
      this.m_disableSpringThisFrame = true;
      this.m_safeStartSmoothing.Clear();
      this.m_positionSafeZoomingOutParam = 0.0f;
      this.m_positionSafeZoomingOutTimeout = 0;
    }

    public MatrixD GetViewMatrix() => MySession.Static.CameraController == null ? MatrixD.Identity : MatrixD.CreateLookAt(this.m_positionSafe, this.m_target, this.m_targetOrientation.Up);

    public bool IsCameraForced() => this.m_positionCurrentIsSafeSinceTime == TimeSpan.MaxValue;

    public bool IsCameraForcedWithDelay() => this.IsCameraForced() || (MySession.Static.ElapsedGameTime - this.m_positionCurrentIsSafeSinceTime).TotalMilliseconds < 1000.0;

    private MyThirdPersonSpectator.MyCameraRaycastResult RaycastOccludingObjects(
      MyEntity controlledEntity,
      ref Vector3D raycastOrigin,
      ref Vector3D raycastEnd,
      ref Vector3D raycastSafeCameraStart,
      ref Vector3D outSafePosition)
    {
      Vector3D vector3D1 = raycastEnd - raycastOrigin;
      vector3D1.Normalize();
      double num1 = double.PositiveInfinity;
      bool flag1 = false;
      Vector3D? nullable = new Vector3D?();
      if (controlledEntity is MyCharacter)
      {
        MatrixD matrixD = controlledEntity.PositionComp.WorldMatrixRef;
        Vector3D translation = matrixD.Translation;
        Vector3D vector3D2 = translation + matrixD.Up * (double) controlledEntity.PositionComp.LocalAABB.Max.Y * 1.14999997615814;
        Vector3D vector3D3 = translation + matrixD.Up * (double) controlledEntity.PositionComp.LocalAABB.Max.Y * 0.850000023841858;
        this.m_raycastList.Clear();
        MyPhysics.CastRay(vector3D3, vector3D2, this.m_raycastList);
        if (this.m_debugDraw && MySession.Static.CameraController != controlledEntity)
          MyRenderProxy.DebugDrawLine3D(vector3D3, vector3D2, Color.Red, Color.Red, false);
        foreach (MyPhysics.HitInfo raycast in this.m_raycastList)
        {
          if (!this.IsEntityFiltered((MyEntity) raycast.HkHitInfo.GetHitEntity(), controlledEntity, raycast.HkHitInfo.Body, raycast.HkHitInfo.GetShapeKey(0)))
          {
            nullable = new Vector3D?(raycast.Position);
            num1 = 0.0;
            double num2 = 0.0 - (double) this.m_currentCameraRadius;
            outSafePosition = raycastOrigin + vector3D1 * num2;
            flag1 = true;
          }
        }
      }
      if (this.m_debugDraw && nullable.HasValue)
      {
        MatrixD viewMatrix = MySector.MainCamera.ViewMatrix;
        MyDebugDrawHelper.DrawNamedPoint(nullable.Value, "OCCLUDER", new Color?(Color.Red), new MatrixD?(viewMatrix));
        nullable = new Vector3D?();
      }
      bool flag2 = false;
      float num3 = 1f;
      this.m_collisionList.Clear();
      Vector3 halfExtents = new Vector3(this.m_currentCameraRadius, this.m_currentCameraRadius, this.m_currentCameraRadius) * 0.02f;
      Vector3D translation1 = raycastOrigin + (double) this.m_currentCameraRadius * vector3D1;
      MyPhysics.GetPenetrationsBox(ref halfExtents, ref translation1, ref Quaternion.Identity, this.m_collisionList, 15);
      if (this.EnableDebugDraw)
        MyRenderProxy.DebugDrawAABB(new BoundingBoxD(translation1 - halfExtents, translation1 + halfExtents), Color.Red);
      foreach (HkBodyCollision collision in this.m_collisionList)
      {
        if (!this.IsEntityFiltered((MyEntity) collision.GetCollisionEntity(), controlledEntity, collision.Body, collision.ShapeKey))
          flag2 = true;
      }
      BoundingBoxD box = new BoundingBoxD(translation1 - 9.99999974737875E-06, translation1 + 1E-05f);
      MyGamePruningStructure.GetAllVoxelMapsInBox(ref box, this.m_voxelList);
      foreach (MyVoxelBase voxel in this.m_voxelList)
      {
        if (voxel.Physics != null && voxel.IsAnyAabbCornerInside(ref MatrixD.Identity, box))
        {
          flag2 = true;
          break;
        }
      }
      this.m_voxelList.Clear();
      if ((raycastEnd - raycastOrigin).LengthSquared() > (double) this.m_currentCameraRadius * (double) this.m_currentCameraRadius)
      {
        HkShape shape = (HkShape) new HkSphereShape(this.m_currentCameraRadius);
        MatrixD identity = MatrixD.Identity;
        identity.Translation = controlledEntity is MyCharacter ? raycastOrigin : raycastOrigin + (double) this.m_currentCameraRadius * vector3D1;
        double num2 = (identity.Translation - this.m_target).LengthSquared();
        this.m_raycastList.Clear();
        uint collisionFilter = 0;
        if (controlledEntity.GetTopMostParent((System.Type) null).Physics is MyPhysicsBody physics)
          collisionFilter = HkGroupFilter.CalcFilterInfo(0, physics.HavokCollisionSystemID, 1, 1);
        MyPhysics.CastShapeReturnContactBodyDatas(raycastEnd, shape, ref identity, collisionFilter, 0.0f, this.m_raycastList);
        if (this.EnableDebugDraw)
        {
          MyRenderProxy.DebugDrawLine3D(identity.Translation, raycastEnd, Color.Red, Color.Red, true);
          MyDebugDrawHelper.DrawNamedPoint(identity.Translation, "RAY_START");
          MyDebugDrawHelper.DrawNamedPoint(raycastEnd, "RAY_END");
        }
        foreach (MyPhysics.HitInfo raycast in this.m_raycastList)
        {
          if ((double) Vector3.Dot(raycast.HkHitInfo.Normal, (Vector3) vector3D1) <= 0.0 && !this.IsEntityFiltered((MyEntity) raycast.HkHitInfo.GetHitEntity(), controlledEntity, raycast.HkHitInfo.Body, raycast.HkHitInfo.GetShapeKey(0)))
          {
            double num4 = (raycast.Position - this.m_target).LengthSquared();
            if (num2 <= num4)
            {
              float hitFraction = raycast.HkHitInfo.HitFraction;
              Vector3D vector3D2 = Vector3D.Lerp(identity.Translation, raycastEnd, Math.Max((double) hitFraction, 0.0001));
              double num5 = Vector3D.DistanceSquared(identity.Translation, vector3D2);
              if ((double) hitFraction < (double) num3 && num5 < num1)
              {
                nullable = new Vector3D?(raycast.Position);
                outSafePosition = vector3D2;
                num1 = num5;
                flag1 = true;
                num3 = hitFraction;
              }
            }
          }
        }
        shape.RemoveReference();
      }
      if (flag2 && !flag1)
      {
        nullable = new Vector3D?(raycastOrigin);
        outSafePosition = raycastOrigin;
        num1 = 0.0;
        flag1 = true;
      }
      if (this.m_debugDraw && nullable.HasValue)
      {
        MatrixD viewMatrix = MySector.MainCamera.ViewMatrix;
        MyDebugDrawHelper.DrawNamedPoint(nullable.Value, "OCCLUDER", new Color?(Color.Red), new MatrixD?(viewMatrix));
        nullable = new Vector3D?();
      }
      if (controlledEntity is MyCharacter)
      {
        float currentCameraRadius = this.m_currentCameraRadius;
        if (num1 < (double) currentCameraRadius * (double) currentCameraRadius)
        {
          this.m_positionCurrentIsSafeSinceTime = TimeSpan.MaxValue;
          return MyThirdPersonSpectator.MyCameraRaycastResult.FoundOccluderNoSpace;
        }
      }
      else
      {
        float num2 = this.m_currentCameraRadius + (this.IsCameraForced() ? 2.5f : 0.025f);
        if (num1 < (double) num2 * (double) num2)
        {
          this.m_positionCurrentIsSafeSinceTime = TimeSpan.MaxValue;
          return MyThirdPersonSpectator.MyCameraRaycastResult.FoundOccluderNoSpace;
        }
      }
      if (!flag1)
      {
        MyPhysics.CastRay(raycastOrigin, raycastSafeCameraStart, this.m_raycastList);
        foreach (MyPhysics.HitInfo raycast in this.m_raycastList)
        {
          if (raycast.HkHitInfo.GetHitEntity() is MyVoxelBase)
          {
            this.m_positionCurrentIsSafeSinceTime = TimeSpan.MaxValue;
            return MyThirdPersonSpectator.MyCameraRaycastResult.FoundOccluderNoSpace;
          }
        }
      }
      return !flag1 ? MyThirdPersonSpectator.MyCameraRaycastResult.Ok : MyThirdPersonSpectator.MyCameraRaycastResult.FoundOccluder;
    }

    private bool IsEntityFiltered(
      MyEntity hitEntity,
      MyEntity controlledEntity,
      HkRigidBody hitRigidBody,
      uint shapeKey)
    {
      if ((HkReferenceObject) hitRigidBody == (HkReferenceObject) null || hitRigidBody.UserObject == null || (hitEntity == controlledEntity || !(hitRigidBody.UserObject is MyPhysicsBody)) || (hitRigidBody.UserObject as MyPhysicsBody).IsPhantom)
        return true;
      if (shapeKey != uint.MaxValue && hitEntity is MyCubeGrid)
      {
        MySlimBlock blockFromShapeKey = ((MyCubeGrid) hitEntity).Physics.Shape.GetBlockFromShapeKey(shapeKey);
        if (blockFromShapeKey != null && blockFromShapeKey.FatBlock is MyLadder fatBlock)
          hitEntity = (MyEntity) fatBlock;
      }
      if (hitEntity is IMyHandheldGunObject<MyDeviceBase> || hitEntity is MyFloatingObject || (hitEntity is MyDebrisBase || hitEntity is MyCharacter))
        return true;
      MyEntity myEntity1 = hitEntity.GetTopMostParent((System.Type) null) ?? hitEntity;
      MyEntity myEntity2 = controlledEntity.GetTopMostParent((System.Type) null) ?? controlledEntity;
      if (myEntity1 == myEntity2)
        return true;
      MyCubeGrid first = myEntity2 as MyCubeGrid;
      MyCubeGrid second = myEntity1 as MyCubeGrid;
      return first != null && second != null && MyGridPhysicalHierarchy.Static.InSameHierarchy(first, second) || controlledEntity is MyCharacter myCharacter && myCharacter.Ladder != null && (hitEntity is MyLadder && myCharacter.Ladder.GetTopMostParent((System.Type) null) == hitEntity.GetTopMostParent((System.Type) null));
    }

    public void ResetInternalTimers() => this.m_positionCurrentIsSafeSinceTime = TimeSpan.Zero;

    private void HandleIntersection(MyEntity controlledEntity, ref Vector3D lastTargetPos)
    {
      MyEntity controlledEntity1 = controlledEntity.GetTopMostParent((System.Type) null) ?? controlledEntity;
      if (controlledEntity1 is MyCubeGrid myCubeGrid && myCubeGrid.IsStatic)
        controlledEntity1 = controlledEntity;
      Vector3D target = this.m_target;
      Vector3D raycastEnd = this.m_position;
      double d = (raycastEnd - this.m_target).LengthSquared();
      if (d > 0.0)
      {
        double num = this.m_lookAt.Length() / Math.Sqrt(d);
        raycastEnd = this.m_target + (raycastEnd - this.m_target) * num;
      }
      LineD line = new LineD(target, raycastEnd);
      if (line.Length > 500.0)
        return;
      MyOrientedBoundingBoxD completeSafeObb = this.ComputeCompleteSafeOBB(controlledEntity1);
      MyOrientedBoundingBoxD safeObbWithCollisionExtents = new MyOrientedBoundingBoxD(completeSafeObb.Center, completeSafeObb.HalfExtent + 2f * this.m_currentCameraRadius, completeSafeObb.Orientation);
      Vector3D castStartSafe;
      LineD safeOBBLine;
      MyThirdPersonSpectator.MyCameraRaycastResult cameraRaycastResult = this.FindSafeStart(controlledEntity, line, ref completeSafeObb, ref safeObbWithCollisionExtents, out castStartSafe, out safeOBBLine);
      Vector3D vector3D1;
      if (controlledEntity is MyCharacter)
      {
        this.m_safeMinimumDistance = (double) this.m_currentCameraRadius;
      }
      else
      {
        vector3D1 = castStartSafe - target;
        this.m_safeMinimumDistance = vector3D1.Length();
        this.m_safeMinimumDistance = Math.Max(this.m_safeMinimumDistance, 2.5);
      }
      Vector3D raycastOrigin = controlledEntity is MyCharacter ? target : castStartSafe;
      Vector3D vector3D2 = raycastEnd;
      if (cameraRaycastResult == MyThirdPersonSpectator.MyCameraRaycastResult.Ok)
      {
        vector3D1 = raycastEnd - raycastOrigin;
        if (vector3D1.LengthSquared() < this.m_safeMinimumDistance * this.m_safeMinimumDistance)
          raycastEnd = raycastOrigin + Vector3D.Normalize(raycastEnd - raycastOrigin) * this.m_safeMinimumDistance;
        cameraRaycastResult = this.RaycastOccludingObjects(controlledEntity, ref raycastOrigin, ref raycastEnd, ref castStartSafe, ref vector3D2);
        if ((double) this.m_safeMaximumDistanceTimeout >= 0.0)
          this.m_safeMaximumDistanceTimeout -= 16.66667f;
        switch (cameraRaycastResult)
        {
          case MyThirdPersonSpectator.MyCameraRaycastResult.Ok:
            if ((double) this.m_safeMaximumDistanceTimeout <= 0.0)
            {
              this.m_safeMaximumDistance = 200.0;
              break;
            }
            break;
          case MyThirdPersonSpectator.MyCameraRaycastResult.FoundOccluder:
            vector3D1 = vector3D2 - target;
            double val2 = vector3D1.Length();
            double num = val2 + (double) this.m_currentCameraRadius;
            if ((double) this.m_safeMaximumDistanceTimeout <= 0.0 || num < this.m_safeMaximumDistance)
              this.m_safeMaximumDistance = num;
            if (val2 < this.m_safeMaximumDistance + (double) this.m_currentCameraRadius && !this.IsCameraForcedWithDelay())
              this.m_safeMaximumDistanceTimeout = (float) this.m_positionSafeZoomingOutDefaultTimeoutMs;
            this.m_safeMinimumDistance = Math.Min(this.m_safeMinimumDistance, val2);
            if (controlledEntity is MyCharacter && safeObbWithCollisionExtents.Contains(ref vector3D2))
            {
              cameraRaycastResult = MyThirdPersonSpectator.MyCameraRaycastResult.FoundOccluderNoSpace;
              break;
            }
            break;
        }
      }
      if (this.IsCameraForced())
        this.m_positionSafe = this.m_target;
      switch (cameraRaycastResult)
      {
        case MyThirdPersonSpectator.MyCameraRaycastResult.Ok:
        case MyThirdPersonSpectator.MyCameraRaycastResult.FoundOccluder:
          bool flag = false;
          if (this.m_positionCurrentIsSafeSinceTime == TimeSpan.MaxValue)
          {
            this.ResetInternalTimers();
            this.ResetSpring();
            flag = true;
            this.m_safeMaximumDistanceTimeout = 0.0f;
          }
          this.PerformZoomInOut(castStartSafe, vector3D2);
          if (flag)
            this.m_positionCurrentIsSafeSinceTime = MySession.Static.ElapsedGameTime;
          this.PerformZoomInOut(castStartSafe, vector3D2);
          if (this.m_positionCurrentIsSafeSinceTime == TimeSpan.MaxValue)
          {
            this.m_positionCurrentIsSafeSinceTime = MySession.Static.ElapsedGameTime;
            break;
          }
          break;
        default:
          this.m_positionSafeZoomingOutParam = 1f;
          this.m_positionCurrentIsSafeSinceTime = TimeSpan.MaxValue;
          this.m_lastRaycastDist = 0.0f;
          this.m_zoomingOutSmoothly = true;
          this.m_positionSafeZoomingOutTimeout = 0;
          break;
      }
      if (this.IsCameraForced())
        this.m_positionSafe = this.m_target;
      if (!this.m_debugDraw)
        return;
      MyRenderProxy.DebugDrawArrow3D(safeOBBLine.From, safeOBBLine.To, Color.White, new Color?(Color.Purple), tipScale: 0.02);
      MyRenderProxy.DebugDrawArrow3D(safeOBBLine.From, castStartSafe, Color.White, new Color?(Color.Red), tipScale: 0.02);
      MatrixD viewMatrix = MySector.MainCamera.ViewMatrix;
      MyDebugDrawHelper.DrawNamedPoint(this.m_position, "mpos", new Color?(Color.Gray), new MatrixD?(viewMatrix));
      MyDebugDrawHelper.DrawNamedPoint(this.m_target, "target", new Color?(Color.Purple), new MatrixD?(viewMatrix));
      MyDebugDrawHelper.DrawNamedPoint(castStartSafe, "safeStart", new Color?(Color.Lime), new MatrixD?(viewMatrix));
      MyDebugDrawHelper.DrawNamedPoint(vector3D2, "safePosCand", new Color?(Color.Pink), new MatrixD?(viewMatrix));
      MyDebugDrawHelper.DrawNamedPoint(this.m_positionSafe, "posSafe", new Color?(Color.White), new MatrixD?(viewMatrix));
      MyRenderProxy.DebugDrawOBB(safeObbWithCollisionExtents, Color.Olive, 0.0f, false, true);
      MyRenderProxy.DebugDrawOBB(completeSafeObb, Color.OliveDrab, 0.0f, false, true);
      MyRenderProxy.DebugDrawText3D(safeObbWithCollisionExtents.Center - Vector3D.Transform(safeObbWithCollisionExtents.HalfExtent, safeObbWithCollisionExtents.Orientation), "safeObb", Color.Olive, 0.5f, false);
      MyRenderProxy.DebugDrawText2D(new Vector2(30f, 30f), cameraRaycastResult.ToString(), Color.Azure, 1f);
      MyRenderProxy.DebugDrawText2D(new Vector2(30f, 50f), this.IsCameraForcedWithDelay() ? (this.IsCameraForced() ? "Forced" : "ForcedDelay") : "Unforced", Color.Azure, 1f);
      MyRenderProxy.DebugDrawText2D(new Vector2(30f, 70f), this.m_zoomingOutSmoothly ? "zooming out" : "ready", Color.Azure, 1f);
      MyRenderProxy.DebugDrawText2D(new Vector2(30f, 90f), "v=" + this.m_velocity.Length().ToString("0.00"), Color.Azure, 1f);
      MyRenderProxy.DebugDrawText2D(new Vector2(30f, 110f), "maxsafedist=" + (object) this.m_safeMaximumDistance, Color.Azure, 1f);
      MyRenderProxy.DebugDrawText2D(new Vector2(30f, 130f), "maxsafedisttimeout=" + (1f / 1000f * this.m_safeMaximumDistanceTimeout).ToString("0.0"), (double) this.m_safeMaximumDistanceTimeout <= 0.0 ? Color.Red : Color.Azure, 1f);
    }

    private MyThirdPersonSpectator.MyCameraRaycastResult FindSafeStart(
      MyEntity controlledEntity,
      LineD line,
      ref MyOrientedBoundingBoxD safeObb,
      ref MyOrientedBoundingBoxD safeObbWithCollisionExtents,
      out Vector3D castStartSafe,
      out LineD safeOBBLine)
    {
      safeOBBLine = new LineD(safeObbWithCollisionExtents.Center, line.From + line.Direction * 2.0 * safeObbWithCollisionExtents.HalfExtent.Length());
      double? nullable1 = safeObbWithCollisionExtents.Intersects(ref safeOBBLine);
      castStartSafe = nullable1.HasValue ? safeOBBLine.From + safeOBBLine.Direction * nullable1.Value : line.From;
      MyThirdPersonSpectator.MyCameraRaycastResult cameraRaycastResult = MyThirdPersonSpectator.MyCameraRaycastResult.Ok;
      if (nullable1.HasValue)
      {
        this.m_raycastList.Clear();
        MatrixD translation = MatrixD.CreateTranslation(castStartSafe);
        HkShape shape = (HkShape) new HkSphereShape(this.m_currentCameraRadius);
        uint collisionFilter = 0;
        if (controlledEntity.GetTopMostParent((System.Type) null).Physics is MyPhysicsBody physics)
          collisionFilter = HkGroupFilter.CalcFilterInfo(0, physics.HavokCollisionSystemID, 1, 1);
        MyPhysics.CastShapeReturnContactBodyDatas(line.From, shape, ref translation, collisionFilter, 0.0f, this.m_raycastList);
        if (this.EnableDebugDraw)
        {
          MyDebugDrawHelper.DrawDashedLine(castStartSafe + 0.1f * Vector3.Up, line.From + 0.1f * Vector3.Up, Color.Red);
          MyRenderProxy.DebugDrawSphere(castStartSafe, this.m_currentCameraRadius, Color.Red);
        }
        MyPhysics.HitInfo? nullable2 = new MyPhysics.HitInfo?();
        foreach (MyPhysics.HitInfo raycast in this.m_raycastList)
        {
          if (!this.IsEntityFiltered(raycast.HkHitInfo.GetHitEntity() as MyEntity, controlledEntity, raycast.HkHitInfo.Body, raycast.HkHitInfo.GetShapeKey(0)))
          {
            nullable2 = new MyPhysics.HitInfo?(raycast);
            break;
          }
        }
        if (!nullable2.HasValue)
        {
          this.m_collisionList.Clear();
          MyPhysics.GetPenetrationsShape(shape, ref castStartSafe, ref Quaternion.Identity, this.m_collisionList, 15);
          foreach (HkBodyCollision collision in this.m_collisionList)
          {
            if (!this.IsEntityFiltered(collision.GetCollisionEntity() as MyEntity, controlledEntity, collision.Body, collision.ShapeKey))
            {
              cameraRaycastResult = MyThirdPersonSpectator.MyCameraRaycastResult.FoundOccluderNoSpace;
              break;
            }
          }
        }
        shape.RemoveReference();
        if (nullable2.HasValue)
        {
          castStartSafe += ((double) nullable2.Value.HkHitInfo.HitFraction - (double) this.m_currentCameraRadius / safeOBBLine.Length) * (line.From - castStartSafe);
          cameraRaycastResult = MyThirdPersonSpectator.MyCameraRaycastResult.FoundOccluderNoSpace;
        }
        Vector3D vector3D = line.To - line.From;
        double num1 = vector3D.LengthSquared();
        vector3D = castStartSafe - line.From;
        double d = vector3D.LengthSquared();
        double num2 = Math.Sqrt(d) - (double) this.m_currentCameraRadius * 0.00999999977648258;
        double num3 = this.m_safeStartSmoothing.Get();
        if (num3 > d)
        {
          this.m_safeStartSmoothingFiltering = Math.Max(this.m_safeStartSmoothingFiltering - 0.025f, 0.0f);
          double num4 = MathHelper.Lerp(d, num3, (double) this.m_safeStartSmoothingFiltering);
          double num5 = Math.Sqrt(num4 / d);
          castStartSafe = line.From + (castStartSafe - line.From) * num5;
          d = num4;
          num2 = Math.Sqrt(d);
        }
        else
          this.m_safeStartSmoothingFiltering = Math.Min(this.m_safeStartSmoothingFiltering + 0.05f, 1f);
        this.m_safeStartSmoothing.Add(d);
        if (num1 < num2 * num2)
        {
          this.m_position = castStartSafe;
          this.m_positionSafe = castStartSafe;
          this.m_positionSafeZoomingOutTimeout = 0;
        }
        if (num1 * 2.0 < d)
          this.m_disableSpringThisFrame = true;
      }
      return cameraRaycastResult;
    }

    private void PerformZoomInOut(Vector3D safeCastStart, Vector3D safePositionCandidate)
    {
      double num1 = Math.Min((safePositionCandidate - this.m_target).Length(), this.m_safeMaximumDistance);
      double num2 = (this.m_positionSafe - this.m_target).Length();
      if (this.m_disableSpringThisFrame)
      {
        this.m_lastRaycastDist = (float) num1;
        this.m_zoomingOutSmoothly = false;
      }
      if (this.IsCameraForced())
      {
        this.m_positionSafeZoomingOutTimeout = 0;
        this.m_positionSafeZoomingOutParam = 0.0f;
        num2 = 0.0;
        this.m_zoomingOutSmoothly = true;
        this.m_desiredPosition = safeCastStart;
        this.m_position = safeCastStart;
      }
      if (!this.m_disableSpringThisFrame && num1 > num2 + (double) this.m_currentCameraRadius)
      {
        this.m_zoomingOutSmoothly = true;
        if ((double) this.m_lastZoomingOutSpeed <= 9.99999974737875E-06 && this.m_positionSafeZoomingOutTimeout <= -this.m_positionSafeZoomingOutDefaultTimeoutMs)
          this.m_positionSafeZoomingOutTimeout = this.m_positionSafeZoomingOutDefaultTimeoutMs;
      }
      else if (num1 < num2 + (double) this.m_currentCameraRadius && Math.Abs(this.m_lookAt.LengthSquared() - num1 * num1) <= 9.99999974737875E-06)
        this.m_zoomingOutSmoothly = false;
      if (this.m_zoomingOutSmoothly)
      {
        this.m_positionSafeZoomingOutTimeout -= 16;
        if (this.m_positionSafeZoomingOutTimeout <= 0 && !this.IsCameraForced())
        {
          float num3 = 1f - MathHelper.Clamp(this.m_lastRaycastDist - (float) Math.Min((safePositionCandidate - this.m_target).Length(), this.m_safeMaximumDistance), 0.95f, 1f);
          this.m_lastZoomingOutSpeed = Math.Abs(num3);
          this.m_positionSafeZoomingOutParam += num3;
          this.m_positionSafeZoomingOutParam = MathHelper.Clamp(this.m_positionSafeZoomingOutParam, 0.0f, 1f);
          double num4 = Math.Min((this.m_positionSafe - this.m_target).Length(), this.m_safeMaximumDistance);
          this.m_positionSafe = Vector3D.Lerp(this.m_target + Vector3D.Normalize(safePositionCandidate - this.m_target) * num4, safePositionCandidate, (double) this.m_positionSafeZoomingOutParam);
          this.m_lastRaycastDist = (float) num1;
        }
        else
        {
          this.m_lastZoomingOutSpeed = 0.0f;
          double num3 = (this.m_positionSafe - this.m_target).Length();
          if (this.IsCameraForced())
            return;
          this.m_positionSafe = this.m_target + Vector3D.Normalize(safePositionCandidate - this.m_target) * num3;
        }
      }
      else
      {
        if (!this.m_disableSpringThisFrame)
        {
          this.m_positionSafeZoomingOutParam = 0.0f;
          this.m_lastZoomingOutSpeed = 0.0f;
          this.m_positionSafeZoomingOutTimeout = this.m_positionSafeZoomingOutDefaultTimeoutMs;
        }
        if (!this.IsCameraForced())
        {
          double num3 = Math.Max(Math.Min((safePositionCandidate - this.m_target).Length(), this.m_safeMaximumDistance), this.m_safeMinimumDistance);
          this.m_positionSafe = this.m_target + Vector3D.Normalize(safePositionCandidate - this.m_target) * num3;
        }
        this.m_lastRaycastDist = (float) num1;
      }
    }

    private void MergeAABB(MyCubeGrid grid) => this.m_safeAABB.Include(grid.PositionComp.WorldAABB);

    private MyOrientedBoundingBoxD ComputeEntitySafeOBB(
      MyEntity controlledEntity)
    {
      MatrixD matrix = controlledEntity.WorldMatrix;
      BoundingBox localAabb = controlledEntity.PositionComp.LocalAABB;
      MyOrientedBoundingBoxD orientedBoundingBoxD = new MyOrientedBoundingBoxD(Vector3D.Transform((Vector3D) localAabb.Center, matrix), (Vector3D) localAabb.HalfExtents, Quaternion.CreateFromRotationMatrix(in matrix));
      if (controlledEntity.GetTopMostParent((System.Type) null) is MyCubeGrid topMostParent)
      {
        this.m_safeAABB = topMostParent.PositionComp.WorldAABB;
        MyGridPhysicalHierarchy.Static.ApplyOnChildren(topMostParent, new Action<MyCubeGrid>(this.MergeAABB));
        orientedBoundingBoxD = MyOrientedBoundingBoxD.CreateFromBoundingBox(this.m_safeAABB);
      }
      return orientedBoundingBoxD;
    }

    private MyOrientedBoundingBoxD ComputeCompleteSafeOBB(
      MyEntity controlledEntity)
    {
      return !(controlledEntity.GetTopMostParent((System.Type) null) is MyCubeGrid topMostParent) ? this.ComputeEntitySafeOBB(controlledEntity) : this.ComputeEntitySafeOBB((MyEntity) MyGridPhysicalHierarchy.Static.GetRoot(topMostParent));
    }

    public void RecalibrateCameraPosition(bool isCharacter = false)
    {
      IMyCameraController cameraController = MySession.Static.CameraController;
      if (!(cameraController is MyEntity) || !(MyThirdPersonSpectator.GetControlledEntity(MySession.Static.ControlledEntity) is Sandbox.Game.Entities.IMyControllableEntity controlledEntity))
        return;
      if (!isCharacter)
      {
        MatrixD headMatrix = controlledEntity.GetHeadMatrix(true);
        this.m_targetOrientation = headMatrix.GetOrientation();
        this.m_target = headMatrix.Translation;
      }
      MyEntity topMostParent = ((MyEntity) cameraController).GetTopMostParent((System.Type) null);
      if (topMostParent.Closed)
        return;
      MatrixD matrix = topMostParent.PositionComp.WorldMatrixNormalizedInv;
      Vector3D vector3D = Vector3D.Transform(this.m_target, matrix);
      MatrixD matrixD = this.m_targetOrientation * matrix;
      BoundingBox localAabb = topMostParent.PositionComp.LocalAABB;
      localAabb.Inflate(1.2f);
      Vector3 center = localAabb.Center;
      Vector3D vector1 = vector3D - center;
      Vector3D vector2_1 = Vector3D.Normalize(matrixD.Backward);
      Vector3D vector2_2 = vector2_1;
      double num1 = Vector3D.Dot(vector1, vector2_2);
      double num2 = Math.Max(Math.Abs(Vector3D.Dot((Vector3D) localAabb.HalfExtents, vector2_1)) - num1, 2.59999990463257);
      double num3 = 2.59999990463257;
      if (Math.Abs(vector2_1.Z) > 0.0001)
        num3 = (double) localAabb.HalfExtents.X * 1.5;
      else if (Math.Abs(vector2_1.X) > 0.0001)
        num3 = (double) localAabb.HalfExtents.Z * 1.5;
      double num4 = Math.Tan((double) MySector.MainCamera.FieldOfView * 0.5);
      double num5 = num3 / (2.0 * num4) + num2;
      this.m_safeMaximumDistance = 200.0;
      this.m_safeMinimumDistance = 2.5;
      this.SetPositionAndLookAt(this.m_initialLookAtDirectionCharacter * MathHelper.Clamp(num5, 2.5, 200.0));
    }

    private static bool HasSamePhysicalGroup(IMyEntity entityA, IMyEntity entityB)
    {
      if (entityA == entityB)
        return true;
      MyCubeGrid nodeA = entityA as MyCubeGrid;
      MyCubeGrid nodeB = entityB as MyCubeGrid;
      return nodeA != null && nodeB != null && MyCubeGridGroups.Static.Physical.HasSameGroup(nodeA, nodeB);
    }

    public void UpdateZoom()
    {
      Sandbox.Game.Entities.IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      MyStringId context = controlledEntity != null ? controlledEntity.ControlContext : MySpaceBindingCreator.CX_BASE;
      int num1 = !MyPerGameSettings.ZoomRequiresLookAroundPressed ? 1 : (MyControllerHelper.IsControl(context, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED) ? 1 : 0);
      double num2 = 0.0;
      if (num1 != 0)
      {
        float zoomMultiplier = MySandboxGame.Config.ZoomMultiplier;
        float num3 = 1f;
        float num4 = 1f;
        int num5 = MyInput.Static.PreviousMouseScrollWheelValue() < MyInput.Static.MouseScrollWheelValue() ? 1 : 0;
        bool flag = MyInput.Static.PreviousMouseScrollWheelValue() > MyInput.Static.MouseScrollWheelValue();
        if ((num5 | (flag ? 1 : 0)) != 0)
          num4 = (float) (1.0 * (double) zoomMultiplier + 1.0);
        if (num5 != 0)
          num2 = this.m_lookAt.Length() / (double) num4;
        else if (flag)
        {
          num2 = this.m_lookAt.Length() * (double) num4;
          this.m_positionSafeZoomingOutTimeout = -this.m_positionSafeZoomingOutDefaultTimeoutMs;
        }
        float num6 = MyControllerHelper.IsControlAnalog(context, MyControlsSpace.CAMERA_ZOOM_IN);
        float num7 = MyControllerHelper.IsControlAnalog(context, MyControlsSpace.CAMERA_ZOOM_OUT);
        if ((double) num6 + (double) num7 > 0.0)
          num3 = (float) (0.00999999977648258 + (double) zoomMultiplier * 0.0900000035762787);
        if ((double) num6 > 0.0)
          num2 = this.m_lookAt.Length() / (1.0 + (double) num3 * (double) num6);
        else if ((double) num7 > 0.0)
        {
          num2 = this.m_lookAt.Length() * (1.0 + (double) num3 * (double) num7);
          this.m_positionSafeZoomingOutTimeout = -this.m_positionSafeZoomingOutDefaultTimeoutMs;
        }
      }
      bool flag1 = false;
      if (num2 > 0.0)
      {
        double num3 = this.m_lookAt.Length();
        double num4 = MathHelper.Clamp(num2, Math.Max(this.m_safeMinimumDistance, 2.5), this.m_safeMaximumDistance);
        this.m_lookAt *= num4 / num3;
        flag1 = num4 > num3;
        if (flag1)
        {
          this.m_positionSafeZoomingOutTimeout = 0;
          this.m_safeMaximumDistanceTimeout = 0.0f;
        }
        this.SaveSettings();
      }
      else
      {
        double num3 = this.m_lookAt.Length();
        this.m_lookAt *= MathHelper.Clamp(num3, 2.5, 200.0) / num3;
        this.SaveSettings();
      }
      this.m_clampedlookAt = this.m_lookAt;
      double num8 = this.m_clampedlookAt.Length();
      this.m_clampedlookAt = this.m_clampedlookAt * MathHelper.Clamp(num8, this.m_safeMinimumDistance, this.m_safeMaximumDistance) / num8;
      if (!flag1 || this.m_lookAt.LengthSquared() >= this.m_safeMinimumDistance * this.m_safeMinimumDistance)
        return;
      this.m_lookAt = this.m_clampedlookAt;
    }

    public bool ResetViewerDistance(double? newDistance = null)
    {
      if (!newDistance.HasValue)
        return false;
      newDistance = new double?(MathHelper.Clamp(newDistance.Value, 2.5, 200.0));
      this.SetPositionAndLookAt((MySession.Static == null || !(MySession.Static.ControlledEntity is MyCharacter) ? this.m_initialLookAtDirection : this.m_initialLookAtDirectionCharacter) * newDistance.Value);
      this.m_disableSpringThisFrame = true;
      this.m_lastRaycastDist = (float) newDistance.Value;
      this.m_safeMaximumDistanceTimeout = 0.0f;
      this.m_zoomingOutSmoothly = false;
      this.m_positionSafeZoomingOutTimeout = -this.m_positionSafeZoomingOutDefaultTimeoutMs;
      this.m_saveSettings = false;
      this.Update();
      this.UpdateZoom();
      this.SaveSettings();
      return true;
    }

    public bool ResetViewerAngle(Vector2? headAngle)
    {
      if (!headAngle.HasValue)
        return false;
      Sandbox.Game.Entities.IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      if (controlledEntity == null)
        return false;
      controlledEntity.HeadLocalXAngle = headAngle.Value.X;
      controlledEntity.HeadLocalYAngle = headAngle.Value.Y;
      return true;
    }

    public double GetViewerDistance() => this.m_clampedlookAt.Length();

    public void SaveSettings() => this.m_saveSettings = true;

    public Vector3D GetCrosshair() => this.m_target + this.m_targetOrientation.Forward * 25000.0;

    public void CompensateQuickTransformChange(ref MatrixD transformDelta)
    {
      this.m_position = Vector3D.Transform(this.m_position, ref transformDelta);
      this.m_positionSafe = Vector3D.Transform(this.m_positionSafe, ref transformDelta);
      this.m_lastTarget = Vector3D.Transform(this.m_lastTarget, ref transformDelta);
      this.m_target = Vector3D.Transform(this.m_target, ref transformDelta);
      this.m_desiredPosition = Vector3D.Transform(this.m_desiredPosition, ref transformDelta);
    }

    private void DebugDrawTrail()
    {
      if (this.m_debugDraw && this.m_enableDebugDrawTrail)
      {
        if (this.m_debugLastSpectatorPositions == null)
        {
          this.m_debugLastSpectatorPositions = new List<Vector3D>(1024);
          this.m_debugLastSpectatorDesiredPositions = new List<Vector3D>(1024);
        }
        this.m_debugLastSpectatorPositions.Add(this.m_position);
        this.m_debugLastSpectatorDesiredPositions.Add(this.m_desiredPosition);
        if (this.m_debugLastSpectatorDesiredPositions.Count > 60)
        {
          this.m_debugLastSpectatorPositions.RemoveRange(0, 1);
          this.m_debugLastSpectatorDesiredPositions.RemoveRange(0, 1);
        }
        for (int index = 1; index < this.m_debugLastSpectatorPositions.Count; ++index)
        {
          float num = (float) index / (float) this.m_debugLastSpectatorPositions.Count;
          Color color = new Color(num * num, 0.0f, 0.0f);
          MyRenderProxy.DebugDrawLine3D(this.m_debugLastSpectatorPositions[index - 1], this.m_debugLastSpectatorPositions[index], color, color, true);
          color = new Color(num * num, num * num, num * num);
          MyRenderProxy.DebugDrawLine3D(this.m_debugLastSpectatorDesiredPositions[index - 1], this.m_debugLastSpectatorDesiredPositions[index], color, color, true);
        }
      }
      else
      {
        this.m_debugLastSpectatorPositions = (List<Vector3D>) null;
        this.m_debugLastSpectatorDesiredPositions = (List<Vector3D>) null;
      }
    }

    private enum MyCameraRaycastResult
    {
      Ok,
      FoundOccluder,
      FoundOccluderNoSpace,
    }

    public class SpringInfo
    {
      public float Stiffness;
      public float Dampening;
      public float Mass;
      public float RotationGain;

      public SpringInfo(float stiffness, float dampening, float mass, float rotationGain)
      {
        this.Stiffness = stiffness;
        this.Dampening = dampening;
        this.Mass = mass;
        this.RotationGain = rotationGain;
      }

      public SpringInfo(MyThirdPersonSpectator.SpringInfo spring) => this.Setup(spring);

      public void Setup(MyThirdPersonSpectator.SpringInfo spring)
      {
        this.Stiffness = spring.Stiffness;
        this.Dampening = spring.Dampening;
        this.Mass = spring.Mass;
      }
    }
  }
}
