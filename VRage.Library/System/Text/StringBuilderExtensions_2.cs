// Decompiled with JetBrains decompiler
// Type: System.Text.StringBuilderExtensions_2
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections.Generic;
using System.Globalization;
using VRage;

namespace System.Text
{
  public static class StringBuilderExtensions_2
  {
    private static NumberFormatInfo m_numberFormatInfoHelper;

    static StringBuilderExtensions_2()
    {
      if (StringBuilderExtensions_2.m_numberFormatInfoHelper != null)
        return;
      StringBuilderExtensions_2.m_numberFormatInfoHelper = CultureInfo.InvariantCulture.NumberFormat.Clone() as NumberFormatInfo;
    }

    public static bool CompareUpdate(this StringBuilder sb, string text)
    {
      if (sb.CompareTo(text) == 0)
        return false;
      sb.Clear();
      sb.Append(text);
      return true;
    }

    public static bool CompareUpdate(this StringBuilder sb, StringBuilder text)
    {
      if (sb.CompareTo(text) == 0)
        return false;
      sb.Clear();
      sb.AppendStringBuilder(text);
      return true;
    }

    public static void TrimEnd(this StringBuilder sb, int length)
    {
      Exceptions.ThrowIf<ArgumentException>(length > sb.Length, "String builder contains less characters then requested number!");
      sb.Length -= length;
    }

    public static StringBuilder GetFormatedLong(
      this StringBuilder sb,
      string before,
      long value,
      string after = "")
    {
      sb.Clear();
      sb.ConcatFormat<string, long, string>("{0}{1: #,0}{2}", before, value, after);
      return sb;
    }

    public static StringBuilder GetFormatedInt(
      this StringBuilder sb,
      string before,
      int value,
      string after = "")
    {
      sb.Clear();
      sb.ConcatFormat<string, int, string>("{0}{1: #,0}{2}", before, value, after);
      return sb;
    }

    public static StringBuilder GetFormatedFloat(
      this StringBuilder sb,
      string before,
      float value,
      string after = "")
    {
      sb.Clear();
      sb.ConcatFormat<string, float, string>("{0}{1: #,0}{2}", before, value, after);
      return sb;
    }

    public static StringBuilder GetFormatedBool(
      this StringBuilder sb,
      string before,
      bool value,
      string after = "")
    {
      sb.Clear();
      sb.ConcatFormat<string, bool, string>("{0}{1}{2}", before, value, after);
      return sb;
    }

    public static StringBuilder GetFormatedDateTimeOffset(
      this StringBuilder sb,
      string before,
      DateTimeOffset value,
      string after = "")
    {
      return sb.GetFormatedDateTimeOffset(before, value.DateTime, after);
    }

    public static StringBuilder GetFormatedDateTimeOffset(
      this StringBuilder sb,
      string before,
      DateTime value,
      string after = "")
    {
      sb.Clear();
      sb.Append(before);
      sb.Concat(value.Year, 4U, '0', 10U, false);
      sb.Append("-");
      sb.Concat(value.Month, 2U);
      sb.Append("-");
      sb.Concat(value.Day, 2U);
      sb.Append(" ");
      sb.Concat(value.Hour, 2U);
      sb.Append(":");
      sb.Concat(value.Minute, 2U);
      sb.Append(":");
      sb.Concat(value.Second, 2U);
      sb.Append(".");
      sb.Concat(value.Millisecond, 3U);
      sb.Append(after);
      return sb;
    }

    public static StringBuilder GetFormatedDateTime(
      this StringBuilder sb,
      DateTime value)
    {
      sb.Clear();
      sb.Concat(value.Day, 2U);
      sb.Append("/");
      sb.Concat(value.Month, 2U);
      sb.Append("/");
      sb.Concat(value.Year, 0U, '0', 10U, false);
      sb.Append(" ");
      sb.Concat(value.Hour, 2U);
      sb.Append(":");
      sb.Concat(value.Minute, 2U);
      sb.Append(":");
      sb.Concat(value.Second, 2U);
      return sb;
    }

    public static StringBuilder GetFormatedDateTimeForFilename(
      this StringBuilder sb,
      DateTime value,
      bool includeMS = false)
    {
      sb.Clear();
      sb.Concat(value.Year, 0U, '0', 10U, false);
      sb.Concat(value.Month, 2U);
      sb.Concat(value.Day, 2U);
      sb.Append("_");
      sb.Concat(value.Hour, 2U);
      sb.Concat(value.Minute, 2U);
      sb.Concat(value.Second, 2U);
      if (includeMS)
        sb.Concat(value.Millisecond, 2U);
      return sb;
    }

    public static StringBuilder AppendFormatedDateTime(
      this StringBuilder sb,
      DateTime value)
    {
      sb.Concat(value.Day, 2U);
      sb.Append("/");
      sb.Concat(value.Month, 2U);
      sb.Append("/");
      sb.Concat(value.Year, 0U, '0', 10U, false);
      sb.Append(" ");
      sb.Concat(value.Hour, 2U);
      sb.Append(":");
      sb.Concat(value.Minute, 2U);
      sb.Append(":");
      sb.Concat(value.Second, 2U);
      return sb;
    }

    public static StringBuilder GetFormatedTimeSpan(
      this StringBuilder sb,
      string before,
      TimeSpan value,
      string after = "")
    {
      sb.Clear();
      sb.Clear();
      sb.Append(before);
      sb.Concat(value.Hours, 2U);
      sb.Append(":");
      sb.Concat(value.Minutes, 2U);
      sb.Append(":");
      sb.Concat(value.Seconds, 2U);
      sb.Append(".");
      sb.Concat(value.Milliseconds, 3U);
      sb.Append(after);
      return sb;
    }

