// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.GUI.MyGuiScreenMotD
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.GUI
{
  internal class MyGuiScreenMotD : MyGuiScreenBase
  {
    private StringBuilder m_message;
    private MyGuiControlLabel m_caption;
    private MyGuiControlMultilineText m_messageMultiline;
    private MyGuiControlButton m_continueButton;

    public StringBuilder MessageOfTheDay
    {
      get => this.m_message;
      private set => this.m_message = value;
    }

    public MyGuiScreenMotD(StringBuilder message)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.8f, 0.8f)), true)
    {
      this.MessageOfTheDay = message;
      this.RecreateControls(true);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenMotD);

    public override void RecreateControls(bool constructor)
    {
      this.m_caption = this.AddCaption(MyTexts.GetString(MyCommonTexts.MotD_Caption));
      MyGuiControlMultilineText controlMultilineText = new MyGuiControlMultilineText();
      controlMultilineText.Position = new Vector2(0.0f, -0.3f);
      controlMultilineText.Size = new Vector2(0.7f, 0.6f);
      controlMultilineText.Font = "Blue";
      controlMultilineText.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
      controlMultilineText.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlMultilineText.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_messageMultiline = controlMultilineText;
      this.m_messageMultiline.Text = MyTexts.SubstituteTexts(this.MessageOfTheDay);
      this.Controls.Add((MyGuiControlBase) this.m_messageMultiline);
      this.m_continueButton = new MyGuiControlButton(new Vector2?(new Vector2(0.0f, 0.35f)), text: MyTexts.Get(MyCommonTexts.MotD_Button), onButtonClick: new Action<MyGuiControlButton>(this.onContinueClick));
      this.Controls.Add((MyGuiControlBase) this.m_continueButton);
    }

    private void onContinueClick(MyGuiControlButton sender) => this.CloseScreen();
  }
}
