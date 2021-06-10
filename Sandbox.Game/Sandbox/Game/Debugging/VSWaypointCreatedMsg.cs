// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Debugging.VSWaypointCreatedMsg
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ProtoBuf;
using Sandbox.Common.ObjectBuilders;
using System.Runtime.CompilerServices;
using VRage.Game.Debugging;
using VRage.Network;

namespace Sandbox.Game.Debugging
{
  [ProtoContract]
  public struct VSWaypointCreatedMsg : MyExternalDebugStructures.IExternalDebugMsg
  {
    [ProtoMember(5)]
    public MyObjectBuilder_Waypoint Waypoint;

    string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr() => "VS_WCTD";

    protected class Sandbox_Game_Debugging_VSWaypointCreatedMsg\u003C\u003EWaypoint\u003C\u003EAccessor : IMemberAccessor<VSWaypointCreatedMsg, MyObjectBuilder_Waypoint>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref VSWaypointCreatedMsg owner, in MyObjectBuilder_Waypoint value) => owner.Waypoint = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref VSWaypointCreatedMsg owner, out MyObjectBuilder_Waypoint value) => value = owner.Waypoint;
    }

    private class Sandbox_Game_Debugging_VSWaypointCreatedMsg\u003C\u003EActor : IActivator, IActivator<VSWaypointCreatedMsg>
    {
      object IActivator.CreateInstance() => (object) new VSWaypointCreatedMsg();

      VSWaypointCreatedMsg IActivator<VSWaypointCreatedMsg>.CreateInstance() => new VSWaypointCreatedMsg();
    }
  }
}
