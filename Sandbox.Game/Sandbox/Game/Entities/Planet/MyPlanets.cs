// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Planet.MyPlanets
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.Entities.Planet
{
  [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 500)]
  public class MyPlanets : MySessionComponentBase
  {
    private readonly List<MyPlanet> m_planets = new List<MyPlanet>();
    private readonly List<BoundingBoxD> m_planetAABBsCache = new List<BoundingBoxD>();
    private readonly Dictionary<MyDefinitionId, int> m_knownPlanetTypes = new Dictionary<MyDefinitionId, int>((IEqualityComparer<MyDefinitionId>) MyDefinitionId.Comparer);

    public event Action<MyPlanet> OnPlanetAdded;

    public event Action<MyPlanet> OnPlanetRemoved;

    public static MyPlanets Static { get; private set; }

    public override void LoadData()
    {
      MyPlanets.Static = this;
      base.LoadData();
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      MyPlanets.Static = (MyPlanets) null;
    }

    public static void Register(MyPlanet myPlanet)
    {
      MyPlanets.Static.m_planets.Add(myPlanet);
      MyPlanets.Static.m_planetAABBsCache.Clear();
      MyPlanets.Static.OnPlanetAdded.InvokeIfNotNull<MyPlanet>(myPlanet);
    }

    public static void UnRegister(MyPlanet myPlanet)
    {
      MyPlanets.Static.m_planets.Remove(myPlanet);
      MyPlanets.Static.m_planetAABBsCache.Clear();
      MyPlanets.Static.OnPlanetRemoved.InvokeIfNotNull<MyPlanet>(myPlanet);
      lock (MyPlanets.Static.m_knownPlanetTypes)
      {
        int num1;
        MyPlanets.Static.m_knownPlanetTypes.TryGetValue(myPlanet.Generator.Id, out num1);
        int num2 = num1 - 1;
        if (num2 == 0)
          MyPlanets.Static.m_knownPlanetTypes.Remove(myPlanet.Generator.Id);
        else
          MyPlanets.Static.m_knownPlanetTypes[myPlanet.Generator.Id] = num2;
      }
    }

    public static List<MyPlanet> GetPlanets() => MyPlanets.Static?.m_planets;

    public MyPlanet GetClosestPlanet(Vector3D position)
    {
      List<MyPlanet> planets = this.m_planets;
      return planets.Count == 0 ? (MyPlanet) null : planets.MinBy<MyPlanet>((Func<MyPlanet, float>) (x => (float) ((Vector3D.DistanceSquared(x.PositionComp.GetPosition(), position) - (double) x.AtmosphereRadius * (double) x.AtmosphereRadius) / 1000.0)));
    }

    public ListReader<BoundingBoxD> GetPlanetAABBs()
    {
      if (this.m_planetAABBsCache.Count == 0)
      {
        foreach (MyEntity planet in this.m_planets)
          this.m_planetAABBsCache.Add(planet.PositionComp.WorldAABB);
      }
      return (ListReader<BoundingBoxD>) this.m_planetAABBsCache;
    }

    public bool CanSpawnPlanet(
      MyPlanetGeneratorDefinition planetType,
      bool register,
      out string errorMessage)
    {
      if (!MyFakes.ENABLE_PLANETS)
      {
        errorMessage = MyTexts.GetString(MySpaceTexts.Notification_PlanetsNotSupported);
        return false;
      }
      lock (this.m_knownPlanetTypes)
      {
        if (!this.m_knownPlanetTypes.ContainsKey(planetType.Id) && this.m_knownPlanetTypes.Count + 1 > this.Session.SessionSettings.MaxPlanets)
        {
          errorMessage = string.Format(MyTexts.GetString(MySpaceTexts.Notification_PlanetNotWhitelisted), (object) planetType.Id.SubtypeId.String);
          return false;
        }
        if (register)
        {
          int num1;
          this.m_knownPlanetTypes.TryGetValue(planetType.Id, out num1);
          int num2 = num1 + 1;
          this.m_knownPlanetTypes[planetType.Id] = num2;
        }
      }
      errorMessage = (string) null;
      return true;
    }
  }
}
