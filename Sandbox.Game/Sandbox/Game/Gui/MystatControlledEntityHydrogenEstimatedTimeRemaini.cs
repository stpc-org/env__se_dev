// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MystatControlledEntityHydrogenEstimatedTimeRemaining
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
  public class MystatControlledEntityHydrogenEstimatedTimeRemaining : MyStatBase
  {
    private MyStatBase m_usageStat;

    public MystatControlledEntityHydrogenEstimatedTimeRemaining() => this.Id = MyStringHash.GetOrCompute("controlled_estimated_time_remaining_hydrogen");

    public override void Update()
    {
      if (this.m_usageStat == null)
        this.m_usageStat = (MyStatBase) MyHud.Stats.GetStat<MyStatControlledEntityHydrogenCapacity>();
      else
        this.CurrentValue = this.m_usageStat.CurrentValue / this.m_usageStat.MaxValue;
    }
  }
}
