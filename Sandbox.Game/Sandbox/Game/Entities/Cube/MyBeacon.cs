// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyBeacon
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Lights;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Graphics;
using VRage.Game.Gui;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender.Lights;

namespace Sandbox.Game.Entities.Cube
{
  [MyCubeBlockType(typeof (MyObjectBuilder_Beacon))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyBeacon), typeof (Sandbox.ModAPI.Ingame.IMyBeacon)})]
  public class MyBeacon : MyFunctionalBlock, Sandbox.ModAPI.IMyBeacon, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyBeacon
  {
    private static readonly Color COLOR_ON = new Color((int) byte.MaxValue, (int) byte.MaxValue, 128);
    private static readonly Color COLOR_OFF = new Color(30, 30, 30);
    private static readonly float POINT_LIGHT_RANGE_SMALL = 2f;
    private static readonly float POINT_LIGHT_RANGE_LARGE = 7.5f;
    private static readonly float POINT_LIGHT_INTENSITY_SMALL = 1f;
    private static readonly float POINT_LIGHT_INTENSITY_LARGE = 1f;
    private static readonly float GLARE_MAX_DISTANCE = 10000f;
    private const float LIGHT_TURNING_ON_TIME_IN_SECONDS = 0.5f;
    private bool m_largeLight;
    private MyLight m_light;
    private Vector3 m_lightPositionOffset;
    private float m_currentLightPower;
    private int m_lastAnimationUpdateTime;
    private bool m_restartTimeMeasure;
    private MyFlareDefinition m_flare;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_radius;
    private bool m_animationRunning;

    public StringBuilder HudText { get; private set; }

    protected override bool CanShowOnHud => false;

    internal MyRadioBroadcaster RadioBroadcaster
    {
      get => (MyRadioBroadcaster) this.Components.Get<MyDataBroadcaster>();
      private set => this.Components.Add<MyDataBroadcaster>((MyDataBroadcaster) value);
    }

    internal MyBeaconDefinition Definition => (MyBeaconDefinition) this.BlockDefinition;

    public MyBeacon()
    {
      this.CreateTerminalControls();
      this.HudText = new StringBuilder();
      this.m_radius.ValueChanged += (Action<SyncBase>) (obj => this.ChangeRadius());
      this.NeedsWorldMatrix = true;
    }

    private void ChangeRadius()
    {
      this.RadioBroadcaster.BroadcastRadius = (float) this.m_radius;
      this.RadioBroadcaster.RaiseBroadcastRadiusChanged();
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyBeacon>())
        return;
      base.CreateTerminalControls();
      MyUniqueList<ITerminalControl> controls = MyTerminalControlFactory.GetList(typeof (MyBeacon)).Controls;
      controls.Remove(controls[5]);
      MyTerminalControlTextbox<MyBeacon> terminalControlTextbox1 = new MyTerminalControlTextbox<MyBeacon>("CustomName", MyCommonTexts.Name, MySpaceTexts.Blank);
      terminalControlTextbox1.Getter = (MyTerminalControlTextbox<MyBeacon>.GetterDelegate) (x => x.CustomName);
      terminalControlTextbox1.Setter = (MyTerminalControlTextbox<MyBeacon>.SetterDelegate) ((x, v) => x.SetCustomName(v));
      terminalControlTextbox1.SupportsMultipleBlocks = false;
      MyTerminalControlFactory.AddControl<MyBeacon>((MyTerminalControl<MyBeacon>) terminalControlTextbox1);
      MyTerminalControlFactory.AddControl<MyBeacon>((MyTerminalControl<MyBeacon>) new MyTerminalControlSeparator<MyBeacon>());
      MyTerminalControlTextbox<MyBeacon> terminalControlTextbox2 = new MyTerminalControlTextbox<MyBeacon>("HudText", MySpaceTexts.BlockPropertiesTitle_HudText, MySpaceTexts.BlockPropertiesTitle_HudText_Tooltip);
      terminalControlTextbox2.Getter = (MyTerminalControlTextbox<MyBeacon>.GetterDelegate) (x => x.HudText);
      terminalControlTextbox2.Setter = (MyTerminalControlTextbox<MyBeacon>.SetterDelegate) ((x, v) => x.SetHudText(v));
      terminalControlTextbox2.SupportsMultipleBlocks = false;
      MyTerminalControlFactory.AddControl<MyBeacon>((MyTerminalControl<MyBeacon>) terminalControlTextbox2);
      MyTerminalControlSlider<MyBeacon> slider = new MyTerminalControlSlider<MyBeacon>("Radius", MySpaceTexts.BlockPropertyTitle_BroadcastRadius, MySpaceTexts.BlockPropertyDescription_BroadcastRadius);
      slider.SetLogLimits((MyTerminalValueControl<MyBeacon, float>.GetterDelegate) (x => 1f), (MyTerminalValueControl<MyBeacon, float>.GetterDelegate) (x => x.Definition.MaxBroadcastRadius));
      slider.DefaultValueGetter = (MyTerminalValueControl<MyBeacon, float>.GetterDelegate) (x => x.Definition.MaxBroadcastRadius / 10f);
      slider.Getter = (MyTerminalValueControl<MyBeacon, float>.GetterDelegate) (x => x.RadioBroadcaster.BroadcastRadius);
      slider.Setter = (MyTerminalValueControl<MyBeacon, float>.SetterDelegate) ((x, v) => x.m_radius.Value = v);
      slider.Writer = (MyTerminalControl<MyBeacon>.WriterDelegate) ((x, result) => result.AppendDecimal(x.RadioBroadcaster.BroadcastRadius, 0).Append(" m"));
      slider.EnableActions<MyBeacon>();
      MyTerminalControlFactory.AddControl<MyBeacon>((MyTerminalControl<MyBeacon>) slider);
    }

    private void SetHudText(StringBuilder text) => this.SetHudText(text.ToString());

    private void SetHudText(string text)
    {
      if (!this.HudText.CompareUpdate(text))
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyBeacon, string>(this, (Func<MyBeacon, Action<string>>) (x => new Action<string>(x.SetHudTextEvent)), text);
    }

    [Event(null, 128)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [BroadcastExcept]
    protected void SetHudTextEvent(string text) => this.HudText.CompareUpdate(text);

    internal bool AnimationRunning
    {
      get => this.m_animationRunning;
      private set
      {
        if (this.m_animationRunning == value)
          return;
        this.m_animationRunning = value;
        if (!value)
          return;
        this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
        this.m_lastAnimationUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      }
    }

    protected override bool CheckIsWorking() => this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.CheckIsWorking();

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      if (this.Definition.EmissiveColorPreset == MyStringHash.NullOrEmpty)
        this.Definition.EmissiveColorPreset = MyStringHash.GetOrCompute("Beacon");
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(MyStringHash.GetOrCompute(this.Definition.ResourceSinkGroup), this.Definition.MaxBroadcastPowerDrainkW, new Func<float>(this.UpdatePowerInput));
      this.ResourceSink = resourceSinkComponent;
      this.RadioBroadcaster = new MyRadioBroadcaster(this.Definition.MaxBroadcastRadius / 10f);
      MyObjectBuilder_Beacon objectBuilderBeacon = (MyObjectBuilder_Beacon) objectBuilder;
      if ((double) objectBuilderBeacon.BroadcastRadius > 0.0)
        this.RadioBroadcaster.BroadcastRadius = objectBuilderBeacon.BroadcastRadius;
      this.RadioBroadcaster.BroadcastRadius = MathHelper.Clamp(this.RadioBroadcaster.BroadcastRadius, 1f, this.Definition.MaxBroadcastRadius);
      this.HudText.Clear();
      if (objectBuilderBeacon.HudText != null)
        this.HudText.Append(objectBuilderBeacon.HudText);
      base.Init(objectBuilder, cubeGrid);
      resourceSinkComponent.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
      resourceSinkComponent.Update();
      this.RadioBroadcaster.OnBroadcastRadiusChanged += new Action(this.OnBroadcastRadiusChanged);
      this.m_largeLight = cubeGrid.GridSizeEnum == MyCubeSize.Large;
      this.m_light = MyLights.AddLight();
      if (this.m_light != null)
      {
        this.m_light.Start(this.DisplayNameText);
        this.m_light.Range = this.m_largeLight ? 2f : 0.3f;
        this.m_light.GlareOn = false;
        this.m_light.GlareQuerySize = this.m_largeLight ? 1.5f : 0.3f;
        this.m_light.GlareQueryShift = this.m_largeLight ? 1f : 0.2f;
        this.m_light.GlareType = MyGlareTypeEnum.Normal;
        this.m_light.GlareMaxDistance = MyBeacon.GLARE_MAX_DISTANCE;
        if (!(MyDefinitionManager.Static.GetDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_FlareDefinition), this.Definition.Flare)) is MyFlareDefinition myFlareDefinition))
          myFlareDefinition = new MyFlareDefinition();
        this.m_flare = myFlareDefinition;
        this.m_light.GlareIntensity = this.m_flare.Intensity;
        this.m_light.GlareSize = this.m_flare.Size;
        this.m_light.SubGlares = this.m_flare.SubGlares;
      }
      this.m_lightPositionOffset = this.m_largeLight ? new Vector3(0.0f, this.CubeGrid.GridSize * 0.3f, 0.0f) : Vector3.Zero;
      this.UpdateLightPosition();
      this.m_restartTimeMeasure = false;
      this.AnimationRunning = true;
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.IsWorkingChanged += new Action<MyCubeBlock>(this.MyBeacon_IsWorkingChanged);
      this.ShowOnHUD = false;
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    private void OnUpdatePower()
    {
      this.ResourceSink.Update();
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    public override void OnRemovedFromScene(object source)
    {
      base.OnRemovedFromScene(source);
      this.IsWorkingChanged -= new Action<MyCubeBlock>(this.MyBeacon_IsWorkingChanged);
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_Beacon builderCubeBlock = (MyObjectBuilder_Beacon) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.HudText = this.HudText.ToString();
      builderCubeBlock.BroadcastRadius = this.RadioBroadcaster.BroadcastRadius;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    private void MyBeacon_IsWorkingChanged(MyCubeBlock obj)
    {
      if (this.RadioBroadcaster != null)
        this.RadioBroadcaster.Enabled = this.IsWorking;
      if (MyFakes.ENABLE_RADIO_HUD)
        return;
      if (this.IsWorking)
        MyHud.LocationMarkers.RegisterMarker((MyEntity) this, new MyHudEntityParams()
        {
          FlagsEnum = MyHudIndicatorFlagsEnum.SHOW_ALL,
          Text = this.HudText.Length > 0 ? this.HudText : this.CustomName
        });
      else
        MyHud.LocationMarkers.UnregisterMarker((MyEntity) this);
    }

    public override List<MyHudEntityParams> GetHudParams(bool allowBlink)
    {
      this.m_hudParams.Clear();
      if (this.CubeGrid == null || this.CubeGrid.MarkedForClose || this.CubeGrid.Closed)
        return this.m_hudParams;
      if (this.IsWorking)
      {
        List<MyHudEntityParams> hudParams = base.GetHudParams(allowBlink);
        StringBuilder hudText = this.HudText;
        if (hudText.Length > 0)
        {
          StringBuilder text = hudParams[0].Text;
          text.Clear();
          if (!string.IsNullOrEmpty(this.GetOwnerFactionTag()))
          {
            text.Append(this.GetOwnerFactionTag());
            text.Append(".");
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
      }
      return this.m_hudParams;
    }

    private void Receiver_IsPoweredChanged()
    {
      if (this.RadioBroadcaster != null)
        this.RadioBroadcaster.Enabled = this.IsWorking;
      this.UpdatePower();
      this.UpdateLightProperties();
      this.UpdateIsWorking();
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    private void ComponentStack_IsFunctionalChanged()
    {
      this.ResourceSink.Update();
      this.UpdatePower();
      this.UpdateLightProperties();
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    protected override void OnStartWorking()
    {
      base.OnStartWorking();
      if ((this.NeedsUpdate & MyEntityUpdateEnum.EACH_FRAME) == MyEntityUpdateEnum.NONE)
        this.m_restartTimeMeasure = true;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
    }

    protected override void OnStopWorking()
    {
      base.OnStopWorking();
      if ((this.NeedsUpdate & MyEntityUpdateEnum.EACH_FRAME) == MyEntityUpdateEnum.NONE)
        this.m_restartTimeMeasure = true;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
    }

    protected override void WorldPositionChanged(object source)
    {
      base.WorldPositionChanged(source);
      if (this.RadioBroadcaster == null)
        return;
      this.RadioBroadcaster.MoveBroadcaster();
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      this.UpdateLightParent();
      this.UpdateLightPosition();
      this.UpdateLightProperties();
      this.UpdateEmissivity();
    }

    public override void OnModelChange()
    {
      base.OnModelChange();
      this.UpdateLightProperties();
      this.UpdateEmissivity();
    }

    public override void UpdateVisual()
    {
      base.UpdateVisual();
      this.UpdateEmissivity();
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      float currentLightPower = this.m_currentLightPower;
      float num1 = 0.0f;
      if (!this.m_restartTimeMeasure)
        num1 = (float) (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastAnimationUpdateTime) / 1000f;
      else
        this.m_restartTimeMeasure = false;
      float num2 = this.IsWorking ? 1f : -1f;
      this.m_currentLightPower = MathHelper.Clamp(this.m_currentLightPower + (float) ((double) num2 * (double) num1 / 0.5), 0.0f, 1f);
      this.m_lastAnimationUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      if (this.m_light != null)
      {
        if ((double) this.m_currentLightPower <= 0.0)
        {
          this.m_light.LightOn = false;
          this.m_light.GlareOn = false;
        }
        else
        {
          this.m_light.LightOn = true;
          this.m_light.GlareOn = true;
        }
        if ((double) currentLightPower != (double) this.m_currentLightPower)
        {
          this.UpdateLightPosition();
          this.UpdateLightParent();
          this.m_light.UpdateLight();
          this.UpdateEmissivity();
          this.UpdateLightProperties();
        }
      }
      if ((double) this.m_currentLightPower == (double) num2 * 0.5 + 0.5)
        this.AnimationRunning = false;
      if (this.AnimationRunning || !this.IsFunctional || this.HasDamageEffect)
        return;
      this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
    }

    public override bool SetEmissiveStateWorking() => false;

    public override bool SetEmissiveStateDisabled() => false;

    public override bool SetEmissiveStateDamaged() => false;

    private void UpdateEmissivity()
    {
      Color color1 = MyBeacon.COLOR_OFF;
      Color color2 = MyBeacon.COLOR_ON;
      if (this.UsesEmissivePreset)
      {
        MyEmissiveColorStateResult result;
        if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Working, out result))
          color2 = result.EmissiveColor;
        if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Disabled, out result))
          color1 = result.EmissiveColor;
      }
      MyCubeBlock.UpdateEmissiveParts(this.Render.RenderObjectIDs[0], this.m_currentLightPower, Color.Lerp(color1, color2, this.m_currentLightPower), Color.White);
    }

    protected override void Closing()
    {
      MyLights.RemoveLight(this.m_light);
      this.RadioBroadcaster.OnBroadcastRadiusChanged -= new Action(this.OnBroadcastRadiusChanged);
      base.Closing();
    }

    private void UpdateLightParent()
    {
      if (this.m_light == null)
        return;
      this.m_light.ParentID = this.CubeGrid.Render.RenderData.GetOrAddCell(this.Position * this.CubeGrid.GridSize).ParentCullObject;
    }

    private void UpdateLightPosition()
    {
      if (this.m_light == null)
        return;
      this.m_light.Position = Vector3D.Transform(this.m_lightPositionOffset, (MatrixD) ref this.PositionComp.LocalMatrixRef);
      if (this.AnimationRunning)
        return;
      this.m_light.UpdateLight();
    }

    private void UpdatePower() => this.AnimationRunning = true;

    private void UpdateLightProperties()
    {
      if (this.m_light == null)
        return;
      Color color = Color.Lerp(MyBeacon.COLOR_OFF, MyBeacon.COLOR_ON, this.m_currentLightPower);
      float num1 = this.m_largeLight ? MyBeacon.POINT_LIGHT_RANGE_LARGE : MyBeacon.POINT_LIGHT_RANGE_SMALL;
      float num2 = this.m_currentLightPower * (this.m_largeLight ? MyBeacon.POINT_LIGHT_INTENSITY_LARGE : MyBeacon.POINT_LIGHT_INTENSITY_SMALL);
      this.m_light.Color = color;
      this.m_light.Range = num1;
      this.m_light.Intensity = num2;
      this.m_light.GlareIntensity = this.m_currentLightPower * this.m_flare.Intensity;
      this.m_light.UpdateLight();
    }

    protected override void OnEnabledChanged()
    {
      this.ResourceSink.Update();
      this.UpdatePower();
      base.OnEnabledChanged();
    }

    private void OnBroadcastRadiusChanged()
    {
      this.ResourceSink.Update();
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    private float UpdatePowerInput()
    {
      float num = (float) ((double) this.RadioBroadcaster.BroadcastRadius / (double) this.Definition.MaxBroadcastRadius * (double) this.Definition.MaxBroadcastPowerDrainkW / 1000.0);
      return !this.Enabled || !this.IsFunctional ? 0.0f : num;
    }

    protected override void OnOwnershipChanged()
    {
      base.OnOwnershipChanged();
      this.RadioBroadcaster.RaiseOwnerChanged();
    }

    public override void OnRemovedByCubeBuilder()
    {
      this.ReleaseInventory(MyEntityExtensions.GetInventory(this));
      base.OnRemovedByCubeBuilder();
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
    }

    float Sandbox.ModAPI.Ingame.IMyBeacon.Radius
    {
      get => this.RadioBroadcaster.BroadcastRadius;
      set => this.RadioBroadcaster.BroadcastRadius = MathHelper.Clamp(value, 0.0f, ((MyBeaconDefinition) this.BlockDefinition).MaxBroadcastRadius);
    }

    string Sandbox.ModAPI.Ingame.IMyBeacon.HudText
    {
      get => this.HudText.ToString();
      set => this.SetHudText(value);
    }

    protected sealed class SetHudTextEvent\u003C\u003ESystem_String : ICallSite<MyBeacon, string, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyBeacon @this,
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

    protected class m_radius\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyBeacon) obj0).m_radius = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_Cube_MyBeacon\u003C\u003EActor : IActivator, IActivator<MyBeacon>
    {
      object IActivator.CreateInstance() => (object) new MyBeacon();

      MyBeacon IActivator<MyBeacon>.CreateInstance() => new MyBeacon();
    }
  }
}
