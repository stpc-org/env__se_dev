// Decompiled with JetBrains decompiler
// Type: System.Text.StringBuilderExtensions
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Globalization;

namespace System.Text
{
  public static class StringBuilderExtensions
  {
    private static readonly char[] ms_digits = new char[16]
    {
      '0',
      '1',
      '2',
      '3',
      '4',
      '5',
      '6',
      '7',
      '8',
      '9',
      'A',
      'B',
      'C',
      'D',
      'E',
      'F'
    };
    private static readonly uint ms_default_decimal_places = 5;
    private static readonly char ms_default_pad_char = '0';

    public static StringBuilder Concat(
      this StringBuilder string_builder,
      ulong uint_val,
      uint pad_amount,
      char pad_char,
      uint base_val,
      bool thousandSeparation)
    {
      uint val2 = 0;
      ulong num1 = uint_val;
      int num2 = 0;
      do
      {
        ++num2;
        if (thousandSeparation && num2 % 4 == 0)
        {
          ++val2;
        }
        else
        {
          num1 /= (ulong) base_val;
          ++val2;
        }
      }
      while (num1 > 0UL);
      string_builder.Append(pad_char, (int) Math.Max(pad_amount, val2));
      int length = string_builder.Length;
      int num3 = 0;
      while (val2 > 0U)
      {
        --length;
        ++num3;
        if (thousandSeparation && num3 % 4 == 0)
        {
          --val2;
          string_builder[length] = NumberFormatInfo.InvariantInfo.NumberGroupSeparator[0];
        }
        else
        {
          string_builder[length] = StringBuilderExtensions.ms_digits[uint_val % (ulong) base_val];
          uint_val /= (ulong) base_val;
          --val2;
        }
      }
      return string_builder;
    }

    public static StringBuilder Concat(
      this StringBuilder string_builder,
      uint uint_val)
    {
      string_builder.Concat((long) uint_val, 0U, StringBuilderExtensions.ms_default_pad_char, 10U, true);
      return string_builder;
    }

    public static StringBuilder Concat(
      this StringBuilder string_builder,
      uint uint_val,
      uint pad_amount)
    {
      string_builder.Concat((long) uint_val, pad_amount, StringBuilderExtensions.ms_default_pad_char, 10U, true);
      return string_builder;
    }

    public static StringBuilder Concat(
      this StringBuilder string_builder,
      uint uint_val,
      uint pad_amount,
      char pad_char)
    {
      string_builder.Concat((long) uint_val, pad_amount, pad_char, 10U, true);
      return string_builder;
    }

    public static StringBuilder Concat(
      this StringBuilder string_builder,
      int int_val,
      uint pad_amount,
      char pad_char,
      uint base_val,
      bool thousandSeparation)
    {
      if (int_val < 0)
      {
        string_builder.Append('-');
        uint num = (uint) (-1 - int_val + 1);
        string_builder.Concat((long) num, pad_amount, pad_char, base_val, thousandSeparation);
      }
      else
        string_builder.Concat((long) (uint) int_val, pad_amount, pad_char, base_val, thousandSeparation);
      return string_builder;
    }

    public static StringBuilder Concat(
      this StringBuilder string_builder,
      long long_val,
      uint pad_amount,
      char pad_char,
      uint base_val,
      bool thousandSeparation)
    {
      if (long_val < 0L)
      {
        string_builder.Append('-');
        ulong uint_val = (ulong) (-1L - long_val) + 1UL;
        string_builder.Concat(uint_val, pad_amount, pad_char, base_val, thousandSeparation);
      }
      else
        string_builder.Concat((ulong) long_val, pad_amount, pad_char, base_val, thousandSeparation);
      return string_builder;
    }

    public static StringBuilder Concat(this StringBuilder string_builder, int int_val)
    {
      string_builder.Concat(int_val, 0U, StringBuilderExtensions.ms_default_pad_char, 10U, true);
      return string_builder;
    }

    public static StringBuilder Concat(
      this StringBuilder string_builder,
      int int_val,
      uint pad_amount)
    {
      string_builder.Concat(int_val, pad_amount, StringBuilderExtensions.ms_default_pad_char, 10U, true);
      return string_builder;
    }

    public static StringBuilder Concat(
      this StringBuilder string_builder,
      int int_val,
      uint pad_amount,
      char pad_char)
    {
      string_builder.Concat(int_val, pad_amount, pad_char, 10U, true);
      return string_builder;
    }

