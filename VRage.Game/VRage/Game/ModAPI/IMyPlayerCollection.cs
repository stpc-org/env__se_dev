// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.IMyPlayerCollection
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;

namespace VRage.Game.ModAPI
{
  public interface IMyPlayerCollection
  {
    void ExtendControl(IMyControllableEntity entityWithControl, IMyEntity entityGettingControl);

    void GetPlayers(List<IMyPlayer> players, Func<IMyPlayer, bool> collect = null);

    bool HasExtendedControl(IMyControllableEntity firstEntity, IMyEntity secondEntity);

    void ReduceControl(
      IMyControllableEntity entityWhichKeepsControl,
      IMyEntity entityWhichLoosesControl);

    void RemoveControlledEntity(IMyEntity entity);

    void TryExtendControl(IMyControllableEntity entityWithControl, IMyEntity entityGettingControl);

    bool TryReduceControl(
      IMyControllableEntity entityWhichKeepsControl,
      IMyEntity entityWhichLoosesControl);

    void SetControlledEntity(ulong steamUserId, IMyEntity entity);

    long Count { get; }

    IMyPlayer GetPlayerControllingEntity(IMyEntity entity);

    void GetAllIdentites(List<IMyIdentity> identities, Func<IMyIdentity, bool> collect = null);

    long TryGetIdentityId(ulong steamId);

    ulong TryGetSteamId(long identityId);

    void RequestChangeBalance(long identityId, long amount);
  }
}
