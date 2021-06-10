// Decompiled with JetBrains decompiler
// Type: VRage.Game.VisualScripting.MyDebuggingNodeLog
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game.VisualScripting
{
  [ProtoContract]
  public struct MyDebuggingNodeLog
  {
    [ProtoMember(5)]
    public int NodeID;
    [ProtoMember(10)]
    public string[] Values;

    protected class VRage_Game_VisualScripting_MyDebuggingNodeLog\u003C\u003ENodeID\u003C\u003EAccessor : IMemberAccessor<MyDebuggingNodeLog, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyDebuggingNodeLog owner, in int value) => owner.NodeID = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyDebuggingNodeLog owner, out int value) => value = owner.NodeID;
    }

    protected class VRage_Game_VisualScripting_MyDebuggingNodeLog\u003C\u003EValues\u003C\u003EAccessor : IMemberAccessor<MyDebuggingNodeLog, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyDebuggingNodeLog owner, in string[] value) => owner.Values = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyDebuggingNodeLog owner, out string[] value) => value = owner.Values;
    }

    private class VRage_Game_VisualScripting_MyDebuggingNodeLog\u003C\u003EActor : IActivator, IActivator<MyDebuggingNodeLog>
    {
      object IActivator.CreateInstance() => (object) new MyDebuggingNodeLog();

      MyDebuggingNodeLog IActivator<MyDebuggingNodeLog>.CreateInstance() => new MyDebuggingNodeLog();
    }
  }
}
