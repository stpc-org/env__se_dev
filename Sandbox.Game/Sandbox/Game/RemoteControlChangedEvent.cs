// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.RemoteControlChangedEvent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game.VisualScripting;

namespace Sandbox.Game
{
  [VisualScriptingEvent(new bool[] {true, true, true, true, true, true}, null)]
  public delegate void RemoteControlChangedEvent(
    bool GotControlled,
    long playerId,
    string entityName,
    long entityId,
    string gridName,
    long gridId);
}
