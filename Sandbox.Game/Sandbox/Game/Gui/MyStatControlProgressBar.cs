// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatControlProgressBar
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics.GUI;
using VRage.Game.GUI;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace Sandbox.Game.GUI
{
  public class MyStatControlProgressBar : MyStatControlBase
  {
    private readonly MyGuiProgressCompositeTexture m_progressionCompositeTexture;
    private readonly MyGuiProgressSimpleTexture m_progressionSimpleTexture;

    public bool Inverted
    {
      get
      {
        if (this.m_progressionSimpleTexture != null)
          return this.m_progressionSimpleTexture.Inverted;
        return this.m_progressionCompositeTexture != null && this.m_progressionCompositeTexture.IsInverted;
      }
      set
      {
        if (this.m_progressionSimpleTexture != null)
          this.m_progressionSimpleTexture.Inverted = value;
        if (this.m_progressionCompositeTexture == null)
          return;
        this.m_progressionCompositeTexture.IsInverted = value;
      }
    }

    public MyStatControlProgressBar(MyStatControls parent, MyObjectBuilder_CompositeTexture texture)
      : base(parent)
    {
      if (!texture.IsValid())
        return;
      this.m_progressionCompositeTexture = new MyGuiProgressCompositeTexture();
      MyObjectBuilder_GuiTexture texture1 = MyGuiTextures.Static.GetTexture(texture.LeftTop);
      this.m_progressionCompositeTexture.LeftTop = new MyGuiSizedTexture()
      {
        Texture = texture1.Path,
        SizePx = (Vector2) texture1.SizePx
      };
      MyObjectBuilder_GuiTexture texture2 = MyGuiTextures.Static.GetTexture(texture.LeftCenter);
      this.m_progressionCompositeTexture.LeftCenter = new MyGuiSizedTexture()
      {
        Texture = texture2.Path,
        SizePx = (Vector2) texture2.SizePx
      };
      MyObjectBuilder_GuiTexture texture3 = MyGuiTextures.Static.GetTexture(texture.LeftBottom);
      this.m_progressionCompositeTexture.LeftBottom = new MyGuiSizedTexture()
      {
        Texture = texture3.Path,
        SizePx = (Vector2) texture3.SizePx
      };
      MyObjectBuilder_GuiTexture texture4 = MyGuiTextures.Static.GetTexture(texture.CenterTop);
      this.m_progressionCompositeTexture.CenterTop = new MyGuiSizedTexture()
      {
        Texture = texture4.Path,
        SizePx = (Vector2) texture4.SizePx
      };
      MyObjectBuilder_GuiTexture texture5 = MyGuiTextures.Static.GetTexture(texture.Center);
      this.m_progressionCompositeTexture.Center = new MyGuiSizedTexture()
      {
        Texture = texture5.Path,
        SizePx = (Vector2) texture5.SizePx
      };
      MyObjectBuilder_GuiTexture texture6 = MyGuiTextures.Static.GetTexture(texture.CenterBottom);
      this.m_progressionCompositeTexture.CenterBottom = new MyGuiSizedTexture()
      {
        Texture = texture6.Path,
        SizePx = (Vector2) texture6.SizePx
      };
      MyObjectBuilder_GuiTexture texture7 = MyGuiTextures.Static.GetTexture(texture.RightTop);
      this.m_progressionCompositeTexture.RightTop = new MyGuiSizedTexture()
      {
        Texture = texture7.Path,
        SizePx = (Vector2) texture7.SizePx
      };
      MyObjectBuilder_GuiTexture texture8 = MyGuiTextures.Static.GetTexture(texture.RightCenter);
      this.m_progressionCompositeTexture.RightCenter = new MyGuiSizedTexture()
      {
        Texture = texture8.Path,
        SizePx = (Vector2) texture8.SizePx
      };
      MyObjectBuilder_GuiTexture texture9 = MyGuiTextures.Static.GetTexture(texture.RightBottom);
      this.m_progressionCompositeTexture.RightBottom = new MyGuiSizedTexture()
      {
        Texture = texture9.Path,
        SizePx = (Vector2) texture9.SizePx
      };
    }

    public MyStatControlProgressBar(
      MyStatControls parent,
      MyObjectBuilder_GuiTexture background,
      MyObjectBuilder_GuiTexture progressBar,
      Vector2I progressBarOffset,
      Vector4? backgroundColorMask = null,
      Vector4? progressColorMask = null)
      : base(parent)
    {
      this.m_progressionSimpleTexture = new MyGuiProgressSimpleTexture()
      {
        BackgroundTexture = background,
        ProgressBarTexture = progressBar,
        ProgressBarTextureOffset = progressBarOffset
      };
      MyGuiProgressSimpleTexture progressionSimpleTexture1 = this.m_progressionSimpleTexture;
      Vector4? nullable = backgroundColorMask;
      Vector4 vector4_1 = nullable ?? Vector4.One;
      progressionSimpleTexture1.BackgroundColorMask = vector4_1;
      MyGuiProgressSimpleTexture progressionSimpleTexture2 = this.m_progressionSimpleTexture;
      nullable = progressColorMask;
      Vector4 vector4_2 = nullable ?? Vector4.One;
      progressionSimpleTexture2.ProgressBarColorMask = vector4_2;
    }

    protected override void OnPositionChanged(Vector2 oldPosition, Vector2 newPosition)
    {
      if (this.m_progressionCompositeTexture != null)
        this.m_progressionCompositeTexture.Position = new Vector2I(newPosition);
      if (this.m_progressionSimpleTexture == null)
        return;
      this.m_progressionSimpleTexture.Position = new Vector2I(newPosition);
    }

    protected override void OnSizeChanged(Vector2 oldSize, Vector2 newSize)
    {
      if (this.m_progressionCompositeTexture != null)
        this.m_progressionCompositeTexture.Size = new Vector2I(newSize);
      if (this.m_progressionSimpleTexture == null)
        return;
      this.m_progressionSimpleTexture.Size = new Vector2I(newSize);
    }

    public override void Draw(float transitionAlpha)
    {
      float progression = 0.0f;
      if ((double) this.StatMaxValue != 0.0)
        progression = this.StatCurrent / this.StatMaxValue;
      Vector4 sourceColorMask = this.m_progressionSimpleTexture != null ? this.m_progressionSimpleTexture.ProgressBarColorMask : this.ColorMask;
      this.BlinkBehavior.UpdateBlink();
      if (this.BlinkBehavior.Blink)
      {
        transitionAlpha = this.BlinkBehavior.CurrentBlinkAlpha;
        if (this.BlinkBehavior.ColorMask.HasValue)
          sourceColorMask = this.BlinkBehavior.ColorMask.Value;
      }
      if (this.m_progressionCompositeTexture != null)
        this.m_progressionCompositeTexture.Draw(progression, MyGuiControlBase.ApplyColorMaskModifiers(sourceColorMask, true, transitionAlpha));
      if (this.m_progressionSimpleTexture == null)
        return;
      this.m_progressionSimpleTexture.Draw(progression, (Color) this.m_progressionSimpleTexture.BackgroundColorMask, MyGuiControlBase.ApplyColorMaskModifiers(sourceColorMask, true, transitionAlpha));
    }
  }
}
