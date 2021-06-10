// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.Ingame.Utilities.MyIniKey
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRage.Utils;

namespace VRage.Game.ModAPI.Ingame.Utilities
{
  public struct MyIniKey : IEquatable<MyIniKey>
  {
    private static readonly char[] INVALID_SECTION_CHARS = new char[4]
    {
      '\r',
      '\n',
      '[',
      ']'
    };
    private static readonly string INVALID_SECTION_CHARS_STR = "\\r, \\n, [, ]";
    private static readonly char[] INVALID_KEY_CHARS = new char[6]
    {
      '\r',
      '\n',
      '|',
      '=',
      '[',
      ']'
    };
    private static readonly string INVALID_KEY_CHARS_STR = "\\r, \\n, |, =, [, ]";
    public static readonly MyIniKey EMPTY = new MyIniKey();
    internal readonly StringSegment SectionSegment;
    internal readonly StringSegment NameSegment;

    internal static string ValidateSection(ref StringSegment segment)
    {
      if (segment.Length == 0)
        return "cannot be empty.";
      return segment.IndexOfAny(MyIniKey.INVALID_KEY_CHARS) >= 0 ? string.Format("contains illegal characters ({0})", (object) MyIniKey.INVALID_KEY_CHARS_STR) : (string) null;
    }

    internal static string ValidateKey(ref StringSegment segment)
    {
      if (segment.Length == 0)
        return "cannot be empty.";
      return segment.IndexOfAny(MyIniKey.INVALID_KEY_CHARS) >= 0 ? string.Format("contains illegal characters ({0})", (object) MyIniKey.INVALID_KEY_CHARS_STR) : (string) null;
    }

    public static bool operator ==(MyIniKey a, MyIniKey b) => a.Equals(b);

    public static bool operator !=(MyIniKey a, MyIniKey b) => !a.Equals(b);

    public static bool TryParse(string input, out MyIniKey key)
    {
      if (string.IsNullOrEmpty(input))
      {
        key = MyIniKey.EMPTY;
        return false;
      }
      int length = input.IndexOf("/", StringComparison.Ordinal);
      if (length == -1)
      {
        key = MyIniKey.EMPTY;
        return false;
      }
      string section = input.Substring(0, length).Trim();
      string name = input.Substring(length + 2).Trim();
      if (section.Length == 0 || section.IndexOfAny(MyIniKey.INVALID_SECTION_CHARS) >= 0 || (name.Length == 0 || name.IndexOfAny(MyIniKey.INVALID_KEY_CHARS) >= 0))
      {
        key = MyIniKey.EMPTY;
        return false;
      }
      key = new MyIniKey(section, name);
      return true;
    }

    public static MyIniKey Parse(string input)
    {
      MyIniKey key;
      if (!MyIniKey.TryParse(input, out key))
        throw new ArgumentException("Invalid configuration key format", nameof (input));
      return key;
    }

    public string Section => this.SectionSegment.ToString();

    public string Name => this.NameSegment.ToString();

    public MyIniKey(string section, string name)
    {
      if (string.IsNullOrWhiteSpace(section))
        throw new ArgumentException("Section cannot be empty.", nameof (section));
      if (section.IndexOfAny(MyIniKey.INVALID_SECTION_CHARS) >= 0)
        throw new ArgumentException(string.Format("Section contains illegal characters ({0})", (object) MyIniKey.INVALID_SECTION_CHARS_STR), nameof (section));
      if (string.IsNullOrWhiteSpace(name))
        throw new ArgumentException("Key cannot be null or whitespace.", nameof (name));
      if (name.IndexOfAny(MyIniKey.INVALID_KEY_CHARS) >= 0)
        throw new ArgumentException(string.Format("Key contains illegal characters ({0})", (object) MyIniKey.INVALID_KEY_CHARS_STR), nameof (name));
      this.SectionSegment = new StringSegment(section);
      this.NameSegment = new StringSegment(name);
    }

    internal MyIniKey(StringSegment section, StringSegment name)
    {
      this.SectionSegment = section;
      this.NameSegment = name;
    }

    public bool IsEmpty => this.NameSegment.Length == 0;

    public bool Equals(MyIniKey other) => this.SectionSegment.EqualsIgnoreCase(other.SectionSegment) && this.NameSegment.EqualsIgnoreCase(other.NameSegment);

    public override bool Equals(object obj) => obj != null && obj is MyIniKey other && this.Equals(other);

    public override int GetHashCode() => MyUtils.GetHashUpperCase(this.SectionSegment.Text, this.SectionSegment.Start, this.SectionSegment.Length) * 397 ^ MyUtils.GetHashUpperCase(this.NameSegment.Text, this.NameSegment.Start, this.NameSegment.Length);

    public override string ToString() => this.IsEmpty ? "" : string.Format("{0}/{1}", (object) this.Section, (object) this.Name);
  }
}
