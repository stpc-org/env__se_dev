// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenEditorError
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyGuiScreenEditorError : MyGuiScreenBase
  {
    protected string m_errorText = "";

    public MyGuiScreenEditorError(string errorText = null)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.6f, 0.7f)), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      this.m_errorText = errorText;
      this.CanBeHidden = false;
      this.CanHideOthers = true;
      this.m_closeOnEsc = true;
      this.EnabledBackgroundFade = true;
      this.CloseButtonEnabled = true;
      this.RecreateControls(true);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenEditorError);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.AddCaption(MySpaceTexts.ProgrammableBlock_CodeEditor_Title, captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.834999978542328 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.835f);
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.834999978542328 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.123000003397465)), this.m_size.Value.X * 0.835f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList);
      MyGuiControlCompositePanel controlCompositePanel = new MyGuiControlCompositePanel();
      controlCompositePanel.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
      controlCompositePanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      controlCompositePanel.Position = new Vector2(0.0f, -23f / 1000f);
      controlCompositePanel.Size = new Vector2(0.5f, 0.465f);
      this.Controls.Add((MyGuiControlBase) controlCompositePanel);
      this.Controls.Add((MyGuiControlBase) new MyGuiControlMultilineText(new Vector2?(new Vector2(0.005f, -0.025f)), new Vector2?(new Vector2(0.485f, 0.44f)))
      {
        Text = new StringBuilder(this.m_errorText),
        TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
        TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
      });
      this.Controls.Add((MyGuiControlBase) new MyGuiControlButton(new Vector2?(new Vector2(0.0f, 0.277f)), size: new Vector2?(MyGuiConstants.BACK_BUTTON_SIZE), text: MyTexts.Get(MyCommonTexts.Ok), onButtonClick: new Action<MyGuiControlButton>(this.OkButtonClicked)));
    }

    private void OkButtonClicked(MyGuiControlButton button) => this.CloseScreen(false);

    public override bool CloseScreen(bool isUnloading = false) => base.CloseScreen(isUnloading);
  }
}
