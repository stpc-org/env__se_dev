// Decompiled with JetBrains decompiler
// Type: VRage.MyTexts
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Utils;

namespace VRage
{
  public static class MyTexts
  {
    private static readonly string LOCALIZATION_TAG_GENERAL = "LOCG";
    public static readonly MyStringId GAMEPAD_VARIANT_ID = MyStringId.GetOrCompute("Gamepad");
    private static readonly Dictionary<MyLanguagesEnum, MyTexts.MyLanguageDescription> m_languageIdToLanguage = new Dictionary<MyLanguagesEnum, MyTexts.MyLanguageDescription>();
    private static readonly Dictionary<string, MyLanguagesEnum> m_cultureToLanguageId = new Dictionary<string, MyLanguagesEnum>();
    private static readonly bool m_checkMissingTexts = false;
    private static MyLocalizationPackage m_package = new MyLocalizationPackage();
    private static MyStringId m_selectedVariant = MyStringId.NullOrEmpty;
    private static Regex m_textReplace;
    private static readonly Dictionary<string, ITextEvaluator> m_evaluators = new Dictionary<string, ITextEvaluator>();
    private static MatchEvaluator m_ReplaceEvaluator = new MatchEvaluator(MyTexts.ReplaceEvaluator);

    static MyTexts()
    {
      MyTexts.AddLanguage(MyLanguagesEnum.English, "en", displayName: "English", isCommunityLocalized: false);
      MyTexts.AddLanguage(MyLanguagesEnum.Czech, "cs", "CZ", "Česky", 0.95f);
      MyTexts.AddLanguage(MyLanguagesEnum.Slovak, "sk", "SK", "Slovenčina", 0.95f);
      MyTexts.AddLanguage(MyLanguagesEnum.German, "de", displayName: "Deutsch", isCommunityLocalized: false);
      MyTexts.AddLanguage(MyLanguagesEnum.Russian, "ru", displayName: "Русский", isCommunityLocalized: false);
      MyTexts.AddLanguage(MyLanguagesEnum.Spanish_Spain, "es", displayName: "Español (España)", isCommunityLocalized: false);
      MyTexts.AddLanguage(MyLanguagesEnum.French, "fr", displayName: "Français", isCommunityLocalized: false);
      MyTexts.AddLanguage(MyLanguagesEnum.Italian, "it", displayName: "Italiano");
      MyTexts.AddLanguage(MyLanguagesEnum.Danish, "da", displayName: "Dansk");
      MyTexts.AddLanguage(MyLanguagesEnum.Dutch, "nl", displayName: "Nederlands");
      MyTexts.AddLanguage(MyLanguagesEnum.Icelandic, "is", "IS", "Íslenska");
      MyTexts.AddLanguage(MyLanguagesEnum.Polish, "pl", "PL", "Polski");
      MyTexts.AddLanguage(MyLanguagesEnum.Finnish, "fi", displayName: "Suomi");
      MyTexts.AddLanguage(MyLanguagesEnum.Hungarian, "hu", "HU", "Magyar", 0.85f);
      MyTexts.AddLanguage(MyLanguagesEnum.Portuguese_Brazil, "pt", "BR", "Português (Brasileiro)");
      MyTexts.AddLanguage(MyLanguagesEnum.Estonian, "et", "EE", "Eesti");
      MyTexts.AddLanguage(MyLanguagesEnum.Norwegian, "no", displayName: "Norsk");
      MyTexts.AddLanguage(MyLanguagesEnum.Spanish_HispanicAmerica, "es", "419", "Español (Latinoamerica)");
      MyTexts.AddLanguage(MyLanguagesEnum.Swedish, "sv", displayName: "Svenska", guiTextScale: 0.9f);
      MyTexts.AddLanguage(MyLanguagesEnum.Catalan, "ca", "AD", "Català", 0.85f);
      MyTexts.AddLanguage(MyLanguagesEnum.Croatian, "hr", "HR", "Hrvatski", 0.9f);
      MyTexts.AddLanguage(MyLanguagesEnum.Romanian, "ro", displayName: "Română", guiTextScale: 0.85f);
      MyTexts.AddLanguage(MyLanguagesEnum.Ukrainian, "uk", displayName: "Українська");
      MyTexts.AddLanguage(MyLanguagesEnum.Turkish, "tr", "TR", "Türkçe");
      MyTexts.AddLanguage(MyLanguagesEnum.Latvian, "lv", displayName: "Latviešu", guiTextScale: 0.87f);
      MyTexts.AddLanguage(MyLanguagesEnum.ChineseChina, "zh", "CN", "Chinese", isCommunityLocalized: false);
      MyTexts.RegisterEvaluator(MyTexts.LOCALIZATION_TAG_GENERAL, (ITextEvaluator) new MyTexts.MyGeneralEvaluator());
    }

