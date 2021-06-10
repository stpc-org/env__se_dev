// Decompiled with JetBrains decompiler
// Type: VRageRender.MyDisplayMode
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Diagnostics;

namespace VRageRender
{
  [DebuggerDisplay("{Width}x{Height}@{RefreshRate}Hz")]
  public struct MyDisplayMode
  {
    public int Width;
    public int Height;
    public int RefreshRate;
    public int? RefreshRateDenominator;

    public float RefreshRateF => !this.RefreshRateDenominator.HasValue ? (float) this.RefreshRate : (float) this.RefreshRate / (float) this.RefreshRateDenominator.Value;

    public MyDisplayMode(int width, int height, int refreshRate, int? refreshRateDenominator = null)
    {
      this.Width = width;
      this.Height = height;
      this.RefreshRate = refreshRate;
      this.RefreshRateDenominator = refreshRateDenominator;
    }

    public override string ToString() => this.RefreshRateDenominator.HasValue ? string.Format("{0}x{1}@{2}Hz", (object) this.Width, (object) this.Height, (object) (float) ((double) this.RefreshRate / (double) this.RefreshRateDenominator.Value)) : string.Format("{0}x{1}@{2}Hz", (object) this.Width, (object) this.Height, (object) this.RefreshRate);
  }
}
