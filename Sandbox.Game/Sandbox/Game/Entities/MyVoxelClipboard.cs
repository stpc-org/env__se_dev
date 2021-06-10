// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyVoxelClipboard
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
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
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
  internal class MyVoxelClipboard
  {
    private List<MyObjectBuilder_EntityBase> m_copiedVoxelMaps = new List<MyObjectBuilder_EntityBase>();
    private List<VRage.Game.Voxels.IMyStorage> m_copiedStorages = new List<VRage.Game.Voxels.IMyStorage>();
    private List<Vector3> m_copiedVoxelMapOffsets = new List<Vector3>();
    private List<MyVoxelBase> m_previewVoxelMaps = new List<MyVoxelBase>();
    protected float m_pasteOrientationAngle;
    protected Vector3 m_pasteDirUp = new Vector3(1f, 0.0f, 0.0f);
    protected Vector3 m_pasteDirForward = new Vector3(0.0f, 1f, 0.0f);
    private Vector3D m_pastePosition;
    private float m_dragDistance;
    private Vector3 m_dragPointToPositionLocal;
    private bool m_canBePlaced;
    private MyEntity m_blockingEntity;
    private bool m_visible = true;
    private bool m_shouldMarkForClose = true;
    private bool m_planetMode;
    private static readonly MyStringId ID_GIZMO_DRAW_LINE = MyStringId.GetOrCompute("GizmoDrawLine");
    private List<MyEntity> m_tmpResultList = new List<MyEntity>();

    public bool IsActive { get; private set; }

    private void Activate()
    {
      this.ChangeClipboardPreview(true);
      this.IsActive = true;
    }

    public void Deactivate()
    {
      this.ChangeClipboardPreview(false);
      this.IsActive = false;
      this.m_planetMode = false;
    }

    public void Hide()
    {
      this.ChangeClipboardPreview(false);
      this.m_planetMode = false;
    }

    public void Show()
    {
      if (!this.IsActive || this.m_previewVoxelMaps.Count != 0)
        return;
      this.ChangeClipboardPreview(true);
    }

    public void ClearClipboard()
    {
      if (this.IsActive)
        this.Deactivate();
      this.m_copiedVoxelMapOffsets.Clear();
      this.m_copiedVoxelMaps.Clear();
    }

    public void CutVoxelMap(MyVoxelBase voxelMap)
    {
      if (voxelMap == null)
        return;
      this.CopyVoxelMap(voxelMap);
      MyEntities.SendCloseRequest((IMyEntity) voxelMap);
      this.Deactivate();
    }

    public void CopyVoxelMap(MyVoxelBase voxelMap)
    {
      if (voxelMap == null)
        return;
      this.m_copiedVoxelMaps.Clear();
      this.m_copiedVoxelMapOffsets.Clear();
      this.CopyVoxelMapInternal(voxelMap);
      this.Activate();
    }

    private void CopyVoxelMapInternal(MyVoxelBase toCopy)
    {
      this.m_copiedVoxelMaps.Add(toCopy.GetObjectBuilder(true));
      MatrixD worldMatrix;
      if (this.m_copiedVoxelMaps.Count == 1)
      {
        MatrixD pasteMatrix = MyVoxelClipboard.GetPasteMatrix();
        worldMatrix = toCopy.WorldMatrix;
        Vector3D translation = worldMatrix.Translation;
        this.m_dragPointToPositionLocal = (Vector3) Vector3D.TransformNormal(toCopy.PositionComp.GetPosition() - translation, toCopy.PositionComp.WorldMatrixNormalizedInv);
        this.m_dragDistance = (float) (translation - pasteMatrix.Translation).Length();
        worldMatrix = toCopy.WorldMatrix;
        this.m_pasteDirUp = (Vector3) worldMatrix.Up;
        worldMatrix = toCopy.WorldMatrix;
        this.m_pasteDirForward = (Vector3) worldMatrix.Forward;
        this.m_pasteOrientationAngle = 0.0f;
      }
      List<Vector3> copiedVoxelMapOffsets = this.m_copiedVoxelMapOffsets;
      worldMatrix = toCopy.WorldMatrix;
      Vector3 vector3 = (Vector3) (worldMatrix.Translation - (Vector3D) this.m_copiedVoxelMaps[0].PositionAndOrientation.Value.Position);
      copiedVoxelMapOffsets.Add(vector3);
    }

    public bool PasteVoxelMap(MyInventory buildInventory = null)
    {
      if (this.m_planetMode)
      {
        if (!this.m_canBePlaced)
        {
          MyHud.Notifications.Add(MyNotificationSingletons.CopyPasteAsteoridObstructed);
          MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
          return false;
        }
        MyEntities.RemapObjectBuilderCollection((IEnumerable<MyObjectBuilder_EntityBase>) this.m_copiedVoxelMaps);
        for (int index = 0; index < this.m_copiedVoxelMaps.Count; ++index)
          MyGuiScreenDebugSpawnMenu.SpawnPlanet(this.m_pastePosition - this.m_copiedVoxelMapOffsets[index]);
        this.Deactivate();
        return true;
      }
      if (this.m_copiedVoxelMaps.Count == 0)
        return false;
      if (this.m_copiedVoxelMaps.Count > 0 && !this.IsActive)
      {
        this.Activate();
        return true;
      }
      if (!this.m_canBePlaced)
      {
        MyHud.Notifications.Add(MyNotificationSingletons.CopyPasteAsteoridObstructed);
        MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
        return false;
      }
      MyGuiAudio.PlaySound(MyGuiSounds.HudPlaceBlock);
      MyGuiScreenDebugSpawnMenu.RecreateAsteroidBeforePaste((float) this.m_previewVoxelMaps[0].PositionComp.GetPosition().Length());
      MyEntities.RemapObjectBuilderCollection((IEnumerable<MyObjectBuilder_EntityBase>) this.m_copiedVoxelMaps);
      foreach (MyVoxelBase previewVoxelMap in this.m_previewVoxelMaps)
      {
        if (Sync.IsServer)
        {
          previewVoxelMap.CreatedByUser = true;
          previewVoxelMap.AsteroidName = MyGuiScreenDebugSpawnMenu.GetAsteroidName();
          this.EnablePhysicsRecursively((MyEntity) previewVoxelMap);
          previewVoxelMap.Save = true;
          this.MakeVisible(previewVoxelMap);
          this.m_shouldMarkForClose = false;
          MyEntities.RaiseEntityCreated((MyEntity) previewVoxelMap);
        }
        else
        {
          this.m_shouldMarkForClose = true;
          MyGuiScreenDebugSpawnMenu.SpawnAsteroid(previewVoxelMap.PositionComp.WorldMatrix);
        }
        previewVoxelMap.AfterPaste();
      }
      this.Deactivate();
      return true;
    }

    public void SetVoxelMapFromBuilder(
      MyObjectBuilder_EntityBase voxelMap,
      VRage.Game.Voxels.IMyStorage storage,
      Vector3 dragPointDelta,
      float dragVectorLength)
    {
      if (this.IsActive)
        this.Deactivate();
      this.m_copiedVoxelMaps.Clear();
      this.m_copiedVoxelMapOffsets.Clear();
      this.m_copiedStorages.Clear();
      this.m_dragPointToPositionLocal = dragPointDelta;
      this.m_dragDistance = dragVectorLength;
      Vector3 offset = Vector3.Zero;
      if (voxelMap is MyObjectBuilder_Planet)
        offset = storage.Size / 2f;
      this.SetVoxelMapFromBuilderInternal(voxelMap, storage, offset);
      this.Activate();
    }

    private void SetVoxelMapFromBuilderInternal(
      MyObjectBuilder_EntityBase voxelMap,
      VRage.Game.Voxels.IMyStorage storage,
      Vector3 offset)
    {
      this.m_copiedVoxelMaps.Add(voxelMap);
      this.m_copiedStorages.Add(storage);
      this.m_copiedVoxelMapOffsets.Add(offset);
    }

    private void ChangeClipboardPreview(bool visible)
    {
      if (this.m_copiedVoxelMaps.Count == 0 || !visible)
      {
        foreach (MyVoxelBase previewVoxelMap in this.m_previewVoxelMaps)
        {
          MyEntities.EnableEntityBoundingBoxDraw((MyEntity) previewVoxelMap, false);
          if (this.m_shouldMarkForClose)
            previewVoxelMap.Close();
        }
        this.m_previewVoxelMaps.Clear();
        this.m_visible = false;
      }
      else
      {
        MyEntities.RemapObjectBuilderCollection((IEnumerable<MyObjectBuilder_EntityBase>) this.m_copiedVoxelMaps);
        for (int index = 0; index < this.m_copiedVoxelMaps.Count; ++index)
        {
          MyObjectBuilder_EntityBase copiedVoxelMap = this.m_copiedVoxelMaps[index];
          VRage.Game.Voxels.IMyStorage copiedStorage = this.m_copiedStorages[index];
          MyVoxelBase voxelMap = (MyVoxelBase) null;
          if (copiedVoxelMap is MyObjectBuilder_VoxelMap)
            voxelMap = (MyVoxelBase) new MyVoxelMap();
          if (copiedVoxelMap is MyObjectBuilder_Planet)
          {
            this.m_planetMode = true;
            this.IsActive = visible;
            this.m_visible = visible;
          }
          else
          {
            MyPositionAndOrientation positionAndOrientation = copiedVoxelMap.PositionAndOrientation.Value;
            voxelMap.Init(copiedVoxelMap, copiedStorage);
            voxelMap.BeforePaste();
            this.DisablePhysicsRecursively((MyEntity) voxelMap);
            this.MakeTransparent(voxelMap);
            MyEntities.Add((MyEntity) voxelMap);
            voxelMap.PositionLeftBottomCorner = this.m_pastePosition - voxelMap.Storage.Size * 0.5f;
            voxelMap.PositionComp.WorldMatrix = this.GetFirstOrientationMatrix();
            voxelMap.PositionComp.SetPosition(this.m_pastePosition);
            voxelMap.Save = false;
            this.m_previewVoxelMaps.Add(voxelMap);
            this.IsActive = visible;
            this.m_visible = visible;
            this.m_shouldMarkForClose = true;
          }
        }
      }
    }

    private void MakeTransparent(MyVoxelBase voxelMap) => voxelMap.Render.Transparency = 0.25f;

    private void MakeVisible(MyVoxelBase voxelMap)
    {
      voxelMap.Render.Transparency = 0.0f;
      voxelMap.Render.UpdateTransparency();
    }

    private void DisablePhysicsRecursively(MyEntity entity)
    {
      if (entity.Physics != null && entity.Physics.Enabled)
        entity.Physics.Enabled = false;
      foreach (MyEntityComponentBase child in entity.Hierarchy.Children)
        this.DisablePhysicsRecursively(child.Container.Entity as MyEntity);
    }

    private void EnablePhysicsRecursively(MyEntity entity)
    {
      if (entity.Physics != null && !entity.Physics.Enabled)
        entity.Physics.Enabled = true;
      foreach (MyEntityComponentBase child in entity.Hierarchy.Children)
        this.EnablePhysicsRecursively(child.Container.Entity as MyEntity);
    }

    public void Update()
    {
      if (!this.IsActive || !this.m_visible)
        return;
      this.UpdatePastePosition();
      this.UpdateVoxelMapTransformations();
      this.m_canBePlaced = this.TestPlacement();
    }

    private void UpdateVoxelMapTransformations()
    {
      Color color = this.m_canBePlaced ? Color.Green : Color.Red;
      if (this.m_planetMode)
      {
        for (int index = 0; index < this.m_copiedVoxelMaps.Count; ++index)
        {
          if (this.m_copiedVoxelMaps[index] is MyObjectBuilder_Planet copiedVoxelMap)
            MyRenderProxy.DebugDrawSphere(this.m_pastePosition, copiedVoxelMap.Radius * 1.1f, color);
        }
      }
      else
      {
        for (int index = 0; index < this.m_previewVoxelMaps.Count; ++index)
        {
          if (this.m_previewVoxelMaps[index] != null && this.m_previewVoxelMaps[index].PositionComp != null)
          {
            this.m_previewVoxelMaps[index].PositionComp.WorldMatrix = this.GetFirstOrientationMatrix();
            this.m_previewVoxelMaps[index].PositionLeftBottomCorner = this.m_pastePosition + this.m_copiedVoxelMapOffsets[index] - this.m_previewVoxelMaps[index].Storage.Size * 0.5f;
            this.m_previewVoxelMaps[index].PositionComp.SetPosition(this.m_pastePosition + this.m_copiedVoxelMapOffsets[index]);
            MatrixD worldMatrix = this.m_previewVoxelMaps[index].PositionComp.WorldMatrixRef;
            BoundingBoxD localbox = new BoundingBoxD((Vector3D) (-this.m_previewVoxelMaps[index].Storage.Size * 0.5f), (Vector3D) (this.m_previewVoxelMaps[index].Storage.Size * 0.5f));
            MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localbox, ref color, MySimpleObjectRasterizer.Wireframe, 1, 0.04f, lineMaterial: new MyStringId?(MyVoxelClipboard.ID_GIZMO_DRAW_LINE), blendType: MyBillboard.BlendTypeEnum.LDR);
          }
        }
      }
    }

    private void UpdatePastePosition()
    {
      MatrixD pasteMatrix = MyVoxelClipboard.GetPasteMatrix();
      Vector3D vector3D = pasteMatrix.Forward * (double) this.m_dragDistance;
      this.m_pastePosition = pasteMatrix.Translation + vector3D;
      if (!MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE)
        return;
      MyRenderProxy.DebugDrawSphere(this.m_pastePosition, 0.15f, (Color) Color.Pink.ToVector3(), depthRead: false);
    }

    private bool TestPlacement()
    {
      if (!MyEntities.IsInsideWorld(this.m_pastePosition))
        return false;
      if (MySession.Static.ControlledEntity != null && (MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Entity || MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.ThirdPersonSpectator || MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Spectator))
      {
        for (int index = 0; index < this.m_previewVoxelMaps.Count; ++index)
        {
          if (!MyEntities.IsInsideWorld(this.m_previewVoxelMaps[index].PositionComp.GetPosition()))
            return false;
          BoundingBoxD worldAabb = this.m_previewVoxelMaps[index].PositionComp.WorldAABB;
          using (this.m_tmpResultList.GetClearToken<MyEntity>())
          {
            MyGamePruningStructure.GetTopMostEntitiesInBox(ref worldAabb, this.m_tmpResultList);
            if (!this.TestPlacement(this.m_tmpResultList))
              return false;
          }
        }
        if (this.m_planetMode)
        {
          for (int index = 0; index < this.m_copiedVoxelMaps.Count; ++index)
          {
            if (this.m_copiedVoxelMaps[index] is MyObjectBuilder_Planet copiedVoxelMap)
            {
              using (this.m_tmpResultList.GetClearToken<MyEntity>())
              {
                BoundingSphereD sphere = new BoundingSphereD(this.m_pastePosition, (double) copiedVoxelMap.Radius * 1.10000002384186);
                MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref sphere, this.m_tmpResultList);
                if (!this.TestPlacement(this.m_tmpResultList))
                  return false;
              }
            }
          }
        }
      }
      return true;
    }

    private bool TestPlacement(List<MyEntity> entities)
    {
      foreach (MyEntity entity in entities)
      {
        switch (entity)
        {
          case MyVoxelBase _:
            continue;
          case MyCubeGrid _:
            if ((entity as MyCubeGrid).IsStatic)
              continue;
            break;
        }
        entities.Clear();
        return false;
      }
      return true;
    }

    private static MatrixD GetPasteMatrix() => MySession.Static.ControlledEntity != null && (MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Entity || MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.ThirdPersonSpectator) ? MySession.Static.ControlledEntity.GetHeadMatrix(true) : MySector.MainCamera.WorldMatrix;

    public void CalculateRotationHints(MyBlockBuilderRotationHints hints, bool isRotating)
    {
      MyVoxelBase myVoxelBase = this.m_previewVoxelMaps.Count > 0 ? this.m_previewVoxelMaps[0] : (MyVoxelBase) null;
      if (myVoxelBase == null)
        return;
      MatrixD worldMatrix = myVoxelBase.WorldMatrix;
      Vector3D vector3D = Vector3D.TransformNormal(-this.m_dragPointToPositionLocal, worldMatrix);
      worldMatrix.Translation += vector3D;
      hints.CalculateRotationHints(worldMatrix, !MyHud.MinimalHud && !MyHud.CutsceneHud && MySandboxGame.Config.RotationHints && !MyInput.Static.IsJoystickLastUsed, isRotating);
    }

    public MatrixD GetFirstOrientationMatrix() => MatrixD.CreateWorld(Vector3D.Zero, this.m_pasteDirForward, this.m_pasteDirUp);

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
      this.ApplyOrientationAngle();
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

    public void MoveEntityFurther() => this.m_dragDistance *= 1.1f;

    public void MoveEntityCloser() => this.m_dragDistance /= 1.1f;
  }
}
