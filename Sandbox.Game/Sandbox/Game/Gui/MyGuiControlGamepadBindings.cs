// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiControlGamepadBindings
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyGuiControlGamepadBindings : MyGuiControlParent
  {
    private const float VIEWPORT_H = 762f;
    private const float VIEWPORT_W = 1320f;
    private const float CONTROL_LEFT_ALIGN = 0.3401515f;
    private const float CONTROL_RIGHT_ALIGN = 0.6598485f;
    private const int LEFT_CONTROLS = 8;
    private readonly float[] CONTROL_VERTICAL_ALIGNS = new float[19]
    {
      120f,
      225f,
      330f,
      434f,
      538f,
      510f,
      560f,
      642f,
      120f,
      187f,
      245f,
      307f,
      372f,
      437f,
      502f,
      565f,
      632f,
      630f,
      680f
    };
    private readonly char[] ICONS = new char[19]
    {
      MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_ZPOS", (string) null)[0],
      MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("BUTTON_J05", (string) null)[0],
      MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("BUTTON_J07", (string) null)[0],
      MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("BUTTON_J09", (string) null)[0],
      MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_MOTION", (string) null)[0],
      MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_MOTION_X", (string) null)[0],
      MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_MOTION_Y", (string) null)[0],
      MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_DPAD", (string) null)[0],
      MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_ZNEG", (string) null)[0],
      MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("BUTTON_J06", (string) null)[0],
      MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("BUTTON_J08", (string) null)[0],
      MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("BUTTON_J04", (string) null)[0],
      MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("BUTTON_J02", (string) null)[0],
      MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("BUTTON_J01", (string) null)[0],
      MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("BUTTON_J03", (string) null)[0],
      MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("BUTTON_J10", (string) null)[0],
      MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_ROTATION", (string) null)[0],
      MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_ROTATION_X", (string) null)[0],
      MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_ROTATION_Y", (string) null)[0]
    };
    private const string GAMEPAD_IMAGE = "Textures\\GUI\\HelpScreen\\ControllerSchema.png";
    private const string GAMEPAD_IMAGE_BACKGROUND = "Textures\\GUI\\Screens\\image_background.dds";
    private BindingType m_currentType;
    private ControlScheme m_currentScheme;

    public MyGuiControlGamepadBindings(BindingType type, ControlScheme scheme, bool isFullWidth = true)
    {
      this.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK;
      this.ColorMask = new Vector4(1f, 0.3f, 0.3f, 1f);
      this.Size = new Vector2(isFullWidth ? 0.553f : 0.5f, 0.35f);
      this.m_currentType = type;
      this.m_currentScheme = scheme;
      this.Recreate();
    }

    private void Recreate()
    {
      this.Controls.Clear();
      switch (this.m_currentScheme)
      {
        case ControlScheme.Default:
          switch (this.m_currentType)
          {
            case BindingType.Character:
              this.RecreateCharacter();
              return;
            case BindingType.Jetpack:
              this.RecreateJetpack();
              return;
            case BindingType.Ship:
              this.RecreateShip();
              return;
            default:
              return;
          }
        case ControlScheme.Alternative:
          switch (this.m_currentType)
          {
            case BindingType.Character:
              this.RecreateAltCharacter();
              return;
            case BindingType.Jetpack:
              this.RecreateAltJetpack();
              return;
            case BindingType.Ship:
              this.RecreateAltShip();
              return;
            default:
              return;
          }
      }
    }

    private void RecreateCharacter() => this.Controls.Add((MyGuiControlBase) this.AddControllerSchema(MySpaceTexts.HelpScreen_ControllerCharacterControl, false, false, MySpaceTexts.HelpScreen_ControllerSecondaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.Inventory, MySpaceTexts.HelpScreen_ControllerBuildMenu, MySpaceTexts.HelpScreen_ControllerHorizontalMover, MyStringId.NullOrEmpty, MyStringId.NullOrEmpty, MySpaceTexts.HelpScreen_ControllerTools, MySpaceTexts.HelpScreen_ControllerPrimaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.RadialMenuGroupTitle_Menu, MySpaceTexts.ControlName_JetpackOn, MySpaceTexts.ControlName_Crouch, MySpaceTexts.ControlName_Jump, MyCommonTexts.ControlName_UseOrInteract, MySpaceTexts.HelpScreen_ControllerSystemMenu, MySpaceTexts.HelpScreen_ControllerRotation, MyStringId.NullOrEmpty, MyStringId.NullOrEmpty));

    private void RecreateJetpack() => this.Controls.Add((MyGuiControlBase) this.AddControllerSchema(MySpaceTexts.HelpScreen_ControllerJetpackControl, false, false, MySpaceTexts.HelpScreen_ControllerSecondaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.Inventory, MySpaceTexts.HelpScreen_ControllerBuildMenu, MySpaceTexts.HelpScreen_ControllerHorizontalMover, MyStringId.NullOrEmpty, MyStringId.NullOrEmpty, MySpaceTexts.HelpScreen_ControllerTools, MySpaceTexts.HelpScreen_ControllerPrimaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.RadialMenuGroupTitle_Menu, MySpaceTexts.ControlName_JetpackOff, MySpaceTexts.ControlName_Down, MySpaceTexts.ControlName_Up, MyCommonTexts.ControlName_UseOrInteract, MySpaceTexts.HelpScreen_ControllerSystemMenu, MySpaceTexts.HelpScreen_ControllerRotation, MyStringId.NullOrEmpty, MyStringId.NullOrEmpty));

    private void RecreateShip() => this.Controls.Add((MyGuiControlBase) this.AddControllerSchema(MySpaceTexts.HelpScreen_ControllerShipControl, false, false, MySpaceTexts.HelpScreen_ControllerSecondaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.Inventory, MyStringId.NullOrEmpty, MySpaceTexts.HelpScreen_ControllerHorizontalMover, MyStringId.NullOrEmpty, MyStringId.NullOrEmpty, MySpaceTexts.HelpScreen_ControllerShipActions, MySpaceTexts.HelpScreen_ControllerPrimaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.RadialMenuGroupTitle_Menu, MySpaceTexts.BlockActionTitle_Lock, MySpaceTexts.HelpScreen_ControllerFlyDown, MySpaceTexts.HelpScreen_ControllerFlyUp, MySpaceTexts.HelpScreen_ControllerLeaveControl, MySpaceTexts.HelpScreen_ControllerSystemMenu, MySpaceTexts.HelpScreen_ControllerRotation, MyStringId.NullOrEmpty, MyStringId.NullOrEmpty));

    private void RecreateAltCharacter() => this.Controls.Add((MyGuiControlBase) this.AddControllerSchema(MySpaceTexts.HelpScreen_ControllerCharacterControl, false, false, MySpaceTexts.HelpScreen_ControllerSecondaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.Inventory, MySpaceTexts.HelpScreen_ControllerBuildMenu, MySpaceTexts.HelpScreen_ControllerHorizontalMover, MyStringId.NullOrEmpty, MyStringId.NullOrEmpty, MySpaceTexts.HelpScreen_ControllerTools, MySpaceTexts.HelpScreen_ControllerPrimaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.RadialMenuGroupTitle_Menu, MySpaceTexts.ControlName_JetpackOn, MySpaceTexts.ControlName_Crouch, MySpaceTexts.ControlName_Jump, MyCommonTexts.ControlName_UseOrInteract, MySpaceTexts.HelpScreen_ControllerSystemMenu, MySpaceTexts.HelpScreen_ControllerRotation, MyStringId.NullOrEmpty, MyStringId.NullOrEmpty));

    private void RecreateAltJetpack() => this.Controls.Add((MyGuiControlBase) this.AddControllerSchema(MySpaceTexts.HelpScreen_ControllerJetpackControl, true, true, MySpaceTexts.HelpScreen_ControllerSecondaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.Inventory, MySpaceTexts.HelpScreen_ControllerBuildMenu, MyStringId.NullOrEmpty, MySpaceTexts.HelpScreen_ControllerHorizontalMover_Forward, MySpaceTexts.HelpScreen_ControllerRotation_Yaw, MySpaceTexts.HelpScreen_ControllerTools, MySpaceTexts.HelpScreen_ControllerPrimaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.RadialMenuGroupTitle_Menu, MySpaceTexts.ControlName_JetpackOff, MyStringId.NullOrEmpty, MyStringId.NullOrEmpty, MyCommonTexts.ControlName_UseOrInteract, MySpaceTexts.HelpScreen_ControllerSystemMenu, MyStringId.NullOrEmpty, MySpaceTexts.HelpScreen_ControllerRotation_Pitch, MySpaceTexts.HelpScreen_ControllerRotation_Roll));

    private void RecreateAltShip() => this.Controls.Add((MyGuiControlBase) this.AddControllerSchema(MySpaceTexts.HelpScreen_ControllerShipControl, true, true, MySpaceTexts.HelpScreen_ControllerSecondaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.Inventory, MyStringId.NullOrEmpty, MyStringId.NullOrEmpty, MySpaceTexts.HelpScreen_ControllerHorizontalMover_Forward, MySpaceTexts.HelpScreen_ControllerRotation_Yaw, MySpaceTexts.HelpScreen_ControllerShipActions, MySpaceTexts.HelpScreen_ControllerPrimaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.RadialMenuGroupTitle_Menu, MySpaceTexts.BlockActionTitle_Lock, MyStringId.NullOrEmpty, MyStringId.NullOrEmpty, MySpaceTexts.HelpScreen_ControllerLeaveControl, MySpaceTexts.HelpScreen_ControllerSystemMenu, MyStringId.NullOrEmpty, MySpaceTexts.HelpScreen_ControllerRotation_Pitch, MySpaceTexts.HelpScreen_ControllerRotation_Roll));

    private MyGuiControlParent AddImagePanel(string imagePath)
    {
      MyGuiControlParent guiControlParent = new MyGuiControlParent();
      guiControlParent.Size = this.Size;
      MyGuiControlImage myGuiControlImage1 = new MyGuiControlImage();
      myGuiControlImage1.Size = guiControlParent.Size;
      myGuiControlImage1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      myGuiControlImage1.Position = Vector2.Zero;
      myGuiControlImage1.BorderEnabled = true;
      myGuiControlImage1.BorderSize = 1;
      myGuiControlImage1.BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f);
      MyGuiControlImage myGuiControlImage2 = myGuiControlImage1;
      myGuiControlImage2.SetTexture("Textures\\GUI\\Screens\\image_background.dds");
      MyGuiControlImage myGuiControlImage3 = new MyGuiControlImage();
      myGuiControlImage3.Size = myGuiControlImage2.Size;
      myGuiControlImage3.OriginAlign = myGuiControlImage2.OriginAlign;
      myGuiControlImage3.Position = myGuiControlImage2.Position;
      myGuiControlImage3.BorderEnabled = true;
      myGuiControlImage3.BorderSize = 1;
      myGuiControlImage3.BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f);
      MyGuiControlImage myGuiControlImage4 = myGuiControlImage3;
      myGuiControlImage4.SetTexture(imagePath);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlImage2);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlImage4);
      return guiControlParent;
    }

    private MyGuiControlParent AddControllerSchema(
      MyStringId title,
      bool splitLS,
      bool splitRS,
      params MyStringId[] controls)
    {
      MyGuiControlParent panel = this.AddImagePanel("Textures\\GUI\\HelpScreen\\ControllerSchema.png");
      for (int index = 0; index < this.CONTROL_VERTICAL_ALIGNS.Length; ++index)
      {
        int num = index < 8 ? 1 : 0;
        float y = this.CONTROL_VERTICAL_ALIGNS[index] / 762f;
        string str = !(controls[index] == MyStringId.NullOrEmpty) ? MyTexts.GetString(controls[index]) : "—";
        string text;
        float x;
        MyGuiDrawAlignEnum align;
        if (num != 0)
        {
          text = string.Format("{0}  {1}", (object) str, (object) this.ICONS[index]);
          x = 0.3401515f;
          align = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
        }
        else
        {
          text = string.Format("{0}  {1}", (object) this.ICONS[index], (object) str);
          x = 0.6598485f;
          align = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
        }
        if ((!splitLS || index != 4) && (splitLS || index != 5 && index != 6) && ((!splitRS || index != 16) && (splitRS || index != 17 && index != 18)))
          Add(x, y, align, (MyGuiControlBase) new MyGuiControlLabel(text: text, textScale: 0.9f, isAutoEllipsisEnabled: true, maxWidth: 0.18f, isAutoScaleEnabled: true));
      }
      Add(0.5f, 0.05f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, (MyGuiControlBase) new MyGuiControlLabel()
      {
        TextEnum = title,
        TextScale = 1.2f
      });
      return panel;

      void Add(float x, float y, MyGuiDrawAlignEnum align, MyGuiControlBase control)
      {
        control.OriginAlign = align;
        control.Position = panel.Size * new Vector2((float) ((double) x * 2.0 - 1.0), (float) ((double) y * 2.0 - 1.0)) * new Vector2(0.5f);
        panel.Controls.Add(control);
      }
    }
  }
}
