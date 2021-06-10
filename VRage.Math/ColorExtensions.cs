// Decompiled with JetBrains decompiler
// Type: VRageMath.ColorExtensions
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System;

namespace VRageMath
{
  public static class ColorExtensions
  {
    public static Vector3 ColorToHSV(this Color rgb)
    {
      System.Drawing.Color color = System.Drawing.Color.FromArgb((int) rgb.R, (int) rgb.G, (int) rgb.B);
      int num1 = (int) Math.Max(color.R, Math.Max(color.G, color.B));
      int num2 = (int) Math.Min(color.R, Math.Min(color.G, color.B));
      double num3 = (double) color.GetHue() / 360.0;
      float num4 = num1 == 0 ? 0.0f : (float) (1.0 - 1.0 * (double) num2 / (double) num1);
      float num5 = (float) num1 / (float) byte.MaxValue;
      double num6 = (double) num4;
      double num7 = (double) num5;
      return new Vector3((float) num3, (float) num6, (float) num7);
    }

    public static Vector3 ColorToHSVDX11(this Color rgb)
    {
      System.Drawing.Color color = System.Drawing.Color.FromArgb((int) rgb.R, (int) rgb.G, (int) rgb.B);
      int num1 = (int) Math.Max(color.R, Math.Max(color.G, color.B));
      int num2 = (int) Math.Min(color.R, Math.Min(color.G, color.B));
      double num3 = (double) color.GetHue() / 360.0;
      float num4 = num1 == 0 ? -1f : (float) (1.0 - 2.0 * (double) num2 / (double) num1);
      float num5 = (float) (2.0 * (double) num1 / (double) byte.MaxValue - 1.0);
      double num6 = (double) num4;
      double num7 = (double) num5;
      return new Vector3((float) num3, (float) num6, (float) num7);
    }

    public static Color HexToColor(string hex)
    {
      if (hex.Length > 0 && !hex.StartsWith("#"))
        hex = "#" + hex;
      Color? nullable = ColorExtensions.FromHtml(hex);
      return !nullable.HasValue ? Color.Pink : nullable.Value;
    }

    public static Vector4 HexToVector4(string hex) => ColorExtensions.HexToColor(hex).ToVector4();

    public static Color? FromHtml(string htmlColor)
    {
      if (!string.IsNullOrEmpty(htmlColor) && htmlColor[0] == '#')
      {
        if (htmlColor.Length == 7)
          return new Color?(new Color(Convert.ToInt32(htmlColor.Substring(1, 2), 16), Convert.ToInt32(htmlColor.Substring(3, 2), 16), Convert.ToInt32(htmlColor.Substring(5, 2), 16)));
        if (htmlColor.Length == 4)
        {
          string str1 = char.ToString(htmlColor[1]);
          string str2 = char.ToString(htmlColor[2]);
          string str3 = char.ToString(htmlColor[3]);
          return new Color?(new Color(Convert.ToInt32(str1 + str1, 16), Convert.ToInt32(str2 + str2, 16), Convert.ToInt32(str3 + str3, 16)));
        }
      }
      return new Color?();
    }

    private static Vector3 Hue(float H)
    {
      double num1 = (double) Math.Abs((float) ((double) H * 6.0 - 3.0)) - 1.0;
      float num2 = 2f - Math.Abs((float) ((double) H * 6.0 - 2.0));
      float num3 = 2f - Math.Abs((float) ((double) H * 6.0 - 4.0));
      return new Vector3(MathHelper.Clamp((float) num1, 0.0f, 1f), MathHelper.Clamp(num2, 0.0f, 1f), MathHelper.Clamp(num3, 0.0f, 1f));
    }

    public static Color HSVtoColor(this Vector3 HSV) => new Color(((ColorExtensions.Hue(HSV.X) - 1f) * HSV.Y + 1f) * HSV.Z);

    public static uint PackHSVToUint(this Vector3 HSV)
    {
      int num1 = (int) Math.Round((double) HSV.X * 360.0);
      int num2 = (int) Math.Round((double) HSV.Y * 100.0 + 100.0);
      int num3 = (int) Math.Round((double) HSV.Z * 100.0 + 100.0);
      int num4 = num2 << 16;
      int num5 = num3 << 24;
      int num6 = num4;
      return (uint) (num1 | num6 | num5);
    }

