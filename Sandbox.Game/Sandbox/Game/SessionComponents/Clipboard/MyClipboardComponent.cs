// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.Clipboard.MyClipboardComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems.ContextHandling;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Components.Session;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components;
using VRage.Input;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.SessionComponents.Clipboard
{
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
  public class MyClipboardComponent : MySessionComponentBase, IMyFocusHolder
  {
    public static MyClipboardComponent Static;
    protected static readonly MyStringId[] m_rotationControls = new MyStringId[6]
    {
      MyControlsSpace.CUBE_ROTATE_VERTICAL_POSITIVE,
      MyControlsSpace.CUBE_ROTATE_VERTICAL_NEGATIVE,
      MyControlsSpace.CUBE_ROTATE_HORISONTAL_POSITIVE,
      MyControlsSpace.CUBE_ROTATE_HORISONTAL_NEGATIVE,
      MyControlsSpace.CUBE_ROTATE_ROLL_POSITIVE,
      MyControlsSpace.CUBE_ROTATE_ROLL_NEGATIVE
    };
    protected static readonly int[] m_rotationDirections = new int[6]
    {
      -1,
      1,
      1,
      -1,
      1,
      -1
    };
    private static MyClipboardDefinition m_definition;
    private static MyGridClipboard m_clipboard;
    private int m_currentGamepadRotationAxis;
    private MyFloatingObjectClipboard m_floatingObjectClipboard = new MyFloatingObjectClipboard();
    private MyVoxelClipboard m_voxelClipboard = new MyVoxelClipboard();
    private MyHudNotification m_symmetryNotification;
    private MyHudNotification m_pasteNotification;
    private MyHudNotification m_blueprintNotification;
    private bool m_showAxis;
    private float IntersectionDistance = 20f;
    private float BLOCK_ROTATION_SPEED = 1f / 500f;
    private MyCubeBlockDefinitionWithVariants m_definitionWithVariants;
    protected MyBlockBuilderRotationHints m_rotationHints = new MyBlockBuilderRotationHints();
    private List<Vector3D> m_collisionTestPoints = new List<Vector3D>(12);
    private int m_lastInputHandleTime;
    protected bool m_rotationHintRotating;
    private bool m_activated;
    private MyHudNotification m_stationRotationNotification;
    private MyHudNotification m_stationRotationNotificationOff;
    private static readonly MyStringId ID_GIZMO_DRAW_LINE_WHITE = MyStringId.GetOrCompute("GizmoDrawLineWhite");

    public static MyClipboardDefinition ClipboardDefinition => MyClipboardComponent.m_definition;

    public MyGridClipboard Clipboard => MyClipboardComponent.m_clipboard;

    internal MyFloatingObjectClipboard FloatingObjectClipboard => this.m_floatingObjectClipboard;

    internal MyVoxelClipboard VoxelClipboard => this.m_voxelClipboard;

    public Vector3D FreePlacementTarget => MyBlockBuilderBase.IntersectionStart + MyBlockBuilderBase.IntersectionDirection * (double) this.IntersectionDistance;

    public bool IsActive => this.m_activated;

    private static bool DeveloperSpectatorIsBuilding => MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Spectator && !MySession.Static.SurvivalMode;

    public static bool SpectatorIsBuilding => MyClipboardComponent.DeveloperSpectatorIsBuilding || MyClipboardComponent.AdminSpectatorIsBuilding;

    private static bool AdminSpectatorIsBuilding => MyFakes.ENABLE_ADMIN_SPECTATOR_BUILDING && MySession.Static != null && (MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Spectator && MyMultiplayer.Static != null) && MySession.Static.IsUserAdmin(Sync.MyId);

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent) => base.Init(sessionComponent);

    public override void InitFromDefinition(MySessionComponentDefinition definition)
    {
      base.InitFromDefinition(definition);
      MyClipboardDefinition clipboardDefinition = definition as MyClipboardDefinition;
      if (MyClipboardComponent.m_clipboard != null)
        return;
      MyClipboardComponent.m_definition = clipboardDefinition;
      MyClipboardComponent.m_clipboard = new MyGridClipboard(MyClipboardComponent.m_definition.PastingSettings);
      if (!MyVRage.Platform.System.IsMemoryLimited)
        return;
      MyClipboardComponent.m_clipboard.MaxVisiblePCU = 20000;
    }

    public override void LoadData()
    {
      base.LoadData();
      MyClipboardComponent.Static = this;
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      if (MyClipboardComponent.m_clipboard != null)
        MyClipboardComponent.m_clipboard.Deactivate();
      if (this.m_floatingObjectClipboard != null)
        this.m_floatingObjectClipboard.Deactivate();
      if (this.m_voxelClipboard != null)
        this.m_voxelClipboard.Deactivate();
      MyClipboardComponent.Static = (MyClipboardComponent) null;
    }

    private void RotateAxis(int index, int sign, bool newlyPressed, int frameDt)
    {
      float angleDelta = (float) frameDt * this.BLOCK_ROTATION_SPEED;
      if (MyInput.Static.IsAnyCtrlKeyPressed())
      {
        if (!newlyPressed)
          return;
        angleDelta = 1.570796f;
      }
      if (MyInput.Static.IsAnyAltKeyPressed())
      {
        if (!newlyPressed)
          return;
        angleDelta = MathHelper.ToRadians(1f);
      }
      if (MyClipboardComponent.m_clipboard.IsActive)
        MyClipboardComponent.m_clipboard.RotateAroundAxis(index, sign, newlyPressed, angleDelta);
      if (this.m_floatingObjectClipboard.IsActive)
        this.m_floatingObjectClipboard.RotateAroundAxis(index, sign, newlyPressed, angleDelta);
      if (!this.m_voxelClipboard.IsActive)
        return;
      this.m_voxelClipboard.RotateAroundAxis(index, sign, newlyPressed, angleDelta);
    }

    private bool CheckCopyPasteAllowed()
    {
      if (MySession.Static.ControlledEntity != null && !MySessionComponentSafeZones.IsActionAllowed((MyEntity) MySession.Static.ControlledEntity, MySafeZoneAction.Building))
        return false;
      if (!MySession.Static.IsCopyPastingEnabled && !MySession.Static.CreativeMode)
      {
        int num = MyClipboardComponent.SpectatorIsBuilding ? 1 : 0;
      }
      return !(MySession.Static.ControlledEntity is MyShipController);
    }

    public bool HandleGameInput()
    {
      this.m_rotationHintRotating = false;
      if (MyGuiScreenGamePlay.DisableInput)
        return false;
      MyStringId context = !this.m_activated || !(MySession.Static.ControlledEntity is MyCharacter) ? MyStringId.NullOrEmpty : MySession.Static.ControlledEntity.AuxiliaryContext;
      bool flag = true;
      if (MySession.Static.ControlledEntity != null)
        flag &= MySessionComponentSafeZones.IsActionAllowed((MyEntity) MySession.Static.ControlledEntity, MySafeZoneAction.Building);
      if (!MySession.Static.IsCopyPastingEnabled && !MySession.Static.CreativeMode)
      {
        if (!MyClipboardComponent.SpectatorIsBuilding)
          ;
      }
      else
      {
        if (MySession.Static.IsCopyPastingEnabled && !(MySession.Static.ControlledEntity is MyShipController) && (this.HandleCopyInput() || flag && (this.HandleDeleteInput() || this.HandleCutInput() || (this.HandlePasteInput() || this.HandleMouseScrollInput(context)))) || this.HandleEscape(context))
          return true;
        if (!flag)
          return false;
        if (this.HandleLeftMouseButton(context))
          return true;
      }
      if (!flag)
        return false;
      if (this.HandleBlueprintInput())
        return true;
      if (!MySession.Static.IsCopyPastingEnabled && !(MySession.Static.ControlledEntity is MyShipController) && (MyInput.Static.IsNewKeyPressed(MyKeys.V) && MyInput.Static.IsAnyCtrlKeyPressed()) && !MyInput.Static.IsAnyShiftKeyPressed())
        MyClipboardComponent.ShowCannotPasteWarning();
      if (MyClipboardComponent.m_clipboard != null && MyClipboardComponent.m_clipboard.IsActive && (MyControllerHelper.IsControl(context, MyControlsSpace.FREE_ROTATION) || MyControllerHelper.IsControl(context, MyControlsSpace.SWITCH_BUILDING_MODE)))
        this.ChangeStationRotation();
      return this.HandleRotationInput(context);
    }

    public void ChangeStationRotation()
    {
      MyClipboardComponent.m_clipboard.EnableStationRotation = !MyClipboardComponent.m_clipboard.EnableStationRotation;
      this.m_floatingObjectClipboard.EnableStationRotation = !this.m_floatingObjectClipboard.EnableStationRotation;
    }

    public bool IsStationRotationenabled() => MyClipboardComponent.m_clipboard.EnableStationRotation;

    public static void ShowCannotPasteWarning()
    {
      StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.Blueprints_NoCreativeRightsMessage), messageCaption: messageCaption));
    }

    private bool HandleLeftMouseButton(MyStringId context)
    {
      if (MyInput.Static.IsNewLeftMousePressed() || MyControllerHelper.IsControl(context, MyControlsSpace.COPY_PASTE_ACTION) || MyControllerHelper.IsControl(context, MyControlsSpace.PRIMARY_TOOL_ACTION))
      {
        bool flag = false;
        if (MyClipboardComponent.m_clipboard.IsActive && MyClipboardComponent.m_clipboard.PasteGrid())
        {
          this.UpdatePasteNotification(MyCommonTexts.CubeBuilderPasteNotification);
          flag = true;
        }
        if (this.m_floatingObjectClipboard.IsActive && this.m_floatingObjectClipboard.PasteFloatingObject())
        {
          this.UpdatePasteNotification(MyCommonTexts.CubeBuilderPasteNotification);
          flag = true;
        }
        if (this.m_voxelClipboard.IsActive && this.m_voxelClipboard.PasteVoxelMap())
        {
          this.UpdatePasteNotification(MyCommonTexts.CubeBuilderPasteNotification);
          flag = true;
        }
        if (flag)
        {
          this.Deactivate();
          return true;
        }
      }
      return false;
    }

    public bool HandleEscapeInternal()
    {
      bool flag = false;
      if (MyClipboardComponent.m_clipboard.IsActive)
      {
        MyClipboardComponent.m_clipboard.Deactivate();
        this.UpdatePasteNotification(MyCommonTexts.CubeBuilderPasteNotification);
        flag = true;
      }
      if (this.m_floatingObjectClipboard.IsActive)
      {
        this.m_floatingObjectClipboard.Deactivate();
        this.UpdatePasteNotification(MyCommonTexts.CubeBuilderPasteNotification);
        flag = true;
      }
      if (this.m_voxelClipboard.IsActive)
      {
        this.m_voxelClipboard.Deactivate();
        this.UpdatePasteNotification(MyCommonTexts.CubeBuilderPasteNotification);
        flag = true;
      }
      if (!flag)
        return false;
      this.Deactivate();
      return true;
    }

    private bool HandleEscape(MyStringId context) => (MyInput.Static.IsNewKeyPressed(MyKeys.Escape) || MyControllerHelper.IsControl(context, MyControlsSpace.COPY_PASTE_CANCEL) || MyControllerHelper.IsControl(context, MyControlsSpace.SLOT0)) && this.HandleEscapeInternal();

    public bool HandlePasteInput() => MyInput.Static.IsNewKeyPressed(MyKeys.V) && MyInput.Static.IsAnyCtrlKeyPressed() && !MyInput.Static.IsAnyShiftKeyPressed() && this.Paste();

    public bool Paste()
    {
      if (!this.CheckCopyPasteAllowed())
        return false;
      bool flag = false;
      MySession.Static.GameFocusManager.Clear();
      if (MyClipboardComponent.m_clipboard.PasteGrid(showWarning: (!this.m_floatingObjectClipboard.HasCopiedFloatingObjects())))
      {
        MySessionComponentVoxelHand.Static.Enabled = false;
        this.UpdatePasteNotification(MyCommonTexts.CubeBuilderPasteNotification);
        flag = true;
      }
      else if (this.m_floatingObjectClipboard.PasteFloatingObject())
      {
        MySessionComponentVoxelHand.Static.Enabled = false;
        this.UpdatePasteNotification(MyCommonTexts.CubeBuilderPasteNotification);
        flag = true;
      }
      if (!flag)
        return false;
      if (this.m_activated)
        this.Deactivate();
      else
        this.Activate();
      return true;
    }

    private bool HandleDeleteInput()
    {
      if (MyInput.Static.IsNewKeyPressed(MyKeys.Delete) && MyInput.Static.IsAnyCtrlKeyPressed())
      {
        MyEntity entity = MyCubeGrid.GetTargetEntity();
        if (entity == null || !MySessionComponentSafeZones.IsActionAllowed(entity, MySafeZoneAction.Building, MySession.Static.LocalCharacterEntityId, Sync.MyId))
          return false;
        if (entity is MyCubeGrid myCubeGrid)
        {
          MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
          if (localHumanPlayer == null)
            return false;
          long identityId = localHumanPlayer.Identity.IdentityId;
          bool flag1 = false;
          bool flag2 = false;
          IMyFaction playerFaction1 = MySession.Static.Factions.TryGetPlayerFaction(identityId);
          if (playerFaction1 != null)
            flag2 = playerFaction1.IsLeader(identityId);
          if (MySession.Static.IsUserAdmin(localHumanPlayer.Id.SteamId))
            flag1 = true;
          else if (myCubeGrid.BigOwners.Count != 0)
          {
            foreach (long bigOwner in myCubeGrid.BigOwners)
            {
              if (bigOwner == identityId)
              {
                flag1 = true;
                break;
              }
              if (MySession.Static.Players.TryGetIdentity(bigOwner) != null && flag2)
              {
                IMyFaction playerFaction2 = MySession.Static.Factions.TryGetPlayerFaction(bigOwner);
                if (playerFaction2 != null && playerFaction1.FactionId == playerFaction2.FactionId)
                {
                  flag1 = true;
                  break;
                }
              }
            }
          }
          else
            flag1 = true;
          if (!flag1)
          {
            MyHud.Notifications.Add(MyNotificationSingletons.DeletePermissionFailed);
            return false;
          }
        }
        bool flag = false;
        if (myCubeGrid != null)
        {
          MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
          bool cutGroup = !MyInput.Static.IsAnyShiftKeyPressed();
          bool cutOverLg = MyInput.Static.IsAnyAltKeyPressed();
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextAreYouSureToDeleteGrid), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (v =>
          {
            if (v == MyGuiScreenMessageBox.ResultEnum.YES)
              this.OnDeleteConfirm(entity as MyCubeGrid, cutGroup, cutOverLg);
            Sandbox.Game.Entities.MyEntities.EnableEntityBoundingBoxDraw(entity, false);
          }))));
          flag = true;
        }
        else if (entity is MyVoxelMap)
        {
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextAreYouSureToDeleteGrid), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (v =>
          {
            if (v == MyGuiScreenMessageBox.ResultEnum.YES)
              this.OnDeleteAsteroidConfirm(entity as MyVoxelMap);
            Sandbox.Game.Entities.MyEntities.EnableEntityBoundingBoxDraw(entity, false);
          }))));
          flag = true;
        }
        else if (entity is MyFloatingObject)
        {
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextAreYouSureToDeleteGrid), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (v =>
          {
            if (v == MyGuiScreenMessageBox.ResultEnum.YES)
              this.OnCutFloatingObjectConfirm(entity as MyFloatingObject);
            Sandbox.Game.Entities.MyEntities.EnableEntityBoundingBoxDraw(entity, false);
          }))));
          flag = true;
        }
        if (flag)
          return true;
      }
      return false;
    }

    private bool HandleCutInput()
    {
      if (MyInput.Static.IsNewKeyPressed(MyKeys.X) && MyInput.Static.IsAnyCtrlKeyPressed())
        this.Cut();
      return false;
    }

    public bool Cut()
    {
      if (!this.CheckCopyPasteAllowed())
        return false;
      MyEntity entity = MyCubeGrid.GetTargetEntity();
      if (entity == null || !MySessionComponentSafeZones.IsActionAllowed(entity, MySafeZoneAction.Building, MySession.Static.LocalCharacterEntityId, Sync.MyId))
        return false;
      if (entity is MyCubeGrid myCubeGrid)
      {
        MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
        if (localHumanPlayer == null)
          return false;
        long identityId = localHumanPlayer.Identity.IdentityId;
        bool flag1 = false;
        bool flag2 = false;
        IMyFaction playerFaction1 = MySession.Static.Factions.TryGetPlayerFaction(identityId);
        if (playerFaction1 != null)
          flag2 = playerFaction1.IsLeader(identityId);
        if (MySession.Static.IsUserAdmin(localHumanPlayer.Id.SteamId))
          flag1 = true;
        else if (myCubeGrid.BigOwners.Count != 0)
        {
          foreach (long bigOwner in myCubeGrid.BigOwners)
          {
            if (bigOwner == identityId)
            {
              flag1 = true;
              break;
            }
            if (MySession.Static.Players.TryGetIdentity(bigOwner) != null && flag2)
            {
              IMyFaction playerFaction2 = MySession.Static.Factions.TryGetPlayerFaction(bigOwner);
              if (playerFaction2 != null && playerFaction1.FactionId == playerFaction2.FactionId)
              {
                flag1 = true;
                break;
              }
            }
          }
        }
        else
          flag1 = true;
        if (!flag1)
        {
          MyHud.Notifications.Add(MyNotificationSingletons.CutPermissionFailed);
          return false;
        }
      }
      bool handled = false;
      if (myCubeGrid != null && !MyClipboardComponent.m_clipboard.IsActive)
      {
        MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
        bool cutGroup = !MyInput.Static.IsAnyShiftKeyPressed();
        bool cutOverLg = MyInput.Static.IsAnyAltKeyPressed();
        Sandbox.Game.Entities.MyEntities.EnableEntityBoundingBoxDraw(entity, true);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextAreYouSureToMoveGridToClipboard), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (v =>
        {
          if (v == MyGuiScreenMessageBox.ResultEnum.YES)
            this.OnCutConfirm(entity as MyCubeGrid, cutGroup, cutOverLg);
          Sandbox.Game.Entities.MyEntities.EnableEntityBoundingBoxDraw(entity, false);
        }))));
        handled = true;
      }
      else if (entity is MyVoxelMap && !this.m_voxelClipboard.IsActive && MyPerGameSettings.GUI.VoxelMapEditingScreen == typeof (MyGuiScreenDebugSpawnMenu))
      {
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MySpaceTexts.MessageBoxTextAreYouSureToRemoveAsteroid), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (v =>
        {
          if (v == MyGuiScreenMessageBox.ResultEnum.YES)
            this.OnCutAsteroidConfirm(entity as MyVoxelMap);
          Sandbox.Game.Entities.MyEntities.EnableEntityBoundingBoxDraw(entity, false);
        }))));
        handled = true;
      }
      else if (entity is MyFloatingObject && !this.m_floatingObjectClipboard.IsActive)
      {
        Sandbox.Game.Entities.MyEntities.EnableEntityBoundingBoxDraw(entity, true);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextAreYouSureToMoveGridToClipboard), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (v =>
        {
          if (v == MyGuiScreenMessageBox.ResultEnum.YES)
          {
            this.OnCutFloatingObjectConfirm(entity as MyFloatingObject);
            handled = true;
          }
          Sandbox.Game.Entities.MyEntities.EnableEntityBoundingBoxDraw(entity, false);
        }))));
        handled = true;
      }
      return handled;
    }

    private bool HandleCopyInput()
    {
      if (MyInput.Static.IsNewKeyPressed(MyKeys.C) && MyInput.Static.IsAnyCtrlKeyPressed() && !MyInput.Static.IsAnyMousePressed())
        this.Copy();
      return false;
    }

    public bool Copy()
    {
      if (!this.CheckCopyPasteAllowed())
        return false;
      if (MyClipboardComponent.m_clipboard.IsBeingAdded)
      {
        MyHud.Notifications.Add(MyNotificationSingletons.CopyFailed);
        return false;
      }
      if (MySession.Static.CameraController is MyCharacter || MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Spectator)
      {
        bool flag = false;
        MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
        MyEntity targetEntity = MyCubeGrid.GetTargetEntity();
        if (!MyClipboardComponent.m_clipboard.IsActive && targetEntity is MyCubeGrid)
        {
          MyCubeGrid myCubeGrid = targetEntity as MyCubeGrid;
          MySessionComponentVoxelHand.Static.Enabled = false;
          this.DeactivateCopyPasteFloatingObject(true);
          if (!MyInput.Static.IsAnyShiftKeyPressed())
          {
            MyClipboardComponent.m_clipboard.CopyGroup(myCubeGrid, MyInput.Static.IsAnyAltKeyPressed() ? GridLinkTypeEnum.Physical : GridLinkTypeEnum.Logical);
            MyClipboardComponent.m_clipboard.Activate();
          }
          else
          {
            MyClipboardComponent.m_clipboard.CopyGrid(myCubeGrid);
            MyClipboardComponent.m_clipboard.Activate();
          }
          this.UpdatePasteNotification(MyCommonTexts.CubeBuilderPasteNotification);
          flag = true;
        }
        else if (!this.m_floatingObjectClipboard.IsActive && targetEntity is MyFloatingObject)
        {
          MySessionComponentVoxelHand.Static.Enabled = false;
          this.DeactivateCopyPaste(true);
          this.m_floatingObjectClipboard.CopyfloatingObject(targetEntity as MyFloatingObject);
          this.UpdatePasteNotification(MyCommonTexts.CubeBuilderPasteNotification);
          flag = true;
        }
        if (flag)
        {
          this.Activate();
          MyHud.Notifications.Add(MyNotificationSingletons.CopySucceeded);
          return true;
        }
        MyHud.Notifications.Add(MyNotificationSingletons.CopyFailed);
      }
      return false;
    }

    private bool HandleRotationInput(MyStringId context)
    {
      int frameDt = MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastInputHandleTime;
      this.m_lastInputHandleTime += frameDt;
      if (this.m_activated)
      {
        for (int index1 = 0; index1 < 6; ++index1)
        {
          if (MyControllerHelper.IsControl(context, MyClipboardComponent.m_rotationControls[index1], MyControlStateType.PRESSED))
          {
            bool newlyPressed = MyControllerHelper.IsControl(context, MyClipboardComponent.m_rotationControls[index1]);
            int index2 = -1;
            int rotationDirection = MyClipboardComponent.m_rotationDirections[index1];
            if (MyFakes.ENABLE_STANDARD_AXES_ROTATION)
            {
              if (this.m_rotationHints.RotationUpAxis != new int[6]
              {
                1,
                1,
                0,
                0,
                2,
                2
              }[index1])
                return true;
            }
            if (index1 < 2)
            {
              index2 = this.m_rotationHints.RotationUpAxis;
              rotationDirection *= this.m_rotationHints.RotationUpDirection;
            }
            if (index1 >= 2 && index1 < 4)
            {
              index2 = this.m_rotationHints.RotationRightAxis;
              rotationDirection *= this.m_rotationHints.RotationRightDirection;
            }
            if (index1 >= 4)
            {
              index2 = this.m_rotationHints.RotationForwardAxis;
              rotationDirection *= this.m_rotationHints.RotationForwardDirection;
            }
            if (index2 != -1)
            {
              this.m_rotationHintRotating |= !newlyPressed;
              this.RotateAxis(index2, rotationDirection, newlyPressed, frameDt);
              return true;
            }
          }
        }
        bool flag1 = MyControllerHelper.IsControl(context, MyControlsSpace.ROTATE_AXIS_LEFT, MyControlStateType.PRESSED);
        bool flag2 = MyControllerHelper.IsControl(context, MyControlsSpace.ROTATE_AXIS_RIGHT, MyControlStateType.PRESSED);
        if (flag1 != flag2)
        {
          int index = -1;
          int sign = flag1 ? -1 : 1;
          bool newlyPressed = flag1 ? MyControllerHelper.IsControl(context, MyControlsSpace.ROTATE_AXIS_LEFT) : MyControllerHelper.IsControl(context, MyControlsSpace.ROTATE_AXIS_RIGHT);
          switch (this.m_currentGamepadRotationAxis)
          {
            case 0:
              index = 0;
              break;
            case 1:
              index = 1;
              break;
            case 2:
              index = 2;
              break;
          }
          if (index != -1)
          {
            this.m_rotationHintRotating |= !newlyPressed;
            this.RotateAxis(index, sign, newlyPressed, frameDt);
            return true;
          }
        }
        if (MyControllerHelper.IsControl(context, MyControlsSpace.CHANGE_ROTATION_AXIS))
          this.m_currentGamepadRotationAxis = (this.m_currentGamepadRotationAxis + 1) % 3;
      }
      return false;
    }

    private bool HandleBlueprintInput() => MyInput.Static.IsNewKeyPressed(MyKeys.B) && MyInput.Static.IsAnyCtrlKeyPressed() && !MyInput.Static.IsAnyMousePressed() && this.CreateBlueprint();

    public bool CreateBlueprint()
    {
      if (!MyClipboardComponent.m_clipboard.IsActive)
      {
        MySessionComponentVoxelHand.Static.Enabled = false;
        MyCubeGrid targetGrid = MyCubeGrid.GetTargetGrid();
        if (targetGrid == null)
          return true;
        if (!MySessionComponentSafeZones.IsActionAllowed((MyEntity) targetGrid, MySafeZoneAction.Building, MySession.Static.LocalCharacterEntityId, Sync.MyId))
          return false;
        if (!MySession.Static.CreativeMode && !MySession.Static.CreativeToolsEnabled(Sync.MyId))
        {
          List<MyCubeGrid> result = new List<MyCubeGrid>();
          if (MyInput.Static.IsAnyShiftKeyPressed())
            result.Add(targetGrid);
          else
            MyCubeGridGroups.Static.GetGroups(MyInput.Static.IsAnyAltKeyPressed() ? GridLinkTypeEnum.Physical : GridLinkTypeEnum.Logical).GetGroupNodes(targetGrid, result);
          bool flag1 = true;
          foreach (MyCubeGrid myCubeGrid in result)
          {
            if (myCubeGrid.BigOwners.Count != 0 && !myCubeGrid.BigOwners.Contains(MySession.Static.LocalPlayerId))
            {
              MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(MySession.Static.LocalPlayerId);
              if (playerFaction == null)
              {
                flag1 = false;
                break;
              }
              bool flag2 = false;
              foreach (long bigOwner in myCubeGrid.BigOwners)
              {
                if (MySession.Static.Factions.GetPlayerFaction(bigOwner) == playerFaction)
                {
                  flag2 = true;
                  break;
                }
              }
              if (!flag2)
              {
                flag1 = false;
                break;
              }
            }
          }
          if (!flag1)
          {
            MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
            this.UpdateBlueprintNotification(MyCommonTexts.CubeBuilderNoBlueprintPermission);
            return true;
          }
        }
        MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
        if (MyInput.Static.IsAnyShiftKeyPressed())
          MyClipboardComponent.m_clipboard.CopyGrid(targetGrid);
        else
          MyClipboardComponent.m_clipboard.CopyGroup(targetGrid, MyInput.Static.IsAnyAltKeyPressed() ? GridLinkTypeEnum.Physical : GridLinkTypeEnum.Logical);
        this.UpdatePasteNotification(MyCommonTexts.CubeBuilderPasteNotification);
        MyBlueprintUtils.OpenBlueprintScreen(MyClipboardComponent.m_clipboard, MySession.Static.CreativeMode || MySession.Static.CreativeToolsEnabled(Sync.MyId), MyBlueprintAccessType.NORMAL, (Action<MyGuiBlueprintScreen_Reworked>) (bp =>
        {
          if (bp == null)
            return;
          bp.CreateBlueprintFromClipboard(true);
          MyClipboardComponent.m_clipboard.Deactivate();
        }));
      }
      return true;
    }

    private void OnCutConfirm(MyCubeGrid targetGrid, bool cutGroup, bool cutOverLgs)
    {
      if (!Sandbox.Game.Entities.MyEntities.EntityExists(targetGrid.EntityId))
        return;
      this.DeactivateCopyPasteVoxel(true);
      this.DeactivateCopyPasteFloatingObject(true);
      if (cutGroup)
        MyClipboardComponent.m_clipboard.CutGroup(targetGrid, cutOverLgs ? GridLinkTypeEnum.Physical : GridLinkTypeEnum.Logical);
      else
        MyClipboardComponent.m_clipboard.CutGrid(targetGrid);
    }

    private void OnDeleteConfirm(MyCubeGrid targetGrid, bool cutGroup, bool cutOverLgs)
    {
      if (!Sandbox.Game.Entities.MyEntities.EntityExists(targetGrid.EntityId))
        return;
      this.DeactivateCopyPasteVoxel(true);
      this.DeactivateCopyPasteFloatingObject(true);
      if (cutGroup)
        MyClipboardComponent.m_clipboard.DeleteGroup(targetGrid, cutOverLgs ? GridLinkTypeEnum.Physical : GridLinkTypeEnum.Logical);
      else
        MyClipboardComponent.m_clipboard.DeleteGrid(targetGrid);
    }

    private void OnDeleteAsteroidConfirm(MyVoxelMap targetVoxelMap)
    {
      if (!Sandbox.Game.Entities.MyEntities.EntityExists(targetVoxelMap.EntityId))
        return;
      this.DeactivateCopyPaste(true);
      this.DeactivateCopyPasteFloatingObject(true);
      Sandbox.Game.Entities.MyEntities.SendCloseRequest((IMyEntity) targetVoxelMap);
    }

    private void OnCutAsteroidConfirm(MyVoxelMap targetVoxelMap)
    {
      if (!Sandbox.Game.Entities.MyEntities.EntityExists(targetVoxelMap.EntityId))
        return;
      this.DeactivateCopyPaste(true);
      this.DeactivateCopyPasteFloatingObject(true);
      Sandbox.Game.Entities.MyEntities.SendCloseRequest((IMyEntity) targetVoxelMap);
    }

    private void OnDeleteFloatingObjectConfirm(MyFloatingObject floatingObj)
    {
      if (!Sandbox.Game.Entities.MyEntities.Exist((MyEntity) floatingObj))
        return;
      this.DeactivateCopyPasteVoxel(true);
      this.DeactivateCopyPaste(true);
      this.m_floatingObjectClipboard.DeleteFloatingObject(floatingObj);
    }

    private void OnCutFloatingObjectConfirm(MyFloatingObject floatingObj)
    {
      if (!Sandbox.Game.Entities.MyEntities.Exist((MyEntity) floatingObj))
        return;
      this.DeactivateCopyPasteVoxel(true);
      this.DeactivateCopyPaste(true);
      this.m_floatingObjectClipboard.CutFloatingObject(floatingObj);
    }

    public void OnLostFocus() => this.Deactivate();

    public void DeactivateCopyPasteVoxel(bool clear = false)
    {
      if (this.m_voxelClipboard.IsActive)
        this.m_voxelClipboard.Deactivate();
      this.RemovePasteNotification();
      if (!clear)
        return;
      this.m_voxelClipboard.ClearClipboard();
    }

    public void DeactivateCopyPasteFloatingObject(bool clear = false)
    {
      if (this.m_floatingObjectClipboard.IsActive)
        this.m_floatingObjectClipboard.Deactivate();
      this.RemovePasteNotification();
      if (!clear)
        return;
      this.m_floatingObjectClipboard.ClearClipboard();
    }

    public void DeactivateCopyPaste(bool clear = false)
    {
      if (MyClipboardComponent.m_clipboard.IsActive)
        MyClipboardComponent.m_clipboard.Deactivate();
      this.RemovePasteNotification();
      if (!clear)
        return;
      MyClipboardComponent.m_clipboard.ClearClipboard();
    }

    private void UpdatePasteNotification(MyStringId myTextsWrapperEnum)
    {
      this.RemovePasteNotification();
      if (!MyClipboardComponent.m_clipboard.IsActive)
        return;
      this.m_pasteNotification = new MyHudNotification(myTextsWrapperEnum, 0, level: MyNotificationLevel.Control);
      MyHud.Notifications.Add((MyHudNotificationBase) this.m_pasteNotification);
    }

    private void RemovePasteNotification()
    {
      if (this.m_pasteNotification == null)
        return;
      MyHud.Notifications.Remove((MyHudNotificationBase) this.m_pasteNotification);
      this.m_pasteNotification = (MyHudNotification) null;
    }

    private void UpdateBlueprintNotification(MyStringId text)
    {
      if (this.m_blueprintNotification != null)
        MyHud.Notifications.Remove((MyHudNotificationBase) this.m_blueprintNotification);
      this.m_blueprintNotification = new MyHudNotification(text, font: "Red");
      MyHud.Notifications.Add((MyHudNotificationBase) this.m_blueprintNotification);
    }

    private bool HandleMouseScrollInput(MyStringId context)
    {
      bool flag1 = MyInput.Static.IsAnyCtrlKeyPressed();
      if (flag1 && MyInput.Static.PreviousMouseScrollWheelValue() < MyInput.Static.MouseScrollWheelValue() || MyControllerHelper.IsControl(context, MyControlsSpace.MOVE_FURTHER, MyControlStateType.PRESSED))
      {
        bool flag2 = false;
        if (MyClipboardComponent.m_clipboard.IsActive)
        {
          MyClipboardComponent.m_clipboard.MoveEntityFurther();
          flag2 = true;
        }
        if (this.m_floatingObjectClipboard.IsActive)
        {
          this.m_floatingObjectClipboard.MoveEntityFurther();
          flag2 = true;
        }
        if (this.m_voxelClipboard.IsActive)
        {
          this.m_voxelClipboard.MoveEntityFurther();
          flag2 = true;
        }
        return flag2;
      }
      if ((!flag1 || MyInput.Static.PreviousMouseScrollWheelValue() <= MyInput.Static.MouseScrollWheelValue()) && !MyControllerHelper.IsControl(context, MyControlsSpace.MOVE_CLOSER, MyControlStateType.PRESSED))
        return false;
      bool flag3 = false;
      if (MyClipboardComponent.m_clipboard.IsActive)
      {
        MyClipboardComponent.m_clipboard.MoveEntityCloser();
        flag3 = true;
      }
      if (this.m_floatingObjectClipboard.IsActive)
      {
        this.m_floatingObjectClipboard.MoveEntityCloser();
        flag3 = true;
      }
      if (this.m_voxelClipboard.IsActive)
      {
        this.m_voxelClipboard.MoveEntityCloser();
        flag3 = true;
      }
      return flag3;
    }

    public static void PrepareCharacterCollisionPoints(List<Vector3D> outList)
    {
      if (!(MySession.Static.ControlledEntity is MyCharacter controlledEntity))
        return;
      float num1 = controlledEntity.Definition.CharacterCollisionHeight * 0.7f;
      float num2 = controlledEntity.Definition.CharacterCollisionWidth * 0.2f;
      if (controlledEntity == null)
        return;
      if (controlledEntity.IsCrouching)
        num1 = controlledEntity.Definition.CharacterCollisionCrouchHeight;
      Matrix matrix = controlledEntity.PositionComp.LocalMatrixRef;
      Vector3 vector3_1 = matrix.Up * num1;
      matrix = controlledEntity.PositionComp.LocalMatrixRef;
      Vector3 vector3_2 = matrix.Forward * num2;
      matrix = controlledEntity.PositionComp.LocalMatrixRef;
      Vector3 vector3_3 = matrix.Right * num2;
      Vector3D position = controlledEntity.Entity.PositionComp.GetPosition();
      matrix = controlledEntity.PositionComp.LocalMatrixRef;
      Vector3 vector3_4 = matrix.Up * 0.2f;
      Vector3D vector3D1 = position + vector3_4;
      float num3 = 0.0f;
      for (int index = 0; index < 6; ++index)
      {
        float num4 = (float) Math.Sin((double) num3);
        float num5 = (float) Math.Cos((double) num3);
        Vector3D vector3D2 = vector3D1 + num4 * vector3_3 + num5 * vector3_2;
        outList.Add(vector3D2);
        outList.Add(vector3D2 + vector3_1);
        num3 += 1.047198f;
      }
    }

    private void Activate()
    {
      MySession.Static.GameFocusManager.Register((IMyFocusHolder) this);
      this.m_activated = true;
    }

    private void Deactivate()
    {
      MySession.Static.GameFocusManager.Unregister((IMyFocusHolder) this);
      this.m_activated = false;
      this.m_rotationHints.ReleaseRenderData();
      this.DeactivateCopyPasteVoxel();
      this.DeactivateCopyPasteFloatingObject();
      this.DeactivateCopyPaste();
    }

    public void ActivateVoxelClipboard(
      MyObjectBuilder_EntityBase voxelMap,
      VRage.Game.Voxels.IMyStorage storage,
      Vector3 centerDeltaDirection,
      float dragVectorLength)
    {
      MySessionComponentVoxelHand.Static.Enabled = false;
      this.m_voxelClipboard.SetVoxelMapFromBuilder(voxelMap, storage, centerDeltaDirection, dragVectorLength);
      this.Activate();
    }

    public void ActivateFloatingObjectClipboard(
      MyObjectBuilder_FloatingObject floatingObject,
      Vector3D centerDeltaDirection,
      float dragVectorLength)
    {
      MySessionComponentVoxelHand.Static.Enabled = false;
      this.m_floatingObjectClipboard.SetFloatingObjectFromBuilder(floatingObject, centerDeltaDirection, dragVectorLength);
      this.Activate();
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (!this.m_activated)
        return;
      MyClipboardComponent.m_clipboard.Update();
      this.m_floatingObjectClipboard.Update();
      this.m_voxelClipboard.Update();
      if (MyClipboardComponent.m_clipboard.IsActive || this.m_floatingObjectClipboard.IsActive || this.m_voxelClipboard.IsActive)
      {
        this.m_collisionTestPoints.Clear();
        MyClipboardComponent.PrepareCharacterCollisionPoints(this.m_collisionTestPoints);
        if (MyClipboardComponent.m_clipboard.IsActive)
          MyClipboardComponent.m_clipboard.Show();
        else
          MyClipboardComponent.m_clipboard.Hide();
        if (this.m_floatingObjectClipboard.IsActive)
        {
          this.m_floatingObjectClipboard.Show();
          this.m_floatingObjectClipboard.HideWhenColliding(this.m_collisionTestPoints);
        }
        else
          this.m_floatingObjectClipboard.Hide();
        if (this.m_voxelClipboard.IsActive)
          this.m_voxelClipboard.Show();
        else
          this.m_voxelClipboard.Hide();
      }
      this.UpdateClipboards();
    }

    private void UpdateClipboards()
    {
      if (MyClipboardComponent.m_clipboard.IsActive)
        MyClipboardComponent.m_clipboard.CalculateRotationHints(this.m_rotationHints, this.m_rotationHintRotating);
      else if (this.m_floatingObjectClipboard.IsActive)
      {
        this.m_floatingObjectClipboard.CalculateRotationHints(this.m_rotationHints, this.m_rotationHintRotating);
      }
      else
      {
        if (!this.m_voxelClipboard.IsActive)
          return;
        this.m_voxelClipboard.CalculateRotationHints(this.m_rotationHints, this.m_rotationHintRotating);
      }
    }

    public override void Draw()
    {
      base.Draw();
      if (!this.IsActive || !MyInput.Static.IsJoystickLastUsed)
        return;
      this.DrawRotationAxis(this.m_currentGamepadRotationAxis);
    }

    private void DrawRotationAxis(int axis)
    {
      Matrix orientationMatrix = MyClipboardComponent.m_clipboard.GetFirstGridOrientationMatrix();
      Vector3D pastePosition = MyClipboardComponent.m_clipboard.PastePosition;
      Vector3D vector3D1 = Vector3D.Zero;
      Color color = Color.White;
      switch (axis)
      {
        case 0:
          vector3D1 = (Vector3D) orientationMatrix.Left;
          color = Color.Red;
          break;
        case 1:
          vector3D1 = (Vector3D) orientationMatrix.Up;
          color = Color.Green;
          break;
        case 2:
          vector3D1 = (Vector3D) orientationMatrix.Forward;
          color = Color.Blue;
          break;
      }
      Vector3D vector3D2 = vector3D1 * ((double) MyClipboardComponent.m_clipboard.GetGridHalfExtent(axis) + 1.0);
      Vector4 vector4 = color.ToVector4();
      MySimpleObjectDraw.DrawLine(pastePosition + vector3D2, pastePosition - vector3D2, new MyStringId?(MyClipboardComponent.ID_GIZMO_DRAW_LINE_WHITE), ref vector4, 0.15f, MyBillboard.BlendTypeEnum.LDR);
    }
  }
}
