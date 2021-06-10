// Decompiled with JetBrains decompiler
// Type: VRageMath.Color
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using ProtoBuf;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath.PackedVector;

namespace VRageMath
{
  [ProtoContract]
  [Serializable]
  public struct Color : IPackedVector<uint>, IPackedVector, IEquatable<Color>
  {
    [ProtoMember(1)]
    public uint PackedValue;

    public byte X
    {
      get => this.R;
      set => this.R = value;
    }

    public byte Y
    {
      get => this.G;
      set => this.G = value;
    }

    public byte Z
    {
      get => this.B;
      set => this.B = value;
    }

    public byte R
    {
      get => (byte) this.PackedValue;
      set => this.PackedValue = this.PackedValue & 4294967040U | (uint) value;
    }

    public byte G
    {
      get => (byte) (this.PackedValue >> 8);
      set => this.PackedValue = (uint) ((int) this.PackedValue & -65281 | (int) value << 8);
    }

    public byte B
    {
      get => (byte) (this.PackedValue >> 16);
      set => this.PackedValue = (uint) ((int) this.PackedValue & -16711681 | (int) value << 16);
    }

    public byte A
    {
      get => (byte) (this.PackedValue >> 24);
      set => this.PackedValue = (uint) ((int) this.PackedValue & 16777215 | (int) value << 24);
    }

    public static Color Transparent => new Color(0U);

    public static Color AliceBlue => new Color(4294965488U);

    public static Color AntiqueWhite => new Color(4292340730U);

    public static Color Aqua => new Color(4294967040U);

    public static Color Aquamarine => new Color(4292149119U);

    public static Color Azure => new Color(4294967280U);

    public static Color Beige => new Color(4292670965U);

    public static Color Bisque => new Color(4291093759U);

    public static Color Black => new Color(4278190080U);

    public static Color BlanchedAlmond => new Color(4291685375U);

    public static Color Blue => new Color(4294901760U);

    public static Color BlueViolet => new Color(4293012362U);

    public static Color Brown => new Color(4280953509U);

    public static Color BurlyWood => new Color(4287084766U);

    public static Color CadetBlue => new Color(4288716383U);

    public static Color Chartreuse => new Color(4278255487U);

    public static Color Chocolate => new Color(4280183250U);

    public static Color Coral => new Color(4283465727U);

    public static Color CornflowerBlue => new Color(4293760356U);

    public static Color Cornsilk => new Color(4292671743U);

    public static Color Crimson => new Color(4282127580U);

    public static Color Cyan => new Color(4294967040U);

    public static Color DarkBlue => new Color(4287299584U);

    public static Color DarkCyan => new Color(4287335168U);

    public static Color DarkGoldenrod => new Color(4278945464U);

    public static Color DarkGray => new Color(4289309097U);

    public static Color DarkGreen => new Color(4278215680U);

    public static Color DarkKhaki => new Color(4285249469U);

    public static Color DarkMagenta => new Color(4287299723U);

    public static Color DarkOliveGreen => new Color(4281297749U);

    public static Color DarkOrange => new Color(4278226175U);

    public static Color DarkOrchid => new Color(4291572377U);

    public static Color DarkRed => new Color(4278190219U);

    public static Color DarkSalmon => new Color(4286224105U);

    public static Color DarkSeaGreen => new Color(4287347855U);

    public static Color DarkSlateBlue => new Color(4287315272U);

    public static Color DarkSlateGray => new Color(4283387695U);

    public static Color DarkTurquoise => new Color(4291939840U);

    public static Color DarkViolet => new Color(4292018324U);

    public static Color DeepPink => new Color(4287829247U);

    public static Color DeepSkyBlue => new Color(4294950656U);

    public static Color DimGray => new Color(4285098345U);

    public static Color DodgerBlue => new Color(4294938654U);

    public static Color Firebrick => new Color(4280427186U);

    public static Color FloralWhite => new Color(4293982975U);

    public static Color ForestGreen => new Color(4280453922U);

    public static Color Fuchsia => new Color(4294902015U);

    public static Color Gainsboro => new Color(4292664540U);

    public static Color GhostWhite => new Color(4294965496U);

    public static Color Gold => new Color(4278245375U);

    public static Color Goldenrod => new Color(4280329690U);

    public static Color Gray => new Color(4286611584U);

    public static Color Green => new Color(4278222848U);

    public static Color GreenYellow => new Color(4281335725U);

    public static Color Honeydew => new Color(4293984240U);

    public static Color HotPink => new Color(4290013695U);

    public static Color IndianRed => new Color(4284243149U);

