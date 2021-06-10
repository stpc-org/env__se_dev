// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Achievements.MyAchievement_GoingGreen
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using SpaceEngineers.Game.Entities.Blocks;
using System;

namespace SpaceEngineers.Game.Achievements
{
  public class MyAchievement_GoingGreen : MySteamAchievementBase
  {
    private const float EndValue = 25f;
    private int m_solarPanelsBuilt;

    protected override (string, string, float) GetAchievementInfo() => (nameof (MyAchievement_GoingGreen), "GoingGreen_SolarPanelsBuilt", 25f);

    public override bool NeedsUpdate => false;

    protected override void LoadStatValue() => this.m_solarPanelsBuilt = this.m_remoteAchievement.StatValueInt;

    public override void SessionBeforeStart()
    {
      if (this.IsAchieved)
        return;
      MyCubeGrids.BlockBuilt += new Action<MyCubeGrid, MySlimBlock>(this.MyCubeGridsOnBlockBuilt);
    }

    private void MyCubeGridsOnBlockBuilt(MyCubeGrid myCubeGrid, MySlimBlock mySlimBlock)
    {
      if (MySession.Static == null || mySlimBlock == null || (mySlimBlock.FatBlock == null || MySession.Static.CreativeMode) || (mySlimBlock.BuiltBy != MySession.Static.LocalPlayerId || !(mySlimBlock.FatBlock is MySolarPanel)))
        return;
      ++this.m_solarPanelsBuilt;
      this.m_remoteAchievement.StatValueInt = this.m_solarPanelsBuilt;
      if ((double) this.m_solarPanelsBuilt < 25.0)
        return;
      this.NotifyAchieved();
      MyCubeGrids.BlockBuilt -= new Action<MyCubeGrid, MySlimBlock>(this.MyCubeGridsOnBlockBuilt);
    }
  }
}
