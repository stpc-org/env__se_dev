// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatControlledEntityEnergyEstimatedTimeRemaining
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using System.Text;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
  public class MyStatControlledEntityEnergyEstimatedTimeRemaining : MyStatBase
  {
    private readonly StringBuilder m_stringBuilder = new StringBuilder();

    public MyStatControlledEntityEnergyEstimatedTimeRemaining() => this.Id = MyStringHash.GetOrCompute("controlled_estimated_time_remaining_energy");

    public override void Update() => this.CurrentValue = MyHud.ShipInfo.FuelRemainingTime;

    public override string ToString()
    {
      this.m_stringBuilder.Clear();
      MyValueFormatter.AppendTimeInBestUnit(this.CurrentValue * 3600f, this.m_stringBuilder);
      return this.m_stringBuilder.ToString();
    }
  }
}
