// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyLightsConstants
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRageMath;

namespace VRage.Game
{
  public static class MyLightsConstants
  {
    public const int MAX_LIGHTS_COUNT = 4000;
    public const int MAX_LIGHTS_COUNT_WHEN_DRAWING = 16;
    public const int MAX_LIGHTS_FOR_EFFECT = 8;
    public const int MAX_POINTLIGHT_RADIUS = 120;
    public const float MAX_SPOTLIGHT_RANGE = 1200f;
    public static readonly float MAX_SPOTLIGHT_ANGLE = 80f;
    public static readonly float MAX_SPOTLIGHT_ANGLE_COS = 1f - (float) Math.Cos((double) MathHelper.ToRadians(MyLightsConstants.MAX_SPOTLIGHT_ANGLE));
  }
}
