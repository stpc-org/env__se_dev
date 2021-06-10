// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyEntityTransformationSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Groups;
using VRage.Input;
using VRage.ModAPI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.SessionComponents
{
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 900)]
  public class MyEntityTransformationSystem : MySessionComponentBase
  {
    private static float PICKING_RAY_LENGTH = 2000f;
    private static float PLANE_THICKNESS = 0.005f;
    private static bool DEBUG = false;
    private MyEntity m_controlledEntity;
    private readonly Dictionary<MyEntity, int> m_cachedBodyCollisionLayers = new Dictionary<MyEntity, int>();
    private MyOrientedBoundingBoxD m_xBB;
    private MyOrientedBoundingBoxD m_yBB;
    private MyOrientedBoundingBoxD m_zBB;
    private MyOrientedBoundingBoxD m_xPlane;
    private MyOrientedBoundingBoxD m_yPlane;
    private MyOrientedBoundingBoxD m_zPlane;
    private int m_selected;
    private MatrixD m_gizmoMatrix;
    private List<MyEntity> m_multiSelectedEntities;
    private List<Vector3D> m_multiSelectedEntityStartingPositions;
    private Vector3D[] m_multiStartingPositionsCopy;
    private PlaneD m_dragPlane;
    private bool m_dragActive;
    private bool m_dragOverAxis;
    private Vector3D m_dragStartingPoint;
    private Vector3D m_dragAxis;
    private Vector3D m_dragStartingPosition;
    private bool m_rotationActive;
    private PlaneD m_rotationPlane;
    private Vector3D m_rotationAxis;
    private Vector3D m_rotationStartingPoint;
    private MatrixD m_storedOrientation;
    private MatrixD m_storedScale;
    private Vector3D m_storedTranslation;
    private MatrixD m_storedWorldMatrix;
    private List<MatrixD> m_startingOrientations;
    private List<MatrixD> m_storedScales;
    private List<Vector3D> m_storedTranslations;
    private LineD m_lastRay;
    private readonly List<MyLineSegmentOverlapResult<MyEntity>> m_rayCastResultList = new List<MyLineSegmentOverlapResult<MyEntity>>();
    private bool m_active;
    private bool m_disableTransformation;

    public bool Active
    {
      get => this.m_active;
      set
      {
        if (this.Session == null || !this.Session.CreativeMode && !MySession.Static.IsUserAdmin(Sync.MyId) && !MySession.Static.CreativeToolsEnabled(Sync.MyId))
          return;
        if (!value)
        {
          this.SetControlledEntity((MyEntity) null);
          if (this.m_multiSelectedEntities != null)
          {
            foreach (MyEntity multiSelectedEntity in this.m_multiSelectedEntities)
              this.Physics_RestorePreviousCollisionLayerState(multiSelectedEntity);
            this.m_multiSelectedEntities.Clear();
            this.m_multiSelectedEntityStartingPositions.Clear();
          }
        }
        this.m_active = value;
      }
    }

    public bool DisableTransformation
    {
      get
      {
        bool flag = false;
        if (this.ControlledEntity is MyWaypoint)
          flag = (this.ControlledEntity as MyWaypoint).Freeze;
        return this.m_disableTransformation | flag;
      }
      set => this.m_disableTransformation = value;
    }

    public event Action<MyEntity, MyEntity> ControlledEntityChanged;

    public event Action<LineD> RayCasted;

    public MyEntityTransformationSystem.CoordinateMode Mode { get; set; }

    public MyEntityTransformationSystem.OperationMode Operation { get; set; }

    public MyEntity ControlledEntity => this.m_controlledEntity;

    public bool DisablePicking { get; set; }

    public LineD LastRay => this.m_lastRay;

    public MyEntityTransformationSystem()
    {
      this.Active = false;
      this.Mode = MyEntityTransformationSystem.CoordinateMode.WorldCoords;
      this.m_selected = -1;
      MySession.Static.CameraAttachedToChanged += (Action<IMyCameraController, IMyCameraController>) ((old, @new) => this.Active = false);
    }

    public override void Draw()
    {
      if (!this.Active)
        return;
      if (MyEntityTransformationSystem.DEBUG)
        MyRenderProxy.DebugDrawLine3D(this.m_lastRay.From, this.m_lastRay.To, Color.Green, Color.Green, true);
      Vector2 screenCoord1 = new Vector2(this.Session.Camera.ViewportSize.X * 0.01f, this.Session.Camera.ViewportSize.Y * 0.05f);
      Vector2 vector2_1 = new Vector2(this.Session.Camera.ViewportSize.Y * 0.11f, 0.0f);
      Vector2 vector2_2 = new Vector2(0.0f, this.Session.Camera.ViewportSize.Y * 0.015f);
      float scale = 0.65f * Math.Min(this.Session.Camera.ViewportSize.X / 1920f, this.Session.Camera.ViewportSize.Y / 1200f);
      MyRenderProxy.DebugDrawText2D(screenCoord1, "Transform:", Color.Yellow, scale);
      switch (this.Operation)
      {
        case MyEntityTransformationSystem.OperationMode.Translation:
          MyRenderProxy.DebugDrawText2D(screenCoord1 + vector2_1, "Translation", Color.Orange, scale);
          break;
        case MyEntityTransformationSystem.OperationMode.Rotation:
          MyRenderProxy.DebugDrawText2D(screenCoord1 + vector2_1, "Rotation", Color.PaleGreen, scale);
          break;
      }
      Vector2 screenCoord2 = screenCoord1 + vector2_2;
      MyRenderProxy.DebugDrawText2D(screenCoord2, "     Coords:", Color.Yellow, scale);
      switch (this.Mode)
      {
        case MyEntityTransformationSystem.CoordinateMode.LocalCoords:
          MyRenderProxy.DebugDrawText2D(screenCoord2 + vector2_1, "Local", Color.PaleGreen, scale);
          break;
        case MyEntityTransformationSystem.CoordinateMode.WorldCoords:
          MyRenderProxy.DebugDrawText2D(screenCoord2 + vector2_1, "World", Color.Orange, scale);
          break;
      }
      Vector2 screenCoord3 = screenCoord2 + 1.5f * vector2_2;
      MyRenderProxy.DebugDrawText2D(screenCoord3, "Cam loc:", Color.Yellow, scale);
      Vector2 vector2_3 = new Vector2(this.Session.Camera.ViewportSize.Y * 0.08f, 0.0f);
      Vector3D position1 = MyAPIGateway.Session.Camera.Position;
      MyRenderProxy.DebugDrawText2D(screenCoord3 + vector2_1, position1.X.ToString("0.00"), Color.Crimson, scale);
      MyRenderProxy.DebugDrawText2D(screenCoord3 + vector2_1 + 1f * vector2_2, position1.Y.ToString("0.00"), Color.PaleGreen, scale);
      MyRenderProxy.DebugDrawText2D(screenCoord3 + vector2_1 + 2f * vector2_2, position1.Z.ToString("0.00"), Color.CornflowerBlue, scale);
      Vector2 screenCoord4 = screenCoord3 + 3.5f * vector2_2;
      bool flag = this.ControlledEntity != null;
      MyRenderProxy.DebugDrawText2D(screenCoord4, flag ? "Selected:" : "No entity selected", Color.Yellow, scale);
      if (flag)
      {
        Vector3D position2 = this.ControlledEntity.PositionComp.GetPosition();
        MyRenderProxy.DebugDrawText2D(screenCoord4 + vector2_1, position2.X.ToString("0.00"), Color.Crimson, scale);
        MyRenderProxy.DebugDrawText2D(screenCoord4 + vector2_1 + 1f * vector2_2, position2.Y.ToString("0.00"), Color.PaleGreen, scale);
        MyRenderProxy.DebugDrawText2D(screenCoord4 + vector2_1 + 2f * vector2_2, position2.Z.ToString("0.00"), Color.CornflowerBlue, scale);
      }
      if (this.ControlledEntity == null)
        return;
      if (this.m_multiSelectedEntities != null)
      {
        foreach (MyEntity multiSelectedEntity in this.m_multiSelectedEntities)
          MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD((BoundingBoxD) multiSelectedEntity.PositionComp.LocalAABB, (MatrixD) ref multiSelectedEntity.PositionComp.LocalMatrixRef), Color.DarkRed, 1f, false, false);
      }
      if (this.ControlledEntity.Parent != null)
      {
        for (MyEntity parent = this.ControlledEntity.Parent; parent != null; parent = parent.Parent)
          MyRenderProxy.DebugDrawLine3D(this.ControlledEntity.Parent.PositionComp.GetPosition(), this.ControlledEntity.PositionComp.GetPosition(), Color.Orange, Color.Blue, false);
      }
      MatrixD projectionMatrix;
      if (this.Operation == MyEntityTransformationSystem.OperationMode.Translation && !this.DisableTransformation)
      {
        double num1 = Vector3D.Distance(this.m_xBB.Center, this.Session.Camera.Position);
        projectionMatrix = this.Session.Camera.ProjectionMatrix;
        double num2 = projectionMatrix.Up.LengthSquared();
        this.m_xBB.HalfExtent = Vector3D.One * 0.008 * num1 * num2;
        this.m_yBB.HalfExtent = Vector3D.One * 0.008 * num1 * num2;
        this.m_zBB.HalfExtent = Vector3D.One * 0.008 * num1 * num2;
        this.DrawOBB(this.m_xBB, Color.Red, 0.5f, 0);
        this.DrawOBB(this.m_yBB, Color.Green, 0.5f, 1);
        this.DrawOBB(this.m_zBB, Color.Blue, 0.5f, 2);
      }
      if (this.Operation == MyEntityTransformationSystem.OperationMode.Rotation && !this.DisableTransformation)
      {
        double num1 = Vector3D.Distance(this.m_xBB.Center, this.Session.Camera.Position);
        projectionMatrix = this.Session.Camera.ProjectionMatrix;
        double num2 = projectionMatrix.Up.LengthSquared();
        this.m_xBB.HalfExtent = Vector3D.One * 0.008 * num1 * num2;
        this.m_yBB.HalfExtent = Vector3D.One * 0.008 * num1 * num2;
        this.m_zBB.HalfExtent = Vector3D.One * 0.008 * num1 * num2;
        MyRenderProxy.DebugDrawSphere(this.m_xBB.Center, (float) this.m_xBB.HalfExtent.X, this.m_selected == 0 ? Color.White : Color.Red, depthRead: false);
        MyRenderProxy.DebugDrawSphere(this.m_yBB.Center, (float) this.m_xBB.HalfExtent.X, this.m_selected == 1 ? Color.White : Color.Green, depthRead: false);
        MyRenderProxy.DebugDrawSphere(this.m_zBB.Center, (float) this.m_xBB.HalfExtent.X, this.m_selected == 2 ? Color.White : Color.Blue, depthRead: false);
      }
      if (!this.DisableTransformation)
      {
        this.DrawOBB(this.m_xPlane, Color.Red, 0.2f, 3);
        this.DrawOBB(this.m_yPlane, Color.Green, 0.2f, 4);
        this.DrawOBB(this.m_zPlane, Color.Blue, 0.2f, 5);
      }
      else
        MyRenderProxy.DebugDrawSphere(this.ControlledEntity.PositionComp.WorldVolume.Center, (float) this.ControlledEntity.PositionComp.WorldVolume.Radius, Color.Yellow, 0.2f);
    }

    private void DrawOBB(
      MyOrientedBoundingBoxD obb,
      Color color,
      float alpha,
      int identificationIndex)
    {
      if (identificationIndex == this.m_selected)
        MyRenderProxy.DebugDrawOBB(obb, Color.White, 0.2f, false, false);
      else
        MyRenderProxy.DebugDrawOBB(obb, color, alpha, false, false);
    }

    public void ChangeOperationMode(MyEntityTransformationSystem.OperationMode mode) => this.Operation = mode;

    public void ChangeCoordSystem(bool world)
    {
      this.Mode = !world ? MyEntityTransformationSystem.CoordinateMode.LocalCoords : MyEntityTransformationSystem.CoordinateMode.WorldCoords;
      if (this.ControlledEntity == null)
        return;
      this.UpdateGizmoPosition();
    }

    public override void UpdateAfterSimulation()
    {
      if (!this.Active)
        return;
      if ((this.m_dragActive || this.m_rotationActive) && MyInput.Static.IsNewRightMousePressed())
      {
        this.SetWorldMatrix(ref this.m_storedWorldMatrix);
        this.m_dragActive = false;
        this.m_rotationActive = false;
        this.m_selected = -1;
      }
      if (!this.DisableTransformation)
      {
        if (this.m_dragActive)
          this.PerformDragg(this.m_dragOverAxis);
        if (this.m_rotationActive)
          this.PerformRotation();
      }
      if (MyInput.Static.IsNewKeyPressed(MyKeys.R))
      {
        switch (this.Operation)
        {
          case MyEntityTransformationSystem.OperationMode.Translation:
            this.Operation = MyEntityTransformationSystem.OperationMode.Rotation;
            break;
          case MyEntityTransformationSystem.OperationMode.Rotation:
            this.Operation = MyEntityTransformationSystem.OperationMode.Translation;
            break;
        }
      }
      if (MyInput.Static.IsNewKeyPressed(MyKeys.T))
      {
        switch (this.Mode)
        {
          case MyEntityTransformationSystem.CoordinateMode.LocalCoords:
            this.Mode = MyEntityTransformationSystem.CoordinateMode.WorldCoords;
            break;
          case MyEntityTransformationSystem.CoordinateMode.WorldCoords:
            this.Mode = MyEntityTransformationSystem.CoordinateMode.LocalCoords;
            break;
        }
        if (this.ControlledEntity != null)
          this.UpdateGizmoPosition();
      }
      if (MyInput.Static.IsAnyShiftKeyPressed() && MyInput.Static.IsNewLeftMousePressed())
      {
        if (this.m_multiSelectedEntities == null)
        {
          this.m_multiSelectedEntities = new List<MyEntity>();
          this.m_multiSelectedEntityStartingPositions = new List<Vector3D>();
        }
        this.m_lastRay = this.CreateRayFromCursorPosition();
        if (this.RayCasted != null)
          this.RayCasted(this.m_lastRay);
        if (this.ControlledEntity != null && this.PickControl())
        {
          this.m_storedWorldMatrix = this.ControlledEntity.PositionComp.WorldMatrixRef;
        }
        else
        {
          this.m_selected = -1;
          MyEntity entity = this.PickEntity();
          if (entity != null)
          {
            if (!this.m_multiSelectedEntities.Contains(entity))
            {
              this.m_multiSelectedEntities.Add(entity);
              this.Physics_MoveEntityToNoCollisionLayer(entity);
              this.m_multiSelectedEntityStartingPositions.Add(entity.PositionComp.GetPosition());
            }
            else
            {
              this.Physics_RestorePreviousCollisionLayerState(entity);
              this.m_multiSelectedEntities.Remove(entity);
              this.m_multiSelectedEntityStartingPositions.Remove(entity.PositionComp.GetPosition());
            }
            if (this.m_multiSelectedEntities.Count > 0)
              this.SetControlledEntity(this.m_multiSelectedEntities[0]);
            this.m_multiStartingPositionsCopy = new Vector3D[this.m_multiSelectedEntityStartingPositions.Count];
            this.m_multiSelectedEntityStartingPositions.CopyTo(this.m_multiStartingPositionsCopy);
          }
        }
      }
      else if (MyInput.Static.IsNewLeftMousePressed())
      {
        if (this.m_multiSelectedEntities != null)
        {
          foreach (MyEntity multiSelectedEntity in this.m_multiSelectedEntities)
            this.Physics_RestorePreviousCollisionLayerState(multiSelectedEntity);
          this.m_multiSelectedEntities.Clear();
          this.m_multiSelectedEntityStartingPositions.Clear();
        }
        else
        {
          this.m_multiSelectedEntities = new List<MyEntity>();
          this.m_multiSelectedEntityStartingPositions = new List<Vector3D>();
        }
        if (this.DisablePicking)
          return;
        this.m_lastRay = this.CreateRayFromCursorPosition();
        if (this.RayCasted != null)
          this.RayCasted(this.m_lastRay);
        if (this.ControlledEntity != null && this.PickControl())
        {
          this.m_storedWorldMatrix = this.ControlledEntity.PositionComp.WorldMatrixRef;
        }
        else
        {
          this.m_selected = -1;
          this.SetControlledEntity(this.PickEntity());
        }
      }
      if (MyInput.Static.IsNewLeftMouseReleased())
      {
        this.m_dragActive = false;
        this.m_rotationActive = false;
        this.m_selected = -1;
        if (this.m_multiSelectedEntities != null && this.m_multiSelectedEntities.Count > 0)
        {
          this.m_multiSelectedEntityStartingPositions.Clear();
          foreach (MyEntity multiSelectedEntity in this.m_multiSelectedEntities)
            this.m_multiSelectedEntityStartingPositions.Add(multiSelectedEntity.PositionComp.GetPosition());
          this.m_multiSelectedEntityStartingPositions.CopyTo(this.m_multiStartingPositionsCopy);
        }
      }
      if (this.ControlledEntity != null && this.ControlledEntity.Physics != null)
      {
        this.ControlledEntity.Physics.ClearSpeed();
        if (this.m_multiSelectedEntities != null)
        {
          foreach (MyEntity multiSelectedEntity in this.m_multiSelectedEntities)
          {
            if (multiSelectedEntity.Physics != null)
              multiSelectedEntity.Physics.ClearSpeed();
          }
        }
      }
      if (this.ControlledEntity == null)
        return;
      this.UpdateGizmoPosition();
    }

    private void PerformRotation()
    {
      LineD fromCursorPosition = this.CreateRayFromCursorPosition();
      Vector3D vector3D = this.m_rotationPlane.Intersection(ref fromCursorPosition.From, ref fromCursorPosition.Direction);
      if (Vector3D.DistanceSquared(this.m_rotationStartingPoint, vector3D) < 9.88131291682493E-323)
        return;
      MatrixD fromQuaternion = MatrixD.CreateFromQuaternion(QuaternionD.CreateFromTwoVectors(this.m_rotationStartingPoint - this.m_gizmoMatrix.Translation, vector3D - this.m_gizmoMatrix.Translation));
      if (this.m_multiSelectedEntities != null)
      {
        for (int index = 0; index < this.m_multiSelectedEntities.Count; ++index)
        {
          MatrixD newWorldMatrix = this.m_startingOrientations[index] * fromQuaternion * this.m_storedScales[index];
          newWorldMatrix.Translation = this.m_storedTranslations[index];
          this.SetWorldMatrix(ref newWorldMatrix, this.m_multiSelectedEntities[index]);
        }
      }
      MatrixD newWorldMatrix1 = this.m_storedOrientation * fromQuaternion * this.m_storedScale;
      newWorldMatrix1.Translation = this.m_storedTranslation;
      this.SetWorldMatrix(ref newWorldMatrix1);
    }

    private LineD CreateRayFromCursorPosition()
    {
      LineD lineD = this.Session.Camera.WorldLineFromScreen(MyInput.Static.GetMousePosition());
      return new LineD(lineD.From, lineD.From + lineD.Direction * (double) MyEntityTransformationSystem.PICKING_RAY_LENGTH);
    }

    private void PerformDragg(bool lockToAxis = true)
    {
      if (this.ControlledEntity == null)
        return;
      LineD fromCursorPosition = this.CreateRayFromCursorPosition();
      Vector3D vector3D = this.m_dragPlane.Intersection(ref fromCursorPosition.From, ref fromCursorPosition.Direction) - this.m_dragStartingPoint;
      if (lockToAxis)
      {
        double num = vector3D.Dot(ref this.m_dragAxis);
        if (Math.Abs(num) < double.Epsilon)
          return;
        if (this.m_multiSelectedEntities.Count <= 0)
        {
          MatrixD newWorldMatrix = this.ControlledEntity.PositionComp.WorldMatrixRef;
          newWorldMatrix.Translation = this.m_dragStartingPosition + this.m_dragAxis * num;
          this.SetWorldMatrix(ref newWorldMatrix);
        }
        else if (this.m_multiSelectedEntities.Count > 0)
        {
          MatrixD newWorldMatrix = this.ControlledEntity.PositionComp.WorldMatrixRef;
          newWorldMatrix.Translation = this.m_dragStartingPosition + this.m_dragAxis * num;
          this.SetWorldMatrix(ref newWorldMatrix);
          for (int index = 0; index < this.m_multiSelectedEntities.Count; ++index)
          {
            if (index != 0)
            {
              newWorldMatrix = this.m_multiSelectedEntities[index].PositionComp.WorldMatrixRef;
              newWorldMatrix.Translation = this.m_multiStartingPositionsCopy[index] + this.m_dragAxis * num;
              this.SetWorldMatrix(ref newWorldMatrix, this.m_multiSelectedEntities[index]);
            }
          }
        }
      }
      else
      {
        if (vector3D.LengthSquared() < double.Epsilon)
          return;
        if (this.m_multiSelectedEntities.Count <= 0)
        {
          MatrixD newWorldMatrix = this.ControlledEntity.PositionComp.WorldMatrixRef;
          newWorldMatrix.Translation = this.m_dragStartingPosition + vector3D;
          this.SetWorldMatrix(ref newWorldMatrix);
        }
        else if (this.m_multiSelectedEntities.Count > 0)
        {
          MatrixD newWorldMatrix = this.ControlledEntity.PositionComp.WorldMatrixRef;
          newWorldMatrix.Translation = this.m_dragStartingPosition + vector3D;
          this.SetWorldMatrix(ref newWorldMatrix);
          for (int index = 0; index < this.m_multiSelectedEntities.Count; ++index)
          {
            if (index != 0)
            {
              newWorldMatrix = this.m_multiSelectedEntities[index].PositionComp.WorldMatrixRef;
              newWorldMatrix.Translation = this.m_multiStartingPositionsCopy[index] + vector3D;
              this.SetWorldMatrix(ref newWorldMatrix, this.m_multiSelectedEntities[index]);
            }
          }
        }
      }
      this.UpdateGizmoPosition();
    }

    private bool PickControl()
    {
      if (this.m_xBB.Intersects(ref this.m_lastRay).HasValue)
      {
        if (this.Operation == MyEntityTransformationSystem.OperationMode.Rotation)
        {
          this.PrepareRotation(this.m_gizmoMatrix.Right);
          this.m_rotationActive = true;
        }
        else
        {
          this.PrepareDrag(new Vector3D?(), new Vector3D?(this.m_gizmoMatrix.Right));
          this.m_dragActive = true;
        }
        this.m_selected = 0;
        return true;
      }
      if (this.m_yBB.Intersects(ref this.m_lastRay).HasValue)
      {
        if (this.Operation == MyEntityTransformationSystem.OperationMode.Rotation)
        {
          this.PrepareRotation(this.m_gizmoMatrix.Up);
          this.m_rotationActive = true;
        }
        else
        {
          this.PrepareDrag(new Vector3D?(), new Vector3D?(this.m_gizmoMatrix.Up));
          this.m_dragActive = true;
        }
        this.m_selected = 1;
        return true;
      }
      if (this.m_zBB.Intersects(ref this.m_lastRay).HasValue)
      {
        if (this.Operation == MyEntityTransformationSystem.OperationMode.Rotation)
        {
          this.PrepareRotation(this.m_gizmoMatrix.Backward);
          this.m_rotationActive = true;
        }
        else
        {
          this.PrepareDrag(new Vector3D?(), new Vector3D?(this.m_gizmoMatrix.Backward));
          this.m_dragActive = true;
        }
        this.m_selected = 2;
        return true;
      }
      if (this.m_xPlane.Intersects(ref this.m_lastRay).HasValue)
      {
        if (this.Operation == MyEntityTransformationSystem.OperationMode.Rotation)
        {
          this.PrepareRotation(this.m_gizmoMatrix.Right);
          this.m_rotationActive = true;
        }
        else
        {
          this.PrepareDrag(new Vector3D?(this.m_gizmoMatrix.Right), new Vector3D?());
          this.m_dragActive = true;
        }
        this.m_selected = 3;
        return true;
      }
      if (this.m_yPlane.Intersects(ref this.m_lastRay).HasValue)
      {
        if (this.Operation == MyEntityTransformationSystem.OperationMode.Rotation)
        {
          this.PrepareRotation(this.m_gizmoMatrix.Up);
          this.m_rotationActive = true;
        }
        else
        {
          this.PrepareDrag(new Vector3D?(this.m_gizmoMatrix.Up), new Vector3D?());
          this.m_dragActive = true;
        }
        this.m_selected = 4;
        return true;
      }
      if (!this.m_zPlane.Intersects(ref this.m_lastRay).HasValue)
        return false;
      if (this.Operation == MyEntityTransformationSystem.OperationMode.Rotation)
      {
        this.PrepareRotation(this.m_gizmoMatrix.Backward);
        this.m_rotationActive = true;
      }
      else
      {
        this.PrepareDrag(new Vector3D?(this.m_gizmoMatrix.Backward), new Vector3D?());
        this.m_dragActive = true;
      }
      this.m_selected = 5;
      return true;
    }

    private void SetWorldMatrix(ref MatrixD newWorldMatrix)
    {
      if (this.ControlledEntity is MyCubeGrid controlledEntity)
      {
        MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group group = MyCubeGridGroups.Static.Physical.GetGroup(controlledEntity);
        MatrixD matrixD = controlledEntity.PositionComp.WorldMatrixNormalizedInv;
        controlledEntity.PositionComp.SetWorldMatrix(ref newWorldMatrix);
        foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node member in group.m_members)
        {
          if (member.NodeData.Parent == null && member.NodeData != controlledEntity)
          {
            MatrixD worldMatrix = member.NodeData.PositionComp.WorldMatrixRef * matrixD * newWorldMatrix;
            member.NodeData.PositionComp.SetWorldMatrix(ref worldMatrix);
          }
        }
      }
      else if (this.ControlledEntity.Parent != null)
        this.ControlledEntity.PositionComp.SetWorldMatrix(ref newWorldMatrix, (object) this.ControlledEntity.Parent, true);
      else
        this.ControlledEntity.PositionComp.SetWorldMatrix(ref newWorldMatrix);
    }

    private void SetWorldMatrix(ref MatrixD newWorldMatrix, MyEntity listedEntity)
    {
      if (listedEntity is MyCubeGrid Node)
      {
        MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group group = MyCubeGridGroups.Static.Physical.GetGroup(Node);
        MatrixD matrixD = Node.PositionComp.WorldMatrixNormalizedInv;
        Node.PositionComp.SetWorldMatrix(ref newWorldMatrix);
        foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node member in group.m_members)
        {
          if (member.NodeData.Parent == null && member.NodeData != Node)
          {
            MatrixD worldMatrix = member.NodeData.PositionComp.WorldMatrixRef * matrixD * newWorldMatrix;
            member.NodeData.PositionComp.SetWorldMatrix(ref worldMatrix);
          }
        }
      }
      else if (listedEntity.Parent != null)
        listedEntity.PositionComp.SetWorldMatrix(ref newWorldMatrix, (object) listedEntity.Parent, true);
      else
        listedEntity.PositionComp.SetWorldMatrix(ref newWorldMatrix);
    }

    private void PrepareRotation(Vector3D axis)
    {
      if (this.m_startingOrientations != null)
        this.m_startingOrientations.Clear();
      this.m_rotationAxis = axis;
      this.m_rotationPlane = new PlaneD(this.m_gizmoMatrix.Translation, this.m_rotationAxis);
      this.m_rotationStartingPoint = this.m_rotationPlane.Intersection(ref this.m_lastRay.From, ref this.m_lastRay.Direction);
      if (this.m_multiSelectedEntities != null)
      {
        this.m_startingOrientations = new List<MatrixD>();
        this.m_storedScales = new List<MatrixD>();
        this.m_storedTranslations = new List<Vector3D>();
        foreach (MyEntity multiSelectedEntity in this.m_multiSelectedEntities)
        {
          MatrixD worldMatrix = multiSelectedEntity.WorldMatrix;
          this.m_startingOrientations.Add(worldMatrix.GetOrientation());
          this.m_storedScales.Add(MatrixD.CreateScale(worldMatrix.Scale));
          this.m_storedTranslations.Add(worldMatrix.Translation);
        }
      }
      MatrixD matrixD = this.ControlledEntity.PositionComp.WorldMatrixRef;
      this.m_storedScale = MatrixD.CreateScale(matrixD.Scale);
      this.m_storedTranslation = matrixD.Translation;
      this.m_storedOrientation = matrixD.GetOrientation();
    }

    private void PrepareDrag(Vector3D? planeNormal, Vector3D? axis)
    {
      if (axis.HasValue)
      {
        Vector3D vector2_1 = this.Session.Camera.Position - this.m_gizmoMatrix.Translation;
        Vector3D vector2_2 = Vector3D.Cross(axis.Value, vector2_1);
        planeNormal = new Vector3D?(Vector3D.Cross(axis.Value, vector2_2));
        this.m_dragPlane = new PlaneD(this.m_gizmoMatrix.Translation, planeNormal.Value);
      }
      else if (planeNormal.HasValue)
        this.m_dragPlane = new PlaneD(this.m_gizmoMatrix.Translation, planeNormal.Value);
      this.m_dragStartingPoint = this.m_dragPlane.Intersection(ref this.m_lastRay.From, ref this.m_lastRay.Direction);
      if (axis.HasValue)
        this.m_dragAxis = axis.Value;
      this.m_dragOverAxis = axis.HasValue;
      this.m_dragStartingPosition = this.ControlledEntity.PositionComp.GetPosition();
    }

    private MyEntity PickEntity()
    {
      MyPhysics.HitInfo? nullable = MyPhysics.CastRay(this.m_lastRay.From, this.m_lastRay.To);
      this.m_rayCastResultList.Clear();
      MyGamePruningStructure.GetAllEntitiesInRay(ref this.m_lastRay, this.m_rayCastResultList);
      int a = 0;
      for (int index = 0; index < this.m_rayCastResultList.Count; ++index)
      {
        if (!(this.m_rayCastResultList[index].Element is MyCubeGrid))
        {
          this.m_rayCastResultList.Swap<MyLineSegmentOverlapResult<MyEntity>>(a, index);
          ++a;
        }
      }
      if (this.m_rayCastResultList.Count == 0 && !nullable.HasValue)
        return (MyEntity) null;
      MyEntity myEntity = (MyEntity) null;
      double distance = double.MaxValue;
      foreach (MyLineSegmentOverlapResult<MyEntity> rayCastResult in this.m_rayCastResultList)
      {
        if (rayCastResult.Element.PositionComp.WorldAABB.Intersects(ref this.m_lastRay, out distance) && (rayCastResult.Element is MyCubeGrid || rayCastResult.Element is MyFloatingObject || (rayCastResult.Element.GetType() == typeof (MyEntity) || rayCastResult.Element.GetType() == typeof (MyWaypoint))))
        {
          myEntity = rayCastResult.Element;
          break;
        }
      }
      if (nullable.HasValue && Vector3D.Distance(nullable.Value.Position, this.m_lastRay.From) < distance)
      {
        IMyEntity hitEntity = nullable.Value.HkHitInfo.GetHitEntity();
        if (hitEntity is MyCubeGrid || hitEntity is MyFloatingObject)
          return (MyEntity) hitEntity;
      }
      if (myEntity == null)
        myEntity = this.ControlledEntity;
      return myEntity ?? (MyEntity) null;
    }

    public void SetControlledEntity(MyEntity entity)
    {
      if (this.ControlledEntity == entity)
        return;
      if (this.ControlledEntity != null)
      {
        this.ControlledEntity.PositionComp.OnPositionChanged -= new Action<MyPositionComponentBase>(this.ControlledEntityPositionChanged);
        this.Physics_RestorePreviousCollisionLayerState();
        if (this.m_multiSelectedEntities != null)
        {
          foreach (MyEntity multiSelectedEntity in this.m_multiSelectedEntities)
            this.Physics_RestorePreviousCollisionLayerState(multiSelectedEntity);
        }
      }
      MyEntity controlledEntity = this.ControlledEntity;
      this.m_controlledEntity = entity;
      if (this.ControlledEntityChanged != null)
        this.ControlledEntityChanged(controlledEntity, entity);
      if (entity == null)
        return;
      this.ControlledEntity.PositionComp.OnPositionChanged += new Action<MyPositionComponentBase>(this.ControlledEntityPositionChanged);
      this.ControlledEntity.OnClosing += (Action<MyEntity>) (myEntity =>
      {
        myEntity.PositionComp.OnPositionChanged -= new Action<MyPositionComponentBase>(this.ControlledEntityPositionChanged);
        this.m_controlledEntity = (MyEntity) null;
      });
      this.Physics_ClearCollisionLayerCache();
      this.Physics_MoveEntityToNoCollisionLayer(this.ControlledEntity);
      this.UpdateGizmoPosition();
    }

    private void Physics_RestorePreviousCollisionLayerState()
    {
      foreach (KeyValuePair<MyEntity, int> bodyCollisionLayer in this.m_cachedBodyCollisionLayers)
      {
        if (bodyCollisionLayer.Key.Physics != null)
          bodyCollisionLayer.Key.Physics.RigidBody.Layer = bodyCollisionLayer.Value;
      }
    }

    private void Physics_RestorePreviousCollisionLayerState(MyEntity entity)
    {
      foreach (KeyValuePair<MyEntity, int> bodyCollisionLayer in this.m_cachedBodyCollisionLayers)
      {
        if (bodyCollisionLayer.Key.Physics != null && bodyCollisionLayer.Key == entity)
        {
          bodyCollisionLayer.Key.Physics.RigidBody.Layer = bodyCollisionLayer.Value;
          this.m_cachedBodyCollisionLayers.Remove(bodyCollisionLayer.Key);
          break;
        }
      }
    }

    private void Physics_ClearCollisionLayerCache() => this.m_cachedBodyCollisionLayers.Clear();

    private void Physics_MoveEntityToNoCollisionLayer(MyEntity entity)
    {
      if (entity is MyCubeGrid Node)
      {
        foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node member in MyCubeGridGroups.Static.Physical.GetGroup(Node).m_members)
        {
          if (member.NodeData.Parent == null)
            this.Physics_MoveEntityToNoCollisionLayerRecursive((MyEntity) member.NodeData);
        }
      }
      else
        this.Physics_MoveEntityToNoCollisionLayerRecursive(entity);
    }

    private void Physics_MoveEntityToNoCollisionLayerRecursive(MyEntity entity)
    {
      if (entity.Physics != null)
      {
        if (this.m_cachedBodyCollisionLayers.ContainsKey(entity))
          return;
        this.m_cachedBodyCollisionLayers.Add(entity, entity.Physics.RigidBody.Layer);
        entity.Physics.RigidBody.Layer = 19;
      }
      foreach (MyHierarchyComponentBase child in entity.Hierarchy.Children)
      {
        if (child.Entity.Physics != null && (HkReferenceObject) child.Entity.Physics.RigidBody != (HkReferenceObject) null)
          this.Physics_MoveEntityToNoCollisionLayerRecursive((MyEntity) child.Entity);
      }
    }

    private void ControlledEntityPositionChanged(MyPositionComponentBase myPositionComponentBase) => this.UpdateGizmoPosition();

    private void UpdateGizmoPosition()
    {
      MatrixD matrixD = this.ControlledEntity.PositionComp.WorldMatrixRef;
      double radius = this.ControlledEntity.PositionComp.WorldVolume.Radius;
      if (radius <= 0.0)
        ++radius;
      double num1 = (this.ControlledEntity.PositionComp.WorldVolume.Center - matrixD.Translation).Length();
      double num2 = radius + num1;
      double num3 = Vector3D.Distance(MyAPIGateway.Session.Camera.Position, this.ControlledEntity.PositionComp.GetPosition()) / (!(this.ControlledEntity is MyCubeGrid) ? 5.0 : ((this.ControlledEntity as MyCubeGrid).GridSizeEnum != MyCubeSize.Large ? 100.0 : 200.0));
      double num4 = num3 <= 0.5 ? num2 * 0.5 : num2 * num3;
      this.m_gizmoMatrix = MatrixD.Identity;
      if (this.Mode == MyEntityTransformationSystem.CoordinateMode.LocalCoords)
      {
        this.m_gizmoMatrix = matrixD;
        this.m_gizmoMatrix = MatrixD.Normalize(this.m_gizmoMatrix);
      }
      else
        this.m_gizmoMatrix.Translation = matrixD.Translation;
      this.m_xBB.Center = new Vector3D(num4, 0.0, 0.0);
      this.m_yBB.Center = new Vector3D(0.0, num4, 0.0);
      this.m_zBB.Center = new Vector3D(0.0, 0.0, num4);
      this.m_xBB.Orientation = Quaternion.Identity;
      this.m_yBB.Orientation = Quaternion.Identity;
      this.m_zBB.Orientation = Quaternion.Identity;
      this.m_xBB.Transform(this.m_gizmoMatrix);
      this.m_yBB.Transform(this.m_gizmoMatrix);
      this.m_zBB.Transform(this.m_gizmoMatrix);
      this.m_xPlane.Center = new Vector3D(-(double) MyEntityTransformationSystem.PLANE_THICKNESS / 2.0, num4 / 2.0, num4 / 2.0);
      this.m_yPlane.Center = new Vector3D(num4 / 2.0, (double) MyEntityTransformationSystem.PLANE_THICKNESS / 2.0, num4 / 2.0);
      this.m_zPlane.Center = new Vector3D(num4 / 2.0, num4 / 2.0, (double) MyEntityTransformationSystem.PLANE_THICKNESS / 2.0);
      this.m_xPlane.HalfExtent = new Vector3D((double) MyEntityTransformationSystem.PLANE_THICKNESS / 2.0, num4 / 2.0, num4 / 2.0);
      this.m_yPlane.HalfExtent = new Vector3D(num4 / 2.0, (double) MyEntityTransformationSystem.PLANE_THICKNESS / 2.0, num4 / 2.0);
      this.m_zPlane.HalfExtent = new Vector3D(num4 / 2.0, num4 / 2.0, (double) MyEntityTransformationSystem.PLANE_THICKNESS / 2.0);
      this.m_xPlane.Orientation = Quaternion.Identity;
      this.m_yPlane.Orientation = Quaternion.Identity;
      this.m_zPlane.Orientation = Quaternion.Identity;
      this.m_xPlane.Transform(this.m_gizmoMatrix);
      this.m_yPlane.Transform(this.m_gizmoMatrix);
      this.m_zPlane.Transform(this.m_gizmoMatrix);
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      this.Session = (IMySession) null;
    }

    public enum CoordinateMode
    {
      LocalCoords,
      WorldCoords,
    }

    public enum OperationMode
    {
      Translation,
      Rotation,
    }
  }
}
