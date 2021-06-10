// Decompiled with JetBrains decompiler
// Type: VRage.Filesystem.FindFilesRegEx.FindFilesPatternToRegex
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Text.RegularExpressions;

namespace VRage.Filesystem.FindFilesRegEx
{
  public static class FindFilesPatternToRegex
  {
    private static Regex HasQuestionMarkRegEx = new Regex("\\?", RegexOptions.Compiled);
    private static Regex IlegalCharactersRegex = new Regex("[\\/:<>|\"]", RegexOptions.Compiled);
    private static Regex CatchExtentionRegex = new Regex("^\\s*.+\\.([^\\.]+)\\s*$", RegexOptions.Compiled);
    private static string NonDotCharacters = "[^.]*";

    public static Regex Convert(string pattern)
    {
      pattern = pattern != null ? pattern.Trim() : throw new ArgumentNullException();
      if (pattern.Length == 0)
        throw new ArgumentException("Pattern is empty.");
      bool flag1 = !FindFilesPatternToRegex.IlegalCharactersRegex.IsMatch(pattern) ? FindFilesPatternToRegex.CatchExtentionRegex.IsMatch(pattern) : throw new ArgumentException("Patterns contains ilegal characters.");
      bool flag2 = false;
      if (FindFilesPatternToRegex.HasQuestionMarkRegEx.IsMatch(pattern))
        flag2 = true;
      else if (flag1)
        flag2 = FindFilesPatternToRegex.CatchExtentionRegex.Match(pattern).Groups[1].Length != 3;
      string str = Regex.Replace("^" + Regex.Replace(Regex.Escape(pattern), "\\\\\\*", ".*"), "\\\\\\?", ".");
      if (!flag2 & flag1)
        str += FindFilesPatternToRegex.NonDotCharacters;
      return new Regex(str + "$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }
  }
}
