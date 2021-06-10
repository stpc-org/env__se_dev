// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugHandItemAnimations3rd
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
  [MyDebugScreen("Game", "Hand item animations 3rd")]
  internal class MyGuiScreenDebugHandItemAnimations3rd : MyGuiScreenDebugHandItemBase
  {
    private Matrix m_storedItem;
    private Matrix m_storedWalkingItem;
    private bool m_canUpdateValues = true;
    private float m_itemRotationX;
    private float m_itemRotationY;
    private float m_itemRotationZ;
    private float m_itemPositionX;
    private float m_itemPositionY;
    private float m_itemPositionZ;
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
    private MyGuiControlSlider m_itemRotationXSlider;
    private MyGuiControlSlider m_itemRotationYSlider;
    private MyGuiControlSlider m_itemRotationZSlider;
    private MyGuiControlSlider m_itemPositionXSlider;
    private MyGuiControlSlider m_itemPositionYSlider;
    private MyGuiControlSlider m_itemPositionZSlider;
    private MyGuiControlSlider m_amplitudeMultiplierSlider;

    public MyGuiScreenDebugHandItemAnimations3rd() => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.AddCaption("Hand item animations 3rd", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.RecreateHandItemsCombo();
      this.m_sliderDebugScale = 0.6f;
      this.m_itemRotationXSlider = this.AddSlider("item rotation X", 0.0f, 0.0f, 360f);
      this.m_itemRotationXSlider.ValueChanged = new Action<MyGuiControlSlider>(this.ItemChanged);
      this.m_itemRotationYSlider = this.AddSlider("item rotation Y", 0.0f, 0.0f, 360f);
      this.m_itemRotationYSlider.ValueChanged = new Action<MyGuiControlSlider>(this.ItemChanged);
      this.m_itemRotationZSlider = this.AddSlider("item rotation Z", 0.0f, 0.0f, 360f);
      this.m_itemRotationZSlider.ValueChanged = new Action<MyGuiControlSlider>(this.ItemChanged);
      this.m_itemPositionXSlider = this.AddSlider("item position X", 0.0f, -1f, 1f);
      this.m_itemPositionXSlider.ValueChanged = new Action<MyGuiControlSlider>(this.ItemChanged);
      this.m_itemPositionYSlider = this.AddSlider("item position Y", 0.0f, -1f, 1f);
      this.m_itemPositionYSlider.ValueChanged = new Action<MyGuiControlSlider>(this.ItemChanged);
      this.m_itemPositionZSlider = this.AddSlider("item position Z", 0.0f, -1f, 1f);
      this.m_itemPositionZSlider.ValueChanged = new Action<MyGuiControlSlider>(this.ItemChanged);
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
      this.m_amplitudeMultiplierSlider = this.AddSlider("Amplitude multiplier", 0.0f, -1f, 3f);
      this.m_amplitudeMultiplierSlider.ValueChanged = new Action<MyGuiControlSlider>(this.WalkingItemChanged);
      this.AddButton(new StringBuilder("Walk!"), new Action<MyGuiControlButton>(this.OnWalk));
      this.AddButton(new StringBuilder("Run!"), new Action<MyGuiControlButton>(this.OnRun));
      this.RecreateSaveAndReloadButtons();
      this.SelectFirstHandItem();
      this.m_currentPosition.Y += 0.01f;
    }

    public override string GetFriendlyName() => "MyGuiScreenDebugHandItemsAnimations3rd";

    protected override void handItemsCombo_ItemSelected()
    {
      base.handItemsCombo_ItemSelected();
      this.m_storedWalkingItem = this.CurrentSelectedItem.ItemWalkingLocation3rd;
      this.m_storedItem = this.CurrentSelectedItem.ItemLocation3rd;
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
      this.m_itemRotationX = 0.0f;
      this.m_itemRotationY = 0.0f;
      this.m_itemRotationZ = 0.0f;
      this.m_itemPositionX = this.m_storedItem.Translation.X;
      this.m_itemPositionY = this.m_storedItem.Translation.Y;
      this.m_itemPositionZ = this.m_storedItem.Translation.Z;
      this.m_canUpdateValues = false;
      this.m_itemWalkingRotationXSlider.Value = this.m_itemWalkingRotationX;
      this.m_itemWalkingRotationYSlider.Value = this.m_itemWalkingRotationY;
      this.m_itemWalkingRotationZSlider.Value = this.m_itemWalkingRotationZ;
      this.m_itemWalkingPositionXSlider.Value = this.m_itemWalkingPositionX;
      this.m_itemWalkingPositionYSlider.Value = this.m_itemWalkingPositionY;
      this.m_itemWalkingPositionZSlider.Value = this.m_itemWalkingPositionZ;
      this.m_itemRotationXSlider.Value = this.m_itemRotationX;
      this.m_itemRotationYSlider.Value = this.m_itemRotationY;
      this.m_itemRotationZSlider.Value = this.m_itemRotationZ;
      this.m_itemPositionXSlider.Value = this.m_itemPositionX;
      this.m_itemPositionYSlider.Value = this.m_itemPositionY;
      this.m_itemPositionZSlider.Value = this.m_itemPositionZ;
      this.m_amplitudeMultiplierSlider.Value = this.CurrentSelectedItem.AmplitudeMultiplier3rd;
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
      this.CurrentSelectedItem.ItemWalkingLocation3rd = this.m_storedWalkingItem * Matrix.CreateRotationX(MathHelper.ToRadians(this.m_itemWalkingRotationX)) * Matrix.CreateRotationY(MathHelper.ToRadians(this.m_itemWalkingRotationY)) * Matrix.CreateRotationZ(MathHelper.ToRadians(this.m_itemWalkingRotationZ));
      this.CurrentSelectedItem.ItemWalkingLocation3rd.Translation = new Vector3(this.m_itemWalkingPositionX, this.m_itemWalkingPositionY, this.m_itemWalkingPositionZ);
      this.CurrentSelectedItem.AmplitudeMultiplier3rd = this.m_amplitudeMultiplierSlider.Value;
    }

    private void ItemChanged(MyGuiControlSlider slider)
    {
      if (!this.m_canUpdateValues)
        return;
      this.m_itemRotationX = this.m_itemRotationXSlider.Value;
      this.m_itemRotationY = this.m_itemRotationYSlider.Value;
      this.m_itemRotationZ = this.m_itemRotationZSlider.Value;
      this.m_itemPositionX = this.m_itemPositionXSlider.Value;
      this.m_itemPositionY = this.m_itemPositionYSlider.Value;
      this.m_itemPositionZ = this.m_itemPositionZSlider.Value;
      this.CurrentSelectedItem.ItemLocation3rd = this.m_storedItem * Matrix.CreateRotationX(MathHelper.ToRadians(this.m_itemRotationX)) * Matrix.CreateRotationY(MathHelper.ToRadians(this.m_itemRotationY)) * Matrix.CreateRotationZ(MathHelper.ToRadians(this.m_itemRotationZ));
      this.CurrentSelectedItem.ItemLocation3rd.Translation = new Vector3(this.m_itemPositionX, this.m_itemPositionY, this.m_itemPositionZ);
    }
  }
}