    public static Color Indigo => new Color(4286709835U);

    public static Color Ivory => new Color(4293984255U);

    public static Color Khaki => new Color(4287424240U);

    public static Color Lavender => new Color(4294633190U);

    public static Color LavenderBlush => new Color(4294308095U);

    public static Color LawnGreen => new Color(4278254716U);

    public static Color LemonChiffon => new Color(4291689215U);

    public static Color LightBlue => new Color(4293318829U);

    public static Color LightCoral => new Color(4286611696U);

    public static Color LightCyan => new Color(4294967264U);

    public static Color LightGoldenrodYellow => new Color(4292016890U);

    public static Color LightGreen => new Color(4287688336U);

    public static Color LightGray => new Color(4292072403U);

    public static Color LightPink => new Color(4290885375U);

    public static Color LightSalmon => new Color(4286226687U);

    public static Color LightSeaGreen => new Color(4289376800U);

    public static Color LightSkyBlue => new Color(4294626951U);

    public static Color LightSlateGray => new Color(4288252023U);

    public static Color LightSteelBlue => new Color(4292789424U);

    public static Color LightYellow => new Color(4292935679U);

    public static Color Lime => new Color(4278255360U);

    public static Color LimeGreen => new Color(4281519410U);

    public static Color Linen => new Color(4293325050U);

    public static Color Magenta => new Color(4294902015U);

    public static Color Maroon => new Color(4278190208U);

    public static Color MediumAquamarine => new Color(4289383782U);

    public static Color MediumBlue => new Color(4291624960U);

    public static Color MediumOrchid => new Color(4292040122U);

    public static Color MediumPurple => new Color(4292571283U);

    public static Color MediumSeaGreen => new Color(4285641532U);

    public static Color MediumSlateBlue => new Color(4293814395U);

    public static Color MediumSpringGreen => new Color(4288346624U);

    public static Color MediumTurquoise => new Color(4291613000U);

    public static Color MediumVioletRed => new Color(4286911943U);

    public static Color MidnightBlue => new Color(4285536537U);

    public static Color MintCream => new Color(4294639605U);

    public static Color MistyRose => new Color(4292994303U);

    public static Color Moccasin => new Color(4290110719U);

    public static Color NavajoWhite => new Color(4289584895U);

    public static Color Navy => new Color(4286578688U);

    public static Color OldLace => new Color(4293326333U);

    public static Color Olive => new Color(4278222976U);

    public static Color OliveDrab => new Color(4280520299U);

    public static Color Orange => new Color(4278232575U);

    public static Color OrangeRed => new Color(4278207999U);

    public static Color Orchid => new Color(4292243674U);

    public static Color PaleGoldenrod => new Color(4289390830U);

    public static Color PaleGreen => new Color(4288215960U);

    public static Color PaleTurquoise => new Color(4293848751U);

    public static Color PaleVioletRed => new Color(4287852763U);

    public static Color PapayaWhip => new Color(4292210687U);

    public static Color PeachPuff => new Color(4290370303U);

    public static Color Peru => new Color(4282353101U);

    public static Color Pink => new Color(4291543295U);

    public static Color Plum => new Color(4292714717U);

    public static Color PowderBlue => new Color(4293320880U);

    public static Color Purple => new Color(4286578816U);

    public static Color Red => new Color(4278190335U);

    public static Color RosyBrown => new Color(4287598524U);

    public static Color RoyalBlue => new Color(4292962625U);

    public static Color SaddleBrown => new Color(4279453067U);

    public static Color Salmon => new Color(4285694202U);

    public static Color SandyBrown => new Color(4284523764U);

    public static Color SeaGreen => new Color(4283927342U);

    public static Color SeaShell => new Color(4293850623U);

    public static Color Sienna => new Color(4281160352U);

    public static Color Silver => new Color(4290822336U);

    public static Color SkyBlue => new Color(4293643911U);

    public static Color SlateBlue => new Color(4291648106U);

    public static Color SlateGray => new Color(4287660144U);

    public static Color Snow => new Color(4294638335U);

    public static Color SpringGreen => new Color(4286578432U);

    public static Color SteelBlue => new Color(4290019910U);

    public static Color Tan => new Color(4287411410U);

    public static Color Teal => new Color(4286611456U);

    public static Color Thistle => new Color(4292394968U);

    public static Color Tomato => new Color(4282868735U);

    public static Color Turquoise => new Color(4291878976U);

    public static Color Violet => new Color(4293821166U);

    public static Color Wheat => new Color(4289978101U);

    public static Color White => new Color(uint.MaxValue);

