// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyGuiControlRating
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
  internal class MyGuiControlRating : MyGuiControlBase
  {
    private Vector2 m_textureSize = new Vector2(32f);
    private float m_space = 8f;
    private Vector2 m_position;
    private int m_value;
    private int m_maxValue;
    public string EmptyTexture = "Textures\\GUI\\Icons\\Rating\\NoStar.png";
    public string FilledTexture = "Textures\\GUI\\Icons\\Rating\\FullStar.png";
    public string HalfFilledTexture = "Textures\\GUI\\Icons\\Rating\\HalfStar.png";

    public int MaxValue
    {
      get => this.m_maxValue;
      set
      {
        this.m_maxValue = value;
        this.RecalculateSize();
      }
    }

    public int Value
    {
      get => this.m_value;
      set => this.m_value = value;
    }

    public MyGuiControlRating(int value = 0, int maxValue = 10)
      : base()
    {
      this.m_value = value;
      this.m_maxValue = maxValue;
      this.BackgroundTexture = (MyGuiCompositeTexture) null;
      this.ColorMask = Vector4.One;
    }

    private void RecalculateSize()
    {
      Vector2 vector2 = MyGuiManager.GetHudNormalizedSizeFromPixelSize(this.m_textureSize) * new Vector2(0.75f, 1f);
      Vector2 sizeFromPixelSize = MyGuiManager.GetHudNormalizedSizeFromPixelSize(new Vector2(this.m_space * 0.75f, 0.0f));
      this.Size = new Vector2((vector2.X + sizeFromPixelSize.X) * (float) this.m_maxValue, vector2.Y);
    }

    public float GetWidth() => (float) (((double) MyGuiManager.GetHudNormalizedSizeFromPixelSize(this.m_textureSize).X * 0.75 + (double) MyGuiManager.GetHudNormalizedSizeFromPixelSize(new Vector2(this.m_space * 0.75f, 0.0f)).X) * (double) this.MaxValue / 2.0);

    public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
    {
      base.Draw(transitionAlpha, backgroundTransitionAlpha);
      if (this.MaxValue <= 0)
        return;
      Vector2 normalizedSize = MyGuiManager.GetHudNormalizedSizeFromPixelSize(this.m_textureSize) * new Vector2(0.75f, 1f);
      Vector2 sizeFromPixelSize = MyGuiManager.GetHudNormalizedSizeFromPixelSize(new Vector2(this.m_space * 0.75f, 0.0f));
      Vector2 vector2_1 = this.GetPositionAbsoluteTopLeft() + new Vector2(0.0f, (float) (((double) this.Size.Y - (double) normalizedSize.Y) / 2.0));
      Vector2 vector2_2 = new Vector2((float) (((double) normalizedSize.X + (double) sizeFromPixelSize.X) * 0.5), normalizedSize.Y);
      for (int index = 0; index < this.MaxValue; index += 2)
      {
        Vector2 normalizedCoord = vector2_1 + new Vector2(vector2_2.X * (float) index, 0.0f);
        if (index == this.Value - 1)
          MyGuiManager.DrawSpriteBatch(this.HalfFilledTexture, normalizedCoord, normalizedSize, MyGuiControlBase.ApplyColorMaskModifiers(this.ColorMask, this.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, waitTillLoaded: false);
        else if (index < this.Value)
          MyGuiManager.DrawSpriteBatch(this.FilledTexture, normalizedCoord, normalizedSize, MyGuiControlBase.ApplyColorMaskModifiers(this.ColorMask, this.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, waitTillLoaded: false);
        else
          MyGuiManager.DrawSpriteBatch(this.EmptyTexture, normalizedCoord, normalizedSize, MyGuiControlBase.ApplyColorMaskModifiers(this.ColorMask, this.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, waitTillLoaded: false);
      }
    }
  }
}
