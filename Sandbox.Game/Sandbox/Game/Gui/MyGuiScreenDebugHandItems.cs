// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugHandItems
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics.GUI;
using System;
using VRageMath;

namespace Sandbox.Game.Gui
{
  [MyDebugScreen("Game", "Hand items")]
  internal class MyGuiScreenDebugHandItems : MyGuiScreenDebugHandItemBase
  {
    private Matrix m_storedLeftHand;
    private Matrix m_storedRightHand;
    private Matrix m_storedItem;
    private bool m_canUpdateValues = true;
    private float m_leftHandRotationX;
    private float m_leftHandRotationY;
    private float m_leftHandRotationZ;
    private float m_leftHandPositionX;
    private float m_leftHandPositionY;
    private float m_leftHandPositionZ;
    private float m_rightHandRotationX;
    private float m_rightHandRotationY;
    private float m_rightHandRotationZ;
    private float m_rightHandPositionX;
    private float m_rightHandPositionY;
    private float m_rightHandPositionZ;
    private float m_itemRotationX;
    private float m_itemRotationY;
    private float m_itemRotationZ;
    private float m_itemPositionX;
    private float m_itemPositionY;
    private float m_itemPositionZ;
    private MyGuiControlSlider m_leftHandRotationXSlider;
    private MyGuiControlSlider m_leftHandRotationYSlider;
    private MyGuiControlSlider m_leftHandRotationZSlider;
    private MyGuiControlSlider m_leftHandPositionXSlider;
    private MyGuiControlSlider m_leftHandPositionYSlider;
    private MyGuiControlSlider m_leftHandPositionZSlider;
    private MyGuiControlSlider m_rightHandRotationXSlider;
    private MyGuiControlSlider m_rightHandRotationYSlider;
    private MyGuiControlSlider m_rightHandRotationZSlider;
    private MyGuiControlSlider m_rightHandPositionXSlider;
    private MyGuiControlSlider m_rightHandPositionYSlider;
    private MyGuiControlSlider m_rightHandPositionZSlider;
    private MyGuiControlSlider m_itemRotationXSlider;
    private MyGuiControlSlider m_itemRotationYSlider;
    private MyGuiControlSlider m_itemRotationZSlider;
    private MyGuiControlSlider m_itemPositionXSlider;
    private MyGuiControlSlider m_itemPositionYSlider;
    private MyGuiControlSlider m_itemPositionZSlider;

