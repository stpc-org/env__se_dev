// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatPlayerSuffocating
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage.Library.Utils;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
  public class MyStatPlayerSuffocating : MyStatBase
  {
    private static readonly MyGameTimer TIMER = new MyGameTimer();
    private static readonly double VISIBLE_TIME_MS = 2000.0;
    private static readonly MyStringHash LOW_PRESSURE_DAMANGE_TYPE = MyStringHash.GetOrCompute("LowPressure");
    private float m_lastHealthRatio;
    private double m_lastTimeChecked;

    public MyStatPlayerSuffocating()
    {
      this.Id = MyStringHash.GetOrCompute("player_suffocating");
      this.m_lastHealthRatio = 1f;
    }

    public override void Update()
    {
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      if (localCharacter == null || localCharacter.StatComp == null)
        return;
      double totalMilliseconds = MyStatPlayerSuffocating.TIMER.ElapsedTimeSpan.TotalMilliseconds;
      if (totalMilliseconds - this.m_lastTimeChecked <= MyStatPlayerSuffocating.VISIBLE_TIME_MS)
        return;
      this.CurrentValue = localCharacter.StatComp.LastDamage.Type == MyStatPlayerSuffocating.LOW_PRESSURE_DAMANGE_TYPE ? 1f : 0.0f;
      if ((double) localCharacter.StatComp.HealthRatio >= (double) this.m_lastHealthRatio)
        this.CurrentValue = 0.0f;
      this.m_lastTimeChecked = totalMilliseconds;
      this.m_lastHealthRatio = localCharacter.StatComp.HealthRatio;
    }
  }
}
