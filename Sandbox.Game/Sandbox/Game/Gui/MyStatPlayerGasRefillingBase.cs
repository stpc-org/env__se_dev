// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatPlayerGasRefillingBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.World;
using VRage.Library.Utils;

namespace Sandbox.Game.GUI
{
  public class MyStatPlayerGasRefillingBase : MyStatBase
  {
    private static readonly MyGameTimer TIMER = new MyGameTimer();
    private static readonly double VISIBLE_TIME_MS = 2000.0;
    private float m_lastGasLevel;
    private double m_lastTimeChecked;

    public override void Update()
    {
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      if (localCharacter == null || localCharacter.OxygenComponent == null)
        return;
      double totalMilliseconds = MyStatPlayerGasRefillingBase.TIMER.ElapsedTimeSpan.TotalMilliseconds;
      if (totalMilliseconds - this.m_lastTimeChecked <= MyStatPlayerGasRefillingBase.VISIBLE_TIME_MS)
        return;
      float gassLevel = this.GetGassLevel(localCharacter.OxygenComponent);
      this.CurrentValue = (double) gassLevel > (double) this.m_lastGasLevel ? 1f : 0.0f;
      this.m_lastTimeChecked = totalMilliseconds;
      this.m_lastGasLevel = gassLevel;
    }

    protected virtual float GetGassLevel(MyCharacterOxygenComponent oxygenComp) => 0.0f;
  }
}
