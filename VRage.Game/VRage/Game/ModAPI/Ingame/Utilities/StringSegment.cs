// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.Ingame.Utilities.StringSegment
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRage.Utils;

namespace VRage.Game.ModAPI.Ingame.Utilities
{
  public struct StringSegment
  {
    private static readonly char[] NEWLINE_CHARS = new char[2]
    {
      '\r',
      '\n'
    };
    public readonly string Text;
    public readonly int Start;
    public readonly int Length;
    private string m_cache;

    public StringSegment(string text)
      : this(text, 0, text.Length)
      => this.m_cache = text;

    public StringSegment(string text, int start, int length)
    {
      this.Text = text;
      this.Start = start;
      this.Length = Math.Max(0, length);
      this.m_cache = (string) null;
    }

    public bool IsEmpty => this.Text == null;

    public bool IsCached => this.m_cache != null;

    public int IndexOf(char ch)
    {
      if (this.Length == 0)
        return -1;
      int num = this.Text.IndexOf(ch, this.Start, this.Length);
      return num >= 0 ? num - this.Start : -1;
    }

    public int IndexOf(char ch, int start)
    {
      if (this.Length == 0)
        return -1;
      int num = this.Text.IndexOf(ch, this.Start + start, this.Length);
      return num >= 0 ? num - this.Start : -1;
    }

    public int IndexOfAny(char[] chars)
    {
      if (this.Length == 0)
        return -1;
      int num = this.Text.IndexOfAny(chars, this.Start, this.Length);
      return num >= 0 ? num : -1;
    }

    public override bool Equals(object obj) => obj is string other && this.Equals(other);

    public override int GetHashCode() => MyUtils.GetHash(this.Text, this.Start, this.Length);

    public bool Equals(string other) => this.Equals(new StringSegment(other));

    public bool Equals(StringSegment other)
    {
      if (this.Length != other.Length)
        return false;
      string text1 = this.Text;
      int start1 = this.Start;
      string text2 = other.Text;
      int start2 = other.Start;
      for (int index = 0; index < this.Length; ++index)
      {
        if ((int) text1[start1] != (int) text2[start2])
          return false;
        ++start1;
        ++start2;
      }
      return true;
    }

    public bool EqualsIgnoreCase(string other) => this.EqualsIgnoreCase(new StringSegment(other));

    public bool EqualsIgnoreCase(StringSegment other)
    {
      if (this.Length != other.Length)
        return false;
      string text1 = this.Text;
      int start1 = this.Start;
      string text2 = other.Text;
      int start2 = other.Start;
      for (int index = 0; index < this.Length; ++index)
      {
        if ((int) char.ToUpperInvariant(text1[start1]) != (int) char.ToUpperInvariant(text2[start2]))
          return false;
        ++start1;
        ++start2;
      }
      return true;
    }

    public override string ToString()
    {
      if (this.Length == 0)
        return "";
      if (this.m_cache == null)
        this.m_cache = this.Text.Substring(this.Start, this.Length);
      return this.m_cache;
    }

    public char this[int i] => i < 0 || i >= this.Length ? char.MinValue : this.Text[this.Start + i];

    public void GetLines(List<StringSegment> lines)
    {
      if (this.Length == 0)
        return;
      string text = this.Text;
      if (string.IsNullOrEmpty(text))
        return;
      int num1 = this.Start;
      int num2 = num1 + this.Length;
      lines.Clear();
      while (num1 < num2)
      {
        int num3 = text.IndexOfAny(StringSegment.NEWLINE_CHARS, num1, num2 - num1);
        if (num3 < 0)
        {
          lines.Add(new StringSegment(text, num1, text.Length - num1));
          break;
        }
        lines.Add(new StringSegment(text, num1, num3 - num1));
        num1 = num3;
        if (num1 < text.Length && text[num1] == '\r')
          ++num1;
        if (num1 < text.Length && text[num1] == '\n')
          ++num1;
      }
    }

    public void GetLines(List<string> lines)
    {
      if (this.Length == 0)
        return;
      string text = this.Text;
      if (string.IsNullOrEmpty(text))
        return;
      int num1 = this.Start;
      int num2 = num1 + this.Length;
      lines.Clear();
      while (num1 < num2)
      {
        int num3 = text.IndexOfAny(StringSegment.NEWLINE_CHARS, num1, num2 - num1);
        if (num3 < 0)
        {
          lines.Add(text.Substring(num1, text.Length - num1));
          break;
        }
        lines.Add(text.Substring(num1, num3 - num1));
        num1 = num3;
        if (num1 < text.Length && text[num1] == '\r')
          ++num1;
        if (num1 < text.Length && text[num1] == '\n')
          ++num1;
      }
    }
  }
}
