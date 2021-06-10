// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MyEntityRemoteController
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Screens.Helpers;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.GameServices;
using VRage.Input;
using VRageMath;

namespace Sandbox.Game.World
{
  public class MyEntityRemoteController
  {
    private static readonly Random m_random = new Random();
    private readonly string[] m_animations = new string[4]
    {
      "Wave",
      "Thumb-Up",
      "FacePalm",
      "Victory"
    };
    private readonly float m_animationTimer = 20f;
    private readonly float m_doubleClickPause = 0.2f;
    private MyEntity m_controlledEntity;
    private float m_currentAnimationTime;
    private bool m_canPlayAnimation;
    private int m_buttonClicks;
    private float m_lastClickTime;
    private float m_currentTime;
    private float m_rotationSpeed;
    private float m_rotationSpeedDecay = 0.95f;
    private Vector3 m_rotationDirection = Vector3.Zero;
    private GlobalAxis m_rotationLocks;
    private Vector3 m_rotationVector = Vector3.One;
    private Dictionary<string, MyGameInventoryItemSlot> m_toolsNames;

    public GlobalAxis RotationLocks
    {
      get => this.m_rotationLocks;
      private set
      {
        this.m_rotationLocks = value;
        this.m_rotationVector = ((this.m_rotationLocks & GlobalAxis.X) == GlobalAxis.None ? Vector3.Right : Vector3.Zero) + ((this.m_rotationLocks & GlobalAxis.Y) == GlobalAxis.None ? Vector3.Up : Vector3.Zero) + ((this.m_rotationLocks & GlobalAxis.Z) == GlobalAxis.None ? Vector3.Backward : Vector3.Zero);
      }
    }

    public MyEntityRemoteController(MyEntity entity)
    {
      this.m_controlledEntity = entity;
      this.m_rotationLocks = GlobalAxis.None;
      this.m_rotationVector = Vector3.One;
      this.m_toolsNames = new Dictionary<string, MyGameInventoryItemSlot>();
      this.m_toolsNames.Add("AutomaticRifleItem", MyGameInventoryItemSlot.Rifle);
      this.m_toolsNames.Add("RapidFireAutomaticRifleItem", MyGameInventoryItemSlot.Rifle);
      this.m_toolsNames.Add("PreciseAutomaticRifleItem", MyGameInventoryItemSlot.Rifle);
      this.m_toolsNames.Add("UltimateAutomaticRifleItem", MyGameInventoryItemSlot.Rifle);
      this.m_toolsNames.Add("WelderItem", MyGameInventoryItemSlot.Welder);
      this.m_toolsNames.Add("Welder2Item", MyGameInventoryItemSlot.Welder);
      this.m_toolsNames.Add("Welder3Item", MyGameInventoryItemSlot.Welder);
      this.m_toolsNames.Add("Welder4Item", MyGameInventoryItemSlot.Welder);
      this.m_toolsNames.Add("AngleGrinderItem", MyGameInventoryItemSlot.Grinder);
      this.m_toolsNames.Add("AngleGrinder2Item", MyGameInventoryItemSlot.Grinder);
      this.m_toolsNames.Add("AngleGrinder3Item", MyGameInventoryItemSlot.Grinder);
      this.m_toolsNames.Add("AngleGrinder4Item", MyGameInventoryItemSlot.Grinder);
      this.m_toolsNames.Add("HandDrillItem", MyGameInventoryItemSlot.Drill);
      this.m_toolsNames.Add("HandDrill2Item", MyGameInventoryItemSlot.Drill);
      this.m_toolsNames.Add("HandDrill3Item", MyGameInventoryItemSlot.Drill);
      this.m_toolsNames.Add("HandDrill4Item", MyGameInventoryItemSlot.Drill);
    }

    public MyEntity GetEntity() => this.m_controlledEntity;

