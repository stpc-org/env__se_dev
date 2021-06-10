// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Debugging.VSEntitiesMsg
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ProtoBuf;
using Sandbox.Common.ObjectBuilders;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Game.Debugging;
using VRage.Network;

namespace Sandbox.Game.Debugging
{
  [ProtoContract]
  public struct VSEntitiesMsg : MyExternalDebugStructures.IExternalDebugMsg
  {
    [ProtoMember(5)]
    public List<MyObjectBuilder_Waypoint> Waypoints;
    [ProtoMember(10)]
    public string[] Folders;

    string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr() => "VS_ENTS";

    protected class Sandbox_Game_Debugging_VSEntitiesMsg\u003C\u003EWaypoints\u003C\u003EAccessor : IMemberAccessor<VSEntitiesMsg, List<MyObjectBuilder_Waypoint>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref VSEntitiesMsg owner, in List<MyObjectBuilder_Waypoint> value) => owner.Waypoints = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref VSEntitiesMsg owner, out List<MyObjectBuilder_Waypoint> value) => value = owner.Waypoints;
    }

    protected class Sandbox_Game_Debugging_VSEntitiesMsg\u003C\u003EFolders\u003C\u003EAccessor : IMemberAccessor<VSEntitiesMsg, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref VSEntitiesMsg owner, in string[] value) => owner.Folders = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref VSEntitiesMsg owner, out string[] value) => value = owner.Folders;
    }

    private class Sandbox_Game_Debugging_VSEntitiesMsg\u003C\u003EActor : IActivator, IActivator<VSEntitiesMsg>
    {
      object IActivator.CreateInstance() => (object) new VSEntitiesMsg();

      VSEntitiesMsg IActivator<VSEntitiesMsg>.CreateInstance() => new VSEntitiesMsg();
    }
  }
}
