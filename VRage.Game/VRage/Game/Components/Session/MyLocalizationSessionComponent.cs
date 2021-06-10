// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.Session.MyLocalizationSessionComponent
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using System.IO;
using VRage.FileSystem;
using VRage.Game.Localization;
using VRage.Game.ObjectBuilders.Components;
using VRage.Utils;

namespace VRage.Game.Components.Session
{
  [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 666, typeof (MyObjectBuilder_LocalizationSessionComponent), null, false)]
  public class MyLocalizationSessionComponent : MySessionComponentBase
  {
    public static readonly string MOD_BUNDLE_NAME = "MySession - Mod Bundle";
    public static readonly string CAMPAIGN_BUNDLE_NAME = "MySession - Campaing Bundle";
    private MyContentPath m_campaignModFolder;
    private MyLocalization.MyBundle m_modBundle;
    private MyLocalization.MyBundle m_campaignBundle;
    private readonly HashSet<MyLocalizationContext> m_influencedContexts = new HashSet<MyLocalizationContext>();

    public MyLocalizationSessionComponent()
    {
      this.m_modBundle.BundleId = MyStringId.GetOrCompute(MyLocalizationSessionComponent.MOD_BUNDLE_NAME);
      this.m_campaignBundle.BundleId = MyStringId.GetOrCompute(MyLocalizationSessionComponent.CAMPAIGN_BUNDLE_NAME);
      this.m_campaignBundle.FilePaths = new List<string>();
      this.m_modBundle.FilePaths = new List<string>();
    }

    public void LoadCampaignLocalization(IEnumerable<string> paths, string campaignModFolderPath = null)
    {
      MyContentPath myContentPath1 = new MyContentPath(campaignModFolderPath ?? MyFileSystem.ContentPath);
      this.m_campaignModFolder = (MyContentPath) campaignModFolderPath;
      this.m_campaignBundle.FilePaths.Clear();
      if ((string.IsNullOrEmpty(campaignModFolderPath) ? 0 : (MyFileSystem.IsDirectory(campaignModFolderPath) ? 1 : 0)) != 0)
        this.m_campaignBundle.FilePaths.Add(campaignModFolderPath);
      string path1 = string.IsNullOrEmpty(myContentPath1.Path) ? myContentPath1.RootFolder : myContentPath1.Path;
      foreach (string path in paths)
      {
        try
        {
          MyContentPath myContentPath2 = new MyContentPath(Path.Combine(path1, path));
          if (myContentPath2.AbsoluteFileExists)
            this.m_campaignBundle.FilePaths.Add(myContentPath2.Absolute);
          else if (myContentPath2.AlternateFileExists)
          {
            this.m_campaignBundle.FilePaths.Add(myContentPath2.AlternatePath);
          }
          else
          {
            foreach (string file in MyFileSystem.GetFiles(myContentPath2.AbsoluteDirExists ? myContentPath2.Absolute : myContentPath2.AlternatePath, "*.sbl", MySearchOption.AllDirectories))
              this.m_campaignBundle.FilePaths.Add(file);
          }
        }
        catch (Exception ex)
        {
          MyLog.Default.WriteLine("Wrong Path for localization component: " + path);
          MyLog.Default.WriteLine(ex.Message);
        }
      }
      if (this.m_campaignBundle.FilePaths.Count <= 0)
        return;
      MyLocalization.Static.LoadBundle(this.m_campaignBundle, this.m_influencedContexts);
    }

    public void ReloadLanguageBundles()
    {
      foreach (MyLocalizationContext influencedContext in this.m_influencedContexts)
        influencedContext.Switch(MyLocalization.Static.CurrentLanguage);
    }

    public override void BeforeStart()
    {
      foreach (MyObjectBuilder_Checkpoint.ModItem mod in this.Session.Mods)
      {
        string path = mod.GetPath();
        try
        {
          foreach (string file in MyFileSystem.GetFiles(path, "*.sbl", MySearchOption.AllDirectories))
            this.m_modBundle.FilePaths.Add(file);
        }
        catch (Exception ex)
        {
          MyLog.Default.WriteLine("MyLocalizationSessionComponent: Problem deserializing " + path + "\n" + (object) ex);
        }
      }
      MyLocalization.Static.LoadBundle(this.m_modBundle, this.m_influencedContexts);
      this.ReloadLanguageBundles();
    }

    protected override void UnloadData() => MyLocalization.Static.DisposeAll();

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      base.Init(sessionComponent);
      if (!(sessionComponent is MyObjectBuilder_LocalizationSessionComponent sessionComponent1))
        return;
      this.m_campaignModFolder = (MyContentPath) sessionComponent1.CampaignModFolderName;
      this.LoadCampaignLocalization((IEnumerable<string>) sessionComponent1.CampaignPaths, this.m_campaignModFolder.Absolute);
    }

    public override MyObjectBuilder_SessionComponent GetObjectBuilder()
    {
      if (base.GetObjectBuilder() is MyObjectBuilder_LocalizationSessionComponent objectBuilder)
      {
        objectBuilder.Language = MyLocalization.Static.CurrentLanguage;
        objectBuilder.CampaignModFolderName = this.m_campaignModFolder.ModFolder;
        foreach (string filePath in this.m_campaignBundle.FilePaths)
          objectBuilder.CampaignPaths.Add(filePath.Replace(MyFileSystem.ContentPath + "\\", ""));
      }
      return (MyObjectBuilder_SessionComponent) objectBuilder;
    }
  }
}
