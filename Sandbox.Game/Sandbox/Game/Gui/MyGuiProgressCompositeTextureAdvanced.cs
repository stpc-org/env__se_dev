// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyGuiProgressCompositeTextureAdvanced
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GUI
{
  public class MyGuiProgressCompositeTextureAdvanced : MyGuiProgressCompositeTexture
  {
    private float[] phasesThresholds;

    public MyGuiProgressCompositeTextureAdvanced(MyGuiCompositeTexture texture)
    {
      this.LeftBottom = texture.LeftBottom;
      this.LeftCenter = texture.LeftCenter;
      this.LeftTop = texture.LeftTop;
      this.CenterBottom = texture.CenterBottom;
      this.Center = texture.Center;
      this.CenterTop = texture.CenterTop;
      this.RightBottom = texture.RightBottom;
      this.RightCenter = texture.RightCenter;
      this.RightTop = texture.RightTop;
    }

    public override void Draw(float progression, Color colorMask)
    {
      if (this.m_positionsAndSizesDirty)
        this.RefreshPositionsAndSizes();
      progression = MyMath.Clamp(progression, 0.0f, 1f);
      int index1 = 0;
      if ((double) progression <= (double) this.phasesThresholds[0])
        index1 = 1;
      if ((double) progression <= (double) this.phasesThresholds[1])
        index1 = 2;
      progression = (float) (((double) progression - (double) this.phasesThresholds[index1]) / ((index1 == 0 ? 1.0 : (double) this.phasesThresholds[index1 - 1]) - (double) this.phasesThresholds[index1]));
      Vector2 progress = Vector2.One;
      bool flag = false;
      switch (this.Orientation)
      {
        case MyGuiProgressCompositeTexture.BarOrientation.HORIZONTAL:
          progress = new Vector2(progression, 1f);
          flag = false;
          break;
        case MyGuiProgressCompositeTexture.BarOrientation.VERTICAL:
          progress = new Vector2(1f, progression);
          flag = true;
          break;
      }
      RectangleF rectangleF = new RectangleF();
      Rectangle? source = new Rectangle?(new Rectangle());
      for (int index2 = 0; index2 < 3; ++index2)
      {
        for (int index3 = 0; index3 < 3; ++index3)
        {
          int num = flag ? index2 : index3;
          if (this.IsInverted)
          {
            if ((index1 < 1 || num != 0) && (index1 < 2 || num != 1))
            {
              MyGuiProgressCompositeTexture.TextureData texture = this.m_textures[index2, index3];
              if (!string.IsNullOrEmpty(texture.Texture.Texture))
              {
                if (index1 == 0 && num == 0)
                {
                  this.SetTarget(ref rectangleF, ref source, texture, progress);
                  MyRenderProxy.DrawSprite(texture.Texture.Texture, ref rectangleF, source, colorMask, 0.0f, false, true);
                }
                else if (index1 == 1 && num == 1)
                {
                  this.SetTarget(ref rectangleF, ref source, texture, progress);
                  MyRenderProxy.DrawSprite(texture.Texture.Texture, ref rectangleF, source, colorMask, 0.0f, false, true);
                }
                else if (index1 == 2 && num == 2)
                {
                  this.SetTarget(ref rectangleF, ref source, texture, progress);
                  MyRenderProxy.DrawSprite(texture.Texture.Texture, ref rectangleF, source, colorMask, 0.0f, false, true);
                }
                else
                {
                  this.SetTarget(ref rectangleF, ref source, texture, Vector2.One);
                  MyRenderProxy.DrawSprite(texture.Texture.Texture, ref rectangleF, source, colorMask, 0.0f, false, true);
                }
              }
            }
          }
          else if ((index1 < 1 || num != 2) && (index1 < 2 || num != 1))
          {
            MyGuiProgressCompositeTexture.TextureData texture = this.m_textures[index2, index3];
            if (!string.IsNullOrEmpty(texture.Texture.Texture))
            {
              if (index1 == 0 && num == 2)
              {
                this.SetTarget(ref rectangleF, ref source, texture, progress);
                MyRenderProxy.DrawSprite(texture.Texture.Texture, ref rectangleF, source, colorMask, 0.0f, false, true);
              }
              else if (index1 == 1 && num == 1)
              {
                this.SetTarget(ref rectangleF, ref source, texture, progress);
                MyRenderProxy.DrawSprite(texture.Texture.Texture, ref rectangleF, source, colorMask, 0.0f, false, true);
              }
              else if (index1 == 2 && num == 0)
              {
                this.SetTarget(ref rectangleF, ref source, texture, progress);
                MyRenderProxy.DrawSprite(texture.Texture.Texture, ref rectangleF, source, colorMask, 0.0f, false, true);
              }
              else
              {
                this.SetTarget(ref rectangleF, ref source, texture, Vector2.One);
                MyRenderProxy.DrawSprite(texture.Texture.Texture, ref rectangleF, source, colorMask, 0.0f, false, true);
              }
            }
          }
        }
      }
    }

    protected void SetTarget(
      ref RectangleF dest,
      ref Rectangle? source,
      MyGuiProgressCompositeTexture.TextureData texData,
      Vector2 progress)
    {
      if (this.IsInverted)
      {
        dest.X = (float) (texData.Position.X + (int) ((double) texData.Size.X * (1.0 - (double) progress.X) + 0.5));
        dest.Y = (float) (texData.Position.Y + (int) ((double) texData.Size.Y * (1.0 - (double) progress.Y) + 0.5));
        dest.Width = (float) (int) ((double) texData.Size.X * (double) progress.X + 0.5);
        dest.Height = (float) (int) ((double) texData.Size.Y * (double) progress.Y + 0.5);
        source = new Rectangle?(new Rectangle((int) ((double) texData.Texture.SizePx.X * (1.0 - (double) progress.X) + 0.5), (int) ((double) texData.Texture.SizePx.Y * (1.0 - (double) progress.Y) + 0.5), (int) ((double) texData.Texture.SizePx.X * (double) progress.X), (int) ((double) texData.Texture.SizePx.Y * (double) progress.Y)));
      }
      else
      {
        dest.X = (float) texData.Position.X;
        dest.Y = (float) texData.Position.Y;
        dest.Width = (float) (int) ((double) texData.Size.X * (double) progress.X);
        dest.Height = (float) (int) ((double) texData.Size.Y * (double) progress.Y);
        source = new Rectangle?(new Rectangle(0, 0, (int) ((double) texData.Texture.SizePx.X * (double) progress.X), (int) ((double) texData.Texture.SizePx.Y * (double) progress.Y)));
      }
    }

    protected override void RefreshPositionsAndSizes()
    {
      this.m_textures[0, 0].Position = this.m_position;
      for (int index1 = 0; index1 < 3; ++index1)
      {
        for (int index2 = 0; index2 < 3; ++index2)
          this.m_textures[index1, index2].Size = (Vector2I) MyGuiManager.GetScreenSizeFromNormalizedSize(this.m_textures[index1, index2].Texture.SizeGui);
      }
      Vector2I vector2I = this.m_size - this.m_textures[0, 0].Size - this.m_textures[2, 2].Size;
      this.m_textures[1, 0].Size.Y = vector2I.Y;
      this.m_textures[1, 2].Size.Y = vector2I.Y;
      this.m_textures[0, 1].Size.X = vector2I.X;
      this.m_textures[2, 1].Size.X = vector2I.X;
      this.m_textures[1, 1].Size = vector2I;
      for (int index1 = 0; index1 < 3; ++index1)
      {
        for (int index2 = 0; index2 < 3; ++index2)
        {
          if (index1 != 0 || index2 != 0)
          {
            int x = index2 > 0 ? this.m_textures[index1, index2 - 1].Position.X + this.m_textures[index1, index2 - 1].Size.X : this.m_textures[0, 0].Position.X;
            int y = index1 > 0 ? this.m_textures[index1 - 1, index2].Position.Y + this.m_textures[index1 - 1, index2].Size.Y : this.m_textures[0, 0].Position.Y;
            this.m_textures[index1, index2].Position = new Vector2I(x, y);
          }
        }
      }
      this.phasesThresholds = new float[3];
      if (this.IsInverted)
      {
        switch (this.Orientation)
        {
          case MyGuiProgressCompositeTexture.BarOrientation.HORIZONTAL:
            this.phasesThresholds[0] = (float) (this.m_textures[0, 2].Size.X + this.m_textures[0, 1].Size.X) / (float) this.m_size.X;
            this.phasesThresholds[1] = (float) this.m_textures[0, 2].Size.X / (float) this.m_size.X;
            this.phasesThresholds[2] = 0.0f;
            break;
          case MyGuiProgressCompositeTexture.BarOrientation.VERTICAL:
            this.phasesThresholds[0] = (float) (this.m_textures[2, 0].Size.Y + this.m_textures[1, 0].Size.Y) / (float) this.m_size.Y;
            this.phasesThresholds[1] = (float) this.m_textures[2, 0].Size.Y / (float) this.m_size.Y;
            this.phasesThresholds[2] = 0.0f;
            break;
        }
      }
      else
      {
        switch (this.Orientation)
        {
          case MyGuiProgressCompositeTexture.BarOrientation.HORIZONTAL:
            this.phasesThresholds[0] = (float) (this.m_textures[0, 0].Size.X + this.m_textures[0, 1].Size.X) / (float) this.m_size.X;
            this.phasesThresholds[1] = (float) this.m_textures[0, 0].Size.X / (float) this.m_size.X;
            this.phasesThresholds[2] = 0.0f;
            break;
          case MyGuiProgressCompositeTexture.BarOrientation.VERTICAL:
            this.phasesThresholds[0] = (float) (this.m_textures[0, 0].Size.Y + this.m_textures[1, 0].Size.Y) / (float) this.m_size.Y;
            this.phasesThresholds[1] = (float) this.m_textures[0, 0].Size.Y / (float) this.m_size.Y;
            this.phasesThresholds[2] = 0.0f;
            break;
        }
      }
      this.m_positionsAndSizesDirty = false;
    }
  }
}
