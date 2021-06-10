// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.CubeBlockEffect
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;

namespace Sandbox.Definitions
{
  public struct CubeBlockEffect
  {
    public string Name;
    public string Origin;
    public float Delay;
    public bool Loop;
    public float SpawnTimeMin;
    public float SpawnTimeMax;
    public float Duration;

    public CubeBlockEffect(
      string Name,
      string Origin,
      float Delay,
      bool Loop,
      float SpawnTimeMin,
      float SpawnTimeMax,
      float Duration)
    {
      this.Name = Name;
      this.Origin = Origin;
      this.Delay = Delay;
      this.Loop = Loop;
      this.SpawnTimeMin = SpawnTimeMin;
      this.SpawnTimeMax = SpawnTimeMax;
      this.Duration = Duration;
    }

    public CubeBlockEffect(
      MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect Effect)
    {
      this.Name = Effect.Name;
      this.Origin = Effect.Origin;
      this.Delay = Effect.Delay;
      this.Loop = Effect.Loop;
      this.SpawnTimeMin = Effect.SpawnTimeMin;
      this.SpawnTimeMax = Effect.SpawnTimeMax;
      this.Duration = Effect.Duration;
    }
  }
}
