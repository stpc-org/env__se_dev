// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.TextSurfaceScripts.MyTSSAnalogClock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.ModAPI.Ingame;
using System;
using System.Text;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace Sandbox.Game.GameSystems.TextSurfaceScripts
{
  [MyTextSurfaceScript("TSS_ClockAnalog", "DisplayName_TSS_ClockAnalog")]
  public class MyTSSAnalogClock : MyTSSCommon
  {
    public static float ASPECT_RATIO = 1.85f;
    public static float DECORATION_RATIO = 0.25f;
    public static readonly float INDICATOR_WIDTH = 0.012f;
    private static Vector2 HOURS_SIZE = new Vector2(0.32f, MyTSSAnalogClock.INDICATOR_WIDTH);
    private static Vector2 MINUTES_SIZE = new Vector2(0.42f, MyTSSAnalogClock.INDICATOR_WIDTH);
    private static Vector2 INDICATORS_SIZE = new Vector2(0.06f, MyTSSAnalogClock.INDICATOR_WIDTH);
    private Vector2 m_innerSize;
    private Vector2 m_clockSize;
    private Vector2 m_decorationSize;
    private Vector2 m_sizeModifier;
    private StringBuilder m_sb = new StringBuilder();

    public override ScriptUpdate NeedsUpdate => ScriptUpdate.Update1000;

    public MyTSSAnalogClock(IMyTextSurface surface, IMyCubeBlock block, Vector2 size)
      : base(surface, block, size)
    {
      this.m_innerSize = new Vector2(MyTSSAnalogClock.ASPECT_RATIO, 1f);
      MyTextSurfaceScriptBase.FitRect(surface.SurfaceSize, ref this.m_innerSize);
      this.m_clockSize = (double) this.m_innerSize.X > (double) this.m_innerSize.Y ? new Vector2(this.m_innerSize.Y) : new Vector2(this.m_innerSize.X);
      this.m_decorationSize = new Vector2(MyTSSAnalogClock.INDICATOR_WIDTH * this.m_innerSize.X, MyTSSAnalogClock.DECORATION_RATIO * this.m_innerSize.Y);
      this.m_sizeModifier = new Vector2(1f, 512f / this.m_clockSize.X);
    }

    public override void Run()
    {
      base.Run();
      using (MySpriteDrawFrame frame = this.m_surface.DrawFrame())
      {
        this.AddBackground(frame, new Color?(new Color(this.m_backgroundColor, 0.66f)));
        Vector2 zero = Vector2.Zero;
        Vector2 vector2_1 = new Vector2(MyTSSAnalogClock.INDICATORS_SIZE.X * 0.5f, 0.0f);
        Color foregroundColor = this.m_foregroundColor;
        for (int index = 0; index < 12; ++index)
        {
          float radians = MathHelper.ToRadians((float) (30 * index));
          Vector2 vector2_2 = new Vector2((float) Math.Cos((double) radians), (float) Math.Sin((double) radians)) * this.m_clockSize * 0.4f - vector2_1;
          ref MySpriteDrawFrame local = ref frame;
          Color? color = new Color?(foregroundColor);
          MySprite sprite = new MySprite(data: "SquareTapered", position: new Vector2?(this.m_halfSize + vector2_2), size: new Vector2?(MyTSSAnalogClock.INDICATORS_SIZE * this.m_clockSize * this.m_sizeModifier), color: color, rotation: radians);
          local.Add(sprite);
        }
        DateTime localTime = DateTime.Now.ToLocalTime();
        float radians1 = MathHelper.ToRadians((float) ((double) (30 * localTime.Hour) + 0.5 * (double) localTime.Minute - 90.0));
        Vector2 vector2_3 = new Vector2((float) Math.Cos((double) radians1), (float) Math.Sin((double) radians1)) * this.m_clockSize * 0.3f * MyTSSAnalogClock.HOURS_SIZE.X;
        ref MySpriteDrawFrame local1 = ref frame;
        Color? color1 = new Color?(foregroundColor);
        MySprite sprite1 = new MySprite(data: "SquareTapered", position: new Vector2?(this.m_halfSize + vector2_3), size: new Vector2?(MyTSSAnalogClock.HOURS_SIZE * this.m_clockSize * this.m_sizeModifier), color: color1, rotation: radians1);
        local1.Add(sprite1);
        float radians2 = MathHelper.ToRadians((float) (6 * localTime.Minute - 90));
        Vector2 vector2_4 = new Vector2((float) Math.Cos((double) radians2), (float) Math.Sin((double) radians2)) * this.m_clockSize * 0.3f * MyTSSAnalogClock.MINUTES_SIZE.X;
        ref MySpriteDrawFrame local2 = ref frame;
        Color? color2 = new Color?(foregroundColor);
        MySprite sprite2 = new MySprite(data: "SquareTapered", position: new Vector2?(this.m_halfSize + vector2_4), size: new Vector2?(MyTSSAnalogClock.MINUTES_SIZE * this.m_clockSize * this.m_sizeModifier), color: color2, rotation: radians2);
        local2.Add(sprite2);
        float scale = (float) ((double) this.m_clockSize.Y / 256.0 * 0.899999976158142);
        float offsetX = (float) (((double) this.m_size.X - (double) this.m_innerSize.X) / 2.0);
        this.AddBrackets(frame, new Vector2(64f, 256f), scale, offsetX);
      }
    }
  }
}
