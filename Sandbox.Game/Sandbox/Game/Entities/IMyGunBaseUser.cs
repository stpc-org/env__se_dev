// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.IMyGunBaseUser
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Entity;

namespace Sandbox.Game.Entities
{
  public interface IMyGunBaseUser
  {
    MyEntity[] IgnoreEntities { get; }

    MyEntity Weapon { get; }

    MyEntity Owner { get; }

    IMyMissileGunObject Launcher { get; }

    MyInventory AmmoInventory { get; }

    MyDefinitionId PhysicalItemId { get; }

    MyInventory WeaponInventory { get; }

    long OwnerId { get; }

    string ConstraintDisplayName { get; }
  }
}
