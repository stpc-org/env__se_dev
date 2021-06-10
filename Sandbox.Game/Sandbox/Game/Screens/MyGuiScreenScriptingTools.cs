// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenScriptingTools
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Debugging;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI.DebugInputComponents;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Components.Session;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.ObjectBuilders.Gui;
using VRage.Game.SessionComponents;
using VRage.Game.VisualScripting.Missions;
using VRage.Generics;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenScriptingTools : MyGuiScreenDebugBase
  {
    private static readonly Vector2 SCREEN_SIZE = new Vector2(0.4f, 1.2f);
    private static readonly float HIDDEN_PART_RIGHT = 0.04f;
    private static readonly float ITEM_HORIZONTAL_PADDING = 0.01f;
    private static readonly float ITEM_VERTICAL_PADDING = 0.005f;
    private static readonly Vector2 BUTTON_SIZE = new Vector2(0.06f, 0.03f);
    private static readonly Vector2 ITEM_SIZE = new Vector2(0.06f, 0.02f);
    public static MyGuiScreenScriptingTools Static;
    private static uint m_entityCounter = 0;
    private IMyCameraController m_previousCameraController;
    private MyGuiControlButton m_setTriggerSizeButton;
    private MyGuiControlButton m_growTriggerButton;
    private MyGuiControlButton m_shrinkTriggerButton;
    private MyGuiControlListbox m_triggersListBox;
    private MyGuiControlListbox m_waypointsListBox;
    private MyGuiControlListbox m_smListBox;
    private MyGuiControlListbox m_levelScriptListBox;
    private MyGuiControlTextbox m_selectedTriggerNameBox;
    private MyGuiControlTextbox m_selectedEntityNameBox;
    private MyGuiControlTextbox m_selectedFunctionalBlockNameBox;
    private MyEntity m_selectedFunctionalBlock;
    private bool m_disablePicking;
    private readonly MyTriggerManipulator m_triggerManipulator;
    private readonly MyEntityTransformationSystem m_transformSys;
    private List<MyWaypoint> m_waypoints = new List<MyWaypoint>();
    private int m_selectedAxis;

    public MyGuiScreenScriptingTools()
      : base(new Vector2(MyGuiManager.GetMaxMouseCoord().X - MyGuiScreenScriptingTools.SCREEN_SIZE.X * 0.5f + MyGuiScreenScriptingTools.HIDDEN_PART_RIGHT, 0.5f), new Vector2?(MyGuiScreenScriptingTools.SCREEN_SIZE), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), false)
    {
      this.CanBeHidden = true;
      this.CanHideOthers = false;
      this.m_canCloseInCloseAllScreenCalls = true;
      this.m_canShareInput = true;
      this.m_isTopScreen = false;
      this.m_isTopMostScreen = false;
      MyGuiScreenScriptingTools.Static = this;
      this.m_triggerManipulator = new MyTriggerManipulator((Predicate<MyTriggerComponent>) (trigger => trigger is MyAreaTriggerComponent));
      this.m_transformSys = MySession.Static.GetComponent<MyEntityTransformationSystem>();
      this.m_transformSys.ControlledEntityChanged += new Action<MyEntity, MyEntity>(this.TransformSysOnControlledEntityChanged);
      this.m_transformSys.RayCasted += new Action<LineD>(this.TransformSysOnRayCasted);
      MySession.Static.SetCameraController(MyCameraControllerEnum.SpectatorFreeMouse, (IMyEntity) null, new Vector3D?());
      MyDebugDrawSettings.ENABLE_DEBUG_DRAW = true;
      MyDebugDrawSettings.DEBUG_DRAW_UPDATE_TRIGGER = true;
      MyDebugDrawSettings.DEBUG_DRAW_WAYPOINTS = true;
      MyDebugDrawSettings.DEBUG_DRAW_CUTSCENES = true;
      this.RecreateControls(true);
      this.InitializeWaypointList();
      this.UpdateWaypointList();
      this.UpdateTriggerList();
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      if (this.m_transformSys.DisablePicking)
        this.m_transformSys.DisablePicking = false;
      if (MyInput.Static.IsNewPrimaryButtonPressed() && (double) MyGuiManager.GetNormalizedCoordinateFromScreenCoordinate(MyInput.Static.GetMousePosition()).X > (double) (this.GetPosition() - MyGuiScreenScriptingTools.SCREEN_SIZE * 0.5f).X)
        this.m_transformSys.DisablePicking = true;
      if (!MyToolbarComponent.IsToolbarControlShown)
        MyToolbarComponent.IsToolbarControlShown = true;
      this.FocusedControl = (MyGuiControlBase) null;
      if (MyInput.Static.IsNewKeyPressed(MyKeys.Escape))
        this.CloseScreen(false);
      else if (MyInput.Static.IsNewKeyPressed(MyKeys.F11))
      {
        this.CloseScreen(false);
        MyScreenManager.AddScreen((MyGuiScreenBase) new MyGuiScreenCutscenes());
      }
      else
      {
        base.HandleInput(receivedFocusInThisUpdate);
        if (MySpectatorCameraController.Static.SpectatorCameraMovement != MySpectatorCameraMovementEnum.FreeMouse)
          MySpectatorCameraController.Static.SpectatorCameraMovement = MySpectatorCameraMovementEnum.FreeMouse;
        foreach (MyGuiScreenBase screen in MyScreenManager.Screens)
        {
          if (!(screen is MyGuiScreenScriptingTools))
            screen.HandleInput(receivedFocusInThisUpdate);
        }
        this.HandleShortcuts();
      }
    }

    private void HandleShortcuts()
    {
      if (MyInput.Static.IsAnyShiftKeyPressed() || MyInput.Static.IsAnyCtrlKeyPressed() || MyInput.Static.IsAnyAltKeyPressed())
        return;
      if (MyInput.Static.IsNewKeyPressed(MyKeys.Add))
        this.GrowTriggerOnClick((MyGuiControlButton) null);
      if (!MyInput.Static.IsNewKeyPressed(MyKeys.Subtract))
        return;
      this.ShrinkTriggerOnClick((MyGuiControlButton) null);
    }

    public override bool CloseScreen(bool isUnloading = false)
    {
      MySpectatorCameraController.Static.SpectatorCameraMovement = MySpectatorCameraMovementEnum.UserControlled;
      if (MySession.Static.ControlledEntity != null)
        MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, (IMyEntity) MySession.Static.ControlledEntity.Entity, new Vector3D?());
      MyDebugDrawSettings.ENABLE_DEBUG_DRAW = false;
      MyDebugDrawSettings.DEBUG_DRAW_UPDATE_TRIGGER = false;
      MyDebugDrawSettings.DEBUG_DRAW_WAYPOINTS = false;
      MyDebugDrawSettings.DEBUG_DRAW_CUTSCENES = false;
      this.m_transformSys.Active = false;
      MyGuiScreenGamePlay.DisableInput = MySession.Static.GetComponent<MySessionComponentCutscenes>().IsCutsceneRunning;
      MyGuiScreenScriptingTools.Static = (MyGuiScreenScriptingTools) null;
      return base.CloseScreen(isUnloading);
    }

    public override bool Update(bool hasFocus)
    {
      if (MyCubeBuilder.Static.CubeBuilderState.CurrentBlockDefinition != null || MyInput.Static.IsRightMousePressed())
        this.DrawMouseCursor = false;
      else
        this.DrawMouseCursor = true;
      this.m_triggerManipulator.CurrentPosition = MyAPIGateway.Session.Camera.Position;
      MyVisualScriptManagerSessionComponent component = MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>();
      MySession.Static.GetComponent<MySessionComponentScriptSharedStorage>();
      if (component == null)
        return false;
      if (component.FailedLevelScriptExceptionTexts != null)
      {
        for (int index = 0; index < component.FailedLevelScriptExceptionTexts.Length; ++index)
        {
          string scriptExceptionText = component.FailedLevelScriptExceptionTexts[index];
          if (scriptExceptionText != null && (bool) this.m_levelScriptListBox.Items[index].UserData)
          {
            this.m_levelScriptListBox.Items[index].Text.Append(" - failed");
            this.m_levelScriptListBox.Items[index].ColorMask = new Vector4?(MyTerminalControlPanel.RED_TEXT_COLOR);
            this.m_levelScriptListBox.Items[index].ToolTip.AddToolTip(scriptExceptionText, font: "Red");
          }
        }
      }
      if (component.SMManager != null && component.SMManager.RunningMachines != null)
      {
        foreach (MyVSStateMachine runningMachine in component.SMManager.RunningMachines)
        {
          MyVSStateMachine stateMachine = runningMachine;
          int index1 = this.m_smListBox.Items.FindIndex((Predicate<MyGuiControlListbox.Item>) (item => (MyVSStateMachine) item.UserData == stateMachine));
          if (index1 == -1)
          {
            MyGuiControlListbox smListBox = this.m_smListBox;
            StringBuilder text = new StringBuilder(stateMachine.Name);
            object obj1 = (object) stateMachine;
            string toolTip = MyTexts.Get(MyCommonTexts.Scripting_Tooltip_Cursors).ToString();
            object userData = obj1;
            MyGuiControlListbox.Item obj2 = new MyGuiControlListbox.Item(text, toolTip, userData: userData);
            int? position = new int?();
            smListBox.Add(obj2, position);
            index1 = this.m_smListBox.Items.Count - 1;
          }
          MyGuiControlListbox.Item obj = this.m_smListBox.Items[index1];
          for (int index2 = obj.ToolTip.ToolTips.Count - 1; index2 >= 0; --index2)
          {
            MyColoredText toolTip = obj.ToolTip.ToolTips[index2];
            bool flag = false;
            foreach (MyStateMachineCursor activeCursor in stateMachine.ActiveCursors)
            {
              if (toolTip.Text.CompareTo(activeCursor.Node.Name) == 0)
              {
                flag = true;
                break;
              }
            }
            if (!flag && index2 != 0)
              obj.ToolTip.ToolTips.RemoveAtFast<MyColoredText>(index2);
          }
          if (stateMachine.ActiveCursors != null)
          {
            foreach (MyStateMachineCursor activeCursor in stateMachine.ActiveCursors)
            {
              bool flag = false;
              for (int index2 = obj.ToolTip.ToolTips.Count - 1; index2 >= 0; --index2)
              {
                if (obj.ToolTip.ToolTips[index2].Text.CompareTo(activeCursor.Node.Name) == 0)
                {
                  flag = true;
                  break;
                }
              }
              if (!flag)
                obj.ToolTip.AddToolTip(activeCursor.Node.Name);
            }
          }
        }
      }
      if (true)
        MyVisualScriptingDebugInputComponent.DrawVariables();
      return base.Update(hasFocus);
    }

    public void UpdateTriggerList()
    {
      List<MyTriggerComponent> allTriggers = MySessionComponentTriggerSystem.Static.GetAllTriggers();
      this.m_triggersListBox.Items.Clear();
      foreach (MyTriggerComponent trigger in allTriggers)
      {
        MyGuiControlListbox.Item triggerListItem = this.CreateTriggerListItem(trigger);
        if (triggerListItem != null)
          this.m_triggersListBox.Add(triggerListItem);
      }
    }

    private MyGuiControlListbox.Item CreateTriggerListItem(
      MyTriggerComponent trigger)
    {
      if (!(trigger is MyAreaTriggerComponent triggerComponent))
        return (MyGuiControlListbox.Item) null;
      StringBuilder text = new StringBuilder("Trigger: ");
      text.Append(triggerComponent.Name).Append(" Entity: ");
      text.Append(string.IsNullOrEmpty(triggerComponent.Entity.Name) ? ((MyEntity) triggerComponent.Entity).DisplayNameText : triggerComponent.Entity.Name);
      return new MyGuiControlListbox.Item(text, "Double click to rename trigger", userData: ((object) triggerComponent));
    }

    private void InitializeWaypointList()
    {
      this.m_waypoints.Clear();
      foreach (MyEntity entity in Sandbox.Game.Entities.MyEntities.GetEntities())
      {
        if (entity is MyWaypoint)
          this.m_waypoints.Add(entity as MyWaypoint);
      }
    }

    public void UpdateWaypointList()
    {
      if (this.m_waypointsListBox == null)
        return;
      ObservableCollection<MyGuiControlListbox.Item> items = this.m_waypointsListBox.Items;
      for (int index = 0; index < items.Count; ++index)
      {
        if (!this.m_waypoints.Contains((MyWaypoint) items[index].UserData))
          items.RemoveAtFast<MyGuiControlListbox.Item>(index);
      }
      foreach (MyWaypoint waypoint1 in this.m_waypoints)
      {
        MyWaypoint waypoint = waypoint1;
        int index = this.m_waypointsListBox.Items.FindIndex((Predicate<MyGuiControlListbox.Item>) (item => (MyWaypoint) item.UserData == waypoint));
        if (index < 0)
        {
          StringBuilder text = new StringBuilder("Waypoint: ");
          text.Append(waypoint.Name);
          this.m_waypointsListBox.Add(new MyGuiControlListbox.Item(text, waypoint.Name, userData: ((object) waypoint)));
        }
        else
          this.m_waypointsListBox.Items[index].Text.Clear().Append("Waypoint: " + waypoint.Name);
      }
      List<MyGuiControlListbox.Item> list = this.m_waypointsListBox.Items.OrderBy<MyGuiControlListbox.Item, string>((Func<MyGuiControlListbox.Item, string>) (x => x.Text.ToString())).ToList<MyGuiControlListbox.Item>();
      for (int index = 0; index < list.Count; ++index)
        items.Move(items.IndexOf(list[index]), index);
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      float y = (float) (((double) MyGuiScreenScriptingTools.SCREEN_SIZE.Y - 1.0) / 2.0);
      Vector2 vector2 = new Vector2(0.02f, 0.0f);
      MyGuiControlLabel myGuiControlLabel = this.AddCaption(MyTexts.Get(MySpaceTexts.ScriptingToolsTransformations).ToString(), new Vector4?(Color.White.ToVector4()), new Vector2?(vector2 + new Vector2(-MyGuiScreenScriptingTools.HIDDEN_PART_RIGHT, y)));
      this.m_currentPosition.Y = myGuiControlLabel.PositionY + myGuiControlLabel.Size.Y + MyGuiScreenScriptingTools.ITEM_VERTICAL_PADDING;
      this.m_transformSys.Active = true;
      this.m_canShareInput = true;
      MyGuiScreenGamePlay.DisableInput = false;
      this.SelectCoordsWorld(false);
      this.SelectOperation(MyEntityTransformationSystem.OperationMode.Translation);
      this.RecreateControlsTransformation();
    }

    private void SelectOperation(MyEntityTransformationSystem.OperationMode mode) => this.m_transformSys.ChangeOperationMode(mode);

    private void SelectCoordsWorld(bool world) => this.m_transformSys.ChangeCoordSystem(world);

    private void DeselectEntityOnClicked(MyGuiControlButton myGuiControlButton) => this.m_transformSys.SetControlledEntity((MyEntity) null);

    private void RecreateControlsTransformation()
    {
      MyGuiControlCombobox transformCombo = this.CreateComboBox();
      transformCombo.AddItem(0L, MyTexts.GetString(MyCommonTexts.ScriptingTools_Translation));
      transformCombo.AddItem(1L, MyTexts.GetString(MyCommonTexts.ScriptingTools_Rotation));
      transformCombo.SelectItemByIndex(0);
      transformCombo.ItemSelected += (MyGuiControlCombobox.ItemSelectedDelegate) (() => this.SelectOperation((MyEntityTransformationSystem.OperationMode) transformCombo.GetSelectedKey()));
      MyGuiControlCombobox comboBox = this.CreateComboBox();
      comboBox.AddItem(0L, MyTexts.GetString(MyCommonTexts.ScriptingTools_Coords_Local));
      comboBox.AddItem(1L, MyTexts.GetString(MyCommonTexts.ScriptingTools_Coords_World));
      comboBox.SelectItemByIndex(0);
      comboBox.ItemSelected += (MyGuiControlCombobox.ItemSelectedDelegate) (() => this.SelectCoordsWorld((int) transformCombo.GetSelectedKey() != 0));
      this.PositionControls(new MyGuiControlBase[2]
      {
        (MyGuiControlBase) comboBox,
        (MyGuiControlBase) transformCombo
      });
      this.m_selectedEntityNameBox = this.CreateTextbox("");
      this.PositionControls(new MyGuiControlBase[3]
      {
        (MyGuiControlBase) this.CreateLabel(MyTexts.GetString(MySpaceTexts.SelectedEntity) + ": "),
        (MyGuiControlBase) this.m_selectedEntityNameBox,
        (MyGuiControlBase) this.CreateButton(MyTexts.GetString(MySpaceTexts.ProgrammableBlock_ButtonRename), new Action<MyGuiControlButton>(this.RenameSelectedEntityOnClick), MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_Rename1))
      });
      this.m_selectedFunctionalBlockNameBox = this.CreateTextbox("");
      this.PositionControls(new MyGuiControlBase[3]
      {
        (MyGuiControlBase) this.CreateLabel(MyTexts.GetString(MySpaceTexts.SelectedBlock) + ": "),
        (MyGuiControlBase) this.m_selectedFunctionalBlockNameBox,
        (MyGuiControlBase) this.CreateButton(MyTexts.GetString(MySpaceTexts.ProgrammableBlock_ButtonRename), new Action<MyGuiControlButton>(this.RenameFunctionalBlockOnClick), MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_Rename2))
      });
      this.PositionControls(new MyGuiControlBase[3]
      {
        (MyGuiControlBase) this.CreateButton(MyTexts.GetString(MySpaceTexts.SpawnEntity), new Action<MyGuiControlButton>(this.SpawnWaypointClicked), MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_SpawnEnt)),
        (MyGuiControlBase) this.CreateButton(MyTexts.GetString(MyCommonTexts.Snap), new Action<MyGuiControlButton>(this.SnapEntityToCameraClick), "Snap entity to camera"),
        (MyGuiControlBase) this.CreateButton(MyTexts.GetString(MyCommonTexts.ScriptingTools_SetPosition), new Action<MyGuiControlButton>(this.SetPositionOnClicked), MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_SetPosition))
      });
      this.PositionControls(new MyGuiControlBase[3]
      {
        (MyGuiControlBase) this.CreateButton(MyTexts.GetString(MyCommonTexts.Select), new Action<MyGuiControlButton>(this.SelectEntityOnClick), "Select entity you are standing in"),
        (MyGuiControlBase) this.CreateButton(MyTexts.GetString(MyCommonTexts.ScriptingTools_DeselectEntity), new Action<MyGuiControlButton>(this.DeselectEntityOnClicked), MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_DeselectEnt)),
        (MyGuiControlBase) this.CreateButton(MyTexts.GetString(MySpaceTexts.DeleteEntity), new Action<MyGuiControlButton>(this.DeleteEntityOnClicked), MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_DeleteEnt))
      });
      this.m_waypointsListBox = this.CreateListBox();
      this.m_waypointsListBox.Size = new Vector2(0.0f, 0.148f);
      this.m_waypointsListBox.ItemClicked += new Action<MyGuiControlListbox>(this.WaypointsListBoxOnItemDoubleClicked);
      this.PositionControl((MyGuiControlBase) this.m_waypointsListBox);
      this.PositionControl((MyGuiControlBase) this.CreateLabel(MyTexts.GetString(MySpaceTexts.Triggers)));
      this.PositionControls(new MyGuiControlBase[2]
      {
        (MyGuiControlBase) this.CreateButton("Attach spherical trigger", new Action<MyGuiControlButton>(this.AttachSphericalTriggerOnClick)),
        (MyGuiControlBase) this.CreateButton("Attach box trigger", new Action<MyGuiControlButton>(this.AttachBoxTriggerOnClick))
      });
      this.PositionControls(new MyGuiControlBase[2]
      {
        (MyGuiControlBase) this.CreateButton(MyTexts.GetString(MyCommonTexts.Snap), new Action<MyGuiControlButton>(this.SnapTriggerToCameraOrEntityOnClick), MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_TriggerSnap)),
        (MyGuiControlBase) this.CreateButton(MyTexts.GetString(MyCommonTexts.ScriptingTools_SetPosition), new Action<MyGuiControlButton>(this.SetPositionOnClicked), MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_SetPosition))
      });
      this.m_growTriggerButton = this.CreateButton(MyTexts.GetString(MyCommonTexts.ScriptingTools_Grow), new Action<MyGuiControlButton>(this.GrowTriggerOnClick), MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_SizeGrow));
      this.m_shrinkTriggerButton = this.CreateButton(MyTexts.GetString(MyCommonTexts.ScriptingTools_Shrink), new Action<MyGuiControlButton>(this.ShrinkTriggerOnClick), MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_SizeShrink));
      this.m_setTriggerSizeButton = this.CreateButton(MyTexts.GetString(MyCommonTexts.Size), new Action<MyGuiControlButton>(this.SetSizeOnClick), MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_SizeSet));
      MyGuiControlCombobox axisCombo = this.CreateComboBox();
      axisCombo.AddItem(-1L, "All");
      axisCombo.AddItem(0L, "X");
      axisCombo.AddItem(1L, "Y");
      axisCombo.AddItem(2L, "Z");
      axisCombo.SelectItemByIndex(0);
      axisCombo.ItemSelected += (MyGuiControlCombobox.ItemSelectedDelegate) (() => this.m_selectedAxis = (int) axisCombo.GetSelectedKey());
      this.PositionControls(new MyGuiControlBase[4]
      {
        (MyGuiControlBase) axisCombo,
        (MyGuiControlBase) this.m_growTriggerButton,
        (MyGuiControlBase) this.m_setTriggerSizeButton,
        (MyGuiControlBase) this.m_shrinkTriggerButton
      });
      this.PositionControls(new MyGuiControlBase[3]
      {
        (MyGuiControlBase) this.CreateButton(MyTexts.GetString(MyCommonTexts.Select), new Action<MyGuiControlButton>(this.SelectTriggerOnClick), MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_TriggerSelect)),
        (MyGuiControlBase) this.CreateButton(MyTexts.GetString(MyCommonTexts.ScriptingTools_DeselectEntity), new Action<MyGuiControlButton>(this.DeselectEntityOnClicked), MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_DeselectEnt)),
        (MyGuiControlBase) this.CreateButton(MyTexts.GetString(MyCommonTexts.Delete), new Action<MyGuiControlButton>(this.DeleteTriggerOnClick), MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_TriggerDelete))
      });
      this.m_selectedTriggerNameBox = this.CreateTextbox(MyTexts.GetString(MySpaceTexts.TriggerNotSelected));
      this.PositionControls(new MyGuiControlBase[2]
      {
        (MyGuiControlBase) this.CreateLabel(MyTexts.GetString(MySpaceTexts.SelectedTrigger) + ":"),
        (MyGuiControlBase) this.m_selectedTriggerNameBox
      });
      this.m_triggersListBox = this.CreateListBox();
      this.m_triggersListBox.Size = new Vector2(0.0f, 0.14f);
      this.m_triggersListBox.ItemClicked += new Action<MyGuiControlListbox>(this.TriggersListBoxOnItemClicked);
      this.m_triggersListBox.ItemDoubleClicked += new Action<MyGuiControlListbox>(this.TriggersListBox_ItemDoubleClicked);
      this.PositionControl((MyGuiControlBase) this.m_triggersListBox);
      this.PositionControl((MyGuiControlBase) this.CreateLabel(MyTexts.Get(MySpaceTexts.RunningLevelScripts).ToString()));
      this.m_levelScriptListBox = this.CreateListBox();
      this.m_levelScriptListBox.Size = new Vector2(0.0f, 0.07f);
      this.PositionControl((MyGuiControlBase) this.m_levelScriptListBox);
      MyVisualScriptManagerSessionComponent component = MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>();
      if (component != null && component.RunningLevelScriptNames != null)
      {
        foreach (string runningLevelScriptName in component.RunningLevelScriptNames)
          this.m_levelScriptListBox.Add(new MyGuiControlListbox.Item(new StringBuilder(runningLevelScriptName), userData: ((object) false)));
      }
      this.PositionControl((MyGuiControlBase) this.CreateLabel(MyTexts.Get(MySpaceTexts.RunningStateMachines).ToString()));
      this.m_smListBox = this.CreateListBox();
      this.m_smListBox.Size = new Vector2(0.0f, 0.07f);
      this.PositionControl((MyGuiControlBase) this.m_smListBox);
      this.m_smListBox.ItemSize = new Vector2(MyGuiScreenScriptingTools.SCREEN_SIZE.X, MyGuiScreenScriptingTools.ITEM_SIZE.Y);
    }

    private void SwitchPageToTransformation(MyGuiControlButton myGuiControlButton)
    {
    }

    private void TransformSysOnRayCasted(LineD ray)
    {
      if (this.m_transformSys.ControlledEntity == null || this.m_disablePicking)
        return;
      MyHighlightData myHighlightData;
      if (this.m_selectedFunctionalBlock != null)
      {
        MyHighlightSystem component = MySession.Static.GetComponent<MyHighlightSystem>();
        if (component != null)
        {
          MyHighlightSystem myHighlightSystem = component;
          myHighlightData = new MyHighlightData();
          myHighlightData.EntityId = this.m_selectedFunctionalBlock.EntityId;
          myHighlightData.PlayerId = -1L;
          myHighlightData.Thickness = -1;
          MyHighlightData data = myHighlightData;
          myHighlightSystem.RequestHighlightChange(data);
        }
        this.m_selectedFunctionalBlock = (MyEntity) null;
      }
      if (this.m_transformSys.ControlledEntity is MyCubeGrid controlledEntity)
      {
        Vector3I? nullable = controlledEntity.RayCastBlocks(ray.From, ray.To);
        if (nullable.HasValue)
        {
          MySlimBlock cubeBlock = controlledEntity.GetCubeBlock(nullable.Value);
          if (cubeBlock.FatBlock != null)
            this.m_selectedFunctionalBlock = (MyEntity) cubeBlock.FatBlock;
        }
      }
      StringBuilder source = new StringBuilder();
      if (this.m_selectedFunctionalBlock != null)
      {
        source.Append(string.IsNullOrEmpty(this.m_selectedFunctionalBlock.Name) ? this.m_selectedFunctionalBlock.DisplayNameText : this.m_selectedFunctionalBlock.Name);
        MyHighlightSystem component = MySession.Static.GetComponent<MyHighlightSystem>();
        if (component != null)
        {
          MyHighlightSystem myHighlightSystem = component;
          myHighlightData = new MyHighlightData();
          myHighlightData.EntityId = this.m_selectedFunctionalBlock.EntityId;
          myHighlightData.IgnoreUseObjectData = true;
          myHighlightData.OutlineColor = new Color?(Color.Blue);
          myHighlightData.PulseTimeInFrames = 120UL;
          myHighlightData.Thickness = 3;
          myHighlightData.PlayerId = -1L;
          MyHighlightData data = myHighlightData;
          myHighlightSystem.RequestHighlightChange(data);
        }
      }
      if (this.m_selectedFunctionalBlockNameBox == null)
        return;
      this.m_selectedFunctionalBlockNameBox.SetText(source);
    }

    private void RenameFunctionalBlockOnClick(MyGuiControlButton myGuiControlButton)
    {
      if (this.m_selectedFunctionalBlock == null)
        return;
      this.m_disablePicking = true;
      this.m_transformSys.DisablePicking = true;
      ValueGetScreenWithCaption screenWithCaption = new ValueGetScreenWithCaption(MyTexts.Get(MySpaceTexts.EntityRename).ToString() + ": " + this.m_selectedFunctionalBlock.DisplayNameText, "", (ValueGetScreenWithCaption.ValueGetScreenAction) (text =>
      {
        if (Sandbox.Game.Entities.MyEntities.TryGetEntityByName(text, out MyEntity _))
          return false;
        this.m_selectedFunctionalBlock.Name = text;
        Sandbox.Game.Entities.MyEntities.SetEntityName(this.m_selectedFunctionalBlock);
        this.m_selectedFunctionalBlockNameBox.SetText(new StringBuilder(text));
        return true;
      }));
      screenWithCaption.Closed += (MyGuiScreenBase.ScreenHandler) ((source, isUnloading) =>
      {
        this.m_disablePicking = false;
        this.m_transformSys.DisablePicking = false;
      });
      MyGuiSandbox.AddScreen((MyGuiScreenBase) screenWithCaption);
    }

    private void RenameSelectedEntityOnClick(MyGuiControlButton myGuiControlButton)
    {
      if (this.m_transformSys.ControlledEntity == null)
        return;
      this.m_disablePicking = true;
      this.m_transformSys.DisablePicking = true;
      MyEntity selectedEntity = this.m_transformSys.ControlledEntity;
      ValueGetScreenWithCaption screenWithCaption = new ValueGetScreenWithCaption(MyTexts.Get(MySpaceTexts.EntityRename).ToString() + ": " + this.m_transformSys.ControlledEntity.DisplayNameText, string.IsNullOrEmpty(this.m_transformSys.ControlledEntity.Name) ? "" : this.m_transformSys.ControlledEntity.Name, (ValueGetScreenWithCaption.ValueGetScreenAction) (text =>
      {
        if (Sandbox.Game.Entities.MyEntities.TryGetEntityByName(text, out MyEntity _))
          return false;
        selectedEntity.Name = text;
        Sandbox.Game.Entities.MyEntities.SetEntityName(selectedEntity);
        this.m_selectedEntityNameBox.SetText(new StringBuilder(text));
        this.InitializeWaypointList();
        this.UpdateWaypointList();
        this.UpdateTriggerList();
        VSWaypointRenamedMsg msg = new VSWaypointRenamedMsg()
        {
          Id = selectedEntity.EntityId,
          Name = text
        };
        MySessionComponentExtDebug.Static.SendMessageToClients<VSWaypointRenamedMsg>(msg);
        return true;
      }));
      screenWithCaption.Closed += (MyGuiScreenBase.ScreenHandler) ((source, isUnloading) =>
      {
        this.m_disablePicking = false;
        this.m_transformSys.DisablePicking = false;
      });
      MyGuiSandbox.AddScreen((MyGuiScreenBase) screenWithCaption);
    }

    private void DeleteEntityOnClicked(MyGuiControlButton myGuiControlButton)
    {
      if (this.m_transformSys.ControlledEntity == null)
        return;
      MyWaypoint wp = this.m_transformSys.ControlledEntity as MyWaypoint;
      if (wp != null && this.m_waypoints.Contains(wp))
      {
        this.m_waypoints.Remove(wp);
        this.UpdateWaypointList();
        VSWaypointDeletedMsg msg = new VSWaypointDeletedMsg()
        {
          Id = wp.EntityId
        };
        MySessionComponentExtDebug.Static.SendMessageToClients<VSWaypointDeletedMsg>(msg);
        MySessionComponentCutscenes component = MySession.Static.GetComponent<MySessionComponentCutscenes>();
        if (component != null)
        {
          foreach (KeyValuePair<string, Cutscene> cutscene in component.GetCutscenes())
          {
            foreach (CutsceneSequenceNode sequenceNode in cutscene.Value.SequenceNodes)
            {
              CutsceneSequenceNodeWaypoint sequenceNodeWaypoint = sequenceNode.Waypoints.FirstOrDefault<CutsceneSequenceNodeWaypoint>((Func<CutsceneSequenceNodeWaypoint, bool>) (x => x.Name == wp.Name));
              if (sequenceNodeWaypoint != null)
                sequenceNode.Waypoints.Remove(sequenceNodeWaypoint);
            }
          }
        }
      }
      this.m_transformSys.ControlledEntity.Close();
      this.m_transformSys.SetControlledEntity((MyEntity) null);
      Sandbox.Game.Entities.MyEntities.DeleteRememberedEntities();
      this.UpdateTriggerList();
    }

    private void AttachSphericalTriggerOnClick(MyGuiControlButton myGuiControlButton) => this.AttachTriggerOnClick(MyTriggerComponent.TriggerType.Sphere);

    private void AttachBoxTriggerOnClick(MyGuiControlButton myGuiControlButton) => this.AttachTriggerOnClick(MyTriggerComponent.TriggerType.OBB);

    private void AttachTriggerOnClick(MyTriggerComponent.TriggerType triggerType)
    {
      if (this.m_transformSys.ControlledEntity == null)
        return;
      MyEntity selectedEntity = this.m_selectedFunctionalBlock != null ? this.m_selectedFunctionalBlock : this.m_transformSys.ControlledEntity;
      MyGuiSandbox.AddScreen((MyGuiScreenBase) new ValueGetScreenWithCaption("Set name of trigger to be attached to " + (string.IsNullOrEmpty(selectedEntity.Name) ? selectedEntity.DisplayNameText : selectedEntity.Name), "", (ValueGetScreenWithCaption.ValueGetScreenAction) (text =>
      {
        MyAreaTriggerComponent triggerComponent1 = new MyAreaTriggerComponent(text);
        triggerComponent1.TriggerAreaType = triggerType;
        this.m_triggerManipulator.SelectedTrigger = (MyTriggerComponent) triggerComponent1;
        if (!selectedEntity.Components.Contains(typeof (MyTriggerAggregate)))
          selectedEntity.Components.Add(typeof (MyTriggerAggregate), (MyComponentBase) new MyTriggerAggregate());
        selectedEntity.Components.Get<MyTriggerAggregate>().AddComponent((MyComponentBase) this.m_triggerManipulator.SelectedTrigger);
        triggerComponent1.Radius = 2.0;
        MyAreaTriggerComponent triggerComponent2 = triggerComponent1;
        Matrix matrix = Matrix.Normalize((Matrix) ref selectedEntity.PositionComp.WorldMatrixRef);
        MyOrientedBoundingBoxD orientedBoundingBoxD = new MyOrientedBoundingBoxD((MatrixD) ref matrix);
        triggerComponent2.OBB = orientedBoundingBoxD;
        triggerComponent1.Center = MyAPIGateway.Session.Camera.Position;
        triggerComponent1.CustomDebugColor = new Color?(Color.Yellow);
        this.DeselectEntity();
        this.UpdateTriggerList();
        this.m_triggersListBox.SelectedItems.Clear();
        this.m_triggersListBox.SelectByUserData((object) triggerComponent1);
        return true;
      })));
    }

    private void RenameTriggerOnClick(MyAreaTriggerComponent trigger) => MyGuiSandbox.AddScreen((MyGuiScreenBase) new ValueGetScreenWithCaption(MyTexts.Get(MySpaceTexts.EntityRename).ToString() + ": " + trigger.Name, trigger.Name, (ValueGetScreenWithCaption.ValueGetScreenAction) (text =>
    {
      this.m_triggerManipulator.SelectedTrigger = (MyTriggerComponent) trigger;
      trigger.Name = text;
      this.UpdateTriggerList();
      this.m_selectedTriggerNameBox.SetText(new StringBuilder(text));
      return true;
    })));

    private void DeselectEntity()
    {
      this.m_transformSys.SetControlledEntity((MyEntity) null);
      this.m_waypointsListBox.SelectedItems.Clear();
    }

    private void DeselectTrigger()
    {
      this.m_triggerManipulator.SelectedTrigger = (MyTriggerComponent) null;
      if (this.m_selectedTriggerNameBox != null)
        this.m_selectedTriggerNameBox.SetText(new StringBuilder());
      if (this.m_triggersListBox == null)
        return;
      this.m_triggersListBox.SelectedItems.Clear();
    }

    private void DeleteTriggerOnClick(MyGuiControlButton myGuiControlButton)
    {
      if (this.m_triggerManipulator.SelectedTrigger == null)
        return;
      if (this.m_triggerManipulator.SelectedTrigger.Entity != null)
        this.m_triggerManipulator.SelectedTrigger.Entity.Components.Remove(typeof (MyTriggerAggregate), (MyComponentBase) this.m_triggerManipulator.SelectedTrigger);
      this.m_triggerManipulator.SelectedTrigger = (MyTriggerComponent) null;
      this.m_selectedEntityNameBox.SetText(new StringBuilder());
      this.UpdateTriggerList();
    }

    private void SnapTriggerToCameraOrEntityOnClick(MyGuiControlButton myGuiControlButton)
    {
      if (this.m_triggerManipulator.SelectedTrigger == null)
        return;
      MyAreaTriggerComponent selectedTrigger = (MyAreaTriggerComponent) this.m_triggerManipulator.SelectedTrigger;
      if (this.m_transformSys.ControlledEntity != null)
        selectedTrigger.Center = this.m_transformSys.ControlledEntity.PositionComp.GetPosition();
      else
        selectedTrigger.Center = MyAPIGateway.Session.Camera.Position;
    }

    private void SnapEntityToCameraClick(MyGuiControlButton myGuiControlButton)
    {
      if (this.m_transformSys.ControlledEntity == null)
        return;
      MatrixD worldMatrix = MyAPIGateway.Session.Camera.WorldMatrix;
      this.m_transformSys.ControlledEntity.PositionComp.SetWorldMatrix(ref worldMatrix);
    }

    private void TransformSysOnControlledEntityChanged(MyEntity oldEntity, MyEntity newEntity)
    {
      if (this.m_disablePicking)
        return;
      StringBuilder source = new StringBuilder();
      if (newEntity != null)
      {
        source.Clear().Append(string.IsNullOrEmpty(newEntity.Name) ? newEntity.DisplayName : newEntity.Name);
        this.DeselectTrigger();
        if (newEntity is MyWaypoint && !this.m_waypoints.Contains(newEntity as MyWaypoint))
          this.m_waypointsListBox.SelectedItems.Clear();
      }
      if (this.m_selectedEntityNameBox != null)
        this.m_selectedEntityNameBox.SetText(source);
      this.TransformSysOnRayCasted(this.m_transformSys.LastRay);
    }

    private void TriggersListBoxOnItemClicked(MyGuiControlListbox listBox)
    {
      if (this.m_triggersListBox.SelectedItems.Count == 0)
        return;
      this.m_triggerManipulator.SelectedTrigger = (MyTriggerComponent) this.m_triggersListBox.SelectedItems[0].UserData;
      if (this.m_triggerManipulator.SelectedTrigger != null)
        this.m_selectedTriggerNameBox.SetText(new StringBuilder(((MyAreaTriggerComponent) this.m_triggerManipulator.SelectedTrigger).Name));
      this.DeselectEntity();
    }

    private void TriggersListBox_ItemDoubleClicked(MyGuiControlListbox obj)
    {
      if (this.m_triggersListBox.SelectedItems.Count == 0)
        return;
      this.m_triggerManipulator.SelectedTrigger = (MyTriggerComponent) this.m_triggersListBox.SelectedItems[0].UserData;
      if (this.m_triggerManipulator.SelectedTrigger != null)
        this.RenameTriggerOnClick((MyAreaTriggerComponent) this.m_triggerManipulator.SelectedTrigger);
      this.DeselectEntity();
    }

    private void WaypointsListBoxOnItemDoubleClicked(MyGuiControlListbox listBox)
    {
      if (this.m_waypointsListBox.SelectedItems.Count == 0)
        return;
      this.m_transformSys.SetControlledEntity((MyEntity) this.m_waypointsListBox.SelectedItems[0].UserData);
      this.DeselectTrigger();
    }

    private void SetSizeOnClick(MyGuiControlButton button)
    {
      if (this.m_triggerManipulator.SelectedTrigger == null)
        return;
      MyAreaTriggerComponent areaTrigger = (MyAreaTriggerComponent) this.m_triggerManipulator.SelectedTrigger;
      MyGuiSandbox.AddScreen((MyGuiScreenBase) new ValueGetScreenWithCaption(MyTexts.Get(MySpaceTexts.SetTriggerSizeDialog).ToString(), areaTrigger.Radius.ToString((IFormatProvider) CultureInfo.InvariantCulture), (ValueGetScreenWithCaption.ValueGetScreenAction) (text =>
      {
        float result;
        if (!float.TryParse(text, out result))
          return false;
        if (this.m_selectedAxis == -1 || areaTrigger.TriggerAreaType == MyTriggerComponent.TriggerType.Sphere)
        {
          areaTrigger.Radius = (double) result;
        }
        else
        {
          switch (this.m_selectedAxis)
          {
            case 0:
              areaTrigger.SizeX = (double) result;
              break;
            case 1:
              areaTrigger.SizeY = (double) result;
              break;
            case 2:
              areaTrigger.SizeZ = (double) result;
              break;
          }
        }
        return true;
      })));
    }

    private void SetPositionOnClicked(MyGuiControlButton button)
    {
      if (this.m_transformSys.ControlledEntity == null)
        return;
      MyEntity entity = this.m_transformSys.ControlledEntity;
      Vector3D position = entity.PositionComp.GetPosition();
      MyGuiSandbox.AddScreen((MyGuiScreenBase) new Vector3GetScreenWithCaption(MyTexts.GetString(MySpaceTexts.SetEntityPositionDialog), position.X.ToString(), position.Y.ToString(), position.Z.ToString(), (Vector3GetScreenWithCaption.Vector3GetScreenAction) ((text1, text2, text3) =>
      {
        double result1;
        double result2;
        double result3;
        if (!double.TryParse(text1, out result1) || !double.TryParse(text2, out result2) || !double.TryParse(text3, out result3))
          return false;
        MatrixD worldMatrix = entity.WorldMatrix;
        worldMatrix.Translation = new Vector3D(result1, result2, result3);
        entity.WorldMatrix = worldMatrix;
        return true;
      })));
    }

    private void ShrinkTriggerOnClick(MyGuiControlButton button)
    {
      if (this.m_triggerManipulator.SelectedTrigger == null)
        return;
      MyAreaTriggerComponent selectedTrigger = (MyAreaTriggerComponent) this.m_triggerManipulator.SelectedTrigger;
      if (selectedTrigger.Radius <= 0.200000002980232)
        return;
      if (this.m_selectedAxis == -1 || selectedTrigger.TriggerAreaType == MyTriggerComponent.TriggerType.Sphere)
      {
        selectedTrigger.Radius -= 0.200000002980232;
      }
      else
      {
        switch (this.m_selectedAxis)
        {
          case 0:
            selectedTrigger.SizeX -= 0.200000002980232;
            break;
          case 1:
            selectedTrigger.SizeY -= 0.200000002980232;
            break;
          case 2:
            selectedTrigger.SizeZ -= 0.200000002980232;
            break;
        }
      }
    }

    private void GrowTriggerOnClick(MyGuiControlButton button)
    {
      if (this.m_triggerManipulator.SelectedTrigger == null)
        return;
      MyAreaTriggerComponent selectedTrigger = (MyAreaTriggerComponent) this.m_triggerManipulator.SelectedTrigger;
      if (this.m_selectedAxis == -1 || selectedTrigger.TriggerAreaType == MyTriggerComponent.TriggerType.Sphere)
      {
        selectedTrigger.Radius += 0.200000002980232;
      }
      else
      {
        switch (this.m_selectedAxis)
        {
          case 0:
            selectedTrigger.SizeX += 0.200000002980232;
            break;
          case 1:
            selectedTrigger.SizeY += 0.200000002980232;
            break;
          case 2:
            selectedTrigger.SizeZ += 0.200000002980232;
            break;
        }
      }
    }

    private void SelectTriggerOnClick(MyGuiControlButton button)
    {
      this.m_triggerManipulator.SelectClosest(MyAPIGateway.Session.Camera.Position);
      if (this.m_triggerManipulator.SelectedTrigger == null)
        return;
      this.m_selectedTriggerNameBox.SetText(new StringBuilder(((MyAreaTriggerComponent) this.m_triggerManipulator.SelectedTrigger).Name));
    }

    private void SelectEntityOnClick(MyGuiControlButton button)
    {
      BoundingSphereD sphere = new BoundingSphereD(MyAPIGateway.Session.Camera.Position, 1.0);
      List<MyEntity> result = new List<MyEntity>();
      MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref sphere, result);
      if (result.Count > 0)
      {
        this.m_transformSys.SetControlledEntity(result[0]);
      }
      else
      {
        foreach (MyWaypoint waypoint in this.m_waypoints)
        {
          if (sphere.Contains(waypoint.PositionComp.GetPosition()) == ContainmentType.Contains)
          {
            this.m_transformSys.SetControlledEntity((MyEntity) waypoint);
            break;
          }
        }
      }
    }

    private void SpawnWaypointClicked(MyGuiControlButton myGuiControlButton) => MyVisualScriptManagerSessionComponent.CreateWaypoint();

    internal void OnWaypointAdded(MyWaypoint waypoint)
    {
      this.m_transformSys.SetControlledEntity((MyEntity) waypoint);
      this.m_waypoints.Add(waypoint);
      this.UpdateWaypointList();
      this.m_waypointsListBox.SelectedItems.Clear();
      this.m_waypointsListBox.SelectByUserData((object) waypoint);
    }

    private void DisableTransformationOnCheckedChanged(MyGuiControlCheckbox checkbox) => this.m_transformSys.DisableTransformation = checkbox.IsChecked;

    private MyGuiControlCheckbox CreateCheckbox(
      Action<MyGuiControlCheckbox> onCheckedChanged,
      bool isChecked,
      string tooltip = null)
    {
      MyGuiControlCheckbox control = new MyGuiControlCheckbox(isChecked: isChecked, visualStyle: MyGuiControlCheckboxStyleEnum.Debug, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      if (!string.IsNullOrEmpty(tooltip))
        control.SetTooltip(tooltip);
      control.Size = MyGuiScreenScriptingTools.ITEM_SIZE;
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
      guiControlTextbox.Size = MyGuiScreenScriptingTools.ITEM_SIZE;
      guiControlTextbox.TextChanged += textChanged;
      this.Controls.Add((MyGuiControlBase) guiControlTextbox);
      return guiControlTextbox;
    }

    private MyGuiControlLabel CreateLabel(string text)
    {
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(size: new Vector2?(MyGuiScreenScriptingTools.ITEM_SIZE), text: text, font: "Debug", originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
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
      guiControlListbox2.ItemSize = new Vector2(MyGuiScreenScriptingTools.SCREEN_SIZE.X, MyGuiScreenScriptingTools.ITEM_SIZE.Y);
      guiControlListbox2.TextScale = 0.6f;
      guiControlListbox2.VisibleRowsCount = 7;
      this.Controls.Add((MyGuiControlBase) guiControlListbox2);
      return guiControlListbox2;
    }

    private MyGuiControlCombobox CreateComboBox()
    {
      MyGuiControlCombobox guiControlCombobox1 = new MyGuiControlCombobox();
      guiControlCombobox1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      guiControlCombobox1.Size = MyGuiScreenScriptingTools.BUTTON_SIZE;
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
      control.Size = MyGuiScreenScriptingTools.BUTTON_SIZE;
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

    private void PositionControl(MyGuiControlBase control)
    {
      float x = (float) ((double) MyGuiScreenScriptingTools.SCREEN_SIZE.X - (double) MyGuiScreenScriptingTools.HIDDEN_PART_RIGHT - (double) MyGuiScreenScriptingTools.ITEM_HORIZONTAL_PADDING * 2.0);
      Vector2 size = control.Size;
      control.Position = new Vector2(this.m_currentPosition.X - MyGuiScreenScriptingTools.SCREEN_SIZE.X / 2f + MyGuiScreenScriptingTools.ITEM_HORIZONTAL_PADDING, this.m_currentPosition.Y + MyGuiScreenScriptingTools.ITEM_VERTICAL_PADDING);
      control.Size = new Vector2(x, size.Y);
      this.m_currentPosition.Y += control.Size.Y + MyGuiScreenScriptingTools.ITEM_VERTICAL_PADDING;
    }

    private void PositionControls(MyGuiControlBase[] controls)
    {
      float x = (float) (((double) MyGuiScreenScriptingTools.SCREEN_SIZE.X - (double) MyGuiScreenScriptingTools.HIDDEN_PART_RIGHT - (double) MyGuiScreenScriptingTools.ITEM_HORIZONTAL_PADDING * 2.0) / (double) controls.Length - 1.0 / 1000.0 * (double) controls.Length);
      float num1 = x + 1f / 1000f * (float) controls.Length;
      float num2 = 0.0f;
      for (int index = 0; index < controls.Length; ++index)
      {
        MyGuiControlBase control = controls[index];
        control.Size = control is MyGuiControlCheckbox ? new Vector2(MyGuiScreenScriptingTools.BUTTON_SIZE.Y) : new Vector2(x, control.Size.Y);
        control.PositionX = (float) ((double) this.m_currentPosition.X + (double) num1 * (double) index - (double) MyGuiScreenScriptingTools.SCREEN_SIZE.X / 2.0) + MyGuiScreenScriptingTools.ITEM_HORIZONTAL_PADDING;
        control.PositionY = this.m_currentPosition.Y + MyGuiScreenScriptingTools.ITEM_VERTICAL_PADDING;
        if ((double) control.Size.Y > (double) num2)
          num2 = control.Size.Y;
      }
      this.m_currentPosition.Y += num2 + MyGuiScreenScriptingTools.ITEM_VERTICAL_PADDING;
    }

    private enum ScriptingToolsScreen
    {
      Transformation,
      Cutscenes,
    }
  }
}
