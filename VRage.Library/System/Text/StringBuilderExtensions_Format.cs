// Decompiled with JetBrains decompiler
// Type: System.Text.StringBuilderExtensions_Format
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Globalization;

namespace System.Text
{
  public static class StringBuilderExtensions_Format
  {
    public static StringBuilder AppendStringBuilder(
      this StringBuilder stringBuilder,
      StringBuilder otherStringBuilder)
    {
      stringBuilder.EnsureCapacity(stringBuilder.Length + otherStringBuilder.Length);
      for (int index = 0; index < otherStringBuilder.Length; ++index)
        stringBuilder.Append(otherStringBuilder[index]);
      return stringBuilder;
    }

    public static StringBuilder AppendSubstring(
      this StringBuilder stringBuilder,
      StringBuilder append,
      int start,
      int count)
    {
      stringBuilder.EnsureCapacity(stringBuilder.Length + count);
      for (int index = 0; index < count; ++index)
        stringBuilder.Append(append[start + index]);
      return stringBuilder;
    }

    public static StringBuilder ConcatFormat<A>(
      this StringBuilder string_builder,
      string format_string,
      A arg1,
      NumberFormatInfo numberFormat = null)
      where A : IConvertible
    {
      return string_builder.ConcatFormat<A, int, int, int, int>(format_string, arg1, 0, 0, 0, 0, numberFormat);
    }

    public static StringBuilder ConcatFormat<A, B>(
      this StringBuilder string_builder,
      string format_string,
      A arg1,
      B arg2,
      NumberFormatInfo numberFormat = null)
      where A : IConvertible
      where B : IConvertible
    {
      return string_builder.ConcatFormat<A, B, int, int, int>(format_string, arg1, arg2, 0, 0, 0, numberFormat);
    }

    public static StringBuilder ConcatFormat<A, B, C>(
      this StringBuilder string_builder,
      string format_string,
      A arg1,
      B arg2,
      C arg3,
      NumberFormatInfo numberFormat = null)
      where A : IConvertible
      where B : IConvertible
      where C : IConvertible
    {
      return string_builder.ConcatFormat<A, B, C, int, int>(format_string, arg1, arg2, arg3, 0, 0, numberFormat);
    }

    public static StringBuilder ConcatFormat<A, B, C, D>(
      this StringBuilder string_builder,
      string format_string,
      A arg1,
      B arg2,
      C arg3,
      D arg4,
      NumberFormatInfo numberFormat = null)
      where A : IConvertible
      where B : IConvertible
      where C : IConvertible
      where D : IConvertible
    {
      return string_builder.ConcatFormat<A, B, C, D, int>(format_string, arg1, arg2, arg3, arg4, 0, numberFormat);
    }

