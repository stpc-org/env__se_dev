// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugNetworkPrediction
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Replication.History;
using Sandbox.Graphics.GUI;
using System;
using VRageMath;

namespace Sandbox.Game.Gui
{
  [MyDebugScreen("VRage", "Network Prediction")]
  internal class MyGuiScreenDebugNetworkPrediction : MyGuiScreenDebugBase
  {
    private MyGuiControlCombobox m_animationComboA;
    private MyGuiControlCombobox m_animationComboB;
    private MyGuiControlSlider m_blendSlider;
    private MyGuiControlCombobox m_animationCombo;
    private MyGuiControlCheckbox m_loopCheckbox;
    private const float m_forcedPriority = 1f;

    public MyGuiScreenDebugNetworkPrediction()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.m_sliderDebugScale = 0.7f;
      this.AddCaption("Network Prediction", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.m_currentPosition.Y += 0.01f;
      if (MyMultiplayer.Static == null)
        return;
      this.AddCheckBox("Apply Corrections", MyPredictedSnapshotSync.POSITION_CORRECTION && MyPredictedSnapshotSync.SMOOTH_POSITION_CORRECTION && (MyPredictedSnapshotSync.LINEAR_VELOCITY_CORRECTION && MyPredictedSnapshotSync.SMOOTH_LINEAR_VELOCITY_CORRECTION) && (MyPredictedSnapshotSync.ROTATION_CORRECTION && MyPredictedSnapshotSync.SMOOTH_ROTATION_CORRECTION && MyPredictedSnapshotSync.ANGULAR_VELOCITY_CORRECTION) && MyPredictedSnapshotSync.SMOOTH_ANGULAR_VELOCITY_CORRECTION, (Action<MyGuiControlCheckbox>) (x =>
      {
        MyPredictedSnapshotSync.POSITION_CORRECTION = x.IsChecked;
        MyPredictedSnapshotSync.SMOOTH_POSITION_CORRECTION = x.IsChecked;
        MyPredictedSnapshotSync.LINEAR_VELOCITY_CORRECTION = x.IsChecked;
        MyPredictedSnapshotSync.SMOOTH_LINEAR_VELOCITY_CORRECTION = x.IsChecked;
        MyPredictedSnapshotSync.ROTATION_CORRECTION = x.IsChecked;
        MyPredictedSnapshotSync.SMOOTH_ROTATION_CORRECTION = x.IsChecked;
        MyPredictedSnapshotSync.ANGULAR_VELOCITY_CORRECTION = x.IsChecked;
        MyPredictedSnapshotSync.SMOOTH_ANGULAR_VELOCITY_CORRECTION = x.IsChecked;
      }));
      this.AddCheckBox("Apply Trend Reset", MyPredictedSnapshotSync.ApplyTrend, (Action<MyGuiControlCheckbox>) (x => MyPredictedSnapshotSync.ApplyTrend = x.IsChecked));
      this.AddCheckBox("Force animated", MyPredictedSnapshotSync.ForceAnimated, (Action<MyGuiControlCheckbox>) (x => MyPredictedSnapshotSync.ForceAnimated = x.IsChecked));
      this.AddSlider("Velocity change to reset", MyPredictedSnapshotSync.MIN_VELOCITY_CHANGE_TO_RESET, 0.0f, 30f, (Action<MyGuiControlSlider>) (slider => MyPredictedSnapshotSync.MIN_VELOCITY_CHANGE_TO_RESET = slider.Value));
      this.AddSlider("Delta factor", MyPredictedSnapshotSync.DELTA_FACTOR, 0.0f, 1f, (Action<MyGuiControlSlider>) (slider => MyPredictedSnapshotSync.DELTA_FACTOR = slider.Value));
      this.AddSlider("Smooth iterations", (float) MyPredictedSnapshotSync.SMOOTH_ITERATIONS, 0.0f, 1000f, (Action<MyGuiControlSlider>) (slider => MyPredictedSnapshotSync.SMOOTH_ITERATIONS = (int) slider.Value));
      this.AddCheckBox("Apply Reset", MySnapshot.ApplyReset, (Action<MyGuiControlCheckbox>) (x => MySnapshot.ApplyReset = x.IsChecked));
      this.AddSlider("Smooth distance", MyPredictedSnapshotSync.SMOOTH_DISTANCE, 0.0f, 1000f, (Action<MyGuiControlSlider>) (slider => MyPredictedSnapshotSync.SMOOTH_DISTANCE = (float) (int) slider.Value));
      this.AddCheckBox("Propagate To Connections", MySnapshotCache.PROPAGATE_TO_CONNECTIONS, (Action<MyGuiControlCheckbox>) (x => MySnapshotCache.PROPAGATE_TO_CONNECTIONS = x.IsChecked));
      this.m_currentPosition.Y += 0.01f;
      this.AddCheckBox("Position corrections", MyPredictedSnapshotSync.POSITION_CORRECTION, (Action<MyGuiControlCheckbox>) (x => MyPredictedSnapshotSync.POSITION_CORRECTION = x.IsChecked));
      this.AddCheckBox("Smooth position corrections", MyPredictedSnapshotSync.SMOOTH_POSITION_CORRECTION, (Action<MyGuiControlCheckbox>) (x => MyPredictedSnapshotSync.SMOOTH_POSITION_CORRECTION = x.IsChecked));
      this.AddSlider("Minimum pos delta", MyPredictedSnapshotSync.MIN_POSITION_DELTA, 0.0f, 0.5f, (Action<MyGuiControlSlider>) (slider => MyPredictedSnapshotSync.MIN_POSITION_DELTA = slider.Value));
      this.AddSlider("Maximum pos delta", MyPredictedSnapshotSync.MAX_POSITION_DELTA, 0.0f, 5f, (Action<MyGuiControlSlider>) (slider => MyPredictedSnapshotSync.MAX_POSITION_DELTA = slider.Value));
      this.m_currentPosition.Y += 0.01f;
      this.AddCheckBox("Linear velocity corrections", MyPredictedSnapshotSync.LINEAR_VELOCITY_CORRECTION, (Action<MyGuiControlCheckbox>) (x => MyPredictedSnapshotSync.LINEAR_VELOCITY_CORRECTION = x.IsChecked));
      this.AddCheckBox("Smooth linear velocity corrections", MyPredictedSnapshotSync.SMOOTH_LINEAR_VELOCITY_CORRECTION, (Action<MyGuiControlCheckbox>) (x => MyPredictedSnapshotSync.SMOOTH_LINEAR_VELOCITY_CORRECTION = x.IsChecked));
      this.AddSlider("Minimum linVel delta", MyPredictedSnapshotSync.MIN_LINEAR_VELOCITY_DELTA, 0.0f, 0.5f, (Action<MyGuiControlSlider>) (slider => MyPredictedSnapshotSync.MIN_LINEAR_VELOCITY_DELTA = slider.Value));
      this.AddSlider("Maximum linVel delta", MyPredictedSnapshotSync.MAX_LINEAR_VELOCITY_DELTA, 0.0f, 5f, (Action<MyGuiControlSlider>) (slider => MyPredictedSnapshotSync.MAX_LINEAR_VELOCITY_DELTA = slider.Value));
      this.m_currentPosition.Y += 0.01f;
      this.AddCheckBox("Rotation corrections", MyPredictedSnapshotSync.ROTATION_CORRECTION, (Action<MyGuiControlCheckbox>) (x => MyPredictedSnapshotSync.ROTATION_CORRECTION = x.IsChecked));
      this.AddCheckBox("Smooth rotation corrections", MyPredictedSnapshotSync.SMOOTH_ROTATION_CORRECTION, (Action<MyGuiControlCheckbox>) (x => MyPredictedSnapshotSync.SMOOTH_ROTATION_CORRECTION = x.IsChecked));
      this.AddSlider("Minimum angle delta", MathHelper.ToDegrees(MyPredictedSnapshotSync.MIN_ROTATION_ANGLE), 0.0f, 90f, (Action<MyGuiControlSlider>) (slider => MyPredictedSnapshotSync.MIN_ROTATION_ANGLE = MathHelper.ToRadians(slider.Value)));
      this.AddSlider("Maximum angle delta", MathHelper.ToDegrees(MyPredictedSnapshotSync.MAX_ROTATION_ANGLE), 0.0f, 90f, (Action<MyGuiControlSlider>) (slider => MyPredictedSnapshotSync.MAX_ROTATION_ANGLE = MathHelper.ToRadians(slider.Value)));
      this.m_currentPosition.Y += 0.01f;
      this.AddCheckBox("Angular velocity corrections", MyPredictedSnapshotSync.ANGULAR_VELOCITY_CORRECTION, (Action<MyGuiControlCheckbox>) (x => MyPredictedSnapshotSync.ANGULAR_VELOCITY_CORRECTION = x.IsChecked));
      this.AddCheckBox("Smooth angular velocity corrections", MyPredictedSnapshotSync.SMOOTH_ANGULAR_VELOCITY_CORRECTION, (Action<MyGuiControlCheckbox>) (x => MyPredictedSnapshotSync.SMOOTH_ANGULAR_VELOCITY_CORRECTION = x.IsChecked));
      this.AddSlider("Minimum angle delta", MyPredictedSnapshotSync.MIN_ANGULAR_VELOCITY_DELTA, 0.0f, 1f, (Action<MyGuiControlSlider>) (slider => MyPredictedSnapshotSync.MIN_ANGULAR_VELOCITY_DELTA = slider.Value));
      this.AddSlider("Maximum angle delta", MyPredictedSnapshotSync.MAX_ANGULAR_VELOCITY_DELTA, 0.0f, 1f, (Action<MyGuiControlSlider>) (slider => MyPredictedSnapshotSync.MAX_ANGULAR_VELOCITY_DELTA = slider.Value));
      this.AddSlider("Impulse scale", MyGridPhysics.PREDICTION_IMPULSE_SCALE, 0.0f, 0.2f, (Action<MyGuiControlSlider>) (slider => MyGridPhysics.PREDICTION_IMPULSE_SCALE = slider.Value));
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugNetworkPrediction);
  }
}
