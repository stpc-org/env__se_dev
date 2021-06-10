// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.History.MyAnimatedSnapshotSync
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Blocks;
using System;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Game.Networking;
using VRage.Library.Utils;
using VRage.Profiler;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Replication.History
{
  public class MyAnimatedSnapshotSync : IMySnapshotSync
  {
    private readonly MySnapshotHistory m_history = new MySnapshotHistory();
    private MyTimeSpan m_safeMovementCounter;
    private Vector3 m_lastVelocity;
    private readonly MyEntity m_entity;
    private Vector3D m_lastPos;
    private bool m_deactivated;
    private bool m_wasExtrapolating;
    private MyTimeSpan m_lastTimeDelta;
    private MyTimeSpan m_lastTime;
    public static MyTimeSpan TimeShift = MyTimeSpan.FromMilliseconds(64.0);
    private int m_invalidParentCounter;
    private readonly List<MyAnimatedSnapshotSync.MyBlend> m_blends = new List<MyAnimatedSnapshotSync.MyBlend>();
    private readonly List<MyAnimatedSnapshotSync.MyBlend> m_blendsToRemove = new List<MyAnimatedSnapshotSync.MyBlend>();

    public MyAnimatedSnapshotSync(MyEntity entity) => this.m_entity = entity;

    public long Update(MyTimeSpan clientTimestamp, MySnapshotSyncSetup setup)
    {
      if (this.m_deactivated && !this.m_history.IsLastActive() || MyFakes.MULTIPLAYER_SKIP_ANIMATION)
        return -1;
      this.m_deactivated = false;
      MyTimeSpan clientTimestamp1 = clientTimestamp - MyAnimatedSnapshotSync.TimeShift;
      MySnapshotHistory.MyItem myItem;
      this.m_history.Get(clientTimestamp1, out myItem);
      this.m_history.PruneTooOld(clientTimestamp1);
      if (myItem.Valid && !myItem.Snapshot.Active || this.m_history.Empty())
        this.m_deactivated = true;
      MyEntity parent = MySnapshot.GetParent(this.m_entity, out bool _);
      bool flag1 = setup.IgnoreParentId || !myItem.Valid || (parent == null && myItem.Snapshot.ParentId == 0L || parent != null && myItem.Snapshot.ParentId == parent.EntityId);
      if (!flag1)
        ++this.m_invalidParentCounter;
      else
        this.m_invalidParentCounter = 0;
      bool flag2 = ((!myItem.Valid || myItem.Type == MySnapshotHistory.SnapshotType.TooNew ? 0 : (myItem.Type != MySnapshotHistory.SnapshotType.TooOld ? 1 : 0)) & (flag1 ? 1 : 0)) != 0;
      if (flag2)
      {
        if (MyFakes.MULTIPLAYER_EXTRAPOLATION_SMOOTHING && setup.ExtrapolationSmoothing)
        {
          this.m_wasExtrapolating = myItem.Type == MySnapshotHistory.SnapshotType.Extrapolation;
          this.m_lastTimeDelta = clientTimestamp1 - this.m_history.GetLastTimestamp();
          this.m_lastTime = myItem.Timestamp;
        }
        if (myItem.Snapshot.Active && this.m_blends.Count > 0)
          this.BlendExtrapolation(ref myItem);
        if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_NETWORK_SYNC)
        {
          MyRenderProxy.DebugDrawAABB(this.m_entity.PositionComp.WorldAABB, Color.White);
          if (!(this.m_entity is MyWheel))
          {
            MatrixD mat;
            myItem.Snapshot.GetMatrix(this.m_entity, out mat);
            double milliseconds = (this.m_history.GetLastTimestamp() - (clientTimestamp - MyAnimatedSnapshotSync.TimeShift)).Milliseconds;
            MyRenderProxy.DebugDrawSphere(mat.Translation, (float) Math.Abs(milliseconds / 32.0), milliseconds < 0.0 ? Color.Red : Color.Green, depthRead: false);
            MyRenderProxy.DebugDrawAxis(mat, 1f, false);
            if (parent != null)
              MyRenderProxy.DebugDrawArrow3D(mat.Translation, parent.WorldMatrix.Translation, Color.Blue);
          }
        }
        MySnapshotCache.Add(this.m_entity, ref myItem.Snapshot, (MySnapshotFlags) setup, myItem.Type == MySnapshotHistory.SnapshotType.Reset);
      }
      if (MySnapshotCache.DEBUG_ENTITY_ID == this.m_entity.EntityId)
      {
        MyStatsGraph.ProfileAdvanced(true);
        MyStatsGraph.Begin("Animation", member: nameof (Update), line: 92, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Game\\Replication\\History\\MyAnimatedSnapshotSync.cs");
        MyStatsGraph.CustomTime("applySnapshot", flag2 ? 1f : 0.5f, "{0}", member: nameof (Update), line: 93, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Game\\Replication\\History\\MyAnimatedSnapshotSync.cs");
        MyStatsGraph.CustomTime("extrapolating", myItem.Type == MySnapshotHistory.SnapshotType.Extrapolation ? 1f : 0.5f, "{0}", member: nameof (Update), line: 94, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Game\\Replication\\History\\MyAnimatedSnapshotSync.cs");
        MyStatsGraph.CustomTime("blendsCount", (float) this.m_blends.Count, "{0}", member: nameof (Update), line: 95, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Game\\Replication\\History\\MyAnimatedSnapshotSync.cs");
        MyStatsGraph.End(new float?(1f), 1f, "{0}", member: nameof (Update), line: 96, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Game\\Replication\\History\\MyAnimatedSnapshotSync.cs");
        MyStatsGraph.ProfileAdvanced(false);
      }
      return !myItem.Valid ? -1L : myItem.Snapshot.ParentId;
    }

    private void BlendExtrapolation(ref MySnapshotHistory.MyItem item)
    {
      this.m_blendsToRemove.Clear();
      MySnapshot ss = new MySnapshot();
      bool flag = true;
      float factor = 1f;
      foreach (MyAnimatedSnapshotSync.MyBlend blend in this.m_blends)
      {
        MyTimeSpan myTimeSpan = item.Timestamp - blend.TimeStart;
        if (myTimeSpan >= MyTimeSpan.Zero && myTimeSpan < blend.Duration)
        {
          MySnapshotHistory.MyItem myItem1 = blend.Item1;
          MySnapshotHistory.MyItem myItem2 = blend.Item2;
          MySnapshotHistory.MyItem myItem3;
          MySnapshotHistory.Lerp(item.Timestamp, ref myItem1, ref myItem2, out myItem3);
          if (flag)
            ss = myItem3.Snapshot;
          else
            myItem3.Snapshot.Lerp(ref ss, factor, out ss);
          if (ss.ParentId == -1L)
          {
            this.m_blendsToRemove.Add(blend);
            flag = true;
            break;
          }
          factor = (float) (1.0 - myTimeSpan.Seconds / blend.Duration.Seconds);
          flag = false;
        }
        else
          this.m_blendsToRemove.Add(blend);
      }
      if (!flag)
      {
        item.Snapshot.Lerp(ref ss, factor, out ss);
        if (ss.ParentId != -1L)
          item.Snapshot = ss;
      }
      foreach (MyAnimatedSnapshotSync.MyBlend myBlend in this.m_blendsToRemove)
        this.m_blends.Remove(myBlend);
    }

    public void Read(ref MySnapshot item, MyTimeSpan timeStamp)
    {
      if (this.m_wasExtrapolating)
      {
        MyTimeSpan myTimeSpan = this.m_lastTimeDelta;
        if (this.m_blends.Count > 0)
        {
          myTimeSpan = this.m_blends[this.m_blends.Count - 1].Duration - (this.m_lastTime - this.m_blends[this.m_blends.Count - 1].TimeStart);
          if (myTimeSpan < this.m_lastTimeDelta)
            myTimeSpan = this.m_lastTimeDelta;
        }
        MyAnimatedSnapshotSync.MyBlend myBlend = new MyAnimatedSnapshotSync.MyBlend()
        {
          TimeStart = this.m_lastTime,
          Duration = myTimeSpan
        };
        if (this.m_history.GetItems(this.m_history.Count - 2, out myBlend.Item1, out myBlend.Item2))
          this.m_blends.Add(myBlend);
        this.m_wasExtrapolating = false;
      }
      this.m_history.Add(ref item, timeStamp);
      this.m_history.PruneTooOld(timeStamp - MyAnimatedSnapshotSync.TimeShift);
    }

    public void Reset(bool reinit = false)
    {
      if (reinit)
      {
        this.m_history.Reset();
        this.m_blends.Clear();
      }
      this.m_deactivated = false;
    }

    public void Destroy() => this.Reset(false);

    private struct MyBlend
    {
      public MySnapshotHistory.MyItem Item1;
      public MySnapshotHistory.MyItem Item2;
      public MyTimeSpan Duration;
      public MyTimeSpan TimeStart;
    }
  }
}
