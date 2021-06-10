// Decompiled with JetBrains decompiler
// Type: VRage.Game.Debugging.ACConnectToEditorMsg
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game.Debugging
{
  [ProtoContract]
  public struct ACConnectToEditorMsg : MyExternalDebugStructures.IExternalDebugMsg
  {
    [ProtoMember(5)]
    public string ACName;

    string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr() => "AC_CON";

    protected class VRage_Game_Debugging_ACConnectToEditorMsg\u003C\u003EACName\u003C\u003EAccessor : IMemberAccessor<ACConnectToEditorMsg, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ACConnectToEditorMsg owner, in string value) => owner.ACName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ACConnectToEditorMsg owner, out string value) => value = owner.ACName;
    }

    private class VRage_Game_Debugging_ACConnectToEditorMsg\u003C\u003EActor : IActivator, IActivator<ACConnectToEditorMsg>
    {
      object IActivator.CreateInstance() => (object) new ACConnectToEditorMsg();

      ACConnectToEditorMsg IActivator<ACConnectToEditorMsg>.CreateInstance() => new ACConnectToEditorMsg();
    }
  }
}
