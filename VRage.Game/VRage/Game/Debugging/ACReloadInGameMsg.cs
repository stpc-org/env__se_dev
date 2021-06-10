// Decompiled with JetBrains decompiler
// Type: VRage.Game.Debugging.ACReloadInGameMsg
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game.Debugging
{
  [ProtoContract]
  public struct ACReloadInGameMsg : MyExternalDebugStructures.IExternalDebugMsg
  {
    [ProtoMember(5)]
    public string ACName;
    [ProtoMember(10)]
    public string ACAddress;
    [ProtoMember(15)]
    public string ACContentAddress;

    string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr() => "AC_LOAD";

    protected class VRage_Game_Debugging_ACReloadInGameMsg\u003C\u003EACName\u003C\u003EAccessor : IMemberAccessor<ACReloadInGameMsg, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ACReloadInGameMsg owner, in string value) => owner.ACName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ACReloadInGameMsg owner, out string value) => value = owner.ACName;
    }

    protected class VRage_Game_Debugging_ACReloadInGameMsg\u003C\u003EACAddress\u003C\u003EAccessor : IMemberAccessor<ACReloadInGameMsg, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ACReloadInGameMsg owner, in string value) => owner.ACAddress = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ACReloadInGameMsg owner, out string value) => value = owner.ACAddress;
    }

    protected class VRage_Game_Debugging_ACReloadInGameMsg\u003C\u003EACContentAddress\u003C\u003EAccessor : IMemberAccessor<ACReloadInGameMsg, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ACReloadInGameMsg owner, in string value) => owner.ACContentAddress = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ACReloadInGameMsg owner, out string value) => value = owner.ACContentAddress;
    }

    private class VRage_Game_Debugging_ACReloadInGameMsg\u003C\u003EActor : IActivator, IActivator<ACReloadInGameMsg>
    {
      object IActivator.CreateInstance() => (object) new ACReloadInGameMsg();

      ACReloadInGameMsg IActivator<ACReloadInGameMsg>.CreateInstance() => new ACReloadInGameMsg();
    }
  }
}
