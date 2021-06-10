// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.MyVisualScriptLogicProvider
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Physics;
using Sandbox.Game.AI;
using Sandbox.Game.Audio;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Interfaces;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Contracts;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.Data.Audio;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.AI;
using VRage.Game.ObjectBuilders.AI.Bot;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game.ObjectBuilders.Gui;
using VRage.Game.VisualScripting;
using VRage.Input;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game
{
  [StaticEventOwner]
  public static class MyVisualScriptLogicProvider
  {
    private static readonly Dictionary<Vector3I, bool> m_thrustDirections = new Dictionary<Vector3I, bool>();
    private static readonly Dictionary<int, MyHudNotification> m_addedNotificationsById = new Dictionary<int, MyHudNotification>();
    private static int m_notificationIdCounter;
    private static List<MyEntity> m_tmpEntities = new List<MyEntity>();
    private static Dictionary<string, (int Time, bool Running)> m_timers = new Dictionary<string, (int, bool)>();
    [Display(Description = "When player leaves cockpit.", Name = "Player Left Cockpit")]
    public static DoubleKeyPlayerEvent PlayerLeftCockpit;
    [Display(Description = "When player leaves cockpit.", Name = "Player Entered Cockpit")]
    public static DoubleKeyPlayerEvent PlayerEnteredCockpit;
    [Display(Description = "Called right after a respawn ship is created for a player.", Name = "Respawn Ship Spawned")]
    public static RespawnShipSpawnedEvent RespawnShipSpawned;
    [Display(Description = "", Name = "Cutscene Node Event")]
    public static CutsceneEvent CutsceneNodeEvent;
    [Display(Description = "When cutscene ended.", Name = "Cutscene Ended")]
    public static CutsceneEvent CutsceneEnded;
    [Display(Description = "When player spawns in the world.", Name = "Player Spawned")]
    public static SingleKeyPlayerEvent PlayerSpawned;
    [Display(Description = "Player was sorted into faction by Team Balancer", Name = "Team Balancer Player Sorted")]
    public static TeamBalancerSortEvent TeamBalancerPlayerSorted;
    [Display(Description = "When player requests respawn.", Name = "Player Respawn request")]
    public static SingleKeyPlayerEvent PlayerRequestsRespawn;
    [Display(Description = "When player dies in the world.", Name = "Player Died")]
    public static SingleKeyPlayerEvent PlayerDied;
    [Display(Description = "When player connects.", Name = "Player Connected")]
    public static SingleKeyPlayerEvent PlayerConnected;
    [Display(Description = "When player disconnects.", Name = "Player Disconnected")]
    public static SingleKeyPlayerEvent PlayerDisconnected;
    [Display(Description = "When player tries to connect.", Name = "Player Connect Request")]
    public static SingleKeyPlayerConnectRequestEvent PlayerConnectRequest;
    [Display(Description = "When player respawns.", Name = "Player Respawned")]
    public static SingleKeyPlayerEvent PlayerRespawnRequest;
    [Display(Description = "When player dies.", Name = "NPC Died")]
    public static SingleKeyEntityNameEvent NPCDied;
    [Display(Description = "When player is recharging health.", Name = "Player Health Recharging")]
    public static PlayerHealthRechargeEvent PlayerHealthRecharging;
    [Display(Description = "When suit is recharging power/oxygen/hydrogen.", Name = "Player Suit Recharging")]
    public static PlayerSuitRechargeEvent PlayerSuitRecharging;
    [Display(Description = "When timer block triggers.", Name = "Timer Block Triggered")]
    public static SingleKeyEntityNameGridNameEvent TimerBlockTriggered;
    [Display(Description = "", Name = "Timer Block Triggered Entity Name")]
    public static SingleKeyEntityNameGridNameEvent TimerBlockTriggeredEntityName;
    [Display(Description = "When player picks up item.", Name = "Player Picked Up Item")]
    public static FloatingObjectPlayerEvent PlayerPickedUp;
    [Display(Description = "When player drops item.", Name = "Player Dropped Item")]
    public static PlayerItemEvent PlayerDropped;
    [Display(Description = "When item spawns.", Name = "Item Spawned")]
    public static ItemSpawnedEvent ItemSpawned;
    [Display(Description = "When someone press the button.", Name = "Button Pressed Entity Name")]
    public static ButtonPanelEvent ButtonPressedEntityName;
    [Display(Description = "When someone press the button.", Name = "Button Pressed Terminal Name")]
    public static ButtonPanelEvent ButtonPressedTerminalName;
    [Display(Description = "When entity leaves area of the trigger.", Name = "Area Trigger Entity Left")]
    public static TriggerEventComplex AreaTrigger_EntityLeft;
    [Display(Description = "When entity enters area of the trigger.", Name = "Area Trigger Entity Entered")]
    public static TriggerEventComplex AreaTrigger_EntityEntered;
    [Display(Description = "When player leaves area of the trigger.", Name = "Area Trigger Left")]
    public static SingleKeyTriggerEvent AreaTrigger_Left;
    [Display(Description = "When player enters area of the trigger.", Name = "Area Trigger Entered")]
    public static SingleKeyTriggerEvent AreaTrigger_Entered;
    [Display(Description = "When screen is added.", Name = "Screen Added")]
    public static ScreenManagerEvent ScreenAdded;
    [Display(Description = "When screen is removed.", Name = "Screen Removed")]
    public static ScreenManagerEvent ScreenRemoved;
    [Display(Description = "When block is removed from the grid, either damage or grinding.", Name = "Block Destroyed")]
    public static SingleKeyEntityNameGridNameEvent BlockDestroyed;
    [Display(Description = "When block is removed from the grid, either damage or grinding.", Name = "Block Integrity Changed ")]
    public static SingleKeyEntityNameGridNameEvent BlockIntegrityChanged;
    [Display(Description = "When block is damaged.", Name = "Block Damaged")]
    public static BlockDamagedEvent BlockDamaged;
    [Display(Description = "When block is build.", Name = "Block Built")]
    public static BlockEvent BlockBuilt;
    [Display(Description = "When prefab is spawned.", Name = "Prefab Spawned")]
    public static SingleKeyEntityNameEvent PrefabSpawned;
    [Display(Description = "When prefab is spawned, includes prefab name.", Name = "Prefab Spawned Detailed")]
    public static PrefabSpawnedEvent PrefabSpawnedDetailed;
    [Display(Description = "When grid is spawned.", Name = "Grid Spawned")]
    public static SingleKeyEntityNameEvent GridSpawned;
    [Display(Description = "When block function state is changed.", Name = "Block Functionality Changed")]
    public static BlockFunctionalityChangedEvent BlockFunctionalityChanged;
    [Display(Description = "When tool is equipped.", Name = "Tool Equipped")]
    public static ToolEquipedEvent ToolEquipped;
    [Display(Description = "When landing gear is unlocked.", Name = "Landing Gear Unlocked")]
    public static LandingGearUnlockedEvent LandingGearUnlocked;
    [Display(Description = "When grid power generation state is changed.", Name = "Grid Power Generation State Changed")]
    public static GridPowerGenerationStateChangedEvent GridPowerGenerationStateChanged;
    [Display(Description = "When room is fully pressurized.", Name = "Room Fully Pressurized")]
    public static RoomFullyPressurizedEvent RoomFullyPressurized;
    [Display(Description = "When new item is build.", Name = "NewBuiltItem")]
    public static NewBuiltItemEvent NewItemBuilt;
    [Display(Description = "When gatling gun or missile launcher shoots.", Name = "WeaponBlockActivated")]
    public static WeaponBlockActivatedEvent WeaponBlockActivated;
    [Display(Description = "When Two connectors dis/connect.", Name = "ConnectorStateChanged")]
    public static ConnectorStateChangedEvent ConnectorStateChanged;
    [Display(Description = "When grid uses jumpdrive to jump.", Name = "GridJumped")]
    public static GridJumpedEvent GridJumped;
    [Display(Description = "When drill obtains ore by mining voxels.", Name = "ShipDrillDrilled")]
    public static ShipDrillCollectedEvent ShipDrillCollected;
    [Display(Description = "When remote control block get controlled by player.", Name = "RemoteControlChanged")]
    public static RemoteControlChangedEvent RemoteControlChanged;
    [Display(Description = "When an item on a toolbar is changed.", Name = "ToolbarItemChanged")]
    public static ToolbarItemChangedEvent ToolbarItemChanged;
    [Display(Description = "When new match state started", Name = "MatchStateStarted")]
    public static MatchStateStartedEvent MatchStateStarted;
    [Display(Description = "When new match state ended", Name = "MatchStateStarted")]
    public static MatchStateEndedEvent MatchStateEnded;
    [Display(Description = "When new match state changed", Name = "MatchStateStarted")]
    public static MatchStateChangedEvent MatchStateChanged;
    [Display(Description = "When contract has been accepted. 'startingFactionId' and 'startingStationId' are only for non-player-made contracts, 'startingBlockId' is only for player-made contracts.", Name = "ContractAccepted")]
    public static ContractAcceptedEvent ContractAccepted;
    [Display(Description = "When contract has been finished. 'startingFactionId' and 'startingStationId' are only for non-player-made contracts, 'startingBlockId' is only for player-made contracts.", Name = "ContractFinished")]
    public static ContractFinishedEvent ContractFinished;
    [Display(Description = "When contract has been failed. 'startingFactionId' and 'startingStationId' are only for non-player-made contracts, 'startingBlockId' is only for player-made contracts.", Name = "ContractFailed")]
    public static ContractFailedEvent ContractFailed;
    [Display(Description = "When contract has been abandoned. 'startingFactionId' and 'startingStationId' are only for non-player-made contracts, 'startingBlockId' is only for player-made contracts.", Name = "ContractAbandoned")]
    public static ContractAbandonedEvent ContractAbandoned;
    [Display(Description = "Called before match state ends. Set interrupt to true to stop state from ending. (If end was caused by timeout and interruption happens, time will still be out and end will be called again the next frame. )", Name = "BeforeMatchStateChanged")]
    public static MatchStateEndingEvent MatchStateEnding;
    private static MyStringId MUSIC = MyStringId.GetOrCompute("Music");
    private static MyStringHash DAMAGE_TYPE_SCRIPT = MyStringHash.GetOrCompute("Script");
    public static bool GameIsReady = false;
    private static bool m_registered = false;
    private static bool m_exitGameDialogOpened = false;
    private static readonly Dictionary<long, List<MyTuple<long, int>>> m_playerIdsToHighlightData = new Dictionary<long, List<MyTuple<long, int>>>();
    private static readonly Color DEFAULT_HIGHLIGHT_COLOR = new Color(0, 96, 209, 25);
    private static readonly MyDefinitionId ElectricityId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GasProperties), "Electricity");

    public static void Init()
    {
      MyCubeGrids.BlockBuilt += (Action<MyCubeGrid, MySlimBlock>) ((grid, block) =>
      {
        if (MyVisualScriptLogicProvider.BlockBuilt == null)
          return;
        MyVisualScriptLogicProvider.BlockBuilt(block.BlockDefinition.Id.TypeId.ToString(), block.BlockDefinition.Id.SubtypeName, grid.Name, block.FatBlock != null ? block.FatBlock.EntityId : 0L);
      });
      if (MyVisualScriptLogicProvider.m_registered)
        return;
      MyVisualScriptLogicProvider.m_registered = true;
      MySession.OnLoading += (Action) (() =>
      {
        MyVisualScriptLogicProvider.m_addedNotificationsById.Clear();
        MyVisualScriptLogicProvider.m_playerIdsToHighlightData.Clear();
      });
      Sandbox.Game.Entities.MyEntities.OnEntityAdd += (Action<MyEntity>) (entity =>
      {
        if (!(entity is MyCubeGrid myCubeGrid) || MyVisualScriptLogicProvider.BlockBuilt == null || myCubeGrid.BlocksCount != 1)
          return;
        MySlimBlock cubeBlock = myCubeGrid.GetCubeBlock(Vector3I.Zero);
        if (cubeBlock == null)
          return;
        MyVisualScriptLogicProvider.BlockBuilt(cubeBlock.BlockDefinition.Id.TypeId.ToString(), cubeBlock.BlockDefinition.Id.SubtypeName, myCubeGrid.Name, cubeBlock.FatBlock != null ? cubeBlock.FatBlock.EntityId : 0L);
      });
      MyScreenManager.ScreenRemoved += (Action<MyGuiScreenBase>) (screen =>
      {
        if (MyVisualScriptLogicProvider.ScreenRemoved == null)
          return;
        MyVisualScriptLogicProvider.ScreenRemoved(screen);
      });
      MyScreenManager.ScreenAdded += (Action<MyGuiScreenBase>) (screen =>
      {
        if (MyVisualScriptLogicProvider.ScreenAdded == null)
          return;
        MyVisualScriptLogicProvider.ScreenAdded(screen);
      });
      MyRespawnComponentBase.RespawnRequested += (Action<MyPlayer>) (player =>
      {
        if (MyVisualScriptLogicProvider.PlayerRespawnRequest == null)
          return;
        MyVisualScriptLogicProvider.PlayerRespawnRequest(player.Identity.IdentityId);
      });
      MyVisualScriptingProxy.RegisterType(typeof (MyGuiSounds));
      MyVisualScriptingProxy.RegisterType(typeof (MyKeys));
      MyVisualScriptingProxy.RegisterType(typeof (FlightMode));
      MyVisualScriptingProxy.RegisterType(typeof (Base6Directions.Direction));
      MyVisualScriptingProxy.RegisterType(typeof (MyGuiDrawAlignEnum));
      MyVisualScriptingProxy.RegisterType(typeof (VRage.Network.JoinResult));
      MyVisualScriptingProxy.WhitelistExtensions(typeof (MyVisualScriptLogicProvider));
      MyVisualScriptLogicProvider.m_timers.Clear();
    }

    public static SerializableDictionary<string, (int Time, bool Running)> GetTimers() => new SerializableDictionary<string, (int, bool)>(MyVisualScriptLogicProvider.m_timers);

    private static bool TryGetGrid(MyEntity entity, out MyCubeGrid grid)
    {
      switch (entity)
      {
        case MyCubeGrid _:
          grid = (MyCubeGrid) entity;
          return true;
        case MyCubeBlock _:
          grid = ((MyCubeBlock) entity).CubeGrid;
          return true;
        default:
          grid = (MyCubeGrid) null;
          return false;
      }
    }

    private static bool TryGetGrid(string entityName, out MyCubeGrid grid)
    {
      grid = (MyCubeGrid) null;
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(entityName);
      switch (entityByName)
      {
        case null:
          return false;
        case MyCubeGrid _:
          grid = (MyCubeGrid) entityByName;
          return true;
        case MyCubeBlock _:
          grid = ((MyCubeBlock) entityByName).CubeGrid;
          return true;
        default:
          return false;
      }
    }

    private static MyFixedPoint GetInventoryItemAmount(
      MyEntity entity,
      MyDefinitionId itemId)
    {
      MyFixedPoint myFixedPoint = (MyFixedPoint) 0;
      if (entity != null && entity.HasInventory)
      {
        for (int index = 0; index < entity.InventoryCount; ++index)
        {
          MyInventory inventory = MyEntityExtensions.GetInventory(entity, index);
          if (inventory != null)
            myFixedPoint += inventory.GetItemAmount(itemId, MyItemFlags.None, false);
        }
      }
      return myFixedPoint;
    }

    private static MyFixedPoint RemoveInventoryItems(
      MyEntity entity,
      MyDefinitionId itemId,
      MyFixedPoint amountToRemove)
    {
      MyFixedPoint myFixedPoint1 = (MyFixedPoint) 0;
      MyFixedPoint myFixedPoint2 = (MyFixedPoint) 0;
      if (entity != null && entity.HasInventory && amountToRemove > (MyFixedPoint) 0)
      {
        for (int index = 0; index < entity.InventoryCount; ++index)
        {
          MyInventory inventory = MyEntityExtensions.GetInventory(entity, index);
          if (inventory != null)
          {
            MyFixedPoint itemAmount = inventory.GetItemAmount(itemId, MyItemFlags.None, false);
            if (itemAmount > (MyFixedPoint) 0)
            {
              MyFixedPoint amount = MyFixedPoint.Min(amountToRemove, itemAmount);
              inventory.RemoveItemsOfType(amount, itemId, MyItemFlags.None, false);
              myFixedPoint1 += amount;
              amountToRemove -= amount;
            }
          }
          if (amountToRemove <= (MyFixedPoint) 0)
            break;
        }
      }
      return myFixedPoint1;
    }

    [VisualScriptingMiscData("AI", "Adds specific drone behavior from preset to a drone. (Reduced parameters)", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetDroneBehaviourBasic(string entityName, string presetName = "Default")
    {
      if (string.IsNullOrEmpty(presetName))
        return;
      MyVisualScriptLogicProvider.SetDroneBehaviourMethod(entityName, presetName, (List<MyEntity>) null, (List<DroneTarget>) null, true, true, 10, TargetPrioritization.PriorityRandom, 10000f, false);
    }

    [VisualScriptingMiscData("AI", "Adds specific drone behavior from preset to a drone. (Extended parameters)", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetDroneBehaviourAdvanced(
      string entityName,
      string presetName = "Default",
      bool activate = true,
      bool assignToPirates = true,
      List<MyEntity> waypoints = null,
      bool cycleWaypoints = false,
      List<MyEntity> targets = null)
    {
      if (string.IsNullOrEmpty(presetName))
        return;
      List<DroneTarget> targets1 = MyVisualScriptLogicProvider.DroneProcessTargets(targets);
      MyVisualScriptLogicProvider.SetDroneBehaviourMethod(entityName, presetName, waypoints, targets1, activate, assignToPirates, 10, TargetPrioritization.PriorityRandom, 10000f, cycleWaypoints);
    }

    [VisualScriptingMiscData("AI", "Adds specific drone behavior from preset to a drone. (Full parameters)", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetDroneBehaviourFull(
      string entityName,
      string presetName = "Default",
      bool activate = true,
      bool assignToPirates = true,
      List<MyEntity> waypoints = null,
      bool cycleWaypoints = false,
      List<MyEntity> targets = null,
      int playerPriority = 10,
      float maxPlayerDistance = 10000f,
      TargetPrioritization prioritizationStyle = TargetPrioritization.PriorityRandom)
    {
      if (string.IsNullOrEmpty(presetName))
        return;
      List<DroneTarget> targets1 = MyVisualScriptLogicProvider.DroneProcessTargets(targets);
      MyVisualScriptLogicProvider.SetDroneBehaviourMethod(entityName, presetName, waypoints, targets1, activate, assignToPirates, playerPriority, prioritizationStyle, maxPlayerDistance, cycleWaypoints);
    }

    public static List<DroneTarget> DroneProcessTargets(List<MyEntity> targets)
    {
      List<DroneTarget> droneTargetList = new List<DroneTarget>();
      if (targets != null)
      {
        foreach (MyEntity target in targets)
        {
          if (target is MyCubeGrid)
          {
            foreach (MySlimBlock block in ((MyCubeGrid) target).GetBlocks())
            {
              if (block.FatBlock is MyShipController)
                droneTargetList.Add(new DroneTarget((MyEntity) block.FatBlock, 8));
              if (block.FatBlock is MyReactor)
                droneTargetList.Add(new DroneTarget((MyEntity) block.FatBlock, 6));
              if (block.FatBlock is MyUserControllableGun)
                droneTargetList.Add(new DroneTarget((MyEntity) block.FatBlock, 10));
            }
          }
          else
            droneTargetList.Add(new DroneTarget(target));
        }
      }
      return droneTargetList;
    }

    private static MyRemoteControl DroneGetRemote(string entityName)
    {
      MyEntity myEntity = MyVisualScriptLogicProvider.GetEntityByName(entityName);
      if (myEntity == null)
        return (MyRemoteControl) null;
      MyRemoteControl myRemoteControl = myEntity as MyRemoteControl;
      if (myEntity is MyCubeBlock && myRemoteControl == null)
        myEntity = (MyEntity) ((MyCubeBlock) myEntity).CubeGrid;
      if (myEntity is MyCubeGrid)
      {
        foreach (MySlimBlock block in ((MyCubeGrid) myEntity).GetBlocks())
        {
          if (block.FatBlock is MyRemoteControl)
          {
            myRemoteControl = block.FatBlock as MyRemoteControl;
            break;
          }
        }
      }
      return myRemoteControl;
    }

    private static void SetDroneBehaviourMethod(
      string entityName,
      string presetName,
      List<MyEntity> waypoints,
      List<DroneTarget> targets,
      bool activate,
      bool assignToPirates,
      int playerPriority,
      TargetPrioritization prioritizationStyle,
      float maxPlayerDistance,
      bool cycleWaypoints)
    {
      MyRemoteControl remote = MyVisualScriptLogicProvider.DroneGetRemote(entityName);
      if (remote == null)
        return;
      if (waypoints != null)
      {
        int index = 0;
        while (index < waypoints.Count)
        {
          if (waypoints[index] == null)
            waypoints.RemoveAtFast<MyEntity>(index);
          else
            ++index;
        }
      }
      if (assignToPirates)
        remote.CubeGrid.ChangeGridOwnership(MyVisualScriptLogicProvider.GetPirateId(), MyOwnershipShareModeEnum.Faction);
      MyDroneAI myDroneAi = new MyDroneAI(remote, presetName, activate, waypoints, targets, playerPriority, prioritizationStyle, maxPlayerDistance, cycleWaypoints);
      remote.SetAutomaticBehaviour((MyRemoteControl.IRemoteControlAutomaticBehaviour) myDroneAi);
      if (!activate)
        return;
      remote.SetAutoPilotEnabled(true);
    }

    [VisualScriptingMiscData("AI", "Gets number of waypoints for specific drone. Returns -1 if drone has no remote or AI behavior.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static int DroneGetWaypointCount(string entityName)
    {
      MyRemoteControl remote = MyVisualScriptLogicProvider.DroneGetRemote(entityName);
      return remote != null && remote.AutomaticBehaviour != null ? remote.AutomaticBehaviour.WaypointList.Count + (remote.AutomaticBehaviour.WaypointActive ? 1 : 0) : -1;
    }

    [VisualScriptingMiscData("AI", "Gets position of curret waypoint of specific drone. If current waypoint exists, returns it position and 'waypointName' will be name of the waypoint. If waypoint does not exists, return current position and 'waypointName' will be empty string.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static Vector3D AutopilotGetCurrentWaypoint(
      string entityName,
      out string waypointName)
    {
      MyRemoteControl remote = MyVisualScriptLogicProvider.DroneGetRemote(entityName);
      waypointName = "";
      if (remote == null)
        return Vector3D.Zero;
      if (remote.CurrentWaypoint == null)
        return remote.PositionComp.GetPosition();
      waypointName = remote.CurrentWaypoint.Name;
      return remote.CurrentWaypoint.Coords;
    }

    [VisualScriptingMiscData("AI", "Orders drone to immediately skip current waypoint and go directly to the next one.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void AutopilotSkipCurrentWaypoint(string entityName)
    {
      MyRemoteControl remote = MyVisualScriptLogicProvider.DroneGetRemote(entityName);
      if (remote == null || remote.CurrentWaypoint == null)
        return;
      remote.AdvanceWaypoint();
    }

    [VisualScriptingMiscData("AI", "Gets count of targets for specific drone. Returns -1 if drone lacks remote or AI behavior.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static int DroneGetTargetsCount(string entityName)
    {
      MyRemoteControl remote = MyVisualScriptLogicProvider.DroneGetRemote(entityName);
      return remote != null && remote.AutomaticBehaviour != null ? remote.AutomaticBehaviour.TargetList.Count : -1;
    }

    [VisualScriptingMiscData("AI", "Returns true if specific drone has both remote and AI behavior, false otherwise.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool DroneHasAI(string entityName)
    {
      MyRemoteControl remote = MyVisualScriptLogicProvider.DroneGetRemote(entityName);
      return remote != null && remote.AutomaticBehaviour != null;
    }

    [VisualScriptingMiscData("AI", "Gets AI behavior of specific drone. Returns empty string if drone lacks remote or AI behavior.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static string DroneGetCurrentAIBehavior(string entityName)
    {
      MyRemoteControl remote = MyVisualScriptLogicProvider.DroneGetRemote(entityName);
      return remote != null && remote.AutomaticBehaviour != null ? remote.AutomaticBehaviour.ToString() : "";
    }

    [VisualScriptingMiscData("AI", "Sets current target of drone to specific entity.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void DroneSetTarget(string entityName, MyEntity target)
    {
      MyRemoteControl remote = MyVisualScriptLogicProvider.DroneGetRemote(entityName);
      if (remote == null || remote.AutomaticBehaviour == null || target == null)
        return;
      remote.AutomaticBehaviour.CurrentTarget = target;
    }

    [VisualScriptingMiscData("AI", "Activates/deactivates ambush mode for specific drone.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void DroneSetAmbushMode(string entityName, bool ambushModeOn = true)
    {
      MyRemoteControl remote = MyVisualScriptLogicProvider.DroneGetRemote(entityName);
      if (remote == null || remote.AutomaticBehaviour == null)
        return;
      remote.AutomaticBehaviour.Ambushing = ambushModeOn;
    }

    [VisualScriptingMiscData("AI", "Sets maximum speed limit of specific drone.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void DroneSetSpeedLimit(string entityName, float speedLimit = 100f)
    {
      MyRemoteControl remote = MyVisualScriptLogicProvider.DroneGetRemote(entityName);
      if (remote == null || remote.AutomaticBehaviour == null)
        return;
      remote.AutomaticBehaviour.SpeedLimit = speedLimit;
      remote.SetAutoPilotSpeedLimit(speedLimit);
    }

    [VisualScriptingMiscData("AI", "Gets speed limit of specific drone.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static float DroneGetSpeedLimit(string entityName)
    {
      MyRemoteControl remote = MyVisualScriptLogicProvider.DroneGetRemote(entityName);
      return remote != null && remote.AutomaticBehaviour != null ? remote.AutomaticBehaviour.SpeedLimit : 0.0f;
    }

    [VisualScriptingMiscData("AI", "Returns true if drone is in ambush mode, false otherwise.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool DroneIsInAmbushMode(string entityName)
    {
      MyRemoteControl remote = MyVisualScriptLogicProvider.DroneGetRemote(entityName);
      return remote != null && remote.AutomaticBehaviour != null && remote.AutomaticBehaviour.Ambushing;
    }

    [VisualScriptingMiscData("AI", "Returns true if specific drone has both working remoteand have operational AI behavior, false otherwise.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool DroneIsOperational(string entityName)
    {
      MyRemoteControl remote = MyVisualScriptLogicProvider.DroneGetRemote(entityName);
      return remote != null && remote.AutomaticBehaviour != null && remote.IsWorking && remote.AutomaticBehaviour.Operational;
    }

    [VisualScriptingMiscData("AI", "Adds specific waypoint to specific drone.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void DroneWaypointAdd(string entityName, MyEntity waypoint)
    {
      MyRemoteControl remote = MyVisualScriptLogicProvider.DroneGetRemote(entityName);
      if (remote == null || remote.AutomaticBehaviour == null)
        return;
      remote.AutomaticBehaviour.WaypointAdd(waypoint);
    }

    [VisualScriptingMiscData("AI", "Enables/disables waypoint cycling for specific drone.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void DroneWaypointSetCycling(string entityName, bool cycleWaypoints = true)
    {
      MyRemoteControl remote = MyVisualScriptLogicProvider.DroneGetRemote(entityName);
      if (remote == null || remote.AutomaticBehaviour == null)
        return;
      remote.AutomaticBehaviour.CycleWaypoints = cycleWaypoints;
    }

    [VisualScriptingMiscData("AI", "Sets player targeting priority of specific drone. (All player controlled entities will be considered a target if priority is greater than 0)", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void DroneSetPlayerPriority(string entityName, int priority)
    {
      MyRemoteControl remote = MyVisualScriptLogicProvider.DroneGetRemote(entityName);
      if (remote == null || remote.AutomaticBehaviour == null)
        return;
      remote.AutomaticBehaviour.PlayerPriority = priority;
    }

    [VisualScriptingMiscData("AI", "Sets target prioritization for specific drone.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void DroneSetPrioritizationStyle(string entityName, TargetPrioritization style)
    {
      MyRemoteControl remote = MyVisualScriptLogicProvider.DroneGetRemote(entityName);
      if (remote == null || remote.AutomaticBehaviour == null)
        return;
      remote.AutomaticBehaviour.PrioritizationStyle = style;
    }

    [VisualScriptingMiscData("AI", "Deletes all waypoints of specific drone.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void DroneWaypointClear(string entityName)
    {
      MyRemoteControl remote = MyVisualScriptLogicProvider.DroneGetRemote(entityName);
      if (remote == null || remote.AutomaticBehaviour == null)
        return;
      remote.AutomaticBehaviour.WaypointClear();
    }

    [VisualScriptingMiscData("AI", "Adds specific entity into targets of specific drone. Priority specifies order in which targets will be dealt with (higher is more important).", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void DroneTargetAdd(string entityName, MyEntity target, int priority = 1)
    {
      MyRemoteControl remote = MyVisualScriptLogicProvider.DroneGetRemote(entityName);
      if (remote == null || remote.AutomaticBehaviour == null)
        return;
      if (target is MyCubeGrid)
      {
        foreach (DroneTarget droneProcessTarget in MyVisualScriptLogicProvider.DroneProcessTargets(new List<MyEntity>()
        {
          target
        }))
          remote.AutomaticBehaviour.TargetAdd(droneProcessTarget);
      }
      else
        remote.AutomaticBehaviour.TargetAdd(new DroneTarget(target, priority));
    }

    [VisualScriptingMiscData("AI", "Clears all targets of specific drone.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void DroneTargetClear(string entityName)
    {
      MyRemoteControl remote = MyVisualScriptLogicProvider.DroneGetRemote(entityName);
      if (remote == null || remote.AutomaticBehaviour == null)
        return;
      remote.AutomaticBehaviour.TargetClear();
    }

    [VisualScriptingMiscData("AI", "Removes specific entity from drone's targets", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void DroneTargetRemove(string entityName, MyEntity target)
    {
      MyRemoteControl remote = MyVisualScriptLogicProvider.DroneGetRemote(entityName);
      if (remote == null || remote.AutomaticBehaviour == null)
        return;
      remote.AutomaticBehaviour.TargetRemove(target);
    }

    [VisualScriptingMiscData("AI", "Sets current target of specific drone to none.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void DroneTargetLoseCurrent(string entityName)
    {
      MyRemoteControl remote = MyVisualScriptLogicProvider.DroneGetRemote(entityName);
      if (remote == null || remote.AutomaticBehaviour == null)
        return;
      remote.AutomaticBehaviour.TargetLoseCurrent();
    }

    [VisualScriptingMiscData("AI", "Enables/disables collision avoidance for specific drone.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void DroneSetCollisionAvoidance(string entityName, bool collisionAvoidance = true)
    {
      MyRemoteControl remote = MyVisualScriptLogicProvider.DroneGetRemote(entityName);
      if (remote == null || remote.AutomaticBehaviour == null)
        return;
      remote.SetCollisionAvoidance(collisionAvoidance);
      remote.AutomaticBehaviour.CollisionAvoidance = collisionAvoidance;
    }

    [VisualScriptingMiscData("AI", "Sets origin point of specific drone. (Once non-kamikaze drone has no weapons, it will retreat to that point.)", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void DroneSetRetreatPosition(string entityName, Vector3D position)
    {
      MyRemoteControl remote = MyVisualScriptLogicProvider.DroneGetRemote(entityName);
      if (remote == null || remote.AutomaticBehaviour == null)
        return;
      remote.AutomaticBehaviour.OriginPoint = position;
    }

    [VisualScriptingMiscData("AI", "Enables/disables if drone should rotate toward it's target.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void DroneSetRotateToTarget(string entityName, bool rotateToTarget = true)
    {
      MyRemoteControl remote = MyVisualScriptLogicProvider.DroneGetRemote(entityName);
      if (remote == null || remote.AutomaticBehaviour == null)
        return;
      remote.AutomaticBehaviour.RotateToTarget = rotateToTarget;
    }

    [VisualScriptingMiscData("AI", "Adds grid with specific name into drone's targets.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void AddGridToTargetList(string gridName, string targetGridname)
    {
      MyCubeGrid grid1;
      MyCubeGrid grid2;
      if (!MyVisualScriptLogicProvider.TryGetGrid(gridName, out grid1) || !MyVisualScriptLogicProvider.TryGetGrid(targetGridname, out grid2))
        return;
      grid1.TargetingAddId(grid2.EntityId);
    }

    [VisualScriptingMiscData("AI", "Removes specific grid from drone's targets.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void RemoveGridFromTargetList(string gridName, string targetGridname)
    {
      MyCubeGrid grid1;
      MyCubeGrid grid2;
      if (!MyVisualScriptLogicProvider.TryGetGrid(gridName, out grid1) || !MyVisualScriptLogicProvider.TryGetGrid(targetGridname, out grid2))
        return;
      grid1.TargetingRemoveId(grid2.EntityId);
    }

    [VisualScriptingMiscData("AI", "Sets whitelist targeting mode. If true, entities in whitelist will be considered a target, if false, entities not in whitelist will be considered a target.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void TargetingSetWhitelist(string gridName, bool whitelistMode = true)
    {
      MyCubeGrid grid;
      if (!MyVisualScriptLogicProvider.TryGetGrid(gridName, out grid))
        return;
      grid.TargetingSetWhitelist(whitelistMode);
    }

    [VisualScriptingMiscData("AI", "Enables drone's autopilot, sets it to one-way go to waypoint and adds that one waypoint. All previous waypoints will be cleared.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void AutopilotGoToPosition(
      string entityName,
      Vector3D position,
      string waypointName = "Waypoint",
      float speedLimit = 120f,
      bool collisionAvoidance = true,
      bool precisionMode = false)
    {
      MyRemoteControl remote = MyVisualScriptLogicProvider.DroneGetRemote(entityName);
      if (remote == null)
        return;
      remote.SetCollisionAvoidance(collisionAvoidance);
      remote.SetAutoPilotSpeedLimit(speedLimit);
      remote.ChangeFlightMode(FlightMode.OneWay);
      remote.SetDockingMode(precisionMode);
      remote.ClearWaypoints();
      remote.AddWaypoint(position, waypointName);
      remote.SetAutoPilotEnabled(true);
    }

    [VisualScriptingMiscData("AI", "Clears all waypoints of specific drone.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void AutopilotClearWaypoints(string entityName) => MyVisualScriptLogicProvider.DroneGetRemote(entityName)?.ClearWaypoints();

    [VisualScriptingMiscData("AI", "Adds new waypoint for specific drone.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void AutopilotAddWaypoint(
      string entityName,
      Vector3D position,
      string waypointName = "Waypoint")
    {
      MyVisualScriptLogicProvider.DroneGetRemote(entityName)?.AddWaypoint(position, waypointName);
    }

    [VisualScriptingMiscData("AI", "Adds list of waypoints to specific drone. All waypoints will be called 'waypointName' followed by space and number. (given by order, starts with 1)", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void AutopilotSetWaypoints(
      string entityName,
      List<Vector3D> positions,
      string waypointName = "Waypoint")
    {
      MyRemoteControl remote = MyVisualScriptLogicProvider.DroneGetRemote(entityName);
      if (remote == null)
        return;
      remote.ClearWaypoints();
      if (positions == null)
        return;
      for (int index = 0; index < positions.Count; ++index)
        remote.AddWaypoint(positions[index], waypointName + " " + (index + 1).ToString());
    }

    [VisualScriptingMiscData("AI", "Enables/disables autopilot of specific drone", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void AutopilotEnabled(string entityName, bool enabled = true) => MyVisualScriptLogicProvider.DroneGetRemote(entityName)?.SetAutoPilotEnabled(enabled);

    [VisualScriptingMiscData("AI", "Activates autopilot of specific drone and set all required parameters. Waypoints will not be cleared.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void AutopilotActivate(
      string entityName,
      FlightMode mode = FlightMode.OneWay,
      float speedLimit = 120f,
      bool collisionAvoidance = true,
      bool precisionMode = false)
    {
      MyRemoteControl remote = MyVisualScriptLogicProvider.DroneGetRemote(entityName);
      if (remote == null)
        return;
      remote.SetCollisionAvoidance(collisionAvoidance);
      remote.SetAutoPilotSpeedLimit(speedLimit);
      remote.ChangeFlightMode(mode);
      remote.SetDockingMode(precisionMode);
      remote.SetAutoPilotEnabled(true);
    }

    [VisualScriptingMiscData("Audio", "Plays specific music cue.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void MusicPlayMusicCue(string cueName, bool playAtLeastOnce = true) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, bool>((Func<IMyEventOwner, Action<string, bool>>) (x => new Action<string, bool>(MyVisualScriptLogicProvider.MusicPlayMusicCueSync)), cueName, playAtLeastOnce);

    [Event(null, 1135)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void MusicPlayMusicCueSync(string cueName, bool playAtLeastOnce = true)
    {
      if (MyAudio.Static == null)
        return;
      if (MyMusicController.Static == null)
      {
        MyMusicController.Static = new MyMusicController(MyAudio.Static.GetAllMusicCues());
        MyAudio.Static.MusicAllowed = false;
        MyMusicController.Static.Active = true;
      }
      MyCueId cueId = MyAudio.Static.GetCueId(cueName);
      if (cueId.IsNull)
        return;
      MySoundData cue = MyAudio.Static.GetCue(cueId);
      if (cue != null && !cue.Category.Equals(MyVisualScriptLogicProvider.MUSIC))
        return;
      MyMusicController.Static.PlaySpecificMusicTrack(cueId, playAtLeastOnce);
    }

    public static void MusicPlayMusicCueLocal(string cueName, bool playAtLeastOnce = true) => MyVisualScriptLogicProvider.MusicPlayMusicCueSync(cueName, playAtLeastOnce);

    [VisualScriptingMiscData("Audio", "Sets currently selected category to specific category and play a track from it.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void MusicPlayMusicCategory(
      string categoryName,
      bool playAtLeastOnce = true,
      long playerId = -1)
    {
      long num = playerId != 0L || MySession.Static.LocalCharacter == null ? playerId : MySession.Static.LocalCharacter.GetPlayerIdentityId();
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, bool, long>((Func<IMyEventOwner, Action<string, bool, long>>) (x => new Action<string, bool, long>(MyVisualScriptLogicProvider.MusicPlayMusicCategorySync)), categoryName, playAtLeastOnce, num);
    }

    [Event(null, 1182)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void MusicPlayMusicCategorySync(
      string categoryName,
      bool playAtLeastOnce = true,
      long playerId = -1)
    {
      if (MyAudio.Static == null || playerId != -1L && MySession.Static.LocalPlayerId != playerId)
        return;
      if (MyMusicController.Static == null)
      {
        MyMusicController.Static = new MyMusicController(MyAudio.Static.GetAllMusicCues());
        MyAudio.Static.MusicAllowed = false;
        MyMusicController.Static.Active = true;
      }
      MyStringId orCompute = MyStringId.GetOrCompute(categoryName);
      if (orCompute.Id == 0)
        return;
      MyMusicController.Static.PlaySpecificMusicCategory(orCompute, playAtLeastOnce);
    }

    public static void MusicPlayMusicCategoryLocal(string categoryName, bool playAtLeastOnce = true) => MyVisualScriptLogicProvider.MusicPlayMusicCategorySync(categoryName, playAtLeastOnce);

    [VisualScriptingMiscData("Audio", "Sets currently selected category to specific music category.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void MusicSetMusicCategory(string categoryName) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string>((Func<IMyEventOwner, Action<string>>) (x => new Action<string>(MyVisualScriptLogicProvider.MusicSetMusicCategorySync)), categoryName);

    [Event(null, 1219)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void MusicSetMusicCategorySync(string categoryName)
    {
      if (MyAudio.Static == null)
        return;
      if (MyMusicController.Static == null)
      {
        MyMusicController.Static = new MyMusicController(MyAudio.Static.GetAllMusicCues());
        MyAudio.Static.MusicAllowed = false;
        MyMusicController.Static.Active = true;
      }
      MyStringId orCompute = MyStringId.GetOrCompute(categoryName);
      if (orCompute.Id == 0)
        return;
      MyMusicController.Static.SetSpecificMusicCategory(orCompute);
    }

    public static void MusicSetMusicCategoryLocal(string categoryName) => MyVisualScriptLogicProvider.MusicSetMusicCategorySync(categoryName);

    [VisualScriptingMiscData("Audio", "Enables/disables dynamic music category changes.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void MusicSetDynamicMusic(bool enabled) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<bool>((Func<IMyEventOwner, Action<bool>>) (x => new Action<bool>(MyVisualScriptLogicProvider.MusicSetDynamicMusicSync)), enabled);

    [Event(null, 1251)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void MusicSetDynamicMusicSync(bool enabled)
    {
      if (MyAudio.Static == null)
        return;
      if (MyMusicController.Static == null)
      {
        MyMusicController.Static = new MyMusicController(MyAudio.Static.GetAllMusicCues());
        MyAudio.Static.MusicAllowed = false;
        MyMusicController.Static.Active = true;
      }
      MyMusicController.Static.CanChangeCategoryGlobal = enabled;
    }

    public static void MusicSetDynamicMusicLocal(bool enabled) => MyVisualScriptLogicProvider.MusicSetDynamicMusicSync(enabled);

    [VisualScriptingMiscData("Audio", "Plays single sound on emitter attached to specific entity.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void PlaySingleSoundAtEntity(string soundName, string entityName) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, string>((Func<IMyEventOwner, Action<string, string>>) (x => new Action<string, string>(MyVisualScriptLogicProvider.PlaySingleSoundAtEntitySync)), soundName, entityName);

    [Event(null, 1279)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void PlaySingleSoundAtEntitySync(string soundName, string entityName)
    {
      if (MyAudio.Static == null || soundName.Length <= 0)
        return;
      MySoundPair soundId = new MySoundPair(soundName);
      if (soundId == MySoundPair.Empty)
        return;
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(entityName);
      if (entityByName == null)
        return;
      MyEntity3DSoundEmitter soundEmitter = MyAudioComponent.TryGetSoundEmitter();
      if (soundEmitter == null)
        return;
      soundEmitter.Entity = entityByName;
      soundEmitter.PlaySound(soundId);
    }

    public static void PlaySingleSoundAtEntityLocal(string soundName, string entityName) => MyVisualScriptLogicProvider.PlaySingleSoundAtEntitySync(soundName, entityName);

    [VisualScriptingMiscData("Audio", "Plays specific 2D HUD sound.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void PlayHudSound(MyGuiSounds sound = MyGuiSounds.HudClick, long playerId = 0) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<MyGuiSounds, long>((Func<IMyEventOwner, Action<MyGuiSounds, long>>) (x => new Action<MyGuiSounds, long>(MyVisualScriptLogicProvider.PlayHudSoundSync)), sound, playerId);

    [Event(null, 1316)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void PlayHudSoundSync(MyGuiSounds sound = MyGuiSounds.HudClick, long playerId = 0)
    {
      if (MyAudio.Static == null || (MySession.Static == null || MySession.Static.LocalPlayerId != playerId) && playerId != 0L)
        return;
      MyGuiAudio.PlaySound(sound);
    }

    public static void PlayHudSoundLocal(MyGuiSounds sound = MyGuiSounds.HudClick, long playerId = 0) => MyVisualScriptLogicProvider.PlayHudSoundSync(sound, playerId);

    [VisualScriptingMiscData("Audio", "Plays specific 3D sound at specific point.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void PlaySingleSoundAtPosition([Nullable] string soundName, Vector3D position) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, Vector3D>((Func<IMyEventOwner, Action<string, Vector3D>>) (x => new Action<string, Vector3D>(MyVisualScriptLogicProvider.PlaySingleSoundAtPositionSync)), soundName, position);

    [Event(null, 1339)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void PlaySingleSoundAtPositionSync([Nullable] string soundName, Vector3D position)
    {
      if (MyAudio.Static == null || soundName.Length <= 0)
        return;
      MySoundPair soundId = new MySoundPair(soundName);
      if (soundId == MySoundPair.Empty)
        return;
      MyEntity3DSoundEmitter soundEmitter = MyAudioComponent.TryGetSoundEmitter();
      if (soundEmitter == null)
        return;
      soundEmitter.SetPosition(new Vector3D?(position));
      soundEmitter.PlaySound(soundId);
    }

    public static void PlaySingleSoundAtPositionLocal(string soundName, Vector3D position) => MyVisualScriptLogicProvider.PlaySingleSoundAtPositionSync(soundName, position);

    [VisualScriptingMiscData("Audio", "Creates new 3D sound emitter at entity.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void CreateSoundEmitterAtEntity([Nullable] string newEmitterId, string entityName) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, string>((Func<IMyEventOwner, Action<string, string>>) (x => new Action<string, string>(MyVisualScriptLogicProvider.CreateSoundEmitterAtEntitySync)), newEmitterId, entityName);

    [Event(null, 1373)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void CreateSoundEmitterAtEntitySync([Nullable] string newEmitterId, string entityName)
    {
      if (MyAudio.Static == null || newEmitterId == null || newEmitterId.Length <= 0)
        return;
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(entityName);
      if (entityByName == null)
        return;
      MyAudioComponent.CreateNewLibraryEmitter(newEmitterId, entityByName);
    }

    public static void CreateSoundEmitterAtEntityLocal(string newEmitterId, string entityName) => MyVisualScriptLogicProvider.CreateSoundEmitterAtEntitySync(newEmitterId, entityName);

    [VisualScriptingMiscData("Audio", "Creates new 3D sound emitter at specific location.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void CreateSoundEmitterAtPosition([Nullable] string newEmitterId, Vector3D position) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, Vector3D>((Func<IMyEventOwner, Action<string, Vector3D>>) (x => new Action<string, Vector3D>(MyVisualScriptLogicProvider.CreateSoundEmitterAtPositionSync)), newEmitterId, position);

    [Event(null, 1405)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void CreateSoundEmitterAtPositionSync([Nullable] string newEmitterId, Vector3D position)
    {
      if (MyAudio.Static == null || newEmitterId == null || newEmitterId.Length <= 0)
        return;
      MyAudioComponent.CreateNewLibraryEmitter(newEmitterId)?.SetPosition(new Vector3D?(position));
    }

    public static void CreateSoundEmitterAtPositionLocal([Nullable] string newEmitterId, Vector3D position) => MyVisualScriptLogicProvider.CreateSoundEmitterAtPositionSync(newEmitterId, position);

    [VisualScriptingMiscData("Audio", "Plays sound on specific emitter. If 'playIn2D' is true, sound will be forced 2D.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void PlaySound([Nullable] string emitterId, string soundName, bool playIn2D = false) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, string, bool>((Func<IMyEventOwner, Action<string, string, bool>>) (x => new Action<string, string, bool>(MyVisualScriptLogicProvider.PlaySoundSync)), emitterId, soundName, playIn2D);

    [Event(null, 1434)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void PlaySoundSync([Nullable] string emitterId, string soundName, bool playIn2D = false)
    {
      if (MyAudio.Static == null || emitterId == null || emitterId.Length <= 0)
        return;
      MySoundPair soundId = new MySoundPair(soundName);
      if (soundId == MySoundPair.Empty)
        return;
      MyAudioComponent.GetLibraryEmitter(emitterId)?.PlaySound(soundId, true, force2D: playIn2D);
    }

    public static void PlaySoundLocal([Nullable] string emitterId, string soundName, bool playIn2D = false) => MyVisualScriptLogicProvider.PlaySoundSync(emitterId, soundName, playIn2D);

    [VisualScriptingMiscData("Audio", "Stops sound played by specific emitter.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void StopSound([Nullable] string emitterId, bool forced = false) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, bool>((Func<IMyEventOwner, Action<string, bool>>) (x => new Action<string, bool>(MyVisualScriptLogicProvider.StopSoundSync)), emitterId, forced);

    [Event(null, 1469)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void StopSoundSync([Nullable] string emitterId, bool forced = false)
    {
      if (MyAudio.Static == null || emitterId == null || emitterId.Length <= 0)
        return;
      MyAudioComponent.GetLibraryEmitter(emitterId)?.StopSound(forced);
    }

    public static void StopSoundLocal(string emitterId, bool forced = false) => MyVisualScriptLogicProvider.StopSoundSync(emitterId, forced);

    [VisualScriptingMiscData("Audio", "Removes specific sound emitter.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void RemoveSoundEmitter([Nullable] string emitterId) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string>((Func<IMyEventOwner, Action<string>>) (x => new Action<string>(MyVisualScriptLogicProvider.RemoveSoundEmitterSync)), emitterId);

    [Event(null, 1499)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void RemoveSoundEmitterSync([Nullable] string emitterId)
    {
      if (MyAudio.Static == null || emitterId == null || emitterId.Length <= 0)
        return;
      MyAudioComponent.RemoveLibraryEmitter(emitterId);
    }

    public static void RemoveSoundEmitterLocal(string emitterId) => MyVisualScriptLogicProvider.RemoveSoundEmitterSync(emitterId);

    [VisualScriptingMiscData("Audio", "Play ambient 2D sound.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void PlaySoundAmbient(string soundName) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string>((Func<IMyEventOwner, Action<string>>) (x => new Action<string>(MyVisualScriptLogicProvider.PlaySoundAmbientSync)), soundName);

    [Event(null, 1527)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void PlaySoundAmbientSync(string soundName)
    {
      if (MyAudio.Static == null || MySession.Static.LocalCharacter == null)
        return;
      MySoundPair soundId = new MySoundPair(soundName);
      if (soundId == MySoundPair.Empty)
        return;
      MyEntity3DSoundEmitter soundEmitter = MyAudioComponent.TryGetSoundEmitter();
      soundEmitter.Entity = (MyEntity) MySession.Static.LocalCharacter;
      soundEmitter?.PlaySound(soundId, force2D: true, alwaysHearOnRealistic: true);
    }

    public static void PlaySoundAmbientLocal(string soundName) => MyVisualScriptLogicProvider.PlaySoundAmbientSync(soundName);

    [VisualScriptingMiscData("Audio", "Set volume for game audio. Use -1 for no change. 0 min, 1 max.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetVolume(float gameVolume = -1f, float musicVolume = -1f, float voiceChatVolume = -1f) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<float, float, float>((Func<IMyEventOwner, Action<float, float, float>>) (x => new Action<float, float, float>(MyVisualScriptLogicProvider.SetVolumeSync)), gameVolume, musicVolume, voiceChatVolume);

    [Event(null, 1568)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void SetVolumeSync(float gameVolume = -1f, float musicVolume = -1f, float voiceChatVolume = -1f)
    {
      if (MyAudio.Static == null)
        return;
      if ((double) gameVolume != -1.0)
        MyAudio.Static.VolumeGame = gameVolume;
      if ((double) musicVolume != -1.0)
        MyAudio.Static.VolumeMusic = musicVolume;
      if ((double) voiceChatVolume == -1.0)
        return;
      MyAudio.Static.VolumeVoiceChat = voiceChatVolume;
    }

    public static void SetVolumeLocal(float gameVolume = -1f, float musicVolume = -1f, float voiceChatVolume = -1f) => MyVisualScriptLogicProvider.SetVolumeSync(gameVolume, musicVolume, voiceChatVolume);

    [VisualScriptingMiscData("Audio", "Get game volume.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static float GetGameVolume() => MyAudio.Static == null ? 0.0f : MyAudio.Static.VolumeGame;

    [VisualScriptingMiscData("Audio", "Get music volume.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static float GetMusicVolume() => MyAudio.Static == null ? 0.0f : MyAudio.Static.VolumeMusic;

    [VisualScriptingMiscData("Audio", "Get voice chat volume.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static float GetVoiceChatVolume() => MyAudio.Static == null ? 0.0f : MyAudio.Static.VolumeVoiceChat;

    [VisualScriptingMiscData("Blocks Generic", "Enables functional block.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void EnableBlock(string blockName) => MyVisualScriptLogicProvider.SetBlockState(blockName, true);

    [VisualScriptingMiscData("Blocks Generic", "Disables functional block.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void DisableBlock(string blockName) => MyVisualScriptLogicProvider.SetBlockState(blockName, false);

    [VisualScriptingMiscData("Blocks Generic", "Return true if 'secondBlock' is reachable from 'firstBlock'. (Can be only onle-way) ", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsConveyorConnected(string firstBlock, string secondBlock)
    {
      if (firstBlock.Equals(secondBlock))
        return true;
      MyEntity entity;
      return Sandbox.Game.Entities.MyEntities.TryGetEntityByName(firstBlock, out entity) && entity is IMyConveyorEndpointBlock conveyorEndpointBlock && (Sandbox.Game.Entities.MyEntities.TryGetEntityByName(secondBlock, out entity) && entity is IMyConveyorEndpointBlock conveyorEndpointBlock) && MyGridConveyorSystem.Reachable(conveyorEndpointBlock.ConveyorEndpoint, conveyorEndpointBlock.ConveyorEndpoint);
    }

    private static void SetBlockState(string name, bool state)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(name, out entity) || !(entity is MyFunctionalBlock))
        return;
      (entity as MyFunctionalBlock).Enabled = state;
    }

    [VisualScriptingMiscData("Blocks Generic", "Enables/disables functional block.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetBlockEnabled(string blockName, bool enabled = true) => MyVisualScriptLogicProvider.SetBlockState(blockName, enabled);

    [VisualScriptingMiscData("Blocks Generic", "Sets custom name of specific terminal block.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetBlockCustomName(string blockName, string newName)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(blockName, out entity) || !(entity is MyTerminalBlock))
        return;
      (entity as MyTerminalBlock).SetCustomName(newName);
    }

    [VisualScriptingMiscData("Blocks Generic", "Sets whether or not terminal block should be shown in terminal screen.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetBlockShowInTerminal(string blockName, bool showInTerminal = true)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(blockName, out entity) || !(entity is MyTerminalBlock))
        return;
      (entity as MyTerminalBlock).ShowInTerminal = showInTerminal;
    }

    [VisualScriptingMiscData("Blocks Generic", "Sets whether or not terminal block should be shown in inventory terminal screen.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetBlockShowInInventory(string blockName, bool showInInventory = true)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(blockName, out entity) || !(entity is MyTerminalBlock))
        return;
      (entity as MyTerminalBlock).ShowInInventory = showInInventory;
    }

    [VisualScriptingMiscData("Blocks Generic", "Sets whether or not terminal block should be seen in HUD.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetBlockShowOnHUD(string blockName, bool showOnHUD = true)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(blockName, out entity) || !(entity is MyTerminalBlock))
        return;
      (entity as MyTerminalBlock).ShowOnHUD = showOnHUD;
    }

    [VisualScriptingMiscData("Blocks Generic", "Returns true if specific cube block exists and is in functional state, otherwise false.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsBlockFunctional(string name)
    {
      MyEntity entity;
      return Sandbox.Game.Entities.MyEntities.TryGetEntityByName(name, out entity) && entity is MyCubeBlock && (entity as MyCubeBlock).IsFunctional;
    }

    [VisualScriptingMiscData("Blocks Generic", "Returns true if specific cube block exists and is in functional state, otherwise false. Access block by Id", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsBlockFunctionalById(long id)
    {
      MyEntity entity;
      return Sandbox.Game.Entities.MyEntities.TryGetEntityById(id, out entity) && entity is MyCubeBlock && (entity as MyCubeBlock).IsFunctional;
    }

    [VisualScriptingMiscData("Blocks Generic", "Returns true if specific functional block exist and is powered, otherwise false. This does not relate to Enabled status, it indicates that there is power available for the block. ", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsBlockPowered(string name)
    {
      MyEntity entity;
      return Sandbox.Game.Entities.MyEntities.TryGetEntityByName(name, out entity) && entity is MyFunctionalBlock && (entity as MyFunctionalBlock).ResourceSink != null && (entity as MyFunctionalBlock).ResourceSink.IsPowered;
    }

    [VisualScriptingMiscData("Blocks Generic", "Returns true if functional block exists and is enabled, otherwise false.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsBlockEnabled(string name)
    {
      MyEntity entity;
      return Sandbox.Game.Entities.MyEntities.TryGetEntityByName(name, out entity) && entity is MyFunctionalBlock && (entity as MyFunctionalBlock).Enabled;
    }

    [VisualScriptingMiscData("Blocks Generic", "Returns true if specific functional block exists and is working, otherwise false.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsBlockWorking(string name)
    {
      MyEntity entity;
      return Sandbox.Game.Entities.MyEntities.TryGetEntityByName(name, out entity) && entity is MyFunctionalBlock && (entity as MyFunctionalBlock).IsWorking;
    }

    [VisualScriptingMiscData("Blocks Generic", "Sets damage multiplier for specific block. (Value above 1 increase damage taken by the block, values in range <0;1> decrease damage taken. )", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetBlockGeneralDamageModifier(string blockName, float modifier = 1f)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(blockName, out entity) || !(entity is MyCubeBlock))
        return;
      ((MyCubeBlock) entity).SlimBlock.BlockGeneralDamageModifier = modifier;
    }

    [VisualScriptingMiscData("Blocks Generic", "Returns grid EntityId of grid that contains block with specific name. Returns 0 if name does not refer to a cube block. (If more entities have same name, only the first one created will be tested.)", -10510688)]
    [VisualScriptingMember(false, false)]
    public static long GetGridIdOfBlock(string entityName) => !(MyVisualScriptLogicProvider.GetEntityByName(entityName) is MyCubeBlock entityByName) ? 0L : entityByName.CubeGrid.EntityId;

    [VisualScriptingMiscData("Blocks Generic", "Returns current integrity of block in interval <0;1>.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static float GetBlockHealth(string entityName, bool buildIntegrity = true)
    {
      if (!(MyVisualScriptLogicProvider.GetEntityByName(entityName) is MyCubeBlock entityByName))
        return 0.0f;
      return buildIntegrity ? entityByName.SlimBlock.BuildIntegrity : entityByName.SlimBlock.Integrity;
    }

    [VisualScriptingMiscData("Blocks Generic", "Sets block integrity to specific value in range <0;1>. 'damageChange' says if the change is treated as damage or repair (Build integrity won't change in case of damage). 'changeOwner' is id of the one who causes the change.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetBlockHealth(
      string entityName,
      float integrity = 1f,
      bool damageChange = true,
      long changeOwner = 0)
    {
      if (!(MyVisualScriptLogicProvider.GetEntityByName(entityName) is MyCubeBlock entityByName))
        return;
      if (damageChange)
        entityByName.SlimBlock.SetIntegrity(entityByName.SlimBlock.BuildIntegrity, integrity, MyIntegrityChangeEnum.Damage, changeOwner);
      else
        entityByName.SlimBlock.SetIntegrity(integrity, integrity, MyIntegrityChangeEnum.Repair, changeOwner);
    }

    [VisualScriptingMiscData("Blocks Generic", "Applies damage to specific block from specific player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void DamageBlock(string entityName, float damage = 0.0f, long damageOwner = 0)
    {
      if (!(MyVisualScriptLogicProvider.GetEntityByName(entityName) is MyCubeBlock entityByName))
        return;
      entityByName.SlimBlock.DoDamage(damage, MyDamageType.Destruction, true, new MyHitInfo?(), damageOwner);
    }

    [VisualScriptingMiscData("Blocks Generic", "Returns ids of attached modules. Output parameters will contain additional informations.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static List<long> GetBlockAttachedUpgradeModules(
      string blockName,
      out int modulesCount,
      out int workingCount,
      out int slotsUsed,
      out int slotsTotal,
      out int incompatibleCount)
    {
      List<long> longList = new List<long>();
      modulesCount = 0;
      workingCount = 0;
      slotsUsed = 0;
      slotsTotal = 0;
      incompatibleCount = 0;
      MyEntity entity;
      Sandbox.Game.Entities.MyEntities.TryGetEntityByName(blockName, out entity);
      if (entity == null || !(entity is MyCubeBlock myCubeBlock))
        return longList;
      slotsTotal = myCubeBlock.GetComponent().ConnectionPositions.Count;
      if (myCubeBlock.CurrentAttachedUpgradeModules != null)
      {
        modulesCount = myCubeBlock.CurrentAttachedUpgradeModules.Count;
        lock (myCubeBlock.CurrentAttachedUpgradeModules)
        {
          foreach (MyCubeBlock.AttachedUpgradeModule attachedUpgradeModule in myCubeBlock.CurrentAttachedUpgradeModules.Values)
          {
            longList.Add(((VRage.ModAPI.IMyEntity) attachedUpgradeModule.Block).EntityId);
            slotsUsed += attachedUpgradeModule.SlotCount;
            incompatibleCount += attachedUpgradeModule.Compatible ? 0 : 1;
            workingCount += !attachedUpgradeModule.Compatible || !attachedUpgradeModule.Block.IsWorking ? 0 : 1;
          }
        }
      }
      return longList;
    }

    [VisualScriptingMiscData("Blocks Generic", "Sets color of specific block.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void ColorBlock(string blockName, Color color)
    {
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(blockName);
      if (entityByName == null || !(entityByName is MyCubeBlock myCubeBlock))
        return;
      Vector3 hsvdX11 = color.ColorToHSVDX11();
      myCubeBlock.CubeGrid.ChangeColorAndSkin(myCubeBlock.SlimBlock, new Vector3?(hsvdX11));
    }

    [VisualScriptingMiscData("Blocks Generic", "Sets skin of specific block.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SkinBlock(string blockName, string skin)
    {
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(blockName);
      if (entityByName == null || !(entityByName is MyCubeBlock myCubeBlock))
        return;
      myCubeBlock.CubeGrid.ChangeColorAndSkin(myCubeBlock.SlimBlock, skinSubtypeId: new MyStringHash?(MyStringHash.GetOrCompute(skin)));
    }

    [VisualScriptingMiscData("Blocks Generic", "Sets color and skin of specific block.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void ColorAndSkinBlock(string blockName, Color color, string skin)
    {
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(blockName);
      if (entityByName == null || !(entityByName is MyCubeBlock myCubeBlock))
        return;
      Vector3 hsvdX11 = color.ColorToHSVDX11();
      myCubeBlock.CubeGrid.ChangeColorAndSkin(myCubeBlock.SlimBlock, new Vector3?(hsvdX11), new MyStringHash?(MyStringHash.GetOrCompute(skin)));
    }

    [VisualScriptingMiscData("Blocks Specific", "Returns merge block status ( -1 - block don't exist, 2 - Locked, 1 - Constrained, 0 - Otherwise).", -10510688)]
    [VisualScriptingMember(false, false)]
    public static int GetMergeBlockStatus(string mergeBlockName)
    {
      MyEntity entity;
      Sandbox.Game.Entities.MyEntities.TryGetEntityByName(mergeBlockName, out entity);
      return entity == null || !(entity is MyFunctionalBlock myFunctionalBlock) ? -1 : myFunctionalBlock.GetBlockSpecificState();
    }

    [VisualScriptingMiscData("Blocks Specific", "Orders specific weapon block (UserControllableGun) to shoot once.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void WeaponShootOnce(string weaponName)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(weaponName, out entity) || !(entity is MyUserControllableGun))
        return;
      (entity as MyUserControllableGun).ShootFromTerminal((Vector3) entity.WorldMatrix.Forward);
    }

    [VisualScriptingMiscData("Blocks Specific", "Turns on/off shooting for specific weapon block (UserControllableGun)", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void WeaponSetShooting(string weaponName, bool shooting = true)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(weaponName, out entity) || !(entity is MyUserControllableGun))
        return;
      (entity as MyUserControllableGun).SetShooting(shooting);
    }

    [VisualScriptingMiscData("Blocks Specific", "Calls 'Start' action on specific functional block.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void StartTimerBlock(string blockName)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(blockName, out entity) || !(entity is Sandbox.ModAPI.IMyFunctionalBlock block))
        return;
      TerminalActionExtensions.ApplyAction(block, "Start");
    }

    [VisualScriptingMiscData("Blocks Specific", "Sets lock state of specific Landing gear.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetLandingGearLock(string entityName, bool locked = true)
    {
      if (!(MyVisualScriptLogicProvider.GetEntityByName(entityName) is IMyLandingGear entityByName))
        return;
      entityByName.RequestLock(locked);
    }

    [VisualScriptingMiscData("Blocks Specific", "Returns true if Landing gear is locked, false otherwise.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsLandingGearLocked(string entityName)
    {
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(entityName);
      return entityByName != null && entityByName is IMyLandingGear myLandingGear && myLandingGear.LockMode == LandingGearMode.Locked;
    }

    [VisualScriptingMiscData("Blocks Specific", "Gets information about specific landing gear. Returns true if informations were obtained, false if no such Landing gear exists.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool GetLandingGearInformation(
      string entityName,
      out bool locked,
      out bool inConstraint,
      out string attachedType,
      out string attachedName)
    {
      locked = false;
      inConstraint = false;
      attachedType = "";
      attachedName = "";
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(entityName);
      if (entityByName == null || !(entityByName is IMyLandingGear myLandingGear))
        return false;
      locked = myLandingGear.LockMode == LandingGearMode.Locked;
      inConstraint = myLandingGear.LockMode == LandingGearMode.ReadyToLock;
      if (locked && myLandingGear.GetAttachedEntity() is MyEntity attachedEntity)
      {
        ref string local = ref attachedType;
        string str;
        switch (attachedEntity)
        {
          case MyCubeBlock _:
            str = "Block";
            break;
          case MyCubeGrid _:
            str = "Grid";
            break;
          case MyVoxelBase _:
            str = "Voxel";
            break;
          default:
            str = "Other";
            break;
        }
        local = str;
        attachedName = attachedEntity.Name;
      }
      return true;
    }

    [VisualScriptingMiscData("Blocks Specific", "Gets information about specific landing gear. Returns true if informations were obtained, false if entity is not a Landing gear.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool GetLandingGearInformationFromEntity(
      MyEntity entity,
      out bool locked,
      out bool inConstraint,
      out string attachedType,
      out string attachedName)
    {
      locked = false;
      inConstraint = false;
      attachedType = "";
      attachedName = "";
      if (entity == null || !(entity is IMyLandingGear myLandingGear))
        return false;
      locked = myLandingGear.LockMode == LandingGearMode.Locked;
      inConstraint = myLandingGear.LockMode == LandingGearMode.ReadyToLock;
      if (locked && myLandingGear.GetAttachedEntity() is MyEntity attachedEntity)
      {
        ref string local = ref attachedType;
        string str;
        switch (attachedEntity)
        {
          case MyCubeBlock _:
            str = "Block";
            break;
          case MyCubeGrid _:
            str = "Grid";
            break;
          case MyVoxelBase _:
            str = "Voxel";
            break;
          default:
            str = "Other";
            break;
        }
        local = str;
        attachedName = attachedEntity.Name;
      }
      return true;
    }

    [VisualScriptingMiscData("Blocks Specific", "Returns true if specific connector is locked. False if unlocked of no such connector exists.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsConnectorLocked(string connectorName)
    {
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(connectorName);
      return entityByName != null && entityByName is Sandbox.ModAPI.IMyShipConnector myShipConnector && myShipConnector.IsConnected;
    }

    [VisualScriptingMiscData("Blocks Specific", "Calls 'Stop' action on specific functional block.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void StopTimerBlock(string blockName)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(blockName, out entity) || !(entity is Sandbox.ModAPI.IMyFunctionalBlock block))
        return;
      TerminalActionExtensions.ApplyAction(block, "Stop");
    }

    [VisualScriptingMiscData("Blocks Specific", "Calls 'TriggerNow' action on specific functional block.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void TriggerTimerBlock(string blockName)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(blockName, out entity) || !(entity is Sandbox.ModAPI.IMyFunctionalBlock block))
        return;
      TerminalActionExtensions.ApplyAction(block, "TriggerNow");
    }

    [VisualScriptingMiscData("Blocks Specific", "Sets specific doors to open/close state. (Doors, SlidingDoors, AirtightDoors)", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void ChangeDoorState(string doorBlockName, bool open = true)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(doorBlockName, out entity))
        return;
      if (entity is MyAdvancedDoor)
        (entity as MyAdvancedDoor).Open = open;
      if (entity is MyAirtightDoorGeneric)
        (entity as MyAirtightDoorGeneric).ChangeOpenClose(open);
      if (!(entity is MyDoor))
        return;
      (entity as MyDoor).Open = open;
    }

    [VisualScriptingMiscData("Blocks Specific", "Returns true if specific doors are open false if closed or door does not exist.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsDoorOpen(string doorBlockName)
    {
      MyEntity entity;
      if (Sandbox.Game.Entities.MyEntities.TryGetEntityByName(doorBlockName, out entity))
      {
        switch (entity)
        {
          case MyAdvancedDoor _:
            return (entity as MyAdvancedDoor).Open;
          case MyAirtightDoorGeneric _:
            return (entity as MyAirtightDoorGeneric).Open;
          case MyDoor _:
            return (entity as MyDoor).Open;
        }
      }
      return false;
    }

    [VisualScriptingMiscData("Blocks Specific", "Sets description of specific Text panel.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetTextPanelDescription(
      string panelName,
      string description,
      bool publicDescription = true)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(panelName, out entity) || !(entity is MyTextPanel myTextPanel))
        return;
      string str = MyStatControlText.SubstituteTexts(description.ToString());
      int num = publicDescription ? 1 : 0;
      EndpointId targetEndpoint = new EndpointId();
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyTextPanel, string, bool>(myTextPanel, (Func<MyTextPanel, Action<string, bool>>) (x => new Action<string, bool>(x.OnChangeDescription)), str, num != 0, targetEndpoint);
    }

    [VisualScriptingMiscData("Blocks Specific", "Sets colors of specific Text panel.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetTextPanelColors(string panelName, Color fontColor, Color backgroundColor)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(panelName, out entity) || !(entity is MyTextPanel myTextPanel))
        return;
      if (fontColor != Color.Transparent)
        myTextPanel.FontColor = fontColor;
      if (!(backgroundColor != Color.Transparent))
        return;
      myTextPanel.BackgroundColor = backgroundColor;
    }

    [VisualScriptingMiscData("Blocks Specific", "Sets title of specific Text panel.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetTextPanelTitle(string panelName, string title, bool publicTitle = true)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(panelName, out entity) || !(entity is MyTextPanel myTextPanel))
        return;
      if (publicTitle)
        myTextPanel.PublicDescription = new StringBuilder(title);
      else
        myTextPanel.PrivateDescription = new StringBuilder(title);
    }

    [VisualScriptingMiscData("Blocks Specific", "Removes pilot from specific Cockpit.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void CockpitRemovePilot(string cockpitName)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(cockpitName, out entity) || !(entity is MyCockpit myCockpit))
        return;
      myCockpit.RemovePilot();
    }

    [VisualScriptingMiscData("Blocks Specific", "Forces player into specific Cockpit.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void CockpitInsertPilot(
      string cockpitName,
      bool keepOriginalPlayerPosition = true,
      long playerId = 0)
    {
      MyCharacter characterFromPlayerId = MyVisualScriptLogicProvider.GetCharacterFromPlayerId(playerId);
      MyEntity entity;
      if (characterFromPlayerId == null || !Sandbox.Game.Entities.MyEntities.TryGetEntityByName(cockpitName, out entity) || !(entity is MyCockpit myCockpit))
        return;
      myCockpit.RemovePilot();
      if (characterFromPlayerId.Parent is MyCockpit)
        (characterFromPlayerId.Parent as MyCockpit).RemovePilot();
      if (characterFromPlayerId.ControllerInfo.Controller == null)
        MyVisualScriptLogicProvider.GetPlayerFromPlayerId(playerId).Controller.TakeControl((Sandbox.Game.Entities.IMyControllableEntity) characterFromPlayerId);
      myCockpit.AttachPilot(characterFromPlayerId, keepOriginalPlayerPosition);
    }

    [VisualScriptingMiscData("Blocks Specific", "Returns identity Id of player occupying cockpit or 0, if no one is in. ", -10510688)]
    [VisualScriptingMember(false, false)]
    public static long CockpitGetPilotId(string cockpitName, out bool occupied)
    {
      occupied = false;
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(cockpitName, out entity) || !(entity is MyCockpit myCockpit) || myCockpit.Pilot == null)
        return 0;
      occupied = myCockpit.Pilot != null;
      return myCockpit.Pilot.GetPlayerIdentityId();
    }

    [VisualScriptingMiscData("Blocks Specific", "Get all cockpits in the grid", -10510688)]
    [VisualScriptingMember(false, false)]
    public static List<string> GetAllCockpits(string gridName)
    {
      List<string> stringList = new List<string>();
      MyCubeGrid entity;
      if (Sandbox.Game.Entities.MyEntities.TryGetEntityByName<MyCubeGrid>(gridName, out entity))
      {
        foreach (MyCockpit myCockpit in entity.GetFatBlocks().OfType<MyCockpit>())
          stringList.Add(myCockpit.Name);
      }
      return stringList;
    }

    [VisualScriptingMiscData("Blocks Specific", "Sets color of specific Lighting block.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetLigtingBlockColor(string lightBlockName, Color color)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(lightBlockName, out entity) || !(entity is MyLightingBlock myLightingBlock))
        return;
      myLightingBlock.Color = color;
    }

    [VisualScriptingMiscData("Blocks Specific", "Sets intensity of specific Lighting block.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetLigtingBlockIntensity(string lightBlockName, float intensity)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(lightBlockName, out entity) || !(entity is MyLightingBlock myLightingBlock))
        return;
      myLightingBlock.Intensity = intensity;
    }

    [VisualScriptingMiscData("Blocks Specific", "Sets radius of specific Lighting block.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetLigtingBlockRadius(string lightBlockName, float radius)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(lightBlockName, out entity) || !(entity is MyLightingBlock myLightingBlock))
        return;
      myLightingBlock.Radius = radius;
    }

    [VisualScriptingMiscData("Blocks Specific", "True if block is part of airtight room (Best used for AirVents).", -10510688)]
    [VisualScriptingMember(true, false)]
    public static bool IsBlockPositionAirtight(string blockName)
    {
      MyEntity entity;
      return Sandbox.Game.Entities.MyEntities.TryGetEntityByName(blockName, out entity) && entity is Sandbox.ModAPI.IMyFunctionalBlock myFunctionalBlock && myFunctionalBlock.CubeGrid.IsRoomAtPositionAirtight(myFunctionalBlock.Position);
    }

    [VisualScriptingMiscData("Cutscenes", "Starts specific cutscene. If 'playerId' is -1, apply for all players, otherwise only for specific player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void StartCutscene(string cutsceneName, bool registerEvents = true, long playerId = -1)
    {
      long num = playerId != 0L || MySession.Static.LocalCharacter == null ? playerId : MySession.Static.LocalCharacter.GetPlayerIdentityId();
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, bool, long>((Func<IMyEventOwner, Action<string, bool, long>>) (x => new Action<string, bool, long>(MyVisualScriptLogicProvider.StartCutsceneSync)), cutsceneName, registerEvents, num);
    }

    [Event(null, 2450)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void StartCutsceneSync(string cutsceneName, bool registerEvents = true, long playerId = -1)
    {
      if (playerId != -1L && MySession.Static.LocalPlayerId != playerId)
        return;
      if (playerId == -1L && Sandbox.Engine.Multiplayer.MyMultiplayer.Static != null && !Sandbox.Engine.Multiplayer.MyMultiplayer.Static.IsServer)
        registerEvents = false;
      MySession.Static.GetComponent<MySessionComponentCutscenes>().PlayCutscene(cutsceneName, registerEvents);
    }

    public static void StartCutsceneLocal(string cutsceneName, bool registerEvents = true, long playerId = -1) => MyVisualScriptLogicProvider.StartCutsceneSync(cutsceneName, registerEvents, playerId);

    [VisualScriptingMiscData("Cutscenes", "Goes to next node in current cutscene. If 'playerId' is -1, apply for all players, otherwise only for specific player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void NextCutsceneNode(long playerId = -1) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (x => new Action<long>(MyVisualScriptLogicProvider.NextCutsceneNodeSync)), playerId != 0L || MySession.Static.LocalCharacter == null ? playerId : MySession.Static.LocalCharacter.GetPlayerIdentityId());

    [Event(null, 2476)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void NextCutsceneNodeSync(long playerId = -1)
    {
      if (playerId != -1L && MySession.Static.LocalPlayerId != playerId)
        return;
      MySession.Static.GetComponent<MySessionComponentCutscenes>().CutsceneNext(true);
    }

    public static void NextCutsceneNodeLocal(long playerId = -1) => MyVisualScriptLogicProvider.NextCutsceneNodeSync(playerId);

    [VisualScriptingMiscData("Cutscenes", "Ends current cutscene. If 'playerId' is -1, apply for all players, otherwise only for specific player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void EndCutscene(long playerId = -1) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (x => new Action<long>(MyVisualScriptLogicProvider.EndCutsceneSync)), playerId != 0L || MySession.Static.LocalCharacter == null ? playerId : MySession.Static.LocalCharacter.GetPlayerIdentityId());

    [Event(null, 2499)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void EndCutsceneSync(long playerId = -1)
    {
      if (playerId != -1L && MySession.Static.LocalPlayerId != playerId)
        return;
      MySession.Static.GetComponent<MySessionComponentCutscenes>().CutsceneEnd();
    }

    public static void EndCutsceneLocal(long playerId = -1) => MyVisualScriptLogicProvider.EndCutsceneSync(playerId);

    [VisualScriptingMiscData("Cutscenes", "Skips current cutscene. If 'playerId' is -1, apply for all players, otherwise only for specific player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SkipCutscene(long playerId = -1) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (x => new Action<long>(MyVisualScriptLogicProvider.SkipCutsceneSync)), playerId != 0L || MySession.Static.LocalCharacter == null ? playerId : MySession.Static.LocalCharacter.GetPlayerIdentityId());

    [Event(null, 2522)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void SkipCutsceneSync(long playerId = -1)
    {
      if (playerId != -1L && MySession.Static.LocalPlayerId != playerId)
        return;
      MySession.Static.GetComponent<MySessionComponentCutscenes>().CutsceneSkip();
    }

    public static void SkipCutsceneLocal(long playerId = -1) => MyVisualScriptLogicProvider.SkipCutsceneSync(playerId);

    [VisualScriptingMiscData("Cutscenes", "Preload entity on clients to prevent streaming and popping", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void RequestVicinityCache(long entityId) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (x => new Action<long>(MyVisualScriptLogicProvider.RequestVicinityCacheSync)), entityId);

    [Event(null, 2544)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void RequestVicinityCacheSync(long entityId = -1)
    {
      MySession.RequestVicinityCache(entityId);
      if (!(Sandbox.Engine.Multiplayer.MyMultiplayer.ReplicationLayer is MyReplicationClient replicationLayer))
        return;
      replicationLayer.RequestReplicable(entityId, (byte) 0, true);
    }

    public static void RequestVicinityCacheLocal(long entityId = -1) => MyVisualScriptLogicProvider.RequestVicinityCacheSync(entityId);

    [VisualScriptingMiscData("Effects", "Creates explosion at specific point with specified radius, causing damage to everything in range.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void CreateExplosion(Vector3D position, float radius, int damage = 5000) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<Vector3D, float, int>((Func<IMyEventOwner, Action<Vector3D, float, int>>) (s => new Action<Vector3D, float, int>(MyVisualScriptLogicProvider.CreateExplosionInternalAll)), position, radius, damage);

    [Event(null, 2574)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    private static void CreateExplosionInternalAll(Vector3D position, float radius, int damage = 5000)
    {
      MyExplosionTypeEnum explosionTypeEnum = MyExplosionTypeEnum.WARHEAD_EXPLOSION_50;
      if ((double) radius < 2.0)
        explosionTypeEnum = MyExplosionTypeEnum.WARHEAD_EXPLOSION_02;
      else if ((double) radius < 15.0)
        explosionTypeEnum = MyExplosionTypeEnum.WARHEAD_EXPLOSION_15;
      else if ((double) radius < 30.0)
        explosionTypeEnum = MyExplosionTypeEnum.WARHEAD_EXPLOSION_30;
      MyExplosionInfo explosionInfo = new MyExplosionInfo()
      {
        PlayerDamage = 0.0f,
        Damage = (float) damage,
        ExplosionType = explosionTypeEnum,
        ExplosionSphere = new BoundingSphereD(position, (double) radius),
        LifespanMiliseconds = 700,
        ParticleScale = 1f,
        Direction = new Vector3?(Vector3.Down),
        VoxelExplosionCenter = position,
        ExplosionFlags = MyExplosionFlags.CREATE_DEBRIS | MyExplosionFlags.AFFECT_VOXELS | MyExplosionFlags.APPLY_FORCE_AND_DAMAGE | MyExplosionFlags.CREATE_DECALS | MyExplosionFlags.CREATE_PARTICLE_EFFECT | MyExplosionFlags.CREATE_SHRAPNELS | MyExplosionFlags.APPLY_DEFORMATION,
        VoxelCutoutScale = 1f,
        PlaySound = true,
        ApplyForceAndDamage = true,
        ObjectsRemoveDelayInMiliseconds = 40
      };
      MyExplosions.AddExplosion(ref explosionInfo);
    }

    [VisualScriptingMiscData("Effects", "Creates specific particle effect at position.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void CreateParticleEffectAtPosition(string effectName, Vector3D position) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, Vector3D>((Func<IMyEventOwner, Action<string, Vector3D>>) (s => new Action<string, Vector3D>(MyVisualScriptLogicProvider.CreateParticleEffectAtPositionInternalAll)), effectName, position);

    [Event(null, 2622)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    private static void CreateParticleEffectAtPositionInternalAll(
      string effectName,
      Vector3D position)
    {
      MyParticlesManager.TryCreateParticleEffect(effectName, MatrixD.CreateWorld(position), out MyParticleEffect _);
    }

    [VisualScriptingMiscData("Effects", "Creates specific particle effect at entity.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void CreateParticleEffectAtEntity(string effectName, string entityName) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, string>((Func<IMyEventOwner, Action<string, string>>) (s => new Action<string, string>(MyVisualScriptLogicProvider.CreateParticleEffectAtEntityInternalAll)), effectName, entityName);

    [Event(null, 2640)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    private static void CreateParticleEffectAtEntityInternalAll(
      string effectName,
      string entityName)
    {
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(entityName);
      if (entityByName == null)
        return;
      MyParticlesManager.TryCreateParticleEffect(effectName, entityByName.WorldMatrix, out MyParticleEffect _);
    }

    [VisualScriptingMiscData("Effects", "Fades/shows screen over period of time.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void ScreenColorFadingStart(float time = 1f, bool toOpaque = true, long playerId = -1)
    {
      switch (playerId)
      {
        case -1:
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<float, bool>((Func<IMyEventOwner, Action<float, bool>>) (s => new Action<float, bool>(MyVisualScriptLogicProvider.ScreenColorFadingStartInternalAll)), time, toOpaque);
          break;
        case 0:
          MyVisualScriptLogicProvider.ScreenColorFadingStart(time, toOpaque);
          break;
        default:
          MyPlayer.PlayerId result;
          if (!MySession.Static.Players.TryGetPlayerId(playerId, out result))
            break;
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<float, bool>((Func<IMyEventOwner, Action<float, bool>>) (s => new Action<float, bool>(MyVisualScriptLogicProvider.ScreenColorFadingStartInternal)), time, toOpaque, new EndpointId(result.SteamId));
          break;
      }
    }

    [Event(null, 2679)]
    [Reliable]
    [ServerInvoked]
    private static void ScreenColorFadingStartInternal(float time = 1f, bool toOpaque = true) => MyVisualScriptLogicProvider.ScreenColorFadingStart(time, toOpaque);

    [Event(null, 2685)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    private static void ScreenColorFadingStartInternalAll(float time = 1f, bool toOpaque = true) => MyVisualScriptLogicProvider.ScreenColorFadingStart(time, toOpaque);

    private static void ScreenColorFadingStart(float time = 1f, bool toOpaque = true) => MyHud.ScreenEffects.FadeScreen(toOpaque ? 0.0f : 1f, time);

    [VisualScriptingMiscData("Effects", "Sets target color for screen fading.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void ScreenColorFadingSetColor(Color color = default (Color), long playerId = -1)
    {
      switch (playerId)
      {
        case -1:
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<Color>((Func<IMyEventOwner, Action<Color>>) (s => new Action<Color>(MyVisualScriptLogicProvider.ScreenColorFadingSetColorInternalAll)), color);
          break;
        case 0:
          MyVisualScriptLogicProvider.ScreenColorFadingSetColor(color);
          break;
        default:
          MyPlayer.PlayerId result;
          if (!MySession.Static.Players.TryGetPlayerId(playerId, out result))
            break;
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<Color>((Func<IMyEventOwner, Action<Color>>) (s => new Action<Color>(MyVisualScriptLogicProvider.ScreenColorFadingSetColorInternal)), color, new EndpointId(result.SteamId));
          break;
      }
    }

    [Event(null, 2724)]
    [Reliable]
    [ServerInvoked]
    private static void ScreenColorFadingSetColorInternal(Color color) => MyVisualScriptLogicProvider.ScreenColorFadingSetColor(color);

    [Event(null, 2730)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    private static void ScreenColorFadingSetColorInternalAll(Color color) => MyVisualScriptLogicProvider.ScreenColorFadingSetColor(color);

    private static void ScreenColorFadingSetColor(Color color) => MyHud.ScreenEffects.BlackScreenColor = new Color(color, 0.0f);

    [VisualScriptingMiscData("Effects", "Switches screen fade state. Screen will un/fade over specified time.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void ScreenColorFadingStartSwitch(float time = 1f, long playerId = -1)
    {
      switch (playerId)
      {
        case -1:
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<float>((Func<IMyEventOwner, Action<float>>) (s => new Action<float>(MyVisualScriptLogicProvider.ScreenColorFadingStartSwitchInternalAll)), time);
          break;
        case 0:
          MyVisualScriptLogicProvider.ScreenColorFadingStartSwitch(time);
          break;
        default:
          MyPlayer.PlayerId result;
          if (!MySession.Static.Players.TryGetPlayerId(playerId, out result))
            break;
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<float>((Func<IMyEventOwner, Action<float>>) (s => new Action<float>(MyVisualScriptLogicProvider.ScreenColorFadingStartSwitchInternal)), time, new EndpointId(result.SteamId));
          break;
      }
    }

    [Event(null, 2769)]
    [Reliable]
    [ServerInvoked]
    private static void ScreenColorFadingStartSwitchInternal(float time = 1f) => MyVisualScriptLogicProvider.ScreenColorFadingStartSwitch(time);

    [Event(null, 2775)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    private static void ScreenColorFadingStartSwitchInternalAll(float time = 1f) => MyVisualScriptLogicProvider.ScreenColorFadingStartSwitch(time);

    private static void ScreenColorFadingStartSwitch(float time = 1f) => MyHud.ScreenEffects.SwitchFadeScreen(time);

    [VisualScriptingMiscData("Effects", "Sets if screen fade should minimize HUD.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void ScreenColorFadingMinimalizeHUD(bool minimalize = true, long playerId = -1)
    {
      switch (playerId)
      {
        case -1:
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<bool>((Func<IMyEventOwner, Action<bool>>) (s => new Action<bool>(MyVisualScriptLogicProvider.ScreenColorFadingMinimalizeHUDInternalAll)), minimalize);
          break;
        case 0:
          MyVisualScriptLogicProvider.ScreenColorFadingMinimalizeHUD(minimalize);
          break;
        default:
          MyPlayer.PlayerId result;
          if (!MySession.Static.Players.TryGetPlayerId(playerId, out result))
            break;
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<bool>((Func<IMyEventOwner, Action<bool>>) (s => new Action<bool>(MyVisualScriptLogicProvider.ScreenColorFadingMinimalizeHUDInternal)), minimalize, new EndpointId(result.SteamId));
          break;
      }
    }

    [Event(null, 2814)]
    [Reliable]
    [ServerInvoked]
    private static void ScreenColorFadingMinimalizeHUDInternal(bool minimalize) => MyVisualScriptLogicProvider.ScreenColorFadingMinimalizeHUD(minimalize);

    [Event(null, 2820)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    private static void ScreenColorFadingMinimalizeHUDInternalAll(bool minimalize) => MyVisualScriptLogicProvider.ScreenColorFadingMinimalizeHUD(minimalize);

    private static void ScreenColorFadingMinimalizeHUD(bool minimalize) => MyHud.ScreenEffects.BlackScreenMinimalizeHUD = minimalize;

    [VisualScriptingMiscData("Effects", "False to force minimize HUD, true to disable force minimization. (Force minimization overrides HUD state without actually changing it so you can revert back safely.)", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void ShowHud(bool flag = true, long playerId = -1)
    {
      switch (playerId)
      {
        case -1:
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<bool>((Func<IMyEventOwner, Action<bool>>) (s => new Action<bool>(MyVisualScriptLogicProvider.ShowHudInternalAll)), flag);
          break;
        case 0:
          MyVisualScriptLogicProvider.ShowHud(flag);
          break;
        default:
          MyPlayer.PlayerId result;
          if (!MySession.Static.Players.TryGetPlayerId(playerId, out result))
            break;
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<bool>((Func<IMyEventOwner, Action<bool>>) (s => new Action<bool>(MyVisualScriptLogicProvider.ShowHudInternal)), flag, new EndpointId(result.SteamId));
          break;
      }
    }

    [Event(null, 2860)]
    [Reliable]
    [ServerInvoked]
    private static void ShowHudInternal(bool flag = true) => MyVisualScriptLogicProvider.ScreenColorFadingMinimalizeHUD(flag);

    [Event(null, 2866)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    private static void ShowHudInternalAll(bool flag = true) => MyVisualScriptLogicProvider.ShowHud(flag);

    private static void ShowHud(bool flag = true) => MyHud.MinimalHud = !flag;

    [VisualScriptingMiscData("Effects", "Set state of HUD to specific state. 0 - minimal hud.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetHudState(int state = 0, long playerId = -1)
    {
      switch (playerId)
      {
        case -1:
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<int>((Func<IMyEventOwner, Action<int>>) (s => new Action<int>(MyVisualScriptLogicProvider.SetHudStateInternalAll)), state);
          break;
        case 0:
          MyVisualScriptLogicProvider.SetHudState(state);
          break;
        default:
          MyPlayer.PlayerId result;
          if (!MySession.Static.Players.TryGetPlayerId(playerId, out result))
            break;
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<int>((Func<IMyEventOwner, Action<int>>) (s => new Action<int>(MyVisualScriptLogicProvider.SetHudStateInternal)), state, new EndpointId(result.SteamId));
          break;
      }
    }

    [Event(null, 2905)]
    [Reliable]
    [ServerInvoked]
    private static void SetHudStateInternal(int state) => MyVisualScriptLogicProvider.SetHudState(state);

    [Event(null, 2911)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    private static void SetHudStateInternalAll(int state) => MyVisualScriptLogicProvider.SetHudState(state);

    private static void SetHudState(int state) => MyHud.HudState = state;

    [VisualScriptingMiscData("Entity", "Gets specific entity by name. If there are more entities by same name, the first one created will be taken.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static MyEntity GetEntityByName(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
        return (MyEntity) null;
      MyEntity entity;
      long result;
      return Sandbox.Game.Entities.MyEntities.TryGetEntityByName(name, out entity) || long.TryParse(name, out result) && Sandbox.Game.Entities.MyEntities.TryGetEntityById(result, out entity) ? entity : (MyEntity) null;
    }

    [VisualScriptingMiscData("Entity", "Gets specific entity by id.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static MyEntity GetEntityById(long id)
    {
      MyEntity entity;
      return !Sandbox.Game.Entities.MyEntities.TryGetEntityById(id, out entity) ? (MyEntity) null : entity;
    }

    [VisualScriptingMiscData("Entity", "Returns entity id of specific entity ", -10510688)]
    [VisualScriptingMember(false, false)]
    public static long GetEntityIdFromName(string name)
    {
      MyEntity entity;
      return !Sandbox.Game.Entities.MyEntities.TryGetEntityByName(name, out entity) ? 0L : entity.EntityId;
    }

    [VisualScriptingMiscData("Entity", "Gets entity id from specific entity.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static long GetEntityIdFromEntity(MyEntity entity) => entity == null ? 0L : entity.EntityId;

    [VisualScriptingMiscData("Entity", "Gets position of specific entity.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static Vector3D GetEntityPosition(string entityName)
    {
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(entityName);
      return entityByName != null ? entityByName.PositionComp.GetPosition() : Vector3D.Zero;
    }

    [VisualScriptingMiscData("Entity", "Gets world matrix of specific entity.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static MatrixD GetEntityWorldMatrix(MyEntity entity) => entity == null ? MatrixD.Identity : entity.WorldMatrix;

    [VisualScriptingMiscData("Entity", "Breaks and returns world matrix of specific entity.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static void GetEntityVectors(
      string entityName,
      out Vector3D position,
      out Vector3D forward,
      out Vector3D up)
    {
      position = Vector3D.Zero;
      forward = Vector3D.Forward;
      up = Vector3D.Up;
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(entityName);
      if (entityByName == null)
        return;
      position = entityByName.PositionComp.WorldMatrixRef.Translation;
      ref Vector3D local1 = ref forward;
      MatrixD matrixD = entityByName.PositionComp.WorldMatrixRef;
      Vector3D forward1 = matrixD.Forward;
      local1 = forward1;
      ref Vector3D local2 = ref up;
      matrixD = entityByName.PositionComp.WorldMatrixRef;
      Vector3D up1 = matrixD.Up;
      local2 = up1;
    }

    [VisualScriptingMiscData("Entity", "Sets world position of specific entity.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetEntityPosition(string entityName, Vector3D position) => MyVisualScriptLogicProvider.GetEntityByName(entityName)?.PositionComp.SetPosition(position);

    [VisualScriptingMiscData("Entity", "Gets vector in world coordination system representing entity's direction (e.g. Direction.Forward will return real forward vector of entity in world coordination system.)", -10510688)]
    [VisualScriptingMember(false, false)]
    public static Vector3D GetEntityDirection(
      string entityName,
      Base6Directions.Direction direction = Base6Directions.Direction.Forward)
    {
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(entityName);
      if (entityByName == null)
        return Vector3D.Forward;
      switch (direction)
      {
        case Base6Directions.Direction.Backward:
          return entityByName.WorldMatrix.Backward;
        case Base6Directions.Direction.Left:
          return entityByName.WorldMatrix.Left;
        case Base6Directions.Direction.Right:
          return entityByName.WorldMatrix.Right;
        case Base6Directions.Direction.Up:
          return entityByName.WorldMatrix.Up;
        case Base6Directions.Direction.Down:
          return entityByName.WorldMatrix.Down;
        default:
          return entityByName.WorldMatrix.Forward;
      }
    }

    [VisualScriptingMiscData("Entity", "Returns true if point is in natural gravity close to planet(eg. if nearest planet exists).", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsPlanetNearby(Vector3D position) => (double) MyGravityProviderSystem.CalculateNaturalGravityInPoint(position).LengthSquared() > 0.0 && MyGamePruningStructure.GetClosestPlanet(position) != null;

    [VisualScriptingMiscData("Entity", "Returns name of a planet if point is close to a plane (in its natural gravity). Else returns 'Void'. !!!BEWARE 'Void' is just for English as this string is localized. For checking if there really is a planet or not use 'IsPlanetNearby(...)' function as output here might be inconsistent between localizations.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static string GetNearestPlanet(Vector3D position)
    {
      if ((double) MyGravityProviderSystem.CalculateNaturalGravityInPoint(position).LengthSquared() > 0.0)
      {
        MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(position);
        if (closestPlanet != null && closestPlanet.Generator != null)
          return closestPlanet.Generator.FolderName;
      }
      return MyTexts.GetString(MyCommonTexts.Void);
    }

    [VisualScriptingMiscData("Entity", "Returns List of ids of entities in defined sphere", -10510688)]
    [VisualScriptingMember(false, false)]
    public static List<long> GetEntitiesInSphere(Vector3D position, float radius)
    {
      BoundingSphereD sphere = new BoundingSphereD(position, (double) radius);
      List<MyEntity> myEntityList = new List<MyEntity>();
      MyGamePruningStructure.GetAllEntitiesInSphere(ref sphere, myEntityList);
      return myEntityList.Select<MyEntity, long>((Func<MyEntity, long>) (x => x.EntityId)).ToList<long>();
    }

    [VisualScriptingMiscData("Entity", "Returns List of ids of entities in hit by a line", -10510688)]
    [VisualScriptingMember(false, false)]
    public static List<long> GetAllEntitiesInRay(
      Vector3D from,
      Vector3D to,
      out List<float> distances)
    {
      LineD ray = new LineD(from, to);
      List<MyLineSegmentOverlapResult<MyEntity>> segmentOverlapResultList = new List<MyLineSegmentOverlapResult<MyEntity>>();
      MyGamePruningStructure.GetAllEntitiesInRay(ref ray, segmentOverlapResultList);
      distances = segmentOverlapResultList.Select<MyLineSegmentOverlapResult<MyEntity>, float>((Func<MyLineSegmentOverlapResult<MyEntity>, float>) (x => (float) x.Distance)).ToList<float>();
      return segmentOverlapResultList.Select<MyLineSegmentOverlapResult<MyEntity>, long>((Func<MyLineSegmentOverlapResult<MyEntity>, long>) (x => x.Element.EntityId)).ToList<long>();
    }

    [VisualScriptingMiscData("Entity", "Adds item defined by id in specific quantity into inventory of entity.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void AddToInventory(string entityname, MyDefinitionId itemId, int amount = 1)
    {
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(entityname);
      if (entityByName == null)
        return;
      MyInventoryBase inventoryBase = entityByName.GetInventoryBase();
      if (inventoryBase == null)
        return;
      MyObjectBuilder_PhysicalObject newObject = (MyObjectBuilder_PhysicalObject) MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) itemId);
      MyFixedPoint myFixedPoint = new MyFixedPoint();
      MyFixedPoint amount1 = (MyFixedPoint) amount;
      inventoryBase.AddItems(amount1, (MyObjectBuilder_Base) newObject);
    }

    [VisualScriptingMiscData("Entity", "Adds item defined by id in specific quantity into inventory of entity.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void AddToInventoryFloat(string entityname, MyDefinitionId itemId, float amount = 1f)
    {
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(entityname);
      if (entityByName == null)
        return;
      MyInventoryBase inventoryBase = entityByName.GetInventoryBase();
      if (inventoryBase == null)
        return;
      MyObjectBuilder_PhysicalObject newObject = (MyObjectBuilder_PhysicalObject) MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) itemId);
      MyFixedPoint myFixedPoint = new MyFixedPoint();
      MyFixedPoint amount1 = (MyFixedPoint) amount;
      inventoryBase.AddItems(amount1, (MyObjectBuilder_Base) newObject);
    }

    [VisualScriptingMiscData("Entity", "Removes item defined by id in specific quantity from inventory of entity.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void RemoveFromEntityInventory(
      string entityName,
      MyDefinitionId itemId = default (MyDefinitionId),
      float amount = 1f)
    {
      MyFixedPoint amountToRemove = (MyFixedPoint) amount;
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(entityName);
      if (entityByName == null)
        return;
      if (entityByName is MyCubeGrid)
      {
        foreach (MyCubeBlock fatBlock in ((MyCubeGrid) entityByName).GetFatBlocks())
        {
          if (fatBlock != null && fatBlock.HasInventory)
            amountToRemove -= MyVisualScriptLogicProvider.RemoveInventoryItems((MyEntity) fatBlock, itemId, amountToRemove);
          if (amountToRemove <= (MyFixedPoint) 0)
            break;
        }
      }
      else
        MyVisualScriptLogicProvider.RemoveInventoryItems(entityByName, itemId, amountToRemove);
    }

    [VisualScriptingMiscData("Entity", "Gets amount of specific items in inventory of entity. (rounded)", -10510688)]
    [VisualScriptingMember(false, false)]
    public static int GetEntityInventoryItemAmount(string entityName, MyDefinitionId itemId) => (int) Math.Round((double) MyVisualScriptLogicProvider.GetEntityInventoryItemAmountPrecise(entityName, itemId));

    [VisualScriptingMiscData("Entity", "Gets amount of specific items in inventory of entity.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static float GetEntityInventoryItemAmountPrecise(
      string entityName,
      MyDefinitionId itemId)
    {
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(entityName);
      if (entityByName == null || !entityByName.HasInventory && !(entityByName is MyCubeGrid))
        return 0.0f;
      MyFixedPoint myFixedPoint = (MyFixedPoint) 0;
      if (entityByName is MyCubeGrid)
      {
        foreach (MyCubeBlock fatBlock in ((MyCubeGrid) entityByName).GetFatBlocks())
        {
          if (fatBlock != null && fatBlock.HasInventory)
            myFixedPoint += MyVisualScriptLogicProvider.GetInventoryItemAmount((MyEntity) fatBlock, itemId);
        }
      }
      else
        myFixedPoint = MyVisualScriptLogicProvider.GetInventoryItemAmount(entityByName, itemId);
      return (float) myFixedPoint;
    }

    [VisualScriptingMiscData("Entity", "Returns true if entity has item in specific inventory on specific slot. Also return definition id of that item and its amount.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool GetEntityInventoryItemAtSlot(
      string entityName,
      out MyDefinitionId itemId,
      out float amount,
      int slot = 0,
      int inventoryId = 0)
    {
      itemId = new MyDefinitionId();
      amount = 0.0f;
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(entityName);
      if (entityByName == null || !entityByName.HasInventory)
        return false;
      inventoryId = Math.Max(inventoryId, 0);
      if (inventoryId >= entityByName.InventoryCount)
        return false;
      MyInventory inventory = MyEntityExtensions.GetInventory(entityByName, inventoryId);
      if (inventory != null)
      {
        MyPhysicalInventoryItem? itemByIndex = inventory.GetItemByIndex(slot);
        if (itemByIndex.HasValue)
        {
          amount = (float) itemByIndex.Value.Amount;
          itemId = itemByIndex.Value.Content.GetObjectId();
          return true;
        }
      }
      return false;
    }

    [VisualScriptingMiscData("Entity", "Changes ownership of a specific block (if entity is block) or ownership of all functional blocks (if entity is grid) to specific player and modify its/theirs share settings.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static bool ChangeOwner(
      string entityName,
      long playerId = 0,
      bool factionShare = false,
      bool allShare = false)
    {
      if (string.IsNullOrEmpty(entityName))
        return false;
      MyOwnershipShareModeEnum shareMode = MyOwnershipShareModeEnum.None;
      if (factionShare)
        shareMode = MyOwnershipShareModeEnum.Faction;
      if (allShare)
        shareMode = MyOwnershipShareModeEnum.All;
      MyEntity entity;
      if (Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity))
      {
        switch (entity)
        {
          case MyCubeBlock myCubeBlock:
            myCubeBlock.ChangeBlockOwnerRequest(0L, shareMode);
            if (playerId > 0L)
              myCubeBlock.ChangeBlockOwnerRequest(playerId, shareMode);
            return true;
          case MyCubeGrid grid:
            foreach (MyCubeBlock fatBlock in grid.GetFatBlocks())
            {
              switch (fatBlock)
              {
                case MyFunctionalBlock _:
                case MyShipController _:
                case MyTerminalBlock _:
                  grid.ChangeOwnerRequest(grid, fatBlock, 0L, shareMode);
                  if (playerId > 0L)
                  {
                    grid.ChangeOwnerRequest(grid, fatBlock, playerId, shareMode);
                    continue;
                  }
                  continue;
                default:
                  continue;
              }
            }
            return true;
        }
      }
      return false;
    }

    [VisualScriptingMiscData("Entity", "Get owner of specific entity. 0 for nobody.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static long GetOwner(string entityName)
    {
      MyEntity entity;
      if (Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity))
      {
        switch (entity)
        {
          case MyCubeBlock myCubeBlock:
            return myCubeBlock.OwnerId;
          case MyCubeGrid myCubeGrid:
            return myCubeGrid.BigOwners.Count <= 0 ? 0L : myCubeGrid.BigOwners[0];
        }
      }
      return 0;
    }

    [VisualScriptingMiscData("Entity", "Renames specific entity.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void RenameEntity(string oldName, string newName = null)
    {
      if (oldName == newName)
        return;
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(oldName);
      if (entityByName == null)
        return;
      entityByName.Name = newName;
      Sandbox.Game.Entities.MyEntities.SetEntityName(entityByName);
    }

    [VisualScriptingMiscData("Entity", "Gets name of specific entity defined by id.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetName(long entityId, string name)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById(entityId, out entity) || MyVisualScriptLogicProvider.GetEntityByName(name) != null)
        return;
      entity.Name = name;
      Sandbox.Game.Entities.MyEntities.SetEntityName(entity);
    }

    [VisualScriptingMiscData("Entity", "Gets name of specific entity defined by id.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static string GetEntityName(long entityId)
    {
      MyEntity entity;
      return !Sandbox.Game.Entities.MyEntities.TryGetEntityById(entityId, out entity) ? string.Empty : entity.Name;
    }

    [VisualScriptingMiscData("Entity", "Gets linear velocity of specific entity.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static Vector3D GetEntitySpeed(string entityName)
    {
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(entityName);
      return entityByName != null && entityByName.Physics != null ? (Vector3D) entityByName.Physics.LinearVelocity : Vector3D.Zero;
    }

    [VisualScriptingMiscData("Entity", "Gets DefinitionId from typeId and subtypeId", -10510688)]
    [VisualScriptingMember(false, false)]
    public static MyDefinitionId GetDefinitionId(string typeId, string subtypeId)
    {
      MyObjectBuilderType result;
      if (MyObjectBuilderType.TryParse(typeId, out result))
        return new MyDefinitionId(result, subtypeId);
      MyObjectBuilderType.TryParse("MyObjectBuilder_" + typeId, out result);
      return new MyDefinitionId(result, subtypeId);
    }

    [VisualScriptingMiscData("Entity", "Gets typeId and subtypeId out of DefinitionId.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static void GetDataFromDefinition(
      MyDefinitionId definitionId,
      out string typeId,
      out string subtypeId)
    {
      typeId = definitionId.TypeId.ToString();
      subtypeId = definitionId.SubtypeId.ToString();
    }

    [VisualScriptingMiscData("Entity", "Removes specific entity from world.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void RemoveEntity(string entityName, bool removeGroup = true, string typeRestriction = null) => MyVisualScriptLogicProvider.RemoveEntity(MyVisualScriptLogicProvider.GetEntityByName(entityName), removeGroup, typeRestriction);

    [VisualScriptingMiscData("Entity", "Removes specific entity from world.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void RemoveEntity(long entityId, bool removeGroup = true, string typeRestriction = null) => MyVisualScriptLogicProvider.RemoveEntity(MyVisualScriptLogicProvider.GetEntityById(entityId), removeGroup, typeRestriction);

    [VisualScriptingMiscData("Entity", "Removes specific entity from world.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void RemoveEntity(MyEntity entity, bool removeGroup = true, string typeRestriction = null)
    {
      IEnumerable<string> allowedTypes = (IEnumerable<string>) null;
      IEnumerable<string> disallowedTypes = (IEnumerable<string>) null;
      if (!string.IsNullOrEmpty(typeRestriction))
      {
        IEnumerable<string> source = ((IEnumerable<string>) typeRestriction.Split(',')).Select<string, string>((Func<string, string>) (type => type.Trim()));
        allowedTypes = source.Where<string>((Func<string, bool>) (x => !x.StartsWith("!")));
        disallowedTypes = source.Where<string>((Func<string, bool>) (x => x.StartsWith("!")));
      }
      MyVisualScriptLogicProvider.RemoveEntity(entity, removeGroup, allowedTypes, disallowedTypes);
    }

    private static void RemoveEntity(
      MyEntity entity,
      bool removeGroup,
      IEnumerable<string> allowedTypes,
      IEnumerable<string> disallowedTypes)
    {
      if (entity == null)
        return;
      if (allowedTypes != null && disallowedTypes != null)
      {
        foreach (string disallowedType in disallowedTypes)
        {
          if (entity.GetType().Name == disallowedType)
            return;
        }
        bool flag = false;
        foreach (string allowedType in allowedTypes)
        {
          if (entity.GetType().Name == allowedType)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          return;
      }
      if (entity is MyCubeBlock)
      {
        MyCubeBlock myCubeBlock = (MyCubeBlock) entity;
        myCubeBlock.CubeGrid.RemoveBlock(myCubeBlock.SlimBlock, true);
      }
      else if (entity is MyCubeGrid & removeGroup)
      {
        MyCubeGrid root = MyGridPhysicalHierarchy.Static.GetRoot((MyCubeGrid) entity);
        MyGridPhysicalHierarchy.Static.ApplyOnChildren(root, (Action<MyCubeGrid>) (x => x.Close()));
        root.Close();
      }
      else
        entity.Close();
    }

    [VisualScriptingMiscData("Entity", "Removes entities in designated area.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void RemoveEntities(
      Vector3D position,
      double radius,
      bool removePlanets = true,
      bool removeGroup = true,
      string typeRestriction = null)
    {
      if (radius <= 0.0)
        return;
      BoundingSphereD sphere = new BoundingSphereD(position, radius);
      MyVisualScriptLogicProvider.m_tmpEntities.Clear();
      MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref sphere, MyVisualScriptLogicProvider.m_tmpEntities);
      if (!removePlanets)
        typeRestriction = !string.IsNullOrEmpty(typeRestriction) ? typeRestriction + ",!MyPlanet" : "!MyPlanet";
      IEnumerable<string> allowedTypes = (IEnumerable<string>) null;
      IEnumerable<string> disallowedTypes = (IEnumerable<string>) null;
      if (!string.IsNullOrEmpty(typeRestriction))
      {
        IEnumerable<string> source = ((IEnumerable<string>) typeRestriction.Split(',')).Select<string, string>((Func<string, string>) (type => type.Trim()));
        allowedTypes = source.Where<string>((Func<string, bool>) (x => !x.StartsWith("!")));
        disallowedTypes = source.Where<string>((Func<string, bool>) (x => x.StartsWith("!")));
      }
      foreach (MyEntity tmpEntity in MyVisualScriptLogicProvider.m_tmpEntities)
        MyVisualScriptLogicProvider.RemoveEntity(tmpEntity, removeGroup, allowedTypes, disallowedTypes);
    }

    [VisualScriptingMiscData("Entity", "Returns true if specific entity is present in the world.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool EntityExists(string entityName)
    {
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(entityName);
      if (!(entityByName is MyCubeGrid))
        return entityByName != null;
      return entityByName.InScene && !entityByName.MarkedForClose;
    }

    private static MyEntityThrustComponent GetThrustComponentByEntityName(
      string entityName)
    {
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(entityName);
      if (entityByName == null)
        return (MyEntityThrustComponent) null;
      MyComponentBase component = (MyComponentBase) null;
      entityByName.Components.TryGet(typeof (MyEntityThrustComponent), out component);
      return component as MyEntityThrustComponent;
    }

    [VisualScriptingMiscData("Entity", "Returns true if entity has dampeners enabled, false otherwise.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool GetDampenersEnabled(string entityName)
    {
      MyEntityThrustComponent componentByEntityName = MyVisualScriptLogicProvider.GetThrustComponentByEntityName(entityName);
      return componentByEntityName != null && componentByEntityName.DampenersEnabled;
    }

    [VisualScriptingMiscData("Entity", "Turns dampeners of specific entity on/off.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetDampenersEnabled(string entityName, bool state)
    {
      MyEntityThrustComponent componentByEntityName = MyVisualScriptLogicProvider.GetThrustComponentByEntityName(entityName);
      if (componentByEntityName == null)
        return;
      componentByEntityName.DampenersEnabled = state;
    }

    [VisualScriptingMiscData("Entity", "Finds free place around the specified position.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static bool FindFreePlace(
      Vector3D position,
      out Vector3D newPosition,
      float radius,
      int maxTestCount = 20,
      int testsPerDistance = 5,
      float stepSize = 1f)
    {
      Vector3D? freePlace = Sandbox.Game.Entities.MyEntities.FindFreePlace(position, radius, maxTestCount, testsPerDistance, stepSize);
      newPosition = freePlace.HasValue ? freePlace.Value : Vector3D.Zero;
      return freePlace.HasValue;
    }

    [VisualScriptingMiscData("Environment", "Sets time of day.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SunRotationSetTime(float time) => MyTimeOfDayHelper.UpdateTimeOfDay(time);

    [VisualScriptingMiscData("Environment", "Enables/disable sun rotation.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SunRotationEnabled(bool enabled) => MySession.Static.Settings.EnableSunRotation = enabled;

    [VisualScriptingMiscData("Environment", "Sets length of day.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SunRotationSetDayLength(float length) => MySession.Static.GetComponent<MySectorWeatherComponent>().RotationInterval = length;

    [VisualScriptingMiscData("Environment", "Gets length of day.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static float SunRotationGetDayLength() => MySession.Static.GetComponent<MySectorWeatherComponent>().RotationInterval;

    [VisualScriptingMiscData("Environment", "Gets current time of day.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static float SunRotationGetCurrentTime() => MyTimeOfDayHelper.TimeOfDay;

    [VisualScriptingMiscData("Environment", "Gets current sun rotation.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static Vector3 GetSunDirection() => MySector.SunProperties.SunDirectionNormalized;

    [VisualScriptingMiscData("Environment", "Returns true if position is on dark side of a planet.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsOnDarkSide(MyPlanet planet, Vector3D position) => MySectorWeatherComponent.IsThereNight(planet, ref position);

    [VisualScriptingMiscData("Environment", "Sets density, multiplier, color, skybox multiplier, and atmosphere multiplier of fog.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void FogSetAll(
      float? density,
      float? multiplier,
      Vector3? color,
      float? skyboxMultiplier = 0.0f,
      float? atmoMultiplier = 0.0f)
    {
      float? nullable = density;
      double num1 = nullable.HasValue ? (double) nullable.GetValueOrDefault() : (double) MyFogProperties.Default.FogDensity;
      nullable = multiplier;
      double num2 = nullable.HasValue ? (double) nullable.GetValueOrDefault() : (double) MyFogProperties.Default.FogMultiplier;
      Vector3 vector3 = color ?? MyFogProperties.Default.FogColor;
      nullable = skyboxMultiplier;
      double num3 = nullable.HasValue ? (double) nullable.GetValueOrDefault() : (double) MyFogProperties.Default.FogSkybox;
      nullable = atmoMultiplier;
      double num4 = nullable.HasValue ? (double) nullable.GetValueOrDefault() : (double) MyFogProperties.Default.FogAtmo;
      EndpointId targetEndpoint = new EndpointId();
      Vector3D? position = new Vector3D?();
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<float, float, Vector3, float, float>((Func<IMyEventOwner, Action<float, float, Vector3, float, float>>) (s => new Action<float, float, Vector3, float, float>(MyVisualScriptLogicProvider.FogSetAllInternalAll)), (float) num1, (float) num2, vector3, (float) num3, (float) num4, targetEndpoint, position);
    }

    [Event(null, 3654)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    private static void FogSetAllInternalAll(
      float density,
      float multiplier,
      Vector3 color,
      float skyboxMultiplier = 0.0f,
      float atmoMultiplier = 0.0f)
    {
      MySector.FogProperties.FogMultiplier = multiplier;
      MySector.FogProperties.FogDensity = density;
      MySector.FogProperties.FogColor = color;
      MySector.FogProperties.FogSkybox = skyboxMultiplier;
      MySector.FogProperties.FogAtmo = atmoMultiplier;
    }

    [VisualScriptingMiscData("Environment", "Sets density of fog.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void FogSetDensity(float density)
    {
      MyFogProperties fogProperties = MySector.FogProperties;
      fogProperties.FogDensity = density;
      MyVisualScriptLogicProvider.FogSetAll(new float?(fogProperties.FogDensity), new float?(fogProperties.FogMultiplier), new Vector3?(fogProperties.FogColor), new float?(fogProperties.FogSkybox), new float?(fogProperties.FogAtmo));
    }

    [VisualScriptingMiscData("Environment", "Sets multiplier of fog.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void FogSetMultiplier(float multiplier)
    {
      MyFogProperties fogProperties = MySector.FogProperties;
      fogProperties.FogMultiplier = multiplier;
      MyVisualScriptLogicProvider.FogSetAll(new float?(fogProperties.FogDensity), new float?(fogProperties.FogMultiplier), new Vector3?(fogProperties.FogColor), new float?(fogProperties.FogSkybox), new float?(fogProperties.FogAtmo));
    }

    [VisualScriptingMiscData("Environment", "Sets color of fog.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void FogSetColor(Vector3 color)
    {
      MyFogProperties fogProperties = MySector.FogProperties;
      fogProperties.FogColor = color;
      MyVisualScriptLogicProvider.FogSetAll(new float?(fogProperties.FogDensity), new float?(fogProperties.FogMultiplier), new Vector3?(fogProperties.FogColor), new float?(fogProperties.FogSkybox), new float?(fogProperties.FogAtmo));
    }

    [VisualScriptingMiscData("Environment", "Sets skybox multiplier of fog.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void FogSetSkyboxMultiplier(float skyboxMultiplier)
    {
      MyFogProperties fogProperties = MySector.FogProperties;
      fogProperties.FogSkybox = skyboxMultiplier;
      MyVisualScriptLogicProvider.FogSetAll(new float?(fogProperties.FogDensity), new float?(fogProperties.FogMultiplier), new Vector3?(fogProperties.FogColor), new float?(fogProperties.FogSkybox), new float?(fogProperties.FogAtmo));
    }

    [VisualScriptingMiscData("Environment", "Sets atmosphere multiplier of fog.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void FogSetAtmoMultiplier(float atmoMultiplier)
    {
      MyFogProperties fogProperties = MySector.FogProperties;
      fogProperties.FogAtmo = atmoMultiplier;
      MyVisualScriptLogicProvider.FogSetAll(new float?(fogProperties.FogDensity), new float?(fogProperties.FogMultiplier), new Vector3?(fogProperties.FogColor), new float?(fogProperties.FogSkybox), new float?(fogProperties.FogAtmo));
    }

    [VisualScriptingMiscData("Environment", "Get temperature based on position", -10510688)]
    [VisualScriptingMember(true, false)]
    public static float GetTemperatureInPoint(Vector3D position) => MySectorWeatherComponent.GetTemperatureInPoint(position);

    [VisualScriptingMiscData("Environment", "Creates lightning on exact position", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void CreateLightning(Vector3D position) => MySession.Static.GetComponent<MySectorWeatherComponent>().CreateLightning(position, new MyObjectBuilder_WeatherLightning(), true);

    [VisualScriptingMiscData("Environment", "Set weather based on character position", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetWeatherC(string weatherCommand, string weatherEffect, float radius) => MySession.Static.GetComponent<MySectorWeatherComponent>().SetWeather(weatherEffect, radius, new Vector3D?(), false, (Vector3D) Vector3.Zero, 0, 1f);

    [VisualScriptingMiscData("Environment", "Set weather based on exact position", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetWeatherP(
      string weatherCommand,
      string weatherEffect,
      float radius,
      Vector3D position)
    {
      MySession.Static.GetComponent<MySectorWeatherComponent>().SetWeather(weatherEffect, radius, new Vector3D?(position), false, (Vector3D) Vector3.Zero, 0, 1f);
    }

    [VisualScriptingMiscData("Environment", "Get weather based on position", -10510688)]
    [VisualScriptingMember(true, false)]
    public static string GetWeather(Vector3D position) => MySession.Static.GetComponent<MySectorWeatherComponent>().GetWeather(position);

    [VisualScriptingMiscData("Environment", "Get weather intensity based on position", -10510688)]
    [VisualScriptingMember(true, false)]
    public static float GetWeatherIntensity(Vector3D position) => MySession.Static.GetComponent<MySectorWeatherComponent>().GetWeatherIntensity(position);

    [VisualScriptingMiscData("Environment", "Remove a weather based on position", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void RemoveWeather(Vector3D position) => MySession.Static.GetComponent<MySectorWeatherComponent>().RemoveWeather(position);

    [VisualScriptingMiscData("Environment", "Summons lightning on a position", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void CreateLightning(
      Vector3D position,
      float boltLength,
      byte boltParts,
      short boltVariation,
      float boltRadius,
      short maxLife,
      int damage)
    {
      MySession.Static.GetComponent<MySectorWeatherComponent>().CreateLightning(position, new MyObjectBuilder_WeatherLightning()
      {
        BoltLength = boltLength,
        BoltParts = boltParts,
        BoltVariation = boltVariation,
        BoltRadius = boltRadius,
        MaxLife = maxLife,
        Damage = damage
      }, true);
    }

    [VisualScriptingMiscData("Environment", "Returns true if position is underground", -10510688)]
    [VisualScriptingMember(true, false)]
    public static bool IsUnderGround(Vector3D position)
    {
      MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(position);
      return closestPlanet != null && closestPlanet.IsUnderGround(position);
    }

    [VisualScriptingMiscData("Factions", "Gets id of local player. Works only on Lobby and clients. On Dedicated server returns 0.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static long GetLocalPlayerId() => MySession.Static.LocalPlayerId;

    [VisualScriptingMiscData("Factions", "Gets id of pirate faction.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static long GetPirateId() => MyPirateAntennas.GetPiratesId();

    [VisualScriptingMiscData("Factions", "Gets tag of faction, specific player is in.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static string GetPlayersFactionTag(long playerId = 0)
    {
      if (playerId <= 0L)
        playerId = MySession.Static.LocalPlayerId;
      return !(MySession.Static.Factions.TryGetPlayerFaction(playerId) is MyFaction playerFaction) ? "" : playerFaction.Tag;
    }

    [VisualScriptingMiscData("Factions", "Gets name of faction, specific player is in.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static string GetPlayersFactionName(long playerId = 0)
    {
      if (playerId <= 0L)
        playerId = MySession.Static.LocalPlayerId;
      return !(MySession.Static.Factions.TryGetPlayerFaction(playerId) is MyFaction playerFaction) ? "" : playerFaction.Name;
    }

    [VisualScriptingMiscData("Factions", "Forces join player into a faction specified by tag. Returns false if faction does not exist, true otherwise. If player was in any faction before, he will be removed from that faction.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static bool SetPlayersFaction(long playerId = 0, string factionTag = "")
    {
      if (playerId <= 0L)
        playerId = MySession.Static.LocalPlayerId;
      MyFaction factionByTag = MySession.Static.Factions.TryGetFactionByTag(factionTag, (IMyFaction) null);
      if (factionByTag == null)
        return false;
      MyVisualScriptLogicProvider.KickPlayerFromFaction(playerId);
      MyFactionCollection.SendJoinRequest(factionByTag.FactionId, playerId);
      return true;
    }

    [VisualScriptingMiscData("Factions", "Returns list of all members (of theirs ids) of specific faction.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static List<long> GetFactionMembers(string factionTag = "")
    {
      List<long> longList = new List<long>();
      MyFaction factionByTag = MySession.Static.Factions.TryGetFactionByTag(factionTag, (IMyFaction) null);
      if (factionByTag == null)
        return longList;
      foreach (KeyValuePair<long, MyFactionMember> member in factionByTag.Members)
        longList.Add(member.Key);
      return longList;
    }

    [VisualScriptingMiscData("Factions", "Kicks specific player from faction he is in.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void KickPlayerFromFaction(long playerId = 0)
    {
      if (playerId <= 0L)
        playerId = MySession.Static.LocalPlayerId;
      MyFactionCollection.KickMember(MySession.Static.Factions.TryGetPlayerFaction(playerId) is MyFaction playerFaction ? playerFaction.FactionId : 0L, playerId);
    }

    [VisualScriptingMiscData("Factions", "Creates new faction.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void CreateFaction(
      long founderId,
      string factionTag,
      string factionName = "",
      string factionDescription = "",
      string factionPrivateText = "")
    {
      MySession.Static.Factions.CreateFaction(founderId, factionTag, factionName, factionDescription, factionPrivateText, MyFactionTypes.None, new Vector3(), new Vector3(), new SerializableDefinitionId?(), 0);
    }

    [VisualScriptingMiscData("Factions", "Returns true if specified two factions are enemies.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool AreFactionsEnemies(string firstFactionTag, string secondFactionTag)
    {
      MyFaction factionByTag1 = MySession.Static.Factions.TryGetFactionByTag(firstFactionTag, (IMyFaction) null);
      if (factionByTag1 == null)
        return false;
      MyFaction factionByTag2 = MySession.Static.Factions.TryGetFactionByTag(firstFactionTag, (IMyFaction) null);
      return factionByTag2 != null && MySession.Static.Factions.AreFactionsEnemies(factionByTag1.FactionId, factionByTag2.FactionId);
    }

    [VisualScriptingMiscData("Factions", "Returns current reputation between two factions. Returns int.MinValue (-2147483648) if either of factions is not found.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static int GetRelationBetweenFactions(string tagA, string tagB)
    {
      MyFaction factionByTag1 = MySession.Static.Factions.TryGetFactionByTag(tagA, (IMyFaction) null);
      MyFaction factionByTag2 = MySession.Static.Factions.TryGetFactionByTag(tagB, (IMyFaction) null);
      return factionByTag1 == null || factionByTag2 == null ? int.MinValue : MySession.Static.Factions.GetRelationBetweenFactions(factionByTag1.FactionId, factionByTag2.FactionId).Item2;
    }

    [VisualScriptingMiscData("Factions", "Returns current reputation between player and faction. Returns int.MinValue (-2147483648) if player or faction is not found.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static int GetRelationBetweenPlayerAndFaction(long playerId, string tagB)
    {
      int num = MySession.Static.Players.TryGetPlayerId(playerId, out MyPlayer.PlayerId _) ? 1 : 0;
      MyFaction factionByTag = MySession.Static.Factions.TryGetFactionByTag(tagB, (IMyFaction) null);
      return num == 0 || factionByTag == null ? int.MinValue : MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(playerId, factionByTag.FactionId).Item2;
    }

    [VisualScriptingMiscData("Factions", "Set reputation between two factions. Reputation will be automatically clamped to allwed range.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetRelationBetweenFactions(string tagA, string tagB, int reputation)
    {
      MyFaction factionByTag1 = MySession.Static.Factions.TryGetFactionByTag(tagA, (IMyFaction) null);
      MyFaction factionByTag2 = MySession.Static.Factions.TryGetFactionByTag(tagB, (IMyFaction) null);
      if (factionByTag1 == null || factionByTag2 == null)
        return;
      MySession.Static.Factions.SetReputationBetweenFactions(factionByTag1.FactionId, factionByTag2.FactionId, MySession.Static.Factions.ClampReputation(reputation));
    }

    [VisualScriptingMiscData("Factions", "Set reputation between player and faction. Reputation will be automatically clamped to allwed range.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetRelationBetweenPlayerAndFaction(
      long playerId,
      string tagB,
      int reputation)
    {
      int num = MySession.Static.Players.TryGetPlayerId(playerId, out MyPlayer.PlayerId _) ? 1 : 0;
      MyFaction factionByTag = MySession.Static.Factions.TryGetFactionByTag(tagB, (IMyFaction) null);
      if (num == 0 || factionByTag == null)
        return;
      MySession.Static.Factions.SetReputationBetweenPlayerAndFaction(playerId, factionByTag.FactionId, MySession.Static.Factions.ClampReputation(reputation));
    }

    [VisualScriptingMiscData("Gameplay", "Returns true if world is creative.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsCreative() => MySession.Static.CreativeMode;

    [VisualScriptingMiscData("Gameplay", "Returns true if world is survival.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsSurvival() => MySession.Static.SurvivalMode;

    [VisualScriptingMiscData("Gameplay", "Returns simulation quality (0 normal, 1 low, 2 verylow) currect platform can handle", -10510688)]
    [VisualScriptingMember(false, false)]
    public static int GetSimulationQuality()
    {
      SimulationQuality? simulationQualityOverride = MyPlatformGameSettings.VST_SIMULATION_QUALITY_OVERRIDE;
      switch (simulationQualityOverride.HasValue ? (int) simulationQualityOverride.GetValueOrDefault() : (int) MyVRage.Platform.System.SimulationQuality)
      {
        case 0:
          return 0;
        case 1:
          return 1;
        case 2:
          return 2;
        default:
          throw new InvalidBranchException();
      }
    }

    [VisualScriptingMiscData("Gameplay", "Enables terminal screen.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void EnableTerminal(bool flag) => MyPerGameSettings.GUI.EnableTerminalScreen = flag;

    [VisualScriptingMiscData("Gameplay", "Saves the game.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static bool SaveSession()
    {
      if (MyAsyncSaving.InProgress)
        return false;
      MyAsyncSaving.Start();
      return true;
    }

    [VisualScriptingMiscData("Gameplay", "Saves the game under specific name.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static bool SaveSessionAs(string saveName)
    {
      if (MyAsyncSaving.InProgress)
        return false;
      MyAsyncSaving.Start(customName: MyStatControlText.SubstituteTexts(saveName));
      return true;
    }

    [VisualScriptingMiscData("Gameplay", "Allows all players to save", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void EnableSaving(bool enable) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<bool>((Func<IMyEventOwner, Action<bool>>) (s => new Action<bool>(MyVisualScriptLogicProvider.EnableSavingSync)), enable);

    [Event(null, 4032)]
    [Reliable]
    [Broadcast]
    [Server]
    private static void EnableSavingSync(bool enable) => MySession.Static.Settings.EnableSaving = enable;

    public static void EnableSavingLocal(bool enable) => MyVisualScriptLogicProvider.EnableSavingSync(enable);

    [VisualScriptingMiscData("Gameplay", "Allows all players to save", -10510688)]
    public static bool IsSavingEnabled() => MySession.Static.Settings.EnableSaving;

    [VisualScriptingMiscData("Gameplay", "Provides information if game is currently being saved", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsGameSaving() => MyAsyncSaving.InProgress;

    [VisualScriptingMiscData("Gameplay", "Displays reload dialog with specific caption and message to load save defined by path.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SessionReloadDialog(string caption, string message, string savePath = null)
    {
      StringBuilder messageCaption = new StringBuilder(caption);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: new StringBuilder(message), messageCaption: messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result =>
      {
        if (result == MyGuiScreenMessageBox.ResultEnum.YES)
          MySessionLoader.LoadSingleplayerSession(savePath ?? MySession.Static.CurrentPath, contextName: MyCampaignManager.Static.ActiveCampaignName);
        else
          MySessionLoader.UnloadAndExitToMenu();
      }))));
    }

    [VisualScriptingMiscData("Gameplay", "Closes active session after the specific time (in ms).", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SessionClose(int fadeTimeMs = 10000, bool showCredits = true, bool closeSession = true)
    {
      if (fadeTimeMs < 0)
        fadeTimeMs = 10000;
      if (!(closeSession | showCredits))
        return;
      if (!MyCampaignManager.Static.IsCampaignRunning)
      {
        MyGuiScreenFade myGuiScreenFade = new MyGuiScreenFade(Color.Black, (uint) fadeTimeMs, 0U);
        myGuiScreenFade.Shown += (Action<MyGuiScreenFade>) (source => MySandboxGame.Static.Invoke((Action) (() =>
        {
          if (closeSession)
            MySessionLoader.UnloadAndExitToMenu();
          if (!showCredits)
            return;
          MyScreenManager.AddScreen((MyGuiScreenBase) new MyGuiScreenGameCredits());
        }), "MyVisualScriptLogicProvider::SessionClose"));
        MyHud.MinimalHud = true;
        MyScreenManager.AddScreen((MyGuiScreenBase) myGuiScreenFade);
      }
      else
      {
        MyGuiScreenFade myGuiScreenFade = new MyGuiScreenFade(Color.Black, (uint) fadeTimeMs, 0U);
        myGuiScreenFade.Shown += (Action<MyGuiScreenFade>) (source => MySandboxGame.Static.Invoke((Action) (() => MySession.Static.GetComponent<MyCampaignSessionComponent>().LoadNextCampaignMission(closeSession, showCredits)), "MyVisualScriptLogicProvider::SessionClose"));
        MyHud.MinimalHud = true;
        MyScreenManager.AddScreen((MyGuiScreenBase) myGuiScreenFade);
      }
    }

    [VisualScriptingMiscData("Gameplay", "Reloads last checkpoint while displaying message on screen.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SessionReloadLastCheckpoint(
      int fadeTimeMs = 10000,
      string message = null,
      float textScale = 1f,
      string font = "Blue")
    {
      if (fadeTimeMs < 0)
        fadeTimeMs = 10000;
      if (MySession.Static.LocalCharacter != null)
        MySession.Static.LocalCharacter.DeactivateRespawn();
      MyGuiScreenFade myGuiScreenFade = new MyGuiScreenFade(Color.Black, (uint) fadeTimeMs, 0U);
      myGuiScreenFade.Shown += (Action<MyGuiScreenFade>) (fade =>
      {
        MySessionLoader.LoadSingleplayerSession(MySession.Static.CurrentPath, contextName: MyCampaignManager.Static.ActiveCampaignName);
        MyHud.MinimalHud = false;
      });
      if (!string.IsNullOrEmpty(message))
      {
        StringBuilder stringBuilder1 = MyTexts.SubstituteTexts(new StringBuilder(message));
        MyGuiControls controls = myGuiScreenFade.Controls;
        Vector2? position = new Vector2?(new Vector2(0.5f));
        Vector2? size = new Vector2?(new Vector2(0.6f, 0.3f));
        Vector4? backgroundColor = new Vector4?();
        StringBuilder stringBuilder2 = stringBuilder1;
        double num = (double) textScale;
        StringBuilder contents = stringBuilder2;
        int? visibleLinesCount = new int?();
        MyGuiBorderThickness? textPadding = new MyGuiBorderThickness?();
        MyGuiControlMultilineText controlMultilineText = new MyGuiControlMultilineText(position, size, backgroundColor, "Red", (float) num, contents: contents, visibleLinesCount: visibleLinesCount, textPadding: textPadding);
        controls.Add((MyGuiControlBase) controlMultilineText);
      }
      MyHud.MinimalHud = true;
      MyScreenManager.AddScreen((MyGuiScreenBase) myGuiScreenFade);
    }

    [VisualScriptingMiscData("Gameplay", "Displays player the dialog to exit game to main menu (for non-campaign) or continue next campaign mission (for campaign).", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SessionExitGameDialog(string caption, string message)
    {
      if (MyVisualScriptLogicProvider.m_exitGameDialogOpened)
        return;
      MyVisualScriptLogicProvider.m_exitGameDialogOpened = true;
      StringBuilder messageCaption = new StringBuilder(caption);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, messageText: new StringBuilder(message), messageCaption: messageCaption, okButtonText: new MyStringId?(MyCampaignManager.Static.IsCampaignRunning ? MyCommonTexts.ScreenMenuButtonContinue : MyCommonTexts.ScreenMenuButtonExitToMainMenu), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result =>
      {
        if (result == MyGuiScreenMessageBox.ResultEnum.YES)
        {
          if (MyCampaignManager.Static.IsCampaignRunning)
            MySession.Static.GetComponent<MyCampaignSessionComponent>().LoadNextCampaignMission(true, false);
          else
            MySessionLoader.UnloadAndExitToMenu();
        }
        MyVisualScriptLogicProvider.m_exitGameDialogOpened = false;
      }))));
    }

    [VisualScriptingMiscData("Gameplay", "Gets path of the session (game/mission) currently being played.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static string GetCurrentSessionPath() => MySession.Static.CurrentPath;

    [VisualScriptingMiscData("Gameplay", "Returns true if session is fully loaded.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsGameLoaded() => MyVisualScriptLogicProvider.GameIsReady;

    [VisualScriptingMiscData("Gameplay", "[Obsolete, use SetMissionOutcome] Sets the state of campaign. Necessary for transitions between missions in campaign.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetCampaignLevelOutcome(string outcome) => MyVisualScriptLogicProvider.SetMissionOutcome(outcome);

    [VisualScriptingMiscData("Gameplay", "Finishes active mission (state Mission Complete) with fadeout (ms).", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void FinishMission(string outcome = "Mission Complete", int fadeTimeMs = 5000)
    {
      MyVisualScriptLogicProvider.SetMissionOutcome(outcome);
      MyVisualScriptLogicProvider.SessionClose(fadeTimeMs);
    }

    [VisualScriptingMiscData("Gameplay", "Sets the state of the mission. Necessary for transitions between missions in the scenario.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetMissionOutcome(string outcome = "Mission Complete")
    {
      MyCampaignSessionComponent component = MySession.Static.GetComponent<MyCampaignSessionComponent>();
      if (component == null)
        return;
      component.CampaignLevelOutcome = outcome;
    }

    [VisualScriptingMember(false, false)]
    [VisualScriptingMiscData("Gameplay", "Get Steam ID from player ID.", -10510688)]
    public static ulong GetSteamId(long playerId) => MySession.Static != null ? MySession.Static.Players.TryGetSteamId(playerId) : 0UL;

    [VisualScriptingMember(true, false)]
    [VisualScriptingMiscData("Gameplay", "Disconnect player.", -10510688)]
    public static void DisconnectPlayer(ulong steamId)
    {
      if (MySession.Static == null || Sandbox.Engine.Multiplayer.MyMultiplayer.Static == null)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.Static.DisconnectClient(steamId);
    }

    [VisualScriptingMember(true, false)]
    [VisualScriptingMiscData("Gameplay", "Kick player.", -10510688)]
    public static void KickPlayer(ulong steamId, bool kick = true, bool add = true)
    {
      if (MySession.Static == null || Sandbox.Engine.Multiplayer.MyMultiplayer.Static == null)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.Static.KickClient(steamId, kick, add);
    }

    [VisualScriptingMember(true, false)]
    [VisualScriptingMiscData("Gameplay", "Enable custom respawn. Needed for PlayerRequestsRespawn to work properly.", -10510688)]
    public static void EnableCustomRespawn(bool enable = true, long playerId = -1)
    {
      long num = playerId != 0L || MySession.Static.LocalCharacter == null ? playerId : MySession.Static.LocalCharacter.GetPlayerIdentityId();
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<bool, long>((Func<IMyEventOwner, Action<bool, long>>) (s => new Action<bool, long>(MyVisualScriptLogicProvider.EnableCustomRespawnSync)), enable, num);
    }

    [Event(null, 4281)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void EnableCustomRespawnSync(bool enable, long playerId = -1)
    {
      if (playerId != -1L && !Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalPlayerId != playerId)
        return;
      MyCampaignSessionComponent component = MySession.Static.GetComponent<MyCampaignSessionComponent>();
      if (component == null)
        return;
      component.CustomRespawnEnabled = enable;
    }

    public static void CustomRespawnRequest(long playerId) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (s => new Action<long>(MyVisualScriptLogicProvider.CustomRespawnRequestServer)), playerId);

    [Event(null, 4299)]
    [Reliable]
    [Server]
    private static void CustomRespawnRequestServer(long playerId)
    {
      SingleKeyPlayerEvent playerRequestsRespawn = MyVisualScriptLogicProvider.PlayerRequestsRespawn;
      if (playerRequestsRespawn == null)
        return;
      playerRequestsRespawn(playerId);
    }

    [VisualScriptingMember(true, false)]
    [VisualScriptingMiscData("Gameplay", "Stop dedicated server (autorestart if enabled).", -10510688)]
    public static void StopDedicatedServer()
    {
      if (!Sync.IsDedicated)
        return;
      MySandboxGame.Log.WriteLineAndConsole("The script stopped DS.");
      MySandboxGame.ExitThreadSafe();
    }

    [VisualScriptingMiscData("Input", "Checks if input control has been pressed this frame.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsNewGameControlPressed(string controlStringId)
    {
      if (!MyInput.Static.IsJoystickLastUsed)
        return MyInput.Static.IsNewGameControlPressed(MyStringId.GetOrCompute(controlStringId));
      MyStringId context1 = MySession.Static.ControlledEntity != null ? MySession.Static.ControlledEntity.ControlContext : MyStringId.NullOrEmpty;
      MyStringId context2 = MySession.Static.ControlledEntity != null ? MySession.Static.ControlledEntity.AuxiliaryContext : MyStringId.NullOrEmpty;
      MyStringId orCompute = MyStringId.GetOrCompute(controlStringId);
      return MyControllerHelper.IsControl(context1, orCompute) || MyControllerHelper.IsControl(context2, MyStringId.GetOrCompute(controlStringId));
    }

    [VisualScriptingMiscData("Input", "Checks if input control is currently pressed.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsGameControlPressed(string controlStringId)
    {
      if (!MyInput.Static.IsJoystickLastUsed)
        return MyInput.Static.IsGameControlPressed(MyStringId.GetOrCompute(controlStringId));
      MyStringId context1 = MySession.Static.ControlledEntity != null ? MySession.Static.ControlledEntity.ControlContext : MyStringId.NullOrEmpty;
      MyStringId context2 = MySession.Static.ControlledEntity != null ? MySession.Static.ControlledEntity.AuxiliaryContext : MyStringId.NullOrEmpty;
      MyStringId orCompute = MyStringId.GetOrCompute(controlStringId);
      return MyControllerHelper.IsControl(context1, orCompute, MyControlStateType.PRESSED) || MyControllerHelper.IsControl(context2, MyStringId.GetOrCompute(controlStringId), MyControlStateType.PRESSED);
    }

    [VisualScriptingMiscData("Input", "Checks if input control has been released this frame.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsNewGameControlReleased(string controlStringId)
    {
      if (!MyInput.Static.IsJoystickLastUsed)
        return MyInput.Static.IsNewGameControlReleased(MyStringId.GetOrCompute(controlStringId));
      MyStringId context1 = MySession.Static.ControlledEntity != null ? MySession.Static.ControlledEntity.ControlContext : MyStringId.NullOrEmpty;
      MyStringId context2 = MySession.Static.ControlledEntity != null ? MySession.Static.ControlledEntity.AuxiliaryContext : MyStringId.NullOrEmpty;
      MyStringId orCompute = MyStringId.GetOrCompute(controlStringId);
      return MyControllerHelper.IsControl(context1, orCompute, MyControlStateType.NEW_RELEASED) || MyControllerHelper.IsControl(context2, MyStringId.GetOrCompute(controlStringId), MyControlStateType.NEW_RELEASED);
    }

    [VisualScriptingMiscData("Input", "Checks if input control is currently released.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsGameControlReleased(string controlStringId)
    {
      if (!MyInput.Static.IsJoystickLastUsed)
        return MyInput.Static.IsGameControlReleased(MyStringId.GetOrCompute(controlStringId));
      MyStringId context1 = MySession.Static.ControlledEntity != null ? MySession.Static.ControlledEntity.ControlContext : MyStringId.NullOrEmpty;
      MyStringId context2 = MySession.Static.ControlledEntity != null ? MySession.Static.ControlledEntity.AuxiliaryContext : MyStringId.NullOrEmpty;
      MyStringId orCompute = MyStringId.GetOrCompute(controlStringId);
      return !MyControllerHelper.IsControl(context1, orCompute, MyControlStateType.PRESSED) || !MyControllerHelper.IsControl(context2, MyStringId.GetOrCompute(controlStringId), MyControlStateType.PRESSED);
    }

    [VisualScriptingMiscData("GPS and Highlights", "Enables/disables highlight of specific object for local player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetHighlight(
      [Nullable] string entityName,
      bool enabled = true,
      int thickness = 10,
      int pulseTimeInFrames = 120,
      Color color = default (Color),
      long playerId = -1,
      [Nullable] string subPartNames = null)
    {
      thickness = enabled ? thickness : -1;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, int, int, Color, long, string>((Func<IMyEventOwner, Action<string, int, int, Color, long, string>>) (x => new Action<string, int, int, Color, long, string>(MyVisualScriptLogicProvider.SetHighlightSync)), entityName, thickness, pulseTimeInFrames, color, playerId, subPartNames);
    }

    [Event(null, 4425)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void SetHighlightSync(
      [Nullable] string entityName,
      int thickness = 10,
      int pulseTimeInFrames = 120,
      Color color = default (Color),
      long playerId = -1,
      [Nullable] string subPartNames = null)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity))
        return;
      if (color == new Color())
        color = MyVisualScriptLogicProvider.DEFAULT_HIGHLIGHT_COLOR;
      if (playerId == -1L)
        playerId = MyVisualScriptLogicProvider.GetLocalPlayerId();
      MyVisualScriptLogicProvider.SetHighlight(new MyHighlightData()
      {
        EntityId = entity.EntityId,
        OutlineColor = new Color?(color),
        PulseTimeInFrames = (ulong) pulseTimeInFrames,
        Thickness = thickness,
        PlayerId = playerId,
        IgnoreUseObjectData = subPartNames == null,
        SubPartNames = string.IsNullOrEmpty(subPartNames) ? "" : subPartNames
      }, playerId);
    }

    public static void SetHighlightLocal(
      string entityName,
      int thickness = 10,
      int pulseTimeInFrames = 120,
      Color color = default (Color),
      long playerId = -1,
      string subPartNames = null)
    {
      MyVisualScriptLogicProvider.SetHighlightSync(entityName, thickness, pulseTimeInFrames, color, playerId, subPartNames);
    }

    [VisualScriptingMiscData("GPS and Highlights", "Enables/disables highlight of specific object for local player. You can set alpha of color too.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetAlphaHighlight(
      string entityName,
      bool enabled = true,
      int thickness = 10,
      int pulseTimeInFrames = 120,
      Color color = default (Color),
      long playerId = -1,
      string subPartNames = null,
      float alpha = 1f)
    {
      Color color1 = color;
      color1.A = (byte) ((double) alpha * (double) byte.MaxValue);
      MyVisualScriptLogicProvider.SetHighlight(entityName, enabled, thickness, pulseTimeInFrames, color1, playerId, subPartNames);
    }

    [VisualScriptingMiscData("GPS and Highlights", "Enables/disables highlight of specific object for all players.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetHighlightForAll(
      string entityName,
      bool enabled = true,
      int thickness = 10,
      int pulseTimeInFrames = 120,
      Color color = default (Color),
      string subPartNames = null)
    {
      ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
      if (onlinePlayers == null || onlinePlayers.Count == 0)
        return;
      foreach (MyPlayer myPlayer in (IEnumerable<MyPlayer>) onlinePlayers)
        MyVisualScriptLogicProvider.SetHighlight(entityName, enabled, thickness, pulseTimeInFrames, color, myPlayer.Identity.IdentityId, subPartNames);
    }

    [VisualScriptingMiscData("GPS and Highlights", "Enables/disables highlight of specific object for all players. You can set alpha of color too.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetAlphaHighlightForAll(
      string entityName,
      bool enabled = true,
      int thickness = 10,
      int pulseTimeInFrames = 120,
      Color color = default (Color),
      string subPartNames = null,
      float alpha = 1f)
    {
      Color color1 = color;
      color1.A = (byte) ((double) alpha * (double) byte.MaxValue);
      MyVisualScriptLogicProvider.SetHighlightForAll(entityName, enabled, thickness, pulseTimeInFrames, color1, subPartNames);
    }

    [VisualScriptingMiscData("GPS and Highlights", "Enables/disables highlight for specific entity and creates/deletes GPS attached to it. For local player only.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetGPSHighlight(
      string entityName,
      string GPSName,
      string GPSDescription,
      Color GPSColor,
      bool enabled = true,
      int thickness = 10,
      int pulseTimeInFrames = 120,
      Color color = default (Color),
      long playerId = -1,
      string subPartNames = null)
    {
      MyVisualScriptLogicProvider.SetGPSHighlightInternal(entityName, GPSName, GPSDescription, GPSColor, enabled, thickness, pulseTimeInFrames, color, playerId, subPartNames);
    }

    [VisualScriptingMiscData("GPS and Highlights", "Enables/disables highlight for specific entity and creates/deletes GPS attached to it. For all players.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetGPSHighlightForAll(
      string entityName,
      string GPSName,
      string GPSDescription,
      Color GPSColor,
      bool enabled = true,
      int thickness = 10,
      int pulseTimeInFrames = 120,
      Color color = default (Color),
      string subPartNames = null)
    {
      ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
      if (onlinePlayers == null || onlinePlayers.Count == 0)
        return;
      foreach (MyPlayer myPlayer in (IEnumerable<MyPlayer>) onlinePlayers)
        MyVisualScriptLogicProvider.SetGPSHighlight(entityName, GPSName, GPSDescription, GPSColor, enabled, thickness, pulseTimeInFrames, color, myPlayer.Identity.IdentityId, subPartNames);
    }

    [VisualScriptingMiscData("GPS and Highlights", "Enables/disables highlight for specific entity and creates/deletes GPS attached to it. For local player only.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetGPSHighlightNoSound(
      string entityName,
      string GPSName,
      string GPSDescription,
      Color GPSColor,
      bool enabled = true,
      int thickness = 10,
      int pulseTimeInFrames = 120,
      Color color = default (Color),
      long playerId = -1,
      string subPartNames = null)
    {
      MyVisualScriptLogicProvider.SetGPSHighlightInternal(entityName, GPSName, GPSDescription, GPSColor, enabled, thickness, pulseTimeInFrames, color, playerId, subPartNames, false);
    }

    [VisualScriptingMiscData("GPS and Highlights", "Enables/disables highlight for specific entity and creates/deletes GPS attached to it. For all players.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetGPSHighlightNoSoundForAll(
      string entityName,
      string GPSName,
      string GPSDescription,
      Color GPSColor,
      bool enabled = true,
      int thickness = 10,
      int pulseTimeInFrames = 120,
      Color color = default (Color),
      string subPartNames = null)
    {
      ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
      if (onlinePlayers == null || onlinePlayers.Count == 0)
        return;
      foreach (MyPlayer myPlayer in (IEnumerable<MyPlayer>) onlinePlayers)
        MyVisualScriptLogicProvider.SetGPSHighlightNoSound(entityName, GPSName, GPSDescription, GPSColor, enabled, thickness, pulseTimeInFrames, color, myPlayer.Identity.IdentityId, subPartNames);
    }

    private static void SetGPSHighlightInternal(
      string entityName,
      string GPSName,
      string GPSDescription,
      Color GPSColor,
      bool enabled = true,
      int thickness = 10,
      int pulseTimeInFrames = 120,
      Color color = default (Color),
      long playerId = -1,
      string subPartNames = null,
      bool playSound = true)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity))
        return;
      if (playerId == -1L)
        playerId = MyVisualScriptLogicProvider.GetLocalPlayerId();
      MyTuple<string, string> myTuple = new MyTuple<string, string>(entityName, GPSName);
      if (enabled)
      {
        MyGps gps = new MyGps()
        {
          ShowOnHud = true,
          Name = GPSName,
          Description = GPSDescription,
          AlwaysVisible = true,
          IsObjective = true
        };
        if (GPSColor != Color.Transparent)
          gps.GPSColor = GPSColor;
        MySession.Static.Gpss.SendAddGps(playerId, ref gps, entity.EntityId, playSound);
      }
      else
      {
        IMyGps gpsByName = MySession.Static.Gpss.GetGpsByName(playerId, GPSName);
        if (gpsByName != null)
          MySession.Static.Gpss.SendDelete(playerId, gpsByName.Hash);
      }
      if (entity.Model == null)
        return;
      MyVisualScriptLogicProvider.SetHighlight(entityName, enabled, thickness, pulseTimeInFrames, color, playerId, subPartNames);
    }

    public static void SetHighlight(MyHighlightData highlightData, long playerId)
    {
      MyHighlightSystem component = MySession.Static.GetComponent<MyHighlightSystem>();
      bool flag = highlightData.Thickness > -1;
      int exclusiveKey = -1;
      if (MyVisualScriptLogicProvider.m_playerIdsToHighlightData.ContainsKey(playerId))
      {
        exclusiveKey = MyVisualScriptLogicProvider.m_playerIdsToHighlightData[playerId].Find((Predicate<MyTuple<long, int>>) (tuple => tuple.Item1 == highlightData.EntityId)).Item2;
        if (exclusiveKey == 0)
          exclusiveKey = -1;
      }
      if (exclusiveKey == -1)
      {
        if (!flag)
          return;
        component.ExclusiveHighlightAccepted += new Action<MyHighlightData, int>(MyVisualScriptLogicProvider.OnExclusiveHighlightAccepted);
        component.ExclusiveHighlightRejected += new Action<MyHighlightData, int>(MyVisualScriptLogicProvider.OnExclusiveHighlightRejected);
        if (!MyVisualScriptLogicProvider.m_playerIdsToHighlightData.ContainsKey(playerId))
          MyVisualScriptLogicProvider.m_playerIdsToHighlightData.Add(playerId, new List<MyTuple<long, int>>());
        MyVisualScriptLogicProvider.m_playerIdsToHighlightData[playerId].Add(new MyTuple<long, int>(highlightData.EntityId, -1));
      }
      else if (!flag)
        MyVisualScriptLogicProvider.m_playerIdsToHighlightData[playerId].RemoveAll((Predicate<MyTuple<long, int>>) (tuple => tuple.Item2 == exclusiveKey));
      component.RequestHighlightChangeExclusive(highlightData, exclusiveKey);
    }

    private static void OnExclusiveHighlightRejected(MyHighlightData data, int exclusiveKey)
    {
      MyVisualScriptLogicProvider.m_playerIdsToHighlightData[data.PlayerId].RemoveAll((Predicate<MyTuple<long, int>>) (tuple => tuple.Item1 == data.EntityId));
      MySession.Static.GetComponent<MyHighlightSystem>().ExclusiveHighlightAccepted -= new Action<MyHighlightData, int>(MyVisualScriptLogicProvider.OnExclusiveHighlightAccepted);
    }

    private static void OnExclusiveHighlightAccepted(MyHighlightData data, int exclusiveKey)
    {
      if ((double) data.Thickness == -1.0)
        return;
      List<MyTuple<long, int>> myTupleList = MyVisualScriptLogicProvider.m_playerIdsToHighlightData[data.PlayerId];
      int index = myTupleList.FindIndex((Predicate<MyTuple<long, int>>) (tuple => tuple.Item1 == data.EntityId));
      if (index == -1)
        return;
      MyTuple<long, int> myTuple = myTupleList[index];
      MyVisualScriptLogicProvider.m_playerIdsToHighlightData[data.PlayerId][index] = new MyTuple<long, int>(myTuple.Item1, exclusiveKey);
      MySession.Static.GetComponent<MyHighlightSystem>().ExclusiveHighlightRejected -= new Action<MyHighlightData, int>(MyVisualScriptLogicProvider.OnExclusiveHighlightRejected);
    }

    [VisualScriptingMiscData("GPS and Highlights", "Sets color of GPS for specific player. If 'playerId' is less or equal to 0, GPS will be modified for local player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetGPSColor(string name, Color newColor, long playerId = -1)
    {
      IMyGps gpsByName = MySession.Static.Gpss.GetGpsByName(playerId > 0L ? playerId : MySession.Static.LocalPlayerId, name);
      if (gpsByName == null)
        return;
      MySession.Static.Gpss.ChangeColor(playerId > 0L ? playerId : MySession.Static.LocalPlayerId, gpsByName.Hash, newColor);
    }

    [VisualScriptingMiscData("GPS and Highlights", "Adds GPS for specified player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void AddGPS(
      string name,
      string description,
      Vector3D position,
      Color GPSColor,
      int disappearsInS = 0,
      long playerId = -1)
    {
      if (playerId <= 0L)
        playerId = MySession.Static.LocalPlayerId;
      MyGps gps = new MyGps()
      {
        ShowOnHud = true,
        Coords = position,
        Name = name,
        Description = description,
        AlwaysVisible = true
      };
      if (disappearsInS > 0)
      {
        TimeSpan timeSpan = TimeSpan.FromSeconds(MySession.Static.ElapsedPlayTime.TotalSeconds + (double) disappearsInS);
        gps.DiscardAt = new TimeSpan?(timeSpan);
      }
      else
        gps.DiscardAt = new TimeSpan?();
      if (GPSColor != Color.Transparent)
        gps.GPSColor = GPSColor;
      MySession.Static.Gpss.SendAddGps(playerId, ref gps);
    }

    [VisualScriptingMiscData("GPS and Highlights", "Adds GPS for all players.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void AddGPSForAll(
      string name,
      string description,
      Vector3D position,
      Color GPSColor,
      int disappearsInS = 0)
    {
      ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
      if (onlinePlayers == null || onlinePlayers.Count == 0)
        return;
      foreach (MyPlayer myPlayer in (IEnumerable<MyPlayer>) onlinePlayers)
        MyVisualScriptLogicProvider.AddGPS(name, description, position, GPSColor, disappearsInS, myPlayer.Identity.IdentityId);
    }

    [VisualScriptingMiscData("GPS and Highlights", "Removes GPS from specific player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void RemoveGPS(string name, long playerId = -1)
    {
      if (playerId <= 0L)
        playerId = MySession.Static.LocalPlayerId;
      IMyGps gpsByName = MySession.Static.Gpss.GetGpsByName(playerId, name);
      if (gpsByName == null)
        return;
      MySession.Static.Gpss.SendDelete(playerId, gpsByName.Hash);
    }

    [VisualScriptingMiscData("GPS and Highlights", "Removes GPS from all players.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void RemoveGPSForAll(string name)
    {
      ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
      if (onlinePlayers == null || onlinePlayers.Count == 0)
        return;
      foreach (MyPlayer myPlayer in (IEnumerable<MyPlayer>) onlinePlayers)
        MyVisualScriptLogicProvider.RemoveGPS(name, myPlayer.Identity.IdentityId);
    }

    [VisualScriptingMiscData("GPS and Highlights", "Creates GPS and attach it to entity for local player only.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void AddGPSToEntity(
      string entityName,
      string GPSName,
      string GPSDescription,
      Color GPSColor,
      long playerId = -1)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity))
        return;
      if (playerId == -1L)
        playerId = MyVisualScriptLogicProvider.GetLocalPlayerId();
      MyTuple<string, string> myTuple = new MyTuple<string, string>(entityName, GPSName);
      MyGps gps = new MyGps()
      {
        ShowOnHud = true,
        Name = GPSName,
        DisplayName = GPSName,
        Description = GPSDescription,
        AlwaysVisible = true
      };
      if (GPSColor != Color.Transparent)
        gps.GPSColor = GPSColor;
      gps.DiscardAt = new TimeSpan?();
      MySession.Static.Gpss.SendAddGps(playerId, ref gps, entity.EntityId);
    }

    [VisualScriptingMiscData("GPS and Highlights", "Creates GPS and attach it to entity for all players", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void AddGPSToEntityForAll(
      string entityName,
      string GPSName,
      string GPSDescription,
      Color GPSColor)
    {
      ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
      if (onlinePlayers == null || onlinePlayers.Count == 0)
        return;
      foreach (MyPlayer myPlayer in (IEnumerable<MyPlayer>) onlinePlayers)
        MyVisualScriptLogicProvider.AddGPSToEntity(entityName, GPSName, GPSDescription, GPSColor, myPlayer.Identity.IdentityId);
    }

    [VisualScriptingMiscData("GPS and Highlights", "Removes specific GPS from specific entity for local player only. ('GPSDescription' is not used. Cant remove due to backward compatibility.)", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void RemoveGPSFromEntity(
      string entityName,
      string GPSName,
      string GPSDescription,
      long playerId = -1)
    {
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out MyEntity _))
        return;
      if (playerId == -1L)
        playerId = MyVisualScriptLogicProvider.GetLocalPlayerId();
      MyTuple<string, string> myTuple = new MyTuple<string, string>(entityName, GPSName);
      IMyGps gpsByName = MySession.Static.Gpss.GetGpsByName(playerId, GPSName);
      if (gpsByName == null)
        return;
      MySession.Static.Gpss.SendDelete(playerId, gpsByName.Hash);
    }

    [VisualScriptingMiscData("GPS and Highlights", "Removes specific GPS from specific entity for all players. ('GPSDescription' is not used. Cant remove due to backward compatibility.)", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void RemoveGPSFromEntityForAll(
      string entityName,
      string GPSName,
      string GPSDescription)
    {
      ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
      if (onlinePlayers == null || onlinePlayers.Count == 0)
        return;
      foreach (MyPlayer myPlayer in (IEnumerable<MyPlayer>) onlinePlayers)
        MyVisualScriptLogicProvider.RemoveGPSFromEntity(entityName, GPSName, GPSDescription, myPlayer.Identity.IdentityId);
    }

    [VisualScriptingMiscData("GPS and Highlights", "Adds GPS for specified player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void AddGPSObjective(
      string name,
      string description,
      Vector3D position,
      Color GPSColor,
      int disappearsInS = 0,
      long playerId = -1)
    {
      if (playerId <= 0L)
        playerId = MySession.Static.LocalPlayerId;
      MyGps gps = new MyGps()
      {
        ShowOnHud = true,
        Coords = position,
        Name = name,
        Description = description,
        AlwaysVisible = true,
        IsObjective = true
      };
      if (disappearsInS > 0)
      {
        TimeSpan timeSpan = TimeSpan.FromSeconds(MySession.Static.ElapsedPlayTime.TotalSeconds + (double) disappearsInS);
        gps.DiscardAt = new TimeSpan?(timeSpan);
      }
      else
        gps.DiscardAt = new TimeSpan?();
      if (GPSColor != Color.Transparent)
        gps.GPSColor = GPSColor;
      MySession.Static.Gpss.SendAddGps(playerId, ref gps);
    }

    [VisualScriptingMiscData("GPS and Highlights", "Adds GPS for all players.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void AddGPSObjectiveForAll(
      string name,
      string description,
      Vector3D position,
      Color GPSColor,
      int disappearsInS = 0)
    {
      ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
      if (onlinePlayers == null || onlinePlayers.Count == 0)
        return;
      foreach (MyPlayer myPlayer in (IEnumerable<MyPlayer>) onlinePlayers)
        MyVisualScriptLogicProvider.AddGPSObjective(name, description, position, GPSColor, disappearsInS, myPlayer.Identity.IdentityId);
    }

    [VisualScriptingMiscData("GPS and Highlights", "Creates GPS and attach it to entity for local player only.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void AddGPSObjectiveToEntity(
      string entityName,
      string GPSName,
      string GPSDescription,
      Color GPSColor,
      long playerId = -1)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity))
        return;
      if (playerId == -1L)
        playerId = MyVisualScriptLogicProvider.GetLocalPlayerId();
      MyTuple<string, string> myTuple = new MyTuple<string, string>(entityName, GPSName);
      MyGps gps = new MyGps()
      {
        ShowOnHud = true,
        Name = GPSName,
        Description = GPSDescription,
        AlwaysVisible = true,
        IsObjective = true
      };
      if (GPSColor != Color.Transparent)
        gps.GPSColor = GPSColor;
      gps.DiscardAt = new TimeSpan?();
      MySession.Static.Gpss.SendAddGps(playerId, ref gps, entity.EntityId);
    }

    [VisualScriptingMiscData("GPS and Highlights", "Creates GPS and attach it to entity for all players", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void AddGPSObjectiveToEntityForAll(
      string entityName,
      string GPSName,
      string GPSDescription,
      Color GPSColor)
    {
      ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
      if (onlinePlayers == null || onlinePlayers.Count == 0)
        return;
      foreach (MyPlayer myPlayer in (IEnumerable<MyPlayer>) onlinePlayers)
        MyVisualScriptLogicProvider.AddGPSObjectiveToEntity(entityName, GPSName, GPSDescription, GPSColor, myPlayer.Identity.IdentityId);
    }

    [VisualScriptingMiscData("Grid", "Returns list of all blocks of type 'blockId' on specific grid.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static List<long> GetIdListOfSpecificGridBlocks(
      string gridName,
      MyDefinitionId blockId)
    {
      List<long> longList = new List<long>();
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(gridName);
      if (entityByName != null && entityByName is MyCubeGrid myCubeGrid)
      {
        foreach (MyCubeBlock fatBlock in myCubeGrid.GetFatBlocks())
        {
          if (fatBlock != null && fatBlock.BlockDefinition != null && fatBlock.BlockDefinition.Id == blockId)
            longList.Add(fatBlock.EntityId);
        }
      }
      return longList;
    }

    [VisualScriptingMiscData("Grid", "Returns sums of current integrities, max integrities, block counts.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool GetGridStatistics(
      string gridName,
      out float currentIntegrity,
      out float maxIntegrity,
      out int blockCount)
    {
      currentIntegrity = 0.0f;
      maxIntegrity = 0.0f;
      blockCount = 0;
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(gridName);
      if (entityByName == null || !(entityByName is MyCubeGrid myCubeGrid))
        return false;
      foreach (MySlimBlock block in myCubeGrid.GetBlocks())
      {
        currentIntegrity += block.Integrity;
        maxIntegrity += block.MaxIntegrity;
      }
      blockCount = myCubeGrid.BlocksCount;
      return true;
    }

    [VisualScriptingMiscData("Grid", "Colors all blocks of specific grid.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void ColorAllGridBlocks(string gridName, Color color, bool playSound = true)
    {
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(gridName);
      if (entityByName == null || !(entityByName is MyCubeGrid myCubeGrid))
        return;
      Vector3 hsvdX11 = color.ColorToHSVDX11();
      myCubeGrid.ColorGrid(hsvdX11, playSound, false);
    }

    [VisualScriptingMiscData("Grid", "Returns entity id of main cockpit or first cockpit found on grid. Also returns other info such as if cockpit is main or if any cockpit was found.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static long GetGridCockpitId(
      string gridName,
      out bool isMainCockpit,
      out bool found,
      bool checkForEnabledShipControl = true)
    {
      isMainCockpit = false;
      found = true;
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(gridName);
      if (entityByName != null && entityByName is MyCubeGrid myCubeGrid)
      {
        if (myCubeGrid.MainCockpit != null)
        {
          isMainCockpit = true;
          return myCubeGrid.MainCockpit.EntityId;
        }
        foreach (MyCockpit fatBlock in myCubeGrid.GetFatBlocks<MyCockpit>())
        {
          if (fatBlock != null && (!checkForEnabledShipControl || fatBlock.EnableShipControl))
            return fatBlock.EntityId;
        }
      }
      found = false;
      return 0;
    }

    [VisualScriptingMiscData("Grid", "Returns count of all blocks of type 'blockId' on specific grid.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static int GetCountOfSpecificGridBlocks(string gridName, MyDefinitionId blockId)
    {
      int num = -2;
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(gridName);
      if (entityByName != null)
      {
        num = -1;
        if (entityByName is MyCubeGrid myCubeGrid)
        {
          num = 0;
          foreach (MyCubeBlock fatBlock in myCubeGrid.GetFatBlocks())
          {
            if (fatBlock != null && fatBlock.BlockDefinition != null && fatBlock.BlockDefinition.Id == blockId)
              ++num;
          }
        }
      }
      return num;
    }

    [VisualScriptingMiscData("Grid", "Returns id of first block of type 'blockId' on specific grid.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static long GetIdOfFirstSpecificGridBlock(string gridName, MyDefinitionId blockId)
    {
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(gridName);
      if (entityByName != null && entityByName is MyCubeGrid myCubeGrid)
      {
        foreach (MyCubeBlock fatBlock in myCubeGrid.GetFatBlocks())
        {
          if (fatBlock != null && fatBlock.BlockDefinition != null && fatBlock.BlockDefinition.Id == blockId)
            return fatBlock.EntityId;
        }
      }
      return 0;
    }

    [VisualScriptingMiscData("Grid", "Sets state of Landing gears for whole grid.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetGridLandingGearsLock(string gridName, bool gearLock = true)
    {
      if (!(MyVisualScriptLogicProvider.GetEntityByName(gridName) is MyCubeGrid entityByName))
        return;
      entityByName.GridSystems.LandingSystem.Switch(gearLock);
    }

    [VisualScriptingMiscData("Grid", "Returns true if any Landing gear of specific grid is in locked state.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsGridLockedWithLandingGear(string gridName)
    {
      if (!(MyVisualScriptLogicProvider.GetEntityByName(gridName) is MyCubeGrid entityByName))
        return false;
      return entityByName.GridSystems.LandingSystem.Locked == MyMultipleEnabledEnum.Mixed || entityByName.GridSystems.LandingSystem.Locked == MyMultipleEnabledEnum.AllEnabled;
    }

    [VisualScriptingMiscData("Grid", "Turns reactors of specific grid on/off.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetGridReactors(string gridName, bool turnOn = true)
    {
      if (!(MyVisualScriptLogicProvider.GetEntityByName(gridName) is MyCubeGrid entityByName))
        return;
      long playerId = -1;
      if (entityByName.BigOwners != null && entityByName.BigOwners.Count > 0)
        playerId = entityByName.BigOwners[0];
      if (turnOn)
        entityByName.GridSystems.SyncObject_PowerProducerStateChanged(MyMultipleEnabledEnum.AllEnabled, playerId);
      else
        entityByName.GridSystems.SyncObject_PowerProducerStateChanged(MyMultipleEnabledEnum.AllDisabled, playerId);
    }

    [VisualScriptingMiscData("Grid", "Enables/disables all weapons(MyUserControllableGun) on the specific grid.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetGridWeaponStatus(string gridName, bool enabled = true)
    {
      if (!(MyVisualScriptLogicProvider.GetEntityByName(gridName) is MyCubeGrid entityByName))
        return;
      foreach (MySlimBlock block in entityByName.GetBlocks())
      {
        if (block.FatBlock is MyUserControllableGun)
          ((MyFunctionalBlock) block.FatBlock).Enabled = enabled;
      }
    }

    [VisualScriptingMiscData("Grid", "Gets number of blocks of specified type on the specific grid.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static int GetNumberOfGridBlocks(
      string entityName,
      string blockTypeId,
      string blockSubtypeId)
    {
      if (!(MyVisualScriptLogicProvider.GetEntityByName(entityName) is MyCubeGrid entityByName))
        return 0;
      int num = 0;
      bool flag1 = !string.IsNullOrEmpty(blockTypeId);
      bool flag2 = !string.IsNullOrEmpty(blockSubtypeId);
      MyObjectBuilderType typeId;
      foreach (MyCubeBlock fatBlock in entityByName.GetFatBlocks())
      {
        if (flag2 & flag1)
        {
          if (fatBlock.BlockDefinition.Id.SubtypeName == blockSubtypeId)
          {
            typeId = fatBlock.BlockDefinition.Id.TypeId;
            if (typeId.ToString() == blockTypeId)
              ++num;
          }
        }
        else if (flag1)
        {
          typeId = fatBlock.BlockDefinition.Id.TypeId;
          if (typeId.ToString() == blockTypeId)
            ++num;
        }
        else if (flag2 && fatBlock.BlockDefinition.Id.SubtypeName == blockSubtypeId)
          ++num;
      }
      return num;
    }

    [VisualScriptingMiscData("Grid", "Returns true if entity has thrusters in all directions.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool HasThrusterInAllDirections(string entityName)
    {
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(entityName);
      if (entityByName == null || !(entityByName is MyCubeGrid myCubeGrid))
        return false;
      MyVisualScriptLogicProvider.ResetThrustDirections();
      foreach (MyThrust fatBlock in myCubeGrid.GetFatBlocks<MyThrust>())
      {
        if (fatBlock.Enabled && (double) Math.Abs(fatBlock.ThrustOverride) < 9.99999974737875E-05 && fatBlock.IsFunctional)
          MyVisualScriptLogicProvider.m_thrustDirections[fatBlock.ThrustForwardVector] = true;
      }
      foreach (bool flag in MyVisualScriptLogicProvider.m_thrustDirections.Values)
      {
        if (!flag)
          return false;
      }
      return true;
    }

    [VisualScriptingMiscData("Grid", "Returns true if grid has enough power or is in 'adaptable-overload'. (grid is overloaded by adaptable block, that won't cause blackout, such as thrusters or batteries)", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool HasPower(string gridName)
    {
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(gridName);
      if (entityByName == null || !(entityByName is MyCubeGrid myCubeGrid))
        return false;
      switch (myCubeGrid.GridSystems.ResourceDistributor.ResourceStateByType(MyResourceDistributorComponent.ElectricityId))
      {
        case MyResourceStateEnum.Ok:
        case MyResourceStateEnum.OverloadAdaptible:
          return true;
        default:
          return false;
      }
    }

    [VisualScriptingMiscData("Grid", "Sets grid's power state.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetGridPowerState(string gridName, bool enabled)
    {
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(gridName);
      if (entityByName == null || !(entityByName is MyCubeGrid myCubeGrid) || myCubeGrid.GridSystems.ResourceDistributor == null)
        return;
      MyMultipleEnabledEnum state = enabled ? MyMultipleEnabledEnum.AllEnabled : MyMultipleEnabledEnum.AllDisabled;
      foreach (long bigOwner in myCubeGrid.BigOwners)
        myCubeGrid.GridSystems.ResourceDistributor.ChangeSourcesState(MyResourceDistributorComponent.ElectricityId, state, bigOwner);
    }

    [VisualScriptingMiscData("Grid", "Sets grid's power state by the specified player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetGridPowerStateByPlayer(string gridName, bool enabled, long playerId)
    {
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(gridName);
      if (entityByName == null || !(entityByName is MyCubeGrid myCubeGrid) || myCubeGrid.GridSystems.ResourceDistributor == null)
        return;
      MyMultipleEnabledEnum state = enabled ? MyMultipleEnabledEnum.AllEnabled : MyMultipleEnabledEnum.AllDisabled;
      myCubeGrid.GridSystems.ResourceDistributor.ChangeSourcesState(MyResourceDistributorComponent.ElectricityId, state, playerId);
    }

    [VisualScriptingMiscData("Grid", "Returns true if the specific grid has at least one gyro that is enabled, powered and not-overridden.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool HasOperationalGyro(string gridName)
    {
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(gridName);
      if (entityByName == null)
        return false;
      MyFatBlockReader<MyGyro> fatBlocks = (entityByName as MyCubeGrid).GetFatBlocks<MyGyro>();
      bool flag = false;
      foreach (MyGyro myGyro in fatBlocks)
      {
        if (myGyro.Enabled && myGyro.IsPowered && !myGyro.GyroOverride)
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    [VisualScriptingMiscData("Grid", "Returns true if the specific grid has at least one cockpit that enables ship control.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool HasOperationalCockpit(string gridName)
    {
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(gridName);
      if (entityByName == null || !(entityByName is MyCubeGrid myCubeGrid))
        return false;
      MyFatBlockReader<MyCockpit> fatBlocks = myCubeGrid.GetFatBlocks<MyCockpit>();
      bool flag = false;
      foreach (MyShipController myShipController in fatBlocks)
      {
        if (myShipController.EnableShipControl)
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    [VisualScriptingMiscData("Grid", "Returns true if the specified grid has at least one Remote in functional state.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool HasWorkingRemote(string gridName)
    {
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(gridName);
      if (entityByName == null || !(entityByName is MyCubeGrid myCubeGrid))
        return false;
      foreach (MyCubeBlock fatBlock in myCubeGrid.GetFatBlocks<MyRemoteControl>())
      {
        if (fatBlock.IsFunctional)
          return true;
      }
      return false;
    }

    [VisualScriptingMiscData("Grid", "Returns true if the specified grid has at least one functional gyro, at least one controlling block (cockpit/remote) and thrusters in all directions.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsFlyable(string entityName)
    {
      if (!(MyVisualScriptLogicProvider.GetEntityByName(entityName) is MyCubeGrid entityByName))
        return false;
      switch (entityByName.GridSystems.ResourceDistributor.ResourceStateByType(MyResourceDistributorComponent.ElectricityId))
      {
        case MyResourceStateEnum.OverloadBlackout:
        case MyResourceStateEnum.NoPower:
          return false;
        default:
          MyFatBlockReader<MyGyro> fatBlocks1 = entityByName.GetFatBlocks<MyGyro>();
          bool flag1 = false;
          foreach (MyGyro myGyro in fatBlocks1)
          {
            if (myGyro.Enabled && myGyro.IsPowered && !myGyro.GyroOverride)
            {
              flag1 = true;
              break;
            }
          }
          if (!flag1)
            return false;
          MyFatBlockReader<MyShipController> fatBlocks2 = entityByName.GetFatBlocks<MyShipController>();
          bool flag2 = false;
          foreach (MyShipController myShipController in fatBlocks2)
          {
            if (myShipController.EnableShipControl)
            {
              flag2 = true;
              break;
            }
          }
          if (!flag2)
            return false;
          MyVisualScriptLogicProvider.ResetThrustDirections();
          foreach (MyThrust fatBlock in entityByName.GetFatBlocks<MyThrust>())
          {
            if (fatBlock.IsPowered && fatBlock.Enabled && (double) Math.Abs(fatBlock.ThrustOverride) < 9.99999974737875E-05)
              MyVisualScriptLogicProvider.m_thrustDirections[fatBlock.ThrustForwardVector] = true;
          }
          foreach (bool flag3 in MyVisualScriptLogicProvider.m_thrustDirections.Values)
          {
            if (!flag3)
              return false;
          }
          return true;
      }
    }

    private static void ResetThrustDirections()
    {
      if (MyVisualScriptLogicProvider.m_thrustDirections.Count == 0)
      {
        MyVisualScriptLogicProvider.m_thrustDirections.Add(Vector3I.Forward, false);
        MyVisualScriptLogicProvider.m_thrustDirections.Add(Vector3I.Backward, false);
        MyVisualScriptLogicProvider.m_thrustDirections.Add(Vector3I.Left, false);
        MyVisualScriptLogicProvider.m_thrustDirections.Add(Vector3I.Right, false);
        MyVisualScriptLogicProvider.m_thrustDirections.Add(Vector3I.Up, false);
        MyVisualScriptLogicProvider.m_thrustDirections.Add(Vector3I.Down, false);
      }
      else
      {
        MyVisualScriptLogicProvider.m_thrustDirections[Vector3I.Forward] = false;
        MyVisualScriptLogicProvider.m_thrustDirections[Vector3I.Backward] = false;
        MyVisualScriptLogicProvider.m_thrustDirections[Vector3I.Left] = false;
        MyVisualScriptLogicProvider.m_thrustDirections[Vector3I.Right] = false;
        MyVisualScriptLogicProvider.m_thrustDirections[Vector3I.Up] = false;
        MyVisualScriptLogicProvider.m_thrustDirections[Vector3I.Down] = false;
      }
    }

    [VisualScriptingMiscData("Grid", "Creates local blueprint for player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void CreateLocalBlueprint(
      string gridName,
      string blueprintName,
      string blueprintDisplayName = null)
    {
      string str = Path.Combine(MyFileSystem.UserDataPath, "Blueprints", "local");
      string path = Path.Combine(str, blueprintName, "bp.sbc");
      if (!MyFileSystem.DirectoryExists(str))
        MyFileSystem.CreateDirectoryRecursive(str);
      if (!MyFileSystem.DirectoryExists(str))
        return;
      if (blueprintDisplayName == null)
        blueprintDisplayName = blueprintName;
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(gridName);
      if (entityByName == null || !(entityByName is MyCubeGrid gridInGroup))
        return;
      MyClipboardComponent.Static.Clipboard.CopyGroup(gridInGroup, GridLinkTypeEnum.Logical);
      MyObjectBuilder_ShipBlueprintDefinition newObject1 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ShipBlueprintDefinition>();
      newObject1.Id = (SerializableDefinitionId) new MyDefinitionId(new MyObjectBuilderType(typeof (MyObjectBuilder_ShipBlueprintDefinition)), MyUtils.StripInvalidChars(blueprintName));
      newObject1.CubeGrids = MyClipboardComponent.Static.Clipboard.CopiedGrids.ToArray();
      newObject1.RespawnShip = false;
      newObject1.DisplayName = blueprintDisplayName;
      newObject1.CubeGrids[0].DisplayName = blueprintDisplayName;
      MyObjectBuilder_Definitions newObject2 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Definitions>();
      newObject2.ShipBlueprints = new MyObjectBuilder_ShipBlueprintDefinition[1];
      newObject2.ShipBlueprints[0] = newObject1;
      MyObjectBuilderSerializer.SerializeXML(path, false, (MyObjectBuilder_Base) newObject2);
      MyClipboardComponent.Static.Clipboard.Deactivate();
    }

    [VisualScriptingMiscData("Grid", "Sets projection highlight for the specific projector block.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetHighlightForProjection(
      string projectorName,
      bool enabled = true,
      int thickness = 5,
      int pulseTimeInFrames = 120,
      Color color = default (Color),
      long playerId = -1)
    {
      MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(projectorName);
      if (entityByName == null || !(entityByName is MyProjectorBase))
        return;
      MyProjectorBase myProjectorBase = (MyProjectorBase) entityByName;
      if (color == new Color())
        color = Color.Blue;
      if (color == new Color())
        color = Color.Blue;
      if (playerId == -1L)
        playerId = MySession.Static.LocalPlayerId;
      MyHighlightData highlightData = new MyHighlightData()
      {
        OutlineColor = new Color?(color),
        PulseTimeInFrames = (ulong) pulseTimeInFrames,
        Thickness = enabled ? thickness : -1,
        PlayerId = playerId,
        IgnoreUseObjectData = true
      };
      foreach (MyCubeGrid previewGrid in myProjectorBase.Clipboard.PreviewGrids)
      {
        highlightData.EntityId = previewGrid.EntityId;
        MyVisualScriptLogicProvider.SetHighlight(highlightData, highlightData.PlayerId);
      }
    }

    [VisualScriptingMiscData("Grid", "Returns true if grid is marked as destructible.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsGridDestructible(string entityName) => !(MyVisualScriptLogicProvider.GetEntityByName(entityName) is MyCubeGrid entityByName) || entityByName.DestructibleBlocks;

    [VisualScriptingMiscData("Grid", "Un/Marks the specific grid as destructible. Such grid cannot be destroyed, but can be grinded", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetGridDestructible(string entityName, bool destructible = true)
    {
      if (!(MyVisualScriptLogicProvider.GetEntityByName(entityName) is MyCubeGrid entityByName))
        return;
      entityByName.DestructibleBlocks = destructible;
    }

    [VisualScriptingMiscData("Grid", "Returns true if grid is marked as destructible.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsGridImmune(string entityName) => !(MyVisualScriptLogicProvider.GetEntityByName(entityName) is MyCubeGrid entityByName) || entityByName.Immune;

    [VisualScriptingMiscData("Grid", "Un/Marks the specific grid as immune. Such grid cannot be destroyed or grinded.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetGridImmune(string entityName, bool immune = true)
    {
      if (!(MyVisualScriptLogicProvider.GetEntityByName(entityName) is MyCubeGrid entityByName))
        return;
      entityByName.Immune = immune;
    }

    [VisualScriptingMiscData("Grid", "Returns true if the specific grid is marked as editable.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsGridEditable(string entityName) => !(MyVisualScriptLogicProvider.GetEntityByName(entityName) is MyCubeGrid entityByName) || entityByName.Editable;

    [VisualScriptingMiscData("Grid", "Un/Marks the specific grid as editable.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetGridEditable(string entityName, bool editable = true)
    {
      if (!(MyVisualScriptLogicProvider.GetEntityByName(entityName) is MyCubeGrid entityByName))
        return;
      entityByName.Editable = editable;
    }

    [VisualScriptingMiscData("Grid", "Sets the specific grid as static/dynamic.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetGridStatic(string gridName, bool isStatic = true)
    {
      if (!(MyVisualScriptLogicProvider.GetEntityByName(gridName) is MyCubeGrid entityByName))
        return;
      if (isStatic)
        entityByName.RequestConversionToStation();
      else
        entityByName.RequestConversionToShip((Action) null);
    }

    [VisualScriptingMiscData("Grid", "Sets grid general damage modifier that multiplies all damage received by that grid.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetGridGeneralDamageModifier(string gridName, float modifier = 1f)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(gridName, out entity) || !(entity is MyCubeGrid))
        return;
      ((MyCubeGrid) entity).GridGeneralDamageModifier.Value = modifier;
    }

    [VisualScriptingMiscData("Grid", "Enables/disables all functional blocks on the specified grid.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetGridBlocksEnabled(string gridName, bool enabled = true)
    {
      if (!(MyVisualScriptLogicProvider.GetEntityByName(gridName) is MyCubeGrid entityByName))
        return;
      foreach (MyCubeBlock fatBlock in entityByName.GetFatBlocks())
      {
        if (fatBlock is MyFunctionalBlock)
          ((MyFunctionalBlock) fatBlock).Enabled = enabled;
      }
    }

    [VisualScriptingMiscData("Grid", "Sets all terminal blocks of specified grid to be (not) shown in terminal screen.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetGridBlocksShowInTerminal(string gridName, bool showInTerminal = true)
    {
      if (!(MyVisualScriptLogicProvider.GetEntityByName(gridName) is MyCubeGrid entityByName))
        return;
      foreach (MyCubeBlock fatBlock in entityByName.GetFatBlocks())
      {
        if (fatBlock is MyTerminalBlock)
          ((MyTerminalBlock) fatBlock).ShowInTerminal = showInTerminal;
      }
    }

    [VisualScriptingMiscData("Grid", "Sets all terminal blocks of specified grid to be (not) shown in inventory screen.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetGridBlocksShowInInventory(string gridName, bool showInInventory = true)
    {
      if (!(MyVisualScriptLogicProvider.GetEntityByName(gridName) is MyCubeGrid entityByName))
        return;
      foreach (MyCubeBlock fatBlock in entityByName.GetFatBlocks())
      {
        if (fatBlock is MyTerminalBlock)
          ((MyTerminalBlock) fatBlock).ShowInInventory = showInInventory;
      }
    }

    [VisualScriptingMiscData("Grid", "Sets all terminal blocks of specified grid to be (not) shown on HUD.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetGridBlocksShowOnHUD(string gridName, bool showOnHUD = true)
    {
      if (!(MyVisualScriptLogicProvider.GetEntityByName(gridName) is MyCubeGrid entityByName))
        return;
      foreach (MyCubeBlock fatBlock in entityByName.GetFatBlocks())
      {
        if (fatBlock is MyTerminalBlock)
          ((MyTerminalBlock) fatBlock).ShowOnHUD = showOnHUD;
      }
    }

    [VisualScriptingMiscData("G-Screen", "Enables/disables toolbar config screen (G-screen).", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void EnableToolbarConfig(bool flag) => MyPerGameSettings.GUI.EnableToolbarConfigScreen = flag;

    [VisualScriptingMiscData("G-Screen", "Sets group mode of toolbar config screen (G-screen) to Hide empty groups.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void ToolbarConfigGroupsHideEmpty() => MyGuiScreenToolbarConfigBase.GroupMode = MyGuiScreenToolbarConfigBase.GroupModes.HideEmpty;

    [VisualScriptingMiscData("G-Screen", "Sets group mode of toolbar config screen (G-screen) to Hide all.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void ToolbarConfigGroupsHideAll() => MyGuiScreenToolbarConfigBase.GroupMode = MyGuiScreenToolbarConfigBase.GroupModes.HideAll;

    [VisualScriptingMiscData("G-Screen", "Sets group mode of toolbar config screen (G-screen) to Hide block groups.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void ToolbarConfigGroupsHideBlockGroups() => MyGuiScreenToolbarConfigBase.GroupMode = MyGuiScreenToolbarConfigBase.GroupModes.HideBlockGroups;

    [VisualScriptingMiscData("G-Screen", "Sets group mode of toolbar config screen (G-screen) to Default.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void ToolbarConfigGroupsDefualtBehavior() => MyGuiScreenToolbarConfigBase.GroupMode = MyGuiScreenToolbarConfigBase.GroupModes.Default;

    [VisualScriptingMiscData("G-Screen", "Adds specific item into research.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void ResearchListAddItem(MyDefinitionId itemId)
    {
      if (MySessionComponentResearch.Static == null)
        return;
      MySessionComponentResearch.Static.AddRequiredResearch(itemId);
    }

    [VisualScriptingMiscData("G-Screen", "Removes specific item from research.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void ResearchListRemoveItem(MyDefinitionId itemId)
    {
      if (MySessionComponentResearch.Static == null)
        return;
      MySessionComponentResearch.Static.RemoveRequiredResearch(itemId);
    }

    [VisualScriptingMiscData("G-Screen", "Clears required research list for all.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void ResearchListClear()
    {
      if (MySessionComponentResearch.Static == null)
        return;
      MySessionComponentResearch.Static.ClearRequiredResearch();
    }

    [VisualScriptingMiscData("G-Screen", "Resets research for all.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void PlayerResearchClearAll()
    {
      if (MySessionComponentResearch.Static == null)
        return;
      MySessionComponentResearch.Static.ResetResearchForAll();
    }

    [VisualScriptingMiscData("G-Screen", "Resets research for the specific player. If 'playerId' equals -1, resets research for the local player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void PlayerResearchClear(long playerId = -1)
    {
      if (playerId == -1L && MySession.Static.LocalCharacter != null)
        playerId = MySession.Static.LocalCharacter.GetPlayerIdentityId();
      if (MySessionComponentResearch.Static == null)
        return;
      MySessionComponentResearch.Static.ResetResearch(playerId);
    }

    [VisualScriptingMiscData("G-Screen", "[OBSOLETE] Enables/disables research whitelist mode.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void ResearchListWhitelist(bool whitelist)
    {
      if (MySessionComponentResearch.Static == null)
        return;
      MySessionComponentResearch.Static.SwitchWhitelistMode(whitelist);
    }

    [VisualScriptingMiscData("G-Screen", "Unlocks the specific research for the specific player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void PlayerResearchUnlock(long playerId, MyDefinitionId itemId)
    {
      if (MySessionComponentResearch.Static == null)
        return;
      MySessionComponentResearch.Static.UnlockResearchDirect(playerId, itemId);
    }

    [VisualScriptingMiscData("G-Screen", "Locks the specific research for the specific player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void PlayerResearchLock(long playerId, MyDefinitionId itemId)
    {
      if (MySessionComponentResearch.Static == null)
        return;
      MySessionComponentResearch.Static.LockResearch(playerId, itemId);
    }

    [VisualScriptingMiscData("GUI", "Gets whole item grid and find index of specific item in it. If no item was found, method will still return the item grid and index will be set to last index in it (GetItemsCount() - 1). Works only when ToolbarConfig screen is opened and focused.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static void GetToolbarConfigGridItemIndexAndControl(
      MyDefinitionId itemDefinition,
      out MyGuiControlBase control,
      out int index)
    {
      control = (MyGuiControlBase) null;
      index = -1;
      MyGuiScreenToolbarConfigBase openedToolbarConfig = MyVisualScriptLogicProvider.GetOpenedToolbarConfig();
      if (openedToolbarConfig == null)
        return;
      control = openedToolbarConfig.GetControlByName("ScrollablePanel\\Grid");
      if (!(control is MyGuiControlGrid myGuiControlGrid))
        return;
      index = 0;
      while (index < myGuiControlGrid.GetItemsCount())
      {
        MyGuiGridItem itemAt = myGuiControlGrid.GetItemAt(index);
        if (itemAt != null && itemAt.UserData != null && (((MyGuiScreenToolbarConfigBase.GridItemUserData) itemAt.UserData).ItemData() is MyObjectBuilder_ToolbarItemDefinition toolbarItemDefinition && (MyDefinitionId) toolbarItemDefinition.DefinitionId == itemDefinition))
          break;
        ++index;
      }
    }

    [VisualScriptingMiscData("GUI", "Gets whole inventory grid of player and find index of specific item in it. If no item was found, method will still return inventory grid and index will be set to last index in it (GetItemsCount() - 1). Works only when Terminal screen is opened and focused.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static void GetPlayersInventoryItemIndexAndControl(
      MyDefinitionId itemDefinition,
      out MyGuiControlBase control,
      out int index)
    {
      control = (MyGuiControlBase) null;
      index = -1;
      MyGuiScreenTerminal openedTerminal = MyVisualScriptLogicProvider.GetOpenedTerminal();
      if (openedTerminal == null)
        return;
      control = openedTerminal.GetControlByName("TerminalTabs\\PageInventory\\LeftInventory\\MyGuiControlInventoryOwner\\InventoryGrid");
      if (!(control is MyGuiControlGrid myGuiControlGrid))
        return;
      index = 0;
      while (index < myGuiControlGrid.GetItemsCount() && !(((MyPhysicalInventoryItem) myGuiControlGrid.GetItemAt(index).UserData).GetDefinitionId() == itemDefinition))
        ++index;
    }

    [VisualScriptingMiscData("GUI", "Gets whole inventory grid of interacted entity and find index of specific item in it. If no item was found, method will still return inventory grid and index will be set to last index in it (GetItemsCount() - 1). Works only when Terminal screen is opened and focused.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static void GetInteractedEntityInventoryItemIndexAndControl(
      MyDefinitionId itemDefinition,
      out MyGuiControlBase control,
      out int index)
    {
      control = (MyGuiControlBase) null;
      index = -1;
      MyGuiScreenTerminal openedTerminal = MyVisualScriptLogicProvider.GetOpenedTerminal();
      if (openedTerminal == null || !(openedTerminal.GetControlByName("TerminalTabs\\PageInventory\\RightInventory\\MyGuiControlInventoryOwner") is MyGuiControlInventoryOwner controlByName))
        return;
      foreach (MyGuiControlGrid contentGrid in controlByName.ContentGrids)
      {
        if (contentGrid != null)
        {
          control = (MyGuiControlBase) contentGrid;
          index = 0;
          while (index < contentGrid.GetItemsCount())
          {
            if (((MyPhysicalInventoryItem) contentGrid.GetItemAt(index).UserData).GetDefinitionId() == itemDefinition)
              return;
            ++index;
          }
        }
      }
    }

    [VisualScriptingMiscData("GUI", "Opens service overlay. If playerID is 0, open it for local player else open it for targeted player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void OpenSteamOverlay(string url, long playerId = 0)
    {
      if (playerId == 0L)
      {
        MyVisualScriptLogicProvider.OpenSteamOverlaySync(url);
      }
      else
      {
        MyPlayer.PlayerId result;
        if (!MySession.Static.Players.TryGetPlayerId(playerId, out result))
          return;
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string>((Func<IMyEventOwner, Action<string>>) (s => new Action<string>(MyVisualScriptLogicProvider.OpenSteamOverlaySync)), url, new EndpointId(result.SteamId));
      }
    }

    [Event(null, 5804)]
    [Reliable]
    [Client]
    private static void OpenSteamOverlaySync(string url)
    {
      if (!MyGuiSandbox.IsUrlWhitelisted(url))
        return;
      MyGuiSandbox.OpenUrl(url, UrlOpenMode.SteamOrExternalWithConfirm);
    }

    public static void OpenSteamOverlayLocal(string url) => MyVisualScriptLogicProvider.OpenSteamOverlaySync(url);

    [VisualScriptingMiscData("GUI", "Highlights specific GUI element in specific screen.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void HighlightGuiControl(string controlName, string activeScreenName)
    {
      foreach (MyGuiScreenBase screen in MyScreenManager.Screens)
      {
        if (screen.Name == activeScreenName)
        {
          foreach (MyGuiControlBase control in screen.Controls)
          {
            if (control.Name == controlName)
              MyGuiScreenHighlight.HighlightControl(new MyGuiScreenHighlight.MyHighlightControl()
              {
                Control = control
              });
          }
        }
      }
    }

    [VisualScriptingMiscData("GUI", "Highlights specific GUI element. If the element is of type MyGuiControlGrid, 'indicies' may be used to select which items should be highlighted. 'customToolTipMessage' can be used for custom tooltip of highlighted element.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void HighlightGuiControl(
      MyGuiControlBase control,
      List<int> indicies = null,
      string customToolTipMessage = null)
    {
      if (control == null)
        return;
      MyGuiScreenHighlight.MyHighlightControl control1 = new MyGuiScreenHighlight.MyHighlightControl()
      {
        Control = control
      };
      if (indicies != null)
        control1.Indices = indicies.ToArray();
      if (!string.IsNullOrEmpty(customToolTipMessage))
        control1.CustomToolTips = new MyToolTips(customToolTipMessage);
      MyGuiScreenHighlight.HighlightControl(control1);
    }

    [VisualScriptingMiscData("GUI", "Gets GUI element by name from the specific screen. You may search through hierarchy of controls by connecting element names with '\\\\'. Such as 'GrandParent\\\\Parent\\\\Child' will return element of name 'Child' that is under element 'Parent' that is under element 'GrandParent' which is in screen. In case specific element was not found, returned element will be the closest parent that was found.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static MyGuiControlBase GetControlByName(
      this MyGuiScreenBase screen,
      string controlName)
    {
      if (string.IsNullOrEmpty(controlName) || screen == null)
        return (MyGuiControlBase) null;
      string[] strArray = controlName.Split('\\');
      MyGuiControlBase controlByName = screen.Controls.GetControlByName(strArray[0]);
      for (int index = 1; index < strArray.Length; ++index)
      {
        switch (controlByName)
        {
          case MyGuiControlParent guiControlParent:
            controlByName = guiControlParent.Controls.GetControlByName(strArray[index]);
            break;
          case MyGuiControlScrollablePanel controlScrollablePanel:
            controlByName = controlScrollablePanel.Controls.GetControlByName(strArray[index]);
            break;
          case null:
            goto label_9;
          default:
            controlByName = controlByName.Elements.GetControlByName(strArray[index]);
            goto label_9;
        }
      }
label_9:
      return controlByName;
    }

    [VisualScriptingMiscData("GUI", "Gets GUI element by name from the specific Gui element. You may search through hierarchy of controls by connecting element names with '\\'. Such as 'GrandParent\\Parent\\Child' will return element of name 'Child' that is under element 'Parent' that is under element 'GrandParent' which is in screen. In case specific element was not found, returned element will be the closest parent that was found.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static MyGuiControlBase GetControlByName(
      this MyGuiControlParent control,
      string controlName)
    {
      if (string.IsNullOrEmpty(controlName) || control == null)
        return (MyGuiControlBase) null;
      string[] strArray = controlName.Split('\\');
      MyGuiControlBase controlByName = control.Controls.GetControlByName(strArray[0]);
      for (int index = 1; index < strArray.Length; ++index)
      {
        switch (controlByName)
        {
          case MyGuiControlParent guiControlParent:
            controlByName = guiControlParent.Controls.GetControlByName(strArray[index]);
            break;
          case MyGuiControlScrollablePanel controlScrollablePanel:
            controlByName = controlScrollablePanel.Controls.GetControlByName(strArray[index]);
            break;
          case null:
            goto label_9;
          default:
            controlByName = controlByName.Elements.GetControlByName(strArray[index]);
            goto label_9;
        }
      }
label_9:
      return controlByName;
    }

    [VisualScriptingMiscData("GUI", "Sets tooltip of specific GUI element.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetTooltip(this MyGuiControlBase control, string text) => control?.SetToolTip(text);

    public static void SetEnabledByExperimental(this MyGuiControlBase control)
    {
      if (MySandboxGame.Config.ExperimentalMode)
        return;
      control.Enabled = false;
      control.ShowTooltipWhenDisabled = true;
      control.SetToolTip(MyTexts.GetString(MyCommonTexts.ExperimentalRequired));
    }

    public static void SetDisabledByExperimental(this MyGuiControlBase control)
    {
      if (MySandboxGame.Config.ExperimentalMode)
        return;
      control.Enabled = false;
      control.ShowTooltipWhenDisabled = true;
      control.SetToolTip(MyTexts.GetString(MyCommonTexts.ExperimentalRequiredToDisable));
    }

    [VisualScriptingMiscData("GUI", "Gets currently opened terminal screen. (only if it is focused)", -10510688)]
    [VisualScriptingMember(false, false)]
    public static MyGuiScreenTerminal GetOpenedTerminal() => MyScreenManager.GetScreenWithFocus() as MyGuiScreenTerminal;

    [VisualScriptingMiscData("GUI", "Gets tab on specific index of specified TabControl element.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static MyGuiControlTabPage GetTab(
      this MyGuiControlTabControl tabs,
      int key)
    {
      return tabs?.GetTabSubControl(key);
    }

    [VisualScriptingMiscData("GUI", "Gets TabControl elements of specific terminal screen.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static MyGuiControlTabControl GetTabs(
      this MyGuiScreenTerminal terminal)
    {
      return terminal == null ? (MyGuiControlTabControl) null : terminal.Controls.GetControlByName("TerminalTabs") as MyGuiControlTabControl;
    }

    [VisualScriptingMiscData("GUI", "Gets currently opened ToolbarConfig screen (G-Screen). (only if it is focused)", -10510688)]
    [VisualScriptingMember(false, false)]
    public static MyGuiScreenToolbarConfigBase GetOpenedToolbarConfig() => MyScreenManager.GetScreenWithFocus() as MyGuiScreenToolbarConfigBase;

    [VisualScriptingMiscData("GUI", "Returns true if specific key was pressed in this frame.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsNewKeyPressed(MyKeys key) => MyInput.Static.IsNewKeyPressed(key);

    [VisualScriptingMiscData("GUI", "Gets friendly name of the specific screen.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static string GetFriendlyName(this MyGuiScreenBase screen) => screen.GetFriendlyName();

    [VisualScriptingMiscData("GUI", "Changes selected page of TabControl element to specific page.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetPage(this MyGuiControlTabControl pageControl, int pageNumber) => pageControl.SelectedPage = pageNumber;

    [VisualScriptingMiscData("GUI", "Changes faction Score", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetFactionScore(long factionId, int score)
    {
      IMyFaction factionById = MySession.Static.Factions.TryGetFactionById(factionId);
      if (factionById == null)
        return;
      factionById.Score = score;
    }

    [VisualScriptingMiscData("GUI", "Changes escape pod finished percentage in scoreCounter.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetFactionObjectivePercentageCompleted(long factionId, float finished)
    {
      IMyFaction factionById = MySession.Static.Factions.TryGetFactionById(factionId);
      if (factionById == null)
        return;
      factionById.ObjectivePercentageCompleted = finished;
    }

    [VisualScriptingMiscData("GUI", "Open Player screen to all players.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void OpenPlayerScreenAll() => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (x => new Action(MyVisualScriptLogicProvider.OpenPlayerScreenAllBroadcast)));

    [Event(null, 6033)]
    [Reliable]
    [Server]
    [Broadcast]
    public static void OpenPlayerScreenAllBroadcast()
    {
      if (Sync.IsDedicated || Sandbox.Engine.Multiplayer.MyMultiplayer.Static == null)
        return;
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateScreen<MyGuiScreenPlayers>());
    }

    [VisualScriptingMiscData("GUI", "Open Player screen to all players.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void OpenFactionVictoryScreenAll(string factionTag, float timeInSec) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, float>((Func<IMyEventOwner, Action<string, float>>) (x => new Action<string, float>(MyVisualScriptLogicProvider.OpenFactionVictoryScreenAllBroadcast)), factionTag, timeInSec);

    [Event(null, 6055)]
    [Reliable]
    [Server]
    [Broadcast]
    public static void OpenFactionVictoryScreenAllBroadcast(string factionTag, float timeInSec)
    {
      if (Sync.IsDedicated)
        return;
      MyGuiScreenVictory screen = MyGuiSandbox.CreateScreen<MyGuiScreenVictory>();
      screen.SetWinner(factionTag);
      screen.SetDuration(timeInSec);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) screen);
    }

    [VisualScriptingMiscData("Misc", "Takes a screenshot and saves it to specific destination.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void TakeScreenshot(string destination, string name) => MyRenderProxy.TakeScreenshot(new Vector2(0.5f, 0.5f), Path.Combine(destination, name, ".png"), false, true, false);

    [VisualScriptingMiscData("Misc", "Returns path to where game content is located.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static string GetContentPath() => MyFileSystem.ContentPath;

    [VisualScriptingMiscData("Misc", "Returns path to where game is being saved.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static string GetSavesPath() => MyFileSystem.SavesPath;

    [VisualScriptingMiscData("Misc", "Returns path to where mods are being stored.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static string GetModsPath() => MyFileSystem.ModsPath;

    [VisualScriptingMiscData("Misc", "Sets custom image for a loading screen.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetCustomLoadingScreenImage(string imagePath) => MySession.Static.CustomLoadingScreenImage = imagePath;

    [VisualScriptingMiscData("Misc", "Sets custom loading text for a loading screen", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetCustomLoadingScreenText(string text) => MySession.Static.CustomLoadingScreenText = text;

    [VisualScriptingMiscData("Misc", "Sets custom skybox for the current game.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetCustomSkybox(string skyboxPath) => MySession.Static.CustomSkybox = skyboxPath;

    [VisualScriptingMiscData("Misc", "Gets name of the control element (keyboard, mouse, gamepad buttons) that is binded to the specific action called 'keyName'. Names are defined in class MyControlsSpace, such as 'STRAFE_LEFT' or 'CUBE_ROTATE_ROLL_POSITIVE'.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static string GetUserControlKey(string keyName)
    {
      MyControl gameControl = MyInput.Static.GetGameControl(MyStringId.GetOrCompute(keyName));
      return gameControl != null ? gameControl.ToString() : "";
    }

    [VisualScriptingMiscData("Misc", "Creates a new color out of red, green and blue. All values must be in range <0;1>.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static Color GetColor(float r = 0.0f, float g = 0.0f, float b = 0.0f)
    {
      r = MathHelper.Clamp(r, 0.0f, 1f);
      g = MathHelper.Clamp(g, 0.0f, 1f);
      b = MathHelper.Clamp(b, 0.0f, 1f);
      return new Color(r, g, b);
    }

    [VisualScriptingMiscData("Notifications", "Shows a notification with specific message and font for the specific player for a defined time. If playerId is equal to 0, notification will be show to local player, otherwise it will be shown to specific player. If playerId is -1, notification is shown to all players.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void ShowNotification(
      string message,
      int disappearTimeMs,
      string font = "White",
      long playerId = 0)
    {
      switch (playerId)
      {
        case -1:
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, int, string>((Func<IMyEventOwner, Action<string, int, string>>) (s => new Action<string, int, string>(MyVisualScriptLogicProvider.ShowNotificationToAllSync)), message, disappearTimeMs, font);
          break;
        case 0:
          if (MyAPIGateway.Utilities == null)
            break;
          MyAPIGateway.Utilities.ShowNotification(message, disappearTimeMs, font);
          break;
        default:
          MyPlayer.PlayerId result;
          if (!MySession.Static.Players.TryGetPlayerId(playerId, out result))
            break;
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, int, string>((Func<IMyEventOwner, Action<string, int, string>>) (s => new Action<string, int, string>(MyVisualScriptLogicProvider.ShowNotificationSync)), message, disappearTimeMs, font, new EndpointId(result.SteamId));
          break;
      }
    }

    [VisualScriptingMiscData("Notifications", "Shows a notification with specific message and font to all players for a defined time.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void ShowNotificationToAll(string message, int disappearTimeMs, string font = "White")
    {
      if (Sandbox.Engine.Multiplayer.MyMultiplayer.Static == null)
      {
        if (MyAPIGateway.Utilities == null)
          return;
        MyAPIGateway.Utilities.ShowNotification(message, disappearTimeMs, font);
      }
      else
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, int, string>((Func<IMyEventOwner, Action<string, int, string>>) (s => new Action<string, int, string>(MyVisualScriptLogicProvider.ShowNotificationToAllSync)), message, disappearTimeMs, font);
    }

    [VisualScriptingMiscData("Notifications", "Sends a scripted chat message under name 'author' to all players (if playerId equal to 0), or to one specific player. In case of singleplayer, message will shown to local player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SendChatMessage(string message, string author = "", long playerId = 0, string font = "Blue")
    {
      if (Sandbox.Engine.Multiplayer.MyMultiplayer.Static != null)
      {
        ScriptedChatMsg msg;
        msg.Text = message;
        msg.Author = author;
        msg.Target = playerId;
        msg.Font = font;
        msg.Color = Color.White;
        MyMultiplayerBase.SendScriptedChatMessage(ref msg);
      }
      else
        MyHud.Chat.multiplayer_ScriptedChatMessageReceived(message, author, font, Color.White);
    }

    [VisualScriptingMiscData("Notifications", "Sends a scripted chat message under name 'author' to all players (if playerId equal to 0), or to one specific player. In case of singleplayer, message will shown to local player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SendChatMessageColored(
      string message,
      Color color,
      string author = "",
      long playerId = 0,
      string font = "White")
    {
      if (Sandbox.Engine.Multiplayer.MyMultiplayer.Static != null)
      {
        ScriptedChatMsg msg;
        msg.Text = message;
        msg.Author = author;
        msg.Target = playerId;
        msg.Font = font;
        msg.Color = color;
        MyMultiplayerBase.SendScriptedChatMessage(ref msg);
      }
      else
        MyHud.Chat.multiplayer_ScriptedChatMessageReceived(message, author, font, color);
    }

    [VisualScriptingMiscData("Notifications", "Sets for how long chat messages should be shown before fading out.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetChatMessageDuration(int durationS = 15) => MyHudChat.MaxMessageTime = durationS * 1000;

    [VisualScriptingMiscData("Notifications", "[Obsolete] Sets maximum count of messages in chat. [Has no effect anymore as whole history is being kept. Number of shown messages is dependant on number of rows they cover.]", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetChatMaxMessageCount(int count = 10) => MyHudChat.MaxMessageCount = count;

    [Event(null, 6247)]
    [Reliable]
    [Client]
    private static void ShowNotificationSync(string message, int disappearTimeMs, string font = "White")
    {
      if (MyAPIGateway.Utilities == null)
        return;
      MyAPIGateway.Utilities.ShowNotification(message, disappearTimeMs, font);
    }

    public static void ShowNotificationLocal(string message, int disappearTimeMs, string font = "White") => MyVisualScriptLogicProvider.ShowNotificationSync(message, disappearTimeMs, font);

    [Event(null, 6260)]
    [Reliable]
    [Broadcast]
    [Server]
    private static void ShowNotificationToAllSync(string message, int disappearTimeMs, string font = "White")
    {
      if (MyAPIGateway.Utilities == null)
        return;
      MyAPIGateway.Utilities.ShowNotification(message, disappearTimeMs, font);
    }

    [VisualScriptingMiscData("Notifications", "Adds a new notification for the specific player and returns if of the notification. Returns -1 if no player corresponds to 'playerId'. For 'playerId' equal to 0 use local player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static int AddNotification(string message, string font = "White", long playerId = 0)
    {
      int notificationId = MyVisualScriptLogicProvider.m_notificationIdCounter++;
      if (playerId == 0L)
      {
        MyVisualScriptLogicProvider.AddNotificationLocal(message, font, notificationId);
        return notificationId;
      }
      if (playerId == -1L)
      {
        MyVisualScriptLogicProvider.AddNotificationToAllSync(message, font, notificationId);
        return notificationId;
      }
      MyPlayer.PlayerId result;
      if (!MySession.Static.Players.TryGetPlayerId(playerId, out result))
        return -1;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, string, int>((Func<IMyEventOwner, Action<string, string, int>>) (s => new Action<string, string, int>(MyVisualScriptLogicProvider.AddNotificationSync)), message, font, notificationId, new EndpointId(result.SteamId));
      return notificationId;
    }

    [Event(null, 6298)]
    [Reliable]
    [Client]
    private static void AddNotificationSync(string message, string font, int notificationId)
    {
      MyHudNotification myHudNotification = new MyHudNotification(MyStringId.GetOrCompute(message), 0, font);
      MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification);
      MyVisualScriptLogicProvider.m_addedNotificationsById[notificationId] = myHudNotification;
    }

    public static void AddNotificationLocal(string message, string font, int notificationId) => MyVisualScriptLogicProvider.AddNotificationSync(message, font, notificationId);

    [Event(null, 6313)]
    [Reliable]
    [Broadcast]
    [Server]
    private static void AddNotificationToAllSync(string message, string font, int notificationId)
    {
      MyHudNotification myHudNotification = new MyHudNotification(MyStringId.GetOrCompute(message), 0, font);
      MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification);
      MyVisualScriptLogicProvider.m_addedNotificationsById[notificationId] = myHudNotification;
    }

    [VisualScriptingMiscData("Notifications", "Removes the specific notification referenced by its id from the specific player. If 'playerId' is equal to 0, apply on local player, if -1, apply to all.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void RemoveNotification(int messageId, long playerId = -1)
    {
      if (playerId == 0L)
        MyVisualScriptLogicProvider.RemoveNotificationSync(messageId);
      else
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<int, long>((Func<IMyEventOwner, Action<int, long>>) (s => new Action<int, long>(MyVisualScriptLogicProvider.RemoveNotificationSync)), messageId, playerId);
    }

    [Event(null, 6336)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void RemoveNotificationSync(int messageId, long playerId = -1)
    {
      MyHudNotification myHudNotification;
      if (playerId != -1L && MySession.Static.LocalPlayerId != playerId || !MyVisualScriptLogicProvider.m_addedNotificationsById.TryGetValue(messageId, out myHudNotification))
        return;
      MyHud.Notifications.Remove((MyHudNotificationBase) myHudNotification);
      MyVisualScriptLogicProvider.m_addedNotificationsById.Remove(messageId);
    }

    public static void RemoveNotificationLocal(int messageId, long playerId = -1) => MyVisualScriptLogicProvider.RemoveNotificationSync(messageId, playerId);

    [VisualScriptingMiscData("Notifications", "Clears all added notifications.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void ClearNotifications(long playerId = -1)
    {
      if (playerId == 0L)
        MyVisualScriptLogicProvider.ClearNotificationSync();
      else
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (s => new Action<long>(MyVisualScriptLogicProvider.ClearNotificationSync)), playerId);
    }

    [Event(null, 6371)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void ClearNotificationSync(long playerId = -1)
    {
      if (playerId != -1L && MySession.Static.LocalPlayerId != playerId)
        return;
      MyHud.Notifications.Clear();
      MyVisualScriptLogicProvider.m_notificationIdCounter = 0;
      MyVisualScriptLogicProvider.m_addedNotificationsById.Clear();
    }

    public static void ClearNotificationLocal(long playerId = -1) => MyVisualScriptLogicProvider.ClearNotificationSync(playerId);

    [VisualScriptingMiscData("Notifications", "Display congratulation screen to playet/s. Use MessageId to select which message should be shown. If player id is 1-, show to all. If it is 0, show to local player. Else it will be used as player identity id.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void DisplayCongratulationScreen(int congratulationMessageId, long playerId)
    {
      switch (playerId)
      {
        case -1:
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<int>((Func<IMyEventOwner, Action<int>>) (s => new Action<int>(MyVisualScriptLogicProvider.DisplayCongratulationScreenInternalAll)), congratulationMessageId);
          break;
        case 0:
          MyVisualScriptLogicProvider.DisplayCongratulationScreenInternal(congratulationMessageId);
          break;
        default:
          MyPlayer.PlayerId result;
          if (!MySession.Static.Players.TryGetPlayerId(playerId, out result))
            break;
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<int>((Func<IMyEventOwner, Action<int>>) (s => new Action<int>(MyVisualScriptLogicProvider.DisplayCongratulationScreenInternal)), congratulationMessageId, new EndpointId(result.SteamId));
          break;
      }
    }

    [Event(null, 6411)]
    [Reliable]
    [ServerInvoked]
    private static void DisplayCongratulationScreenInternal(int congratulationMessageId) => MyVisualScriptLogicProvider.DisplayCongratulationScreen(congratulationMessageId);

    [Event(null, 6417)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    private static void DisplayCongratulationScreenInternalAll(int congratulationMessageId) => MyVisualScriptLogicProvider.DisplayCongratulationScreen(congratulationMessageId);

    private static void DisplayCongratulationScreen(int congratulationMessageId) => MyScreenManager.AddScreen((MyGuiScreenBase) new MyGuiScreenCongratulation(congratulationMessageId));

    private static MyCharacter GetCharacterFromPlayerId(long playerId = 0)
    {
      if (playerId == 0L)
        return MySession.Static.LocalCharacter;
      return MySession.Static.Players.TryGetIdentity(playerId)?.Character;
    }

    private static MyIdentity GetIdentityFromPlayerId(long playerId = 0)
    {
      if (playerId != 0L)
        return MySession.Static.Players.TryGetIdentity(playerId);
      return MySession.Static.LocalHumanPlayer != null ? MySession.Static.LocalHumanPlayer.Identity : (MyIdentity) null;
    }

    private static MyPlayer GetPlayerFromPlayerId(long playerId = 0)
    {
      if (playerId == 0L)
        return MySession.Static.LocalHumanPlayer;
      MyPlayer player = (MyPlayer) null;
      MyPlayer.PlayerId result;
      if (!MySession.Static.Players.TryGetPlayerId(playerId, out result))
        return (MyPlayer) null;
      MySession.Static.Players.TryGetPlayerById(result, out player);
      return player;
    }

    [VisualScriptingMiscData("Player", "Gets online players.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static List<long> GetOnlinePlayers()
    {
      ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
      List<long> longList = new List<long>();
      if (onlinePlayers != null && onlinePlayers.Count > 0)
      {
        foreach (MyPlayer myPlayer in (IEnumerable<MyPlayer>) onlinePlayers)
          longList.Add(myPlayer.Identity.IdentityId);
      }
      return longList;
    }

    [VisualScriptingMiscData("Player", "Gets online and local players count.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static int GetPlayersCount() => MyVisualScriptLogicProvider.GetPlayers().Count;

    [VisualScriptingMiscData("Player", "Gets online and local players.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static List<long> GetPlayers()
    {
      ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
      List<long> longList = new List<long>();
      if (onlinePlayers != null && onlinePlayers.Count > 0)
      {
        foreach (MyPlayer myPlayer in (IEnumerable<MyPlayer>) onlinePlayers)
          longList.Add(myPlayer.Identity.IdentityId);
      }
      if (MySession.Static.LocalPlayerId != 0L && !longList.Contains(MySession.Static.LocalPlayerId))
        longList.Add(MySession.Static.LocalPlayerId);
      return longList;
    }

    [VisualScriptingMiscData("Player", "Get admins.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static List<long> GetAdmins()
    {
      ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
      List<long> longList = new List<long>();
      if (onlinePlayers != null && onlinePlayers.Count > 0)
      {
        foreach (MyPlayer myPlayer in (IEnumerable<MyPlayer>) onlinePlayers)
        {
          if (MySession.Static.GetUserPromoteLevel(myPlayer.Id.SteamId) == MyPromoteLevel.Admin)
            longList.Add(myPlayer.Identity.IdentityId);
        }
      }
      return longList;
    }

    [VisualScriptingMiscData("Player", "Gets oxygen level at player's position.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static float GetOxygenLevelAtPlayersPosition(long playerId = 0)
    {
      if (MySession.Static.Settings.EnableOxygenPressurization && MySession.Static.Settings.EnableOxygen)
      {
        MyCharacter characterFromPlayerId = MyVisualScriptLogicProvider.GetCharacterFromPlayerId(playerId);
        if (characterFromPlayerId != null && characterFromPlayerId.OxygenComponent != null)
          return (float) characterFromPlayerId.OxygenLevelAtCharacterLocation;
      }
      return 1f;
    }

    [VisualScriptingMiscData("Player", "Gets player's helmet status.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool GetPlayersHelmetStatus(long playerId = 0)
    {
      if (MySession.Static.Settings.EnableOxygenPressurization && MySession.Static.Settings.EnableOxygen)
      {
        MyCharacter characterFromPlayerId = MyVisualScriptLogicProvider.GetCharacterFromPlayerId(playerId);
        if (characterFromPlayerId != null && characterFromPlayerId.OxygenComponent != null)
          return characterFromPlayerId.OxygenComponent.HelmetEnabled;
      }
      return false;
    }

    [VisualScriptingMiscData("Player", "Toggle broadcasting for the player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void TogglePlayersBroadcasting(long playerId = 0, bool enabled = true)
    {
      MyCharacter characterFromPlayerId = MyVisualScriptLogicProvider.GetCharacterFromPlayerId(playerId);
      if (characterFromPlayerId == null || !Sync.IsServer)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCharacter, bool>(characterFromPlayerId, (Func<MyCharacter, Action<bool>>) (x => new Action<bool>(x.EnableBroadcasting)), enabled);
    }

    [VisualScriptingMiscData("Player", "Toggle player's ability to toggle broadcasting. Server still can change player's broadcasting state.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void TogglePlayersBroadcastEnabling(long playerId = 0, bool enabled = true)
    {
      MyCharacter characterFromPlayerId = MyVisualScriptLogicProvider.GetCharacterFromPlayerId(playerId);
      if (characterFromPlayerId == null || !Sync.IsServer)
        return;
      characterFromPlayerId.EnableBroadcastingPlayerToggle(enabled);
    }

    [VisualScriptingMiscData("Player", "Sets player's helmet status.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetPlayersHelmetStatus(long playerId = 0, bool status = true)
    {
      MyCharacter characterFromPlayerId = MyVisualScriptLogicProvider.GetCharacterFromPlayerId(playerId);
      if (characterFromPlayerId == null || characterFromPlayerId.OxygenComponent == null || characterFromPlayerId.OxygenComponent.HelmetEnabled == status)
        return;
      ((VRage.Game.ModAPI.Interfaces.IMyControllableEntity) characterFromPlayerId).SwitchHelmet();
    }

    [VisualScriptingMiscData("Player", "Gets player's controlled cube block (grid).", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool GetPlayerControlledBlockData(
      out string controlType,
      out long blockId,
      out string blockName,
      out long gridId,
      out string gridName,
      out bool isRespawnShip,
      long playerId = 0)
    {
      controlType = (string) null;
      blockId = 0L;
      blockName = (string) null;
      gridId = 0L;
      gridName = (string) null;
      isRespawnShip = false;
      MyPlayer playerFromPlayerId = MyVisualScriptLogicProvider.GetPlayerFromPlayerId(playerId);
      if (playerFromPlayerId == null || playerFromPlayerId.Controller == null || (playerFromPlayerId.Controller.ControlledEntity == null || !(playerFromPlayerId.Controller.ControlledEntity.Entity is MyCubeBlock)))
        return false;
      MyCubeBlock entity = (MyCubeBlock) playerFromPlayerId.Controller.ControlledEntity.Entity;
      ref string local = ref controlType;
      string str;
      switch (entity)
      {
        case MyCockpit _:
          str = "Cockpit";
          break;
        case MyRemoteControl _:
          str = "Remote";
          break;
        case MyUserControllableGun _:
          str = "Turret";
          break;
        default:
          str = "Other";
          break;
      }
      local = str;
      blockId = entity.EntityId;
      blockName = entity.Name;
      gridId = entity.CubeGrid.EntityId;
      gridName = entity.CubeGrid.Name;
      isRespawnShip = entity.CubeGrid.IsRespawnGrid;
      return true;
    }

    [VisualScriptingMiscData("Player", "Gets players entity ID.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static long GetPlayersEntityId(long playerId = 0)
    {
      MyIdentity identityFromPlayerId = MyVisualScriptLogicProvider.GetIdentityFromPlayerId(playerId);
      return identityFromPlayerId != null ? identityFromPlayerId.Character.EntityId : 0L;
    }

    [VisualScriptingMiscData("Player", "Gets players entity name.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static string GetPlayersEntityName(long playerId = 0)
    {
      MyCharacter characterFromPlayerId = MyVisualScriptLogicProvider.GetCharacterFromPlayerId(playerId);
      return characterFromPlayerId != null ? characterFromPlayerId.Name : "";
    }

    [VisualScriptingMiscData("Player", "Gets player's speed (linear velocity).", -10510688)]
    [VisualScriptingMember(false, false)]
    public static Vector3D GetPlayersSpeed(long playerId = 0)
    {
      MyCharacter characterFromPlayerId = MyVisualScriptLogicProvider.GetCharacterFromPlayerId(playerId);
      return characterFromPlayerId != null ? (Vector3D) characterFromPlayerId.Physics.LinearVelocity : Vector3D.Zero;
    }

    [VisualScriptingMiscData("Player", "Sets player's speed (linear velocity).", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetPlayersSpeed(Vector3D speed = default (Vector3D), long playerId = 0)
    {
      MyCharacter characterFromPlayerId = MyVisualScriptLogicProvider.GetCharacterFromPlayerId(playerId);
      if (characterFromPlayerId == null)
        return;
      if (speed != Vector3D.Zero)
      {
        float num1 = Math.Max(characterFromPlayerId.Definition.MaxSprintSpeed, Math.Max(characterFromPlayerId.Definition.MaxRunSpeed, characterFromPlayerId.Definition.MaxBackrunSpeed));
        float num2 = MyGridPhysics.ShipMaxLinearVelocity() + num1;
        if (speed.LengthSquared() > (double) num2 * (double) num2)
        {
          speed.Normalize();
          speed *= (double) num2;
        }
      }
      characterFromPlayerId.Physics.LinearVelocity = (Vector3) speed;
    }

    [VisualScriptingMiscData("Player", "Sets player's color (RGB).", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetPlayersColorInRGB(long playerId = 0, Color colorRBG = default (Color))
    {
      MyCharacter characterFromPlayerId = MyVisualScriptLogicProvider.GetCharacterFromPlayerId(playerId);
      characterFromPlayerId?.ChangeModelAndColor(characterFromPlayerId.ModelName, colorRBG.ColorToHSVDX11());
    }

    [VisualScriptingMiscData("Player", "Sets player's color (HSV).", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetPlayersColorInHSV(long playerId = 0, Vector3 colorHSV = default (Vector3))
    {
      MyCharacter characterFromPlayerId = MyVisualScriptLogicProvider.GetCharacterFromPlayerId(playerId);
      characterFromPlayerId?.ChangeModelAndColor(characterFromPlayerId.ModelName, colorHSV);
    }

    [VisualScriptingMiscData("Player", "Checks if player is in cockpit.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsPlayerInCockpit(long playerId = 0, string gridName = null, string cockpitName = null)
    {
      MyPlayer playerFromPlayerId = MyVisualScriptLogicProvider.GetPlayerFromPlayerId(playerId);
      MyCockpit myCockpit = (MyCockpit) null;
      if (playerFromPlayerId != null && playerFromPlayerId.Controller != null && playerFromPlayerId.Controller.ControlledEntity != null)
        myCockpit = playerFromPlayerId.Controller.ControlledEntity.Entity as MyCockpit;
      return myCockpit != null && (string.IsNullOrEmpty(gridName) || !(myCockpit.CubeGrid.Name != gridName)) && (string.IsNullOrEmpty(cockpitName) || !(myCockpit.Name != cockpitName));
    }

    [VisualScriptingMiscData("Player", "Checks if player is controlling something over remote.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsPlayerInRemote(long playerId = 0, string gridName = null, string remoteName = null)
    {
      MyPlayer playerFromPlayerId = MyVisualScriptLogicProvider.GetPlayerFromPlayerId(playerId);
      MyRemoteControl myRemoteControl = (MyRemoteControl) null;
      if (playerFromPlayerId != null && playerFromPlayerId.Controller != null && playerFromPlayerId.Controller.ControlledEntity != null)
        myRemoteControl = playerFromPlayerId.Controller.ControlledEntity.Entity as MyRemoteControl;
      return myRemoteControl != null && (string.IsNullOrEmpty(gridName) || !(myRemoteControl.CubeGrid.Name != gridName)) && (string.IsNullOrEmpty(remoteName) || !(myRemoteControl.Name != remoteName));
    }

    [VisualScriptingMiscData("Player", "Checks if player is controlling weapon.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsPlayerInWeapon(long playerId = 0, string gridName = null, string weaponName = null)
    {
      MyPlayer playerFromPlayerId = MyVisualScriptLogicProvider.GetPlayerFromPlayerId(playerId);
      MyUserControllableGun userControllableGun = (MyUserControllableGun) null;
      if (playerFromPlayerId != null && playerFromPlayerId.Controller != null && playerFromPlayerId.Controller.ControlledEntity != null)
        userControllableGun = playerFromPlayerId.Controller.ControlledEntity.Entity as MyUserControllableGun;
      return userControllableGun != null && (string.IsNullOrEmpty(gridName) || !(userControllableGun.CubeGrid.Name != gridName)) && (string.IsNullOrEmpty(weaponName) || !(userControllableGun.Name != weaponName));
    }

    [VisualScriptingMiscData("Player", "Checks if player is dead.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsPlayerDead(long playerId = 0)
    {
      MyCharacter characterFromPlayerId = MyVisualScriptLogicProvider.GetCharacterFromPlayerId(playerId);
      return characterFromPlayerId != null && characterFromPlayerId.IsDead;
    }

    [VisualScriptingMiscData("Player", "Gets player's name.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static string GetPlayersName(long playerId = 0)
    {
      MyIdentity identityFromPlayerId = MyVisualScriptLogicProvider.GetIdentityFromPlayerId(playerId);
      return identityFromPlayerId != null ? identityFromPlayerId.DisplayName : "";
    }

    [VisualScriptingMiscData("Player", "Gets player's health.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static float GetPlayersHealth(long playerId = 0)
    {
      MyIdentity identityFromPlayerId = MyVisualScriptLogicProvider.GetIdentityFromPlayerId(playerId);
      return identityFromPlayerId != null ? identityFromPlayerId.Character.StatComp.Health.Value : -1f;
    }

    [VisualScriptingMiscData("Player", "Checks if player's jetpack is enabled.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsPlayersJetpackEnabled(long playerId = 0)
    {
      MyIdentity identityFromPlayerId = MyVisualScriptLogicProvider.GetIdentityFromPlayerId(playerId);
      return identityFromPlayerId != null && identityFromPlayerId.Character != null && identityFromPlayerId.Character.JetpackComp != null && identityFromPlayerId.Character.JetpackComp.TurnedOn;
    }

    [VisualScriptingMiscData("Player", "Gets oxygen level of player's suit.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static float GetPlayersOxygenLevel(long playerId = 0)
    {
      MyIdentity identityFromPlayerId = MyVisualScriptLogicProvider.GetIdentityFromPlayerId(playerId);
      return identityFromPlayerId != null ? identityFromPlayerId.Character.OxygenComponent.SuitOxygenLevel : -1f;
    }

    [VisualScriptingMiscData("Player", "Gets hydrogen level of player's suit.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static float GetPlayersHydrogenLevel(long playerId = 0)
    {
      MyIdentity identityFromPlayerId = MyVisualScriptLogicProvider.GetIdentityFromPlayerId(playerId);
      return identityFromPlayerId != null ? identityFromPlayerId.Character.OxygenComponent.GetGasFillLevel(MyCharacterOxygenComponent.HydrogenId) : -1f;
    }

    [VisualScriptingMiscData("Player", "Gets energy level of player's suit.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static float GetPlayersEnergyLevel(long playerId = 0)
    {
      MyIdentity identityFromPlayerId = MyVisualScriptLogicProvider.GetIdentityFromPlayerId(playerId);
      return identityFromPlayerId != null ? identityFromPlayerId.Character.SuitEnergyLevel : -1f;
    }

    [VisualScriptingMiscData("Player", "Sets player's health.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetPlayersHealth(long playerId = 0, float value = 100f)
    {
      MyIdentity identityFromPlayerId = MyVisualScriptLogicProvider.GetIdentityFromPlayerId(playerId);
      if (identityFromPlayerId == null || identityFromPlayerId.Character == null || identityFromPlayerId.Character.StatComp == null)
        return;
      float num1 = identityFromPlayerId.Character.StatComp.Health.Value;
      if ((double) value < (double) num1)
      {
        float num2 = num1 - value;
        identityFromPlayerId.Character.StatComp.DoDamage(num2, (object) new MyDamageInformation(false, num2, MyVisualScriptLogicProvider.DAMAGE_TYPE_SCRIPT, 0L));
      }
      else
        identityFromPlayerId.Character.StatComp.Health.Value = value;
    }

    [VisualScriptingMiscData("Player", "Sets oxygen level of the player's suit.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetPlayersOxygenLevel(long playerId = 0, float value = 1f)
    {
      MyIdentity identityFromPlayerId = MyVisualScriptLogicProvider.GetIdentityFromPlayerId(playerId);
      if (identityFromPlayerId == null || identityFromPlayerId.Character == null || identityFromPlayerId.Character.OxygenComponent == null)
        return;
      identityFromPlayerId.Character.OxygenComponent.SuitOxygenLevel = value;
    }

    [VisualScriptingMiscData("Player", "Sets hydrogen level of the player's suit.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetPlayersHydrogenLevel(long playerId = 0, float value = 1f)
    {
      MyIdentity identityFromPlayerId = MyVisualScriptLogicProvider.GetIdentityFromPlayerId(playerId);
      if (identityFromPlayerId == null)
        return;
      MyDefinitionId hydrogenId = MyCharacterOxygenComponent.HydrogenId;
      identityFromPlayerId?.Character?.OxygenComponent?.UpdateStoredGasLevel(ref hydrogenId, value);
    }

    [VisualScriptingMiscData("Player", "Sets energy level of the player's suit.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetPlayersEnergyLevel(long playerId = 0, float value = 1f)
    {
      MyIdentity identityFromPlayerId = MyVisualScriptLogicProvider.GetIdentityFromPlayerId(playerId);
      if (identityFromPlayerId == null || identityFromPlayerId == null)
        return;
      identityFromPlayerId.Character?.SuitBattery?.ResourceSource?.SetRemainingCapacityByType(MyResourceDistributorComponent.ElectricityId, value * 1E-05f);
    }

    [VisualScriptingMiscData("Player", "Sets player's position.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetPlayersPosition(long playerId = 0, Vector3D position = default (Vector3D))
    {
      MyIdentity identityFromPlayerId = MyVisualScriptLogicProvider.GetIdentityFromPlayerId(playerId);
      if (identityFromPlayerId == null)
        return;
      if (identityFromPlayerId?.Character != null)
      {
        if (identityFromPlayerId.Character.IsOnLadder)
          identityFromPlayerId.Character.GetOffLadder();
        if (identityFromPlayerId.Character.IsUsing is MyCockpit)
          ((MyCockpit) identityFromPlayerId.Character.IsUsing).RemovePilot();
      }
      identityFromPlayerId?.Character?.PositionComp?.SetPosition(position);
    }

    [VisualScriptingMiscData("Player", "Gets player's position.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static Vector3D GetPlayersPosition(long playerId = 0)
    {
      MyIdentity identityFromPlayerId = MyVisualScriptLogicProvider.GetIdentityFromPlayerId(playerId);
      return identityFromPlayerId != null && identityFromPlayerId.Character != null && identityFromPlayerId.Character.PositionComp != null ? identityFromPlayerId.Character.PositionComp.GetPosition() : Vector3D.Zero;
    }

    [VisualScriptingMiscData("Player", "Gets player's inventory item amount.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static int GetPlayersInventoryItemAmount(long playerId = 0, MyDefinitionId itemId = default (MyDefinitionId)) => (int) Math.Round((double) MyVisualScriptLogicProvider.GetPlayersInventoryItemAmountPrecise(playerId, itemId));

    [VisualScriptingMiscData("Player", "Gets player's inventory item amount (precise).", -10510688)]
    [VisualScriptingMember(false, false)]
    public static float GetPlayersInventoryItemAmountPrecise(long playerId = 0, MyDefinitionId itemId = default (MyDefinitionId))
    {
      MyIdentity identityFromPlayerId = MyVisualScriptLogicProvider.GetIdentityFromPlayerId(playerId);
      return identityFromPlayerId != null && (!itemId.TypeId.IsNull && identityFromPlayerId.Character != null) ? (float) MyVisualScriptLogicProvider.GetInventoryItemAmount((MyEntity) identityFromPlayerId.Character, itemId) : 0.0f;
    }

    [VisualScriptingMiscData("Player", "Adds the specified item to the player's inventory.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void AddToPlayersInventory(long playerId = 0, MyDefinitionId itemId = default (MyDefinitionId), int amount = 1)
    {
      MyIdentity identityFromPlayerId = MyVisualScriptLogicProvider.GetIdentityFromPlayerId(playerId);
      if (identityFromPlayerId == null || identityFromPlayerId.Character == null)
        return;
      MyInventory inventory = MyEntityExtensions.GetInventory(identityFromPlayerId.Character);
      if (inventory == null)
        return;
      MyObjectBuilder_PhysicalObject newObject = (MyObjectBuilder_PhysicalObject) MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) itemId);
      MyFixedPoint myFixedPoint = new MyFixedPoint();
      MyFixedPoint amount1 = (MyFixedPoint) amount;
      inventory.AddItems(amount1, (MyObjectBuilder_Base) newObject);
    }

    [VisualScriptingMiscData("Player", "Removes the specified item from the player's inventory.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void RemoveFromPlayersInventory(long playerId = 0, MyDefinitionId itemId = default (MyDefinitionId), int amount = 1)
    {
      MyIdentity identityFromPlayerId = MyVisualScriptLogicProvider.GetIdentityFromPlayerId(playerId);
      if (identityFromPlayerId == null)
        return;
      MyInventory inventory = MyEntityExtensions.GetInventory(identityFromPlayerId.Character);
      if (inventory == null)
        return;
      MyObjectBuilder_PhysicalObject newObject = (MyObjectBuilder_PhysicalObject) MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) itemId);
      MyFixedPoint myFixedPoint1 = new MyFixedPoint();
      MyFixedPoint myFixedPoint2 = (MyFixedPoint) amount;
      MyFixedPoint itemAmount = inventory.GetItemAmount(itemId, MyItemFlags.None, false);
      inventory.RemoveItemsOfType(myFixedPoint2 < itemAmount ? myFixedPoint2 : itemAmount, newObject, false, true);
    }

    [VisualScriptingMiscData("Player", "Sets player's damage modifier.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetPlayerGeneralDamageModifier(long playerId = 0, float modifier = 1f)
    {
      MyCharacter myCharacter = (MyCharacter) null;
      if (playerId > 0L)
      {
        MyIdentity identity = MySession.Static.Players.TryGetIdentity(playerId);
        if (identity != null)
          myCharacter = identity.Character;
      }
      else
        myCharacter = MySession.Static.LocalCharacter;
      if (myCharacter == null)
        return;
      myCharacter.CharacterGeneralDamageModifier = modifier;
    }

    [VisualScriptingMiscData("Player", "Spawns player on the specified position.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SpawnPlayer(MatrixD worldMatrix, Vector3D velocity = default (Vector3D), long playerId = 0)
    {
      MyPlayer.PlayerId result;
      if (!MySession.Static.Players.TryGetPlayerId(playerId, out result))
        return;
      MyPlayer playerById = MySession.Static.Players.GetPlayerById(result);
      if (playerById == null)
        return;
      if (playerById.Character != null && !playerById.Character.IsDead)
      {
        if (playerById.Character.IsOnLadder)
          playerById.Character.GetOffLadder();
        if (playerById.Character.IsUsing is MyCockpit)
          ((MyCockpit) playerById.Character.IsUsing).RemovePilot();
        playerById.Character.PositionComp.SetWorldMatrix(ref worldMatrix);
      }
      else
      {
        playerById.SpawnAt(worldMatrix, (Vector3) velocity, (MyEntity) null, (MyBotDefinition) null);
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (s => new Action(MyVisualScriptLogicProvider.CloseRespawnScreen)), new EndpointId(playerById.Id.SteamId));
      }
    }

    [Event(null, 7060)]
    [Reliable]
    [Client]
    private static void CloseRespawnScreen() => Sync.Players.RespawnComponent.CloseRespawnScreenNow();

    [VisualScriptingMiscData("Player", "Sets player's input black list. Enables/Disables specified control of the character.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetPlayerInputBlacklistState(
      string controlStringId,
      long playerId = -1,
      bool enabled = false)
    {
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, long, bool>((Func<IMyEventOwner, Action<string, long, bool>>) (s => new Action<string, long, bool>(MyVisualScriptLogicProvider.SetPlayerInputBlacklistStateSync)), controlStringId, playerId, enabled);
    }

    [Event(null, 7073)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void SetPlayerInputBlacklistStateSync(
      string controlStringId,
      long playerId = -1,
      bool enabled = false)
    {
      if (playerId != -1L && MySession.Static.LocalPlayerId != playerId)
        return;
      MyInput.Static.SetControlBlock(MyStringId.GetOrCompute(controlStringId), !enabled);
    }

    [VisualScriptingMiscData("Player", "Toggles the players ability to sprint", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void ToggleAbilityToSprint(long playerId = 0, bool canSprint = true) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long, bool>((Func<IMyEventOwner, Action<long, bool>>) (s => new Action<long, bool>(MyVisualScriptLogicProvider.ToggleAbilityToSprintSync)), playerId, canSprint);

    [Event(null, 7088)]
    [Reliable]
    [Server]
    [Broadcast]
    public static void ToggleAbilityToSprintSync(long playerId = 0, bool canSprint = true)
    {
      MyCharacter characterFromPlayerId = MyVisualScriptLogicProvider.GetCharacterFromPlayerId(playerId);
      if (characterFromPlayerId == null)
        return;
      characterFromPlayerId.CanSprint = canSprint;
    }

    [VisualScriptingMiscData("Questlog", "Sets title and visibility of the quest for the specified player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetQuestlog(bool visible = true, string questName = "", long playerId = -1)
    {
      long num = playerId != 0L || MySession.Static.LocalCharacter == null ? playerId : MySession.Static.LocalCharacter.GetPlayerIdentityId();
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<bool, string, long>((Func<IMyEventOwner, Action<bool, string, long>>) (s => new Action<bool, string, long>(MyVisualScriptLogicProvider.SetQuestlogSync)), visible, questName, num);
    }

    [Event(null, 7109)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void SetQuestlogSync(bool visible = true, string questName = "", long playerId = -1)
    {
      if (playerId != -1L && !Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalPlayerId != playerId)
        return;
      MySession.Static.GetComponent<MySessionComponentIngameHelp>()?.TryCancelObjective();
      if (visible && !MyHud.Questlog.Visible)
        MyVisualScriptLogicProvider.PlayHudSound(MyGuiSounds.HudGPSNotification3, playerId);
      MyHud.Questlog.QuestTitle = questName;
      MyHud.Questlog.CleanDetails();
      MyHud.Questlog.Visible = visible;
      MyHud.Questlog.IsUsedByVisualScripting = visible;
    }

    public static void SetQuestlogLocal(bool visible = true, string questName = "", long playerId = -1) => MyVisualScriptLogicProvider.SetQuestlogSync(visible, questName, playerId);

    [VisualScriptingMiscData("Questlog", "Sets title of the quest for the specified player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetQuestlogTitle(string questName = "", long playerId = -1)
    {
      long num = playerId != 0L || MySession.Static.LocalCharacter == null ? playerId : MySession.Static.LocalCharacter.GetPlayerIdentityId();
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, long>((Func<IMyEventOwner, Action<string, long>>) (s => new Action<string, long>(MyVisualScriptLogicProvider.SetQuestlogTitleSync)), questName, num);
    }

    [Event(null, 7145)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void SetQuestlogTitleSync(string questName = "", long playerId = -1)
    {
      if (playerId != -1L && !Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalPlayerId != playerId)
        return;
      MyHud.Questlog.QuestTitle = questName;
    }

    public static void SetQuestlogTitleLocal(string questName = "", long playerId = -1) => MyVisualScriptLogicProvider.SetQuestlogTitleSync(questName, playerId);

    [VisualScriptingMiscData("Questlog", "Sets detail of the quest for the specified player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static int AddQuestlogDetail(
      string questDetailRow = "",
      bool completePrevious = true,
      bool useTyping = true,
      long playerId = -1)
    {
      long num = playerId != 0L || MySession.Static.LocalCharacter == null ? playerId : MySession.Static.LocalCharacter.GetPlayerIdentityId();
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, bool, bool, long>((Func<IMyEventOwner, Action<string, bool, bool, long>>) (s => new Action<string, bool, bool, long>(MyVisualScriptLogicProvider.AddQuestlogDetailSync)), questDetailRow, completePrevious, useTyping, num);
      return MyHud.Questlog.GetQuestGetails().Length - 1;
    }

    [Event(null, 7169)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void AddQuestlogDetailSync(
      string questDetailRow = "",
      bool completePrevious = true,
      bool useTyping = true,
      long playerId = -1)
    {
      if (playerId != -1L && !Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalPlayerId != playerId)
        return;
      if (!Sandbox.Engine.Platform.Game.IsDedicated)
        MyVisualScriptLogicProvider.PlayHudSound(MyGuiSounds.HudQuestlogDetail, playerId);
      MyHud.Questlog.AddDetail(questDetailRow, useTyping);
      int num = MyHud.Questlog.GetQuestGetails().Length - 1;
      if (!completePrevious)
        return;
      MyVisualScriptLogicProvider.PlayHudSound(MyGuiSounds.HudObjectiveComplete, playerId);
      MyHud.Questlog.SetCompleted(num - 1);
    }

    public static void AddQuestlogDetailLocal(
      string questDetailRow = "",
      bool completePrevious = true,
      bool useTyping = true,
      long playerId = -1)
    {
      MyVisualScriptLogicProvider.AddQuestlogDetailSync(questDetailRow, completePrevious, useTyping, playerId);
    }

    [VisualScriptingMiscData("Questlog", "Sets objective of the quest for the specified player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static int AddQuestlogObjective(
      string questDetailRow = "",
      bool completePrevious = true,
      bool useTyping = true,
      long playerId = -1)
    {
      long num = playerId != 0L || MySession.Static.LocalCharacter == null ? playerId : MySession.Static.LocalCharacter.GetPlayerIdentityId();
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, bool, bool, long>((Func<IMyEventOwner, Action<string, bool, bool, long>>) (s => new Action<string, bool, bool, long>(MyVisualScriptLogicProvider.AddQuestlogObjectiveSync)), questDetailRow, completePrevious, useTyping, num);
      return MyHud.Questlog.GetQuestGetails().Length - 1;
    }

    [Event(null, 7207)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void AddQuestlogObjectiveSync(
      string questDetailRow = "",
      bool completePrevious = true,
      bool useTyping = true,
      long playerId = -1)
    {
      if (playerId != -1L && !Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalPlayerId != playerId)
        return;
      if (!Sandbox.Engine.Platform.Game.IsDedicated)
        MyVisualScriptLogicProvider.PlayHudSound(MyGuiSounds.HudQuestlogDetail, playerId);
      MyHud.Questlog.AddDetail(questDetailRow, useTyping, true);
      int num = MyHud.Questlog.GetQuestGetails().Length - 1;
      if (!completePrevious)
        return;
      MyVisualScriptLogicProvider.PlayHudSound(MyGuiSounds.HudObjectiveComplete, playerId);
      MyHud.Questlog.SetCompleted(num - 1);
    }

    public static void AddQuestlogObjectiveLocal(
      string questDetailRow = "",
      bool completePrevious = true,
      bool useTyping = true,
      long playerId = -1)
    {
      MyVisualScriptLogicProvider.AddQuestlogObjectiveSync(questDetailRow, completePrevious, useTyping, playerId);
    }

    [VisualScriptingMiscData("Questlog", "Sets completed of the quest detail for the specified player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetQuestlogDetailCompleted(int lineId = 0, bool completed = true, long playerId = -1)
    {
      long num = playerId != 0L || MySession.Static.LocalCharacter == null ? playerId : MySession.Static.LocalCharacter.GetPlayerIdentityId();
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<int, bool, long>((Func<IMyEventOwner, Action<int, bool, long>>) (s => new Action<int, bool, long>(MyVisualScriptLogicProvider.SetQuestlogDetailCompletedSync)), lineId, completed, num);
    }

    [Event(null, 7244)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void SetQuestlogDetailCompletedSync(int lineId = 0, bool completed = true, long playerId = -1)
    {
      if (playerId != -1L && !Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalPlayerId != playerId)
        return;
      if (completed && !Sandbox.Engine.Platform.Game.IsDedicated)
        MyVisualScriptLogicProvider.PlayHudSound(MyGuiSounds.HudObjectiveComplete, playerId);
      MyHud.Questlog.SetCompleted(lineId, completed);
    }

    public static void SetQuestlogDetailCompletedLocal(int lineId = 0, bool completed = true, long playerId = -1) => MyVisualScriptLogicProvider.SetQuestlogDetailCompletedSync(lineId, completed, playerId);

    [VisualScriptingMiscData("Questlog", "Sets completed on all quest details for the specified player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetAllQuestlogDetailsCompleted(bool completed = true, long playerId = -1)
    {
      long num = playerId != 0L || MySession.Static.LocalCharacter == null ? playerId : MySession.Static.LocalCharacter.GetPlayerIdentityId();
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<bool, long>((Func<IMyEventOwner, Action<bool, long>>) (s => new Action<bool, long>(MyVisualScriptLogicProvider.SetAllQuestlogDetailsCompletedSync)), completed, num);
    }

    [Event(null, 7269)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void SetAllQuestlogDetailsCompletedSync(bool completed = true, long playerId = -1)
    {
      if (playerId != -1L && !Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalPlayerId != playerId)
        return;
      if (completed && !Sandbox.Engine.Platform.Game.IsDedicated)
        MyVisualScriptLogicProvider.PlayHudSound(MyGuiSounds.HudObjectiveComplete, playerId);
      MyHud.Questlog.SetAllCompleted(completed);
    }

    public static void SetAllQuestlogDetailsCompletedLocal(bool completed = true, long playerId = -1) => MyVisualScriptLogicProvider.SetAllQuestlogDetailsCompletedSync(completed, playerId);

    [VisualScriptingMiscData("Questlog", "Replaces detail of the quest for the specified player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void ReplaceQuestlogDetail(
      int id = 0,
      string newDetail = "",
      bool useTyping = true,
      long playerId = -1)
    {
      long num = playerId != 0L || MySession.Static.LocalCharacter == null ? playerId : MySession.Static.LocalCharacter.GetPlayerIdentityId();
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<int, string, bool, long>((Func<IMyEventOwner, Action<int, string, bool, long>>) (s => new Action<int, string, bool, long>(MyVisualScriptLogicProvider.ReplaceQuestlogDetailSync)), id, newDetail, useTyping, num);
    }

    [Event(null, 7293)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void ReplaceQuestlogDetailSync(
      int id = 0,
      string newDetail = "",
      bool useTyping = true,
      long playerId = -1)
    {
      if (playerId != -1L && !Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalPlayerId != playerId)
        return;
      MyHud.Questlog.ModifyDetail(id, newDetail, useTyping);
    }

    public static void ReplaceQuestlogDetailLocal(
      int id = 0,
      string newDetail = "",
      bool useTyping = true,
      long playerId = -1)
    {
      MyVisualScriptLogicProvider.ReplaceQuestlogDetailSync(id, newDetail, useTyping, playerId);
    }

    [VisualScriptingMiscData("Questlog", "Removes details of the quest for the specified player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void RemoveQuestlogDetails(long playerId = -1) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (s => new Action<long>(MyVisualScriptLogicProvider.RemoveQuestlogDetailsSync)), playerId != 0L || MySession.Static.LocalCharacter == null ? playerId : MySession.Static.LocalCharacter.GetPlayerIdentityId());

    [Event(null, 7314)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void RemoveQuestlogDetailsSync(long playerId = -1)
    {
      if (playerId != -1L && !Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalPlayerId != playerId)
        return;
      MyHud.Questlog.CleanDetails();
    }

    public static void RemoveQuestlogDetailsLocal(long playerId = -1) => MyVisualScriptLogicProvider.RemoveQuestlogDetailsSync(playerId);

    [VisualScriptingMiscData("Questlog", "Obsolete. Does not do anything.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetQuestlogPage(int value = 0, long playerId = -1)
    {
      long num = playerId != 0L || MySession.Static.LocalCharacter == null ? playerId : MySession.Static.LocalCharacter.GetPlayerIdentityId();
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<int, long>((Func<IMyEventOwner, Action<int, long>>) (s => new Action<int, long>(MyVisualScriptLogicProvider.SetQuestlogPageSync)), value, num);
    }

    [Event(null, 7335)]
    [Reliable]
    [Server]
    [Broadcast]
    [Obsolete]
    private static void SetQuestlogPageSync(int value = 0, long playerId = -1)
    {
    }

    [VisualScriptingMiscData("Questlog", "Obsolete. Returns -1.", -10510688)]
    [VisualScriptingMember(false, false)]
    [Obsolete]
    public static int GetQuestlogPage() => -1;

    [VisualScriptingMiscData("Questlog", "Obsolete. Returns -1.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static int GetQuestlogMaxPages() => -1;

    [VisualScriptingMiscData("Questlog", "Sets visible of the quest for the specified player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetQuestlogVisible(bool value = true, long playerId = -1)
    {
      long num = playerId != 0L || MySession.Static.LocalCharacter == null ? playerId : MySession.Static.LocalCharacter.GetPlayerIdentityId();
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<bool, long>((Func<IMyEventOwner, Action<bool, long>>) (s => new Action<bool, long>(MyVisualScriptLogicProvider.SetQuestlogVisibleSync)), value, num);
    }

    [Event(null, 7369)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void SetQuestlogVisibleSync(bool value = true, long playerId = -1)
    {
      if (playerId != -1L && !Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalPlayerId != playerId)
        return;
      if (value && !MyHud.Questlog.Visible && !Sandbox.Engine.Platform.Game.IsDedicated)
        MyVisualScriptLogicProvider.PlayHudSound(MyGuiSounds.HudGPSNotification3, playerId);
      MyHud.Questlog.Visible = value;
    }

    public static void SetQuestlogVisibleLocal(bool value = true, long playerId = -1) => MyVisualScriptLogicProvider.SetQuestlogVisibleSync(value, playerId);

    [VisualScriptingMiscData("Questlog", "Obsolete. Returns -1.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static int GetQuestlogPageFromMessage(int id = 0) => -1;

    [VisualScriptingMiscData("Questlog", "Enables highlight of the questlog for the specified player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void EnableHighlight(bool enable = true, long playerId = -1)
    {
      long num = playerId != 0L || MySession.Static.LocalCharacter == null ? playerId : MySession.Static.LocalCharacter.GetPlayerIdentityId();
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<bool, long>((Func<IMyEventOwner, Action<bool, long>>) (s => new Action<bool, long>(MyVisualScriptLogicProvider.EnableHighlightSync)), enable, num);
    }

    [Event(null, 7401)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void EnableHighlightSync(bool enable = true, long playerId = -1)
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated || playerId != -1L && MySession.Static.LocalPlayerId != playerId)
        return;
      MyHud.Questlog.HighlightChanges = enable;
    }

    public static void EnableHighlightLocal(bool enable = true, long playerId = -1) => MyVisualScriptLogicProvider.EnableHighlightSync(enable, playerId);

    [VisualScriptingMiscData("Questlog", "Returns true if all essential hints have been completed.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static bool AreEssentialGoodbotHintsDone()
    {
      List<string> tutorialsFinished = MySandboxGame.Config.TutorialsFinished;
      HashSet<string> essentialObjectiveIds = MySessionComponentIngameHelp.EssentialObjectiveIds;
      int num = 0;
      foreach (string str in tutorialsFinished)
      {
        if (essentialObjectiveIds.Contains(str))
          ++num;
      }
      return num == essentialObjectiveIds.Count;
    }

    [VisualScriptingMiscData("Spawn", "Spawns the group of prefabs at the specified position.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SpawnGroup(
      string subtypeId,
      Vector3D position,
      Vector3D direction,
      Vector3D up,
      long ownerId = 0,
      string newGridName = null)
    {
      ListReader<MySpawnGroupDefinition> groupDefinitions = MyDefinitionManager.Static.GetSpawnGroupDefinitions();
      MySpawnGroupDefinition spawnGroupDefinition1 = (MySpawnGroupDefinition) null;
      foreach (MySpawnGroupDefinition spawnGroupDefinition2 in groupDefinitions)
      {
        if (spawnGroupDefinition2.Id.SubtypeName == subtypeId)
        {
          spawnGroupDefinition1 = spawnGroupDefinition2;
          break;
        }
      }
      if (spawnGroupDefinition1 == null)
        return;
      List<MyCubeGrid> tmpGridList = new List<MyCubeGrid>();
      direction.Normalize();
      up.Normalize();
      MatrixD world = MatrixD.CreateWorld(position, direction, up);
      Action action = (Action) (() =>
      {
        if (newGridName == null || tmpGridList.Count <= 0)
          return;
        tmpGridList[0].Name = newGridName;
        Sandbox.Game.Entities.MyEntities.SetEntityName((MyEntity) tmpGridList[0]);
      });
      Stack<Action> callbacks = new Stack<Action>();
      callbacks.Push(action);
      foreach (MySpawnGroupDefinition.SpawnGroupPrefab prefab in spawnGroupDefinition1.Prefabs)
      {
        Vector3D position1 = Vector3D.Transform((Vector3D) prefab.Position, world);
        MyPrefabManager.Static.SpawnPrefab(tmpGridList, prefab.SubtypeId, position1, (Vector3) direction, (Vector3) up, (Vector3) ((double) prefab.Speed * direction), beaconName: prefab.BeaconText, spawningOptions: SpawningOptions.RotateFirstCockpitTowardsDirection, ownerId: ownerId, updateSync: true, callbacks: callbacks);
      }
    }

    [VisualScriptingMiscData("Spawn", "Spawns local blueprint at the specified position.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SpawnLocalBlueprint(
      string name,
      Vector3D position,
      Vector3D direction = default (Vector3D),
      string newGridName = null,
      long ownerId = 0)
    {
      MyVisualScriptLogicProvider.SpawnAlignedToGravityWithOffset(name, position, direction, newGridName, ownerId);
    }

    [VisualScriptingMiscData("Spawn", "Spawns local blueprint at the specified position and aligned to gravity.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SpawnLocalBlueprintInGravity(
      string name,
      Vector3D position,
      float rotationAngle = 0.0f,
      float gravityOffset = 0.0f,
      string newGridName = null,
      long ownerId = 0)
    {
      MyVisualScriptLogicProvider.SpawnAlignedToGravityWithOffset(name, position, new Vector3D(), newGridName, ownerId, gravityOffset, rotationAngle);
    }

    [VisualScriptingMiscData("Spawn", "Spawns the item at the specified position.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SpawnItem(
      MyDefinitionId itemId,
      Vector3D position,
      string inheritsVelocityFrom = "",
      float amount = 1f)
    {
      MyFixedPoint amount1 = (MyFixedPoint) amount;
      MyObjectBuilder_PhysicalObject newObject = (MyObjectBuilder_PhysicalObject) MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) itemId);
      if (newObject == null)
        return;
      MyPhysicalInventoryItem physicalInventoryItem = new MyPhysicalInventoryItem(amount1, newObject);
      MyPhysicsComponentBase component = (MyPhysicsComponentBase) null;
      MyEntity entity;
      if (!string.IsNullOrEmpty(inheritsVelocityFrom) && Sandbox.Game.Entities.MyEntities.TryGetEntityByName(inheritsVelocityFrom, out entity))
        entity.Components.TryGet<MyPhysicsComponentBase>(out component);
      Vector3D vector3D = Vector3D.Forward;
      Vector3D vector1 = Vector3D.Up;
      Vector3D naturalGravityInPoint = (Vector3D) MyGravityProviderSystem.CalculateNaturalGravityInPoint(position);
      if (naturalGravityInPoint != Vector3D.Zero)
      {
        vector1 = Vector3D.Normalize(naturalGravityInPoint) * -1.0;
        vector3D = vector1 != Vector3D.Right ? Vector3D.Cross(vector1, Vector3D.Right) : Vector3D.Forward;
      }
      Vector3D position1 = position;
      Vector3D forward = vector3D;
      Vector3D up = vector1;
      MyPhysicsComponentBase motionInheritedFrom = component;
      MyFloatingObjects.Spawn(physicalInventoryItem, position1, forward, up, motionInheritedFrom);
    }

    private static void SpawnAlignedToGravityWithOffset(
      string name,
      Vector3D position,
      Vector3D direction,
      string newGridName,
      long ownerId = 0,
      float gravityOffset = 0.0f,
      float gravityRotation = 0.0f)
    {
      string path = Path.Combine(Path.Combine(MyFileSystem.UserDataPath, "Blueprints", "local"), name, "bp.sbc");
      MyObjectBuilder_ShipBlueprintDefinition[] blueprintDefinitionArray = (MyObjectBuilder_ShipBlueprintDefinition[]) null;
      if (MyFileSystem.FileExists(path))
      {
        MyObjectBuilder_Definitions objectBuilder;
        if (!MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_Definitions>(path, out objectBuilder))
          return;
        blueprintDefinitionArray = objectBuilder.ShipBlueprints;
      }
      if (blueprintDefinitionArray == null)
        return;
      Vector3 vector3 = MyGravityProviderSystem.CalculateNaturalGravityInPoint(position);
      if (vector3 == Vector3.Zero)
        vector3 = MyGravityProviderSystem.CalculateArtificialGravityInPoint(position);
      Vector3D vector3D;
      if (vector3 != Vector3.Zero)
      {
        double num = (double) vector3.Normalize();
        vector3D = (Vector3D) -vector3;
        position += vector3 * gravityOffset;
        if (direction == Vector3D.Zero)
        {
          direction = Vector3D.CalculatePerpendicularVector((Vector3D) vector3);
          if ((double) gravityRotation != 0.0)
          {
            MatrixD fromAxisAngle = MatrixD.CreateFromAxisAngle(vector3D, (double) gravityRotation);
            direction = Vector3D.Transform(direction, fromAxisAngle);
          }
        }
      }
      else if (direction == Vector3D.Zero)
      {
        direction = Vector3D.Right;
        vector3D = Vector3D.Up;
      }
      else
        vector3D = Vector3D.CalculatePerpendicularVector(-direction);
      List<MyObjectBuilder_CubeGrid> objectBuilderCubeGridList = new List<MyObjectBuilder_CubeGrid>();
      foreach (MyObjectBuilder_PrefabDefinition prefabDefinition in blueprintDefinitionArray)
      {
        foreach (MyObjectBuilder_CubeGrid cubeGrid in prefabDefinition.CubeGrids)
        {
          cubeGrid.CreatePhysics = true;
          cubeGrid.EnableSmallToLargeConnections = true;
          if (!string.IsNullOrEmpty(newGridName))
            cubeGrid.Name = objectBuilderCubeGridList.Count > 0 ? newGridName + objectBuilderCubeGridList.Count.ToString() : newGridName;
          objectBuilderCubeGridList.Add(cubeGrid);
        }
      }
      if (!Sandbox.Engine.Platform.Game.IsDedicated)
        MyHud.PushRotatingWheelVisible();
      MatrixD world = MatrixD.CreateWorld(position, direction, vector3D);
      MyCubeGrid.RelocateGrids(objectBuilderCubeGridList, world);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<MyCubeGrid.MyPasteGridParameters>((Func<IMyEventOwner, Action<MyCubeGrid.MyPasteGridParameters>>) (s => new Action<MyCubeGrid.MyPasteGridParameters>(MyCubeGrid.TryPasteGrid_Implementation)), new MyCubeGrid.MyPasteGridParameters(objectBuilderCubeGridList, false, false, Vector3.Zero, true, new MyCubeGrid.RelativeOffset()
      {
        Use = false
      }, MySession.Static.GetComponent<MySessionComponentDLC>().GetAvailableClientDLCsIds()));
    }

    [VisualScriptingMiscData("Spawn", "Spawns the bot at the specified position.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SpawnBot(string subtypeName, Vector3D position)
    {
      MyBotDefinition botDefinition;
      if (!MyDefinitionManager.Static.TryGetBotDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_AnimalBot), subtypeName), out botDefinition) || botDefinition == null)
        return;
      MyAIComponent.Static.SpawnNewBot(botDefinition as MyAgentDefinition, position, false);
    }

    [VisualScriptingMiscData("Spawn", "Spawns the bot at the specified position, orientation and specific data.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static long SpawnBot(
      string subtypeName,
      Vector3D position,
      Vector3D direction,
      Vector3D up,
      string name)
    {
      MyBotDefinition botDefinition;
      if (!MyDefinitionManager.Static.TryGetBotDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_AnimalBot), subtypeName), out botDefinition))
        return 0;
      long num = 0;
      Vector3? up1 = new Vector3?();
      Vector3? direction1 = new Vector3?();
      if (!direction.IsZero())
        direction1 = new Vector3?((Vector3) direction);
      if (!up.IsZero())
        up1 = new Vector3?((Vector3) up);
      if (botDefinition != null)
        num = MyAIComponent.Static.SpawnNewBotSync(botDefinition as MyAgentDefinition, Sync.MyId, position, direction1, up1, name);
      return num;
    }

    [VisualScriptingMiscData("Spawn", "Spawns the prefab at the specified position.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SpawnPrefab(
      string prefabName,
      Vector3D position,
      Vector3D direction,
      Vector3D up,
      long ownerId = 0,
      string beaconName = null,
      string entityName = null,
      SpawningOptions spawningOptions = SpawningOptions.RotateFirstCockpitTowardsDirection)
    {
      if (MyPrefabManager.Static == null)
      {
        MyLog.Default.WriteLine("Spawn Prefab failed. Prefab manager is not initialized.");
      }
      else
      {
        direction.Normalize();
        up.Normalize();
        if (!direction.IsValid() || !up.IsValid() || !position.IsValid())
          return;
        MyPrefabManager.Static.SpawnPrefab(prefabName, position, (Vector3) direction, (Vector3) up, beaconName: beaconName, entityName: entityName, spawningOptions: spawningOptions, ownerId: ownerId, updateSync: true);
      }
    }

    [VisualScriptingMiscData("Spawn", "Spawns the prefab at the specified position aligned to gravity.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SpawnPrefabInGravity(
      string prefabName,
      Vector3D position,
      Vector3D direction,
      long ownerId = 0,
      string beaconName = null,
      string entityName = null,
      float gravityOffset = 0.0f,
      float gravityRotation = 0.0f,
      SpawningOptions spawningOptions = SpawningOptions.RotateFirstCockpitTowardsDirection)
    {
      if (MyPrefabManager.Static == null)
      {
        MyLog.Default.WriteLine("Spawn Prefab failed. Prefab manager is not initialized.");
      }
      else
      {
        if (!direction.IsValid() || !position.IsValid())
          return;
        Vector3 vector3 = MyGravityProviderSystem.CalculateNaturalGravityInPoint(position);
        if (vector3 == Vector3.Zero)
          vector3 = MyGravityProviderSystem.CalculateArtificialGravityInPoint(position);
        Vector3D axis;
        if (vector3 != Vector3.Zero)
        {
          double num = (double) vector3.Normalize();
          axis = (Vector3D) -vector3;
          position += vector3 * gravityOffset;
          if (direction == Vector3D.Zero)
          {
            direction = Vector3D.CalculatePerpendicularVector((Vector3D) vector3);
            if ((double) gravityRotation != 0.0)
            {
              MatrixD fromAxisAngle = MatrixD.CreateFromAxisAngle(axis, (double) gravityRotation);
              direction = Vector3D.Transform(direction, fromAxisAngle);
            }
          }
        }
        else if (direction == Vector3D.Zero)
        {
          direction = Vector3D.Right;
          axis = Vector3D.Up;
        }
        else
          axis = Vector3D.CalculatePerpendicularVector(-direction);
        direction.Normalize();
        axis.Normalize();
        if (!direction.IsValid() || !axis.IsValid())
          return;
        MyPrefabManager.Static.SpawnPrefab(prefabName, position, (Vector3) direction, (Vector3) axis, beaconName: beaconName, entityName: entityName, spawningOptions: spawningOptions, ownerId: ownerId, updateSync: true);
      }
    }

    [VisualScriptingMiscData("State Machines", "Starts the specified state machine.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void StartStateMachine(string stateMachineName, long ownerId = 0) => MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.SMManager.Run(stateMachineName, ownerId);

    [VisualScriptingMiscData("Toolbar", "Sets item to the specified slot for the player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetToolbarSlotToItem(int slot, MyDefinitionId itemId, long playerId = -1)
    {
      if (itemId.TypeId.IsNull)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<int, SerializableDefinitionId, long>((Func<IMyEventOwner, Action<int, SerializableDefinitionId, long>>) (s => new Action<int, SerializableDefinitionId, long>(MyVisualScriptLogicProvider.SetToolbarSlotToItemSync)), slot, (SerializableDefinitionId) itemId, playerId);
    }

    [Event(null, 7825)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void SetToolbarSlotToItemSync(
      int slot,
      SerializableDefinitionId itemId,
      long playerId = -1)
    {
      MyDefinitionBase definition;
      if (playerId != -1L && MySession.Static.LocalPlayerId != playerId || !MyDefinitionManager.Static.TryGetDefinition<MyDefinitionBase>((MyDefinitionId) itemId, out definition))
        return;
      MyToolbarItem toolbarItem = MyToolbarItemFactory.CreateToolbarItem(MyToolbarItemFactory.ObjectBuilderFromDefinition(definition));
      int? selectedSlot = MyToolbarComponent.CurrentToolbar.SelectedSlot;
      if (selectedSlot.HasValue)
      {
        selectedSlot = MyToolbarComponent.CurrentToolbar.SelectedSlot;
        int num = slot;
        if (selectedSlot.GetValueOrDefault() == num & selectedSlot.HasValue)
          MyToolbarComponent.CurrentToolbar.Unselect(false);
      }
      MyToolbarComponent.CurrentToolbar.SetItemAtSlot(slot, toolbarItem);
    }

    public static void SetToolbarSlotToItemLocal(
      int slot,
      SerializableDefinitionId itemId,
      long playerId = -1)
    {
      MyVisualScriptLogicProvider.SetToolbarSlotToItemSync(slot, itemId, playerId);
    }

    [VisualScriptingMiscData("Toolbar", "Switches the specified toolbar slot for the player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SwitchToolbarToSlot(int slot, long playerId = -1) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<int, long>((Func<IMyEventOwner, Action<int, long>>) (s => new Action<int, long>(MyVisualScriptLogicProvider.SwitchToolbarToSlotSync)), slot, playerId);

    [Event(null, 7854)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void SwitchToolbarToSlotSync(int slot, long playerId = -1)
    {
      if (playerId != -1L && MySession.Static.LocalPlayerId != playerId || (slot < 0 || slot >= MyToolbarComponent.CurrentToolbar.SlotCount))
        return;
      int? selectedSlot = MyToolbarComponent.CurrentToolbar.SelectedSlot;
      if (selectedSlot.HasValue)
      {
        selectedSlot = MyToolbarComponent.CurrentToolbar.SelectedSlot;
        if (selectedSlot.Value == slot)
          MyToolbarComponent.CurrentToolbar.Unselect(false);
      }
      MyToolbarComponent.CurrentToolbar.ActivateItemAtSlot(slot);
    }

    public static void SwitchToolbarToSlotLocal(int slot, long playerId = -1) => MyVisualScriptLogicProvider.SwitchToolbarToSlotSync(slot, playerId);

    [VisualScriptingMiscData("Toolbar", "Clears the toolbar slot for the player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void ClearToolbarSlot(int slot, long playerId = -1) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<int, long>((Func<IMyEventOwner, Action<int, long>>) (s => new Action<int, long>(MyVisualScriptLogicProvider.ClearToolbarSlotSync)), slot, playerId);

    [Event(null, 7880)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void ClearToolbarSlotSync(int slot, long playerId = -1)
    {
      if (slot < 0 || slot >= MyToolbarComponent.CurrentToolbar.SlotCount || playerId != -1L && MySession.Static.LocalPlayerId != playerId)
        return;
      MyToolbarComponent.CurrentToolbar.SetItemAtSlot(slot, (MyToolbarItem) null);
    }

    public static void ClearToolbarSlotLocal(int slot, long playerId = -1) => MyVisualScriptLogicProvider.ClearToolbarSlotSync(slot, playerId);

    [VisualScriptingMiscData("Toolbar", "Clears all toolbar slots for the specified player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void ClearAllToolbarSlots(long playerId = -1) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (s => new Action<long>(MyVisualScriptLogicProvider.ClearAllToolbarSlotsSync)), playerId);

    [Event(null, 7903)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void ClearAllToolbarSlotsSync(long playerId = -1)
    {
      if (playerId != -1L && MySession.Static.LocalPlayerId != playerId)
        return;
      int currentPage = MyToolbarComponent.CurrentToolbar.CurrentPage;
      for (int page = 0; page < MyToolbarComponent.CurrentToolbar.PageCount; ++page)
      {
        MyToolbarComponent.CurrentToolbar.SwitchToPage(page);
        for (int slot = 0; slot < MyToolbarComponent.CurrentToolbar.SlotCount; ++slot)
          MyToolbarComponent.CurrentToolbar.SetItemAtSlot(slot, (MyToolbarItem) null);
      }
      MyToolbarComponent.CurrentToolbar.SwitchToPage(currentPage);
    }

    public static void ClearAllToolbarSlotsLocal(long playerId = -1) => MyVisualScriptLogicProvider.ClearAllToolbarSlotsSync(playerId);

    [VisualScriptingMiscData("Toolbar", "Sets the specified page for the toolbar.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetToolbarPage(int page, long playerId = -1) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<int, long>((Func<IMyEventOwner, Action<int, long>>) (s => new Action<int, long>(MyVisualScriptLogicProvider.SetToolbarPageSync)), page, playerId);

    [Event(null, 7933)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void SetToolbarPageSync(int page, long playerId = -1)
    {
      if (playerId != -1L && MySession.Static.LocalPlayerId != playerId || (page < 0 || page >= MyToolbarComponent.CurrentToolbar.PageCount))
        return;
      MyToolbarComponent.CurrentToolbar.SwitchToPage(page);
    }

    public static void SetToolbarPageLocal(int page, long playerId = -1) => MyVisualScriptLogicProvider.SetToolbarPageSync(page, playerId);

    [VisualScriptingMiscData("Toolbar", "Reloads default settings for the toolbar", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void ReloadToolbarDefaults(long playerId = -1) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (s => new Action<long>(MyVisualScriptLogicProvider.ReloadToolbarDefaultsSync)), playerId);

    [Event(null, 7956)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void ReloadToolbarDefaultsSync(long playerId = -1)
    {
      if (playerId != -1L && MySession.Static.LocalPlayerId != playerId)
        return;
      MyToolbarComponent.CurrentToolbar.SetDefaults();
    }

    public static void ReloadToolbarDefaultsLocal(long playerId = -1) => MyVisualScriptLogicProvider.ReloadToolbarDefaultsSync(playerId);

    [VisualScriptingMiscData("Triggers", "Creates area trigger at the position of specified entity.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void CreateAreaTriggerOnEntity(string entityName, float radius, string name)
    {
      MyAreaTriggerComponent triggerComponent = new MyAreaTriggerComponent(name);
      triggerComponent.Radius = (double) radius;
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity))
        return;
      if (!entity.Components.Contains(typeof (MyTriggerAggregate)))
        entity.Components.Add(typeof (MyTriggerAggregate), (MyComponentBase) new MyTriggerAggregate());
      entity.Components.Get<MyTriggerAggregate>().AddComponent((MyComponentBase) triggerComponent);
      triggerComponent.Center = entity.PositionComp.GetPosition();
    }

    [VisualScriptingMiscData("Triggers", "Creates area trigger at the relative position to the specified entity.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void CreateAreaTriggerRelativeToEntity(
      Vector3D position,
      string entityName,
      float radius,
      string name)
    {
      MyAreaTriggerComponent triggerComponent = new MyAreaTriggerComponent(name);
      triggerComponent.Radius = (double) radius;
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity))
        return;
      if (!entity.Components.Contains(typeof (MyTriggerAggregate)))
        entity.Components.Add(typeof (MyTriggerAggregate), (MyComponentBase) new MyTriggerAggregate());
      entity.Components.Get<MyTriggerAggregate>().AddComponent((MyComponentBase) triggerComponent);
      triggerComponent.Center = position;
    }

    [VisualScriptingMiscData("Triggers", "Creates area trigger at the position.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static long CreateAreaTriggerOnPosition(Vector3D position, float radius, string name)
    {
      MyAreaTriggerComponent triggerComponent = new MyAreaTriggerComponent(name);
      MyEntity entity = new MyEntity();
      entity.PositionComp.SetPosition(position);
      entity.EntityId = MyEntityIdentifier.AllocateId();
      Sandbox.Game.Entities.MyEntities.Add(entity);
      if (!entity.Components.Contains(typeof (MyTriggerAggregate)))
        entity.Components.Add(typeof (MyTriggerAggregate), (MyComponentBase) new MyTriggerAggregate());
      entity.Components.Get<MyTriggerAggregate>().AddComponent((MyComponentBase) triggerComponent);
      triggerComponent.Center = position;
      triggerComponent.Radius = (double) radius;
      return entity.EntityId;
    }

    [VisualScriptingMiscData("Triggers", "Removes all area triggers from the specified entity.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void RemoveAllTriggersFromEntity(string entityName)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity))
        return;
      entity.Components.Remove(typeof (MyTriggerAggregate));
    }

    [VisualScriptingMiscData("Triggers", "Remove area trigger with the specified name.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void RemoveTrigger(string triggerName)
    {
      if (MySessionComponentTriggerSystem.Static == null)
        return;
      MyTriggerComponent foundTrigger;
      MyEntity triggersEntity = MySessionComponentTriggerSystem.Static.GetTriggersEntity(triggerName, out foundTrigger);
      if (triggersEntity == null || foundTrigger == null)
        return;
      MyTriggerAggregate component;
      if (triggersEntity.Components.TryGet<MyTriggerAggregate>(out component))
        component.RemoveComponent((MyComponentBase) foundTrigger);
      else
        triggersEntity.Components.Remove(typeof (MyAreaTriggerComponent), (MyComponentBase) (foundTrigger as MyAreaTriggerComponent));
    }

    [VisualScriptingMiscData("Achievements", "Award player achievement. Id ID is -1, unlock to all, if ID is 0, unlock to local player, if anything else, it unlocks to player with that ID", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void UnlockAchievementById(int achievementId, long playerId)
    {
      switch (playerId)
      {
        case -1:
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<int>((Func<IMyEventOwner, Action<int>>) (s => new Action<int>(MyVisualScriptLogicProvider.UnlockAchievementInternalAll)), achievementId);
          break;
        case 0:
          MyVisualScriptLogicProvider.UnlockAchievementInternal(achievementId);
          break;
        default:
          MyPlayer.PlayerId result;
          if (!MySession.Static.Players.TryGetPlayerId(playerId, out result))
            break;
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<int>((Func<IMyEventOwner, Action<int>>) (s => new Action<int>(MyVisualScriptLogicProvider.UnlockAchievementInternal)), achievementId, new EndpointId(result.SteamId));
          break;
      }
    }

    [Event(null, 8090)]
    [Reliable]
    [ServerInvoked]
    private static void UnlockAchievementInternal(int achievementId) => MyVisualScriptLogicProvider.UnlockAchievement(achievementId);

    [Event(null, 8096)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    private static void UnlockAchievementInternalAll(int achievementId) => MyVisualScriptLogicProvider.UnlockAchievement(achievementId);

    private static void UnlockAchievement(int achievementId)
    {
      if (achievementId < 0 || achievementId >= MyVisualScriptLogicProvider.AllowedAchievementsHelper.AllowedAchievements.Count || Sync.IsDedicated)
        return;
      MyGameService.GetAchievement(MyVisualScriptLogicProvider.AllowedAchievementsHelper.AllowedAchievements[achievementId], (string) null, 0.0f).Unlock();
    }

    [VisualScriptingMiscData("Definitions", "Returns true if the type id and subtype id match.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool DefinitionIdMatch(
      string typeId,
      string subtypeId,
      string matchTypeId,
      string matchSubtypeId)
    {
      return string.Equals(typeId, matchTypeId) && string.Equals(subtypeId, matchSubtypeId);
    }

    [VisualScriptingMiscData("Store", "Cancels listed item in specified store.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static bool CancelStoreItem(string storeEntityName, long id) => MyVisualScriptLogicProvider.GetEntityByName(storeEntityName) is Sandbox.ModAPI.IMyStoreBlock entityByName && entityByName.CancelStoreItem(id);

    [VisualScriptingMiscData("Store", "Inserts offer to specified store.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static MyStoreInsertResults InsertOffer(
      string storeEntityName,
      MyStoreItemData item,
      out long id)
    {
      if (MyVisualScriptLogicProvider.GetEntityByName(storeEntityName) is Sandbox.ModAPI.IMyStoreBlock entityByName)
        return entityByName.InsertOffer(item, out id);
      id = 0L;
      return MyStoreInsertResults.Error;
    }

    [VisualScriptingMiscData("Store", "Inserts order to specified store.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static MyStoreInsertResults InsertOrder(
      string storeEntityName,
      MyStoreItemData item,
      out long id)
    {
      if (MyVisualScriptLogicProvider.GetEntityByName(storeEntityName) is Sandbox.ModAPI.IMyStoreBlock entityByName)
        return entityByName.InsertOrder(item, out id);
      id = 0L;
      return MyStoreInsertResults.Error;
    }

    [VisualScriptingMiscData("Contract", "Create and add new Hauling contract. Returns true if contract creation was successful. Id of newly created contract is stored in out variable id. End block is contract block where package is to be delivered.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static bool AddHaulingContract(
      long startBlockId,
      int moneyReward,
      int collateral,
      int duration,
      long endBlockId,
      out long id)
    {
      id = 0L;
      MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
      if (component == null)
        return false;
      MyContractHauling myContractHauling = new MyContractHauling(startBlockId, moneyReward, collateral, duration, endBlockId);
      MyAddContractResultWrapper contractResultWrapper = component.AddContract((IMyContract) myContractHauling);
      if (!contractResultWrapper.Success)
        return false;
      id = contractResultWrapper.ContractId;
      return true;
    }

    [VisualScriptingMiscData("Contract", "Create and add new Acquisition contract. Returns true if contract creation was successful. Id of newly created contract is stored in out variable id. End block is contract block where items of type 'itemType' in quantity 'itemAmount' are to be delivered.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static bool AddAcquisitionContract(
      long startBlockId,
      int moneyReward,
      int collateral,
      int duration,
      long endBlockId,
      MyDefinitionId itemType,
      int itemAmount,
      out long id)
    {
      id = 0L;
      MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
      if (component == null)
        return false;
      MyContractAcquisition contractAcquisition = new MyContractAcquisition(startBlockId, moneyReward, collateral, duration, endBlockId, itemType, itemAmount);
      MyAddContractResultWrapper contractResultWrapper = component.AddContract((IMyContract) contractAcquisition);
      if (!contractResultWrapper.Success)
        return false;
      id = contractResultWrapper.ContractId;
      return true;
    }

    [VisualScriptingMiscData("Contract", "Create and add new Escort contract. Returns true if contract creation was successful. Id of newly created contract is stored in out variable id. Escort ship will start from 'start' flying towards the 'end'. Escorted ship will be owned by 'ownerIdentityId'", -10510688)]
    [VisualScriptingMember(true, false)]
    public static bool AddEscortContract(
      long startBlockId,
      int moneyReward,
      int collateral,
      int duration,
      Vector3D start,
      Vector3D end,
      long ownerIdentityId,
      out long id)
    {
      id = 0L;
      MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
      if (component == null)
        return false;
      MyContractEscort myContractEscort = new MyContractEscort(startBlockId, moneyReward, collateral, duration, start, end, ownerIdentityId);
      MyAddContractResultWrapper contractResultWrapper = component.AddContract((IMyContract) myContractEscort);
      if (!contractResultWrapper.Success)
        return false;
      id = contractResultWrapper.ContractId;
      return true;
    }

    [VisualScriptingMiscData("Contract", "Create and add new Search contract. Returns true if contract creation was successful. Id of newly created contract is stored in out variable id. 'targetGridId' is id of grid that will be searched and 'searchRadius' is radius of sphere around searched grid where GPS will be randomly placed in", -10510688)]
    [VisualScriptingMember(true, false)]
    public static bool AddSearchContract(
      long startBlockId,
      int moneyReward,
      int collateral,
      int duration,
      long targetGridId,
      double searchRadius,
      out long id)
    {
      id = 0L;
      MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
      if (component == null)
        return false;
      MyContractSearch myContractSearch = new MyContractSearch(startBlockId, moneyReward, collateral, duration, targetGridId, searchRadius);
      MyAddContractResultWrapper contractResultWrapper = component.AddContract((IMyContract) myContractSearch);
      if (!contractResultWrapper.Success)
        return false;
      id = contractResultWrapper.ContractId;
      return true;
    }

    [VisualScriptingMiscData("Contract", "Create and add new Bounty contract. Returns true if contract creation was successful. Id of newly created contract is stored in out variable id.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static bool AddBountyContract(
      long startBlockId,
      int moneyReward,
      int collateral,
      int duration,
      long targetIdentityId,
      out long id)
    {
      id = 0L;
      MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
      if (component == null)
        return false;
      MyContractBounty myContractBounty = new MyContractBounty(startBlockId, moneyReward, collateral, duration, targetIdentityId);
      MyAddContractResultWrapper contractResultWrapper = component.AddContract((IMyContract) myContractBounty);
      if (!contractResultWrapper.Success)
        return false;
      id = contractResultWrapper.ContractId;
      return true;
    }

    [VisualScriptingMiscData("Contract", "Create and add new Repair contract. Returns true if contract creation was successful. Id of newly created contract is stored in out variable id.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static bool AddRepairContract(
      long startBlockId,
      int moneyReward,
      int collateral,
      int duration,
      long targetGridId,
      out long id)
    {
      id = 0L;
      MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
      if (component == null)
        return false;
      MyContractRepair myContractRepair = new MyContractRepair(startBlockId, moneyReward, collateral, duration, targetGridId);
      MyAddContractResultWrapper contractResultWrapper = component.AddContract((IMyContract) myContractRepair);
      if (!contractResultWrapper.Success)
        return false;
      id = contractResultWrapper.ContractId;
      return true;
    }

    [VisualScriptingMiscData("Contract", "Remove inactive contract. Does not work if contract has already been accepted.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static bool RemoveContract(long id)
    {
      MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
      return component.IsContractInInactive(id) && component.RemoveContract(id);
    }

    [VisualScriptingMiscData("Entity Storage", "Stores string in the entity storage. This value is accessible from all scripts.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void StoreEntityString(string entityName, string key, string value)
    {
      MyEntity entity;
      if (string.IsNullOrEmpty(entityName) || !Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity))
        return;
      if (entity.EntityStorage == null)
        entity.EntityStorage = new MyEntityStorageComponent();
      entity.EntityStorage.Write(key, value);
    }

    [VisualScriptingMiscData("Entity Storage", "Stores boolean in the entity storage. This value is accessible from all scripts.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void StoreEntityBool(string entityName, string key, bool value)
    {
      MyEntity entity;
      if (string.IsNullOrEmpty(entityName) || !Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity))
        return;
      if (entity.EntityStorage == null)
        entity.EntityStorage = new MyEntityStorageComponent();
      entity.EntityStorage.Write(key, value);
    }

    [VisualScriptingMiscData("Entity Storage", "Stores integer in the entity storage. This value is accessible from all scripts.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void StoreEntityInteger(string entityName, string key, int value)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity))
        return;
      if (entity.EntityStorage == null)
        entity.EntityStorage = new MyEntityStorageComponent();
      entity.EntityStorage.Write(key, value);
    }

    [VisualScriptingMiscData("Entity Storage", "Stores long integer in the entity storage. This value is accessible from all scripts.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void StoreEntityLong(string entityName, string key, long value)
    {
      MyEntity entity;
      if (string.IsNullOrEmpty(entityName) || !Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity))
        return;
      if (entity.EntityStorage == null)
        entity.EntityStorage = new MyEntityStorageComponent();
      entity.EntityStorage.Write(key, value);
    }

    [VisualScriptingMiscData("Entity Storage", "Stores float in the entity storage. This value is accessible from all scripts.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void StoreEntityFloat(string entityName, string key, float value)
    {
      MyEntity entity;
      if (string.IsNullOrEmpty(entityName) || !Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity))
        return;
      if (entity.EntityStorage == null)
        entity.EntityStorage = new MyEntityStorageComponent();
      entity.EntityStorage.Write(key, value);
    }

    [VisualScriptingMiscData("Entity Storage", "Stores Vector3D (doubles) in the entity storage. This value is accessible from all scripts.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void StoreEntityVector(string entityName, string key, Vector3D value)
    {
      MyEntity entity;
      if (string.IsNullOrEmpty(entityName) || !Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity))
        return;
      if (entity.EntityStorage == null)
        entity.EntityStorage = new MyEntityStorageComponent();
      entity.EntityStorage.Write(key, value);
    }

    [VisualScriptingMiscData("Entity Storage", "Loads string from the entity storage.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static string LoadEntityString(string entityName, string key)
    {
      MyEntity entity;
      return string.IsNullOrEmpty(entityName) || !Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity) || entity.EntityStorage == null ? "" : entity.EntityStorage.ReadString(key);
    }

    [VisualScriptingMiscData("Entity Storage", "Loads boolean from the entity storage.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool LoadEntityBool(string entityName, string key)
    {
      MyEntity entity;
      return !string.IsNullOrEmpty(entityName) && Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity) && entity.EntityStorage != null && entity.EntityStorage.ReadBool(key);
    }

    [VisualScriptingMiscData("Entity Storage", "Loads integer from the entity storage.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static int LoadEntityInteger(string entityName, string key)
    {
      MyEntity entity;
      return string.IsNullOrEmpty(entityName) || !Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity) || entity.EntityStorage == null ? 0 : entity.EntityStorage.ReadInt(key);
    }

    [VisualScriptingMiscData("Entity Storage", "Loads long integer from the entity storage.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static long LoadEntityLong(string entityName, string key)
    {
      MyEntity entity;
      return Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity) && entity.EntityStorage != null ? entity.EntityStorage.ReadLong(key) : 0L;
    }

    [VisualScriptingMiscData("Entity Storage", "Loads float from the entity storage.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static float LoadEntityFloat(string entityName, string key)
    {
      MyEntity entity;
      return string.IsNullOrEmpty(entityName) || !Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity) || entity.EntityStorage == null ? 0.0f : entity.EntityStorage.ReadFloat(key);
    }

    [VisualScriptingMiscData("Entity Storage", "Loads Vector3D (doubles) from the entity storage.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static Vector3D LoadEntityVector(string entityName, string key)
    {
      MyEntity entity;
      return string.IsNullOrEmpty(entityName) || !Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity) || entity.EntityStorage == null ? Vector3D.Zero : entity.EntityStorage.ReadVector3D(key);
    }

    [VisualScriptingMiscData("Entity Storage", "Loads string list from the entity storage.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static List<string> LoadEntityStringList(string entityName, string key)
    {
      if (string.IsNullOrEmpty(entityName))
        return new List<string>();
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity))
        return new List<string>();
      return entity.EntityStorage == null ? new List<string>() : entity.EntityStorage.ReadStringList(key);
    }

    [VisualScriptingMiscData("Entity Storage", "Loads boolean list from the entity storage.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static List<bool> LoadEntityBoolList(string entityName, string key)
    {
      if (string.IsNullOrEmpty(entityName))
        return new List<bool>();
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity))
        return new List<bool>();
      if (entity.EntityStorage == null)
      {
        entity.EntityStorage = new MyEntityStorageComponent();
        entity.EntityStorage.Write(key, new List<bool>());
      }
      return entity.EntityStorage.ReadBoolList(key);
    }

    [VisualScriptingMiscData("Entity Storage", "Loads integer list from the entity storage.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static List<int> LoadEntityIntegerList(string entityName, string key)
    {
      if (string.IsNullOrEmpty(entityName))
        return new List<int>();
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity))
        return new List<int>();
      if (entity.EntityStorage == null)
      {
        entity.EntityStorage = new MyEntityStorageComponent();
        entity.EntityStorage.Write(key, new List<int>());
      }
      return entity.EntityStorage.ReadIntList(key);
    }

    [VisualScriptingMiscData("Entity Storage", "Loads long list from the entity storage.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static List<long> LoadEntityLongList(string entityName, string key)
    {
      if (string.IsNullOrEmpty(entityName))
        return new List<long>();
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity))
        return new List<long>();
      if (entity.EntityStorage == null)
      {
        entity.EntityStorage = new MyEntityStorageComponent();
        entity.EntityStorage.Write(key, new List<long>());
      }
      return entity.EntityStorage.ReadLongList(key);
    }

    [VisualScriptingMiscData("Entity Storage", "Loads float from the entity storage.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static List<float> LoadEntityFloatList(string entityName, string key)
    {
      if (string.IsNullOrEmpty(entityName))
        return new List<float>();
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity))
        return new List<float>();
      if (entity.EntityStorage == null)
      {
        entity.EntityStorage = new MyEntityStorageComponent();
        entity.EntityStorage.Write(key, new List<float>());
      }
      return entity.EntityStorage.ReadFloatList(key);
    }

    [VisualScriptingMiscData("Entity Storage", "Loads Vector3D (doubles) from the entity storage.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static List<Vector3D> LoadEntityVectorList(string entityName, string key)
    {
      if (string.IsNullOrEmpty(entityName))
        return new List<Vector3D>();
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity))
        return new List<Vector3D>();
      if (entity.EntityStorage == null)
      {
        entity.EntityStorage = new MyEntityStorageComponent();
        entity.EntityStorage.Write(key, new List<Vector3D>());
      }
      return entity.EntityStorage.ReadVector3DList(key);
    }

    [VisualScriptingMiscData("Entity Storage", "Stores list of string in the entity storage. This value is accessible from all scripts.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void StoreEntityStringList(string entityName, string key, List<string> value)
    {
      MyEntity entity;
      if (string.IsNullOrEmpty(entityName) || !Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity))
        return;
      if (entity.EntityStorage == null)
        entity.EntityStorage = new MyEntityStorageComponent();
      entity.EntityStorage.Write(key, value);
    }

    [VisualScriptingMiscData("Entity Storage", "Stores boolean list in the entity storage. This value is accessible from all scripts.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void StoreEntityBoolList(string entityName, string key, List<bool> value)
    {
      MyEntity entity;
      if (string.IsNullOrEmpty(entityName) || !Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity))
        return;
      if (entity.EntityStorage == null)
        entity.EntityStorage = new MyEntityStorageComponent();
      entity.EntityStorage.Write(key, value);
    }

    [VisualScriptingMiscData("Entity Storage", "Stores integer list in the entity storage. This value is accessible from all scripts.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void StoreEntityIntegerList(string entityName, string key, List<int> value)
    {
      MyEntity entity;
      if (string.IsNullOrEmpty(entityName) || !Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity))
        return;
      if (entity.EntityStorage == null)
        entity.EntityStorage = new MyEntityStorageComponent();
      entity.EntityStorage.Write(key, value);
    }

    [VisualScriptingMiscData("Entity Storage", "Stores long list in the entity storage. This value is accessible from all scripts.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void StoreEntityLongList(string entityName, string key, List<long> value)
    {
      MyEntity entity;
      if (string.IsNullOrEmpty(entityName) || !Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity))
        return;
      if (entity.EntityStorage == null)
        entity.EntityStorage = new MyEntityStorageComponent();
      entity.EntityStorage.Write(key, value);
    }

    [VisualScriptingMiscData("Entity Storage", "Stores float list in the entity storage. This value is accessible from all scripts.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void StoreEntityFloatList(string entityName, string key, List<float> value)
    {
      MyEntity entity;
      if (string.IsNullOrEmpty(entityName) || !Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity))
        return;
      if (entity.EntityStorage == null)
        entity.EntityStorage = new MyEntityStorageComponent();
      entity.EntityStorage.Write(key, value);
    }

    [VisualScriptingMiscData("Entity Storage", "Stores Vector3D list in the entity storage. This value is accessible from all scripts.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void StoreEntityVectorList(string entityName, string key, List<Vector3D> value)
    {
      MyEntity entity;
      if (string.IsNullOrEmpty(entityName) || !Sandbox.Game.Entities.MyEntities.TryGetEntityByName(entityName, out entity))
        return;
      if (entity.EntityStorage == null)
        entity.EntityStorage = new MyEntityStorageComponent();
      entity.EntityStorage.Write(key, value);
    }

    [VisualScriptingMiscData("Timers", "Start a timer with a specified key.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void StartGameplayTimer(string key, bool reset)
    {
      if (!MyVisualScriptLogicProvider.m_timers.ContainsKey(key) | reset)
      {
        MyVisualScriptLogicProvider.m_timers[key] = (MySandboxGame.TotalGamePlayTimeInMilliseconds, true);
      }
      else
      {
        if (MyVisualScriptLogicProvider.m_timers[key].Running)
          return;
        MyVisualScriptLogicProvider.m_timers[key] = (MySandboxGame.TotalGamePlayTimeInMilliseconds - MyVisualScriptLogicProvider.m_timers[key].Time, true);
      }
    }

    [VisualScriptingMiscData("Timers", "Stop timer running.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void StopGameplayTimer(string key, bool reset = false)
    {
      if (!reset)
      {
        if (!MyVisualScriptLogicProvider.m_timers.ContainsKey(key) || !MyVisualScriptLogicProvider.m_timers[key].Running)
          return;
        MyVisualScriptLogicProvider.m_timers[key] = (MySandboxGame.TotalGamePlayTimeInMilliseconds - MyVisualScriptLogicProvider.m_timers[key].Time, false);
      }
      else
        MyVisualScriptLogicProvider.m_timers[key] = (0, false);
    }

    [VisualScriptingMiscData("Timers", "Return elapsed time in milliseconds for given timer.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static int GetGameplayElapsedTime(string key)
    {
      if (!MyVisualScriptLogicProvider.m_timers.ContainsKey(key))
        return 0;
      return !MyVisualScriptLogicProvider.m_timers[key].Running ? MyVisualScriptLogicProvider.m_timers[key].Time : MySandboxGame.TotalGamePlayTimeInMilliseconds - MyVisualScriptLogicProvider.m_timers[key].Time;
    }

    [VisualScriptingMiscData("Timers", "Return elapsed time in milliseconds for given timer. Format according to https://docs.microsoft.com/cs-cz/dotnet/api/system.timespan.tostring", -10510688)]
    [VisualScriptingMember(false, false)]
    public static string GetGameplayElapsedTimeToString(string key, string format) => TimeSpan.FromMilliseconds((double) MyVisualScriptLogicProvider.GetGameplayElapsedTime(key)).ToString(format, (IFormatProvider) CultureInfo.InvariantCulture);

    [VisualScriptingMiscData("UIString", "Create UI string on specific coordinates. PlayerId -1 stands for all players, 0 for local player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void CreateUIString(
      long id,
      string text,
      float normalizedXPos,
      float normalizedYPos,
      float scale = 1f,
      string font = "White",
      MyGuiDrawAlignEnum drawAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
      long playerId = -1)
    {
      long num = playerId != 0L || MySession.Static.LocalCharacter == null ? playerId : MySession.Static.LocalCharacter.GetPlayerIdentityId();
      MyUIString myUiString = new MyUIString()
      {
        Text = text,
        NormalizedCoord = new Vector2(normalizedXPos, normalizedYPos),
        Scale = scale,
        Font = font,
        DrawAlign = drawAlign
      };
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long, MyUIString, long>((Func<IMyEventOwner, Action<long, MyUIString, long>>) (s => new Action<long, MyUIString, long>(MyVisualScriptLogicProvider.CreateUIStringSync)), id, myUiString, num);
    }

    [Event(null, 8892)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void CreateUIStringSync(long id, MyUIString UIString, long playerId = -1)
    {
      if (playerId != -1L && !Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalPlayerId != playerId)
        return;
      MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.CreateUIString(id, UIString);
    }

    [VisualScriptingMiscData("UIString", "Remove UI string with specific id. PlayerId -1 stands for all players, 0 for local player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void RemoveUIString(long id, long playerId = -1)
    {
      long num = playerId != 0L || MySession.Static.LocalCharacter == null ? playerId : MySession.Static.LocalCharacter.GetPlayerIdentityId();
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long, long>((Func<IMyEventOwner, Action<long, long>>) (s => new Action<long, long>(MyVisualScriptLogicProvider.RemoveUIStringSync)), id, num);
    }

    [Event(null, 8913)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void RemoveUIStringSync(long id, long playerId = -1)
    {
      if (playerId != -1L && !Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalPlayerId != playerId)
        return;
      MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.RemoveUIString(id);
    }

    [VisualScriptingMiscData("Board screen", "Create board screen on specific coordinates. PlayerId -1 stands for all players, 0 for local player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void CreateBoardScreen(
      string boardId,
      float normalizedPosX,
      float normalizedPosY,
      float normalizedSizeX,
      float normalizedSizeY,
      long playerId = -1)
    {
      long num = playerId != 0L || MySession.Static.LocalCharacter == null ? playerId : MySession.Static.LocalCharacter.GetPlayerIdentityId();
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, float, float, float, float, long>((Func<IMyEventOwner, Action<string, float, float, float, float, long>>) (s => new Action<string, float, float, float, float, long>(MyVisualScriptLogicProvider.CreateBoardScreenSync)), boardId, normalizedPosX, normalizedPosY, normalizedSizeX, normalizedSizeY, num);
    }

    [Event(null, 8938)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void CreateBoardScreenSync(
      string boardId,
      float normalizedPosX,
      float normalizedPosY,
      float normalizedSizeX,
      float normalizedSizeY,
      long playerId = -1)
    {
      if (playerId != -1L && !Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalPlayerId != playerId)
        return;
      MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.CreateBoardScreen(boardId, normalizedPosX, normalizedPosY, normalizedSizeX, normalizedSizeY);
    }

    [VisualScriptingMiscData("Board screen", "Remove board screen with specific id. PlayerId -1 stands for all players, 0 for local player.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void RemoveBoardScreen(string boardId, long playerId = -1)
    {
      long num = playerId != 0L || MySession.Static.LocalCharacter == null ? playerId : MySession.Static.LocalCharacter.GetPlayerIdentityId();
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, long>((Func<IMyEventOwner, Action<string, long>>) (s => new Action<string, long>(MyVisualScriptLogicProvider.RemoveBoardScreenSync)), boardId, num);
    }

    [Event(null, 8960)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void RemoveBoardScreenSync(string boardId, long playerId = -1)
    {
      if (playerId != -1L && !Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalPlayerId != playerId)
        return;
      MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.RemoveBoardScreen(boardId);
    }

    [VisualScriptingMiscData("Board screen", "Add column to board screen.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void AddColumn(
      string boardId,
      string columnId,
      float width,
      string headerText,
      MyGuiDrawAlignEnum headerDrawAlign,
      MyGuiDrawAlignEnum columnDrawAlign,
      long playerId = -1)
    {
      long num = playerId != 0L || MySession.Static.LocalCharacter == null ? playerId : MySession.Static.LocalCharacter.GetPlayerIdentityId();
      MyGuiScreenBoard.MyColumn myColumn = new MyGuiScreenBoard.MyColumn()
      {
        ColumnDrawAlign = columnDrawAlign,
        HeaderDrawAlign = headerDrawAlign,
        HeaderText = headerText,
        Width = width,
        Visible = true
      };
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, string, MyGuiScreenBoard.MyColumn, long>((Func<IMyEventOwner, Action<string, string, MyGuiScreenBoard.MyColumn, long>>) (s => new Action<string, string, MyGuiScreenBoard.MyColumn, long>(MyVisualScriptLogicProvider.AddColumnSync)), boardId, columnId, myColumn, num);
    }

    [Event(null, 8991)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void AddColumnSync(
      string boardId,
      string columnId,
      MyGuiScreenBoard.MyColumn column,
      long playerId = -1)
    {
      if (playerId != -1L && !Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalPlayerId != playerId)
        return;
      MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.AddColumn(boardId, columnId, column);
    }

    [VisualScriptingMiscData("Board screen", "Remove column from board screen.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void RemoveColumn(string boardId, string columnId, long playerId = -1)
    {
      long num = playerId != 0L || MySession.Static.LocalCharacter == null ? playerId : MySession.Static.LocalCharacter.GetPlayerIdentityId();
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, string, long>((Func<IMyEventOwner, Action<string, string, long>>) (s => new Action<string, string, long>(MyVisualScriptLogicProvider.RemoveColumnSync)), boardId, columnId, num);
    }

    [Event(null, 9013)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void RemoveColumnSync(string boardId, string columnId, long playerId = -1)
    {
      if (playerId != -1L && !Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalPlayerId != playerId)
        return;
      MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.RemoveColumn(boardId, columnId);
    }

    [VisualScriptingMiscData("Board screen", "Add row to board screen.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void AddRow(string boardId, string rowId, long playerId = -1)
    {
      long num = playerId != 0L || MySession.Static.LocalCharacter == null ? playerId : MySession.Static.LocalCharacter.GetPlayerIdentityId();
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, string, long>((Func<IMyEventOwner, Action<string, string, long>>) (s => new Action<string, string, long>(MyVisualScriptLogicProvider.AddRowSync)), boardId, rowId, num);
    }

    [Event(null, 9035)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void AddRowSync(string boardId, string rowId, long playerId = -1)
    {
      if (playerId != -1L && !Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalPlayerId != playerId)
        return;
      MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.AddRow(boardId, rowId);
    }

    [VisualScriptingMiscData("Board screen", "Add row to board screen.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void RemoveRow(string boardId, string rowId, long playerId = -1)
    {
      long num = playerId != 0L || MySession.Static.LocalCharacter == null ? playerId : MySession.Static.LocalCharacter.GetPlayerIdentityId();
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, string, long>((Func<IMyEventOwner, Action<string, string, long>>) (s => new Action<string, string, long>(MyVisualScriptLogicProvider.RemoveRowSync)), boardId, rowId, num);
    }

    [Event(null, 9057)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void RemoveRowSync(string boardId, string rowId, long playerId = -1)
    {
      if (playerId != -1L && !Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalPlayerId != playerId)
        return;
      MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.RemoveRow(boardId, rowId);
    }

    [VisualScriptingMiscData("Board screen", "Set cell text in the board screen.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetCell(
      string boardId,
      string rowId,
      string columnId,
      string text,
      long playerId = -1)
    {
      long num = playerId != 0L || MySession.Static.LocalCharacter == null ? playerId : MySession.Static.LocalCharacter.GetPlayerIdentityId();
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, string, string, string, long>((Func<IMyEventOwner, Action<string, string, string, string, long>>) (s => new Action<string, string, string, string, long>(MyVisualScriptLogicProvider.SetCellSync)), boardId, rowId, columnId, text, num);
    }

    [Event(null, 9079)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void SetCellSync(
      string boardId,
      string rowId,
      string columnId,
      string text,
      long playerId = -1)
    {
      if (playerId != -1L && !Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalPlayerId != playerId)
        return;
      MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.SetCell(boardId, rowId, columnId, text);
    }

    [VisualScriptingMiscData("Board screen", "Set row ranking used for sorting.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetRowRanking(string boardId, string rowId, int ranking, long playerId = -1)
    {
      long num = playerId != 0L || MySession.Static.LocalCharacter == null ? playerId : MySession.Static.LocalCharacter.GetPlayerIdentityId();
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, string, int, long>((Func<IMyEventOwner, Action<string, string, int, long>>) (s => new Action<string, string, int, long>(MyVisualScriptLogicProvider.SetRowRankingSync)), boardId, rowId, ranking, num);
    }

    [Event(null, 9101)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void SetRowRankingSync(
      string boardId,
      string rowId,
      int ranking,
      long playerId = -1)
    {
      if (playerId != -1L && !Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalPlayerId != playerId)
        return;
      MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.SetRowRanking(boardId, rowId, ranking);
    }

    [VisualScriptingMiscData("Board screen", "Set row ranking used for sorting.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SortByColumn(
      string boardId,
      string columnId,
      bool ascending,
      long playerId = -1)
    {
      long num = playerId != 0L || MySession.Static.LocalCharacter == null ? playerId : MySession.Static.LocalCharacter.GetPlayerIdentityId();
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, string, bool, long>((Func<IMyEventOwner, Action<string, string, bool, long>>) (s => new Action<string, string, bool, long>(MyVisualScriptLogicProvider.SortByColumnSync)), boardId, columnId, ascending, num);
    }

    [Event(null, 9123)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void SortByColumnSync(
      string boardId,
      string columnId,
      bool ascending,
      long playerId = -1)
    {
      if (playerId != -1L && !Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalPlayerId != playerId)
        return;
      MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.SortByColumn(boardId, columnId, ascending);
    }

    [VisualScriptingMiscData("Board screen", "Set row ranking used for sorting.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SortByRanking(string boardId, bool ascending, long playerId = -1)
    {
      long num = playerId != 0L || MySession.Static.LocalCharacter == null ? playerId : MySession.Static.LocalCharacter.GetPlayerIdentityId();
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, bool, long>((Func<IMyEventOwner, Action<string, bool, long>>) (s => new Action<string, bool, long>(MyVisualScriptLogicProvider.SortByRankingSync)), boardId, ascending, num);
    }

    [Event(null, 9145)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void SortByRankingSync(string boardId, bool ascending, long playerId = -1)
    {
      if (playerId != -1L && !Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalPlayerId != playerId)
        return;
      MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.SortByRanking(boardId, ascending);
    }

    [VisualScriptingMiscData("Board screen", "Set row ranking used for sorting.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void ShowOrderInColumn(string boardId, string columnId, long playerId = -1)
    {
      long num = playerId != 0L || MySession.Static.LocalCharacter == null ? playerId : MySession.Static.LocalCharacter.GetPlayerIdentityId();
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, string, long>((Func<IMyEventOwner, Action<string, string, long>>) (s => new Action<string, string, long>(MyVisualScriptLogicProvider.ShowOrderInColumnSync)), boardId, columnId, num);
    }

    [Event(null, 9167)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void ShowOrderInColumnSync(string boardId, string columnId, long playerId = -1)
    {
      if (playerId != -1L && !Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalPlayerId != playerId)
        return;
      MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.ShowOrderInColumn(boardId, columnId);
    }

    [VisualScriptingMiscData("Board screen", "Set column visible state.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetColumnVisibility(
      string boardId,
      string columnId,
      bool visible,
      long playerId = -1)
    {
      long num = playerId != 0L || MySession.Static.LocalCharacter == null ? playerId : MySession.Static.LocalCharacter.GetPlayerIdentityId();
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, string, bool, long>((Func<IMyEventOwner, Action<string, string, bool, long>>) (s => new Action<string, string, bool, long>(MyVisualScriptLogicProvider.SetColumnVisibilitySync)), boardId, columnId, visible, num);
    }

    [Event(null, 9190)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void SetColumnVisibilitySync(
      string boardId,
      string columnId,
      bool visible,
      long playerId = -1)
    {
      if (playerId != -1L && !Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalPlayerId != playerId)
        return;
      MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.SetColumnVisibility(boardId, columnId, visible);
    }

    [VisualScriptingMember(false, false)]
    [VisualScriptingMiscData("Other", "Enum constants", -10510688)]
    public static VRage.Network.JoinResult JoinResult(VRage.Network.JoinResult value) => value;

    [VisualScriptingMiscData("Other", "Get name of the current state of the match.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static string GetMatchState()
    {
      MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
      return component == null ? string.Empty : component.State.ToString();
    }

    [VisualScriptingMiscData("Other", "Immediately progress match into next phase. Returns name of new state.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static string AdvanceMatchState()
    {
      MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
      if (component == null)
        return string.Empty;
      component.AdvanceToNextState();
      return component.State.ToString();
    }

    [VisualScriptingMiscData("Other", "Get remaining time of the current match state. Time is in minutes.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static float GetMatchStateRemainingDuration()
    {
      MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
      return component == null ? 0.0f : component.RemainingMinutes;
    }

    [VisualScriptingMiscData("Other", "Set remaining time of the current match state. Time is in minutes. If the value is lower or equal to zero, next match phase will start.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetMatchStateRemainingDuration(float time) => MySession.Static.GetComponent<MySessionComponentMatch>()?.SetRemainingTime(time);

    [VisualScriptingMiscData("Other", "Get remaining time of the current match state. Time is in minutes. Positive value will lenghten the phase, negative will shorten it. If remaining duration becomes lower or equal to zero, next match phase will start", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void AddMatchStateRemainingDuration(float time) => MySession.Static.GetComponent<MySessionComponentMatch>()?.AddRemainingTime(time);

    [VisualScriptingMiscData("Other", "Sets whether match component should be runing or not. When component is running, remaining time of the state will be ticking out and states will be changing. If component is not running, it will be effectively paused until running again.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetMatchIsRunningState(bool isRunning) => MySession.Static.GetComponent<MySessionComponentMatch>()?.SetIsRunning(isRunning);

    [VisualScriptingMiscData("Other", "Get information whether component handling the match is running or not. Running component will have the time advancing and states will be changing.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static bool GetMatchIsRunningState()
    {
      MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
      return component != null && component.IsRunning;
    }

    private class AllowedAchievementsHelper
    {
      public static readonly List<string> AllowedAchievements = new List<string>();

      static AllowedAchievementsHelper()
      {
        MyVisualScriptLogicProvider.AllowedAchievementsHelper.AllowedAchievements.Add("Promoted_engineer");
        MyVisualScriptLogicProvider.AllowedAchievementsHelper.AllowedAchievements.Add("Engineering_degree");
        MyVisualScriptLogicProvider.AllowedAchievementsHelper.AllowedAchievements.Add("Planetesphobia");
        MyVisualScriptLogicProvider.AllowedAchievementsHelper.AllowedAchievements.Add("Rapid_disassembly");
        MyVisualScriptLogicProvider.AllowedAchievementsHelper.AllowedAchievements.Add("It_takes_but_one");
        MyVisualScriptLogicProvider.AllowedAchievementsHelper.AllowedAchievements.Add("I_see_dead_drones");
        MyVisualScriptLogicProvider.AllowedAchievementsHelper.AllowedAchievements.Add("Bring_it_on");
        MyVisualScriptLogicProvider.AllowedAchievementsHelper.AllowedAchievements.Add("Im_doing_my_part");
        MyVisualScriptLogicProvider.AllowedAchievementsHelper.AllowedAchievements.Add("Scrap_delivery");
        MyVisualScriptLogicProvider.AllowedAchievementsHelper.AllowedAchievements.Add("Joint_operation");
        MyVisualScriptLogicProvider.AllowedAchievementsHelper.AllowedAchievements.Add("Flak_fodde");
        MyVisualScriptLogicProvider.AllowedAchievementsHelper.AllowedAchievements.Add("MyAchievement_PlayingItCool");
      }
    }

    protected sealed class MusicPlayMusicCueSync\u003C\u003ESystem_String\u0023System_Boolean : ICallSite<IMyEventOwner, string, bool, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string cueName,
        in bool playAtLeastOnce,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.MusicPlayMusicCueSync(cueName, playAtLeastOnce);
      }
    }

    protected sealed class MusicPlayMusicCategorySync\u003C\u003ESystem_String\u0023System_Boolean\u0023System_Int64 : ICallSite<IMyEventOwner, string, bool, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string categoryName,
        in bool playAtLeastOnce,
        in long playerId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.MusicPlayMusicCategorySync(categoryName, playAtLeastOnce, playerId);
      }
    }

    protected sealed class MusicSetMusicCategorySync\u003C\u003ESystem_String : ICallSite<IMyEventOwner, string, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string categoryName,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.MusicSetMusicCategorySync(categoryName);
      }
    }

    protected sealed class MusicSetDynamicMusicSync\u003C\u003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in bool enabled,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.MusicSetDynamicMusicSync(enabled);
      }
    }

    protected sealed class PlaySingleSoundAtEntitySync\u003C\u003ESystem_String\u0023System_String : ICallSite<IMyEventOwner, string, string, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string soundName,
        in string entityName,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.PlaySingleSoundAtEntitySync(soundName, entityName);
      }
    }

    protected sealed class PlayHudSoundSync\u003C\u003EVRage_Audio_MyGuiSounds\u0023System_Int64 : ICallSite<IMyEventOwner, MyGuiSounds, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyGuiSounds sound,
        in long playerId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.PlayHudSoundSync(sound, playerId);
      }
    }

    protected sealed class PlaySingleSoundAtPositionSync\u003C\u003ESystem_String\u0023VRageMath_Vector3D : ICallSite<IMyEventOwner, string, Vector3D, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string soundName,
        in Vector3D position,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.PlaySingleSoundAtPositionSync(soundName, position);
      }
    }

    protected sealed class CreateSoundEmitterAtEntitySync\u003C\u003ESystem_String\u0023System_String : ICallSite<IMyEventOwner, string, string, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string newEmitterId,
        in string entityName,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.CreateSoundEmitterAtEntitySync(newEmitterId, entityName);
      }
    }

    protected sealed class CreateSoundEmitterAtPositionSync\u003C\u003ESystem_String\u0023VRageMath_Vector3D : ICallSite<IMyEventOwner, string, Vector3D, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string newEmitterId,
        in Vector3D position,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.CreateSoundEmitterAtPositionSync(newEmitterId, position);
      }
    }

    protected sealed class PlaySoundSync\u003C\u003ESystem_String\u0023System_String\u0023System_Boolean : ICallSite<IMyEventOwner, string, string, bool, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string emitterId,
        in string soundName,
        in bool playIn2D,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.PlaySoundSync(emitterId, soundName, playIn2D);
      }
    }

    protected sealed class StopSoundSync\u003C\u003ESystem_String\u0023System_Boolean : ICallSite<IMyEventOwner, string, bool, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string emitterId,
        in bool forced,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.StopSoundSync(emitterId, forced);
      }
    }

    protected sealed class RemoveSoundEmitterSync\u003C\u003ESystem_String : ICallSite<IMyEventOwner, string, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string emitterId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.RemoveSoundEmitterSync(emitterId);
      }
    }

    protected sealed class PlaySoundAmbientSync\u003C\u003ESystem_String : ICallSite<IMyEventOwner, string, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string soundName,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.PlaySoundAmbientSync(soundName);
      }
    }

    protected sealed class SetVolumeSync\u003C\u003ESystem_Single\u0023System_Single\u0023System_Single : ICallSite<IMyEventOwner, float, float, float, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in float gameVolume,
        in float musicVolume,
        in float voiceChatVolume,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.SetVolumeSync(gameVolume, musicVolume, voiceChatVolume);
      }
    }

    protected sealed class StartCutsceneSync\u003C\u003ESystem_String\u0023System_Boolean\u0023System_Int64 : ICallSite<IMyEventOwner, string, bool, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string cutsceneName,
        in bool registerEvents,
        in long playerId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.StartCutsceneSync(cutsceneName, registerEvents, playerId);
      }
    }

    protected sealed class NextCutsceneNodeSync\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long playerId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.NextCutsceneNodeSync(playerId);
      }
    }

    protected sealed class EndCutsceneSync\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long playerId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.EndCutsceneSync(playerId);
      }
    }

    protected sealed class SkipCutsceneSync\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long playerId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.SkipCutsceneSync(playerId);
      }
    }

    protected sealed class RequestVicinityCacheSync\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
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
        MyVisualScriptLogicProvider.RequestVicinityCacheSync(entityId);
      }
    }

    protected sealed class CreateExplosionInternalAll\u003C\u003EVRageMath_Vector3D\u0023System_Single\u0023System_Int32 : ICallSite<IMyEventOwner, Vector3D, float, int, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in Vector3D position,
        in float radius,
        in int damage,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.CreateExplosionInternalAll(position, radius, damage);
      }
    }

    protected sealed class CreateParticleEffectAtPositionInternalAll\u003C\u003ESystem_String\u0023VRageMath_Vector3D : ICallSite<IMyEventOwner, string, Vector3D, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string effectName,
        in Vector3D position,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.CreateParticleEffectAtPositionInternalAll(effectName, position);
      }
    }

    protected sealed class CreateParticleEffectAtEntityInternalAll\u003C\u003ESystem_String\u0023System_String : ICallSite<IMyEventOwner, string, string, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string effectName,
        in string entityName,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.CreateParticleEffectAtEntityInternalAll(effectName, entityName);
      }
    }

    protected sealed class ScreenColorFadingStartInternal\u003C\u003ESystem_Single\u0023System_Boolean : ICallSite<IMyEventOwner, float, bool, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in float time,
        in bool toOpaque,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.ScreenColorFadingStartInternal(time, toOpaque);
      }
    }

    protected sealed class ScreenColorFadingStartInternalAll\u003C\u003ESystem_Single\u0023System_Boolean : ICallSite<IMyEventOwner, float, bool, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in float time,
        in bool toOpaque,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.ScreenColorFadingStartInternalAll(time, toOpaque);
      }
    }

    protected sealed class ScreenColorFadingSetColorInternal\u003C\u003EVRageMath_Color : ICallSite<IMyEventOwner, Color, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in Color color,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.ScreenColorFadingSetColorInternal(color);
      }
    }

    protected sealed class ScreenColorFadingSetColorInternalAll\u003C\u003EVRageMath_Color : ICallSite<IMyEventOwner, Color, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in Color color,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.ScreenColorFadingSetColorInternalAll(color);
      }
    }

    protected sealed class ScreenColorFadingStartSwitchInternal\u003C\u003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in float time,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.ScreenColorFadingStartSwitchInternal(time);
      }
    }

    protected sealed class ScreenColorFadingStartSwitchInternalAll\u003C\u003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in float time,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.ScreenColorFadingStartSwitchInternalAll(time);
      }
    }

    protected sealed class ScreenColorFadingMinimalizeHUDInternal\u003C\u003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in bool minimalize,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.ScreenColorFadingMinimalizeHUDInternal(minimalize);
      }
    }

    protected sealed class ScreenColorFadingMinimalizeHUDInternalAll\u003C\u003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in bool minimalize,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.ScreenColorFadingMinimalizeHUDInternalAll(minimalize);
      }
    }

    protected sealed class ShowHudInternal\u003C\u003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in bool flag,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.ShowHudInternal(flag);
      }
    }

    protected sealed class ShowHudInternalAll\u003C\u003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in bool flag,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.ShowHudInternalAll(flag);
      }
    }

    protected sealed class SetHudStateInternal\u003C\u003ESystem_Int32 : ICallSite<IMyEventOwner, int, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in int state,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.SetHudStateInternal(state);
      }
    }

    protected sealed class SetHudStateInternalAll\u003C\u003ESystem_Int32 : ICallSite<IMyEventOwner, int, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in int state,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.SetHudStateInternalAll(state);
      }
    }

    protected sealed class FogSetAllInternalAll\u003C\u003ESystem_Single\u0023System_Single\u0023VRageMath_Vector3\u0023System_Single\u0023System_Single : ICallSite<IMyEventOwner, float, float, Vector3, float, float, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in float density,
        in float multiplier,
        in Vector3 color,
        in float skyboxMultiplier,
        in float atmoMultiplier,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.FogSetAllInternalAll(density, multiplier, color, skyboxMultiplier, atmoMultiplier);
      }
    }

    protected sealed class EnableSavingSync\u003C\u003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in bool enable,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.EnableSavingSync(enable);
      }
    }

    protected sealed class EnableCustomRespawnSync\u003C\u003ESystem_Boolean\u0023System_Int64 : ICallSite<IMyEventOwner, bool, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in bool enable,
        in long playerId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.EnableCustomRespawnSync(enable, playerId);
      }
    }

    protected sealed class CustomRespawnRequestServer\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long playerId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.CustomRespawnRequestServer(playerId);
      }
    }

    protected sealed class SetHighlightSync\u003C\u003ESystem_String\u0023System_Int32\u0023System_Int32\u0023VRageMath_Color\u0023System_Int64\u0023System_String : ICallSite<IMyEventOwner, string, int, int, Color, long, string>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string entityName,
        in int thickness,
        in int pulseTimeInFrames,
        in Color color,
        in long playerId,
        in string subPartNames)
      {
        MyVisualScriptLogicProvider.SetHighlightSync(entityName, thickness, pulseTimeInFrames, color, playerId, subPartNames);
      }
    }

    protected sealed class OpenSteamOverlaySync\u003C\u003ESystem_String : ICallSite<IMyEventOwner, string, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string url,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.OpenSteamOverlaySync(url);
      }
    }

    protected sealed class OpenPlayerScreenAllBroadcast\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
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
        MyVisualScriptLogicProvider.OpenPlayerScreenAllBroadcast();
      }
    }

    protected sealed class OpenFactionVictoryScreenAllBroadcast\u003C\u003ESystem_String\u0023System_Single : ICallSite<IMyEventOwner, string, float, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string factionTag,
        in float timeInSec,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.OpenFactionVictoryScreenAllBroadcast(factionTag, timeInSec);
      }
    }

    protected sealed class ShowNotificationSync\u003C\u003ESystem_String\u0023System_Int32\u0023System_String : ICallSite<IMyEventOwner, string, int, string, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string message,
        in int disappearTimeMs,
        in string font,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.ShowNotificationSync(message, disappearTimeMs, font);
      }
    }

    protected sealed class ShowNotificationToAllSync\u003C\u003ESystem_String\u0023System_Int32\u0023System_String : ICallSite<IMyEventOwner, string, int, string, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string message,
        in int disappearTimeMs,
        in string font,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.ShowNotificationToAllSync(message, disappearTimeMs, font);
      }
    }

    protected sealed class AddNotificationSync\u003C\u003ESystem_String\u0023System_String\u0023System_Int32 : ICallSite<IMyEventOwner, string, string, int, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string message,
        in string font,
        in int notificationId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.AddNotificationSync(message, font, notificationId);
      }
    }

    protected sealed class AddNotificationToAllSync\u003C\u003ESystem_String\u0023System_String\u0023System_Int32 : ICallSite<IMyEventOwner, string, string, int, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string message,
        in string font,
        in int notificationId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.AddNotificationToAllSync(message, font, notificationId);
      }
    }

    protected sealed class RemoveNotificationSync\u003C\u003ESystem_Int32\u0023System_Int64 : ICallSite<IMyEventOwner, int, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in int messageId,
        in long playerId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.RemoveNotificationSync(messageId, playerId);
      }
    }

    protected sealed class ClearNotificationSync\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long playerId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.ClearNotificationSync(playerId);
      }
    }

    protected sealed class DisplayCongratulationScreenInternal\u003C\u003ESystem_Int32 : ICallSite<IMyEventOwner, int, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in int congratulationMessageId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.DisplayCongratulationScreenInternal(congratulationMessageId);
      }
    }

    protected sealed class DisplayCongratulationScreenInternalAll\u003C\u003ESystem_Int32 : ICallSite<IMyEventOwner, int, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in int congratulationMessageId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.DisplayCongratulationScreenInternalAll(congratulationMessageId);
      }
    }

    protected sealed class CloseRespawnScreen\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
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
        MyVisualScriptLogicProvider.CloseRespawnScreen();
      }
    }

    protected sealed class SetPlayerInputBlacklistStateSync\u003C\u003ESystem_String\u0023System_Int64\u0023System_Boolean : ICallSite<IMyEventOwner, string, long, bool, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string controlStringId,
        in long playerId,
        in bool enabled,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.SetPlayerInputBlacklistStateSync(controlStringId, playerId, enabled);
      }
    }

    protected sealed class ToggleAbilityToSprintSync\u003C\u003ESystem_Int64\u0023System_Boolean : ICallSite<IMyEventOwner, long, bool, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long playerId,
        in bool canSprint,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.ToggleAbilityToSprintSync(playerId, canSprint);
      }
    }

    protected sealed class SetQuestlogSync\u003C\u003ESystem_Boolean\u0023System_String\u0023System_Int64 : ICallSite<IMyEventOwner, bool, string, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in bool visible,
        in string questName,
        in long playerId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.SetQuestlogSync(visible, questName, playerId);
      }
    }

    protected sealed class SetQuestlogTitleSync\u003C\u003ESystem_String\u0023System_Int64 : ICallSite<IMyEventOwner, string, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string questName,
        in long playerId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.SetQuestlogTitleSync(questName, playerId);
      }
    }

    protected sealed class AddQuestlogDetailSync\u003C\u003ESystem_String\u0023System_Boolean\u0023System_Boolean\u0023System_Int64 : ICallSite<IMyEventOwner, string, bool, bool, long, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string questDetailRow,
        in bool completePrevious,
        in bool useTyping,
        in long playerId,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.AddQuestlogDetailSync(questDetailRow, completePrevious, useTyping, playerId);
      }
    }

    protected sealed class AddQuestlogObjectiveSync\u003C\u003ESystem_String\u0023System_Boolean\u0023System_Boolean\u0023System_Int64 : ICallSite<IMyEventOwner, string, bool, bool, long, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string questDetailRow,
        in bool completePrevious,
        in bool useTyping,
        in long playerId,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.AddQuestlogObjectiveSync(questDetailRow, completePrevious, useTyping, playerId);
      }
    }

    protected sealed class SetQuestlogDetailCompletedSync\u003C\u003ESystem_Int32\u0023System_Boolean\u0023System_Int64 : ICallSite<IMyEventOwner, int, bool, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in int lineId,
        in bool completed,
        in long playerId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.SetQuestlogDetailCompletedSync(lineId, completed, playerId);
      }
    }

    protected sealed class SetAllQuestlogDetailsCompletedSync\u003C\u003ESystem_Boolean\u0023System_Int64 : ICallSite<IMyEventOwner, bool, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in bool completed,
        in long playerId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.SetAllQuestlogDetailsCompletedSync(completed, playerId);
      }
    }

    protected sealed class ReplaceQuestlogDetailSync\u003C\u003ESystem_Int32\u0023System_String\u0023System_Boolean\u0023System_Int64 : ICallSite<IMyEventOwner, int, string, bool, long, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in int id,
        in string newDetail,
        in bool useTyping,
        in long playerId,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.ReplaceQuestlogDetailSync(id, newDetail, useTyping, playerId);
      }
    }

    protected sealed class RemoveQuestlogDetailsSync\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long playerId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.RemoveQuestlogDetailsSync(playerId);
      }
    }

    protected sealed class SetQuestlogPageSync\u003C\u003ESystem_Int32\u0023System_Int64 : ICallSite<IMyEventOwner, int, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in int value,
        in long playerId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.SetQuestlogPageSync(value, playerId);
      }
    }

    protected sealed class SetQuestlogVisibleSync\u003C\u003ESystem_Boolean\u0023System_Int64 : ICallSite<IMyEventOwner, bool, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in bool value,
        in long playerId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.SetQuestlogVisibleSync(value, playerId);
      }
    }

    protected sealed class EnableHighlightSync\u003C\u003ESystem_Boolean\u0023System_Int64 : ICallSite<IMyEventOwner, bool, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in bool enable,
        in long playerId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.EnableHighlightSync(enable, playerId);
      }
    }

    protected sealed class SetToolbarSlotToItemSync\u003C\u003ESystem_Int32\u0023VRage_ObjectBuilders_SerializableDefinitionId\u0023System_Int64 : ICallSite<IMyEventOwner, int, SerializableDefinitionId, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in int slot,
        in SerializableDefinitionId itemId,
        in long playerId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.SetToolbarSlotToItemSync(slot, itemId, playerId);
      }
    }

    protected sealed class SwitchToolbarToSlotSync\u003C\u003ESystem_Int32\u0023System_Int64 : ICallSite<IMyEventOwner, int, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in int slot,
        in long playerId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.SwitchToolbarToSlotSync(slot, playerId);
      }
    }

    protected sealed class ClearToolbarSlotSync\u003C\u003ESystem_Int32\u0023System_Int64 : ICallSite<IMyEventOwner, int, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in int slot,
        in long playerId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.ClearToolbarSlotSync(slot, playerId);
      }
    }

    protected sealed class ClearAllToolbarSlotsSync\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long playerId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.ClearAllToolbarSlotsSync(playerId);
      }
    }

    protected sealed class SetToolbarPageSync\u003C\u003ESystem_Int32\u0023System_Int64 : ICallSite<IMyEventOwner, int, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in int page,
        in long playerId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.SetToolbarPageSync(page, playerId);
      }
    }

    protected sealed class ReloadToolbarDefaultsSync\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long playerId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.ReloadToolbarDefaultsSync(playerId);
      }
    }

    protected sealed class UnlockAchievementInternal\u003C\u003ESystem_Int32 : ICallSite<IMyEventOwner, int, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in int achievementId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.UnlockAchievementInternal(achievementId);
      }
    }

    protected sealed class UnlockAchievementInternalAll\u003C\u003ESystem_Int32 : ICallSite<IMyEventOwner, int, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in int achievementId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.UnlockAchievementInternalAll(achievementId);
      }
    }

    protected sealed class CreateUIStringSync\u003C\u003ESystem_Int64\u0023Sandbox_Game_MyUIString\u0023System_Int64 : ICallSite<IMyEventOwner, long, MyUIString, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long id,
        in MyUIString UIString,
        in long playerId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.CreateUIStringSync(id, UIString, playerId);
      }
    }

    protected sealed class RemoveUIStringSync\u003C\u003ESystem_Int64\u0023System_Int64 : ICallSite<IMyEventOwner, long, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long id,
        in long playerId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.RemoveUIStringSync(id, playerId);
      }
    }

    protected sealed class CreateBoardScreenSync\u003C\u003ESystem_String\u0023System_Single\u0023System_Single\u0023System_Single\u0023System_Single\u0023System_Int64 : ICallSite<IMyEventOwner, string, float, float, float, float, long>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string boardId,
        in float normalizedPosX,
        in float normalizedPosY,
        in float normalizedSizeX,
        in float normalizedSizeY,
        in long playerId)
      {
        MyVisualScriptLogicProvider.CreateBoardScreenSync(boardId, normalizedPosX, normalizedPosY, normalizedSizeX, normalizedSizeY, playerId);
      }
    }

    protected sealed class RemoveBoardScreenSync\u003C\u003ESystem_String\u0023System_Int64 : ICallSite<IMyEventOwner, string, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string boardId,
        in long playerId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.RemoveBoardScreenSync(boardId, playerId);
      }
    }

    protected sealed class AddColumnSync\u003C\u003ESystem_String\u0023System_String\u0023Sandbox_Game_Gui_MyGuiScreenBoard\u003C\u003EMyColumn\u0023System_Int64 : ICallSite<IMyEventOwner, string, string, MyGuiScreenBoard.MyColumn, long, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string boardId,
        in string columnId,
        in MyGuiScreenBoard.MyColumn column,
        in long playerId,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.AddColumnSync(boardId, columnId, column, playerId);
      }
    }

    protected sealed class RemoveColumnSync\u003C\u003ESystem_String\u0023System_String\u0023System_Int64 : ICallSite<IMyEventOwner, string, string, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string boardId,
        in string columnId,
        in long playerId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.RemoveColumnSync(boardId, columnId, playerId);
      }
    }

    protected sealed class AddRowSync\u003C\u003ESystem_String\u0023System_String\u0023System_Int64 : ICallSite<IMyEventOwner, string, string, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string boardId,
        in string rowId,
        in long playerId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.AddRowSync(boardId, rowId, playerId);
      }
    }

    protected sealed class RemoveRowSync\u003C\u003ESystem_String\u0023System_String\u0023System_Int64 : ICallSite<IMyEventOwner, string, string, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string boardId,
        in string rowId,
        in long playerId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.RemoveRowSync(boardId, rowId, playerId);
      }
    }

    protected sealed class SetCellSync\u003C\u003ESystem_String\u0023System_String\u0023System_String\u0023System_String\u0023System_Int64 : ICallSite<IMyEventOwner, string, string, string, string, long, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string boardId,
        in string rowId,
        in string columnId,
        in string text,
        in long playerId,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.SetCellSync(boardId, rowId, columnId, text, playerId);
      }
    }

    protected sealed class SetRowRankingSync\u003C\u003ESystem_String\u0023System_String\u0023System_Int32\u0023System_Int64 : ICallSite<IMyEventOwner, string, string, int, long, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string boardId,
        in string rowId,
        in int ranking,
        in long playerId,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.SetRowRankingSync(boardId, rowId, ranking, playerId);
      }
    }

    protected sealed class SortByColumnSync\u003C\u003ESystem_String\u0023System_String\u0023System_Boolean\u0023System_Int64 : ICallSite<IMyEventOwner, string, string, bool, long, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string boardId,
        in string columnId,
        in bool ascending,
        in long playerId,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.SortByColumnSync(boardId, columnId, ascending, playerId);
      }
    }

    protected sealed class SortByRankingSync\u003C\u003ESystem_String\u0023System_Boolean\u0023System_Int64 : ICallSite<IMyEventOwner, string, bool, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string boardId,
        in bool ascending,
        in long playerId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.SortByRankingSync(boardId, ascending, playerId);
      }
    }

    protected sealed class ShowOrderInColumnSync\u003C\u003ESystem_String\u0023System_String\u0023System_Int64 : ICallSite<IMyEventOwner, string, string, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string boardId,
        in string columnId,
        in long playerId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.ShowOrderInColumnSync(boardId, columnId, playerId);
      }
    }

    protected sealed class SetColumnVisibilitySync\u003C\u003ESystem_String\u0023System_String\u0023System_Boolean\u0023System_Int64 : ICallSite<IMyEventOwner, string, string, bool, long, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string boardId,
        in string columnId,
        in bool visible,
        in long playerId,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVisualScriptLogicProvider.SetColumnVisibilitySync(boardId, columnId, visible, playerId);
      }
    }
  }
}
