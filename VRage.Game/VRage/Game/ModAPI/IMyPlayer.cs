// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.IMyPlayer
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.ModAPI;
using VRageMath;

namespace VRage.Game.ModAPI
{
  public interface IMyPlayer
  {
    event Action<IMyPlayer, IMyIdentity> IdentityChanged;

    IMyNetworkClient Client { get; }

    MyRelationsBetweenPlayerAndBlock GetRelationTo(long playerId);

    HashSet<long> Grids { get; }

    void AddGrid(long gridEntityId);

    void RemoveGrid(long gridEntityId);

    IMyEntityController Controller { get; }

    Vector3D GetPosition();

    ulong SteamUserId { get; }

    string DisplayName { get; }

    [Obsolete("Use IdentityId instead.")]
    long PlayerID { get; }

    long IdentityId { get; }

    [Obsolete("Use Promote Level instead")]
    bool IsAdmin { get; }

    [Obsolete("Use Promote Level instead")]
    bool IsPromoted { get; }

    MyPromoteLevel PromoteLevel { get; }

    IMyCharacter Character { get; }

    bool IsBot { get; }

    IMyIdentity Identity { get; }

    ListReader<long> RespawnShip { get; }

    List<Vector3> BuildColorSlots { get; set; }

    ListReader<Vector3> DefaultBuildColorSlots { get; }

    Vector3 SelectedBuildColor { get; set; }

    int SelectedBuildColorSlot { get; set; }

    void ChangeOrSwitchToColor(Vector3 color);

    void SetDefaultColors();

    void SpawnIntoCharacter(IMyCharacter character);

    void SpawnAt(
      MatrixD worldMatrix,
      Vector3 velocity,
      IMyEntity spawnedBy,
      bool findFreePlace = true,
      string modelName = null,
      Color? color = null);

    void SpawnAt(MatrixD worldMatrix, Vector3 velocity, IMyEntity spawnedBy);

    bool TryGetBalanceInfo(out long balance);

    string GetBalanceShortString();

    void RequestChangeBalance(long amount);
  }
}