    public static StringBuilder GetStrings(
      this StringBuilder sb,
      string before,
      string value = "",
      string after = "")
    {
      sb.Clear();
      sb.ConcatFormat<string, string, string>("{0}{1}{2}", before, value, after);
      return sb;
    }

    public static StringBuilder AppendFormatedDecimal(
      this StringBuilder sb,
      string before,
      float value,
      int decimalDigits,
      string after = "")
    {
      sb.Clear();
      StringBuilderExtensions_2.m_numberFormatInfoHelper.NumberDecimalDigits = decimalDigits;
      sb.ConcatFormat<string, float, string>("{0}{1 }{2}", before, value, after, StringBuilderExtensions_2.m_numberFormatInfoHelper);
      return sb;
    }

    public static StringBuilder AppendInt64(this StringBuilder sb, long number)
    {
      sb.ConcatFormat<long>("{0}", number);
      return sb;
    }

    public static StringBuilder AppendInt32(this StringBuilder sb, int number)
    {
      sb.ConcatFormat<int>("{0}", number);
      return sb;
    }

    public static int GetDecimalCount(float number, int validDigitCount)
    {
      int num;
      for (num = validDigitCount; (double) number >= 1.0 && num > 0; --num)
        number /= 10f;
      return num;
    }

    public static int GetDecimalCount(double number, int validDigitCount)
    {
      int num;
      for (num = validDigitCount; number >= 1.0 && num > 0; --num)
        number /= 10.0;
      return num;
    }

    public static int GetDecimalCount(Decimal number, int validDigitCount)
    {
      int num;
      for (num = validDigitCount; number >= 1M && num > 0; --num)
        number /= 10M;
      return num;
    }

    public static StringBuilder AppendDecimalDigit(
      this StringBuilder sb,
      float number,
      int validDigitCount)
    {
      return sb.AppendDecimal(number, StringBuilderExtensions_2.GetDecimalCount(number, validDigitCount));
    }

    public static StringBuilder AppendDecimalDigit(
      this StringBuilder sb,
      double number,
      int validDigitCount)
    {
      return sb.AppendDecimal(number, StringBuilderExtensions_2.GetDecimalCount(number, validDigitCount));
    }

    public static StringBuilder AppendDecimal(
      this StringBuilder sb,
      float number,
      int decimals)
    {
      StringBuilderExtensions_2.m_numberFormatInfoHelper.NumberDecimalDigits = Math.Max(0, Math.Min(decimals, 99));
      sb.ConcatFormat<float>("{0}", number, StringBuilderExtensions_2.m_numberFormatInfoHelper);
      return sb;
    }

    public static StringBuilder AppendDecimal(
      this StringBuilder sb,
      double number,
      int decimals)
    {
      StringBuilderExtensions_2.m_numberFormatInfoHelper.NumberDecimalDigits = Math.Max(0, Math.Min(decimals, 99));
      sb.ConcatFormat<double>("{0}", number, StringBuilderExtensions_2.m_numberFormatInfoHelper);
      return sb;
    }

    public static List<StringBuilder> Split(this StringBuilder sb, char separator)
    {
      List<StringBuilder> stringBuilderList = new List<StringBuilder>();
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < sb.Length; ++index)
      {
        if ((int) sb[index] == (int) separator)
        {
          stringBuilderList.Add(stringBuilder);
          stringBuilder = new StringBuilder();
        }
        else
          stringBuilder.Append(sb[index]);
      }
      if (stringBuilder.Length > 0)
        stringBuilderList.Add(stringBuilder);
      return stringBuilderList;
    }

    public static StringBuilder TrimTrailingWhitespace(this StringBuilder sb)
    {
      int length = sb.Length;
      while (length > 0 && (sb[length - 1] == ' ' || sb[length - 1] == '\r' || sb[length - 1] == '\n'))
        --length;
      sb.Length = length;
      return sb;
    }

    public static int CompareTo(this StringBuilder self, StringBuilder other)
    {
      int index = 0;
      int num;
      while (true)
      {
        bool flag1 = index < self.Length;
        bool flag2 = index < other.Length;
        if (flag1 | flag2)
        {
          if (flag1)
          {
            if (flag2)
            {
              num = self[index].CompareTo(other[index]);
              if (num == 0)
                ++index;
              else
                goto label_8;
            }
            else
              goto label_6;
          }
          else
            goto label_4;
        }
        else
          break;
      }
      return 0;
label_4:
      return -1;
label_6:
      return 1;
label_8:
      return num;
    }

    public static int CompareTo(this StringBuilder self, string other)
    {
      int index = 0;
      int num;
      while (true)
      {
        bool flag1 = index < self.Length;
        bool flag2 = index < other.Length;
        if (flag1 | flag2)
        {
          if (flag1)
          {
            if (flag2)
            {
              num = self[index].CompareTo(other[index]);
              if (num == 0)
                ++index;
              else
                goto label_8;
            }
            else
              goto label_6;
          }
          else
            goto label_4;
        }
        else
          break;
      }
      return 0;
label_4:
      return -1;
label_6:
      return 1;
label_8:
      return num;
    }

    public static int CompareToIgnoreCase(this StringBuilder self, StringBuilder other)
    {
      int index = 0;
      int num;
      while (true)
      {
        bool flag1 = index < self.Length;
        bool flag2 = index < other.Length;
        if (flag1 | flag2)
        {
          if (flag1)
          {
            if (flag2)
            {
              num = char.ToLowerInvariant(self[index]).CompareTo(char.ToLowerInvariant(other[index]));
              if (num == 0)
                ++index;
              else
                goto label_8;
            }
            else
              goto label_6;
          }
          else
            goto label_4;
        }
        else
          break;
      }
      return 0;
label_4:
      return -1;
label_6:
      return 1;
label_8:
      return num;
    }
  }
}
