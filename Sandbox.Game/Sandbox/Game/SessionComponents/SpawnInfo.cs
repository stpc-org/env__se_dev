// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.SpawnInfo
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;

namespace Sandbox.Game.SessionComponents
{
  public struct SpawnInfo
  {
    public long IdentityId;
    public MyPlanet Planet;
    public float CollisionRadius;
    public float PlanetDeployAltitude;
    public float MinimalAirDensity;
  }
}
