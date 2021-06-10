// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Debugging.VSWaypointPathUpdatedMsg
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
  public struct VSWaypointPathUpdatedMsg : MyExternalDebugStructures.IExternalDebugMsg
  {
    [ProtoMember(5)]
    public long Id;
    [ProtoMember(10)]
    public string Path;

    string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr() => "VS_WPUD";

    protected class Sandbox_Game_Debugging_VSWaypointPathUpdatedMsg\u003C\u003EId\u003C\u003EAccessor : IMemberAccessor<VSWaypointPathUpdatedMsg, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref VSWaypointPathUpdatedMsg owner, in long value) => owner.Id = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref VSWaypointPathUpdatedMsg owner, out long value) => value = owner.Id;
    }

    protected class Sandbox_Game_Debugging_VSWaypointPathUpdatedMsg\u003C\u003EPath\u003C\u003EAccessor : IMemberAccessor<VSWaypointPathUpdatedMsg, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref VSWaypointPathUpdatedMsg owner, in string value) => owner.Path = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref VSWaypointPathUpdatedMsg owner, out string value) => value = owner.Path;
    }

    private class Sandbox_Game_Debugging_VSWaypointPathUpdatedMsg\u003C\u003EActor : IActivator, IActivator<VSWaypointPathUpdatedMsg>
    {
      object IActivator.CreateInstance() => (object) new VSWaypointPathUpdatedMsg();

      VSWaypointPathUpdatedMsg IActivator<VSWaypointPathUpdatedMsg>.CreateInstance() => new VSWaypointPathUpdatedMsg();
    }
  }
}
