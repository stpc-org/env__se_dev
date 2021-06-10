// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.Blocks.SafeZone.MySafeZoneBlock
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using ObjectBuilders.SafeZone;
using Sandbox;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using SpaceEngineers.Game.Definitions.SafeZone;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Definitions;
using VRage.Game.Entity;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.ObjectBuilders.Components.Beacon;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks.SafeZone
{
  [MyCubeBlockType(typeof (MyObjectBuilder_SafeZoneBlock))]
  [MyTerminalInterface(new Type[] {typeof (SpaceEngineers.Game.ModAPI.IMySafeZoneBlock), typeof (SpaceEngineers.Game.ModAPI.Ingame.IMySafeZoneBlock)})]
  public class MySafeZoneBlock : MyFunctionalBlock, IMyConveyorEndpointBlock, SpaceEngineers.Game.ModAPI.IMySafeZoneBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMySafeZoneBlock, IMyMultiTextPanelComponentOwner, IMyTextPanelComponentOwner, Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider
  {
    private MySafeZoneComponent m_safeZoneManager;
    private MyMultilineConveyorEndpoint m_conveyorEndpoint;
    private MyMultiTextPanelComponent m_multiPanel;
    private MyGuiScreenTextPanel m_textBoxMultiPanel;
    protected MySoundPair m_processSound = new MySoundPair();
    protected bool m_isSoundRunning;
    private MySessionComponentDLC m_dlcComponent;
    private bool m_readyToRecieveEvents;
    private bool m_isTextPanelOpen;

    internal MySafeZoneBlockDefinition Definition => (MySafeZoneBlockDefinition) this.BlockDefinition;

    public IMyConveyorEndpoint ConveyorEndpoint => (IMyConveyorEndpoint) this.m_conveyorEndpoint;

    public MySafeZoneBlock() => this.Render = (MyRenderComponentBase) new MyRenderComponentScreenAreas((MyEntity) this);

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      MyObjectBuilder_SafeZoneBlock builderSafeZoneBlock = objectBuilder as MyObjectBuilder_SafeZoneBlock;
      this.m_safeZoneManager = new MySafeZoneComponent();
      this.Components.Add<MySafeZoneComponent>(this.m_safeZoneManager);
      this.m_safeZoneManager.Init(this, builderSafeZoneBlock.SafeZoneId);
      this.m_dlcComponent = MySession.Static.GetComponent<MySessionComponentDLC>();
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(MyStringHash.GetOrCompute(this.Definition.ResourceSinkGroup), this.Definition.MaxSafeZonePowerDrainkW, new Func<float>(this.UpdatePowerInput));
      this.ResourceSink = resourceSinkComponent;
      this.ResourceSink.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
      this.ResourceSink.Update();
      this.CheckObjectBuilders(objectBuilder as MyObjectBuilder_SafeZoneBlock, this.Definition);
      base.Init(objectBuilder, cubeGrid);
      this.m_processSound = this.BlockDefinition.ActionSound;
      if (this.Definition.ScreenAreas != null && this.Definition.ScreenAreas.Count > 0)
      {
        this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
        this.m_multiPanel = new MyMultiTextPanelComponent((MyTerminalBlock) this, this.Definition.ScreenAreas, builderSafeZoneBlock.TextPanels);
        this.m_multiPanel.Init(new Action<int, int[]>(this.SendAddImagesToSelectionRequest), new Action<int, int[]>(this.SendRemoveSelectedImageRequest), new Action<int, string>(this.ChangeTextRequest), new Action<int, MySerializableSpriteCollection>(this.UpdateSpriteCollection));
      }
      this.UpdateEffectsAndText();
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    private void CheckObjectBuilders(
      MyObjectBuilder_SafeZoneBlock builder,
      MySafeZoneBlockDefinition definition)
    {
      if (builder == null || definition == null || builder.ComponentContainer == null)
        return;
      foreach (MyObjectBuilder_ComponentContainer.ComponentData component1 in builder.ComponentContainer.Components)
      {
        if (component1.Component is MyObjectBuilder_SafeZoneComponent component && component.SafeZoneOb != null && component.SafeZoneOb is MyObjectBuilder_SafeZone safeZoneOb)
        {
          Vector3 size = safeZoneOb.Size;
          size.X = MathHelper.Clamp(size.X, definition.MinSafeZoneRadius, 2f * definition.MaxSafeZoneRadius);
          size.Y = MathHelper.Clamp(size.Y, definition.MinSafeZoneRadius, 2f * definition.MaxSafeZoneRadius);
          size.Z = MathHelper.Clamp(size.Z, definition.MinSafeZoneRadius, 2f * definition.MaxSafeZoneRadius);
          safeZoneOb.Size = size;
          safeZoneOb.Radius = MathHelper.Clamp(safeZoneOb.Radius, definition.MinSafeZoneRadius, definition.MaxSafeZoneRadius);
        }
      }
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      this.ResourceSink.Update();
      this.UpdateEffectsAndText();
      this.UpdateScreen();
      this.m_readyToRecieveEvents = true;
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MySafeZoneBlock>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlSeparator<MySafeZoneBlock> controlSeparator = new MyTerminalControlSeparator<MySafeZoneBlock>();
      controlSeparator.Visible = (Func<MySafeZoneBlock, bool>) (beacon => beacon.Definition.CubeSize == MyCubeSize.Large);
      MyTerminalControlFactory.AddControl<MySafeZoneBlock>((MyTerminalControl<MySafeZoneBlock>) controlSeparator);
      MyTerminalControlFactory.AddControl<MySafeZoneBlock>((MyTerminalControl<MySafeZoneBlock>) new MyTerminalControlLabel<MySafeZoneBlock>(MySpaceTexts.TerminalSafeZoneNeedsStation));
      MyTerminalControlOnOffSwitch<MySafeZoneBlock> safeZoneOnOff = new MyTerminalControlOnOffSwitch<MySafeZoneBlock>("SafeZoneCreate", MySpaceTexts.Beacon_SafeZone_Desc, on: new MyStringId?(MySpaceTexts.Beacon_SafeZone_On), off: new MyStringId?(MySpaceTexts.Beacon_SafeZone_Off));
      safeZoneOnOff.Getter = (MyTerminalValueControl<MySafeZoneBlock, bool>.GetterDelegate) (beacon => (ulong) beacon.m_safeZoneManager.SafeZoneEntityId > 0UL);
      safeZoneOnOff.Setter = (MyTerminalValueControl<MySafeZoneBlock, bool>.SetterDelegate) ((beacon, isturnOn) => beacon.OnSafezoneCreateRemove(isturnOn));
      safeZoneOnOff.Enabled = (Func<MySafeZoneBlock, bool>) (beacon => !beacon.m_safeZoneManager.WaitingResponse && beacon.Enabled && beacon.IsFunctional && beacon.CubeGrid.IsStatic);
      safeZoneOnOff.Visible = (Func<MySafeZoneBlock, bool>) (beacon => beacon.Definition.CubeSize == MyCubeSize.Large);
      safeZoneOnOff.DynamicTooltipGetter = (MyTerminalControl<MySafeZoneBlock>.TooltipGetter) (beacon => beacon.OnGetTooltip());
      MyTerminalControlFactory.AddControl<MySafeZoneBlock>((MyTerminalControl<MySafeZoneBlock>) safeZoneOnOff);
      MyTerminalControlCombobox<MySafeZoneBlock> terminalControlCombobox = new MyTerminalControlCombobox<MySafeZoneBlock>("SafeZoneShapeCombo", MySpaceTexts.SafeZone_SelectZoneShape, MySpaceTexts.Beacon_SafeZone_Shape_TTIP);
      MyTerminalControlComboBoxItem controlComboBoxItem = new MyTerminalControlComboBoxItem();
      controlComboBoxItem.Key = 0L;
      controlComboBoxItem.Value = MySpaceTexts.SafeZone_Spherical;
      MyTerminalControlComboBoxItem sphere = controlComboBoxItem;
      controlComboBoxItem = new MyTerminalControlComboBoxItem();
      controlComboBoxItem.Key = 1L;
      controlComboBoxItem.Value = MySpaceTexts.SafeZone_Cubical;
      MyTerminalControlComboBoxItem box = controlComboBoxItem;
      terminalControlCombobox.ComboBoxContent = (Action<List<MyTerminalControlComboBoxItem>>) (list =>
      {
        list.Add(sphere);
        list.Add(box);
      });
      terminalControlCombobox.Setter = (MyTerminalValueControl<MySafeZoneBlock, long>.SetterDelegate) ((beacon, key) => beacon.m_safeZoneManager.OnSafeZoneShapeChanged((MySafeZoneShape) key));
      terminalControlCombobox.Getter = (MyTerminalValueControl<MySafeZoneBlock, long>.GetterDelegate) (beacon => beacon.m_safeZoneManager.GetSafeZoneShape());
      terminalControlCombobox.Visible = (Func<MySafeZoneBlock, bool>) (beacon => beacon.Definition.CubeSize == MyCubeSize.Large);
      terminalControlCombobox.Enabled = (Func<MySafeZoneBlock, bool>) (beacon => (ulong) beacon.m_safeZoneManager.SafeZoneEntityId > 0UL);
      MyTerminalControlFactory.AddControl<MySafeZoneBlock>((MyTerminalControl<MySafeZoneBlock>) terminalControlCombobox);
      MyTerminalControlSlider<MySafeZoneBlock> terminalControlSlider1 = new MyTerminalControlSlider<MySafeZoneBlock>("SafeZoneSlider", MySpaceTexts.Beacon_SafeZone_RangeSlider, MySpaceTexts.Beacon_SafeZone_RangeSlider_TTIP);
      terminalControlSlider1.SetLogLimits((MyTerminalValueControl<MySafeZoneBlock, float>.GetterDelegate) (beacon => beacon.Definition.MinSafeZoneRadius), (MyTerminalValueControl<MySafeZoneBlock, float>.GetterDelegate) (beacon => beacon.Definition.MaxSafeZoneRadius));
      terminalControlSlider1.DefaultValueGetter = (MyTerminalValueControl<MySafeZoneBlock, float>.GetterDelegate) (beacon => beacon.m_safeZoneManager.GetRadius());
      terminalControlSlider1.Getter = (MyTerminalValueControl<MySafeZoneBlock, float>.GetterDelegate) (beacon => beacon.m_safeZoneManager.GetRadius());
      terminalControlSlider1.Setter = (MyTerminalValueControl<MySafeZoneBlock, float>.SetterDelegate) ((beacon, radius) => beacon.m_safeZoneManager.SetRadius(radius));
      terminalControlSlider1.Writer = (MyTerminalControl<MySafeZoneBlock>.WriterDelegate) ((beacon, result) => result.AppendDecimal(beacon.m_safeZoneManager.GetRadius(), 0).Append(" m"));
      terminalControlSlider1.Enabled = (Func<MySafeZoneBlock, bool>) (beacon => (ulong) beacon.m_safeZoneManager.SafeZoneEntityId > 0UL);
      terminalControlSlider1.Visible = (Func<MySafeZoneBlock, bool>) (beacon => beacon.Definition.CubeSize == MyCubeSize.Large && beacon.m_safeZoneManager.GetSafeZoneShape() == 0L);
      MyTerminalControlFactory.AddControl<MySafeZoneBlock>((MyTerminalControl<MySafeZoneBlock>) terminalControlSlider1);
      MyTerminalControlSlider<MySafeZoneBlock> slider = new MyTerminalControlSlider<MySafeZoneBlock>("SafeZoneXSlider", MySpaceTexts.SafeZone_Size_X, MySpaceTexts.Beacon_SafeZone_RangeSlider_TTIP);
      slider.SetLogLimits((MyTerminalValueControl<MySafeZoneBlock, float>.GetterDelegate) (beacon => beacon.Definition.MinSafeZoneRadius), (MyTerminalValueControl<MySafeZoneBlock, float>.GetterDelegate) (beacon => beacon.Definition.MaxSafeZoneRadius * 2f));
      slider.DefaultValueGetter = (MyTerminalValueControl<MySafeZoneBlock, float>.GetterDelegate) (beacon => beacon.m_safeZoneManager.GetSize().X);
      slider.Getter = (MyTerminalValueControl<MySafeZoneBlock, float>.GetterDelegate) (beacon => beacon.m_safeZoneManager.GetSize().X);
      slider.Setter = (MyTerminalValueControl<MySafeZoneBlock, float>.SetterDelegate) ((beacon, value) => beacon.m_safeZoneManager.SetSize(MyGuiScreenAdminMenu.MyZoneAxisTypeEnum.X, value));
      slider.Writer = (MyTerminalControl<MySafeZoneBlock>.WriterDelegate) ((beacon, result) => result.AppendDecimal(beacon.m_safeZoneManager.GetSize().X, 0).Append(" m"));
      slider.Enabled = (Func<MySafeZoneBlock, bool>) (beacon => (ulong) beacon.m_safeZoneManager.SafeZoneEntityId > 0UL);
      slider.Visible = (Func<MySafeZoneBlock, bool>) (beacon => beacon.Definition.CubeSize == MyCubeSize.Large && beacon.m_safeZoneManager.GetSafeZoneShape() == 1L);
      slider.EnableActions<MySafeZoneBlock>();
      MyTerminalControlFactory.AddControl<MySafeZoneBlock>((MyTerminalControl<MySafeZoneBlock>) slider);
      MyTerminalControlSlider<MySafeZoneBlock> terminalControlSlider2 = new MyTerminalControlSlider<MySafeZoneBlock>("SafeZoneYSlider", MySpaceTexts.SafeZone_Size_Y, MySpaceTexts.Beacon_SafeZone_RangeSlider_TTIP);
      terminalControlSlider2.SetLogLimits((MyTerminalValueControl<MySafeZoneBlock, float>.GetterDelegate) (beacon => beacon.Definition.MinSafeZoneRadius), (MyTerminalValueControl<MySafeZoneBlock, float>.GetterDelegate) (beacon => beacon.Definition.MaxSafeZoneRadius * 2f));
      terminalControlSlider2.DefaultValueGetter = (MyTerminalValueControl<MySafeZoneBlock, float>.GetterDelegate) (beacon => beacon.m_safeZoneManager.GetSize().Y);
      terminalControlSlider2.Getter = (MyTerminalValueControl<MySafeZoneBlock, float>.GetterDelegate) (beacon => beacon.m_safeZoneManager.GetSize().Y);
      terminalControlSlider2.Setter = (MyTerminalValueControl<MySafeZoneBlock, float>.SetterDelegate) ((beacon, value) => beacon.m_safeZoneManager.SetSize(MyGuiScreenAdminMenu.MyZoneAxisTypeEnum.Y, value));
      terminalControlSlider2.Writer = (MyTerminalControl<MySafeZoneBlock>.WriterDelegate) ((beacon, result) => result.AppendDecimal(beacon.m_safeZoneManager.GetSize().Y, 0).Append(" m"));
      terminalControlSlider2.Enabled = (Func<MySafeZoneBlock, bool>) (beacon => (ulong) beacon.m_safeZoneManager.SafeZoneEntityId > 0UL);
      terminalControlSlider2.Visible = (Func<MySafeZoneBlock, bool>) (beacon => beacon.Definition.CubeSize == MyCubeSize.Large && beacon.m_safeZoneManager.GetSafeZoneShape() == 1L);
      MyTerminalControlFactory.AddControl<MySafeZoneBlock>((MyTerminalControl<MySafeZoneBlock>) terminalControlSlider2);
      MyTerminalControlSlider<MySafeZoneBlock> terminalControlSlider3 = new MyTerminalControlSlider<MySafeZoneBlock>("SafeZoneZSlider", MySpaceTexts.SafeZone_Size_Z, MySpaceTexts.Beacon_SafeZone_RangeSlider_TTIP);
      terminalControlSlider3.SetLogLimits((MyTerminalValueControl<MySafeZoneBlock, float>.GetterDelegate) (beacon => beacon.Definition.MinSafeZoneRadius), (MyTerminalValueControl<MySafeZoneBlock, float>.GetterDelegate) (beacon => beacon.Definition.MaxSafeZoneRadius * 2f));
      terminalControlSlider3.DefaultValueGetter = (MyTerminalValueControl<MySafeZoneBlock, float>.GetterDelegate) (beacon => beacon.m_safeZoneManager.GetSize().Z);
      terminalControlSlider3.Getter = (MyTerminalValueControl<MySafeZoneBlock, float>.GetterDelegate) (beacon => beacon.m_safeZoneManager.GetSize().Z);
      terminalControlSlider3.Setter = (MyTerminalValueControl<MySafeZoneBlock, float>.SetterDelegate) ((beacon, value) => beacon.m_safeZoneManager.SetSize(MyGuiScreenAdminMenu.MyZoneAxisTypeEnum.Z, value));
      terminalControlSlider3.Writer = (MyTerminalControl<MySafeZoneBlock>.WriterDelegate) ((beacon, result) => result.AppendDecimal(beacon.m_safeZoneManager.GetSize().Z, 0).Append(" m"));
      terminalControlSlider3.Enabled = (Func<MySafeZoneBlock, bool>) (beacon => (ulong) beacon.m_safeZoneManager.SafeZoneEntityId > 0UL);
      terminalControlSlider3.Visible = (Func<MySafeZoneBlock, bool>) (beacon => beacon.Definition.CubeSize == MyCubeSize.Large && beacon.m_safeZoneManager.GetSafeZoneShape() == 1L);
      MyTerminalControlFactory.AddControl<MySafeZoneBlock>((MyTerminalControl<MySafeZoneBlock>) terminalControlSlider3);
      MyTerminalControlButton<MySafeZoneBlock> terminalControlButton = new MyTerminalControlButton<MySafeZoneBlock>("SafeZoneFilterBtn", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_ConfigureFilter, MySpaceTexts.Beacon_SafeZone_FilterBtn_TTIP, (Action<MySafeZoneBlock>) (x => x.OnSafeZoneFilterBtnPressed()));
      terminalControlButton.Visible = (Func<MySafeZoneBlock, bool>) (beacon => beacon.Definition.CubeSize == MyCubeSize.Large);
      terminalControlButton.Enabled = (Func<MySafeZoneBlock, bool>) (beacon => (ulong) beacon.m_safeZoneManager.SafeZoneEntityId > 0UL);
      MyTerminalControlFactory.AddControl<MySafeZoneBlock>((MyTerminalControl<MySafeZoneBlock>) terminalControlButton);
      MyTerminalControlCheckbox<MySafeZoneBlock> terminalControlCheckbox1 = new MyTerminalControlCheckbox<MySafeZoneBlock>("SafeZoneDamageCb", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowDamage, MySpaceTexts.Beacon_SafeZone_AllowDmg_TTIP, justify: true);
      terminalControlCheckbox1.Setter = (MyTerminalValueControl<MySafeZoneBlock, bool>.SetterDelegate) ((beacon, isChecked) => beacon.m_safeZoneManager.OnSafeZoneSettingChanged(MySafeZoneAction.Damage, isChecked));
      terminalControlCheckbox1.Getter = (MyTerminalValueControl<MySafeZoneBlock, bool>.GetterDelegate) (beacon => beacon.m_safeZoneManager.GetSafeZoneSetting(MySafeZoneAction.Damage));
      terminalControlCheckbox1.Visible = (Func<MySafeZoneBlock, bool>) (beacon => beacon.Definition.CubeSize == MyCubeSize.Large);
      terminalControlCheckbox1.Enabled = (Func<MySafeZoneBlock, bool>) (beacon => (ulong) beacon.m_safeZoneManager.SafeZoneEntityId > 0UL);
      MyTerminalControlFactory.AddControl<MySafeZoneBlock>((MyTerminalControl<MySafeZoneBlock>) terminalControlCheckbox1);
      MyTerminalControlCheckbox<MySafeZoneBlock> terminalControlCheckbox2 = new MyTerminalControlCheckbox<MySafeZoneBlock>("SafeZoneShootingCb", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowShooting, MySpaceTexts.Beacon_SafeZone_AllowShoot_TTIP, justify: true);
      terminalControlCheckbox2.Setter = (MyTerminalValueControl<MySafeZoneBlock, bool>.SetterDelegate) ((beacon, isChecked) => beacon.m_safeZoneManager.OnSafeZoneSettingChanged(MySafeZoneAction.Shooting, isChecked));
      terminalControlCheckbox2.Getter = (MyTerminalValueControl<MySafeZoneBlock, bool>.GetterDelegate) (beacon => beacon.m_safeZoneManager.GetSafeZoneSetting(MySafeZoneAction.Shooting));
      terminalControlCheckbox2.Visible = (Func<MySafeZoneBlock, bool>) (beacon => beacon.Definition.CubeSize == MyCubeSize.Large);
      terminalControlCheckbox2.Enabled = (Func<MySafeZoneBlock, bool>) (beacon => (ulong) beacon.m_safeZoneManager.SafeZoneEntityId > 0UL);
      MyTerminalControlFactory.AddControl<MySafeZoneBlock>((MyTerminalControl<MySafeZoneBlock>) terminalControlCheckbox2);
      MyTerminalControlCheckbox<MySafeZoneBlock> terminalControlCheckbox3 = new MyTerminalControlCheckbox<MySafeZoneBlock>("SafeZoneDrillingCb", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowDrilling, MySpaceTexts.Beacon_SafeZone_AllowDrill_TTIP, justify: true);
      terminalControlCheckbox3.Setter = (MyTerminalValueControl<MySafeZoneBlock, bool>.SetterDelegate) ((beacon, isChecked) => beacon.m_safeZoneManager.OnSafeZoneSettingChanged(MySafeZoneAction.Drilling, isChecked));
      terminalControlCheckbox3.Getter = (MyTerminalValueControl<MySafeZoneBlock, bool>.GetterDelegate) (beacon => beacon.m_safeZoneManager.GetSafeZoneSetting(MySafeZoneAction.Drilling));
      terminalControlCheckbox3.Visible = (Func<MySafeZoneBlock, bool>) (beacon => beacon.Definition.CubeSize == MyCubeSize.Large);
      terminalControlCheckbox3.Enabled = (Func<MySafeZoneBlock, bool>) (beacon => (ulong) beacon.m_safeZoneManager.SafeZoneEntityId > 0UL);
      MyTerminalControlFactory.AddControl<MySafeZoneBlock>((MyTerminalControl<MySafeZoneBlock>) terminalControlCheckbox3);
      MyTerminalControlCheckbox<MySafeZoneBlock> terminalControlCheckbox4 = new MyTerminalControlCheckbox<MySafeZoneBlock>("SafeZoneWeldingCb", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowWelding, MySpaceTexts.Beacon_SafeZone_AllowWeld_TTIP, justify: true);
      terminalControlCheckbox4.Setter = (MyTerminalValueControl<MySafeZoneBlock, bool>.SetterDelegate) ((beacon, isChecked) => beacon.m_safeZoneManager.OnSafeZoneSettingChanged(MySafeZoneAction.Welding, isChecked));
      terminalControlCheckbox4.Getter = (MyTerminalValueControl<MySafeZoneBlock, bool>.GetterDelegate) (beacon => beacon.m_safeZoneManager.GetSafeZoneSetting(MySafeZoneAction.Welding));
      terminalControlCheckbox4.Visible = (Func<MySafeZoneBlock, bool>) (beacon => beacon.Definition.CubeSize == MyCubeSize.Large);
      terminalControlCheckbox4.Enabled = (Func<MySafeZoneBlock, bool>) (beacon => (ulong) beacon.m_safeZoneManager.SafeZoneEntityId > 0UL);
      MyTerminalControlFactory.AddControl<MySafeZoneBlock>((MyTerminalControl<MySafeZoneBlock>) terminalControlCheckbox4);
      MyTerminalControlCheckbox<MySafeZoneBlock> terminalControlCheckbox5 = new MyTerminalControlCheckbox<MySafeZoneBlock>("SafeZoneGrindingCb", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowGrinding, MySpaceTexts.Beacon_SafeZone_AllowGrind_TTIP, justify: true);
      terminalControlCheckbox5.Setter = (MyTerminalValueControl<MySafeZoneBlock, bool>.SetterDelegate) ((beacon, isChecked) => beacon.m_safeZoneManager.OnSafeZoneSettingChanged(MySafeZoneAction.Grinding, isChecked));
      terminalControlCheckbox5.Getter = (MyTerminalValueControl<MySafeZoneBlock, bool>.GetterDelegate) (beacon => beacon.m_safeZoneManager.GetSafeZoneSetting(MySafeZoneAction.Grinding));
      terminalControlCheckbox5.Visible = (Func<MySafeZoneBlock, bool>) (beacon => beacon.Definition.CubeSize == MyCubeSize.Large);
      terminalControlCheckbox5.Enabled = (Func<MySafeZoneBlock, bool>) (beacon => (ulong) beacon.m_safeZoneManager.SafeZoneEntityId > 0UL);
      MyTerminalControlFactory.AddControl<MySafeZoneBlock>((MyTerminalControl<MySafeZoneBlock>) terminalControlCheckbox5);
      MyTerminalControlCheckbox<MySafeZoneBlock> terminalControlCheckbox6 = new MyTerminalControlCheckbox<MySafeZoneBlock>("SafeZoneBuildingCb", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowBuilding, MySpaceTexts.Beacon_SafeZone_AllowBuild_TTIP, justify: true);
      terminalControlCheckbox6.Setter = (MyTerminalValueControl<MySafeZoneBlock, bool>.SetterDelegate) ((beacon, isChecked) => beacon.m_safeZoneManager.OnSafeZoneSettingChanged(MySafeZoneAction.Building, isChecked));
      terminalControlCheckbox6.Getter = (MyTerminalValueControl<MySafeZoneBlock, bool>.GetterDelegate) (beacon => beacon.m_safeZoneManager.GetSafeZoneSetting(MySafeZoneAction.Building));
      terminalControlCheckbox6.Visible = (Func<MySafeZoneBlock, bool>) (beacon => beacon.Definition.CubeSize == MyCubeSize.Large);
      terminalControlCheckbox6.Enabled = (Func<MySafeZoneBlock, bool>) (beacon => (ulong) beacon.m_safeZoneManager.SafeZoneEntityId > 0UL);
      MyTerminalControlFactory.AddControl<MySafeZoneBlock>((MyTerminalControl<MySafeZoneBlock>) terminalControlCheckbox6);
      MyTerminalControlCheckbox<MySafeZoneBlock> terminalControlCheckbox7 = new MyTerminalControlCheckbox<MySafeZoneBlock>("SafeZoneBuildingProjectionCb", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowBuildingProjections, MySpaceTexts.Beacon_SafeZone_AllowBuild_TTIP, justify: true);
      terminalControlCheckbox7.Setter = (MyTerminalValueControl<MySafeZoneBlock, bool>.SetterDelegate) ((beacon, isChecked) => beacon.m_safeZoneManager.OnSafeZoneSettingChanged(MySafeZoneAction.BuildingProjections, isChecked));
      terminalControlCheckbox7.Getter = (MyTerminalValueControl<MySafeZoneBlock, bool>.GetterDelegate) (beacon => beacon.m_safeZoneManager.GetSafeZoneSetting(MySafeZoneAction.BuildingProjections));
      terminalControlCheckbox7.Visible = (Func<MySafeZoneBlock, bool>) (beacon => beacon.Definition.CubeSize == MyCubeSize.Large);
      terminalControlCheckbox7.Enabled = (Func<MySafeZoneBlock, bool>) (beacon => (ulong) beacon.m_safeZoneManager.SafeZoneEntityId > 0UL);
      MyTerminalControlFactory.AddControl<MySafeZoneBlock>((MyTerminalControl<MySafeZoneBlock>) terminalControlCheckbox7);
      MyTerminalControlCheckbox<MySafeZoneBlock> terminalControlCheckbox8 = new MyTerminalControlCheckbox<MySafeZoneBlock>("SafeZoneVoxelHandCb", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowVoxelHands, MySpaceTexts.Beacon_SafeZone_AllowVoxel_TTIP, justify: true);
      terminalControlCheckbox8.Setter = (MyTerminalValueControl<MySafeZoneBlock, bool>.SetterDelegate) ((beacon, isChecked) => beacon.m_safeZoneManager.OnSafeZoneSettingChanged(MySafeZoneAction.VoxelHand, isChecked));
      terminalControlCheckbox8.Getter = (MyTerminalValueControl<MySafeZoneBlock, bool>.GetterDelegate) (beacon => beacon.m_safeZoneManager.GetSafeZoneSetting(MySafeZoneAction.VoxelHand));
      terminalControlCheckbox8.Visible = (Func<MySafeZoneBlock, bool>) (beacon => beacon.Definition.CubeSize == MyCubeSize.Large);
      terminalControlCheckbox8.Enabled = (Func<MySafeZoneBlock, bool>) (beacon => (ulong) beacon.m_safeZoneManager.SafeZoneEntityId > 0UL);
      MyTerminalControlFactory.AddControl<MySafeZoneBlock>((MyTerminalControl<MySafeZoneBlock>) terminalControlCheckbox8);
      MyTerminalControlCheckbox<MySafeZoneBlock> terminalControlCheckbox9 = new MyTerminalControlCheckbox<MySafeZoneBlock>("SafeZoneLandingGearCb", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowLandingGear, MySpaceTexts.Beacon_SafeZone_AllowLandingGear_TTIP, justify: true);
      terminalControlCheckbox9.Setter = (MyTerminalValueControl<MySafeZoneBlock, bool>.SetterDelegate) ((beacon, isChecked) => beacon.m_safeZoneManager.OnSafeZoneSettingChanged(MySafeZoneAction.LandingGearLock, isChecked));
      terminalControlCheckbox9.Getter = (MyTerminalValueControl<MySafeZoneBlock, bool>.GetterDelegate) (beacon => beacon.m_safeZoneManager.GetSafeZoneSetting(MySafeZoneAction.LandingGearLock));
      terminalControlCheckbox9.Visible = (Func<MySafeZoneBlock, bool>) (beacon => beacon.Definition.CubeSize == MyCubeSize.Large);
      terminalControlCheckbox9.Enabled = (Func<MySafeZoneBlock, bool>) (beacon => (ulong) beacon.m_safeZoneManager.SafeZoneEntityId > 0UL);
      MyTerminalControlFactory.AddControl<MySafeZoneBlock>((MyTerminalControl<MySafeZoneBlock>) terminalControlCheckbox9);
      MyTerminalControlCheckbox<MySafeZoneBlock> terminalControlCheckbox10 = new MyTerminalControlCheckbox<MySafeZoneBlock>("SafeZoneConvertToStationCb", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowConvertToStation, MySpaceTexts.Beacon_SafeZone_AllowConvertToStation_TTIP, justify: true);
      terminalControlCheckbox10.Setter = (MyTerminalValueControl<MySafeZoneBlock, bool>.SetterDelegate) ((beacon, isChecked) => beacon.m_safeZoneManager.OnSafeZoneSettingChanged(MySafeZoneAction.ConvertToStation, isChecked));
      terminalControlCheckbox10.Getter = (MyTerminalValueControl<MySafeZoneBlock, bool>.GetterDelegate) (beacon => beacon.m_safeZoneManager.GetSafeZoneSetting(MySafeZoneAction.ConvertToStation));
      terminalControlCheckbox10.Visible = (Func<MySafeZoneBlock, bool>) (beacon => beacon.Definition.CubeSize == MyCubeSize.Large);
      terminalControlCheckbox10.Enabled = (Func<MySafeZoneBlock, bool>) (beacon => (ulong) beacon.m_safeZoneManager.SafeZoneEntityId > 0UL);
      MyTerminalControlFactory.AddControl<MySafeZoneBlock>((MyTerminalControl<MySafeZoneBlock>) terminalControlCheckbox10);
      MyTerminalControlColor<MySafeZoneBlock> safeZoneColor = new MyTerminalControlColor<MySafeZoneBlock>("SafeZoneColor", MySpaceTexts.ScreenAdmin_Safezone_ColorLabel);
      safeZoneColor.Enabled = (Func<MySafeZoneBlock, bool>) (beacon => (ulong) beacon.m_safeZoneManager.SafeZoneEntityId > 0UL);
      safeZoneColor.Visible = (Func<MySafeZoneBlock, bool>) (beacon => beacon.Definition.CubeSize == MyCubeSize.Large);
      safeZoneColor.DynamicTooltipGetter = (MyTerminalControl<MySafeZoneBlock>.TooltipGetter) (beacon => beacon.OnGetColorTooltip());
      safeZoneColor.Getter = (MyTerminalValueControl<MySafeZoneBlock, Color>.GetterDelegate) (beacon => beacon.m_safeZoneManager.GetColor());
      safeZoneColor.Setter = (MyTerminalValueControl<MySafeZoneBlock, Color>.SetterDelegate) ((beacon, color) => beacon.m_safeZoneManager.SetColor(color));
      MyTerminalControlFactory.AddControl<MySafeZoneBlock>((MyTerminalControl<MySafeZoneBlock>) safeZoneColor);
      MyTerminalControlCombobox<MySafeZoneBlock> safeZoneTextureCombo = new MyTerminalControlCombobox<MySafeZoneBlock>("SafeZoneTextureCombo", MySpaceTexts.SafeZone_Texture, MySpaceTexts.SafeZone_Texture_TTIP);
      safeZoneTextureCombo.ComboBoxContentWithBlock = (MyTerminalControlCombobox<MySafeZoneBlock>.ComboBoxContentDelegate) ((beacon, list) => beacon.GetTexturesList(list));
      safeZoneTextureCombo.Setter = (MyTerminalValueControl<MySafeZoneBlock, long>.SetterDelegate) ((beacon, key) => beacon.OnTextureSelected(key));
      safeZoneTextureCombo.Getter = (MyTerminalValueControl<MySafeZoneBlock, long>.GetterDelegate) (beacon => beacon.m_safeZoneManager.GetTexture());
      safeZoneTextureCombo.Visible = (Func<MySafeZoneBlock, bool>) (beacon => beacon.Definition.CubeSize == MyCubeSize.Large);
      safeZoneTextureCombo.Enabled = (Func<MySafeZoneBlock, bool>) (beacon => beacon.m_safeZoneManager.SafeZoneEntityId != 0L && this.m_dlcComponent.HasDLC("Economy", MySession.Static.LocalHumanPlayer.Id.SteamId));
      safeZoneTextureCombo.DynamicTooltipGetter = (MyTerminalControl<MySafeZoneBlock>.TooltipGetter) (beacon => beacon.OnGetTextureTooltip());
      MyTerminalControlFactory.AddControl<MySafeZoneBlock>((MyTerminalControl<MySafeZoneBlock>) safeZoneTextureCombo);
      MyTerminalControlFactory.AddControl<MySafeZoneBlock>((MyTerminalControl<MySafeZoneBlock>) new MyTerminalControlSeparator<MySafeZoneBlock>());
      MyMultiTextPanelComponent.CreateTerminalControls<MySafeZoneBlock>();
      if (Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      MySandboxGame.Static.Invoke((Action) (() =>
      {
        safeZoneOnOff.GetGuiControl().ShowTooltipWhenDisabled = true;
        safeZoneColor.GetGuiControl().ShowTooltipWhenDisabled = true;
        safeZoneTextureCombo.GetGuiControl().ShowTooltipWhenDisabled = true;
      }), "SafeZoneBlock - TerminalControl");
    }

    private void OnTextureSelected(long key)
    {
      IEnumerable<MySafeZoneTexturesDefinition> allDefinitions = MyDefinitionManager.Static.GetAllDefinitions<MySafeZoneTexturesDefinition>();
      if (allDefinitions == null)
      {
        MyLog.Default.Error("Textures definition for safe zone are missing. Without it, safezone wont work propertly.");
      }
      else
      {
        MyStringHash texture = MyStringHash.TryGet((int) key);
        bool flag = false;
        foreach (MySafeZoneTexturesDefinition texturesDefinition in allDefinitions)
        {
          if (texturesDefinition.DisplayTextId == texture)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          MyLog.Default.Error("Safe zone texture not found.");
        else
          this.m_safeZoneManager.SetTexture(texture);
      }
    }

    private void GetTexturesList(ICollection<MyTerminalControlComboBoxItem> list)
    {
      IEnumerable<MySafeZoneTexturesDefinition> allDefinitions = MyDefinitionManager.Static.GetAllDefinitions<MySafeZoneTexturesDefinition>();
      if (allDefinitions == null)
      {
        MyLog.Default.Error("Textures definition for safe zone are missing. Without it, safezone wont work propertly.");
      }
      else
      {
        foreach (MySafeZoneTexturesDefinition texturesDefinition in allDefinitions)
        {
          MyTerminalControlComboBoxItem controlComboBoxItem = new MyTerminalControlComboBoxItem()
          {
            Key = (long) (int) texturesDefinition.DisplayTextId,
            Value = MyStringId.GetOrCompute(texturesDefinition.DisplayTextId.String)
          };
          list.Add(controlComboBoxItem);
        }
      }
    }

    private void OnZonechipContentsChanged(MyInventoryBase obj)
    {
      this.m_safeZoneManager.SetActivate_Server(true);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      this.UpdateEffectsAndText();
    }

    private string OnGetTooltip() => new StringBuilder().AppendFormat(MySpaceTexts.Beacon_SafeZone_ToolTip, (object) this.Definition.SafeZoneActivationTimeS, (object) this.Definition.SafeZoneUpkeep, (object) this.Definition.SafeZoneUpkeepTimeM, this.Definition.SafeZoneActivationTimeS == 0U ? (object) "" : (object) MyTexts.GetString(MySpaceTexts.Beacon_SafeZone_ToolTip_PluralSuffix_Activation), this.Definition.SafeZoneUpkeep == 0U ? (object) "" : (object) MyTexts.GetString(MySpaceTexts.Beacon_SafeZone_ToolTip_PluralSuffix_ZoneChips), this.Definition.SafeZoneUpkeepTimeM == 0U ? (object) "" : (object) MyTexts.GetString(MySpaceTexts.Beacon_SafeZone_ToolTip_PluralSuffix_Minutes)).ToString();

    private string OnGetColorTooltip() => MyTexts.GetString(MySpaceTexts.SafeZone_Color_TTP);

    private string OnGetTextureTooltip() => !this.m_dlcComponent.HasDLC("Economy", MySession.Static.LocalHumanPlayer.Id.SteamId) ? MyTexts.GetString(MySpaceTexts.SafeZone_Texture_DLCReq_TTIP) : MyTexts.GetString(MySpaceTexts.SafeZone_Texture_TTIP);

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_SafeZoneBlock builderCubeBlock = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_SafeZoneBlock;
      if (!copy)
        builderCubeBlock.SafeZoneId = this.m_safeZoneManager.SafeZoneEntityId;
      if (this.m_multiPanel != null)
        builderCubeBlock.TextPanels = this.m_multiPanel.Serialize();
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      if (MyEntityExtensions.GetInventory(this) != null)
        MyEntityExtensions.GetInventory(this).ContentsChanged += new Action<MyInventoryBase>(this.OnZonechipContentsChanged);
      MySessionComponentSafeZones.OnSafeZoneUpdated += new Action<MySafeZone>(this.OnSafeZoneUpdated);
      this.CubeGrid.OnStaticChanged += new Action<MyCubeGrid, bool>(this.OnIsStaticChanged);
      this.IsWorkingChanged += new Action<MyCubeBlock>(this.OnIsWorkingChanged);
      this.m_safeZoneManager.SafeZoneChanged += new Action(this.OnSafeZoneChanged);
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      if (this.m_multiPanel != null && this.m_multiPanel.SurfaceCount > 0)
        this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      if (this.m_multiPanel == null)
        return;
      this.m_multiPanel.AddToScene();
    }

    public override void OnRemovedFromScene(object source)
    {
      base.OnRemovedFromScene(source);
      if (MyEntityExtensions.GetInventory(this) != null)
        MyEntityExtensions.GetInventory(this).ContentsChanged -= new Action<MyInventoryBase>(this.OnZonechipContentsChanged);
      this.ResourceSink.IsPoweredChanged -= new Action(this.Receiver_IsPoweredChanged);
      MySessionComponentSafeZones.OnSafeZoneUpdated -= new Action<MySafeZone>(this.OnSafeZoneUpdated);
      this.CubeGrid.OnStaticChanged -= new Action<MyCubeGrid, bool>(this.OnIsStaticChanged);
      this.IsWorkingChanged -= new Action<MyCubeBlock>(this.OnIsWorkingChanged);
      this.m_safeZoneManager.SafeZoneChanged -= new Action(this.OnSafeZoneChanged);
      this.SlimBlock.ComponentStack.IsFunctionalChanged -= new Action(this.ComponentStack_IsFunctionalChanged);
      if (!Sync.IsServer)
        return;
      this.m_safeZoneManager.SafeZoneRemove_Server();
    }

    private void ComponentStack_IsFunctionalChanged()
    {
      this.ResourceSink.Update();
      this.UpdateEffectsAndText();
    }

    private void OnSafezoneCreateRemove(bool turnOnSafeZone)
    {
      this.m_safeZoneManager.OnSafezoneCreateRemove_Request(turnOnSafeZone);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    private float UpdatePowerInput()
    {
      float powerDrain = this.m_safeZoneManager.GetPowerDrain();
      return !this.Enabled || !this.IsFunctional ? 0.0f : powerDrain;
    }

    private void OnSafeZoneChanged()
    {
      this.ResourceSink.Update();
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      this.UpdateEffectsAndText();
    }

    private void OnSafeZoneUpdated(MySafeZone obj)
    {
      if (this.m_safeZoneManager == null || this.m_safeZoneManager.SafeZoneEntityId != obj.EntityId)
        return;
      this.OnSafeZoneChanged();
    }

    private void Receiver_IsPoweredChanged()
    {
      this.UpdateIsWorking();
      this.UpdateEffectsAndText();
    }

    private void OnIsStaticChanged(MyCubeGrid grid, bool isStatic)
    {
      if (!isStatic && Sync.IsServer)
        this.m_safeZoneManager.SafeZoneRemove_Server();
      this.RaisePropertiesChanged();
    }

    private void OnIsWorkingChanged(MyCubeBlock obj)
    {
      if (this.CubeGrid == null || this.CubeGrid.MarkedForClose || !this.CubeGrid.IsStatic)
        return;
      if (Sync.IsServer && this.m_readyToRecieveEvents)
      {
        if (this.IsWorking)
          this.m_safeZoneManager.SafeZoneCreate_Server();
        else
          this.m_safeZoneManager.SafeZoneRemove_Server();
      }
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    public override void UpdateAfterSimulation100()
    {
      base.UpdateAfterSimulation100();
      bool flag = false;
      if (this.IsWorking && this.IsFunctional)
      {
        flag = this.m_safeZoneManager.Update();
        if (flag)
          this.UpdateEffectsAndText();
      }
      if (flag || !this.IsFunctional || this.HasDamageEffect)
        return;
      this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_100TH_FRAME;
    }

    protected override bool CheckIsWorking() => this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.CheckIsWorking();

    protected override void OnEnabledChanged()
    {
      this.ResourceSink.Update();
      base.OnEnabledChanged();
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      this.m_isSoundRunning = false;
      this.UpdateEffectsAndText();
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
      this.m_safeZoneManager.SetTextInfo(detailedInfo);
    }

    private void UpdateEffectsAndText()
    {
      if (this.m_soundEmitter != null)
      {
        bool flag = this.m_safeZoneManager.IsSafeZoneInWorld();
        if (flag != this.m_isSoundRunning)
        {
          if (flag)
            this.m_soundEmitter.PlaySound(this.m_processSound, true);
          else
            this.m_soundEmitter.StopSound(false);
          this.m_isSoundRunning = flag;
        }
      }
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    private void OnSafeZoneFilterBtnPressed() => this.m_safeZoneManager.OnSafeZoneFilterBtnPressed();

    public void InitializeConveyorEndpoint() => this.m_conveyorEndpoint = new MyMultilineConveyorEndpoint((MyCubeBlock) this);

    public bool AllowSelfPulling() => true;

    public PullInformation GetPullInformation()
    {
      MyInventory inventory = MyEntityExtensions.GetInventory(this);
      if (inventory == null)
        return (PullInformation) null;
      return new PullInformation()
      {
        OwnerID = this.OwnerId,
        Inventory = inventory,
        Constraint = inventory.Constraint
      };
    }

    public PullInformation GetPushInformation()
    {
      MyInventory inventory = MyEntityExtensions.GetInventory(this);
      if (inventory == null)
        return (PullInformation) null;
      return new PullInformation()
      {
        OwnerID = this.OwnerId,
        Inventory = inventory,
        Constraint = inventory.Constraint
      };
    }

    public override void UpdateAfterSimulation10()
    {
      base.UpdateAfterSimulation10();
      if (this.m_multiPanel == null)
        return;
      this.m_multiPanel.UpdateAfterSimulation(this.IsWorking);
    }

    private void UpdateScreen() => this.m_multiPanel?.UpdateScreen(this.IsWorking);

    private void SendAddImagesToSelectionRequest(int panelIndex, int[] selection) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MySafeZoneBlock, int, int[]>(this, (Func<MySafeZoneBlock, Action<int, int[]>>) (x => new Action<int, int[]>(x.OnSelectImageRequest)), panelIndex, selection);

    private void SendRemoveSelectedImageRequest(int panelIndex, int[] selection) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MySafeZoneBlock, int, int[]>(this, (Func<MySafeZoneBlock, Action<int, int[]>>) (x => new Action<int, int[]>(x.OnRemoveSelectedImageRequest)), panelIndex, selection);

    private void ChangeTextRequest(int panelIndex, string text) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MySafeZoneBlock, int, string>(this, (Func<MySafeZoneBlock, Action<int, string>>) (x => new Action<int, string>(x.OnChangeTextRequest)), panelIndex, text);

    [Event(null, 713)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnChangeTextRequest(int panelIndex, [Nullable] string text) => this.m_multiPanel?.ChangeText(panelIndex, text);

    private void UpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites)
    {
      if (!Sync.IsServer)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MySafeZoneBlock, int, MySerializableSpriteCollection>(this, (Func<MySafeZoneBlock, Action<int, MySerializableSpriteCollection>>) (x => new Action<int, MySerializableSpriteCollection>(x.OnUpdateSpriteCollection)), panelIndex, sprites);
    }

    [Event(null, 729)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    [DistanceRadius(32f)]
    private void OnUpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites) => this.m_multiPanel?.UpdateSpriteCollection(panelIndex, sprites);

    int Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider.SurfaceCount => this.m_multiPanel == null ? 0 : this.m_multiPanel.SurfaceCount;

    Sandbox.ModAPI.Ingame.IMyTextSurface Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider.GetSurface(
      int index)
    {
      return this.m_multiPanel == null ? (Sandbox.ModAPI.Ingame.IMyTextSurface) null : this.m_multiPanel.GetSurface(index);
    }

    [Event(null, 746)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnRemoveSelectedImageRequest(int panelIndex, int[] selection)
    {
      if (this.m_multiPanel == null)
        return;
      this.m_multiPanel.RemoveItems(panelIndex, selection);
    }

    [Event(null, 755)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnSelectImageRequest(int panelIndex, int[] selection)
    {
      if (this.m_multiPanel == null)
        return;
      this.m_multiPanel.SelectItems(panelIndex, selection);
    }

    void IMyMultiTextPanelComponentOwner.SelectPanel(
      List<MyGuiControlListbox.Item> panelItems)
    {
      if (this.m_multiPanel != null)
        this.m_multiPanel.SelectPanel((int) panelItems[0].UserData);
      this.RaisePropertiesChanged();
    }

    MyMultiTextPanelComponent IMyMultiTextPanelComponentOwner.MultiTextPanel => this.m_multiPanel;

    public MyTextPanelComponent PanelComponent => this.m_multiPanel == null ? (MyTextPanelComponent) null : this.m_multiPanel.PanelComponent;

    public void OpenWindow(bool isEditable, bool sync, bool isPublic)
    {
      if (sync)
      {
        this.SendChangeOpenMessage(true, isEditable, Sync.MyId, isPublic);
      }
      else
      {
        this.CreateTextBox(isEditable, new StringBuilder(this.PanelComponent.Text.ToString()), isPublic);
        MyGuiScreenGamePlay.TmpGameplayScreenHolder = MyGuiScreenGamePlay.ActiveGameplayScreen;
        MyScreenManager.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) this.m_textBoxMultiPanel);
      }
    }

    private void CreateTextBox(bool isEditable, StringBuilder description, bool isPublic)
    {
      string displayNameText = this.DisplayNameText;
      string displayName = this.PanelComponent.DisplayName;
      string description1 = description.ToString();
      bool flag = isEditable;
      Action<VRage.Game.ModAPI.ResultEnum> resultCallback = new Action<VRage.Game.ModAPI.ResultEnum>(this.OnClosedPanelTextBox);
      int num = flag ? 1 : 0;
      this.m_textBoxMultiPanel = new MyGuiScreenTextPanel(displayNameText, "", displayName, description1, resultCallback, editable: (num != 0));
    }

    public void OnClosedPanelTextBox(VRage.Game.ModAPI.ResultEnum result)
    {
      if (this.m_textBoxMultiPanel == null)
        return;
      if (this.m_textBoxMultiPanel.Description.Text.Length > 100000)
      {
        Action<MyGuiScreenMessageBox.ResultEnum> callback = new Action<MyGuiScreenMessageBox.ResultEnum>(this.OnClosedPanelMessageBox);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.MessageBoxTextTooLongText), callback: callback));
      }
      else
        this.CloseWindow(true);
    }

    public void OnClosedPanelMessageBox(MyGuiScreenMessageBox.ResultEnum result)
    {
      if (result == MyGuiScreenMessageBox.ResultEnum.YES)
      {
        this.m_textBoxMultiPanel.Description.Text.Remove(100000, this.m_textBoxMultiPanel.Description.Text.Length - 100000);
        this.CloseWindow(true);
      }
      else
      {
        this.CreateTextBox(true, this.m_textBoxMultiPanel.Description.Text, true);
        MyScreenManager.AddScreen((MyGuiScreenBase) this.m_textBoxMultiPanel);
      }
    }

    [Event(null, 835)]
    [Reliable]
    [Broadcast]
    private void OnChangeOpenSuccess(bool isOpen, bool editable, ulong user, bool isPublic) => this.OnChangeOpen(isOpen, editable, user, isPublic);

    private void SendChangeOpenMessage(bool isOpen, bool editable = false, ulong user = 0, bool isPublic = false) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MySafeZoneBlock, bool, bool, ulong, bool>(this, (Func<MySafeZoneBlock, Action<bool, bool, ulong, bool>>) (x => new Action<bool, bool, ulong, bool>(x.OnChangeOpenRequest)), isOpen, editable, user, isPublic);

    [Event(null, 846)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void OnChangeOpenRequest(bool isOpen, bool editable, ulong user, bool isPublic)
    {
      if (((!Sync.IsServer ? 0 : (this.IsTextPanelOpen ? 1 : 0)) & (isOpen ? 1 : 0)) != 0)
        return;
      this.OnChangeOpen(isOpen, editable, user, isPublic);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MySafeZoneBlock, bool, bool, ulong, bool>(this, (Func<MySafeZoneBlock, Action<bool, bool, ulong, bool>>) (x => new Action<bool, bool, ulong, bool>(x.OnChangeOpenSuccess)), isOpen, editable, user, isPublic);
    }

    private void OnChangeOpen(bool isOpen, bool editable, ulong user, bool isPublic)
    {
      this.IsTextPanelOpen = isOpen;
      if (((Sandbox.Engine.Platform.Game.IsDedicated ? 0 : ((long) user == (long) Sync.MyId ? 1 : 0)) & (isOpen ? 1 : 0)) == 0)
        return;
      this.OpenWindow(editable, false, isPublic);
    }

    public bool IsTextPanelOpen
    {
      get => this.m_isTextPanelOpen;
      set
      {
        if (this.m_isTextPanelOpen == value)
          return;
        this.m_isTextPanelOpen = value;
        this.RaisePropertiesChanged();
      }
    }

    private void CloseWindow(bool isPublic)
    {
      MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiScreenGamePlay.TmpGameplayScreenHolder;
      MyGuiScreenGamePlay.TmpGameplayScreenHolder = (MyGuiScreenBase) null;
      foreach (MySlimBlock cubeBlock in this.CubeGrid.CubeBlocks)
      {
        if (cubeBlock.FatBlock != null && cubeBlock.FatBlock.EntityId == this.EntityId)
        {
          this.SendChangeDescriptionMessage(this.m_textBoxMultiPanel.Description.Text, isPublic);
          this.SendChangeOpenMessage(false);
          break;
        }
      }
    }

    private void SendChangeDescriptionMessage(StringBuilder description, bool isPublic)
    {
      if (this.CubeGrid.IsPreview || !this.CubeGrid.SyncFlag)
      {
        this.PanelComponent.Text = description;
      }
      else
      {
        if (description.CompareTo(this.PanelComponent.Text) == 0)
          return;
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MySafeZoneBlock, string, bool>(this, (Func<MySafeZoneBlock, Action<string, bool>>) (x => new Action<string, bool>(x.OnChangeDescription)), description.ToString(), isPublic);
      }
    }

    [Event(null, 914)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    public void OnChangeDescription(string description, bool isPublic)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Clear().Append(description);
      this.PanelComponent.Text = stringBuilder;
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    public override void UpdateVisual()
    {
      base.UpdateVisual();
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    public override void OnModelChange()
    {
      base.OnModelChange();
      if (this.m_multiPanel != null)
        this.m_multiPanel.Reset();
      if (this.ResourceSink == null)
        return;
      this.UpdateScreen();
    }

    public override void OnRemovedByCubeBuilder()
    {
      this.ReleaseInventory(MyEntityExtensions.GetInventory(this));
      base.OnRemovedByCubeBuilder();
    }

    public override void OnDestroy()
    {
      this.ReleaseInventory(MyEntityExtensions.GetInventory(this), true);
      base.OnDestroy();
    }

    void SpaceEngineers.Game.ModAPI.IMySafeZoneBlock.EnableSafeZone(bool turnOn)
    {
      if (!this.CubeGrid.IsStatic)
        return;
      this.OnSafezoneCreateRemove(turnOn);
    }

    bool SpaceEngineers.Game.ModAPI.IMySafeZoneBlock.IsSafeZoneEnabled() => (ulong) this.m_safeZoneManager.SafeZoneEntityId > 0UL;

    protected sealed class OnChangeTextRequest\u003C\u003ESystem_Int32\u0023System_String : ICallSite<MySafeZoneBlock, int, string, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MySafeZoneBlock @this,
        in int panelIndex,
        in string text,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeTextRequest(panelIndex, text);
      }
    }

    protected sealed class OnUpdateSpriteCollection\u003C\u003ESystem_Int32\u0023VRage_Game_GUI_TextPanel_MySerializableSpriteCollection : ICallSite<MySafeZoneBlock, int, MySerializableSpriteCollection, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MySafeZoneBlock @this,
        in int panelIndex,
        in MySerializableSpriteCollection sprites,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnUpdateSpriteCollection(panelIndex, sprites);
      }
    }

    protected sealed class OnRemoveSelectedImageRequest\u003C\u003ESystem_Int32\u0023System_Int32\u003C\u0023\u003E : ICallSite<MySafeZoneBlock, int, int[], DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MySafeZoneBlock @this,
        in int panelIndex,
        in int[] selection,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnRemoveSelectedImageRequest(panelIndex, selection);
      }
    }

    protected sealed class OnSelectImageRequest\u003C\u003ESystem_Int32\u0023System_Int32\u003C\u0023\u003E : ICallSite<MySafeZoneBlock, int, int[], DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MySafeZoneBlock @this,
        in int panelIndex,
        in int[] selection,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnSelectImageRequest(panelIndex, selection);
      }
    }

    protected sealed class OnChangeOpenSuccess\u003C\u003ESystem_Boolean\u0023System_Boolean\u0023System_UInt64\u0023System_Boolean : ICallSite<MySafeZoneBlock, bool, bool, ulong, bool, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MySafeZoneBlock @this,
        in bool isOpen,
        in bool editable,
        in ulong user,
        in bool isPublic,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeOpenSuccess(isOpen, editable, user, isPublic);
      }
    }

    protected sealed class OnChangeOpenRequest\u003C\u003ESystem_Boolean\u0023System_Boolean\u0023System_UInt64\u0023System_Boolean : ICallSite<MySafeZoneBlock, bool, bool, ulong, bool, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MySafeZoneBlock @this,
        in bool isOpen,
        in bool editable,
        in ulong user,
        in bool isPublic,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeOpenRequest(isOpen, editable, user, isPublic);
      }
    }

    protected sealed class OnChangeDescription\u003C\u003ESystem_String\u0023System_Boolean : ICallSite<MySafeZoneBlock, string, bool, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MySafeZoneBlock @this,
        in string description,
        in bool isPublic,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeDescription(description, isPublic);
      }
    }
  }
}
