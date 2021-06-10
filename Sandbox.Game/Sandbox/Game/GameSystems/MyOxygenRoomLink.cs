// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyOxygenRoomLink
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

namespace Sandbox.Game.GameSystems
{
  public class MyOxygenRoomLink
  {
    public MyOxygenRoom Room { get; set; }

    public MyOxygenRoomLink(MyOxygenRoom room) => this.SetRoom(room);

    private void SetRoom(MyOxygenRoom room)
    {
      this.Room = room;
      this.Room.Link = this;
    }
  }
}
