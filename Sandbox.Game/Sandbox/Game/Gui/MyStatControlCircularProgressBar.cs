// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatControlCircularProgressBar
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics.GUI;
using System;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GUI
{
  public class MyStatControlCircularProgressBar : MyStatControlBase
  {
    private static MyGameTimer TIMER = new MyGameTimer();
    private float m_progression;
    private Vector2 m_segmentOrigin;
    private Vector2 m_segmentSize;
    private Vector2 m_invScale;
    private int m_animatedSegmentIndex;
    private double m_animationTimeSwitchedSegment;
    private double m_animationTimeStarted;
    private bool m_animating;
    private readonly MyGuiSizedTexture m_backgroundTexture;
    private readonly MyGuiSizedTexture m_texture;
    private readonly MyGuiSizedTexture? m_firstTexture;
    private readonly MyGuiSizedTexture? m_lastTexture;
    private float m_textureRotationAngle;
    private float m_textureRotationOffset;

    public int NumberOfSegments { get; set; }

    public bool Animate { get; set; }

    public double SegmentAnimationMs { get; set; }

    public double AnimationDelay { get; set; }

    public bool ShowEmptySegments { get; set; }

    public Vector4 EmptySegmentColorMask { get; set; }

    public Vector4 FullSegmentColorMask { get; set; }

    public Vector4 AnimatedSegmentColorMask { get; set; }

    public float TextureRotationAngle
    {
      get => MathHelper.ToDegrees(this.m_textureRotationAngle);
      set => this.m_textureRotationAngle = MathHelper.ToRadians(value);
    }

    public float TextureRotationOffset
    {
      get => MathHelper.ToDegrees(this.m_textureRotationOffset);
      set => this.m_textureRotationOffset = MathHelper.ToRadians(value);
    }

    public Vector2 SegmentSize
    {
      get => this.m_segmentSize;
      set
      {
        this.m_segmentSize = value;
        this.RecalcScale();
      }
    }

    public Vector2 SegmentOrigin
    {
      get => this.m_segmentOrigin;
      set
      {
        this.m_segmentOrigin = value;
        this.RecalcScale();
      }
    }

    public MyStatControlCircularProgressBar(
      MyStatControls parent,
      MyObjectBuilder_GuiTexture texture,
      MyObjectBuilder_GuiTexture backgroundTexture = null,
      MyObjectBuilder_GuiTexture firstTexture = null,
      MyObjectBuilder_GuiTexture lastTexture = null)
      : base(parent)
    {
      MyGuiSizedTexture myGuiSizedTexture;
      if (backgroundTexture != null)
      {
        myGuiSizedTexture = new MyGuiSizedTexture();
        myGuiSizedTexture.Texture = backgroundTexture.Path;
        myGuiSizedTexture.SizePx = (Vector2) backgroundTexture.SizePx;
        this.m_backgroundTexture = myGuiSizedTexture;
      }
      myGuiSizedTexture = new MyGuiSizedTexture();
      myGuiSizedTexture.Texture = texture.Path;
      myGuiSizedTexture.SizePx = (Vector2) texture.SizePx;
      this.m_texture = myGuiSizedTexture;
      if (firstTexture != null)
      {
        myGuiSizedTexture = new MyGuiSizedTexture();
        myGuiSizedTexture.Texture = firstTexture.Path;
        myGuiSizedTexture.SizePx = (Vector2) firstTexture.SizePx;
        this.m_firstTexture = new MyGuiSizedTexture?(myGuiSizedTexture);
      }
      if (lastTexture != null)
      {
        myGuiSizedTexture = new MyGuiSizedTexture();
        myGuiSizedTexture.Texture = lastTexture.Path;
        myGuiSizedTexture.SizePx = (Vector2) lastTexture.SizePx;
        this.m_lastTexture = new MyGuiSizedTexture?(myGuiSizedTexture);
      }
      this.ShowEmptySegments = true;
      this.EmptySegmentColorMask = new Vector4(1f, 1f, 1f, 0.5f);
      this.FullSegmentColorMask = Vector4.One;
      this.AnimatedSegmentColorMask = new Vector4(1f, 1f, 1f, 0.8f);
      this.NumberOfSegments = 10;
      this.AnimationDelay = 2000.0;
      this.SegmentAnimationMs = 50.0;
      this.m_textureRotationAngle = 0.36f;
      this.m_segmentOrigin = new Vector2(this.m_texture.SizePx.X / 2f, this.m_texture.SizePx.Y / 2f);
    }

    private void RecalcScale() => this.m_invScale = 1f / (this.m_segmentSize / this.m_texture.SizePx);

    public override void Draw(float transitionAlpha)
    {
      Vector4 vector4_1 = Vector4.One;
      this.BlinkBehavior.UpdateBlink();
      if (this.BlinkBehavior.Blink)
      {
        transitionAlpha = this.BlinkBehavior.CurrentBlinkAlpha;
        Vector4? colorMask = this.BlinkBehavior.ColorMask;
        if (colorMask.HasValue)
        {
          colorMask = this.BlinkBehavior.ColorMask;
          vector4_1 = (Vector4) MyGuiControlBase.ApplyColorMaskModifiers(colorMask.Value, true, transitionAlpha);
        }
      }
      float num1 = this.StatCurrent / this.StatMaxValue;
      double num2 = 0.0;
      if (this.Animate)
      {
        num2 = MyStatControlCircularProgressBar.TIMER.Elapsed.Milliseconds;
        if (!this.m_animating && num2 - this.m_animationTimeStarted > this.AnimationDelay)
        {
          this.m_animating = true;
          this.m_animationTimeStarted = num2;
          this.m_animatedSegmentIndex = 0;
          this.m_animationTimeSwitchedSegment = num2;
        }
      }
      Rectangle? sourceRectangle = new Rectangle?();
      RectangleF destination = new RectangleF()
      {
        Position = this.Position + new Vector2(-this.SegmentOrigin.X, this.SegmentOrigin.Y),
        Size = this.SegmentSize
      };
      float num3 = 1f / (float) this.NumberOfSegments;
      for (int index = 0; index < this.NumberOfSegments; ++index)
      {
        Vector2 rightVector = new Vector2((float) Math.Cos((double) this.m_textureRotationAngle * (double) index + (double) this.m_textureRotationOffset), (float) Math.Sin((double) this.m_textureRotationAngle * (double) index + (double) this.m_textureRotationOffset));
        Vector4 vector4_2 = this.EmptySegmentColorMask * vector4_1;
        sourceRectangle = new Rectangle?();
        destination.Position = this.Position + new Vector2(-this.SegmentOrigin.X, this.SegmentOrigin.Y);
        destination.Size = this.SegmentSize;
        Vector2 origin = this.Position + this.Size / 2f;
        MyGuiSizedTexture texture = this.m_texture;
        if (index == 0 && this.m_firstTexture.HasValue)
          texture = this.m_firstTexture.Value;
        else if (index == this.NumberOfSegments - 1 && this.m_lastTexture.HasValue)
          texture = this.m_lastTexture.Value;
        if (this.ShowEmptySegments)
          MyRenderProxy.DrawSpriteExt(texture.Texture, ref destination, sourceRectangle, (Color) vector4_2, ref rightVector, ref origin, false, true);
        bool flag = true;
        if (this.m_animating && this.m_animatedSegmentIndex == index)
        {
          vector4_2 = this.AnimatedSegmentColorMask * vector4_1;
          if (num2 - this.m_animationTimeSwitchedSegment > this.SegmentAnimationMs)
          {
            this.m_animationTimeSwitchedSegment = num2;
            ++this.m_animatedSegmentIndex;
          }
          flag = false;
        }
        else if ((double) index < (double) num1 * (double) this.NumberOfSegments)
        {
          if ((double) num1 / ((double) (index + 1) * (double) num3) < 1.0)
          {
            float y = num1 % num3 * (float) this.NumberOfSegments;
            float num4 = 1f - y;
            sourceRectangle = new Rectangle?(new Rectangle(0, (int) ((double) num4 * (double) this.m_texture.SizePx.Y), (int) this.m_texture.SizePx.X, (int) ((double) y * (double) this.m_texture.SizePx.Y)));
            destination.Size = this.SegmentSize * new Vector2(1f, y);
            destination.Position = this.Position + new Vector2(-this.SegmentOrigin.X, this.SegmentOrigin.Y + this.SegmentSize.Y * num4);
          }
          vector4_2 = this.FullSegmentColorMask * vector4_1;
          flag = false;
        }
        if (this.m_animatedSegmentIndex >= this.NumberOfSegments)
          this.m_animating = false;
        if (!flag)
          MyRenderProxy.DrawSpriteExt(texture.Texture, ref destination, sourceRectangle, (Color) vector4_2, ref rightVector, ref origin, false, true);
      }
      if (string.IsNullOrEmpty(this.m_backgroundTexture.Texture))
        return;
      destination = new RectangleF(this.Position - this.Size / 2f, this.Size);
      MyRenderProxy.DrawSprite(this.m_texture.Texture, ref destination, sourceRectangle, Color.White, 0.0f, false, true);
    }
  }
}
