// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Multiplayer.MyControlKickClientMsg
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace Sandbox.Engine.Multiplayer
{
  [Serializable]
  public struct MyControlKickClientMsg
  {
    public ulong KickedClient;
    public BoolBlit Kicked;
    public BoolBlit Add;

    protected class Sandbox_Engine_Multiplayer_MyControlKickClientMsg\u003C\u003EKickedClient\u003C\u003EAccessor : IMemberAccessor<MyControlKickClientMsg, ulong>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyControlKickClientMsg owner, in ulong value) => owner.KickedClient = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyControlKickClientMsg owner, out ulong value) => value = owner.KickedClient;
    }

    protected class Sandbox_Engine_Multiplayer_MyControlKickClientMsg\u003C\u003EKicked\u003C\u003EAccessor : IMemberAccessor<MyControlKickClientMsg, BoolBlit>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyControlKickClientMsg owner, in BoolBlit value) => owner.Kicked = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyControlKickClientMsg owner, out BoolBlit value) => value = owner.Kicked;
    }

    protected class Sandbox_Engine_Multiplayer_MyControlKickClientMsg\u003C\u003EAdd\u003C\u003EAccessor : IMemberAccessor<MyControlKickClientMsg, BoolBlit>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyControlKickClientMsg owner, in BoolBlit value) => owner.Add = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyControlKickClientMsg owner, out BoolBlit value) => value = owner.Add;
    }
  }
}