    public static Vector3 UnpackHSVFromUint(uint packed) => new Vector3((float) (ushort) packed / 360f, (float) ((int) (byte) (packed >> 16) - 100) / 100f, (float) ((int) (byte) (packed >> 24) - 100) / 100f);

    public static float HueDistance(this Color color, float hue)
    {
      float val1 = Math.Abs(color.ColorToHSV().X - hue);
      return Math.Min(val1, 1f - val1);
    }

    public static float HueDistance(this Color color, Color otherColor) => color.HueDistance(otherColor.ColorToHSV().X);

    public static Vector3 TemperatureToRGB(float temperature)
    {
      Vector3 vector3 = new Vector3();
      temperature /= 100f;
      if ((double) temperature <= 66.0)
      {
        vector3.X = 1f;
        vector3.Y = (float) MathHelper.Saturate(0.390081579 * Math.Log((double) temperature) - 0.631841444);
      }
      else
      {
        float num = temperature - 60f;
        vector3.X = (float) MathHelper.Saturate(1.292936186 * Math.Pow((double) num, -0.1332047592));
        vector3.Y = (float) MathHelper.Saturate(1.129890861 * Math.Pow((double) num, -0.0755148492));
      }
      vector3.Z = (double) temperature < 66.0 ? ((double) temperature > 19.0 ? (float) MathHelper.Saturate(0.543206789 * Math.Log((double) temperature - 10.0) - 1.196254089) : 0.0f) : 1f;
      return vector3;
    }

    public static Vector4 UnmultiplyColor(this Vector4 c) => (double) c.W == 0.0 ? Vector4.Zero : new Vector4(c.X / c.W, c.Y / c.W, c.Z / c.W, c.W);

    public static Vector4 PremultiplyColor(this Vector4 c) => new Vector4(c.X * c.W, c.Y * c.W, c.Z * c.W, c.W);

    public static Vector4 ToSRGB(this Vector4 c) => new Vector4(ColorExtensions.ToSRGBComponent(c.X), ColorExtensions.ToSRGBComponent(c.Y), ColorExtensions.ToSRGBComponent(c.Z), c.W);

    public static Vector4 ToLinearRGB(this Vector4 c) => new Vector4(ColorExtensions.ToLinearRGBComponent(c.X), ColorExtensions.ToLinearRGBComponent(c.Y), ColorExtensions.ToLinearRGBComponent(c.Z), c.W);

    public static Vector3 ToLinearRGB(this Vector3 c) => new Vector3(ColorExtensions.ToLinearRGBComponent(c.X), ColorExtensions.ToLinearRGBComponent(c.Y), ColorExtensions.ToLinearRGBComponent(c.Z));

    public static Vector3 ToSRGB(this Vector3 c) => new Vector3(ColorExtensions.ToSRGBComponent(c.X), ColorExtensions.ToSRGBComponent(c.Y), ColorExtensions.ToSRGBComponent(c.Z));

    public static Vector3 ToGray(this Vector3 c) => new Vector3((float) (0.212599992752075 * (double) c.X + 0.715200006961823 * (double) c.Y + 0.0722000002861023 * (double) c.Z));

    public static Vector4 ToGray(this Vector4 c)
    {
      double num = 0.212599992752075 * (double) c.X + 0.715200006961823 * (double) c.Y + 0.0722000002861023 * (double) c.Z;
      return new Vector4((float) num, (float) num, (float) num, c.W);
    }

    public static float ToLinearRGBComponent(float c) => (double) c > 0.0404499992728233 ? (float) Math.Pow(((double) c + 0.0549999997019768) / 1.05499994754791, 2.40000009536743) : c / 12.92f;

    public static float ToSRGBComponent(float c) => (double) c > 0.00313080009073019 ? (float) (Math.Pow((double) c, 0.416666656732559) * 1.05499994754791 - 0.0549999997019768) : c * 12.92f;

    public static Color Shade(this Color c, float r) => new Color((int) ((double) c.R * (double) r), (int) ((double) c.G * (double) r), (int) ((double) c.B * (double) r), (int) c.A);

    public static Color Tint(this Color c, float r) => new Color((int) ((double) c.R + (double) ((int) byte.MaxValue - (int) c.R) * (double) r), (int) ((double) c.G + (double) ((int) byte.MaxValue - (int) c.G) * (double) r), (int) ((double) c.B + (double) ((int) byte.MaxValue - (int) c.B) * (double) r), (int) c.A);

    public static Color Alpha(this Color c, float a) => new Color(c, a);
  }
}
