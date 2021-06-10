// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Debugging.SelectedTreeMsg
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
  public struct SelectedTreeMsg : MyExternalDebugStructures.IExternalDebugMsg
  {
    [ProtoMember(5)]
    public string BehaviorTreeName;

    string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr() => "SELTREE";

    protected class Sandbox_Game_Debugging_SelectedTreeMsg\u003C\u003EBehaviorTreeName\u003C\u003EAccessor : IMemberAccessor<SelectedTreeMsg, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SelectedTreeMsg owner, in string value) => owner.BehaviorTreeName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SelectedTreeMsg owner, out string value) => value = owner.BehaviorTreeName;
    }

    private class Sandbox_Game_Debugging_SelectedTreeMsg\u003C\u003EActor : IActivator, IActivator<SelectedTreeMsg>
    {
      object IActivator.CreateInstance() => (object) new SelectedTreeMsg();

      SelectedTreeMsg IActivator<SelectedTreeMsg>.CreateInstance() => new SelectedTreeMsg();
    }
  }
}
