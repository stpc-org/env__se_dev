// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyVisualScriptManagerSessionComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Debugging;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Debugging;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Game.ObjectBuilders.Gui;
using VRage.Game.ObjectBuilders.VisualScripting;
using VRage.Game.SessionComponents;
using VRage.Game.VisualScripting;
using VRage.Game.VisualScripting.Missions;
using VRage.Generics;
using VRage.ObjectBuilders;
using VRage.Scripting;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation | MyUpdateOrder.AfterSimulation, 1000, typeof (MyObjectBuilder_VisualScriptManagerSessionComponent), null, false)]
  public class MyVisualScriptManagerSessionComponent : MySessionComponentBase
  {
    public static readonly string WAYPOINT_NAME_PREFIX = "Waypoint_";
    private static bool m_firstUpdate = true;
    private CachingList<IMyLevelScript> m_levelScripts;
    private MyObjectBuilder_VisualScriptManagerSessionComponent m_objectBuilder;
    private MyVSStateMachineManager m_smManager;
    private readonly Dictionary<string, string> m_relativePathsToAbsolute = new Dictionary<string, string>();
    private readonly List<string> m_stateMachineDefinitionFilePaths = new List<string>();
    private string[] m_runningLevelScriptNames;
    private string[] m_failedLevelScriptExceptionTexts;
    private HashSet<string> m_worldOutlineFolders = new HashSet<string>();
    private Dictionary<long, MyUIString> m_UIStrings = new Dictionary<long, MyUIString>();
    private StringBuilder m_UIStringBuilder = new StringBuilder();
    private Dictionary<string, MyGuiScreenBoard> m_boardScreens = new Dictionary<string, MyGuiScreenBoard>();
    private int m_updateCounter;
    private const int LIVE_DEBUGGING_DELAY = 120;
    private int m_liveDebuggingCounter;
    private bool m_hadClients;

    public bool IsActive => this.m_levelScripts != null;

    public CachingList<IMyLevelScript> LevelScripts => this.m_levelScripts;

    public MyVSStateMachineManager SMManager => this.m_smManager;

    public MyObjectBuilder_Questlog QuestlogData
    {
      get => this.m_objectBuilder != null ? this.m_objectBuilder.Questlog : (MyObjectBuilder_Questlog) null;
      set
      {
        if (this.m_objectBuilder == null)
          return;
        this.m_objectBuilder.Questlog = value;
      }
    }

    public MyObjectBuilder_ExclusiveHighlights ExclusiveHighlightsData
    {
      get => this.m_objectBuilder != null ? this.m_objectBuilder.ExclusiveHighlights : (MyObjectBuilder_ExclusiveHighlights) null;
      set
      {
        if (this.m_objectBuilder == null)
          return;
        this.m_objectBuilder.ExclusiveHighlights = value;
      }
    }

    public string[] RunningLevelScriptNames => this.m_runningLevelScriptNames;

    public string[] FailedLevelScriptExceptionTexts => this.m_failedLevelScriptExceptionTexts;

    public string CampaignModPath { get; set; }

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      base.Init(sessionComponent);
      MyVSStateMachineFinalNode.Finished += new Action<string, string, bool, bool>(this.OnSMFinished);
      MyObjectBuilder_VisualScriptManagerSessionComponent sessionComponent1 = (MyObjectBuilder_VisualScriptManagerSessionComponent) sessionComponent;
      this.m_objectBuilder = sessionComponent1;
      if (sessionComponent1.BoardScreens != null)
      {
        this.m_boardScreens.Clear();
        foreach (MyObjectBuilder_BoardScreen boardScreen in sessionComponent1.BoardScreens)
        {
          Vector2 textLeftTopPosition = MyGuiManager.GetScreenTextLeftTopPosition();
          MyGuiScreenBoard myGuiScreenBoard = new MyGuiScreenBoard((Vector2) boardScreen.Coords, textLeftTopPosition, (Vector2) boardScreen.Size);
          myGuiScreenBoard.Init(boardScreen);
          this.m_boardScreens.Add(boardScreen.Id, myGuiScreenBoard);
          if (!Sync.IsDedicated)
            MyScreenManager.AddScreen((MyGuiScreenBase) myGuiScreenBoard);
        }
      }
      if (!this.Session.IsServer)
        return;
      MyVisualScriptManagerSessionComponent.m_firstUpdate = sessionComponent1.FirstRun;
      this.m_worldOutlineFolders.Clear();
      if (sessionComponent1.WorldOutlineFolders != null)
      {
        foreach (string worldOutlineFolder in sessionComponent1.WorldOutlineFolders)
          this.m_worldOutlineFolders.Add(worldOutlineFolder);
      }
      this.FixFilepathsPlatform();
    }

    private void OnSMFinished(
      string machineName,
      string transitionName,
      bool showCredits,
      bool closeSession)
    {
      string activeCampaignName = MyCampaignManager.Static.ActiveCampaignName;
      Sandbox.Game.MyVisualScriptLogicProvider.SessionClose(showCredits: showCredits, closeSession: closeSession);
    }

    public override void BeforeStart()
    {
      if (this.m_objectBuilder == null || !this.Session.IsServer)
        return;
      this.m_relativePathsToAbsolute.Clear();
      this.m_stateMachineDefinitionFilePaths.Clear();
      string scenarioName = (string) null;
      if (this.m_objectBuilder.LevelScriptFiles != null)
      {
        foreach (string levelScriptFile in this.m_objectBuilder.LevelScriptFiles)
        {
          MyContentPath myContentPath = (MyContentPath) Path.Combine(this.CampaignModPath ?? MyFileSystem.ContentPath, levelScriptFile);
          if (myContentPath.GetExitingFilePath() != null)
          {
            this.m_relativePathsToAbsolute.Add(levelScriptFile, myContentPath.GetExitingFilePath());
            if (levelScriptFile.StartsWith("Scenarios"))
            {
              string str = levelScriptFile.Substring("Scenarios\\".Length);
              int length = str.IndexOf('\\');
              scenarioName = str.Substring(0, length);
            }
          }
          else
            MyLog.Default.WriteLine(levelScriptFile + " Level Script was not found.");
        }
      }
      if (this.m_objectBuilder.StateMachines != null)
      {
        foreach (string stateMachine in this.m_objectBuilder.StateMachines)
        {
          MyContentPath myContentPath = (MyContentPath) Path.Combine(this.CampaignModPath ?? MyFileSystem.ContentPath, stateMachine);
          if (myContentPath.GetExitingFilePath() != null)
          {
            if (!this.m_relativePathsToAbsolute.ContainsKey(stateMachine))
              this.m_stateMachineDefinitionFilePaths.Add(myContentPath.GetExitingFilePath());
            this.m_relativePathsToAbsolute.Add(stateMachine, myContentPath.GetExitingFilePath());
          }
          else
            MyLog.Default.WriteLine(stateMachine + " Mission File was not found.");
        }
      }
      bool flag1 = false;
      if (this.Session.Mods != null)
      {
        foreach (MyObjectBuilder_Checkpoint.ModItem mod in this.Session.Mods)
        {
          string str = mod.GetPath();
          if (!MyFileSystem.DirectoryExists(str))
          {
            str = Path.Combine(MyFileSystem.ModsPath, mod.Name);
            if (!MyFileSystem.DirectoryExists(str))
              str = (string) null;
          }
          if (!string.IsNullOrEmpty(str))
          {
            foreach (string file in MyFileSystem.GetFiles(str, "*", MySearchOption.AllDirectories))
            {
              string extension = Path.GetExtension(file);
              string key = MyFileSystem.MakeRelativePath(Path.Combine(str, "VisualScripts"), file);
              if (extension == ".vs" || extension == ".vsc")
              {
                if (this.m_relativePathsToAbsolute.ContainsKey(key))
                {
                  this.m_relativePathsToAbsolute[key] = file;
                  flag1 = true;
                }
                else
                  this.m_relativePathsToAbsolute.Add(key, file);
              }
            }
          }
        }
      }
      if (this.m_relativePathsToAbsolute.Count == 0)
        return;
      bool compilationSupported = MyVRage.Platform.Scripting.IsRuntimeCompilationSupported;
      IVSTAssemblyProvider assemblyProvider = MyVRage.Platform.Scripting.VSTAssemblyProvider;
      bool flag2 = false;
      if (!flag1)
      {
        string assemblyName = scenarioName != null ? MyVisualScriptingAssemblyHelper.MakeAssemblyName(scenarioName) : "VisualScripts";
        try
        {
          flag2 = assemblyProvider.TryLoad(assemblyName, compilationSupported);
        }
        catch (FileNotFoundException ex)
        {
        }
        catch (Exception ex)
        {
          MyLog.Default.Error("Cannot load visual script assembly: " + ex.Message);
        }
      }
      if ((flag1 || !flag2) && MyVRage.Platform.Scripting.IsRuntimeCompilationSupported)
        assemblyProvider.Init((IEnumerable<string>) this.m_relativePathsToAbsolute.Values, this.CampaignModPath);
      else if (!flag2)
      {
        MyLog.Default.Error("Precompiled mod scripts assembly was not loaded. Scripts will not function.");
        return;
      }
      this.m_levelScripts = new CachingList<IMyLevelScript>();
      string[] levelScriptFiles = this.m_objectBuilder.LevelScriptFiles;
      HashSet<string> scriptNames = new HashSet<string>((levelScriptFiles != null ? ((IEnumerable<string>) levelScriptFiles).Select<string, string>(new Func<string, string>(Path.GetFileNameWithoutExtension)) : (IEnumerable<string>) null) ?? Enumerable.Empty<string>());
      assemblyProvider.GetLevelScriptInstances(scriptNames).ForEach((Action<IMyLevelScript>) (script => this.m_levelScripts.Add(script)));
      this.m_levelScripts.ApplyAdditions();
      this.m_runningLevelScriptNames = this.m_levelScripts.Select<IMyLevelScript, string>((Func<IMyLevelScript, string>) (x => x.GetType().Name)).ToArray<string>();
      this.m_failedLevelScriptExceptionTexts = new string[this.m_runningLevelScriptNames.Length];
      this.m_smManager = new MyVSStateMachineManager();
      foreach (string definitionFilePath in this.m_stateMachineDefinitionFilePaths)
        this.m_smManager.AddMachine(definitionFilePath);
      if (this.m_objectBuilder != null && this.m_objectBuilder.ScriptStateMachineManager != null)
      {
        foreach (MyObjectBuilder_ScriptStateMachineManager.CursorStruct activeStateMachine in this.m_objectBuilder.ScriptStateMachineManager.ActiveStateMachines)
          this.m_smManager.Restore(activeStateMachine.StateMachineName, (IEnumerable<MyObjectBuilder_ScriptSMCursor>) activeStateMachine.Cursors);
      }
      this.ConvertOldWaypoints();
      this.UpdateFoldersFromWaypoints();
    }

    private void ConvertOldWaypoints()
    {
      List<MyEntity> myEntityList = new List<MyEntity>();
      foreach (MyEntity entity in MyEntities.GetEntities())
      {
        if (this.IsOldWaypoint(entity))
          myEntityList.Add(entity);
      }
      foreach (MyEntity entity in myEntityList)
      {
        MyEntities.Close(entity);
        MyEntities.DeleteRememberedEntities();
        MyEntities.Add((MyEntity) MyVisualScriptManagerSessionComponent.CreateWaypoint(entity.Name, entity.EntityId, entity.PositionComp.WorldMatrixRef));
      }
    }

    private bool IsOldWaypoint(MyEntity ent) => ent.Name != null && ent.Name.Length >= MyVisualScriptManagerSessionComponent.WAYPOINT_NAME_PREFIX.Length && (MyVisualScriptManagerSessionComponent.WAYPOINT_NAME_PREFIX.Equals(ent.Name.Substring(0, MyVisualScriptManagerSessionComponent.WAYPOINT_NAME_PREFIX.Length)) && !(ent is MyWaypoint));

    private void UpdateFoldersFromWaypoints()
    {
      foreach (MyEntity entity in MyEntities.GetEntities())
      {
        if (entity is MyWaypoint myWaypoint)
          this.CreateFoldersFromPath(myWaypoint.Path);
      }
    }

    public override void UpdateBeforeSimulation()
    {
      if (!this.Session.IsServer)
        return;
      if (this.m_smManager != null)
        this.m_smManager.Update();
      if (this.m_levelScripts == null)
        return;
      if (VRage.Game.VisualScripting.MyVisualScriptLogicProvider.MissionUpdate != null)
        VRage.Game.VisualScripting.MyVisualScriptLogicProvider.MissionUpdate();
      if (this.m_updateCounter % 10 == 0 && VRage.Game.VisualScripting.MyVisualScriptLogicProvider.MissionUpdate10 != null)
        VRage.Game.VisualScripting.MyVisualScriptLogicProvider.MissionUpdate10();
      if (this.m_updateCounter % 100 == 0 && VRage.Game.VisualScripting.MyVisualScriptLogicProvider.MissionUpdate100 != null)
        VRage.Game.VisualScripting.MyVisualScriptLogicProvider.MissionUpdate100();
      if (this.m_updateCounter % 1000 == 0 && VRage.Game.VisualScripting.MyVisualScriptLogicProvider.MissionUpdate1000 != null)
        VRage.Game.VisualScripting.MyVisualScriptLogicProvider.MissionUpdate1000();
      if (++this.m_updateCounter > 1000)
        this.m_updateCounter = 0;
      foreach (IMyLevelScript levelScript in this.m_levelScripts)
      {
        try
        {
          if (MyVisualScriptManagerSessionComponent.m_firstUpdate)
            levelScript.GameStarted();
          else
            levelScript.Update();
        }
        catch (Exception ex)
        {
          string name = levelScript.GetType().Name;
          for (int index = 0; index < this.m_runningLevelScriptNames.Length; ++index)
          {
            if (this.m_runningLevelScriptNames[index] == name)
            {
              // ISSUE: explicit reference operation
              ^ref this.m_runningLevelScriptNames[index] += " - failed";
              this.m_failedLevelScriptExceptionTexts[index] = ex.ToString();
            }
          }
          this.m_levelScripts.Remove(levelScript);
        }
      }
      this.m_levelScripts.ApplyRemovals();
      MyVisualScriptManagerSessionComponent.m_firstUpdate = false;
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      this.LiveDebugging();
      foreach (MyUIString myUiString in this.m_UIStrings.Values)
      {
        this.m_UIStringBuilder.Clear();
        this.m_UIStringBuilder.Append(myUiString.Text);
        MyGuiManager.DrawString(myUiString.Font, this.m_UIStringBuilder.ToString(), myUiString.NormalizedCoord, myUiString.Scale, drawAlign: myUiString.DrawAlign);
      }
    }

    public override void LoadData()
    {
      base.LoadData();
      if (MySessionComponentExtDebug.Static.IsHandlerRegistered(new MySessionComponentExtDebug.ReceivedMsgHandler(this.LiveDebugging_ReceivedMessageHandler)))
        return;
      MySessionComponentExtDebug.Static.ReceivedMsg += new MySessionComponentExtDebug.ReceivedMsgHandler(this.LiveDebugging_ReceivedMessageHandler);
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      this.DisposeRunningScripts();
      this.SendDisconnectMessage();
      this.m_UIStrings.Clear();
    }

    private void DisposeRunningScripts()
    {
      if (this.m_levelScripts == null)
        return;
      foreach (IMyLevelScript levelScript in this.m_levelScripts)
      {
        levelScript.GameFinished();
        levelScript.Dispose();
      }
      this.m_smManager.Dispose();
      this.m_smManager = (MyVSStateMachineManager) null;
      this.m_levelScripts.Clear();
      this.m_levelScripts.ApplyRemovals();
    }

    public override MyObjectBuilder_SessionComponent GetObjectBuilder()
    {
      if (!this.Session.IsServer)
        return (MyObjectBuilder_SessionComponent) null;
      this.m_objectBuilder.ScriptStateMachineManager = this.m_smManager?.GetObjectBuilder();
      this.m_objectBuilder.FirstRun = MyVisualScriptManagerSessionComponent.m_firstUpdate;
      this.m_objectBuilder.Questlog = MyHud.Questlog.GetObjectBuilder();
      MyHighlightSystem component = MySession.Static.GetComponent<MyHighlightSystem>();
      if (component != null)
        this.m_objectBuilder.ExclusiveHighlights = component.GetExclusiveHighlightsObjectBuilder();
      this.m_objectBuilder.BoardScreens = this.m_boardScreens.Select<KeyValuePair<string, MyGuiScreenBoard>, MyObjectBuilder_BoardScreen>((Func<KeyValuePair<string, MyGuiScreenBoard>, MyObjectBuilder_BoardScreen>) (x => x.Value.GetBoardObjectBuilder(x.Key))).ToArray<MyObjectBuilder_BoardScreen>();
      return (MyObjectBuilder_SessionComponent) this.m_objectBuilder;
    }

    public void Reset()
    {
      if (this.m_smManager != null)
        this.m_smManager.Dispose();
      MyVisualScriptManagerSessionComponent.m_firstUpdate = true;
    }

    private void FixFilepathsPlatform()
    {
      if (this.m_objectBuilder.LevelScriptFiles != null)
      {
        for (int index = 0; index < this.m_objectBuilder.LevelScriptFiles.Length; ++index)
        {
          string str = this.FixFilepathPlatform(this.m_objectBuilder.LevelScriptFiles[index]);
          this.m_objectBuilder.LevelScriptFiles[index] = str;
        }
      }
      if (this.m_objectBuilder.StateMachines == null)
        return;
      for (int index = 0; index < this.m_objectBuilder.StateMachines.Length; ++index)
      {
        string str = this.FixFilepathPlatform(this.m_objectBuilder.StateMachines[index]);
        this.m_objectBuilder.StateMachines[index] = str;
      }
    }

    private string FixFilepathPlatform(string filePath)
    {
      MyContentPath myContentPath = (MyContentPath) Path.Combine(this.CampaignModPath ?? MyFileSystem.ContentPath, filePath);
      if (!string.IsNullOrEmpty(myContentPath.GetExitingFilePath()))
        return filePath;
      string path2 = filePath;
      int length = filePath.LastIndexOf("Scripts\\");
      if (length != -1)
        path2 = filePath.Substring(0, length) + "PC\\Scripts\\" + filePath.Substring(length + 8, filePath.Length - (length + 8));
      myContentPath = (MyContentPath) Path.Combine(this.CampaignModPath ?? MyFileSystem.ContentPath, path2);
      return !string.IsNullOrEmpty(myContentPath.GetExitingFilePath()) ? path2 : filePath;
    }

    public void CreateUIString(long id, MyUIString UIString) => this.m_UIStrings[id] = UIString;

    public void RemoveUIString(long id) => this.m_UIStrings.Remove(id);

    public void CreateBoardScreen(
      string boardId,
      float normalizedPosX,
      float normalizedPosY,
      float normalizedSizeX,
      float normalizedSizeY)
    {
      Vector2 textLeftTopPosition = MyGuiManager.GetScreenTextLeftTopPosition();
      MyGuiScreenBoard myGuiScreenBoard = new MyGuiScreenBoard(new Vector2(normalizedPosX, normalizedPosY), textLeftTopPosition, new Vector2(normalizedSizeX, normalizedSizeY));
      if (!Sync.IsDedicated)
        MyScreenManager.AddScreen((MyGuiScreenBase) myGuiScreenBoard);
      this.m_boardScreens[boardId] = myGuiScreenBoard;
    }

    public void RemoveBoardScreen(string boardId)
    {
      MyGuiScreenBoard myGuiScreenBoard;
      if (!this.m_boardScreens.TryGetValue(boardId, out myGuiScreenBoard))
        return;
      if (!Sync.IsDedicated)
        MyScreenManager.RemoveScreen((MyGuiScreenBase) myGuiScreenBoard);
      this.m_boardScreens.Remove(boardId);
    }

    public void AddColumn(string boardId, string columnId, MyGuiScreenBoard.MyColumn column)
    {
      MyGuiScreenBoard myGuiScreenBoard;
      if (!this.m_boardScreens.TryGetValue(boardId, out myGuiScreenBoard))
        return;
      myGuiScreenBoard.AddColumn(columnId, column);
    }

    public void AddColumn(
      string boardId,
      string columnId,
      float width,
      string headerText,
      MyGuiDrawAlignEnum headerDrawAlign,
      MyGuiDrawAlignEnum columnDrawAlign)
    {
      MyGuiScreenBoard myGuiScreenBoard;
      if (!this.m_boardScreens.TryGetValue(boardId, out myGuiScreenBoard))
        return;
      myGuiScreenBoard.AddColumn(columnId, width, headerText, headerDrawAlign, columnDrawAlign);
    }

    public void RemoveColumn(string boardId, string columnId)
    {
      MyGuiScreenBoard myGuiScreenBoard;
      if (!this.m_boardScreens.TryGetValue(boardId, out myGuiScreenBoard))
        return;
      myGuiScreenBoard.RemoveColumn(boardId);
    }

    public void AddRow(string boardId, string rowId)
    {
      MyGuiScreenBoard myGuiScreenBoard;
      if (!this.m_boardScreens.TryGetValue(boardId, out myGuiScreenBoard))
        return;
      myGuiScreenBoard.AddRow(rowId);
    }

    public void RemoveRow(string boardId, string rowId)
    {
      MyGuiScreenBoard myGuiScreenBoard;
      if (!this.m_boardScreens.TryGetValue(boardId, out myGuiScreenBoard))
        return;
      myGuiScreenBoard.RemoveRow(rowId);
    }

    public void SetCell(string boardId, string rowId, string columnId, string text)
    {
      MyGuiScreenBoard myGuiScreenBoard;
      if (!this.m_boardScreens.TryGetValue(boardId, out myGuiScreenBoard))
        return;
      myGuiScreenBoard.SetCell(rowId, columnId, text);
    }

    public void SetRowRanking(string boardId, string rowId, int ranking)
    {
      MyGuiScreenBoard myGuiScreenBoard;
      if (!this.m_boardScreens.TryGetValue(boardId, out myGuiScreenBoard))
        return;
      myGuiScreenBoard.SetRowRanking(rowId, ranking);
    }

    public void SortByColumn(string boardId, string columnId, bool ascending)
    {
      MyGuiScreenBoard myGuiScreenBoard;
      if (!this.m_boardScreens.TryGetValue(boardId, out myGuiScreenBoard))
        return;
      myGuiScreenBoard.SortByColumn(columnId, ascending);
    }

    public void SortByRanking(string boardId, bool ascending)
    {
      MyGuiScreenBoard myGuiScreenBoard;
      if (!this.m_boardScreens.TryGetValue(boardId, out myGuiScreenBoard))
        return;
      myGuiScreenBoard.SortByRanking(ascending);
    }

    public void ShowOrderInColumn(string boardId, string columnId)
    {
      MyGuiScreenBoard myGuiScreenBoard;
      if (!this.m_boardScreens.TryGetValue(boardId, out myGuiScreenBoard))
        return;
      myGuiScreenBoard.ShowOrderInColumn(columnId);
    }

    public void SetColumnVisibility(string boardId, string columnId, bool visible)
    {
      MyGuiScreenBoard myGuiScreenBoard;
      if (!this.m_boardScreens.TryGetValue(boardId, out myGuiScreenBoard))
        return;
      myGuiScreenBoard.SetColumnVisibility(columnId, visible);
    }

    private void LiveDebugging()
    {
      if (!this.Session.IsServer)
        return;
      if (this.m_liveDebuggingCounter == 0)
      {
        this.m_liveDebuggingCounter = 120;
        this.SendStatusMessage();
        if (!this.m_hadClients && MySessionComponentExtDebug.Static.HasClients)
        {
          MyDebugDrawSettings.ENABLE_DEBUG_DRAW = true;
          MyDebugDrawSettings.DEBUG_DRAW_WAYPOINTS = true;
        }
        this.m_hadClients = MySessionComponentExtDebug.Static.HasClients;
      }
      if (MyVRage.Platform.Scripting.VSTAssemblyProvider.DebugEnabled && this.m_smManager?.RunningMachines != null)
      {
        foreach (MyVSStateMachine runningMachine in this.m_smManager.RunningMachines)
          MyVisualScriptingDebug.LogStateMachine(runningMachine);
        this.SendLoggedNodesMessage();
      }
      MyVisualScriptingDebug.Clear();
      --this.m_liveDebuggingCounter;
    }

    private void SendStatusMessage()
    {
      VSStatusMsg msg = new VSStatusMsg()
      {
        World = MySession.Static.CurrentPath,
        VSComponent = (MyObjectBuilder_VisualScriptManagerSessionComponent) this.GetObjectBuilder()
      };
      MySessionComponentExtDebug.Static.SendMessageToClients<VSStatusMsg>(msg);
    }

    private void SendDisconnectMessage()
    {
      VSDisconnectMsg msg = new VSDisconnectMsg();
      MySessionComponentExtDebug.Static.SendMessageToClients<VSDisconnectMsg>(msg);
    }

    private void LiveDebugging_ReceivedMessageHandler(
      MyExternalDebugStructures.CommonMsgHeader messageHeader,
      byte[] messageData)
    {
      if (MyExternalDebugStructures.ReadMessageFromPtr<VSReqEntitiesMsg>(ref messageHeader, messageData, out VSReqEntitiesMsg _))
        this.SendEntitiesToClients();
      if (MyExternalDebugStructures.ReadMessageFromPtr<VSReqTriggersMsg>(ref messageHeader, messageData, out VSReqTriggersMsg _))
        this.SendAllTriggers();
      if (MyExternalDebugStructures.ReadMessageFromPtr<VSReqEntityIdsMsg>(ref messageHeader, messageData, out VSReqEntityIdsMsg _))
        this.SendEntityIDs();
      if (MyExternalDebugStructures.ReadMessageFromPtr<VSReqWaypointCreateMsg>(ref messageHeader, messageData, out VSReqWaypointCreateMsg _))
        MyVisualScriptManagerSessionComponent.CreateWaypoint();
      VSReqWaypointDeleteMsg outMsg1;
      if (MyExternalDebugStructures.ReadMessageFromPtr<VSReqWaypointDeleteMsg>(ref messageHeader, messageData, out outMsg1))
        this.DeleteWaypoint(outMsg1.Id);
      VSFolderDeletedMsg outMsg2;
      if (MyExternalDebugStructures.ReadMessageFromPtr<VSFolderDeletedMsg>(ref messageHeader, messageData, out outMsg2))
        this.DeleteFolder(outMsg2.Path);
      VSFolderCreatedMsg outMsg3;
      if (MyExternalDebugStructures.ReadMessageFromPtr<VSFolderCreatedMsg>(ref messageHeader, messageData, out outMsg3))
        this.CreateFoldersFromPath(outMsg3.Path);
      VSWaypointRenamedMsg outMsg4;
      if (MyExternalDebugStructures.ReadMessageFromPtr<VSWaypointRenamedMsg>(ref messageHeader, messageData, out outMsg4))
        this.RenameWaypoint(outMsg4.Id, outMsg4.Name);
      VSFolderRenamedMsg outMsg5;
      if (MyExternalDebugStructures.ReadMessageFromPtr<VSFolderRenamedMsg>(ref messageHeader, messageData, out outMsg5))
        this.RenameFolder(outMsg5.OldPath, outMsg5.NewPath);
      VSWaypointPathUpdatedMsg outMsg6;
      MyWaypoint entity1;
      if (MyExternalDebugStructures.ReadMessageFromPtr<VSWaypointPathUpdatedMsg>(ref messageHeader, messageData, out outMsg6) && MyEntities.TryGetEntityById<MyWaypoint>(outMsg6.Id, out entity1))
        entity1.Path = outMsg6.Path;
      VSWaypointVisibilityUpdatedMsg outMsg7;
      MyWaypoint entity2;
      if (MyExternalDebugStructures.ReadMessageFromPtr<VSWaypointVisibilityUpdatedMsg>(ref messageHeader, messageData, out outMsg7) && MyEntities.TryGetEntityById<MyWaypoint>(outMsg7.Id, out entity2))
        entity2.Visible = outMsg7.Visible;
      VSWaypointFreezeUpdatedMsg outMsg8;
      MyWaypoint entity3;
      if (MyExternalDebugStructures.ReadMessageFromPtr<VSWaypointFreezeUpdatedMsg>(ref messageHeader, messageData, out outMsg8) && MyEntities.TryGetEntityById<MyWaypoint>(outMsg8.Id, out entity3))
        entity3.Freeze = outMsg8.Freeze;
      if (MyExternalDebugStructures.ReadMessageFromPtr<VSSaveWorldMsg>(ref messageHeader, messageData, out VSSaveWorldMsg _) && MySession.Static != null && !MyAsyncSaving.InProgress)
        MyAsyncSaving.Start((Action) (() => MySector.ResetEyeAdaptation = true));
      VSActivatesStatesMsg outMsg9;
      if (!MyExternalDebugStructures.ReadMessageFromPtr<VSActivatesStatesMsg>(ref messageHeader, messageData, out outMsg9) || MySession.Static == null)
        return;
      foreach (MyVSStateMachine runningMachine in this.SMManager.RunningMachines)
      {
        if (runningMachine.Name == outMsg9.SMName)
        {
          foreach (MyStateMachineCursor activeCursor in runningMachine.ActiveCursors)
            runningMachine.DeleteCursor(activeCursor.Id);
          foreach (string activeState in outMsg9.ActiveStates)
            runningMachine.CreateCursor(activeState);
          break;
        }
      }
    }

    public void SendEntitiesToClients()
    {
      IEnumerable<MyWaypoint> myWaypoints = MyEntities.GetEntities().OfType<MyWaypoint>();
      List<MyObjectBuilder_Waypoint> objectBuilderWaypointList = new List<MyObjectBuilder_Waypoint>();
      foreach (MyWaypoint myWaypoint in myWaypoints)
        objectBuilderWaypointList.Add((MyObjectBuilder_Waypoint) myWaypoint.GetObjectBuilder(false));
      VSEntitiesMsg msg = new VSEntitiesMsg()
      {
        Folders = this.m_worldOutlineFolders.ToArray<string>(),
        Waypoints = objectBuilderWaypointList
      };
      MySessionComponentExtDebug.Static.SendMessageToClients<VSEntitiesMsg>(msg);
    }

    public static MyWaypoint CreateWaypoint(string name, long id, MatrixD worldMatrix)
    {
      MyObjectBuilder_Waypoint objectBuilderWaypoint = new MyObjectBuilder_Waypoint();
      objectBuilderWaypoint.Name = name;
      objectBuilderWaypoint.EntityId = id;
      objectBuilderWaypoint.PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation(worldMatrix.Translation, (Vector3) worldMatrix.Forward, (Vector3) worldMatrix.Up));
      objectBuilderWaypoint.Visible = true;
      objectBuilderWaypoint.Freeze = false;
      MyWaypoint entity = MyEntityFactory.CreateEntity<MyWaypoint>((MyObjectBuilder_Base) objectBuilderWaypoint);
      entity.Init((MyObjectBuilder_EntityBase) objectBuilderWaypoint);
      entity.Components.Remove<MyPhysicsComponentBase>();
      return entity;
    }

    public static void CreateWaypoint()
    {
      int num = 0;
      string name;
      do
      {
        name = MyVisualScriptManagerSessionComponent.WAYPOINT_NAME_PREFIX + (object) num++;
      }
      while (MyEntities.TryGetEntityByName(name, out MyEntity _));
      MatrixD worldMatrix = MyAPIGateway.Session.Camera.WorldMatrix;
      MyWaypoint waypoint = MyVisualScriptManagerSessionComponent.CreateWaypoint(name, MyEntityIdentifier.AllocateId(), worldMatrix);
      MyEntities.Add((MyEntity) waypoint);
      if (MyGuiScreenScriptingTools.Static != null)
        MyGuiScreenScriptingTools.Static.OnWaypointAdded(waypoint);
      VSWaypointCreatedMsg msg = new VSWaypointCreatedMsg()
      {
        Waypoint = (MyObjectBuilder_Waypoint) waypoint.GetObjectBuilder(false)
      };
      MySessionComponentExtDebug.Static.SendMessageToClients<VSWaypointCreatedMsg>(msg);
    }

    private void DeleteWaypoint(long id)
    {
      MyEntity entity;
      if (!MyEntities.TryGetEntityById(id, out entity))
        return;
      entity.Close();
    }

    private void DeleteFolder(string path) => this.m_worldOutlineFolders.RemoveWhere((Predicate<string>) (x => x.StartsWith(path)));

    private void CreateFoldersFromPath(string path)
    {
      for (int startIndex = path.LastIndexOf('\\'); startIndex != -1; startIndex = path.LastIndexOf('\\'))
      {
        this.m_worldOutlineFolders.Add(path);
        path = path.Remove(startIndex);
      }
      if (string.IsNullOrEmpty(path))
        return;
      this.m_worldOutlineFolders.Add(path);
    }

    private void RenameWaypoint(long id, string name)
    {
      MyWaypoint entity;
      if (!MyEntities.TryGetEntityById<MyWaypoint>(id, out entity))
        return;
      entity.Name = name;
      MyEntities.SetEntityName((MyEntity) entity, false);
      if (MyGuiScreenScriptingTools.Static == null)
        return;
      MyGuiScreenScriptingTools.Static.UpdateWaypointList();
      MyGuiScreenScriptingTools.Static.UpdateTriggerList();
    }

    private void RenameFolder(string oldPath, string newPath)
    {
      List<string> stringList = new List<string>();
      foreach (string worldOutlineFolder in this.m_worldOutlineFolders)
      {
        if (worldOutlineFolder.StartsWith(oldPath))
          stringList.Add(worldOutlineFolder.Replace(oldPath, newPath));
      }
      this.DeleteFolder(oldPath);
      foreach (string str in stringList)
        this.m_worldOutlineFolders.Add(str);
    }

    private void SendAllTriggers()
    {
      List<MyTriggerComponent> allTriggers = MySessionComponentTriggerSystem.Static.GetAllTriggers();
      List<MyObjectBuilder_AreaTrigger> list = allTriggers.ConvertAll<MyObjectBuilder_TriggerBase>((Converter<MyTriggerComponent, MyObjectBuilder_TriggerBase>) (x => (MyObjectBuilder_TriggerBase) x.Serialize(false))).Where<MyObjectBuilder_TriggerBase>((Func<MyObjectBuilder_TriggerBase, bool>) (x => x is MyObjectBuilder_AreaTrigger)).Cast<MyObjectBuilder_AreaTrigger>().ToList<MyObjectBuilder_AreaTrigger>();
      List<long> longList = allTriggers.ConvertAll<long>((Converter<MyTriggerComponent, long>) (x => x.Entity.EntityId));
      VSTriggersMsg msg = new VSTriggersMsg()
      {
        Triggers = list.ToArray(),
        Parents = longList.ToArray()
      };
      MySessionComponentExtDebug.Static.SendMessageToClients<VSTriggersMsg>(msg);
    }

    private void SendEntityIDs()
    {
      List<SimpleEntityInfo> simpleEntityInfoList = new List<SimpleEntityInfo>();
      foreach (MyEntity entity in MyEntities.GetEntities())
      {
        SimpleEntityInfo simpleEntityInfo = new SimpleEntityInfo()
        {
          Id = entity.EntityId,
          Name = entity.Name,
          DisplayName = entity.DisplayName,
          Type = entity.GetType().Name
        };
        simpleEntityInfoList.Add(simpleEntityInfo);
      }
      VSEntityIdsMsg msg = new VSEntityIdsMsg()
      {
        Data = simpleEntityInfoList.ToArray()
      };
      MySessionComponentExtDebug.Static.SendMessageToClients<VSEntityIdsMsg>(msg);
    }

    private void SendLoggedNodesMessage()
    {
      VSLoggedNodesMsg msg = new VSLoggedNodesMsg()
      {
        Time = MySandboxGame.TotalTimeInMilliseconds,
        Nodes = MyVisualScriptingDebug.LoggedNodes.ToArray<MyDebuggingNodeLog>(),
        StateMachines = MyVisualScriptingDebug.StateMachines.ToArray<MyDebuggingStateMachine>()
      };
      MySessionComponentExtDebug.Static.SendMessageToClients<VSLoggedNodesMsg>(msg);
    }
  }
}
