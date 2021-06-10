// Decompiled with JetBrains decompiler
// Type: VRageRender.MyRenderSettings1
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRageRender
{
  public struct MyRenderSettings1 : IEquatable<MyRenderSettings1>
  {
    public bool HqTarget;
    public MyAntialiasingMode AntialiasingMode;
    public bool AmbientOcclusionEnabled;
    public MyShadowsQuality ShadowQuality;
    public MyRenderQualityEnum ShadowGPUQuality;
    public MyTextureQuality TextureQuality;
    public MyTextureQuality VoxelTextureQuality;
    public MyTextureAnisoFiltering AnisotropicFiltering;
    public MyRenderQualityEnum ModelQuality;
    public MyRenderQualityEnum VoxelQuality;
    public bool HqDepth;
    public MyRenderQualityEnum VoxelShaderQuality;
    public MyRenderQualityEnum AlphaMaskedShaderQuality;
    public MyRenderQualityEnum AtmosphereShaderQuality;
    public float GrassDrawDistance;
    public float GrassDensityFactor;
    public float DistanceFade;
    public MyRenderQualityEnum ParticleQuality;

    public override int GetHashCode() => base.GetHashCode();

    bool IEquatable<MyRenderSettings1>.Equals(MyRenderSettings1 other) => this.Equals(ref other);

    public override bool Equals(object other)
    {
      MyRenderSettings1 other1 = (MyRenderSettings1) other;
      return this.Equals(ref other1);
    }

    public bool Equals(ref MyRenderSettings1 other) => this.GrassDensityFactor.IsEqual(other.GrassDensityFactor, 0.1f) && this.GrassDrawDistance.IsEqual(other.GrassDrawDistance, 2f) && (this.ModelQuality == other.ModelQuality && this.VoxelQuality == other.VoxelQuality) && (this.AntialiasingMode == other.AntialiasingMode && this.ShadowQuality == other.ShadowQuality && (this.ShadowGPUQuality == other.ShadowGPUQuality && this.AmbientOcclusionEnabled == other.AmbientOcclusionEnabled)) && (this.TextureQuality == other.TextureQuality && this.VoxelTextureQuality == other.VoxelTextureQuality && (this.AnisotropicFiltering == other.AnisotropicFiltering && this.HqDepth == other.HqDepth) && (this.VoxelShaderQuality == other.VoxelShaderQuality && this.AlphaMaskedShaderQuality == other.AlphaMaskedShaderQuality && (this.AtmosphereShaderQuality == other.AtmosphereShaderQuality && this.DistanceFade.IsEqual(other.DistanceFade, 4f)))) && this.ParticleQuality == other.ParticleQuality;
  }
}
