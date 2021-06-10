// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Multiplayer.MyMessageId
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

namespace Sandbox.Engine.Multiplayer
{
  public enum MyMessageId : byte
  {
    FLUSH = 2,
    RPC = 3,
    REPLICATION_CREATE = 4,
    REPLICATION_DESTROY = 5,
    SERVER_DATA = 6,
    SERVER_STATE_SYNC = 7,
    CLIENT_READY = 8,
    CLIENT_UPDATE = 9,
    REPLICATION_READY = 10, // 0x0A
    REPLICATION_STREAM_BEGIN = 11, // 0x0B
    JOIN_RESULT = 12, // 0x0C
    WORLD_DATA = 13, // 0x0D
    CLIENT_CONNECTED = 14, // 0x0E
    CLIENT_ACKS = 17, // 0x11
    REPLICATION_ISLAND_DONE = 18, // 0x12
    REPLICATION_REQUEST = 19, // 0x13
    WORLD = 20, // 0x14
    PLAYER_DATA = 21, // 0x15
  }
}
