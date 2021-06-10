// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.Blocks.MySolarPanel
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.ModAPI;
using SpaceEngineers.Game.EntityComponents.DebugRenders;
using SpaceEngineers.Game.EntityComponents.GameLogic;
using System;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Graphics;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_SolarPanel))]
  [MyTerminalInterface(new Type[] {typeof (SpaceEngineers.Game.ModAPI.IMySolarPanel), typeof (SpaceEngineers.Game.ModAPI.Ingame.IMySolarPanel)})]
  public class MySolarPanel : MyEnvironmentalPowerProducer, SpaceEngineers.Game.ModAPI.IMySolarPanel, Sandbox.ModAPI.IMyPowerProducer, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyPowerProducer, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, SpaceEngineers.Game.ModAPI.Ingame.IMySolarPanel
  {
    private static readonly string[] m_emissiveTextureNames = new string[4]
    {
      "Emissive0",
      "Emissive1",
      "Emissive2",
      "Emissive3"
    };

    public MySolarPanelDefinition SolarPanelDefinition { get; private set; }

    public MySolarGameLogicComponent SolarComponent { get; private set; }

    protected override float CurrentProductionRatio => this.SolarComponent.MaxOutput;

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.SolarPanelDefinition = (MySolarPanelDefinition) this.BlockDefinition;
      this.GameLogic = (MyGameLogicComponent) (this.SolarComponent = new MySolarGameLogicComponent());
      this.SolarComponent.OnProductionChanged += new Action(((MyEnvironmentalPowerProducer) this).OnProductionChanged);
      this.SolarComponent.Initialize(this.SolarPanelDefinition.PanelOrientation, this.SolarPanelDefinition.IsTwoSided, this.SolarPanelDefinition.PanelOffset, (MyFunctionalBlock) this);
      base.Init(objectBuilder, cubeGrid);
      this.SourceComp.Enabled = this.Enabled;
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentSolarPanel((MyTerminalBlock) this));
    }

    protected override void OnProductionChanged()
    {
      base.OnProductionChanged();
      this.UpdateEmissivity();
    }

    public override void UpdateVisual()
    {
      base.UpdateVisual();
      this.UpdateEmissivity();
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      this.UpdateEmissivity();
    }

    public override bool SetEmissiveStateWorking() => false;

    public override bool SetEmissiveStateDamaged() => false;

    public override bool SetEmissiveStateDisabled() => false;

    public override void SetDamageEffect(bool show)
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      base.SetDamageEffect(show);
      if (this.m_soundEmitter == null || this.BlockDefinition.DamagedSound == null || show || !(this.m_soundEmitter.SoundId == this.BlockDefinition.DamagedSound.Arcade) && !(this.m_soundEmitter.SoundId != this.BlockDefinition.DamagedSound.Realistic))
        return;
      this.m_soundEmitter.StopSound(false);
    }

    protected void UpdateEmissivity()
    {
      if (!this.InScene)
        return;
      Color emissivePartColor1 = Color.Red;
      MyEmissiveColorStateResult result1;
      if (!this.IsFunctional)
      {
        MyEmissiveColorStateResult result2;
        if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Damaged, out result2))
          emissivePartColor1 = result2.EmissiveColor;
        for (int index = 0; index < 4; ++index)
          MyEntity.UpdateNamedEmissiveParts(this.Render.RenderObjectIDs[0], MySolarPanel.m_emissiveTextureNames[index], emissivePartColor1, 0.0f);
      }
      else if (!this.IsWorking)
      {
        MyEmissiveColorStateResult result2;
        if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Disabled, out result2))
          emissivePartColor1 = result2.EmissiveColor;
        for (int index = 0; index < 4; ++index)
          MyEntity.UpdateNamedEmissiveParts(this.Render.RenderObjectIDs[0], MySolarPanel.m_emissiveTextureNames[index], emissivePartColor1, 1f);
      }
      else if ((double) this.SourceComp.MaxOutput > 0.0)
      {
        for (int index = 0; index < 4; ++index)
        {
          if ((double) index < (double) this.SourceComp.MaxOutput / (double) this.BlockDefinition.MaxPowerOutput * 4.0)
          {
            Color emissivePartColor2 = Color.Green;
            if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Working, out result1))
              emissivePartColor2 = result1.EmissiveColor;
            MyEntity.UpdateNamedEmissiveParts(this.Render.RenderObjectIDs[0], MySolarPanel.m_emissiveTextureNames[index], emissivePartColor2, 1f);
          }
          else
          {
            Color emissivePartColor2 = Color.Black;
            if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Damaged, out result1))
              emissivePartColor2 = result1.EmissiveColor;
            MyEntity.UpdateNamedEmissiveParts(this.Render.RenderObjectIDs[0], MySolarPanel.m_emissiveTextureNames[index], emissivePartColor2, 1f);
          }
        }
      }
      else
      {
        MyEmissiveColorStateResult result2;
        if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Warning, out result2))
          emissivePartColor1 = result2.EmissiveColor;
        MyEntity.UpdateNamedEmissiveParts(this.Render.RenderObjectIDs[0], MySolarPanel.m_emissiveTextureNames[0], emissivePartColor1, 1f);
        for (int index = 1; index < 4; ++index)
          MyEntity.UpdateNamedEmissiveParts(this.Render.RenderObjectIDs[0], MySolarPanel.m_emissiveTextureNames[index], emissivePartColor1, 1f);
      }
    }
  }
}
