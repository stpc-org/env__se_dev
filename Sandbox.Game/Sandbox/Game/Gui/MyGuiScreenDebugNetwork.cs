// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugNetwork
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems.Chat;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication;
using Sandbox.Game.Replication.History;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using VRage;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.Models;
using VRage.Library.Debugging;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Voxels;

namespace Sandbox.Game.Gui
{
  [MyDebugScreen("VRage", "Network")]
  [StaticEventOwner]
  internal class MyGuiScreenDebugNetwork : MyGuiScreenDebugBase
  {
    private MyGuiControlLabel m_entityLabel;
    private MyEntity m_currentEntity;
    private MyGuiControlSlider m_up;
    private MyGuiControlSlider m_right;
    private MyGuiControlSlider m_forward;
    private MyGuiControlButton m_kickButton;
    private MyGuiControlLabel m_profileLabel;
    private bool m_profileEntityLocked;
    private const float FORCED_PRIORITY = 1f;
    private readonly MyPredictedSnapshotSyncSetup m_kickSetup;
    private MyGuiControlLabel m_dataRateLabel;
    private Thread m_debugTransferThread;
    private bool m_debugDataTransfer;
    private bool m_sendInParallel;
    private volatile int m_debugTransferRate;
    private static byte[] m_message;
    private readonly MyTimedStatWindow<int> m_bytesPerSecond;
    public static bool DebugDrawSpatialReplicationLayers;
    private List<MyGuiScreenDebugNetwork.Layer> m_layers;

