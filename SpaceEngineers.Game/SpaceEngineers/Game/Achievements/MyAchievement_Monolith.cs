// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Achievements.MyAchievement_Monolith
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Achievements
{
  public class MyAchievement_Monolith : MySteamAchievementBase
  {
    private const uint UPDATE_INTERVAL_S = 3;
    private bool m_globalConditions;
    private uint m_lastTimeUpdatedS;
    private readonly List<MyCubeGrid> m_monolithGrids = new List<MyCubeGrid>();

    protected override (string, string, float) GetAchievementInfo() => (nameof (MyAchievement_Monolith), (string) null, 0.0f);

    public override bool NeedsUpdate => this.m_globalConditions && (uint) MySession.Static.ElapsedPlayTime.TotalSeconds - this.m_lastTimeUpdatedS > 3U;

    public override void SessionUpdate()
    {
      if (this.IsAchieved || MySession.Static.LocalCharacter == null)
        return;
      this.m_lastTimeUpdatedS = (uint) MySession.Static.ElapsedPlayTime.TotalSeconds;
      if (MySession.Static.LocalCharacter == null)
        return;
      Vector3D position = MySession.Static.LocalCharacter.PositionComp.GetPosition();
      foreach (MyCubeGrid monolithGrid in this.m_monolithGrids)
      {
        Vector3D center = monolithGrid.PositionComp.WorldVolume.Center;
        if (Vector3D.DistanceSquared(position, center) < 400.0 + monolithGrid.PositionComp.WorldVolume.Radius)
        {
          this.NotifyAchieved();
          break;
        }
      }
    }

    public override void SessionBeforeStart()
    {
      if (this.IsAchieved)
        return;
      this.m_globalConditions = !MySession.Static.CreativeMode;
      if (!this.m_globalConditions)
        return;
      this.m_lastTimeUpdatedS = 0U;
      this.m_monolithGrids.Clear();
      foreach (MyEntity entity in Sandbox.Game.Entities.MyEntities.GetEntities())
      {
        if (entity is MyCubeGrid myCubeGrid && myCubeGrid.BlocksCount == 1 && myCubeGrid.CubeBlocks.FirstElement<MySlimBlock>().BlockDefinition.Id.SubtypeId == MyStringHash.GetOrCompute("Monolith"))
          this.m_monolithGrids.Add(myCubeGrid);
      }
    }
  }
}
