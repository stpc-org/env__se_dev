// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyReflectorLight
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Lights;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.ModAPI;
using System;
using System.Runtime.InteropServices;
using VRage.Game;
using VRage.Game.Components;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender.Lights;

namespace Sandbox.Game.Entities
{
  [MyCubeBlockType(typeof (MyObjectBuilder_ReflectorLight))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyReflectorLight), typeof (Sandbox.ModAPI.Ingame.IMyReflectorLight)})]
  public class MyReflectorLight : MyLightingBlock, Sandbox.ModAPI.IMyReflectorLight, Sandbox.ModAPI.IMyLightingBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyLightingBlock, Sandbox.ModAPI.Ingame.IMyReflectorLight
  {
    private MyFlareDefinition m_flare;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_rotationSpeedForSubparts;
    private Matrix m_rotationMatrix = Matrix.Identity;
    private static readonly Color COLOR_OFF = new Color(30, 30, 30);
    private bool m_wasWorking = true;

    private float GlareQuerySizeDef => this.CubeGrid.GridScale * (this.IsLargeLight ? 0.5f : 0.1f);

    public override bool IsReflector => true;

    public bool IsReflectorEnabled => this.m_lights.Count > 0 && this.m_lights[0].ReflectorOn;

    protected override bool SupportsFalloff => false;

    public string ReflectorConeMaterial => this.BlockDefinition.ReflectorConeMaterial;

    public MyReflectorLight()
    {
      this.Render = (MyRenderComponentBase) new MyRenderComponentReflectorLight(this.m_lights);
      this.m_rotationSpeedForSubparts.ValueChanged += (Action<SyncBase>) (x => this.RotationSpeedChanged());
    }

    private void RotationSpeedChanged() => this.m_rotationMatrix = Matrix.CreateRotationZ((float) this.m_rotationSpeedForSubparts);

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      base.Init(objectBuilder, cubeGrid);
      MyObjectBuilder_ReflectorLight builderReflectorLight = (MyObjectBuilder_ReflectorLight) objectBuilder;
      this.m_rotationSpeedForSubparts.SetLocalValue(this.BlockDefinition.RotationSpeedBounds.Clamp((double) builderReflectorLight.RotationSpeed == -1.0 ? this.BlockDefinition.RotationSpeedBounds.Default : builderReflectorLight.RotationSpeed));
    }

    protected override void InitLight(MyLight light, Vector4 color, float radius, float falloff)
    {
      light.Start(color, this.CubeGrid.GridScale * radius, this.DisplayNameText);
      light.ReflectorOn = true;
      light.LightType = MyLightType.SPOTLIGHT;
      light.ReflectorTexture = this.BlockDefinition.ReflectorTexture;
      light.Falloff = 0.3f;
      light.GlossFactor = 0.0f;
      light.ReflectorGlossFactor = 1f;
      light.ReflectorFalloff = 0.5f;
      light.GlareOn = light.LightOn;
      light.GlareQuerySize = this.GlareQuerySizeDef;
      light.GlareType = MyGlareTypeEnum.Directional;
      if (!(MyDefinitionManager.Static.GetDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_FlareDefinition), this.BlockDefinition.Flare)) is MyFlareDefinition myFlareDefinition))
        myFlareDefinition = new MyFlareDefinition();
      this.m_flare = myFlareDefinition;
      light.GlareSize = this.m_flare.Size;
      light.SubGlares = this.m_flare.SubGlares;
      this.UpdateIntensity();
      this.Render.NeedsDrawFromParent = true;
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyReflectorLight>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlSlider<MyReflectorLight> slider = new MyTerminalControlSlider<MyReflectorLight>("RotationSpeed", MySpaceTexts.BlockPropertyTitle_LightReflectorRotationSpeed, MySpaceTexts.BlockPropertyTitle_LightReflectorRotationSpeed);
      slider.SetLimits((MyTerminalValueControl<MyReflectorLight, float>.GetterDelegate) (x => x.BlockDefinition.RotationSpeedBounds.Min), (MyTerminalValueControl<MyReflectorLight, float>.GetterDelegate) (x => x.BlockDefinition.RotationSpeedBounds.Max));
      slider.DefaultValueGetter = (MyTerminalValueControl<MyReflectorLight, float>.GetterDelegate) (x => x.BlockDefinition.RotationSpeedBounds.Default);
      slider.Getter = (MyTerminalValueControl<MyReflectorLight, float>.GetterDelegate) (x => (float) x.m_rotationSpeedForSubparts);
      slider.Setter = (MyTerminalValueControl<MyReflectorLight, float>.SetterDelegate) ((x, v) => x.m_rotationSpeedForSubparts.Value = v);
      slider.Writer = (MyTerminalControl<MyReflectorLight>.WriterDelegate) ((x, result) => result.Append(MyValueFormatter.GetFormatedFloat((float) x.m_rotationSpeedForSubparts, 2)));
      slider.Visible = (Func<MyReflectorLight, bool>) (x => x.Subparts.Count > 0);
      slider.EnableActions<MyReflectorLight>();
      MyTerminalControlFactory.AddControl<MyReflectorLight>((MyTerminalControl<MyReflectorLight>) slider);
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_ReflectorLight builderCubeBlock = (MyObjectBuilder_ReflectorLight) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.RotationSpeed = (float) this.m_rotationSpeedForSubparts;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    protected override void UpdateEnabled(bool state)
    {
      if (this.m_lights == null)
        return;
      bool flag = state && this.CubeGrid.Projector == null;
      foreach (MyLight light in this.m_lights)
      {
        light.ReflectorOn = flag;
        light.LightOn = flag;
        light.GlareOn = flag;
      }
    }

    protected override void UpdateIntensity()
    {
      float num1 = this.CurrentLightPower * this.Intensity;
      foreach (MyLight light in this.m_lights)
      {
        light.ReflectorIntensity = num1 * 8f;
        light.Intensity = num1 * 0.3f;
        float num2 = num1 / this.IntensityBounds.Max;
        float num3 = this.m_flare.Intensity * num1;
        if ((double) num3 < (double) this.m_flare.Intensity)
          num3 = this.m_flare.Intensity;
        light.GlareIntensity = num3;
        light.GlareSize = this.m_flare.Size * (float) ((double) num2 / 2.0 + 0.5);
        this.BulbColor = this.ComputeBulbColor();
      }
    }

    public override void UpdateAfterSimulationParallel()
    {
      base.UpdateAfterSimulationParallel();
      if (!this.HasSubPartLights || (double) (float) this.m_rotationSpeedForSubparts <= 0.0 || (double) this.CurrentLightPower <= 0.0)
        return;
      this.m_positionDirty = true;
      for (int index = 0; index < this.m_lightLocalData.Count; ++index)
      {
        if (this.m_lightLocalData[index].Subpart != null)
        {
          Matrix localMatrix = this.m_rotationMatrix * this.m_lightLocalData[index].Subpart.PositionComp.LocalMatrixRef;
          this.m_lightLocalData[index].Subpart.PositionComp.SetLocalMatrix(ref localMatrix);
        }
      }
    }

    protected override bool NeedPerFrameUpdate => ((base.NeedPerFrameUpdate ? 1 : 0) | (!this.HasSubPartLights || (double) (float) this.m_rotationSpeedForSubparts <= 0.0 ? 0 : ((double) this.CurrentLightPower > 0.0 ? 1 : 0))) != 0;

    public override void UpdateVisual()
    {
      base.UpdateVisual();
      this.UpdateEmissivity(true);
    }

    public MyReflectorBlockDefinition BlockDefinition
    {
      get
      {
        if (base.BlockDefinition is MyReflectorBlockDefinition)
          return (MyReflectorBlockDefinition) base.BlockDefinition;
        this.SlimBlock.BlockDefinition = (MyCubeBlockDefinition) new MyReflectorBlockDefinition();
        return (MyReflectorBlockDefinition) base.BlockDefinition;
      }
    }

    protected override void UpdateRadius(float value)
    {
      base.UpdateRadius(value);
      this.Radius = (float) (10.0 * ((double) this.ReflectorRadius / (double) this.ReflectorRadiusBounds.Max));
    }

    protected override void UpdateEmissivity(bool force = false)
    {
      bool flag = this.m_lights.Count > 0 && this.m_lights[0].ReflectorOn;
      if (this.m_lights == null || this.m_wasWorking == (this.IsWorking & flag) && !force)
        return;
      this.m_wasWorking = this.IsWorking & flag;
      if (this.m_wasWorking)
        MyCubeBlock.UpdateEmissiveParts(this.Render.RenderObjectIDs[0], 1f, this.Color, Color.White);
      else
        MyCubeBlock.UpdateEmissiveParts(this.Render.RenderObjectIDs[0], 0.0f, MyReflectorLight.COLOR_OFF, Color.White);
    }

    protected class m_rotationSpeedForSubparts\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyReflectorLight) obj0).m_rotationSpeedForSubparts = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_MyReflectorLight\u003C\u003EActor : IActivator, IActivator<MyReflectorLight>
    {
      object IActivator.CreateInstance() => (object) new MyReflectorLight();

      MyReflectorLight IActivator<MyReflectorLight>.CreateInstance() => new MyReflectorLight();
    }
  }
}
