// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyShipController
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Electricity;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication.ClientStates;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Gui;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.ObjectBuilders.Components;
using VRage.Groups;
using VRage.Input;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
  [MyTerminalInterface(new System.Type[] {typeof (Sandbox.ModAPI.IMyShipController), typeof (Sandbox.ModAPI.Ingame.IMyShipController)})]
  public class MyShipController : MyTerminalBlock, IMyControllableEntity, VRage.Game.ModAPI.Interfaces.IMyControllableEntity, IMyRechargeSocketOwner, Sandbox.ModAPI.IMyShipController, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyTerminalBlock, Sandbox.ModAPI.Ingame.IMyShipController
  {
    public MyGridGyroSystem GridGyroSystem;
    public MyGridSelectionSystem GridSelectionSystem;
    public MyGridReflectorLightSystem GridReflectorLights;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_controlThrusters;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_controlWheels;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_controlGyros;
    private bool m_reactorsSwitched = true;
    private bool m_mainCockpitOverwritten;
    protected MyRechargeSocket m_rechargeSocket;
    private MyHudNotification m_notificationLeave;
    private MyHudNotification m_notificationTerminal;
    private MyHudNotification m_landingGearsNotification;
    private MyHudNotification m_noWeaponNotification;
    private MyHudNotification m_weaponSelectedNotification;
    private MyHudNotification m_outOfAmmoNotification;
    private MyHudNotification m_weaponNotWorkingNotification;
    private MyHudNotification m_noControlNotification;
    private MyHudNotification m_connectorsNotification;
    private MyDLCs.MyDLC m_dlcNotificationDisplayed;
    protected bool m_enableFirstPerson;
    protected bool m_enableShipControl = true;
    protected bool m_enableBuilderCockpit;
    private static readonly float RollControlMultiplier = 0.2f;
    private bool m_forcedFPS;
    private MyDefinitionId? m_selectedGunId;
    private MyToolbar m_toolbar;
    private MyToolbar m_buildToolbar;
    public bool BuildingMode;
    [Obsolete]
    public bool hasPower;
    private readonly CachingList<MyGroupControlSystem> m_controlSystems = new CachingList<MyGroupControlSystem>();
    protected MyEntity3DSoundEmitter m_soundEmitter;
    protected MySoundPair m_baseIdleSound;
    protected MySoundPair GetOutOfCockpitSound = MySoundPair.Empty;
    protected MySoundPair GetInCockpitSound = MySoundPair.Empty;
    private MyCasterComponent m_raycaster;
    private int m_switchWeaponCounter;
    private readonly bool[] m_isShooting;
    private static bool m_shouldSetOtherToolbars;
    private bool m_syncing;
    protected MyCharacter m_lastPilot;
    private bool m_isControlled;
    private readonly MyControllerInfo m_info = new MyControllerInfo();
    protected bool m_singleWeaponMode;
    protected Vector3 m_headLocalPosition;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_isMainCockpit;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_horizonIndicatorEnabled;

    public MyResourceDistributorComponent GridResourceDistributor => this.CubeGrid == null ? (MyResourceDistributorComponent) null : (MyResourceDistributorComponent) this.CubeGrid.GridSystems.ResourceDistributor;

    public MyGridWheelSystem GridWheels => this.CubeGrid == null ? (MyGridWheelSystem) null : this.CubeGrid.GridSystems.WheelSystem;

    public MyEntityThrustComponent EntityThrustComponent => this.CubeGrid == null ? (MyEntityThrustComponent) null : this.CubeGrid.Components.Get<MyEntityThrustComponent>();

    protected virtual MyStringId LeaveNotificationHintText => MySpaceTexts.NotificationHintLeaveCockpit;

    public bool EnableShipControl => this.m_enableShipControl;

    public bool PlayDefaultUseSound => this.GetInCockpitSound == MySoundPair.Empty;

    private Vector3 MoveIndicator { get; set; }

    private Vector2 RotationIndicator { get; set; }

    private float RollIndicator { get; set; }

    public MyToolbar Toolbar => this.BuildingMode ? this.m_buildToolbar : this.m_toolbar;

    private bool IsWaitingForWeaponSwitch => (uint) this.m_switchWeaponCounter > 0U;

    protected bool IsShooting(MyShootActionEnum action) => this.m_isShooting[(int) action];

    public bool IsShooting()
    {
      foreach (int index in MyEnum<MyShootActionEnum>.Values)
      {
        if (this.m_isShooting[index])
          return true;
      }
      return false;
    }

    public bool HasWheels => this.ControlWheels && this.GridWheels.WheelCount > 0;

    public MyGroups<MyCubeGrid, MyGridPhysicalGroupData> ControlGroup => MyCubeGridGroups.Static.Physical;

    public virtual MyCharacter Pilot => (MyCharacter) null;

    protected virtual ControllerPriority Priority => ControllerPriority.Primary;

    public MyShipController()
    {
      this.CreateTerminalControls();
      this.m_isShooting = new bool[(int) (MyEnum<MyShootActionEnum>.Range.Max + (byte) 1)];
      this.ControllerInfo.ControlAcquired += new Action<MyEntityController>(this.OnControlAcquired);
      this.ControllerInfo.ControlReleased += new Action<MyEntityController>(this.OnControlReleased);
      this.GridSelectionSystem = new MyGridSelectionSystem(this);
      if (!Sandbox.Engine.Platform.Game.IsDedicated)
        this.m_soundEmitter = new MyEntity3DSoundEmitter((MyEntity) this, true);
      this.m_isMainCockpit.ValueChanged += (Action<SyncBase>) (x => this.MainCockpitChanged());
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyShipController>())
        return;
      base.CreateTerminalControls();
      if (MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT)
      {
        MyTerminalControlCheckbox<MyShipController> checkbox1 = new MyTerminalControlCheckbox<MyShipController>("ControlThrusters", MySpaceTexts.TerminalControlPanel_Cockpit_ControlThrusters, MySpaceTexts.TerminalControlPanel_Cockpit_ControlThrusters);
        checkbox1.Getter = (MyTerminalValueControl<MyShipController, bool>.GetterDelegate) (x => x.ControlThrusters);
        checkbox1.Setter = (MyTerminalValueControl<MyShipController, bool>.SetterDelegate) ((x, v) => x.ControlThrusters = v);
        checkbox1.Visible = (Func<MyShipController, bool>) (x => x.m_enableShipControl);
        checkbox1.Enabled = (Func<MyShipController, bool>) (x => x.IsMainCockpitFree());
        MyTerminalAction<MyShipController> myTerminalAction1 = checkbox1.EnableAction<MyShipController>();
        if (myTerminalAction1 != null)
          myTerminalAction1.Enabled = (Func<MyShipController, bool>) (x => x.m_enableShipControl);
        MyTerminalControlFactory.AddControl<MyShipController>((MyTerminalControl<MyShipController>) checkbox1);
        MyTerminalControlCheckbox<MyShipController> checkbox2 = new MyTerminalControlCheckbox<MyShipController>("ControlWheels", MySpaceTexts.TerminalControlPanel_Cockpit_ControlWheels, MySpaceTexts.TerminalControlPanel_Cockpit_ControlWheels);
        checkbox2.Getter = (MyTerminalValueControl<MyShipController, bool>.GetterDelegate) (x => x.ControlWheels);
        checkbox2.Setter = (MyTerminalValueControl<MyShipController, bool>.SetterDelegate) ((x, v) => x.ControlWheels = v);
        checkbox2.Visible = (Func<MyShipController, bool>) (x => x.m_enableShipControl);
        checkbox2.Enabled = (Func<MyShipController, bool>) (x => x.GridWheels.WheelCount > 0 && x.IsMainCockpitFree());
        MyTerminalAction<MyShipController> myTerminalAction2 = checkbox2.EnableAction<MyShipController>();
        if (myTerminalAction2 != null)
          myTerminalAction2.Enabled = (Func<MyShipController, bool>) (x => x.m_enableShipControl);
        MyTerminalControlFactory.AddControl<MyShipController>((MyTerminalControl<MyShipController>) checkbox2);
        MyTerminalControlCheckbox<MyShipController> checkbox3 = new MyTerminalControlCheckbox<MyShipController>("ControlGyros", MySpaceTexts.TerminalControlPanel_Cockpit_ControlGyros, MySpaceTexts.TerminalControlPanel_Cockpit_ControlGyros_Tooltip);
        checkbox3.Getter = (MyTerminalValueControl<MyShipController, bool>.GetterDelegate) (x => x.ControlGyros);
        checkbox3.Setter = (MyTerminalValueControl<MyShipController, bool>.SetterDelegate) ((x, v) => x.ControlGyros = v);
        checkbox3.Visible = (Func<MyShipController, bool>) (x => x.m_enableShipControl);
        checkbox3.Enabled = (Func<MyShipController, bool>) (x => x.IsMainCockpitFree());
        MyTerminalAction<MyShipController> myTerminalAction3 = checkbox3.EnableAction<MyShipController>();
        if (myTerminalAction3 != null)
          myTerminalAction3.Enabled = (Func<MyShipController, bool>) (x => x.m_enableShipControl);
        MyTerminalControlFactory.AddControl<MyShipController>((MyTerminalControl<MyShipController>) checkbox3);
        MyTerminalControlCheckbox<MyShipController> checkbox4 = new MyTerminalControlCheckbox<MyShipController>("HandBrake", MySpaceTexts.TerminalControlPanel_Cockpit_Handbrake, MySpaceTexts.TerminalControlPanel_Cockpit_Handbrake);
        checkbox4.Getter = (MyTerminalValueControl<MyShipController, bool>.GetterDelegate) (x => x.CubeGrid.GridSystems.WheelSystem.HandBrake);
        checkbox4.Setter = (MyTerminalValueControl<MyShipController, bool>.SetterDelegate) ((x, v) => x.SwitchHandbrake());
        checkbox4.Visible = (Func<MyShipController, bool>) (x => x.m_enableShipControl);
        checkbox4.Enabled = (Func<MyShipController, bool>) (x => x.GridWheels.WheelCount > 0 && x.IsMainCockpitFree());
        MyTerminalAction<MyShipController> myTerminalAction4 = checkbox4.EnableAction<MyShipController>();
        if (myTerminalAction4 != null)
          myTerminalAction4.Enabled = (Func<MyShipController, bool>) (x => x.m_enableShipControl);
        MyTerminalControlFactory.AddControl<MyShipController>((MyTerminalControl<MyShipController>) checkbox4);
      }
      if (MyFakes.ENABLE_DAMPENERS_OVERRIDE)
      {
        MyTerminalControlCheckbox<MyShipController> checkbox = new MyTerminalControlCheckbox<MyShipController>("DampenersOverride", MySpaceTexts.ControlName_InertialDampeners, MySpaceTexts.ControlName_InertialDampeners);
        checkbox.Getter = (MyTerminalValueControl<MyShipController, bool>.GetterDelegate) (x => x.EntityThrustComponent != null && x.EntityThrustComponent.DampenersEnabled);
        checkbox.Setter = (MyTerminalValueControl<MyShipController, bool>.SetterDelegate) ((x, v) => x.CubeGrid.EnableDampingInternal(v, true));
        checkbox.Visible = (Func<MyShipController, bool>) (x => x.m_enableShipControl);
        MyTerminalAction<MyShipController> myTerminalAction = checkbox.EnableAction<MyShipController>();
        if (myTerminalAction != null)
          myTerminalAction.Enabled = (Func<MyShipController, bool>) (x => x.m_enableShipControl);
        checkbox.Enabled = (Func<MyShipController, bool>) (x => x.IsMainCockpitFree());
        MyTerminalControlFactory.AddControl<MyShipController>((MyTerminalControl<MyShipController>) checkbox);
      }
      MyTerminalControlCheckbox<MyShipController> checkbox5 = new MyTerminalControlCheckbox<MyShipController>("HorizonIndicator", MySpaceTexts.TerminalControlPanel_Cockpit_HorizonIndicator, MySpaceTexts.TerminalControlPanel_Cockpit_HorizonIndicator);
      checkbox5.Getter = (MyTerminalValueControl<MyShipController, bool>.GetterDelegate) (x => x.HorizonIndicatorEnabled);
      checkbox5.Setter = (MyTerminalValueControl<MyShipController, bool>.SetterDelegate) ((x, v) => x.HorizonIndicatorEnabled = v);
      checkbox5.Enabled = (Func<MyShipController, bool>) (x => true);
      checkbox5.Visible = (Func<MyShipController, bool>) (x => x.CanHaveHorizon());
      checkbox5.EnableAction<MyShipController>();
      MyTerminalControlFactory.AddControl<MyShipController>((MyTerminalControl<MyShipController>) checkbox5);
      MyTerminalControlCheckbox<MyShipController> checkbox6 = new MyTerminalControlCheckbox<MyShipController>("MainCockpit", MySpaceTexts.TerminalControlPanel_Cockpit_MainCockpit, MySpaceTexts.TerminalControlPanel_Cockpit_MainCockpit);
      checkbox6.Getter = (MyTerminalValueControl<MyShipController, bool>.GetterDelegate) (x => x.IsMainCockpit);
      checkbox6.Setter = (MyTerminalValueControl<MyShipController, bool>.SetterDelegate) ((x, v) => x.IsMainCockpit = v);
      checkbox6.Enabled = (Func<MyShipController, bool>) (x => x.IsMainCockpitFree());
      checkbox6.Visible = (Func<MyShipController, bool>) (x => x.CanBeMainCockpit());
      checkbox6.EnableAction<MyShipController>();
      MyTerminalControlFactory.AddControl<MyShipController>((MyTerminalControl<MyShipController>) checkbox6);
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.SyncFlag = true;
      base.Init(objectBuilder, cubeGrid);
      MyDefinitionManager.Static.GetCubeBlockDefinition(objectBuilder.GetId());
      this.m_enableFirstPerson = this.BlockDefinition.EnableFirstPerson || !MySession.Static.Settings.Enable3rdPersonView;
      this.m_enableShipControl = this.BlockDefinition.EnableShipControl;
      this.m_enableBuilderCockpit = this.BlockDefinition.EnableBuilderCockpit;
      this.m_rechargeSocket = new MyRechargeSocket();
      MyObjectBuilder_ShipController builderShipController = (MyObjectBuilder_ShipController) objectBuilder;
      SerializableDefinitionId? selectedGunId = builderShipController.SelectedGunId;
      this.m_selectedGunId = selectedGunId.HasValue ? new MyDefinitionId?((MyDefinitionId) selectedGunId.GetValueOrDefault()) : new MyDefinitionId?();
      this.m_controlThrusters.SetLocalValue(builderShipController.ControlThrusters);
      this.m_controlWheels.SetLocalValue(builderShipController.ControlWheels);
      this.m_controlGyros.SetLocalValue(builderShipController.ControlGyros);
      if (builderShipController.IsMainCockpit)
        this.m_isMainCockpit.SetLocalValue(true);
      this.m_horizonIndicatorEnabled.SetLocalValue(builderShipController.HorizonIndicatorEnabled);
      this.m_toolbar = new MyToolbar(this.ToolbarType);
      this.m_toolbar.Init(builderShipController.Toolbar, (MyEntity) this);
      this.m_toolbar.ItemChanged += new Action<MyToolbar, MyToolbar.IndexArgs, bool>(this.Toolbar_ItemChanged);
      this.m_buildToolbar = new MyToolbar(MyToolbarType.BuildCockpit);
      this.m_buildToolbar.Init(builderShipController.BuildToolbar, (MyEntity) this);
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
      this.m_baseIdleSound = this.BlockDefinition.PrimarySound;
      this.CubeGrid.OnGridSplit += new Action<MyCubeGrid, MyCubeGrid>(this.CubeGrid_OnGridSplit);
      this.Components.ComponentAdded += new Action<System.Type, MyEntityComponentBase>(this.OnComponentAdded);
      this.Components.ComponentRemoved += new Action<System.Type, MyEntityComponentBase>(this.OnComponentRemoved);
      this.UpdateShipInfo();
      if (this.BlockDefinition.GetInSound != null && this.BlockDefinition.GetInSound.Length > 0)
        this.GetInCockpitSound = new MySoundPair(this.BlockDefinition.GetInSound);
      if (this.BlockDefinition.GetOutSound != null && this.BlockDefinition.GetOutSound.Length > 0)
        this.GetOutOfCockpitSound = new MySoundPair(this.BlockDefinition.GetOutSound);
      this.m_controlThrusters.ValueChanged += new Action<SyncBase>(this.m_controlThrusters_ValueChanged);
    }

    private void m_controlThrusters_ValueChanged(SyncBase obj)
    {
      if (this.EntityThrustComponent == null || !Sandbox.Game.Multiplayer.Sync.Players.HasExtendedControl((IMyControllableEntity) this, (MyEntity) this.CubeGrid))
        return;
      this.EntityThrustComponent.Enabled = (bool) this.m_controlThrusters;
    }

    protected virtual void ComponentStack_IsFunctionalChanged()
    {
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_ShipController builderCubeBlock = (MyObjectBuilder_ShipController) base.GetObjectBuilderCubeBlock(copy);
      MyDefinitionId? selectedGunId = this.m_selectedGunId;
      builderCubeBlock.SelectedGunId = selectedGunId.HasValue ? new SerializableDefinitionId?((SerializableDefinitionId) selectedGunId.GetValueOrDefault()) : new SerializableDefinitionId?();
      builderCubeBlock.UseSingleWeaponMode = this.m_singleWeaponMode;
      builderCubeBlock.ControlThrusters = (bool) this.m_controlThrusters;
      builderCubeBlock.ControlWheels = (bool) this.m_controlWheels;
      builderCubeBlock.ControlGyros = (bool) this.m_controlGyros;
      builderCubeBlock.Toolbar = this.m_toolbar.GetObjectBuilder();
      builderCubeBlock.BuildToolbar = this.m_buildToolbar.GetObjectBuilder();
      builderCubeBlock.IsMainCockpit = (bool) this.m_isMainCockpit;
      builderCubeBlock.HorizonIndicatorEnabled = this.HorizonIndicatorEnabled;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public virtual MatrixD GetHeadMatrix(
      bool includeY,
      bool includeX = true,
      bool forceBoneMatrix = false,
      bool forceHeadBone = false)
    {
      return this.PositionComp.WorldMatrixRef;
    }

    public override MatrixD GetViewMatrix()
    {
      MatrixD headMatrix = this.GetHeadMatrix(!this.ForceFirstPersonCamera, !this.ForceFirstPersonCamera, false, false);
      MatrixD result;
      MatrixD.Invert(ref headMatrix, out result);
      return result;
    }

    public bool PrimaryLookaround => !this.m_enableShipControl;

    public void MoveAndRotate(
      Vector3 moveIndicator,
      Vector2 rotationIndicator,
      float rollIndicator)
    {
      this.MoveIndicator = moveIndicator;
      this.RotationIndicator = rotationIndicator;
      this.RollIndicator = rollIndicator;
    }

    public void WheelJump(bool controlPressed)
    {
      if (!MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT || this.GridWheels == null || (!this.ControlWheels || !this.m_enableShipControl) || (this.m_info.Controller == null || !this.IsControllingCockpit()))
        return;
      this.CubeGrid.GridSystems.WheelSystem.UpdateJumpControlState(controlPressed, true);
    }

    public bool NeedsPerFrameUpdate => this.CubeGrid.GridSystems.ControlSystem != null && this.CubeGrid.GridSystems.ControlSystem.GetShipController() == this;

    public void MoveAndRotate()
    {
      if (this.Closed)
        return;
      MyGroupControlSystem controlSystem = this.CubeGrid.GridSystems.ControlSystem;
      if (controlSystem == null || controlSystem.GetShipController() != null && controlSystem.GetShipController() != this)
        return;
      this.LastMotionIndicator = this.MoveIndicator;
      this.LastRotationIndicator = new Vector3(this.RotationIndicator, this.RollIndicator);
      if (MySessionComponentReplay.Static.IsEntityBeingRecorded(this.CubeGrid.EntityId))
      {
        PerFrameData data = new PerFrameData()
        {
          MovementData = new MovementData?(new MovementData()
          {
            MoveVector = (SerializableVector3) this.MoveIndicator,
            RotateVector = new SerializableVector3(this.RotationIndicator.X, this.RotationIndicator.Y, this.RollIndicator)
          })
        };
        MySessionComponentReplay.Static.ProvideEntityRecordData(this.CubeGrid.EntityId, data);
      }
      if ((!this.m_enableShipControl || !(this.MoveIndicator == Vector3.Zero) || !(this.RotationIndicator == Vector2.Zero) ? 0 : ((double) this.RollIndicator == 0.0 ? 1 : 0)) != 0)
      {
        this.ClearMovementControl();
      }
      else
      {
        if (!this.IsMainCockpit && this.CubeGrid.HasMainCockpit() && !this.m_mainCockpitOverwritten || this.EntityThrustComponent == null && this.GridGyroSystem == null && this.GridWheels == null || this.GridResourceDistributor == null)
          return;
        MyPlayer controllingPlayer = Sandbox.Game.Multiplayer.Sync.Players.GetControllingPlayer((MyEntity) this.CubeGrid);
        if (!Sandbox.Game.Multiplayer.Sync.Players.HasExtendedControl((IMyControllableEntity) this, (MyEntity) this.CubeGrid) && !MySessionComponentReplay.Static.IsEntityBeingReplayed(this.CubeGrid.EntityId) && (this.Pilot == null || controllingPlayer == null || controllingPlayer.Character != this.Pilot) || !this.m_enableShipControl)
          return;
        if (!this.CubeGrid.Physics.RigidBody.IsActive)
          this.CubeGrid.ActivatePhysics();
        MyEntityThrustComponent entityThrustComponent = this.EntityThrustComponent;
        if (this.CubeGrid.GridSystems.ResourceDistributor.ResourceState == MyResourceStateEnum.NoPower)
          return;
        Matrix result;
        this.Orientation.GetMatrix(out result);
        if (entityThrustComponent != null)
        {
          entityThrustComponent.Enabled = (bool) this.m_controlThrusters;
          Vector3 vector3 = Vector3.Transform(this.MoveIndicator, result);
          entityThrustComponent.ControlThrust += vector3;
        }
        if (this.GridGyroSystem != null && (bool) this.m_controlGyros)
        {
          Vector2 sphere = Vector2.ClampToSphere(this.RotationIndicator / 20f, 1f);
          float num = this.RollIndicator * MyShipController.RollControlMultiplier;
          Vector3 vector = Vector3.Transform(new Vector3(-sphere.X, -sphere.Y, -num), result);
          Vector3.ClampToSphere(vector, 1f);
          this.GridGyroSystem.ControlTorque += vector;
        }
        if (!MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT || this.GridWheels == null || !this.ControlWheels)
          return;
        this.GridWheels.AngularVelocity = this.MoveIndicator;
      }
    }

    public void MoveAndRotateStopped() => this.ClearMovementControl();

    public void ClearMovementControl()
    {
      if (this.CubeGrid.GridSystems.ControlSystem == null || this.CubeGrid.GridSystems.ControlSystem.GetShipController() != this)
        return;
      if (this.CubeGrid.GridSystems.ControlSystem != null && this.CubeGrid.GridSystems.ControlSystem.GetShipController() == this)
      {
        this.MoveIndicator = Vector3.Zero;
        this.RotationIndicator = Vector2.Zero;
        this.RollIndicator = 0.0f;
      }
      if (!this.m_enableShipControl)
        return;
      MyEntityThrustComponent entityThrustComponent = this.EntityThrustComponent;
      if (entityThrustComponent != null && !entityThrustComponent.AutopilotEnabled)
        entityThrustComponent.ControlThrust = Vector3.Zero;
      if (this.GridGyroSystem != null && !this.GridGyroSystem.AutopilotEnabled)
        this.GridGyroSystem.ControlTorque = Vector3.Zero;
      if (!MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT || this.GridWheels == null)
        return;
      this.GridWheels.AngularVelocity = Vector3.Zero;
    }

    public virtual bool ForceFirstPersonCamera
    {
      get => this.m_forcedFPS && this.m_enableFirstPerson;
      set
      {
        if (this.m_forcedFPS == value)
          return;
        this.m_forcedFPS = value;
        this.UpdateCameraAfterChange(false);
      }
    }

    public virtual bool EnableFirstPersonView
    {
      get => this.m_enableFirstPerson;
      set => this.m_enableFirstPerson = value;
    }

    public override void UpdatingStopped()
    {
      base.UpdatingStopped();
      this.ClearMovementControl();
    }

    public void UpdateControls() => this.MoveAndRotate();

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (this.CubeGrid.GridSystems.ControlSystem != null && (this.CubeGrid.GridSystems.ControlSystem.GetShipController() == this || this.CubeGrid.ControlledFromTurret))
      {
        if (this.EntityThrustComponent != null && !this.EntityThrustComponent.AutopilotEnabled)
          this.EntityThrustComponent.ControlThrust = Vector3.Zero;
        if (this.GridGyroSystem != null && !this.GridGyroSystem.AutopilotEnabled)
          this.GridGyroSystem.ControlTorque = Vector3.Zero;
        if (MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT && this.GridWheels != null)
          this.GridWheels.AngularVelocity = Vector3.Zero;
      }
      this.UpdateShipInfo();
      if (this.ControllerInfo.Controller != null && MySession.Static.LocalHumanPlayer != null && this.ControllerInfo.Controller == MySession.Static.LocalHumanPlayer.Controller)
      {
        MyEntityController controller = this.CubeGrid.GridSystems.ControlSystem.GetController();
        if (controller == this.ControllerInfo.Controller)
        {
          if (this.m_noControlNotification != null)
          {
            MyHud.Notifications.Remove((MyHudNotificationBase) this.m_noControlNotification);
            this.m_noControlNotification = (MyHudNotification) null;
          }
        }
        else if (this.m_noControlNotification == null && this.EnableShipControl)
        {
          this.m_noControlNotification = controller != null || this.CubeGrid.GridSystems.ControlSystem.GetShipController() == null ? (!this.CubeGrid.HasMainCockpit() || this.CubeGrid.IsMainCockpit((MyTerminalBlock) this) ? (controller == null || !(controller.ControlledEntity is MyCubeBlock) || this.CubeGrid.CubeBlocks.Contains((controller.ControlledEntity as MyCubeBlock).SlimBlock) ? (!this.CubeGrid.IsStatic ? new MyHudNotification(MySpaceTexts.Notification_NoControl, 0) : new MyHudNotification(MySpaceTexts.Notification_NoControlStation, 0)) : new MyHudNotification(MySpaceTexts.Notification_NoControlOtherShip, 0)) : new MyHudNotification(MySpaceTexts.Notification_NoControlNotMain)) : (this.CubeGrid.GridSystems.ControlSystem.GetShipController().Priority != ControllerPriority.AutoPilot ? new MyHudNotification(MySpaceTexts.Notification_NoControlLowerPriority, 0) : new MyHudNotification(MySpaceTexts.Notification_NoControlAutoPilot, 0));
          MyHud.Notifications.Add((MyHudNotificationBase) this.m_noControlNotification);
        }
      }
      foreach (MyShootActionEnum action in MyEnum<MyShootActionEnum>.Values)
      {
        if (this.IsShooting(action))
          this.Shoot(action);
      }
      if (this.CanBeMainCockpit())
      {
        if (this.CubeGrid.HasMainCockpit() && !this.CubeGrid.IsMainCockpit((MyTerminalBlock) this))
        {
          this.DetailedInfo.Clear();
          this.DetailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MainCockpit));
          this.DetailedInfo.Append(": " + (object) this.CubeGrid.MainCockpit.CustomName);
        }
        else
          this.DetailedInfo.Clear();
      }
      this.HandleBuldingMode();
    }

    private void HandleBuldingMode()
    {
      if (!MySandboxGame.Config.ExperimentalMode || (!this.BuildingMode || MySession.Static.IsCameraControlledObject()) && (!MyInput.Static.IsNewKeyPressed(MyKeys.G) || !MyInput.Static.IsAnyCtrlKeyPressed() || (MyInput.Static.IsAnyMousePressed() || !this.m_enableBuilderCockpit) || (!this.CanBeMainCockpit() || !MySession.Static.IsCameraControlledObject() || MySession.Static.ControlledEntity != this)))
        return;
      this.BuildingMode = !this.BuildingMode;
      MyGuiAudio.PlaySound(MyGuiSounds.HudUse);
      this.Toolbar.Unselect();
      if (this.BuildingMode)
      {
        MyHud.Crosshair.ChangeDefaultSprite(MyHudTexturesEnum.Target_enemy, 0.01f);
        MyCubeBuilder.Static.Activate(new MyDefinitionId?());
      }
      else
      {
        MyHud.Crosshair.ResetToDefault();
        MyCubeBuilder.Static.Deactivate();
      }
    }

    private void UpdateShipInfo()
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated || MySession.Static.LocalHumanPlayer != null && this.ControllerInfo.Controller != MySession.Static.LocalHumanPlayer.Controller)
        return;
      if (this.GridResourceDistributor != null)
      {
        MyHud.ShipInfo.FuelRemainingTime = this.GridResourceDistributor.RemainingFuelTimeByType(MyResourceDistributorComponent.ElectricityId);
        MyHud.ShipInfo.Reactors = this.GridResourceDistributor.MaxAvailableResourceByType(MyResourceDistributorComponent.ElectricityId);
        MyHud.ShipInfo.ResourceState = this.GridResourceDistributor.ResourceStateByType(MyResourceDistributorComponent.ElectricityId);
      }
      if (this.GridGyroSystem != null)
        MyHud.ShipInfo.GyroCount = this.GridGyroSystem.GyroCount;
      MyEntityThrustComponent entityThrustComponent = this.EntityThrustComponent;
      if (entityThrustComponent == null)
        return;
      MyHud.ShipInfo.ThrustCount = entityThrustComponent.ThrustCount;
      MyHud.ShipInfo.DampenersEnabled = entityThrustComponent.DampenersEnabled;
    }

    public override void UpdateOnceBeforeFrame() => base.UpdateOnceBeforeFrame();

    public override void UpdateBeforeSimulation10()
    {
      this.UpdateShipInfo10();
      base.UpdateBeforeSimulation10();
    }

    protected void UpdateShipInfo10()
    {
      if (Sandbox.Game.Multiplayer.Sync.IsDedicated || this.CubeGrid.GridSystems == null)
        return;
      MyGridResourceDistributorSystem resourceDistributor = this.CubeGrid.GridSystems.ResourceDistributor;
      this.hasPower = resourceDistributor != null && resourceDistributor.ResourceStateByType(MyResourceDistributorComponent.ElectricityId) != MyResourceStateEnum.NoPower;
      if (this.ControllerInfo == null || !this.ControllerInfo.IsLocallyHumanControlled())
        return;
      if (this.GridResourceDistributor != null)
      {
        MyHud.ShipInfo.PowerUsage = (double) this.GridResourceDistributor.MaxAvailableResourceByType(MyResourceDistributorComponent.ElectricityId) != 0.0 ? this.GridResourceDistributor.TotalRequiredInputByType(MyResourceDistributorComponent.ElectricityId) / this.GridResourceDistributor.MaxAvailableResourceByType(MyResourceDistributorComponent.ElectricityId) : 0.0f;
        MyHud.ShipInfo.NumberOfBatteries = this.GridResourceDistributor.GetSourceCount(MyResourceDistributorComponent.ElectricityId, MyStringHash.GetOrCompute("Battery"));
        this.GridResourceDistributor.UpdateHud(MyHud.SinkGroupInfo);
      }
      this.UpdateShipMass();
      if (this.Parent != null && this.Parent.Physics != null)
      {
        MyHud.ShipInfo.SpeedInKmH = this.HasWheels;
        MyHud.ShipInfo.Speed = this.Parent.Physics.LinearVelocity.Length();
      }
      if (this.GridReflectorLights != null)
        MyHud.ShipInfo.ReflectorLights = this.GridReflectorLights.ReflectorsEnabled;
      if (this.CubeGrid.GridSystems.LandingSystem != null)
      {
        MyHud.ShipInfo.LandingGearsTotal = this.CubeGrid.GridSystems.LandingSystem.TotalGearCount;
        MyHud.ShipInfo.LandingGearsLocked = this.CubeGrid.GridSystems.LandingSystem[LandingGearMode.Locked];
        MyHud.ShipInfo.LandingGearsInProximity = this.CubeGrid.GridSystems.LandingSystem[LandingGearMode.ReadyToLock];
      }
      else
      {
        MyHud.ShipInfo.LandingGearsTotal = 0;
        MyHud.ShipInfo.LandingGearsLocked = 0;
        MyHud.ShipInfo.LandingGearsInProximity = 0;
      }
    }

    private void UpdateShipMass()
    {
      MyHud.ShipInfo.Mass = 0;
      if (!(this.Parent is MyCubeGrid parent))
        return;
      MyHud.ShipInfo.Mass = parent.GetCurrentMass();
    }

    public override void UpdateBeforeSimulation100()
    {
      if (this.m_soundEmitter != null)
      {
        this.m_soundEmitter.Update();
        this.UpdateSoundState();
      }
      base.UpdateBeforeSimulation100();
    }

    public static bool HasPriorityOver(MyShipController first, MyShipController second)
    {
      if (first.Priority < second.Priority)
        return true;
      if (first.Priority > second.Priority)
        return false;
      if (first.CubeGrid.Physics == null && second.CubeGrid.Physics == null)
        return first.CubeGrid.BlocksCount > second.CubeGrid.BlocksCount;
      if (first.CubeGrid.Physics != null && second.CubeGrid.Physics != null)
      {
        HkMassProperties? massProperties = first.CubeGrid.Physics.Shape.MassProperties;
        if (massProperties.HasValue)
        {
          massProperties = second.CubeGrid.Physics.Shape.MassProperties;
          if (massProperties.HasValue)
          {
            massProperties = first.CubeGrid.Physics.Shape.MassProperties;
            double mass1 = (double) massProperties.Value.Mass;
            massProperties = second.CubeGrid.Physics.Shape.MassProperties;
            double mass2 = (double) massProperties.Value.Mass;
            return mass1 > mass2;
          }
        }
      }
      return first.CubeGrid.Physics == null;
    }

    public void RefreshControlNotifications()
    {
      this.RemoveControlNotifications();
      string str1 = "[" + MyInput.Static.GetGameControl(MyControlsSpace.USE).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard) + "]";
      this.m_notificationLeave = new MyHudNotification(this.LeaveNotificationHintText, 0);
      if (!MyInput.Static.IsJoystickConnected() || !MyInput.Static.IsJoystickLastUsed)
        this.m_notificationLeave.SetTextFormatArguments((object) str1, (object) this.DisplayNameText);
      else
        this.m_notificationLeave.SetTextFormatArguments((object) MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPACESHIP, MyControlsSpace.USE), (object) this.DisplayNameText);
      this.m_notificationLeave.Level = MyNotificationLevel.Control;
      string str2 = "[" + MyInput.Static.GetGameControl(MyControlsSpace.TERMINAL).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard) + "]";
      if (!MyInput.Static.IsJoystickConnected() || !MyInput.Static.IsJoystickLastUsed)
      {
        this.m_notificationTerminal = new MyHudNotification(MySpaceTexts.NotificationHintOpenShipControlPanel, 0);
        this.m_notificationTerminal.SetTextFormatArguments((object) str2);
        this.m_notificationTerminal.Level = MyNotificationLevel.Control;
      }
      else
        this.m_notificationTerminal = (MyHudNotification) null;
      MyHud.Notifications.Add((MyHudNotificationBase) this.m_notificationLeave);
      if (this.m_notificationTerminal == null)
        return;
      MyHud.Notifications.Add((MyHudNotificationBase) this.m_notificationTerminal);
    }

    public void RemoveControlNotifications()
    {
      if (this.m_notificationLeave != null)
        MyHud.Notifications.Remove((MyHudNotificationBase) this.m_notificationLeave);
      if (this.m_notificationTerminal == null)
        return;
      MyHud.Notifications.Remove((MyHudNotificationBase) this.m_notificationTerminal);
    }

    private void RefreshDLCNotification()
    {
      if (this.m_dlcNotificationDisplayed != null)
        return;
      MySessionComponentDLC component = MySession.Static.GetComponent<MySessionComponentDLC>();
      MyDLCs.MyDLC missingDefinitionDlc = component.GetFirstMissingDefinitionDLC((MyDefinitionBase) this.BlockDefinition, Sandbox.Game.Multiplayer.Sync.MyId);
      if (missingDefinitionDlc == null)
        return;
      component.PushUsedUnownedDLC(missingDefinitionDlc);
      this.m_dlcNotificationDisplayed = missingDefinitionDlc;
    }

    private void RemoveDLCNotification()
    {
      if (this.m_dlcNotificationDisplayed == null)
        return;
      MySession.Static.GetComponent<MySessionComponentDLC>().PopUsedUnownedDLC(this.m_dlcNotificationDisplayed);
      this.m_dlcNotificationDisplayed = (MyDLCs.MyDLC) null;
    }

    private void OnComponentAdded(System.Type arg1, MyEntityComponentBase arg2)
    {
      if (!(arg1 == typeof (MyCasterComponent)))
        return;
      this.m_raycaster = arg2 as MyCasterComponent;
      this.PositionComp.OnPositionChanged += new Action<MyPositionComponentBase>(this.OnPositionChanged);
      this.OnPositionChanged(this.PositionComp);
    }

    private void OnComponentRemoved(System.Type arg1, MyEntityComponentBase arg2)
    {
      if (!(arg1 == typeof (MyCasterComponent)))
        return;
      this.m_raycaster = (MyCasterComponent) null;
      this.PositionComp.OnPositionChanged -= new Action<MyPositionComponentBase>(this.OnPositionChanged);
    }

    public MySlimBlock RaycasterHitBlock => this.m_raycaster?.HitBlock;

    private void OnPositionChanged(MyPositionComponentBase obj)
    {
      MatrixD newTransform = obj.WorldMatrixRef;
      Vector3D vector3D = Vector3D.Transform(this.BlockDefinition.RaycastOffset, newTransform);
      newTransform.Translation = vector3D;
      if (this.m_raycaster == null)
        return;
      this.m_raycaster.OnWorldPosChanged(ref newTransform);
    }

    public override void OnAddedToScene(object source)
    {
      this.Render.NearFlag = false;
      base.OnAddedToScene(source);
      MyPlayerCollection.UpdateControl((MyEntity) this.CubeGrid);
    }

    public override void OnRemovedFromScene(object source)
    {
      this.m_controlSystems.ApplyChanges();
      base.OnRemovedFromScene(source);
    }

    protected virtual void OnControlAcquired_UpdateCamera()
    {
    }

    protected virtual bool IsCameraController() => false;

    private void OnControlEntityChanged(
      IMyControllableEntity oldControl,
      IMyControllableEntity newControl)
    {
      if (!this.m_enableShipControl || oldControl == null || (oldControl.Entity == null || newControl == null) || (newControl.Entity == null || !this.CubeGrid.IsMainCockpit(oldControl.Entity as MyTerminalBlock)) || (oldControl.Entity.Parent == null ? oldControl.Entity : oldControl.Entity.Parent).EntityId != (newControl.Entity.Parent == null ? newControl.Entity : newControl.Entity.Parent).EntityId)
        return;
      this.ControlGroup.GetGroup(this.CubeGrid)?.GroupData.ControlSystem.AddControllerBlock(this);
      this.GridSelectionSystem.OnControlAcquired();
      this.m_mainCockpitOverwritten = true;
    }

    protected void OnControlAcquired(MyEntityController controller)
    {
      this.m_isControlled = true;
      controller.ControlledEntityChanged += new Action<IMyControllableEntity, IMyControllableEntity>(this.OnControlEntityChanged);
      if (MySession.Static.LocalHumanPlayer == controller.Player || Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        if (MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT && this.m_enableShipControl && (this.IsMainCockpit || !this.CubeGrid.HasMainCockpit()) && (this.CubeGrid.GridSystems.ControlSystem != null && (this.CubeGrid.GridSystems.ControlSystem.GetShipController() == this || this.CubeGrid.GridSystems.ControlSystem.GetShipController() == null)))
          this.GridWheels.InitControl(controller.ControlledEntity as MyEntity);
        if (MySession.Static.CameraController is MyEntity && this.IsCameraController() && (MySession.Static.LocalHumanPlayer == controller.Player && !MySession.Static.GetComponent<MySessionComponentCutscenes>().IsCutsceneRunning))
          MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, (VRage.ModAPI.IMyEntity) this, new Vector3D?());
        if (this.GridResourceDistributor != null)
          this.GridResourceDistributor.ConveyorSystem_OnPoweredChanged();
        if (this.EntityThrustComponent != null)
          this.EntityThrustComponent.MarkDirty();
        this.Static_CameraAttachedToChanged((IMyCameraController) null, (IMyCameraController) null);
        if (MySession.Static.LocalHumanPlayer == controller.Player)
        {
          if (MySession.Static.Settings.RespawnShipDelete && this.CubeGrid.IsRespawnGrid)
            MyHud.Notifications.Add(MyNotificationSingletons.RespawnShipWarning);
          this.RefreshControlNotifications();
          this.RefreshDLCNotification();
          if (this.IsCameraController())
            this.OnControlAcquired_UpdateCamera();
          MyHud.HideAll();
          MyHud.ShipInfo.Show((Action<MyHudShipInfo>) null);
          MyHud.Crosshair.ResetToDefault();
          MyHud.SinkGroupInfo.Visible = true;
          MyHud.GravityIndicator.Entity = (MyEntity) this;
          MyHud.GravityIndicator.Show((Action<MyHudGravityIndicator>) null);
          MyHud.OreMarkers.Visible = true;
          MyHud.LargeTurretTargets.Visible = true;
        }
      }
      else
        this.UpdateHudMarker();
      if (this.m_enableShipControl && (this.IsMainCockpit || !this.CubeGrid.HasMainCockpit()))
      {
        this.ControlGroup.GetGroup(this.CubeGrid)?.GroupData.ControlSystem.AddControllerBlock(this);
        this.GridSelectionSystem.OnControlAcquired();
      }
      if (this.BuildingMode && MySession.Static.ControlledEntity is MyRemoteControl)
        this.BuildingMode = false;
      if (this.BuildingMode)
        MyHud.Crosshair.ChangeDefaultSprite(MyHudTexturesEnum.Target_enemy, 0.01f);
      else
        MyHud.Crosshair.ResetToDefault();
      MyEntityThrustComponent entityThrustComponent = this.EntityThrustComponent;
      if (controller == Sandbox.Game.Multiplayer.Sync.Players.GetEntityController((MyEntity) this.CubeGrid) && entityThrustComponent != null)
        entityThrustComponent.Enabled = (bool) this.m_controlThrusters;
      this.UpdateShipInfo10();
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
      if (!Sandbox.Game.Multiplayer.Sync.IsServer && controller.Player != MySession.Static.LocalHumanPlayer)
        return;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
    }

    protected virtual void OnControlReleased_UpdateCamera()
    {
    }

    protected virtual void OnControlReleased(MyEntityController controller)
    {
      this.m_isControlled = false;
      controller.ControlledEntityChanged -= new Action<IMyControllableEntity, IMyControllableEntity>(this.OnControlEntityChanged);
      this.m_mainCockpitOverwritten = false;
      MyEntityThrustComponent entityThrustComponent = this.EntityThrustComponent;
      if (Sandbox.Game.Multiplayer.Sync.Players.GetEntityController((MyEntity) this) == controller && entityThrustComponent != null)
        entityThrustComponent.Enabled = true;
      if ((MySession.Static.LocalHumanPlayer == controller.Player || Sandbox.Game.Multiplayer.Sync.IsServer) && entityThrustComponent != null)
        this.ClearMovementControl();
      if (MySession.Static.LocalHumanPlayer == controller.Player)
      {
        this.OnControlReleased_UpdateCamera();
        this.ForceFirstPersonCamera = false;
        if (MyGuiScreenGamePlay.Static != null)
          this.Static_CameraAttachedToChanged((IMyCameraController) null, (IMyCameraController) null);
        MyHud.Notifications.Remove(MyNotificationSingletons.RespawnShipWarning);
        this.RemoveControlNotifications();
        this.RemoveDLCNotification();
        MyHud.ShipInfo.Hide();
        MyHud.GravityIndicator.Hide();
        MyHud.Crosshair.HideDefaultSprite();
        MyHud.Crosshair.Recenter();
        MyHud.LargeTurretTargets.Visible = false;
        MyHud.Notifications.Remove((MyHudNotificationBase) this.m_noControlNotification);
        this.m_noControlNotification = (MyHudNotification) null;
      }
      else if (!MyFakes.ENABLE_RADIO_HUD)
        MyHud.LocationMarkers.UnregisterMarker((MyEntity) this);
      if (this.IsShooting())
        this.EndShootAll();
      if (this.m_enableShipControl && (this.IsMainCockpit || !this.CubeGrid.HasMainCockpit()))
      {
        if (this.GridSelectionSystem != null)
          this.GridSelectionSystem.OnControlReleased();
        this.ControlGroup.GetGroup(this.CubeGrid)?.GroupData.ControlSystem.RemoveControllerBlock(this);
      }
      if (!MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT || !this.m_enableShipControl || !this.IsControllingCockpit())
        return;
      this.GridWheels.ReleaseControl(controller.ControlledEntity as MyEntity);
    }

    public virtual void ForceReleaseControl()
    {
    }

    private void UpdateHudMarker()
    {
      if (MyFakes.ENABLE_RADIO_HUD)
        return;
      MyHud.LocationMarkers.RegisterMarker((MyEntity) this, new MyHudEntityParams()
      {
        FlagsEnum = MyHudIndicatorFlagsEnum.SHOW_TEXT,
        Text = new StringBuilder(this.ControllerInfo.Controller.Player.DisplayName),
        ShouldDraw = new Func<bool>(MyHud.CheckShowPlayerNamesOnHud)
      });
    }

    protected virtual bool ShouldSit() => !this.m_enableShipControl;

    private void Static_CameraAttachedToChanged(
      IMyCameraController oldController,
      IMyCameraController newController)
    {
      if (MySession.Static.ControlledEntity == this && newController != MyThirdPersonSpectator.Static && newController != this)
        this.EndShootAll();
      this.UpdateCameraAfterChange();
    }

    protected virtual void UpdateCameraAfterChange(bool resetHeadLocalAngle = true)
    {
    }

    public void Shoot(MyShootActionEnum action)
    {
      if (!this.m_enableShipControl || this.IsWaitingForWeaponSwitch || !this.GridSelectionSystem.CanShoot(action, out MyGunStatusEnum _, out IMyGunObject<MyDeviceBase> _))
        return;
      this.GridSelectionSystem.Shoot(action);
    }

    public void Zoom(bool newKeyPress)
    {
    }

    public void Use()
    {
      if (this.GetOutOfCockpitSound == MySoundPair.Empty)
        MyGuiAudio.PlaySound(MyGuiSounds.HudUse);
      this.RaiseControlledEntityUsed();
    }

    public void PlayUseSound(bool getIn)
    {
      if (this.m_soundEmitter == null)
        return;
      this.m_soundEmitter.VolumeMultiplier = 1f;
      if (getIn)
        this.m_soundEmitter.PlaySound(this.GetInCockpitSound, force2D: (MySession.Static.LocalCharacter != null && this.Pilot == MySession.Static.LocalCharacter));
      else
        this.m_soundEmitter.PlaySound(this.GetOutOfCockpitSound, force2D: (MySession.Static.LocalCharacter != null && this.m_lastPilot == MySession.Static.LocalCharacter));
    }

    public void RaiseControlledEntityUsed() => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyShipController>(this, (Func<MyShipController, Action>) (x => new Action(x.sync_ControlledEntity_Used)));

    public void UseContinues()
    {
    }

    public void UseFinished()
    {
    }

    public void PickUp()
    {
    }

    public void PickUpContinues()
    {
    }

    public void PickUpFinished()
    {
    }

    public void Crouch()
    {
    }

    public void Jump(Vector3 moveIndicator)
    {
    }

    public void SwitchWalk()
    {
    }

    public void Sprint(bool enabled)
    {
    }

    public void Up()
    {
    }

    public void Down()
    {
    }

    public virtual void ShowInventory()
    {
    }

    public virtual void ShowTerminal()
    {
    }

    public void SwitchBroadcasting()
    {
      if (!this.m_enableShipControl)
        return;
      if (this.CubeGrid.GridSystems.RadioSystem.AntennasBroadcasterEnabled == MyMultipleEnabledEnum.AllDisabled)
      {
        this.CubeGrid.GridSystems.RadioSystem.AntennasBroadcasterEnabled = MyMultipleEnabledEnum.AllEnabled;
        MyGuiAudio.PlaySound(MyGuiSounds.HudAntennaOn);
      }
      else
      {
        this.CubeGrid.GridSystems.RadioSystem.AntennasBroadcasterEnabled = MyMultipleEnabledEnum.AllDisabled;
        if (this.CubeGrid.GridSystems.RadioSystem.AntennasBroadcasterEnabled == MyMultipleEnabledEnum.NoObjects)
          return;
        MyGuiAudio.PlaySound(MyGuiSounds.HudAntennaOff);
      }
    }

    public void SwitchDamping()
    {
      if (!this.m_enableShipControl || this.EntityThrustComponent == null)
        return;
      this.CubeGrid.EnableDampingInternal(!this.EntityThrustComponent.DampenersEnabled, true);
      if (this.EntityThrustComponent.DampenersEnabled)
        return;
      this.RelativeDampeningEntity = (MyEntity) null;
    }

    public virtual void SwitchThrusts()
    {
    }

    public void Die()
    {
    }

    public void SwitchLights()
    {
      if (!this.m_enableShipControl)
        return;
      if (this.GridReflectorLights.ReflectorsEnabled == MyMultipleEnabledEnum.AllDisabled)
        this.GridReflectorLights.ReflectorsEnabled = MyMultipleEnabledEnum.AllEnabled;
      else
        this.GridReflectorLights.ReflectorsEnabled = MyMultipleEnabledEnum.AllDisabled;
    }

    public void SwitchHandbrake()
    {
      if (!this.m_enableShipControl || !this.IsMainCockpit && this.CubeGrid.HasMainCockpit())
        return;
      this.CubeGrid.SetHandbrakeRequest(!this.CubeGrid.GridSystems.WheelSystem.HandBrake);
    }

    public void SwitchLandingGears()
    {
      if (this.m_enableShipControl && (this.IsMainCockpit || !this.CubeGrid.HasMainCockpit()))
      {
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, bool>(this.CubeGrid, (Func<MyCubeGrid, Action<bool>>) (x => new Action<bool>(x.SetHandbrakeRequest)), !this.CubeGrid.GridSystems.WheelSystem.HandBrake);
        this.CubeGrid.GridSystems.LandingSystem.Switch();
        this.CubeGrid.GridSystems.ConveyorSystem.ToggleConnectors();
        if (this.CubeGrid.GridSystems.WheelSystem.HandBrake)
          MyGuiAudio.PlaySound(MyGuiSounds.HudBrakeOff);
        else
          MyGuiAudio.PlaySound(MyGuiSounds.HudBrakeOn);
      }
      this.HudNotifications();
    }

    public void HudNotifications()
    {
      if (!this.ControllerInfo.IsLocallyHumanControlled())
        return;
      if (this.CubeGrid.GridSystems.LandingSystem.HudMessage != MyStringId.NullOrEmpty)
      {
        this.m_landingGearsNotification = new MyHudNotification(this.CubeGrid.GridSystems.LandingSystem.HudMessage);
        MyHud.Notifications.Add((MyHudNotificationBase) this.m_landingGearsNotification);
        this.CubeGrid.GridSystems.LandingSystem.HudMessage = MyStringId.NullOrEmpty;
      }
      if (this.CubeGrid.GridSystems.ConveyorSystem.HudMessage != MyStringId.NullOrEmpty)
      {
        this.m_connectorsNotification = new MyHudNotification(this.CubeGrid.GridSystems.ConveyorSystem.HudMessage);
        MyHud.Notifications.Add((MyHudNotificationBase) this.m_connectorsNotification);
        this.CubeGrid.GridSystems.ConveyorSystem.HudMessage = MyStringId.NullOrEmpty;
      }
      if (string.IsNullOrEmpty(this.CubeGrid.GridSystems.ConveyorSystem.HudMessageCustom))
        return;
      MyHudNotification myHudNotification = new MyHudNotification(MySpaceTexts.Format_OneParameter);
      myHudNotification.SetTextFormatArguments((object) this.CubeGrid.GridSystems.ConveyorSystem.HudMessageCustom);
      MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification);
      this.CubeGrid.GridSystems.ConveyorSystem.HudMessageCustom = string.Empty;
    }

    public void SwitchReactors()
    {
      if (this.CubeGrid.MainCockpit != null && !this.IsMainCockpit || !this.m_enableShipControl)
        return;
      if (this.CubeGrid.SwitchPower())
        this.CubeGrid.ChangePowerProducerState(MyMultipleEnabledEnum.AllEnabled, MySession.Static.LocalPlayerId);
      else
        this.CubeGrid.ChangePowerProducerState(MyMultipleEnabledEnum.AllDisabled, MySession.Static.LocalPlayerId);
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.CubeGrid.ActivatePhysics();
    }

    public void DrawHud(IMyCameraController camera, long playerId)
    {
      if (camera is MySpectatorCameraController)
      {
        MyHud.Crosshair.Recenter();
      }
      else
      {
        if (this.GridSelectionSystem != null)
          this.GridSelectionSystem.DrawHud(camera, playerId);
        Vector3D worldPosition = this.PositionComp.GetPosition() + 1000.0 * this.PositionComp.WorldMatrixRef.Forward;
        Vector2 zero = Vector2.Zero;
        ref Vector2 local = ref zero;
        if (MyHudCrosshair.GetProjectedVector(worldPosition, ref local))
          MyHud.Crosshair.ChangePosition(zero);
        if (this.m_raycaster == null)
          return;
        MySlimBlock block = this.m_raycaster.HitBlock;
        if (block == null)
        {
          MyWelder.ProjectionRaycastData projectedBlock = this.FindProjectedBlock();
          if (projectedBlock.raycastResult == BuildCheckResult.OK)
            block = projectedBlock.hitCube;
        }
        if (block != null)
        {
          if (block.CubeGrid.Projector == null)
          {
            MySlimBlock.SetBlockComponents(MyHud.BlockInfo, block);
            MyHud.BlockInfo.BlockIntegrity = block.Integrity / block.MaxIntegrity;
          }
          else
          {
            MySlimBlock.SetBlockComponents(MyHud.BlockInfo, block.BlockDefinition);
            MyHud.BlockInfo.BlockIntegrity = 0.01f;
            MyHud.BlockInfo.MissingComponentIndex = 0;
          }
          MyHud.BlockInfo.DefinitionId = block.BlockDefinition.Id;
          MyHud.BlockInfo.BlockName = block.BlockDefinition.DisplayNameText;
          MyHud.BlockInfo.PCUCost = block.BlockDefinition.PCU;
          MyHud.BlockInfo.BlockIcons = block.BlockDefinition.Icons;
          MyHud.BlockInfo.CriticalIntegrity = block.BlockDefinition.CriticalIntegrityRatio;
          MyHud.BlockInfo.CriticalComponentIndex = (int) block.BlockDefinition.CriticalGroup;
          MyHud.BlockInfo.OwnershipIntegrity = block.BlockDefinition.OwnershipIntegrityRatio;
          MyHud.BlockInfo.BlockBuiltBy = block.BuiltBy;
          MyHud.BlockInfo.GridSize = block.CubeGrid.GridSizeEnum;
        }
        else
        {
          MyHud.BlockInfo.DefinitionId = new MyDefinitionId();
          MyHud.BlockInfo.MissingComponentIndex = -1;
          MyHud.BlockInfo.BlockName = this.m_raycaster.Caster.DrillDefinition.DisplayNameText;
          MyHud.BlockInfo.SetContextHelp(this.m_raycaster.Caster.DrillDefinition);
          MyHud.BlockInfo.PCUCost = 0;
          MyHud.BlockInfo.BlockIcons = this.m_raycaster.Caster.DrillDefinition.Icons;
          MyHud.BlockInfo.BlockIntegrity = 1f;
          MyHud.BlockInfo.CriticalIntegrity = 0.0f;
          MyHud.BlockInfo.CriticalComponentIndex = 0;
          MyHud.BlockInfo.OwnershipIntegrity = 0.0f;
          MyHud.BlockInfo.BlockBuiltBy = 0L;
          MyHud.BlockInfo.GridSize = MyCubeSize.Small;
          MyHud.BlockInfo.Components.Clear();
        }
      }
    }

    public MyEntity TopGrid => this.Parent;

    public MyEntity IsUsing => (MyEntity) null;

    public virtual bool IsLargeShip() => true;

    public override Vector3D LocationForHudMarker => base.LocationForHudMarker + 0.65 * (double) this.CubeGrid.GridSize * (double) this.BlockDefinition.Size.Y * this.PositionComp.WorldMatrixRef.Up;

    public MyShipControllerDefinition BlockDefinition => base.BlockDefinition as MyShipControllerDefinition;

    public bool ControlThrusters
    {
      get => (bool) this.m_controlThrusters;
      set => this.m_controlThrusters.Value = value;
    }

    public bool ControlWheels
    {
      get => (bool) this.m_controlWheels;
      set => this.m_controlWheels.Value = value;
    }

    public bool ControlGyros
    {
      get => (bool) this.m_controlGyros;
      set => this.m_controlGyros.Value = value;
    }

    public bool CanSwitchToWeapon(MyDefinitionId? weapon)
    {
      if (!weapon.HasValue)
        return true;
      MyObjectBuilderType typeId = weapon.Value.TypeId;
      return typeId == typeof (MyObjectBuilder_Drill) || typeId == typeof (MyObjectBuilder_SmallMissileLauncher) || (typeId == typeof (MyObjectBuilder_SmallGatlingGun) || typeId == typeof (MyObjectBuilder_ShipGrinder)) || (typeId == typeof (MyObjectBuilder_ShipWelder) || typeId == typeof (MyObjectBuilder_SmallMissileLauncherReload));
    }

    public void SwitchToWeapon(MyDefinitionId weapon)
    {
      if (!this.m_enableShipControl)
        return;
      this.SwitchToWeaponInternal(new MyDefinitionId?(weapon), true);
    }

    public void SwitchToWeapon(MyToolbarItemWeapon weapon)
    {
      if (!this.m_enableShipControl)
        return;
      this.SwitchToWeaponInternal(weapon?.Definition.Id, true);
    }

    private void SwitchToWeaponInternal(MyDefinitionId? weapon, bool updateSync)
    {
      MyDefinitionId? nullable1;
      if (MySessionComponentReplay.Static.IsEntityBeingRecorded(this.CubeGrid.EntityId))
      {
        PerFrameData perFrameData = new PerFrameData();
        ref PerFrameData local1 = ref perFrameData;
        SwitchWeaponData switchWeaponData = new SwitchWeaponData();
        ref SwitchWeaponData local2 = ref switchWeaponData;
        nullable1 = weapon;
        SerializableDefinitionId? nullable2 = nullable1.HasValue ? new SerializableDefinitionId?((SerializableDefinitionId) nullable1.GetValueOrDefault()) : new SerializableDefinitionId?();
        local2.WeaponDefinition = nullable2;
        SwitchWeaponData? nullable3 = new SwitchWeaponData?(switchWeaponData);
        local1.SwitchWeaponData = nullable3;
        PerFrameData data = perFrameData;
        MySessionComponentReplay.Static.ProvideEntityRecordData(this.CubeGrid.EntityId, data);
      }
      if (updateSync)
      {
        this.RequestSwitchToWeapon(weapon, (MyObjectBuilder_EntityBase) null, 0L);
      }
      else
      {
        this.StopCurrentWeaponShooting();
        if (weapon.HasValue)
        {
          this.SwitchToWeaponInternal(weapon);
          ((System.Type) weapon.Value.TypeId).Name.Split('_');
        }
        else
        {
          this.m_selectedGunId = new MyDefinitionId?();
          MyGridSelectionSystem gridSelectionSystem = this.GridSelectionSystem;
          nullable1 = new MyDefinitionId?();
          MyDefinitionId? gunId = nullable1;
          gridSelectionSystem.SwitchTo(gunId);
        }
      }
    }

    private void SwitchToWeaponInternal(MyDefinitionId? gunId)
    {
      this.GridSelectionSystem.SwitchTo(gunId, this.m_singleWeaponMode);
      this.m_selectedGunId = gunId;
      if (!this.ControllerInfo.IsLocallyHumanControlled())
        return;
      if (this.m_weaponSelectedNotification == null)
        this.m_weaponSelectedNotification = new MyHudNotification(MyCommonTexts.NotificationSwitchedToWeapon);
      this.m_weaponSelectedNotification.SetTextFormatArguments((object) MyDeviceBase.GetGunNotificationName(this.m_selectedGunId.Value));
      if (this.m_weaponSelectedNotification.Alive)
      {
        MyHud.Notifications.Update((MyHudNotificationBase) this.m_weaponSelectedNotification);
        this.m_weaponSelectedNotification.ResetAliveTime();
      }
      else
        MyHud.Notifications.Add((MyHudNotificationBase) this.m_weaponSelectedNotification);
    }

    private void SwitchAmmoMagazineInternal(bool sync)
    {
      if (sync)
      {
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyShipController>(this, (Func<MyShipController, Action>) (x => new Action(x.OnSwitchAmmoMagazineRequest)));
      }
      else
      {
        if (!this.m_enableShipControl || this.IsWaitingForWeaponSwitch)
          return;
        this.GridSelectionSystem.SwitchAmmoMagazine();
      }
    }

    private void SwitchAmmoMagazineSuccess()
    {
      if (!this.GridSelectionSystem.CanSwitchAmmoMagazine())
        return;
      this.SwitchAmmoMagazineInternal(false);
    }

    private void ShowShootNotification(MyGunStatusEnum status, IMyGunObject<MyDeviceBase> weapon)
    {
      if (!this.ControllerInfo.IsLocallyHumanControlled())
        return;
      switch (status)
      {
        case MyGunStatusEnum.NotFunctional:
          if (this.m_weaponNotWorkingNotification == null)
            this.m_weaponNotWorkingNotification = new MyHudNotification(MyCommonTexts.NotificationWeaponNotWorking, 2000, "Red");
          if (weapon is MyCubeBlock)
            this.m_weaponNotWorkingNotification.SetTextFormatArguments((object) (weapon as MyCubeBlock).DisplayNameText);
          MyHud.Notifications.Add((MyHudNotificationBase) this.m_weaponNotWorkingNotification);
          break;
        case MyGunStatusEnum.OutOfAmmo:
          if (this.m_outOfAmmoNotification == null)
            this.m_outOfAmmoNotification = new MyHudNotification(MyCommonTexts.OutOfAmmo, 2000, "Red");
          if (weapon is MyCubeBlock)
            this.m_outOfAmmoNotification.SetTextFormatArguments((object) (weapon as MyCubeBlock).DisplayNameText);
          MyHud.Notifications.Add((MyHudNotificationBase) this.m_outOfAmmoNotification);
          break;
        case MyGunStatusEnum.NotSelected:
          if (this.m_noWeaponNotification == null)
          {
            this.m_noWeaponNotification = new MyHudNotification(MyCommonTexts.NotificationNoWeaponSelected, 2000, "Red");
            MyHud.Notifications.Add((MyHudNotificationBase) this.m_noWeaponNotification);
          }
          MyHud.Notifications.Add((MyHudNotificationBase) this.m_noWeaponNotification);
          break;
      }
    }

    public override void OnRegisteredToGridSystems()
    {
      this.GridGyroSystem = this.CubeGrid.GridSystems.GyroSystem;
      this.GridReflectorLights = this.CubeGrid.GridSystems.ReflectorLightSystem;
      this.CubeGrid.AddedToLogicalGroup += new Action<MyGridLogicalGroupData>(this.CubeGrid_AddedToLogicalGroup);
      this.CubeGrid.RemovedFromLogicalGroup += new Action(this.CubeGrid_RemovedFromLogicalGroup);
      this.SetWeaponSystem(this.CubeGrid.GridSystems.WeaponSystem);
      base.OnRegisteredToGridSystems();
    }

    public override void OnUnregisteredFromGridSystems()
    {
      if (this.EntityThrustComponent != null)
        this.ClearMovementControl();
      this.CubeGrid.AddedToLogicalGroup -= new Action<MyGridLogicalGroupData>(this.CubeGrid_AddedToLogicalGroup);
      this.CubeGrid.RemovedFromLogicalGroup -= new Action(this.CubeGrid_RemovedFromLogicalGroup);
      this.CubeGrid_RemovedFromLogicalGroup();
      this.GridGyroSystem = (MyGridGyroSystem) null;
      this.GridReflectorLights = (MyGridReflectorLightSystem) null;
      base.OnUnregisteredFromGridSystems();
    }

    private void CubeGrid_RemovedFromLogicalGroup()
    {
      this.GridSelectionSystem.WeaponSystem = (MyGridWeaponSystem) null;
      this.GridSelectionSystem.SwitchTo(new MyDefinitionId?());
    }

    private void CubeGrid_AddedToLogicalGroup(MyGridLogicalGroupData obj) => this.SetWeaponSystem(obj.WeaponSystem);

    public void SetWeaponSystem(MyGridWeaponSystem weaponSystem)
    {
      this.GridSelectionSystem.WeaponSystem = weaponSystem;
      this.GridSelectionSystem.SwitchTo(this.m_selectedGunId, this.m_singleWeaponMode);
    }

    public override void UpdateVisual()
    {
      if (this.Render.NearFlag)
        this.Render.ColorMaskHsv = this.SlimBlock.ColorMaskHSV;
      else
        base.UpdateVisual();
    }

    [Event(null, 1925)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    protected void sync_ControlledEntity_Used()
    {
      this.OnControlledEntity_Used();
      if (this.GetOutOfCockpitSound == MySoundPair.Empty)
        return;
      this.PlayUseSound(false);
    }

    [Event(null, 1933)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnSwitchHelmet()
    {
      if (this.Pilot == null || this.Pilot.OxygenComponent == null)
        return;
      this.Pilot.OxygenComponent.SwitchHelmet();
    }

    protected virtual void OnControlledEntity_Used()
    {
    }

    public MyEntity Entity => (MyEntity) this;

    public MyControllerInfo ControllerInfo => this.m_info;

    private void SwitchToWeaponSuccess(
      MyDefinitionId? weapon,
      MyObjectBuilder_Base weaponObjectBuilder,
      long weaponEntityId)
    {
      this.SwitchToWeaponInternal(weapon, false);
    }

    MyRechargeSocket IMyRechargeSocketOwner.RechargeSocket => this.m_rechargeSocket;

    public void BeginShoot(MyShootActionEnum action)
    {
      if (this.IsWaitingForWeaponSwitch)
        return;
      if (MySessionComponentReplay.Static.IsEntityBeingRecorded(this.CubeGrid.EntityId))
      {
        PerFrameData data = new PerFrameData()
        {
          ShootData = new ShootData?(new ShootData()
          {
            Begin = true,
            ShootAction = (byte) action
          })
        };
        MySessionComponentReplay.Static.ProvideEntityRecordData(this.CubeGrid.EntityId, data);
      }
      MyGunStatusEnum status = MyGunStatusEnum.OK;
      IMyGunObject<MyDeviceBase> FailedGun = (IMyGunObject<MyDeviceBase>) null;
      this.GridSelectionSystem.CanShoot(action, out status, out FailedGun);
      if (status != MyGunStatusEnum.OK)
        this.ShowShootNotification(status, FailedGun);
      this.BeginShootSync(action);
    }

    protected void EndShootAll()
    {
      foreach (MyShootActionEnum action in MyEnum<MyShootActionEnum>.Values)
      {
        if (this.IsShooting(action))
          this.EndShoot(action);
      }
    }

    private void StopCurrentWeaponShooting()
    {
      foreach (MyShootActionEnum action in MyEnum<MyShootActionEnum>.Values)
      {
        if (this.IsShooting(action))
          this.GridSelectionSystem.EndShoot(action);
      }
    }

    public void EndShoot(MyShootActionEnum action)
    {
      if (MySessionComponentReplay.Static.IsEntityBeingRecorded(this.CubeGrid.EntityId))
      {
        PerFrameData data = new PerFrameData()
        {
          ShootData = new ShootData?(new ShootData()
          {
            Begin = false,
            ShootAction = (byte) action
          })
        };
        MySessionComponentReplay.Static.ProvideEntityRecordData(this.CubeGrid.EntityId, data);
      }
      if (this.BuildingMode && this.Pilot != null)
        this.Pilot.EndShoot(action);
      this.EndShootSync(action);
    }

    public void OnBeginShoot(MyShootActionEnum action)
    {
      MyGunStatusEnum status = MyGunStatusEnum.OK;
      IMyGunObject<MyDeviceBase> FailedGun = (IMyGunObject<MyDeviceBase>) null;
      if (!this.GridSelectionSystem.CanShoot(action, out status, out FailedGun) && status != MyGunStatusEnum.OK && status != MyGunStatusEnum.Cooldown)
        this.ShootBeginFailed(action, status, FailedGun);
      else
        this.GridSelectionSystem.BeginShoot(action);
    }

    public void OnEndShoot(MyShootActionEnum action) => this.GridSelectionSystem.EndShoot(action);

    private void ShootBeginFailed(
      MyShootActionEnum action,
      MyGunStatusEnum status,
      IMyGunObject<MyDeviceBase> failedGun)
    {
      failedGun?.BeginFailReaction(action, status);
      if (MySession.Static.ControlledEntity == null || this.CubeGrid != ((MyEntity) MySession.Static.ControlledEntity).GetTopMostParent((System.Type) null))
        return;
      failedGun.BeginFailReactionLocal(action, status);
    }

    protected override void Closing()
    {
      if (MyFakes.ENABLE_NEW_SOUNDS)
        this.StopLoopSound();
      this.IsMainCockpit = false;
      if (!this.CubeGrid.MarkedForClose)
        this.CubeGrid.OnGridSplit -= new Action<MyCubeGrid, MyCubeGrid>(this.CubeGrid_OnGridSplit);
      if (this.m_soundEmitter != null)
      {
        this.m_soundEmitter.StopSound(true);
        this.m_soundEmitter = (MyEntity3DSoundEmitter) null;
      }
      base.Closing();
    }

    protected virtual void UpdateSoundState()
    {
    }

    protected virtual void StartLoopSound()
    {
    }

    protected virtual void StopLoopSound()
    {
    }

    public void RemoveUsers(bool local)
    {
      if (local)
        this.RemoveLocal();
      else
        this.RaiseControlledEntityUsed();
    }

    protected virtual void RemoveLocal()
    {
    }

    internal void SwitchWeaponMode() => this.SingleWeaponMode = !this.SingleWeaponMode;

    public bool SingleWeaponMode
    {
      get => this.m_singleWeaponMode;
      private set
      {
        if (this.m_singleWeaponMode == value)
          return;
        this.m_singleWeaponMode = value;
        if (this.m_selectedGunId.HasValue)
          this.SwitchToWeapon(this.m_selectedGunId.Value);
        else
          this.SwitchToWeapon((MyToolbarItemWeapon) null);
      }
    }

    public bool IsMainCockpit
    {
      get => (bool) this.m_isMainCockpit;
      set => this.m_isMainCockpit.Value = value;
    }

    bool Sandbox.ModAPI.Ingame.IMyShipController.IsMainCockpit
    {
      get => this.IsMainCockpit;
      set
      {
        if (!this.IsMainCockpitFree() || !this.CanBeMainCockpit())
          return;
        this.IsMainCockpit = value;
      }
    }

    private void MainCockpitChanged()
    {
      if ((bool) this.m_isMainCockpit)
      {
        this.CubeGrid.SetMainCockpit((MyTerminalBlock) this);
      }
      else
      {
        if (!this.CubeGrid.IsMainCockpit((MyTerminalBlock) this))
          return;
        this.CubeGrid.SetMainCockpit((MyTerminalBlock) null);
      }
    }

    protected virtual bool CanBeMainCockpit() => false;

    protected virtual bool CanHaveHorizon() => this.BlockDefinition.EnableShipControl;

    protected bool IsMainCockpitFree() => !this.CubeGrid.HasMainCockpit() || this.CubeGrid.IsMainCockpit((MyTerminalBlock) this);

    protected bool IsControllingCockpit() => this.IsMainCockpitFree() || this.m_mainCockpitOverwritten;

    public bool HorizonIndicatorEnabled
    {
      get => (bool) this.m_horizonIndicatorEnabled && this.CanHaveHorizon();
      set => this.m_horizonIndicatorEnabled.Value = value;
    }

    public virtual MyToolbarType ToolbarType => !this.m_enableShipControl ? MyToolbarType.Seat : MyToolbarType.Ship;

    bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.ForceFirstPersonCamera
    {
      get => this.ForceFirstPersonCamera;
      set => this.ForceFirstPersonCamera = value;
    }

    MatrixD VRage.Game.ModAPI.Interfaces.IMyControllableEntity.GetHeadMatrix(
      bool includeY,
      bool includeX,
      bool forceHeadAnim,
      bool forceHeadBone)
    {
      return this.GetHeadMatrix(includeY, includeX, forceHeadAnim, false);
    }

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.MoveAndRotate(
      Vector3 moveIndicator,
      Vector2 rotationIndicator,
      float rollIndicator)
    {
      this.MoveAndRotate(moveIndicator, rotationIndicator, rollIndicator);
    }

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.MoveAndRotateStopped() => this.MoveAndRotateStopped();

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Use() => this.Use();

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.UseContinues() => this.UseContinues();

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.PickUp() => this.PickUp();

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.PickUpContinues() => this.PickUpContinues();

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Jump(
      Vector3 moveIndicator)
    {
      this.Jump(moveIndicator);
    }

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Up() => this.Up();

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Crouch() => this.Crouch();

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Down() => this.Down();

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.ShowInventory() => this.ShowInventory();

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.ShowTerminal() => this.ShowTerminal();

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchThrusts() => this.SwitchThrusts();

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchDamping() => this.SwitchDamping();

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchLights() => this.SwitchLights();

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchLandingGears() => this.SwitchLandingGears();

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchHandbrake() => this.SwitchHandbrake();

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchReactors() => this.SwitchReactors();

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchHelmet()
    {
      if (this.Pilot == null || !Sandbox.Game.Multiplayer.Sync.IsServer && MySession.Static.LocalCharacter != this.Pilot)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyShipController>(this, (Func<MyShipController, Action>) (x => new Action(x.OnSwitchHelmet)));
    }

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Die() => this.Die();

    bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledThrusts => false;

    bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledDamping => this.EntityThrustComponent != null && this.EntityThrustComponent.DampenersEnabled;

    bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledLights => this.GridReflectorLights.ReflectorsEnabled == MyMultipleEnabledEnum.AllEnabled;

    bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledLeadingGears
    {
      get
      {
        MyMultipleEnabledEnum locked = this.CubeGrid.GridSystems.LandingSystem.Locked;
        return locked == MyMultipleEnabledEnum.Mixed || locked == MyMultipleEnabledEnum.AllEnabled;
      }
    }

    bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledReactors => this.GridResourceDistributor != null && this.GridResourceDistributor.SourcesEnabled != MyMultipleEnabledEnum.AllDisabled;

    bool IMyControllableEntity.EnabledBroadcasting => false;

    bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledHelmet => false;

    void IMyControllableEntity.SwitchAmmoMagazine()
    {
      if (!this.m_enableShipControl || !this.GridSelectionSystem.CanSwitchAmmoMagazine())
        return;
      this.SwitchAmmoMagazineInternal(true);
    }

    bool IMyControllableEntity.CanSwitchAmmoMagazine() => this.m_selectedGunId.HasValue && this.GridSelectionSystem.CanSwitchAmmoMagazine();

    public virtual float HeadLocalXAngle { get; set; }

    public virtual float HeadLocalYAngle { get; set; }

    bool Sandbox.ModAPI.Ingame.IMyShipController.IsUnderControl => this.ControllerInfo.Controller != null;

    bool Sandbox.ModAPI.Ingame.IMyShipController.ControlWheels
    {
      get => this.ControlWheels;
      set
      {
        if (!this.m_enableShipControl || this.GridWheels.WheelCount <= 0 || !this.IsMainCockpitFree())
          return;
        this.ControlWheels = value;
      }
    }

    bool Sandbox.ModAPI.Ingame.IMyShipController.ControlThrusters
    {
      get => this.ControlThrusters;
      set
      {
        if (!this.m_enableShipControl || !this.IsMainCockpitFree())
          return;
        this.ControlThrusters = value;
      }
    }

    bool Sandbox.ModAPI.Ingame.IMyShipController.HandBrake
    {
      get => this.CubeGrid.GridSystems.WheelSystem.HandBrake;
      set
      {
        if (!this.m_enableShipControl || this.GridWheels.WheelCount <= 0 || (!this.IsMainCockpitFree() || this.CubeGrid.GridSystems.WheelSystem.HandBrake == value))
          return;
        this.SwitchHandbrake();
      }
    }

    bool Sandbox.ModAPI.Ingame.IMyShipController.DampenersOverride
    {
      get => this.EntityThrustComponent != null && this.EntityThrustComponent.DampenersEnabled;
      set
      {
        if (!this.m_enableShipControl)
          return;
        this.CubeGrid.EnableDampingInternal(value, true);
      }
    }

    Vector3 Sandbox.ModAPI.Ingame.IMyShipController.MoveIndicator => this.MoveIndicator;

    Vector2 Sandbox.ModAPI.Ingame.IMyShipController.RotationIndicator => this.RotationIndicator;

    float Sandbox.ModAPI.Ingame.IMyShipController.RollIndicator => this.RollIndicator;

    Vector3D Sandbox.ModAPI.Ingame.IMyShipController.CenterOfMass => this.CubeGrid.Physics.CenterOfMassWorld;

    private void CubeGrid_OnGridSplit(MyCubeGrid grid1, MyCubeGrid grid2)
    {
      this.CheckGridCokpit(grid1);
      this.CheckGridCokpit(grid2);
    }

    private bool HasCockpit(MyCubeGrid grid) => grid.CubeBlocks.Contains(this.SlimBlock);

    private void CheckGridCokpit(MyCubeGrid grid)
    {
      if (this.HasCockpit(grid) || !grid.IsMainCockpit((MyTerminalBlock) this) || this.CubeGrid == grid)
        return;
      grid.SetMainCockpit((MyTerminalBlock) null);
    }

    public MyEntityCameraSettings GetCameraEntitySettings() => (MyEntityCameraSettings) null;

    public MyStringId ControlContext => MySpaceBindingCreator.CX_SPACESHIP;

    public MyStringId AuxiliaryContext
    {
      get
      {
        if (MyCubeBuilder.Static.IsActivated)
          return MySpaceBindingCreator.AX_BUILD;
        if (MySessionComponentVoxelHand.Static.Enabled)
          return MySpaceBindingCreator.AX_VOXEL;
        return MyClipboardComponent.Static.IsActive ? MySpaceBindingCreator.AX_CLIPBOARD : MySpaceBindingCreator.AX_ACTIONS;
      }
    }

    public override void SetDamageEffect(bool show)
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      base.SetDamageEffect(show);
      if (this.m_soundEmitter == null || this.BlockDefinition.DamagedSound == null)
        return;
      if (show)
      {
        this.m_soundEmitter.PlaySound(this.BlockDefinition.DamagedSound, true);
      }
      else
      {
        if (!(this.m_soundEmitter.SoundId == this.BlockDefinition.DamagedSound.Arcade) && !(this.m_soundEmitter.SoundId != this.BlockDefinition.DamagedSound.Realistic))
          return;
        this.m_soundEmitter.StopSound(false);
      }
    }

    public override void StopDamageEffect(bool stopSound = true)
    {
      base.StopDamageEffect(stopSound);
      if (!stopSound || this.m_soundEmitter == null || this.BlockDefinition.DamagedSound == null || !(this.m_soundEmitter.SoundId == this.BlockDefinition.DamagedSound.Arcade) && !(this.m_soundEmitter.SoundId != this.BlockDefinition.DamagedSound.Realistic))
        return;
      this.m_soundEmitter.StopSound(true);
    }

    private void RequestSwitchToWeapon(
      MyDefinitionId? weapon,
      MyObjectBuilder_EntityBase weaponObjectBuilder,
      long weaponEntityId)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        ++this.m_switchWeaponCounter;
      MyDefinitionId? nullable = weapon;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyShipController, SerializableDefinitionId?, MyObjectBuilder_EntityBase, long>(this, (Func<MyShipController, Action<SerializableDefinitionId?, MyObjectBuilder_EntityBase, long>>) (x => new Action<SerializableDefinitionId?, MyObjectBuilder_EntityBase, long>(x.SwitchToWeaponMessage)), nullable.HasValue ? new SerializableDefinitionId?((SerializableDefinitionId) nullable.GetValueOrDefault()) : new SerializableDefinitionId?(), weaponObjectBuilder, weaponEntityId);
    }

    [Event(null, 2578)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void SwitchToWeaponMessage(
      SerializableDefinitionId? weapon,
      [Serialize(MyObjectFlags.DefaultZero | MyObjectFlags.Dynamic, DynamicSerializerType = typeof (MyObjectBuilderDynamicSerializer))] MyObjectBuilder_EntityBase weaponObjectBuilder,
      long weaponEntityId)
    {
      SerializableDefinitionId? nullable = weapon;
      if (!this.CanSwitchToWeapon(nullable.HasValue ? new MyDefinitionId?((MyDefinitionId) nullable.GetValueOrDefault()) : new MyDefinitionId?()))
      {
        if (MyEventContext.Current.IsLocallyInvoked)
          this.OnSwitchToWeaponFailure(weapon, weaponObjectBuilder, weaponEntityId);
        else
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyShipController, SerializableDefinitionId?, MyObjectBuilder_EntityBase, long>(this, (Func<MyShipController, Action<SerializableDefinitionId?, MyObjectBuilder_EntityBase, long>>) (x => new Action<SerializableDefinitionId?, MyObjectBuilder_EntityBase, long>(x.OnSwitchToWeaponFailure)), weapon, weaponObjectBuilder, weaponEntityId, MyEventContext.Current.Sender);
      }
      else
      {
        if (weaponObjectBuilder != null && weaponObjectBuilder.EntityId == 0L)
        {
          weaponObjectBuilder = (MyObjectBuilder_EntityBase) weaponObjectBuilder.Clone();
          weaponObjectBuilder.EntityId = weaponEntityId == 0L ? MyEntityIdentifier.AllocateId() : weaponEntityId;
        }
        this.OnSwitchToWeaponSuccess(weapon, weaponObjectBuilder, weaponEntityId);
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyShipController, SerializableDefinitionId?, MyObjectBuilder_EntityBase, long>(this, (Func<MyShipController, Action<SerializableDefinitionId?, MyObjectBuilder_EntityBase, long>>) (x => new Action<SerializableDefinitionId?, MyObjectBuilder_EntityBase, long>(x.OnSwitchToWeaponSuccess)), weapon, weaponObjectBuilder, weaponEntityId);
      }
    }

    [Event(null, 2605)]
    [Reliable]
    [Client]
    private void OnSwitchToWeaponFailure(
      SerializableDefinitionId? weapon,
      [Serialize(MyObjectFlags.DefaultZero | MyObjectFlags.Dynamic, DynamicSerializerType = typeof (MyObjectBuilderDynamicSerializer))] MyObjectBuilder_EntityBase weaponObjectBuilder,
      long weaponEntityId)
    {
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      --this.m_switchWeaponCounter;
    }

    [Event(null, 2614)]
    [Reliable]
    [Broadcast]
    private void OnSwitchToWeaponSuccess(
      SerializableDefinitionId? weapon,
      [Serialize(MyObjectFlags.DefaultZero | MyObjectFlags.Dynamic, DynamicSerializerType = typeof (MyObjectBuilderDynamicSerializer))] MyObjectBuilder_EntityBase weaponObjectBuilder,
      long weaponEntityId)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer && this.m_switchWeaponCounter > 0)
        --this.m_switchWeaponCounter;
      SerializableDefinitionId? nullable = weapon;
      this.SwitchToWeaponSuccess(nullable.HasValue ? new MyDefinitionId?((MyDefinitionId) nullable.GetValueOrDefault()) : new MyDefinitionId?(), (MyObjectBuilder_Base) weaponObjectBuilder, weaponEntityId);
    }

    [Event(null, 2630)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void OnSwitchAmmoMagazineRequest()
    {
      if (!((IMyControllableEntity) this).CanSwitchAmmoMagazine())
        return;
      this.SwitchAmmoMagazineSuccess();
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyShipController>(this, (Func<MyShipController, Action>) (x => new Action(x.OnSwitchAmmoMagazineSuccess)));
    }

    [Event(null, 2642)]
    [Reliable]
    [Broadcast]
    private void OnSwitchAmmoMagazineSuccess() => this.SwitchAmmoMagazineSuccess();

    public void BeginShootSync(MyShootActionEnum action = MyShootActionEnum.PrimaryAction)
    {
      this.StartShooting(action);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyShipController, MyShootActionEnum>(this, (Func<MyShipController, Action<MyShootActionEnum>>) (x => new Action<MyShootActionEnum>(x.ShootBeginCallback)), action);
      if (!MyFakes.SIMULATE_QUICK_TRIGGER)
        return;
      this.EndShootInternal(action);
    }

    [Event(null, 2658)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [BroadcastExcept]
    private void ShootBeginCallback(MyShootActionEnum action)
    {
      if ((!Sandbox.Game.Multiplayer.Sync.IsServer ? 0 : (MyEventContext.Current.IsLocallyInvoked ? 1 : 0)) != 0)
        return;
      this.StartShooting(action);
    }

    private void StartShooting(MyShootActionEnum action)
    {
      this.m_isShooting[(int) action] = true;
      this.OnBeginShoot(action);
    }

    private void StopShooting(MyShootActionEnum action)
    {
      this.m_isShooting[(int) action] = false;
      this.OnEndShoot(action);
    }

    public void EndShootSync(MyShootActionEnum action = MyShootActionEnum.PrimaryAction)
    {
      if (MyFakes.SIMULATE_QUICK_TRIGGER)
        return;
      this.EndShootInternal(action);
    }

    private void EndShootInternal(MyShootActionEnum action = MyShootActionEnum.PrimaryAction)
    {
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyShipController, MyShootActionEnum>(this, (Func<MyShipController, Action<MyShootActionEnum>>) (x => new Action<MyShootActionEnum>(x.ShootEndCallback)), action);
      this.StopShooting(action);
    }

    [Event(null, 2694)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [BroadcastExcept]
    private void ShootEndCallback(MyShootActionEnum action)
    {
      if ((!Sandbox.Game.Multiplayer.Sync.IsServer ? 0 : (MyEventContext.Current.IsLocallyInvoked ? 1 : 0)) != 0)
        return;
      this.StopShooting(action);
    }

    private void Toolbar_ItemChanged(MyToolbar self, MyToolbar.IndexArgs index, bool isGamepad)
    {
      if (this.m_syncing)
        return;
      MyToolbarItem myToolbarItem = !isGamepad ? self.GetItemAtIndex(index.ItemIndex) : self.GetItemAtLinearIndexGamepad(index.ItemIndex);
      if (myToolbarItem != null)
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyShipController, MyObjectBuilder_ToolbarItem, int, bool>(this, (Func<MyShipController, Action<MyObjectBuilder_ToolbarItem, int, bool>>) (x => new Action<MyObjectBuilder_ToolbarItem, int, bool>(x.SendToolbarItemChanged)), myToolbarItem.GetObjectBuilder(), index.ItemIndex, isGamepad);
      else
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyShipController, int, bool>(this, (Func<MyShipController, Action<int, bool>>) (x => new Action<int, bool>(x.SendToolbarItemRemoved)), index.ItemIndex, isGamepad);
    }

    [Event(null, 2729)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void SendToolbarItemRemoved(int index, bool isGamepad)
    {
      this.m_syncing = true;
      this.Toolbar.SetItemAtIndex(index, (MyToolbarItem) null, isGamepad);
      this.m_syncing = false;
    }

    [Event(null, 2737)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void SendToolbarItemChanged(
      [DynamicObjectBuilder(false)] MyObjectBuilder_ToolbarItem sentItem,
      int index,
      bool isGamepad)
    {
      this.m_syncing = true;
      MyToolbarItem myToolbarItem = (MyToolbarItem) null;
      if (sentItem != null)
        myToolbarItem = MyToolbarItemFactory.CreateToolbarItem(sentItem);
      this.Toolbar.SetItemAtIndex(index, myToolbarItem, isGamepad);
      this.m_syncing = false;
    }

    public MyGridClientState GetNetState() => new MyGridClientState()
    {
      Move = this.MoveIndicator,
      Rotation = this.RotationIndicator,
      Roll = this.RollIndicator
    };

    public void SetNetState(MyGridClientState netState) => this.MoveAndRotate(netState.Move, netState.Rotation, netState.Roll);

    public void RemoveControlSystem(MyGroupControlSystem controlSystem) => this.m_controlSystems.Remove(controlSystem);

    public void AddControlSystem(MyGroupControlSystem controlSystem) => this.m_controlSystems.Add(controlSystem);

    public bool ShouldEndShootingOnPause(MyShootActionEnum action) => true;

    public MyEntity RelativeDampeningEntity
    {
      get => this.CubeGrid.GridSystems.ControlSystem.RelativeDampeningEntity;
      set => this.CubeGrid.GridSystems.ControlSystem.RelativeDampeningEntity = value;
    }

    public bool TryEnableBrakes(bool enable)
    {
      if (!MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT || this.GridWheels == null || (!this.ControlWheels || !this.EnableShipControl) || !this.IsMainCockpit && this.CubeGrid.HasMainCockpit())
        return false;
      this.CubeGrid.GridSystems.WheelSystem.Brake = enable;
      return true;
    }

    public MyWelder.ProjectionRaycastData FindProjectedBlock()
    {
      if (this.m_raycaster != null)
        return MyWelder.FindProjectedBlock(this.m_raycaster, 2f);
      return new MyWelder.ProjectionRaycastData()
      {
        raycastResult = BuildCheckResult.NotFound
      };
    }

    public bool IsDefault3rdView => this.BlockDefinition.IsDefault3rdView;

    VRage.ModAPI.IMyEntity VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Entity => (VRage.ModAPI.IMyEntity) this.Entity;

    public Vector3 LastMotionIndicator { get; set; }

    public Vector3 LastRotationIndicator { get; set; }

    IMyControllerInfo VRage.Game.ModAPI.Interfaces.IMyControllableEntity.ControllerInfo => (IMyControllerInfo) this.ControllerInfo;

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.DrawHud(
      IMyCameraController camera,
      long playerId)
    {
      if (camera == null)
        return;
      this.DrawHud(camera, playerId);
    }

    bool Sandbox.ModAPI.Ingame.IMyShipController.CanControlShip => this.EnableShipControl;

    bool Sandbox.ModAPI.Ingame.IMyShipController.HasWheels => this.GridWheels.WheelCount > 0;

    public Vector3D GetNaturalGravity() => (Vector3D) MyGravityProviderSystem.CalculateNaturalGravityInPoint(this.WorldMatrix.Translation);

    public Vector3D GetArtificialGravity() => (Vector3D) MyGravityProviderSystem.CalculateArtificialGravityInPoint(this.WorldMatrix.Translation);

    public Vector3D GetTotalGravity() => (Vector3D) MyGravityProviderSystem.CalculateTotalGravityInPoint(this.WorldMatrix.Translation);

    double Sandbox.ModAPI.Ingame.IMyShipController.GetShipSpeed()
    {
      MyPhysicsComponentBase physicsComponentBase = this.Parent != null ? this.Parent.Physics : (MyPhysicsComponentBase) null;
      return (physicsComponentBase == null ? Vector3D.Zero : new Vector3D(physicsComponentBase.LinearVelocity)).Length();
    }

    MyShipVelocities Sandbox.ModAPI.Ingame.IMyShipController.GetShipVelocities()
    {
      MyPhysicsComponentBase physicsComponentBase = this.Parent != null ? this.Parent.Physics : (MyPhysicsComponentBase) null;
      return new MyShipVelocities(physicsComponentBase == null ? Vector3D.Zero : new Vector3D(physicsComponentBase.LinearVelocity), physicsComponentBase == null ? Vector3D.Zero : new Vector3D(physicsComponentBase.AngularVelocity));
    }

    public MyShipMass CalculateShipMass()
    {
      float baseMass;
      float physicalMass;
      float currentMass = this.CubeGrid.GetCurrentMass(out baseMass, out physicalMass);
      return new MyShipMass(baseMass, currentMass, physicalMass);
    }

    bool Sandbox.ModAPI.Ingame.IMyShipController.TryGetPlanetElevation(
      MyPlanetElevation detail,
      out double elevation)
    {
      if (!MyGravityProviderSystem.IsPositionInNaturalGravity(this.PositionComp.GetPosition()))
      {
        elevation = double.PositiveInfinity;
        return false;
      }
      BoundingBoxD worldAabb = this.PositionComp.WorldAABB;
      MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(ref worldAabb);
      if (closestPlanet == null)
      {
        elevation = double.PositiveInfinity;
        return false;
      }
      if (detail != MyPlanetElevation.Sealevel)
      {
        if (detail != MyPlanetElevation.Surface)
          throw new ArgumentOutOfRangeException(nameof (detail), (object) detail, (string) null);
        Vector3D centerOfMassWorld = this.CubeGrid.Physics.CenterOfMassWorld;
        Vector3D surfacePointGlobal = closestPlanet.GetClosestSurfacePointGlobal(ref centerOfMassWorld);
        elevation = Vector3D.Distance(surfacePointGlobal, centerOfMassWorld);
        return true;
      }
      elevation = (worldAabb.Center - closestPlanet.PositionComp.GetPosition()).Length() - (double) closestPlanet.AverageRadius;
      return true;
    }

    bool Sandbox.ModAPI.Ingame.IMyShipController.ShowHorizonIndicator
    {
      get => this.HorizonIndicatorEnabled;
      set
      {
        if (!this.CanHaveHorizon())
          return;
        this.HorizonIndicatorEnabled = value;
      }
    }

    bool Sandbox.ModAPI.Ingame.IMyShipController.TryGetPlanetPosition(
      out Vector3D position)
    {
      if (!MyGravityProviderSystem.IsPositionInNaturalGravity(this.PositionComp.GetPosition()))
      {
        position = Vector3D.Zero;
        return false;
      }
      BoundingBoxD worldAabb = this.PositionComp.WorldAABB;
      MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(ref worldAabb);
      if (closestPlanet == null)
      {
        position = Vector3D.Zero;
        return false;
      }
      position = closestPlanet.PositionComp.GetPosition();
      return true;
    }

    Vector3 Sandbox.ModAPI.IMyShipController.MoveIndicator => this.MoveIndicator;

    Vector2 Sandbox.ModAPI.IMyShipController.RotationIndicator => this.RotationIndicator;

    float Sandbox.ModAPI.IMyShipController.RollIndicator => this.RollIndicator;

    bool Sandbox.ModAPI.IMyShipController.HasFirstPersonCamera => this.EnableFirstPersonView;

    IMyCharacter Sandbox.ModAPI.IMyShipController.Pilot => (IMyCharacter) this.Pilot;

    IMyCharacter Sandbox.ModAPI.IMyShipController.LastPilot => (IMyCharacter) this.m_lastPilot;

    bool Sandbox.ModAPI.IMyShipController.IsShooting => this.IsShooting();

    protected sealed class sync_ControlledEntity_Used\u003C\u003E : ICallSite<MyShipController, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyShipController @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.sync_ControlledEntity_Used();
      }
    }

    protected sealed class OnSwitchHelmet\u003C\u003E : ICallSite<MyShipController, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyShipController @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnSwitchHelmet();
      }
    }

    protected sealed class SwitchToWeaponMessage\u003C\u003ESystem_Nullable`1\u003CVRage_ObjectBuilders_SerializableDefinitionId\u003E\u0023VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u0023System_Int64 : ICallSite<MyShipController, SerializableDefinitionId?, MyObjectBuilder_EntityBase, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyShipController @this,
        in SerializableDefinitionId? weapon,
        in MyObjectBuilder_EntityBase weaponObjectBuilder,
        in long weaponEntityId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.SwitchToWeaponMessage(weapon, weaponObjectBuilder, weaponEntityId);
      }
    }

    protected sealed class OnSwitchToWeaponFailure\u003C\u003ESystem_Nullable`1\u003CVRage_ObjectBuilders_SerializableDefinitionId\u003E\u0023VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u0023System_Int64 : ICallSite<MyShipController, SerializableDefinitionId?, MyObjectBuilder_EntityBase, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyShipController @this,
        in SerializableDefinitionId? weapon,
        in MyObjectBuilder_EntityBase weaponObjectBuilder,
        in long weaponEntityId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnSwitchToWeaponFailure(weapon, weaponObjectBuilder, weaponEntityId);
      }
    }

    protected sealed class OnSwitchToWeaponSuccess\u003C\u003ESystem_Nullable`1\u003CVRage_ObjectBuilders_SerializableDefinitionId\u003E\u0023VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u0023System_Int64 : ICallSite<MyShipController, SerializableDefinitionId?, MyObjectBuilder_EntityBase, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyShipController @this,
        in SerializableDefinitionId? weapon,
        in MyObjectBuilder_EntityBase weaponObjectBuilder,
        in long weaponEntityId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnSwitchToWeaponSuccess(weapon, weaponObjectBuilder, weaponEntityId);
      }
    }

    protected sealed class OnSwitchAmmoMagazineRequest\u003C\u003E : ICallSite<MyShipController, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyShipController @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnSwitchAmmoMagazineRequest();
      }
    }

    protected sealed class OnSwitchAmmoMagazineSuccess\u003C\u003E : ICallSite<MyShipController, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyShipController @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnSwitchAmmoMagazineSuccess();
      }
    }

    protected sealed class ShootBeginCallback\u003C\u003ESandbox_Game_Entities_MyShootActionEnum : ICallSite<MyShipController, MyShootActionEnum, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyShipController @this,
        in MyShootActionEnum action,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ShootBeginCallback(action);
      }
    }

    protected sealed class ShootEndCallback\u003C\u003ESandbox_Game_Entities_MyShootActionEnum : ICallSite<MyShipController, MyShootActionEnum, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyShipController @this,
        in MyShootActionEnum action,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ShootEndCallback(action);
      }
    }

    protected sealed class SendToolbarItemRemoved\u003C\u003ESystem_Int32\u0023System_Boolean : ICallSite<MyShipController, int, bool, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyShipController @this,
        in int index,
        in bool isGamepad,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.SendToolbarItemRemoved(index, isGamepad);
      }
    }

    protected sealed class SendToolbarItemChanged\u003C\u003EVRage_Game_MyObjectBuilder_ToolbarItem\u0023System_Int32\u0023System_Boolean : ICallSite<MyShipController, MyObjectBuilder_ToolbarItem, int, bool, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyShipController @this,
        in MyObjectBuilder_ToolbarItem sentItem,
        in int index,
        in bool isGamepad,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.SendToolbarItemChanged(sentItem, index, isGamepad);
      }
    }

    protected class m_controlThrusters\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyShipController) obj0).m_controlThrusters = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_controlWheels\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyShipController) obj0).m_controlWheels = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_controlGyros\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyShipController) obj0).m_controlGyros = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_isMainCockpit\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyShipController) obj0).m_isMainCockpit = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_horizonIndicatorEnabled\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyShipController) obj0).m_horizonIndicatorEnabled = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_MyShipController\u003C\u003EActor : IActivator, IActivator<MyShipController>
    {
      object IActivator.CreateInstance() => (object) new MyShipController();

      MyShipController IActivator<MyShipController>.CreateInstance() => new MyShipController();
    }
  }
}
