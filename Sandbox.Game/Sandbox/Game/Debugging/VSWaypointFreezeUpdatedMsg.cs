// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Debugging.VSWaypointFreezeUpdatedMsg
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
  public struct VSWaypointFreezeUpdatedMsg : MyExternalDebugStructures.IExternalDebugMsg
  {
    [ProtoMember(5)]
    public long Id;
    [ProtoMember(10)]
    public bool Freeze;

    string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr() => "VS_WFUD";

    protected class Sandbox_Game_Debugging_VSWaypointFreezeUpdatedMsg\u003C\u003EId\u003C\u003EAccessor : IMemberAccessor<VSWaypointFreezeUpdatedMsg, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref VSWaypointFreezeUpdatedMsg owner, in long value) => owner.Id = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref VSWaypointFreezeUpdatedMsg owner, out long value) => value = owner.Id;
    }

    protected class Sandbox_Game_Debugging_VSWaypointFreezeUpdatedMsg\u003C\u003EFreeze\u003C\u003EAccessor : IMemberAccessor<VSWaypointFreezeUpdatedMsg, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref VSWaypointFreezeUpdatedMsg owner, in bool value) => owner.Freeze = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref VSWaypointFreezeUpdatedMsg owner, out bool value) => value = owner.Freeze;
    }

    private class Sandbox_Game_Debugging_VSWaypointFreezeUpdatedMsg\u003C\u003EActor : IActivator, IActivator<VSWaypointFreezeUpdatedMsg>
    {
      object IActivator.CreateInstance() => (object) new VSWaypointFreezeUpdatedMsg();

      VSWaypointFreezeUpdatedMsg IActivator<VSWaypointFreezeUpdatedMsg>.CreateInstance() => new VSWaypointFreezeUpdatedMsg();
    }
  }
}
