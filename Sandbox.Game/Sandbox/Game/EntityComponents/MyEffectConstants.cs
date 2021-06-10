// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.MyEffectConstants
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Runtime.InteropServices;

namespace Sandbox.Game.EntityComponents
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct MyEffectConstants
  {
    public static float HealthTick = 0.004166667f;
    public static float HealthInterval = 1f;
    public static float MinOxygenLevelForHealthRegeneration = 0.75f;
    public static float GenericHeal = -0.075f;
    public static float MedRoomHeal = 5f * MyEffectConstants.GenericHeal;
  }
}
