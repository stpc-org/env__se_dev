// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.IMySession
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.ModAPI.Interfaces;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ModAPI
{
  public interface IMySession
  {
    float AssemblerEfficiencyMultiplier { get; }

    float AssemblerSpeedMultiplier { get; }

    bool AutoHealing { get; }

    uint AutoSaveInMinutes { get; }

    void BeforeStartComponents();

    IMyCameraController CameraController { get; }

    bool CargoShipsEnabled { get; }

    [Obsolete("Client saving not supported anymore")]
    bool ClientCanSave { get; }

    bool CreativeMode { get; }

    string CurrentPath { get; }

    string Description { get; set; }

    IMyCamera Camera { get; }

    double CameraTargetDistance { get; set; }

    IMyPlayer LocalHumanPlayer { get; }

    IMyWeatherEffects WeatherEffects { get; }

    IMyConfig Config { get; }

    void Draw();

    TimeSpan ElapsedPlayTime { get; }

    bool EnableCopyPaste { get; }

    MyEnvironmentHostilityEnum EnvironmentHostility { get; }

    DateTime GameDateTime { get; set; }

    void GameOver();

    void GameOver(MyStringId? customMessage);

    MyObjectBuilder_Checkpoint GetCheckpoint(string saveName);

    MyObjectBuilder_Sector GetSector();

    Dictionary<string, byte[]> GetVoxelMapsArray();

    MyObjectBuilder_World GetWorld();

    float GrinderSpeedMultiplier { get; }

    float HackSpeedMultiplier { get; }

    float InventoryMultiplier { get; }

    float CharactersInventoryMultiplier { get; }

    float BlocksInventorySizeMultiplier { get; }

    bool IsCameraAwaitingEntity { get; set; }

    List<MyObjectBuilder_Checkpoint.ModItem> Mods { get; set; }

    bool IsCameraControlledObject { get; }

    bool IsCameraUserControlledSpectator { get; }

    bool IsPausable();

    bool IsServer { get; }

    short MaxFloatingObjects { get; }

    short MaxBackupSaves { get; }

    short MaxPlayers { get; }

    bool MultiplayerAlive { get; set; }

    bool MultiplayerDirect { get; set; }

    double MultiplayerLastMsg { get; set; }

    string Name { get; set; }

    float NegativeIntegrityTotal { get; set; }

    MyOnlineModeEnum OnlineMode { get; }

    string Password { get; set; }

    float PositiveIntegrityTotal { get; set; }

    float RefinerySpeedMultiplier { get; }

    void RegisterComponent(
      MySessionComponentBase component,
      MyUpdateOrder updateOrder,
      int priority);

    bool Save(string customSaveName = null);

    void SetCameraController(
      MyCameraControllerEnum cameraControllerEnum,
      IMyEntity cameraEntity = null,
      Vector3D? position = null);

    void SetAsNotReady();

    bool ShowPlayerNamesOnHud { get; }

    bool SurvivalMode { get; }

    bool ThrusterDamage { get; }

    string ThumbPath { get; }

    TimeSpan TimeOnBigShip { get; }

    TimeSpan TimeOnFoot { get; }

    TimeSpan TimeOnJetpack { get; }

    TimeSpan TimeOnSmallShip { get; }

    void Unload();

    void UnloadDataComponents();

    void UnloadMultiplayer();

    void UnregisterComponent(MySessionComponentBase component);

    void Update(MyTimeSpan time);

    void UpdateComponents();

    bool WeaponsEnabled { get; }

    float WelderSpeedMultiplier { get; }

    ulong? WorkshopId { get; }

    IMyVoxelMaps VoxelMaps { get; }

    IMyPlayer Player { get; }

    IMyControllableEntity ControlledObject { get; }

    MyObjectBuilder_SessionSettings SessionSettings { get; }

    IMyFactionCollection Factions { get; }

    IMyDamageSystem DamageSystem { get; }

    IMyGpsCollection GPS { get; }

    event Action OnSessionReady;

    event Action OnSessionLoading;

    BoundingBoxD WorldBoundaries { get; }

    MyPromoteLevel PromoteLevel { get; }

    MyPromoteLevel GetUserPromoteLevel(ulong steamId);

    bool HasCreativeRights { get; }

    bool IsUserAdmin(ulong steamId);

    [Obsolete("Use GetUserPromoteLevel")]
    bool IsUserPromoted(ulong steamId);

    [Obsolete("Use HasCreativeRights")]
    bool HasAdminPrivileges { get; }

    void SetComponentUpdateOrder(MySessionComponentBase component, MyUpdateOrder order);

    Version Version { get; }

    IMyOxygenProviderSystem OxygenProviderSystem { get; }
  }
}
