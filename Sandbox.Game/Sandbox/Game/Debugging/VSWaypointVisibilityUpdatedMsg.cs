// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Debugging.VSWaypointVisibilityUpdatedMsg
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
  public struct VSWaypointVisibilityUpdatedMsg : MyExternalDebugStructures.IExternalDebugMsg
  {
    [ProtoMember(5)]
    public long Id;
    [ProtoMember(10)]
    public bool Visible;

    string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr() => "VS_WVUD";

    protected class Sandbox_Game_Debugging_VSWaypointVisibilityUpdatedMsg\u003C\u003EId\u003C\u003EAccessor : IMemberAccessor<VSWaypointVisibilityUpdatedMsg, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref VSWaypointVisibilityUpdatedMsg owner, in long value) => owner.Id = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref VSWaypointVisibilityUpdatedMsg owner, out long value) => value = owner.Id;
    }

    protected class Sandbox_Game_Debugging_VSWaypointVisibilityUpdatedMsg\u003C\u003EVisible\u003C\u003EAccessor : IMemberAccessor<VSWaypointVisibilityUpdatedMsg, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref VSWaypointVisibilityUpdatedMsg owner, in bool value) => owner.Visible = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref VSWaypointVisibilityUpdatedMsg owner, out bool value) => value = owner.Visible;
    }

    private class Sandbox_Game_Debugging_VSWaypointVisibilityUpdatedMsg\u003C\u003EActor : IActivator, IActivator<VSWaypointVisibilityUpdatedMsg>
    {
      object IActivator.CreateInstance() => (object) new VSWaypointVisibilityUpdatedMsg();

      VSWaypointVisibilityUpdatedMsg IActivator<VSWaypointVisibilityUpdatedMsg>.CreateInstance() => new VSWaypointVisibilityUpdatedMsg();
    }
  }
}
