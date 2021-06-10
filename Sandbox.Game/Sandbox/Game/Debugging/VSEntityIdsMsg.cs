// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Debugging.VSEntityIdsMsg
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
  public struct VSEntityIdsMsg : MyExternalDebugStructures.IExternalDebugMsg
  {
    [ProtoMember(5)]
    public SimpleEntityInfo[] Data;

    string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr() => "VS_ENTI";

    protected class Sandbox_Game_Debugging_VSEntityIdsMsg\u003C\u003EData\u003C\u003EAccessor : IMemberAccessor<VSEntityIdsMsg, SimpleEntityInfo[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref VSEntityIdsMsg owner, in SimpleEntityInfo[] value) => owner.Data = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref VSEntityIdsMsg owner, out SimpleEntityInfo[] value) => value = owner.Data;
    }

    private class Sandbox_Game_Debugging_VSEntityIdsMsg\u003C\u003EActor : IActivator, IActivator<VSEntityIdsMsg>
    {
      object IActivator.CreateInstance() => (object) new VSEntityIdsMsg();

      VSEntityIdsMsg IActivator<VSEntityIdsMsg>.CreateInstance() => new VSEntityIdsMsg();
    }
  }
}
