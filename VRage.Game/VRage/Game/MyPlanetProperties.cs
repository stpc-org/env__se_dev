// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyPlanetProperties
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

namespace VRage.Game
{
  public struct MyPlanetProperties
  {
    [StructDefault]
    public static readonly MyPlanetProperties Default = new MyPlanetProperties()
    {
      AtmosphereIntensityMultiplier = 35f,
      AtmosphereIntensityAmbientMultiplier = 35f,
      AtmosphereDesaturationFactorForward = 0.5f,
      CloudsIntensityMultiplier = 40f
    };
    public float AtmosphereIntensityMultiplier;
    public float AtmosphereIntensityAmbientMultiplier;
    public float AtmosphereDesaturationFactorForward;
    public float CloudsIntensityMultiplier;

    private static class Defaults
    {
      public const float AtmosphereIntensityMultiplier = 35f;
      public const float AtmosphereIntensityAmbientMultiplier = 35f;
      public const float AtmosphereDesaturationFactorForward = 0.5f;
      public const float CloudsIntensityMultiplier = 40f;
    }
  }
}