    public MyGuiScreenDebugHandItems() => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.AddCaption("Hand items properties", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.RecreateHandItemsCombo();
      this.m_sliderDebugScale = 0.6f;
      this.m_leftHandRotationXSlider = this.AddSlider("Left hand rotation X", 0.0f, 0.0f, 360f);
      this.m_leftHandRotationXSlider.ValueChanged = new Action<MyGuiControlSlider>(this.LeftHandChanged);
      this.m_leftHandRotationYSlider = this.AddSlider("Left hand rotation Y", 0.0f, 0.0f, 360f);
      this.m_leftHandRotationYSlider.ValueChanged = new Action<MyGuiControlSlider>(this.LeftHandChanged);
      this.m_leftHandRotationZSlider = this.AddSlider("Left hand rotation Z", 0.0f, 0.0f, 360f);
      this.m_leftHandRotationZSlider.ValueChanged = new Action<MyGuiControlSlider>(this.LeftHandChanged);
      this.m_leftHandPositionXSlider = this.AddSlider("Left hand position X", 0.0f, -1f, 1f);
      this.m_leftHandPositionXSlider.ValueChanged = new Action<MyGuiControlSlider>(this.LeftHandChanged);
      this.m_leftHandPositionYSlider = this.AddSlider("Left hand position Y", 0.0f, -1f, 1f);
      this.m_leftHandPositionYSlider.ValueChanged = new Action<MyGuiControlSlider>(this.LeftHandChanged);
      this.m_leftHandPositionZSlider = this.AddSlider("Left hand position Z", 0.0f, -1f, 1f);
      this.m_leftHandPositionZSlider.ValueChanged = new Action<MyGuiControlSlider>(this.LeftHandChanged);
      this.m_rightHandRotationXSlider = this.AddSlider("Right hand rotation X", 0.0f, 0.0f, 360f);
      this.m_rightHandRotationXSlider.ValueChanged = new Action<MyGuiControlSlider>(this.RightHandChanged);
      this.m_rightHandRotationYSlider = this.AddSlider("Right hand rotation Y", 0.0f, 0.0f, 360f);
      this.m_rightHandRotationYSlider.ValueChanged = new Action<MyGuiControlSlider>(this.RightHandChanged);
      this.m_rightHandRotationZSlider = this.AddSlider("Right hand rotation Z", 0.0f, 0.0f, 360f);
      this.m_rightHandRotationZSlider.ValueChanged = new Action<MyGuiControlSlider>(this.RightHandChanged);
      this.m_rightHandPositionXSlider = this.AddSlider("Right hand position X", 0.0f, -1f, 1f);
      this.m_rightHandPositionXSlider.ValueChanged = new Action<MyGuiControlSlider>(this.RightHandChanged);
      this.m_rightHandPositionYSlider = this.AddSlider("Right hand position Y", 0.0f, -1f, 1f);
      this.m_rightHandPositionYSlider.ValueChanged = new Action<MyGuiControlSlider>(this.RightHandChanged);
      this.m_rightHandPositionZSlider = this.AddSlider("Right hand position Z", 0.0f, -1f, 1f);
      this.m_rightHandPositionZSlider.ValueChanged = new Action<MyGuiControlSlider>(this.RightHandChanged);
      this.m_itemRotationXSlider = this.AddSlider("Item rotation X", 0.0f, 0.0f, 360f);
      this.m_itemRotationXSlider.ValueChanged = new Action<MyGuiControlSlider>(this.ItemChanged);
      this.m_itemRotationYSlider = this.AddSlider("Item rotation Y", 0.0f, 0.0f, 360f);
      this.m_itemRotationYSlider.ValueChanged = new Action<MyGuiControlSlider>(this.ItemChanged);
      this.m_itemRotationZSlider = this.AddSlider("Item rotation Z", 0.0f, 0.0f, 360f);
      this.m_itemRotationZSlider.ValueChanged = new Action<MyGuiControlSlider>(this.ItemChanged);
      this.m_itemPositionXSlider = this.AddSlider("Item position X", 0.0f, -1f, 1f);
      this.m_itemPositionXSlider.ValueChanged = new Action<MyGuiControlSlider>(this.ItemChanged);
      this.m_itemPositionYSlider = this.AddSlider("Item position Y", 0.0f, -1f, 1f);
      this.m_itemPositionYSlider.ValueChanged = new Action<MyGuiControlSlider>(this.ItemChanged);
      this.m_itemPositionZSlider = this.AddSlider("Item position Z", 0.0f, -1f, 1f);
      this.m_itemPositionZSlider.ValueChanged = new Action<MyGuiControlSlider>(this.ItemChanged);
      this.RecreateSaveAndReloadButtons();
      this.SelectFirstHandItem();
      this.m_currentPosition.Y += 0.01f;
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugHandItems);

    protected override void handItemsCombo_ItemSelected()
    {
      base.handItemsCombo_ItemSelected();
      this.m_storedLeftHand = this.CurrentSelectedItem.LeftHand;
      this.m_storedRightHand = this.CurrentSelectedItem.RightHand;
      this.m_storedItem = this.CurrentSelectedItem.ItemLocation;
      this.UpdateValues();
    }