    public MyGuiScreenDebugNetwork()
    {
      MyPredictedSnapshotSyncSetup snapshotSyncSetup = new MyPredictedSnapshotSyncSetup();
      snapshotSyncSetup.AllowForceStop = false;
      snapshotSyncSetup.ApplyPhysicsAngular = false;
      snapshotSyncSetup.ApplyPhysicsLinear = false;
      snapshotSyncSetup.ApplyRotation = false;
      snapshotSyncSetup.ApplyPosition = true;
      snapshotSyncSetup.ExtrapolationSmoothing = true;
      this.m_kickSetup = snapshotSyncSetup;
      this.m_sendInParallel = true;
      this.m_debugTransferRate = 10000;
      this.m_bytesPerSecond = new MyTimedStatWindow<int>(TimeSpan.FromSeconds(5.0), (MyTimedStatWindow.IStatArithmetic<int>) new MyGuiScreenDebugNetwork.IntArithmetic());
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.RecreateControls(true);
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.m_sliderDebugScale = 1f;
      this.AddCaption("Network", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.m_currentPosition.Y += 0.01f;
      if (MyMultiplayer.Static != null)
      {
        this.AddSlider("Priority multiplier", 1f, 0.0f, 16f, (Action<MyGuiControlSlider>) (slider => MyMultiplayer.RaiseStaticEvent<float>((Func<IMyEventOwner, Action<float>>) (x => new Action<float>(MyMultiplayerBase.OnSetPriorityMultiplier)), slider.Value)));
        this.m_currentPosition.Y += 0.01f;
        this.AddCheckBox("Smooth ping", MyMultiplayer.Static.ReplicationLayer.UseSmoothPing, (Action<MyGuiControlCheckbox>) (x => MyMultiplayer.Static.ReplicationLayer.UseSmoothPing = x.IsChecked));
        this.AddSlider("Ping smooth factor", MyMultiplayer.Static.ReplicationLayer.PingSmoothFactor, 0.0f, 3f, (Action<MyGuiControlSlider>) (slider => MyMultiplayer.Static.ReplicationLayer.PingSmoothFactor = slider.Value));
        this.AddSlider("Timestamp correction minimum", (float) MyMultiplayer.Static.ReplicationLayer.TimestampCorrectionMinimum, 0.0f, 100f, (Action<MyGuiControlSlider>) (slider => MyMultiplayer.Static.ReplicationLayer.TimestampCorrectionMinimum = (int) slider.Value));
        this.AddCheckBox("Smooth timestamp correction", MyMultiplayer.Static.ReplicationLayer.UseSmoothCorrection, (Action<MyGuiControlCheckbox>) (x => MyMultiplayer.Static.ReplicationLayer.UseSmoothCorrection = x.IsChecked));
        this.AddSlider("Smooth timestamp correction amplitude", MyMultiplayer.Static.ReplicationLayer.SmoothCorrectionAmplitude, 0.0f, 5f, (Action<MyGuiControlSlider>) (slider => MyMultiplayer.Static.ReplicationLayer.SmoothCorrectionAmplitude = (float) (int) slider.Value));
      }
      this.AddCheckBox("Physics World Locking", MyFakes.WORLD_LOCKING_IN_CLIENTUPDATE, (Action<MyGuiControlCheckbox>) (x => MyFakes.WORLD_LOCKING_IN_CLIENTUPDATE = x.IsChecked));
      this.AddCheckBox("Pause physics", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.PAUSE_PHYSICS)));
      this.AddCheckBox("Client physics constraints", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.MULTIPLAYER_CLIENT_CONSTRAINTS)));
      this.AddCheckBox("New timing", MyReplicationClient.SynchronizationTimingType == MyReplicationClient.TimingType.LastServerTime, (Action<MyGuiControlCheckbox>) (x => MyReplicationClient.SynchronizationTimingType = x.IsChecked ? MyReplicationClient.TimingType.LastServerTime : MyReplicationClient.TimingType.ServerTimestep));
      this.AddSlider("Animation time shift [ms]", (float) MyAnimatedSnapshotSync.TimeShift.Milliseconds, 0.0f, 1000f, (Action<MyGuiControlSlider>) (slider => MyAnimatedSnapshotSync.TimeShift = MyTimeSpan.FromMilliseconds((double) slider.Value)));
      this.AddCheckBox("Prediction in jetpack", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.MULTIPLAYER_CLIENT_SIMULATE_CONTROLLED_CHARACTER_IN_JETPACK)));
      this.AddCheckBox("Prediction for grids", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.MULTIPLAYER_CLIENT_SIMULATE_CONTROLLED_GRID)));
      this.AddCheckBox("Skip prediction", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.MULTIPLAYER_SKIP_PREDICTION)));
      this.AddCheckBox("Skip prediction subgrids", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.MULTIPLAYER_SKIP_PREDICTION_SUBGRIDS)));
      this.AddCheckBox("Extrapolation smoothing", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.MULTIPLAYER_EXTRAPOLATION_SMOOTHING)));
      this.AddCheckBox("Skip animation", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.MULTIPLAYER_SKIP_ANIMATION)));
      this.AddCheckBox("SnapshotCache Hierarchy Propagation", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.SNAPSHOTCACHE_HIERARCHY)));
      this.AddCheckBox("Force Playout Delay Buffer", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.ForcePlayoutDelayBuffer)));
      this.AddCheckBox("World snapshots", MyFakes.WORLD_SNAPSHOTS, (Action<MyGuiControlCheckbox>) (x =>
      {
        MyFakes.WORLD_SNAPSHOTS = x.IsChecked;
        MyMultiplayer.RaiseStaticEvent<bool>((Func<IMyEventOwner, Action<bool>>) (y => new Action<bool>(MyGuiScreenDebugNetwork.OnWorldSnapshotsChange)), x.IsChecked);
      }));
      this.AddCheckBox("Mechanical Pivots in Snapshots", MyFakes.SNAPSHOTS_MECHANICAL_PIVOTS, (Action<MyGuiControlCheckbox>) (x =>
      {
        MyFakes.SNAPSHOTS_MECHANICAL_PIVOTS = x.IsChecked;
        MyMultiplayer.RaiseStaticEvent<bool>((Func<IMyEventOwner, Action<bool>>) (y => new Action<bool>(MyGuiScreenDebugNetwork.OnSnapshotsMechanicalPivotsChange)), x.IsChecked);
      }));
      this.AddCheckBox("Draw Spatial Layers", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyGuiScreenDebugNetwork.DebugDrawSpatialReplicationLayers)));
      this.AddCheckBox("Enable Debug Data Transfer", false, (Action<MyGuiControlCheckbox>) (cb =>
      {
        this.m_debugDataTransfer = cb.IsChecked;
        this.WakeDebugSender();
      }));
      this.AddCheckBox("Send Debug Data in Worker", true, (Action<MyGuiControlCheckbox>) (cb =>
      {
        this.m_sendInParallel = cb.IsChecked;
        this.WakeDebugSender();
      }));
      this.AddSlider("Debug Data Transfer", 10000f, 5000000f, (Func<float>) (() => (float) this.m_debugTransferRate), (Action<float>) (x => this.m_debugTransferRate = (int) x)).ValueChanged += (Action<MyGuiControlSlider>) (x => this.WakeDebugSender());
      this.m_dataRateLabel = this.AddLabel("No Debug Data Transfer", (Vector4) Color.White, 1f);
    }

    [ChatCommand("/nml", "", "", MyPromoteLevel.Admin)]
    private static void ChatCommandSetNetworkMonitorTimeout(string[] args)
    {
      int result;
      if (args == null || args.Length != 1 || !int.TryParse(args[0], out result))
        return;
      MyNetworkMonitor.UpdateLatency = result;
    }

    [Event(null, 149)]
    [Reliable]
    [Server]
    private static void OnSnapshotsMechanicalPivotsChange(bool state) => MyFakes.SNAPSHOTS_MECHANICAL_PIVOTS = state;

    [Event(null, 155)]
    [Reliable]
    [Server]
    private static void OnWorldSnapshotsChange(bool state) => MyFakes.WORLD_SNAPSHOTS = state;

    public override bool Update(bool hasFocus)
    {
      bool flag = base.Update(hasFocus);
      if (this.m_kickButton == null || this.m_entityLabel == null || MySession.Static == null)
        return flag;
      MyEntity myEntity = (MyEntity) null;
      if (MySession.Static != null)
      {
        LineD line = new LineD(MyBlockBuilderBase.IntersectionStart, MyBlockBuilderBase.IntersectionStart + MyBlockBuilderBase.IntersectionDirection * 500.0);
        MyIntersectionResultLineTriangleEx? intersectionWithLine = MyEntities.GetIntersectionWithLine(ref line, (MyEntity) MySession.Static.LocalCharacter, (MyEntity) null, ignoreObjectsWithoutPhysics: false);
        if (intersectionWithLine.HasValue)
          myEntity = intersectionWithLine.Value.Entity as MyEntity;
      }
      if (myEntity != this.m_currentEntity && !this.m_profileEntityLocked)
      {
        this.m_currentEntity = myEntity;
        this.m_kickButton.Enabled = this.m_currentEntity != null;
        this.m_entityLabel.Text = this.m_currentEntity != null ? this.m_currentEntity.DisplayName : "";
        this.m_profileLabel.Text = this.m_entityLabel.Text;
        MySnapshotCache.DEBUG_ENTITY_ID = this.m_currentEntity != null ? this.m_currentEntity.EntityId : 0L;
        MyFakes.VDB_ENTITY = this.m_currentEntity;
        MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (x => new Action<long>(MyMultiplayerBase.OnSetDebugEntity)), this.m_currentEntity == null ? 0L : this.m_currentEntity.EntityId);
      }
      return flag;
    }

    public override bool Draw()
    {
      this.DebugDraw();
      if (this.m_debugDataTransfer && MyScreenManager.GetFirstScreenOfType<MyGuiScreenGamePlay>() != null)
      {
        if (this.m_sendInParallel)
        {
          if (this.m_debugTransferThread == null)
          {
            this.m_debugTransferThread = new Thread(new ThreadStart(this.SendDataLoop))
            {
              Name = "Debug Data Transfer",
              IsBackground = true
            };
            this.m_debugTransferThread.Start();
          }
        }
        else
          this.SendPart(0.01666667f);
        lock (this.m_bytesPerSecond)
          this.m_dataRateLabel.Text = string.Format("{0} B/s", (object) ((double) this.m_bytesPerSecond.Total / this.m_bytesPerSecond.MaxTime.TotalSeconds));
      }
      else
        this.m_dataRateLabel.Text = "No Debug Data Transfer";
      return base.Draw();
    }

    protected override void OnClosed()
    {
      base.OnClosed();
      this.m_debugTransferRate = -1;
      this.m_debugTransferThread?.Join();
      this.m_debugTransferThread = (Thread) null;
    }

    private void SendDataLoop()
    {
      while (this.m_debugTransferRate >= 0)
      {
        if (!this.m_debugDataTransfer || !this.m_sendInParallel || !Sync.MultiplayerActive)
        {
          lock (this.m_debugTransferThread)
            Monitor.Wait((object) this.m_debugTransferThread);
        }
        else
        {
          this.SendPart((float) MyNetworkMonitor.UpdateLatency / 1000f);
          lock (this.m_debugTransferThread)
            Monitor.Wait((object) this.m_debugTransferThread, TimeSpan.FromMilliseconds((double) MyNetworkMonitor.UpdateLatency));
        }
      }
    }

    private void SendPart(float factor)
    {
      int length = (int) ((double) this.m_debugTransferRate * (double) factor);
      byte[] message = MyGuiScreenDebugNetwork.m_message;
      if ((message != null ? (message.Length != length ? 1 : 0) : 1) != 0)
        MyGuiScreenDebugNetwork.m_message = new byte[length];
      MyMultiplayer.RaiseStaticEvent<byte[]>((Func<IMyEventOwner, Action<byte[]>>) (x => new Action<byte[]>(MyGuiScreenDebugNetwork.SendDataServer)), MyGuiScreenDebugNetwork.m_message);
      lock (this.m_bytesPerSecond)
      {
        this.m_bytesPerSecond.Current += MyGuiScreenDebugNetwork.m_message.Length;
        this.m_bytesPerSecond.Advance();
      }
    }

    private void WakeDebugSender()
    {
      if (this.m_debugTransferThread == null || !this.m_debugDataTransfer)
        return;
      lock (this.m_debugTransferThread)
        Monitor.Pulse((object) this.m_debugTransferThread);
    }

    [Event(null, 292)]
    [Reliable]
    [Server]
    private static void SendDataServer(byte[] data) => MyMultiplayer.RaiseStaticEvent<byte[]>((Func<IMyEventOwner, Action<byte[]>>) (x => new Action<byte[]>(MyGuiScreenDebugNetwork.SendDataClient)), data, MyEventContext.Current.Sender);

    [Event(null, 298)]
    [Reliable]
    [Client]
    private static void SendDataClient(byte[] data)
    {
    }

    private void DebugDraw()
    {
      if (!MyGuiScreenDebugNetwork.DebugDrawSpatialReplicationLayers)
        return;
      if (!Sync.IsServer && MyScreenManager.GetFirstScreenOfType<MyGuiScreenGamePlay>() != null)
        MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (x => new Action(MyGuiScreenDebugNetwork.RequestLayersFromServer)));
      if (this.m_layers == null)
        return;
      for (int index = 0; index < this.m_layers.Count; ++index)
      {
        MyGuiScreenDebugNetwork.Layer layer = this.m_layers[index];
        Color lodColor = (Color) MyClipmap.LodColors[index];
        float num = layer.Enabled ? 1f : 0.3f;
        Color color1 = lodColor.Alpha(num);
        MyRenderProxy.DebugDrawAABB((BoundingBoxD) layer.Bounds, color1, num);
        foreach (MyGuiScreenDebugNetwork.Layer.Entity entity in layer.Entities)
        {
          if (entity.Bounds.HasValue)
          {
            Color color2 = color1;
            if (!MyEntities.EntityExists(entity.Id))
              color2 = color1.Alpha(0.3f);
            BoundingBoxD aabb = entity.Bounds.Value;
            MyRenderProxy.DebugDrawAABB(aabb, color2, (float) color2.A / (float) byte.MaxValue);
            MyRenderProxy.DebugDrawText3D(aabb.Center, entity.Id.ToString(), color2, 0.5f, true, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
          }
        }
        string text = string.Format("[{0}] {1} PCU {2}", (object) index, (object) layer.PCU, layer.Enabled ? (object) "Enabled" : (object) "Disabled");
        MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, (float) (200 + index * 17)), text, color1.Alpha(1f), 0.7f);
      }
    }

    [Event(null, 396)]
    [Reliable]
    [Server]
    private static void RequestLayersFromServer()
    {
      List<MyGuiScreenDebugNetwork.Layer> layerList = new List<MyGuiScreenDebugNetwork.Layer>();
      MyGuiScreenDebugNetwork.GetLayerData(MyEventContext.Current.Sender, layerList);
      MyMultiplayer.RaiseStaticEvent<List<MyGuiScreenDebugNetwork.Layer>>((Func<IMyEventOwner, Action<List<MyGuiScreenDebugNetwork.Layer>>>) (x => new Action<List<MyGuiScreenDebugNetwork.Layer>>(MyGuiScreenDebugNetwork.ReceiveLayersFromServer)), layerList, MyEventContext.Current.Sender);
    }

    [Event(null, 405)]
    [Reliable]
    [Client]
    private static void ReceiveLayersFromServer(List<MyGuiScreenDebugNetwork.Layer> layers)
    {
      MyGuiScreenDebugNetwork firstScreenOfType = MyScreenManager.GetFirstScreenOfType<MyGuiScreenDebugNetwork>();
      if (firstScreenOfType == null)
        return;
      firstScreenOfType.m_layers = layers;
    }

    private static void GetLayerData(
      EndpointId endpoint,
      List<MyGuiScreenDebugNetwork.Layer> layerList)
    {
      foreach ((BoundingBoxD Bounds, IEnumerable<IMyReplicable> Replicables, int PCU, bool Enabled) tuple in ((MyReplicationServer) MyMultiplayer.Static.ReplicationLayer).GetLayerData(endpoint))
        layerList.Add(new MyGuiScreenDebugNetwork.Layer((BoundingBox) tuple.Bounds, tuple.Replicables.OfType<IMyEntityReplicable>().Where<IMyEntityReplicable>((Func<IMyEntityReplicable, bool>) (x => !(x is MyVoxelReplicable))).Select<IMyEntityReplicable, MyGuiScreenDebugNetwork.Layer.Entity>(new Func<IMyEntityReplicable, MyGuiScreenDebugNetwork.Layer.Entity>(GetEntity)).ToList<MyGuiScreenDebugNetwork.Layer.Entity>(), tuple.PCU, tuple.Enabled));

      MyGuiScreenDebugNetwork.Layer.Entity GetEntity(IMyEntityReplicable r)
      {
        BoundingBoxD aabb = ((MyExternalReplicable) r).GetAABB();
        return new MyGuiScreenDebugNetwork.Layer.Entity(r.EntityId, new BoundingBoxD?(aabb));
      }
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugNetwork);

    private class IntArithmetic : MyTimedStatWindow.IStatArithmetic<int>
    {
      public void Add(in int lhs, in int rhs, out int result) => result = lhs + rhs;

      public void Subtract(in int lhs, in int rhs, out int result) => result = lhs - rhs;

      void MyTimedStatWindow.IStatArithmetic<int>.Add(
        in int lhs,
        in int rhs,
        out int result)
      {
        this.Add(in lhs, in rhs, out result);
      }

      void MyTimedStatWindow.IStatArithmetic<int>.Subtract(
        in int lhs,
        in int rhs,
        out int result)
      {
        this.Subtract(in lhs, in rhs, out result);
      }
    }

    [Serializable]
    private struct Layer
    {
      public BoundingBox Bounds;
      public List<MyGuiScreenDebugNetwork.Layer.Entity> Entities;
      public int PCU;
      public bool Enabled;

      public Layer(
        BoundingBox bounds,
        List<MyGuiScreenDebugNetwork.Layer.Entity> entities,
        int pcu,
        bool enabled)
      {
        this.Bounds = bounds;
        this.Entities = entities;
        this.PCU = pcu;
        this.Enabled = enabled;
      }

      [Serializable]
      public struct Entity
      {
        public long Id;
        public BoundingBoxD? Bounds;

        public Entity(long id, BoundingBoxD? bounds)
        {
          this.Id = id;
          this.Bounds = bounds;
        }

        protected class Sandbox_Game_Gui_MyGuiScreenDebugNetwork\u003C\u003ELayer\u003C\u003EEntity\u003C\u003EId\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenDebugNetwork.Layer.Entity, long>
        {
          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Set(ref MyGuiScreenDebugNetwork.Layer.Entity owner, in long value) => owner.Id = value;

          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Get(ref MyGuiScreenDebugNetwork.Layer.Entity owner, out long value) => value = owner.Id;
        }

        protected class Sandbox_Game_Gui_MyGuiScreenDebugNetwork\u003C\u003ELayer\u003C\u003EEntity\u003C\u003EBounds\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenDebugNetwork.Layer.Entity, BoundingBoxD?>
        {
          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Set(
            ref MyGuiScreenDebugNetwork.Layer.Entity owner,
            in BoundingBoxD? value)
          {
            owner.Bounds = value;
          }

          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Get(
            ref MyGuiScreenDebugNetwork.Layer.Entity owner,
            out BoundingBoxD? value)
          {
            value = owner.Bounds;
          }
        }
      }

      protected class Sandbox_Game_Gui_MyGuiScreenDebugNetwork\u003C\u003ELayer\u003C\u003EBounds\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenDebugNetwork.Layer, BoundingBox>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGuiScreenDebugNetwork.Layer owner, in BoundingBox value) => owner.Bounds = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGuiScreenDebugNetwork.Layer owner, out BoundingBox value) => value = owner.Bounds;
      }

      protected class Sandbox_Game_Gui_MyGuiScreenDebugNetwork\u003C\u003ELayer\u003C\u003EEntities\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenDebugNetwork.Layer, List<MyGuiScreenDebugNetwork.Layer.Entity>>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyGuiScreenDebugNetwork.Layer owner,
          in List<MyGuiScreenDebugNetwork.Layer.Entity> value)
        {
          owner.Entities = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyGuiScreenDebugNetwork.Layer owner,
          out List<MyGuiScreenDebugNetwork.Layer.Entity> value)
        {
          value = owner.Entities;
        }
      }

      protected class Sandbox_Game_Gui_MyGuiScreenDebugNetwork\u003C\u003ELayer\u003C\u003EPCU\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenDebugNetwork.Layer, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGuiScreenDebugNetwork.Layer owner, in int value) => owner.PCU = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGuiScreenDebugNetwork.Layer owner, out int value) => value = owner.PCU;
      }

      protected class Sandbox_Game_Gui_MyGuiScreenDebugNetwork\u003C\u003ELayer\u003C\u003EEnabled\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenDebugNetwork.Layer, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGuiScreenDebugNetwork.Layer owner, in bool value) => owner.Enabled = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGuiScreenDebugNetwork.Layer owner, out bool value) => value = owner.Enabled;
      }
    }

    protected sealed class OnSnapshotsMechanicalPivotsChange\u003C\u003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in bool state,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenDebugNetwork.OnSnapshotsMechanicalPivotsChange(state);
      }
    }

    protected sealed class OnWorldSnapshotsChange\u003C\u003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in bool state,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenDebugNetwork.OnWorldSnapshotsChange(state);
      }
    }

    protected sealed class SendDataServer\u003C\u003ESystem_Byte\u003C\u0023\u003E : ICallSite<IMyEventOwner, byte[], DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in byte[] data,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenDebugNetwork.SendDataServer(data);
      }
    }

    protected sealed class SendDataClient\u003C\u003ESystem_Byte\u003C\u0023\u003E : ICallSite<IMyEventOwner, byte[], DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in byte[] data,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenDebugNetwork.SendDataClient(data);
      }
    }

    protected sealed class RequestLayersFromServer\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenDebugNetwork.RequestLayersFromServer();
      }
    }

    protected sealed class ReceiveLayersFromServer\u003C\u003ESystem_Collections_Generic_List`1\u003CSandbox_Game_Gui_MyGuiScreenDebugNetwork\u003C\u003ELayer\u003E : ICallSite<IMyEventOwner, List<MyGuiScreenDebugNetwork.Layer>, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in List<MyGuiScreenDebugNetwork.Layer> layers,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenDebugNetwork.ReceiveLayersFromServer(layers);
      }
    }
  }
}