    private static void AddLanguage(
      MyLanguagesEnum id,
      string cultureName,
      string subcultureName = null,
      string displayName = null,
      float guiTextScale = 1f,
      bool isCommunityLocalized = true)
    {
      MyTexts.MyLanguageDescription languageDescription = new MyTexts.MyLanguageDescription(id, displayName, cultureName, subcultureName, guiTextScale, isCommunityLocalized);
      MyTexts.m_languageIdToLanguage.Add(id, languageDescription);
      MyTexts.m_cultureToLanguageId.Add(languageDescription.FullCultureName, id);
    }

    public static MyStringId GlobalVariantSelector => MyTexts.m_selectedVariant;

    public static DictionaryReader<MyLanguagesEnum, MyTexts.MyLanguageDescription> Languages => new DictionaryReader<MyLanguagesEnum, MyTexts.MyLanguageDescription>(MyTexts.m_languageIdToLanguage);

    public static MyLanguagesEnum GetBestSuitableLanguage(string culture)
    {
      MyLanguagesEnum myLanguagesEnum = MyLanguagesEnum.English;
      if (!MyTexts.m_cultureToLanguageId.TryGetValue(culture, out myLanguagesEnum))
      {
        string[] strArray = culture.Split('-');
        string str1 = strArray[0];
        string str2 = strArray[1];
        foreach (KeyValuePair<MyLanguagesEnum, MyTexts.MyLanguageDescription> keyValuePair in MyTexts.m_languageIdToLanguage)
        {
          if (keyValuePair.Value.FullCultureName == str1)
            return keyValuePair.Key;
        }
      }
      return myLanguagesEnum;
    }

    public static string GetSystemLanguage() => CultureInfo.InstalledUICulture.Name;

    public static void LoadSupportedLanguages(
      string rootDirectory,
      HashSet<MyLanguagesEnum> outSupportedLanguages)
    {
      outSupportedLanguages.Add(MyLanguagesEnum.English);
      IEnumerable<string> files = MyFileSystem.GetFiles(rootDirectory, "*.resx", MySearchOption.TopDirectoryOnly);
      HashSet<string> stringSet = new HashSet<string>();
      foreach (string path in files)
      {
        string[] strArray = Path.GetFileNameWithoutExtension(path).Split('.');
        if (strArray.Length > 1)
          stringSet.Add(strArray[1]);
      }
      foreach (string key in stringSet)
      {
        MyLanguagesEnum myLanguagesEnum;
        if (MyTexts.m_cultureToLanguageId.TryGetValue(key, out myLanguagesEnum))
          outSupportedLanguages.Add(myLanguagesEnum);
      }
    }

    public static void SetGlobalVariantSelector(MyStringId variantName) => MyTexts.m_selectedVariant = variantName;

    public static string SubstituteTexts(string text, string context = null) => text != null ? MyTexts.m_textReplace.Replace(text, (MatchEvaluator) (match => MyTexts.ReplaceEvaluator(match, context))) : (string) null;

    public static StringBuilder SubstituteTexts(StringBuilder text)
    {
      if (text == null)
        return (StringBuilder) null;
      string input = text.ToString();
      string str = MyTexts.m_textReplace.Replace(input, MyTexts.m_ReplaceEvaluator);
      return !(input == str) ? new StringBuilder(str) : text;
    }

    public static StringBuilder Get(MyStringId id)
    {
      StringBuilder messageSb;
      if (!MyTexts.m_package.TryGetStringBuilder(id, MyTexts.m_selectedVariant, out messageSb))
        messageSb = !MyTexts.m_checkMissingTexts ? new StringBuilder(id.ToString()) : new StringBuilder("X_" + id.ToString());
      if (MyTexts.m_checkMissingTexts)
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("T_");
        messageSb = stringBuilder.Append((object) messageSb);
      }
      string input = messageSb.ToString();
      string str = MyTexts.m_textReplace.Replace(input, MyTexts.m_ReplaceEvaluator);
      return !(input == str) ? new StringBuilder(str) : messageSb;
    }

