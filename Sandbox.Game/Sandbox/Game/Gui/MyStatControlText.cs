// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatControlText
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System.Text;
using VRage;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GUI
{
  public class MyStatControlText : MyStatControlBase
  {
    private const string STAT_TAG = "{STAT}";
    private readonly string m_text;
    private readonly StringBuilder m_textTmp = new StringBuilder(128);
    private string m_precisionFormat;
    private int m_precision;
    private MyFont m_font;
    private MyStringHash m_fontHash;
    private readonly bool m_hasStat;
    private Vector2 m_lastDrawOffset;
    private Vector2 m_lastSize;
    private string m_lastStatString;
    private string m_currentText;
    private bool m_dirty = true;

    public float Scale { get; set; }

    public Vector4 TextColorMask { get; set; }

    public MyGuiDrawAlignEnum TextAlign { get; set; }

    public string Font
    {
      get => this.m_fontHash.String;
      set
      {
        this.m_fontHash = MyStringHash.GetOrCompute(value);
        this.m_font = MyGuiManager.GetFont(this.m_fontHash);
        this.m_dirty = true;
      }
    }

    public static string SubstituteTexts(string text, string context = null) => MyTexts.SubstituteTexts(text, context);

    public MyStatControlText(MyStatControls parent, string text)
      : base(parent)
    {
      this.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      this.Font = "Blue";
      this.Scale = 1f;
      this.TextColorMask = Vector4.One;
      this.m_text = MyTexts.SubstituteTexts(text);
      this.m_hasStat = this.m_text.Contains("{STAT}");
    }

    public override void Draw(float transitionAlpha)
    {
      Vector4 textColorMask = this.TextColorMask;
      if (this.BlinkBehavior.Blink && this.BlinkBehavior.ColorMask.HasValue)
        textColorMask = this.BlinkBehavior.ColorMask.Value;
      Vector4 vector4 = (Vector4) MyGuiControlBase.ApplyColorMaskModifiers(textColorMask, true, transitionAlpha);
      if (this.m_hasStat)
      {
        if (this.m_lastStatString != this.StatString)
        {
          this.m_lastStatString = this.StatString;
          this.m_textTmp.Clear();
          this.m_textTmp.Append(this.m_text);
          this.m_textTmp.Replace("{STAT}", this.m_lastStatString);
          this.m_dirty = true;
          this.m_currentText = this.m_textTmp.ToString();
        }
      }
      else
        this.m_currentText = this.m_text;
      if (this.m_dirty)
      {
        this.m_lastSize = this.m_font.MeasureString(this.m_currentText, this.Scale);
        this.m_lastDrawOffset = MyUtils.GetCoordTopLeftFromAligned(Vector2.Zero, this.m_lastSize, this.TextAlign) + this.Size / 2f;
        this.m_dirty = false;
      }
      MyRenderProxy.DrawString((int) this.m_fontHash, this.Position + this.m_lastDrawOffset, (Color) vector4, this.m_currentText, this.Scale, this.m_lastSize.X + 100f, false);
    }
  }
}
