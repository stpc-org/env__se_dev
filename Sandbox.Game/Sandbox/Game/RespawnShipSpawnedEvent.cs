// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.RespawnShipSpawnedEvent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game.VisualScripting;

namespace Sandbox.Game
{
  [VisualScriptingEvent(new bool[] {true, false}, null)]
  public delegate void RespawnShipSpawnedEvent(
    long shipEntityId,
    long playerId,
    string respawnShipPrefabName);
}
