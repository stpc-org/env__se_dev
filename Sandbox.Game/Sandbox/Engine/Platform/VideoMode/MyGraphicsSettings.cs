// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Platform.VideoMode.MyGraphicsSettings
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Utils;

namespace Sandbox.Engine.Platform.VideoMode
{
  public struct MyGraphicsSettings
  {
    public MyPerformanceSettings PerformanceSettings;
    public float FieldOfView;
    public bool PostProcessingEnabled;
    public MyStringId GraphicsRenderer;
    public float FlaresIntensity;

    public override bool Equals(object other)
    {
      MyGraphicsSettings other1 = (MyGraphicsSettings) other;
      return this.Equals(ref other1);
    }

    public bool Equals(ref MyGraphicsSettings other) => (double) this.FieldOfView == (double) other.FieldOfView && this.PostProcessingEnabled == other.PostProcessingEnabled && ((double) this.FlaresIntensity == (double) other.FlaresIntensity && this.GraphicsRenderer == other.GraphicsRenderer) && this.PerformanceSettings.Equals((object) other.PerformanceSettings);
  }
}
