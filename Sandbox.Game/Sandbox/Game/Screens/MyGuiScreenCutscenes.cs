// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenCutscenes
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenCutscenes : MyGuiScreenDebugBase
  {
    private static readonly Vector2 SCREEN_SIZE = new Vector2(0.4f, 1.2f);
    private static readonly float HIDDEN_PART_RIGHT = 0.04f;
    private static readonly float ITEM_HORIZONTAL_PADDING = 0.01f;
    private static readonly float ITEM_VERTICAL_PADDING = 0.005f;
    private static readonly Vector2 BUTTON_SIZE = new Vector2(0.06f, 0.03f);
    private static readonly Vector2 ITEM_SIZE = new Vector2(0.06f, 0.02f);
    public static MyGuiScreenCutscenes Static;
    private Dictionary<string, Cutscene> m_cutscenes;
    private Cutscene m_cutsceneCurrent;
    private int m_selectedCutsceneNodeIndex = -1;
    private bool m_cutscenePlaying;
    private MyGuiControlCombobox m_cutsceneSelection;
    private MyGuiControlButton m_cutsceneDeleteButton;
    private MyGuiControlButton m_cutscenePlayButton;
    private MyGuiControlButton m_cutsceneRevertButton;
    private MyGuiControlButton m_cutsceneSaveButton;
    private MyGuiControlTextbox m_cutscenePropertyStartEntity;
    private MyGuiControlTextbox m_cutscenePropertyStartLookAt;
    private MyGuiControlCombobox m_cutscenePropertyNextCutscene;
    private MyGuiControlTextbox m_cutscenePropertyStartingFOV;
    private MyGuiControlCheckbox m_cutscenePropertyCanBeSkipped;
    private MyGuiControlCheckbox m_cutscenePropertyFireEventsDuringSkip;
    private MyGuiControlListbox m_cutsceneNodes;
    private MyGuiControlButton m_cutsceneNodeButtonAdd;
    private MyGuiControlButton m_cutsceneNodeButtonMoveUp;
    private MyGuiControlButton m_cutsceneNodeButtonMoveDown;
    private MyGuiControlButton m_cutsceneNodeButtonDelete;
    private MyGuiControlButton m_cutsceneNodeButtonDeleteAll;
    private MyGuiControlTextbox m_cutsceneNodePropertyTime;
    private MyGuiControlTextbox m_cutsceneNodePropertyMoveTo;
    private MyGuiControlTextbox m_cutsceneNodePropertyMoveToInstant;
    private MyGuiControlTextbox m_cutsceneNodePropertyRotateLike;
    private MyGuiControlTextbox m_cutsceneNodePropertyRotateLikeInstant;
    private MyGuiControlTextbox m_cutsceneNodePropertyRotateTowards;
    private MyGuiControlTextbox m_cutsceneNodePropertyRotateTowardsInstant;
    private MyGuiControlTextbox m_cutsceneNodePropertyRotateTowardsLock;
    private MyGuiControlTextbox m_cutsceneNodePropertyAttachAll;
    private MyGuiControlTextbox m_cutsceneNodePropertyAttachPosition;
    private MyGuiControlTextbox m_cutsceneNodePropertyAttachRotation;
    private MyGuiControlTextbox m_cutsceneNodePropertyEvent;
    private MyGuiControlTextbox m_cutsceneNodePropertyEventDelay;
    private MyGuiControlTextbox m_cutsceneNodePropertyFOVChange;
    private MyGuiControlTextbox m_cutsceneNodePropertyWaypoints;

    public MyGuiScreenCutscenes()
      : base(new Vector2(MyGuiManager.GetMaxMouseCoord().X - MyGuiScreenCutscenes.SCREEN_SIZE.X * 0.5f + MyGuiScreenCutscenes.HIDDEN_PART_RIGHT, 0.5f), new Vector2?(MyGuiScreenCutscenes.SCREEN_SIZE), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), false)
    {
      this.CanBeHidden = true;
      this.CanHideOthers = false;
      this.m_canCloseInCloseAllScreenCalls = true;
      this.m_canShareInput = true;
      this.m_isTopScreen = false;
      this.m_isTopMostScreen = false;
      MyGuiScreenCutscenes.Static = this;
      MySession.Static.SetCameraController(MyCameraControllerEnum.SpectatorFreeMouse, (IMyEntity) null, new Vector3D?());
      MyDebugDrawSettings.ENABLE_DEBUG_DRAW = true;
      MyDebugDrawSettings.DEBUG_DRAW_CUTSCENES = true;
      MyDebugDrawSettings.DEBUG_DRAW_WAYPOINTS = true;
      this.RecreateControls(true);
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      if (!MyToolbarComponent.IsToolbarControlShown)
        MyToolbarComponent.IsToolbarControlShown = true;
      if (MyInput.Static.IsNewKeyPressed(MyKeys.Escape) || MyInput.Static.IsNewKeyPressed(MyKeys.F11))
      {
        this.CloseScreenWithSave();
      }
      else
      {
        base.HandleInput(receivedFocusInThisUpdate);
        if (MySpectatorCameraController.Static.SpectatorCameraMovement != MySpectatorCameraMovementEnum.FreeMouse)
          MySpectatorCameraController.Static.SpectatorCameraMovement = MySpectatorCameraMovementEnum.FreeMouse;
        foreach (MyGuiScreenBase screen in MyScreenManager.Screens)
        {
          switch (screen)
          {
            case MyGuiScreenScriptingTools _:
            case MyGuiScreenCutscenes _:
              continue;
            default:
              screen.HandleInput(receivedFocusInThisUpdate);
              continue;
          }
        }
      }
    }

    public override bool CloseScreen(bool isUnloading = false)
    {
      MySpectatorCameraController.Static.SpectatorCameraMovement = MySpectatorCameraMovementEnum.UserControlled;
      if (MySession.Static.ControlledEntity != null)
        MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, (IMyEntity) MySession.Static.ControlledEntity.Entity, new Vector3D?());
      MyDebugDrawSettings.ENABLE_DEBUG_DRAW = false;
      MyDebugDrawSettings.DEBUG_DRAW_CUTSCENES = false;
      MyDebugDrawSettings.DEBUG_DRAW_WAYPOINTS = false;
      MyGuiScreenGamePlay.DisableInput = MySession.Static.GetComponent<MySessionComponentCutscenes>().IsCutsceneRunning;
      MyGuiScreenCutscenes.Static = (MyGuiScreenCutscenes) null;
      return base.CloseScreen(isUnloading);
    }

    public override bool Update(bool hasFocus)
    {
      this.UpdateCutscenes();
      return base.Update(hasFocus);
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      float y = (float) (((double) MyGuiScreenCutscenes.SCREEN_SIZE.Y - 1.0) / 2.0);
      Vector2 vector2 = new Vector2(0.02f, 0.0f);
      MyGuiControlLabel myGuiControlLabel = this.AddCaption(MyTexts.Get(MySpaceTexts.ScriptingToolsCutscenes).ToString(), new Vector4?(Color.White.ToVector4()), new Vector2?(vector2 + new Vector2(-MyGuiScreenCutscenes.HIDDEN_PART_RIGHT, y)));
      this.m_currentPosition.Y = myGuiControlLabel.PositionY + myGuiControlLabel.Size.Y + MyGuiScreenCutscenes.ITEM_VERTICAL_PADDING;
      this.RecreateControlsCutscenes();
    }

    private void RecreateControlsCutscenes()
    {
      this.m_cutscenes = MySession.Static.GetComponent<MySessionComponentCutscenes>().GetCutscenes();
      this.m_currentPosition.Y += MyGuiScreenCutscenes.ITEM_SIZE.Y;
      this.PositionControls(new MyGuiControlBase[2]
      {
        (MyGuiControlBase) this.CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_New).ToString(), new Action<MyGuiControlButton>(this.CreateNewCutsceneClicked)),
        (MyGuiControlBase) this.CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_ClearAllCutscenes).ToString(), new Action<MyGuiControlButton>(this.ClearAllCutscenesClicked))
      });
      this.m_cutsceneSelection = this.CreateComboBox();
      foreach (Cutscene cutscene in this.m_cutscenes.Values)
        this.m_cutsceneSelection.AddItem(cutscene.Name.GetHashCode64(), cutscene.Name);
      this.m_cutsceneSelection.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.m_cutsceneSelection_ItemSelected);
      this.PositionControls(new MyGuiControlBase[2]
      {
        (MyGuiControlBase) this.CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Selected).ToString()),
        (MyGuiControlBase) this.m_cutsceneSelection
      });
      this.m_cutsceneDeleteButton = this.CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Delete).ToString(), new Action<MyGuiControlButton>(this.DeleteCurrentCutsceneClicked));
      this.m_cutscenePlayButton = this.CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Play).ToString(), new Action<MyGuiControlButton>(this.WatchCutsceneClicked));
      this.m_cutscenePlayButton.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Play_Extended).ToString());
      this.m_cutsceneSaveButton = this.CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Save).ToString(), new Action<MyGuiControlButton>(this.SaveCutsceneClicked));
      this.m_cutsceneRevertButton = this.CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Revert).ToString(), new Action<MyGuiControlButton>(this.RevertCutsceneClicked));
      this.PositionControls(new MyGuiControlBase[4]
      {
        (MyGuiControlBase) this.m_cutscenePlayButton,
        (MyGuiControlBase) this.m_cutsceneSaveButton,
        (MyGuiControlBase) this.m_cutsceneRevertButton,
        (MyGuiControlBase) this.m_cutsceneDeleteButton
      });
      this.m_currentPosition.Y += MyGuiScreenCutscenes.ITEM_SIZE.Y / 2f;
      this.m_cutscenePropertyNextCutscene = this.CreateComboBox();
      this.m_cutscenePropertyNextCutscene.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.CutscenePropertyNextCutscene_ItemSelected);
      this.PositionControls(new MyGuiControlBase[2]
      {
        (MyGuiControlBase) this.CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_New).ToString()),
        (MyGuiControlBase) this.m_cutscenePropertyNextCutscene
      });
      this.PositionControls(new MyGuiControlBase[3]
      {
        (MyGuiControlBase) this.CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_PosRot).ToString()),
        (MyGuiControlBase) this.CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_LookRot).ToString()),
        (MyGuiControlBase) this.CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_FOV).ToString())
      });
      this.m_cutscenePropertyStartEntity = this.CreateTextbox("", new Action<MyGuiControlTextbox>(this.CutscenePropertyStartEntity_TextChanged));
      this.m_cutscenePropertyStartEntity.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_PosRot_Extended).ToString());
      this.m_cutscenePropertyStartLookAt = this.CreateTextbox("", new Action<MyGuiControlTextbox>(this.CutscenePropertyStartLookAt_TextChanged));
      this.m_cutscenePropertyStartLookAt.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_LookRot_Extended).ToString());
      this.m_cutscenePropertyStartingFOV = this.CreateTextbox("", new Action<MyGuiControlTextbox>(this.CutscenePropertyStartingFOV_TextChanged));
      this.m_cutscenePropertyStartingFOV.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_FOV_Extended).ToString());
      this.PositionControls(new MyGuiControlBase[3]
      {
        (MyGuiControlBase) this.m_cutscenePropertyStartEntity,
        (MyGuiControlBase) this.m_cutscenePropertyStartLookAt,
        (MyGuiControlBase) this.m_cutscenePropertyStartingFOV
      });
      this.m_cutscenePropertyCanBeSkipped = this.CreateCheckbox(new Action<MyGuiControlCheckbox>(this.CutscenePropertyCanBeSkippedChanged), true);
      this.m_cutscenePropertyCanBeSkipped.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Skippable).ToString());
      this.m_cutscenePropertyFireEventsDuringSkip = this.CreateCheckbox(new Action<MyGuiControlCheckbox>(this.CutscenePropertyFireEventsDuringSkipChanged), true);
      this.m_cutscenePropertyFireEventsDuringSkip.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_SkipWarning).ToString());
      this.PositionControls(new MyGuiControlBase[4]
      {
        (MyGuiControlBase) this.CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_CanSkip).ToString()),
        (MyGuiControlBase) this.m_cutscenePropertyCanBeSkipped,
        (MyGuiControlBase) this.CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Events).ToString()),
        (MyGuiControlBase) this.m_cutscenePropertyFireEventsDuringSkip
      });
      this.m_currentPosition.Y += MyGuiScreenCutscenes.ITEM_SIZE.Y;
      this.m_cutsceneNodeButtonAdd = this.CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_AddNode).ToString(), new Action<MyGuiControlButton>(this.CutsceneNodeButtonAddClicked));
      this.m_cutsceneNodeButtonDelete = this.CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Delete).ToString(), new Action<MyGuiControlButton>(this.CutsceneNodeButtonDeleteClicked));
      this.m_cutsceneNodeButtonDeleteAll = this.CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_ClearAll).ToString(), new Action<MyGuiControlButton>(this.CutsceneNodeButtonDeleteAllClicked));
      this.m_cutsceneNodeButtonMoveUp = this.CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_MoveUp).ToString(), new Action<MyGuiControlButton>(this.CutsceneNodeButtonMoveUpClicked));
      this.m_cutsceneNodeButtonMoveDown = this.CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_MoveDown).ToString(), new Action<MyGuiControlButton>(this.CutsceneNodeButtonMoveDownClicked));
      this.PositionControls(new MyGuiControlBase[3]
      {
        (MyGuiControlBase) this.CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Nodes).ToString()),
        (MyGuiControlBase) this.m_cutsceneNodeButtonAdd,
        (MyGuiControlBase) this.m_cutsceneNodeButtonDeleteAll
      });
      this.PositionControls(new MyGuiControlBase[4]
      {
        (MyGuiControlBase) this.CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_CurrentNode).ToString()),
        (MyGuiControlBase) this.m_cutsceneNodeButtonMoveUp,
        (MyGuiControlBase) this.m_cutsceneNodeButtonMoveDown,
        (MyGuiControlBase) this.m_cutsceneNodeButtonDelete
      });
      this.m_cutsceneNodes = this.CreateListBox();
      this.m_cutsceneNodes.VisibleRowsCount = 5;
      this.m_cutsceneNodes.Size = new Vector2(0.0f, 0.12f);
      this.m_cutsceneNodes.ItemsSelected += new Action<MyGuiControlListbox>(this.m_cutsceneNodes_ItemsSelected);
      this.PositionControl((MyGuiControlBase) this.m_cutsceneNodes);
      this.m_cutsceneNodePropertyTime = this.CreateTextbox("", new Action<MyGuiControlTextbox>(this.CutsceneNodePropertyTime_TextChanged));
      this.m_cutsceneNodePropertyTime.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Time_Extended).ToString());
      this.m_cutsceneNodePropertyEvent = this.CreateTextbox("", new Action<MyGuiControlTextbox>(this.CutsceneNodePropertyEvent_TextChanged));
      this.m_cutsceneNodePropertyEvent.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Event_Extended).ToString());
      this.m_cutsceneNodePropertyEventDelay = this.CreateTextbox("", new Action<MyGuiControlTextbox>(this.CutsceneNodePropertyEventDelay_TextChanged));
      this.m_cutsceneNodePropertyEventDelay.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_EventDelay_Extended).ToString());
      this.m_cutsceneNodePropertyFOVChange = this.CreateTextbox("", new Action<MyGuiControlTextbox>(this.CutsceneNodePropertyFOV_TextChanged));
      this.m_cutsceneNodePropertyFOVChange.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_FOVChange_Extended).ToString());
      this.PositionControls(new MyGuiControlBase[4]
      {
        (MyGuiControlBase) this.CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Time).ToString()),
        (MyGuiControlBase) this.CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Event).ToString()),
        (MyGuiControlBase) this.CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_EventDelay).ToString()),
        (MyGuiControlBase) this.CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_FOVChange).ToString())
      });
      this.PositionControls(new MyGuiControlBase[4]
      {
        (MyGuiControlBase) this.m_cutsceneNodePropertyTime,
        (MyGuiControlBase) this.m_cutsceneNodePropertyEvent,
        (MyGuiControlBase) this.m_cutsceneNodePropertyEventDelay,
        (MyGuiControlBase) this.m_cutsceneNodePropertyFOVChange
      });
      this.m_currentPosition.Y += MyGuiScreenCutscenes.ITEM_SIZE.Y / 2f;
      this.PositionControls(new MyGuiControlBase[3]
      {
        (MyGuiControlBase) this.CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Action).ToString()),
        (MyGuiControlBase) this.CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_OverTime).ToString()),
        (MyGuiControlBase) this.CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Instant).ToString())
      });
      this.m_cutsceneNodePropertyMoveTo = this.CreateTextbox("", new Action<MyGuiControlTextbox>(this.CutsceneNodePropertyMoveTo_TextChanged));
      this.m_cutsceneNodePropertyMoveTo.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_MoveTo_Extended1).ToString());
      this.m_cutsceneNodePropertyMoveToInstant = this.CreateTextbox("", new Action<MyGuiControlTextbox>(this.CutsceneNodePropertyMoveToInstant_TextChanged));
      this.m_cutsceneNodePropertyMoveToInstant.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_MoveTo_Extended2).ToString());
      this.PositionControls(new MyGuiControlBase[3]
      {
        (MyGuiControlBase) this.CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_MoveTo).ToString()),
        (MyGuiControlBase) this.m_cutsceneNodePropertyMoveTo,
        (MyGuiControlBase) this.m_cutsceneNodePropertyMoveToInstant
      });
      this.m_cutsceneNodePropertyRotateLike = this.CreateTextbox("", new Action<MyGuiControlTextbox>(this.CutsceneNodePropertyRotateLike_TextChanged));
      this.m_cutsceneNodePropertyRotateLike.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_RotateLike_Extended1).ToString());
      this.m_cutsceneNodePropertyRotateLikeInstant = this.CreateTextbox("", new Action<MyGuiControlTextbox>(this.CutsceneNodePropertyRotateLikeInstant_TextChanged));
      this.m_cutsceneNodePropertyRotateLikeInstant.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_RotateLike_Extended2).ToString());
      this.PositionControls(new MyGuiControlBase[3]
      {
        (MyGuiControlBase) this.CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_RotateLike).ToString()),
        (MyGuiControlBase) this.m_cutsceneNodePropertyRotateLike,
        (MyGuiControlBase) this.m_cutsceneNodePropertyRotateLikeInstant
      });
      this.m_cutsceneNodePropertyRotateTowards = this.CreateTextbox("", new Action<MyGuiControlTextbox>(this.CutsceneNodePropertyLookAt_TextChanged));
      this.m_cutsceneNodePropertyRotateTowards.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_LookAt_Extended1).ToString());
      this.m_cutsceneNodePropertyRotateTowardsInstant = this.CreateTextbox("", new Action<MyGuiControlTextbox>(this.CutsceneNodePropertyLookAtInstant_TextChanged));
      this.m_cutsceneNodePropertyRotateTowardsInstant.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_LookAt_Extended2).ToString());
      this.PositionControls(new MyGuiControlBase[3]
      {
        (MyGuiControlBase) this.CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_LookAt).ToString()),
        (MyGuiControlBase) this.m_cutsceneNodePropertyRotateTowards,
        (MyGuiControlBase) this.m_cutsceneNodePropertyRotateTowardsInstant
      });
      this.m_currentPosition.Y += MyGuiScreenCutscenes.ITEM_SIZE.Y;
      this.m_cutsceneNodePropertyRotateTowardsLock = this.CreateTextbox("", new Action<MyGuiControlTextbox>(this.CutsceneNodePropertyLockRotationTo_TextChanged));
      this.m_cutsceneNodePropertyRotateTowardsLock.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Track_Extended1).ToString());
      this.m_cutsceneNodePropertyAttachAll = this.CreateTextbox("", new Action<MyGuiControlTextbox>(this.CutsceneNodePropertyAttachTo_TextChanged));
      this.m_cutsceneNodePropertyAttachAll.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Track_Extended2).ToString());
      this.m_cutsceneNodePropertyAttachPosition = this.CreateTextbox("", new Action<MyGuiControlTextbox>(this.CutsceneNodePropertyAttachPositionTo_TextChanged));
      this.m_cutsceneNodePropertyAttachPosition.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Track_Extended3).ToString());
      this.m_cutsceneNodePropertyAttachRotation = this.CreateTextbox("", new Action<MyGuiControlTextbox>(this.CutsceneNodePropertyAttachRotationTo_TextChanged));
      this.m_cutsceneNodePropertyAttachRotation.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Track_Extended4).ToString());
      this.PositionControls(new MyGuiControlBase[4]
      {
        (MyGuiControlBase) this.CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_TrackLook).ToString()),
        (MyGuiControlBase) this.CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_TrackPosRot).ToString()),
        (MyGuiControlBase) this.CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_TrackPos).ToString()),
        (MyGuiControlBase) this.CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_TrackRot).ToString())
      });
      this.PositionControls(new MyGuiControlBase[4]
      {
        (MyGuiControlBase) this.m_cutsceneNodePropertyRotateTowardsLock,
        (MyGuiControlBase) this.m_cutsceneNodePropertyAttachAll,
        (MyGuiControlBase) this.m_cutsceneNodePropertyAttachPosition,
        (MyGuiControlBase) this.m_cutsceneNodePropertyAttachRotation
      });
      this.m_currentPosition.Y += MyGuiScreenCutscenes.ITEM_SIZE.Y / 2f;
      this.m_cutsceneNodePropertyWaypoints = this.CreateTextbox("", new Action<MyGuiControlTextbox>(this.CutsceneNodePropertyWaypoints_TextChanged));
      this.m_cutsceneNodePropertyWaypoints.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Waypoints_Extended).ToString());
      this.PositionControl((MyGuiControlBase) this.CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Waypoints).ToString()));
      this.PositionControl((MyGuiControlBase) this.m_cutsceneNodePropertyWaypoints);
      this.m_cutsceneCurrent = (Cutscene) null;
      this.m_selectedCutsceneNodeIndex = -1;
      this.m_cutsceneSaveButton.Enabled = false;
      if (this.m_cutscenes.Count > 0)
        this.m_cutsceneSelection.SelectItemByIndex(0);
      else
        this.UpdateCutsceneFields();
    }

    private void CutsceneTextbox_FocusChanged(MyGuiControlBase arg1, bool focused) => MyGuiScreenGamePlay.DisableInput = focused;

    private void UpdateCutscenes()
    {
      if (!this.m_cutscenePlaying || MySession.Static.GetComponent<MySessionComponentCutscenes>().IsCutsceneRunning)
        return;
      this.StopCutscene();
    }

    private void StopCutscene()
    {
      this.State = MyGuiScreenState.OPENED;
      MyDebugDrawSettings.ENABLE_DEBUG_DRAW = true;
      MySession.Static.SetCameraController(MyCameraControllerEnum.SpectatorFreeMouse, (IMyEntity) null, new Vector3D?());
      this.m_cutscenePlaying = false;
    }

    private void UpdateCutsceneFields()
    {
      string name = this.m_cutsceneSelection.GetSelectedIndex() >= 0 ? this.m_cutsceneSelection.GetSelectedValue().ToString() : "";
      this.m_cutsceneCurrent = (Cutscene) null;
      Cutscene cutsceneCopy = MySession.Static.GetComponent<MySessionComponentCutscenes>().GetCutsceneCopy(name);
      bool flag = cutsceneCopy != null;
      this.m_cutsceneDeleteButton.Enabled = flag;
      this.m_cutscenePlayButton.Enabled = flag;
      this.m_cutsceneSaveButton.Enabled = false;
      this.m_cutsceneRevertButton.Enabled = false;
      this.m_cutscenePropertyNextCutscene.Enabled = flag;
      this.m_cutscenePropertyNextCutscene.ClearItems();
      this.m_cutscenePropertyNextCutscene.AddItem(0L, MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_None));
      this.m_cutscenePropertyNextCutscene.SelectItemByIndex(0);
      this.m_cutscenePropertyStartEntity.Enabled = flag;
      this.m_cutscenePropertyStartLookAt.Enabled = flag;
      this.m_cutscenePropertyStartingFOV.Enabled = flag;
      this.m_cutscenePropertyCanBeSkipped.Enabled = flag;
      this.m_cutscenePropertyFireEventsDuringSkip.Enabled = flag;
      this.m_cutsceneNodes.ClearItems();
      if (flag)
      {
        this.m_cutscenePropertyStartEntity.Text = cutsceneCopy.StartEntity;
        this.m_cutscenePropertyStartLookAt.Text = cutsceneCopy.StartLookAt;
        this.m_cutscenePropertyStartingFOV.Text = cutsceneCopy.StartingFOV.ToString();
        this.m_cutscenePropertyCanBeSkipped.IsChecked = cutsceneCopy.CanBeSkipped;
        this.m_cutscenePropertyFireEventsDuringSkip.IsChecked = cutsceneCopy.FireEventsDuringSkip;
        foreach (string key in this.m_cutscenes.Keys)
        {
          if (!key.Equals(cutsceneCopy.Name))
          {
            this.m_cutscenePropertyNextCutscene.AddItem(key.GetHashCode64(), key);
            if (key.Equals(cutsceneCopy.NextCutscene))
              this.m_cutscenePropertyNextCutscene.SelectItemByKey(key.GetHashCode64());
          }
        }
        if (cutsceneCopy.SequenceNodes != null)
        {
          for (int index = 0; index < cutsceneCopy.SequenceNodes.Count; ++index)
            this.m_cutsceneNodes.Add(new MyGuiControlListbox.Item(new StringBuilder((index + 1).ToString() + ": " + cutsceneCopy.SequenceNodes[index].GetNodeSummary()), cutsceneCopy.SequenceNodes[index].GetNodeDescription()));
        }
      }
      this.m_cutsceneCurrent = cutsceneCopy;
      this.UpdateCutsceneNodeFields();
    }

    private void CutsceneChanged()
    {
      this.m_cutsceneSaveButton.Enabled = this.m_cutsceneCurrent != null;
      this.m_cutsceneRevertButton.Enabled = this.m_cutsceneCurrent != null;
      if (this.m_selectedCutsceneNodeIndex < 0)
        return;
      this.m_cutsceneNodes.ItemsSelected -= new Action<MyGuiControlListbox>(this.m_cutsceneNodes_ItemsSelected);
      this.m_cutsceneNodes.Items.RemoveAt(this.m_selectedCutsceneNodeIndex);
      this.m_cutsceneNodes.Items.Insert(this.m_selectedCutsceneNodeIndex, new MyGuiControlListbox.Item(new StringBuilder((this.m_selectedCutsceneNodeIndex + 1).ToString() + ": " + this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].GetNodeSummary()), this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].GetNodeDescription()));
      this.SelectListboxItemAtIndex(this.m_cutsceneNodes, this.m_selectedCutsceneNodeIndex);
      this.m_cutsceneNodes.ItemsSelected += new Action<MyGuiControlListbox>(this.m_cutsceneNodes_ItemsSelected);
    }

    private void UpdateCutsceneNodeFields()
    {
      bool flag = this.m_cutsceneCurrent != null && this.m_cutsceneNodes.SelectedItems.Count > 0;
      this.m_cutsceneNodeButtonMoveUp.Enabled = flag;
      this.m_cutsceneNodeButtonMoveDown.Enabled = flag;
      this.m_cutsceneNodeButtonDelete.Enabled = flag;
      this.m_cutsceneNodePropertyTime.Enabled = flag;
      this.m_cutsceneNodePropertyMoveTo.Enabled = flag;
      this.m_cutsceneNodePropertyMoveToInstant.Enabled = flag;
      this.m_cutsceneNodePropertyRotateLike.Enabled = flag;
      this.m_cutsceneNodePropertyRotateLikeInstant.Enabled = flag;
      this.m_cutsceneNodePropertyRotateTowards.Enabled = flag;
      this.m_cutsceneNodePropertyRotateTowardsInstant.Enabled = flag;
      this.m_cutsceneNodePropertyEvent.Enabled = flag;
      this.m_cutsceneNodePropertyEventDelay.Enabled = flag;
      this.m_cutsceneNodePropertyFOVChange.Enabled = flag;
      this.m_cutsceneNodePropertyRotateTowardsLock.Enabled = flag;
      this.m_cutsceneNodePropertyAttachAll.Enabled = flag;
      this.m_cutsceneNodePropertyAttachPosition.Enabled = flag;
      this.m_cutsceneNodePropertyAttachRotation.Enabled = flag;
      this.m_cutsceneNodePropertyWaypoints.Enabled = flag;
      if (!flag)
        return;
      this.m_selectedCutsceneNodeIndex = this.GetListboxSelectedIndex(this.m_cutsceneNodes);
      this.m_cutsceneNodeButtonMoveUp.Enabled = this.m_selectedCutsceneNodeIndex > 0 && this.m_cutsceneNodes.Items.Count > 1;
      this.m_cutsceneNodeButtonMoveDown.Enabled = this.m_selectedCutsceneNodeIndex < this.m_cutsceneNodes.Items.Count - 1;
      this.m_cutsceneNodePropertyTime.Text = Math.Max(this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].Time, 0.0f).ToString();
      this.m_cutsceneNodePropertyMoveTo.Text = this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].MoveTo != null ? this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].MoveTo : "";
      this.m_cutsceneNodePropertyMoveToInstant.Text = this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].SetPositionTo != null ? this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].SetPositionTo : "";
      this.m_cutsceneNodePropertyRotateLike.Text = this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].RotateLike != null ? this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].RotateLike : "";
      this.m_cutsceneNodePropertyRotateLikeInstant.Text = this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].SetRotationLike != null ? this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].SetRotationLike : "";
      this.m_cutsceneNodePropertyRotateTowards.Text = this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].RotateTowards != null ? this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].RotateTowards : "";
      this.m_cutsceneNodePropertyRotateTowardsInstant.Text = this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].LookAt != null ? this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].LookAt : "";
      this.m_cutsceneNodePropertyEvent.Text = this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].Event != null ? this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].Event : "";
      this.m_cutsceneNodePropertyEventDelay.Text = Math.Max(this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].EventDelay, 0.0f).ToString();
      this.m_cutsceneNodePropertyFOVChange.Text = Math.Max(this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].ChangeFOVTo, 0.0f).ToString();
      this.m_cutsceneNodePropertyRotateTowardsLock.Text = this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].LockRotationTo == null ? "" : (this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].LockRotationTo.Length > 0 ? this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].LockRotationTo : "X");
      this.m_cutsceneNodePropertyAttachAll.Text = this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].AttachTo == null ? "" : (this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].AttachTo.Length > 0 ? this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].AttachTo : "X");
      this.m_cutsceneNodePropertyAttachPosition.Text = this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].AttachPositionTo == null ? "" : (this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].AttachPositionTo.Length > 0 ? this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].AttachPositionTo : "X");
      this.m_cutsceneNodePropertyAttachRotation.Text = this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].AttachRotationTo == null ? "" : (this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].AttachRotationTo.Length > 0 ? this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].AttachRotationTo : "X");
      if (this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].Waypoints != null)
      {
        StringBuilder stringBuilder = new StringBuilder();
        for (int index = 0; index < this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].Waypoints.Count; ++index)
        {
          if (index > 0)
            stringBuilder.Append(";");
          stringBuilder.Append(this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].Waypoints[index].Name);
        }
        this.m_cutsceneNodePropertyWaypoints.Text = stringBuilder.ToString();
      }
      else
        this.m_cutsceneNodePropertyWaypoints.Text = "";
    }

    private void CloseScreenWithSave()
    {
      if (this.m_cutsceneSaveButton.Enabled)
      {
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.Cutscene_Unsaved_Caption);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO_CANCEL, messageText: MyTexts.Get(MyCommonTexts.Cutscene_Unsaved_Text), messageCaption: messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result =>
        {
          if (result == MyGuiScreenMessageBox.ResultEnum.YES)
            this.SaveCutsceneClicked(this.m_cutsceneSaveButton);
          if (result != MyGuiScreenMessageBox.ResultEnum.YES && result != MyGuiScreenMessageBox.ResultEnum.NO)
            return;
          this.CloseScreen(false);
        }))));
      }
      else
        this.CloseScreen(false);
    }

    private void m_cutsceneSelection_ItemSelected()
    {
      if (this.m_cutsceneSaveButton.Enabled)
      {
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.Cutscene_Unsaved_Text);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO_CANCEL, messageText: MyTexts.Get(MyCommonTexts.Cutscene_Unsaved_Caption), messageCaption: messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result =>
        {
          if (result == MyGuiScreenMessageBox.ResultEnum.YES)
            this.SaveCutsceneClicked(this.m_cutsceneSaveButton);
          if (result != MyGuiScreenMessageBox.ResultEnum.YES && result != MyGuiScreenMessageBox.ResultEnum.NO)
            return;
          this.UpdateCutsceneFields();
        }))));
      }
      else
        this.UpdateCutsceneFields();
    }

    private void WatchCutsceneClicked(MyGuiControlButton myGuiControlButton)
    {
      if (this.m_cutsceneSelection.GetSelectedValue() != null && !this.m_cutscenePlaying)
      {
        MyDebugDrawSettings.ENABLE_DEBUG_DRAW = false;
        MySession.Static.GetComponent<MySessionComponentCutscenes>().PlayCutscene(this.m_cutsceneCurrent, false);
        this.State = MyGuiScreenState.HIDDEN;
        this.m_cutscenePlaying = true;
      }
      else
      {
        MySession.Static.GetComponent<MySessionComponentCutscenes>().CutsceneEnd(copyToSpectator: true);
        this.StopCutscene();
      }
    }

    private void SaveCutsceneClicked(MyGuiControlButton myGuiControlButton)
    {
      if (this.m_cutsceneCurrent == null)
        return;
      this.m_cutscenes[this.m_cutsceneCurrent.Name] = this.m_cutsceneCurrent;
      this.m_cutsceneSaveButton.Enabled = false;
      this.m_cutsceneRevertButton.Enabled = false;
    }

    private void RevertCutsceneClicked(MyGuiControlButton myGuiControlButton)
    {
      StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.Cutscene_Revert_Caption);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MyCommonTexts.Cutscene_Revert_Text), messageCaption: messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result =>
      {
        if (result != MyGuiScreenMessageBox.ResultEnum.YES)
          return;
        this.UpdateCutsceneFields();
      }))));
    }

    private void ClearAllCutscenesClicked(MyGuiControlButton myGuiControlButton)
    {
      StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.Cutscene_DeleteAll_Caption);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MyCommonTexts.Cutscene_DeleteAll_Text), messageCaption: messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result =>
      {
        if (result != MyGuiScreenMessageBox.ResultEnum.YES)
          return;
        this.m_cutscenes.Clear();
        this.m_cutsceneSelection.ClearItems();
        this.UpdateCutsceneFields();
      }))));
    }

    private void DeleteCurrentCutsceneClicked(MyGuiControlButton myGuiControlButton)
    {
      if (this.m_cutsceneSelection.GetSelectedIndex() < 0)
        return;
      StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.Cutscene_Delete_Caption);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.Cutscene_Delete_Text), (object) this.m_cutsceneSelection.GetItemByIndex(this.m_cutsceneSelection.GetSelectedIndex()).Value), messageCaption: messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result =>
      {
        if (result != MyGuiScreenMessageBox.ResultEnum.YES)
          return;
        this.m_cutscenes.Remove(this.m_cutsceneSelection.GetItemByIndex(this.m_cutsceneSelection.GetSelectedIndex()).Value.ToString());
        this.m_cutsceneSelection.RemoveItemByIndex(this.m_cutsceneSelection.GetSelectedIndex());
        if (this.m_cutscenes.Count > 0)
          this.m_cutsceneSelection.SelectItemByIndex(0);
        else
          this.UpdateCutsceneFields();
      }))));
    }

    private void CreateNewCutsceneClicked(MyGuiControlButton myGuiControlButton)
    {
      if (this.m_cutsceneSaveButton.Enabled)
      {
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.Cutscene_Unsaved_Caption);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO_CANCEL, messageText: MyTexts.Get(MyCommonTexts.Cutscene_Unsaved_Text), messageCaption: messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result =>
        {
          if (result == MyGuiScreenMessageBox.ResultEnum.YES)
            this.SaveCutsceneClicked(this.m_cutsceneSaveButton);
          if (result != MyGuiScreenMessageBox.ResultEnum.YES && result != MyGuiScreenMessageBox.ResultEnum.NO)
            return;
          this.NewCutscene();
        }))));
      }
      else
        this.NewCutscene();
    }

    private void NewCutscene() => MyGuiSandbox.AddScreen((MyGuiScreenBase) new ValueGetScreenWithCaption(MyTexts.Get(MyCommonTexts.Cutscene_New_Caption).ToString(), "", (ValueGetScreenWithCaption.ValueGetScreenAction) (text =>
    {
      if (this.m_cutscenes.ContainsKey(text))
        return false;
      this.m_cutscenes.Add(text, new Cutscene()
      {
        Name = text
      });
      long hashCode64 = text.GetHashCode64();
      this.m_cutsceneSelection.AddItem(hashCode64, text);
      this.m_cutsceneSelection.SelectItemByKey(hashCode64);
      return true;
    })));

    private void CutscenePropertyCanBeSkippedChanged(MyGuiControlCheckbox checkbox)
    {
      if (this.m_cutsceneCurrent == null)
        return;
      this.m_cutsceneCurrent.CanBeSkipped = checkbox.IsChecked;
      this.CutsceneChanged();
    }

    private void CutscenePropertyFireEventsDuringSkipChanged(MyGuiControlCheckbox checkbox)
    {
      if (this.m_cutsceneCurrent == null)
        return;
      this.m_cutsceneCurrent.FireEventsDuringSkip = checkbox.IsChecked;
      this.CutsceneChanged();
    }

    private void CutscenePropertyStartEntity_TextChanged(MyGuiControlTextbox obj)
    {
      if (this.m_cutsceneCurrent == null)
        return;
      this.m_cutsceneCurrent.StartEntity = obj.Text;
      this.CutsceneChanged();
    }

    private void CutscenePropertyStartLookAt_TextChanged(MyGuiControlTextbox obj)
    {
      if (this.m_cutsceneCurrent == null)
        return;
      this.m_cutsceneCurrent.StartLookAt = obj.Text;
      this.CutsceneChanged();
    }

    private void CutscenePropertyStartingFOV_TextChanged(MyGuiControlTextbox obj)
    {
      if (this.m_cutsceneCurrent == null)
        return;
      float result;
      this.m_cutsceneCurrent.StartingFOV = !float.TryParse(obj.Text, out result) ? 70f : result;
      this.CutsceneChanged();
    }

    private void CutscenePropertyNextCutscene_ItemSelected()
    {
      if (this.m_cutsceneCurrent == null)
        return;
      this.m_cutsceneCurrent.NextCutscene = this.m_cutscenePropertyNextCutscene.GetSelectedKey() != 0L ? this.m_cutscenePropertyNextCutscene.GetSelectedValue().ToString() : (string) null;
      this.CutsceneChanged();
    }

    private void m_cutsceneNodes_ItemsSelected(MyGuiControlListbox obj)
    {
      bool enabled = this.m_cutsceneSaveButton.Enabled;
      this.m_selectedCutsceneNodeIndex = this.GetListboxSelectedIndex(this.m_cutsceneNodes);
      this.UpdateCutsceneNodeFields();
      this.m_cutsceneSaveButton.Enabled = enabled;
      this.m_cutsceneRevertButton.Enabled = enabled;
    }

    private void CutsceneNodeButtonAddClicked(MyGuiControlButton myGuiControlButton)
    {
      if (this.m_cutsceneCurrent == null)
        return;
      if (this.m_cutsceneCurrent.SequenceNodes == null)
        this.m_cutsceneCurrent.SequenceNodes = new List<CutsceneSequenceNode>();
      CutsceneSequenceNode cutsceneSequenceNode = new CutsceneSequenceNode();
      this.m_cutsceneCurrent.SequenceNodes.Add(cutsceneSequenceNode);
      this.m_cutsceneNodes.Add(new MyGuiControlListbox.Item(new StringBuilder(this.m_cutsceneCurrent.SequenceNodes.Count.ToString() + ": " + cutsceneSequenceNode.GetNodeSummary()), cutsceneSequenceNode.GetNodeDescription()));
      this.SelectListboxItemAtIndex(this.m_cutsceneNodes, this.m_cutsceneCurrent.SequenceNodes.Count - 1);
      this.CutsceneChanged();
    }

    private void CutsceneNodeButtonDeleteAllClicked(MyGuiControlButton myGuiControlButton)
    {
      if (this.m_cutsceneCurrent == null)
        return;
      StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.Cutscene_DeleteAllNodes_Caption);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MyCommonTexts.Cutscene_DeleteAllNodes_Text), messageCaption: messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result =>
      {
        if (result != MyGuiScreenMessageBox.ResultEnum.YES)
          return;
        this.m_cutsceneCurrent.SequenceNodes.Clear();
        this.m_cutsceneCurrent.SequenceNodes = (List<CutsceneSequenceNode>) null;
        this.m_cutsceneNodes.ClearItems();
        this.UpdateCutsceneNodeFields();
        this.m_cutsceneNodes.ScrollToolbarToTop();
        this.CutsceneChanged();
      }))));
    }

    private void CutsceneNodeButtonDeleteClicked(MyGuiControlButton myGuiControlButton)
    {
      if (this.m_cutsceneCurrent == null)
        return;
      StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.Cutscene_DeleteNode_Caption);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MyCommonTexts.Cutscene_DeleteNode_Text), messageCaption: messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result =>
      {
        if (result != MyGuiScreenMessageBox.ResultEnum.YES || this.m_selectedCutsceneNodeIndex < 0)
          return;
        this.m_cutsceneCurrent.SequenceNodes.RemoveAt(this.m_selectedCutsceneNodeIndex);
        this.m_cutsceneNodes.Items.RemoveAt(this.m_selectedCutsceneNodeIndex);
        this.SelectListboxItemAtIndex(this.m_cutsceneNodes, this.m_selectedCutsceneNodeIndex);
        this.CutsceneChanged();
      }))));
    }

    private void CutsceneNodeButtonMoveUpClicked(MyGuiControlButton myGuiControlButton)
    {
      int listboxSelectedIndex = this.GetListboxSelectedIndex(this.m_cutsceneNodes);
      if (this.m_cutsceneCurrent == null || listboxSelectedIndex < 0)
        return;
      CutsceneSequenceNode sequenceNode = this.m_cutsceneCurrent.SequenceNodes[listboxSelectedIndex];
      this.m_cutsceneCurrent.SequenceNodes.RemoveAt(listboxSelectedIndex);
      this.m_cutsceneCurrent.SequenceNodes.Insert(listboxSelectedIndex - 1, sequenceNode);
      MyGuiControlListbox.Item obj = this.m_cutsceneNodes.Items[listboxSelectedIndex];
      this.m_cutsceneNodes.Items.RemoveAt(listboxSelectedIndex);
      this.m_cutsceneNodes.Items.Insert(listboxSelectedIndex - 1, obj);
      this.SelectListboxItemAtIndex(this.m_cutsceneNodes, listboxSelectedIndex - 1);
      this.CutsceneChanged();
    }

    private void CutsceneNodeButtonMoveDownClicked(MyGuiControlButton myGuiControlButton)
    {
      int listboxSelectedIndex = this.GetListboxSelectedIndex(this.m_cutsceneNodes);
      if (this.m_cutsceneCurrent == null || listboxSelectedIndex < 0)
        return;
      CutsceneSequenceNode sequenceNode = this.m_cutsceneCurrent.SequenceNodes[listboxSelectedIndex];
      this.m_cutsceneCurrent.SequenceNodes.RemoveAt(listboxSelectedIndex);
      this.m_cutsceneCurrent.SequenceNodes.Insert(listboxSelectedIndex + 1, sequenceNode);
      MyGuiControlListbox.Item obj = this.m_cutsceneNodes.Items[listboxSelectedIndex];
      this.m_cutsceneNodes.Items.RemoveAt(listboxSelectedIndex);
      this.m_cutsceneNodes.Items.Insert(listboxSelectedIndex + 1, obj);
      this.SelectListboxItemAtIndex(this.m_cutsceneNodes, listboxSelectedIndex + 1);
      this.CutsceneChanged();
    }

    private void CutsceneNodePropertyMoveTo_TextChanged(MyGuiControlTextbox obj)
    {
      if (this.m_cutsceneCurrent == null || this.m_selectedCutsceneNodeIndex < 0)
        return;
      this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].MoveTo = obj.Text.Length > 0 ? obj.Text : (string) null;
      this.CutsceneChanged();
    }

    private void CutsceneNodePropertyMoveToInstant_TextChanged(MyGuiControlTextbox obj)
    {
      if (this.m_cutsceneCurrent == null || this.m_selectedCutsceneNodeIndex < 0)
        return;
      this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].SetPositionTo = obj.Text.Length > 0 ? obj.Text : (string) null;
      this.CutsceneChanged();
    }

    private void CutsceneNodePropertyRotateLike_TextChanged(MyGuiControlTextbox obj)
    {
      if (this.m_cutsceneCurrent == null || this.m_selectedCutsceneNodeIndex < 0)
        return;
      this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].RotateLike = obj.Text.Length > 0 ? obj.Text : (string) null;
      this.CutsceneChanged();
    }

    private void CutsceneNodePropertyRotateLikeInstant_TextChanged(MyGuiControlTextbox obj)
    {
      if (this.m_cutsceneCurrent == null || this.m_selectedCutsceneNodeIndex < 0)
        return;
      this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].SetRotationLike = obj.Text.Length > 0 ? obj.Text : (string) null;
      this.CutsceneChanged();
    }

    private void CutsceneNodePropertyLookAt_TextChanged(MyGuiControlTextbox obj)
    {
      if (this.m_cutsceneCurrent == null || this.m_selectedCutsceneNodeIndex < 0)
        return;
      this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].RotateTowards = obj.Text.Length > 0 ? obj.Text : (string) null;
      this.CutsceneChanged();
    }

    private void CutsceneNodePropertyLookAtInstant_TextChanged(MyGuiControlTextbox obj)
    {
      if (this.m_cutsceneCurrent == null || this.m_selectedCutsceneNodeIndex < 0)
        return;
      this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].LookAt = obj.Text.Length > 0 ? obj.Text : (string) null;
      this.CutsceneChanged();
    }

    private void CutsceneNodePropertyEvent_TextChanged(MyGuiControlTextbox obj)
    {
      if (this.m_cutsceneCurrent == null || this.m_selectedCutsceneNodeIndex < 0)
        return;
      this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].Event = obj.Text.Length > 0 ? obj.Text : (string) null;
      this.CutsceneChanged();
    }

    private void CutsceneNodePropertyTime_TextChanged(MyGuiControlTextbox obj)
    {
      if (this.m_cutsceneCurrent == null || this.m_selectedCutsceneNodeIndex < 0)
        return;
      float result;
      this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].Time = !float.TryParse(obj.Text, out result) ? 0.0f : Math.Max(0.0f, result);
      this.CutsceneChanged();
    }

    private void CutsceneNodePropertyFOV_TextChanged(MyGuiControlTextbox obj)
    {
      if (this.m_cutsceneCurrent == null || this.m_selectedCutsceneNodeIndex < 0)
        return;
      float result;
      this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].ChangeFOVTo = !float.TryParse(obj.Text, out result) ? 0.0f : Math.Max(0.0f, result);
      this.CutsceneChanged();
    }

    private void CutsceneNodePropertyEventDelay_TextChanged(MyGuiControlTextbox obj)
    {
      if (this.m_cutsceneCurrent == null || this.m_selectedCutsceneNodeIndex < 0)
        return;
      float result;
      this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].EventDelay = !float.TryParse(obj.Text, out result) ? 0.0f : Math.Max(0.0f, result);
      this.CutsceneChanged();
    }

    private void CutsceneNodePropertyAttachTo_TextChanged(MyGuiControlTextbox obj)
    {
      if (this.m_cutsceneCurrent == null || this.m_selectedCutsceneNodeIndex < 0)
        return;
      this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].AttachTo = obj.Text.Length > 0 ? (obj.Text.Length > 1 || !obj.Text.ToUpper().Equals("X") ? obj.Text : "") : (string) null;
      this.CutsceneChanged();
    }

    private void CutsceneNodePropertyAttachPositionTo_TextChanged(MyGuiControlTextbox obj)
    {
      if (this.m_cutsceneCurrent == null || this.m_selectedCutsceneNodeIndex < 0)
        return;
      this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].AttachPositionTo = obj.Text.Length > 0 ? (obj.Text.Length > 1 || !obj.Text.ToUpper().Equals("X") ? obj.Text : "") : (string) null;
      this.CutsceneChanged();
    }

    private void CutsceneNodePropertyAttachRotationTo_TextChanged(MyGuiControlTextbox obj)
    {
      if (this.m_cutsceneCurrent == null || this.m_selectedCutsceneNodeIndex < 0)
        return;
      this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].AttachRotationTo = obj.Text.Length > 0 ? (obj.Text.Length > 1 || !obj.Text.ToUpper().Equals("X") ? obj.Text : "") : (string) null;
      this.CutsceneChanged();
    }

    private void CutsceneNodePropertyLockRotationTo_TextChanged(MyGuiControlTextbox obj)
    {
      if (this.m_cutsceneCurrent == null || this.m_selectedCutsceneNodeIndex < 0)
        return;
      this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].LockRotationTo = obj.Text.Length > 0 ? (obj.Text.Length > 1 || !obj.Text.ToUpper().Equals("X") ? obj.Text : "") : (string) null;
      this.CutsceneChanged();
    }

    private void CutsceneNodePropertyWaypoints_TextChanged(MyGuiControlTextbox obj)
    {
      if (this.m_cutsceneCurrent == null || this.m_selectedCutsceneNodeIndex < 0)
        return;
      bool flag = obj.Text.Length == 0;
      if (!flag)
      {
        string[] strArray = obj.Text.Split(new string[1]
        {
          ";"
        }, StringSplitOptions.RemoveEmptyEntries);
        if (strArray.Length != 0)
        {
          if (this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].Waypoints == null)
            this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].Waypoints = new List<CutsceneSequenceNodeWaypoint>();
          this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].Waypoints.Clear();
          foreach (string str in strArray)
            this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].Waypoints.Add(new CutsceneSequenceNodeWaypoint()
            {
              Name = str
            });
        }
        else
          flag = true;
      }
      if (flag)
      {
        if (this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].Waypoints != null)
          this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].Waypoints.Clear();
        this.m_cutsceneCurrent.SequenceNodes[this.m_selectedCutsceneNodeIndex].Waypoints = (List<CutsceneSequenceNodeWaypoint>) null;
      }
      this.CutsceneChanged();
    }

    private MyGuiControlCheckbox CreateCheckbox(
      Action<MyGuiControlCheckbox> onCheckedChanged,
      bool isChecked,
      string tooltip = null)
    {
      MyGuiControlCheckbox control = new MyGuiControlCheckbox(isChecked: isChecked, visualStyle: MyGuiControlCheckboxStyleEnum.Debug, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      if (!string.IsNullOrEmpty(tooltip))
        control.SetTooltip(tooltip);
      control.Size = MyGuiScreenCutscenes.ITEM_SIZE;
      control.IsCheckedChanged += onCheckedChanged;
      this.Controls.Add((MyGuiControlBase) control);
      return control;
    }

    private MyGuiControlTextbox CreateTextbox(
      string text,
      Action<MyGuiControlTextbox> textChanged = null)
    {
      MyGuiControlTextbox guiControlTextbox = new MyGuiControlTextbox(defaultText: text, visualStyle: MyGuiControlTextboxStyleEnum.Debug);
      guiControlTextbox.Enabled = false;
      guiControlTextbox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      guiControlTextbox.Size = MyGuiScreenCutscenes.ITEM_SIZE;
      guiControlTextbox.TextChanged += textChanged;
      guiControlTextbox.FocusChanged += new Action<MyGuiControlBase, bool>(this.CutsceneTextbox_FocusChanged);
      this.Controls.Add((MyGuiControlBase) guiControlTextbox);
      return guiControlTextbox;
    }

    private MyGuiControlLabel CreateLabel(string text)
    {
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(size: new Vector2?(MyGuiScreenCutscenes.ITEM_SIZE), text: text, font: "Debug", originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      return myGuiControlLabel;
    }

    private MyGuiControlListbox CreateListBox()
    {
      MyGuiControlListbox guiControlListbox1 = new MyGuiControlListbox(visualStyle: MyGuiControlListboxStyleEnum.Blueprints);
      guiControlListbox1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      guiControlListbox1.Size = new Vector2(1f, 0.15f);
      MyGuiControlListbox guiControlListbox2 = guiControlListbox1;
      guiControlListbox2.MultiSelect = false;
      guiControlListbox2.Enabled = true;
      guiControlListbox2.ItemSize = new Vector2(MyGuiScreenCutscenes.SCREEN_SIZE.X, MyGuiScreenCutscenes.ITEM_SIZE.Y);
      guiControlListbox2.TextScale = 0.6f;
      guiControlListbox2.VisibleRowsCount = 7;
      this.Controls.Add((MyGuiControlBase) guiControlListbox2);
      return guiControlListbox2;
    }

    private MyGuiControlCombobox CreateComboBox()
    {
      MyGuiControlCombobox guiControlCombobox1 = new MyGuiControlCombobox();
      guiControlCombobox1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      guiControlCombobox1.Size = MyGuiScreenCutscenes.BUTTON_SIZE;
      guiControlCombobox1.Position = new Vector2(this.m_buttonXOffset, this.m_currentPosition.Y);
      MyGuiControlCombobox guiControlCombobox2 = guiControlCombobox1;
      guiControlCombobox2.Enabled = true;
      this.Controls.Add((MyGuiControlBase) guiControlCombobox2);
      return guiControlCombobox2;
    }

    private MyGuiControlButton CreateButton(
      string text,
      Action<MyGuiControlButton> onClick,
      string tooltip = null)
    {
      MyGuiControlButton control = new MyGuiControlButton(new Vector2?(new Vector2(this.m_buttonXOffset, this.m_currentPosition.Y)), MyGuiControlButtonStyleEnum.Rectangular, colorMask: new Vector4?(Color.Yellow.ToVector4()), text: new StringBuilder(text), textScale: (0.8f * MyGuiConstants.DEBUG_BUTTON_TEXT_SCALE * this.m_scale), onButtonClick: onClick);
      if (!string.IsNullOrEmpty(tooltip))
        control.SetTooltip(tooltip);
      control.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      control.Size = MyGuiScreenCutscenes.BUTTON_SIZE;
      this.Controls.Add((MyGuiControlBase) control);
      return control;
    }

    private int GetListboxSelectedIndex(MyGuiControlListbox listbox)
    {
      if (listbox.SelectedItems.Count == 0)
        return -1;
      for (int index = 0; index < listbox.Items.Count; ++index)
      {
        if (listbox.Items[index] == listbox.SelectedItems[0])
          return index;
      }
      return -1;
    }

    private void SelectListboxItemAtIndex(MyGuiControlListbox listbox, int index)
    {
      List<bool> states = new List<bool>();
      for (int index1 = 0; index1 < this.m_cutsceneCurrent.SequenceNodes.Count; ++index1)
        states.Add(index1 == index);
      this.m_cutsceneNodes.ChangeSelection(states);
    }

    private void PositionControl(MyGuiControlBase control)
    {
      float x = (float) ((double) MyGuiScreenCutscenes.SCREEN_SIZE.X - (double) MyGuiScreenCutscenes.HIDDEN_PART_RIGHT - (double) MyGuiScreenCutscenes.ITEM_HORIZONTAL_PADDING * 2.0);
      Vector2 size = control.Size;
      control.Position = new Vector2(this.m_currentPosition.X - MyGuiScreenCutscenes.SCREEN_SIZE.X / 2f + MyGuiScreenCutscenes.ITEM_HORIZONTAL_PADDING, this.m_currentPosition.Y + MyGuiScreenCutscenes.ITEM_VERTICAL_PADDING);
      control.Size = new Vector2(x, size.Y);
      this.m_currentPosition.Y += control.Size.Y + MyGuiScreenCutscenes.ITEM_VERTICAL_PADDING;
    }

    private void PositionControls(MyGuiControlBase[] controls)
    {
      float x = (float) (((double) MyGuiScreenCutscenes.SCREEN_SIZE.X - (double) MyGuiScreenCutscenes.HIDDEN_PART_RIGHT - (double) MyGuiScreenCutscenes.ITEM_HORIZONTAL_PADDING * 2.0) / (double) controls.Length - 1.0 / 1000.0 * (double) controls.Length);
      float num1 = x + 1f / 1000f * (float) controls.Length;
      float num2 = 0.0f;
      for (int index = 0; index < controls.Length; ++index)
      {
        MyGuiControlBase control = controls[index];
        control.Size = control is MyGuiControlCheckbox ? new Vector2(MyGuiScreenCutscenes.BUTTON_SIZE.Y) : new Vector2(x, control.Size.Y);
        control.PositionX = (float) ((double) this.m_currentPosition.X + (double) num1 * (double) index - (double) MyGuiScreenCutscenes.SCREEN_SIZE.X / 2.0) + MyGuiScreenCutscenes.ITEM_HORIZONTAL_PADDING;
        control.PositionY = this.m_currentPosition.Y + MyGuiScreenCutscenes.ITEM_VERTICAL_PADDING;
        if ((double) control.Size.Y > (double) num2)
          num2 = control.Size.Y;
      }
      this.m_currentPosition.Y += num2 + MyGuiScreenCutscenes.ITEM_VERTICAL_PADDING;
    }
  }
}