    public static Color WhiteSmoke => new Color(4294309365U);

    public static Color Yellow => new Color(4278255615U);

    public static Color YellowGreen => new Color(4281519514U);

    public Color(uint packedValue) => this.PackedValue = packedValue;

    public Color(int r, int g, int b)
    {
      if (((r | g | b) & -256) != 0)
      {
        r = Color.ClampToByte64((long) r);
        g = Color.ClampToByte64((long) g);
        b = Color.ClampToByte64((long) b);
      }
      g <<= 8;
      b <<= 16;
      this.PackedValue = (uint) (r | g | b | -16777216);
    }

    public Color(int r, int g, int b, int a)
    {
      if (((r | g | b | a) & -256) != 0)
      {
        r = Color.ClampToByte32(r);
        g = Color.ClampToByte32(g);
        b = Color.ClampToByte32(b);
        a = Color.ClampToByte32(a);
      }
      g <<= 8;
      b <<= 16;
      a <<= 24;
      this.PackedValue = (uint) (r | g | b | a);
    }

    public Color(float rgb) => this.PackedValue = Color.PackHelper(rgb, rgb, rgb, 1f);

    public Color(float r, float g, float b) => this.PackedValue = Color.PackHelper(r, g, b, 1f);

    public Color(float r, float g, float b, float a) => this.PackedValue = Color.PackHelper(r, g, b, a);

    public Color(Color color, float a) => this.PackedValue = Color.PackHelper((float) color.R / (float) byte.MaxValue, (float) color.G / (float) byte.MaxValue, (float) color.B / (float) byte.MaxValue, a);

    public Color(Vector3 vector) => this.PackedValue = Color.PackHelper(vector.X, vector.Y, vector.Z, 1f);

    public Color(Vector4 vector) => this.PackedValue = Color.PackHelper(vector.X, vector.Y, vector.Z, vector.W);

    public static Color operator *(Color value, float scale)
    {
      int packedValue;
      uint num1 = (uint) (packedValue = (int) value.PackedValue);
      uint num2 = (uint) (byte) ((uint) packedValue >> 8);
      uint num3 = (uint) (byte) ((uint) packedValue >> 16);
      int num4 = (int) (byte) ((uint) packedValue >> 24);
      scale *= 65536f;
      uint num5 = (double) scale >= 0.0 ? ((double) scale <= 16777215.0 ? (uint) scale : 16777215U) : 0U;
      uint num6 = num1 * num5 >> 16;
      uint num7 = num2 * num5 >> 16;
      uint num8 = num3 * num5 >> 16;
      int num9 = (int) num5;
      uint num10 = (uint) (num4 * num9) >> 16;
      if (num6 > (uint) byte.MaxValue)
        num6 = (uint) byte.MaxValue;
      if (num7 > (uint) byte.MaxValue)
        num7 = (uint) byte.MaxValue;
      if (num8 > (uint) byte.MaxValue)
        num8 = (uint) byte.MaxValue;
      if (num10 > (uint) byte.MaxValue)
        num10 = (uint) byte.MaxValue;
      Color color;
      color.PackedValue = (uint) ((int) num6 | (int) num7 << 8 | (int) num8 << 16 | (int) num10 << 24);
      return color;
    }

    public static Color operator +(Color value, Color other) => new Color((int) value.R + (int) other.R, (int) value.G + (int) other.G, (int) value.B + (int) other.B, (int) value.A + (int) other.A);

    public static Color operator *(Color value, Color other)
    {
      Vector4 vector4_1 = value.ToVector4();
      Vector4 vector4_2 = other.ToVector4();
      return new Color(vector4_1.X * vector4_2.X, vector4_1.Y * vector4_2.Y, vector4_1.Z * vector4_2.Z, vector4_1.W * vector4_2.W);
    }

    public static bool operator ==(Color a, Color b) => a.Equals(b);

    public static bool operator !=(Color a, Color b) => !a.Equals(b);

    void IPackedVector.PackFromVector4(Vector4 vector) => this.PackedValue = Color.PackHelper(vector.X, vector.Y, vector.Z, vector.W);

    public static Color FromNonPremultiplied(Vector4 vector)
    {
      Color color;
      color.PackedValue = Color.PackHelper(vector.X * vector.W, vector.Y * vector.W, vector.Z * vector.W, vector.W);
      return color;
    }

