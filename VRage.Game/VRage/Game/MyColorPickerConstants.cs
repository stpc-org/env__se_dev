// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyColorPickerConstants
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRageMath;

namespace VRage.Game
{
  public static class MyColorPickerConstants
  {
    public static readonly float SATURATION_DELTA = 0.8f;
    public static readonly float VALUE_DELTA = 0.55f;
    public static readonly float VALUE_COLORIZE_DELTA = 0.1f;

    public static Vector3 HSVOffsetToHSV(Vector3 hsvOffset) => new Vector3(hsvOffset.X, MathHelper.Clamp(hsvOffset.Y + MyColorPickerConstants.SATURATION_DELTA, 0.0f, 1f), MathHelper.Clamp(hsvOffset.Z + MyColorPickerConstants.VALUE_DELTA - MyColorPickerConstants.VALUE_COLORIZE_DELTA, 0.0f, 1f));

    public static Vector3 HSVToHSVOffset(Vector3 hsv)
    {
      float y = hsv.Y - MyColorPickerConstants.SATURATION_DELTA;
      float z = hsv.Z - MyColorPickerConstants.VALUE_DELTA + MyColorPickerConstants.VALUE_COLORIZE_DELTA;
      return new Vector3(hsv.X, y, z);
    }
  }
}
