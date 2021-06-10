// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Localization.MyLanguage
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game.Localization;
using VRage.Utils;

namespace Sandbox.Game.Localization
{
  public static class MyLanguage
  {
    private static string m_actualCultureName;
    private static MyLanguagesEnum m_actualLanguage;
    private static HashSet<MyLanguagesEnum> m_supportedLanguages = new HashSet<MyLanguagesEnum>();
    private static string m_currentOSCultureName;

    public static void Init()
    {
      MyTexts.LoadSupportedLanguages(MyLanguage.GetLocalizationPath(), MyLanguage.m_supportedLanguages);
      MyLanguage.LoadLanguage(MyLanguagesEnum.English);
    }

    public static HashSetReader<MyLanguagesEnum> SupportedLanguages => (HashSetReader<MyLanguagesEnum>) MyLanguage.m_supportedLanguages;

    public static MyLanguagesEnum CurrentLanguage
    {
      set
      {
        MyLanguage.LoadLanguage(value);
        if (MySandboxGame.Config.Language != MyLanguage.m_actualLanguage)
        {
          MySandboxGame.Config.Language = MyLanguage.m_actualLanguage;
          MySandboxGame.Config.Save();
        }
        MyGuiManager.CurrentLanguage = MyLanguage.m_actualLanguage;
        MyLanguage.m_actualCultureName = MyLanguage.ConvertLangEnum(MyLanguage.m_actualLanguage);
        MyLocalization.Static.Switch(MyLanguage.m_actualCultureName);
      }
      get => MyLanguage.m_actualLanguage;
    }

    public static string CurrentCultureName
    {
      get => MyLanguage.m_actualCultureName ?? CultureInfo.CurrentCulture.Name;
      set => MyLanguage.m_actualCultureName = value;
    }

    private static void LoadLanguage(MyLanguagesEnum value)
    {
      MyTexts.MyLanguageDescription language = MyTexts.Languages[value];
      MyTexts.Clear();
      MyTexts.LoadTexts(MyLanguage.GetLocalizationPath(), language.CultureName, language.SubcultureName);
      MyGuiManager.LanguageTextScale = language.GuiTextScale;
      MyLanguage.m_actualLanguage = value;
      MyLanguage.m_actualCultureName = MyLanguage.ConvertLangEnum(value);
    }

    private static string GetLocalizationPath() => Path.Combine(MyFileSystem.ContentPath, "Data", "Localization");

    [Conditional("DEBUG")]
    private static void GenerateCurrentLanguageCharTable()
    {
      SortedSet<char> sortedSet = new SortedSet<char>();
      foreach (MyStringId enumValue in typeof (MyStringId).GetEnumValues())
      {
        StringBuilder stringBuilder = MyTexts.Get(enumValue);
        for (int index = 0; index < stringBuilder.Length; ++index)
          sortedSet.Add(stringBuilder[index]);
      }
      List<char> charList = new List<char>((IEnumerable<char>) sortedSet);
      string userDataPath = MyFileSystem.UserDataPath;
      string path2_1 = string.Format("character-table-{0}.txt", (object) MyLanguage.CurrentLanguage);
      using (StreamWriter streamWriter = new StreamWriter(Path.Combine(userDataPath, path2_1)))
      {
        foreach (char ch in charList)
          streamWriter.WriteLine(string.Format("{0}\t{1:x4}", (object) ch, (object) (int) ch));
      }
      string path2_2 = string.Format("character-ranges-{0}.txt", (object) MyLanguage.CurrentLanguage);
      using (StreamWriter streamWriter = new StreamWriter(Path.Combine(userDataPath, path2_2)))
      {
        int index = 0;
        while (index < charList.Count)
        {
          int num1;
          int num2 = num1 = (int) charList[index];
          for (++index; index < charList.Count && (int) charList[index] == num2 + 1; ++index)
            num2 = (int) charList[index];
          streamWriter.WriteLine(string.Format("-range {0:x4}-{1:x4}", (object) num1, (object) num2));
        }
      }
    }

    public static MyLanguagesEnum GetOsLanguageCurrent() => MyLanguage.ConvertLangEnum(MyLanguage.m_currentOSCultureName);

    public static MyLanguagesEnum GetOsLanguageCurrentOfficial()
    {
      MyLanguagesEnum key = MyLanguage.ConvertLangEnum(MyLanguage.m_currentOSCultureName);
      MyTexts.MyLanguageDescription languageDescription;
      MyTexts.Languages.TryGetValue(key, out languageDescription);
      return languageDescription == null || languageDescription.IsCommunityLocalized ? MyLanguagesEnum.English : key;
    }