    public static Color FromNonPremultiplied(int r, int g, int b, int a)
    {
      r = Color.ClampToByte64((long) r * (long) a / (long) byte.MaxValue);
      g = Color.ClampToByte64((long) g * (long) a / (long) byte.MaxValue);
      b = Color.ClampToByte64((long) b * (long) a / (long) byte.MaxValue);
      a = Color.ClampToByte32(a);
      g <<= 8;
      b <<= 16;
      a <<= 24;
      Color color;
      color.PackedValue = (uint) (r | g | b | a);
      return color;
    }

    private static uint PackHelper(float vectorX, float vectorY, float vectorZ, float vectorW) => (uint) ((int) PackUtils.PackUNorm((float) byte.MaxValue, vectorX) | (int) PackUtils.PackUNorm((float) byte.MaxValue, vectorY) << 8 | (int) PackUtils.PackUNorm((float) byte.MaxValue, vectorZ) << 16 | (int) PackUtils.PackUNorm((float) byte.MaxValue, vectorW) << 24);

    private static int ClampToByte32(int value)
    {
      if (value < 0)
        return 0;
      return value > (int) byte.MaxValue ? (int) byte.MaxValue : value;
    }

    private static int ClampToByte64(long value)
    {
      if (value < 0L)
        return 0;
      return value > (long) byte.MaxValue ? (int) byte.MaxValue : (int) value;
    }

    public Vector3 ToVector3()
    {
      Vector3 vector3;
      vector3.X = PackUtils.UnpackUNorm((uint) byte.MaxValue, this.PackedValue);
      vector3.Y = PackUtils.UnpackUNorm((uint) byte.MaxValue, this.PackedValue >> 8);
      vector3.Z = PackUtils.UnpackUNorm((uint) byte.MaxValue, this.PackedValue >> 16);
      return vector3;
    }

    public Vector4 ToVector4()
    {
      Vector4 vector4;
      vector4.X = PackUtils.UnpackUNorm((uint) byte.MaxValue, this.PackedValue);
      vector4.Y = PackUtils.UnpackUNorm((uint) byte.MaxValue, this.PackedValue >> 8);
      vector4.Z = PackUtils.UnpackUNorm((uint) byte.MaxValue, this.PackedValue >> 16);
      vector4.W = PackUtils.UnpackUNorm((uint) byte.MaxValue, this.PackedValue >> 24);
      return vector4;
    }

    public static Color Lerp(Color value1, Color value2, float amount)
    {
      int packedValue1 = (int) value1.PackedValue;
      uint packedValue2 = value2.PackedValue;
      int num1 = (int) (byte) packedValue1;
      int num2 = (int) (byte) ((uint) packedValue1 >> 8);
      int num3 = (int) (byte) ((uint) packedValue1 >> 16);
      int num4 = (int) (byte) ((uint) packedValue1 >> 24);
      int num5 = (int) (byte) packedValue2;
      int num6 = (int) (byte) (packedValue2 >> 8);
      int num7 = (int) (byte) (packedValue2 >> 16);
      int num8 = (int) (byte) (packedValue2 >> 24);
      int num9 = (int) PackUtils.PackUNorm(65536f, amount);
      int num10 = num1 + ((num5 - num1) * num9 >> 16);
      int num11 = num2 + ((num6 - num2) * num9 >> 16);
      int num12 = num3 + ((num7 - num3) * num9 >> 16);
      int num13 = num4 + ((num8 - num4) * num9 >> 16);
      Color color;
      color.PackedValue = (uint) (num10 | num11 << 8 | num12 << 16 | num13 << 24);
      return color;
    }

    public static Color Multiply(Color value, float scale)
    {
      int packedValue;
      uint num1 = (uint) (packedValue = (int) value.PackedValue);
      uint num2 = (uint) (byte) ((uint) packedValue >> 8);
      uint num3 = (uint) (byte) ((uint) packedValue >> 16);
      int num4 = (int) (byte) ((uint) packedValue >> 24);
      scale *= 65536f;
      uint num5 = (double) scale >= 0.0 ? ((double) scale <= 16777215.0 ? (uint) scale : 16777215U) : 0U;
      uint num6 = num1 * num5 >> 16;
      uint num7 = num2 * num5 >> 16;
      uint num8 = num3 * num5 >> 16;
      int num9 = (int) num5;
      uint num10 = (uint) (num4 * num9) >> 16;
      if (num6 > (uint) byte.MaxValue)
        num6 = (uint) byte.MaxValue;
      if (num7 > (uint) byte.MaxValue)
        num7 = (uint) byte.MaxValue;
      if (num8 > (uint) byte.MaxValue)
        num8 = (uint) byte.MaxValue;
      if (num10 > (uint) byte.MaxValue)
        num10 = (uint) byte.MaxValue;
      Color color;
      color.PackedValue = (uint) ((int) num6 | (int) num7 << 8 | (int) num8 << 16 | (int) num10 << 24);
      return color;
    }

