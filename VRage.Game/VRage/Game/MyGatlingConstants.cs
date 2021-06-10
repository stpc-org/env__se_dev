// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyGatlingConstants
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRageMath;

namespace VRage.Game
{
  public static class MyGatlingConstants
  {
    public const float ROTATION_SPEED_PER_SECOND = 12.56637f;
    public const int ROTATION_TIMEOUT = 2000;
    public const int SHOT_INTERVAL_IN_MILISECONDS = 95;
    public const int REAL_SHOTS_PER_SECOND = 45;
    public const int MIN_TIME_RELEASE_INTERVAL_IN_MILISECONDS = 204;
    public static readonly float SHOT_PROJECTILE_DEBRIS_MAX_DEVIATION_ANGLE = MathHelper.ToRadians(30f);
    public static readonly float COCKPIT_GLASS_PROJECTILE_DEBRIS_MAX_DEVIATION_ANGLE = MathHelper.ToRadians(10f);
    public const int SMOKE_INCREASE_PER_SHOT = 19;
    public const int SMOKE_DECREASE = 1;
    public const int SMOKES_MAX = 50;
    public const int SMOKES_MIN = 40;
    public const int SMOKES_INTERVAL_IN_MILISECONDS = 10;
  }
}
