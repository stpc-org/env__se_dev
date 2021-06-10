// Decompiled with JetBrains decompiler
// Type: VRage.Profiler.MyDrawArea
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage.Profiler
{
  public class MyDrawArea
  {
    public readonly float XStart;
    public readonly float YStart;
    public readonly float XScale;
    public readonly float YScale;

    public float YRange { get; private set; }

    public float YLegendMsIncrement { get; private set; }

    public int YLegendMsCount { get; private set; }

    public float YLegendIncrement { get; private set; }

    public int Index { get; private set; }

    public MyDrawArea(float xStart, float yStart, float xScale, float yScale, float yRange)
    {
      this.Index = (int) Math.Round(Math.Log((double) yRange, 2.0) * 2.0);
      this.XStart = xStart;
      this.YStart = yStart;
      this.XScale = xScale;
      this.YScale = yScale;
      this.UpdateRange();
    }

    public void IncreaseYRange()
    {
      ++this.Index;
      this.UpdateRange();
    }

    public void DecreaseYRange()
    {
      --this.Index;
      this.UpdateRange();
    }

    public float GetYRange(int index) => (float) (Math.Pow(2.0, (double) (index / 2)) * (1.0 + (double) (index % 2) * (index < 0 ? 0.25 : 0.5)));

    private void UpdateRange()
    {
      this.YRange = this.GetYRange(this.Index);
      this.YLegendMsCount = this.Index % 2 == 0 ? 8 : 12;
      this.YLegendMsIncrement = this.YRange / (float) this.YLegendMsCount;
      this.YLegendIncrement = this.YScale / this.YRange * this.YLegendMsIncrement;
    }
  }
}
