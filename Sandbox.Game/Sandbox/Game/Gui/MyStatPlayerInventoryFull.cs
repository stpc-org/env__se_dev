// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatPlayerInventoryFull
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Library.Utils;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
  public class MyStatPlayerInventoryFull : MyStatBase
  {
    private static readonly MyGameTimer TIMER = new MyGameTimer();
    private static readonly double VISIBLE_TIME_MS = 5000.0;
    private double m_visibleFromMs;
    private bool m_inventoryFull;

    public bool InventoryFull
    {
      get => this.m_inventoryFull;
      set
      {
        if (value)
          this.m_visibleFromMs = MyStatPlayerInventoryFull.TIMER.ElapsedTimeSpan.TotalMilliseconds;
        this.m_inventoryFull = value;
        this.CurrentValue = value ? 1f : 0.0f;
      }
    }

    public MyStatPlayerInventoryFull() => this.Id = MyStringHash.GetOrCompute("player_inventory_full");

    public override void Update()
    {
      if (!this.m_inventoryFull || MyStatPlayerInventoryFull.TIMER.ElapsedTimeSpan.TotalMilliseconds - this.m_visibleFromMs <= MyStatPlayerInventoryFull.VISIBLE_TIME_MS)
        return;
      this.m_inventoryFull = false;
      this.CurrentValue = 0.0f;
    }
  }
}