    public static StringBuilder ConcatFormat<A, B, C, D, E>(
      this StringBuilder string_builder,
      string format_string,
      A arg1,
      B arg2,
      C arg3,
      D arg4,
      E arg5,
      NumberFormatInfo numberFormat = null)
      where A : IConvertible
      where B : IConvertible
      where C : IConvertible
      where D : IConvertible
      where E : IConvertible
    {
      int startIndex = 0;
      numberFormat = numberFormat ?? CultureInfo.InvariantCulture.NumberFormat;
      for (int index1 = 0; index1 < format_string.Length; ++index1)
      {
        if (format_string[index1] == '{')
        {
          if (startIndex < index1)
            string_builder.Append(format_string, startIndex, index1 - startIndex);
          uint base_value = 10;
          uint padding = 0;
          uint decimal_places = (uint) numberFormat.NumberDecimalDigits;
          bool thousandSeparation = !string.IsNullOrEmpty(numberFormat.NumberGroupSeparator);
          int index2 = index1 + 1;
          char ch = format_string[index2];
          if (ch == '{')
          {
            string_builder.Append('{');
            index1 = index2 + 1;
          }
          else
          {
            index1 = index2 + 1;
            if (format_string[index1] == ':')
            {
              ++index1;
              while (format_string[index1] == '0')
              {
                ++index1;
                ++padding;
              }
              if (format_string[index1] == 'X')
              {
                ++index1;
                base_value = 16U;
                if (format_string[index1] >= '0' && format_string[index1] <= '9')
                {
                  padding = (uint) format_string[index1] - 48U;
                  ++index1;
                }
              }
              else if (format_string[index1] == '.')
              {
                ++index1;
                decimal_places = 0U;
                while (format_string[index1] == '0')
                {
                  ++index1;
                  ++decimal_places;
                }
              }
            }
            while (format_string[index1] != '}')
              ++index1;
            switch (ch)
            {
              case '0':
                string_builder.ConcatFormatValue<A>(arg1, padding, base_value, decimal_places, thousandSeparation);
                break;
              case '1':
                string_builder.ConcatFormatValue<B>(arg2, padding, base_value, decimal_places, thousandSeparation);
                break;
              case '2':
                string_builder.ConcatFormatValue<C>(arg3, padding, base_value, decimal_places, thousandSeparation);
                break;
              case '3':
                string_builder.ConcatFormatValue<D>(arg4, padding, base_value, decimal_places, thousandSeparation);
                break;
              case '4':
                string_builder.ConcatFormatValue<E>(arg5, padding, base_value, decimal_places, thousandSeparation);
                break;
            }
          }
          startIndex = index1 + 1;
        }
      }
      if (startIndex < format_string.Length)
        string_builder.Append(format_string, startIndex, format_string.Length - startIndex);
      return string_builder;
    }

    private static void ConcatFormatValue<T>(
      this StringBuilder string_builder,
      T arg,
      uint padding,
      uint base_value,
      uint decimal_places,
      bool thousandSeparation)
      where T : IConvertible
    {
      switch (arg.GetTypeCode())
      {
        case TypeCode.Boolean:
          if (arg.ToBoolean((IFormatProvider) CultureInfo.InvariantCulture))
          {
            string_builder.Append("true");
            break;
          }
          string_builder.Append("false");
          break;
        case TypeCode.Int32:
          string_builder.Concat(arg.ToInt32((IFormatProvider) NumberFormatInfo.InvariantInfo), padding, '0', base_value, thousandSeparation);
          break;
        case TypeCode.UInt32:
          string_builder.Concat((long) arg.ToUInt32((IFormatProvider) NumberFormatInfo.InvariantInfo), padding, '0', base_value, thousandSeparation);
          break;
        case TypeCode.Int64:
          string_builder.Concat(arg.ToInt64((IFormatProvider) NumberFormatInfo.InvariantInfo), padding, '0', base_value, thousandSeparation);
          break;
        case TypeCode.UInt64:
          string_builder.Concat(arg.ToInt32((IFormatProvider) NumberFormatInfo.InvariantInfo), padding, '0', base_value, thousandSeparation);
          break;
        case TypeCode.Single:
          string_builder.Concat(arg.ToSingle((IFormatProvider) NumberFormatInfo.InvariantInfo), decimal_places, padding, '0', false);
          break;
        case TypeCode.Double:
          string_builder.Concat(arg.ToDouble((IFormatProvider) NumberFormatInfo.InvariantInfo), decimal_places, padding, '0', false);
          break;
        case TypeCode.Decimal:
          string_builder.Concat(arg.ToSingle((IFormatProvider) NumberFormatInfo.InvariantInfo), decimal_places, padding, '0', false);
          break;
        case TypeCode.String:
          string_builder.Append(arg.ToString());
          break;
      }
    }

    public static StringBuilder ToUpper(this StringBuilder self)
    {
      for (int index = 0; index < self.Length; ++index)
        self[index] = char.ToUpper(self[index]);
      return self;
    }

    public static StringBuilder ToLower(this StringBuilder self)
    {
      for (int index = 0; index < self.Length; ++index)
        self[index] = char.ToLower(self[index]);
      return self;
    }

    public static StringBuilder FirstLetterUpperCase(this StringBuilder self)
    {
      if (self.Length > 0)
        self[0] = char.ToUpper(self[0]);
      return self;
    }
  }
}
