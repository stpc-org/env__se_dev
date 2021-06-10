// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyGuiProgressCompositeTexture
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRageMath;

namespace Sandbox.Game.GUI
{
  public class MyGuiProgressCompositeTexture
  {
    protected readonly MyGuiProgressCompositeTexture.TextureData[,] m_textures = new MyGuiProgressCompositeTexture.TextureData[3, 3];
    protected bool m_positionsAndSizesDirty = true;
    protected Vector2I m_position = Vector2I.Zero;
    protected Vector2I m_size = Vector2I.Zero;

    public bool IsInverted { get; set; }

    public MyGuiProgressCompositeTexture.BarOrientation Orientation { get; set; }

    public MyGuiSizedTexture LeftTop
    {
      get => this.m_textures[0, 0].Texture;
      set
      {
        this.m_textures[0, 0].Texture = value;
        this.m_textures[0, 0].Size = this.ToVector2I(value.SizePx);
        this.m_positionsAndSizesDirty = true;
      }
    }

    public MyGuiSizedTexture LeftCenter
    {
      get => this.m_textures[1, 0].Texture;
      set
      {
        this.m_textures[1, 0].Texture = value;
        this.m_textures[1, 0].Size = this.ToVector2I(value.SizePx);
        this.m_positionsAndSizesDirty = true;
      }
    }

    public MyGuiSizedTexture LeftBottom
    {
      get => this.m_textures[2, 0].Texture;
      set
      {
        this.m_textures[2, 0].Texture = value;
        this.m_textures[2, 0].Size = this.ToVector2I(value.SizePx);
        this.m_positionsAndSizesDirty = true;
      }
    }

    public MyGuiSizedTexture CenterTop
    {
      get => this.m_textures[0, 1].Texture;
      set
      {
        this.m_textures[0, 1].Texture = value;
        this.m_textures[0, 1].Size = this.ToVector2I(value.SizePx);
        this.m_positionsAndSizesDirty = true;
      }
    }

    public MyGuiSizedTexture Center
    {
      get => this.m_textures[1, 1].Texture;
      set
      {
        this.m_textures[1, 1].Texture = value;
        this.m_textures[1, 1].Size = this.ToVector2I(value.SizePx);
        this.m_positionsAndSizesDirty = true;
      }
    }

    public MyGuiSizedTexture CenterBottom
    {
      get => this.m_textures[2, 1].Texture;
      set
      {
        this.m_textures[2, 1].Texture = value;
        this.m_textures[2, 1].Size = this.ToVector2I(value.SizePx);
        this.m_positionsAndSizesDirty = true;
      }
    }

    public MyGuiSizedTexture RightTop
    {
      get => this.m_textures[0, 2].Texture;
      set
      {
        this.m_textures[0, 2].Texture = value;
        this.m_textures[0, 2].Size = this.ToVector2I(value.SizePx);
        this.m_positionsAndSizesDirty = true;
      }
    }

    public MyGuiSizedTexture RightCenter
    {
      get => this.m_textures[1, 2].Texture;
      set
      {
        this.m_textures[1, 2].Texture = value;
        this.m_textures[1, 2].Size = this.ToVector2I(value.SizePx);
        this.m_positionsAndSizesDirty = true;
      }
    }

    public MyGuiSizedTexture RightBottom
    {
      get => this.m_textures[2, 2].Texture;
      set
      {
        this.m_textures[2, 2].Texture = value;
        this.m_textures[2, 2].Size = this.ToVector2I(value.SizePx);
        this.m_positionsAndSizesDirty = true;
      }
    }

    public Vector2I Position
    {
      get => this.m_position;
      set
      {
        this.m_position = value;
        this.m_positionsAndSizesDirty = true;
      }
    }

    public Vector2I Size
    {
      get => this.m_size;
      set
      {
        this.m_size = value;
        this.m_positionsAndSizesDirty = true;
      }
    }

