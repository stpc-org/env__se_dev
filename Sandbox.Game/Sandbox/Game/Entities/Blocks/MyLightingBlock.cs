// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MyLightingBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Lights;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Entities.Blocks
{
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyLightingBlock), typeof (Sandbox.ModAPI.Ingame.IMyLightingBlock)})]
  public abstract class MyLightingBlock : MyFunctionalBlock, Sandbox.ModAPI.IMyLightingBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyLightingBlock, IMyParallelUpdateable
  {
    private const double MIN_MOVEMENT_SQUARED_FOR_UPDATE = 0.0001;
    private const int NUM_DECIMALS = 1;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_blinkIntervalSeconds;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_blinkLength;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_blinkOffset;
    protected List<MyLight> m_lights = new List<MyLight>();
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_intensity;
    private readonly VRage.Sync.Sync<Color, SyncDirection.BothWays> m_lightColor;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_lightRadius;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_lightFalloff;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_lightOffset;
    protected List<MyLightingBlock.LightLocalData> m_lightLocalData = new List<MyLightingBlock.LightLocalData>();
    private readonly float m_lightTurningOnSpeed = 0.05f;
    protected bool m_positionDirty = true;
    protected bool m_needsRecreateLights;
    protected bool HasSubPartLights;
    private const int MaxLightUpdateDistance = 5000;
    private bool m_emissiveMaterialDirty;
    private Color m_bulbColor = Color.Black;
    private float m_currentLightPower;
    private bool m_blinkOn = true;
    private float m_radius;
    private float m_reflectorRadius;
    private Color m_color;
    private float m_falloff;
    private MyParallelUpdateFlag m_parallelFlag;

    public MyLightingBlockDefinition BlockDefinition => (MyLightingBlockDefinition) base.BlockDefinition;

    public MyBounds BlinkIntervalSecondsBounds => this.BlockDefinition.BlinkIntervalSeconds;

    public MyBounds BlinkLenghtBounds => this.BlockDefinition.BlinkLenght;

    public MyBounds BlinkOffsetBounds => this.BlockDefinition.BlinkOffset;

    public MyBounds FalloffBounds => this.BlockDefinition.LightFalloff;

    public MyBounds OffsetBounds => this.BlockDefinition.LightOffset;

    public MyBounds RadiusBounds => this.BlockDefinition.LightRadius;

    public MyBounds ReflectorRadiusBounds => this.BlockDefinition.LightReflectorRadius;

    public MyBounds IntensityBounds => this.BlockDefinition.LightIntensity;

    public float ReflectorConeDegrees => this.BlockDefinition.ReflectorConeDegrees;

    public Vector4 LightColorDef => (this.IsLargeLight ? new Color((int) byte.MaxValue, (int) byte.MaxValue, 222) : new Color(206, 235, (int) byte.MaxValue)).ToVector4();

    protected override bool CheckIsWorking() => this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.CheckIsWorking();

    public bool IsLargeLight { get; private set; }

    public abstract bool IsReflector { get; }

    protected abstract bool SupportsFalloff { get; }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyLightingBlock>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlColor<MyLightingBlock> terminalControlColor = new MyTerminalControlColor<MyLightingBlock>("Color", MySpaceTexts.BlockPropertyTitle_LightColor);
      terminalControlColor.Getter = (MyTerminalValueControl<MyLightingBlock, Color>.GetterDelegate) (x => x.Color);
      terminalControlColor.Setter = (MyTerminalValueControl<MyLightingBlock, Color>.SetterDelegate) ((x, v) => x.m_lightColor.Value = v);
      MyTerminalControlFactory.AddControl<MyLightingBlock>((MyTerminalControl<MyLightingBlock>) terminalControlColor);
      MyTerminalControlSlider<MyLightingBlock> slider1 = new MyTerminalControlSlider<MyLightingBlock>("Radius", MySpaceTexts.BlockPropertyTitle_LightRadius, MySpaceTexts.BlockPropertyDescription_LightRadius);
      slider1.SetLimits((MyTerminalValueControl<MyLightingBlock, float>.GetterDelegate) (x => !x.IsReflector ? x.RadiusBounds.Min : x.ReflectorRadiusBounds.Min), (MyTerminalValueControl<MyLightingBlock, float>.GetterDelegate) (x => !x.IsReflector ? x.RadiusBounds.Max : x.ReflectorRadiusBounds.Max));
      slider1.DefaultValueGetter = (MyTerminalValueControl<MyLightingBlock, float>.GetterDelegate) (x => !x.IsReflector ? x.RadiusBounds.Default : x.ReflectorRadiusBounds.Default);
      slider1.Getter = (MyTerminalValueControl<MyLightingBlock, float>.GetterDelegate) (x => !x.IsReflector ? x.Radius : x.ReflectorRadius);
      slider1.Setter = (MyTerminalValueControl<MyLightingBlock, float>.SetterDelegate) ((x, v) => x.m_lightRadius.Value = v);
      slider1.Writer = (MyTerminalControl<MyLightingBlock>.WriterDelegate) ((x, result) => result.Append(MyValueFormatter.GetFormatedFloat(x.IsReflector ? x.m_reflectorRadius : x.m_radius, 1)).Append(" m"));
      slider1.EnableActions<MyLightingBlock>();
      MyTerminalControlFactory.AddControl<MyLightingBlock>((MyTerminalControl<MyLightingBlock>) slider1);
      MyTerminalControlSlider<MyLightingBlock> slider2 = new MyTerminalControlSlider<MyLightingBlock>("Falloff", MySpaceTexts.BlockPropertyTitle_LightFalloff, MySpaceTexts.BlockPropertyDescription_LightFalloff);
      slider2.SetLimits((MyTerminalValueControl<MyLightingBlock, float>.GetterDelegate) (x => x.FalloffBounds.Min), (MyTerminalValueControl<MyLightingBlock, float>.GetterDelegate) (x => x.FalloffBounds.Max));
      slider2.DefaultValueGetter = (MyTerminalValueControl<MyLightingBlock, float>.GetterDelegate) (x => x.FalloffBounds.Default);
      slider2.Getter = (MyTerminalValueControl<MyLightingBlock, float>.GetterDelegate) (x => x.Falloff);
      slider2.Setter = (MyTerminalValueControl<MyLightingBlock, float>.SetterDelegate) ((x, v) => x.m_lightFalloff.Value = v);
      slider2.Writer = (MyTerminalControl<MyLightingBlock>.WriterDelegate) ((x, result) => result.Append(MyValueFormatter.GetFormatedFloat(x.Falloff, 1)));
      slider2.Visible = (Func<MyLightingBlock, bool>) (x => x.SupportsFalloff);
      slider2.EnableActions<MyLightingBlock>();
      MyTerminalControlFactory.AddControl<MyLightingBlock>((MyTerminalControl<MyLightingBlock>) slider2);
      MyTerminalControlSlider<MyLightingBlock> slider3 = new MyTerminalControlSlider<MyLightingBlock>("Intensity", MySpaceTexts.BlockPropertyTitle_LightIntensity, MySpaceTexts.BlockPropertyDescription_LightIntensity);
      slider3.SetLimits((MyTerminalValueControl<MyLightingBlock, float>.GetterDelegate) (x => x.IntensityBounds.Min), (MyTerminalValueControl<MyLightingBlock, float>.GetterDelegate) (x => x.IntensityBounds.Max));
      slider3.DefaultValueGetter = (MyTerminalValueControl<MyLightingBlock, float>.GetterDelegate) (x => x.IntensityBounds.Default);
      slider3.Getter = (MyTerminalValueControl<MyLightingBlock, float>.GetterDelegate) (x => x.Intensity);
      slider3.Setter = (MyTerminalValueControl<MyLightingBlock, float>.SetterDelegate) ((x, v) => x.Intensity = v);
      slider3.Writer = (MyTerminalControl<MyLightingBlock>.WriterDelegate) ((x, result) => result.Append(MyValueFormatter.GetFormatedFloat(x.Intensity, 1)));
      slider3.EnableActions<MyLightingBlock>();
      MyTerminalControlFactory.AddControl<MyLightingBlock>((MyTerminalControl<MyLightingBlock>) slider3);
      MyTerminalControlSlider<MyLightingBlock> slider4 = new MyTerminalControlSlider<MyLightingBlock>("Offset", MySpaceTexts.BlockPropertyTitle_LightOffset, MySpaceTexts.BlockPropertyDescription_LightOffset);
      slider4.SetLimits((MyTerminalValueControl<MyLightingBlock, float>.GetterDelegate) (x => x.OffsetBounds.Min), (MyTerminalValueControl<MyLightingBlock, float>.GetterDelegate) (x => x.OffsetBounds.Max));
      slider4.DefaultValueGetter = (MyTerminalValueControl<MyLightingBlock, float>.GetterDelegate) (x => x.OffsetBounds.Default);
      slider4.Getter = (MyTerminalValueControl<MyLightingBlock, float>.GetterDelegate) (x => x.Offset);
      slider4.Setter = (MyTerminalValueControl<MyLightingBlock, float>.SetterDelegate) ((x, v) => x.m_lightOffset.Value = v);
      slider4.Writer = (MyTerminalControl<MyLightingBlock>.WriterDelegate) ((x, result) => result.Append(MyValueFormatter.GetFormatedFloat(x.Offset, 1)));
      slider4.EnableActions<MyLightingBlock>();
      MyTerminalControlFactory.AddControl<MyLightingBlock>((MyTerminalControl<MyLightingBlock>) slider4);
      MyTerminalControlSlider<MyLightingBlock> slider5 = new MyTerminalControlSlider<MyLightingBlock>("Blink Interval", MySpaceTexts.BlockPropertyTitle_LightBlinkInterval, MySpaceTexts.BlockPropertyDescription_LightBlinkInterval);
      slider5.SetLimits((MyTerminalValueControl<MyLightingBlock, float>.GetterDelegate) (x => x.BlinkIntervalSecondsBounds.Min), (MyTerminalValueControl<MyLightingBlock, float>.GetterDelegate) (x => x.BlinkIntervalSecondsBounds.Max));
      slider5.DefaultValueGetter = (MyTerminalValueControl<MyLightingBlock, float>.GetterDelegate) (x => x.BlinkIntervalSecondsBounds.Default);
      slider5.Getter = (MyTerminalValueControl<MyLightingBlock, float>.GetterDelegate) (x => x.BlinkIntervalSeconds);
      slider5.Setter = (MyTerminalValueControl<MyLightingBlock, float>.SetterDelegate) ((x, v) => x.BlinkIntervalSeconds = v);
      slider5.Writer = (MyTerminalControl<MyLightingBlock>.WriterDelegate) ((x, result) => result.Append(MyValueFormatter.GetFormatedFloat(x.BlinkIntervalSeconds, 1)).Append(" s"));
      slider5.EnableActions<MyLightingBlock>();
      MyTerminalControlFactory.AddControl<MyLightingBlock>((MyTerminalControl<MyLightingBlock>) slider5);
      MyTerminalControlSlider<MyLightingBlock> slider6 = new MyTerminalControlSlider<MyLightingBlock>("Blink Lenght", MySpaceTexts.BlockPropertyTitle_LightBlinkLenght, MySpaceTexts.BlockPropertyDescription_LightBlinkLenght, true, true);
      slider6.SetLimits((MyTerminalValueControl<MyLightingBlock, float>.GetterDelegate) (x => x.BlinkLenghtBounds.Min), (MyTerminalValueControl<MyLightingBlock, float>.GetterDelegate) (x => x.BlinkLenghtBounds.Max));
      slider6.DefaultValueGetter = (MyTerminalValueControl<MyLightingBlock, float>.GetterDelegate) (x => x.BlinkLenghtBounds.Default);
      slider6.Getter = (MyTerminalValueControl<MyLightingBlock, float>.GetterDelegate) (x => x.BlinkLength);
      slider6.Setter = (MyTerminalValueControl<MyLightingBlock, float>.SetterDelegate) ((x, v) => x.BlinkLength = v);
      slider6.Writer = (MyTerminalControl<MyLightingBlock>.WriterDelegate) ((x, result) => result.Append(MyValueFormatter.GetFormatedFloat(x.BlinkLength, 1)).Append(" %"));
      slider6.EnableActions<MyLightingBlock>();
      MyTerminalControlFactory.AddControl<MyLightingBlock>((MyTerminalControl<MyLightingBlock>) slider6);
      MyTerminalControlSlider<MyLightingBlock> slider7 = new MyTerminalControlSlider<MyLightingBlock>("Blink Offset", MySpaceTexts.BlockPropertyTitle_LightBlinkOffset, MySpaceTexts.BlockPropertyDescription_LightBlinkOffset, true, true);
      slider7.SetLimits((MyTerminalValueControl<MyLightingBlock, float>.GetterDelegate) (x => x.BlinkOffsetBounds.Min), (MyTerminalValueControl<MyLightingBlock, float>.GetterDelegate) (x => x.BlinkOffsetBounds.Max));
      slider7.DefaultValueGetter = (MyTerminalValueControl<MyLightingBlock, float>.GetterDelegate) (x => x.BlinkOffsetBounds.Default);
      slider7.Getter = (MyTerminalValueControl<MyLightingBlock, float>.GetterDelegate) (x => x.BlinkOffset);
      slider7.Setter = (MyTerminalValueControl<MyLightingBlock, float>.SetterDelegate) ((x, v) => x.BlinkOffset = v);
      slider7.Writer = (MyTerminalControl<MyLightingBlock>.WriterDelegate) ((x, result) => result.Append(MyValueFormatter.GetFormatedFloat(x.BlinkOffset, 1)).Append(" %"));
      slider7.EnableActions<MyLightingBlock>();
      MyTerminalControlFactory.AddControl<MyLightingBlock>((MyTerminalControl<MyLightingBlock>) slider7);
    }

    public Color Color
    {
      get => this.m_color;
      set
      {
        if (!(this.m_color != value))
          return;
        this.m_color = value;
        this.BulbColor = this.ComputeBulbColor();
        this.UpdateEmissivity(true);
        this.UpdateLightProperties();
        this.RaisePropertiesChanged();
      }
    }

    public float Radius
    {
      get => this.m_radius;
      set
      {
        if ((double) this.m_radius == (double) value)
          return;
        this.m_radius = value;
        this.UpdateLightProperties();
        this.RaisePropertiesChanged();
      }
    }

    public float ReflectorRadius
    {
      get => this.m_reflectorRadius;
      set
      {
        if ((double) this.m_reflectorRadius == (double) value)
          return;
        this.m_reflectorRadius = value;
        this.UpdateLightProperties();
        this.RaisePropertiesChanged();
      }
    }

    public float BlinkLength
    {
      get => (float) this.m_blinkLength;
      set
      {
        if ((double) (float) this.m_blinkLength == (double) value)
          return;
        this.m_blinkLength.Value = (float) Math.Round((double) value, 1);
        this.RaisePropertiesChanged();
      }
    }

    public float BlinkOffset
    {
      get => (float) this.m_blinkOffset;
      set
      {
        if ((double) (float) this.m_blinkOffset == (double) value)
          return;
        this.m_blinkOffset.Value = (float) Math.Round((double) value, 1);
        this.RaisePropertiesChanged();
      }
    }

    public float BlinkIntervalSeconds
    {
      get => (float) this.m_blinkIntervalSeconds;
      set
      {
        if ((double) (float) this.m_blinkIntervalSeconds == (double) value)
          return;
        this.m_blinkIntervalSeconds.Value = (double) value <= (double) (float) this.m_blinkIntervalSeconds ? (float) Math.Round((double) value - 0.0499899983406067, 1) : (float) Math.Round((double) value + 0.0499899983406067, 1);
        if ((double) (float) this.m_blinkIntervalSeconds == 0.0 && this.Enabled)
          this.UpdateEnabled();
        this.RaisePropertiesChanged();
      }
    }

    public virtual float Falloff
    {
      get => this.m_falloff;
      set
      {
        if ((double) this.m_falloff == (double) value)
          return;
        this.m_falloff = value;
        this.UpdateIntensity();
        this.UpdateLightProperties();
        this.RaisePropertiesChanged();
      }
    }

    public float Intensity
    {
      get => (float) this.m_intensity;
      set
      {
        if ((double) (float) this.m_intensity == (double) value)
          return;
        this.m_intensity.Value = value;
        this.UpdateIntensity();
        this.UpdateLightProperties();
        this.RaisePropertiesChanged();
      }
    }

    public float Offset
    {
      get => (float) this.m_lightOffset;
      set
      {
        if ((double) (float) this.m_lightOffset == (double) value)
          return;
        this.m_lightOffset.Value = value;
        this.UpdateLightProperties();
        this.RaisePropertiesChanged();
      }
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(this.BlockDefinition.ResourceSinkGroup, this.BlockDefinition.RequiredPowerInput, (Func<float>) (() => !this.Enabled || !this.IsFunctional ? 0.0f : this.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId)));
      resourceSinkComponent.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
      this.ResourceSink = resourceSinkComponent;
      base.Init(objectBuilder, cubeGrid);
      this.IsLargeLight = cubeGrid.GridSizeEnum == MyCubeSize.Large;
      MyObjectBuilder_LightingBlock builderLightingBlock = (MyObjectBuilder_LightingBlock) objectBuilder;
      this.m_color = (Color) ((double) builderLightingBlock.ColorAlpha == -1.0 ? this.LightColorDef : new Vector4(builderLightingBlock.ColorRed, builderLightingBlock.ColorGreen, builderLightingBlock.ColorBlue, builderLightingBlock.ColorAlpha));
      MyBounds myBounds = this.RadiusBounds;
      this.m_radius = myBounds.Clamp((double) builderLightingBlock.Radius == -1.0 ? this.RadiusBounds.Default : builderLightingBlock.Radius);
      myBounds = this.ReflectorRadiusBounds;
      this.m_reflectorRadius = myBounds.Clamp((double) builderLightingBlock.ReflectorRadius == -1.0 ? this.ReflectorRadiusBounds.Default : builderLightingBlock.ReflectorRadius);
      myBounds = this.FalloffBounds;
      this.m_falloff = myBounds.Clamp((double) builderLightingBlock.Falloff == -1.0 ? this.FalloffBounds.Default : builderLightingBlock.Falloff);
      VRage.Sync.Sync<float, SyncDirection.BothWays> blinkIntervalSeconds = this.m_blinkIntervalSeconds;
      myBounds = this.BlinkIntervalSecondsBounds;
      double num1 = (double) myBounds.Clamp((double) builderLightingBlock.BlinkIntervalSeconds == -1.0 ? this.BlinkIntervalSecondsBounds.Default : builderLightingBlock.BlinkIntervalSeconds);
      blinkIntervalSeconds.SetLocalValue((float) num1);
      VRage.Sync.Sync<float, SyncDirection.BothWays> blinkLength = this.m_blinkLength;
      myBounds = this.BlinkLenghtBounds;
      double num2 = (double) myBounds.Clamp((double) builderLightingBlock.BlinkLenght == -1.0 ? this.BlinkLenghtBounds.Default : builderLightingBlock.BlinkLenght);
      blinkLength.SetLocalValue((float) num2);
      VRage.Sync.Sync<float, SyncDirection.BothWays> blinkOffset = this.m_blinkOffset;
      myBounds = this.BlinkOffsetBounds;
      double num3 = (double) myBounds.Clamp((double) builderLightingBlock.BlinkOffset == -1.0 ? this.BlinkOffsetBounds.Default : builderLightingBlock.BlinkOffset);
      blinkOffset.SetLocalValue((float) num3);
      VRage.Sync.Sync<float, SyncDirection.BothWays> intensity = this.m_intensity;
      myBounds = this.IntensityBounds;
      double num4 = (double) myBounds.Clamp((double) builderLightingBlock.Intensity == -1.0 ? this.IntensityBounds.Default : builderLightingBlock.Intensity);
      intensity.SetLocalValue((float) num4);
      VRage.Sync.Sync<float, SyncDirection.BothWays> lightOffset = this.m_lightOffset;
      myBounds = this.OffsetBounds;
      double num5 = (double) myBounds.Clamp((double) builderLightingBlock.Offset == -1.0 ? this.OffsetBounds.Default : builderLightingBlock.Offset);
      lightOffset.SetLocalValue((float) num5);
      this.UpdateLightData();
      this.m_positionDirty = true;
      this.CreateLights();
      this.UpdateIntensity();
      this.UpdateLightPosition();
      this.UpdateLightBlink();
      this.UpdateEnabled();
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      this.ResourceSink.Update();
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.IsWorkingChanged += new Action<MyCubeBlock>(this.CubeBlock_OnWorkingChanged);
      MyCubeGrid cubeGrid1 = this.CubeGrid;
      cubeGrid1.IsPreviewChanged = cubeGrid1.IsPreviewChanged + new Action<bool>(this.OnIsPreviewChanged);
    }

    private void OnIsPreviewChanged(bool isPreview) => MySandboxGame.Static.Invoke((Action) (() =>
    {
      if (this.MarkedForClose)
        return;
      MyCubeGrid cubeGrid = this.CubeGrid;
      if ((cubeGrid != null ? (!cubeGrid.MarkedForClose ? 1 : 0) : 0) == 0)
        return;
      this.UpdateVisual();
    }), "LightPreviewUpdate");

    private void UpdateLightData()
    {
      this.m_lightLocalData.Clear();
      this.HasSubPartLights = false;
      foreach (KeyValuePair<string, MyModelDummy> dummy in MyModels.GetModelOnlyDummies(this.BlockDefinition.Model).Dummies)
      {
        string lower = dummy.Key.ToLower();
        if (!lower.Contains("subpart") && lower.Contains("light"))
          this.m_lightLocalData.Add(new MyLightingBlock.LightLocalData()
          {
            LocalMatrix = Matrix.Normalize(dummy.Value.Matrix),
            Subpart = (MyEntitySubpart) null
          });
      }
      foreach (KeyValuePair<string, MyEntitySubpart> subpart in this.Subparts)
      {
        foreach (KeyValuePair<string, MyModelDummy> dummy in subpart.Value.Model.Dummies)
        {
          if (dummy.Key.ToLower().Contains("light"))
          {
            this.m_lightLocalData.Add(new MyLightingBlock.LightLocalData()
            {
              LocalMatrix = Matrix.Normalize(dummy.Value.Matrix),
              Subpart = subpart.Value
            });
            this.HasSubPartLights = true;
          }
        }
      }
    }

    private void CreateLights()
    {
      this.CloseLights();
      foreach (MyLightingBlock.LightLocalData lightLocalData in this.m_lightLocalData)
      {
        MyLight light = MyLights.AddLight();
        if (light != null)
        {
          this.m_lights.Add(light);
          this.InitLight(light, (Vector4) this.m_color, this.m_radius, this.m_falloff);
          light.ReflectorColor = this.m_color;
          light.ReflectorRange = this.m_reflectorRadius;
          light.Range = this.m_radius;
          light.ReflectorConeDegrees = this.ReflectorConeDegrees;
          this.UpdateRadius(this.IsReflector ? this.m_reflectorRadius : this.m_radius);
        }
      }
      this.m_positionDirty = true;
    }

    private void UpdateParents()
    {
      uint parentCullObject = this.CubeGrid.Render.RenderData.GetOrAddCell(this.Position * this.CubeGrid.GridSize).ParentCullObject;
      foreach (MyLight light in this.m_lights)
        light.ParentID = parentCullObject;
    }

    public override void OnRegisteredToGridSystems()
    {
      base.OnRegisteredToGridSystems();
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    protected abstract void InitLight(MyLight light, Vector4 color, float radius, float falloff);

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_LightingBlock builderCubeBlock = (MyObjectBuilder_LightingBlock) base.GetObjectBuilderCubeBlock(copy);
      Vector4 vector4 = this.m_color.ToVector4();
      builderCubeBlock.ColorRed = vector4.X;
      builderCubeBlock.ColorGreen = vector4.Y;
      builderCubeBlock.ColorBlue = vector4.Z;
      builderCubeBlock.ColorAlpha = vector4.W;
      builderCubeBlock.Radius = this.m_radius;
      builderCubeBlock.ReflectorRadius = this.m_reflectorRadius;
      builderCubeBlock.Falloff = this.Falloff;
      builderCubeBlock.Intensity = (float) this.m_intensity;
      builderCubeBlock.BlinkIntervalSeconds = (float) this.m_blinkIntervalSeconds;
      builderCubeBlock.BlinkLenght = (float) this.m_blinkLength;
      builderCubeBlock.BlinkOffset = (float) this.m_blinkOffset;
      builderCubeBlock.Offset = (float) this.m_lightOffset;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    private void CloseLights()
    {
      foreach (MyLight light in this.m_lights)
        MyLights.RemoveLight(light);
      this.m_lights.Clear();
    }

    protected override void Closing()
    {
      if (this.CubeGrid != null)
      {
        MyCubeGrid cubeGrid = this.CubeGrid;
        cubeGrid.IsPreviewChanged = cubeGrid.IsPreviewChanged - new Action<bool>(this.OnIsPreviewChanged);
      }
      this.CloseLights();
      base.Closing();
    }

    public MyLightingBlock()
    {
      this.CreateTerminalControls();
      this.m_intensity.ValueChanged += (Action<SyncBase>) (x => this.IntensityChanged());
      this.m_lightColor.ValueChanged += (Action<SyncBase>) (x => this.LightColorChanged());
      this.m_lightRadius.ValueChanged += (Action<SyncBase>) (x => this.LightRadiusChanged());
      this.m_lightFalloff.ValueChanged += (Action<SyncBase>) (x => this.LightFalloffChanged());
      this.m_lightOffset.ValueChanged += (Action<SyncBase>) (x => this.LightOffsetChanged());
    }

    private void IntensityChanged()
    {
      this.UpdateIntensity();
      this.UpdateLightProperties();
    }

    private void LightFalloffChanged() => this.Falloff = this.m_lightFalloff.Value;

    private void LightOffsetChanged() => this.UpdateLightProperties();

    protected virtual void UpdateRadius(float value)
    {
      if (this.IsReflector)
        this.ReflectorRadius = value;
      else
        this.Radius = value;
    }

    private void LightRadiusChanged() => this.UpdateRadius(this.m_lightRadius.Value);

    private void LightColorChanged() => this.Color = this.m_lightColor.Value;

    private float GetNewLightPower() => MathHelper.Clamp(this.CurrentLightPower + (this.IsWorking ? 1f : -1f) * this.m_lightTurningOnSpeed, 0.0f, 1f);

    protected override void OnStartWorking()
    {
      base.OnStartWorking();
      this.m_emissiveMaterialDirty = true;
      this.m_parallelFlag.Enable((MyEntity) this);
    }

    protected override void OnStopWorking()
    {
      base.OnStopWorking();
      this.m_emissiveMaterialDirty = true;
      this.m_parallelFlag.Enable((MyEntity) this);
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      if (this.m_needsRecreateLights)
      {
        this.m_needsRecreateLights = false;
        this.CreateLights();
        this.UpdateLightPosition();
        this.UpdateIntensity();
        this.UpdateLightBlink();
        this.UpdateEnabled();
      }
      this.UpdateParents();
      this.UpdateLightProperties();
      if (!this.NeedPerFrameUpdate)
        return;
      this.m_parallelFlag.Enable((MyEntity) this);
    }

    public virtual void UpdateAfterSimulationParallel()
    {
      if ((MySector.MainCamera.Position - this.PositionComp.GetPosition()).AbsMax() > 5000.0)
        return;
      uint parentCullObject = this.CubeGrid.Render.RenderData.GetOrAddCell(this.Position * this.CubeGrid.GridSize).ParentCullObject;
      foreach (MyLight light in this.m_lights)
        light.ParentID = parentCullObject;
      float newLightPower = this.GetNewLightPower();
      if ((double) newLightPower != (double) this.CurrentLightPower)
      {
        this.CurrentLightPower = newLightPower;
        this.UpdateIntensity();
      }
      this.UpdateLightBlink();
      this.UpdateEnabled();
      this.UpdateLightPosition();
      this.UpdateLightProperties();
      this.UpdateEmissivity();
      this.UpdateEmissiveMaterial();
    }

    public override void UpdateAfterSimulation100()
    {
      if ((MySector.MainCamera.Position - this.PositionComp.GetPosition()).AbsMax() > 5000.0)
      {
        this.m_parallelFlag.Disable((MyEntity) this);
      }
      else
      {
        this.m_parallelFlag.Set((MyEntity) this, this.NeedPerFrameUpdate);
        this.UpdateLightProperties();
      }
    }

    protected virtual bool NeedPerFrameUpdate => (0 | ((double) (float) this.m_blinkIntervalSeconds <= 0.0 ? 0 : (this.IsWorking ? 1 : 0)) | ((double) this.GetNewLightPower() != (double) this.CurrentLightPower ? 1 : 0)) != 0;

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      uint parentCullObject = this.CubeGrid.Render.RenderData.GetOrAddCell(this.Position * this.CubeGrid.GridSize).ParentCullObject;
      foreach (MyLight light in this.m_lights)
        light.ParentID = parentCullObject;
      this.UpdateLightPosition();
      this.UpdateLightProperties();
      this.UpdateEmissivity(true);
    }

    private void UpdateEnabled() => this.UpdateEnabled((double) this.CurrentLightPower * (double) this.Intensity > 0.0 && this.m_blinkOn && !this.IsPreview && !this.CubeGrid.IsPreview);

    protected abstract void UpdateEnabled(bool state);

    protected abstract void UpdateIntensity();

    private void UpdateLightBlink()
    {
      if ((double) (float) this.m_blinkIntervalSeconds > 0.000989999971352518)
      {
        ulong num1 = (ulong) ((double) (float) this.m_blinkIntervalSeconds * 1000.0);
        ulong num2 = (ulong) (MySession.Static.ElapsedGameTime.TotalMilliseconds - (double) num1 * (double) (float) this.m_blinkOffset * 0.00999999977648258) % num1;
        this.m_blinkOn = (ulong) ((double) num1 * (double) (float) this.m_blinkLength * 0.00999999977648258) > num2;
      }
      else
        this.m_blinkOn = true;
    }

    protected virtual void UpdateEmissivity(bool force = false)
    {
    }

    protected override void OnEnabledChanged()
    {
      this.ResourceSink.Update();
      base.OnEnabledChanged();
    }

    private void ComponentStack_IsFunctionalChanged() => this.ResourceSink.Update();

    private void Receiver_IsPoweredChanged() => this.UpdateIsWorking();

    private void CubeBlock_OnWorkingChanged(MyCubeBlock block) => this.m_positionDirty = true;

    protected Color ComputeBulbColor()
    {
      float num1 = (float) (0.125 + (double) this.IntensityBounds.Normalize(this.Intensity) * 0.25);
      Color color = this.Color;
      double num2 = (double) color.R * 0.5 + (double) num1;
      color = this.Color;
      double num3 = (double) color.G * 0.5 + (double) num1;
      color = this.Color;
      double num4 = (double) color.B * 0.5 + (double) num1;
      return new Color((float) num2, (float) num3, (float) num4);
    }

    private void UpdateLightProperties()
    {
      foreach (MyLight light in this.m_lights)
      {
        light.Range = this.m_radius;
        light.ReflectorRange = this.m_reflectorRadius;
        light.Color = this.m_color;
        light.ReflectorColor = this.m_color;
        light.Falloff = this.m_falloff;
        light.PointLightOffset = this.Offset;
        light.UpdateLight();
      }
    }

    private void UpdateLightPosition()
    {
      if (this.m_lights == null || this.m_lights.Count == 0 || !this.m_positionDirty)
        return;
      this.m_positionDirty = false;
      MatrixD matrixD = (MatrixD) ref this.PositionComp.LocalMatrixRef;
      for (int index = 0; index < this.m_lightLocalData.Count; ++index)
      {
        MatrixD matrix = (MatrixD) ref this.PositionComp.LocalMatrixRef;
        if (this.m_lightLocalData[index].Subpart != null)
          matrix = this.m_lightLocalData[index].Subpart.PositionComp.LocalMatrixRef * matrix;
        MyLight light = this.m_lights[index];
        light.Position = Vector3D.Transform(this.m_lightLocalData[index].LocalMatrix.Translation, matrix);
        light.ReflectorDirection = (Vector3) Vector3D.TransformNormal(this.m_lightLocalData[index].LocalMatrix.Forward, matrix);
        light.ReflectorUp = (Vector3) Vector3D.TransformNormal(this.m_lightLocalData[index].LocalMatrix.Right, matrix);
      }
    }

    public override void OnCubeGridChanged(MyCubeGrid oldGrid)
    {
      base.OnCubeGridChanged(oldGrid);
      this.m_positionDirty = true;
    }

    public float CurrentLightPower
    {
      get => this.m_currentLightPower;
      set
      {
        if ((double) this.m_currentLightPower == (double) value)
          return;
        this.m_currentLightPower = value;
        this.m_emissiveMaterialDirty = true;
      }
    }

    public Color BulbColor
    {
      get => this.m_bulbColor;
      set
      {
        if (!(this.m_bulbColor != value))
          return;
        this.m_bulbColor = value;
        this.m_emissiveMaterialDirty = true;
      }
    }

    public override void OnModelChange()
    {
      base.OnModelChange();
      this.UpdateLightData();
      this.m_needsRecreateLights = true;
      this.m_emissiveMaterialDirty = true;
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    public override void UpdateVisual()
    {
      base.UpdateVisual();
      this.UpdateParents();
      this.m_positionDirty = true;
      this.m_emissiveMaterialDirty = true;
      this.UpdateLightPosition();
      this.UpdateIntensity();
      this.UpdateLightBlink();
      this.UpdateEnabled();
    }

    private void UpdateEmissiveMaterial()
    {
      if (!this.m_emissiveMaterialDirty)
        return;
      foreach (uint renderObjectId in this.Render.RenderObjectIDs)
        this.UpdateEmissiveMaterial(renderObjectId);
      foreach (MyLightingBlock.LightLocalData lightLocalData in this.m_lightLocalData)
      {
        if (lightLocalData.Subpart != null && lightLocalData.Subpart.Render != null)
        {
          foreach (uint renderObjectId in lightLocalData.Subpart.Render.RenderObjectIDs)
            this.UpdateEmissiveMaterial(renderObjectId);
        }
      }
      this.m_emissiveMaterialDirty = false;
    }

    private void UpdateEmissiveMaterial(uint renderId)
    {
      MyRenderProxy.UpdateModelProperties(renderId, "Emissive", (RenderFlags) 0, (RenderFlags) 0, new Color?(this.BulbColor), new float?(this.CurrentLightPower));
      MyRenderProxy.UpdateModelProperties(renderId, "EmissiveSpotlight", (RenderFlags) 0, (RenderFlags) 0, new Color?(this.BulbColor), new float?(this.CurrentLightPower));
    }

    float Sandbox.ModAPI.Ingame.IMyLightingBlock.ReflectorRadius => this.ReflectorRadius;

    float Sandbox.ModAPI.Ingame.IMyLightingBlock.BlinkLenght => this.BlinkLength;

    float Sandbox.ModAPI.Ingame.IMyLightingBlock.Radius
    {
      get => !this.IsReflector ? this.Radius : this.ReflectorRadius;
      set
      {
        value = this.IsReflector ? MathHelper.Clamp(value, this.ReflectorRadiusBounds.Min, this.ReflectorRadiusBounds.Max) : MathHelper.Clamp(value, this.RadiusBounds.Min, this.RadiusBounds.Max);
        this.m_lightRadius.Value = value;
      }
    }

    float Sandbox.ModAPI.Ingame.IMyLightingBlock.Intensity
    {
      get => (float) this.m_intensity;
      set
      {
        value = MathHelper.Clamp(value, this.IntensityBounds.Min, this.IntensityBounds.Max);
        this.m_intensity.Value = value;
      }
    }

    float Sandbox.ModAPI.Ingame.IMyLightingBlock.Falloff
    {
      get => (float) this.m_lightFalloff;
      set
      {
        value = MathHelper.Clamp(value, this.FalloffBounds.Min, this.FalloffBounds.Max);
        this.m_lightFalloff.Value = value;
      }
    }

    float Sandbox.ModAPI.Ingame.IMyLightingBlock.BlinkIntervalSeconds
    {
      get => this.BlinkIntervalSeconds;
      set
      {
        value = MathHelper.Clamp(value, this.BlinkIntervalSecondsBounds.Min, this.BlinkIntervalSecondsBounds.Max);
        this.BlinkIntervalSeconds = value;
      }
    }

    float Sandbox.ModAPI.Ingame.IMyLightingBlock.BlinkLength
    {
      get => this.BlinkLength;
      set
      {
        value = MathHelper.Clamp(value, this.BlinkLenghtBounds.Min, this.BlinkLenghtBounds.Max);
        this.BlinkLength = value;
      }
    }

    float Sandbox.ModAPI.Ingame.IMyLightingBlock.BlinkOffset
    {
      get => this.BlinkOffset;
      set
      {
        value = MathHelper.Clamp(value, this.BlinkOffsetBounds.Min, this.BlinkOffsetBounds.Max);
        this.BlinkOffset = value;
      }
    }

    Color Sandbox.ModAPI.Ingame.IMyLightingBlock.Color
    {
      get => this.Color;
      set => this.m_lightColor.Value = value;
    }

    public void UpdateBeforeSimulationParallel()
    {
    }

    public MyParallelUpdateFlags UpdateFlags => this.m_parallelFlag.GetFlags((MyEntity) this);

    public override void DisableUpdates()
    {
      base.DisableUpdates();
      this.m_parallelFlag.Disable((MyEntity) this);
    }

    protected class LightLocalData
    {
      public Matrix LocalMatrix;
      public MyEntitySubpart Subpart;
    }

    protected class m_blinkIntervalSeconds\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyLightingBlock) obj0).m_blinkIntervalSeconds = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_blinkLength\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyLightingBlock) obj0).m_blinkLength = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_blinkOffset\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyLightingBlock) obj0).m_blinkOffset = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_intensity\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyLightingBlock) obj0).m_intensity = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_lightColor\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<Color, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<Color, SyncDirection.BothWays>(obj1, obj2));
        ((MyLightingBlock) obj0).m_lightColor = (VRage.Sync.Sync<Color, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_lightRadius\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyLightingBlock) obj0).m_lightRadius = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_lightFalloff\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyLightingBlock) obj0).m_lightFalloff = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_lightOffset\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyLightingBlock) obj0).m_lightOffset = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }
  }
}
