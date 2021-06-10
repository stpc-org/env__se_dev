// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.MyWeaponPrediction
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using System;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.Weapons
{
  public static class MyWeaponPrediction
  {
    public static bool GetPredictedTargetPosition(
      MyGunBase gun,
      MyEntity shooter,
      MyEntity target,
      out Vector3 predictedPosition,
      out float timeToHit,
      float shootDelay = 0.0f)
    {
      if (target == null || target.PositionComp == null || (shooter == null || shooter.PositionComp == null))
      {
        predictedPosition = Vector3.Zero;
        timeToHit = 0.0f;
        return false;
      }
      Vector3 center = (Vector3) target.PositionComp.WorldAABB.Center;
      Vector3 muzzleWorldPosition = (Vector3) gun.GetMuzzleWorldPosition();
      Vector3 vector2 = center - muzzleWorldPosition;
      Vector3 vector3_1 = Vector3.Zero;
      if (target.Physics != null)
        vector3_1 = target.Physics.LinearVelocity;
      Vector3 vector3_2 = Vector3.Zero;
      if (shooter.Physics != null)
        vector3_2 = shooter.Physics.LinearVelocity;
      Vector3 vector1 = vector3_1 - vector3_2;
      float projectileSpeed = MyWeaponPrediction.GetProjectileSpeed(gun);
      float num1 = vector1.LengthSquared() - projectileSpeed * projectileSpeed;
      double num2 = 2.0 * (double) Vector3.Dot(vector1, vector2);
      float num3 = vector2.LengthSquared();
      float num4 = (float) (-num2 / (2.0 * (double) num1));
      float num5 = (float) Math.Sqrt(num2 * num2 - 4.0 * (double) num1 * (double) num3) / (2f * num1);
      float num6 = num4 - num5;
      float num7 = num4 + num5;
      float num8 = ((double) num6 <= (double) num7 || (double) num7 <= 0.0 ? num6 : num7) + shootDelay;
      predictedPosition = center + vector1 * num8;
      Vector3 vector3_3 = predictedPosition - muzzleWorldPosition;
      timeToHit = vector3_3.Length() / projectileSpeed;
      return true;
    }

    public static float GetProjectileSpeed(MyGunBase gun)
    {
      if (gun == null)
        return 0.0f;
      float num = 0.0f;
      if (gun.CurrentAmmoMagazineDefinition != null)
        num = MyDefinitionManager.Static.GetAmmoDefinition(gun.CurrentAmmoMagazineDefinition.AmmoDefinitionId).DesiredSpeed;
      return num;
    }
  }
}
