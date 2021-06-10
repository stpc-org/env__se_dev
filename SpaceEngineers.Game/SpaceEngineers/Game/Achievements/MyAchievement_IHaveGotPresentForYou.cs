// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Achievements.MyAchievement_IHaveGotPresentForYou
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;

namespace SpaceEngineers.Game.Achievements
{
  internal class MyAchievement_IHaveGotPresentForYou : MySteamAchievementBase
  {
    private bool m_someoneIsDead;
    private bool m_imDead;
    private long m_lastAttackerID;
    private List<long> m_warheadList = new List<long>();

    protected override (string, string, float) GetAchievementInfo() => (nameof (MyAchievement_IHaveGotPresentForYou), (string) null, 0.0f);

    public override bool NeedsUpdate => false;

    public override void SessionBeforeStart()
    {
      base.SessionBeforeStart();
      if (this.IsAchieved)
        return;
      MyCharacter.OnCharacterDied += new Action<MyCharacter>(this.MyCharacter_OnCharacterDied);
      MyWarhead.OnCreated += new Action<MyWarhead>(this.MyWarhead_OnCreated);
      MyWarhead.OnDeleted += new Action<MyWarhead>(this.MyWarhead_OnDeleted);
    }

    private void MyWarhead_OnCreated(MyWarhead obj)
    {
      if (obj.BuiltBy != MySession.Static.LocalPlayerId || this.m_warheadList.Contains(obj.CubeGrid.EntityId))
        return;
      this.m_warheadList.Add(obj.CubeGrid.EntityId);
    }

    private void MyWarhead_OnDeleted(MyWarhead obj) => this.m_warheadList.Remove(obj.CubeGrid.EntityId);

    private void MyCharacter_OnCharacterDied(MyCharacter character)
    {
      if (character.StatComp.LastDamage.Type != MyDamageType.Explosion)
        return;
      long attackerId = character.StatComp.LastDamage.AttackerId;
      if (attackerId != this.m_lastAttackerID)
      {
        this.m_someoneIsDead = false;
        this.m_imDead = false;
        this.m_lastAttackerID = attackerId;
      }
      if (character.GetPlayerIdentityId() == MySession.Static.LocalHumanPlayer.Identity.IdentityId)
        this.m_imDead = true;
      else if (character.IsPlayer)
        this.m_someoneIsDead = true;
      if (!this.m_imDead || !this.m_someoneIsDead || (this.m_lastAttackerID != attackerId || !this.m_warheadList.Contains(this.m_lastAttackerID)))
        return;
      this.NotifyAchieved();
      MyCharacter.OnCharacterDied -= new Action<MyCharacter>(this.MyCharacter_OnCharacterDied);
      MyWarhead.OnCreated -= new Action<MyWarhead>(this.MyWarhead_OnCreated);
      MyWarhead.OnDeleted -= new Action<MyWarhead>(this.MyWarhead_OnDeleted);
    }
  }
}
