// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.IMyMissileGunObject
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Weapons;

namespace Sandbox.Game.Entities
{
  public interface IMyMissileGunObject : IMyGunObject<MyGunBase>
  {
    void MissileShootEffect();

    void ShootMissile(MyObjectBuilder_Missile builder);

    void RemoveMissile(long entityId);
  }
}
