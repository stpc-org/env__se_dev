// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.IMyHandheldGunObject`1
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Weapons;
using VRage.Game;

namespace Sandbox.Game.Entities
{
  public interface IMyHandheldGunObject<out T> : IMyGunObject<T> where T : MyDeviceBase
  {
    MyObjectBuilder_PhysicalGunObject PhysicalObject { get; }

    MyPhysicalItemDefinition PhysicalItemDefinition { get; }

    bool ForceAnimationInsteadOfIK { get; }

    bool IsBlocking { get; }

    int CurrentAmmunition { set; get; }

    int CurrentMagazineAmmunition { set; get; }

    int CurrentMagazineAmount { set; get; }

    long OwnerId { get; }

    long OwnerIdentityId { get; }

    bool Reloadable { get; }

    bool IsReloading { get; }

    bool NeedsReload { get; }

    bool CanReload();

    bool Reload();

    float GetReloadDuration();

    bool CanDoubleClickToStick(MyShootActionEnum action);

    bool ShouldEndShootOnPause(MyShootActionEnum action);

    void DoubleClicked(MyShootActionEnum action);

    bool IsRecoiling { get; }

    void PlayReloadSound();

    bool GetShakeOnAction(MyShootActionEnum action);
  }
}
