// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MyPlayer
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.World
{
  public class MyPlayer : IMyPlayer
  {
    public const int BUILD_COLOR_SLOTS_COUNT = 14;
    private static readonly List<Vector3> m_buildColorDefaults = new List<Vector3>();
    private bool m_isWildlifeAgent;
    private MyNetworkClient m_client;
    private MyIdentity m_identity;
    private int m_selectedBuildColorSlot;
    private string m_buildArmorSkin = string.Empty;
    private List<Vector3> m_buildColorHSVSlots = new List<Vector3>();
    private bool m_forceRealPlayer;
    private int m_leftFilterTypeIndex;
    private int m_rightFilterTypeIndex;
    private MyGuiControlRadioButtonStyleEnum m_leftFilter;
    private MyGuiControlRadioButtonStyleEnum m_rightFilter;
    public HashSet<long> Grids = new HashSet<long>();
    public List<long> CachedControllerId;

    public static int SelectedColorSlot => MySession.Static.LocalHumanPlayer == null ? 0 : MySession.Static.LocalHumanPlayer.SelectedBuildColorSlot;

    public static Vector3 SelectedColor => MySession.Static.LocalHumanPlayer == null ? MyPlayer.m_buildColorDefaults[0] : MySession.Static.LocalHumanPlayer.SelectedBuildColor;

    public static ListReader<Vector3> ColorSlots => MySession.Static.LocalHumanPlayer == null ? new ListReader<Vector3>(MyPlayer.m_buildColorDefaults) : (ListReader<Vector3>) MySession.Static.LocalHumanPlayer.BuildColorSlots;

    public static ListReader<Vector3> DefaultBuildColorSlots => (ListReader<Vector3>) MyPlayer.m_buildColorDefaults;

    public static string SelectedArmorSkin => MySession.Static.LocalHumanPlayer?.BuildArmorSkin ?? string.Empty;

    public bool IsWildlifeAgent
    {
      get => this.m_isWildlifeAgent;
      set => this.m_isWildlifeAgent = value;
    }

    public MyNetworkClient Client => this.m_client;

    public MyIdentity Identity
    {
      get => this.m_identity;
      set
      {
        this.m_identity = value;
        if (this.IdentityChanged == null)
          return;
        this.IdentityChanged(this, value);
      }
    }

    public event Action<MyPlayer, MyIdentity> IdentityChanged;

    public MyEntityController Controller { get; private set; }

    public string DisplayName { get; private set; }

    public int SelectedBuildColorSlot
    {
      get => this.m_selectedBuildColorSlot;
      set
      {
        this.m_selectedBuildColorSlot = MathHelper.Clamp(value, 0, this.m_buildColorHSVSlots.Count - 1);
        MySession.Static.InvokeLocalPlayerSkinOrColorChanged();
      }
    }

    public Vector3 SelectedBuildColor
    {
      get => this.m_buildColorHSVSlots[this.m_selectedBuildColorSlot];
      set => this.m_buildColorHSVSlots[this.m_selectedBuildColorSlot] = value;
    }

    public List<Vector3> BuildColorSlots
    {
      get => this.m_buildColorHSVSlots;
      set => this.SetBuildColorSlots(value);
    }

    public string BuildArmorSkin
    {
      get => this.m_buildArmorSkin;
      set
      {
        this.m_buildArmorSkin = value;
        MySession.Static.InvokeLocalPlayerSkinOrColorChanged();
      }
    }

    public bool IsLocalPlayer => this.m_client == Sync.Clients.LocalClient;

    public bool IsRemotePlayer => this.m_client != Sync.Clients.LocalClient;

    public bool IsRealPlayer => this.m_forceRealPlayer || this.Id.SerialId == 0;

    public bool IsBot => !this.IsRealPlayer;

    public int LeftFilterTypeIndex
    {
      get => this.m_leftFilterTypeIndex;
      set => this.m_leftFilterTypeIndex = value;
    }

    public int RightFilterTypeIndex
    {
      get => this.m_rightFilterTypeIndex;
      set => this.m_rightFilterTypeIndex = value;
    }

    public MyGuiControlRadioButtonStyleEnum LeftFilter
    {
      get => this.m_leftFilter;
      set => this.m_leftFilter = value;
    }

    public MyGuiControlRadioButtonStyleEnum RightFilter
    {
      get => this.m_rightFilter;
      set => this.m_rightFilter = value;
    }

    public bool IsImmortal => this.IsRealPlayer && (uint) this.Id.SerialId > 0U;

    public MyCharacter Character => this.Identity.Character;

    public IEnumerable<MyCharacter> SavedCharacters
    {
      get
      {
        foreach (long savedCharacter in this.Identity.SavedCharacters)
        {
          MyCharacter entity;
          if (MyEntities.TryGetEntityById<MyCharacter>(savedCharacter, out entity))
            yield return entity;
        }
      }
    }

    public MyPlayer.PlayerId Id { get; protected set; }

    public List<long> RespawnShip => this.m_identity == null ? (List<long>) null : this.m_identity.RespawnShips;

    static MyPlayer() => MyPlayer.InitDefaultColors();

    public MyPlayer(MyNetworkClient client, MyPlayer.PlayerId id)
    {
      this.m_client = client;
      this.Id = id;
      this.Controller = new MyEntityController(this);
    }

    public void GetColorPreviousCurrentNext(out Vector3 prev, out Vector3 cur, out Vector3 next)
    {
      prev = this.m_buildColorHSVSlots[(this.m_selectedBuildColorSlot + this.m_buildColorHSVSlots.Count - 1) % this.m_buildColorHSVSlots.Count];
      cur = this.m_buildColorHSVSlots[this.m_selectedBuildColorSlot];
      next = this.m_buildColorHSVSlots[(this.m_selectedBuildColorSlot + 1) % this.m_buildColorHSVSlots.Count];
    }

    public void Init(MyObjectBuilder_Player objectBuilder)
    {
      this.DisplayName = objectBuilder.DisplayName;
      this.Identity = Sync.Players.TryGetIdentity(objectBuilder.IdentityId);
      this.m_forceRealPlayer = objectBuilder.ForceRealPlayer;
      this.m_isWildlifeAgent = objectBuilder.IsWildlifeAgent;
      this.m_buildArmorSkin = objectBuilder.BuildArmorSkin;
      this.m_selectedBuildColorSlot = objectBuilder.BuildColorSlot;
      if (this.m_buildColorHSVSlots.Count < 14)
      {
        int count = this.m_buildColorHSVSlots.Count;
        for (int index = 0; index < 14 - count; ++index)
          this.m_buildColorHSVSlots.Add(MyRenderComponentBase.OldBlackToHSV);
      }
      if (objectBuilder.BuildColorSlots == null || objectBuilder.BuildColorSlots.Count == 0)
        this.SetDefaultColors();
      else if (objectBuilder.BuildColorSlots.Count == 14)
        this.m_buildColorHSVSlots = objectBuilder.BuildColorSlots;
      else if (objectBuilder.BuildColorSlots.Count > 14)
      {
        this.m_buildColorHSVSlots = new List<Vector3>(14);
        for (int index = 0; index < 14; ++index)
          this.m_buildColorHSVSlots.Add(objectBuilder.BuildColorSlots[index]);
      }
      else
      {
        this.m_buildColorHSVSlots = objectBuilder.BuildColorSlots;
        for (int index = this.m_buildColorHSVSlots.Count - 1; index < 14; ++index)
          this.m_buildColorHSVSlots.Add(MyRenderComponentBase.OldBlackToHSV);
      }
      if (!Sync.IsServer || this.Id.SerialId != 0)
        return;
      if (MyCubeBuilder.AllPlayersColors == null)
        MyCubeBuilder.AllPlayersColors = new Dictionary<MyPlayer.PlayerId, List<Vector3>>();
      if (!MyCubeBuilder.AllPlayersColors.ContainsKey(this.Id))
        MyCubeBuilder.AllPlayersColors.Add(this.Id, this.m_buildColorHSVSlots);
      else
        MyCubeBuilder.AllPlayersColors.TryGetValue(this.Id, out this.m_buildColorHSVSlots);
    }

    public MyObjectBuilder_Player GetObjectBuilder()
    {
      MyObjectBuilder_Player objectBuilderPlayer = new MyObjectBuilder_Player()
      {
        DisplayName = this.DisplayName,
        IdentityId = this.Identity.IdentityId,
        Connected = true,
        ForceRealPlayer = this.m_forceRealPlayer
      };
      objectBuilderPlayer.BuildArmorSkin = this.m_buildArmorSkin;
      objectBuilderPlayer.BuildColorSlot = this.m_selectedBuildColorSlot;
      objectBuilderPlayer.IsWildlifeAgent = this.m_isWildlifeAgent;
      if (!MyPlayer.IsColorsSetToDefaults(this.m_buildColorHSVSlots))
      {
        objectBuilderPlayer.BuildColorSlots = new List<Vector3>();
        foreach (Vector3 buildColorHsvSlot in this.m_buildColorHSVSlots)
          objectBuilderPlayer.BuildColorSlots.Add(buildColorHsvSlot);
      }
      return objectBuilderPlayer;
    }

    public static bool IsColorsSetToDefaults(List<Vector3> colors)
    {
      if (colors.Count != 14)
        return false;
      for (int index = 0; index < 14; ++index)
      {
        if (colors[index] != MyPlayer.m_buildColorDefaults[index])
          return false;
      }
      return true;
    }

    public void SetDefaultColors()
    {
      for (int index = 0; index < 14; ++index)
        this.m_buildColorHSVSlots[index] = MyPlayer.m_buildColorDefaults[index];
    }

    private static void InitDefaultColors()
    {
      if (MyPlayer.m_buildColorDefaults.Count < 14)
      {
        int count = MyPlayer.m_buildColorDefaults.Count;
        for (int index = 0; index < 14 - count; ++index)
          MyPlayer.m_buildColorDefaults.Add(MyRenderComponentBase.OldBlackToHSV);
      }
      MyPlayer.m_buildColorDefaults[0] = MyRenderComponentBase.OldGrayToHSV;
      MyPlayer.m_buildColorDefaults[1] = MyRenderComponentBase.OldRedToHSV;
      MyPlayer.m_buildColorDefaults[2] = MyRenderComponentBase.OldGreenToHSV;
      MyPlayer.m_buildColorDefaults[3] = MyRenderComponentBase.OldBlueToHSV;
      MyPlayer.m_buildColorDefaults[4] = MyRenderComponentBase.OldYellowToHSV;
      MyPlayer.m_buildColorDefaults[5] = MyRenderComponentBase.OldWhiteToHSV;
      MyPlayer.m_buildColorDefaults[6] = MyRenderComponentBase.OldBlackToHSV;
      for (int index = 7; index < 14; ++index)
        MyPlayer.m_buildColorDefaults[index] = MyPlayer.m_buildColorDefaults[index - 7] + new Vector3(0.0f, 0.15f, 0.2f);
    }

    public void ChangeOrSwitchToColor(Vector3 color)
    {
      for (int index = 0; index < 14; ++index)
      {
        if (this.m_buildColorHSVSlots[index] == color)
        {
          this.m_selectedBuildColorSlot = index;
          return;
        }
      }
      this.SelectedBuildColor = color;
    }

    public void SetBuildColorSlots(List<Vector3> newColors)
    {
      for (int index = 0; index < 14; ++index)
        this.m_buildColorHSVSlots[index] = MyRenderComponentBase.OldBlackToHSV;
      for (int index = 0; index < Math.Min(newColors.Count, 14); ++index)
        this.m_buildColorHSVSlots[index] = newColors[index];
      if (MyCubeBuilder.AllPlayersColors == null || !MyCubeBuilder.AllPlayersColors.Remove(this.Id))
        return;
      MyCubeBuilder.AllPlayersColors.Add(this.Id, this.m_buildColorHSVSlots);
    }

    public Vector3D GetPosition() => this.Controller.ControlledEntity != null && this.Controller.ControlledEntity.Entity != null ? this.Controller.ControlledEntity.Entity.PositionComp.GetPosition() : Vector3D.Zero;

    public void SpawnAt(
      MatrixD worldMatrix,
      Vector3 velocity,
      MyEntity spawnedBy,
      MyBotDefinition botDefinition,
      bool findFreePlace = true,
      string modelName = null,
      Color? color = null)
    {
      if (!Sync.IsServer || this.Identity == null)
        return;
      MatrixD worldMatrix1 = worldMatrix;
      Vector3 velocity1 = velocity;
      string displayName = this.Identity.DisplayName;
      string model = modelName ?? this.Identity.Model;
      Vector3? colorMask = color.HasValue ? new Vector3?(color.Value.ToVector3()) : new Vector3?();
      bool flag = this.Id.SerialId == 0;
      long identityId1 = this.Identity.IdentityId;
      MyBotDefinition botDefinition1 = botDefinition;
      int num1 = flag ? 1 : 0;
      long identityId2 = identityId1;
      MyCharacter character = MyCharacter.CreateCharacter(worldMatrix1, velocity1, displayName, model, colorMask, botDefinition1, false, useInventory: (num1 != 0), identityId: identityId2);
      if (findFreePlace)
      {
        float radius = character.Render.GetModel().BoundingBox.Size.Length() / 2f * 0.9f;
        Vector3 up = (Vector3) worldMatrix.Up;
        double num2 = (double) up.Normalize();
        Vector3 vector3 = up * (radius + 0.01f);
        MatrixD matrix = worldMatrix;
        matrix.Translation = worldMatrix.Translation + vector3;
        Vector3D? freePlace = MyEntities.FindFreePlace(ref matrix, (Vector3) matrix.GetDirectionVector(Base6Directions.Direction.Up), radius, 200, 15, 0.2f);
        if (!freePlace.HasValue)
        {
          freePlace = MyEntities.FindFreePlace(ref matrix, (Vector3) matrix.GetDirectionVector(Base6Directions.Direction.Right), radius, 200, 15, 0.2f);
          if (!freePlace.HasValue)
            freePlace = MyEntities.FindFreePlace(worldMatrix.Translation + vector3, radius, 200, 15, 0.2f);
        }
        if (freePlace.HasValue)
        {
          worldMatrix.Translation = freePlace.Value - vector3;
          character.PositionComp.SetWorldMatrix(ref worldMatrix);
        }
      }
      Sync.Players.SetPlayerCharacter(this, character, spawnedBy);
      Sync.Players.RevivePlayer(this);
    }

    public void SpawnIntoCharacter(MyCharacter character)
    {
      Sync.Players.SetPlayerCharacter(this, character, (MyEntity) null);
      Sync.Players.RevivePlayer(this);
    }

    public static MyRelationsBetweenPlayerAndBlock GetRelationBetweenPlayers(
      long playerId1,
      long playerId2)
    {
      if (playerId1 == playerId2)
        return MyRelationsBetweenPlayerAndBlock.Owner;
      IMyFaction playerFaction1 = MySession.Static.Factions.TryGetPlayerFaction(playerId1);
      IMyFaction playerFaction2 = MySession.Static.Factions.TryGetPlayerFaction(playerId2);
      if (playerFaction1 == null || playerFaction2 == null)
        return MyRelationsBetweenPlayerAndBlock.Enemies;
      if (playerFaction1 == playerFaction2)
        return MyRelationsBetweenPlayerAndBlock.FactionShare;
      return MySession.Static.Factions.GetRelationBetweenFactions(playerFaction1.FactionId, playerFaction2.FactionId).Item1 == MyRelationsBetweenFactions.Neutral ? MyRelationsBetweenPlayerAndBlock.Neutral : MyRelationsBetweenPlayerAndBlock.Enemies;
    }

    public MyRelationsBetweenPlayerAndBlock GetRelationTo(
      long playerId)
    {
      return this.Identity == null ? MyRelationsBetweenPlayerAndBlock.Enemies : MyPlayer.GetRelationBetweenPlayers(this.Identity.IdentityId, playerId);
    }

    public static MyRelationsBetweenPlayers GetRelationsBetweenPlayers(
      long playerId1,
      long playerId2)
    {
      return MyIDModule.GetRelationPlayerPlayer(playerId1, playerId2);
    }

    public void RemoveGrid(long gridEntityId) => this.Grids.Remove(gridEntityId);

    public void AddGrid(long gridEntityId) => this.Grids.Add(gridEntityId);

    public static MyPlayer GetPlayerFromCharacter(MyCharacter character)
    {
      if (character == null)
        return (MyPlayer) null;
      return character.ControllerInfo != null && character.ControllerInfo.Controller != null ? character.ControllerInfo.Controller.Player : (MyPlayer) null;
    }

    public static MyPlayer GetPlayerFromWeapon(IMyGunBaseUser gunUser)
    {
      if (gunUser == null)
        return (MyPlayer) null;
      return gunUser.Owner is MyCharacter owner ? MyPlayer.GetPlayerFromCharacter(owner) : (MyPlayer) null;
    }

    public void ReleaseControls()
    {
      this.Controller.SaveCamera();
      if (this.Controller.ControlledEntity == null)
        return;
      this.Controller.ControlledEntity.ControllerInfo.ReleaseControls();
    }

    public void AcquireControls()
    {
      if (this.Controller.ControlledEntity != null)
        this.Controller.ControlledEntity.ControllerInfo.AcquireControls();
      this.Controller.SetCamera();
    }

    event Action<IMyPlayer, IMyIdentity> IMyPlayer.IdentityChanged
    {
      add => this.IdentityChanged += this.GetDelegate(value);
      remove => this.IdentityChanged -= this.GetDelegate(value);
    }

    private Action<MyPlayer, MyIdentity> GetDelegate(
      Action<IMyPlayer, IMyIdentity> value)
    {
      return (Action<MyPlayer, MyIdentity>) Delegate.CreateDelegate(typeof (Action<MyPlayer, MyIdentity>), value.Target, value.Method);
    }

    IMyNetworkClient IMyPlayer.Client => (IMyNetworkClient) this.Client;

    MyRelationsBetweenPlayerAndBlock IMyPlayer.GetRelationTo(
      long playerId)
    {
      return this.GetRelationTo(playerId);
    }

    HashSet<long> IMyPlayer.Grids => this.Grids;

    void IMyPlayer.RemoveGrid(long gridEntityId) => this.RemoveGrid(gridEntityId);

    void IMyPlayer.AddGrid(long gridEntityId) => this.AddGrid(gridEntityId);

    IMyEntityController IMyPlayer.Controller => (IMyEntityController) this.Controller;

    string IMyPlayer.DisplayName => this.DisplayName;

    ulong IMyPlayer.SteamUserId => this.Id.SteamId;

    Vector3D IMyPlayer.GetPosition() => this.GetPosition();

    long IMyPlayer.PlayerID => this.Identity.IdentityId;

    long IMyPlayer.IdentityId => this.Identity.IdentityId;

    bool IMyPlayer.IsAdmin => MySession.Static.IsUserAdmin(this.Id.SteamId);

    bool IMyPlayer.IsPromoted => MySession.Static.IsUserSpaceMaster(this.Id.SteamId);

    MyPromoteLevel IMyPlayer.PromoteLevel => MySession.Static.GetUserPromoteLevel(this.Id.SteamId);

    IMyCharacter IMyPlayer.Character => (IMyCharacter) this.Character;

    Vector3 IMyPlayer.SelectedBuildColor
    {
      get => this.SelectedBuildColor;
      set => this.SelectedBuildColor = value;
    }

    int IMyPlayer.SelectedBuildColorSlot
    {
      get => this.SelectedBuildColorSlot;
      set => this.SelectedBuildColorSlot = value;
    }

    bool IMyPlayer.IsBot => this.IsBot;

    IMyIdentity IMyPlayer.Identity => (IMyIdentity) this.Identity;

    ListReader<long> IMyPlayer.RespawnShip => this.m_identity == null ? (ListReader<long>) (List<long>) null : (ListReader<long>) this.m_identity.RespawnShips;

    List<Vector3> IMyPlayer.BuildColorSlots
    {
      get => this.BuildColorSlots;
      set => this.BuildColorSlots = value;
    }

    ListReader<Vector3> IMyPlayer.DefaultBuildColorSlots => MyPlayer.DefaultBuildColorSlots;

    void IMyPlayer.ChangeOrSwitchToColor(Vector3 color) => this.ChangeOrSwitchToColor(color);

    void IMyPlayer.SetDefaultColors() => this.SetDefaultColors();

    void IMyPlayer.SpawnIntoCharacter(IMyCharacter character) => this.SpawnIntoCharacter((MyCharacter) character);

    void IMyPlayer.SpawnAt(
      MatrixD worldMatrix,
      Vector3 velocity,
      IMyEntity spawnedBy,
      bool findFreePlace,
      string modelName,
      Color? color)
    {
      this.SpawnAt(worldMatrix, velocity, (MyEntity) spawnedBy, (MyBotDefinition) null, findFreePlace, modelName, color);
    }

    void IMyPlayer.SpawnAt(MatrixD worldMatrix, Vector3 velocity, IMyEntity spawnedBy) => this.SpawnAt(worldMatrix, velocity, (MyEntity) spawnedBy, (MyBotDefinition) null);

    bool IMyPlayer.TryGetBalanceInfo(out long balance)
    {
      balance = 0L;
      MyAccountInfo account;
      if (MyBankingSystem.Static == null || !MyBankingSystem.Static.TryGetAccountInfo(this.Identity.IdentityId, out account))
        return false;
      balance = account.Balance;
      return true;
    }

    string IMyPlayer.GetBalanceShortString() => MyBankingSystem.Static == null ? (string) null : MyBankingSystem.Static.GetBalanceShortString(this.Identity.IdentityId);

    void IMyPlayer.RequestChangeBalance(long amount)
    {
      if (MyBankingSystem.Static == null)
        return;
      MyBankingSystem.ChangeBalance(this.Identity.IdentityId, amount);
    }

    [Serializable]
    public struct PlayerId : IComparable<MyPlayer.PlayerId>
    {
      public ulong SteamId;
      public int SerialId;
      public static readonly MyPlayer.PlayerId.PlayerIdComparerType Comparer = new MyPlayer.PlayerId.PlayerIdComparerType();

      public bool IsValid => this.SteamId > 0UL;

      public PlayerId(ulong steamId)
        : this(steamId, 0)
      {
      }

      public PlayerId(ulong steamId, int serialId)
      {
        this.SteamId = steamId;
        this.SerialId = serialId;
      }

      public static bool operator ==(MyPlayer.PlayerId a, MyPlayer.PlayerId b) => (long) a.SteamId == (long) b.SteamId && a.SerialId == b.SerialId;

      public static bool operator !=(MyPlayer.PlayerId a, MyPlayer.PlayerId b) => !(a == b);

      public override string ToString() => this.SteamId.ToString() + ":" + this.SerialId.ToString();

      public override bool Equals(object obj) => obj is MyPlayer.PlayerId playerId && playerId == this;

      public override int GetHashCode() => this.SteamId.GetHashCode() * 571 ^ this.SerialId.GetHashCode();

      public int CompareTo(MyPlayer.PlayerId other)
      {
        if (this.SteamId < other.SteamId)
          return -1;
        if (this.SteamId > other.SteamId)
          return 1;
        if (this.SerialId < other.SerialId)
          return -1;
        return this.SerialId > other.SerialId ? 1 : 0;
      }

      public static MyPlayer.PlayerId operator ++(MyPlayer.PlayerId id)
      {
        ++id.SerialId;
        return id;
      }

      public static MyPlayer.PlayerId operator --(MyPlayer.PlayerId id)
      {
        --id.SerialId;
        return id;
      }

      public class PlayerIdComparerType : IEqualityComparer<MyPlayer.PlayerId>
      {
        public bool Equals(MyPlayer.PlayerId left, MyPlayer.PlayerId right) => left == right;

        public int GetHashCode(MyPlayer.PlayerId playerId) => playerId.GetHashCode();
      }

      protected class Sandbox_Game_World_MyPlayer\u003C\u003EPlayerId\u003C\u003ESteamId\u003C\u003EAccessor : IMemberAccessor<MyPlayer.PlayerId, ulong>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyPlayer.PlayerId owner, in ulong value) => owner.SteamId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyPlayer.PlayerId owner, out ulong value) => value = owner.SteamId;
      }

      protected class Sandbox_Game_World_MyPlayer\u003C\u003EPlayerId\u003C\u003ESerialId\u003C\u003EAccessor : IMemberAccessor<MyPlayer.PlayerId, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyPlayer.PlayerId owner, in int value) => owner.SerialId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyPlayer.PlayerId owner, out int value) => value = owner.SerialId;
      }
    }
  }
}
