// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MyCollector
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_Collector))]
  [MyTerminalInterface(new System.Type[] {typeof (Sandbox.ModAPI.IMyCollector), typeof (Sandbox.ModAPI.Ingame.IMyCollector)})]
  public class MyCollector : MyFunctionalBlock, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyCollector, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyCollector, IMyInventoryOwner
  {
    private HkConstraint m_phantomConstraint;
    private VRage.Sync.Sync<bool, SyncDirection.BothWays> m_useConveyorSystem;
    private MyMultilineConveyorEndpoint m_multilineConveyorEndpoint;
    private bool m_isCollecting;
    private readonly MyConcurrentHashSet<MyFloatingObject> m_entitiesToTake = new MyConcurrentHashSet<MyFloatingObject>();

    public MyPoweredCargoContainerDefinition BlockDefinition => this.SlimBlock.BlockDefinition as MyPoweredCargoContainerDefinition;

    private bool ShouldHavePhantom => this.CubeGrid.CreatePhysics && !this.CubeGrid.IsPreview && Sandbox.Game.Multiplayer.Sync.IsServer;

    public MyCollector() => this.CreateTerminalControls();

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyCollector>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlOnOffSwitch<MyCollector> onOff = new MyTerminalControlOnOffSwitch<MyCollector>("UseConveyor", MySpaceTexts.Terminal_UseConveyorSystem);
      onOff.Getter = (MyTerminalValueControl<MyCollector, bool>.GetterDelegate) (x => x.UseConveyorSystem);
      onOff.Setter = (MyTerminalValueControl<MyCollector, bool>.SetterDelegate) ((x, v) => x.UseConveyorSystem = v);
      onOff.EnableToggleAction<MyCollector>();
      MyTerminalControlFactory.AddControl<MyCollector>((MyTerminalControl<MyCollector>) onOff);
    }

    protected override bool CheckIsWorking() => this.ResourceSink != null && this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.CheckIsWorking();

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      MyObjectBuilder_Collector builderCollector = objectBuilder as MyObjectBuilder_Collector;
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(MyStringHash.GetOrCompute(this.BlockDefinition.ResourceSinkGroup), this.BlockDefinition.RequiredPowerInput, new Func<float>(this.ComputeRequiredPower));
      this.ResourceSink = resourceSinkComponent;
      base.Init(objectBuilder, cubeGrid);
      if (MyFakes.ENABLE_INVENTORY_FIX)
        this.FixSingleInventory();
      if (MyEntityExtensions.GetInventory(this) == null)
      {
        MyInventory myInventory = new MyInventory(this.BlockDefinition.InventorySize.Volume, this.BlockDefinition.InventorySize, MyInventoryFlags.CanSend);
        this.Components.Add<MyInventoryBase>((MyInventoryBase) myInventory);
        myInventory.Init(builderCollector.Inventory);
      }
      if (Sandbox.Game.Multiplayer.Sync.IsServer && this.CubeGrid.CreatePhysics)
        this.LoadDummies();
      this.ResourceSink.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.UpdateReceiver);
      this.EnabledChanged += new Action<MyTerminalBlock>(this.UpdateReceiver);
      this.m_useConveyorSystem.SetLocalValue(builderCollector.UseConveyorSystem);
      this.ResourceSink.Update();
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      if (!((HkReferenceObject) this.m_phantomConstraint == (HkReferenceObject) null) || !this.ShouldHavePhantom)
        return;
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    public override void OnRemovedFromScene(object source)
    {
      this.DisposePhantomContraint();
      base.OnRemovedFromScene(source);
    }

    protected float ComputeRequiredPower() => !this.Enabled || !this.IsFunctional ? 0.0f : this.BlockDefinition.RequiredPowerInput;

    private void UpdateReceiver(MyTerminalBlock block) => this.ResourceSink.Update();

    private void UpdateReceiver() => this.ResourceSink.Update();

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_Collector builderCubeBlock = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_Collector;
      builderCubeBlock.UseConveyorSystem = (bool) this.m_useConveyorSystem;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public override void UpdateBeforeSimulation100()
    {
      base.UpdateBeforeSimulation100();
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || !this.IsWorking || !(bool) this.m_useConveyorSystem)
        return;
      MyInventory inventory = MyEntityExtensions.GetInventory(this);
      if (inventory.GetItemsCount() <= 0)
        return;
      MyGridConveyorSystem.PushAnyRequest((IMyConveyorEndpointBlock) this, inventory);
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        if (this.m_entitiesToTake.Count <= 0)
          return;
        this.m_entitiesToTake.Clear();
      }
      else
      {
        if ((HkReferenceObject) this.m_phantomConstraint == (HkReferenceObject) null && this.ShouldHavePhantom)
          this.CreatePhantomConstraint();
        if (!this.Enabled || !this.IsWorking)
          return;
        bool flag = false;
        this.m_isCollecting = true;
        foreach (MyFloatingObject myFloatingObject in this.m_entitiesToTake)
        {
          MyEntityExtensions.GetInventory(this).TakeFloatingObject(myFloatingObject);
          flag = true;
        }
        this.m_isCollecting = false;
        if (!flag)
          return;
        Vector3D position1 = this.m_entitiesToTake.ElementAt<MyFloatingObject>(0).PositionComp.GetPosition();
        Vector3D position2 = position1;
        MatrixD worldMatrix = this.WorldMatrix;
        Vector3D down = worldMatrix.Down;
        worldMatrix = this.WorldMatrix;
        Vector3D forward = worldMatrix.Forward;
        MyParticlesManager.TryCreateParticleEffect("Smoke_Collector", MatrixD.CreateWorld(position2, down, forward), out MyParticleEffect _);
        if (this.m_soundEmitter != null)
          this.m_soundEmitter.PlaySound(this.m_actionSound);
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCollector, Vector3D>(this, (Func<MyCollector, Action<Vector3D>>) (x => new Action<Vector3D>(x.PlayActionSoundAndParticle)), position1);
      }
    }

    [Event(null, 221)]
    [Reliable]
    [Broadcast]
    private void PlayActionSoundAndParticle(Vector3D position)
    {
      Vector3D position1 = position;
      MatrixD worldMatrix = this.WorldMatrix;
      Vector3D down = worldMatrix.Down;
      worldMatrix = this.WorldMatrix;
      Vector3D forward = worldMatrix.Forward;
      MyParticlesManager.TryCreateParticleEffect("Smoke_Collector", MatrixD.CreateWorld(position1, down, forward), out MyParticleEffect _);
      if (this.m_soundEmitter == null)
        return;
      this.m_soundEmitter.PlaySound(this.m_actionSound);
    }

    private void Receiver_IsPoweredChanged() => this.UpdateIsWorking();

    protected override void OnStartWorking()
    {
      base.OnStartWorking();
      if (!this.ShouldHavePhantom)
        return;
      MyPhysicsBody physics = this.Physics;
      if (physics == null || physics.Enabled)
        return;
      physics.Enabled = true;
      this.CreatePhantomConstraint();
    }

    protected override void OnStopWorking()
    {
      base.OnStopWorking();
      if (this.Physics == null)
        return;
      this.DisposePhantomContraint();
      this.Physics.Enabled = false;
    }

    public override void OnDestroy()
    {
      this.ReleaseInventory(MyEntityExtensions.GetInventory(this));
      base.OnDestroy();
    }

    public override void OnRemovedByCubeBuilder()
    {
      this.ReleaseInventory(MyEntityExtensions.GetInventory(this));
      base.OnRemovedByCubeBuilder();
    }

    private void LoadDummies()
    {
      foreach (KeyValuePair<string, MyModelDummy> dummy in MyModels.GetModelOnlyDummies(this.BlockDefinition.Model).Dummies)
      {
        if (dummy.Key.ToLower().Contains("collector"))
        {
          Matrix matrix = dummy.Value.Matrix;
          Vector3 halfExtents;
          this.GetBoxFromMatrix(matrix, out halfExtents, out Vector3 _, out Quaternion _);
          HkBvShape fieldShape = this.CreateFieldShape(halfExtents);
          this.Physics = new MyPhysicsBody((VRage.ModAPI.IMyEntity) this, RigidBodyFlag.RBF_UNLOCKED_SPEEDS);
          this.Physics.IsPhantom = true;
          this.Physics.CreateFromCollisionObject((HkShape) fieldShape, matrix.Translation, this.WorldMatrix, collisionFilter: 26);
          this.Physics.Enabled = true;
          this.Physics.RigidBody.ContactPointCallbackEnabled = false;
          fieldShape.Base.RemoveReference();
          break;
        }
      }
    }

    private void Inventory_ContentChangedCallback(MyInventoryBase inventory)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    private HkBvShape CreateFieldShape(Vector3 extents)
    {
      HkPhantomCallbackShape phantomCallbackShape = new HkPhantomCallbackShape(new HkPhantomHandler(this.phantom_Enter), new HkPhantomHandler(this.phantom_Leave));
      return new HkBvShape((HkShape) new HkBoxShape(extents), (HkShape) phantomCallbackShape, HkReferencePolicy.TakeOwnership);
    }

    private void phantom_Leave(HkPhantomCallbackShape shape, HkRigidBody body)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || this.m_isCollecting)
        return;
      List<VRage.ModAPI.IMyEntity> allEntities = body.GetAllEntities();
      foreach (VRage.ModAPI.IMyEntity myEntity in allEntities)
        this.m_entitiesToTake.Remove(myEntity as MyFloatingObject);
      allEntities.Clear();
    }

    private void phantom_Enter(HkPhantomCallbackShape shape, HkRigidBody body)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      List<VRage.ModAPI.IMyEntity> allEntities = body.GetAllEntities();
      foreach (VRage.ModAPI.IMyEntity myEntity in allEntities)
      {
        if (myEntity is MyFloatingObject)
        {
          this.m_entitiesToTake.Add(myEntity as MyFloatingObject);
          MySandboxGame.Static.Invoke((Action) (() => this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME), "MyCollector::NeedsUpdate");
        }
      }
      allEntities.Clear();
    }

    private void RigidBody_ContactPointCallback(ref HkContactPointEvent value)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      VRage.ModAPI.IMyEntity otherEntity = value.GetOtherEntity((VRage.ModAPI.IMyEntity) this);
      if (!(otherEntity is MyFloatingObject))
        return;
      this.m_entitiesToTake.Add(otherEntity as MyFloatingObject);
      MySandboxGame.Static.Invoke((Action) (() => this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME), "MyCollector::NeedsUpdate");
    }

    private void GetBoxFromMatrix(
      Matrix m,
      out Vector3 halfExtents,
      out Vector3 position,
      out Quaternion orientation)
    {
      MatrixD matrix = Matrix.Normalize(m) * this.WorldMatrix;
      orientation = Quaternion.CreateFromRotationMatrix(in matrix);
      halfExtents = Vector3.Abs(m.Scale) / 2f;
      halfExtents = new Vector3(halfExtents.X, halfExtents.Y, halfExtents.Z);
      position = (Vector3) matrix.Translation;
    }

    public override void OnCubeGridChanged(MyCubeGrid oldGrid)
    {
      base.OnCubeGridChanged(oldGrid);
      this.DisposePhantomContraint(oldGrid);
      if (!this.ShouldHavePhantom)
        return;
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    private void CreatePhantomConstraint()
    {
      if ((HkReferenceObject) this.m_phantomConstraint != (HkReferenceObject) null)
        this.DisposePhantomContraint();
      MyGridPhysics physics1 = this.CubeGrid.Physics;
      MyPhysicsBody physics2 = this.Physics;
      if (physics1 == null || physics2 == null || !physics2.Enabled)
        return;
      Matrix translation = Matrix.CreateTranslation(-physics2.Center);
      Matrix pivotA = this.PositionComp.LocalMatrixRef;
      Matrix pivotB = translation;
      HkFixedConstraintData data = new HkFixedConstraintData();
      data.SetInBodySpace(pivotA, pivotB, (MyPhysicsBody) physics1, physics2);
      this.m_phantomConstraint = new HkConstraint(physics1.RigidBody, physics2.RigidBody, (HkConstraintData) data);
      physics1.AddConstraint(this.m_phantomConstraint);
    }

    private void DisposePhantomContraint(MyCubeGrid oldGrid = null)
    {
      if ((HkReferenceObject) this.m_phantomConstraint == (HkReferenceObject) null)
        return;
      if (oldGrid == null)
        oldGrid = this.CubeGrid;
      oldGrid.Physics.RemoveConstraint(this.m_phantomConstraint);
      this.m_phantomConstraint.Dispose();
      this.m_phantomConstraint = (HkConstraint) null;
    }

    protected override void OnInventoryComponentAdded(MyInventoryBase inventory)
    {
      base.OnInventoryComponentAdded(inventory);
      if (MyEntityExtensions.GetInventory(this) == null)
        return;
      MyEntityExtensions.GetInventory(this).ContentsChanged += new Action<MyInventoryBase>(this.Inventory_ContentChangedCallback);
    }

    protected override void OnInventoryComponentRemoved(MyInventoryBase inventory)
    {
      base.OnInventoryComponentRemoved(inventory);
      if (!(inventory is MyInventory myInventory))
        return;
      myInventory.ContentsChanged -= new Action<MyInventoryBase>(this.Inventory_ContentChangedCallback);
    }

    public IMyConveyorEndpoint ConveyorEndpoint => (IMyConveyorEndpoint) this.m_multilineConveyorEndpoint;

    public void InitializeConveyorEndpoint()
    {
      this.m_multilineConveyorEndpoint = new MyMultilineConveyorEndpoint((MyCubeBlock) this);
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentDrawConveyorEndpoint((IMyConveyorEndpoint) this.m_multilineConveyorEndpoint));
    }

    private bool UseConveyorSystem
    {
      get => (bool) this.m_useConveyorSystem;
      set => this.m_useConveyorSystem.Value = value;
    }

    bool Sandbox.ModAPI.Ingame.IMyCollector.UseConveyorSystem
    {
      get => (bool) this.m_useConveyorSystem;
      set => this.m_useConveyorSystem.Value = value;
    }

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

    public PullInformation GetPullInformation() => (PullInformation) null;

    public PullInformation GetPushInformation() => new PullInformation()
    {
      Inventory = MyEntityExtensions.GetInventory(this),
      OwnerID = this.OwnerId,
      Constraint = new MyInventoryConstraint("Empty constraint")
    };

    public bool AllowSelfPulling() => false;

    protected sealed class PlayActionSoundAndParticle\u003C\u003EVRageMath_Vector3D : ICallSite<MyCollector, Vector3D, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCollector @this,
        in Vector3D position,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.PlayActionSoundAndParticle(position);
      }
    }

    protected class m_useConveyorSystem\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyCollector) obj0).m_useConveyorSystem = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_Blocks_MyCollector\u003C\u003EActor : IActivator, IActivator<MyCollector>
    {
      object IActivator.CreateInstance() => (object) new MyCollector();

      MyCollector IActivator<MyCollector>.CreateInstance() => new MyCollector();
    }
  }
}
