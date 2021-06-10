// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.IMyHandToolComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Character;
using VRage.Game.Entity;
using VRage.Game.ModAPI;

namespace Sandbox.Game.Entities
{
  public interface IMyHandToolComponent
  {
    bool Hit(MyEntity entity, MyHitInfo hitInfo, uint shapeKey, float efficiency);

    void Update();

    void DrawHud();

    void OnControlAcquired(MyCharacter owner);

    void OnControlReleased();

    void Shoot();

    string GetStateForTarget(MyEntity targetEntity, uint shapeKey);
  }
}
