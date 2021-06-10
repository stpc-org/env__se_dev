// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MyGodRaysProperties
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRageMath;

namespace Sandbox.Game.World
{
  internal class MyGodRaysProperties
  {
    public bool Enabled;
    public float Density = 0.34f;
    public float Weight = 1.27f;
    public float Decay = 0.97f;
    public float Exposition = 0.077f;

    public MyGodRaysProperties InterpolateWith(
      MyGodRaysProperties otherProperties,
      float interpolator)
    {
      return new MyGodRaysProperties()
      {
        Density = MathHelper.Lerp(this.Density, otherProperties.Density, interpolator),
        Weight = MathHelper.Lerp(this.Weight, otherProperties.Weight, interpolator),
        Decay = MathHelper.Lerp(this.Decay, otherProperties.Decay, interpolator),
        Exposition = MathHelper.Lerp(this.Exposition, otherProperties.Exposition, interpolator),
        Enabled = (double) MathHelper.Lerp(this.Enabled ? 1f : 0.0f, otherProperties.Enabled ? 1f : 0.0f, interpolator) > 0.5
      };
    }
  }
}
