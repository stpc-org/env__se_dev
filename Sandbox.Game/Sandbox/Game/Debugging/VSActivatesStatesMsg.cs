// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Debugging.VSActivatesStatesMsg
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
  public struct VSActivatesStatesMsg : MyExternalDebugStructures.IExternalDebugMsg
  {
    [ProtoMember(5)]
    public string SMName;
    [ProtoMember(10)]
    public string[] ActiveStates;

    string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr() => "VS_SETS";

    protected class Sandbox_Game_Debugging_VSActivatesStatesMsg\u003C\u003ESMName\u003C\u003EAccessor : IMemberAccessor<VSActivatesStatesMsg, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref VSActivatesStatesMsg owner, in string value) => owner.SMName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref VSActivatesStatesMsg owner, out string value) => value = owner.SMName;
    }

    protected class Sandbox_Game_Debugging_VSActivatesStatesMsg\u003C\u003EActiveStates\u003C\u003EAccessor : IMemberAccessor<VSActivatesStatesMsg, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref VSActivatesStatesMsg owner, in string[] value) => owner.ActiveStates = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref VSActivatesStatesMsg owner, out string[] value) => value = owner.ActiveStates;
    }

    private class Sandbox_Game_Debugging_VSActivatesStatesMsg\u003C\u003EActor : IActivator, IActivator<VSActivatesStatesMsg>
    {
      object IActivator.CreateInstance() => (object) new VSActivatesStatesMsg();

      VSActivatesStatesMsg IActivator<VSActivatesStatesMsg>.CreateInstance() => new VSActivatesStatesMsg();
    }
  }
}
