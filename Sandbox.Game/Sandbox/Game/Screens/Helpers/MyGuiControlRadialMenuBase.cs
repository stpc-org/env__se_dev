// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyGuiControlRadialMenuBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Screens.Helpers
{
  public abstract class MyGuiControlRadialMenuBase : MyGuiScreenBase
  {
    protected static readonly TimeSpan MOVEMENT_SUPPRESSION = TimeSpan.FromSeconds(2.0);
    protected static readonly float RADIUS = 0.11f;
    protected static readonly Dictionary<MyDefinitionId, int> m_lastSelectedSection = new Dictionary<MyDefinitionId, int>();
    private const int ITEMS_COUNT = 8;
    protected List<MyGuiControlImageRotatable> m_buttons;
    private List<MyGuiControlImage> m_tabs;
    private List<MyGuiControlLabel> m_tabLabels;
    protected List<MyGuiControlImage> m_icons;
    protected MyRadialMenu m_data;
    protected MyGuiControlLabel m_tooltipName;
    protected MyGuiControlLabel m_tooltipState;
    protected MyGuiControlLabel m_tooltipShortcut;
    protected MyGuiControlImage m_cancelButton;
    protected int m_selectedButton;
    private MyGuiControlLabel m_leftButtonHint;
    private MyGuiControlLabel m_rightButtonHint;
    private readonly Func<bool> m_handleInputCallback;
    private readonly MyStringId m_closingControl;
    protected int m_currentSection;
    private Vector2 m_tabSize;
    private Vector2 m_tabSizeSmall;
    protected const float HINTS_POS_Y = 0.365f;
    private float CATEGORY_POS_Y;
    private float m_hintYPos;
    private float m_hintIconYPosOffset;
    private MyGuiControlImageRotatable m_buttonHighlight;

    public int CurrentTabIndex => this.m_currentSection;

    protected MyGuiControlRadialMenuBase(
      MyRadialMenu data,
      MyStringId closingControl,
      Func<bool> handleInputCallback)
    {
      Vector2? position1 = new Vector2?();
      Vector4? backgroundColor1 = new Vector4?();
      Vector4? backgroundColor2 = backgroundColor1;
      Vector2? size1 = new Vector2?();
      double uiBkOpacity = (double) MySandboxGame.Config.UIBkOpacity;
      double uiOpacity = (double) MySandboxGame.Config.UIOpacity;
      int? gamepadSlot = new int?();
      // ISSUE: explicit constructor call
      base.\u002Ector(position1, backgroundColor2, size1, backgroundTransition: ((float) uiBkOpacity), guiTransition: ((float) uiOpacity), gamepadSlot: gamepadSlot);
      this.m_isTopMostScreen = true;
      this.DrawMouseCursor = false;
      this.m_closeOnEsc = true;
      this.m_closingControl = closingControl;
      this.m_handleInputCallback = handleInputCallback;
      MyCharacter.OnCharacterDied += new Action<MyCharacter>(this.MyCharacter_OnCharacterDied);
      this.m_buttons = new List<MyGuiControlImageRotatable>();
      this.m_tabs = new List<MyGuiControlImage>();
      this.m_tabLabels = new List<MyGuiControlLabel>();
      this.m_icons = new List<MyGuiControlImage>();
      this.m_data = data;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      myGuiControlLabel1.Font = "Blue";
      myGuiControlLabel1.UseTextShadow = true;
      myGuiControlLabel1.Visible = false;
      this.m_tooltipName = myGuiControlLabel1;
      this.m_tooltipName.TextScale *= 1.2f;
      this.m_tooltipName.RecalculateSize();
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel();
      myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      myGuiControlLabel2.Font = "Blue";
      myGuiControlLabel2.UseTextShadow = true;
      myGuiControlLabel2.Visible = false;
      this.m_tooltipState = myGuiControlLabel2;
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel();
      myGuiControlLabel3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      myGuiControlLabel3.Font = "Blue";
      myGuiControlLabel3.UseTextShadow = true;
      myGuiControlLabel3.Visible = false;
      this.m_tooltipShortcut = myGuiControlLabel3;
      this.m_tooltipShortcut.TextScale = 1f;
      float num1 = 0.005f;
      backgroundColor1 = new Vector4?(new Vector4(1f, 1f, 1f, 0.8f));
      MyGuiControlImage myGuiControlImage1 = new MyGuiControlImage(new Vector2?(new Vector2(-0.19f, 0.365f)), new Vector2?(MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.MinSizeGui), backgroundColor1, textures: new string[1]
      {
        "Textures\\GUI\\Controls\\button_default_outlineless.dds"
      });
      Vector2? position2 = new Vector2?(myGuiControlImage1.Position);
      Vector2? size2 = new Vector2?();
      string text = string.Format(MyTexts.GetString(MySpaceTexts.RadialMenu_HintClose), (object) MyControllerHelper.GetCodeForControl(MyControllerHelper.CX_GUI, MyControlsGUI.CANCEL).ToString());
      Vector4? backgroundColor3 = new Vector4?();
      Vector4? colorMask = backgroundColor3;
      MyGuiControlLabel myGuiControlLabel4 = new MyGuiControlLabel(position2, size2, text, colorMask);
      myGuiControlLabel4.PositionY += myGuiControlLabel4.Size.Y / 2f + num1;
      myGuiControlLabel4.DrawAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      this.AddControl((IVRageGuiControl) myGuiControlImage1);
      this.AddControl((IVRageGuiControl) myGuiControlLabel4);
      backgroundColor3 = new Vector4?(new Vector4(1f, 1f, 1f, 0.8f));
      MyGuiControlImage myGuiControlImage2 = new MyGuiControlImage(new Vector2?(new Vector2(0.19f, 0.365f)), new Vector2?(MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.MinSizeGui), backgroundColor3, textures: new string[1]
      {
        "Textures\\GUI\\Controls\\button_default_outlineless.dds"
      });
      MyGuiControlLabel myGuiControlLabel5 = new MyGuiControlLabel(new Vector2?(myGuiControlImage2.Position), text: string.Format(MyTexts.GetString(MySpaceTexts.RadialMenu_HintConfirm), (object) MyControllerHelper.GetCodeForControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT).ToString()), originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
      myGuiControlLabel5.PositionY += myGuiControlLabel5.Size.Y / 2f + num1;
      myGuiControlLabel5.DrawAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      this.AddControl((IVRageGuiControl) myGuiControlImage2);
      this.AddControl((IVRageGuiControl) myGuiControlLabel5);
      MyGuiControlImage myGuiControlImage3 = new MyGuiControlImage();
      myGuiControlImage3.SetTexture("Textures\\GUI\\Controls\\RadialMenuBackground.dds");
      myGuiControlImage3.Size = new Vector2(884f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      this.AddControl((IVRageGuiControl) myGuiControlImage3);
      MyGuiControlImage myGuiControlImage4 = new MyGuiControlImage();
      myGuiControlImage4.SetTexture("Textures\\GUI\\Controls\\RadialOuterCircle.dds");
      myGuiControlImage4.Size = new Vector2(632f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      this.AddControl((IVRageGuiControl) myGuiControlImage4);
      MyGuiControlImage myGuiControlImage5 = new MyGuiControlImage();
      myGuiControlImage5.SetTexture("Textures\\GUI\\Controls\\RadialBrackets.dds");
      myGuiControlImage5.Size = new Vector2(674f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      this.AddControl((IVRageGuiControl) myGuiControlImage5);
      foreach (MyRadialMenuSection currentSection in this.m_data.CurrentSections)
      {
        MyGuiControlImage myGuiControlImage6 = new MyGuiControlImage(backgroundColor: new Vector4?(new Vector4(1f, 1f, 1f, 0.8f)));
        myGuiControlImage6.SetTexture("Textures\\GUI\\Controls\\button_default_outlineless.dds");
        this.m_tabs.Add(myGuiControlImage6);
        this.AddControl((IVRageGuiControl) myGuiControlImage6);
        MyGuiControlLabel myGuiControlLabel6 = new MyGuiControlLabel();
        myGuiControlLabel6.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
        MyGuiControlLabel myGuiControlLabel7 = myGuiControlLabel6;
        myGuiControlLabel7.Text = MyTexts.GetString(currentSection.Label);
        myGuiControlLabel7.DrawAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
        this.m_tabLabels.Add(myGuiControlLabel7);
        this.AddControl((IVRageGuiControl) myGuiControlLabel7);
      }
      for (int index = 0; index < 8; ++index)
      {
        MyGuiControlImageRotatable controlImageRotatable = new MyGuiControlImageRotatable();
        controlImageRotatable.SetTexture("Textures\\GUI\\Controls\\RadialSectorUnSelected.dds");
        float num2 = 0.7853982f * (float) index;
        controlImageRotatable.Rotation = num2;
        controlImageRotatable.Size = new Vector2(288f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
        controlImageRotatable.Position = new Vector2((float) Math.Cos((double) num2 - 1.57079601287842), (float) Math.Sin((double) num2 - 1.57079601287842)) * 144f / MyGuiConstants.GUI_OPTIMAL_SIZE;
        this.m_buttons.Add(controlImageRotatable);
        this.AddControl((IVRageGuiControl) controlImageRotatable);
      }
      this.GenerateIcons(8);
      this.m_buttonHighlight = new MyGuiControlImageRotatable();
      this.m_buttonHighlight.SetTexture("Textures\\GUI\\Controls\\RadialSectorSelected.dds");
      this.m_buttonHighlight.Size = new Vector2(288f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      this.m_buttonHighlight.Visible = false;
      this.AddControl((IVRageGuiControl) this.m_buttonHighlight);
      this.AddControl((IVRageGuiControl) this.m_tooltipName);
      this.AddControl((IVRageGuiControl) this.m_tooltipState);
      this.AddControl((IVRageGuiControl) this.m_tooltipShortcut);
      this.m_cancelButton = new MyGuiControlImage();
      this.AddControl((IVRageGuiControl) this.m_cancelButton);
      this.m_cancelButton.SetTexture("Textures\\GUI\\Controls\\RadialCentralCircle.dds");
      this.m_cancelButton.Size = new Vector2(126f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      MyGuiControlImage myGuiControlImage7 = new MyGuiControlImage();
      this.AddControl((IVRageGuiControl) myGuiControlImage7);
      myGuiControlImage7.SetTexture("Textures\\GUI\\Icons\\HideWeapon.dds");
      myGuiControlImage7.Size = new Vector2(90f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      MyGuiControlLabel myGuiControlLabel8 = new MyGuiControlLabel(text: MyControllerHelper.GetCodeForControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_LEFT).ToString(), textScale: 1f, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
      myGuiControlLabel8.Position = new Vector2((float) (-0.0450000017881393 - (double) MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.MinSizeGui.X / 2.0), this.CATEGORY_POS_Y);
      MyGuiControlLabel myGuiControlLabel9 = myGuiControlLabel8;
      this.m_leftButtonHint = myGuiControlLabel8;
      this.AddControl((IVRageGuiControl) myGuiControlLabel9);
      MyGuiControlLabel myGuiControlLabel10 = new MyGuiControlLabel(text: MyControllerHelper.GetCodeForControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_RIGHT).ToString(), textScale: 1f);
      myGuiControlLabel10.Position = new Vector2((float) (0.0399999991059303 + (double) MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.MinSizeGui.X / 2.0), this.CATEGORY_POS_Y);
      MyGuiControlLabel myGuiControlLabel11 = myGuiControlLabel10;
      this.m_rightButtonHint = myGuiControlLabel10;
      this.AddControl((IVRageGuiControl) myGuiControlLabel11);
      this.m_hintYPos = this.CATEGORY_POS_Y + (float) (((double) this.m_tabSizeSmall.Y - (double) this.m_leftButtonHint.Size.Y) / 2.0);
      this.m_leftButtonHint.DrawAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      this.m_rightButtonHint.DrawAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      this.m_leftButtonHint.PositionY = this.m_hintYPos + this.m_leftButtonHint.Size.Y / 2f - this.m_hintIconYPosOffset;
      this.m_rightButtonHint.PositionY = this.m_hintYPos + this.m_leftButtonHint.Size.Y / 2f - this.m_hintIconYPosOffset;
      this.UpdateHighlight(-1, -1);
      MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
      HashSet<string> textures = new HashSet<string>();
      this.GetTexturesForPreload(textures);
      MyRenderProxy.PreloadTextures((IEnumerable<string>) textures, TextureType.GUI);
    }

    private void MyCharacter_OnCharacterDied(MyCharacter obj) => this.CloseScreenNow();

    protected virtual void GenerateIcons(int maxSize)
    {
      for (int index = 0; index < maxSize; ++index)
      {
        MyGuiControlImage myGuiControlImage = new MyGuiControlImage();
        myGuiControlImage.Size = new Vector2(65f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
        this.m_icons.Add(myGuiControlImage);
        this.AddControl((IVRageGuiControl) myGuiControlImage);
        float num = (float) (6.28318548202515 / (double) maxSize * (double) index - 1.57079601287842);
        myGuiControlImage.Position = new Vector2(1f, 1.333333f) * new Vector2((float) Math.Cos((double) num), (float) Math.Sin((double) num)) * MyGuiControlRadialMenuBase.RADIUS;
      }
    }

    protected bool Cancel()
    {
      if (MySession.Static.LocalCharacter == null)
        return false;
      if (MySession.Static.ControlledEntity is MyCharacter controlledEntity)
      {
        // ISSUE: explicit non-virtual call
        __nonvirtual (controlledEntity.SwitchToWeapon((MyToolbarItemWeapon) null));
      }
      MySessionComponentVoxelHand.Static.Enabled = false;
      MyClipboardComponent.Static?.HandleEscapeInternal();
      this.CloseScreen();
      return true;
    }

    protected override void OnClosed()
    {
      MyToolSwitcher component = MySession.Static.GetComponent<MyToolSwitcher>();
      component.SwitchingEnabled = false;
      component.ToolSwitched -= new Action<bool>(((MyGuiScreenBase) this).CloseScreenNow);
      MyGuiScreenGamePlay.Static.SuppressMovement = MySession.Static.ElapsedGameTime + MyGuiControlRadialMenuBase.MOVEMENT_SUPPRESSION;
      base.OnClosed();
    }

    protected bool ButtonAction(int section, int itemIndex)
    {
      List<MyRadialMenuItem> items = this.m_data.CurrentSections[section].Items;
      if (items.Count <= itemIndex)
        return false;
      MyRadialMenuItem myRadialMenuItem = items[itemIndex];
      if (!myRadialMenuItem.CanBeActivated)
        return false;
      this.ActivateItem(myRadialMenuItem);
      if (myRadialMenuItem.CloseMenu)
      {
        this.CloseScreen();
      }
      else
      {
        this.UpdateTooltip();
        this.UpdateIcon();
      }
      return true;
    }

    protected virtual void UpdateIcon()
    {
      MyRadialMenuSection currentSection = this.m_data.CurrentSections[this.m_currentSection];
      for (int index = 0; index < currentSection.Items.Count; ++index)
        this.m_icons[index].SetTexture(currentSection.Items[index].GetIcon());
    }

    protected virtual void ActivateItem(MyRadialMenuItem item) => item.Activate();

    protected void SwitchSection(int index)
    {
      List<MyRadialMenuSection> currentSections = this.m_data.CurrentSections;
      int index1 = index < 0 ? 0 : (index >= currentSections.Count ? currentSections.Count - 1 : index);
      this.m_currentSection = index1;
      MyGuiControlRadialMenuBase.m_lastSelectedSection[this.m_data.Id] = index1;
      float categoryPosY = this.CATEGORY_POS_Y;
      MyRadialMenuSection currentSection = this.m_data.CurrentSections[index1];
      for (int index2 = 0; index2 < this.m_tabs.Count; ++index2)
      {
        if (index2 == index1)
        {
          MyGuiControlImage tab = this.m_tabs[index2];
          MyGuiControlLabel tabLabel = this.m_tabLabels[index2];
          Vector2 vector2_1 = new Vector2(0.0f, categoryPosY);
          Vector2 vector2_2 = vector2_1;
          tabLabel.Position = vector2_2;
          Vector2 vector2_3 = vector2_1;
          tab.Position = vector2_3;
          this.m_tabs[index2].Size = this.m_tabSize;
          this.m_tabs[index2].SetTexture("Textures\\GUI\\Controls\\button_default_outlineless_active.dds");
          this.m_tabLabels[index2].IsAutoScaleEnabled = true;
          this.m_tabLabels[index2].IsAutoEllipsisEnabled = true;
          this.m_tabLabels[index2].SetMaxWidth(this.m_tabs[index2].Size.X);
          this.m_tabLabels[index2].DoEllipsisAndScaleAdjust(true, 0.8f, true);
          this.m_tabLabels[index2].PositionY = this.m_hintYPos;
          this.m_tabLabels[index2].DrawAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
        }
        else
        {
          if (Math.Abs(index2 - index1) == 1)
          {
            this.m_tabs[index2].Position = new Vector2((float) (index2 - index1) * 0.21f, categoryPosY);
            this.m_tabs[index2].Size = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.MinSizeGui;
            this.m_tabLabels[index2].IsAutoScaleEnabled = true;
            this.m_tabLabels[index2].IsAutoEllipsisEnabled = true;
            this.m_tabLabels[index2].SetMaxWidth((float) ((double) this.m_tabs[index2].Size.X - (double) this.m_leftButtonHint.Size.X - 0.025000000372529));
            this.m_tabLabels[index2].DoEllipsisAndScaleAdjust(true, 0.8f, true);
            this.m_tabLabels[index2].Position = new Vector2((float) (index2 - index1) * 0.22f, this.m_hintYPos);
            this.m_tabLabels[index2].DrawAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
          }
          else
          {
            this.m_tabs[index2].Position = new Vector2((float) ((double) Math.Sign(index2 - index1) * 0.284999996423721 + (double) (index2 - index1) * 0.00999999977648258), categoryPosY);
            this.m_tabs[index2].Size = this.m_tabSizeSmall;
            this.m_tabLabels[index2].IsAutoScaleEnabled = true;
            this.m_tabLabels[index2].IsAutoEllipsisEnabled = true;
            this.m_tabLabels[index2].SetMaxWidth(this.m_tabs[index2].Size.X);
            this.m_tabLabels[index2].DoEllipsisAndScaleAdjust(true, 0.8f, true);
            this.m_tabLabels[index2].PositionY = this.m_hintYPos;
            this.m_tabLabels[index2].DrawAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
          }
          this.m_tabs[index2].SetTexture("Textures\\GUI\\Controls\\button_default_outlineless.dds");
        }
        this.m_tabLabels[index2].Visible = Math.Abs(index2 - index1) <= 1;
      }
      this.SetIconTextures(currentSection);
      this.m_tooltipName.Visible = false;
      this.m_tooltipState.Visible = false;
      this.m_tooltipShortcut.Visible = false;
      this.m_leftButtonHint.Visible = (uint) index1 > 0U;
      this.m_rightButtonHint.Visible = index1 != this.m_data.CurrentSections.Count - 1;
      this.UpdateTooltip();
      this.RegenerateBlockHints();
    }

    protected virtual void SetIconTextures(MyRadialMenuSection selectedSection)
    {
      for (int index = 0; index < this.m_buttons.Count; ++index)
      {
        MyGuiControlImageRotatable button = this.m_buttons[index];
        MyGuiControlImage icon = this.m_icons[index];
        if (index < selectedSection.Items.Count)
        {
          button.Visible = icon.Visible = true;
          MyRadialMenuItem myRadialMenuItem = selectedSection.Items[index];
          icon.SetTexture(myRadialMenuItem.GetIcon());
          icon.ColorMask = (Vector4) (myRadialMenuItem.Enabled() ? Color.White : Color.Gray);
        }
        else
          icon.Visible = false;
      }
    }

    public override bool Update(bool hasFocus)
    {
      if (!MyInput.Static.IsJoystickLastUsed)
      {
        this.CloseScreen();
        return base.Update(hasFocus);
      }
      Vector3 positionForGameplay = MyInput.Static.GetJoystickPositionForGameplay(RequestedJoystickAxis.NoZ);
      Vector3 rotationForGameplay = MyInput.Static.GetJoystickRotationForGameplay(RequestedJoystickAxis.NoZ);
      Vector3 vector3 = Vector3.IsZero(positionForGameplay) ? rotationForGameplay : positionForGameplay;
      if (!Vector3.IsZero(vector3))
      {
        int newIndex = (int) Math.Round((6.28318548202515 + (1.57079601287842 + Math.Atan2((double) vector3.Y, (double) vector3.X))) % 6.28318548202515 / (6.28318548202515 / (double) this.m_buttons.Count)) % this.m_buttons.Count;
        if (newIndex != this.m_selectedButton)
        {
          this.UpdateHighlight(this.m_selectedButton, newIndex);
          this.m_selectedButton = newIndex;
          this.UpdateTooltip();
          this.RegenerateBlockHints();
          MyGuiSoundManager.PlaySound(GuiSounds.MouseOver);
        }
      }
      else if (this.m_cancelButton != null && this.m_selectedButton != -1)
      {
        this.UpdateHighlight(this.m_selectedButton, -1);
        this.m_selectedButton = -1;
        this.UpdateTooltip();
        this.RegenerateBlockHints();
        MyGuiSoundManager.PlaySound(GuiSounds.MouseOver);
      }
      return base.Update(hasFocus);
    }

    protected virtual void RegenerateBlockHints()
    {
    }

    protected abstract void UpdateTooltip();

    protected virtual void UpdateHighlight(int oldIndex, int newIndex)
    {
      if (oldIndex == -1)
        this.m_cancelButton.SetTexture("Textures\\GUI\\Controls\\RadialCentralCircle.dds");
      if (newIndex == -1)
      {
        this.m_cancelButton.SetTexture("Textures\\GUI\\Controls\\RadialCentralCircleSelected.dds");
        this.m_buttonHighlight.Visible = false;
      }
      else
      {
        float num = 0.7853982f * (float) newIndex;
        this.m_buttonHighlight.Rotation = num;
        this.m_buttonHighlight.Position = new Vector2((float) Math.Cos((double) num - 1.57079601287842), (float) Math.Sin((double) num - 1.57079601287842)) * 144f / MyGuiConstants.GUI_OPTIMAL_SIZE;
        this.m_buttonHighlight.Visible = true;
      }
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_LEFT) && this.m_currentSection > 0)
      {
        this.SwitchSection(this.m_currentSection - 1);
        MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
      }
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_RIGHT) && this.m_data.CurrentSections.Count > this.m_currentSection + 1)
      {
        this.SwitchSection(this.m_currentSection + 1);
        MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
      }
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT))
      {
        if ((this.m_selectedButton == -1 ? (this.Cancel() ? 1 : 0) : (this.ButtonAction(this.m_currentSection, this.m_selectedButton) ? 1 : 0)) != 0)
          MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
        else
          MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
      }
      else if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, this.m_closingControl) || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.CANCEL))
      {
        this.CloseScreen();
        MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
      }
      else
      {
        Func<bool> handleInputCallback = this.m_handleInputCallback;
        if ((handleInputCallback != null ? (handleInputCallback() ? 1 : 0) : 0) == 0)
          return;
        this.CloseScreen();
      }
    }

    public virtual void GetTexturesForPreload(HashSet<string> textures)
    {
      foreach (MyGuiControlBase control in this.Controls)
      {
        if (control is MyGuiControlImage myGuiControlImage && myGuiControlImage.Textures != null)
        {
          foreach (MyGuiControlImage.MyDrawTexture texture in myGuiControlImage.Textures)
          {
            Add(texture.Texture);
            Add(texture.MaskTexture);
          }
        }
      }
      foreach (MyRadialMenuSection radialMenuSection in this.m_data.SectionsComplete)
      {
        foreach (MyRadialMenuItem myRadialMenuItem in radialMenuSection.Items)
          Add(myRadialMenuItem.GetIcon());
      }

      void Add(string texture)
      {
        if (string.IsNullOrEmpty(texture))
          return;
        textures.Add(texture);
      }
    }

    public override string GetFriendlyName() => "RadialMenu";
  }
}
