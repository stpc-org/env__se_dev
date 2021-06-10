// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MyParticleDustProperties
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRageMath;

namespace Sandbox.Game.World
{
  internal class MyParticleDustProperties
  {
    public bool Enabled;
    public float DustBillboardRadius = 3f;
    public float DustFieldCountInDirectionHalf = 5f;
    public float DistanceBetween = 180f;
    public float AnimSpeed = 0.004f;
    public Color Color = Color.White;
    public int Texture;

    public MyParticleDustProperties InterpolateWith(
      MyParticleDustProperties otherProperties,
      float interpolator)
    {
      return new MyParticleDustProperties()
      {
        DustFieldCountInDirectionHalf = MathHelper.Lerp(this.DustFieldCountInDirectionHalf, otherProperties.DustFieldCountInDirectionHalf, interpolator),
        DistanceBetween = MathHelper.Lerp(this.DistanceBetween, otherProperties.DistanceBetween, interpolator),
        AnimSpeed = MathHelper.Lerp(this.AnimSpeed, otherProperties.AnimSpeed, interpolator),
        Color = Color.Lerp(this.Color, otherProperties.Color, interpolator),
        Enabled = (double) MathHelper.Lerp(this.Enabled ? 1f : 0.0f, otherProperties.Enabled ? 1f : 0.0f, interpolator) > 0.5,
        DustBillboardRadius = (double) interpolator <= 0.5 ? this.DustBillboardRadius : otherProperties.DustBillboardRadius,
        Texture = (double) interpolator <= 0.5 ? this.Texture : otherProperties.Texture
      };
    }
  }
}
