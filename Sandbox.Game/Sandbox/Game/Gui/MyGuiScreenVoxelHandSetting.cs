// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenVoxelHandSetting
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  internal class MyGuiScreenVoxelHandSetting : MyGuiScreenBase
  {
    private static readonly Vector2 SCREEN_SIZE = new Vector2(0.37f, 1.2f);
    private static readonly float HIDDEN_PART_RIGHT = 0.04f;
    private Vector2 m_controlPadding = new Vector2(0.02f, 0.02f);
    private MyGuiControlLabel m_labelSettings;
    private MyGuiControlLabel m_labelSnapToVoxel;
    private MyGuiControlCheckbox m_checkSnapToVoxel;
    private MyGuiControlLabel m_labelProjectToVoxel;
    private MyGuiControlCheckbox m_projectToVoxel;
    private MyGuiControlLabel m_labelFreezePhysics;
    private MyGuiControlCheckbox m_freezePhysicsCheck;
    private MyGuiControlLabel m_labelShowGizmos;
    private MyGuiControlCheckbox m_showGizmos;
    private MyGuiControlLabel m_labelTransparency;
    private MyGuiControlSlider m_sliderTransparency;
    private MyGuiControlLabel m_labelZoom;
    private MyGuiControlSlider m_sliderZoom;
    private MyGuiControlVoxelHandSettings m_voxelControl;

    public MyGuiScreenVoxelHandSetting()
      : base(new Vector2?(new Vector2(MyGuiManager.GetMaxMouseCoord().X - MyGuiScreenVoxelHandSetting.SCREEN_SIZE.X * 0.5f + MyGuiScreenVoxelHandSetting.HIDDEN_PART_RIGHT, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR * MySandboxGame.Config.UIBkOpacity), new Vector2?(MyGuiScreenVoxelHandSetting.SCREEN_SIZE))
    {
      this.CanHideOthers = false;
      this.EnabledBackgroundFade = false;
      this.RecreateControls(true);
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      float num1 = -0.465f;
      float num2 = (float) (((double) MyGuiScreenVoxelHandSetting.SCREEN_SIZE.Y - 1.0) / 2.0);
      this.AddCaption(MyTexts.Get(MyCommonTexts.VoxelHandSettingScreen_Caption).ToString(), new Vector4?(Color.White.ToVector4()), new Vector2?(this.m_controlPadding + new Vector2(-MyGuiScreenVoxelHandSetting.HIDDEN_PART_RIGHT, num2 - 0.03f)));
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), 0.44f), this.m_size.Value.X * 0.73f);
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), 0.048f), this.m_size.Value.X * 0.73f);
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), -0.235f), this.m_size.Value.X * 0.73f);
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), -0.394f), this.m_size.Value.X * 0.73f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList);
      float y1 = num1 + 0.042f;
      MyGuiControlCheckbox guiControlCheckbox1 = new MyGuiControlCheckbox();
      guiControlCheckbox1.Position = new Vector2(0.12f, y1);
      guiControlCheckbox1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      this.m_checkSnapToVoxel = guiControlCheckbox1;
      this.m_checkSnapToVoxel.IsChecked = MySessionComponentVoxelHand.Static.SnapToVoxel;
      this.m_checkSnapToVoxel.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.SnapToVoxel_Changed);
      float y2 = y1 + 0.01f;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.Position = new Vector2(-0.15f, y2);
      myGuiControlLabel1.TextEnum = MyCommonTexts.VoxelHandSettingScreen_HandSnapToVoxel;
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_labelSnapToVoxel = myGuiControlLabel1;
      float y3 = y2 + 0.036f;
      MyGuiControlCheckbox guiControlCheckbox2 = new MyGuiControlCheckbox();
      guiControlCheckbox2.Position = new Vector2(0.12f, y3);
      guiControlCheckbox2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      this.m_projectToVoxel = guiControlCheckbox2;
      this.m_projectToVoxel.IsChecked = MySessionComponentVoxelHand.Static.ProjectToVoxel;
      this.m_projectToVoxel.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.ProjectToVoxel_Changed);
      float y4 = y3 + 0.01f;
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel();
      myGuiControlLabel2.Position = new Vector2(-0.15f, y4);
      myGuiControlLabel2.TextEnum = MyCommonTexts.VoxelHandSettingScreen_HandProjectToVoxel;
      myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_labelProjectToVoxel = myGuiControlLabel2;
      float y5 = y4 + 0.036f;
      MyGuiControlCheckbox guiControlCheckbox3 = new MyGuiControlCheckbox();
      guiControlCheckbox3.Position = new Vector2(0.12f, y5);
      guiControlCheckbox3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      this.m_freezePhysicsCheck = guiControlCheckbox3;
      this.m_freezePhysicsCheck.IsChecked = MySessionComponentVoxelHand.Static.FreezePhysics;
      this.m_freezePhysicsCheck.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.FreezePhysics_Changed);
      float y6 = y5 + 0.01f;
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel();
      myGuiControlLabel3.Position = new Vector2(-0.15f, y6);
      myGuiControlLabel3.TextEnum = MyCommonTexts.VoxelHandSettingScreen_FreezePhysics;
      myGuiControlLabel3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_labelFreezePhysics = myGuiControlLabel3;
      float y7 = y6 + 0.036f;
      MyGuiControlCheckbox guiControlCheckbox4 = new MyGuiControlCheckbox();
      guiControlCheckbox4.Position = new Vector2(0.12f, y7);
      guiControlCheckbox4.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      this.m_showGizmos = guiControlCheckbox4;
      this.m_showGizmos.IsChecked = MySessionComponentVoxelHand.Static.ShowGizmos;
      this.m_showGizmos.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.ShowGizmos_Changed);
      float y8 = y7 + 0.01f;
      MyGuiControlLabel myGuiControlLabel4 = new MyGuiControlLabel();
      myGuiControlLabel4.Position = new Vector2(-0.15f, y8);
      myGuiControlLabel4.TextEnum = MyCommonTexts.VoxelHandSettingScreen_HandShowGizmos;
      myGuiControlLabel4.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_labelShowGizmos = myGuiControlLabel4;
      float y9 = y8 + 0.045f;
      MyGuiControlLabel myGuiControlLabel5 = new MyGuiControlLabel();
      myGuiControlLabel5.Position = new Vector2(-0.15f, y9);
      myGuiControlLabel5.TextEnum = MyCommonTexts.VoxelHandSettingScreen_HandTransparency;
      myGuiControlLabel5.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_labelTransparency = myGuiControlLabel5;
      float y10 = y9 + 0.027f;
      MyGuiControlSlider guiControlSlider1 = new MyGuiControlSlider();
      guiControlSlider1.Position = new Vector2(-0.15f, y10);
      guiControlSlider1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_sliderTransparency = guiControlSlider1;
      this.m_sliderTransparency.Size = new Vector2(0.263f, 0.1f);
      this.m_sliderTransparency.MinValue = 0.0f;
      this.m_sliderTransparency.MaxValue = 1f;
      this.m_sliderTransparency.Value = 1f - MySessionComponentVoxelHand.Static.ShapeColor.ToVector4().W;
      this.m_sliderTransparency.ValueChanged += new Action<MyGuiControlSlider>(this.BrushTransparency_ValueChanged);
      float y11 = y10 + 57f / 1000f;
      MyGuiControlLabel myGuiControlLabel6 = new MyGuiControlLabel();
      myGuiControlLabel6.Position = new Vector2(-0.15f, y11);
      myGuiControlLabel6.TextEnum = MyCommonTexts.VoxelHandSettingScreen_HandDistance;
      myGuiControlLabel6.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_labelZoom = myGuiControlLabel6;
      float y12 = y11 + 0.027f;
      MyGuiControlSlider guiControlSlider2 = new MyGuiControlSlider();
      guiControlSlider2.Position = new Vector2(-0.15f, y12);
      guiControlSlider2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_sliderZoom = guiControlSlider2;
      this.m_sliderZoom.Size = new Vector2(0.263f, 0.1f);
      this.m_sliderZoom.MaxValue = MySessionComponentVoxelHand.MAX_BRUSH_ZOOM;
      this.m_sliderZoom.Value = MySessionComponentVoxelHand.Static.GetBrushZoom();
      this.m_sliderZoom.MinValue = MySessionComponentVoxelHand.MIN_BRUSH_ZOOM;
      this.m_sliderZoom.Enabled = !MySessionComponentVoxelHand.Static.ProjectToVoxel;
      this.m_sliderZoom.ValueChanged += new Action<MyGuiControlSlider>(this.BrushZoom_ValueChanged);
      this.m_voxelControl = new MyGuiControlVoxelHandSettings();
      this.m_voxelControl.Position = new Vector2(-0.05f, 0.17f);
      this.m_voxelControl.Size = new Vector2(0.3f, 0.4f);
      this.m_voxelControl.Item = MyToolbarComponent.CurrentToolbar.SelectedItem as MyToolbarItemVoxelHand;
      this.m_voxelControl.UpdateFromBrush(MySessionComponentVoxelHand.Static.CurrentShape);
      StringBuilder stringBuilder1;
      if (MyInput.Static.IsJoystickLastUsed)
      {
        StringBuilder stringBuilder2 = new StringBuilder(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.PRIMARY_TOOL_ACTION));
        StringBuilder stringBuilder3 = new StringBuilder(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.SECONDARY_TOOL_ACTION));
        StringBuilder stringBuilder4 = new StringBuilder(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.VOXEL_MATERIAL_SELECT));
        stringBuilder1 = new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.VoxelHands_Description_Gamepad), (object) stringBuilder2, (object) stringBuilder3, (object) stringBuilder4));
      }
      else
      {
        StringBuilder output1 = (StringBuilder) null;
        StringBuilder output2 = (StringBuilder) null;
        StringBuilder output3 = (StringBuilder) null;
        StringBuilder output4 = (StringBuilder) null;
        MyInput.Static.GetGameControl(MyControlsSpace.PRIMARY_TOOL_ACTION).AppendBoundButtonNames(ref output1, unassignedText: MyInput.Static.GetUnassignedName());
        MyInput.Static.GetGameControl(MyControlsSpace.SECONDARY_TOOL_ACTION).AppendBoundButtonNames(ref output2, unassignedText: MyInput.Static.GetUnassignedName());
        MyInput.Static.GetGameControl(MyControlsSpace.SWITCH_LEFT).AppendBoundButtonNames(ref output3, unassignedText: MyInput.Static.GetUnassignedName());
        MyInput.Static.GetGameControl(MyControlsSpace.SWITCH_RIGHT).AppendBoundButtonNames(ref output4, unassignedText: MyInput.Static.GetUnassignedName());
        stringBuilder1 = new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.VoxelHands_Description), (object) output1, (object) output2, (object) output3, (object) output4));
      }
      MyGuiControlMultilineText controlMultilineText = new MyGuiControlMultilineText(new Vector2?(new Vector2(-0.15f, 0.252f)), new Vector2?(new Vector2(0.275f, 0.125f)));
      controlMultilineText.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlMultilineText.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlMultilineText.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlMultilineText.Text = stringBuilder1;
      this.Controls.Add((MyGuiControlBase) controlMultilineText);
      Vector2 vector2_1 = new Vector2(-0.083f, 0.36f);
      Vector2 vector2_2 = new Vector2(0.134f, 0.038f);
      MyGuiControlButton button = this.CreateButton(0.265f, MyTexts.Get(MyCommonTexts.Close), new Action<MyGuiControlButton>(this.OKButtonClicked), tooltip: new MyStringId?(MySpaceTexts.ToolTipNewsletter_Close), textScale: 0.8f);
      button.Position = vector2_1 + new Vector2(0.0f, 2f) * vector2_2;
      button.PositionX += vector2_2.X / 2f;
      button.ShowTooltipWhenDisabled = true;
      this.Controls.Add((MyGuiControlBase) button);
      this.Controls.Add((MyGuiControlBase) this.m_labelSnapToVoxel);
      this.Controls.Add((MyGuiControlBase) this.m_checkSnapToVoxel);
      this.Controls.Add((MyGuiControlBase) this.m_labelShowGizmos);
      this.Controls.Add((MyGuiControlBase) this.m_showGizmos);
      this.Controls.Add((MyGuiControlBase) this.m_labelProjectToVoxel);
      this.Controls.Add((MyGuiControlBase) this.m_projectToVoxel);
      this.Controls.Add((MyGuiControlBase) this.m_labelFreezePhysics);
      this.Controls.Add((MyGuiControlBase) this.m_freezePhysicsCheck);
      this.Controls.Add((MyGuiControlBase) this.m_labelTransparency);
      this.Controls.Add((MyGuiControlBase) this.m_sliderTransparency);
      this.Controls.Add((MyGuiControlBase) this.m_labelZoom);
      this.Controls.Add((MyGuiControlBase) this.m_sliderZoom);
      this.Controls.Add((MyGuiControlBase) this.m_voxelControl);
    }

    private MyGuiControlButton CreateButton(
      float usableWidth,
      StringBuilder text,
      Action<MyGuiControlButton> onClick,
      bool enabled = true,
      MyStringId? tooltip = null,
      float textScale = 1f)
    {
      Vector2? position = new Vector2?();
      Vector2? size = new Vector2?();
      Vector4? colorMask = new Vector4?();
      StringBuilder text1 = text;
      Action<MyGuiControlButton> action = onClick;
      double num = (double) textScale;
      Action<MyGuiControlButton> onButtonClick = action;
      int? buttonIndex = new int?();
      MyGuiControlButton guiControlButton = new MyGuiControlButton(position, MyGuiControlButtonStyleEnum.Rectangular, size, colorMask, text: text1, textScale: ((float) num), onButtonClick: onButtonClick, buttonIndex: buttonIndex);
      guiControlButton.Size = new Vector2(usableWidth, 0.034f);
      guiControlButton.Position = guiControlButton.Position + new Vector2(-0.02f, 0.0f);
      if (tooltip.HasValue)
        guiControlButton.SetToolTip(tooltip.Value);
      return guiControlButton;
    }

    private void SnapToVoxel_Changed(MyGuiControlCheckbox sender) => MySessionComponentVoxelHand.Static.SnapToVoxel = this.m_checkSnapToVoxel.IsChecked;

    private void ShowGizmos_Changed(MyGuiControlCheckbox sender) => MySessionComponentVoxelHand.Static.ShowGizmos = this.m_showGizmos.IsChecked;

    private void ProjectToVoxel_Changed(MyGuiControlCheckbox sender)
    {
      MySessionComponentVoxelHand.Static.ProjectToVoxel = this.m_projectToVoxel.IsChecked;
      this.m_sliderZoom.Enabled = !this.m_projectToVoxel.IsChecked;
    }

    private void FreezePhysics_Changed(MyGuiControlCheckbox sender) => MySessionComponentVoxelHand.Static.FreezePhysics = sender.IsChecked;

    private void BrushTransparency_ValueChanged(MyGuiControlSlider sender) => MySessionComponentVoxelHand.Static.ShapeColor.A = (byte) ((1.0 - (double) this.m_sliderTransparency.Value) * (double) byte.MaxValue);

    private void BrushZoom_ValueChanged(MyGuiControlSlider sender) => MySessionComponentVoxelHand.Static.SetBrushZoom(this.m_sliderZoom.Value);

    private void OKButtonClicked(MyGuiControlButton sender) => this.CloseScreen();

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.VOXEL_HAND_SETTINGS))
        this.CloseScreen();
      base.HandleInput(receivedFocusInThisUpdate);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenVoxelHandSetting);
  }
}