    public override string ToString() => string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{{R:{0} G:{1} B:{2} A:{3}}}", (object) this.R, (object) this.G, (object) this.B, (object) this.A);

    public override int GetHashCode() => this.PackedValue.GetHashCode();

    public override bool Equals(object obj) => obj is Color other && this.Equals(other);

    public bool Equals(Color other) => this.PackedValue.Equals(other.PackedValue);

    public static implicit operator Color(Vector3 v) => new Color(v.X, v.Y, v.Z, 1f);

    public static implicit operator Vector3(Color v) => v.ToVector3();

    public static implicit operator Color(Vector4 v) => new Color(v.X, v.Y, v.Z, v.W);

    public static implicit operator Vector4(Color v) => v.ToVector4();

    uint IPackedVector<uint>.PackedValue
    {
      get => this.PackedValue;
      set => this.PackedValue = value;
    }

    public static Color Lighten(Color inColor, double inAmount) => new Color((int) Math.Min((double) byte.MaxValue, (double) inColor.R + (double) byte.MaxValue * inAmount), (int) Math.Min((double) byte.MaxValue, (double) inColor.G + (double) byte.MaxValue * inAmount), (int) Math.Min((double) byte.MaxValue, (double) inColor.B + (double) byte.MaxValue * inAmount), (int) inColor.A);

    public static Color Darken(Color inColor, double inAmount) => new Color((int) Math.Max(0.0, (double) inColor.R - (double) byte.MaxValue * inAmount), (int) Math.Max(0.0, (double) inColor.G - (double) byte.MaxValue * inAmount), (int) Math.Max(0.0, (double) inColor.B - (double) byte.MaxValue * inAmount), (int) inColor.A);

    public Color ToGray()
    {
      Vector4 vector4 = (Vector4) this;
      double num = 0.298900008201599 * (double) vector4.X + 0.587000012397766 * (double) vector4.Y + 57.0 / 500.0 * (double) vector4.Z;
      return new Color((float) num, (float) num, (float) num, vector4.W);
    }

    protected class VRageMath_Color\u003C\u003EPackedValue\u003C\u003EAccessor : IMemberAccessor<Color, uint>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Color owner, in uint value) => owner.PackedValue = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Color owner, out uint value) => value = owner.PackedValue;
    }

    protected class VRageMath_Color\u003C\u003EX\u003C\u003EAccessor : IMemberAccessor<Color, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Color owner, in byte value) => owner.X = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Color owner, out byte value) => value = owner.X;
    }

    protected class VRageMath_Color\u003C\u003EY\u003C\u003EAccessor : IMemberAccessor<Color, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Color owner, in byte value) => owner.Y = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Color owner, out byte value) => value = owner.Y;
    }

    protected class VRageMath_Color\u003C\u003EZ\u003C\u003EAccessor : IMemberAccessor<Color, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Color owner, in byte value) => owner.Z = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Color owner, out byte value) => value = owner.Z;
    }

    protected class VRageMath_Color\u003C\u003ER\u003C\u003EAccessor : IMemberAccessor<Color, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Color owner, in byte value) => owner.R = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Color owner, out byte value) => value = owner.R;
    }

    protected class VRageMath_Color\u003C\u003EG\u003C\u003EAccessor : IMemberAccessor<Color, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Color owner, in byte value) => owner.G = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Color owner, out byte value) => value = owner.G;
    }

    protected class VRageMath_Color\u003C\u003EB\u003C\u003EAccessor : IMemberAccessor<Color, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Color owner, in byte value) => owner.B = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Color owner, out byte value) => value = owner.B;
    }

    protected class VRageMath_Color\u003C\u003EA\u003C\u003EAccessor : IMemberAccessor<Color, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Color owner, in byte value) => owner.A = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Color owner, out byte value) => value = owner.A;
    }

    protected class VRageMath_Color\u003C\u003EVRageMath\u002EPackedVector\u002EIPackedVector\u003CSystem\u002EUInt32\u003E\u002EPackedValue\u003C\u003EAccessor : IMemberAccessor<Color, uint>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Color owner, in uint value) => owner.VRageMath\u002EPackedVector\u002EIPackedVector\u003CSystem\u002EUInt32\u003E\u002EPackedValue = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Color owner, out uint value) => value = owner.VRageMath\u002EPackedVector\u002EIPackedVector\u003CSystem\u002EUInt32\u003E\u002EPackedValue;
    }
  }
}
