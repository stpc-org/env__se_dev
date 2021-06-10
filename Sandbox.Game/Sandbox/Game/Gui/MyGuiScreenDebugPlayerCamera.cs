// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugPlayerCamera
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.World;
using System;
using VRage.Game.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  [MyDebugScreen("Game", "Player Camera")]
  internal class MyGuiScreenDebugPlayerCamera : MyGuiScreenDebugBase
  {
    public MyGuiScreenDebugPlayerCamera()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.AddCaption("Player Head Shake", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_scale = 0.7f;
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.m_currentPosition.Y += 0.01f;
      if (MySector.MainCamera != null)
      {
        MyCameraSpring cameraSpring = MySector.MainCamera.CameraSpring;
        this.AddLabel("Camera target spring", Color.Yellow.ToVector4(), 1f);
        this.AddSlider("Stiffness", 0.0f, 50f, (Func<float>) (() => cameraSpring.SpringStiffness), (Action<float>) (s => cameraSpring.SpringStiffness = s));
        this.AddSlider("Dampening", 0.0f, 1f, (Func<float>) (() => cameraSpring.SpringDampening), (Action<float>) (s => cameraSpring.SpringDampening = s));
        this.AddSlider("CenterMaxVelocity", 0.0f, 10f, (Func<float>) (() => cameraSpring.SpringMaxVelocity), (Action<float>) (s => cameraSpring.SpringMaxVelocity = s));
        this.AddSlider("SpringMaxLength", 0.0f, 2f, (Func<float>) (() => cameraSpring.SpringMaxLength), (Action<float>) (s => cameraSpring.SpringMaxLength = s));
      }
      this.m_currentPosition.Y += 0.01f;
      if (MyThirdPersonSpectator.Static == null)
        return;
      this.AddLabel("Third person spectator", Color.Yellow.ToVector4(), 1f);
      this.m_currentPosition.Y += 0.01f;
      this.AddCheckBox("Debug draw", (Func<bool>) (() => MyThirdPersonSpectator.Static.EnableDebugDraw), (Action<bool>) (s => MyThirdPersonSpectator.Static.EnableDebugDraw = s));
      this.AddLabel("Normal spring", Color.Yellow.ToVector4(), 0.7f);
      this.AddSlider("Stiffness", 1f, 50000f, (Func<float>) (() => MyThirdPersonSpectator.Static.NormalSpring.Stiffness), (Action<float>) (s => MyThirdPersonSpectator.Static.NormalSpring.Stiffness = s));
      this.AddSlider("Damping", 1f, 5000f, (Func<float>) (() => MyThirdPersonSpectator.Static.NormalSpring.Dampening), (Action<float>) (s => MyThirdPersonSpectator.Static.NormalSpring.Dampening = s));
      this.AddSlider("Mass", 0.1f, 500f, (Func<float>) (() => MyThirdPersonSpectator.Static.NormalSpring.Mass), (Action<float>) (s => MyThirdPersonSpectator.Static.NormalSpring.Mass = s));
    }

    public override string GetFriendlyName() => "MyGuiScreenDebugPlayerShake";
  }
}
