// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.MySurfaceParams
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRageMath;

namespace Sandbox.Game.WorldEnvironment
{
  public struct MySurfaceParams
  {
    public Vector3 Position;
    public Vector3 Gravity;
    public Vector3 Normal;
    public byte Material;
    public float HeightRatio;
    public float Latitude;
    public float Longitude;
    public byte Biome;
  }
}
