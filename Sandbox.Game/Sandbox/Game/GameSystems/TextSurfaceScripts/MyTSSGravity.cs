// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.TextSurfaceScripts.MyTSSGravity
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;
using Sandbox.Graphics;
using System;
using System.Text;
using VRage;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Sandbox.Game.GameSystems.TextSurfaceScripts
{
  [MyTextSurfaceScript("TSS_Gravity", "DisplayName_TSS_Gravity")]
  public class MyTSSGravity : MyTSSCommon
  {
    public static float ASPECT_RATIO = 3f;
    public static float DECORATION_RATIO = 0.25f;
    public static float TEXT_RATIO = 0.25f;
    private Vector2 m_innerSize;
    private Vector2 m_decorationSize;
    private float m_firstLine;
    private float m_secondLine;
    private StringBuilder m_sb = new StringBuilder();

    public override ScriptUpdate NeedsUpdate => ScriptUpdate.Update10;

    public MyTSSGravity(Sandbox.ModAPI.IMyTextSurface surface, VRage.Game.ModAPI.IMyCubeBlock block, Vector2 size)
      : base((Sandbox.ModAPI.Ingame.IMyTextSurface) surface, (VRage.Game.ModAPI.Ingame.IMyCubeBlock) block, size)
    {
      this.m_innerSize = new Vector2(MyTSSGravity.ASPECT_RATIO, 1f);
      MyTextSurfaceScriptBase.FitRect(surface.SurfaceSize, ref this.m_innerSize);
      this.m_decorationSize = new Vector2(0.012f * this.m_innerSize.X, MyTSSGravity.DECORATION_RATIO * this.m_innerSize.Y);
      this.m_sb.Clear();
      this.m_sb.Append((object) MyTexts.Get(MySpaceTexts.AGravity));
      this.m_sb.Append(": 00.00g");
      Vector2 vector2 = MyGuiManager.MeasureStringRaw("Monospace", this.m_sb, 1f);
      float val2 = MyTSSGravity.TEXT_RATIO * this.m_innerSize.Y / vector2.Y;
      this.m_fontScale = Math.Min(this.m_innerSize.X * 0.72f / vector2.X, val2);
      this.m_firstLine = this.m_halfSize.Y - this.m_decorationSize.Y * 0.55f;
      this.m_secondLine = this.m_halfSize.Y + this.m_decorationSize.Y * 0.55f;
    }

    public override void Run()
    {
      base.Run();
      using (MySpriteDrawFrame frame = this.m_surface.DrawFrame())
      {
        this.AddBackground(frame, new Color?(new Color(this.m_backgroundColor, 0.66f)));
        if (this.m_block == null)
          return;
        Vector3D position = this.m_block.GetPosition();
        float num1 = MyGravityProviderSystem.CalculateArtificialGravityInPoint(position, MyGravityProviderSystem.CalculateArtificialGravityStrengthMultiplier(MyGravityProviderSystem.CalculateHighestNaturalGravityMultiplierInPoint(position))).Length() / 9.81f;
        this.m_sb.Clear();
        this.m_sb.Append((object) MyTexts.Get(MySpaceTexts.AGravity));
        this.m_sb.AppendFormat(": {0:F2}g", (object) num1);
        Vector2 vector2_1 = MyGuiManager.MeasureStringRaw(this.m_fontId, this.m_sb, this.m_fontScale);
        MySprite sprite1 = new MySprite()
        {
          Position = new Vector2?(new Vector2(this.m_halfSize.X, this.m_firstLine - vector2_1.Y * 0.5f)),
          Size = new Vector2?(new Vector2(this.m_innerSize.X, this.m_innerSize.Y)),
          Type = SpriteType.TEXT,
          FontId = this.m_fontId,
          Alignment = TextAlignment.CENTER,
          Color = new Color?(this.m_foregroundColor),
          RotationOrScale = this.m_fontScale,
          Data = this.m_sb.ToString()
        };
        frame.Add(sprite1);
        float num2 = MyGravityProviderSystem.CalculateNaturalGravityInPoint(position).Length() / 9.81f;
        this.m_sb.Clear();
        this.m_sb.Append((object) MyTexts.Get(MySpaceTexts.PGravity));
        this.m_sb.AppendFormat(": {0:F2}g", (object) num2);
        Vector2 vector2_2 = MyGuiManager.MeasureStringRaw(this.m_fontId, this.m_sb, this.m_fontScale);
        MySprite sprite2 = new MySprite()
        {
          Position = new Vector2?(new Vector2(this.m_halfSize.X, this.m_secondLine - vector2_2.Y * 0.5f)),
          Size = new Vector2?(new Vector2(this.m_innerSize.X, this.m_innerSize.Y)),
          Type = SpriteType.TEXT,
          FontId = this.m_fontId,
          Alignment = TextAlignment.CENTER,
          Color = new Color?(this.m_foregroundColor),
          RotationOrScale = this.m_fontScale,
          Data = this.m_sb.ToString()
        };
        frame.Add(sprite2);
        float scale = (float) ((double) this.m_innerSize.Y / 256.0 * 0.899999976158142);
        float offsetX = (float) (((double) this.m_size.X - (double) this.m_innerSize.X) / 2.0);
        this.AddBrackets(frame, new Vector2(64f, 256f), scale, offsetX);
      }
    }

    public override void Dispose() => base.Dispose();
  }
}
