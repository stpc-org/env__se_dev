// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyMotorBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Entities.Cube
{
  [MyTerminalInterface(new System.Type[] {typeof (Sandbox.ModAPI.IMyMotorBase), typeof (Sandbox.ModAPI.Ingame.IMyMotorBase)})]
  public abstract class MyMotorBase : MyMechanicalConnectionBlockBase, Sandbox.ModAPI.IMyMotorBase, Sandbox.ModAPI.IMyMechanicalConnectionBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock, Sandbox.ModAPI.Ingame.IMyMotorBase
  {
    private const string ROTOR_DUMMY_KEY = "electric_motor";
    private new static List<HkBodyCollision> m_penetrations = new List<HkBodyCollision>();
    private Vector3 m_dummyPos;
    protected readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_dummyDisplacement;

    public Vector3 DummyPosition
    {
      get
      {
        Vector3 vector3_1 = Vector3.Zero;
        if ((double) this.m_dummyPos.Length() > 0.0)
        {
          Vector3 vector3_2 = Vector3.DominantAxisProjection(this.m_dummyPos);
          double num = (double) vector3_2.Normalize();
          vector3_1 = vector3_2 * (float) this.m_dummyDisplacement;
        }
        else
          vector3_1 = new Vector3(0.0f, (float) this.m_dummyDisplacement, 0.0f);
        return Vector3.Transform(this.m_dummyPos + vector3_1, this.PositionComp.LocalMatrixRef);
      }
    }

    public float DummyDisplacement
    {
      get => (float) this.m_dummyDisplacement + this.ModelDummyDisplacement;
      set
      {
        if (this.m_dummyDisplacement.Value.IsEqual(value - this.ModelDummyDisplacement))
          return;
        this.m_dummyDisplacement.Value = value - this.ModelDummyDisplacement;
      }
    }

    protected void CheckDisplacementLimits()
    {
      if (this.TopGrid == null)
        return;
      if (this.TopGrid.GridSizeEnum == MyCubeSize.Small)
      {
        if ((double) this.DummyDisplacement < (double) this.MotorDefinition.RotorDisplacementMinSmall)
          this.DummyDisplacement = this.MotorDefinition.RotorDisplacementMinSmall;
        if ((double) this.DummyDisplacement <= (double) this.MotorDefinition.RotorDisplacementMaxSmall)
          return;
        this.DummyDisplacement = this.MotorDefinition.RotorDisplacementMaxSmall;
      }
      else
      {
        if ((double) this.DummyDisplacement < (double) this.MotorDefinition.RotorDisplacementMin)
          this.DummyDisplacement = this.MotorDefinition.RotorDisplacementMin;
        if ((double) this.DummyDisplacement <= (double) this.MotorDefinition.RotorDisplacementMax)
          return;
        this.DummyDisplacement = this.MotorDefinition.RotorDisplacementMax;
      }
    }

    public MyCubeGrid RotorGrid => this.TopGrid;

    public MyCubeBlock Rotor => (MyCubeBlock) this.TopBlock;

    public float RequiredPowerInput => this.MotorDefinition.RequiredPowerInput;

    protected MyMotorStatorDefinition MotorDefinition => (MyMotorStatorDefinition) ((MyCubeBlock) this).BlockDefinition;

    protected virtual float ModelDummyDisplacement => 0.0f;

    public Vector3 RotorAngularVelocity => this.CubeGrid.Physics.RigidBody.AngularVelocity - this.TopGrid.Physics.RigidBody.AngularVelocity;

    public Vector3 AngularVelocity
    {
      get => this.TopGrid.Physics.RigidBody.AngularVelocity;
      set => this.TopGrid.Physics.RigidBody.AngularVelocity = value;
    }

    public float MaxRotorAngularVelocity => MyGridPhysics.GetShipMaxAngularVelocity(this.CubeGrid.GridSizeEnum);

    protected override bool CheckIsWorking() => this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.CheckIsWorking();

    protected virtual float ComputeRequiredPowerInput() => !base.CheckIsWorking() ? 0.0f : this.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId);

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.SyncFlag = true;
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(this.MotorDefinition.ResourceSinkGroup, this.MotorDefinition.RequiredPowerInput, new Func<float>(this.ComputeRequiredPowerInput));
      resourceSinkComponent.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
      this.ResourceSink = resourceSinkComponent;
      base.Init(objectBuilder, cubeGrid);
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.ResourceSink.Update();
      this.m_dummyDisplacement.SetLocalValue(0.0f);
      this.m_dummyDisplacement.ValueChanged += new Action<SyncBase>(this.m_dummyDisplacement_ValueChanged);
      this.LoadDummyPosition();
      MyObjectBuilder_MotorBase builderMotorBase = objectBuilder as MyObjectBuilder_MotorBase;
      if (Sandbox.Game.Multiplayer.Sync.IsServer && builderMotorBase.RotorEntityId.HasValue && builderMotorBase.RotorEntityId.Value != 0L)
        this.m_connectionState.Value = new MyMechanicalConnectionBlockBase.State()
        {
          TopBlockId = builderMotorBase.RotorEntityId,
          Welded = builderMotorBase.WeldedEntityId.HasValue || builderMotorBase.ForceWeld
        };
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentMotorBase(this));
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
    }

    private void m_dummyDisplacement_ValueChanged(SyncBase obj)
    {
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.CheckDisplacementLimits();
      if (!((HkReferenceObject) this.m_constraint != (HkReferenceObject) null))
        return;
      this.CubeGrid.Physics.RigidBody.Activate();
      if (this.TopGrid == null)
        return;
      this.TopGrid.Physics.RigidBody.Activate();
    }

    private void Receiver_IsPoweredChanged() => this.UpdateIsWorking();

    private void ComponentStack_IsFunctionalChanged() => this.ResourceSink.Update();

    protected override void OnEnabledChanged()
    {
      this.ResourceSink.Update();
      base.OnEnabledChanged();
    }

    private void LoadDummyPosition()
    {
      foreach (KeyValuePair<string, MyModelDummy> dummy in MyModels.GetModelOnlyDummies(((MyCubeBlock) this).BlockDefinition.Model).Dummies)
      {
        if (dummy.Key.StartsWith("electric_motor", StringComparison.InvariantCultureIgnoreCase))
        {
          this.m_dummyPos = Matrix.Normalize(dummy.Value.Matrix).Translation;
          break;
        }
      }
    }

    protected override MatrixD GetTopGridMatrix()
    {
      Vector3D position = Vector3D.Transform(this.DummyPosition, this.CubeGrid.WorldMatrix);
      MatrixD worldMatrix = this.WorldMatrix;
      Vector3D forward = worldMatrix.Forward;
      worldMatrix = this.WorldMatrix;
      Vector3D up = worldMatrix.Up;
      return MatrixD.CreateWorld(position, forward, up);
    }

    public override void UpdateBeforeSimulation100()
    {
      base.UpdateBeforeSimulation100();
      this.UpdateSoundState();
    }

    public override void ComputeTopQueryBox(
      out Vector3D pos,
      out Vector3 halfExtents,
      out Quaternion orientation)
    {
      MatrixD matrix = this.WorldMatrix;
      orientation = Quaternion.CreateFromRotationMatrix(in matrix);
      halfExtents = Vector3.One * this.CubeGrid.GridSize * 0.35f;
      halfExtents.Y = this.CubeGrid.GridSize * 0.25f;
      pos = matrix.Translation + 0.349999994039536 * (double) this.CubeGrid.GridSize * this.WorldMatrix.Up;
    }

    protected virtual void UpdateSoundState()
    {
      if (!MySandboxGame.IsGameReady || this.m_soundEmitter == null || !this.IsWorking)
        return;
      if (this.TopGrid == null || this.TopGrid.Physics == null)
      {
        this.m_soundEmitter.StopSound(true);
      }
      else
      {
        if (this.IsWorking && (double) Math.Abs(this.TopGrid.Physics.RigidBody.DeltaAngle.W) > 0.000250000011874363)
          this.m_soundEmitter.PlaySingleSound(((MyCubeBlock) this).BlockDefinition.PrimarySound, true);
        else
          this.m_soundEmitter.StopSound(false);
        if (this.m_soundEmitter.Sound == null || !this.m_soundEmitter.Sound.IsPlaying)
          return;
        this.m_soundEmitter.Sound.FrequencyRatio = MyAudio.Static.SemitonesToFrequencyRatio((float) (4.0 * ((double) Math.Abs(this.RotorAngularVelocity.Length()) - 0.5 * (double) this.MaxRotorAngularVelocity)) / this.MaxRotorAngularVelocity);
      }
    }

    protected override Vector3D TransformPosition(ref Vector3D position) => Vector3D.Transform(this.DummyPosition, this.CubeGrid.WorldMatrix);

    protected override void DisposeConstraint(MyCubeGrid topGrid)
    {
      if ((HkReferenceObject) this.m_constraint != (HkReferenceObject) null)
      {
        this.CubeGrid.Physics.RemoveConstraint(this.m_constraint);
        this.m_constraint.Dispose();
        this.m_constraint = (HkConstraint) null;
      }
      base.DisposeConstraint(topGrid);
    }

    VRage.Game.ModAPI.IMyCubeGrid Sandbox.ModAPI.IMyMotorBase.RotorGrid => (VRage.Game.ModAPI.IMyCubeGrid) this.TopGrid;

    VRage.Game.ModAPI.IMyCubeBlock Sandbox.ModAPI.IMyMotorBase.Rotor => (VRage.Game.ModAPI.IMyCubeBlock) this.TopBlock;

    event Action<Sandbox.ModAPI.IMyMotorBase> Sandbox.ModAPI.IMyMotorBase.AttachedEntityChanged
    {
      add => this.AttachedEntityChanged += this.GetDelegate(value);
      remove => this.AttachedEntityChanged -= this.GetDelegate(value);
    }

    private Action<MyMechanicalConnectionBlockBase> GetDelegate(
      Action<Sandbox.ModAPI.IMyMotorBase> value)
    {
      return (Action<MyMechanicalConnectionBlockBase>) Delegate.CreateDelegate(typeof (Action<MyMechanicalConnectionBlockBase>), value.Target, value.Method);
    }

    void Sandbox.ModAPI.IMyMotorBase.Attach(Sandbox.ModAPI.IMyMotorRotor rotor, bool updateGroup) => ((Sandbox.ModAPI.IMyMechanicalConnectionBlock) this).Attach((Sandbox.ModAPI.IMyAttachableTopBlock) rotor, updateGroup);

    protected class m_dummyDisplacement\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyMotorBase) obj0).m_dummyDisplacement = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }
  }
}
