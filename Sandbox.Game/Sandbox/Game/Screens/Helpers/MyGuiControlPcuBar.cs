// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyGuiControlPcuBar
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
  public class MyGuiControlPcuBar : MyGuiControlInfoProgressBar
  {
    private int m_maxPCU;
    private int m_currentPCU;
    private int m_currentDisplayedPCU;
    private int m_frameCounterPCU;

    public MyGuiControlPcuBar(Vector2? position = null, float? width = null)
    {
      float? nullable = width;
      // ISSUE: explicit constructor call
      base.\u002Ector(nullable.HasValue ? nullable.GetValueOrDefault() : 0.32f, position, "Textures\\GUI\\PCU.png", "PCU:");
    }

    public void UpdatePCU(MyIdentity identity, bool performAnimation)
    {
      this.m_maxPCU = 0;
      this.m_currentPCU = 0;
      if (identity != null)
      {
        this.m_maxPCU = identity.GetMaxPCU();
        this.m_currentPCU = identity.BlockLimits.PCU;
      }
      if (MySession.Static.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.NONE || MySession.Static.TotalPCU == 0)
      {
        this.m_currentDisplayedPCU = this.m_currentPCU;
        this.m_RightLabel.TextEnum = MyCommonTexts.Unlimited;
      }
      else if (this.m_currentDisplayedPCU != this.m_currentPCU)
      {
        if (performAnimation)
        {
          int num = Math.Max(1, Math.Abs((this.m_currentPCU - this.m_currentDisplayedPCU) / 20));
          this.m_currentDisplayedPCU = this.m_currentPCU < this.m_currentDisplayedPCU ? this.m_currentDisplayedPCU - num : this.m_currentDisplayedPCU + num;
        }
        else
          this.m_currentDisplayedPCU = this.m_currentPCU;
        this.m_RightLabel.Text = string.Format("{0} / {1}", (object) this.m_currentPCU, (object) this.m_maxPCU);
      }
      this.m_BarInnerLine.Size = new Vector2(this.m_maxPCU != 0 ? Math.Min(this.m_barSize.X / (float) this.m_maxPCU * (float) this.m_currentDisplayedPCU, this.m_barSize.X) : 0.0f, this.m_barSize.Y);
    }
  }
}
