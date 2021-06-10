// Decompiled with JetBrains decompiler
// Type: VRage.Game.Extensions.MyObjectBuilderExtensions
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.ObjectBuilders;

namespace VRage.Game.Extensions
{
  public static class MyObjectBuilderExtensions
  {
    public static bool HasPlanets(this MyObjectBuilder_ScenarioDefinition scenario)
    {
      if (scenario.WorldGeneratorOperations != null)
      {
        foreach (MyObjectBuilder_WorldGeneratorOperation generatorOperation in scenario.WorldGeneratorOperations)
        {
          switch (generatorOperation)
          {
            case MyObjectBuilder_WorldGeneratorOperation_CreatePlanet _:
              return true;
            case MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab _:
              return true;
            default:
              continue;
          }
        }
      }
      return false;
    }

    public static bool HasPlanets(this MyObjectBuilder_Sector sector)
    {
      if (sector.SectorObjects != null)
      {
        foreach (MyObjectBuilder_EntityBase sectorObject in sector.SectorObjects)
        {
          if (sectorObject is MyObjectBuilder_Planet)
            return true;
        }
      }
      return false;
    }
  }
}
