// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MyMechanicalConnectionBlockBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Blocks
{
  [MyTerminalInterface(new System.Type[] {typeof (Sandbox.ModAPI.IMyMechanicalConnectionBlock), typeof (Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock)})]
  public abstract class MyMechanicalConnectionBlockBase : MyFunctionalBlock, Sandbox.ModAPI.IMyMechanicalConnectionBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock
  {
    protected readonly VRage.Sync.Sync<MyMechanicalConnectionBlockBase.State, SyncDirection.FromServer> m_connectionState;
    protected VRage.Sync.Sync<bool, SyncDirection.BothWays> m_forceWeld;
    protected VRage.Sync.Sync<float, SyncDirection.BothWays> m_weldSpeed;
    private float m_weldSpeedSq;
    private float m_unweldSpeedSq;
    private VRage.Sync.Sync<bool, SyncDirection.BothWays> m_shareInertiaTensor;
    private MyAttachableTopBlockBase m_topBlock;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_safetyDetach;
    protected static List<HkBodyCollision> m_penetrations = new List<HkBodyCollision>();
    protected static HashSet<MySlimBlock> m_tmpSet = new HashSet<MySlimBlock>();
    protected HkConstraint m_constraint;
    private bool m_needReattach;
    private bool m_updateAttach;

    private bool ShareInertiaTensor
    {
      get => (bool) this.m_shareInertiaTensor;
      set
      {
        if (!MyCubeBlock.AllowExperimentalValues)
          return;
        this.m_shareInertiaTensor.Value = value;
      }
    }

    public MyCubeGrid TopGrid => this.TopBlock == null ? (MyCubeGrid) null : this.TopBlock.CubeGrid;

    public MyAttachableTopBlockBase TopBlock => this.m_topBlock;

    protected bool m_isWelding { get; private set; }

    protected bool m_welded { get; private set; }

    protected bool m_isAttached { get; private set; }

    public float SafetyDetach
    {
      get => this.m_safetyDetach.Value;
      set => this.m_safetyDetach.Value = value;
    }

    protected event Action<MyMechanicalConnectionBlockBase> AttachedEntityChanged;

    private MyMechanicalConnectionBlockBaseDefinition BlockDefinition => (MyMechanicalConnectionBlockBaseDefinition) base.BlockDefinition;

    public MyMechanicalConnectionBlockBase()
    {
      this.m_connectionState.ValueChanged += (Action<SyncBase>) (o => this.OnAttachTargetChanged());
      this.m_connectionState.Validate = new SyncValidate<MyMechanicalConnectionBlockBase.State>(this.ValidateTopBlockId);
      this.CreateTerminalControls();
      this.m_updateAttach = true;
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyMechanicalConnectionBlockBase>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlSlider<MyMechanicalConnectionBlockBase> slider1 = new MyTerminalControlSlider<MyMechanicalConnectionBlockBase>("Weld speed", MySpaceTexts.BlockPropertyTitle_WeldSpeed, MySpaceTexts.Blank);
      slider1.SetLimits((MyTerminalValueControl<MyMechanicalConnectionBlockBase, float>.GetterDelegate) (block => 0.0f), (MyTerminalValueControl<MyMechanicalConnectionBlockBase, float>.GetterDelegate) (block => MyGridPhysics.SmallShipMaxLinearVelocity()));
      slider1.DefaultValueGetter = (MyTerminalValueControl<MyMechanicalConnectionBlockBase, float>.GetterDelegate) (block => MyGridPhysics.LargeShipMaxLinearVelocity() - 5f);
      slider1.Visible = (Func<MyMechanicalConnectionBlockBase, bool>) (x => false);
      slider1.Getter = (MyTerminalValueControl<MyMechanicalConnectionBlockBase, float>.GetterDelegate) (x => (float) x.m_weldSpeed);
      slider1.Setter = (MyTerminalValueControl<MyMechanicalConnectionBlockBase, float>.SetterDelegate) ((x, v) => x.m_weldSpeed.Value = v);
      slider1.Writer = (MyTerminalControl<MyMechanicalConnectionBlockBase>.WriterDelegate) ((x, res) => res.AppendDecimal((float) Math.Sqrt((double) x.m_weldSpeedSq), 1).Append("m/s"));
      slider1.EnableActions<MyMechanicalConnectionBlockBase>();
      MyTerminalControlFactory.AddControl<MyMechanicalConnectionBlockBase>((MyTerminalControl<MyMechanicalConnectionBlockBase>) slider1);
      MyTerminalControlCheckbox<MyMechanicalConnectionBlockBase> checkbox = new MyTerminalControlCheckbox<MyMechanicalConnectionBlockBase>("Force weld", MySpaceTexts.BlockPropertyTitle_WeldForce, MySpaceTexts.Blank);
      checkbox.Visible = (Func<MyMechanicalConnectionBlockBase, bool>) (x => false);
      checkbox.Getter = (MyTerminalValueControl<MyMechanicalConnectionBlockBase, bool>.GetterDelegate) (x => (bool) x.m_forceWeld);
      checkbox.Setter = (MyTerminalValueControl<MyMechanicalConnectionBlockBase, bool>.SetterDelegate) ((x, v) => x.m_forceWeld.Value = v);
      checkbox.EnableAction<MyMechanicalConnectionBlockBase>();
      MyTerminalControlFactory.AddControl<MyMechanicalConnectionBlockBase>((MyTerminalControl<MyMechanicalConnectionBlockBase>) checkbox);
      MyTerminalControlSlider<MyMechanicalConnectionBlockBase> slider2 = new MyTerminalControlSlider<MyMechanicalConnectionBlockBase>("SafetyDetach", MySpaceTexts.BlockPropertyTitle_SafetyDetach, MySpaceTexts.BlockPropertyTooltip_SafetyDetach);
      slider2.Getter = (MyTerminalValueControl<MyMechanicalConnectionBlockBase, float>.GetterDelegate) (x => x.SafetyDetach);
      slider2.Setter = (MyTerminalValueControl<MyMechanicalConnectionBlockBase, float>.SetterDelegate) ((x, v) => x.SafetyDetach = v);
      slider2.DefaultValueGetter = (MyTerminalValueControl<MyMechanicalConnectionBlockBase, float>.GetterDelegate) (x => 5f);
      slider2.SetLimits((MyTerminalValueControl<MyMechanicalConnectionBlockBase, float>.GetterDelegate) (x => x.BlockDefinition.SafetyDetachMin), (MyTerminalValueControl<MyMechanicalConnectionBlockBase, float>.GetterDelegate) (x => x.BlockDefinition.SafetyDetachMax));
      slider2.Writer = (MyTerminalControl<MyMechanicalConnectionBlockBase>.WriterDelegate) ((x, result) => MyValueFormatter.AppendDistanceInBestUnit(x.SafetyDetach, result));
      slider2.Enabled = (Func<MyMechanicalConnectionBlockBase, bool>) (b => b.m_isAttached);
      slider2.EnableActions<MyMechanicalConnectionBlockBase>();
      MyTerminalControlFactory.AddControl<MyMechanicalConnectionBlockBase>((MyTerminalControl<MyMechanicalConnectionBlockBase>) slider2);
      if (!MySandboxGame.Config.ExperimentalMode || !this.AllowShareInertiaTensor())
        return;
      MyTerminalControlCheckbox<MyMechanicalConnectionBlockBase> sharedInertiaTensor = new MyTerminalControlCheckbox<MyMechanicalConnectionBlockBase>("ShareInertiaTensor", MySpaceTexts.BlockPropertyTitle_ShareTensor, MySpaceTexts.BlockPropertyTooltip_ShareTensor);
      sharedInertiaTensor.Enabled = (Func<MyMechanicalConnectionBlockBase, bool>) (x => MyCubeBlock.AllowExperimentalValues);
      sharedInertiaTensor.Visible = (Func<MyMechanicalConnectionBlockBase, bool>) (x => MyCubeBlock.AllowExperimentalValues);
      sharedInertiaTensor.Getter = (MyTerminalValueControl<MyMechanicalConnectionBlockBase, bool>.GetterDelegate) (x => MyMechanicalConnectionBlockBase.SetTensorUIColor(x, x.ShareInertiaTensor, sharedInertiaTensor));
      sharedInertiaTensor.Setter = (MyTerminalValueControl<MyMechanicalConnectionBlockBase, bool>.SetterDelegate) ((x, v) => x.ShareInertiaTensor = MyMechanicalConnectionBlockBase.SetTensorUIColor(x, v, sharedInertiaTensor));
      sharedInertiaTensor.EnableAction<MyMechanicalConnectionBlockBase>();
      MyTerminalControlFactory.AddControl<MyMechanicalConnectionBlockBase>((MyTerminalControl<MyMechanicalConnectionBlockBase>) sharedInertiaTensor);
    }

    protected virtual bool AllowShareInertiaTensor() => true;

    private static bool SetTensorUIColor(
      MyMechanicalConnectionBlockBase block,
      bool isUnsafeValue,
      MyTerminalControlCheckbox<MyMechanicalConnectionBlockBase> control)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsDedicated)
      {
        Vector4 vector4 = control.GetGuiControl().ColorMask;
        if (isUnsafeValue)
          vector4 = (Vector4) Color.Red;
        control.GetGuiControl().Elements[0].ColorMask = vector4;
      }
      return isUnsafeValue;
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      base.Init(objectBuilder, cubeGrid);
      MyObjectBuilder_MechanicalConnectionBlock mechanicalConnectionBlock = objectBuilder as MyObjectBuilder_MechanicalConnectionBlock;
      this.m_weldSpeed.SetLocalValue(MathHelper.Clamp(mechanicalConnectionBlock.WeldSpeed, 0.0f, MyGridPhysics.SmallShipMaxLinearVelocity()));
      this.m_forceWeld.SetLocalValue(mechanicalConnectionBlock.ForceWeld);
      if (mechanicalConnectionBlock.TopBlockId.HasValue && mechanicalConnectionBlock.TopBlockId.Value != 0L)
        this.m_connectionState.SetLocalValue(new MyMechanicalConnectionBlockBase.State()
        {
          TopBlockId = mechanicalConnectionBlock.TopBlockId,
          Welded = mechanicalConnectionBlock.IsWelded || mechanicalConnectionBlock.ForceWeld
        });
      if (!MyCubeBlock.AllowExperimentalValues || !this.AllowShareInertiaTensor())
        mechanicalConnectionBlock.ShareInertiaTensor = false;
      this.m_shareInertiaTensor.SetLocalValue(mechanicalConnectionBlock.ShareInertiaTensor);
      this.m_shareInertiaTensor.ValueChanged += new Action<SyncBase>(this.ShareInertiaTensor_ValueChanged);
      VRage.Sync.Sync<float, SyncDirection.BothWays> safetyDetach1 = this.m_safetyDetach;
      float? safetyDetach2 = mechanicalConnectionBlock.SafetyDetach;
      double num = (double) MathHelper.Clamp(safetyDetach2.HasValue ? safetyDetach2.GetValueOrDefault() : this.BlockDefinition.SafetyDetach, this.BlockDefinition.SafetyDetachMin, this.BlockDefinition.SafetyDetachMax + 0.1f);
      safetyDetach1.SetLocalValue((float) num);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    private void ShareInertiaTensor_ValueChanged(SyncBase obj)
    {
      this.UpdateSharedTensorState();
      this.OnUnsafeSettingsChanged();
    }

    private void UpdateSharedTensorState()
    {
      if (this.ShareInertiaTensor)
      {
        if (this.TopGrid == null)
          return;
        MySharedTensorsGroups.Link(this.CubeGrid, this.TopGrid, (MyCubeBlock) this);
      }
      else
        MySharedTensorsGroups.BreakLinkIfExists(this.CubeGrid, this.TopGrid, (MyCubeBlock) this);
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_MechanicalConnectionBlock builderCubeBlock = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_MechanicalConnectionBlock;
      builderCubeBlock.WeldSpeed = (float) this.m_weldSpeed;
      builderCubeBlock.ForceWeld = (bool) this.m_forceWeld;
      builderCubeBlock.TopBlockId = this.m_connectionState.Value.TopBlockId;
      builderCubeBlock.IsWelded = this.m_connectionState.Value.Welded;
      builderCubeBlock.SafetyDetach = new float?(this.SafetyDetach);
      builderCubeBlock.ShareInertiaTensor = this.ShareInertiaTensor && this.AllowShareInertiaTensor();
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      if (this.m_updateAttach)
        this.UpdateAttachState();
      if (this.m_needReattach)
        this.Reattach(this.TopGrid);
      this.OnUnsafeSettingsChanged();
      bool flag = this.CubeGrid != null && this.CubeGrid.IsPreview;
      if (!this.m_updateAttach && !this.m_needReattach || flag)
        return;
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      this.CheckSafetyDetach();
    }

    private bool ValidateTopBlockId(MyMechanicalConnectionBlockBase.State newState)
    {
      if (!newState.TopBlockId.HasValue)
        return true;
      long? topBlockId = newState.TopBlockId;
      long num = 0;
      return topBlockId.GetValueOrDefault() == num & topBlockId.HasValue && !this.m_connectionState.Value.TopBlockId.HasValue;
    }

    private void WeldSpeed_ValueChanged(SyncBase obj)
    {
      this.m_weldSpeedSq = (float) this.m_weldSpeed * (float) this.m_weldSpeed;
      this.m_unweldSpeedSq = Math.Max((float) this.m_weldSpeed - 2f, 0.0f);
      this.m_unweldSpeedSq *= this.m_unweldSpeedSq;
    }

    private void OnForceWeldChanged()
    {
      if (!this.m_isAttached || !Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      if ((bool) this.m_forceWeld)
      {
        if (this.m_welded)
          return;
        this.WeldGroup(true);
        this.SetDetailedInfoDirty();
      }
      else
        this.RaisePropertiesChanged();
    }

    private void OnAttachTargetChanged()
    {
      this.m_updateAttach = true;
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    protected void CheckSafetyDetach()
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || (HkReferenceObject) this.m_constraint == (HkReferenceObject) null)
        return;
      if (!this.CubeGrid.Physics.IsActive)
      {
        MyGridPhysics physics = this.TopGrid.Physics;
        if (physics != null && !physics.IsActive)
          return;
      }
      float safetyDetach = this.SafetyDetach;
      if ((double) this.GetConstraintDisplacementSq() <= (double) safetyDetach * (double) safetyDetach)
        return;
      this.Detach(true);
    }

    protected virtual float GetConstraintDisplacementSq()
    {
      Vector3 pivotA;
      Vector3 pivotB;
      this.m_constraint.GetPivotsInWorld(out pivotA, out pivotB);
      return (pivotA - pivotB).LengthSquared();
    }

    private void WeldGroup(bool force)
    {
      if (!MyFakes.WELD_ROTORS)
        return;
      this.m_isWelding = true;
      this.DisposeConstraint(this.TopGrid);
      MyWeldingGroups.Static.CreateLink(this.EntityId, (MyEntity) this.CubeGrid, (MyEntity) this.TopGrid);
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        MatrixD matrixD = this.TopBlock.CubeGrid.WorldMatrix * MatrixD.Invert(this.WorldMatrix);
        this.m_connectionState.Value = new MyMechanicalConnectionBlockBase.State()
        {
          TopBlockId = new long?(this.TopBlock.EntityId),
          Welded = true
        };
      }
      this.m_welded = true;
      this.m_isWelding = false;
      this.RaisePropertiesChanged();
    }

    private void UnweldGroup(MyCubeGrid topGrid)
    {
      if (!this.m_welded)
        return;
      this.m_isWelding = true;
      MyWeldingGroups.Static.BreakLink(this.EntityId, (MyEntity) this.CubeGrid, (MyEntity) topGrid);
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.m_connectionState.Value = new MyMechanicalConnectionBlockBase.State()
        {
          TopBlockId = new long?(this.TopBlock.EntityId),
          Welded = false
        };
      this.m_welded = false;
      this.m_isWelding = false;
      this.RaisePropertiesChanged();
    }

    private void cubeGrid_OnPhysicsChanged(MyEntity obj)
    {
      if (Sandbox.Game.Entities.MyEntities.IsClosingAll)
        return;
      this.cubeGrid_OnPhysicsChanged();
      if (this.TopGrid == null || this.CubeGrid == null)
        return;
      if (MyCubeGridGroups.Static.Logical.GetGroup(this.TopBlock.CubeGrid) != MyCubeGridGroups.Static.Logical.GetGroup(this.CubeGrid))
      {
        this.m_needReattach = true;
        this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      }
      else
      {
        if (this.TopGrid.Physics == null || this.CubeGrid.Physics == null)
          return;
        if ((HkReferenceObject) this.m_constraint != (HkReferenceObject) null)
        {
          if ((!((HkReferenceObject) this.m_constraint.RigidBodyA == (HkReferenceObject) this.CubeGrid.Physics.RigidBody) || !((HkReferenceObject) this.m_constraint.RigidBodyB == (HkReferenceObject) this.TopGrid.Physics.RigidBody) ? (!((HkReferenceObject) this.m_constraint.RigidBodyA == (HkReferenceObject) this.TopGrid.Physics.RigidBody) ? 0 : ((HkReferenceObject) this.m_constraint.RigidBodyB == (HkReferenceObject) this.CubeGrid.Physics.RigidBody ? 1 : 0)) : 1) != 0)
            return;
          this.m_needReattach = true;
          this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
        }
        else
        {
          if (this.m_welded)
            return;
          this.m_needReattach = (HkReferenceObject) this.TopGrid.Physics.RigidBody != (HkReferenceObject) this.CubeGrid.Physics.RigidBody;
          if (!this.m_needReattach)
            return;
          this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
        }
      }
    }

    protected virtual void cubeGrid_OnPhysicsChanged()
    {
    }

    private void TopBlock_OnClosing(MyEntity obj) => this.Detach(true);

    public override void OnUnregisteredFromGridSystems()
    {
      base.OnUnregisteredFromGridSystems();
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || !this.m_isAttached)
        return;
      this.m_needReattach = false;
      MyMechanicalConnectionBlockBase.State state = this.m_connectionState.Value;
      this.Detach(true);
      this.m_connectionState.Value = state;
    }

    public override void OnRegisteredToGridSystems()
    {
      base.OnRegisteredToGridSystems();
      this.m_updateAttach = true;
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    private void RaiseAttachedEntityChanged()
    {
      if (this.AttachedEntityChanged == null)
        return;
      this.AttachedEntityChanged(this);
    }

    public virtual void OnTopBlockCubeGridChanged(MyCubeGrid oldGrid)
    {
      MyAttachableTopBlockBase topBlock = this.TopBlock;
      this.Detach(oldGrid, true);
      this.m_connectionState.SetLocalValue(new MyMechanicalConnectionBlockBase.State()
      {
        TopBlockId = new long?(topBlock.EntityId),
        Welded = this.m_welded
      });
      this.MarkForReattach();
    }

    protected virtual void Detach(MyCubeGrid topGrid, bool updateGroups)
    {
      if (this.CubeGrid.Physics != null)
        this.CubeGrid.Physics.AddDirtyBlock(this.SlimBlock);
      if (this.m_welded)
        this.UnweldGroup(topGrid);
      if (updateGroups && !Sandbox.Game.Entities.MyEntities.IsClosingAll)
      {
        this.m_needReattach = false;
        this.BreakLinks(topGrid, this.TopBlock);
        if (Sandbox.Game.Multiplayer.Sync.IsServer)
          this.m_connectionState.Value = new MyMechanicalConnectionBlockBase.State()
          {
            TopBlockId = new long?(),
            Welded = false
          };
      }
      this.DisposeConstraint(topGrid);
      if (this.TopBlock != null)
        this.TopBlock.Detach(false);
      this.m_topBlock = (MyAttachableTopBlockBase) null;
      this.m_isAttached = false;
      if (!Sandbox.Game.Entities.MyEntities.IsClosingAll)
        this.SetDetailedInfoDirty();
      if (!updateGroups || Sandbox.Game.Entities.MyEntities.IsClosingAll)
        return;
      this.RaiseAttachedEntityChanged();
    }

    protected virtual void Detach(bool updateGroups = true) => this.Detach(this.TopGrid, updateGroups);

    protected virtual void DisposeConstraint(MyCubeGrid topGrid) => MySharedTensorsGroups.BreakLinkIfExists(this.CubeGrid, topGrid, (MyCubeBlock) this);

    protected virtual bool CreateConstraint(MyAttachableTopBlockBase top)
    {
      if (!this.CanAttach(top) || this.m_welded || !((HkReferenceObject) this.CubeGrid.Physics.RigidBody != (HkReferenceObject) top.CubeGrid.Physics.RigidBody))
        return false;
      this.UpdateSharedTensorState();
      return true;
    }

    protected virtual bool Attach(MyAttachableTopBlockBase topBlock, bool updateGroup = true)
    {
      if (topBlock.CubeGrid.Physics == null || this.CubeGrid.Physics == null || !this.CubeGrid.Physics.Enabled)
        return false;
      this.m_topBlock = topBlock;
      this.TopBlock.Attach(this);
      if (updateGroup)
      {
        this.CubeGrid.OnPhysicsChanged += new Action<MyEntity>(this.cubeGrid_OnPhysicsChanged);
        this.TopGrid.OnPhysicsChanged += new Action<MyEntity>(this.cubeGrid_OnPhysicsChanged);
        this.TopBlock.OnClosing += new Action<MyEntity>(this.TopBlock_OnClosing);
        if (this.CubeGrid != topBlock.CubeGrid)
        {
          this.OnConstraintAdded(GridLinkTypeEnum.Physical, (VRage.ModAPI.IMyEntity) this.TopGrid);
          this.OnConstraintAdded(GridLinkTypeEnum.Logical, (VRage.ModAPI.IMyEntity) this.TopGrid);
          this.OnConstraintAdded(GridLinkTypeEnum.Mechanical, (VRage.ModAPI.IMyEntity) this.TopGrid);
          MyGridPhysicalHierarchy.Static.CreateLink(this.EntityId, this.CubeGrid, this.TopGrid);
        }
        this.RaiseAttachedEntityChanged();
      }
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        MatrixD matrixD = this.TopBlock.CubeGrid.WorldMatrix * MatrixD.Invert(this.WorldMatrix);
        this.m_connectionState.Value = new MyMechanicalConnectionBlockBase.State()
        {
          TopBlockId = new long?(this.TopBlock.EntityId),
          Welded = this.m_welded
        };
      }
      this.m_isAttached = true;
      return true;
    }

    [Event(null, 671)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    protected void FindAndAttachTopServer()
    {
      MyAttachableTopBlockBase matchingTop = this.FindMatchingTop();
      if (matchingTop == null)
        return;
      this.TryAttach(matchingTop);
    }

    private void UpdateAttachState()
    {
      this.m_updateAttach = false;
      this.m_needReattach = false;
      if (!this.m_connectionState.Value.TopBlockId.HasValue)
      {
        if (this.m_isAttached)
          this.Detach(true);
      }
      else
      {
        long? topBlockId1 = this.m_connectionState.Value.TopBlockId;
        long num = 0;
        if (topBlockId1.GetValueOrDefault() == num & topBlockId1.HasValue)
        {
          if (this.m_isAttached)
            this.Detach(true);
          if (Sandbox.Game.Multiplayer.Sync.IsServer)
            this.FindAndAttachTopServer();
        }
        else
        {
          if (this.TopBlock != null)
          {
            long entityId = this.TopBlock.EntityId;
            long? topBlockId2 = this.m_connectionState.Value.TopBlockId;
            long valueOrDefault = topBlockId2.GetValueOrDefault();
            if (entityId == valueOrDefault & topBlockId2.HasValue)
            {
              if (this.m_welded != this.m_connectionState.Value.Welded)
              {
                if (this.m_connectionState.Value.Welded)
                {
                  this.WeldGroup(true);
                  goto label_20;
                }
                else
                {
                  this.UnweldGroup(this.TopGrid);
                  goto label_20;
                }
              }
              else
                goto label_20;
            }
          }
          long entityId1 = this.m_connectionState.Value.TopBlockId.Value;
          if (this.TopBlock != null)
            this.Detach(true);
          MyAttachableTopBlockBase top;
          ref MyAttachableTopBlockBase local = ref top;
          if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyAttachableTopBlockBase>(entityId1, out local) || !this.TryAttach(top))
          {
            if (Sandbox.Game.Multiplayer.Sync.IsServer && (top == null || top.MarkedForClose))
            {
              this.m_connectionState.Value = new MyMechanicalConnectionBlockBase.State()
              {
                TopBlockId = new long?()
              };
            }
            else
            {
              this.m_updateAttach = true;
              this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
            }
          }
        }
      }
label_20:
      this.RefreshConstraint();
    }

    private bool TryAttach(MyAttachableTopBlockBase top, bool updateGroup = true) => this.CanAttach(top) && this.Attach(top, updateGroup);

    private bool CanAttach(MyAttachableTopBlockBase top) => (this.MarkedForClose ? 1 : (this.CubeGrid.MarkedForClose ? 1 : 0)) == 0 && (top.MarkedForClose ? 1 : (top.CubeGrid.MarkedForClose ? 1 : 0)) == 0 && ((this.CubeGrid.Physics == null || (HkReferenceObject) this.CubeGrid.Physics.RigidBody == (HkReferenceObject) null ? 1 : (!this.CubeGrid.Physics.RigidBody.InWorld ? 1 : 0)) == 0 && ((top.CubeGrid.Physics == null || (HkReferenceObject) top.CubeGrid.Physics.RigidBody == (HkReferenceObject) null ? 1 : (!top.CubeGrid.Physics.RigidBody.InWorld ? 1 : 0)) == 0 && top.CubeGrid.Physics.HavokWorld == this.CubeGrid.Physics.HavokWorld));

    private bool CanPlaceTop()
    {
      Vector3D pos;
      Vector3 halfExtents;
      Quaternion orientation;
      this.ComputeTopQueryBox(out pos, out halfExtents, out orientation);
      using (MyUtils.ReuseCollection<HkBodyCollision>(ref MyMechanicalConnectionBlockBase.m_penetrations))
      {
        MyPhysics.GetPenetrationsBox(ref halfExtents, ref pos, ref orientation, MyMechanicalConnectionBlockBase.m_penetrations, 15);
        foreach (HkBodyCollision penetration in MyMechanicalConnectionBlockBase.m_penetrations)
        {
          VRage.ModAPI.IMyEntity collisionEntity = penetration.GetCollisionEntity();
          if (collisionEntity != null && collisionEntity != this.CubeGrid && collisionEntity is MyCubeGrid myCubeGrid)
          {
            Vector3D worldStart = this.TransformPosition(ref pos);
            Vector3I? nullable = myCubeGrid.RayCastBlocks(worldStart, worldStart + this.WorldMatrix.Up);
            if (nullable.HasValue)
              return myCubeGrid.GetCubeBlock(nullable.Value) == null;
          }
        }
      }
      return true;
    }

    private MyAttachableTopBlockBase FindMatchingTop()
    {
      Vector3D pos;
      Vector3 halfExtents;
      Quaternion orientation;
      this.ComputeTopQueryBox(out pos, out halfExtents, out orientation);
      using (MyUtils.ReuseCollection<HkBodyCollision>(ref MyMechanicalConnectionBlockBase.m_penetrations))
      {
        MyPhysics.GetPenetrationsBox(ref halfExtents, ref pos, ref orientation, MyMechanicalConnectionBlockBase.m_penetrations, 15);
        foreach (HkBodyCollision penetration in MyMechanicalConnectionBlockBase.m_penetrations)
        {
          VRage.ModAPI.IMyEntity collisionEntity = penetration.GetCollisionEntity();
          if (collisionEntity != null && collisionEntity != this.CubeGrid)
          {
            MyAttachableTopBlockBase topInGrid1 = this.FindTopInGrid(collisionEntity, pos);
            if (topInGrid1 != null)
              return topInGrid1;
            if (collisionEntity.Physics is MyPhysicsBody physics)
            {
              foreach (MyPhysicsComponentBase child in physics.WeldInfo.Children)
              {
                MyAttachableTopBlockBase topInGrid2 = this.FindTopInGrid(child.Entity, pos);
                if (topInGrid2 != null)
                  return topInGrid2;
              }
            }
          }
        }
      }
      return (MyAttachableTopBlockBase) null;
    }

    private MyAttachableTopBlockBase FindTopInGrid(
      VRage.ModAPI.IMyEntity entity,
      Vector3D pos)
    {
      if (entity is MyCubeGrid myCubeGrid)
      {
        Vector3D worldStart = this.TransformPosition(ref pos);
        Vector3I? nullable = myCubeGrid.RayCastBlocks(worldStart, worldStart + this.WorldMatrix.Up);
        if (nullable.HasValue)
        {
          MySlimBlock cubeBlock = myCubeGrid.GetCubeBlock(nullable.Value);
          if (cubeBlock != null && cubeBlock.FatBlock != null)
            return cubeBlock.FatBlock as MyAttachableTopBlockBase;
        }
      }
      return (MyAttachableTopBlockBase) null;
    }

    protected virtual Vector3D TransformPosition(ref Vector3D position) => position;

    public abstract void ComputeTopQueryBox(
      out Vector3D pos,
      out Vector3 halfExtents,
      out Quaternion orientation);

    public override void OnRemovedFromScene(object source)
    {
      base.OnRemovedFromScene(source);
      if (this.m_isAttached)
      {
        this.m_needReattach = false;
        MyMechanicalConnectionBlockBase.State state = this.m_connectionState.Value;
        this.Detach(true);
        if (Sandbox.Game.Multiplayer.Sync.IsServer)
          this.m_connectionState.Value = state;
      }
      this.m_shareInertiaTensor.ValueChanged -= new Action<SyncBase>(this.ShareInertiaTensor_ValueChanged);
    }

    public override void OnBuildSuccess(long builtBy, bool instantBuild)
    {
      base.OnBuildSuccess(builtBy, instantBuild);
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.CreateTopPartAndAttach(builtBy, false, instantBuild);
    }

    protected void RecreateTop(long? builderId = null, bool smallToLarge = false, bool instantBuild = false)
    {
      long num1 = builderId.HasValue ? builderId.Value : MySession.Static.LocalPlayerId;
      if (this.m_isAttached || !this.CanPlaceTop())
      {
        if (num1 != MySession.Static.LocalPlayerId)
          return;
        MyHud.Notifications.Add(MyNotificationSingletons.HeadAlreadyExists);
      }
      else
      {
        MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.TryGetDefinitionGroup(this.BlockDefinition.TopPart);
        MyCubeSize myCubeSize = this.CubeGrid.GridSizeEnum;
        if (smallToLarge && myCubeSize == MyCubeSize.Large)
          myCubeSize = MyCubeSize.Small;
        int num2 = (int) myCubeSize;
        MyCubeBlockDefinition cubeBlockDefinition = definitionGroup[(MyCubeSize) num2];
        bool flag = MySession.Static.CreativeToolsEnabled(MySession.Static.Players.TryGetSteamId(num1));
        if (!MySession.Static.CheckLimitsAndNotify(num1, cubeBlockDefinition.BlockPairName, flag ? cubeBlockDefinition.PCU : MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST, 1))
          return;
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyMechanicalConnectionBlockBase, long, bool, bool>(this, (Func<MyMechanicalConnectionBlockBase, Action<long, bool, bool>>) (x => new Action<long, bool, bool>(x.DoRecreateTop)), num1, smallToLarge, instantBuild);
      }
    }

    [Event(null, 948)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void DoRecreateTop(long builderId, bool smallToLarge, bool instantBuild)
    {
      if (this.TopBlock != null)
        return;
      this.CreateTopPartAndAttach(builderId, smallToLarge, instantBuild);
    }

    private void Reattach(MyCubeGrid topGrid)
    {
      this.m_needReattach = false;
      if (this.TopBlock == null)
      {
        MyLog.Default.WriteLine("TopBlock null in MechanicalConnection.Reatach");
        this.m_updateAttach = true;
        this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      }
      else
      {
        bool flag = MyCubeGridGroups.Static.Logical.GetGroup(topGrid) != MyCubeGridGroups.Static.Logical.GetGroup(this.CubeGrid);
        MyAttachableTopBlockBase topBlock = this.TopBlock;
        this.Detach(topGrid, flag);
        if (this.TryAttach(topBlock, flag))
        {
          if (topBlock.CubeGrid.Physics == null)
            return;
          topBlock.CubeGrid.Physics.ForceActivate();
        }
        else
        {
          if (!flag)
          {
            this.BreakLinks(topGrid, topBlock);
            this.RaiseAttachedEntityChanged();
          }
          if (!Sandbox.Game.Multiplayer.Sync.IsServer)
            return;
          this.m_connectionState.Value = new MyMechanicalConnectionBlockBase.State()
          {
            TopBlockId = new long?(0L)
          };
        }
      }
    }

    [Event(null, 996)]
    [Reliable]
    [Client]
    private void NotifyTopPartFailed(MySession.LimitResult result)
    {
      if (result == MySession.LimitResult.Passed)
        return;
      MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
      MyHud.Notifications.Add(MySession.GetNotificationForLimitResult(result));
    }

    private void BreakLinks(MyCubeGrid topGrid, MyAttachableTopBlockBase topBlock)
    {
      if (this.CubeGrid != this.TopGrid)
      {
        this.OnConstraintRemoved(GridLinkTypeEnum.Physical, (VRage.ModAPI.IMyEntity) topGrid);
        this.OnConstraintRemoved(GridLinkTypeEnum.Logical, (VRage.ModAPI.IMyEntity) topGrid);
        this.OnConstraintRemoved(GridLinkTypeEnum.Mechanical, (VRage.ModAPI.IMyEntity) topGrid);
        MyGridPhysicalHierarchy.Static.BreakLink(this.EntityId, this.CubeGrid, topGrid);
      }
      if (this.CubeGrid != null)
        this.CubeGrid.OnPhysicsChanged -= new Action<MyEntity>(this.cubeGrid_OnPhysicsChanged);
      if (topGrid != null)
        topGrid.OnPhysicsChanged -= new Action<MyEntity>(this.cubeGrid_OnPhysicsChanged);
      if (topBlock == null)
        return;
      topBlock.OnClosing -= new Action<MyEntity>(this.TopBlock_OnClosing);
    }

    private void CreateTopPartAndAttach(long builtBy, bool smallToLarge, bool instantBuild)
    {
      MyAttachableTopBlockBase topBlock;
      this.CreateTopPart(out topBlock, builtBy, MyDefinitionManager.Static.TryGetDefinitionGroup(this.BlockDefinition.TopPart), smallToLarge, instantBuild);
      if (topBlock == null)
        return;
      this.Attach(topBlock, true);
    }

    protected virtual bool CanPlaceRotor(MyAttachableTopBlockBase rotorBlock, long builtBy) => true;

    private void RefreshConstraint()
    {
      if (this.m_welded)
      {
        if (!((HkReferenceObject) this.m_constraint != (HkReferenceObject) null))
          return;
        this.DisposeConstraint(this.TopGrid);
      }
      else
      {
        bool flag = (HkReferenceObject) this.m_constraint == (HkReferenceObject) null;
        if ((HkReferenceObject) this.m_constraint != (HkReferenceObject) null && !this.m_constraint.InWorld)
        {
          this.DisposeConstraint(this.TopGrid);
          flag = true;
        }
        if (!flag || this.TopBlock == null)
          return;
        this.CreateConstraint(this.TopBlock);
        this.RaisePropertiesChanged();
      }
    }

    private void CreateTopPart(
      out MyAttachableTopBlockBase topBlock,
      long builtBy,
      MyCubeBlockDefinitionGroup topGroup,
      bool smallToLarge,
      bool instantBuild)
    {
      if (topGroup == null)
      {
        topBlock = (MyAttachableTopBlockBase) null;
      }
      else
      {
        MyCubeSize myCubeSize = this.CubeGrid.GridSizeEnum;
        if (smallToLarge && myCubeSize == MyCubeSize.Large)
          myCubeSize = MyCubeSize.Small;
        MatrixD topGridMatrix = this.GetTopGridMatrix();
        MyCubeBlockDefinition definition = topGroup[myCubeSize];
        ulong steamId = MySession.Static.Players.TryGetSteamId(builtBy);
        bool flag = MySession.Static.CreativeToolsEnabled(steamId);
        string failedBlockType = string.Empty;
        MySession.LimitResult limitResult = MySession.Static.IsWithinWorldLimits(out failedBlockType, builtBy, definition.BlockPairName, flag ? definition.PCU : MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST, 1);
        if (limitResult != MySession.LimitResult.Passed)
        {
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyMechanicalConnectionBlockBase, MySession.LimitResult>(this, (Func<MyMechanicalConnectionBlockBase, Action<MySession.LimitResult>>) (x => new Action<MySession.LimitResult>(x.NotifyTopPartFailed)), limitResult, new EndpointId(steamId));
          topBlock = (MyAttachableTopBlockBase) null;
        }
        else
        {
          MyObjectBuilder_CubeBlock blockObjectBuilder = MyCubeGrid.CreateBlockObjectBuilder(definition, Vector3I.Zero, MyBlockOrientation.Identity, MyEntityIdentifier.AllocateId(), this.BuiltBy, MySession.Static.CreativeMode | instantBuild);
          if ((Vector3) definition.Center != Vector3.Zero)
            topGridMatrix.Translation = Vector3D.Transform(-definition.Center * MyDefinitionManager.Static.GetCubeSize(myCubeSize), topGridMatrix);
          if (blockObjectBuilder is MyObjectBuilder_AttachableTopBlockBase attachableTopBlockBase)
            attachableTopBlockBase.YieldLastComponent = false;
          if (blockObjectBuilder is MyObjectBuilder_Wheel objectBuilderWheel)
            objectBuilderWheel.YieldLastComponent = false;
          MyObjectBuilder_CubeGrid newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_CubeGrid>();
          newObject.GridSizeEnum = myCubeSize;
          newObject.IsStatic = false;
          newObject.PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation(topGridMatrix));
          newObject.CubeBlocks.Add(blockObjectBuilder);
          MyCubeGrid entity = MyEntityFactory.CreateEntity<MyCubeGrid>((MyObjectBuilder_Base) newObject);
          entity.Init((MyObjectBuilder_EntityBase) newObject);
          topBlock = (MyAttachableTopBlockBase) entity.GetCubeBlock(Vector3I.Zero).FatBlock;
          if (!this.CanPlaceTop(topBlock, builtBy))
          {
            topBlock = (MyAttachableTopBlockBase) null;
            entity.Close();
          }
          else
          {
            Sandbox.Game.Entities.MyEntities.Add((MyEntity) entity);
            MatrixD matrixD = topBlock.CubeGrid.WorldMatrix * MatrixD.Invert(this.WorldMatrix);
            if (!Sandbox.Game.Multiplayer.Sync.IsServer)
              return;
            this.m_connectionState.Value = new MyMechanicalConnectionBlockBase.State()
            {
              TopBlockId = new long?(topBlock.EntityId)
            };
          }
        }
      }
    }

    protected abstract MatrixD GetTopGridMatrix();

    protected virtual bool CanPlaceTop(MyAttachableTopBlockBase topBlock, long builtBy) => true;

    public MyStringId GetAttachState()
    {
      if (this.m_welded || this.m_isWelding)
        return MySpaceTexts.BlockPropertiesText_MotorLocked;
      if (!this.m_connectionState.Value.TopBlockId.HasValue)
        return MySpaceTexts.BlockPropertiesText_MotorDetached;
      if (this.m_connectionState.Value.TopBlockId.Value == 0L)
        return MySpaceTexts.BlockPropertiesText_MotorAttachingAny;
      return this.m_isAttached ? MySpaceTexts.BlockPropertiesText_MotorAttached : MySpaceTexts.BlockPropertiesText_MotorAttachingSpecific;
    }

    protected HkConstraint SafeConstraint
    {
      get
      {
        this.RefreshConstraint();
        return this.m_constraint;
      }
    }

    protected void CallDetach()
    {
      if (!this.m_isAttached)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyMechanicalConnectionBlockBase>(this, (Func<MyMechanicalConnectionBlockBase, Action>) (x => new Action(x.DetachRequest)));
    }

    protected void CallAttach()
    {
      if (this.m_isAttached)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyMechanicalConnectionBlockBase>(this, (Func<MyMechanicalConnectionBlockBase, Action>) (x => new Action(x.FindAndAttachTopServer)));
    }

    public virtual Vector3? GetConstraintPosition(MyCubeGrid grid, bool opposite = false) => new Vector3?();

    public void MarkForReattach()
    {
      this.m_needReattach = true;
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    protected override bool HasUnsafeSettingsCollector() => this.ShareInertiaTensor || base.HasUnsafeSettingsCollector();

    [Event(null, 1205)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    protected void DetachRequest() => this.Detach(true);

    void Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock.Attach()
    {
      if (this.m_connectionState.Value.TopBlockId.HasValue)
        return;
      this.m_connectionState.Value = new MyMechanicalConnectionBlockBase.State()
      {
        TopBlockId = new long?(0L)
      };
    }

    void Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock.Detach() => this.m_connectionState.Value = new MyMechanicalConnectionBlockBase.State()
    {
      TopBlockId = new long?()
    };

    void Sandbox.ModAPI.IMyMechanicalConnectionBlock.Attach(
      Sandbox.ModAPI.IMyAttachableTopBlock top,
      bool updateGroup)
    {
      if (top == null)
        return;
      this.Attach((MyAttachableTopBlockBase) top, updateGroup);
    }

    bool Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock.IsAttached => this.m_isAttached;

    float Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock.SafetyLockSpeed
    {
      get => (float) this.m_weldSpeed;
      set
      {
        value = MathHelper.Clamp(value, 0.0f, this.CubeGrid.GridSizeEnum == MyCubeSize.Large ? MyGridPhysics.LargeShipMaxLinearVelocity() : MyGridPhysics.SmallShipMaxLinearVelocity());
        this.m_weldSpeed.Value = value;
      }
    }

    bool Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock.SafetyLock
    {
      get => this.m_isWelding || this.m_welded;
      set
      {
        if ((this.m_isWelding ? 1 : (this.m_welded ? 1 : 0)) == (value ? 1 : 0))
          return;
        this.m_forceWeld.Value = value;
      }
    }

    bool Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock.IsLocked => this.m_isWelding || this.m_welded;

    bool Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock.PendingAttachment => this.m_connectionState.Value.TopBlockId.HasValue && this.m_connectionState.Value.TopBlockId.Value == 0L;

    VRage.Game.ModAPI.IMyCubeGrid Sandbox.ModAPI.IMyMechanicalConnectionBlock.TopGrid => (VRage.Game.ModAPI.IMyCubeGrid) this.TopGrid;

    Sandbox.ModAPI.IMyAttachableTopBlock Sandbox.ModAPI.IMyMechanicalConnectionBlock.Top => (Sandbox.ModAPI.IMyAttachableTopBlock) this.TopBlock;

    VRage.Game.ModAPI.Ingame.IMyCubeGrid Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock.TopGrid => (VRage.Game.ModAPI.Ingame.IMyCubeGrid) this.TopGrid;

    Sandbox.ModAPI.Ingame.IMyAttachableTopBlock Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock.Top => (Sandbox.ModAPI.Ingame.IMyAttachableTopBlock) this.TopBlock;

    [Serializable]
    protected struct State
    {
      public long? TopBlockId;
      public bool Welded;

      protected class Sandbox_Game_Entities_Blocks_MyMechanicalConnectionBlockBase\u003C\u003EState\u003C\u003ETopBlockId\u003C\u003EAccessor : IMemberAccessor<MyMechanicalConnectionBlockBase.State, long?>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyMechanicalConnectionBlockBase.State owner, in long? value) => owner.TopBlockId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyMechanicalConnectionBlockBase.State owner, out long? value) => value = owner.TopBlockId;
      }

      protected class Sandbox_Game_Entities_Blocks_MyMechanicalConnectionBlockBase\u003C\u003EState\u003C\u003EWelded\u003C\u003EAccessor : IMemberAccessor<MyMechanicalConnectionBlockBase.State, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyMechanicalConnectionBlockBase.State owner, in bool value) => owner.Welded = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyMechanicalConnectionBlockBase.State owner, out bool value) => value = owner.Welded;
      }
    }

    protected sealed class FindAndAttachTopServer\u003C\u003E : ICallSite<MyMechanicalConnectionBlockBase, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyMechanicalConnectionBlockBase @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.FindAndAttachTopServer();
      }
    }

    protected sealed class DoRecreateTop\u003C\u003ESystem_Int64\u0023System_Boolean\u0023System_Boolean : ICallSite<MyMechanicalConnectionBlockBase, long, bool, bool, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyMechanicalConnectionBlockBase @this,
        in long builderId,
        in bool smallToLarge,
        in bool instantBuild,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.DoRecreateTop(builderId, smallToLarge, instantBuild);
      }
    }

    protected sealed class NotifyTopPartFailed\u003C\u003ESandbox_Game_World_MySession\u003C\u003ELimitResult : ICallSite<MyMechanicalConnectionBlockBase, MySession.LimitResult, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyMechanicalConnectionBlockBase @this,
        in MySession.LimitResult result,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.NotifyTopPartFailed(result);
      }
    }

    protected sealed class DetachRequest\u003C\u003E : ICallSite<MyMechanicalConnectionBlockBase, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyMechanicalConnectionBlockBase @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.DetachRequest();
      }
    }

    protected class m_connectionState\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<MyMechanicalConnectionBlockBase.State, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<MyMechanicalConnectionBlockBase.State, SyncDirection.FromServer>(obj1, obj2));
        ((MyMechanicalConnectionBlockBase) obj0).m_connectionState = (VRage.Sync.Sync<MyMechanicalConnectionBlockBase.State, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_forceWeld\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyMechanicalConnectionBlockBase) obj0).m_forceWeld = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_weldSpeed\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyMechanicalConnectionBlockBase) obj0).m_weldSpeed = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_shareInertiaTensor\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyMechanicalConnectionBlockBase) obj0).m_shareInertiaTensor = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_safetyDetach\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyMechanicalConnectionBlockBase) obj0).m_safetyDetach = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }
  }
}
