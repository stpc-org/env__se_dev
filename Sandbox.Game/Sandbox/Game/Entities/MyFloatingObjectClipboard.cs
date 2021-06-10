// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyFloatingObjectClipboard
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Input;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
  internal class MyFloatingObjectClipboard
  {
    private List<MyObjectBuilder_FloatingObject> m_copiedFloatingObjects = new List<MyObjectBuilder_FloatingObject>();
    private List<Vector3D> m_copiedFloatingObjectOffsets = new List<Vector3D>();
    private List<MyFloatingObject> m_previewFloatingObjects = new List<MyFloatingObject>();
    private Vector3D m_pastePosition;
    private Vector3D m_pastePositionPrevious;
    private bool m_calculateVelocity = true;
    private Vector3 m_objectVelocity = Vector3.Zero;
    private float m_pasteOrientationAngle;
    private Vector3 m_pasteDirUp = new Vector3(1f, 0.0f, 0.0f);
    private Vector3 m_pasteDirForward = new Vector3(0.0f, 1f, 0.0f);
    private float m_dragDistance;
    private Vector3D m_dragPointToPositionLocal;
    private bool m_canBePlaced;
    private List<MyPhysics.HitInfo> m_raycastCollisionResults = new List<MyPhysics.HitInfo>();
    private float m_closestHitDistSq = float.MaxValue;
    private Vector3D m_hitPos = (Vector3D) new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 m_hitNormal = new Vector3(1f, 0.0f, 0.0f);
    private bool m_visible = true;
    private bool m_enableStationRotation;

    public bool IsActive { get; private set; }

    public List<MyFloatingObject> PreviewFloatingObjects => this.m_previewFloatingObjects;

    public bool EnableStationRotation
    {
      get => this.m_enableStationRotation && MyFakes.ENABLE_STATION_ROTATION;
      set => this.m_enableStationRotation = value;
    }

    public MyFloatingObjectClipboard(bool calculateVelocity = true) => this.m_calculateVelocity = calculateVelocity;

    private void Activate()
    {
      this.ChangeClipboardPreview(true);
      this.IsActive = true;
    }

    public void Deactivate()
    {
      this.ChangeClipboardPreview(false);
      this.IsActive = false;
    }

    public void Hide() => this.ChangeClipboardPreview(false);

    public void Show()
    {
      if (!this.IsActive || this.m_previewFloatingObjects.Count != 0)
        return;
      this.ChangeClipboardPreview(true);
    }

    public void CutFloatingObject(MyFloatingObject floatingObject)
    {
      if (floatingObject == null)
        return;
      this.CopyfloatingObject(floatingObject);
      this.DeleteFloatingObject(floatingObject);
    }

    public void DeleteFloatingObject(MyFloatingObject floatingObject)
    {
      if (floatingObject == null)
        return;
      MyFloatingObjects.RemoveFloatingObject(floatingObject, true);
      this.Deactivate();
    }

    public void CopyfloatingObject(MyFloatingObject floatingObject)
    {
      if (floatingObject == null)
        return;
      this.m_copiedFloatingObjects.Clear();
      this.m_copiedFloatingObjectOffsets.Clear();
      this.CopyFloatingObjectInternal(floatingObject);
      this.Activate();
    }

    private void CopyFloatingObjectInternal(MyFloatingObject toCopy)
    {
      MyObjectBuilder_FloatingObject objectBuilder = (MyObjectBuilder_FloatingObject) toCopy.GetObjectBuilder(true);
      objectBuilder.EntityId = 0L;
      objectBuilder.Name = (string) null;
      this.m_copiedFloatingObjects.Add(objectBuilder);
      if (this.m_copiedFloatingObjects.Count == 1)
      {
        MatrixD pasteMatrix = MyFloatingObjectClipboard.GetPasteMatrix();
        Vector3D translation = toCopy.WorldMatrix.Translation;
        this.m_dragPointToPositionLocal = Vector3D.TransformNormal(toCopy.PositionComp.GetPosition() - translation, toCopy.PositionComp.WorldMatrixNormalizedInv);
        this.m_dragDistance = (float) (translation - pasteMatrix.Translation).Length();
        this.m_pasteDirUp = (Vector3) toCopy.WorldMatrix.Up;
        this.m_pasteDirForward = (Vector3) toCopy.WorldMatrix.Forward;
        this.m_pasteOrientationAngle = 0.0f;
      }
      this.m_copiedFloatingObjectOffsets.Add(toCopy.WorldMatrix.Translation - (Vector3D) this.m_copiedFloatingObjects[0].PositionAndOrientation.Value.Position);
    }

    public bool PasteFloatingObject(MyInventory buildInventory = null)
    {
      if (this.m_copiedFloatingObjects.Count == 0)
        return false;
      if (this.m_copiedFloatingObjects.Count > 0 && !this.IsActive)
      {
        if (this.CheckPastedFloatingObjects())
        {
          this.Activate();
        }
        else
        {
          MyHud.Notifications.Add(MyNotificationSingletons.CopyPasteBlockNotAvailable);
          MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
        }
        return true;
      }
      if (!this.m_canBePlaced)
      {
        MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
        return false;
      }
      MyGuiAudio.PlaySound(MyGuiSounds.HudPlaceItem);
      MyEntities.RemapObjectBuilderCollection((IEnumerable<MyObjectBuilder_EntityBase>) this.m_copiedFloatingObjects);
      bool flag = false;
      int index = 0;
      foreach (MyObjectBuilder_FloatingObject copiedFloatingObject in this.m_copiedFloatingObjects)
      {
        copiedFloatingObject.PersistentFlags = MyPersistentEntityFlags2.Enabled | MyPersistentEntityFlags2.InScene;
        copiedFloatingObject.PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation(this.m_previewFloatingObjects[index].WorldMatrix));
        ++index;
        MyFloatingObjects.RequestSpawnCreative(copiedFloatingObject);
        flag = true;
      }
      this.Deactivate();
      return flag;
    }

    private bool CheckPastedFloatingObjects()
    {
      foreach (MyObjectBuilder_FloatingObject copiedFloatingObject in this.m_copiedFloatingObjects)
      {
        if (!MyDefinitionManager.Static.TryGetPhysicalItemDefinition(copiedFloatingObject.Item.PhysicalContent.GetId(), out MyPhysicalItemDefinition _))
          return false;
      }
      return true;
    }

    public void SetFloatingObjectFromBuilder(
      MyObjectBuilder_FloatingObject floatingObject,
      Vector3D dragPointDelta,
      float dragVectorLength)
    {
      if (this.IsActive)
        this.Deactivate();
      this.m_copiedFloatingObjects.Clear();
      this.m_copiedFloatingObjectOffsets.Clear();
      MatrixD pasteMatrix = MyFloatingObjectClipboard.GetPasteMatrix();
      Matrix matrix = (Matrix) ref pasteMatrix;
      this.m_dragPointToPositionLocal = dragPointDelta;
      this.m_dragDistance = dragVectorLength;
      MyPositionAndOrientation positionAndOrientation = floatingObject.PositionAndOrientation ?? MyPositionAndOrientation.Default;
      this.m_pasteDirUp = (Vector3) positionAndOrientation.Up;
      this.m_pasteDirForward = (Vector3) positionAndOrientation.Forward;
      this.SetFloatingObjectFromBuilderInternal(floatingObject, Vector3D.Zero);
      this.Activate();
    }

    private void SetFloatingObjectFromBuilderInternal(
      MyObjectBuilder_FloatingObject floatingObject,
      Vector3D offset)
    {
      this.m_copiedFloatingObjects.Add(floatingObject);
      this.m_copiedFloatingObjectOffsets.Add(offset);
    }

    private void ChangeClipboardPreview(bool visible)
    {
      if (this.m_copiedFloatingObjects.Count == 0 || !visible)
      {
        foreach (MyFloatingObject previewFloatingObject in this.m_previewFloatingObjects)
        {
          MyEntities.EnableEntityBoundingBoxDraw((MyEntity) previewFloatingObject, false);
          previewFloatingObject.Close();
        }
        this.m_previewFloatingObjects.Clear();
        this.m_visible = false;
      }
      else
      {
        MyEntities.RemapObjectBuilderCollection((IEnumerable<MyObjectBuilder_EntityBase>) this.m_copiedFloatingObjects);
        foreach (MyObjectBuilder_EntityBase copiedFloatingObject in this.m_copiedFloatingObjects)
        {
          if (!(MyEntities.CreateFromObjectBuilder(copiedFloatingObject, false) is MyFloatingObject fromObjectBuilder))
          {
            this.ChangeClipboardPreview(false);
            break;
          }
          this.MakeTransparent(fromObjectBuilder);
          this.IsActive = visible;
          this.m_visible = visible;
          MyEntities.Add((MyEntity) fromObjectBuilder);
          MyFloatingObjects.UnregisterFloatingObject(fromObjectBuilder);
          fromObjectBuilder.Save = false;
          fromObjectBuilder.IsPreview = true;
          this.DisablePhysicsRecursively((MyEntity) fromObjectBuilder);
          this.m_previewFloatingObjects.Add(fromObjectBuilder);
        }
      }
    }

    private void MakeTransparent(MyFloatingObject floatingObject)
    {
      floatingObject.Render.Transparency = 0.5f;
      MyEntitySubpart myEntitySubpart;
      if (!floatingObject.Subparts.TryGetValue(MyAutomaticRifleGun.NAME_SUBPART_MAGAZINE, out myEntitySubpart) || myEntitySubpart.Render == null)
        return;
      myEntitySubpart.Render.Transparency = 0.5f;
    }

    private void DisablePhysicsRecursively(MyEntity entity)
    {
      if (entity.Physics != null && entity.Physics.Enabled)
        entity.Physics.Enabled = false;
      if (entity is MyFloatingObject myFloatingObject)
        myFloatingObject.NeedsUpdate = MyEntityUpdateEnum.NONE;
      foreach (MyEntityComponentBase child in entity.Hierarchy.Children)
        this.DisablePhysicsRecursively(child.Container.Entity as MyEntity);
    }

    public void Update()
    {
      if (!this.IsActive || !this.m_visible)
        return;
      this.UpdateHitEntity();
      this.UpdatePastePosition();
      this.UpdateFloatingObjectTransformations();
      if (this.m_calculateVelocity)
        this.m_objectVelocity = (Vector3) ((this.m_pastePosition - this.m_pastePositionPrevious) / 0.0166666675359011);
      this.m_canBePlaced = this.TestPlacement();
      this.UpdatePreviewBBox();
      if (!MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE)
        return;
      MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, 0.0f), "FW: " + this.m_pasteDirForward.ToString(), Color.Red, 1f);
      MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, 20f), "UP: " + this.m_pasteDirUp.ToString(), Color.Red, 1f);
      MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, 40f), "AN: " + this.m_pasteOrientationAngle.ToString(), Color.Red, 1f);
    }

    private void UpdateHitEntity()
    {
      MatrixD pasteMatrix = MyFloatingObjectClipboard.GetPasteMatrix();
      MyPhysics.CastRay(pasteMatrix.Translation, pasteMatrix.Translation + pasteMatrix.Forward * (double) this.m_dragDistance, this.m_raycastCollisionResults);
      this.m_closestHitDistSq = float.MaxValue;
      this.m_hitPos = new Vector3D(0.0, 0.0, 0.0);
      this.m_hitNormal = new Vector3(1f, 0.0f, 0.0f);
      foreach (MyPhysics.HitInfo raycastCollisionResult in this.m_raycastCollisionResults)
      {
        MyPhysicsBody userObject = (MyPhysicsBody) raycastCollisionResult.HkHitInfo.Body.UserObject;
        if (userObject != null)
        {
          IMyEntity entity = userObject.Entity;
          if (entity is MyVoxelMap || entity is MyCubeGrid && entity.EntityId != this.m_previewFloatingObjects[0].EntityId)
          {
            float num = (float) (raycastCollisionResult.Position - pasteMatrix.Translation).LengthSquared();
            if ((double) num < (double) this.m_closestHitDistSq)
            {
              this.m_closestHitDistSq = num;
              this.m_hitPos = raycastCollisionResult.Position;
              this.m_hitNormal = raycastCollisionResult.HkHitInfo.Normal;
            }
          }
        }
      }
      this.m_raycastCollisionResults.Clear();
    }

    private bool TestPlacement()
    {
      if (this.m_previewFloatingObjects == null)
        return false;
      for (int index = 0; index < this.m_previewFloatingObjects.Count; ++index)
      {
        MyFloatingObject previewFloatingObject = this.m_previewFloatingObjects[index];
        if (previewFloatingObject != null && (!MyEntities.IsInsideWorld(previewFloatingObject.PositionComp.GetPosition()) || previewFloatingObject.Physics == null))
          return false;
      }
      for (int index = 0; index < this.m_previewFloatingObjects.Count; ++index)
      {
        MyFloatingObject previewFloatingObject = this.m_previewFloatingObjects[index];
        if (previewFloatingObject != null)
        {
          MatrixD matrix = previewFloatingObject.WorldMatrix;
          Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(in matrix);
          Vector3D translation = previewFloatingObject.PositionComp.GetPosition() + Vector3D.Transform((Vector3D) previewFloatingObject.PositionComp.LocalVolume.Center, fromRotationMatrix);
          List<HkBodyCollision> results = new List<HkBodyCollision>();
          MyPhysics.GetPenetrationsShape(previewFloatingObject.Physics.RigidBody.GetShape(), ref translation, ref fromRotationMatrix, results, 23);
          foreach (HkBodyCollision collision in results)
          {
            IMyEntity collisionEntity = collision.GetCollisionEntity();
            if (collisionEntity != null && !collisionEntity.Closed && (collisionEntity.Physics == null || !collisionEntity.Physics.IsPhantom))
              return false;
          }
        }
      }
      return true;
    }

    private void UpdateFloatingObjectTransformations()
    {
      Matrix orientationMatrix = this.GetFirstGridOrientationMatrix();
      MatrixD matrixD = (MatrixD) ref orientationMatrix;
      MatrixD matrix1 = MatrixD.Invert(this.m_copiedFloatingObjects[0].PositionAndOrientation.Value.GetMatrix()).GetOrientation() * matrixD;
      for (int index = 0; index < this.m_previewFloatingObjects.Count; ++index)
      {
        MatrixD matrix2 = this.m_copiedFloatingObjects[index].PositionAndOrientation.Value.GetMatrix();
        Vector3D normal = matrix2.Translation - (Vector3D) this.m_copiedFloatingObjects[0].PositionAndOrientation.Value.Position;
        this.m_copiedFloatingObjectOffsets[index] = Vector3D.TransformNormal(normal, matrix1);
        Vector3D vector3D = this.m_pastePosition + this.m_copiedFloatingObjectOffsets[index];
        MatrixD rotationMatrix = matrix2 * matrix1;
        rotationMatrix.Translation = Vector3D.Zero;
        MatrixD worldMatrix = MatrixD.Orthogonalize(rotationMatrix);
        worldMatrix.Translation = vector3D;
        this.m_previewFloatingObjects[index].PositionComp.SetWorldMatrix(ref worldMatrix);
      }
    }

    private void UpdatePastePosition()
    {
      this.m_pastePositionPrevious = this.m_pastePosition;
      MatrixD pasteMatrix = MyFloatingObjectClipboard.GetPasteMatrix();
      Vector3D vector3D = pasteMatrix.Forward * (double) this.m_dragDistance;
      this.m_pastePosition = pasteMatrix.Translation + vector3D;
      Matrix orientationMatrix = this.GetFirstGridOrientationMatrix();
      this.m_pastePosition += Vector3D.TransformNormal(this.m_dragPointToPositionLocal, (MatrixD) ref orientationMatrix);
      if (!MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE)
        return;
      MyRenderProxy.DebugDrawSphere(pasteMatrix.Translation + vector3D, 0.15f, Color.Pink, depthRead: false);
      MyRenderProxy.DebugDrawSphere(this.m_pastePosition, 0.15f, (Color) Color.Pink.ToVector3(), depthRead: false);
    }

    private static MatrixD GetPasteMatrix() => MySession.Static.ControlledEntity != null && (MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Entity || MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.ThirdPersonSpectator) ? MySession.Static.ControlledEntity.GetHeadMatrix(true) : MySector.MainCamera.WorldMatrix;

    private Matrix GetFirstGridOrientationMatrix() => Matrix.CreateWorld(Vector3.Zero, this.m_pasteDirForward, this.m_pasteDirUp) * Matrix.CreateFromAxisAngle(this.m_pasteDirUp, this.m_pasteOrientationAngle);

    private void UpdatePreviewBBox()
    {
      if (this.m_previewFloatingObjects == null)
        return;
      if (!this.m_visible)
      {
        foreach (MyEntity previewFloatingObject in this.m_previewFloatingObjects)
          MyEntities.EnableEntityBoundingBoxDraw(previewFloatingObject, false);
      }
      else
      {
        Vector4 vector4;
        ref Vector4 local = ref vector4;
        Color color = Color.Red;
        Vector3 vector3_1 = color.ToVector3() * 0.8f;
        local = new Vector4(vector3_1, 1f);
        if (this.m_canBePlaced)
        {
          color = Color.Gray;
          vector4 = color.ToVector4();
        }
        Vector3 vector3_2 = new Vector3(0.1f);
        foreach (MyEntity previewFloatingObject in this.m_previewFloatingObjects)
          MyEntities.EnableEntityBoundingBoxDraw(previewFloatingObject, true, new Vector4?(vector4), inflateAmount: new Vector3?(vector3_2));
      }
    }

    public void CalculateRotationHints(MyBlockBuilderRotationHints hints, bool isRotating)
    {
      MyObjectBuilder_FloatingObject builderFloatingObject = this.m_copiedFloatingObjects.Count > 0 ? this.m_copiedFloatingObjects[0] : (MyObjectBuilder_FloatingObject) null;
      if (builderFloatingObject == null)
        return;
      MatrixD matrix = builderFloatingObject.PositionAndOrientation.Value.GetMatrix();
      Vector3D vector3D = Vector3D.TransformNormal(-this.m_dragPointToPositionLocal, matrix);
      matrix.Translation += vector3D;
      hints.CalculateRotationHints(matrix, !MyHud.MinimalHud && !MyHud.CutsceneHud && MySandboxGame.Config.RotationHints && !MyInput.Static.IsJoystickLastUsed, isRotating);
    }

    public bool HasCopiedFloatingObjects() => this.m_copiedFloatingObjects.Count > 0;

    public string CopiedGridsName => this.HasCopiedFloatingObjects() ? this.m_copiedFloatingObjects[0].Name : (string) null;

    public void HideWhenColliding(List<Vector3D> collisionTestPoints)
    {
      if (this.m_previewFloatingObjects.Count == 0)
        return;
      bool flag = true;
      foreach (Vector3D collisionTestPoint in collisionTestPoints)
      {
        foreach (MyFloatingObject previewFloatingObject in this.m_previewFloatingObjects)
        {
          Vector3D point = Vector3.Transform((Vector3) collisionTestPoint, previewFloatingObject.PositionComp.WorldMatrixNormalizedInv);
          if (previewFloatingObject.PositionComp.LocalAABB.Contains(point) == ContainmentType.Contains)
          {
            flag = false;
            break;
          }
        }
        if (!flag)
          break;
      }
      foreach (MyEntity previewFloatingObject in this.m_previewFloatingObjects)
        previewFloatingObject.Render.Visible = flag;
    }

    public void ClearClipboard()
    {
      if (this.IsActive)
        this.Deactivate();
      this.m_copiedFloatingObjects.Clear();
      this.m_copiedFloatingObjectOffsets.Clear();
    }

    public void RotateAroundAxis(int axisIndex, int sign, bool newlyPressed, float angleDelta)
    {
      switch (axisIndex)
      {
        case 0:
          if (sign < 0)
          {
            this.UpMinus(angleDelta);
            break;
          }
          this.UpPlus(angleDelta);
          break;
        case 1:
          if (sign < 0)
          {
            this.AngleMinus(angleDelta);
            break;
          }
          this.AnglePlus(angleDelta);
          break;
        case 2:
          if (sign < 0)
          {
            this.RightPlus(angleDelta);
            break;
          }
          this.RightMinus(angleDelta);
          break;
      }
    }

    private void AnglePlus(float angle)
    {
      this.m_pasteOrientationAngle += angle;
      if ((double) this.m_pasteOrientationAngle < 6.28318548202515)
        return;
      this.m_pasteOrientationAngle -= 6.283185f;
    }

    private void AngleMinus(float angle)
    {
      this.m_pasteOrientationAngle -= angle;
      if ((double) this.m_pasteOrientationAngle >= 0.0)
        return;
      this.m_pasteOrientationAngle += 6.283185f;
    }

    private void UpPlus(float angle)
    {
      this.ApplyOrientationAngle();
      Vector3.Cross(this.m_pasteDirForward, this.m_pasteDirUp);
      float num1 = (float) Math.Cos((double) angle);
      float num2 = (float) Math.Sin((double) angle);
      Vector3 vector3 = this.m_pasteDirUp * num1 - this.m_pasteDirForward * num2;
      this.m_pasteDirForward = this.m_pasteDirUp * num2 + this.m_pasteDirForward * num1;
      this.m_pasteDirUp = vector3;
    }

    private void UpMinus(float angle) => this.UpPlus(-angle);

    private void RightPlus(float angle)
    {
      this.ApplyOrientationAngle();
      Vector3 vector3 = Vector3.Cross(this.m_pasteDirForward, this.m_pasteDirUp);
      float num1 = (float) Math.Cos((double) angle);
      float num2 = (float) Math.Sin((double) angle);
      this.m_pasteDirUp = this.m_pasteDirUp * num1 + vector3 * num2;
    }

    private void RightMinus(float angle) => this.RightPlus(-angle);

    public void MoveEntityFurther() => this.m_dragDistance *= 1.1f;

    public void MoveEntityCloser() => this.m_dragDistance /= 1.1f;

    private void ApplyOrientationAngle()
    {
      this.m_pasteDirForward = Vector3.Normalize(this.m_pasteDirForward);
      this.m_pasteDirUp = Vector3.Normalize(this.m_pasteDirUp);
      Vector3 vector3 = Vector3.Cross(this.m_pasteDirForward, this.m_pasteDirUp);
      float num1 = (float) Math.Cos((double) this.m_pasteOrientationAngle);
      float num2 = (float) Math.Sin((double) this.m_pasteOrientationAngle);
      this.m_pasteDirForward = this.m_pasteDirForward * num1 - vector3 * num2;
      this.m_pasteOrientationAngle = 0.0f;
    }
  }
}
