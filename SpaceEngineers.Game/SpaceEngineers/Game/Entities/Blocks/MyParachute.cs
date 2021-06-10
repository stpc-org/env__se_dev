// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.Blocks.MyParachute
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_Parachute))]
  [MyTerminalInterface(new Type[] {typeof (SpaceEngineers.Game.ModAPI.IMyParachute), typeof (SpaceEngineers.Game.ModAPI.Ingame.IMyParachute)})]
  public class MyParachute : MyDoorBase, SpaceEngineers.Game.ModAPI.IMyParachute, SpaceEngineers.Game.ModAPI.Ingame.IMyParachute, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyConveyorEndpointBlock
  {
    private const float MIN_DEPLOY_HEIGHT = 10f;
    private const float MAX_DEPLOY_HEIGHT = 10000f;
    private const double DENSITY_OF_AIR_IN_ONE_ATMO = 1.225;
    private const float NO_DRAG_SPEED_SQRD = 0.1f;
    private const float NO_DRAG_SPEED_RANGE = 20f;
    private bool m_stateChange;
    private List<MyEntity3DSoundEmitter> m_emitter = new List<MyEntity3DSoundEmitter>();
    private MyMultilineConveyorEndpoint m_conveyorEndpoint;
    protected readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_autoDeploy;
    protected readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_deployHeight;
    private MyPlanet m_nearPlanetCache;
    private MyEntitySubpart m_parachuteSubpart;
    private Vector3 m_lastParachuteVelocityVector = Vector3.Zero;
    private Vector3 m_lastParachuteScale = Vector3.Zero;
    private Vector3 m_gravityCache = Vector3.Zero;
    private Vector3D m_chuteScale = Vector3D.Zero;
    private Vector3D? m_closestPointCache;
    private int m_parachuteAnimationState;
    private int m_cutParachuteTimer;
    private bool m_canDeploy;
    private bool m_canCheckAutoDeploy;
    private bool m_atmosphereDirty = true;
    private float m_minAtmosphere = 0.2f;
    private float m_dragCoefficient = 1f;
    private float m_atmosphereDensityCache;
    private MyFixedPoint m_requiredItemsInInventory = (MyFixedPoint) 0;
    private Quaternion m_lastParachuteRotation = Quaternion.Identity;
    private Matrix m_lastParachuteLocalMatrix = Matrix.Identity;
    private MatrixD m_lastParachuteWorldMatrix = MatrixD.Identity;

    public MyParachute()
    {
      this.m_emitter.Clear();
      this.m_open.ValueChanged += (Action<SyncBase>) (x => this.OnStateChange());
    }

    public IMyConveyorEndpoint ConveyorEndpoint => (IMyConveyorEndpoint) this.m_conveyorEndpoint;

    DoorStatus SpaceEngineers.Game.ModAPI.Ingame.IMyParachute.Status => (bool) this.m_open ? DoorStatus.Open : DoorStatus.Closed;

    public float OpenRatio => (bool) this.m_open ? 1f : 0.0f;

    void SpaceEngineers.Game.ModAPI.Ingame.IMyParachute.OpenDoor()
    {
      if (!this.IsWorking || (bool) this.m_open)
        return;
      ((SpaceEngineers.Game.ModAPI.Ingame.IMyParachute) this).ToggleDoor();
    }

    void SpaceEngineers.Game.ModAPI.Ingame.IMyParachute.CloseDoor()
    {
      if (!this.IsWorking || !(bool) this.m_open)
        return;
      ((SpaceEngineers.Game.ModAPI.Ingame.IMyParachute) this).ToggleDoor();
    }

    void SpaceEngineers.Game.ModAPI.Ingame.IMyParachute.ToggleDoor()
    {
      if (!this.IsWorking)
        return;
      this.SetOpenRequest(!this.Open, this.OwnerId);
    }

    public bool AutoDeploy
    {
      get => (bool) this.m_autoDeploy;
      set
      {
        if ((bool) this.m_autoDeploy == value)
          return;
        this.m_autoDeploy.Value = value;
      }
    }

    public float DeployHeight
    {
      get => (float) this.m_deployHeight;
      set
      {
        value = MathHelper.Clamp(value, 10f, 10000f);
        if ((double) (float) this.m_deployHeight == (double) value)
          return;
        this.m_deployHeight.Value = value;
      }
    }

    public float DragCoefficient => this.m_dragCoefficient;

    public bool CanDeploy
    {
      get => this.IsWorking && this.Enabled && this.m_canDeploy;
      set => this.m_canDeploy = value;
    }

    public float Atmosphere
    {
      get
      {
        if (!this.m_atmosphereDirty)
          return this.m_atmosphereDensityCache;
        this.m_atmosphereDirty = false;
        return this.m_nearPlanetCache == null ? (this.m_atmosphereDensityCache = 0.0f) : (this.m_atmosphereDensityCache = this.m_nearPlanetCache.GetAirDensity(this.WorldMatrix.Translation));
      }
    }

    public override void UpdateVisual()
    {
      base.UpdateVisual();
      this.UpdateEmissivity();
    }

    protected override bool CheckIsWorking() => this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.CheckIsWorking();

    private void UpdateEmissivity()
    {
      if (this.Enabled && this.ResourceSink != null && this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
      {
        MyCubeBlock.UpdateEmissiveParts(this.Render.RenderObjectIDs[0], 1f, Color.Green, Color.White);
        this.OnStateChange();
      }
      else
        MyCubeBlock.UpdateEmissiveParts(this.Render.RenderObjectIDs[0], 0.0f, Color.Red, Color.White);
    }

    private MyParachuteDefinition BlockDefinition => (MyParachuteDefinition) base.BlockDefinition;

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyParachute>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlCheckbox<MyParachute> checkbox = new MyTerminalControlCheckbox<MyParachute>("AutoDeploy", MySpaceTexts.Parachute_AutoDeploy, MySpaceTexts.Parachute_AutoDeployTooltip, new MyStringId?(MySpaceTexts.Parachute_AutoDeployOn), new MyStringId?(MySpaceTexts.Parachute_AutoDeployOff));
      checkbox.Getter = (MyTerminalValueControl<MyParachute, bool>.GetterDelegate) (x => x.AutoDeploy);
      checkbox.Setter = (MyTerminalValueControl<MyParachute, bool>.SetterDelegate) ((x, v) => x.SetAutoDeployRequest(v, x.OwnerId));
      checkbox.EnableAction<MyParachute>();
      MyTerminalControlFactory.AddControl<MyParachute>((MyTerminalControl<MyParachute>) checkbox);
      MyTerminalControlSlider<MyParachute> terminalControlSlider = new MyTerminalControlSlider<MyParachute>("AutoDeployHeight", MySpaceTexts.Parachute_DeployHeightTitle, MySpaceTexts.Parachute_DeployHeightTooltip, true, true);
      terminalControlSlider.Getter = (MyTerminalValueControl<MyParachute, float>.GetterDelegate) (x => x.DeployHeight);
      terminalControlSlider.Setter = (MyTerminalValueControl<MyParachute, float>.SetterDelegate) ((x, v) => x.SetDeployHeightRequest(v, x.OwnerId));
      terminalControlSlider.Writer = (MyTerminalControl<MyParachute>.WriterDelegate) ((b, v) => v.Append(string.Format("{0:N0} m", (object) b.DeployHeight)));
      terminalControlSlider.SetLogLimits(10f, 10000f);
      MyTerminalControlFactory.AddControl<MyParachute>((MyTerminalControl<MyParachute>) terminalControlSlider);
    }

    public void SetAutoDeployRequest(bool autodeploy, long identityId) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyParachute, bool, long>(this, (Func<MyParachute, Action<bool, long>>) (x => new Action<bool, long>(x.AutoDeployRequest)), autodeploy, identityId);

    [Event(null, 257)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void AutoDeployRequest(bool autodeploy, long identityId)
    {
      MyRelationsBetweenPlayerAndBlock userRelationToOwner = this.GetUserRelationToOwner(identityId);
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(identityId);
      MyPlayer myPlayer = identity == null || identity.Character == null ? (MyPlayer) null : MyPlayer.GetPlayerFromCharacter(identity.Character);
      bool flag = false;
      AdminSettingsEnum adminSettingsEnum;
      if (myPlayer != null && !userRelationToOwner.IsFriendly() && MySession.Static.RemoteAdminSettings.TryGetValue(myPlayer.Client.SteamUserId, out adminSettingsEnum))
        flag = adminSettingsEnum.HasFlag((Enum) AdminSettingsEnum.UseTerminals);
      if (!(userRelationToOwner.IsFriendly() | flag))
        return;
      this.AutoDeploy = autodeploy;
    }

    public void SetDeployHeightRequest(float deployHeight, long identityId) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyParachute, float, long>(this, (Func<MyParachute, Action<float, long>>) (x => new Action<float, long>(x.DeployHeightRequest)), deployHeight, identityId);

    [Event(null, 283)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void DeployHeightRequest(float deployHeight, long identityId)
    {
      MyRelationsBetweenPlayerAndBlock userRelationToOwner = this.GetUserRelationToOwner(identityId);
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(identityId);
      MyPlayer myPlayer = identity == null || identity.Character == null ? (MyPlayer) null : MyPlayer.GetPlayerFromCharacter(identity.Character);
      bool flag = false;
      AdminSettingsEnum adminSettingsEnum;
      if (myPlayer != null && !userRelationToOwner.IsFriendly() && MySession.Static.RemoteAdminSettings.TryGetValue(myPlayer.Client.SteamUserId, out adminSettingsEnum))
        flag = adminSettingsEnum.HasFlag((Enum) AdminSettingsEnum.UseTerminals);
      if (!(userRelationToOwner.IsFriendly() | flag))
        return;
      this.DeployHeight = deployHeight;
    }

    private void OnStateChange()
    {
      this.ResourceSink.Update();
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME;
      if ((bool) this.m_open)
      {
        Action<bool> doorStateChanged = this.DoorStateChanged;
        if (doorStateChanged != null)
          doorStateChanged((bool) this.m_open);
      }
      this.m_stateChange = true;
    }

    protected override void OnEnabledChanged()
    {
      this.ResourceSink.Update();
      base.OnEnabledChanged();
    }

    public override void OnBuildSuccess(long builtBy, bool instantBuild)
    {
      this.ResourceSink.Update();
      base.OnBuildSuccess(builtBy, instantBuild);
    }

    public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
    {
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(this.BlockDefinition.ResourceSinkGroup, this.BlockDefinition.PowerConsumptionMoving, new Func<float>(this.UpdatePowerInput));
      this.ResourceSink = resourceSinkComponent;
      base.Init(builder, cubeGrid);
      MyObjectBuilder_Parachute builderParachute = (MyObjectBuilder_Parachute) builder;
      this.m_open.Value = builderParachute.Open;
      this.m_deployHeight.Value = builderParachute.DeployHeight;
      this.m_autoDeploy.Value = builderParachute.AutoDeploy;
      this.m_parachuteAnimationState = builderParachute.ParachuteState;
      if (this.m_parachuteAnimationState > 50)
        this.m_parachuteAnimationState = 0;
      this.m_dragCoefficient = this.BlockDefinition.DragCoefficient;
      this.m_minAtmosphere = this.BlockDefinition.MinimumAtmosphereLevel;
      resourceSinkComponent.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
      resourceSinkComponent.Update();
      this.OnStateChange();
      this.InitializeConveyorEndpoint();
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentDrawConveyorEndpoint((IMyConveyorEndpoint) this.m_conveyorEndpoint));
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.ResourceSink.Update();
      MyInventory myInventory = MyEntityExtensions.GetInventory(this);
      MyComponentDefinition componentDefinition = MyDefinitionManager.Static.GetComponentDefinition(this.BlockDefinition.MaterialDefinitionId);
      if (myInventory == null)
      {
        Vector3 one = Vector3.One;
        myInventory = new MyInventory(componentDefinition.Volume * (float) this.BlockDefinition.MaterialDeployCost, one, MyInventoryFlags.CanReceive);
        this.Components.Add<MyInventoryBase>((MyInventoryBase) myInventory);
      }
      this.inventory_ContentsChanged((MyInventoryBase) myInventory);
      myInventory.ContentsChanged += new Action<MyInventoryBase>(this.inventory_ContentsChanged);
      MyInventoryConstraint inventoryConstraint = new MyInventoryConstraint(MySpaceTexts.Parachute_ConstraintItem);
      inventoryConstraint.Add(this.BlockDefinition.MaterialDefinitionId);
      inventoryConstraint.Icon = MyGuiConstants.TEXTURE_ICON_FILTER_COMPONENT;
      myInventory.Constraint = inventoryConstraint;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    public void InitializeConveyorEndpoint() => this.m_conveyorEndpoint = new MyMultilineConveyorEndpoint((MyCubeBlock) this);

    private MyEntitySubpart LoadSubpartFromName(string name)
    {
      MyEntitySubpart myEntitySubpart1;
      if (this.Subparts.TryGetValue(name, out myEntitySubpart1))
        return myEntitySubpart1;
      MyEntitySubpart myEntitySubpart2 = new MyEntitySubpart();
      string model = Path.Combine(Path.GetDirectoryName(this.Model.AssetName), name) + ".mwm";
      myEntitySubpart2.Render.EnableColorMaskHsv = this.Render.EnableColorMaskHsv;
      myEntitySubpart2.Render.ColorMaskHsv = this.Render.ColorMaskHsv;
      myEntitySubpart2.Render.TextureChanges = this.Render.TextureChanges;
      myEntitySubpart2.Render.MetalnessColorable = this.Render.MetalnessColorable;
      myEntitySubpart2.Init((StringBuilder) null, model, (MyEntity) this, new float?());
      this.Subparts[name] = myEntitySubpart2;
      if (this.InScene)
        myEntitySubpart2.OnAddedToScene((object) this);
      return myEntitySubpart2;
    }

    private void InitSubparts()
    {
      if (!this.CubeGrid.CreatePhysics)
        return;
      this.m_emitter.Clear();
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_Parachute builderCubeBlock = (MyObjectBuilder_Parachute) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.Open = (bool) this.m_open;
      builderCubeBlock.AutoDeploy = (bool) this.m_autoDeploy;
      builderCubeBlock.DeployHeight = (float) this.m_deployHeight;
      builderCubeBlock.ParachuteState = this.m_parachuteAnimationState;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    protected float UpdatePowerInput() => !this.Enabled || !this.IsFunctional ? 0.0f : this.BlockDefinition.PowerConsumptionIdle;

    private void StartSound(int emitterId, MySoundPair cuePair)
    {
      if (this.m_emitter[emitterId].Sound != null && this.m_emitter[emitterId].Sound.IsPlaying && (this.m_emitter[emitterId].SoundId == cuePair.Arcade || this.m_emitter[emitterId].SoundId == cuePair.Realistic))
        return;
      this.m_emitter[emitterId].StopSound(true);
      this.m_emitter[emitterId].PlaySingleSound(cuePair);
    }

    public override void UpdateSoundEmitters()
    {
      for (int index = 0; index < this.m_emitter.Count; ++index)
      {
        if (this.m_emitter[index] != null)
          this.m_emitter[index].Update();
      }
    }

    public override void UpdateOnceBeforeFrame() => this.UpdateNearPlanet();

    public override void UpdateBeforeSimulation()
    {
      if ((bool) this.m_open || this.m_parachuteAnimationState > 0 && this.m_parachuteAnimationState < 50)
      {
        this.UpdateParachute();
      }
      else
      {
        this.UpdateCutChute();
        if (this.m_parachuteSubpart != null && this.m_parachuteSubpart.Render.RenderObjectIDs[0] != uint.MaxValue)
          this.m_parachuteSubpart.Render.Visible = false;
      }
      if (Sandbox.Game.Multiplayer.Sync.IsServer && this.m_canCheckAutoDeploy)
        this.CheckAutoDeploy();
      if (this.m_stateChange)
      {
        this.ResourceSink.Update();
        this.RaisePropertiesChanged();
        if (!(bool) this.m_open)
          this.DoorStateChanged.InvokeIfNotNull<bool>((bool) this.m_open);
        this.m_stateChange = false;
      }
      base.UpdateBeforeSimulation();
    }

    public override void UpdateBeforeSimulation10()
    {
      this.m_atmosphereDirty = true;
      if (this.CubeGrid.Physics != null)
      {
        this.m_gravityCache = (Vector3) this.GetNaturalGravity();
        this.m_canCheckAutoDeploy = false;
        if (this.AutoDeploy && this.CanDeploy && ((double) this.CubeGrid.Physics.LinearVelocity.LengthSquared() > 2.0 && (double) Vector3.Dot(this.m_gravityCache, this.CubeGrid.Physics.LinearVelocity) > 0.600000023841858))
          this.m_canCheckAutoDeploy = this.TryGetClosestPointInAtmosphere(out this.m_closestPointCache);
      }
      base.UpdateBeforeSimulation10();
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (this.CubeGrid.Physics == null)
        return;
      this.UpdateParachutePosition();
    }

    public override void UpdateAfterSimulation100()
    {
      base.UpdateAfterSimulation100();
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || this.CanDeploy)
        return;
      this.AttemptPullRequiredInventoryItems();
    }

    public void AttemptPullRequiredInventoryItems()
    {
      if (!((MyFixedPoint) this.BlockDefinition.MaterialDeployCost > this.m_requiredItemsInInventory))
        return;
      this.CubeGrid.GridSystems.ConveyorSystem.PullItem(this.BlockDefinition.MaterialDefinitionId, new MyFixedPoint?((MyFixedPoint) this.BlockDefinition.MaterialDeployCost - this.m_requiredItemsInInventory), (IMyConveyorEndpointBlock) this, MyEntityExtensions.GetInventory(this), false, false);
    }

    private bool CheckDeployChute()
    {
      if (this.CubeGrid.Physics == null || !this.CanDeploy || (this.m_parachuteAnimationState > 0 || (double) this.Atmosphere < (double) this.m_minAtmosphere))
        return false;
      if (!MySession.Static.CreativeMode)
      {
        MyInventory inventory = MyEntityExtensions.GetInventory(this);
        if (inventory.GetItemAmount(this.BlockDefinition.MaterialDefinitionId, MyItemFlags.None, false) >= (MyFixedPoint) this.BlockDefinition.MaterialDeployCost)
        {
          inventory.RemoveItemsOfType((MyFixedPoint) this.BlockDefinition.MaterialDeployCost, this.BlockDefinition.MaterialDefinitionId, MyItemFlags.None, false);
        }
        else
        {
          this.CanDeploy = false;
          return false;
        }
      }
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyParachute>(this, (Func<MyParachute, Action>) (x => new Action(x.DoDeployChute)));
      return true;
    }

    [Event(null, 582)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    private void DoDeployChute()
    {
      this.m_parachuteAnimationState = 1;
      this.m_lastParachuteRotation = Quaternion.Identity;
      this.m_lastParachuteScale = Vector3.Zero;
      this.m_cutParachuteTimer = 0;
      if (this.m_parachuteSubpart == null)
        this.m_parachuteSubpart = this.LoadSubpartFromName(this.BlockDefinition.ParachuteSubpartName);
      this.m_parachuteSubpart.Render.Visible = true;
      if (this.ParachuteStateChanged == null)
        return;
      this.ParachuteStateChanged(true);
    }

    private void RemoveChute()
    {
      this.m_parachuteAnimationState = 0;
      if (this.m_parachuteSubpart == null)
        return;
      this.m_parachuteSubpart.Render.Visible = false;
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      ((SpaceEngineers.Game.ModAPI.Ingame.IMyParachute) this).CloseDoor();
    }

    private void UpdateParachute()
    {
      if (this.CubeGrid.Physics == null)
        return;
      if (this.m_parachuteAnimationState > 50)
      {
        if (Sandbox.Game.Multiplayer.Sync.IsServer && this.CanDeploy && ((bool) this.m_open && this.CheckDeployChute()))
          return;
        this.UpdateCutChute();
      }
      else
      {
        if (this.m_parachuteAnimationState == 0 && Sandbox.Game.Multiplayer.Sync.IsServer && (this.CanDeploy && (bool) this.m_open))
          this.CheckDeployChute();
        if (this.m_parachuteAnimationState > 0 && this.m_parachuteAnimationState < 50)
          ++this.m_parachuteAnimationState;
        Vector3 zero = Vector3.Zero;
        bool flag = false;
        float num1 = this.CubeGrid.Physics.LinearVelocity.LengthSquared();
        Vector3 vector3;
        if ((double) num1 > 2.0)
        {
          vector3 = this.CubeGrid.Physics.LinearVelocity;
          this.m_cutParachuteTimer = 0;
        }
        else if (0.100000001490116 > (double) num1)
        {
          flag = true;
          vector3 = Vector3.Lerp(this.m_lastParachuteVelocityVector, -this.m_gravityCache, 0.05f);
          if ((double) Vector3.DistanceSquared(vector3, -this.m_gravityCache) < 1.0 / 400.0)
          {
            ++this.m_cutParachuteTimer;
            if (this.m_cutParachuteTimer > 60)
            {
              if (Sandbox.Game.Multiplayer.Sync.IsServer)
                ((SpaceEngineers.Game.ModAPI.Ingame.IMyParachute) this).CloseDoor();
              this.UpdateCutChute();
              return;
            }
          }
        }
        else
        {
          flag = true;
          vector3 = this.CubeGrid.Physics.LinearVelocity;
        }
        double d1 = 10.0 * ((double) this.Atmosphere - (double) this.BlockDefinition.ReefAtmosphereLevel) * ((double) this.m_parachuteAnimationState / 50.0);
        double d2;
        if (d1 <= 0.5 || double.IsNaN(d1))
        {
          d2 = 0.5;
        }
        else
        {
          d2 = Math.Log(d1 - 0.99) + 5.0;
          if (d2 < 0.5 || double.IsNaN(d2))
            d2 = 0.5;
        }
        this.m_chuteScale.Z = this.m_parachuteAnimationState != 0 ? Math.Log((double) this.m_parachuteAnimationState / 1.5) * (double) this.CubeGrid.GridSize * 20.0 : 0.0;
        this.m_chuteScale.X = this.m_chuteScale.Y = d2 * (double) this.BlockDefinition.RadiusMultiplier * (double) this.CubeGrid.GridSize;
        this.m_lastParachuteVelocityVector = vector3;
        MatrixD matrixD1;
        Vector3D vector3D1;
        if (Vector3D.IsZero((Vector3D) vector3))
        {
          matrixD1 = this.PositionComp.WorldMatrix;
          vector3D1 = matrixD1.Up;
        }
        else
          vector3D1 = Vector3D.Normalize((Vector3D) vector3);
        Quaternion quaternion = Quaternion.Lerp(this.m_lastParachuteRotation, Quaternion.CreateFromRotationMatrix(Matrix.CreateFromDir((Vector3) vector3D1, new Vector3(0.0f, 1f, 0.0f)).GetOrientation()), 0.02f);
        this.m_chuteScale = Vector3D.Lerp((Vector3D) this.m_lastParachuteScale, this.m_chuteScale, 0.02);
        double num2 = this.m_chuteScale.X / 2.0;
        this.m_lastParachuteScale = (Vector3) this.m_chuteScale;
        this.m_lastParachuteRotation = quaternion;
        MatrixD matrixD2 = MatrixD.Invert(this.WorldMatrix);
        Quaternion parachuteRotation = this.m_lastParachuteRotation;
        matrixD1 = this.WorldMatrix;
        Vector3D translation = matrixD1.Translation;
        matrixD1 = this.WorldMatrix;
        Vector3D vector3D2 = matrixD1.Up * ((double) this.CubeGrid.GridSize / 2.0);
        Vector3D position1 = translation + vector3D2;
        Vector3D lastParachuteScale = (Vector3D) this.m_lastParachuteScale;
        this.m_lastParachuteWorldMatrix = MatrixD.CreateFromTransformScale(parachuteRotation, position1, lastParachuteScale);
        matrixD1 = this.m_lastParachuteWorldMatrix * matrixD2;
        this.m_lastParachuteLocalMatrix = (Matrix) ref matrixD1;
        if (num2 <= 0.0 | flag || (double) vector3.LengthSquared() <= 1.0)
          return;
        Vector3D vector3D3 = -vector3D1;
        double num3 = Math.PI * num2 * num2;
        double scaleFactor = 2.5 * ((double) this.Atmosphere * 1.225) * (double) vector3.LengthSquared() * num3 * (double) this.DragCoefficient;
        if (scaleFactor <= 0.0 || this.CubeGrid.Physics.IsStatic)
          return;
        MyGridPhysics physics = this.CubeGrid.Physics;
        Vector3? force = new Vector3?((Vector3) Vector3D.Multiply(vector3D3, scaleFactor));
        matrixD1 = this.WorldMatrix;
        Vector3D? position2 = new Vector3D?(matrixD1.Translation);
        Vector3? torque = new Vector3?(Vector3.Zero);
        float? maxSpeed = new float?();
        physics.AddForce(MyPhysicsForceType.APPLY_WORLD_FORCE, force, position2, torque, maxSpeed, true, false);
      }
    }

    private void UpdateParachutePosition()
    {
      if (this.m_parachuteSubpart == null || this.m_parachuteAnimationState <= 0)
        return;
      this.m_parachuteSubpart.PositionComp.SetLocalMatrix(ref this.m_lastParachuteLocalMatrix);
    }

    private void UpdateCutChute()
    {
      if (this.CubeGrid.Physics == null || this.m_parachuteAnimationState == 0)
        return;
      if (this.m_parachuteAnimationState > 100)
      {
        this.RemoveChute();
      }
      else
      {
        if (this.m_parachuteAnimationState < 50)
          this.m_parachuteAnimationState = 50;
        if (this.m_parachuteAnimationState == 50)
          this.ParachuteStateChanged.InvokeIfNotNull<bool>(false);
        ++this.m_parachuteAnimationState;
        if (this.m_parachuteSubpart == null)
          return;
        this.m_lastParachuteWorldMatrix.Translation += this.m_gravityCache * 0.05f;
        MatrixD matrixD = this.m_lastParachuteWorldMatrix * MatrixD.Invert(this.WorldMatrix);
        Matrix localMatrix = (Matrix) ref matrixD;
        this.m_parachuteSubpart.PositionComp.SetLocalMatrix(ref localMatrix);
      }
    }

    private void CheckAutoDeploy()
    {
      if (!this.m_closestPointCache.HasValue || Vector3D.DistanceSquared(this.m_closestPointCache.Value, this.WorldMatrix.Translation) >= (double) this.DeployHeight * (double) this.DeployHeight)
        return;
      ((SpaceEngineers.Game.ModAPI.Ingame.IMyParachute) this).OpenDoor();
    }

    private void UpdateNearPlanet()
    {
      BoundingBoxD worldAabb = this.PositionComp.WorldAABB;
      this.m_nearPlanetCache = MyGamePruningStructure.GetClosestPlanet(ref worldAabb);
    }

    protected override void Closing()
    {
      for (int index = 0; index < this.m_emitter.Count; ++index)
      {
        if (this.m_emitter[index] != null)
          this.m_emitter[index].StopSound(true);
      }
      base.Closing();
    }

    public override void OnModelChange()
    {
      base.OnModelChange();
      this.InitSubparts();
    }

    private void Receiver_IsPoweredChanged()
    {
      this.UpdateIsWorking();
      this.UpdateEmissivity();
    }

    private void inventory_ContentsChanged(MyInventoryBase obj)
    {
      if (MySession.Static.CreativeMode)
      {
        this.CanDeploy = true;
      }
      else
      {
        this.m_requiredItemsInInventory = obj.GetItemAmount(this.BlockDefinition.MaterialDefinitionId);
        if (this.m_requiredItemsInInventory >= (MyFixedPoint) this.BlockDefinition.MaterialDeployCost)
          this.CanDeploy = true;
        else
          this.CanDeploy = false;
      }
    }

    private void ComponentStack_IsFunctionalChanged() => this.ResourceSink.Update();

    private event Action<bool> DoorStateChanged;

    event Action<bool> SpaceEngineers.Game.ModAPI.IMyParachute.DoorStateChanged
    {
      add => this.DoorStateChanged += value;
      remove => this.DoorStateChanged -= value;
    }

    private event Action<bool> ParachuteStateChanged;

    event Action<bool> SpaceEngineers.Game.ModAPI.IMyParachute.ParachuteStateChanged
    {
      add => this.ParachuteStateChanged += value;
      remove => this.ParachuteStateChanged -= value;
    }

    public PullInformation GetPullInformation() => new PullInformation()
    {
      Inventory = MyEntityExtensions.GetInventory(this),
      OwnerID = this.OwnerId,
      ItemDefinition = this.BlockDefinition.MaterialDefinitionId
    };

    public PullInformation GetPushInformation() => (PullInformation) null;

    public bool AllowSelfPulling() => false;

    private bool TryGetClosestPointInAtmosphere(out Vector3D? closestPoint) => !this.TryGetClosestPoint(out closestPoint) || (double) this.m_minAtmosphere <= (double) this.Atmosphere;

    public bool TryGetClosestPoint(out Vector3D? closestPoint)
    {
      closestPoint = new Vector3D?();
      BoundingBoxD worldAabb = this.PositionComp.WorldAABB;
      this.m_nearPlanetCache = MyGamePruningStructure.GetClosestPlanet(ref worldAabb);
      if (this.m_nearPlanetCache == null)
        return false;
      Vector3D centerOfMassWorld = this.CubeGrid.Physics.CenterOfMassWorld;
      closestPoint = new Vector3D?(this.m_nearPlanetCache.GetClosestSurfacePointGlobal(ref centerOfMassWorld));
      return true;
    }

    public Vector3D GetVelocity()
    {
      MyPhysicsComponentBase physicsComponentBase = this.Parent != null ? this.Parent.Physics : (MyPhysicsComponentBase) null;
      return physicsComponentBase != null ? new Vector3D(physicsComponentBase.GetVelocityAtPoint(this.PositionComp.GetPosition())) : Vector3D.Zero;
    }

    public Vector3D GetNaturalGravity() => (Vector3D) MyGravityProviderSystem.CalculateNaturalGravityInPoint(this.WorldMatrix.Translation);

    public Vector3D GetArtificialGravity() => (Vector3D) MyGravityProviderSystem.CalculateArtificialGravityInPoint(this.WorldMatrix.Translation);

    public override void OnRemovedByCubeBuilder()
    {
      this.ReleaseInventory(MyEntityExtensions.GetInventory(this));
      base.OnRemovedByCubeBuilder();
    }

    public override void OnDestroy()
    {
      this.ReleaseInventory(MyEntityExtensions.GetInventory(this), true);
      base.OnDestroy();
    }

    public Vector3D GetTotalGravity() => (Vector3D) MyGravityProviderSystem.CalculateTotalGravityInPoint(this.WorldMatrix.Translation);

    public override void ContactCallbackInternal() => base.ContactCallbackInternal();

    public override bool EnableContactCallbacks() => false;

    public override bool IsClosing() => false;

    protected sealed class AutoDeployRequest\u003C\u003ESystem_Boolean\u0023System_Int64 : ICallSite<MyParachute, bool, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyParachute @this,
        in bool autodeploy,
        in long identityId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.AutoDeployRequest(autodeploy, identityId);
      }
    }

    protected sealed class DeployHeightRequest\u003C\u003ESystem_Single\u0023System_Int64 : ICallSite<MyParachute, float, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyParachute @this,
        in float deployHeight,
        in long identityId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.DeployHeightRequest(deployHeight, identityId);
      }
    }

    protected sealed class DoDeployChute\u003C\u003E : ICallSite<MyParachute, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyParachute @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.DoDeployChute();
      }
    }

    protected class m_autoDeploy\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyParachute) obj0).m_autoDeploy = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_deployHeight\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyParachute) obj0).m_deployHeight = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }
  }
}
