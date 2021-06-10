// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.StateGroups.MyCharacterPhysicsStateGroup
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication.History;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Networking;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Replication.StateGroups
{
  public class MyCharacterPhysicsStateGroup : MyEntityPhysicsStateGroupBase
  {
    public static MyTimeSpan ParentChangeTimeOut = MyTimeSpan.FromMilliseconds(100.0);
    public static readonly MyEntityPhysicsStateGroupBase.ParentingSetup JetpackParentingSetup = new MyEntityPhysicsStateGroupBase.ParentingSetup()
    {
      MaxParentDistance = 100f,
      MinParentSpeed = 20f,
      MaxParentAcceleration = 6f,
      MinInsideParentSpeed = 20f,
      MaxParentDisconnectDistance = 100f,
      MinDisconnectParentSpeed = 15f,
      MaxDisconnectParentAcceleration = 30f,
      MinDisconnectInsideParentSpeed = 10f
    };
    private const float FallMaxParentDisconnectDistance = 10f;
    private readonly List<MyEntity> m_tmpEntityResults = new List<MyEntity>();
    private MyTimeSpan m_lastTimestamp;
    private static readonly MyPredictedSnapshotSyncSetup m_controlledJetPackSettings;
    private static readonly MyPredictedSnapshotSyncSetup m_controlledJetPackMovingSettings;
    private static readonly MyPredictedSnapshotSyncSetup m_controlledJetPackNewParentSettings;
    private static readonly MyPredictedSnapshotSyncSetup m_controlledSettings;
    private static readonly MyPredictedSnapshotSyncSetup m_deadSettings;
    private static readonly MyPredictedSnapshotSyncSetup m_controlledAnimatedSettings;
    private static readonly MyPredictedSnapshotSyncSetup m_controlledMovingSettings;
    private static readonly MyPredictedSnapshotSyncSetup m_controlledNewParentSettings;
    private static readonly MyPredictedSnapshotSyncSetup m_settings;
    private static readonly MyPredictedSnapshotSyncSetup m_controlledLadderSettings;
    private readonly MyPredictedSnapshotSync m_predictedSync;
    private readonly IMySnapshotSync m_animatedSync;
    private long m_lastParentId;
    private MyTimeSpan m_lastParentTime;
    private MyTimeSpan m_lastAnimatedTime;
    private byte m_syncLinearVelocity = 2;
    private const float NEW_PARENT_TIMEOUT = 3f;
    private const float SYNC_CHANGE_TIMEOUT = 0.1f;
    public static float EXCESSIVE_CORRECTION_THRESHOLD;

    public MyCharacter Entity => (MyCharacter) base.Entity;

    public double AverageCorrection => this.m_predictedSync.AverageCorrection.Sum;

    public MyCharacterPhysicsStateGroup(MyEntity entity, IMyReplicable ownerReplicable)
      : base(entity, ownerReplicable, false)
    {
      this.m_predictedSync = new MyPredictedSnapshotSync((MyEntity) this.Entity);
      this.m_animatedSync = (IMySnapshotSync) new MyAnimatedSnapshotSync((MyEntity) this.Entity);
      this.m_snapshotSync = this.m_animatedSync;
      if (!Sync.IsServer)
        return;
      this.Entity.Hierarchy.OnParentChanged += new Action<MyHierarchyComponentBase, MyHierarchyComponentBase>(this.OnEntityParentChanged);
    }

    private void OnEntityParentChanged(
      MyHierarchyComponentBase oldParent,
      MyHierarchyComponentBase newParent)
    {
      if (oldParent == null || newParent != null)
        return;
      MyMultiplayer.GetReplicationServer().AddToDirtyGroups((IMyStateGroup) this);
    }

    public override void ClientUpdate(MyTimeSpan clientTimestamp)
    {
      bool controlledLocally = this.IsControlledLocally;
      int num1 = this.Entity.IsClientPredicted ? 1 : 0;
      bool flag1 = true;
      MyPredictedSnapshotSyncSetup snapshotSyncSetup = MyCharacterPhysicsStateGroup.m_settings;
      if (num1 != 0)
      {
        if (this.m_snapshotSync != this.m_predictedSync)
          this.m_lastAnimatedTime = MySandboxGame.Static.SimulationTime;
        this.m_snapshotSync = (IMySnapshotSync) this.m_predictedSync;
        long parentId = this.m_predictedSync.GetParentId();
        if (parentId != -1L)
          this.Entity.ClosestParentId = parentId;
        bool flag2 = MySandboxGame.Static.SimulationTime < this.m_lastParentTime + MyTimeSpan.FromSeconds(3.0);
        if (MySandboxGame.Static.SimulationTime < this.m_lastAnimatedTime + MyTimeSpan.FromSeconds(0.100000001490116))
          this.m_predictedSync.Reset(true);
        flag1 = this.m_predictedSync.Inited;
        bool flag3 = this.Entity.MoveIndicator != Vector3.Zero;
        if (controlledLocally)
          snapshotSyncSetup = !this.Entity.InheritRotation ? (flag2 ? MyCharacterPhysicsStateGroup.m_controlledJetPackNewParentSettings : (flag3 ? MyCharacterPhysicsStateGroup.m_controlledJetPackMovingSettings : MyCharacterPhysicsStateGroup.m_controlledJetPackSettings)) : (this.Entity.IsOnLadder || this.Entity.CurrentMovementState == MyCharacterMovementEnum.LadderOut ? MyCharacterPhysicsStateGroup.m_controlledLadderSettings : (flag2 ? MyCharacterPhysicsStateGroup.m_controlledNewParentSettings : (flag3 ? MyCharacterPhysicsStateGroup.m_controlledMovingSettings : MyCharacterPhysicsStateGroup.m_controlledSettings)));
      }
      else
      {
        if (controlledLocally)
          snapshotSyncSetup = MyCharacterPhysicsStateGroup.m_controlledAnimatedSettings;
        this.m_snapshotSync = this.m_animatedSync;
      }
      if (this.Entity.IsDead)
        snapshotSyncSetup = MyCharacterPhysicsStateGroup.m_deadSettings;
      long num2 = this.m_snapshotSync.Update(clientTimestamp, (MySnapshotSyncSetup) snapshotSyncSetup);
      if (num2 != -1L)
        this.Entity.ClosestParentId = num2;
      this.Entity.AlwaysDisablePrediction = MyPredictedSnapshotSync.ForceAnimated;
      this.Entity.LastSnapshotFlags = (MySnapshotFlags) snapshotSyncSetup;
      if (this.m_predictedSync.AverageCorrection.Sum > (double) MyCharacterPhysicsStateGroup.EXCESSIVE_CORRECTION_THRESHOLD)
      {
        this.Entity.ForceDisablePrediction = true;
        this.m_predictedSync.AverageCorrection.Reset();
      }
      if (!this.m_predictedSync.Inited || flag1 || (this.Entity.Physics == null || this.Entity.Physics.CharacterProxy == null))
        return;
      this.Entity.Physics.CharacterProxy.SetSupportedState(true);
    }

    public override void Serialize(
      BitStream stream,
      MyClientInfo forClient,
      MyTimeSpan serverTimestamp,
      MyTimeSpan lastClientTimestamp,
      byte packetId,
      int maxBitPosition,
      HashSet<string> cachedData)
    {
      if (stream.Writing)
      {
        this.UpdateEntitySupport();
        MySnapshot mySnapshot = new MySnapshot((MyEntity) this.Entity, forClient, inheritRotation: this.Entity.InheritRotation);
        if (this.Entity.Parent != null)
          mySnapshot.Active = false;
        mySnapshot.Write(stream);
        stream.WriteBool(true);
        this.Entity.SerializeControls(stream);
      }
      else
      {
        MySnapshot snapshot = new MySnapshot(stream);
        if (snapshot.ParentId != this.m_lastParentId)
        {
          this.m_lastParentId = snapshot.ParentId;
          if (this.m_supportInited)
            this.m_lastParentTime = MySandboxGame.Static.SimulationTime;
          else
            this.m_supportInited = true;
        }
        this.m_animatedSync.Read(ref snapshot, serverTimestamp);
        this.m_predictedSync.Read(ref snapshot, lastClientTimestamp);
        if (!stream.ReadBool())
          return;
        this.Entity.DeserializeControls(stream, false);
      }
    }

    private void UpdateEntitySupport()
    {
      MyTimeSpan simulationTime = MySandboxGame.Static.SimulationTime;
      if (this.m_lastTimestamp + MyCharacterPhysicsStateGroup.ParentChangeTimeOut > simulationTime || this.Entity.Physics == null)
        return;
      if (this.Entity.Parent != null)
      {
        this.m_lastSupportId = this.Entity.Parent.EntityId;
      }
      else
      {
        this.m_lastTimestamp = simulationTime;
        if ((this.Entity.JetpackRunning ? 1 : (this.Entity.IsDead ? 1 : 0)) == 0)
        {
          bool flag1 = false;
          if (this.Entity.Physics.CharacterProxy != null && this.Entity.Physics.CharacterProxy.Supported)
          {
            List<MyEntity> tmpEntityResults = this.m_tmpEntityResults;
            this.Entity.Physics.CharacterProxy.GetSupportingEntities(tmpEntityResults);
            bool flag2 = false;
            foreach (MyEntity myEntity in tmpEntityResults)
            {
              if (myEntity is MyCubeGrid || myEntity is MyVoxelBase)
              {
                this.m_supportInited = true;
                flag2 = true;
                if (myEntity.Physics.IsStatic)
                {
                  this.Entity.ClosestParentId = this.m_lastSupportId = 0L;
                  break;
                }
                this.Entity.ClosestParentId = this.m_lastSupportId = myEntity.EntityId;
                flag1 = true;
                break;
              }
            }
            if (tmpEntityResults.Count > 0 && !flag2)
              this.Entity.ClosestParentId = this.UpdateParenting(MyCharacterPhysicsStateGroup.JetpackParentingSetup, this.Entity.ClosestParentId);
            tmpEntityResults.Clear();
          }
          else if (this.Entity.IsOnLadder)
          {
            this.Entity.ClosestParentId = this.Entity.Ladder.CubeGrid.EntityId;
            flag1 = true;
          }
          if (flag1 || this.Entity.ClosestParentId == 0L)
            return;
          MyEntity entity;
          MyEntities.TryGetEntityById(this.Entity.ClosestParentId, out entity);
          if (!(entity is MyCubeGrid myCubeGrid) || myCubeGrid.PositionComp.WorldAABB.DistanceSquared(this.Entity.PositionComp.GetPosition()) <= 100.0)
            return;
          this.Entity.ClosestParentId = 0L;
        }
        else
          this.Entity.ClosestParentId = this.UpdateParenting(MyCharacterPhysicsStateGroup.JetpackParentingSetup, this.Entity.ClosestParentId);
      }
    }

    public override bool IsStillDirty(Endpoint forClient) => this.Entity.Parent == null;

    public override void Destroy()
    {
      if (Sync.IsServer)
        this.Entity.Hierarchy.OnParentChanged -= new Action<MyHierarchyComponentBase, MyHierarchyComponentBase>(this.OnEntityParentChanged);
      base.Destroy();
    }

    static MyCharacterPhysicsStateGroup()
    {
      MyPredictedSnapshotSyncSetup snapshotSyncSetup1 = new MyPredictedSnapshotSyncSetup();
      snapshotSyncSetup1.ProfileName = "ControlledJetpack";
      snapshotSyncSetup1.ApplyPosition = true;
      snapshotSyncSetup1.ApplyRotation = false;
      snapshotSyncSetup1.ApplyPhysicsAngular = false;
      snapshotSyncSetup1.ApplyPhysicsLinear = true;
      snapshotSyncSetup1.ExtrapolationSmoothing = true;
      snapshotSyncSetup1.InheritRotation = false;
      snapshotSyncSetup1.ApplyPhysicsLocal = true;
      snapshotSyncSetup1.IsControlled = true;
      snapshotSyncSetup1.AllowForceStop = true;
      snapshotSyncSetup1.MaxPositionFactor = 100f;
      snapshotSyncSetup1.MinPositionFactor = 100f;
      snapshotSyncSetup1.MaxLinearFactor = 1f;
      snapshotSyncSetup1.MaxRotationFactor = 1f;
      snapshotSyncSetup1.MaxAngularFactor = 1f;
      snapshotSyncSetup1.IterationsFactor = 0.3f;
      snapshotSyncSetup1.IgnoreParentId = true;
      snapshotSyncSetup1.UserTrend = true;
      MyCharacterPhysicsStateGroup.m_controlledJetPackSettings = snapshotSyncSetup1;
      MyPredictedSnapshotSyncSetup snapshotSyncSetup2 = new MyPredictedSnapshotSyncSetup();
      snapshotSyncSetup2.ProfileName = "ControlledJetpackMoving";
      snapshotSyncSetup2.ApplyPosition = true;
      snapshotSyncSetup2.ApplyRotation = false;
      snapshotSyncSetup2.ApplyPhysicsAngular = false;
      snapshotSyncSetup2.ApplyPhysicsLinear = true;
      snapshotSyncSetup2.ExtrapolationSmoothing = true;
      snapshotSyncSetup2.InheritRotation = false;
      snapshotSyncSetup2.ApplyPhysicsLocal = true;
      snapshotSyncSetup2.IsControlled = true;
      snapshotSyncSetup2.AllowForceStop = true;
      snapshotSyncSetup2.MaxPositionFactor = 100f;
      snapshotSyncSetup2.MaxLinearFactor = 1f;
      snapshotSyncSetup2.MaxRotationFactor = 1f;
      snapshotSyncSetup2.MaxAngularFactor = 1f;
      snapshotSyncSetup2.IterationsFactor = 0.3f;
      snapshotSyncSetup2.IgnoreParentId = true;
      snapshotSyncSetup2.UserTrend = true;
      MyCharacterPhysicsStateGroup.m_controlledJetPackMovingSettings = snapshotSyncSetup2;
      MyPredictedSnapshotSyncSetup snapshotSyncSetup3 = new MyPredictedSnapshotSyncSetup();
      snapshotSyncSetup3.ProfileName = "ControlledJetpackNewParent";
      snapshotSyncSetup3.ApplyPosition = true;
      snapshotSyncSetup3.ApplyRotation = false;
      snapshotSyncSetup3.ApplyPhysicsAngular = false;
      snapshotSyncSetup3.ApplyPhysicsLinear = true;
      snapshotSyncSetup3.ExtrapolationSmoothing = true;
      snapshotSyncSetup3.InheritRotation = false;
      snapshotSyncSetup3.ApplyPhysicsLocal = true;
      snapshotSyncSetup3.IsControlled = true;
      snapshotSyncSetup3.AllowForceStop = true;
      snapshotSyncSetup3.MaxPositionFactor = 100f;
      snapshotSyncSetup3.MaxLinearFactor = 1f;
      snapshotSyncSetup3.MaxRotationFactor = 1f;
      snapshotSyncSetup3.MaxAngularFactor = 1f;
      snapshotSyncSetup3.IterationsFactor = 1.5f;
      snapshotSyncSetup3.IgnoreParentId = true;
      snapshotSyncSetup3.UserTrend = true;
      MyCharacterPhysicsStateGroup.m_controlledJetPackNewParentSettings = snapshotSyncSetup3;
      MyPredictedSnapshotSyncSetup snapshotSyncSetup4 = new MyPredictedSnapshotSyncSetup();
      snapshotSyncSetup4.ProfileName = "ControlledCharacter";
      snapshotSyncSetup4.ApplyPosition = true;
      snapshotSyncSetup4.ApplyRotation = false;
      snapshotSyncSetup4.ApplyPhysicsAngular = false;
      snapshotSyncSetup4.ApplyPhysicsLinear = false;
      snapshotSyncSetup4.ExtrapolationSmoothing = true;
      snapshotSyncSetup4.InheritRotation = true;
      snapshotSyncSetup4.ApplyPhysicsLocal = true;
      snapshotSyncSetup4.IsControlled = true;
      snapshotSyncSetup4.AllowForceStop = true;
      snapshotSyncSetup4.MinPositionFactor = 100f;
      snapshotSyncSetup4.MaxPositionFactor = 10f;
      snapshotSyncSetup4.MaxLinearFactor = 100f;
      snapshotSyncSetup4.MaxRotationFactor = 100f;
      snapshotSyncSetup4.MaxAngularFactor = 1f;
      snapshotSyncSetup4.IterationsFactor = 0.3f;
      snapshotSyncSetup4.IgnoreParentId = true;
      snapshotSyncSetup4.UserTrend = true;
      MyCharacterPhysicsStateGroup.m_controlledSettings = snapshotSyncSetup4;
      MyPredictedSnapshotSyncSetup snapshotSyncSetup5 = new MyPredictedSnapshotSyncSetup();
      snapshotSyncSetup5.ProfileName = "DeadCharacter";
      snapshotSyncSetup5.ApplyPosition = false;
      snapshotSyncSetup5.ApplyRotation = false;
      snapshotSyncSetup5.ApplyPhysicsAngular = false;
      snapshotSyncSetup5.ApplyPhysicsLinear = false;
      snapshotSyncSetup5.ExtrapolationSmoothing = true;
      snapshotSyncSetup5.InheritRotation = true;
      snapshotSyncSetup5.ApplyPhysicsLocal = true;
      snapshotSyncSetup5.IsControlled = true;
      snapshotSyncSetup5.AllowForceStop = true;
      snapshotSyncSetup5.MinPositionFactor = 100f;
      snapshotSyncSetup5.MaxPositionFactor = 100f;
      snapshotSyncSetup5.MaxLinearFactor = 100f;
      snapshotSyncSetup5.MaxRotationFactor = 100f;
      snapshotSyncSetup5.MaxAngularFactor = 1f;
      snapshotSyncSetup5.IterationsFactor = 0.3f;
      snapshotSyncSetup5.IgnoreParentId = true;
      snapshotSyncSetup5.UserTrend = false;
      MyCharacterPhysicsStateGroup.m_deadSettings = snapshotSyncSetup5;
      MyPredictedSnapshotSyncSetup snapshotSyncSetup6 = new MyPredictedSnapshotSyncSetup();
      snapshotSyncSetup6.ProfileName = "ControlledAnimatedCharacter";
      snapshotSyncSetup6.ApplyPosition = true;
      snapshotSyncSetup6.ApplyRotation = false;
      snapshotSyncSetup6.ApplyPhysicsAngular = true;
      snapshotSyncSetup6.ApplyPhysicsLinear = true;
      snapshotSyncSetup6.ExtrapolationSmoothing = true;
      snapshotSyncSetup6.InheritRotation = true;
      snapshotSyncSetup6.ApplyPhysicsLocal = true;
      snapshotSyncSetup6.IsControlled = true;
      snapshotSyncSetup6.AllowForceStop = true;
      snapshotSyncSetup6.MinPositionFactor = 100f;
      snapshotSyncSetup6.MaxPositionFactor = 10f;
      snapshotSyncSetup6.MaxLinearFactor = 100f;
      snapshotSyncSetup6.MaxRotationFactor = 100f;
      snapshotSyncSetup6.MaxAngularFactor = 1f;
      snapshotSyncSetup6.IterationsFactor = 0.3f;
      snapshotSyncSetup6.IgnoreParentId = true;
      snapshotSyncSetup6.UserTrend = true;
      MyCharacterPhysicsStateGroup.m_controlledAnimatedSettings = snapshotSyncSetup6;
      MyPredictedSnapshotSyncSetup snapshotSyncSetup7 = new MyPredictedSnapshotSyncSetup();
      snapshotSyncSetup7.ProfileName = "ControlledCharacterMoving";
      snapshotSyncSetup7.ApplyPosition = true;
      snapshotSyncSetup7.ApplyRotation = false;
      snapshotSyncSetup7.ApplyPhysicsAngular = false;
      snapshotSyncSetup7.ApplyPhysicsLinear = false;
      snapshotSyncSetup7.ExtrapolationSmoothing = true;
      snapshotSyncSetup7.InheritRotation = true;
      snapshotSyncSetup7.ApplyPhysicsLocal = true;
      snapshotSyncSetup7.IsControlled = true;
      snapshotSyncSetup7.AllowForceStop = true;
      snapshotSyncSetup7.MaxPositionFactor = 10f;
      snapshotSyncSetup7.MaxLinearFactor = 100f;
      snapshotSyncSetup7.MaxRotationFactor = 100f;
      snapshotSyncSetup7.MaxAngularFactor = 1f;
      snapshotSyncSetup7.IterationsFactor = 0.3f;
      snapshotSyncSetup7.IgnoreParentId = true;
      snapshotSyncSetup7.UserTrend = true;
      MyCharacterPhysicsStateGroup.m_controlledMovingSettings = snapshotSyncSetup7;
      MyPredictedSnapshotSyncSetup snapshotSyncSetup8 = new MyPredictedSnapshotSyncSetup();
      snapshotSyncSetup8.ProfileName = "ControlledCharacterNewParent";
      snapshotSyncSetup8.ApplyPosition = true;
      snapshotSyncSetup8.ApplyRotation = false;
      snapshotSyncSetup8.ApplyPhysicsAngular = false;
      snapshotSyncSetup8.ApplyPhysicsLinear = false;
      snapshotSyncSetup8.ExtrapolationSmoothing = true;
      snapshotSyncSetup8.InheritRotation = true;
      snapshotSyncSetup8.ApplyPhysicsLocal = true;
      snapshotSyncSetup8.IsControlled = true;
      snapshotSyncSetup8.AllowForceStop = true;
      snapshotSyncSetup8.MaxPositionFactor = 100f;
      snapshotSyncSetup8.MaxLinearFactor = 100f;
      snapshotSyncSetup8.MaxRotationFactor = 100f;
      snapshotSyncSetup8.MaxAngularFactor = 1f;
      snapshotSyncSetup8.IterationsFactor = 1.5f;
      snapshotSyncSetup8.IgnoreParentId = true;
      snapshotSyncSetup8.UserTrend = true;
      MyCharacterPhysicsStateGroup.m_controlledNewParentSettings = snapshotSyncSetup8;
      MyPredictedSnapshotSyncSetup snapshotSyncSetup9 = new MyPredictedSnapshotSyncSetup();
      snapshotSyncSetup9.ProfileName = "GeneralCharacter";
      snapshotSyncSetup9.ApplyPosition = true;
      snapshotSyncSetup9.ApplyRotation = true;
      snapshotSyncSetup9.ApplyPhysicsAngular = false;
      snapshotSyncSetup9.ApplyPhysicsLinear = true;
      snapshotSyncSetup9.ExtrapolationSmoothing = true;
      snapshotSyncSetup9.ApplyPhysicsLocal = true;
      snapshotSyncSetup9.IsControlled = false;
      snapshotSyncSetup9.MaxPositionFactor = 100f;
      snapshotSyncSetup9.MaxLinearFactor = 100f;
      snapshotSyncSetup9.MaxRotationFactor = 180f;
      snapshotSyncSetup9.MaxAngularFactor = 1f;
      snapshotSyncSetup9.IterationsFactor = 0.25f;
      snapshotSyncSetup9.IgnoreParentId = true;
      snapshotSyncSetup9.UserTrend = true;
      MyCharacterPhysicsStateGroup.m_settings = snapshotSyncSetup9;
      MyPredictedSnapshotSyncSetup snapshotSyncSetup10 = new MyPredictedSnapshotSyncSetup();
      snapshotSyncSetup10.ProfileName = "ControlledLadderCharacter";
      snapshotSyncSetup10.ApplyPosition = false;
      snapshotSyncSetup10.ApplyRotation = false;
      snapshotSyncSetup10.ApplyPhysicsAngular = false;
      snapshotSyncSetup10.ApplyPhysicsLinear = false;
      snapshotSyncSetup10.ExtrapolationSmoothing = true;
      snapshotSyncSetup10.InheritRotation = true;
      snapshotSyncSetup10.ApplyPhysicsLocal = true;
      snapshotSyncSetup10.IsControlled = true;
      snapshotSyncSetup10.AllowForceStop = true;
      snapshotSyncSetup10.MinPositionFactor = 100f;
      snapshotSyncSetup10.MaxPositionFactor = 10f;
      snapshotSyncSetup10.MaxLinearFactor = 100f;
      snapshotSyncSetup10.MaxRotationFactor = 100f;
      snapshotSyncSetup10.MaxAngularFactor = 1f;
      snapshotSyncSetup10.IterationsFactor = 0.3f;
      snapshotSyncSetup10.IgnoreParentId = true;
      snapshotSyncSetup10.UserTrend = true;
      MyCharacterPhysicsStateGroup.m_controlledLadderSettings = snapshotSyncSetup10;
      MyCharacterPhysicsStateGroup.EXCESSIVE_CORRECTION_THRESHOLD = 20f;
    }
  }
}
