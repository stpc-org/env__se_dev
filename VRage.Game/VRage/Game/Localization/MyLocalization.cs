// Decompiled with JetBrains decompiler
// Type: VRage.Game.Localization.MyLocalization
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VRage.FileSystem;
using VRage.Game.ObjectBuilders;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.Localization
{
  public class MyLocalization
  {
    private Dictionary<string, string> m_pathToContextTranslator = new Dictionary<string, string>();
    private static readonly FastResourceLock m_localizationLoadingLock = new FastResourceLock();
    public static readonly string LOCALIZATION_FOLDER = "Data\\Localization";
    private static readonly StringBuilder m_defaultLocalization = new StringBuilder("Failed localization attempt. Missing or not loaded contexts.");
    private static MyLocalization m_instance;
    private readonly Dictionary<MyStringId, MyLocalizationContext> m_contexts = new Dictionary<MyStringId, MyLocalizationContext>((IEqualityComparer<MyStringId>) MyStringId.Comparer);
    private readonly Dictionary<MyStringId, MyLocalizationContext> m_disposableContexts = new Dictionary<MyStringId, MyLocalizationContext>((IEqualityComparer<MyStringId>) MyStringId.Comparer);
    private readonly Dictionary<MyStringId, MyLocalization.MyBundle> m_loadedBundles = new Dictionary<MyStringId, MyLocalization.MyBundle>((IEqualityComparer<MyStringId>) MyStringId.Comparer);
    private static readonly string LOCALIZATION_TAG_CAMPAIGN = "LOCC";
    private static readonly string LOCALIZATION_TAG = "LOC";

    public string CurrentLanguage { get; private set; }

    public Dictionary<string, string> PathToContextTranslator => this.m_pathToContextTranslator;

    public static MyLocalization Static
    {
      get
      {
        if (MyLocalization.m_instance == null)
        {
          MyLocalization.m_instance = new MyLocalization();
          MyLocalization.m_instance.Init();
        }
        return MyLocalization.m_instance;
      }
    }

    public static void Initialize()
    {
      MyLocalization myLocalization = MyLocalization.Static;
    }

    private MyLocalization()
    {
    }

    private void Init()
    {
      foreach (string file in MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, MyLocalization.LOCALIZATION_FOLDER), "*.sbl", MySearchOption.AllDirectories))
        this.LoadLocalizationFile(file, MyStringId.NullOrEmpty);
    }

    public void InitLoader(Action loader)
    {
      MyTexts.RegisterEvaluator(MyLocalization.LOCALIZATION_TAG_CAMPAIGN, (ITextEvaluator) new MyLocalization.CampaignEvaluate(loader));
      MyTexts.RegisterEvaluator(MyLocalization.LOCALIZATION_TAG, (ITextEvaluator) new MyLocalization.UniversalEvaluate());
    }

    private MyLocalizationContext LoadLocalizationFile(
      string filePath,
      MyStringId bundleId,
      bool disposableContext = false)
    {
      if (!MyFileSystem.FileExists(filePath))
      {
        MyLog.Default.WriteLine("File does not exist: " + filePath);
        return (MyLocalizationContext) null;
      }
      MyObjectBuilder_Localization objectBuilder;
      if (!MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_Localization>(filePath, out objectBuilder))
        return (MyLocalizationContext) null;
      MyLocalizationContext orGetContext = this.CreateOrGetContext(MyStringId.GetOrCompute(objectBuilder.Context), disposableContext);
      orGetContext.InsertFileInfo(new MyLocalizationContext.LocalizationFileInfo(filePath, objectBuilder, bundleId));
      return orGetContext;
    }

    private MyLocalizationContext CreateOrGetContext(
      MyStringId contextId,
      bool disposable)
    {
      MyLocalizationContext localizationContext1 = (MyLocalizationContext) null;
      if (!disposable)
      {
        this.m_contexts.TryGetValue(contextId, out localizationContext1);
        if (localizationContext1 == null)
        {
          this.m_contexts.Add(contextId, localizationContext1 = new MyLocalizationContext(contextId));
          MyLocalizationContext localizationContext2;
          if (this.m_disposableContexts.TryGetValue(contextId, out localizationContext2))
          {
            localizationContext1.TwinContext = localizationContext2;
            localizationContext2.TwinContext = localizationContext1;
          }
        }
      }
      else
      {
        this.m_disposableContexts.TryGetValue(contextId, out localizationContext1);
        if (localizationContext1 == null)
        {
          this.m_disposableContexts.Add(contextId, localizationContext1 = new MyLocalizationContext(contextId));
          MyLocalizationContext localizationContext2;
          if (this.m_contexts.TryGetValue(contextId, out localizationContext2))
          {
            localizationContext1.TwinContext = localizationContext2;
            localizationContext2.TwinContext = localizationContext1;
          }
        }
      }
      return localizationContext1;
    }

    public void Switch(string language)
    {
      this.CurrentLanguage = language;
      foreach (MyLocalizationContext localizationContext in this.m_contexts.Values)
        localizationContext.Switch(language);
      foreach (MyLocalizationContext localizationContext in this.m_disposableContexts.Values)
        localizationContext.Switch(language);
    }

    public bool DisposeContext(MyStringId nameId)
    {
      MyLocalizationContext localizationContext;
      if (!this.m_disposableContexts.TryGetValue(nameId, out localizationContext))
        return false;
      localizationContext.Dispose();
      this.m_disposableContexts.Remove(nameId);
      return true;
    }

    public void DisposeAll()
    {
      this.m_disposableContexts.Values.ForEach<MyLocalizationContext>((Action<MyLocalizationContext>) (context => context.Dispose()));
      this.m_disposableContexts.Clear();
    }

    public void LoadBundle(
      MyLocalization.MyBundle bundle,
      HashSet<MyLocalizationContext> influencedContexts = null,
      bool disposableContexts = true)
    {
      if (this.m_loadedBundles.ContainsKey(bundle.BundleId))
      {
        this.NotifyBundleConflict(bundle.BundleId);
      }
      else
      {
        foreach (string filePath in bundle.FilePaths)
        {
          MyLocalizationContext localizationContext = this.LoadLocalizationFile(filePath, bundle.BundleId, true);
          if (localizationContext != null && influencedContexts != null)
            influencedContexts.Add(localizationContext);
          MyStringId name;
          if (localizationContext != null)
          {
            if (this.m_pathToContextTranslator.ContainsKey(filePath))
            {
              Dictionary<string, string> contextTranslator = this.m_pathToContextTranslator;
              string key = filePath;
              name = localizationContext.Name;
              string str = name.String;
              contextTranslator[key] = str;
            }
            else
            {
              Dictionary<string, string> contextTranslator = this.m_pathToContextTranslator;
              string key = filePath;
              name = localizationContext.Name;
              string str = name.String;
              contextTranslator.Add(key, str);
            }
          }
        }
      }
    }

    public void UnloadBundle(MyStringId bundleId)
    {
      foreach (MyLocalizationContext localizationContext in this.m_contexts.Values)
        localizationContext.UnloadBundle(bundleId);
      foreach (MyLocalizationContext localizationContext in this.m_disposableContexts.Values)
        localizationContext.UnloadBundle(bundleId);
    }

    public StringBuilder this[MyStringId contextName, MyStringId tag] => this.Get(contextName, tag);

    public StringBuilder this[string contexName, string tag] => this[MyStringId.GetOrCompute(contexName), MyStringId.GetOrCompute(tag)];

    public MyLocalizationContext this[MyStringId contextName]
    {
      get
      {
        MyLocalizationContext localizationContext;
        if (this.m_disposableContexts.TryGetValue(contextName, out localizationContext))
          return localizationContext;
        this.m_contexts.TryGetValue(contextName, out localizationContext);
        return localizationContext;
      }
    }

    public MyLocalizationContext this[string contextName] => this[MyStringId.GetOrCompute(contextName)];

    public StringBuilder Get(MyStringId contextId, MyStringId id)
    {
      StringBuilder stringBuilder = MyLocalization.m_defaultLocalization;
      MyLocalizationContext localizationContext;
      if (this.m_disposableContexts.TryGetValue(contextId, out localizationContext))
      {
        stringBuilder = localizationContext.Localize(id);
        if (stringBuilder != null)
          return stringBuilder;
      }
      if (this.m_contexts.TryGetValue(contextId, out localizationContext))
        stringBuilder = localizationContext.Localize(id);
      if (stringBuilder == null)
        stringBuilder = new StringBuilder();
      return stringBuilder;
    }

    private void NotifyBundleConflict(MyStringId bundleId) => MyLog.Default.WriteLine("MyLocalization: Bundle conflict - Bundle already loaded: " + bundleId.String);

    private class CampaignEvaluate : ITextEvaluator
    {
      private static Action m_loader;

      public CampaignEvaluate(Action loader) => MyLocalization.CampaignEvaluate.m_loader = loader;

      public string TokenEvaluate(string token, string context) => MyLocalization.CampaignEvaluate.Evaluate(token, context);

      public static string Evaluate(string token, string context, bool assert = true)
      {
        MyLocalizationContext localizationContext = MyLocalization.Static[context ?? "Common"];
        if (localizationContext == null)
          return "";
        if (localizationContext.IdsCount == 0)
        {
          MyLocalization.m_localizationLoadingLock.AcquireExclusive();
          if (localizationContext.IdsCount == 0)
            MyLocalization.CampaignEvaluate.m_loader();
          MyLocalization.m_localizationLoadingLock.ReleaseExclusive();
        }
        StringBuilder stringBuilder = localizationContext[MyStringId.GetOrCompute(token)];
        return stringBuilder != null ? stringBuilder.ToString() : "";
      }
    }

    private class UniversalEvaluate : ITextEvaluator
    {
      public string TokenEvaluate(string token, string context)
      {
        string str = MyLocalization.CampaignEvaluate.Evaluate(token, context, false);
        if (!string.IsNullOrEmpty(str))
          return str;
        StringBuilder stringBuilder = MyTexts.Get(MyStringId.GetOrCompute(token));
        return stringBuilder != null ? stringBuilder.ToString() : "";
      }
    }

    public struct MyBundle
    {
      public MyStringId BundleId;
      public List<string> FilePaths;
    }
  }
}