    public static string TrySubstitute(string input)
    {
      MyStringId orCompute = MyStringId.GetOrCompute(input);
      string message;
      return !MyTexts.m_package.TryGet(orCompute, MyTexts.m_selectedVariant, out message) ? input : MyTexts.m_textReplace.Replace(message, MyTexts.m_ReplaceEvaluator);
    }

    public static void RegisterEvaluator(string prefix, ITextEvaluator eval)
    {
      MyTexts.m_evaluators.Add(prefix, eval);
      MyTexts.InitReplace();
    }

    private static void InitReplace()
    {
      StringBuilder stringBuilder = new StringBuilder();
      int num = 0;
      stringBuilder.Append("{(");
      foreach (KeyValuePair<string, ITextEvaluator> evaluator in MyTexts.m_evaluators)
      {
        if (num != 0)
          stringBuilder.Append("|");
        stringBuilder.AppendFormat(evaluator.Key);
        ++num;
      }
      stringBuilder.Append("):((?:\\w|:)*)}");
      MyTexts.m_textReplace = new Regex(stringBuilder.ToString());
    }

    private static string ReplaceEvaluator(Match match) => MyTexts.ReplaceEvaluator(match, (string) null);

    private static string ReplaceEvaluator(Match match, string context)
    {
      ITextEvaluator textEvaluator;
      return match.Groups.Count != 3 || !MyTexts.m_evaluators.TryGetValue(match.Groups[1].Value, out textEvaluator) ? string.Empty : textEvaluator.TokenEvaluate(match.Groups[2].Value, context);
    }

    public static bool MatchesReplaceFormat(string str)
    {
      Regex textReplace = MyTexts.m_textReplace;
      return textReplace != null && textReplace.IsMatch(str);
    }

    public static string GetString(MyStringId id)
    {
      string message;
      if (!MyTexts.m_package.TryGet(id, MyTexts.m_selectedVariant, out message))
        message = !MyTexts.m_checkMissingTexts ? id.ToString() : "X_" + id.ToString();
      if (MyTexts.m_checkMissingTexts)
        message = "T_" + message;
      return MyTexts.m_textReplace.Replace(message, MyTexts.m_ReplaceEvaluator);
    }

    public static string GetString(string keyString) => MyTexts.GetString(MyStringId.GetOrCompute(keyString));

    public static bool Exists(MyStringId id) => MyTexts.m_package.ContainsKey(id);

    public static void Clear()
    {
      MyTexts.m_package.Clear();
      MyTexts.m_package.AddMessage("", "");
    }

    private static string GetPathWithFile(string file, List<string> allFiles)
    {
      foreach (string allFile in allFiles)
      {
        if (allFile.Contains(file))
          return allFile;
      }
      return (string) null;
    }

    public static bool IsTagged(string text, int position, string tag)
    {
      for (int index = 0; index < tag.Length; ++index)
      {
        if ((int) text[position + index] != (int) tag[index])
          return false;
      }
      return true;
    }

