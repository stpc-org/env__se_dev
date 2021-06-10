// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.CubeBlockEffectBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

namespace Sandbox.Definitions
{
  public class CubeBlockEffectBase
  {
    public string Name;
    public float ParameterMin;
    public float ParameterMax;
    public CubeBlockEffect[] ParticleEffects;
    public CubeBlockEffect[] SoundEffects;

    public CubeBlockEffectBase(string Name, float ParameterMin, float ParameterMax)
    {
      this.Name = Name;
      this.ParameterMin = ParameterMin;
      this.ParameterMax = ParameterMax;
    }
  }
}
