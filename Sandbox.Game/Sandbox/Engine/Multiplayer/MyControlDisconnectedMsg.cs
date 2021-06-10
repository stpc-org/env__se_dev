// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Multiplayer.MyControlDisconnectedMsg
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace Sandbox.Engine.Multiplayer
{
  [Serializable]
  public struct MyControlDisconnectedMsg
  {
    public ulong Client;

    protected class Sandbox_Engine_Multiplayer_MyControlDisconnectedMsg\u003C\u003EClient\u003C\u003EAccessor : IMemberAccessor<MyControlDisconnectedMsg, ulong>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyControlDisconnectedMsg owner, in ulong value) => owner.Client = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyControlDisconnectedMsg owner, out ulong value) => value = owner.Client;
    }
  }
}
