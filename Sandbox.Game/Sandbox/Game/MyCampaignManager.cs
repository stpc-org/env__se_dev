// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.MyCampaignManager
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Engine.Networking;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components.Session;
using VRage.Game.Localization;
using VRage.Game.ObjectBuilders;
using VRage.Game.ObjectBuilders.Campaign;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.ObjectBuilders.VisualScripting;
using VRage.GameServices;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game
{
  public class MyCampaignManager
  {
    private const string CAMPAIGN_CONTENT_RELATIVE_PATH = "Campaigns";
    private readonly string m_scenariosContentRelativePath = "Scenarios";
    private readonly string m_scenarioFileExtension = "*.scf";
    private const string CAMPAIGN_DEBUG_RELATIVE_PATH = "Worlds\\Campaigns";
    private static MyCampaignManager m_instance;
    private string m_activeCampaignName;
    private MyObjectBuilder_Campaign m_activeCampaign;
    private readonly Dictionary<string, List<MyObjectBuilder_Campaign>> m_campaignsByNames = new Dictionary<string, List<MyObjectBuilder_Campaign>>();
    private readonly List<string> m_activeCampaignLevelNames = new List<string>();
    private Dictionary<string, MyLocalization.MyBundle> m_campaignMenuLocalizationBundle = new Dictionary<string, MyLocalization.MyBundle>();
    private readonly HashSet<MyLocalizationContext> m_campaignLocContexts = new HashSet<MyLocalizationContext>();
    private MyLocalization.MyBundle? m_currentMenuBundle;
    private readonly List<MyWorkshopItem> m_subscribedCampaignItems = new List<MyWorkshopItem>();
    private Task m_refreshTask;
    public static Action AfterCampaignLocalizationsLoaded;

    public static MyCampaignManager Static
    {
      get
      {
        if (MyCampaignManager.m_instance == null)
          MyCampaignManager.m_instance = new MyCampaignManager();
        return MyCampaignManager.m_instance;
      }
    }

    public static event Action OnActiveCampaignChanged;

    public (MyGameServiceCallResult, string) RefreshSubscribedModDataResult { get; private set; }

    public IEnumerable<MyObjectBuilder_Campaign> Campaigns
    {
      get
      {
        List<MyObjectBuilder_Campaign> objectBuilderCampaignList1 = new List<MyObjectBuilder_Campaign>();
        foreach (List<MyObjectBuilder_Campaign> objectBuilderCampaignList2 in this.m_campaignsByNames.Values)
          objectBuilderCampaignList1.AddRange((IEnumerable<MyObjectBuilder_Campaign>) objectBuilderCampaignList2);
        return (IEnumerable<MyObjectBuilder_Campaign>) objectBuilderCampaignList1;
      }
    }

    public IEnumerable<string> CampaignNames => (IEnumerable<string>) this.m_campaignsByNames.Keys;

    public IEnumerable<string> ActiveCampaignLevels => (IEnumerable<string>) this.m_activeCampaignLevelNames;

    public string ActiveCampaignName => this.m_activeCampaignName;

    public MyObjectBuilder_Campaign ActiveCampaign => this.m_activeCampaign;

    public string ActiveCampaingPlatform { get; private set; }

    public bool IsCampaignRunning
    {
      get
      {
        if (MySession.Static == null)
          return false;
        MyCampaignSessionComponent component = MySession.Static.GetComponent<MyCampaignSessionComponent>();
        return component != null && component.Running;
      }
    }

    public bool IsScenarioRunning
    {
      get
      {
        if (MySession.Static == null)
          return false;
        MyCampaignSessionComponent component = MySession.Static.GetComponent<MyCampaignSessionComponent>();
        return component != null && component.IsScenarioRunning;
      }
    }

    public IEnumerable<string> LocalizationLanguages => this.m_activeCampaign == null ? (IEnumerable<string>) null : (IEnumerable<string>) this.m_activeCampaign.LocalizationLanguages;

    public bool IsNewCampaignLevelLoading { get; private set; }

    public event Action OnCampaignFinished;

    public void Init()
    {
      MyLocalization.Static.InitLoader(new Action(this.LoadCampaignLocalization));
      MySandboxGame.Log.WriteLine("MyCampaignManager.Constructor() - START");
      foreach (string file in MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, this.m_scenariosContentRelativePath), this.m_scenarioFileExtension, MySearchOption.AllDirectories))
      {
        MyObjectBuilder_VSFiles objectBuilder;
        if (MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_VSFiles>(file, out objectBuilder) && objectBuilder.Campaign != null)
        {
          objectBuilder.Campaign.IsVanilla = true;
          objectBuilder.Campaign.IsLocalMod = false;
          objectBuilder.Campaign.CampaignPath = file;
          objectBuilder.Campaign.IsDebug = Path.GetDirectoryName(file).ToLower().EndsWith("_test");
          this.LoadCampaignData(objectBuilder.Campaign);
        }
      }
      foreach (string file in MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, "Worlds\\Campaigns"), "*.vs", MySearchOption.TopDirectoryOnly))
      {
        MyObjectBuilder_VSFiles objectBuilder;
        if (MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_VSFiles>(file, out objectBuilder) && objectBuilder.Campaign != null)
        {
          objectBuilder.Campaign.IsVanilla = true;
          objectBuilder.Campaign.IsLocalMod = false;
          objectBuilder.Campaign.IsDebug = true;
          this.LoadCampaignData(objectBuilder.Campaign);
        }
      }
      MySandboxGame.Log.WriteLine("MyCampaignManager.Constructor() - END");
    }

    public Task RefreshModData() => !this.m_refreshTask.IsComplete ? this.m_refreshTask : (this.m_refreshTask = Parallel.Start((Action) (() =>
    {
      this.RefreshLocalModData();
      if (!MyGameService.IsActive || !MyGameService.IsOnline)
        return;
      this.RefreshSubscribedModData();
    })));

    private void RefreshLocalModData()
    {
      string[] directories = Directory.GetDirectories(MyFileSystem.ModsPath);
      foreach (List<MyObjectBuilder_Campaign> objectBuilderCampaignList in this.m_campaignsByNames.Values)
        objectBuilderCampaignList.RemoveAll((Predicate<MyObjectBuilder_Campaign>) (campaign => campaign.IsLocalMod));
      foreach (string localModPath in directories)
        this.RegisterLocalModData(localModPath);
    }

    private void RegisterLocalModData(string localModPath)
    {
      foreach (string file in MyFileSystem.GetFiles(Path.Combine(localModPath, "Campaigns"), "*.vs", MySearchOption.TopDirectoryOnly))
        this.LoadScenarioFile(file);
      foreach (string file in MyFileSystem.GetFiles(Path.Combine(localModPath, this.m_scenariosContentRelativePath), this.m_scenarioFileExtension, MySearchOption.AllDirectories))
        this.LoadScenarioFile(file);
    }

    private void LoadScenarioFile(string modFile)
    {
      MyObjectBuilder_VSFiles objectBuilder;
      if (!MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_VSFiles>(modFile, out objectBuilder) || objectBuilder.Campaign == null)
        return;
      objectBuilder.Campaign.IsVanilla = false;
      objectBuilder.Campaign.IsLocalMod = true;
      objectBuilder.Campaign.ModFolderPath = this.GetModFolderPath(modFile);
      this.LoadCampaignData(objectBuilder.Campaign);
    }

    private void RefreshSubscribedModData()
    {
      (MyGameServiceCallResult, string) campaignsBlocking = MyWorkshop.GetSubscribedCampaignsBlocking(this.m_subscribedCampaignItems);
      List<MyObjectBuilder_Campaign> objectBuilderCampaignList1 = new List<MyObjectBuilder_Campaign>();
      foreach (List<MyObjectBuilder_Campaign> objectBuilderCampaignList2 in this.m_campaignsByNames.Values)
      {
        List<MyObjectBuilder_Campaign> campaignList = objectBuilderCampaignList2;
        foreach (MyObjectBuilder_Campaign objectBuilderCampaign in campaignList)
        {
          if (objectBuilderCampaign.PublishedFileId != 0UL)
          {
            bool flag = false;
            for (int index = 0; index < this.m_subscribedCampaignItems.Count; ++index)
            {
              MyWorkshopItem subscribedCampaignItem = this.m_subscribedCampaignItems[index];
              if ((long) subscribedCampaignItem.Id == (long) objectBuilderCampaign.PublishedFileId && subscribedCampaignItem.ServiceName == objectBuilderCampaign.PublishedServiceName)
              {
                this.m_subscribedCampaignItems.RemoveAtFast<MyWorkshopItem>(index);
                flag = true;
                break;
              }
            }
            if (!flag)
              objectBuilderCampaignList1.Add(objectBuilderCampaign);
          }
        }
        objectBuilderCampaignList1.ForEach((Action<MyObjectBuilder_Campaign>) (campaignToRemove => campaignList.Remove(campaignToRemove)));
        objectBuilderCampaignList1.Clear();
      }
      MyWorkshop.DownloadModsBlockingUGC(this.m_subscribedCampaignItems, (MyWorkshop.CancelToken) null);
      foreach (MyWorkshopItem subscribedCampaignItem in this.m_subscribedCampaignItems)
        this.RegisterWorshopModDataUGC(subscribedCampaignItem);
      this.RefreshSubscribedModDataResult = campaignsBlocking;
    }

    private void RegisterWorshopModDataUGC(MyWorkshopItem mod)
    {
      string folder = mod.Folder;
      IEnumerable<string> files1 = MyFileSystem.GetFiles(folder, "*.vs", MySearchOption.AllDirectories);
      this.LoadScenarioMod(mod, files1);
      IEnumerable<string> files2 = MyFileSystem.GetFiles(folder, this.m_scenarioFileExtension, MySearchOption.AllDirectories);
      this.LoadScenarioMod(mod, files2);
    }

    private void LoadScenarioMod(MyWorkshopItem mod, IEnumerable<string> visualScriptingFiles)
    {
      foreach (string visualScriptingFile in visualScriptingFiles)
      {
        MyObjectBuilder_VSFiles objectBuilder;
        if (MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_VSFiles>(visualScriptingFile, out objectBuilder) && objectBuilder.Campaign != null)
        {
          objectBuilder.Campaign.IsVanilla = false;
          objectBuilder.Campaign.IsLocalMod = false;
          objectBuilder.Campaign.PublishedFileId = mod.Id;
          objectBuilder.Campaign.PublishedServiceName = mod.ServiceName;
          objectBuilder.Campaign.ModFolderPath = this.GetModFolderPath(visualScriptingFile);
          this.LoadCampaignData(objectBuilder.Campaign);
        }
      }
    }

    public void PublishActive(string[] tags, string[] serviceNames)
    {
      WorkshopId[] publishedIds = MyWorkshop.GetWorkshopIdFromMod(this.m_activeCampaign.ModFolderPath);
      if (publishedIds == null)
        publishedIds = new WorkshopId[1]
        {
          new WorkshopId(0UL, MyGameService.GetDefaultUGC().ServiceName)
        };
      MyWorkshop.PublishModAsync(this.m_activeCampaign.ModFolderPath, this.m_activeCampaign.Name, this.m_activeCampaign.Description, MyWorkshop.FilterWorkshopIds(publishedIds, serviceNames), tags, MyPublishedFileVisibility.Public, new Action<bool, MyGameServiceCallResult, string, MyWorkshopItem[]>(this.OnPublishFinished));
    }

    private void OnPublishFinished(
      bool publishSuccess,
      MyGameServiceCallResult publishResult,
      string publishResultServiceName,
      MyWorkshopItem[] publishedFiles)
    {
      if (publishedFiles.Length != 0)
        MyWorkshop.GenerateModInfoLocal(this.m_activeCampaign.ModFolderPath, publishedFiles, Sync.MyId);
      MyWorkshop.ReportPublish(publishedFiles, publishResult, publishResultServiceName);
    }

    private string GetModFolderPath(string path)
    {
      int num = path.IndexOf("Campaigns", StringComparison.InvariantCulture);
      if (num == -1)
        num = path.IndexOf(this.m_scenariosContentRelativePath, StringComparison.InvariantCulture);
      return path.Remove(num - 1);
    }

    private void LoadCampaignData(MyObjectBuilder_Campaign campaignOb)
    {
      if (!MyDLCs.IsDLCSupported(campaignOb.DLC))
        return;
      bool isCompatible = true;
      MyLocalCache.GetSessionPathFromScenarioObjectBuilder(campaignOb, "", false, out isCompatible);
      if (!isCompatible)
        return;
      if (campaignOb.PublishedFileId != 0UL && campaignOb.PublishedServiceName == null)
        campaignOb.PublishedServiceName = MyGameService.GetDefaultUGC().ServiceName;
      if (this.m_campaignsByNames.ContainsKey(campaignOb.Name))
      {
        List<MyObjectBuilder_Campaign> campaignsByName = this.m_campaignsByNames[campaignOb.Name];
        foreach (MyObjectBuilder_Campaign objectBuilderCampaign in campaignsByName)
        {
          if (objectBuilderCampaign.IsLocalMod == campaignOb.IsLocalMod && objectBuilderCampaign.IsMultiplayer == campaignOb.IsMultiplayer && (objectBuilderCampaign.IsVanilla == campaignOb.IsVanilla && (long) objectBuilderCampaign.PublishedFileId == (long) campaignOb.PublishedFileId) && objectBuilderCampaign.PublishedServiceName == campaignOb.PublishedServiceName)
            return;
        }
        campaignsByName.Add(campaignOb);
      }
      else
      {
        this.m_campaignsByNames.Add(campaignOb.Name, new List<MyObjectBuilder_Campaign>());
        this.m_campaignsByNames[campaignOb.Name].Add(campaignOb);
      }
      if (string.IsNullOrEmpty(campaignOb.DescriptionLocalizationFile))
        return;
      FileInfo fileInfo = new FileInfo(Path.Combine(campaignOb.ModFolderPath ?? MyFileSystem.ContentPath, campaignOb.DescriptionLocalizationFile));
      if (!fileInfo.Exists)
        return;
      string[] files = Directory.GetFiles(fileInfo.Directory.FullName, Path.GetFileNameWithoutExtension(fileInfo.Name) + "*.sbl", SearchOption.TopDirectoryOnly);
      string str1 = string.IsNullOrEmpty(campaignOb.ModFolderPath) ? campaignOb.Name : Path.Combine(campaignOb.ModFolderPath, campaignOb.Name);
      MyLocalization.MyBundle myBundle = new MyLocalization.MyBundle()
      {
        BundleId = MyStringId.GetOrCompute(str1),
        FilePaths = new List<string>()
      };
      foreach (string str2 in files)
      {
        if (!myBundle.FilePaths.Contains(str2))
          myBundle.FilePaths.Add(str2);
      }
      if (this.m_campaignMenuLocalizationBundle.ContainsKey(str1))
        this.m_campaignMenuLocalizationBundle[str1] = myBundle;
      else
        this.m_campaignMenuLocalizationBundle.Add(str1, myBundle);
    }

    public CloudResult LoadSessionFromActiveCampaign(
      string relativePath,
      Action afterLoad = null,
      string campaignDirectoryName = null,
      string campaignName = null,
      MyOnlineModeEnum onlineMode = MyOnlineModeEnum.OFFLINE,
      int maxPlayers = 0,
      bool runAsInstance = true)
    {
      MyLog.Default.WriteLine(nameof (LoadSessionFromActiveCampaign));
      string path2 = relativePath;
      string path1;
      if (this.m_activeCampaign.IsVanilla || this.m_activeCampaign.IsDebug)
      {
        path1 = Path.Combine(MyFileSystem.ContentPath, path2);
        if (!MyFileSystem.FileExists(path1))
        {
          MySandboxGame.Log.WriteLine("ERROR: Missing vanilla world file in campaign: " + this.m_activeCampaignName);
          return CloudResult.Failed;
        }
      }
      else
      {
        path1 = Path.Combine(this.m_activeCampaign.ModFolderPath, path2);
        if (!MyFileSystem.FileExists(path1))
        {
          path1 = Path.Combine(MyFileSystem.ContentPath, path2);
          if (!MyFileSystem.FileExists(path1))
          {
            MySandboxGame.Log.WriteLine("ERROR: Missing world file in campaign: " + this.m_activeCampaignName);
            return CloudResult.Failed;
          }
        }
      }
      DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(path1));
      string str1 = directoryInfo.FullName;
      string str2 = (string) null;
      if (runAsInstance)
      {
        bool flag = true;
        if (string.IsNullOrEmpty(campaignDirectoryName))
        {
          campaignDirectoryName = this.ActiveCampaignName + " " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
          flag = false;
        }
        if (MyPlatformGameSettings.GAME_SAVES_TO_CLOUD)
        {
          string str3 = directoryInfo.Name;
          while (true)
          {
            str2 = campaignDirectoryName + " " + str3;
            List<MyCloudFileInfo> myCloudFileInfoList = (List<MyCloudFileInfo>) null;
            if (flag)
              myCloudFileInfoList = MyGameService.GetCloudFiles(str2);
            if (myCloudFileInfoList != null && myCloudFileInfoList.Count != 0)
              str3 = directoryInfo.Name + " " + MyUtils.GetRandomInt(int.MaxValue).ToString("########");
            else
              break;
          }
          str1 = Path.Combine(MyFileSystem.SavesPath, str2);
          if (MyFileSystem.DirectoryExists(str1))
            Directory.Delete(str1, true);
        }
        else
        {
          str1 = Path.Combine(MyFileSystem.SavesPath, campaignDirectoryName, directoryInfo.Name);
          while (MyFileSystem.DirectoryExists(str1))
            str1 = Path.Combine(MyFileSystem.SavesPath, campaignDirectoryName, directoryInfo.Name + " " + MyUtils.GetRandomInt(int.MaxValue).ToString("########"));
        }
        MyUtils.CopyDirectory(directoryInfo.FullName, str1);
        if (this.m_activeCampaign != null)
        {
          string path3 = Path.Combine(str1, Path.GetFileName(path1));
          MyObjectBuilder_Checkpoint objectBuilder;
          if (MyFileSystem.FileExists(path3) && MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_Checkpoint>(path3, out objectBuilder))
          {
            foreach (MyObjectBuilder_SessionComponent sessionComponent1 in objectBuilder.SessionComponents)
            {
              if (sessionComponent1 is MyObjectBuilder_LocalizationSessionComponent sessionComponent)
              {
                sessionComponent.CampaignModFolderName = this.m_activeCampaign.ModFolderPath;
                break;
              }
            }
            MyObjectBuilderSerializer.SerializeXML(path3, false, (MyObjectBuilder_Base) objectBuilder);
          }
        }
        if (MyPlatformGameSettings.GAME_SAVES_TO_CLOUD)
        {
          CloudResult cloudResult = MyCloudHelper.UploadFiles(MyCloudHelper.LocalToCloudWorldPath(str1), str1, MyPlatformGameSettings.GAME_SAVES_COMPRESSED_BY_DEFAULT);
          if (cloudResult != CloudResult.Ok)
            return cloudResult;
        }
      }
      if (!string.IsNullOrEmpty(MyLanguage.CurrentCultureName))
        afterLoad += (Action) (() =>
        {
          MyLocalizationSessionComponent component = MySession.Static.GetComponent<MyLocalizationSessionComponent>();
          component.LoadCampaignLocalization((IEnumerable<string>) this.m_activeCampaign.LocalizationPaths, this.m_activeCampaign.ModFolderPath);
          component.ReloadLanguageBundles();
          MyCampaignManager.AfterCampaignLocalizationsLoaded.InvokeIfNotNull();
        });
      afterLoad += (Action) (() =>
      {
        MySession.Static.Save((string) null);
        this.IsNewCampaignLevelLoading = false;
      });
      this.IsNewCampaignLevelLoading = true;
      if (MyLocalization.Static != null)
        MyLocalization.Static.DisposeAll();
      if (Sync.IsDedicated)
      {
        MyWorkshop.CancelToken cancelToken = new MyWorkshop.CancelToken();
        MySessionLoader.LoadDedicatedSession(str1, cancelToken, afterLoad);
      }
      else
        MySessionLoader.LoadSingleplayerSession(str1, afterLoad, campaignName, new MyOnlineModeEnum?(onlineMode), maxPlayers, str2);
      return CloudResult.Ok;
    }

    public void LoadCampaignLocalization()
    {
      if (MySession.Static == null)
        return;
      MyLocalizationSessionComponent component = MySession.Static.GetComponent<MyLocalizationSessionComponent>();
      if (component == null || this.m_activeCampaign == null)
        return;
      component.LoadCampaignLocalization((IEnumerable<string>) this.m_activeCampaign.LocalizationPaths, this.m_activeCampaign.ModFolderPath);
      component.ReloadLanguageBundles();
    }

    public bool SwitchCampaign(
      string name,
      bool isVanilla = true,
      ulong publisherFileId = 0,
      string publisherServiceName = null,
      string localModFolder = null,
      string platform = null)
    {
      if (this.m_campaignsByNames.ContainsKey(name))
      {
        this.ActiveCampaingPlatform = platform;
        foreach (MyObjectBuilder_Campaign objectBuilderCampaign in this.m_campaignsByNames[name])
        {
          if (objectBuilderCampaign.IsVanilla == isVanilla && ((objectBuilderCampaign.IsLocalMod ? 1 : 0) == (localModFolder == null ? 0 : (publisherFileId == 0UL ? 1 : 0)) && (long) objectBuilderCampaign.PublishedFileId == (long) publisherFileId && objectBuilderCampaign.PublishedServiceName == publisherServiceName))
          {
            this.m_activeCampaign = objectBuilderCampaign;
            this.m_activeCampaignName = name;
            this.m_activeCampaignLevelNames.Clear();
            MyLog.Default.WriteLine("Switching active campaign: " + this.m_activeCampaign?.Name);
            foreach (MyObjectBuilder_CampaignSMNode node in this.m_activeCampaign.GetStateMachine(platform).Nodes)
              this.m_activeCampaignLevelNames.Add(node.Name);
            MyCampaignManager.OnActiveCampaignChanged.InvokeIfNotNull();
            return true;
          }
          if (publisherFileId == 0UL && objectBuilderCampaign.PublishedFileId != 0UL)
          {
            publisherFileId = objectBuilderCampaign.PublishedFileId;
            return true;
          }
        }
      }
      if (publisherFileId != 0UL)
      {
        if (this.DownloadCampaign(publisherFileId, publisherServiceName))
          return this.SwitchCampaign(name, isVanilla, publisherFileId, publisherServiceName, localModFolder, platform);
      }
      else if (!isVanilla && localModFolder != null && MyFileSystem.DirectoryExists(localModFolder))
      {
        this.RegisterLocalModData(localModFolder);
        return this.SwitchCampaign(name, isVanilla, publisherFileId, publisherServiceName, localModFolder, platform);
      }
      return false;
    }

    public void SetExperimentalCampaign(MyObjectBuilder_Checkpoint checkpoint)
    {
      MyObjectBuilder_CampaignSessionComponent sessionComponent = checkpoint.SessionComponents.OfType<MyObjectBuilder_CampaignSessionComponent>().FirstOrDefault<MyObjectBuilder_CampaignSessionComponent>();
      if (sessionComponent == null || !sessionComponent.IsVanilla)
        return;
      checkpoint.Settings.ExperimentalMode = true;
      sessionComponent.IsVanilla = false;
    }

    public bool IsCampaign(MyObjectBuilder_CampaignSessionComponent ob) => this.IsCampaign(ob.CampaignName, ob.IsVanilla, ob.Mod.PublishedFileId);

    public bool IsCampaign(string campaignName, bool isVanilla, ulong modPublishedFileId) => ((string.IsNullOrEmpty(campaignName) ? 0 : (this.m_campaignsByNames.ContainsKey(campaignName) ? 1 : 0)) & (isVanilla ? 1 : 0)) != 0 && modPublishedFileId == 0UL;

    public void Unload()
    {
      this.m_activeCampaign = (MyObjectBuilder_Campaign) null;
      this.m_activeCampaignName = (string) null;
      this.m_activeCampaignLevelNames.Clear();
    }

    public bool DownloadCampaign(ulong publisherFileId, string publisherServiceName)
    {
      new MyWorkshop.ResultData().Success = false;
      MyWorkshopItem workshopItem = MyGameService.CreateWorkshopItem(publisherServiceName);
      workshopItem.Id = publisherFileId;
      MyWorkshop.ResultData resultData = MyWorkshop.DownloadModsBlockingUGC(new List<MyWorkshopItem>((IEnumerable<MyWorkshopItem>) new MyWorkshopItem[1]
      {
        workshopItem
      }), (MyWorkshop.CancelToken) null);
      if (!resultData.Success || resultData.Mods.Count == 0)
        return false;
      this.RegisterWorshopModDataUGC(resultData.Mods[0]);
      return true;
    }

    public void ReloadMenuLocalization(string name)
    {
      if (this.m_currentMenuBundle.HasValue)
      {
        MyLocalization.Static.UnloadBundle(this.m_currentMenuBundle.Value.BundleId);
        this.m_campaignLocContexts.Clear();
      }
      if (!this.m_campaignMenuLocalizationBundle.ContainsKey(name))
        return;
      this.m_currentMenuBundle = new MyLocalization.MyBundle?(this.m_campaignMenuLocalizationBundle[name]);
      if (!this.m_currentMenuBundle.HasValue)
        return;
      MyLocalization.Static.LoadBundle(this.m_currentMenuBundle.Value, this.m_campaignLocContexts, false);
      foreach (MyLocalizationContext campaignLocContext in this.m_campaignLocContexts)
        campaignLocContext.Switch(MyLanguage.CurrentCultureName);
    }

    public CloudResult RunNewCampaign(
      string campaignName,
      MyOnlineModeEnum onlineMode,
      int maxPlayers,
      string platform,
      bool runAsInstance = true)
    {
      if (this.m_activeCampaign == null)
        return CloudResult.Failed;
      MyObjectBuilder_CampaignSM stateMachine = this.m_activeCampaign.GetStateMachine(platform);
      MyObjectBuilder_CampaignSMNode builderCampaignSmNode = stateMachine != null ? this.FindStartingState(stateMachine) : (MyObjectBuilder_CampaignSMNode) null;
      if (builderCampaignSmNode == null)
        return CloudResult.Failed;
      int num = MySandboxGame.Config.ExperimentalMode ? stateMachine.MaxLobbyPlayersExperimental : stateMachine.MaxLobbyPlayers;
      if (num > 0 && !Sync.IsDedicated)
      {
        if (num == 1)
          onlineMode = MyOnlineModeEnum.OFFLINE;
        else
          maxPlayers = num;
      }
      return this.LoadSessionFromActiveCampaign(builderCampaignSmNode.SaveFilePath, campaignName: campaignName, onlineMode: onlineMode, maxPlayers: maxPlayers, runAsInstance: runAsInstance);
    }

    public void RunCampaign(string path, bool runAsInstance = true, string platform = null)
    {
      MyObjectBuilder_Campaign objectBuilderCampaign = (MyObjectBuilder_Campaign) null;
      MyObjectBuilder_VSFiles objectBuilder;
      if (MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_VSFiles>(path, out objectBuilder) && objectBuilder.Campaign != null)
      {
        objectBuilder.Campaign.IsVanilla = false;
        objectBuilder.Campaign.IsLocalMod = true;
        this.LoadCampaignData(objectBuilder.Campaign);
        objectBuilderCampaign = objectBuilder.Campaign;
      }
      if (objectBuilderCampaign == null || Sync.IsDedicated && !objectBuilder.Campaign.IsMultiplayer)
        return;
      this.ActiveCampaingPlatform = platform;
      if (!MyCampaignManager.Static.SwitchCampaign(objectBuilderCampaign.Name, objectBuilderCampaign.IsVanilla, objectBuilderCampaign.PublishedFileId, objectBuilderCampaign.PublishedServiceName, objectBuilderCampaign.ModFolderPath = this.GetModFolderPath(path)))
        return;
      MyOnlineModeEnum onlineMode = objectBuilderCampaign.IsMultiplayer ? MyOnlineModeEnum.PUBLIC : MyOnlineModeEnum.OFFLINE;
      int maxPlayers = objectBuilderCampaign.MaxPlayers;
      int num = (int) MyCampaignManager.Static.RunNewCampaign(objectBuilderCampaign.Name, onlineMode, maxPlayers, platform, runAsInstance);
    }

    private MyObjectBuilder_CampaignSMNode FindStartingState(
      MyObjectBuilder_CampaignSM stateMachine)
    {
      bool flag = false;
      foreach (MyObjectBuilder_CampaignSMNode node in stateMachine.Nodes)
      {
        foreach (MyObjectBuilder_CampaignSMTransition transition in stateMachine.Transitions)
        {
          if (transition.To == node.Name)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          return node;
        flag = false;
      }
      return (MyObjectBuilder_CampaignSMNode) null;
    }

    public void NotifyCampaignFinished()
    {
      Action campaignFinished = this.OnCampaignFinished;
      if (campaignFinished == null)
        return;
      campaignFinished();
    }
  }
}
