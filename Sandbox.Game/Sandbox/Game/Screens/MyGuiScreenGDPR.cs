// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenGDPR
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenGDPR : MyGuiScreenBase
  {
    private MyGuiControlButton m_yesBtn;
    private MyGuiControlButton m_noBtn;
    private MyGuiControlButton m_linkBtn;

    public MyGuiScreenGDPR()
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.5264286f, 0.4293893f)), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      MySandboxGame.Log.WriteLine("MyGuiScreenGDPR.ctor START");
      this.EnabledBackgroundFade = true;
      this.m_closeOnEsc = true;
      this.CloseButtonEnabled = true;
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

    public override string GetFriendlyName() => nameof (MyGuiScreenGDPR);

    protected void BuildControls()
    {
      this.AddCaption(MyTexts.GetString(MySpaceTexts.GDPR_Caption), captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      MyGuiControlSeparatorList controlSeparatorList1 = new MyGuiControlSeparatorList();
      controlSeparatorList1.AddHorizontal(-new Vector2((float) ((double) this.m_size.Value.X * 0.779999971389771 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.79f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList1);
      MyGuiControlSeparatorList controlSeparatorList2 = new MyGuiControlSeparatorList();
      controlSeparatorList2.AddHorizontal(-new Vector2((float) ((double) this.m_size.Value.X * 0.779999971389771 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.123000003397465)), this.m_size.Value.X * 0.79f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList2);
      MyGuiControlMultilineText controlMultilineText = new MyGuiControlMultilineText(new Vector2?(new Vector2(0.015f, 0.005f + 0.095f)), new Vector2?(new Vector2(0.44f, 0.45f)), textScale: 0.76f, textBoxAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      controlMultilineText.AppendText(MyTexts.GetString(MySpaceTexts.GDPR_Text1), "Blue", 0.76f, (Vector4) Color.White);
      controlMultilineText.AppendText("\n\n", "Blue", 0.76f, (Vector4) Color.White);
      controlMultilineText.AppendText(MyTexts.GetString(MySpaceTexts.GDPR_Text2), "Blue", 0.76f, (Vector4) Color.White);
      Vector2 backButtonSize = MyGuiConstants.BACK_BUTTON_SIZE;
      this.m_linkBtn = new MyGuiControlButton(new Vector2?(new Vector2(-0.0f, 0.068f)), size: new Vector2?(backButtonSize), originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, text: MyTexts.Get(MyCommonTexts.Yes), onButtonClick: new Action<MyGuiControlButton>(this.OnLinkButtonClick));
      this.m_linkBtn.Enabled = true;
      this.m_linkBtn.VisualStyle = MyGuiControlButtonStyleEnum.ClickableText;
      this.m_linkBtn.Text = MyTexts.GetString(MySpaceTexts.GDPR_PrivacyPolicy);
      this.Controls.Add((MyGuiControlBase) controlMultilineText);
      this.Controls.Add((MyGuiControlBase) this.m_linkBtn);
      this.m_yesBtn = new MyGuiControlButton(new Vector2?(new Vector2(-0.1f, 0.17f)), size: new Vector2?(backButtonSize), originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, text: MyTexts.Get(MyCommonTexts.Yes), onButtonClick: new Action<MyGuiControlButton>(this.OnYesButtonClick));
      this.m_yesBtn.Enabled = true;
      this.m_yesBtn.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipNewsletter_Ok));
      this.Controls.Add((MyGuiControlBase) this.m_yesBtn);
      this.m_noBtn = new MyGuiControlButton(new Vector2?(new Vector2(0.1f, 0.17f)), size: new Vector2?(backButtonSize), originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, text: MyTexts.Get(MyCommonTexts.No), onButtonClick: new Action<MyGuiControlButton>(this.OnNoButtonClick));
      this.m_noBtn.Enabled = true;
      this.m_noBtn.SetToolTip(MyTexts.GetString(MySpaceTexts.DetailScreen_Button_Close));
      this.Controls.Add((MyGuiControlBase) this.m_noBtn);
    }

    private void OnLinkButtonClick(object sender) => MyGuiSandbox.OpenUrl("http://mirror.keenswh.com/policy/KSWH_Privacy_Policy.pdf", UrlOpenMode.ExternalWithConfirm);

    private void OnYesButtonClick(object sender)
    {
      MySandboxGame.Config.GDPRConsent = new bool?(true);
      MySandboxGame.Config.Save();
      ConsentSenderGDPR.TrySendConsent();
      this.CloseScreen();
    }

    private void OnNoButtonClick(object sender)
    {
      MySandboxGame.Config.GDPRConsent = new bool?(false);
      MySandboxGame.Config.Save();
      ConsentSenderGDPR.TrySendConsent();
      this.CloseScreen();
    }
  }
}
