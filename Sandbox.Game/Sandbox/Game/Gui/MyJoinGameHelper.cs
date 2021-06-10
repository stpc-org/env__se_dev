// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyJoinGameHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Analytics;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.ObjectBuilders;
using VRage.GameServices;
using VRage.Network;
using VRage.Profiler;
using VRage.Utils;

namespace Sandbox.Game.Gui
{
  public static class MyJoinGameHelper
  {
    private static MyGuiScreenProgress m_progress;

    private static bool JoinGameTest(IMyLobby lobby)
    {
      if (!lobby.IsValid)
        return false;
      if (!MyMultiplayerLobby.IsLobbyCorrectVersion(lobby))
      {
        string format = MyTexts.GetString(MyCommonTexts.MultiplayerError_IncorrectVersion);
        string str1 = MyBuildNumbers.ConvertBuildNumberFromIntToString((int) MyFinalBuildConstants.APP_VERSION);
        int lobbyAppVersion = MyMultiplayerLobby.GetLobbyAppVersion(lobby);
        if (lobbyAppVersion != 0)
        {
          string str2 = MyBuildNumbers.ConvertBuildNumberFromIntToString(lobbyAppVersion);
          MyGuiSandbox.Show(new StringBuilder(string.Format(format, (object) str1, (object) str2)));
        }
        return false;
      }
      if (!MyFakes.ENABLE_MP_DATA_HASHES || MyMultiplayerLobby.HasSameData(lobby))
        return true;
      MyGuiSandbox.Show(MyCommonTexts.MultiplayerError_DifferentData);
      MySandboxGame.Log.WriteLine("Different game data when connecting to server. Local hash: " + MyDataIntegrityChecker.GetHashBase64() + ", server hash: " + MyMultiplayerLobby.GetDataHash(lobby));
      return false;
    }

    public static void JoinGame(IMyLobby lobby, bool requestData = true)
    {
      if (MySession.Static != null)
      {
        MySession.Static.Unload();
        MySession.Static = (MySession) null;
      }
      if (requestData && string.IsNullOrEmpty(lobby.GetData("appVersion")))
      {
        MyLobbyHelper myLobbyHelper = new MyLobbyHelper(lobby);
        myLobbyHelper.OnSuccess += (Action<IMyLobby, bool>) ((l, isSuccess) =>
        {
          if (!isSuccess)
            MyJoinGameHelper.JoinGame(lobby.LobbyId);
          MyJoinGameHelper.JoinGame(l, false);
        });
        if (myLobbyHelper.RequestData())
          return;
      }
      if (!MyJoinGameHelper.JoinGameTest(lobby))
        return;
      MyJoinGameHelper.JoinGame(lobby.LobbyId);
    }

