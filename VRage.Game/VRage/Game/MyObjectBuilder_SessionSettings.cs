// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_SessionSettings
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game
{
  [ProtoContract(SkipConstructor = true)]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_SessionSettings : MyObjectBuilder_Base
  {
    public static int MaxSafePCU = 600000;
    public static int MaxSafePlayers = 16;
    public static int MaxSafePCU_Remote = MyObjectBuilder_SessionSettings.MaxSafePCU;
    public static int MaxSafePlayers_Remote = MyObjectBuilder_SessionSettings.MaxSafePlayers;
    [XmlIgnore]
    public const uint DEFAULT_AUTOSAVE_IN_MINUTES = 5;
    [ProtoMember(1)]
    [Display(Description = "The type of the game mode.", Name = "Game Mode")]
    [Category("Others")]
    [GameRelation(VRage.Game.Game.Shared)]
    public MyGameModeEnum GameMode;
    [ProtoMember(3)]
    [Display(Description = "The multiplier for inventory size for the characters.", Name = "Characters Inventory Size")]
    [Category("Multipliers")]
    [GameRelation(VRage.Game.Game.Shared)]
    [Range(1, 100)]
    public float InventorySizeMultiplier = 3f;
    [ProtoMember(5)]
    [Display(Description = "The multiplier for inventory size for the blocks.", Name = "Blocks Inventory Size")]
    [Category("Multipliers")]
    [GameRelation(VRage.Game.Game.Shared)]
    [Range(1, 100)]
    public float BlocksInventorySizeMultiplier = 1f;
    [ProtoMember(7)]
    [Display(Description = "The multiplier for assembler speed.", Name = "Assembler Speed")]
    [Category("Multipliers")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Range(1, 100)]
    public float AssemblerSpeedMultiplier = 3f;
    [ProtoMember(9)]
    [Display(Description = "The multiplier for assembler efficiency.", Name = "Assembler Efficiency")]
    [Category("Multipliers")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Range(1, 100)]
    public float AssemblerEfficiencyMultiplier = 3f;
    [ProtoMember(11)]
    [Display(Description = "The multiplier for refinery speed.", Name = "Refinery Speed")]
    [Category("Multipliers")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Range(1, 100)]
    public float RefinerySpeedMultiplier = 3f;
    [ProtoMember(13)]
    public MyOnlineModeEnum OnlineMode;
    [ProtoMember(15)]
    [Display(Description = "The maximum number of connected players.", Name = "Max Players")]
    [GameRelation(VRage.Game.Game.Shared)]
    [Category("Players")]
    [Range(2, 64)]
    public short MaxPlayers = 4;
    [ProtoMember(17)]
    [Display(Description = "The maximum number of existing floating objects.", Name = "Max Floating Objects")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Range(2, 1024)]
    [Category("Environment")]
    public short MaxFloatingObjects = 100;
    [ProtoMember(19)]
    [Display(Description = "The maximum number of backup saves.", Name = "Max Backup Saves")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Range(0, 1000)]
    [Category("Others")]
    public short MaxBackupSaves = 5;
    [ProtoMember(21)]
    [Display(Description = "The maximum number of blocks in one grid.", Name = "Max Grid Blocks")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Range(0, 2147483647)]
    [Category("Block Limits")]
    public int MaxGridSize = 50000;
    [ProtoMember(23)]
    [Display(Description = "The maximum number of blocks per player.", Name = "Max Blocks per Player")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Range(0, 2147483647)]
    [Category("Block Limits")]
    public int MaxBlocksPerPlayer = 100000;
    [ProtoMember(25)]
    [Display(Description = "The total number of Performance Cost Units in the world.", Name = "World PCU")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Range(0, 2147483647)]
    [Category("Block Limits")]
    public int TotalPCU = 600000;
    [ProtoMember(27)]
    [Display(Description = "Number of Performance Cost Units allocated for pirate faction.", Name = "Pirate PCU")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Range(0, 2147483647)]
    [Category("Block Limits")]
    public int PiratePCU = 50000;
    [ProtoMember(29)]
    [Display(Description = "The maximum number of existing factions in the world.", Name = "Max Factions Count")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Range(0, 2147483647)]
    [Category("Block Limits")]
    public int MaxFactionsCount;
    [ProtoMember(31)]
    [Display(Description = "Defines block limits mode.", Name = "Block Limits Mode")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Block Limits")]
    public MyBlockLimitsEnabledEnum BlockLimitsEnabled;
    [ProtoMember(33)]
    [Display(Description = "Enables possibility to remove grid remotely from the world by an author.", Name = "Enable Remote Grid Removal")]
    [Category("Others")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    public bool EnableRemoteBlockRemoval = true;
    [ProtoMember(35)]
    [Display(Description = "Defines hostility of the environment.", Name = "Environment Hostility")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Environment")]
    public MyEnvironmentHostilityEnum EnvironmentHostility = MyEnvironmentHostilityEnum.NORMAL;
    [ProtoMember(37)]
    [Display(Description = "Auto-healing heals players only in oxygen environments and during periods of not taking damage.", Name = "Auto Healing")]
    [Category("Players")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    public bool AutoHealing = true;
    [ProtoMember(39)]
    [Display(Description = "Enables copy and paste feature.", Name = "Enable Copy & Paste")]
    [GameRelation(VRage.Game.Game.Shared)]
    [Category("Players")]
    public bool EnableCopyPaste = true;
    [ProtoMember(41)]
    [Display(Description = "Enables weapons.", Name = "Enable Weapons")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Others")]
    public bool WeaponsEnabled = true;
    [ProtoMember(43)]
    [Display(Description = "", Name = "Show Player Names on HUD")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Players")]
    public bool ShowPlayerNamesOnHud = true;
    [ProtoMember(45)]
    [Display(Description = "Enables thruster damage.", Name = "Enable Thruster Damage")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Others")]
    public bool ThrusterDamage = true;
    [ProtoMember(47)]
    [Display(Description = "Enables spawning of cargo ships.", Name = "Enable Cargo Ships")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("NPCs")]
    public bool CargoShipsEnabled = true;
    [ProtoMember(49)]
    [Display(Description = "Enables spectator camera.", Name = "Enable Spectator Camera")]
    [GameRelation(VRage.Game.Game.Shared)]
    [Category("Others")]
    public bool EnableSpectator;
    [ProtoMember(51)]
    [Display(Description = "Defines the size of the world.", Name = "World Size [km]")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Environment")]
    [Range(0, 2147483647)]
    public int WorldSizeKm;
    [ProtoMember(53)]
    [Display(Description = "When enabled respawn ship is removed after player logout.", Name = "Remove Respawn Ships on Logoff")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Others")]
    public bool RespawnShipDelete;
    [ProtoMember(55)]
    [Display(Description = "", Name = "Reset Ownership")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Players")]
    public bool ResetOwnership;
    [ProtoMember(57)]
    [Display(Description = "The multiplier for welder speed.", Name = "Welder Speed")]
    [Category("Multipliers")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Range(0, 100)]
    public float WelderSpeedMultiplier = 2f;
    [ProtoMember(59)]
    [Display(Description = "The multiplier for grinder speed.", Name = "Grinder Speed")]
    [Category("Multipliers")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Range(0, 100)]
    public float GrinderSpeedMultiplier = 2f;
    [ProtoMember(61)]
    [Display(Description = "Enables realistic sounds.", Name = "Enable Realistic Sound")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Environment")]
    public bool RealisticSound;
    [ProtoMember(63)]
    [Display(Description = "The multiplier for hacking speed.", Name = "Hacking Speed")]
    [Category("Multipliers")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Range(0, 100)]
    public float HackSpeedMultiplier = 0.33f;
    [ProtoMember(65)]
    [Display(Description = "Enables permanent death.", Name = "Permanent Death")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Players")]
    public bool? PermanentDeath = new bool?(false);
    [ProtoMember(67)]
    [Display(Description = "Defines autosave interval.", Name = "Autosave Interval [mins]")]
    [GameRelation(VRage.Game.Game.Shared)]
    [Category("Others")]
    [Range(0.0, 4294967295.0)]
    public uint AutoSaveInMinutes = 5;
    [ProtoMember(69)]
    [Display(Description = "Enables saving from the menu.", Name = "Enable Saving from Menu")]
    [GameRelation(VRage.Game.Game.Shared)]
    [Category("Others")]
    public bool EnableSaving = true;
    [ProtoMember(71)]
    [Display(Description = "Enables infinite ammunition in survival game mode.", Name = "Enable Infinite Ammunition in Survival")]
    [GameRelation(VRage.Game.Game.Shared)]
    [Category("Others")]
    public bool InfiniteAmmo;
    [ProtoMember(73)]
    [Display(Description = "Enables drop containers (unknown signals).", Name = "Enable Drop Containers")]
    [GameRelation(VRage.Game.Game.Shared)]
    [Category("Others")]
    [PlatformPC]
    public bool EnableContainerDrops = true;
    [ProtoMember(75)]
    [Display(Description = "The multiplier for respawn ship timer.", Name = "Respawn Ship Time Multiplier")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Players")]
    [Range(0, 100)]
    public float SpawnShipTimeMultiplier;
    [ProtoMember(77)]
    [Display(Description = "Defines density of the procedurally generated content.", Name = "Procedural Density")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Environment")]
    [Range(0, 1)]
    public float ProceduralDensity;
    [ProtoMember(79)]
    [Display(Description = "Defines unique starting seed for the procedurally generated content.", Name = "Procedural Seed")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Environment")]
    [Range(-2147483648, 2147483647)]
    public int ProceduralSeed;
    [ProtoMember(81)]
    [Display(Description = "Enables destruction feature for the blocks.", Name = "Enable Destructible Blocks")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Environment")]
    public bool DestructibleBlocks = true;
    [ProtoMember(83)]
    [Display(Description = "Enables in game scripts.", Name = "Enable Ingame Scripts")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Others")]
    public bool EnableIngameScripts = true;
    [ProtoMember(85)]
    [Display(Name = "View Distance")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Environment")]
    [Range(5, 50000)]
    [Browsable(false)]
    public int ViewDistance = 15000;
    [ProtoMember(87)]
    [DefaultValue(false)]
    [Display(Description = "Enables tool shake feature.", Name = "Enable Tool Shake")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Players")]
    public bool EnableToolShake;
    [ProtoMember(89)]
    [Display(Description = "", Name = "Voxel Generator Version")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Environment")]
    [Range(0, 100)]
    [PlatformPC]
    public int VoxelGeneratorVersion = 4;
    [ProtoMember(91)]
    [Display(Description = "Enables oxygen in the world.", Name = "Enable Oxygen")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Environment")]
    public bool EnableOxygen;
    [ProtoMember(93)]
    [Display(Description = "Enables airtightness in the world.", Name = "Enable Airtightness")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Environment")]
    public bool EnableOxygenPressurization;
    [ProtoMember(95)]
    [Display(Description = "Enables 3rd person camera.", Name = "Enable 3rd Person Camera")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Players")]
    public bool Enable3rdPersonView = true;
    [ProtoMember(97)]
    [Display(Description = "Enables random encounters in the world.", Name = "Enable Encounters")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("NPCs")]
    public bool EnableEncounters = true;
    [ProtoMember(99)]
    [Display(Description = "Enables possibility of converting grid to station.", Name = "Enable Convert to Station")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Others")]
    public bool EnableConvertToStation = true;
    [ProtoMember(101)]
    [Display(Description = "By enabling this option grids will no longer turn dynamic when disconnected from static grids.", Name = "Unsupported stations")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Environment")]
    public bool StationVoxelSupport;
    [ProtoMember(103)]
    [Display(Description = "Enables sun rotation.", Name = "Enable Sun Rotation")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Environment")]
    public bool EnableSunRotation = true;
    [ProtoMember(105)]
    [Display(Description = "Enables respawn ships.", Name = "Enable Respawn Ships")]
    [GameRelation(VRage.Game.Game.Shared)]
    [Category("Others")]
    public bool EnableRespawnShips = true;
    [ProtoMember(107)]
    [Display(Name = "")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Browsable(false)]
    public bool ScenarioEditMode;
    [ProtoMember(109)]
    [Display(Name = "")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Browsable(false)]
    public bool Scenario;
    [ProtoMember(111)]
    [Display(Name = "")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Browsable(false)]
    public bool CanJoinRunning;
    [ProtoMember(113)]
    [Display(Description = "", Name = "Physics Iterations")]
    [Category("Environment")]
    [Range(2, 32)]
    public int PhysicsIterations = 8;
    [ProtoMember(115)]
    [Display(Description = "Defines interval of one rotation of the sun.", Name = "Sun Rotation Interval")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Environment")]
    [Range(0, 1440)]
    public float SunRotationIntervalMinutes = 120f;
    [ProtoMember(117)]
    [Display(Description = "Enables jetpack.", Name = "Enable Jetpack")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Players")]
    public bool EnableJetpack = true;
    [ProtoMember(119)]
    [Display(Description = "Enables spawning with tools in the inventory.", Name = "Spawn with Tools")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Players")]
    public bool SpawnWithTools = true;
    [ProtoMember(121)]
    [Display(Name = "")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Browsable(false)]
    public bool StartInRespawnScreen;
    [ProtoMember(123)]
    [Display(Description = "Enables voxel destructions.", Name = "Enable Voxel Destruction")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Environment")]
    public bool EnableVoxelDestruction = true;
    [ProtoMember(125)]
    [Display(Name = "")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Browsable(false)]
    [Range(0, 2147483647)]
    public int MaxDrones = 5;
    [ProtoMember(127)]
    [Display(Description = "Enables spawning of drones in the world.", Name = "Enable Drones")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("NPCs")]
    public bool EnableDrones = true;
    [ProtoMember(129)]
    [Display(Description = "Enables spawning of wolves in the world.", Name = "Enable Wolves")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("NPCs")]
    public bool EnableWolfs = true;
    [ProtoMember(131)]
    [Display(Description = "Enables spawning of spiders in the world.", Name = "Enable Spiders")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("NPCs")]
    public bool EnableSpiders;
    [ProtoMember(133)]
    [Display(Description = "", Name = "Flora Density Multiplier")]
    [GameRelation(VRage.Game.Game.Shared)]
    [Category("Environment")]
    [Range(0, 100)]
    [Browsable(false)]
    public float FloraDensityMultiplier = 1f;
    [ProtoMember(135)]
    [Display(Name = "Enable Structural Simulation")]
    [GameRelation(VRage.Game.Game.MedievalEngineers)]
    public bool EnableStructuralSimulation;
    [ProtoMember(137)]
    [Display(Name = "Max Active Fracture Pieces")]
    [GameRelation(VRage.Game.Game.MedievalEngineers)]
    [Range(0, 2147483647)]
    public int MaxActiveFracturePieces = 50;
    [ProtoMember(139)]
    [Display(Name = "Block Type World Limits")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Block Limits")]
    public SerializableDictionary<string, short> BlockTypeLimits = new SerializableDictionary<string, short>(new Dictionary<string, short>()
    {
      {
        "Assembler",
        (short) 24
      },
      {
        "Refinery",
        (short) 24
      },
      {
        "Blast Furnace",
        (short) 24
      },
      {
        "Antenna",
        (short) 30
      },
      {
        "Drill",
        (short) 30
      },
      {
        "InteriorTurret",
        (short) 50
      },
      {
        "GatlingTurret",
        (short) 50
      },
      {
        "MissileTurret",
        (short) 50
      },
      {
        "ExtendedPistonBase",
        (short) 50
      },
      {
        "MotorStator",
        (short) 50
      },
      {
        "MotorAdvancedStator",
        (short) 50
      },
      {
        "ShipWelder",
        (short) 100
      },
      {
        "ShipGrinder",
        (short) 150
      }
    });
    [ProtoMember(141)]
    [Display(Description = "Enables scripter role for administration.", Name = "Enable Scripter Role")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Others")]
    public bool EnableScripterRole;
    [ProtoMember(143, IsRequired = false)]
    [Display(Description = "Defines minimum respawn time for drop containers.", Name = "Min Drop Container Respawn Time")]
    [GameRelation(VRage.Game.Game.Shared)]
    [Category("Others")]
    [Range(0, 100)]
    [PlatformPC]
    public int MinDropContainerRespawnTime = 5;
    [ProtoMember(145, IsRequired = false)]
    [Display(Description = "Defines maximum respawn time for drop containers.", Name = "Max Drop Container Respawn Time")]
    [GameRelation(VRage.Game.Game.Shared)]
    [Category("Others")]
    [Range(0, 100)]
    [PlatformPC]
    public int MaxDropContainerRespawnTime = 20;
    [ProtoMember(147, IsRequired = false)]
    [Display(Description = "Enable explosion damage from missiles being applied to its own grid.", Name = "Enable friendly missile damage")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Environment")]
    public bool EnableTurretsFriendlyFire;
    [ProtoMember(149, IsRequired = false)]
    [Display(Description = "Enables sub-grid damage.", Name = "Enable Sub-Grid Damage")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Environment")]
    public bool EnableSubgridDamage;
    [ProtoMember(151, IsRequired = false)]
    [Display(Description = "Defines synchronization distance in multiplayer. High distance can slow down server drastically. Use with caution.", Name = "Sync Distance")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Environment")]
    [Range(1000, 20000)]
    [PlatformPC]
    public int SyncDistance = 3000;
    [ProtoMember(153, IsRequired = false)]
    [Display(Description = "Enables experimental mode.", Name = "Experimental Mode")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Others")]
    public bool ExperimentalMode;
    [ProtoMember(155, IsRequired = false)]
    [Display(Description = "Enables adaptive simulation quality system. This system is useful if you have a lot of voxel deformations in the world and low simulation speed.", Name = "Adaptive Simulation Quality")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Others")]
    public bool AdaptiveSimulationQuality = true;
    [ProtoMember(157, IsRequired = false)]
    [Display(Description = "Enables voxel hand.", Name = "Enable voxel hand")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Others")]
    public bool EnableVoxelHand;
    [ProtoMember(158, IsRequired = false)]
    [Display(Description = "Defines time in hours after which inactive identities that do not own any grids will be removed. Set 0 to disable.", Name = "Remove Old Identities (h)")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Trash Removal")]
    [Range(0, 2147483647)]
    public int RemoveOldIdentitiesH;
    [ProtoMember(159, IsRequired = false)]
    [Display(Description = "Enables trash removal system.", Name = "Trash Removal Enabled")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Trash Removal")]
    public bool TrashRemovalEnabled = true;
    [ProtoMember(160, IsRequired = false)]
    [Display(Description = "Defines time in minutes after which grids will be stopped if far from player. Set 0 to disable.", Name = "Stop Grids Period (m)")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Trash Removal")]
    [Range(0, 2147483647)]
    public int StopGridsPeriodMin = 15;
    [ProtoMember(161, IsRequired = false)]
    [Display(Description = "Defines flags for trash removal system.", Name = "Trash Removal Flags")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Trash Removal")]
    [MyFlagEnum(typeof (MyTrashRemovalFlags))]
    public int TrashFlagsValue = 7706;
    [ProtoMember(162, IsRequired = false)]
    [Display(Description = "Defines time in minutes after which inactive players will be kicked. 0 is off.", Name = "AFK Timeout")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Trash Removal")]
    [Range(0, 2147483647)]
    public int AFKTimeountMin;
    [ProtoMember(163, IsRequired = false)]
    [Display(Description = "Defines block count threshold for trash removal system.", Name = "Block Count Threshold")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Trash Removal")]
    [Range(0, 2147483647)]
    public int BlockCountThreshold = 20;
    [ProtoMember(165, IsRequired = false)]
    [Display(Description = "Defines player distance threshold for trash removal system.", Name = "Player Distance Threshold [m]")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Trash Removal")]
    public float PlayerDistanceThreshold = 500f;
    [ProtoMember(167, IsRequired = false)]
    [Display(Description = "By setting this, server will keep number of grids around this value. \n !WARNING! It ignores Powered and Fixed flags, Block Count and lowers Distance from player.\n Set to 0 to disable.", Name = "Optimal Grid Count")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Trash Removal")]
    [Range(0, 2147483647)]
    public int OptimalGridCount;
    [ProtoMember(169, IsRequired = false)]
    [Display(Description = "Defines player inactivity (time from logout) threshold for trash removal system. \n !WARNING! This will remove all grids of the player.\n Set to 0 to disable.", Name = "Player Inactivity Threshold [hours]")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Trash Removal")]
    public float PlayerInactivityThreshold;
    [ProtoMember(171, IsRequired = false)]
    [Display(Description = "Defines character removal threshold for trash removal system. If player disconnects it will remove his character after this time.\n Set to 0 to disable.", Name = "Character Removal Threshold [mins]")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Trash Removal")]
    [Range(0, 2147483647)]
    public int PlayerCharacterRemovalThreshold = 15;
    [ProtoMember(173, IsRequired = false)]
    [Display(Description = "Enables system for voxel reverting.", Name = "Voxel Reverting Enabled")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Trash Removal")]
    public bool VoxelTrashRemovalEnabled;
    [ProtoMember(175, IsRequired = false)]
    [Display(Description = "Only voxel chunks that are further from player will be reverted.", Name = "Distance voxel from player (m)")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Trash Removal")]
    public float VoxelPlayerDistanceThreshold = 5000f;
    [ProtoMember(177, IsRequired = false)]
    [Display(Description = "Only voxel chunks that are further from any grid will be reverted.", Name = "Distance voxel from grid (m)")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Trash Removal")]
    public float VoxelGridDistanceThreshold = 5000f;
    [ProtoMember(179, IsRequired = false)]
    [Display(Description = "Only voxel chunks that have been modified longer time age may be reverted.", Name = "Voxel age (min)")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Trash Removal")]
    [Range(0, 2147483647)]
    public int VoxelAgeThreshold = 24;
    [ProtoMember(181, IsRequired = false)]
    [Display(Description = "Enables research progression.", Name = "Enable Progression")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Others")]
    public bool EnableResearch;
    [ProtoMember(183, IsRequired = false)]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Display(Description = "Enables Good.bot hints in the world. If user has disabled hints, this will not override that.", Name = "Enable Good.bot Hints")]
    [Category("Others")]
    public bool EnableGoodBotHints = true;
    [ProtoMember(185, IsRequired = false)]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Display(Description = "Sets optimal distance in meters the game should take into consideration when spawning new player near others.", Name = "Optimal respawn distance")]
    [Category("Players")]
    public float OptimalSpawnDistance = 16000f;
    [ProtoMember(187, IsRequired = false)]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Display(Description = "Enables automatic respawn at nearest available respawn point", Name = "Enable Autorespawn")]
    [Category("Players")]
    public bool EnableAutorespawn = true;
    [ProtoMember(188)]
    [Display(Description = "If enabled bounty contracts will be available on stations.", Name = "Enable Bounty Contracts")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Players")]
    public bool EnableBountyContracts = true;
    [ProtoMember(189, IsRequired = false)]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Display(Description = "Allows super gridding exploit to be used", Name = "Enable Supergridding")]
    [Category("Others")]
    public bool EnableSupergridding;
    [ProtoMember(191)]
    [Display(Description = "Enables economy features.", Name = "Enable Economy")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("NPCs")]
    public bool EnableEconomy;
    [ProtoMember(194)]
    [Display(Description = "Resource deposits count coefficient for generated world content (voxel generator version > 2).", Name = "Deposits Count Coefficient")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Environment")]
    [Range(0, 10)]
    public float DepositsCountCoefficient = 2f;
    [ProtoMember(197)]
    [Display(Description = "Resource deposit size denominator for generated world content (voxel generator version > 2).", Name = "Deposit Size Denominator")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Environment")]
    [Range(1.0, 100.0)]
    public float DepositSizeDenominator = 30f;
    [ProtoMember(198)]
    [Display(Description = "Enable automatic weather generation on planets.", Name = "Enable weather system")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Environment")]
    public bool WeatherSystem = true;
    [ProtoMember(200)]
    [Display(Description = "Harvest ratio multiplier for drills.", Name = "Harvest Ratio Multiplier")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Multipliers")]
    [Range(0.0, 100.0)]
    public float HarvestRatioMultiplier = 1f;
    [ProtoMember(203)]
    [Display(Description = "The number of NPC factions generated on the start of the world.", Name = "NPC Factions Count")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("NPCs")]
    [Range(10.0, 100.0)]
    public int TradeFactionsCount = 15;
    [ProtoMember(206)]
    [Display(Description = "The inner radius [m] (center is in 0,0,0), where stations can spawn. Does not affect planet-bound stations (surface Outposts and Orbital stations).", Name = "Stations Inner Radius")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("NPCs")]
    [Range(200000.0, 9.22337203685478E+18)]
    public double StationsDistanceInnerRadius = 10000000.0;
    [ProtoMember(209)]
    [Display(Description = "The outer radius [m] (center is in 0,0,0), where stations can spawn. Does not affect planet-bound stations (surface Outposts and Orbital stations).", Name = "Stations Outer Radius Start")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("NPCs")]
    [Range(500000.0, 9.22337203685478E+18)]
    public double StationsDistanceOuterRadiusStart = 10000000.0;
    [ProtoMember(212)]
    [Display(Description = "The outer radius [m] (center is in 0,0,0), where stations can spawn. Does not affect planet-bound stations (surface Outposts and Orbital stations).", Name = "Stations Outer Radius End")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("NPCs")]
    [Range(1000000.0, 9.22337203685478E+18)]
    public double StationsDistanceOuterRadiusEnd = 30000000.0;
    [ProtoMember(215)]
    [Display(Description = "Time period between two economy updates.", Name = "Economy tick time")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("NPCs")]
    [Range(300, 3600)]
    public int EconomyTickInSeconds = 1200;
    [ProtoMember(217)]
    [Browsable(false)]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    public bool SimplifiedSimulation;
    [ProtoMember(219)]
    [Browsable(false)]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [XmlArrayItem("Warning")]
    public List<string> SuppressedWarnings = new List<string>();
    [ProtoMember(222)]
    [Display(Description = "Enable trading of PCUs between players or factions depending on PCU settings.", Name = "Enable PCU trading")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Players")]
    public bool EnablePcuTrading = true;
    [ProtoMember(220)]
    [Display(Description = "Enables shared accounts to join multiplayer games.", Name = "Enable family sharing")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Players")]
    public bool FamilySharing = true;
    [ProtoMember(224)]
    [Display(Description = "When enabled game will update physics only in the specific clusters, which are necessary. Dedicated server options only.", Name = "Enable Selective Physics Updates")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Others")]
    public bool EnableSelectivePhysicsUpdates;
    [ProtoMember(228)]
    [Display(Description = "To conserve memory, predefined asteroids has to be disabled on consoles.", Name = "Enable predefined asteroids")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Others")]
    [PlatformPC]
    public bool PredefinedAsteroids = true;
    [ProtoMember(232)]
    [Display(Description = "To conserve memory, some of the blocks have different PCU values for consoles.", Name = "Use Console PCU")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Others")]
    [PlatformPC]
    public bool UseConsolePCU;
    [ProtoMember(236)]
    [Display(Description = "Limit maximum number of types of planets in the world.", Name = "Max Planet Types")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Others")]
    [PlatformPC]
    [Range(0, 99)]
    public int MaxPlanets = 99;
    [ProtoMember(240)]
    [Display(Description = "Filter offensive words from all input methods.", Name = "Offensive Words Filtering")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Others")]
    [PlatformPC]
    public bool OffensiveWordsFiltering;
    [ProtoMember(245)]
    [Display(Name = "Adjustable Max Vehicle Speed")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Environment")]
    [Browsable(false)]
    public bool AdjustableMaxVehicleSpeed = true;
    [ProtoMember(246)]
    [Display(Description = "Enable component handling the match", Name = "Enable match")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Others")]
    public bool EnableMatchComponent;
    [ProtoMember(247)]
    [Display(Description = "Duration of PreMatch phase of the match", Name = "PreMatch duration")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Range(0.0, 60000.0)]
    [Category("Others")]
    public float PreMatchDuration;
    [ProtoMember(248)]
    [Display(Description = "Duration of Match phase of the match", Name = "Match duration")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Range(0.0, 60000.0)]
    [Category("Others")]
    public float MatchDuration;
    [ProtoMember(249)]
    [Display(Description = "Duration of PostMatch phase of the match", Name = "PostMatch duration")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Range(0.0, 60000.0)]
    [Category("Others")]
    public float PostMatchDuration;
    [ProtoMember(250)]
    [Display(Name = "Enable Friendly Fire")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("PvP")]
    public bool EnableFriendlyFire = true;
    [ProtoMember(251)]
    [Display(Name = "Enable team balancing")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("PvP")]
    public bool EnableTeamBalancing;
    [ProtoMember(252)]
    [Display(Name = "Character Speed Multiplier")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Players")]
    [Range(0.75, 1.0)]
    public float CharacterSpeedMultiplier = 1f;
    [ProtoMember(253)]
    [Display(Name = "Enable weapon recoil.")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Players")]
    public bool EnableRecoil = true;
    [ProtoMember(254)]
    [Display(Description = "This multiplier only applies for damage caused to the player by environment.", Name = "Environment Damage Multiplier")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Players")]
    [Range(0, 2)]
    public float EnvironmentDamageMultiplier = 1f;
    [ProtoMember(255)]
    [Display(Description = "Enable aim assist for gamepad.", Name = "Enable Gamepad Aim Assist")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Players")]
    public bool EnableGamepadAimAssist;
    [ProtoMember(256)]
    [Display(Description = "Sets the timer (minutes) for the backpack to be removed from the world. Default is 5 minutes.", Name = "Backpack Despawn Time")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Players")]
    [Range(0, 10)]
    public float BackpackDespawnTimer = 5f;
    [ProtoMember(257)]
    [Display(Description = "Shows player names above the head if they are in the same faction and personal broadcast is off.", Name = "Show Faction Player Names")]
    [GameRelation(VRage.Game.Game.SpaceEngineers)]
    [Category("Players")]
    [Browsable(false)]
    public bool EnableFactionPlayerNames;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool AutoSave
    {
      get => this.AutoSaveInMinutes > 0U;
      set => this.AutoSaveInMinutes = value ? 5U : 0U;
    }

    public bool ShouldSerializeAutoSave() => false;

    [Display(Name = "Client can save")]
    [GameRelation(VRage.Game.Game.Shared)]
    [XmlIgnore]
    [NoSerialize]
    [Browsable(false)]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool ClientCanSave
    {
      get => false;
      set
      {
      }
    }

    public bool ShouldSerializeProceduralDensity() => (double) this.ProceduralDensity > 0.0;

    public bool ShouldSerializeProceduralSeed() => (double) this.ProceduralDensity > 0.0;

    [XmlIgnore]
    [ProtoIgnore]
    [Browsable(false)]
    public MyTrashRemovalFlags TrashFlags
    {
      get => (MyTrashRemovalFlags) this.TrashFlagsValue;
      set => this.TrashFlagsValue = (int) value;
    }

    public bool ShouldSerializeTrashFlags() => false;

    public bool IsSettingsExperimental(bool remote) => (ulong) this.GetExperimentalReason(remote) > 0UL;

    public MyObjectBuilder_SessionSettings.ExperimentalReason GetExperimentalReason(
      bool remote)
    {
      MyObjectBuilder_SessionSettings.ExperimentalReason experimentalReason = (MyObjectBuilder_SessionSettings.ExperimentalReason) 0;
      if (this.ExperimentalMode)
        experimentalReason |= MyObjectBuilder_SessionSettings.ExperimentalReason.ExperimentalMode;
      if (this.MaxPlayers == (short) 0 || (int) this.MaxPlayers > (remote ? MyObjectBuilder_SessionSettings.MaxSafePlayers_Remote : MyObjectBuilder_SessionSettings.MaxSafePlayers))
        experimentalReason |= MyObjectBuilder_SessionSettings.ExperimentalReason.MaxPlayers;
      if ((double) this.ProceduralDensity > 0.349999994039536)
        experimentalReason |= MyObjectBuilder_SessionSettings.ExperimentalReason.ProceduralDensity;
      if ((double) this.SunRotationIntervalMinutes <= 29.0)
        experimentalReason |= MyObjectBuilder_SessionSettings.ExperimentalReason.SunRotationIntervalMinutes;
      if (this.MaxFloatingObjects > (short) 100)
        experimentalReason |= MyObjectBuilder_SessionSettings.ExperimentalReason.MaxFloatingObjects;
      if (this.PhysicsIterations != 8)
        experimentalReason |= MyObjectBuilder_SessionSettings.ExperimentalReason.PhysicsIterations;
      if (this.SyncDistance != 3000)
        experimentalReason |= MyObjectBuilder_SessionSettings.ExperimentalReason.SyncDistance;
      if (this.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.NONE)
        experimentalReason |= MyObjectBuilder_SessionSettings.ExperimentalReason.BlockLimitsEnabled;
      if (this.TotalPCU > (remote ? MyObjectBuilder_SessionSettings.MaxSafePCU_Remote : MyObjectBuilder_SessionSettings.MaxSafePCU))
        experimentalReason |= MyObjectBuilder_SessionSettings.ExperimentalReason.TotalPCU;
      if (this.EnableSpectator)
        experimentalReason |= MyObjectBuilder_SessionSettings.ExperimentalReason.EnableSpectator;
      if (this.ResetOwnership)
        experimentalReason |= MyObjectBuilder_SessionSettings.ExperimentalReason.ResetOwnership;
      bool? permanentDeath = this.PermanentDeath;
      bool flag = true;
      if (permanentDeath.GetValueOrDefault() == flag & permanentDeath.HasValue)
        experimentalReason |= MyObjectBuilder_SessionSettings.ExperimentalReason.PermanentDeath;
      if (this.EnableIngameScripts)
        experimentalReason |= MyObjectBuilder_SessionSettings.ExperimentalReason.EnableIngameScripts;
      if (this.StationVoxelSupport)
        experimentalReason |= MyObjectBuilder_SessionSettings.ExperimentalReason.StationVoxelSupport;
      if (this.EnableSubgridDamage)
        experimentalReason |= MyObjectBuilder_SessionSettings.ExperimentalReason.EnableSubgridDamage;
      if (!this.AdaptiveSimulationQuality)
        experimentalReason |= MyObjectBuilder_SessionSettings.ExperimentalReason.AdaptiveSimulationQuality;
      if (this.EnableSupergridding)
        experimentalReason |= MyObjectBuilder_SessionSettings.ExperimentalReason.SupergriddingEnabled;
      return experimentalReason;
    }

    public void LogMembers(MyLog log, LoggingOptions options)
    {
      log.WriteLine("Settings:");
      using (log.IndentUsing(options))
      {
        log.WriteLine("GameMode = " + (object) this.GameMode);
        log.WriteLine("MaxPlayers = " + (object) this.MaxPlayers);
        log.WriteLine("OnlineMode = " + (object) this.OnlineMode);
        log.WriteLine("TotalPCU = " + (object) this.TotalPCU);
        log.WriteLine("PiratePCU = " + (object) this.PiratePCU);
        log.WriteLine("AutoHealing = " + this.AutoHealing.ToString());
        log.WriteLine("WeaponsEnabled = " + this.WeaponsEnabled.ToString());
        log.WriteLine("ThrusterDamage = " + this.ThrusterDamage.ToString());
        log.WriteLine("EnableSpectator = " + this.EnableSpectator.ToString());
        log.WriteLine("EnableCopyPaste = " + this.EnableCopyPaste.ToString());
        log.WriteLine("MaxFloatingObjects = " + (object) this.MaxFloatingObjects);
        log.WriteLine("MaxGridSize = " + (object) this.MaxGridSize);
        log.WriteLine("MaxBlocksPerPlayer = " + (object) this.MaxBlocksPerPlayer);
        log.WriteLine("CargoShipsEnabled = " + this.CargoShipsEnabled.ToString());
        log.WriteLine("EnvironmentHostility = " + (object) this.EnvironmentHostility);
        log.WriteLine("ShowPlayerNamesOnHud = " + this.ShowPlayerNamesOnHud.ToString());
        log.WriteLine("InventorySizeMultiplier = " + (object) this.InventorySizeMultiplier);
        log.WriteLine("BlocksInventorySizeMultiplier = " + (object) this.BlocksInventorySizeMultiplier);
        log.WriteLine("RefinerySpeedMultiplier = " + (object) this.RefinerySpeedMultiplier);
        log.WriteLine("AssemblerSpeedMultiplier = " + (object) this.AssemblerSpeedMultiplier);
        log.WriteLine("AssemblerEfficiencyMultiplier = " + (object) this.AssemblerEfficiencyMultiplier);
        log.WriteLine("WelderSpeedMultiplier = " + (object) this.WelderSpeedMultiplier);
        log.WriteLine("GrinderSpeedMultiplier = " + (object) this.GrinderSpeedMultiplier);
        log.WriteLine("ClientCanSave = " + this.ClientCanSave.ToString());
        log.WriteLine("HackSpeedMultiplier = " + (object) this.HackSpeedMultiplier);
        log.WriteLine("PermanentDeath = " + (object) this.PermanentDeath);
        log.WriteLine("DestructibleBlocks =  " + this.DestructibleBlocks.ToString());
        log.WriteLine("EnableScripts =  " + this.EnableIngameScripts.ToString());
        log.WriteLine("AutoSaveInMinutes = " + (object) this.AutoSaveInMinutes);
        log.WriteLine("SpawnShipTimeMultiplier = " + (object) this.SpawnShipTimeMultiplier);
        log.WriteLine("ProceduralDensity = " + (object) this.ProceduralDensity);
        log.WriteLine("ProceduralSeed = " + (object) this.ProceduralSeed);
        log.WriteLine("DestructibleBlocks = " + this.DestructibleBlocks.ToString());
        log.WriteLine("EnableIngameScripts = " + this.EnableIngameScripts.ToString());
        log.WriteLine("ViewDistance = " + (object) this.ViewDistance);
        log.WriteLine("Voxel destruction = " + this.EnableVoxelDestruction.ToString());
        log.WriteLine("EnableStructuralSimulation = " + this.EnableStructuralSimulation.ToString());
        log.WriteLine("MaxActiveFracturePieces = " + (object) this.MaxActiveFracturePieces);
        log.WriteLine("EnableContainerDrops = " + this.EnableContainerDrops.ToString());
        log.WriteLine("MinDropContainerRespawnTime = " + (object) this.MinDropContainerRespawnTime);
        log.WriteLine("MaxDropContainerRespawnTime = " + (object) this.MaxDropContainerRespawnTime);
        log.WriteLine("EnableTurretsFriendlyFire = " + this.EnableTurretsFriendlyFire.ToString());
        log.WriteLine("EnableSubgridDamage = " + this.EnableSubgridDamage.ToString());
        log.WriteLine("SyncDistance = " + (object) this.SyncDistance);
        log.WriteLine("BlockLimitsEnabled = " + (object) this.BlockLimitsEnabled);
        log.WriteLine("AFKTimeoutMin = " + (object) this.AFKTimeountMin);
        log.WriteLine("StopGridsPeriodMin = " + (object) this.StopGridsPeriodMin);
        log.WriteLine("MaxPlanets = " + (object) this.MaxPlanets);
        log.WriteLine("MaxBackupSaves = " + (object) this.MaxBackupSaves);
        log.WriteLine("MaxFactionsCount = " + (object) this.MaxFactionsCount);
        log.WriteLine("EnableRemoteBlockRemoval = " + this.EnableRemoteBlockRemoval.ToString());
        log.WriteLine("RespawnShipDelete = " + this.RespawnShipDelete.ToString());
        log.WriteLine("WorldSizeKm = " + (object) this.WorldSizeKm);
        log.WriteLine("ResetOwnership = " + this.ResetOwnership.ToString());
        log.WriteLine("RealisticSound = " + this.RealisticSound.ToString());
        log.WriteLine("EnableSaving = " + this.EnableSaving.ToString());
        log.WriteLine("InfiniteAmmo = " + this.InfiniteAmmo.ToString());
        log.WriteLine("EnableToolShake = " + this.EnableToolShake.ToString());
        log.WriteLine("VoxelGeneratorVersion = " + (object) this.VoxelGeneratorVersion);
        log.WriteLine("OffensiveWordsFiltering = " + this.OffensiveWordsFiltering.ToString());
        log.WriteLine("UseConsolePCU = " + this.UseConsolePCU.ToString());
        log.WriteLine("PredefinedAsteroids = " + this.PredefinedAsteroids.ToString());
        log.WriteLine("EnableSelectivePhysicsUpdates = " + this.EnableSelectivePhysicsUpdates.ToString());
        log.WriteLine("FamilySharing = " + this.FamilySharing.ToString());
        log.WriteLine("EnablePcuTrading = " + this.EnablePcuTrading.ToString());
        log.WriteLine("SuppressedWarnings = " + (this.SuppressedWarnings != null ? string.Join(",", (IEnumerable<string>) this.SuppressedWarnings) : ""));
        log.WriteLine("SimplifiedSimulation = " + this.SimplifiedSimulation.ToString());
        log.WriteLine("EconomyTickInSeconds = " + (object) this.EconomyTickInSeconds);
        log.WriteLine("StationsDistanceOuterRadiusEnd = " + (object) this.StationsDistanceOuterRadiusEnd);
        log.WriteLine("StationsDistanceOuterRadiusStart = " + (object) this.StationsDistanceOuterRadiusStart);
        log.WriteLine("StationsDistanceInnerRadius = " + (object) this.StationsDistanceInnerRadius);
        log.WriteLine("TradeFactionsCount = " + (object) this.TradeFactionsCount);
        log.WriteLine("HarvestRatioMultiplier = " + (object) this.HarvestRatioMultiplier);
        log.WriteLine("WeatherSystem = " + this.WeatherSystem.ToString());
        log.WriteLine("DepositSizeDenominator = " + (object) this.DepositSizeDenominator);
        log.WriteLine("DepositsCountCoefficient = " + (object) this.DepositsCountCoefficient);
        log.WriteLine("EnableEconomy = " + this.EnableEconomy.ToString());
        log.WriteLine("EnableBountyContracts = " + this.EnableBountyContracts.ToString());
        log.WriteLine("EnableSupergridding = " + this.EnableSupergridding.ToString());
        log.WriteLine("EnableAutorespawn = " + this.EnableAutorespawn.ToString());
        log.WriteLine("OptimalSpawnDistance = " + (object) this.OptimalSpawnDistance);
        log.WriteLine("EnableGoodBotHints = " + this.EnableGoodBotHints.ToString());
        log.WriteLine("EnableResearch = " + this.EnableResearch.ToString());
        log.WriteLine("VoxelAgeThreshold = " + (object) this.VoxelAgeThreshold);
        log.WriteLine("VoxelGridDistanceThreshold = " + (object) this.VoxelGridDistanceThreshold);
        log.WriteLine("VoxelPlayerDistanceThreshold = " + (object) this.VoxelPlayerDistanceThreshold);
        log.WriteLine("VoxelTrashRemovalEnabled = " + this.VoxelTrashRemovalEnabled.ToString());
        log.WriteLine("PlayerCharacterRemovalThreshold = " + (object) this.PlayerCharacterRemovalThreshold);
        log.WriteLine("PlayerInactivityThreshold = " + (object) this.PlayerInactivityThreshold);
        log.WriteLine("OptimalGridCount = " + (object) this.OptimalGridCount);
        log.WriteLine("PlayerDistanceThreshold = " + (object) this.PlayerDistanceThreshold);
        log.WriteLine("BlockCountThreshold = " + (object) this.BlockCountThreshold);
        log.WriteLine("TrashFlagsValue = " + (object) this.TrashFlagsValue);
        log.WriteLine("TrashRemovalEnabled = " + this.TrashRemovalEnabled.ToString());
        log.WriteLine("RemoveOldIdentitiesH = " + (object) this.RemoveOldIdentitiesH);
        log.WriteLine("EnableVoxelHand = " + this.EnableVoxelHand.ToString());
        log.WriteLine("AdaptiveSimulationQuality = " + this.AdaptiveSimulationQuality.ToString());
        log.WriteLine("EnableScripterRole = " + this.EnableScripterRole.ToString());
        log.WriteLine("BlockTypeLimits = " + this.BlockTypeLimits.ToString());
        log.WriteLine("FloraDensityMultiplier = " + (object) this.FloraDensityMultiplier);
        log.WriteLine("EnableSpiders = " + this.EnableSpiders.ToString());
        log.WriteLine("EnableWolfs = " + this.EnableWolfs.ToString());
        log.WriteLine("EnableDrones = " + this.EnableDrones.ToString());
        log.WriteLine("MaxDrones = " + (object) this.MaxDrones);
        log.WriteLine("StartInRespawnScreen = " + this.StartInRespawnScreen.ToString());
        log.WriteLine("SpawnWithTools = " + this.SpawnWithTools.ToString());
        log.WriteLine("EnableJetpack = " + this.EnableJetpack.ToString());
        log.WriteLine("SunRotationIntervalMinutes = " + (object) this.SunRotationIntervalMinutes);
        log.WriteLine("PhysicsIterations = " + (object) this.PhysicsIterations);
        log.WriteLine("CanJoinRunning = " + this.CanJoinRunning.ToString());
        log.WriteLine("Scenario = " + this.Scenario.ToString());
        log.WriteLine("ScenarioEditMode = " + this.ScenarioEditMode.ToString());
        log.WriteLine("EnableRespawnShips = " + this.EnableRespawnShips.ToString());
        log.WriteLine("EnableSunRotation = " + this.EnableSunRotation.ToString());
        log.WriteLine("StationVoxelSupport = " + this.StationVoxelSupport.ToString());
        log.WriteLine("EnableConvertToStation = " + this.EnableConvertToStation.ToString());
        log.WriteLine("EnableEncounters = " + this.EnableEncounters.ToString());
        log.WriteLine("Enable3rdPersonView = " + this.Enable3rdPersonView.ToString());
        log.WriteLine("EnableOxygenPressurization = " + this.EnableOxygenPressurization.ToString());
        log.WriteLine("EnableOxygen = " + this.EnableOxygen.ToString());
        log.WriteLine("ExperimentalMode = " + this.ExperimentalMode.ToString());
        log.WriteLine("ExperimentalModeReason = " + (object) this.GetExperimentalReason(false));
        log.WriteLine("MovementSpeedMultiplier = " + (object) this.CharacterSpeedMultiplier);
        log.WriteLine("EnableRecoil = " + this.EnableRecoil.ToString());
        log.WriteLine("EnvironmentDamageMultiplier = " + (object) this.EnvironmentDamageMultiplier);
      }
    }

    public static int GetInitialPCU(MyObjectBuilder_SessionSettings settings)
    {
      switch (settings.BlockLimitsEnabled)
      {
        case MyBlockLimitsEnabledEnum.NONE:
          return int.MaxValue;
        case MyBlockLimitsEnabledEnum.PER_FACTION:
          return settings.MaxFactionsCount == 0 ? settings.TotalPCU : settings.TotalPCU / settings.MaxFactionsCount;
        case MyBlockLimitsEnabledEnum.PER_PLAYER:
          return settings.TotalPCU / (int) settings.MaxPlayers;
        default:
          return settings.TotalPCU;
      }
    }

    [Flags]
    public enum ExperimentalReason : long
    {
      ExperimentalMode = 2,
      MaxPlayers = 4,
      ProceduralDensity = 16, // 0x0000000000000010
      SunRotationIntervalMinutes = 256, // 0x0000000000000100
      MaxFloatingObjects = 512, // 0x0000000000000200
      PhysicsIterations = 1024, // 0x0000000000000400
      SyncDistance = 2048, // 0x0000000000000800
      BlockLimitsEnabled = 8192, // 0x0000000000002000
      TotalPCU = 16384, // 0x0000000000004000
      EnableSpectator = 131072, // 0x0000000000020000
      ResetOwnership = 1048576, // 0x0000000000100000
      PermanentDeath = 4194304, // 0x0000000000400000
      EnableIngameScripts = 67108864, // 0x0000000004000000
      StationVoxelSupport = 17179869184, // 0x0000000400000000
      EnableSubgridDamage = 8796093022208, // 0x0000080000000000
      AdaptiveSimulationQuality = 140737488355328, // 0x0000800000000000
      ExperimentalTurnedOnInConfiguration = 281474976710656, // 0x0001000000000000
      InsufficientHardware = 562949953421312, // 0x0002000000000000
      Mods = 1125899906842624, // 0x0004000000000000
      Plugins = 2251799813685248, // 0x0008000000000000
      SupergriddingEnabled = 4503599627370496, // 0x0010000000000000
      ReasonMax = -2147483648, // 0xFFFFFFFF80000000
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EGameMode\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, MyGameModeEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in MyGameModeEnum value) => owner.GameMode = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out MyGameModeEnum value) => value = owner.GameMode;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EInventorySizeMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in float value) => owner.InventorySizeMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out float value) => value = owner.InventorySizeMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EBlocksInventorySizeMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in float value) => owner.BlocksInventorySizeMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out float value) => value = owner.BlocksInventorySizeMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EAssemblerSpeedMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in float value) => owner.AssemblerSpeedMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out float value) => value = owner.AssemblerSpeedMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EAssemblerEfficiencyMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in float value) => owner.AssemblerEfficiencyMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out float value) => value = owner.AssemblerEfficiencyMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003ERefinerySpeedMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in float value) => owner.RefinerySpeedMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out float value) => value = owner.RefinerySpeedMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EOnlineMode\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, MyOnlineModeEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in MyOnlineModeEnum value) => owner.OnlineMode = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out MyOnlineModeEnum value) => value = owner.OnlineMode;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EMaxPlayers\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, short>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in short value) => owner.MaxPlayers = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out short value) => value = owner.MaxPlayers;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EMaxFloatingObjects\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, short>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in short value) => owner.MaxFloatingObjects = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out short value) => value = owner.MaxFloatingObjects;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EMaxBackupSaves\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, short>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in short value) => owner.MaxBackupSaves = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out short value) => value = owner.MaxBackupSaves;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EMaxGridSize\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in int value) => owner.MaxGridSize = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out int value) => value = owner.MaxGridSize;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EMaxBlocksPerPlayer\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in int value) => owner.MaxBlocksPerPlayer = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out int value) => value = owner.MaxBlocksPerPlayer;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003ETotalPCU\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in int value) => owner.TotalPCU = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out int value) => value = owner.TotalPCU;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EPiratePCU\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in int value) => owner.PiratePCU = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out int value) => value = owner.PiratePCU;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EMaxFactionsCount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in int value) => owner.MaxFactionsCount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out int value) => value = owner.MaxFactionsCount;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EBlockLimitsEnabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, MyBlockLimitsEnabledEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SessionSettings owner,
        in MyBlockLimitsEnabledEnum value)
      {
        owner.BlockLimitsEnabled = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SessionSettings owner,
        out MyBlockLimitsEnabledEnum value)
      {
        value = owner.BlockLimitsEnabled;
      }
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableRemoteBlockRemoval\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableRemoteBlockRemoval = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableRemoteBlockRemoval;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnvironmentHostility\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, MyEnvironmentHostilityEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SessionSettings owner,
        in MyEnvironmentHostilityEnum value)
      {
        owner.EnvironmentHostility = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SessionSettings owner,
        out MyEnvironmentHostilityEnum value)
      {
        value = owner.EnvironmentHostility;
      }
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EAutoHealing\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.AutoHealing = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.AutoHealing;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableCopyPaste\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableCopyPaste = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableCopyPaste;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EWeaponsEnabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.WeaponsEnabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.WeaponsEnabled;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EShowPlayerNamesOnHud\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.ShowPlayerNamesOnHud = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.ShowPlayerNamesOnHud;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EThrusterDamage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.ThrusterDamage = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.ThrusterDamage;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003ECargoShipsEnabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.CargoShipsEnabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.CargoShipsEnabled;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableSpectator\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableSpectator = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableSpectator;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EWorldSizeKm\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in int value) => owner.WorldSizeKm = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out int value) => value = owner.WorldSizeKm;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003ERespawnShipDelete\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.RespawnShipDelete = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.RespawnShipDelete;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EResetOwnership\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.ResetOwnership = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.ResetOwnership;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EWelderSpeedMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in float value) => owner.WelderSpeedMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out float value) => value = owner.WelderSpeedMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EGrinderSpeedMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in float value) => owner.GrinderSpeedMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out float value) => value = owner.GrinderSpeedMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003ERealisticSound\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.RealisticSound = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.RealisticSound;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EHackSpeedMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in float value) => owner.HackSpeedMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out float value) => value = owner.HackSpeedMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EPermanentDeath\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool? value) => owner.PermanentDeath = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool? value) => value = owner.PermanentDeath;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EAutoSaveInMinutes\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, uint>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in uint value) => owner.AutoSaveInMinutes = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out uint value) => value = owner.AutoSaveInMinutes;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableSaving\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableSaving = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableSaving;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EInfiniteAmmo\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.InfiniteAmmo = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.InfiniteAmmo;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableContainerDrops\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableContainerDrops = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableContainerDrops;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003ESpawnShipTimeMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in float value) => owner.SpawnShipTimeMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out float value) => value = owner.SpawnShipTimeMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EProceduralDensity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in float value) => owner.ProceduralDensity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out float value) => value = owner.ProceduralDensity;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EProceduralSeed\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in int value) => owner.ProceduralSeed = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out int value) => value = owner.ProceduralSeed;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EDestructibleBlocks\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.DestructibleBlocks = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.DestructibleBlocks;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableIngameScripts\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableIngameScripts = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableIngameScripts;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EViewDistance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in int value) => owner.ViewDistance = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out int value) => value = owner.ViewDistance;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableToolShake\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableToolShake = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableToolShake;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EVoxelGeneratorVersion\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in int value) => owner.VoxelGeneratorVersion = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out int value) => value = owner.VoxelGeneratorVersion;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableOxygen\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableOxygen = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableOxygen;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableOxygenPressurization\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableOxygenPressurization = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableOxygenPressurization;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnable3rdPersonView\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.Enable3rdPersonView = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.Enable3rdPersonView;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableEncounters\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableEncounters = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableEncounters;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableConvertToStation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableConvertToStation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableConvertToStation;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EStationVoxelSupport\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.StationVoxelSupport = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.StationVoxelSupport;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableSunRotation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableSunRotation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableSunRotation;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableRespawnShips\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableRespawnShips = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableRespawnShips;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EScenarioEditMode\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.ScenarioEditMode = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.ScenarioEditMode;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EScenario\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.Scenario = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.Scenario;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003ECanJoinRunning\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.CanJoinRunning = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.CanJoinRunning;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EPhysicsIterations\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in int value) => owner.PhysicsIterations = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out int value) => value = owner.PhysicsIterations;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003ESunRotationIntervalMinutes\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in float value) => owner.SunRotationIntervalMinutes = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out float value) => value = owner.SunRotationIntervalMinutes;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableJetpack\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableJetpack = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableJetpack;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003ESpawnWithTools\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.SpawnWithTools = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.SpawnWithTools;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EStartInRespawnScreen\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.StartInRespawnScreen = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.StartInRespawnScreen;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableVoxelDestruction\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableVoxelDestruction = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableVoxelDestruction;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EMaxDrones\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in int value) => owner.MaxDrones = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out int value) => value = owner.MaxDrones;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableDrones\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableDrones = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableDrones;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableWolfs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableWolfs = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableWolfs;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableSpiders\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableSpiders = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableSpiders;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EFloraDensityMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in float value) => owner.FloraDensityMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out float value) => value = owner.FloraDensityMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableStructuralSimulation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableStructuralSimulation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableStructuralSimulation;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EMaxActiveFracturePieces\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in int value) => owner.MaxActiveFracturePieces = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out int value) => value = owner.MaxActiveFracturePieces;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EBlockTypeLimits\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, SerializableDictionary<string, short>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SessionSettings owner,
        in SerializableDictionary<string, short> value)
      {
        owner.BlockTypeLimits = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SessionSettings owner,
        out SerializableDictionary<string, short> value)
      {
        value = owner.BlockTypeLimits;
      }
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableScripterRole\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableScripterRole = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableScripterRole;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EMinDropContainerRespawnTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in int value) => owner.MinDropContainerRespawnTime = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out int value) => value = owner.MinDropContainerRespawnTime;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EMaxDropContainerRespawnTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in int value) => owner.MaxDropContainerRespawnTime = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out int value) => value = owner.MaxDropContainerRespawnTime;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableTurretsFriendlyFire\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableTurretsFriendlyFire = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableTurretsFriendlyFire;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableSubgridDamage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableSubgridDamage = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableSubgridDamage;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003ESyncDistance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in int value) => owner.SyncDistance = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out int value) => value = owner.SyncDistance;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EExperimentalMode\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.ExperimentalMode = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.ExperimentalMode;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EAdaptiveSimulationQuality\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.AdaptiveSimulationQuality = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.AdaptiveSimulationQuality;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableVoxelHand\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableVoxelHand = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableVoxelHand;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003ERemoveOldIdentitiesH\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in int value) => owner.RemoveOldIdentitiesH = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out int value) => value = owner.RemoveOldIdentitiesH;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003ETrashRemovalEnabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.TrashRemovalEnabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.TrashRemovalEnabled;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EStopGridsPeriodMin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in int value) => owner.StopGridsPeriodMin = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out int value) => value = owner.StopGridsPeriodMin;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003ETrashFlagsValue\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in int value) => owner.TrashFlagsValue = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out int value) => value = owner.TrashFlagsValue;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EAFKTimeountMin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in int value) => owner.AFKTimeountMin = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out int value) => value = owner.AFKTimeountMin;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EBlockCountThreshold\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in int value) => owner.BlockCountThreshold = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out int value) => value = owner.BlockCountThreshold;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EPlayerDistanceThreshold\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in float value) => owner.PlayerDistanceThreshold = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out float value) => value = owner.PlayerDistanceThreshold;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EOptimalGridCount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in int value) => owner.OptimalGridCount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out int value) => value = owner.OptimalGridCount;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EPlayerInactivityThreshold\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in float value) => owner.PlayerInactivityThreshold = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out float value) => value = owner.PlayerInactivityThreshold;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EPlayerCharacterRemovalThreshold\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in int value) => owner.PlayerCharacterRemovalThreshold = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out int value) => value = owner.PlayerCharacterRemovalThreshold;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EVoxelTrashRemovalEnabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.VoxelTrashRemovalEnabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.VoxelTrashRemovalEnabled;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EVoxelPlayerDistanceThreshold\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in float value) => owner.VoxelPlayerDistanceThreshold = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out float value) => value = owner.VoxelPlayerDistanceThreshold;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EVoxelGridDistanceThreshold\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in float value) => owner.VoxelGridDistanceThreshold = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out float value) => value = owner.VoxelGridDistanceThreshold;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EVoxelAgeThreshold\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in int value) => owner.VoxelAgeThreshold = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out int value) => value = owner.VoxelAgeThreshold;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableResearch\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableResearch = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableResearch;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableGoodBotHints\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableGoodBotHints = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableGoodBotHints;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EOptimalSpawnDistance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in float value) => owner.OptimalSpawnDistance = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out float value) => value = owner.OptimalSpawnDistance;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableAutorespawn\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableAutorespawn = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableAutorespawn;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableBountyContracts\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableBountyContracts = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableBountyContracts;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableSupergridding\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableSupergridding = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableSupergridding;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableEconomy\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableEconomy = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableEconomy;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EDepositsCountCoefficient\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in float value) => owner.DepositsCountCoefficient = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out float value) => value = owner.DepositsCountCoefficient;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EDepositSizeDenominator\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in float value) => owner.DepositSizeDenominator = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out float value) => value = owner.DepositSizeDenominator;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EWeatherSystem\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.WeatherSystem = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.WeatherSystem;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EHarvestRatioMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in float value) => owner.HarvestRatioMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out float value) => value = owner.HarvestRatioMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003ETradeFactionsCount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in int value) => owner.TradeFactionsCount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out int value) => value = owner.TradeFactionsCount;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EStationsDistanceInnerRadius\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in double value) => owner.StationsDistanceInnerRadius = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out double value) => value = owner.StationsDistanceInnerRadius;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EStationsDistanceOuterRadiusStart\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in double value) => owner.StationsDistanceOuterRadiusStart = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out double value) => value = owner.StationsDistanceOuterRadiusStart;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EStationsDistanceOuterRadiusEnd\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in double value) => owner.StationsDistanceOuterRadiusEnd = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out double value) => value = owner.StationsDistanceOuterRadiusEnd;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEconomyTickInSeconds\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in int value) => owner.EconomyTickInSeconds = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out int value) => value = owner.EconomyTickInSeconds;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003ESimplifiedSimulation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.SimplifiedSimulation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.SimplifiedSimulation;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003ESuppressedWarnings\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in List<string> value) => owner.SuppressedWarnings = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out List<string> value) => value = owner.SuppressedWarnings;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnablePcuTrading\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnablePcuTrading = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnablePcuTrading;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EFamilySharing\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.FamilySharing = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.FamilySharing;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableSelectivePhysicsUpdates\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableSelectivePhysicsUpdates = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableSelectivePhysicsUpdates;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EPredefinedAsteroids\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.PredefinedAsteroids = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.PredefinedAsteroids;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EUseConsolePCU\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.UseConsolePCU = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.UseConsolePCU;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EMaxPlanets\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in int value) => owner.MaxPlanets = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out int value) => value = owner.MaxPlanets;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EOffensiveWordsFiltering\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.OffensiveWordsFiltering = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.OffensiveWordsFiltering;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EAdjustableMaxVehicleSpeed\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.AdjustableMaxVehicleSpeed = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.AdjustableMaxVehicleSpeed;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableMatchComponent\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableMatchComponent = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableMatchComponent;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EPreMatchDuration\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in float value) => owner.PreMatchDuration = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out float value) => value = owner.PreMatchDuration;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EMatchDuration\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in float value) => owner.MatchDuration = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out float value) => value = owner.MatchDuration;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EPostMatchDuration\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in float value) => owner.PostMatchDuration = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out float value) => value = owner.PostMatchDuration;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableFriendlyFire\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableFriendlyFire = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableFriendlyFire;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableTeamBalancing\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableTeamBalancing = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableTeamBalancing;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003ECharacterSpeedMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in float value) => owner.CharacterSpeedMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out float value) => value = owner.CharacterSpeedMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableRecoil\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableRecoil = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableRecoil;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnvironmentDamageMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in float value) => owner.EnvironmentDamageMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out float value) => value = owner.EnvironmentDamageMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableGamepadAimAssist\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableGamepadAimAssist = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableGamepadAimAssist;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EBackpackDespawnTimer\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in float value) => owner.BackpackDespawnTimer = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out float value) => value = owner.BackpackDespawnTimer;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EEnableFactionPlayerNames\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.EnableFactionPlayerNames = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.EnableFactionPlayerNames;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SessionSettings, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SessionSettings, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EAutoSave\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.AutoSave = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.AutoSave;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EClientCanSave\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in bool value) => owner.ClientCanSave = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out bool value) => value = owner.ClientCanSave;
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003ETrashFlags\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, MyTrashRemovalFlags>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SessionSettings owner,
        in MyTrashRemovalFlags value)
      {
        owner.TrashFlags = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SessionSettings owner,
        out MyTrashRemovalFlags value)
      {
        value = owner.TrashFlags;
      }
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SessionSettings, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SessionSettings, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionSettings owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionSettings owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_SessionSettings\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_SessionSettings>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_SessionSettings();

      MyObjectBuilder_SessionSettings IActivator<MyObjectBuilder_SessionSettings>.CreateInstance() => new MyObjectBuilder_SessionSettings();
    }
  }
}
