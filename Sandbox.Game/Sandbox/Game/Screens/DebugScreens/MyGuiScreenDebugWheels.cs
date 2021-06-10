// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.MyGuiScreenDebugWheels
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using System;
using System.Linq.Expressions;
using VRage;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
  [MyDebugScreen("VRage", "Wheels")]
  public class MyGuiScreenDebugWheels : MyGuiScreenDebugBase
  {
    public MyGuiScreenDebugWheels()
      : base()
      => this.RecreateControls(true);

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugWheels);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.BackgroundColor = new Vector4?(new Vector4(1f, 1f, 1f, 0.5f));
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.13f);
      this.AddCaption("Wheels", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.AddCaption("DebugDraw");
      this.AddCheckBox("Physics", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_WHEEL_PHYSICS)));
      this.AddCheckBox("Systems", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_WHEEL_SYSTEMS)));
      this.AddCheckBox("Draw voxel contact materials", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_VOXEL_CONTACT_MATERIAL)));
      this.AddCheckBox("Disable Wheel Trails", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_DISABLE_TRACKTRAILS)));
      this.AddSubcaption("Response modifier");
      this.AddCheckBox("Prediction", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.MULTIPLAYER_CLIENT_SIMULATE_CONTROLLED_CAR)));
      this.AddSlider("Max accel", 0.0f, 0.1f, (Func<float>) (() => MyPhysicsConfig.WheelSoftnessVelocity), (Action<float>) (v => MyPhysicsConfig.WheelSoftnessVelocity = v));
      this.AddSlider("Softness ratio", 0.0f, 1f, (Func<float>) (() => MyPhysicsConfig.WheelSoftnessRatio), (Action<float>) (v => MyPhysicsConfig.WheelSoftnessRatio = v));
      this.AddSubcaption("Steering model");
      this.AddSlider("Slip countdown", 0.0f, 100f, (Func<float>) (() => (float) MyPhysicsConfig.WheelSlipCountdown), (Action<float>) (x => MyPhysicsConfig.WheelSlipCountdown = (int) x));
      this.AddSlider("Impulse Blending", 0.0f, 1f, (Func<float>) (() => MyPhysicsConfig.WheelImpulseBlending), (Action<float>) (x => MyPhysicsConfig.WheelImpulseBlending = x));
      this.AddSlider("Impulse Blending", 0.0f, 1f, (Func<float>) (() => MyPhysicsConfig.WheelImpulseBlending), (Action<float>) (x => MyPhysicsConfig.WheelImpulseBlending = x));
      this.AddSlider("Slip CutAway Ratio", 0.0f, 1f, (Func<float>) (() => MyPhysicsConfig.WheelSlipCutAwayRatio), (Action<float>) (x => MyPhysicsConfig.WheelSlipCutAwayRatio = x));
      this.AddSlider("Surface Material steering ratio", 0.0f, 1f, (Func<float>) (() => MyPhysicsConfig.WheelSurfaceMaterialSteerRatio), (Action<float>) (x => MyPhysicsConfig.WheelSurfaceMaterialSteerRatio = x));
      this.AddCheckBox("Override axle friction", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyPhysicsConfig.OverrideWheelAxleFriction)));
      this.AddSlider("Axle friction", 0.0f, 10000f, (Func<float>) (() => MyPhysicsConfig.WheelAxleFriction), (Action<float>) (x => MyPhysicsConfig.WheelAxleFriction = x));
      this.AddSlider("Artificial breaking", 0.0f, 10f, (Func<float>) (() => MyPhysicsConfig.ArtificialBrakingMultiplier), (Action<float>) (x => MyPhysicsConfig.ArtificialBrakingMultiplier = x));
      this.AddSlider("Artificial breaking CoM stabilization", 0.0f, 1f, (Func<float>) (() => MyPhysicsConfig.ArtificialBrakingCoMStabilization), (Action<float>) (x => MyPhysicsConfig.ArtificialBrakingCoMStabilization = x));
    }
  }
}