    public static void JoinGame(
      MyGameServerItem server,
      Dictionary<string, string> rules,
      bool enableGuiBackgroundFade = true,
      Action failedToJoin = null)
    {
      if (MySession.Static != null)
      {
        MySession.Static.Unload();
        MySession.Static = (MySession) null;
      }
      MySpaceAnalytics.Instance.SetWorldEntry(MyWorldEntryEnum.Join);
      if (server.ServerVersion != (int) MyFinalBuildConstants.APP_VERSION)
      {
        StringBuilder text = new StringBuilder();
        text.AppendFormat(MyTexts.GetString(MyCommonTexts.MultiplayerError_IncorrectVersion), (object) MyFinalBuildConstants.APP_VERSION, (object) server.ServerVersion);
        MyGuiSandbox.Show(text, MyCommonTexts.MessageBoxCaptionError);
        if (failedToJoin == null)
          return;
        failedToJoin();
      }
      else
      {
        if (MyFakes.ENABLE_MP_DATA_HASHES)
        {
          string gameTagByPrefix = server.GetGameTagByPrefix("datahash");
          if (gameTagByPrefix != "" && gameTagByPrefix != MyDataIntegrityChecker.GetHashBase64())
          {
            MyGuiSandbox.Show(MyCommonTexts.MultiplayerError_DifferentData);
            MySandboxGame.Log.WriteLine("Different game data when connecting to server. Local hash: " + MyDataIntegrityChecker.GetHashBase64() + ", server hash: " + gameTagByPrefix);
            if (failedToJoin == null)
              return;
            failedToJoin();
            return;
          }
        }
        MyCachedServerItem csi = new MyCachedServerItem(server);
        csi.Rules = rules;
        if (rules != null)
        {
          csi.DeserializeSettings();
          if (!string.IsNullOrEmpty(MyJoinGameHelper.GetNonConsentedServiceNameInMyCachedServerItem(csi)))
          {
            MySessionLoader.ShowUGCConsentNotAcceptedWarning(MyJoinGameHelper.GetNonConsentedServiceNameInMyCachedServerItem(csi));
            foreach (MyGuiScreenBase screen in MyScreenManager.Screens)
            {
              if (screen is MyGuiScreenProgress)
                screen.CloseScreenNow();
            }
            if (failedToJoin == null)
              return;
            failedToJoin();
          }
          else
          {
            MyGameService.AddHistoryGame(server);
            MyMultiplayerClient multiplayer = new MyMultiplayerClient(server, new MySyncLayer(new MyTransportLayer(2)));
            if (!string.IsNullOrEmpty(MyJoinGameHelper.GetNonConsentedServiceNameInMyMultiplayerClient((MyMultiplayerBase) multiplayer)))
            {
              MySessionLoader.ShowUGCConsentNotAcceptedWarning(MyJoinGameHelper.GetNonConsentedServiceNameInMyMultiplayerClient((MyMultiplayerBase) multiplayer));
              foreach (MyGuiScreenBase screen in MyScreenManager.Screens)
              {
                if (screen is MyGuiScreenProgress)
                  screen.CloseScreenNow();
              }
              if (failedToJoin == null)
                return;
              failedToJoin();
            }
            else
            {
              multiplayer.ExperimentalMode = MySandboxGame.Config.ExperimentalMode;
              MyMultiplayer.Static = (MyMultiplayerBase) multiplayer;
              MyGuiScreenProgress progress = new MyGuiScreenProgress(MyTexts.Get(MyCommonTexts.DialogTextJoiningWorld), new MyStringId?(MyCommonTexts.Cancel), false, enableGuiBackgroundFade);
              MyGuiSandbox.AddScreen((MyGuiScreenBase) progress);
              progress.ProgressCancelled += (Action) (() =>
              {
                multiplayer.Dispose();
                MySessionLoader.UnloadAndExitToMenu();
                if (MyMultiplayer.Static == null)
                  return;
                MyMultiplayer.Static.Dispose();
              });
              multiplayer.OnJoin += (Action) (() => MyJoinGameHelper.OnJoin(progress, true, (IMyLobby) null, MyLobbyStatusCode.Success, (MyMultiplayerBase) multiplayer));
              Action<string> onProfilerCommandExecuted = (Action<string>) (desc =>
              {
                MyHudNotification myHudNotification = new MyHudNotification(MyStringId.GetOrCompute(desc));
                MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification);
                MyLog.Default.WriteLine(desc);
              });
              VRage.Profiler.MyRenderProfiler.GetProfilerFromServer = (Action) (() =>
              {
                onProfilerCommandExecuted("Command executed: Download profiler");
                MyMultiplayer.Static.ProfilerDone = onProfilerCommandExecuted;
                MyMultiplayer.Static.DownloadProfiler();
              });
              Sandbox.MyRenderProfiler.ServerInvoke = (Action<RenderProfilerCommand, int>) ((cmd, payload) =>
              {
                onProfilerCommandExecuted("Command executed: " + cmd.ToString());
                MyMultiplayer.RaiseStaticEvent<RenderProfilerCommand, int>((Func<IMyEventOwner, Action<RenderProfilerCommand, int>>) (_ => new Action<RenderProfilerCommand, int>(Sandbox.MyRenderProfiler.OnCommandReceived)), cmd, payload);
              });
            }
          }
        }
        else
        {
          if (failedToJoin == null)
            return;
          failedToJoin();
        }
      }
    }

    public static void JoinGame(
      MyGameServerItem server,
      bool enableGuiBackgroundFade = true,
      Action failedToJoin = null)
    {
      MyGameService.ServerDiscovery.GetServerRules(server, (ServerRulesResponse) (rules => MyJoinGameHelper.JoinGame(server, rules, enableGuiBackgroundFade, failedToJoin)), (Action) (() => MyJoinGameHelper.JoinGame(server, (Dictionary<string, string>) null, enableGuiBackgroundFade, failedToJoin)));
    }

