// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.ITerminalControlSync
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using VRage.Library.Collections;

namespace Sandbox.Game.Gui
{
  public interface ITerminalControlSync
  {
    void Serialize(BitStream stream, MyTerminalBlock block);
  }
}
