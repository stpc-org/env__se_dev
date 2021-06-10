// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MySliderScaleHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRageMath;

namespace Sandbox.Game.SessionComponents
{
  public class MySliderScaleHelper
  {
    public static float RescaleUp(float value)
    {
      float num = (float) ((double) value * 1.20000004768372 * 2.0);
      return (double) num % 1.0 <= 1.0 / 1000.0 ? 0.5f * num : (float) (0.5 * ((double) num + 1.0 - (double) num % 1.0));
    }

    public static float RescaleDown(float value)
    {
      float num = (float) ((double) value * 0.800000011920929 * 2.0);
      return (float) (0.5 * ((double) num - (double) num % 1.0));
    }

    public static void ScaleSliderUp(ref MyBrushGUIPropertyNumberSlider slider) => slider.Value = MyMath.Clamp(MySliderScaleHelper.RescaleUp(slider.Value), slider.ValueMin, slider.ValueMax);

    public static void ScaleSliderDown(ref MyBrushGUIPropertyNumberSlider slider) => slider.Value = MyMath.Clamp(MySliderScaleHelper.RescaleDown(slider.Value), slider.ValueMin, slider.ValueMax);
  }
}