    public static StringBuilder Concat(
      this StringBuilder string_builder,
      float float_val,
      uint decimal_places,
      uint pad_amount,
      char pad_char,
      bool thousandSeparator)
    {
      if (decimal_places == 0U)
      {
        long long_val = (double) float_val < 0.0 ? (long) ((double) float_val - 0.5) : (long) ((double) float_val + 0.5);
        string_builder.Concat(long_val, pad_amount, pad_char, 10U, thousandSeparator);
      }
      else
      {
        float num1 = 0.5f;
        for (int index = 0; (long) index < (long) decimal_places; ++index)
          num1 *= 0.1f;
        float_val += (double) float_val >= 0.0 ? num1 : -num1;
        long long_val = (long) float_val;
        if (long_val == 0L && (double) float_val < 0.0)
          string_builder.Append('-');
        string_builder.Concat(long_val, pad_amount, pad_char, 10U, thousandSeparator);
        string_builder.Append('.');
        float num2 = Math.Abs(float_val - (float) long_val);
        uint num3 = decimal_places;
        do
        {
          num2 *= 10f;
          --num3;
        }
        while (num3 > 0U);
        string_builder.Concat((long) (uint) num2, decimal_places, '0', 10U, thousandSeparator);
      }
      return string_builder;
    }

    public static StringBuilder Concat(
      this StringBuilder string_builder,
      float float_val)
    {
      string_builder.Concat(float_val, StringBuilderExtensions.ms_default_decimal_places, 0U, StringBuilderExtensions.ms_default_pad_char, false);
      return string_builder;
    }

    public static StringBuilder Concat(
      this StringBuilder string_builder,
      float float_val,
      uint decimal_places)
    {
      string_builder.Concat(float_val, decimal_places, 0U, StringBuilderExtensions.ms_default_pad_char, false);
      return string_builder;
    }

    public static StringBuilder Concat(
      this StringBuilder string_builder,
      float float_val,
      uint decimal_places,
      uint pad_amount)
    {
      string_builder.Concat(float_val, decimal_places, pad_amount, StringBuilderExtensions.ms_default_pad_char, false);
      return string_builder;
    }

    public static StringBuilder Concat(
      this StringBuilder string_builder,
      double double_val,
      uint decimal_places,
      uint pad_amount,
      char pad_char,
      bool thousandSeparator)
    {
      if (decimal_places == 0U)
      {
        long long_val = double_val < 0.0 ? (long) (double_val - 0.5) : (long) (double_val + 0.5);
        string_builder.Concat(long_val, pad_amount, pad_char, 10U, thousandSeparator);
      }
      else
      {
        double num1 = 0.5;
        for (int index = 0; (long) index < (long) decimal_places; ++index)
          num1 *= 0.100000001490116;
        double_val += double_val >= 0.0 ? num1 : -num1;
        long long_val = (long) double_val;
        if (long_val == 0L && double_val < 0.0)
          string_builder.Append('-');
        string_builder.Concat(long_val, pad_amount, pad_char, 10U, thousandSeparator);
        string_builder.Append('.');
        double num2 = Math.Abs(double_val - (double) long_val);
        uint num3 = decimal_places;
        do
        {
          num2 *= 10.0;
          --num3;
        }
        while (num3 > 0U);
        string_builder.Concat((long) (uint) num2, decimal_places, '0', 10U, thousandSeparator);
      }
      return string_builder;
    }

    public static StringBuilder Concat(
      this StringBuilder string_builder,
      double double_val)
    {
      string_builder.Concat(double_val, StringBuilderExtensions.ms_default_decimal_places, 0U, StringBuilderExtensions.ms_default_pad_char, false);
      return string_builder;
    }

    public static StringBuilder Concat(
      this StringBuilder string_builder,
      double double_val,
      uint decimal_places)
    {
      string_builder.Concat(double_val, decimal_places, 0U, StringBuilderExtensions.ms_default_pad_char, false);
      return string_builder;
    }

    public static StringBuilder Concat(
      this StringBuilder string_builder,
      double double_val,
      uint decimal_places,
      uint pad_amount)
    {
      string_builder.Concat(double_val, decimal_places, pad_amount, StringBuilderExtensions.ms_default_pad_char, false);
      return string_builder;
    }
  }
}