    public static MyLanguagesEnum ConvertLangEnum(string cultureName)
    {
      if (cultureName == null)
        cultureName = CultureInfo.CurrentCulture.Name;
      switch (cultureName)
      {
        case "ca-ES":
          return MyLanguagesEnum.Catalan;
        case "cs-CZ":
          return MyLanguagesEnum.Czech;
        case "da-DK":
          return MyLanguagesEnum.Danish;
        case "de-DE":
          return MyLanguagesEnum.German;
        case "en-US":
          return MyLanguagesEnum.English;
        case "es":
          return MyLanguagesEnum.Spanish_HispanicAmerica;
        case "es-ES":
          return MyLanguagesEnum.Spanish_Spain;
        case "et-EE":
          return MyLanguagesEnum.Estonian;
        case "fi-FI":
          return MyLanguagesEnum.Finnish;
        case "fr-FR":
          return MyLanguagesEnum.French;
        case "hr-HR":
          return MyLanguagesEnum.Croatian;
        case "hu-HU":
          return MyLanguagesEnum.Hungarian;
        case "is-IS":
          return MyLanguagesEnum.Icelandic;
        case "it-IT":
          return MyLanguagesEnum.Italian;
        case "lv-LV":
          return MyLanguagesEnum.Latvian;
        case "nb-NO":
          return MyLanguagesEnum.Norwegian;
        case "nl-NL":
          return MyLanguagesEnum.Dutch;
        case "pl-PL":
          return MyLanguagesEnum.Polish;
        case "pt-BR":
          return MyLanguagesEnum.Portuguese_Brazil;
        case "ro-RO":
          return MyLanguagesEnum.Romanian;
        case "ru-RU":
          return MyLanguagesEnum.Russian;
        case "sk-SK":
          return MyLanguagesEnum.Slovak;
        case "sv-SE":
          return MyLanguagesEnum.Swedish;
        case "tr-TR":
          return MyLanguagesEnum.Turkish;
        case "uk-UA":
          return MyLanguagesEnum.Ukrainian;
        case "zh-CN":
          return MyLanguagesEnum.ChineseChina;
        default:
          return MyLanguagesEnum.English;
      }
    }

    public static string ConvertLangEnum(MyLanguagesEnum enumVal)
    {
      switch (enumVal)
      {
        case MyLanguagesEnum.English:
          return "en-US";
        case MyLanguagesEnum.Czech:
          return "cs-CZ";
        case MyLanguagesEnum.Slovak:
          return "sk-SK";
        case MyLanguagesEnum.German:
          return "de-DE";
        case MyLanguagesEnum.Russian:
          return "ru-RU";
        case MyLanguagesEnum.Spanish_Spain:
          return "es-ES";
        case MyLanguagesEnum.French:
          return "fr-FR";
        case MyLanguagesEnum.Italian:
          return "it-IT";
        case MyLanguagesEnum.Danish:
          return "da-DK";
        case MyLanguagesEnum.Dutch:
          return "nl-NL";
        case MyLanguagesEnum.Icelandic:
          return "is-IS";
        case MyLanguagesEnum.Polish:
          return "pl-PL";
        case MyLanguagesEnum.Finnish:
          return "fi-FI";
        case MyLanguagesEnum.Hungarian:
          return "hu-HU";
        case MyLanguagesEnum.Portuguese_Brazil:
          return "pt-BR";
        case MyLanguagesEnum.Estonian:
          return "et-EE";
        case MyLanguagesEnum.Norwegian:
          return "nb-NO";
        case MyLanguagesEnum.Spanish_HispanicAmerica:
          return "es";
        case MyLanguagesEnum.Swedish:
          return "sv-SE";
        case MyLanguagesEnum.Catalan:
          return "ca-ES";
        case MyLanguagesEnum.Croatian:
          return "hr-HR";
        case MyLanguagesEnum.Romanian:
          return "ro-RO";
        case MyLanguagesEnum.Ukrainian:
          return "uk-UA";
        case MyLanguagesEnum.Turkish:
          return "tr-TR";
        case MyLanguagesEnum.Latvian:
          return "lv-LV";
        case MyLanguagesEnum.ChineseChina:
          return "zh-CN";
        default:
          return "en-US";
      }
    }

    public static void ObtainCurrentOSCulture() => MyLanguage.m_currentOSCultureName = CultureInfo.CurrentCulture.Name;

    public static string GetCurrentOSCulture() => MyLanguage.m_currentOSCultureName;
  }
}
