// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyRadioAntenna
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Gui;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
  [MyCubeBlockType(typeof (MyObjectBuilder_RadioAntenna))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyRadioAntenna), typeof (Sandbox.ModAPI.Ingame.IMyRadioAntenna)})]
  public class MyRadioAntenna : MyFunctionalBlock, IMyGizmoDrawableObject, Sandbox.ModAPI.IMyRadioAntenna, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyRadioAntenna
  {
    protected Color m_gizmoColor;
    protected const float m_maxGizmoDrawDistance = 10000f;
    private MyTuple<string, MyTransmitTarget>? m_nextBroadcast;
    private VRage.Sync.Sync<bool, SyncDirection.BothWays> m_ignoreOtherBroadcast;
    private VRage.Sync.Sync<bool, SyncDirection.BothWays> m_ignoreAlliedBroadcast;
    private static MyTerminalControlCheckbox<MyRadioAntenna> m_ignoreOtherCheckbox;
    private static MyTerminalControlCheckbox<MyRadioAntenna> m_ignoreAllyCheckbox;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_radius;
    private bool onceUpdated;
    public readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> EnableBroadcasting;
    private VRage.Sync.Sync<bool, SyncDirection.BothWays> m_showShipName;

    private static event Action<MyRadioAntenna, string, MyTransmitTarget> m_messageRequest;

    private MyRadioBroadcaster RadioBroadcaster
    {
      get => (MyRadioBroadcaster) this.Components.Get<MyDataBroadcaster>();
      set => this.Components.Add<MyDataBroadcaster>((MyDataBroadcaster) value);
    }

    private MyRadioReceiver RadioReceiver
    {
      get => (MyRadioReceiver) this.Components.Get<MyDataReceiver>();
      set => this.Components.Add<MyDataReceiver>((MyDataReceiver) value);
    }

    public bool ShowShipName
    {
      get => (bool) this.m_showShipName;
      set => this.m_showShipName.Value = value;
    }

    public StringBuilder HudText { get; private set; }

    protected override bool CanShowOnHud => false;

    private void SetHudText(StringBuilder text) => this.SetHudText(text.ToString());

    private void SetHudText(string text)
    {
      if (!this.HudText.CompareUpdate(text))
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyRadioAntenna, string>(this, (Func<MyRadioAntenna, Action<string>>) (x => new Action<string>(x.SetHudTextEvent)), text);
    }

    [Event(null, 102)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [BroadcastExcept]
    protected void SetHudTextEvent(string text) => this.HudText.CompareUpdate(text);

    public Color GetGizmoColor() => this.m_gizmoColor;

    public Vector3 GetPositionInGrid() => (Vector3) this.Position;

    public bool CanBeDrawn() => MyCubeGrid.ShowAntennaGizmos && this.IsWorking && (this.HasLocalPlayerAccess() && this.GetDistanceBetweenCameraAndBoundingSphere() <= 10000.0) && MyRadioAntenna.IsRecievedByPlayer((MyCubeBlock) this);

    public BoundingBox? GetBoundingBox() => new BoundingBox?();

    public float GetRadius() => this.RadioBroadcaster.BroadcastRadius;

    public MatrixD GetWorldMatrix() => this.PositionComp.WorldMatrixRef;

    public bool EnableLongDrawDistance() => true;

    public static bool IsRecievedByPlayer(MyCubeBlock cubeBlock)
    {
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      return localCharacter != null && MyAntennaSystem.Static.CheckConnection((MyDataReceiver) localCharacter.RadioReceiver, (MyEntity) cubeBlock, localCharacter.GetPlayerIdentityId(), false);
    }

    public MyRadioAntenna()
    {
      this.CreateTerminalControls();
      this.HudText = new StringBuilder();
      this.NeedsWorldMatrix = true;
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyRadioAntenna>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlFactory.RemoveBaseClass<MyRadioAntenna, MyTerminalBlock>();
      MyTerminalControlOnOffSwitch<MyRadioAntenna> controlOnOffSwitch1 = new MyTerminalControlOnOffSwitch<MyRadioAntenna>("ShowInTerminal", MySpaceTexts.Terminal_ShowInTerminal, MySpaceTexts.Terminal_ShowInTerminalToolTip);
      controlOnOffSwitch1.Getter = (MyTerminalValueControl<MyRadioAntenna, bool>.GetterDelegate) (x => x.ShowInTerminal);
      controlOnOffSwitch1.Setter = (MyTerminalValueControl<MyRadioAntenna, bool>.SetterDelegate) ((x, v) => x.ShowInTerminal = v);
      MyTerminalControlFactory.AddControl<MyRadioAntenna>((MyTerminalControl<MyRadioAntenna>) controlOnOffSwitch1);
      MyTerminalControlOnOffSwitch<MyRadioAntenna> controlOnOffSwitch2 = new MyTerminalControlOnOffSwitch<MyRadioAntenna>("ShowInToolbarConfig", MySpaceTexts.Terminal_ShowInToolbarConfig, MySpaceTexts.Terminal_ShowInToolbarConfigToolTip, max_Width: 0.25f, is_AutoEllipsisEnabled: true, is_AutoScaleEnabled: true);
      controlOnOffSwitch2.Getter = (MyTerminalValueControl<MyRadioAntenna, bool>.GetterDelegate) (x => x.ShowInToolbarConfig);
      controlOnOffSwitch2.Setter = (MyTerminalValueControl<MyRadioAntenna, bool>.SetterDelegate) ((x, v) => x.ShowInToolbarConfig = v);
      MyTerminalControlFactory.AddControl<MyRadioAntenna>((MyTerminalControl<MyRadioAntenna>) controlOnOffSwitch2);
      MyTerminalControlButton<MyRadioAntenna> terminalControlButton = new MyTerminalControlButton<MyRadioAntenna>("CustomData", MySpaceTexts.Terminal_CustomData, MySpaceTexts.Terminal_CustomDataTooltip, new Action<MyRadioAntenna>(MyTerminalBlock.CustomDataClicked));
      terminalControlButton.Enabled = (Func<MyRadioAntenna, bool>) (x => !x.m_textboxOpen);
      terminalControlButton.SupportsMultipleBlocks = false;
      MyTerminalControlFactory.AddControl<MyRadioAntenna>((MyTerminalControl<MyRadioAntenna>) terminalControlButton);
      MyTerminalControlTextbox<MyRadioAntenna> terminalControlTextbox1 = new MyTerminalControlTextbox<MyRadioAntenna>("CustomName", MyCommonTexts.Name, MySpaceTexts.Blank);
      terminalControlTextbox1.Getter = (MyTerminalControlTextbox<MyRadioAntenna>.GetterDelegate) (x => x.CustomName);
      terminalControlTextbox1.Setter = (MyTerminalControlTextbox<MyRadioAntenna>.SetterDelegate) ((x, v) => x.SetCustomName(v));
      terminalControlTextbox1.SupportsMultipleBlocks = false;
      MyTerminalControlFactory.AddControl<MyRadioAntenna>((MyTerminalControl<MyRadioAntenna>) terminalControlTextbox1);
      MyTerminalControlFactory.AddControl<MyRadioAntenna>((MyTerminalControl<MyRadioAntenna>) new MyTerminalControlSeparator<MyRadioAntenna>());
      MyTerminalControlTextbox<MyRadioAntenna> terminalControlTextbox2 = new MyTerminalControlTextbox<MyRadioAntenna>("HudText", MySpaceTexts.BlockPropertiesTitle_HudText, MySpaceTexts.Antenna_HudTextToolTip);
      terminalControlTextbox2.Getter = (MyTerminalControlTextbox<MyRadioAntenna>.GetterDelegate) (x => x.HudText);
      terminalControlTextbox2.Setter = (MyTerminalControlTextbox<MyRadioAntenna>.SetterDelegate) ((x, v) => x.SetHudText(v));
      terminalControlTextbox2.SupportsMultipleBlocks = false;
      MyTerminalControlFactory.AddControl<MyRadioAntenna>((MyTerminalControl<MyRadioAntenna>) terminalControlTextbox2);
      MyTerminalControlFactory.AddControl<MyRadioAntenna>((MyTerminalControl<MyRadioAntenna>) new MyTerminalControlSeparator<MyRadioAntenna>());
      MyTerminalControlSlider<MyRadioAntenna> slider = new MyTerminalControlSlider<MyRadioAntenna>("Radius", MySpaceTexts.BlockPropertyTitle_BroadcastRadius, MySpaceTexts.BlockPropertyDescription_BroadcastRadius);
      slider.SetLogLimits((MyTerminalValueControl<MyRadioAntenna, float>.GetterDelegate) (block => 1f), (MyTerminalValueControl<MyRadioAntenna, float>.GetterDelegate) (block => (block.BlockDefinition as MyRadioAntennaDefinition).MaxBroadcastRadius));
      slider.DefaultValueGetter = (MyTerminalValueControl<MyRadioAntenna, float>.GetterDelegate) (x => (x.BlockDefinition as MyRadioAntennaDefinition).MaxBroadcastRadius / 10f);
      slider.Getter = (MyTerminalValueControl<MyRadioAntenna, float>.GetterDelegate) (x => x.RadioBroadcaster.BroadcastRadius);
      slider.Setter = (MyTerminalValueControl<MyRadioAntenna, float>.SetterDelegate) ((x, v) => x.m_radius.Value = v);
      slider.Writer = (MyTerminalControl<MyRadioAntenna>.WriterDelegate) ((x, result) =>
      {
        if (x.RadioBroadcaster == null)
          return;
        result.Append((object) new StringBuilder().AppendDecimal(x.RadioBroadcaster.BroadcastRadius, 0).Append(" m"));
      });
      slider.EnableActions<MyRadioAntenna>();
      MyTerminalControlFactory.AddControl<MyRadioAntenna>((MyTerminalControl<MyRadioAntenna>) slider);
      MyTerminalControlCheckbox<MyRadioAntenna> checkbox1 = new MyTerminalControlCheckbox<MyRadioAntenna>("EnableBroadCast", MySpaceTexts.Antenna_EnableBroadcast, MySpaceTexts.Antenna_EnableBroadcast);
      checkbox1.Getter = (MyTerminalValueControl<MyRadioAntenna, bool>.GetterDelegate) (x => x.EnableBroadcasting.Value);
      checkbox1.Setter = (MyTerminalValueControl<MyRadioAntenna, bool>.SetterDelegate) ((x, v) => x.EnableBroadcasting.Value = v);
      checkbox1.EnableAction<MyRadioAntenna>();
      MyTerminalControlFactory.AddControl<MyRadioAntenna>((MyTerminalControl<MyRadioAntenna>) checkbox1);
      MyTerminalControlCheckbox<MyRadioAntenna> checkbox2 = new MyTerminalControlCheckbox<MyRadioAntenna>("ShowShipName", MySpaceTexts.BlockPropertyTitle_ShowShipName, MySpaceTexts.BlockPropertyDescription_ShowShipName);
      checkbox2.Getter = (MyTerminalValueControl<MyRadioAntenna, bool>.GetterDelegate) (x => x.ShowShipName);
      checkbox2.Setter = (MyTerminalValueControl<MyRadioAntenna, bool>.SetterDelegate) ((x, v) => x.ShowShipName = v);
      checkbox2.EnableAction<MyRadioAntenna>();
      MyTerminalControlFactory.AddControl<MyRadioAntenna>((MyTerminalControl<MyRadioAntenna>) checkbox2);
      MyTerminalControlFactory.AddControl<MyRadioAntenna>((MyTerminalControl<MyRadioAntenna>) new MyTerminalControlSeparator<MyRadioAntenna>());
    }

    private void ChangeRadius()
    {
      if (this.RadioBroadcaster == null)
        return;
      this.RadioBroadcaster.BroadcastRadius = (float) this.m_radius;
      this.RadioBroadcaster.RaiseBroadcastRadiusChanged();
    }

    private void ChangeEnableBroadcast()
    {
      if (this.RadioBroadcaster == null)
        return;
      this.RadioBroadcaster.Enabled = (bool) this.EnableBroadcasting && this.IsWorking;
      this.RadioBroadcaster.WantsToBeEnabled = (bool) this.EnableBroadcasting;
      this.ResourceSink.Update();
      this.RaisePropertiesChanged();
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    private void OnShowShipNameChanged()
    {
      this.RadioBroadcaster.RaiseAntennaNameChanged((MyTerminalBlock) this);
      if ((bool) this.m_showShipName)
        this.CubeGrid.OnNameChanged += new Action<MyCubeGrid>(this.OnShipNameChanged);
      else
        this.CubeGrid.OnNameChanged -= new Action<MyCubeGrid>(this.OnShipNameChanged);
    }

    private void OnShipNameChanged(MyCubeGrid grid) => this.RadioBroadcaster.RaiseAntennaNameChanged((MyTerminalBlock) this);

    public override void OnCubeGridChanged(MyCubeGrid oldGrid)
    {
      if ((bool) this.m_showShipName)
      {
        oldGrid.OnNameChanged -= new Action<MyCubeGrid>(this.OnShipNameChanged);
        this.CubeGrid.OnNameChanged += new Action<MyCubeGrid>(this.OnShipNameChanged);
        this.RadioBroadcaster.RaiseAntennaNameChanged((MyTerminalBlock) this);
      }
      base.OnCubeGridChanged(oldGrid);
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.RadioBroadcaster = new MyRadioBroadcaster();
      this.RadioReceiver = new MyRadioReceiver();
      MyRadioAntennaDefinition blockDefinition = this.BlockDefinition as MyRadioAntennaDefinition;
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(blockDefinition.ResourceSinkGroup, 1f / 500f, new Func<float>(this.UpdatePowerInput));
      resourceSinkComponent.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
      this.ResourceSink = resourceSinkComponent;
      this.IsWorkingChanged += new Action<MyCubeBlock>(this.OnIsWorkingChanged);
      base.Init(objectBuilder, cubeGrid);
      MyObjectBuilder_RadioAntenna builderRadioAntenna = (MyObjectBuilder_RadioAntenna) objectBuilder;
      this.RadioBroadcaster.BroadcastRadius = (double) builderRadioAntenna.BroadcastRadius <= 0.0 ? blockDefinition.MaxBroadcastRadius / 10f : builderRadioAntenna.BroadcastRadius;
      this.HudText.Clear();
      if (builderRadioAntenna.HudText != null)
        this.HudText.Append(builderRadioAntenna.HudText);
      this.RadioBroadcaster.BroadcastRadius = MathHelper.Clamp(this.RadioBroadcaster.BroadcastRadius, 1f, blockDefinition.MaxBroadcastRadius);
      this.ResourceSink.Update();
      this.RadioBroadcaster.WantsToBeEnabled = builderRadioAntenna.EnableBroadcasting;
      this.m_showShipName.SetLocalValue(builderRadioAntenna.ShowShipName);
      this.m_ignoreOtherBroadcast.SetLocalValue(builderRadioAntenna.IgnoreOther);
      this.m_ignoreAlliedBroadcast.SetLocalValue(builderRadioAntenna.IgnoreAllied);
      this.ShowOnHUD = false;
      this.m_gizmoColor = (Color) new Vector4(0.2f, 0.2f, 0.0f, 0.5f);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      this.m_radius.ValueChanged += (Action<SyncBase>) (obj => this.ChangeRadius());
      this.EnableBroadcasting.ValueChanged += (Action<SyncBase>) (obj => this.ChangeEnableBroadcast());
      this.m_showShipName.ValueChanged += (Action<SyncBase>) (obj => this.OnShowShipNameChanged());
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.EnableBroadcasting.Value = this.RadioBroadcaster.WantsToBeEnabled;
      this.RadioBroadcaster.OnBroadcastRadiusChanged += new Action(this.OnBroadcastRadiusChanged);
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      if (!(bool) this.m_showShipName)
        return;
      this.CubeGrid.OnNameChanged += new Action<MyCubeGrid>(this.OnShipNameChanged);
    }

    public override void OnRemovedFromScene(object source)
    {
      base.OnRemovedFromScene(source);
      this.RadioBroadcaster.OnBroadcastRadiusChanged -= new Action(this.OnBroadcastRadiusChanged);
      this.SlimBlock.ComponentStack.IsFunctionalChanged -= new Action(this.ComponentStack_IsFunctionalChanged);
      if (!(bool) this.m_showShipName)
        return;
      this.CubeGrid.OnNameChanged -= new Action<MyCubeGrid>(this.OnShipNameChanged);
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      if (Sandbox.Game.Multiplayer.Sync.IsServer && !this.onceUpdated)
      {
        this.IsWorkingChanged += new Action<MyCubeBlock>(this.UpdatePirateAntenna);
        this.CustomNameChanged += new Action<MyTerminalBlock>(this.UpdatePirateAntenna);
        this.OwnershipChanged += new Action<MyTerminalBlock>(this.UpdatePirateAntenna);
        this.UpdatePirateAntenna((MyCubeBlock) this);
      }
      this.onceUpdated = true;
      if (!this.m_nextBroadcast.HasValue)
        return;
      Action<MyRadioAntenna, string, MyTransmitTarget> messageRequest = MyRadioAntenna.m_messageRequest;
      if (messageRequest != null)
        messageRequest(this, this.m_nextBroadcast.Value.Item1, this.m_nextBroadcast.Value.Item2);
      this.m_nextBroadcast = new MyTuple<string, MyTransmitTarget>?();
    }

    protected override void Closing()
    {
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.UpdatePirateAntenna(true);
      base.Closing();
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_RadioAntenna builderCubeBlock = (MyObjectBuilder_RadioAntenna) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.BroadcastRadius = this.RadioBroadcaster.BroadcastRadius;
      builderCubeBlock.ShowShipName = this.ShowShipName;
      builderCubeBlock.EnableBroadcasting = this.RadioBroadcaster.WantsToBeEnabled;
      builderCubeBlock.HudText = this.HudText.ToString();
      builderCubeBlock.IgnoreAllied = this.m_ignoreAlliedBroadcast.Value;
      builderCubeBlock.IgnoreOther = this.m_ignoreOtherBroadcast.Value;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public override void UpdateAfterSimulation10()
    {
      base.UpdateAfterSimulation10();
      this.RadioReceiver.UpdateBroadcastersInRange();
    }

    protected override void WorldPositionChanged(object source)
    {
      base.WorldPositionChanged(source);
      if (this.RadioBroadcaster == null)
        return;
      this.RadioBroadcaster.MoveBroadcaster();
    }

    public override List<MyHudEntityParams> GetHudParams(bool allowBlink)
    {
      this.m_hudParams.Clear();
      if (this.CubeGrid == null || this.CubeGrid.MarkedForClose || this.CubeGrid.Closed)
        return this.m_hudParams;
      if (this.IsWorking)
      {
        List<MyHudEntityParams> hudParams = base.GetHudParams(allowBlink && this.HasLocalPlayerAccess());
        StringBuilder hudText = this.HudText;
        if (this.ShowShipName || hudText.Length > 0)
        {
          StringBuilder text = hudParams[0].Text;
          text.Clear();
          if (!string.IsNullOrEmpty(this.GetOwnerFactionTag()))
          {
            text.Append(this.GetOwnerFactionTag());
            text.Append(".");
          }
          if (this.ShowShipName)
          {
            text.Append(this.CubeGrid.DisplayName);
            text.Append(" - ");
          }
          text.Append((object) hudText);
        }
        this.m_hudParams.AddRange((IEnumerable<MyHudEntityParams>) hudParams);
        if (this.HasLocalPlayerAccess() && this.SlimBlock.CubeGrid.GridSystems.TerminalSystem != null)
        {
          this.SlimBlock.CubeGrid.GridSystems.TerminalSystem.NeedsHudUpdate = true;
          foreach (MyTerminalBlock hudBlock in this.SlimBlock.CubeGrid.GridSystems.TerminalSystem.HudBlocks)
          {
            if (hudBlock != this)
              this.m_hudParams.AddRange((IEnumerable<MyHudEntityParams>) hudBlock.GetHudParams(true));
          }
        }
        MyEntityController entityController = MySession.Static.Players.GetEntityController((MyEntity) this.CubeGrid);
        if (entityController != null && entityController.ControlledEntity is MyCockpit controlledEntity && controlledEntity.Pilot != null)
          this.m_hudParams.AddRange((IEnumerable<MyHudEntityParams>) controlledEntity.GetHudParams(true));
      }
      return this.m_hudParams;
    }

    private void UpdatePirateAntenna(MyCubeBlock obj) => this.UpdatePirateAntenna();

    public void UpdatePirateAntenna(bool remove = false)
    {
      bool activeState = this.IsWorking && Sandbox.Game.Multiplayer.Sync.Players.GetNPCIdentities().Contains(this.OwnerId);
      MyPirateAntennas.UpdatePirateAntenna(this.EntityId, remove, activeState, this.HudText.Length > 0 ? this.HudText : this.CustomName);
    }

    protected override void OnEnabledChanged()
    {
      base.OnEnabledChanged();
      this.UpdateEnabled();
    }

    protected void OnIsWorkingChanged(MyCubeBlock block) => this.UpdateEnabled();

    protected void UpdateEnabled()
    {
      this.ResourceSink.Update();
      if (this.onceUpdated)
      {
        this.RadioReceiver.Enabled = this.IsWorking;
        this.RadioBroadcaster.Enabled = (bool) this.EnableBroadcasting && this.IsWorking;
        this.RadioBroadcaster.WantsToBeEnabled = (bool) this.EnableBroadcasting;
        this.RadioReceiver.UpdateBroadcastersInRange();
      }
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    protected override bool CheckIsWorking() => this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.CheckIsWorking();

    private void Receiver_IsPoweredChanged()
    {
      this.UpdateIsWorking();
      if (this.RadioBroadcaster != null)
        this.RadioBroadcaster.Enabled = this.IsWorking && (bool) this.EnableBroadcasting;
      if (this.RadioReceiver != null)
        this.RadioReceiver.Enabled = this.IsWorking;
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    private void ComponentStack_IsFunctionalChanged()
    {
      this.ResourceSink.Update();
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    private void OnBroadcastRadiusChanged()
    {
      this.ResourceSink.Update();
      this.RaisePropertiesChanged();
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    private float UpdatePowerInput()
    {
      float num = (float) (((bool) this.EnableBroadcasting ? (double) this.RadioBroadcaster.BroadcastRadius : 1.0) / 500.0);
      return !this.Enabled || !this.IsFunctional ? 0.0f : num * (1f / 500f);
    }

    protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
    {
      base.UpdateDetailedInfo(detailedInfo);
      detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
      detailedInfo.Append(this.BlockDefinition.DisplayNameText);
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyProperties_CurrentInput));
      MyValueFormatter.AppendWorkInBestUnit(this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) ? this.ResourceSink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId) : 0.0f, detailedInfo);
    }

    float Sandbox.ModAPI.Ingame.IMyRadioAntenna.Radius
    {
      get => this.GetRadius();
      set => this.m_radius.Value = MathHelper.Clamp(value, 0.0f, ((MyRadioAntennaDefinition) this.BlockDefinition).MaxBroadcastRadius);
    }

    string Sandbox.ModAPI.Ingame.IMyRadioAntenna.HudText
    {
      get => this.HudText.ToString();
      set => this.SetHudText(value);
    }

    private bool IsBroadcasting() => this.RadioBroadcaster != null && this.RadioBroadcaster.WantsToBeEnabled;

    protected override void OnOwnershipChanged()
    {
      base.OnOwnershipChanged();
      this.RadioBroadcaster.RaiseOwnerChanged();
    }

    bool Sandbox.ModAPI.Ingame.IMyRadioAntenna.IsBroadcasting => this.IsBroadcasting();

    bool Sandbox.ModAPI.Ingame.IMyRadioAntenna.EnableBroadcasting
    {
      get => this.EnableBroadcasting.Value;
      set => this.EnableBroadcasting.Value = value;
    }

    public float GetRodRadius()
    {
      if (!(this.BlockDefinition is MyRadioAntennaDefinition))
        return 0.0f;
      return this.CubeGrid.GridSizeEnum != MyCubeSize.Large ? ((MyRadioAntennaDefinition) this.BlockDefinition).LightningRodRadiusSmall : ((MyRadioAntennaDefinition) this.BlockDefinition).LightningRodRadiusLarge;
    }

    protected sealed class SetHudTextEvent\u003C\u003ESystem_String : ICallSite<MyRadioAntenna, string, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyRadioAntenna @this,
        in string text,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.SetHudTextEvent(text);
      }
    }

    protected class m_ignoreOtherBroadcast\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyRadioAntenna) obj0).m_ignoreOtherBroadcast = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_ignoreAlliedBroadcast\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyRadioAntenna) obj0).m_ignoreAlliedBroadcast = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_radius\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyRadioAntenna) obj0).m_radius = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class EnableBroadcasting\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyRadioAntenna) obj0).EnableBroadcasting = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_showShipName\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyRadioAntenna) obj0).m_showShipName = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_Cube_MyRadioAntenna\u003C\u003EActor : IActivator, IActivator<MyRadioAntenna>
    {
      object IActivator.CreateInstance() => (object) new MyRadioAntenna();

      MyRadioAntenna IActivator<MyRadioAntenna>.CreateInstance() => new MyRadioAntenna();
    }
  }
}
