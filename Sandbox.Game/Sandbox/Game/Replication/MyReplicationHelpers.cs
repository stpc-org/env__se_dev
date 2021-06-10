// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.MyReplicationHelpers
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

namespace Sandbox.Game.Replication
{
  public static class MyReplicationHelpers
  {
    public static float RampPriority(
      float priority,
      int frameCountWithoutSync,
      float updateOncePer,
      float rampAmount = 0.5f,
      bool alsoRampDown = true)
    {
      if ((double) frameCountWithoutSync >= (double) updateOncePer)
      {
        float num1 = ((float) frameCountWithoutSync - updateOncePer) / updateOncePer;
        if ((double) num1 > 1.0)
        {
          float num2 = (num1 - 1f) * rampAmount;
          priority *= num2;
        }
        return priority;
      }
      return !alsoRampDown ? priority : 0.0f;
    }
  }
}
