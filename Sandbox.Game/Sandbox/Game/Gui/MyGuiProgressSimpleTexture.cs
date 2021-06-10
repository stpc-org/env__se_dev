// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyGuiProgressSimpleTexture
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GUI
{
  public class MyGuiProgressSimpleTexture
  {
    private bool m_dirty;
    private MyObjectBuilder_GuiTexture m_backgroundTexture;
    private MyObjectBuilder_GuiTexture m_progressBarTexture;
    private Vector2I m_size;
    private Vector2I m_barSize;
    private Vector2I m_progressBarTextureOffset;
    private Vector2 m_zeroOrigin = Vector2.Zero;

    public MyObjectBuilder_GuiTexture BackgroundTexture
    {
      get => this.m_backgroundTexture;
      set
      {
        this.m_backgroundTexture = value;
        this.m_dirty = true;
      }
    }

    public MyObjectBuilder_GuiTexture ProgressBarTexture
    {
      get => this.m_progressBarTexture;
      set
      {
        this.m_progressBarTexture = value;
        this.m_dirty = true;
      }
    }

    public Vector2I Size
    {
      get => this.m_size;
      set
      {
        this.m_size = value;
        this.m_dirty = true;
      }
    }

    public Vector2I ProgressBarTextureOffset
    {
      get => this.m_progressBarTextureOffset;
      set
      {
        this.m_progressBarTextureOffset = value;
        this.m_dirty = true;
      }
    }

    public Vector4 BackgroundColorMask { get; set; }

    public Vector4 ProgressBarColorMask { get; set; }

    public Vector2I Position { get; set; }

    public bool Inverted { get; set; }

    public void Draw(float progression, Color backgroundColorMask, Color progressColorMask)
    {
      Rectangle? sourceRectangle = new Rectangle?();
      RectangleF destination = new RectangleF();
      progression = MyMath.Clamp(progression, 0.0f, 1f);
      if (this.m_dirty)
        this.RecalculateInternals();
      destination.X = (float) this.Position.X;
      destination.Y = (float) this.Position.Y;
      destination.Width = (float) this.m_size.X;
      destination.Height = (float) this.m_size.Y;
      MyRenderProxy.DrawSprite(this.m_backgroundTexture.Path, ref destination, sourceRectangle, backgroundColorMask, 0.0f, false, true);
      Vector2I vector2I = this.Position + this.m_progressBarTextureOffset;
      if (this.Inverted)
      {
        destination.X = (float) vector2I.X + (float) this.m_barSize.X * (1f - progression);
        destination.Y = (float) vector2I.Y;
        destination.Width = (float) (int) ((double) this.m_barSize.X * (double) progression);
        destination.Height = (float) this.m_barSize.Y;
        sourceRectangle = new Rectangle?(new Rectangle((int) ((double) this.m_progressBarTexture.SizePx.X * (1.0 - (double) progression)), 0, (int) ((double) this.m_progressBarTexture.SizePx.X * (double) progression), this.m_progressBarTexture.SizePx.Y));
      }
      else
      {
        destination.X = (float) vector2I.X;
        destination.Y = (float) vector2I.Y;
        destination.Width = (float) (int) ((double) this.m_barSize.X * (double) progression);
        destination.Height = (float) this.m_barSize.Y;
        sourceRectangle = new Rectangle?(new Rectangle(0, 0, (int) ((double) this.m_progressBarTexture.SizePx.X * (double) progression), this.m_progressBarTexture.SizePx.Y));
      }
      MyRenderProxy.DrawSprite(this.m_progressBarTexture.Path, ref destination, sourceRectangle, progressColorMask, 0.0f, false, true);
    }

    private void RecalculateInternals()
    {
      Vector2 vector2 = new Vector2((float) this.m_size.X / (float) this.m_backgroundTexture.SizePx.X, (float) this.m_size.Y / (float) this.m_backgroundTexture.SizePx.Y);
      this.m_barSize = new Vector2I((Vector2) this.m_progressBarTexture.SizePx * vector2);
      this.m_progressBarTextureOffset = new Vector2I((Vector2) this.m_progressBarTextureOffset * vector2);
      this.m_dirty = false;
    }
  }
}
