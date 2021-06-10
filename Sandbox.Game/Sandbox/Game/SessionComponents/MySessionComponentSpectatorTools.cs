// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MySessionComponentSpectatorTools
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Input;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
  [StaticEventOwner]
  public class MySessionComponentSpectatorTools : MySessionComponentBase, IMySpectatorTools
  {
    private const int MAX_SLOTS = 10;
    private static MySessionComponentSpectatorTools m_instance;
    public MyCameraMode m_lockMode;
    private MyLockEntityState[] m_trackedSlots = new MyLockEntityState[10];
    private int m_selectedSlot = -1;
    private MyLockEntityState m_cameraState;
    private Vector3 m_smoothMouseDelta = (Vector3) Vector3D.Zero;
    private Vector2 m_smoothMouse = Vector2.Zero;
    private float m_smoothRoll;

    public float SmoothCameraLERP { get; set; } = 0.9f;

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      base.Init(sessionComponent);
      MySessionComponentSpectatorTools.m_instance = this;
      for (int index = 0; index > 10; ++index)
        this.Clear(ref this.m_trackedSlots[index]);
      this.Clear(ref this.m_cameraState);
    }

    public override bool IsRequiredByGame => true;

    public void SetTarget(IMyEntity ent) => this.UpdateLockEntity(ent);

    public IMyEntity GetTarget()
    {
      MyEntity entity;
      return MyEntities.TryGetEntityById(this.m_cameraState.LockEntityID, out entity) ? (IMyEntity) entity : (IMyEntity) null;
    }

    public void SetMode(MyCameraMode mode) => this.m_lockMode = mode;

    public MyCameraMode GetMode() => this.m_lockMode;

    public IReadOnlyList<MyLockEntityState> TrackedSlots => (IReadOnlyList<MyLockEntityState>) this.m_trackedSlots;

    public override void UpdateAfterSimulation()
    {
      if (MySession.Static == null || Sync.IsDedicated || !(MySession.Static.CameraController is MySpectator))
        return;
      MySpectator cameraController = (MySpectator) MySession.Static.CameraController;
      IMyEntity target = this.GetTarget();
      IMyCharacter myCharacter = target as IMyCharacter;
      if (target == null && this.m_cameraState.LockEntityID != -1L)
      {
        this.m_lockMode = MyCameraMode.None;
        this.Clear(ref this.m_cameraState);
      }
      if (target != null && target.Physics != null && (cameraController != null && !MyAPIGateway.Session.IsCameraControlledObject))
      {
        Vector3 vector3_1 = MyAPIGateway.Input.GetPositionDelta();
        Vector2 vector2 = MyAPIGateway.Input.GetRotation() * (1f / 400f) * cameraController.SpeedModeAngular;
        float angle = MyAPIGateway.Input.GetRoll() * 0.0125f * cameraController.SpeedModeAngular;
        if (!MyAPIGateway.Session.IsCameraUserControlledSpectator || MyAPIGateway.Gui.ChatEntryVisible || (MyAPIGateway.Gui.IsCursorVisible || MyAPIGateway.Gui.GetCurrentScreen != MyTerminalPageEnum.None))
        {
          vector3_1 = (Vector3) Vector3D.Zero;
          vector2 = Vector2.Zero;
          angle = 0.0f;
        }
        if (MyAPIGateway.Session.IsCameraUserControlledSpectator)
        {
          float amount = 1f - MathHelper.Lerp(0.01f, this.SmoothCameraLERP * 0.99f, MathHelper.Clamp(this.SmoothCameraLERP * 4f, 0.0f, 1f));
          this.m_smoothMouseDelta += vector3_1;
          vector3_1 = (Vector3) Vector3D.Lerp(Vector3D.Zero, (Vector3D) this.m_smoothMouseDelta, (double) amount);
          this.m_smoothMouseDelta -= vector3_1;
          this.m_smoothMouse += vector2;
          vector2 = Vector2.Lerp(Vector2.Zero, this.m_smoothMouse, amount);
          this.m_smoothMouse -= vector2;
          this.m_smoothRoll += angle;
          angle = this.m_smoothRoll * amount;
          this.m_smoothRoll -= angle;
        }
        else
        {
          this.m_smoothMouse = Vector2.Zero;
          this.m_smoothMouseDelta = Vector3.Zero;
          this.m_smoothRoll = 0.0f;
        }
        Vector3 vector3_2 = vector3_1 * cameraController.SpeedModeLinear;
        if (MyAPIGateway.Session.IsCameraUserControlledSpectator)
        {
          Vector3D xOut;
          Vector3D vector3D;
          if ((double) angle != 0.0)
          {
            MyUtils.VectorPlaneRotation(this.m_cameraState.LocalMatrix.Up, this.m_cameraState.LocalMatrix.Right, out xOut, out vector3D, angle);
            this.m_cameraState.LocalMatrix.Right = vector3D;
            this.m_cameraState.LocalMatrix.Up = xOut;
          }
          Vector3D yOut;
          if ((double) vector2.Y != 0.0)
          {
            MyUtils.VectorPlaneRotation(this.m_cameraState.LocalMatrix.Right, this.m_cameraState.LocalMatrix.Forward, out vector3D, out yOut, -vector2.Y);
            this.m_cameraState.LocalMatrix.Right = vector3D;
            this.m_cameraState.LocalMatrix.Forward = yOut;
          }
          if ((double) vector2.X != 0.0)
          {
            MyUtils.VectorPlaneRotation(this.m_cameraState.LocalMatrix.Up, this.m_cameraState.LocalMatrix.Forward, out xOut, out yOut, vector2.X);
            this.m_cameraState.LocalMatrix.Up = xOut;
            this.m_cameraState.LocalMatrix.Forward = yOut;
          }
        }
        switch (this.m_lockMode)
        {
          case MyCameraMode.Free:
            Vector3D vector3D1 = (double) vector3_2.X * MySector.MainCamera.WorldMatrix.Right + (double) vector3_2.Y * MySector.MainCamera.WorldMatrix.Up + (double) vector3_2.Z * MySector.MainCamera.WorldMatrix.Backward;
            MatrixD worldMatrix1 = target.WorldMatrix;
            cameraController.Position = myCharacter == null ? target.WorldVolume.Center + this.m_cameraState.LocalVector + vector3D1 : myCharacter.GetHeadMatrix(true).Translation + this.m_cameraState.LocalVector + vector3D1;
            break;
          case MyCameraMode.Follow:
            this.m_cameraState.LocalMatrix.Translation += (double) vector3_2.X * this.m_cameraState.LocalMatrix.Right + (double) vector3_2.Y * this.m_cameraState.LocalMatrix.Up + (double) vector3_2.Z * this.m_cameraState.LocalMatrix.Backward;
            MatrixD worldMatrix2 = target.WorldMatrix;
            if (myCharacter != null)
              worldMatrix2 = myCharacter.GetHeadMatrix(true);
            MatrixD world = this.LocalToWorld(this.m_cameraState.LocalMatrix, worldMatrix2);
            cameraController.Position = world.Translation;
            cameraController.SetTarget(cameraController.Position + world.Forward, new Vector3D?(world.Up));
            break;
          case MyCameraMode.Orbit:
            Vector3D vector3D2 = target.WorldVolume.Center;
            if (myCharacter != null)
              vector3D2 = myCharacter.GetHeadMatrix(true).Translation;
            this.m_cameraState.LocalDistance = Math.Max(this.m_cameraState.LocalDistance + (double) vector3_2.Z, target.WorldVolume.Radius);
            MatrixD matrixD = myCharacter != null ? this.LocalToWorld(this.m_cameraState.LocalMatrix, myCharacter.GetHeadMatrix(true)) : this.LocalToWorld(this.m_cameraState.LocalMatrix, target.WorldMatrix);
            cameraController.Position = vector3D2 - matrixD.Forward * this.m_cameraState.LocalDistance;
            cameraController.SetTarget(vector3D2 + matrixD.Forward, new Vector3D?(matrixD.Up));
            break;
        }
        MatrixD orientation = cameraController.Orientation;
        orientation.Translation = cameraController.Position;
        if (myCharacter != null)
        {
          MatrixD headMatrix = myCharacter.GetHeadMatrix(true);
          this.m_cameraState.LocalMatrix = this.WorldToLocal(orientation, headMatrix);
          this.m_cameraState.LocalVector = cameraController.Position - headMatrix.Translation;
        }
        else
        {
          this.m_cameraState.LocalMatrix = this.WorldToLocalNI(orientation, target.WorldMatrixNormalizedInv);
          this.m_cameraState.LocalVector = cameraController.Position - target.WorldVolume.Center;
        }
        if (this.m_lockMode != MyCameraMode.Orbit)
          this.m_cameraState.LocalDistance = this.m_cameraState.LocalVector.Length();
        this.m_cameraState.LastKnownPosition = target.GetPosition();
      }
      if (this.m_cameraState.LockEntityID == -1L || !(MySession.Static.CameraController is MySpectator) || (MyAPIGateway.Session.IsCameraControlledObject || MyAPIGateway.Session.IsCameraControlledObject) || MyHud.IsHudMinimal)
        return;
      string font = "GameCredits";
      MyGuiManager.DrawString(font, "Spectating " + this.m_cameraState.LockEntityDisplayName, new Vector2(0.5f, 0.02f), 0.9f, new Color?(Color.White), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
      MyGuiManager.DrawString(font, this.m_lockMode.ToString() + " mode", new Vector2(0.5f, 0.05f), 0.6f, new Color?(Color.White), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
    }

    public void LockHitEntity()
    {
      if (this.m_cameraState.LockEntityID != -1L)
      {
        this.Clear(ref this.m_cameraState);
        this.m_lockMode = MyCameraMode.None;
      }
      else
      {
        Vector3D to = MySector.MainCamera.Position + MySector.MainCamera.WorldMatrix.Forward * 1000.0;
        IHitInfo hitInfo = (IHitInfo) null;
        MyAPIGateway.Physics.CastRay(MySector.MainCamera.Position, to, out hitInfo);
        if (hitInfo == null || !(hitInfo.HitEntity is IMyCubeGrid) && !(hitInfo.HitEntity is IMyCharacter) || hitInfo.HitEntity.Physics == null)
          return;
        this.UpdateLockEntity(hitInfo.HitEntity);
      }
    }

    public void ClearTrackedSlot(int slotIndex) => this.Clear(ref this.m_trackedSlots[this.m_selectedSlot]);

    public void SaveTrackedSlot(int slotIndex) => this.m_trackedSlots[slotIndex] = this.m_cameraState;

    public void SelectTrackedSlot(int slotIndex)
    {
      this.m_selectedSlot = slotIndex;
      this.m_cameraState = this.m_trackedSlots[this.m_selectedSlot];
      if (this.m_cameraState.LockEntityID == -1L || MyEntities.EntityExists(this.m_cameraState.LockEntityID))
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (x => new Action<long>(MySessionComponentSpectatorTools.EntityPositionRequest)), this.m_cameraState.LockEntityID);
    }

    public void SwitchMode()
    {
      switch (this.m_lockMode)
      {
        case MyCameraMode.None:
          this.m_lockMode = MyCameraMode.Free;
          break;
        case MyCameraMode.Free:
          this.m_lockMode = MyCameraMode.Follow;
          break;
        case MyCameraMode.Follow:
          this.m_lockMode = MyCameraMode.Orbit;
          break;
        case MyCameraMode.Orbit:
          this.m_lockMode = MyCameraMode.None;
          break;
      }
    }

    private void UpdateLockEntity(IMyEntity lockEntity)
    {
      this.Clear(ref this.m_cameraState);
      this.m_cameraState.LockEntityID = lockEntity.EntityId;
      this.m_cameraState.LockEntityDisplayName = lockEntity.DisplayName;
      this.m_lockMode = MyCameraMode.Free;
      IMyCharacter myCharacter = lockEntity as IMyCharacter;
      MatrixD orientation = MySector.MainCamera.WorldMatrix.GetOrientation();
      orientation.Translation = MySector.MainCamera.Position;
      if (myCharacter != null)
      {
        this.m_cameraState.LocalMatrix = this.WorldToLocal(orientation, myCharacter.GetHeadMatrix(true));
        this.m_cameraState.LocalVector = MySector.MainCamera.Position - myCharacter.GetHeadMatrix(true).Translation;
      }
      else
      {
        this.m_cameraState.LocalMatrix = this.WorldToLocalNI(orientation, lockEntity.WorldMatrixNormalizedInv);
        this.m_cameraState.LocalVector = MySector.MainCamera.Position - lockEntity.WorldVolume.Center;
      }
      this.m_cameraState.LocalDistance = this.m_cameraState.LocalVector.Length();
    }

    private void Clear(ref MyLockEntityState state)
    {
      state.LockEntityID = -1L;
      state.LockEntityDisplayName = (string) null;
      state.LastKnownPosition = Vector3D.Zero;
      this.ClearRelativePosition(ref state);
    }

    private void ClearRelativePosition(ref MyLockEntityState state)
    {
      state.LocalDistance = 5.0;
      state.LocalVector = state.LocalDistance * Vector3D.Backward;
      state.LocalMatrix = MatrixD.CreateTranslation(state.LocalVector);
    }

    private MatrixD WorldToLocal(MatrixD current, MatrixD worldMatrix) => current * MatrixD.Normalize(MatrixD.Invert(worldMatrix));

    private MatrixD WorldToLocalNI(MatrixD current, MatrixD worldMatrixNormalizedInv) => current * worldMatrixNormalizedInv;

    private MatrixD LocalToWorld(MatrixD local, MatrixD worldMatrix) => local * worldMatrix;

    public override void HandleInput()
    {
      base.HandleInput();
      if (!MySession.Static.IsCameraUserControlledSpectator() || MyAPIGateway.Gui.ChatEntryVisible || (MyAPIGateway.Gui.IsCursorVisible || MyAPIGateway.Gui.GetCurrentScreen != MyTerminalPageEnum.None))
        return;
      if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SPECTATOR_LOCK) || MyControllerHelper.IsControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.SPECTATOR_LOCK))
        this.LockHitEntity();
      if ((MyInput.Static.IsAnyCtrlKeyPressed() && MyInput.Static.IsKeyPress(MyKeys.F8) || MyControllerHelper.IsControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.SPECTATOR_FOCUS_PLAYER, MyControlStateType.PRESSED)) && MySession.Static.ControlledEntity != null)
      {
        MySpectator.Static.Position = MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition() + MySpectator.Static.ThirdPersonCameraDelta;
        MySpectator.Static.SetTarget(MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition(), new Vector3D?(MySession.Static.ControlledEntity.Entity.PositionComp.WorldMatrixRef.Up));
      }
      if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SPECTATOR_SWITCHMODE))
        this.SwitchMode();
      int slotIndex = -1;
      if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad0))
        slotIndex = 0;
      if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad1))
        slotIndex = 1;
      if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad2))
        slotIndex = 2;
      if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad3))
        slotIndex = 3;
      if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad4))
        slotIndex = 4;
      if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad5))
        slotIndex = 5;
      if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad6))
        slotIndex = 6;
      if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad7))
        slotIndex = 7;
      if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad8))
        slotIndex = 8;
      if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad9))
        slotIndex = 9;
      if (slotIndex != -1)
      {
        if (MyInput.Static.IsAnyCtrlKeyPressed())
          this.SaveTrackedSlot(slotIndex);
        else
          this.SelectTrackedSlot(slotIndex);
      }
      if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SPECTATOR_NEXTPLAYER))
        this.NextPlayer();
      if (!MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SPECTATOR_PREVPLAYER))
        return;
      this.PreviousPlayer();
    }

    public void NextPlayer() => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<int>((Func<IMyEventOwner, Action<int>>) (x => new Action<int>(MySessionComponentSpectatorTools.ChangePlayerRequest)), 1);

    public void PreviousPlayer() => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<int>((Func<IMyEventOwner, Action<int>>) (x => new Action<int>(MySessionComponentSpectatorTools.ChangePlayerRequest)), -1);

    protected override void UnloadData()
    {
      base.UnloadData();
      MyAPIGateway.SpectatorTools = (IMySpectatorTools) null;
    }

    [Event(null, 491)]
    [Reliable]
    [Server]
    private static void ChangePlayerRequest(int direction)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
      {
        (Sandbox.Engine.Multiplayer.MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        List<MyEntityList.MyEntityListInfoItem> entityList = MyEntityList.GetEntityList(MyEntityList.MyEntityTypeEnum.Characters);
        if (!MyEventContext.Current.IsLocallyInvoked)
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<List<MyEntityList.MyEntityListInfoItem>, int>((Func<IMyEventOwner, Action<List<MyEntityList.MyEntityListInfoItem>, int>>) (x => new Action<List<MyEntityList.MyEntityListInfoItem>, int>(MySessionComponentSpectatorTools.ChangePlayerResponse)), entityList, direction, MyEventContext.Current.Sender);
        else
          MySessionComponentSpectatorTools.ChangePlayerResponse(entityList, direction);
      }
    }

    [Event(null, 508)]
    [Reliable]
    [Server]
    private static void EntityPositionRequest(long entityId)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
      {
        (Sandbox.Engine.Multiplayer.MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        MyEntity entity;
        if (!MyEntities.TryGetEntityById(entityId, out entity))
          return;
        if (MyEventContext.Current.IsLocallyInvoked)
          MySessionComponentSpectatorTools.EntityPositionResponse(entityId, entity.PositionComp.GetPosition());
        else
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long, Vector3D>((Func<IMyEventOwner, Action<long, Vector3D>>) (x => new Action<long, Vector3D>(MySessionComponentSpectatorTools.EntityPositionResponse)), entityId, entity.PositionComp.GetPosition(), MyEventContext.Current.Sender);
      }
    }

    [Event(null, 534)]
    [Reliable]
    [Client]
    private static void ChangePlayerResponse(
      List<MyEntityList.MyEntityListInfoItem> entities,
      int direction)
    {
      if (entities == null)
        return;
      MyEntityList.SortEntityList(MyEntityList.MyEntitySortOrder.DisplayName, ref entities, false);
      MySessionComponentSpectatorTools.m_instance.ChangePlayer(entities, direction);
    }

    private void ChangePlayer(List<MyEntityList.MyEntityListInfoItem> entities, int direction)
    {
      if (MySession.Static == null)
        return;
      for (int index1 = 0; index1 < entities.Count; ++index1)
      {
        if (entities[index1].EntityId == MySession.Static.LocalCharacterEntityId)
        {
          int index2 = -1;
          switch (direction)
          {
            case -1:
              index2 = index1 != 0 ? index1 - 1 : entities.Count - 1;
              break;
            case 1:
              index2 = index1 >= entities.Count - 1 ? 0 : index1 + 1;
              break;
          }
          if (index2 == -1)
            break;
          this.Clear(ref this.m_cameraState);
          this.m_cameraState.LockEntityDisplayName = entities[index2].DisplayName;
          this.m_cameraState.LockEntityID = entities[index2].EntityId;
          this.m_lockMode = MyCameraMode.Follow;
          break;
        }
      }
    }

    [Event(null, 585)]
    [Reliable]
    [Client]
    private static void EntityPositionResponse(long entityId, Vector3D position) => MySessionComponentSpectatorTools.m_instance.UpdateEntityPosition(entityId, position);

    private void UpdateEntityPosition(long entityId, Vector3D position)
    {
      for (int index = 0; index < 10; ++index)
      {
        MyLockEntityState trackedSlot = this.m_trackedSlots[index];
        if (trackedSlot.LockEntityID == entityId)
        {
          trackedSlot.LastKnownPosition = position;
          this.m_trackedSlots[index] = trackedSlot;
        }
      }
      if (this.m_cameraState.LockEntityID != entityId)
        return;
      this.m_cameraState.LastKnownPosition = position;
    }

    protected sealed class ChangePlayerRequest\u003C\u003ESystem_Int32 : ICallSite<IMyEventOwner, int, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in int direction,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentSpectatorTools.ChangePlayerRequest(direction);
      }
    }

    protected sealed class EntityPositionRequest\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentSpectatorTools.EntityPositionRequest(entityId);
      }
    }

    protected sealed class ChangePlayerResponse\u003C\u003ESystem_Collections_Generic_List`1\u003CSandbox_Game_Entities_MyEntityList\u003C\u003EMyEntityListInfoItem\u003E\u0023System_Int32 : ICallSite<IMyEventOwner, List<MyEntityList.MyEntityListInfoItem>, int, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in List<MyEntityList.MyEntityListInfoItem> entities,
        in int direction,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentSpectatorTools.ChangePlayerResponse(entities, direction);
      }
    }

    protected sealed class EntityPositionResponse\u003C\u003ESystem_Int64\u0023VRageMath_Vector3D : ICallSite<IMyEventOwner, long, Vector3D, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in Vector3D position,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentSpectatorTools.EntityPositionResponse(entityId, position);
      }
    }
  }
}
