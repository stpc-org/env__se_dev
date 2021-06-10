// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Multiplayer.PlayerDataMsg
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Runtime.CompilerServices;
using VRage.Game;
using VRage.Network;

namespace Sandbox.Engine.Multiplayer
{
  [Serializable]
  public struct PlayerDataMsg
  {
    public ulong ClientSteamId;
    public int PlayerSerialId;
    public bool NewIdentity;
    public MyObjectBuilder_Player PlayerBuilder;

    protected class Sandbox_Engine_Multiplayer_PlayerDataMsg\u003C\u003EClientSteamId\u003C\u003EAccessor : IMemberAccessor<PlayerDataMsg, ulong>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PlayerDataMsg owner, in ulong value) => owner.ClientSteamId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PlayerDataMsg owner, out ulong value) => value = owner.ClientSteamId;
    }

    protected class Sandbox_Engine_Multiplayer_PlayerDataMsg\u003C\u003EPlayerSerialId\u003C\u003EAccessor : IMemberAccessor<PlayerDataMsg, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PlayerDataMsg owner, in int value) => owner.PlayerSerialId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PlayerDataMsg owner, out int value) => value = owner.PlayerSerialId;
    }

    protected class Sandbox_Engine_Multiplayer_PlayerDataMsg\u003C\u003ENewIdentity\u003C\u003EAccessor : IMemberAccessor<PlayerDataMsg, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PlayerDataMsg owner, in bool value) => owner.NewIdentity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PlayerDataMsg owner, out bool value) => value = owner.NewIdentity;
    }

    protected class Sandbox_Engine_Multiplayer_PlayerDataMsg\u003C\u003EPlayerBuilder\u003C\u003EAccessor : IMemberAccessor<PlayerDataMsg, MyObjectBuilder_Player>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PlayerDataMsg owner, in MyObjectBuilder_Player value) => owner.PlayerBuilder = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PlayerDataMsg owner, out MyObjectBuilder_Player value) => value = owner.PlayerBuilder;
    }
  }
}
