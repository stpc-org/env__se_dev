// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Multiplayer.PlayerRequestArgs
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.World;

namespace Sandbox.Game.Multiplayer
{
  public class PlayerRequestArgs
  {
    public MyPlayer.PlayerId PlayerId;
    public bool Cancel;

    public PlayerRequestArgs(MyPlayer.PlayerId playerId)
    {
      this.PlayerId = playerId;
      this.Cancel = false;
    }
  }
}
