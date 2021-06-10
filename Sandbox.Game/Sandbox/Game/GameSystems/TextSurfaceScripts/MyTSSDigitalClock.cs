// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.TextSurfaceScripts.MyTSSDigitalClock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics;
using System;
using System.Text;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Sandbox.Game.GameSystems.TextSurfaceScripts
{
  [MyTextSurfaceScript("TSS_ClockDigital", "DisplayName_TSS_ClockDigital")]
  public class MyTSSDigitalClock : MyTSSCommon
  {
    public static float ASPECT_RATIO = 2.5f;
    public static float DECORATION_RATIO = 0.25f;
    public static float TEXT_RATIO = 0.25f;
    private Vector2 m_innerSize;
    private Vector2 m_decorationSize;
    private StringBuilder m_sb = new StringBuilder();

    public override ScriptUpdate NeedsUpdate => ScriptUpdate.Update10;

    public MyTSSDigitalClock(Sandbox.ModAPI.IMyTextSurface surface, VRage.Game.ModAPI.IMyCubeBlock block, Vector2 size)
      : base((Sandbox.ModAPI.Ingame.IMyTextSurface) surface, (VRage.Game.ModAPI.Ingame.IMyCubeBlock) block, size)
    {
      this.m_innerSize = new Vector2(MyTSSDigitalClock.ASPECT_RATIO, 1f);
      MyTextSurfaceScriptBase.FitRect(surface.SurfaceSize, ref this.m_innerSize);
      this.m_decorationSize = new Vector2(0.012f * this.m_innerSize.X, MyTSSDigitalClock.DECORATION_RATIO * this.m_innerSize.Y);
      this.m_sb.Clear();
      this.m_sb.Append("M");
      Vector2 vector2 = MyGuiManager.MeasureStringRaw(this.m_fontId, this.m_sb, 1f);
      this.m_fontScale = MyTSSDigitalClock.TEXT_RATIO * this.m_innerSize.Y / vector2.Y;
    }

    public override void Run()
    {
      base.Run();
      using (MySpriteDrawFrame frame = this.m_surface.DrawFrame())
      {
        this.AddBackground(frame, new Color?(new Color(this.m_backgroundColor, 0.66f)));
        DateTime dateTime = DateTime.Now;
        dateTime = dateTime.ToLocalTime();
        string str = dateTime.ToString("HH:mm:ss");
        Vector2 vector2 = MyGuiManager.MeasureStringRaw(this.m_fontId, new StringBuilder(str), this.m_fontScale);
        MySprite sprite = new MySprite()
        {
          Position = new Vector2?(new Vector2(this.m_halfSize.X, this.m_halfSize.Y - vector2.Y * 0.5f)),
          Size = new Vector2?(new Vector2(this.m_innerSize.X, this.m_innerSize.Y)),
          Type = SpriteType.TEXT,
          FontId = this.m_fontId,
          Alignment = TextAlignment.CENTER,
          Color = new Color?(this.m_foregroundColor),
          RotationOrScale = this.m_fontScale,
          Data = str
        };
        frame.Add(sprite);
        float scale = (float) ((double) this.m_innerSize.Y / 256.0 * 0.899999976158142);
        float offsetX = (float) (((double) this.m_size.X - (double) this.m_innerSize.X) / 2.0);
        this.AddBrackets(frame, new Vector2(64f, 256f), scale, offsetX);
      }
    }
  }
}
