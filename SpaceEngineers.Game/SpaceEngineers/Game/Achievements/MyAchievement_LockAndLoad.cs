// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Achievements.MyAchievement_LockAndLoad
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;
using VRage.Game;
using VRage.Game.Entity;

namespace SpaceEngineers.Game.Achievements
{
  internal class MyAchievement_LockAndLoad : MySteamAchievementBase
  {
    protected override (string, string, float) GetAchievementInfo() => (nameof (MyAchievement_LockAndLoad), (string) null, 0.0f);

    public override bool NeedsUpdate => false;

    public override void SessionBeforeStart()
    {
      if (this.IsAchieved)
        return;
      MyCharacter.OnCharacterDied += new Action<MyCharacter>(this.MyCharacter_OnCharacterDied);
    }

    private void MyCharacter_OnCharacterDied(MyCharacter character)
    {
      MyEntity entity;
      MyEntities.TryGetEntityById(character.StatComp.LastDamage.AttackerId, out entity);
      if (!(entity is MyAutomaticRifleGun automaticRifleGun) || character.GetPlayerIdentityId() == MySession.Static.LocalHumanPlayer.Identity.IdentityId || (!(character.StatComp.LastDamage.Type == MyDamageType.Bullet) || automaticRifleGun.Owner.GetPlayerIdentityId() != MySession.Static.LocalHumanPlayer.Identity.IdentityId))
        return;
      this.NotifyAchieved();
      MyCharacter.OnCharacterDied -= new Action<MyCharacter>(this.MyCharacter_OnCharacterDied);
    }
  }
}
