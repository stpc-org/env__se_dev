// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Debugging.VSLoggedNodesMsg
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Game.Debugging;
using VRage.Game.VisualScripting;
using VRage.Network;

namespace Sandbox.Game.Debugging
{
  [ProtoContract]
  public struct VSLoggedNodesMsg : MyExternalDebugStructures.IExternalDebugMsg
  {
    [ProtoMember(5)]
    public int Time;
    [ProtoMember(10)]
    public MyDebuggingNodeLog[] Nodes;
    [ProtoMember(15)]
    public MyDebuggingStateMachine[] StateMachines;

    string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr() => "VS_LGN";

    protected class Sandbox_Game_Debugging_VSLoggedNodesMsg\u003C\u003ETime\u003C\u003EAccessor : IMemberAccessor<VSLoggedNodesMsg, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref VSLoggedNodesMsg owner, in int value) => owner.Time = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref VSLoggedNodesMsg owner, out int value) => value = owner.Time;
    }

    protected class Sandbox_Game_Debugging_VSLoggedNodesMsg\u003C\u003ENodes\u003C\u003EAccessor : IMemberAccessor<VSLoggedNodesMsg, MyDebuggingNodeLog[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref VSLoggedNodesMsg owner, in MyDebuggingNodeLog[] value) => owner.Nodes = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref VSLoggedNodesMsg owner, out MyDebuggingNodeLog[] value) => value = owner.Nodes;
    }

    protected class Sandbox_Game_Debugging_VSLoggedNodesMsg\u003C\u003EStateMachines\u003C\u003EAccessor : IMemberAccessor<VSLoggedNodesMsg, MyDebuggingStateMachine[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref VSLoggedNodesMsg owner, in MyDebuggingStateMachine[] value) => owner.StateMachines = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref VSLoggedNodesMsg owner, out MyDebuggingStateMachine[] value) => value = owner.StateMachines;
    }

    private class Sandbox_Game_Debugging_VSLoggedNodesMsg\u003C\u003EActor : IActivator, IActivator<VSLoggedNodesMsg>
    {
      object IActivator.CreateInstance() => (object) new VSLoggedNodesMsg();

      VSLoggedNodesMsg IActivator<VSLoggedNodesMsg>.CreateInstance() => new VSLoggedNodesMsg();
    }
  }
}
