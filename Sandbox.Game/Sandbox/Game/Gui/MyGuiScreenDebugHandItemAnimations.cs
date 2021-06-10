// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugHandItemAnimations
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.Gui
{
  [MyDebugScreen("Game", "Hand item animations")]
  internal class MyGuiScreenDebugHandItemAnimations : MyGuiScreenDebugHandItemBase
  {
    private Matrix m_storedWalkingItem;
    private bool m_canUpdateValues = true;
    private float m_itemWalkingRotationX;
    private float m_itemWalkingRotationY;
    private float m_itemWalkingRotationZ;
    private float m_itemWalkingPositionX;
    private float m_itemWalkingPositionY;
    private float m_itemWalkingPositionZ;
    private MyGuiControlSlider m_itemWalkingRotationXSlider;
    private MyGuiControlSlider m_itemWalkingRotationYSlider;
    private MyGuiControlSlider m_itemWalkingRotationZSlider;
    private MyGuiControlSlider m_itemWalkingPositionXSlider;
    private MyGuiControlSlider m_itemWalkingPositionYSlider;
    private MyGuiControlSlider m_itemWalkingPositionZSlider;
    private MyGuiControlSlider m_blendTimeSlider;
    private MyGuiControlSlider m_xAmplitudeOffsetSlider;
    private MyGuiControlSlider m_yAmplitudeOffsetSlider;
    private MyGuiControlSlider m_zAmplitudeOffsetSlider;
    private MyGuiControlSlider m_xAmplitudeScaleSlider;
    private MyGuiControlSlider m_yAmplitudeScaleSlider;
    private MyGuiControlSlider m_zAmplitudeScaleSlider;
    private MyGuiControlSlider m_runMultiplierSlider;
    private MyGuiControlCheckbox m_simulateLeftHandCheckbox;
    private MyGuiControlCheckbox m_simulateRightHandCheckbox;

    public MyGuiScreenDebugHandItemAnimations() => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.AddCaption("Hand item animations", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.RecreateHandItemsCombo();
      this.m_sliderDebugScale = 0.6f;
      this.m_itemWalkingRotationXSlider = this.AddSlider("Walk item rotation X", 0.0f, 0.0f, 360f);
      this.m_itemWalkingRotationXSlider.ValueChanged = new Action<MyGuiControlSlider>(this.WalkingItemChanged);
      this.m_itemWalkingRotationYSlider = this.AddSlider("Walk item rotation Y", 0.0f, 0.0f, 360f);
      this.m_itemWalkingRotationYSlider.ValueChanged = new Action<MyGuiControlSlider>(this.WalkingItemChanged);
      this.m_itemWalkingRotationZSlider = this.AddSlider("Walk item rotation Z", 0.0f, 0.0f, 360f);
      this.m_itemWalkingRotationZSlider.ValueChanged = new Action<MyGuiControlSlider>(this.WalkingItemChanged);
      this.m_itemWalkingPositionXSlider = this.AddSlider("Walk item position X", 0.0f, -1f, 1f);
      this.m_itemWalkingPositionXSlider.ValueChanged = new Action<MyGuiControlSlider>(this.WalkingItemChanged);
      this.m_itemWalkingPositionYSlider = this.AddSlider("Walk item position Y", 0.0f, -1f, 1f);
      this.m_itemWalkingPositionYSlider.ValueChanged = new Action<MyGuiControlSlider>(this.WalkingItemChanged);
      this.m_itemWalkingPositionZSlider = this.AddSlider("Walk item position Z", 0.0f, -1f, 1f);
      this.m_itemWalkingPositionZSlider.ValueChanged = new Action<MyGuiControlSlider>(this.WalkingItemChanged);
      this.m_blendTimeSlider = this.AddSlider("Blend time", 0.0f, 1f / 1000f, 1f);
      this.m_blendTimeSlider.ValueChanged = new Action<MyGuiControlSlider>(this.AmplitudeChanged);
      this.m_xAmplitudeOffsetSlider = this.AddSlider("X offset amplitude", 0.0f, -5f, 5f);
      this.m_xAmplitudeOffsetSlider.ValueChanged = new Action<MyGuiControlSlider>(this.AmplitudeChanged);
      this.m_yAmplitudeOffsetSlider = this.AddSlider("Y offset amplitude", 0.0f, -5f, 5f);
      this.m_yAmplitudeOffsetSlider.ValueChanged = new Action<MyGuiControlSlider>(this.AmplitudeChanged);
      this.m_zAmplitudeOffsetSlider = this.AddSlider("Z offset amplitude", 0.0f, -5f, 5f);
      this.m_zAmplitudeOffsetSlider.ValueChanged = new Action<MyGuiControlSlider>(this.AmplitudeChanged);
      this.m_xAmplitudeScaleSlider = this.AddSlider("X scale amplitude", 0.0f, -5f, 5f);
      this.m_xAmplitudeScaleSlider.ValueChanged = new Action<MyGuiControlSlider>(this.AmplitudeChanged);
      this.m_yAmplitudeScaleSlider = this.AddSlider("Y scale amplitude", 0.0f, -5f, 5f);
      this.m_yAmplitudeScaleSlider.ValueChanged = new Action<MyGuiControlSlider>(this.AmplitudeChanged);
      this.m_zAmplitudeScaleSlider = this.AddSlider("Z scale amplitude", 0.0f, -5f, 5f);
      this.m_zAmplitudeScaleSlider.ValueChanged = new Action<MyGuiControlSlider>(this.AmplitudeChanged);
      this.m_runMultiplierSlider = this.AddSlider("Run multiplier", 0.0f, -5f, 5f);
      this.m_runMultiplierSlider.ValueChanged = new Action<MyGuiControlSlider>(this.AmplitudeChanged);
      this.m_simulateLeftHandCheckbox = this.AddCheckBox("Simulate left hand", false, new Action<MyGuiControlCheckbox>(this.SimulateHandChanged));
      this.m_simulateRightHandCheckbox = this.AddCheckBox("Simulate right hand", false, new Action<MyGuiControlCheckbox>(this.SimulateHandChanged));
      this.AddButton(new StringBuilder("Walk!"), new Action<MyGuiControlButton>(this.OnWalk));
      this.AddButton(new StringBuilder("Run!"), new Action<MyGuiControlButton>(this.OnRun));
      this.RecreateSaveAndReloadButtons();
      this.SelectFirstHandItem();
      this.m_currentPosition.Y += 0.01f;
    }

    public override string GetFriendlyName() => "MyGuiScreenDebugHandItemsAnimations";

    protected override void handItemsCombo_ItemSelected()
    {
      base.handItemsCombo_ItemSelected();
      this.m_storedWalkingItem = this.CurrentSelectedItem.ItemWalkingLocation;
      this.UpdateValues();
    }

    private void OnWalk(MyGuiControlButton button)
    {
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      localCharacter.SwitchAnimation(MyCharacterMovementEnum.Walking);
      localCharacter.SetCurrentMovementState(MyCharacterMovementEnum.Walking);
    }

    private void OnRun(MyGuiControlButton button)
    {
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      localCharacter.SwitchAnimation(MyCharacterMovementEnum.Sprinting);
      localCharacter.SetCurrentMovementState(MyCharacterMovementEnum.Sprinting);
    }

    private void UpdateValues()
    {
      this.m_itemWalkingRotationX = 0.0f;
      this.m_itemWalkingRotationY = 0.0f;
      this.m_itemWalkingRotationZ = 0.0f;
      this.m_itemWalkingPositionX = this.m_storedWalkingItem.Translation.X;
      this.m_itemWalkingPositionY = this.m_storedWalkingItem.Translation.Y;
      this.m_itemWalkingPositionZ = this.m_storedWalkingItem.Translation.Z;
      this.m_canUpdateValues = false;
      this.m_itemWalkingRotationXSlider.Value = this.m_itemWalkingRotationX;
      this.m_itemWalkingRotationYSlider.Value = this.m_itemWalkingRotationY;
      this.m_itemWalkingRotationZSlider.Value = this.m_itemWalkingRotationZ;
      this.m_itemWalkingPositionXSlider.Value = this.m_itemWalkingPositionX;
      this.m_itemWalkingPositionYSlider.Value = this.m_itemWalkingPositionY;
      this.m_itemWalkingPositionZSlider.Value = this.m_itemWalkingPositionZ;
      this.m_blendTimeSlider.Value = this.CurrentSelectedItem.BlendTime;
      this.m_xAmplitudeOffsetSlider.Value = this.CurrentSelectedItem.XAmplitudeOffset;
      this.m_yAmplitudeOffsetSlider.Value = this.CurrentSelectedItem.YAmplitudeOffset;
      this.m_zAmplitudeOffsetSlider.Value = this.CurrentSelectedItem.ZAmplitudeOffset;
      this.m_xAmplitudeScaleSlider.Value = this.CurrentSelectedItem.XAmplitudeScale;
      this.m_yAmplitudeScaleSlider.Value = this.CurrentSelectedItem.YAmplitudeScale;
      this.m_zAmplitudeScaleSlider.Value = this.CurrentSelectedItem.ZAmplitudeScale;
      this.m_runMultiplierSlider.Value = this.CurrentSelectedItem.RunMultiplier;
      this.m_simulateLeftHandCheckbox.IsChecked = this.CurrentSelectedItem.SimulateLeftHand;
      this.m_simulateRightHandCheckbox.IsChecked = this.CurrentSelectedItem.SimulateRightHand;
      this.m_canUpdateValues = true;
    }

    private void WalkingItemChanged(MyGuiControlSlider slider)
    {
      if (!this.m_canUpdateValues)
        return;
      this.m_itemWalkingRotationX = this.m_itemWalkingRotationXSlider.Value;
      this.m_itemWalkingRotationY = this.m_itemWalkingRotationYSlider.Value;
      this.m_itemWalkingRotationZ = this.m_itemWalkingRotationZSlider.Value;
      this.m_itemWalkingPositionX = this.m_itemWalkingPositionXSlider.Value;
      this.m_itemWalkingPositionY = this.m_itemWalkingPositionYSlider.Value;
      this.m_itemWalkingPositionZ = this.m_itemWalkingPositionZSlider.Value;
      this.CurrentSelectedItem.ItemWalkingLocation = this.m_storedWalkingItem * Matrix.CreateRotationX(MathHelper.ToRadians(this.m_itemWalkingRotationX)) * Matrix.CreateRotationY(MathHelper.ToRadians(this.m_itemWalkingRotationY)) * Matrix.CreateRotationZ(MathHelper.ToRadians(this.m_itemWalkingRotationZ));
      this.CurrentSelectedItem.ItemWalkingLocation.Translation = new Vector3(this.m_itemWalkingPositionX, this.m_itemWalkingPositionY, this.m_itemWalkingPositionZ);
    }

    private void AmplitudeChanged(MyGuiControlSlider slider)
    {
      if (!this.m_canUpdateValues)
        return;
      this.CurrentSelectedItem.BlendTime = this.m_blendTimeSlider.Value;
      this.CurrentSelectedItem.XAmplitudeOffset = this.m_xAmplitudeOffsetSlider.Value;
      this.CurrentSelectedItem.YAmplitudeOffset = this.m_yAmplitudeOffsetSlider.Value;
      this.CurrentSelectedItem.ZAmplitudeOffset = this.m_zAmplitudeOffsetSlider.Value;
      this.CurrentSelectedItem.XAmplitudeScale = this.m_xAmplitudeScaleSlider.Value;
      this.CurrentSelectedItem.YAmplitudeScale = this.m_yAmplitudeScaleSlider.Value;
      this.CurrentSelectedItem.ZAmplitudeScale = this.m_zAmplitudeScaleSlider.Value;
      this.CurrentSelectedItem.RunMultiplier = this.m_runMultiplierSlider.Value;
    }

    private void SimulateHandChanged(MyGuiControlCheckbox checkbox)
    {
      if (!this.m_canUpdateValues)
        return;
      this.CurrentSelectedItem.SimulateLeftHand = this.m_simulateLeftHandCheckbox.IsChecked;
      this.CurrentSelectedItem.SimulateRightHand = this.m_simulateRightHandCheckbox.IsChecked;
    }
  }
}
