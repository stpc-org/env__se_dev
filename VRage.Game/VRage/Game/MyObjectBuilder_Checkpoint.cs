// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_Checkpoint
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.FileSystem;
using VRage.Game.Definitions;
using VRage.Game.ModAPI;
using VRage.GameServices;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_Checkpoint : MyObjectBuilder_Base
  {
    private static SerializableDefinitionId DEFAULT_SCENARIO = new SerializableDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_ScenarioDefinition), "EmptyWorld");
    public static DateTime DEFAULT_DATE = new DateTime(1215, 7, 1, 12, 0, 0);
    [ProtoMember(1)]
    public SerializableVector3I CurrentSector;
    [ProtoMember(4)]
    public long ElapsedGameTime;
    [ProtoMember(7)]
    public string SessionName;
    [ProtoMember(10)]
    public MyPositionAndOrientation SpectatorPosition = new MyPositionAndOrientation((MatrixD) ref Matrix.Identity);
    [ProtoMember(11)]
    public SerializableVector2 SpectatorSpeed = new SerializableVector2(0.1f, 1f);
    [ProtoMember(13)]
    public bool SpectatorIsLightOn;
    [ProtoMember(16)]
    [DefaultValue(MyCameraControllerEnum.Spectator)]
    public MyCameraControllerEnum CameraController;
    [ProtoMember(19)]
    public long CameraEntity;
    [ProtoMember(22)]
    [DefaultValue(-1)]
    public long ControlledObject = -1;
    [ProtoMember(25)]
    public string Password;
    [ProtoMember(28)]
    public string Description;
    [ProtoMember(31)]
    public DateTime LastSaveTime;
    [ProtoMember(34)]
    public float SpectatorDistance;
    [ProtoMember(37)]
    [DefaultValue(null)]
    public ulong? WorkshopId;
    [ProtoMember(38)]
    [DefaultValue(null)]
    public string WorkshopServiceName;
    [ProtoMember(39)]
    [DefaultValue(null)]
    public ulong? WorkshopId1;
    [ProtoMember(41)]
    [DefaultValue(null)]
    public string WorkshopServiceName1;
    [ProtoMember(40)]
    public MyObjectBuilder_Toolbar CharacterToolbar;
    [ProtoMember(43)]
    [DefaultValue(null)]
    public SerializableDictionary<long, MyObjectBuilder_Checkpoint.PlayerId> ControlledEntities;
    [ProtoMember(46)]
    [XmlElement("Settings", Type = typeof (MyAbstractXmlSerializer<MyObjectBuilder_SessionSettings>))]
    public MyObjectBuilder_SessionSettings Settings = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_SessionSettings>();
    public MyObjectBuilder_ScriptManager ScriptManagerData;
    [ProtoMember(49)]
    public int AppVersion;
    [ProtoMember(52)]
    [DefaultValue(null)]
    public MyObjectBuilder_FactionCollection Factions;
    [ProtoMember(82)]
    public List<MyObjectBuilder_Checkpoint.ModItem> Mods;
    [ProtoMember(85)]
    public SerializableDictionary<ulong, MyPromoteLevel> PromotedUsers;
    public HashSet<ulong> CreativeTools;
    [ProtoMember(88)]
    public SerializableDefinitionId Scenario = MyObjectBuilder_Checkpoint.DEFAULT_SCENARIO;
    [ProtoMember(103)]
    public List<MyObjectBuilder_Checkpoint.RespawnCooldownItem> RespawnCooldowns;
    [ProtoMember(106)]
    public List<MyObjectBuilder_Identity> Identities;
    [ProtoMember(109)]
    public List<MyObjectBuilder_Client> Clients;
    [ProtoMember(112)]
    public MyEnvironmentHostilityEnum? PreviousEnvironmentHostility;
    [ProtoMember(113)]
    [DefaultValue(0)]
    public ulong SharedToolbar;
    [ProtoMember(115)]
    public SerializableDictionary<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player> AllPlayersData;
    [ProtoMember(118)]
    public SerializableDictionary<MyObjectBuilder_Checkpoint.PlayerId, List<Vector3>> AllPlayersColors;
    [ProtoMember(121)]
    public List<MyObjectBuilder_ChatHistory> ChatHistory;
    [ProtoMember(124)]
    public List<MyObjectBuilder_FactionChatHistory> FactionChatHistory;
    [ProtoMember(127)]
    public List<long> NonPlayerIdentities;
    [ProtoMember(130)]
    public SerializableDictionary<long, MyObjectBuilder_Gps> Gps;
    [ProtoMember(133)]
    public SerializableBoundingBoxD? WorldBoundaries;
    [ProtoMember(136)]
    [XmlArrayItem("MyObjectBuilder_SessionComponent", Type = typeof (MyAbstractXmlSerializer<MyObjectBuilder_SessionComponent>))]
    public List<MyObjectBuilder_SessionComponent> SessionComponents;
    [ProtoMember(139)]
    public SerializableDefinitionId GameDefinition = (SerializableDefinitionId) MyGameDefinition.Default;
    [ProtoMember(142)]
    public HashSet<string> SessionComponentEnabled = new HashSet<string>();
    [ProtoMember(145)]
    public HashSet<string> SessionComponentDisabled = new HashSet<string>();
    [ProtoMember(148)]
    public DateTime InGameTime = MyObjectBuilder_Checkpoint.DEFAULT_DATE;
    public string CustomLoadingScreenImage;
    public string CustomLoadingScreenText;
    [ProtoMember(160)]
    public string CustomSkybox = "";
    [ProtoMember(163)]
    [DefaultValue(9)]
    public int RequiresDX = 9;
    [ProtoMember(166)]
    public List<string> VicinityModelsCache;
    [ProtoMember(169)]
    public List<string> VicinityArmorModelsCache;
    [ProtoMember(172)]
    public List<string> VicinityVoxelCache;
    [ProtoMember(175)]
    public SerializableDictionary<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player> ConnectedPlayers;
    [ProtoMember(178)]
    public SerializableDictionary<MyObjectBuilder_Checkpoint.PlayerId, long> DisconnectedPlayers;
    public List<MyObjectBuilder_Checkpoint.PlayerItem> AllPlayers;
    [ProtoMember(181)]
    public SerializableDictionary<ulong, int> RemoteAdminSettings = new SerializableDictionary<ulong, int>();

    public bool ShouldSerializeClients() => this.Clients != null && (uint) this.Clients.Count > 0U;

    public bool ShouldSerializeAllPlayersColors() => this.AllPlayersColors != null && this.AllPlayersColors.Dictionary.Count > 0;

    public bool ShouldSerializeWorldBoundaries() => this.WorldBoundaries.HasValue;

    public bool ShouldSerializeInGameTime() => this.InGameTime != MyObjectBuilder_Checkpoint.DEFAULT_DATE;

    public DateTime GameTime
    {
      get => new DateTime(2081, 1, 1, 0, 0, 0, DateTimeKind.Utc) + new TimeSpan(this.ElapsedGameTime);
      set => this.ElapsedGameTime = (value - new DateTime(2081, 1, 1)).Ticks;
    }

    public bool ShouldSerializeGameTime() => false;

    public MyOnlineModeEnum OnlineMode
    {
      get => this.Settings.OnlineMode;
      set => this.Settings.OnlineMode = value;
    }

    public bool ShouldSerializeOnlineMode() => false;

    public bool AutoHealing
    {
      get => this.Settings.AutoHealing;
      set => this.Settings.AutoHealing = value;
    }

    public bool ShouldSerializeAutoHealing() => false;

    public bool ShouldSerializeConnectedPlayers() => false;

    public bool ShouldSerializeDisconnectedPlayers() => false;

    public bool EnableCopyPaste
    {
      get => this.Settings.EnableCopyPaste;
      set => this.Settings.EnableCopyPaste = value;
    }

    public bool ShouldSerializeEnableCopyPaste() => false;

    public short MaxPlayers
    {
      get => this.Settings.MaxPlayers;
      set => this.Settings.MaxPlayers = value;
    }

    public bool ShouldSerializeMaxPlayers() => false;

    public bool WeaponsEnabled
    {
      get => this.Settings.WeaponsEnabled;
      set => this.Settings.WeaponsEnabled = value;
    }

    public bool ShouldSerializeWeaponsEnabled() => false;

    public bool ShowPlayerNamesOnHud
    {
      get => this.Settings.ShowPlayerNamesOnHud;
      set => this.Settings.ShowPlayerNamesOnHud = value;
    }

    public bool ShouldSerializeShowPlayerNamesOnHud() => false;

    public short MaxFloatingObjects
    {
      get => this.Settings.MaxFloatingObjects;
      set => this.Settings.MaxFloatingObjects = value;
    }

    public bool ShouldSerializeMaxFloatingObjects() => false;

    public MyGameModeEnum GameMode
    {
      get => this.Settings.GameMode;
      set => this.Settings.GameMode = value;
    }

    public bool ShouldSerializeGameMode() => false;

    public float InventorySizeMultiplier
    {
      get => this.Settings.InventorySizeMultiplier;
      set => this.Settings.InventorySizeMultiplier = value;
    }

    public bool ShouldSerializeInventorySizeMultiplier() => false;

    public float AssemblerSpeedMultiplier
    {
      get => this.Settings.AssemblerSpeedMultiplier;
      set => this.Settings.AssemblerSpeedMultiplier = value;
    }

    public bool ShouldSerializeAssemblerSpeedMultiplier() => false;

    public float AssemblerEfficiencyMultiplier
    {
      get => this.Settings.AssemblerEfficiencyMultiplier;
      set => this.Settings.AssemblerEfficiencyMultiplier = value;
    }

    public bool ShouldSerializeAssemblerEfficiencyMultiplier() => false;

    public float RefinerySpeedMultiplier
    {
      get => this.Settings.RefinerySpeedMultiplier;
      set => this.Settings.RefinerySpeedMultiplier = value;
    }

    public bool ShouldSerializeRefinerySpeedMultiplier() => false;

    public bool ThrusterDamage
    {
      get => this.Settings.ThrusterDamage;
      set => this.Settings.ThrusterDamage = value;
    }

    public bool ShouldSerializeThrusterDamage() => false;

    public bool CargoShipsEnabled
    {
      get => this.Settings.CargoShipsEnabled;
      set => this.Settings.CargoShipsEnabled = value;
    }

    public bool ShouldSerializeCargoShipsEnabled() => false;

    public bool ShouldSerializeAllPlayers() => false;

    public bool AutoSave
    {
      get => this.Settings.AutoSaveInMinutes > 0U;
      set => this.Settings.AutoSaveInMinutes = value ? 5U : 0U;
    }

    public bool ShouldSerializeAutoSave() => false;

    public bool IsSettingsExperimental(bool remoteSetting)
    {
      List<MyObjectBuilder_Checkpoint.ModItem> mods = this.Mods;
      // ISSUE: explicit non-virtual call
      if ((mods != null ? (__nonvirtual (mods.Count) > 0 ? 1 : 0) : 0) != 0)
        return true;
      MyObjectBuilder_SessionSettings settings = this.Settings;
      return settings != null && settings.IsSettingsExperimental(remoteSetting);
    }

    [ProtoContract]
    public struct PlayerId : IEquatable<MyObjectBuilder_Checkpoint.PlayerId>
    {
      [ProtoMember(183)]
      public ulong ClientId;
      [ProtoMember(185)]
      public int SerialId;
      [ProtoMember(186)]
      public ulong HashedId;

      public ulong GetClientId() => this.ClientId != 0UL ? this.ClientId : MyObjectBuilder_Checkpoint.PlayerId.UnHash(this.HashedId);

      public ulong GetHashedId() => this.HashedId != 0UL ? this.HashedId : MyObjectBuilder_Checkpoint.PlayerId.Hash(this.ClientId);

      public PlayerId(ulong steamId, int serialId = 0)
      {
        this.ClientId = 0UL;
        this.HashedId = MyObjectBuilder_Checkpoint.PlayerId.Hash(steamId);
        this.SerialId = serialId;
      }

      private static ulong Hash(ulong clientId)
      {
        clientId = (ulong) (((long) clientId ^ (long) (clientId >> 30)) * -4658895280553007687L);
        clientId = (ulong) (((long) clientId ^ (long) (clientId >> 27)) * -7723592293110705685L);
        clientId ^= clientId >> 31;
        return clientId;
      }

      private static ulong UnHash(ulong hashedId)
      {
        hashedId = (ulong) (((long) hashedId ^ (long) (hashedId >> 31) ^ (long) (hashedId >> 62)) * 3573116690164977347L);
        hashedId = (ulong) (((long) hashedId ^ (long) (hashedId >> 27) ^ (long) (hashedId >> 54)) * -7575587736534282103L);
        hashedId = hashedId ^ hashedId >> 30 ^ hashedId >> 60;
        return hashedId;
      }

      public bool Equals(MyObjectBuilder_Checkpoint.PlayerId other) => (long) this.GetHashedId() == (long) other.GetHashedId() && this.SerialId == other.SerialId;

      public override bool Equals(object obj) => obj is MyObjectBuilder_Checkpoint.PlayerId other && this.Equals(other);

      public override int GetHashCode() => this.GetHashedId().GetHashCode() * 397 ^ this.SerialId;

      protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EPlayerId\u003C\u003EClientId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint.PlayerId, ulong>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Checkpoint.PlayerId owner, in ulong value) => owner.ClientId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_Checkpoint.PlayerId owner, out ulong value) => value = owner.ClientId;
      }

      protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EPlayerId\u003C\u003ESerialId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint.PlayerId, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Checkpoint.PlayerId owner, in int value) => owner.SerialId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_Checkpoint.PlayerId owner, out int value) => value = owner.SerialId;
      }

      protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EPlayerId\u003C\u003EHashedId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint.PlayerId, ulong>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Checkpoint.PlayerId owner, in ulong value) => owner.HashedId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_Checkpoint.PlayerId owner, out ulong value) => value = owner.HashedId;
      }

      private class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EPlayerId\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Checkpoint.PlayerId>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_Checkpoint.PlayerId();

        MyObjectBuilder_Checkpoint.PlayerId IActivator<MyObjectBuilder_Checkpoint.PlayerId>.CreateInstance() => new MyObjectBuilder_Checkpoint.PlayerId();
      }
    }

    [ProtoContract]
    public struct PlayerItem
    {
      [ProtoMember(55)]
      public long PlayerId;
      [ProtoMember(58)]
      public bool IsDead;
      [ProtoMember(61)]
      public string Name;
      [ProtoMember(64)]
      public ulong SteamId;
      [ProtoMember(67)]
      public string Model;

      public PlayerItem(long id, string name, bool isDead, ulong steamId, string model)
      {
        this.PlayerId = id;
        this.IsDead = isDead;
        this.Name = name;
        this.SteamId = steamId;
        this.Model = model;
      }

      protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EPlayerItem\u003C\u003EPlayerId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint.PlayerItem, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Checkpoint.PlayerItem owner, in long value) => owner.PlayerId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_Checkpoint.PlayerItem owner, out long value) => value = owner.PlayerId;
      }

      protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EPlayerItem\u003C\u003EIsDead\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint.PlayerItem, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Checkpoint.PlayerItem owner, in bool value) => owner.IsDead = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_Checkpoint.PlayerItem owner, out bool value) => value = owner.IsDead;
      }

      protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EPlayerItem\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint.PlayerItem, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Checkpoint.PlayerItem owner, in string value) => owner.Name = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_Checkpoint.PlayerItem owner, out string value) => value = owner.Name;
      }

      protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EPlayerItem\u003C\u003ESteamId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint.PlayerItem, ulong>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Checkpoint.PlayerItem owner, in ulong value) => owner.SteamId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_Checkpoint.PlayerItem owner, out ulong value) => value = owner.SteamId;
      }

      protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EPlayerItem\u003C\u003EModel\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint.PlayerItem, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Checkpoint.PlayerItem owner, in string value) => owner.Model = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_Checkpoint.PlayerItem owner, out string value) => value = owner.Model;
      }

      private class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EPlayerItem\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Checkpoint.PlayerItem>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_Checkpoint.PlayerItem();

        MyObjectBuilder_Checkpoint.PlayerItem IActivator<MyObjectBuilder_Checkpoint.PlayerItem>.CreateInstance() => new MyObjectBuilder_Checkpoint.PlayerItem();
      }
    }

    [ProtoContract]
    public struct ModItem
    {
      private MyWorkshopItem m_workshopItem;
      [ProtoMember(70)]
      public string Name;
      [ProtoMember(73)]
      [DefaultValue(0)]
      public ulong PublishedFileId;
      [ProtoMember(74)]
      public string PublishedServiceName;
      [ProtoMember(76)]
      [DefaultValue(false)]
      public bool IsDependency;
      [ProtoMember(79)]
      [XmlAttribute]
      public string FriendlyName;

      public bool ShouldSerializeName() => this.Name != null;

      public bool ShouldSerializePublishedFileId() => this.PublishedFileId > 0UL;

      public bool ShouldSerializeIsDependency() => true;

      public bool ShouldSerializeFriendlyName() => !string.IsNullOrEmpty(this.FriendlyName);

      public ModItem(ulong publishedFileId, string publishedServiceName)
      {
        this.Name = publishedFileId.ToString() + ".sbm";
        this.PublishedFileId = publishedFileId;
        this.PublishedServiceName = publishedServiceName;
        this.FriendlyName = string.Empty;
        this.IsDependency = false;
        this.m_workshopItem = (MyWorkshopItem) null;
      }

      public ModItem(ulong publishedFileId, string publishedServiceName, bool isDependency)
      {
        this.Name = publishedFileId.ToString() + ".sbm";
        this.PublishedFileId = publishedFileId;
        this.PublishedServiceName = publishedServiceName;
        this.FriendlyName = string.Empty;
        this.IsDependency = isDependency;
        this.m_workshopItem = (MyWorkshopItem) null;
      }

      public ModItem(string name, ulong publishedFileId, string publishedServiceName)
      {
        this.Name = name ?? publishedFileId.ToString() + ".sbm";
        this.PublishedFileId = publishedFileId;
        this.PublishedServiceName = publishedServiceName;
        this.FriendlyName = string.Empty;
        this.IsDependency = false;
        this.m_workshopItem = (MyWorkshopItem) null;
      }

      public ModItem(
        string name,
        ulong publishedFileId,
        string publishedServiceName,
        string friendlyName)
      {
        this.Name = name ?? publishedFileId.ToString() + ".sbm";
        this.PublishedFileId = publishedFileId;
        this.PublishedServiceName = publishedServiceName;
        this.FriendlyName = friendlyName;
        this.IsDependency = false;
        this.m_workshopItem = (MyWorkshopItem) null;
      }

      public override string ToString() => string.Format("{0} ({1}:{2})", (object) this.FriendlyName, (object) this.PublishedServiceName, (object) this.PublishedFileId);

      public void SetModData(MyWorkshopItem workshopItem) => this.m_workshopItem = workshopItem;

      public bool IsModData() => this.m_workshopItem != null;

      public MyWorkshopItem GetModData()
      {
        if (this.m_workshopItem == null)
          throw new Exception("Mod data not initialized");
        return this.m_workshopItem;
      }

      public string GetPath() => this.m_workshopItem != null ? this.m_workshopItem.Folder : Path.Combine(MyFileSystem.ModsPath, this.Name);

      protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EModItem\u003C\u003Em_workshopItem\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint.ModItem, MyWorkshopItem>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_Checkpoint.ModItem owner,
          in MyWorkshopItem value)
        {
          owner.m_workshopItem = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_Checkpoint.ModItem owner,
          out MyWorkshopItem value)
        {
          value = owner.m_workshopItem;
        }
      }

      protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EModItem\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint.ModItem, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Checkpoint.ModItem owner, in string value) => owner.Name = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_Checkpoint.ModItem owner, out string value) => value = owner.Name;
      }

      protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EModItem\u003C\u003EPublishedFileId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint.ModItem, ulong>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Checkpoint.ModItem owner, in ulong value) => owner.PublishedFileId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_Checkpoint.ModItem owner, out ulong value) => value = owner.PublishedFileId;
      }

      protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EModItem\u003C\u003EPublishedServiceName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint.ModItem, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Checkpoint.ModItem owner, in string value) => owner.PublishedServiceName = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_Checkpoint.ModItem owner, out string value) => value = owner.PublishedServiceName;
      }

      protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EModItem\u003C\u003EIsDependency\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint.ModItem, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Checkpoint.ModItem owner, in bool value) => owner.IsDependency = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_Checkpoint.ModItem owner, out bool value) => value = owner.IsDependency;
      }

      protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EModItem\u003C\u003EFriendlyName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint.ModItem, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Checkpoint.ModItem owner, in string value) => owner.FriendlyName = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_Checkpoint.ModItem owner, out string value) => value = owner.FriendlyName;
      }

      private class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EModItem\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Checkpoint.ModItem>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_Checkpoint.ModItem();

        MyObjectBuilder_Checkpoint.ModItem IActivator<MyObjectBuilder_Checkpoint.ModItem>.CreateInstance() => new MyObjectBuilder_Checkpoint.ModItem();
      }
    }

    [ProtoContract]
    public struct RespawnCooldownItem
    {
      [ProtoMember(91)]
      [Obsolete]
      public ulong PlayerSteamId;
      [ProtoMember(94)]
      [Obsolete]
      public int PlayerSerialId;
      [ProtoMember(95)]
      public long IdentityId;
      [ProtoMember(97)]
      public string RespawnShipId;
      [ProtoMember(100)]
      public int Cooldown;

      protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ERespawnCooldownItem\u003C\u003EPlayerSteamId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint.RespawnCooldownItem, ulong>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_Checkpoint.RespawnCooldownItem owner,
          in ulong value)
        {
          owner.PlayerSteamId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_Checkpoint.RespawnCooldownItem owner,
          out ulong value)
        {
          value = owner.PlayerSteamId;
        }
      }

      protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ERespawnCooldownItem\u003C\u003EPlayerSerialId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint.RespawnCooldownItem, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_Checkpoint.RespawnCooldownItem owner,
          in int value)
        {
          owner.PlayerSerialId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_Checkpoint.RespawnCooldownItem owner,
          out int value)
        {
          value = owner.PlayerSerialId;
        }
      }

      protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ERespawnCooldownItem\u003C\u003EIdentityId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint.RespawnCooldownItem, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_Checkpoint.RespawnCooldownItem owner,
          in long value)
        {
          owner.IdentityId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_Checkpoint.RespawnCooldownItem owner,
          out long value)
        {
          value = owner.IdentityId;
        }
      }

      protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ERespawnCooldownItem\u003C\u003ERespawnShipId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint.RespawnCooldownItem, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_Checkpoint.RespawnCooldownItem owner,
          in string value)
        {
          owner.RespawnShipId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_Checkpoint.RespawnCooldownItem owner,
          out string value)
        {
          value = owner.RespawnShipId;
        }
      }

      protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ERespawnCooldownItem\u003C\u003ECooldown\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint.RespawnCooldownItem, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_Checkpoint.RespawnCooldownItem owner,
          in int value)
        {
          owner.Cooldown = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_Checkpoint.RespawnCooldownItem owner,
          out int value)
        {
          value = owner.Cooldown;
        }
      }

      private class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ERespawnCooldownItem\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Checkpoint.RespawnCooldownItem>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_Checkpoint.RespawnCooldownItem();

        MyObjectBuilder_Checkpoint.RespawnCooldownItem IActivator<MyObjectBuilder_Checkpoint.RespawnCooldownItem>.CreateInstance() => new MyObjectBuilder_Checkpoint.RespawnCooldownItem();
      }
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ECurrentSector\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, SerializableVector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in SerializableVector3I value) => owner.CurrentSector = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out SerializableVector3I value) => value = owner.CurrentSector;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EElapsedGameTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in long value) => owner.ElapsedGameTime = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out long value) => value = owner.ElapsedGameTime;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ESessionName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in string value) => owner.SessionName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out string value) => value = owner.SessionName;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ESpectatorPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, MyPositionAndOrientation>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Checkpoint owner,
        in MyPositionAndOrientation value)
      {
        owner.SpectatorPosition = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Checkpoint owner,
        out MyPositionAndOrientation value)
      {
        value = owner.SpectatorPosition;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ESpectatorSpeed\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, SerializableVector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in SerializableVector2 value) => owner.SpectatorSpeed = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out SerializableVector2 value) => value = owner.SpectatorSpeed;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ESpectatorIsLightOn\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in bool value) => owner.SpectatorIsLightOn = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out bool value) => value = owner.SpectatorIsLightOn;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ECameraController\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, MyCameraControllerEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in MyCameraControllerEnum value) => owner.CameraController = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Checkpoint owner,
        out MyCameraControllerEnum value)
      {
        value = owner.CameraController;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ECameraEntity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in long value) => owner.CameraEntity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out long value) => value = owner.CameraEntity;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EControlledObject\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in long value) => owner.ControlledObject = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out long value) => value = owner.ControlledObject;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EPassword\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in string value) => owner.Password = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out string value) => value = owner.Password;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EDescription\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in string value) => owner.Description = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out string value) => value = owner.Description;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ELastSaveTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, DateTime>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in DateTime value) => owner.LastSaveTime = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out DateTime value) => value = owner.LastSaveTime;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ESpectatorDistance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in float value) => owner.SpectatorDistance = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out float value) => value = owner.SpectatorDistance;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EWorkshopId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, ulong?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in ulong? value) => owner.WorkshopId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out ulong? value) => value = owner.WorkshopId;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EWorkshopServiceName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in string value) => owner.WorkshopServiceName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out string value) => value = owner.WorkshopServiceName;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EWorkshopId1\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, ulong?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in ulong? value) => owner.WorkshopId1 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out ulong? value) => value = owner.WorkshopId1;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EWorkshopServiceName1\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in string value) => owner.WorkshopServiceName1 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out string value) => value = owner.WorkshopServiceName1;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ECharacterToolbar\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, MyObjectBuilder_Toolbar>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Checkpoint owner,
        in MyObjectBuilder_Toolbar value)
      {
        owner.CharacterToolbar = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Checkpoint owner,
        out MyObjectBuilder_Toolbar value)
      {
        value = owner.CharacterToolbar;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EControlledEntities\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, SerializableDictionary<long, MyObjectBuilder_Checkpoint.PlayerId>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Checkpoint owner,
        in SerializableDictionary<long, MyObjectBuilder_Checkpoint.PlayerId> value)
      {
        owner.ControlledEntities = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Checkpoint owner,
        out SerializableDictionary<long, MyObjectBuilder_Checkpoint.PlayerId> value)
      {
        value = owner.ControlledEntities;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ESettings\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, MyObjectBuilder_SessionSettings>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Checkpoint owner,
        in MyObjectBuilder_SessionSettings value)
      {
        owner.Settings = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Checkpoint owner,
        out MyObjectBuilder_SessionSettings value)
      {
        value = owner.Settings;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EScriptManagerData\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, MyObjectBuilder_ScriptManager>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Checkpoint owner,
        in MyObjectBuilder_ScriptManager value)
      {
        owner.ScriptManagerData = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Checkpoint owner,
        out MyObjectBuilder_ScriptManager value)
      {
        value = owner.ScriptManagerData;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EAppVersion\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in int value) => owner.AppVersion = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out int value) => value = owner.AppVersion;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EFactions\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, MyObjectBuilder_FactionCollection>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Checkpoint owner,
        in MyObjectBuilder_FactionCollection value)
      {
        owner.Factions = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Checkpoint owner,
        out MyObjectBuilder_FactionCollection value)
      {
        value = owner.Factions;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EMods\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, List<MyObjectBuilder_Checkpoint.ModItem>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Checkpoint owner,
        in List<MyObjectBuilder_Checkpoint.ModItem> value)
      {
        owner.Mods = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Checkpoint owner,
        out List<MyObjectBuilder_Checkpoint.ModItem> value)
      {
        value = owner.Mods;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EPromotedUsers\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, SerializableDictionary<ulong, MyPromoteLevel>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Checkpoint owner,
        in SerializableDictionary<ulong, MyPromoteLevel> value)
      {
        owner.PromotedUsers = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Checkpoint owner,
        out SerializableDictionary<ulong, MyPromoteLevel> value)
      {
        value = owner.PromotedUsers;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ECreativeTools\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, HashSet<ulong>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in HashSet<ulong> value) => owner.CreativeTools = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out HashSet<ulong> value) => value = owner.CreativeTools;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EScenario\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Checkpoint owner,
        in SerializableDefinitionId value)
      {
        owner.Scenario = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Checkpoint owner,
        out SerializableDefinitionId value)
      {
        value = owner.Scenario;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ERespawnCooldowns\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, List<MyObjectBuilder_Checkpoint.RespawnCooldownItem>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Checkpoint owner,
        in List<MyObjectBuilder_Checkpoint.RespawnCooldownItem> value)
      {
        owner.RespawnCooldowns = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Checkpoint owner,
        out List<MyObjectBuilder_Checkpoint.RespawnCooldownItem> value)
      {
        value = owner.RespawnCooldowns;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EIdentities\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, List<MyObjectBuilder_Identity>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Checkpoint owner,
        in List<MyObjectBuilder_Identity> value)
      {
        owner.Identities = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Checkpoint owner,
        out List<MyObjectBuilder_Identity> value)
      {
        value = owner.Identities;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EClients\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, List<MyObjectBuilder_Client>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Checkpoint owner,
        in List<MyObjectBuilder_Client> value)
      {
        owner.Clients = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Checkpoint owner,
        out List<MyObjectBuilder_Client> value)
      {
        value = owner.Clients;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EPreviousEnvironmentHostility\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, MyEnvironmentHostilityEnum?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Checkpoint owner,
        in MyEnvironmentHostilityEnum? value)
      {
        owner.PreviousEnvironmentHostility = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Checkpoint owner,
        out MyEnvironmentHostilityEnum? value)
      {
        value = owner.PreviousEnvironmentHostility;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ESharedToolbar\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, ulong>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in ulong value) => owner.SharedToolbar = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out ulong value) => value = owner.SharedToolbar;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EAllPlayersData\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, SerializableDictionary<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Checkpoint owner,
        in SerializableDictionary<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player> value)
      {
        owner.AllPlayersData = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Checkpoint owner,
        out SerializableDictionary<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player> value)
      {
        value = owner.AllPlayersData;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EAllPlayersColors\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, SerializableDictionary<MyObjectBuilder_Checkpoint.PlayerId, List<Vector3>>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Checkpoint owner,
        in SerializableDictionary<MyObjectBuilder_Checkpoint.PlayerId, List<Vector3>> value)
      {
        owner.AllPlayersColors = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Checkpoint owner,
        out SerializableDictionary<MyObjectBuilder_Checkpoint.PlayerId, List<Vector3>> value)
      {
        value = owner.AllPlayersColors;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EChatHistory\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, List<MyObjectBuilder_ChatHistory>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Checkpoint owner,
        in List<MyObjectBuilder_ChatHistory> value)
      {
        owner.ChatHistory = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Checkpoint owner,
        out List<MyObjectBuilder_ChatHistory> value)
      {
        value = owner.ChatHistory;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EFactionChatHistory\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, List<MyObjectBuilder_FactionChatHistory>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Checkpoint owner,
        in List<MyObjectBuilder_FactionChatHistory> value)
      {
        owner.FactionChatHistory = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Checkpoint owner,
        out List<MyObjectBuilder_FactionChatHistory> value)
      {
        value = owner.FactionChatHistory;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ENonPlayerIdentities\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, List<long>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in List<long> value) => owner.NonPlayerIdentities = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out List<long> value) => value = owner.NonPlayerIdentities;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EGps\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, SerializableDictionary<long, MyObjectBuilder_Gps>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Checkpoint owner,
        in SerializableDictionary<long, MyObjectBuilder_Gps> value)
      {
        owner.Gps = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Checkpoint owner,
        out SerializableDictionary<long, MyObjectBuilder_Gps> value)
      {
        value = owner.Gps;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EWorldBoundaries\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, SerializableBoundingBoxD?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Checkpoint owner,
        in SerializableBoundingBoxD? value)
      {
        owner.WorldBoundaries = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Checkpoint owner,
        out SerializableBoundingBoxD? value)
      {
        value = owner.WorldBoundaries;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ESessionComponents\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, List<MyObjectBuilder_SessionComponent>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Checkpoint owner,
        in List<MyObjectBuilder_SessionComponent> value)
      {
        owner.SessionComponents = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Checkpoint owner,
        out List<MyObjectBuilder_SessionComponent> value)
      {
        value = owner.SessionComponents;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EGameDefinition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Checkpoint owner,
        in SerializableDefinitionId value)
      {
        owner.GameDefinition = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Checkpoint owner,
        out SerializableDefinitionId value)
      {
        value = owner.GameDefinition;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ESessionComponentEnabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, HashSet<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in HashSet<string> value) => owner.SessionComponentEnabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out HashSet<string> value) => value = owner.SessionComponentEnabled;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ESessionComponentDisabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, HashSet<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in HashSet<string> value) => owner.SessionComponentDisabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out HashSet<string> value) => value = owner.SessionComponentDisabled;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EInGameTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, DateTime>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in DateTime value) => owner.InGameTime = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out DateTime value) => value = owner.InGameTime;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ECustomLoadingScreenImage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in string value) => owner.CustomLoadingScreenImage = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out string value) => value = owner.CustomLoadingScreenImage;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ECustomLoadingScreenText\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in string value) => owner.CustomLoadingScreenText = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out string value) => value = owner.CustomLoadingScreenText;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ECustomSkybox\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in string value) => owner.CustomSkybox = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out string value) => value = owner.CustomSkybox;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ERequiresDX\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in int value) => owner.RequiresDX = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out int value) => value = owner.RequiresDX;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EVicinityModelsCache\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in List<string> value) => owner.VicinityModelsCache = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out List<string> value) => value = owner.VicinityModelsCache;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EVicinityArmorModelsCache\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in List<string> value) => owner.VicinityArmorModelsCache = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out List<string> value) => value = owner.VicinityArmorModelsCache;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EVicinityVoxelCache\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in List<string> value) => owner.VicinityVoxelCache = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out List<string> value) => value = owner.VicinityVoxelCache;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EConnectedPlayers\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, SerializableDictionary<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Checkpoint owner,
        in SerializableDictionary<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player> value)
      {
        owner.ConnectedPlayers = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Checkpoint owner,
        out SerializableDictionary<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player> value)
      {
        value = owner.ConnectedPlayers;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EDisconnectedPlayers\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, SerializableDictionary<MyObjectBuilder_Checkpoint.PlayerId, long>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Checkpoint owner,
        in SerializableDictionary<MyObjectBuilder_Checkpoint.PlayerId, long> value)
      {
        owner.DisconnectedPlayers = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Checkpoint owner,
        out SerializableDictionary<MyObjectBuilder_Checkpoint.PlayerId, long> value)
      {
        value = owner.DisconnectedPlayers;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EAllPlayers\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, List<MyObjectBuilder_Checkpoint.PlayerItem>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Checkpoint owner,
        in List<MyObjectBuilder_Checkpoint.PlayerItem> value)
      {
        owner.AllPlayers = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Checkpoint owner,
        out List<MyObjectBuilder_Checkpoint.PlayerItem> value)
      {
        value = owner.AllPlayers;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ERemoteAdminSettings\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, SerializableDictionary<ulong, int>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Checkpoint owner,
        in SerializableDictionary<ulong, int> value)
      {
        owner.RemoteAdminSettings = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Checkpoint owner,
        out SerializableDictionary<ulong, int> value)
      {
        value = owner.RemoteAdminSettings;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Checkpoint, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Checkpoint, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EGameTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, DateTime>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in DateTime value) => owner.GameTime = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out DateTime value) => value = owner.GameTime;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EOnlineMode\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, MyOnlineModeEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in MyOnlineModeEnum value) => owner.OnlineMode = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out MyOnlineModeEnum value) => value = owner.OnlineMode;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EAutoHealing\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in bool value) => owner.AutoHealing = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out bool value) => value = owner.AutoHealing;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EEnableCopyPaste\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in bool value) => owner.EnableCopyPaste = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out bool value) => value = owner.EnableCopyPaste;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EMaxPlayers\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, short>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in short value) => owner.MaxPlayers = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out short value) => value = owner.MaxPlayers;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EWeaponsEnabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in bool value) => owner.WeaponsEnabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out bool value) => value = owner.WeaponsEnabled;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EShowPlayerNamesOnHud\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in bool value) => owner.ShowPlayerNamesOnHud = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out bool value) => value = owner.ShowPlayerNamesOnHud;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EMaxFloatingObjects\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, short>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in short value) => owner.MaxFloatingObjects = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out short value) => value = owner.MaxFloatingObjects;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EGameMode\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, MyGameModeEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in MyGameModeEnum value) => owner.GameMode = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out MyGameModeEnum value) => value = owner.GameMode;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EInventorySizeMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in float value) => owner.InventorySizeMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out float value) => value = owner.InventorySizeMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EAssemblerSpeedMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in float value) => owner.AssemblerSpeedMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out float value) => value = owner.AssemblerSpeedMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EAssemblerEfficiencyMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in float value) => owner.AssemblerEfficiencyMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out float value) => value = owner.AssemblerEfficiencyMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ERefinerySpeedMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in float value) => owner.RefinerySpeedMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out float value) => value = owner.RefinerySpeedMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EThrusterDamage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in bool value) => owner.ThrusterDamage = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out bool value) => value = owner.ThrusterDamage;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ECargoShipsEnabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in bool value) => owner.CargoShipsEnabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out bool value) => value = owner.CargoShipsEnabled;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EAutoSave\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in bool value) => owner.AutoSave = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out bool value) => value = owner.AutoSave;
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Checkpoint, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Checkpoint, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Checkpoint owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Checkpoint owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_Checkpoint\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Checkpoint>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_Checkpoint();

      MyObjectBuilder_Checkpoint IActivator<MyObjectBuilder_Checkpoint>.CreateInstance() => new MyObjectBuilder_Checkpoint();
    }
  }
}
