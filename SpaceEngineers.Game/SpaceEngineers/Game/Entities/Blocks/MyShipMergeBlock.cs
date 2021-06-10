// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.Blocks.MyShipMergeBlock
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using SpaceEngineers.Game.EntityComponents.DebugRenders;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace SpaceEngineers.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_MergeBlock))]
  [MyTerminalInterface(new System.Type[] {typeof (SpaceEngineers.Game.ModAPI.IMyShipMergeBlock), typeof (SpaceEngineers.Game.ModAPI.Ingame.IMyShipMergeBlock)})]
  public class MyShipMergeBlock : MyFunctionalBlock, SpaceEngineers.Game.ModAPI.IMyShipMergeBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMyShipMergeBlock
  {
    private HkConstraint m_constraint;
    private MyShipMergeBlock m_other;
    private MyConcurrentHashSet<MyCubeGrid> m_gridList = new MyConcurrentHashSet<MyCubeGrid>();
    private ushort m_frameCounter;
    private MyShipMergeBlock.UpdateBeforeFlags m_updateBeforeFlags;
    private Base6Directions.Direction m_forward;
    private Base6Directions.Direction m_right;
    private Base6Directions.Direction m_otherRight;
    private VRage.Sync.Sync<MyShipMergeBlock.MergeState, SyncDirection.FromServer> m_mergeState;
    private bool HasConstraint;

    public bool InConstraint => (HkReferenceObject) this.m_constraint != (HkReferenceObject) null;

    private HkConstraint SafeConstraint
    {
      get
      {
        if ((HkReferenceObject) this.m_constraint != (HkReferenceObject) null && !this.m_constraint.InWorld)
          this.RemoveConstraintInBoth();
        return this.m_constraint;
      }
    }

    public MyShipMergeBlock Other => this.m_other ?? this.GetOtherMergeBlock();

    public int GridCount => this.m_gridList.Count;

    public Base6Directions.Direction OtherRight => this.m_otherRight;

    private bool IsWithinWorldLimits => MySession.Static.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.NONE || MySession.Static.MaxGridSize == 0 || this.CubeGrid.BlocksCount + this.m_other.CubeGrid.BlocksCount <= MySession.Static.MaxGridSize;

    public MyShipMergeBlock()
    {
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.m_mergeState.ValueChanged += (Action<SyncBase>) (x => this.UpdateEmissivity());
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      base.Init(objectBuilder, cubeGrid);
      this.LoadDummies();
      this.SlimBlock.DeformationRatio = (this.BlockDefinition as MyMergeBlockDefinition).DeformationRatio;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
      this.NeedsWorldMatrix = true;
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentShipMergeBlock(this));
    }

    protected override bool CheckIsWorking()
    {
      MyShipMergeBlock otherMergeBlock = this.GetOtherMergeBlock();
      return (otherMergeBlock == null || otherMergeBlock.FriendlyWithBlock((MyCubeBlock) this)) && base.CheckIsWorking();
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      this.IsWorkingChanged += new Action<MyCubeBlock>(this.MyShipMergeBlock_IsWorkingChanged);
      this.CheckConnectionAllowed = !this.IsWorking;
      this.Physics.Enabled = this.IsWorking;
      this.UpdateState();
      this.GetOtherMergeBlock()?.UpdateIsWorkingBeforeNextFrame();
    }

    public override void OnRemovedFromScene(object source)
    {
      base.OnRemovedFromScene(source);
      this.GetOtherMergeBlock()?.UpdateIsWorkingBeforeNextFrame();
      this.RemoveConstraintInBoth();
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.m_mergeState.Value = MyShipMergeBlock.MergeState.UNSET;
    }

    public void UpdateIsWorkingBeforeNextFrame()
    {
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      this.m_updateBeforeFlags |= MyShipMergeBlock.UpdateBeforeFlags.UpdateIsWorking;
    }

    private void LoadDummies()
    {
      foreach (KeyValuePair<string, MyModelDummy> dummy in MyModels.GetModelOnlyDummies(this.BlockDefinition.Model).Dummies)
      {
        if (dummy.Key.ToLower().Contains("merge"))
        {
          Matrix matrix = dummy.Value.Matrix;
          Vector3 extents = matrix.Scale / 2f;
          Vector3 vec = Vector3.DominantAxisProjection(matrix.Translation / extents);
          double num = (double) vec.Normalize();
          this.m_forward = Base6Directions.GetDirection(vec);
          this.m_right = Base6Directions.GetPerpendicular(this.m_forward);
          MatrixD worldTransform = MatrixD.Normalize((MatrixD) ref matrix) * this.WorldMatrix;
          HkBvShape fieldShape = this.CreateFieldShape(extents);
          this.Physics = new MyPhysicsBody((VRage.ModAPI.IMyEntity) this, RigidBodyFlag.RBF_STATIC | RigidBodyFlag.RBF_UNLOCKED_SPEEDS);
          this.Physics.IsPhantom = true;
          this.Physics.CreateFromCollisionObject((HkShape) fieldShape, matrix.Translation, worldTransform, collisionFilter: 24);
          this.Physics.Enabled = this.IsWorking;
          this.Physics.RigidBody.ContactPointCallbackEnabled = true;
          fieldShape.Base.RemoveReference();
          break;
        }
      }
    }

    private HkBvShape CreateFieldShape(Vector3 extents)
    {
      HkPhantomCallbackShape phantomCallbackShape = new HkPhantomCallbackShape(new HkPhantomHandler(this.phantom_Enter), new HkPhantomHandler(this.phantom_Leave));
      return new HkBvShape((HkShape) new HkBoxShape(extents), (HkShape) phantomCallbackShape, HkReferencePolicy.TakeOwnership);
    }

    private void phantom_Leave(HkPhantomCallbackShape shape, HkRigidBody body)
    {
      List<VRage.ModAPI.IMyEntity> allEntities = body.GetAllEntities();
      foreach (VRage.ModAPI.IMyEntity myEntity in allEntities)
        this.m_gridList.Remove(myEntity as MyCubeGrid);
      allEntities.Clear();
    }

    private void phantom_Enter(HkPhantomCallbackShape shape, HkRigidBody body)
    {
      List<VRage.ModAPI.IMyEntity> allEntities = body.GetAllEntities();
      foreach (VRage.ModAPI.IMyEntity myEntity in allEntities)
      {
        if (myEntity is MyCubeGrid instance && instance.GridSizeEnum == this.CubeGrid.GridSizeEnum && (instance != this.CubeGrid && !((HkReferenceObject) instance.Physics.RigidBody != (HkReferenceObject) body)))
          this.m_gridList.Add(instance);
      }
      allEntities.Clear();
    }

    private void CalculateMergeArea(out Vector3I minI, out Vector3I maxI)
    {
      Vector3I intVector = Base6Directions.GetIntVector(this.Orientation.TransformDirection(this.m_forward));
      minI = this.Min + intVector;
      maxI = this.Max + intVector;
      if (intVector.X + intVector.Y + intVector.Z == -1)
        maxI += (maxI - minI) * intVector;
      else
        minI += (maxI - minI) * intVector;
    }

    private MySlimBlock GetBlockInMergeArea()
    {
      Vector3I minI;
      Vector3I maxI;
      this.CalculateMergeArea(out minI, out maxI);
      Vector3I next = minI;
      Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref minI, ref maxI);
      while (vector3IRangeIterator.IsValid())
      {
        MySlimBlock cubeBlock = this.CubeGrid.GetCubeBlock(next);
        if (cubeBlock != null)
          return cubeBlock;
        vector3IRangeIterator.GetNext(out next);
      }
      return (MySlimBlock) null;
    }

    private MyShipMergeBlock GetOtherMergeBlock()
    {
      Vector3I minI1;
      Vector3I maxI1;
      this.CalculateMergeArea(out minI1, out maxI1);
      Vector3I next = minI1;
      Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref minI1, ref maxI1);
      while (vector3IRangeIterator.IsValid())
      {
        MySlimBlock cubeBlock = this.CubeGrid.GetCubeBlock(next);
        if (cubeBlock != null && cubeBlock.FatBlock != null && cubeBlock.FatBlock is MyShipMergeBlock fatBlock)
        {
          Vector3I minI2;
          Vector3I maxI2;
          fatBlock.CalculateMergeArea(out minI2, out maxI2);
          Vector3I intVector = Base6Directions.GetIntVector(this.Orientation.TransformDirection(this.m_forward));
          minI2 = maxI1 - (minI2 + intVector);
          maxI2 = maxI2 + intVector - minI1;
          if (minI2.X >= 0 && minI2.Y >= 0 && (minI2.Z >= 0 && maxI2.X >= 0) && (maxI2.Y >= 0 && maxI2.Z >= 0))
            return fatBlock;
        }
        vector3IRangeIterator.GetNext(out next);
      }
      return (MyShipMergeBlock) null;
    }

    private Vector3 GetMergeNormalWorld() => (Vector3) this.WorldMatrix.GetDirectionVector(this.m_forward);

    private void MyShipMergeBlock_IsWorkingChanged(MyCubeBlock obj)
    {
      if (this.Physics != null)
        this.Physics.Enabled = this.IsWorking;
      if (!this.IsWorking && this.GetOtherMergeBlock() == null && this.InConstraint)
        this.RemoveConstraintInBoth();
      this.CheckConnectionAllowed = !this.IsWorking;
      this.CubeGrid.UpdateBlockNeighbours(this.SlimBlock);
      this.UpdateState();
    }

    protected override void OnStopWorking()
    {
      this.UpdateState();
      base.OnStopWorking();
    }

    protected override void OnStartWorking()
    {
      this.UpdateState();
      base.OnStartWorking();
    }

    private void UpdateState()
    {
      if (!this.InScene || !Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      MyShipMergeBlock.MergeState mergeState = MyShipMergeBlock.MergeState.WORKING;
      MyShipMergeBlock otherMergeBlock = this.GetOtherMergeBlock();
      if (!this.IsWorking)
        mergeState = MyShipMergeBlock.MergeState.NONE;
      else if (otherMergeBlock != null && otherMergeBlock.IsWorking)
      {
        if (Base6Directions.GetFlippedDirection(otherMergeBlock.Orientation.TransformDirection(otherMergeBlock.m_forward)) == this.Orientation.TransformDirection(this.m_forward))
          mergeState = MyShipMergeBlock.MergeState.LOCKED;
      }
      else if (this.InConstraint)
        mergeState = MyShipMergeBlock.MergeState.CONSTRAINED;
      if (mergeState == (MyShipMergeBlock.MergeState) this.m_mergeState)
        return;
      this.m_mergeState.Value = mergeState;
      this.UpdateEmissivity();
      otherMergeBlock?.UpdateIsWorkingBeforeNextFrame();
    }

    public override void CheckEmissiveState(bool force)
    {
      if (force && Sandbox.Game.Multiplayer.Sync.IsServer)
        this.m_mergeState.Value = MyShipMergeBlock.MergeState.UNSET;
      this.UpdateState();
      if ((MyShipMergeBlock.MergeState) this.m_mergeState == MyShipMergeBlock.MergeState.UNSET)
        return;
      this.UpdateEmissivity();
    }

    private void UpdateEmissivity()
    {
      switch (this.m_mergeState.Value)
      {
        case MyShipMergeBlock.MergeState.NONE:
          MyShipMergeBlock otherMergeBlock = this.GetOtherMergeBlock();
          if (otherMergeBlock != null && !otherMergeBlock.FriendlyWithBlock((MyCubeBlock) this))
          {
            this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Working, this.Render.RenderObjectIDs[0]);
            break;
          }
          if (this.IsFunctional)
          {
            this.SetEmissiveStateDisabled();
            break;
          }
          this.SetEmissiveStateDamaged();
          break;
        case MyShipMergeBlock.MergeState.WORKING:
          this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Working, this.Render.RenderObjectIDs[0]);
          break;
        case MyShipMergeBlock.MergeState.CONSTRAINED:
          this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Constraint, this.Render.RenderObjectIDs[0]);
          break;
        case MyShipMergeBlock.MergeState.LOCKED:
          this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Locked, this.Render.RenderObjectIDs[0]);
          break;
      }
    }

    public override void OnModelChange()
    {
      base.OnModelChange();
      this.UpdateState();
    }

    protected override void OnOwnershipChanged()
    {
      base.OnOwnershipChanged();
      this.UpdateIsWorkingBeforeNextFrame();
    }

    private void CalculateMergeData(ref MyShipMergeBlock.MergeData data)
    {
      float num1 = this.BlockDefinition is MyMergeBlockDefinition blockDefinition ? blockDefinition.Strength : 0.1f;
      ref MyShipMergeBlock.MergeData local1 = ref data;
      MatrixD worldMatrix = this.WorldMatrix;
      Vector3D translation1 = worldMatrix.Translation;
      worldMatrix = this.m_other.WorldMatrix;
      Vector3D translation2 = worldMatrix.Translation;
      double num2 = (translation1 - translation2).Length() - (double) this.CubeGrid.GridSize;
      local1.Distance = (float) num2;
      data.StrengthFactor = (float) Math.Exp(-(double) data.Distance / (double) this.CubeGrid.GridSize);
      float num3 = MathHelper.Lerp(0.0f, num1 * (this.CubeGrid.GridSizeEnum == MyCubeSize.Large ? 0.005f : 0.1f), data.StrengthFactor);
      Vector3 velocityAtPoint1 = this.CubeGrid.Physics.GetVelocityAtPoint(this.PositionComp.GetPosition());
      Vector3 velocityAtPoint2 = this.m_other.CubeGrid.Physics.GetVelocityAtPoint(this.m_other.PositionComp.GetPosition());
      data.RelativeVelocity = velocityAtPoint2 - velocityAtPoint1;
      float num4 = data.RelativeVelocity.Length();
      float num5 = Math.Max((float) (3.59999990463257 / ((double) num4 > 0.100000001490116 ? (double) num4 : 0.100000001490116)), 1f);
      data.ConstraintStrength = num3 / num5;
      Vector3 vector3_1 = (Vector3) (this.m_other.PositionComp.GetPosition() - this.PositionComp.GetPosition());
      worldMatrix = this.WorldMatrix;
      Vector3 directionVector1 = (Vector3) worldMatrix.GetDirectionVector(this.m_forward);
      data.Distance = vector3_1.Length();
      data.PositionOk = (double) data.Distance < (double) this.CubeGrid.GridSize + 0.170000001788139;
      ref MyShipMergeBlock.MergeData local2 = ref data;
      Vector3 vector3_2 = directionVector1;
      worldMatrix = this.m_other.WorldMatrix;
      Vector3D directionVector2 = worldMatrix.GetDirectionVector(this.m_forward);
      double num6 = (vector3_2 + directionVector2).Length();
      local2.AxisDelta = (float) num6;
      data.AxisOk = (double) data.AxisDelta < 0.100000001490116;
      ref MyShipMergeBlock.MergeData local3 = ref data;
      worldMatrix = this.WorldMatrix;
      Vector3D directionVector3 = worldMatrix.GetDirectionVector(this.m_right);
      worldMatrix = this.m_other.WorldMatrix;
      Vector3D directionVector4 = worldMatrix.GetDirectionVector(this.m_other.m_otherRight);
      double num7 = (directionVector3 - directionVector4).Length();
      local3.RotationDelta = (float) num7;
      data.RotationOk = (double) data.RotationDelta < 0.0799999982118607;
    }

    private void DebugDrawInfo(Vector2 offset)
    {
      MyShipMergeBlock.MergeData data = new MyShipMergeBlock.MergeData();
      this.CalculateMergeData(ref data);
      MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, 75f) + offset, "x = " + data.StrengthFactor.ToString(), Color.Green, 0.8f);
      MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, 0.0f) + offset, "Merge block strength: " + data.ConstraintStrength.ToString(), Color.Green, 0.8f);
      MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, 15f) + offset, "Merge block dist: " + (data.Distance - this.CubeGrid.GridSize).ToString(), data.PositionOk ? Color.Green : Color.Red, 0.8f);
      MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, 30f) + offset, "Frame counter: " + this.m_frameCounter.ToString(), this.m_frameCounter >= (ushort) 6 ? Color.Green : Color.Red, 0.8f);
      MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, 45f) + offset, "Rotation difference: " + data.RotationDelta.ToString(), data.RotationOk ? Color.Green : Color.Red, 0.8f);
      MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, 60f) + offset, "Axis difference: " + data.AxisDelta.ToString(), data.AxisOk ? Color.Green : Color.Red, 0.8f);
      float num = data.RelativeVelocity.Length();
      MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, 90f) + offset, (double) num > 0.5 ? "Quick" : "Slow", (double) num > 0.5 ? Color.Red : Color.Green, 0.8f);
    }

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      if (!((HkReferenceObject) this.SafeConstraint != (HkReferenceObject) null))
        return;
      if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_CONNECTORS_AND_MERGE_BLOCKS && this.CustomName.ToString() == "DEBUG")
      {
        this.DebugDrawInfo(new Vector2(0.0f, 0.0f));
        this.m_other.DebugDrawInfo(new Vector2(0.0f, 120f));
        MyRenderProxy.DebugDrawLine3D(this.PositionComp.GetPosition(), this.PositionComp.GetPosition() + this.WorldMatrix.GetDirectionVector(this.m_right) * 10.0, Color.Red, Color.Red, false);
        MyRenderProxy.DebugDrawLine3D(this.m_other.PositionComp.GetPosition(), this.m_other.PositionComp.GetPosition() + this.m_other.WorldMatrix.GetDirectionVector(this.m_other.m_otherRight) * 10.0, Color.Red, Color.Red, false);
        MyRenderProxy.DebugDrawLine3D(this.PositionComp.GetPosition(), this.PositionComp.GetPosition() + this.WorldMatrix.GetDirectionVector(this.m_otherRight) * 5.0, Color.Yellow, Color.Yellow, false);
        MyRenderProxy.DebugDrawLine3D(this.m_other.PositionComp.GetPosition(), this.m_other.PositionComp.GetPosition() + this.m_other.WorldMatrix.GetDirectionVector(this.m_other.m_right) * 5.0, Color.Yellow, Color.Yellow, false);
      }
      Vector3 velocityAtPoint = this.CubeGrid.Physics.GetVelocityAtPoint(this.PositionComp.GetPosition());
      Vector3 vector3 = this.m_other.CubeGrid.Physics.GetVelocityAtPoint(this.m_other.PositionComp.GetPosition()) - velocityAtPoint;
      if ((double) vector3.Length() <= 0.5)
        return;
      if (!this.CubeGrid.Physics.IsStatic)
      {
        MyGridPhysics physics = this.CubeGrid.Physics;
        physics.LinearVelocity = physics.LinearVelocity + vector3 * 0.05f;
      }
      if (this.m_other.CubeGrid.Physics.IsStatic)
        return;
      MyGridPhysics physics1 = this.m_other.CubeGrid.Physics;
      physics1.LinearVelocity = physics1.LinearVelocity - vector3 * 0.05f;
    }

    public override void UpdateBeforeSimulation10()
    {
      base.UpdateBeforeSimulation10();
      if (!this.CheckUnobstructed())
      {
        if (!((HkReferenceObject) this.SafeConstraint != (HkReferenceObject) null))
          return;
        this.RemoveConstraintInBoth();
      }
      else if ((HkReferenceObject) this.SafeConstraint != (HkReferenceObject) null)
      {
        if ((this.CubeGrid.IsStatic ? 1 : (!this.m_other.CubeGrid.IsStatic ? 1 : 0)) == 0 || !this.IsWorking || (!this.m_other.IsWorking || !this.IsWithinWorldLimits))
          return;
        if (this.BlockDefinition is MyMergeBlockDefinition blockDefinition)
        {
          double strength = (double) blockDefinition.Strength;
        }
        if ((this.WorldMatrix.Translation - this.m_other.WorldMatrix.Translation).Length() - (double) this.CubeGrid.GridSize > (double) this.CubeGrid.GridSize * 3.0)
        {
          this.RemoveConstraintInBoth();
        }
        else
        {
          MyShipMergeBlock.MergeData data = new MyShipMergeBlock.MergeData();
          this.CalculateMergeData(ref data);
          (this.m_constraint.ConstraintData as HkMalleableConstraintData).Strength = data.ConstraintStrength;
          if (data.PositionOk && data.AxisOk && data.RotationOk)
          {
            if (this.m_frameCounter++ < (ushort) 3)
              return;
            Vector3I otherGridOffset1 = this.CalculateOtherGridOffset();
            Vector3I otherGridOffset2 = this.m_other.CalculateOtherGridOffset();
            if (!this.CubeGrid.CanMergeCubes(this.m_other.CubeGrid, otherGridOffset1))
            {
              if (!this.CubeGrid.GridSystems.ControlSystem.IsLocallyControlled && !this.m_other.CubeGrid.GridSystems.ControlSystem.IsLocallyControlled)
                return;
              MyHud.Notifications.Add(MyNotificationSingletons.ObstructingBlockDuringMerge);
            }
            else
            {
              if (this.BeforeMerge != null)
                this.BeforeMerge();
              if (!Sandbox.Game.Multiplayer.Sync.IsServer)
                return;
              foreach (MySlimBlock block in this.CubeGrid.GetBlocks())
              {
                if (block.FatBlock is MyShipMergeBlock fatBlock && fatBlock != this && fatBlock.InConstraint)
                  (block.FatBlock as MyShipMergeBlock).RemoveConstraintInBoth();
              }
              if (this.CubeGrid.MergeGrid_MergeBlock(this.m_other.CubeGrid, otherGridOffset1, true) == null)
                this.m_other.CubeGrid.MergeGrid_MergeBlock(this.CubeGrid, otherGridOffset2, false);
              this.RemoveConstraintInBoth();
            }
          }
          else
            this.m_frameCounter = (ushort) 0;
        }
      }
      else
      {
        foreach (MyCubeGrid grid in this.m_gridList)
        {
          if (!grid.MarkedForClose)
          {
            Vector3I zero = Vector3I.Zero;
            double maxValue = double.MaxValue;
            LineD line = new LineD(this.Physics.ClusterToWorld(this.Physics.RigidBody.Position), this.Physics.ClusterToWorld(this.Physics.RigidBody.Position) + this.GetMergeNormalWorld());
            if (grid.GetLineIntersectionExactGrid(ref line, ref zero, ref maxValue) && grid.GetCubeBlock(zero).FatBlock is MyShipMergeBlock fatBlock)
            {
              if (fatBlock.InConstraint || !fatBlock.IsWorking || !fatBlock.CheckUnobstructed() || ((double) fatBlock.GetMergeNormalWorld().Dot(this.GetMergeNormalWorld()) > 0.0 || !fatBlock.FriendlyWithBlock((MyCubeBlock) this)))
                break;
              this.CreateConstraint(grid, fatBlock);
              this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
              this.m_updateBeforeFlags |= MyShipMergeBlock.UpdateBeforeFlags.EnableConstraint;
              break;
            }
          }
        }
      }
    }

    private bool CheckUnobstructed() => this.GetBlockInMergeArea() == null;

    private Vector3I CalculateOtherGridOffset()
    {
      Vector3 vector3_1 = this.ConstraintPositionInGridSpace() / this.CubeGrid.GridSize;
      Vector3 position = -this.m_other.ConstraintPositionInGridSpace() / this.m_other.CubeGrid.GridSize;
      Base6Directions.Direction direction = this.Orientation.TransformDirection(this.m_right);
      Base6Directions.Direction newB = this.Orientation.TransformDirection(this.m_forward);
      Base6Directions.Direction flippedDirection = Base6Directions.GetFlippedDirection(this.m_other.Orientation.TransformDirection(this.m_other.m_forward));
      MatrixI rotation = MatrixI.CreateRotation(this.m_other.CubeGrid.WorldMatrix.GetClosestDirection(this.CubeGrid.WorldMatrix.GetDirectionVector(direction)), flippedDirection, direction, newB);
      Vector3 result;
      Vector3.Transform(ref position, ref rotation, out result);
      Vector3 vector3_2 = result;
      return Vector3I.Round(vector3_1 + vector3_2);
    }

    private Vector3 ConstraintPositionInGridSpace() => this.Position * this.CubeGrid.GridSize + this.PositionComp.LocalMatrixRef.GetDirectionVector(this.m_forward) * (this.CubeGrid.GridSize * 0.5f);

    private void CreateConstraint(MyCubeGrid other, MyShipMergeBlock block)
    {
      HkPrismaticConstraintData data = new HkPrismaticConstraintData();
      Vector3 posA = this.ConstraintPositionInGridSpace();
      Vector3 posB = block.ConstraintPositionInGridSpace();
      Vector3 directionVector1 = this.PositionComp.LocalMatrixRef.GetDirectionVector(this.m_forward);
      Vector3 directionVector2 = this.PositionComp.LocalMatrixRef.GetDirectionVector(this.m_right);
      Vector3 axisB = -block.PositionComp.LocalMatrixRef.GetDirectionVector(this.m_forward);
      Base6Directions.Direction closestDirection1 = block.WorldMatrix.GetClosestDirection(this.WorldMatrix.GetDirectionVector(this.m_right));
      Base6Directions.Direction closestDirection2 = this.WorldMatrix.GetClosestDirection(block.WorldMatrix.GetDirectionVector(block.m_right));
      Vector3 directionVector3 = block.PositionComp.LocalMatrixRef.GetDirectionVector(closestDirection1);
      data.SetInBodySpace(posA, posB, directionVector1, axisB, directionVector2, directionVector3, (MyPhysicsBody) this.CubeGrid.Physics, (MyPhysicsBody) other.Physics);
      HkMalleableConstraintData malleableConstraintData = new HkMalleableConstraintData();
      malleableConstraintData.SetData((HkConstraintData) data);
      data.ClearHandle();
      malleableConstraintData.Strength = 1E-05f;
      HkConstraint hkConstraint = new HkConstraint(this.CubeGrid.Physics.RigidBody, other.Physics.RigidBody, (HkConstraintData) malleableConstraintData);
      this.AddConstraint(hkConstraint);
      this.SetConstraint(block, hkConstraint, closestDirection2);
      this.m_other.SetConstraint(this, hkConstraint, closestDirection1);
    }

    private void AddConstraint(HkConstraint newConstraint)
    {
      this.HasConstraint = true;
      this.CubeGrid.Physics.AddConstraint(newConstraint);
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      if (this.m_updateBeforeFlags.HasFlag((Enum) MyShipMergeBlock.UpdateBeforeFlags.EnableConstraint))
      {
        if ((HkReferenceObject) this.SafeConstraint != (HkReferenceObject) null)
          this.m_constraint.Enabled = true;
      }
      else if (this.m_updateBeforeFlags.HasFlag((Enum) MyShipMergeBlock.UpdateBeforeFlags.UpdateIsWorking))
      {
        this.UpdateIsWorking();
        this.UpdateState();
      }
      this.m_updateBeforeFlags = MyShipMergeBlock.UpdateBeforeFlags.None;
    }

    public override bool ConnectionAllowed(
      ref Vector3I otherBlockPos,
      ref Vector3I faceNormal,
      MyCubeBlockDefinition def)
    {
      return this.ConnectionAllowedInternal(ref faceNormal, def);
    }

    public override bool ConnectionAllowed(
      ref Vector3I otherBlockMinPos,
      ref Vector3I otherBlockMaxPos,
      ref Vector3I faceNormal,
      MyCubeBlockDefinition def)
    {
      return this.ConnectionAllowedInternal(ref faceNormal, def);
    }

    private bool ConnectionAllowedInternal(ref Vector3I faceNormal, MyCubeBlockDefinition def) => this.IsWorking || def != this.BlockDefinition || this.Orientation.TransformDirectionInverse(Base6Directions.GetDirection(faceNormal)) != this.m_forward;

    protected void SetConstraint(
      MyShipMergeBlock otherBlock,
      HkConstraint constraint,
      Base6Directions.Direction otherRight)
    {
      if ((HkReferenceObject) this.m_constraint != (HkReferenceObject) null || this.m_other != null)
        return;
      this.m_constraint = constraint;
      this.m_other = otherBlock;
      this.m_otherRight = otherRight;
      this.UpdateState();
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
    }

    protected void RemoveConstraint()
    {
      this.m_constraint = (HkConstraint) null;
      this.m_other = (MyShipMergeBlock) null;
      this.UpdateState();
      if (this.HasDamageEffect)
        return;
      this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
    }

    protected void RemoveConstraintInBoth()
    {
      if (this.HasConstraint)
      {
        this.m_other.RemoveConstraint();
        this.CubeGrid.Physics.RemoveConstraint(this.m_constraint);
        this.m_constraint.Dispose();
        this.RemoveConstraint();
        this.HasConstraint = false;
      }
      else
      {
        if (this.m_other == null)
          return;
        this.m_other.RemoveConstraintInBoth();
      }
    }

    protected override void Closing()
    {
      base.Closing();
      if (!this.InConstraint)
        return;
      this.RemoveConstraintInBoth();
    }

    private event Action BeforeMerge;

    event Action SpaceEngineers.Game.ModAPI.IMyShipMergeBlock.BeforeMerge
    {
      add => this.BeforeMerge += value;
      remove => this.BeforeMerge += value;
    }

    public override int GetBlockSpecificState()
    {
      if ((MyShipMergeBlock.MergeState) this.m_mergeState == MyShipMergeBlock.MergeState.LOCKED)
        return 2;
      return (MyShipMergeBlock.MergeState) this.m_mergeState != MyShipMergeBlock.MergeState.CONSTRAINED ? 0 : 1;
    }

    public bool IsLocked => (MyShipMergeBlock.MergeState) this.m_mergeState == MyShipMergeBlock.MergeState.LOCKED;

    SpaceEngineers.Game.ModAPI.IMyShipMergeBlock SpaceEngineers.Game.ModAPI.IMyShipMergeBlock.Other => (SpaceEngineers.Game.ModAPI.IMyShipMergeBlock) this.Other;

    bool SpaceEngineers.Game.ModAPI.Ingame.IMyShipMergeBlock.IsConnected => this.Other != null;

    [Flags]
    private enum UpdateBeforeFlags : byte
    {
      None = 0,
      EnableConstraint = 1,
      UpdateIsWorking = 2,
    }

    private enum MergeState
    {
      UNSET,
      NONE,
      WORKING,
      CONSTRAINED,
      LOCKED,
    }

    private struct MergeData
    {
      public bool PositionOk;
      public bool RotationOk;
      public bool AxisOk;
      public float Distance;
      public float RotationDelta;
      public float AxisDelta;
      public float ConstraintStrength;
      public float StrengthFactor;
      public Vector3 RelativeVelocity;
    }

    protected class m_mergeState\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<MyShipMergeBlock.MergeState, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<MyShipMergeBlock.MergeState, SyncDirection.FromServer>(obj1, obj2));
        ((MyShipMergeBlock) obj0).m_mergeState = (VRage.Sync.Sync<MyShipMergeBlock.MergeState, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }
  }
}
