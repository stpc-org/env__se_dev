// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenWorldGeneratorSettings
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenWorldGeneratorSettings : MyGuiScreenBase
  {
    private MyGuiScreenWorldSettings m_parent;
    private int? m_asteroidAmount;
    private MyGuiControlButton m_okButton;
    private MyGuiControlButton m_cancelButton;
    private MyGuiControlCombobox m_asteroidAmountCombo;
    private MyGuiControlLabel m_asteroidAmountLabel;

    public int AsteroidAmount
    {
      get => !this.m_asteroidAmount.HasValue ? -1 : this.m_asteroidAmount.Value;
      set
      {
        this.m_asteroidAmount = new int?(value);
        switch (value)
        {
          case -4:
            this.m_asteroidAmountCombo.SelectItemByKey(-4L);
            break;
          case -3:
            this.m_asteroidAmountCombo.SelectItemByKey(-3L);
            break;
          case -2:
            this.m_asteroidAmountCombo.SelectItemByKey(-2L);
            break;
          case -1:
            this.m_asteroidAmountCombo.SelectItemByKey(-1L);
            break;
          case 0:
            this.m_asteroidAmountCombo.SelectItemByKey(0L);
            break;
          case 4:
            this.m_asteroidAmountCombo.SelectItemByKey(4L);
            break;
          case 7:
            this.m_asteroidAmountCombo.SelectItemByKey(7L);
            break;
          case 16:
            this.m_asteroidAmountCombo.SelectItemByKey(16L);
            break;
        }
      }
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenWorldGeneratorSettings);

    public static Vector2 CalcSize() => new Vector2(0.65f, 0.3f);

    public MyGuiScreenWorldGeneratorSettings(MyGuiScreenWorldSettings parent)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(MyGuiScreenWorldGeneratorSettings.CalcSize()), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      this.m_parent = parent;
      this.RecreateControls(true);
      this.SetSettingsToControls();
    }

    public event Action OnOkButtonClicked;

    public void GetSettings(MyObjectBuilder_SessionSettings output)
    {
    }

    protected virtual void SetSettingsToControls()
    {
    }

    public override void RecreateControls(bool constructor)
    {
      float x1 = 0.309375f;
      base.RecreateControls(constructor);
      this.AddCaption(MySpaceTexts.ScreenCaptionWorldGeneratorSettings, new Vector4?(), new Vector2?(), 0.8f);
      this.m_asteroidAmountCombo = new MyGuiControlCombobox(size: new Vector2?(new Vector2(x1, 0.04f)));
      this.m_asteroidAmountCombo.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.m_asteroidAmountCombo_ItemSelected);
      this.m_asteroidAmountCombo.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsAsteroidAmount));
      if (MyFakes.ENABLE_ASTEROID_FIELDS)
      {
        this.m_asteroidAmountCombo.AddItem(-4L, MySpaceTexts.WorldSettings_AsteroidAmountProceduralNone);
        this.m_asteroidAmountCombo.AddItem(-1L, MySpaceTexts.WorldSettings_AsteroidAmountProceduralLow);
        this.m_asteroidAmountCombo.AddItem(-2L, MySpaceTexts.WorldSettings_AsteroidAmountProceduralNormal);
        if (Environment.Is64BitProcess)
          this.m_asteroidAmountCombo.AddItem(-3L, MySpaceTexts.WorldSettings_AsteroidAmountProceduralHigh);
      }
      this.m_asteroidAmountLabel = this.MakeLabel(MySpaceTexts.Asteroid_Amount);
      this.Controls.Add((MyGuiControlBase) this.m_asteroidAmountLabel);
      this.Controls.Add((MyGuiControlBase) this.m_asteroidAmountCombo);
      int num = 0;
      float y = 0.12f;
      float x2 = 0.055f;
      float x3 = 0.25f;
      Vector2 vector2_1 = new Vector2(0.0f, 0.052f);
      Vector2 vector2_2 = -this.m_size.Value / 2f + new Vector2(x2, y);
      Vector2 vector2_3 = vector2_2 + new Vector2(x3, 0.0f);
      foreach (MyGuiControlBase control in this.Controls)
      {
        control.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
        control.Position = !(control is MyGuiControlLabel) ? vector2_3 + vector2_1 * (float) num++ : vector2_2 + vector2_1 * (float) num;
      }
      Vector2 vector2_4 = this.m_size.Value / 2f - new Vector2(0.23f, 0.03f);
      this.m_okButton = new MyGuiControlButton(new Vector2?(vector2_4 - new Vector2(0.01f, 0.0f)), size: new Vector2?(MyGuiConstants.BACK_BUTTON_SIZE), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, text: MyTexts.Get(MyCommonTexts.Ok), onButtonClick: new Action<MyGuiControlButton>(this.OkButtonClicked));
      this.m_cancelButton = new MyGuiControlButton(new Vector2?(vector2_4 + new Vector2(0.01f, 0.0f)), size: new Vector2?(MyGuiConstants.BACK_BUTTON_SIZE), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM, text: MyTexts.Get(MyCommonTexts.Cancel), onButtonClick: new Action<MyGuiControlButton>(this.CancelButtonClicked));
      this.Controls.Add((MyGuiControlBase) this.m_okButton);
      this.Controls.Add((MyGuiControlBase) this.m_cancelButton);
    }

    private void grassDensitySlider_ValueChanged(MyGuiControlSlider slider)
    {
      MyRenderProxy.Settings.User.GrassDensityFactor = slider.Value;
      MyRenderProxy.SetSettingsDirty();
    }

    private void m_asteroidAmountCombo_ItemSelected() => this.m_asteroidAmount = new int?((int) this.m_asteroidAmountCombo.GetSelectedKey());

    private MyGuiControlLabel MakeLabel(MyStringId textEnum) => new MyGuiControlLabel(text: MyTexts.GetString(textEnum));

    private MyGuiScreenWorldGeneratorSettings.MyFloraDensityEnum FloraDensityEnumKey(
      int floraDensity)
    {
      return Enum.IsDefined(typeof (MyGuiScreenWorldGeneratorSettings.MyFloraDensityEnum), (object) (MyGuiScreenWorldGeneratorSettings.MyFloraDensityEnum) floraDensity) ? (MyGuiScreenWorldGeneratorSettings.MyFloraDensityEnum) floraDensity : MyGuiScreenWorldGeneratorSettings.MyFloraDensityEnum.LOW;
    }

    private void CancelButtonClicked(object sender) => this.CloseScreen();

    private void OkButtonClicked(object sender)
    {
      if (this.OnOkButtonClicked != null)
        this.OnOkButtonClicked();
      this.CloseScreen();
    }

    protected new MyGuiControlLabel AddCaption(
      MyStringId textEnum,
      Vector4? captionTextColor = null,
      Vector2? captionOffset = null,
      float captionScale = 0.8f)
    {
      return this.AddCaption(MyTexts.GetString(textEnum), captionTextColor, captionOffset, captionScale);
    }

    public enum AsteroidAmountEnum
    {
      ProceduralNone = -4, // 0xFFFFFFFC
      ProceduralHigh = -3, // 0xFFFFFFFD
      ProceduralNormal = -2, // 0xFFFFFFFE
      ProceduralLow = -1, // 0xFFFFFFFF
      None = 0,
      Normal = 4,
      More = 7,
      Many = 16, // 0x00000010
    }

    public enum MyFloraDensityEnum
    {
      NONE = 0,
      LOW = 10, // 0x0000000A
      MEDIUM = 20, // 0x00000014
      HIGH = 30, // 0x0000001E
      EXTREME = 40, // 0x00000028
    }
  }
}
