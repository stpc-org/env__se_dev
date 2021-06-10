// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.MyGuiScreenDebugJetpack
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Components;
using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using System;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
  [MyDebugScreen("Cosmetics", "Jetpack visual")]
  internal class MyGuiScreenDebugJetpack : MyGuiScreenDebugBase
  {
    public override string GetFriendlyName() => nameof (MyGuiScreenDebugJetpack);

    public MyGuiScreenDebugJetpack()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.m_currentPosition.Y += 0.01f;
      this.m_scale = 0.7f;
      this.AddCaption("Jetpack visual", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition.Y += 0.01f;
      this.AddLabel("Light", Color.Yellow.ToVector4(), 1.2f);
      this.AddSlider("Intensity const", MyRenderComponentCharacter.JETPACK_LIGHT_INTENSITY_BASE, 0.0f, 1000f, (Action<MyGuiControlSlider>) (slider => MyRenderComponentCharacter.JETPACK_LIGHT_INTENSITY_BASE = slider.Value));
      this.AddSlider("Intensity from thrust length", MyRenderComponentCharacter.JETPACK_LIGHT_INTENSITY_LENGTH, 0.0f, 1000f, (Action<MyGuiControlSlider>) (slider => MyRenderComponentCharacter.JETPACK_LIGHT_INTENSITY_LENGTH = slider.Value));
      this.AddSlider("Range from thrust radius", MyRenderComponentCharacter.JETPACK_LIGHT_RANGE_RADIUS, 0.0f, 10f, (Action<MyGuiControlSlider>) (slider => MyRenderComponentCharacter.JETPACK_LIGHT_RANGE_RADIUS = slider.Value));
      this.AddSlider("Range from thrust length", MyRenderComponentCharacter.JETPACK_LIGHT_RANGE_LENGTH, 0.0f, 10f, (Action<MyGuiControlSlider>) (slider => MyRenderComponentCharacter.JETPACK_LIGHT_RANGE_LENGTH = slider.Value));
      this.m_currentPosition.Y += 0.01f;
      this.AddLabel("Glare", Color.Yellow.ToVector4(), 1.2f);
      this.AddSlider("Intensity const", MyRenderComponentCharacter.JETPACK_GLARE_INTENSITY_BASE, 0.0f, 10f, (Action<MyGuiControlSlider>) (slider => MyRenderComponentCharacter.JETPACK_GLARE_INTENSITY_BASE = slider.Value));
      this.AddSlider("Intensity from thrust length", MyRenderComponentCharacter.JETPACK_GLARE_INTENSITY_LENGTH, 0.0f, 100f, (Action<MyGuiControlSlider>) (slider => MyRenderComponentCharacter.JETPACK_GLARE_INTENSITY_LENGTH = slider.Value));
      this.AddSlider("Size from thrust radius", MyRenderComponentCharacter.JETPACK_GLARE_SIZE_RADIUS, 0.0f, 10f, (Action<MyGuiControlSlider>) (slider => MyRenderComponentCharacter.JETPACK_GLARE_SIZE_RADIUS = slider.Value));
      this.AddSlider("Size from thrust length", MyRenderComponentCharacter.JETPACK_GLARE_SIZE_LENGTH, 0.0f, 10f, (Action<MyGuiControlSlider>) (slider => MyRenderComponentCharacter.JETPACK_GLARE_SIZE_LENGTH = slider.Value));
      this.m_currentPosition.Y += 0.02f;
      this.AddLabel("Thrust", Color.Yellow.ToVector4(), 1.2f);
      this.AddSlider("Intensity base", MyRenderComponentCharacter.JETPACK_THRUST_INTENSITY_BASE, 0.0f, 10f, (Action<MyGuiControlSlider>) (slider => MyRenderComponentCharacter.JETPACK_THRUST_INTENSITY_BASE = slider.Value));
      this.AddSlider("Intensity", MyRenderComponentCharacter.JETPACK_THRUST_INTENSITY, 0.0f, 10f, (Action<MyGuiControlSlider>) (slider => MyRenderComponentCharacter.JETPACK_THRUST_INTENSITY = slider.Value));
      this.AddSlider("Radius", MyRenderComponentCharacter.JETPACK_THRUST_THICKNESS, 0.0f, 10f, (Action<MyGuiControlSlider>) (slider => MyRenderComponentCharacter.JETPACK_THRUST_THICKNESS = slider.Value));
      this.AddSlider("Length", MyRenderComponentCharacter.JETPACK_THRUST_LENGTH, 0.0f, 10f, (Action<MyGuiControlSlider>) (slider => MyRenderComponentCharacter.JETPACK_THRUST_LENGTH = slider.Value));
      this.AddSlider("Offset", MyRenderComponentCharacter.JETPACK_THRUST_OFFSET, -1f, 1f, (Action<MyGuiControlSlider>) (slider => MyRenderComponentCharacter.JETPACK_THRUST_OFFSET = slider.Value));
      this.m_currentPosition.Y += 0.02f;
    }
  }
}
