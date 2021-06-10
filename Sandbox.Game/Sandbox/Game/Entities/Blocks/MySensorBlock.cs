// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MySensorBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_SensorBlock))]
  [MyTerminalInterface(new System.Type[] {typeof (Sandbox.ModAPI.IMySensorBlock), typeof (Sandbox.ModAPI.Ingame.IMySensorBlock)})]
  public class MySensorBlock : MyFunctionalBlock, Sandbox.ModAPI.IMySensorBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMySensorBlock, IMyGizmoDrawableObject
  {
    private const float MIN_RANGE = 0.1f;
    private Color m_gizmoColor;
    private const float m_maxGizmoDrawDistance = 400f;
    private BoundingBox m_gizmoBoundingBox;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_playProximitySound;
    private bool m_enablePlaySoundEvent;
    private readonly MyConcurrentHashSet<MyDetectedEntityInfo> m_detectedEntities = new MyConcurrentHashSet<MyDetectedEntityInfo>();
    private VRage.Sync.Sync<bool, SyncDirection.BothWays> m_active;
    private List<ToolbarItem> m_items;
    private static readonly List<MyEntity> m_potentialPenetrations = new List<MyEntity>();
    protected HkShape m_fieldShape;
    private bool m_recreateField;
    private readonly VRage.Sync.Sync<Vector3, SyncDirection.BothWays> m_fieldMin;
    private readonly VRage.Sync.Sync<Vector3, SyncDirection.BothWays> m_fieldMax;
    private readonly VRage.Sync.Sync<MySensorFilterFlags, SyncDirection.BothWays> m_flags;
    private static List<MyToolbar> m_openedToolbars;
    private static bool m_shouldSetOtherToolbars;
    private bool m_syncing;

    private MySensorBlockDefinition BlockDefinition => (MySensorBlockDefinition) base.BlockDefinition;

    public bool IsActive
    {
      get => (bool) this.m_active;
      set => this.m_active.Value = value;
    }

    public MyEntity LastDetectedEntity { get; private set; }

    public MyToolbar Toolbar { get; set; }

    public Vector3 FieldMin
    {
      get => (Vector3) this.m_fieldMin;
      set
      {
        this.m_fieldMin.Value = value;
        float inputWhenEnabled = this.CalculateRequiredPowerInputWhenEnabled();
        this.ResourceSink.SetMaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId, inputWhenEnabled);
        this.ResourceSink.SetRequiredInputByType(MyResourceDistributorComponent.ElectricityId, inputWhenEnabled);
        this.SetDetailedInfoDirty();
      }
    }

    public Vector3 FieldMax
    {
      get => (Vector3) this.m_fieldMax;
      set
      {
        this.m_fieldMax.Value = value;
        float inputWhenEnabled = this.CalculateRequiredPowerInputWhenEnabled();
        this.ResourceSink.SetMaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId, inputWhenEnabled);
        this.ResourceSink.SetRequiredInputByType(MyResourceDistributorComponent.ElectricityId, inputWhenEnabled);
        this.SetDetailedInfoDirty();
      }
    }

    public float MaxRange => this.BlockDefinition.MaxRange;

    public MySensorFilterFlags Filters
    {
      get => (MySensorFilterFlags) this.m_flags;
      set => this.m_flags.Value = value;
    }

    public bool PlayProximitySound
    {
      get => (bool) this.m_playProximitySound;
      set => this.m_playProximitySound.Value = value;
    }

    public bool DetectPlayers
    {
      get => (uint) (this.Filters & MySensorFilterFlags.Players) > 0U;
      set
      {
        if (value)
          this.Filters |= MySensorFilterFlags.Players;
        else
          this.Filters &= ~MySensorFilterFlags.Players;
      }
    }

    public bool DetectFloatingObjects
    {
      get => (uint) (this.Filters & MySensorFilterFlags.FloatingObjects) > 0U;
      set
      {
        if (value)
          this.Filters |= MySensorFilterFlags.FloatingObjects;
        else
          this.Filters &= ~MySensorFilterFlags.FloatingObjects;
      }
    }

    public bool DetectSmallShips
    {
      get => (uint) (this.Filters & MySensorFilterFlags.SmallShips) > 0U;
      set
      {
        if (value)
          this.Filters |= MySensorFilterFlags.SmallShips;
        else
          this.Filters &= ~MySensorFilterFlags.SmallShips;
      }
    }

    public bool DetectLargeShips
    {
      get => (uint) (this.Filters & MySensorFilterFlags.LargeShips) > 0U;
      set
      {
        if (value)
          this.Filters |= MySensorFilterFlags.LargeShips;
        else
          this.Filters &= ~MySensorFilterFlags.LargeShips;
      }
    }

    public bool DetectStations
    {
      get => (uint) (this.Filters & MySensorFilterFlags.Stations) > 0U;
      set
      {
        if (value)
          this.Filters |= MySensorFilterFlags.Stations;
        else
          this.Filters &= ~MySensorFilterFlags.Stations;
      }
    }

    public bool DetectSubgrids
    {
      get => (uint) (this.Filters & MySensorFilterFlags.Subgrids) > 0U;
      set
      {
        if (value)
          this.Filters |= MySensorFilterFlags.Subgrids;
        else
          this.Filters &= ~MySensorFilterFlags.Subgrids;
      }
    }

    public bool DetectAsteroids
    {
      get => (uint) (this.Filters & MySensorFilterFlags.Asteroids) > 0U;
      set
      {
        if (value)
          this.Filters |= MySensorFilterFlags.Asteroids;
        else
          this.Filters &= ~MySensorFilterFlags.Asteroids;
      }
    }

    public bool DetectOwner
    {
      get => (uint) (this.Filters & MySensorFilterFlags.Owner) > 0U;
      set
      {
        if (value)
          this.Filters |= MySensorFilterFlags.Owner;
        else
          this.Filters &= ~MySensorFilterFlags.Owner;
      }
    }

    public bool DetectFriendly
    {
      get => (uint) (this.Filters & MySensorFilterFlags.Friendly) > 0U;
      set
      {
        if (value)
          this.Filters |= MySensorFilterFlags.Friendly;
        else
          this.Filters &= ~MySensorFilterFlags.Friendly;
      }
    }

    public bool DetectNeutral
    {
      get => (uint) (this.Filters & MySensorFilterFlags.Neutral) > 0U;
      set
      {
        if (value)
          this.Filters |= MySensorFilterFlags.Neutral;
        else
          this.Filters &= ~MySensorFilterFlags.Neutral;
      }
    }

    public bool DetectEnemy
    {
      get => (uint) (this.Filters & MySensorFilterFlags.Enemy) > 0U;
      set
      {
        if (value)
          this.Filters |= MySensorFilterFlags.Enemy;
        else
          this.Filters &= ~MySensorFilterFlags.Enemy;
      }
    }

    public float LeftExtend
    {
      get => -this.m_fieldMin.Value.X;
      set
      {
        Vector3 fieldMin = this.FieldMin;
        if ((double) fieldMin.X == -(double) value)
          return;
        fieldMin.X = -value;
        this.FieldMin = fieldMin;
      }
    }

    public float RightExtend
    {
      get => this.m_fieldMax.Value.X;
      set
      {
        Vector3 fieldMax = this.FieldMax;
        if ((double) fieldMax.X == (double) value)
          return;
        fieldMax.X = value;
        this.FieldMax = fieldMax;
      }
    }

    public float BottomExtend
    {
      get => -this.m_fieldMin.Value.Y;
      set
      {
        Vector3 fieldMin = this.FieldMin;
        if ((double) fieldMin.Y == -(double) value)
          return;
        fieldMin.Y = -value;
        this.FieldMin = fieldMin;
      }
    }

    public float TopExtend
    {
      get => this.m_fieldMax.Value.Y;
      set
      {
        Vector3 fieldMax = this.FieldMax;
        if ((double) fieldMax.Y == (double) value)
          return;
        fieldMax.Y = value;
        this.FieldMax = fieldMax;
      }
    }

    public float FrontExtend
    {
      get => -this.m_fieldMin.Value.Z;
      set
      {
        Vector3 fieldMin = this.FieldMin;
        if ((double) fieldMin.Z == -(double) value)
          return;
        fieldMin.Z = -value;
        this.FieldMin = fieldMin;
      }
    }

    public float BackExtend
    {
      get => this.m_fieldMax.Value.Z;
      set
      {
        Vector3 fieldMax = this.FieldMax;
        if ((double) fieldMax.Z == (double) value)
          return;
        fieldMax.Z = value;
        this.FieldMax = fieldMax;
      }
    }

    public MySensorBlock()
    {
      this.CreateTerminalControls();
      this.m_active.ValueChanged += (Action<SyncBase>) (x => this.IsActiveChanged());
      this.m_fieldMax.ValueChanged += (Action<SyncBase>) (x => this.UpdateField());
      this.m_fieldMin.ValueChanged += (Action<SyncBase>) (x => this.UpdateField());
    }

    protected new void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MySensorBlock>())
        return;
      base.CreateTerminalControls();
      MySensorBlock.m_openedToolbars = new List<MyToolbar>();
      MyTerminalControlFactory.AddControl<MySensorBlock>((MyTerminalControl<MySensorBlock>) new MyTerminalControlButton<MySensorBlock>("Open Toolbar", MySpaceTexts.BlockPropertyTitle_SensorToolbarOpen, MySpaceTexts.BlockPropertyDescription_SensorToolbarOpen, (Action<MySensorBlock>) (self =>
      {
        MySensorBlock.m_openedToolbars.Add(self.Toolbar);
        if (MyGuiScreenToolbarConfigBase.Static != null)
          return;
        MySensorBlock.m_shouldSetOtherToolbars = true;
        MyToolbarComponent.CurrentToolbar = self.Toolbar;
        MyGuiScreenBase screen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.ToolbarConfigScreen, (object) 0, (object) self, null);
        MyToolbarComponent.AutoUpdate = false;
        screen.Closed += (MyGuiScreenBase.ScreenHandler) ((source, isUnloading) =>
        {
          MyToolbarComponent.AutoUpdate = true;
          MySensorBlock.m_openedToolbars.Clear();
        });
        MyGuiSandbox.AddScreen(screen);
      })));
      MyTerminalControlSlider<MySensorBlock> slider1 = new MyTerminalControlSlider<MySensorBlock>("Left", MySpaceTexts.BlockPropertyTitle_SensorFieldWidthMin, MySpaceTexts.BlockPropertyDescription_SensorFieldLeft);
      slider1.SetLimits((MyTerminalValueControl<MySensorBlock, float>.GetterDelegate) (block => 0.1f), (MyTerminalValueControl<MySensorBlock, float>.GetterDelegate) (block => block.MaxRange));
      slider1.DefaultValue = new float?(5f);
      slider1.Getter = (MyTerminalValueControl<MySensorBlock, float>.GetterDelegate) (x => x.LeftExtend);
      slider1.Setter = (MyTerminalValueControl<MySensorBlock, float>.SetterDelegate) ((x, v) => x.LeftExtend = v);
      slider1.Writer = (MyTerminalControl<MySensorBlock>.WriterDelegate) ((x, result) => result.AppendFormatedDecimal("", x.LeftExtend, 1, " m"));
      slider1.EnableActions<MySensorBlock>();
      MyTerminalControlFactory.AddControl<MySensorBlock>((MyTerminalControl<MySensorBlock>) slider1);
      MyTerminalControlSlider<MySensorBlock> slider2 = new MyTerminalControlSlider<MySensorBlock>("Right", MySpaceTexts.BlockPropertyTitle_SensorFieldWidthMax, MySpaceTexts.BlockPropertyDescription_SensorFieldRight);
      slider2.SetLimits((MyTerminalValueControl<MySensorBlock, float>.GetterDelegate) (block => 0.1f), (MyTerminalValueControl<MySensorBlock, float>.GetterDelegate) (block => block.MaxRange));
      slider2.DefaultValue = new float?(5f);
      slider2.Getter = (MyTerminalValueControl<MySensorBlock, float>.GetterDelegate) (x => x.RightExtend);
      slider2.Setter = (MyTerminalValueControl<MySensorBlock, float>.SetterDelegate) ((x, v) => x.RightExtend = v);
      slider2.Writer = (MyTerminalControl<MySensorBlock>.WriterDelegate) ((x, result) => result.AppendFormatedDecimal("", x.RightExtend, 1, " m"));
      slider2.EnableActions<MySensorBlock>();
      MyTerminalControlFactory.AddControl<MySensorBlock>((MyTerminalControl<MySensorBlock>) slider2);
      MyTerminalControlSlider<MySensorBlock> slider3 = new MyTerminalControlSlider<MySensorBlock>("Bottom", MySpaceTexts.BlockPropertyTitle_SensorFieldHeightMin, MySpaceTexts.BlockPropertyDescription_SensorFieldBottom);
      slider3.SetLimits((MyTerminalValueControl<MySensorBlock, float>.GetterDelegate) (block => 0.1f), (MyTerminalValueControl<MySensorBlock, float>.GetterDelegate) (block => block.MaxRange));
      slider3.DefaultValue = new float?(5f);
      slider3.Getter = (MyTerminalValueControl<MySensorBlock, float>.GetterDelegate) (x => x.BottomExtend);
      slider3.Setter = (MyTerminalValueControl<MySensorBlock, float>.SetterDelegate) ((x, v) => x.BottomExtend = v);
      slider3.Writer = (MyTerminalControl<MySensorBlock>.WriterDelegate) ((x, result) => result.AppendFormatedDecimal("", x.BottomExtend, 1, " m"));
      slider3.EnableActions<MySensorBlock>();
      MyTerminalControlFactory.AddControl<MySensorBlock>((MyTerminalControl<MySensorBlock>) slider3);
      MyTerminalControlSlider<MySensorBlock> slider4 = new MyTerminalControlSlider<MySensorBlock>("Top", MySpaceTexts.BlockPropertyTitle_SensorFieldHeightMax, MySpaceTexts.BlockPropertyDescription_SensorFieldTop);
      slider4.SetLimits((MyTerminalValueControl<MySensorBlock, float>.GetterDelegate) (block => 0.1f), (MyTerminalValueControl<MySensorBlock, float>.GetterDelegate) (block => block.MaxRange));
      slider4.DefaultValue = new float?(5f);
      slider4.Getter = (MyTerminalValueControl<MySensorBlock, float>.GetterDelegate) (x => x.TopExtend);
      slider4.Setter = (MyTerminalValueControl<MySensorBlock, float>.SetterDelegate) ((x, v) => x.TopExtend = v);
      slider4.Writer = (MyTerminalControl<MySensorBlock>.WriterDelegate) ((x, result) => result.AppendFormatedDecimal("", x.TopExtend, 1, " m"));
      slider4.EnableActions<MySensorBlock>();
      MyTerminalControlFactory.AddControl<MySensorBlock>((MyTerminalControl<MySensorBlock>) slider4);
      MyTerminalControlSlider<MySensorBlock> slider5 = new MyTerminalControlSlider<MySensorBlock>("Back", MySpaceTexts.BlockPropertyTitle_SensorFieldDepthMax, MySpaceTexts.BlockPropertyDescription_SensorFieldBack);
      slider5.SetLimits((MyTerminalValueControl<MySensorBlock, float>.GetterDelegate) (block => 0.1f), (MyTerminalValueControl<MySensorBlock, float>.GetterDelegate) (block => block.MaxRange));
      slider5.DefaultValue = new float?(5f);
      slider5.Getter = (MyTerminalValueControl<MySensorBlock, float>.GetterDelegate) (x => x.BackExtend);
      slider5.Setter = (MyTerminalValueControl<MySensorBlock, float>.SetterDelegate) ((x, v) => x.BackExtend = v);
      slider5.Writer = (MyTerminalControl<MySensorBlock>.WriterDelegate) ((x, result) => result.AppendFormatedDecimal("", x.BackExtend, 1, " m"));
      slider5.EnableActions<MySensorBlock>();
      MyTerminalControlFactory.AddControl<MySensorBlock>((MyTerminalControl<MySensorBlock>) slider5);
      MyTerminalControlSlider<MySensorBlock> slider6 = new MyTerminalControlSlider<MySensorBlock>("Front", MySpaceTexts.BlockPropertyTitle_SensorFieldDepthMin, MySpaceTexts.BlockPropertyDescription_SensorFieldFront);
      slider6.SetLimits((MyTerminalValueControl<MySensorBlock, float>.GetterDelegate) (block => 0.1f), (MyTerminalValueControl<MySensorBlock, float>.GetterDelegate) (block => block.MaxRange));
      slider6.DefaultValue = new float?(5f);
      slider6.Getter = (MyTerminalValueControl<MySensorBlock, float>.GetterDelegate) (x => x.FrontExtend);
      slider6.Setter = (MyTerminalValueControl<MySensorBlock, float>.SetterDelegate) ((x, v) => x.FrontExtend = v);
      slider6.Writer = (MyTerminalControl<MySensorBlock>.WriterDelegate) ((x, result) => result.AppendFormatedDecimal("", x.FrontExtend, 1, " m"));
      slider6.EnableActions<MySensorBlock>();
      MyTerminalControlFactory.AddControl<MySensorBlock>((MyTerminalControl<MySensorBlock>) slider6);
      MyTerminalControlFactory.AddControl<MySensorBlock>((MyTerminalControl<MySensorBlock>) new MyTerminalControlSeparator<MySensorBlock>());
      MyTerminalControlOnOffSwitch<MySensorBlock> controlOnOffSwitch = new MyTerminalControlOnOffSwitch<MySensorBlock>("Audible Proximity Alert", MySpaceTexts.BlockPropertyTitle_SensorPlaySound, MySpaceTexts.BlockPropertyTitle_SensorPlaySound);
      controlOnOffSwitch.Getter = (MyTerminalValueControl<MySensorBlock, bool>.GetterDelegate) (x => x.PlayProximitySound);
      controlOnOffSwitch.Setter = (MyTerminalValueControl<MySensorBlock, bool>.SetterDelegate) ((x, v) => x.PlayProximitySound = v);
      MyTerminalControlFactory.AddControl<MySensorBlock>((MyTerminalControl<MySensorBlock>) controlOnOffSwitch);
      MyTerminalControlOnOffSwitch<MySensorBlock> onOff1 = new MyTerminalControlOnOffSwitch<MySensorBlock>("Detect Players", MySpaceTexts.BlockPropertyTitle_SensorDetectPlayers, MySpaceTexts.BlockPropertyTitle_SensorDetectPlayers);
      onOff1.Getter = (MyTerminalValueControl<MySensorBlock, bool>.GetterDelegate) (x => x.DetectPlayers);
      onOff1.Setter = (MyTerminalValueControl<MySensorBlock, bool>.SetterDelegate) ((x, v) => x.DetectPlayers = v);
      onOff1.EnableToggleAction<MySensorBlock>(MyTerminalActionIcons.CHARACTER_TOGGLE);
      onOff1.EnableOnOffActions<MySensorBlock>(MyTerminalActionIcons.CHARACTER_ON, MyTerminalActionIcons.CHARACTER_OFF);
      MyTerminalControlFactory.AddControl<MySensorBlock>((MyTerminalControl<MySensorBlock>) onOff1);
      MyTerminalControlOnOffSwitch<MySensorBlock> onOff2 = new MyTerminalControlOnOffSwitch<MySensorBlock>("Detect Floating Objects", MySpaceTexts.BlockPropertyTitle_SensorDetectFloatingObjects, MySpaceTexts.BlockPropertyTitle_SensorDetectFloatingObjects);
      onOff2.Getter = (MyTerminalValueControl<MySensorBlock, bool>.GetterDelegate) (x => x.DetectFloatingObjects);
      onOff2.Setter = (MyTerminalValueControl<MySensorBlock, bool>.SetterDelegate) ((x, v) => x.DetectFloatingObjects = v);
      onOff2.EnableToggleAction<MySensorBlock>(MyTerminalActionIcons.MOVING_OBJECT_TOGGLE);
      onOff2.EnableOnOffActions<MySensorBlock>(MyTerminalActionIcons.MOVING_OBJECT_ON, MyTerminalActionIcons.MOVING_OBJECT_OFF);
      MyTerminalControlFactory.AddControl<MySensorBlock>((MyTerminalControl<MySensorBlock>) onOff2);
      MyTerminalControlOnOffSwitch<MySensorBlock> onOff3 = new MyTerminalControlOnOffSwitch<MySensorBlock>("Detect Small Ships", MySpaceTexts.BlockPropertyTitle_SensorDetectSmallShips, MySpaceTexts.BlockPropertyTitle_SensorDetectSmallShips);
      onOff3.Getter = (MyTerminalValueControl<MySensorBlock, bool>.GetterDelegate) (x => x.DetectSmallShips);
      onOff3.Setter = (MyTerminalValueControl<MySensorBlock, bool>.SetterDelegate) ((x, v) => x.DetectSmallShips = v);
      onOff3.EnableToggleAction<MySensorBlock>(MyTerminalActionIcons.SMALLSHIP_TOGGLE);
      onOff3.EnableOnOffActions<MySensorBlock>(MyTerminalActionIcons.SMALLSHIP_ON, MyTerminalActionIcons.SMALLSHIP_OFF);
      MyTerminalControlFactory.AddControl<MySensorBlock>((MyTerminalControl<MySensorBlock>) onOff3);
      MyTerminalControlOnOffSwitch<MySensorBlock> onOff4 = new MyTerminalControlOnOffSwitch<MySensorBlock>("Detect Large Ships", MySpaceTexts.BlockPropertyTitle_SensorDetectLargeShips, MySpaceTexts.BlockPropertyTitle_SensorDetectLargeShips);
      onOff4.Getter = (MyTerminalValueControl<MySensorBlock, bool>.GetterDelegate) (x => x.DetectLargeShips);
      onOff4.Setter = (MyTerminalValueControl<MySensorBlock, bool>.SetterDelegate) ((x, v) => x.DetectLargeShips = v);
      onOff4.EnableToggleAction<MySensorBlock>(MyTerminalActionIcons.LARGESHIP_TOGGLE);
      onOff4.EnableOnOffActions<MySensorBlock>(MyTerminalActionIcons.LARGESHIP_ON, MyTerminalActionIcons.LARGESHIP_OFF);
      MyTerminalControlFactory.AddControl<MySensorBlock>((MyTerminalControl<MySensorBlock>) onOff4);
      MyTerminalControlOnOffSwitch<MySensorBlock> onOff5 = new MyTerminalControlOnOffSwitch<MySensorBlock>("Detect Stations", MySpaceTexts.BlockPropertyTitle_SensorDetectStations, MySpaceTexts.BlockPropertyTitle_SensorDetectStations);
      onOff5.Getter = (MyTerminalValueControl<MySensorBlock, bool>.GetterDelegate) (x => x.DetectStations);
      onOff5.Setter = (MyTerminalValueControl<MySensorBlock, bool>.SetterDelegate) ((x, v) => x.DetectStations = v);
      onOff5.EnableToggleAction<MySensorBlock>(MyTerminalActionIcons.STATION_TOGGLE);
      onOff5.EnableOnOffActions<MySensorBlock>(MyTerminalActionIcons.STATION_ON, MyTerminalActionIcons.STATION_OFF);
      MyTerminalControlFactory.AddControl<MySensorBlock>((MyTerminalControl<MySensorBlock>) onOff5);
      MyTerminalControlOnOffSwitch<MySensorBlock> onOff6 = new MyTerminalControlOnOffSwitch<MySensorBlock>("Detect Subgrids", MySpaceTexts.BlockPropertyTitle_SensorDetectSubgrids, MySpaceTexts.BlockPropertyTitle_SensorDetectSubgrids);
      onOff6.Getter = (MyTerminalValueControl<MySensorBlock, bool>.GetterDelegate) (x => x.DetectSubgrids);
      onOff6.Setter = (MyTerminalValueControl<MySensorBlock, bool>.SetterDelegate) ((x, v) => x.DetectSubgrids = v);
      onOff6.EnableToggleAction<MySensorBlock>(MyTerminalActionIcons.SUBGRID_TOGGLE);
      onOff6.EnableOnOffActions<MySensorBlock>(MyTerminalActionIcons.SUBGRID_ON, MyTerminalActionIcons.SUBGRID_OFF);
      MyTerminalControlFactory.AddControl<MySensorBlock>((MyTerminalControl<MySensorBlock>) onOff6);
      MyTerminalControlOnOffSwitch<MySensorBlock> onOff7 = new MyTerminalControlOnOffSwitch<MySensorBlock>("Detect Asteroids", MySpaceTexts.BlockPropertyTitle_SensorDetectAsteroids, MySpaceTexts.BlockPropertyTitle_SensorDetectAsteroids);
      onOff7.Getter = (MyTerminalValueControl<MySensorBlock, bool>.GetterDelegate) (x => x.DetectAsteroids);
      onOff7.Setter = (MyTerminalValueControl<MySensorBlock, bool>.SetterDelegate) ((x, v) => x.DetectAsteroids = v);
      onOff7.EnableToggleAction<MySensorBlock>();
      onOff7.EnableOnOffActions<MySensorBlock>();
      MyTerminalControlFactory.AddControl<MySensorBlock>((MyTerminalControl<MySensorBlock>) onOff7);
      MyTerminalControlFactory.AddControl<MySensorBlock>((MyTerminalControl<MySensorBlock>) new MyTerminalControlSeparator<MySensorBlock>());
      MyTerminalControlOnOffSwitch<MySensorBlock> onOff8 = new MyTerminalControlOnOffSwitch<MySensorBlock>("Detect Owner", MySpaceTexts.BlockPropertyTitle_SensorDetectOwner, MySpaceTexts.BlockPropertyTitle_SensorDetectOwner);
      onOff8.Getter = (MyTerminalValueControl<MySensorBlock, bool>.GetterDelegate) (x => x.DetectOwner);
      onOff8.Setter = (MyTerminalValueControl<MySensorBlock, bool>.SetterDelegate) ((x, v) => x.DetectOwner = v);
      onOff8.EnableToggleAction<MySensorBlock>();
      onOff8.EnableOnOffActions<MySensorBlock>();
      MyTerminalControlFactory.AddControl<MySensorBlock>((MyTerminalControl<MySensorBlock>) onOff8);
      MyTerminalControlOnOffSwitch<MySensorBlock> onOff9 = new MyTerminalControlOnOffSwitch<MySensorBlock>("Detect Friendly", MySpaceTexts.BlockPropertyTitle_SensorDetectFriendly, MySpaceTexts.BlockPropertyTitle_SensorDetectFriendly);
      onOff9.Getter = (MyTerminalValueControl<MySensorBlock, bool>.GetterDelegate) (x => x.DetectFriendly);
      onOff9.Setter = (MyTerminalValueControl<MySensorBlock, bool>.SetterDelegate) ((x, v) => x.DetectFriendly = v);
      onOff9.EnableToggleAction<MySensorBlock>();
      onOff9.EnableOnOffActions<MySensorBlock>();
      MyTerminalControlFactory.AddControl<MySensorBlock>((MyTerminalControl<MySensorBlock>) onOff9);
      MyTerminalControlOnOffSwitch<MySensorBlock> onOff10 = new MyTerminalControlOnOffSwitch<MySensorBlock>("Detect Neutral", MySpaceTexts.BlockPropertyTitle_SensorDetectNeutral, MySpaceTexts.BlockPropertyTitle_SensorDetectNeutral);
      onOff10.Getter = (MyTerminalValueControl<MySensorBlock, bool>.GetterDelegate) (x => x.DetectNeutral);
      onOff10.Setter = (MyTerminalValueControl<MySensorBlock, bool>.SetterDelegate) ((x, v) => x.DetectNeutral = v);
      onOff10.EnableToggleAction<MySensorBlock>();
      onOff10.EnableOnOffActions<MySensorBlock>();
      MyTerminalControlFactory.AddControl<MySensorBlock>((MyTerminalControl<MySensorBlock>) onOff10);
      MyTerminalControlOnOffSwitch<MySensorBlock> onOff11 = new MyTerminalControlOnOffSwitch<MySensorBlock>("Detect Enemy", MySpaceTexts.BlockPropertyTitle_SensorDetectEnemy, MySpaceTexts.BlockPropertyTitle_SensorDetectEnemy);
      onOff11.Getter = (MyTerminalValueControl<MySensorBlock, bool>.GetterDelegate) (x => x.DetectEnemy);
      onOff11.Setter = (MyTerminalValueControl<MySensorBlock, bool>.SetterDelegate) ((x, v) => x.DetectEnemy = v);
      onOff11.EnableToggleAction<MySensorBlock>();
      onOff11.EnableOnOffActions<MySensorBlock>();
      MyTerminalControlFactory.AddControl<MySensorBlock>((MyTerminalControl<MySensorBlock>) onOff11);
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.SyncFlag = true;
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(this.BlockDefinition.ResourceSinkGroup, this.BlockDefinition.RequiredPowerInput, new Func<float>(this.CalculateRequiredPowerInput));
      this.ResourceSink = resourceSinkComponent;
      base.Init(objectBuilder, cubeGrid);
      this.m_items = new List<ToolbarItem>(2);
      for (int index = 0; index < 2; ++index)
        this.m_items.Add(new ToolbarItem() { EntityID = 0L });
      this.Toolbar = new MyToolbar(MyToolbarType.ButtonPanel, 2, 1);
      this.Toolbar.DrawNumbers = false;
      MyObjectBuilder_SensorBlock builderSensorBlock = (MyObjectBuilder_SensorBlock) objectBuilder;
      this.m_fieldMin.SetLocalValue(Vector3.Clamp((Vector3) builderSensorBlock.FieldMin, new Vector3(-this.MaxRange), -new Vector3(0.1f)));
      this.m_fieldMax.SetLocalValue(Vector3.Clamp((Vector3) builderSensorBlock.FieldMax, new Vector3(0.1f), new Vector3(this.MaxRange)));
      this.m_playProximitySound.SetLocalValue(builderSensorBlock.PlaySound);
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        this.DetectPlayers = builderSensorBlock.DetectPlayers;
        this.DetectFloatingObjects = builderSensorBlock.DetectFloatingObjects;
        this.DetectSmallShips = builderSensorBlock.DetectSmallShips;
        this.DetectLargeShips = builderSensorBlock.DetectLargeShips;
        this.DetectStations = builderSensorBlock.DetectStations;
        this.DetectSubgrids = builderSensorBlock.DetectSubgrids;
        this.DetectAsteroids = builderSensorBlock.DetectAsteroids;
        this.DetectOwner = builderSensorBlock.DetectOwner;
        this.DetectFriendly = builderSensorBlock.DetectFriendly;
        this.DetectNeutral = builderSensorBlock.DetectNeutral;
        this.DetectEnemy = builderSensorBlock.DetectEnemy;
      }
      this.m_active.SetLocalValue(builderSensorBlock.IsActive);
      this.Toolbar.Init(builderSensorBlock.Toolbar, (MyEntity) this);
      for (int index = 0; index < 2; ++index)
      {
        MyToolbarItem itemAtIndex = this.Toolbar.GetItemAtIndex(index);
        if (itemAtIndex != null)
        {
          this.m_items.RemoveAt(index);
          this.m_items.Insert(index, ToolbarItem.FromItem(itemAtIndex));
        }
      }
      this.Toolbar.ItemChanged += new Action<MyToolbar, MyToolbar.IndexArgs, bool>(this.Toolbar_ItemChanged);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.ResourceSink.SetMaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId, this.CalculateRequiredPowerInputWhenEnabled());
      this.ResourceSink.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
      this.ResourceSink.RequiredInputChanged += new MyRequiredResourceChangeDelegate(this.Receiver_RequiredInputChanged);
      this.ResourceSink.Update();
      this.m_fieldShape = this.GetHkShape();
      this.OnClose += (Action<MyEntity>) (self => this.m_fieldShape.RemoveReference());
      this.m_gizmoColor = (Color) new Vector4(0.35f, 0.0f, 0.0f, 0.5f);
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      this.ResourceSink.Update();
      this.UpdateEmissive();
    }

    public override void OnBuildSuccess(long builtBy, bool instantBuild)
    {
      this.ResourceSink.Update();
      base.OnBuildSuccess(builtBy, instantBuild);
    }

    public bool UpdateEmissive() => this.IsWorking && this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) ? this.SetEmissiveState(this.IsActive ? MyCubeBlock.m_emissiveNames.Alternative : MyCubeBlock.m_emissiveNames.Working, this.Render.RenderObjectIDs[0]) : this.SetEmissiveState(this.IsFunctional ? MyCubeBlock.m_emissiveNames.Disabled : MyCubeBlock.m_emissiveNames.Damaged, this.Render.RenderObjectIDs[0]);

    protected void UpdateField() => this.m_recreateField = true;

    protected HkShape GetHkShape() => (HkShape) new HkBoxShape((this.m_fieldMax.Value - this.m_fieldMin.Value) * 0.5f);

    protected override void OnEnabledChanged()
    {
      this.ResourceSink.Update();
      this.UpdateEmissive();
      base.OnEnabledChanged();
    }

    protected float CalculateRequiredPowerInput() => this.Enabled && this.IsFunctional ? this.CalculateRequiredPowerInputWhenEnabled() : 0.0f;

    protected float CalculateRequiredPowerInputWhenEnabled() => (float) (0.000300000014249235 * Math.Pow((double) (this.m_fieldMax.Value - this.m_fieldMin.Value).Volume, 0.333333343267441));

    protected void Receiver_IsPoweredChanged() => MySandboxGame.Static.Invoke((Action) (() =>
    {
      if (this.Closed)
        return;
      this.UpdateIsWorking();
      this.ResourceSink.Update();
      this.UpdateEmissive();
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }), "MySensorBlock::Receiver_IsPoweredChanged");

    protected void Receiver_RequiredInputChanged(
      MyDefinitionId resourceTypeId,
      MyResourceSinkComponent receiver,
      float oldRequirement,
      float newRequirement)
    {
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    private void ComponentStack_IsFunctionalChanged()
    {
      this.ResourceSink.Update();
      this.UpdateEmissive();
    }

    protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
    {
      base.UpdateDetailedInfo(detailedInfo);
      detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
      detailedInfo.Append(this.BlockDefinition.DisplayNameText);
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
      MyValueFormatter.AppendWorkInBestUnit(this.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId), detailedInfo);
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyProperties_CurrentInput));
      MyValueFormatter.AppendWorkInBestUnit(this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) ? this.ResourceSink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId) : 0.0f, detailedInfo);
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_SensorBlock builderCubeBlock = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_SensorBlock;
      builderCubeBlock.FieldMin = (SerializableVector3) this.FieldMin;
      builderCubeBlock.FieldMax = (SerializableVector3) this.FieldMax;
      builderCubeBlock.PlaySound = this.PlayProximitySound;
      builderCubeBlock.DetectPlayers = this.DetectPlayers;
      builderCubeBlock.DetectFloatingObjects = this.DetectFloatingObjects;
      builderCubeBlock.DetectSmallShips = this.DetectSmallShips;
      builderCubeBlock.DetectLargeShips = this.DetectLargeShips;
      builderCubeBlock.DetectStations = this.DetectStations;
      builderCubeBlock.DetectSubgrids = this.DetectSubgrids;
      builderCubeBlock.DetectAsteroids = this.DetectAsteroids;
      builderCubeBlock.DetectOwner = this.DetectOwner;
      builderCubeBlock.DetectFriendly = this.DetectFriendly;
      builderCubeBlock.DetectNeutral = this.DetectNeutral;
      builderCubeBlock.DetectEnemy = this.DetectEnemy;
      builderCubeBlock.IsActive = this.IsActive;
      builderCubeBlock.Toolbar = this.Toolbar.GetObjectBuilder();
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    private void Toolbar_ItemChanged(MyToolbar self, MyToolbar.IndexArgs index, bool isGamepad)
    {
      if (this.m_syncing)
        return;
      ToolbarItem toolbarItem1 = ToolbarItem.FromItem(self.GetItemAtIndex(index.ItemIndex));
      ToolbarItem toolbarItem2 = this.m_items[index.ItemIndex];
      if (toolbarItem1.EntityID == 0L && toolbarItem2.EntityID == 0L || toolbarItem1.EntityID != 0L && toolbarItem2.EntityID != 0L && toolbarItem1.Equals((object) toolbarItem2))
        return;
      this.m_items.RemoveAt(index.ItemIndex);
      this.m_items.Insert(index.ItemIndex, toolbarItem1);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MySensorBlock, ToolbarItem, int>(this, (Func<MySensorBlock, Action<ToolbarItem, int>>) (x => new Action<ToolbarItem, int>(x.SendToolbarItemChanged)), toolbarItem1, index.ItemIndex);
      if (!MySensorBlock.m_shouldSetOtherToolbars)
        return;
      MySensorBlock.m_shouldSetOtherToolbars = false;
      foreach (MyToolbar openedToolbar in MySensorBlock.m_openedToolbars)
      {
        if (openedToolbar != self)
          openedToolbar.SetItemAtIndex(index.ItemIndex, self.GetItemAtIndex(index.ItemIndex));
      }
      MySensorBlock.m_shouldSetOtherToolbars = true;
    }

    private void OnFirstEnter()
    {
      this.UpdateEmissive();
      this.Toolbar.UpdateItem(0);
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.Toolbar.ActivateItemAtSlot(0, playActivationSound: false);
      if (!this.PlayProximitySound)
        return;
      this.PlayActionSound();
      if (!this.m_enablePlaySoundEvent)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MySensorBlock>(this, (Func<MySensorBlock, Action>) (x => new Action(x.PlayActionSound)));
    }

    private void OnLastLeave()
    {
      this.UpdateEmissive();
      this.Toolbar.UpdateItem(1);
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.Toolbar.ActivateItemAtSlot(1, playActivationSound: false);
    }

    public bool ShouldDetectRelation(MyRelationsBetweenPlayerAndBlock relation)
    {
      switch (relation)
      {
        case MyRelationsBetweenPlayerAndBlock.NoOwnership:
        case MyRelationsBetweenPlayerAndBlock.FactionShare:
        case MyRelationsBetweenPlayerAndBlock.Friends:
          return this.DetectFriendly;
        case MyRelationsBetweenPlayerAndBlock.Owner:
          return this.DetectOwner;
        case MyRelationsBetweenPlayerAndBlock.Neutral:
          return this.DetectNeutral;
        case MyRelationsBetweenPlayerAndBlock.Enemies:
          return this.DetectEnemy;
        default:
          throw new InvalidBranchException();
      }
    }

    public bool ShouldDetectGrid(MyCubeGrid grid)
    {
      bool flag = true;
      foreach (long bigOwner in grid.BigOwners)
      {
        if (this.ShouldDetectRelation(MyPlayer.GetRelationBetweenPlayers(this.OwnerId, bigOwner)))
          return true;
        flag = false;
      }
      return flag && this.ShouldDetectRelation(MyRelationsBetweenPlayerAndBlock.Enemies);
    }

    private bool ShouldDetect(MyEntity entity)
    {
      if (entity == null || entity == this.CubeGrid)
        return false;
      if (this.DetectPlayers && entity is MyCharacter)
        return this.ShouldDetectRelation((entity as MyCharacter).GetRelationTo(this.OwnerId));
      if (this.DetectFloatingObjects && entity is MyFloatingObject)
        return true;
      MyCubeGrid myCubeGrid = entity as MyCubeGrid;
      if (this.DetectSubgrids && myCubeGrid != null && MyCubeGridGroups.Static.Logical.HasSameGroup(myCubeGrid, this.CubeGrid))
        return this.ShouldDetectGrid(myCubeGrid);
      if (myCubeGrid != null && MyCubeGridGroups.Static.Logical.HasSameGroup(myCubeGrid, this.CubeGrid))
        return false;
      if (this.DetectSmallShips && myCubeGrid != null && myCubeGrid.GridSizeEnum == MyCubeSize.Small || this.DetectLargeShips && myCubeGrid != null && (myCubeGrid.GridSizeEnum == MyCubeSize.Large && !myCubeGrid.IsStatic) || this.DetectStations && myCubeGrid != null && (myCubeGrid.GridSizeEnum == MyCubeSize.Large && myCubeGrid.IsStatic))
        return this.ShouldDetectGrid(myCubeGrid);
      return this.DetectAsteroids && entity is MyVoxelBase;
    }

    private bool GetPropertiesFromEntity(
      MyEntity entity,
      ref Vector3D position1,
      out Quaternion rotation2,
      out Vector3 posDiff,
      out HkShape? shape2)
    {
      rotation2 = new Quaternion();
      posDiff = Vector3.Zero;
      shape2 = new HkShape?();
      if (entity.Physics == null || !entity.Physics.Enabled)
        return false;
      if ((HkReferenceObject) entity.Physics.RigidBody != (HkReferenceObject) null)
      {
        shape2 = new HkShape?(entity.Physics.RigidBody.GetShape());
        MatrixD worldMatrix = entity.WorldMatrix;
        rotation2 = Quaternion.CreateFromForwardUp((Vector3) worldMatrix.Forward, (Vector3) worldMatrix.Up);
        posDiff = (Vector3) (entity.PositionComp.GetPosition() - position1);
        if (entity is MyVoxelBase)
        {
          MyVoxelBase myVoxelBase = entity as MyVoxelBase;
          posDiff -= (Vector3) (myVoxelBase.Size / 2);
        }
      }
      else
      {
        if (entity.GetPhysicsBody().CharacterProxy == null)
          return false;
        shape2 = new HkShape?(entity.GetPhysicsBody().CharacterProxy.GetShape());
        MatrixD worldMatrix = entity.WorldMatrix;
        rotation2 = Quaternion.CreateFromForwardUp((Vector3) worldMatrix.Forward, (Vector3) worldMatrix.Up);
        posDiff = (Vector3) (entity.PositionComp.WorldAABB.Center - position1);
      }
      return true;
    }

    public override void UpdateAfterSimulation10()
    {
      base.UpdateAfterSimulation10();
      this.m_enablePlaySoundEvent = true;
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || !this.IsWorking)
        return;
      if (!this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
      {
        if (!this.ResourceSink.IsPowerAvailable(MyResourceDistributorComponent.ElectricityId, this.BlockDefinition.RequiredPowerInput))
          return;
        float newRequiredInput = this.ResourceSink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId);
        this.ResourceSink.SetRequiredInputByType(MyResourceDistributorComponent.ElectricityId, 0.0f);
        this.ResourceSink.SetRequiredInputByType(MyResourceDistributorComponent.ElectricityId, newRequiredInput);
      }
      Vector3 forward = (Vector3) this.WorldMatrix.Forward;
      MatrixD worldMatrix = this.WorldMatrix;
      Vector3 up = (Vector3) worldMatrix.Up;
      Quaternion fromForwardUp = Quaternion.CreateFromForwardUp(forward, up);
      Vector3D position1 = this.PositionComp.GetPosition() + Vector3D.Transform((Vector3D) (this.PositionComp.LocalVolume.Center + (this.m_fieldMax.Value + this.m_fieldMin.Value) * 0.5f), fromForwardUp);
      if (this.m_recreateField)
      {
        this.m_recreateField = false;
        this.m_fieldShape.RemoveReference();
        this.m_fieldShape = this.GetHkShape();
        this.ResourceSink.Update();
      }
      BoundingBoxD boundingBoxD = new BoundingBoxD((Vector3D) this.m_fieldMin.Value, (Vector3D) this.m_fieldMax.Value);
      boundingBoxD = boundingBoxD.Translate((Vector3D) this.PositionComp.LocalVolume.Center);
      ref BoundingBoxD local = ref boundingBoxD;
      worldMatrix = this.WorldMatrix;
      MatrixD orientation = worldMatrix.GetOrientation();
      boundingBoxD = local.TransformFast(orientation);
      BoundingBoxD box1 = boundingBoxD.Translate(this.PositionComp.GetPosition());
      MyEntityQueryType qtype = (MyEntityQueryType) 0;
      if (this.DetectAsteroids || this.DetectStations)
        qtype |= MyEntityQueryType.Static;
      if (this.DetectFloatingObjects || this.DetectLargeShips || (this.DetectSmallShips || this.DetectSubgrids) || this.DetectPlayers)
        qtype |= MyEntityQueryType.Dynamic;
      MyGamePruningStructure.GetTopMostEntitiesInBox(ref box1, MySensorBlock.m_potentialPenetrations, qtype);
      this.LastDetectedEntity = (MyEntity) null;
      this.m_detectedEntities.Clear();
      using (HkAccessControl.PushState(HkAccessControl.AccessState.SharedRead))
      {
        foreach (MyEntity potentialPenetration in MySensorBlock.m_potentialPenetrations)
        {
          Quaternion rotation2;
          Vector3 posDiff;
          HkShape? shape2;
          if (!(potentialPenetration is MyVoxelBase) && this.ShouldDetect(potentialPenetration) && (this.GetPropertiesFromEntity(potentialPenetration, ref position1, out rotation2, out posDiff, out shape2) && potentialPenetration.GetPhysicsBody().HavokWorld.IsPenetratingShapeShape(this.m_fieldShape, ref Vector3.Zero, ref fromForwardUp, shape2.Value, ref posDiff, ref rotation2)))
          {
            this.LastDetectedEntity = potentialPenetration;
            this.m_detectedEntities.Add(MyDetectedEntityInfoHelper.Create(potentialPenetration, this.OwnerId));
          }
        }
      }
      if (this.DetectAsteroids)
      {
        foreach (MyEntity potentialPenetration in MySensorBlock.m_potentialPenetrations)
        {
          if (potentialPenetration is MyVoxelBase)
          {
            if (potentialPenetration is MyVoxelPhysics myVoxelPhysics)
            {
              Vector3 localPosition1;
              MyVoxelCoordSystems.WorldPositionToLocalPosition(box1.Min, myVoxelPhysics.PositionComp.WorldMatrixRef, myVoxelPhysics.PositionComp.WorldMatrixInvScaled, myVoxelPhysics.SizeInMetresHalf, out localPosition1);
              Vector3 localPosition2;
              MyVoxelCoordSystems.WorldPositionToLocalPosition(box1.Max, myVoxelPhysics.PositionComp.WorldMatrixRef, myVoxelPhysics.PositionComp.WorldMatrixInvScaled, myVoxelPhysics.SizeInMetresHalf, out localPosition2);
              BoundingBoxI box2 = new BoundingBoxI(new Vector3I(localPosition1), new Vector3I(localPosition2));
              box2.Translate(myVoxelPhysics.StorageMin);
              if (myVoxelPhysics.Storage.Intersect(ref box2, 1, false) != ContainmentType.Disjoint)
              {
                this.LastDetectedEntity = potentialPenetration;
                this.m_detectedEntities.Add(MyDetectedEntityInfoHelper.Create(potentialPenetration, this.OwnerId));
              }
            }
            else
            {
              Quaternion rotation2;
              Vector3 posDiff;
              HkShape? shape2;
              if (this.GetPropertiesFromEntity(potentialPenetration, ref position1, out rotation2, out posDiff, out shape2) && potentialPenetration.GetPhysicsBody().HavokWorld.IsPenetratingShapeShape(this.m_fieldShape, ref Vector3.Zero, ref fromForwardUp, shape2.Value, ref posDiff, ref rotation2))
              {
                this.LastDetectedEntity = potentialPenetration;
                this.m_detectedEntities.Add(MyDetectedEntityInfoHelper.Create(potentialPenetration, this.OwnerId));
              }
            }
          }
        }
      }
      this.IsActive = this.m_detectedEntities.Count > 0;
      MySensorBlock.m_potentialPenetrations.Clear();
    }

    private event Action<bool> StateChanged;

    event Action<bool> Sandbox.ModAPI.IMySensorBlock.StateChanged
    {
      add => this.StateChanged += value;
      remove => this.StateChanged -= value;
    }

    public Color GetGizmoColor() => this.m_gizmoColor;

    public bool CanBeDrawn() => MyCubeGrid.ShowSenzorGizmos && this.ShowOnHUD && (this.IsWorking && this.HasLocalPlayerAccess()) && this.GetDistanceBetweenPlayerPositionAndBoundingSphere() <= 400.0;

    public BoundingBox? GetBoundingBox()
    {
      this.m_gizmoBoundingBox.Min = this.PositionComp.LocalVolume.Center + this.FieldMin;
      this.m_gizmoBoundingBox.Max = this.PositionComp.LocalVolume.Center + this.FieldMax;
      return new BoundingBox?(this.m_gizmoBoundingBox);
    }

    public float GetRadius() => -1f;

    public MatrixD GetWorldMatrix() => this.WorldMatrix;

    public Vector3 GetPositionInGrid() => (Vector3) this.Position;

    public bool EnableLongDrawDistance() => false;

    private void IsActiveChanged()
    {
      if ((bool) this.m_active)
        this.OnFirstEnter();
      else
        this.OnLastLeave();
      this.m_gizmoColor = !(bool) this.m_active ? (Color) new Vector4(0.35f, 0.0f, 0.0f, 0.5f) : (Color) new Vector4(0.0f, 0.35f, 0.0f, 0.5f);
      Action<bool> stateChanged = this.StateChanged;
      if (stateChanged == null)
        return;
      stateChanged((bool) this.m_active);
    }

    [Event(null, 1206)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void SendToolbarItemChanged(ToolbarItem sentItem, int index)
    {
      this.m_syncing = true;
      MyToolbarItem myToolbarItem = (MyToolbarItem) null;
      if (sentItem.EntityID != 0L)
        myToolbarItem = ToolbarItem.ToItem(sentItem);
      this.Toolbar.SetItemAtIndex(index, myToolbarItem);
      this.m_syncing = false;
    }

    [Event(null, 1219)]
    [Reliable]
    [Broadcast]
    private void PlayActionSound()
    {
      if (this.m_soundEmitter == null)
        return;
      this.m_soundEmitter.PlaySound(this.m_actionSound, force3D: new bool?(true));
    }

    float Sandbox.ModAPI.Ingame.IMySensorBlock.LeftExtend
    {
      get => this.LeftExtend;
      set => this.LeftExtend = MathHelper.Clamp(value, 0.1f, this.BlockDefinition.MaxRange);
    }

    float Sandbox.ModAPI.Ingame.IMySensorBlock.RightExtend
    {
      get => this.RightExtend;
      set => this.RightExtend = MathHelper.Clamp(value, 0.1f, this.BlockDefinition.MaxRange);
    }

    float Sandbox.ModAPI.Ingame.IMySensorBlock.TopExtend
    {
      get => this.TopExtend;
      set => this.TopExtend = MathHelper.Clamp(value, 0.1f, this.BlockDefinition.MaxRange);
    }

    float Sandbox.ModAPI.Ingame.IMySensorBlock.BottomExtend
    {
      get => this.BottomExtend;
      set => this.BottomExtend = MathHelper.Clamp(value, 0.1f, this.BlockDefinition.MaxRange);
    }

    float Sandbox.ModAPI.Ingame.IMySensorBlock.FrontExtend
    {
      get => this.FrontExtend;
      set => this.FrontExtend = MathHelper.Clamp(value, 0.1f, this.BlockDefinition.MaxRange);
    }

    float Sandbox.ModAPI.Ingame.IMySensorBlock.BackExtend
    {
      get => this.BackExtend;
      set => this.BackExtend = MathHelper.Clamp(value, 0.1f, this.BlockDefinition.MaxRange);
    }

    bool Sandbox.ModAPI.Ingame.IMySensorBlock.PlayProximitySound
    {
      get => this.PlayProximitySound;
      set => this.PlayProximitySound = value;
    }

    bool Sandbox.ModAPI.Ingame.IMySensorBlock.DetectPlayers
    {
      get => this.DetectPlayers;
      set => this.DetectPlayers = value;
    }

    bool Sandbox.ModAPI.Ingame.IMySensorBlock.DetectFloatingObjects
    {
      get => this.DetectFloatingObjects;
      set => this.DetectFloatingObjects = value;
    }

    bool Sandbox.ModAPI.Ingame.IMySensorBlock.DetectSmallShips
    {
      get => this.DetectSmallShips;
      set => this.DetectSmallShips = value;
    }

    bool Sandbox.ModAPI.Ingame.IMySensorBlock.DetectLargeShips
    {
      get => this.DetectLargeShips;
      set => this.DetectLargeShips = value;
    }

    bool Sandbox.ModAPI.Ingame.IMySensorBlock.DetectStations
    {
      get => this.DetectStations;
      set => this.DetectStations = value;
    }

    bool Sandbox.ModAPI.Ingame.IMySensorBlock.DetectAsteroids
    {
      get => this.DetectAsteroids;
      set => this.DetectAsteroids = value;
    }

    bool Sandbox.ModAPI.Ingame.IMySensorBlock.DetectOwner
    {
      get => this.DetectOwner;
      set => this.DetectOwner = value;
    }

    bool Sandbox.ModAPI.Ingame.IMySensorBlock.DetectFriendly
    {
      get => this.DetectFriendly;
      set => this.DetectFriendly = value;
    }

    bool Sandbox.ModAPI.Ingame.IMySensorBlock.DetectNeutral
    {
      get => this.DetectNeutral;
      set => this.DetectNeutral = value;
    }

    bool Sandbox.ModAPI.Ingame.IMySensorBlock.DetectEnemy
    {
      get => this.DetectEnemy;
      set => this.DetectEnemy = value;
    }

    Vector3 Sandbox.ModAPI.IMySensorBlock.FieldMin
    {
      get => this.FieldMin;
      set => this.FieldMin = value;
    }

    Vector3 Sandbox.ModAPI.IMySensorBlock.FieldMax
    {
      get => this.FieldMax;
      set => this.FieldMax = value;
    }

    bool Sandbox.ModAPI.Ingame.IMySensorBlock.IsActive => this.IsActive;

    MyDetectedEntityInfo Sandbox.ModAPI.Ingame.IMySensorBlock.LastDetectedEntity => MyDetectedEntityInfoHelper.Create(this.LastDetectedEntity, this.OwnerId);

    void Sandbox.ModAPI.Ingame.IMySensorBlock.DetectedEntities(
      List<MyDetectedEntityInfo> result)
    {
      result.Clear();
      result.AddRange((IEnumerable<MyDetectedEntityInfo>) this.m_detectedEntities);
    }

    protected sealed class SendToolbarItemChanged\u003C\u003ESandbox_Game_Entities_Blocks_ToolbarItem\u0023System_Int32 : ICallSite<MySensorBlock, ToolbarItem, int, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MySensorBlock @this,
        in ToolbarItem sentItem,
        in int index,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.SendToolbarItemChanged(sentItem, index);
      }
    }

    protected sealed class PlayActionSound\u003C\u003E : ICallSite<MySensorBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MySensorBlock @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.PlayActionSound();
      }
    }

    protected class m_playProximitySound\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MySensorBlock) obj0).m_playProximitySound = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_active\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MySensorBlock) obj0).m_active = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_fieldMin\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<Vector3, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<Vector3, SyncDirection.BothWays>(obj1, obj2));
        ((MySensorBlock) obj0).m_fieldMin = (VRage.Sync.Sync<Vector3, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_fieldMax\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<Vector3, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<Vector3, SyncDirection.BothWays>(obj1, obj2));
        ((MySensorBlock) obj0).m_fieldMax = (VRage.Sync.Sync<Vector3, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_flags\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<MySensorFilterFlags, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<MySensorFilterFlags, SyncDirection.BothWays>(obj1, obj2));
        ((MySensorBlock) obj0).m_flags = (VRage.Sync.Sync<MySensorFilterFlags, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_Blocks_MySensorBlock\u003C\u003EActor : IActivator, IActivator<MySensorBlock>
    {
      object IActivator.CreateInstance() => (object) new MySensorBlock();

      MySensorBlock IActivator<MySensorBlock>.CreateInstance() => new MySensorBlock();
    }
  }
}
