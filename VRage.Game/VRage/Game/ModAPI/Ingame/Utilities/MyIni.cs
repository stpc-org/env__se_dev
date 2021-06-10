// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.Ingame.Utilities.MyIni
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace VRage.Game.ModAPI.Ingame.Utilities
{
  public class MyIni
  {
    private readonly Dictionary<MyIniKey, StringSegment> m_items = new Dictionary<MyIniKey, StringSegment>((IEqualityComparer<MyIniKey>) MyIni.MyIniKeyComparer.DEFAULT);
    private readonly Dictionary<StringSegment, int> m_sections = new Dictionary<StringSegment, int>((IEqualityComparer<StringSegment>) StringSegmentIgnoreCaseComparer.DEFAULT);
    private readonly Dictionary<MyIniKey, StringSegment> m_itemComments = new Dictionary<MyIniKey, StringSegment>((IEqualityComparer<MyIniKey>) MyIni.MyIniKeyComparer.DEFAULT);
    private readonly Dictionary<StringSegment, StringSegment> m_sectionComments = new Dictionary<StringSegment, StringSegment>((IEqualityComparer<StringSegment>) StringSegmentIgnoreCaseComparer.DEFAULT);
    private string m_content;
    private int m_sectionCounter;
    private StringBuilder m_tmpContentBuilder;
    private StringBuilder m_tmpValueBuilder;
    private List<MyIniKey> m_tmpKeyList;
    private List<string> m_tmpStringList;
    private StringSegment m_endComment;
    private StringSegment m_endContent;

    public static bool HasSection(string config, string section) => MyIni.FindSection(config, section) >= 0;

    private static int FindSection(string config, string section)
    {
      TextPtr ptr = new TextPtr(config);
      if (MyIni.MatchesSection(ref ptr, section))
        return ptr.Index;
      while (!ptr.IsOutOfBounds())
      {
        ptr = TextPtr.op_Increment(ptr.Find("\n"));
        if (ptr.Char == '[')
        {
          if (MyIni.MatchesSection(ref ptr, section))
            return ptr.Index;
        }
        else if (ptr.StartsWith("---"))
        {
          ptr = (ptr + 3).SkipWhitespace();
          if (ptr.IsEndOfLine())
            break;
        }
      }
      return -1;
    }

    private static bool MatchesSection(ref TextPtr ptr, string section)
    {
      if (!ptr.StartsWith("["))
        return false;
      TextPtr textPtr = ptr + 1;
      if (!textPtr.StartsWithCaseInsensitive(section))
        return false;
      textPtr += section.Length;
      return textPtr.StartsWith("]");
    }

    private StringBuilder TmpContentBuilder
    {
      get
      {
        if (this.m_tmpContentBuilder == null)
          this.m_tmpContentBuilder = new StringBuilder();
        return this.m_tmpContentBuilder;
      }
    }

    private StringBuilder TmpValueBuilder
    {
      get
      {
        if (this.m_tmpValueBuilder == null)
          this.m_tmpValueBuilder = new StringBuilder();
        return this.m_tmpValueBuilder;
      }
    }

    private List<MyIniKey> TmpKeyList
    {
      get
      {
        if (this.m_tmpKeyList == null)
          this.m_tmpKeyList = new List<MyIniKey>();
        return this.m_tmpKeyList;
      }
    }

    private List<string> TmpStringList
    {
      get
      {
        if (this.m_tmpStringList == null)
          this.m_tmpStringList = new List<string>();
        return this.m_tmpStringList;
      }
    }

    public string EndContent
    {
      get => this.m_endContent.ToString();
      set
      {
        this.m_endContent = value == null ? new StringSegment() : new StringSegment(value);
        this.m_content = (string) null;
      }
    }

    public bool ContainsSection(string section) => this.m_sections.ContainsKey(new StringSegment(section));

    public bool ContainsKey(string section, string name) => this.ContainsKey(new MyIniKey(section, name));

    public bool ContainsKey(MyIniKey key) => this.m_items.ContainsKey(key);

    public void GetKeys(string section, List<MyIniKey> keys)
    {
      if (keys == null)
        return;
      this.GetKeys(new StringSegment(section), keys);
    }

    private void GetKeys(StringSegment section, List<MyIniKey> keys)
    {
      if (keys == null)
        return;
      keys.Clear();
      foreach (MyIniKey key in this.m_items.Keys)
      {
        if (key.SectionSegment.EqualsIgnoreCase(section))
          keys.Add(key);
      }
    }

    public void GetKeys(List<MyIniKey> keys)
    {
      if (keys == null)
        return;
      keys.Clear();
      foreach (MyIniKey key in this.m_items.Keys)
        keys.Add(key);
    }

    public void GetSections(List<string> names)
    {
      if (names == null)
        return;
      names.Clear();
      foreach (StringSegment key in this.m_sections.Keys)
        names.Add(key.ToString());
    }

    public string EndComment
    {
      get
      {
        StringSegment endCommentSegment = this.GetEndCommentSegment();
        return endCommentSegment.IsEmpty ? (string) null : endCommentSegment.ToString();
      }
      set
      {
        if (value == null)
        {
          this.m_endComment = new StringSegment();
        }
        else
        {
          this.m_endComment = new StringSegment(value);
          this.m_content = (string) null;
        }
      }
    }

    private StringSegment GetEndCommentSegment()
    {
      StringSegment endComment = this.m_endComment;
      if (!endComment.IsCached)
      {
        this.RealizeComment(ref endComment);
        this.m_endComment = endComment;
      }
      return endComment;
    }

    public void SetEndComment(string comment)
    {
      if (comment == null)
      {
        this.m_endComment = new StringSegment();
      }
      else
      {
        this.m_endComment = new StringSegment(comment);
        this.m_content = (string) null;
      }
    }

    public string GetSectionComment(string section)
    {
      StringSegment sectionCommentSegment = this.GetSectionCommentSegment(new StringSegment(section));
      return sectionCommentSegment.IsEmpty ? (string) null : sectionCommentSegment.ToString();
    }

    private StringSegment GetSectionCommentSegment(StringSegment key)
    {
      StringSegment comment;
      if (!this.m_sectionComments.TryGetValue(key, out comment))
        return new StringSegment();
      if (!comment.IsCached)
      {
        this.RealizeComment(ref comment);
        this.m_sectionComments[key] = comment;
      }
      return comment;
    }

    public void SetSectionComment(string section, string comment)
    {
      StringSegment key = new StringSegment(section);
      if (!this.m_sections.ContainsKey(key))
        throw new ArgumentException("No section named " + section);
      if (comment == null)
      {
        this.m_sectionComments.Remove(key);
      }
      else
      {
        StringSegment stringSegment = new StringSegment(comment);
        this.m_sectionComments[key] = stringSegment;
        this.m_content = (string) null;
      }
    }

    public string GetComment(string section, string name) => this.GetComment(new MyIniKey(section, name));

    public string GetComment(MyIniKey key)
    {
      StringSegment commentSegment = this.GetCommentSegment(key);
      return commentSegment.IsEmpty ? (string) null : commentSegment.ToString();
    }

    private StringSegment GetCommentSegment(MyIniKey key)
    {
      StringSegment comment;
      if (!this.m_itemComments.TryGetValue(key, out comment))
        return new StringSegment();
      if (!comment.IsCached)
      {
        this.RealizeComment(ref comment);
        this.m_itemComments[key] = comment;
      }
      return comment;
    }

    public void SetComment(string section, string name, string comment) => this.SetComment(new MyIniKey(section, name), comment);

    public void SetComment(MyIniKey key, string comment)
    {
      if (!this.m_items.ContainsKey(key))
        throw new ArgumentException("No item named " + (object) key);
      if (comment == null)
      {
        this.m_itemComments.Remove(key);
      }
      else
      {
        StringSegment stringSegment = new StringSegment(comment);
        this.m_itemComments[key] = stringSegment;
        this.m_content = (string) null;
      }
    }

    public MyIniValue Get(string section, string name) => this.Get(new MyIniKey(section, name));

    public MyIniValue Get(MyIniKey key)
    {
      StringSegment stringSegment;
      if (!this.m_items.TryGetValue(key, out stringSegment))
        return MyIniValue.EMPTY;
      this.Realize(ref key, ref stringSegment);
      return new MyIniValue(key, stringSegment.ToString());
    }

    private void RealizeComment(ref StringSegment comment)
    {
      if (comment.IsCached)
        return;
      TextPtr textPtr1 = new TextPtr(comment.Text, comment.Start);
      if (comment.Length <= 0)
        return;
      StringBuilder tmpValueBuilder = this.TmpValueBuilder;
      try
      {
        TextPtr textPtr2 = textPtr1 + comment.Length;
        bool flag = false;
        while ((int) textPtr1 < (int) textPtr2)
        {
          if (flag)
            tmpValueBuilder.Append('\n');
          if (textPtr1.Char == ';')
          {
            ++textPtr1;
            TextPtr endOfLine = textPtr1.FindEndOfLine();
            int count = endOfLine.Index - textPtr1.Index;
            tmpValueBuilder.Append(textPtr1.Content, textPtr1.Index, count);
            textPtr1 = endOfLine.SkipWhitespace();
            if (textPtr1.IsEndOfLine())
            {
              if (textPtr1.Char == '\r')
                textPtr1 += 2;
              else
                ++textPtr1;
            }
          }
          else
          {
            textPtr1 = textPtr1.SkipWhitespace();
            if (textPtr1.IsEndOfLine())
            {
              if (textPtr1.Char == '\r')
                textPtr1 += 2;
              else
                ++textPtr1;
            }
            else
              break;
          }
          flag = true;
        }
        comment = new StringSegment(tmpValueBuilder.ToString());
      }
      finally
      {
        tmpValueBuilder.Clear();
      }
    }

    private void Realize(ref MyIniKey key, ref StringSegment value)
    {
      if (value.IsCached)
        return;
      string text = value.Text;
      TextPtr textPtr = new TextPtr(text, value.Start);
      if (value.Length > 0 && textPtr.IsNewLine())
      {
        StringBuilder tmpValueBuilder = this.TmpValueBuilder;
        try
        {
          textPtr = textPtr.FindEndOfLine(true);
          ++textPtr;
          int count = value.Start + value.Length - textPtr.Index;
          tmpValueBuilder.Append(text, textPtr.Index, count);
          tmpValueBuilder.Replace("\n|", "\n");
          this.m_items[key] = value = new StringSegment(tmpValueBuilder.ToString());
        }
        finally
        {
          tmpValueBuilder.Clear();
        }
      }
      else
        this.m_items[key] = value = new StringSegment(value.ToString());
    }

    public void Delete(string section, string name)
    {
      this.Delete(new MyIniKey(section, name));
      this.m_content = (string) null;
    }

    public void Delete(MyIniKey key)
    {
      if (key.IsEmpty)
        throw new ArgumentException("Key cannot be empty", nameof (key));
      this.m_items.Remove(key);
      this.m_itemComments.Remove(key);
      this.m_content = (string) null;
    }

    public void Set(string section, string name, string value) => this.Set(new MyIniKey(section, name), value);

    public void Set(MyIniKey key, string value)
    {
      if (key.IsEmpty)
        throw new ArgumentException("Key cannot be empty", nameof (key));
      if (value == null)
      {
        this.Delete(key);
      }
      else
      {
        StringSegment sectionSegment = key.SectionSegment;
        this.AddSection(ref sectionSegment);
        this.m_items[key] = new StringSegment(value);
        this.m_content = (string) null;
      }
    }

    public void Set(string section, string name, bool value) => this.Set(section, name, value ? "true" : "false");

    public void Set(MyIniKey key, bool value) => this.Set(key, value ? "true" : "false");

    public void Set(string section, string name, byte value) => this.Set(section, name, value.ToString((IFormatProvider) CultureInfo.InvariantCulture));

    public void Set(MyIniKey key, byte value) => this.Set(key, value.ToString((IFormatProvider) CultureInfo.InvariantCulture));

    public void Set(string section, string name, sbyte value) => this.Set(section, name, value.ToString((IFormatProvider) CultureInfo.InvariantCulture));

    public void Set(MyIniKey key, sbyte value) => this.Set(key, value.ToString((IFormatProvider) CultureInfo.InvariantCulture));

    public void Set(string section, string name, ushort value) => this.Set(section, name, value.ToString((IFormatProvider) CultureInfo.InvariantCulture));

    public void Set(MyIniKey key, ushort value) => this.Set(key, value.ToString((IFormatProvider) CultureInfo.InvariantCulture));

    public void Set(string section, string name, short value) => this.Set(section, name, value.ToString((IFormatProvider) CultureInfo.InvariantCulture));

    public void Set(MyIniKey key, short value) => this.Set(key, value.ToString((IFormatProvider) CultureInfo.InvariantCulture));

    public void Set(string section, string name, uint value) => this.Set(section, name, value.ToString((IFormatProvider) CultureInfo.InvariantCulture));

    public void Set(MyIniKey key, uint value) => this.Set(key, value.ToString((IFormatProvider) CultureInfo.InvariantCulture));

    public void Set(string section, string name, int value) => this.Set(section, name, value.ToString((IFormatProvider) CultureInfo.InvariantCulture));

    public void Set(MyIniKey key, int value) => this.Set(key, value.ToString((IFormatProvider) CultureInfo.InvariantCulture));

    public void Set(string section, string name, ulong value) => this.Set(section, name, value.ToString((IFormatProvider) CultureInfo.InvariantCulture));

    public void Set(MyIniKey key, ulong value) => this.Set(key, value.ToString((IFormatProvider) CultureInfo.InvariantCulture));

    public void Set(string section, string name, long value) => this.Set(section, name, value.ToString((IFormatProvider) CultureInfo.InvariantCulture));

    public void Set(MyIniKey key, long value) => this.Set(key, value.ToString((IFormatProvider) CultureInfo.InvariantCulture));

    public void Set(string section, string name, float value) => this.Set(section, name, value.ToString("R", (IFormatProvider) CultureInfo.InvariantCulture));

    public void Set(MyIniKey key, float value) => this.Set(key, value.ToString("R", (IFormatProvider) CultureInfo.InvariantCulture));

    public void Set(string section, string name, double value) => this.Set(section, name, value.ToString("R", (IFormatProvider) CultureInfo.InvariantCulture));

    public void Set(MyIniKey key, double value) => this.Set(key, value.ToString("R", (IFormatProvider) CultureInfo.InvariantCulture));

    public void Set(string section, string name, Decimal value) => this.Set(section, name, value.ToString((IFormatProvider) CultureInfo.InvariantCulture));

    public void Set(MyIniKey key, Decimal value) => this.Set(key, value.ToString((IFormatProvider) CultureInfo.InvariantCulture));

    public void AddSection(string section)
    {
      StringSegment section1 = new StringSegment(section);
      this.AddSection(ref section1);
      this.m_content = (string) null;
    }

    public bool DeleteSection(string section)
    {
      StringSegment stringSegment = new StringSegment(section);
      if (!this.m_sections.Remove(stringSegment))
        return false;
      List<MyIniKey> myIniKeyList = new List<MyIniKey>();
      foreach (MyIniKey key in this.m_items.Keys)
      {
        if (key.NameSegment.EqualsIgnoreCase(stringSegment))
          myIniKeyList.Add(key);
      }
      foreach (MyIniKey key in myIniKeyList)
        this.m_items.Remove(key);
      this.m_sectionComments.Remove(stringSegment);
      this.m_content = (string) null;
      return true;
    }

    public void Clear()
    {
      this.m_items.Clear();
      this.m_sections.Clear();
      this.m_content = (string) null;
      this.m_sectionCounter = 0;
      this.m_endContent = new StringSegment();
      this.m_endComment = new StringSegment();
      this.m_itemComments.Clear();
      this.m_sectionComments.Clear();
      if (this.m_tmpContentBuilder != null)
        this.m_tmpContentBuilder.Clear();
      if (this.m_tmpValueBuilder != null)
        this.m_tmpValueBuilder.Clear();
      if (this.m_tmpKeyList != null)
        this.m_tmpKeyList.Clear();
      if (this.m_tmpStringList == null)
        return;
      this.m_tmpStringList.Clear();
    }

    public bool TryParse(string content)
    {
      MyIniParseResult result = new MyIniParseResult();
      return this.TryParseCore(content, (string) null, ref result);
    }

    public bool TryParse(string content, out MyIniParseResult result)
    {
      result = new MyIniParseResult(new TextPtr(content), (string) null);
      return this.TryParseCore(content, (string) null, ref result);
    }

    public bool TryParse(string content, string section, out MyIniParseResult result)
    {
      result = new MyIniParseResult(new TextPtr(content), (string) null);
      return this.TryParseCore(content, section, ref result);
    }

    public bool TryParse(string content, string section)
    {
      MyIniParseResult result = new MyIniParseResult();
      return this.TryParseCore(content, section, ref result);
    }

    private bool TryParseCore(string content, string section, ref MyIniParseResult result)
    {
      content = content ?? "";
      if (string.Equals(this.m_content, content, StringComparison.Ordinal))
        return true;
      this.Clear();
      if (string.IsNullOrWhiteSpace(content))
      {
        this.m_content = content;
        return true;
      }
      TextPtr ptr = new TextPtr(content);
      if (section != null)
      {
        int section1 = MyIni.FindSection(content, section);
        if (section1 == -1)
        {
          if (result.IsDefined)
            result = new MyIniParseResult(new TextPtr(content), string.Format("Cannot find section \"{0}\"", (object) section));
          return false;
        }
        ptr += section1;
      }
      while (!ptr.IsOutOfBounds())
      {
        bool success;
        if (!this.TryParseSection(ref ptr, ref result, out success, section == null))
        {
          if (!success)
            return false;
          break;
        }
        if (section != null)
        {
          this.m_content = (string) null;
          return true;
        }
      }
      this.m_content = content;
      return true;
    }

    public void Invalidate() => this.m_content = (string) null;

    private void ReadPrefix(ref TextPtr ptr, out StringSegment prefix)
    {
      bool flag = false;
      TextPtr textPtr1 = ptr;
      while (!ptr.IsOutOfBounds())
      {
        if (ptr.IsStartOfLine() && ptr.Char == ';')
        {
          if (!flag)
          {
            flag = true;
            textPtr1 = ptr;
          }
          ptr = ptr.FindEndOfLine();
        }
        TextPtr textPtr2 = ptr.SkipWhitespace();
        if (textPtr2.IsNewLine())
          ptr = textPtr2.Char != '\r' ? textPtr2 + 1 : textPtr2 + 2;
        else
          break;
      }
      if (flag)
      {
        TextPtr textPtr2 = ptr;
        while (char.IsWhiteSpace(textPtr2.Char) && (int) textPtr2 > (int) textPtr1)
          --textPtr2;
        int length = textPtr2.Index - textPtr1.Index;
        if (length > 0)
        {
          prefix = new StringSegment(ptr.Content, textPtr1.Index, length);
          return;
        }
      }
      prefix = new StringSegment();
    }

    private bool TryParseSection(
      ref TextPtr ptr,
      ref MyIniParseResult result,
      out bool success,
      bool parseEndContent)
    {
      TextPtr ptr1 = ptr;
      StringSegment prefix;
      this.ReadPrefix(ref ptr1, out prefix);
      this.m_endComment = prefix;
      if (parseEndContent && this.TryParseEndContent(ref ptr1))
      {
        ptr = ptr1;
        success = true;
        return false;
      }
      if (ptr1.IsOutOfBounds())
      {
        ptr = new TextPtr(ptr1.Content, ptr1.Content.Length);
        success = true;
        return false;
      }
      if (ptr1.Char != '[')
      {
        if (result.IsDefined)
          result = new MyIniParseResult(ptr, "Expected [section] definition");
        success = false;
        return false;
      }
      TextPtr endOfLine1 = ptr1.FindEndOfLine();
      while (endOfLine1.Index > ptr1.Index && endOfLine1.Char != ']')
        --endOfLine1;
      if (endOfLine1.Char != ']')
      {
        if (result.IsDefined)
          result = new MyIniParseResult(ptr, "Expected [section] definition");
        success = false;
        return false;
      }
      TextPtr ptr2 = TextPtr.op_Increment(ptr1);
      StringSegment key = new StringSegment(ptr2.Content, ptr2.Index, endOfLine1.Index - ptr2.Index);
      string str = MyIniKey.ValidateSection(ref key);
      if (str != null)
      {
        if (result.IsDefined)
          result = new MyIniParseResult(ptr2, string.Format("Section {0}", (object) str));
        success = false;
        return false;
      }
      TextPtr ptr3 = (endOfLine1 + 1).SkipWhitespace();
      if (!ptr3.IsEndOfLine())
      {
        if (result.IsDefined)
          result = new MyIniParseResult(ptr3, "Expected newline");
        success = false;
        return false;
      }
      TextPtr endOfLine2 = ptr3.FindEndOfLine(true);
      this.AddSection(ref key);
      if (!prefix.IsEmpty)
      {
        this.m_sectionComments[key] = prefix;
        this.m_endComment = new StringSegment();
      }
      do
        ;
      while (this.TryParseItem(ref key, ref endOfLine2, ref result, out success, parseEndContent));
      if (!success)
        return false;
      ptr = endOfLine2;
      success = true;
      return true;
    }

    private void AddSection(ref StringSegment section)
    {
      if (this.m_sections.ContainsKey(section))
        return;
      this.m_sections[section] = this.m_sectionCounter;
      ++this.m_sectionCounter;
    }

    private bool TryParseItem(
      ref StringSegment section,
      ref TextPtr ptr,
      ref MyIniParseResult result,
      out bool success,
      bool parseEndContent)
    {
      TextPtr ptr1 = ptr;
      StringSegment prefix;
      this.ReadPrefix(ref ptr1, out prefix);
      this.m_endComment = prefix;
      if (parseEndContent && this.TryParseEndContent(ref ptr1))
      {
        ptr = new TextPtr(ptr1.Content, ptr1.Content.Length);
        success = true;
        return false;
      }
      ptr1 = ptr1.TrimStart();
      if (ptr1.IsOutOfBounds() || ptr1.Char == '[')
      {
        success = true;
        return false;
      }
      TextPtr inLine = ptr1.FindInLine('=');
      if (inLine.IsOutOfBounds())
      {
        if (result.IsDefined)
          result = new MyIniParseResult(ptr1, "Expected key=value definition");
        success = false;
        return false;
      }
      StringSegment segment = new StringSegment(ptr1.Content, ptr1.Index, inLine.TrimEnd().Index - ptr1.Index);
      string str = MyIniKey.ValidateKey(ref segment);
      if (str != null)
      {
        if (result.IsDefined)
          result = new MyIniParseResult(ptr1, string.Format("Key {0}", (object) str));
        success = false;
        return false;
      }
      ptr1 = inLine + 1;
      ptr1 = ptr1.TrimStart();
      TextPtr textPtr1 = ptr1.FindEndOfLine();
      StringSegment stringSegment = new StringSegment(ptr1.Content, ptr1.Index, textPtr1.TrimEnd().Index - ptr1.Index);
      if (stringSegment.Length == 0)
      {
        TextPtr endOfLine1 = textPtr1.FindEndOfLine(true);
        if (endOfLine1.Char == '|')
        {
          TextPtr textPtr2 = endOfLine1;
          TextPtr endOfLine2;
          do
          {
            endOfLine2 = textPtr2.FindEndOfLine();
            textPtr2 = endOfLine2.FindEndOfLine(true);
          }
          while (textPtr2.Char == '|');
          textPtr1 = endOfLine2;
        }
        stringSegment = new StringSegment(ptr1.Content, ptr1.Index, textPtr1.Index - ptr1.Index);
      }
      MyIniKey key = new MyIniKey(section, segment);
      if (this.m_items.ContainsKey(key))
      {
        if (result.IsDefined)
          result = new MyIniParseResult(new TextPtr(segment.Text, segment.Start), string.Format("Duplicate key {0}", (object) key));
        success = false;
        return false;
      }
      this.m_items[key] = stringSegment;
      if (!prefix.IsEmpty)
      {
        this.m_itemComments[key] = prefix;
        this.m_endComment = new StringSegment();
      }
      ptr = textPtr1.FindEndOfLine(true);
      success = true;
      return true;
    }

    private bool TryParseEndContent(ref TextPtr ptr)
    {
      if (!ptr.StartsWith("---"))
        return false;
      TextPtr textPtr = (ptr + 3).SkipWhitespace();
      if (!textPtr.IsEndOfLine())
        return false;
      ptr = textPtr.FindEndOfLine(true);
      textPtr = new TextPtr(ptr.Content, ptr.Content.Length);
      this.m_endContent = new StringSegment(ptr.Content, ptr.Index, textPtr.Index - ptr.Index);
      ptr = textPtr;
      return true;
    }

    public override string ToString()
    {
      if (this.m_content == null)
        this.m_content = this.GenerateContent();
      return this.m_content;
    }

    private static bool NeedsMultilineFormat(ref StringSegment str)
    {
      if (str.Length <= 0)
        return false;
      return char.IsWhiteSpace(str[0]) || char.IsWhiteSpace(str[str.Length - 1]) || str.IndexOf('\n') >= 0;
    }

    private string GenerateContent()
    {
      StringBuilder tmpContentBuilder = this.TmpContentBuilder;
      List<MyIniKey> tmpKeyList = this.TmpKeyList;
      List<string> tmpStringList = this.TmpStringList;
      try
      {
        bool flag = false;
        foreach (StringSegment stringSegment1 in (IEnumerable<StringSegment>) this.m_sections.Keys.OrderBy<StringSegment, int>((Func<StringSegment, int>) (s => this.m_sections[s])))
        {
          if (flag)
            tmpContentBuilder.Append('\n');
          flag = true;
          StringSegment stringSegment2 = this.GetSectionCommentSegment(stringSegment1);
          if (!stringSegment2.IsEmpty)
          {
            stringSegment2.GetLines(tmpStringList);
            foreach (string str in tmpStringList)
            {
              tmpContentBuilder.Append(";");
              tmpContentBuilder.Append(str);
              tmpContentBuilder.Append('\n');
            }
          }
          tmpContentBuilder.Append("[");
          tmpContentBuilder.Append((object) stringSegment1);
          tmpContentBuilder.Append("]\n");
          this.GetKeys(stringSegment1, tmpKeyList);
          for (int index = 0; index < tmpKeyList.Count; ++index)
          {
            MyIniKey key = tmpKeyList[index];
            StringSegment nameSegment = key.NameSegment;
            stringSegment2 = this.GetCommentSegment(key);
            if (!stringSegment2.IsEmpty)
            {
              stringSegment2.GetLines(tmpStringList);
              foreach (string str in tmpStringList)
              {
                tmpContentBuilder.Append(";");
                tmpContentBuilder.Append(str);
                tmpContentBuilder.Append('\n');
              }
            }
            tmpContentBuilder.Append(nameSegment.Text, nameSegment.Start, nameSegment.Length);
            tmpContentBuilder.Append('=');
            StringSegment str1 = this.m_items[key];
            if (MyIni.NeedsMultilineFormat(ref str1))
            {
              this.Realize(ref key, ref str1);
              str1.GetLines(tmpStringList);
              tmpContentBuilder.Append('\n');
              foreach (string str2 in tmpStringList)
              {
                tmpContentBuilder.Append("|");
                tmpContentBuilder.Append(str2);
                tmpContentBuilder.Append('\n');
              }
            }
            else
            {
              tmpContentBuilder.Append(str1.Text, str1.Start, str1.Length);
              tmpContentBuilder.Append('\n');
            }
          }
        }
        StringSegment endCommentSegment = this.GetEndCommentSegment();
        if (!endCommentSegment.IsEmpty)
        {
          tmpContentBuilder.Append('\n');
          endCommentSegment.GetLines(tmpStringList);
          foreach (string str in tmpStringList)
          {
            tmpContentBuilder.Append(";");
            tmpContentBuilder.Append(str);
            tmpContentBuilder.Append('\n');
          }
        }
        if (this.m_endContent.Length > 0)
        {
          tmpContentBuilder.Append('\n');
          tmpContentBuilder.Append("---\n");
          tmpContentBuilder.Append((object) this.m_endContent);
        }
        string str3 = tmpContentBuilder.ToString();
        tmpContentBuilder.Clear();
        tmpKeyList.Clear();
        return str3;
      }
      finally
      {
        tmpContentBuilder.Clear();
        tmpKeyList.Clear();
      }
    }

    private class MyIniKeyComparer : IEqualityComparer<MyIniKey>
    {
      public static readonly MyIni.MyIniKeyComparer DEFAULT = new MyIni.MyIniKeyComparer();

      public bool Equals(MyIniKey x, MyIniKey y) => x.Equals(y);

      public int GetHashCode(MyIniKey obj) => obj.GetHashCode();
    }
  }
}
