// Decompiled with JetBrains decompiler
// Type: VRageRender.MyRefreshRatePriorityComparer
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Generic;

namespace VRageRender
{
  public class MyRefreshRatePriorityComparer : IComparer<MyDisplayMode>
  {
    private static readonly float[] m_refreshRates = new float[5]
    {
      60f,
      75f,
      59f,
      72f,
      100f
    };

    public int Compare(MyDisplayMode x, MyDisplayMode y)
    {
      if (x.Width != y.Width)
        return x.Width.CompareTo(y.Width);
      if (x.Height != y.Height)
        return x.Height.CompareTo(y.Height);
      if ((double) x.RefreshRateF == (double) y.RefreshRateF)
        return 0;
      for (int index = 0; index < MyRefreshRatePriorityComparer.m_refreshRates.Length; ++index)
      {
        if ((double) x.RefreshRateF == (double) MyRefreshRatePriorityComparer.m_refreshRates[index])
          return -1;
        if ((double) y.RefreshRateF == (double) MyRefreshRatePriorityComparer.m_refreshRates[index])
          return 1;
      }
      return x.RefreshRate.CompareTo(y.RefreshRate);
    }
  }
}
