// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyWeatherEffectDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;
using VRageRender;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_WeatherEffectDefinition), null)]
  public class MyWeatherEffectDefinition : MyDefinitionBase
  {
    public MyFogProperties FogProperties = MyFogProperties.Default;
    public string AmbientSound = "";
    public float AmbientVolume;
    public string EffectName = "";
    public float ParticleRadius;
    public int ParticleCount;
    public float ParticleScale = 1f;
    public float LightningIntervalMin;
    public float LightningIntervalMax;
    public float LightningCharacterHitIntervalMin;
    public float LightningCharacterHitIntervalMax;
    public float LightningGridHitIntervalMin;
    public float LightningGridHitIntervalMax;
    public float ParticleAlphaMultiplier = 1f;
    public Vector3 SunColor = MyEnvironmentLightData.Default.SunColor;
    public Vector3 SunSpecularColor = MyEnvironmentLightData.Default.SunSpecularColor;
    public float SunIntensity = MySunProperties.Default.SunIntensity;
    public float ShadowFadeout = MyEnvironmentLightData.Default.ShadowFadeoutMultiplier;
    public float WindOutputModifier = 1f;
    public float SolarOutputModifier = 1f;
    public float TemperatureModifier = 1f;
    public float OxygenLevelModifier = 1f;
    public float FoliageWindModifier = 1f;
    public MyObjectBuilder_WeatherLightning Lightning;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_WeatherEffectDefinition effectDefinition = builder as MyObjectBuilder_WeatherEffectDefinition;
      this.FogProperties.FogColor = effectDefinition.FogColor;
      this.FogProperties.FogDensity = effectDefinition.FogDensity;
      this.FogProperties.FogMultiplier = effectDefinition.FogMultiplier;
      this.FogProperties.FogSkybox = effectDefinition.FogSkyboxMultiplier;
      this.FogProperties.FogAtmo = effectDefinition.FogAtmoMultiplier;
      this.AmbientSound = effectDefinition.AmbientSound;
      this.AmbientVolume = effectDefinition.AmbientVolume;
      this.EffectName = effectDefinition.EffectName;
      this.ParticleRadius = effectDefinition.ParticleRadius;
      this.ParticleCount = effectDefinition.ParticleCount;
      this.ParticleScale = effectDefinition.ParticleScale;
      this.LightningIntervalMin = effectDefinition.LightningIntervalMin;
      this.LightningIntervalMax = effectDefinition.LightningIntervalMax;
      this.LightningCharacterHitIntervalMin = effectDefinition.LightningCharacterHitIntervalMin;
      this.LightningCharacterHitIntervalMax = effectDefinition.LightningCharacterHitIntervalMax;
      this.LightningGridHitIntervalMin = effectDefinition.LightningGridHitIntervalMin;
      this.LightningGridHitIntervalMax = effectDefinition.LightningGridHitIntervalMax;
      this.ParticleAlphaMultiplier = effectDefinition.ParticleAlphaMultiplier;
      this.SunColor = effectDefinition.SunColor;
      this.SunSpecularColor = effectDefinition.SunSpecularColor;
      this.SunIntensity = effectDefinition.SunIntensity;
      this.ShadowFadeout = effectDefinition.ShadowFadeout;
      this.WindOutputModifier = effectDefinition.WindOutputModifier;
      this.SolarOutputModifier = effectDefinition.SolarOutputModifier;
      this.TemperatureModifier = effectDefinition.TemperatureModifier;
      this.OxygenLevelModifier = effectDefinition.OxygenLevelModifier;
      this.FoliageWindModifier = effectDefinition.FoliageWindModifier;
      this.Lightning = effectDefinition.Lightning;
    }

    public void Lerp(
      MyWeatherEffectDefinition targetWeather,
      MyWeatherEffectDefinition outputWeather,
      float ratio)
    {
      outputWeather.FogProperties.FogColor = Vector3.Lerp(this.FogProperties.FogColor, targetWeather.FogProperties.FogColor, ratio);
      outputWeather.FogProperties.FogDensity = MathHelper.Lerp(this.FogProperties.FogDensity, targetWeather.FogProperties.FogDensity, ratio);
      outputWeather.FogProperties.FogMultiplier = MathHelper.Lerp(this.FogProperties.FogMultiplier, targetWeather.FogProperties.FogMultiplier, ratio);
      outputWeather.FogProperties.FogSkybox = MathHelper.Lerp(this.FogProperties.FogSkybox, targetWeather.FogProperties.FogSkybox, ratio);
      outputWeather.FogProperties.FogAtmo = MathHelper.Lerp(this.FogProperties.FogAtmo, targetWeather.FogProperties.FogAtmo, ratio);
      outputWeather.AmbientSound = (double) ratio < 0.5 ? this.AmbientSound : targetWeather.AmbientSound;
      outputWeather.AmbientVolume = MathHelper.Lerp(this.AmbientVolume, targetWeather.AmbientVolume, ratio);
      outputWeather.EffectName = (double) ratio < 0.5 ? this.EffectName : targetWeather.EffectName;
      outputWeather.ParticleRadius = MathHelper.Lerp(this.ParticleRadius, targetWeather.ParticleRadius, ratio);
      outputWeather.ParticleCount = MathHelper.RoundToInt(MathHelper.Lerp((float) this.ParticleCount, (float) targetWeather.ParticleCount, ratio));
      outputWeather.ParticleScale = MathHelper.Lerp(this.ParticleScale, targetWeather.ParticleScale, ratio);
      outputWeather.ParticleAlphaMultiplier = MathHelper.Lerp(this.ParticleAlphaMultiplier, targetWeather.ParticleAlphaMultiplier, ratio);
      outputWeather.SunColor = Vector3.Lerp(this.SunColor, targetWeather.SunColor, ratio);
      outputWeather.SunSpecularColor = Vector3.Lerp(this.SunSpecularColor, targetWeather.SunSpecularColor, ratio);
      outputWeather.SunIntensity = MathHelper.Lerp(this.SunIntensity, targetWeather.SunIntensity, MathHelper.Clamp(ratio * 2f, 0.0f, 1f));
      outputWeather.ShadowFadeout = MathHelper.Lerp(this.ShadowFadeout, targetWeather.ShadowFadeout, ratio);
      outputWeather.WindOutputModifier = MathHelper.Lerp(this.WindOutputModifier, targetWeather.WindOutputModifier, ratio);
      outputWeather.SolarOutputModifier = MathHelper.Lerp(this.SolarOutputModifier, targetWeather.SolarOutputModifier, ratio);
      outputWeather.TemperatureModifier = MathHelper.Lerp(this.TemperatureModifier, targetWeather.TemperatureModifier, ratio);
      outputWeather.OxygenLevelModifier = MathHelper.Lerp(this.OxygenLevelModifier, targetWeather.OxygenLevelModifier, ratio);
      outputWeather.FoliageWindModifier = MathHelper.Lerp(this.FoliageWindModifier, targetWeather.FoliageWindModifier, ratio);
      outputWeather.LightningIntervalMin = MathHelper.Lerp(this.LightningIntervalMin, targetWeather.LightningIntervalMin, ratio);
      outputWeather.LightningIntervalMax = MathHelper.Lerp(this.LightningIntervalMax, targetWeather.LightningIntervalMax, ratio);
      outputWeather.LightningCharacterHitIntervalMin = MathHelper.Lerp(this.LightningCharacterHitIntervalMin, targetWeather.LightningCharacterHitIntervalMin, ratio);
      outputWeather.LightningCharacterHitIntervalMax = MathHelper.Lerp(this.LightningCharacterHitIntervalMax, targetWeather.LightningCharacterHitIntervalMax, ratio);
      outputWeather.LightningGridHitIntervalMin = MathHelper.Lerp(this.LightningGridHitIntervalMin, targetWeather.LightningGridHitIntervalMin, ratio);
      outputWeather.LightningGridHitIntervalMax = MathHelper.Lerp(this.LightningGridHitIntervalMax, targetWeather.LightningGridHitIntervalMax, ratio);
    }

    private class Sandbox_Definitions_MyWeatherEffectDefinition\u003C\u003EActor : IActivator, IActivator<MyWeatherEffectDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyWeatherEffectDefinition();

      MyWeatherEffectDefinition IActivator<MyWeatherEffectDefinition>.CreateInstance() => new MyWeatherEffectDefinition();
    }
  }
}
