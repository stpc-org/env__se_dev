// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Multiplayer.MyControlMessageCallback`1
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Multiplayer;
using System;
using VRage;
using VRage.Serialization;

namespace Sandbox.Engine.Multiplayer
{
  public class MyControlMessageCallback<TMsg> : IControlMessageCallback<TMsg>, ITransportCallback
    where TMsg : struct
  {
    private readonly ISerializer<TMsg> m_serializer;
    private readonly ControlMessageHandler<TMsg> m_callback;
    public readonly MyMessagePermissions Permission;

    public MyControlMessageCallback(
      ControlMessageHandler<TMsg> callback,
      ISerializer<TMsg> serializer,
      MyMessagePermissions permission)
    {
      this.m_callback = callback;
      this.m_serializer = serializer;
      this.Permission = permission;
    }

    public void Write(ByteStream destination, ref TMsg msg) => this.m_serializer.Serialize(destination, ref msg);

    void ITransportCallback.Receive(ByteStream source, ulong sender)
    {
      if (!MySyncLayer.CheckReceivePermissions(sender, this.Permission))
        return;
      TMsg data;
      try
      {
        this.m_serializer.Deserialize(source, out data);
      }
      catch (Exception ex)
      {
        MySandboxGame.Log.WriteLine(new Exception(string.Format("Error deserializing '{0}', message size '{1}'", (object) typeof (TMsg).Name, (object) source.Length), ex));
        return;
      }
      this.m_callback(ref data, sender);
    }
  }
}
