// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MySessionLoader
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Audio;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.ObjectBuilders;
using VRage.GameServices;
using VRage.Utils;

namespace Sandbox.Game.World
{
  public static class MySessionLoader
  {
    public static MyWorkshopItem LastLoadedSessionWorkshopItem;

    public static event Action BattleWorldLoaded;

    public static event Action ScenarioWorldLoaded;

    public static void StartNewSession(
      string sessionName,
      MyObjectBuilder_SessionSettings settings,
      List<MyObjectBuilder_Checkpoint.ModItem> mods,
      MyScenarioDefinition scenarioDefinition = null,
      int asteroidAmount = 0,
      string description = "",
      string passwd = "")
    {
      MyLog.Default.WriteLine("StartNewSandbox - Start");
      if (!MyWorkshop.CheckLocalModsAllowed(mods, settings.OnlineMode == MyOnlineModeEnum.OFFLINE))
      {
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.DialogTextLocalModsDisabledInMultiplayer), messageCaption: messageCaption));
        MyLog.Default.WriteLine("LoadSession() - End");
      }
      else
        MyWorkshop.DownloadModsAsync(mods, (Action<bool>) (success =>
        {
          if (success || settings.OnlineMode == MyOnlineModeEnum.OFFLINE && MyWorkshop.CanRunOffline(mods))
          {
            MyScreenManager.RemoveAllScreensExcept((MyGuiScreenBase) null);
            if (asteroidAmount < 0)
            {
              MyWorldGenerator.SetProceduralSettings(new int?(asteroidAmount), settings);
              asteroidAmount = 0;
            }
            MySpaceAnalytics.Instance.SetWorldEntry(MyWorldEntryEnum.Custom);
            MySessionLoader.StartLoading((Action) (() =>
            {
              MySpaceAnalytics.Instance.SetWorldEntry(MyWorldEntryEnum.Custom);
              MySession.Start(sessionName, description, passwd, settings, mods, new MyWorldGenerator.Args()
              {
                AsteroidAmount = asteroidAmount,
                Scenario = scenarioDefinition
              });
            }));
          }
          else if (MyGameService.IsOnline)
          {
            StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
            MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.DialogTextDownloadModsFailed), messageCaption: messageCaption));
          }
          else
          {
            StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
            MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.DialogTextDownloadModsFailedSteamOffline), (object) MySession.GameServiceName)), messageCaption: messageCaption));
          }
          MyLog.Default.WriteLine("StartNewSandbox - End");
        }));
    }

    public static void LoadLastSession()
    {
      string lastSessionPath = MyLocalCache.GetLastSessionPath();
      bool flag = false;
      if (lastSessionPath != null && MyPlatformGameSettings.GAME_SAVES_TO_CLOUD)
        flag = !MyCloudHelper.ExtractFilesTo(MyCloudHelper.LocalToCloudWorldPath(lastSessionPath + "/"), lastSessionPath, false);
      if (lastSessionPath == null | flag || !MyFileSystem.DirectoryExists(lastSessionPath))
        MySandboxGame.Static.Invoke((Action) (() => MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.MessageBoxLastSessionNotFound), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)))), "New Game screen");
      else
        MySessionLoader.LoadSingleplayerSession(lastSessionPath);
    }

    public static void LoadMultiplayerSession(
      MyObjectBuilder_World world,
      MyMultiplayerBase multiplayerSession)
    {
      MyLog.Default.WriteLine("LoadSession() - Start");
      if (!MyWorkshop.CheckLocalModsAllowed(world.Checkpoint.Mods, false))
      {
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.DialogTextLocalModsDisabledInMultiplayer), messageCaption: messageCaption));
        MyLog.Default.WriteLine("LoadSession() - End");
      }
      else
        MyWorkshop.DownloadModsAsync(world.Checkpoint.Mods, (Action<bool>) (success =>
        {
          if (success)
          {
            MyScreenManager.CloseAllScreensNowExcept((MyGuiScreenBase) null);
            MyGuiSandbox.Update(16);
            if (MySession.Static != null)
            {
              MySession.Static.Unload();
              MySession.Static = (MySession) null;
            }
            MySessionLoader.StartLoading((Action) (() => MySession.LoadMultiplayer(world, multiplayerSession)));
          }
          else
          {
            multiplayerSession.Dispose();
            MySessionLoader.UnloadAndExitToMenu();
            if (MyGameService.IsOnline)
            {
              StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
              MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.DialogTextDownloadModsFailed), messageCaption: messageCaption));
            }
            else
            {
              StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
              MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.DialogTextDownloadModsFailedSteamOffline), (object) MySession.GameServiceName)), messageCaption: messageCaption));
            }
          }
          MyLog.Default.WriteLine("LoadSession() - End");
        }), (Action) (() =>
        {
          multiplayerSession.Dispose();
          MySessionLoader.UnloadAndExitToMenu();
        }));
    }

    public static bool LoadDedicatedSession(
      string sessionPath,
      MyWorkshop.CancelToken cancelToken,
      Action afterLoad = null)
    {
      ulong sizeInBytes;
      MyObjectBuilder_Checkpoint checkpoint = MyLocalCache.LoadCheckpoint(sessionPath, out sizeInBytes);
      if (!MySessionLoader.HasOnlyModsFromConsentedUGCs(checkpoint))
      {
        MySessionLoader.ShowUGCConsentNotAcceptedWarning(MySessionLoader.GetNonConsentedServiceNameInCheckpoint(checkpoint));
        MyLog.Default.WriteLineAndConsole("LoadCheckpoint failed. No UGC consent.");
        MySandboxGame.Static.Exit();
        return false;
      }
      if (MySession.IsCompatibleVersion(checkpoint))
      {
        if (MyWorkshop.DownloadWorldModsBlocking(checkpoint.Mods, cancelToken).Success)
        {
          MySpaceAnalytics.Instance.SetWorldEntry(MyWorldEntryEnum.Load);
          MySession.Load(sessionPath, checkpoint, sizeInBytes);
          if (afterLoad != null)
            afterLoad();
          MySession.Static.StartServer(MyMultiplayer.Static);
          return true;
        }
        MyLog.Default.WriteLineAndConsole("Unable to download mods");
        MySandboxGame.Static.Exit();
        return false;
      }
      MyLog.Default.WriteLineAndConsole(MyTexts.Get(MyCommonTexts.DialogTextIncompatibleWorldVersion).ToString());
      MySandboxGame.Static.Exit();
      return false;
    }

    public static void LoadMultiplayerScenarioWorld(
      MyObjectBuilder_World world,
      MyMultiplayerBase multiplayerSession)
    {
      MyLog.Default.WriteLine("LoadMultiplayerScenarioWorld() - Start");
      if (!MyWorkshop.CheckLocalModsAllowed(world.Checkpoint.Mods, false))
      {
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.DialogTextLocalModsDisabledInMultiplayer), messageCaption: messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result => MySessionLoader.UnloadAndExitToMenu()))));
        MyLog.Default.WriteLine("LoadMultiplayerScenarioWorld() - End");
      }
      else
        MyWorkshop.DownloadModsAsync(world.Checkpoint.Mods, (Action<bool>) (success =>
        {
          if (success)
          {
            MyScreenManager.CloseAllScreensNowExcept((MyGuiScreenBase) null);
            MyGuiSandbox.Update(16);
            MySessionLoader.StartLoading((Action) (() =>
            {
              MySession.Static.LoadMultiplayerWorld(world, multiplayerSession);
              if (MySessionLoader.ScenarioWorldLoaded == null)
                return;
              MySessionLoader.ScenarioWorldLoaded();
            }));
          }
          else
          {
            StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
            MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.DialogTextDownloadModsFailed), messageCaption: messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result => MySandboxGame.Static.Invoke(new Action(MySessionLoader.UnloadAndExitToMenu), "UnloadAndExitToMenu")))));
          }
          MyLog.Default.WriteLine("LoadMultiplayerScenarioWorld() - End");
        }), (Action) (() => MySessionLoader.UnloadAndExitToMenu()));
    }

    private static void CheckDx11AndLoad(
      MyObjectBuilder_Checkpoint checkpoint,
      string sessionPath,
      ulong checkpointSizeInBytes,
      Action afterLoad = null)
    {
      MySessionLoader.LoadSingleplayerSession(checkpoint, sessionPath, checkpointSizeInBytes, afterLoad);
    }

    public static void LoadSingleplayerSession(
      string sessionDirectory,
      Action afterLoad = null,
      string contextName = null,
      MyOnlineModeEnum? onlineMode = null,
      int maxPlayers = 0,
      string forceSessionName = null)
    {
      MyLog.Default.WriteLine("LoadSession() - Start");
      MyLog.Default.WriteLine(sessionDirectory);
      ulong sizeInBytes = 0;
      MyLocalCache.LoadWorldConfiguration(sessionDirectory, out sizeInBytes);
      ulong checkpointSizeInBytes;
      MyObjectBuilder_Checkpoint checkpoint = MyLocalCache.LoadCheckpoint(sessionDirectory, out checkpointSizeInBytes, forceOnlineMode: onlineMode);
      if (checkpoint == null)
      {
        MyLog.Default.WriteLine(MyTexts.Get(MyCommonTexts.WorldFileIsCorruptedAndCouldNotBeLoaded).ToString());
        MySandboxGame.Static.Invoke((Action) (() =>
        {
          StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.WorldFileIsCorruptedAndCouldNotBeLoaded), messageCaption: messageCaption));
        }), "New Game screen");
        MyLog.Default.WriteLine("LoadSession() - End");
      }
      else
      {
        if (forceSessionName != null)
          checkpoint.SessionName = forceSessionName;
        checkpoint.CustomLoadingScreenText = MyStatControlText.SubstituteTexts(checkpoint.CustomLoadingScreenText, contextName);
        if (onlineMode.HasValue)
          checkpoint.MaxPlayers = (short) maxPlayers;
        if (MySessionLoader.HasOnlyModsFromConsentedUGCs(checkpoint))
        {
          if (checkpoint.OnlineMode != MyOnlineModeEnum.OFFLINE)
            MyGameService.Service.RequestPermissions(Permissions.Multiplayer, true, (Action<PermissionResult>) (granted =>
            {
              switch (granted)
              {
                case PermissionResult.Granted:
                  MyGameService.Service.RequestPermissions(Permissions.UGC, true, (Action<PermissionResult>) (ugcGranted =>
                  {
                    switch (ugcGranted)
                    {
                      case PermissionResult.Granted:
                        Run();
                        break;
                      case PermissionResult.Error:
                        MySandboxGame.Static.Invoke((Action) (() => MyGuiSandbox.Show(MyCommonTexts.XBoxPermission_MultiplayerError, type: MyMessageBoxStyleEnum.Info)), "New Game screen");
                        break;
                    }
                  }));
                  break;
                case PermissionResult.Error:
                  MySandboxGame.Static.Invoke((Action) (() => MyGuiSandbox.Show(MyCommonTexts.XBoxPermission_MultiplayerError, type: MyMessageBoxStyleEnum.Info)), "New Game screen");
                  break;
              }
            }));
          else
            Run();
        }
        else
          MySessionLoader.ShowUGCConsentNotAcceptedWarning(MySessionLoader.GetNonConsentedServiceNameInCheckpoint(checkpoint));
      }

      void Run() => MySessionLoader.CheckDx11AndLoad(checkpoint, sessionDirectory, checkpointSizeInBytes, afterLoad);
    }

    public static bool HasOnlyModsFromConsentedUGCs(MyObjectBuilder_Checkpoint checkpoint)
    {
      bool flag = true;
      if (checkpoint != null && !Sync.IsDedicated)
      {
        foreach (MyObjectBuilder_Checkpoint.ModItem mod in checkpoint.Mods)
        {
          IMyUGCService aggregate = MyGameService.WorkshopService.GetAggregate(mod.PublishedServiceName);
          if (aggregate != null && aggregate.ServiceName == mod.PublishedServiceName && !aggregate.IsConsentGiven)
            flag = false;
        }
      }
      return flag;
    }

    public static string GetNonConsentedServiceNameInCheckpoint(
      MyObjectBuilder_Checkpoint checkpoint)
    {
      string str = "";
      if (checkpoint != null && !Sync.IsDedicated)
      {
        foreach (MyObjectBuilder_Checkpoint.ModItem mod in checkpoint.Mods)
        {
          IMyUGCService aggregate = MyGameService.WorkshopService.GetAggregate(mod.PublishedServiceName);
          if (aggregate != null && aggregate.ServiceName == mod.PublishedServiceName && !aggregate.IsConsentGiven)
            str = aggregate.ServiceName;
        }
      }
      return str;
    }

    public static void ShowUGCConsentNotAcceptedWarning(string serviceName)
    {
      StringBuilder msgText = new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.WorldFileIsContainsModsFromNotConsentedUGCAndCouldNotBeLoaded), (object) serviceName);
      MyLog.Default.WriteLine(msgText.ToString());
      MySandboxGame.Static.Invoke((Action) (() =>
      {
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: msgText, messageCaption: messageCaption));
      }), "New Game screen");
      MyLog.Default.WriteLine("LoadSession() - End");
    }

    public static void ShowUGCConsentNeededForThisServiceWarning()
    {
      MyLog.Default.WriteLine(new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.NotAbleToUseBecauseNotConsentedToUGC)).ToString());
      MySandboxGame.Static.Invoke((Action) (() =>
      {
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.NotAbleToUseBecauseNotConsentedToUGC)), messageCaption: messageCaption));
      }), "New Game screen");
      MyLog.Default.WriteLine("LoadSession() - End");
    }

    private static string GetCustomLoadingScreenImagePath(string relativePath)
    {
      if (string.IsNullOrEmpty(relativePath))
        return (string) null;
      string path = Path.Combine(MyFileSystem.SavesPath, relativePath);
      if (!MyFileSystem.FileExists(path))
        path = Path.Combine(MyFileSystem.ContentPath, relativePath);
      if (!MyFileSystem.FileExists(path))
        path = Path.Combine(MyFileSystem.ModsPath, relativePath);
      if (!MyFileSystem.FileExists(path))
        path = (string) null;
      return path;
    }

    public static void LoadSingleplayerSession(
      MyObjectBuilder_Checkpoint checkpoint,
      string sessionPath,
      ulong checkpointSizeInBytes,
      Action afterLoad = null)
    {
      if (!MySession.IsCompatibleVersion(checkpoint))
      {
        MyLog.Default.WriteLine(MyTexts.Get(MyCommonTexts.DialogTextIncompatibleWorldVersionWarning).ToString());
        MySandboxGame.Static.Invoke((Action) (() =>
        {
          StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MyCommonTexts.DialogTextIncompatibleWorldVersionWarning), messageCaption: messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result =>
          {
            if (result == MyGuiScreenMessageBox.ResultEnum.YES)
              MySessionLoader.LoadSingleplayerSessionInternal(checkpoint, sessionPath, checkpointSizeInBytes, afterLoad);
            else
              MyLog.Default.WriteLine("LoadSession() - Cancelled");
          }))));
        }), "LoadSingleplayerSession failed");
      }
      else
        MySessionLoader.LoadSingleplayerSessionInternal(checkpoint, sessionPath, checkpointSizeInBytes, afterLoad);
    }

    private static void LoadSingleplayerSessionInternal(
      MyObjectBuilder_Checkpoint checkpoint,
      string sessionPath,
      ulong checkpointSizeInBytes,
      Action afterLoad = null)
    {
      bool flag = false;
      MyObjectBuilder_CampaignSessionComponent ob = checkpoint.SessionComponents.OfType<MyObjectBuilder_CampaignSessionComponent>().FirstOrDefault<MyObjectBuilder_CampaignSessionComponent>();
      if (ob != null)
        flag = ((flag ? 1 : 0) | (MyCampaignManager.Static == null ? 0 : (MyCampaignManager.Static.IsCampaign(ob) ? 1 : 0))) != 0;
      bool experimentalMode = checkpoint.Settings.ExperimentalMode;
      MyObjectBuilder_SessionSettings.ExperimentalReason experimentalReason = checkpoint.Settings.GetExperimentalReason(false);
      MyLog.Default.WriteLineAndConsole("CheckPoint Experimental mode: " + (experimentalMode ? "Yes" : "No"));
      MyLog.Default.WriteLineAndConsole("CheckPoint Experimental mode reason: " + (object) experimentalReason);
      if (!flag && (ulong) experimentalReason > 0UL | experimentalMode && !MySandboxGame.Config.ExperimentalMode)
        ShowLoadingError(MyCommonTexts.SaveGameErrorExperimental);
      else if (!MyWorkshop.CheckLocalModsAllowed(checkpoint.Mods, checkpoint.Settings.OnlineMode == MyOnlineModeEnum.OFFLINE))
      {
        MySandboxGame.Static.Invoke((Action) (() =>
        {
          StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.DialogTextLocalModsDisabledInMultiplayer), messageCaption: messageCaption));
        }), "New Game screen");
        MyLog.Default.WriteLine("LoadSession() - End");
      }
      else
        MyWorkshop.DownloadModsAsync(checkpoint.Mods, (Action<bool>) (success =>
        {
          MySandboxGame.Static.Invoke((Action) (() => DownloadModsDone(success)), "MySessionLoader::DownloadModsDone");
          MyLog.Default.WriteLine("LoadSession() - End");
        }), new Action(MySessionLoader.UnloadAndExitToMenu));

      void ShowLoadingError(MyStringId errorMessage)
      {
        MyLog.Default.WriteLine(MyTexts.GetString(errorMessage));
        MySandboxGame.Static.Invoke((Action) (() =>
        {
          StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(errorMessage), messageCaption: messageCaption));
        }), "SessionLoadingError");
      }

      void DownloadModsDone(bool success)
      {
        if (success || checkpoint.Settings.OnlineMode == MyOnlineModeEnum.OFFLINE && MyWorkshop.CanRunOffline(checkpoint.Mods))
        {
          MyScreenManager.CloseAllScreensNowExcept((MyGuiScreenBase) null);
          MyGuiSandbox.Update(16);
          string customLoadingScreenPath = MySessionLoader.GetCustomLoadingScreenImagePath(checkpoint.CustomLoadingScreenImage);
          MySessionLoader.StartLoading((Action) (() =>
          {
            if (MySession.Static != null)
            {
              MySession.Static.Unload();
              MySession.Static = (MySession) null;
            }
            MySpaceAnalytics.Instance.SetWorldEntry(MyWorldEntryEnum.Load);
            MySession.Load(sessionPath, checkpoint, checkpointSizeInBytes, allowXml: false);
            if (afterLoad == null)
              return;
            afterLoad();
          }), (Action) (() => MySessionLoader.StartLoading((Action) (() =>
          {
            if (MySession.Static != null)
            {
              MySession.Static.Unload();
              MySession.Static = (MySession) null;
            }
            MySpaceAnalytics.Instance.SetWorldEntry(MyWorldEntryEnum.Load);
            MySession.Load(sessionPath, checkpoint, checkpointSizeInBytes);
            if (afterLoad == null)
              return;
            afterLoad();
          }), customLoadingBackground: customLoadingScreenPath, customLoadingtext: checkpoint.CustomLoadingScreenText)), customLoadingScreenPath, checkpoint.CustomLoadingScreenText);
        }
        else
        {
          MyLog.Default.WriteLine(MyTexts.Get(MyCommonTexts.DialogTextDownloadModsFailed).ToString());
          if (MyGameService.IsOnline)
          {
            StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
            MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.DialogTextDownloadModsFailed), messageCaption: messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result => MySessionLoader.UnloadAndExitToMenu()))));
          }
          else
          {
            StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
            MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.DialogTextDownloadModsFailedSteamOffline), (object) MySession.GameServiceName)), messageCaption: messageCaption));
          }
        }
      }
    }

    public static void StartLoading(
      Action loadingAction,
      Action loadingActionXMLAllowed = null,
      string customLoadingBackground = null,
      string customLoadingtext = null)
    {
      if (MySpaceAnalytics.Instance != null)
        MySpaceAnalytics.Instance.StoreWorldLoadingStartTime();
      MyGuiScreenGamePlay newGameplayScreen = new MyGuiScreenGamePlay();
      MyGuiScreenGamePlay guiScreenGamePlay = newGameplayScreen;
      guiScreenGamePlay.OnLoadingAction = guiScreenGamePlay.OnLoadingAction + loadingAction;
      MyGuiScreenLoading guiScreenLoading = new MyGuiScreenLoading((MyGuiScreenBase) newGameplayScreen, MyGuiScreenGamePlay.Static, customLoadingBackground, customLoadingtext);
      guiScreenLoading.OnScreenLoadingFinished += (Action) (() =>
      {
        if (MySession.Static == null)
          return;
        MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.HUDScreen));
        newGameplayScreen.LoadingDone = true;
      });
      guiScreenLoading.OnLoadingXMLAllowed = loadingActionXMLAllowed;
      MyGuiSandbox.AddScreen((MyGuiScreenBase) guiScreenLoading);
    }

    public static void Unload()
    {
      try
      {
        try
        {
          if (MySession.Static != null)
          {
            MySession.Static.Unload();
          }
          else
          {
            MyScreenManager.CloseAllScreensNowExcept((MyGuiScreenBase) null, true);
            MyGuiSandbox.Update(16);
          }
        }
        finally
        {
          MySession.Static = (MySession) null;
          try
          {
            if (MyMusicController.Static != null)
            {
              MyMusicController.Static.Unload();
              MyAudio.Static.MusicAllowed = true;
              MyAudio.Static.Mute = false;
            }
          }
          finally
          {
            MyMusicController.Static = (MyMusicController) null;
            if (MyMultiplayer.Static != null)
              MyMultiplayer.Static.Dispose();
          }
        }
      }
      catch (Exception ex)
      {
        MySession.Static = (MySession) null;
        MySandboxGame.Log.WriteLine("ERROR: Failed to cleanly unload session:");
        MySandboxGame.Log.WriteLine(Environment.StackTrace);
        MySandboxGame.Log.WriteLine(ex);
      }
    }

    public static void UnloadAndExitToMenu()
    {
      MySessionLoader.Unload();
      MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.MainMenu));
    }

    public static void LoadInventoryScene()
    {
      if (!MyGameService.IsActive || !MyFakes.ENABLE_MAIN_MENU_INVENTORY_SCENE)
        return;
      string str = Path.Combine(MyFileSystem.ContentPath, "InventoryScenes\\Inventory-9");
      DictionaryValuesReader<MyDefinitionId, MyMainMenuInventorySceneDefinition> menuInventoryScenes = MyDefinitionManager.Static.GetMainMenuInventoryScenes();
      if (menuInventoryScenes.Count > 0)
      {
        List<MyMainMenuInventorySceneDefinition> list = menuInventoryScenes.ToList<MyMainMenuInventorySceneDefinition>();
        int randomInt = MyUtils.GetRandomInt(list.Count);
        str = Path.Combine(MyFileSystem.ContentPath, list[randomInt].SceneDirectory);
      }
      ulong sizeInBytes;
      MyObjectBuilder_Checkpoint checkpoint = MyLocalCache.LoadCheckpoint(str, out sizeInBytes);
      MySession.Load(str, checkpoint, sizeInBytes, false);
    }

    public static void ExitGame()
    {
      if (MySpaceAnalytics.Instance != null)
        MySpaceAnalytics.Instance.ReportSessionEnd("Exit to Windows");
      MyScreenManager.CloseAllScreensNowExcept((MyGuiScreenBase) null);
      MySandboxGame.ExitThreadSafe();
    }
  }
}
