// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Achievements.MyAchievement_TheHarderTheyFall
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;

namespace SpaceEngineers.Game.Achievements
{
  internal class MyAchievement_TheHarderTheyFall : MySteamAchievementBase
  {
    private const float DESTROY_BLOCK_MASS_KG = 1000000f;
    private float m_massDestroyed;

    protected override (string, string, float) GetAchievementInfo() => (nameof (MyAchievement_TheHarderTheyFall), "TheHarderTheyFall_MassDestroyed", 1000000f);

    public override bool NeedsUpdate => false;

    protected override void LoadStatValue() => this.m_massDestroyed = this.m_remoteAchievement.StatValueFloat;

    public override void SessionBeforeStart()
    {
      if (this.IsAchieved)
        return;
      MyCubeGrids.BlockDestroyed += new Action<MyCubeGrid, MySlimBlock>(this.MyCubeGridsOnBlockDestroyed);
    }

    private void MyCubeGridsOnBlockDestroyed(MyCubeGrid myCubeGrid, MySlimBlock mySlimBlock)
    {
      if (MySession.Static.CreativeMode)
        return;
      this.m_massDestroyed += mySlimBlock.GetMass();
      this.m_remoteAchievement.StatValueFloat = this.m_massDestroyed;
      if ((double) this.m_massDestroyed < 1000000.0)
        return;
      this.NotifyAchieved();
      MyCubeGrids.BlockDestroyed -= new Action<MyCubeGrid, MySlimBlock>(this.MyCubeGridsOnBlockDestroyed);
    }
  }
}
