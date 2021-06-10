// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.Blocks.MyInteriorLight
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Lights;
using Sandbox.ModAPI;
using System;
using VRage.Game;
using VRage.ObjectBuilders;
using VRageMath;
using VRageRender.Lights;

namespace SpaceEngineers.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_InteriorLight))]
  [MyTerminalInterface(new Type[] {typeof (SpaceEngineers.Game.ModAPI.IMyInteriorLight), typeof (SpaceEngineers.Game.ModAPI.Ingame.IMyInteriorLight)})]
  public class MyInteriorLight : MyLightingBlock, SpaceEngineers.Game.ModAPI.IMyInteriorLight, Sandbox.ModAPI.IMyLightingBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyLightingBlock, SpaceEngineers.Game.ModAPI.Ingame.IMyInteriorLight
  {
    private MyFlareDefinition m_flare;

    public override bool IsReflector => false;

    protected override bool SupportsFalloff => true;

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid) => base.Init(objectBuilder, cubeGrid);

    protected override void InitLight(MyLight light, Vector4 color, float radius, float falloff)
    {
      light.Start(color, radius, this.DisplayNameText);
      light.Falloff = falloff;
      this.UpdateGlare(light);
    }

    private void UpdateGlare(MyLight light)
    {
      light.GlareOn = light.LightOn && !this.IsPreview && !this.CubeGrid.IsPreview;
      light.GlareIntensity = 0.4f;
      light.GlareQuerySize = 0.2f;
      light.GlareType = MyGlareTypeEnum.Normal;
      if (!(MyDefinitionManager.Static.GetDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_FlareDefinition), this.BlockDefinition.Flare)) is MyFlareDefinition myFlareDefinition))
        myFlareDefinition = new MyFlareDefinition();
      this.m_flare = myFlareDefinition;
      light.GlareSize = this.m_flare.Size;
      light.SubGlares = this.m_flare.SubGlares;
      this.UpdateIntensity();
    }

    protected override void UpdateEnabled(bool state)
    {
      foreach (MyLight light in this.m_lights)
      {
        light.LightOn = state;
        light.GlareOn = state;
      }
    }

    protected override void UpdateIntensity()
    {
      float num1 = this.CurrentLightPower * this.Intensity;
      foreach (MyLight light in this.m_lights)
      {
        light.Intensity = num1 * 2f;
        float num2 = this.m_flare.Intensity * num1;
        if ((double) num2 < (double) this.m_flare.Intensity)
          num2 = this.m_flare.Intensity;
        light.GlareIntensity = num2;
      }
      this.BulbColor = this.ComputeBulbColor();
    }

    public override void UpdateVisual()
    {
      base.UpdateVisual();
      foreach (MyLight light in this.m_lights)
      {
        this.UpdateGlare(light);
        light.UpdateLight();
      }
      this.UpdateEmissivity(true);
    }

    protected override void UpdateEmissivity(bool force = false)
    {
      if (this.m_lights == null)
        return;
      base.UpdateEmissivity(force);
      foreach (MyLight light in this.m_lights)
        MyCubeBlock.UpdateEmissiveParts(this.Render.RenderObjectIDs[0], light.LightOn ? light.Intensity : 0.0f, Color.Lerp(this.Color, this.Color.ToGray(), 0.5f), Color.Black);
    }
  }
}
