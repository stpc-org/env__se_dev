// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Achievements.MyAchievement_GiantLeapForMankind
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.ModAPI;
using VRageMath;

namespace SpaceEngineers.Game.Achievements
{
  internal class MyAchievement_GiantLeapForMankind : MySteamAchievementBase
  {
    private const double CHECK_INTERVAL_S = 3.0;
    private const int DISTANCE_TO_BE_WALKED = 1969;
    private float m_metersWalkedOnMoon;
    private readonly List<MyPhysics.HitInfo> m_hits = new List<MyPhysics.HitInfo>();
    private double m_lastCheckS;

    protected override (string, string, float) GetAchievementInfo() => (nameof (MyAchievement_GiantLeapForMankind), "GiantLeapForMankind_WalkedMoon", 1969f);

    public override bool NeedsUpdate => !this.IsAchieved;

    protected override void LoadStatValue() => this.m_metersWalkedOnMoon = this.m_remoteAchievement.StatValueFloat;

    public override void SessionBeforeStart() => this.m_lastCheckS = 0.0;

    public override void SessionUpdate()
    {
      if (MySession.Static?.LocalCharacter?.Physics == null)
        return;
      TimeSpan elapsedPlayTime = MySession.Static.ElapsedPlayTime;
      double num1 = elapsedPlayTime.TotalSeconds - this.m_lastCheckS;
      if (num1 < 3.0)
        return;
      elapsedPlayTime = MySession.Static.ElapsedPlayTime;
      this.m_lastCheckS = elapsedPlayTime.TotalSeconds;
      double num2 = (double) MySession.Static.LocalCharacter.Physics.LinearVelocity.Length();
      if (!MyCharacter.IsWalkingState(MySession.Static.LocalCharacter.GetCurrentMovementState()) && !MyCharacter.IsRunningState(MySession.Static.LocalCharacter.GetCurrentMovementState()) || !this.IsWalkingOnMoon(MySession.Static.LocalCharacter))
        return;
      this.m_metersWalkedOnMoon += (float) (num1 * num2);
      this.m_remoteAchievement.StatValueFloat = this.m_metersWalkedOnMoon;
      if ((double) this.m_metersWalkedOnMoon < 1969.0)
        return;
      this.NotifyAchieved();
    }

    private bool IsWalkingOnMoon(MyCharacter character)
    {
      float groundSearchDistance = MyConstants.DEFAULT_GROUND_SEARCH_DISTANCE;
      Vector3D from = character.PositionComp.GetPosition() + character.PositionComp.WorldMatrixRef.Up * 0.5;
      Vector3D to = from + character.PositionComp.WorldMatrixRef.Down * (double) groundSearchDistance;
      MyPhysics.CastRay(from, to, this.m_hits, 18);
      int index = 0;
      while (index < this.m_hits.Count && ((HkReferenceObject) this.m_hits[index].HkHitInfo.Body == (HkReferenceObject) null || this.m_hits[index].HkHitInfo.GetHitEntity() == character.Components))
        ++index;
      if (this.m_hits.Count == 0)
        return false;
      if (index < this.m_hits.Count)
      {
        MyPhysics.HitInfo hit = this.m_hits[index];
        IMyEntity hitEntity = hit.HkHitInfo.GetHitEntity();
        if (Vector3D.DistanceSquared(hit.Position, from) < (double) groundSearchDistance * (double) groundSearchDistance && hitEntity is MyVoxelBase myVoxelBase && (myVoxelBase.Storage != null && myVoxelBase.Storage.DataProvider != null) && myVoxelBase.Storage.DataProvider is MyPlanetStorageProvider)
        {
          MyPlanetStorageProvider dataProvider = myVoxelBase.Storage.DataProvider as MyPlanetStorageProvider;
          if (dataProvider.Generator != null && dataProvider.Generator.FolderName == "Moon")
          {
            this.m_hits.Clear();
            return true;
          }
        }
      }
      this.m_hits.Clear();
      return false;
    }
  }
}
