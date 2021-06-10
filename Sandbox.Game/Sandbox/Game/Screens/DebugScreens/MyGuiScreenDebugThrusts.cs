// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.MyGuiScreenDebugThrusts
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using Sandbox.RenderDirect.ActorComponents;
using System;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
  [MyDebugScreen("Cosmetics", "Thrusts visual")]
  internal class MyGuiScreenDebugThrusts : MyGuiScreenDebugBase
  {
    public override string GetFriendlyName() => nameof (MyGuiScreenDebugThrusts);

    public MyGuiScreenDebugThrusts()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.m_currentPosition.Y += 0.01f;
      this.m_scale = 0.7f;
      this.AddCaption("Thrusts visual", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition.Y += 0.01f;
      this.AddLabel("Thrust Light", Color.Yellow.ToVector4(), 1.2f);
      this.AddSlider("Intensity const", MyThrustFlameAnimator.LIGHT_INTENSITY_BASE, 0.0f, 1000f, (Action<MyGuiControlSlider>) (slider => MyThrustFlameAnimator.LIGHT_INTENSITY_BASE = slider.Value));
      this.AddSlider("Intensity from thrust length", MyThrustFlameAnimator.LIGHT_INTENSITY_LENGTH, 0.0f, 1000f, (Action<MyGuiControlSlider>) (slider => MyThrustFlameAnimator.LIGHT_INTENSITY_LENGTH = slider.Value));
      this.AddSlider("Range from thrust radius", MyThrustFlameAnimator.LIGHT_RANGE_RADIUS, 0.0f, 10f, (Action<MyGuiControlSlider>) (slider => MyThrustFlameAnimator.LIGHT_RANGE_RADIUS = slider.Value));
      this.AddSlider("Range from thrust length", MyThrustFlameAnimator.LIGHT_RANGE_LENGTH, 0.0f, 10f, (Action<MyGuiControlSlider>) (slider => MyThrustFlameAnimator.LIGHT_RANGE_LENGTH = slider.Value));
      this.m_currentPosition.Y += 0.01f;
      this.AddLabel("Thrust Glare", Color.Yellow.ToVector4(), 1.2f);
      this.AddSlider("Intensity const", MyThrustFlameAnimator.GLARE_INTENSITY_BASE, 0.0f, 3f, (Action<MyGuiControlSlider>) (slider => MyThrustFlameAnimator.GLARE_INTENSITY_BASE = slider.Value));
      this.AddSlider("Intensity from thrust length", MyThrustFlameAnimator.GLARE_INTENSITY_LENGTH, 0.0f, 10f, (Action<MyGuiControlSlider>) (slider => MyThrustFlameAnimator.GLARE_INTENSITY_LENGTH = slider.Value));
      this.AddSlider("Size from thrust radius", MyThrustFlameAnimator.GLARE_SIZE_RADIUS, 0.0f, 10f, (Action<MyGuiControlSlider>) (slider => MyThrustFlameAnimator.GLARE_SIZE_RADIUS = slider.Value));
      this.AddSlider("Size from thrust length", MyThrustFlameAnimator.GLARE_SIZE_LENGTH, 0.0f, 10f, (Action<MyGuiControlSlider>) (slider => MyThrustFlameAnimator.GLARE_SIZE_LENGTH = slider.Value));
      this.AddLabel("Thrust", Color.Yellow.ToVector4(), 1.2f);
      this.AddSlider("Intensity", MyThrustFlameAnimator.THRUST_INTENSITY, 0.0f, 10f, (Action<MyGuiControlSlider>) (slider => MyThrustFlameAnimator.THRUST_INTENSITY = slider.Value));
      this.AddSlider("Intensity from thrust length", MyThrustFlameAnimator.THRUST_LENGTH_INTENSITY, 0.0f, 2f, (Action<MyGuiControlSlider>) (slider => MyThrustFlameAnimator.THRUST_LENGTH_INTENSITY = slider.Value));
      this.AddSlider("Radius", MyThrustFlameAnimator.THRUST_THICKNESS, 0.0f, 10f, (Action<MyGuiControlSlider>) (slider => MyThrustFlameAnimator.THRUST_THICKNESS = slider.Value));
      this.m_currentPosition.Y += 0.02f;
    }
  }
}
