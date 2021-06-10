// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.MyDebugRenderComponentEnvironmentSector
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Components;
using Sandbox.Game.Entities.Planet;
using VRage.ModAPI;

namespace Sandbox.Game.WorldEnvironment
{
  internal class MyDebugRenderComponentEnvironmentSector : MyDebugRenderComponent
  {
    public override void DebugDraw()
    {
      if (!MyPlanetEnvironmentSessionComponent.DebugDrawSectors)
        return;
      ((MyEnvironmentSector) this.Entity).DebugDraw();
    }

    public MyDebugRenderComponentEnvironmentSector(IMyEntity entity)
      : base(entity)
    {
    }
  }
}
