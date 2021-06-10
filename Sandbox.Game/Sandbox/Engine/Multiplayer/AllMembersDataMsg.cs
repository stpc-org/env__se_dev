// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Multiplayer.AllMembersDataMsg
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Game;
using VRage.Network;

namespace Sandbox.Engine.Multiplayer
{
  [Serializable]
  public struct AllMembersDataMsg
  {
    public List<MyObjectBuilder_Identity> Identities;
    public List<MyPlayerCollection.AllPlayerData> Players;
    public List<MyObjectBuilder_Faction> Factions;
    public List<MyObjectBuilder_Client> Clients;

    protected class Sandbox_Engine_Multiplayer_AllMembersDataMsg\u003C\u003EIdentities\u003C\u003EAccessor : IMemberAccessor<AllMembersDataMsg, List<MyObjectBuilder_Identity>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref AllMembersDataMsg owner, in List<MyObjectBuilder_Identity> value) => owner.Identities = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref AllMembersDataMsg owner, out List<MyObjectBuilder_Identity> value) => value = owner.Identities;
    }

    protected class Sandbox_Engine_Multiplayer_AllMembersDataMsg\u003C\u003EPlayers\u003C\u003EAccessor : IMemberAccessor<AllMembersDataMsg, List<MyPlayerCollection.AllPlayerData>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref AllMembersDataMsg owner,
        in List<MyPlayerCollection.AllPlayerData> value)
      {
        owner.Players = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref AllMembersDataMsg owner,
        out List<MyPlayerCollection.AllPlayerData> value)
      {
        value = owner.Players;
      }
    }

    protected class Sandbox_Engine_Multiplayer_AllMembersDataMsg\u003C\u003EFactions\u003C\u003EAccessor : IMemberAccessor<AllMembersDataMsg, List<MyObjectBuilder_Faction>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref AllMembersDataMsg owner, in List<MyObjectBuilder_Faction> value) => owner.Factions = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref AllMembersDataMsg owner, out List<MyObjectBuilder_Faction> value) => value = owner.Factions;
    }

    protected class Sandbox_Engine_Multiplayer_AllMembersDataMsg\u003C\u003EClients\u003C\u003EAccessor : IMemberAccessor<AllMembersDataMsg, List<MyObjectBuilder_Client>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref AllMembersDataMsg owner, in List<MyObjectBuilder_Client> value) => owner.Clients = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref AllMembersDataMsg owner, out List<MyObjectBuilder_Client> value) => value = owner.Clients;
    }
  }
}
