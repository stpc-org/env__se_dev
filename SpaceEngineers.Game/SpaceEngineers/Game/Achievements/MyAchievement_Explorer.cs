// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Achievements.MyAchievement_Explorer
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections;
using System.Collections.Generic;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Achievements
{
  public class MyAchievement_Explorer : MySteamAchievementBase
  {
    private const uint CHECK_INTERVAL_S = 3;
    private const uint PLANET_COUNT = 6;
    private BitArray m_exploredPlanetData;
    private readonly int[] m_bitArrayConversionArray = new int[1];
    private int m_planetsDiscovered;
    private uint m_lastCheckS;
    private readonly Dictionary<MyStringHash, int> m_planetNamesToIndexes = new Dictionary<MyStringHash, int>();
    private bool m_globalConditionsMet;

    protected override (string, string, float) GetAchievementInfo() => (nameof (MyAchievement_Explorer), "Explorer_ExplorePlanetsData", 6f);

    public override bool NeedsUpdate => this.m_globalConditionsMet;

    public override void Init()
    {
      base.Init();
      if (this.IsAchieved)
        return;
      this.m_planetNamesToIndexes.Add(MyStringHash.GetOrCompute("Alien"), 0);
      this.m_planetNamesToIndexes.Add(MyStringHash.GetOrCompute("EarthLike"), 1);
      this.m_planetNamesToIndexes.Add(MyStringHash.GetOrCompute("Europa"), 2);
      this.m_planetNamesToIndexes.Add(MyStringHash.GetOrCompute("Mars"), 3);
      this.m_planetNamesToIndexes.Add(MyStringHash.GetOrCompute("Moon"), 4);
      this.m_planetNamesToIndexes.Add(MyStringHash.GetOrCompute("Titan"), 5);
    }

    public override void SessionLoad()
    {
      this.m_globalConditionsMet = !MySession.Static.CreativeMode;
      this.m_lastCheckS = 0U;
    }

    public override void SessionUpdate()
    {
      if (MySession.Static.LocalCharacter == null || this.IsAchieved)
        return;
      uint totalSeconds = (uint) MySession.Static.ElapsedPlayTime.TotalSeconds;
      if (totalSeconds - this.m_lastCheckS <= 3U || MySession.Static.LocalCharacter == null)
        return;
      Vector3D position = MySession.Static.LocalCharacter.PositionComp.GetPosition();
      Vector3 naturalGravityInPoint = MyGravityProviderSystem.CalculateNaturalGravityInPoint(position);
      this.m_lastCheckS = totalSeconds;
      Vector3 zero = Vector3.Zero;
      if (naturalGravityInPoint == zero)
        return;
      MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(position);
      int index1;
      if (closestPlanet == null || !this.m_planetNamesToIndexes.TryGetValue(closestPlanet.Generator.Id.SubtypeId, out index1) || this.m_exploredPlanetData[index1])
        return;
      this.m_exploredPlanetData[index1] = true;
      this.m_planetsDiscovered = 0;
      for (int index2 = 0; index2 < 6; ++index2)
      {
        if (this.m_exploredPlanetData[index2])
          ++this.m_planetsDiscovered;
      }
      this.StoreSteamData();
      if (this.m_planetsDiscovered < 6)
        this.m_remoteAchievement.IndicateProgress((uint) this.m_planetsDiscovered);
      else
        this.NotifyAchieved();
    }

    protected override void LoadStatValue() => this.m_exploredPlanetData = new BitArray(new int[1]
    {
      this.m_remoteAchievement.StatValueConditionBitField
    });

    private void StoreSteamData()
    {
      this.m_exploredPlanetData.CopyTo((Array) this.m_bitArrayConversionArray, 0);
      this.m_remoteAchievement.StatValueConditionBitField = this.m_bitArrayConversionArray[0];
    }
  }
}