    private static string GetNonConsentedServiceNameInMyMultiplayerClient(MyMultiplayerBase mClient)
    {
      string str = "";
      if (mClient != null && !Sync.IsDedicated)
      {
        using (List<MyObjectBuilder_Checkpoint.ModItem>.Enumerator enumerator = mClient.Mods.GetEnumerator())
        {
          if (enumerator.MoveNext())
          {
            MyObjectBuilder_Checkpoint.ModItem current = enumerator.Current;
            IMyUGCService aggregate = MyGameService.WorkshopService.GetAggregate(current.PublishedServiceName);
            if (aggregate != null)
            {
              if (aggregate.ServiceName == current.PublishedServiceName)
              {
                if (!aggregate.IsConsentGiven)
                  str = aggregate.ServiceName;
              }
            }
          }
        }
      }
      return str;
    }

    private static string GetNonConsentedServiceNameInMyCachedServerItem(MyCachedServerItem csi)
    {
      string str = "";
      if (csi != null && csi.Mods != null && !Sync.IsDedicated)
      {
        using (List<WorkshopId>.Enumerator enumerator = csi.Mods.GetEnumerator())
        {
          if (enumerator.MoveNext())
          {
            WorkshopId current = enumerator.Current;
            IMyUGCService aggregate = MyGameService.WorkshopService.GetAggregate(current.ServiceName);
            if (aggregate != null)
            {
              if (aggregate.ServiceName == current.ServiceName)
              {
                if (!aggregate.IsConsentGiven)
                  str = aggregate.ServiceName;
              }
            }
          }
        }
      }
      return str;
    }

    public static void JoinGame(ulong lobbyId) => MyGameService.Service.RequestPermissions(Permissions.Multiplayer, true, (Action<PermissionResult>) (granted =>
    {
      switch (granted)
      {
        case PermissionResult.Granted:
          MyGameService.Service.RequestPermissions(Permissions.UGC, true, (Action<PermissionResult>) (ugcGranted =>
          {
            if (ugcGranted == PermissionResult.Granted)
            {
              MyJoinGameHelper.JoinGameInternal(lobbyId);
            }
            else
            {
              if (ugcGranted != PermissionResult.Error)
                return;
              MyGuiSandbox.Show(MyCommonTexts.XBoxPermission_MultiplayerError, type: MyMessageBoxStyleEnum.Info);
            }
          }));
          break;
        case PermissionResult.Error:
          MyGuiSandbox.Show(MyCommonTexts.XBoxPermission_MultiplayerError, type: MyMessageBoxStyleEnum.Info);
          break;
      }
    }));

    private static void JoinGameInternal(ulong lobbyId)
    {
      MyGuiScreenProgress progress = new MyGuiScreenProgress(MyTexts.Get(MyCommonTexts.DialogTextJoiningWorld), new MyStringId?(MyCommonTexts.Cancel));
      MyGuiSandbox.AddScreen((MyGuiScreenBase) progress);
      progress.ProgressCancelled += (Action) (() => MySessionLoader.UnloadAndExitToMenu());
      MyLog.Default.WriteLine("Joining lobby: " + (object) lobbyId);
      MyMultiplayerJoinResult result = MyMultiplayer.JoinLobby(lobbyId);
      result.JoinDone += (Action<bool, IMyLobby, MyLobbyStatusCode, MyMultiplayerBase>) ((success, lobby, response, multiplayer) =>
      {
        if (multiplayer != null && multiplayer != null && !string.IsNullOrEmpty(MyJoinGameHelper.GetNonConsentedServiceNameInMyMultiplayerClient(multiplayer)))
        {
          MySessionLoader.ShowUGCConsentNotAcceptedWarning(MyJoinGameHelper.GetNonConsentedServiceNameInMyMultiplayerClient(multiplayer));
          foreach (MyGuiScreenBase screen in MyScreenManager.Screens)
          {
            if (screen is MyGuiScreenProgress)
              screen.CloseScreenNow();
          }
        }
        else
          MyJoinGameHelper.OnJoin(progress, success, lobby, response, multiplayer);
      });
      progress.ProgressCancelled += (Action) (() => result.Cancel());
    }

