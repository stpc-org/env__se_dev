// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Debugging.VSTriggersMsg
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Game.Debugging;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;

namespace Sandbox.Game.Debugging
{
  [ProtoContract]
  public struct VSTriggersMsg : MyExternalDebugStructures.IExternalDebugMsg
  {
    [ProtoMember(5)]
    public MyObjectBuilder_AreaTrigger[] Triggers;
    [ProtoMember(10)]
    public long[] Parents;

    string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr() => "VS_TRGS";

    protected class Sandbox_Game_Debugging_VSTriggersMsg\u003C\u003ETriggers\u003C\u003EAccessor : IMemberAccessor<VSTriggersMsg, MyObjectBuilder_AreaTrigger[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref VSTriggersMsg owner, in MyObjectBuilder_AreaTrigger[] value) => owner.Triggers = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref VSTriggersMsg owner, out MyObjectBuilder_AreaTrigger[] value) => value = owner.Triggers;
    }

    protected class Sandbox_Game_Debugging_VSTriggersMsg\u003C\u003EParents\u003C\u003EAccessor : IMemberAccessor<VSTriggersMsg, long[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref VSTriggersMsg owner, in long[] value) => owner.Parents = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref VSTriggersMsg owner, out long[] value) => value = owner.Parents;
    }

    private class Sandbox_Game_Debugging_VSTriggersMsg\u003C\u003EActor : IActivator, IActivator<VSTriggersMsg>
    {
      object IActivator.CreateInstance() => (object) new VSTriggersMsg();

      VSTriggersMsg IActivator<VSTriggersMsg>.CreateInstance() => new VSTriggersMsg();
    }
  }
}
