// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHudText
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using System.Text;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyHudText
  {
    public static readonly MyHudText.ComparerType Comparer = new MyHudText.ComparerType();
    public string Font;
    public Vector2 Position;
    public Color Color;
    public float Scale;
    public MyGuiDrawAlignEnum Alignement;
    public bool Visible;
    private readonly StringBuilder m_text;

    public MyHudText() => this.m_text = new StringBuilder(256);

    public MyHudText Start(
      string font,
      Vector2 position,
      Color color,
      float scale,
      MyGuiDrawAlignEnum alignement)
    {
      this.Font = font;
      this.Position = position;
      this.Color = color;
      this.Scale = scale;
      this.Alignement = alignement;
      this.m_text.Clear();
      return this;
    }

    public void Append(StringBuilder sb) => this.m_text.AppendStringBuilder(sb);

    public void Append(string text) => this.m_text.Append(text);

    public void AppendInt32(int number) => this.m_text.AppendInt32(number);

    public void AppendLine() => this.m_text.AppendLine();

    public StringBuilder GetStringBuilder() => this.m_text;

    public class ComparerType : IComparer<MyHudText>
    {
      public int Compare(MyHudText x, MyHudText y) => x.Font.CompareTo(y.Font);
    }
  }
}
