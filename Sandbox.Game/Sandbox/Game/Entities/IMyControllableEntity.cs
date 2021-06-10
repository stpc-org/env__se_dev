// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.IMyControllableEntity
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Utils;

namespace Sandbox.Game.Entities
{
  public interface IMyControllableEntity : VRage.Game.ModAPI.Interfaces.IMyControllableEntity
  {
    MyControllerInfo ControllerInfo { get; }

    MyEntity Entity { get; }

    float HeadLocalXAngle { get; set; }

    float HeadLocalYAngle { get; set; }

    void BeginShoot(MyShootActionEnum action);

    void EndShoot(MyShootActionEnum action);

    bool ShouldEndShootingOnPause(MyShootActionEnum action);

    void OnBeginShoot(MyShootActionEnum action);

    void OnEndShoot(MyShootActionEnum action);

    void UseFinished();

    void PickUpFinished();

    void Sprint(bool enabled);

    void SwitchToWeapon(MyDefinitionId weaponDefinition);

    void SwitchToWeapon(MyToolbarItemWeapon weapon);

    bool CanSwitchToWeapon(MyDefinitionId? weaponDefinition);

    void SwitchAmmoMagazine();

    bool CanSwitchAmmoMagazine();

    void SwitchBroadcasting();

    bool EnabledBroadcasting { get; }

    MyToolbarType ToolbarType { get; }

    MyEntityCameraSettings GetCameraEntitySettings();

    MyStringId ControlContext { get; }

    MyStringId AuxiliaryContext { get; }

    MyToolbar Toolbar { get; }

    MyEntity RelativeDampeningEntity { get; set; }
  }
}
