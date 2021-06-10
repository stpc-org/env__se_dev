// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Debugging.VSStatusMsg
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Game;
using VRage.Game.Debugging;
using VRage.Network;

namespace Sandbox.Game.Debugging
{
  [ProtoContract]
  public struct VSStatusMsg : MyExternalDebugStructures.IExternalDebugMsg
  {
    [ProtoMember(5)]
    public string World;
    [ProtoMember(10)]
    public MyObjectBuilder_VisualScriptManagerSessionComponent VSComponent;

    string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr() => "VS_STS";

    protected class Sandbox_Game_Debugging_VSStatusMsg\u003C\u003EWorld\u003C\u003EAccessor : IMemberAccessor<VSStatusMsg, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref VSStatusMsg owner, in string value) => owner.World = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref VSStatusMsg owner, out string value) => value = owner.World;
    }

    protected class Sandbox_Game_Debugging_VSStatusMsg\u003C\u003EVSComponent\u003C\u003EAccessor : IMemberAccessor<VSStatusMsg, MyObjectBuilder_VisualScriptManagerSessionComponent>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref VSStatusMsg owner,
        in MyObjectBuilder_VisualScriptManagerSessionComponent value)
      {
        owner.VSComponent = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref VSStatusMsg owner,
        out MyObjectBuilder_VisualScriptManagerSessionComponent value)
      {
        value = owner.VSComponent;
      }
    }

    private class Sandbox_Game_Debugging_VSStatusMsg\u003C\u003EActor : IActivator, IActivator<VSStatusMsg>
    {
      object IActivator.CreateInstance() => (object) new VSStatusMsg();

      VSStatusMsg IActivator<VSStatusMsg>.CreateInstance() => new VSStatusMsg();
    }
  }
}
