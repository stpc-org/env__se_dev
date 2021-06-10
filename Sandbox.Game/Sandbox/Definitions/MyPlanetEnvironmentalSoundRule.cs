// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyPlanetEnvironmentalSoundRule
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
  public struct MyPlanetEnvironmentalSoundRule
  {
    public SymmetricSerializableRange Latitude;
    public SerializableRange Height;
    public SerializableRange SunAngleFromZenith;
    public MyStringHash EnvironmentSound;

    public bool Check(float angleFromEquator, float height, float sunAngleFromZenith) => this.Latitude.ValueBetween(angleFromEquator) && this.Height.ValueBetween(height) && this.SunAngleFromZenith.ValueBetween(sunAngleFromZenith);
  }
}
