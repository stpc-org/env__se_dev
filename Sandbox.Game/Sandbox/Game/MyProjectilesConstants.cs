// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.MyProjectilesConstants
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game
{
  public static class MyProjectilesConstants
  {
    public const int MAX_PROJECTILES_COUNT = 8192;
    public const float HIT_STRENGTH_IMPULSE = 500f;
    private static readonly Dictionary<int, Vector3> m_projectileTrailColors = new Dictionary<int, Vector3>(10);
    public static readonly float AUTOAIMING_PRECISION = 500f;

    static MyProjectilesConstants()
    {
      MyProjectilesConstants.m_projectileTrailColors.Add(-1, Vector3.One);
      MyProjectilesConstants.m_projectileTrailColors.Add(1, Vector3.One);
      MyProjectilesConstants.m_projectileTrailColors.Add(0, new Vector3(10f, 10f, 10f));
      MyProjectilesConstants.m_projectileTrailColors.Add(3, Vector3.One);
      MyProjectilesConstants.m_projectileTrailColors.Add(2, Vector3.One);
      MyProjectilesConstants.m_projectileTrailColors.Add(4, Vector3.One);
    }

    public static Vector3 GetProjectileTrailColorByType(MyAmmoType ammoType)
    {
      Vector3 vector3;
      if (!MyProjectilesConstants.m_projectileTrailColors.TryGetValue((int) ammoType, out vector3))
        MyProjectilesConstants.m_projectileTrailColors.TryGetValue(-1, out vector3);
      return vector3;
    }
  }
}
