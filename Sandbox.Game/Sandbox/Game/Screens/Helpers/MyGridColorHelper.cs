// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyGridColorHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
  internal class MyGridColorHelper
  {
    private Dictionary<MyCubeGrid, Color?> m_colors = new Dictionary<MyCubeGrid, Color?>();
    private int m_lastColorIndex;

    public void Init(MyCubeGrid mainGrid = null)
    {
      this.m_lastColorIndex = 0;
      this.m_colors.Clear();
      if (mainGrid == null)
        return;
      this.m_colors.Add(mainGrid, new Color?());
    }

    public Color? GetGridColor(MyCubeGrid grid)
    {
      Color? nullable;
      if (!this.m_colors.TryGetValue(grid, out nullable))
      {
        do
        {
          nullable = new Color?(new Vector3((float) (this.m_lastColorIndex++ % 20) / 20f, 0.75f, 1f).HSVtoColor());
        }
        while ((double) nullable.Value.HueDistance(Color.Red) < 0.0399999991059303 || (double) nullable.Value.HueDistance(0.65f) < 0.0700000002980232);
        this.m_colors[grid] = nullable;
      }
      return nullable;
    }
  }
}
