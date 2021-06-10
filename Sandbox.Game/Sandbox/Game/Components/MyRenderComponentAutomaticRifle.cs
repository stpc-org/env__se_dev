// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyRenderComponentAutomaticRifle
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Weapons;
using VRage.Game;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
  internal class MyRenderComponentAutomaticRifle : MyRenderComponent
  {
    private static readonly MyStringId ID_MUZZLE_FLASH_SIDE = MyStringId.GetOrCompute("MuzzleFlashMachineGunSide");
    private static readonly MyStringId ID_MUZZLE_FLASH_FRONT = MyStringId.GetOrCompute("MuzzleFlashMachineGunFront");
    private MyAutomaticRifleGun m_rifleGun;

    public static void GenerateMuzzleFlash(
      Vector3D position,
      Vector3 dir,
      float radius,
      float length)
    {
      MyRenderComponentAutomaticRifle.GenerateMuzzleFlash(position, dir, uint.MaxValue, ref MatrixD.Zero, radius, length);
    }

    public static void GenerateMuzzleFlash(
      Vector3D position,
      Vector3 dir,
      uint renderObjectID,
      ref MatrixD worldToLocal,
      float radius,
      float length)
    {
      float angle = MyParticlesManager.Paused ? 0.0f : MyUtils.GetRandomFloat(0.0f, 1.570796f);
      float num = 10f;
      Vector4 color = new Vector4(num, num, num, 1f);
      MyTransparentGeometry.AddLineBillboard(MyRenderComponentAutomaticRifle.ID_MUZZLE_FLASH_SIDE, color, position, renderObjectID, ref worldToLocal, dir, length, 0.15f, MyBillboard.BlendTypeEnum.AdditiveBottom);
      MyTransparentGeometry.AddPointBillboard(MyRenderComponentAutomaticRifle.ID_MUZZLE_FLASH_FRONT, color, position, renderObjectID, ref worldToLocal, radius, angle, blendType: MyBillboard.BlendTypeEnum.AdditiveBottom);
    }

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      this.m_rifleGun = this.Container.Entity as MyAutomaticRifleGun;
    }

    public override void Draw()
    {
      int num = MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_rifleGun.LastTimeShoot;
      MyGunBase gunBase = this.m_rifleGun.GunBase;
      if (!gunBase.UseDefaultMuzzleFlash || num > gunBase.MuzzleFlashLifeSpan)
        return;
      MyRenderComponentAutomaticRifle.GenerateMuzzleFlash(gunBase.GetMuzzleWorldPosition(), (Vector3) gunBase.GetMuzzleWorldMatrix().Forward, 0.1f, 0.3f);
    }

    private class Sandbox_Game_Components_MyRenderComponentAutomaticRifle\u003C\u003EActor : IActivator, IActivator<MyRenderComponentAutomaticRifle>
    {
      object IActivator.CreateInstance() => (object) new MyRenderComponentAutomaticRifle();

      MyRenderComponentAutomaticRifle IActivator<MyRenderComponentAutomaticRifle>.CreateInstance() => new MyRenderComponentAutomaticRifle();
    }
  }
}
