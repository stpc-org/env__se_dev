// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugHandItemShoot
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
  [MyDebugScreen("Game", "Hand item shoot")]
  internal class MyGuiScreenDebugHandItemShoot : MyGuiScreenDebugHandItemBase
  {
    private Matrix m_storedShootLocation;
    private Matrix m_storedShootLocation3rd;
    private bool m_canUpdateValues = true;
    private float m_itemRotationX;
    private float m_itemRotationY;
    private float m_itemRotationZ;
    private float m_itemPositionX;
    private float m_itemPositionY;
    private float m_itemPositionZ;
    private MyGuiControlSlider m_itemRotationXSlider;
    private MyGuiControlSlider m_itemRotationYSlider;
    private MyGuiControlSlider m_itemRotationZSlider;
    private MyGuiControlSlider m_itemPositionXSlider;
    private MyGuiControlSlider m_itemPositionYSlider;
    private MyGuiControlSlider m_itemPositionZSlider;
    private float m_itemRotationX3rd;
    private float m_itemRotationY3rd;
    private float m_itemRotationZ3rd;
    private float m_itemPositionX3rd;
    private float m_itemPositionY3rd;
    private float m_itemPositionZ3rd;
    private MyGuiControlSlider m_itemRotationX3rdSlider;
    private MyGuiControlSlider m_itemRotationY3rdSlider;
    private MyGuiControlSlider m_itemRotationZ3rdSlider;
    private MyGuiControlSlider m_itemPositionX3rdSlider;
    private MyGuiControlSlider m_itemPositionY3rdSlider;
    private MyGuiControlSlider m_itemPositionZ3rdSlider;
    private MyGuiControlSlider m_itemMuzzlePositionXSlider;
    private MyGuiControlSlider m_itemMuzzlePositionYSlider;
    private MyGuiControlSlider m_itemMuzzlePositionZSlider;
    private MyGuiControlSlider m_blendSlider;
    private MyGuiControlSlider m_shootScatterXSlider;
    private MyGuiControlSlider m_shootScatterYSlider;
    private MyGuiControlSlider m_shootScatterZSlider;
    private MyGuiControlSlider m_scatterSpeedSlider;

    public MyGuiScreenDebugHandItemShoot() => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.AddCaption("Hand item shoot", new Vector4?(Color.Yellow.ToVector4()));
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
      this.m_itemRotationX3rdSlider = this.AddSlider("item rotation X 3rd", 0.0f, 0.0f, 360f);
      this.m_itemRotationX3rdSlider.ValueChanged = new Action<MyGuiControlSlider>(this.ItemChanged);
      this.m_itemRotationY3rdSlider = this.AddSlider("item rotation Y 3rd", 0.0f, 0.0f, 360f);
      this.m_itemRotationY3rdSlider.ValueChanged = new Action<MyGuiControlSlider>(this.ItemChanged);
      this.m_itemRotationZ3rdSlider = this.AddSlider("item rotation Z 3rd", 0.0f, 0.0f, 360f);
      this.m_itemRotationZ3rdSlider.ValueChanged = new Action<MyGuiControlSlider>(this.ItemChanged);
      this.m_itemPositionX3rdSlider = this.AddSlider("item position X 3rd", 0.0f, -1f, 1f);
      this.m_itemPositionX3rdSlider.ValueChanged = new Action<MyGuiControlSlider>(this.ItemChanged);
      this.m_itemPositionY3rdSlider = this.AddSlider("item position Y 3rd", 0.0f, -1f, 1f);
      this.m_itemPositionY3rdSlider.ValueChanged = new Action<MyGuiControlSlider>(this.ItemChanged);
      this.m_itemPositionZ3rdSlider = this.AddSlider("item position Z 3rd", 0.0f, -1f, 1f);
      this.m_itemPositionZ3rdSlider.ValueChanged = new Action<MyGuiControlSlider>(this.ItemChanged);
      this.m_itemMuzzlePositionXSlider = this.AddSlider("item muzzle X", 0.0f, -1f, 1f);
      this.m_itemMuzzlePositionXSlider.ValueChanged = new Action<MyGuiControlSlider>(this.ItemChanged);
      this.m_itemMuzzlePositionYSlider = this.AddSlider("item muzzle Y", 0.0f, -1f, 1f);
      this.m_itemMuzzlePositionYSlider.ValueChanged = new Action<MyGuiControlSlider>(this.ItemChanged);
      this.m_itemMuzzlePositionZSlider = this.AddSlider("item muzzle Z", 0.0f, -1f, 1f);
      this.m_itemMuzzlePositionZSlider.ValueChanged = new Action<MyGuiControlSlider>(this.ItemChanged);
      this.m_blendSlider = this.AddSlider("Shoot blend", 0.0f, 0.0f, 3f);
      this.m_blendSlider.ValueChanged = new Action<MyGuiControlSlider>(this.ItemChanged);
      this.m_shootScatterXSlider = this.AddSlider("Scatter X", 0.0f, 0.0f, 1f);
      this.m_shootScatterXSlider.ValueChanged = new Action<MyGuiControlSlider>(this.ItemChanged);
      this.m_shootScatterYSlider = this.AddSlider("Scatter Y", 0.0f, 0.0f, 1f);
      this.m_shootScatterYSlider.ValueChanged = new Action<MyGuiControlSlider>(this.ItemChanged);
      this.m_shootScatterZSlider = this.AddSlider("Scatter Z", 0.0f, 0.0f, 1f);
      this.m_shootScatterZSlider.ValueChanged = new Action<MyGuiControlSlider>(this.ItemChanged);
      this.m_scatterSpeedSlider = this.AddSlider("Scatter speed", 0.0f, 0.0f, 1f);
      this.m_scatterSpeedSlider.ValueChanged = new Action<MyGuiControlSlider>(this.ItemChanged);
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
      this.m_storedShootLocation = this.CurrentSelectedItem.ItemShootLocation;
      this.m_storedShootLocation3rd = this.CurrentSelectedItem.ItemShootLocation3rd;
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
      this.m_itemRotationX = 0.0f;
      this.m_itemRotationY = 0.0f;
      this.m_itemRotationZ = 0.0f;
      this.m_itemPositionX = this.m_storedShootLocation.Translation.X;
      this.m_itemPositionY = this.m_storedShootLocation.Translation.Y;
      this.m_itemPositionZ = this.m_storedShootLocation.Translation.Z;
      this.m_itemRotationX3rd = 0.0f;
      this.m_itemRotationY3rd = 0.0f;
      this.m_itemRotationZ3rd = 0.0f;
      this.m_itemPositionX3rd = this.m_storedShootLocation3rd.Translation.X;
      this.m_itemPositionY3rd = this.m_storedShootLocation3rd.Translation.Y;
      this.m_itemPositionZ3rd = this.m_storedShootLocation3rd.Translation.Z;
      this.m_canUpdateValues = false;
      this.m_itemRotationXSlider.Value = this.m_itemRotationX;
      this.m_itemRotationYSlider.Value = this.m_itemRotationY;
      this.m_itemRotationZSlider.Value = this.m_itemRotationZ;
      this.m_itemPositionXSlider.Value = this.m_itemPositionX;
      this.m_itemPositionYSlider.Value = this.m_itemPositionY;
      this.m_itemPositionZSlider.Value = this.m_itemPositionZ;
      this.m_itemRotationX3rdSlider.Value = this.m_itemRotationX3rd;
      this.m_itemRotationY3rdSlider.Value = this.m_itemRotationY3rd;
      this.m_itemRotationZ3rdSlider.Value = this.m_itemRotationZ3rd;
      this.m_itemPositionX3rdSlider.Value = this.m_itemPositionX3rd;
      this.m_itemPositionY3rdSlider.Value = this.m_itemPositionY3rd;
      this.m_itemPositionZ3rdSlider.Value = this.m_itemPositionZ3rd;
      this.m_itemMuzzlePositionXSlider.Value = this.CurrentSelectedItem.MuzzlePosition.X;
      this.m_itemMuzzlePositionYSlider.Value = this.CurrentSelectedItem.MuzzlePosition.Y;
      this.m_itemMuzzlePositionZSlider.Value = this.CurrentSelectedItem.MuzzlePosition.Z;
      this.m_shootScatterXSlider.Value = this.CurrentSelectedItem.ShootScatter.X;
      this.m_shootScatterYSlider.Value = this.CurrentSelectedItem.ShootScatter.Y;
      this.m_shootScatterZSlider.Value = this.CurrentSelectedItem.ShootScatter.Z;
      this.m_scatterSpeedSlider.Value = this.CurrentSelectedItem.ScatterSpeed;
      this.m_blendSlider.Value = this.CurrentSelectedItem.ShootBlend;
      this.m_canUpdateValues = true;
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
      this.CurrentSelectedItem.ItemShootLocation = this.m_storedShootLocation * Matrix.CreateRotationX(MathHelper.ToRadians(this.m_itemRotationX)) * Matrix.CreateRotationY(MathHelper.ToRadians(this.m_itemRotationY)) * Matrix.CreateRotationZ(MathHelper.ToRadians(this.m_itemRotationZ));
      this.CurrentSelectedItem.ItemShootLocation.Translation = new Vector3(this.m_itemPositionX, this.m_itemPositionY, this.m_itemPositionZ);
      this.m_itemRotationX3rd = this.m_itemRotationX3rdSlider.Value;
      this.m_itemRotationY3rd = this.m_itemRotationY3rdSlider.Value;
      this.m_itemRotationZ3rd = this.m_itemRotationZ3rdSlider.Value;
      this.m_itemPositionX3rd = this.m_itemPositionX3rdSlider.Value;
      this.m_itemPositionY3rd = this.m_itemPositionY3rdSlider.Value;
      this.m_itemPositionZ3rd = this.m_itemPositionZ3rdSlider.Value;
      this.CurrentSelectedItem.ItemShootLocation3rd = this.m_storedShootLocation3rd * Matrix.CreateRotationX(MathHelper.ToRadians(this.m_itemRotationX3rd)) * Matrix.CreateRotationY(MathHelper.ToRadians(this.m_itemRotationY3rd)) * Matrix.CreateRotationZ(MathHelper.ToRadians(this.m_itemRotationZ3rd));
      this.CurrentSelectedItem.ItemShootLocation3rd.Translation = new Vector3(this.m_itemPositionX3rd, this.m_itemPositionY3rd, this.m_itemPositionZ3rd);
      this.CurrentSelectedItem.ShootBlend = this.m_blendSlider.Value;
      this.CurrentSelectedItem.MuzzlePosition.X = this.m_itemMuzzlePositionXSlider.Value;
      this.CurrentSelectedItem.MuzzlePosition.Y = this.m_itemMuzzlePositionYSlider.Value;
      this.CurrentSelectedItem.MuzzlePosition.Z = this.m_itemMuzzlePositionZSlider.Value;
      this.CurrentSelectedItem.ShootScatter.X = this.m_shootScatterXSlider.Value;
      this.CurrentSelectedItem.ShootScatter.Y = this.m_shootScatterYSlider.Value;
      this.CurrentSelectedItem.ShootScatter.Z = this.m_shootScatterZSlider.Value;
      this.CurrentSelectedItem.ScatterSpeed = this.m_scatterSpeedSlider.Value;
    }
  }
}