    public void Update(bool isMouseOverAnyControl)
    {
      this.m_currentTime += 0.01666667f;
      this.m_currentAnimationTime += 0.01666667f;
      if ((double) this.m_currentAnimationTime > (double) this.m_animationTimer)
      {
        this.m_currentAnimationTime = 0.0f;
        this.m_canPlayAnimation = true;
      }
      if (MyInput.Static.IsMousePressed(MyMouseButtonsEnum.Left) && !isMouseOverAnyControl)
        this.SetRotationWithSpeed(Vector3.One, MyInput.Static.GetCursorPositionDelta().X * 50f);
      float num1 = MyControllerHelper.IsControlAnalog(MyControllerHelper.CX_GUI, MyControlsGUI.SCROLL_LEFT);
      float num2 = MyControllerHelper.IsControlAnalog(MyControllerHelper.CX_GUI, MyControlsGUI.SCROLL_RIGHT) - num1;
      if ((double) num2 != 0.0)
        this.SetRotationWithSpeed(Vector3.One, num2 * 200f);
      if (MyInput.Static.IsMousePressed(MyMouseButtonsEnum.Right) && !isMouseOverAnyControl)
        this.PlayRandomCharacterAnimation();
      if (MyInput.Static.IsNewMousePressed(MyMouseButtonsEnum.Left) && !isMouseOverAnyControl)
      {
        if ((double) this.m_lastClickTime + (double) this.m_doubleClickPause > (double) this.m_currentTime)
          ++this.m_buttonClicks;
        this.m_lastClickTime = this.m_currentTime;
      }
      if ((double) this.m_currentTime > (double) this.m_lastClickTime + (double) this.m_doubleClickPause)
      {
        this.m_buttonClicks = 0;
        this.m_lastClickTime = this.m_currentTime;
      }
      if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.HELMET) || this.m_buttonClicks == 2)
      {
        this.ToggleCharacterHelmet();
        this.m_buttonClicks = 0;
      }
      if (MyInput.Static.IsNewKeyPressed(MyKeys.D1))
        this.PlayCharacterAnimation(this.m_animations[0]);
      if (MyInput.Static.IsNewKeyPressed(MyKeys.D2))
        this.PlayCharacterAnimation(this.m_animations[1]);
      if (MyInput.Static.IsNewKeyPressed(MyKeys.D3))
        this.PlayCharacterAnimation(this.m_animations[2]);
      if (MyInput.Static.IsNewKeyPressed(MyKeys.D4))
        this.PlayCharacterAnimation(this.m_animations[3]);
      if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.CROUCH))
        MySession.Static.LocalCharacter.Crouch();
      if ((double) this.m_rotationSpeed == 0.0)
        return;
      if (this.m_rotationDirection != Vector3.Zero)
        this.RotateEntity(this.m_rotationDirection * this.m_rotationSpeed * 0.01666667f);
      this.m_rotationSpeed *= this.m_rotationSpeedDecay;
      if ((double) Math.Abs(this.m_rotationSpeed) >= 1.0 / 1000.0)
        return;
      this.m_rotationSpeed = 0.0f;
    }

    public void LockRotationAxis(GlobalAxis axis) => this.RotationLocks |= axis;

    public void UnlockRotationAxis(GlobalAxis axis) => this.RotationLocks &= ~axis;

    public void SetRotationWithSpeed(Vector3 rotation, float speed)
    {
      this.m_rotationDirection = rotation;
      this.m_rotationSpeed = speed;
    }

    public void RotateEntity(Vector3 rotation)
    {
      if (this.m_controlledEntity is MyCharacter controlledEntity)
      {
        controlledEntity.MoveAndRotate(Vector3.Zero, new Vector2(0.0f, rotation.Y) * -3f, 0.0f);
      }
      else
      {
        if (this.m_controlledEntity == null || !this.m_controlledEntity.InScene)
          return;
        rotation = rotation * 3.141593f / 180f;
        MatrixD result1;
        MatrixD.CreateFromYawPitchRoll((double) rotation.X * (double) this.m_rotationVector.X, (double) rotation.Y * (double) this.m_rotationVector.Y, (double) rotation.Z * (double) this.m_rotationVector.Z, out result1);
        result1.Translation = Vector3D.Zero;
        MatrixD worldMatrix = this.m_controlledEntity.WorldMatrix;
        MatrixD result2;
        MatrixD.Multiply(ref result1, ref worldMatrix, out result2);
        this.m_controlledEntity.WorldMatrix = result2;
      }
    }

    public List<MyPhysicalInventoryItem> GetInventoryTools()
    {
      List<MyPhysicalInventoryItem> physicalInventoryItemList = new List<MyPhysicalInventoryItem>();
      if (this.m_controlledEntity is MyCharacter controlledEntity)
      {
        foreach (MyPhysicalInventoryItem physicalInventoryItem in controlledEntity.GetInventoryBase().GetItems())
        {
          if (this.m_toolsNames.ContainsKey(physicalInventoryItem.Content.SubtypeName))
            physicalInventoryItemList.Add(physicalInventoryItem);
        }
      }
      return physicalInventoryItemList;
    }

    public MyGameInventoryItemSlot GetToolSlot(string name) => this.m_toolsNames.ContainsKey(name) ? this.m_toolsNames[name] : MyGameInventoryItemSlot.None;

    public void ToggleCharacterHelmet()
    {
      if (!(this.m_controlledEntity is IMyControllableEntity controlledEntity))
        return;
      controlledEntity.SwitchHelmet();
    }

    public void PlayCharacterAnimation(string animationName)
    {
      if (!(this.m_controlledEntity is MyCharacter controlledEntity) || MyDefinitionManager.Static.TryGetAnimationDefinition(animationName) == null || !controlledEntity.UseNewAnimationSystem)
        return;
      controlledEntity.TriggerCharacterAnimationEvent(animationName.ToLower(), true);
    }

    public void PlayRandomCharacterAnimation()
    {
      if (!this.m_canPlayAnimation)
        return;
      this.PlayCharacterAnimation(this.m_animations[MyEntityRemoteController.m_random.Next(0, this.m_animations.Length)]);
      this.m_canPlayAnimation = false;
    }

    public void ActivateCharacterToolbarItem(MyDefinitionId item)
    {
      if (!(this.m_controlledEntity is MyCharacter controlledEntity))
        return;
      MyToolbar toolbar = controlledEntity.Toolbar;
      if (toolbar == null)
        return;
      if (item.TypeId.IsNull)
      {
        toolbar.Unselect(false);
      }
      else
      {
        MyDefinitionBase definition;
        if (!MyDefinitionManager.Static.TryGetDefinition<MyDefinitionBase>(item, out definition) || !(MyToolbarItemFactory.CreateToolbarItem(MyToolbarItemFactory.ObjectBuilderFromDefinition(definition)) is MyToolbarItemWeapon toolbarItem))
          return;
        controlledEntity.SwitchToWeapon(toolbarItem);
      }
    }

    public void ToggleCharacterBackpack()
    {
      if (!(this.m_controlledEntity is MyCharacter controlledEntity))
        return;
      controlledEntity.EnableBag(!controlledEntity.EnabledBag);
    }

    public void ChangeCharacterColor(Color color) => this.ChangeCharacterColor(color.ColorToHSVDX11());

    public void ChangeCharacterColor(Vector3 hsvColor)
    {
      if (!(this.m_controlledEntity is MyCharacter controlledEntity))
        return;
      controlledEntity.ChangeModelAndColor(controlledEntity.ModelName, hsvColor);
      MyLocalCache.SaveInventoryConfig(controlledEntity);
    }
  }
}
