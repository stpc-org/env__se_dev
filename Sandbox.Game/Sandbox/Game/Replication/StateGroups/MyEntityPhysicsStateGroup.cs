// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.StateGroups.MyEntityPhysicsStateGroup
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Replication.History;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Game.Networking;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Replication.StateGroups
{
  public class MyEntityPhysicsStateGroup : MyEntityPhysicsStateGroupBase
  {
    private static readonly MyPredictedSnapshotSyncSetup m_settings;
    private static readonly MyPredictedSnapshotSyncSetup m_controlledSettings;
    private static readonly MyPredictedSnapshotSyncSetup m_controlledNewParentSettings;
    private static readonly MyPredictedSnapshotSyncSetup m_wheelSettings;
    private static readonly MyPredictedSnapshotSyncSetup m_carSettings;
    public static readonly MyEntityPhysicsStateGroupBase.ParentingSetup GridParentingSetup;
    private readonly MyPredictedSnapshotSync m_predictedSync;
    private readonly IMySnapshotSync m_animatedSync;
    private MyTimeSpan m_lastParentTime;
    private long m_lastParentId;
    private bool m_inheritRotation;
    private const float NEW_PARENT_TIMEOUT = 3f;
    private MyTimeSpan m_lastAnimatedTime;
    private const float SYNC_CHANGE_TIMEOUT = 0.1f;

    public MyEntityPhysicsStateGroup(MyEntity entity, IMyReplicable ownerReplicable)
      : base(entity, ownerReplicable, false)
    {
      this.m_predictedSync = new MyPredictedSnapshotSync(this.Entity);
      this.m_animatedSync = (IMySnapshotSync) new MyAnimatedSnapshotSync(this.Entity);
      this.m_snapshotSync = this.m_animatedSync;
    }

    public override void ClientUpdate(MyTimeSpan clientTimestamp)
    {
      MyPredictedSnapshotSyncSetup snapshotSyncSetup;
      if ((!(this.Entity is MyCubeGrid entity) ? 0 : (entity.IsClientPredicted ? 1 : 0)) != 0)
      {
        if (this.m_snapshotSync != this.m_predictedSync)
          this.m_lastAnimatedTime = MySandboxGame.Static.SimulationTime;
        this.m_snapshotSync = (IMySnapshotSync) this.m_predictedSync;
        if (!this.m_inheritRotation)
        {
          long parentId = this.m_predictedSync.GetParentId();
          if (parentId != -1L)
            entity.ClosestParentId = parentId;
        }
        else
          entity.ClosestParentId = 0L;
        bool flag = MySandboxGame.Static.SimulationTime < this.m_lastParentTime + MyTimeSpan.FromSeconds(3.0);
        if (MySandboxGame.Static.SimulationTime < this.m_lastAnimatedTime + MyTimeSpan.FromSeconds(0.100000001490116))
          this.m_predictedSync.Reset(true);
        snapshotSyncSetup = entity.IsClientPredictedWheel ? MyEntityPhysicsStateGroup.m_wheelSettings : (entity.IsClientPredictedCar ? MyEntityPhysicsStateGroup.m_carSettings : (flag ? MyEntityPhysicsStateGroup.m_controlledNewParentSettings : MyEntityPhysicsStateGroup.m_controlledSettings));
      }
      else
      {
        if (this.m_snapshotSync != this.m_animatedSync)
          this.m_animatedSync.Reset();
        this.m_snapshotSync = this.m_animatedSync;
        snapshotSyncSetup = MyEntityPhysicsStateGroup.m_settings;
      }
      long num = this.m_snapshotSync.Update(clientTimestamp, (MySnapshotSyncSetup) snapshotSyncSetup);
      if (entity != null)
      {
        if (!this.m_inheritRotation)
        {
          if (num != -1L)
            entity.ClosestParentId = num;
        }
        else
          entity.ClosestParentId = 0L;
        entity.ForceDisablePrediction = MyPredictedSnapshotSync.ForceAnimated;
      }
      this.Entity.LastSnapshotFlags = (MySnapshotFlags) snapshotSyncSetup;
    }

    private bool UpdateEntitySupport()
    {
      if (this.Entity is MyCubeGrid entity)
      {
        if (entity.IsClientPredicted)
        {
          entity.ClosestParentId = this.UpdateParenting(MyEntityPhysicsStateGroup.GridParentingSetup, entity.ClosestParentId);
          return false;
        }
        entity.ClosestParentId = 0L;
      }
      return true;
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
        bool inheritRotation = this.UpdateEntitySupport();
        MySnapshot mySnapshot = new MySnapshot(this.Entity, forClient, inheritRotation: inheritRotation);
        this.m_forcedWorldSnapshots = mySnapshot.SkippedParent;
        mySnapshot.Write(stream);
        stream.WriteBool(forClient.PlayerControllableUsesPredictedPhysics);
        stream.WriteBool(true);
        this.Entity.SerializeControls(stream);
      }
      else
      {
        MySnapshot snapshot = new MySnapshot(stream);
        this.m_inheritRotation = snapshot.InheritRotation;
        snapshot.GetMatrix(this.Entity, out MatrixD _);
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
        bool flag = stream.ReadBool();
        if (this.Entity is MyCubeGrid entity)
          entity.AllowPrediction = flag;
        if (!stream.ReadBool())
          return;
        this.Entity.DeserializeControls(stream, false);
      }
    }

    static MyEntityPhysicsStateGroup()
    {
      MyPredictedSnapshotSyncSetup snapshotSyncSetup1 = new MyPredictedSnapshotSyncSetup();
      snapshotSyncSetup1.ProfileName = "GeneralGrid";
      snapshotSyncSetup1.ApplyPosition = true;
      snapshotSyncSetup1.ApplyRotation = true;
      snapshotSyncSetup1.ApplyPhysicsAngular = true;
      snapshotSyncSetup1.ApplyPhysicsLinear = true;
      snapshotSyncSetup1.ExtrapolationSmoothing = true;
      snapshotSyncSetup1.IsControlled = false;
      snapshotSyncSetup1.ApplyPhysicsLocal = false;
      snapshotSyncSetup1.MaxPositionFactor = 100f;
      snapshotSyncSetup1.MaxLinearFactor = 100f;
      snapshotSyncSetup1.MaxRotationFactor = 100f;
      snapshotSyncSetup1.MaxAngularFactor = 1f;
      snapshotSyncSetup1.IterationsFactor = 1f;
      MyEntityPhysicsStateGroup.m_settings = snapshotSyncSetup1;
      MyPredictedSnapshotSyncSetup snapshotSyncSetup2 = new MyPredictedSnapshotSyncSetup();
      snapshotSyncSetup2.ProfileName = "ControlledGrid";
      snapshotSyncSetup2.ApplyPosition = true;
      snapshotSyncSetup2.ApplyRotation = true;
      snapshotSyncSetup2.ApplyPhysicsAngular = true;
      snapshotSyncSetup2.ApplyPhysicsLinear = true;
      snapshotSyncSetup2.ExtrapolationSmoothing = true;
      snapshotSyncSetup2.InheritRotation = false;
      snapshotSyncSetup2.ApplyPhysicsLocal = true;
      snapshotSyncSetup2.IsControlled = true;
      snapshotSyncSetup2.MaxPositionFactor = 100f;
      snapshotSyncSetup2.MaxLinearFactor = 100f;
      snapshotSyncSetup2.MaxRotationFactor = 100f;
      snapshotSyncSetup2.MaxAngularFactor = 10f;
      snapshotSyncSetup2.MinAngularFactor = 1000f;
      snapshotSyncSetup2.IterationsFactor = 1f;
      MyEntityPhysicsStateGroup.m_controlledSettings = snapshotSyncSetup2;
      MyPredictedSnapshotSyncSetup snapshotSyncSetup3 = new MyPredictedSnapshotSyncSetup();
      snapshotSyncSetup3.ProfileName = "ControlledGridNewParent";
      snapshotSyncSetup3.ApplyPosition = true;
      snapshotSyncSetup3.ApplyRotation = true;
      snapshotSyncSetup3.ApplyPhysicsAngular = true;
      snapshotSyncSetup3.ApplyPhysicsLinear = true;
      snapshotSyncSetup3.ExtrapolationSmoothing = true;
      snapshotSyncSetup3.IsControlled = true;
      snapshotSyncSetup3.ApplyPhysicsLocal = true;
      snapshotSyncSetup3.MaxPositionFactor = 100f;
      snapshotSyncSetup3.MaxLinearFactor = 100f;
      snapshotSyncSetup3.MaxRotationFactor = 100f;
      snapshotSyncSetup3.MaxAngularFactor = 10f;
      snapshotSyncSetup3.MinAngularFactor = 1000f;
      snapshotSyncSetup3.IterationsFactor = 5f;
      snapshotSyncSetup3.InheritRotation = false;
      MyEntityPhysicsStateGroup.m_controlledNewParentSettings = snapshotSyncSetup3;
      MyPredictedSnapshotSyncSetup snapshotSyncSetup4 = new MyPredictedSnapshotSyncSetup();
      snapshotSyncSetup4.ProfileName = "Wheel";
      snapshotSyncSetup4.ApplyPosition = false;
      snapshotSyncSetup4.ApplyRotation = false;
      snapshotSyncSetup4.ApplyPhysicsAngular = true;
      snapshotSyncSetup4.ApplyPhysicsLinear = false;
      snapshotSyncSetup4.ExtrapolationSmoothing = true;
      snapshotSyncSetup4.IsControlled = false;
      snapshotSyncSetup4.UpdateAlways = false;
      snapshotSyncSetup4.MaxPositionFactor = 1f;
      snapshotSyncSetup4.MaxLinearFactor = 1f;
      snapshotSyncSetup4.MaxRotationFactor = 1f;
      snapshotSyncSetup4.MaxAngularFactor = 1f;
      snapshotSyncSetup4.IterationsFactor = 1f;
      MyEntityPhysicsStateGroup.m_wheelSettings = snapshotSyncSetup4;
      MyPredictedSnapshotSyncSetup snapshotSyncSetup5 = new MyPredictedSnapshotSyncSetup();
      snapshotSyncSetup5.ProfileName = "Car";
      snapshotSyncSetup5.ApplyPosition = true;
      snapshotSyncSetup5.ApplyRotation = true;
      snapshotSyncSetup5.ApplyPhysicsAngular = true;
      snapshotSyncSetup5.ApplyPhysicsLinear = false;
      snapshotSyncSetup5.ExtrapolationSmoothing = true;
      snapshotSyncSetup5.InheritRotation = false;
      snapshotSyncSetup5.ApplyPhysicsLocal = true;
      snapshotSyncSetup5.IsControlled = true;
      snapshotSyncSetup5.MaxPositionFactor = 100f;
      snapshotSyncSetup5.MaxLinearFactor = 100f;
      snapshotSyncSetup5.MaxRotationFactor = 100f;
      snapshotSyncSetup5.MaxAngularFactor = 10f;
      snapshotSyncSetup5.MinAngularFactor = 1000f;
      snapshotSyncSetup5.IterationsFactor = 1f;
      MyEntityPhysicsStateGroup.m_carSettings = snapshotSyncSetup5;
      MyEntityPhysicsStateGroup.GridParentingSetup = new MyEntityPhysicsStateGroupBase.ParentingSetup()
      {
        MaxParentDistance = 100f,
        MinParentSpeed = 30f,
        MaxParentAcceleration = 6f,
        MinInsideParentSpeed = 20f,
        MaxParentDisconnectDistance = 100f,
        MinDisconnectParentSpeed = 25f,
        MaxDisconnectParentAcceleration = 30f,
        MinDisconnectInsideParentSpeed = 10f
      };
    }
  }
}