    public static void LoadTexts(string rootDirectory, string cultureName = null, string subcultureName = null)
    {
      HashSet<string> stringSet1 = new HashSet<string>();
      HashSet<string> stringSet2 = new HashSet<string>();
      HashSet<string> stringSet3 = new HashSet<string>();
      IEnumerable<string> files = MyFileSystem.GetFiles(rootDirectory, "*.resx", MySearchOption.AllDirectories);
      List<string> allFiles = new List<string>();
      foreach (string path in files)
      {
        if (path.Contains("MyCommonTexts"))
          stringSet1.Add(Path.GetFileNameWithoutExtension(path).Split('.')[0]);
        else if (path.Contains(nameof (MyTexts)))
          stringSet2.Add(Path.GetFileNameWithoutExtension(path).Split('.')[0]);
        else if (path.Contains("MyCoreTexts"))
          stringSet3.Add(Path.GetFileNameWithoutExtension(path).Split('.')[0]);
        else
          continue;
        allFiles.Add(path);
      }
      foreach (object obj in stringSet1)
        MyTexts.PatchTexts(MyTexts.GetPathWithFile(string.Format("{0}.resx", obj), allFiles));
      foreach (object obj in stringSet2)
        MyTexts.PatchTexts(MyTexts.GetPathWithFile(string.Format("{0}.resx", obj), allFiles));
      foreach (object obj in stringSet3)
        MyTexts.PatchTexts(MyTexts.GetPathWithFile(string.Format("{0}.resx", obj), allFiles));
      if (cultureName == null)
        return;
      foreach (object obj in stringSet1)
        MyTexts.PatchTexts(MyTexts.GetPathWithFile(string.Format("{0}.{1}.resx", obj, (object) cultureName), allFiles));
      foreach (object obj in stringSet2)
        MyTexts.PatchTexts(MyTexts.GetPathWithFile(string.Format("{0}.{1}.resx", obj, (object) cultureName), allFiles));
      foreach (object obj in stringSet3)
        MyTexts.PatchTexts(MyTexts.GetPathWithFile(string.Format("{0}.{1}.resx", obj, (object) cultureName), allFiles));
      if (subcultureName == null)
        return;
      foreach (object obj in stringSet1)
        MyTexts.PatchTexts(MyTexts.GetPathWithFile(string.Format("{0}.{1}-{2}.resx", obj, (object) cultureName, (object) subcultureName), allFiles));
      foreach (object obj in stringSet2)
        MyTexts.PatchTexts(MyTexts.GetPathWithFile(string.Format("{0}.{1}-{2}.resx", obj, (object) cultureName, (object) subcultureName), allFiles));
      foreach (object obj in stringSet3)
        MyTexts.PatchTexts(MyTexts.GetPathWithFile(string.Format("{0}.{1}-{2}.resx", obj, (object) cultureName, (object) subcultureName), allFiles));
    }

    private static void PatchTexts(string resourceFile)
    {
      if (!MyFileSystem.FileExists(resourceFile))
        return;
      using (Stream inStream = MyFileSystem.OpenRead(resourceFile))
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(inStream);
        foreach (XmlNode selectNode in xmlDocument.SelectNodes("/root/data"))
        {
          string key = selectNode.Attributes["name"].Value;
          string message = (string) null;
          foreach (XmlNode childNode in selectNode.ChildNodes)
          {
            if (childNode.Name.Equals("value", StringComparison.InvariantCultureIgnoreCase))
            {
              XmlNodeReader xmlNodeReader = new XmlNodeReader(childNode);
              if (xmlNodeReader.Read())
                message = xmlNodeReader.ReadString();
            }
          }
          if (!string.IsNullOrEmpty(key) && message != null)
            MyTexts.m_package.AddMessage(key, message, true);
        }
      }
    }

    public static StringBuilder AppendFormat(
      this StringBuilder stringBuilder,
      MyStringId textEnum,
      object arg0)
    {
      return stringBuilder.AppendFormat(MyTexts.GetString(textEnum), arg0);
    }

    public static StringBuilder AppendFormat(
      this StringBuilder stringBuilder,
      MyStringId textEnum,
      params object[] arg)
    {
      return stringBuilder.AppendFormat(MyTexts.GetString(textEnum), arg);
    }

    public static StringBuilder AppendFormat(
      this StringBuilder stringBuilder,
      MyStringId textEnum,
      MyStringId arg0)
    {
      return stringBuilder.AppendFormat(MyTexts.GetString(textEnum), (object) MyTexts.GetString(arg0));
    }

    public class MyLanguageDescription
    {
      public readonly MyLanguagesEnum Id;
      public readonly string Name;
      public readonly string CultureName;
      public readonly string SubcultureName;
      public readonly string FullCultureName;
      public readonly bool IsCommunityLocalized;
      public readonly float GuiTextScale;

      internal MyLanguageDescription(
        MyLanguagesEnum id,
        string displayName,
        string cultureName,
        string subcultureName,
        float guiTextScale,
        bool isCommunityLocalized)
      {
        this.Id = id;
        this.Name = displayName;
        this.CultureName = cultureName;
        this.SubcultureName = subcultureName;
        this.FullCultureName = !string.IsNullOrWhiteSpace(subcultureName) ? string.Format("{0}-{1}", (object) cultureName, (object) subcultureName) : cultureName;
        this.IsCommunityLocalized = isCommunityLocalized;
        this.GuiTextScale = guiTextScale;
      }
    }

    private class MyGeneralEvaluator : ITextEvaluator
    {
      public string TokenEvaluate(string token, string context)
      {
        StringBuilder stringBuilder = MyTexts.Get(MyStringId.GetOrCompute(token));
        return stringBuilder != null ? stringBuilder.ToString() : "";
      }
    }
  }
}
