// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHudCrosshair
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Game.GUI;
using Sandbox.Game.World;
using Sandbox.Graphics;
using System.Collections.Generic;
using VRage.Game.Gui;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
  public class MyHudCrosshair
  {
    private Vector2 m_rightVector;
    private Vector2 m_position;
    private List<MyHudCrosshair.SpriteInfo> m_sprites;
    private int m_lastGameplayTimeInMs;
    protected MyObjectBuilder_CrosshairStyle m_style;
    protected MyStatControls m_statControls;
    private static MyStringId m_defaultSpriteId = MyStringId.GetOrCompute("Default");

    public Vector2 Position => this.m_position;

    public static Vector2 ScreenCenter => new Vector2(0.5f, MyGuiManager.GetHudSizeHalf().Y);

    public bool Visible { get; private set; }

    public MyHudCrosshair()
    {
      this.m_sprites = new List<MyHudCrosshair.SpriteInfo>();
      this.m_lastGameplayTimeInMs = 0;
      this.ResetToDefault();
    }

    public void ResetToDefault(bool clear = true) => this.SetDefaults(clear);

    public void HideDefaultSprite()
    {
      for (int index = 0; index < this.m_sprites.Count; ++index)
      {
        MyHudCrosshair.SpriteInfo sprite = this.m_sprites[index];
        if (!(sprite.SpriteId != MyHudCrosshair.m_defaultSpriteId))
        {
          sprite.Visible = false;
          this.m_sprites[index] = sprite;
        }
      }
    }

    public void Recenter() => this.m_position = MyHudCrosshair.ScreenCenter;

    public void ChangePosition(Vector2 newPosition) => this.m_position = newPosition;

    public void ChangeDefaultSprite(MyHudTexturesEnum newSprite, float size = 0.0f)
    {
      for (int index = 0; index < this.m_sprites.Count; ++index)
      {
        MyHudCrosshair.SpriteInfo sprite = this.m_sprites[index];
        if (!(sprite.SpriteId != MyHudCrosshair.m_defaultSpriteId))
        {
          if ((double) size != 0.0)
            sprite.HalfSize = Vector2.One * size;
          sprite.SpriteEnum = newSprite;
          this.m_sprites[index] = sprite;
        }
      }
    }

    public void AddTemporarySprite(
      MyHudTexturesEnum spriteEnum,
      MyStringId spriteId,
      int timeout = 2000,
      int fadeTime = 1000,
      Color? color = null,
      float size = 0.02f)
    {
      MyHudCrosshair.SpriteInfo spriteInfo = new MyHudCrosshair.SpriteInfo();
      spriteInfo.Color = color.HasValue ? color.Value : MyHudConstants.HUD_COLOR_LIGHT;
      spriteInfo.FadeoutTime = fadeTime;
      spriteInfo.HalfSize = Vector2.One * size;
      spriteInfo.SpriteId = spriteId;
      spriteInfo.SpriteEnum = spriteEnum;
      spriteInfo.TimeRemaining = timeout;
      spriteInfo.Visible = true;
      for (int index = 0; index < this.m_sprites.Count; ++index)
      {
        if (this.m_sprites[index].SpriteId == spriteId)
        {
          this.m_sprites[index] = spriteInfo;
          return;
        }
      }
      this.m_sprites.Add(spriteInfo);
    }

    private void SetDefaults(bool clear)
    {
      if (clear)
        this.m_sprites.Clear();
      this.CreateDefaultSprite();
      this.m_rightVector = Vector2.UnitX;
    }

    private void CreateDefaultSprite()
    {
      MyHudCrosshair.SpriteInfo spriteInfo = new MyHudCrosshair.SpriteInfo();
      spriteInfo.Color = MyHudConstants.HUD_COLOR_LIGHT;
      spriteInfo.FadeoutTime = 0;
      spriteInfo.HalfSize = Vector2.One * 0.02f;
      spriteInfo.SpriteId = MyHudCrosshair.m_defaultSpriteId;
      spriteInfo.SpriteEnum = MyHudTexturesEnum.crosshair;
      spriteInfo.TimeRemaining = 0;
      spriteInfo.Visible = true;
      bool flag = false;
      for (int index = 0; index < this.m_sprites.Count; ++index)
      {
        if (this.m_sprites[index].SpriteId == MyHudCrosshair.m_defaultSpriteId)
        {
          this.m_sprites[index] = spriteInfo;
          flag = true;
          break;
        }
      }
      if (flag)
        return;
      this.m_sprites.Add(spriteInfo);
    }

    public static bool GetTarget(Vector3D from, Vector3D to, ref Vector3D target)
    {
      MyPhysics.HitInfo? nullable = MyPhysics.CastRay(from, to, 15);
      if (nullable.HasValue)
        target = nullable.Value.Position;
      return nullable.HasValue;
    }

    public static bool GetProjectedTarget(Vector3D from, Vector3D to, ref Vector2 target)
    {
      Vector3D zero = Vector3D.Zero;
      return MyHudCrosshair.GetTarget(from, to, ref zero) && MyHudCrosshair.GetProjectedVector(zero, ref target);
    }

    public void Update()
    {
      int timeInMilliseconds = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      if (this.m_lastGameplayTimeInMs == 0)
      {
        this.m_lastGameplayTimeInMs = timeInMilliseconds;
      }
      else
      {
        int num = timeInMilliseconds - this.m_lastGameplayTimeInMs;
        this.m_lastGameplayTimeInMs = timeInMilliseconds;
        for (int index = 0; index < this.m_sprites.Count; ++index)
        {
          MyHudCrosshair.SpriteInfo sprite = this.m_sprites[index];
          if (!(sprite.SpriteId == MyHudCrosshair.m_defaultSpriteId))
          {
            sprite.TimeRemaining -= num;
            if (sprite.TimeRemaining <= 0)
            {
              this.m_sprites.RemoveAt(index);
              --index;
            }
            else
              this.m_sprites[index] = sprite;
          }
        }
      }
    }

    public void Draw(string atlas, MyAtlasTextureCoordinate[] atlasCoords)
    {
      float x = (float) MyGuiManager.GetSafeFullscreenRectangle().Width / MyGuiManager.GetHudSize().X;
      float y = (float) MyGuiManager.GetSafeFullscreenRectangle().Height / MyGuiManager.GetHudSize().Y;
      Vector2 position = this.m_position;
      if (MyVideoSettingsManager.IsTripleHead())
        ++position.X;
      foreach (MyHudCrosshair.SpriteInfo sprite in this.m_sprites)
      {
        if (sprite.Visible)
        {
          int spriteEnum = (int) sprite.SpriteEnum;
          if (spriteEnum < atlasCoords.Length)
          {
            MyAtlasTextureCoordinate atlasCoord = atlasCoords[spriteEnum];
            Color color = sprite.Color;
            if (sprite.TimeRemaining < sprite.FadeoutTime)
              color.A = (byte) ((int) color.A * sprite.TimeRemaining / sprite.FadeoutTime);
            MyRenderProxy.DrawSpriteAtlas(atlas, position, atlasCoord.Offset, atlasCoord.Size, this.m_rightVector, new Vector2(x, y), color, sprite.HalfSize, false);
          }
        }
      }
      if (this.m_statControls == null || this.m_style == null)
        return;
      Rectangle fullscreenRectangle = MyGuiManager.GetFullscreenRectangle();
      Vector2 size = new Vector2((float) fullscreenRectangle.Width, (float) fullscreenRectangle.Height);
      Vector2 coordScreen = this.m_style.Position * size;
      this.m_statControls.Position = (this.Position - MyHudCrosshair.ScreenCenter) / MyGuiManager.GetHudSize() * size + MyUtils.AlignCoord(coordScreen, size, this.m_style.OriginAlign);
      this.m_statControls.Draw(1f, 1f);
    }

    public static bool GetProjectedVector(Vector3D worldPosition, ref Vector2 target)
    {
      Vector3D vector3D = Vector3D.Transform(worldPosition, MySector.MainCamera.ViewMatrix);
      Vector4 vector4 = Vector4.Transform((Vector3) vector3D, (Matrix) ref MySector.MainCamera.ProjectionMatrix);
      if (vector3D.Z > 0.0 || (double) vector4.W == 0.0)
        return false;
      target = new Vector2((float) ((double) vector4.X / (double) vector4.W / 2.0 + 0.5), (float) (-(double) vector4.Y / (double) vector4.W / 2.0 + 0.5));
      if (MyVideoSettingsManager.IsTripleHead())
        target.X = (float) (((double) target.X - 0.333333343267441) / 0.333333343267441);
      target.Y *= MyGuiManager.GetHudSize().Y;
      return true;
    }

    public void RecreateControls(bool constructor)
    {
      this.m_style = MyHud.HudDefinition.Crosshair;
      if (this.m_style == null)
        return;
      this.InitStatControls();
    }

    private void InitStatControls()
    {
      Rectangle fullscreenRectangle = MyGuiManager.GetFullscreenRectangle();
      Vector2 size = new Vector2((float) fullscreenRectangle.Width, (float) fullscreenRectangle.Height);
      this.m_statControls = new MyStatControls((MyObjectBuilder_StatControls) this.m_style, MyGuiManager.GetSafeScreenScale() * MyHud.HudElementsScaleMultiplier);
      Vector2 coordScreen = this.m_style.Position * size;
      this.m_statControls.Position = (this.Position - MyHudCrosshair.ScreenCenter) / MyGuiManager.GetHudSize() * size + MyUtils.AlignCoord(coordScreen, size, this.m_style.OriginAlign);
    }

    private struct SpriteInfo
    {
      public MyHudTexturesEnum SpriteEnum;
      public Color Color;
      public Vector2 HalfSize;
      public MyStringId SpriteId;
      public int FadeoutTime;
      public int TimeRemaining;
      public bool Visible;
    }
  }
}
