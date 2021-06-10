// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.MyGuiScreenDebugHeadlamp
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using System;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
  [MyDebugScreen("Cosmetics", "Headlamp")]
  internal class MyGuiScreenDebugHeadlamp : MyGuiScreenDebugBase
  {
    public override string GetFriendlyName() => nameof (MyGuiScreenDebugHeadlamp);

    public MyGuiScreenDebugHeadlamp()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.m_currentPosition.Y += 0.01f;
      this.m_scale = 0.7f;
      this.AddCaption("Headlamp", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition.Y += 0.01f;
      this.AddLabel("Spot", Color.Yellow.ToVector4(), 1.2f);
      this.AddColor("Color", (Color) MyCharacter.REFLECTOR_COLOR, (Action<MyGuiControlColor>) (v =>
      {
        MyCharacter.REFLECTOR_COLOR = (Vector4) v.Color;
        MyCharacter.LIGHT_PARAMETERS_CHANGED = true;
      }));
      this.AddSlider("Falloff", MyCharacter.REFLECTOR_FALLOFF, 0.0f, 5f, (Action<MyGuiControlSlider>) (slider =>
      {
        MyCharacter.REFLECTOR_FALLOFF = slider.Value;
        MyCharacter.LIGHT_PARAMETERS_CHANGED = true;
      }));
      this.AddSlider("Gloss factor", MyCharacter.REFLECTOR_GLOSS_FACTOR, 0.0f, 5f, (Action<MyGuiControlSlider>) (slider =>
      {
        MyCharacter.REFLECTOR_GLOSS_FACTOR = slider.Value;
        MyCharacter.LIGHT_PARAMETERS_CHANGED = true;
      }));
      this.AddSlider("Diffuse factor", MyCharacter.REFLECTOR_DIFFUSE_FACTOR, 0.0f, 5f, (Action<MyGuiControlSlider>) (slider =>
      {
        MyCharacter.REFLECTOR_DIFFUSE_FACTOR = slider.Value;
        MyCharacter.LIGHT_PARAMETERS_CHANGED = true;
      }));
      this.AddSlider("Intensity", MyCharacter.REFLECTOR_INTENSITY, 0.0f, 200f, (Action<MyGuiControlSlider>) (slider =>
      {
        MyCharacter.REFLECTOR_INTENSITY = slider.Value;
        MyCharacter.LIGHT_PARAMETERS_CHANGED = true;
      }));
      this.m_currentPosition.Y += 0.01f;
      this.AddLabel("Point", Color.Yellow.ToVector4(), 1.2f);
      this.AddColor("Color", (Color) MyCharacter.POINT_COLOR, (Action<MyGuiControlColor>) (v =>
      {
        MyCharacter.POINT_COLOR = (Vector4) v.Color;
        MyCharacter.LIGHT_PARAMETERS_CHANGED = true;
      }));
      this.AddSlider("Falloff", MyCharacter.POINT_FALLOFF, 0.0f, 5f, (Action<MyGuiControlSlider>) (slider =>
      {
        MyCharacter.POINT_FALLOFF = slider.Value;
        MyCharacter.LIGHT_PARAMETERS_CHANGED = true;
      }));
      this.AddSlider("Gloss factor", MyCharacter.POINT_GLOSS_FACTOR, 0.0f, 5f, (Action<MyGuiControlSlider>) (slider =>
      {
        MyCharacter.POINT_GLOSS_FACTOR = slider.Value;
        MyCharacter.LIGHT_PARAMETERS_CHANGED = true;
      }));
      this.AddSlider("Diffuse factor", MyCharacter.POINT_DIFFUSE_FACTOR, 0.0f, 5f, (Action<MyGuiControlSlider>) (slider =>
      {
        MyCharacter.POINT_DIFFUSE_FACTOR = slider.Value;
        MyCharacter.LIGHT_PARAMETERS_CHANGED = true;
      }));
      this.AddSlider("Intensity", MyCharacter.POINT_LIGHT_INTENSITY, 0.0f, 50f, (Action<MyGuiControlSlider>) (slider =>
      {
        MyCharacter.POINT_LIGHT_INTENSITY = slider.Value;
        MyCharacter.LIGHT_PARAMETERS_CHANGED = true;
      }));
      this.AddSlider("Range", MyCharacter.POINT_LIGHT_RANGE, 0.0f, 10f, (Action<MyGuiControlSlider>) (slider =>
      {
        MyCharacter.POINT_LIGHT_RANGE = slider.Value;
        MyCharacter.LIGHT_PARAMETERS_CHANGED = true;
      }));
    }
  }
}
