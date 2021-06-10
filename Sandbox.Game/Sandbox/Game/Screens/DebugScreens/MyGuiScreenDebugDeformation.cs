// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.MyGuiScreenDebugDeformation
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using System;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
  [MyDebugScreen("VRage", "Deformation")]
  public class MyGuiScreenDebugDeformation : MyGuiScreenDebugBase
  {
    public MyGuiScreenDebugDeformation()
      : base()
      => this.RecreateControls(true);

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugDeformation);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.BackgroundColor = new Vector4?(new Vector4(1f, 1f, 1f, 0.5f));
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.13f);
      this.AddCaption("Deformation", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.AddSlider("Break Velocity", 0.0f, 100f, (Func<float>) (() => MyFakes.DEFORMATION_MINIMUM_VELOCITY), (Action<float>) (v => MyFakes.DEFORMATION_MINIMUM_VELOCITY = v));
      this.m_currentPosition.Y += 0.01f;
      this.AddSlider("Vertical Offset Ratio", 0.0f, 5f, (Func<float>) (() => MyFakes.DEFORMATION_OFFSET_RATIO), (Action<float>) (v => MyFakes.DEFORMATION_OFFSET_RATIO = v));
      this.AddSlider("Vertical Offset Limit", 0.0f, 100f, (Func<float>) (() => MyFakes.DEFORMATION_OFFSET_MAX), (Action<float>) (v => MyFakes.DEFORMATION_OFFSET_MAX = v));
      this.m_currentPosition.Y += 0.01f;
      this.AddSlider("Velocity Relay", 0.0f, 1f, (Func<float>) (() => MyFakes.DEFORMATION_VELOCITY_RELAY), (Action<float>) (v => MyFakes.DEFORMATION_VELOCITY_RELAY = v));
      this.m_currentPosition.Y += 0.01f;
      this.AddSlider("Projectile Vertical Offset", 0.0f, 0.05f, (Func<float>) (() => MyFakes.DEFORMATION_PROJECTILE_OFFSET_RATIO), (Action<float>) (v => MyFakes.DEFORMATION_PROJECTILE_OFFSET_RATIO = v));
      this.AddCaption("Simple controls (use on your own risk)");
      this.m_currentPosition.Y += 0.01f;
      this.AddCheckBox("Voxel cutouts enabled", MyFakes.DEFORMATION_EXPLOSIONS, (Action<MyGuiControlCheckbox>) (x => MyFakes.DEFORMATION_EXPLOSIONS = x.IsChecked));
      this.AddSlider("Voxel cutouts multiplier", 0.0f, 15f, (Func<float>) (() => MyFakes.DEFORMATION_VOXEL_CUTOUT_MULTIPLIER), (Action<float>) (v => MyFakes.DEFORMATION_VOXEL_CUTOUT_MULTIPLIER = v));
      this.AddSlider("Voxel cutout max radius", 0.0f, 100f, (Func<float>) (() => MyFakes.DEFORMATION_VOXEL_CUTOUT_MAX_RADIUS), (Action<float>) (v => MyFakes.DEFORMATION_VOXEL_CUTOUT_MAX_RADIUS = v));
      this.AddSlider("Grid damage multiplier", 0.0f, 10f, (Func<float>) (() => MyFakes.DEFORMATION_DAMAGE_MULTIPLIER), (Action<float>) (v => MyFakes.DEFORMATION_DAMAGE_MULTIPLIER = v));
    }
  }
}
