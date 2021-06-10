// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenWelcomeScreen
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenWelcomeScreen : MyGuiScreenBase
  {
    private MyGuiControlButton m_okBtn;

    public MyGuiScreenWelcomeScreen()
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.5264286f, 0.7633588f)), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      MySandboxGame.Log.WriteLine("MyGuiScreenWelcomeScreen.ctor START");
      this.EnabledBackgroundFade = true;
      this.m_closeOnEsc = true;
      this.m_drawEvenWithoutFocus = true;
      this.CanHideOthers = true;
      this.CanBeHidden = true;
      this.m_canCloseInCloseAllScreenCalls = false;
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this.RecreateControls(true);
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.BuildControls();
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenWelcomeScreen);

    protected void BuildControls()
    {
      this.AddCaption(MyCommonTexts.ScreenCaptionWelcomeScreen, captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      int newsletterCurrentStatus = (int) MySandboxGame.Config.NewsletterCurrentStatus;
      MyGuiControlSeparatorList controlSeparatorList1 = new MyGuiControlSeparatorList();
      controlSeparatorList1.AddHorizontal(-new Vector2((float) ((double) this.m_size.Value.X * 0.779999971389771 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.79f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList1);
      MyGuiControlSeparatorList controlSeparatorList2 = new MyGuiControlSeparatorList();
      controlSeparatorList2.AddHorizontal(-new Vector2((float) ((double) this.m_size.Value.X * 0.779999971389771 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.123000003397465)), this.m_size.Value.X * 0.79f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList2);
      float num = 0.095f;
      MyGuiControlMultilineText controlMultilineText = new MyGuiControlMultilineText(new Vector2?(new Vector2(0.015f, num - 0.162f)), new Vector2?(new Vector2(0.44f, 0.45f)), textScale: 0.76f, textBoxAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      controlMultilineText.AppendText(MyTexts.GetString(MySpaceTexts.WelcomeScreen_Text1), "Blue", 0.76f, (Vector4) Color.White);
      controlMultilineText.AppendText("\n\n", "Blue", 0.76f, (Vector4) Color.White);
      controlMultilineText.AppendText(MyTexts.GetString(MySpaceTexts.WelcomeScreen_Text2), "Blue", 0.76f, (Vector4) Color.White);
      controlMultilineText.AppendText("\n\n", "Blue", 0.76f, (Vector4) Color.White);
      controlMultilineText.AppendText(string.Format(MyTexts.GetString(MySpaceTexts.WelcomeScreen_Text3), (object) MyGameService.Service.ServiceName), "Blue", 0.76f, (Vector4) Color.White);
      MyGuiControlPanel myGuiControlPanel = new MyGuiControlPanel(new Vector2?(new Vector2(-0.08f, 0.07f + num)), new Vector2?(MyGuiConstants.TEXTURE_KEEN_LOGO.MinSizeGui), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      myGuiControlPanel.BackgroundTexture = MyGuiConstants.TEXTURE_KEEN_LOGO;
      this.Controls.Add((MyGuiControlBase) myGuiControlPanel);
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel(new Vector2?(new Vector2(0.195f, 0.1f + num)), text: MyTexts.GetString(MySpaceTexts.WelcomeScreen_SignatureTitle));
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel1);
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(new Vector2?(new Vector2(0.195f, 0.125f + num)), text: MyTexts.GetString(MySpaceTexts.WelcomeScreen_Signature));
      myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      this.m_okBtn = new MyGuiControlButton(new Vector2?(new Vector2(0.0f, 0.338f)), size: new Vector2?(MyGuiConstants.BACK_BUTTON_SIZE), originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, text: MyTexts.Get(MyCommonTexts.Ok), onButtonClick: new Action<MyGuiControlButton>(this.OnCloseButtonClick));
      this.m_okBtn.Enabled = true;
      this.m_okBtn.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipNewsletter_Ok));
      this.Controls.Add((MyGuiControlBase) controlMultilineText);
      this.Controls.Add((MyGuiControlBase) this.m_okBtn);
      this.CloseButtonEnabled = true;
    }

    private void OnCloseButtonClick(object sender)
    {
      MySandboxGame.Config.WelcomScreenCurrentStatus = MyConfig.WelcomeScreenStatus.AlreadySeen;
      MySandboxGame.Config.Save();
      this.CloseScreen();
    }
  }
}
