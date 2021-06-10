// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.IMyWeatherEffects
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using VRageMath;

namespace VRage.Game.ModAPI
{
  public interface IMyWeatherEffects
  {
    string GetWeather(Vector3D position);

    bool GetWeather(Vector3D position, out MyObjectBuilder_WeatherEffect weatherEffect);

    bool SetWeather(
      string weatherEffect,
      float radius,
      Vector3D? weatherPosition,
      bool verbose,
      Vector3D velocity,
      int length = 0,
      float intensity = 1f);

    bool RemoveWeather(Vector3D position);

    void RemoveWeather(MyObjectBuilder_WeatherEffect weatherEffect);

    void CreateLightning(
      Vector3D position,
      MyObjectBuilder_WeatherLightning lightning,
      bool doDamage = true);

    float GetWeatherIntensity(Vector3D position);

    float GetWeatherIntensity(Vector3D position, MyObjectBuilder_WeatherEffect weatherEffect);

    float GetOxygenMultiplier(Vector3D position);

    float GetOxygenMultiplier(Vector3D position, MyObjectBuilder_WeatherEffect weatherEffect);

    float GetSolarMultiplier(Vector3D position);

    float GetSolarMultiplier(Vector3D position, MyObjectBuilder_WeatherEffect weatherEffect);

    float GetTemperatureMultiplier(Vector3D position);

    float GetTemperatureMultiplier(Vector3D position, MyObjectBuilder_WeatherEffect weatherEffect);

    float GetWindMultiplier(Vector3D position);

    float GetWindMultiplier(Vector3D position, MyObjectBuilder_WeatherEffect weatherEffect);

    float? FogMultiplierOverride { get; set; }

    float? FogDensityOverride { get; set; }

    Vector3? FogColorOverride { get; set; }

    float? FogSkyboxOverride { get; set; }

    float? FogAtmoOverride { get; set; }

    MatrixD? ParticleDirectionOverride { get; set; }

    Vector3? ParticleVelocityOverride { get; set; }

    float? SunIntensityOverride { get; set; }

    List<MyObjectBuilder_WeatherPlanetData> GetWeatherPlanetData();
  }
}
