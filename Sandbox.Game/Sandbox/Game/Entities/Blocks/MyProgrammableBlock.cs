// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MyProgrammableBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.IntergridCommunication;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.Compiler;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.Groups;
using VRage.Library;
using VRage.ModAPI;
using VRage.Network;
using VRage.Scripting;
using VRage.Serialization;
using VRage.Utils;

namespace Sandbox.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_MyProgrammableBlock))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyProgrammableBlock), typeof (Sandbox.ModAPI.Ingame.IMyProgrammableBlock)})]
  public class MyProgrammableBlock : MyFunctionalBlock, Sandbox.ModAPI.IMyProgrammableBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyProgrammableBlock, Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider, IMyMultiTextPanelComponentOwner, IMyTextPanelComponentOwner
  {
    private static readonly string[] NEW_LINES = new string[2]
    {
      "\r\n",
      "\n"
    };
    private const string DEFAULT_SCRIPT_TEMPLATE = "public Program()\r\n{{\r\n{0}\r\n}}\r\n\r\npublic void Save()\r\n{{\r\n{1}\r\n}}\r\n\r\npublic void Main(string argument, UpdateType updateSource)\r\n{{\r\n{2}\r\n}}\r\n";
    private const int MAX_NUM_EXECUTED_INSTRUCTIONS = 50000;
    private const int MAX_NUM_METHOD_CALLS = 10000;
    private const int MAX_ECHO_LENGTH = 8000;
    private IMyGridProgram m_instance;
    private MyProgrammableBlock.RuntimeInfo m_runtime;
    private string m_programData;
    private string m_storageData;
    private string m_editorData;
    private string m_terminalRunArgument = string.Empty;
    private readonly StringBuilder m_echoOutput = new StringBuilder();
    private bool m_consoleOpen;
    private MyGuiScreenEditor m_editorScreen;
    private Assembly m_assembly;
    private readonly List<string> m_compilerErrors = new List<string>();
    private MyProgrammableBlock.ScriptTerminationReason m_terminationReason;
    private bool m_isRunning;
    private bool m_mainMethodSupportsArgument;
    private ulong m_userId;
    private readonly List<MyCubeGrid> m_groupCache = new List<MyCubeGrid>();
    private bool m_needsInstantiation;
    private MyProgrammableBlock.MyGridTerminalWrapper m_terminalWrapper = new MyProgrammableBlock.MyGridTerminalWrapper();
    private MyMultiTextPanelComponent m_multiPanel;
    private MyGuiScreenTextPanel m_textBoxMultiPanel;
    internal MyIngameScriptComponent ScriptComponent;
    private readonly HashSet<EndpointId> m_subscribers = new HashSet<EndpointId>();
    private bool m_isTextPanelOpen;

    public string TerminalRunArgument
    {
      get => this.m_terminalRunArgument;
      set => this.m_terminalRunArgument = value ?? string.Empty;
    }

    public MyProgrammableBlock()
    {
      this.CreateTerminalControls();
      this.Render = (MyRenderComponentBase) new MyRenderComponentScreenAreas((MyEntity) this);
    }

    public ulong UserId
    {
      get => this.m_userId;
      set => this.m_userId = value;
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyProgrammableBlock>())
        return;
      base.CreateTerminalControls();
      string gameServiceName = MySession.GameServiceName;
      MyStringId panelEditCodeTooltip = MySpaceTexts.TerminalControlPanel_EditCode_Tooltip;
      MyTerminalControlButton<MyProgrammableBlock> terminalControlButton1 = new MyTerminalControlButton<MyProgrammableBlock>("Edit", MySpaceTexts.TerminalControlPanel_EditCode, panelEditCodeTooltip, (Action<MyProgrammableBlock>) (b => b.SendOpenEditorRequest()));
      terminalControlButton1.Visible = (Func<MyProgrammableBlock, bool>) (b => MyFakes.ENABLE_PROGRAMMABLE_BLOCK && MySession.Static.EnableIngameScripts);
      terminalControlButton1.Enabled = (Func<MyProgrammableBlock, bool>) (b => MySession.Static.IsUserScripter(Sync.MyId));
      MyTerminalControlFactory.AddControl<MyProgrammableBlock>((MyTerminalControl<MyProgrammableBlock>) terminalControlButton1);
      MyTerminalControlTextbox<MyProgrammableBlock> terminalControlTextbox = new MyTerminalControlTextbox<MyProgrammableBlock>("ConsoleCommand", MySpaceTexts.TerminalControlPanel_RunArgument, MySpaceTexts.TerminalControlPanel_RunArgument_ToolTip);
      terminalControlTextbox.Visible = (Func<MyProgrammableBlock, bool>) (e => MyFakes.ENABLE_PROGRAMMABLE_BLOCK && MySession.Static.EnableIngameScripts);
      terminalControlTextbox.Getter = (MyTerminalControlTextbox<MyProgrammableBlock>.GetterDelegate) (e => new StringBuilder(e.TerminalRunArgument));
      terminalControlTextbox.Setter = (MyTerminalControlTextbox<MyProgrammableBlock>.SetterDelegate) ((e, v) => e.TerminalRunArgument = v.ToString());
      MyTerminalControlFactory.AddControl<MyProgrammableBlock>((MyTerminalControl<MyProgrammableBlock>) terminalControlTextbox);
      MyTerminalControlButton<MyProgrammableBlock> terminalControlButton2 = new MyTerminalControlButton<MyProgrammableBlock>("TerminalRun", MySpaceTexts.TerminalControlPanel_RunCode, MySpaceTexts.TerminalControlPanel_RunCode_Tooltip, (Action<MyProgrammableBlock>) (b => b.Run(b.TerminalRunArgument, UpdateType.Terminal)));
      terminalControlButton2.Visible = (Func<MyProgrammableBlock, bool>) (b => MyFakes.ENABLE_PROGRAMMABLE_BLOCK && MySession.Static.EnableIngameScripts);
      terminalControlButton2.Enabled = (Func<MyProgrammableBlock, bool>) (b => b.IsWorking && b.IsFunctional);
      MyTerminalControlFactory.AddControl<MyProgrammableBlock>((MyTerminalControl<MyProgrammableBlock>) terminalControlButton2);
      MyTerminalControlButton<MyProgrammableBlock> terminalControlButton3 = new MyTerminalControlButton<MyProgrammableBlock>("Recompile", MySpaceTexts.TerminalControlPanel_Recompile, MySpaceTexts.TerminalControlPanel_Recompile_Tooltip, (Action<MyProgrammableBlock>) (b => b.SendRecompile()));
      terminalControlButton3.Visible = (Func<MyProgrammableBlock, bool>) (b => MyFakes.ENABLE_PROGRAMMABLE_BLOCK && MySession.Static.EnableIngameScripts);
      terminalControlButton3.Enabled = (Func<MyProgrammableBlock, bool>) (b => b.IsWorking && b.IsFunctional);
      MyTerminalControlFactory.AddControl<MyProgrammableBlock>((MyTerminalControl<MyProgrammableBlock>) terminalControlButton3);
      MyTerminalControlFactory.AddAction<MyProgrammableBlock>(new MyTerminalAction<MyProgrammableBlock>("Run", MyTexts.Get(MySpaceTexts.TerminalControlPanel_RunCode), new Action<MyProgrammableBlock, ListReader<TerminalActionParameter>>(MyProgrammableBlock.OnRunApplied), (MyTerminalControl<MyProgrammableBlock>.WriterDelegate) null, MyTerminalActionIcons.START)
      {
        Enabled = (Func<MyProgrammableBlock, bool>) (b => b.IsFunctional),
        DoUserParameterRequest = new Action<IList<TerminalActionParameter>, Action<bool>>(MyProgrammableBlock.RequestRunArgument),
        ParameterDefinitions = {
          TerminalActionParameter.Get((object) string.Empty)
        }
      });
      MyTerminalControlFactory.AddAction<MyProgrammableBlock>(new MyTerminalAction<MyProgrammableBlock>("RunWithDefaultArgument", MyTexts.Get(MySpaceTexts.TerminalControlPanel_RunCodeDefault), new Action<MyProgrammableBlock>(MyProgrammableBlock.OnRunDefaultApplied), MyTerminalActionIcons.START)
      {
        Enabled = (Func<MyProgrammableBlock, bool>) (b => b.IsFunctional)
      });
      MyMultiTextPanelComponent.CreateTerminalControls<MyProgrammableBlock>();
    }

    private static void OnRunApplied(
      MyProgrammableBlock programmableBlock,
      ListReader<TerminalActionParameter> parameters)
    {
      string str = (string) null;
      TerminalActionParameter terminalActionParameter = parameters.FirstOrDefault<TerminalActionParameter>();
      if (!terminalActionParameter.IsEmpty && terminalActionParameter.TypeCode == TypeCode.String)
        str = terminalActionParameter.Value as string;
      programmableBlock.Run(str, UpdateType.Trigger);
    }

    private static void OnRunDefaultApplied(MyProgrammableBlock programmableBlock) => programmableBlock.Run(programmableBlock.TerminalRunArgument, UpdateType.Trigger);

    private static void RequestRunArgument(
      IList<TerminalActionParameter> list,
      Action<bool> callback)
    {
      MyGuiScreenDialogText screenDialogText = new MyGuiScreenDialogText(string.Empty, new MyStringId?(MySpaceTexts.DialogText_RunArgument));
      screenDialogText.OnConfirmed += (Action<string>) (argument =>
      {
        list[0] = TerminalActionParameter.Get((object) argument);
        callback(true);
      });
      MyGuiSandbox.AddScreen((MyGuiScreenBase) screenDialogText);
    }

    private static string ToIndentedComment(string input) => "    // " + string.Join("\n    // ", input.Split(MyProgrammableBlock.NEW_LINES, StringSplitOptions.None));

    private void OpenEditor()
    {
      if (this.m_editorData == null)
        this.m_editorData = string.Format("public Program()\r\n{{\r\n{0}\r\n}}\r\n\r\npublic void Save()\r\n{{\r\n{1}\r\n}}\r\n\r\npublic void Main(string argument, UpdateType updateSource)\r\n{{\r\n{2}\r\n}}\r\n", (object) MyProgrammableBlock.ToIndentedComment(MyTexts.GetString(MySpaceTexts.ProgrammableBlock_DefaultScript_Constructor).Trim()), (object) MyProgrammableBlock.ToIndentedComment(MyTexts.GetString(MySpaceTexts.ProgrammableBlock_DefaultScript_Save).Trim()), (object) MyProgrammableBlock.ToIndentedComment(MyTexts.GetString(MySpaceTexts.ProgrammableBlock_DefaultScript_Main).Trim()));
      this.m_editorScreen = new MyGuiScreenEditor(this.m_editorData, new Action<VRage.Game.ModAPI.ResultEnum>(this.SaveCode), new Action(this.SaveCode));
      MyGuiScreenGamePlay.TmpGameplayScreenHolder = MyGuiScreenGamePlay.ActiveGameplayScreen;
      MyScreenManager.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) this.m_editorScreen);
    }

    private void SaveCode()
    {
      if (this.m_editorScreen.TextTooLong())
      {
        StringBuilder messageCaption = MyTexts.Get(MySpaceTexts.ProgrammableBlock_CodeChanged);
        MyScreenManager.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MySpaceTexts.ProgrammableBlock_Editor_TextTooLong), messageCaption: messageCaption, canHideOthers: false));
      }
      else
      {
        this.m_editorData = this.m_programData = this.m_editorScreen.Description.Text.ToString();
        if (Sync.IsServer)
          this.Recompile(true);
        else
          this.SendUpdateProgramRequest(this.m_programData);
      }
    }

    public void SendRecompile() => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProgrammableBlock, bool>(this, (Func<MyProgrammableBlock, Action<bool>>) (x => new Action<bool>(x.Recompile)), true);

    [Event(null, 326)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void Recompile(bool instantiate = true)
    {
      this.m_compilerErrors.Clear();
      this.m_echoOutput.Clear();
      this.UpdateStorage();
      this.Compile(this.m_programData, this.m_storageData, instantiate);
    }

    private void UpdateStorage()
    {
      if (this.m_instance == null)
        return;
      this.m_storageData = this.m_instance.Storage;
      if (!this.m_instance.HasSaveMethod)
        return;
      string response;
      int num = (int) this.RunSandboxedProgramAction((Action<IMyGridProgram>) (program =>
      {
        this.m_runtime.BeginSaveOperation();
        this.m_instance.ElapsedTime = TimeSpan.Zero;
        program.Save();
      }), out response);
      this.SetDetailedInfo(response);
      if (this.m_instance == null)
        return;
      this.m_storageData = this.m_instance.Storage;
    }

    private void SaveCode(VRage.Game.ModAPI.ResultEnum result)
    {
      MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiScreenGamePlay.TmpGameplayScreenHolder;
      MyGuiScreenGamePlay.TmpGameplayScreenHolder = (MyGuiScreenBase) null;
      this.SendCloseEditor();
      if (this.m_editorScreen.TextTooLong())
      {
        StringBuilder messageCaption = MyTexts.Get(MySpaceTexts.ProgrammableBlock_CodeChanged);
        MyScreenManager.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MySpaceTexts.ProgrammableBlock_Editor_TextTooLong), messageCaption: messageCaption, canHideOthers: false));
      }
      else
      {
        this.DetailedInfo.Clear();
        this.RaisePropertiesChanged();
        if (result == VRage.Game.ModAPI.ResultEnum.OK)
        {
          this.SaveCode();
        }
        else
        {
          if (!(this.m_editorScreen.Description.Text.ToString() != this.m_programData))
            return;
          StringBuilder messageCaption = MyTexts.Get(MySpaceTexts.ProgrammableBlock_CodeChanged);
          MyGuiScreenMessageBox messageBox = MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MySpaceTexts.ProgrammableBlock_SaveChanges), messageCaption: messageCaption, canHideOthers: false);
          messageBox.ResultCallback = (Action<MyGuiScreenMessageBox.ResultEnum>) (result2 =>
          {
            if (result2 == MyGuiScreenMessageBox.ResultEnum.YES)
              this.SaveCode(VRage.Game.ModAPI.ResultEnum.OK);
            else
              this.m_editorData = this.m_programData;
          });
          MyScreenManager.AddScreen((MyGuiScreenBase) messageBox);
        }
      }
    }

    public MyProgrammableBlock.ScriptTerminationReason ExecuteCode(
      string argument,
      UpdateType updateSource,
      out string response)
    {
      if (MySession.Static == null || MySession.Static.Settings == null || MySession.Static.EnableIngameScripts)
        return this.RunSandboxedProgramAction((Action<IMyGridProgram>) (program =>
        {
          this.m_runtime.BeginMainOperation();
          this.m_instance.ElapsedTime = this.m_runtime.TimeSinceLastRun;
          program.Main(argument, updateSource);
          this.m_runtime.EndMainOperation();
        }), out response);
      response = MyTexts.GetString("ProgrammableBlock_Error_ScriptsDisabled");
      return MyProgrammableBlock.ScriptTerminationReason.None;
    }

    public MyProgrammableBlock.ScriptTerminationReason RunSandboxedProgramAction(
      Action<IMyGridProgram> action,
      out string response)
    {
      if (MySandboxGame.Static.UpdateThread != Thread.CurrentThread && MyVRage.Platform.Scripting.ReportIncorrectBehaviour(MyCommonTexts.ModRuleViolation_PBParallelInvocation))
        MyLog.Default.Log(MyLogSeverity.Error, "PB invoked from parallel thread (logged only once)!" + Environment.NewLine + Environment.StackTrace);
      if (this.m_isRunning)
      {
        response = MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Exception_AllreadyRunning);
        return MyProgrammableBlock.ScriptTerminationReason.AlreadyRunning;
      }
      if (this.m_terminationReason != MyProgrammableBlock.ScriptTerminationReason.None)
      {
        response = this.DetailedInfo.ToString();
        return this.m_terminationReason;
      }
      this.DetailedInfo.Clear();
      this.m_echoOutput.Clear();
      if (this.m_assembly == (Assembly) null)
      {
        response = MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Exception_NoAssembly);
        return MyProgrammableBlock.ScriptTerminationReason.NoScript;
      }
      if (this.m_instance == null)
      {
        if (this.m_needsInstantiation && this.CheckIsWorking() && this.Enabled)
        {
          this.m_needsInstantiation = false;
          this.CreateInstance(this.m_assembly, (IEnumerable<string>) this.m_compilerErrors, this.m_storageData);
          if (this.m_instance == null)
          {
            response = this.DetailedInfo.ToString();
            return this.m_terminationReason;
          }
        }
        else
        {
          response = MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Exception_NoAssembly);
          return MyProgrammableBlock.ScriptTerminationReason.NoScript;
        }
      }
      MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(this.CubeGrid);
      MyGridTerminalSystem terminalSystem = group.GroupData.TerminalSystem;
      this.m_terminalWrapper.SetInstance(terminalSystem);
      MyCubeGridGroups.Static.GetGroups(GridLinkTypeEnum.Logical).GetGroupNodes(this.CubeGrid, this.m_groupCache);
      group.GroupData.UpdateGridOwnership(this.m_groupCache, this.OwnerId);
      this.m_groupCache.Clear();
      if (terminalSystem != null)
        terminalSystem.UpdateGridBlocksOwnership(this.OwnerId);
      else
        MyLog.Default.Critical("Probrammable block terminal system is null! Crash");
      this.m_instance.GridTerminalSystem = (Sandbox.ModAPI.Ingame.IMyGridTerminalSystem) this.m_terminalWrapper;
      return this.RunSandboxedProgramActionCore(action, out response);
    }

    private MyProgrammableBlock.ScriptTerminationReason RunSandboxedProgramActionCore(
      Action<IMyGridProgram> action,
      out string response)
    {
      this.m_isRunning = true;
      response = "";
      try
      {
        IlInjector.ICounterHandle counterHandle = IlInjector.BeginRunBlock(50000, 10000);
        int num = counterHandle.Depth - 1;
        try
        {
          this.m_runtime.InjectorHandle = counterHandle;
          action(this.m_instance);
        }
        finally
        {
          counterHandle.Dispose();
          if (counterHandle.Depth != num)
            MyLog.Default.Log(MyLogSeverity.Error, "PB {0} invoke depth leak: {1} -> {2}", (object) this.EntityId, (object) num, (object) counterHandle.Depth);
        }
        if (this.m_echoOutput.Length > 0)
          response = this.m_echoOutput.ToString();
        return this.m_terminationReason;
      }
      catch (Exception ex)
      {
        Exception exception = ex;
        if (exception is TargetInvocationException)
          exception = exception.InnerException;
        if (this.m_echoOutput.Length > 0)
          response = this.m_echoOutput.ToString();
        if (exception is ScriptOutOfRangeException)
        {
          if (IlInjector.IsWithinRunBlock())
          {
            response += MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Exception_NestedTooComplex);
            return MyProgrammableBlock.ScriptTerminationReason.InstructionOverflow;
          }
          response += MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Exception_TooComplex);
          this.OnProgramTermination(MyProgrammableBlock.ScriptTerminationReason.InstructionOverflow);
        }
        else
        {
          string fullName = typeof (MyGridProgram).FullName;
          string str = exception.StackTrace;
          int num = str.IndexOf(fullName);
          if (num > 0)
          {
            int length = str.LastIndexOf(MyEnvironment.NewLine, num, num, StringComparison.InvariantCulture);
            str = str.Substring(0, length);
          }
          response = response + MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Exception_ExceptionCaught) + exception.Message + "\n" + str;
          this.OnProgramTermination(MyProgrammableBlock.ScriptTerminationReason.RuntimeException);
        }
        return this.m_terminationReason;
      }
      finally
      {
        this.m_runtime.InjectorHandle = (IlInjector.ICounterHandle) null;
        this.m_isRunning = false;
      }
    }

    private void OnProgramTermination(MyProgrammableBlock.ScriptTerminationReason reason)
    {
      this.m_terminationReason = reason;
      this.m_instance = (IMyGridProgram) null;
      this.m_assembly = (Assembly) null;
      this.m_echoOutput.Clear();
      this.m_runtime.Reset();
    }

    public void Run(string argument, UpdateType updateSource)
    {
      if (!this.IsWorking || !this.IsFunctional || this.CubeGrid.Physics == null)
        return;
      MySimpleProfiler.Begin("Scripts", MySimpleProfiler.ProfilingBlockType.BLOCK, nameof (Run));
      if (Sync.IsServer)
      {
        string response;
        int num = (int) this.ExecuteCode(argument, updateSource, out response);
        this.SetDetailedInfo(response);
      }
      else
        this.SendRunProgramRequest(argument, updateSource);
      MySimpleProfiler.End(nameof (Run));
    }

    private void SetDetailedInfo(string detailedInfo)
    {
      if (!(this.DetailedInfo.ToString() != detailedInfo))
        return;
      if (this.m_subscribers.Count != 0)
      {
        foreach (EndpointId subscriber in this.m_subscribers)
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProgrammableBlock, string>(this, (Func<MyProgrammableBlock, Action<string>>) (x => new Action<string>(x.WriteProgramResponseForSubscribers)), detailedInfo, subscriber);
      }
      this.WriteProgramResponseForSubscribers(detailedInfo);
    }

    [Event(null, 638)]
    [Reliable]
    [Client]
    private void WriteProgramResponseForSubscribers(string response)
    {
      this.DetailedInfo.Clear();
      this.DetailedInfo.Append(response);
      this.RaisePropertiesChanged();
    }

    [Event(null, 646)]
    [Reliable]
    [Server]
    private void SubscribeToProgramResponse(bool register)
    {
      if (register)
      {
        EndpointId sender = MyEventContext.Current.Sender;
        this.m_subscribers.Add(sender);
        MySandboxGame.Static.Invoke((Action) (() => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProgrammableBlock, string>(this, (Func<MyProgrammableBlock, Action<string>>) (x => new Action<string>(x.WriteProgramResponseForSubscribers)), this.DetailedInfo.ToString(), sender)), nameof (SubscribeToProgramResponse));
      }
      else
        this.m_subscribers.Remove(MyEventContext.Current.Sender);
    }

    public override void OnOpenedInTerminal(bool state) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProgrammableBlock, bool>(this, (Func<MyProgrammableBlock, Action<bool>>) (x => new Action<bool>(x.SubscribeToProgramResponse)), state);

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      MyProgrammableBlockDefinition blockDefinition = this.BlockDefinition as MyProgrammableBlockDefinition;
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(blockDefinition.ResourceSinkGroup, 0.0005f, (Func<float>) (() => !this.Enabled || !this.IsFunctional ? 0.0f : this.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId)));
      resourceSinkComponent.IsPoweredChanged += new Action(this.PowerReceiver_IsPoweredChanged);
      this.ResourceSink = resourceSinkComponent;
      base.Init(objectBuilder, cubeGrid);
      MyObjectBuilder_MyProgrammableBlock programmableBlock = (MyObjectBuilder_MyProgrammableBlock) objectBuilder;
      this.m_editorData = this.m_programData = programmableBlock.Program;
      this.m_storageData = programmableBlock.Storage;
      this.m_terminalRunArgument = programmableBlock.DefaultRunArgument;
      if (Sync.IsServer && !string.IsNullOrEmpty(this.m_programData))
      {
        this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
        this.m_needsInstantiation = true;
      }
      this.m_runtime = new MyProgrammableBlock.RuntimeInfo(this);
      this.ScriptComponent = this.GameLogic.GetAs<MyIngameScriptComponent>();
      this.ResourceSink.Update();
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      if (Sync.IsServer && Sync.Clients != null)
        Sync.Clients.ClientRemoved += new Action<ulong>(this.ProgrammableBlock_ClientRemoved);
      if (blockDefinition.ScreenAreas != null && blockDefinition.ScreenAreas.Count > 0)
      {
        this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
        this.m_multiPanel = new MyMultiTextPanelComponent((MyTerminalBlock) this, blockDefinition.ScreenAreas, programmableBlock.TextPanels);
        this.m_multiPanel.Init(new Action<int, int[]>(this.SendAddImagesToSelectionRequest), new Action<int, int[]>(this.SendRemoveSelectedImageRequest), new Action<int, string>(this.ChangeTextRequest), new Action<int, MySerializableSpriteCollection>(this.UpdateSpriteCollection));
      }
      if (!Sync.IsServer || !MySession.Static.EnableIngameScripts || (this.m_programData == null || !(this.m_assembly == (Assembly) null)))
        return;
      this.Recompile(false);
    }

    protected override void Closing()
    {
      base.Closing();
      if (this.m_multiPanel != null)
        this.m_multiPanel.SetRender((MyRenderComponentScreenAreas) null);
      if (Sync.Clients == null)
        return;
      Sync.Clients.ClientRemoved -= new Action<ulong>(this.ProgrammableBlock_ClientRemoved);
    }

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      if (Sync.IsServer && this.m_needsInstantiation)
      {
        this.m_needsInstantiation = false;
        if (MySession.Static.EnableIngameScripts)
        {
          if (this.m_programData != null && this.m_assembly == (Assembly) null)
            this.Recompile(false);
          if (this.m_assembly != (Assembly) null && this.m_instance == null)
            this.CreateInstance(this.m_assembly, (IEnumerable<string>) this.m_compilerErrors, this.m_storageData);
        }
        else
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProgrammableBlock, string>(this, (Func<MyProgrammableBlock, Action<string>>) (x => new Action<string>(x.WriteProgramResponse)), MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Exception_NotAllowed));
      }
      if (this.HasDamageEffect)
        return;
      this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
    }

    public override void OnModelChange()
    {
      base.OnModelChange();
      if (this.m_multiPanel != null)
        this.m_multiPanel.Reset();
      if (this.ResourceSink == null)
        return;
      this.UpdateScreen();
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_MyProgrammableBlock builderCubeBlock = (MyObjectBuilder_MyProgrammableBlock) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.Program = this.m_programData;
      builderCubeBlock.DefaultRunArgument = this.m_terminalRunArgument;
      if (Sync.IsServer)
      {
        this.UpdateStorage();
        builderCubeBlock.Storage = this.m_instance == null ? this.m_storageData : this.m_instance.Storage;
      }
      if (this.m_multiPanel != null)
        builderCubeBlock.TextPanels = this.m_multiPanel.Serialize();
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    private void Compile(string program, string storage, bool instantiate = false)
    {
      if (!Sync.IsServer)
        return;
      this.ScriptComponent.NeedsUpdate = MyEntityUpdateEnum.NONE;
      this.ScriptComponent.NextUpdate = UpdateType.None;
      if (!MySession.Static.EnableIngameScripts || this.CubeGrid.IsPreview || !this.CubeGrid.CreatePhysics)
        return;
      this.m_terminationReason = MyProgrammableBlock.ScriptTerminationReason.None;
      try
      {
        List<VRage.Scripting.Message> diagnostics;
        this.m_assembly = MyVRage.Platform.Scripting.CompileIngameScriptAsync(Path.Combine(MyFileSystem.UserDataPath, this.GetAssemblyName()), program, out diagnostics, "PB: " + this.DisplayName + " (" + (object) this.EntityId + ")", "Program", typeof (MyGridProgram).Name).Result;
        this.m_compilerErrors.Clear();
        this.m_compilerErrors.AddRange(diagnostics.Select<VRage.Scripting.Message, string>((Func<VRage.Scripting.Message, string>) (m => m.Text)));
        if (!instantiate)
          return;
        this.CreateInstance(this.m_assembly, (IEnumerable<string>) this.m_compilerErrors, storage);
      }
      catch (Exception ex)
      {
        this.SetDetailedInfo(MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Exception_ExceptionCaught) + ex.Message);
      }
    }

    private string GetAssemblyName()
    {
      char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(this.EntityId);
      stringBuilder.Append("-");
      for (int index = 0; index < this.CustomName.Length; ++index)
      {
        char element = this.CustomName[index];
        if (invalidFileNameChars.Contains<char>(element))
          stringBuilder.Append("_");
        else
          stringBuilder.Append(element);
      }
      stringBuilder.Append(".dll");
      return stringBuilder.ToString();
    }

    private bool CreateInstance(Assembly assembly, IEnumerable<string> messages, string storage)
    {
      this.m_needsInstantiation = false;
      string response = string.Join("\n", messages);
      if (assembly == (Assembly) null)
        return false;
      Type type = assembly.GetType("Program");
      if (type != (Type) null)
      {
        if (this.RunSandboxedProgramActionCore((Action<IMyGridProgram>) (_ => this.m_instance = FormatterServices.GetUninitializedObject(type) as IMyGridProgram), out response) != MyProgrammableBlock.ScriptTerminationReason.None)
        {
          this.SetDetailedInfo(response);
          return false;
        }
        ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Type.EmptyTypes, (ParameterModifier[]) null);
        if (this.m_instance == null || constructor == (ConstructorInfo) null)
        {
          response = MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Exception_NoValidConstructor) + "\n\n" + response;
          this.SetDetailedInfo(response);
          return false;
        }
        this.m_runtime.Reset();
        this.m_instance.Runtime = (IMyGridProgramRuntimeInfo) this.m_runtime;
        this.m_instance.Storage = storage;
        this.m_instance.Me = (Sandbox.ModAPI.Ingame.IMyProgrammableBlock) this;
        this.m_instance.Echo = new Action<string>(this.EchoTextToDetailInfo);
        MyIGCSystemSessionComponent.Static.EvictContextFor(this);
        MyIntergridCommunicationContext m_IGCContextCache = (MyIntergridCommunicationContext) null;
        this.m_instance.IGC_ContextGetter = (Func<IMyIntergridCommunicationSystem>) (() =>
        {
          if (m_IGCContextCache == null)
            m_IGCContextCache = MyIGCSystemSessionComponent.Static.GetOrMakeContextFor(this);
          return (IMyIntergridCommunicationSystem) m_IGCContextCache;
        });
        int num = (int) this.RunSandboxedProgramAction((Action<IMyGridProgram>) (p =>
        {
          constructor.Invoke((object) p, (object[]) null);
          if (this.m_instance.HasMainMethod)
            return;
          if (this.m_echoOutput.Length > 0)
            response = response + "\n\n" + this.m_echoOutput.ToString();
          response = MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Exception_NoMain) + "\n\n" + response;
          this.OnProgramTermination(MyProgrammableBlock.ScriptTerminationReason.NoEntryPoint);
        }), out response);
        this.SetDetailedInfo(response);
      }
      return true;
    }

    private void EchoTextToDetailInfo(string line)
    {
      line = line ?? string.Empty;
      int num1 = line.Length + 1;
      if (num1 > 8000)
      {
        this.m_echoOutput.Clear();
        line = line.Substring(0, 8000);
        num1 = 8000;
      }
      int num2 = this.m_echoOutput.Length + num1;
      if (num2 > 8000)
        this.m_echoOutput.Remove(0, num2 - 8000);
      this.m_echoOutput.Append(line);
      this.m_echoOutput.Append('\n');
    }

    private void ShowEditorAllReadyOpen() => MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder("Editor is opened by another player.")));

    public void UpdateProgram(string program)
    {
      this.m_editorData = this.m_programData = program;
      if (!Sync.IsServer)
        return;
      this.Recompile(true);
    }

    [Event(null, 980)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void WriteProgramResponse(string response)
    {
      this.DetailedInfo.Clear();
      this.DetailedInfo.Append(response);
      this.RaisePropertiesChanged();
    }

    protected override void OnOwnershipChanged()
    {
      base.OnOwnershipChanged();
      if (!MySession.Static.SurvivalMode)
        return;
      this.OnProgramTermination(MyProgrammableBlock.ScriptTerminationReason.OwnershipChange);
      if (!Sync.IsServer)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProgrammableBlock, string>(this, (Func<MyProgrammableBlock, Action<string>>) (x => new Action<string>(x.WriteProgramResponse)), MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Exception_Ownershipchanged));
    }

    private void PowerReceiver_IsPoweredChanged()
    {
      this.UpdateIsWorking();
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    protected override bool CheckIsWorking()
    {
      int num = !this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) ? 0 : (base.CheckIsWorking() ? 1 : 0);
      if (num == 0)
        return num != 0;
      if (!this.m_needsInstantiation)
        return num != 0;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
      return num != 0;
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      this.ResourceSink.Update();
    }

    public override void UpdateAfterSimulation10()
    {
      base.UpdateAfterSimulation10();
      if (this.m_multiPanel == null)
        return;
      this.m_multiPanel.UpdateAfterSimulation(this.IsWorking);
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      this.UpdateScreen();
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      if (this.m_multiPanel == null)
        return;
      this.m_multiPanel.AddToScene();
    }

    private void UpdateScreen() => this.m_multiPanel?.UpdateScreen(this.IsWorking);

    private void ComponentStack_IsFunctionalChanged() => this.ResourceSink.Update();

    public void ProgrammableBlock_ClientRemoved(ulong playerId)
    {
      if ((long) playerId != (long) this.m_userId)
        return;
      this.SendCloseEditor();
    }

    protected override void OnEnabledChanged()
    {
      this.ResourceSink.Update();
      base.OnEnabledChanged();
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    private void SendOpenEditorRequest()
    {
      if (Sync.IsServer)
      {
        if (!this.m_consoleOpen)
        {
          this.m_consoleOpen = true;
          this.OpenEditor();
        }
        else
          this.ShowEditorAllReadyOpen();
      }
      else
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProgrammableBlock>(this, (Func<MyProgrammableBlock, Action>) (x => new Action(x.OpenEditorRequest)));
    }

    [Event(null, 1092)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void OpenEditorRequest()
    {
      if (!this.m_consoleOpen)
      {
        this.UserId = MyEventContext.Current.Sender.Value;
        this.m_consoleOpen = true;
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProgrammableBlock>(this, (Func<MyProgrammableBlock, Action>) (x => new Action(x.OpenEditorSucess)), new EndpointId(this.UserId));
      }
      else
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProgrammableBlock>(this, (Func<MyProgrammableBlock, Action>) (x => new Action(x.OpenEditorFailure)), new EndpointId(this.UserId));
    }

    [Event(null, 1107)]
    [Reliable]
    [Client]
    private void OpenEditorSucess() => this.OpenEditor();

    [Event(null, 1113)]
    [Reliable]
    [Client]
    private void OpenEditorFailure() => this.ShowEditorAllReadyOpen();

    private void SendCloseEditor()
    {
      if (Sync.IsServer)
        this.m_consoleOpen = false;
      else
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProgrammableBlock>(this, (Func<MyProgrammableBlock, Action>) (x => new Action(x.CloseEditor)));
    }

    [Event(null, 1131)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void CloseEditor() => this.m_consoleOpen = false;

    private void SendUpdateProgramRequest(string program) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProgrammableBlock, byte[]>(this, (Func<MyProgrammableBlock, Action<byte[]>>) (x => new Action<byte[]>(x.UpdateProgram)), StringCompressor.CompressString(program));

    [Event(null, 1142)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void UpdateProgram(byte[] program)
    {
      string program1 = StringCompressor.DecompressString(program);
      if (Sync.IsServer && (program1.Length > 100000 ? 0 : (MyEventContext.Current.IsLocallyInvoked ? 1 : (MySession.Static.IsUserScripter(MyEventContext.Current.Sender.Value) ? 1 : 0))) == 0)
        MyEventContext.ValidationFailed();
      else
        this.UpdateProgram(program1);
    }

    private void SendRunProgramRequest(string argument, UpdateType updateSource) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProgrammableBlock, byte[], UpdateType>(this, (Func<MyProgrammableBlock, Action<byte[], UpdateType>>) (x => new Action<byte[], UpdateType>(x.RunProgramRequest)), StringCompressor.CompressString(argument ?? string.Empty), updateSource);

    [Event(null, 1167)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void RunProgramRequest(byte[] argument, UpdateType updateType) => this.Run(StringCompressor.DecompressString(argument), updateType);

    bool Sandbox.ModAPI.Ingame.IMyProgrammableBlock.TryRun(string argument)
    {
      if (this.m_instance == null || this.m_isRunning || (!this.IsWorking || !this.IsFunctional) || (!this.IsFunctional || !this.IsWorking))
        return false;
      string response;
      int num = (int) this.ExecuteCode(argument ?? "", UpdateType.Script, out response);
      this.SetDetailedInfo(response);
      if (num == 3)
        throw new ScriptOutOfRangeException("This exception crashes the game only when Mod runs programmable block with script that exceeds the stack limit. Consider catching this exception or rewriting your script.");
      return num == 0;
    }

    bool Sandbox.ModAPI.Ingame.IMyProgrammableBlock.IsRunning => this.m_isRunning;

    void Sandbox.ModAPI.IMyProgrammableBlock.Recompile() => this.SendRecompile();

    void Sandbox.ModAPI.IMyProgrammableBlock.Run() => this.Run(this.TerminalRunArgument, UpdateType.Mod);

    void Sandbox.ModAPI.IMyProgrammableBlock.Run(string argument) => this.Run(argument, UpdateType.Mod);

    void Sandbox.ModAPI.IMyProgrammableBlock.Run(
      string argument,
      UpdateType updateSource)
    {
      this.Run(argument, updateSource);
    }

    bool Sandbox.ModAPI.IMyProgrammableBlock.TryRun(string argument)
    {
      if (this.m_instance == null || this.m_isRunning || (!this.IsWorking || !this.IsFunctional) || (!this.IsFunctional || !this.IsWorking))
        return false;
      string response;
      int num = (int) this.ExecuteCode(argument ?? "", UpdateType.Mod, out response);
      this.SetDetailedInfo(response);
      if (num == 3)
        throw new ScriptOutOfRangeException("This exception crashes the game only when Mod runs programmable block with script that exceeds the stack limit. Consider catching this exception or rewriting your script.");
      return num == 0;
    }

    string Sandbox.ModAPI.IMyProgrammableBlock.ProgramData
    {
      get => this.m_programData;
      set
      {
        this.m_editorData = this.m_programData = value;
        if (Sync.IsServer)
          this.Recompile(true);
        else
          this.SendUpdateProgramRequest(this.m_programData);
      }
    }

    string Sandbox.ModAPI.IMyProgrammableBlock.StorageData
    {
      get => this.m_instance == null ? (string) null : this.m_instance.Storage;
      set
      {
        if (this.m_instance == null)
          return;
        this.m_instance.Storage = value;
      }
    }

    bool Sandbox.ModAPI.IMyProgrammableBlock.HasCompileErrors => this.m_compilerErrors.Count > 0;

    int Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider.SurfaceCount => this.m_multiPanel == null ? 0 : this.m_multiPanel.SurfaceCount;

    Sandbox.ModAPI.Ingame.IMyTextSurface Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider.GetSurface(
      int index)
    {
      return this.m_multiPanel == null ? (Sandbox.ModAPI.Ingame.IMyTextSurface) null : this.m_multiPanel.GetSurface(index);
    }

    public override void UpdateVisual()
    {
      base.UpdateVisual();
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    protected override void OnStartWorking() => this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;

    protected override void OnStopWorking() => this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;

    private void SendRemoveSelectedImageRequest(int panelIndex, int[] selection) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProgrammableBlock, int, int[]>(this, (Func<MyProgrammableBlock, Action<int, int[]>>) (x => new Action<int, int[]>(x.OnRemoveSelectedImageRequest)), panelIndex, selection);

    [Event(null, 1503)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnRemoveSelectedImageRequest(int panelIndex, int[] selection) => this.m_multiPanel?.RemoveItems(panelIndex, selection);

    private void SendAddImagesToSelectionRequest(int panelIndex, int[] selection) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProgrammableBlock, int, int[]>(this, (Func<MyProgrammableBlock, Action<int, int[]>>) (x => new Action<int, int[]>(x.OnSelectImageRequest)), panelIndex, selection);

    [Event(null, 1514)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnSelectImageRequest(int panelIndex, int[] selection) => this.m_multiPanel?.SelectItems(panelIndex, selection);

    private void ChangeTextRequest(int panelIndex, string text) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProgrammableBlock, int, string>(this, (Func<MyProgrammableBlock, Action<int, string>>) (x => new Action<int, string>(x.OnChangeTextRequest)), panelIndex, text);

    [Event(null, 1525)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnChangeTextRequest(int panelIndex, [Nullable] string text) => this.m_multiPanel?.ChangeText(panelIndex, text);

    private void UpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites)
    {
      if (!Sync.IsServer)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProgrammableBlock, int, MySerializableSpriteCollection>(this, (Func<MyProgrammableBlock, Action<int, MySerializableSpriteCollection>>) (x => new Action<int, MySerializableSpriteCollection>(x.OnUpdateSpriteCollection)), panelIndex, sprites);
    }

    [Event(null, 1539)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    [DistanceRadius(32f)]
    private void OnUpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites) => this.m_multiPanel?.UpdateSpriteCollection(panelIndex, sprites);

    void IMyMultiTextPanelComponentOwner.SelectPanel(
      List<MyGuiControlListbox.Item> panelItems)
    {
      if (this.m_multiPanel != null)
        this.m_multiPanel.SelectPanel((int) panelItems[0].UserData);
      this.RaisePropertiesChanged();
    }

    MyMultiTextPanelComponent IMyMultiTextPanelComponentOwner.MultiTextPanel => this.m_multiPanel;

    public MyTextPanelComponent PanelComponent => this.m_multiPanel == null ? (MyTextPanelComponent) null : this.m_multiPanel.PanelComponent;

    public bool IsTextPanelOpen
    {
      get => this.m_isTextPanelOpen;
      set
      {
        if (this.m_isTextPanelOpen == value)
          return;
        this.m_isTextPanelOpen = value;
        this.RaisePropertiesChanged();
      }
    }

    public void OpenWindow(bool isEditable, bool sync, bool isPublic)
    {
      if (sync)
      {
        this.SendChangeOpenMessage(true, isEditable, Sync.MyId, isPublic);
      }
      else
      {
        this.CreateTextBox(isEditable, new StringBuilder(this.PanelComponent.Text.ToString()), isPublic);
        MyGuiScreenGamePlay.TmpGameplayScreenHolder = MyGuiScreenGamePlay.ActiveGameplayScreen;
        MyScreenManager.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) this.m_textBoxMultiPanel);
      }
    }

    private void SendChangeOpenMessage(bool isOpen, bool editable = false, ulong user = 0, bool isPublic = false) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProgrammableBlock, bool, bool, ulong, bool>(this, (Func<MyProgrammableBlock, Action<bool, bool, ulong, bool>>) (x => new Action<bool, bool, ulong, bool>(x.OnChangeOpenRequest)), isOpen, editable, user, isPublic);

    [Event(null, 1594)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void OnChangeOpenRequest(bool isOpen, bool editable, ulong user, bool isPublic)
    {
      if (((!Sync.IsServer ? 0 : (this.IsTextPanelOpen ? 1 : 0)) & (isOpen ? 1 : 0)) != 0)
        return;
      this.OnChangeOpen(isOpen, editable, user, isPublic);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProgrammableBlock, bool, bool, ulong, bool>(this, (Func<MyProgrammableBlock, Action<bool, bool, ulong, bool>>) (x => new Action<bool, bool, ulong, bool>(x.OnChangeOpenSuccess)), isOpen, editable, user, isPublic);
    }

    [Event(null, 1605)]
    [Reliable]
    [Broadcast]
    private void OnChangeOpenSuccess(bool isOpen, bool editable, ulong user, bool isPublic) => this.OnChangeOpen(isOpen, editable, user, isPublic);

    private void OnChangeOpen(bool isOpen, bool editable, ulong user, bool isPublic)
    {
      this.IsTextPanelOpen = isOpen;
      if (((Sandbox.Engine.Platform.Game.IsDedicated ? 0 : ((long) user == (long) Sync.MyId ? 1 : 0)) & (isOpen ? 1 : 0)) == 0)
        return;
      this.OpenWindow(editable, false, isPublic);
    }

    private void CreateTextBox(bool isEditable, StringBuilder description, bool isPublic)
    {
      string displayNameText = this.DisplayNameText;
      string displayName = this.PanelComponent.DisplayName;
      string description1 = description.ToString();
      bool flag = isEditable;
      Action<VRage.Game.ModAPI.ResultEnum> resultCallback = new Action<VRage.Game.ModAPI.ResultEnum>(this.OnClosedPanelTextBox);
      int num = flag ? 1 : 0;
      this.m_textBoxMultiPanel = new MyGuiScreenTextPanel(displayNameText, "", displayName, description1, resultCallback, editable: (num != 0));
    }

    public void OnClosedPanelTextBox(VRage.Game.ModAPI.ResultEnum result)
    {
      if (this.m_textBoxMultiPanel == null)
        return;
      if (this.m_textBoxMultiPanel.Description.Text.Length > 100000)
      {
        Action<MyGuiScreenMessageBox.ResultEnum> callback = new Action<MyGuiScreenMessageBox.ResultEnum>(this.OnClosedPanelMessageBox);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.MessageBoxTextTooLongText), callback: callback));
      }
      else
        this.CloseWindow(true);
    }

    public void OnClosedPanelMessageBox(MyGuiScreenMessageBox.ResultEnum result)
    {
      if (result == MyGuiScreenMessageBox.ResultEnum.YES)
      {
        this.m_textBoxMultiPanel.Description.Text.Remove(100000, this.m_textBoxMultiPanel.Description.Text.Length - 100000);
        this.CloseWindow(true);
      }
      else
      {
        this.CreateTextBox(true, this.m_textBoxMultiPanel.Description.Text, true);
        MyScreenManager.AddScreen((MyGuiScreenBase) this.m_textBoxMultiPanel);
      }
    }

    private void CloseWindow(bool isPublic)
    {
      MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiScreenGamePlay.TmpGameplayScreenHolder;
      MyGuiScreenGamePlay.TmpGameplayScreenHolder = (MyGuiScreenBase) null;
      foreach (MySlimBlock cubeBlock in this.CubeGrid.CubeBlocks)
      {
        if (cubeBlock.FatBlock != null && cubeBlock.FatBlock.EntityId == this.EntityId)
        {
          this.SendChangeDescriptionMessage(this.m_textBoxMultiPanel.Description.Text, isPublic);
          this.SendChangeOpenMessage(false);
          break;
        }
      }
    }

    private void SendChangeDescriptionMessage(StringBuilder description, bool isPublic)
    {
      if (this.CubeGrid.IsPreview || !this.CubeGrid.SyncFlag)
      {
        this.PanelComponent.Text = description;
      }
      else
      {
        if (description.CompareTo(this.PanelComponent.Text) == 0)
          return;
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProgrammableBlock, string, bool>(this, (Func<MyProgrammableBlock, Action<string, bool>>) (x => new Action<string, bool>(x.OnChangeDescription)), description.ToString(), isPublic);
      }
    }

    [Event(null, 1698)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    public void OnChangeDescription(string description, bool isPublic)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Clear().Append(description);
      this.PanelComponent.Text = stringBuilder;
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    public enum ScriptTerminationReason
    {
      None,
      NoScript,
      NoEntryPoint,
      InstructionOverflow,
      OwnershipChange,
      RuntimeException,
      AlreadyRunning,
    }

    private class RuntimeInfo : IMyGridProgramRuntimeInfo
    {
      private static readonly double STOPWATCH_MS_FREQUENCY = 1000.0 / (double) Stopwatch.Frequency;
      private const long TIMESPAN_TICKS_PER_FRAME = 166666;
      private long m_startTicks;
      private int m_lastRunFrame;
      private readonly MyProgrammableBlock m_block;

      public IlInjector.ICounterHandle InjectorHandle { get; set; }

      public TimeSpan TimeSinceLastRun => new TimeSpan((long) (MySession.Static.GameplayFrameCounter - this.m_lastRunFrame) * 166666L);

      public double LastRunTimeMs { get; private set; }

      public int MaxInstructionCount => this.InjectorHandle.MaxInstructionCount;

      public int CurrentInstructionCount => this.InjectorHandle.InstructionCount;

      public int MaxCallChainDepth => this.InjectorHandle.MaxMethodCallCount;

      public int CurrentCallChainDepth => this.InjectorHandle.MethodCallCount;

      public UpdateFrequency UpdateFrequency
      {
        get
        {
          UpdateFrequency updateFrequency = UpdateFrequency.None;
          MyEntityUpdateEnum needsUpdate = this.m_block.ScriptComponent.NeedsUpdate;
          if (needsUpdate.HasFlag((Enum) MyEntityUpdateEnum.EACH_FRAME))
            updateFrequency |= UpdateFrequency.Update1;
          if (needsUpdate.HasFlag((Enum) MyEntityUpdateEnum.EACH_10TH_FRAME))
            updateFrequency |= UpdateFrequency.Update10;
          if (needsUpdate.HasFlag((Enum) MyEntityUpdateEnum.EACH_100TH_FRAME))
            updateFrequency |= UpdateFrequency.Update100;
          if (this.m_block.ScriptComponent.NextUpdate.HasFlag((Enum) UpdateType.Once))
            updateFrequency |= UpdateFrequency.Once;
          return updateFrequency;
        }
        set
        {
          if ((value & ~(UpdateFrequency.Update1 | UpdateFrequency.Update10 | UpdateFrequency.Update100 | UpdateFrequency.Once)) != UpdateFrequency.None)
            throw new ArgumentException("Unsupported flags in UpdateFrequency");
          if (value == UpdateFrequency.None)
          {
            this.m_block.ScriptComponent.NextUpdate = UpdateType.None;
          }
          else
          {
            MyEntityUpdateEnum needsUpdate = this.m_block.ScriptComponent.NeedsUpdate;
            MyEntityUpdateEnum entityUpdateEnum1 = !value.HasFlag((Enum) UpdateFrequency.Update1) ? needsUpdate & ~MyEntityUpdateEnum.EACH_FRAME : needsUpdate | MyEntityUpdateEnum.EACH_FRAME;
            MyEntityUpdateEnum entityUpdateEnum2 = !value.HasFlag((Enum) UpdateFrequency.Update10) ? entityUpdateEnum1 & ~MyEntityUpdateEnum.EACH_10TH_FRAME : entityUpdateEnum1 | MyEntityUpdateEnum.EACH_10TH_FRAME;
            MyEntityUpdateEnum entityUpdateEnum3 = !value.HasFlag((Enum) UpdateFrequency.Update100) ? entityUpdateEnum2 & ~MyEntityUpdateEnum.EACH_100TH_FRAME : entityUpdateEnum2 | MyEntityUpdateEnum.EACH_100TH_FRAME;
            if (value.HasFlag((Enum) UpdateFrequency.Once))
            {
              this.m_block.ScriptComponent.NextUpdate |= UpdateType.Once;
              entityUpdateEnum3 |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
            }
            else
              this.m_block.ScriptComponent.NextUpdate &= ~UpdateType.Once;
            this.m_block.ScriptComponent.NeedsUpdate = entityUpdateEnum3;
          }
        }
      }

      public RuntimeInfo(MyProgrammableBlock block) => this.m_block = block;

      public void Reset()
      {
        this.m_startTicks = 0L;
        this.LastRunTimeMs = 0.0;
        this.m_lastRunFrame = MySession.Static.GameplayFrameCounter;
      }

      public void BeginMainOperation() => this.m_startTicks = Stopwatch.GetTimestamp();

      public void EndMainOperation()
      {
        long timestamp = Stopwatch.GetTimestamp();
        this.m_lastRunFrame = MySession.Static.GameplayFrameCounter;
        this.LastRunTimeMs = (double) (timestamp - this.m_startTicks) * MyProgrammableBlock.RuntimeInfo.STOPWATCH_MS_FREQUENCY;
      }

      public void BeginSaveOperation() => this.LastRunTimeMs = 0.0;
    }

    public class MyGridTerminalWrapper : Sandbox.ModAPI.Ingame.IMyGridTerminalSystem
    {
      private Sandbox.ModAPI.Ingame.IMyGridTerminalSystem m_terminalInstance;

      internal void SetInstance(MyGridTerminalSystem terminalSystem) => this.m_terminalInstance = (Sandbox.ModAPI.Ingame.IMyGridTerminalSystem) terminalSystem;

      void Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlocks(
        List<Sandbox.ModAPI.Ingame.IMyTerminalBlock> blocks)
      {
        this.m_terminalInstance.GetBlocks(blocks);
      }

      void Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlockGroups(
        List<Sandbox.ModAPI.Ingame.IMyBlockGroup> blockGroups,
        Func<Sandbox.ModAPI.Ingame.IMyBlockGroup, bool> collect)
      {
        this.m_terminalInstance.GetBlockGroups(blockGroups, collect);
      }

      void Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlocksOfType<T>(
        List<Sandbox.ModAPI.Ingame.IMyTerminalBlock> blocks,
        Func<Sandbox.ModAPI.Ingame.IMyTerminalBlock, bool> collect)
      {
        this.m_terminalInstance.GetBlocksOfType<T>(blocks, collect);
      }

      void Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlocksOfType<T>(
        List<T> blocks,
        Func<T, bool> collect)
      {
        this.m_terminalInstance.GetBlocksOfType<T>(blocks, collect);
      }

      void Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.SearchBlocksOfName(
        string name,
        List<Sandbox.ModAPI.Ingame.IMyTerminalBlock> blocks,
        Func<Sandbox.ModAPI.Ingame.IMyTerminalBlock, bool> collect)
      {
        this.m_terminalInstance.SearchBlocksOfName(name, blocks, collect);
      }

      Sandbox.ModAPI.Ingame.IMyTerminalBlock Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlockWithName(
        string name)
      {
        return this.m_terminalInstance.GetBlockWithName(name);
      }

      Sandbox.ModAPI.Ingame.IMyBlockGroup Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlockGroupWithName(
        string name)
      {
        return this.m_terminalInstance.GetBlockGroupWithName(name);
      }

      Sandbox.ModAPI.Ingame.IMyTerminalBlock Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlockWithId(
        long id)
      {
        return this.m_terminalInstance.GetBlockWithId(id);
      }
    }

    protected sealed class Recompile\u003C\u003ESystem_Boolean : ICallSite<MyProgrammableBlock, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProgrammableBlock @this,
        in bool instantiate,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.Recompile(instantiate);
      }
    }

    protected sealed class WriteProgramResponseForSubscribers\u003C\u003ESystem_String : ICallSite<MyProgrammableBlock, string, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProgrammableBlock @this,
        in string response,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.WriteProgramResponseForSubscribers(response);
      }
    }

    protected sealed class SubscribeToProgramResponse\u003C\u003ESystem_Boolean : ICallSite<MyProgrammableBlock, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProgrammableBlock @this,
        in bool register,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.SubscribeToProgramResponse(register);
      }
    }

    protected sealed class WriteProgramResponse\u003C\u003ESystem_String : ICallSite<MyProgrammableBlock, string, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProgrammableBlock @this,
        in string response,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.WriteProgramResponse(response);
      }
    }

    protected sealed class OpenEditorRequest\u003C\u003E : ICallSite<MyProgrammableBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProgrammableBlock @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OpenEditorRequest();
      }
    }

    protected sealed class OpenEditorSucess\u003C\u003E : ICallSite<MyProgrammableBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProgrammableBlock @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OpenEditorSucess();
      }
    }

    protected sealed class OpenEditorFailure\u003C\u003E : ICallSite<MyProgrammableBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProgrammableBlock @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OpenEditorFailure();
      }
    }

    protected sealed class CloseEditor\u003C\u003E : ICallSite<MyProgrammableBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProgrammableBlock @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.CloseEditor();
      }
    }

    protected sealed class UpdateProgram\u003C\u003ESystem_Byte\u003C\u0023\u003E : ICallSite<MyProgrammableBlock, byte[], DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProgrammableBlock @this,
        in byte[] program,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.UpdateProgram(program);
      }
    }

    protected sealed class RunProgramRequest\u003C\u003ESystem_Byte\u003C\u0023\u003E\u0023Sandbox_ModAPI_Ingame_UpdateType : ICallSite<MyProgrammableBlock, byte[], UpdateType, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProgrammableBlock @this,
        in byte[] argument,
        in UpdateType updateType,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.RunProgramRequest(argument, updateType);
      }
    }

    protected sealed class OnRemoveSelectedImageRequest\u003C\u003ESystem_Int32\u0023System_Int32\u003C\u0023\u003E : ICallSite<MyProgrammableBlock, int, int[], DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProgrammableBlock @this,
        in int panelIndex,
        in int[] selection,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnRemoveSelectedImageRequest(panelIndex, selection);
      }
    }

    protected sealed class OnSelectImageRequest\u003C\u003ESystem_Int32\u0023System_Int32\u003C\u0023\u003E : ICallSite<MyProgrammableBlock, int, int[], DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProgrammableBlock @this,
        in int panelIndex,
        in int[] selection,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnSelectImageRequest(panelIndex, selection);
      }
    }

    protected sealed class OnChangeTextRequest\u003C\u003ESystem_Int32\u0023System_String : ICallSite<MyProgrammableBlock, int, string, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProgrammableBlock @this,
        in int panelIndex,
        in string text,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeTextRequest(panelIndex, text);
      }
    }

    protected sealed class OnUpdateSpriteCollection\u003C\u003ESystem_Int32\u0023VRage_Game_GUI_TextPanel_MySerializableSpriteCollection : ICallSite<MyProgrammableBlock, int, MySerializableSpriteCollection, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProgrammableBlock @this,
        in int panelIndex,
        in MySerializableSpriteCollection sprites,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnUpdateSpriteCollection(panelIndex, sprites);
      }
    }

    protected sealed class OnChangeOpenRequest\u003C\u003ESystem_Boolean\u0023System_Boolean\u0023System_UInt64\u0023System_Boolean : ICallSite<MyProgrammableBlock, bool, bool, ulong, bool, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProgrammableBlock @this,
        in bool isOpen,
        in bool editable,
        in ulong user,
        in bool isPublic,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeOpenRequest(isOpen, editable, user, isPublic);
      }
    }

    protected sealed class OnChangeOpenSuccess\u003C\u003ESystem_Boolean\u0023System_Boolean\u0023System_UInt64\u0023System_Boolean : ICallSite<MyProgrammableBlock, bool, bool, ulong, bool, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProgrammableBlock @this,
        in bool isOpen,
        in bool editable,
        in ulong user,
        in bool isPublic,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeOpenSuccess(isOpen, editable, user, isPublic);
      }
    }

    protected sealed class OnChangeDescription\u003C\u003ESystem_String\u0023System_Boolean : ICallSite<MyProgrammableBlock, string, bool, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProgrammableBlock @this,
        in string description,
        in bool isPublic,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeDescription(description, isPublic);
      }
    }

    private class Sandbox_Game_Entities_Blocks_MyProgrammableBlock\u003C\u003EActor : IActivator, IActivator<MyProgrammableBlock>
    {
      object IActivator.CreateInstance() => (object) new MyProgrammableBlock();

      MyProgrammableBlock IActivator<MyProgrammableBlock>.CreateInstance() => new MyProgrammableBlock();
    }
  }
}
