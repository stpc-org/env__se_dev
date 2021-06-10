// Decompiled with JetBrains decompiler
// Type: Sandbox.Graphics.GUI.MyOffensiveWords
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Sandbox.Graphics.GUI
{
  public class MyOffensiveWords
  {
    private static MyOffensiveWords m_instance;
    private Regex m_blacklistRegEx;
    private Regex m_whitelistRegEx;

    public static MyOffensiveWords Instance => MyOffensiveWords.m_instance ?? (MyOffensiveWords.m_instance = new MyOffensiveWords());

    private Regex CreateFilterRegEx(List<string> list)
    {
      StringBuilder stringBuilder = new StringBuilder("\\b(");
      foreach (string str in list)
      {
        stringBuilder.Append(str.Replace("|", "[|]"));
        stringBuilder.Append("|");
      }
      if (list.Count > 0)
        stringBuilder.Remove(stringBuilder.Length - 1, 1);
      stringBuilder.Append(")\\b");
      return new Regex(stringBuilder.ToString(), RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }

    public void Init(List<string> blacklist, List<string> whitelist)
    {
      List<string> list1 = blacklist;
      if (list1 == null)
        list1 = new List<string>() { "" };
      this.m_blacklistRegEx = this.CreateFilterRegEx(list1);
      List<string> list2 = whitelist;
      if (list2 == null)
        list2 = new List<string>() { "" };
      this.m_whitelistRegEx = this.CreateFilterRegEx(list2);
    }

    public string IsTextOffensive(string text)
    {
      if (this.m_blacklistRegEx == null)
        return (string) null;
      Match match = this.m_blacklistRegEx.Match(this.m_whitelistRegEx.Replace(text, string.Empty));
      return match.Length <= 0 ? (string) null : match.Value;
    }

    public string IsTextOffensive(StringBuilder sb) => this.m_blacklistRegEx == null ? (string) null : this.IsTextOffensive(sb.ToString());

    public string FixOffensiveString(string text, string replacement = "***") => this.m_blacklistRegEx != null ? this.m_blacklistRegEx.Replace(text, replacement) : text;
  }
}
