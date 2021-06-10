// Decompiled with JetBrains decompiler
// Type: VRage.Game.Debugging.ACSendStateToEditorMsg
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game.Debugging
{
  [ProtoContract]
  public struct ACSendStateToEditorMsg : MyExternalDebugStructures.IExternalDebugMsg
  {
    [ProtoMember(5)]
    public string CurrentNodeAddress;
    [ProtoMember(10)]
    public int[] VisitedTreeNodesPath;

    public static ACSendStateToEditorMsg Create(
      string currentNodeAddress,
      int[] visitedTreeNodesPath)
    {
      ACSendStateToEditorMsg stateToEditorMsg = new ACSendStateToEditorMsg()
      {
        CurrentNodeAddress = currentNodeAddress,
        VisitedTreeNodesPath = new int[64]
      };
      if (visitedTreeNodesPath != null)
        Array.Copy((Array) visitedTreeNodesPath, (Array) stateToEditorMsg.VisitedTreeNodesPath, Math.Min(visitedTreeNodesPath.Length, 64));
      return stateToEditorMsg;
    }

    string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr() => "AC_STA";

    protected class VRage_Game_Debugging_ACSendStateToEditorMsg\u003C\u003ECurrentNodeAddress\u003C\u003EAccessor : IMemberAccessor<ACSendStateToEditorMsg, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ACSendStateToEditorMsg owner, in string value) => owner.CurrentNodeAddress = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ACSendStateToEditorMsg owner, out string value) => value = owner.CurrentNodeAddress;
    }

    protected class VRage_Game_Debugging_ACSendStateToEditorMsg\u003C\u003EVisitedTreeNodesPath\u003C\u003EAccessor : IMemberAccessor<ACSendStateToEditorMsg, int[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ACSendStateToEditorMsg owner, in int[] value) => owner.VisitedTreeNodesPath = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ACSendStateToEditorMsg owner, out int[] value) => value = owner.VisitedTreeNodesPath;
    }

    private class VRage_Game_Debugging_ACSendStateToEditorMsg\u003C\u003EActor : IActivator, IActivator<ACSendStateToEditorMsg>
    {
      object IActivator.CreateInstance() => (object) new ACSendStateToEditorMsg();

      ACSendStateToEditorMsg IActivator<ACSendStateToEditorMsg>.CreateInstance() => new ACSendStateToEditorMsg();
    }
  }
}
