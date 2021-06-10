// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyLaserAntenna
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Graphics;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
  [MyCubeBlockType(typeof (MyObjectBuilder_LaserAntenna))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyLaserAntenna), typeof (Sandbox.ModAPI.Ingame.IMyLaserAntenna)})]
  public class MyLaserAntenna : MyFunctionalBlock, IMyGizmoDrawableObject, Sandbox.ModAPI.IMyLaserAntenna, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyLaserAntenna
  {
    protected Color m_gizmoColor = (Color) new Vector4(0.1f, 0.1f, 0.0f, 0.1f);
    protected const float m_maxGizmoDrawDistance = 10000f;
    private bool m_onceUpdated;
    private const double PHYSICS_CAST_LENGTH = 25.0;
    private const float INFINITE_RANGE = 1E+08f;
    private bool resetPartsParent;
    private MyLaserAntenna.StateEnum m_state;
    private long? m_targetId;
    private readonly StringBuilder m_lastKnownTargetName = new StringBuilder();
    private static string m_clipboardText;
    private readonly StringBuilder m_termGpsName = new StringBuilder();
    private Vector3D? m_termGpsCoords;
    private long? m_selectedEntityId;
    private bool m_rotationFinished = true;
    private float m_needRotation;
    private float m_needElevation;
    private float m_minElevationRadians;
    private float m_maxElevationRadians = 6.283185f;
    private float m_minAzimuthRadians;
    private float m_maxAzimuthRadians = 6.283185f;
    private bool m_outsideLimits;
    private Vector3D m_targetCoords;
    private float m_maxRange;
    private VRage.Sync.Sync<float, SyncDirection.BothWays> m_range;
    protected static float m_Max_LosDist = 10000f;
    private VRage.Sync.Sync<bool, SyncDirection.FromServer> m_permanentConnection;
    private bool m_OnlyPermanentExists;
    public bool m_needLineOfSight = true;
    private HashSet<VRage.ModAPI.IMyEntity> m_children = new HashSet<VRage.ModAPI.IMyEntity>();
    private static MyTerminalControlButton<MyLaserAntenna> idleButton;
    private static MyTerminalControlButton<MyLaserAntenna> connectGPS;
    private static MyTerminalControlListbox<MyLaserAntenna> receiversList;
    private static MyTerminalControlTextbox<MyLaserAntenna> gpsCoords;
    private static MyTerminalControlButton<MyLaserAntenna> PasteGpsCoords;
    private static MyTerminalControlButton<MyLaserAntenna> ConnectReceiver;
    private Vector3D m_temp;
    protected StringBuilder m_tempSB = new StringBuilder();
    protected VRage.Sync.Sync<bool, SyncDirection.FromServer> m_canLaseTargetCoords;
    private List<MyPhysics.HitInfo> m_hits = new List<MyPhysics.HitInfo>();
    private List<MyLineSegmentOverlapResult<MyVoxelBase>> m_voxelHits = new List<MyLineSegmentOverlapResult<MyVoxelBase>>();
    private List<MyLineSegmentOverlapResult<MyEntity>> m_entityHits = new List<MyLineSegmentOverlapResult<MyEntity>>();
    private bool m_wasVisible;
    private float m_rotation;
    private float m_elevation;
    protected MyEntity m_base1;
    protected MyEntity m_base2;
    protected int m_rotationInterval_ms;
    protected int m_elevationInterval_ms;

    private MyLaserBroadcaster Broadcaster
    {
      get => (MyLaserBroadcaster) this.Components.Get<MyDataBroadcaster>();
      set => this.Components.Add<MyDataBroadcaster>((MyDataBroadcaster) value);
    }

    private MyLaserReceiver Receiver
    {
      get => (MyLaserReceiver) this.Components.Get<MyDataReceiver>();
      set => this.Components.Add<MyDataReceiver>((MyDataReceiver) value);
    }

    public MyLaserAntenna.StateEnum State
    {
      get => this.m_state;
      private set
      {
        this.m_state = value;
        if (this.m_state != MyLaserAntenna.StateEnum.idle && this.m_state != MyLaserAntenna.StateEnum.rot_GPS)
          return;
        this.m_targetId = new long?();
      }
    }

    public long? TargetId => this.m_targetId;

    public Vector3D HeadPos => this.m_base2 != null ? this.m_base2.PositionComp.GetPosition() : this.PositionComp.GetPosition();

    public MatrixD InitializationMatrix { get; private set; }

    public Color GetGizmoColor() => this.m_gizmoColor;

    public Vector3 GetPositionInGrid() => (Vector3) this.Position;

    public float GetRadius() => 100f;

    public bool CanBeDrawn() => MyCubeGrid.ShowAntennaGizmos && this.IsWorking && (this.HasLocalPlayerAccess() && this.GetDistanceBetweenCameraAndBoundingSphere() <= 10000.0);

    public BoundingBox? GetBoundingBox() => new BoundingBox?();

    public MatrixD GetWorldMatrix() => this.PositionComp.WorldMatrixRef;

    public bool EnableLongDrawDistance() => true;

    public Vector3D TargetCoords => this.m_targetCoords;

    public bool CanLaseTargetCoords => (bool) this.m_canLaseTargetCoords;

    public MyLaserAntennaDefinition BlockDefinition => (MyLaserAntennaDefinition) base.BlockDefinition;

    public MyLaserAntenna() => this.CreateTerminalControls();

    static MyLaserAntenna() => MyLaserAntenna.m_Max_LosDist = (float) MySession.Static.Settings.ViewDistance;

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyLaserAntenna>())
        return;
      base.CreateTerminalControls();
      MyLaserAntenna.idleButton = new MyTerminalControlButton<MyLaserAntenna>("Idle", MySpaceTexts.LaserAntennaIdleButton, MySpaceTexts.Blank, (Action<MyLaserAntenna>) (self =>
      {
        self.SetIdle();
        MyLaserAntenna.idleButton.UpdateVisual();
      }));
      MyLaserAntenna.idleButton.Enabled = (Func<MyLaserAntenna, bool>) (x => (uint) x.m_state > 0U);
      MyLaserAntenna.idleButton.EnableAction<MyLaserAntenna>();
      MyTerminalControlFactory.AddControl<MyLaserAntenna>((MyTerminalControl<MyLaserAntenna>) MyLaserAntenna.idleButton);
      MyTerminalControlSlider<MyLaserAntenna> slider = new MyTerminalControlSlider<MyLaserAntenna>("Range", MySpaceTexts.BlockPropertyTitle_LaserRange, MySpaceTexts.BlockPropertyDescription_LaserRange);
      slider.SetLogLimits((MyTerminalValueControl<MyLaserAntenna, float>.GetterDelegate) (block => 1f), (MyTerminalValueControl<MyLaserAntenna, float>.GetterDelegate) (block => (double) block.m_maxRange >= 0.0 ? block.m_maxRange : 1E+08f));
      slider.DefaultValueGetter = (MyTerminalValueControl<MyLaserAntenna, float>.GetterDelegate) (block => (double) block.m_maxRange >= 0.0 ? block.m_maxRange : 1E+08f);
      slider.Getter = (MyTerminalValueControl<MyLaserAntenna, float>.GetterDelegate) (x => x.m_range.Value);
      slider.Setter = (MyTerminalValueControl<MyLaserAntenna, float>.SetterDelegate) ((x, v) => x.m_range.Value = v);
      slider.Writer = (MyTerminalControl<MyLaserAntenna>.WriterDelegate) ((x, result) =>
      {
        if ((double) x.m_range.Value < 100000000.0)
          MyValueFormatter.AppendDistanceInBestUnit((float) (int) x.m_range.Value, result);
        else
          result.Append(MyTexts.GetString(MySpaceTexts.ScreenTerminal_Infinite));
      });
      slider.EnableActions<MyLaserAntenna>();
      MyTerminalControlFactory.AddControl<MyLaserAntenna>((MyTerminalControl<MyLaserAntenna>) slider);
      MyTerminalControlFactory.AddControl<MyLaserAntenna>((MyTerminalControl<MyLaserAntenna>) new MyTerminalControlSeparator<MyLaserAntenna>());
      MyTerminalControlFactory.AddControl<MyLaserAntenna>((MyTerminalControl<MyLaserAntenna>) new MyTerminalControlButton<MyLaserAntenna>("CopyCoords", MySpaceTexts.LaserAntennaCopyCoords, MySpaceTexts.LaserAntennaCopyCoordsHelp, (Action<MyLaserAntenna>) (self =>
      {
        StringBuilder stringBuilder1 = new StringBuilder(self.DisplayNameText);
        stringBuilder1.Replace(':', ' ');
        StringBuilder stringBuilder2 = new StringBuilder("GPS:", 256);
        stringBuilder2.Append((object) stringBuilder1);
        stringBuilder2.Append(":");
        stringBuilder2.Append(Math.Round(self.HeadPos.X, 2).ToString((IFormatProvider) CultureInfo.InvariantCulture));
        stringBuilder2.Append(":");
        stringBuilder2.Append(Math.Round(self.HeadPos.Y, 2).ToString((IFormatProvider) CultureInfo.InvariantCulture));
        stringBuilder2.Append(":");
        stringBuilder2.Append(Math.Round(self.HeadPos.Z, 2).ToString((IFormatProvider) CultureInfo.InvariantCulture));
        stringBuilder2.Append(":");
        MyVRage.Platform.System.Clipboard = stringBuilder2.ToString();
      }), true));
      MyTerminalControlButton<MyLaserAntenna> terminalControlButton = new MyTerminalControlButton<MyLaserAntenna>("CopyTargetCoords", MySpaceTexts.LaserAntennaCopyTargetCoords, MySpaceTexts.LaserAntennaCopyTargetCoordsHelp, (Action<MyLaserAntenna>) (self =>
      {
        if (!self.m_targetId.HasValue)
          return;
        StringBuilder stringBuilder1 = new StringBuilder(self.m_lastKnownTargetName.ToString());
        stringBuilder1.Replace(':', ' ');
        StringBuilder stringBuilder2 = new StringBuilder("GPS:", 256);
        stringBuilder2.Append((object) stringBuilder1);
        stringBuilder2.Append(":");
        stringBuilder2.Append(Math.Round(self.m_targetCoords.X, 2).ToString((IFormatProvider) CultureInfo.InvariantCulture));
        stringBuilder2.Append(":");
        stringBuilder2.Append(Math.Round(self.m_targetCoords.Y, 2).ToString((IFormatProvider) CultureInfo.InvariantCulture));
        stringBuilder2.Append(":");
        stringBuilder2.Append(Math.Round(self.m_targetCoords.Z, 2).ToString((IFormatProvider) CultureInfo.InvariantCulture));
        stringBuilder2.Append(":");
        MyVRage.Platform.System.Clipboard = stringBuilder2.ToString();
      }), true);
      terminalControlButton.Enabled = (Func<MyLaserAntenna, bool>) (x => x.m_targetId.HasValue);
      MyTerminalControlFactory.AddControl<MyLaserAntenna>((MyTerminalControl<MyLaserAntenna>) terminalControlButton);
      MyLaserAntenna.PasteGpsCoords = new MyTerminalControlButton<MyLaserAntenna>("PasteGpsCoords", MySpaceTexts.LaserAntennaPasteGPS, MySpaceTexts.Blank, (Action<MyLaserAntenna>) (self =>
      {
        Thread thread = new Thread((ThreadStart) (() => MyLaserAntenna.PasteFromClipboard()));
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();
        self.PasteCoordinates(MyLaserAntenna.m_clipboardText);
      }));
      MyLaserAntenna.PasteGpsCoords.EnableAction<MyLaserAntenna>();
      MyTerminalControlFactory.AddControl<MyLaserAntenna>((MyTerminalControl<MyLaserAntenna>) MyLaserAntenna.PasteGpsCoords);
      MyLaserAntenna.gpsCoords = new MyTerminalControlTextbox<MyLaserAntenna>("gpsCoords", MySpaceTexts.LaserAntennaSelectedCoords, MySpaceTexts.Blank);
      MyLaserAntenna.gpsCoords.Getter = (MyTerminalControlTextbox<MyLaserAntenna>.GetterDelegate) (x => x.m_termGpsName);
      MyLaserAntenna.gpsCoords.Enabled = (Func<MyLaserAntenna, bool>) (x => false);
      MyTerminalControlFactory.AddControl<MyLaserAntenna>((MyTerminalControl<MyLaserAntenna>) MyLaserAntenna.gpsCoords);
      MyLaserAntenna.connectGPS = new MyTerminalControlButton<MyLaserAntenna>("ConnectGPS", MySpaceTexts.LaserAntennaConnectGPS, MySpaceTexts.Blank, (Action<MyLaserAntenna>) (self =>
      {
        if (!self.m_termGpsCoords.HasValue)
          return;
        self.ConnectToGps();
      }), true);
      MyLaserAntenna.connectGPS.Enabled = (Func<MyLaserAntenna, bool>) (x => x.CanConnectToGPS());
      MyLaserAntenna.connectGPS.EnableAction<MyLaserAntenna>();
      MyTerminalControlFactory.AddControl<MyLaserAntenna>((MyTerminalControl<MyLaserAntenna>) MyLaserAntenna.connectGPS);
      MyTerminalControlCheckbox<MyLaserAntenna> checkbox = new MyTerminalControlCheckbox<MyLaserAntenna>("isPerm", MySpaceTexts.LaserAntennaPermanentCheckbox, MySpaceTexts.Blank);
      checkbox.Getter = (MyTerminalValueControl<MyLaserAntenna, bool>.GetterDelegate) (self => self.m_permanentConnection.Value);
      checkbox.Setter = (MyTerminalValueControl<MyLaserAntenna, bool>.SetterDelegate) ((self, v) => self.ChangePerm(v));
      checkbox.Enabled = (Func<MyLaserAntenna, bool>) (self => self.State == MyLaserAntenna.StateEnum.connected);
      checkbox.EnableAction<MyLaserAntenna>();
      MyTerminalControlFactory.AddControl<MyLaserAntenna>((MyTerminalControl<MyLaserAntenna>) checkbox);
      MyTerminalControlFactory.AddControl<MyLaserAntenna>((MyTerminalControl<MyLaserAntenna>) new MyTerminalControlSeparator<MyLaserAntenna>());
      MyLaserAntenna.receiversList = new MyTerminalControlListbox<MyLaserAntenna>("receiversList", MySpaceTexts.LaserAntennaReceiversList, MySpaceTexts.LaserAntennaReceiversListHelp);
      MyLaserAntenna.receiversList.ListContent = (MyTerminalControlListbox<MyLaserAntenna>.ListContentDelegate) ((x, population, selected, focusedItem) => x.PopulatePossibleReceivers(population, selected));
      MyLaserAntenna.receiversList.ItemSelected = (MyTerminalControlListbox<MyLaserAntenna>.SelectItemDelegate) ((x, y) => x.ReceiverSelected(y));
      MyTerminalControlFactory.AddControl<MyLaserAntenna>((MyTerminalControl<MyLaserAntenna>) MyLaserAntenna.receiversList);
      MyLaserAntenna.ConnectReceiver = new MyTerminalControlButton<MyLaserAntenna>("ConnectReceiver", MySpaceTexts.LaserAntennaConnectButton, MySpaceTexts.Blank, (Action<MyLaserAntenna>) (self => self.ConnectToId()));
      MyLaserAntenna.ConnectReceiver.Enabled = (Func<MyLaserAntenna, bool>) (x => x.m_selectedEntityId.HasValue);
      MyTerminalControlFactory.AddControl<MyLaserAntenna>((MyTerminalControl<MyLaserAntenna>) MyLaserAntenna.ConnectReceiver);
    }

    private static void UpdateVisuals()
    {
      MyLaserAntenna.gpsCoords.UpdateVisual();
      MyLaserAntenna.idleButton.UpdateVisual();
      MyLaserAntenna.connectGPS.UpdateVisual();
      MyLaserAntenna.receiversList.UpdateVisual();
      MyLaserAntenna.ConnectReceiver.UpdateVisual();
    }

    private bool CanConnectToGPS() => this.m_termGpsCoords.HasValue && this.Dist2To(this.m_termGpsCoords) >= 1.0;

    private static void PasteFromClipboard() => MyLaserAntenna.m_clipboardText = MyVRage.Platform.System.Clipboard;

    public void DoPasteCoords(string str)
    {
      if (MyGpsCollection.ParseOneGPS(str, this.m_termGpsName, ref this.m_temp))
      {
        if (!this.m_termGpsCoords.HasValue)
          this.m_termGpsCoords = new Vector3D?(this.m_temp);
        this.m_termGpsCoords = new Vector3D?(this.m_temp);
        this.m_termGpsName.Append(" ").Append(this.m_temp.X).Append(":").Append(this.m_temp.Y).Append(":").Append(this.m_temp.Z);
        this.ConnectToGps(this.m_termGpsCoords);
      }
      MyLaserAntenna.UpdateVisuals();
    }

    public override bool GetIntersectionWithAABB(ref BoundingBoxD aabb)
    {
      this.Hierarchy.GetChildrenRecursive(this.m_children);
      foreach (MyEntity child in this.m_children)
      {
        MyModel model = child.Model;
        if (model != null && model.GetTrianglePruningStructure().GetIntersectionWithAABB((VRage.ModAPI.IMyEntity) child, ref aabb))
          return true;
      }
      MyModel model1 = this.Model;
      return model1 != null && model1.GetTrianglePruningStructure().GetIntersectionWithAABB((VRage.ModAPI.IMyEntity) this, ref aabb);
    }

    protected void UpdateMyStateText()
    {
      StringBuilder stateText = this.Broadcaster.StateText;
      stateText.Clear().Append((object) this.CustomName);
      stateText.Append(" [");
      switch (this.State)
      {
        case MyLaserAntenna.StateEnum.idle:
          stateText.Append((object) this.State);
          break;
        case MyLaserAntenna.StateEnum.rot_GPS:
        case MyLaserAntenna.StateEnum.rot_Rec:
          if (this.m_permanentConnection.Value)
          {
            stateText.Append("#>>");
            break;
          }
          stateText.Append(">>");
          break;
        case MyLaserAntenna.StateEnum.search_GPS:
          stateText.Append("?>");
          break;
        case MyLaserAntenna.StateEnum.contact_Rec:
          if (this.m_permanentConnection.Value)
          {
            stateText.Append("#~>");
            break;
          }
          stateText.Append("~>");
          break;
        case MyLaserAntenna.StateEnum.connected:
          if (this.m_permanentConnection.Value)
          {
            stateText.Append("#=>");
            break;
          }
          stateText.Append("=>");
          break;
      }
      if (this.State == MyLaserAntenna.StateEnum.connected || this.State == MyLaserAntenna.StateEnum.contact_Rec || this.State == MyLaserAntenna.StateEnum.rot_Rec)
        stateText.Append((object) this.m_lastKnownTargetName);
      else if (this.State == MyLaserAntenna.StateEnum.rot_GPS || this.State == MyLaserAntenna.StateEnum.search_GPS)
      {
        stateText.Append((object) this.m_termGpsName);
        stateText.Append(" ");
        stateText.Append((object) this.m_termGpsCoords);
      }
      stateText.Append("]");
      this.Broadcaster.RaiseChangeStateText();
    }

    protected void PopulatePossibleReceivers(
      ICollection<MyGuiControlListbox.Item> population,
      ICollection<MyGuiControlListbox.Item> selected)
    {
      if (MySession.Static == null || this.Closed)
        return;
      foreach (MyLaserBroadcaster laserBroadcaster in MyAntennaSystem.Static.LaserAntennas.Values)
      {
        if (laserBroadcaster != this.Broadcaster && (laserBroadcaster.RealAntenna == null || laserBroadcaster.RealAntenna.Enabled && laserBroadcaster.RealAntenna.IsFunctional && (double) this.ResourceSink.SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId) > 0.990000009536743) && (laserBroadcaster.CanBeUsedByPlayer(this.OwnerId) && this.Broadcaster.CanBeUsedByPlayer(laserBroadcaster.Owner) && MyAntennaSystem.Static.GetAllRelayedBroadcasters((MyDataReceiver) this.Receiver, this.OwnerId, false).Contains((MyDataBroadcaster) laserBroadcaster)))
        {
          MyGuiControlListbox.Item obj = new MyGuiControlListbox.Item(ref laserBroadcaster.StateText, userData: ((object) laserBroadcaster));
          population.Add(obj);
          long? selectedEntityId = this.m_selectedEntityId;
          long antennaEntityId = laserBroadcaster.AntennaEntityId;
          if (selectedEntityId.GetValueOrDefault() == antennaEntityId & selectedEntityId.HasValue)
            selected.Add(obj);
        }
      }
      MyLaserAntenna.ConnectReceiver.UpdateVisual();
    }

    protected void ReceiverSelected(List<MyGuiControlListbox.Item> y)
    {
      this.m_selectedEntityId = new long?((y.First<MyGuiControlListbox.Item>().UserData as MyLaserBroadcaster).AntennaEntityId);
      MyLaserAntenna.ConnectReceiver.UpdateVisual();
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.ResourceSink = new MyResourceSinkComponent();
      this.ResourceSink.Init(this.BlockDefinition.ResourceSinkGroup, this.BlockDefinition.PowerInputLasing, new Func<float>(this.UpdatePowerInput));
      this.Broadcaster = new MyLaserBroadcaster();
      this.Receiver = new MyLaserReceiver();
      base.Init(objectBuilder, cubeGrid);
      MyObjectBuilder_LaserAntenna builderLaserAntenna = (MyObjectBuilder_LaserAntenna) objectBuilder;
      this.State = (MyLaserAntenna.StateEnum) ((uint) builderLaserAntenna.State & 7U);
      this.m_permanentConnection.ValueChanged += new Action<SyncBase>(this.PermanentConnectionOnValueChanged);
      this.m_permanentConnection.SetLocalValue(((uint) builderLaserAntenna.State & 8U) > 0U);
      this.m_targetId = builderLaserAntenna.targetEntityId;
      this.m_lastKnownTargetName.Append(builderLaserAntenna.LastKnownTargetName);
      if (builderLaserAntenna.gpsTarget.HasValue)
        this.m_termGpsCoords = builderLaserAntenna.gpsTarget;
      this.m_termGpsName.Clear().Append(builderLaserAntenna.gpsTargetName);
      this.m_rotation = builderLaserAntenna.HeadRotation.X;
      this.m_elevation = builderLaserAntenna.HeadRotation.Y;
      this.m_targetCoords = builderLaserAntenna.LastTargetPosition;
      this.m_maxRange = this.BlockDefinition.MaxRange;
      this.m_range.Value = builderLaserAntenna.Range;
      this.m_needLineOfSight = this.BlockDefinition.RequireLineOfSight;
      if (this.BlockDefinition != null)
      {
        this.m_minElevationRadians = MathHelper.ToRadians(this.NormalizeAngle(this.BlockDefinition.MinElevationDegrees));
        this.m_maxElevationRadians = MathHelper.ToRadians(this.NormalizeAngle(this.BlockDefinition.MaxElevationDegrees));
        if ((double) this.m_minElevationRadians > (double) this.m_maxElevationRadians)
          this.m_minElevationRadians -= 6.283185f;
        this.m_minAzimuthRadians = MathHelper.ToRadians(this.NormalizeAngle(this.BlockDefinition.MinAzimuthDegrees));
        this.m_maxAzimuthRadians = MathHelper.ToRadians(this.NormalizeAngle(this.BlockDefinition.MaxAzimuthDegrees));
        if ((double) this.m_minAzimuthRadians > (double) this.m_maxAzimuthRadians)
          this.m_minAzimuthRadians -= 6.283185f;
        this.ClampRotationAndElevation();
      }
      this.InitializationMatrix = (MatrixD) ref this.PositionComp.LocalMatrixRef;
      this.ResourceSink.IsPoweredChanged += new Action(this.IsPoweredChanged);
      this.ResourceSink.Update();
      this.OnClose += (Action<MyEntity>) (_param1 => this.OnClosed());
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.Receiver.Enabled = this.IsWorking;
      this.UpdateEmissivity();
      this.NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    private void PermanentConnectionOnValueChanged(SyncBase obj)
    {
      if (!this.IsReadyForReplication)
        return;
      this.UpdateMyStateText();
    }

    protected float NormalizeAngle(int angle)
    {
      int num = angle % 360;
      return num == 0 && angle != 0 ? 360f : (float) num;
    }

    protected void ClampRotationAndElevation()
    {
      float num1 = this.ClampRotation(this.m_rotation);
      float num2 = this.ClampElevation(this.m_elevation);
      this.m_outsideLimits = (double) num1 != (double) this.m_rotation || (double) num2 != (double) this.m_elevation;
      this.m_rotation = num1;
      this.m_elevation = num2;
    }

    private float ClampRotation(float value)
    {
      if (this.IsRotationLimited())
        value = Math.Min(this.m_maxAzimuthRadians, Math.Max(this.m_minAzimuthRadians, value));
      return value;
    }

    private bool IsRotationLimited() => (double) Math.Abs((float) ((double) this.m_maxAzimuthRadians - (double) this.m_minAzimuthRadians - 6.28318548202515)) > 0.01;

    private float ClampElevation(float value)
    {
      if (this.IsElevationLimited())
        value = Math.Min(this.m_maxElevationRadians, Math.Max(this.m_minElevationRadians, value));
      return value;
    }

    private bool IsElevationLimited() => (double) Math.Abs((float) ((double) this.m_maxElevationRadians - (double) this.m_minElevationRadians - 6.28318548202515)) > 0.01;

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      this.SetParent();
      if (!this.m_onceUpdated)
      {
        if (this.CubeGrid.Physics != null)
          MyAntennaSystem.Static.AddLaser(this.Broadcaster.AntennaEntityId, this.Broadcaster);
        this.ResourceSink.Update();
        this.UpdateMyStateText();
        this.UpdateEmissivity();
      }
      this.m_onceUpdated = true;
    }

    private void SetParent()
    {
      MyCubeGridRenderCell orAddCell = this.CubeGrid.RenderData.GetOrAddCell(this.Position * this.CubeGrid.GridSize);
      if (this.m_base1 != null)
      {
        this.m_base1.Render.SetParent(0, orAddCell.ParentCullObject);
        this.m_base1.NeedsWorldMatrix = false;
        this.m_base1.InvalidateOnMove = false;
      }
      if (this.m_base2 == null)
        return;
      this.m_base2.Render.SetParent(0, orAddCell.ParentCullObject);
      this.m_base2.NeedsWorldMatrix = false;
      this.m_base2.InvalidateOnMove = false;
    }

    public override void OnCubeGridChanged(MyCubeGrid oldGrid)
    {
      base.OnCubeGridChanged(oldGrid);
      this.resetPartsParent = true;
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      this.UpdateEmissivity();
    }

    protected override void OnOwnershipChanged()
    {
      base.OnOwnershipChanged();
      this.Broadcaster.RaiseOwnerChanged();
      this.Receiver.UpdateBroadcastersInRange();
      MyLaserAntenna.UpdateVisuals();
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_LaserAntenna builderCubeBlock = (MyObjectBuilder_LaserAntenna) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.State = (byte) (this.State | (this.m_permanentConnection.Value ? (MyLaserAntenna.StateEnum) 8 : MyLaserAntenna.StateEnum.idle));
      builderCubeBlock.targetEntityId = this.m_targetId;
      builderCubeBlock.gpsTarget = this.m_termGpsCoords;
      builderCubeBlock.gpsTargetName = this.m_termGpsName.ToString();
      builderCubeBlock.HeadRotation = new Vector2(this.m_rotation, this.m_elevation);
      builderCubeBlock.LastTargetPosition = this.m_targetCoords;
      builderCubeBlock.LastKnownTargetName = this.m_lastKnownTargetName.ToString();
      builderCubeBlock.Range = this.m_range.Value;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (this.resetPartsParent)
      {
        this.SetParent();
        this.resetPartsParent = false;
      }
      if (!this.Enabled || !this.IsFunctional || ((double) this.ResourceSink.SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId) <= 0.990000009536743 || this.NeedsUpdate.HasFlag((Enum) MyEntityUpdateEnum.BEFORE_NEXT_FRAME)))
        return;
      if (this.State != MyLaserAntenna.StateEnum.idle)
        this.GetRotationAndElevation(this.m_targetCoords, ref this.m_needRotation, ref this.m_needElevation);
      this.RotationAndElevation(this.m_needRotation, this.m_needElevation);
      this.TryLaseTargetCoords();
    }

    public override void UpdateAfterSimulation10()
    {
      base.UpdateAfterSimulation10();
      if (!this.Enabled || !this.IsFunctional || (double) this.ResourceSink.SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId) <= 0.990000009536743)
        return;
      this.TryUpdateTargetCoords();
      this.ResourceSink.Update();
    }

    public override void UpdateAfterSimulation100()
    {
      base.UpdateAfterSimulation100();
      if (!this.Enabled || !this.IsFunctional || (double) this.ResourceSink.SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId) <= 0.990000009536743)
        return;
      this.Receiver.UpdateBroadcastersInRange();
      this.TryUpdateTargetCoords();
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.m_canLaseTargetCoords.Value = false;
      switch (this.State)
      {
        case MyLaserAntenna.StateEnum.rot_GPS:
          if (!this.m_rotationFinished)
            break;
          this.ShiftModeSync(MyLaserAntenna.StateEnum.search_GPS);
          break;
        case MyLaserAntenna.StateEnum.search_GPS:
          if (!this.m_rotationFinished)
          {
            this.ShiftModeSync(MyLaserAntenna.StateEnum.rot_GPS);
            break;
          }
          if (Sandbox.Game.Multiplayer.Sync.MultiplayerActive && !Sandbox.Game.Multiplayer.Sync.IsServer)
            break;
          MyLaserAntenna la1 = (MyLaserAntenna) null;
          double num1 = double.MaxValue;
          float maxValue = float.MaxValue;
          bool flag = false;
          MyLaserAntenna la2 = (MyLaserAntenna) null;
          foreach (MyLaserBroadcaster laserBroadcaster in MyAntennaSystem.Static.LaserAntennas.Values)
          {
            MyLaserAntenna realAntenna = laserBroadcaster.RealAntenna;
            if (realAntenna.Enabled && realAntenna.IsFunctional && ((double) realAntenna.ResourceSink.SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId) > 0.990000009536743 && !(realAntenna.m_permanentConnection.Value & flag)) && (realAntenna.Broadcaster.CanBeUsedByPlayer(this.OwnerId) && this.Broadcaster.CanBeUsedByPlayer(realAntenna.OwnerId) && realAntenna.EntityId != this.EntityId))
            {
              double num2 = realAntenna.Dist2To(new Vector3D?(this.m_targetCoords));
              if (num2 < 100.0)
              {
                if (realAntenna.m_permanentConnection.Value)
                {
                  flag = true;
                  la2 = laserBroadcaster.RealAntenna;
                }
                else
                {
                  if (realAntenna.State == MyLaserAntenna.StateEnum.idle)
                  {
                    la1 = realAntenna;
                    break;
                  }
                  if (num2 < (double) maxValue)
                  {
                    num1 = (double) maxValue;
                    la1 = realAntenna;
                  }
                }
              }
            }
          }
          if (la1 == null)
          {
            if (this.m_OnlyPermanentExists)
            {
              if (flag)
                break;
              this.m_OnlyPermanentExists = false;
              this.SetDetailedInfoDirty();
              this.RaisePropertiesChanged();
              break;
            }
            if (!flag || !this.IsInRange((MyEntity) la2) || !this.LosTests(la2))
              break;
            this.m_OnlyPermanentExists = true;
            this.SetDetailedInfoDirty();
            this.RaisePropertiesChanged();
            break;
          }
          if (!this.IsInRange((MyEntity) la1) || !this.LosTests(la1))
            break;
          this.ConnectToRec(la1.EntityId);
          break;
        case MyLaserAntenna.StateEnum.rot_Rec:
          if (!this.m_rotationFinished)
            break;
          this.ShiftModeSync(MyLaserAntenna.StateEnum.contact_Rec);
          break;
        case MyLaserAntenna.StateEnum.contact_Rec:
          if (!this.m_targetId.HasValue)
            break;
          if (!this.m_rotationFinished)
          {
            this.ShiftModeSync(MyLaserAntenna.StateEnum.rot_Rec);
            break;
          }
          MyLaserAntenna laserById1 = MyLaserAntenna.GetLaserById(this.m_targetId.Value);
          if (!Sandbox.Game.Multiplayer.Sync.IsServer || laserById1 == null || laserById1.State != MyLaserAntenna.StateEnum.contact_Rec && laserById1.State != MyLaserAntenna.StateEnum.connected && laserById1.State != MyLaserAntenna.StateEnum.rot_Rec)
            break;
          long? targetId1 = laserById1.m_targetId;
          long entityId1 = this.EntityId;
          if (!(targetId1.GetValueOrDefault() == entityId1 & targetId1.HasValue) || !laserById1.Enabled || (!laserById1.IsFunctional || (double) laserById1.ResourceSink.SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId) <= 0.990000009536743) || (!this.IsInRange((MyEntity) laserById1) || !laserById1.Broadcaster.CanBeUsedByPlayer(this.OwnerId) || (!this.Broadcaster.CanBeUsedByPlayer(laserById1.OwnerId) || laserById1.Dist2To(new Vector3D?(this.m_targetCoords)) > 100.0)) || !this.LosTests(laserById1))
            break;
          if (this.Dist2To(new Vector3D?(laserById1.m_targetCoords)) > 100.0 || !laserById1.m_rotationFinished)
          {
            this.SetupLaseTargetCoords();
            laserById1.m_targetCoords = this.HeadPos;
            laserById1.m_rotationFinished = false;
            break;
          }
          this.m_canLaseTargetCoords.Value = true;
          this.ShiftModeSync(MyLaserAntenna.StateEnum.connected);
          break;
        case MyLaserAntenna.StateEnum.connected:
          if (!Sandbox.Game.Multiplayer.Sync.IsServer)
            break;
          if (!this.m_rotationFinished)
            this.ShiftModeSync(MyLaserAntenna.StateEnum.rot_Rec);
          if (!this.m_targetId.HasValue)
            this.ShiftModeSync(MyLaserAntenna.StateEnum.contact_Rec);
          MyLaserAntenna laserById2 = MyLaserAntenna.GetLaserById(this.m_targetId.Value);
          if (laserById2 != null)
          {
            long? targetId2 = laserById2.m_targetId;
            long entityId2 = this.EntityId;
            if (targetId2.GetValueOrDefault() == entityId2 & targetId2.HasValue && laserById2.State == MyLaserAntenna.StateEnum.connected && (laserById2.Enabled && laserById2.IsFunctional) && ((double) laserById2.ResourceSink.SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId) > 0.990000009536743 && laserById2.m_rotationFinished && (this.IsInRange((MyEntity) laserById2) && laserById2.Broadcaster.CanBeUsedByPlayer(this.OwnerId))) && (this.Broadcaster.CanBeUsedByPlayer(laserById2.OwnerId) && this.LosTest(laserById2.HeadPos)))
            {
              this.m_targetCoords = laserById2.HeadPos;
              this.m_canLaseTargetCoords.Value = true;
              break;
            }
          }
          this.ShiftModeSync(MyLaserAntenna.StateEnum.contact_Rec);
          break;
      }
    }

    protected void SetupLaseTargetCoords()
    {
      this.m_canLaseTargetCoords.Value = false;
      if (!this.m_rotationFinished || !this.m_wasVisible || !this.m_targetId.HasValue)
        return;
      MyLaserAntenna laserById = MyLaserAntenna.GetLaserById(this.m_targetId.Value);
      if (laserById == null)
        return;
      long? targetId = laserById.m_targetId;
      long entityId = this.EntityId;
      if (!(targetId.GetValueOrDefault() == entityId & targetId.HasValue) || !laserById.Enabled || (!laserById.IsFunctional || (double) laserById.ResourceSink.SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId) <= 0.990000009536743) || (!this.IsInRange((MyEntity) laserById) || !laserById.Broadcaster.CanBeUsedByPlayer(this.OwnerId) || !this.Broadcaster.CanBeUsedByPlayer(laserById.OwnerId)))
        return;
      this.m_canLaseTargetCoords.Value = true;
    }

    protected void TryLaseTargetCoords()
    {
      if (!(bool) this.m_canLaseTargetCoords || !this.m_targetId.HasValue)
        return;
      MyLaserAntenna laserById = MyLaserAntenna.GetLaserById(this.m_targetId.Value);
      if (laserById == null)
        return;
      laserById.m_targetCoords = this.HeadPos;
    }

    protected void TryUpdateTargetCoords()
    {
      if (!this.m_targetId.HasValue)
        return;
      MyLaserAntenna laserById = MyLaserAntenna.GetLaserById(this.m_targetId.Value);
      if (laserById != null)
      {
        if (laserById.Enabled && laserById.IsFunctional && (double) this.ResourceSink.SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId) > 0.990000009536743)
        {
          long? targetId = laserById.m_targetId;
          long entityId = this.EntityId;
          if (!(targetId.GetValueOrDefault() == entityId & targetId.HasValue))
          {
            this.ShiftModeSync(MyLaserAntenna.StateEnum.idle);
            return;
          }
        }
        if (!MyAntennaSystem.Static.GetAllRelayedBroadcasters((MyDataReceiver) this.Receiver, this.OwnerId, false).Contains((MyDataBroadcaster) laserById.Broadcaster))
          return;
        this.m_targetCoords = laserById.HeadPos;
        if (this.m_lastKnownTargetName.CompareTo(laserById.CustomName) == 0)
          return;
        this.m_lastKnownTargetName.Clear().Append((object) laserById.CustomName);
        this.UpdateMyStateText();
      }
      else
      {
        MyLaserBroadcaster laserBroadcasterById = MyLaserAntenna.GetLaserBroadcasterById(this.m_targetId.Value);
        if (laserBroadcasterById == null)
          return;
        this.m_targetCoords = laserBroadcasterById.BroadcastPosition;
      }
    }

    private double Dist2To(Vector3D? here) => here.HasValue ? Vector3D.DistanceSquared(here.Value, this.HeadPos) : 3.40282346638529E+38;

    public bool IsInRange(MyEntity target)
    {
      if (target is MyLaserAntenna myLaserAntenna)
      {
        float num = Math.Min((float) myLaserAntenna.m_range, (float) this.m_range);
        return (double) num >= 100000000.0 || this.Dist2To(new Vector3D?(myLaserAntenna.HeadPos)) <= (double) num * (double) num;
      }
      return (double) (float) this.m_range >= 100000000.0 || this.Dist2To(new Vector3D?(target.PositionComp.GetPosition())) <= (double) (float) this.m_range * (double) (float) this.m_range;
    }

    public MyLaserAntenna GetOther() => this.State == MyLaserAntenna.StateEnum.connected ? MyLaserAntenna.GetLaserById(this.m_targetId.Value) : (MyLaserAntenna) null;

    protected override void OnEnabledChanged()
    {
      this.ResourceSink.Update();
      if (this.Enabled && this.State == MyLaserAntenna.StateEnum.connected)
        this.ShiftModeSync(MyLaserAntenna.StateEnum.rot_Rec);
      this.Receiver.UpdateBroadcastersInRange();
      base.OnEnabledChanged();
    }

    protected override void OnStopWorking()
    {
      this.UpdateEmissivity();
      base.OnStopWorking();
    }

    protected override void OnStartWorking()
    {
      this.UpdateEmissivity();
      base.OnStartWorking();
    }

    protected override bool CheckIsWorking() => base.CheckIsWorking() && this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);

    private void IsPoweredChanged() => MySandboxGame.Static.Invoke((Action) (() =>
    {
      if (this.Closed)
        return;
      this.UpdateIsWorking();
      if (this.State == MyLaserAntenna.StateEnum.connected && !this.IsWorking)
        this.ShiftModeSync(MyLaserAntenna.StateEnum.rot_Rec);
      if (this.Receiver != null)
        this.Receiver.Enabled = this.IsWorking;
      this.m_rotationInterval_ms = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
      this.UpdateEmissivity();
    }), "MyLaserAntenna::IsPoweredChanged");

    private void ComponentStack_IsFunctionalChanged()
    {
      this.ResourceSink.Update();
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
      this.UpdateEmissivity();
    }

    internal override void OnIntegrityChanged(
      float buildIntegrity,
      float integrity,
      bool setOwnership,
      long owner,
      MyOwnershipShareModeEnum sharing = MyOwnershipShareModeEnum.Faction)
    {
      base.OnIntegrityChanged(buildIntegrity, integrity, setOwnership, owner, sharing);
      this.m_termGpsCoords = new Vector3D?();
      this.m_termGpsName.Clear();
      this.ShiftModeSync(MyLaserAntenna.StateEnum.idle);
    }

    public void OnClosed() => MyAntennaSystem.Static.RemoveLaser(this.Broadcaster.AntennaEntityId);

    private float UpdatePowerInput()
    {
      if (!this.Enabled || !this.IsFunctional)
      {
        this.SetDetailedInfoDirty();
        this.RaisePropertiesChanged();
        return 0.0f;
      }
      float num1 = 0.0f;
      switch (this.State)
      {
        case MyLaserAntenna.StateEnum.idle:
          num1 = this.m_rotationFinished ? this.BlockDefinition.PowerInputIdle : this.BlockDefinition.PowerInputTurning;
          break;
        case MyLaserAntenna.StateEnum.rot_GPS:
        case MyLaserAntenna.StateEnum.rot_Rec:
          num1 = this.BlockDefinition.PowerInputTurning;
          break;
        case MyLaserAntenna.StateEnum.search_GPS:
        case MyLaserAntenna.StateEnum.contact_Rec:
        case MyLaserAntenna.StateEnum.connected:
          double powerInputLasing = (double) this.BlockDefinition.PowerInputLasing;
          double num2 = powerInputLasing / 2.0 / 200000.0;
          double num3 = powerInputLasing * 200000.0 - num2 * 200000.0 * 200000.0;
          double d = Math.Min(this.Dist2To(new Vector3D?(this.m_targetCoords)), (double) (float) this.m_range * (double) (float) this.m_range);
          num1 = d <= 40000000000.0 ? (float) (powerInputLasing * Math.Sqrt(d)) / 1000000f : (float) (d * num2 + num3) / 1000000f;
          break;
      }
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
      return num1;
    }

    public override bool SetEmissiveStateWorking() => false;

    public override bool SetEmissiveStateDamaged() => false;

    public override bool SetEmissiveStateDisabled() => false;

    private void UpdateEmissivity()
    {
      if (!this.InScene || this.m_base2 == null || this.m_base2.Render == null)
        return;
      Color emissivePartColor1 = Color.Red;
      if (!this.IsWorking)
      {
        MyEmissiveColorStateResult result;
        if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Disabled, out result))
          emissivePartColor1 = result.EmissiveColor;
        MyEntity.UpdateNamedEmissiveParts(this.m_base2.Render.RenderObjectIDs[0], "Emissive0", emissivePartColor1, 0.0f);
      }
      else
      {
        Color emissivePartColor2;
        switch (this.State)
        {
          case MyLaserAntenna.StateEnum.idle:
            emissivePartColor2 = Color.Green;
            MyEmissiveColorStateResult result1;
            if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Working, out result1))
            {
              emissivePartColor2 = result1.EmissiveColor;
              break;
            }
            break;
          case MyLaserAntenna.StateEnum.rot_GPS:
          case MyLaserAntenna.StateEnum.rot_Rec:
            emissivePartColor2 = Color.Yellow;
            MyEmissiveColorStateResult result2;
            if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Warning, out result2))
            {
              emissivePartColor2 = result2.EmissiveColor;
              break;
            }
            break;
          case MyLaserAntenna.StateEnum.connected:
            emissivePartColor2 = Color.SteelBlue;
            MyEmissiveColorStateResult result3;
            if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Alternative, out result3))
            {
              emissivePartColor2 = result3.EmissiveColor;
              break;
            }
            break;
          default:
            emissivePartColor2 = Color.GreenYellow;
            MyEmissiveColorStateResult result4;
            if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Working, out result4))
            {
              emissivePartColor2 = result4.EmissiveColor;
              break;
            }
            break;
        }
        MyEntity.UpdateNamedEmissiveParts(this.m_base2.Render.RenderObjectIDs[0], "Emissive0", emissivePartColor2, 1f);
      }
    }

    protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
    {
      base.UpdateDetailedInfo(detailedInfo);
      detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
      detailedInfo.Append(this.BlockDefinition.DisplayNameText);
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyProperties_CurrentInput));
      MyValueFormatter.AppendWorkInBestUnit(this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) ? this.ResourceSink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId) : 0.0f, detailedInfo);
      detailedInfo.Append("\n");
      if (!this.Enabled)
        return;
      switch (this.State)
      {
        case MyLaserAntenna.StateEnum.idle:
          detailedInfo.Append((object) MyTexts.Get(MySpaceTexts.LaserAntennaModeIdle));
          break;
        case MyLaserAntenna.StateEnum.rot_GPS:
          detailedInfo.Append((object) MyTexts.Get(MySpaceTexts.LaserAntennaModeRotGPS));
          break;
        case MyLaserAntenna.StateEnum.search_GPS:
          detailedInfo.Append((object) MyTexts.Get(MySpaceTexts.LaserAntennaModeSearchGPS));
          if (this.m_OnlyPermanentExists)
          {
            detailedInfo.Append("\n");
            detailedInfo.Append((object) MyTexts.Get(MySpaceTexts.LaserAntennaOnlyPerm));
            break;
          }
          break;
        case MyLaserAntenna.StateEnum.rot_Rec:
          detailedInfo.Append((object) MyTexts.Get(MySpaceTexts.LaserAntennaModeRotRec));
          detailedInfo.Append((object) this.m_lastKnownTargetName);
          break;
        case MyLaserAntenna.StateEnum.contact_Rec:
          detailedInfo.Append((object) MyTexts.Get(MySpaceTexts.LaserAntennaModeContactRec));
          detailedInfo.Append((object) this.m_lastKnownTargetName);
          break;
        case MyLaserAntenna.StateEnum.connected:
          detailedInfo.Append((object) MyTexts.Get(MySpaceTexts.LaserAntennaModeConnectedTo));
          detailedInfo.Append((object) this.m_lastKnownTargetName);
          break;
      }
      if (!this.m_outsideLimits)
        return;
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.LaserAntennaOutsideLimits));
    }

    protected void SetIdle()
    {
      this.ChangeModeSync(MyLaserAntenna.StateEnum.idle);
      MyLaserAntenna.receiversList.UpdateVisual();
    }

    protected void ConnectToId() => this.ConnectToRec(this.m_selectedEntityId.Value);

    protected void ConnectToGps(Vector3D? coords = null) => this.ChangeModeSync(MyLaserAntenna.StateEnum.rot_GPS, coords);

    internal void ChangeMode(MyLaserAntenna.StateEnum Mode, Vector3D? coords = null)
    {
      switch (Mode)
      {
        case MyLaserAntenna.StateEnum.idle:
          this.m_state = MyLaserAntenna.StateEnum.idle;
          this.IdleOther();
          break;
        case MyLaserAntenna.StateEnum.rot_GPS:
          this.m_state = MyLaserAntenna.StateEnum.idle;
          this.IdleOther();
          break;
      }
      this.DoChangeMode(Mode, coords);
      this.Receiver.UpdateBroadcastersInRange();
    }

    internal void DoChangeMode(MyLaserAntenna.StateEnum Mode, Vector3D? coords = null)
    {
      this.State = Mode;
      this.Broadcaster.RaiseChangeSuccessfullyContacting();
      this.m_OnlyPermanentExists = false;
      this.Receiver.UpdateBroadcastersInRange();
      if (MySession.Static.LocalCharacter != null)
        MySession.Static.LocalCharacter.RadioReceiver.UpdateBroadcastersInRange();
      MyLaserAntenna.receiversList.UpdateVisual();
      if (this.m_targetId.HasValue)
      {
        MyLaserAntenna laserById = MyLaserAntenna.GetLaserById(this.m_targetId.Value);
        if (laserById != null)
        {
          laserById.UpdateVisual();
          laserById.ResourceSink.Update();
        }
      }
      this.ResourceSink.Update();
      this.UpdateVisual();
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
      this.UpdateEmissivity();
      this.UpdateMyStateText();
      switch (Mode)
      {
        case MyLaserAntenna.StateEnum.idle:
          this.m_needRotation = 0.0f;
          this.m_needElevation = 0.0f;
          this.m_rotationInterval_ms = MySandboxGame.TotalGamePlayTimeInMilliseconds;
          this.m_lastKnownTargetName.Clear();
          if (!Sandbox.Game.Multiplayer.Sync.IsServer)
            break;
          this.m_permanentConnection.Value = false;
          break;
        case MyLaserAntenna.StateEnum.rot_GPS:
          if (coords.HasValue)
            this.m_termGpsCoords = coords;
          this.m_targetCoords = this.m_termGpsCoords.Value;
          this.m_lastKnownTargetName.Clear().Append((object) this.m_termGpsName).Append(" ").Append((object) this.m_termGpsCoords);
          if (!Sandbox.Game.Multiplayer.Sync.IsServer)
            break;
          this.m_permanentConnection.Value = false;
          break;
      }
    }

    protected bool IsInContact(MyLaserAntenna la) => la != null && this.Receiver.BroadcastersInRange.Contains((MyDataBroadcaster) la.Broadcaster);

    protected bool IdleOther()
    {
      if (this.m_targetId.HasValue)
      {
        MyLaserAntenna laserById = MyLaserAntenna.GetLaserById(this.m_targetId.Value);
        if (laserById == null || !laserById.Enabled || (!laserById.IsFunctional || (double) laserById.ResourceSink.SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId) <= 0.990000009536743))
          return false;
        if (laserById.State == MyLaserAntenna.StateEnum.idle)
          return true;
        long? targetId = laserById.m_targetId;
        long entityId = this.EntityId;
        if (targetId.GetValueOrDefault() == entityId & targetId.HasValue && this.IsInContact(laserById))
        {
          laserById.ChangeModeSync(MyLaserAntenna.StateEnum.idle);
          return true;
        }
      }
      return true;
    }

    internal bool ConnectTo(long DestId)
    {
      MyLaserAntenna laserById = MyLaserAntenna.GetLaserById(DestId);
      if (laserById == null || !laserById.Broadcaster.CanBeUsedByPlayer(this.OwnerId) || !this.Broadcaster.CanBeUsedByPlayer(laserById.OwnerId))
        return false;
      this.IdleOther();
      this.DoConnectTo(DestId);
      if (laserById != null)
      {
        long? targetId = laserById.m_targetId;
        long entityId = this.EntityId;
        if (!(targetId.GetValueOrDefault() == entityId & targetId.HasValue))
          laserById.ConnectToRec(this.EntityId);
      }
      return true;
    }

    internal void DoConnectTo(long DestId, Vector3D? targetCoords = null, string name = null)
    {
      this.State = MyLaserAntenna.StateEnum.rot_Rec;
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.m_permanentConnection.Value = false;
      this.m_targetId = new long?(DestId);
      this.Broadcaster.RaiseChangeSuccessfullyContacting();
      this.m_rotationInterval_ms = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      MyLaserAntenna laserById = MyLaserAntenna.GetLaserById(DestId);
      if (laserById != null)
      {
        this.m_targetCoords = laserById.HeadPos;
        this.m_lastKnownTargetName.Clear().Append((object) laserById.CustomName);
      }
      else if (targetCoords.HasValue && name != null)
      {
        this.m_targetCoords = targetCoords.Value;
        this.m_lastKnownTargetName.Clear().Append(name);
      }
      else
      {
        this.m_targetCoords = Vector3D.Zero;
        this.m_lastKnownTargetName.Clear().Append("???");
      }
      this.ResourceSink.Update();
      this.Receiver.UpdateBroadcastersInRange();
      MyLaserAntenna.UpdateVisuals();
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
      this.UpdateEmissivity();
      this.UpdateMyStateText();
    }

    internal bool DoSetIsPerm(bool isPerm)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || this.m_permanentConnection.Value == isPerm || (this.State != MyLaserAntenna.StateEnum.connected || !this.m_targetId.HasValue))
        return false;
      MyLaserAntenna laserById = MyLaserAntenna.GetLaserById(this.m_targetId.Value);
      if (laserById == null)
        return false;
      laserById.m_permanentConnection.Value = isPerm;
      this.m_permanentConnection.Value = isPerm;
      return true;
    }

    protected static MyLaserAntenna GetLaserById(long id) => MyLaserAntenna.GetLaserBroadcasterById(id)?.RealAntenna;

    protected static MyLaserBroadcaster GetLaserBroadcasterById(long id)
    {
      MyLaserBroadcaster laserBroadcaster = (MyLaserBroadcaster) null;
      MyAntennaSystem.Static.LaserAntennas.TryGetValue(id, out laserBroadcaster);
      return laserBroadcaster;
    }

    protected bool LosTests(MyLaserAntenna la)
    {
      this.m_wasVisible = true;
      if (!this.LosTest(la.HeadPos))
        la.m_wasVisible = false;
      if (this.m_wasVisible)
        this.m_wasVisible = la.LosTest(this.HeadPos);
      return this.m_wasVisible;
    }

    protected bool LosTest(Vector3D target)
    {
      if (!this.m_needLineOfSight)
        return true;
      target = (this.HeadPos + target) * 0.5;
      LineD lineD1 = new LineD(this.HeadPos, target);
      if (lineD1.Length > 25.0)
      {
        this.m_voxelHits.Clear();
        MyGamePruningStructure.GetVoxelMapsOverlappingRay(ref lineD1, this.m_voxelHits);
        foreach (MyLineSegmentOverlapResult<MyVoxelBase> voxelHit in this.m_voxelHits)
        {
          if (voxelHit.Element is MyPlanet element)
          {
            double? nullable = new BoundingSphereD(element.PositionComp.GetPosition(), (double) element.MaximumRadius).Intersects(new RayD(lineD1.From, lineD1.Direction));
            if (nullable.HasValue && lineD1.Length >= nullable.Value && element.RootVoxel.Storage.Intersect(ref lineD1))
            {
              this.m_wasVisible = false;
              return false;
            }
          }
        }
        LineD lineD2 = lineD1.Length > (double) MyLaserAntenna.m_Max_LosDist ? new LineD(lineD1.From + lineD1.Direction * 25.0, lineD1.From + lineD1.Direction * (double) MyLaserAntenna.m_Max_LosDist) : new LineD(lineD1.From + lineD1.Direction * 25.0, lineD1.To);
        this.m_entityHits.Clear();
        MyGamePruningStructure.GetTopmostEntitiesOverlappingRay(ref lineD2, this.m_entityHits);
        foreach (MyLineSegmentOverlapResult<MyEntity> entityHit in this.m_entityHits)
        {
          if (entityHit.Element is MyVoxelBase element)
          {
            if (!(element is MyPlanet) && element.RootVoxel.Storage.Intersect(ref lineD2))
            {
              this.m_wasVisible = false;
              return false;
            }
          }
          else if (entityHit.Element is MyCubeGrid element)
          {
            if (element.Physics != null)
            {
              Vector3I? nullable = element.RayCastBlocks(lineD1.To, lineD1.From);
              if (nullable.HasValue && element.GetCubeBlock(nullable.Value) != this.SlimBlock)
              {
                this.m_wasVisible = false;
                return false;
              }
            }
          }
          else
          {
            this.m_wasVisible = false;
            return false;
          }
        }
      }
      MyPhysics.CastRay(lineD1.From, lineD1.From + lineD1.Direction * Math.Min(25.0, lineD1.Length), this.m_hits, 9);
      foreach (MyPhysics.HitInfo hit in this.m_hits)
      {
        VRage.ModAPI.IMyEntity hitEntity = hit.HkHitInfo.GetHitEntity();
        if (hitEntity != this.CubeGrid)
        {
          this.m_wasVisible = false;
          return false;
        }
        MyCubeGrid myCubeGrid = hitEntity as MyCubeGrid;
        if (myCubeGrid.Physics != null)
        {
          Vector3I? nullable = myCubeGrid.RayCastBlocks(lineD1.To, lineD1.From);
          if (nullable.HasValue && myCubeGrid.GetCubeBlock(nullable.Value) != this.SlimBlock)
          {
            this.m_wasVisible = false;
            return false;
          }
        }
      }
      return true;
    }

    private Vector3 LookAt(Vector3D target)
    {
      MatrixD worldMatrix = this.GetWorldMatrix();
      Vector3D translation = worldMatrix.Translation;
      Vector3D cameraTarget = target;
      worldMatrix = this.GetWorldMatrix();
      Vector3D up = worldMatrix.Up;
      MatrixD matrix = MatrixD.Normalize(MatrixD.Invert(MatrixD.CreateLookAt(translation, cameraTarget, up))) * MatrixD.Invert(MatrixD.Normalize(this.InitializationMatrixWorld));
      return MyMath.QuaternionToEuler(Quaternion.CreateFromRotationMatrix(in matrix));
    }

    protected void ResetRotation()
    {
      this.m_rotation = 0.0f;
      this.m_elevation = 0.0f;
      this.ClampRotationAndElevation();
      this.m_rotationInterval_ms = MySandboxGame.TotalGamePlayTimeInMilliseconds;
    }

    public override void OnModelChange()
    {
      base.OnModelChange();
      if (this.IsBuilt)
      {
        this.m_base1 = (MyEntity) this.Subparts["LaserComTurret"];
        this.m_base2 = (MyEntity) this.m_base1.Subparts["LaserCom"];
      }
      else
      {
        this.m_base1 = (MyEntity) null;
        this.m_base2 = (MyEntity) null;
      }
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      this.UpdateEmissivity();
    }

    private MatrixD InitializationMatrixWorld => this.InitializationMatrix * this.Parent.WorldMatrix;

    protected void RotateModels()
    {
      this.ClampRotationAndElevation();
      if (this.m_base1 == null || this.m_base2 == null)
        return;
      Matrix result1;
      Matrix.CreateRotationY(this.m_rotation, out result1);
      result1.Translation = this.m_base1.PositionComp.LocalMatrixRef.Translation;
      Matrix matrix2 = this.PositionComp.LocalMatrixRef;
      Matrix result2;
      Matrix.Multiply(ref result1, ref matrix2, out result2);
      this.m_base1.PositionComp.SetLocalMatrix(ref result1, (object) this.m_base1.Physics, false, ref result2);
      Matrix result3;
      Matrix.CreateRotationX(this.m_elevation, out result3);
      result3.Translation = this.m_base2.PositionComp.LocalMatrixRef.Translation;
      Matrix result4;
      Matrix.Multiply(ref result3, ref result2, out result4);
      Matrix localMatrix = result4;
      localMatrix.Translation = this.m_base2.PositionComp.LocalMatrixRef.Translation;
      this.m_base2.PositionComp.SetLocalMatrix(ref localMatrix, (object) this.m_base2.Physics, true, ref result4);
    }

    protected void GetRotationAndElevation(
      Vector3D target,
      ref float needRotation,
      ref float needElevation)
    {
      Vector3 zero = Vector3.Zero;
      Vector3 vector3 = this.LookAt(target);
      needRotation = vector3.Y;
      needElevation = vector3.X;
    }

    public bool RotationAndElevation(float needRotation, float needElevation)
    {
      float num1 = this.BlockDefinition.RotationRate * (float) (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_rotationInterval_ms);
      float num2 = needRotation - this.m_rotation;
      if ((double) num2 > 3.14159297943115)
        num2 -= 6.283185f;
      else if ((double) num2 < -3.14159297943115)
        num2 += 6.283185f;
      float max1 = Math.Abs(num2);
      if ((double) max1 > 1.0 / 1000.0)
      {
        float num3 = MathHelper.Clamp(num1, float.Epsilon, max1);
        this.m_rotation += (double) num2 > 0.0 ? num3 : -num3;
      }
      else
        this.m_rotation = needRotation;
      if ((double) this.m_rotation > 3.14159297943115)
        this.m_rotation -= 6.283185f;
      else if ((double) this.m_rotation < -3.14159297943115)
        this.m_rotation += 6.283185f;
      float num4 = this.BlockDefinition.RotationRate * (float) (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_elevationInterval_ms);
      float num5 = needElevation - this.m_elevation;
      float max2 = Math.Abs(num5);
      if ((double) max2 > 1.0 / 1000.0)
      {
        float num3 = MathHelper.Clamp(num4, float.Epsilon, max2);
        this.m_elevation += (double) num5 > 0.0 ? num3 : -num3;
      }
      else
        this.m_elevation = needElevation;
      this.m_elevationInterval_ms = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      this.m_rotationInterval_ms = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      this.RotateModels();
      float num6 = Math.Abs(Math.Abs(needRotation) - Math.Abs(this.m_rotation));
      float num7 = Math.Abs(Math.Abs(needElevation) - Math.Abs(this.m_elevation));
      this.m_rotationFinished = (double) num6 <= 1.40129846432482E-45 && (double) num7 <= 1.40129846432482E-45;
      return this.m_rotationFinished;
    }

    private void PasteCoordinates(string coords)
    {
      if (!Sandbox.Game.Multiplayer.Sync.MultiplayerActive)
        this.DoPasteCoords(coords);
      else
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyLaserAntenna, string>(this, (Func<MyLaserAntenna, Action<string>>) (x => new Action<string>(x.PasteCoordinatesSuccess)), coords);
    }

    [Event(null, 1734)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void PasteCoordinatesSuccess(string coords) => this.DoPasteCoords(coords);

    private void ChangePerm(bool isPerm)
    {
      if (!Sandbox.Game.Multiplayer.Sync.MultiplayerActive)
        this.DoSetIsPerm(isPerm);
      else
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyLaserAntenna, bool>(this, (Func<MyLaserAntenna, Action<bool>>) (x => new Action<bool>(x.ChangePermRequest)), isPerm);
    }

    [Event(null, 1752)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void ChangePermRequest(bool isPerm) => this.DoSetIsPerm(isPerm);

    private void ChangeModeSync(MyLaserAntenna.StateEnum Mode, Vector3D? coords = null) => this.ChangeMode(Mode, true, coords);

    private void ShiftModeSync(MyLaserAntenna.StateEnum Mode) => this.ChangeMode(Mode, false);

    private void ChangeMode(MyLaserAntenna.StateEnum mode, bool uploadFromClient, Vector3D? coords = null)
    {
      if (!Sandbox.Game.Multiplayer.Sync.MultiplayerActive)
      {
        this.ChangeMode(mode);
      }
      else
      {
        if (!uploadFromClient && !Sandbox.Game.Multiplayer.Sync.IsServer)
          return;
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyLaserAntenna, MyLaserAntenna.StateEnum, Vector3D?>(this, (Func<MyLaserAntenna, Action<MyLaserAntenna.StateEnum, Vector3D?>>) (x => new Action<MyLaserAntenna.StateEnum, Vector3D?>(x.OnChangeModeRequest)), mode, coords);
      }
    }

    [Event(null, 1778)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnChangeModeRequest(MyLaserAntenna.StateEnum mode, Vector3D? coords) => this.ChangeMode(mode, coords);

    public void ConnectToRec(long TgtReceiver)
    {
      if (!Sandbox.Game.Multiplayer.Sync.MultiplayerActive)
        this.ConnectTo(TgtReceiver);
      else
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyLaserAntenna, long>(this, (Func<MyLaserAntenna, Action<long>>) (x => new Action<long>(x.OnConnectToRecRequest)), TgtReceiver);
    }

    [Event(null, 1796)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void OnConnectToRecRequest(long targetEntityId)
    {
      if (!this.ConnectTo(targetEntityId))
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyLaserAntenna, long, Vector3D, string>(this, (Func<MyLaserAntenna, Action<long, Vector3D, string>>) (x => new Action<long, Vector3D, string>(x.OnConnectToRecSuccess)), targetEntityId, this.m_targetCoords, this.m_lastKnownTargetName.ToString());
    }

    [Event(null, 1803)]
    [Reliable]
    [Broadcast]
    private void OnConnectToRecSuccess(long targetEntityId, Vector3D targetCoords, string name) => this.DoConnectTo(targetEntityId, new Vector3D?(targetCoords), name);

    Vector3D Sandbox.ModAPI.Ingame.IMyLaserAntenna.TargetCoords => this.m_targetCoords;

    void Sandbox.ModAPI.Ingame.IMyLaserAntenna.SetTargetCoords(string coords)
    {
      if (coords == null)
        return;
      this.PasteCoordinates(coords);
    }

    void Sandbox.ModAPI.Ingame.IMyLaserAntenna.Connect()
    {
      if (!this.CanConnectToGPS())
        return;
      this.ConnectToGps();
    }

    bool Sandbox.ModAPI.Ingame.IMyLaserAntenna.IsPermanent
    {
      get => this.m_permanentConnection.Value;
      set => this.ChangePerm(value);
    }

    bool Sandbox.ModAPI.Ingame.IMyLaserAntenna.RequireLoS => this.m_needLineOfSight;

    bool Sandbox.ModAPI.Ingame.IMyLaserAntenna.IsOutsideLimits => this.m_outsideLimits;

    MyLaserAntennaStatus Sandbox.ModAPI.Ingame.IMyLaserAntenna.Status
    {
      get
      {
        if (this.m_outsideLimits)
          return MyLaserAntennaStatus.OutOfRange;
        switch (this.State)
        {
          case MyLaserAntenna.StateEnum.idle:
            return MyLaserAntennaStatus.Idle;
          case MyLaserAntenna.StateEnum.rot_GPS:
            return MyLaserAntennaStatus.RotatingToTarget;
          case MyLaserAntenna.StateEnum.search_GPS:
            return MyLaserAntennaStatus.SearchingTargetForAntenna;
          case MyLaserAntenna.StateEnum.rot_Rec:
            return MyLaserAntennaStatus.RotatingToTarget;
          case MyLaserAntenna.StateEnum.contact_Rec:
            return MyLaserAntennaStatus.Connecting;
          case MyLaserAntenna.StateEnum.connected:
            return MyLaserAntennaStatus.Connected;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }

    Sandbox.ModAPI.IMyLaserAntenna Sandbox.ModAPI.IMyLaserAntenna.Other => (Sandbox.ModAPI.IMyLaserAntenna) this.GetOther();

    bool Sandbox.ModAPI.IMyLaserAntenna.IsInRange(Sandbox.ModAPI.IMyLaserAntenna target) => this.IsInRange((MyEntity) target);

    float Sandbox.ModAPI.Ingame.IMyLaserAntenna.Range
    {
      get => this.m_range.Value;
      set => this.m_range.Value = value;
    }

    public enum StateEnum : byte
    {
      idle,
      rot_GPS,
      search_GPS,
      rot_Rec,
      contact_Rec,
      connected,
    }

    protected sealed class PasteCoordinatesSuccess\u003C\u003ESystem_String : ICallSite<MyLaserAntenna, string, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyLaserAntenna @this,
        in string coords,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.PasteCoordinatesSuccess(coords);
      }
    }

    protected sealed class ChangePermRequest\u003C\u003ESystem_Boolean : ICallSite<MyLaserAntenna, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyLaserAntenna @this,
        in bool isPerm,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ChangePermRequest(isPerm);
      }
    }

    protected sealed class OnChangeModeRequest\u003C\u003ESandbox_Game_Entities_Cube_MyLaserAntenna\u003C\u003EStateEnum\u0023System_Nullable`1\u003CVRageMath_Vector3D\u003E : ICallSite<MyLaserAntenna, MyLaserAntenna.StateEnum, Vector3D?, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyLaserAntenna @this,
        in MyLaserAntenna.StateEnum mode,
        in Vector3D? coords,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeModeRequest(mode, coords);
      }
    }

    protected sealed class OnConnectToRecRequest\u003C\u003ESystem_Int64 : ICallSite<MyLaserAntenna, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyLaserAntenna @this,
        in long targetEntityId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnConnectToRecRequest(targetEntityId);
      }
    }

    protected sealed class OnConnectToRecSuccess\u003C\u003ESystem_Int64\u0023VRageMath_Vector3D\u0023System_String : ICallSite<MyLaserAntenna, long, Vector3D, string, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyLaserAntenna @this,
        in long targetEntityId,
        in Vector3D targetCoords,
        in string name,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnConnectToRecSuccess(targetEntityId, targetCoords, name);
      }
    }

    protected class m_range\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyLaserAntenna) obj0).m_range = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_permanentConnection\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.FromServer>(obj1, obj2));
        ((MyLaserAntenna) obj0).m_permanentConnection = (VRage.Sync.Sync<bool, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_canLaseTargetCoords\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.FromServer>(obj1, obj2));
        ((MyLaserAntenna) obj0).m_canLaseTargetCoords = (VRage.Sync.Sync<bool, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_Cube_MyLaserAntenna\u003C\u003EActor : IActivator, IActivator<MyLaserAntenna>
    {
      object IActivator.CreateInstance() => (object) new MyLaserAntenna();

      MyLaserAntenna IActivator<MyLaserAntenna>.CreateInstance() => new MyLaserAntenna();
    }
  }
}
