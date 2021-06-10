// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.MyGuiScreenDebugRenderLodding
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Screens.DebugScreens
{
  [MyDebugScreen("Render", "Lodding")]
  public class MyGuiScreenDebugRenderLodding : MyGuiScreenDebugBase
  {
    public MyGuiScreenDebugRenderLodding()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.m_sliderDebugScale = 0.7f;
      this.AddCaption("Lodding", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.m_currentPosition.Y += 0.01f;
      this.AddLabel("The new pipeline - lod shift", (Vector4) Color.White, 1f);
      this.AddSlider("GBuffer", (float) MySector.Lodding.CurrentSettings.GBuffer.LodShift, 0.0f, 6f, (Action<MyGuiControlSlider>) (x =>
      {
        MySector.Lodding.CurrentSettings.GBuffer.LodShift = (int) Math.Round((double) x.Value);
        MySector.Lodding.CurrentSettings.GBuffer.LodShiftVisible = (int) Math.Round((double) x.Value);
      }));
      if (MySector.Lodding.CurrentSettings.CascadeDepths.Length >= 3)
      {
        this.AddSlider("CSM_0 Visible in gbuffer", (float) MySector.Lodding.CurrentSettings.CascadeDepths[0].LodShiftVisible, 0.0f, 6f, (Action<MyGuiControlSlider>) (x => MySector.Lodding.CurrentSettings.CascadeDepths[0].LodShiftVisible = (int) Math.Round((double) x.Value)));
        this.AddSlider("CSM_0", (float) MySector.Lodding.CurrentSettings.CascadeDepths[0].LodShift, 0.0f, 6f, (Action<MyGuiControlSlider>) (x => MySector.Lodding.CurrentSettings.CascadeDepths[0].LodShift = (int) Math.Round((double) x.Value)));
        this.AddSlider("CSM_1 Visible in gbuffer", (float) MySector.Lodding.CurrentSettings.CascadeDepths[1].LodShiftVisible, 0.0f, 6f, (Action<MyGuiControlSlider>) (x => MySector.Lodding.CurrentSettings.CascadeDepths[1].LodShiftVisible = (int) Math.Round((double) x.Value)));
        this.AddSlider("CSM_1", (float) MySector.Lodding.CurrentSettings.CascadeDepths[1].LodShift, 0.0f, 6f, (Action<MyGuiControlSlider>) (x => MySector.Lodding.CurrentSettings.CascadeDepths[1].LodShift = (int) Math.Round((double) x.Value)));
        this.AddSlider("CSM_2 Visible in gbuffer", (float) MySector.Lodding.CurrentSettings.CascadeDepths[2].LodShiftVisible, 0.0f, 6f, (Action<MyGuiControlSlider>) (x => MySector.Lodding.CurrentSettings.CascadeDepths[2].LodShiftVisible = (int) Math.Round((double) x.Value)));
        this.AddSlider("CSM_2", (float) MySector.Lodding.CurrentSettings.CascadeDepths[2].LodShift, 0.0f, 6f, (Action<MyGuiControlSlider>) (x => MySector.Lodding.CurrentSettings.CascadeDepths[2].LodShift = (int) Math.Round((double) x.Value)));
      }
      this.AddSlider("Single depth visible in gbuffer", (float) MySector.Lodding.CurrentSettings.SingleDepth.LodShiftVisible, 0.0f, 6f, (Action<MyGuiControlSlider>) (x => MySector.Lodding.CurrentSettings.SingleDepth.LodShiftVisible = (int) Math.Round((double) x.Value)));
      this.AddSlider("Single depth", (float) MySector.Lodding.CurrentSettings.SingleDepth.LodShift, 0.0f, 6f, (Action<MyGuiControlSlider>) (x => MySector.Lodding.CurrentSettings.SingleDepth.LodShift = (int) Math.Round((double) x.Value)));
      this.AddLabel("The new pipeline - min lod", (Vector4) Color.White, 1f);
      this.AddSlider("GBuffer", (float) MySector.Lodding.CurrentSettings.GBuffer.MinLod, 0.0f, 6f, (Action<MyGuiControlSlider>) (x => MySector.Lodding.CurrentSettings.GBuffer.MinLod = (int) Math.Round((double) x.Value)));
      if (MySector.Lodding.CurrentSettings.CascadeDepths.Length >= 3)
      {
        this.AddSlider("CSM_0", (float) MySector.Lodding.CurrentSettings.CascadeDepths[0].MinLod, 0.0f, 6f, (Action<MyGuiControlSlider>) (x => MySector.Lodding.CurrentSettings.CascadeDepths[0].MinLod = (int) Math.Round((double) x.Value)));
        this.AddSlider("CSM_1", (float) MySector.Lodding.CurrentSettings.CascadeDepths[1].MinLod, 0.0f, 6f, (Action<MyGuiControlSlider>) (x => MySector.Lodding.CurrentSettings.CascadeDepths[1].MinLod = (int) Math.Round((double) x.Value)));
        this.AddSlider("CSM_2", (float) MySector.Lodding.CurrentSettings.CascadeDepths[2].MinLod, 0.0f, 6f, (Action<MyGuiControlSlider>) (x => MySector.Lodding.CurrentSettings.CascadeDepths[2].MinLod = (int) Math.Round((double) x.Value)));
      }
      this.AddSlider("Single depth", (float) MySector.Lodding.CurrentSettings.SingleDepth.MinLod, 0.0f, 6f, (Action<MyGuiControlSlider>) (x => MySector.Lodding.CurrentSettings.SingleDepth.MinLod = (int) Math.Round((double) x.Value)));
      this.AddLabel("The new pipeline - global", (Vector4) Color.White, 1f);
      this.AddSlider("Object distance mult", MySector.Lodding.CurrentSettings.Global.ObjectDistanceMult, 0.01f, 8f, (Action<MyGuiControlSlider>) (x => MySector.Lodding.CurrentSettings.Global.ObjectDistanceMult = x.Value));
      this.AddSlider("Object distance add", MySector.Lodding.CurrentSettings.Global.ObjectDistanceAdd, -100f, 100f, (Action<MyGuiControlSlider>) (x => MySector.Lodding.CurrentSettings.Global.ObjectDistanceAdd = x.Value));
      this.AddSlider("Min transition in seconds", MySector.Lodding.CurrentSettings.Global.MinTransitionInSeconds, 0.0f, 2f, (Action<MyGuiControlSlider>) (x => MySector.Lodding.CurrentSettings.Global.MinTransitionInSeconds = x.Value));
      this.AddSlider("Max transition in seconds", MySector.Lodding.CurrentSettings.Global.MaxTransitionInSeconds, 0.0f, 2f, (Action<MyGuiControlSlider>) (x => MySector.Lodding.CurrentSettings.Global.MaxTransitionInSeconds = x.Value));
      this.AddSlider("Transition dead zone - const", MySector.Lodding.CurrentSettings.Global.TransitionDeadZoneConst, 0.0f, 2f, (Action<MyGuiControlSlider>) (x => MySector.Lodding.CurrentSettings.Global.TransitionDeadZoneConst = x.Value));
      this.AddSlider("Transition dead zone - dist mult", MySector.Lodding.CurrentSettings.Global.TransitionDeadZoneDistanceMult, 0.0f, 2f, (Action<MyGuiControlSlider>) (x => MySector.Lodding.CurrentSettings.Global.TransitionDeadZoneDistanceMult = x.Value));
      this.AddSlider("Lod histeresis ratio", MySector.Lodding.CurrentSettings.Global.HisteresisRatio, 0.0f, 1f, (Action<MyGuiControlSlider>) (x => MySector.Lodding.CurrentSettings.Global.HisteresisRatio = x.Value));
      this.AddCheckBox("Update lods", (Func<bool>) (() => MySector.Lodding.CurrentSettings.Global.IsUpdateEnabled), (Action<bool>) (x => MySector.Lodding.CurrentSettings.Global.IsUpdateEnabled = x));
      this.AddCheckBox("Display lod", MyRenderProxy.Settings.DisplayGbufferLOD, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayGbufferLOD = x.IsChecked));
      this.AddCheckBox("Enable lod selection", MySector.Lodding.CurrentSettings.Global.EnableLodSelection, (Action<MyGuiControlCheckbox>) (x => MySector.Lodding.CurrentSettings.Global.EnableLodSelection = x.IsChecked));
      this.AddSlider("Lod selection", (float) MySector.Lodding.CurrentSettings.Global.LodSelection, 0.0f, 5f, (Action<MyGuiControlSlider>) (x => MySector.Lodding.CurrentSettings.Global.LodSelection = (int) Math.Round((double) x.Value)));
    }

    protected override void ValueChanged(MyGuiControlBase sender)
    {
      MyRenderProxy.SetSettingsDirty();
      MyRenderProxy.UpdateNewLoddingSettings(MySector.Lodding.CurrentSettings);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugRenderLodding);
  }
}
