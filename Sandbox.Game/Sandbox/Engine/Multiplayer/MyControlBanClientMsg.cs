// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Multiplayer.MyControlBanClientMsg
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace Sandbox.Engine.Multiplayer
{
  [Serializable]
  public struct MyControlBanClientMsg
  {
    public ulong BannedClient;
    public BoolBlit Banned;

    protected class Sandbox_Engine_Multiplayer_MyControlBanClientMsg\u003C\u003EBannedClient\u003C\u003EAccessor : IMemberAccessor<MyControlBanClientMsg, ulong>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyControlBanClientMsg owner, in ulong value) => owner.BannedClient = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyControlBanClientMsg owner, out ulong value) => value = owner.BannedClient;
    }

    protected class Sandbox_Engine_Multiplayer_MyControlBanClientMsg\u003C\u003EBanned\u003C\u003EAccessor : IMemberAccessor<MyControlBanClientMsg, BoolBlit>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyControlBanClientMsg owner, in BoolBlit value) => owner.Banned = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyControlBanClientMsg owner, out BoolBlit value) => value = owner.Banned;
    }
  }
}
