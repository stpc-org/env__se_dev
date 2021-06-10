// Decompiled with JetBrains decompiler
// Type: VRage.Noise.MyRNG
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.Noise
{
  public struct MyRNG
  {
    private const uint MAX_MASK = 2147483647;
    private const float MAX_MASK_FLOAT = 2.147484E+09f;
    public uint Seed;

    public MyRNG(int seed = 1) => this.Seed = (uint) seed;

    public uint NextInt() => this.Gen();

    public float NextFloat() => (float) this.Gen() / (float) int.MaxValue;

    public int NextIntRange(float min, float max) => (int) ((double) min + ((double) max - (double) min) * (double) this.NextFloat() + 0.5);

    public float NextFloatRange(float min, float max) => min + (max - min) * this.NextFloat();

    private uint Gen() => this.Seed = (uint) ((int) this.Seed * 16807 & int.MaxValue);
  }
}
