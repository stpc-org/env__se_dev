// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenNoGameItemDrop
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenNoGameItemDrop : MyGuiScreenBase
  {
    public MyGuiScreenNoGameItemDrop()
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.41f, 0.4f)), true)
    {
      this.EnabledBackgroundFade = true;
      this.CloseButtonEnabled = true;
      this.RecreateControls(true);
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.AddCaption(MyCommonTexts.ScreenCaptionClaimGameItem, captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.740000009536743 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.74f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList);
      MyGuiControlImage myGuiControlImage = new MyGuiControlImage(new Vector2?(new Vector2(-0.15f, -0.107f)), new Vector2?(new Vector2(0.3f, 0.17f)), textures: new string[1]
      {
        "Textures\\GUI\\ClaimItem.png"
      }, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      myGuiControlImage.BorderEnabled = true;
      myGuiControlImage.BorderSize = 2;
      myGuiControlImage.BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f);
      this.Controls.Add((MyGuiControlBase) myGuiControlImage);
      this.Elements.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(0.0f, 0.085f)), text: MyTexts.GetString(MyCommonTexts.ScreenNoGameItemText), colorMask: new Vector4?(Vector4.One), originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER)
      {
        Font = "White"
      });
      this.Controls.Add((MyGuiControlBase) new MyGuiControlButton(new Vector2?(new Vector2(0.0f, 0.168f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, text: MyTexts.Get(MyCommonTexts.Close), onButtonClick: new Action<MyGuiControlButton>(this.OnClaimButtonClick)));
    }

    private void OnClaimButtonClick(MyGuiControlButton obj) => this.CloseScreen();

    protected override void OnClosed()
    {
      base.OnClosed();
      MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) null;
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenNoGameItemDrop);
  }
}