    public static void OnJoin(
      MyGuiScreenProgress progress,
      bool success,
      IMyLobby lobby,
      MyLobbyStatusCode response,
      MyMultiplayerBase multiplayer)
    {
      MyLog.Default.WriteLine(string.Format("Lobby join response: {0}, enter state: {1}", (object) success, (object) response));
      if (success && response == MyLobbyStatusCode.Success && (long) multiplayer.GetOwner() != (long) Sync.MyId)
        MyJoinGameHelper.DownloadWorld(progress, multiplayer);
      else
        MyJoinGameHelper.OnJoinFailed(progress, multiplayer, response);
    }

    private static void DownloadWorld(MyGuiScreenProgress progress, MyMultiplayerBase multiplayer)
    {
      MyJoinGameHelper.m_progress = progress;
      progress.Text = MyTexts.Get(MyCommonTexts.MultiplayerStateConnectingToServer);
      MyLog.Default.WriteLine("World requested");
      Stopwatch worldRequestTime = Stopwatch.StartNew();
      ulong serverId = multiplayer.GetOwner();
      bool connected = false;
      progress.Tick += (Action) (() =>
      {
        MyP2PSessionState state = new MyP2PSessionState();
        if (MyGameService.Peer2Peer != null)
          MyGameService.Peer2Peer.GetSessionState(multiplayer.ServerId, ref state);
        if (!connected && state.ConnectionActive)
        {
          MyLog.Default.WriteLine("World requested - connection alive");
          connected = true;
          progress.Text = MyTexts.Get(MyCommonTexts.MultiplayerStateWaitingForServer);
        }
        if ((long) serverId != (long) multiplayer.GetOwner())
        {
          MyLog.Default.WriteLine("World requested - failed, version mismatch");
          progress.Cancel();
          MyGuiSandbox.Show(MyCommonTexts.MultiplayerErrorServerHasLeft);
          multiplayer.Dispose();
        }
        else
        {
          bool flag = MyScreenManager.IsScreenOfTypeOpen(typeof (MyGuiScreenDownloadMods));
          if (!flag && !worldRequestTime.IsRunning)
            worldRequestTime.Start();
          else if (flag && worldRequestTime.IsRunning)
            worldRequestTime.Stop();
          if (!worldRequestTime.IsRunning || worldRequestTime.Elapsed.TotalSeconds <= 40.0)
            return;
          MyLog.Default.WriteLine("World requested - failed, server changed");
          progress.Cancel();
          MyGuiSandbox.Show(MyCommonTexts.MultiplaterJoin_ServerIsNotResponding);
          multiplayer.Dispose();
        }
      });
      multiplayer.DownloadWorld((int) MyFinalBuildConstants.APP_VERSION);
    }

    public static StringBuilder GetErrorMessage(MyLobbyStatusCode response)
    {
      MyStringId id;
      switch (response)
      {
        case MyLobbyStatusCode.DoesntExist:
          id = MyCommonTexts.LobbyDoesntExist;
          break;
        case MyLobbyStatusCode.NotAllowed:
          id = MyCommonTexts.LobbyNotAllowed;
          break;
        case MyLobbyStatusCode.Full:
          id = MyCommonTexts.LobbyFull;
          break;
        case MyLobbyStatusCode.Error:
          id = MyCommonTexts.LobbyError;
          break;
        case MyLobbyStatusCode.Banned:
          id = MyCommonTexts.LobbyBanned;
          break;
        case MyLobbyStatusCode.Limited:
          id = MyCommonTexts.LobbyLimited;
          break;
        case MyLobbyStatusCode.ClanDisabled:
          id = MyCommonTexts.LobbyClanDisabled;
          break;
        case MyLobbyStatusCode.CommunityBan:
          id = MyCommonTexts.LobbyCommunityBan;
          break;
        case MyLobbyStatusCode.MemberBlockedYou:
          id = MyCommonTexts.LobbyMemberBlockedYou;
          break;
        case MyLobbyStatusCode.YouBlockedMember:
          id = MyCommonTexts.LobbyYouBlockedMember;
          break;
        case MyLobbyStatusCode.FriendsOnly:
          id = MyCommonTexts.OnlyFriendsCanJoinThisGame;
          break;
        case MyLobbyStatusCode.Cancelled:
          id = MyCommonTexts.LobbyCancelled;
          break;
        case MyLobbyStatusCode.LostInternetConnection:
          id = MyCommonTexts.LobbyLostInternetConnection;
          break;
        case MyLobbyStatusCode.ServiceUnavailable:
          id = MyCommonTexts.LobbyServiceUnavailable;
          break;
        case MyLobbyStatusCode.NoDirectConnections:
          id = MyCommonTexts.LobbyNoDirectConnections;
          break;
        case MyLobbyStatusCode.VersionMismatch:
          id = MyCommonTexts.LobbyVersionMismatch;
          break;
        case MyLobbyStatusCode.UserMultiplayerRestricted:
          id = MyCommonTexts.LobbyUserMultiplayerRestricted;
          break;
        case MyLobbyStatusCode.ConnectionProblems:
          id = MyCommonTexts.LobbyConnectionProblems;
          break;
        case MyLobbyStatusCode.InvalidPasscode:
          id = MyCommonTexts.LobbyInvalidPasscode;
          break;
        case MyLobbyStatusCode.NoUser:
          id = MyCommonTexts.LobbyNoUser;
          break;
        default:
          id = MyCommonTexts.LobbyError;
          break;
      }
      return new StringBuilder(string.Format(MyTexts.GetString(id), (object) MySession.GameServiceName));
    }

