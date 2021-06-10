// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyConstants
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRageMath;

namespace VRage.Game
{
  public static class MyConstants
  {
    public static readonly float FIELD_OF_VIEW_CONFIG_MIN = MathHelper.ToRadians(40f);
    public static readonly float FIELD_OF_VIEW_CONFIG_MAX = MathHelper.ToRadians(120f);
    public static readonly float FIELD_OF_VIEW_CONFIG_MAX_SAFE = MathHelper.ToRadians(120f);
    public static readonly float FIELD_OF_VIEW_CONFIG_DEFAULT = MathHelper.ToRadians(70f);
    public static readonly float FIELD_OF_VIEW_CONFIG_MAX_DUAL_HEAD = MathHelper.ToRadians(80f);
    public static readonly float FIELD_OF_VIEW_CONFIG_MAX_TRIPLE_HEAD = MathHelper.ToRadians(70f);
    public const int FAREST_TIME_IN_PAST = -60000;
    public static readonly Vector3D GAME_PRUNING_STRUCTURE_AABB_EXTENSION = new Vector3D(3.0);
    public static float DEFAULT_INTERACTIVE_DISTANCE = 5f;
    public static float DEFAULT_GROUND_SEARCH_DISTANCE = 2f;
    public static float FLOATING_OBJ_INTERACTIVE_DISTANCE = 3f;
    public static float MAX_THRUST = 1.5f;
  }
}
