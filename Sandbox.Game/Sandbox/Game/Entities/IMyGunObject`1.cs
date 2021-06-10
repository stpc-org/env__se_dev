// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.IMyGunObject`1
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Character;
using Sandbox.Game.Weapons;
using VRage.Game;
using VRage.Game.ModAPI.Interfaces;
using VRageMath;

namespace Sandbox.Game.Entities
{
  public interface IMyGunObject<out T> where T : MyDeviceBase
  {
    float BackkickForcePerSecond { get; }

    float ShakeAmount { get; }

    MyDefinitionId DefinitionId { get; }

    bool EnabledInWorldRules { get; }

    T GunBase { get; }

    bool IsSkinnable { get; }

    Vector3 DirectionToTarget(Vector3D target);

    bool CanShoot(MyShootActionEnum action, long shooter, out MyGunStatusEnum status);

    void Shoot(
      MyShootActionEnum action,
      Vector3 direction,
      Vector3D? overrideWeaponPos,
      string gunAction = null);

    void BeginShoot(MyShootActionEnum action);

    void EndShoot(MyShootActionEnum action);

    bool IsShooting { get; }

    int ShootDirectionUpdateTime { get; }

    bool NeedsShootDirectionWhileAiming { get; }

    float MaximumShotLength { get; }

    void BeginFailReaction(MyShootActionEnum action, MyGunStatusEnum status);

    void BeginFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status);

    void ShootFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status);

    int GetTotalAmmunitionAmount();

    int GetAmmunitionAmount();

    int GetMagazineAmount();

    void OnControlAcquired(MyCharacter owner);

    void OnControlReleased();

    void DrawHud(IMyCameraController camera, long playerId);

    void DrawHud(IMyCameraController camera, long playerId, bool fullUpdate);

    void UpdateSoundEmitter();

    bool SupressShootAnimation();

    Vector3D GetMuzzlePosition();
  }
}
