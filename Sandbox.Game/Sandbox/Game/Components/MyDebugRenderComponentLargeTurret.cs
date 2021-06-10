// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyDebugRenderComponentLargeTurret
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.EntityComponents;
using Sandbox.Game.Weapons;
using VRage.Game.Components;
using VRage.ModAPI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
  internal class MyDebugRenderComponentLargeTurret : MyDebugRenderComponent
  {
    private MyLargeTurretBase m_turretBase;

    public MyDebugRenderComponentLargeTurret(MyLargeTurretBase turretBase)
      : base((IMyEntity) turretBase)
      => this.m_turretBase = turretBase;

    public override void DebugDraw()
    {
      if (this.m_turretBase.Render.GetModel() != null)
      {
        BoundingSphere boundingSphere = this.m_turretBase.Render.GetModel().BoundingSphere;
      }
      Vector3 vector = new Vector3();
      switch (this.m_turretBase.GetStatus())
      {
        case MyLargeTurretBase.MyLargeShipGunStatus.MyWeaponStatus_Deactivated:
          vector = Color.Green.ToVector3();
          break;
        case MyLargeTurretBase.MyLargeShipGunStatus.MyWeaponStatus_Searching:
          vector = Color.Red.ToVector3();
          break;
        case MyLargeTurretBase.MyLargeShipGunStatus.MyWeaponStatus_Shooting:
          vector = Color.White.ToVector3();
          break;
      }
      Color colorFrom = new Color(vector);
      Color colorTo = new Color(vector);
      if (this.m_turretBase.Target != null)
      {
        MyRenderProxy.DebugDrawLine3D(this.m_turretBase.Barrel.Entity.PositionComp.GetPosition(), this.m_turretBase.Target.PositionComp.GetPosition(), colorFrom, colorTo, false);
        MyRenderProxy.DebugDrawSphere(this.m_turretBase.Target.PositionComp.GetPosition(), this.m_turretBase.Target.PositionComp.LocalVolume.Radius, Color.White, depthRead: false);
      }
      this.m_turretBase.Components.Get<MyResourceSinkComponent>()?.DebugDraw((Matrix) ref this.m_turretBase.PositionComp.WorldMatrixRef);
      base.DebugDraw();
    }
  }
}
