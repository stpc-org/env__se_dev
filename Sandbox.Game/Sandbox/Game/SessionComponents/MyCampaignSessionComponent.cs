// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyCampaignSessionComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Screens;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders;
using VRage.Game.ObjectBuilders.Campaign;
using VRage.Game.VisualScripting.Campaign;
using VRage.GameServices;
using VRage.Generics;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.SessionComponents
{
  [StaticEventOwner]
  [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 666, typeof (MyObjectBuilder_CampaignSessionComponent), null, false)]
  public class MyCampaignSessionComponent : MySessionComponentBase
  {
    private MyCampaignStateMachine m_runningCampaignSM;
    private readonly Dictionary<ulong, MyObjectBuilder_Inventory> m_savedCharacterInventoriesPlayerIds = new Dictionary<ulong, MyObjectBuilder_Inventory>();
    private bool m_isScenarioRunning;
    private static ulong m_ownerId;
    private static ulong m_oldLobbyId;
    private static ulong m_elapsedMs;

    public string CampaignLevelOutcome { get; set; }

    public bool Running => this.m_runningCampaignSM != null && this.m_runningCampaignSM.CurrentNode != null;

    public bool IsScenarioRunning => this.m_isScenarioRunning;

    public bool CustomRespawnEnabled { get; set; }

    private void LoadCampaignStateMachine(string activeState = null, string platform = "")
    {
      MyObjectBuilder_Campaign activeCampaign = MyCampaignManager.Static.ActiveCampaign;
      if (activeCampaign == null)
        return;
      MyLog.Default.WriteLine("Loading campaign state machine: " + activeCampaign.Name + ";" + activeState);
      this.m_runningCampaignSM = new MyCampaignStateMachine();
      this.m_runningCampaignSM.Deserialize(activeCampaign.GetStateMachine(platform));
      if (activeState != null)
        this.m_runningCampaignSM.SetState(activeState);
      else
        this.m_runningCampaignSM.ResetToStart();
      if (this.m_runningCampaignSM.CurrentNode == null)
        this.m_runningCampaignSM.ResetToStart();
      this.m_runningCampaignSM.CurrentNode.OnUpdate((MyStateMachine) this.m_runningCampaignSM, new List<string>());
    }

    private void UpdateStateMachine()
    {
      List<string> eventCollection = new List<string>();
      string name1 = this.m_runningCampaignSM.CurrentNode.Name;
      this.m_runningCampaignSM.TriggerAction(MyStringId.GetOrCompute(this.CampaignLevelOutcome));
      this.m_runningCampaignSM.Update(eventCollection);
      string name2 = (this.m_runningCampaignSM.CurrentNode as MyCampaignStateMachineNode).Name;
      if (name1 == name2)
        MySandboxGame.Log.WriteLine("ERROR: Campaign is stuck in one state! Check the campaign file.");
      this.CampaignLevelOutcome = (string) null;
    }

    private void LoadPlayersInventories()
    {
      MyObjectBuilder_Inventory builderInventory1;
      if (this.m_savedCharacterInventoriesPlayerIds.TryGetValue(MySession.Static.LocalHumanPlayer.Id.SteamId, out builderInventory1) && MySession.Static.LocalCharacter != null)
      {
        MyInventory inventory = MyEntityExtensions.GetInventory(MySession.Static.LocalCharacter);
        foreach (MyObjectBuilder_InventoryItem builderInventoryItem in builderInventory1.Items)
          inventory.AddItems(builderInventoryItem.Amount, (MyObjectBuilder_Base) builderInventoryItem.PhysicalContent);
      }
      if (MyMultiplayer.Static == null || !MyMultiplayer.Static.IsServer)
        return;
      MySession.Static.Players.PlayersChanged += (Action<bool, MyPlayer.PlayerId>) ((added, id) =>
      {
        MyPlayer playerById = MySession.Static.Players.GetPlayerById(id);
        MyObjectBuilder_Inventory builderInventory2;
        if (playerById.Character == null || !this.m_savedCharacterInventoriesPlayerIds.TryGetValue(playerById.Id.SteamId, out builderInventory2))
          return;
        MyInventory inventory = MyEntityExtensions.GetInventory(MySession.Static.LocalCharacter);
        foreach (MyObjectBuilder_InventoryItem builderInventoryItem in builderInventory2.Items)
          inventory.AddItems(builderInventoryItem.Amount, (MyObjectBuilder_Base) builderInventoryItem.PhysicalContent);
      });
    }

    private void SavePlayersInventories()
    {
      this.m_savedCharacterInventoriesPlayerIds.Clear();
      foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) MySession.Static.Players.GetOnlinePlayers())
      {
        if (onlinePlayer.Character != null)
        {
          MyInventory inventory = MyEntityExtensions.GetInventory(onlinePlayer.Character);
          if (inventory != null)
          {
            MyObjectBuilder_Inventory objectBuilder = inventory.GetObjectBuilder();
            this.m_savedCharacterInventoriesPlayerIds[onlinePlayer.Id.SteamId] = objectBuilder;
          }
        }
      }
    }

    public void LoadNextCampaignMission(bool closeSession, bool showCredits)
    {
      if (MyMultiplayer.Static != null && !MyMultiplayer.Static.IsServer)
        return;
      this.SavePlayersInventories();
      string directoryName = Path.GetDirectoryName(MySession.Static.CurrentPath.Replace(MyFileSystem.SavesPath + "\\", ""));
      if (this.m_runningCampaignSM.Finished)
      {
        if (closeSession)
        {
          this.CallCloseOnClients();
          MySessionLoader.UnloadAndExitToMenu();
        }
        MyCampaignManager.Static.NotifyCampaignFinished();
        if (!showCredits)
          return;
        MyScreenManager.AddScreen((MyGuiScreenBase) new MyGuiScreenGameCredits());
      }
      else
      {
        this.UpdateStateMachine();
        string savePath = (this.m_runningCampaignSM.CurrentNode as MyCampaignStateMachineNode).SavePath;
        this.CallReconnectOnClients();
        MyStringId errorMessage;
        if (!MyCloudHelper.IsError(MyCampaignManager.Static.LoadSessionFromActiveCampaign(savePath, (Action) (() =>
        {
          MySession.Static.RegisterComponent((MySessionComponentBase) this, MyUpdateOrder.NoUpdate, 555);
          this.LoadPlayersInventories();
        }), directoryName, MyCampaignManager.Static.ActiveCampaignName), out errorMessage))
          return;
        MyGuiScreenMessageBox messageBox = MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(errorMessage), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
        messageBox.SkipTransition = true;
        messageBox.InstantClose = false;
        MyGuiSandbox.AddScreen((MyGuiScreenBase) messageBox);
      }
    }

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      base.Init(sessionComponent);
      MyVisualScriptManagerSessionComponent component = ((MySession) this.Session).GetComponent<MyVisualScriptManagerSessionComponent>();
      component.CampaignModPath = (string) null;
      MyObjectBuilder_CampaignSessionComponent sessionComponent1 = sessionComponent as MyObjectBuilder_CampaignSessionComponent;
      if (sessionComponent1.Mod.PublishedFileId != 0UL && sessionComponent1.Mod.PublishedServiceName == null)
        sessionComponent1.Mod.PublishedServiceName = MyGameService.GetDefaultUGC().ServiceName;
      this.m_isScenarioRunning = sessionComponent1 != null && sessionComponent1.IsVanilla;
      this.CampaignLevelOutcome = sessionComponent1.CurrentOutcome;
      this.CustomRespawnEnabled = sessionComponent1.CustomRespawnEnabled;
      if (MyMultiplayer.Static != null && !MyMultiplayer.Static.IsServer)
        return;
      if (sessionComponent1 == null || string.IsNullOrEmpty(sessionComponent1.CampaignName))
      {
        if (!MyCampaignManager.Static.IsNewCampaignLevelLoading)
          return;
        component.CampaignModPath = MyCampaignManager.Static.ActiveCampaign.ModFolderPath;
      }
      else
      {
        if (!MyCampaignManager.Static.SwitchCampaign(sessionComponent1.CampaignName, sessionComponent1.IsVanilla, sessionComponent1.Mod.PublishedFileId, sessionComponent1.Mod.PublishedServiceName, sessionComponent1.LocalModFolder))
        {
          MyLog.Default.WriteLine("MyCampaignManager - Unable to download or switch to campaign: " + sessionComponent1.CampaignName);
          if (sessionComponent1.IsVanilla)
            throw new Exception("MyCampaignManager - Unable to download or switch to campaign: " + sessionComponent1.CampaignName);
        }
        string dlc1 = MyCampaignManager.Static.ActiveCampaign?.DLC;
        MyDLCs.MyDLC dlc2;
        if (!string.IsNullOrEmpty(dlc1) && MyDLCs.TryGetDLC(dlc1, out dlc2) && (!MyGameService.IsDlcInstalled(dlc2.AppId) && !Sandbox.Engine.Platform.Game.IsDedicated))
          throw new MyLoadingNeedDLCException(dlc2);
        this.LoadCampaignStateMachine(sessionComponent1.ActiveState);
        if (MyCampaignManager.Static.ActiveCampaign == null)
          return;
        component.CampaignModPath = MyCampaignManager.Static.ActiveCampaign.ModFolderPath;
      }
    }

    protected override void UnloadData()
    {
      MyCampaignManager.Static.Unload();
      base.UnloadData();
    }

    public override MyObjectBuilder_SessionComponent GetObjectBuilder()
    {
      if (base.GetObjectBuilder() is MyObjectBuilder_CampaignSessionComponent objectBuilder)
      {
        objectBuilder.ActiveState = this.m_runningCampaignSM != null ? this.m_runningCampaignSM.CurrentNode.Name : (string) null;
        if (MyCampaignManager.Static.ActiveCampaign != null)
        {
          objectBuilder.CampaignName = MyCampaignManager.Static.ActiveCampaign.Name;
          objectBuilder.IsVanilla = MyCampaignManager.Static.ActiveCampaign.IsVanilla;
          objectBuilder.LocalModFolder = MyCampaignManager.Static.ActiveCampaign.ModFolderPath;
        }
        objectBuilder.CurrentOutcome = this.CampaignLevelOutcome;
        objectBuilder.CustomRespawnEnabled = this.CustomRespawnEnabled;
      }
      return (MyObjectBuilder_SessionComponent) objectBuilder;
    }

    private void CallReconnectOnClients()
    {
      foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) MySession.Static.Players.GetOnlinePlayers())
      {
        if (onlinePlayer.Identity.IdentityId != MySession.Static.LocalPlayerId)
          MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (s => new Action(MyCampaignSessionComponent.Reconnect)), new EndpointId(onlinePlayer.Id.SteamId));
      }
    }

    private void CallCloseOnClients()
    {
      foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) MySession.Static.Players.GetOnlinePlayers())
      {
        if (onlinePlayer.Identity.IdentityId != MySession.Static.LocalPlayerId)
          MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (s => new Action(MyCampaignSessionComponent.CloseGame)), new EndpointId(onlinePlayer.Id.SteamId));
      }
    }

    [Event(null, 367)]
    [Reliable]
    [Client]
    private static void Reconnect()
    {
      MyCampaignSessionComponent.m_ownerId = MyMultiplayer.Static.ServerId;
      MyCampaignSessionComponent.m_elapsedMs = 0UL;
      MyCampaignSessionComponent.m_oldLobbyId = (MyMultiplayer.Static as MyMultiplayerLobbyClient).LobbyId;
      MySessionLoader.UnloadAndExitToMenu();
      MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenProgress(MyTexts.Get(MyCommonTexts.LoadingDialogServerIsLoadingWorld), new MyStringId?(MyCommonTexts.Cancel)));
      Parallel.Start(new Action(MyCampaignSessionComponent.FindLobby));
    }

    [Event(null, 385)]
    [Reliable]
    [Client]
    private static void CloseGame() => MySessionLoader.UnloadAndExitToMenu();

    private static void FindLobby()
    {
      Thread.Sleep(5000);
      MyGameService.RequestLobbyList(new Action<bool>(MyCampaignSessionComponent.LobbiesRequestCompleted));
    }

    private static void LobbiesRequestCompleted(bool success)
    {
      if (!success)
        return;
      List<IMyLobby> lobbies = new List<IMyLobby>();
      MyGameService.AddPublicLobbies(lobbies);
      MyGameService.AddFriendLobbies(lobbies);
      string str = MyFinalBuildConstants.APP_VERSION.FormattedText.ToString();
      foreach (IMyLobby lobby in lobbies)
      {
        if (!(lobby.GetData("appVersion") != str) && (long) MyMultiplayerLobby.GetLobbyHostSteamId(lobby) == (long) MyCampaignSessionComponent.m_ownerId && (long) lobby.LobbyId != (long) MyCampaignSessionComponent.m_oldLobbyId)
        {
          MyScreenManager.RemoveScreenByType(typeof (MyGuiScreenProgress));
          MyJoinGameHelper.JoinGame(lobby);
          return;
        }
      }
      MyCampaignSessionComponent.m_elapsedMs += 5000UL;
      if (MyCampaignSessionComponent.m_elapsedMs > 120000UL)
        MyScreenManager.RemoveScreenByType(typeof (MyGuiScreenProgress));
      else
        MyCampaignSessionComponent.FindLobby();
    }

    protected sealed class Reconnect\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyCampaignSessionComponent.Reconnect();
      }
    }

    protected sealed class CloseGame\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyCampaignSessionComponent.CloseGame();
      }
    }
  }
}
