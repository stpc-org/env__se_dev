// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatPlayerInventoryCapacityChanged
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage;
using VRage.Library.Utils;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
  public class MyStatPlayerInventoryCapacityChanged : MyStatBase
  {
    private static readonly MyGameTimer TIMER = new MyGameTimer();
    private static readonly double VISIBLE_TIME_MS = 3000.0;
    private int m_lastVolume;
    private double m_timeToggled;

    public MyStatPlayerInventoryCapacityChanged() => this.Id = MyStringHash.GetOrCompute("player_inventory_capacity_changed");

    public override void Update()
    {
      double totalMilliseconds = MyStatPlayerInventoryCapacityChanged.TIMER.ElapsedTimeSpan.TotalMilliseconds;
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      if (localCharacter != null)
      {
        int intSafe = MyFixedPoint.MultiplySafe(MyEntityExtensions.GetInventory(localCharacter).CurrentVolume, 1000).ToIntSafe();
        if (this.m_lastVolume != intSafe)
        {
          this.CurrentValue = 1f;
          this.m_timeToggled = totalMilliseconds;
          this.m_lastVolume = intSafe;
        }
      }
      if ((double) this.CurrentValue != 1.0 || totalMilliseconds - this.m_timeToggled <= MyStatPlayerInventoryCapacityChanged.VISIBLE_TIME_MS)
        return;
      this.CurrentValue = 0.0f;
    }
  }
}
