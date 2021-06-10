// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.EntityComponents.Renders.MyRenderComponentLargeTurret
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Game.Components;
using Sandbox.Game.Weapons;

namespace SpaceEngineers.Game.EntityComponents.Renders
{
  internal class MyRenderComponentLargeTurret : MyRenderComponentCubeBlock
  {
    private MyLargeTurretBase m_turretBase;

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      this.m_turretBase = this.Container.Entity as MyLargeTurretBase;
    }

    public override void Draw()
    {
      if (!this.m_turretBase.IsWorking)
        return;
      base.Draw();
      if (this.m_turretBase.Barrel == null)
        return;
      this.m_turretBase.Barrel.Draw();
    }
  }
}
