// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Debugging.VSWaypointDeletedMsg
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Game.Debugging;
using VRage.Network;

namespace Sandbox.Game.Debugging
{
  [ProtoContract]
  public struct VSWaypointDeletedMsg : MyExternalDebugStructures.IExternalDebugMsg
  {
    [ProtoMember(5)]
    public long Id;

    string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr() => "VS_WDLT";

    protected class Sandbox_Game_Debugging_VSWaypointDeletedMsg\u003C\u003EId\u003C\u003EAccessor : IMemberAccessor<VSWaypointDeletedMsg, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref VSWaypointDeletedMsg owner, in long value) => owner.Id = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref VSWaypointDeletedMsg owner, out long value) => value = owner.Id;
    }

    private class Sandbox_Game_Debugging_VSWaypointDeletedMsg\u003C\u003EActor : IActivator, IActivator<VSWaypointDeletedMsg>
    {
      object IActivator.CreateInstance() => (object) new VSWaypointDeletedMsg();

      VSWaypointDeletedMsg IActivator<VSWaypointDeletedMsg>.CreateInstance() => new VSWaypointDeletedMsg();
    }
  }
}