    private void UpdateValues()
    {
      this.m_leftHandRotationX = 0.0f;
      this.m_leftHandRotationY = 0.0f;
      this.m_leftHandRotationZ = 0.0f;
      this.m_leftHandPositionX = this.m_storedLeftHand.Translation.X;
      this.m_leftHandPositionY = this.m_storedLeftHand.Translation.Y;
      this.m_leftHandPositionZ = this.m_storedLeftHand.Translation.Z;
      this.m_rightHandRotationX = 0.0f;
      this.m_rightHandRotationY = 0.0f;
      this.m_rightHandRotationZ = 0.0f;
      this.m_rightHandPositionX = this.m_storedRightHand.Translation.X;
      this.m_rightHandPositionY = this.m_storedRightHand.Translation.Y;
      this.m_rightHandPositionZ = this.m_storedRightHand.Translation.Z;
      this.m_itemRotationX = 0.0f;
      this.m_itemRotationY = 0.0f;
      this.m_itemRotationZ = 0.0f;
      this.m_itemPositionX = this.m_storedItem.Translation.X;
      this.m_itemPositionY = this.m_storedItem.Translation.Y;
      this.m_itemPositionZ = this.m_storedItem.Translation.Z;
      this.m_canUpdateValues = false;
      this.m_leftHandRotationXSlider.Value = this.m_leftHandRotationX;
      this.m_leftHandRotationYSlider.Value = this.m_leftHandRotationY;
      this.m_leftHandRotationZSlider.Value = this.m_leftHandRotationZ;
      this.m_leftHandPositionXSlider.Value = this.m_leftHandPositionX;
      this.m_leftHandPositionYSlider.Value = this.m_leftHandPositionY;
      this.m_leftHandPositionZSlider.Value = this.m_leftHandPositionZ;
      this.m_rightHandRotationXSlider.Value = this.m_rightHandRotationX;
      this.m_rightHandRotationYSlider.Value = this.m_rightHandRotationY;
      this.m_rightHandRotationZSlider.Value = this.m_rightHandRotationZ;
      this.m_rightHandPositionXSlider.Value = this.m_rightHandPositionX;
      this.m_rightHandPositionYSlider.Value = this.m_rightHandPositionY;
      this.m_rightHandPositionZSlider.Value = this.m_rightHandPositionZ;
      this.m_itemRotationXSlider.Value = this.m_itemRotationX;
      this.m_itemRotationYSlider.Value = this.m_itemRotationY;
      this.m_itemRotationZSlider.Value = this.m_itemRotationZ;
      this.m_itemPositionXSlider.Value = this.m_itemPositionX;
      this.m_itemPositionYSlider.Value = this.m_itemPositionY;
      this.m_itemPositionZSlider.Value = this.m_itemPositionZ;
      this.m_canUpdateValues = true;
    }

    private void LeftHandChanged(MyGuiControlSlider slider)
    {
      if (!this.m_canUpdateValues)
        return;
      this.m_leftHandRotationX = this.m_leftHandRotationXSlider.Value;
      this.m_leftHandRotationY = this.m_leftHandRotationYSlider.Value;
      this.m_leftHandRotationZ = this.m_leftHandRotationZSlider.Value;
      this.m_leftHandPositionX = this.m_leftHandPositionXSlider.Value;
      this.m_leftHandPositionY = this.m_leftHandPositionYSlider.Value;
      this.m_leftHandPositionZ = this.m_leftHandPositionZSlider.Value;
      this.CurrentSelectedItem.LeftHand = this.m_storedLeftHand * Matrix.CreateRotationX(MathHelper.ToRadians(this.m_leftHandRotationX)) * Matrix.CreateRotationY(MathHelper.ToRadians(this.m_leftHandRotationY)) * Matrix.CreateRotationZ(MathHelper.ToRadians(this.m_leftHandRotationZ));
      this.CurrentSelectedItem.LeftHand.Translation = new Vector3(this.m_leftHandPositionX, this.m_leftHandPositionY, this.m_leftHandPositionZ);
    }

    private void RightHandChanged(MyGuiControlSlider slider)
    {
      if (!this.m_canUpdateValues)
        return;
      this.m_rightHandRotationX = this.m_rightHandRotationXSlider.Value;
      this.m_rightHandRotationY = this.m_rightHandRotationYSlider.Value;
      this.m_rightHandRotationZ = this.m_rightHandRotationZSlider.Value;
      this.m_rightHandPositionX = this.m_rightHandPositionXSlider.Value;
      this.m_rightHandPositionY = this.m_rightHandPositionYSlider.Value;
      this.m_rightHandPositionZ = this.m_rightHandPositionZSlider.Value;
      this.CurrentSelectedItem.RightHand = this.m_storedRightHand * Matrix.CreateRotationX(MathHelper.ToRadians(this.m_rightHandRotationX)) * Matrix.CreateRotationY(MathHelper.ToRadians(this.m_rightHandRotationY)) * Matrix.CreateRotationZ(MathHelper.ToRadians(this.m_rightHandRotationZ));
      this.CurrentSelectedItem.RightHand.Translation = new Vector3(this.m_rightHandPositionX, this.m_rightHandPositionY, this.m_rightHandPositionZ);
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
      this.CurrentSelectedItem.ItemLocation = this.m_storedItem * Matrix.CreateRotationX(MathHelper.ToRadians(this.m_itemRotationX)) * Matrix.CreateRotationY(MathHelper.ToRadians(this.m_itemRotationY)) * Matrix.CreateRotationZ(MathHelper.ToRadians(this.m_itemRotationZ));
      this.CurrentSelectedItem.ItemLocation.Translation = new Vector3(this.m_itemPositionX, this.m_itemPositionY, this.m_itemPositionZ);
    }
  }
}
