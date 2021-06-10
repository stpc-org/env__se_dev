// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenCongratulation
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
  internal class MyGuiScreenCongratulation : MyGuiScreenBase
  {
    private int m_messageId;

    public MyGuiScreenCongratulation(int messageId)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.7f, 0.54f)))
    {
      this.m_messageId = messageId;
      this.RecreateControls(true);
      if (MyAudio.Static == null)
        return;
      MyAudio.Static.PlaySound(MySoundPair.GetCueId("ArcNewItemImpact"));
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenCongratulation);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      Vector2? position1 = this.Size;
      Vector2 vector2_1 = position1 ?? new Vector2(1.2f, 0.5f);
      Vector2 vector2_2 = new Vector2(0.0f, -0.22f);
      string str1 = MyTexts.GetString(MyCommonTexts.Campaign_Congratulation_Caption);
      Vector2? position2 = new Vector2?(vector2_2);
      position1 = new Vector2?();
      Vector2? size = position1;
      string text = str1;
      Vector4? colorMask = new Vector4?();
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(position2, size, text, colorMask, 1.5f, originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
      MyGuiControlMultilineText controlMultilineText = new MyGuiControlMultilineText(new Vector2?(Vector2.Zero), new Vector2?(vector2_1), new Vector4?(Color.White.ToVector4()), "White", textAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
      this.Controls.Add((MyGuiControlBase) controlMultilineText);
      this.Controls.Add((MyGuiControlBase) new MyGuiControlButton(new Vector2?(new Vector2(0.0f, 0.22f)), size: new Vector2?(MyGuiConstants.BACK_BUTTON_SIZE), text: MyTexts.Get(MyCommonTexts.Ok), onButtonClick: new Action<MyGuiControlButton>(this.OkButtonClicked)));
      string empty = string.Empty;
      int messageId = this.m_messageId;
      string str2 = MyTexts.GetString(MyCommonTexts.Campaign_Congratulation_Text);
      string str3 = "Textures\\GUI\\PromotedEngineer.dds";
      position1 = new Vector2?();
      MyGuiControlImage myGuiControlImage = new MyGuiControlImage(position1, new Vector2?(new Vector2(0.12f, 0.16f)), textures: new string[1]
      {
        str3
      });
      myGuiControlImage.Position = new Vector2(0.0f, -0.03f);
      this.Controls.Add((MyGuiControlBase) myGuiControlImage);
      controlMultilineText.Text = new StringBuilder(str2);
    }

    private void OkButtonClicked(MyGuiControlButton button) => this.CloseScreen();
  }
}
