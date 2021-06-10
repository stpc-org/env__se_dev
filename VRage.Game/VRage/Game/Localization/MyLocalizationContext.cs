// Decompiled with JetBrains decompiler
// Type: VRage.Game.Localization.MyLocalizationContext
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.Localization
{
  public class MyLocalizationContext
  {
    protected readonly MyStringId m_contextName;
    protected readonly List<string> m_languagesHelper = new List<string>();
    private readonly List<MyLocalizationContext.LocalizationFileInfo> m_localizationFileInfos = new List<MyLocalizationContext.LocalizationFileInfo>();
    protected readonly Dictionary<MyStringId, MyObjectBuilder_Localization> m_loadedFiles = new Dictionary<MyStringId, MyObjectBuilder_Localization>((IEqualityComparer<MyStringId>) MyStringId.Comparer);
    private MyLocalizationPackage m_package = new MyLocalizationPackage();
    private MyLocalizationContext m_twinContext;
    private readonly HashSet<ulong> m_switchHelper = new HashSet<ulong>();

    public ListReader<string> Languages => (ListReader<string>) this.m_languagesHelper;

    public IEnumerable<MyStringId> Ids => (IEnumerable<MyStringId>) this.m_package.Keys;

    public int IdsCount => this.m_package.Keys.Count;

    public MyStringId Name => this.m_contextName;

    public string CurrentLanguage { get; private set; }

    internal MyLocalizationContext TwinContext
    {
      get => this.m_twinContext;
      set => this.m_twinContext = value;
    }

    public void Dispose()
    {
      this.m_languagesHelper.Clear();
      this.m_package.Clear();
      this.m_loadedFiles.Clear();
      this.m_switchHelper.Clear();
      this.m_localizationFileInfos.Clear();
    }

    internal MyLocalizationContext(MyStringId name) => this.m_contextName = name;

    internal void UnloadBundle(MyStringId bundleId)
    {
      int index = 0;
      while (index < this.m_localizationFileInfos.Count)
      {
        MyLocalizationContext.LocalizationFileInfo localizationFileInfo = this.m_localizationFileInfos[index];
        if (bundleId == MyStringId.NullOrEmpty && localizationFileInfo.Bundle != MyStringId.NullOrEmpty || localizationFileInfo.Bundle == bundleId && localizationFileInfo.Bundle != MyStringId.NullOrEmpty)
        {
          this.m_loadedFiles.Remove(MyStringId.GetOrCompute(localizationFileInfo.HeaderPath));
          this.m_localizationFileInfos.RemoveAt(index);
        }
        else
          ++index;
      }
      this.Switch(this.CurrentLanguage);
    }

    internal void InsertFileInfo(MyLocalizationContext.LocalizationFileInfo info)
    {
      this.m_localizationFileInfos.Add(info);
      if (this.m_languagesHelper.Contains(info.Header.Language))
        return;
      this.m_languagesHelper.Add(info.Header.Language);
    }

    private void Load(
      MyLocalizationContext.LocalizationFileInfo fileInfo)
    {
      if (string.IsNullOrEmpty(fileInfo.Header.ResXName) || fileInfo.Header.Entries.Count > 0)
        return;
      string directoryName = Path.GetDirectoryName(fileInfo.HeaderPath);
      if (string.IsNullOrEmpty(directoryName))
        return;
      using (Stream inStream = MyFileSystem.OpenRead(Path.Combine(directoryName, fileInfo.Header.ResXName)))
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(inStream);
        foreach (XmlNode selectNode in xmlDocument.SelectNodes("/root/data"))
        {
          string str1 = selectNode.Attributes["name"].Value;
          string str2 = (string) null;
          foreach (XmlNode childNode in selectNode.ChildNodes)
          {
            if (childNode.Name.Equals("value", StringComparison.InvariantCultureIgnoreCase))
            {
              XmlNodeReader xmlNodeReader = new XmlNodeReader(childNode);
              if (xmlNodeReader.Read())
                str2 = xmlNodeReader.ReadString();
            }
          }
          fileInfo.Header.Entries.Add(new MyObjectBuilder_Localization.KeyEntry()
          {
            Key = str1,
            Value = str2
          });
        }
      }
    }

    public StringBuilder this[MyStringId id] => this.Localize(id);

    public StringBuilder this[string nameId] => this.Localize(MyStringId.GetOrCompute(nameId));

    public void Switch(string language)
    {
      this.CurrentLanguage = language;
      this.m_package.Clear();
      this.m_switchHelper.Clear();
      foreach (MyLocalizationContext.LocalizationFileInfo localizationFileInfo in this.m_localizationFileInfos)
      {
        if (!(localizationFileInfo.Header.Language != language) && !(localizationFileInfo.Bundle != MyStringId.NullOrEmpty))
        {
          this.m_switchHelper.Add((ulong) localizationFileInfo.Header.Id);
          this.Load(localizationFileInfo);
          this.LoadLocalizationFileData(localizationFileInfo.Header);
        }
      }
      foreach (MyLocalizationContext.LocalizationFileInfo localizationFileInfo in this.m_localizationFileInfos)
      {
        if (!(localizationFileInfo.Header.Language != language) && !(localizationFileInfo.Bundle == MyStringId.NullOrEmpty))
        {
          this.m_switchHelper.Add((ulong) localizationFileInfo.Header.Id);
          this.Load(localizationFileInfo);
          this.LoadLocalizationFileData(localizationFileInfo.Header, true);
        }
      }
      foreach (MyLocalizationContext.LocalizationFileInfo localizationFileInfo in this.m_localizationFileInfos)
      {
        if (localizationFileInfo.Header.Default)
        {
          this.Load(localizationFileInfo);
          this.LoadLocalizationFileData(localizationFileInfo.Header, suppressError: true);
        }
      }
    }

    private void LoadLocalizationFileData(
      MyObjectBuilder_Localization localization,
      bool overrideExisting = false,
      bool suppressError = false)
    {
      if (localization == null)
        return;
      foreach (MyObjectBuilder_Localization.KeyEntry entry in localization.Entries)
      {
        if (!this.m_package.AddMessage(entry.Key, entry.Value, overrideExisting) && !overrideExisting && !suppressError)
          MyLog.Default.WriteLine("LocalizationContext: Context " + this.m_contextName.String + " already contains id " + entry.Key + " conflicting entry won't be overwritten.");
      }
    }

    public StringBuilder Localize(MyStringId id)
    {
      StringBuilder messageSb;
      if (this.m_package.TryGetStringBuilder(id, MyTexts.GlobalVariantSelector, out messageSb))
        return MyTexts.SubstituteTexts(messageSb);
      return this.TwinContext?.Localize(id);
    }

    public override int GetHashCode() => this.m_contextName.Id;

    protected bool Equals(MyLocalizationContext other) => this.m_contextName.Equals(other.m_contextName);

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (this == obj)
        return true;
      return obj is MyStringId id ? this.m_contextName.Equals(id) : this.Equals((MyLocalizationContext) obj);
    }

    internal struct LocalizationFileInfo
    {
      public readonly MyObjectBuilder_Localization Header;
      public readonly MyStringId Bundle;
      public readonly string HeaderPath;

      public LocalizationFileInfo(
        string headerFilePath,
        MyObjectBuilder_Localization header,
        MyStringId bundle)
      {
        this.Bundle = bundle;
        this.Header = header;
        this.HeaderPath = headerFilePath;
      }
    }
  }
}
