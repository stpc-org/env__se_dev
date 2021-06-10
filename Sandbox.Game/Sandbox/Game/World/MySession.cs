// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MySession
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface;
using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Engine;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.ContextHandling;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.Weapons;
using Sandbox.Game.World.Generator;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Definitions;
using VRage.Game.Entity;
using VRage.Game.Factions.Definitions;
using VRage.Game.GUI;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders;
using VRage.Game.SessionComponents;
using VRage.Game.Voxels;
using VRage.GameServices;
using VRage.Input;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Plugins;
using VRage.Profiler;
using VRage.Render.Particles;
using VRage.Scripting;
using VRage.Serialization;
using VRage.UserInterface;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.World
{
  [StaticEventOwner]
  public sealed class MySession : IMyNetObject, IMyEventOwner, IMySession
  {
    private static readonly MySession.ComponentComparer SessionComparer = new MySession.ComponentComparer();
    private readonly CachingDictionary<Type, MySessionComponentBase> m_sessionComponents = new CachingDictionary<Type, MySessionComponentBase>();
    private readonly Dictionary<int, SortedSet<MySessionComponentBase>> m_sessionComponentsForUpdate = new Dictionary<int, SortedSet<MySessionComponentBase>>();
    private readonly List<MySessionComponentBase> m_sessionComponentForDraw = new List<MySessionComponentBase>();
    private readonly List<MySessionComponentBase> m_sessionComponentForDrawAsync = new List<MySessionComponentBase>();
    private HashSet<string> m_componentsToLoad;
    public HashSet<string> SessionComponentEnabled = new HashSet<string>();
    public HashSet<string> SessionComponentDisabled = new HashSet<string>();
    private const string SAVING_FOLDER = ".new";
    public const int MIN_NAME_LENGTH = 5;
    public const int MAX_NAME_LENGTH = 90;
    public const int MAX_DESCRIPTION_LENGTH = 7999;
    internal MySpectatorCameraController Spectator = new MySpectatorCameraController();
    internal MyTimeSpan m_timeOfSave;
    internal DateTime m_lastTimeMemoryLogged;
    private Dictionary<string, short> EmptyBlockTypeLimitDictionary = new Dictionary<string, short>();
    private static MySession m_static;
    public int RequiresDX = 9;
    private bool m_isSaveInProgress;
    private bool m_isSnapshotSaveInProgress;
    public MyObjectBuilder_SessionSettings Settings;
    private bool? m_saveOnUnloadOverride;
    private bool? m_isRunningExperimentalCache;
    public MyScriptManager ScriptManager;
    public List<Tuple<string, MyBlueprintItemInfo>> BattleBlueprints;
    public Dictionary<ulong, MyPromoteLevel> PromotedUsers = new Dictionary<ulong, MyPromoteLevel>();
    public MyScenarioDefinition Scenario;
    public BoundingBoxD? WorldBoundaries;
    public readonly MyVoxelMaps VoxelMaps = new MyVoxelMaps();
    public readonly MyFactionCollection Factions = new MyFactionCollection();
    public MyPlayerCollection Players = new MyPlayerCollection();
    public MyPerPlayerData PerPlayerData = new MyPerPlayerData();
    public readonly MyToolBarCollection Toolbars = new MyToolBarCollection();
    internal MyVirtualClients VirtualClients = new MyVirtualClients();
    internal MyCameraCollection Cameras = new MyCameraCollection();
    public MyGpsCollection Gpss = new MyGpsCollection();
    public MyBlockLimits NPCBlockLimits;
    public MyBlockLimits GlobalBlockLimits;
    public MyBlockLimits SessionBlockLimits;
    public int TotalSessionPCU;
    public bool ServerSaving;
    private AdminSettingsEnum m_adminSettings;
    private Dictionary<ulong, AdminSettingsEnum> m_remoteAdminSettings = new Dictionary<ulong, AdminSettingsEnum>();
    private bool m_streamingInProgress;
    private List<MyUpdateCallback> m_updateCallbacks = new List<MyUpdateCallback>();
    private static bool m_showMotD = false;
    public Dictionary<string, MyFixedPoint> AmountMined = new Dictionary<string, MyFixedPoint>();
    private bool m_cameraAwaitingEntity;
    private IMyCameraController m_cameraController = (IMyCameraController) MySpectatorCameraController.Static;
    public ulong WorldSizeInBytes;
    private int m_gameplayFrameCounter;
    private const int FRAMES_TO_CONSIDER_READY = 10;
    private int m_framesToReady;
    public HashSet<ulong> CreativeTools = new HashSet<ulong>();
    private bool m_updateAllowed;
    private MyHudNotification m_aliveNotification;
    private List<MySessionComponentBase> m_loadOrder = new List<MySessionComponentBase>();
    private static int m_profilerDumpDelay;
    private int m_currentDumpNumber;
    private MyObjectBuilder_SessionSettings _settings;
    private DateTime m_streamingIndicatorShowTime = DateTime.MaxValue;
    private MyMultiplayerHostResult m_serverRequest;
    public const float ADAPTIVE_LOAD_THRESHOLD = 90f;
    private int m_simQualitySwitchFrames;
    private int m_lastQualitySwitchFrame;
    private const int ConsecutiveFramesToShowWarning = 300;
    private MyOxygenProviderSystemHelper m_oxygenHelper = new MyOxygenProviderSystemHelper();

    private void PrepareBaseSession(
      List<MyObjectBuilder_Checkpoint.ModItem> mods,
      MyScenarioDefinition definition = null)
    {
      MyGeneralStats.Static.LoadData();
      this.ScriptManager.Init((MyObjectBuilder_ScriptManager) null);
      MyDefinitionManager.Static.LoadData(mods);
      this.LoadGameDefinition(new MyDefinitionId?(definition != null ? definition.GameDefinition : MyGameDefinition.Default));
      this.Scenario = definition;
      if (definition != null)
      {
        this.WorldBoundaries = definition.WorldBoundaries;
        MySector.EnvironmentDefinition = MyDefinitionManager.Static.GetDefinition<MyEnvironmentDefinition>(definition.Environment);
      }
      MySector.InitEnvironmentSettings();
      MyModAPIHelper.Initialize();
      this.LoadDataComponents();
      this.InitDataComponents();
      MyModAPIHelper.Initialize();
    }

    private void PrepareBaseSession(
      MyObjectBuilder_Checkpoint checkpoint,
      MyObjectBuilder_Sector sector)
    {
      MyGeneralStats.Static.LoadData();
      if (MyVRage.Platform.Scripting.IsRuntimeCompilationSupported)
        MyVRage.Platform.Scripting.CompileIngameScriptAsync("Dummy", string.Empty, out List<VRage.Scripting.Message> _, string.Empty, "Program", typeof (MyGridProgram).Name);
      MyGuiTextures.Static.Unload();
      this.ScriptManager.Init(checkpoint.ScriptManagerData);
      MyDefinitionManager.Static.LoadData(checkpoint.Mods);
      MySession.PreloadModels(sector);
      MyLocalCache.PreloadLocalInventoryConfig();
      if (MyFakes.PRIORITIZED_VICINITY_ASSETS_LOADING && !Sandbox.Engine.Platform.Game.IsDedicated)
      {
        MySession.PreloadVicinityCache(checkpoint.VicinityVoxelCache, checkpoint.VicinityModelsCache, checkpoint.VicinityArmorModelsCache);
        MyScreenManager.GetFirstScreenOfType<MyGuiScreenLoading>()?.DrawLoading();
      }
      this.VirtualClients.Init();
      this.LoadGameDefinition(checkpoint);
      if (!Sandbox.Engine.Platform.Game.IsDedicated)
        MyGuiManager.InitFonts();
      MyDefinitionManager.Static.TryGetDefinition<MyScenarioDefinition>((MyDefinitionId) checkpoint.Scenario, out this.Scenario);
      SerializableBoundingBoxD? worldBoundaries = checkpoint.WorldBoundaries;
      this.WorldBoundaries = worldBoundaries.HasValue ? new BoundingBoxD?((BoundingBoxD) worldBoundaries.GetValueOrDefault()) : new BoundingBoxD?();
      if (!this.WorldBoundaries.HasValue && this.Scenario != null)
        this.WorldBoundaries = this.Scenario.WorldBoundaries;
      MySector.InitEnvironmentSettings(sector.Environment);
      if (MyFakes.PRIORITIZED_VICINITY_ASSETS_LOADING && !Sandbox.Engine.Platform.Game.IsDedicated)
        MyRenderProxy.PreloadTextures((IEnumerable<string>) new string[1]
        {
          !string.IsNullOrEmpty(this.CustomSkybox) ? this.CustomSkybox : MySector.EnvironmentDefinition.EnvironmentTexture
        }, TextureType.CubeMap);
      MyModAPIHelper.Initialize();
      this.LoadDataComponents();
      this.LoadObjectBuildersComponents(checkpoint.SessionComponents);
      MyModAPIHelper.Initialize();
      if (!Sync.IsDedicated || MySessionComponentAnimationSystem.Static == null)
        return;
      MySessionComponentAnimationSystem.Static.SetUpdateOrder(MyUpdateOrder.NoUpdate);
    }

    private void RegisterComponentsFromAssemblies()
    {
      Assembly executingAssembly = Assembly.GetExecutingAssembly();
      AssemblyName[] array = ((IEnumerable<AssemblyName>) executingAssembly.GetReferencedAssemblies()).GroupBy<AssemblyName, string>((Func<AssemblyName, string>) (x => x.Name)).Select<IGrouping<string, AssemblyName>, AssemblyName>((Func<IGrouping<string, AssemblyName>, AssemblyName>) (y => y.First<AssemblyName>())).ToArray<AssemblyName>();
      this.m_componentsToLoad = new HashSet<string>();
      this.m_componentsToLoad.UnionWith((IEnumerable<string>) this.GameDefinition.SessionComponents.Keys);
      this.m_componentsToLoad.RemoveWhere((Predicate<string>) (x => this.SessionComponentDisabled.Contains(x)));
      this.m_componentsToLoad.UnionWith((IEnumerable<string>) this.SessionComponentEnabled);
      foreach (AssemblyName assemblyRef in array)
      {
        try
        {
          if (!assemblyRef.Name.Contains("Sandbox"))
          {
            if (!assemblyRef.Name.Equals("VRage.Game"))
              continue;
          }
          Assembly assembly = Assembly.Load(assemblyRef);
          object[] customAttributes = assembly.GetCustomAttributes(typeof (AssemblyProductAttribute), false);
          if (customAttributes.Length != 0)
          {
            AssemblyProductAttribute productAttribute = customAttributes[0] as AssemblyProductAttribute;
            if (!(productAttribute.Product == "Sandbox"))
            {
              if (!(productAttribute.Product == "VRage.Game"))
                continue;
            }
            this.RegisterComponentsFromAssembly(assembly);
          }
        }
        catch (Exception ex)
        {
          MyLog.Default.WriteLine("Error while resolving session components assemblies");
          MyLog.Default.WriteLine(ex.ToString());
        }
      }
      try
      {
        foreach (KeyValuePair<MyModContext, HashSet<MyStringId>> keyValuePair in this.ScriptManager.ScriptsPerMod)
          this.RegisterComponentsFromAssembly(this.ScriptManager.Scripts[keyValuePair.Value.First<MyStringId>()], true, keyValuePair.Key);
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine("Error while loading modded session components");
        MyLog.Default.WriteLine(ex.ToString());
      }
      try
      {
        foreach (object plugin in MyPlugins.Plugins)
          this.RegisterComponentsFromAssembly(plugin.GetType().Assembly, true);
      }
      catch (Exception ex)
      {
      }
      try
      {
        this.RegisterComponentsFromAssembly(MyPlugins.GameAssembly);
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine("Error while resolving session components MOD assemblies");
        MyLog.Default.WriteLine(ex.ToString());
      }
      try
      {
        this.RegisterComponentsFromAssembly(MyPlugins.UserAssemblies);
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine("Error while resolving session components MOD assemblies");
        MyLog.Default.WriteLine(ex.ToString());
      }
      this.RegisterComponentsFromAssembly(executingAssembly);
      foreach (MySessionComponentBase sessionComponentBase in this.m_sessionComponents.Values)
      {
        if (sessionComponentBase.ModContext == null || sessionComponentBase.ModContext.IsBaseGame)
          this.m_sessionComponentForDrawAsync.Add(sessionComponentBase);
        else
          this.m_sessionComponentForDraw.Add(sessionComponentBase);
      }
    }

    public T GetComponent<T>() where T : MySessionComponentBase
    {
      MySessionComponentBase sessionComponentBase;
      this.m_sessionComponents.TryGetValue(typeof (T), out sessionComponentBase);
      return sessionComponentBase as T;
    }

    public void RegisterComponent(
      MySessionComponentBase component,
      MyUpdateOrder updateOrder,
      int priority)
    {
      this.m_sessionComponents[component.ComponentType] = component;
      component.Session = (IMySession) this;
      this.AddComponentForUpdate(updateOrder, component);
      this.m_sessionComponents.ApplyChanges();
    }

    public void UnregisterComponent(MySessionComponentBase component)
    {
      component.Session = (IMySession) null;
      this.m_sessionComponents.Remove(component.ComponentType);
    }

    public void RegisterComponentsFromAssembly(
      Assembly[] assemblies,
      bool modAssembly = false,
      MyModContext context = null)
    {
      if (assemblies == null)
        return;
      foreach (Assembly assembly in assemblies)
        this.RegisterComponentsFromAssembly(assembly, modAssembly, context);
    }

    public void RegisterComponentsFromAssembly(
      Assembly assembly,
      bool modAssembly = false,
      MyModContext context = null)
    {
      if (assembly == (Assembly) null)
        return;
      MySandboxGame.Log.WriteLine("Registered modules from: " + assembly.FullName);
      foreach (Type type in assembly.GetTypes())
      {
        if (Attribute.IsDefined((MemberInfo) type, typeof (MySessionComponentDescriptor)))
          this.TryRegisterSessionComponent(type, modAssembly, context);
      }
    }

    private void TryRegisterSessionComponent(Type type, bool modAssembly, MyModContext context)
    {
      try
      {
        MyDefinitionId? definition = new MyDefinitionId?();
        MySessionComponentBase instance = (MySessionComponentBase) Activator.CreateInstance(type);
        if (!(instance.IsRequiredByGame | modAssembly) && !this.GetComponentInfo(type, out definition))
          return;
        this.RegisterComponent(instance, instance.UpdateOrder, instance.Priority);
        this.GetComponentInfo(type, out definition);
        instance.Definition = definition;
        instance.ModContext = (IMyModContext) context;
      }
      catch (Exception ex)
      {
        MySandboxGame.Log.WriteLine("Exception during loading of type : " + type.Name);
      }
    }

    private bool GetComponentInfo(Type type, out MyDefinitionId? definition)
    {
      string key = (string) null;
      if (this.m_componentsToLoad.Contains(type.Name))
        key = type.Name;
      else if (this.m_componentsToLoad.Contains(type.FullName))
        key = type.FullName;
      if (key != null)
      {
        this.GameDefinition.SessionComponents.TryGetValue(key, out definition);
        return true;
      }
      definition = new MyDefinitionId?();
      return false;
    }

    public void AddComponentForUpdate(MyUpdateOrder updateOrder, MySessionComponentBase component)
    {
      for (int index = 0; index <= 2; ++index)
      {
        if ((updateOrder & (MyUpdateOrder) (1 << index)) != MyUpdateOrder.NoUpdate)
        {
          SortedSet<MySessionComponentBase> sortedSet = (SortedSet<MySessionComponentBase>) null;
          if (!this.m_sessionComponentsForUpdate.TryGetValue(1 << index, out sortedSet))
            this.m_sessionComponentsForUpdate.Add(1 << index, sortedSet = new SortedSet<MySessionComponentBase>((IComparer<MySessionComponentBase>) MySession.SessionComparer));
          sortedSet.Add(component);
        }
      }
    }

    public void SetComponentUpdateOrder(MySessionComponentBase component, MyUpdateOrder order)
    {
      for (int key = 0; key <= 2; ++key)
      {
        SortedSet<MySessionComponentBase> sortedSet = (SortedSet<MySessionComponentBase>) null;
        if ((order & (MyUpdateOrder) (1 << key)) != MyUpdateOrder.NoUpdate)
        {
          if (!this.m_sessionComponentsForUpdate.TryGetValue(1 << key, out sortedSet))
          {
            sortedSet = new SortedSet<MySessionComponentBase>();
            this.m_sessionComponentsForUpdate.Add(key, sortedSet);
          }
          sortedSet.Add(component);
        }
        else if (this.m_sessionComponentsForUpdate.TryGetValue(1 << key, out sortedSet))
          sortedSet.Remove(component);
      }
    }

    public void LoadObjectBuildersComponents(
      List<MyObjectBuilder_SessionComponent> objectBuilderData)
    {
      foreach (MyObjectBuilder_SessionComponent sessionComponent in objectBuilderData)
      {
        Type sessionComponentType;
        MySessionComponentBase sessionComponentBase;
        if ((sessionComponentType = MySessionComponentMapping.TryGetMappedSessionComponentType((MyObjectBuilderType) sessionComponent.GetType())) != (Type) null && this.m_sessionComponents.TryGetValue(sessionComponentType, out sessionComponentBase))
          sessionComponentBase.Init(sessionComponent);
      }
      this.InitDataComponents();
    }

    private void InitDataComponents()
    {
      foreach (MySessionComponentBase sessionComponentBase in this.m_sessionComponents.Values)
      {
        if (!sessionComponentBase.Initialized)
        {
          MyObjectBuilder_SessionComponent sessionComponent = (MyObjectBuilder_SessionComponent) null;
          if (sessionComponentBase.ObjectBuilderType != MyObjectBuilderType.Invalid)
            sessionComponent = (MyObjectBuilder_SessionComponent) Activator.CreateInstance((Type) sessionComponentBase.ObjectBuilderType);
          sessionComponentBase.Init(sessionComponent);
        }
      }
    }

    public void LoadDataComponents()
    {
      MyTimeOfDayHelper.Reset();
      this.RaiseOnLoading();
      Sync.Clients.SetLocalSteamId(Sync.MyId, !(Sandbox.Engine.Multiplayer.MyMultiplayer.Static is MyMultiplayerClient), MyGameService.UserName);
      Sync.Players.RegisterEvents();
      this.SetAsNotReady();
      HashSet<MySessionComponentBase> sessionComponentBaseSet = new HashSet<MySessionComponentBase>();
      do
      {
        this.m_sessionComponents.ApplyChanges();
        foreach (MySessionComponentBase component in this.m_sessionComponents.Values)
        {
          if (!sessionComponentBaseSet.Contains(component))
          {
            this.LoadComponent(component);
            sessionComponentBaseSet.Add(component);
          }
        }
      }
      while (this.m_sessionComponents.HasChanges());
    }

    private void LoadComponent(MySessionComponentBase component)
    {
      if (component.Loaded)
        return;
      foreach (Type dependency in component.Dependencies)
      {
        MySessionComponentBase component1;
        this.m_sessionComponents.TryGetValue(dependency, out component1);
        if (component1 != null)
          this.LoadComponent(component1);
      }
      if (!this.m_loadOrder.Contains(component))
      {
        this.m_loadOrder.Add(component);
        component.LoadData();
        component.AfterLoadData();
      }
      else
      {
        string str = string.Format("Circular dependency: {0}", (object) component.DebugName);
        MySandboxGame.Log.WriteLine(str);
        throw new Exception(str);
      }
    }

    public void UnloadDataComponents(bool beforeLoadWorld = false)
    {
      MySessionComponentBase sessionComponentBase = (MySessionComponentBase) null;
      try
      {
        for (int index = this.m_loadOrder.Count - 1; index >= 0; --index)
        {
          sessionComponentBase = this.m_loadOrder[index];
          sessionComponentBase.UnloadDataConditional();
        }
      }
      catch (Exception ex)
      {
        IMyModContext modContext = sessionComponentBase.ModContext;
        if (modContext != null && !modContext.IsBaseGame)
          throw new ModCrashedException(ex, modContext);
        throw;
      }
      MySessionComponentMapping.Clear();
      this.m_sessionComponents.Clear();
      this.m_loadOrder.Clear();
      foreach (SortedSet<MySessionComponentBase> sortedSet in this.m_sessionComponentsForUpdate.Values)
        sortedSet.Clear();
      this.m_sessionComponentsForUpdate.Clear();
      this.m_sessionComponentForDraw.Clear();
      this.m_sessionComponentForDrawAsync.Clear();
      if (!beforeLoadWorld)
      {
        Sync.Players.UnregisterEvents();
        Sync.Clients.Clear();
        MyNetworkReader.Clear();
      }
      this.Ready = false;
    }

    public void BeforeStartComponents()
    {
      this.TotalDamageDealt = 0U;
      this.TotalBlocksCreated = 0U;
      this.ToolbarPageSwitches = 0U;
      this.ElapsedPlayTime = new TimeSpan();
      this.m_timeOfSave = MySandboxGame.Static.TotalTime;
      MyFpsManager.Reset();
      foreach (MySessionComponentBase sessionComponentBase in this.m_sessionComponents.Values)
        sessionComponentBase.BeforeStart();
      if (MySpaceAnalytics.Instance == null)
        return;
      if (Sandbox.Engine.Platform.Game.IsDedicated)
        MySpaceAnalytics.Instance.StartSessionAndIdentifyPlayer(Guid.NewGuid().ToString(), true);
      MySpaceAnalytics.Instance.ReportWorldStart(this.Settings);
    }

    public void UpdateComponents()
    {
      SortedSet<MySessionComponentBase> sortedSet = (SortedSet<MySessionComponentBase>) null;
      if (this.m_sessionComponentsForUpdate.TryGetValue(1, out sortedSet))
      {
        foreach (MySessionComponentBase sessionComponentBase in sortedSet)
        {
          if (sessionComponentBase.UpdatedBeforeInit() || MySandboxGame.IsGameReady)
            sessionComponentBase.UpdateBeforeSimulation();
        }
      }
      if (Sandbox.Engine.Multiplayer.MyMultiplayer.Static != null)
        Sandbox.Engine.Multiplayer.MyMultiplayer.Static.ReplicationLayer.Simulate();
      if (this.m_sessionComponentsForUpdate.TryGetValue(2, out sortedSet))
      {
        foreach (MySessionComponentBase sessionComponentBase in sortedSet)
        {
          if (sessionComponentBase.UpdatedBeforeInit() || MySandboxGame.IsGameReady)
            sessionComponentBase.Simulate();
        }
      }
      if (!this.m_sessionComponentsForUpdate.TryGetValue(4, out sortedSet))
        return;
      foreach (MySessionComponentBase sessionComponentBase in sortedSet)
      {
        if (sessionComponentBase.UpdatedBeforeInit() || MySandboxGame.IsGameReady)
          sessionComponentBase.UpdateAfterSimulation();
      }
    }

    public void UpdateComponentsWhilePaused()
    {
      SortedSet<MySessionComponentBase> sortedSet = (SortedSet<MySessionComponentBase>) null;
      if (this.m_sessionComponentsForUpdate.TryGetValue(1, out sortedSet))
      {
        foreach (MySessionComponentBase sessionComponentBase in sortedSet)
        {
          if (sessionComponentBase.UpdateOnPause)
            sessionComponentBase.UpdateBeforeSimulation();
        }
      }
      if (this.m_sessionComponentsForUpdate.TryGetValue(2, out sortedSet))
      {
        foreach (MySessionComponentBase sessionComponentBase in sortedSet)
        {
          if (sessionComponentBase.UpdateOnPause)
            sessionComponentBase.Simulate();
        }
      }
      if (!this.m_sessionComponentsForUpdate.TryGetValue(4, out sortedSet))
        return;
      foreach (MySessionComponentBase sessionComponentBase in sortedSet)
      {
        if (sessionComponentBase.UpdateOnPause)
          sessionComponentBase.UpdateAfterSimulation();
      }
    }

    public static string GameServiceName => MyGameService.Service == null ? "" : MyGameService.Service.ServiceName;

    public static float GetPlayerDistance(MyEntity entity, ICollection<MyPlayer> players)
    {
      Vector3D translation = entity.WorldMatrix.Translation;
      float num1 = float.MaxValue;
      foreach (MyPlayer player in (IEnumerable<MyPlayer>) players)
      {
        Sandbox.Game.Entities.IMyControllableEntity controlledEntity = player.Controller.ControlledEntity;
        if (controlledEntity != null)
        {
          float num2 = Vector3.DistanceSquared((Vector3) controlledEntity.Entity.WorldMatrix.Translation, (Vector3) translation);
          if ((double) num2 < (double) num1)
            num1 = num2;
        }
      }
      return (float) Math.Sqrt((double) num1);
    }

    public static float GetOwnerLoginTimeSeconds(MyCubeGrid grid) => grid == null || grid.BigOwners.Count == 0 ? 0.0f : MySession.GetIdentityLoginTimeSeconds(grid.BigOwners[0]);

    public static float GetIdentityLoginTimeSeconds(long identityId)
    {
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(identityId);
      return identity == null ? 0.0f : (float) (int) (DateTime.Now - identity.LastLoginTime).TotalSeconds;
    }

    public static float GetOwnerLogoutTimeSeconds(MyCubeGrid grid)
    {
      if (grid == null || grid.BigOwners.Count == 0)
        return 0.0f;
      if (grid.BigOwners.Count == 1)
        return MySession.GetIdentityLogoutTimeSeconds(grid.BigOwners[0]);
      float num = float.MaxValue;
      foreach (long bigOwner in grid.BigOwners)
      {
        float logoutTimeSeconds = MySession.GetIdentityLogoutTimeSeconds(bigOwner);
        if ((double) logoutTimeSeconds < (double) num)
          num = logoutTimeSeconds;
      }
      return num;
    }

    public static float GetIdentityLogoutTimeSeconds(long identityId)
    {
      MyPlayer.PlayerId result;
      if (MySession.Static.Players.TryGetPlayerId(identityId, out result) && MySession.Static.Players.GetPlayerById(result) != null)
        return 0.0f;
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(identityId);
      return identity == null || MySession.Static.Players.IdentityIsNpc(identityId) ? 0.0f : (float) (int) (DateTime.Now - identity.LastLogoutTime).TotalSeconds;
    }

    public static MySession Static
    {
      get => MySession.m_static;
      set
      {
        MySession.m_static = value;
        MyVRage.Platform.SessionReady = value != null;
      }
    }

    public DateTime GameDateTime
    {
      get => new DateTime(2081, 1, 1, 0, 0, 0, DateTimeKind.Utc) + this.ElapsedGameTime;
      set => this.ElapsedGameTime = value - new DateTime(2081, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    }

    public TimeSpan ElapsedGameTime { get; set; }

    public DateTime InGameTime { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Password { get; set; }

    public ulong? WorkshopId { get; private set; }

    public string CurrentPath { get; set; }

    public float SessionSimSpeedPlayer { get; private set; }

    public float SessionSimSpeedServer { get; private set; }

    public bool CameraOnCharacter { get; set; }

    public uint AutoSaveInMinutes => MyFakes.ENABLE_AUTOSAVE && this.Settings != null ? this.Settings.AutoSaveInMinutes : 0U;

    public bool? SaveOnUnloadOverride => this.m_saveOnUnloadOverride;

    public bool SetSaveOnUnloadOverride_Dedicated(bool? save)
    {
      if (!Sandbox.Engine.Platform.Game.IsDedicated)
        return false;
      this.m_saveOnUnloadOverride = save;
      return true;
    }

    public bool IsAdminMenuEnabled => this.IsUserModerator(Sync.MyId);

    public bool CreativeMode => this.Settings.GameMode == MyGameModeEnum.Creative;

    public bool SurvivalMode => this.Settings.GameMode == MyGameModeEnum.Survival;

    public bool InfiniteAmmo => this.Settings.InfiniteAmmo || this.Settings.GameMode == MyGameModeEnum.Creative;

    public bool EnableContainerDrops => this.Settings.EnableContainerDrops && this.Settings.GameMode == MyGameModeEnum.Survival;

    public int MinDropContainerRespawnTime => this.Settings.MinDropContainerRespawnTime * 60;

    public int MaxDropContainerRespawnTime => this.Settings.MaxDropContainerRespawnTime * 60;

    public bool AutoHealing => this.Settings.AutoHealing;

    public bool ThrusterDamage => this.Settings.ThrusterDamage;

    public bool WeaponsEnabled => this.Settings.WeaponsEnabled;

    public bool CargoShipsEnabled => this.Settings.CargoShipsEnabled;

    public bool DestructibleBlocks => this.Settings.DestructibleBlocks;

    public bool EnableIngameScripts => this.Settings.EnableIngameScripts;

    public bool Enable3RdPersonView => this.Settings.Enable3rdPersonView;

    public bool EnableToolShake => this.Settings.EnableToolShake;

    public bool ShowPlayerNamesOnHud => this.Settings.ShowPlayerNamesOnHud;

    public bool EnableConvertToStation => this.Settings.EnableConvertToStation;

    public short MaxPlayers => this.Settings.MaxPlayers;

    public short MaxFloatingObjects => this.Settings.MaxFloatingObjects;

    public short MaxBackupSaves => this.Settings.MaxBackupSaves;

    public int MaxGridSize => this.Settings.MaxGridSize;

    public int MaxBlocksPerPlayer => this.Settings.MaxBlocksPerPlayer;

    public Dictionary<string, short> BlockTypeLimits => this.Settings.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.NONE ? this.EmptyBlockTypeLimitDictionary : this.Settings.BlockTypeLimits.Dictionary;

    public bool EnableRemoteBlockRemoval => this.Settings.EnableRemoteBlockRemoval;

    public float InventoryMultiplier => this.Settings.InventorySizeMultiplier;

    public float CharactersInventoryMultiplier => this.Settings.InventorySizeMultiplier;

    public float BlocksInventorySizeMultiplier => this.Settings.BlocksInventorySizeMultiplier;

    public bool SimplifiedSimulation => MyPlatformGameSettings.SIMPLIFIED_SIMULATION_OVERRIDE ?? this.Settings.SimplifiedSimulation;

    public float RefinerySpeedMultiplier => this.Settings.RefinerySpeedMultiplier;

    public float AssemblerSpeedMultiplier => this.Settings.AssemblerSpeedMultiplier;

    public float AssemblerEfficiencyMultiplier => this.Settings.AssemblerEfficiencyMultiplier;

    public float WelderSpeedMultiplier => this.Settings.WelderSpeedMultiplier;

    public float GrinderSpeedMultiplier => this.Settings.GrinderSpeedMultiplier;

    public float HackSpeedMultiplier => this.Settings.HackSpeedMultiplier;

    public MyOnlineModeEnum OnlineMode => this.Settings.OnlineMode;

    public MyEnvironmentHostilityEnum EnvironmentHostility => this.Settings.EnvironmentHostility;

    public bool StartInRespawnScreen => this.Settings.StartInRespawnScreen;

    public bool EnableVoxelDestruction => this.Settings.EnableVoxelDestruction;

    public MyBlockLimitsEnabledEnum BlockLimitsEnabled => this.Settings.BlockLimitsEnabled;

    public int TotalPCU => this.Settings.TotalPCU;

    public int PiratePCU => this.Settings.PiratePCU;

    public int MaxFactionsCount => this.BlockLimitsEnabled != MyBlockLimitsEnabledEnum.PER_FACTION ? this.Settings.MaxFactionsCount : Math.Max(1, this.Settings.MaxFactionsCount);

    public bool ResearchEnabled => this.Settings.EnableResearch && !this.CreativeMode;

    public string CustomLoadingScreenImage { get; set; }

    public string CustomLoadingScreenText { get; set; }

    public string CustomSkybox { get; set; }

    public ulong SharedToolbar { get; set; }

    public bool IsRunningExperimental
    {
      get
      {
        if (!this.m_isRunningExperimentalCache.HasValue)
          this.m_isRunningExperimentalCache = !Sync.IsServer ? new bool?(Sandbox.Engine.Multiplayer.MyMultiplayer.Static.IsServerExperimental) : new bool?(MySandboxGame.Config.ExperimentalMode);
        return this.m_isRunningExperimentalCache.Value;
      }
    }

    public bool IsSettingsExperimental(bool remote = false) => this.GetSettingsExperimentalReason(remote, (MyObjectBuilder_Checkpoint) null) != (MyObjectBuilder_SessionSettings.ExperimentalReason) 0 && !MyCampaignManager.Static.IsCampaignRunning;

    public MyObjectBuilder_SessionSettings.ExperimentalReason GetSettingsExperimentalReason(
      bool remote,
      MyObjectBuilder_Checkpoint checkpoint)
    {
      MyObjectBuilder_SessionSettings.ExperimentalReason experimentalReason = this.Settings.GetExperimentalReason(remote);
      if (MySandboxGame.Config.ExperimentalMode && (Sandbox.Engine.Multiplayer.MyMultiplayer.Static == null || Sandbox.Engine.Multiplayer.MyMultiplayer.Static.IsServerExperimental))
        experimentalReason |= MyObjectBuilder_SessionSettings.ExperimentalReason.ExperimentalTurnedOnInConfiguration;
      if (MySandboxGame.InsufficientHardware)
        experimentalReason |= MyObjectBuilder_SessionSettings.ExperimentalReason.InsufficientHardware;
      if (this.Mods.Count > 0)
        experimentalReason |= MyObjectBuilder_SessionSettings.ExperimentalReason.Mods;
      if (MySandboxGame.ConfigDedicated != null && MySandboxGame.ConfigDedicated.Plugins != null && MySandboxGame.ConfigDedicated.Plugins.Count != 0)
        experimentalReason |= MyObjectBuilder_SessionSettings.ExperimentalReason.Plugins;
      bool flag = false;
      if (checkpoint != null)
      {
        MyObjectBuilder_CampaignSessionComponent ob = checkpoint.SessionComponents.OfType<MyObjectBuilder_CampaignSessionComponent>().FirstOrDefault<MyObjectBuilder_CampaignSessionComponent>();
        if (ob != null)
          flag = ((flag ? 1 : 0) | (MyCampaignManager.Static == null ? 0 : (MyCampaignManager.Static.IsCampaign(ob) ? 1 : 0))) != 0;
      }
      if (flag)
        experimentalReason = (MyObjectBuilder_SessionSettings.ExperimentalReason) 0;
      return experimentalReason;
    }

    public bool EnableSpiders => this.Settings.EnableSpiders;

    public bool EnableWolfs => this.Settings.EnableWolfs;

    public bool EnableScripterRole => this.Settings.EnableScripterRole;

    public bool IsScenario => this.Settings.Scenario;

    public bool LoadedAsMission { get; private set; }

    public bool PersistentEditMode { get; private set; }

    public List<MyObjectBuilder_Checkpoint.ModItem> Mods { get; set; }

    BoundingBoxD IMySession.WorldBoundaries => !this.WorldBoundaries.HasValue ? BoundingBoxD.CreateInvalid() : this.WorldBoundaries.Value;

    public MySyncLayer SyncLayer { get; private set; }

    public MyChatSystem ChatSystem => this.GetComponent<MyChatSystem>();

    public MyChatBot ChatBot => this.GetComponent<MyChatBot>();

    public event Action<ulong, MyPromoteLevel> OnUserPromoteLevelChanged;

    public static bool ShowMotD
    {
      get => MySession.m_showMotD;
      set => MySession.m_showMotD = value;
    }

    public TimeSpan ElapsedPlayTime { get; private set; }

    public TimeSpan TimeOnFoot { get; private set; }

    public TimeSpan TimeSprinting { get; private set; }

    public TimeSpan TimeOnJetpack { get; private set; }

    public TimeSpan TimePilotingSmallShip { get; private set; }

    public TimeSpan TimePilotingBigShip { get; private set; }

    public TimeSpan TimeOnStation { get; private set; }

    public TimeSpan TimeOnShips { get; private set; }

    public TimeSpan TimeOnAsteroids { get; private set; }

    public TimeSpan TimeOnPlanets { get; private set; }

    public TimeSpan TimeInBuilderMode { get; private set; }

    public TimeSpan TimeCreativeToolsEnabled { get; private set; }

    public TimeSpan TimeUsingMouseInput { get; private set; }

    public TimeSpan TimeUsingGamepadInput { get; private set; }

    public TimeSpan TimeInCameraGridFirstPerson { get; private set; }

    public TimeSpan TimeInCameraGridThirdPerson { get; private set; }

    public TimeSpan TimeInCameraCharFirstPerson { get; private set; }

    public TimeSpan TimeInCameraCharThirdPerson { get; private set; }

    public TimeSpan TimeInCameraToolFirstPerson { get; private set; }

    public TimeSpan TimeInCameraToolThirdPerson { get; private set; }

    public TimeSpan TimeInCameraWeaponFirstPerson { get; private set; }

    public TimeSpan TimeInCameraWeaponThirdPerson { get; private set; }

    public TimeSpan TimeInCameraBuildingFirstPerson { get; private set; }

    public TimeSpan TimeInCameraBuildingThirdPerson { get; private set; }

    public TimeSpan TimeGrindingBlocks { get; private set; }

    public TimeSpan TimeGrindingFriendlyBlocks { get; private set; }

    public TimeSpan TimeGrindingNeutralBlocks { get; private set; }

    public TimeSpan TimeGrindingEnemyBlocks { get; private set; }

    public float PositiveIntegrityTotal { get; set; }

    public float NegativeIntegrityTotal { get; set; }

    public ulong VoxelHandVolumeChanged { get; set; }

    public uint TotalDamageDealt { get; set; }

    public uint TotalBlocksCreated { get; set; }

    public uint TotalBlocksCreatedFromShips { get; set; }

    public uint ToolbarPageSwitches { get; set; }

    public IMyWeatherEffects WeatherEffects => (IMyWeatherEffects) MySession.Static.GetComponent<MySectorWeatherComponent>();

    public MyPlayer LocalHumanPlayer => Sync.Clients?.LocalClient?.FirstPlayer;

    public event Action OnLocalPlayerSkinOrColorChanged;

    IMyPlayer IMySession.LocalHumanPlayer => (IMyPlayer) this.LocalHumanPlayer;

    public MyEntity TopMostControlledEntity
    {
      get
      {
        MyEntity entity = this.ControlledEntity?.Entity;
        MyEntity topMostParent = entity?.GetTopMostParent((Type) null);
        return topMostParent == null || Sync.Players.GetControllingPlayer(entity) != Sync.Players.GetControllingPlayer(topMostParent) ? entity : topMostParent;
      }
    }

    public Sandbox.Game.Entities.IMyControllableEntity ControlledEntity => this.LocalHumanPlayer?.Controller.ControlledEntity;

    public MyCharacter LocalCharacter => this.LocalHumanPlayer?.Character;

    public IEnumerable<MyCharacter> SavedCharacters => this.LocalHumanPlayer?.SavedCharacters;

    public long LocalCharacterEntityId
    {
      get
      {
        MyCharacter localCharacter = this.LocalCharacter;
        return localCharacter == null ? 0L : __nonvirtual (localCharacter.EntityId);
      }
    }

    public long LocalPlayerId
    {
      get
      {
        MyPlayer localHumanPlayer = this.LocalHumanPlayer;
        return localHumanPlayer == null ? 0L : localHumanPlayer.Identity.IdentityId;
      }
    }

    public event Action<IMyCameraController, IMyCameraController> CameraAttachedToChanged;

    public bool IsCameraAwaitingEntity
    {
      get => this.m_cameraAwaitingEntity;
      set => this.m_cameraAwaitingEntity = value;
    }

    public IMyCameraController CameraController
    {
      get => this.m_cameraController;
      set
      {
        if (this.m_cameraController == value)
          return;
        IMyCameraController cameraController = this.m_cameraController;
        this.m_cameraController = value;
        if (MySession.Static == null)
          return;
        if (this.CameraAttachedToChanged != null)
          this.CameraAttachedToChanged(cameraController, this.m_cameraController);
        if (cameraController != null)
        {
          cameraController.OnReleaseControl(this.m_cameraController);
          if (cameraController.Entity != null)
            cameraController.Entity.OnClosing -= new Action<MyEntity>(this.OnCameraEntityClosing);
        }
        this.m_cameraController.OnAssumeControl(cameraController);
        if (this.m_cameraController.Entity != null)
          this.m_cameraController.Entity.OnClosing += new Action<MyEntity>(this.OnCameraEntityClosing);
        this.m_cameraController.ForceFirstPersonCamera = false;
      }
    }

    public bool IsValid => true;

    public int GameplayFrameCounter => this.m_gameplayFrameCounter;

    public bool Ready { get; private set; }

    public bool IsUnloading { get; private set; }

    public static event Action OnLoading;

    public static event Action OnUnloading;

    public static event Action AfterLoading;

    public static event Action BeforeLoading;

    public static event Action OnUnloaded;

    public event Action OnReady;

    public event Action<MyObjectBuilder_Checkpoint> OnSavingCheckpoint;

    public MyEnvironmentHostilityEnum? PreviousEnvironmentHostility { get; set; }

    public MyPromoteLevel GetUserPromoteLevel(ulong steamId)
    {
      if (MySession.Static.OnlineMode == MyOnlineModeEnum.OFFLINE || MySession.Static.OnlineMode != MyOnlineModeEnum.OFFLINE && (long) steamId == (long) Sync.ServerId)
        return MyPromoteLevel.Owner;
      MyPromoteLevel myPromoteLevel;
      MySession.Static.PromotedUsers.TryGetValue(steamId, out myPromoteLevel);
      return myPromoteLevel;
    }

    public bool IsUserScripter(ulong steamId) => !this.EnableScripterRole || this.GetUserPromoteLevel(steamId) >= MyPromoteLevel.Scripter;

    public bool IsUserModerator(ulong steamId) => this.GetUserPromoteLevel(steamId) >= MyPromoteLevel.Moderator;

    public bool IsUserSpaceMaster(ulong steamId) => this.GetUserPromoteLevel(steamId) >= MyPromoteLevel.SpaceMaster;

    public bool IsUserAdmin(ulong steamId) => this.GetUserPromoteLevel(steamId) >= MyPromoteLevel.Admin;

    public bool IsUserOwner(ulong steamId) => this.GetUserPromoteLevel(steamId) >= MyPromoteLevel.Owner;

    public bool HasCreativeRights => this.HasPlayerCreativeRights(Sync.MyId);

    public bool CreativeToolsEnabled(ulong user) => this.CreativeTools.Contains(user) && this.HasPlayerCreativeRights(user);

    public void EnableCreativeTools(ulong user, bool value)
    {
      if (value && this.HasCreativeRights)
        this.CreativeTools.Add(user);
      else
        this.CreativeTools.Remove(user);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<bool>((Func<IMyEventOwner, Action<bool>>) (s => new Action<bool>(MySession.OnCreativeToolsEnabled)), value);
    }

    [Event(null, 823)]
    [Reliable]
    [Server]
    private static void OnCreativeToolsEnabled(bool value)
    {
      ulong steamId = MyEventContext.Current.Sender.Value;
      if (value && MySession.Static.HasPlayerCreativeRights(steamId))
        MySession.Static.CreativeTools.Add(steamId);
      else
        MySession.Static.CreativeTools.Remove(steamId);
    }

    [Event(null, 837)]
    [Reliable]
    [Server]
    public static void OnCrash()
    {
    }

    public bool IsCopyPastingEnabled
    {
      get
      {
        if (this.CreativeToolsEnabled(Sync.MyId) && this.HasCreativeRights)
          return true;
        return this.CreativeMode && this.Settings.EnableCopyPaste;
      }
    }

    public bool IsCopyPastingEnabledForUser(ulong user)
    {
      if (this.CreativeToolsEnabled(user) && this.HasPlayerCreativeRights(user))
        return true;
      return this.CreativeMode && this.Settings.EnableCopyPaste;
    }

    public MyGameFocusManager GameFocusManager { get; private set; }

    public AdminSettingsEnum AdminSettings
    {
      get => this.m_adminSettings;
      set => this.m_adminSettings = value;
    }

    public Dictionary<ulong, AdminSettingsEnum> RemoteAdminSettings
    {
      get => this.m_remoteAdminSettings;
      set => this.m_remoteAdminSettings = value;
    }

    public bool StreamingInProgress
    {
      get => this.m_streamingInProgress;
      set
      {
        if (this.m_streamingInProgress == value)
          return;
        this.m_streamingInProgress = value;
        if (this.m_streamingInProgress)
        {
          MyHud.PushRotatingWheelVisible();
          MyHud.RotatingWheelText = MyTexts.Get(MySpaceTexts.LoadingWheel_Streaming);
        }
        else
        {
          MyHud.PopRotatingWheelVisible();
          MyHud.RotatingWheelText = MyHud.Empty;
        }
      }
    }

    public bool IsSaveInProgress => this.m_isSaveInProgress || this.m_isSnapshotSaveInProgress;

    public static void HitIndicatorActivation(MySession.MyHitIndicatorTarget type, ulong shooter)
    {
      if (shooter == 0UL || (long) Sync.MyId != (long) shooter && !Sync.Clients.HasClient(shooter))
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<MySession.MyHitIndicatorTarget>((Func<IMyEventOwner, Action<MySession.MyHitIndicatorTarget>>) (x => new Action<MySession.MyHitIndicatorTarget>(MySession.HitIndicatorActivationInternal)), type, new EndpointId(shooter));
    }

    [Event(null, 930)]
    [Reliable]
    [Client]
    public static void HitIndicatorActivationInternal(MySession.MyHitIndicatorTarget type) => MyScreenManager.GetFirstScreenOfType<MyGuiScreenHudSpace>()?.ActivateHitIndicator(type);

    public bool SetUserPromoteLevel(ulong steamId, MyPromoteLevel level)
    {
      if (level < MyPromoteLevel.None || level > MyPromoteLevel.Admin)
        throw new ArgumentOutOfRangeException(nameof (level), (object) level, (string) null);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<ulong, MyPromoteLevel>((Func<IMyEventOwner, Action<ulong, MyPromoteLevel>>) (x => new Action<ulong, MyPromoteLevel>(MySession.OnPromoteLevelSet)), steamId, level);
      return true;
    }

    [Event(null, 948)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    private static void OnPromoteLevelSet(ulong steamId, MyPromoteLevel level)
    {
      if (level == MyPromoteLevel.None)
        MySession.Static.PromotedUsers.Remove(steamId);
      else
        MySession.Static.PromotedUsers[steamId] = level;
      AdminSettingsEnum adminSettingsEnum;
      if (MySession.Static.RemoteAdminSettings.TryGetValue(steamId, out adminSettingsEnum))
      {
        if (!MySession.Static.IsUserAdmin(steamId))
        {
          MySession.Static.RemoteAdminSettings[steamId] = adminSettingsEnum & ~AdminSettingsEnum.AdminOnly;
          if ((long) steamId == (long) Sync.MyId)
            MySession.Static.AdminSettings = MySession.Static.RemoteAdminSettings[steamId];
        }
        else if (!MySession.Static.IsUserModerator(steamId))
        {
          MySession.Static.RemoteAdminSettings[steamId] = AdminSettingsEnum.None;
          if ((long) steamId == (long) Sync.MyId)
            MySession.Static.AdminSettings = MySession.Static.RemoteAdminSettings[steamId];
        }
      }
      if (MySession.Static.OnUserPromoteLevelChanged == null)
        return;
      MySession.Static.OnUserPromoteLevelChanged(steamId, level);
    }

    public bool CanPromoteUser(ulong requester, ulong target)
    {
      MyPromoteLevel userPromoteLevel1 = this.GetUserPromoteLevel(requester);
      MyPromoteLevel userPromoteLevel2 = this.GetUserPromoteLevel(target);
      return userPromoteLevel2 < MyPromoteLevel.Admin && userPromoteLevel1 >= userPromoteLevel2 && userPromoteLevel1 >= MyPromoteLevel.Admin;
    }

    public bool CanDemoteUser(ulong requester, ulong target)
    {
      MyPromoteLevel userPromoteLevel1 = this.GetUserPromoteLevel(requester);
      MyPromoteLevel userPromoteLevel2 = this.GetUserPromoteLevel(target);
      switch (userPromoteLevel2)
      {
        case MyPromoteLevel.Scripter:
        case MyPromoteLevel.Moderator:
        case MyPromoteLevel.SpaceMaster:
        case MyPromoteLevel.Admin:
          return userPromoteLevel1 >= userPromoteLevel2 && userPromoteLevel1 >= MyPromoteLevel.Admin;
        default:
          return false;
      }
    }

    public void SetAsNotReady()
    {
      this.m_framesToReady = 10;
      this.Ready = false;
    }

    public bool HasPlayerCreativeRights(ulong steamId) => Sandbox.Engine.Multiplayer.MyMultiplayer.Static == null || this.IsUserSpaceMaster(steamId) || this.CreativeMode;

    public bool HasPlayerSpectatorRights(ulong steamId)
    {
      if (this.CreativeMode || this.Settings.EnableSpectator || this.IsUserAdmin(steamId))
        return true;
      return this.IsUserModerator(steamId) && this.CreativeToolsEnabled(steamId);
    }

    private void RaiseOnLoading()
    {
      Action onLoading = MySession.OnLoading;
      if (onLoading == null)
        return;
      onLoading();
    }

    [Event(null, 1016)]
    [Reliable]
    [Broadcast]
    private static void OnServerSaving(bool saveStarted) => MySession.Static.ServerSaving = saveStarted;

    [Event(null, 1025)]
    [Reliable]
    [Broadcast]
    private static void OnServerPerformanceWarning(
      string key,
      MySimpleProfiler.ProfilingBlockType type)
    {
      MySimpleProfiler.ShowServerPerformanceWarning(key, type);
    }

    private void PerformanceWarning(MySimpleProfiler.MySimpleProfilingBlock block) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, MySimpleProfiler.ProfilingBlockType>((Func<IMyEventOwner, Action<string, MySimpleProfiler.ProfilingBlockType>>) (s => new Action<string, MySimpleProfiler.ProfilingBlockType>(MySession.OnServerPerformanceWarning)), block.Name, block.Type);

    private MySession(MySyncLayer syncLayer, bool registerComponents = true)
    {
      if (syncLayer == null)
        MyLog.Default.WriteLine("MySession.Static.MySession() - sync layer is null");
      this.SyncLayer = syncLayer;
      this.ElapsedGameTime = new TimeSpan();
      this.Spectator.Reset();
      MyCubeGrid.ResetInfoGizmos();
      this.m_timeOfSave = MyTimeSpan.Zero;
      this.ElapsedGameTime = new TimeSpan();
      this.Ready = false;
      this.MultiplayerLastMsg = 0.0;
      this.MultiplayerAlive = true;
      this.MultiplayerDirect = true;
      this.AppVersionFromSave = (int) MyFinalBuildConstants.APP_VERSION;
      this.Factions.FactionStateChanged += new Action<MyFactionStateChange, long, long, long, long>(this.OnFactionsStateChanged);
      this.ScriptManager = new MyScriptManager();
      GC.Collect(2, GCCollectionMode.Forced);
      MyLog log1 = MySandboxGame.Log;
      long num = GC.GetTotalMemory(false);
      string msg1 = string.Format("GC Memory: {0} B", (object) num.ToString("##,#"));
      log1.WriteLine(msg1);
      MyLog log2 = MySandboxGame.Log;
      num = Process.GetCurrentProcess().PrivateMemorySize64;
      string msg2 = string.Format("Process Memory: {0} B", (object) num.ToString("##,#"));
      log2.WriteLine(msg2);
      this.GameFocusManager = new MyGameFocusManager();
    }

    private MySession()
      : this(Sandbox.Engine.Platform.Game.IsDedicated ? Sandbox.Engine.Multiplayer.MyMultiplayer.Static.SyncLayer : new MySyncLayer(new MyTransportLayer(2)))
    {
    }

    static MySession()
    {
      if (MyAPIGatewayShortcuts.GetMainCamera == null)
        MyAPIGatewayShortcuts.GetMainCamera = new MyAPIGatewayShortcuts.GetMainCameraCallback(MySession.GetMainCamera);
      if (MyAPIGatewayShortcuts.GetWorldBoundaries == null)
        MyAPIGatewayShortcuts.GetWorldBoundaries = new MyAPIGatewayShortcuts.GetWorldBoundariesCallback(MySession.GetWorldBoundaries);
      if (MyAPIGatewayShortcuts.GetLocalPlayerPosition != null)
        return;
      MyAPIGatewayShortcuts.GetLocalPlayerPosition = new MyAPIGatewayShortcuts.GetLocalPlayerPositionCallback(MySession.GetLocalPlayerPosition);
    }

    internal void StartServer(MyMultiplayerBase multiplayer)
    {
      multiplayer.WorldName = this.Name;
      multiplayer.GameMode = this.Settings.GameMode;
      multiplayer.WorldSize = this.WorldSizeInBytes;
      multiplayer.AppVersion = (int) MyFinalBuildConstants.APP_VERSION;
      multiplayer.DataHash = MyDataIntegrityChecker.GetHashBase64();
      multiplayer.InventoryMultiplier = this.Settings.InventorySizeMultiplier;
      multiplayer.BlocksInventoryMultiplier = this.Settings.BlocksInventorySizeMultiplier;
      multiplayer.AssemblerMultiplier = this.Settings.AssemblerEfficiencyMultiplier;
      multiplayer.RefineryMultiplier = this.Settings.RefinerySpeedMultiplier;
      multiplayer.WelderMultiplier = this.Settings.WelderSpeedMultiplier;
      multiplayer.GrinderMultiplier = this.Settings.GrinderSpeedMultiplier;
      multiplayer.MemberLimit = (int) this.Settings.MaxPlayers;
      multiplayer.Mods = this.Mods;
      multiplayer.ViewDistance = this.Settings.ViewDistance;
      multiplayer.SyncDistance = this.Settings.SyncDistance;
      multiplayer.Scenario = this.IsScenario;
      multiplayer.ExperimentalMode = this.IsSettingsExperimental();
      MyCachedServerItem.SendSettingsToSteam();
      if (Sandbox.Engine.Platform.Game.IsDedicated)
      {
        (multiplayer as MyDedicatedServerBase).SendGameTagsToSteam();
        MySimpleProfiler.ShowPerformanceWarning += new Action<MySimpleProfiler.MySimpleProfilingBlock>(this.PerformanceWarning);
      }
      if (multiplayer is MyMultiplayerLobby)
        ((MyMultiplayerLobby) multiplayer).HostSteamId = Sandbox.Engine.Multiplayer.MyMultiplayer.Static.ServerId;
      MySession.Static.Gpss.RegisterChat(multiplayer);
    }

    private void DisconnectMultiplayer()
    {
      if (Sandbox.Engine.Multiplayer.MyMultiplayer.Static == null)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.Static.ReplicationLayer.Disconnect();
    }

    private void UnloadMultiplayer()
    {
      if (Sandbox.Engine.Multiplayer.MyMultiplayer.Static == null)
        return;
      this.Gpss.UnregisterChat(Sandbox.Engine.Multiplayer.MyMultiplayer.Static);
      Sandbox.Engine.Multiplayer.MyMultiplayer.Static.Dispose();
      this.SyncLayer = (MySyncLayer) null;
    }

    private void LoadGameDefinition(MyDefinitionId? gameDef = null)
    {
      if (!gameDef.HasValue)
        gameDef = new MyDefinitionId?(MyGameDefinition.Default);
      MySession.Static.GameDefinition = MyDefinitionManager.Static.GetDefinition<MyGameDefinition>(gameDef.Value);
      if (MySession.Static.GameDefinition == null)
        MySession.Static.GameDefinition = MyGameDefinition.DefaultDefinition;
      this.RegisterComponentsFromAssemblies();
    }

    private void LoadGameDefinition(MyObjectBuilder_Checkpoint checkpoint)
    {
      if (checkpoint.GameDefinition.IsNull())
      {
        this.LoadGameDefinition();
      }
      else
      {
        MySession.Static.GameDefinition = MyDefinitionManager.Static.GetDefinition<MyGameDefinition>((MyDefinitionId) checkpoint.GameDefinition);
        this.SessionComponentDisabled = checkpoint.SessionComponentDisabled;
        this.SessionComponentEnabled = checkpoint.SessionComponentEnabled;
        this.RegisterComponentsFromAssemblies();
        MySession.ShowMotD = true;
      }
    }

    private void CheckUpdate()
    {
      bool flag = true;
      if (this.IsPausable())
        flag = !MySandboxGame.IsPaused && MySandboxGame.Static.IsActive;
      if (this.m_updateAllowed == flag)
        return;
      this.m_updateAllowed = flag;
      if (!this.m_updateAllowed)
      {
        MyLog.Default.WriteLine("Updating stopped.");
        SortedSet<MySessionComponentBase> sortedSet = (SortedSet<MySessionComponentBase>) null;
        if (!this.m_sessionComponentsForUpdate.TryGetValue(4, out sortedSet))
          return;
        foreach (MySessionComponentBase sessionComponentBase in sortedSet)
          sessionComponentBase.UpdatingStopped();
      }
      else
        MyLog.Default.WriteLine("Updating continues.");
    }

    private void CheckSimSpeed()
    {
      if (!this.Settings.AdaptiveSimulationQuality)
        return;
      int num1 = !this.HighSimulationQuality ? 1 : 0;
      bool flag = (double) MySandboxGame.Static.CPULoadSmooth > 90.0;
      int num2 = flag ? 1 : 0;
      if (num1 != num2)
      {
        --this.m_simQualitySwitchFrames;
        if (this.m_simQualitySwitchFrames > 0)
          return;
        this.HighSimulationQuality = !flag;
        this.m_simQualitySwitchFrames = 30;
        this.m_lastQualitySwitchFrame = this.GameplayFrameCounter - 30;
      }
      else
        this.m_simQualitySwitchFrames = Math.Min(this.m_simQualitySwitchFrames + 1, 30);
    }

    public void Update(MyTimeSpan updateTime)
    {
      if (this.m_updateAllowed && Sandbox.Engine.Multiplayer.MyMultiplayer.Static != null)
        Sandbox.Engine.Multiplayer.MyMultiplayer.Static.ReplicationLayer.UpdateClientStateGroups();
      this.CheckUpdate();
      this.CheckSimSpeed();
      MySession.CheckProfilerDump();
      if (MySandboxGame.Config.SyncRendering)
        Parallel.Scheduler.WaitForTasksToFinish(TimeSpan.FromMilliseconds(-1.0));
      Parallel.RunCallbacks();
      TimeSpan elapsedTimespan = new TimeSpan(0, 0, 0, 0, 16);
      if (this.m_updateAllowed || Sandbox.Engine.Platform.Game.IsDedicated)
      {
        if (MySandboxGame.IsPaused)
          return;
        if (Sandbox.Engine.Multiplayer.MyMultiplayer.Static != null)
          Sandbox.Engine.Multiplayer.MyMultiplayer.Static.ReplicationLayer.UpdateBefore();
        this.UpdateComponents();
        this.Gpss.Update();
        if (Sandbox.Engine.Multiplayer.MyMultiplayer.Static != null)
        {
          Sandbox.Engine.Multiplayer.MyMultiplayer.Static.ReplicationLayer.UpdateAfter();
          Sandbox.Engine.Multiplayer.MyMultiplayer.Static.Tick();
        }
        if ((this.CameraController == null || !this.CameraController.IsInFirstPersonView) && MyThirdPersonSpectator.Static != null)
          MyThirdPersonSpectator.Static.Update();
        if (this.IsServer)
          this.Players.SendDirtyBlockLimits();
        this.ElapsedGameTime += MyRandom.EnableDeterminism ? TimeSpan.FromMilliseconds(16.0) : elapsedTimespan;
        if (this.m_lastTimeMemoryLogged + TimeSpan.FromSeconds(30.0) < DateTime.UtcNow)
        {
          float allocated;
          float used;
          MyVRage.Platform.System.GetGCMemory(out allocated, out used);
          MySandboxGame.Log.WriteLine(string.Format("GC Memory: {0} / {1} MB", (object) allocated, (object) used));
          this.m_lastTimeMemoryLogged = DateTime.UtcNow;
        }
        if (this.AutoSaveInMinutes > 0U && MySandboxGame.IsGameReady && updateTime.TimeSpan - this.m_timeOfSave.TimeSpan > TimeSpan.FromMinutes((double) this.AutoSaveInMinutes))
        {
          MySandboxGame.Log.WriteLine("Autosave initiated");
          MyCharacter localCharacter = this.LocalCharacter;
          bool flag1 = localCharacter != null && !localCharacter.IsDead || localCharacter == null;
          MySandboxGame.Log.WriteLine("Character state: " + flag1.ToString());
          bool flag2 = flag1 & Sync.IsServer;
          MyLog log1 = MySandboxGame.Log;
          bool flag3 = Sync.IsServer;
          string msg1 = "IsServer: " + flag3.ToString();
          log1.WriteLine(msg1);
          bool flag4 = flag2 & !MyAsyncSaving.InProgress;
          MyLog log2 = MySandboxGame.Log;
          flag3 = MyAsyncSaving.InProgress;
          string msg2 = "MyAsyncSaving.InProgress: " + flag3.ToString();
          log2.WriteLine(msg2);
          if (flag4)
          {
            MySandboxGame.Log.WriteLineAndConsole("Autosave");
            MyAsyncSaving.Start((Action) (() => MySector.ResetEyeAdaptation = true));
          }
          this.m_timeOfSave = updateTime;
        }
        if (MySandboxGame.IsGameReady && this.m_framesToReady > 0)
        {
          --this.m_framesToReady;
          if (this.m_framesToReady == 0)
          {
            this.Ready = true;
            MyAudio.Static.PlayMusic();
            this.OnReady.InvokeIfNotNull();
            this.OnReady = (Action) null;
            MySimpleProfiler.Reset(true);
            if (Sandbox.Engine.Platform.Game.IsDedicated)
            {
              MyGameService.GameServer.SetGameReady(true);
              if (!Console.IsInputRedirected && MySandboxGame.IsConsoleVisible)
                MyLog.Default.WriteLineAndConsole("Game ready... Press Ctrl+C to exit");
              else
                MyLog.Default.WriteLineAndConsole("Game ready... ");
            }
          }
        }
        if (Sync.MultiplayerActive && !Sync.IsServer)
          this.CheckMultiplayerStatus();
        ++this.m_gameplayFrameCounter;
      }
      else if (MySandboxGame.IsPaused && Sync.IsServer && !Sandbox.Engine.Platform.Game.IsDedicated)
        this.UpdateComponentsWhilePaused();
      this.UpdateStatistics(ref elapsedTimespan);
      this.DebugDraw();
      for (int index = this.m_updateCallbacks.Count - 1; index >= 0; --index)
      {
        this.m_updateCallbacks[index].Update();
        if (this.m_updateCallbacks[index].ToBeRemoved)
          this.m_updateCallbacks.RemoveAtFast<MyUpdateCallback>(index);
      }
    }

    public void AddUpdateCallback(MyUpdateCallback callback) => this.m_updateCallbacks.Add(callback);

    private static void CheckProfilerDump()
    {
      --MySession.m_profilerDumpDelay;
      if (MySession.m_profilerDumpDelay == 0)
      {
        MyRenderProxy.GetRenderProfiler().Dump();
        VRage.Profiler.MyRenderProfiler.SetLevel(0);
      }
      else
      {
        if (MySession.m_profilerDumpDelay >= 0)
          return;
        MySession.m_profilerDumpDelay = -1;
      }
    }

    private void DebugDraw()
    {
      if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
        return;
      int num = MyDebugDrawSettings.DEBUG_DRAW_CONTROLLED_ENTITIES ? 1 : 0;
      if (MyDebugDrawSettings.DEBUG_DRAW_ASTEROID_COMPOSITION)
        this.VoxelMaps.DebugDraw(MyVoxelDebugDrawMode.Content_DataProvider);
      if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_ACCESS)
        this.VoxelMaps.DebugDraw(MyVoxelDebugDrawMode.Content_Access);
      if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_FULLCELLS)
        this.VoxelMaps.DebugDraw(MyVoxelDebugDrawMode.FullCells);
      if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_CONTENT_MICRONODES)
        this.VoxelMaps.DebugDraw(MyVoxelDebugDrawMode.Content_MicroNodes);
      if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_CONTENT_MICRONODES_SCALED)
        this.VoxelMaps.DebugDraw(MyVoxelDebugDrawMode.Content_MicroNodesScaled);
      if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_CONTENT_MACRONODES)
        this.VoxelMaps.DebugDraw(MyVoxelDebugDrawMode.Content_MacroNodes);
      if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_CONTENT_MACROLEAVES)
        this.VoxelMaps.DebugDraw(MyVoxelDebugDrawMode.Content_MacroLeaves);
      if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_CONTENT_MACRO_SCALED)
        this.VoxelMaps.DebugDraw(MyVoxelDebugDrawMode.Content_MacroScaled);
      if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_MATERIALS_MACRONODES)
        this.VoxelMaps.DebugDraw(MyVoxelDebugDrawMode.Materials_MacroNodes);
      if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_MATERIALS_MACROLEAVES)
        this.VoxelMaps.DebugDraw(MyVoxelDebugDrawMode.Materials_MacroLeaves);
      if (!MyDebugDrawSettings.DEBUG_DRAW_ENCOUNTERS)
        return;
      MyEncounterGenerator.Static.DebugDraw();
    }

    private void CheckMultiplayerStatus()
    {
      this.MultiplayerAlive = Sandbox.Engine.Multiplayer.MyMultiplayer.Static.IsConnectionAlive;
      this.MultiplayerDirect = Sandbox.Engine.Multiplayer.MyMultiplayer.Static.IsConnectionDirect;
      if (Sync.IsServer)
      {
        this.MultiplayerLastMsg = 0.0;
      }
      else
      {
        this.MultiplayerLastMsg = (DateTime.UtcNow - Sandbox.Engine.Multiplayer.MyMultiplayer.Static.LastMessageReceived).TotalSeconds;
        if (!(Sandbox.Engine.Multiplayer.MyMultiplayer.ReplicationLayer is MyReplicationClient replicationLayer))
          return;
        this.MultiplayerPing = replicationLayer.Ping;
        DateTime now = DateTime.Now;
        if (replicationLayer.PendingStreamingRelicablesCount > 0)
        {
          TimeSpan timeSpan = TimeSpan.FromSeconds(0.5);
          DateTime dateTime = now + timeSpan;
          if (this.m_streamingIndicatorShowTime > dateTime)
            this.m_streamingIndicatorShowTime = dateTime;
        }
        else
        {
          TimeSpan timeSpan = TimeSpan.FromSeconds(1.0);
          if (now < this.m_streamingIndicatorShowTime || now >= this.m_streamingIndicatorShowTime + timeSpan)
            this.m_streamingIndicatorShowTime = DateTime.MaxValue;
        }
        this.StreamingInProgress = this.m_streamingIndicatorShowTime <= now;
      }
    }

    public bool IsPausable() => !Sync.MultiplayerActive;

    public bool IsServer => Sync.IsServer || Sandbox.Engine.Multiplayer.MyMultiplayer.Static == null;

    private void UpdateStatistics(ref TimeSpan elapsedTimespan)
    {
      this.ElapsedPlayTime += MyRandom.EnableDeterminism ? TimeSpan.FromMilliseconds(16.0) : elapsedTimespan;
      this.SessionSimSpeedPlayer += MyPhysics.SimulationRatio * (float) elapsedTimespan.TotalSeconds;
      this.SessionSimSpeedServer += Sync.ServerSimulationRatio * (float) elapsedTimespan.TotalSeconds;
      if (this.LocalHumanPlayer != null && this.LocalHumanPlayer.Character != null)
      {
        if (this.ControlledEntity is MyCharacter)
        {
          if (((MyCharacter) this.ControlledEntity).GetCurrentMovementState() == MyCharacterMovementEnum.Flying)
          {
            this.TimeOnJetpack += elapsedTimespan;
          }
          else
          {
            this.TimeOnFoot += elapsedTimespan;
            if (((MyCharacter) this.ControlledEntity).IsSprinting)
              this.TimeSprinting += elapsedTimespan;
          }
          MyCharacterSoundComponent soundComp = ((MyCharacter) this.ControlledEntity).SoundComp;
          if (soundComp != null)
          {
            if (soundComp.StandingOnGrid != null)
            {
              if (soundComp.StandingOnGrid.IsStatic)
                this.TimeOnStation += elapsedTimespan;
              else
                this.TimeOnShips += elapsedTimespan;
            }
            if (soundComp.StandingOnVoxel != null)
            {
              if (soundComp.StandingOnVoxel is MyVoxelPhysics && soundComp.StandingOnVoxel.RootVoxel is MyPlanet)
                this.TimeOnPlanets += elapsedTimespan;
              else
                this.TimeOnAsteroids += elapsedTimespan;
            }
          }
          if (((MyCharacter) this.ControlledEntity).IsInFirstPersonView)
            this.TimeInCameraCharFirstPerson += elapsedTimespan;
          else
            this.TimeInCameraCharThirdPerson += elapsedTimespan;
          IMyEntity equippedTool = ((MyCharacter) this.ControlledEntity).EquippedTool;
          switch (equippedTool)
          {
            case IMyHandheldGunObject<MyGunBase> _:
              if (((MyCharacter) this.ControlledEntity).IsInFirstPersonView)
              {
                this.TimeInCameraWeaponFirstPerson += elapsedTimespan;
                break;
              }
              this.TimeInCameraWeaponThirdPerson += elapsedTimespan;
              break;
            case MyBlockPlacerBase _:
              if (((MyCharacter) this.ControlledEntity).IsInFirstPersonView)
              {
                this.TimeInCameraBuildingFirstPerson += elapsedTimespan;
                break;
              }
              this.TimeInCameraBuildingThirdPerson += elapsedTimespan;
              break;
            case IMyHandheldGunObject<MyToolBase> _:
              if (((MyCharacter) this.ControlledEntity).IsInFirstPersonView)
                this.TimeInCameraToolFirstPerson += elapsedTimespan;
              else
                this.TimeInCameraToolThirdPerson += elapsedTimespan;
              if (((IMyGunObject<MyToolBase>) equippedTool).IsShooting && equippedTool is MyAngleGrinder && ((MyEngineerToolBase) equippedTool).HasHitBlock)
              {
                this.TimeGrindingBlocks += elapsedTimespan;
                MyCubeGrid cubeGrid = ((MyEngineerToolBase) equippedTool).GetTargetBlock().CubeGrid;
                long playerId = cubeGrid.BigOwners.Count > 0 ? cubeGrid.BigOwners[0] : (cubeGrid.SmallOwners.Count > 0 ? cubeGrid.SmallOwners[0] : 0L);
                switch (playerId != 0L ? ((MyCharacter) this.ControlledEntity).GetRelationTo(playerId) : MyRelationsBetweenPlayerAndBlock.NoOwnership)
                {
                  case MyRelationsBetweenPlayerAndBlock.NoOwnership:
                  case MyRelationsBetweenPlayerAndBlock.Neutral:
                    this.TimeGrindingNeutralBlocks += elapsedTimespan;
                    break;
                  case MyRelationsBetweenPlayerAndBlock.Owner:
                  case MyRelationsBetweenPlayerAndBlock.FactionShare:
                  case MyRelationsBetweenPlayerAndBlock.Friends:
                    this.TimeGrindingFriendlyBlocks += elapsedTimespan;
                    break;
                  default:
                    this.TimeGrindingEnemyBlocks += elapsedTimespan;
                    break;
                }
              }
              else
                break;
              break;
          }
        }
        else if (this.ControlledEntity is MyCockpit)
        {
          if (((MyShipController) this.ControlledEntity).IsLargeShip())
            this.TimePilotingBigShip += elapsedTimespan;
          else
            this.TimePilotingSmallShip += elapsedTimespan;
          if (((MyShipController) this.ControlledEntity).BuildingMode)
            this.TimeInBuilderMode += elapsedTimespan;
          if (((MyCockpit) this.ControlledEntity).IsInFirstPersonView)
            this.TimeInCameraGridFirstPerson += elapsedTimespan;
          else
            this.TimeInCameraGridThirdPerson += elapsedTimespan;
        }
      }
      if (this.CreativeToolsEnabled(Sync.MyId))
        this.TimeCreativeToolsEnabled += elapsedTimespan;
      if (MyInput.Static.IsJoystickLastUsed)
        this.TimeUsingGamepadInput += elapsedTimespan;
      else
        this.TimeUsingMouseInput += elapsedTimespan;
    }

    public void ResetStatistics()
    {
      this.AmountMined = new Dictionary<string, MyFixedPoint>();
      this.ElapsedPlayTime = TimeSpan.FromMinutes(0.0);
      this.TimeOnJetpack = TimeSpan.FromMinutes(0.0);
      this.TimeOnFoot = TimeSpan.FromMinutes(0.0);
      this.TimeSprinting = TimeSpan.FromMinutes(0.0);
      this.TimeOnStation = TimeSpan.FromMinutes(0.0);
      this.TimeOnShips = TimeSpan.FromMinutes(0.0);
      this.TimeOnPlanets = TimeSpan.FromMinutes(0.0);
      this.TimeOnAsteroids = TimeSpan.FromMinutes(0.0);
      this.TimeInCameraCharFirstPerson = TimeSpan.FromMinutes(0.0);
      this.TimeInCameraCharThirdPerson = TimeSpan.FromMinutes(0.0);
      this.TimeInCameraWeaponFirstPerson = TimeSpan.FromMinutes(0.0);
      this.TimeInCameraWeaponThirdPerson = TimeSpan.FromMinutes(0.0);
      this.TimeInCameraBuildingFirstPerson = TimeSpan.FromMinutes(0.0);
      this.TimeInCameraBuildingThirdPerson = TimeSpan.FromMinutes(0.0);
      this.TimeInCameraToolFirstPerson = TimeSpan.FromMinutes(0.0);
      this.TimeInCameraToolThirdPerson = TimeSpan.FromMinutes(0.0);
      this.TimeInCameraGridFirstPerson = TimeSpan.FromMinutes(0.0);
      this.TimeInCameraGridThirdPerson = TimeSpan.FromMinutes(0.0);
      this.TimePilotingBigShip = TimeSpan.FromMinutes(0.0);
      this.TimePilotingSmallShip = TimeSpan.FromMinutes(0.0);
      this.TimeInBuilderMode = TimeSpan.FromMinutes(0.0);
      this.TimeCreativeToolsEnabled = TimeSpan.FromMinutes(0.0);
      this.TimeUsingGamepadInput = TimeSpan.FromMinutes(0.0);
      this.TimeUsingMouseInput = TimeSpan.FromMinutes(0.0);
      this.TimeGrindingBlocks = TimeSpan.FromMinutes(0.0);
      this.TimeGrindingFriendlyBlocks = TimeSpan.FromMinutes(0.0);
      this.TimeGrindingNeutralBlocks = TimeSpan.FromMinutes(0.0);
      this.TimeGrindingEnemyBlocks = TimeSpan.FromMinutes(0.0);
      this.PositiveIntegrityTotal = 0.0f;
      this.NegativeIntegrityTotal = 0.0f;
      this.VoxelHandVolumeChanged = 0UL;
      this.TotalDamageDealt = 0U;
      this.TotalBlocksCreated = 0U;
      this.TotalBlocksCreatedFromShips = 0U;
      this.ToolbarPageSwitches = 0U;
    }

    public void HandleInput()
    {
      foreach (MySessionComponentBase sessionComponentBase in this.m_sessionComponents.Values)
        sessionComponentBase.HandleInput();
    }

    public void Draw()
    {
      foreach (MySessionComponentBase sessionComponentBase in this.m_sessionComponents.Values)
        sessionComponentBase.Draw();
    }

    public void DrawAsync()
    {
      foreach (MySessionComponentBase sessionComponentBase in this.m_sessionComponentForDrawAsync)
        sessionComponentBase.Draw();
    }

    public void DrawSync()
    {
      foreach (MySessionComponentBase sessionComponentBase in this.m_sessionComponentForDraw)
        sessionComponentBase.Draw();
    }

    public static bool IsCompatibleVersion(MyObjectBuilder_Checkpoint checkpoint) => checkpoint != null && checkpoint.AppVersion <= (int) MyFinalBuildConstants.APP_VERSION;

    public static void Start(
      string name,
      string description,
      string password,
      MyObjectBuilder_SessionSettings settings,
      List<MyObjectBuilder_Checkpoint.ModItem> mods,
      MyWorldGenerator.Args generationArgs)
    {
      MyLog.Default.WriteLineAndConsole("Starting world " + name);
      MyEntityContainerEventExtensions.InitEntityEvents();
      MySession.Static = new MySession();
      MySession.Static.Name = name;
      MySession.Static.Mods = mods;
      MySession.Static.Description = description;
      MySession.Static.Password = password;
      MySession.Static.Settings = settings;
      MySession.Static.Scenario = generationArgs.Scenario;
      double num = (double) (settings.WorldSizeKm * 500);
      if (num > 0.0)
        MySession.Static.WorldBoundaries = new BoundingBoxD?(new BoundingBoxD(new Vector3D(-num, -num, -num), new Vector3D(num, num, num)));
      MyVisualScriptLogicProvider.Init();
      MySession.Static.InGameTime = generationArgs.Scenario.GameDate;
      MySession.Static.RequiresDX = generationArgs.Scenario.HasPlanets ? 11 : 9;
      if (MySession.Static.OnlineMode != MyOnlineModeEnum.OFFLINE)
      {
        MySession.Static.StartServerRequest();
        MyLobbyStatusCode statusCode;
        if (MySession.Static.WaitForServerRequest(out statusCode))
        {
          MySession.Static.ShowLoadingError(true, statusCode);
          return;
        }
      }
      MySession.Static.IsCameraAwaitingEntity = true;
      string sessionUniqueName = MyUtils.StripInvalidChars(name);
      MySession.Static.CurrentPath = MyLocalCache.GetSessionSavesPath(sessionUniqueName, false, false);
      while (Directory.Exists(MySession.Static.CurrentPath))
        MySession.Static.CurrentPath = MyLocalCache.GetSessionSavesPath(sessionUniqueName + MyUtils.GetRandomInt(int.MaxValue).ToString("########"), false, false);
      MySession.Static.PrepareBaseSession(mods, generationArgs.Scenario);
      MySector.EnvironmentDefinition = MyDefinitionManager.Static.GetDefinition<MyEnvironmentDefinition>(generationArgs.Scenario.Environment);
      MyWorldGenerator.GenerateWorld(generationArgs);
      if (Sync.IsServer)
        MySession.Static.InitializeFactions();
      if (!Sandbox.Engine.Platform.Game.IsDedicated)
        MyToolBarCollection.RequestCreateToolbar(new MyPlayer.PlayerId(Sync.MyId, 0));
      MySession.Static.LogSettings(generationArgs.Scenario.DisplayNameText.ToString(), generationArgs.AsteroidAmount);
      if (generationArgs.Scenario.SunDirection.IsValid())
      {
        MySector.SunProperties.SunDirectionNormalized = Vector3.Normalize(generationArgs.Scenario.SunDirection);
        MySector.SunProperties.BaseSunDirectionNormalized = Vector3.Normalize(generationArgs.Scenario.SunDirection);
      }
      MyPrefabManager.FinishedProcessingGrids.Reset();
      if (MyPrefabManager.PendingGrids > 0)
        MyPrefabManager.FinishedProcessingGrids.WaitOne();
      Parallel.RunCallbacks();
      Sandbox.Game.Entities.MyEntities.UpdateOnceBeforeFrame();
      MySession.Static.BeforeStartComponents();
      MySession.Static.Save((string) null);
      MySession.Static.SessionSimSpeedPlayer = 0.0f;
      MySession.Static.SessionSimSpeedServer = 0.0f;
      MySpectatorCameraController.Static.InitLight(false);
    }

    public MyGameDefinition GameDefinition { get; set; }

    internal static void LoadMultiplayer(
      MyObjectBuilder_World world,
      MyMultiplayerBase multiplayerSession)
    {
      if (MyFakes.ENABLE_PRELOAD_CHARACTER_ANIMATIONS)
        MySession.PreloadAnimations("Models\\Characters\\Animations");
      MySession.Static = new MySession(multiplayerSession.SyncLayer);
      MySession.Static.Mods = world.Checkpoint.Mods;
      MySession.Static.Settings = world.Checkpoint.Settings;
      MySession.Static.CurrentPath = MyLocalCache.GetSessionSavesPath(MyUtils.StripInvalidChars(world.Checkpoint.SessionName), false, false);
      if (!MyDefinitionManager.Static.TryGetDefinition<MyScenarioDefinition>((MyDefinitionId) world.Checkpoint.Scenario, out MySession.Static.Scenario))
        MySession.Static.Scenario = MyDefinitionManager.Static.GetScenarioDefinitions().FirstOrDefault<MyScenarioDefinition>();
      MySession mySession = MySession.Static;
      SerializableBoundingBoxD? worldBoundaries = world.Checkpoint.WorldBoundaries;
      BoundingBoxD? nullable = worldBoundaries.HasValue ? new BoundingBoxD?((BoundingBoxD) worldBoundaries.GetValueOrDefault()) : new BoundingBoxD?();
      mySession.WorldBoundaries = nullable;
      MySession.Static.InGameTime = MyObjectBuilder_Checkpoint.DEFAULT_DATE;
      MySession.Static.LoadMembersFromWorld(world, multiplayerSession);
      MySandboxGame.Static.SessionCompatHelper.FixSessionComponentObjectBuilders(world.Checkpoint, world.Sector);
      MySession.Static.PrepareBaseSession(world.Checkpoint, world.Sector);
      if (MyFakes.MP_SYNC_CLUSTERTREE)
        MyPhysics.DeserializeClusters(world.Clusters);
      try
      {
        foreach (MyObjectBuilder_Planet planet in world.Planets)
        {
          MyPlanet myPlanet = new MyPlanet();
          MyPlanetStorageProvider planetStorageProvider = new MyPlanetStorageProvider();
          MyPlanetGeneratorDefinition definition = MyDefinitionManager.Static.GetDefinition<MyPlanetGeneratorDefinition>(MyStringHash.GetOrCompute(planet.PlanetGenerator));
          planetStorageProvider.Init((long) planet.Seed, definition, (double) planet.Radius, true);
          VRage.Game.Voxels.IMyStorage storage = (VRage.Game.Voxels.IMyStorage) new MyOctreeStorage((IMyStorageDataProvider) planetStorageProvider, planetStorageProvider.StorageSize);
          myPlanet.Init((MyObjectBuilder_EntityBase) planet, storage);
          Sandbox.Game.Entities.MyEntities.Add((MyEntity) myPlanet);
        }
      }
      catch (MyPlanetWhitelistException ex)
      {
        throw new MyLoadingTooManyPlanetsException();
      }
      long controlledObject = world.Checkpoint.ControlledObject;
      world.Checkpoint.ControlledObject = -1L;
      if (multiplayerSession != null)
        MySession.Static.Gpss.RegisterChat(multiplayerSession);
      MySession.Static.CameraController = (IMyCameraController) MySpectatorCameraController.Static;
      MySession.Static.LoadWorld(world.Checkpoint, world.Sector);
      if (Sync.IsServer)
        MySession.Static.InitializeFactions();
      MySession.Static.Settings.AutoSaveInMinutes = 0U;
      MySession.Static.IsCameraAwaitingEntity = true;
      MyGeneralStats.Clear();
      if (!Sandbox.Engine.Platform.Game.IsDedicated)
        MySession.CacheGuiTexturePaths();
      MySession.Static.BeforeStartComponents();
    }

    public static void LoadMission(
      string sessionPath,
      MyObjectBuilder_Checkpoint checkpoint,
      ulong checkpointSizeInBytes,
      bool persistentEditMode)
    {
      MySession.LoadMission(sessionPath, checkpoint, checkpointSizeInBytes, checkpoint.SessionName, checkpoint.Description);
      MySession.Static.PersistentEditMode = persistentEditMode;
      MySession.Static.LoadedAsMission = true;
    }

    public static void LoadMission(
      string sessionPath,
      MyObjectBuilder_Checkpoint checkpoint,
      ulong checkpointSizeInBytes,
      string name,
      string description)
    {
      MySpaceAnalytics.Instance.SetWorldEntry(MyWorldEntryEnum.Load);
      MySession.Load(sessionPath, checkpoint, checkpointSizeInBytes);
      MySession.Static.Name = name;
      MySession.Static.Description = description;
      string sessionUniqueName = MyUtils.StripInvalidChars(checkpoint.SessionName);
      MySession.Static.CurrentPath = MyLocalCache.GetSessionSavesPath(sessionUniqueName, false, false);
      while (Directory.Exists(MySession.Static.CurrentPath))
        MySession.Static.CurrentPath = MyLocalCache.GetSessionSavesPath(sessionUniqueName + MyUtils.GetRandomInt(int.MaxValue).ToString("########"), false, false);
    }

    public static void Load(
      string sessionPath,
      MyObjectBuilder_Checkpoint checkpoint,
      ulong checkpointSizeInBytes,
      bool saveLastStates = true,
      bool allowXml = true)
    {
      PrioritizedScheduler scheduler = Parallel.Scheduler as PrioritizedScheduler;
      scheduler.SuspendThreads(WorkPriority.VeryLow);
      try
      {
        MyLog.Default.WriteLineAndConsole("Loading session: " + sessionPath);
        MySession.Static = new MySession()
        {
          Name = MyStatControlText.SubstituteTexts(checkpoint.SessionName),
          Description = checkpoint.Description,
          Mods = checkpoint.Mods,
          Settings = checkpoint.Settings,
          CurrentPath = sessionPath
        };
        MySession.Static.Settings.EnableIngameScripts &= MyVRage.Platform.Scripting.IsRuntimeCompilationSupported;
        MyObjectBuilder_SessionSettings.ExperimentalReason experimentalReason = MySession.Static.GetSettingsExperimentalReason(false, checkpoint);
        MyLog.Default.WriteLineAndConsole("Experimental mode: " + (experimentalReason != (MyObjectBuilder_SessionSettings.ExperimentalReason) 0 ? "Yes" : "No"));
        MyLog.Default.WriteLineAndConsole("Experimental mode reason: " + (object) experimentalReason);
        MyLog.Default.WriteLineAndConsole("Console compatibility: " + (MyPlatformGameSettings.CONSOLE_COMPATIBLE ? "Yes" : "No"));
        MyEntityIdentifier.Reset();
        bool needsXml = false;
        ulong sizeInBytes;
        MyObjectBuilder_Sector sector = MyLocalCache.LoadSector(sessionPath, (Vector3I) checkpoint.CurrentSector, allowXml, out sizeInBytes, out needsXml);
        if (sector == null)
        {
          if (!allowXml & needsXml)
            throw new MyLoadingNeedXMLException();
          throw new ApplicationException("Sector could not be loaded");
        }
        if (!Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.OnlineMode != MyOnlineModeEnum.OFFLINE)
          MySession.Static.StartServerRequest();
        ulong voxelsSizeInBytes = MySession.GetVoxelsSizeInBytes(sessionPath);
        MySession.Static.WorldSizeInBytes = checkpointSizeInBytes + sizeInBytes + voxelsSizeInBytes;
        MyCubeGrid.Preload();
        Action beforeLoading = MySession.BeforeLoading;
        if (beforeLoading != null)
          beforeLoading();
        MySandboxGame.Static.SessionCompatHelper.FixSessionComponentObjectBuilders(checkpoint, sector);
        MySession.Static.PrepareBaseSession(checkpoint, sector);
        MyVisualScriptLogicProvider.Init();
        MyLobbyStatusCode statusCode;
        if (!Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.OnlineMode != MyOnlineModeEnum.OFFLINE && !MySession.Static.WaitForServerRequest(out statusCode))
        {
          MySession.Static.ShowLoadingError(true, statusCode);
        }
        else
        {
          MySession.Static.LoadWorld(checkpoint, sector);
          scheduler.ResumeThreads(WorkPriority.VeryLow);
          if (Sync.IsServer)
            MySession.Static.InitializeFactions();
          if (saveLastStates)
            MyLocalCache.SaveLastSessionInfo(sessionPath, false, false, MySession.Static.Name, (string) null, 0);
          MySession.Static.LogSettings();
          MyHud.Notifications.Get(MyNotificationSingletons.WorldLoaded).SetTextFormatArguments((object) MySession.Static.Name);
          MyHud.Notifications.Add(MyNotificationSingletons.WorldLoaded);
          if (!MyFakes.LOAD_UNCONTROLLED_CHARACTERS && !MySessionComponentReplay.Static.HasAnyData)
            MySession.Static.RemoveUncontrolledCharacters();
          MyGeneralStats.Clear();
          MyHudChat.ResetChatSettings();
          MySession.Static.BeforeStartComponents();
          if (!Sandbox.Engine.Platform.Game.IsDedicated)
            MySession.CacheGuiTexturePaths();
          MySession.RaiseAfterLoading();
          MySpaceBindingCreator.CreateBindingDefault();
          if (!Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalCharacter != null)
          {
            foreach (MyCharacter savedCharacter in MySession.Static.SavedCharacters)
              MyLocalCache.LoadInventoryConfig(savedCharacter);
          }
          MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.SendEntitiesToClients();
          MyLog.Default.WriteLineAndConsole("Session loaded");
        }
      }
      finally
      {
        scheduler.ResumeThreads(WorkPriority.VeryLow);
      }
    }

    private static void PreloadModels(MyObjectBuilder_Sector sector)
    {
      HashSet<string> source1 = new HashSet<string>();
      if (sector.SectorObjects != null)
      {
        foreach (MyObjectBuilder_EntityBase sectorObject in sector.SectorObjects)
        {
          if (sectorObject is MyObjectBuilder_CubeGrid)
          {
            foreach (MyObjectBuilder_Base cubeBlock in (sectorObject as MyObjectBuilder_CubeGrid).CubeBlocks)
            {
              MyCubeBlockDefinition blockDefinition;
              if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(cubeBlock.GetId(), out blockDefinition) && !string.IsNullOrEmpty(blockDefinition.Model))
                source1.Add(blockDefinition.Model);
            }
          }
        }
      }
      string[] blockModels = source1.ToArray<string>();
      WorkPriority priority = WorkPriority.Normal;
      Parallel.Start((Action) (() =>
      {
        WorkOptions workOptions = Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Loading, "Block Models Preload For");
        workOptions.MaximumThreads = int.MaxValue;
        Parallel.For(0, blockModels.Length, (Action<int>) (i => MyModels.GetModelOnlyData(blockModels[i])), priority, new WorkOptions?(workOptions));
      }), Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Loading, "Block Models Preload"), priority);
      HashSet<string> source2 = new HashSet<string>();
      foreach (MyPhysicalItemDefinition physicalItemDefinition in MyDefinitionManager.Static.GetPhysicalItemDefinitions())
      {
        if (physicalItemDefinition.HasModelVariants)
        {
          if (physicalItemDefinition.Models != null)
          {
            foreach (string model in physicalItemDefinition.Models)
              source2.Add(model);
          }
        }
        else if (!string.IsNullOrEmpty(physicalItemDefinition.Model))
          source2.Add(physicalItemDefinition.Model);
      }
      foreach (MyDebrisDefinition debrisDefinition in MyDefinitionManager.Static.GetDebrisDefinitions())
      {
        if (!string.IsNullOrEmpty(debrisDefinition.Model))
          source2.Add(debrisDefinition.Model);
      }
      WorkPriority animPriority = WorkPriority.VeryLow;
      string[] models = source2.ToArray<string>();
      Parallel.Start((Action) (() =>
      {
        WorkOptions workOptions = Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Loading, "Models Preload For");
        workOptions.MaximumThreads = int.MaxValue;
        Parallel.For(0, models.Length, (Action<int>) (i => MyModels.GetModelOnlyData(models[i])), animPriority, new WorkOptions?(workOptions));
        MyCharacter.Preload(animPriority);
      }), Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Loading, "Models Preload"), animPriority);
    }

    private static void CacheGuiTexturePaths()
    {
      foreach (MyFactionIconsDefinition allDefinition in MyDefinitionManager.Static.GetAllDefinitions<MyFactionIconsDefinition>())
      {
        if (allDefinition.Icons != null && allDefinition.Icons.Length != 0)
        {
          for (int index = 0; index < allDefinition.Icons.Length; ++index)
          {
            if (!string.IsNullOrEmpty(allDefinition.Icons[index]))
              ImageManager.Instance.AddImage(allDefinition.Icons[index]);
          }
        }
      }
      foreach (MyPhysicalItemDefinition physicalItemDefinition in MyDefinitionManager.Static.GetPhysicalItemDefinitions())
      {
        if (physicalItemDefinition.Icons != null && physicalItemDefinition.Icons.Length != 0 && !string.IsNullOrEmpty(physicalItemDefinition.Icons[0]))
          ImageManager.Instance.AddImage(physicalItemDefinition.Icons[0]);
      }
      foreach (MyPrefabDefinition prefabDefinition in MyDefinitionManager.Static.GetPrefabDefinitions().Values)
      {
        if (!prefabDefinition.Context.IsBaseGame)
        {
          string modPath = prefabDefinition.Context.ModPath;
          if (prefabDefinition.Icons != null && prefabDefinition.Icons.Length == 1 && !string.IsNullOrEmpty(prefabDefinition.Icons[0]))
          {
            prefabDefinition.Icons[0] = Path.Combine(modPath, prefabDefinition.Icons[0]);
            ImageManager.Instance.AddImage(prefabDefinition.Icons[0]);
          }
          if (!string.IsNullOrEmpty(prefabDefinition.TooltipImage))
          {
            prefabDefinition.TooltipImage = Path.Combine(modPath, prefabDefinition.TooltipImage);
            ImageManager.Instance.AddImage(prefabDefinition.TooltipImage);
          }
        }
      }
    }

    private static void PreloadAnimations(string relativeDirectory)
    {
      IEnumerable<string> files = MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, relativeDirectory), "*.mwm", MySearchOption.AllDirectories);
      if (files == null || !files.Any<string>())
        return;
      foreach (string str in files)
        MyModels.GetModelOnlyAnimationData(str.Replace(MyFileSystem.ContentPath, string.Empty).TrimStart(Path.DirectorySeparatorChar));
    }

    internal static void CreateWithEmptyWorld(MyMultiplayerBase multiplayerSession)
    {
      MySession.Static = new MySession(multiplayerSession.SyncLayer, false);
      MySession.Static.InGameTime = MyObjectBuilder_Checkpoint.DEFAULT_DATE;
      MySession.Static.Gpss.RegisterChat(multiplayerSession);
      MySession.Static.CameraController = (IMyCameraController) MySpectatorCameraController.Static;
      MySession.Static.Settings = new MyObjectBuilder_SessionSettings();
      MySession.Static.Settings.AutoSaveInMinutes = 0U;
      MySession.Static.IsCameraAwaitingEntity = true;
      MySession.Static.PrepareBaseSession(new List<MyObjectBuilder_Checkpoint.ModItem>());
      multiplayerSession.StartProcessingClientMessagesWithEmptyWorld();
      if (Sync.IsServer)
        MySession.Static.InitializeFactions();
      MyLocalCache.ClearLastSessionInfo();
      if (!Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalHumanPlayer == null)
        Sync.Players.RequestNewPlayer(Sync.MyId, 0, MyGameService.UserName, (string) null, true, true);
      MyGeneralStats.Clear();
    }

    internal void LoadMultiplayerWorld(
      MyObjectBuilder_World world,
      MyMultiplayerBase multiplayerSession)
    {
      MySession.Static.UnloadDataComponents(true);
      MyDefinitionManager.Static.UnloadData();
      MySession.Static.Mods = world.Checkpoint.Mods;
      MySession.Static.Settings = world.Checkpoint.Settings;
      MySession.Static.CurrentPath = MyLocalCache.GetSessionSavesPath(MyUtils.StripInvalidChars(world.Checkpoint.SessionName), false, false);
      if (!MyDefinitionManager.Static.TryGetDefinition<MyScenarioDefinition>((MyDefinitionId) world.Checkpoint.Scenario, out MySession.Static.Scenario))
        MySession.Static.Scenario = MyDefinitionManager.Static.GetScenarioDefinitions().FirstOrDefault<MyScenarioDefinition>();
      MySession.Static.InGameTime = MyObjectBuilder_Checkpoint.DEFAULT_DATE;
      MySandboxGame.Static.SessionCompatHelper.FixSessionComponentObjectBuilders(world.Checkpoint, world.Sector);
      MySession.Static.PrepareBaseSession(world.Checkpoint, world.Sector);
      long controlledObject = world.Checkpoint.ControlledObject;
      world.Checkpoint.ControlledObject = -1L;
      MySession.Static.Gpss.RegisterChat(multiplayerSession);
      MySession.Static.CameraController = (IMyCameraController) MySpectatorCameraController.Static;
      MySession.Static.LoadWorld(world.Checkpoint, world.Sector);
      if (Sync.IsServer)
        MySession.Static.InitializeFactions();
      MySession.Static.Settings.AutoSaveInMinutes = 0U;
      MySession.Static.IsCameraAwaitingEntity = true;
      MyLocalCache.ClearLastSessionInfo();
      if (!Sandbox.Engine.Platform.Game.IsDedicated)
        MySession.CacheGuiTexturePaths();
      MySession.Static.BeforeStartComponents();
    }

    private void LoadMembersFromWorld(
      MyObjectBuilder_World world,
      MyMultiplayerBase multiplayerSession)
    {
      if (!(multiplayerSession is MyMultiplayerClient))
        return;
      (multiplayerSession as MyMultiplayerClient).LoadMembersFromWorld(world.Checkpoint.Clients);
    }

    private void RemoveUncontrolledCharacters()
    {
      if (!Sync.IsServer)
        return;
      foreach (MyCharacter myCharacter in Sandbox.Game.Entities.MyEntities.GetEntities().OfType<MyCharacter>())
      {
        if ((myCharacter.ControllerInfo.Controller == null || myCharacter.ControllerInfo.IsRemotelyControlled() && myCharacter.GetCurrentMovementState() != MyCharacterMovementEnum.Died) && ((!(this.ControlledEntity is MyLargeTurretBase controlledEntity) || controlledEntity.Pilot != myCharacter) && (!(this.ControlledEntity is MyRemoteControl controlledEntity) || controlledEntity.Pilot != myCharacter)))
          myCharacter.Close();
      }
      foreach (MyCubeGrid myCubeGrid in Sandbox.Game.Entities.MyEntities.GetEntities().OfType<MyCubeGrid>())
      {
        foreach (MySlimBlock block in myCubeGrid.GetBlocks())
        {
          if (block.FatBlock is MyCockpit fatBlock && !(fatBlock is MyCryoChamber) && (fatBlock.Pilot != null && fatBlock.Pilot != this.LocalCharacter))
          {
            fatBlock.Pilot.Close();
            fatBlock.ClearSavedpilot();
          }
        }
      }
    }

    private void StartServerRequest()
    {
      if (MyGameService.IsOnline)
      {
        this.UnloadMultiplayer();
        MyNetworkMonitor.StartSession();
        this.m_serverRequest = Sandbox.Engine.Multiplayer.MyMultiplayer.HostLobby(MySession.GetLobbyType(MySession.Static.OnlineMode), (int) MySession.Static.MaxPlayers, MySession.Static.SyncLayer);
        this.m_serverRequest.Done += new Action<bool, MyLobbyStatusCode, MyMultiplayerBase>(MySession.OnMultiplayerHost);
      }
      else
        this.m_serverRequest = (MyMultiplayerHostResult) null;
    }

    private bool WaitForServerRequest(out MyLobbyStatusCode statusCode)
    {
      if (this.m_serverRequest != null)
      {
        this.m_serverRequest.Wait();
        statusCode = this.m_serverRequest.StatusCode;
        return this.m_serverRequest.Success;
      }
      statusCode = MyLobbyStatusCode.NoUser;
      return false;
    }

    private static MyLobbyType GetLobbyType(MyOnlineModeEnum onlineMode)
    {
      switch (onlineMode)
      {
        case MyOnlineModeEnum.PUBLIC:
          return MyLobbyType.Public;
        case MyOnlineModeEnum.FRIENDS:
          return MyLobbyType.FriendsOnly;
        case MyOnlineModeEnum.PRIVATE:
          return MyLobbyType.Private;
        default:
          return MyLobbyType.Private;
      }
    }

    private static void OnMultiplayerHost(
      bool success,
      MyLobbyStatusCode reason,
      MyMultiplayerBase multiplayer)
    {
      if (success)
        MySession.Static?.StartServer(multiplayer);
      else
        MySession.Static?.UnloadMultiplayer();
    }

    private void LoadWorld(MyObjectBuilder_Checkpoint checkpoint, MyObjectBuilder_Sector sector)
    {
      MySandboxGame.Static.SessionCompatHelper.FixSessionObjectBuilders(checkpoint, sector);
      Sandbox.Game.Entities.MyEntities.MemoryLimitAddFailureReset();
      this.ElapsedGameTime = new TimeSpan(checkpoint.ElapsedGameTime);
      this.InGameTime = checkpoint.InGameTime;
      this.Name = MyStatControlText.SubstituteTexts(checkpoint.SessionName);
      this.Description = checkpoint.Description;
      this.PromotedUsers = checkpoint.PromotedUsers != null ? checkpoint.PromotedUsers.Dictionary : new Dictionary<ulong, MyPromoteLevel>();
      this.CreativeTools = checkpoint.CreativeTools ?? new HashSet<ulong>();
      this.m_remoteAdminSettings.Clear();
      if (checkpoint?.AllPlayersData != null)
      {
        foreach (KeyValuePair<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player> keyValuePair in checkpoint.AllPlayersData.Dictionary)
        {
          ulong clientId = keyValuePair.Key.GetClientId();
          AdminSettingsEnum adminSettingsEnum = (AdminSettingsEnum) keyValuePair.Value.RemoteAdminSettings;
          int num;
          if (checkpoint.RemoteAdminSettings != null && checkpoint.RemoteAdminSettings.Dictionary.TryGetValue(clientId, out num))
            adminSettingsEnum = (AdminSettingsEnum) num;
          if (!MyPlatformGameSettings.IsIgnorePcuAllowed)
            adminSettingsEnum = adminSettingsEnum & ~AdminSettingsEnum.IgnorePcu & ~AdminSettingsEnum.KeepOriginalOwnershipOnPaste;
          this.m_remoteAdminSettings[clientId] = adminSettingsEnum;
          if (!Sync.IsDedicated && (long) clientId == (long) Sync.MyId)
            this.m_adminSettings = adminSettingsEnum;
          MyPromoteLevel myPromoteLevel;
          if (!this.PromotedUsers.TryGetValue(clientId, out myPromoteLevel))
            myPromoteLevel = MyPromoteLevel.None;
          if (keyValuePair.Value.PromoteLevel > myPromoteLevel)
            this.PromotedUsers[clientId] = keyValuePair.Value.PromoteLevel;
          if (!this.CreativeTools.Contains(clientId) && keyValuePair.Value.CreativeToolsEnabled)
            this.CreativeTools.Add(clientId);
        }
      }
      if (Sandbox.Engine.Platform.Game.IsDedicated)
      {
        foreach (KeyValuePair<ulong, MyPromoteLevel> keyValuePair in this.PromotedUsers.Where<KeyValuePair<ulong, MyPromoteLevel>>((Func<KeyValuePair<ulong, MyPromoteLevel>, bool>) (e => e.Value == MyPromoteLevel.Owner)).ToList<KeyValuePair<ulong, MyPromoteLevel>>())
          this.PromotedUsers.Remove(keyValuePair.Key);
        foreach (string administrator in MySandboxGame.ConfigDedicated.Administrators)
        {
          ulong result;
          if (ulong.TryParse(administrator, out result))
            this.PromotedUsers[result] = MyPromoteLevel.Owner;
        }
      }
      this.WorkshopId = checkpoint.WorkshopId;
      this.Password = checkpoint.Password;
      this.PreviousEnvironmentHostility = checkpoint.PreviousEnvironmentHostility;
      this.RequiresDX = checkpoint.RequiresDX;
      this.CustomLoadingScreenImage = checkpoint.CustomLoadingScreenImage;
      this.CustomLoadingScreenText = checkpoint.CustomLoadingScreenText;
      this.CustomSkybox = checkpoint.CustomSkybox;
      this.AppVersionFromSave = checkpoint.AppVersion;
      MyToolbarComponent.InitCharacterToolbar(checkpoint.CharacterToolbar);
      this.LoadCameraControllerSettings(checkpoint);
      this.TotalSessionPCU = 0;
      this.NPCBlockLimits = new MyBlockLimits(MySession.Static.PiratePCU, 0);
      this.GlobalBlockLimits = new MyBlockLimits(MySession.Static.TotalPCU, 0);
      this.SessionBlockLimits = new MyBlockLimits(MySession.Static.TotalPCU, 0);
      MyPlayer.PlayerId playerId = new MyPlayer.PlayerId();
      MyPlayer.PlayerId? savingPlayerId = new MyPlayer.PlayerId?();
      if (this.TryFindSavingPlayerId(checkpoint, out playerId) && (!this.IsScenario || MySession.Static.OnlineMode == MyOnlineModeEnum.OFFLINE))
        savingPlayerId = new MyPlayer.PlayerId?(playerId);
      if (Sync.IsServer || !this.IsScenario && MyPerGameSettings.Game == GameEnum.SE_GAME)
        Sync.Players.LoadIdentities(checkpoint, savingPlayerId);
      if (!this.IsScenario || !MySession.Static.Settings.StartInRespawnScreen)
        Sync.Players.LoadConnectedPlayers(checkpoint, savingPlayerId);
      else
        MySession.Static.Settings.StartInRespawnScreen = false;
      Sync.Players.RespawnComponent.InitFromCheckpoint(checkpoint);
      this.Toolbars.LoadToolbars(checkpoint);
      if (checkpoint.Factions != null && (Sync.IsServer || !this.IsScenario && MyPerGameSettings.Game == GameEnum.SE_GAME))
        MySession.Static.Factions.Init(checkpoint.Factions);
      if ((double) this.Settings.ProceduralDensity == 0.0)
      {
        if (MyVRage.Platform.System.IsMemoryLimited)
        {
          MyStorageBase.ResetCache();
          GC.Collect(GC.MaxGeneration);
        }
        MyVRage.Platform.System.OnSessionStarted(SessionType.ExtendedEntities);
      }
      else
        MyVRage.Platform.System.OnSessionStarted(SessionType.Normal);
      MyStringId? errorMessage;
      if (!Sandbox.Game.Entities.MyEntities.Load(sector.SectorObjects, out errorMessage))
      {
        this.ShowLoadingError(errorMessage: errorMessage);
      }
      else
      {
        if (!MyCampaignManager.Static.IsScenarioRunning)
        {
          if ((MyPlatformGameSettings.CONSOLE_COMPATIBLE || !MySandboxGame.Config.ExperimentalMode) && !Sandbox.Engine.Platform.Game.IsDedicated)
          {
            foreach (MyIdentity allIdentity in (IEnumerable<MyIdentity>) this.Players.GetAllIdentities())
            {
              if (!this.Players.IdentityIsNpc(allIdentity.IdentityId) && allIdentity.BlockLimits.IsOverLimits)
              {
                this.ShowLoadingError(errorMessage: new MyStringId?(!MySandboxGame.Config.ExperimentalMode ? MyCommonTexts.SaveGameErrorExperimental : MyCommonTexts.SaveGameErrorOverBlockLimits));
                return;
              }
            }
          }
          if (Sync.IsServer && !MyPlatformGameSettings.IsIgnorePcuAllowed && this.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.GLOBALLY)
          {
            // ISSUE: method pointer
            this.OnReady += new Action((object) this, __methodptr(\u003CLoadWorld\u003Eg__TestUncountedPCUs\u007C579_0));
          }
        }
        Parallel.RunCallbacks();
        MySandboxGame.Static.SessionCompatHelper.AfterEntitiesLoad(sector.AppVersion);
        MyGlobalEvents.LoadEvents(sector.SectorEvents);
        MySpectatorCameraController.Static.InitLight(checkpoint.SpectatorIsLightOn);
        if (Sync.IsServer)
        {
          MySpectatorCameraController.Static.SetViewMatrix(MatrixD.Invert(checkpoint.SpectatorPosition.GetMatrix()));
          MySpectatorCameraController.Static.SpeedModeLinear = checkpoint.SpectatorSpeed.X;
          MySpectatorCameraController.Static.SpeedModeAngular = checkpoint.SpectatorSpeed.Y;
        }
        if (!this.IsScenario || !MySession.Static.Settings.StartInRespawnScreen)
          Sync.Players.LoadControlledEntities(checkpoint.ControlledEntities, checkpoint.ControlledObject, savingPlayerId);
        this.LoadCamera(checkpoint);
        if (this.CreativeMode && !Sandbox.Engine.Platform.Game.IsDedicated && (this.LocalHumanPlayer != null && this.LocalHumanPlayer.Character != null) && this.LocalHumanPlayer.Character.IsDead)
          MyPlayerCollection.RequestLocalRespawn();
        if (Sandbox.Engine.Multiplayer.MyMultiplayer.Static != null)
          Sandbox.Engine.Multiplayer.MyMultiplayer.Static.OnSessionReady();
        if (!Sandbox.Engine.Platform.Game.IsDedicated && this.LocalHumanPlayer == null)
          Sync.Players.RequestNewPlayer(Sync.MyId, 0, MyGameService.UserName, (string) null, true, true);
        else if (this.ControlledEntity == null && Sync.IsServer && !Sandbox.Engine.Platform.Game.IsDedicated)
        {
          MyLog.Default.WriteLine("ControlledObject was null, respawning character");
          this.m_cameraAwaitingEntity = true;
          MyPlayerCollection.RequestLocalRespawn();
        }
        this.SharedToolbar = checkpoint.SharedToolbar;
        if (!Sandbox.Engine.Platform.Game.IsDedicated)
        {
          MyPlayer.PlayerId pid = new MyPlayer.PlayerId(Sync.MyId, 0);
          MyToolbar myToolbar = this.Toolbars.TryGetPlayerToolbar(pid);
          if (checkpoint.SharedToolbar != 0UL)
          {
            MyToolbar playerToolbar = this.Toolbars.TryGetPlayerToolbar(new MyPlayer.PlayerId(checkpoint.SharedToolbar, 0));
            if (playerToolbar != null)
              myToolbar = playerToolbar;
          }
          if (myToolbar == null)
          {
            MyToolBarCollection.RequestCreateToolbar(pid);
            MyToolbarComponent.InitCharacterToolbar(this.Scenario.DefaultToolbar);
          }
          else
            MyToolbarComponent.InitCharacterToolbar(myToolbar.GetObjectBuilder());
          this.GetComponent<MyRadialMenuComponent>().InitDefaultLastUsed(this.Scenario.DefaultToolbar);
        }
        this.Gpss.LoadGpss(checkpoint);
        MyRenderProxy.RebuildCullingStructure();
        this.Settings.ResetOwnership = false;
        if (!this.CreativeMode)
          MyDebugDrawSettings.DEBUG_DRAW_PHYSICS = false;
        if (!MySandboxGame.Config.SyncRendering)
          MyRenderProxy.WaitForFrame();
        MyRenderProxy.CollectGarbage();
      }
    }

    public int AppVersionFromSave { get; private set; }

    private bool TryFindSavingPlayerId(
      MyObjectBuilder_Checkpoint checkpoint,
      out MyPlayer.PlayerId playerId)
    {
      playerId = new MyPlayer.PlayerId();
      if (!MyFakes.REUSE_OLD_PLAYER_IDENTITY || !Sync.IsServer || (Sync.Clients.Count != 1 || Sandbox.Engine.Platform.Game.IsDedicated) || (checkpoint?.AllPlayersData == null || checkpoint.Identities == null))
        return false;
      bool flag = false;
      MyObjectBuilder_Identity objectBuilderIdentity = checkpoint.Identities.Find((Predicate<MyObjectBuilder_Identity>) (i => i.CharacterEntityId == checkpoint.ControlledObject));
      if (objectBuilderIdentity == null)
      {
        MyObjectBuilder_Checkpoint.PlayerId playerId1;
        if (checkpoint.ControlledEntities.Dictionary.TryGetValue(checkpoint.ControlledObject, out playerId1))
        {
          foreach (KeyValuePair<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player> keyValuePair in checkpoint.AllPlayersData.Dictionary)
          {
            MyObjectBuilder_Checkpoint.PlayerId key = keyValuePair.Key;
            if ((long) key.GetClientId() == (long) playerId1.GetClientId() && keyValuePair.Key.SerialId == playerId1.SerialId)
              playerId = new MyPlayer.PlayerId(playerId1.GetClientId(), playerId1.SerialId);
            key = keyValuePair.Key;
            if ((long) key.GetClientId() == (long) Sync.MyId && keyValuePair.Key.SerialId == 0)
              flag = true;
          }
        }
        return !flag;
      }
      foreach (KeyValuePair<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player> keyValuePair in checkpoint.AllPlayersData.Dictionary)
      {
        MyObjectBuilder_Checkpoint.PlayerId key;
        if (keyValuePair.Value.IdentityId == objectBuilderIdentity.IdentityId)
        {
          ref MyPlayer.PlayerId local = ref playerId;
          key = keyValuePair.Key;
          MyPlayer.PlayerId playerId1 = new MyPlayer.PlayerId(key.GetClientId(), keyValuePair.Key.SerialId);
          local = playerId1;
        }
        key = keyValuePair.Key;
        if ((long) key.GetClientId() == (long) Sync.MyId && keyValuePair.Key.SerialId == 0)
          flag = true;
      }
      return !flag;
    }

    private void LoadCamera(MyObjectBuilder_Checkpoint checkpoint)
    {
      if ((double) checkpoint.SpectatorDistance > 0.0)
      {
        MyThirdPersonSpectator.Static.UpdateAfterSimulation();
        MyThirdPersonSpectator.Static.ResetViewerDistance(new double?((double) checkpoint.SpectatorDistance));
      }
      MySandboxGame.Log.WriteLine("Checkpoint.CameraAttachedTo: " + (object) checkpoint.CameraEntity);
      MyCameraControllerEnum cameraControllerEnum = checkpoint.CameraController;
      if (!MySession.Static.Enable3RdPersonView && cameraControllerEnum == MyCameraControllerEnum.ThirdPersonSpectator)
        cameraControllerEnum = checkpoint.CameraController = MyCameraControllerEnum.Entity;
      IMyEntity cameraEntity;
      if (checkpoint.CameraEntity == 0L && this.ControlledEntity != null)
      {
        cameraEntity = (IMyEntity) (this.ControlledEntity as MyEntity);
        if (cameraEntity != null)
        {
          if (this.ControlledEntity is MyRemoteControl controlledEntity)
            cameraEntity = (IMyEntity) controlledEntity.Pilot;
          else if (!(this.ControlledEntity is IMyCameraController))
          {
            cameraEntity = (IMyEntity) null;
            cameraControllerEnum = MyCameraControllerEnum.Spectator;
          }
        }
      }
      else if (!Sandbox.Game.Entities.MyEntities.EntityExists(checkpoint.CameraEntity))
      {
        cameraEntity = (IMyEntity) (this.ControlledEntity as MyEntity);
        if (cameraEntity != null)
        {
          cameraControllerEnum = MyCameraControllerEnum.Entity;
          if (!(this.ControlledEntity is IMyCameraController))
          {
            cameraEntity = (IMyEntity) null;
            cameraControllerEnum = MyCameraControllerEnum.Spectator;
          }
        }
        else
        {
          MyLog.Default.WriteLine("ERROR: Camera entity from checkpoint does not exists!");
          cameraControllerEnum = MyCameraControllerEnum.Spectator;
        }
      }
      else
        cameraEntity = (IMyEntity) Sandbox.Game.Entities.MyEntities.GetEntityById(checkpoint.CameraEntity);
      if (cameraControllerEnum == MyCameraControllerEnum.Spectator && cameraEntity != null)
        cameraControllerEnum = MyCameraControllerEnum.Entity;
      MyEntityCameraSettings cameraSettings = (MyEntityCameraSettings) null;
      bool flag = false;
      if (!Sandbox.Engine.Platform.Game.IsDedicated && (cameraControllerEnum == MyCameraControllerEnum.Entity || cameraControllerEnum == MyCameraControllerEnum.ThirdPersonSpectator) && cameraEntity != null && (MySession.Static.Cameras.TryGetCameraSettings(this.LocalHumanPlayer == null ? new MyPlayer.PlayerId(Sync.MyId, 0) : this.LocalHumanPlayer.Id, cameraEntity.EntityId, cameraEntity is MyCharacter && this.LocalCharacter == cameraEntity, out cameraSettings) && !cameraSettings.IsFirstPerson))
      {
        cameraControllerEnum = MyCameraControllerEnum.ThirdPersonSpectator;
        flag = true;
      }
      MySession.Static.IsCameraAwaitingEntity = false;
      this.SetCameraController(cameraControllerEnum, cameraEntity, new Vector3D?());
      if (!flag)
        return;
      MyThirdPersonSpectator.Static.ResetViewerAngle(cameraSettings.HeadAngle);
      MyThirdPersonSpectator.Static.ResetViewerDistance(new double?(cameraSettings.Distance));
    }

    private void LoadCameraControllerSettings(MyObjectBuilder_Checkpoint checkpoint) => this.Cameras.LoadCameraCollection(checkpoint);

    internal static void FixIncorrectSettings(MyObjectBuilder_SessionSettings settings)
    {
      MyObjectBuilder_SessionSettings newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_SessionSettings>();
      int num1 = settings.EnableWolfs ? 1 : 0;
      int num2 = settings.EnableSpiders ? 1 : 0;
      if ((double) settings.RefinerySpeedMultiplier <= 0.0)
        settings.RefinerySpeedMultiplier = newObject.RefinerySpeedMultiplier;
      if ((double) settings.AssemblerSpeedMultiplier <= 0.0)
        settings.AssemblerSpeedMultiplier = newObject.AssemblerSpeedMultiplier;
      if ((double) settings.AssemblerEfficiencyMultiplier <= 0.0)
        settings.AssemblerEfficiencyMultiplier = newObject.AssemblerEfficiencyMultiplier;
      if ((double) settings.InventorySizeMultiplier <= 0.0)
        settings.InventorySizeMultiplier = newObject.InventorySizeMultiplier;
      if ((double) settings.WelderSpeedMultiplier <= 0.0)
        settings.WelderSpeedMultiplier = newObject.WelderSpeedMultiplier;
      if ((double) settings.GrinderSpeedMultiplier <= 0.0)
        settings.GrinderSpeedMultiplier = newObject.GrinderSpeedMultiplier;
      if ((double) settings.HackSpeedMultiplier <= 0.0)
        settings.HackSpeedMultiplier = newObject.HackSpeedMultiplier;
      if (!settings.PermanentDeath.HasValue)
        settings.PermanentDeath = new bool?(true);
      settings.ViewDistance = MathHelper.Clamp(settings.ViewDistance, 1000, 50000);
      settings.SyncDistance = MathHelper.Clamp(settings.SyncDistance, 1000, 20000);
      if (Sandbox.Engine.Platform.Game.IsDedicated)
      {
        settings.Scenario = false;
        settings.ScenarioEditMode = false;
      }
      if (MySession.Static != null && MySession.Static.Scenario != null)
        settings.WorldSizeKm = MySession.Static.Scenario.HasPlanets ? 0 : settings.WorldSizeKm;
      if (MySession.Static == null || MySession.Static.WorldBoundaries.HasValue || settings.WorldSizeKm <= 0)
        return;
      double num3 = (double) (settings.WorldSizeKm * 500);
      if (num3 <= 0.0)
        return;
      MySession.Static.WorldBoundaries = new BoundingBoxD?(new BoundingBoxD(new Vector3D(-num3, -num3, -num3), new Vector3D(num3, num3, num3)));
    }

    private void ShowLoadingError(
      bool lobbyFailed = false,
      MyLobbyStatusCode statusCode = MyLobbyStatusCode.Error,
      MyStringId? errorMessage = null)
    {
      StringBuilder stringBuilder;
      if (lobbyFailed)
      {
        stringBuilder = MyJoinGameHelper.GetErrorMessage(statusCode);
        MyLog.Default.WriteLine("ShowLoadingError: " + (object) statusCode + " / " + (object) stringBuilder);
      }
      else
        stringBuilder = !Sandbox.Game.Entities.MyEntities.MemoryLimitAddFailure ? MyTexts.Get(errorMessage ?? MyCommonTexts.MessageBoxTextErrorLoadingEntities) : MyTexts.Get(MyCommonTexts.MessageBoxTextMemoryLimitReachedDuringLoad);
      throw new MyLoadingException(stringBuilder.ToString());
    }

    internal void FixMissingCharacter()
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      int num = this.ControlledEntity == null ? 0 : (this.ControlledEntity is MyCockpit ? 1 : 0);
      bool flag1 = Sandbox.Game.Entities.MyEntities.GetEntities().OfType<MyCharacter>().Any<MyCharacter>();
      bool flag2 = this.ControlledEntity != null && this.ControlledEntity is MyRemoteControl && (this.ControlledEntity as MyRemoteControl).WasControllingCockpitWhenSaved();
      bool flag3 = this.ControlledEntity != null && this.ControlledEntity is MyLargeTurretBase && (this.ControlledEntity as MyLargeTurretBase).WasControllingCockpitWhenSaved();
      if (num != 0 || flag1 || (flag2 || flag3))
        return;
      MyPlayerCollection.RequestLocalRespawn();
    }

    public MyCameraControllerEnum GetCameraControllerEnum()
    {
      if (this.CameraController == MySpectatorCameraController.Static)
      {
        switch (MySpectatorCameraController.Static.SpectatorCameraMovement)
        {
          case MySpectatorCameraMovementEnum.UserControlled:
            return MyCameraControllerEnum.Spectator;
          case MySpectatorCameraMovementEnum.ConstantDelta:
            return MyCameraControllerEnum.SpectatorDelta;
          case MySpectatorCameraMovementEnum.None:
            return MyCameraControllerEnum.SpectatorFixed;
          case MySpectatorCameraMovementEnum.Orbit:
            return MyCameraControllerEnum.SpectatorOrbit;
        }
      }
      else
      {
        if (this.CameraController == MyThirdPersonSpectator.Static)
          return MyCameraControllerEnum.ThirdPersonSpectator;
        if (this.CameraController is MyEntity || this.CameraController is MyEntityRespawnComponentBase)
          return !this.CameraController.IsInFirstPersonView && !this.CameraController.ForceFirstPersonCamera || !this.CameraController.EnableFirstPersonView ? MyCameraControllerEnum.ThirdPersonSpectator : MyCameraControllerEnum.Entity;
      }
      return MyCameraControllerEnum.Spectator;
    }

    [Event(null, 3147)]
    [Client]
    [Reliable]
    public static void SetSpectatorPositionFromServer(Vector3D position) => MySession.Static.SetCameraController(MyCameraControllerEnum.Spectator, (IMyEntity) null, new Vector3D?(position));

    public void SetCameraController(
      MyCameraControllerEnum cameraControllerEnum,
      IMyEntity cameraEntity = null,
      Vector3D? position = null)
    {
      if (cameraEntity != null && this.Spectator.Position == Vector3.Zero)
      {
        MySpectatorCameraController spectator1 = this.Spectator;
        Vector3D position1 = cameraEntity.GetPosition();
        MatrixD matrixD = cameraEntity.WorldMatrix;
        Vector3D vector3D1 = matrixD.Forward * 4.0;
        Vector3D vector3D2 = position1 + vector3D1;
        matrixD = cameraEntity.WorldMatrix;
        Vector3D vector3D3 = matrixD.Up * 2.0;
        Vector3D vector3D4 = vector3D2 + vector3D3;
        spectator1.Position = vector3D4;
        MySpectatorCameraController spectator2 = this.Spectator;
        Vector3D position2 = cameraEntity.GetPosition();
        matrixD = cameraEntity.PositionComp.WorldMatrixRef;
        Vector3D? up = new Vector3D?(matrixD.Up);
        spectator2.SetTarget(position2, up);
        this.Spectator.Initialized = true;
      }
      this.CameraOnCharacter = cameraEntity is MyCharacter;
      switch (cameraControllerEnum)
      {
        case MyCameraControllerEnum.Spectator:
          MySession.Static.CameraController = (IMyCameraController) MySpectatorCameraController.Static;
          if (MySession.Static.ControlledEntity != null && MySession.Static.ControlledEntity is MyCockpit)
            ((MyShipController) MySession.Static.ControlledEntity).RemoveControlNotifications();
          MySpectatorCameraController.Static.SpectatorCameraMovement = MySpectatorCameraMovementEnum.UserControlled;
          if (!position.HasValue)
            break;
          MySpectatorCameraController.Static.Position = position.Value;
          break;
        case MyCameraControllerEnum.Entity:
          if (cameraEntity is IMyCameraController)
          {
            MySession.Static.CameraController = (IMyCameraController) cameraEntity;
            break;
          }
          MyEntityRespawnComponentBase component;
          if (cameraEntity.Components.TryGet<MyEntityRespawnComponentBase>(out component))
          {
            MySession.Static.CameraController = (IMyCameraController) component;
            break;
          }
          MySession.Static.CameraController = (IMyCameraController) this.LocalCharacter;
          break;
        case MyCameraControllerEnum.ThirdPersonSpectator:
          if (cameraEntity != null)
            MySession.Static.CameraController = (IMyCameraController) cameraEntity;
          MySession.Static.CameraController.IsInFirstPersonView = false;
          break;
        case MyCameraControllerEnum.SpectatorDelta:
          MySession.Static.CameraController = (IMyCameraController) MySpectatorCameraController.Static;
          MySpectatorCameraController.Static.SpectatorCameraMovement = MySpectatorCameraMovementEnum.ConstantDelta;
          if (!position.HasValue)
            break;
          MySpectatorCameraController.Static.Position = position.Value;
          break;
        case MyCameraControllerEnum.SpectatorFixed:
          MySession.Static.CameraController = (IMyCameraController) MySpectatorCameraController.Static;
          MySpectatorCameraController.Static.SpectatorCameraMovement = MySpectatorCameraMovementEnum.None;
          if (!position.HasValue)
            break;
          MySpectatorCameraController.Static.Position = position.Value;
          break;
        case MyCameraControllerEnum.SpectatorOrbit:
          MySession.Static.CameraController = (IMyCameraController) MySpectatorCameraController.Static;
          MySpectatorCameraController.Static.SpectatorCameraMovement = MySpectatorCameraMovementEnum.Orbit;
          if (!position.HasValue)
            break;
          MySpectatorCameraController.Static.Position = position.Value;
          break;
        case MyCameraControllerEnum.SpectatorFreeMouse:
          MySession.Static.CameraController = (IMyCameraController) MySpectatorCameraController.Static;
          MySpectatorCameraController.Static.SpectatorCameraMovement = MySpectatorCameraMovementEnum.FreeMouse;
          if (!position.HasValue)
            break;
          MySpectatorCameraController.Static.Position = position.Value;
          break;
      }
    }

    public void SetEntityCameraPosition(MyPlayer.PlayerId pid, IMyEntity cameraEntity)
    {
      if (this.LocalHumanPlayer == null || this.LocalHumanPlayer.Id != pid)
        return;
      MyEntityCameraSettings cameraSettings;
      if (this.Cameras.TryGetCameraSettings(pid, cameraEntity.EntityId, cameraEntity is MyCharacter && this.LocalCharacter == cameraEntity, out cameraSettings))
      {
        if (cameraSettings.IsFirstPerson)
          return;
        this.SetCameraController(MyCameraControllerEnum.ThirdPersonSpectator, cameraEntity, new Vector3D?());
        MyThirdPersonSpectator.Static.ResetViewerAngle(cameraSettings.HeadAngle);
        MyThirdPersonSpectator.Static.ResetViewerDistance(new double?(cameraSettings.Distance));
      }
      else
      {
        if (this.GetCameraControllerEnum() != MyCameraControllerEnum.ThirdPersonSpectator && (!(cameraEntity is Sandbox.ModAPI.IMyShipController) || !((Sandbox.ModAPI.IMyShipController) cameraEntity).IsDefault3rdView))
          return;
        this.SetCameraController(MyCameraControllerEnum.ThirdPersonSpectator, cameraEntity, new Vector3D?());
        MyThirdPersonSpectator.Static.RecalibrateCameraPosition(cameraEntity is MyCharacter);
        MyThirdPersonSpectator.Static.ResetSpring();
        MyThirdPersonSpectator.Static.UpdateZoom();
      }
    }

    public bool IsCameraControlledObject() => this.ControlledEntity == MySession.Static.CameraController;

    public bool IsCameraUserControlledSpectator()
    {
      if (MySpectatorCameraController.Static == null)
        return true;
      if (MySession.Static.CameraController != MySpectatorCameraController.Static)
        return false;
      return MySpectatorCameraController.Static.SpectatorCameraMovement == MySpectatorCameraMovementEnum.UserControlled || MySpectatorCameraController.Static.SpectatorCameraMovement == MySpectatorCameraMovementEnum.Orbit || MySpectatorCameraController.Static.SpectatorCameraMovement == MySpectatorCameraMovementEnum.FreeMouse;
    }

    public bool IsCameraUserAnySpectator()
    {
      if (MySpectatorCameraController.Static == null)
        return true;
      return MySession.Static.CameraController == MySpectatorCameraController.Static && MySpectatorCameraController.Static.SpectatorCameraMovement != MySpectatorCameraMovementEnum.None;
    }

    public float GetCameraTargetDistance() => (float) MyThirdPersonSpectator.Static.GetViewerDistance();

    public void SetCameraTargetDistance(double distance) => MyThirdPersonSpectator.Static.ResetViewerDistance(distance == 0.0 ? new double?() : new double?(distance));

    public void SaveControlledEntityCameraSettings(bool isFirstPerson)
    {
      if (this.ControlledEntity == null || this.LocalHumanPlayer == null || this.ControlledEntity is MyCharacter controlledEntity && controlledEntity.IsDead)
        return;
      this.Cameras.SaveEntityCameraSettings(this.LocalHumanPlayer.Id, this.ControlledEntity.Entity.EntityId, isFirstPerson, MyThirdPersonSpectator.Static.GetViewerDistance(), controlledEntity != null && this.LocalCharacter == this.ControlledEntity, this.ControlledEntity.HeadLocalXAngle, this.ControlledEntity.HeadLocalYAngle);
    }

    public bool Save(string customSaveName = null)
    {
      this.m_isSnapshotSaveInProgress = true;
      MySessionSnapshot snapshot;
      if (!this.Save(out snapshot, customSaveName))
      {
        this.m_isSnapshotSaveInProgress = false;
        return false;
      }
      int num = snapshot.Save((Func<bool>) null, (string) null) ? 1 : 0;
      if (num != 0)
        this.WorldSizeInBytes = snapshot.SavedSizeInBytes;
      this.m_isSnapshotSaveInProgress = false;
      return num != 0;
    }

    public bool Save(out MySessionSnapshot snapshot, string customSaveName = null)
    {
      this.m_isSaveInProgress = true;
      if (Sync.IsServer)
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<bool>((Func<IMyEventOwner, Action<bool>>) (x => new Action<bool>(MySession.OnServerSaving)), true);
      snapshot = new MySessionSnapshot();
      MySandboxGame.Log.WriteLine("Saving world - START");
      using (MySandboxGame.Log.IndentUsing())
      {
        string saveName = customSaveName ?? this.Name;
        if (customSaveName != null)
        {
          if (!Path.IsPathRooted(customSaveName))
          {
            string directoryName = Path.GetDirectoryName(this.CurrentPath);
            this.CurrentPath = !Directory.Exists(directoryName) ? MyLocalCache.GetSessionSavesPath(customSaveName, false) : Path.Combine(directoryName, customSaveName);
          }
          else
          {
            this.CurrentPath = customSaveName;
            saveName = Path.GetFileName(customSaveName);
          }
        }
        snapshot.TargetDir = this.CurrentPath;
        snapshot.SavingDir = Path.Combine(snapshot.TargetDir, ".new");
        try
        {
          MySandboxGame.Log.WriteLine("Making world state snapshot.");
          this.LogMemoryUsage("Before snapshot.");
          snapshot.CheckpointSnapshot = this.GetCheckpoint(saveName, false);
          snapshot.SectorSnapshot = this.GetSector(true);
          snapshot.CompressedVoxelSnapshots = this.VoxelMaps.GetVoxelMapsData(true, true);
          snapshot.VicinityGatherTask = this.GatherVicinityInformation(snapshot.CheckpointSnapshot);
          Dictionary<string, VRage.Game.Voxels.IMyStorage> voxelStorageNameCache = new Dictionary<string, VRage.Game.Voxels.IMyStorage>();
          snapshot.VoxelSnapshots = this.VoxelMaps.GetVoxelMapsData(true, false, voxelStorageNameCache);
          snapshot.VoxelStorageNameCache = voxelStorageNameCache;
          this.LogMemoryUsage("After snapshot.");
          this.SaveDataComponents();
        }
        catch (Exception ex)
        {
          MySandboxGame.Log.WriteLine(ex);
          this.m_isSaveInProgress = false;
          return false;
        }
        finally
        {
          this.SaveEnded();
        }
        this.LogMemoryUsage("Directory cleanup");
      }
      MySandboxGame.Log.WriteLine("Saving world - END");
      this.m_isSaveInProgress = false;
      return true;
    }

    public void SaveEnded()
    {
      if (!Sync.IsServer)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<bool>((Func<IMyEventOwner, Action<bool>>) (x => new Action<bool>(MySession.OnServerSaving)), false);
    }

    public string ThumbPath => Path.Combine(this.CurrentPath, MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION);

    public void SaveDataComponents()
    {
      foreach (MySessionComponentBase component in this.m_sessionComponents.Values)
        this.SaveComponent(component);
    }

    private void SaveComponent(MySessionComponentBase component) => component.SaveData();

    public MyObjectBuilder_World GetWorld(
      bool includeEntities = true,
      bool isClientRequest = false)
    {
      return new MyObjectBuilder_World()
      {
        Checkpoint = this.GetCheckpoint(this.Name, isClientRequest),
        Sector = this.GetSector(includeEntities),
        VoxelMaps = includeEntities ? new SerializableDictionary<string, byte[]>(MySession.Static.GetVoxelMapsArray(false)) : new SerializableDictionary<string, byte[]>()
      };
    }

    public MyObjectBuilder_Sector GetSector(bool includeEntities = true)
    {
      MyObjectBuilder_Sector newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Sector>();
      if (includeEntities)
        newObject.SectorObjects = Sandbox.Game.Entities.MyEntities.Save();
      newObject.SectorEvents = MyGlobalEvents.GetObjectBuilder();
      newObject.Environment = MySector.GetEnvironmentSettings();
      newObject.AppVersion = (int) MyFinalBuildConstants.APP_VERSION;
      return newObject;
    }

    public MyObjectBuilder_Checkpoint GetCheckpoint(
      string saveName,
      bool isClientRequest = false)
    {
      MatrixD matrix = MatrixD.Invert(MySpectatorCameraController.Static.GetViewMatrix());
      MyCameraControllerEnum cameraControllerEnum = this.GetCameraControllerEnum();
      MyObjectBuilder_Checkpoint newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Checkpoint>();
      MyObjectBuilder_SessionSettings builderSessionSettings = MyObjectBuilderSerializer.Clone((MyObjectBuilder_Base) this.Settings) as MyObjectBuilder_SessionSettings;
      builderSessionSettings.ScenarioEditMode = builderSessionSettings.ScenarioEditMode || this.PersistentEditMode;
      newObject.SessionName = saveName;
      newObject.Description = this.Description;
      newObject.Password = this.Password;
      newObject.LastSaveTime = DateTime.Now;
      newObject.WorkshopId = this.WorkshopId;
      newObject.ElapsedGameTime = this.ElapsedGameTime.Ticks;
      newObject.InGameTime = this.InGameTime;
      newObject.Settings = builderSessionSettings;
      newObject.Mods = this.Mods;
      newObject.CharacterToolbar = MyToolbarComponent.CharacterToolbar.GetObjectBuilder();
      newObject.Scenario = (SerializableDefinitionId) this.Scenario.Id;
      MyObjectBuilder_Checkpoint builderCheckpoint = newObject;
      BoundingBoxD? worldBoundaries = this.WorldBoundaries;
      SerializableBoundingBoxD? nullable = worldBoundaries.HasValue ? new SerializableBoundingBoxD?((SerializableBoundingBoxD) worldBoundaries.GetValueOrDefault()) : new SerializableBoundingBoxD?();
      builderCheckpoint.WorldBoundaries = nullable;
      newObject.PreviousEnvironmentHostility = this.PreviousEnvironmentHostility;
      newObject.RequiresDX = this.RequiresDX;
      newObject.CustomLoadingScreenImage = this.CustomLoadingScreenImage;
      newObject.CustomLoadingScreenText = this.CustomLoadingScreenText;
      newObject.CustomSkybox = this.CustomSkybox;
      newObject.GameDefinition = (SerializableDefinitionId) this.GameDefinition.Id;
      newObject.SessionComponentDisabled = this.SessionComponentDisabled;
      newObject.SessionComponentEnabled = this.SessionComponentEnabled;
      newObject.SharedToolbar = this.SharedToolbar;
      newObject.PromotedUsers = (SerializableDictionary<ulong, MyPromoteLevel>) null;
      newObject.RemoteAdminSettings = (SerializableDictionary<ulong, int>) null;
      newObject.CreativeTools = (HashSet<ulong>) null;
      Sync.Players.SavePlayers(newObject, this.RemoteAdminSettings, this.PromotedUsers, this.CreativeTools);
      this.Toolbars.SaveToolbars(newObject);
      this.Cameras.SaveCameraCollection(newObject);
      this.Gpss.SaveGpss(newObject);
      newObject.Factions = !MyFakes.SHOW_FACTIONS_GUI ? (MyObjectBuilder_FactionCollection) null : this.Factions.GetObjectBuilder();
      newObject.Identities = Sync.Players.SaveIdentities();
      newObject.RespawnCooldowns = new List<MyObjectBuilder_Checkpoint.RespawnCooldownItem>();
      Sync.Players.RespawnComponent.SaveToCheckpoint(newObject);
      newObject.ControlledEntities = Sync.Players.SerializeControlledEntities();
      newObject.SpectatorPosition = new MyPositionAndOrientation(ref matrix);
      newObject.SpectatorSpeed = new SerializableVector2(MySpectatorCameraController.Static.SpeedModeLinear, MySpectatorCameraController.Static.SpeedModeAngular);
      newObject.SpectatorIsLightOn = MySpectatorCameraController.Static.IsLightOn;
      newObject.SpectatorDistance = (float) MyThirdPersonSpectator.Static.GetViewerDistance();
      newObject.CameraController = cameraControllerEnum;
      if (cameraControllerEnum == MyCameraControllerEnum.Entity)
        newObject.CameraEntity = ((MyEntity) this.CameraController).EntityId;
      if (this.ControlledEntity != null)
      {
        newObject.ControlledObject = this.ControlledEntity.Entity.EntityId;
        if (!(this.ControlledEntity is MyCharacter))
          ;
      }
      else
        newObject.ControlledObject = -1L;
      newObject.AppVersion = (int) MyFinalBuildConstants.APP_VERSION;
      if (isClientRequest)
        newObject.Clients = this.SaveMembers();
      newObject.NonPlayerIdentities = Sync.Players.SaveNpcIdentities();
      this.SaveSessionComponentObjectBuilders(newObject, isClientRequest);
      newObject.ScriptManagerData = this.ScriptManager.GetObjectBuilder();
      if (this.OnSavingCheckpoint != null)
        this.OnSavingCheckpoint(newObject);
      return newObject;
    }

    public static void RequestVicinityCache(long entityId) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (s => new Action<long>(MySession.OnRequestVicinityInformation)), entityId);

    [Event(null, 3597)]
    [Reliable]
    [Server]
    private static void OnRequestVicinityInformation(long entityId) => MySession.SendVicinityInformation(entityId, MyEventContext.Current.Sender);

    public static void SendVicinityInformation(long entityId, EndpointId client)
    {
      MyEntity entityById = Sandbox.Game.Entities.MyEntities.GetEntityById(entityId);
      if (entityById == null)
        return;
      BoundingSphereD bs = new BoundingSphereD(entityById.PositionComp.WorldMatrixRef.Translation, MyFakes.PRIORITIZED_CUBE_VICINITY_RADIUS);
      HashSet<string> voxelMaterials = new HashSet<string>();
      HashSet<string> models = new HashSet<string>();
      HashSet<string> armorModels = new HashSet<string>();
      MySession.Static.GatherVicinityInformation(bs, voxelMaterials, models, armorModels, (Action) (() => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<List<string>, List<string>, List<string>>((Func<IMyEventOwner, Action<List<string>, List<string>, List<string>>>) (s => new Action<List<string>, List<string>, List<string>>(MySession.OnVicinityInformation)), voxelMaterials.ToList<string>(), models.ToList<string>(), armorModels.ToList<string>(), client)));
    }

    [Event(null, 3620)]
    [Reliable]
    [Client]
    private static void OnVicinityInformation(
      List<string> voxels,
      List<string> models,
      List<string> armorModels)
    {
      MySession.PreloadVicinityCache(voxels, models, armorModels);
    }

    private static void PreloadVicinityCache(
      List<string> voxels,
      List<string> models,
      List<string> armorModels)
    {
      if (voxels != null && voxels.Count > 0)
      {
        byte[] materials = new byte[voxels.Count];
        int num = 0;
        foreach (string voxel in voxels)
        {
          if (voxel != null)
          {
            MyVoxelMaterialDefinition materialDefinition = MyDefinitionManager.Static.GetVoxelMaterialDefinition(voxel);
            materials[num++] = materialDefinition == null ? (byte) 0 : materialDefinition.Index;
          }
        }
        MyRenderProxy.PreloadVoxelMaterials(materials);
      }
      if (models != null && models.Count > 0)
        MyRenderProxy.PreloadModels(models, true);
      if (armorModels == null || armorModels.Count <= 0)
        return;
      MyRenderProxy.PreloadModels(armorModels, false);
    }

    private Task GatherVicinityInformation(MyObjectBuilder_Checkpoint checkpoint)
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated || !MyFakes.PRIORITIZED_VICINITY_ASSETS_LOADING)
        return new Task();
      if (checkpoint.VicinityArmorModelsCache == null)
        checkpoint.VicinityArmorModelsCache = new List<string>();
      else
        checkpoint.VicinityArmorModelsCache.Clear();
      if (checkpoint.VicinityModelsCache == null)
        checkpoint.VicinityModelsCache = new List<string>();
      else
        checkpoint.VicinityModelsCache.Clear();
      if (checkpoint.VicinityVoxelCache == null)
        checkpoint.VicinityVoxelCache = new List<string>();
      else
        checkpoint.VicinityVoxelCache.Clear();
      if (this.LocalCharacter == null)
        return new Task();
      BoundingSphereD bs = new BoundingSphereD(this.LocalCharacter.WorldMatrix.Translation, MyFakes.PRIORITIZED_CUBE_VICINITY_RADIUS);
      HashSet<string> voxelMaterials = new HashSet<string>();
      HashSet<string> models = new HashSet<string>();
      HashSet<string> armorModels = new HashSet<string>();
      return this.GatherVicinityInformation(bs, voxelMaterials, models, armorModels, (Action) (() =>
      {
        if (this.LocalCharacter.CurrentWeapon != null)
          checkpoint.VicinityArmorModelsCache.Add(this.LocalCharacter.CurrentWeapon.PhysicalItemDefinition.Model);
        MyCharacterDefinition result;
        MyDefinitionManager.Static.Characters.TryGetValue(this.LocalCharacter.ModelName, out result);
        if (!string.IsNullOrEmpty(result.Model))
          checkpoint.VicinityArmorModelsCache.Add(result.Model);
        checkpoint.VicinityArmorModelsCache.AddRange((IEnumerable<string>) armorModels);
        checkpoint.VicinityModelsCache.AddRange((IEnumerable<string>) models);
        checkpoint.VicinityVoxelCache.AddRange((IEnumerable<string>) voxelMaterials);
      }));
    }

    public Task GatherVicinityInformation(
      BoundingSphereD bs,
      HashSet<string> voxelMaterials,
      HashSet<string> models,
      HashSet<string> armorModels,
      Action completion)
    {
      List<MyEntity> result = new List<MyEntity>();
      MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref bs, result);
      List<MyVoxelBase> voxelMaps = (List<MyVoxelBase>) null;
      foreach (MyEntity myEntity in result)
      {
        if (myEntity is MyCubeGrid myCubeGrid)
        {
          if (myCubeGrid.RenderData != null && myCubeGrid.RenderData.Cells != null)
          {
            foreach (KeyValuePair<Vector3I, MyCubeGridRenderCell> cell in myCubeGrid.Render.RenderData.Cells)
            {
              if (cell.Value.CubeParts != null)
              {
                foreach (KeyValuePair<MyCubePart, ConcurrentDictionary<uint, bool>> cubePart in cell.Value.CubeParts)
                  this.AddAllModels(cubePart.Key.Model, armorModels);
              }
            }
          }
          foreach (MySlimBlock cubeBlock in myCubeGrid.CubeBlocks)
          {
            if (cubeBlock.FatBlock != null && cubeBlock.FatBlock.Model != null)
              this.AddAllModels(cubeBlock.FatBlock.Model, models);
          }
        }
        else if (myEntity is MyVoxelBase myVoxelBase && !(myVoxelBase is MyVoxelPhysics))
        {
          voxelMaps = voxelMaps ?? new List<MyVoxelBase>();
          voxelMaps.Add(myVoxelBase);
        }
      }
      if (voxelMaps != null)
        return Parallel.Start((IWork) new MySession.GatherVoxelMaterialsWork(voxelMaps, voxelMaterials, bs, completion));
      completion.InvokeIfNotNull();
      return new Task();
    }

    private static void GetVoxelMaterials(
      HashSet<string> voxelMaterials,
      MyVoxelBase voxel,
      int lod,
      Vector3D center,
      float radius)
    {
      MyShapeSphere myShapeSphere = new MyShapeSphere()
      {
        Center = center,
        Radius = radius
      };
      foreach (byte materialIndex in voxel.GetMaterialsInShape((MyShape) myShapeSphere, lod))
      {
        MyVoxelMaterialDefinition materialDefinition = MyDefinitionManager.Static.GetVoxelMaterialDefinition(materialIndex);
        if (materialDefinition != null)
          voxelMaterials.Add(materialDefinition.Id.SubtypeName);
      }
    }

    private void AddAllModels(MyModel model, HashSet<string> models)
    {
      if (string.IsNullOrEmpty(model.AssetName))
        return;
      models.Add(model.AssetName);
    }

    private void SaveSessionComponentObjectBuilders(
      MyObjectBuilder_Checkpoint checkpoint,
      bool isClientRequest)
    {
      checkpoint.SessionComponents = new List<MyObjectBuilder_SessionComponent>();
      foreach (MySessionComponentBase sessionComponentBase in this.m_sessionComponents.Values)
      {
        if (!isClientRequest || !sessionComponentBase.IsServerOnly)
        {
          MyObjectBuilder_SessionComponent objectBuilder = sessionComponentBase.GetObjectBuilder();
          if (objectBuilder != null)
            checkpoint.SessionComponents.Add(objectBuilder);
        }
      }
    }

    public Dictionary<string, byte[]> GetVoxelMapsArray(bool includeChanged) => this.VoxelMaps.GetVoxelMapsArray(includeChanged);

    public List<MyObjectBuilder_Planet> GetPlanetObjectBuilders()
    {
      List<MyObjectBuilder_Planet> objectBuilderPlanetList = new List<MyObjectBuilder_Planet>();
      foreach (MyVoxelBase instance in this.VoxelMaps.Instances)
      {
        if (instance is MyPlanet myPlanet)
          objectBuilderPlanetList.Add(myPlanet.GetObjectBuilder(false) as MyObjectBuilder_Planet);
      }
      return objectBuilderPlanetList;
    }

    internal List<MyObjectBuilder_Client> SaveMembers(bool forceSave = false)
    {
      if (Sandbox.Engine.Multiplayer.MyMultiplayer.Static == null)
        return (List<MyObjectBuilder_Client>) null;
      if (!forceSave && Sandbox.Engine.Multiplayer.MyMultiplayer.Static.Members.Count<ulong>() == 1)
      {
        using (IEnumerator<ulong> enumerator = Sandbox.Engine.Multiplayer.MyMultiplayer.Static.Members.GetEnumerator())
        {
          enumerator.MoveNext();
          if ((long) enumerator.Current == (long) Sync.MyId)
            return (List<MyObjectBuilder_Client>) null;
        }
      }
      List<MyObjectBuilder_Client> objectBuilderClientList = new List<MyObjectBuilder_Client>();
      foreach (ulong member in Sandbox.Engine.Multiplayer.MyMultiplayer.Static.Members)
        objectBuilderClientList.Add(new MyObjectBuilder_Client()
        {
          SteamId = member,
          Name = Sandbox.Engine.Multiplayer.MyMultiplayer.Static.GetMemberName(member),
          IsAdmin = MySession.Static.IsUserAdmin(member),
          ClientService = Sandbox.Engine.Multiplayer.MyMultiplayer.Static.GetMemberServiceName(member)
        });
      return objectBuilderClientList;
    }

    public void GameOver() => this.GameOver(new MyStringId?(MyCommonTexts.MP_YouHaveBeenKilled));

    public void GameOver(MyStringId? customMessage)
    {
    }

    public void Unload()
    {
      this.IsUnloading = true;
      MySession.OnUnloading.InvokeIfNotNull();
      MyGuiScreenProgress guiScreenProgress = new MyGuiScreenProgress(MyTexts.Get(MySpaceTexts.ProgressScreen_UnloadingWorld));
      MyScreenManager.AddScreenNow((MyGuiScreenBase) guiScreenProgress);
      guiScreenProgress.DrawPaused();
      try
      {
        bool finish;
        do
        {
          finish = Parallel.Scheduler.WaitForTasksToFinish(TimeSpan.FromMilliseconds(16.0));
          try
          {
            Parallel.RunCallbacks();
          }
          catch (Exception ex)
          {
            MyLog.Default.WriteLine("Exception occurred while unloading session");
            MyLog.Default.WriteLine(ex);
          }
          MyVRage.Platform.Update();
          MyGameService.Update();
        }
        while (!(finish & !MySandboxGame.Static.ProcessInvoke()));
        MyScreenManager.CloseAllScreensNowExcept((MyGuiScreenBase) MyScreenManager.GetFirstScreenOfType<MyGuiScreenLoading>() ?? (MyGuiScreenBase) MyScreenManager.GetFirstScreenOfType<MyGuiScreenProgress>(), true);
        MyGuiSandbox.Update(16);
        MyScreenManager.CloseAllScreensNowExcept((MyGuiScreenBase) MyScreenManager.GetFirstScreenOfType<MyGuiScreenLoading>() ?? (MyGuiScreenBase) MyScreenManager.GetFirstScreenOfType<MyGuiScreenProgress>(), true);
        MySandboxGame.IsPaused = false;
        if (MyHud.RotatingWheelVisible)
          MyHud.PopRotatingWheelVisible();
        Sandbox.Engine.Platform.Game.EnableSimSpeedLocking = false;
        MySpectatorCameraController.Static.CleanLight();
        if (MySpaceAnalytics.Instance != null)
          MySpaceAnalytics.Instance.ReportWorldEnd();
        MySandboxGame.Log.WriteLine("MySession::Unload START");
        MySessionSnapshot.WaitForSaving();
        MySandboxGame.Log.WriteLine("AutoSaveInMinutes: " + (object) this.AutoSaveInMinutes);
        MySandboxGame.Log.WriteLine("MySandboxGame.IsDedicated: " + Sandbox.Engine.Platform.Game.IsDedicated.ToString());
        MySandboxGame.Log.WriteLine("IsServer: " + Sync.IsServer.ToString());
        bool? onUnloadOverride = this.SaveOnUnloadOverride;
        if (onUnloadOverride.HasValue)
        {
          onUnloadOverride = this.SaveOnUnloadOverride;
          if (onUnloadOverride.Value)
            goto label_12;
        }
        onUnloadOverride = this.SaveOnUnloadOverride;
        if (onUnloadOverride.HasValue || this.AutoSaveInMinutes <= 0U || !Sandbox.Engine.Platform.Game.IsDedicated)
          goto label_13;
label_12:
        MySandboxGame.Log.WriteLineAndConsole("Autosave in unload");
        this.Save((string) null);
label_13:
        MySandboxGame.Static.ClearInvokeQueue();
        MyDroneAIDataStatic.Reset();
        MyAudio.Static.StopUpdatingAll3DCues();
        MyAudio.Static.Mute = true;
        MyAudio.Static.StopMusic();
        MyAudio.Static.ChangeGlobalVolume(1f, 0.0f);
        MyAudio.ReloadData(MyAudioExtensions.GetSoundDataFromDefinitions(), MyAudioExtensions.GetEffectData());
        MyEntity3DSoundEmitter.LastTimePlaying.Clear();
        MyEntity3DSoundEmitter.ClearEntityEmitters();
        this.Ready = false;
        this.VoxelMaps.Clear();
        MySandboxGame.Config.Save();
        if (this.LocalHumanPlayer != null && this.LocalHumanPlayer.Controller != null)
          this.LocalHumanPlayer.Controller.SaveCamera();
        this.DisconnectMultiplayer();
        this.UnloadDataComponents(false);
        this.UnloadMultiplayer();
        MyTerminalControlFactory.Unload();
        MyDefinitionManager.Static.UnloadData();
        MyDefinitionManager.Static.PreloadDefinitions();
        if (!Sync.IsDedicated)
          MyGuiManager.InitFonts();
        MyInput.Static.ClearBlacklist();
        if (!Sandbox.Engine.Platform.Game.IsDedicated && EmptyKeys.UserInterface.Engine.Instance.AssetManager is MyAssetManager assetManager)
          assetManager.UnloadGeneratedTextures();
        MyDefinitionErrors.Clear();
        MyResourceDistributorComponent.Clear();
        MyRenderProxy.UnloadData();
        MyHud.Questlog.CleanDetails();
        MyHud.Questlog.Visible = false;
        MyAPIGateway.Clean();
        MyOxygenProviderSystem.ClearOxygenGenerators();
        MyDynamicAABBTree.Dispose();
        MyDynamicAABBTreeD.Dispose();
        MyParticleEffectsLibrary.Close();
        this.Factions.Clear();
        if (MyVRage.Platform.System.IsMemoryLimited)
          MyModels.UnloadModdedModels();
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        MySandboxGame.Log.WriteLine("MySession::Unload END");
        if (MyCubeBuilder.AllPlayersColors != null)
          MyCubeBuilder.AllPlayersColors.Clear();
        MySession.OnUnloaded.InvokeIfNotNull();
        MyVRage.Platform.System.OnSessionUnloaded();
        Parallel.Scheduler.WaitForTasksToFinish(new TimeSpan(-1L));
        Parallel.Clean();
      }
      finally
      {
        guiScreenProgress.CloseScreen();
      }
      this.IsUnloading = false;
    }

    private void InitializeFactions() => this.Factions.CreateDefaultFactions();

    public static void InitiateDump()
    {
      VRage.Profiler.MyRenderProfiler.SetLevel(-1);
      MySession.m_profilerDumpDelay = 60;
    }

    private static ulong GetVoxelsSizeInBytes(string sessionPath)
    {
      ulong num = 0;
      foreach (string file in Directory.GetFiles(sessionPath, "*.vx2", SearchOption.TopDirectoryOnly))
      {
        FileInfo fileInfo = new FileInfo(file);
        num += (ulong) fileInfo.Length;
      }
      return num;
    }

    private void LogMemoryUsage(string msg) => MySandboxGame.Log.WriteMemoryUsage(msg);

    private void LogSettings(string scenario = null, int asteroidAmount = 0)
    {
      MyLog log = MySandboxGame.Log;
      log.WriteLine("MySession.Static.LogSettings - START", LoggingOptions.SESSION_SETTINGS);
      using (log.IndentUsing(LoggingOptions.SESSION_SETTINGS))
      {
        log.WriteLine("Name = " + this.Name, LoggingOptions.SESSION_SETTINGS);
        log.WriteLine("Description = " + this.Description, LoggingOptions.SESSION_SETTINGS);
        log.WriteLine("GameDateTime = " + (object) this.GameDateTime, LoggingOptions.SESSION_SETTINGS);
        if (scenario != null)
        {
          log.WriteLine("Scenario = " + scenario, LoggingOptions.SESSION_SETTINGS);
          log.WriteLine("AsteroidAmount = " + (object) asteroidAmount, LoggingOptions.SESSION_SETTINGS);
        }
        log.WriteLine("Password = " + this.Password, LoggingOptions.SESSION_SETTINGS);
        log.WriteLine("CurrentPath = " + this.CurrentPath, LoggingOptions.SESSION_SETTINGS);
        log.WriteLine("WorkshopId = " + (object) this.WorkshopId, LoggingOptions.SESSION_SETTINGS);
        log.WriteLine("CameraController = " + (object) this.CameraController, LoggingOptions.SESSION_SETTINGS);
        log.WriteLine("ThumbPath = " + this.ThumbPath, LoggingOptions.SESSION_SETTINGS);
        this.Settings.LogMembers(log, LoggingOptions.SESSION_SETTINGS);
      }
      log.WriteLine("MySession.Static.LogSettings - END", LoggingOptions.SESSION_SETTINGS);
    }

    public bool MultiplayerAlive { get; set; }

    public bool MultiplayerDirect { get; set; }

    public double MultiplayerLastMsg { get; set; }

    public MyTimeSpan MultiplayerPing { get; set; }

    public bool HighSimulationQuality { get; private set; } = true;

    public bool HighSimulationQualityNotification
    {
      get
      {
        if (!this.Settings.AdaptiveSimulationQuality || Sync.IsServer && this.HighSimulationQuality || this.GameplayFrameCounter - this.m_lastQualitySwitchFrame < 300)
          return true;
        return !Sync.IsServer && (double) Sync.ServerCPULoadSmooth <= 90.0;
      }
    }

    public bool GetVoxelHandAvailable(MyCharacter character)
    {
      MyPlayer playerFromCharacter = MyPlayer.GetPlayerFromCharacter(character);
      return playerFromCharacter != null && this.GetVoxelHandAvailable(playerFromCharacter.Client.SteamUserId);
    }

    public bool GetVoxelHandAvailable(ulong user)
    {
      if (!this.Settings.EnableVoxelHand)
        return false;
      return !this.SurvivalMode || this.CreativeToolsEnabled(user);
    }

    private void OnFactionsStateChanged(
      MyFactionStateChange change,
      long fromFactionId,
      long toFactionId,
      long playerId,
      long sender)
    {
      string messageText = (string) null;
      if (change == MyFactionStateChange.FactionMemberKick && sender != playerId && this.LocalPlayerId == playerId)
        messageText = MyTexts.GetString(MyCommonTexts.MessageBoxTextYouHaveBeenKickedFromFaction);
      else if (change == MyFactionStateChange.FactionMemberAcceptJoin && sender != playerId && this.LocalPlayerId == playerId)
        messageText = MyTexts.GetString(MyCommonTexts.MessageBoxTextYouHaveBeenAcceptedToFaction);
      else if (change == MyFactionStateChange.FactionMemberNotPossibleJoin && sender != playerId && this.LocalPlayerId == playerId)
        messageText = MyTexts.GetString(MyCommonTexts.MessageBoxTextYouCannotJoinToFaction);
      else if (change == MyFactionStateChange.FactionMemberNotPossibleJoin && this.LocalPlayerId == sender)
        messageText = MyTexts.GetString(MyCommonTexts.MessageBoxTextApplicantCannotJoinToFaction);
      else if (change == MyFactionStateChange.FactionMemberAcceptJoin && (MySession.Static.Factions[toFactionId].IsFounder(this.LocalPlayerId) || MySession.Static.Factions[toFactionId].IsLeader(this.LocalPlayerId)) && playerId != 0L)
      {
        MyIdentity identity = Sync.Players.TryGetIdentity(playerId);
        if (identity != null)
        {
          string displayName = identity.DisplayName;
          messageText = string.Format(MyTexts.GetString(MyCommonTexts.Faction_PlayerJoined), (object) displayName);
        }
      }
      else if (change == MyFactionStateChange.FactionMemberLeave && (MySession.Static.Factions[toFactionId].IsFounder(this.LocalPlayerId) || MySession.Static.Factions[toFactionId].IsLeader(this.LocalPlayerId)) && playerId != 0L)
      {
        MyIdentity identity = Sync.Players.TryGetIdentity(playerId);
        if (identity != null)
        {
          string displayName = identity.DisplayName;
          messageText = string.Format(MyTexts.GetString(MyCommonTexts.Faction_PlayerLeft), (object) displayName);
        }
      }
      else if (change == MyFactionStateChange.FactionMemberSendJoin && (MySession.Static.Factions[toFactionId].IsFounder(this.LocalPlayerId) || MySession.Static.Factions[toFactionId].IsLeader(this.LocalPlayerId)) && playerId != 0L)
      {
        MyIdentity identity = Sync.Players.TryGetIdentity(playerId);
        if (identity != null)
        {
          string displayName = identity.DisplayName;
          messageText = string.Format(MyTexts.GetString(MyCommonTexts.Faction_PlayerApplied), (object) displayName);
        }
      }
      if (messageText == null)
        return;
      MyHud.Chat.ShowMessage(MyTexts.GetString(MySpaceTexts.ChatBotName), messageText);
    }

    private static IMyCamera GetMainCamera() => (IMyCamera) MySector.MainCamera;

    private static BoundingBoxD GetWorldBoundaries() => MySession.Static == null || !MySession.Static.WorldBoundaries.HasValue ? new BoundingBoxD() : MySession.Static.WorldBoundaries.Value;

    private static Vector3D GetLocalPlayerPosition() => MySession.Static != null && MySession.Static.LocalHumanPlayer != null ? MySession.Static.LocalHumanPlayer.GetPosition() : new Vector3D();

    public short GetBlockTypeLimit(string blockType)
    {
      int num1 = 1;
      switch (this.BlockLimitsEnabled)
      {
        case MyBlockLimitsEnabledEnum.NONE:
          return 0;
        case MyBlockLimitsEnabledEnum.GLOBALLY:
          num1 = 1;
          break;
        case MyBlockLimitsEnabledEnum.PER_FACTION:
          num1 = this.MaxFactionsCount != 0 ? 1 : 1;
          break;
        case MyBlockLimitsEnabledEnum.PER_PLAYER:
          num1 = 1;
          break;
      }
      short num2;
      if (!this.BlockTypeLimits.TryGetValue(blockType, out num2))
        return 0;
      return num2 > (short) 0 && (int) num2 / num1 == 0 ? (short) 1 : (short) ((int) num2 / num1);
    }

    private static void RaiseAfterLoading()
    {
      Action afterLoading = MySession.AfterLoading;
      if (afterLoading == null)
        return;
      afterLoading();
    }

    public MySession.LimitResult IsWithinWorldLimits(
      out string failedBlockType,
      long ownerID,
      string blockName,
      int pcuToBuild,
      int blocksToBuild = 0,
      int blocksCount = 0,
      Dictionary<string, int> blocksPerType = null)
    {
      failedBlockType = (string) null;
      if (this.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.NONE)
        return MySession.LimitResult.Passed;
      ulong steamId = this.Players.TryGetSteamId(ownerID);
      if (steamId != 0UL && MySession.Static.IsUserAdmin(steamId))
      {
        AdminSettingsEnum? nullable1 = new AdminSettingsEnum?();
        if ((long) steamId == (long) Sync.MyId)
          nullable1 = new AdminSettingsEnum?(MySession.Static.AdminSettings);
        else if (MySession.Static.RemoteAdminSettings.ContainsKey(steamId))
          nullable1 = new AdminSettingsEnum?(MySession.Static.RemoteAdminSettings[steamId]);
        AdminSettingsEnum? nullable2 = nullable1;
        AdminSettingsEnum? nullable3 = nullable2.HasValue ? new AdminSettingsEnum?(nullable2.GetValueOrDefault() & AdminSettingsEnum.IgnorePcu) : new AdminSettingsEnum?();
        AdminSettingsEnum adminSettingsEnum = AdminSettingsEnum.None;
        if (!(nullable3.GetValueOrDefault() == adminSettingsEnum & nullable3.HasValue))
          return MySession.LimitResult.Passed;
      }
      MyIdentity identity = this.Players.TryGetIdentity(ownerID);
      if (this.MaxGridSize != 0 && blocksCount + blocksToBuild > this.MaxGridSize)
        return MySession.LimitResult.MaxGridSize;
      if (identity != null)
      {
        MyBlockLimits blockLimits = identity.BlockLimits;
        if (this.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.PER_FACTION && this.Factions.GetPlayerFaction(identity.IdentityId) == null)
          return MySession.LimitResult.NoFaction;
        if (blockLimits != null)
        {
          if (this.MaxBlocksPerPlayer != 0 && blockLimits.BlocksBuilt + blocksToBuild > blockLimits.MaxBlocks)
            return MySession.LimitResult.MaxBlocksPerPlayer;
          if (this.TotalPCU != 0 && pcuToBuild > blockLimits.PCU)
            return MySession.LimitResult.PCU;
          if (blocksPerType != null)
          {
            foreach (KeyValuePair<string, short> blockTypeLimit in this.BlockTypeLimits)
            {
              if (blocksPerType.ContainsKey(blockTypeLimit.Key))
              {
                int num = blocksPerType[blockTypeLimit.Key];
                MyBlockLimits.MyTypeLimitData myTypeLimitData;
                if (blockLimits.BlockTypeBuilt.TryGetValue(blockTypeLimit.Key, out myTypeLimitData))
                  num += myTypeLimitData.BlocksBuilt;
                if (num > (int) this.GetBlockTypeLimit(blockTypeLimit.Key))
                  return MySession.LimitResult.BlockTypeLimit;
              }
            }
          }
          else if (blockName != null)
          {
            short blockTypeLimit = this.GetBlockTypeLimit(blockName);
            if (blockTypeLimit > (short) 0)
            {
              MyBlockLimits.MyTypeLimitData myTypeLimitData;
              if (blockLimits.BlockTypeBuilt.TryGetValue(blockName, out myTypeLimitData))
                blocksToBuild += myTypeLimitData.BlocksBuilt;
              if (blocksToBuild > (int) blockTypeLimit)
                return MySession.LimitResult.BlockTypeLimit;
            }
          }
        }
      }
      return MySession.LimitResult.Passed;
    }

    public bool CheckLimitsAndNotify(
      long ownerID,
      string blockName,
      int pcuToBuild,
      int blocksToBuild = 0,
      int blocksCount = 0,
      Dictionary<string, int> blocksPerType = null)
    {
      MySession.LimitResult result = this.IsWithinWorldLimits(out string _, ownerID, blockName, pcuToBuild, blocksToBuild, blocksCount, blocksPerType);
      if (result == MySession.LimitResult.Passed)
        return true;
      MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
      MyHud.Notifications.Add(MySession.GetNotificationForLimitResult(result));
      return false;
    }

    public static MyNotificationSingletons GetNotificationForLimitResult(
      MySession.LimitResult result)
    {
      switch (result)
      {
        case MySession.LimitResult.MaxGridSize:
          return MyNotificationSingletons.LimitsGridSize;
        case MySession.LimitResult.NoFaction:
          return MyNotificationSingletons.LimitsNoFaction;
        case MySession.LimitResult.BlockTypeLimit:
          return MyNotificationSingletons.LimitsPerBlockType;
        case MySession.LimitResult.MaxBlocksPerPlayer:
          return MyNotificationSingletons.LimitsPlayer;
        case MySession.LimitResult.PCU:
          return MyNotificationSingletons.LimitsPCU;
        default:
          return MyNotificationSingletons.LimitsPCU;
      }
    }

    public bool CheckResearchAndNotify(long identityId, MyDefinitionId id)
    {
      if (!MySession.Static.Settings.EnableResearch || MySessionComponentResearch.Static.CanUse(identityId, id) || (MySession.Static.CreativeMode || MySession.Static.CreativeToolsEnabled(MySession.Static.Players.TryGetSteamId(identityId))))
        return true;
      if (MySession.Static.LocalCharacter != null && identityId == MySession.Static.LocalCharacter.GetPlayerIdentityId())
        MyHud.Notifications.Add(MyNotificationSingletons.BlockNotResearched);
      return false;
    }

    public bool CheckDLCAndNotify(MyDefinitionBase definition)
    {
      MyHudNotificationBase notification = MyHud.Notifications.Get(MyNotificationSingletons.MissingDLC);
      MyDLCs.MyDLC missingDefinitionDlc = this.GetComponent<MySessionComponentDLC>().GetFirstMissingDefinitionDLC(definition, Sync.MyId);
      if (missingDefinitionDlc == null)
        return true;
      notification.SetTextFormatArguments((object) MyTexts.Get(missingDefinitionDlc.DisplayName));
      MyHud.Notifications.Add(notification);
      return false;
    }

    private void OnCameraEntityClosing(MyEntity entity) => this.SetCameraController(MyCameraControllerEnum.Spectator, (IMyEntity) null, new Vector3D?());

    public static void PerformPlatformPatchBeforeLoad(
      MyObjectBuilder_Checkpoint checkpoint,
      MyGameModeEnum? forceGameMode,
      MyOnlineModeEnum? forceOnlineMode)
    {
      if (checkpoint == null)
        return;
      MySession.PerformPlatformPatchBeforeLoad(checkpoint.Settings, forceGameMode, forceOnlineMode);
      if (MyPlatformGameSettings.IsModdingAllowed)
        return;
      checkpoint.Mods.Clear();
    }

    public static void PerformPlatformPatchBeforeLoad(
      MyObjectBuilder_WorldConfiguration worldConfig,
      MyGameModeEnum? forceGameMode,
      MyOnlineModeEnum? forceOnlineMode)
    {
      if (worldConfig == null)
        return;
      MySession.PerformPlatformPatchBeforeLoad(worldConfig.Settings, forceGameMode, forceOnlineMode);
      if (MyPlatformGameSettings.IsModdingAllowed)
        return;
      worldConfig.Mods.Clear();
    }

    public static void PerformPlatformPatchBeforeLoad(
      MyObjectBuilder_SessionSettings settings,
      MyGameModeEnum? forceGameMode,
      MyOnlineModeEnum? forceOnlineMode)
    {
      MySession.FixIncorrectSettings(settings);
      if (forceGameMode.HasValue)
        settings.GameMode = forceGameMode.Value;
      if (forceOnlineMode.HasValue)
        settings.OnlineMode = forceOnlineMode.Value;
      if (MyPlatformGameSettings.CONSOLE_COMPATIBLE)
      {
        settings.MaxPlanets = Math.Min(settings.MaxPlanets, 3);
        settings.PredefinedAsteroids = false;
        settings.UseConsolePCU = true;
        settings.VoxelGeneratorVersion = 5;
        settings.EnableContainerDrops = false;
        settings.OffensiveWordsFiltering = true;
        if (!Sandbox.Engine.Platform.Game.IsDedicated)
        {
          settings.EnableIngameScripts = false;
          if (!settings.Scenario)
          {
            int? nullable = settings.OnlineMode == MyOnlineModeEnum.OFFLINE ? MyPlatformGameSettings.OFFLINE_TOTAL_PCU_MAX : MyPlatformGameSettings.LOBBY_TOTAL_PCU_MAX;
            if (nullable.HasValue)
            {
              int num = Math.Min(settings.TotalPCU, nullable.Value);
              if (settings.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.NONE)
              {
                num = nullable.Value;
                settings.BlockLimitsEnabled = MyBlockLimitsEnabledEnum.GLOBALLY;
              }
              settings.TotalPCU = num;
            }
          }
        }
      }
      if (MyPlatformGameSettings.FORCED_VOXEL_TRASH_REMOVAL_SETTINGS.HasValue)
      {
        MyPlatformGameSettings.VoxelTrashRemovalSettings trashRemovalSettings = MyPlatformGameSettings.FORCED_VOXEL_TRASH_REMOVAL_SETTINGS.Value;
        settings.VoxelTrashRemovalEnabled = true;
        settings.VoxelAgeThreshold = trashRemovalSettings.Age;
        settings.VoxelGridDistanceThreshold = (float) trashRemovalSettings.MinDistanceFromGrid;
        settings.VoxelPlayerDistanceThreshold = (float) trashRemovalSettings.MinDistanceFromPlayer;
        if (trashRemovalSettings.RevertAsteroids)
          settings.TrashFlags |= MyTrashRemovalFlags.RevertAsteroids;
        if (trashRemovalSettings.RevertCloseToNPCGrids)
          settings.TrashFlags |= MyTrashRemovalFlags.RevertCloseToNPCGrids;
      }
      if (!MyPlatformGameSettings.ENABLE_TRASH_REMOVAL_SETTING)
        settings.TrashRemovalEnabled = true;
      Dictionary<string, short> dictionary = settings.BlockTypeLimits.Dictionary;
      foreach ((string str, short num1) in MyPlatformGameSettings.ADDITIONAL_BLOCK_LIMITS)
      {
        short local_7 = dictionary.GetValueOrDefault<string, short>(str, short.MaxValue);
        dictionary[str] = Math.Min(num1, local_7);
      }
      if (Sandbox.Engine.Platform.Game.IsDedicated || settings.OnlineMode == MyOnlineModeEnum.OFFLINE || settings.MaxPlayers != (short) 0 && (int) settings.MaxPlayers <= MyPlatformGameSettings.LOBBY_MAX_PLAYERS)
        return;
      settings.MaxPlayers = (short) MyPlatformGameSettings.LOBBY_MAX_PLAYERS;
    }

    public void InvokeLocalPlayerSkinOrColorChanged() => this.OnLocalPlayerSkinOrColorChanged.InvokeIfNotNull();

    IMyVoxelMaps IMySession.VoxelMaps => (IMyVoxelMaps) this.VoxelMaps;

    IMyCameraController IMySession.CameraController => this.CameraController;

    float IMySession.AssemblerEfficiencyMultiplier => this.AssemblerEfficiencyMultiplier;

    float IMySession.AssemblerSpeedMultiplier => this.AssemblerSpeedMultiplier;

    bool IMySession.AutoHealing => this.AutoHealing;

    uint IMySession.AutoSaveInMinutes => this.AutoSaveInMinutes;

    void IMySession.BeforeStartComponents() => this.BeforeStartComponents();

    bool IMySession.CargoShipsEnabled => this.CargoShipsEnabled;

    bool IMySession.ClientCanSave => false;

    bool IMySession.CreativeMode => this.CreativeMode;

    string IMySession.CurrentPath => this.CurrentPath;

    string IMySession.Description
    {
      get => this.Description;
      set => this.Description = value;
    }

    void IMySession.Draw() => this.Draw();

    TimeSpan IMySession.ElapsedPlayTime => this.ElapsedPlayTime;

    bool IMySession.EnableCopyPaste => this.IsCopyPastingEnabled;

    MyEnvironmentHostilityEnum IMySession.EnvironmentHostility => this.EnvironmentHostility;

    DateTime IMySession.GameDateTime
    {
      get => this.GameDateTime;
      set => this.GameDateTime = value;
    }

    void IMySession.GameOver() => this.GameOver();

    void IMySession.GameOver(MyStringId? customMessage) => this.GameOver(customMessage);

    MyObjectBuilder_Checkpoint IMySession.GetCheckpoint(
      string saveName)
    {
      return this.GetCheckpoint(saveName, false);
    }

    MyObjectBuilder_Sector IMySession.GetSector() => this.GetSector(true);

    Dictionary<string, byte[]> IMySession.GetVoxelMapsArray() => this.GetVoxelMapsArray(true);

    MyObjectBuilder_World IMySession.GetWorld() => this.GetWorld(true, false);

    float IMySession.GrinderSpeedMultiplier => this.GrinderSpeedMultiplier;

    float IMySession.HackSpeedMultiplier => this.HackSpeedMultiplier;

    float IMySession.InventoryMultiplier => this.InventoryMultiplier;

    float IMySession.CharactersInventoryMultiplier => this.CharactersInventoryMultiplier;

    float IMySession.BlocksInventorySizeMultiplier => this.BlocksInventorySizeMultiplier;

    bool IMySession.IsCameraAwaitingEntity
    {
      get => this.IsCameraAwaitingEntity;
      set => this.IsCameraAwaitingEntity = value;
    }

    bool IMySession.IsCameraControlledObject => this.IsCameraControlledObject();

    bool IMySession.IsCameraUserControlledSpectator => this.IsCameraUserControlledSpectator();

    bool IMySession.IsPausable() => this.IsPausable();

    short IMySession.MaxFloatingObjects => this.MaxFloatingObjects;

    short IMySession.MaxBackupSaves => this.MaxBackupSaves;

    short IMySession.MaxPlayers => this.MaxPlayers;

    bool IMySession.MultiplayerAlive
    {
      get => this.MultiplayerAlive;
      set => this.MultiplayerAlive = value;
    }

    bool IMySession.MultiplayerDirect
    {
      get => this.MultiplayerDirect;
      set => this.MultiplayerDirect = value;
    }

    double IMySession.MultiplayerLastMsg
    {
      get => this.MultiplayerLastMsg;
      set => this.MultiplayerLastMsg = value;
    }

    string IMySession.Name
    {
      get => this.Name;
      set => this.Name = value;
    }

    float IMySession.NegativeIntegrityTotal
    {
      get => this.NegativeIntegrityTotal;
      set => this.NegativeIntegrityTotal = value;
    }

    MyOnlineModeEnum IMySession.OnlineMode => this.OnlineMode;

    string IMySession.Password
    {
      get => this.Password;
      set => this.Password = value;
    }

    float IMySession.PositiveIntegrityTotal
    {
      get => this.PositiveIntegrityTotal;
      set => this.PositiveIntegrityTotal = value;
    }

    float IMySession.RefinerySpeedMultiplier => this.RefinerySpeedMultiplier;

    void IMySession.RegisterComponent(
      MySessionComponentBase component,
      MyUpdateOrder updateOrder,
      int priority)
    {
      this.RegisterComponent(component, updateOrder, priority);
    }

    bool IMySession.Save(string customSaveName) => this.Save(customSaveName);

    void IMySession.SetAsNotReady() => this.SetAsNotReady();

    void IMySession.SetCameraController(
      MyCameraControllerEnum cameraControllerEnum,
      IMyEntity cameraEntity,
      Vector3D? position)
    {
      this.SetCameraController(cameraControllerEnum, cameraEntity, position);
    }

    bool IMySession.ShowPlayerNamesOnHud => this.ShowPlayerNamesOnHud;

    bool IMySession.SurvivalMode => this.SurvivalMode;

    bool IMySession.ThrusterDamage => this.ThrusterDamage;

    string IMySession.ThumbPath => this.ThumbPath;

    TimeSpan IMySession.TimeOnBigShip => this.TimePilotingBigShip;

    TimeSpan IMySession.TimeOnFoot => this.TimeOnFoot;

    TimeSpan IMySession.TimeOnJetpack => this.TimeOnJetpack;

    TimeSpan IMySession.TimeOnSmallShip => this.TimePilotingSmallShip;

    void IMySession.Unload() => this.Unload();

    void IMySession.UnloadDataComponents() => this.UnloadDataComponents(false);

    void IMySession.UnloadMultiplayer() => this.UnloadMultiplayer();

    void IMySession.UnregisterComponent(MySessionComponentBase component) => this.UnregisterComponent(component);

    void IMySession.Update(MyTimeSpan time) => this.Update(time);

    void IMySession.UpdateComponents() => this.UpdateComponents();

    bool IMySession.WeaponsEnabled => this.WeaponsEnabled;

    float IMySession.WelderSpeedMultiplier => this.WelderSpeedMultiplier;

    ulong? IMySession.WorkshopId => this.WorkshopId;

    IMyPlayer IMySession.Player => (IMyPlayer) this.LocalHumanPlayer;

    VRage.Game.ModAPI.Interfaces.IMyControllableEntity IMySession.ControlledObject => (VRage.Game.ModAPI.Interfaces.IMyControllableEntity) this.ControlledEntity;

    MyObjectBuilder_SessionSettings IMySession.SessionSettings => this.Settings;

    IMyFactionCollection IMySession.Factions => (IMyFactionCollection) this.Factions;

    IMyCamera IMySession.Camera => (IMyCamera) MySector.MainCamera;

    double IMySession.CameraTargetDistance
    {
      get => (double) this.GetCameraTargetDistance();
      set => this.SetCameraTargetDistance(value);
    }

    public IMyConfig Config => (IMyConfig) MySandboxGame.Config;

    IMyDamageSystem IMySession.DamageSystem => (IMyDamageSystem) MyDamageSystem.Static;

    IMyGpsCollection IMySession.GPS => (IMyGpsCollection) MySession.Static.Gpss;

    bool IMySession.IsUserAdmin(ulong steamId) => MySession.Static.IsUserAdmin(steamId);

    [Obsolete("Use GetUserPromoteLevel")]
    bool IMySession.IsUserPromoted(ulong steamId) => MySession.Static.IsUserSpaceMaster(steamId);

    [Obsolete("Use HasCreativeRights")]
    bool IMySession.HasAdminPrivileges => this.HasCreativeRights;

    event Action IMySession.OnSessionReady
    {
      add => MySession.Static.OnReady += value;
      remove => MySession.Static.OnReady -= value;
    }

    event Action IMySession.OnSessionLoading
    {
      add => MySession.OnLoading += value;
      remove => MySession.OnLoading -= value;
    }

    MyPromoteLevel IMySession.PromoteLevel => this.GetUserPromoteLevel(Sync.MyId);

    MyPromoteLevel IMySession.GetUserPromoteLevel(ulong steamId) => this.GetUserPromoteLevel(steamId);

    bool IMySession.HasCreativeRights => this.HasCreativeRights;

    Version IMySession.Version => (Version) MyFinalBuildConstants.APP_VERSION;

    IMyOxygenProviderSystem IMySession.OxygenProviderSystem => (IMyOxygenProviderSystem) this.m_oxygenHelper;

    private class ComponentComparer : IComparer<MySessionComponentBase>
    {
      public int Compare(MySessionComponentBase x, MySessionComponentBase y)
      {
        int num = x.Priority.CompareTo(y.Priority);
        return num == 0 ? string.Compare(x.GetType().FullName, y.GetType().FullName, StringComparison.Ordinal) : num;
      }
    }

    public enum MyHitIndicatorTarget
    {
      Character,
      Headshot,
      Kill,
      Grid,
      Other,
      Friend,
      None,
    }

    private class GatherVoxelMaterialsWork : IPrioritizedWork, IWork
    {
      private List<MyVoxelBase> m_voxelMaps;
      private HashSet<string> m_target;
      private BoundingSphereD m_sphere;
      private Action m_completion;
      private int m_index;

      public GatherVoxelMaterialsWork(
        List<MyVoxelBase> voxelMaps,
        HashSet<string> target,
        BoundingSphereD sphere,
        Action completion)
      {
        this.m_voxelMaps = voxelMaps;
        this.m_target = target;
        this.m_sphere = sphere;
        this.m_completion = completion;
        this.m_index = -1;
      }

      public void DoWork(WorkData workData = null)
      {
        int index = Interlocked.Increment(ref this.m_index);
        if (index >= this.m_voxelMaps.Count)
          return;
        MyVoxelBase voxelMap = this.m_voxelMaps[index];
        MySession.GetVoxelMaterials(this.m_target, voxelMap, 7, this.m_sphere.Center, (float) MyFakes.PRIORITIZED_VOXEL_VICINITY_RADIUS_FAR);
        MySession.GetVoxelMaterials(this.m_target, voxelMap, 1, this.m_sphere.Center, (float) MyFakes.PRIORITIZED_VOXEL_VICINITY_RADIUS_CLOSE);
        if (index != this.m_voxelMaps.Count - 1)
          return;
        Action completion = this.m_completion;
        if (completion == null)
          return;
        completion();
      }

      public WorkOptions Options => new WorkOptions()
      {
        MaximumThreads = this.m_voxelMaps.Count,
        TaskType = MyProfiler.TaskType.Voxels
      };

      public WorkPriority Priority => WorkPriority.VeryLow;
    }

    public enum LimitResult
    {
      Passed,
      MaxGridSize,
      NoFaction,
      BlockTypeLimit,
      MaxBlocksPerPlayer,
      PCU,
    }

    protected sealed class OnCreativeToolsEnabled\u003C\u003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in bool value,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySession.OnCreativeToolsEnabled(value);
      }
    }

    protected sealed class OnCrash\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
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
        MySession.OnCrash();
      }
    }

    protected sealed class HitIndicatorActivationInternal\u003C\u003ESandbox_Game_World_MySession\u003C\u003EMyHitIndicatorTarget : ICallSite<IMyEventOwner, MySession.MyHitIndicatorTarget, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MySession.MyHitIndicatorTarget type,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySession.HitIndicatorActivationInternal(type);
      }
    }

    protected sealed class OnPromoteLevelSet\u003C\u003ESystem_UInt64\u0023VRage_Game_ModAPI_MyPromoteLevel : ICallSite<IMyEventOwner, ulong, MyPromoteLevel, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in ulong steamId,
        in MyPromoteLevel level,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySession.OnPromoteLevelSet(steamId, level);
      }
    }

    protected sealed class OnServerSaving\u003C\u003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in bool saveStarted,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySession.OnServerSaving(saveStarted);
      }
    }

    protected sealed class OnServerPerformanceWarning\u003C\u003ESystem_String\u0023VRage_MySimpleProfiler\u003C\u003EProfilingBlockType : ICallSite<IMyEventOwner, string, MySimpleProfiler.ProfilingBlockType, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string key,
        in MySimpleProfiler.ProfilingBlockType type,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySession.OnServerPerformanceWarning(key, type);
      }
    }

    protected sealed class SetSpectatorPositionFromServer\u003C\u003EVRageMath_Vector3D : ICallSite<IMyEventOwner, Vector3D, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in Vector3D position,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySession.SetSpectatorPositionFromServer(position);
      }
    }

    protected sealed class OnRequestVicinityInformation\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySession.OnRequestVicinityInformation(entityId);
      }
    }

    protected sealed class OnVicinityInformation\u003C\u003ESystem_Collections_Generic_List`1\u003CSystem_String\u003E\u0023System_Collections_Generic_List`1\u003CSystem_String\u003E\u0023System_Collections_Generic_List`1\u003CSystem_String\u003E : ICallSite<IMyEventOwner, List<string>, List<string>, List<string>, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in List<string> voxels,
        in List<string> models,
        in List<string> armorModels,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySession.OnVicinityInformation(voxels, models, armorModels);
      }
    }
  }
}
