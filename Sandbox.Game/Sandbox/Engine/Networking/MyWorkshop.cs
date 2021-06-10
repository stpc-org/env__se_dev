// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Networking.MyWorkshop
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Mvvm;
using ParallelTasks;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.ViewModels;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.Compression;
using VRage.FileSystem;
using VRage.Game;
using VRage.GameServices;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Engine.Networking
{
  public static class MyWorkshop
  {
    private const int MOD_NAME_LIMIT = 25;
    private const string MOD_INFO_FILE = "modinfo.sbmi";
    private static MyWorkshopItemPublisher m_publisher;
    private static MyGuiScreenDownloadMods m_downloadScreen;
    private static MyWorkshop.DownloadModsResult m_downloadResult;
    private static readonly int m_dependenciesRequestTimeout = 30000;
    private static readonly string m_workshopWorldsDir = "WorkshopWorlds";
    private static readonly string m_workshopWorldsPath = Path.Combine(MyFileSystem.UserDataPath, MyWorkshop.m_workshopWorldsDir);
    private static readonly string m_workshopWorldSuffix = ".sbw";
    private static readonly string m_workshopBlueprintsPath = Path.Combine(MyFileSystem.UserDataPath, "Blueprints", "workshop");
    private static readonly string m_workshopBlueprintSuffix = ".sbb";
    private static readonly string m_workshopScriptPath = Path.Combine(MyFileSystem.UserDataPath, "IngameScripts", "workshop");
    private static readonly string m_workshopModsPath = MyFileSystem.ModsPath;
    public static readonly string WorkshopModSuffix = "_legacy.bin";
    private static readonly string m_workshopScenariosPath = Path.Combine(MyFileSystem.UserDataPath, "Scenarios", "workshop");
    private static readonly string m_workshopScenariosSuffix = ".sbs";
    private static readonly string[] m_previewFileNames = new string[2]
    {
      "thumb.png",
      MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION
    };
    private const string ModMetadataFileName = "metadata.mod";
    private static readonly HashSet<string> m_ignoredExecutableExtensions = new HashSet<string>((IEnumerable<string>) new string[48]
    {
      ".action",
      ".apk",
      ".app",
      ".bat",
      ".bin",
      ".cmd",
      ".com",
      ".command",
      ".cpl",
      ".csh",
      ".dll",
      ".exe",
      ".gadget",
      ".inf1",
      ".ins",
      ".inx",
      ".ipa",
      ".isu",
      ".job",
      ".jse",
      ".ksh",
      ".lnk",
      ".msc",
      ".msi",
      ".msp",
      ".mst",
      ".osx",
      ".out",
      ".pif",
      ".paf",
      ".prg",
      ".ps1",
      ".reg",
      ".rgs",
      ".run",
      ".sct",
      ".shb",
      ".shs",
      ".so",
      ".u3p",
      ".vb",
      ".vbe",
      ".vbs",
      ".vbscript",
      ".workflow",
      ".ws",
      ".wsf",
      ".suo"
    });
    private static readonly HashSet<string> m_scriptExtensions = new HashSet<string>()
    {
      ".cs"
    };
    private static readonly int m_bufferSize = 1048576;
    private static byte[] buffer = new byte[MyWorkshop.m_bufferSize];
    private static MyWorkshop.Category[] m_modCategories;
    private static MyWorkshop.Category[] m_worldCategories;
    private static MyWorkshop.Category[] m_blueprintCategories;
    private static MyWorkshop.Category[] m_scenarioCategories;
    private static MyWorkshop.Category[] m_scriptCategories;
    public const string WORKSHOP_DEVELOPMENT_TAG = "development";
    public const string WORKSHOP_WORLD_TAG = "world";
    public const string WORKSHOP_CAMPAIGN_TAG = "campaign";
    public const string WORKSHOP_MOD_TAG = "mod";
    public const string WORKSHOP_BLUEPRINT_TAG = "blueprint";
    public const string WORKSHOP_SCENARIO_TAG = "scenario";
    public const string WORKSHOP_INGAMESCRIPT_TAG = "ingameScript";
    private static FastResourceLock m_modLock = new FastResourceLock();
    private static string ContentTag;
    private static Action SubscriptionChangedCallback;
    private static Action<bool, MyGameServiceCallResult, string, MyWorkshopItem[]> m_onPublishingFinished;
    private static MyGuiScreenProgressAsync m_asyncPublishScreen;
    private static MyWorkshop.CancelToken m_cancelTokenDownloadMods;

    public static MyWorkshop.Category[] ModCategories => MyWorkshop.m_modCategories;

    public static MyWorkshop.Category[] WorldCategories => MyWorkshop.m_worldCategories;

    public static MyWorkshop.Category[] BlueprintCategories => MyWorkshop.m_blueprintCategories;

    public static MyWorkshop.Category[] ScenarioCategories => MyWorkshop.m_scenarioCategories;

    public static MyWorkshop.Category[] ScriptCategories => MyWorkshop.m_scriptCategories;

    public static void Init(
      MyWorkshop.Category[] modCategories,
      MyWorkshop.Category[] worldCategories,
      MyWorkshop.Category[] blueprintCategories,
      MyWorkshop.Category[] scenarioCategories,
      MyWorkshop.Category[] scriptCategories)
    {
      MyWorkshop.m_modCategories = modCategories;
      MyWorkshop.m_worldCategories = worldCategories;
      MyWorkshop.m_blueprintCategories = blueprintCategories;
      MyWorkshop.m_scenarioCategories = scenarioCategories;
      MyWorkshop.m_scriptCategories = scriptCategories;
    }

    public static void PublishModAsync(
      string localModFolder,
      string publishedTitle,
      string publishedDescription,
      WorkshopId[] workshopIds,
      string[] tags,
      MyPublishedFileVisibility visibility,
      Action<bool, MyGameServiceCallResult, string, MyWorkshopItem[]> callbackOnFinished = null)
    {
      MyWorkshop.m_onPublishingFinished = callbackOnFinished;
      HashSet<string> ignoredPaths = new HashSet<string>();
      ignoredPaths.Add("modinfo.sbmi");
      MyStringId textUploadingWorld = MyCommonTexts.ProgressTextUploadingWorld;
      MyStringId? cancelText = new MyStringId?();
      Func<IMyAsyncResult> beginAction = (Func<IMyAsyncResult>) (() => (IMyAsyncResult) new MyWorkshop.PublishItemResult(localModFolder, publishedTitle, publishedDescription, workshopIds, visibility, tags, MyWorkshop.m_ignoredExecutableExtensions, ignoredPaths));
      Action<IMyAsyncResult, MyGuiScreenProgressAsync> endAction = new Action<IMyAsyncResult, MyGuiScreenProgressAsync>(MyWorkshop.endActionPublish);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) (MyWorkshop.m_asyncPublishScreen = new MyGuiScreenProgressAsync(textUploadingWorld, cancelText, beginAction, endAction)));
    }

    public static WorkshopId[] GetWorkshopIdFromLocalMod(
      string localModFolder,
      WorkshopId additionalWorkshopId)
    {
      return MyWorkshop.GetWorkshopIdFromLocalModInternal(Path.Combine(MyFileSystem.ModsPath, localModFolder, "modinfo.sbmi"), additionalWorkshopId);
    }

    public static WorkshopId[] GetWorkshopIdFromMod(string modFolder) => MyWorkshop.GetWorkshopIdFromLocalModInternal(Path.Combine(modFolder, "modinfo.sbmi"), new WorkshopId());

    public static WorkshopId[] GetWorkshopIdFromLocalBlueprint(
      string localFolder,
      WorkshopId additionalWorkshopId)
    {
      return MyWorkshop.GetWorkshopIdFromLocalModInternal(Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, localFolder, "modinfo.sbmi"), additionalWorkshopId);
    }

    public static WorkshopId[] GetWorkshopIdFromLocalScript(
      string localFolder,
      WorkshopId additionalWorkshopId)
    {
      return MyWorkshop.GetWorkshopIdFromLocalModInternal(Path.Combine(MyBlueprintUtils.SCRIPT_FOLDER_LOCAL, localFolder, "modinfo.sbmi"), additionalWorkshopId);
    }

    private static WorkshopId[] GetWorkshopIdFromLocalModInternal(
      string modInfoPath,
      WorkshopId additionalWorkshopId)
    {
      MyObjectBuilder_ModInfo objectBuilder;
      if (File.Exists(modInfoPath) && MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_ModInfo>(modInfoPath, out objectBuilder))
      {
        if (objectBuilder.WorkshopIds != null)
        {
          if (additionalWorkshopId.Id == 0UL)
            return objectBuilder.WorkshopIds;
          List<WorkshopId> list = ((IEnumerable<WorkshopId>) objectBuilder.WorkshopIds).ToList<WorkshopId>();
          int index = list.FindIndex((Predicate<WorkshopId>) (x => x.ServiceName == additionalWorkshopId.ServiceName));
          if (index != -1)
            list[index] = additionalWorkshopId;
          else
            list.Add(additionalWorkshopId);
          return list.ToArray();
        }
        if (objectBuilder.WorkshopId != 0UL)
        {
          string serviceName = MyGameService.GetDefaultUGC().ServiceName;
          if (additionalWorkshopId.Id != 0UL)
          {
            if (additionalWorkshopId.ServiceName == serviceName)
              return new WorkshopId[1]
              {
                additionalWorkshopId
              };
            return new WorkshopId[2]
            {
              new WorkshopId()
              {
                Id = objectBuilder.WorkshopId,
                ServiceName = MyGameService.GetDefaultUGC().ServiceName
              },
              additionalWorkshopId
            };
          }
          return new WorkshopId[1]
          {
            new WorkshopId(objectBuilder.WorkshopId, MyGameService.GetDefaultUGC().ServiceName)
          };
        }
      }
      if (additionalWorkshopId.Id == 0UL)
        return new WorkshopId[0];
      return new WorkshopId[1]{ additionalWorkshopId };
    }

    public static void PublishWorldAsync(
      string localWorldFolder,
      string publishedTitle,
      string publishedDescription,
      WorkshopId[] workshopIds,
      string[] tags,
      MyPublishedFileVisibility visibility,
      Action<bool, MyGameServiceCallResult, string, MyWorkshopItem[]> callbackOnFinished = null)
    {
      MyWorkshop.m_onPublishingFinished = callbackOnFinished;
      HashSet<string> ignoredExtensions = new HashSet<string>((IEnumerable<string>) MyWorkshop.m_ignoredExecutableExtensions);
      ignoredExtensions.Add(".xmlcache");
      ignoredExtensions.Add(".png");
      HashSet<string> ignoredPaths = new HashSet<string>();
      ignoredPaths.Add("Backup");
      MyStringId textUploadingWorld = MyCommonTexts.ProgressTextUploadingWorld;
      MyStringId? cancelText = new MyStringId?();
      Func<IMyAsyncResult> beginAction = (Func<IMyAsyncResult>) (() => (IMyAsyncResult) new MyWorkshop.PublishItemResult(localWorldFolder, publishedTitle, publishedDescription, workshopIds, visibility, tags, ignoredExtensions, ignoredPaths));
      Action<IMyAsyncResult, MyGuiScreenProgressAsync> endAction = new Action<IMyAsyncResult, MyGuiScreenProgressAsync>(MyWorkshop.endActionPublish);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) (MyWorkshop.m_asyncPublishScreen = new MyGuiScreenProgressAsync(textUploadingWorld, cancelText, beginAction, endAction)));
    }

    public static void PublishBlueprintAsync(
      string localWorldFolder,
      string publishedTitle,
      string publishedDescription,
      WorkshopId[] workshopIds,
      string[] tags,
      uint[] requiredDLCs,
      MyPublishedFileVisibility visibility,
      Action<bool, MyGameServiceCallResult, string, MyWorkshopItem[]> callbackOnFinished = null)
    {
      MyWorkshop.m_onPublishingFinished = callbackOnFinished;
      MyStringId textUploadingWorld = MyCommonTexts.ProgressTextUploadingWorld;
      MyStringId? cancelText = new MyStringId?();
      Func<IMyAsyncResult> beginAction = (Func<IMyAsyncResult>) (() => (IMyAsyncResult) new MyWorkshop.PublishItemResult(localWorldFolder, publishedTitle, publishedDescription, workshopIds, visibility, tags, MyWorkshop.m_ignoredExecutableExtensions, requiredDLCs: requiredDLCs));
      Action<IMyAsyncResult, MyGuiScreenProgressAsync> endAction = new Action<IMyAsyncResult, MyGuiScreenProgressAsync>(MyWorkshop.endActionPublish);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) (MyWorkshop.m_asyncPublishScreen = new MyGuiScreenProgressAsync(textUploadingWorld, cancelText, beginAction, endAction)));
    }

    public static void PublishScenarioAsync(
      string localWorldFolder,
      string publishedTitle,
      string publishedDescription,
      WorkshopId[] workshopIds,
      MyPublishedFileVisibility visibility,
      Action<bool, MyGameServiceCallResult, string, MyWorkshopItem[]> callbackOnFinished = null)
    {
      MyWorkshop.m_onPublishingFinished = callbackOnFinished;
      string[] tags = new string[1]{ "scenario" };
      MyStringId textUploadingWorld = MyCommonTexts.ProgressTextUploadingWorld;
      MyStringId? cancelText = new MyStringId?();
      Func<IMyAsyncResult> beginAction = (Func<IMyAsyncResult>) (() => (IMyAsyncResult) new MyWorkshop.PublishItemResult(localWorldFolder, publishedTitle, publishedDescription, workshopIds, visibility, tags, MyWorkshop.m_ignoredExecutableExtensions));
      Action<IMyAsyncResult, MyGuiScreenProgressAsync> endAction = new Action<IMyAsyncResult, MyGuiScreenProgressAsync>(MyWorkshop.endActionPublish);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) (MyWorkshop.m_asyncPublishScreen = new MyGuiScreenProgressAsync(textUploadingWorld, cancelText, beginAction, endAction)));
    }

    public static void PublishIngameScriptAsync(
      string localWorldFolder,
      string publishedTitle,
      string publishedDescription,
      WorkshopId[] workshopIds,
      string[] tags,
      MyPublishedFileVisibility visibility,
      Action<bool, MyGameServiceCallResult, string, MyWorkshopItem[]> callbackOnFinished = null)
    {
      MyWorkshop.m_onPublishingFinished = callbackOnFinished;
      HashSet<string> ignoredExtensions = new HashSet<string>((IEnumerable<string>) MyWorkshop.m_ignoredExecutableExtensions);
      ignoredExtensions.Add(".sbmi");
      MyStringId textUploadingWorld = MyCommonTexts.ProgressTextUploadingWorld;
      MyStringId? cancelText = new MyStringId?();
      Func<IMyAsyncResult> beginAction = (Func<IMyAsyncResult>) (() => (IMyAsyncResult) new MyWorkshop.PublishItemResult(localWorldFolder, publishedTitle, publishedDescription, workshopIds, visibility, tags, ignoredExtensions));
      Action<IMyAsyncResult, MyGuiScreenProgressAsync> endAction = new Action<IMyAsyncResult, MyGuiScreenProgressAsync>(MyWorkshop.endActionPublish);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) (MyWorkshop.m_asyncPublishScreen = new MyGuiScreenProgressAsync(textUploadingWorld, cancelText, beginAction, endAction)));
    }

    private static (MyGameServiceCallResult, string) PublishItemBlocking(
      string localFolder,
      string publishedTitle,
      string publishedDescription,
      WorkshopId[] workshopIds,
      MyPublishedFileVisibility visibility,
      string[] tags,
      HashSet<string> ignoredExtensions,
      HashSet<string> ignoredPaths,
      uint[] requiredDLCs,
      out MyWorkshopItem[] publishedItems)
    {
      MySandboxGame.Log.WriteLine("PublishItemBlocking - START");
      MySandboxGame.Log.IncreaseIndent();
      publishedItems = Array.Empty<MyWorkshopItem>();
      if (tags.Length == 0)
      {
        MySandboxGame.Log.WriteLine("Error: Can not publish with no tags!");
        MySandboxGame.Log.DecreaseIndent();
        MySandboxGame.Log.WriteLine("PublishItemBlocking - END");
        return (MyGameServiceCallResult.InvalidParam, (string) null);
      }
      if (!MyGameService.IsActive && !MyGameService.IsOnline)
        return (MyGameServiceCallResult.NoUser, (string) null);
      using (AutoResetEvent resetEvent = new AutoResetEvent(false))
      {
        bool publishPossible = false;
        MyGameService.Service.RequestPermissions(Permissions.ShareContent, true, (Action<bool>) (granted =>
        {
          if (granted)
          {
            MyGameService.Service.RequestPermissions(Permissions.UGC, true, (Action<bool>) (ugcGranted =>
            {
              publishPossible = ugcGranted;
              resetEvent.Set();
            }));
          }
          else
          {
            publishPossible = false;
            resetEvent.Set();
          }
        }));
        resetEvent.WaitOne();
        if (!publishPossible)
          return (MyGameServiceCallResult.PlatformPublishRestricted, (string) null);
      }
      List<MyWorkshopItem> myWorkshopItemList = new List<MyWorkshopItem>();
      (MyGameServiceCallResult, string) valueTuple = (MyGameServiceCallResult.OK, "");
      foreach (WorkshopId workshopId in workshopIds)
      {
        MyWorkshopItem publishedItem;
        MyGameServiceCallResult serviceCallResult = MyWorkshop.PublishItemBlocking(localFolder, publishedTitle, publishedDescription, workshopId.Id, workshopId.ServiceName, visibility, tags, ignoredExtensions, ignoredPaths, requiredDLCs, out publishedItem);
        if (publishedItem != null)
          myWorkshopItemList.Add(publishedItem);
        if (serviceCallResult != MyGameServiceCallResult.OK)
          valueTuple = (serviceCallResult, workshopId.ServiceName);
      }
      if (myWorkshopItemList.Count > 0)
        publishedItems = myWorkshopItemList.ToArray();
      return valueTuple;
    }

    private static MyGameServiceCallResult PublishItemBlocking(
      string localFolder,
      string publishedTitle,
      string publishedDescription,
      ulong workshopId,
      string serviceName,
      MyPublishedFileVisibility visibility,
      string[] tags,
      HashSet<string> ignoredExtensions,
      HashSet<string> ignoredPaths,
      uint[] requiredDLCs,
      out MyWorkshopItem publishedItem)
    {
      publishedItem = (MyWorkshopItem) null;
      MyWorkshopItemPublisher workshopPublisher = MyGameService.CreateWorkshopPublisher(serviceName);
      if (workshopPublisher == null)
        return MyGameServiceCallResult.ServiceUnavailable;
      if (workshopId != 0UL)
      {
        List<MyWorkshopItem> myWorkshopItemList = new List<MyWorkshopItem>();
        if (MyWorkshop.GetItemsBlockingUGC(new List<WorkshopId>()
        {
          new WorkshopId(workshopId, serviceName)
        }, myWorkshopItemList))
        {
          if (myWorkshopItemList.Count != 0)
          {
            MyWorkshopItem myWorkshopItem = myWorkshopItemList.FirstOrDefault<MyWorkshopItem>((Func<MyWorkshopItem, bool>) (wi => (long) wi.Id == (long) workshopId));
            if (myWorkshopItem != null)
              publishedTitle = myWorkshopItem.Title;
          }
          else
            workshopId = 0UL;
        }
        else
          workshopId = 0UL;
      }
      workshopPublisher.Title = publishedTitle;
      workshopPublisher.Description = publishedDescription;
      workshopPublisher.Visibility = visibility;
      workshopPublisher.Tags = new List<string>((IEnumerable<string>) tags);
      workshopPublisher.Id = workshopId;
      string filename = Path.Combine(localFolder, "metadata.mod");
      MyModMetadata mod = (MyModMetadata) MyModMetadataLoader.Load(filename);
      MyWorkshop.CheckAndFixModMetadata(ref mod);
      MyModMetadataLoader.Save(filename, (ModMetadataFile) mod);
      bool flag1 = MyWorkshop.CheckModFolder(ref localFolder, ignoredExtensions, ignoredPaths);
      workshopPublisher.Folder = localFolder;
      workshopPublisher.Metadata = mod;
      foreach (string previewFileName in MyWorkshop.m_previewFileNames)
      {
        string path = Path.Combine(localFolder, previewFileName);
        if (File.Exists(path))
        {
          workshopPublisher.Thumbnail = path;
          break;
        }
      }
      if (workshopPublisher.Tags.Contains("mod"))
      {
        bool flag2 = false;
        foreach (string enumerateFile in Directory.EnumerateFiles(workshopPublisher.Folder, "*", SearchOption.AllDirectories))
        {
          if (MyWorkshop.m_scriptExtensions.Contains(Path.GetExtension(enumerateFile)))
          {
            flag2 = true;
            break;
          }
        }
        if (!flag2)
          workshopPublisher.Tags.Add(MySteamConstants.TAG_NO_SCRIPTS);
      }
      try
      {
        MyGameServiceCallResult publishResult = MyGameServiceCallResult.Fail;
        using (AutoResetEvent resetEvent = new AutoResetEvent(false))
        {
          MyWorkshop.m_publisher = workshopPublisher;
          MyWorkshopItem publishedItemLocal = (MyWorkshopItem) null;
          MyWorkshop.m_publisher.ItemPublished += (MyWorkshopItemPublisher.PublishItemResult) ((result, item) =>
          {
            publishResult = result;
            if (result == MyGameServiceCallResult.OK)
              MySandboxGame.Log.WriteLine("Published file update successful");
            else
              MySandboxGame.Log.WriteLine(string.Format("Error during publishing: {0}", (object) result));
            if (result == MyGameServiceCallResult.OK)
              publishedItemLocal = item;
            resetEvent.Set();
          });
          if (requiredDLCs != null)
          {
            foreach (uint requiredDlC in requiredDLCs)
              MyWorkshop.m_publisher.DLCs.Add(requiredDlC);
          }
          MyWorkshop.m_publisher.Publish();
          resetEvent.WaitOne();
          publishedItem = publishedItemLocal;
        }
        return publishResult;
      }
      finally
      {
        MyWorkshop.m_publisher = (MyWorkshopItemPublisher) null;
        if (flag1 && localFolder.StartsWith(Path.GetTempPath()))
          Directory.Delete(localFolder, true);
      }
    }

    private static bool CheckModFolder(
      ref string localFolder,
      HashSet<string> ignoredExtensions,
      HashSet<string> ignoredPaths)
    {
      if ((ignoredExtensions == null || ignoredExtensions.Count == 0) && (ignoredPaths == null || ignoredPaths.Count == 0))
        return false;
      string str = Path.Combine(Path.GetTempPath(), string.Format("{0}-{1}", (object) Process.GetCurrentProcess().Id, (object) Path.GetFileName(localFolder)));
      if (Directory.Exists(str))
        Directory.Delete(str, true);
      localFolder = MyFileSystem.TerminatePath(localFolder);
      int sourcePathLength = localFolder.Length;
      MyFileSystem.CopyAll(localFolder, str, (Predicate<string>) (s =>
      {
        if (ignoredExtensions != null)
        {
          string extension = Path.GetExtension(s);
          if (extension != null && ignoredExtensions.Contains(extension))
            return false;
        }
        return ignoredPaths == null || !ignoredPaths.Contains(s.Substring(sourcePathLength));
      }));
      localFolder = str;
      return true;
    }

    public static MyModCompatibility CheckModCompatibility(string localFullPath)
    {
      if (string.IsNullOrWhiteSpace(localFullPath))
        return MyModCompatibility.Unknown;
      string str = Path.Combine(localFullPath, "metadata.mod");
      return !MyFileSystem.FileExists(str) ? MyModCompatibility.Unknown : MyWorkshop.CheckModCompatibility((MyModMetadata) MyModMetadataLoader.Load(str));
    }

    public static MyModCompatibility CheckModCompatibility(MyModMetadata mod) => MyModCompatibility.Ok;

    private static void CheckAndFixModMetadata(ref MyModMetadata mod)
    {
      if (mod == null)
        mod = new MyModMetadata();
      if (!(mod.ModVersion == (Version) null))
        return;
      mod.ModVersion = new Version(1, 0);
    }

    private static void endActionPublish(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
    {
      MyWorkshop.PublishItemResult publishItemResult = result as MyWorkshop.PublishItemResult;
      screen.CloseScreenNow();
      if (MyWorkshop.m_onPublishingFinished != null)
      {
        Action<bool, MyGameServiceCallResult, string, MyWorkshopItem[]> publishingFinished = MyWorkshop.m_onPublishingFinished;
        MyWorkshopItem[] publishedItems1 = publishItemResult.PublishedItems;
        int num = publishedItems1 != null ? ((uint) publishedItems1.Length > 0U ? 1 : 0) : 0;
        int result1 = (int) publishItemResult.Result;
        string resultServiceName = publishItemResult.ResultServiceName;
        MyWorkshopItem[] publishedItems2 = publishItemResult.PublishedItems;
        publishingFinished(num != 0, (MyGameServiceCallResult) result1, resultServiceName, publishedItems2);
      }
      MyWorkshop.m_onPublishingFinished = (Action<bool, MyGameServiceCallResult, string, MyWorkshopItem[]>) null;
      MyWorkshop.m_asyncPublishScreen = (MyGuiScreenProgressAsync) null;
    }

    public static (MyGameServiceCallResult, string) GetSubscribedWorldsBlocking(
      List<MyWorkshopItem> results)
    {
      MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedWorldsBlocking - START");
      try
      {
        return MyWorkshop.GetSubscribedItemsBlockingUGC(results, new string[1]
        {
          "world"
        });
      }
      finally
      {
        MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedWorldsBlocking - END");
      }
    }

    public static (MyGameServiceCallResult, string) GetSubscribedCampaignsBlocking(
      List<MyWorkshopItem> results)
    {
      MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedWorldsBlocking - START");
      try
      {
        return MyWorkshop.GetSubscribedItemsBlockingUGC(results, new string[1]
        {
          "campaign"
        });
      }
      finally
      {
        MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedWorldsBlocking - END");
      }
    }

    public static (MyGameServiceCallResult, string) GetSubscribedModsBlocking(
      List<MyWorkshopItem> results)
    {
      MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedModsBlocking - START");
      try
      {
        return MyWorkshop.GetSubscribedItemsBlockingUGC(results, new string[1]
        {
          "mod"
        });
      }
      finally
      {
        MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedModsBlocking - END");
      }
    }

    public static (MyGameServiceCallResult, string) GetSubscribedScenariosBlocking(
      List<MyWorkshopItem> results)
    {
      MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedScenariosBlocking - START");
      try
      {
        return MyWorkshop.GetSubscribedItemsBlockingUGC(results, new string[1]
        {
          "scenario"
        });
      }
      finally
      {
        MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedScenariosBlocking - END");
      }
    }

    public static (MyGameServiceCallResult, string) GetSubscribedBlueprintsBlocking(
      List<MyWorkshopItem> results)
    {
      MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedModsBlocking - START");
      try
      {
        return MyWorkshop.GetSubscribedItemsBlockingUGC(results, new string[1]
        {
          "blueprint"
        });
      }
      finally
      {
        MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedModsBlocking - END");
      }
    }

    public static (MyGameServiceCallResult, string) GetSubscribedIngameScriptsBlocking(
      List<MyWorkshopItem> results)
    {
      MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedModsBlocking - START");
      try
      {
        return MyWorkshop.GetSubscribedItemsBlockingUGC(results, new string[1]
        {
          "ingameScript"
        });
      }
      finally
      {
        MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedModsBlocking - END");
      }
    }

    private static Dictionary<string, List<ulong>> ToDictionary(
      IEnumerable<WorkshopId> workshopIds)
    {
      Dictionary<string, List<ulong>> dictionary = new Dictionary<string, List<ulong>>();
      foreach (WorkshopId workshopId in workshopIds)
      {
        string key = workshopId.ServiceName ?? MyGameService.GetDefaultUGC().ServiceName;
        List<ulong> ulongList;
        if (!dictionary.TryGetValue(key, out ulongList))
          dictionary.Add(key, ulongList = new List<ulong>());
        ulongList.Add(workshopId.Id);
      }
      return dictionary;
    }

    public static void GetItemsAsync(
      List<WorkshopId> items,
      Action<bool, List<MyWorkshopItem>> onDone)
    {
      Parallel.Start((Action) (() =>
      {
        List<MyWorkshopItem> resultDestination = new List<MyWorkshopItem>();
        onDone.InvokeIfNotNull<bool, List<MyWorkshopItem>>(MyWorkshop.GetItemsBlockingUGC(items, resultDestination), resultDestination);
      }));
    }

    public static bool GetItemsBlockingUGC(
      List<WorkshopId> workshopIds,
      List<MyWorkshopItem> resultDestination)
    {
      if (!MyGameService.IsOnline && !Sandbox.Engine.Platform.Game.IsDedicated)
        return false;
      if (workshopIds.Count == 0)
        return true;
      Dictionary<string, List<ulong>> dictionary = MyWorkshop.ToDictionary((IEnumerable<WorkshopId>) workshopIds);
      MySandboxGame.Log.WriteLine(string.Format("MyWorkshop.GetItemsBlocking: getting {0} items", (object) workshopIds.Count));
      resultDestination.Clear();
      foreach (KeyValuePair<string, List<ulong>> keyValuePair in dictionary)
      {
        string serviceName = keyValuePair.Key ?? MyGameService.GetDefaultUGC().ServiceName;
        MyWorkshopQuery workshopQuery = MyGameService.CreateWorkshopQuery(serviceName);
        if (workshopQuery == null)
        {
          MySandboxGame.Log.WriteLine("Unknown UGC service name: " + serviceName);
        }
        else
        {
          workshopQuery.ItemIds = keyValuePair.Value;
          using (AutoResetEvent resetEvent = new AutoResetEvent(false))
          {
            workshopQuery.QueryCompleted += (MyWorkshopQuery.QueryCompletedDelegate) (result =>
            {
              if (result == MyGameServiceCallResult.OK)
                MySandboxGame.Log.WriteLine("Mod query successful");
              else
                MySandboxGame.Log.WriteLine(string.Format("Error during mod query: {0}", (object) result));
              resetEvent.Set();
            });
            workshopQuery.Run();
            if (!resetEvent.WaitOne())
              return false;
          }
          if (workshopQuery.Items != null)
            resultDestination.AddRange((IEnumerable<MyWorkshopItem>) workshopQuery.Items);
        }
      }
      return true;
    }

    private static (MyGameServiceCallResult, string) GetSubscribedItemsBlockingUGC(
      List<MyWorkshopItem> results,
      string[] tags)
    {
      (MyGameServiceCallResult, string) valueTuple = (MyGameServiceCallResult.OK, (string) null);
      results.Clear();
      foreach (IMyUGCService aggregate in MyGameService.WorkshopService.GetAggregates())
      {
        if (aggregate.IsConsentGiven)
        {
          MyGameServiceCallResult blockingUgcInternal = MyWorkshop.GetSubscribedItemsBlockingUGCInternal(aggregate.ServiceName, results, (IEnumerable<string>) tags);
          if (blockingUgcInternal != MyGameServiceCallResult.OK)
            valueTuple = (blockingUgcInternal, aggregate.ServiceName);
        }
      }
      return valueTuple;
    }

    private static MyGameServiceCallResult GetSubscribedItemsBlockingUGCInternal(
      string serviceName,
      List<MyWorkshopItem> results,
      IEnumerable<string> tags)
    {
      if (!MyGameService.IsActive)
        return MyGameServiceCallResult.NoUser;
      MyWorkshopQuery workshopQuery = MyGameService.CreateWorkshopQuery(serviceName);
      workshopQuery.UserId = Sync.MyId;
      if (tags != null)
      {
        if (workshopQuery.RequiredTags == null)
          workshopQuery.RequiredTags = new List<string>();
        workshopQuery.RequiredTags.AddRange(tags);
      }
      MyGameServiceCallResult queryResult = MyGameServiceCallResult.Fail;
      using (AutoResetEvent resetEvent = new AutoResetEvent(false))
      {
        workshopQuery.QueryCompleted += (MyWorkshopQuery.QueryCompletedDelegate) (result =>
        {
          queryResult = result;
          if (result == MyGameServiceCallResult.OK)
            MySandboxGame.Log.WriteLine("Query successful.");
          else
            MySandboxGame.Log.WriteLine(string.Format("Error during UGC query: {0}", (object) result));
          resetEvent.Set();
        });
        workshopQuery.Run();
        if (MyFakes.FORCE_NO_WORKER)
        {
          while (!resetEvent.WaitOne(0))
            MyGameService.Update();
        }
        else if (!resetEvent.WaitOne())
          return MyGameServiceCallResult.AccessDenied;
      }
      if (workshopQuery.Items != null)
        results.AddRange((IEnumerable<MyWorkshopItem>) workshopQuery.Items);
      return queryResult;
    }

    public static void DownloadModsAsync(
      List<MyObjectBuilder_Checkpoint.ModItem> mods,
      Action<bool> onFinishedCallback,
      Action onCancelledCallback = null)
    {
      if (mods == null || mods.Count == 0)
      {
        onFinishedCallback(true);
      }
      else
      {
        if (!Directory.Exists(MyWorkshop.m_workshopModsPath))
          Directory.CreateDirectory(MyWorkshop.m_workshopModsPath);
        MyWorkshop.m_cancelTokenDownloadMods = new MyWorkshop.CancelToken();
        MyGameService.Service.RequestPermissions(Permissions.UGC, true, (Action<bool>) (granted =>
        {
          if (granted)
            MyWorkshop.StartDownloadModsAsync(mods, onFinishedCallback, onCancelledCallback);
          else
            onCancelledCallback();
        }));
      }
    }

    private static void StartDownloadModsAsync(
      List<MyObjectBuilder_Checkpoint.ModItem> mods,
      Action<bool> onFinishedCallback,
      Action onCancelledCallback = null)
    {
      MyWorkshop.m_downloadScreen = new MyGuiScreenDownloadMods((Action<MyGuiScreenMessageBox.ResultEnum>) (r =>
      {
        MyWorkshop.m_cancelTokenDownloadMods.Cancel = true;
        if (onCancelledCallback == null)
          return;
        onCancelledCallback();
      }));
      MyWorkshop.m_downloadScreen.Closed += new MyGuiScreenBase.ScreenHandler(MyWorkshop.OnDownloadScreenClosed);
      MyWorkshop.m_downloadResult = new MyWorkshop.DownloadModsResult(mods, onFinishedCallback, MyWorkshop.m_cancelTokenDownloadMods);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyWorkshop.m_downloadScreen);
    }

    private static void FixModServiceName(List<MyObjectBuilder_Checkpoint.ModItem> mods)
    {
      for (int index = 0; index < mods.Count; ++index)
      {
        MyObjectBuilder_Checkpoint.ModItem mod = mods[index];
        if (string.IsNullOrEmpty(mod.PublishedServiceName))
        {
          mod.PublishedServiceName = MyGameService.GetDefaultUGC().ServiceName;
          mods[index] = mod;
        }
      }
    }

    private static void OnDownloadScreenClosed(MyGuiScreenBase source, bool isUnloading)
    {
      if (MyWorkshop.m_cancelTokenDownloadMods == null)
        return;
      MyWorkshop.m_cancelTokenDownloadMods.Cancel = true;
    }

    private static void endActionDownloadMods()
    {
      MyWorkshop.m_downloadScreen.CloseScreen();
      if (!MyWorkshop.m_downloadResult.Result.Success)
        MySandboxGame.Log.WriteLine(string.Format("Error downloading mods"));
      MyWorkshop.m_downloadResult.Callback(MyWorkshop.m_downloadResult.Result.Success);
    }

    public static MyWorkshop.ResultData DownloadModsBlockingUGC(
      List<MyWorkshopItem> mods,
      MyWorkshop.CancelToken cancelToken)
    {
      int counter = 0;
      string numMods = mods.Count.ToString();
      CachingList<MyWorkshopItem> cachingList1 = new CachingList<MyWorkshopItem>();
      CachingList<MyWorkshopItem> cachingList2 = new CachingList<MyWorkshopItem>();
      List<KeyValuePair<MyWorkshopItem, string>> currentMods = new List<KeyValuePair<MyWorkshopItem, string>>();
      bool flag = false;
      long timestamp1 = Stopwatch.GetTimestamp();
      double byteSize = 0.0;
      for (int index = 0; index < mods.Count; ++index)
        byteSize += (double) mods[index].Size;
      string str = MyUtils.FormatByteSizePrefix(ref byteSize);
      string sizeStr = byteSize.ToString("N1") + str + "B";
      double runningTotal = 0.0;
      foreach (MyWorkshopItem mod1 in mods)
      {
        MyWorkshopItem mod = mod1;
        if (!MyGameService.IsOnline)
          flag = true;
        else if (cancelToken != null && cancelToken.Cancel)
        {
          flag = true;
        }
        else
        {
          MyWorkshop.UpdateDownloadScreen(counter, numMods, currentMods, sizeStr, runningTotal, mod);
          if (!MyWorkshop.UpdateMod(mod))
          {
            MySandboxGame.Log.WriteLineAndConsole(string.Format("Mod failed: Id = {0}, title = '{1}'", (object) mod.Id, (object) mod.Title));
            cachingList1.Add(mod);
            flag = true;
            if (cancelToken != null)
              cancelToken.Cancel = true;
          }
          else
          {
            MySandboxGame.Log.WriteLineAndConsole(string.Format("Up to date mod: Id = {0}, title = '{1}'", (object) mod.Id, (object) mod.Title));
            if (MyWorkshop.m_downloadScreen != null)
            {
              using (MyWorkshop.m_modLock.AcquireExclusiveUsing())
              {
                runningTotal += (double) mod.Size;
                ++counter;
                currentMods.RemoveAll((Predicate<KeyValuePair<MyWorkshopItem, string>>) (e => e.Key == mod));
              }
            }
          }
        }
      }
      long timestamp2 = Stopwatch.GetTimestamp();
      if (flag)
      {
        cachingList1.ApplyChanges();
        if (cachingList1.Count > 0)
        {
          foreach (MyWorkshopItem myWorkshopItem in cachingList1)
            MySandboxGame.Log.WriteLineAndConsole(string.Format("Failed to download mod: Id = {0}, title = '{1}'", (object) myWorkshopItem.Id, (object) myWorkshopItem.Title));
        }
        else if (cancelToken == null || !cancelToken.Cancel)
          MySandboxGame.Log.WriteLineAndConsole(string.Format("Failed to download mods because service is not in Online Mode."));
        else
          MySandboxGame.Log.WriteLineAndConsole(string.Format("Failed to download mods because download was stopped."));
        return new MyWorkshop.ResultData();
      }
      cachingList2.ApplyChanges();
      MyWorkshop.ResultData resultData = new MyWorkshop.ResultData()
      {
        Success = true,
        MismatchMods = new List<MyWorkshopItem>((IEnumerable<MyWorkshopItem>) cachingList2),
        Mods = new List<MyWorkshopItem>((IEnumerable<MyWorkshopItem>) mods)
      };
      double num = (double) (timestamp2 - timestamp1) / (double) Stopwatch.Frequency;
      MySandboxGame.Log.WriteLineAndConsole(string.Format("Mod download time: {0:0.00} seconds", (object) num));
      return resultData;
    }

    private static void UpdateDownloadScreen(
      int counter,
      string numMods,
      List<KeyValuePair<MyWorkshopItem, string>> currentMods,
      string sizeStr,
      double runningTotal,
      MyWorkshopItem mod)
    {
      if (MyWorkshop.m_downloadScreen == null)
        return;
      string str1;
      if (mod.Title.Length <= 25)
      {
        str1 = mod.Title;
      }
      else
      {
        string str2 = mod.Title.Substring(0, 25);
        int length = str2.LastIndexOf(' ');
        if (length != -1)
          str2 = str2.Substring(0, length);
        str1 = str2 + "...";
      }
      StringBuilder stringBuilder = new StringBuilder();
      using (MyWorkshop.m_modLock.AcquireExclusiveUsing())
      {
        double byteSize = runningTotal;
        string str2 = MyUtils.FormatByteSizePrefix(ref byteSize);
        double size = (double) mod.Size;
        string str3 = MyUtils.FormatByteSizePrefix(ref size);
        currentMods.Add(new KeyValuePair<MyWorkshopItem, string>(mod, str1 + " " + size.ToString("N1") + str3 + "B"));
        stringBuilder.AppendLine();
        foreach (KeyValuePair<MyWorkshopItem, string> currentMod in currentMods)
          stringBuilder.AppendLine(currentMod.Value);
        stringBuilder.AppendLine(MyTexts.GetString(MyCommonTexts.DownloadingMods_Completed) + (object) counter + "/" + numMods + " : " + byteSize.ToString("N1") + str2 + "B/" + sizeStr);
      }
      MySandboxGame.Static.Invoke("MySteamWorkshop::set loading text", (object) stringBuilder, (Action<object>) (text => MyWorkshop.m_downloadScreen.MessageText = (StringBuilder) text));
    }

    public static bool DownloadScriptBlocking(MyWorkshopItem item)
    {
      if (!MyGameService.IsOnline)
        return false;
      if (!MyWorkshop.IsUpToDate(item))
      {
        if (!MyWorkshop.UpdateMod(item))
          return false;
      }
      else
        MySandboxGame.Log.WriteLineAndConsole(string.Format("Up to date mod: Id = {0}, title = '{1}'", (object) item.Id, (object) item.Title));
      return true;
    }

    public static bool DownloadBlueprintBlockingUGC(MyWorkshopItem item, bool check = true)
    {
      if (!check || !item.IsUpToDate())
      {
        if (!MyWorkshop.UpdateMod(item))
          return false;
      }
      else
        MySandboxGame.Log.WriteLineAndConsole(string.Format("Up to date mod: Id = {0}, title = '{1}'", (object) item.Id, (object) item.Title));
      return true;
    }

    public static bool IsUpToDate(MyWorkshopItem item)
    {
      if (!MyGameService.IsOnline)
        return false;
      item.UpdateState();
      return item.IsUpToDate();
    }

    public static MyWorkshop.ResultData DownloadWorldModsBlocking(
      List<MyObjectBuilder_Checkpoint.ModItem> mods,
      MyWorkshop.CancelToken cancelToken)
    {
      MyWorkshop.ResultData ret = new MyWorkshop.ResultData();
      Task task = Parallel.Start((Action) (() => ret = MyWorkshop.DownloadWorldModsBlockingInternal(mods, cancelToken)));
      while (!task.IsComplete)
      {
        MyGameService.Update();
        Thread.Sleep(10);
      }
      return ret;
    }

    public static MyWorkshop.ResultData DownloadWorldModsBlockingInternal(
      List<MyObjectBuilder_Checkpoint.ModItem> mods,
      MyWorkshop.CancelToken cancelToken)
    {
      MyWorkshop.ResultData ret = new MyWorkshop.ResultData();
      ret.Success = true;
      if (!MyFakes.ENABLE_WORKSHOP_MODS)
      {
        if (cancelToken != null)
          ret.Cancel = cancelToken.Cancel;
        return ret;
      }
      MySandboxGame.Log.WriteLineAndConsole("Downloading world mods - START");
      MySandboxGame.Log.IncreaseIndent();
      if (mods != null && mods.Count > 0)
      {
        MyWorkshop.FixModServiceName(mods);
        List<WorkshopId> workshopIds = new List<WorkshopId>();
        foreach (MyObjectBuilder_Checkpoint.ModItem mod in mods)
        {
          if (mod.PublishedFileId != 0UL)
          {
            WorkshopId workshopId = new WorkshopId(mod.PublishedFileId, mod.PublishedServiceName);
            if (!workshopIds.Contains(workshopId))
              workshopIds.Add(workshopId);
          }
          else if (Sandbox.Engine.Platform.Game.IsDedicated)
          {
            MySandboxGame.Log.WriteLineAndConsole("Local mods are not allowed in multiplayer.");
            MySandboxGame.Log.DecreaseIndent();
            return new MyWorkshop.ResultData();
          }
        }
        workshopIds.Sort();
        bool flag = false;
        if (MyPlatformGameSettings.CONSOLE_COMPATIBLE)
        {
          foreach (WorkshopId workshopId in workshopIds)
          {
            IMyUGCService aggregate = MyGameService.WorkshopService.GetAggregate(workshopId.ServiceName);
            if (aggregate == null)
            {
              flag = true;
              MySandboxGame.Log.WriteLineAndConsole(string.Format("Can't download mod {0}. Service {1} is not available", (object) workshopId.Id, (object) workshopId.ServiceName));
            }
            else if (!aggregate.IsConsoleCompatible)
            {
              flag = true;
              MySandboxGame.Log.WriteLineAndConsole(string.Format("Can't download mod {0}. Service {1} is not console compatible", (object) workshopId.Id, (object) aggregate.ServiceName));
            }
          }
        }
        if (flag)
          ret.Success = false;
        else if (Sandbox.Engine.Platform.Game.IsDedicated)
        {
          if (MySandboxGame.ConfigDedicated.AutodetectDependencies)
            MyWorkshop.AddModDependencies(mods, workshopIds);
          MyGameService.SetServerModTemporaryDirectory();
          ret = MyWorkshop.DownloadModsBlocking(mods, ret, workshopIds, cancelToken);
        }
        else
        {
          if (Sync.IsServer)
            MyWorkshop.AddModDependencies(mods, workshopIds);
          ret = MyWorkshop.DownloadModsBlocking(mods, ret, workshopIds, cancelToken);
        }
      }
      MySandboxGame.Log.DecreaseIndent();
      MySandboxGame.Log.WriteLineAndConsole("Downloading world mods - END");
      if (cancelToken != null)
        ret.Cancel |= cancelToken.Cancel;
      return ret;
    }

    private static void AddModDependencies(
      List<MyObjectBuilder_Checkpoint.ModItem> mods,
      List<WorkshopId> workshopIds)
    {
      HashSet<WorkshopId> workshopIds1 = new HashSet<WorkshopId>();
      HashSet<WorkshopId> workshopIdSet = new HashSet<WorkshopId>();
      foreach (MyObjectBuilder_Checkpoint.ModItem mod in mods)
      {
        WorkshopId workshopId = new WorkshopId(mod.PublishedFileId, mod.PublishedServiceName);
        workshopIdSet.Add(workshopId);
        if (!mod.IsDependency && mod.PublishedFileId != 0UL)
          workshopIds1.Add(workshopId);
      }
      foreach (MyWorkshopItem myWorkshopItem in MyWorkshop.GetModsDependencyHiearchy(workshopIds1, out bool _))
      {
        WorkshopId workshopId = new WorkshopId(myWorkshopItem.Id, myWorkshopItem.ServiceName);
        if (workshopIdSet.Add(workshopId))
          mods.Add(new MyObjectBuilder_Checkpoint.ModItem(myWorkshopItem.Id, myWorkshopItem.ServiceName, true)
          {
            FriendlyName = myWorkshopItem.Title
          });
        if (!workshopIds.Contains(workshopId))
          workshopIds.Add(workshopId);
      }
    }

    public static List<MyWorkshopItem> GetModsDependencyHiearchy(
      HashSet<WorkshopId> workshopIds,
      out bool hasReferenceIssue)
    {
      hasReferenceIssue = false;
      List<MyWorkshopItem> myWorkshopItemList = new List<MyWorkshopItem>();
      HashSet<WorkshopId> workshopIdSet = new HashSet<WorkshopId>();
      List<WorkshopId> workshopIds1 = new List<WorkshopId>();
      Stack<WorkshopId> workshopIdStack = new Stack<WorkshopId>();
      foreach (WorkshopId workshopId in workshopIds)
        workshopIdStack.Push(workshopId);
      while (workshopIdStack.Count > 0)
      {
        while (workshopIdStack.Count > 0)
        {
          WorkshopId workshopId = workshopIdStack.Pop();
          if (!workshopIdSet.Contains(workshopId))
          {
            workshopIdSet.Add(workshopId);
            workshopIds1.Add(workshopId);
          }
          else
          {
            hasReferenceIssue = true;
            MyLog.Default.WriteLineAndConsole(string.Format("Reference issue detected (circular reference or wrong order) for mod {0}:{1}", (object) workshopId.ServiceName, (object) workshopId.Id));
          }
        }
        if (workshopIds1.Count != 0)
        {
          List<MyWorkshopItem> modsInfo = MyWorkshop.GetModsInfo(workshopIds1);
          if (modsInfo != null)
          {
            foreach (MyWorkshopItem myWorkshopItem in modsInfo)
            {
              myWorkshopItemList.Insert(0, myWorkshopItem);
              myWorkshopItem.UpdateDependencyBlocking();
              for (int index = myWorkshopItem.Dependencies.Count - 1; index >= 0; --index)
              {
                ulong dependency = myWorkshopItem.Dependencies[index];
                workshopIdStack.Push(new WorkshopId(dependency, myWorkshopItem.ServiceName));
              }
            }
          }
          workshopIds1.Clear();
        }
      }
      return myWorkshopItemList;
    }

    public static List<MyWorkshopItem> GetModsInfo(List<WorkshopId> workshopIds)
    {
      Dictionary<string, List<ulong>> dictionary = MyWorkshop.ToDictionary((IEnumerable<WorkshopId>) workshopIds);
      List<MyWorkshopItem> myWorkshopItemList = (List<MyWorkshopItem>) null;
      foreach (KeyValuePair<string, List<ulong>> keyValuePair in dictionary)
      {
        if (myWorkshopItemList == null)
        {
          myWorkshopItemList = MyWorkshop.GetModsInfo(keyValuePair.Key, keyValuePair.Value);
        }
        else
        {
          List<MyWorkshopItem> modsInfo = MyWorkshop.GetModsInfo(keyValuePair.Key, keyValuePair.Value);
          if (modsInfo != null)
            myWorkshopItemList.AddRange((IEnumerable<MyWorkshopItem>) modsInfo);
        }
      }
      return myWorkshopItemList;
    }

    public static List<MyWorkshopItem> GetModsInfo(
      string serviceName,
      List<ulong> workshopIds)
    {
      MyWorkshopQuery workshopQuery = MyGameService.CreateWorkshopQuery(serviceName);
      if (workshopQuery == null)
        return (List<MyWorkshopItem>) null;
      workshopQuery.ItemIds = workshopIds;
      using (AutoResetEvent resetEvent = new AutoResetEvent(false))
      {
        workshopQuery.QueryCompleted += (MyWorkshopQuery.QueryCompletedDelegate) (result =>
        {
          if (result == MyGameServiceCallResult.OK)
            MySandboxGame.Log.WriteLine("Mod dependencies query successful");
          else
            MySandboxGame.Log.WriteLine(string.Format("Error during mod dependencies query: {0}", (object) result));
          resetEvent.Set();
        });
        workshopQuery.Run();
        if (!resetEvent.WaitOne(MyWorkshop.m_dependenciesRequestTimeout))
        {
          workshopQuery.Dispose();
          return (List<MyWorkshopItem>) null;
        }
      }
      List<MyWorkshopItem> items = workshopQuery.Items;
      workshopQuery.Dispose();
      return items;
    }

    private static MyWorkshop.ResultData DownloadModsBlocking(
      List<MyObjectBuilder_Checkpoint.ModItem> mods,
      MyWorkshop.ResultData ret,
      List<WorkshopId> workshopIds,
      MyWorkshop.CancelToken cancelToken)
    {
      List<MyWorkshopItem> toGet = new List<MyWorkshopItem>(workshopIds.Count);
      if (!MyWorkshop.GetItemsBlockingUGC(workshopIds, toGet))
      {
        MySandboxGame.Log.WriteLine("Could not obtain workshop item details");
        ret.Success = false;
      }
      else if (workshopIds.Count != toGet.Count)
      {
        MySandboxGame.Log.WriteLine(string.Format("Could not obtain all workshop item details, expected {0}, got {1}", (object) workshopIds.Count, (object) toGet.Count));
        ret.Success = false;
      }
      else
      {
        if (MyWorkshop.m_downloadScreen != null)
          MySandboxGame.Static.Invoke((Action) (() => MyWorkshop.m_downloadScreen.MessageText = new StringBuilder(MyTexts.GetString(MyCommonTexts.ProgressTextDownloadingMods) + " 0 of " + toGet.Count.ToString())), nameof (DownloadModsBlocking));
        ret = MyWorkshop.DownloadModsBlockingUGC(toGet, cancelToken);
        if (!ret.Success)
        {
          MySandboxGame.Log.WriteLine("Downloading mods failed");
        }
        else
        {
          MyObjectBuilder_Checkpoint.ModItem[] array = mods.ToArray();
          for (int i = 0; i < array.Length; ++i)
          {
            MyWorkshopItem workshopItem = toGet.Find((Predicate<MyWorkshopItem>) (x => (long) x.Id == (long) array[i].PublishedFileId));
            if (workshopItem != null)
            {
              array[i].FriendlyName = workshopItem.Title;
              array[i].SetModData(workshopItem);
            }
            else
              array[i].FriendlyName = array[i].Name;
          }
          mods.Clear();
          mods.AddRange((IEnumerable<MyObjectBuilder_Checkpoint.ModItem>) array);
        }
      }
      return ret;
    }

    private static bool UpdateMod(MyWorkshopItem mod)
    {
      mod.UpdateState();
      if (mod.IsUpToDate())
        return true;
      MySandboxGame.Log.WriteLineAndConsole(string.Format("Downloading: Id = {0}, title = '{1}' ", (object) mod.Id, (object) mod.Title));
      using (AutoResetEvent resetEvent = new AutoResetEvent(false))
      {
        MyWorkshopItem.DownloadItemResult downloadItemResult = (MyWorkshopItem.DownloadItemResult) ((result, id) =>
        {
          switch (result)
          {
            case MyGameServiceCallResult.OK:
              MySandboxGame.Log.WriteLineAndConsole("Mod download successful.");
              break;
            case MyGameServiceCallResult.Pending:
              return;
            default:
              MySandboxGame.Log.WriteLineAndConsole(string.Format("Error during downloading: {0}", (object) result));
              break;
          }
          resetEvent.Set();
        });
        mod.ItemDownloaded += downloadItemResult;
        mod.Download();
        resetEvent.WaitOne();
        mod.ItemDownloaded -= downloadItemResult;
      }
      return true;
    }

    public static WorkshopId[] ToWorkshopIds(this MyWorkshopItem[] items) => ((IEnumerable<MyWorkshopItem>) items).ToList<MyWorkshopItem>().ConvertAll<WorkshopId>((Converter<MyWorkshopItem, WorkshopId>) (x => new WorkshopId(x.Id, x.ServiceName))).ToArray();

    public static bool GenerateModInfoLocal(
      string modPath,
      MyWorkshopItem[] publishedFiles,
      ulong steamIDOwner)
    {
      return MyWorkshop.GenerateModInfo(Path.Combine(MyFileSystem.ModsPath, modPath), publishedFiles, steamIDOwner);
    }

    public static bool GenerateModInfo(
      string modPath,
      MyWorkshopItem[] publishedFiles,
      ulong steamIDOwner)
    {
      WorkshopId[] workshopIds = publishedFiles.ToWorkshopIds();
      string str = Path.Combine(modPath, "modinfo.sbmi");
      WorkshopId[] localModInternal = MyWorkshop.GetWorkshopIdFromLocalModInternal(str, new WorkshopId());
      MyObjectBuilder_ModInfo newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ModInfo>();
      newObject.WorkshopId = 0UL;
      newObject.SteamIDOwner = steamIDOwner;
      if (localModInternal != null)
      {
        List<WorkshopId> list = ((IEnumerable<WorkshopId>) localModInternal).ToList<WorkshopId>();
        foreach (WorkshopId workshopId1 in workshopIds)
        {
          WorkshopId workshopId = workshopId1;
          int index = list.FindIndex((Predicate<WorkshopId>) (x => x.ServiceName == workshopId.ServiceName));
          if (index != -1)
            list[index] = workshopId;
          else
            list.Add(workshopId);
        }
        newObject.WorkshopIds = list.ToArray();
      }
      else
        newObject.WorkshopIds = workshopIds;
      if (MyObjectBuilderSerializer.SerializeXML(str, false, (MyObjectBuilder_Base) newObject))
        return true;
      MySandboxGame.Log.WriteLine(string.Format("Error creating modinfo: {0}, mod='{1}'", (object) workshopIds, (object) modPath));
      return false;
    }

    public static void CreateWorldInstanceAsync(
      MyWorkshopItem world,
      MyWorkshop.MyWorkshopPathInfo pathInfo,
      bool overwrite,
      Action<bool, string> callbackOnFinished = null)
    {
      MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenProgressAsync(MyCommonTexts.ProgressTextCreatingWorld, new MyStringId?(), (Func<IMyAsyncResult>) (() => (IMyAsyncResult) new MyWorkshop.CreateWorldResult(world, pathInfo, callbackOnFinished, overwrite)), new Action<IMyAsyncResult, MyGuiScreenProgressAsync>(MyWorkshop.endActionCreateWorldInstance)));
    }

    private static void endActionCreateWorldInstance(
      IMyAsyncResult result,
      MyGuiScreenProgressAsync screen)
    {
      screen.CloseScreen();
      MyWorkshop.CreateWorldResult createWorldResult = (MyWorkshop.CreateWorldResult) result;
      Action<bool, string> callback = createWorldResult.Callback;
      if (callback == null)
        return;
      callback(createWorldResult.Success, createWorldResult.m_createdSessionPath);
    }

    public static void UpdateWorldsAsync(
      List<MyWorkshopItem> worlds,
      MyWorkshop.MyWorkshopPathInfo pathInfo,
      Action<bool> callbackOnFinished = null)
    {
      MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, new MyStringId?(), (Func<IMyAsyncResult>) (() => (IMyAsyncResult) new MyWorkshop.UpdateWorldsResult(worlds, pathInfo, callbackOnFinished)), new Action<IMyAsyncResult, MyGuiScreenProgressAsync>(MyWorkshop.endActionUpdateWorld)));
    }

    private static void endActionUpdateWorld(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
    {
      screen.CloseScreen();
      MyWorkshop.UpdateWorldsResult updateWorldsResult = (MyWorkshop.UpdateWorldsResult) result;
      Action<bool> callback = updateWorldsResult.Callback;
      if (callback == null)
        return;
      callback(updateWorldsResult.Success);
    }

    public static bool TryUpdateWorldsBlocking(
      List<MyWorkshopItem> worlds,
      MyWorkshop.MyWorkshopPathInfo pathInfo)
    {
      if (!Directory.Exists(pathInfo.Path))
        Directory.CreateDirectory(pathInfo.Path);
      if (!MyGameService.IsOnline)
        return false;
      bool flag = true;
      foreach (MyWorkshopItem world in worlds)
        flag &= MyWorkshop.UpdateMod(world);
      return flag;
    }

    public static bool TryCreateWorldInstanceBlocking(
      MyWorkshopItem world,
      MyWorkshop.MyWorkshopPathInfo pathInfo,
      out string sessionPath,
      bool overwrite)
    {
      string sessionUniqueName = MyUtils.StripInvalidChars(world.Title);
      sessionPath = (string) null;
      Path.Combine(pathInfo.Path, world.Id.ToString() + pathInfo.Suffix);
      if (!MyGameService.IsOnline || !MyWorkshop.UpdateMod(world))
        return false;
      sessionPath = MyLocalCache.GetSessionSavesPath(sessionUniqueName, false, false);
      if (MyPlatformGameSettings.GAME_SAVES_TO_CLOUD)
      {
        if (overwrite)
          MyCloudHelper.Delete(MyCloudHelper.LocalToCloudWorldPath(sessionPath));
        if (Directory.Exists(sessionPath))
          Directory.Delete(sessionPath, true);
        while (true)
        {
          List<MyCloudFileInfo> cloudFiles = MyGameService.GetCloudFiles(MyCloudHelper.LocalToCloudWorldPath(sessionPath));
          if (cloudFiles != null && cloudFiles.Count != 0)
            sessionPath = MyLocalCache.GetSessionSavesPath(sessionUniqueName + MyUtils.GetRandomInt(int.MaxValue).ToString("########"), false, false);
          else
            break;
        }
      }
      else
      {
        if (overwrite && Directory.Exists(sessionPath))
          Directory.Delete(sessionPath, true);
        while (Directory.Exists(sessionPath))
          sessionPath = MyLocalCache.GetSessionSavesPath(sessionUniqueName + MyUtils.GetRandomInt(int.MaxValue).ToString("########"), false, false);
      }
      if (!Directory.Exists(sessionPath))
        Directory.CreateDirectory(sessionPath);
      if (MyFileSystem.IsDirectory(world.Folder))
        MyFileSystem.CopyAll(world.Folder, sessionPath);
      else
        MyZipArchive.ExtractToDirectory(world.Folder, sessionPath);
      MyObjectBuilder_Checkpoint checkpoint = MyLocalCache.LoadCheckpoint(sessionPath, out ulong _);
      if (checkpoint == null)
        return false;
      checkpoint.SessionName = string.Format("({0}) {1}", (object) pathInfo.NamePrefix, (object) world.Title);
      checkpoint.LastSaveTime = DateTime.Now;
      checkpoint.WorkshopId = new ulong?();
      MyLocalCache.SaveCheckpoint(checkpoint, sessionPath);
      return true;
    }

    private static string GetErrorString(bool ioFailure, MyGameServiceCallResult result) => !ioFailure ? result.ToString() : "IO Failure";

    public static bool CheckLocalModsAllowed(
      List<MyObjectBuilder_Checkpoint.ModItem> mods,
      bool allowLocalMods)
    {
      foreach (MyObjectBuilder_Checkpoint.ModItem mod in mods)
      {
        if (mod.PublishedFileId == 0UL && !allowLocalMods)
          return false;
      }
      return true;
    }

    public static bool CanRunOffline(List<MyObjectBuilder_Checkpoint.ModItem> mods)
    {
      foreach (MyObjectBuilder_Checkpoint.ModItem mod in mods)
      {
        if (mod.PublishedFileId != 0UL)
        {
          string path = Path.Combine(MyFileSystem.ModsPath, mod.Name);
          if (!Directory.Exists(path) && !File.Exists(path))
            return false;
        }
      }
      return true;
    }

    public static string GetWorkshopErrorText(
      MyGameServiceCallResult result,
      string serviceName,
      bool workshopPermitted)
    {
      MyStringId id;
      switch (result)
      {
        case MyGameServiceCallResult.OK:
          id = MyStringId.NullOrEmpty;
          break;
        case MyGameServiceCallResult.FileNotFound:
          id = MyCommonTexts.MessageBoxTextPublishFailed_FileNotFound;
          break;
        case MyGameServiceCallResult.AccessDenied:
          id = MyCommonTexts.MessageBoxTextPublishFailed_AccessDenied;
          break;
        case MyGameServiceCallResult.ParentalControlRestricted:
          id = workshopPermitted ? MySpaceTexts.WorkshopRestricted : MySpaceTexts.WorkshopAgeRestricted;
          break;
        case MyGameServiceCallResult.NoUser:
          id = MySpaceTexts.WorkshopNoUser;
          break;
        case MyGameServiceCallResult.PlatformRestricted:
          id = MySpaceTexts.WorkshopRestricted;
          break;
        case MyGameServiceCallResult.PlatformPublishRestricted:
          id = MySpaceTexts.WorkshopRestricted;
          break;
        default:
          id = MySpaceTexts.WorkshopError;
          break;
      }
      return (!string.IsNullOrEmpty(serviceName) ? (object) (serviceName + ": ") : (object) "").ToString() + (object) MyTexts.Get(id);
    }

    public static WorkshopId[] FilterWorkshopIds(
      WorkshopId[] publishedIds,
      string[] selectedServiceNames)
    {
      List<WorkshopId> workshopIds = ((IEnumerable<WorkshopId>) publishedIds).ToList<WorkshopId>();
      List<string> list = ((IEnumerable<string>) selectedServiceNames).ToList<string>();
      int i = 0;
      while (i < workshopIds.Count)
      {
        if (list.FindIndex((Predicate<string>) (x => x == workshopIds[i].ServiceName)) == -1)
          workshopIds.RemoveAt(i);
        else
          i++;
      }
      foreach (string selectedServiceName in selectedServiceNames)
      {
        string serviceName = selectedServiceName;
        if (workshopIds.FindIndex((Predicate<WorkshopId>) (x => x.ServiceName == serviceName)) == -1)
          workshopIds.Add(new WorkshopId(0UL, serviceName));
      }
      return workshopIds.ToArray();
    }

    public static void ReportPublish(
      MyWorkshopItem[] publishedFiles,
      MyGameServiceCallResult result,
      string resultServiceName,
      Action onDone = null,
      int index = 0)
    {
      if (publishedFiles == null || index == publishedFiles.Length)
      {
        if (result != MyGameServiceCallResult.OK)
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder(MyWorkshop.GetWorkshopErrorText(result, resultServiceName, true)), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionWorldPublishFailed), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (r => onDone.InvokeIfNotNull()))));
        else
          onDone.InvokeIfNotNull();
      }
      else
      {
        MyWorkshopItem publishedFile = publishedFiles[index++];
        MyGuiSandbox.OpenUrl(publishedFile.GetItemUrl(), UrlOpenMode.SteamOrExternalWithConfirm, new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.MessageBoxTextWorldPublishedBrowser), (object) MyGameService.Service.ServiceName, (object) publishedFile.ServiceName), MyTexts.Get(MySpaceTexts.WorkshopItemPublished), new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.MessageBoxTextWorldPublished), (object) MyGameService.Service.ServiceName, (object) publishedFile.ServiceName), MyTexts.Get(MySpaceTexts.WorkshopItemPublished), (Action<bool>) (success => MyWorkshop.ReportPublish(publishedFiles, result, resultServiceName, onDone, index)));
      }
    }

    public static void OpenWorkshopBrowser(string contentTag, Action subscriptionChangedCallback = null) => MyGameService.Service.RequestPermissions(Permissions.UGC, true, (Action<bool>) (granted =>
    {
      if (!granted)
        return;
      MyWorkshop.ContentTag = contentTag;
      MyWorkshop.SubscriptionChangedCallback = subscriptionChangedCallback;
      if (MyGameService.AtLeastOneUGCServiceConsented)
      {
        MyWorkshop.OpenWorkshopBrowserWindow();
      }
      else
      {
        MyModIoConsentViewModel consentViewModel = new MyModIoConsentViewModel(new Action(MyWorkshop.OpenWorkshopBrowserWindow));
        ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>().CreateScreen((ViewModelBase) consentViewModel);
      }
    }));

    private static void OpenWorkshopBrowserWindow() => ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>().CreateScreen((ViewModelBase) new MyWorkshopBrowserViewModel()
    {
      AdditionalTag = MyWorkshop.ContentTag
    });

    public struct Category
    {
      public string Id;
      public MyStringId LocalizableName;
      public bool IsVisibleForFilter;
    }

    public struct MyWorkshopPathInfo
    {
      public string Path;
      public string Suffix;
      public string NamePrefix;

      public static MyWorkshop.MyWorkshopPathInfo CreateWorldInfo() => new MyWorkshop.MyWorkshopPathInfo()
      {
        Path = MyWorkshop.m_workshopWorldsPath,
        Suffix = MyWorkshop.m_workshopWorldSuffix,
        NamePrefix = "Workshop"
      };

      public static MyWorkshop.MyWorkshopPathInfo CreateScenarioInfo() => new MyWorkshop.MyWorkshopPathInfo()
      {
        Path = MyWorkshop.m_workshopScenariosPath,
        Suffix = MyWorkshop.m_workshopScenariosSuffix,
        NamePrefix = "Scenario"
      };
    }

    private class CreateWorldResult : IMyAsyncResult
    {
      public string m_createdSessionPath;

      public Task Task { get; private set; }

      public bool Success { get; private set; }

      public Action<bool, string> Callback { get; private set; }

      public CreateWorldResult(
        MyWorkshopItem world,
        MyWorkshop.MyWorkshopPathInfo pathInfo,
        Action<bool, string> callback,
        bool overwrite)
      {
        MyWorkshop.CreateWorldResult createWorldResult = this;
        this.Callback = callback;
        this.Task = Parallel.Start((Action) (() => createWorldResult.Success = MyWorkshop.TryCreateWorldInstanceBlocking(world, pathInfo, out createWorldResult.m_createdSessionPath, overwrite)));
      }

      public bool IsCompleted => this.Task.IsComplete;
    }

    private class UpdateWorldsResult : IMyAsyncResult
    {
      public Task Task { get; private set; }

      public bool Success { get; private set; }

      public Action<bool> Callback { get; private set; }

      public UpdateWorldsResult(
        List<MyWorkshopItem> worlds,
        MyWorkshop.MyWorkshopPathInfo pathInfo,
        Action<bool> callback)
      {
        MyWorkshop.UpdateWorldsResult updateWorldsResult = this;
        this.Callback = callback;
        this.Task = Parallel.Start((Action) (() => updateWorldsResult.Success = MyWorkshop.TryUpdateWorldsBlocking(worlds, pathInfo)));
      }

      public bool IsCompleted => this.Task.IsComplete;
    }

    private class PublishItemResult : IMyAsyncResult
    {
      public MyGameServiceCallResult Result;
      public string ResultServiceName;
      public MyWorkshopItem[] PublishedItems;

      public Task Task { get; private set; }

      public PublishItemResult(
        string localFolder,
        string publishedTitle,
        string publishedDescription,
        WorkshopId[] workshopIds,
        MyPublishedFileVisibility visibility,
        string[] tags,
        HashSet<string> ignoredExtensions,
        HashSet<string> ignoredPaths = null,
        uint[] requiredDLCs = null)
      {
        MyWorkshop.PublishItemResult publishItemResult = this;
        this.Task = Parallel.Start((Action) (() =>
        {
          MyWorkshopItem[] publishedItems;
          (MyGameServiceCallResult, string) tuple = MyWorkshop.PublishItemBlocking(localFolder, publishedTitle, publishedDescription, workshopIds, visibility, tags, ignoredExtensions, ignoredPaths, requiredDLCs, out publishedItems);
          publishItemResult.PublishedItems = publishedItems;
          publishItemResult.Result = tuple.Item1;
          publishItemResult.ResultServiceName = tuple.Item2;
        }));
      }

      public bool IsCompleted => this.Task.IsComplete;
    }

    public struct ResultData
    {
      public bool Success;
      public bool Cancel;
      public List<MyWorkshopItem> Mods;
      public List<MyWorkshopItem> MismatchMods;

      public ResultData(bool success, bool cancel)
      {
        this.Success = success;
        this.Cancel = cancel;
        this.Mods = new List<MyWorkshopItem>();
        this.MismatchMods = new List<MyWorkshopItem>();
      }
    }

    private class DownloadModsResult : IMyAsyncResult
    {
      public MyWorkshop.ResultData Result;
      public Action<bool> Callback;

      public Task Task { get; private set; }

      public DownloadModsResult(
        List<MyObjectBuilder_Checkpoint.ModItem> mods,
        Action<bool> onFinishedCallback,
        MyWorkshop.CancelToken cancelToken)
      {
        MyWorkshop.DownloadModsResult downloadModsResult = this;
        this.Callback = onFinishedCallback;
        this.Task = Parallel.Start((Action) (() =>
        {
          downloadModsResult.Result = MyWorkshop.DownloadWorldModsBlockingInternal(mods, cancelToken);
          if (downloadModsResult.Result.Cancel)
            return;
          MySandboxGame.Static.Invoke(new Action(MyWorkshop.endActionDownloadMods), "DownloadModsResult::endActionDownloadMods");
        }));
      }

      public bool IsCompleted => this.Task.IsComplete;
    }

    public class CancelToken
    {
      public bool Cancel;
    }
  }
}
