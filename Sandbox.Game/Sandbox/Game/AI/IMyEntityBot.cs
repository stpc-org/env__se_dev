// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.IMyEntityBot
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.AI
{
  public interface IMyEntityBot : IMyBot
  {
    MyEntity BotEntity { get; }

    void Spawn(Vector3D? spawnPosition, Vector3? direction, Vector3? up, bool spawnedByPlayer);
  }
}
