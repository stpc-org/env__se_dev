// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Platform.VideoMode.MyPerformanceSettings
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRageRender;

namespace Sandbox.Engine.Platform.VideoMode
{
  public struct MyPerformanceSettings
  {
    public MyRenderSettings1 RenderSettings;
    public bool EnableDamageEffects;

    public override bool Equals(object other)
    {
      MyPerformanceSettings other1 = (MyPerformanceSettings) other;
      return this.Equals(ref other1);
    }

    private bool Equals(ref MyPerformanceSettings other) => this.EnableDamageEffects == other.EnableDamageEffects && this.RenderSettings.Equals(ref other.RenderSettings);
  }
}
