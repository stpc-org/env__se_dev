// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.History.MyPredictedSnapshotSync
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.World;
using System;
using VRage;
using VRage.Game.Entity;
using VRage.Game.Networking;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Profiler;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Replication.History
{
  public class MyPredictedSnapshotSync : IMySnapshotSync
  {
    public static float DELTA_FACTOR = 0.8f;
    public static int SMOOTH_ITERATIONS = 30;
    public static bool POSITION_CORRECTION = true;
    public static bool SMOOTH_POSITION_CORRECTION = true;
    public static float MIN_POSITION_DELTA = 0.005f;
    public static float MAX_POSITION_DELTA = 0.5f;
    public static bool ROTATION_CORRECTION = true;
    public static bool SMOOTH_ROTATION_CORRECTION = true;
    public static float MIN_ROTATION_ANGLE = 0.03490659f;
    public static float MAX_ROTATION_ANGLE = 0.1745329f;
    public static bool LINEAR_VELOCITY_CORRECTION = true;
    public static bool SMOOTH_LINEAR_VELOCITY_CORRECTION = true;
    public static float MIN_LINEAR_VELOCITY_DELTA = 0.01f;
    public static float MAX_LINEAR_VELOCITY_DELTA = 4f;
    public static bool ANGULAR_VELOCITY_CORRECTION = true;
    public static bool SMOOTH_ANGULAR_VELOCITY_CORRECTION = true;
    public static float MIN_ANGULAR_VELOCITY_DELTA = 0.01f;
    public static float MAX_ANGULAR_VELOCITY_DELTA = 0.5f;
    public static float MIN_VELOCITY_CHANGE_TO_RESET = 10f;
    public static bool SKIP_CORRECTIONS_FOR_DEBUG_ENTITY;
    public static float SMOOTH_DISTANCE = 150f;
    public static bool ApplyTrend = true;
    public static bool ForceAnimated = false;
    public readonly MyMovingAverage AverageCorrection = new MyMovingAverage(60);
    private readonly MyEntity m_entity;
    private readonly MySnapshotHistory m_clientHistory = new MySnapshotHistory();
    private readonly MySnapshotHistory m_receivedQueue = new MySnapshotHistory();
    private readonly MySnapshotFlags m_currentFlags = new MySnapshotFlags();
    private bool m_inited;
    private MyPredictedSnapshotSync.ResetType m_wasReset = MyPredictedSnapshotSync.ResetType.Initial;
    private int m_animDeltaLinearVelocityIterations;
    private MyTimeSpan m_animDeltaLinearVelocityTimestamp;
    private Vector3 m_animDeltaLinearVelocity;
    private int m_animDeltaPositionIterations;
    private MyTimeSpan m_animDeltaPositionTimestamp;
    private Vector3D m_animDeltaPosition;
    private int m_animDeltaRotationIterations;
    private MyTimeSpan m_animDeltaRotationTimestamp;
    private Quaternion m_animDeltaRotation;
    private int m_animDeltaAngularVelocityIterations;
    private MyTimeSpan m_animDeltaAngularVelocityTimestamp;
    private Vector3 m_animDeltaAngularVelocity;
    private MyTimeSpan m_lastServerTimestamp;
    private MyTimeSpan m_trendStart;
    private Vector3 m_lastServerVelocity;
    private int m_stopSuspected;
    private MyTimeSpan m_debugLastClientTimestamp;
    private MySnapshot m_debugLastClientSnapshot;
    private MySnapshot m_debugLastServerSnapshot;
    private MySnapshot m_debugLastDelta;
    private static float TREND_TIMEOUT = 0.2f;

    public bool Inited => this.m_inited;

    public MyPredictedSnapshotSync(MyEntity entity) => this.m_entity = entity;

    public void Destroy() => this.Reset(false);

    public long Update(MyTimeSpan clientTimestamp, MySnapshotSyncSetup setup)
    {
      if (MyFakes.MULTIPLAYER_SKIP_PREDICTION_SUBGRIDS && MySnapshot.GetParent(this.m_entity, out bool _) != null || MyFakes.MULTIPLAYER_SKIP_PREDICTION)
      {
        this.Reset(false);
        this.m_receivedQueue.Reset();
        return -1;
      }
      if (!this.m_entity.InScene || this.m_entity.Parent != null || this.m_entity.Physics == null || (HkReferenceObject) this.m_entity.Physics.RigidBody != (HkReferenceObject) null && !this.m_entity.Physics.RigidBody.IsActive && !(setup as MyPredictedSnapshotSyncSetup).UpdateAlways || MySnapshotCache.DEBUG_ENTITY_ID == this.m_entity.EntityId && MyPredictedSnapshotSync.SKIP_CORRECTIONS_FOR_DEBUG_ENTITY)
        return -1;
      if (this.m_inited)
        this.UpdatePrediction(clientTimestamp, setup);
      else
        this.InitPrediction(clientTimestamp, setup);
      return -1;
    }

    private bool InitPrediction(MyTimeSpan clientTimestamp, MySnapshotSyncSetup setup)
    {
      MySnapshotHistory.MyItem myItem;
      this.m_receivedQueue.GetItem(clientTimestamp, out myItem);
      if (myItem.Valid)
      {
        MyEntity parent = MySnapshot.GetParent(this.m_entity, out bool _);
        if ((parent == null ? 0L : parent.EntityId) == myItem.Snapshot.ParentId)
        {
          MySnapshotCache.Add(this.m_entity, ref myItem.Snapshot, (MySnapshotFlags) setup, true);
          this.m_inited = true;
          return true;
        }
        this.m_inited = false;
      }
      return false;
    }

    private bool UpdatePrediction(MyTimeSpan clientTimestamp, MySnapshotSyncSetup setup)
    {
      MyPredictedSnapshotSyncSetup setup1 = setup as MyPredictedSnapshotSyncSetup;
      bool flag1 = (this.m_entity.WorldMatrix.Translation - MySector.MainCamera.Position).LengthSquared() < (double) MyPredictedSnapshotSync.SMOOTH_DISTANCE * (double) MyPredictedSnapshotSync.SMOOTH_DISTANCE;
      if (!flag1)
        setup1 = setup1.NotSmoothed;
      if (!setup1.Smoothing)
        this.m_animDeltaPositionIterations = this.m_animDeltaLinearVelocityIterations = this.m_animDeltaRotationIterations = this.m_animDeltaAngularVelocityIterations = 0;
      MySnapshot mySnapshot1 = new MySnapshot(this.m_entity, new MyClientInfo(), setup.ApplyPhysicsLocal, setup.InheritRotation);
      MySnapshot mySnapshot2 = mySnapshot1;
      if (!this.m_clientHistory.Empty())
      {
        MySnapshotHistory.MyItem myItem1;
        this.m_clientHistory.GetLast(out myItem1);
        if (mySnapshot1.ParentId != myItem1.Snapshot.ParentId || mySnapshot1.InheritRotation != myItem1.Snapshot.InheritRotation)
        {
          this.Reset(false);
          MySnapshotHistory.MyItem myItem2;
          this.m_receivedQueue.GetLast(out myItem2);
          if (myItem2.Snapshot.ParentId == mySnapshot1.ParentId && myItem2.Snapshot.InheritRotation == mySnapshot1.InheritRotation)
          {
            mySnapshot1.LinearVelocity = myItem2.Snapshot.LinearVelocity;
            MySnapshotCache.Add(this.m_entity, ref mySnapshot1, (MySnapshotFlags) setup1, true);
            this.m_wasReset = MyPredictedSnapshotSync.ResetType.NoReset;
          }
        }
      }
      this.m_clientHistory.Add(ref mySnapshot1, clientTimestamp);
      MySnapshotHistory.MyItem serverItem;
      MySnapshotHistory.MyItem myItem = this.UpdateFromServerQueue(clientTimestamp, setup1, ref mySnapshot1, out serverItem);
      float seconds1 = (float) (this.m_lastServerTimestamp - serverItem.Timestamp).Seconds;
      bool flag2 = false;
      Vector3 vector3 = Vector3.Zero;
      bool flag3 = false;
      if (myItem.Valid)
      {
        if (myItem.Snapshot.Position != Vector3D.Zero || (double) myItem.Snapshot.Rotation.W != 1.0)
          flag2 = true;
        mySnapshot1.Add(ref myItem.Snapshot);
        this.m_clientHistory.ApplyDelta(myItem.Timestamp, ref myItem.Snapshot);
        vector3 = (Vector3) myItem.Snapshot.Position;
        flag3 = true;
      }
      if ((this.m_animDeltaPositionIterations > 0 || this.m_animDeltaLinearVelocityIterations > 0 || this.m_animDeltaRotationIterations > 0 ? 1 : (this.m_animDeltaAngularVelocityIterations > 0 ? 1 : 0)) != 0)
      {
        if (this.m_animDeltaPositionIterations > 0)
        {
          this.m_clientHistory.ApplyDeltaPosition(this.m_animDeltaPositionTimestamp, this.m_animDeltaPosition);
          mySnapshot1.Position += this.m_animDeltaPosition;
          --this.m_animDeltaPositionIterations;
          this.m_currentFlags.ApplyPosition = true;
          vector3 = (Vector3) (vector3 + this.m_animDeltaPosition);
        }
        if (this.m_animDeltaLinearVelocityIterations > 0)
        {
          this.m_clientHistory.ApplyDeltaLinearVelocity(this.m_animDeltaLinearVelocityTimestamp, this.m_animDeltaLinearVelocity);
          mySnapshot1.LinearVelocity += this.m_animDeltaLinearVelocity;
          --this.m_animDeltaLinearVelocityIterations;
          this.m_currentFlags.ApplyPhysicsLinear = true;
        }
        if (this.m_animDeltaAngularVelocityIterations > 0)
        {
          this.m_clientHistory.ApplyDeltaAngularVelocity(this.m_animDeltaAngularVelocityTimestamp, this.m_animDeltaAngularVelocity);
          mySnapshot1.AngularVelocity += this.m_animDeltaAngularVelocity;
          --this.m_animDeltaAngularVelocityIterations;
          this.m_currentFlags.ApplyPhysicsAngular = true;
        }
        if (this.m_animDeltaRotationIterations > 0)
        {
          this.m_clientHistory.ApplyDeltaRotation(this.m_animDeltaRotationTimestamp, this.m_animDeltaRotation);
          mySnapshot1.Rotation *= Quaternion.Inverse(this.m_animDeltaRotation);
          mySnapshot1.Rotation.Normalize();
          --this.m_animDeltaRotationIterations;
          this.m_currentFlags.ApplyRotation = true;
        }
        flag3 = true;
      }
      if (flag3)
      {
        this.DebugDraw(ref serverItem, ref mySnapshot1, clientTimestamp, setup1);
        this.m_currentFlags.ApplyPhysicsLocal = setup.ApplyPhysicsLocal;
        this.m_currentFlags.InheritRotation = setup.InheritRotation;
        bool reset = myItem.Type == MySnapshotHistory.SnapshotType.Reset | flag2;
        MySnapshotCache.Add(this.m_entity, ref mySnapshot1, this.m_currentFlags, reset);
      }
      this.AverageCorrection.Enqueue(vector3.Length());
      if (MySnapshotCache.DEBUG_ENTITY_ID == this.m_entity.EntityId)
      {
        MyStatsGraph.ProfileAdvanced(true);
        MyStatsGraph.Begin("Prediction", member: nameof (UpdatePrediction), line: 333, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Game\\Replication\\History\\MyPredictedSnapshotSync.cs");
        MyStatsGraph.CustomTime("applySnapshot", flag3 ? 1f : 0.5f, "{0}", member: nameof (UpdatePrediction), line: 334, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Game\\Replication\\History\\MyPredictedSnapshotSync.cs");
        MyStatsGraph.CustomTime("smoothing", flag1 ? 1f : 0.5f, "{0}", member: nameof (UpdatePrediction), line: 335, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Game\\Replication\\History\\MyPredictedSnapshotSync.cs");
        if (setup1.ApplyPosition)
          MyStatsGraph.CustomTime("Pos", (float) this.m_debugLastDelta.Position.Length(), "{0}", member: nameof (UpdatePrediction), line: 339, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Game\\Replication\\History\\MyPredictedSnapshotSync.cs");
        if (setup1.ApplyRotation)
        {
          float angle;
          this.m_debugLastDelta.Rotation.GetAxisAngle(out Vector3 _, out angle);
          MyStatsGraph.CustomTime("Rot", Math.Abs(angle), "{0}", member: nameof (UpdatePrediction), line: 347, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Game\\Replication\\History\\MyPredictedSnapshotSync.cs");
        }
        if (setup1.ApplyPhysicsLinear)
          MyStatsGraph.CustomTime("linVel", this.m_debugLastDelta.LinearVelocity.Length(), "{0}", member: nameof (UpdatePrediction), line: 351, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Game\\Replication\\History\\MyPredictedSnapshotSync.cs");
        if (setup1.ApplyPhysicsAngular)
          MyStatsGraph.CustomTime("angVel", Math.Abs(this.m_debugLastDelta.AngularVelocity.Length()), "{0}", member: nameof (UpdatePrediction), line: 353, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Game\\Replication\\History\\MyPredictedSnapshotSync.cs");
        MyStatsGraph.End(member: nameof (UpdatePrediction), line: 355, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Game\\Replication\\History\\MyPredictedSnapshotSync.cs");
        MyStatsGraph.ProfileAdvanced(false);
        if (flag3)
        {
          if (setup1.ApplyPosition)
          {
            Vector3D vector3D1 = (serverItem.Snapshot.Position - this.m_debugLastServerSnapshot.Position) / (double) seconds1;
            this.m_debugLastServerSnapshot = serverItem.Snapshot;
            float seconds2 = (float) (this.m_debugLastClientTimestamp - clientTimestamp).Seconds;
            Vector3D vector3D2 = (mySnapshot1.Position - this.m_debugLastClientSnapshot.Position) / (double) seconds2;
            this.m_debugLastClientSnapshot = mySnapshot1;
            this.m_debugLastClientTimestamp = clientTimestamp;
            MySnapshot ss;
            mySnapshot2.Diff(ref mySnapshot1, out ss);
            ss.Position.Length();
            double num = (double) ss.LinearVelocity.Length();
            myItem.Snapshot.Position.Length();
            this.m_animDeltaPosition.Length();
            if (this.m_entity is MyCubeGrid entity && MyGridPhysicalHierarchy.Static.GetEntityConnectingToParent(entity) is MyPistonBase connectingToParent)
            {
              double currentPosition = (double) connectingToParent.CurrentPosition;
            }
          }
          else if (!setup1.ApplyRotation)
          {
            int num1 = setup1.ApplyPhysicsAngular ? 1 : 0;
          }
        }
      }
      this.m_clientHistory.Prune(this.m_lastServerTimestamp, MyTimeSpan.Zero, 3);
      return flag3;
    }

    public void Read(ref MySnapshot snapshot, MyTimeSpan timeStamp)
    {
      if (this.m_entity.Parent != null || this.m_entity.Physics == null)
        return;
      if (this.m_entity.Physics.IsInWorld && (HkReferenceObject) this.m_entity.Physics.RigidBody != (HkReferenceObject) null && (!this.m_entity.Physics.RigidBody.IsActive && snapshot.Active))
        this.m_entity.Physics.RigidBody.Activate();
      if (!this.m_receivedQueue.Empty())
      {
        MySnapshotHistory.MyItem myItem;
        this.m_receivedQueue.GetLast(out myItem);
        if (snapshot.ParentId != myItem.Snapshot.ParentId || snapshot.InheritRotation != myItem.Snapshot.InheritRotation)
          this.m_receivedQueue.Reset();
      }
      this.m_receivedQueue.Add(ref snapshot, timeStamp);
      this.m_receivedQueue.Prune(timeStamp, MyTimeSpan.Zero, 10);
    }

    public void Reset(bool reinit = false)
    {
      this.m_clientHistory.Reset();
      this.m_animDeltaRotationIterations = this.m_animDeltaLinearVelocityIterations = this.m_animDeltaPositionIterations = this.m_animDeltaAngularVelocityIterations = 0;
      this.m_lastServerVelocity = (Vector3) Vector3D.PositiveInfinity;
      this.m_wasReset = MyPredictedSnapshotSync.ResetType.Reset;
      this.m_trendStart = MyTimeSpan.FromSeconds(-1.0);
      if (!reinit)
        return;
      this.m_inited = false;
    }

    private MySnapshotHistory.MyItem UpdateFromServerQueue(
      MyTimeSpan clientTimestamp,
      MyPredictedSnapshotSyncSetup setup,
      ref MySnapshot currentSnapshot,
      out MySnapshotHistory.MyItem serverItem)
    {
      this.m_currentFlags.Init(false);
      bool flag1 = false;
      this.m_receivedQueue.GetItem(clientTimestamp, out serverItem);
      if (serverItem.Valid)
      {
        if (serverItem.Timestamp != this.m_lastServerTimestamp)
        {
          MySnapshotHistory.MyItem myItem;
          this.m_clientHistory.Get(serverItem.Timestamp, out myItem);
          if (myItem.Valid && (myItem.Type == MySnapshotHistory.SnapshotType.Exact || myItem.Type == MySnapshotHistory.SnapshotType.Interpolation) && (serverItem.Snapshot.ParentId == myItem.Snapshot.ParentId && serverItem.Snapshot.InheritRotation == myItem.Snapshot.InheritRotation))
          {
            this.m_lastServerTimestamp = serverItem.Timestamp;
            if (this.UpdateTrend(setup, ref serverItem, ref myItem))
              return serverItem;
            MySnapshot ss;
            if (!serverItem.Snapshot.Active && !setup.IsControlled)
            {
              serverItem.Snapshot.Diff(ref currentSnapshot, out ss);
              if (ss.CheckThresholds(0.0001f, 0.0001f, 0.0001f, 0.0001f))
              {
                this.Reset(true);
              }
              else
              {
                serverItem.Valid = false;
                return serverItem;
              }
            }
            else
              serverItem.Snapshot.Diff(ref myItem.Snapshot, out ss);
            this.m_debugLastDelta = ss;
            this.UpdateForceStop(ref ss, ref currentSnapshot, ref serverItem, setup);
            if (this.m_wasReset != MyPredictedSnapshotSync.ResetType.NoReset)
            {
              this.m_wasReset = MyPredictedSnapshotSync.ResetType.NoReset;
              serverItem.Snapshot = ss;
              serverItem.Type = MySnapshotHistory.SnapshotType.Reset;
              return serverItem;
            }
            int num1 = (int) ((double) MyPredictedSnapshotSync.SMOOTH_ITERATIONS * (double) setup.IterationsFactor);
            bool flag2 = false;
            if (setup.ApplyPosition && MyPredictedSnapshotSync.POSITION_CORRECTION)
            {
              float num2 = setup.MaxPositionFactor * setup.MaxPositionFactor;
              double num3 = ss.Position.LengthSquared();
              if (num3 > (double) MyPredictedSnapshotSync.MAX_POSITION_DELTA * (double) MyPredictedSnapshotSync.MAX_POSITION_DELTA * (double) num2)
              {
                Vector3D position = ss.Position;
                double num4 = position.Normalize() - (double) MyPredictedSnapshotSync.MAX_POSITION_DELTA * (1.0 - (double) MyPredictedSnapshotSync.DELTA_FACTOR);
                ss.Position = position * num4;
                flag2 = true;
                this.m_animDeltaPositionIterations = 0;
                this.m_currentFlags.ApplyPosition = true;
              }
              else if (!MyPredictedSnapshotSync.SMOOTH_POSITION_CORRECTION || !setup.Smoothing)
              {
                this.m_animDeltaPositionIterations = 0;
              }
              else
              {
                float num4 = MyPredictedSnapshotSync.MIN_POSITION_DELTA * setup.MinPositionFactor;
                if (num3 > (double) num4 * (double) num4)
                  this.m_animDeltaPositionIterations = num1;
                if (this.m_animDeltaPositionIterations > 0)
                {
                  this.m_animDeltaPosition = ss.Position / (double) this.m_animDeltaPositionIterations;
                  this.m_animDeltaPositionTimestamp = serverItem.Timestamp;
                }
                ss.Position = Vector3D.Zero;
              }
            }
            else
            {
              ss.Position = Vector3D.Zero;
              this.m_animDeltaPositionIterations = 0;
            }
            if (setup.ApplyRotation && MyPredictedSnapshotSync.ROTATION_CORRECTION)
            {
              Vector3 axis;
              float angle;
              ss.Rotation.GetAxisAngle(out axis, out angle);
              if ((double) angle > 3.14159297943115)
              {
                axis = -axis;
                angle = 6.283186f - angle;
              }
              if ((double) angle > (double) MyPredictedSnapshotSync.MAX_ROTATION_ANGLE * (double) setup.MaxRotationFactor)
              {
                ss.Rotation = Quaternion.CreateFromAxisAngle(axis, angle - MyPredictedSnapshotSync.MAX_ROTATION_ANGLE * (1f - MyPredictedSnapshotSync.DELTA_FACTOR));
                ss.Rotation.Normalize();
                flag2 = true;
                this.m_animDeltaRotationIterations = 0;
                this.m_currentFlags.ApplyRotation = true;
              }
              else if (!MyPredictedSnapshotSync.SMOOTH_ROTATION_CORRECTION || !setup.Smoothing)
              {
                this.m_animDeltaRotationIterations = 0;
              }
              else
              {
                if ((double) angle > (double) MyPredictedSnapshotSync.MIN_ROTATION_ANGLE)
                  this.m_animDeltaRotationIterations = num1;
                if (this.m_animDeltaRotationIterations > 0)
                {
                  this.m_animDeltaRotation = Quaternion.CreateFromAxisAngle(axis, angle / (float) this.m_animDeltaRotationIterations);
                  this.m_animDeltaRotationTimestamp = serverItem.Timestamp;
                }
                ss.Rotation = Quaternion.Identity;
              }
            }
            else
            {
              ss.Rotation = Quaternion.Identity;
              this.m_animDeltaRotationIterations = 0;
            }
            if (setup.ApplyPhysicsLinear && MyPredictedSnapshotSync.LINEAR_VELOCITY_CORRECTION)
            {
              float num2 = MyPredictedSnapshotSync.MIN_LINEAR_VELOCITY_DELTA * MyPredictedSnapshotSync.MIN_LINEAR_VELOCITY_DELTA;
              float num3 = setup.MinLinearFactor * setup.MinLinearFactor * num2;
              float num4 = ss.LinearVelocity.LengthSquared();
              if ((double) serverItem.Snapshot.LinearVelocity.LengthSquared() == 0.0 && (double) num4 < (double) num2)
              {
                flag2 = true;
                this.m_animDeltaLinearVelocityIterations = 0;
                this.m_currentFlags.ApplyPhysicsLinear = true;
              }
              else if (!MyPredictedSnapshotSync.SMOOTH_LINEAR_VELOCITY_CORRECTION || !setup.Smoothing)
              {
                if ((double) num4 > (double) MyPredictedSnapshotSync.MAX_LINEAR_VELOCITY_DELTA * (double) MyPredictedSnapshotSync.MAX_LINEAR_VELOCITY_DELTA * (double) setup.MaxLinearFactor * (double) setup.MaxLinearFactor)
                {
                  ss.LinearVelocity *= MyPredictedSnapshotSync.DELTA_FACTOR;
                  flag2 = true;
                  this.m_animDeltaLinearVelocityIterations = 0;
                  this.m_currentFlags.ApplyPhysicsLinear = true;
                }
                else
                  this.m_animDeltaLinearVelocityIterations = 0;
              }
              else
              {
                if ((double) num4 > (double) num3)
                  this.m_animDeltaLinearVelocityIterations = num1;
                if (this.m_animDeltaLinearVelocityIterations > 0)
                {
                  this.m_animDeltaLinearVelocity = ss.LinearVelocity * MyPredictedSnapshotSync.DELTA_FACTOR / (float) this.m_animDeltaLinearVelocityIterations;
                  this.m_animDeltaLinearVelocityTimestamp = serverItem.Timestamp;
                }
                ss.LinearVelocity = Vector3.Zero;
              }
            }
            else
            {
              ss.LinearVelocity = Vector3.Zero;
              this.m_animDeltaLinearVelocityIterations = 0;
            }
            if (setup.ApplyPhysicsAngular && MyPredictedSnapshotSync.ANGULAR_VELOCITY_CORRECTION)
            {
              float num2 = ss.AngularVelocity.LengthSquared();
              if ((double) num2 > (double) MyPredictedSnapshotSync.MAX_ANGULAR_VELOCITY_DELTA * (double) MyPredictedSnapshotSync.MAX_ANGULAR_VELOCITY_DELTA * (double) setup.MaxAngularFactor * (double) setup.MaxAngularFactor)
              {
                ss.AngularVelocity *= MyPredictedSnapshotSync.DELTA_FACTOR;
                flag2 = true;
                this.m_currentFlags.ApplyPhysicsAngular = true;
                this.m_animDeltaAngularVelocityIterations = 0;
              }
              else if (!MyPredictedSnapshotSync.SMOOTH_ANGULAR_VELOCITY_CORRECTION || !setup.Smoothing)
              {
                this.m_animDeltaAngularVelocityIterations = 0;
              }
              else
              {
                if ((double) num2 > (double) MyPredictedSnapshotSync.MIN_ANGULAR_VELOCITY_DELTA * (double) MyPredictedSnapshotSync.MIN_ANGULAR_VELOCITY_DELTA * (double) setup.MinAngularFactor * (double) setup.MinAngularFactor)
                  this.m_animDeltaAngularVelocityIterations = num1;
                if (this.m_animDeltaAngularVelocityIterations > 0)
                {
                  this.m_animDeltaAngularVelocity = ss.AngularVelocity * MyPredictedSnapshotSync.DELTA_FACTOR / (float) this.m_animDeltaAngularVelocityIterations;
                  this.m_animDeltaAngularVelocityTimestamp = serverItem.Timestamp;
                }
                ss.AngularVelocity = Vector3.Zero;
              }
            }
            else
            {
              ss.AngularVelocity = Vector3.Zero;
              this.m_animDeltaAngularVelocityIterations = 0;
            }
            if (MyCompilationSymbols.EnableNetworkPositionTracking & flag2)
            {
              long entityId = this.m_entity.EntityId;
              long debugEntityId = MySnapshotCache.DEBUG_ENTITY_ID;
            }
            serverItem.Snapshot = ss;
            serverItem.Valid = flag2;
          }
          else if (myItem.Type == MySnapshotHistory.SnapshotType.TooNew && !myItem.Snapshot.Active)
          {
            this.Reset(true);
          }
          else
          {
            if (MyPredictedSnapshotSync.POSITION_CORRECTION && myItem.Valid && (serverItem.Snapshot.ParentId != myItem.Snapshot.ParentId || serverItem.Snapshot.InheritRotation != myItem.Snapshot.InheritRotation))
            {
              if (this.m_trendStart.Seconds < 0.0)
              {
                this.m_trendStart = clientTimestamp;
              }
              else
              {
                MyTimeSpan myTimeSpan = clientTimestamp - this.m_trendStart;
                double seconds1 = myTimeSpan.Seconds;
                double trendTimeout = (double) MyPredictedSnapshotSync.TREND_TIMEOUT;
                myTimeSpan = MySession.Static.MultiplayerPing;
                double seconds2 = myTimeSpan.Seconds;
                double num = trendTimeout + seconds2;
                if (seconds1 > num && MyPredictedSnapshotSync.ApplyTrend)
                {
                  this.Reset(true);
                  serverItem.Valid = false;
                  return serverItem;
                }
              }
            }
            else
              this.m_trendStart = MyTimeSpan.FromSeconds(-1.0);
            serverItem.Valid = false;
            flag1 = (uint) this.m_wasReset > 0U;
            if (this.m_wasReset != MyPredictedSnapshotSync.ResetType.NoReset || !MyCompilationSymbols.EnableNetworkPositionTracking)
              ;
          }
        }
        else
        {
          serverItem.Valid = false;
          flag1 = (uint) this.m_wasReset > 0U;
        }
      }
      else if (!this.m_receivedQueue.Empty())
        flag1 = true;
      if (flag1)
      {
        if (serverItem.Valid && (serverItem.Type == MySnapshotHistory.SnapshotType.Exact || serverItem.Type == MySnapshotHistory.SnapshotType.Interpolation || serverItem.Type == MySnapshotHistory.SnapshotType.Extrapolation))
        {
          long debugEntityId = MySnapshotCache.DEBUG_ENTITY_ID;
          long entityId = this.m_entity.EntityId;
          MySnapshot ss;
          serverItem.Snapshot.Diff(ref currentSnapshot, out ss);
          this.m_currentFlags.Init((MySnapshotFlags) setup);
          this.m_currentFlags.ApplyPhysicsLinear &= MyPredictedSnapshotSync.LINEAR_VELOCITY_CORRECTION;
          this.m_currentFlags.ApplyPhysicsAngular &= MyPredictedSnapshotSync.ANGULAR_VELOCITY_CORRECTION;
          serverItem.Valid = ss.Active;
          serverItem.Snapshot = ss;
          serverItem.Type = MySnapshotHistory.SnapshotType.Reset;
          this.m_debugLastDelta = ss;
          return serverItem;
        }
        serverItem.Valid = false;
        long debugEntityId1 = MySnapshotCache.DEBUG_ENTITY_ID;
        long entityId1 = this.m_entity.EntityId;
      }
      return serverItem;
    }

    private void DebugDraw(
      ref MySnapshotHistory.MyItem serverItem,
      ref MySnapshot currentSnapshot,
      MyTimeSpan clientTimestamp,
      MyPredictedSnapshotSyncSetup setup)
    {
      if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW || !MyDebugDrawSettings.DEBUG_DRAW_NETWORK_SYNC)
        return;
      MatrixD mat1;
      serverItem.Snapshot.GetMatrix(this.m_entity, out mat1);
      MyRenderProxy.DebugDrawAxis(mat1, 1f, false);
      MatrixD worldMatrix = this.m_entity.WorldMatrix;
      MyRenderProxy.DebugDrawAxis(worldMatrix, 0.2f, false);
      MyRenderProxy.DebugDrawArrow3DDir(worldMatrix.Translation, (Vector3D) serverItem.Snapshot.GetLinearVelocity(setup.ApplyPhysicsLocal), Color.White);
      double milliseconds = (serverItem.Timestamp - clientTimestamp).Milliseconds;
      float scale = (float) Math.Abs(milliseconds / 32.0);
      MyRenderProxy.DebugDrawAABB(new BoundingBoxD(mat1.Translation - Vector3.One, mat1.Translation + Vector3.One), milliseconds < 0.0 ? Color.Red : Color.Green, scale: scale, depthRead: false);
      MyEntity parent = MySnapshot.GetParent(this.m_entity, out bool _);
      if (parent != null)
        MyRenderProxy.DebugDrawArrow3D(worldMatrix.Translation, parent.WorldMatrix.Translation, Color.Blue);
      MatrixD mat2;
      currentSnapshot.GetMatrix(this.m_entity, out mat2, applyRotation: false);
      MyRenderProxy.DebugDrawArrow3D(this.m_entity.WorldMatrix.Translation, mat2.Translation, Color.Goldenrod);
    }

    private void UpdateForceStop(
      ref MySnapshot delta,
      ref MySnapshot currentSnapshot,
      ref MySnapshotHistory.MyItem serverItem,
      MyPredictedSnapshotSyncSetup setup)
    {
      if (this.m_lastServerVelocity.IsValid() && setup.ApplyPhysicsLinear && setup.AllowForceStop)
      {
        Vector3 vector3 = serverItem.Snapshot.LinearVelocity - this.m_lastServerVelocity;
        this.m_lastServerVelocity = serverItem.Snapshot.LinearVelocity;
        double num1 = (double) vector3.LengthSquared();
        if (this.m_stopSuspected > 0)
        {
          float num2 = (float) ((double) MyPredictedSnapshotSync.MIN_VELOCITY_CHANGE_TO_RESET / 2.0 * ((double) MyPredictedSnapshotSync.MIN_VELOCITY_CHANGE_TO_RESET / 2.0));
          if ((double) (serverItem.Snapshot.LinearVelocity - currentSnapshot.LinearVelocity).LengthSquared() > (double) num2)
          {
            this.Reset(false);
            this.m_wasReset = MyPredictedSnapshotSync.ResetType.ForceStop;
            serverItem.Snapshot.Diff(ref currentSnapshot, out delta);
            this.m_stopSuspected = 0;
          }
        }
        double num3 = (double) MyPredictedSnapshotSync.MIN_VELOCITY_CHANGE_TO_RESET * (double) MyPredictedSnapshotSync.MIN_VELOCITY_CHANGE_TO_RESET;
        if (num1 > num3)
        {
          this.m_stopSuspected = 10;
          int num2 = MyCompilationSymbols.EnableNetworkPositionTracking ? 1 : 0;
        }
        else
        {
          if (this.m_stopSuspected <= 0)
            return;
          --this.m_stopSuspected;
        }
      }
      else
        this.m_lastServerVelocity = serverItem.Snapshot.LinearVelocity;
    }

    private bool UpdateTrend(
      MyPredictedSnapshotSyncSetup setup,
      ref MySnapshotHistory.MyItem serverItem,
      ref MySnapshotHistory.MyItem item)
    {
      if (setup.UserTrend && this.m_receivedQueue.Count > 1 && MyPredictedSnapshotSync.POSITION_CORRECTION)
      {
        MySnapshotHistory.MyItem myItem1;
        this.m_receivedQueue.GetFirst(out myItem1);
        MySnapshotHistory.MyItem myItem2;
        this.m_receivedQueue.GetLast(out myItem2);
        Vector3 vector3_1 = Vector3.Sign((Vector3) ((myItem2.Snapshot.Position - myItem1.Snapshot.Position) / (myItem2.Timestamp.Seconds - myItem1.Timestamp.Seconds)), 1f);
        MySnapshotHistory.MyItem myItem3;
        this.m_clientHistory.GetLast(out myItem3);
        Vector3 vector3_2 = Vector3.Sign((Vector3) ((myItem3.Snapshot.Position - item.Snapshot.Position) / (myItem3.Timestamp.Seconds - item.Timestamp.Seconds)), 1f);
        if (vector3_1 == Vector3.Zero && vector3_1 != vector3_2)
        {
          if (this.m_trendStart.Seconds < 0.0)
            this.m_trendStart = item.Timestamp;
          else if ((item.Timestamp - this.m_trendStart).Seconds > (myItem2.Timestamp - this.m_trendStart).Seconds && MyPredictedSnapshotSync.ApplyTrend)
          {
            this.Reset(true);
            serverItem.Valid = false;
            return true;
          }
        }
        else
          this.m_trendStart = MyTimeSpan.FromSeconds(-1.0);
      }
      else
        this.m_trendStart = MyTimeSpan.FromSeconds(-1.0);
      return false;
    }

    public long GetParentId() => this.m_receivedQueue.Empty() ? -1L : this.m_receivedQueue.GetLastParentId();

    private enum ResetType
    {
      NoReset,
      Initial,
      Reset,
      ForceStop,
    }
  }
}
