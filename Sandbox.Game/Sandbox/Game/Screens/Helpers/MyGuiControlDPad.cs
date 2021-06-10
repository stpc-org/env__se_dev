// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyGuiControlDPad
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.GameServices;
using VRage.Input;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Screens.Helpers
{
  internal class MyGuiControlDPad : MyGuiControlBase, IDisposable
  {
    private static readonly string DEFAULT_SKIN = "Textures\\GUI\\Icons\\Skins\\Armor\\DefaultArmor.DDS";
    private MyObjectBuilder_DPadControlVisualStyle m_style;
    private MyStringId m_lastContext;
    private MyGuiControlImage m_upImage;
    private MyGuiControlImage m_leftImage;
    private MyGuiControlImage m_rightImage;
    private MyGuiControlImage m_downImage;
    private MyGuiControlImage m_upImageTop;
    private MyGuiControlImage m_leftImageTop;
    private MyGuiControlImage m_rightImageTop;
    private MyGuiControlImage m_downImageTop;
    private MyGuiControlImage m_arrows;
    private MyGuiControlLabel m_bottomHintLeft;
    private MyGuiControlLabel m_bottomHintRight;
    private Vector2 m_subiconOffset;
    private MyGuiControlImageRotatable m_upBackground;
    private MyGuiControlImageRotatable m_upMidground;
    private MyGuiControlImageRotatable m_leftBackground;
    private MyGuiControlImageRotatable m_rightBackground;
    private MyGuiControlImageRotatable m_downBackground;
    private List<MyGuiControlImage> m_images;
    private List<MyGuiControlImageRotatable> m_backgrounds;
    private List<MyGuiControlImageRotatable> m_midgrounds;
    private MyGuiControlDPad.MyDPadVisualLayouts m_visuals;
    private MyGuiControlImageRotatable m_upBackgroundColor;
    private MyGuiControlImageRotatable m_leftBackgroundColor;
    private MyGuiControlImageRotatable m_rightBackgroundColor;
    private MyGuiControlImageRotatable m_downBackgroundColor;
    private MyGuiControlImage m_centerImageInner;
    private MyGuiControlImage m_centerImageOuter;
    private List<MyGuiControlImage> m_imagesColor;
    private List<MyGuiControlImageRotatable> m_backgroundsColor;
    private MyGuiControlLabel m_upLabel;
    private MyGuiControlLabel m_leftLabel;
    private MyGuiControlLabel m_rightLabel;
    private MyGuiControlLabel m_downLabel;
    private Func<string> m_upFunc;
    private Func<string> m_leftFunc;
    private Func<string> m_rightFunc;
    private Func<string> m_downFunc;
    private MyToolSwitcher m_toolSwitcher;
    private MyDefinitionBase m_handWeaponDefinition;
    private bool m_keepHandWeaponAmmoCount;
    private bool m_preloadSkins = true;
    private List<MyGuiControlDPad.MyBlinkAnimator> m_activeAnimators = new List<MyGuiControlDPad.MyBlinkAnimator>(4);
    private static int BLINK_DURATION = 5;
    private bool[] m_canBlink = new bool[4];

    private MyGuiControlDPad.MyDPadVisualLayouts Visuals
    {
      get => this.m_visuals;
      set => this.m_visuals = value;
    }

    public MyGuiControlDPad(MyObjectBuilder_DPadControlVisualStyle style)
      : base()
    {
      this.m_style = style;
      if (this.m_style.VisibleCondition != null)
        this.InitStatConditions(this.m_style.VisibleCondition);
      this.m_images = new List<MyGuiControlImage>();
      this.m_backgrounds = new List<MyGuiControlImageRotatable>();
      this.m_midgrounds = new List<MyGuiControlImageRotatable>();
      this.m_imagesColor = new List<MyGuiControlImage>();
      this.m_backgroundsColor = new List<MyGuiControlImageRotatable>();
      this.m_backgrounds.Add(this.m_upBackground = new MyGuiControlImageRotatable());
      this.m_midgrounds.Add(this.m_upMidground = new MyGuiControlImageRotatable());
      this.m_backgrounds.Add(this.m_leftBackground = new MyGuiControlImageRotatable());
      this.m_backgrounds.Add(this.m_rightBackground = new MyGuiControlImageRotatable());
      this.m_backgrounds.Add(this.m_downBackground = new MyGuiControlImageRotatable());
      Vector2 vector2_1 = new Vector2(200f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      this.m_upBackground.Size = this.m_upMidground.Size = this.m_leftBackground.Size = this.m_rightBackground.Size = this.m_downBackground.Size = vector2_1;
      this.SetBackgroundTexture(ref this.m_upBackground);
      this.SetBackgroundTexture(ref this.m_leftBackground);
      this.SetBackgroundTexture(ref this.m_rightBackground);
      this.SetBackgroundTexture(ref this.m_downBackground);
      this.m_leftBackground.Rotation = -1.570796f;
      this.m_rightBackground.Rotation = 1.570796f;
      this.m_downBackground.Rotation = 3.141593f;
      Vector2 vector2_2 = new Vector2(48f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      this.m_subiconOffset = new Vector2(vector2_2.X * 0.33f, vector2_2.Y * -0.33f);
      this.m_images.Add(this.m_upImage = new MyGuiControlImage(size: new Vector2?(vector2_2)));
      this.m_images.Add(this.m_leftImage = new MyGuiControlImage(size: new Vector2?(vector2_2)));
      this.m_images.Add(this.m_rightImage = new MyGuiControlImage(size: new Vector2?(vector2_2)));
      this.m_images.Add(this.m_downImage = new MyGuiControlImage(size: new Vector2?(vector2_2)));
      this.m_images.Add(this.m_upImageTop = new MyGuiControlImage(size: new Vector2?(vector2_2 / 3f)));
      this.m_images.Add(this.m_leftImageTop = new MyGuiControlImage(size: new Vector2?(vector2_2 / 3f)));
      this.m_images.Add(this.m_rightImageTop = new MyGuiControlImage(size: new Vector2?(vector2_2 / 3f)));
      this.m_images.Add(this.m_downImageTop = new MyGuiControlImage(size: new Vector2?(vector2_2 / 3f)));
      this.m_arrows = new MyGuiControlImage(size: new Vector2?(new Vector2(200f) / MyGuiConstants.GUI_OPTIMAL_SIZE));
      this.m_arrows.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RadialSector_arrows.png");
      this.m_bottomHintLeft = new MyGuiControlLabel(new Vector2?(Vector2.Zero), originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
      this.m_bottomHintRight = new MyGuiControlLabel(new Vector2?(Vector2.Zero), originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
      this.m_backgroundsColor.Add(this.m_upBackgroundColor = new MyGuiControlImageRotatable());
      this.m_backgroundsColor.Add(this.m_downBackgroundColor = new MyGuiControlImageRotatable());
      this.m_backgroundsColor.Add(this.m_leftBackgroundColor = new MyGuiControlImageRotatable());
      this.m_backgroundsColor.Add(this.m_rightBackgroundColor = new MyGuiControlImageRotatable());
      this.m_upBackgroundColor.Size = this.m_leftBackgroundColor.Size = this.m_rightBackgroundColor.Size = this.m_downBackgroundColor.Size = vector2_1;
      this.m_leftBackgroundColor.Rotation = -1.570796f;
      this.m_rightBackgroundColor.Rotation = 1.570796f;
      this.m_downBackgroundColor.Rotation = 3.141593f;
      this.m_imagesColor.Add(this.m_centerImageInner = new MyGuiControlImage(size: new Vector2?(vector2_1)));
      this.m_imagesColor.Add(this.m_centerImageOuter = new MyGuiControlImage(size: new Vector2?(vector2_1)));
      this.m_upBackgroundColor.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\WhiteSquare.png", "Textures\\GUI\\Icons\\HUD 2017\\BCTPeripheralCircle.dds");
      this.m_downBackgroundColor.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\WhiteSquare.png", "Textures\\GUI\\Icons\\HUD 2017\\BCTPeripheralCircle.dds");
      this.m_centerImageOuter.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\WhiteSquare.png", "Textures\\GUI\\Icons\\HUD 2017\\BCTMiddleCircle.dds");
      this.m_activeAnimators.Add(new MyGuiControlDPad.MyBlinkAnimator()
      {
        Control = this.m_upBackground,
        Duration = -1,
        Highlighted = false
      });
      this.m_activeAnimators.Add(new MyGuiControlDPad.MyBlinkAnimator()
      {
        Control = this.m_leftBackground,
        Duration = -1,
        Highlighted = false
      });
      this.m_activeAnimators.Add(new MyGuiControlDPad.MyBlinkAnimator()
      {
        Control = this.m_rightBackground,
        Duration = -1,
        Highlighted = false
      });
      this.m_activeAnimators.Add(new MyGuiControlDPad.MyBlinkAnimator()
      {
        Control = this.m_downBackground,
        Duration = -1,
        Highlighted = false
      });
      float textScale = 0.45f;
      this.m_upLabel = new MyGuiControlLabel(new Vector2?(Vector2.Zero), text: "", textScale: textScale, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
      this.m_leftLabel = new MyGuiControlLabel(new Vector2?(Vector2.Zero), text: "", textScale: textScale, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
      this.m_rightLabel = new MyGuiControlLabel(new Vector2?(Vector2.Zero), text: "", textScale: textScale, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
      this.m_downLabel = new MyGuiControlLabel(new Vector2?(Vector2.Zero), text: "", textScale: textScale, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
      MyCubeBuilder.Static.OnActivated += new Action(this.RefreshIcons);
      MyCubeBuilder.Static.OnDeactivated += new Action(this.RefreshIcons);
      MyCubeBuilder.Static.OnBlockVariantChanged += new Action(this.RefreshIcons);
      MyCubeBuilder.Static.OnSymmetrySetupModeChanged += new Action(this.RefreshIcons);
      MyCockpit.OnPilotAttached += new Action(this.RefreshIcons);
      MySession.Static.OnLocalPlayerSkinOrColorChanged += new Action(this.RefreshIcons);
      MyCubeBuilder.Static.OnToolTypeChanged += new Action(this.RefreshIcons);
      MySessionComponentVoxelHand.Static.OnEnabledChanged += new Action(this.RefreshIcons);
      MySessionComponentVoxelHand.Static.OnBrushChanged += new Action(this.RefreshIcons);
      this.m_toolSwitcher = MySession.Static.GetComponent<MyToolSwitcher>();
      this.m_toolSwitcher.ToolsRefreshed += new Action(this.RefreshIcons);
      MySession.Static.GetComponent<MyEmoteSwitcher>().OnActiveStateChanged += new Action(this.RefreshIcons);
      MySession.Static.GetComponent<MyEmoteSwitcher>().OnPageChanged += new Action(this.RefreshIcons);
      MyGuiControlRadialMenuBlock.OnSelectionConfirmed += new Action<MyGuiControlRadialMenuBlock>(this.RefreshIconsRadialBlock);
      if (MyGuiScreenGamePlay.Static == null)
        return;
      MyGuiScreenGamePlay.Static.OnHelmetChanged += new Action(this.RefreshIcons);
      MyGuiScreenGamePlay.Static.OnHeadlightChanged += new Action(this.RefreshIcons);
    }

    internal void UnregisterEvents()
    {
      MyCockpit.OnPilotAttached -= new Action(this.RefreshIcons);
      MyGuiControlRadialMenuBlock.OnSelectionConfirmed -= new Action<MyGuiControlRadialMenuBlock>(this.RefreshIconsRadialBlock);
    }

    public void RefreshIcons()
    {
      this.CleanUp(true);
      if (MySession.Static.GetComponent<MyEmoteSwitcher>().IsActive)
      {
        this.RefreshEmoteIcons();
      }
      else
      {
        IMyControllableEntity controlledEntity1 = MySession.Static.ControlledEntity;
        MyStringId myStringId = controlledEntity1 != null ? controlledEntity1.AuxiliaryContext : MyStringId.NullOrEmpty;
        if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.FAKE_MODIFIER_LB, MyControlStateType.PRESSED))
        {
          if (myStringId == MySpaceBindingCreator.AX_TOOLS)
            this.RefreshToolShortcuts();
          else if (myStringId == MySpaceBindingCreator.AX_ACTIONS)
            this.RefreshShipToolbarShortcuts();
          else if (myStringId == MySpaceBindingCreator.AX_BUILD)
            this.RefreshBuildingShortcutIcons();
          else if (myStringId == MySpaceBindingCreator.AX_VOXEL)
            this.RefreshVoxelShortcutIcons();
          else if (myStringId == MySpaceBindingCreator.AX_COLOR_PICKER)
            this.RefreshColorShortcutIcons();
          else if (myStringId == MySpaceBindingCreator.AX_CLIPBOARD)
            this.RefreshClipboardShortcutIcons();
          else
            this.RefreshEmptyIcons();
        }
        else if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.FAKE_MODIFIER_RB, MyControlStateType.PRESSED))
        {
          IMyControllableEntity controlledEntity2 = MySession.Static.ControlledEntity;
          if ((controlledEntity2 != null ? controlledEntity2.ControlContext : MyStringId.NullOrEmpty) == MySpaceBindingCreator.CX_SPACESHIP)
            this.RefreshShipShortcutIcons();
          else
            this.RefreshCharacterShortcutIcons();
        }
        else if (myStringId == MySpaceBindingCreator.AX_TOOLS)
          this.RefreshToolIcons();
        else if (myStringId == MySpaceBindingCreator.AX_ACTIONS)
          this.RefreshShipToolbarIcons();
        else if (myStringId == MySpaceBindingCreator.AX_BUILD)
          this.RefreshBuildingIcons();
        else if (myStringId == MySpaceBindingCreator.AX_SYMMETRY)
          this.RefreshSymmetryIcons();
        else if (myStringId == MySpaceBindingCreator.AX_COLOR_PICKER)
          this.RefreshColorIcons();
        else if (myStringId == MySpaceBindingCreator.AX_CLIPBOARD)
        {
          this.RefreshClipboardIcons();
        }
        else
        {
          if (!(myStringId == MySpaceBindingCreator.AX_VOXEL))
            return;
          this.RefreshVoxelIcons();
        }
      }
    }

    private void CleanUp(bool full)
    {
      this.m_upImage.ColorMask = Vector4.One;
      this.m_leftImage.ColorMask = Vector4.One;
      this.m_rightImage.ColorMask = Vector4.One;
      this.m_downImage.ColorMask = Vector4.One;
      this.m_upMidground.SetTexture(string.Empty);
      this.m_upImageTop.SetTexture(string.Empty);
      this.m_leftImageTop.SetTexture(string.Empty);
      this.m_rightImageTop.SetTexture(string.Empty);
      this.m_downImageTop.SetTexture(string.Empty);
      this.m_upImageTop.ColorMask = Vector4.One;
      this.m_leftImageTop.ColorMask = Vector4.One;
      this.m_rightImageTop.ColorMask = Vector4.One;
      this.m_downImageTop.ColorMask = Vector4.One;
      this.SetBackgroundTexture(ref this.m_activeAnimators[0].Control, this.m_activeAnimators[0].Highlighted);
      this.SetBackgroundTexture(ref this.m_activeAnimators[1].Control, this.m_activeAnimators[1].Highlighted);
      this.SetBackgroundTexture(ref this.m_activeAnimators[2].Control, this.m_activeAnimators[2].Highlighted);
      this.SetBackgroundTexture(ref this.m_activeAnimators[3].Control, this.m_activeAnimators[3].Highlighted);
      this.m_bottomHintLeft.Text = string.Empty;
      this.m_bottomHintRight.Text = string.Empty;
      this.Visuals = MyGuiControlDPad.MyDPadVisualLayouts.Classic;
      if (!full)
        return;
      this.m_upLabel.Text = string.Empty;
      this.m_leftLabel.Text = string.Empty;
      this.m_rightLabel.Text = string.Empty;
      this.m_downLabel.Text = string.Empty;
      this.m_upLabel.ColorMask = Vector4.One;
      this.m_leftLabel.ColorMask = Vector4.One;
      this.m_rightLabel.ColorMask = Vector4.One;
      this.m_downLabel.ColorMask = Vector4.One;
      this.m_upFunc = (Func<string>) null;
      this.m_leftFunc = (Func<string>) null;
      this.m_rightFunc = (Func<string>) null;
      this.m_downFunc = (Func<string>) null;
      this.m_handWeaponDefinition = (MyDefinitionBase) null;
      this.m_keepHandWeaponAmmoCount = false;
    }

    private void RefreshShipShortcutIcons()
    {
      Vector4 vector4 = Vector4.One;
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      if (MySession.Static.ControlledEntity != null && !MySession.Static.ControlledEntity.EnabledLights)
        vector4 = new Vector4(0.5f);
      this.m_upImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\SwitchCamera.png");
      this.m_leftImage.SetTexture(string.Empty);
      this.m_rightImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\LightCenter.png");
      this.m_rightImage.ColorMask = vector4;
      this.m_downImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\GridPowerOnCenter.png");
    }

    private void RefreshCharacterShortcutIcons()
    {
      Vector4 vector4_1 = Vector4.One;
      string texture = "Textures\\GUI\\Icons\\HUD 2017\\PlayerHelmetOn.png";
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      if (localCharacter != null && localCharacter.OxygenComponent != null && !localCharacter.OxygenComponent.HelmetEnabled)
      {
        vector4_1 = new Vector4(0.5f);
        texture = "Textures\\GUI\\Icons\\HUD 2017\\PlayerHelmetOff.png";
      }
      Vector4 vector4_2 = Vector4.One;
      if (localCharacter != null && !localCharacter.LightEnabled)
        vector4_2 = new Vector4(0.5f);
      this.m_upImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\SwitchCamera.png");
      this.m_leftImage.SetTexture(texture);
      this.m_leftImage.ColorMask = vector4_1;
      this.m_rightImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\LightCenter.png");
      this.m_rightImage.ColorMask = vector4_2;
      this.m_downImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\ColorPicker.png");
    }

    private void RefreshToolIcons()
    {
      this.m_upImage.SetTextures(this.GetNextToolImage(MyToolSwitcher.ToolType.Drill));
      this.m_rightImage.SetTextures(this.GetNextToolImage(MyToolSwitcher.ToolType.Welder));
      this.m_leftImage.SetTextures(this.GetNextToolImage(MyToolSwitcher.ToolType.Grinder));
      this.m_downImage.SetTextures(this.GetNextToolImage(MyToolSwitcher.ToolType.Weapon));
      this.m_upLabel.Text = string.Empty;
      this.m_leftLabel.Text = string.Empty;
      this.m_rightLabel.Text = string.Empty;
      this.m_upFunc = (Func<string>) null;
      this.m_leftFunc = (Func<string>) null;
      this.m_rightFunc = (Func<string>) null;
      this.m_keepHandWeaponAmmoCount = this.m_handWeaponDefinition != null;
      this.m_handWeaponDefinition = this.GetWeaponDefinition();
      if (this.m_handWeaponDefinition != null)
        this.m_downFunc = new Func<string>(this.GetAmmoCount);
      this.SetBackgroundTexture(ref this.m_upBackground, this.m_toolSwitcher.IsEquipped(MyToolSwitcher.ToolType.Drill));
      this.SetBackgroundTexture(ref this.m_leftBackground, this.m_toolSwitcher.IsEquipped(MyToolSwitcher.ToolType.Grinder));
      this.SetBackgroundTexture(ref this.m_rightBackground, this.m_toolSwitcher.IsEquipped(MyToolSwitcher.ToolType.Welder));
      this.SetBackgroundTexture(ref this.m_downBackground, this.m_toolSwitcher.IsEquipped(MyToolSwitcher.ToolType.Weapon));
    }

    private void SetBackgroundTexture(ref MyGuiControlImageRotatable background, bool isHighlighted = false)
    {
      if (isHighlighted)
        background.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RadialSectorOn.png");
      else
        background.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RadialSector.png");
    }

    private void RefreshIconsRadialBlock(MyGuiControlRadialMenuBlock menu) => this.RefreshIcons();

    private string GetAmmoCount()
    {
      if (this.m_handWeaponDefinition == null)
        return (string) null;
      bool flag = false;
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      if ((localCharacter == null ? 0 : (localCharacter.FindWeaponItemByDefinition(this.m_handWeaponDefinition.Id).HasValue ? 1 : (!localCharacter.WeaponTakesBuilderFromInventory(new MyDefinitionId?(this.m_handWeaponDefinition.Id)) ? 1 : 0))) != 0)
      {
        IMyHandheldGunObject<MyDeviceBase> currentWeapon = localCharacter.CurrentWeapon;
        if (currentWeapon != null)
          flag = MyDefinitionManager.Static.GetPhysicalItemForHandItem(currentWeapon.DefinitionId).Id == this.m_handWeaponDefinition.Id;
        if (localCharacter.LeftHandItem != null)
          flag |= this.m_handWeaponDefinition == localCharacter.LeftHandItem.PhysicalItemDefinition;
        if (flag && currentWeapon != null && (MyDefinitionManager.Static.GetPhysicalItemForHandItem(currentWeapon.DefinitionId) is MyWeaponItemDefinition physicalItemForHandItem && physicalItemForHandItem.ShowAmmoCount))
          return string.Format("{0} • {1}", (object) localCharacter.CurrentWeapon.GetAmmunitionAmount().ToString(), (object) localCharacter.CurrentWeapon.GetMagazineAmount());
      }
      return this.m_keepHandWeaponAmmoCount ? (string) null : "0 • 0";
    }

    private void RefreshBuildingShortcutIcons()
    {
      this.m_upImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\MoveFurther.png");
      this.m_leftImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\ToggleSymmetry.png");
      this.m_rightImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\Autorotate.png");
      this.m_downImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\MoveCloser.png");
    }

    private void RefreshShipToolbarShortcuts()
    {
      this.m_upImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\Contract.png");
      this.m_leftImage.SetTexture(string.Empty);
      this.m_rightImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\ToggleHud.png");
      this.m_downImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\Chat.png");
    }

    private void RefreshToolShortcuts()
    {
      this.m_upImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\Contract.png");
      this.m_leftImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\ProgressionTree.png");
      this.m_rightImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\ToggleHud.png");
      this.m_downImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\Chat.png");
    }

    private void RefreshBuildingIcons()
    {
      MyCubeBlockDefinition currentBlockDefinition = MyCubeBuilder.Static.CurrentBlockDefinition;
      if (currentBlockDefinition != null)
        this.m_upImage.SetTextures(currentBlockDefinition.Icons);
      this.m_leftImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RotateCounterClockwise.png");
      this.m_rightImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RotateClockwise.png");
      this.m_downImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RotationPlane.png");
    }

    private void RefreshSymmetryIcons()
    {
      this.m_upImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\CloseSymmetrySetup.png");
      this.m_leftImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RemoveSymmetryPlane.png");
      this.m_rightImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\PlaceSymmetryPlane.png");
      this.m_downImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\SwitchSymmetryAxis.png");
    }

    private void RefreshEmptyIcons()
    {
      this.m_upImage.SetTexture(string.Empty);
      this.m_leftImage.SetTexture(string.Empty);
      this.m_rightImage.SetTexture(string.Empty);
      this.m_downImage.SetTexture(string.Empty);
    }

    private void RefreshVoxelShortcutIcons()
    {
      this.m_upImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\MoveFurther.png");
      this.m_leftImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RotateClockwise.png");
      this.m_rightImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RotationPlane.png");
      this.m_downImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\MoveCloser.png");
      this.m_leftImage.ColorMask = this.m_rightImage.ColorMask = MySessionComponentVoxelHand.Static.IsBrushRotationEnabled() ? Vector4.One : new Vector4(0.5f);
    }

    private void RefreshVoxelIcons()
    {
      this.m_upImage.SetTexture(string.Empty);
      this.m_leftImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\ScaleDown.png");
      this.m_rightImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\ScaleUp.png");
      this.m_downImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\SetupVoxelHand.png");
      this.m_upMidground.SetTexture(MySessionComponentVoxelHand.Static.CurrentMaterialTextureName, "Textures\\GUI\\Icons\\HUD 2017\\RadialSelectorVoxelCurrent.png");
    }

    private void RefreshColorShortcutIcons()
    {
      this.m_upImage.SetTexture(string.Empty);
      this.m_leftImage.SetTexture(string.Empty);
      this.m_rightImage.SetTexture(string.Empty);
      this.m_downImage.SetTexture(string.Empty);
    }

    private void RefreshColorIcons()
    {
      Vector4 colPrev;
      Vector4 colCur;
      Vector4 colNext;
      this.GetColors(out colPrev, out colCur, out colNext);
      string skinPrev;
      string skinCur;
      string skinNext;
      this.GetSkins(out skinPrev, out skinCur, out skinNext);
      this.m_upBackgroundColor.ColorMask = colNext;
      this.m_centerImageOuter.ColorMask = colCur;
      this.m_downBackgroundColor.ColorMask = colPrev;
      this.m_leftBackgroundColor.SetTexture(skinPrev, "Textures\\GUI\\Icons\\HUD 2017\\BCTPeripheralCircle.dds");
      this.m_rightBackgroundColor.SetTexture(skinNext, "Textures\\GUI\\Icons\\HUD 2017\\BCTPeripheralCircle.dds");
      this.m_centerImageInner.SetTexture(skinCur, "Textures\\GUI\\Icons\\HUD 2017\\BCTCentralCircle.dds");
      this.Visuals = MyGuiControlDPad.MyDPadVisualLayouts.ColorPicker;
    }

    private void RefreshClipboardShortcutIcons()
    {
      this.m_upImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\MoveFurther.png");
      this.m_leftImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\Autorotate.png");
      this.m_rightImage.SetTexture(string.Empty);
      this.m_downImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\MoveCloser.png");
    }

    private void RefreshClipboardIcons()
    {
      this.m_upImage.SetTexture(string.Empty);
      this.m_leftImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RotateCounterClockwise.png");
      this.m_rightImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RotateClockwise.png");
      this.m_downImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RotationPlane.png");
    }

    private void GetSkins(out string skinPrev, out string skinCur, out string skinNext)
    {
      skinPrev = skinCur = skinNext = string.Empty;
      List<MyAssetModifierDefinition> source = new List<MyAssetModifierDefinition>();
      HashSet<string> stringSet = new HashSet<string>();
      foreach (MyGameInventoryItem inventoryItem in (IEnumerable<MyGameInventoryItem>) MyGameService.InventoryItems)
      {
        if (inventoryItem.ItemDefinition.ItemSlot == MyGameInventoryItemSlot.Armor)
        {
          MyAssetModifierDefinition modifierDefinition = MyDefinitionManager.Static.GetAssetModifierDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_AssetModifierDefinition), inventoryItem.ItemDefinition.AssetModifierId));
          if (modifierDefinition != null && !stringSet.Contains(inventoryItem.ItemDefinition.AssetModifierId))
          {
            source.Add(modifierDefinition);
            stringSet.Add(inventoryItem.ItemDefinition.AssetModifierId);
          }
        }
      }
      string buildArmorSkin = MySession.Static.LocalHumanPlayer.BuildArmorSkin;
      int index1 = -1;
      for (int index2 = 0; index2 < source.Count; ++index2)
      {
        if (source[index2].Id.SubtypeName == buildArmorSkin)
        {
          index1 = index2;
          break;
        }
      }
      int num = source.Count + 1;
      int index3 = (index1 + num) % num - 1;
      int index4 = (index1 + 2) % num - 1;
      skinPrev = index3 == -1 ? MyGuiControlDPad.DEFAULT_SKIN : source[index3].Icons[0];
      skinCur = index1 == -1 ? MyGuiControlDPad.DEFAULT_SKIN : source[index1].Icons[0];
      skinNext = index4 == -1 ? MyGuiControlDPad.DEFAULT_SKIN : source[index4].Icons[0];
      if (!this.m_preloadSkins)
        return;
      this.m_preloadSkins = false;
      MyRenderProxy.PreloadTextures(source.SelectMany<MyAssetModifierDefinition, string>((Func<MyAssetModifierDefinition, IEnumerable<string>>) (x => (IEnumerable<string>) x.Icons)), TextureType.GUI);
    }

    private void GetColors(out Vector4 colPrev, out Vector4 colCur, out Vector4 colNext)
    {
      Vector3 prev;
      Vector3 cur;
      Vector3 next;
      MySession.Static.LocalHumanPlayer.GetColorPreviousCurrentNext(out prev, out cur, out next);
      colPrev = (Vector4) MyColorPickerConstants.HSVOffsetToHSV(prev).HSVtoColor();
      colCur = (Vector4) MyColorPickerConstants.HSVOffsetToHSV(cur).HSVtoColor();
      colNext = (Vector4) MyColorPickerConstants.HSVOffsetToHSV(next).HSVtoColor();
    }

    private void RefreshShipToolbarIcons()
    {
      this.m_upImage.SetTextures(MyToolbarComponent.CurrentToolbar.GetItemIconsGamepad(0));
      this.m_leftImage.SetTextures(MyToolbarComponent.CurrentToolbar.GetItemIconsGamepad(1));
      this.m_rightImage.SetTextures(MyToolbarComponent.CurrentToolbar.GetItemIconsGamepad(2));
      this.m_downImage.SetTextures(MyToolbarComponent.CurrentToolbar.GetItemIconsGamepad(3));
      this.m_upLabel.ColorMask = this.m_upImageTop.ColorMask = this.m_upImage.ColorMask = MyToolbarComponent.CurrentToolbar.GetItemIconsColormaskGamepad(0);
      this.m_leftLabel.ColorMask = this.m_leftImageTop.ColorMask = this.m_leftImage.ColorMask = MyToolbarComponent.CurrentToolbar.GetItemIconsColormaskGamepad(1);
      this.m_rightLabel.ColorMask = this.m_rightImageTop.ColorMask = this.m_rightImage.ColorMask = MyToolbarComponent.CurrentToolbar.GetItemIconsColormaskGamepad(2);
      this.m_downLabel.ColorMask = this.m_downImageTop.ColorMask = this.m_downImage.ColorMask = MyToolbarComponent.CurrentToolbar.GetItemIconsColormaskGamepad(3);
      this.m_upImageTop.SetTexture(MyToolbarComponent.CurrentToolbar.GetItemSubiconGamepad(0));
      this.m_leftImageTop.SetTexture(MyToolbarComponent.CurrentToolbar.GetItemSubiconGamepad(1));
      this.m_rightImageTop.SetTexture(MyToolbarComponent.CurrentToolbar.GetItemSubiconGamepad(2));
      this.m_downImageTop.SetTexture(MyToolbarComponent.CurrentToolbar.GetItemSubiconGamepad(3));
      this.m_bottomHintLeft.Text = "\xE009+\xE001";
      this.m_bottomHintRight.Text = "\xE009+\xE003";
      this.Visuals = MyGuiControlDPad.MyDPadVisualLayouts.Classic;
      this.m_upLabel.Text = MyToolbarComponent.CurrentToolbar.GetItemAction(0);
      this.m_leftLabel.Text = MyToolbarComponent.CurrentToolbar.GetItemAction(1);
      this.m_rightLabel.Text = MyToolbarComponent.CurrentToolbar.GetItemAction(2);
      this.m_downLabel.Text = MyToolbarComponent.CurrentToolbar.GetItemAction(3);
      this.m_upFunc = (Func<string>) null;
      this.m_leftFunc = (Func<string>) null;
      this.m_rightFunc = (Func<string>) null;
      this.m_downFunc = (Func<string>) null;
      this.m_handWeaponDefinition = (MyDefinitionBase) null;
      this.m_keepHandWeaponAmmoCount = false;
    }

    private void RefreshEmoteIcons()
    {
      MyEmoteSwitcher component = MySession.Static.GetComponent<MyEmoteSwitcher>();
      if (component == null)
        return;
      this.m_upImage.SetTexture(component.GetIconUp());
      this.m_leftImage.SetTexture(component.GetIconLeft());
      this.m_rightImage.SetTexture(component.GetIconRight());
      this.m_downImage.SetTexture(component.GetIconDown());
      Vector4 iconUpMask = component.GetIconUpMask();
      Vector4 iconLeftMask = component.GetIconLeftMask();
      Vector4 iconRightMask = component.GetIconRightMask();
      Vector4 iconDownMask = component.GetIconDownMask();
      this.m_upImage.ColorMask = iconUpMask;
      this.m_leftImage.ColorMask = iconLeftMask;
      this.m_rightImage.ColorMask = iconRightMask;
      this.m_downImage.ColorMask = iconDownMask;
      this.m_upImageTop.SetTexture(component.GetSubIconUp());
      this.m_leftImageTop.SetTexture(component.GetSubIconLeft());
      this.m_rightImageTop.SetTexture(component.GetSubIconRight());
      this.m_downImageTop.SetTexture(component.GetSubIconDown());
      this.m_upImageTop.ColorMask = iconUpMask;
      this.m_leftImageTop.ColorMask = iconLeftMask;
      this.m_rightImageTop.ColorMask = iconRightMask;
      this.m_downImageTop.ColorMask = iconDownMask;
      this.m_bottomHintLeft.Text = '\xE001'.ToString();
      this.m_bottomHintRight.Text = '\xE003'.ToString();
      this.Visuals = MyGuiControlDPad.MyDPadVisualLayouts.Classic;
      this.m_upLabel.Text = string.Empty;
      this.m_leftLabel.Text = string.Empty;
      this.m_rightLabel.Text = string.Empty;
      this.m_downLabel.Text = string.Empty;
      this.m_upFunc = (Func<string>) null;
      this.m_leftFunc = (Func<string>) null;
      this.m_rightFunc = (Func<string>) null;
      this.m_downFunc = (Func<string>) null;
      this.m_handWeaponDefinition = (MyDefinitionBase) null;
      this.m_keepHandWeaponAmmoCount = false;
    }

    private string[] GetNextToolImage(MyToolSwitcher.ToolType type)
    {
      MyDefinitionId? currentOrNextTool = MySession.Static.GetComponent<MyToolSwitcher>().GetCurrentOrNextTool(type);
      if (!currentOrNextTool.HasValue)
        return (string[]) null;
      return type != MyToolSwitcher.ToolType.Weapon ? MyDefinitionManager.Static.GetPhysicalItemForHandItem(currentOrNextTool.Value).Icons : MyDefinitionManager.Static.GetPhysicalItemDefinition(currentOrNextTool.Value).Icons;
    }

    private MyDefinitionBase GetWeaponDefinition()
    {
      MyDefinitionId? currentOrNextTool = MySession.Static.GetComponent<MyToolSwitcher>().GetCurrentOrNextTool(MyToolSwitcher.ToolType.Weapon);
      return !currentOrNextTool.HasValue ? (MyDefinitionBase) null : (MyDefinitionBase) MyDefinitionManager.Static.GetPhysicalItemDefinition(currentOrNextTool.Value);
    }

    protected override void OnPositionChanged()
    {
      base.OnPositionChanged();
      Vector2 positionAbsoluteCenter = this.GetPositionAbsoluteCenter();
      float num1 = 0.65f;
      this.m_arrows.Position = this.m_upBackground.Position = this.m_upMidground.Position = this.m_leftBackground.Position = this.m_rightBackground.Position = this.m_downBackground.Position = this.m_upBackgroundColor.Position = this.m_leftBackgroundColor.Position = this.m_rightBackgroundColor.Position = this.m_downBackgroundColor.Position = this.m_centerImageInner.Position = this.m_centerImageOuter.Position = positionAbsoluteCenter;
      this.m_upImageTop.Position = (this.m_upImage.Position = positionAbsoluteCenter + new Vector2(0.0f, -65f) / MyGuiConstants.GUI_OPTIMAL_SIZE * num1) + this.m_subiconOffset;
      this.m_leftImageTop.Position = (this.m_leftImage.Position = positionAbsoluteCenter + new Vector2(-65f, 0.0f) / MyGuiConstants.GUI_OPTIMAL_SIZE * num1) + this.m_subiconOffset;
      this.m_rightImageTop.Position = (this.m_rightImage.Position = positionAbsoluteCenter + new Vector2(65f, 0.0f) / MyGuiConstants.GUI_OPTIMAL_SIZE * num1) + this.m_subiconOffset;
      this.m_downImageTop.Position = (this.m_downImage.Position = positionAbsoluteCenter + new Vector2(0.0f, 65f) / MyGuiConstants.GUI_OPTIMAL_SIZE * num1) + this.m_subiconOffset;
      float num2 = 80f;
      this.m_bottomHintLeft.Position = positionAbsoluteCenter + new Vector2(-0.0035f, -1f / 1000f) + new Vector2(-num2, num2) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      this.m_bottomHintRight.Position = positionAbsoluteCenter + new Vector2(-0.0035f, -1f / 1000f) + new Vector2(num2, num2) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      Vector2 vector2 = new Vector2(25f) / MyGuiConstants.GUI_OPTIMAL_SIZE * new Vector2(0.3f, 1.3f);
      this.m_upLabel.Position = this.m_upImageTop.Position + vector2;
      this.m_leftLabel.Position = this.m_leftImageTop.Position + vector2;
      this.m_rightLabel.Position = this.m_rightImageTop.Position + vector2;
      this.m_downLabel.Position = this.m_downImageTop.Position + vector2;
    }

    public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
    {
      if (!this.Visible || this.m_style.VisibleCondition != null && !this.m_style.VisibleCondition.Eval())
        return;
      if (this.m_upFunc != null)
      {
        string str = this.m_upFunc();
        if (!string.IsNullOrEmpty(str))
          this.m_upLabel.Text = str;
      }
      if (this.m_leftFunc != null)
      {
        string str = this.m_leftFunc();
        if (!string.IsNullOrEmpty(str))
          this.m_upLabel.Text = str;
      }
      if (this.m_rightFunc != null)
      {
        string str = this.m_rightFunc();
        if (!string.IsNullOrEmpty(str))
          this.m_rightLabel.Text = str;
      }
      if (this.m_downFunc != null)
      {
        string str = this.m_downFunc();
        if (!string.IsNullOrEmpty(str))
          this.m_downLabel.Text = str;
      }
      switch (this.m_visuals)
      {
        case MyGuiControlDPad.MyDPadVisualLayouts.Classic:
          foreach (MyGuiControlBase background in this.m_backgrounds)
            background.Draw(transitionAlpha * MySandboxGame.Config.HUDBkOpacity, backgroundTransitionAlpha * MySandboxGame.Config.HUDBkOpacity);
          foreach (MyGuiControlBase midground in this.m_midgrounds)
            midground.Draw(transitionAlpha * MySandboxGame.Config.UIOpacity, backgroundTransitionAlpha);
          using (List<MyGuiControlImage>.Enumerator enumerator = this.m_images.GetEnumerator())
          {
            while (enumerator.MoveNext())
              enumerator.Current.Draw(transitionAlpha * MySandboxGame.Config.UIOpacity, backgroundTransitionAlpha);
            break;
          }
        case MyGuiControlDPad.MyDPadVisualLayouts.ColorPicker:
          foreach (MyGuiControlBase myGuiControlBase in this.m_backgroundsColor)
            myGuiControlBase.Draw(transitionAlpha * MySandboxGame.Config.UIOpacity, backgroundTransitionAlpha);
          using (List<MyGuiControlImage>.Enumerator enumerator = this.m_imagesColor.GetEnumerator())
          {
            while (enumerator.MoveNext())
              enumerator.Current.Draw(transitionAlpha * MySandboxGame.Config.UIOpacity, backgroundTransitionAlpha);
            break;
          }
      }
      this.m_arrows.Draw(transitionAlpha * MySandboxGame.Config.UIOpacity, backgroundTransitionAlpha);
      this.m_bottomHintLeft.Draw(transitionAlpha, backgroundTransitionAlpha);
      this.m_bottomHintRight.Draw(transitionAlpha, backgroundTransitionAlpha);
      this.m_upLabel.Draw(transitionAlpha, backgroundTransitionAlpha);
      this.m_leftLabel.Draw(transitionAlpha, backgroundTransitionAlpha);
      this.m_rightLabel.Draw(transitionAlpha, backgroundTransitionAlpha);
      this.m_downLabel.Draw(transitionAlpha, backgroundTransitionAlpha);
      base.Draw(transitionAlpha, backgroundTransitionAlpha);
    }

    private void InitStatConditions(ConditionBase conditionBase)
    {
      foreach (ConditionBase term in (conditionBase as Condition).Terms)
      {
        if (term is StatCondition statCondition)
        {
          IMyHudStat stat = MyHud.Stats.GetStat(statCondition.StatId);
          statCondition.SetStat(stat);
        }
      }
    }

    public override void Update()
    {
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      MyStringId myStringId = controlledEntity != null ? controlledEntity.AuxiliaryContext : MyStringId.NullOrEmpty;
      if (myStringId != this.m_lastContext)
      {
        this.RefreshIcons();
        this.m_lastContext = myStringId;
      }
      else if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.FAKE_MODIFIER_LB) || MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.FAKE_MODIFIER_LB, MyControlStateType.NEW_RELEASED) || (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.FAKE_MODIFIER_RB) || MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.FAKE_MODIFIER_RB, MyControlStateType.NEW_RELEASED)))
        this.RefreshIcons();
      base.Update();
      foreach (MyGuiControlDPad.MyBlinkAnimator activeAnimator in this.m_activeAnimators)
      {
        if (activeAnimator.Duration >= 0)
          --activeAnimator.Duration;
        if (activeAnimator.Duration == 0)
        {
          activeAnimator.Highlighted = !activeAnimator.Highlighted;
          this.SetBackgroundTexture(ref activeAnimator.Control, activeAnimator.Highlighted);
        }
      }
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_BASE, MyControlsSpace.FAKE_UP))
        this.Blink(0);
      else if (MyControllerHelper.IsControl(MyControllerHelper.CX_BASE, MyControlsSpace.FAKE_DOWN))
        this.Blink(3);
      else if (MyControllerHelper.IsControl(MyControllerHelper.CX_BASE, MyControlsSpace.FAKE_LEFT))
      {
        this.Blink(1);
      }
      else
      {
        if (!MyControllerHelper.IsControl(MyControllerHelper.CX_BASE, MyControlsSpace.FAKE_RIGHT))
          return;
        this.Blink(2);
      }
    }

    private void Blink(int idx)
    {
      if (idx < 0 || idx > 3 || this.m_canBlink[idx])
        return;
      this.m_activeAnimators[idx].Duration = MyGuiControlDPad.BLINK_DURATION;
      this.m_activeAnimators[idx].Highlighted = true;
      this.SetBackgroundTexture(ref this.m_activeAnimators[idx].Control, true);
    }

    public void Dispose()
    {
      if (MyCubeBuilder.Static != null)
      {
        MyCubeBuilder.Static.OnActivated -= new Action(this.RefreshIcons);
        MyCubeBuilder.Static.OnDeactivated -= new Action(this.RefreshIcons);
        MyCubeBuilder.Static.OnBlockVariantChanged -= new Action(this.RefreshIcons);
        MyCubeBuilder.Static.OnSymmetrySetupModeChanged -= new Action(this.RefreshIcons);
        MyCubeBuilder.Static.OnToolTypeChanged -= new Action(this.RefreshIcons);
      }
      if (MySession.Static != null)
      {
        MySession.Static.OnLocalPlayerSkinOrColorChanged -= new Action(this.RefreshIcons);
        MySession.Static.GetComponent<MyEmoteSwitcher>().OnActiveStateChanged -= new Action(this.RefreshIcons);
        MySession.Static.GetComponent<MyEmoteSwitcher>().OnPageChanged -= new Action(this.RefreshIcons);
      }
      if (MySessionComponentVoxelHand.Static != null)
      {
        MySessionComponentVoxelHand.Static.OnEnabledChanged -= new Action(this.RefreshIcons);
        MySessionComponentVoxelHand.Static.OnBrushChanged -= new Action(this.RefreshIcons);
      }
      if (this.m_toolSwitcher != null)
        this.m_toolSwitcher.ToolsRefreshed -= new Action(this.RefreshIcons);
      if (MyGuiScreenGamePlay.Static != null)
      {
        MyGuiScreenGamePlay.Static.OnHelmetChanged -= new Action(this.RefreshIcons);
        MyGuiScreenGamePlay.Static.OnHeadlightChanged -= new Action(this.RefreshIcons);
      }
      MyCockpit.OnPilotAttached -= new Action(this.RefreshIcons);
      MyGuiControlRadialMenuBlock.OnSelectionConfirmed -= new Action<MyGuiControlRadialMenuBlock>(this.RefreshIconsRadialBlock);
    }

    private enum MyDPadVisualLayouts
    {
      Classic,
      ColorPicker,
    }

    private class MyBlinkAnimator
    {
      public MyGuiControlImageRotatable Control;
      public int Duration = -1;
      public bool Highlighted;
    }
  }
}
