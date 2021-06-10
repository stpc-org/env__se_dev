// Decompiled with JetBrains decompiler
// Type: VRage.Game.VisualScripting.MyDebuggingStateMachine
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game.VisualScripting
{
  [ProtoContract]
  public struct MyDebuggingStateMachine
  {
    [ProtoMember(5)]
    public string SMName;
    [ProtoMember(10)]
    public string[] Cursors;

    protected class VRage_Game_VisualScripting_MyDebuggingStateMachine\u003C\u003ESMName\u003C\u003EAccessor : IMemberAccessor<MyDebuggingStateMachine, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyDebuggingStateMachine owner, in string value) => owner.SMName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyDebuggingStateMachine owner, out string value) => value = owner.SMName;
    }

    protected class VRage_Game_VisualScripting_MyDebuggingStateMachine\u003C\u003ECursors\u003C\u003EAccessor : IMemberAccessor<MyDebuggingStateMachine, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyDebuggingStateMachine owner, in string[] value) => owner.Cursors = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyDebuggingStateMachine owner, out string[] value) => value = owner.Cursors;
    }

    private class VRage_Game_VisualScripting_MyDebuggingStateMachine\u003C\u003EActor : IActivator, IActivator<MyDebuggingStateMachine>
    {
      object IActivator.CreateInstance() => (object) new MyDebuggingStateMachine();

      MyDebuggingStateMachine IActivator<MyDebuggingStateMachine>.CreateInstance() => new MyDebuggingStateMachine();
    }
  }
}
