// Decompiled with JetBrains decompiler
// Type: VRage.Game.Debugging.MyExternalDebugStructures
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using VRage.ObjectBuilders;
using VRage.Serialization;

namespace VRage.Game.Debugging
{
  public static class MyExternalDebugStructures
  {
    public static readonly int MsgHeaderSize = Marshal.SizeOf(typeof (MyExternalDebugStructures.CommonMsgHeader));

    public static ISerializer<TMsg> GetSerializer<TMsg>() where TMsg : struct => Attribute.IsDefined((MemberInfo) typeof (TMsg), typeof (ProtoContractAttribute)) ? MyExternalDebugStructures.CreateProto<TMsg>() : (ISerializer<TMsg>) null;

    private static ISerializer<TMsg> CreateProto<TMsg>() => (ISerializer<TMsg>) MyExternalDebugStructures.DefaultProtoSerializer<TMsg>.Default;

    public static bool ReadMessageFromPtr<TMessage>(
      ref MyExternalDebugStructures.CommonMsgHeader header,
      byte[] data,
      out TMessage outMsg)
      where TMessage : struct, MyExternalDebugStructures.IExternalDebugMsg
    {
      outMsg = default (TMessage);
      if (header.MsgType != outMsg.GetTypeStr())
        return false;
      MyExternalDebugStructures.GetSerializer<TMessage>().Deserialize(new ByteStream(data, header.MsgSize), out outMsg);
      return true;
    }

    private class DefaultProtoSerializer<T>
    {
      public static readonly ProtoSerializer<T> Default = new ProtoSerializer<T>(MyObjectBuilderSerializer.Serializer);
    }

    public interface IExternalDebugMsg
    {
      string GetTypeStr();
    }

    public struct CommonMsgHeader
    {
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
      public string MsgHeader;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
      public string MsgType;
      [MarshalAs(UnmanagedType.I4)]
      public int MsgSize;

      public static MyExternalDebugStructures.CommonMsgHeader Create(
        string msgType,
        int msgSize = 0)
      {
        return new MyExternalDebugStructures.CommonMsgHeader()
        {
          MsgHeader = "VRAGEMS",
          MsgType = msgType,
          MsgSize = msgSize
        };
      }

      public bool IsValid => this.MsgHeader == "VRAGEMS";
    }
  }
}
