// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.Ingame.Utilities.TextPtr
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace VRage.Game.ModAPI.Ingame.Utilities
{
  [DebuggerDisplay("{ToDebugString(),nq}")]
  public struct TextPtr
  {
    public readonly string Content;
    public readonly int Index;

    public static implicit operator int(TextPtr ptr) => ptr.Index;

    public static implicit operator string(TextPtr ptr) => ptr.Content;

    public static TextPtr operator +(TextPtr ptr, int offset) => new TextPtr(ptr.Content, ptr.Index + offset);

    public static TextPtr operator ++(TextPtr ptr) => new TextPtr(ptr.Content, ptr.Index + 1);

    public static TextPtr operator -(TextPtr ptr, int offset) => new TextPtr(ptr.Content, ptr.Index - offset);

    public static TextPtr operator --(TextPtr ptr) => new TextPtr(ptr.Content, ptr.Index - 1);

    public TextPtr(string content)
    {
      this.Content = content;
      this.Index = 0;
    }

    public TextPtr(string content, int index)
    {
      this.Content = content;
      this.Index = index;
    }

    public bool IsOutOfBounds() => this.Index < 0 || this.Index >= this.Content.Length;

    public char Char => !this.IsOutOfBounds() ? this.Content[this.Index] : char.MinValue;

    public bool IsEmpty => this.Content == null;

    public int FindLineNo()
    {
      string content = this.Content;
      int index1 = this.Index;
      int num = 1;
      for (int index2 = 0; index2 < index1; ++index2)
      {
        if (content[index2] == '\n')
          ++num;
      }
      return num;
    }

    public TextPtr Find(string str)
    {
      if (this.IsOutOfBounds())
        return this;
      int index = this.Content.IndexOf(str, this.Index, StringComparison.InvariantCulture);
      return index == -1 ? new TextPtr(this.Content, this.Content.Length) : new TextPtr(this.Content, index);
    }

    public TextPtr Find(char ch)
    {
      if (this.IsOutOfBounds())
        return this;
      int index = this.Content.IndexOf(ch, this.Index);
      return index == -1 ? new TextPtr(this.Content, this.Content.Length) : new TextPtr(this.Content, index);
    }

    public TextPtr FindAny(char[] chs)
    {
      if (this.IsOutOfBounds())
        return this;
      int index = this.Content.IndexOfAny(chs, this.Index);
      return index == -1 ? new TextPtr(this.Content, this.Content.Length) : new TextPtr(this.Content, index);
    }

    public TextPtr FindInLine(char ch)
    {
      if (this.IsOutOfBounds())
        return this;
      string content = this.Content;
      int length = this.Content.Length;
      for (int index = this.Index; index < length; ++index)
      {
        char ch1 = content[index];
        if ((int) ch1 == (int) ch)
          return new TextPtr(content, index);
        if (ch1 == '\r' || ch1 == '\n')
          break;
      }
      return new TextPtr(this.Content, this.Content.Length);
    }

    public TextPtr FindAnyInLine(char[] chs)
    {
      if (this.IsOutOfBounds())
        return this;
      string content = this.Content;
      int length = this.Content.Length;
      for (int index = this.Index; index < length; ++index)
      {
        char ch = content[index];
        if (Array.IndexOf<char>(chs, ch) >= 0)
          return new TextPtr(content, index);
        if (ch == '\r' || ch == '\n')
          break;
      }
      return new TextPtr(this.Content, this.Content.Length);
    }

    public TextPtr FindEndOfLine(bool skipNewline = false)
    {
      int length = this.Content.Length;
      if (this.Index >= length)
        return this;
      TextPtr textPtr = this;
      while (textPtr.Index < length)
      {
        if (textPtr.IsNewLine())
        {
          if (skipNewline)
          {
            if (textPtr.Char == '\r')
              ++textPtr;
            ++textPtr;
            break;
          }
          break;
        }
        ++textPtr;
      }
      return textPtr;
    }

    public bool StartsWithCaseInsensitive(string what)
    {
      TextPtr textPtr = this;
      foreach (char c in what)
      {
        if ((int) char.ToUpper(textPtr.Char) != (int) char.ToUpper(c))
          return false;
        ++textPtr;
      }
      return true;
    }

    public bool StartsWith(string what)
    {
      TextPtr textPtr = this;
      foreach (char ch in what)
      {
        if ((int) textPtr.Char != (int) ch)
          return false;
        ++textPtr;
      }
      return true;
    }

    public TextPtr SkipWhitespace(bool skipNewline = false)
    {
      TextPtr textPtr = this;
      int length = textPtr.Content.Length;
      if (skipNewline)
      {
        while (true)
        {
          char c = textPtr.Char;
          if (textPtr.Index < length && char.IsWhiteSpace(c))
            ++textPtr;
          else
            break;
        }
        return textPtr;
      }
      while (true)
      {
        char c = textPtr.Char;
        if (textPtr.Index < length && !textPtr.IsNewLine() && char.IsWhiteSpace(c))
          ++textPtr;
        else
          break;
      }
      return textPtr;
    }

    public bool IsEndOfLine() => this.Index >= this.Content.Length || this.IsNewLine();

    public bool IsStartOfLine() => this.Index <= 0 || (this - 1).IsNewLine();

    public bool IsNewLine()
    {
      switch (this.Char)
      {
        case '\n':
          return true;
        case '\r':
          return this.Index < this.Content.Length - 1 && this.Content[this.Index + 1] == '\n';
        default:
          return false;
      }
    }

    public TextPtr TrimStart()
    {
      string content = this.Content;
      int index = this.Index;
      for (int length = content.Length; index < length; ++index)
      {
        switch (content[index])
        {
          case '\t':
          case ' ':
            continue;
          default:
            goto label_4;
        }
      }
label_4:
      return new TextPtr(content, index);
    }

    public TextPtr TrimEnd()
    {
      string content = this.Content;
      int index;
      for (index = this.Index - 1; index >= 0; --index)
      {
        switch (content[index])
        {
          case '\t':
          case ' ':
            continue;
          default:
            goto label_4;
        }
      }
label_4:
      return new TextPtr(content, index + 1);
    }

    private string ToDebugString()
    {
      if (this.Index < 0)
        return "<before>";
      if (this.Index >= this.Content.Length)
        return "<after>";
      int num = this.Index + 37;
      return Regex.Replace(num <= this.Content.Length ? this.Content.Substring(this.Index, num - this.Index) + "..." : this.Content.Substring(this.Index, this.Content.Length - this.Index), "[\\r\\t\\n]", (MatchEvaluator) (match =>
      {
        string str = match.Value;
        if (str == "\r")
          return "\\r";
        if (str == "\t")
          return "\\t";
        return str == "\n" ? "\\n" : match.Value;
      }));
    }
  }
}
