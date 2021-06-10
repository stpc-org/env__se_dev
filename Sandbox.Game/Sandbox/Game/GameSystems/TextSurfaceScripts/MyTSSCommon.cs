// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.TextSurfaceScripts.MyTSSCommon
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.ModAPI.Ingame;
using System;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace Sandbox.Game.GameSystems.TextSurfaceScripts
{
  public abstract class MyTSSCommon : MyTextSurfaceScriptBase
  {
    protected string m_fontId = "Monospace";
    protected float m_fontScale = 1f;

    protected MyTSSCommon(IMyTextSurface surface, IMyCubeBlock block, Vector2 size)
      : base(surface, block, size)
    {
    }

    public override void Run()
    {
      this.m_backgroundColor = this.m_surface.ScriptBackgroundColor;
      this.m_foregroundColor = this.m_surface.ScriptForegroundColor;
    }

    protected MySpriteDrawFrame AddBackground(
      MySpriteDrawFrame frame,
      Color? color = null)
    {
      MySprite defaultBackground = MyTextSurfaceHelper.DEFAULT_BACKGROUND;
      defaultBackground.Color = new Color?(color ?? Color.White);
      ref Vector2? local1 = ref defaultBackground.Position;
      Vector2? nullable = local1;
      Vector2 vector2 = this.m_surface.TextureSize / 2f;
      local1 = nullable.HasValue ? new Vector2?(nullable.GetValueOrDefault() + vector2) : new Vector2?();
      frame.Add(defaultBackground);
      ref Vector2? local2 = ref defaultBackground.Position;
      nullable = local2;
      Vector2 backgroundShift = MyTextSurfaceHelper.BACKGROUND_SHIFT;
      local2 = nullable.HasValue ? new Vector2?(nullable.GetValueOrDefault() + backgroundShift) : new Vector2?();
      frame.Add(defaultBackground);
      return frame;
    }

    protected MySpriteDrawFrame AddBrackets(
      MySpriteDrawFrame frame,
      Vector2 size,
      float scale,
      float offsetX = 0.0f)
    {
      ref MySpriteDrawFrame local1 = ref frame;
      MySprite mySprite = new MySprite(data: "DecorativeBracketLeft", color: new Color?(this.m_foregroundColor));
      mySprite.Position = new Vector2?(new Vector2(size.X * scale + offsetX, this.m_halfSize.Y));
      mySprite.Size = new Vector2?(size * scale);
      MySprite sprite1 = mySprite;
      local1.Add(sprite1);
      ref MySpriteDrawFrame local2 = ref frame;
      mySprite = new MySprite(data: "DecorativeBracketRight", color: new Color?(this.m_foregroundColor));
      mySprite.Position = new Vector2?(new Vector2(this.m_size.X - size.X * scale - offsetX, this.m_halfSize.Y));
      mySprite.Size = new Vector2?(size * scale);
      MySprite sprite2 = mySprite;
      local2.Add(sprite2);
      return frame;
    }

    protected MySpriteDrawFrame AddProgressBar(
      MySpriteDrawFrame frame,
      Vector2 position,
      Vector2 size,
      float ratio,
      Color barBgColor,
      Color barFgColor,
      string barBgSprite = null,
      string barFgSprite = null)
    {
      MySprite mySprite = new MySprite(data: (barBgSprite ?? "SquareSimple"), color: new Color?(barBgColor));
      mySprite.Alignment = TextAlignment.LEFT;
      mySprite.Position = new Vector2?(position - new Vector2(size.X * 0.5f, 0.0f));
      mySprite.Size = new Vector2?(size);
      MySprite sprite1 = mySprite;
      frame.Add(sprite1);
      mySprite = new MySprite(data: (barFgSprite ?? "SquareSimple"), color: new Color?(barFgColor));
      mySprite.Alignment = TextAlignment.LEFT;
      mySprite.Position = new Vector2?(position - new Vector2(size.X * 0.5f, 0.0f));
      mySprite.Size = new Vector2?(new Vector2(size.X * ratio, size.Y));
      MySprite sprite2 = mySprite;
      frame.Add(sprite2);
      return frame;
    }

    protected MySpriteDrawFrame AddTextBox(
      MySpriteDrawFrame frame,
      Vector2 position,
      Vector2 size,
      string text,
      string font,
      float scale,
      Color bgColor,
      Color textColor,
      string bgSprite = null,
      float textOffset = 0.0f)
    {
      Vector2 position1 = position + new Vector2(size.X * 0.5f, 0.0f);
      if (!string.IsNullOrEmpty(bgSprite))
      {
        MySprite sprite = MySprite.CreateSprite(bgSprite, position1, size);
        sprite.Color = new Color?(bgColor);
        sprite.Alignment = TextAlignment.RIGHT;
        frame.Add(sprite);
      }
      MySprite text1 = MySprite.CreateText(text, font, textColor, scale, TextAlignment.RIGHT);
      text1.Position = new Vector2?(position1 + new Vector2(-textOffset, (float) (-(double) size.Y * 0.5)));
      text1.Size = new Vector2?(size);
      frame.Add(text1);
      return frame;
    }

    protected MySpriteDrawFrame AddLine(
      MySpriteDrawFrame frame,
      Vector2 startPos,
      Vector2 endPos,
      Color color,
      float thicknessPx)
    {
      Vector2 vector2_1 = endPos - startPos;
      Vector2 size = new Vector2(thicknessPx, vector2_1.Length());
      Vector2 vector2_2 = Vector2.Normalize(vector2_1);
      float num1 = Vector2.Dot(vector2_2, Vector2.UnitX);
      if ((double) vector2_2.Y > 0.0)
        num1 = -num1;
      float num2 = (float) (-Math.Acos((double) num1) + 1.57079601287842);
      MySprite sprite = MySprite.CreateSprite("SquareTapered", startPos + vector2_1 * 0.5f, size);
      sprite.Color = new Color?(color);
      sprite.RotationOrScale = num2;
      frame.Add(sprite);
      return frame;
    }
  }
}
