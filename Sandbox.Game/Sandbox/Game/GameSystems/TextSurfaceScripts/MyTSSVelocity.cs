// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.TextSurfaceScripts.MyTSSVelocity
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Graphics;
using System;
using System.Text;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Sandbox.Game.GameSystems.TextSurfaceScripts
{
  [MyTextSurfaceScript("TSS_Velocity", "DisplayName_TSS_Velocity")]
  public class MyTSSVelocity : MyTSSCommon
  {
    public static float ASPECT_RATIO = 3f;
    public static float DECORATION_RATIO = 0.25f;
    public static float TEXT_RATIO = 0.25f;
    private Vector2 m_innerSize;
    private Vector2 m_decorationSize;
    private float m_firstLine;
    private float m_secondLine;
    private StringBuilder m_sb = new StringBuilder();
    private MyCubeGrid m_grid;

    public override ScriptUpdate NeedsUpdate => ScriptUpdate.Update10;

    public MyTSSVelocity(Sandbox.ModAPI.IMyTextSurface surface, VRage.Game.ModAPI.IMyCubeBlock block, Vector2 size)
      : base((Sandbox.ModAPI.Ingame.IMyTextSurface) surface, (VRage.Game.ModAPI.Ingame.IMyCubeBlock) block, size)
    {
      this.m_innerSize = new Vector2(MyTSSVelocity.ASPECT_RATIO, 1f);
      MyTextSurfaceScriptBase.FitRect(this.Surface.SurfaceSize, ref this.m_innerSize);
      this.m_decorationSize = new Vector2(0.012f * this.m_innerSize.X, MyTSSVelocity.DECORATION_RATIO * this.m_innerSize.Y);
      this.m_sb.Clear();
      this.m_sb.Append("M");
      Vector2 vector2 = MyGuiManager.MeasureStringRaw(this.m_fontId, this.m_sb, 1f);
      this.m_fontScale = MyTSSVelocity.TEXT_RATIO * this.m_innerSize.Y / vector2.Y;
      this.m_firstLine = this.m_halfSize.Y - this.m_decorationSize.Y * 0.55f;
      this.m_secondLine = this.m_halfSize.Y + this.m_decorationSize.Y * 0.55f;
      if (this.m_block == null)
        return;
      this.m_grid = this.m_block.CubeGrid as MyCubeGrid;
    }

    public override void Run()
    {
      base.Run();
      using (MySpriteDrawFrame frame = this.m_surface.DrawFrame())
      {
        this.AddBackground(frame, new Color?(new Color(this.m_backgroundColor, 0.66f)));
        if (this.m_grid == null || this.m_grid.Physics == null)
          return;
        Color barBgColor = new Color(this.m_foregroundColor, 0.1f);
        float num1 = this.m_grid.Physics.LinearVelocity.Length();
        float num2 = Math.Max(MyGridPhysics.ShipMaxLinearVelocity(), 1f);
        float ratio = num1 / num2;
        string str = string.Format("{0:F2} m/s", (object) num1);
        Vector2 vector2_1 = MyGuiManager.MeasureStringRaw(this.m_fontId, new StringBuilder(str), this.m_fontScale);
        MySprite sprite = new MySprite()
        {
          Position = new Vector2?(new Vector2(this.m_halfSize.X, this.m_firstLine - vector2_1.Y * 0.5f)),
          Size = new Vector2?(new Vector2(this.m_innerSize.X, this.m_innerSize.Y)),
          Type = SpriteType.TEXT,
          FontId = this.m_fontId,
          Alignment = TextAlignment.CENTER,
          Color = new Color?(this.m_foregroundColor),
          RotationOrScale = this.m_fontScale,
          Data = str
        };
        frame.Add(sprite);
        this.m_sb.Clear();
        this.m_sb.Append("[");
        Vector2 vector2_2 = MyGuiManager.MeasureStringRaw(this.m_fontId, this.m_sb, this.m_decorationSize.Y / MyGuiManager.MeasureStringRaw(this.m_fontId, this.m_sb, 1f).Y);
        float x = this.m_innerSize.X * 0.6f;
        this.AddProgressBar(frame, new Vector2(this.m_halfSize.X, this.m_secondLine), new Vector2(x, vector2_2.Y * 0.4f), ratio, barBgColor, this.m_foregroundColor);
        float scale = (float) ((double) this.m_innerSize.Y / 256.0 * 0.899999976158142);
        float offsetX = (float) (((double) this.m_size.X - (double) this.m_innerSize.X) / 2.0);
        this.AddBrackets(frame, new Vector2(64f, 256f), scale, offsetX);
      }
    }

    public override void Dispose()
    {
      base.Dispose();
      this.m_grid = (MyCubeGrid) null;
    }
  }
}