    private static void OnJoinFailed(
      MyGuiScreenProgress progress,
      MyMultiplayerBase multiplayer,
      MyLobbyStatusCode response)
    {
      multiplayer?.Dispose();
      progress.Cancel();
      if (response == MyLobbyStatusCode.Success)
        return;
      StringBuilder errorMessage = MyJoinGameHelper.GetErrorMessage(response);
      MyLog.Default.WriteLine("OnJoinFailed: " + (object) response + " / " + (object) errorMessage);
      MyGuiSandbox.Show(errorMessage);
    }

    private static void CheckDx11AndJoin(MyObjectBuilder_World world, MyMultiplayerBase multiplayer)
    {
      if (multiplayer.Scenario)
        MySessionLoader.LoadMultiplayerScenarioWorld(world, multiplayer);
      else
        MySessionLoader.LoadMultiplayerSession(world, multiplayer);
    }

    public static void OnDX11SwitchRequestAnswer(MyGuiScreenMessageBox.ResultEnum result)
    {
      if (result == MyGuiScreenMessageBox.ResultEnum.YES)
      {
        MySandboxGame.Config.GraphicsRenderer = MySandboxGame.DirectX11RendererKey;
        MySandboxGame.Config.Save();
        MyGuiSandbox.BackToMainMenu();
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MySpaceTexts.QuickstartDX11PleaseRestartGame), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
      }
      else
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MySpaceTexts.QuickstartSelectDifferent), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
    }

    public static void WorldReceived(MyObjectBuilder_World world, MyMultiplayerBase multiplayer)
    {
      if (world == null)
      {
        MyLog.Default.WriteLine("World requested - failed, version mismatch");
        MyJoinGameHelper.m_progress.Cancel();
        MyJoinGameHelper.m_progress = (MyGuiScreenProgress) null;
        MyGuiSandbox.Show(MyCommonTexts.MultiplayerErrorAppVersionMismatch);
        multiplayer.Dispose();
      }
      else
      {
        if (world?.Checkpoint?.Settings != null && !MySandboxGame.Config.ExperimentalMode)
        {
          int num = world.Checkpoint.Settings.IsSettingsExperimental(true) ? 1 : (world.Checkpoint.Mods == null ? 0 : ((uint) world.Checkpoint.Mods.Count > 0U ? 1 : 0));
          bool flag = world.Checkpoint.SessionComponents.Find((Predicate<MyObjectBuilder_SessionComponent>) (x => x is MyObjectBuilder_CampaignSessionComponent)) is MyObjectBuilder_CampaignSessionComponent sessionComponent && sessionComponent.IsVanilla;
          if (num != 0 && !flag)
          {
            MySessionLoader.UnloadAndExitToMenu();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(MyCommonTexts.DialogTextJoinWorldFailed, (object) MyTexts.GetString(MyCommonTexts.MultiplayerErrorExperimental));
            MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: stringBuilder, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
            return;
          }
        }
        MyJoinGameHelper.m_progress = (MyGuiScreenProgress) null;
        MyJoinGameHelper.CheckDx11AndJoin(world, multiplayer);
      }
    }
  }
}
