// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyRemoteControl
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.AI.Navigation;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.Entities.UseObject;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.AI;
using VRage.Groups;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
  [MyCubeBlockType(typeof (MyObjectBuilder_RemoteControl))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyRemoteControl), typeof (Sandbox.ModAPI.Ingame.IMyRemoteControl)})]
  public class MyRemoteControl : MyShipController, IMyUsableEntity, Sandbox.ModAPI.IMyRemoteControl, Sandbox.ModAPI.IMyShipController, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyTerminalBlock, Sandbox.ModAPI.Ingame.IMyShipController, VRage.Game.ModAPI.Interfaces.IMyControllableEntity, Sandbox.ModAPI.Ingame.IMyRemoteControl, IMyParallelUpdateable
  {
    private static readonly double MAX_STOPPING_DISTANCE = 3000.0;
    private static readonly double PLANET_REPULSION_RADIUS = 2500.0;
    private static readonly double PLANET_AVOIDANCE_RADIUS = 5000.0;
    private static readonly double PLANET_AVOIDANCE_TOLERANCE = 100.0;
    private static MyObjectBuilder_AutopilotClipboard m_clipboard;
    private static MyGuiControlListbox m_gpsGuiControl;
    private static MyGuiControlListbox m_waypointGuiControl;
    private static MyTerminalControlListbox<MyRemoteControl> m_waypointList;
    private static MyTerminalControlListbox<MyRemoteControl> m_gpsList;
    private const float MAX_TERMINAL_DISTANCE_SQUARED = 10f;
    private long? m_savedPreviousControlledEntityId;
    private IMyControllableEntity m_previousControlledEntity;
    private VRage.Sync.Sync<long, SyncDirection.BothWays> m_bindedCamera;
    private static MyTerminalControlCombobox<MyRemoteControl> m_cameraList = (MyTerminalControlCombobox<MyRemoteControl>) null;
    private MyCharacter cockpitPilot;
    private List<MyRemoteControl.MyAutopilotWaypoint> m_waypoints;
    private MyRemoteControl.MyAutopilotWaypoint m_currentWaypoint;
    private MyRemoteControl.PlanetCoordInformation m_destinationInfo;
    private MyRemoteControl.PlanetCoordInformation m_currentInfo;
    private Vector3D m_currentWorldPosition;
    private Vector3D m_previousWorldPosition;
    private bool m_rotateBetweenWaypoints;
    private VRage.Sync.Sync<float, SyncDirection.BothWays> m_autopilotSpeedLimit;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_useCollisionAvoidance;
    private int m_collisionCtr;
    private Vector3D m_oldCollisionDelta = Vector3D.Zero;
    private MyStuckDetection m_stuckDetection;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_autoPilotEnabled;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_dockingModeEnabled;
    private readonly VRage.Sync.Sync<FlightMode, SyncDirection.BothWays> m_currentFlightMode;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_waitForFreeWay;
    private bool m_patrolDirectionForward = true;
    private Vector3D m_startPosition;
    private MyToolbar m_actionToolbar;
    private readonly VRage.Sync.Sync<Base6Directions.Direction, SyncDirection.BothWays> m_currentDirection;
    private Vector3D m_lastDelta = Vector3D.Zero;
    private float m_lastAutopilotSpeedLimit = 2f;
    private int m_collisionAvoidanceFrameSkip;
    private float m_rotateFor;
    private readonly List<MyRemoteControl.DetectedObject> m_detectedObstacles = new List<MyRemoteControl.DetectedObject>();
    private MyRemoteControl.IRemoteControlAutomaticBehaviour m_automaticBehaviour;
    private readonly VRage.Sync.Sync<float, SyncDirection.FromServer> m_waypointThresholdDistance;
    private static readonly Dictionary<Base6Directions.Direction, MyStringId> m_directionNames = new Dictionary<Base6Directions.Direction, MyStringId>()
    {
      {
        Base6Directions.Direction.Forward,
        MyCommonTexts.Thrust_Forward
      },
      {
        Base6Directions.Direction.Backward,
        MyCommonTexts.Thrust_Back
      },
      {
        Base6Directions.Direction.Left,
        MyCommonTexts.Thrust_Left
      },
      {
        Base6Directions.Direction.Right,
        MyCommonTexts.Thrust_Right
      },
      {
        Base6Directions.Direction.Up,
        MyCommonTexts.Thrust_Up
      },
      {
        Base6Directions.Direction.Down,
        MyCommonTexts.Thrust_Down
      }
    };
    private static readonly Dictionary<Base6Directions.Direction, Vector3D> m_upVectors = new Dictionary<Base6Directions.Direction, Vector3D>()
    {
      {
        Base6Directions.Direction.Forward,
        Vector3D.Up
      },
      {
        Base6Directions.Direction.Backward,
        Vector3D.Up
      },
      {
        Base6Directions.Direction.Left,
        Vector3D.Up
      },
      {
        Base6Directions.Direction.Right,
        Vector3D.Up
      },
      {
        Base6Directions.Direction.Up,
        Vector3D.Right
      },
      {
        Base6Directions.Direction.Down,
        Vector3D.Right
      }
    };
    private bool m_syncing;
    private List<IMyGps> m_selectedGpsLocations;
    private List<MyRemoteControl.MyAutopilotWaypoint> m_selectedWaypoints;
    private readonly StringBuilder m_tempName = new StringBuilder();
    private readonly StringBuilder m_tempTooltip = new StringBuilder();
    private readonly StringBuilder m_tempActions = new StringBuilder();
    private List<MyEntity> m_collisionEntityList;
    private List<MySlimBlock> m_collisionBlockList;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_isMainRemoteControl;
    private bool m_releaseRequested;
    private bool m_forceBehaviorUpdate;
    private MyParallelUpdateFlag m_parallelFlag;

    public IMyControllableEntity PreviousControlledEntity
    {
      get
      {
        if (this.m_savedPreviousControlledEntityId.HasValue && this.TryFindSavedEntity())
          this.m_savedPreviousControlledEntityId = new long?();
        return this.m_previousControlledEntity;
      }
      private set
      {
        if (value == this.m_previousControlledEntity)
          return;
        if (this.m_previousControlledEntity != null)
        {
          this.m_previousControlledEntity.Entity.OnMarkForClose -= new Action<MyEntity>(this.Entity_OnPreviousMarkForClose);
          if (this.m_previousControlledEntity.Entity is MyCockpit entity && entity.Pilot != null)
            entity.Pilot.OnMarkForClose -= new Action<MyEntity>(this.Entity_OnPreviousMarkForClose);
        }
        this.m_previousControlledEntity = value;
        if (this.m_previousControlledEntity != null)
          this.AddPreviousControllerEvents();
        this.SetEmissiveStateWorking();
      }
    }

    public override MyCharacter Pilot => this.PreviousControlledEntity is MyCharacter controlledEntity ? controlledEntity : this.cockpitPilot;

    private MyRemoteControlDefinition BlockDefinition => (MyRemoteControlDefinition) base.BlockDefinition;

    public MyRemoteControl.MyAutopilotWaypoint CurrentWaypoint
    {
      get => this.m_currentWaypoint;
      set
      {
        this.m_currentWaypoint = value;
        if (this.m_currentWaypoint == null)
          return;
        this.m_startPosition = this.WorldMatrix.Translation;
      }
    }

    public bool RotateBetweenWaypoints
    {
      get => MyFakes.ENABLE_VR_REMOTE_CONTROL_WAYPOINTS_FAST_MOVEMENT && this.m_rotateBetweenWaypoints;
      set => this.m_rotateBetweenWaypoints = value;
    }

    public double TargettingAimDelta { get; private set; }

    public MyRemoteControl.IRemoteControlAutomaticBehaviour AutomaticBehaviour => this.m_automaticBehaviour;

    public MyRemoteControl()
    {
      this.CreateTerminalControls();
      this.TargettingAimDelta = 0.0;
      this.m_autoPilotEnabled.ValueChanged += (Action<SyncBase>) (x => this.OnSetAutoPilotEnabled());
      this.m_isMainRemoteControl.ValueChanged += (Action<SyncBase>) (x => this.MainRemoteControlChanged());
    }

    private void FillCameraComboBoxContent(ICollection<MyTerminalControlComboBoxItem> items)
    {
      items.Add(new MyTerminalControlComboBoxItem()
      {
        Key = 0L,
        Value = MyCommonTexts.ScreenGraphicsOptions_AntiAliasing_None
      });
      bool flag = false;
      foreach (MyCameraBlock fatBlock in this.CubeGrid.GetFatBlocks<MyCameraBlock>())
      {
        items.Add(new MyTerminalControlComboBoxItem()
        {
          Key = fatBlock.EntityId,
          Value = MyStringId.GetOrCompute(fatBlock.CustomName.ToString())
        });
        if (fatBlock.EntityId == (long) this.m_bindedCamera)
          flag = true;
      }
      MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(this.CubeGrid);
      if (group != null)
      {
        foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node node in group.Nodes)
        {
          if (node.NodeData != this.CubeGrid)
          {
            foreach (MyCameraBlock fatBlock in node.NodeData.GetFatBlocks<MyCameraBlock>())
            {
              items.Add(new MyTerminalControlComboBoxItem()
              {
                Key = fatBlock.EntityId,
                Value = MyStringId.GetOrCompute(fatBlock.CustomName.ToString())
              });
              if (fatBlock.EntityId == (long) this.m_bindedCamera)
                flag = true;
            }
          }
        }
      }
      if (flag)
        return;
      this.m_bindedCamera.Value = 0L;
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyRemoteControl>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlCheckbox<MyRemoteControl> checkbox = new MyTerminalControlCheckbox<MyRemoteControl>("MainRemoteControl", MySpaceTexts.TerminalControlPanel_Cockpit_MainRemoteControl, MySpaceTexts.TerminalControlPanel_Cockpit_MainRemoteControl);
      checkbox.Getter = (MyTerminalValueControl<MyRemoteControl, bool>.GetterDelegate) (x => x.IsMainRemoteControl);
      checkbox.Setter = (MyTerminalValueControl<MyRemoteControl, bool>.SetterDelegate) ((x, v) => x.IsMainRemoteControl = v);
      checkbox.Enabled = (Func<MyRemoteControl, bool>) (x => x.IsMainRemoteControlFree());
      checkbox.SupportsMultipleBlocks = false;
      checkbox.EnableAction<MyRemoteControl>();
      MyTerminalControlFactory.AddControl<MyRemoteControl>((MyTerminalControl<MyRemoteControl>) checkbox);
      MyTerminalControlButton<MyRemoteControl> button = new MyTerminalControlButton<MyRemoteControl>("Control", MySpaceTexts.ControlRemote, MySpaceTexts.Blank, (Action<MyRemoteControl>) (b => b.RequestControl()));
      button.Enabled = (Func<MyRemoteControl, bool>) (r => r.CanControl(MySession.Static.ControlledEntity));
      button.SupportsMultipleBlocks = false;
      MyTerminalAction<MyRemoteControl> myTerminalAction = button.EnableAction<MyRemoteControl>(MyTerminalActionIcons.TOGGLE);
      if (myTerminalAction != null)
      {
        myTerminalAction.InvalidToolbarTypes = new List<MyToolbarType>()
        {
          MyToolbarType.ButtonPanel
        };
        myTerminalAction.ValidForGroups = false;
      }
      MyTerminalControlFactory.AddControl<MyRemoteControl>((MyTerminalControl<MyRemoteControl>) button);
      MyTerminalControlFactory.AddControl<MyRemoteControl>((MyTerminalControl<MyRemoteControl>) new MyTerminalControlSeparator<MyRemoteControl>());
      MyTerminalControlOnOffSwitch<MyRemoteControl> onOff1 = new MyTerminalControlOnOffSwitch<MyRemoteControl>("AutoPilot", MySpaceTexts.BlockPropertyTitle_AutoPilot, MySpaceTexts.Blank);
      onOff1.Getter = (MyTerminalValueControl<MyRemoteControl, bool>.GetterDelegate) (x => (bool) x.m_autoPilotEnabled);
      onOff1.Setter = (MyTerminalValueControl<MyRemoteControl, bool>.SetterDelegate) ((x, v) => x.SetAutoPilotEnabled(v));
      onOff1.Enabled = (Func<MyRemoteControl, bool>) (r => r.CanEnableAutoPilot());
      onOff1.EnableToggleAction<MyRemoteControl>();
      onOff1.EnableOnOffActions<MyRemoteControl>();
      MyTerminalControlFactory.AddControl<MyRemoteControl>((MyTerminalControl<MyRemoteControl>) onOff1);
      MyTerminalControlOnOffSwitch<MyRemoteControl> onOff2 = new MyTerminalControlOnOffSwitch<MyRemoteControl>("CollisionAvoidance", MySpaceTexts.BlockPropertyTitle_CollisionAvoidance, MySpaceTexts.Blank);
      onOff2.Getter = (MyTerminalValueControl<MyRemoteControl, bool>.GetterDelegate) (x => (bool) x.m_useCollisionAvoidance);
      onOff2.Setter = (MyTerminalValueControl<MyRemoteControl, bool>.SetterDelegate) ((x, v) => x.SetCollisionAvoidance(v));
      onOff2.Enabled = (Func<MyRemoteControl, bool>) (r => true);
      onOff2.EnableToggleAction<MyRemoteControl>();
      onOff2.EnableOnOffActions<MyRemoteControl>();
      MyTerminalControlFactory.AddControl<MyRemoteControl>((MyTerminalControl<MyRemoteControl>) onOff2);
      MyTerminalControlOnOffSwitch<MyRemoteControl> onOff3 = new MyTerminalControlOnOffSwitch<MyRemoteControl>("DockingMode", MySpaceTexts.BlockPropertyTitle_EnableDockingMode, MySpaceTexts.Blank);
      onOff3.Getter = (MyTerminalValueControl<MyRemoteControl, bool>.GetterDelegate) (x => (bool) x.m_dockingModeEnabled);
      onOff3.Setter = (MyTerminalValueControl<MyRemoteControl, bool>.SetterDelegate) ((x, v) => x.SetDockingMode(v));
      onOff3.Enabled = (Func<MyRemoteControl, bool>) (r => r.IsWorking);
      onOff3.EnableToggleAction<MyRemoteControl>();
      onOff3.EnableOnOffActions<MyRemoteControl>();
      MyTerminalControlFactory.AddControl<MyRemoteControl>((MyTerminalControl<MyRemoteControl>) onOff3);
      MyTerminalControlCombobox<MyRemoteControl> terminalControlCombobox1 = new MyTerminalControlCombobox<MyRemoteControl>("CameraList", MySpaceTexts.BlockPropertyTitle_AssignedCamera, MySpaceTexts.Blank);
      terminalControlCombobox1.ComboBoxContentWithBlock = (MyTerminalControlCombobox<MyRemoteControl>.ComboBoxContentDelegate) ((x, list) => x.FillCameraComboBoxContent(list));
      terminalControlCombobox1.Getter = (MyTerminalValueControl<MyRemoteControl, long>.GetterDelegate) (x => (long) x.m_bindedCamera);
      terminalControlCombobox1.Setter = (MyTerminalValueControl<MyRemoteControl, long>.SetterDelegate) ((x, y) => x.m_bindedCamera.Value = y);
      MyTerminalControlFactory.AddControl<MyRemoteControl>((MyTerminalControl<MyRemoteControl>) terminalControlCombobox1);
      MyRemoteControl.m_cameraList = terminalControlCombobox1;
      MyTerminalControlCombobox<MyRemoteControl> terminalControlCombobox2 = new MyTerminalControlCombobox<MyRemoteControl>("FlightMode", MySpaceTexts.BlockPropertyTitle_FlightMode, MySpaceTexts.Blank);
      terminalControlCombobox2.ComboBoxContent = (Action<List<MyTerminalControlComboBoxItem>>) (x => MyRemoteControl.FillFlightModeCombo(x));
      terminalControlCombobox2.Getter = (MyTerminalValueControl<MyRemoteControl, long>.GetterDelegate) (x => (long) x.m_currentFlightMode.Value);
      terminalControlCombobox2.Setter = (MyTerminalValueControl<MyRemoteControl, long>.SetterDelegate) ((x, v) => x.ChangeFlightMode((FlightMode) v));
      terminalControlCombobox2.SetSerializerRange((int) MyEnum<FlightMode>.Range.Min, (int) MyEnum<FlightMode>.Range.Max);
      MyTerminalControlFactory.AddControl<MyRemoteControl>((MyTerminalControl<MyRemoteControl>) terminalControlCombobox2);
      MyTerminalControlCombobox<MyRemoteControl> terminalControlCombobox3 = new MyTerminalControlCombobox<MyRemoteControl>("Direction", MySpaceTexts.BlockPropertyTitle_ForwardDirection, MySpaceTexts.Blank);
      terminalControlCombobox3.ComboBoxContent = (Action<List<MyTerminalControlComboBoxItem>>) (x => MyRemoteControl.FillDirectionCombo(x));
      terminalControlCombobox3.Getter = (MyTerminalValueControl<MyRemoteControl, long>.GetterDelegate) (x => (long) x.m_currentDirection.Value);
      terminalControlCombobox3.Setter = (MyTerminalValueControl<MyRemoteControl, long>.SetterDelegate) ((x, v) => x.ChangeDirection((Base6Directions.Direction) v));
      MyTerminalControlFactory.AddControl<MyRemoteControl>((MyTerminalControl<MyRemoteControl>) terminalControlCombobox3);
      MyTerminalControlSlider<MyRemoteControl> slider = new MyTerminalControlSlider<MyRemoteControl>("SpeedLimit", MySpaceTexts.BlockPropertyTitle_RemoteBlockSpeedLimit, MySpaceTexts.BlockPropertyTitle_RemoteBlockSpeedLimit);
      slider.SetLimits(0.0f, 100f);
      slider.DefaultValue = new float?(100f);
      slider.Getter = (MyTerminalValueControl<MyRemoteControl, float>.GetterDelegate) (x => (float) x.m_autopilotSpeedLimit);
      slider.Setter = (MyTerminalValueControl<MyRemoteControl, float>.SetterDelegate) ((x, v) => x.m_autopilotSpeedLimit.Value = v);
      slider.Writer = (MyTerminalControl<MyRemoteControl>.WriterDelegate) ((x, sb) => sb.Append(MyValueFormatter.GetFormatedFloat((float) x.m_autopilotSpeedLimit, 0)));
      slider.EnableActions<MyRemoteControl>();
      MyTerminalControlFactory.AddControl<MyRemoteControl>((MyTerminalControl<MyRemoteControl>) slider);
      MyRemoteControl.m_waypointList = new MyTerminalControlListbox<MyRemoteControl>("WaypointList", MySpaceTexts.BlockPropertyTitle_Waypoints, MySpaceTexts.Blank, true);
      MyRemoteControl.m_waypointList.ListContent = (MyTerminalControlListbox<MyRemoteControl>.ListContentDelegate) ((x, list1, list2, focusedItem) => x.FillWaypointList(list1, list2));
      MyRemoteControl.m_waypointList.ItemSelected = (MyTerminalControlListbox<MyRemoteControl>.SelectItemDelegate) ((x, y) => x.SelectWaypoint(y));
      MyTerminalControlFactory.AddControl<MyRemoteControl>((MyTerminalControl<MyRemoteControl>) MyRemoteControl.m_waypointList);
      MyTerminalControlButton<MyRemoteControl> terminalControlButton1 = new MyTerminalControlButton<MyRemoteControl>("Open Toolbar", MySpaceTexts.BlockPropertyTitle_AutoPilotToolbarOpen, MySpaceTexts.BlockPropertyPopup_AutoPilotToolbarOpen, (Action<MyRemoteControl>) (self =>
      {
        MyToolbarItem[] actions = self.m_selectedWaypoints[0].Actions;
        if (actions != null)
        {
          for (int i = 0; i < actions.Length; ++i)
          {
            if (actions[i] != null)
              self.m_actionToolbar.SetItemAtIndex(i, actions[i]);
          }
        }
        self.m_actionToolbar.ItemChanged += new Action<MyToolbar, MyToolbar.IndexArgs, bool>(self.Toolbar_ItemChanged);
        if (MyGuiScreenToolbarConfigBase.Static != null)
          return;
        MyToolbarComponent.CurrentToolbar = self.m_actionToolbar;
        MyGuiScreenBase screen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.ToolbarConfigScreen, (object) 0, (object) self, null);
        MyToolbarComponent.AutoUpdate = false;
        screen.Closed += (MyGuiScreenBase.ScreenHandler) ((source, isUnloading) =>
        {
          MyToolbarComponent.AutoUpdate = true;
          self.m_actionToolbar.ItemChanged -= new Action<MyToolbar, MyToolbar.IndexArgs, bool>(self.Toolbar_ItemChanged);
          self.m_actionToolbar.Clear();
        });
        MyGuiSandbox.AddScreen(screen);
      }));
      terminalControlButton1.Enabled = (Func<MyRemoteControl, bool>) (r => r.m_selectedWaypoints.Count == 1);
      terminalControlButton1.SupportsMultipleBlocks = false;
      MyTerminalControlFactory.AddControl<MyRemoteControl>((MyTerminalControl<MyRemoteControl>) terminalControlButton1);
      MyTerminalControlButton<MyRemoteControl> terminalControlButton2 = new MyTerminalControlButton<MyRemoteControl>("RemoveWaypoint", MySpaceTexts.BlockActionTitle_RemoveWaypoint, MySpaceTexts.Blank, (Action<MyRemoteControl>) (b => b.RemoveWaypoints()));
      terminalControlButton2.Enabled = (Func<MyRemoteControl, bool>) (r => r.CanRemoveWaypoints());
      terminalControlButton2.SupportsMultipleBlocks = false;
      MyTerminalControlFactory.AddControl<MyRemoteControl>((MyTerminalControl<MyRemoteControl>) terminalControlButton2);
      MyTerminalControlButton<MyRemoteControl> terminalControlButton3 = new MyTerminalControlButton<MyRemoteControl>("MoveUp", MySpaceTexts.BlockActionTitle_MoveWaypointUp, MySpaceTexts.Blank, (Action<MyRemoteControl>) (b => b.MoveWaypointsUp()));
      terminalControlButton3.Enabled = (Func<MyRemoteControl, bool>) (r => r.CanMoveWaypointsUp());
      terminalControlButton3.SupportsMultipleBlocks = false;
      MyTerminalControlFactory.AddControl<MyRemoteControl>((MyTerminalControl<MyRemoteControl>) terminalControlButton3);
      MyTerminalControlButton<MyRemoteControl> terminalControlButton4 = new MyTerminalControlButton<MyRemoteControl>("MoveDown", MySpaceTexts.BlockActionTitle_MoveWaypointDown, MySpaceTexts.Blank, (Action<MyRemoteControl>) (b => b.MoveWaypointsDown()));
      terminalControlButton4.Enabled = (Func<MyRemoteControl, bool>) (r => r.CanMoveWaypointsDown());
      terminalControlButton4.SupportsMultipleBlocks = false;
      MyTerminalControlFactory.AddControl<MyRemoteControl>((MyTerminalControl<MyRemoteControl>) terminalControlButton4);
      MyTerminalControlButton<MyRemoteControl> terminalControlButton5 = new MyTerminalControlButton<MyRemoteControl>("AddWaypoint", MySpaceTexts.BlockActionTitle_AddWaypoint, MySpaceTexts.Blank, (Action<MyRemoteControl>) (b => b.AddWaypoints()));
      terminalControlButton5.Enabled = (Func<MyRemoteControl, bool>) (r => r.CanAddWaypoints());
      terminalControlButton5.SupportsMultipleBlocks = false;
      MyTerminalControlFactory.AddControl<MyRemoteControl>((MyTerminalControl<MyRemoteControl>) terminalControlButton5);
      MyRemoteControl.m_gpsList = new MyTerminalControlListbox<MyRemoteControl>("GpsList", MySpaceTexts.BlockPropertyTitle_GpsLocations, MySpaceTexts.Blank, true);
      MyRemoteControl.m_gpsList.ListContent = (MyTerminalControlListbox<MyRemoteControl>.ListContentDelegate) ((x, list1, list2, focusedItem) => x.FillGpsList(list1, list2));
      MyRemoteControl.m_gpsList.ItemSelected = (MyTerminalControlListbox<MyRemoteControl>.SelectItemDelegate) ((x, y) => x.SelectGps(y));
      MyTerminalControlFactory.AddControl<MyRemoteControl>((MyTerminalControl<MyRemoteControl>) MyRemoteControl.m_gpsList);
      foreach (KeyValuePair<Base6Directions.Direction, MyStringId> directionName in MyRemoteControl.m_directionNames)
        MyTerminalControlFactory.AddAction<MyRemoteControl>(new MyTerminalAction<MyRemoteControl>(MyTexts.Get(directionName.Value).ToString(), MyTexts.Get(directionName.Value), new Action<MyRemoteControl, ListReader<TerminalActionParameter>>(MyRemoteControl.OnAction), (MyTerminalControl<MyRemoteControl>.WriterDelegate) null, MyTerminalActionIcons.TOGGLE)
        {
          Enabled = (Func<MyRemoteControl, bool>) (b => b.IsWorking),
          ParameterDefinitions = {
            TerminalActionParameter.Get((object) (byte) directionName.Key)
          }
        });
      MyTerminalControlButton<MyRemoteControl> terminalControlButton6 = new MyTerminalControlButton<MyRemoteControl>("Reset", MySpaceTexts.BlockActionTitle_WaypointReset, MySpaceTexts.BlockActionTooltip_WaypointReset, (Action<MyRemoteControl>) (b => b.ResetWaypoint()), true);
      terminalControlButton6.Enabled = (Func<MyRemoteControl, bool>) (r => r.IsWorking);
      terminalControlButton6.SupportsMultipleBlocks = false;
      MyTerminalControlFactory.AddControl<MyRemoteControl>((MyTerminalControl<MyRemoteControl>) terminalControlButton6);
      MyTerminalControlButton<MyRemoteControl> terminalControlButton7 = new MyTerminalControlButton<MyRemoteControl>("Copy", MySpaceTexts.BlockActionTitle_RemoteCopy, MySpaceTexts.Blank, (Action<MyRemoteControl>) (b => b.CopyAutopilotSetup()));
      terminalControlButton7.Enabled = (Func<MyRemoteControl, bool>) (r => r.IsWorking);
      terminalControlButton7.SupportsMultipleBlocks = false;
      MyTerminalControlFactory.AddControl<MyRemoteControl>((MyTerminalControl<MyRemoteControl>) terminalControlButton7);
      MyTerminalControlButton<MyRemoteControl> terminalControlButton8 = new MyTerminalControlButton<MyRemoteControl>("Paste", MySpaceTexts.BlockActionTitle_RemotePaste, MySpaceTexts.Blank, (Action<MyRemoteControl>) (b => b.PasteAutopilotSetup()));
      terminalControlButton8.Enabled = (Func<MyRemoteControl, bool>) (r => r.IsWorking && MyRemoteControl.m_clipboard != null);
      terminalControlButton8.SupportsMultipleBlocks = false;
      MyTerminalControlFactory.AddControl<MyRemoteControl>((MyTerminalControl<MyRemoteControl>) terminalControlButton8);
    }

    private static void OnAction(
      MyRemoteControl block,
      ListReader<TerminalActionParameter> paramteres)
    {
      TerminalActionParameter terminalActionParameter = paramteres.FirstOrDefault<TerminalActionParameter>();
      if (terminalActionParameter.IsEmpty)
        return;
      block.ChangeDirection((Base6Directions.Direction) terminalActionParameter.Value);
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.SyncFlag = true;
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(this.BlockDefinition.ResourceSinkGroup, this.BlockDefinition.RequiredPowerInput, new Func<float>(this.CalculateRequiredPowerInput));
      this.ResourceSink = resourceSinkComponent;
      base.Init(objectBuilder, cubeGrid);
      this.NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      this.m_parallelFlag.Enable((MyEntity) this);
      MyObjectBuilder_RemoteControl builderRemoteControl = (MyObjectBuilder_RemoteControl) objectBuilder;
      this.m_savedPreviousControlledEntityId = builderRemoteControl.PreviousControlledEntityId;
      resourceSinkComponent.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
      resourceSinkComponent.RequiredInputChanged += new MyRequiredResourceChangeDelegate(this.Receiver_RequiredInputChanged);
      resourceSinkComponent.Update();
      this.m_autoPilotEnabled.SetLocalValue(builderRemoteControl.AutoPilotEnabled);
      this.m_dockingModeEnabled.SetLocalValue(builderRemoteControl.DockingModeEnabled);
      this.m_currentFlightMode.SetLocalValue((FlightMode) builderRemoteControl.FlightMode);
      this.m_currentDirection.SetLocalValue((Base6Directions.Direction) builderRemoteControl.Direction);
      this.m_waitForFreeWay.SetLocalValue(builderRemoteControl.WaitForFreeWay);
      this.m_autopilotSpeedLimit.SetLocalValue(builderRemoteControl.AutopilotSpeedLimit);
      this.m_bindedCamera.SetLocalValue(builderRemoteControl.BindedCamera);
      this.m_waypointThresholdDistance.SetLocalValue(builderRemoteControl.WaypointThresholdDistance);
      this.m_isMainRemoteControl.SetLocalValue(builderRemoteControl.IsMainRemoteControl);
      this.m_stuckDetection = new MyStuckDetection(0.03f, 0.01f, this.CubeGrid.PositionComp.WorldAABB);
      if (builderRemoteControl.Coords == null || builderRemoteControl.Coords.Count == 0)
      {
        if (builderRemoteControl.Waypoints == null)
        {
          this.m_waypoints = new List<MyRemoteControl.MyAutopilotWaypoint>();
          this.CurrentWaypoint = (MyRemoteControl.MyAutopilotWaypoint) null;
        }
        else
        {
          this.m_waypoints = new List<MyRemoteControl.MyAutopilotWaypoint>(builderRemoteControl.Waypoints.Count);
          for (int index = 0; index < builderRemoteControl.Waypoints.Count; ++index)
            this.m_waypoints.Add(new MyRemoteControl.MyAutopilotWaypoint(builderRemoteControl.Waypoints[index], this));
        }
      }
      else
      {
        this.m_waypoints = new List<MyRemoteControl.MyAutopilotWaypoint>(builderRemoteControl.Coords.Count);
        for (int index = 0; index < builderRemoteControl.Coords.Count; ++index)
          this.m_waypoints.Add(new MyRemoteControl.MyAutopilotWaypoint(builderRemoteControl.Coords[index], builderRemoteControl.Names[index], this));
        if (builderRemoteControl.AutoPilotToolbar != null && (FlightMode) this.m_currentFlightMode == FlightMode.OneWay)
          this.m_waypoints[this.m_waypoints.Count - 1].SetActions(builderRemoteControl.AutoPilotToolbar.Slots);
      }
      this.CurrentWaypoint = builderRemoteControl.CurrentWaypointIndex == -1 || builderRemoteControl.CurrentWaypointIndex >= this.m_waypoints.Count ? (MyRemoteControl.MyAutopilotWaypoint) null : this.m_waypoints[builderRemoteControl.CurrentWaypointIndex];
      this.UpdatePlanetWaypointInfo();
      this.m_actionToolbar = new MyToolbar(MyToolbarType.ButtonPanel, pageCount: 1);
      this.m_actionToolbar.DrawNumbers = false;
      this.m_actionToolbar.Init((MyObjectBuilder_Toolbar) null, (MyEntity) this);
      this.m_selectedGpsLocations = new List<IMyGps>();
      this.m_selectedWaypoints = new List<MyRemoteControl.MyAutopilotWaypoint>();
      this.RaisePropertiesChangedRemote();
      this.SetDetailedInfoDirty();
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyRemoteControl.MyDebugRenderComponentRemoteControl(this));
      this.m_useCollisionAvoidance.SetLocalValue(builderRemoteControl.CollisionAvoidance);
      if (builderRemoteControl.AutomaticBehaviour == null || !(builderRemoteControl.AutomaticBehaviour is MyObjectBuilder_DroneAI))
        return;
      MyDroneAI myDroneAi = new MyDroneAI();
      myDroneAi.Load(builderRemoteControl.AutomaticBehaviour, this);
      this.SetAutomaticBehaviour((MyRemoteControl.IRemoteControlAutomaticBehaviour) myDroneAi);
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      this.ResourceSink.Update();
      if ((bool) this.m_autoPilotEnabled)
        this.ControlGroup.GetGroup(this.CubeGrid)?.GroupData.ControlSystem.AddControllerBlock((MyShipController) this);
      if (this.CubeGrid.IsPreview || this.CubeGrid.GridSystems == null || this.CubeGrid.GridSystems.RadioSystem == null)
        return;
      this.CubeGrid.GridSystems.RadioSystem.UpdateRemoteControlInfo();
    }

    public override void UpdateOnceBeforeFrame()
    {
      if (!Sandbox.Engine.Platform.Game.IsDedicated)
      {
        if (MyRemoteControl.m_waypointGuiControl == null && MyRemoteControl.m_waypointList != null)
        {
          MyRemoteControl.m_waypointGuiControl = (MyGuiControlListbox) ((MyGuiControlBlockProperty) MyRemoteControl.m_waypointList.GetGuiControl()).PropertyControl;
          MyRemoteControl.m_waypointList = (MyTerminalControlListbox<MyRemoteControl>) null;
        }
        if (MyRemoteControl.m_gpsGuiControl == null && MyRemoteControl.m_gpsList != null)
        {
          MyRemoteControl.m_gpsGuiControl = (MyGuiControlListbox) ((MyGuiControlBlockProperty) MyRemoteControl.m_gpsList.GetGuiControl()).PropertyControl;
          MyRemoteControl.m_gpsList = (MyTerminalControlListbox<MyRemoteControl>) null;
        }
      }
      base.UpdateOnceBeforeFrame();
      if (!(bool) this.m_autoPilotEnabled || this.CubeGrid.IsPreview)
        return;
      this.ControlGroup.GetGroup(this.CubeGrid)?.GroupData.ControlSystem.AddControllerBlock((MyShipController) this);
    }

    private bool CanEnableAutoPilot() => (this.m_automaticBehaviour != null || this.m_waypoints.Count != 0 && (this.m_waypoints.Count != 1 || (FlightMode) this.m_currentFlightMode == FlightMode.OneWay)) && this.IsFunctional && this.m_previousControlledEntity == null;

    private static void FillFlightModeCombo(List<MyTerminalControlComboBoxItem> list)
    {
      list.Add(new MyTerminalControlComboBoxItem()
      {
        Key = 0L,
        Value = MySpaceTexts.BlockPropertyTitle_FlightMode_Patrol
      });
      list.Add(new MyTerminalControlComboBoxItem()
      {
        Key = 1L,
        Value = MySpaceTexts.BlockPropertyTitle_FlightMode_Circle
      });
      list.Add(new MyTerminalControlComboBoxItem()
      {
        Key = 2L,
        Value = MySpaceTexts.BlockPropertyTitle_FlightMode_OneWay
      });
    }

    private static void FillDirectionCombo(List<MyTerminalControlComboBoxItem> list)
    {
      foreach (KeyValuePair<Base6Directions.Direction, MyStringId> directionName in MyRemoteControl.m_directionNames)
        list.Add(new MyTerminalControlComboBoxItem()
        {
          Key = (long) directionName.Key,
          Value = directionName.Value
        });
    }

    public void SetCollisionAvoidance(bool enabled) => this.m_useCollisionAvoidance.Value = enabled;

    public void SetAutoPilotEnabled(bool enabled)
    {
      if (!this.CanEnableAutoPilot() && enabled)
        return;
      if (!enabled)
        this.ClearMovementControl();
      this.m_autoPilotEnabled.Value = enabled;
    }

    bool Sandbox.ModAPI.Ingame.IMyRemoteControl.IsAutoPilotEnabled => this.m_autoPilotEnabled.Value;

    public bool IsAutopilotEnabled() => this.m_autoPilotEnabled.Value;

    public bool HasWaypoints() => this.m_waypoints.Count > 0;

    public void SetWaypointThresholdDistance(float thresholdDistance) => this.m_waypointThresholdDistance.Value = thresholdDistance;

    private void RemoveAutoPilot()
    {
      MyEntityThrustComponent entityThrustComponent = this.CubeGrid.Components.Get<MyEntityThrustComponent>();
      if (entityThrustComponent != null)
        entityThrustComponent.AutoPilotControlThrust = Vector3.Zero;
      this.CubeGrid.GridSystems.GyroSystem.ControlTorque = Vector3.Zero;
      this.ControlGroup.GetGroup(this.CubeGrid)?.GroupData.ControlSystem.RemoveControllerBlock((MyShipController) this);
      if (this.CubeGrid.GridSystems.ControlSystem == null || this.CubeGrid.GridSystems.ControlSystem.GetShipController() is MyRemoteControl shipController && (bool) shipController.m_autoPilotEnabled)
        return;
      this.SetAutopilot(false);
    }

    private void OnSetAutoPilotEnabled()
    {
      if (MyEntities.IsAsyncUpdateInProgress)
      {
        MySandboxGame.Static.Invoke((Action) (() => this.OnSetAutoPilotEnabled()), "Auto Pilot Enabled Change");
      }
      else
      {
        if (!(bool) this.m_autoPilotEnabled)
        {
          this.RemoveAutoPilot();
          if (this.m_automaticBehaviour != null)
            this.m_automaticBehaviour.StopWorking();
        }
        else
        {
          this.ControlGroup.GetGroup(this.CubeGrid)?.GroupData.ControlSystem.AddControllerBlock((MyShipController) this);
          this.SetAutopilot(true);
          this.ResetShipControls();
        }
        this.ResourceSink.Update();
      }
    }

    private void SetAutopilot(bool enabled)
    {
      MyEntityThrustComponent entityThrustComponent = this.CubeGrid.Components.Get<MyEntityThrustComponent>();
      if (entityThrustComponent != null)
      {
        entityThrustComponent.AutopilotEnabled = enabled;
        entityThrustComponent.MarkDirty();
      }
      if (this.CubeGrid.GridSystems.GyroSystem == null)
        return;
      this.CubeGrid.GridSystems.GyroSystem.AutopilotEnabled = enabled;
      this.CubeGrid.GridSystems.GyroSystem.MarkDirty();
    }

    public void SetDockingMode(bool enabled) => this.m_dockingModeEnabled.Value = enabled;

    private void SelectGps(List<MyGuiControlListbox.Item> selection)
    {
      this.m_selectedGpsLocations.Clear();
      if (selection.Count > 0)
      {
        foreach (MyGuiControlListbox.Item obj in selection)
          this.m_selectedGpsLocations.Add((IMyGps) obj.UserData);
      }
      this.RaisePropertiesChangedRemote();
    }

    private void SelectWaypoint(List<MyGuiControlListbox.Item> selection)
    {
      this.m_selectedWaypoints.Clear();
      if (selection.Count > 0)
      {
        foreach (MyGuiControlListbox.Item obj in selection)
          this.m_selectedWaypoints.Add((MyRemoteControl.MyAutopilotWaypoint) obj.UserData);
      }
      this.RaisePropertiesChangedRemote();
    }

    private void AddWaypoints()
    {
      if (this.m_selectedGpsLocations.Count <= 0)
        return;
      int count = this.m_selectedGpsLocations.Count;
      Vector3D[] vector3DArray = new Vector3D[count];
      string[] strArray = new string[count];
      for (int index = 0; index < count; ++index)
      {
        vector3DArray[index] = this.m_selectedGpsLocations[index].Coords;
        strArray[index] = this.m_selectedGpsLocations[index].Name;
      }
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyRemoteControl, Vector3D[], string[]>(this, (Func<MyRemoteControl, Action<Vector3D[], string[]>>) (x => new Action<Vector3D[], string[]>(x.OnAddWaypoints)), vector3DArray, strArray);
      this.m_selectedGpsLocations.Clear();
    }

    [Event(null, 990)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnAddWaypoints(Vector3D[] coords, string[] names)
    {
      for (int index = 0; index < coords.Length; ++index)
        this.m_waypoints.Add(new MyRemoteControl.MyAutopilotWaypoint(coords[index], names[index], this));
      this.RaisePropertiesChangedRemote();
    }

    private bool CanMoveItemUp(int index)
    {
      if (index == -1)
        return false;
      for (int index1 = index - 1; index1 >= 0; --index1)
      {
        if (!this.m_selectedWaypoints.Contains(this.m_waypoints[index1]))
          return true;
      }
      return false;
    }

    private void MoveWaypointsUp()
    {
      if (this.m_selectedWaypoints.Count <= 0)
        return;
      List<int> intList = new List<int>(this.m_selectedWaypoints.Count);
      foreach (MyRemoteControl.MyAutopilotWaypoint selectedWaypoint in this.m_selectedWaypoints)
      {
        int index = this.m_waypoints.IndexOf(selectedWaypoint);
        if (this.CanMoveItemUp(index))
          intList.Add(index);
      }
      if (intList.Count <= 0)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyRemoteControl, List<int>>(this, (Func<MyRemoteControl, Action<List<int>>>) (x => new Action<List<int>>(x.OnMoveWaypointsUp)), intList);
    }

    [Event(null, 1041)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnMoveWaypointsUp(List<int> indexes)
    {
      for (int index = 0; index < indexes.Count; ++index)
        this.SwapWaypoints(indexes[index] - 1, indexes[index]);
      this.RaisePropertiesChangedRemote();
    }

    private bool CanMoveItemDown(int index)
    {
      if (index == -1)
        return false;
      for (int index1 = index + 1; index1 < this.m_waypoints.Count; ++index1)
      {
        if (!this.m_selectedWaypoints.Contains(this.m_waypoints[index1]))
          return true;
      }
      return false;
    }

    private void MoveWaypointsDown()
    {
      if (this.m_selectedWaypoints.Count <= 0)
        return;
      List<int> intList = new List<int>(this.m_selectedWaypoints.Count);
      foreach (MyRemoteControl.MyAutopilotWaypoint selectedWaypoint in this.m_selectedWaypoints)
      {
        int index = this.m_waypoints.IndexOf(selectedWaypoint);
        if (this.CanMoveItemDown(index))
          intList.Add(index);
      }
      if (intList.Count <= 0)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyRemoteControl, List<int>>(this, (Func<MyRemoteControl, Action<List<int>>>) (x => new Action<List<int>>(x.OnMoveWaypointsDown)), intList);
    }

    [Event(null, 1090)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnMoveWaypointsDown(List<int> indexes)
    {
      for (int index1 = indexes.Count - 1; index1 >= 0; --index1)
      {
        int index2 = indexes[index1];
        this.SwapWaypoints(index2, index2 + 1);
      }
      this.RaisePropertiesChangedRemote();
    }

    private void SwapWaypoints(int index1, int index2)
    {
      MyRemoteControl.MyAutopilotWaypoint waypoint1 = this.m_waypoints[index1];
      MyRemoteControl.MyAutopilotWaypoint waypoint2 = this.m_waypoints[index2];
      this.m_waypoints[index1] = waypoint2;
      this.m_waypoints[index2] = waypoint1;
    }

    private void RemoveWaypoints()
    {
      if (this.m_selectedWaypoints.Count <= 0)
        return;
      int[] array = new int[this.m_selectedWaypoints.Count];
      for (int index = 0; index < this.m_selectedWaypoints.Count; ++index)
      {
        MyRemoteControl.MyAutopilotWaypoint selectedWaypoint = this.m_selectedWaypoints[index];
        array[index] = this.m_waypoints.IndexOf(selectedWaypoint);
      }
      Array.Sort<int>(array);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyRemoteControl, int[]>(this, (Func<MyRemoteControl, Action<int[]>>) (x => new Action<int[]>(x.OnRemoveWaypoints)), array);
      this.m_selectedWaypoints.Clear();
      this.RaisePropertiesChangedRemote();
    }

    [Event(null, 1131)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnRemoveWaypoints(int[] indexes)
    {
      bool flag = false;
      for (int index1 = indexes.Length - 1; index1 >= 0; --index1)
      {
        int index2 = indexes[index1];
        if (index2 > -1 && index2 < this.m_waypoints.Count)
        {
          MyRemoteControl.MyAutopilotWaypoint waypoint = this.m_waypoints[index2];
          this.m_waypoints.Remove(waypoint);
          if (this.CurrentWaypoint == waypoint)
            flag = true;
        }
      }
      if (flag)
        this.AdvanceWaypoint();
      this.RaisePropertiesChangedRemote();
      if (!this.IsAutopilotEnabled() || this.CanEnableAutoPilot())
        return;
      this.SetAutoPilotEnabled(false);
    }

    public void ChangeFlightMode(FlightMode flightMode)
    {
      if (flightMode != (FlightMode) this.m_currentFlightMode)
        this.m_currentFlightMode.Value = flightMode;
      this.SetAutoPilotEnabled((bool) this.m_autoPilotEnabled);
    }

    public void SetWaitForFreeWay(bool waitForFreeWay)
    {
      if (waitForFreeWay != (bool) this.m_waitForFreeWay)
        this.m_waitForFreeWay.Value = waitForFreeWay;
      this.SetAutoPilotEnabled((bool) this.m_autoPilotEnabled);
    }

    public void SetAutoPilotSpeedLimit(float speedLimit) => this.m_autopilotSpeedLimit.Value = speedLimit;

    public void ChangeDirection(Base6Directions.Direction direction) => this.m_currentDirection.Value = direction;

    private bool CanAddWaypoints()
    {
      if (this.m_selectedGpsLocations.Count == 0)
        return false;
      int count = this.m_waypoints.Count;
      return true;
    }

    private bool CanMoveWaypointsUp()
    {
      if (this.m_selectedWaypoints.Count == 0 || this.m_waypoints.Count == 0)
        return false;
      foreach (MyRemoteControl.MyAutopilotWaypoint selectedWaypoint in this.m_selectedWaypoints)
      {
        if (this.CanMoveItemUp(this.m_waypoints.IndexOf(selectedWaypoint)))
          return true;
      }
      return false;
    }

    private bool CanMoveWaypointsDown()
    {
      if (this.m_selectedWaypoints.Count == 0 || this.m_waypoints.Count == 0)
        return false;
      foreach (MyRemoteControl.MyAutopilotWaypoint selectedWaypoint in this.m_selectedWaypoints)
      {
        if (this.CanMoveItemDown(this.m_waypoints.IndexOf(selectedWaypoint)))
          return true;
      }
      return false;
    }

    private bool CanRemoveWaypoints() => this.m_selectedWaypoints.Count > 0;

    private void ResetWaypoint()
    {
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyRemoteControl>(this, (Func<MyRemoteControl, Action>) (x => new Action(x.OnResetWaypoint)));
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.OnResetWaypoint();
    }

    [Event(null, 1271)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [BroadcastExcept]
    private void OnResetWaypoint()
    {
      if (this.m_waypoints.Count <= 0)
        return;
      this.CurrentWaypoint = this.m_waypoints[0];
      this.m_patrolDirectionForward = true;
      this.RaisePropertiesChangedRemote();
    }

    private void CopyAutopilotSetup()
    {
      MyRemoteControl.m_clipboard = new MyObjectBuilder_AutopilotClipboard();
      MyRemoteControl.m_clipboard.Direction = (byte) this.m_currentDirection.Value;
      MyRemoteControl.m_clipboard.FlightMode = (int) this.m_currentFlightMode.Value;
      MyRemoteControl.m_clipboard.RemoteEntityId = this.EntityId;
      MyRemoteControl.m_clipboard.DockingModeEnabled = (bool) this.m_dockingModeEnabled;
      MyRemoteControl.m_clipboard.Waypoints = new List<MyObjectBuilder_AutopilotWaypoint>(this.m_waypoints.Count);
      foreach (MyRemoteControl.MyAutopilotWaypoint waypoint in this.m_waypoints)
        MyRemoteControl.m_clipboard.Waypoints.Add(waypoint.GetObjectBuilder());
      this.RaisePropertiesChangedRemote();
    }

    private void PasteAutopilotSetup()
    {
      if (MyRemoteControl.m_clipboard == null)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyRemoteControl, MyObjectBuilder_AutopilotClipboard>(this, (Func<MyRemoteControl, Action<MyObjectBuilder_AutopilotClipboard>>) (x => new Action<MyObjectBuilder_AutopilotClipboard>(x.OnPasteAutopilotSetup)), MyRemoteControl.m_clipboard);
    }

    [Event(null, 1305)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnPasteAutopilotSetup(MyObjectBuilder_AutopilotClipboard clipboard)
    {
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        this.m_currentDirection.Value = (Base6Directions.Direction) clipboard.Direction;
        this.m_currentFlightMode.Value = (FlightMode) clipboard.FlightMode;
        this.m_dockingModeEnabled.Value = clipboard.DockingModeEnabled;
      }
      if (clipboard.Waypoints != null)
      {
        this.m_waypoints = new List<MyRemoteControl.MyAutopilotWaypoint>(clipboard.Waypoints.Count);
        foreach (MyObjectBuilder_AutopilotWaypoint waypoint in clipboard.Waypoints)
        {
          if (waypoint.Actions != null)
          {
            foreach (MyObjectBuilder_ToolbarItem action in waypoint.Actions)
            {
              if (action is MyObjectBuilder_ToolbarItemTerminalBlock itemTerminalBlock && itemTerminalBlock.BlockEntityId == clipboard.RemoteEntityId)
                itemTerminalBlock.BlockEntityId = this.EntityId;
            }
          }
          this.m_waypoints.Add(new MyRemoteControl.MyAutopilotWaypoint(waypoint, this));
        }
      }
      this.m_selectedWaypoints.Clear();
      this.RaisePropertiesChangedRemote();
    }

    public void ClearWaypoints()
    {
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyRemoteControl>(this, (Func<MyRemoteControl, Action>) (x => new Action(x.ClearWaypoints_Implementation)));
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.ClearWaypoints_Implementation();
    }

    void Sandbox.ModAPI.Ingame.IMyRemoteControl.GetWaypointInfo(
      List<MyWaypointInfo> waypoints)
    {
      if (waypoints == null)
        return;
      waypoints.Clear();
      for (int index = 0; index < this.m_waypoints.Count; ++index)
      {
        MyRemoteControl.MyAutopilotWaypoint waypoint = this.m_waypoints[index];
        waypoints.Add(new MyWaypointInfo(waypoint.Name, waypoint.Coords));
      }
    }

    [Event(null, 1361)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [BroadcastExcept]
    private void ClearWaypoints_Implementation()
    {
      this.m_waypoints.Clear();
      this.AdvanceWaypoint();
      this.RaisePropertiesChangedRemote();
    }

    public void AddWaypoint(Vector3D point, string name) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyRemoteControl, Vector3D, string>(this, (Func<MyRemoteControl, Action<Vector3D, string>>) (x => new Action<Vector3D, string>(x.OnAddWaypoint)), point, name);

    public void AddWaypoint(MyWaypointInfo coords) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyRemoteControl, Vector3D, string>(this, (Func<MyRemoteControl, Action<Vector3D, string>>) (x => new Action<Vector3D, string>(x.OnAddWaypoint)), coords.Coords, coords.Name);

    [Event(null, 1379)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnAddWaypoint(Vector3D point, string name)
    {
      this.m_waypoints.Add(new MyRemoteControl.MyAutopilotWaypoint(point, name, this));
      this.RaisePropertiesChangedRemote();
    }

    public void AddWaypoint(
      Vector3D point,
      string name,
      List<MyObjectBuilder_ToolbarItem> actionBuilders)
    {
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyRemoteControl, Vector3D, string, List<MyObjectBuilder_ToolbarItem>>(this, (Func<MyRemoteControl, Action<Vector3D, string, List<MyObjectBuilder_ToolbarItem>>>) (x => new Action<Vector3D, string, List<MyObjectBuilder_ToolbarItem>>(x.OnAddWaypoint)), point, name, actionBuilders);
    }

    [Event(null, 1391)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnAddWaypoint(
      Vector3D point,
      string name,
      [DynamicItem(typeof (MyObjectBuilderDynamicSerializer), false)] List<MyObjectBuilder_ToolbarItem> actionBuilders)
    {
      this.m_waypoints.Add(new MyRemoteControl.MyAutopilotWaypoint(point, name, actionBuilders, (List<int>) null, this));
      this.RaisePropertiesChangedRemote();
    }

    private void FillGpsList(
      ICollection<MyGuiControlListbox.Item> gpsItemList,
      ICollection<MyGuiControlListbox.Item> selectedGpsItemList)
    {
      List<IMyGps> list = new List<IMyGps>();
      MySession.Static.Gpss.GetGpsList(MySession.Static.LocalPlayerId, list);
      foreach (IMyGps myGps in list)
      {
        MyGuiControlListbox.Item obj = new MyGuiControlListbox.Item(new StringBuilder(myGps.Name), userData: ((object) myGps));
        gpsItemList.Add(obj);
        if (this.m_selectedGpsLocations.Contains(myGps))
          selectedGpsItemList.Add(obj);
      }
    }

    private void FillWaypointList(
      ICollection<MyGuiControlListbox.Item> waypoints,
      ICollection<MyGuiControlListbox.Item> selectedWaypoints)
    {
      foreach (MyRemoteControl.MyAutopilotWaypoint waypoint in this.m_waypoints)
      {
        this.m_tempName.Append(waypoint.Name);
        int num1 = 0;
        this.m_tempActions.Append("\nActions:");
        if (waypoint.Actions != null)
        {
          foreach (MyToolbarItem action in waypoint.Actions)
          {
            if (action != null)
            {
              this.m_tempActions.Append("\n");
              int num2 = (int) action.Update((MyEntity) this);
              this.m_tempActions.AppendStringBuilder(action.DisplayName);
              ++num1;
            }
          }
        }
        this.m_tempTooltip.AppendStringBuilder(this.m_tempName);
        this.m_tempTooltip.Append('\n');
        this.m_tempTooltip.Append(waypoint.Coords.ToString());
        if (num1 > 0)
        {
          this.m_tempName.Append(" [");
          this.m_tempName.Append(num1.ToString());
          if (num1 > 1)
            this.m_tempName.Append(" Actions]");
          else
            this.m_tempName.Append(" Action]");
          this.m_tempTooltip.AppendStringBuilder(this.m_tempActions);
        }
        MyGuiControlListbox.Item obj = new MyGuiControlListbox.Item(this.m_tempName, this.m_tempTooltip.ToString(), userData: ((object) waypoint));
        waypoints.Add(obj);
        if (this.m_selectedWaypoints.Contains(waypoint))
          selectedWaypoints.Add(obj);
        this.m_tempName.Clear();
        this.m_tempTooltip.Clear();
        this.m_tempActions.Clear();
      }
    }

    private void Toolbar_ItemChanged(MyToolbar self, MyToolbar.IndexArgs index, bool isGamepad)
    {
      if (this.m_selectedWaypoints.Count != 1)
        return;
      int waypointIndex = this.m_waypoints.IndexOf(this.m_selectedWaypoints[0]);
      if (waypointIndex < 0)
        return;
      this.SendToolbarItemChanged(ToolbarItem.FromItem(self.GetItemAtIndex(index.ItemIndex)), index.ItemIndex, waypointIndex);
    }

    private void RaisePropertiesChangedRemote()
    {
      int num1 = MyRemoteControl.m_gpsGuiControl != null ? MyRemoteControl.m_gpsGuiControl.FirstVisibleRow : 0;
      int num2 = MyRemoteControl.m_waypointGuiControl != null ? MyRemoteControl.m_waypointGuiControl.FirstVisibleRow : 0;
      MySandboxGame.Static.Invoke(new Action(((MyTerminalBlock) this).RaisePropertiesChanged), "MyRemoteControl.RaisePropertiesChangedRemote");
      if (MyRemoteControl.m_gpsGuiControl != null && num1 < MyRemoteControl.m_gpsGuiControl.Items.Count)
        MyRemoteControl.m_gpsGuiControl.FirstVisibleRow = num1;
      if (MyRemoteControl.m_waypointGuiControl == null || num2 >= MyRemoteControl.m_waypointGuiControl.Items.Count)
        return;
      MyRemoteControl.m_waypointGuiControl.FirstVisibleRow = num2;
    }

    private void UpdateAutopilot()
    {
      if (!this.IsWorking || !(bool) this.m_autoPilotEnabled)
        return;
      MyShipController shipController = this.CubeGrid.GridSystems.ControlSystem.GetShipController();
      if (shipController == null)
      {
        this.ControlGroup.GetGroup(this.CubeGrid)?.GroupData.ControlSystem.AddControllerBlock((MyShipController) this);
        shipController = this.CubeGrid.GridSystems.ControlSystem.GetShipController();
      }
      if (shipController != this)
        return;
      MyEntityThrustComponent entityThrustComponent = this.CubeGrid.Components.Get<MyEntityThrustComponent>();
      if (entityThrustComponent != null && !entityThrustComponent.AutopilotEnabled)
        entityThrustComponent.AutopilotEnabled = true;
      if (entityThrustComponent != null)
        entityThrustComponent.Enabled = this.ControlThrusters;
      if (this.CurrentWaypoint == null && this.m_waypoints.Count > 0)
      {
        this.CurrentWaypoint = this.m_waypoints[0];
        this.UpdatePlanetWaypointInfo();
        this.RaisePropertiesChangedRemote();
        this.SetDetailedInfoDirty();
      }
      MatrixD worldMatrix;
      if (this.CurrentWaypoint != null)
      {
        if ((this.m_automaticBehaviour == null || !this.m_automaticBehaviour.Ambushing) && (this.IsInStoppingDistance() || this.m_automaticBehaviour == null && this.m_stuckDetection.IsStuck))
        {
          this.AdvanceWaypoint();
          if (!(bool) this.m_autoPilotEnabled)
            this.m_forceBehaviorUpdate = true;
        }
        if (MyFakes.ENABLE_VR_REMOTE_CONTROL_WAYPOINTS_FAST_MOVEMENT)
        {
          MyRemoteControl.MyAutopilotWaypoint autopilotWaypoint = (MyRemoteControl.MyAutopilotWaypoint) null;
          while (this.CurrentWaypoint != null && this.CurrentWaypoint != autopilotWaypoint && (this.m_automaticBehaviour == null || !this.m_automaticBehaviour.IsActive) && this.IsInStoppingDistance())
          {
            autopilotWaypoint = this.CurrentWaypoint;
            this.AdvanceWaypoint();
            if (!(bool) this.m_autoPilotEnabled)
              this.m_forceBehaviorUpdate = true;
          }
        }
        if (Sandbox.Game.Multiplayer.Sync.IsServer && this.CurrentWaypoint != null && (!this.IsInStoppingDistance() && (bool) this.m_autoPilotEnabled))
        {
          Vector3D deltaPos;
          Vector3D perpDeltaPos;
          Vector3D targetDelta;
          float autopilotSpeedLimit;
          this.CalculateDeltaPos(entityThrustComponent, out deltaPos, out perpDeltaPos, out targetDelta, out autopilotSpeedLimit);
          bool rotating;
          bool isLabile;
          this.UpdateGyro(targetDelta, perpDeltaPos, out rotating, out isLabile);
          if (this.m_automaticBehaviour == null)
            this.m_stuckDetection.SetRotating(rotating);
          if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
          {
            MyRenderProxy.DebugDrawLine3D(this.WorldMatrix.Translation, this.WorldMatrix.Translation + deltaPos, Color.Green, Color.GreenYellow, false);
            foreach (MyRemoteControl.MyAutopilotWaypoint waypoint in this.m_waypoints)
            {
              MyRenderProxy.DebugDrawSphere(waypoint.Coords, 3f, Color.GreenYellow, depthRead: false, smooth: true);
              MyRenderProxy.DebugDrawText3D(waypoint.Coords, waypoint.Name, Color.GreenYellow, 1f, false);
            }
          }
          this.m_rotateFor -= 0.01666667f;
          if (this.m_automaticBehaviour != null && this.m_automaticBehaviour.Ambushing)
          {
            if (entityThrustComponent != null)
              entityThrustComponent.AutoPilotControlThrust = Vector3.Zero;
          }
          else
          {
            if (((this.m_automaticBehaviour == null ? 1 : (!this.m_automaticBehaviour.IsActive ? 1 : 0)) & (rotating ? 1 : 0)) != 0 && !isLabile)
            {
              if (MyFakes.ENABLE_NEW_COLLISION_AVOIDANCE && this.m_useCollisionAvoidance.Value)
              {
                Vector3D coords = this.CurrentWaypoint.Coords;
                worldMatrix = this.WorldMatrix;
                Vector3D translation = worldMatrix.Translation;
                if (Vector3D.DistanceSquared(coords, translation) >= 25.0)
                  goto label_39;
              }
              if (entityThrustComponent != null)
              {
                entityThrustComponent.AutoPilotControlThrust = Vector3.Zero;
                goto label_43;
              }
              else
                goto label_43;
            }
label_39:
            this.UpdateThrust(entityThrustComponent, deltaPos, perpDeltaPos, (double) autopilotSpeedLimit);
          }
        }
      }
      else if (Sandbox.Game.Multiplayer.Sync.IsServer && this.m_automaticBehaviour != null && (this.m_automaticBehaviour.IsActive && this.m_automaticBehaviour.RotateToTarget))
      {
        bool rotating;
        bool isLabile;
        this.UpdateGyro((Vector3D) Vector3.Zero, (Vector3D) Vector3.Zero, out rotating, out isLabile);
        if (rotating && !isLabile && entityThrustComponent != null)
          entityThrustComponent.AutoPilotControlThrust = Vector3.Zero;
      }
label_43:
      if (this.m_automaticBehaviour != null)
        return;
      MyStuckDetection stuckDetection = this.m_stuckDetection;
      worldMatrix = this.WorldMatrix;
      Vector3D translation1 = worldMatrix.Translation;
      worldMatrix = this.WorldMatrix;
      Vector3 forward = (Vector3) worldMatrix.Forward;
      Vector3D targetLocation = this.CurrentWaypoint == null ? Vector3D.Zero : this.CurrentWaypoint.Coords;
      stuckDetection.Update(translation1, forward, targetLocation);
    }

    private bool IsInStoppingDistance()
    {
      int num1 = this.m_waypoints.IndexOf(this.CurrentWaypoint);
      double num2 = (this.WorldMatrix.Translation - this.CurrentWaypoint.Coords).LengthSquared();
      double radius = (double) this.CubeGrid.GridSize * 3.0;
      if (this.m_automaticBehaviour != null && this.m_automaticBehaviour.IsActive)
        radius = (double) this.m_automaticBehaviour.WaypointThresholdDistance;
      else if ((double) (float) this.m_waypointThresholdDistance > 0.0)
        radius = (double) (float) this.m_waypointThresholdDistance;
      else if ((bool) this.m_dockingModeEnabled || (FlightMode) this.m_currentFlightMode == FlightMode.OneWay && num1 == this.m_waypoints.Count - 1)
        radius = (double) this.CubeGrid.GridSize < 0.5 ? (double) this.CubeGrid.GridSize : (double) this.CubeGrid.GridSize * 0.25;
      if (MyFakes.ENABLE_VR_REMOTE_CONTROL_WAYPOINTS_FAST_MOVEMENT)
      {
        if (num2 < radius * radius || (this.m_previousWorldPosition - this.CurrentWaypoint.Coords).LengthSquared() < radius * radius)
          return true;
        Vector3D direction = this.WorldMatrix.Translation - this.m_previousWorldPosition;
        double num3 = direction.Normalize();
        if (num3 > 0.01)
        {
          RayD ray = new RayD(this.m_previousWorldPosition, direction);
          double? nullable = new BoundingSphereD(this.CurrentWaypoint.Coords, radius).Intersects(ray);
          return nullable.HasValue && nullable.Value <= num3;
        }
      }
      return num2 < radius * radius;
    }

    public void AdvanceWaypoint()
    {
      int index = this.m_waypoints.IndexOf(this.CurrentWaypoint);
      MyRemoteControl.MyAutopilotWaypoint oldWaypoint = this.CurrentWaypoint;
      bool enabled = (bool) this.m_autoPilotEnabled;
      if (this.m_waypoints.Count > 0)
      {
        if ((FlightMode) this.m_currentFlightMode == FlightMode.Circle)
          index = (index + 1) % this.m_waypoints.Count;
        else if ((FlightMode) this.m_currentFlightMode == FlightMode.Patrol)
        {
          if (this.m_patrolDirectionForward)
          {
            ++index;
            if (index >= this.m_waypoints.Count)
            {
              index = this.m_waypoints.Count == 1 ? 0 : this.m_waypoints.Count - 2;
              this.m_patrolDirectionForward = false;
            }
          }
          else
          {
            --index;
            if (index < 0)
            {
              index = 1;
              if (this.m_waypoints.Count == 1)
                index = 0;
              this.m_patrolDirectionForward = true;
            }
          }
        }
        else if ((FlightMode) this.m_currentFlightMode == FlightMode.OneWay)
        {
          ++index;
          if (index >= this.m_waypoints.Count)
          {
            index = 0;
            this.CubeGrid.GridSystems.GyroSystem.ControlTorque = Vector3.Zero;
            MyEntityThrustComponent entityThrustComponent = this.CubeGrid.Components.Get<MyEntityThrustComponent>();
            if (entityThrustComponent != null)
              entityThrustComponent.AutoPilotControlThrust = Vector3.Zero;
            if (Sandbox.Game.Multiplayer.Sync.IsServer && (this.m_automaticBehaviour == null || !this.m_automaticBehaviour.IsActive))
              enabled = false;
          }
        }
      }
      if (index < 0 || index >= this.m_waypoints.Count)
      {
        this.CurrentWaypoint = (MyRemoteControl.MyAutopilotWaypoint) null;
        if (Sandbox.Game.Multiplayer.Sync.IsServer && (this.m_automaticBehaviour == null || !this.m_automaticBehaviour.IsActive))
          enabled = false;
        this.UpdatePlanetWaypointInfo();
        this.RaisePropertiesChangedRemote();
        this.SetDetailedInfoDirty();
      }
      else
      {
        this.CurrentWaypoint = this.m_waypoints[index];
        if (this.CurrentWaypoint != oldWaypoint || this.m_waypoints.Count == 1)
        {
          if (Sandbox.Game.Multiplayer.Sync.IsServer && !oldWaypoint.Actions.IsNullOrEmpty<MyToolbarItem>())
            MyEntities.InvokeLater((Action) (() =>
            {
              foreach (MyToolbarItem action in oldWaypoint.Actions)
              {
                if (action != null)
                {
                  this.m_actionToolbar.SetItemAtIndex(0, action);
                  this.m_actionToolbar.UpdateItem(0);
                  this.m_actionToolbar.ActivateItemAtSlot(0, playActivationSound: false, userActivated: false);
                }
              }
              this.m_actionToolbar.Clear();
            }), "Autopilot waypoint action");
          this.UpdatePlanetWaypointInfo();
          this.RaisePropertiesChangedRemote();
          this.SetDetailedInfoDirty();
        }
      }
      if (Sandbox.Game.Multiplayer.Sync.IsServer && enabled != (bool) this.m_autoPilotEnabled)
        this.SetAutoPilotEnabled(enabled);
      this.m_stuckDetection.Reset(this.m_automaticBehaviour != null && this.m_automaticBehaviour.IsActive && this.m_automaticBehaviour.ResetStuckDetection || this.CurrentWaypoint != oldWaypoint);
      if (this.m_automaticBehaviour == null)
        return;
      this.m_automaticBehaviour.WaypointAdvanced();
    }

    private void UpdatePlanetWaypointInfo()
    {
      if (this.CurrentWaypoint == null)
        this.m_destinationInfo.Clear();
      else
        this.m_destinationInfo.Calculate(this.CurrentWaypoint.Coords);
    }

    private Vector3D GetAngleVelocity(QuaternionD q1, QuaternionD q2)
    {
      q1.Conjugate();
      QuaternionD quaternionD = q2 * q1;
      double num = 2.0 * Math.Acos(MathHelper.Clamp(quaternionD.W, -1.0, 1.0));
      if (num > Math.PI)
        num -= 2.0 * Math.PI;
      return num * new Vector3D(quaternionD.X, quaternionD.Y, quaternionD.Z) / Math.Sqrt(quaternionD.X * quaternionD.X + quaternionD.Y * quaternionD.Y + quaternionD.Z * quaternionD.Z);
    }

    private MatrixD GetOrientation() => MatrixD.CreateWorld(Vector3D.Zero, (Vector3D) Base6Directions.GetVector((Base6Directions.Direction) this.m_currentDirection), MyRemoteControl.m_upVectors[(Base6Directions.Direction) this.m_currentDirection]) * this.WorldMatrix.GetOrientation();

    private void UpdateGyro(
      Vector3D deltaPos,
      Vector3D perpDeltaPos,
      out bool rotating,
      out bool isLabile)
    {
      isLabile = false;
      rotating = true;
      MyGridGyroSystem gyroSystem = this.CubeGrid.GridSystems.GyroSystem;
      gyroSystem.ControlTorque = Vector3.Zero;
      Vector3D vector3D1;
      if (this.CurrentWaypoint != null)
      {
        vector3D1 = this.CurrentWaypoint.Coords - this.WorldMatrix.Translation;
        if (vector3D1.Length() < (double) this.CubeGrid.PositionComp.LocalVolume.Radius)
        {
          rotating = false;
          isLabile = true;
          return;
        }
      }
      Vector3D angularVelocity = (Vector3D) this.CubeGrid.Physics.AngularVelocity;
      MatrixD matrix1 = this.GetOrientation();
      MatrixD orientation = this.CubeGrid.PositionComp.WorldMatrixNormalizedInv.GetOrientation();
      Matrix matrix2 = (Matrix) ref orientation;
      Vector3D gravityWorld = this.m_currentInfo.GravityWorld;
      QuaternionD fromRotationMatrix = QuaternionD.CreateFromRotationMatrix(matrix1);
      Vector3D vector3D2;
      QuaternionD fromForwardUp;
      if (this.m_currentInfo.IsValid())
      {
        vector3D2 = perpDeltaPos;
        vector3D2.Normalize();
        fromForwardUp = QuaternionD.CreateFromForwardUp(vector3D2, -gravityWorld);
        isLabile = Vector3D.Dot(vector3D2, matrix1.Forward) > 0.95 || Math.Abs(Vector3D.Dot(Vector3D.Normalize(deltaPos), gravityWorld)) > 0.95;
      }
      else
      {
        vector3D2 = deltaPos;
        vector3D2.Normalize();
        fromForwardUp = QuaternionD.CreateFromForwardUp(vector3D2, matrix1.Up);
      }
      if (this.m_automaticBehaviour != null && this.m_automaticBehaviour.IsActive && (this.m_automaticBehaviour.RotateToTarget && this.m_automaticBehaviour.CurrentTarget != null))
      {
        isLabile = false;
        matrix1 = MatrixD.CreateWorld(Vector3D.Zero, (Vector3D) Base6Directions.GetVector(Base6Directions.Direction.Forward), MyRemoteControl.m_upVectors[Base6Directions.Direction.Forward]) * this.WorldMatrix.GetOrientation();
        fromRotationMatrix = QuaternionD.CreateFromRotationMatrix(matrix1);
        MatrixD worldMatrix = this.m_automaticBehaviour.CurrentTarget.WorldMatrix;
        Vector3D translation1 = worldMatrix.Translation;
        worldMatrix = this.WorldMatrix;
        Vector3D translation2 = worldMatrix.Translation;
        vector3D2 = translation1 - translation2;
        if (this.m_automaticBehaviour.CurrentTarget is MyCharacter)
        {
          Vector3D vector3D3 = vector3D2;
          worldMatrix = this.m_automaticBehaviour.CurrentTarget.WorldMatrix;
          Vector3D vector3D4 = worldMatrix.Up * (double) this.m_automaticBehaviour.PlayerYAxisOffset;
          vector3D2 = vector3D3 + vector3D4;
        }
        vector3D2.Normalize();
        worldMatrix = this.m_automaticBehaviour.CurrentTarget.WorldMatrix;
        Vector3D up1 = worldMatrix.Up;
        up1.Normalize();
        Vector3D up2 = Math.Abs(Vector3D.Dot(vector3D2, up1)) < 0.98 ? Vector3D.Cross(Vector3D.Cross(vector3D2, up1), vector3D2) : Vector3D.CalculatePerpendicularVector(vector3D2);
        fromForwardUp = QuaternionD.CreateFromForwardUp(vector3D2, up2);
      }
      Vector3D angleVelocity = this.GetAngleVelocity(fromRotationMatrix, fromForwardUp);
      Vector3D vector3D5 = angleVelocity * angularVelocity.Dot(ref angleVelocity);
      Vector3D vector3D6 = Vector3D.Transform(angleVelocity, matrix2);
      double num = Math.Acos(MathHelper.Clamp(Vector3D.Dot(vector3D2, matrix1.Forward), -1.0, 1.0));
      this.TargettingAimDelta = num;
      if (num < 0.03)
      {
        rotating = false;
      }
      else
      {
        rotating = rotating && !this.RotateBetweenWaypoints;
        Vector3D vector3D3 = angularVelocity - gyroSystem.GetAngularVelocity((Vector3) -vector3D6);
        vector3D1 = angularVelocity / vector3D3;
        double d1 = vector3D1.Max();
        double d2 = num / vector3D5.Length() * num;
        if (double.IsNaN(d1) || double.IsInfinity(d2) || d2 > d1)
        {
          if ((bool) this.m_dockingModeEnabled)
            vector3D6 /= 4.0;
          gyroSystem.ControlTorque = (Vector3) vector3D6;
          gyroSystem.MarkDirty();
        }
        else if (num < 0.1 && this.m_automaticBehaviour != null && (this.m_automaticBehaviour.RotateToTarget && this.m_automaticBehaviour.CurrentTarget != null))
        {
          gyroSystem.ControlTorque = (Vector3) (vector3D6 / 3.0);
          gyroSystem.MarkDirty();
        }
        if ((bool) this.m_dockingModeEnabled)
          ;
      }
    }

    private void CalculateDeltaPos(
      MyEntityThrustComponent thrust,
      out Vector3D deltaPos,
      out Vector3D perpDeltaPos,
      out Vector3D targetDelta,
      out float autopilotSpeedLimit)
    {
      autopilotSpeedLimit = this.m_autopilotSpeedLimit.Value;
      this.m_currentInfo.Calculate(this.WorldMatrix.Translation);
      Vector3D coords = this.CurrentWaypoint.Coords;
      Vector3D translation = this.WorldMatrix.Translation;
      targetDelta = coords - translation;
      if ((bool) this.m_useCollisionAvoidance)
      {
        if (MyFakes.ENABLE_NEW_COLLISION_AVOIDANCE)
        {
          deltaPos = this.AvoidCollisionsVs2(thrust, targetDelta, ref autopilotSpeedLimit);
          targetDelta = deltaPos;
        }
        else
          deltaPos = this.AvoidCollisions(targetDelta, ref autopilotSpeedLimit);
      }
      else
        deltaPos = targetDelta;
      perpDeltaPos = Vector3D.Reject(targetDelta, this.m_currentInfo.GravityWorld);
    }

    private void FillListOfDetectedObjects(
      Vector3D pos,
      MyEntity parentEntity,
      ref int listLimit,
      ref Vector3D shipFront,
      ref float closestEntityDist,
      ref MyEntity closestEntity)
    {
      float dist = Vector3.DistanceSquared((Vector3) pos, (Vector3) shipFront);
      if ((double) dist < (double) closestEntityDist)
      {
        closestEntityDist = dist;
        closestEntity = parentEntity;
      }
      if (this.m_detectedObstacles.Count == 0)
      {
        this.m_detectedObstacles.Add(new MyRemoteControl.DetectedObject(dist, pos, parentEntity is MyVoxelBase));
      }
      else
      {
        for (int index = 0; index < this.m_detectedObstacles.Count; ++index)
        {
          if ((double) dist < (double) this.m_detectedObstacles[index].Distance)
          {
            if (this.m_detectedObstacles.Count == listLimit)
              this.m_detectedObstacles.RemoveAt(listLimit - 1);
            this.m_detectedObstacles.AddOrInsert<MyRemoteControl.DetectedObject>(new MyRemoteControl.DetectedObject(dist, pos, parentEntity is MyVoxelBase), index);
            break;
          }
        }
      }
      if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
        return;
      MyRenderProxy.DebugDrawSphere(pos, 1.5f, Color.Red, depthRead: false);
    }

    private Vector3D AvoidCollisionsVs2(
      MyEntityThrustComponent thrust,
      Vector3D delta,
      ref float autopilotSpeedLimit)
    {
      if (this.m_collisionAvoidanceFrameSkip > 0)
      {
        --this.m_collisionAvoidanceFrameSkip;
        autopilotSpeedLimit = this.m_lastAutopilotSpeedLimit;
        return this.m_lastDelta;
      }
      this.m_collisionAvoidanceFrameSkip = 19;
      if (thrust == null)
        return delta;
      bool enableDebugDraw = MyDebugDrawSettings.ENABLE_DEBUG_DRAW;
      int listLimit = 5;
      bool flag1 = true;
      Vector3D vector3D1 = delta;
      float currentMass = (float) this.CubeGrid.GetCurrentMass();
      Vector3 linearVelocity = this.CubeGrid.Physics.LinearVelocity;
      float num1 = Math.Max(linearVelocity.Length(), 1f);
      float num2 = (double) num1 <= 1.0 ? 1f : 1.25f;
      double num3 = (double) thrust.GetMaxThrustInDirection(this.m_currentDirection.Value) / (double) currentMass;
      double num4 = Math.Min((double) num1 / num3 * (double) num1 / 2.0, MyRemoteControl.MAX_STOPPING_DISTANCE);
      float radius = this.CubeGrid.PositionComp.LocalVolume.Radius;
      Vector3D vector3D2 = Vector3D.One;
      if (!Vector3D.IsZero((Vector3D) linearVelocity))
        vector3D2 = Vector3D.Normalize((Vector3D) linearVelocity);
      Vector3D vector3D3 = vector3D2 * num4;
      Vector3D center1 = this.CubeGrid.PositionComp.WorldAABB.Center;
      Vector3D shipFront = (double) radius * vector3D2 + center1;
      Vector3D pointFrom = shipFront + vector3D2 * num4;
      MatrixD worldMatrix1 = this.CubeGrid.WorldMatrix;
      Vector3D vector3D4 = Vector3D.Reject(worldMatrix1.Up, vector3D2);
      bool flag2 = true;
      if (vector3D4.LengthSquared() < 0.25)
      {
        vector3D4 = Vector3D.Reject(worldMatrix1.Right, vector3D2);
        flag2 = false;
      }
      Quaternion fromForwardUp = Quaternion.CreateFromForwardUp((Vector3) vector3D2, (Vector3) vector3D4);
      fromForwardUp.Normalize();
      Vector3D vector3D5 = Vector3D.Reject((Vector3D) this.CubeGrid.PositionComp.LocalAABB.HalfExtents, Vector3D.TransformNormal(vector3D2, this.CubeGrid.PositionComp.WorldMatrixInvScaled));
      double x = Math.Abs(vector3D5.X) + Math.Abs(vector3D5.Z);
      double y = Math.Abs(vector3D5.Y) + Math.Abs(vector3D5.Z);
      if (!flag2)
      {
        x = Math.Abs(vector3D5.Y) + Math.Abs(vector3D5.Z);
        y = Math.Abs(vector3D5.Y) + Math.Abs(vector3D5.X);
      }
      Vector3D center2 = center1 + ((double) radius + (double) num1) * vector3D2;
      Vector3D vector3D6 = new Vector3D(x, y, (double) radius + (double) num1);
      MyOrientedBoundingBoxD obb = new MyOrientedBoundingBoxD(center2, vector3D6, fromForwardUp);
      MatrixD boxTransform = MatrixD.CreateFromQuaternion(fromForwardUp);
      boxTransform.Translation = center2;
      BoundingBoxD box = new BoundingBoxD(-vector3D6, vector3D6);
      if (enableDebugDraw)
        MyRenderProxy.DebugDrawOBB(obb, Color.Red, 0.25f, false, false);
      BoundingSphereD sphere = new BoundingSphereD(shipFront + vector3D3 / 2.0, num4 / 2.0);
      List<MyEntity> result1 = this.m_collisionEntityList ?? (this.m_collisionEntityList = new List<MyEntity>());
      List<MySlimBlock> blocks = this.m_collisionBlockList ?? (this.m_collisionBlockList = new List<MySlimBlock>());
      MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref sphere, result1);
      MyEntity closestEntity = (MyEntity) null;
      float maxValue = float.MaxValue;
      bool flag3 = false;
      foreach (MyEntity parentEntity in result1)
      {
        if (parentEntity.Physics == null || parentEntity.Physics.IsStatic || (double) parentEntity.PositionComp.LocalVolume.Radius >= (double) radius * 0.0500000007450581)
        {
          if (parentEntity is MyCubeGrid Node)
          {
            if (MyCubeGridGroups.Static.Physical.GetGroup(this.CubeGrid) != MyCubeGridGroups.Static.Physical.GetGroup(Node))
            {
              MatrixD worldMatrix2 = Node.WorldMatrix;
              Node.GetBlocksIntersectingOBB(in box, in boxTransform, blocks);
              foreach (MySlimBlock mySlimBlock in blocks)
              {
                Matrix localMatrix;
                mySlimBlock.GetLocalMatrix(out localMatrix);
                Vector3 translation = localMatrix.Translation;
                Vector3D result2;
                Vector3D.Transform(ref translation, ref worldMatrix2, out result2);
                this.FillListOfDetectedObjects(result2, (MyEntity) Node, ref listLimit, ref shipFront, ref maxValue, ref closestEntity);
              }
              blocks.Clear();
            }
            else
              continue;
          }
          if (parentEntity is MyCharacter & flag1)
          {
            MatrixD worldMatrix2 = parentEntity.WorldMatrix;
            Vector3D translation = worldMatrix2.Translation;
            if (obb.Contains(ref translation))
            {
              worldMatrix2 = parentEntity.WorldMatrix;
              this.FillListOfDetectedObjects(worldMatrix2.Translation, parentEntity, ref listLimit, ref shipFront, ref maxValue, ref closestEntity);
            }
          }
          if (parentEntity is MyPlanet myPlanet)
          {
            float num5 = myPlanet.MaximumRadius + radius;
            if (Vector3D.DistanceSquared(myPlanet.PositionComp.GetPosition(), center1) < (double) num5 * (double) num5)
              flag3 = true;
          }
          else if (parentEntity is MyVoxelBase)
            flag3 = true;
        }
      }
      this.m_collisionEntityList.Clear();
      bool flag4 = false;
      if (flag3)
      {
        Vector3D[] corners = new Vector3D[8];
        obb.GetCorners(corners, 0);
        for (int index = -1; index < 4; ++index)
        {
          Vector3D vector3D7 = index >= 0 ? corners[index + 4] : pointFrom;
          MyPhysics.HitInfo? nullable = MyPhysics.CastRay(shipFront + vector3D2 * 0.1, vector3D7);
          if (nullable.HasValue)
          {
            if (!(nullable.Value.HkHitInfo.Body.UserObject is MyEntity parentEntity) && nullable.Value.HkHitInfo.Body.UserObject is MyVoxelPhysicsBody userObject)
              parentEntity = userObject.Entity as MyEntity;
            this.FillListOfDetectedObjects(nullable.Value.Position, parentEntity, ref listLimit, ref shipFront, ref maxValue, ref closestEntity);
            if (index == -1)
              flag4 = true;
          }
          if (enableDebugDraw)
            MyRenderProxy.DebugDrawLine3D(shipFront, vector3D7, Color.Pink, Color.White, false);
        }
      }
      if ((double) maxValue < 3.40282346638529E+38)
      {
        this.m_rotateFor = 3f;
        int num5 = 0;
        Vector3D zero = Vector3D.Zero;
        bool flag5 = false;
        if (!flag4)
        {
          for (int index = 0; index < this.m_detectedObstacles.Count; ++index)
          {
            if (index == 4)
            {
              flag5 = true;
              break;
            }
            if (!this.m_detectedObstacles[index].IsVoxel)
              break;
          }
        }
        Vector3D vector3D7;
        if (flag5)
        {
          vector3D7 = pointFrom;
        }
        else
        {
          for (int index = 0; index < this.m_detectedObstacles.Count; ++index)
          {
            if (index == 0 || (double) this.m_detectedObstacles[index].Distance - (double) this.m_detectedObstacles[0].Distance < 225.0)
            {
              ++num5;
              Vector3D vector2 = this.m_detectedObstacles[index].Position - worldMatrix1.Translation;
              Vector3D vector3D8 = Vector3D.Cross(Vector3D.Cross(delta, vector2), delta);
              Vector3D pointTo = this.m_detectedObstacles[index].Position - Vector3D.Normalize(vector3D8) * (double) radius * (double) num2 * 2.0;
              if (enableDebugDraw)
                MyRenderProxy.DebugDrawLine3D(worldMatrix1.Translation, pointTo, Color.White, Color.Tomato, false);
              zero += pointTo;
            }
          }
          vector3D7 = zero / (double) num5;
        }
        autopilotSpeedLimit = (float) (1.0 + (double) autopilotSpeedLimit * Math.Sqrt((double) maxValue) / num4 * 0.5);
        if (num5 < 5 | flag5)
          delta = vector3D7 - worldMatrix1.Translation;
        else if (closestEntity != null)
        {
          Vector3D center3 = closestEntity.PositionComp.WorldAABB.Center;
          Vector3D vector2 = center3 - this.WorldMatrix.Translation;
          if (closestEntity.Physics != null && !closestEntity.Physics.IsStatic && this.m_waitForFreeWay.Value)
          {
            vector2.Normalize();
            delta = -vector2 * (double) radius;
          }
          else
          {
            Vector3D vector3D8 = Vector3D.Cross(Vector3D.Cross(delta, vector2), delta);
            delta = center3 - Vector3D.Normalize(vector3D8) * (double) radius * (double) num2 * 2.0 - center3;
            delta *= 2.0;
          }
          autopilotSpeedLimit *= 0.75f;
        }
        Vector3D vector1 = Vector3D.Normalize(delta);
        if (Vector3D.Dot(vector1, Vector3D.Normalize(this.m_detectedObstacles[0].Position - this.WorldMatrix.Translation)) > 0.5)
          delta *= -1.0;
        else if (Vector3D.Dot(vector1, Vector3D.Normalize(vector3D1)) < -0.5)
          delta = vector3D1;
        if (enableDebugDraw)
          MyRenderProxy.DebugDrawLine3D(worldMatrix1.Translation, worldMatrix1.Translation + delta, Color.Red, Color.Aquamarine, false);
      }
      else if (enableDebugDraw && (double) this.m_rotateFor <= 0.0)
        MyRenderProxy.DebugDrawLine3D(pointFrom, pointFrom + vector3D3, Color.Yellow, Color.Green, false);
      this.m_detectedObstacles.Clear();
      if ((double) maxValue == 3.40282346638529E+38 && (double) this.m_rotateFor > 1.5)
      {
        autopilotSpeedLimit = this.m_lastAutopilotSpeedLimit;
        return this.m_lastDelta;
      }
      this.m_lastAutopilotSpeedLimit = autopilotSpeedLimit;
      this.m_lastDelta = delta;
      return delta;
    }

    private Vector3D AvoidCollisions(Vector3D delta, ref float autopilotSpeedLimit)
    {
      if (this.m_collisionCtr <= 0)
      {
        this.m_collisionCtr = 0;
        bool enableDebugDraw = MyDebugDrawSettings.ENABLE_DEBUG_DRAW;
        Vector3D centerOfMassWorld = this.CubeGrid.Physics.CenterOfMassWorld;
        double num1 = this.CubeGrid.PositionComp.WorldVolume.Radius * 1.29999995231628;
        if (MyFakes.ENABLE_VR_DRONE_COLLISIONS)
          num1 = this.CubeGrid.PositionComp.WorldVolume.Radius * 1.0;
        Vector3D linearVelocity = (Vector3D) this.CubeGrid.Physics.LinearVelocity;
        double num2 = linearVelocity.Length();
        double radius = this.CubeGrid.PositionComp.WorldVolume.Radius * 10.0 + num2 * num2 * 0.05;
        if (MyFakes.ENABLE_VR_DRONE_COLLISIONS)
          radius = this.CubeGrid.PositionComp.WorldVolume.Radius + num2 * num2 * 0.05;
        BoundingSphereD boundingSphere = new BoundingSphereD(centerOfMassWorld, radius);
        Vector3D point = boundingSphere.Center + linearVelocity * 2.0;
        if (MyFakes.ENABLE_VR_DRONE_COLLISIONS)
          point = boundingSphere.Center + linearVelocity;
        if (enableDebugDraw)
        {
          MyRenderProxy.DebugDrawSphere(boundingSphere.Center, (float) num1, Color.HotPink, depthRead: false);
          MyRenderProxy.DebugDrawSphere(boundingSphere.Center + linearVelocity, 1f, Color.HotPink, depthRead: false);
          MyRenderProxy.DebugDrawSphere(boundingSphere.Center, (float) radius, Color.White, depthRead: false);
        }
        Vector3D zero1 = Vector3D.Zero;
        Vector3D zero2 = Vector3D.Zero;
        int num3 = 0;
        double val1 = 0.0;
        List<MyEntity> entitiesInSphere = MyEntities.GetTopMostEntitiesInSphere(ref boundingSphere);
        IMyGravityProvider nearestProvider;
        if (MyGravityProviderSystem.GetStrongestNaturalGravityWell(centerOfMassWorld, out nearestProvider) > 0.0 && nearestProvider is MyGravityProviderComponent)
        {
          MyEntity entity = (MyEntity) ((MyEntityComponentBase) nearestProvider).Entity;
          if (!entitiesInSphere.Contains(entity))
            entitiesInSphere.Add(entity);
        }
        for (int index = 0; index < entitiesInSphere.Count; ++index)
        {
          MyEntity myEntity = entitiesInSphere[index];
          if (myEntity != this.Parent)
          {
            Vector3D vector3D1 = Vector3D.Zero;
            Vector3D zero3 = Vector3D.Zero;
            Vector3D vector3D2;
            Vector3D vector3D3;
            switch (myEntity)
            {
              case MyCubeGrid _:
              case MyVoxelMap _:
              case MySkinnedEntity _:
                if (!MyFakes.ENABLE_VR_DRONE_COLLISIONS || !(myEntity is MyCubeGrid) || !(myEntity as MyCubeGrid).IsStatic)
                {
                  if (myEntity is MyCubeGrid)
                  {
                    MyCubeGrid Node = myEntity as MyCubeGrid;
                    if (MyCubeGridGroups.Static.Physical.GetGroup(this.CubeGrid) == MyCubeGridGroups.Static.Physical.GetGroup(Node))
                      continue;
                  }
                  BoundingSphereD worldVolume = myEntity.PositionComp.WorldVolume;
                  worldVolume.Radius += num1;
                  Vector3D vector1 = worldVolume.Center - boundingSphere.Center;
                  if ((double) this.CubeGrid.Physics.LinearVelocity.LengthSquared() <= 5.0 || Vector3D.Dot(delta, this.CubeGrid.Physics.LinearVelocity) >= 0.0)
                  {
                    double num4 = vector1.Length();
                    BoundingSphereD boundingSphereD = new BoundingSphereD(worldVolume.Center + linearVelocity, worldVolume.Radius + num2);
                    if (boundingSphereD.Contains(point) == ContainmentType.Contains)
                    {
                      autopilotSpeedLimit = 2f;
                      if (enableDebugDraw)
                        MyRenderProxy.DebugDrawSphere(boundingSphereD.Center, (float) boundingSphereD.Radius, Color.Red, depthRead: false);
                    }
                    else if (Vector3D.Dot(vector1, linearVelocity) < 0.0)
                    {
                      if (enableDebugDraw)
                        MyRenderProxy.DebugDrawSphere(boundingSphereD.Center, (float) boundingSphereD.Radius, Color.Red, depthRead: false);
                    }
                    else if (enableDebugDraw)
                      MyRenderProxy.DebugDrawSphere(boundingSphereD.Center, (float) boundingSphereD.Radius, Color.DarkOrange, depthRead: false);
                    double num5 = 2.0 * Math.Exp(-0.693 * num4 / (worldVolume.Radius + this.CubeGrid.PositionComp.WorldVolume.Radius + num2));
                    vector3D2 = boundingSphereD.Center - boundingSphere.Center;
                    double val2 = Math.Min(1.0, Math.Max(0.0, -vector3D2.Length() / boundingSphereD.Radius + 2.0));
                    val1 = Math.Max(val1, val2);
                    Vector3D vector3D4 = vector1 / num4;
                    vector3D1 = -vector3D4 * num5;
                    vector3D3 = -vector3D4 * val2;
                    break;
                  }
                  continue;
                }
                continue;
              case MyPlanet _:
                MyPlanet myPlanet = myEntity as MyPlanet;
                float gravityLimit = ((MySphericalNaturalGravityComponent) myPlanet.Components.Get<MyGravityProviderComponent>()).GravityLimit;
                Vector3D translation = myPlanet.WorldMatrix.Translation;
                Vector3D vector3D5 = translation - centerOfMassWorld;
                double num6 = vector3D5.Length() - (double) gravityLimit;
                if (num6 <= MyRemoteControl.PLANET_AVOIDANCE_RADIUS && num6 >= -MyRemoteControl.PLANET_AVOIDANCE_TOLERANCE)
                {
                  Vector3D vector3D4 = translation - this.m_currentWaypoint.Coords;
                  if (Vector3D.IsZero(vector3D4))
                    vector3D4 = (Vector3D) Vector3.Up;
                  else
                    vector3D4.Normalize();
                  Vector3D vector3D6 = translation + vector3D4 * (double) gravityLimit;
                  Vector3D vector3D7 = vector3D5;
                  vector3D7.Normalize();
                  vector3D2 = vector3D6 - centerOfMassWorld;
                  double d = vector3D2.LengthSquared();
                  if (d < MyRemoteControl.PLANET_REPULSION_RADIUS * MyRemoteControl.PLANET_REPULSION_RADIUS)
                  {
                    double amount = Math.Sqrt(d) / MyRemoteControl.PLANET_REPULSION_RADIUS;
                    Vector3D vector = centerOfMassWorld - vector3D6;
                    Vector3D vector3D8;
                    if (Vector3D.IsZero(vector))
                    {
                      vector3D8 = Vector3D.CalculatePerpendicularVector(vector3D4);
                    }
                    else
                    {
                      vector3D8 = Vector3D.Reject(vector, vector3D4);
                      vector3D8.Normalize();
                    }
                    vector3D1 = Vector3D.Lerp(vector3D4, vector3D8, amount);
                  }
                  else
                  {
                    Vector3D vector3D8 = this.m_currentWaypoint.Coords - centerOfMassWorld;
                    vector3D8.Normalize();
                    if (Vector3D.Dot(vector3D8, vector3D7) > 0.0)
                    {
                      vector3D1 = Vector3D.Reject(vector3D8, vector3D7);
                      if (Vector3D.IsZero(vector3D1))
                        vector3D1 = Vector3D.CalculatePerpendicularVector(vector3D7);
                      else
                        vector3D1.Normalize();
                    }
                  }
                  vector3D2 = point - translation;
                  double num4 = vector3D2.Length();
                  if (num4 < (double) gravityLimit)
                    this.m_autopilotSpeedLimit.Value = 2f;
                  double num5 = ((double) gravityLimit + MyRemoteControl.PLANET_AVOIDANCE_RADIUS - num4) / MyRemoteControl.PLANET_AVOIDANCE_RADIUS;
                  vector3D1 *= num5;
                  vector3D3 = -vector3D7 * num5;
                  break;
                }
                continue;
              default:
                continue;
            }
            zero1 += vector3D1;
            zero2 += vector3D3;
            ++num3;
          }
        }
        entitiesInSphere.Clear();
        if (num3 > 0)
        {
          double num4 = delta.Length();
          delta /= num4;
          Vector3D vector3D1 = zero1 * ((1.0 - val1) * 0.100000001490116 / (double) num3);
          Vector3D vector3D2 = vector3D1 + delta;
          delta += vector3D1 + zero2;
          delta *= num4;
          if (enableDebugDraw)
          {
            MyRenderProxy.DebugDrawArrow3D(centerOfMassWorld, centerOfMassWorld + delta / num4 * 100.0, Color.Green, new Color?(Color.Green));
            MyRenderProxy.DebugDrawArrow3D(centerOfMassWorld, centerOfMassWorld + zero2 * 100.0, Color.Red, new Color?(Color.Red));
            MyRenderProxy.DebugDrawSphere(centerOfMassWorld, 100f, Color.Gray, 0.5f, false);
          }
        }
        this.m_oldCollisionDelta = delta;
        return delta;
      }
      --this.m_collisionCtr;
      return this.m_oldCollisionDelta;
    }

    private void UpdateThrust(
      MyEntityThrustComponent thrustSystem,
      Vector3D delta,
      Vector3D perpDelta,
      double maxSpeed)
    {
      if (thrustSystem == null)
        return;
      thrustSystem.AutoPilotControlThrust = Vector3.Zero;
      if (!thrustSystem.Enabled)
        return;
      MatrixD matrixD = this.CubeGrid.PositionComp.WorldMatrixNormalizedInv;
      matrixD = matrixD.GetOrientation();
      Matrix matrix = (Matrix) ref matrixD;
      Vector3D v1 = delta;
      v1.Normalize();
      Vector3D linearVelocity = (Vector3D) this.CubeGrid.Physics.LinearVelocity;
      Vector3 vector3_1 = Vector3.Transform((Vector3) v1, matrix);
      int num1 = MyDebugDrawSettings.ENABLE_DEBUG_DRAW ? 1 : 0;
      thrustSystem.AutoPilotControlThrust = Vector3.Zero;
      Vector3 zero = Vector3.Zero;
      zero.X = thrustSystem.GetMaxThrustInDirection((double) vector3_1.X > 0.0 ? Base6Directions.Direction.Left : Base6Directions.Direction.Right);
      zero.Y = thrustSystem.GetMaxThrustInDirection((double) vector3_1.Y > 0.0 ? Base6Directions.Direction.Down : Base6Directions.Direction.Up);
      zero.Z = thrustSystem.GetMaxThrustInDirection((double) vector3_1.Z > 0.0 ? Base6Directions.Direction.Backward : Base6Directions.Direction.Forward);
      int num2 = MyDebugDrawSettings.ENABLE_DEBUG_DRAW ? 1 : 0;
      float num3 = (float) ((double) Math.Abs(vector3_1.X) * (double) zero.X + (double) Math.Abs(vector3_1.Y) * (double) zero.Y + (double) Math.Abs(vector3_1.Z) * (double) zero.Z);
      if (linearVelocity.Length() > 3.0 && linearVelocity.Dot(ref v1) < 0.0)
        return;
      Vector3D perpendicularVector = Vector3D.CalculatePerpendicularVector(v1);
      Vector3D v2 = Vector3D.Cross(v1, perpendicularVector);
      Vector3D vector3D = v1 * linearVelocity.Dot(ref v1);
      Vector3D position = perpendicularVector * linearVelocity.Dot(ref perpendicularVector) + v2 * linearVelocity.Dot(ref v2);
      double d1 = delta.Length() / vector3D.Length();
      double d2 = linearVelocity.Length() * (double) this.CubeGrid.Physics.Mass / (double) num3;
      if ((bool) this.m_dockingModeEnabled)
        d2 *= 2.5;
      int num4 = MyDebugDrawSettings.ENABLE_DEBUG_DRAW ? 1 : 0;
      if (!double.IsInfinity(d1) && !double.IsInfinity(d2) && (!double.IsNaN(d2) && d1 <= d2) || linearVelocity.LengthSquared() >= maxSpeed * maxSpeed)
        return;
      Vector3 vector3_2 = (Vector3) (Vector3D.Transform(delta, matrix) - Vector3D.Transform(position, matrix));
      double num5 = (double) vector3_2.Normalize();
      thrustSystem.AutoPilotControlThrust = vector3_2;
    }

    private void ResetShipControls()
    {
      MyEntityThrustComponent entityThrustComponent = this.CubeGrid.Components.Get<MyEntityThrustComponent>();
      if (entityThrustComponent == null || !entityThrustComponent.Enabled)
        return;
      entityThrustComponent.DampenersEnabled = true;
    }

    bool Sandbox.ModAPI.Ingame.IMyRemoteControl.GetNearestPlayer(
      out Vector3D playerPosition)
    {
      playerPosition = new Vector3D();
      if (!MySession.Static.Players.IdentityIsNpc(this.OwnerId))
        return false;
      MyPlayer nearestPlayer = this.GetNearestPlayer();
      if (nearestPlayer == null)
        return false;
      playerPosition = nearestPlayer.Controller.ControlledEntity.Entity.WorldMatrix.Translation;
      return true;
    }

    bool Sandbox.ModAPI.IMyRemoteControl.GetNearestPlayer(
      out Vector3D playerPosition)
    {
      playerPosition = new Vector3D();
      MyPlayer nearestPlayer = this.GetNearestPlayer();
      if (nearestPlayer == null)
        return false;
      playerPosition = nearestPlayer.Controller.ControlledEntity.Entity.WorldMatrix.Translation;
      return true;
    }

    public bool GetNearestPlayer(out MatrixD playerWorldTransform, Vector3 offset)
    {
      playerWorldTransform = MatrixD.Identity;
      if (!MySession.Static.Players.IdentityIsNpc(this.OwnerId))
        return false;
      MyPlayer nearestPlayer = this.GetNearestPlayer();
      if (nearestPlayer == null)
        return false;
      playerWorldTransform = nearestPlayer.Controller.ControlledEntity.Entity.WorldMatrix;
      Vector3 vector3 = Vector3.TransformNormal(offset, playerWorldTransform);
      playerWorldTransform.Translation += vector3;
      return true;
    }

    public new Vector3D GetNaturalGravity() => (Vector3D) MyGravityProviderSystem.CalculateNaturalGravityInPoint(this.WorldMatrix.Translation);

    public MyPlayer GetNearestPlayer()
    {
      Vector3D translation1 = this.WorldMatrix.Translation;
      double num1 = double.MaxValue;
      MyPlayer myPlayer = (MyPlayer) null;
      foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) MySession.Static.Players.GetOnlinePlayers())
      {
        IMyControllableEntity controlledEntity = onlinePlayer.Controller.ControlledEntity;
        if (controlledEntity != null)
        {
          Vector3D translation2 = controlledEntity.Entity.WorldMatrix.Translation;
          double num2 = Vector3D.DistanceSquared(translation1, translation2);
          if (num2 < num1)
          {
            num1 = num2;
            myPlayer = onlinePlayer;
          }
        }
      }
      return myPlayer;
    }

    Vector3D Sandbox.ModAPI.IMyRemoteControl.GetFreeDestination(
      Vector3D originalDestination,
      float checkRadius,
      float shipRadius)
    {
      return this.GetFreeDestination(originalDestination, checkRadius, shipRadius);
    }

    private Vector3D GetFreeDestination(
      Vector3D originalDestination,
      float checkRadius,
      float shipRadius)
    {
      MyCestmirDebugInputComponent.ClearDebugSpheres();
      MyCestmirDebugInputComponent.ClearDebugPoints();
      MyCestmirDebugInputComponent.AddDebugPoint(this.WorldMatrix.Translation, Color.Green);
      if ((double) shipRadius == 0.0)
        shipRadius = (float) this.CubeGrid.PositionComp.WorldVolume.Radius;
      BoundingSphereD boundingSphere = new BoundingSphereD(this.WorldMatrix.Translation, (double) shipRadius + (double) checkRadius);
      Vector3D direction1 = originalDestination - this.WorldMatrix.Translation;
      double val2 = direction1.Length();
      direction1 /= val2;
      RayD rayD = new RayD(this.WorldMatrix.Translation, direction1);
      double maxValue = double.MaxValue;
      BoundingSphereD boundingSphereD = new BoundingSphereD();
      List<MyEntity> entitiesInSphere = MyEntities.GetTopMostEntitiesInSphere(ref boundingSphere);
      for (int index = 0; index < entitiesInSphere.Count; ++index)
      {
        MyEntity myEntity = entitiesInSphere[index];
        switch (myEntity)
        {
          case MyCubeGrid _:
          case MyVoxelMap _:
            if (myEntity.Parent == null && myEntity != this.Parent)
            {
              BoundingSphereD worldVolume = myEntity.PositionComp.WorldVolume;
              worldVolume.Radius += (double) shipRadius;
              MyCestmirDebugInputComponent.AddDebugSphere(worldVolume.Center, (float) myEntity.PositionComp.WorldVolume.Radius, Color.Plum);
              MyCestmirDebugInputComponent.AddDebugSphere(worldVolume.Center, (float) myEntity.PositionComp.WorldVolume.Radius + shipRadius, Color.Purple);
              double? nullable = rayD.Intersects(worldVolume);
              if (nullable.HasValue && nullable.Value < maxValue)
              {
                maxValue = nullable.Value;
                boundingSphereD = worldVolume;
                break;
              }
              break;
            }
            break;
        }
      }
      Vector3D vector3D1;
      if (maxValue != double.MaxValue)
      {
        Vector3D position = rayD.Position + maxValue * rayD.Direction;
        MyCestmirDebugInputComponent.AddDebugSphere(position, 1f, Color.Blue);
        Vector3D direction2 = position - boundingSphereD.Center;
        if (Vector3D.IsZero(direction2))
          direction2 = Vector3D.Up;
        direction2.Normalize();
        MyCestmirDebugInputComponent.AddDebugSphere(position + direction2, 1f, Color.Red);
        Vector3D vector3D2 = Vector3D.Reject(rayD.Direction, direction2);
        vector3D2.Normalize();
        Vector3D vector3D3 = vector3D2 * Math.Max(20.0, boundingSphereD.Radius * 0.5);
        MyCestmirDebugInputComponent.AddDebugSphere(position + vector3D3, 1f, Color.LightBlue);
        vector3D1 = position + vector3D3;
      }
      else
        vector3D1 = rayD.Position + rayD.Direction * Math.Min((double) checkRadius, val2);
      entitiesInSphere.Clear();
      return vector3D1;
    }

    private bool TryFindSavedEntity()
    {
      MyEntity entity;
      if (this.m_savedPreviousControlledEntityId.HasValue && MyEntities.TryGetEntityById(this.m_savedPreviousControlledEntityId.Value, out entity))
      {
        this.m_previousControlledEntity = (IMyControllableEntity) entity;
        if (this.m_previousControlledEntity != null)
        {
          this.AddPreviousControllerEvents();
          if (this.m_previousControlledEntity is MyCockpit)
            this.cockpitPilot = (this.m_previousControlledEntity as MyCockpit).Pilot;
          return true;
        }
      }
      return false;
    }

    public bool WasControllingCockpitWhenSaved()
    {
      MyEntity entity;
      return this.m_savedPreviousControlledEntityId.HasValue && MyEntities.TryGetEntityById(this.m_savedPreviousControlledEntityId.Value, out entity) && entity is MyCockpit;
    }

    private void AddPreviousControllerEvents()
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.m_previousControlledEntity.Entity.OnMarkForClose += new Action<MyEntity>(this.Entity_OnPreviousMarkForClose);
      if (!(this.m_previousControlledEntity.Entity is MyTerminalBlock entity))
        return;
      entity.IsWorkingChanged += new Action<MyCubeBlock>(this.PreviousCubeBlock_IsWorkingChanged);
      if (!(this.m_previousControlledEntity.Entity is MyCockpit entity) || entity.Pilot == null)
        return;
      entity.Pilot.OnMarkForClose += new Action<MyEntity>(this.Entity_OnPreviousMarkForClose);
    }

    private void PreviousCubeBlock_IsWorkingChanged(MyCubeBlock obj)
    {
      if (obj.IsWorking || obj.Closed || obj.MarkedForClose)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyRemoteControl, bool>(this, (Func<MyRemoteControl, Action<bool>>) (x => new Action<bool>(x.RequestRelease)), false);
    }

    private void Entity_OnPreviousMarkForClose(MyEntity obj) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyRemoteControl, bool>(this, (Func<MyRemoteControl, Action<bool>>) (x => new Action<bool>(x.RequestRelease)), true);

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_RemoteControl builderCubeBlock = (MyObjectBuilder_RemoteControl) base.GetObjectBuilderCubeBlock(copy);
      if (this.m_previousControlledEntity != null)
        builderCubeBlock.PreviousControlledEntityId = new long?(this.m_previousControlledEntity.Entity.EntityId);
      builderCubeBlock.AutoPilotEnabled = (bool) this.m_autoPilotEnabled;
      builderCubeBlock.DockingModeEnabled = (bool) this.m_dockingModeEnabled;
      builderCubeBlock.FlightMode = (int) this.m_currentFlightMode.Value;
      builderCubeBlock.Direction = (byte) this.m_currentDirection.Value;
      builderCubeBlock.WaitForFreeWay = this.m_waitForFreeWay.Value;
      builderCubeBlock.AutopilotSpeedLimit = (float) this.m_autopilotSpeedLimit;
      builderCubeBlock.WaypointThresholdDistance = (float) this.m_waypointThresholdDistance;
      builderCubeBlock.BindedCamera = this.m_bindedCamera.Value;
      builderCubeBlock.IsMainRemoteControl = (bool) this.m_isMainRemoteControl;
      builderCubeBlock.Waypoints = new List<MyObjectBuilder_AutopilotWaypoint>(this.m_waypoints.Count);
      foreach (MyRemoteControl.MyAutopilotWaypoint waypoint in this.m_waypoints)
        builderCubeBlock.Waypoints.Add(waypoint.GetObjectBuilder());
      builderCubeBlock.CurrentWaypointIndex = this.CurrentWaypoint == null ? -1 : this.m_waypoints.IndexOf(this.CurrentWaypoint);
      builderCubeBlock.CollisionAvoidance = (bool) this.m_useCollisionAvoidance;
      builderCubeBlock.AutomaticBehaviour = this.m_automaticBehaviour != null ? this.m_automaticBehaviour.GetObjectBuilder() : (MyObjectBuilder_AutomaticBehaviour) null;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public bool CanControl(IMyControllableEntity controllingEntity)
    {
      if (!this.CheckPreviousEntity(controllingEntity) || (bool) this.m_autoPilotEnabled)
        return false;
      this.UpdateIsWorking();
      return this.IsWorking && this.PreviousControlledEntity == null && this.CheckRangeAndAccess(controllingEntity, controllingEntity.ControllerInfo.Controller.Player);
    }

    protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
    {
      base.UpdateDetailedInfo(detailedInfo);
      detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
      detailedInfo.Append(this.BlockDefinition.DisplayNameText);
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
      MyValueFormatter.AppendWorkInBestUnit(this.BlockDefinition.RequiredPowerInput, detailedInfo);
      detailedInfo.Append("\n");
      if (this.m_previousControlledEntity is MyCharacter controlledEntity && controlledEntity != MySession.Static.LocalCharacter)
      {
        detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.RemoteControlUsedBy));
        detailedInfo.Append(controlledEntity.DisplayNameText);
        detailedInfo.Append("\n");
      }
      if (!(bool) this.m_autoPilotEnabled || this.CurrentWaypoint == null)
        return;
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.RemoteControlWaypoint));
      detailedInfo.Append(this.CurrentWaypoint.Name);
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.RemoteControlCoord));
      detailedInfo.Append((object) this.CurrentWaypoint.Coords);
    }

    protected override void ComponentStack_IsFunctionalChanged()
    {
      base.ComponentStack_IsFunctionalChanged();
      if (!this.IsWorking)
      {
        if (Sandbox.Game.Multiplayer.Sync.IsServer)
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyRemoteControl, bool>(this, (Func<MyRemoteControl, Action<bool>>) (x => new Action<bool>(x.RequestRelease)), false);
        if ((bool) this.m_autoPilotEnabled)
        {
          this.SetAutoPilotEnabled(false);
          if (this.m_automaticBehaviour != null)
            this.m_automaticBehaviour.StopWorking();
        }
      }
      this.ResourceSink.Update();
      this.RaisePropertiesChangedRemote();
      this.SetDetailedInfoDirty();
    }

    private void Receiver_RequiredInputChanged(
      MyDefinitionId resourceTypeId,
      MyResourceSinkComponent receiver,
      float oldRequirement,
      float newRequirement)
    {
      this.RaisePropertiesChangedRemote();
      this.SetDetailedInfoDirty();
    }

    private void Receiver_IsPoweredChanged()
    {
      this.UpdateIsWorking();
      this.RaisePropertiesChangedRemote();
      this.SetDetailedInfoDirty();
      if (this.IsWorking || !Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.m_releaseRequested = true;
    }

    private float CalculateRequiredPowerInput() => (bool) this.m_autoPilotEnabled || this.ControllerInfo.Controller != null ? this.BlockDefinition.RequiredPowerInput : 1E-07f;

    public override void ShowTerminal() => MyGuiScreenTerminal.Show(MyTerminalPageEnum.ControlPanel, MySession.Static.LocalHumanPlayer.Character, (MyEntity) this, true);

    public void RequestControl()
    {
      if (!MyFakes.ENABLE_REMOTE_CONTROL || MySession.Static.ControlledEntity == this)
        return;
      if (MyGuiScreenTerminal.IsOpen)
        MyGuiScreenTerminal.Hide();
      if (MySession.Static.ControlledEntity == null)
        return;
      this.RequestUse(UseActionEnum.Manipulate, MySession.Static.ControlledEntity);
    }

    private void AcquireControl() => this.AcquireControl(MySession.Static.ControlledEntity);

    private void AcquireControl(IMyControllableEntity previousControlledEntity)
    {
      if (!this.CheckPreviousEntity(previousControlledEntity))
        return;
      if ((bool) this.m_autoPilotEnabled)
        this.SetAutoPilotEnabled(false);
      this.PreviousControlledEntity = previousControlledEntity;
      if (this.PreviousControlledEntity is MyShipController controlledEntity)
      {
        this.m_enableFirstPerson = controlledEntity.EnableFirstPersonView;
        this.cockpitPilot = controlledEntity.Pilot;
        if (this.cockpitPilot != null)
          this.cockpitPilot.CurrentRemoteControl = (IMyControllableEntity) this;
      }
      else
      {
        this.m_enableFirstPerson = true;
        if (this.PreviousControlledEntity is MyCharacter controlledEntity)
          controlledEntity.CurrentRemoteControl = (IMyControllableEntity) this;
      }
      if (MyCubeBuilder.Static.IsActivated)
        MySession.Static.GameFocusManager.Clear();
      this.SetEmissiveStateWorking();
      if (Sandbox.Game.Multiplayer.Sync.IsServer || !previousControlledEntity.ControllerInfo.IsLocallyControlled())
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.GetReplicationClient()?.RequestReplicable(previousControlledEntity.Entity.GetTopMostParent((Type) null).EntityId, (byte) 0, true);
    }

    private bool CheckPreviousEntity(IMyControllableEntity entity)
    {
      switch (entity)
      {
        case MyCharacter _:
          return true;
        case MyCryoChamber _:
          return false;
        case MyCockpit _:
          return true;
        default:
          return false;
      }
    }

    public void RequestControlFromLoad() => this.AcquireControl();

    public void RequestUse(UseActionEnum actionEnum, IMyControllableEntity usedBy) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyRemoteControl, UseActionEnum, long>(this, (Func<MyRemoteControl, Action<UseActionEnum, long>>) (x => new Action<UseActionEnum, long>(x.RequestUseMessage)), actionEnum, usedBy.Entity.EntityId);

    [Event(null, 3185)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void RequestUseMessage(UseActionEnum useAction, long usedById)
    {
      MyEntity entity;
      int num = MyEntities.TryGetEntityById<MyEntity>(usedById, out entity) ? 1 : 0;
      IMyControllableEntity controllableEntity = entity as IMyControllableEntity;
      UseActionResult useResult = UseActionResult.OK;
      if (num != 0 && (useResult = this.CanUse(useAction, controllableEntity)) == UseActionResult.OK && this.CanControl(controllableEntity))
      {
        this.UseSuccessCallback(useAction, usedById, useResult);
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyRemoteControl, UseActionEnum, long, UseActionResult>(this, (Func<MyRemoteControl, Action<UseActionEnum, long, UseActionResult>>) (x => new Action<UseActionEnum, long, UseActionResult>(x.UseSuccessCallback)), useAction, usedById, useResult);
        if (MyVisualScriptLogicProvider.RemoteControlChanged == null)
          return;
        long playerId = 0;
        if (this.ControllerInfo != null)
          playerId = this.ControllerInfo.ControllingIdentityId;
        MyVisualScriptLogicProvider.RemoteControlChanged(true, playerId, this.Name, this.EntityId, this.CubeGrid.Name, this.CubeGrid.EntityId);
      }
      else if (MyEventContext.Current.IsLocallyInvoked)
        this.UseFailureCallback(useAction, usedById, useResult);
      else
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyRemoteControl, UseActionEnum, long, UseActionResult>(this, (Func<MyRemoteControl, Action<UseActionEnum, long, UseActionResult>>) (x => new Action<UseActionEnum, long, UseActionResult>(x.UseFailureCallback)), useAction, usedById, useResult, MyEventContext.Current.Sender);
    }

    [Event(null, 3222)]
    [Reliable]
    [Broadcast]
    private void UseSuccessCallback(
      UseActionEnum useAction,
      long usedById,
      UseActionResult useResult)
    {
      MyEntity entity;
      if (!MyEntities.TryGetEntityById<MyEntity>(usedById, out entity) || !(entity is IMyControllableEntity user))
        return;
      MyRelationsBetweenPlayerAndBlock relations = MyRelationsBetweenPlayerAndBlock.NoOwnership;
      MyCubeBlock myCubeBlock = (MyCubeBlock) this;
      if (myCubeBlock != null && user.ControllerInfo.Controller != null)
        relations = myCubeBlock.GetUserRelationToOwner(user.ControllerInfo.Controller.Player.Identity.IdentityId);
      if (relations.IsFriendly())
        this.sync_UseSuccess(useAction, user);
      else
        this.sync_UseFailed(useAction, useResult, user);
    }

    [Event(null, 3252)]
    [Reliable]
    [Client]
    private void UseFailureCallback(
      UseActionEnum useAction,
      long usedById,
      UseActionResult useResult)
    {
      MyEntity entity;
      MyEntities.TryGetEntityById<MyEntity>(usedById, out entity);
      IMyControllableEntity user = entity as IMyControllableEntity;
      this.sync_UseFailed(useAction, useResult, user);
    }

    private void sync_UseFailed(
      UseActionEnum actionEnum,
      UseActionResult actionResult,
      IMyControllableEntity user)
    {
      if (user == null || !user.ControllerInfo.IsLocallyHumanControlled())
        return;
      switch (actionResult)
      {
        case UseActionResult.UsedBySomeoneElse:
          MyHud.Notifications.Add((MyHudNotificationBase) new MyHudNotification(MyCommonTexts.AlreadyUsedBySomebodyElse, font: "Red"));
          break;
        case UseActionResult.AccessDenied:
          MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
          break;
        case UseActionResult.Unpowered:
          MyHud.Notifications.Add((MyHudNotificationBase) new MyHudNotification(MySpaceTexts.BlockIsNotPowered, font: "Red"));
          break;
        case UseActionResult.CockpitDamaged:
          MyHudNotification myHudNotification = new MyHudNotification(MySpaceTexts.Notification_ControllableBlockIsDamaged, font: "Red");
          myHudNotification.SetTextFormatArguments((object[]) new string[1]
          {
            this.DefinitionDisplayNameText
          });
          MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification);
          break;
        case UseActionResult.MissingDLC:
          MySession.Static.CheckDLCAndNotify((MyDefinitionBase) this.BlockDefinition);
          break;
      }
    }

    public override void OnRegisteredToGridSystems()
    {
      base.OnRegisteredToGridSystems();
      if ((bool) this.m_autoPilotEnabled)
        this.SetAutopilot(true);
      if (!this.CubeGrid.InScene || this.CubeGrid.GridSystems == null || this.CubeGrid.GridSystems.RadioSystem == null)
        return;
      this.CubeGrid.GridSystems.RadioSystem.UpdateRemoteControlInfo();
    }

    public override void OnUnregisteredFromGridSystems()
    {
      base.OnUnregisteredFromGridSystems();
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyRemoteControl, bool>(this, (Func<MyRemoteControl, Action<bool>>) (x => new Action<bool>(x.RequestRelease)), false);
      if ((bool) this.m_autoPilotEnabled)
        this.RemoveAutoPilot();
      if (this.CubeGrid.GridSystems == null || this.CubeGrid.GridSystems.RadioSystem == null)
        return;
      this.CubeGrid.GridSystems.RadioSystem.UpdateRemoteControlInfo();
    }

    public override void ForceReleaseControl()
    {
      base.ForceReleaseControl();
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyRemoteControl, bool>(this, (Func<MyRemoteControl, Action<bool>>) (x => new Action<bool>(x.RequestRelease)), false);
    }

    [Event(null, 3325)]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void RequestRelease(bool previousClosed)
    {
      if (!MyFakes.ENABLE_REMOTE_CONTROL)
        return;
      if (MyVisualScriptLogicProvider.RemoteControlChanged != null)
        MyVisualScriptLogicProvider.RemoteControlChanged(false, this.ControllerInfo == null ? 0L : this.ControllerInfo.ControllingIdentityId, this.Name, this.EntityId, this.CubeGrid.Name, this.CubeGrid.EntityId);
      if (this.m_previousControlledEntity != null)
      {
        if (this.m_previousControlledEntity is MyCockpit)
        {
          if (this.cockpitPilot != null)
            this.cockpitPilot.CurrentRemoteControl = (IMyControllableEntity) null;
          MyCockpit controlledEntity = this.m_previousControlledEntity as MyCockpit;
          if (previousClosed || controlledEntity.Pilot == null)
          {
            this.ReturnControl((IMyControllableEntity) this.cockpitPilot);
            return;
          }
        }
        if (this.m_previousControlledEntity is MyCharacter controlledEntity)
          controlledEntity.CurrentRemoteControl = (IMyControllableEntity) null;
        this.ReturnControl(this.m_previousControlledEntity);
        this.GetFirstRadioReceiver()?.Clear();
      }
      this.RefreshTerminal();
      this.SetEmissiveStateWorking();
      this.ResourceSink.Update();
    }

    private void RefreshTerminal()
    {
      if (this.Pilot == MySession.Static.LocalCharacter)
        return;
      this.RaisePropertiesChanged();
      this.RaisePropertiesChangedRemote();
      this.SetDetailedInfoDirty();
    }

    private void ReturnControl(IMyControllableEntity nextControllableEntity)
    {
      if (this.ControllerInfo.Controller != null)
        this.SwitchControl(nextControllableEntity);
      this.PreviousControlledEntity = (IMyControllableEntity) null;
      if (Sandbox.Game.Multiplayer.Sync.IsServer || nextControllableEntity == null || !nextControllableEntity.ControllerInfo.IsLocallyControlled())
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.GetReplicationClient()?.RequestReplicable(nextControllableEntity.Entity.GetTopMostParent((Type) null).EntityId, (byte) 0, false);
    }

    protected void sync_UseSuccess(UseActionEnum actionEnum, IMyControllableEntity user)
    {
      this.AcquireControl(user);
      if (user.ControllerInfo != null && user.ControllerInfo.Controller != null)
      {
        user.SwitchControl((IMyControllableEntity) this);
        this.RefreshTerminal();
      }
      if ((long) this.m_bindedCamera != 0L && user == MySession.Static.LocalCharacter)
      {
        MyEntity entity;
        if (MyEntities.TryGetEntityById((long) this.m_bindedCamera, out entity))
        {
          if (entity is MyCameraBlock myCameraBlock)
            myCameraBlock.RequestSetView();
          else if (Sandbox.Game.Multiplayer.Sync.IsServer)
            this.m_bindedCamera.Value = 0L;
        }
        else if (Sandbox.Game.Multiplayer.Sync.IsServer)
          this.m_bindedCamera.Value = 0L;
      }
      this.ResourceSink.Update();
    }

    protected override ControllerPriority Priority => (bool) this.m_autoPilotEnabled ? ControllerPriority.AutoPilot : ControllerPriority.Secondary;

    public override void UpdateAfterSimulation10()
    {
      base.UpdateAfterSimulation10();
      if (!MyFakes.ENABLE_REMOTE_CONTROL)
        return;
      if (this.m_previousControlledEntity != null)
      {
        if (Sandbox.Game.Multiplayer.Sync.IsServer && !this.RemoteIsInRangeAndPlayerHasAccess())
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyRemoteControl, bool>(this, (Func<MyRemoteControl, Action<bool>>) (x => new Action<bool>(x.RequestRelease)), false);
        this.GetFirstRadioReceiver()?.UpdateHud(true);
      }
      if (!(bool) this.m_autoPilotEnabled)
        return;
      this.ResetShipControls();
    }

    private MyDataReceiver GetFirstRadioReceiver()
    {
      HashSet<MyDataReceiver> output = new HashSet<MyDataReceiver>();
      MyAntennaSystem.Static.GetEntityReceivers((MyEntity) this.CubeGrid, ref output);
      return output.Count > 0 ? output.FirstElement<MyDataReceiver>() : (MyDataReceiver) null;
    }

    private bool RemoteIsInRangeAndPlayerHasAccess() => this.ControllerInfo.Controller != null && this.CheckRangeAndAccess(this.PreviousControlledEntity, this.ControllerInfo.Controller.Player);

    private bool CheckRangeAndAccess(IMyControllableEntity controlledEntity, MyPlayer player)
    {
      switch (controlledEntity)
      {
        case MyTerminalBlock myTerminalBlock:
          return MyAntennaSystem.Static.CheckConnection((MyEntity) myTerminalBlock.SlimBlock.CubeGrid, (MyEntity) this.CubeGrid, player);
        case MyCharacter myCharacter:
          return MyAntennaSystem.Static.CheckConnection((MyEntity) myCharacter, (MyEntity) this.CubeGrid, player);
        default:
          return true;
      }
    }

    protected override void OnOwnershipChanged()
    {
      base.OnOwnershipChanged();
      if (this.PreviousControlledEntity != null && Sandbox.Game.Multiplayer.Sync.IsServer && this.ControllerInfo.Controller != null)
      {
        switch (this.GetUserRelationToOwner(this.ControllerInfo.ControllingIdentityId))
        {
          case MyRelationsBetweenPlayerAndBlock.Neutral:
          case MyRelationsBetweenPlayerAndBlock.Enemies:
            this.RaiseControlledEntityUsed();
            break;
        }
      }
      if (this.CubeGrid.GridSystems == null || this.CubeGrid.GridSystems.RadioSystem == null)
        return;
      this.CubeGrid.GridSystems.RadioSystem.UpdateRemoteControlInfo();
    }

    protected override void OnControlledEntity_Used()
    {
      base.OnControlledEntity_Used();
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyRemoteControl, bool>(this, (Func<MyRemoteControl, Action<bool>>) (x => new Action<bool>(x.RequestRelease)), false);
    }

    public override MatrixD GetHeadMatrix(
      bool includeY,
      bool includeX = true,
      bool forceHeadAnim = false,
      bool forceHeadBone = false)
    {
      return this.m_previousControlledEntity != null ? this.m_previousControlledEntity.GetHeadMatrix(includeY, includeX, forceHeadAnim) : MatrixD.Identity;
    }

    public UseActionResult CanUse(
      UseActionEnum actionEnum,
      IMyControllableEntity user)
    {
      return this.m_previousControlledEntity != null && user != this.m_previousControlledEntity ? UseActionResult.UsedBySomeoneElse : UseActionResult.OK;
    }

    protected override bool CheckIsWorking() => this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.CheckIsWorking();

    public override bool SetEmissiveStateWorking()
    {
      if (!this.IsWorking)
        return false;
      if (this.m_previousControlledEntity != null)
        return this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Alternative, this.Render.RenderObjectIDs[0]);
      return (bool) this.m_autoPilotEnabled && this.m_automaticBehaviour != null ? this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Warning, this.Render.RenderObjectIDs[0]) : this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Working, this.Render.RenderObjectIDs[0]);
    }

    public override void ShowInventory()
    {
      base.ShowInventory();
      if (!this.m_enableShipControl)
        return;
      MyCharacter user = this.GetUser();
      if (user == null)
        return;
      MyGuiScreenTerminal.Show(MyTerminalPageEnum.Inventory, user, (MyEntity) this, true);
    }

    private MyCharacter GetUser()
    {
      if (this.PreviousControlledEntity == null)
        return (MyCharacter) null;
      if (this.cockpitPilot != null)
        return this.cockpitPilot;
      return this.PreviousControlledEntity is MyCharacter controlledEntity ? controlledEntity : (MyCharacter) null;
    }

    private void SendToolbarItemChanged(ToolbarItem item, int index, int waypointIndex)
    {
      if (this.m_syncing)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyRemoteControl, ToolbarItem, int, int>(this, (Func<MyRemoteControl, Action<ToolbarItem, int, int>>) (x => new Action<ToolbarItem, int, int>(x.OnToolbarItemChanged)), item, index, waypointIndex);
    }

    [Event(null, 3639)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnToolbarItemChanged(ToolbarItem item, int index, int waypointIndex)
    {
      if (waypointIndex < 0 || waypointIndex >= this.m_waypoints.Count)
        return;
      this.m_syncing = true;
      MyToolbarItem myToolbarItem = (MyToolbarItem) null;
      if (item.EntityID != 0L)
      {
        if (string.IsNullOrEmpty(item.GroupName))
        {
          MyTerminalBlock entity;
          if (MyEntities.TryGetEntityById<MyTerminalBlock>(item.EntityID, out entity))
          {
            MyObjectBuilder_ToolbarItemTerminalBlock itemTerminalBlock = MyToolbarItemFactory.TerminalBlockObjectBuilderFromBlock(entity);
            itemTerminalBlock._Action = item.Action;
            itemTerminalBlock.Parameters = item.Parameters;
            myToolbarItem = MyToolbarItemFactory.CreateToolbarItem((MyObjectBuilder_ToolbarItem) itemTerminalBlock);
          }
        }
        else
        {
          MyRemoteControl entity;
          if (MyEntities.TryGetEntityById<MyRemoteControl>(item.EntityID, out entity))
          {
            MyCubeGrid cubeGrid = entity.CubeGrid;
            string groupName = item.GroupName;
            MyBlockGroup group = cubeGrid.GridSystems.TerminalSystem.BlockGroups.Find((Predicate<MyBlockGroup>) (x => x.Name.ToString() == groupName));
            if (group != null)
            {
              MyObjectBuilder_ToolbarItemTerminalGroup itemTerminalGroup = MyToolbarItemFactory.TerminalGroupObjectBuilderFromGroup(group);
              itemTerminalGroup._Action = item.Action;
              itemTerminalGroup.BlockEntityId = item.EntityID;
              itemTerminalGroup.Parameters = item.Parameters;
              myToolbarItem = MyToolbarItemFactory.CreateToolbarItem((MyObjectBuilder_ToolbarItem) itemTerminalGroup);
            }
          }
        }
      }
      MyRemoteControl.MyAutopilotWaypoint waypoint = this.m_waypoints[waypointIndex];
      if (waypoint.Actions == null)
        waypoint.InitActions();
      if (index >= 0 && index < waypoint.Actions.Length)
        waypoint.Actions[index] = myToolbarItem;
      this.RaisePropertiesChangedRemote();
      this.m_syncing = false;
    }

    public void SetAutomaticBehaviour(
      MyRemoteControl.IRemoteControlAutomaticBehaviour automaticBehaviour)
    {
      this.m_automaticBehaviour = automaticBehaviour;
    }

    public void RemoveAutomaticBehaviour() => this.m_automaticBehaviour = (MyRemoteControl.IRemoteControlAutomaticBehaviour) null;

    public bool IsMainRemoteControl
    {
      get => (bool) this.m_isMainRemoteControl;
      set => this.m_isMainRemoteControl.Value = value;
    }

    private void SetMainRemoteControl(bool value)
    {
      if (value && this.CubeGrid.HasMainRemoteControl() && !this.CubeGrid.IsMainRemoteControl((MyTerminalBlock) this))
        this.IsMainRemoteControl = false;
      else
        this.IsMainRemoteControl = value;
    }

    private void MainRemoteControlChanged()
    {
      if ((bool) this.m_isMainRemoteControl)
        this.CubeGrid.SetMainRemoteControl((MyTerminalBlock) this);
      else if (this.CubeGrid.IsMainRemoteControl((MyTerminalBlock) this))
        this.CubeGrid.SetMainRemoteControl((MyTerminalBlock) null);
      if (this.CubeGrid.GridSystems == null || this.CubeGrid.GridSystems.RadioSystem == null)
        return;
      this.CubeGrid.GridSystems.RadioSystem.UpdateRemoteControlInfo();
    }

    protected bool IsMainRemoteControlFree() => !this.CubeGrid.HasMainRemoteControl() || this.CubeGrid.IsMainRemoteControl((MyTerminalBlock) this);

    float Sandbox.ModAPI.Ingame.IMyRemoteControl.SpeedLimit
    {
      get => this.m_autopilotSpeedLimit.Value;
      set => this.m_autopilotSpeedLimit.Value = value;
    }

    FlightMode Sandbox.ModAPI.Ingame.IMyRemoteControl.FlightMode
    {
      get => this.m_currentFlightMode.Value;
      set => this.m_currentFlightMode.Value = value;
    }

    Base6Directions.Direction Sandbox.ModAPI.Ingame.IMyRemoteControl.Direction
    {
      get => this.m_currentDirection.Value;
      set => this.m_currentDirection.Value = value;
    }

    MyWaypointInfo Sandbox.ModAPI.Ingame.IMyRemoteControl.CurrentWaypoint => this.CurrentWaypoint != null ? new MyWaypointInfo(this.CurrentWaypoint.Name, this.CurrentWaypoint.Coords) : MyWaypointInfo.Empty;

    bool Sandbox.ModAPI.Ingame.IMyRemoteControl.WaitForFreeWay
    {
      get => this.m_waitForFreeWay.Value;
      set => this.m_waitForFreeWay.Value = value;
    }

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      if (this.IsWorking && ((bool) this.m_autoPilotEnabled || this.m_forceBehaviorUpdate) && this.m_automaticBehaviour != null)
        this.m_automaticBehaviour.Update();
      this.m_forceBehaviorUpdate = false;
    }

    public void UpdateBeforeSimulationParallel()
    {
      this.m_previousWorldPosition = this.m_currentWorldPosition;
      this.m_currentWorldPosition = this.WorldMatrix.Translation;
      if (this.m_releaseRequested && Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        this.m_releaseRequested = false;
        if (!this.IsWorking)
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyRemoteControl, bool>(this, (Func<MyRemoteControl, Action<bool>>) (x => new Action<bool>(x.RequestRelease)), false);
      }
      if (this.m_savedPreviousControlledEntityId.HasValue)
      {
        MySession.Static.Players.UpdatePlayerControllers(this.EntityId);
        if (this.TryFindSavedEntity())
          this.m_savedPreviousControlledEntityId = new long?();
      }
      this.UpdateAutopilot();
    }

    public void UpdateAfterSimulationParallel()
    {
    }

    public MyParallelUpdateFlags UpdateFlags => this.m_parallelFlag.GetFlags((MyEntity) this);

    public override void DisableUpdates()
    {
      base.DisableUpdates();
      this.m_parallelFlag.Disable((MyEntity) this);
    }

    public class MyAutopilotWaypoint
    {
      public Vector3D Coords;
      public string Name;
      public MyToolbarItem[] Actions;

      public MyAutopilotWaypoint(
        Vector3D coords,
        string name,
        List<MyObjectBuilder_ToolbarItem> actionBuilders,
        List<int> indexes,
        MyRemoteControl owner)
      {
        this.Coords = coords;
        this.Name = name;
        if (actionBuilders == null)
          return;
        this.InitActions();
        bool flag = indexes != null && indexes.Count > 0;
        int num = flag ? 1 : 0;
        for (int index = 0; index < actionBuilders.Count; ++index)
        {
          if (actionBuilders[index] != null)
          {
            if (flag)
              this.Actions[indexes[index]] = MyToolbarItemFactory.CreateToolbarItem(actionBuilders[index]);
            else
              this.Actions[index] = MyToolbarItemFactory.CreateToolbarItem(actionBuilders[index]);
          }
        }
      }

      public MyAutopilotWaypoint(Vector3D coords, string name, MyRemoteControl owner)
        : this(coords, name, (List<MyObjectBuilder_ToolbarItem>) null, (List<int>) null, owner)
      {
      }

      public MyAutopilotWaypoint(IMyGps gps, MyRemoteControl owner)
        : this(gps.Coords, gps.Name, (List<MyObjectBuilder_ToolbarItem>) null, (List<int>) null, owner)
      {
      }

      public MyAutopilotWaypoint(MyObjectBuilder_AutopilotWaypoint builder, MyRemoteControl owner)
        : this(builder.Coords, builder.Name, builder.Actions, builder.Indexes, owner)
      {
      }

      public void InitActions() => this.Actions = new MyToolbarItem[9];

      public void SetActions(List<MyObjectBuilder_Toolbar.Slot> actionSlots)
      {
        this.Actions = new MyToolbarItem[9];
        for (int index = 0; index < actionSlots.Count; ++index)
        {
          if (actionSlots[index].Data != null)
            this.Actions[index] = MyToolbarItemFactory.CreateToolbarItem(actionSlots[index].Data);
        }
      }

      public MyObjectBuilder_AutopilotWaypoint GetObjectBuilder()
      {
        MyObjectBuilder_AutopilotWaypoint newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_AutopilotWaypoint>();
        newObject.Coords = this.Coords;
        newObject.Name = this.Name;
        if (this.Actions != null)
        {
          bool flag = false;
          foreach (MyToolbarItem action in this.Actions)
          {
            if (action != null)
              flag = true;
          }
          if (flag)
          {
            newObject.Actions = new List<MyObjectBuilder_ToolbarItem>();
            newObject.Indexes = new List<int>();
            for (int index = 0; index < this.Actions.Length; ++index)
            {
              MyToolbarItem action = this.Actions[index];
              if (action != null)
              {
                newObject.Actions.Add(action.GetObjectBuilder());
                newObject.Indexes.Add(index);
              }
            }
          }
        }
        return newObject;
      }
    }

    private struct PlanetCoordInformation
    {
      public MyPlanet Planet;
      public double Elevation;
      public double Height;
      public Vector3D PlanetVector;
      public Vector3D GravityWorld;

      internal void Clear()
      {
        this.Planet = (MyPlanet) null;
        this.Elevation = 0.0;
        this.Height = 0.0;
        this.PlanetVector = Vector3D.Up;
        this.GravityWorld = Vector3D.Down;
      }

      internal bool IsValid() => this.Planet != null;

      internal void Calculate(Vector3D worldPoint)
      {
        this.Clear();
        MyPlanet closestPlanet = MyPlanets.Static.GetClosestPlanet(worldPoint);
        if (closestPlanet == null)
          return;
        MySphericalNaturalGravityComponent gravityComponent = (MySphericalNaturalGravityComponent) closestPlanet.Components.Get<MyGravityProviderComponent>();
        Vector3D vector3D = worldPoint - closestPlanet.PositionComp.GetPosition();
        float gravityLimit = gravityComponent.GravityLimit;
        if (vector3D.LengthSquared() > (double) gravityLimit * (double) gravityLimit)
          return;
        this.Planet = closestPlanet;
        this.PlanetVector = vector3D;
        if (Vector3D.IsZero(this.PlanetVector))
          return;
        this.GravityWorld = (Vector3D) gravityComponent.GetWorldGravityNormalized(in worldPoint);
        Vector3 localPos = (Vector3) (worldPoint - this.Planet.WorldMatrix.Translation);
        Vector3D surfacePointLocal = this.Planet.GetClosestSurfacePointLocal(ref localPos);
        this.Height = Vector3D.Distance(localPos, surfacePointLocal);
        this.Elevation = this.PlanetVector.Length();
        this.PlanetVector *= 1.0 / this.Elevation;
      }

      internal double EstimateDistanceToGround(Vector3D worldPoint) => 0.0;
    }

    public interface IRemoteControlAutomaticBehaviour
    {
      bool NeedUpdate { get; }

      bool IsActive { get; }

      bool RotateToTarget { get; set; }

      bool CollisionAvoidance { get; set; }

      float SpeedLimit { get; set; }

      int PlayerPriority { get; set; }

      float MaxPlayerDistance { get; }

      string CurrentBehavior { get; }

      TargetPrioritization PrioritizationStyle { get; set; }

      MyEntity CurrentTarget { get; set; }

      List<DroneTarget> TargetList { get; }

      List<MyEntity> WaypointList { get; }

      bool WaypointActive { get; }

      bool CycleWaypoints { get; set; }

      Vector3D OriginPoint { get; set; }

      float PlayerYAxisOffset { get; }

      float WaypointThresholdDistance { get; }

      bool ResetStuckDetection { get; }

      bool Ambushing { get; set; }

      bool Operational { get; }

      void Update();

      void WaypointAdvanced();

      void TargetAdd(DroneTarget target);

      void TargetClear();

      void TargetRemove(MyEntity target);

      void TargetLoseCurrent();

      void WaypointAdd(MyEntity waypoint);

      void WaypointClear();

      void StopWorking();

      void DebugDraw();

      void Load(MyObjectBuilder_AutomaticBehaviour objectBuilder, MyRemoteControl remoteControl);

      MyObjectBuilder_AutomaticBehaviour GetObjectBuilder();
    }

    public struct DetectedObject
    {
      public float Distance;
      public Vector3D Position;
      public bool IsVoxel;

      public DetectedObject(float dist, Vector3D pos, bool voxel)
      {
        this.Distance = dist;
        this.Position = pos;
        this.IsVoxel = voxel;
      }
    }

    private class MyDebugRenderComponentRemoteControl : MyDebugRenderComponent
    {
      private readonly MyRemoteControl m_remote;
      private MyRemoteControl.MyAutopilotWaypoint m_prevWaypoint;

      public MyDebugRenderComponentRemoteControl(MyRemoteControl remote)
        : base((VRage.ModAPI.IMyEntity) remote)
        => this.m_remote = remote;

      public override void DebugDraw()
      {
      }
    }

    protected sealed class OnAddWaypoints\u003C\u003EVRageMath_Vector3D\u003C\u0023\u003E\u0023System_String\u003C\u0023\u003E : ICallSite<MyRemoteControl, Vector3D[], string[], DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyRemoteControl @this,
        in Vector3D[] coords,
        in string[] names,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnAddWaypoints(coords, names);
      }
    }

    protected sealed class OnMoveWaypointsUp\u003C\u003ESystem_Collections_Generic_List`1\u003CSystem_Int32\u003E : ICallSite<MyRemoteControl, List<int>, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyRemoteControl @this,
        in List<int> indexes,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnMoveWaypointsUp(indexes);
      }
    }

    protected sealed class OnMoveWaypointsDown\u003C\u003ESystem_Collections_Generic_List`1\u003CSystem_Int32\u003E : ICallSite<MyRemoteControl, List<int>, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyRemoteControl @this,
        in List<int> indexes,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnMoveWaypointsDown(indexes);
      }
    }

    protected sealed class OnRemoveWaypoints\u003C\u003ESystem_Int32\u003C\u0023\u003E : ICallSite<MyRemoteControl, int[], DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyRemoteControl @this,
        in int[] indexes,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnRemoveWaypoints(indexes);
      }
    }

    protected sealed class OnResetWaypoint\u003C\u003E : ICallSite<MyRemoteControl, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyRemoteControl @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnResetWaypoint();
      }
    }

    protected sealed class OnPasteAutopilotSetup\u003C\u003ESandbox_Common_ObjectBuilders_MyObjectBuilder_AutopilotClipboard : ICallSite<MyRemoteControl, MyObjectBuilder_AutopilotClipboard, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyRemoteControl @this,
        in MyObjectBuilder_AutopilotClipboard clipboard,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnPasteAutopilotSetup(clipboard);
      }
    }

    protected sealed class ClearWaypoints_Implementation\u003C\u003E : ICallSite<MyRemoteControl, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyRemoteControl @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ClearWaypoints_Implementation();
      }
    }

    protected sealed class OnAddWaypoint\u003C\u003EVRageMath_Vector3D\u0023System_String : ICallSite<MyRemoteControl, Vector3D, string, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyRemoteControl @this,
        in Vector3D point,
        in string name,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnAddWaypoint(point, name);
      }
    }

    protected sealed class OnAddWaypoint\u003C\u003EVRageMath_Vector3D\u0023System_String\u0023System_Collections_Generic_List`1\u003CVRage_Game_MyObjectBuilder_ToolbarItem\u003E : ICallSite<MyRemoteControl, Vector3D, string, List<MyObjectBuilder_ToolbarItem>, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyRemoteControl @this,
        in Vector3D point,
        in string name,
        in List<MyObjectBuilder_ToolbarItem> actionBuilders,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnAddWaypoint(point, name, actionBuilders);
      }
    }

    protected sealed class RequestUseMessage\u003C\u003EVRage_Game_Entity_UseObject_UseActionEnum\u0023System_Int64 : ICallSite<MyRemoteControl, UseActionEnum, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyRemoteControl @this,
        in UseActionEnum useAction,
        in long usedById,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.RequestUseMessage(useAction, usedById);
      }
    }

    protected sealed class UseSuccessCallback\u003C\u003EVRage_Game_Entity_UseObject_UseActionEnum\u0023System_Int64\u0023VRage_Game_Entity_UseObject_UseActionResult : ICallSite<MyRemoteControl, UseActionEnum, long, UseActionResult, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyRemoteControl @this,
        in UseActionEnum useAction,
        in long usedById,
        in UseActionResult useResult,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.UseSuccessCallback(useAction, usedById, useResult);
      }
    }

    protected sealed class UseFailureCallback\u003C\u003EVRage_Game_Entity_UseObject_UseActionEnum\u0023System_Int64\u0023VRage_Game_Entity_UseObject_UseActionResult : ICallSite<MyRemoteControl, UseActionEnum, long, UseActionResult, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyRemoteControl @this,
        in UseActionEnum useAction,
        in long usedById,
        in UseActionResult useResult,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.UseFailureCallback(useAction, usedById, useResult);
      }
    }

    protected sealed class RequestRelease\u003C\u003ESystem_Boolean : ICallSite<MyRemoteControl, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyRemoteControl @this,
        in bool previousClosed,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.RequestRelease(previousClosed);
      }
    }

    protected sealed class OnToolbarItemChanged\u003C\u003ESandbox_Game_Entities_Blocks_ToolbarItem\u0023System_Int32\u0023System_Int32 : ICallSite<MyRemoteControl, ToolbarItem, int, int, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyRemoteControl @this,
        in ToolbarItem item,
        in int index,
        in int waypointIndex,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnToolbarItemChanged(item, index, waypointIndex);
      }
    }

    protected class m_bindedCamera\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<long, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<long, SyncDirection.BothWays>(obj1, obj2));
        ((MyRemoteControl) obj0).m_bindedCamera = (VRage.Sync.Sync<long, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_autopilotSpeedLimit\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyRemoteControl) obj0).m_autopilotSpeedLimit = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_useCollisionAvoidance\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyRemoteControl) obj0).m_useCollisionAvoidance = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_autoPilotEnabled\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyRemoteControl) obj0).m_autoPilotEnabled = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_dockingModeEnabled\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyRemoteControl) obj0).m_dockingModeEnabled = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_currentFlightMode\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<FlightMode, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<FlightMode, SyncDirection.BothWays>(obj1, obj2));
        ((MyRemoteControl) obj0).m_currentFlightMode = (VRage.Sync.Sync<FlightMode, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_waitForFreeWay\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyRemoteControl) obj0).m_waitForFreeWay = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_currentDirection\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<Base6Directions.Direction, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<Base6Directions.Direction, SyncDirection.BothWays>(obj1, obj2));
        ((MyRemoteControl) obj0).m_currentDirection = (VRage.Sync.Sync<Base6Directions.Direction, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_waypointThresholdDistance\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.FromServer>(obj1, obj2));
        ((MyRemoteControl) obj0).m_waypointThresholdDistance = (VRage.Sync.Sync<float, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_isMainRemoteControl\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyRemoteControl) obj0).m_isMainRemoteControl = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_MyRemoteControl\u003C\u003EActor : IActivator, IActivator<MyRemoteControl>
    {
      object IActivator.CreateInstance() => (object) new MyRemoteControl();

      MyRemoteControl IActivator<MyRemoteControl>.CreateInstance() => new MyRemoteControl();
    }
  }
}
