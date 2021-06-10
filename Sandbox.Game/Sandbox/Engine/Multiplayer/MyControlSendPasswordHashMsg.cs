// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Multiplayer.MyControlSendPasswordHashMsg
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.Serialization;

namespace Sandbox.Engine.Multiplayer
{
  [ProtoContract]
  public struct MyControlSendPasswordHashMsg
  {
    [ProtoMember(1)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public byte[] PasswordHash;

    protected class Sandbox_Engine_Multiplayer_MyControlSendPasswordHashMsg\u003C\u003EPasswordHash\u003C\u003EAccessor : IMemberAccessor<MyControlSendPasswordHashMsg, byte[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyControlSendPasswordHashMsg owner, in byte[] value) => owner.PasswordHash = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyControlSendPasswordHashMsg owner, out byte[] value) => value = owner.PasswordHash;
    }

    private class Sandbox_Engine_Multiplayer_MyControlSendPasswordHashMsg\u003C\u003EActor : IActivator, IActivator<MyControlSendPasswordHashMsg>
    {
      object IActivator.CreateInstance() => (object) new MyControlSendPasswordHashMsg();

      MyControlSendPasswordHashMsg IActivator<MyControlSendPasswordHashMsg>.CreateInstance() => new MyControlSendPasswordHashMsg();
    }
  }
}
