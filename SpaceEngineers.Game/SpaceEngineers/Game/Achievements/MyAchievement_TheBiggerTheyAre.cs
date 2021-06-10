// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Achievements.MyAchievement_TheBiggerTheyAre
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
  internal class MyAchievement_TheBiggerTheyAre : MySteamAchievementBase
  {
    private const float BUILT_BLOCK_MASS_KG = 1000000f;
    private int m_massBuilt;

    protected override (string, string, float) GetAchievementInfo() => (nameof (MyAchievement_TheBiggerTheyAre), "TheBiggerTheyAre_MassBuilt", 1000000f);

    public override bool NeedsUpdate => false;

    protected override void LoadStatValue() => this.m_massBuilt = this.m_remoteAchievement.StatValueInt;

    public override void SessionBeforeStart()
    {
      if (this.IsAchieved)
        return;
      MyCubeGrids.BlockBuilt += new Action<MyCubeGrid, MySlimBlock>(this.MyCubeGridsOnBlockBuilt);
    }

    private void MyCubeGridsOnBlockBuilt(MyCubeGrid myCubeGrid, MySlimBlock mySlimBlock)
    {
      if (MySession.Static.CreativeMode)
        return;
      this.m_massBuilt += (int) mySlimBlock.GetMass();
      if ((double) this.m_massBuilt < 1000000.0)
      {
        this.m_remoteAchievement.StatValueInt = this.m_massBuilt;
      }
      else
      {
        this.m_remoteAchievement.StatValueInt = 1000000;
        this.NotifyAchieved();
        MyCubeGrids.BlockBuilt -= new Action<MyCubeGrid, MySlimBlock>(this.MyCubeGridsOnBlockBuilt);
      }
    }
  }
}
