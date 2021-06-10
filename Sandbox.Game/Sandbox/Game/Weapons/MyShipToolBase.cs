// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.MyShipToolBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Audio;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment;
using Sandbox.Game.WorldEnvironment.Modules;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Weapons
{
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyShipToolBase), typeof (Sandbox.ModAPI.Ingame.IMyShipToolBase)})]
  public abstract class MyShipToolBase : MyFunctionalBlock, IMyGunObject<MyToolBase>, IMyInventoryOwner, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyShipToolBase, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyShipToolBase
  {
    protected float DEFAULT_REACH_DISTANCE = 4.5f;
    private MyMultilineConveyorEndpoint m_endpoint;
    private MyDefinitionId m_defId;
    private bool m_wantsToActivate;
    private bool m_isActivated;
    protected bool m_isActivatedOnSomething;
    protected int m_lastTimeActivate;
    private int m_shootHeatup;
    private int m_activateCounter;
    private HashSet<MyEntity> m_entitiesInContact;
    protected BoundingSphere m_detectorSphere;
    protected bool m_checkEnvironmentSector;
    private HashSet<MySlimBlock> m_blocksToActivateOn;
    private HashSet<MySlimBlock> m_tempBlocksBuffer;
    private VRage.Sync.Sync<bool, SyncDirection.BothWays> m_useConveyorSystem;
    protected MyCharacter m_controller;
    private bool m_effectActivated;
    private bool m_animationActivated;

    protected bool WantsToActivate
    {
      get => this.m_wantsToActivate;
      set
      {
        this.m_wantsToActivate = value;
        this.UpdateActivationState();
      }
    }

    public bool IsHeatingUp => this.m_shootHeatup > 0;

    public int HeatUpFrames { get; protected set; }

    public bool IsSkinnable => false;

    public MyShipToolBase() => this.CreateTerminalControls();

    protected override bool CheckIsWorking() => this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.CheckIsWorking();

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyShipToolBase>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlOnOffSwitch<MyShipToolBase> onOff = new MyTerminalControlOnOffSwitch<MyShipToolBase>("UseConveyor", MySpaceTexts.Terminal_UseConveyorSystem);
      onOff.Getter = (MyTerminalValueControl<MyShipToolBase, bool>.GetterDelegate) (x => x.UseConveyorSystem);
      onOff.Setter = (MyTerminalValueControl<MyShipToolBase, bool>.SetterDelegate) ((x, v) => x.UseConveyorSystem = v);
      onOff.EnableToggleAction<MyShipToolBase>();
      MyTerminalControlFactory.AddControl<MyShipToolBase>((MyTerminalControl<MyShipToolBase>) onOff);
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(MyStringHash.GetOrCompute("Defense"), 1f / 500f, new Func<float>(this.ComputeRequiredPower));
      resourceSinkComponent.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
      this.ResourceSink = resourceSinkComponent;
      base.Init(objectBuilder, cubeGrid);
      this.m_entitiesInContact = new HashSet<MyEntity>();
      this.m_blocksToActivateOn = new HashSet<MySlimBlock>();
      this.m_tempBlocksBuffer = new HashSet<MySlimBlock>();
      this.m_isActivated = false;
      this.m_isActivatedOnSomething = false;
      this.m_wantsToActivate = false;
      this.m_shootHeatup = 0;
      this.m_activateCounter = 0;
      this.m_defId = objectBuilder.GetId();
      MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(this.m_defId);
      MyObjectBuilder_ShipToolBase builderShipToolBase = objectBuilder as MyObjectBuilder_ShipToolBase;
      float maxVolume = (float) ((double) cubeBlockDefinition.Size.X * (double) cubeGrid.GridSize * (double) cubeBlockDefinition.Size.Y * (double) cubeGrid.GridSize * (double) cubeBlockDefinition.Size.Z * (double) cubeGrid.GridSize * 0.5);
      Vector3 size = new Vector3((float) cubeBlockDefinition.Size.X, (float) cubeBlockDefinition.Size.Y, (float) cubeBlockDefinition.Size.Z * 0.5f);
      if (MyEntityExtensions.GetInventory(this) == null)
      {
        MyInventory myInventory = new MyInventory(maxVolume, size, MyInventoryFlags.CanSend);
        this.Components.Add<MyInventoryBase>((MyInventoryBase) myInventory);
        myInventory.Init(builderShipToolBase.Inventory);
      }
      this.Enabled = builderShipToolBase.Enabled;
      this.UseConveyorSystem = builderShipToolBase.UseConveyorSystem;
      this.m_checkEnvironmentSector = builderShipToolBase.CheckEnvironmentSector;
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.LoadDummies();
      this.UpdateActivationState();
      this.IsWorkingChanged += new Action<MyCubeBlock>(this.MyShipToolBase_IsWorkingChanged);
      this.ResourceSink.Update();
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_ShipToolBase builderCubeBlock = (MyObjectBuilder_ShipToolBase) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.UseConveyorSystem = this.UseConveyorSystem;
      builderCubeBlock.CheckEnvironmentSector = this.m_checkEnvironmentSector;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

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

    private void LoadDummies()
    {
      MyModel modelOnlyDummies = MyModels.GetModelOnlyDummies(this.BlockDefinition.Model);
      MyShipToolDefinition blockDefinition = (MyShipToolDefinition) this.BlockDefinition;
      foreach (KeyValuePair<string, MyModelDummy> dummy in modelOnlyDummies.Dummies)
      {
        if (dummy.Key.ToLower().Contains("detector_shiptool"))
        {
          Matrix matrix1 = dummy.Value.Matrix;
          double num = (double) matrix1.Scale.AbsMin();
          Matrix matrix2 = this.PositionComp.LocalMatrixRef;
          Matrix matrix3 = matrix1 * matrix2;
          this.m_detectorSphere = new BoundingSphere(matrix3.Translation + matrix3.Forward * blockDefinition.SensorOffset, blockDefinition.SensorRadius);
          break;
        }
      }
    }

    protected void SetBuildingMusic(int amount)
    {
      if (MySession.Static == null || this.m_controller != MySession.Static.LocalCharacter || MyMusicController.Static == null)
        return;
      MyMusicController.Static.Building(amount);
    }

    protected virtual bool CanInteractWithSelf => false;

    private bool CanInteractWith(VRage.ModAPI.IMyEntity entity)
    {
      if (entity == null || entity == this.CubeGrid && !this.CanInteractWithSelf)
        return false;
      switch (entity)
      {
        case MyCubeGrid _:
        case MyCharacter _:
          return true;
        default:
          return false;
      }
    }

    protected override void OnEnabledChanged()
    {
      this.WantsToActivate = this.Enabled;
      base.OnEnabledChanged();
    }

    private void ComponentStack_IsFunctionalChanged() => this.ResourceSink.Update();

    private void Receiver_IsPoweredChanged()
    {
      this.UpdateIsWorking();
      this.UpdateActivationState();
    }

    private void MyShipToolBase_IsWorkingChanged(MyCubeBlock obj) => this.UpdateActivationState();

    public override void OnModelChange()
    {
      base.OnModelChange();
      this.UpdateActivationState();
    }

    private void UpdateActivationState()
    {
      if (this.ResourceSink != null)
        this.ResourceSink.Update();
      if ((this.Enabled || this.WantsToActivate) && (this.IsFunctional && this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId)))
        this.StartShooting();
      else
        this.StopShooting();
    }

    private float ComputeRequiredPower() => !this.IsFunctional || !this.Enabled && !this.WantsToActivate ? 1E-06f : this.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId);

    public override void UpdateAfterSimulation10()
    {
      base.UpdateAfterSimulation10();
      if (this.m_isActivated && this.CanShoot(MyShootActionEnum.PrimaryAction, this.OwnerId, out MyGunStatusEnum _))
      {
        if (!this.m_animationActivated)
        {
          this.m_animationActivated = true;
          this.StartAnimation();
        }
        this.ActivateCommon();
      }
      else if (this.m_animationActivated)
      {
        this.m_animationActivated = false;
        this.StopAnimation();
      }
      if (!this.m_isActivatedOnSomething && !this.m_effectActivated)
        return;
      bool flag = Vector3D.DistanceSquared(MySector.MainCamera.Position, this.PositionComp.GetPosition()) < 10000.0;
      if (!this.m_isActivatedOnSomething || !flag)
      {
        if (this.m_effectActivated)
          this.StopEffects();
        this.m_effectActivated = false;
      }
      else
      {
        if (!this.m_isActivatedOnSomething)
          return;
        if (!this.m_effectActivated)
          this.StartEffects();
        this.m_effectActivated = true;
      }
    }

    protected abstract bool Activate(HashSet<MySlimBlock> targets);

    protected abstract void StartEffects();

    protected abstract void StopEffects();

    protected virtual void StartAnimation()
    {
    }

    protected virtual void StopAnimation()
    {
    }

    private void ActivateCommon()
    {
      BoundingSphereD boundingSphereD = new BoundingSphereD(Vector3D.Transform(this.m_detectorSphere.Center, this.CubeGrid.WorldMatrix), (double) this.m_detectorSphere.Radius);
      BoundingSphereD sphere = new BoundingSphereD(boundingSphereD.Center, (double) this.m_detectorSphere.Radius * 0.5);
      if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
      {
        MyRenderProxy.DebugDrawSphere(boundingSphereD.Center, (float) boundingSphereD.Radius, (Color) Color.Red.ToVector3(), depthRead: false, persistent: true);
        MyRenderProxy.DebugDrawSphere(sphere.Center, (float) sphere.Radius, (Color) Color.Blue.ToVector3(), depthRead: false, persistent: true);
      }
      this.m_isActivatedOnSomething = false;
      List<MyEntity> entitiesInSphere = Sandbox.Game.Entities.MyEntities.GetTopMostEntitiesInSphere(ref boundingSphereD);
      bool flag = false;
      this.m_entitiesInContact.Clear();
      foreach (MyEntity myEntity in entitiesInSphere)
      {
        if (myEntity is MyEnvironmentSector)
          flag = true;
        MyEntity topMostParent = myEntity.GetTopMostParent((Type) null);
        if (this.CanInteractWith((VRage.ModAPI.IMyEntity) topMostParent))
          this.m_entitiesInContact.Add(topMostParent);
      }
      if (this.m_checkEnvironmentSector & flag)
      {
        MyPhysics.HitInfo? nullable = MyPhysics.CastRay(boundingSphereD.Center, boundingSphereD.Center + boundingSphereD.Radius * this.WorldMatrix.Forward, 24);
        if (nullable.HasValue && nullable.HasValue)
        {
          VRage.ModAPI.IMyEntity hitEntity = nullable.Value.HkHitInfo.GetHitEntity();
          if (hitEntity is MyEnvironmentSector)
          {
            MyEnvironmentSector environmentSector = hitEntity as MyEnvironmentSector;
            uint shapeKey = nullable.Value.HkHitInfo.GetShapeKey(0);
            int itemFromShapeKey = environmentSector.GetItemFromShapeKey(shapeKey);
            if (environmentSector.DataView.Items[itemFromShapeKey].ModelIndex >= (short) 0)
            {
              MyBreakableEnvironmentProxy module = environmentSector.GetModule<MyBreakableEnvironmentProxy>();
              Vector3D vector3D = this.CubeGrid.WorldMatrix.Right + this.CubeGrid.WorldMatrix.Forward;
              vector3D.Normalize();
              double num1 = 10.0;
              float num2 = (float) (num1 * num1) * this.CubeGrid.Physics.Mass;
              int itemId = itemFromShapeKey;
              Vector3D position = (Vector3D) nullable.Value.HkHitInfo.Position;
              Vector3D hitnormal = vector3D;
              double impactEnergy = (double) num2;
              module.BreakAt(itemId, position, hitnormal, impactEnergy);
            }
          }
        }
      }
      entitiesInSphere.Clear();
      foreach (MyEntity myEntity in this.m_entitiesInContact)
      {
        MyCubeGrid myCubeGrid = myEntity as MyCubeGrid;
        MyCharacter myCharacter = myEntity as MyCharacter;
        if (myCubeGrid != null)
        {
          this.m_tempBlocksBuffer.Clear();
          myCubeGrid.GetBlocksInsideSphere(ref boundingSphereD, this.m_tempBlocksBuffer, true);
          this.m_blocksToActivateOn.UnionWith((IEnumerable<MySlimBlock>) this.m_tempBlocksBuffer);
        }
        if (myCharacter != null && Sandbox.Game.Multiplayer.Sync.IsServer)
        {
          MyStringHash damageType = MyDamageType.Drill;
          switch (this)
          {
            case Sandbox.ModAPI.IMyShipGrinder _:
              damageType = MyDamageType.Grind;
              break;
            case Sandbox.ModAPI.IMyShipWelder _:
              damageType = MyDamageType.Weld;
              break;
          }
          if (new MyOrientedBoundingBoxD((BoundingBoxD) myCharacter.PositionComp.LocalAABB, myCharacter.PositionComp.WorldMatrixRef).Intersects(ref sphere))
            myCharacter.DoDamage(20f, damageType, true, this.EntityId);
        }
      }
      this.m_isActivatedOnSomething |= this.Activate(this.m_blocksToActivateOn);
      ++this.m_activateCounter;
      this.m_lastTimeActivate = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      this.PlayLoopSound(this.m_isActivatedOnSomething);
      this.m_blocksToActivateOn.Clear();
    }

    public override void OnRemovedFromScene(object source)
    {
      base.OnRemovedFromScene(source);
      this.StopEffects();
      this.StopLoopSound();
    }

    public override void OnAddedToScene(object source)
    {
      this.LoadDummies();
      base.OnAddedToScene(source);
      this.UpdateActivationState();
    }

    protected override void Closing()
    {
      base.Closing();
      this.StopEffects();
      this.StopLoopSound();
    }

    public float BackkickForcePerSecond => 0.0f;

    public float ShakeAmount { get; protected set; }

    public MyDefinitionId DefinitionId => this.m_defId;

    public bool EnabledInWorldRules => true;

    protected virtual void StartShooting() => this.m_isActivated = true;

    protected virtual void StopShooting()
    {
      this.m_wantsToActivate = false;
      this.m_isActivated = false;
      this.m_isActivatedOnSomething = false;
      if (this.Physics != null)
        this.Physics.Enabled = false;
      if (this.ResourceSink != null)
        this.ResourceSink.Update();
      this.m_shootHeatup = 0;
      this.StopEffects();
      this.StopLoopSound();
    }

    public int GetTotalAmmunitionAmount() => throw new NotImplementedException();

    public int GetAmmunitionAmount() => throw new NotImplementedException();

    public int GetMagazineAmount() => throw new NotImplementedException();

    public virtual void OnControlAcquired(MyCharacter owner)
    {
    }

    public virtual void OnControlReleased()
    {
      if (this.Enabled || this.Closed)
        return;
      this.StopShooting();
    }

    public void DrawHud(IMyCameraController camera, long playerId, bool fullUpdate) => this.DrawHud(camera, playerId);

    public void DrawHud(IMyCameraController camera, long playerId)
    {
    }

    public void SetInventory(MyInventory inventory, int index) => this.Components.Add<MyInventoryBase>((MyInventoryBase) inventory);

    public bool UseConveyorSystem
    {
      get => (bool) this.m_useConveyorSystem;
      set => this.m_useConveyorSystem.Value = value;
    }

    public IMyConveyorEndpoint ConveyorEndpoint => (IMyConveyorEndpoint) this.m_endpoint;

    public void InitializeConveyorEndpoint()
    {
      this.m_endpoint = new MyMultilineConveyorEndpoint((MyCubeBlock) this);
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentDrawConveyorEndpoint((IMyConveyorEndpoint) this.m_endpoint));
    }

    protected abstract void StopLoopSound();

    protected abstract void PlayLoopSound(bool activated);

    public Vector3 DirectionToTarget(Vector3D target) => throw new NotImplementedException();

    public virtual bool CanShoot(
      MyShootActionEnum action,
      long shooter,
      out MyGunStatusEnum status)
    {
      status = MyGunStatusEnum.OK;
      if (action != MyShootActionEnum.PrimaryAction)
      {
        status = MyGunStatusEnum.Failed;
        return false;
      }
      if (!this.IsFunctional)
      {
        status = MyGunStatusEnum.NotFunctional;
        return false;
      }
      if (!this.HasPlayerAccess(shooter))
      {
        status = MyGunStatusEnum.AccessDenied;
        return false;
      }
      if (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastTimeActivate >= 250)
        return true;
      status = MyGunStatusEnum.Cooldown;
      return false;
    }

    public void Shoot(
      MyShootActionEnum action,
      Vector3 direction,
      Vector3D? overrideWeaponPos,
      string gunAction)
    {
      if (action != MyShootActionEnum.PrimaryAction)
        return;
      if (this.m_shootHeatup < this.HeatUpFrames)
      {
        ++this.m_shootHeatup;
      }
      else
      {
        this.WantsToActivate = true;
        this.ResourceSink.Update();
      }
    }

    public virtual void BeginShoot(MyShootActionEnum action)
    {
    }

    public virtual void EndShoot(MyShootActionEnum action)
    {
      if (action != MyShootActionEnum.PrimaryAction || this.Enabled)
        return;
      this.StopShooting();
    }

    public bool IsShooting => this.m_isActivated;

    public int ShootDirectionUpdateTime => 0;

    public bool NeedsShootDirectionWhileAiming => false;

    public float MaximumShotLength => 0.0f;

    public void BeginFailReaction(MyShootActionEnum action, MyGunStatusEnum status)
    {
    }

    public void BeginFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
    {
    }

    public void ShootFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
    {
    }

    public MyToolBase GunBase => (MyToolBase) null;

    bool Sandbox.ModAPI.Ingame.IMyShipToolBase.UseConveyorSystem
    {
      get => this.UseConveyorSystem;
      set => this.UseConveyorSystem = value;
    }

    public void UpdateSoundEmitter()
    {
      if (this.m_soundEmitter == null)
        return;
      this.m_soundEmitter.Update();
    }

    public bool SupressShootAnimation() => false;

    int IMyInventoryOwner.InventoryCount => this.InventoryCount;

    long IMyInventoryOwner.EntityId => this.EntityId;

    bool IMyInventoryOwner.HasInventory => this.HasInventory;

    bool IMyInventoryOwner.UseConveyorSystem
    {
      get => this.UseConveyorSystem;
      set => this.UseConveyorSystem = value;
    }

    VRage.Game.ModAPI.Ingame.IMyInventory IMyInventoryOwner.GetInventory(
      int index)
    {
      return (VRage.Game.ModAPI.Ingame.IMyInventory) MyEntityExtensions.GetInventory(this, index);
    }

    public virtual PullInformation GetPullInformation() => (PullInformation) null;

    public virtual PullInformation GetPushInformation() => (PullInformation) null;

    public bool AllowSelfPulling() => false;

    public Vector3D GetMuzzlePosition() => this.PositionComp.GetPosition();

    protected class m_useConveyorSystem\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyShipToolBase) obj0).m_useConveyorSystem = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }
  }
}