    public virtual void Draw(float progression, Color colorMask)
    {
      if (this.m_positionsAndSizesDirty)
        this.RefreshPositionsAndSizes();
      progression = MyMath.Clamp(progression, 0.0f, 1f);
      Rectangle rect = new Rectangle();
      for (int index1 = 0; index1 < 3; ++index1)
      {
        for (int index2 = 0; index2 < 3; ++index2)
        {
          MyGuiProgressCompositeTexture.TextureData texture = this.m_textures[index1, index2];
          if (!string.IsNullOrEmpty(texture.Texture.Texture))
          {
            if (index1 == 1 && index2 == 1)
            {
              int num1;
              int num2;
              int num3;
              int num4;
              if (this.Orientation == MyGuiProgressCompositeTexture.BarOrientation.HORIZONTAL)
              {
                num1 = this.m_textures[1, 1].Size.X;
                num2 = num1;
                num3 = this.m_textures[0, 1].Size.X;
                num4 = (int) ((double) num3 * (double) progression) + 1;
              }
              else
              {
                num1 = this.m_textures[1, 1].Size.Y;
                num2 = num1;
                num3 = this.m_textures[1, 0].Size.Y;
                num4 = (int) ((double) num3 * (double) progression) + 1;
              }
              this.SetTarget(ref rect, texture);
              if (this.IsInverted)
              {
                if (this.Orientation == MyGuiProgressCompositeTexture.BarOrientation.HORIZONTAL)
                  rect.X += num3 - num1;
                else
                  rect.Y += num3 - num1;
              }
              for (; num2 < num4; num2 += num1)
              {
                MyGuiManager.DrawSprite(texture.Texture.Texture, rect, colorMask, false, true);
                if (this.Orientation == MyGuiProgressCompositeTexture.BarOrientation.HORIZONTAL)
                  rect.X = !this.IsInverted ? texture.Position.X + num2 : texture.Position.X + num3 - num2;
                else
                  rect.Y = !this.IsInverted ? texture.Position.Y + num2 : texture.Position.Y + num3 - num2;
              }
              int num5 = num2 - num4;
              int num6 = num1 - num5;
              if (num5 > 1)
              {
                if (this.Orientation == MyGuiProgressCompositeTexture.BarOrientation.HORIZONTAL)
                {
                  rect.Width = num6;
                  if (this.IsInverted)
                    rect.X += num1;
                }
                else
                {
                  rect.Height = num6;
                  if (this.IsInverted)
                    rect.Y += num1;
                }
                MyGuiManager.DrawSprite(texture.Texture.Texture, rect, colorMask, false, true);
              }
            }
            else
            {
              this.SetTarget(ref rect, texture);
              MyGuiManager.DrawSprite(texture.Texture.Texture, rect, colorMask, false, true);
            }
          }
        }
      }
    }

    protected void SetTarget(ref Rectangle rect, MyGuiProgressCompositeTexture.TextureData texData)
    {
      rect.X = texData.Position.X;
      rect.Y = texData.Position.Y;
      rect.Width = texData.Size.X;
      rect.Height = texData.Size.Y;
    }

    protected virtual void RefreshPositionsAndSizes()
    {
      this.m_textures[0, 0].Position = this.m_position;
      Vector2I vector2I = this.m_size - this.m_textures[0, 0].Size - this.m_textures[2, 2].Size;
      this.m_textures[1, 0].Size.Y = vector2I.Y;
      this.m_textures[1, 2].Size.Y = vector2I.Y;
      this.m_textures[0, 1].Size.X = vector2I.X;
      this.m_textures[2, 1].Size.X = vector2I.X;
      this.m_textures[1, 1].Size.Y = vector2I.Y;
      Vector2I size = this.m_textures[1, 1].Size;
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
      this.m_textures[1, 1].Size = size;
      this.m_positionsAndSizesDirty = false;
    }

    private Vector2I ToVector2I(Vector2 source) => new Vector2I((int) source.X, (int) source.Y);

    public enum BarOrientation
    {
      HORIZONTAL,
      VERTICAL,
    }

    protected struct TextureData
    {
      public Vector2I Position;
      public Vector2I Size;
      public MyGuiSizedTexture Texture;

      public override string ToString() => "Position: " + (object) this.Position + " Size: " + (object) this.Size;
    }
  }
}
