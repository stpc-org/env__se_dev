// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Debugging.VSWaypointRenamedMsg
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
  public struct VSWaypointRenamedMsg : MyExternalDebugStructures.IExternalDebugMsg
  {
    [ProtoMember(5)]
    public long Id;
    [ProtoMember(10)]
    public string Name;

    string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr() => "VS_WREN";

    protected class Sandbox_Game_Debugging_VSWaypointRenamedMsg\u003C\u003EId\u003C\u003EAccessor : IMemberAccessor<VSWaypointRenamedMsg, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref VSWaypointRenamedMsg owner, in long value) => owner.Id = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref VSWaypointRenamedMsg owner, out long value) => value = owner.Id;
    }

    protected class Sandbox_Game_Debugging_VSWaypointRenamedMsg\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<VSWaypointRenamedMsg, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref VSWaypointRenamedMsg owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref VSWaypointRenamedMsg owner, out string value) => value = owner.Name;
    }

    private class Sandbox_Game_Debugging_VSWaypointRenamedMsg\u003C\u003EActor : IActivator, IActivator<VSWaypointRenamedMsg>
    {
      object IActivator.CreateInstance() => (object) new VSWaypointRenamedMsg();

      VSWaypointRenamedMsg IActivator<VSWaypointRenamedMsg>.CreateInstance() => new VSWaypointRenamedMsg();
    }
  }
}
