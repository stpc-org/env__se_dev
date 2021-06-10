﻿// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.MyGuidedMissileConstants
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRageMath;

namespace Sandbox.Game
{
  internal class MyGuidedMissileConstants
  {
    public const float GENERATE_SMOKE_TRAIL_PARTICLE_DENSITY_PER_METER = 4f;
    public const int MAX_MISSILES_COUNT = 50;
    public const int MISSILE_LAUNCHER_SHOT_INTERVAL_IN_MILISECONDS = 1000;
    public const float MISSILE_BLEND_VELOCITIES_IN_MILISECONDS = 400f;
    public const int MISSILE_TIMEOUT = 15000;
    public static readonly Vector4 MISSILE_LIGHT_COLOR = new Vector4(1.5f, 1.5f, 1f, 1f);
    public static float MISSILE_TURN_SPEED = 5f;
    public const int MISSILE_INIT_TIME = 500;
    public static readonly Vector3 MISSILE_INIT_DIR = new Vector3(0.0f, -0.5f, -10f);
    public const float MISSILE_PREDICATION_TIME_TRESHOLD = 0.05f;
    public const int MISSILE_TARGET_UPDATE_INTERVAL_IN_MS = 100;
    public const float VISUAL_GUIDED_MISSILE_FOV = 40f;
    public const float VISUAL_GUIDED_MISSILE_RANGE = 1000f;
    public const float ENGINE_GUIDED_MISSILE_RADIUS = 200f;
  }
}
